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
    public class DictController : BaseController
    {
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
        DictService dictService = new DictService();
        [HttpGet]
        public ActionResult GetTreeJson()
        {
            var data = dictService.GetList();
            var treeList = new List<TreeViewModel>();
            foreach (DictGridDto dict in data)
            {
                TreeViewModel tree = new TreeViewModel();
                bool hasChildren = data.Count(t => t.ParentId == dict.Id) == 0 ? false : true;
                tree.id = dict.Id.ToString();
                tree.text = dict.FullName;
                tree.value = dict.EnCode;
                tree.parentId = dict.ParentId.ToString();
                tree.isexpand = true;
                tree.complete = true;
                tree.hasChildren = hasChildren;
                treeList.Add(tree);
            }
            return Content(treeList.TreeViewJson());
        }

        [HttpGet]
        public ActionResult GetTreeGridJson()
        {
            var data = dictService.GetList();
            var treeList = new List<TreeGridModel>();
            foreach (DictGridDto dict in data)
            {
                TreeGridModel treeModel = new TreeGridModel();
                bool hasChildren = data.Count(t => t.ParentId == dict.Id) == 0 ? false : true;
                treeModel.id = dict.Id.ToString();
                treeModel.isLeaf = hasChildren;
                treeModel.parentId = dict.ParentId.ToString();
                treeModel.expanded = hasChildren;
                treeModel.entityJson = dict.ToJson();
                treeList.Add(treeModel);
            }
            var strJson = treeList.TreeGridJson();
            return Content(strJson);
        }

        [HttpGet]
        public ActionResult GetTreeSelectJson()
        {
            var data = dictService.GetList();
            var treeList = new List<TreeSelectModel>();
            foreach (DictGridDto dict in data)
            {
                TreeSelectModel treeModel = new TreeSelectModel();
                treeModel.id = dict.Id.ToString();
                treeModel.text = dict.FullName;
                treeModel.parentId = dict.ParentId.ToString();
                treeList.Add(treeModel);
            }
            return Content(treeList.TreeSelectJson());
        }

        [HttpGet]
        public ActionResult GetFormJson(string keyValue)
        {
            var data = dictService.GetForm(keyValue);
            return Content(data.ToJson());
        }

        [HttpPost]
        public ActionResult SubmitForm(DictInputDto dictInputDto, string keyValue)
        {
            dictService.SubmitForm(dictInputDto, keyValue);
            return Success("操作成功。");
        }
    }
}