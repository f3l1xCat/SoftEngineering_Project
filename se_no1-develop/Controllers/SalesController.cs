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
    public class SalesController : Controller
    {
        DB db = new DB();
        private SalesService salesService = new SalesService();
        // GET: Sales
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetDataByID(string salesID)
        {
            return Content(JsonConvert.SerializeObject(salesService.GetDataByID(salesID)), "application/json");
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

            return Content(JsonConvert.SerializeObject(salesService.LoadAllData(draw, start, length, sortColumn, sortColumnDir, searchValue)), "application/json");
        }

        [HttpPost]
        public ActionResult Create(Sale sale)
        {
            return Content(JsonConvert.SerializeObject(salesService.Create(sale)), "application/json");
        }

        [HttpPost]
        public ActionResult Delete(string salesID)
        {
            return Content(JsonConvert.SerializeObject(salesService.Delete(salesID)), "application/json");
        }

        [HttpPost]
        public ActionResult Edit(Sale sale)
        {
            return Content(JsonConvert.SerializeObject(salesService.Edit(sale)), "application/json");
        }
    }
}