using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SE_No1.Models;
using SE_No1.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SE_No1.Controllers
{
    public class AccountController : Controller
    {
        AccountService service = new AccountService();
        // GET: Account
        public ActionResult Login()
        {
            if (System.Web.HttpContext.Current.Session["UserName"] != null) //已登入重導至首頁
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel loginInfo)
        {
            if (ModelState.IsValid)
            {
                if (service.AuthenticateUser(loginInfo.UserName, loginInfo.ScrStr))
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    return RedirectToAction("Error", "Error", new { errorMsg = "帳號或密碼錯誤" });
                }
            }
            return View();
        }

        public ActionResult Logout()
        {
            System.Web.HttpContext.Current.Session.RemoveAll();
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public ActionResult IsValidPassword(string psw)
        {
            return Content(JsonConvert.SerializeObject(new JObject() { { "isValid", service.IsValidPassword(psw) } }), "application/json");
        }
    }
}