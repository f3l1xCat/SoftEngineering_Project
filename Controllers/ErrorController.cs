using SE_No1.Models;
using SE_No1.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SE_No1.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult Error(string errorMsg, string redirectUrl = "/Account/Login")
        {
            ViewData["Message"] = errorMsg;
            ViewData["RedirectUrl"] = redirectUrl;

            return View();
        }
    }
}