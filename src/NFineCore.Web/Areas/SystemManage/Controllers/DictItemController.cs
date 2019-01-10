using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NFineCore.Support;
using NFineCore.EntityFramework.Dtos.SystemManage;
using NFineCore.EntityFramework.Models.SystemManage;
using NFineCore.Service.SystemManage;
using NFineCore.Web.Controllers;
using NFineCore.Web.Attributes;

namespace NFineCore.Web.Areas.SystemManage.Controllers
{
    [Area("SystemManage")]
    public class DictItemController : BaseController
    {
        DictService dictService = new DictService();
        DictItemService dictItemService = new DictItemService();

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
        public ActionResult GetGridJson(long dictId, string keyword)
        {
            var data = dictItemService.GetList(dictId, keyword);
            return Content(data.ToJson());
        }
        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = dictItemService.GetForm(keyValue);
            return Content(data.ToJson());
        }

        [HttpPost]
        public ActionResult SubmitForm(DictItemInputDto dictItemInputDto, string keyValue)
        {
            dictItemService.SubmitForm(dictItemInputDto, keyValue);
            return Success("操作成功。");
        }
        [HttpPost]
        public ActionResult DeleteForm(string keyValue)
        {
            dictItemService.DeleteForm(keyValue);
            return Success("操作成功。");
        }
        [HttpGet]
        public ActionResult GetSelectJson(string enCode)
        {
            var data = dictService.GetDictItemList(enCode);
            List<object> list = new List<object>();
            foreach (DictItemGridDto item in data)
            {
                list.Add(new { id = item.ItemCode, text = item.ItemName });
            }
            return Content(list.ToJson());
        }
    }
}