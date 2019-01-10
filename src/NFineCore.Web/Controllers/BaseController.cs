using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NFineCore.Support;

namespace NFineCore.Web.Controllers
{
    public class BaseController : Controller
    {
        public virtual IActionResult Index()
        {
            return View();
        }

        public virtual IActionResult List()
        {
            return View();
        }

        public virtual IActionResult Create()
        {
            return View();
        }

        public virtual IActionResult Edit()
        {
            return View();
        }

        public virtual IActionResult Delete()
        {
            return View();
        }
        public virtual IActionResult Details()
        {
            return View();
        }

        public virtual IActionResult Form()
        {
            return View();
        }

        protected virtual ActionResult Success(string message)
        {
            return Content(new AjaxResult { state = ResultType.success.ToString(), message = message }.ToJson());
        }
        protected virtual ActionResult Success(string message, object data)
        {
            return Content(new AjaxResult { state = ResultType.success.ToString(), message = message, data = data }.ToJson());
        }
        protected virtual ActionResult Error(string message)
        {
            return Content(new AjaxResult { state = ResultType.error.ToString(), message = message }.ToJson());
        }
    }
}