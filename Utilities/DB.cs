using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Web;
using System.Web.Management;
using SE_No1.Models;
using SE_No1.Models.ViewModels;

namespace SE_No1.Utilities
{
    public class DB
    {
        JRCHENGEntities db = new JRCHENGEntities();

        //員工
        #region
        public IQueryable<Employee> getEmployees()
        {
            return db.Employees;
        }

        public string createEmployee(Employee employee)
        {
            try
            {
                db.Employees.Add(employee);
                db.SaveChanges();
                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string deleteEmployee(int employeeID)
        {
            try
            {
                Employee employee = db.Employees.Where(x => x.EmployeeID == employeeID).FirstOrDefault();
                db.Employees.Remove(employee);
                db.SaveChanges();
                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string updateEmployee(Employee employee)
        {
            try
            {
                db.Entry(employee).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        #endregion

        //角色
        #region
        public IQueryable<Role> getRoles()
        {
            return db.Roles;
        }

        public string createRole(Role role)
        {
            try
            {
                db.Roles.Add(role);
                db.SaveChanges();
                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string updateRole(Role role)
        {
            try
            {
                db.Entry(role).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string deleteRole(int RoleID)
        {
            try
            {
                Role role = db.Roles.Where(x => x.RoleID == RoleID).FirstOrDefault();
                db.Roles.Remove(role);
                db.SaveChanges();
                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        #endregion

        //銷售
        #region
        public IQueryable<Sale> getSales()
        {
            return db.Sales;
        }

        public IQueryable<fnGetSalesData_Result> getSalesData()
        {
            var salesData = db.fnGetSalesData().DefaultIfEmpty();
            return salesData;
        }

        public bool createSales(Sale sale)
        {
            try
            {
                db.Sales.Add(sale);
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool deleteSales(string salesID)
        {
            try
            {
                var salesIDtoInt = Convert.ToInt32(salesID);
                var sale = getSales().Where(x => x.SalesID == salesIDtoInt).FirstOrDefault();
                db.Sales.Remove(sale);
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool updateSales(Sale sale)
        {
            try
            {
                db.Entry(sale).State = EntityState.Modified;
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion

        //進貨
        #region
        public IQueryable<Purchase> getPurchases()
        {
            return db.Purchases;
        }

        public bool createPurchases(Purchase purchase)
        {
            try
            {
                db.Purchases.Add(purchase);
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool deletePurchases(string purchasesID)
        {
            try
            {
                var purchasesIDtoInt = Convert.ToInt32(purchasesID);
                var purchase = getPurchases().Where(x => x.PurchasesID == purchasesIDtoInt).FirstOrDefault();
                db.Purchases.Remove(purchase);
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool updatePurchases(Purchase purchase)
        {
            try
            {
                db.Entry(purchase).State = EntityState.Modified;
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                throw e;
            }
        }
        #endregion

        //產品
        #region
        public IQueryable<View_Products_Partners> getProducts_dbView()
        {
            return db.View_Products_Partners;
        }

        public IQueryable<Product> getProducts()
        {
            return db.Products;
        }


        public bool createProduct(Product product)
        {
            try
            {
                db.Products.Add(product);
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool deleteProduct(int productID)
        {
            try
            {
                Product product = db.Products.Where(x => x.ProductID.Equals(productID)).FirstOrDefault();
                db.Products.Remove(product);
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool updateProduct(Product product)
        {
            try
            {
                db.Entry(product).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        #endregion

        //合作夥伴
        #region
        public IQueryable<Partner> getPartner()
        {
            return db.Partners;
        }

        /// <summary>
        /// 撈合作夥伴資料By合作夥伴種類
        /// </summary>
        /// <param name="corporateType">合作夥伴種類</param>
        /// <returns></returns>
        public IQueryable<Partner> getPartnerByCorporateType(string corporateType)
        {
            return db.Partners.Where(x => x.CorporateType == corporateType);
        }

        public string createPartner(Partner partner)
        {
            try
            {
                db.Partners.Add(partner);
                db.SaveChanges();
                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string updatePartner(Partner partner)
        {
            try
            {
                db.Entry(partner).State = EntityState.Modified;
                db.SaveChanges();
                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string deletePartner(int corporateID)
        {
            try
            {
                Partner partner = getPartner().Where(x => x.CorporateID == corporateID).FirstOrDefault();
                db.Partners.Remove(partner);
                db.SaveChanges();
                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        #endregion

        //存貨
        #region
        public IQueryable<Stock> getStock()
        {
            return db.Stocks;
        }

        public string createStock(Stock stocks)
        {
            try
            {
                db.Stocks.Add(stocks);
                db.SaveChanges();
                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string updateStock(Stock stock)
        {
            try
            {
                db.Entry(stock).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public string deleteStock(int StockID)
        {
            try
            {
                Stock stock = db.Stocks.Where(x => x.StockID == StockID).FirstOrDefault();
                db.Stocks.Remove(stock);
                db.SaveChanges();
                return null;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        #endregion
    }
}