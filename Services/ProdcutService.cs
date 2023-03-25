using Newtonsoft.Json.Linq;
using SE_No1.Controllers;
using SE_No1.Models;
using SE_No1.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Net;
using System.Web;

namespace SE_No1.Services
{
    public class ProductService
    {
        public static string serverName = "https://localhost:44382/Content/Images/uploads/";
        DB db = new DB();
        public JObject LoadAllData(string draw, string start, string length, string sortColumn, string sortColumnDir, string searchValue)
        {
            try
            {
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                // Getting all sales data    
                var productsData = db.getProducts_dbView();

                //Sorting    
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    productsData = productsData.OrderBy(sortColumn + " " + sortColumnDir);
                }

                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    productsData = productsData.Where(m => m.ProductName.ToLower().Contains(searchValue.ToLower()));
                }

                //total number of rows count     
                recordsTotal = productsData.Count();

                //Paging     
                var data = productsData.Skip(skip).Take(pageSize).ToList();

                List<View_Products_Partners> productData = new List<View_Products_Partners>();
                foreach (var p in data) {
                    View_Products_Partners vpp = new View_Products_Partners();
                    vpp.ProductID= p.ProductID;
                    vpp.ProductName = p.ProductName;
                    vpp.ProductSize = p.ProductSize;
                    vpp.ProductCategory = p.ProductCategory;
                    vpp.ProductClassifier= p.ProductClassifier;
                    vpp.Pricing = p.Pricing;
                    vpp.CorporateName = p.CorporateName;
                    vpp.ProductImageUrl = p.ProductImageUrl;

                    productData.Add(vpp);
                }

                //Returning Json Data
                JObject ret = new JObject() { { "draw", draw }, { "recordsFiltered", recordsTotal }, { "recordsTotal", recordsTotal } };
                ret["data"] = JToken.FromObject(productData);
                return ret;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public JArray GetDataList()
        {
            JArray ret = new JArray();
            List<Product> productList = db.getProducts().ToList();
            foreach (Product product in productList)
            {
                JObject r = new JObject()
                {
                    {"ProductID", product.ProductID },
                    {"ProductName", product.ProductName }
                };
                ret.Add(r);
            }

            return ret;
        }
        public JObject GetDataByID(int productID)
        {
            Product product = db.getProducts().Where(x => x.ProductID == productID).FirstOrDefault();
            JObject ret = JObject.FromObject(product);

            return ret;
        }

        public Result Create(Product product)
        {
            Result ret = new Result();
            try
            {                
                product.ProductImageUrl = ImageUploadController.serverName + product.ProductImageUrl;
                bool stat = db.createProduct(product);
                if (!stat)
                {
                    ret.success = false;
                    ret.errorMsg = Message.createFail;
                }
                else
                {
                    ret.successMsg = Message.createSuccess;
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
                Product p = db.getProducts().Where(x => x.ProductID == id).FirstOrDefault();
                if (p != null)
                {
                    File.Delete(HttpContext.Current.Server.MapPath(p.ProductImageUrl));
                }
                bool stat = db.deleteProduct(id);
                if (!stat)
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

        public Result Edit(Product product)
        {
            Result ret = new Result();
            try
            {
                Product p = db.getProducts().Where(x => x.ProductID == product.ProductID).FirstOrDefault();
                if (p != null && Path.GetFileName(p.ProductImageUrl) != Path.GetFileName(product.ProductImageUrl)) //有換圖片先把本來的圖片刪除
                {
                    File.Delete(HttpContext.Current.Server.MapPath(p.ProductImageUrl));
                }

                p.CorporateID = product.CorporateID;
                p.ProductCategory = product.ProductCategory;
                p.ProductName = product.ProductName;
                p.ProductSize = product.ProductSize;
                p.ProductImageUrl = ImageUploadController.serverName + product.ProductImageUrl;
                p.ProductClassifier = product.ProductClassifier;
                p.Pricing = product.Pricing;

                bool stat = db.updateProduct(p);
                if (!stat)
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
    }
}