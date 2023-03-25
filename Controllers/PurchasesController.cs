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
    public class PurchasesController : Controller
    {
        DB db = new DB();
        private PurchasesService PurchasesService = new PurchasesService();
        // GET: Purchases
        public ActionResult Index()
        {
            //取得產品資料for下拉式選單
            var productList = db.getProducts();

            //取得合作夥伴資料for下拉式選單
            var partnerList = db.getPartnerByCorporateType(CorporateType.Supplier);

            ViewBag.ProductList = productList;
            ViewBag.PartnerList = partnerList;

            return View();
        }

        /// <summary>
        /// 取得進貨訂單資料By進貨訂單ID
        /// </summary>
        /// <param name="PurchasesID">進貨訂單ID</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetDataByID(string PurchasesID)
        {
            return Content(JsonConvert.SerializeObject(PurchasesService.GetDataByID(PurchasesID)), "application/json");
        }

        /// <summary>
        /// 撈全部的欄位和資料
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult LoadAllData()
        {
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();
            var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
            var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();

            return Content(JsonConvert.SerializeObject(PurchasesService.LoadAllData(draw, start, length, sortColumn, sortColumnDir, searchValue)), "application/json");
        }

        /// <summary>
        /// 新增進貨訂單資料表
        /// </summary>
        /// <param name="purchase">銷售訂單資訊</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(Purchase purchase)
        {
            return Content(JsonConvert.SerializeObject(PurchasesService.Create(purchase)), "application/json");
        }

        /// <summary>
        /// 新增進貨訂單資料表
        /// </summary>
        /// <param name="PurchasesID">進貨訂單ID</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(string PurchasesID)
        {
            return Content(JsonConvert.SerializeObject(PurchasesService.Delete(PurchasesID)), "application/json");
        }

        /// <summary>
        /// 修改進貨訂單資料表
        /// </summary>
        /// <param name="purchase">進貨訂單資訊</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(Purchase purchase)
        {
            return Content(JsonConvert.SerializeObject(PurchasesService.Edit(purchase)), "application/json");
        }
    }
}