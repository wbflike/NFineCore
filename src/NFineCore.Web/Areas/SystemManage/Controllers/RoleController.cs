using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NFineCore.Support;
using NFineCore.EntityFramework.Dtos.SystemManage;
using NFineCore.EntityFramework.Models.SystemManage;
using NFineCore.Service.SystemManage;
using NFineCore.Web.Controllers;
using SharpRepository.Repository;
using NFineCore.Web.Attributes;

namespace NFineCore.Web.Areas.SystemManage.Controllers
{
    [Area("SystemManage")]
    public class RoleController : BaseController
    {
        RoleService roleService = new RoleService();

        [PermissionCheck]
        public override IActionResult Index()
        {
            return View();
        }
        [PermissionCheck]
        public override IActionResult Form()
        {
            return View();
        }
        [PermissionCheck]
        public override IActionResult Details()
        {
            return View();
        }
        [HttpGet]
        [PermissionCheck("SystemManage_Role_Index")]
        public ActionResult GetSelectJson(string keyword)
        {
            var data = roleService.GetList(keyword);
            return Content(data.ToJson());
        }

        [HttpGet]
        [PermissionCheck("SystemManage_Role_Index")]
        public ActionResult GetGridJson(Pagination pagination, string keyword)
        {
            var data = new
            {
                rows = roleService.GetList(pagination, keyword),
                total = pagination.total,
                page = pagination.page,
                records = pagination.records
            };
            return Content(data.ToJson());
        }

        [HttpGet]
        [PermissionCheck("SystemManage_Role_Form")]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = roleService.GetForm(keyValue);
            return Content(data.ToJson());
        }

        [HttpPost]
        [PermissionCheck("SystemManage_Role_Form")]
        public ActionResult SubmitForm(RoleInputDto roleInputDto, string resourceIds, string keyValue)
        {
            string[] resourceIdsArray = { };
            if (!string.IsNullOrEmpty(resourceIds))
            {
                resourceIdsArray = resourceIds.Split(',');
            }            
            roleService.SubmitForm(roleInputDto, resourceIdsArray, keyValue);
            return Success("操作成功。");
        }

        [HttpPost]
        [PermissionCheck("SystemManage_Role_Delete")]
        public ActionResult DeleteForm(string keyValue)
        {
            roleService.DeleteForm(keyValue);
            return Success("操作成功。");
        }
    }
}