using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SE_No1.Models
{
    public class AccountModel
    {
    }

    public class LoginModel
    {
        public string UserName { get; set; } //使用者名稱
        public string ScrStr { get; set; } //密碼
    }
}