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
using System.Configuration;
using System.Web.Script.Serialization;
using static LC_Reports_V1.Models.OssLabClass;

namespace LC_Reports_V1.Controllers
{
    public class CPCController : Controller
    {
        // GET: CPC

        //public static string LabTypesPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/LC_LabTypes.json");
        //public static string LabSitesPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/LC_LabSites.json");
        //public static string LabLocationsPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/LC_LabLocations.json");
        //public static string LabInfoPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/LC_LabInfo.json");
        //public static string LabOEMCsvPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/LAB_OEM_Mapping.csv");
        ////public static string LabOEMJsonPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/LAB_OEM_Mapping.json");
        //public List<LCfilterInfo.LabBookingExport> lstExports = new List<LCfilterInfo.LabBookingExport>();
        //public static LCfilterInfo.LabBookingExport objLabExport = new LCfilterInfo.LabBookingExport();
        //public static ReportParameters objLabReport = new ReportParameters();
        public static bool ServerTimeout = false;

        //public static List<SelectListItem> lstLabIDs = new List<SelectListItem>();
        //public static List<SelectListItem> lstLocs = new List<SelectListItem>();
        //public static List<SelectListItem> lstLabTypes = new List<SelectListItem>();
        public static bool refresh = false;
        public static bool reload = false;
        //public static LCfilterInfo.LabBookingExport report = new LCfilterInfo.LabBookingExport();
        public static CPCReportAttributes cpcobj = new CPCReportAttributes();
        

        public static DateTime FromDate = DateTime.Now;
        public static DateTime ToDate = DateTime.Now;


        public ActionResult Index()
        {

            LCfilterInfo lcfilterInfo = new Models.LCfilterInfo();
            return View(lcfilterInfo);

            //return View();
;            
        }

        private SqlConnection con;

        private void connection()
        {
            ////var datasource = @"cob1098672\sqlexpress";//your server
            ////var database = "BookingServerReplica"; //your database name
            ////var username = "espis5"; //username of server to connect
            ////var password = "espis5@12345"; //password

            //var datasource = @"BANEN1093154\SQLEXPRESSESPIS5";//your server
            //var database = "BookingServerReplica"; //your database name
            //var username = "jov6cob"; //username of server to connect
            //var password = "serveruser@12345"; //password

            ////your connection string 
            //string connString = @"Data Source=" + datasource + ";Initial Catalog="
            //            + database + ";Persist Security Info=True;User ID=" + username + ";Password=" + password;

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

        [HttpGet]
        public ActionResult GetLabTypes()
        {
            using (BookingServerReplicaEntities db = new BookingServerReplicaEntities())
            {
                //List<Models.Site> objSites = db.Sites.ToList();
                //List<Models.Location> lstLabLocs = db.Locations.ToList();
                List<Models.LabType> objLabType = db.LabTypes.ToList();
                //List<LabInfo> lstLabDef = db.LabInfoes.ToList();

                List<SelectListItem> lstLabTypes = new List<SelectListItem>();
                objLabType = objLabType.GroupBy(x => x.DisplayName).Select(x => x.First()).ToList(); ;
                foreach (Models.LabType labattr in objLabType)
                {
                    lstLabTypes.Add(new SelectListItem { Text = labattr.DisplayName.Trim(), Value = labattr.DisplayName.Trim() });

                }
                lstLabTypes.Sort((a, b) => a.Value.CompareTo(b.Value));
                return Json(new { data = lstLabTypes }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        public ActionResult GetLocation()
        {
            using (BookingServerReplicaEntities db = new BookingServerReplicaEntities())
            {
                List<Models.Site> objSites = db.Sites.ToList();
                List<Models.Location> lstLabLocs = db.Locations.ToList();

                List<SelectListItem> lstLocations = new List<SelectListItem>();
                foreach (Models.Site sitesattr in objSites)
                {
                    lstLocations.Add(new SelectListItem { Text = sitesattr.DisplayName, Value = sitesattr.RbCode });
                }
                lstLocations.Sort((a, b) => a.Value.CompareTo(b.Value));
                return Json(new { data = lstLocations }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult WeeklyAverage()
        {
            string year = TempData["year"].ToString();
            //string year = Session["year"].ToString();
            AverageMasterDetails Data = new AverageMasterDetails();

            Data.LocationTSG4 = GetLoc("TSG4", year);
            Data.LocationPBOX = GetLoc("PBOX", year);
            Data.LocationMLC = GetLoc("MLC", year);
            Data.LocationACUROT = GetLoc("ACUROT", year);
            
            Data.ProjectTSG4 = GetProject("TSG4", year);
            Data.ProjectPBOX = GetProject("PBOX", year);
            Data.ProjectMLC = GetProject("MLC", year);
            Data.ProjectACUROT = GetProject("ACUROT", year);

            return View(Data);
        }

        public ActionResult WeeklyAveragePrj()
        {
            string year = TempData["year"].ToString();
            //string year = Session["year"].ToString();
            AverageMasterDetails Data = new AverageMasterDetails();

            Data.ProjectTSG4 = GetProject("TSG4", year);
            Data.ProjectPBOX = GetProject("PBOX", year);
            Data.ProjectMLC = GetProject("MLC", year);
            Data.ProjectACUROT = GetProject("ACUROT", year);
            
            return View(Data);
        }

        public ActionResult WeeklyAverageCharts()
        {
            string year = TempData["year"].ToString();
            //string year = Session["year"].ToString();
            AverageMasterDetails data = new AverageMasterDetails();
            data.AvgRegionChart_TSG4 = GetRegionAverage("TSG4", year);
            data.AvgRegionChart_PBOX = GetRegionAverage("PBOX", year);
            data.AvgRegionChart_MLC = GetRegionAverage("MLC", year);
            data.AvgRegionChart_ACUROT = GetRegionAverage("ACUROT", year);

            //data.PrgAvgDE = GetProjectAverageIN("DE");
            //data.PrgAvgJP = GetProjectAverageIN("JP");
            //data.PrgAvgCN = GetProjectAverageIN("CN");
            //data.PrgAvgIN = GetProjectAverageIN("IN");
            //data.PrgAvgVN = GetProjectAverageIN("VN");
            //data.PrgAvgUS = GetProjectAverageIN("US");

            //data.LocAverageDE = GetLocAverage("DE");
            //data.LocAverageJP = GetLocAverage("JP");
            //data.LocAverageCN = GetLocAverage("CN");
            //data.LocAverageIN = GetLocAverage("IN");
            //data.LocAverageVN = GetLocAverage("VN");
            //data.LocAverageUS = GetLocAverage("US");

            return View(data);
        }

        public ActionResult WeeklyAverageCharts_SetUpType(string Loc)
        {
            string year = TempData["year"].ToString();
            //string year = Session["year"].ToString();
            AverageMasterDetails data = new AverageMasterDetails();
            data.PrgAvgIN_TSG4 = GetProjectAverageIN(Loc,"TSG4", year);
            data.PrgAvgIN_PBOX = GetProjectAverageIN(Loc, "PBOX", year);
            data.PrgAvgIN_MLC = GetProjectAverageIN(Loc, "MLC", year);
            data.PrgAvgIN_ACUROT = GetProjectAverageIN(Loc, "ACUROT", year);

            data.LocAverage_TSG4 = GetLocAverage(Loc, "TSG4", year);
            data.LocAverage_PBOX = GetLocAverage(Loc, "PBOX", year);
            data.LocAverage_MLC = GetLocAverage(Loc, "MLC", year);
            data.LocAverage_ACUROT = GetLocAverage(Loc, "ACUROT", year);

            data.Loc = Loc;
            
            return View(data);
        }

        
        private static List<Locations> GetLoc(string SType , string year)
        {
            string Query = "";
            DataSet ds = new DataSet();
            SqlConnection conn = new SqlConnection();
            string constring = ConfigurationManager.ConnectionStrings["dbConnection"].ToString();
            conn = new SqlConnection(constring);
            conn.Open();
            Query = " EXEC [dbo].[GetAverageUtilByLocation] '" + SType + "', '" + year + "';";
            SqlCommand cmd = new SqlCommand(Query, conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            conn.Close();

            List<Locations> locationRes = new List<Locations>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                Locations loc = new Locations();
                loc.Nos = Convert.ToInt32(row["Nos"]);
                loc.Loc = row["Loc"].ToString();
                loc.WeekNo = row["WeekNo"].ToString();
                loc.Value = Convert.ToDouble(row["Value"]);
                loc.Average = Convert.ToDouble(row["Average"]);
                loc.SetupType = row["SetupType"].ToString();
                locationRes.Add(loc);
            }

            return locationRes;
        }

        private static List<Projects> GetProject(string SType, string year)
        {
            string Query = "";
            DataSet ds = new DataSet();
            SqlConnection conn = new SqlConnection();
            string constring = ConfigurationManager.ConnectionStrings["dbConnection"].ToString();
            conn = new SqlConnection(constring);
            conn.Open();
            Query = " EXEC [dbo].[GetAverageUtilByProject] '" + SType + "', '" + year + "' ;";
            SqlCommand cmd = new SqlCommand(Query, conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            conn.Close();

            List<Projects> ProjectRes = new List<Projects>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                Projects prj = new Projects();
                prj.Nos = Convert.ToInt32(row["Nos"]);
                prj.ComputerIds = row["ComputerIds"].ToString();
                prj.Project = row["Project"].ToString();
                prj.WeekNo = row["WeekNo"].ToString();
                prj.Value = Convert.ToDouble(row["Value"]);
                prj.Average = Convert.ToDouble(row["Average"]);
                prj.Location = row["Location"].ToString();
                prj.SetupType = row["SetupType"].ToString();
                ProjectRes.Add(prj);
            }

            return ProjectRes;
        }

        private static List<AverageRegionChart> GetRegionAverage(string setuptype , string year)
        {
            string Query = "";
            DataSet ds = new DataSet();
            SqlConnection conn = new SqlConnection();
            string constring = ConfigurationManager.ConnectionStrings["dbConnection"].ToString();
            conn = new SqlConnection(constring);
            conn.Open();
            Query = " EXEC [dbo].[GetAverageUtilByLocation_Chart] '" + setuptype + "','" + year + "';";
            SqlCommand cmd = new SqlCommand(Query, conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            conn.Close();

            List<AverageRegionChart> regionRes = new List<AverageRegionChart>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                AverageRegionChart avg = new AverageRegionChart();
                avg.WeekNo = row["WeekNo"].ToString();
                avg.CN = Convert.ToDouble(row["CN"]);
                avg.EU = Convert.ToDouble(row["EU"]);
                avg.NA = Convert.ToDouble(row["NA"]);
                avg.RBEI = Convert.ToDouble(row["RBEI"]);
                avg.RBJP = Convert.ToDouble(row["RBJP"]);
                avg.RBVH = Convert.ToDouble(row["RBVH"]);
                regionRes.Add(avg);
            }

            return regionRes;
        }

        private static List<ProjectAverageIN> GetProjectAverageIN(string Loc, string setuptype, string year)
        {
            string Query = "" ;
            DataSet ds = new DataSet();
            SqlConnection conn = new SqlConnection();
            string constring = ConfigurationManager.ConnectionStrings["dbConnection"].ToString();
            conn = new SqlConnection(constring);
            conn.Open();
            Query = " EXEC [dbo].[GetAverageUtilByProject_Chart] '" + Loc + "', '" + setuptype + "', '" + year + "' ; ";
            SqlCommand cmd = new SqlCommand(Query, conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            conn.Close();

            List<ProjectAverageIN> prjRes = new List<ProjectAverageIN>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                ProjectAverageIN avg = new ProjectAverageIN();
                avg.Project = row["Project"].ToString();
                avg.TAverage = Convert.ToDouble(row["TAverage"]);
                avg.DAverage = Convert.ToDouble(row["DAverage"]);
                prjRes.Add(avg);
            }

            return prjRes;
        }

        private static List<LocationAverage> GetLocAverage(string Loc,  string setuptype, string year)
        {
            string Query = "";
            DataSet ds = new DataSet();
            SqlConnection conn = new SqlConnection();
            string constring = ConfigurationManager.ConnectionStrings["dbConnection"].ToString();
            conn = new SqlConnection(constring);
            conn.Open();
            Query = " Select [dbo].[fn_GetLocationAverage] ('" + Loc + "', '" + setuptype + "', '" + year + "') as TAverage ; ";
            SqlCommand cmd = new SqlCommand(Query, conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            conn.Close();

            List<LocationAverage> LRes = new List<LocationAverage>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                LocationAverage avg = new LocationAverage();
                avg.LAverage = Convert.ToInt32(row["TAverage"]);
                LRes.Add(avg);
            }

            return LRes;
        }


        [HttpPost]
        public ActionResult GetTimeConfig()
        {

            connection();
            DataTable dt = new DataTable();

            string Query = " Select Night_From_Time , Night_To_Time , Weekend_From_Time , Weekend_To_Time , Date_Modified  from TimeConfig  ";

            OpenConnection();
            SqlCommand cmd = new SqlCommand(Query, con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            CloseConnection();

            List<OssTimeConfig> OssTimeresult = new List<OssTimeConfig>();


            foreach (DataRow row in dt.Rows)
            {
                OssTimeConfig item = new OssTimeConfig();
                item.Night_From_Time = row["Night_From_Time"].ToString() + " hrs";
                item.Night_To_Time = row["Night_To_Time"].ToString() + " hrs";
                item.Weekend_From_Time = row["Weekend_From_Time"].ToString() + " hrs";
                item.Weekend_To_Time = row["Weekend_To_Time"].ToString() + " hrs";
                item.Date_Modified = row["Date_Modified"].ToString();
                OssTimeresult.Add(item);
            }

            return Json(new { data = OssTimeresult, JsonRequestBehavior.AllowGet });
        }

        [HttpPost]
        public ActionResult SetTempData(string year)
        {
            TempData["year"] = year;
            return new EmptyResult();
        }
        public ActionResult GenerateCPC(double startdate, double enddate, string[] slocation, string[] ssetuptype, string[] stype, string[] sactivity, string[] sfilter, string sview, int year )
        {
            var obj = "";
            DataTable dt = new DataTable();

            TempData["year"] = year.ToString();
            

            //DateTime startDate = new DateTime(DateTime.Now.Year, 01, 01);
            //DateTime endDate = new DateTime(DateTime.Now.Year, 12, 31);

            System.DateTime stDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            stDateTime = stDateTime.AddMilliseconds(startdate).ToLocalTime();

            System.DateTime eDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            eDateTime = eDateTime.AddMilliseconds(enddate).ToLocalTime();

            //DateTime sdate = Convert.ToDateTime(startDate);
            //DateTime edate = Convert.ToDateTime(endDate);

            //DateTime sdate = stDateTime;
            //DateTime edate = eDateTime;

            DateTime sdate = Convert.ToDateTime(stDateTime.ToString("yyyy-MM-dd"));
            DateTime edate = Convert.ToDateTime(eDateTime.ToString("yyyy-MM-dd"));

            //FromDate = Convert.ToDateTime(startDate); 
            //ToDate = Convert.ToDateTime(endDate);

            List<SelectListItem> lstLabIDs = new List<SelectListItem>();
            try
            {
                DataTable typedt = new DataTable();
                typedt.Columns.Add(new DataColumn("Type", typeof(string)));
                typedt.Columns.Add(new DataColumn("Name", typeof(string)));

                foreach (var settype in ssetuptype)
                    typedt.Rows.Add("Lab", settype);


                //DataTable locdt = new DataTable();
                //locdt.Columns.Add(new DataColumn("Location", typeof(string)));

                foreach (var loc in slocation)
                {
                    //locdt.Rows.Add(loc);
                    typedt.Rows.Add("Loc", loc);
                    if (loc == "Ban")
                    {
                        typedt.Rows.Add("Loc", "Cob");
                    }
                }

                if (stype != null) 
                {
                    foreach (var type in stype)
                    {
                        //locdt.Rows.Add(loc);
                        typedt.Rows.Add("Type", type);
                    }
                        
                }


                if (sactivity != null)
                {
                    foreach (var act in sactivity)
                        //locdt.Rows.Add(loc);
                        typedt.Rows.Add("Activity", act);
                }

                if (sfilter != null)
                {
                    foreach (var filter in sfilter)
                        //locdt.Rows.Add(loc);
                        typedt.Rows.Add("Filter", filter);

                }

                //if (sview != null)
                //{
                //    typedt.Rows.Add("View", sview);
                //}
                //else
                //{

                //    typedt.Rows.Add("View", "Date");
                //}
                // populate DataTable from your List here

                //var datasource = @"cob1098672\sqlexpress";//your server
                //var database = "BookingServerReplica"; //your database name
                //var username = "espis5"; //username of server to connect
                //var password = "espis5@12345"; //password

                //var datasource = @"BANEN1093154\SQLEXPRESSESPIS5";//your server
                //var database = "BookingServerReplica"; //your database name
                //var username = "jov6cob"; //username of server to connect
                //var password = "serveruser@12345"; //password

                ////your connection string 
                //string connString = @"Data Source=" + datasource + ";Initial Catalog="
                //            + database + ";Persist Security Info=True;User ID=" + username + ";Password=" + password;

                ////create instanace of database connection
                //SqlConnection conn = new SqlConnection(connString);
                //dt = new DataTable();
                //conn.Open();
                //string Query = " Exec [dbo].[GetCPCSummary] '" + sdate + "','" + edate + "','" + stype + "','" + slocation + "'";
                //SqlCommand cmd = new SqlCommand(Query, conn);
                //SqlDataAdapter da = new SqlDataAdapter(cmd);
                //da.Fill(dt);
                //conn.Close();
                connection();

                SqlCommand command = new SqlCommand();

                if (sview == "Date")
                {
                   
                    command.Connection = con;
                    command.CommandText = "dbo.GetCPCDateSummary";
                    command.CommandType = CommandType.StoredProcedure;


                    // Add the input parameter and set its properties.
                    SqlParameter parameter1 = new SqlParameter();
                    parameter1.ParameterName = "@StartDate";
                    parameter1.SqlDbType = SqlDbType.DateTime;
                    parameter1.Direction = ParameterDirection.Input;
                    parameter1.Value = sdate;

                    SqlParameter parameter2 = new SqlParameter();
                    parameter2.ParameterName = "@EndDate";
                    parameter2.SqlDbType = SqlDbType.DateTime;
                    parameter2.Direction = ParameterDirection.Input;
                    parameter2.Value = edate;


                    SqlParameter parameter3 = new SqlParameter();
                    parameter3.ParameterName = "@LabTypeList";
                    parameter3.SqlDbType = SqlDbType.Structured;
                    parameter3.TypeName = "dbo.LabTypesList";
                    parameter3.Direction = ParameterDirection.Input;
                    parameter3.Value = typedt;

                    //SqlParameter parameter4 = new SqlParameter();
                    //parameter4.ParameterName = "@LocationList";
                    //parameter4.SqlDbType = SqlDbType.Structured;
                    //parameter3.TypeName = "dbo.LocationsList";
                    //parameter4.Direction = ParameterDirection.Input;
                    //parameter4.Value = locdt;


                    // Add the parameter to the Parameters collection.
                    command.Parameters.Add(parameter3);
                    //command.Parameters.Add(parameter4);
                    command.Parameters.Add(parameter1);
                    command.Parameters.Add(parameter2);
                }
                else if( sview == "Week")
                {
                    command.Connection = con;
                    command.CommandText = "dbo.GetCPCWeeklySummary";
                    command.CommandType = CommandType.StoredProcedure;


                    // Add    the input parameter and set its properties.
                    SqlParameter parameter1 = new SqlParameter();
                    parameter1.ParameterName = "@LabTypeList";
                    parameter1.SqlDbType = SqlDbType.Structured;
                    parameter1.TypeName = "dbo.LabTypesList";
                    parameter1.Direction = ParameterDirection.Input;
                    parameter1.Value = typedt;

                    SqlParameter parameter2 = new SqlParameter();
                    parameter2.ParameterName = "@Year";
                    parameter2.SqlDbType = SqlDbType.Int;
                    parameter2.Direction = ParameterDirection.Input;
                    //parameter2.Value = DateTime.Now.Year;
                    parameter2.Value = year;


                    command.Parameters.Add(parameter1);
                    command.Parameters.Add(parameter2);
                }
                else
                {
                    command.Connection = con;
                    command.CommandText = "dbo.GetCPCMonthlySummary";
                    command.CommandType = CommandType.StoredProcedure;


                    // Add the input parameter and set its properties.
                    SqlParameter parameter1 = new SqlParameter();
                    parameter1.ParameterName = "@LabTypeList";
                    parameter1.SqlDbType = SqlDbType.Structured;
                    parameter1.TypeName = "dbo.LabTypesList";
                    parameter1.Direction = ParameterDirection.Input;
                    parameter1.Value = typedt;

                    SqlParameter parameter2 = new SqlParameter();
                    parameter2.ParameterName = "@Year";
                    parameter2.SqlDbType = SqlDbType.Int;
                    parameter2.Direction = ParameterDirection.Input;
                    //parameter2.Value = DateTime.Now.Year;
                    parameter2.Value = year;


                    command.Parameters.Add(parameter1);
                    command.Parameters.Add(parameter2);

                   
                }
                // Open the connection and execute the reader.
                //conn.Open();
                OpenConnection();

                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(dt);
                CloseConnection();

               

                
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }

            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            List<Dictionary<string, object>> parentRow = new List<Dictionary<string, object>>();
            Dictionary<string, object> childRow;
            foreach (DataRow row in dt.Rows)
            {
                childRow = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns)
                {
                    childRow.Add(col.ColumnName, row[col]);
                }
                parentRow.Add(childRow);
            }

            jsSerializer.MaxJsonLength = Int32.MaxValue;

            var resultData = parentRow;
            var result = new ContentResult
            {
                Content = jsSerializer.Serialize(resultData),
                ContentType = "application/json"
            };

            JsonResult result1 = Json(result);
            result1.MaxJsonLength = Int32.MaxValue;
            result1.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
           


            //return Json(new { data = jsSerializer.Serialize(parentRow) }, JsonRequestBehavior.AllowGet);

            //return Json(new { data = parentRow }, JsonRequestBehavior.AllowGet);

            return Json(new { data = result1 }, JsonRequestBehavior.AllowGet);
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

        

        private void Timeout_check_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {

            ServerTimeout = true;
        }

       
    }
}