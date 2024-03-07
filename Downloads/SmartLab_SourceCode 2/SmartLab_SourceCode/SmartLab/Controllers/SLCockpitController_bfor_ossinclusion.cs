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
    //[Authorize(Users = @"apac\din2cob,apac\jov6cob,apac\mta2cob,apac\muu4cob,apac\sbr2kor,apac\chb1kor,apac\oig1cob,apac\mae9cob,apac\rau2kor,apac\rma5cob, apac\pch2kor, apac\mxk8kor, apac\ghb1cob, apac\vvs2kor")]
    public class SLCockpitController : Controller
    {
        private SqlConnection con, budgetingcon;      
        public static List<SPOTONData_Table_2021> lstUsers = new List<SPOTONData_Table_2021>();
        public static List<BU_Table> lstBUs = new List<BU_Table>();

        public static List<DEPT_Table> lstDEPTs = new List<DEPT_Table>();
        //public static List<Groups_Table> lstGroups = new List<Groups_Table>();
        public static List<OEM_Table> lstOEMs = new List<OEM_Table>();

        public static List<Category_Table> lstPrdCateg = new List<Category_Table>();
        public static List<ItemsCostList_Table> lstItems = new List<ItemsCostList_Table>();
        public static List<CostElement_Table> lstCostElement = new List<CostElement_Table>();
        public static List<tbl_UserIDs_Table> lstPrivileged = new List<tbl_UserIDs_Table>();
        public static List<BU_SPOCS> lstBU_SPOCs = new List<BU_SPOCS>();
        public static List<Groups_Table_Test> lstGroups_test = new List<Groups_Table_Test>(); //with old new groups

        public string authorise()
        {
            string NTID = "";
            connection();
            string qry = " Select isnull(ADSID,'') as NTID from Cockpit_UserIDs_table where ADSID = '" + User.Identity.Name.Split('\\')[1].Trim() + "'";
            BudgetingOpenConnection();
            SqlCommand command = new SqlCommand(qry, budgetingcon);
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
            BudgetingCloseConnection();
            return NTID;
        }

        public ActionResult SLAuthorise()
        {
            string NTID = authorise();
           
             return Json(new { data = NTID }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Index()
        {

            string NTID = authorise();
            if (NTID == "")
            {
                //throw new HttpException(404, "Sorry! Current user is not authorised to access this view!");
                return Content("Sorry! Current user is not authorised to access this view!");
            }
            else
            {


                LCfilterInfo lc = new LCfilterInfo();
                lc.startDate = new DateTime(DateTime.Now.Year, 01, 01);
                lc.endDate = DateTime.Now;
                //lc.startDate = DateTime.Now;
                //lc.endDate = DateTime.Now;




                using (BudgetingToolServerDBEntities1 db = new BudgetingToolServerDBEntities1())
                {
                    if (lstUsers == null || lstUsers.Count == 0)
                        lstUsers = db.SPOTONData_Table_2021.ToList<SPOTONData_Table_2021>();
                    if (lstOEMs == null || lstOEMs.Count == 0)
                        lstOEMs = db.OEM_Table.ToList<OEM_Table>();
                    if (lstBUs == null || lstBUs.Count == 0)
                        lstBUs = db.BU_Table.ToList<BU_Table>();
                    if (lstDEPTs == null || lstDEPTs.Count == 0)
                        lstDEPTs = db.DEPT_Table.ToList<DEPT_Table>();
                    if (lstCostElement == null || lstCostElement.Count == 0)
                        lstCostElement = db.CostElement_Table.ToList<CostElement_Table>();
                    if (lstPrdCateg == null || lstPrdCateg.Count == 0)
                        lstPrdCateg = db.Category_Table.ToList<Category_Table>();
                    if (lstItems == null || lstItems.Count == 0)
                        lstItems = db.ItemsCostList_Table.ToList<ItemsCostList_Table>()/*.FindAll(item=>item.Deleted != true)*/; //refresh this
                    if (lstPrivileged == null || lstPrivileged.Count == 0)
                        lstPrivileged = db.tbl_UserIDs_Table.ToList<tbl_UserIDs_Table>();
                    if (lstBU_SPOCs == null || lstBU_SPOCs.Count == 0)
                        lstBU_SPOCs = db.BU_SPOCS.ToList<BU_SPOCS>();
                    if (lstGroups_test == null || lstGroups_test.Count == 0)
                        lstGroups_test = db.Groups_Table_Test.ToList<Groups_Table_Test>();




                    lstUsers.Sort((a, b) => a.EmployeeName.CompareTo(b.EmployeeName));
                    lstBUs.Sort((a, b) => a.BU.CompareTo(b.BU));
                    lstOEMs.Sort((a, b) => a.OEM.CompareTo(b.OEM));
                    lstDEPTs.Sort((a, b) => a.DEPT.CompareTo(b.DEPT));
                    lstCostElement.Sort((a, b) => a.CostElement.CompareTo(b.CostElement));
                    lstPrdCateg.Sort((a, b) => a.Category.CompareTo(b.Category));
                    lstItems.Sort((a, b) => a.Item_Name.CompareTo(b.Item_Name));
                    lstPrivileged.Sort((a, b) => a.ADSID.CompareTo(b.ADSID));
                    lstBU_SPOCs.Sort((a, b) => a.VKMspoc.CompareTo(b.VKMspoc));
                    lstGroups_test.Sort((a, b) => a.Group.CompareTo(b.Group));

                }
                return View(lc);
            }
           
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
            //var password = "serveruser@12345"; //password

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
            try
            {
                DataTable dt = new DataTable();
                string Query = "";
                connection();
                OpenConnection();
                Query = " Exec [dbo].[GetInfraUtilization] '" + SDate + "', '" + EDate + "', '" + Location + "' ";
                // Query = " Exec [dbo].[GetInfraUtilization] ";
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

                WriteLog("Infra utilization fetched successfully " + DateTime.Now.ToString());
                return Json(new { data = result1 }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                WriteErrorLog("Error in fetching Infra Utilisation: " + ex.Message.ToString() + " "  + DateTime.Now.ToString());
                return Json(new { data = "" }, JsonRequestBehavior.AllowGet);
            }
            finally { 
            }
        }

        //public ActionResult GetTSOUDetails()
        //{
        //    Del_list Res = new Del_list();
        //    Res.tsou = GetTSOU();
        //}

        ///*private static LCfilterInfo.LabBookingExport*/
        public ActionResult GetTSOU(DateTime SDate, DateTime EDate, string Location, string LabType)
        {
            try
            {
                double overallManualhrs;
                double overallAutomatedhrs;
                double overallAutomatedCaplhrs;
                double overallManualCaplhrs;
                double overallDowntimehours;
                DataTable dt = new DataTable();
                string Query = "";
                string constring = ConfigurationManager.ConnectionStrings["dbConnection"].ToString();
                SqlConnection con = new SqlConnection();
                con = new SqlConnection(constring);
                con.Open();

            Query = " Exec [dbo].[GetTSOU_Chart] '" + SDate + "', '" + EDate + "', '" + Location + "' , '" + LabType + "'";
           // Query = " Exec [dbo].[GetTSOU_Chart] ";
            SqlCommand cmd = new SqlCommand(Query, con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();

            overallManualhrs = 0;
            overallAutomatedhrs = 0;
            overallAutomatedCaplhrs = 0;
            overallManualCaplhrs = 0;
            overallDowntimehours = 0;

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
                
                LabArray[i].TSOULabel =/* dt.Rows[i]["Model"].ToString() + " " + */dt.Rows[i]["Inventory"].ToString() + " " + dt.Rows[i]["RbCode"].ToString();
                LabArray[i].DowntimeHours = Convert.ToDouble(dt.Rows[i]["DowntimeHours"].ToString());
                LabArray[i].TotalSum = LabArray[i].ManualTotalHours + LabArray[i].ManualCaplTotalHours + LabArray[i].AutomatedTotalHours + LabArray[i].AutomatedCaplTotalHours +
                                            LabArray[i].DowntimeHours;
                    overallManualhrs += Convert.ToDouble(dt.Rows[i]["ManualTotalHours"].ToString());
                overallManualCaplhrs += Convert.ToDouble(dt.Rows[i]["ManualCaplTotalHours"].ToString());
                overallAutomatedhrs += Convert.ToDouble(dt.Rows[i]["AutomatedTotalHours"].ToString());
                overallAutomatedCaplhrs += Convert.ToDouble(dt.Rows[i]["AutomatedCaplTotalHours"].ToString());
                overallDowntimehours += Convert.ToDouble(dt.Rows[i]["DowntimeHours"].ToString());

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
            objBookingExport.OverallDowntimeHours = overallDowntimehours;

                WriteLog("TSOU Chart data fetched successfully " + DateTime.Now.ToString());
                return Json(new { data = objBookingExport }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                WriteErrorLog("Error in fetching TSOU Chart data " + ex.Message.ToString() + " " + DateTime.Now.ToString());
                return Json(new { data = "" }, JsonRequestBehavior.AllowGet);
            }
            finally
            {

            }
            //return objBookingExport;
        }


        public ActionResult GetTSOUOEM(DateTime SDate, DateTime EDate, string Location, string LabType)
        {
            try
            {
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
                //LabArray[i].TSOULabel = dt.Rows[i]["Model"].ToString() + " " + dt.Rows[i]["RbCode"].ToString();
                overallManualhrs += Convert.ToDouble(dt.Rows[i]["ManualTotalHours"].ToString());
                overallManualCaplhrs += Convert.ToDouble(dt.Rows[i]["ManualCaplTotalHours"].ToString());
                overallAutomatedhrs += Convert.ToDouble(dt.Rows[i]["AutomatedTotalHours"].ToString());
                overallAutomatedCaplhrs += Convert.ToDouble(dt.Rows[i]["AutomatedCaplTotalHours"].ToString());
                LabArray[i].TotalSum = LabArray[i].ManualTotalHours + LabArray[i].ManualCaplTotalHours + LabArray[i].AutomatedTotalHours + LabArray[i].AutomatedCaplTotalHours;


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

                WriteLog("TSOU OEM Chart data fetched successfully " + DateTime.Now.ToString());
                return Json(new { data = objBookingExport }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                WriteErrorLog("Error in fetching TSOU OEM data " + ex.Message.ToString() + " " + DateTime.Now.ToString());
                return Json(new { data = "" }, JsonRequestBehavior.AllowGet);
            }
            finally
            {

            }
        }


        public ActionResult GetTSOUDetails(string SDate, string EDate, string Location, string LabType)
        {
            Del_list Res = new Del_list();
            try
            {
                
                Res.tsou = GetTSOU(SDate, EDate, Location, LabType);
                Res.tsouoem = GetTSOUOEM(SDate, EDate, Location, LabType);
                TempData["Location"] = Location;
                TempData["LabType"] = LabType;

                WriteLog("TSOU & OEM Chart data fetched successfully " + DateTime.Now.ToString());
                return View(Res);
            }
            catch (Exception ex)
            {
                WriteErrorLog("Error in fetching TSOU & OEM Chart data " + ex.Message.ToString() + " "  + DateTime.Now.ToString());
                return View(Res);
            }
        }

        private static LabBookingExport GetTSOUOEM(string SDate, string EDate, string Location, string LabType)
        {
            //DateTime SDate = new DateTime(DateTime.Now.Year, 01, 01);
            //DateTime EDate = DateTime.Now;
            //string Location = "Ban,Cob";
            //string LabType = "CCHIL";
            double overallManualhrs;
            double overallAutomatedhrs;
            double overallAutomatedCaplhrs;
            double overallManualCaplhrs;
            double overallDowntimehours;
            DataTable dt = new DataTable();
            string Query = "";
            string constring = ConfigurationManager.ConnectionStrings["dbConnection"].ToString();
            SqlConnection con = new SqlConnection();
            con = new SqlConnection(constring);
            con.Open();

            Query = " Exec [dbo].[GetTSOUByOEM_Chart] '" + SDate + "', '" + EDate + "', '" + Location + "' , '" + LabType + "'";
            //Query = " Exec [dbo].[GetTSOU_Chart] ";
            SqlCommand cmd = new SqlCommand(Query, con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();

            overallManualhrs = 0;
            overallAutomatedhrs = 0;
            overallAutomatedCaplhrs = 0;
            overallManualCaplhrs = 0;
            overallDowntimehours = 0;

            LabBookingExport objBookingExport = new LabBookingExport();
            objBookingExport = new LabBookingExport();

            LabBookingExportLab[] LabArray = new LabBookingExportLab[dt.Rows.Count];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //LCfilterInfo.LabBookingExportLab LabArray = new LCfilterInfo.LabBookingExportLab();

                LabArray[i] = new LabBookingExportLab();
                //LabArray[i].id = (ushort)Convert.ToUInt32(dt.Rows[i]["Id"].ToString());
                LabArray[i].Model = dt.Rows[i]["Model"].ToString();
                //LabArray[i].Inventory = dt.Rows[i]["Inventory"].ToString();
                LabArray[i].Location = dt.Rows[i]["RbCode"].ToString();
                LabArray[i].OEM = dt.Rows[i]["OEM"].ToString().ToUpper();
                LabArray[i].ManualTotalHours = Convert.ToDouble(dt.Rows[i]["ManualTotalHours"].ToString());
                LabArray[i].ManualCaplTotalHours = Convert.ToDouble(dt.Rows[i]["ManualCaplTotalHours"].ToString());
                LabArray[i].AutomatedTotalHours = Convert.ToDouble(dt.Rows[i]["AutomatedTotalHours"].ToString());
                LabArray[i].AutomatedCaplTotalHours = Convert.ToDouble(dt.Rows[i]["AutomatedCaplTotalHours"].ToString());
                LabArray[i].DowntimeHours = Convert.ToDouble(dt.Rows[i]["DowntimeHours"].ToString());
                LabArray[i].TotalSum = LabArray[i].ManualTotalHours + LabArray[i].ManualCaplTotalHours + LabArray[i].AutomatedTotalHours + LabArray[i].AutomatedCaplTotalHours +
                                             LabArray[i].DowntimeHours;
               

                LabArray[i].tsoulabel = dt.Rows[i]["Model"].ToString() + " " + dt.Rows[i]["RbCode"].ToString();
                overallManualhrs += Convert.ToDouble(dt.Rows[i]["ManualTotalHours"].ToString());
                overallManualCaplhrs += Convert.ToDouble(dt.Rows[i]["ManualCaplTotalHours"].ToString());
                overallAutomatedhrs += Convert.ToDouble(dt.Rows[i]["AutomatedTotalHours"].ToString());
                overallAutomatedCaplhrs += Convert.ToDouble(dt.Rows[i]["AutomatedCaplTotalHours"].ToString());
                overallDowntimehours += Convert.ToDouble(dt.Rows[i]["DowntimeHours"].ToString());

            }

            objBookingExport.Labs = LabArray;
            objBookingExport.StartDate_UI = SDate;
            objBookingExport.EndDate_UI = EDate;
            objBookingExport.StartDate = Convert.ToDateTime(SDate);
            objBookingExport.EndDate = Convert.ToDateTime(EDate);
            objBookingExport.OverallManualHours = overallManualhrs;
            objBookingExport.OverallManualCaplHours = overallManualCaplhrs;
            objBookingExport.OverallAutomatedHours = overallAutomatedhrs;
            objBookingExport.OverallAutomatedCaplHours = overallAutomatedCaplhrs;
            objBookingExport.OverallDowntimeHours = overallDowntimehours;


            //  return Json(new { data = objBookingExport }, JsonRequestBehavior.AllowGet);
            return objBookingExport;
        }

        private static LCfilterInfo.LabBookingExport GetTSOU(string SDate, string EDate, string Location, string LabType)
        {
            double overallManualhrs;
            double overallAutomatedhrs;
            double overallAutomatedCaplhrs;
            double overallManualCaplhrs;
            double overallDowntimehours;
            DataTable dt = new DataTable();
            string Query = "";
            string constring = ConfigurationManager.ConnectionStrings["dbConnection"].ToString();
            SqlConnection con = new SqlConnection();
            con = new SqlConnection(constring);
            con.Open();

            Query = " Exec [dbo].[GetTSOU_Chart] '" + SDate + "', '" + EDate + "', '" + Location + "' , '" + LabType + "'";
            //Query = " Exec [dbo].[GetTSOU_Chart] ";
            SqlCommand cmd = new SqlCommand(Query, con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            con.Close();

            overallManualhrs = 0;
            overallAutomatedhrs = 0;
            overallAutomatedCaplhrs = 0;
            overallManualCaplhrs = 0;
            overallDowntimehours = 0;

            LCfilterInfo.LabBookingExport objBookingExport = new LCfilterInfo.LabBookingExport();
            //objBookingExport = new LabBookingExport();

            LCfilterInfo.LabBookingExportLab[] LabArray = new LCfilterInfo.LabBookingExportLab[dt.Rows.Count];

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //LCfilterInfo.LabBookingExportLab LabArray = new LCfilterInfo.LabBookingExportLab();

                LabArray[i] = new LCfilterInfo.LabBookingExportLab();
                LabArray[i].id = (ushort)Convert.ToUInt32(dt.Rows[i]["Id"].ToString());
                LabArray[i].Model = dt.Rows[i]["Model"].ToString();
                LabArray[i].Inventory = dt.Rows[i]["Inventory"].ToString();
                LabArray[i].Location = dt.Rows[i]["RbCode"].ToString();
                LabArray[i].OEM = dt.Rows[i]["OEM"].ToString().ToUpper();
                LabArray[i].ManualTotalHours = Convert.ToDouble(dt.Rows[i]["ManualTotalHours"].ToString());
                LabArray[i].ManualCaplTotalHours = Convert.ToDouble(dt.Rows[i]["ManualCaplTotalHours"].ToString());
                LabArray[i].AutomatedTotalHours = Convert.ToDouble(dt.Rows[i]["AutomatedTotalHours"].ToString());
                LabArray[i].AutomatedCaplTotalHours = Convert.ToDouble(dt.Rows[i]["AutomatedCaplTotalHours"].ToString());
                LabArray[i].DowntimeHours = Convert.ToDouble(dt.Rows[i]["DowntimeHours"].ToString());
                LabArray[i].TotalSum = LabArray[i].ManualTotalHours + LabArray[i].ManualCaplTotalHours + LabArray[i].AutomatedTotalHours + LabArray[i].AutomatedCaplTotalHours +
                                            LabArray[i].DowntimeHours;
                LabArray[i].TSOULabel = /*dt.Rows[i]["Model"].ToString() + " " +*/ dt.Rows[i]["Inventory"].ToString().Replace("OTB_AS_","").Trim().Replace("CCSIL_AS_", "").Trim() + " " + dt.Rows[i]["RbCode"].ToString();
               
                overallManualhrs += Convert.ToDouble(dt.Rows[i]["ManualTotalHours"].ToString());
                overallManualCaplhrs += Convert.ToDouble(dt.Rows[i]["ManualCaplTotalHours"].ToString());
                overallAutomatedhrs += Convert.ToDouble(dt.Rows[i]["AutomatedTotalHours"].ToString());
                overallAutomatedCaplhrs += Convert.ToDouble(dt.Rows[i]["AutomatedCaplTotalHours"].ToString());
                overallDowntimehours += Convert.ToDouble(dt.Rows[i]["DowntimeHours"].ToString());

                //if(LabArray[i].DowntimeHours > 0)
                //{
                //    var v= 0;
                //}
            }



            objBookingExport.Labs = LabArray;
            //objBookingExport.StartDate_UI = SDate.ToShortDateString();
            //objBookingExport.EndDate_UI = EDate.ToShortDateString();
            objBookingExport.StartDate_UI = SDate;
            objBookingExport.EndDate_UI = EDate;
            objBookingExport.StartDate = Convert.ToDateTime(SDate);
            objBookingExport.EndDate = Convert.ToDateTime(EDate);
            objBookingExport.OverallManualHours = overallManualhrs;
            objBookingExport.OverallManualCaplHours = overallManualCaplhrs;
            objBookingExport.OverallAutomatedHours = overallAutomatedhrs;
            objBookingExport.OverallAutomatedCaplHours = overallAutomatedCaplhrs;
            objBookingExport.OverallDowntimeHours = overallDowntimehours;


            //return Json(new { data = objBookingExport }, JsonRequestBehavior.AllowGet);
            return objBookingExport;
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
                WriteLog("Head Count Chart data fetched successfully " + DateTime.Now.ToString());
            }


            




                catch (Exception ex)
                {
                WriteErrorLog("Error in fetching Head Count Chart data " + ex.Message.ToString() + " " + DateTime.Now.ToString());
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
            Del_list Res = new Del_list();
            DataTable dt = new DataTable();

            try
            {
                //create instanace of database connection
                // SqlConnection conn = new SqlConnection(connString);
                connection();
                dt = new DataTable();
                //conn.Open();
                BudgetingOpenConnection();
                string Query = "SELECT" +
                    "( select ',' + CM.SkillSetName  from dbo.HC_SkillSet_Table as CM  where ',' + SM.SkillSet + ',' like '%,' + convert(varchar, CM.ID) + ',%'  for xml path(''), type ).value('substring(text()[1], 2)', 'varchar(max)') as SkillSetName, " +

                    " [HC_Role_Table].RoleName as Role, Year, isnull([Plan],0) as [Plan] , isnull([Utilize],0) as Utilize from [HC_Table] as SM inner join [dbo].[HC_Role_Table] on SM.Role = HC_Role_Table.ID where Year =" + year_selected;
                //,isnull( [" + isPlan_Utilize + "] ,0) as [" + isPlan_Utilize + "]
                SqlCommand cmd = new SqlCommand(Query, budgetingcon);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                //conn.Close();
                BudgetingCloseConnection();

                List<HeadCount> ds = new List<HeadCount>();
                foreach (DataRow row in dt.Rows)
                {
                    HeadCount del = new HeadCount();
                    del.SkillSetName = row["SkillSetName"].ToString();
                    del.Role = row["Role"].ToString();
                    del.Year = Convert.ToInt32(row["Year"].ToString());

                  //  if (isPlan_Utilize.ToLower() == "utilize")
                  //  {
                   //     del.Plan = 0;
                        del.Utilize = Convert.ToDouble(row["Utilize"].ToString());
                   // }
                   // else
                   // {
                   //     del.Utilize = 0;
                        del.Plan = Convert.ToDouble(row["Plan"].ToString());
                   // }
                    ds.Add(del);
                }

                Res.hclist = ds;
                TempData["isPlan_Utilize"] = isPlan_Utilize.ToString();
                WriteLog("Head Count drill down Chart data fetched successfully " + DateTime.Now.ToString());

            }
            catch (Exception ex)
            {
                WriteErrorLog("Error in fetching Head Count drill down Chart data " + ex.Message.ToString() + " " + DateTime.Now.ToString());
            }
            finally
            {

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


            //return Json(new { data = result1 }, JsonRequestBehavior.AllowGet);
            // }

            //return View();

            return View(Res);
        }

        public ActionResult TCData(string Year)
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
               
                Query = " Select [Budget_Plan] = Budget_Plan, [Available] = Budget_Plan - (Invoice + [Open]) from [TravelCost_Table] where year = '" + Year + "' and Cmmt_Item = 106";//get data of recent 3 yrs only
               
                SqlCommand cmd = new SqlCommand(Query, budgetingcon);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                //conn.Close();
                BudgetingCloseConnection();
                WriteLog("Travel Cost data fetched successfully " + DateTime.Now.ToString());
            }







            catch (Exception ex)
            {
                WriteErrorLog("Error in fetching Travel Cost data " + ex.Message.ToString() + " " + DateTime.Now.ToString());
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
                    childRow.Add(col.ColumnName, decimal.Parse(row[col].ToString()).ToString("C0", CultureInfo.CurrentCulture));
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


        public ActionResult TC_Drilldown(string Year = "")
        {
            TC_List Res = new TC_List();
            DataTable dt = new DataTable();
            connection();
            dt = new DataTable();
            //conn.Open();
            BudgetingOpenConnection();
            string Query = "";
            if (Year == "")
                Query = " Select ID,Cmmt_Item,[Year],Budget_Plan,Invoice,[Open], [Bud_Inv], [Available] from [TravelCost_Table] ";
            else
                Query = " Select ID,Cmmt_Item,[Year],Budget_Plan,Invoice,[Open], [Bud_Inv], [Available] from [TravelCost_Table] where Year =" + Year + " and Cmmt_Item = 106";
            SqlCommand cmd = new SqlCommand(Query, budgetingcon);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            //conn.Close();
            BudgetingCloseConnection();

            List<TCView> ds = new List<TCView>();
            foreach (DataRow row in dt.Rows)
            {
                TCView del = new TCView();
                del.ID = Convert.ToInt32(row["ID"]);
                del.Cmmt_Item = Convert.ToInt32(row["Cmmt_Item"]);
                del.Year = Convert.ToInt32(row["Year"]);
                del.Budget_Plan = Convert.ToDecimal(row["Budget_Plan"]);
                del.Invoice = Convert.ToDecimal(row["Invoice"]);
                del.Open = Convert.ToDecimal(row["Open"]);
                del.Available = Convert.ToDecimal(row["Available"]);
                del.Bud_Inv = Convert.ToDecimal(row["Bud_Inv"]);

                ds.Add(del);
            }
            Res.tclist = ds;

            return View(Res);

            //JsonResult result1 = GetTC(Year);
            //return Json(new { data = result1 }, JsonRequestBehavior.AllowGet);




        }




        public ActionResult VKMSummary()
        {
            return View();
        }


        public ActionResult GetHwDamage_Totals(string hwDamage_date)
        {

            var Year_sel = DateTime.Parse(hwDamage_date).Year;
            var Month_sel = DateTime.Parse(hwDamage_date).Month;
            //DataTable dt = new DataTable();
            string SelTotal = "",
                   PrevTotal = "";


            try
            {
                //create instanace of database connection
                // SqlConnection conn = new SqlConnection(connString);
                connection();
                //dt = new DataTable();
                string Query = "";
                string Query1 = "";
                //conn.Open();
                BudgetingOpenConnection();

                Query = " SELECT sum(Total_Price) as SelectedTotal from HwDamageCost_Table WHERE Year = " + Year_sel + " and Month <= " + Month_sel + "";
                Query1 = " SELECT sum(Total_Price) as SelectedTotal from HwDamageCost_Table WHERE Year = " + (Year_sel-1) + " and Month <= 12"; //prev yr of selected

                SqlCommand cmd = new SqlCommand(Query, budgetingcon);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                SelTotal = cmd.ExecuteScalar().ToString();
                cmd = new SqlCommand(Query1, budgetingcon);
                da = new SqlDataAdapter(cmd);
                PrevTotal = cmd.ExecuteScalar().ToString();
                BudgetingCloseConnection();

                WriteLog("HW damage Totals fetched successfully " + DateTime.Now.ToString());
            }
            catch (Exception ex)
            {
                WriteErrorLog("Error in fetching HW Damage totals: " + ex.Message.ToString() + " " + DateTime.Now.ToString());
            }
            finally
            {

            }

            return Json(new { data1 = SelTotal, data2 = PrevTotal }, JsonRequestBehavior.AllowGet);
            // }

            //return View();
        }


        //********************************CHART*************************************


        public ActionResult HWDamageOEM (string charttype, string hwDamage_date)
        {
            Del_list Res = new Del_list();
            try
            {
                var Year_sel = DateTime.Parse(hwDamage_date).Year;
                var Month_sel = DateTime.Parse(hwDamage_date).Month;
                DataTable dt = new DataTable();
                string Query = "";
                connection();
                BudgetingOpenConnection();
                if (charttype == "OEM")
                {
                    Query = "select  [HwDamageCost_Month_Table].Month,[HwDamageCost_Table].Month as MID, [HwDamageCost_Table].[Year], [OEM_Table].OEM, sum(Total_Price) as Damage_Cost from HwDamageCost_Table inner join [dbo].[HwDamageCost_Month_Table] on HwDamageCost_Table.Month = HwDamageCost_Month_Table.ID inner join [dbo].[OEM_Table] on HwDamageCost_Table.Project_Team = [OEM_Table].ID  WHERE [HwDamageCost_Table].Year = " + Year_sel + " and [HwDamageCost_Table].Month <= " + Month_sel + " group by  Project_Team,[HwDamageCost_Month_Table].Month, [HwDamageCost_Month_Table].Month, [HwDamageCost_Table].[Year],[HwDamageCost_Table].Month, [OEM_Table].OEM order by convert(INT,[HwDamageCost_Table].Month)";
                }
                SqlCommand cmd = new SqlCommand(Query, budgetingcon);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                BudgetingCloseConnection();

            List<HWOEM> ds = new List<HWOEM>();
            foreach (DataRow row in dt.Rows)
            {
                HWOEM del = new HWOEM();
                del.MonthOEM = row["OEM"].ToString() + "_" + row["Month"].ToString();
                del.Month = row["Month"].ToString();
                del.MID   = Convert.ToInt32(row["MID"]);
                del.Year = Convert.ToInt32(row["Year"]);
                del.OEM = row["OEM"].ToString();
                del.Damage_Cost = Convert.ToDouble(row["Damage_Cost"]);
                ds.Add(del);
            }

                Res.hwoem = ds;

                TempData["oem_EndDate"] = Convert.ToDateTime(hwDamage_date).ToString("MM/yyyy");
                return View(Res);
            }
            catch(Exception ex)
            {
                WriteErrorLog("Error in fetching HW Damage totals: " + ex.Message.ToString() + " " + DateTime.Now.ToString());
                return View(Res);
            }
        }





        [HttpPost]
        public ActionResult GenerateHwDamage_chart(string charttype, string hwDamage_date)
        {
            DataTable dt = new DataTable();
            var Year_sel = DateTime.Parse(hwDamage_date).Year;
            var Month_sel = DateTime.Parse(hwDamage_date).Month;
            try
            {
                //create instanace of database connection
                // SqlConnection conn = new SqlConnection(connString);
                connection();
                dt = new DataTable();
                //conn.Open();
                BudgetingOpenConnection();
                string Query = "";
                if (charttype == "Month")
                {
                    Query = " select [HwDamageCost_Month_Table].Month, [HwDamageCost_Table].Month as MID, [HwDamageCost_Table].[Year], sum(Total_Price) as Damage_Cost from HwDamageCost_Table inner join [dbo].[HwDamageCost_Month_Table] on HwDamageCost_Table.Month = HwDamageCost_Month_Table.ID  WHERE [HwDamageCost_Table].Year = " + Year_sel + " and [HwDamageCost_Table].Month <= " + Month_sel + " group by [HwDamageCost_Month_Table].Month, [HwDamageCost_Month_Table].Month,[HwDamageCost_Table].Month, [HwDamageCost_Table].[Year] order by convert(INT,[HwDamageCost_Table].Month) ";
                }
                if (charttype == "OEM")
                {
                    Query = "select  [HwDamageCost_Month_Table].Month,[HwDamageCost_Table].Month as MID, [HwDamageCost_Table].[Year], [OEM_Table].OEM, sum(Total_Price) as Damage_Cost from HwDamageCost_Table inner join [dbo].[HwDamageCost_Month_Table] on HwDamageCost_Table.Month = HwDamageCost_Month_Table.ID inner join [dbo].[OEM_Table] on HwDamageCost_Table.Project_Team = [OEM_Table].ID  WHERE [HwDamageCost_Table].Year = " + Year_sel + " and [HwDamageCost_Table].Month <= " + Month_sel + " group by  Project_Team,[HwDamageCost_Month_Table].Month, [HwDamageCost_Month_Table].Month, [HwDamageCost_Table].[Year],[HwDamageCost_Table].Month, [OEM_Table].OEM order by convert(INT,[HwDamageCost_Table].Month)";
                }
                SqlCommand cmd = new SqlCommand(Query, budgetingcon);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                BudgetingCloseConnection();
                WriteLog("HW Damage Chart data fetched successfully " + DateTime.Now.ToString());
            }

            catch (Exception ex)
            {
                WriteErrorLog("Error in fetching HW Damage chart data: " + ex.Message.ToString() + " " + DateTime.Now.ToString());
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
        }



        //********************** PO *********************

        public ActionResult GetPO_Totals(DateTime SDate, DateTime EDate)
        {

            DataTable dt = new DataTable();

            try
            {
                //create instanace of database connection
                // SqlConnection conn = new SqlConnection(connString);
                connection();
                //dt = new DataTable();
                string Query = "";
                //conn.Open();
                BudgetingOpenConnection();
                //left join = returns all records from the left table(table1), and the matching records from the right table(table2).The result is 0 records from the right side, if there is no match.

               Query = "Select count(A.Id) as TotalItems,count(B.id) as OnTimeDelivery from( (select RequestId as Id from RequestItems_Table where convert(date, ActualDeliveryDate) >= '" + SDate.ToShortDateString() + "' and convert(date, ActualDeliveryDate) <= '" + EDate.ToShortDateString() + "') as A left join (Select RequestId as Id from RequestItems_Table where convert(date, ActualDeliveryDate) >= '" + SDate.ToShortDateString() + "' and convert(date, ActualDeliveryDate) <= '" + EDate.ToShortDateString() + "' and datediff(day,convert(date,TentativeDeliveryDate), convert(date,ActualDeliveryDate)) <= 0 ) as B on A.Id = B.Id ) ";
                    
                SqlCommand cmd = new SqlCommand(Query, budgetingcon);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                BudgetingCloseConnection();
                WriteLog("PO Totals fetched successfully " + DateTime.Now.ToString());
            }
            catch (Exception ex)
            {
                WriteLog("Error in fetching PO Totals: " + ex.Message.ToString() + " " + DateTime.Now.ToString());
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


        //public ActionResult GetDeliveryStatus()
        //{
        //    return View();
        //}

        
        public ActionResult GetDeliveryStatus(string SDate, string EDate)
        {

            try
            {
                Del_list Res = new Del_list();
                //SDate = new DateTime(DateTime.Now.Year, 08, 01);
                //EDate = new DateTime(DateTime.Now.Year, 08, 31);
                DataTable dt = new DataTable();
                string Query = "";
                connection();
                BudgetingOpenConnection();
                Query = " Exec [dbo].[GetDeliveryStatus] '" + SDate + "', '" +  EDate + "' ";
                //Query = " Exec [dbo].[GetInfraUtilization] ";
                SqlCommand cmd = new SqlCommand(Query, budgetingcon);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                BudgetingCloseConnection();

                List<DeliveryStatus> ds = new List<DeliveryStatus>();
                foreach (DataRow row in dt.Rows)
                {
                    DeliveryStatus del = new DeliveryStatus();
                    del.Value = Convert.ToInt32(row["Value"]);
                    del.Description = row["Description"].ToString();
                    del.OrderBy = Convert.ToInt32(row["OrderBy"].ToString());
                   
                    ds.Add(del);
                }
                Res.del_status = ds;

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


                //return Json(new { data = result1 }, JsonRequestBehavior.AllowGet);
                //return PartialView("GetDeliveryStatus", Res);
                //return View(Res);
                TempData["Del_StartDate"] = Convert.ToDateTime(SDate).ToString("dd/MM/yyyy");
                TempData["Del_EndDate"] = Convert.ToDateTime(EDate).ToString("dd/MM/yyyy");

                WriteLog("Delivered Status data fetched successfully " + DateTime.Now.ToString());

                return View(Res);
                //return Json(new { data = Res }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                WriteErrorLog("Error in fetching delivered status: " + ex.Message.ToString() + " " + DateTime.Now.ToString());
                return Json(new { data = "" }, JsonRequestBehavior.AllowGet);

            }


        }

        [HttpPost]
        public ActionResult GetPurchaseOrderStatus(DateTime SDate, DateTime EDate)
        {
            try
            {
                //Del_list Res = new Del_list();
                //SDate = new DateTime(DateTime.Now.Year, 08, 01);
                //EDate = new DateTime(DateTime.Now.Year, 08, 31);
                DataTable dt = new DataTable();
                string Query = "";
                connection();
                BudgetingOpenConnection();
                Query = " Exec [dbo].[GetPurchaseOrderStatus] '" + SDate + "', '" + EDate + "' ";
                //Query = " Exec [dbo].[GetInfraUtilization] ";
                SqlCommand cmd = new SqlCommand(Query, budgetingcon);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                BudgetingCloseConnection();
                //List<PO> ds = new List<PO>();
                //foreach (DataRow row in dt.Rows)
                //{
                //    PO del = new PO();
                //    del.Value = Convert.ToInt32(row["Value"]);
                //    del.Description = row["Description"].ToString();
                //    del.OrderBy = Convert.ToInt32(row["OrderBy"].ToString());

                //    ds.Add(del);
                //}
                //Res.po_status = ds;
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

                WriteLog("Purchase Order Status data fetched successfully " + DateTime.Now.ToString());
                return Json(new { data = result1 }, JsonRequestBehavior.AllowGet);
                //return View(Res);
            }
            catch (Exception ex)
            {
                WriteErrorLog("Error in fetching Purchase Order Status data: " + ex.Message.ToString() + " " + DateTime.Now.ToString());
                return Json(new { data = "" }, JsonRequestBehavior.AllowGet);

            }


        }

        void WriteLog(string Message)
        {
            string execPath = AppDomain.CurrentDomain.BaseDirectory;
            execPath = execPath + "Log\\log" + DateTime.Now.ToString("ddMMyyyy") + ".txt";
            StreamWriter file = new StreamWriter(execPath, append: true);
            file.WriteLine(Message + "\r\n");
            file.Close();
        }

        void WriteErrorLog(string Message)
        {
            string execPath = AppDomain.CurrentDomain.BaseDirectory;
            execPath = execPath + "ErrorLog\\log" + DateTime.Now.ToString("ddMMyyyy") + ".txt";
            StreamWriter file = new StreamWriter(execPath, append: true);
            file.WriteLine(Message + "\r\n");
            file.Close();
        }

        [HttpPost]
        public ActionResult UserGrp_Validate()
        {
            int UserGroup = 0 ;
            connection();
            string qry = " Select UserGroup as UserGroup from Cockpit_UserIDs_table where ADSID = '" + User.Identity.Name.Split('\\')[1].Trim() + "'";
            BudgetingOpenConnection();
            SqlCommand command = new SqlCommand(qry, budgetingcon);
            SqlDataReader dr = command.ExecuteReader();
            if (dr.HasRows)
            {
                dr.Read();
                UserGroup = int.Parse(dr["UserGroup"].ToString());

            }
            else
            {
                UserGroup = 3;
            }
            dr.Close();
            BudgetingCloseConnection();
            return Json(new {data = UserGroup }, JsonRequestBehavior.AllowGet);

        }

        public ActionResult DownloadTemplate()
        {
            string folderPath = Server.MapPath("~/Templates/");
            string fileName = "SLCockpitV1.pdf";
            byte[] fileBytes = System.IO.File.ReadAllBytes(folderPath + fileName);

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }




    }

    public class DeliveryStatus
    {
        public int Value { get; set; }
        public string Description { get; set; }
        public int OrderBy { get; set; }

    }

    public class Del_list
    {
       public List<DeliveryStatus> del_status { get; set; }
        public List<PO> po_status { get; set; }

        public LCfilterInfo.LabBookingExport tsou { get; set; }

        public LabBookingExport tsouoem { get; set; }
        public List<HWOEM> hwoem { get; set; }
        public List<HeadCount> hclist { get; set; }
    }


    public class PO
    {
        public int Value { get; set; }
        public string Description { get; set; }
        public int OrderBy { get; set; }

    }
    public class HWOEM
    {
        public string Month { get; set; }
        public string MonthOEM { get; set; }
        public int MID { get; set; }
        public int Year { get; set; }
        public string OEM { get; set; }
        public double Damage_Cost { get; set; }
    }
    public class HeadCount
    {
        public string SkillSetName { get; set; }
        public string Role { get; set; }
        public int Year { get; set; }
        public double Utilize { get; set; }
        public double Plan { get; set; }
    }


    public class LabBookingExport
    {

        private LabBookingExportLab[] labsField;

        private double overallManualhrs;
        private double overallAutomatedhrs;
        private double overallAutomatedCaplhrs;
        private double overallManualCaplhrs;
        private DateTime startDate;
        private DateTime endDate;

        private string ui_startDate;
        private string ui_endDate;
        public string StartDate_UI
        {
            get { return this.ui_startDate; }
            set { this.ui_startDate = value; }
        }

        public string EndDate_UI
        {
            get { return this.ui_endDate; }
            set { this.ui_endDate = value; }
        }


        public DateTime StartDate
        {
            get { return this.startDate; }
            set { this.startDate = value; }
        }

        public DateTime EndDate
        {
            get { return this.endDate; }
            set { this.endDate = value; }
        }

        public double OverallManualHours
        {
            get { return this.overallManualhrs; }
            set { this.overallManualhrs = value; }
        }
        public double OverallManualCaplHours
        {
            get { return this.overallManualCaplhrs; }
            set { this.overallManualCaplhrs = value; }
        }

        public double OverallAutomatedHours
        {
            get { return this.overallAutomatedhrs; }
            set { this.overallAutomatedhrs = value; }
        }
        public double OverallAutomatedCaplHours
        {
            get { return this.overallAutomatedCaplhrs; }
            set { this.overallAutomatedCaplhrs = value; }
        }
        public double OverallDowntimeHours
        {
            get;
            set;
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("Lab", IsNullable = false)]
        public LabBookingExportLab[] Labs
        {
            get
            {
                return this.labsField;
            }
            set
            {
                this.labsField = value;
            }
        }


    }

    /// <remarks/>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public partial class LabBookingExportLab
    {

        private string modelField;

        private string ownerField;

        private string descriptionField;

        private string inventoryField;

        private string locationField;

        private string subLocationField;

        private ushort idField;

        private string nameField;
        private string oemField;

        private double ManualTotalHoursField = 0;

        private double AutomatedTotalHoursField = 0;

        private double ManualCaplTotalHoursField = 0;

        private double AutomatedCaplTotalHoursField = 0;
        private string tsoulabelField;

        private double TotalSumField = 0;

        public double TotalSum
        {
            get
            {
                return this.TotalSumField;
            }
            set
            {
                this.TotalSumField = value;
            }
        }
        public double ManualTotalHours
        {
            get
            {
                return this.ManualTotalHoursField;
            }
            set
            {
                this.ManualTotalHoursField = value;
            }
        }

        public double AutomatedTotalHours
        {
            get
            {
                return this.AutomatedTotalHoursField;
            }
            set
            {
                this.AutomatedTotalHoursField = value;
            }
        }

        public double ManualCaplTotalHours
        {
            get
            {
                return this.ManualCaplTotalHoursField;
            }
            set
            {
                this.ManualCaplTotalHoursField = value;
            }
        }

        public double AutomatedCaplTotalHours
        {
            get
            {
                return this.AutomatedCaplTotalHoursField;
            }
            set
            {
                this.AutomatedCaplTotalHoursField = value;
            }
        }


        public double DowntimeHours { get; set; }



        /// <remarks/>
        public string tsoulabel
        {
            get
            {
                return this.tsoulabelField;
            }
            set
            {
                this.tsoulabelField = value;
            }
        }

        public string Model
        {
            get
            {
                return this.modelField;
            }
            set
            {
                this.modelField = value;
            }
        }

        public ushort id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        public string Owner
        {
            get
            {
                return this.ownerField;
            }
            set
            {
                this.ownerField = value;
            }
        }
        public string OEM
        {
            get
            {
                return this.oemField;
            }
            set
            {
                this.oemField = value;

            }
        }

        /// <remarks/>
        public string Description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public string Inventory
        {
            get
            {
                return this.inventoryField;
            }
            set
            {
                this.inventoryField = value;
            }
        }

        /// <remarks/>
        public string Location
        {
            get
            {
                return this.locationField;
            }
            set
            {
                this.locationField = value;
            }
        }

        /// <remarks/>
        public string SubLocation
        {
            get
            {
                return this.subLocationField;
            }
            set
            {
                this.subLocationField = value;
            }
        }

    }

}




