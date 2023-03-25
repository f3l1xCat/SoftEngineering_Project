using SE_No1.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SE_No1.Models.ViewModels
{
    public class EmployeeVM
    {
        public int? EmployeeID { get; set; }
        [Required(ErrorMessage = "請輸入帳號")]
        [StringLength(16, MinimumLength = 8)]
        [RegularExpression("^[a-zA-Z0-9]*$", ErrorMessage = "帳號只能包含英數字")]
        public string AccountName { get; set; }
        [Required(ErrorMessage = "請輸入員工姓名")]
        [StringLength(30)]
        public string EmployeeName { get; set; }
        public string EmployeePsw { get; set; }
        public string EmployeePswCfm { get; set; }
        [Required(ErrorMessage = "請選擇性別")]
        public string EmployeeSex { get; set; }
        public string MaritalStatus { get; set; }
        [Required(ErrorMessage = "請輸入出生日期")]
        [DoB(ErrorMessage = "員工年齡不得小於15歲")]
        public System.DateTime? EmployeeDoB { get; set; }
        [Required(ErrorMessage = "請輸入職稱")]
        public string JobTitle { get; set; }
        [Required(ErrorMessage = "請輸入身分證字號")]
        [ROCID(ErrorMessage = "身份證字號格式錯誤")]
        public string IdentityCard { get; set; }
        [Required(ErrorMessage = "請輸入聯絡地址")]
        [StringLength(100)]
        public string Address { get; set; }
        [Display(Name = "電話號碼")]
        [Required(ErrorMessage = "請輸入電話號碼")]
        [Phone]
        [StringLength(10)]
        public string PhoneNumber { get; set; }
        [Display(Name = "室內電話")]
        [Required(ErrorMessage = "請輸入室內電話")]
        [Phone]
        [StringLength(10)]
        public string HomePhoneNumber { get; set; }
        [Required(ErrorMessage = "請選擇部門")]
        public string Department { get; set; }
        [Required(ErrorMessage = "請輸入緊急聯絡人")]
        [StringLength(30)]
        public string EmergencyContact { get; set; }
        [Display(Name = "緊急連絡人電話")]
        [Required(ErrorMessage = "請輸入緊急連絡人電話")]
        [Phone]
        [StringLength(10)]
        public string EmergencyPhoneNumber { get; set; }
        [Required(ErrorMessage = "請輸入緊急連絡人關係")]
        [StringLength(10)]
        [RegularExpression("^[\u4e00-\u9fa5]+$", ErrorMessage = "緊急聯絡人關係只可輸入中文")]
        public string EmergencyContactRelationship { get; set; }
        [Required(ErrorMessage = "請輸入在職狀態")]
        public string EmploymentStatus { get; set; }
        [Required(ErrorMessage = "請選擇角色")]
        public int? RoleID { get; set; }
    }
}