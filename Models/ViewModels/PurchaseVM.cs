using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SE_No1.Models.ViewModels
{
    public class PurchaseVM
    {
        public int PurchasesID { get; set; }
        public int PurchasesCount { get; set; }
        public System.DateTime PurchasesTime { get; set; }
        public int FinalPrice { get; set; }
        public string ProductName { get; set; }
        public string CorporateName { get; set; }
    }
}