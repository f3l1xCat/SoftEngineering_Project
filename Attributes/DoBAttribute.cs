using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace SE_No1.Attributes
{
    public class DoBAttribute: ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            bool ret = false;
            DateTime dateTime = (DateTime)value;
            DateTime today = DateTime.Now;
            TimeSpan span = today - dateTime;
            if (span.Days > (365 * 15))
            {
                ret = true;
            }

            return ret;
        }
    }
}