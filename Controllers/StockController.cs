using Newtonsoft.Json;
using SE_No1.Attributes;
using SE_No1.Models;
using SE_No1.Services;
using SE_No1.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace SE_No1.Controllers
{
    [CustomAuthorize]
    public class StockController : Controller
    {
        DB db = new DB();
        private StockService StockService = new StockService();
        // GET: Sales
        public ActionResult Index()
        {
            //取得產品資料for下拉式選單
            var stockList = db.getStock();

            //取得合作夥伴資料for下拉式選單
            //var partnerList = db.getPartner();

            ViewBag.StockList = stockList;
            //ViewBag.PartnerList = partnerList;

            return View();
        }

        [HttpPost]
        public ActionResult GetDataByID(int stockID)
        {
            return Content(JsonConvert.SerializeObject(StockService.GetDataByID(stockID)), "application/json");
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

            return Content(JsonConvert.SerializeObject(StockService.LoadAllData(draw, start, length, sortColumn, sortColumnDir, searchValue)), "application/json");
        }

        [HttpPost]
        public ActionResult Create(Stock stock)
        {
            if (ModelState.IsValid)
            {
                return Content(JsonConvert.SerializeObject(StockService.Create(stock)), "application/json");
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
        public ActionResult Delete(int stockID)
        {
            return Content(JsonConvert.SerializeObject(StockService.Delete(stockID)), "application/json");
        }

        [HttpPost]
        public ActionResult Edit(Stock stock)
        {
           if (ModelState.IsValid)
            {
                return Content(JsonConvert.SerializeObject(StockService.Edit(stock)), "application/json");
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
    }
}