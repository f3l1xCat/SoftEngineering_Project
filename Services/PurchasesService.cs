using Microsoft.Ajax.Utilities;
using Newtonsoft.Json.Linq;
using SE_No1.Models;
using SE_No1.Models.ViewModels;
using SE_No1.Utilities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Web.Http.Results;

namespace SE_No1.Services
{
    public class PurchasesService
    {
        DB db = new DB();
        Result result = new Result();

        public JObject LoadAllData(string draw, string start, string length, string sortColumn, string sortColumnDir, string searchValue)
        {
            try
            {
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                // 取得Purchases全部資料   
                var PurchasesData = db.getPurchases();

                //排序    
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    PurchasesData = PurchasesData.OrderBy(sortColumn + " " + sortColumnDir);
                }


                //total number of rows count     
                recordsTotal = PurchasesData.Count();

                //Paging     
                var data = PurchasesData.Skip(skip).Take(pageSize).ToList();

                List<PurchaseVM> purchaseData = new List<PurchaseVM>();
                foreach (Purchase p in data)
                {
                    PurchaseVM purchase = new PurchaseVM();
                    purchase.PurchasesID = p.PurchasesID;
                    purchase.PurchasesCount = p.PurchasesCount;
                    purchase.PurchasesTime = p.PurchasesTime;
                    purchase.FinalPrice = p.FinalPrice;
                    purchase.ProductName = db.getProducts().Where(x => x.ProductID == p.ProductID).FirstOrDefault().ProductName;
                    purchase.CorporateName = db.getPartner().Where(x => x.CorporateID == p.CorporateID).FirstOrDefault().CorporateName;

                    purchaseData.Add(purchase);
                }

                //搜尋by條件    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    purchaseData = purchaseData.AsQueryable().Where(m => m.ProductName.ToLower().Contains(searchValue.ToLower())).ToList();
                }

                //Returning Json Data
                JObject ret = new JObject() { { "draw", draw }, { "recordsFiltered", recordsTotal }, { "recordsTotal", recordsTotal } };
                ret["data"] = JToken.FromObject(purchaseData);
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 取得進貨訂單資料By進貨訂單ID
        /// </summary>
        /// <param name="PurchasesID">進貨訂單ID</param>
        /// <returns></returns>
        public JObject GetDataByID(string PurchasesID)
        {
            var PurchasesIDtoInt = Convert.ToInt32(PurchasesID);
            var sale = db.getPurchases().Where(x => x.PurchasesID == PurchasesIDtoInt).FirstOrDefault();
            JObject ret = JObject.FromObject(sale);

            return ret;
        }

        /// <summary>
        /// 新增進貨訂單資料
        /// </summary>
        /// <param name="purchase">進貨訂單資料</param>
        /// <returns></returns>
        public Result Create(Purchase purchase)
        {
            //輸入資料邏輯判斷，若有誤直接return, Type = 0 新增
            if (!checkPurchases(purchase, 0)) return result;

            try
            {
                //進DB新增資料
                bool ret = db.createPurchases(purchase);

                if (!ret)
                {
                    result.success = false;
                    result.errorMsg = Message.createFail;
                }
                else
                {
                    result.successMsg = Message.createSuccess;
                }
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
                result.success = false;
                result.errorMsg = ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 刪除進貨訂單資料
        /// </summary>
        /// <param name="PurchasesID">傳入進貨訂單ID</param>
        /// <returns></returns>
        public Result Delete(string PurchasesID)
        {
            Result ret = new Result();

            //判斷輸入id是否為空，若為空直接return
            if (String.IsNullOrEmpty(PurchasesID))
            {
                ret.success = false;
                ret.errorMsg = Message.deleteFail;
                return ret;
            }

            try
            {
                //進DB刪除資料
                var result = db.deletePurchases(PurchasesID);

                if (result != true)
                {
                    ret.success = false;
                    ret.errorMsg = Message.deleteFail;
                }
                else
                {
                    ret.successMsg = Message.deleteSuccess;
                }
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
                ret.success = false;
                ret.errorMsg = ex.Message;
            }

            return ret;
        }

        /// <summary>
        /// 修改進貨訂單資料
        /// </summary>
        /// <param name="sale">進貨訂單資料</param>
        /// <returns></returns>
        public Result Edit(Purchase purchase)
        {
            Result ret = new Result();

            //輸入資料邏輯判斷，若有誤直接return
            if (!checkPurchases(purchase, Message.editData))
            {
                return ret;
            }

            try
            {
                bool result = db.updatePurchases(purchase);

                if (!result)
                {
                    ret.success = false;
                    ret.errorMsg = Message.editFail;
                }
                else
                {
                    ret.successMsg = Message.editSuccess;
                }
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
                ret.success = false;
                ret.errorMsg = ex.Message;
            }

            return ret;
        }

        /// <summary>
        /// 檢查輸入資料是否正確
        /// </summary>
        /// <param name="purchase">進貨訂單資料</param>
        /// <param name="type">DB執行的動作 0:新增 1:修改 </param>
        /// <returns></returns>
        private bool checkPurchases(Purchase purchase, int type)
        {
            //是否有進貨資料
            if (purchase == null) return errorMsg("", Message.empty);

            //若為修改資料，則多檢查ID是否有值
            if (type == Message.editData)
            {
                //檢查是否有進貨編號
                if (purchase.PurchasesID.Equals(0))
                {
                    return errorMsg("進貨編號", Message.empty);
                }
            }

            //檢查進貨數量是否為0
            if (purchase.PurchasesCount.Equals(0))
            {
                return errorMsg("進貨數量", Message.zero);
            }

            //檢查進貨數量是否為負值
            if (purchase.PurchasesCount < 0)
            {
                return errorMsg("進貨數量", Message.negativeNum);
            }

            //檢查進貨價格是否為0
            if (purchase.FinalPrice.Equals(0))
            {
                return errorMsg("進貨價格", Message.zero);
            }

            //檢查進貨價格是否為負值
            if (purchase.FinalPrice < 0)
            {
                return errorMsg("進貨價格", Message.negativeNum);
            }

            //檢查進貨日期是否在範圍內
            if (purchase.PurchasesTime != null)
            {
                DateTime startTime = Convert.ToDateTime("2022/1/1");
                DateTime endTime = DateTime.Now;
                if (purchase.PurchasesTime < startTime || purchase.PurchasesTime >= endTime)
                {
                    return errorMsg("進貨日期", Message.wrong);
                }
            }

            //檢查是否有填寫供應商
            if (purchase.CorporateID.Equals(0))
            {
                return errorMsg("供應商", Message.empty);
            }

            //檢查是否有填寫產品
            if (purchase.ProductID.Equals(0))
            {
                return errorMsg("產品名稱", Message.empty);
            }

            return true;
        }

        /// <summary>
        /// 錯誤訊息
        /// </summary>
        /// <param name="fieldName">欄位名稱</param>
        /// <param name="dataStatus">資料狀態</param>
        /// <returns></returns>
        private bool errorMsg(string fieldName, int dataStatus)
        {
            CommonCodes.errorMsg(fieldName, dataStatus, ref result);
            return result.success;
        }
    }
}