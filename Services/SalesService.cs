using Antlr.Runtime;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json.Linq;
using SE_No1.Models;
using SE_No1.Utilities;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Web.Http.Results;

namespace SE_No1.Services
{
    public class SalesService
    {
        DB db = new DB();
        Message msg = new Message();
        Result result = new Result();

        public JObject LoadAllData(string draw, string start, string length, string sortColumn, string sortColumnDir, string searchValue)
        {
            try
            {
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                //取得全部銷售資料   
                var salesData = db.getSalesData();

                //排序    
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    salesData = salesData.OrderBy(sortColumn + " " + sortColumnDir);
                }

                //搜尋by條件    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    salesData = salesData.Where(m => m.ProductName.ToLower().Contains(searchValue.ToLower()));
                }

                //total number of rows count     
                recordsTotal = salesData.Count();

                //Paging     
                var data = salesData.Skip(skip).Take(pageSize).ToList();

                //Returning Json Data
                JObject ret = new JObject() { { "draw", draw }, { "recordsFiltered", recordsTotal }, { "recordsTotal", recordsTotal } };
                ret["data"] = JToken.FromObject(data);
                return ret;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 取得銷售訂單資料By銷售訂單ID
        /// </summary>
        /// <param name="salesID">銷售訂單ID</param>
        /// <returns></returns>
        public JObject GetDataByID(string salesID)
        {
            var salesIDtoInt = Convert.ToInt32(salesID);
            var sale = db.getSales().Where(x => x.SalesID == salesIDtoInt).FirstOrDefault();
            JObject ret = JObject.FromObject(sale);

            return ret;
        }

        /// <summary>
        /// 新增銷貨訂單資料
        /// </summary>
        /// <param name="sale">銷貨訂單資料</param>
        /// <returns></returns>
        public Result Create(Sale sale)
        {
            //輸入資料邏輯判斷，若有誤直接return, Type = 0 新增
            if (!checkSales(sale, 0)) return result;

            try
            {
                //進DB新增資料
                bool ret = db.createSales(sale);

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
        /// 刪除銷售訂單資料
        /// </summary>
        /// <param name="salesID">傳入銷售訂單ID</param>
        /// <returns></returns>
        public Result Delete(string salesID)
        {
            //判斷輸入id是否為空，若為空直接return
            if (String.IsNullOrEmpty(salesID))
            {
                result.success = false;
                result.errorMsg = Message.deleteFail;
                return result;
            }

            try
            {
                //進DB刪除資料
                var ret = db.deleteSales(salesID);

                if (!ret)
                {
                    result.success = false;
                    result.errorMsg = Message.deleteFail;
                }
                else
                {
                    result.successMsg = Message.deleteSuccess;
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
        /// 修改銷貨訂單資料
        /// </summary>
        /// <param name="sale">銷售訂單資料</param>
        /// <returns></returns>
        public Result Edit(Sale sale)
        {
            //輸入資料邏輯判斷，若有誤直接return, Type = 1 修改
            if (!checkSales(sale, 1)) return result;

            try
            {
                bool ret = db.updateSales(sale);

                if (!ret)
                {
                    result.success = false;
                    result.errorMsg = Message.editFail;
                }
                else
                {
                    result.successMsg = Message.editSuccess;
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
        /// 檢查輸入資料是否正確
        /// </summary>
        /// <param name="sale">銷售訂單資料</param>
        /// <param name="type">DB執行的動作 0:新增 1:修改 </param>
        /// <returns></returns>
        private bool checkSales(Sale sale, int type)
        {
            result.success = true;
            StringBuilder sb = new StringBuilder();
            
            //是否有銷售資料
            if (sale == null) 
            {
                result.success = false;
                errorMsg("", Message.empty);
                return result.success;
            }
            
            //若為修改資料，則多檢查ID是否有值
            if (type == Message.editData)
            {
                //檢查是否有銷售編號
                if (sale.SalesID.Equals(0))
                {
                    result.success = false;
                    errorMsg("銷貨編號", Message.empty);
                    return result.success;
                }
            }

            //檢查銷貨數量是否超過限制值
            if (sale.SalesCount > Message.numLimit || sale.SalesCount.Equals(0))
            {
                sb.Append(errorMsg("銷貨數量", Message.wrong));
            }

            //檢查銷貨數量是否為負值
            if (sale.SalesCount < 0)
            {
                sb.Append(errorMsg("銷貨數量", Message.negativeNum));
            }

            //檢查銷貨價格是否超過限制值
            if (sale.SalesPrice > Message.numLimit || sale.SalesPrice.Equals(0))
            {
                sb.Append(errorMsg("銷貨價格", Message.wrong));
            }

            //檢查銷貨價格是否為負值
            if (sale.SalesPrice < 0)
            {
                sb.Append(errorMsg("銷貨價格", Message.negativeNum));
            }

            //檢查銷貨日期是否在範圍內
            if (sale.SalesTime != null)
            {
                DateTime startTime = Convert.ToDateTime("2022/1/1");
                DateTime endTime = DateTime.Now;
                if (sale.SalesTime < startTime || sale.SalesTime > endTime)
                {
                    sb.Append(errorMsg("銷貨日期", Message.wrong));
                }
            }

            //檢查產品欄位是否有值
            if (sale.ProductID.Equals(0))
            {
                sb.Append(errorMsg("產品名稱", Message.empty));
            }

            //檢查通路商欄位是否有值
            if (sale.CorporateID.Equals(0))
            {
                sb.Append(errorMsg("通路商", Message.empty));
            }

            result.errorMsg = sb.ToString();

            return result.success;
        }

        /// <summary>
        /// 錯誤訊息
        /// </summary>
        /// <param name="fieldName">欄位名稱</param>
        /// <param name="dataStatus">資料狀態</param>
        /// <returns></returns>
        private string errorMsg(string fieldName, int dataStatus)
        {
            result.success = false;
            return CommonCodes.errorMsg(fieldName, dataStatus, ref result); ;
        }
    }
}