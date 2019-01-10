using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NFineCore.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NFineCore.Web.Filters
{
    public class LoginAuthFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext filterContext)
        {
            string areaName = filterContext.ActionDescriptor.RouteValues["area"];
            string controllerName = filterContext.ActionDescriptor.RouteValues["controller"];
            string actionName = filterContext.ActionDescriptor.RouteValues["action"];
            if (areaName == null)
            {
                if (controllerName == "Account" && actionName == "Login") return;
                if (controllerName == "Account" && actionName == "GetAuthCode") return;
                if (controllerName == "Account" && actionName == "CheckLogin") return;
            }

            if (OperatorProvider.Provider.GetCurrent() == null)
            {
                filterContext.Result = new RedirectResult("/Account/Login");
            }
        }
    }
}
