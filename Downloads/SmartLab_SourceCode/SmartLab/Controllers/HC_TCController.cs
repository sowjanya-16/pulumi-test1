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
    public class HC_TCController : Controller
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

            

            return View();
;           
        }
        public ActionResult IndexTC()
        {



            return View();
            ;
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
            //var password = "serveruser@123"; //password

            string connString = ConfigurationManager.ConnectionStrings["BudgetingdbConnection"].ToString();
            conn = new SqlConnection(connString);

            //your connection string 
            //string connString = @"Data Source=" + datasource + ";Initial Catalog="
            //            + database + ";Persist Security Info=True;User ID=" + username + ";Password=" + password;

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

        public ActionResult GenerateHC()
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
                OpenConnection();
                string Query = "select SM.Role as RoleID,  SM.Year, SM.SkillSet, SM.[Plan], sm.Utilize," +
                    "( select ',' + CM.SkillSetName  from dbo.HC_SkillSet_Table as CM  where ',' + SM.SkillSet + ','" +
                    " like '%,' + convert(varchar, CM.ID) + ',%' " +
                    " for xml path(''), type ).value('substring(text()[1], 2)', 'varchar(max)') as SkillSetName, " +
                    "[HC_Role_Table].RoleName from dbo.HC_Table as SM inner join [dbo].[HC_Role_Table]" +
                    " on SM.Role = HC_Role_Table.ID where ISNULL(sm.role, '') <> ''";
                //" Select [HC_Table].*,[HC_SkillSet_Table].SkillSetName from [dbo].[HC_Table] inner join [dbo].[HC_SkillSet_Table] on HC_Table.SkillSet = HC_SkillSet_Table.ID ";
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
            //System.Data.DataTable dt1 = new System.Data.DataTable();
            //dt.Columns.Add("Role", typeof(string)); /// Planned + (year variable) / Utilized  + (year variable)
            //dt.Columns.Add("Skill", typeof(string));
            //string yrs = string.Empty;
            //for(int yr=2021; yr<=DateTime.Now.AddYears(1).Year; yr++)
            //{

            //    dt.Columns.Add(yr + " Plan", typeof(string)); //add vkm text to yr
            //    dt.Columns.Add(yr + " Utilize", typeof(string));

            //}


            //DataRow dr = dt.NewRow();
            ////dr[0] = "MAE";
            //int rcnt = -1;
            //int rcnt1 = 2; //2

            //foreach (var info in yrs)
            //{
            //    //dr[++rcnt] = info.P_MAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
            //    //dr[++rcnt1] = info.U_MAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
                
            //}
            return Json(new { data = result1 }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAddHC()
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
                OpenConnection();
                string Query = "select SM.Role as RoleID,SM.ID,  SM.Year, SM.SkillSet, SM.[Plan], sm.Utilize,( select ',' + CM.RoleName  from dbo.HC_Role_Table as CM  where ',' + SM.Role + ',' like '%,' + convert(varchar,CM.ID) + ',%'  for xml path(''), type ).value('substring(text()[1], 2)', 'varchar(max)') as Role from dbo.HC_Table as SM  where ISNULL(sm.role, '') <> ''";
                //" Select [HC_Table].*,[HC_SkillSet_Table].SkillSetName from [dbo].[HC_Table] inner join [dbo].[HC_SkillSet_Table] on HC_Table.SkillSet = HC_SkillSet_Table.ID ";
                //"select ID, Location, Month, QUOTENAME(Project_Team, '[]') as Project_Team,Qty,Item_Name,Cost_inEUR,Total_Price,RequestorNT,Hw_status,Updated_At from HwDamageCost_Table";
                // "select ID, Location, Month, Project_Team,Qty,Item_Name,Cost_inEUR,Total_Price,RequestorNT,Hw_status,Updated_At from HwDamageCost_Table";


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
        public ActionResult GenerateAddHC()
        {
          



            //return Json(new { data = jsSerializer.Serialize(parentRow) }, JsonRequestBehavior.AllowGet);

            //return Json(new { data = parentRow }, JsonRequestBehavior.AllowGet);
            //System.Data.DataTable dt1 = new System.Data.DataTable();
            //dt.Columns.Add("Role", typeof(string)); /// Planned + (year variable) / Utilized  + (year variable)
            //dt.Columns.Add("Skill", typeof(string));
            //string yrs = string.Empty;
            //for(int yr=2021; yr<=DateTime.Now.AddYears(1).Year; yr++)
            //{

            //    dt.Columns.Add(yr + " Plan", typeof(string)); //add vkm text to yr
            //    dt.Columns.Add(yr + " Utilize", typeof(string));

            //}


            //DataRow dr = dt.NewRow();
            ////dr[0] = "MAE";
            //int rcnt = -1;
            //int rcnt1 = 2; //2

            //foreach (var info in yrs)
            //{
            //    //dr[++rcnt] = info.P_MAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
            //    //dr[++rcnt1] = info.U_MAE_Totals.ToString("C0", CultureInfo.CurrentCulture);

            //}
            return Json(new { data = GetAddHC() }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult saveUtilizeCount(double newhc,/* double oldhc, */List<string>row , List<string> column)
        {
            DataTable dt = new DataTable();
            connection();
            dt = new DataTable();
            string skillids_list = "";
            //commented because Role is hidden in UI
            //OpenConnection(); //-not needed since multiple skillsets possible
            //string Query = " SELECT ID FROM [HC_Role_Table] WHERE RoleName = '" + row[0] + "'";
            //SqlCommand cmd = new SqlCommand(Query, conn);
            //SqlDataAdapter da = new SqlDataAdapter(cmd);
            //da.Fill(dt);

            //CloseConnection();
            //var roleid = dt.Rows[0].ItemArray[0];
            OpenConnection();


            ///ids from concatenated skill names
            ///

            var skill_list =  row[0].Split(',').ToList();
            string SkillNames_concat = "\'" + String.Join("\',\'", skill_list.ToArray()) + "\'";
            string Query = "SELECT string_agg(id, ',') as SkillIDs FROM[HC_SkillSet_Table] WHERE SkillSetName in (" + SkillNames_concat + ")";
            SqlCommand cmd = new SqlCommand(Query, conn);
            try
            {
                OpenConnection();
                skillids_list = cmd.ExecuteScalar().ToString();
                CloseConnection();
            }
            catch (Exception ex)
            {
                //MessageBox.Show(" Not Updated");
            }
            finally
            {
                CloseConnection();
            }






            Query = "UPDATE [HC_Table] SET Utilize = '" + newhc + "' WHERE SkillSet = '" + skillids_list /*+ "'AND Role = '"+ roleid*/ + "' AND Year ='"+ column[0]+ "'";

            cmd = new SqlCommand(Query, conn);
            try
            {
                OpenConnection();
                cmd.ExecuteNonQuery();
                //MessageBox.Show("Updated..");
            }
            catch (Exception ex)
            {
                //MessageBox.Show(" Not Updated");
            }
            finally
            {
                CloseConnection();
            }
            
            return Json(new { data = "Updated successfully" }, JsonRequestBehavior.AllowGet);
        }


        private JsonResult GetTC(string Year)
        {
            DataTable dt = new DataTable();

            try
            {
                //create instanace of database connection
                // SqlConnection conn = new SqlConnection(connString);
                connection();
                dt = new DataTable();
                //conn.Open();
                OpenConnection();
                string Query = "";
                if (Year == "")
                    Query = " Select * from [TravelCost_Table] ";
                else
                    Query = " Select * from [TravelCost_Table] where Year =" + Year + "and Cmmt_Item = 106";
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
        public ActionResult GenerateTC(string Year = "")
        {
            var obj = "";

            JsonResult result1 = GetTC(Year);


            //return Json(new { data = jsSerializer.Serialize(parentRow) }, JsonRequestBehavior.AllowGet);

            //return Json(new { data = parentRow }, JsonRequestBehavior.AllowGet);

            return Json(new { data = result1 }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Lookup_TC()
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
                OpenConnection();
                string Query = " Select * from [CmmtItem_Table] ";
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

        public ActionResult Lookup_HC()
        {
            var obj = "";
            DataSet dt = new DataSet();

            try
            {
                //create instanace of database connection
                // SqlConnection conn = new SqlConnection(connString);
                connection();
                dt = new DataSet();
                //conn.Open();
                OpenConnection();
                string Query = " Select * from [HC_SkillSet_Table]; Select RoleName from [HC_Role_Table] ";
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

            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            List<Dictionary<string, object>> parentRow = new List<Dictionary<string, object>>();
            Dictionary<string, object> childRow;
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

            JsonResult result_skill = Json(result);
            result_skill.MaxJsonLength = Int32.MaxValue;
            result_skill.JsonRequestBehavior = JsonRequestBehavior.AllowGet;


            //Role lookup
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

            JsonResult result_role = Json(result1);
            result_role.MaxJsonLength = Int32.MaxValue;
            result_role.JsonRequestBehavior = JsonRequestBehavior.AllowGet;


            return Json(new { data_skill = result_skill, data_role = result_role }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Save_TCdata(TCView req )
        {
            DataTable dt = new DataTable();
            connection();
            dt = new DataTable();
            string Query = "";
            //OpenConnection();
            // Query = " SELECT ID FROM[HC_SkillSet_Table] WHERE SkillSetName = '" + row[1] + "'";
            // cmd = new SqlCommand(Query, conn);
            //SqlDataAdapter da = new SqlDataAdapter(cmd);
            //da.Fill(dt);

            //CloseConnection();
            //var skillsetid = dt.Rows[0].ItemArray[0];
            

            if(req.ID != 0)
            {
                //Query = "UPDATE [TravelCost_Table] SET [Cmmt_Item] = '" + req.Cmmt_Item + "', [Year] = '" + req.Year + "',[Budget_Plan] = '" + req.Budget_Plan + "',[Invoice] = '" + req.Invoice + "',[Open] = '" + req.Open + "',  WHERE ID = '" + req.ID +  "'";
                Query = "UPDATE [TravelCost_Table] SET [Cmmt_Item] = @Cmmt_Item , Year= @Year, Budget_Plan= @Budget_Plan, Invoice= @Invoice,Bud_Inv = @Bud_Inv, Available= @Available, [Open]= @Open  WHERE ID = @ID";
            }
            else
            {
                //insert

                //Query = "INSERT INTO [TravelCost_Table] (" + "Cmmt_Item, Year, Budget_Plan, Invoice, Open" + ") VALUES ('" + req.Cmmt_Item + "','" +  req.Year + "','" +  req.Budget_Plan + "','" +  req.Invoice +"','" + req.Open + "')";
                Query = "INSERT INTO [TravelCost_Table] (Cmmt_Item, Year, Budget_Plan, Invoice, [Open], Bud_Inv, Available) VALUES(@Cmmt_Item, @Year, @Budget_Plan, @Invoice, @Open, @Bud_Inv, @Available )";
            }
            SqlCommand cmd = new SqlCommand(Query, conn);
            cmd.Parameters.AddWithValue("@Cmmt_Item", req.Cmmt_Item);
            cmd.Parameters.AddWithValue("@Year", req.Year);
            cmd.Parameters.AddWithValue("@Budget_Plan", req.Budget_Plan);
            cmd.Parameters.AddWithValue("@Invoice", req.Invoice);
            cmd.Parameters.AddWithValue("@Open", req.Open);
            cmd.Parameters.AddWithValue("@Bud_Inv", req.Bud_Inv);
            cmd.Parameters.AddWithValue("@Available", req.Available);
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

            return Json(new { data = GetTC(""), success = true }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Save_HCdata(HCView req)
        {

            DataTable dt = new DataTable();
            dt = new DataTable();
            connection();
            string Query = "";
            int isRole_exist;
            string ids_list = "";
            SqlCommand cmd;
            OpenConnection();
            //inserting new roles if provided
            foreach (var i in req.Role)
            {
                //IF EXISTS(SELECT ID FROM[OEM_Table] WHERE OEM = 'audi')SELECT 1 ELSE SELECT 0
                Query = "IF EXISTS(SELECT ID FROM [HC_Role_Table] WHERE RoleName =  @RoleName_i)SELECT 1 ELSE SELECT 0";
                cmd = new SqlCommand(Query, conn);
                cmd.Parameters.AddWithValue("@RoleName_i ", i);
                try
                {
                    OpenConnection();
                    isRole_exist = int.Parse(cmd.ExecuteScalar().ToString());
                    CloseConnection();

                    if (isRole_exist == 0)
                    {
                        //insert i - req.role
                        Query = "INSERT INTO [HC_Role_Table] (RoleName) VALUES(@RoleName)";
                        cmd = new SqlCommand(Query, conn);
                        cmd.Parameters.AddWithValue("@RoleName ", i);
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
                    }
                }
                catch (Exception ex)
                {
                    //handle exception
                }
               
            }

            //inserting entries in hc_tbl if req.ID == 0
            //else updte entry
            //while updating append the ids of the req.roles with , 
            string RoleNames_list = "\'" + String.Join("\',\'", req.Role.ToArray()) + "\'";
            Query = "SELECT string_agg(id, ',') as RoleIDs FROM[HC_Role_Table] WHERE RoleName in (" + RoleNames_list +")";
                //SELECT ID FROM [HC_Role_Table] WHERE RoleName in (" + RoleNames_list +")";
            cmd = new SqlCommand(Query, conn);
            //cmd.Parameters.AddWithValue("@RoleNames_list ", "\'"  +  String.Join("\',\'", req.Role.ToArray()).Replace("\r\n","") + "\'");
            try
            {
                OpenConnection();
                //SqlDataAdapter da = new SqlDataAdapter(cmd);
                //da.Fill(dt);
                //var y = String.Join(",", dt.AsEnumerable().Select(x => x.ItemArray.ToString()));
                ids_list = cmd.ExecuteScalar().ToString();
                CloseConnection();


            }
            catch (Exception ex)
            {
                //MessageBox.Show(" Not Updated");
            }
            finally
            {
                CloseConnection();
            }
            if (req.ID != 0)
            {
                Query = "UPDATE [HC_Table] SET [SkillSet] = @SkillSet , Role= @Role, [Year]= @Year,[Plan] =@Plan, Utilize=@Utilize WHERE ID = @ID";
            }
            else
            {
                //insert
                 Query = "INSERT INTO [HC_Table] (SkillSet, Role, Year, [Plan],  Utilize) VALUES(@SkillSet , @Role, @Year, @Plan, @Utilize)";
            }
            

            //SELECT STRING_AGG(CONVERT(NVARCHAR(max), chkjoin ), ',') AS csv FROM Person.Person;
            cmd = new SqlCommand(Query, conn);
            cmd.Parameters.AddWithValue("@SkillSet ", req.SkillSet);
            cmd.Parameters.AddWithValue("@Role", ids_list/*String.Join(",", req.Role.ToArray())*/);
            cmd.Parameters.AddWithValue("@Year", req.Year);
            cmd.Parameters.AddWithValue("@Plan", req.Plan);
            cmd.Parameters.AddWithValue("@Utilize", req.Utilize);


            if (req.ID != 0)
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


            //string Query = " SELECT ID FROM[HC_SkillSet_Table] WHERE SkillSetName = '" + row[1] + "'";
            //SqlCommand cmd = new SqlCommand(Query, conn);
            //SqlDataAdapter da = new SqlDataAdapter(cmd);
            //da.Fill(dt);

            //CloseConnection();
            //var skillsetid = dt.Rows[0].ItemArray[0];
            //OpenConnection();

           

            return Json(new { data = GetAddHC(), success = true, msg = "Updated successfully" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Delete(int id/*, string useryear*/)
        {


            DataTable dt = new DataTable();
            connection();
            dt = new DataTable();
            string Query = "";

            //Query = "UPDATE [TravelCost_Table] SET [Cmmt_Item] = '" + req.Cmmt_Item + "', [Year] = '" + req.Year + "',[Budget_Plan] = '" + req.Budget_Plan + "',[Invoice] = '" + req.Invoice + "',[Open] = '" + req.Open + "',  WHERE ID = '" + req.ID +  "'";
            Query = "IF EXISTS(SELECT ID FROM [HC_Table] WHERE ID = @id)DELETE FROM [HC_Table] WHERE ID = @ID";

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
            return Json(new { data = GetAddHC(), success = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetBalanceBudget(string BudgPlan, string Inv)
        {

            decimal Bal_Budget = 0;
            if (BudgPlan != null && BudgPlan.Trim() != "")
                Bal_Budget = decimal.Parse(BudgPlan);
            if (Inv != null && Inv.Trim() != "")
                Bal_Budget = decimal.Parse(BudgPlan) - decimal.Parse(Inv);
            

            return Json(new { Bal_Budget = Bal_Budget,  JsonRequestBehavior.AllowGet });

        }

        [HttpPost]
        public ActionResult GetAvailableBudget(string BudgPlan, string Inv, string Open)
        {

            decimal Bal_Budget = 0;
            if (BudgPlan != null && BudgPlan.Trim() != "")
                Bal_Budget = decimal.Parse(BudgPlan);
            if (Inv != null && Inv.Trim() != "" && Open != null && Open.Trim() != "")
                Bal_Budget = decimal.Parse(BudgPlan) - decimal.Parse(Inv) - decimal.Parse(Open);
            else if (Inv != null && Inv.Trim() != "" )
                Bal_Budget = decimal.Parse(BudgPlan) - decimal.Parse(Inv);
            else if (Open != null && Open.Trim() != "")
                Bal_Budget = decimal.Parse(BudgPlan) - decimal.Parse(Open);


            return Json(new { Bal_Budget = Bal_Budget, JsonRequestBehavior.AllowGet });

        }



        //public string [,] GetLabActivity(string labID, DateTime StartDate, DateTime EndDate, string LabType, string LabLocation)
        //{
        //    DateTime dtS = StartDate;
        //    DateTime dtE = EndDate;

        //    DateTime sDate = dtS;
        //    DateTime eDate = dtE;


        //    using (BookingServerReplicaEntities db = new BookingServerReplicaEntities())
        //    {
        //        List<Models.LabInfo> wrapperLabs = db.LabInfoes.ToList();
        //        List<Models.LabType> objLabType = db.LabTypes.ToList();
        //        List<Models.Location> lstLabLocs = db.Locations.ToList();
        //        List<Models.Site> objSites = db.Sites.ToList();

        //        string[,] TotalHours ;

        //        TotalHours = new string[1,3];


        //        try
        //        {
        //                    List<LabComputersPr> labComputers = new List<LabComputersPr>();
        //                    labComputers = db.LabComputersPrs.Where(x => x.LabId.ToString() == labID.ToString()).ToList();
        //                    if (labComputers != null)
        //                    {
        //                        //foreach (var labs in labComputers)
        //                        //{
        //                        string labid = labID;
        //                        int countManual = 0, countAutomated = 0;
        //                        int pccnt = 0, daycount = 0;
        //                        double ManualTotalHours = 0;
        //                        double AutomatedTotalHours = 0;
        //                        double ManualCaplTotalHours = 0;
        //                        double AutomatedCaplTotalHours = 0;
        //                        double TotalLabHours = 0;
        //                        sDate = dtS;
        //                        eDate = dtE;
        //                        List<DailySummaryTable> dailysummary = new List<DailySummaryTable>();
        //                        while (sDate <= eDate)
        //                        {
        //                            foreach (var computer in labComputers)
        //                            {

        //                                int compid = computer.Id;
        //                                string sDt = sDate.ToShortDateString();
        //                                dailysummary = db.DailySummaryTables.Where(x => x.ComputerID.ToString() == compid.ToString() && x.Date == sDt).ToList();
        //                                foreach (var summary in dailysummary)
        //                                {
        //                                    ManualTotalHours += Convert.ToDouble(summary.ManualTotalHours);
        //                                    AutomatedTotalHours += Convert.ToDouble(summary.AutomatedTotalHours);
        //                                }
        //                                pccnt++;
        //                            }
        //                            daycount++;

        //                    ManualTotalHours = Math.Round(ManualTotalHours, 2);
        //                    AutomatedTotalHours = Math.Round(AutomatedTotalHours, 2);
        //                    TotalLabHours = Math.Round(ManualTotalHours, 2) + Math.Round(AutomatedTotalHours, 2);

        //                    string ManualHours = ManualTotalHours.ToString(), AutomatedHours = AutomatedTotalHours.ToString(), LabHours = TotalLabHours.ToString();

        //                            TotalHours[0, 0] = ManualHours;
        //                            TotalHours[0, 1] = AutomatedHours;
        //                            TotalHours[0, 2] = LabHours;

        //                            //objLabExport.Labs[lab_index].Model = labid;
        //                            //objLabExport.Labs[lab_index].ManualTotalHours = ManualTotalHours;
        //                            //objLabExport.Labs[lab_index].AutomatedTotalHours = AutomatedTotalHours;
        //                            //objLabExport.Labs[lab_index].ManualCaplTotalHours = ManualCaplTotalHours;
        //                            //objLabExport.Labs[lab_index].AutomatedCaplTotalHours = AutomatedCaplTotalHours;
        //                            sDate = sDate.AddDays(1);
        //                        }
        //                        //labcount++;
        //                    }
        //                //}
        //            //}
        //        }
        //        catch (Exception ex)
        //        {
        //            HomeController.logger.Error(ex, "GetLabActivity");
        //            //return;
        //        }
        //        finally
        //        {
        //            //try
        //            //{
        //            //    LabBookingWrapper.APIDispose();
        //            //}
        //            //catch (Exception ex)
        //            //{
        //            //    HomeController.logger.Error(ex, "APIDispose");
        //            //}
        //        }
        //        return (string[,])TotalHours.Clone();
        //    }
        //}




        //public void getInitData()
        //{
        //    List<WrapperSite> loc_wrapperSites = new List<WrapperSite>();
        //    List<WrapperLocation> loc_wrapperLocations = new List<WrapperLocation>();
        //    List<WrapperLab> loc_wrapperLabs = new List<WrapperLab>();
        //    List<WrapperComputer> loc_wrapperComps = new List<WrapperComputer>();
        //    List<WrapperLabType> loc_wrapperLabType = new List<WrapperLabType>();
        //    List<WrapperUser> loc_wrapperUser = new List<WrapperUser>();

        //    try
        //    {
        //        //objLabExport = new LabBookingExport();
        //        string apistatus = LabBookingWrapper.APIInit("tracker");
        //        if (apistatus.Contains("SUCCESS"))
        //        {

        //            WrapperSites wrapperSites = LabBookingWrapper.GetSites();

        //            if (wrapperSites != null)
        //            {
        //                if (wrapperSites.successMsg.Contains("SUCCESS"))
        //                {
        //                    foreach (WrapperSite site in wrapperSites.SitesList)
        //                    {
        //                        loc_wrapperSites.Add(site);
        //                        WrapperLocations wrapperLocations = LabBookingWrapper.GetLocationsBySite(site.Id);
        //                        if (wrapperLocations.successMsg.Contains("SUCCESS"))
        //                        {
        //                            foreach (WrapperLocation location in wrapperLocations.LocationsList)
        //                            {
        //                                loc_wrapperLocations.Add(location);
        //                                var labOwner = LabBookingWrapper.GetUser(location.AdminUserGroupId);
        //                                if (labOwner.successMsg.Contains("SUCCESS"))
        //                                {
        //                                    loc_wrapperUser.Add(labOwner.UserObj);
        //                                }

        //                                WrapperLabs wrapperLabs = LabBookingWrapper.GetLabsByLocation(location.Id);

        //                                if (wrapperLabs.successMsg.Contains("SUCCESS"))
        //                                {
        //                                    foreach (WrapperLab lab in wrapperLabs.LabsList)
        //                                    {
        //                                        loc_wrapperLabs.Add(lab);

        //                                        var labtypeQ = LabBookingWrapper.GetlabType(lab.TypeId);

        //                                        if (labtypeQ.successMsg.Contains("SUCCESS"))
        //                                        {
        //                                            foreach (WrapperLabType labtype in labtypeQ.labType)
        //                                                loc_wrapperLabType.Add(labtype);
        //                                        }


        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //            //var labtypeQ = LabBookingWrapper.GetLabTypes();

        //            //if (labtypeQ.successMsg.Contains("SUCCESS"))
        //            //{
        //            //    foreach (WrapperLabType labtype in labtypeQ.labType)
        //            //        loc_wrapperLabType.Add(labtype);
        //            //}
        //        }
        //    }
        //    catch (Exception ex) { return; }
        //    finally { LabBookingWrapper.APIDispose(); }

        //    var json = JsonConvert.SerializeObject(loc_wrapperSites);
        //    System.IO.File.WriteAllText(LabSitesPath, JsonConvert.SerializeObject(json));

        //    json = JsonConvert.SerializeObject(loc_wrapperLocations);
        //    System.IO.File.WriteAllText(LabLocationsPath, JsonConvert.SerializeObject(json));

        //    json = JsonConvert.SerializeObject(loc_wrapperLabType);
        //    System.IO.File.WriteAllText(LabTypesPath, JsonConvert.SerializeObject(json));

        //    json = JsonConvert.SerializeObject(loc_wrapperLabs);
        //    System.IO.File.WriteAllText(LabInfoPath, JsonConvert.SerializeObject(json));
        //    //using (StreamWriter file = System.IO.File.CreateText(LabInfoPath))
        //    //{
        //    //    JsonSerializer serializer = new JsonSerializer();
        //    //    //serialize object directly into file stream
        //    //    serializer.Serialize(file, json);
        //    //}
        //}

        //public void loadJsonObjects()
        //{
        //    try
        //    {
        //        string jsonLabType = System.IO.File.ReadAllText(LabTypesPath);

        //        jsonLabType = jsonLabType.Replace("\\", string.Empty);
        //        jsonLabType = jsonLabType.Trim('"');

        //        objLabType = JsonConvert.DeserializeObject<List<WrapperLabType>>(jsonLabType);

        //        string jsonSites = System.IO.File.ReadAllText(LabSitesPath);
        //        jsonSites = jsonSites.Replace("\\", string.Empty);
        //        jsonSites = jsonSites.Trim('"');
        //        objSites = JsonConvert.DeserializeObject<List<WrapperSite>>(jsonSites);

        //        string jsonlabs = System.IO.File.ReadAllText(LabInfoPath);
        //        jsonlabs = jsonlabs.Replace("\\", string.Empty);
        //        jsonlabs = jsonlabs.Trim('"');
        //        lstLabDef = JsonConvert.DeserializeObject<List<WrapperLab>>(jsonlabs);

        //        string jsonLocs = System.IO.File.ReadAllText(LabLocationsPath);
        //        jsonLocs = jsonLocs.Replace("\\", string.Empty);
        //        jsonLocs = jsonLocs.Trim('"');
        //        lstLabLocs = JsonConvert.DeserializeObject<List<WrapperLocation>>(jsonLocs);

        //        //convertcsvtojson(LabOEMCsvPath, LabOEMJsonPath);

        //        //string jsonLabOEM = System.IO.File.ReadAllText(LabOEMJsonPath);
        //        //jsonLabOEM = jsonLabOEM.Replace("\\", string.Empty);
        //        //jsonLabOEM = jsonLabOEM.Trim('"');
        //        //laboemmapping = JsonConvert.DeserializeObject<List<laboemjson>>(jsonLabOEM);
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

        //public void initDataObject()
        //{
        //    #region Fill main Export Object

        //    using (BookingServerReplicaEntities db = new BookingServerReplicaEntities())
        //    {
        //        List<Models.Site> objSites = db.Sites.ToList();
        //        List<Models.Location> lstLabLocs = db.Locations.ToList();
        //        List<Models.LabType> objLabType = db.LabTypes.ToList();
        //        List<LabInfo> lstLabDef = db.LabInfoes.ToList();

        //        if (objLabExport.Labs == null)
        //        {
        //            objLabExport.Labs = new LCfilterInfo.LabBookingExportLab[lstLabDef.Count];
        //            int count = 0;
        //            foreach (Models.LabInfo lab in lstLabDef)
        //            {
        //                objLabExport.Labs[count] = new LCfilterInfo.LabBookingExportLab();
        //                objLabExport.Labs[count].Description = lab.Description;
        //                objLabExport.Labs[count].id = (ushort)lab.Id;
        //                objLabExport.Labs[count].Inventory = lab.DisplayName;
        //                var temp1 = objSites.FirstOrDefault(site => site.Id == lstLabLocs.First(s => s.Id == lab.LocationId).SiteId);
        //                if (temp1 != null)
        //                    objLabExport.Labs[count].Location = temp1.RbCode;
        //                else
        //                    objLabExport.Labs[count].Location = "NA";
        //                var temp = objLabType.FirstOrDefault(s => s.Id == lab.TypeId);
        //                if (temp != null)
        //                    objLabExport.Labs[count].Model = temp.DisplayName;
        //                else
        //                    objLabExport.Labs[count].Model = "NA";
        //                objLabExport.Labs[count].Owner = lab.DisplayName;
        //                objLabExport.Labs[count].name = lab.DisplayName;

        //                objLabExport.Labs[count].SubLocation = lab.Description;


        //                count++;
        //            }
        //            objLabExport.Labs = objLabExport.Labs.OrderBy(item => item.id).ToArray();


        //        }
        //        #endregion
        //    }
        //}

        //public void getInitData()
        //{
        //    using (BookingServerReplicaEntities db = new BookingServerReplicaEntities())
        //    {
        //        List<Models.Site> loc_wrapperSites = new List<Site>();
        //        List<Models.Location> loc_wrapperLocations = new List<Location>();
        //        List<Models.LabInfo> loc_wrapperLabs = new List<LabInfo>();
        //        List<Models.LabComputersPr> loc_wrapperComps = new List<LabComputersPr>();
        //        List<Models.LabType> loc_wrapperLabType = new List<LabType>();


        //        List<Models.Site> wrapperSites = db.Sites.ToList();
        //        List<Models.Location> wrapperLocations = db.Locations.ToList();
        //        List<Models.LabInfo> wrapperLabs = db.LabInfoes.ToList();
        //        List<Models.LabComputersPr> wrapperComputers = db.LabComputersPrs.ToList();
        //        List<Models.LabType> wrapperLabType = db.LabTypes.ToList();

        //        try
        //        {
        //            //objLabExport = new LabBookingExport();
        //            if (wrapperSites != null)
        //            {
        //                foreach (Models.Site site in wrapperSites)
        //                {
        //                    loc_wrapperSites.Add(site);
        //                    var LocationsList = from p in db.Locations where p.SiteId == site.Id select p;
        //                    IEnumerable<Location> location = LocationsList.ToList();
        //                    foreach (Models.Location loc in location)
        //                    {
        //                        loc_wrapperLocations.Add(loc);

        //                        var LabsList = from p in db.LabInfoes where p.LocationId == loc.Id select p;
        //                        IEnumerable<LabInfo> labinfo = LabsList.ToList();
        //                        foreach (Models.LabInfo lab in labinfo)
        //                        {
        //                            loc_wrapperLabs.Add(lab);
        //                            var labtypeQ = from p in db.LabTypes where p.Id == lab.TypeId select p;
        //                            IEnumerable<LabType> labType = labtypeQ.ToList();
        //                            foreach (Models.LabType labtype in labType)
        //                            {
        //                                loc_wrapperLabType.Add(labtype);
        //                            }
        //                        }
        //                    }

        //                }
        //            }

        //        }
        //        catch (Exception ex)
        //        {
        //            return;
        //        }
        //        finally
        //        {
        //            //LabBookingWrapper.APIDispose(); 
        //        }

        //        var json = JsonConvert.SerializeObject(loc_wrapperSites);
        //        System.IO.File.WriteAllText(LabSitesPath, JsonConvert.SerializeObject(json));

        //        json = JsonConvert.SerializeObject(loc_wrapperLocations);
        //        System.IO.File.WriteAllText(LabLocationsPath, JsonConvert.SerializeObject(json));

        //        json = JsonConvert.SerializeObject(loc_wrapperLabType);
        //        System.IO.File.WriteAllText(LabTypesPath, JsonConvert.SerializeObject(json));

        //        json = JsonConvert.SerializeObject(loc_wrapperLabs);
        //        System.IO.File.WriteAllText(LabInfoPath, JsonConvert.SerializeObject(json));
        //    }


        //    //using (StreamWriter file = System.IO.File.CreateText(LabInfoPath))
        //    //{
        //    //    JsonSerializer serializer = new JsonSerializer();
        //    //    //serialize object directly into file stream
        //    //    serializer.Serialize(file, json);
        //    //}
        //}



        //public void initDataObject()
        //{
        //    #region Fill main Export Object
        //    if (objLabExport.Labs == null)
        //    {
        //        objLabExport.Labs = new LCfilterInfo.LabBookingExportLab[lstLabDef.Count];
        //        int count = 0;
        //        foreach (WrapperLab lab in lstLabDef)
        //        {
        //            objLabExport.Labs[count] = new LCfilterInfo.LabBookingExportLab();
        //            objLabExport.Labs[count].Description = lab.Description;
        //            objLabExport.Labs[count].id = (ushort)lab.Id;
        //            objLabExport.Labs[count].Inventory = lab.DisplayName;
        //            var temp1 = objSites.FirstOrDefault(site => site.Id == lstLabLocs.First(s => s.Id == lab.LocationId).SiteId);
        //            if (temp1 != null)
        //                objLabExport.Labs[count].Location = temp1.RbCode;
        //            else
        //                objLabExport.Labs[count].Location = "NA";
        //            var temp = objLabType.FirstOrDefault(s => s.Id == lab.TypeId);
        //            if (temp != null)
        //                objLabExport.Labs[count].Model = temp.DisplayName;
        //            else
        //                objLabExport.Labs[count].Model = "NA";
        //            objLabExport.Labs[count].Owner = lab.DisplayName;
        //            objLabExport.Labs[count].name = lab.DisplayName;

        //            objLabExport.Labs[count].SubLocation = lab.Description;


        //            count++;
        //        }
        //        objLabExport.Labs = objLabExport.Labs.OrderBy(item => item.id).ToArray();


        //    }
        //    #endregion
        //}


        //#region API FUNCTION 
        /// <summary>
        /// Function to get the lab activity from the server
        /// </summary>
        /// <param name="labID"></param>
        /// <param name="StartDate"></param>
        /// <param name="EndDate"></param>
        /// <param name="LabType"></param>
        /// <param name="LabLocation"></param>
        //public void GetLabActivity(DateTime StartDate, DateTime EndDate, string LabType, string LabLocation)
        //{
        //    try
        //    {
        //        //objLabExport = new LabBookingExport();
        //        string apistatus = LabBookingWrapper.APIInit("tracker");
        //        if (apistatus.Contains("SUCCESS"))
        //        {
        //            foreach (WrapperLab lab in lstLabDef)
        //            {
        //                int lab_index = Array.FindIndex(objLabExport.Labs, row => row.id == lab.Id);
        //                if (objLabType.First(s => s.Id == lab.TypeId).DisplayName.Equals(LabType) && objSites.First(site => site.Id == lstLabLocs.First(s => s.Id == lab.LocationId).SiteId).RbCode.Equals(LabLocation))
        //                {
        //                    WrapperComputers wrapperComputers = LabBookingWrapper.GetComputersByLab(lab.Id);
        //                    if (wrapperComputers.successMsg.Contains("SUCCESS"))
        //                    {
        //                        objLabExport.Labs[lab_index].PCs = new LCfilterInfo.LabBookingExportLabPC[wrapperComputers.ComputersList.Count];
        //                        int countManual = 0, countAutomated = 0;
        //                        int pccnt = 0;
        //                        List<WrapperActivity> loc_activities = new List<WrapperActivity>();
        //                        foreach (WrapperComputer computer in wrapperComputers.ComputersList)
        //                        {
        //                            objLabExport.Labs[lab_index].PCs[pccnt] = new LCfilterInfo.LabBookingExportLabPC();
        //                            objLabExport.Labs[lab_index].PCs[pccnt].Created = computer.Created;
        //                            objLabExport.Labs[lab_index].PCs[pccnt].CreatedAt = computer.CreatedAt;
        //                            objLabExport.Labs[lab_index].PCs[pccnt].Description = computer.Description;
        //                            objLabExport.Labs[lab_index].PCs[pccnt].DisplayName = computer.DisplayName;
        //                            objLabExport.Labs[lab_index].PCs[pccnt].FQDN = computer.FQDN;
        //                            objLabExport.Labs[lab_index].PCs[pccnt].Id = computer.Id;
        //                            objLabExport.Labs[lab_index].PCs[pccnt].LabId = computer.LabId;
        //                            objLabExport.Labs[lab_index].PCs[pccnt].LocationId = computer.LocationId;
        //                            objLabExport.Labs[lab_index].PCs[pccnt].TrackerConfigId = computer.TrackerConfigId;
        //                            objLabExport.Labs[lab_index].PCs[pccnt].Updated = computer.Updated;
        //                            objLabExport.Labs[lab_index].PCs[pccnt].UpdatedAt = computer.UpdatedAt;
        //                            WrapperActivities wrapperActivities = new WrapperActivities();

        //                            wrapperActivities = LabBookingWrapper.GetActivitiesByComputer(computer.Id, StartDate.AddDays(-7), EndDate.AddDays(7));
        //                            ///to retry in case of timeout
        //                            ServerTimeout = false;
        //                            System.Timers.Timer timeout_check = new System.Timers.Timer();
        //                            timeout_check.Interval = 5000;
        //                            timeout_check.Elapsed += Timeout_check_Elapsed;
        //                            timeout_check.Start();
        //                            while (!wrapperActivities.successMsg.Contains("SUCCESS") && !ServerTimeout)
        //                            {
        //                                wrapperActivities = LabBookingWrapper.GetActivitiesByComputer(computer.Id, StartDate.AddDays(-7), EndDate.AddDays(7));
        //                                Thread.Sleep(1000);
        //                            }
        //                            if (wrapperActivities.successMsg.Contains("SUCCESS"))
        //                            {
        //                                countManual += wrapperActivities.ActivitiesList.FindAll(item => item.Type == WrapperActivityType.Manual).Count + wrapperActivities.ActivitiesList.FindAll(item => item.Type == WrapperActivityType.ManualTest).Count;
        //                                countAutomated += wrapperActivities.ActivitiesList.FindAll(item => item.Type == WrapperActivityType.AutomatedTest).Count;
        //                                foreach (WrapperActivity wrapperActivity in wrapperActivities.ActivitiesList)
        //                                {
        //                                    loc_activities.Add(wrapperActivity);
        //                                }
        //                            }
        //                            pccnt++;
        //                        }
        //                        objLabExport.Labs[lab_index].ManualSessions = new LCfilterInfo.LabBookingExportManualSessSpan[countManual];
        //                        objLabExport.Labs[lab_index].AutomatedSessions = new LCfilterInfo.LabBookingExportAutoSessSpan[countAutomated];
        //                        int mancnt = 0, autocnt = 0;
        //                        foreach (WrapperActivity wrapperActivity in loc_activities)
        //                        {
        //                            //if(wrapperActivity.Info.ToLower().Contains("canw") || wrapperActivity.Info.ToLower().Contains("canoe"))
        //                            //{
        //                            //    objLabExport.Labs[lab_index].AutomatedSessions[autocnt] = new LCfilterInfo.LabBookingExportAutoSessSpan();
        //                            //    objLabExport.Labs[lab_index].AutomatedSessions[autocnt].isActive = wrapperActivity.IsActive;
        //                            //    if (wrapperActivity.IsActive)
        //                            //        objLabExport.Labs[lab_index].AutomatedSessions[autocnt].end = DateTime.Now;
        //                            //    else
        //                            //        objLabExport.Labs[lab_index].AutomatedSessions[autocnt].end = wrapperActivity.EndDate;
        //                            //    objLabExport.Labs[lab_index].AutomatedSessions[autocnt].start = wrapperActivity.StartDate;
        //                            //    objLabExport.Labs[lab_index].AutomatedSessions[autocnt].trigger = wrapperActivity.Type.ToString();
        //                            //    objLabExport.Labs[lab_index].AutomatedSessions[autocnt].Value = wrapperActivity.ComputerId.ToString() + "_" + wrapperActivity.Info + "_" + wrapperActivity.SessionId;

        //                            //    autocnt++;
        //                            //    continue;
        //                            //}

        //                            if (wrapperActivity.Type == WrapperActivityType.Manual || wrapperActivity.Type == WrapperActivityType.ManualTest)
        //                            {
        //                                objLabExport.Labs[lab_index].ManualSessions[mancnt] = new LCfilterInfo.LabBookingExportManualSessSpan();
        //                                objLabExport.Labs[lab_index].ManualSessions[mancnt].isActive = wrapperActivity.IsActive;
        //                                if (wrapperActivity.IsActive)
        //                                    objLabExport.Labs[lab_index].ManualSessions[mancnt].end = DateTime.Now;
        //                                else
        //                                    objLabExport.Labs[lab_index].ManualSessions[mancnt].end = wrapperActivity.EndDate;
        //                                objLabExport.Labs[lab_index].ManualSessions[mancnt].start = wrapperActivity.StartDate;
        //                                objLabExport.Labs[lab_index].ManualSessions[mancnt].trigger = wrapperActivity.Type.ToString();
        //                                objLabExport.Labs[lab_index].ManualSessions[mancnt].Value = wrapperActivity.ComputerId.ToString() + "_" + wrapperActivity.Info + "_" + wrapperActivity.SessionId;
        //                                mancnt++;
        //                            }
        //                            else if (wrapperActivity.Type == WrapperActivityType.AutomatedTest)
        //                            {
        //                                objLabExport.Labs[lab_index].AutomatedSessions[autocnt] = new LCfilterInfo.LabBookingExportAutoSessSpan();
        //                                objLabExport.Labs[lab_index].AutomatedSessions[autocnt].isActive = wrapperActivity.IsActive;
        //                                if (wrapperActivity.IsActive)
        //                                    objLabExport.Labs[lab_index].AutomatedSessions[autocnt].end = DateTime.Now;
        //                                else
        //                                    objLabExport.Labs[lab_index].AutomatedSessions[autocnt].end = wrapperActivity.EndDate;
        //                                objLabExport.Labs[lab_index].AutomatedSessions[autocnt].start = wrapperActivity.StartDate;
        //                                objLabExport.Labs[lab_index].AutomatedSessions[autocnt].trigger = wrapperActivity.Type.ToString();
        //                                objLabExport.Labs[lab_index].AutomatedSessions[autocnt].Value = wrapperActivity.ComputerId.ToString() + "_" + wrapperActivity.Info + "_" + wrapperActivity.SessionId;

        //                                autocnt++;
        //                            }
        //                        }
        //                    }
        //                }

        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        HomeController.logger.Error(ex, "GetLabActivity");
        //        return;
        //    }
        //    finally
        //    {
        //        try
        //        {
        //            LabBookingWrapper.APIDispose();
        //        }
        //        catch (Exception ex)
        //        {
        //            HomeController.logger.Error(ex, "APIDispose");
        //        }
        //    }



        //    //objLabExport.Labs[count].AutomatedSessions;
        //    //objLabExport.Labs[count].Defectives;
        //    //objLabExport.Labs[count].ManualSessions;
        //    //objLabExport.Labs[count].PCs;
        //    //objLabExport.Labs[count].Reservations;
        //}

        //public void GetLabActivity(DateTime StartDate, DateTime EndDate, string LabType, string LabLocation)
        //{
        //    //DateTime dtS = StartDate.AddDays(-7);
        //    //DateTime dtE = EndDate.AddDays(7).AddTicks(-1);

        //    DateTime dtS = StartDate;
        //    DateTime dtE = EndDate.AddDays(1).AddTicks(-1);


        //    //string tsStart = string.Format("{0:HH:mm:ss}", (DateTime)dtS);
        //    //string tsEnd = string.Format("{0:HH:mm:ss}", (DateTime)dtE);

        //    //TimeSpan startTime = TimeSpan.Parse(tsStart);
        //    //TimeSpan endTime = TimeSpan.Parse(tsEnd);

        //    //DateTime sDate = StartDate;
        //    //DateTime eDate = EndDate;

        //    DateTime sDate = dtS;
        //    DateTime eDate = dtE;


        //    using (BookingServerReplicaEntities db = new BookingServerReplicaEntities())
        //    {
        //        List<Models.LabInfo> wrapperLabs = db.LabInfoes.ToList();
        //        List<Models.LabType> objLabType = db.LabTypes.ToList();
        //        List<Models.Location> lstLabLocs = db.Locations.ToList();
        //        List<Models.Site> objSites = db.Sites.ToList();

        //        try
        //        {
        //            int labcount = 0;
        //            foreach (var lab in wrapperLabs)
        //            {
        //                int lab_index = Array.FindIndex(objLabExport.Labs, row => row.id == lab.Id);
        //                if (objLabType.First(s => s.Id == lab.TypeId).DisplayName.Equals(LabType) && objSites.First(site => site.Id == lstLabLocs.First(s => s.Id == lab.LocationId).SiteId).RbCode.Equals(LabLocation))
        //                {
        //                    List<LabComputersPr> labComputers = new List<LabComputersPr>();
        //                    labComputers = db.LabComputersPrs.Where(x => x.LabId.ToString() == lab.Id.ToString()).ToList();
        //                    if (labComputers != null)
        //                    {
        //                        //foreach (var labs in labComputers)
        //                        //{
        //                        string labid = lab.Id.ToString();
        //                        int countManual = 0, countAutomated = 0;
        //                        int pccnt = 0, daycount = 0;
        //                        double ManualTotalHours = 0;
        //                        double AutomatedTotalHours = 0;
        //                        double ManualCaplTotalHours = 0;
        //                        double AutomatedCaplTotalHours = 0;
        //                        sDate = dtS;
        //                        eDate = dtE;
        //                        List<Daily_SummaryTable_Test> dailysummary = new List<Daily_SummaryTable_Test>();
        //                        while (sDate <= eDate)
        //                        {
        //                            foreach (var computer in labComputers)
        //                            {

        //                                int compid = computer.Id;
        //                                string sDt = sDate.ToShortDateString();
        //                                dailysummary = db.Daily_SummaryTable_Test.Where(x => x.ComputerID.ToString() == compid.ToString() && x.Date == sDt).ToList();
        //                                foreach (var summary in dailysummary)
        //                                {
        //                                    ManualTotalHours += Convert.ToDouble(summary.ManualTotalHours);
        //                                    AutomatedTotalHours += Convert.ToDouble(summary.AutomatedTotalHours);
        //                                    ManualCaplTotalHours += Convert.ToDouble(summary.ManualCaplTotalHours);
        //                                    AutomatedCaplTotalHours += Convert.ToDouble(summary.AutomatedCaplTotalHours);
        //                                }
        //                                pccnt++;
        //                            }
        //                            daycount++;
        //                            //objLabExport.Labs[lab_index].Model = labid;
        //                            objLabExport.Labs[lab_index].ManualTotalHours = ManualTotalHours;
        //                            objLabExport.Labs[lab_index].AutomatedTotalHours = AutomatedTotalHours;
        //                            objLabExport.Labs[lab_index].ManualCaplTotalHours = ManualCaplTotalHours;
        //                            objLabExport.Labs[lab_index].AutomatedCaplTotalHours = AutomatedCaplTotalHours;
        //                            sDate = sDate.AddDays(1);
        //                        }
        //                        labcount++;
        //                    }
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            HomeController.logger.Error(ex, "GetLabActivity");
        //            return;
        //        }
        //        finally
        //        {
        //            //try
        //            //{
        //            //    LabBookingWrapper.APIDispose();
        //            //}
        //            //catch (Exception ex)
        //            //{
        //            //    HomeController.logger.Error(ex, "APIDispose");
        //            //}
        //        }

        //    }
        //}


        //public void GetLCData(DateTime StartDate, DateTime EndDate)
        //{
        //    string URLString = string.Empty;
        //    StringBuilder strtemp = new StringBuilder();

        //    //Report ground work 
        //    LC_Model tempModelObj = new LC_Model();
        //    LC_TSIU tempTSIUObj = new LC_TSIU();

        //    List<SelectListItem> templst1 = new List<SelectListItem>();
        //    List<SelectListItem> templst2 = new List<SelectListItem>();
        //    List<SelectListItem> templst3 = new List<SelectListItem>();
        //    if (objLabExport != null && objLabExport.Labs.Length > 0)
        //    {
        //        if (objLabReport.LCParams == null)
        //            objLabReport.LCParams = new List<Models.LC_Model>();
        //        //if (objLabReport.LCTSIUParams == null)
        //        //{
        //        objLabReport.LCTSIUParams = new List<LC_TSIU>();
        //        //   }

        //    }

        //    foreach (LCfilterInfo.LabBookingExportLab lab in objLabExport.Labs)
        //    {
        //        var jsonItems = JsonConvert.SerializeObject(objLabExport.Labs);
        //        if (lab.id == 0)
        //        {
        //            continue;
        //        }

        //        //lstLabDef.Add(lab);
        //        if (templst1.Count == 0)
        //            templst1.Add(new SelectListItem { Text = lab.id.ToString(), Value = lab.id.ToString() });
        //        else if (!templst1.Exists(CheckNameAndId("labid", lab)))
        //            templst1.Add(new SelectListItem { Text = lab.id.ToString(), Value = lab.id.ToString() });



        //        if (templst2.Count == 0)
        //            templst2.Add(new SelectListItem { Text = lab.Location.Trim(), Value = lab.Location.Trim() });
        //        else if (!templst2.Exists(CheckNameAndId("location", lab)))
        //            templst2.Add(new SelectListItem { Text = lab.Location.Trim(), Value = lab.Location.Trim() });

        //        if (templst3.Count == 0)
        //            templst3.Add(new SelectListItem { Text = lab.Model.Trim(), Value = lab.Model.Trim() });
        //        else if (!templst3.Exists(CheckNameAndId("labtype", lab)))
        //            templst3.Add(new SelectListItem { Text = lab.Model.Trim(), Value = lab.Model.Trim() });
        //        // templst3.Add(lab.Model.Trim());



        //        //Report ground work
        //        tempModelObj.LCID = lab.id.ToString();
        //        tempModelObj.LCInventory = lab.Inventory;
        //        tempModelObj.LCLocation = lab.Location;
        //        tempModelObj.LCSubLocation = lab.SubLocation;
        //        tempModelObj.LCModel = lab.Model;
        //        tempModelObj.LCName = lab.name;
        //        tempModelObj.LCOwner = lab.Owner;
        //        tempModelObj.LCPCNode = string.Empty;
        //        tempModelObj.LCAutomatedTestTotalSpan = TimeSpan.Zero;
        //        tempModelObj.LCDefectiveTotalSpan = TimeSpan.Zero;
        //        tempModelObj.LCManualTestTotalSpan = TimeSpan.Zero;
        //        tempModelObj.LCReservedTotalSpan = TimeSpan.Zero;

        //        if (lab.Defectives != null && lab.Defectives.Length > 0)
        //        {
        //            foreach (LCfilterInfo.LabBookingExportLabDefectSpan span in lab.Defectives)
        //            {
        //                tempModelObj.LCDefectiveTotalSpan += LCreportClass.UnixTimeStampsToTimeSpan(span.start, span.end);
        //            }
        //        }
        //        if (lab.Reservations != null && lab.Reservations.Length > 0)
        //        {
        //            foreach (LCfilterInfo.LabBookingExportLabReserveSpan span in lab.Reservations)
        //            {
        //                tempModelObj.LCReservedTotalSpan += LCreportClass.UnixTimeStampsToTimeSpan(span.start, span.end);
        //            }
        //        }
        //        string projectname = "not available";
        //        if (lab.PCs != null)
        //            projectname = GetProjectInformation(lab.PCs[0].FQDN.Split('.')[0], StartDate.ToString("dd-MM-yyyy"));
        //        if (lab.ManualSessions != null && lab.ManualSessions.Length > 0)
        //        {
        //            #region commented
        //            //foreach (LCfilterInfo.LabBookingExportManualSessSpan span in lab.ManualSessions)
        //            //{
        //            //    if (span.end.Date < StartDate.Date || span.start.Date > EndDate.Date)
        //            //        continue;
        //            //    //tempModelObj.LCManualTestTotalSpan += LCreportClass.UnixTimeStampsToTimeSpan(span.start, span.end);
        //            //    tempModelObj.LCManualTestTotalSpan += span.end.Subtract(span.start);
        //            //    //tempTSIUObj.LC_TotalManualHours = LCreportClass.UnixTimeStampsToTimeSpan(span.start, span.end);
        //            //    tempTSIUObj.LC_TotalManualHours = span.end.Subtract(span.start);
        //            //    //tempTSIUObj.endTime = LCreportClass.UnixTimeStampsToDate(span.end);
        //            //    tempTSIUObj.endTime = span.end;
        //            //    //tempTSIUObj.startTime = LCreportClass.UnixTimeStampsToDate(span.start);
        //            //    tempTSIUObj.startTime = span.start;
        //            //    tempTSIUObj.TypeofUsage = "Manual";
        //            //    tempTSIUObj.ID_key = lab.id.ToString();
        //            //    tempTSIUObj.LC_ProjectName_TSIU = projectname;
        //            //    //tempTSIUObj.LC_Location = lab.Location + lab.SubLocation;
        //            //    tempTSIUObj.LC_Location = lab.Location;
        //            //    tempTSIUObj.LC_Name = lab.Inventory;
        //            //    tempTSIUObj.LC_Lab_Type = lab.Model;
        //            //    objLabReport.LCTSIUParams.Add(tempTSIUObj);
        //            //    tempTSIUObj = null;
        //            //    tempTSIUObj = new LC_TSIU();
        //            //    // mancnt++;
        //            //    //  tempTSIUObj.LC_TotalManualHours += LCreportClass.UnixTimeStampsToTimeSpan(span.start, span.end);
        //            //}
        //            #endregion
        //            #region  code for individual split of activity for more that 24 hrs
        //            foreach (LCfilterInfo.LabBookingExportManualSessSpan manTime in lab.ManualSessions)
        //            {
        //                if (manTime == null)
        //                    continue;
        //                if (manTime.end.Date < StartDate.Date || manTime.start.Date > EndDate.Date)
        //                    continue;

        //                //remove overlapping times on the boundaries of chosen start and end date
        //                List<LCfilterInfo.LabBookingExportManualSessSpan> templist = new List<LCfilterInfo.LabBookingExportManualSessSpan>();
        //                bool dayChangeFlag = false;
        //                if (manTime.start.Day == manTime.end.Day)
        //                {
        //                    dayChangeFlag = false;
        //                }
        //                else
        //                {
        //                    dayChangeFlag = true;
        //                }
        //                if (!dayChangeFlag)
        //                {

        //                }
        //                else
        //                {
        //                    double daydiff = manTime.end.Subtract(manTime.start).TotalDays;
        //                    DateTime tempsDate = manTime.start;
        //                    do
        //                    {
        //                        if (manTime.end.Date == tempsDate.Date)
        //                        {
        //                            LCfilterInfo.LabBookingExportManualSessSpan tempobj = new LCfilterInfo.LabBookingExportManualSessSpan();
        //                            tempobj.isActive = manTime.isActive;
        //                            tempobj.trigger = manTime.trigger;
        //                            tempobj.Value = manTime.Value;
        //                            tempobj.start = manTime.end.AddMinutes(-manTime.end.TimeOfDay.TotalMinutes);
        //                            tempobj.end = manTime.end;
        //                            templist.Add(tempobj);
        //                            break;
        //                        }
        //                        else if (daydiff == manTime.end.Subtract(tempsDate).TotalDays)
        //                        {
        //                            LCfilterInfo.LabBookingExportManualSessSpan tempobj = new LCfilterInfo.LabBookingExportManualSessSpan();
        //                            tempobj.isActive = manTime.isActive;
        //                            tempobj.trigger = manTime.trigger;
        //                            tempobj.Value = manTime.Value;
        //                            tempobj.start = manTime.start;
        //                            tempobj.end = manTime.start.AddDays(1).AddMinutes(-(manTime.start.TimeOfDay.TotalMinutes + 1));
        //                            templist.Add(tempobj);
        //                            tempsDate = manTime.start.AddDays(1).AddMinutes(-(manTime.start.TimeOfDay.TotalMinutes));
        //                        }
        //                        else
        //                        {
        //                            LCfilterInfo.LabBookingExportManualSessSpan tempobj = new LCfilterInfo.LabBookingExportManualSessSpan();
        //                            tempobj.isActive = manTime.isActive;
        //                            tempobj.trigger = manTime.trigger;
        //                            tempobj.Value = manTime.Value;
        //                            tempobj.start = new DateTime(manTime.end.AddDays(-daydiff).Year, manTime.end.AddDays(-daydiff).Month, manTime.end.AddDays(-daydiff).Day, 0, 0, 0);
        //                            tempobj.end = new DateTime(manTime.end.AddDays(-daydiff).Year, manTime.end.AddDays(-daydiff).Month, manTime.end.AddDays(-daydiff).Day, 23, 59, 59);
        //                            templist.Add(tempobj);
        //                            tempsDate = new DateTime(manTime.end.AddDays(-daydiff).Year, manTime.end.AddDays(-daydiff).Month, manTime.end.AddDays(-daydiff).Day, 23, 59, 59).AddMinutes(1);
        //                        }
        //                        daydiff--;
        //                    }
        //                    while (daydiff > -0.99);
        //                }
        //                if (dayChangeFlag)
        //                {
        //                    foreach (LCfilterInfo.LabBookingExportManualSessSpan manspan in templist)
        //                    {
        //                        if (manspan.end.Date < StartDate.Date || manspan.start.Date > EndDate.Date)
        //                            continue;

        //                        //tempModelObj.LCManualTestTotalSpan += LCreportClass.UnixTimeStampsToTimeSpan(span.start, span.end);
        //                        tempModelObj.LCManualTestTotalSpan += manspan.end.Subtract(manspan.start);
        //                        //tempTSIUObj.LC_TotalManualHours = LCreportClass.UnixTimeStampsToTimeSpan(span.start, span.end);
        //                        tempTSIUObj.LC_TotalManualHours = manspan.end.Subtract(manspan.start);
        //                        //tempTSIUObj.endTime = LCreportClass.UnixTimeStampsToDate(span.end);
        //                        tempTSIUObj.endTime = manspan.end.ToUniversalTime().AddHours(GetUtcOffsetFromcountryCode(objSites.First(x => x.RbCode == lab.Location).CountryCode));
        //                        //tempTSIUObj.startTime = LCreportClass.UnixTimeStampsToDate(span.start);
        //                        tempTSIUObj.startTime = manspan.start.ToUniversalTime().AddHours(GetUtcOffsetFromcountryCode(objSites.First(x => x.RbCode == lab.Location).CountryCode));
        //                        if (manspan.Value.ToLower().Contains("can"))
        //                        {
        //                            tempTSIUObj.TypeofUsage = "Manual_CAPL";
        //                            tempTSIUObj.LC_TotalManualCAPLHours = manspan.end.Subtract(manspan.start);
        //                        }
        //                        else
        //                        {
        //                            tempTSIUObj.LC_TotalManualHours = manspan.end.Subtract(manspan.start);
        //                            tempTSIUObj.TypeofUsage = "Manual";
        //                        }

        //                        tempTSIUObj.ID_key = lab.id.ToString();
        //                        tempTSIUObj.LC_ProjectName_TSIU = projectname;
        //                        //tempTSIUObj.LC_Location = lab.Location + lab.SubLocation;
        //                        tempTSIUObj.LC_Location = lab.Location;
        //                        tempTSIUObj.LC_Name = lab.Inventory;
        //                        tempTSIUObj.LC_Lab_Type = lab.Model;
        //                        objLabReport.LCTSIUParams.Add(tempTSIUObj);
        //                        tempTSIUObj = null;
        //                        tempTSIUObj = new LC_TSIU();
        //                    }
        //                    templist.Clear();
        //                }
        //                else
        //                {
        //                    //tempModelObj.LCManualTestTotalSpan += LCreportClass.UnixTimeStampsToTimeSpan(span.start, span.end);
        //                    tempModelObj.LCManualTestTotalSpan += manTime.end.Subtract(manTime.start);
        //                    //tempTSIUObj.LC_TotalManualHours = LCreportClass.UnixTimeStampsToTimeSpan(span.start, span.end);

        //                    //tempTSIUObj.endTime = LCreportClass.UnixTimeStampsToDate(span.end);
        //                    tempTSIUObj.endTime = manTime.end.ToUniversalTime().AddHours(GetUtcOffsetFromcountryCode(objSites.First(x => x.RbCode == lab.Location).CountryCode));
        //                    //tempTSIUObj.startTime = LCreportClass.UnixTimeStampsToDate(span.start);
        //                    tempTSIUObj.startTime = manTime.start.ToUniversalTime().AddHours(GetUtcOffsetFromcountryCode(objSites.First(x => x.RbCode == lab.Location).CountryCode));
        //                    if (manTime.Value.ToLower().Contains("can"))
        //                    {
        //                        tempTSIUObj.TypeofUsage = "Manual_CAPL";
        //                        tempTSIUObj.LC_TotalManualCAPLHours = manTime.end.Subtract(manTime.start);
        //                    }
        //                    else
        //                    {
        //                        tempTSIUObj.TypeofUsage = "Manual";
        //                        tempTSIUObj.LC_TotalManualHours = manTime.end.Subtract(manTime.start);
        //                    }
        //                    tempTSIUObj.ID_key = lab.id.ToString();
        //                    tempTSIUObj.LC_ProjectName_TSIU = projectname;
        //                    //tempTSIUObj.LC_Location = lab.Location + lab.SubLocation;
        //                    tempTSIUObj.LC_Location = lab.Location;
        //                    tempTSIUObj.LC_Name = lab.Inventory;
        //                    tempTSIUObj.LC_Lab_Type = lab.Model;
        //                    objLabReport.LCTSIUParams.Add(tempTSIUObj);
        //                    tempTSIUObj = null;
        //                    tempTSIUObj = new LC_TSIU();
        //                }

        //            }

        //            #endregion

        //        }
        //        if (lab.AutomatedSessions != null && lab.AutomatedSessions.Length > 0)
        //        {
        //            #region commented
        //            //foreach (LCfilterInfo.LabBookingExportAutoSessSpan span in lab.AutomatedSessions)
        //            //{
        //            //    //tempModelObj.LCAutomatedTestTotalSpan += LCreportClass.UnixTimeStampsToTimeSpan(span.start, span.end);
        //            //    tempModelObj.LCAutomatedTestTotalSpan += span.end.Subtract(span.start);
        //            //    //tempTSIUObj.LC_AutomatedTotalHours = LCreportClass.UnixTimeStampsToTimeSpan(span.start, span.end);
        //            //    tempTSIUObj.LC_AutomatedTotalHours = span.end.Subtract(span.start);
        //            //    //tempTSIUObj.endTime = LCreportClass.UnixTimeStampsToDate(span.end);
        //            //    tempTSIUObj.endTime = span.end;
        //            //    //tempTSIUObj.startTime = LCreportClass.UnixTimeStampsToDate(span.start);
        //            //    tempTSIUObj.startTime = span.start;
        //            //    tempTSIUObj.TypeofUsage = "Automated";
        //            //    tempTSIUObj.ID_key = lab.id.ToString(); ;
        //            //    tempTSIUObj.LC_ProjectName_TSIU = projectname;
        //            //    tempTSIUObj.LC_Location = lab.Location + lab.SubLocation;
        //            //    tempTSIUObj.LC_Name = lab.Inventory;
        //            //    tempTSIUObj.LC_Lab_Type = lab.Model;
        //            //    objLabReport.LCTSIUParams.Add(tempTSIUObj);
        //            //    tempTSIUObj = null;
        //            //    tempTSIUObj = new LC_TSIU();
        //            //    //  tempTSIUObj.LC_AutomatedTotalHours = LCreportClass.UnixTimeStampsToTimeSpan(span.start, span.end);
        //            //    //autocnt++;
        //            //}
        //            #endregion
        //            #region  code to split for activities more than 24 hours
        //            foreach (LCfilterInfo.LabBookingExportAutoSessSpan AutoTime in lab.AutomatedSessions)
        //            {
        //                if (AutoTime == null)
        //                    continue;
        //                if (AutoTime.end.Date < StartDate.Date || AutoTime.start.Date > EndDate.Date)
        //                    continue;

        //                //remove overlapping times on the boundaries of chosen start and end date
        //                List<LCfilterInfo.LabBookingExportAutoSessSpan> templist = new List<LCfilterInfo.LabBookingExportAutoSessSpan>();
        //                bool dayChangeFlag = false;
        //                if (AutoTime.start.Day == AutoTime.end.Day)
        //                {
        //                    dayChangeFlag = false;
        //                }
        //                else
        //                {
        //                    dayChangeFlag = true;
        //                }
        //                if (!dayChangeFlag)
        //                {

        //                }
        //                else
        //                {
        //                    double daydiff = AutoTime.end.Subtract(AutoTime.start).TotalDays;
        //                    DateTime tempsDate = AutoTime.start;
        //                    do
        //                    {
        //                        if (AutoTime.end.Date == tempsDate.Date)
        //                        {
        //                            LCfilterInfo.LabBookingExportAutoSessSpan tempobj = new LCfilterInfo.LabBookingExportAutoSessSpan();
        //                            tempobj.isActive = AutoTime.isActive;
        //                            tempobj.trigger = AutoTime.trigger;
        //                            tempobj.Value = AutoTime.Value;
        //                            tempobj.start = AutoTime.end.AddMinutes(-AutoTime.end.TimeOfDay.TotalMinutes);
        //                            tempobj.end = AutoTime.end;
        //                            templist.Add(tempobj);
        //                            break;
        //                        }
        //                        else if (daydiff == AutoTime.end.Subtract(tempsDate).TotalDays)
        //                        {
        //                            LCfilterInfo.LabBookingExportAutoSessSpan tempobj = new LCfilterInfo.LabBookingExportAutoSessSpan();
        //                            tempobj.isActive = AutoTime.isActive;
        //                            tempobj.trigger = AutoTime.trigger;
        //                            tempobj.Value = AutoTime.Value;
        //                            tempobj.start = AutoTime.start;
        //                            tempobj.end = AutoTime.start.AddDays(1).AddMinutes(-(AutoTime.start.TimeOfDay.TotalMinutes + 1));
        //                            templist.Add(tempobj);
        //                            tempsDate = AutoTime.start.AddDays(1).AddMinutes(-(AutoTime.start.TimeOfDay.TotalMinutes));
        //                        }
        //                        else
        //                        {
        //                            LCfilterInfo.LabBookingExportAutoSessSpan tempobj = new LCfilterInfo.LabBookingExportAutoSessSpan();
        //                            tempobj.isActive = AutoTime.isActive;
        //                            tempobj.trigger = AutoTime.trigger;
        //                            tempobj.Value = AutoTime.Value;
        //                            tempobj.start = new DateTime(AutoTime.end.AddDays(-daydiff).Year, AutoTime.end.AddDays(-daydiff).Month, AutoTime.end.AddDays(-daydiff).Day, 0, 0, 0);
        //                            tempobj.end = new DateTime(AutoTime.end.AddDays(-daydiff).Year, AutoTime.end.AddDays(-daydiff).Month, AutoTime.end.AddDays(-daydiff).Day, 23, 59, 59);
        //                            templist.Add(tempobj);
        //                            tempsDate = new DateTime(AutoTime.end.AddDays(-daydiff).Year, AutoTime.end.AddDays(-daydiff).Month, AutoTime.end.AddDays(-daydiff).Day, 23, 59, 59).AddMinutes(1);
        //                        }
        //                        daydiff--;
        //                    }
        //                    while (daydiff > -0.99);
        //                }
        //                if (dayChangeFlag)
        //                {
        //                    foreach (LCfilterInfo.LabBookingExportAutoSessSpan autospan in templist)
        //                    {
        //                        if (autospan.end.Date < StartDate.Date || autospan.start.Date > EndDate.Date)
        //                            continue;

        //                        //tempModelObj.LCAutomatedTestTotalSpan += LCreportClass.UnixTimeStampsToTimeSpan(span.start, span.end);
        //                        tempModelObj.LCAutomatedTestTotalSpan += autospan.end.Subtract(autospan.start);
        //                        //tempTSIUObj.LC_AutomatedTotalHours = LCreportClass.UnixTimeStampsToTimeSpan(span.start, span.end);
        //                        tempTSIUObj.LC_AutomatedTotalHours = autospan.end.Subtract(autospan.start);
        //                        //tempTSIUObj.endTime = LCreportClass.UnixTimeStampsToDate(span.end);
        //                        tempTSIUObj.endTime = autospan.end.ToUniversalTime().AddHours(GetUtcOffsetFromcountryCode(objSites.First(x => x.RbCode == lab.Location).CountryCode));
        //                        //tempTSIUObj.startTime = LCreportClass.UnixTimeStampsToDate(span.start);
        //                        tempTSIUObj.startTime = autospan.start.ToUniversalTime().AddHours(GetUtcOffsetFromcountryCode(objSites.First(x => x.RbCode == lab.Location).CountryCode));
        //                        if (autospan.Value.ToLower().Contains("can"))
        //                        {
        //                            tempTSIUObj.TypeofUsage = "Automated_CAPL";
        //                            tempTSIUObj.LC_AutomatedCAPLTotalHours = autospan.end.Subtract(autospan.start);
        //                        }
        //                        else
        //                        {
        //                            tempTSIUObj.TypeofUsage = "Automated";
        //                            tempTSIUObj.LC_AutomatedTotalHours = autospan.end.Subtract(autospan.start);
        //                        }
        //                        tempTSIUObj.ID_key = lab.id.ToString(); ;
        //                        tempTSIUObj.LC_ProjectName_TSIU = projectname;
        //                        tempTSIUObj.LC_Location = lab.Location + lab.SubLocation;
        //                        tempTSIUObj.LC_Name = lab.Inventory;
        //                        tempTSIUObj.LC_Lab_Type = lab.Model;
        //                        objLabReport.LCTSIUParams.Add(tempTSIUObj);
        //                        tempTSIUObj = null;
        //                        tempTSIUObj = new LC_TSIU();
        //                    }
        //                    templist.Clear();
        //                }
        //                else
        //                {
        //                    //tempModelObj.LCAutomatedTestTotalSpan += LCreportClass.UnixTimeStampsToTimeSpan(span.start, span.end);
        //                    tempModelObj.LCAutomatedTestTotalSpan += AutoTime.end.Subtract(AutoTime.start);
        //                    //tempTSIUObj.LC_AutomatedTotalHours = LCreportClass.UnixTimeStampsToTimeSpan(span.start, span.end);

        //                    //tempTSIUObj.endTime = LCreportClass.UnixTimeStampsToDate(span.end);
        //                    tempTSIUObj.endTime = AutoTime.end.ToUniversalTime().AddHours(GetUtcOffsetFromcountryCode(objSites.First(x => x.RbCode == lab.Location).CountryCode));
        //                    //tempTSIUObj.startTime = LCreportClass.UnixTimeStampsToDate(span.start);
        //                    tempTSIUObj.startTime = AutoTime.start.ToUniversalTime().AddHours(GetUtcOffsetFromcountryCode(objSites.First(x => x.RbCode == lab.Location).CountryCode));
        //                    if (AutoTime.Value.ToLower().Contains("can"))
        //                    {
        //                        tempTSIUObj.TypeofUsage = "Automated_CAPL";
        //                        tempTSIUObj.LC_AutomatedCAPLTotalHours = AutoTime.end.Subtract(AutoTime.start);
        //                    }
        //                    else
        //                    {
        //                        tempTSIUObj.TypeofUsage = "Automated";
        //                        tempTSIUObj.LC_AutomatedTotalHours = AutoTime.end.Subtract(AutoTime.start);
        //                    }
        //                    tempTSIUObj.ID_key = lab.id.ToString(); ;
        //                    tempTSIUObj.LC_ProjectName_TSIU = projectname;
        //                    tempTSIUObj.LC_Location = lab.Location + lab.SubLocation;
        //                    tempTSIUObj.LC_Name = lab.Inventory;
        //                    tempTSIUObj.LC_Lab_Type = lab.Model;
        //                    objLabReport.LCTSIUParams.Add(tempTSIUObj);
        //                    tempTSIUObj = null;
        //                    tempTSIUObj = new LC_TSIU();
        //                }
        //            }
        //            #endregion


        //        }
        //        objLabReport.LCParams.Add(tempModelObj);
        //        tempModelObj = null;
        //        tempModelObj = new LC_Model();
        //        // objLabReport.LCParams.Add(tempModelObj);
        //    }

        //    //lstLabIDs = templst1.Distinct().ToList();
        //    //lstLocs = templst2.Distinct().ToList();
        //    //lstLabTypes = templst3.Distinct().ToList();

        //    templst1.RemoveRange(0, templst1.Count);
        //    templst2.RemoveRange(0, templst2.Count);
        //    templst3.RemoveRange(0, templst3.Count);
        //}
        //#endregion
        //static double GetUtcOffsetFromcountryCode(string countryCode)
        //{
        //    switch (countryCode.ToUpper())
        //    {
        //        case "AU": return 10;
        //        case "CN": return 8;
        //        case "DE": return 1.0;
        //        case "FR": return 1.0;
        //        case "IN": return 5.5;
        //        case "JP": return 9;
        //        case "MX": return -6.0;
        //        case "PT": return 0.0;
        //        case "US": return -5.0;
        //        case "VN": return 7.0;
        //        // To be completed with all known Countries
        //        default: return 0.0;
        //    }
        //}

        //public static string GetProjectInformation(string computername, string startDate)
        //{
        //    try
        //    {
        //        string connectionString, projectname = String.Empty;
        //        connectionString = @"Data Source=COB-C-00092\\SQLEXPRESS;Initial Catalog=PromasterImport_HWInfo;Integrated Security=SSPI;"; //use this ipaddress 10.169.31.156



        //        using (SqlConnection _con = new SqlConnection(connectionString))
        //        {
        //            string queryStatement = "SELECT DISTINCT EEPName FROM dbo.Hardware_Desc WHERE System_name ='" + computername.ToUpper() + "' AND Date ='" + startDate + "'";

        //            using (SqlCommand _cmd = new SqlCommand(queryStatement, _con))
        //            {
        //                DataTable customerTable = new DataTable("Top5Customers");

        //                SqlDataAdapter _dap = new SqlDataAdapter(_cmd);

        //                _con.Open();

        //                _dap.Fill(customerTable);

        //                if (customerTable != null)
        //                    if (customerTable.Rows != null)
        //                        if (customerTable.Rows.Count > 0)
        //                            if (customerTable.Rows[0] != null)
        //                                if (customerTable.Rows[0].ItemArray != null)
        //                                    if (customerTable.Rows[0].ItemArray.Count() > 0)
        //                                        projectname = customerTable.Rows[0].ItemArray[0].ToString();
        //                _con.Close();

        //            }


        //            return projectname;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        HomeController.logger.LogException(LogLevel.Error, "getprojectInformation", ex);
        //        return "not available";
        //    }
        //}

        //private static Predicate<SelectListItem> CheckNameAndId(string key, LCfilterInfo.LabBookingExportLab lab)
        //{
        //    if (key.Equals("location"))
        //        return x => x.Text == lab.Location.Trim() && x.Value == lab.Location.Trim();
        //    else if (key.Equals("labtype"))
        //        return x => x.Text == lab.Model.Trim() && x.Value == lab.Model.Trim();
        //    else if (key.Equals("labid"))
        //        return x => x.Text == lab.id.ToString() && x.Value == lab.id.ToString();
        //    else
        //        return null;
        //}
    }



    /// <summary>
    /// template for the Request table view
    /// </summary>
    /// 

    public class TC_List
    {
        public List<TCView> tclist { get; set; }
    }

    public partial class TCView
    {

        public int ID { get; set; }
        public int Cmmt_Item { get; set; }
        public int Year { get; set; }
        public decimal Budget_Plan { get; set; }
        public decimal Invoice { get; set; }
        public decimal Open { get; set; }
        public decimal Available { get; set; }
        public decimal Bud_Inv { get; set; }



    }


    public partial class HwDamageView
    {

        public int ID { get; set; }
        public int Year { get; set; }
        public string     Location       { get; set; }
        public string Month         { get; set; }
        public int Item_Name     { get; set; }
        public int BU { get; set; }
        public int Qty            { get; set; }
        public string Project_Team   { get; set; }
        public decimal Cost_inEUR     { get; set; }
        public decimal Total_Price    { get; set; }
        public string RequestorNT   { get; set; }
        public string Hw_status { get; set; }
        public DateTime Updated_At { get; set; }
        public string Remarks { get; set; }


    }

    public class HWRepairable
    {
       
        public int S_No { get; set; }
        public string ItemName { get; set; }
        public int Repair_Currency { get; set; }
        public double Repairable_Cost { get; set; }
        public double Repairable_Cost_EUR { get; set; }
        public DateTime Repair_UpdatedAt { get; set; }
        public string Repair_UpdatedBy { get; set; }
    }

    public partial class HwDamageCost
    {
        public int ID { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public int Location { get; set; }
        public int BU { get; set; }
        public int Item_Name { get; set; }
        public int Qty { get; set; }
        public int Project_Team { get; set; }
        public int Cost_inEUR { get; set; }
        public int Total_Price { get; set; }
        public string RequestorNT { get; set; }
        public string Hw_status { get; set; }
        public string Remarks { get; set; }

    }

    public partial class HCView
    {

        public int ID { get; set; }
        public string SkillSet { get; set; }
        public string Year { get; set; }
        public decimal Utilize { get; set; }
        public decimal Plan { get; set; }
        public List<string> Role { get; set; }
      
    }


}