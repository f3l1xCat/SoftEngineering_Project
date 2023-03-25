using Newtonsoft.Json;
using SE_No1.Attributes;
using SE_No1.Models;
using SE_No1.Services;
using SE_No1.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace SE_No1.Controllers
{
    [CustomAuthorize]
    public class ProductController : Controller
    {
        DB db = new DB();
        private ProductService productService = new ProductService();
        // GET: Product
        public ActionResult Index()
        {
            //取得合作夥伴資料for下拉式選單
            var partnerList = db.getPartnerByCorporateType(CorporateType.Supplier);

            ViewBag.PartnerList = partnerList;

            return View();
        }
        public ActionResult GetDataByID(int productID)
        {
            return Content(JsonConvert.SerializeObject(productService.GetDataByID(productID)), "application/json");
        }
        public ActionResult LoadAllData()
        {
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();
            var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
            var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();

            return Content(JsonConvert.SerializeObject(productService.LoadAllData(draw, start, length, sortColumn, sortColumnDir, searchValue)), "application/json");
        }

        [HttpPost]
        public ActionResult Create(Product product)
        {
            return Content(JsonConvert.SerializeObject(productService.Create(product)), "application/json");
        }

        [HttpPost]
        public ActionResult Delete(int productID)
        {
            return Content(JsonConvert.SerializeObject(productService.Delete(productID)), "application/json");
        }

        [HttpPost]
        public ActionResult Edit(Product product)
        {
            return Content(JsonConvert.SerializeObject(productService.Edit(product)), "application/json");
        }
        [HttpPost]
        public ActionResult GetDataList()
        {
            return Content(JsonConvert.SerializeObject(productService.GetDataList()), "application/json");
        }
    }
}