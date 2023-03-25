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
    public class RoleService
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
                var roleData = db.getRoles();

                //Sorting    
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    roleData = roleData.OrderBy(sortColumn + " " + sortColumnDir);
                }
                //Search    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    roleData = roleData.Where(m => m.RoleName.ToLower().Contains(searchValue.ToLower()));
                }

                //total number of rows count     
                recordsTotal = roleData.Count();
                //Paging     
                var data = roleData.Skip(skip).Take(pageSize).ToList();

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

        public JArray GetDataList()
        {
            JArray ret = new JArray();
            List<Role> roleList = db.getRoles().ToList();
            foreach (Role role in roleList)
            {
                JObject r = new JObject()
                {
                    {"RoleID", role.RoleID },
                    {"RoleName", role.RoleName }
                };
                ret.Add(r);
            }

            return ret;
        }

        public JObject GetDataByID(int RoleID)
        {
            Role role = db.getRoles().Where(x => x.RoleID == RoleID).FirstOrDefault();
            JObject ret = JObject.FromObject(role);

            return ret;
        }

        public Result Create(Role role)
        {
            Result ret = new Result();

            try
            {
                string stat = db.createRole(role);
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
                string stat = db.deleteRole(id);
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

        public Result Edit(Role role)
        {
            Result ret = new Result();

            try
            {
                string stat = db.updateRole(role);
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