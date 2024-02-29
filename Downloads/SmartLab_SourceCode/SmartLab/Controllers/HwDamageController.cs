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
    public class HwDamageController : Controller
    {
       
        public static bool ServerTimeout = false;
        public static bool refresh = false;
        public static bool reload = false;
        //public static LCfilterInfo.LabBookingExport report = new LCfilterInfo.LabBookingExport();
        public static CPCReportAttributes cpcobj = new CPCReportAttributes();
        

        public static DateTime FromDate = DateTime.Now;
        public static DateTime ToDate = DateTime.Now;
        public static List<OEM_Table> lstOEMs = new List<OEM_Table>();
        public bool UserAutorizeToEdit = false;

        public ActionResult Index()
        {
            string NTID = authorise();
            if (NTID == "")
            {
                TempData["UserAuthorizeToEdit"] = false;
                UserAutorizeToEdit = false;

            }
            else
            {
                TempData["UserAuthorizeToEdit"] = true;
                UserAutorizeToEdit = true;
            }

            //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())
            //{
            //    if (lstOEMs == null || lstOEMs.Count == 0)
            //        lstOEMs = db.OEM_Table.ToList<OEM_Table>();
            //    lstOEMs.Sort((a, b) => a.OEM.CompareTo(b.OEM));
            //}
            //OEMList oemlist = new OEMList();
            //foreach (var oem in lstOEMs)
            //{
            //    oemlist.OEMSelectList.Add(new SelectListItem { Text = oem.OEM, Value = oem.ID.ToString() });
            //}
            //oemlist.OEMSelectList.Sort((a, b) => a.Text.CompareTo(b.Text));

            return View(/*oemlist*/);       
        }
       
        private SqlConnection conn;

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
            string connString = ConfigurationManager.ConnectionStrings["BudgetingdbConnection"].ToString();
            conn = new SqlConnection(connString);

            //string constring = ConfigurationManager.ConnectionStrings["dbConnection"].ToString();
            //conn = new SqlConnection(connString);
        }

        private void OpenConnection()
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
        }

        private void CloseConnection()
        {
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
        }

        //public ActionResult GenerateHC()
        //{
        //    var obj = "";
        //    DataTable dt = new DataTable();

        //    try
        //    {
        //        //create instanace of database connection
        //        // SqlConnection conn = new SqlConnection(connString);
        //        connection();
        //        dt = new DataTable();
        //        //conn.Open();
        //        OpenConnection();
        //        string Query = " Select [HC_Table].*,[HC_SkillSet_Table].SkillSetName from [dbo].[HC_Table] inner join [dbo].[HC_SkillSet_Table] on HC_Table.SkillSet = HC_SkillSet_Table.ID ";
        //        SqlCommand cmd = new SqlCommand(Query, conn);
        //        SqlDataAdapter da = new SqlDataAdapter(cmd);
        //        da.Fill(dt);
        //        //conn.Close();
        //        CloseConnection();
        //    }







        //    catch (Exception ex)
        //    {

        //    }
        //    finally
        //    {

        //    }

        //    JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
        //    List<Dictionary<string, object>> parentRow = new List<Dictionary<string, object>>();
        //    Dictionary<string, object> childRow;
        //    foreach (DataRow row in dt.Rows)
        //    {
        //        childRow = new Dictionary<string, object>();
        //        foreach (DataColumn col in dt.Columns)
        //        {
        //            childRow.Add(col.ColumnName, row[col]);
        //        }
        //        parentRow.Add(childRow);
        //    }

        //    jsSerializer.MaxJsonLength = Int32.MaxValue;

        //    var resultData = parentRow;
        //    var result = new ContentResult
        //    {
        //        Content = jsSerializer.Serialize(resultData),
        //        ContentType = "application/json"
        //    };

        //    JsonResult result1 = Json(result);
        //    result1.MaxJsonLength = Int32.MaxValue;
        //    result1.JsonRequestBehavior = JsonRequestBehavior.AllowGet;



        //    //return Json(new { data = jsSerializer.Serialize(parentRow) }, JsonRequestBehavior.AllowGet);

        //    //return Json(new { data = parentRow }, JsonRequestBehavior.AllowGet);

        //    return Json(new { data = result1 }, JsonRequestBehavior.AllowGet);
        //}

        //public ActionResult saveUtilizeCount(double newhc, double oldhc, List<string>row , List<string> column)
        //{
        //    DataTable dt = new DataTable();
        //    connection();
        //    dt = new DataTable();

        //    OpenConnection();
        //    string Query = " SELECT ID FROM[HC_SkillSet_Table] WHERE SkillSetName = '" + row[1] +"'";
        //    SqlCommand cmd = new SqlCommand(Query, conn);
        //    SqlDataAdapter da = new SqlDataAdapter(cmd);
        //    da.Fill(dt);

        //    CloseConnection();
        //    var skillsetid = dt.Rows[0].ItemArray[0];
        //    OpenConnection();

        //    Query = "UPDATE [HC_Table] SET Utilize = '" + newhc + "' WHERE SkillSet = '" + skillsetid + "'AND Role = '"+ row[0] +"'AND Year ='"+ column[0]+ "'";

        //    cmd = new SqlCommand(Query, conn);
        //    try
        //    {
        //        cmd.ExecuteNonQuery();
        //        //MessageBox.Show("Updated..");
        //    }
        //    catch (Exception)
        //    {
        //        //MessageBox.Show(" Not Updated");
        //    }
        //    finally
        //    {
        //        CloseConnection();
        //    }

        //    return Json(new { data = "Updated successfully" }, JsonRequestBehavior.AllowGet);
        //}


        public string authorise()
        {
            string NTID = "";
            connection();
            string qry = " Select isnull(ADSID,'') as NTID from HwDamageCost_UserIDs_Table where ADSID = '" + User.Identity.Name.Split('\\')[1].Trim() + "'";
            OpenConnection();
            SqlCommand command = new SqlCommand(qry, conn);
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
            return NTID;
        }

        public ActionResult SLAuthorise()
        {
            string NTID = authorise();

            return Json(new { data = NTID }, JsonRequestBehavior.AllowGet);
        }

        private JsonResult GetHwDamage(string yr/*string[] oem*/)
        {
            DataTable dt = new DataTable();

            try
            {
                //create instanace of database connection
                // SqlConnection conn = new SqlConnection(connString);
                string oemlist = string.Empty;
                //foreach (var p in oem)
                //{
                //    oemlist += ("'" + p.Trim() + "',");
                   
                //}

                //oemlist = oemlist.Remove(oemlist.Length - 1, 1);

                connection();
                dt = new DataTable();
                //conn.Open();
                OpenConnection();
                //string Query = " Select * from [HwDamageCost_Table] where Project_Team IN (" + oemlist + ") and BU IN (Select BU from HwDamageCost_UserIDs_Table where ADSID = '" + User.Identity.Name.Split('\\')[1].Trim() + "')";

                string Query = " Exec [dbo].[HWDamage] '" + User.Identity.Name.Split('\\')[1].Trim() + "', '" +yr + "' ";

                SqlCommand cmd = new SqlCommand(Query, conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                //conn.Close();
                CloseConnection();
            }

            catch (Exception ex)
            {

            }
            finally
            {

            }

            List<HwDamageCost> ds = new List<HwDamageCost>();
            foreach (DataRow row in dt.Rows)
            {
                HwDamageCost del = new HwDamageCost();
                del.ID = Convert.ToInt32(row["ID"]);
                del.BU = Convert.ToInt32(row["BU"]);
                del.Item_Name = Convert.ToInt32(row["Item_Name"]);
                del.Location = Convert.ToInt32(row["Location"]);
                del.Month = Convert.ToInt32(row["Month"]);
                del.Project_Team = Convert.ToInt32(row["Project_Team"]);
                del.Qty =  Convert.ToInt32(row["Qty"]);
                del.Year = Convert.ToInt32(row["Year"]);
                del.Cost_inEUR = Convert.ToInt32(row["Cost_inEUR"]);
                del.Total_Price = Convert.ToInt32(row["Total_Price"]);
                del.Hw_status = row["Hw_status"].ToString();
                del.RequestorNT = row["RequestorNT"].ToString();
                del.Remarks = row["Remarks"].ToString();
                ds.Add(del);
            }
            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
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

            jsSerializer.MaxJsonLength = Int32.MaxValue;

            //var resultData = parentRow;
            var result = new ContentResult
            {
                Content = jsSerializer.Serialize(ds),
                ContentType = "application/json"
            };

            JsonResult result1 = Json(result);
            result1.MaxJsonLength = Int32.MaxValue;
            result1.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            return result1;
        }
       [HttpPost]
        public ActionResult GenerateHwDamage( string yr/*string[] oem*/)
        {
            var obj = "";

            JsonResult result1 = GetHwDamage(yr/*oem*/);

            return Json(new { data = result1 }, JsonRequestBehavior.AllowGet);
        }

       
        public ActionResult Lookup_HwDamage()
        {
            var obj = "";
            DataSet dt = new DataSet();

            try
            {
                //create instanace of database connection
                // SqlConnection conn = new SqlConnection(connString);
                connection();
                dt = new DataSet();
                string Query = "";
                //conn.Open();
                OpenConnection();
                if(UserAutorizeToEdit == true)
                    Query = " Select * from [HwDamageCost_Location_Table]; Select * from [HwDamageCost_Month_Table];Select * from [ItemsCostList_Table] where BU IN (Select BU from HwDamageCost_UserIDs_Table where ADSID = '" + User.Identity.Name.Split('\\')[1].Trim() + "') OR BU IS NULL order by [Item Name]; Select * from [OEM_Table]; Select * from [Currency_Table];Select * from [BU_Table] where ID IN (3,5); ";
                else
                    Query = " Select * from [HwDamageCost_Location_Table]; Select * from [HwDamageCost_Month_Table];Select * from [ItemsCostList_Table] order by [Item Name]; Select * from [OEM_Table]; Select * from [Currency_Table];Select * from [BU_Table] where ID IN (3,5); ";

                SqlCommand cmd = new SqlCommand(Query, conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                //conn.Close();
                CloseConnection();
            }

            catch (Exception ex)
            {

            }
            finally
            {

            }
            //Loc lookup
            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            List<Dictionary<string, object>> parentRow = new List<Dictionary<string, object>>();
            Dictionary<string, object> childRow;
            //foreach (DataRow row in dt.Rows)
            foreach (DataRow row in dt.Tables[0].Rows)
            {
                    
                childRow = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Tables[0].Columns)
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

            JsonResult result_Loc = Json(result);
            result_Loc.MaxJsonLength = Int32.MaxValue;
            result_Loc.JsonRequestBehavior = JsonRequestBehavior.AllowGet;


            //Month lookup
            List<Dictionary<string, object>> parentRow1 = new List<Dictionary<string, object>>();
            Dictionary<string, object> childRow1;
            //foreach (DataRow row in dt.Rows)
            foreach (DataRow row in dt.Tables[1].Rows)
            {

                childRow1 = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Tables[1].Columns)
                {
                    childRow1.Add(col.ColumnName, row[col]);
                }
                parentRow1.Add(childRow1);
            }

            jsSerializer.MaxJsonLength = Int32.MaxValue;

            var resultData1 = parentRow1;
            var result1 = new ContentResult
            {
                Content = jsSerializer.Serialize(resultData1),
                ContentType = "application/json"
            };

            JsonResult result_Mon = Json(result1);
            result_Mon.MaxJsonLength = Int32.MaxValue;
            result_Mon.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            //Item master
            List<Dictionary<string, object>> parentRow2 = new List<Dictionary<string, object>>();
            Dictionary<string, object> childRow2;
            //foreach (DataRow row in dt.Rows)
            foreach (DataRow row in dt.Tables[2].Rows)
            {

                childRow2 = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Tables[2].Columns)
                {
                    childRow2.Add(col.ColumnName, row[col]);
                }
                parentRow2.Add(childRow2);
            }

            jsSerializer.MaxJsonLength = Int32.MaxValue;

            var resultData2 = parentRow2;
            var result2 = new ContentResult
            {
                Content = jsSerializer.Serialize(resultData2),
                ContentType = "application/json"
            };

            JsonResult result_Item = Json(result2);
            result_Item.MaxJsonLength = Int32.MaxValue;
            result_Item.JsonRequestBehavior = JsonRequestBehavior.AllowGet;


            //oem lookup
            List<Dictionary<string, object>> parentRow3 = new List<Dictionary<string, object>>();
            Dictionary<string, object> childRow3;
            //foreach (DataRow row in dt.Rows)
            foreach (DataRow row in dt.Tables[3].Rows)
            {

                childRow3 = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Tables[3].Columns)
                {
                    childRow3.Add(col.ColumnName, row[col]);
                }
                parentRow3.Add(childRow3);
            }

            jsSerializer.MaxJsonLength = Int32.MaxValue;

            var resultData3 = parentRow3;
            var result3 = new ContentResult
            {
                Content = jsSerializer.Serialize(resultData3),
                ContentType = "application/json"
            };

            JsonResult result_Project = Json(result3);
            result_Project.MaxJsonLength = Int32.MaxValue;
            result_Project.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            //curr lookup
            List<Dictionary<string, object>> parentRow4 = new List<Dictionary<string, object>>();
            Dictionary<string, object> childRow4;
            //foreach (DataRow row in dt.Rows)
            foreach (DataRow row in dt.Tables[4].Rows)
            {

                childRow4 = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Tables[4].Columns)
                {
                    childRow4.Add(col.ColumnName, row[col]);
                }
                parentRow4.Add(childRow4);
            }

            jsSerializer.MaxJsonLength = Int32.MaxValue;

            var resultData4 = parentRow4;
            var result4 = new ContentResult
            {
                Content = jsSerializer.Serialize(resultData4),
                ContentType = "application/json"
            };

            JsonResult result_Curr = Json(result4);
            result_Project.MaxJsonLength = Int32.MaxValue;
            result_Project.JsonRequestBehavior = JsonRequestBehavior.AllowGet;


            //bu lookup
            List<Dictionary<string, object>> parentRow5 = new List<Dictionary<string, object>>();
            Dictionary<string, object> childRow5;
            //foreach (DataRow row in dt.Rows)
            foreach (DataRow row in dt.Tables[5].Rows)
            {

                childRow5 = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Tables[5].Columns)
                {
                    childRow5.Add(col.ColumnName, row[col]);
                }
                parentRow5.Add(childRow5);
            }

            jsSerializer.MaxJsonLength = Int32.MaxValue;

            var resultData5 = parentRow5;
            var result5 = new ContentResult
            {
                Content = jsSerializer.Serialize(resultData5),
                ContentType = "application/json"
            };

            JsonResult result_bu = Json(result5);
            result_bu.MaxJsonLength = Int32.MaxValue;
            result_bu.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            return Json(new {data_BU = result_bu, data_Mon = result_Mon, data_Loc = result_Loc, data_Item = result_Item, data_Project = result_Project, data_Curr = result_Curr }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetDetails_basedonUser()
        {
            var User = System.Web.HttpContext.Current.User.Identity.Name.Split('\\')[1].ToUpper();
            string Query, Hw_status = "";
            int Location,Month,Project_Team, BU;
            int Qty, Year;
            connection();
            Query = "IF EXISTS(SELECT RequestorNT from HwDamageCost_Table where RequestorNT = @User)SELECT TOP 1 [BU], [Location],[Month] ,[Qty] ,[Project_Team] ,[Hw_status] ,[Year]  from HwDamageCost_Table where RequestorNT = @User order by Updated_At desc ELSE SELECT 0";
            OpenConnection();
            SqlCommand cmd = new SqlCommand(Query, conn);
            cmd.Parameters.AddWithValue("@User ", User);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                dr.Read();
                if (dr.FieldCount == 1)
                {
                    CloseConnection();
                    return Json(new { success = false }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    BU = int.Parse(dr["BU"].ToString());
                    Location = int.Parse(dr["Location"].ToString());
                    Month = int.Parse(dr["Month"].ToString());
                    Project_Team = int.Parse(dr["Project_Team"].ToString());
                    Hw_status = dr["Hw_status"].ToString();
                    Qty = int.Parse(dr["Qty"].ToString());
                    Year = int.Parse(dr["Year"].ToString());
                    CloseConnection();
                    return Json(new { success = true,BU = BU, Location = Location, Month = Month, Project_Team= Project_Team, Hw_status = Hw_status, Qty= Qty, Year= Year }, JsonRequestBehavior.AllowGet);

                }

                
              
            }
            else
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);

        }


        [HttpPost]
        public ActionResult GetCostEUR(string Hw_status, int ItemName)
        {
            connection();
            DataTable dt = new DataTable();
            string Query = "";
            if(Hw_status.Trim() == "Repairable")
            {
                Query = " select Repairable_Cost_EUR from ItemsCostList_Table where S#No = " + ItemName + " And Repairable_Cost_EUR is not null";
            }
            else if(Hw_status.Trim() == "Non-Repairable")
            {
                //decimal conversionEURate = /*(decimal)1.15;*/ db.Currency_Conversion_Table.FirstOrDefault(x => x.Currency.Contains("EUR")).ConversionRate_to_USD;
                //(double?)((decimal)item.Repairable_Cost * (1/conversionEURate));
                Query = "select Unitpriceusd / (select ConversionRate_to_USD from Currency_Conversion_Table where Currency = 'EUR') AS NonRepairCost_EUR from[ItemsCostList_Table] where S#No = " + ItemName + "  ";
                
            }
            else
            {
                return Json(new { msg = "Please enter Hw Damage Status!", JsonRequestBehavior.AllowGet });
            }
            
            OpenConnection();
            SqlCommand cmd = new SqlCommand(Query, conn);
            SqlDataReader dr = cmd.ExecuteReader();
            double Cost_EUR = 0.0 ; //
            if (dr.HasRows)
            {
                dr.Read();
                if (Hw_status.Trim() == "Repairable")
                {
                    Cost_EUR = Convert.ToDouble(dr["Repairable_Cost_EUR"].ToString());
                }
                else if (Hw_status.Trim() == "Non-Repairable")
                {
                    Cost_EUR = Convert.ToDouble(dr["NonRepairCost_EUR"].ToString());
                }
                
            }
            else
            {
                return Json(new { msg = "Please provide the required Cost details", JsonRequestBehavior.AllowGet });
               
            }
            CloseConnection();
            return Json(new { Cost_EUR = Cost_EUR, JsonRequestBehavior.AllowGet });
        }

        [HttpPost]
        public ActionResult Save_HwDamagedata(HwDamageView req, string yr /*string[] oem*/)
        {
            DataTable dt = new DataTable();
           
            connection();
            dt = new DataTable();
            string Query = "";
            System.Security.Principal.IPrincipal User = System.Web.HttpContext.Current.User;

            //SPOTONData_Table_2021 PresentUser = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper()));
            //           CREATE TABLE[dbo].[HwDamageCost_Table](
            //  [ID][int] IDENTITY(1, 1) NOT NULL,
            //  [Location] [varchar](255) NULL,
            //[Month] [nvarchar](10) NULL,
            //[Item_Name] [varchar](255) NULL,
            //[Qty] [varchar](255) NULL,
            //[Project_Team] [varchar](100) NULL,
            //[Cost_inEUR] [float] NULL,
            //[Total_Price] [float] NULL,
            //[RequestorNT] [nvarchar](50) NULL



            if (req.ID != 0)
            {
                //Query = "UPDATE [TravelCost_Table] SET [Cmmt_Item] = '" + req.Cmmt_Item + "', [Year] = '" + req.Year + "',[Budget_Plan] = '" + req.Budget_Plan + "',[Invoice] = '" + req.Invoice + "',[Open] = '" + req.Open + "',  WHERE ID = '" + req.ID +  "'";
                Query = "UPDATE [HwDamageCost_Table] SET [Year] = @Year ,[Location] = @Location ,BU =  @BU, Month= @Month, [Item_Name]= @Item_Name,Hw_status =@Hw_status, Updated_At=@Updated_At, Qty= @Qty, [Project_Team]= @Project_Team, Cost_inEUR= @Cost_inEUR, [Total_Price]= @Total_Price, [RequestorNT]= @RequestorNT , Remarks = @Remarks  WHERE ID = @ID";
            }
            else
            {
                //insert

                //Query = "INSERT INTO [TravelCost_Table] (" + "Cmmt_Item, Year, Budget_Plan, Invoice, Open" + ") VALUES ('" + req.Cmmt_Item + "','" +  req.Year + "','" +  req.Budget_Plan + "','" +  req.Invoice +"','" + req.Open + "')";
                Query = "INSERT INTO [HwDamageCost_Table] ( Year, Location, BU, Month, [Item_Name], Qty, [Project_Team],Hw_status, Cost_inEUR, Updated_At,[Total_Price],RequestorNT, Remarks) VALUES( @Year,  @Location ,@BU, @Month, @Item_Name, @Qty, @Project_Team, @Hw_status,@Cost_inEUR,@Updated_At,@Total_Price,@RequestorNT,@Remarks)";
            }
            SqlCommand cmd = new SqlCommand(Query, conn);
            cmd.Parameters.AddWithValue("@Location ", req.Location);
            cmd.Parameters.AddWithValue("@Month", req.Month);
            cmd.Parameters.AddWithValue("@BU", req.BU);
            cmd.Parameters.AddWithValue("@Year", req.Year);
            cmd.Parameters.AddWithValue("@Item_Name", req.Item_Name);
            cmd.Parameters.AddWithValue("@Qty", req.Qty);
            cmd.Parameters.AddWithValue("@Project_Team", req.Project_Team);
            cmd.Parameters.AddWithValue("@Hw_status", req.Hw_status);
            cmd.Parameters.AddWithValue("@Cost_inEUR", req.Cost_inEUR);
            cmd.Parameters.AddWithValue("@Total_Price", req.Total_Price);
            cmd.Parameters.AddWithValue("@RequestorNT", User.Identity.Name.Split('\\')[1].ToUpper());
            cmd.Parameters.AddWithValue("@Updated_At", DateTime.Now);

            cmd.Parameters.AddWithValue("@Remarks", req.Remarks);

            if (req.ID !=0)
                cmd.Parameters.AddWithValue("@ID", req.ID);


            try
            {
                OpenConnection();
                cmd.ExecuteNonQuery();
                Console.WriteLine("Records Inserted Successfully");
            }
            catch (Exception ex)
            {
                //MessageBox.Show(" Not Updated");
            }
            finally
            {
                CloseConnection();
            }

            return Json(new { data = GetHwDamage(yr), success = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(int id , string yr/*string[] oem*//*, string useryear*/)
        {

            
            DataTable dt = new DataTable();
            connection();
            dt = new DataTable();
            string Query = "";
            
                //Query = "UPDATE [TravelCost_Table] SET [Cmmt_Item] = '" + req.Cmmt_Item + "', [Year] = '" + req.Year + "',[Budget_Plan] = '" + req.Budget_Plan + "',[Invoice] = '" + req.Invoice + "',[Open] = '" + req.Open + "',  WHERE ID = '" + req.ID +  "'";
            Query="IF EXISTS(SELECT ID FROM [HwDamageCost_Table] WHERE ID = @id)DELETE FROM [HwDamageCost_Table] WHERE ID = @ID";
  
            SqlCommand cmd = new SqlCommand(Query, conn);
            
            cmd.Parameters.AddWithValue("@ID", id);


            try
            {
                OpenConnection();
                cmd.ExecuteNonQuery();
                Console.WriteLine("Records Deleted Successfully");
            }
            catch (Exception ex)
            {
                //MessageBox.Show(" Not Updated");
            }
            finally
            {
                CloseConnection();
            }
            return Json(new { data = GetHwDamage(yr), success = true }, JsonRequestBehavior.AllowGet);
        }



        //for HWDamage_Repairable interface 

        public ActionResult IndexHwDamageMasterlist()
        {



            return View();
            ;
        }

        public JsonResult HWDamage_Repair()
        {
            connection();
            DataTable dt = new DataTable();
            //string Query = "SELECT [ItemsCostList_Table].[Item Name],[ItemsCostList_Table].[S#No] as S_No, Repairable_Cost, Repair_Currency, Repairable_Cost_EUR, Repair_UpdatedBy, ItemsCostList_Table.[UnitPriceUSD] * (1/[Currency_Conversion_Table].[ConversionRate_to_USD]) AS NonRepairable_EUR FROM ItemsCostList_Table inner join [Currency_Conversion_Table] on [ItemsCostList_Table].[Currency] =  [Currency_Conversion_Table].ID  WHERE DELETED =" + 0 + " AND [Cost Element] IN ('1','2') and BU  IN (Select BU from HwDamageCost_UserIDs_Table where ADSID = '" + User.Identity.Name.Split('\\')[1].Trim() + "') AND VKM_Year = '" + DateTime.Now.Year + "'";//MAE, NMAE - MB Alone
            string Query = "SELECT [ItemsCostList_Table].[Item Name],[ItemsCostList_Table].[S#No] as S_No, Repairable_Cost, Repair_Currency, Repairable_Cost_EUR, Repair_UpdatedBy, ItemsCostList_Table.[UnitPriceUSD] * (1/[Currency_Conversion_Table].[ConversionRate_to_USD]) AS NonRepairable_EUR FROM ItemsCostList_Table inner join [Currency_Conversion_Table] on [ItemsCostList_Table].[Currency] =  [Currency_Conversion_Table].ID  WHERE [Cost Element] IN ('1','2') and BU  IN (Select BU from HwDamageCost_UserIDs_Table where ADSID = '" + User.Identity.Name.Split('\\')[1].Trim() + "') AND VKM_Year = '" + DateTime.Now.Year + "'";//MAE, NMAE - MB Alone

            //mb - mae, nmae ; oss - mae, nmae, 
            try
            {
                OpenConnection();
                SqlCommand cmd = new SqlCommand(Query, conn);
               
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
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
            return result1;

        }

        [HttpPost]
        public ActionResult HWDamage_Repairable()
        {
           
            return Json(new { data = HWDamage_Repair(), success = true }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Save_HwDamagedata_Repairable(HWRepairable req)
        {
            DataTable dt = new DataTable();
            connection();
            dt = new DataTable();
            string Query = "";
            
            //if (req.ID != 0)
            //{
                  Query = "UPDATE [ItemsCostList_Table] SET [Repairable_Cost] = @Repairable_Cost ,[Repair_Currency] = @Repair_Currency , [Repairable_Cost_EUR]= @Repairable_Cost_EUR,Repair_UpdatedAt =@Repair_UpdatedAt, [Repair_UpdatedBy]=@Repair_UpdatedBy   WHERE [S#No] = @ID";
            //}
           
            SqlCommand cmd = new SqlCommand(Query, conn);
            cmd.Parameters.AddWithValue("@Repairable_Cost ", req.Repairable_Cost);
            cmd.Parameters.AddWithValue("@Repair_Currency", req.Repair_Currency);
            cmd.Parameters.AddWithValue("@Repairable_Cost_EUR", req.Repairable_Cost_EUR);
            cmd.Parameters.AddWithValue("@Repair_UpdatedAt", DateTime.Now);
            cmd.Parameters.AddWithValue("@Repair_UpdatedBy", System.Web.HttpContext.Current.User.Identity.Name.Split('\\')[1].ToUpper());

                cmd.Parameters.AddWithValue("@ID", req.S_No);


            try
            {
                OpenConnection();
                cmd.ExecuteNonQuery();
                Console.WriteLine("Records Inserted Successfully");
            }
            catch (Exception ex)
            {
                //MessageBox.Show(" Not Updated");
            }
            finally
            {
                CloseConnection();
            }

            return Json(new { data = HWDamage_Repair(), success = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetEURINRates()
        {
            using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())
            {
                EURINR eurinr = new EURINR();
                eurinr.EUR = /*(decimal)1.15;*/ db.Currency_Conversion_Table.FirstOrDefault(x => x.Currency.Contains("EUR")).ConversionRate_to_USD;
                eurinr.INR = /*(decimal)0.014;*/db.Currency_Conversion_Table.FirstOrDefault(x => x.Currency.Contains("INR")).ConversionRate_to_USD;

                return Json(eurinr, JsonRequestBehavior.AllowGet);
            }

        }

    }





}



    


