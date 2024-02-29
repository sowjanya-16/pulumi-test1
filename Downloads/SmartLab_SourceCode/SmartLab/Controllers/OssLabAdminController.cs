using LabBookingWrap;
using LC_Reports_V1.Models;
using Newtonsoft.Json;
using System;
using NLog;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Threading;
using ClosedXML.Excel;
using System.Globalization;
using System.Data.OleDb;
using System.Web;
using System.Configuration;
using System.Web.Script.Serialization;
using static LC_Reports_V1.Models.OssLabClass;
using System.Data.Entity;

namespace LC_Reports_V1.Controllers
{
    /// <summary>
    /// Controller class for LabCar View
    /// </summary>
    [Authorize(Users = @"apac\din2cob,apac\rma5cob,apac\ak71kor,apac\chb1kor,apac\oig1cob,apac\mae9cob,apac\ghb1cob,apac\SIF1COB")]
    public class OssLabAdminController : Controller
    {

        private SqlConnection con;

        private void connection()
        {

            //var datasource = @"BANEN1093154\SQLEXPRESSESPIS5";//your server
            //var database = "BookingServerReplica"; //your database name
            //var username = "jov6cob"; //username of server to connect
            //var password = "serveruser@123"; //password

            ////your connection string 
            //string connString = @"Data Source=" + datasource + ";Initial Catalog="
            //            + database + ";Persist Security Info=True;User ID=" + username + ";Password=" + password;

            string constring = ConfigurationManager.ConnectionStrings["dbConnection"].ToString();
            con = new SqlConnection(constring);
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

        public ActionResult Index()
        {
            string NTID = "";
            connection();
            string qry = " Select isnull(NTID,'') as NTID from Oss_Admin where NTID = '" + User.Identity.Name.Split('\\')[1].Trim() + "'";
            OpenConnection();
            SqlCommand command = new SqlCommand(qry, con);
            SqlDataReader dr = command.ExecuteReader();
            if (dr.HasRows)
            {
                dr.Read();
                NTID = dr["NTID"].ToString();

            }
            else
            {
                NTID = "";
            }
            dr.Close();
            CloseConnection();

            if (NTID == "")
            {
                //throw new HttpException(404, "Sorry! Current user is not authorised to access this view!");
                return Content("Sorry! Current user is not authorised to access this view!");
            }
            else
            {
                return View();
            }
            
        }

        [HttpPost]
        public ActionResult GetLabDetails()
        {
            //string NTID = "";
            //connection();
            //string qry = " Select isnull(NTID,'') as NTID from Oss_Admin where NTID = '" + User.Identity.Name.Split('\\')[1].Trim() + "'";
            //OpenConnection();
            //SqlCommand command = new SqlCommand(qry, con);
            //SqlDataReader dr = command.ExecuteReader();
            //if  (dr.HasRows)
            //{
            //    dr.Read();
            //    NTID = dr["NTID"].ToString();
               
            // }
            //else
            //{
            //    NTID = "";
            //}
            //dr.Close();
            //CloseConnection();

            //if (NTID == "")
            //{
            //    return Json(new { success = false, message = "Sorry! Current user is not authorised to access this view!" }, JsonRequestBehavior.AllowGet);
            //}
             
            //else
            //{


                connection();
                DataTable dt = new DataTable();
                string Query = " Select Comp_DisplayName as LabId,FQDN,Location,Lab, Project_Change as ProjectChange, Current_Project as CurrentProject  ,Category , Type, Responsible , Setup_Type as SetupType  from OSS_LabAdmin where isnull(Lab_Name,'')  <> '' order by Comp_DisplayName, ComputerID  ";
                // string Query = " Select Comp_DisplayName as LabId,FQDN,Location,Lab, Project_Change as ProjectChange, Current_Project as CurrentProject  ,Category , Type, Responsible , Setup_Type as SetupType, From_Week as FromWeek, To_Week as ToWeek from OSS_LabAdmin where isnull(Lab_Name,'')  <> '' ";

                //string Query = " Select Lab_Name as LabId,Lab, convert(varchar,Project_Change,23) as ProjectChange, Current_Project as CurrentProject  ,Category , Type, Responsible , Setup_Type as SetupType, Convert(nvarchar,From_Week,103) as FromWeek, Convert(nvarchar,To_Week,103) as ToWeek from OSS_LabAdmin where isnull(Lab_Name,'')  <> '' ";

                //string Query = " Select Lab_Name as LabId,Lab, Project_Change as ProjectChange, Current_Project as CurrentProject  ,Category , Type, Responsible , Setup_Type as SetupType, From_Week as FromWeek, To_Week as ToWeek from OSS_LabAdmin where isnull(Lab_Name,'')  <> '' ";

                OpenConnection();
                SqlCommand cmd = new SqlCommand(Query, con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                CloseConnection();

                List<OssLabDetails> Ossresult = new List<OssLabDetails>();

                foreach (DataRow row in dt.Rows)
                {
                    OssLabDetails item = new OssLabDetails();
                    item.LabId = row["LabId"].ToString();
                    item.FQDN = row["FQDN"].ToString();
                    item.Location = row["Location"].ToString();
                    item.Lab = row["Lab"].ToString();
                    //item.ProjectChange = Convert.ToDateTime(row["ProjectChange"]).ToString("yyyy-MM-dd");
                    //if (row["ProjectChange"].ToString() == "1900-01-01")
                    //{
                    //    item.ProjectChange = "";
                    //}
                    //else
                    //{
                    //    item.ProjectChange = row["ProjectChange"].ToString();
                    //}
                    item.ProjectChange = row["ProjectChange"].ToString();
                    item.CurrentProject = row["CurrentProject"].ToString();
                    item.Category = row["Category"].ToString();
                    item.Type = row["Type"].ToString();
                    item.Responsible = row["Responsible"].ToString();
                    item.SetupType = row["SetupType"].ToString();
                    //item.FromWeek = row["FromWeek"].ToString();
                    //item.ToWeek = row["ToWeek"].ToString();
                    Ossresult.Add(item);
                }



                //JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
                //List<Dictionary<string, object>> parentRow = new List<Dictionary<string, object>>();
                //Dictionary<string, object> childRow;
                //foreach (DataRow row in dt.Rows)
                //{
                //    childRow = new Dictionary<string, object>();
                //    foreach (DataColumn col in dt.Columns)
                //    {
                //        childRow.Add(col.ColumnName, row[col]);
                //    }
                //    parentRow.Add(childRow);
                //}

                //jsSerializer.MaxJsonLength = Int32.MaxValue;

                //var resultData = parentRow;
                //var result = new ContentResult
                //{
                //    Content = jsSerializer.Serialize(resultData),
                //    ContentType = "application/json"
                //};

                //JsonResult result1 = Json(result);
                //result1.MaxJsonLength = Int32.MaxValue;
                //result1.JsonRequestBehavior = JsonRequestBehavior.AllowGet;


                //return Json(new { data = result1, JsonRequestBehavior.AllowGet });

                return Json(new { data = Ossresult, success = true, JsonRequestBehavior.AllowGet });
            //}

        }

        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonResult()
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior,
                MaxJsonLength = Int32.MaxValue
            };
        }

        public ActionResult DownloadTemplate()
        {
            string folderPath = Server.MapPath("~/Templates/");
            string filename = "OssMasterListTemplate.xlsx";
            var filePath = folderPath + filename;
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, filename);
        }

        [HttpPost]
        public ActionResult UpdateLabDetails(OssLabDetails item)
        {
            string Query = "";
            int result = 0;
            connection();
            if (item.ProjectChange == null)
                item.ProjectChange = "";
            if (item.Lab == null)
                item.Lab = "";
            if (item.Category == null)
                item.Category = "";
            if (item.CurrentProject == null)
                item.CurrentProject = "";
            if (item.Type == null)
                item.Type = "";
            if (item.SetupType == null)
                item.SetupType = "";
            if (item.Responsible == null)
                item.Responsible = "";
            Query += " Exec [dbo].[InsertOssLabAdmin] '" + item.LabId.ToString() + "','" + item.FQDN.ToString() + "','" + item.Location.ToString() + "','" + item.Lab.ToString() + "','" + item.ProjectChange.ToString() + "','" + item.CurrentProject.ToString() + "','" + item.Category.ToString() + "','" + item.Type.ToString() + "','" + item.Responsible.ToString() + "','" + item.SetupType.ToString() + "'";
           // Query += " Exec [dbo].[InsertOssLabAdmin] '" + item.LabId.ToString() + "','" + item.FQDN.ToString() + "','" + item.Location.ToString() + "','" + item.Lab.ToString() + "','"  + item.CurrentProject.ToString() + "','" + item.Category.ToString() + "','" + item.Type.ToString() + "','" + item.Responsible.ToString() + "','" + item.SetupType.ToString() + "'";

            OpenConnection();
            SqlCommand command = new SqlCommand(Query, con);
            command.ExecuteNonQuery();
            CloseConnection();
            return Json("", JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult ValidateLabID(string labid)
        {
            //string Query = "";
            //int result = 0;
            //connection();

            ////Query += " Exec [dbo].[InsertOssLabAdmin] '" + item.LabId.ToString() + "','" + item.Lab.ToString() + "','" + item.ProjectChange.ToString() + "','" + item.CurrentProject.ToString() + "','" + item.Category.ToString() + "','" + item.Type.ToString() + "','" + item.Responsible.ToString() + "','" + item.SetupType.ToString() + "','" + item.FromWeek.ToString() + "','" + item.ToWeek.ToString() + "'";

            //OpenConnection();
            //SqlCommand command = new SqlCommand(Query, con);
            //command.ExecuteNonQuery();
            //CloseConnection();
            return Json("", JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase postedFile)
        {

            try
            {
                if (postedFile != null)
                {

                    string fileExtension = Path.GetExtension(postedFile.FileName);

                    //Validate uploaded file and return error.
                    if (fileExtension != ".xls" && fileExtension != ".xlsx")
                    {

                        return Json(new {success=false, errormsg = "Please select the excel file with.xls or.xlsx extension" });

                    }

                    string folderPath = Server.MapPath("~/UploadedFiles/");
                    //Check Directory exists else create one
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    //Save file to folder
                    var filePath = folderPath + Path.GetFileName(postedFile.FileName);
                    postedFile.SaveAs(filePath);

                    //Get file extension

                    string excelConString = "";

                    //Get connection string using extension 
                    switch (fileExtension)
                    {
                        //If uploaded file is Excel 1997-2007.
                        case ".xls":
                            excelConString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR=YES'";
                            break;
                        //If uploaded file is Excel 2007 and above
                        case ".xlsx":
                            excelConString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 8.0;HDR=YES'";
                            break;
                    }

                    //Read data from first sheet of excel into datatable
                    DataTable dt = new DataTable();
                    excelConString = string.Format(excelConString, filePath);

                    using (OleDbConnection excelOledbConnection = new OleDbConnection(excelConString))
                    {
                        using (OleDbCommand excelDbCommand = new OleDbCommand())
                        {
                            using (OleDbDataAdapter excelDataAdapter = new OleDbDataAdapter())
                            {
                                excelDbCommand.Connection = excelOledbConnection;

                                excelOledbConnection.Open();
                                //Get schema from excel sheet
                                //DataTable excelSchema = GetSchemaFromExcel(excelOledbConnection);
                                DataTable excelSchema = excelOledbConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                                //Get sheet name
                                string sheetName = excelSchema.Rows[0]["TABLE_NAME"].ToString();//give worksheetnames
                                excelOledbConnection.Close();

                                //Read Data from First Sheet.
                                excelOledbConnection.Open();
                                excelDbCommand.CommandText = "SELECT * From [" + sheetName + "]";
                                excelDataAdapter.SelectCommand = excelDbCommand;
                                //Fill datatable from adapter
                                excelDataAdapter.Fill(dt);
                                excelOledbConnection.Close();
                            }
                        }
                    }

                    int errcount = 0;
                    int result = 0;
                    string Query = "";
                    
                    foreach (DataRow row in dt.Rows)
                    {
                        if (((row[0] == DBNull.Value) && (row[1] == DBNull.Value) && (row[2] == DBNull.Value) && (row[3] == DBNull.Value) && (row[4] == DBNull.Value) && (row[5] == DBNull.Value) && (row[6] == DBNull.Value) && (row[7] == DBNull.Value) && (row[8] == DBNull.Value) && (row[9] == DBNull.Value) ) || (String.IsNullOrWhiteSpace(row[0].ToString()) && String.IsNullOrWhiteSpace(row[1].ToString()) && String.IsNullOrWhiteSpace(row[2].ToString()) && String.IsNullOrWhiteSpace(row[3].ToString()) && String.IsNullOrWhiteSpace(row[4].ToString()) && String.IsNullOrWhiteSpace(row[5].ToString()) && String.IsNullOrWhiteSpace(row[6].ToString()) && String.IsNullOrWhiteSpace(row[7].ToString()) && String.IsNullOrWhiteSpace(row[8].ToString()) && String.IsNullOrWhiteSpace(row[9].ToString())))
                        {
                            continue;
                        }
                        else
                        {

                            if (String.IsNullOrWhiteSpace(row[0].ToString()) || row[0] == DBNull.Value)
                            {
                                return Json(new { success = false, errormsg = "Please check the lab names" });
                            }

                            Query += " Exec [dbo].[InsertOssLabAdmin] '" + row[0].ToString() + "','" + row[1].ToString() + "','" + row[2].ToString() + "','" + row[3].ToString() + "','" + row[4].ToString() + "','" + row[5].ToString() + "','" + row[6].ToString() + "','" + row[7].ToString() + "','" + row[8].ToString() + "','" + row[9].ToString()  + "' ";

                        }
                    }

                    OpenConnection();
                    SqlCommand command = new SqlCommand(Query, con);
                    command.ExecuteNonQuery();
                    CloseConnection();
                }
            }
            catch (Exception ex)
            {                
                CloseConnection();

                return Json(new { success = false, errormsg = ex.Message.ToString() });
            }
            finally
            {

            }
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult TimeConfigurer(string night_fromtime, string night_totime, string weekend_fromtime, string weekend_totime)
        {
            TimeConfigurationAttributes tcattributes = new TimeConfigurationAttributes();
            using (BookingServerReplicaEntities db = new BookingServerReplicaEntities())
            {
                TimeConfig timeConfig = new TimeConfig();
                timeConfig.Night_From_Time = night_fromtime.Split(' ')[0].Trim();
                timeConfig.Night_To_Time = night_totime.Split(' ')[0].Trim();
                timeConfig.Weekend_From_Time = weekend_fromtime.Split(' ')[0].Trim();
                timeConfig.Weekend_To_Time = weekend_totime.Split(' ')[0].Trim();
                timeConfig.Date_Modified = DateTime.Now;
                if (db.TimeConfigs.Count() == 0)
                {
                    db.TimeConfigs.Add(timeConfig);
                    db.SaveChanges();
                }
                else
                {
                    timeConfig.SNo = 1;
                    db.Entry(timeConfig).State = EntityState.Modified;
                    db.SaveChanges();
                }

            }

            return Json(new { data = tcattributes }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult ReloadTimeConfigurer()
        {
            TimeConfigurationAttributes tcattributes = new TimeConfigurationAttributes();
            using (BookingServerReplicaEntities db = new BookingServerReplicaEntities())
            {
                var data = db.TimeConfigs.FirstOrDefault(x => x.SNo == 1);
                tcattributes.SNo = data.SNo;
                tcattributes.night_fromtime = data.Night_From_Time;
                tcattributes.night_totime = data.Night_To_Time;
                tcattributes.weekend_fromtime = data.Weekend_From_Time;
                tcattributes.weekend_totime = data.Weekend_To_Time;

                return Json(new { data = tcattributes }, JsonRequestBehavior.AllowGet);
            }

        }


        public ActionResult MailConfig()
        {
            connection();
            DataTable dt = new DataTable();
            string Query = " Select OSS_LabAdmin.ComputerID,Comp_DisplayName as CompName, Responsible, L1_Responsible,L2_LMT,L2_EM,L2_SWPCM,L2_SWTC,L2_SysTC,L3_DH1,L3_DH2,OSS_MailConfig.ModifiedUser from OSS_LabAdmin left join OSS_MailConfig on OSS_LabAdmin.ComputerID = OSS_MailConfig.ComputerID where isnull(OSS_LabAdmin.ComputerID,0) <> 0 order by Comp_DisplayName ";
            OpenConnection();
            SqlCommand cmd = new SqlCommand(Query, con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            CloseConnection();

            List<EmailConfig> configresult = new List<EmailConfig>();

            foreach (DataRow row in dt.Rows)
            {
                EmailConfig item = new EmailConfig();
                item.ComputerID = Convert.ToInt32(row["ComputerID"]);
                item.DisplayName = row["CompName"].ToString();
                item.Responsible = row["Responsible"].ToString();
                item.ResNTID = row["L1_Responsible"].ToString();
                item.LMT = row["L2_LMT"].ToString();
                item.EM = row["L2_EM"].ToString();
                item.SWPCM = row["L2_SWPCM"].ToString();
                item.SWTC = row["L2_SWTC"].ToString();
                item.SysTC = row["L2_SysTC"].ToString();
                item.DH1 = row["L3_DH1"].ToString();
                item.DH2 = row["L3_DH2"].ToString();
                //item.ModifiedUser = row["ModifiedUser"].ToString();
                configresult.Add(item);
            }

            EmailDetails data = new EmailDetails();
            data.ConfigList = configresult;
            return View(data);
        }


        [HttpPost]
        public ActionResult MailConfig(HttpPostedFileBase postedFile)
        {

            try
            {
                if (postedFile != null)
                {

                    string fileExtension = Path.GetExtension(postedFile.FileName);

                    //Validate uploaded file and return error.
                    if (fileExtension != ".xls" && fileExtension != ".xlsx")
                    {

                        return Json(new { success = false, errormsg = "Please select the excel file with.xls or.xlsx extension" });

                    }

                    string folderPath = Server.MapPath("~/UploadedFiles/");
                    //Check Directory exists else create one
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    //Save file to folder
                    var filePath = folderPath + Path.GetFileName(postedFile.FileName);
                    postedFile.SaveAs(filePath);

                    //Get file extension

                    string excelConString = "";

                    //Get connection string using extension 
                    switch (fileExtension)
                    {
                        //If uploaded file is Excel 1997-2007.
                        case ".xls":
                            excelConString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR=YES'";
                            break;
                        //If uploaded file is Excel 2007 and above
                        case ".xlsx":
                            excelConString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 8.0;HDR=YES'";
                            break;
                    }

                    //Read data from first sheet of excel into datatable
                    DataTable dt = new DataTable();
                    excelConString = string.Format(excelConString, filePath);

                    using (OleDbConnection excelOledbConnection = new OleDbConnection(excelConString))
                    {
                        using (OleDbCommand excelDbCommand = new OleDbCommand())
                        {
                            using (OleDbDataAdapter excelDataAdapter = new OleDbDataAdapter())
                            {
                                excelDbCommand.Connection = excelOledbConnection;

                                excelOledbConnection.Open();
                                //Get schema from excel sheet
                                //DataTable excelSchema = GetSchemaFromExcel(excelOledbConnection);
                                DataTable excelSchema = excelOledbConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                                //Get sheet name
                                string sheetName = excelSchema.Rows[0]["TABLE_NAME"].ToString();//give worksheetnames
                                excelOledbConnection.Close();

                                //Read Data from First Sheet.
                                excelOledbConnection.Open();
                                excelDbCommand.CommandText = "SELECT * From [" + sheetName + "]";
                                excelDataAdapter.SelectCommand = excelDbCommand;
                                //Fill datatable from adapter
                                excelDataAdapter.Fill(dt);
                                excelOledbConnection.Close();
                            }
                        }
                    }

                    int errcount = 0;
                    string Query = "";
                    int result = 0;
                    connection();

                    dt.Rows.Remove(dt.Rows[0]);
                    dt.AcceptChanges();
                    foreach (DataRow row in dt.Rows)
                    {
                        
                        if (((row[0] == DBNull.Value) && (row[1] == DBNull.Value) && (row[2] == DBNull.Value) && (row[3] == DBNull.Value) && (row[4] == DBNull.Value) && (row[5] == DBNull.Value) && (row[6] == DBNull.Value) && (row[7] == DBNull.Value) && (row[8] == DBNull.Value) && (row[9] == DBNull.Value)) || (String.IsNullOrWhiteSpace(row[0].ToString()) && String.IsNullOrWhiteSpace(row[1].ToString()) && String.IsNullOrWhiteSpace(row[2].ToString()) && String.IsNullOrWhiteSpace(row[3].ToString()) && String.IsNullOrWhiteSpace(row[4].ToString()) && String.IsNullOrWhiteSpace(row[5].ToString()) && String.IsNullOrWhiteSpace(row[6].ToString()) && String.IsNullOrWhiteSpace(row[7].ToString()) && String.IsNullOrWhiteSpace(row[8].ToString()) && String.IsNullOrWhiteSpace(row[9].ToString())))
                        {
                            continue;
                        }
                        else
                        {

                            if (String.IsNullOrWhiteSpace(row[0].ToString()) || row[0] == DBNull.Value)
                            {
                                return Json(new { success = false, errormsg = "Please check the lab names" });
                            }
                            string user = User.Identity.Name.Split('\\')[1].ToUpper();
                            Query += " Exec [dbo].[InsertMailConfig] 0,'" + row[0].ToString() + "','" + row[2].ToString() + "','" + row[3].ToString() + "','" + row[4].ToString() + "','" + row[5].ToString() + "','" + row[6].ToString() + "','" + row[7].ToString() + "','" + row[8].ToString() + "','" + row[9].ToString() + "' ; ";

                        }
                    }

                    OpenConnection();
                    SqlCommand command = new SqlCommand(Query, con);
                    command.ExecuteNonQuery();
                    CloseConnection();
                }
            }
            catch (Exception ex)
            {
                CloseConnection();

                return Json(new { success = false, errormsg = ex.Message.ToString() });
            }
            finally
            {

            }
            return Json(new { success = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult UpdateMailDetails(EmailConfig item)
        {
            string Query = "";
            int result = 0;
            connection();

            if (item.DisplayName == null)
                item.DisplayName = "";
            if (item.Responsible == null)
                item.Responsible = "";
            if (item.ResNTID == null)
                item.ResNTID = "";
            if (item.LMT == null)
                item.LMT = "";
            if (item.EM == null)
                item.EM = "";
            if (item.SWPCM == null)
                item.SWPCM = "";
            if (item.SWTC == null)
                item.SWTC = "";
            if (item.SysTC == null)
                item.SysTC = "";
            if (item.DH1 == null)
                item.DH1 = "";
            if (item.DH2 == null)
                item.DH2 = "";
            

        Query += " Exec [dbo].[InsertMailConfig] '" + item.ComputerID.ToString() + "','" + item.DisplayName.ToString() + "','" + item.ResNTID.ToString() + "','" + item.LMT.ToString() + "','" + item.EM.ToString() + "','" + item.SWPCM.ToString() + "','" + item.SWTC.ToString() + "','" + item.SysTC.ToString() + "','" + item.DH1.ToString() + "','" + item.DH2.ToString() + "' ";
            // Query += " Exec [dbo].[InsertOssLabAdmin] '" + item.LabId.ToString() + "','" + item.FQDN.ToString() + "','" + item.Location.ToString() + "','" + item.Lab.ToString() + "','"  + item.CurrentProject.ToString() + "','" + item.Category.ToString() + "','" + item.Type.ToString() + "','" + item.Responsible.ToString() + "','" + item.SetupType.ToString() + "'";

            OpenConnection();
            SqlCommand command = new SqlCommand(Query, con);
            command.ExecuteNonQuery();
            CloseConnection();
            return Json("", JsonRequestBehavior.AllowGet);
        }



        public class TimeConfigurationAttributes
        {
            public int SNo { get; set; }
            public string night_fromtime { get; set; }
            public string night_totime { get; set; }
            public string weekend_fromtime { get; set; }
            public string weekend_totime { get; set; }


        }
    }
}