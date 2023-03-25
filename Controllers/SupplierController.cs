using Newtonsoft.Json;
using SE_No1.Attributes;
using SE_No1.Models;
using SE_No1.Services;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Web.Mvc;

namespace SE_No1.Controllers
{
    [CustomAuthorize]
    public class SupplierController : Controller
    {
        private PartnerService partnerService = new PartnerService();
        static string CorporateType = "";

        // GET: Partner(供應商)
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 取得供應商資料By合作夥伴編號
        /// </summary>
        /// <param name="CorporateID"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetDataByID(string CorporateID)
        {
            var corporateIDtoInt = Convert.ToInt32(CorporateID);
            return Content(JsonConvert.SerializeObject(partnerService.GetDataByID(corporateIDtoInt)), "application/json");
        }

        /// <summary>
        /// 撈全部的欄位資料
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult LoadAllData(string corporateType)
        {
            CorporateType = corporateType;
            var draw = Request.Form.GetValues("draw").FirstOrDefault();
            var start = Request.Form.GetValues("start").FirstOrDefault();
            var length = Request.Form.GetValues("length").FirstOrDefault();
            var sortColumn = Request.Form.GetValues("columns[" + Request.Form.GetValues("order[0][column]").FirstOrDefault() + "][name]").FirstOrDefault();
            var sortColumnDir = Request.Form.GetValues("order[0][dir]").FirstOrDefault();
            var searchValue = Request.Form.GetValues("search[value]").FirstOrDefault();

            //這邊多傳入合作夥伴的種類(供應商)
            return Content(JsonConvert.SerializeObject(partnerService.LoadAllData(draw, start, length, sortColumn, sortColumnDir, searchValue, corporateType)), "application/json");
        }

        /// <summary>
        /// 取得全部供應商資料By合作夥伴種類
        /// </summary>
        /// <param name="corporateType">合作夥伴種類</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetDataList()
        {
            return Content(JsonConvert.SerializeObject(partnerService.GetDataList(CorporateType)), "application/json");
        }

        /// <summary>
        /// 新增供應商資料
        /// </summary>
        /// <param name="partner"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Create(Partner partner)
        {
            return Content(JsonConvert.SerializeObject(partnerService.Create(partner)), "application/json");
        }

        /// <summary>
        /// 刪除供應商資料
        /// </summary>
        /// <param name="CorporateID"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(int CorporateID)
        {
            return Content(JsonConvert.SerializeObject(partnerService.Delete(CorporateID)), "application/json");
        }

        /// <summary>
        /// 編輯供應商資料
        /// </summary>
        /// <param name="partner"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Edit(Partner partner)
        {
            return Content(JsonConvert.SerializeObject(partnerService.Edit(partner)), "application/json");
        }
    }
}