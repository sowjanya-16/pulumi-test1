using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using LC_Reports_V1.Models;
using System.Globalization;
using System.Web.UI.WebControls;
using System.IO;
using System.Web.UI;
using System.Data;
using ClosedXML.Excel;
using System.Data.SqlClient;
using System.Web.Script.Serialization;
using System.Configuration;

namespace LC_Reports_V1.Controllers
{
    [Authorize(Users = @"apac\din2cob,apac\jov6cob,apac\mta2cob,apac\muu4cob,apac\sbr2kor,apac\chb1kor,apac\oig1cob,apac\mae9cob,apac\rau2kor,apac\rma5cob, apac\pch2kor, apac\mxk8kor, , apac\ghb1cob")]
    public class SLCockpitController : Controller
    {
        private SqlConnection con, budgetingcon;
        public ActionResult Index()
        {
            LCfilterInfo lc = new LCfilterInfo();
            lc.startDate = new DateTime(DateTime.Now.Year, 01, 01);
            lc.endDate = DateTime.Now;
            //lc.startDate = DateTime.Now;
            //lc.endDate = DateTime.Now;
            return View(lc);
        }

        public ActionResult Overview()
        {
            return View();
        }
       
        private void connection()
            {
            //var datasource = @"cob1098672\sqlexpress";//your server
            //var database = "BookingServerReplica"; //your database name
            //var username = "espis5"; //username of server to connect
            //var password = "espis5@12345"; //password

            //var datasource = @"BANEN1093154\SQLEXPRESSESPIS5";//your server
            //var database = "BudgetingToolDB"; //your database name
            //var username = "jov6cob"; //username of server to connect
            //var password = "serveruser@123"; //password

            //your connection string 
            //string connString = @"Data Source=" + datasource + ";Initial Catalog="
            //            + database + ";Persist Security Info=True;User ID=" + username + ";Password=" + password;

            string constring = ConfigurationManager.ConnectionStrings["dbConnection"].ToString();
            string budgeting_constring = ConfigurationManager.ConnectionStrings["BudgetingdbConnection"].ToString();
            con = new SqlConnection(constring);
            budgetingcon = new SqlConnection(budgeting_constring);
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
        private void BudgetingOpenConnection()
        {
            if (budgetingcon.State == ConnectionState.Closed)
            {
                budgetingcon.Open();
            }
        }

        private void BudgetingCloseConnection()
        {
            if (budgetingcon.State == ConnectionState.Open)
            {
                budgetingcon.Close();
            }
        }


        public ActionResult InfraUtilisation(DateTime SDate, DateTime EDate, string Location)
        {
            DataTable dt = new DataTable();
            string Query = "";
            connection();
            OpenConnection();
            Query = " Exec [dbo].[GetInfraUtilization] '" + SDate + "', '" + EDate + "', '" + Location + "' ";
            //Query = " Exec [dbo].[GetInfraUtilization] ";
            SqlCommand cmd = new SqlCommand(Query, con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            CloseConnection();

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

            return Json(new { data = result1 }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetTSOU(DateTime SDate, DateTime EDate, string Location, string LabType)
        {
            double overallManualhrs;
            double overallAutomatedhrs;
            double overallAutomatedCaplhrs;
            double overallManualCaplhrs;
            DataTable dt = new DataTable();
            string Query = "";
            connection();
            OpenConnection();
            Query = " Exec [dbo].[GetTSOU_Chart] '" + SDate + "', '" + EDate + "', '" + Location + "' , '" + LabType + "'";
            //Query = " Exec [dbo].[GetTSOU_Chart] ";
            SqlCommand cmd = new SqlCommand(Query, con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            CloseConnection();

            overallManualhrs = 0;
            overallAutomatedhrs = 0;
            overallAutomatedCaplhrs = 0;
            overallManualCaplhrs = 0;

            LCfilterInfo.LabBookingExport objBookingExport = new LCfilterInfo.LabBookingExport();
            objBookingExport = new LCfilterInfo.LabBookingExport();

            LCfilterInfo.LabBookingExportLab[] LabArray = new LCfilterInfo.LabBookingExportLab[dt.Rows.Count];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //LCfilterInfo.LabBookingExportLab LabArray = new LCfilterInfo.LabBookingExportLab();

                LabArray[i] = new LCfilterInfo.LabBookingExportLab();
                LabArray[i].id = (ushort)Convert.ToUInt32(dt.Rows[i]["Id"].ToString());
                LabArray[i].Model = dt.Rows[i]["Model"].ToString();
                LabArray[i].Inventory = dt.Rows[i]["Inventory"].ToString();
                LabArray[i].Location = dt.Rows[i]["RbCode"].ToString();
                LabArray[i].OEM = dt.Rows[i]["OEM"].ToString();
                LabArray[i].ManualTotalHours = Convert.ToDouble(dt.Rows[i]["ManualTotalHours"].ToString());
                LabArray[i].ManualCaplTotalHours = Convert.ToDouble(dt.Rows[i]["ManualCaplTotalHours"].ToString());
                LabArray[i].AutomatedTotalHours = Convert.ToDouble(dt.Rows[i]["AutomatedTotalHours"].ToString());
                LabArray[i].AutomatedCaplTotalHours = Convert.ToDouble(dt.Rows[i]["AutomatedCaplTotalHours"].ToString());
                LabArray[i].TSOULabel = dt.Rows[i]["Model"].ToString() + " " + dt.Rows[i]["Inventory"].ToString() + " " + dt.Rows[i]["RbCode"].ToString();
                overallManualhrs += Convert.ToDouble(dt.Rows[i]["ManualTotalHours"].ToString());
                overallManualCaplhrs += Convert.ToDouble(dt.Rows[i]["ManualCaplTotalHours"].ToString());
                overallAutomatedhrs += Convert.ToDouble(dt.Rows[i]["AutomatedTotalHours"].ToString());
                overallAutomatedCaplhrs += Convert.ToDouble(dt.Rows[i]["AutomatedCaplTotalHours"].ToString());

            }

            objBookingExport.Labs = LabArray;
            objBookingExport.StartDate_UI = SDate.ToShortDateString();
            objBookingExport.EndDate_UI = EDate.ToShortDateString();
            objBookingExport.StartDate = SDate;
            objBookingExport.EndDate = EDate;
            objBookingExport.OverallManualHours = overallManualhrs;
            objBookingExport.OverallManualCaplHours = overallManualCaplhrs;
            objBookingExport.OverallAutomatedHours = overallAutomatedhrs;
            objBookingExport.OverallAutomatedCaplHours = overallAutomatedCaplhrs;


            return Json(new { data = objBookingExport }, JsonRequestBehavior.AllowGet);
        }

      
        public ActionResult GetTSOUOEM(DateTime SDate, DateTime EDate, string Location, string LabType)
        {
            //DateTime SDate = new DateTime(DateTime.Now.Year, 01, 01);
            //DateTime EDate = DateTime.Now;
            //string Location = "Ban,Cob";
            //string LabType = "CCHIL";
            double overallManualhrs;
            double overallAutomatedhrs;
            double overallAutomatedCaplhrs;
            double overallManualCaplhrs;
            DataTable dt = new DataTable();
            string Query = "";
            connection();
            OpenConnection();
            Query = " Exec [dbo].[GetTSOUByOEM_Chart] '" + SDate + "', '" + EDate + "', '" + Location + "' , '" + LabType + "'";
            //Query = " Exec [dbo].[GetTSOU_Chart] ";
            SqlCommand cmd = new SqlCommand(Query, con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            CloseConnection();

            overallManualhrs = 0;
            overallAutomatedhrs = 0;
            overallAutomatedCaplhrs = 0;
            overallManualCaplhrs = 0;

            LCfilterInfo.LabBookingExport objBookingExport = new LCfilterInfo.LabBookingExport();
            objBookingExport = new LCfilterInfo.LabBookingExport();

            LCfilterInfo.LabBookingExportLab[] LabArray = new LCfilterInfo.LabBookingExportLab[dt.Rows.Count];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //LCfilterInfo.LabBookingExportLab LabArray = new LCfilterInfo.LabBookingExportLab();

                LabArray[i] = new LCfilterInfo.LabBookingExportLab();
                //LabArray[i].id = (ushort)Convert.ToUInt32(dt.Rows[i]["Id"].ToString());
                LabArray[i].Model = dt.Rows[i]["Model"].ToString();
                //LabArray[i].Inventory = dt.Rows[i]["Inventory"].ToString();
                LabArray[i].Location = dt.Rows[i]["RbCode"].ToString();
                LabArray[i].OEM = dt.Rows[i]["OEM"].ToString();
                LabArray[i].ManualTotalHours = Convert.ToDouble(dt.Rows[i]["ManualTotalHours"].ToString());
                LabArray[i].ManualCaplTotalHours = Convert.ToDouble(dt.Rows[i]["ManualCaplTotalHours"].ToString());
                LabArray[i].AutomatedTotalHours = Convert.ToDouble(dt.Rows[i]["AutomatedTotalHours"].ToString());
                LabArray[i].AutomatedCaplTotalHours = Convert.ToDouble(dt.Rows[i]["AutomatedCaplTotalHours"].ToString());
                LabArray[i].TSOULabel = dt.Rows[i]["Model"].ToString() + " " + dt.Rows[i]["RbCode"].ToString();
                overallManualhrs += Convert.ToDouble(dt.Rows[i]["ManualTotalHours"].ToString());
                overallManualCaplhrs += Convert.ToDouble(dt.Rows[i]["ManualCaplTotalHours"].ToString());
                overallAutomatedhrs += Convert.ToDouble(dt.Rows[i]["AutomatedTotalHours"].ToString());
                overallAutomatedCaplhrs += Convert.ToDouble(dt.Rows[i]["AutomatedCaplTotalHours"].ToString());

            }

            objBookingExport.Labs = LabArray;
            objBookingExport.StartDate_UI = SDate.ToShortDateString();
            objBookingExport.EndDate_UI = EDate.ToShortDateString();
            objBookingExport.StartDate = SDate;
            objBookingExport.EndDate = EDate;
            objBookingExport.OverallManualHours = overallManualhrs;
            objBookingExport.OverallManualCaplHours = overallManualCaplhrs;
            objBookingExport.OverallAutomatedHours = overallAutomatedhrs;
            objBookingExport.OverallAutomatedCaplHours = overallAutomatedCaplhrs;


            return Json(new { data = objBookingExport }, JsonRequestBehavior.AllowGet);
        }



        public ActionResult HCChart()
        {
            
                var obj = "";
                DataTable dt = new DataTable();

                try
                {
                    //create instanace of database connection
                    // SqlConnection conn = new SqlConnection(connString);
                    connection();
                    dt = new DataTable();
                    string Query = "";
                //conn.Open();
                BudgetingOpenConnection();
                    if(DateTime.Now.Year + 1 >= 2024) //vkm yr = cy + 1
                        Query = " Select Year, [Plan] = sum([Plan]), [Utilize] = sum([Utilize]) from [HC_Table] where year > '" + (DateTime.Now.Year - 2) + "' group by year";//get data of recent 3 yrs only
                    else
                        Query = " Select Year, [Plan] = sum([Plan]), [Utilize] = sum([Utilize]) from [HC_Table] group by year";
                    SqlCommand cmd = new SqlCommand(Query, budgetingcon);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                //budgetingcon.Close();
                BudgetingCloseConnection();
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
           // }

            //return View();
        }

        public ActionResult HCChart_Drilldown(string year_selected, string isPlan_Utilize)
        {

            var obj = "";
            DataTable dt = new DataTable();

            try
            {
                //create instanace of database connection
                // SqlConnection conn = new SqlConnection(connString);
                connection();
                dt = new DataTable();
                //conn.Open();
                BudgetingOpenConnection();
                string Query = " Select Role, [HC_SkillSet_Table].SkillSetName as SkillName, Year, [" + isPlan_Utilize + "] from [HC_Table] inner join [dbo].[HC_SkillSet_Table] on HC_Table.SkillSet = HC_SkillSet_Table.ID where Year =" + year_selected;
                SqlCommand cmd = new SqlCommand(Query, budgetingcon);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                //conn.Close();
                BudgetingCloseConnection();
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


            return Json(new { data = result1 }, JsonRequestBehavior.AllowGet);
            // }

            //return View();
        }

        public ActionResult TCData()
        {

            var obj = "";
            DataTable dt = new DataTable();

            try
            {
                //create instanace of database connection
                // SqlConnection conn = new SqlConnection(connString);
                connection();
                dt = new DataTable();
                string Query = "";
                //conn.Open();
                BudgetingOpenConnection();
               
                Query = " Select [Budget_Plan] = Budget_Plan, [Available] = Budget_Plan - (Invoice + [Open]) from [TravelCost_Table] where year = '" + DateTime.Now.Year + "'";//get data of recent 3 yrs only
               
                SqlCommand cmd = new SqlCommand(Query, budgetingcon);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                //conn.Close();
                BudgetingCloseConnection();
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
            // }

            //return View();
        }

        public ActionResult VKMSummary()
        {
            return View();
        }

    }
}




