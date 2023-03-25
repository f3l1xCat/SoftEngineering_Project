using Newtonsoft.Json;
using SE_No1.Models;
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
    public class EmployeeController : Controller
    {
        DB db = new DB();
        private EmployeeService service = new EmployeeService();
        // GET: Employee
        public ActionResult Index()
        {
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
        public ActionResult Create(Employee employee)
        {
            return Content(JsonConvert.SerializeObject(service.Create(employee)), "application/json");
        }

        [HttpPost]
        public ActionResult Delete(int employeeID)
        {
            return Content(JsonConvert.SerializeObject(service.Delete(employeeID)), "application/json");
        }

        [HttpPost]
        public ActionResult Edit(Employee employee)
        {
            return Content(JsonConvert.SerializeObject(service.Edit(employee)), "application/json");
        }
    }
}