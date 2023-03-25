using Newtonsoft.Json;
using SE_No1.Attributes;
using SE_No1.Models;
using SE_No1.Services;
using SE_No1.Utilities;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Web.Mvc;

namespace SE_No1.Controllers
{
    [CustomAuthorize]
    public class SalesController : Controller
    {
        DB db = new DB();
        private SalesService salesService = new SalesService();
        // GET: Sales
        public ActionResult Index()
        {
            //取得產品資料for下拉式選單
            var productList = db.getProducts();

            //取得合作夥伴資料for下拉式選單
            var partnerList = db.getPartnerByCorporateType(CorporateType.DownstreamClient);

            ViewBag.ProductList = productList;
            ViewBag.PartnerList = partnerList;

            return View();
        }

        /// <summary>
        /// 取得銷售訂單資料By銷售訂單編號
        /// </summary>
        /// <param name="salesID">銷售訂單編號</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetDataByID(string salesID)
        {
            return Content(JsonConvert.SerializeObject(salesService.GetDataByID(salesID)), "application/json");
        }

        /// <summary>
        /// 撈全部的欄位資料
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

            return Content(JsonConvert.SerializeObject(salesService.LoadAllData(draw, start, length, sortColumn, sortColumnDir, searchValue)), "application/json");
        }

        /// <summary>
        /// 新增銷售訂單資料表
        /// </summary>
        /// <param name="sale">銷售訂單資訊</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(Sale sale)
        {
            return Content(JsonConvert.SerializeObject(salesService.Create(sale)), "application/json");
        }

        /// <summary>
        /// 新增銷售訂單資料表
        /// </summary>
        /// <param name="salesID">銷售訂單ID</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(string salesID)
        {
            return Content(JsonConvert.SerializeObject(salesService.Delete(salesID)), "application/json");
        }

        /// <summary>
        /// 修改銷售訂單資料表
        /// </summary>
        /// <param name="sale">銷售訂單資訊</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(Sale sale)
        {
            return Content(JsonConvert.SerializeObject(salesService.Edit(sale)), "application/json");
        }
    }
}