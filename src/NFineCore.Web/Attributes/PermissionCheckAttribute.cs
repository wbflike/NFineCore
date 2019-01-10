using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using NFineCore.EntityFramework.Dtos.SystemManage;
using NFineCore.Service.SystemManage;
using NFineCore.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NFineCore.Web.Attributes
{
    public class PermissionCheckAttribute:ActionFilterAttribute
    {
        public string PermissionCode { get; set; }
        public PermissionCheckAttribute()
        {

        }
        public PermissionCheckAttribute(string permissionCode)
        {
            PermissionCode = permissionCode;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            string areaName = filterContext.ActionDescriptor.RouteValues["area"];
            string controllerName = filterContext.ActionDescriptor.RouteValues["controller"];
            string actionName = filterContext.ActionDescriptor.RouteValues["action"];

            if (!string.IsNullOrEmpty(areaName))
            {
                OperatorModel operatorModel = OperatorProvider.Provider.GetCurrent();
                PermissionService permissionService = new PermissionService();
                var permissions = permissionService.GetPermsResList(Convert.ToInt64(operatorModel.Id));

                if (string.IsNullOrEmpty(PermissionCode)) {
                    PermissionCode = areaName + "_" + controllerName + "_" + actionName;
                }
               
                List<ResourceGridDto> query = permissions.Where(r => r.PermissionCode == PermissionCode).ToList();
                if (query.Count() > 0)
                {
                    base.OnActionExecuting(filterContext);
                }
                else 
                {
                    filterContext.HttpContext.Response.ContentType = "text/html;charset=utf-8";
                    filterContext.HttpContext.Response.WriteAsync("<script type='text/javascript'>alert('很抱歉，您的权限不足，访问被拒绝， 请联系系统管理员！');</script>");
                    return;
                }                
            }
        }

        //public override void OnActionExecuted(ActionExecutedContext filterContext)
        //{
        //    base.OnActionExecuted(filterContext);
        //    filterContext.HttpContext.Response.WriteAsync("<br />" + "执行OnActionExecuted：" + Message + "<br />");
        //}

        //public override void OnResultExecuting(ResultExecutingContext filterContext)
        //{
        //    base.OnResultExecuting(filterContext);
        //    filterContext.HttpContext.Response.WriteAsync("<br />" + "执行OnResultExecuting：" + Message + "<br />");
        //}

        //public override void OnResultExecuted(ResultExecutedContext filterContext)
        //{
        //    base.OnResultExecuted(filterContext);
        //    filterContext.HttpContext.Response.WriteAsync("<br />" + "执行OnResultExecuted：" + Message + "<br />");
        //}
    }
}
