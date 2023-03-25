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

namespace SE_No1.Controllers
{
    [CustomAuthorize]
    public class RoleController : Controller
    {
        DB db = new DB();
        private RoleService service = new RoleService();
        // GET: Role
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetDataByID(int RoleID)
        {
            return Content(JsonConvert.SerializeObject(service.GetDataByID(RoleID)), "application/json");
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
        public ActionResult GetDataList()
        {
            return Content(JsonConvert.SerializeObject(service.GetDataList()), "application/json");
        }

        [HttpPost]
        public ActionResult Create(Role role)
        {
            if (ModelState.IsValid)
            {
                return Content(JsonConvert.SerializeObject(service.Create(role)), "application/json");
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
        public ActionResult Delete(int RoleID)
        {
            return Content(JsonConvert.SerializeObject(service.Delete(RoleID)), "application/json");
        }

        [HttpPost]
        public ActionResult Edit(Role role)
        {
            if (ModelState.IsValid)
            {
                return Content(JsonConvert.SerializeObject(service.Edit(role)), "application/json");
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
                        if (e.ErrorMessage.Contains("RoleID")) //RoleID的錯誤不必顯示
                        {
                            continue;
                        }
                        ret.errorMsg += e.ErrorMessage + "<br />";
                    }
                }
                return Content(JsonConvert.SerializeObject(ret), "application/json");
            }            
        }
    }
}