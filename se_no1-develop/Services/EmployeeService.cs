using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SE_No1.Models;
using SE_No1.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Web;

namespace SE_No1.Services
{
    public class EmployeeService
    {
        DB db = new DB();
        public JObject LoadAllData(string draw, string start, string length, string sortColumn, string sortColumnDir, string searchValue)
        {
            try
            {
                //Paging Size (10,20,50,100)    
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                // Getting all Customer data    
                var employeeData = db.getEmployees();

                //Sorting    
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    employeeData = employeeData.OrderBy(sortColumn + " " + sortColumnDir);
                }
                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    employeeData = employeeData.Where(m => m.EmployeeName.ToLower().Contains(searchValue.ToLower()));
                }

                //測試資料
                //List<Employee> employeeData = new List<Employee> { 
                //    new Employee { employeeID = "5", employeeName = "Gabi Braun"},
                //    new Employee { employeeID="6", employeeName = "Pieck Fingger"},
                //    new Employee { employeeID="7", employeeName = "Sasha Blouse"},
                //    new Employee { employeeID="8", employeeName = "Levi Ackerman"},
                //    new Employee { employeeID="9", employeeName = "Mikasa Ackerman"},
                //    new Employee { employeeID="10", employeeName = "Eren Yeager"},
                //    new Employee { employeeID="11", employeeName = "Zeke Yeager"},
                //    new Employee { employeeID="12", employeeName = "Falco Grice"},
                //    new Employee { employeeID="13", employeeName = "Arni Leonheart"},
                //    new Employee { employeeID="14", employeeName = "Armin Arlert"},
                //    new Employee { employeeID="15", employeeName = "Hange Zoe"},
                //    new Employee { employeeID="16", employeeName = "Grisha Yeager"},
                //    new Employee { employeeID="17", employeeName = "Carla Yeager"},
                //    new Employee { employeeID="18", employeeName = "Rainer Braun"},
                //    new Employee { employeeID="19", employeeName = "Connie Springer"}
                //};
                ////Search    
                //if (!string.IsNullOrEmpty(searchValue))
                //{
                //    employeeData = employeeData.Where(m => m.employeeName.ToLower().Contains(searchValue.ToLower())).ToList();
                //}

                //total number of rows count     
                recordsTotal = employeeData.Count();
                //Paging     
                var data = employeeData.Skip(skip).Take(pageSize).ToList();

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

        public JObject GetDataByID(int employeeID)
        {
            Employee emp = db.getEmployees().Where(x => x.EmployeeID == employeeID).FirstOrDefault();
            JObject ret = JObject.FromObject(emp);

            return ret;
        }

        public Result Create(Employee employee)
        {
            Result ret = new Result();

            try
            {
                string stat = db.createEmployee(employee);
                if (stat != null)
                {
                    ret.success = false;
                    ret.errorMsg = stat;
                }
                else
                {
                    ret.successMsg = Convert.ToString(System.Web.Configuration.WebConfigurationManager.AppSettings["CreateSuccessMsg"]);
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
                string stat = db.deleteEmployee(id);
                if (stat != null)
                {
                    ret.success = false;
                    ret.errorMsg = stat;
                }
                else
                {
                    ret.successMsg = Convert.ToString(System.Web.Configuration.WebConfigurationManager.AppSettings["DeleteSuccessMsg"]);
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

        public Result Edit(Employee employee)
        {
            Result ret = new Result();

            try
            {
                string stat = db.updateEmployee(employee);
                if (stat != null)
                {
                    ret.success = false;
                    ret.errorMsg = stat;
                }
                else
                {
                    ret.successMsg = Convert.ToString(System.Web.Configuration.WebConfigurationManager.AppSettings["UpdateSuccessMsg"]);
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