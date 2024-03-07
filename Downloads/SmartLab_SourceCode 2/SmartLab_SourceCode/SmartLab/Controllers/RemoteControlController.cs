using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Threading;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace LC_Reports_V1.Controllers
{
    public class RemoteControlController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }



        private SqlConnection con;

        private void connection()
        {
            string connstring = ConfigurationManager.ConnectionStrings["dbConnection"].ToString();
            con = new SqlConnection(connstring);
        }

        private void OpenConnection()
        {
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
        }

        private void CloseConnection()
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }

        public ActionResult LabCarView()
        {
            return View();
        }
        public ActionResult LabCarViewBan()
        {
            return View();
        }
        public ActionResult SwitchOnLabcar(string LabName)
        {
            string url = "", content = "", IPAddress = "", RTPCUrl = "", msg = "0";
            int statuscode = 0;
            try
            {

                IPAddress = GetApiConfig(LabName);
                RTPCUrl = "http://" + IPAddress + "/home/CheckRTPCStatus";

                // RTPCUrl = "http://10.169.222.61/home/CheckRTPCStatus";
                //statuscode = GetRTPCStatus(RTPCUrl);

                //if (statuscode == 1)
                //{
                //    msg = "1";
                //}
                //else
                //{
                url = "http://" + IPAddress + "/home/SwitchLabcar?n=1";
                HttpMessageHandler handler = new HttpClientHandler()
                {

                };

                var httpclient = new HttpClient(handler)

                {
                    BaseAddress = new Uri(url),
                    Timeout = new TimeSpan(0, 2, 0)
                };

                httpclient.DefaultRequestHeaders.Add("contenttype", "text/html");

                //this is the key section you were missing    
                //var plaintextbytes = System.Text.Encoding.UTF8.GetBytes("admin:admin");
                //var plaintextbytes = System.Text.Encoding.UTF8.GetBytes("oig1cob:Oig1cob.33726530");
                //string val = System.Convert.ToBase64String(plaintextbytes);
                //httpclient.DefaultRequestHeaders.Add("authorization", "basic " + val);
                HttpResponseMessage response = new HttpResponseMessage();
                response = httpclient.GetAsync(url).Result;

                //if (response.IsSuccessStatusCode)
                //{
                using (StreamReader stream = new StreamReader(response.Content.ReadAsStreamAsync().Result))
                {
                    content = stream.ReadToEnd();
                }

                msg = content.ToString();
                //}
                return Json(new { success = true, data = msg }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                msg = "Failed to get status";
                return Json(new { success = true, data = msg }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult SwitchOnRTPC(string LabName, string LabcarStatus)
        {
            int statuscode = 0;
            string RTPCUrl = "", IPAddress = "", msg = "";
            //if (LabName == "LC124")
            //{
            //    IPAddress = "10.169.231.24";

            //}
            //else if (LabName == "LC136")
            //{
            //    //IPAddress = "10.169.222.61";
            //    IPAddress = "10.169.223.107";

            //}
            IPAddress = GetApiConfig(LabName);
            RTPCUrl = "http://" + IPAddress + "/home/CheckRTPCStatus";
            //if (LabcarStatus == "0")
            //{
            int i = 0;
            while (i < 4)
            {

                statuscode = GetRTPCStatus(RTPCUrl);
                i++;
                if (statuscode == 1)
                {
                    msg = "1";
                    break;
                }
                else
                {
                    //if (i < 4)
                    //    msg = "Failed to get status";
                }
                Thread.Sleep(60000);
            }
            //}

            return Json(new { success = true, data = msg }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult SwitchOffLabcar(string LabName)
        {
            string url = "", content = "", IPAddress = "", RTPCUrl = "", msg = "0";
            int statuscode = 0;
            try
            {
                //if (LabName == "LC124")
                //{
                //    IPAddress = "10.169.231.24";

                //}
                //else if (LabName == "LC136")
                //{
                //    //IPAddress = "10.169.222.61";
                //    IPAddress = "10.169.223.107";

                //}
                IPAddress = GetApiConfig(LabName);
                RTPCUrl = "http://" + IPAddress + "/home/CheckRTPCStatus";

                //  RTPCUrl = "http://10.169.222.61/home/CheckRTPCStatus";
                statuscode = GetRTPCStatus(RTPCUrl);

                if (statuscode == 1)
                {
                    url = "http://" + IPAddress + "/home/SwitchLabcar?n=0";
                    HttpMessageHandler handler = new HttpClientHandler()
                    {

                    };

                    var httpclient = new HttpClient(handler)

                    {
                        BaseAddress = new Uri(url),
                        Timeout = new TimeSpan(0, 2, 0)
                    };

                    httpclient.DefaultRequestHeaders.Add("contenttype", "text/html");

                    //this is the key section you were missing    
                    //var plaintextbytes = System.Text.Encoding.UTF8.GetBytes("admin:admin");
                    //var plaintextbytes = System.Text.Encoding.UTF8.GetBytes("oig1cob:Oig1cob.33726530");
                    //string val = System.Convert.ToBase64String(plaintextbytes);
                    //httpclient.DefaultRequestHeaders.Add("authorization", "basic " + val);
                    HttpResponseMessage response = new HttpResponseMessage();
                    response = httpclient.GetAsync(url).Result;

                    //if (response.IsSuccessStatusCode)
                    //{
                    using (StreamReader stream = new StreamReader(response.Content.ReadAsStreamAsync().Result))
                    {
                        content = stream.ReadToEnd();
                    }

                    //}

                    if (content == "1")
                    {
                        msg = content;
                    }

                }

                return Json(new { success = true, data = msg }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                msg = "Failed to switch off";
                return Json(new { success = true, data = msg }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult GetStatus(string LabName)
        {
            int statuscode = 0;
            string IPAddress = "", RTPCUrl = "";
            try
            {
                IPAddress = GetApiConfig(LabName);
                RTPCUrl = "http://" + IPAddress + "/home/CheckRTPCStatus";
                statuscode = GetRTPCStatus(RTPCUrl);
                //if (LabName == "LC124")
                //{
                //    RTPCUrl = "http://10.169.231.24/home/CheckRTPCStatus";
                //    statuscode = GetRTPCStatus(RTPCUrl);
                //}
                //else if(LabName=="LC136")
                //{
                //    RTPCUrl = "http://10.169.223.107/home/CheckRTPCStatus";
                //    statuscode = GetRTPCStatus(RTPCUrl);
                //}
                //else
                //{
                //    statuscode = 0;
                //}


                return Json(new { data = statuscode.ToString(), success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = true, data = "Failed to switch off" }, JsonRequestBehavior.AllowGet);
            }


        }
        public int GetRTPCStatus(string url)
        {
            int status = 0;
            try
            {
                HttpMessageHandler handler = new HttpClientHandler()
                {

                };

                var httpclient = new HttpClient(handler)

                {
                    BaseAddress = new Uri(url),
                    Timeout = new TimeSpan(0, 2, 0)
                };

                httpclient.DefaultRequestHeaders.Add("contenttype", "text/html");

                //this is the key section you were missing    
                //var plaintextbytes = System.Text.Encoding.UTF8.GetBytes("admin:admin");
                //var plaintextbytes = System.Text.Encoding.UTF8.GetBytes("oig1cob:Oig1cob.33726530");
                //string val = System.Convert.ToBase64String(plaintextbytes);
                //httpclient.DefaultRequestHeaders.Add("authorization", "basic " + val);
                HttpResponseMessage response = new HttpResponseMessage();
                response = httpclient.GetAsync(url).Result;
                string content = string.Empty;

                using (StreamReader stream = new StreamReader(response.Content.ReadAsStreamAsync().Result))
                {
                    content = stream.ReadToEnd();
                }


                //content = response.Content.Headers.ContentLength.Value.ToString ();
                status = Convert.ToInt16(content);
                return status;
            }
            catch (Exception ex)
            {
                return status;
            }

        }

        public string GetApiConfig(string LabName)
        {
            SqlDataReader dr;
            string IpAddress = "";
            try
            {
                connection();
                string Query = " Select APIConfig as IPAddress from LabCarConfig Where LabName = '" + LabName + "'";
                OpenConnection();
                SqlCommand cmd = new SqlCommand(Query, con);
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    IpAddress = dr["IPAddress"].ToString();
                }
                dr.Close();
                CloseConnection();

                return IpAddress;
            }
            catch (Exception ex)
            {
                return "";
            }

        }
    }
}