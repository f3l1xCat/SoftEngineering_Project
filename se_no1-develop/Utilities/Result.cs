using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SE_No1.Utilities
{
    public class Result
    {
        public bool success { get; set; }
        public string errorMsg { get; set; }
        public string successMsg { get; set; }

        public Result()
        {
            success = true;
            errorMsg = null;
            successMsg = null;
        }
    }
}