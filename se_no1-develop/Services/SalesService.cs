using Newtonsoft.Json.Linq;
using SE_No1.Models;
using SE_No1.Utilities;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace SE_No1.Services
{
    public class SalesService
    {
        DB db = new DB();
        public JObject LoadAllData(string draw, string start, string length, string sortColumn, string sortColumnDir, string searchValue)
        {
            try
            {
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                // Getting all sales data    
                var salesData = db.getSalesData();

                //Sorting    
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    salesData = salesData.OrderBy(sortColumn + " " + sortColumnDir);
                }

                //TODO#111423020 要看一下查詢條件怎麼下
                ////Search    
                //if (!string.IsNullOrEmpty(searchValue))
                //{
                //    employeeData = employeeData.Where(m => m.employeeName.ToLower().Contains(searchValue.ToLower()));
                //}

                //total number of rows count     
                recordsTotal = salesData.Count();
                //Paging     
                var data = salesData.Skip(skip).Take(pageSize).ToList();

                //Returning Json Data
                JObject ret = new JObject() { { "draw", draw }, { "recordsFiltered", recordsTotal }, { "recordsTotal", recordsTotal } };
                ret["data"] = JToken.FromObject(data);
                return ret;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public JObject GetDataByID(string salesID)
        {
            Sale sale = db.getSales().Where(x => x.SalesID == Convert.ToInt32(salesID)).FirstOrDefault();
            JObject ret = JObject.FromObject(sale);

            return ret;
        }

        public Result Create(Sale sale)
        {
            Result ret = new Result();

            try
            {
                bool stat = db.createSales(sale);
                if (!stat)
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

        public Result Delete(string id)
        {
            Result ret = new Result();

            try
            {
                bool stat = db.deleteSales(id);
                if (!stat)
                {
                    ret.success = false;
                    ret.errorMsg = "刪除失敗";
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

        public Result Edit(Sale sale)
        {
            Result ret = new Result();

            try
            {
                bool stat = db.updateSales(sale);
                if (!stat)
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
    }
}