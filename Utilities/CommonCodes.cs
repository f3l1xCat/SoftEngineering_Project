using SE_No1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Http.Results;
using System.Web.Mvc;
using System.Web.Security;

namespace SE_No1.Utilities
{
    public static class CommonCodes
    {
        static DB db = new DB();
        public static byte[] sha256_hash(String value)
        {
            StringBuilder Sb = new StringBuilder();

            using (SHA256 hash = SHA256Managed.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(value));

                return result;
            }
        }

        public static bool checkAuth(string controllerName, string actionName)
        {
            bool ret = false;
            int RoleID = Convert.ToInt32(System.Web.HttpContext.Current.Session["RoleID"]);
            try
            {
                List<string> checkList = new List<string> { "Create", "Delete", "Update", "Index" };
                if (checkList.Contains(actionName))
                {
                    actionName = actionName == "Index" ? "Retrieve" : actionName;
                    actionName = actionName == "Edit" ? "Update" : actionName;
                    //取得允許角色列表
                    IQueryable<Role> roles = db.getRoles().Where(controllerName + actionName + " == True");
                    foreach (Role r in roles)
                    {
                        if (r.RoleID == RoleID)
                        {
                            ret = true;
                            break;
                        }
                    }
                }
                else
                {
                    ret = true;
                }
            }
            catch (Exception ex)
            {
                //沒有在列表，放行
                ret = true;
            }

            return ret;
        }

        /// <summary>
        /// 組錯誤訊息
        /// </summary>
        /// <param name="fieldName">欄位名稱</param>
        /// <param name="dataStatus">資料狀態</param>
        /// <param name="result">資料訊息</param>
        public static string errorMsg(string fieldName, int dataStatus, ref Result result)
        {
            var msgWords = "{0}。欄位名稱:{1}。";
            result.success = false;

            //若資料欄位為空
            if (dataStatus.Equals(Message.empty))
            {
                result.errorMsg = String.Format(msgWords, Message.dataIsEmpty, fieldName);
            }
            //若資料欄位有誤
            else if (dataStatus.Equals(Message.wrong))
            {
                if (fieldName == "銷貨日期" || fieldName == "進貨日期")
                {
                   result.errorMsg = String.Format(msgWords, Message.dateTimeIsWrong, fieldName);
                }
                else 
                {
                   result.errorMsg = String.Format(msgWords, Message.dataIsWrong, fieldName);
                }
            }
            //若資料欄位為0
            else if (dataStatus.Equals(Message.zero))
            {
                result.errorMsg = String.Format(msgWords, Message.dataIsZero, fieldName);
            }
            //若資料欄位為負
            else if (dataStatus.Equals(Message.negativeNum))
            {
                result.errorMsg = String.Format(msgWords, Message.dataIsNegativeNum, fieldName);
            }
            else 
            {
                result.errorMsg = Message.dataIsWrong;
            }

            return result.errorMsg;
        }
    }
}