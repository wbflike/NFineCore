using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace NFineCore.Web.Areas.ExampleManage.Controllers
{

    [Area("ExampleManage")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult SearchResults()
        {
            return View();
        }
        public IActionResult ForumMain()
        {
            return View();
        }
        public IActionResult Clients()
        {
            return View();
        }
    }
}