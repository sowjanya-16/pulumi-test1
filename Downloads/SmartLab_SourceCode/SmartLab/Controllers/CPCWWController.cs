using ClosedXML.Excel;
using LC_Reports_V1.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LC_Reports_V1.Controllers
{
    public class CPCWWController : Controller
    {

        private static List<string> LabTypes = new List<string>();
        private static List<string> Locations = new List<string>();
        //private SqlConnection conn;
        // private void connection()
        //{
        //    string constring = ConfigurationManager.ConnectionStrings["dbConnection"].ToString();
        //    conn = new SqlConnection(constring);
        //}
        //initialise connection
        private SqlConnection conn;
         private void connection()
        {
            string constring = ConfigurationManager.ConnectionStrings["dbConnection"].ToString();
            conn = new SqlConnection(constring);
        }


        //private void connection()
        //{

        //    string constring = ConfigurationManager.ConnectionStrings["dbConnection"].ToString();
        //    conn = new SqlConnection(constring);
        //}

        //private void OpenConnection()
        //{
        //    if(conn.State == System.Data.ConnectionState.Closed)
        //    {
        //        conn.Open();
        //    }
        //}

        //private void OpenConnection()
        //{
        //    if (conn.State == ConnectionState.Closed)
        //    {
        //        conn.Open();
        //    }
        //}

        private void CloseConnection()
        {
            if(conn.State ==System.Data.ConnectionState.Open)
            {
                conn.Close();
            }
        }
        //private void CloseConnection()
        //{
        //    if(conn.State ==System.Data.ConnectionState.Open)
        //    {
        //        conn.Close();
        //    }
        //}
        //private void CloseConnection()
        //{
        //    if (conn.State == ConnectionState.Open)
        //    {
        //        conn.Close();
        //    }
        //}

        // GET: CPCWW
        //public ActionResult Index()
        //{
        //    return View();
        //}
        //public ActionResult CPCChartsWW()
        //{
        //    return View();
        //}
        
        [HttpGet]
        //function to return list of Labtypes
        public ActionResult GetLabTypes()
        {
            using (BookingServerReplicaEntities db = new BookingServerReplicaEntities())
            {
                //select list item is used to represent as dropdown
                List<SelectListItem> lstLabTypes = new List<SelectListItem>();
                lstLabTypes.Add(new SelectListItem { Text = "CCHIL", Value = "CCHIL" });
                lstLabTypes.Add(new SelectListItem { Text = "ET", Value = "ET" });
                lstLabTypes.Add(new SelectListItem { Text = "CCSIL", Value = "CCSIL" });
                lstLabTypes.Add(new SelectListItem { Text = "OTB", Value = "OTB" });



                return Json(new { data = lstLabTypes }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        //function to return list of Lab Locations
        public ActionResult GetLocation()
        {
            using (BookingServerReplicaEntities db = new BookingServerReplicaEntities())
            {
                List<SelectListItem> lstLocations = new List<SelectListItem>();
                lstLocations.Add(new SelectListItem { Text = "Abstatt", Value = "Abt" }); 
                lstLocations.Add(new SelectListItem { Text = "Coimbatore", Value = "Cob" }); 
                lstLocations.Add(new SelectListItem { Text = "Bengaluru", Value = "Ban" }); 
                lstLocations.Add(new SelectListItem { Text = "Clayton", Value = "Cl" }); 
                lstLocations.Add(new SelectListItem { Text = "Farmington Hills", Value = "Fh" }); 
                lstLocations.Add(new SelectListItem { Text = "Guadalajara", Value = "Ga" }); 
                lstLocations.Add(new SelectListItem { Text = "Ho Chi Minh City", Value = "Hc" }); 
                lstLocations.Add(new SelectListItem { Text = "Suzhou", Value = "Szh" }); 
                lstLocations.Add(new SelectListItem { Text = "Plymouth ", Value = "Ply" }); 
                lstLocations.Add(new SelectListItem { Text = "Yokohama", Value = "Yh" }); 
                
              


                return Json(new { data = lstLocations }, JsonRequestBehavior.AllowGet);
            }
        }


        /* Action result when page loads, startdate, enddate are the paramaters passed from LabReport.cshtml */
        public ActionResult CPCChartsWW(string startdate, string enddate)
        {

            MasterChartObjDetails masterChartObjDetails = new MasterChartObjDetails();
            masterChartObjDetails.MonthlyChartLocwisedata = GetMonthlyChartLocwise(startdate,enddate);
            //masterChartObjDetails.MonthlyAvgTotalUtilizationdata = GetMonthlyAverageValuesChartLocwise(startdate, enddate);
            masterChartObjDetails.MonthlyIndexValueChartdata = GetMonthlyIndexValChartLocwise(startdate, enddate);
            return View(masterChartObjDetails);
            //return Json(new { success=true, data = wwcpc.WWCPC_Charts }, JsonRequestBehavior.AllowGet);
        }
        /* Action result when page loads after dates changed, startdate, enddate are the paramaters passed from CPCChartsWW */
        public ActionResult CPCCharts1(string startdate, string enddate, string type, string location)
            {

            MasterChartObjDetails masterChartObjDetails = new MasterChartObjDetails();
            masterChartObjDetails.MonthlyChartLocwisedata = GetMonthlyChartLocwise(startdate, enddate);
            masterChartObjDetails.MonthlyChartModelwisedata = GetMonthlyChartModelLocwise(startdate, enddate, type, location);
            //masterChartObjDetails.MonthlyChartOEMwisedata = GetMonthlywiseOEMbasedChart(startdate, enddate, oem);
            //masterChartObjDetails.MonthlyAvgTotalUtilizationdata= GetMonthlyAverageValuesChartLocwise(startdate, enddate);
            masterChartObjDetails.MonthlyIndexValueChartdata = GetMonthlyIndexValChartLocwise(startdate, enddate);
            return Json(new { success = true, data = masterChartObjDetails }, JsonRequestBehavior.AllowGet);
        }

        /* Action result for Month wise Location based utilization */
        private static WWCPC_Charts_AS_Locationwise GetMonthlyChartLocwise(string startdate, string enddate)
        {
            string Query = "";
            DataSet ds = new DataSet();
            SqlConnection conn = new SqlConnection();
            string constring = ConfigurationManager.ConnectionStrings["dbConnection"].ToString();
            conn = new SqlConnection(constring);

            //object initialization
            WWCPC_Charts_AS_Locationwise wwcpc = new WWCPC_Charts_AS_Locationwise();
            //convert timestamp to Date time
            DateTime strtdt = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(Math.Round(Convert.ToInt64(startdate) / 1000d)).ToLocalTime();
            DateTime enddt = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(Math.Round(Convert.ToInt64(enddate) / 1000d)).ToLocalTime();

            //check to see if dates are null
            if (startdate == null || enddate == null)
            {
                wwcpc.StartDate = DateTime.Now.AddYears(-1);
                wwcpc.EndDate = DateTime.Now.AddDays(-1);
            }
            else
            {
                wwcpc.StartDate = strtdt;
                wwcpc.EndDate = enddt;

            }
            /* dates to pass onto the UI datepickers as inputs */
            wwcpc.StDate = strtdt.ToShortDateString();
            wwcpc.EdDate = enddt.ToShortDateString();
           
            conn.Open();
            DataTable dt = new DataTable();
            /* query to execute the stored procedure passing the parameters Start and End Dates to get desired output*/
            //string Query = " Select SNo,Lab_ID,Lab_Name,ComputerID,Comp_DisplayName,FQDN,Location,Category,Start_Time,End_Time,ALM_imp_ID,Remarks,ModifiedUser from OSS_DownTime order by Comp_DisplayName,ComputerID  ";
             Query = " Exec [dbo].[GetRbCodeWise_Chart]  '" + wwcpc.StartDate + "','" + wwcpc.EndDate + "'  ";
            //OpenConnection();
            SqlCommand cmd = new SqlCommand(Query, conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);                      
            da.Fill(ds);
            conn.Close();
            
            //List<Get_DownTimeDetails> DownTimeresult = new List<Get_DownTimeDetails>();
            wwcpc.WWCPC_Charts = new List<WWCPC_Attributes_AS>();
            //filling the list obj with data
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                WWCPC_Attributes_AS item = new WWCPC_Attributes_AS();
                item.Month = Convert.ToInt32(row["Month"]);
                item.Year = Convert.ToInt32(row["Yr"]);
                item.Monthyear = row["Mthyr"].ToString();
                item.RBCode = row["RbCode"].ToString();
                item.ManualUsage = Convert.ToDouble(row["ManualTotalHours"]);
                item.AutomatedUsage = Convert.ToDouble(row["AutomatedTotalHours"]);
                item.ManualCAPLUsage = Convert.ToDouble(row["ManualCaplTotalHours"]);
                item.AutomatedCAPLUsage = Convert.ToDouble(row["AutomatedCaplTotalHours"]);
                //item.AutomatedCAPLUsage = Convert.ToDouble(row["AutomatedCaplTotalHours"]);
                item.TotalUtilization = Convert.ToDouble(row["TotalUtilization"]);
                //add each iteration entries to list obj
                wwcpc.WWCPC_Charts.Add(item);
            }

            //sort the list obj by year and month
            wwcpc.WWCPC_Charts = wwcpc.WWCPC_Charts.OrderByDescending(x => x.Year).ThenByDescending(x => x.Month).ToList();

            return wwcpc;

        }
        // Gets the monthly chart based on the LabType and Location selected by the user.
        private static WWCPC_charts_AS_Modelwise GetMonthlyChartModelLocwise(string startdate, string enddate, string type, string location)
        {
            string Query = "";
            DataSet ds = new DataSet();
            SqlConnection conn = new SqlConnection();
            string constring = ConfigurationManager.ConnectionStrings["dbConnection"].ToString();
            conn = new SqlConnection(constring);

            //object initialization
            WWCPC_charts_AS_Modelwise wwcpc = new WWCPC_charts_AS_Modelwise();
            //convert timestamp to Date time
            DateTime strtdt = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(Math.Round(Convert.ToInt64(startdate) / 1000d)).ToLocalTime();
            DateTime enddt = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(Math.Round(Convert.ToInt64(enddate) / 1000d)).ToLocalTime();


            if (startdate == null || enddate == null)
            {
                wwcpc.StartDate = DateTime.Now.AddYears(-1);
                wwcpc.EndDate = DateTime.Now.AddDays(-1);
            }
            else
            {
                wwcpc.StartDate = strtdt;
                wwcpc.EndDate = enddt;

            }
            wwcpc.Type = type;
            wwcpc.RbCode = location;
            /* dates to pass onto the UI datepickers as inputs */
            wwcpc.StDate = strtdt.ToShortDateString();
            wwcpc.EdDate = enddt.ToShortDateString();

            conn.Open();
            DataTable dt = new DataTable();
            /* query to execute the stored procedure passing the parameters Start and End Dates, LabType and Location to get desired output*/
            //string Query = " Select SNo,Lab_ID,Lab_Name,ComputerID,Comp_DisplayName,FQDN,Location,Category,Start_Time,End_Time,ALM_imp_ID,Remarks,ModifiedUser from OSS_DownTime order by Comp_DisplayName,ComputerID  ";
            Query = " Exec [dbo].[GetLabTypeWise_Chart]  '" + wwcpc.StartDate.ToShortDateString() + "','" + wwcpc.EndDate.ToShortDateString() + "','" + wwcpc.Type + "','" + wwcpc.RbCode + "'  ";
            //OpenConnection();
            SqlCommand cmd = new SqlCommand(Query, conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            conn.Close();

            //List<Get_DownTimeDetails> DownTimeresult = new List<Get_DownTimeDetails>();
            wwcpc.WWCPC_Charts = new List<WWCPC_Attributes_AS>();
            //filling the list obj with data
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                WWCPC_Attributes_AS item = new WWCPC_Attributes_AS();
                item.Month = Convert.ToInt32(row["Month"]);
                item.Year = Convert.ToInt32(row["Yr"]);
                item.Model = row["Model"].ToString();
                item.Monthyear = row["Mthyr"].ToString();
                item.RBCode = row["RbCode"].ToString();
                item.ManualUsage = Convert.ToDouble(row["ManualTotalHours"]);
                item.AutomatedUsage = Convert.ToDouble(row["AutomatedTotalHours"]);
                item.ManualCAPLUsage = Convert.ToDouble(row["ManualCaplTotalHours"]);
                item.AutomatedCAPLUsage = Convert.ToDouble(row["AutomatedCaplTotalHours"]);
                item.AutomatedCAPLUsage = Convert.ToDouble(row["AutomatedCaplTotalHours"]);
                item.TotalUtilization = Convert.ToDouble(row["TotalUtilization"]);
                //add each iteration entries to list obj
                wwcpc.WWCPC_Charts.Add(item);
            }

            //sort the list obj by year and month
            wwcpc.WWCPC_Charts = wwcpc.WWCPC_Charts.OrderByDescending(x => x.Year).ThenByDescending(x => x.Month).ToList();

            return wwcpc;

        }

        //private static WWCPC_charts_AS_OEMwise GetMonthlywiseOEMbasedChart(string startdate, string enddate, int OEM)
        //{
        //    string Query = "";
        //    DataTable dt = new DataTable();
        //    DataSet ds = new DataSet();
        //    SqlConnection conn = new SqlConnection();
        //    string constring = ConfigurationManager.ConnectionStrings["dbConnection"].ToString();
        //    conn = new SqlConnection(constring);
        //    WWCPC_charts_AS_OEMwise wwcpc_OEM = new WWCPC_charts_AS_OEMwise();
        //    //wwcpc_OEM.WWCPC_Chart_OEM
        //    DateTime strtdt = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(Math.Round(Convert.ToInt64(startdate) / 1000d)).ToLocalTime();
        //    DateTime enddt = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(Math.Round(Convert.ToInt64(enddate) / 1000d)).ToLocalTime();


        //    WWCPC_Attributes_AS item = new WWCPC_Attributes_AS();
        //    WWCPC_Charts_AS wwcpc = new WWCPC_Charts_AS();
        //    if (startdate == null || enddate == null)
        //    {
        //        wwcpc_OEM.StartDate = DateTime.Now.AddYears(-1);
        //        wwcpc_OEM.EndDate = DateTime.Now.AddDays(-1);
        //    }
        //    else
        //    {
        //        wwcpc_OEM.StartDate = strtdt;
        //        wwcpc_OEM.EndDate = enddt;

        //    }

        //    //wwcpc_OEM.RbCode = location;
        //    /* dates to pass onto the UI datepickers as inputs */
        //    wwcpc_OEM.StDate = strtdt.ToShortDateString();
        //    wwcpc_OEM.EdDate = enddt.ToShortDateString();

        //    try
        //    {

        //        conn.Open();
        //        //DataTable dt = new DataTable();

        //    connection();
        //    DataTable dt = new DataTable();
        //    DateTime Startdate = DateTime.Now.AddMonths(-12);
        //    DateTime Enddate = DateTime.Now.AddDays(-1);
        //    //string Query = " Select SNo,Lab_ID,Lab_Name,ComputerID,Comp_DisplayName,FQDN,Location,Category,Start_Time,End_Time,ALM_imp_ID,Remarks,ModifiedUser from OSS_DownTime order by Comp_DisplayName,ComputerID  ";
        //        Query = " Exec [dbo].[GetOEMWise_Chart]  '" + wwcpc_OEM.StartDate + "','" + wwcpc_OEM.EndDate + "','" + wwcpc_OEM.OEM + "'  ";
        //        //OpenConnection();
        //    SqlCommand cmd = new SqlCommand(Query, conn);
        //    SqlDataAdapter da = new SqlDataAdapter(cmd);
        //        da.Fill(ds);
        //        conn.Close();

        //    //List<Get_DownTimeDetails> DownTimeresult = new List<Get_DownTimeDetails>();
        //        wwcpc_OEM.WWCPC_Chart_OEM = new List<WWCPC_Attributes_AS_OEM>();
        //        foreach (DataRow row in ds.Tables[0].Rows)
        //    {
        //            WWCPC_Attributes_AS_OEM item = new WWCPC_Attributes_AS_OEM();
        //        item.Month = Convert.ToInt32(row["Month"]);
        //        item.Year = Convert.ToInt32(row["Yr"]);
        //            item.Monthyear = row["Mthyr"].ToString();
        //            item.OEM = row["OEM"].ToString();
        //        item.RBCode = row["RbCode"].ToString();
        //        item.ManualUsage = Convert.ToDouble(row["ManualTotalHours"]);
        //        item.AutomatedUsage = Convert.ToDouble(row["AutomatedTotalHours"]);
        //        item.ManualCAPLUsage = Convert.ToDouble(row["ManualCaplTotalHours"]);
        //        item.AutomatedCAPLUsage = Convert.ToDouble(row["AutomatedCaplTotalHours"]);
        //        item.AutomatedCAPLUsage = Convert.ToDouble(row["AutomatedCaplTotalHours"]);
        //        item.TotalUtilization = Convert.ToDouble(row["TotalUtilization"]);

        //            wwcpc_OEM.WWCPC_Chart_OEM.Add(item);
        //        }


        //        wwcpc_OEM.WWCPC_Chart_OEM = wwcpc_OEM.WWCPC_Chart_OEM.OrderByDescending(x => x.Year).ThenByDescending(x => x.Month).ToList();
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    return wwcpc_OEM;
        //}

        private static WWCPC_charts_AS_IndexvaluesChart GetMonthlyIndexValChartLocwise(string startdate, string enddate)
        {
            string Query = "";
            // to store the query data
            DataSet ds = new DataSet();
            //establish sql connection
            SqlConnection conn = new SqlConnection();
            //use connection string name for connecting to the particular database
            string constring = ConfigurationManager.ConnectionStrings["dbConnection"].ToString();
            conn = new SqlConnection(constring);

            // intialisation of object
            WWCPC_charts_AS_IndexvaluesChart wwcpc_indexvalchart = new WWCPC_charts_AS_IndexvaluesChart();
            //convert start and end timestamps to Date and time format
            DateTime strtdt = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(Math.Round(Convert.ToInt64(startdate) / 1000d)).ToLocalTime();
            DateTime enddt = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(Math.Round(Convert.ToInt64(enddate) / 1000d)).ToLocalTime();


            if (startdate == null || enddate == null)
            {
                //setting default dates
                wwcpc_indexvalchart.StartDate = DateTime.Now.AddYears(-1);
                wwcpc_indexvalchart.EndDate = DateTime.Now.AddDays(-1);
            }
            else
            {
                wwcpc_indexvalchart.StartDate = strtdt;
                wwcpc_indexvalchart.EndDate = enddt;

            }
            /* dates to pass onto the UI datepickers as inputs */
            wwcpc_indexvalchart.StDate = strtdt.ToShortDateString();
            wwcpc_indexvalchart.EdDate = enddt.ToShortDateString();
            //open connection
            conn.Open();
            // object to store data rows table data from stored procedure
            DataTable dt = new DataTable();

            //string Query = " Select SNo,Lab_ID,Lab_Name,ComputerID,Comp_DisplayName,FQDN,Location,Category,Start_Time,End_Time,ALM_imp_ID,Remarks,ModifiedUser from OSS_DownTime order by Comp_DisplayName,ComputerID  ";
            //stored procedure execution using the query and passing the parameters
            Query = " Exec [dbo].[WorldWideWiseIndex_Chart_test]  '" + wwcpc_indexvalchart.StartDate + "','" + wwcpc_indexvalchart.EndDate + "'  ";
            //OpenConnection();
            SqlCommand cmd = new SqlCommand(Query, conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            conn.Close();

            //List<Get_DownTimeDetails> DownTimeresult = new List<Get_DownTimeDetails>();
            // initialise list object
            wwcpc_indexvalchart.WWCPC_Charts_IndexVal = new List<WWCPC_Attributes_AS_Index_Value>();
            //foreach loop to populate the values coming from DB into list object
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                WWCPC_Attributes_AS_Index_Value item = new WWCPC_Attributes_AS_Index_Value();
                item.Month = Convert.ToInt32(row["Month"]);
                item.Year = Convert.ToInt32(row["Yr"]);
                item.Monthyear = row["Mthyr"].ToString();
                item.RBCode = row["RbCode"].ToString();
                item.TotalManualHours= Convert.ToDouble(row["TotalManualHours"]);
                item.TotalAutomatedHours= Convert.ToDouble(row["TotalAutoHours"]);
                item.IndexValue = Convert.ToDouble(row["IndexVal"]);               
                wwcpc_indexvalchart.WWCPC_Charts_IndexVal.Add(item);
            }

            //sort the object data using Year attribute
            wwcpc_indexvalchart.WWCPC_Charts_IndexVal = wwcpc_indexvalchart.WWCPC_Charts_IndexVal.OrderByDescending(x => x.Year).ThenByDescending(x => x.Month).ToList();
            //return object data to javascript file wwcpc_indexval
            return wwcpc_indexvalchart;

        }


        private static WWCPC_charts_AS_AvgUtil GetMonthlyAverageValuesChartLocwise(string startdate, string enddate)
        {
            string Query = "";
            DataSet ds = new DataSet();
            SqlConnection conn = new SqlConnection();
            string constring = ConfigurationManager.ConnectionStrings["dbConnection"].ToString();
            conn = new SqlConnection(constring);


            WWCPC_charts_AS_AvgUtil wwcpc = new WWCPC_charts_AS_AvgUtil();
            DateTime strtdt = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(Math.Round(Convert.ToInt64(startdate) / 1000d)).ToLocalTime();
            DateTime enddt = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(Math.Round(Convert.ToInt64(enddate) / 1000d)).ToLocalTime();


            if (startdate == null || enddate == null)
            {
                wwcpc.StartDate = DateTime.Now.AddYears(-1);
                wwcpc.EndDate = DateTime.Now.AddDays(-1);
            }
            else
            {
                wwcpc.StartDate = strtdt;
                wwcpc.EndDate = enddt;

            }
            /* dates to pass onto the UI datepickers as inputs */
            wwcpc.StDate = strtdt.ToShortDateString();
            wwcpc.EdDate = enddt.ToShortDateString();

            conn.Open();
            DataTable dt = new DataTable();

            //string Query = " Select SNo,Lab_ID,Lab_Name,ComputerID,Comp_DisplayName,FQDN,Location,Category,Start_Time,End_Time,ALM_imp_ID,Remarks,ModifiedUser from OSS_DownTime order by Comp_DisplayName,ComputerID  ";
            Query = " Exec [dbo].[WorldWideWiseAverage_Chart_test]  '" + wwcpc.StartDate + "','" + wwcpc.EndDate + "'  ";
            //OpenConnection();
            SqlCommand cmd = new SqlCommand(Query, conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            conn.Close();

            //List<Get_DownTimeDetails> DownTimeresult = new List<Get_DownTimeDetails>();
            wwcpc.WWCPC_Charts = new List<WWCPC_Attributes_AS_Avg_values>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                WWCPC_Attributes_AS_Avg_values item = new WWCPC_Attributes_AS_Avg_values();
                item.Month = Convert.ToInt32(row["Month"]);
                item.Year = Convert.ToInt32(row["Yr"]);
                item.Monthyear = row["Mthyr"].ToString();
                item.RBCode = row["RbCode"].ToString();
                item.LabCount = Convert.ToInt32(row["LabCount"]);
                item.AverageTotalUtilization = Convert.ToDouble(row["AverageofTotalUtilization"]);
                item.TotalUtilization = Convert.ToDouble(row["TotalUtilization"]);
                wwcpc.WWCPC_Charts.Add(item);
            }


            wwcpc.WWCPC_Charts = wwcpc.WWCPC_Charts.OrderByDescending(x => x.Year).ThenByDescending(x => x.Month).ToList();

            return wwcpc;

        }

        [HttpPost]
        public ActionResult ExportDataToExcel_Locationwise(WWCPC_Charts_AS_Locationwise excelobject)
        {
            try
            {
                // give the file name which can be saved under downloads
                string filename = @"Month_wise_Location_based_utilization_in_hours_"+excelobject.StDate+"_"+excelobject.EdDate+".xlsx";
                // no of columns must be equal to no of Data Columns that must be created and fill with data using excel object
                System.Data.DataTable dt = new System.Data.DataTable("Info");
                dt.Columns.AddRange(new DataColumn[7] {new DataColumn("Location"),
                                            new DataColumn("Month & Year"),                                           
                                            new DataColumn("Manual Hours Usage", typeof(double)),
                                            new DataColumn("Automated Hours Usage", typeof(double)),
                                            new DataColumn("Manual Capl Hours Usage", typeof(double)),
                                            new DataColumn("Automated Capl Hours Usage", typeof(double)),
                                            new DataColumn("Overall Time in hrs", typeof(double))});
                //check if excelobject coming from ajax call is null
                if (excelobject == null)
                {
                    return Json(new { success = false, message = "Excel export Unsuccessful" }, JsonRequestBehavior.AllowGet);
                }
                //assign the data from excel object to the Rows object where the data must be placed in the excel sheet
                foreach (var info in excelobject.WWCPC_Charts)
                {
                    //double Total = Math.Round(info.ManualUsage, 2) + Math.Round(info.ManualCAPLUsage, 2) + Math.Round(info.AutomatedUsage, 2) + Math.Round(info.AutomatedCAPLUsage,2);
                    dt.Rows.Add(info.RBCode, info.Monthyear, Math.Round(info.ManualUsage, 2), Math.Round(info.AutomatedUsage, 2), Math.Round(info.ManualCAPLUsage, 2), Math.Round(info.AutomatedCAPLUsage, 2), Math.Round(info.TotalUtilization, 2));
                }
                using (XLWorkbook wb = new XLWorkbook())
                {
                    //sheet name
                    var ws = wb.Worksheets.Add("LocUtilization_Data");
                    //the range of cells mentioned under will have a styled table according to the properties given below
                    ws.Range("A1:B2").Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    ws.Range("A1:B2").Style.Border.InsideBorder = XLBorderStyleValues.Dotted;
                    ws.Range("A1:B2").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    ws.Range("A1:B2").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    ws.Range("A1:B2").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    ws.Range("A1:B2").Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    ws.Range("A1:B2").Style.Font.Bold = true;
                    ws.Cell(1, 1).Style.Fill.BackgroundColor = XLColor.LightBlue;
                    ws.Cell(2, 1).Style.Fill.BackgroundColor = XLColor.LightBlue;
                    ws.Cell(1, 2).Style.Fill.BackgroundColor = XLColor.LightBlue;
                    ws.Cell(2, 2).Style.Fill.BackgroundColor = XLColor.LightBlue;

                    // assign start and end dates as the intial values
                    ws.Cell(1, 1).Value = "Start Date";
                    ws.Cell(1, 2).Value = excelobject.StDate;
                    ws.Cell(2, 1).Value = "End Date";
                    ws.Cell(2, 2).Value = excelobject.EdDate;

                    // insert the table containing the data at the 4th row from first column
                    ws.Cell(4, 1).InsertTable(dt);
                    //wb.Worksheets.Add(dt);
                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);
                        var exobj = File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
                        return Json(exobj, JsonRequestBehavior.AllowGet);
                    }
                }
            }

            catch (Exception ex)
            {
                HomeController.logger.Error(ex, "ExportTSOUDataToExcel");
                return Json(new { success = false, message = "Chart data Excel Export Unsuccessful" }, JsonRequestBehavior.AllowGet);

            }

        }
        /* Action result for exporting ModelLocationwise chart data in excel */
        [HttpPost]
        public ActionResult ExportDataToExcel_ModelLocationwise(WWCPC_charts_AS_Modelwise excelobject, string type, string location)
        {
            try
            {
                string startdate = string.Empty;
                string enddate = string.Empty;
                // give the file name which can be saved under downloads
                string filename = @"Month_wise_"+ type+ "_"+ location + "_based_utilization_in_hours" + excelobject.StDate + "_" + excelobject.EdDate + ".xlsx";
                // no of columns must be equal to no of Data Columns that must be created and fill with data using excel object
                System.Data.DataTable dt = new System.Data.DataTable("Info");
                dt.Columns.AddRange(new DataColumn[8] {new DataColumn("LabType"),
                                            new DataColumn("Location"),
                                            new DataColumn("Month & Year"),
                                            new DataColumn("Manual Hours Usage", typeof(double)),
                                            new DataColumn("Automated Hours Usage", typeof(double)),
                                            new DataColumn("Manual Capl Hours Usage", typeof(double)),
                                            new DataColumn("Automated Capl Hours Usage", typeof(double)),
                                            new DataColumn("Overall Time in hrs", typeof(double))});
                //check if excelobject coming from ajax call is null
                if (excelobject == null)
                {
                    return Json(new { success = false, message = "Excel export Unsuccessful" }, JsonRequestBehavior.AllowGet);
                }
                //assign the data from excel object to the Rows object where the data must be placed in the excel sheet
                foreach (var info in excelobject.WWCPC_Charts)
                {
                    //double Total = Math.Round(info.ManualUsage, 2) + Math.Round(info.ManualCAPLUsage, 2) + Math.Round(info.AutomatedUsage, 2) + Math.Round(info.AutomatedCAPLUsage,2);
                    dt.Rows.Add(info.Type, info.RBCode, info.Monthyear, Math.Round(info.ManualUsage, 2), Math.Round(info.AutomatedUsage, 2), Math.Round(info.ManualCAPLUsage, 2), Math.Round(info.AutomatedCAPLUsage, 2), Math.Round(info.TotalUtilization, 2));
                }

                using (XLWorkbook wb = new XLWorkbook())
                {
                    //sheet name
                    var ws = wb.Worksheets.Add("LocModelUtilization_Data");
                    //the range of cells mentioned under will have a styled table according to the properties given below
                    ws.Range("A1:B2").Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    ws.Range("A1:B2").Style.Border.InsideBorder = XLBorderStyleValues.Dotted;
                    ws.Range("A1:B2").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    ws.Range("A1:B2").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    ws.Range("A1:B2").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    ws.Range("A1:B2").Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    ws.Range("A1:B2").Style.Font.Bold = true;
                    ws.Cell(1, 1).Style.Fill.BackgroundColor = XLColor.LightBlue;
                    ws.Cell(2, 1).Style.Fill.BackgroundColor = XLColor.LightBlue;
                    ws.Cell(1, 2).Style.Fill.BackgroundColor = XLColor.LightBlue;
                    ws.Cell(2, 2).Style.Fill.BackgroundColor = XLColor.LightBlue;

                    // assign start and end dates as the intial values
                    ws.Cell(1, 1).Value = "Start Date";
                    ws.Cell(1, 2).Value = excelobject.StDate;
                    ws.Cell(2, 1).Value = "End Date";
                    ws.Cell(2, 2).Value = excelobject.EdDate;

                    // insert the table containing the data at the 4th row from first column
                    ws.Cell(4, 1).InsertTable(dt);
                    //wb.Worksheets.Add(dt);
                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);
                        var exobj = File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
                        return Json(exobj, JsonRequestBehavior.AllowGet);
                    }
                }
            }

            catch (Exception ex)
            {
                HomeController.logger.Error(ex, "ExportTSOUDataToExcel");
                return Json(new { success = false, message = "Chart data Excel Export Unsuccessful" }, JsonRequestBehavior.AllowGet);

            }

        }
		 /* Action result for exporting chart data in excel */
        [HttpPost]
        public ActionResult ExportDataToExcel_AvgTotalLocationwise(WWCPC_charts_AS_AvgUtil excelobject)
        {
            try
            {

                string filename = @"Month_wise_Location_based_Average_Total_utilization_in_hours_" + excelobject.StDate + "_" + excelobject.EdDate + " .xlsx";
                System.Data.DataTable dt = new System.Data.DataTable("Info");
                dt.Columns.AddRange(new DataColumn[4] {new DataColumn("Location"),
                                            new DataColumn("Month & Year"),
                                            new DataColumn("Average of Overall Time in Hours", typeof(double)),                                            
                                            new DataColumn("Overall Time in Hours", typeof(double))});
                if (excelobject == null)
                {
                    return Json(new { success = false, message = "Excel export Unsuccessful" }, JsonRequestBehavior.AllowGet);
                }
                foreach (var info in excelobject.WWCPC_Charts)
                {
                    //double Total = Math.Round(info.ManualUsage, 2) + Math.Round(info.ManualCAPLUsage, 2) + Math.Round(info.AutomatedUsage, 2) + Math.Round(info.AutomatedCAPLUsage,2);
                    dt.Rows.Add(info.RBCode, info.Monthyear, Math.Round(info.AverageTotalUtilization), Math.Round(info.TotalUtilization, 2));
                }
                using (XLWorkbook wb = new XLWorkbook())
                {
                    var ws = wb.Worksheets.Add("Monthly_wise_Location_based_Average_Total_utilization_in_hours");
                    ws.Range("A1:B2").Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    ws.Range("A1:B2").Style.Border.InsideBorder = XLBorderStyleValues.Dotted;
                    ws.Range("A1:B2").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    ws.Range("A1:B2").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    ws.Range("A1:B2").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    ws.Range("A1:B2").Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    ws.Range("A1:B2").Style.Font.Bold = true;
                    ws.Cell(1, 1).Style.Fill.BackgroundColor = XLColor.LightBlue;
                    ws.Cell(2, 1).Style.Fill.BackgroundColor = XLColor.LightBlue;
                    ws.Cell(1, 2).Style.Fill.BackgroundColor = XLColor.LightBlue;
                    ws.Cell(2, 2).Style.Fill.BackgroundColor = XLColor.LightBlue;


                    ws.Cell(1, 1).Value = "Start Date";
                    ws.Cell(1, 2).Value = excelobject.StDate;
                    ws.Cell(2, 1).Value = "End Date";
                    ws.Cell(2, 2).Value = excelobject.EdDate;


                    ws.Cell(4, 1).InsertTable(dt);
                    //wb.Worksheets.Add(dt);
                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);
                        var exobj = File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
                        return Json(exobj, JsonRequestBehavior.AllowGet);
                    }
                }
            }

            catch (Exception ex)
            {
                HomeController.logger.Error(ex, "ExportTSOUDataToExcel");
                return Json(new { success = false, message = "Chart data Excel Export Unsuccessful" }, JsonRequestBehavior.AllowGet);

            }

        }

		 /* Action result for exporting chart data in excel */
        [HttpPost]
        public ActionResult ExportDataToExcel_IndexvalLocationwise(WWCPC_charts_AS_IndexvaluesChart excelobject)
        {
            try
            {
                // give the file name which can be saved under downloads
                string filename = @"Month_wise_Location_based_Index_Values_" + excelobject.StDate + "_" + excelobject.EdDate + " .xlsx";
                System.Data.DataTable dt = new System.Data.DataTable("Info");
                // no of columns must be equal to no of Data Columns that must be created and fill with data using excel object
                dt.Columns.AddRange(new DataColumn[5] {new DataColumn("Location"),
                                            new DataColumn("Month & Year"),
                                            new DataColumn("Total Manual Hours per month", typeof(double)),
                                            new DataColumn("Total Automated Hours per month", typeof(double)),
                                            new DataColumn("Index Value", typeof(double))});
                //check if excelobject coming from ajax call is null
                if (excelobject == null)
                {
                    return Json(new { success = false, message = "Excel export Unsuccessful" }, JsonRequestBehavior.AllowGet);
                }
                //assign the data from excel object to the Rows object where the data must be placed in the excel sheet
                foreach (var info in excelobject.WWCPC_Charts_IndexVal)
                {
                    //double Total = Math.Round(info.ManualUsage, 2) + Math.Round(info.ManualCAPLUsage, 2) + Math.Round(info.AutomatedUsage, 2) + Math.Round(info.AutomatedCAPLUsage,2);
                    dt.Rows.Add(info.RBCode, info.Monthyear, info.TotalManualHours,info.TotalAutomatedHours, info.IndexValue);
                }
                using (XLWorkbook wb = new XLWorkbook())
                {
                    //sheet name
                    var ws = wb.Worksheets.Add("Index_Values_Data");
                    //the range of cells mentioned under will have a styled table according to the properties given below
                    ws.Range("A1:B2").Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    ws.Range("A1:B2").Style.Border.InsideBorder = XLBorderStyleValues.Dotted;
                    ws.Range("A1:B2").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    ws.Range("A1:B2").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    ws.Range("A1:B2").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    ws.Range("A1:B2").Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    ws.Range("A1:B2").Style.Font.Bold = true;
                    ws.Cell(1, 1).Style.Fill.BackgroundColor = XLColor.LightBlue;
                    ws.Cell(2, 1).Style.Fill.BackgroundColor = XLColor.LightBlue;
                    ws.Cell(1, 2).Style.Fill.BackgroundColor = XLColor.LightBlue;
                    ws.Cell(2, 2).Style.Fill.BackgroundColor = XLColor.LightBlue;

                    // assign start and end dates as the intial values
                    ws.Cell(1, 1).Value = "Start Date";
                    ws.Cell(1, 2).Value = excelobject.StDate;
                    ws.Cell(2, 1).Value = "End Date";
                    ws.Cell(2, 2).Value = excelobject.EdDate;

                    // insert the table containing the data at the 4th row from first column
                    ws.Cell(4, 1).InsertTable(dt);
                    //wb.Worksheets.Add(dt);
                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);
                        var exobj = File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
                        return Json(exobj, JsonRequestBehavior.AllowGet);
                    }
                }
            }

            catch (Exception ex)
            {
                HomeController.logger.Error(ex, "ExportTSOUDataToExcel");
                return Json(new { success = false, message = "Chart data Excel Export Unsuccessful" }, JsonRequestBehavior.AllowGet);

            }

        }
    }

    
}