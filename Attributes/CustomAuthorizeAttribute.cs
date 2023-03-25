using SE_No1.Models;
using SE_No1.Services;
using SE_No1.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SE_No1.Attributes
{
    public class CustomAuthorizeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session["UserName"] == null)
            {
                filterContext.Result = new RedirectToRouteResult(
                   new RouteValueDictionary
                   {
                       { "controller", "Error" },
                       { "action", "Error" },
                       { "errorMsg", "請先進行登入" },
                       { "redircetUrl", "/Account/Login" }
                   });
            }
            else
            {
                string actionName = filterContext.ActionDescriptor.ActionName;
                string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
                if (!CommonCodes.checkAuth(controllerName, actionName))
                {
                    filterContext.Result = new RedirectToRouteResult(
                           new RouteValueDictionary
                           {
                                    { "controller", "Error" },
                                    { "action", "Error" },
                                    { "errorMsg", "您沒有權限" }
                           });
                }

            }
        }
    }
}