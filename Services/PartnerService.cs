using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SE_No1.Models;
using SE_No1.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace SE_No1.Services
{
    public class PartnerService
    {
        DB db = new DB();
        Result result = new Result();
        static string CorporateType = "";

        public JObject LoadAllData(string draw, string start, string length, string sortColumn, string sortColumnDir, string searchValue, string corporateType)
        {
            try
            {
                CorporateType = corporateType;

                //Paging Size (10,20,50,100)    
                int pageSize = length != null ? Convert.ToInt32(length) : 0;
                int skip = start != null ? Convert.ToInt32(start) : 0;
                int recordsTotal = 0;

                //取得合作夥伴資料By合作夥伴種類  
                var partnerData = db.getPartnerByCorporateType(corporateType);

                //排序    
                if (!(string.IsNullOrEmpty(sortColumn) && string.IsNullOrEmpty(sortColumnDir)))
                {
                    partnerData = partnerData.OrderBy(sortColumn + " " + sortColumnDir);
                }

                //搜尋條件by合作夥伴名稱    
                if (!string.IsNullOrEmpty(searchValue))
                {
                    partnerData = partnerData.Where(m => m.CorporateName.ToLower().Contains(searchValue.ToLower()));
                }

                //total number of rows count     
                recordsTotal = partnerData.Count();

                //Paging     
                var data = partnerData.Skip(skip).Take(pageSize).ToList();

                //Returning Json Data
                JObject ret = new JObject() { { "draw", draw }, { "recordsFiltered", recordsTotal }, { "recordsTotal", recordsTotal } };
                ret["data"] = JToken.FromObject(data);
                return ret;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 取得合作夥伴資料By合作夥伴種類
        /// </summary>
        /// <param name="corporateType">合作夥伴種類</param>
        /// <returns></returns>
        public JArray GetDataList(string corporateType)
        {
            JArray result = new JArray();
            List<Partner> partnerList = db.getPartnerByCorporateType(corporateType).ToList();
            foreach (Partner partner in partnerList)
            {
                JObject r = new JObject()
                {
                    {"CorporateID", partner.CorporateID },
                    {"CorporateName", partner.CorporateName},
                    {"CorporatePrinciple", partner.CorporatePrinciple},
                    {"CorporateDescription", partner.CorporateDescription},
                    {"CorporateAddress", partner.CorporateAddress},
                    {"CorporateTaxIDNo", partner.CorporateTaxIDNo},
                    {"CorporateType", partner.CorporateType}
                };

                result.Add(r);
            }

            return result;
        }

        /// <summary>
        /// 取得合作夥伴資料By合作夥伴ID
        /// </summary>
        /// <param name="corporateID">合作夥伴ID</param>
        /// <returns></returns>
        public JObject GetDataByID(int corporateID)
        {
            Partner partner = db.getPartner().Where(x => x.CorporateID == corporateID).FirstOrDefault();
            JObject ret = JObject.FromObject(partner);

            return ret;
        }

        /// <summary>
        /// 新增合作夥伴資料
        /// </summary>
        /// <param name="partner">合作夥伴資料</param>
        /// <returns></returns>
        public Result Create(Partner partner)
        {
            //輸入資料邏輯判斷，若有誤直接return
            if (!checkPartners(partner, Message.createData)) return result;

            try
            {
                string stat = db.createPartner(partner);
                if (stat != null)
                {
                    result.success = false;
                    result.errorMsg = stat;
                }
                else
                {
                    result.successMsg = Convert.ToString(System.Web.Configuration.WebConfigurationManager.AppSettings["CreateSuccessMsg"]);
                }
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
                result.success = false;
                result.errorMsg = ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 刪除合作夥伴資料
        /// </summary>
        /// <param name="corporateID">合作夥伴資料</param>
        /// <returns></returns>
        public Result Delete(int corporateID)
        {
            Result ret = new Result();

            try
            {
                string stat = db.deletePartner(corporateID);
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

        /// <summary>
        /// 修改合作夥伴資料
        /// </summary>
        /// <param name="partner">合作夥伴資料</param>
        /// <returns></returns>
        public Result Edit(Partner partner)
        {
            //輸入資料邏輯判斷，若有誤直接return
            if (!checkPartners(partner, Message.editData)) return result;

            try
            {
                string stat = db.updatePartner(partner);
                if (stat != null)
                {
                    result.success = false;
                    result.errorMsg = stat;
                }
                else
                {
                    result.successMsg = Convert.ToString(System.Web.Configuration.WebConfigurationManager.AppSettings["UpdateSuccessMsg"]);
                }
            }
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
                result.success = false;
                result.errorMsg = ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 檢查輸入資料是否正確
        /// </summary>
        /// <param name="partner">銷售訂單資料</param>
        /// <param name="type">DB執行的動作 0:新增 1:修改 </param>
        /// <returns></returns>
        private bool checkPartners(Partner partner, int type)
        {
            result.success = true;
            StringBuilder sb = new StringBuilder();

            //檢查合作夥伴資料是否為空
            if (partner == null)
            {
                result.success = false;
                errorMsg("", Message.empty);
                return result.success;
            } 
            
            //若為修改合作夥伴資料時，則多檢查ID是否有值
            if (type.Equals(Message.editData))
            {
                if (partner.CorporateID.Equals(0))
                {
                    result.success = false;
                    errorMsg("合作夥伴編號", Message.empty);
                    return result.success;
                }
            }

            //檢查合作夥伴名稱是否為空
            if (String.IsNullOrEmpty(partner.CorporateName))
            {
                //取得目前合作夥伴種類名稱
                var fieldName = String.Format("{0}名稱", CorporateType);
                sb.Append(errorMsg(fieldName, Message.empty));
            }

            //檢查合作夥伴名稱是超過限制大小
            if (partner.CorporateName.Length > Message.twentyLimit)
            {
                //取得目前合作夥伴種類名稱
                var fieldName = String.Format("{0}名稱", CorporateType);
                sb.Append(errorMsg(fieldName, Message.wrong));
            }

            //檢查描述是否為空
            if (String.IsNullOrEmpty(partner.CorporateDescription))
            {
                sb.Append(errorMsg("描述", Message.empty));
            }
            else 
            {
                if (partner.CorporateDescription.Length > Message.thirtyLimit) 
                {
                    sb.Append(errorMsg("描述", Message.wrong));
                }
            }


            //檢查負責人是否為空
            if (String.IsNullOrEmpty(partner.CorporatePrinciple))
            {
                sb.Append(errorMsg("公司負責人", Message.empty));
            }
            else
            {
                if (partner.CorporatePrinciple.Length > Message.tenLimit)
                {
                    sb.Append(errorMsg("公司負責人", Message.wrong));
                }
            }

            //檢查地址是否為空
            if (String.IsNullOrEmpty(partner.CorporateAddress))
            {
                sb.Append(errorMsg("地址", Message.empty));
            }
            else 
            {
                if (partner.CorporateAddress.Length > Message.twentyLimit)
                {
                    sb.Append(errorMsg("地址", Message.wrong));
                }
            }

            //檢查統編是否為空
            if (String.IsNullOrEmpty(partner.CorporateTaxIDNo))
            {
                sb.Append(errorMsg("統編", Message.empty));
            } 
            else 
            {
                //檢查統編是否為數字
                if (!checkUID(partner.CorporateTaxIDNo))
                {
                    sb.Append(errorMsg("統一編號", Message.wrong));
                }
            }

            //檢查合作夥伴種類是否為空
            if (String.IsNullOrEmpty(partner.CorporateType))
            {
                sb.Append(errorMsg("合作夥伴種類", Message.empty));
            }
            else
            {
                if (partner.CorporateType.Length > Message.tenLimit)
                {
                    sb.Append(errorMsg("合作夥伴種類", Message.wrong));
                }
            }

            result.errorMsg = sb.ToString();

            return result.success;
        }

        /// <summary>
        /// 統編邏輯檢查
        /// </summary>
        /// <param name="idNo">統編</param>
        /// <returns></returns>
        public bool checkUID(string idNo)
        {
            if (idNo == null)
            {
                return false;
            }

            Regex regex = new Regex(@"^\d{8}$");
            Match match = regex.Match(idNo);

            if (!match.Success)
            {
                return false;
            }

            int[] idNoArray = idNo.ToCharArray().Select(c => Convert.ToInt32(c.ToString())).ToArray();
            int[] weight = new int[] { 1, 2, 1, 2, 1, 2, 4, 1 };

            int subSum;     //小和
            int sum = 0;    //總和
            int sumFor7 = 1;

            for (int i = 0; i < idNoArray.Length; i++)
            {
                subSum = idNoArray[i] * weight[i];
                sum += (subSum / 10)   //商數
                     + (subSum % 10);  //餘數                
            }

            if (idNoArray[6] == 7)
            {
                //若第7碼=7，則會出現兩種數值都算對，因此要特別處理。
                sumFor7 = sum + 1;
            }

            return (sum % 10 == 0) || (sumFor7 % 10 == 0);
        }

        /// <summary>
        /// 錯誤訊息
        /// </summary>
        /// <param name="fieldName">欄位名稱</param>
        /// <param name="dataStatus">資料狀態</param>
        /// <returns></returns>
        private string errorMsg(string fieldName, int dataStatus)
        {
            result.success = false;
            return CommonCodes.errorMsg(fieldName, dataStatus, ref result); ;
        }
    }
}