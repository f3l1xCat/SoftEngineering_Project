using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SE_No1.Models
{
    [MetadataType(typeof(CustomStock))]
    public partial class Stock
    {
    }

    public class CustomStock
    {
        public int StockID { get; set; }
        [Required(ErrorMessage = "請輸入存貨數量")]
        [RegularExpression("^[0-9]{1,4}$", ErrorMessage = "存貨數量格式錯誤")]
        public int StockCount { get; set; }
        [Required(ErrorMessage = "請選擇貨架編號")]
        public int ShelveID { get; set; }
        [Required(ErrorMessage = "請選擇產品名稱")]
        public int ProductID { get; set; }
        [Required(ErrorMessage = "請選擇倉庫編號")]
        public int StoreHouseID { get; set; }
        [Required(ErrorMessage = "請選擇日期")]
        public System.DateTime StoreTime { get; set; }
    }
}