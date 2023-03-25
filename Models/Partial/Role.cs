using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SE_No1.Models
{
    [MetadataType(typeof(CustonRole))]
    public partial class Role
    {
    }

    public class CustonRole
    {
        public int RoleID { get; set; }
        [Required(ErrorMessage = "請輸入角色名稱")]
        public string RoleName { get; set; }
        public Nullable<bool> EmployeeCreate { get; set; }
        public Nullable<bool> EmployeeRetrieve { get; set; }
        public Nullable<bool> EmployeeUpdate { get; set; }
        public Nullable<bool> EmployeeDelete { get; set; }
        public Nullable<bool> SalesCreate { get; set; }
        public Nullable<bool> SalesRetrieve { get; set; }
        public Nullable<bool> SalesUpdate { get; set; }
        public Nullable<bool> SalesDelete { get; set; }
        public Nullable<bool> StockCreate { get; set; }
        public Nullable<bool> StockRetrieve { get; set; }
        public Nullable<bool> StockUpdate { get; set; }
        public Nullable<bool> StockDelete { get; set; }
        public Nullable<bool> PurchasesCreate { get; set; }
        public Nullable<bool> PurchasesRetrieve { get; set; }
        public Nullable<bool> PurchasesUpdate { get; set; }
        public Nullable<bool> PurchasesDelete { get; set; }
        public Nullable<bool> ProductCreate { get; set; }
        public Nullable<bool> ProductRetrieve { get; set; }
        public Nullable<bool> ProductUpdate { get; set; }
        public Nullable<bool> ProductDelete { get; set; }
        public Nullable<bool> RoleCreate { get; set; }
        public Nullable<bool> RoleRetrieve { get; set; }
        public Nullable<bool> RoleUpdate { get; set; }
        public Nullable<bool> RoleDelete { get; set; }
        public Nullable<bool> SupplierCreate { get; set; }
        public Nullable<bool> SupplierRetrieve { get; set; }
        public Nullable<bool> SupplierUpdate { get; set; }
        public Nullable<bool> SupplierDelete { get; set; }
    }
}