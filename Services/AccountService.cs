using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SE_No1.Models;
using SE_No1.Models.ViewModels;
using SE_No1.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Web;

namespace SE_No1.Services
{
    public class AccountService
    {
        DB db = new DB();
        
        public bool AuthenticateUser(string userName, string scrStr) 
        {
            bool ret = false;
            Employee emp = db.getEmployees().Where(x => x.AccountName == userName).FirstOrDefault();
            if (emp != null)
            {
                //密碼驗證
                if (emp.EmployeePsw.SequenceEqual(CommonCodes.sha256_hash(scrStr)))
                {
                    System.Web.HttpContext.Current.Session["UserName"] = emp.AccountName;
                    System.Web.HttpContext.Current.Session["EmployeeName"] = emp.EmployeeName;
                    System.Web.HttpContext.Current.Session["EmployeeID"] = emp.EmployeeID;
                    System.Web.HttpContext.Current.Session["RoleID"] = emp.RoleID;
                    ret = true;
                }
            }
            return ret;
        }

        //檢查密碼長度介於8~16碼且包含大小寫及底線
        public bool IsValidPassword(string psw)
        {
            bool ret = false;
            if (psw.Length >= 8 && psw.Length <= 16 && psw.Any(char.IsUpper) && psw.Any(char.IsLower) && psw.Contains("_"))
            {
                ret = true;
            }

            return ret;
        }
    }
}