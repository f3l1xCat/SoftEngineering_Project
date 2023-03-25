using Newtonsoft.Json;
using SE_No1.Attributes;
using SE_No1.Models;
using SE_No1.Models.ViewModels;
using SE_No1.Services;
using SE_No1.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Web;
using System.Web.Mvc;

namespace SE_No1.Controllers
{
    [CustomAuthorize]
    public class EmployeeController : Controller
    {
        private EmployeeService service = new EmployeeService();
        // GET: Employee

        public ActionResult Index()
        {
            //if (!CommonCodes.checkAuth())
            //{
            //    return RedirectToAction("Error", "Error", new { errorMsg = "您沒有權限執行此動作" });
            //}
            return View();
        }

        [HttpPost]
        public ActionResult GetDataByID(int employeeID)
        {
            return Content(JsonConvert.SerializeObject(service.GetDataByID(employeeID)), "application/json");
        }

        [HttpPost]
        public ActionResult LoadAllData()
        {
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();
            var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
            var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();

            return Content(JsonConvert.SerializeObject(service.LoadAllData(draw, start, length, sortColumn, sortColumnDir, searchValue)), "application/json");
        }

        [HttpPost]
        public ActionResult Create(EmployeeVM employeeVM)
        {
            if (ModelState.IsValid)
            {
                return Content(JsonConvert.SerializeObject(service.Create(employeeVM)), "application/json");
            }
            else
            {
                Result ret = new Result();
                ret.success = false;
                ret.errorMsg = "";
                foreach (var v in ModelState.Values)
                {
                    foreach (var e in v.Errors)
                    {
                        ret.errorMsg += e.ErrorMessage + "<br />";
                    }
                }
                return Content(JsonConvert.SerializeObject(ret), "application/json");
            }
        }

        [HttpPost]
        public ActionResult Delete(int employeeID)
        {
            return Content(JsonConvert.SerializeObject(service.Delete(employeeID)), "application/json");
        }

        [HttpPost]
        public ActionResult Edit(EmployeeVM employeeVM)
        {
            if (ModelState.IsValid)
            {
                return Content(JsonConvert.SerializeObject(service.Edit(employeeVM)), "application/json");
            }
            else
            {
                Result ret = new Result();
                ret.success = false;
                ret.errorMsg = "";
                foreach (var v in ModelState.Values)
                {
                    foreach (var e in v.Errors)
                    {
                        ret.errorMsg += e.ErrorMessage + "<br />";
                    }
                }
                return Content(JsonConvert.SerializeObject(ret), "application/json");
            }
        }

        [HttpPost]
        public ActionResult IsROCID(string id)
        {
            return Content(JsonConvert.SerializeObject(service.IsROCID(id)), "application/json");
        }

        [HttpPost]
        public ActionResult IsDoBValid(string dob)
        {
            return Content(JsonConvert.SerializeObject(service.IsDoBValid(dob)), "application/json");
        }
    }
}