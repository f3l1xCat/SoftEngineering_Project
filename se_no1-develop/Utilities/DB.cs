using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Web;
using SE_No1.Models;

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
                return false;
            }
        }

        public bool deleteSales(string salesID)
        {
            try
            {
                Sale sale = db.Sales.Where(x => x.SalesID.Equals(salesID)).FirstOrDefault();
                db.Sales.Remove(sale);
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool updateSales(Sale sale)
        {
            try
            {
                db.Entry(sale).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        #endregion
    }
}