using System;
using System.Drawing;
using System.Web.Http.Results;

namespace SE_No1.Utilities
{
    public class Message
    {
        //Database異動訊息
        public static string createSuccess = "新增成功";
        public static string createFail = "新增失敗";
        public static string editSuccess = "修改成功";
        public static string editFail = "修改失敗";
        public static string deleteSuccess = "刪除成功";
        public static string deleteFail = "刪除失敗";

        //資料狀態
        public static int empty = 0; //資料內容為空
        public static int wrong = 1; //資料有誤
        public static int zero = 2; //資料為零
        public static int negativeNum = 3; //資料為零

        //邏輯判斷錯誤訊息
        public static string dataIsWrong = "資料輸入有誤或超出範圍，請填入正確資訊";
        public static string dataIsEmpty = "資料不可為空";
        public static string dataIsZero = "請輸入1至1000000內之數值";
        public static string dataIsNegativeNum = "資料不可為負";
        public static string dateTimeIsWrong = "日期不可小於2022/1/1或大於現在時間";
        public static string taxIDIsWrong = "格式有誤，請輸入正確之統一編號";

        //資料庫動作
        public static int createData = 0; //正在執行新增資料
        public static int editData = 1; //正在執行修改資料

        //錯誤訊息
        public static string permissionDenied = "您沒有權限執行動作";
        public static string required = "不可為空";

        //檢查邏輯
        public static int numLimit = 1000000;
        public static int thirtyLimit = 30;
        public static int tenLimit = 10;
        public static int twentyLimit = 20;

    }
}