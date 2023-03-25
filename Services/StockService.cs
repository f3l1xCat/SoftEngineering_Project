using Newtonsoft.Json.Linq;
using SE_No1.Models;
using SE_No1.Models.ViewModels;
using SE_No1.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Web.Http.Results;

namespace SE_No1.Services
{
    public class StockService
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

                // Getting all sales data    
                var stockData = db.getStock();
                var productsData = db.getProducts();

                //Sorting    
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    stockData = stockData.OrderBy(sortColumn + " " + sortColumnDir);
                }

               

                //total number of rows count     
                recordsTotal = stockData.Count();
                //Paging     
                var data = stockData.Skip(skip).Take(pageSize).ToList();
                List<stock_product_view> retData = new List<stock_product_view>();
                foreach (var item in data)
                {
                    stock_product_view temp = new stock_product_view();
                    temp.ProductID = item.ProductID;
                    temp.ProductName = db.getProducts().Where(x => x.ProductID == item.ProductID).FirstOrDefault().ProductName;
                    temp.ShelveID = item.ShelveID;
                    temp.StockID = item.StockID;
                    temp.StoreHouseID = item.StoreHouseID;
                    temp.StockCount= item.StockCount;
                    temp.StoreTime= item.StoreTime;
                    retData.Add(temp);
                
                }
                ////Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    retData = retData.AsQueryable().Where(m => m.ProductName.ToLower().Contains(searchValue.ToLower())).ToList();
                }
                //Returning Json Data
                JObject ret = new JObject() { { "draw", draw }, { "recordsFiltered", recordsTotal }, { "recordsTotal", recordsTotal } };
                ret["data"] = JToken.FromObject(retData);
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public JObject GetDataByID(int stockID)
        {
            Stock stock = db.getStock().Where(x => x.StockID == stockID).FirstOrDefault();
            JObject ret = JObject.FromObject(stock);

            return ret;
        }

        public Result Create(Stock stock)
        {
            Result ret = new Result();
            //if (!checkStock(stock, Message.editData))
            //{
            //    return ret;
            //}
            try
            {
                string stat = db.createStock(stock);
                if (stat != null)
                {
                    ret.success = false;
                    ret.errorMsg = "新增失敗";
                }
                else
                {
                    ret.successMsg = "新增成功";
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

        public Result Delete(int id)
        {
            Result ret = new Result();

            try
            {
                string stat = db.deleteStock(id); //錯誤訊息
                if (stat != null)
                {
                    ret.success = false;
                    ret.errorMsg = stat; //顯示錯誤訊息
                }
                else
                {
                    ret.successMsg = "刪除成功";
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

        public Result Edit(Stock stock)
        {
            Result ret = new Result();
            //輸入資料邏輯判斷，若有誤直接return
            //if (!checkStock(stock, Message.editData))
           // {
            //    return ret;
            //}
            try
            {
                string stat = db.updateStock(stock);
                if (stat != null)
                {
                    ret.success = false;
                    ret.errorMsg = "修改失敗";
                }
                else
                {
                    ret.successMsg = "修改成功";
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

        //private bool checkStock(Stock stock, int type)
        //{
        //    if (stock == null) return errorMsg("", Message.empty);
        //    if (stock.StoreTime != null)
        //    {
        //        DateTime startTime = Convert.ToDateTime("2022/1/1");
        //        DateTime endTime = DateTime.Now;
        //        if (stock.StoreTime < startTime || stock.StoreTime >= endTime)
        //        {
        //            return errorMsg("進貨日期", Message.wrong);
        //        }
        //    }
        //    return true;
        //}

        //private bool errorMsg(string fieldName, int dataStatus)
        //{
        //    CommonCodes.errorMsg(fieldName, dataStatus, ref result);
        //    return result.success;
        //}
    }
}