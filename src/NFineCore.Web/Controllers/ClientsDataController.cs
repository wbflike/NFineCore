using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NFineCore.Support;
using NFineCore.EntityFramework.Dtos.SystemManage;
using NFineCore.EntityFramework.Models.SystemManage;
using NFineCore.Service.SystemManage;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace NFineCore.Web.Controllers
{
    public class ClientsDataController : Controller
    {
        [HttpGet]
        public IActionResult GetClientsDataJson()
        {
            OperatorModel operatorModel = OperatorProvider.Provider.GetCurrent();
            PermissionService permissionService = new PermissionService();
            var permissions = permissionService.GetPermsResList(Convert.ToInt64(operatorModel.Id));

            var data = new
            {
                dataItems = this.GetDictItemList(),
                organize = this.GetOrganizeList(),
                role = this.GetRoleList(),
                duty = this.GetDutyList(),
                currentUser = this.GetUserForm(operatorModel.Id),
                authorizeMenu = this.GetMenuList(permissions),
                authorizeButton = this.GetMenuButtonList(permissions),
                wxMenu = this.GetWxMenuList()
            };
            return Content(data.ToJson());
        }

        private object GetDictItemList()
        {
            var itemdata = new DictItemService().GetList();
            Dictionary<string, object> dictionaryItem = new Dictionary<string, object>();
            foreach (var item in new DictService().GetList())
            {
                var dataItemList = itemdata.FindAll(t => t.DictId.Equals(item.Id));
                Dictionary<string, string> dictionaryItemList = new Dictionary<string, string>();
                foreach (var itemList in dataItemList)
                {
                    dictionaryItemList.Add(itemList.ItemCode, itemList.ItemName);
                }
                dictionaryItem.Add(item.EnCode, dictionaryItemList);
            }
            return dictionaryItem;
        }

        private object GetOrganizeList()
        {
            OrganizeService organizeService = new OrganizeService();
            var data = organizeService.GetList();
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            foreach (OrganizeGridDto item in data)
            {
                var fieldItem = new
                {
                    encode = item.EnCode,
                    fullname = item.FullName
                };
                dictionary.Add(item.Id.ToString(), fieldItem);
            }
            return dictionary;
        }

        private object GetRoleList()
        {
            RoleService roleService = new RoleService();
            var data = roleService.GetList();
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            foreach (RoleGridDto item in data)
            {
                var fieldItem = new
                {
                    encode = item.EnCode,
                    fullname = item.FullName
                };
                dictionary.Add(item.Id, fieldItem);
            }
            return dictionary;
        }

        private object GetDutyList()
        {
            DutyService dutyService = new DutyService();
            var data = dutyService.GetList();
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            foreach (DutyGridDto item in data)
            {
                var fieldItem = new
                {
                    encode = item.EnCode,
                    fullname = item.FullName
                };
                dictionary.Add(item.Id, fieldItem);
            }
            return dictionary;
        }

        private object GetMenuList(List<ResourceGridDto> resources)
        {
            resources = resources.Where(a => a.ObjectType == "Menu").ToList();
            return ToMenuJson(resources, "0");
        }

        private object GetWxMenuList()
        {
            ResourceService resourceService = new ResourceService();
            var wxMenus = resourceService.GetWxMenuList();
            return ToMenuJson(wxMenus, "0");
        }

        private string ToMenuJson(List<ResourceGridDto> data, string parentId)
        {
            StringBuilder sbJson = new StringBuilder();
            sbJson.Append("[");
            List<ResourceGridDto> entitys = data.FindAll(t => t.ParentId.ToString() == parentId);
            if (entitys.Count > 0)
            {
                foreach (var item in entitys)
                {
                    string strJson = item.ToJson();
                    strJson = strJson.Insert(strJson.Length - 1, ",\"ChildNodes\":" + ToMenuJson(data, item.Id.ToString()) + "");
                    sbJson.Append(strJson + ",");
                }
                sbJson = sbJson.Remove(sbJson.Length - 1, 1);
            }
            sbJson.Append("]");
            return sbJson.ToString();
        }

        private object GetMenuButtonList(List<ResourceGridDto> resources)
        {
            resources = resources.Where(a => a.ObjectType == "Button").ToList();
            var dataModuleId = resources.Distinct(new ExtList<ResourceGridDto>("ParentId"));
            Dictionary<string, object> dictionary = new Dictionary<string, object>();
            foreach (ResourceGridDto item in dataModuleId)
            {
                var buttonList = resources.Where(t => t.ParentId.Equals(item.ParentId));
                dictionary.Add(item.ParentId, buttonList);
            }
            return dictionary;
        }

        private object GetUserForm(string keyValue)
        {
            UserService userService = new UserService();
            var data = userService.GetForm(keyValue);
            return data;
        }
    }
}