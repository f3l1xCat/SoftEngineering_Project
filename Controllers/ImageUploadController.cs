using Newtonsoft.Json;
using SE_No1.Models;
using SE_No1.Services;
using SE_No1.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Web;
using System.Web.Mvc;

namespace SE_No1.Controllers
{
    public class ImageUploadController : Controller
    {
        // GET: ImageUpload
        /*public ActionResult Index()
        {
            return View();
        }*/
        public static string serverName = "/Content/Images/uploads/";
        public static string GetIPAddress()
        {            
            string strHostName = System.Net.Dns.GetHostName();
            IPHostEntry ipHostInfo = Dns.GetHostEntry(strHostName);
            //IPAddress ipAddress = ipHostInfo.AddressList[0];

            var ippaddress = ipHostInfo.AddressList.FirstOrDefault(ip => ip.AddressFamily == AddressFamily.InterNetwork);
            return ippaddress.ToString();
        }

        [HttpPost]
        public ActionResult UploadImage()
        {
            Result ret = new Result();
            if (Request.Files.Count > 0)
            {
                try
                {
                    HttpFileCollectionBase files = Request.Files;
                    HttpPostedFileBase file = files[0];
                    string fileName = file.FileName;
                    fileName = Path.Combine(Server.MapPath(serverName), fileName);
                    if (System.IO.File.Exists(fileName))
                    {
                        throw new Exception("File Existed");
                    }
                    file.SaveAs(fileName);
                    ret.successMsg = "File Upload Successfully!";
                }
                catch (Exception ex)
                {
                    ret.success = false;
                    ret.errorMsg = "Error occured." + ex.Message;
                }
            }
            else
            {
                ret.success = false;
                ret.errorMsg = "No files selected!";
            }

            return Content(JsonConvert.SerializeObject(ret), "application/json");
        }
    }
}