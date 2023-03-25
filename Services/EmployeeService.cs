using Jint.Native;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SE_No1.Models;
using SE_No1.Models.ViewModels;
using SE_No1.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text.RegularExpressions;
using System.Web;

namespace SE_No1.Services
{
    public class EmployeeService
    {
        DB db = new DB();
        AccountService accountService = new AccountService();
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
                    employeeData = employeeData.Where(m => m.EmployeeName.ToLower().Contains(searchValue.ToLower()) || 
                                                        m.AccountName.ToLower().Contains(searchValue.ToLower()) ||
                                                        m.EmployeeDoB.ToString().Contains(searchValue.ToLower()) ||
                                                        m.JobTitle.ToLower().Contains(searchValue.ToLower()) ||
                                                        m.IdentityCard.ToLower().Contains(searchValue.ToLower()) ||
                                                        m.PhoneNumber.Contains(searchValue.ToLower()) ||
                                                        m.Department.ToLower().Contains(searchValue.ToLower()) ||
                                                        m.EmploymentStatus.ToLower().Contains(searchValue.ToLower()) ||
                                                        (m.EmployeeSex == "1"? "男": "女").Contains(searchValue.ToLower()));
                }

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

        public Result Create(EmployeeVM employeeVM)
        {
            Result ret = new Result();

            try
            {
                //檢查帳號是否重複
                if (db.getEmployees().Where(x => x.AccountName == employeeVM.AccountName).Any())
                {
                    throw new Exception("帳號重複，請更換");
                }
                //檢查身分證字號是否重複
                if (!IsIdNotDuplicate(employeeVM.IdentityCard))
                {
                    throw new Exception("身分證字號重複，請更換");
                }
                //檢查密碼格式是否正確
                if (!accountService.IsValidPassword(employeeVM.EmployeePsw))
                {
                    throw new Exception("密碼長度需介於8~16碼，需包含大小寫及底線");
                }
                //檢查密碼確認是否正確
                if (employeeVM.EmployeePsw != employeeVM.EmployeePswCfm)
                {
                    throw new Exception("密碼確認與密碼不相同");
                }
                string stat = db.createEmployee(VMtoEmployee(employeeVM));
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

        public Result Edit(EmployeeVM employeeVM)
        {
            Result ret = new Result();

            try
            {
                Employee empOgn = db.getEmployees().Where(x => x.EmployeeID == employeeVM.EmployeeID).FirstOrDefault();
                empOgn.EmployeeName = employeeVM.EmployeeName;
                empOgn.EmployeeSex = employeeVM.EmployeeSex;
                empOgn.MaritalStatus = employeeVM.MaritalStatus;
                empOgn.EmployeeDoB = employeeVM.EmployeeDoB.Value;
                empOgn.JobTitle = employeeVM.JobTitle;
                empOgn.IdentityCard = employeeVM.IdentityCard;
                if (empOgn.Address != employeeVM.Address) //變更身分證字號
                {
                    //檢查身分證字號是否重複
                    if (!IsIdNotDuplicate(employeeVM.IdentityCard))
                    {
                        throw new Exception("身分證字號重複，請更換");
                    }
                    else
                    {
                        empOgn.Address = employeeVM.Address;
                    }
                }
                empOgn.PhoneNumber = employeeVM.PhoneNumber;
                empOgn.HomePhoneNumber = employeeVM.HomePhoneNumber;
                empOgn.Department = employeeVM.Department;
                empOgn.EmergencyContact = employeeVM.EmergencyContact;
                empOgn.EmergencyPhoneNumber = employeeVM.EmergencyPhoneNumber;
                empOgn.EmergencyContactRelationship = employeeVM.EmergencyContactRelationship;
                empOgn.EmploymentStatus = employeeVM.EmploymentStatus;
                empOgn.RoleID = employeeVM.RoleID.Value;
                //沒有填寫密碼，維持不變
                if (employeeVM.EmployeePsw == null)
                {
                }
                else
                {
                    //檢查密碼格式是否正確
                    if (!accountService.IsValidPassword(employeeVM.EmployeePsw))
                    {
                        throw new Exception("密碼長度需介於8~16碼，需包含大小寫及底線");
                    }
                    //確認密碼是否相同
                    if (employeeVM.EmployeePsw == employeeVM.EmployeePswCfm)
                    {
                        empOgn.EmployeePsw = CommonCodes.sha256_hash(employeeVM.EmployeePsw);
                    }
                    else
                    {
                        throw new Exception("密碼確認與密碼不相同");
                    }
                }
                string stat = db.updateEmployee(empOgn);
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

        public Employee VMtoEmployee(EmployeeVM employeeVM)
        {
            Employee ret = new Employee();

            ret.EmployeeID = employeeVM.EmployeeID != null ? employeeVM.EmployeeID.Value : 0;
            ret.AccountName = employeeVM.AccountName;
            ret.EmployeeName = employeeVM.EmployeeName;
            ret.EmployeeSex = employeeVM.EmployeeSex;
            ret.EmployeePsw = CommonCodes.sha256_hash(employeeVM.EmployeePsw);
            ret.MaritalStatus = employeeVM.MaritalStatus;
            ret.EmployeeDoB = employeeVM.EmployeeDoB.Value;
            ret.JobTitle = employeeVM.JobTitle;
            ret.IdentityCard = employeeVM.IdentityCard;
            ret.Address = employeeVM.Address;
            ret.PhoneNumber = employeeVM.PhoneNumber;
            ret.HomePhoneNumber = employeeVM.HomePhoneNumber;
            ret.Department = employeeVM.Department;
            ret.EmergencyContact = employeeVM.EmergencyContact;
            ret.EmergencyPhoneNumber = employeeVM.EmergencyPhoneNumber;
            ret.EmergencyContactRelationship = employeeVM.EmergencyContactRelationship;
            ret.EmploymentStatus = employeeVM.EmploymentStatus;
            ret.RoleID = employeeVM.RoleID.Value;

            return ret;
        }

        public JObject IsROCID(string id)
        {
            bool isValid;
            if (id == null)
            {
                isValid = false;
            }
            else
            {
                id = id.ToUpper();
                Regex regex = new Regex(@"^([A-Z])([1-2]\d{8})$");
                Match match = regex.Match(id);
                if (!match.Success)
                {
                    isValid = false;
                }
                else
                {
                    ///建立字母對應表(A~Z)
                    ///A=10 B=11 C=12 D=13 E=14 F=15 G=16 H=17 J=18 K=19 L=20 M=21 N=22
                    ///P=23 Q=24 R=25 S=26 T=27 U=28 V=29 X=30 Y=31 W=32  Z=33 I=34 O=35 
                    string alphabet = "ABCDEFGHJKLMNPQRSTUVXYWZIO";
                    string transferIdNo = $"{(alphabet.IndexOf(match.Groups[1].Value) + 10)}" +
                                          $"{match.Groups[2].Value}";
                    int[] idNoArray = transferIdNo.ToCharArray()
                                                  .Select(c => Convert.ToInt32(c.ToString()))
                                                  .ToArray();
                    int sum = idNoArray[0];
                    int[] weight = new int[] { 9, 8, 7, 6, 5, 4, 3, 2, 1, 1 };
                    for (int i = 0; i < weight.Length; i++)
                    {
                        sum += weight[i] * idNoArray[i + 1];
                    }
                    isValid = (sum % 10 == 0);
                }
            }
            JObject ret = new JObject { { "isValid", isValid } };

            return ret;
        }

        public bool IsIdNotDuplicate(string idNo)
        {
            bool ret = false;
            if (!db.getEmployees().Where(x => x.IdentityCard == idNo).Any())
            {
                ret = true;
            }

            return ret;
        }

        public JObject IsDoBValid(string dob)
        {
            bool ret = false;
            DateTime dateTime;
            bool parse = DateTime.TryParse(dob, out dateTime);
            if (parse)
            {
                DateTime today = DateTime.Now;
                TimeSpan span = today - dateTime;
                if (span.Days > (365 * 15))
                {
                    ret = true;
                }
            }

            return new JObject() { { "isValid", ret } };
        }
    }
}