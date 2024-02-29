using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace LC_Reports_V1.Controllers
{
    public class GRMEMSupportController : Controller
    {
        private SqlConnection budgetingcon;
        private void connection()
        {

            string budgeting_constring = ConfigurationManager.ConnectionStrings["BudgetingdbConnection"].ToString();
            budgetingcon = new SqlConnection(budgeting_constring);
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



        // GET: GRMEMSupport
        public ActionResult Index()
        {

            return View();
        }


        // GET: GRMEMSupport/Details/5
        public JsonResult Getdata(string year)
        {
            DataTable dt = new DataTable();
            try
            {
                connection();

                string Query = "select [VKMPlanning_GRMEMSupport].*, section_table.section, Groups_Table_Test.[group],BU_Table.BU," +
                //"HC_Role_Table.RoleName ," + 
                "( select ',' + HC_SkillSet_Table.SkillSetName  from dbo.HC_SkillSet_Table where ',' + [VKMPlanning_GRMEMSupport].VKM_Role + ',' like '%,' + convert(varchar, HC_SkillSet_Table.ID) + ',%' " +
                " for xml path(''), type ).value('substring(text()[1], 2)', 'varchar(max)') as VKM_Role," +
                " dept_table.dept,  HC_Role_Table.RoleName  from[VKMPlanning_GRMEMSupport] inner join Groups_Table_Test on[VKMPlanning_GRMEMSupport].[Group_ID] " +
                "= Groups_Table_Test.id inner join section_table on[VKMPlanning_GRMEMSupport].Section_ID = section_table.id inner join BU_Table on " +
                "[VKMPlanning_GRMEMSupport].Product_Area_ID = BU_Table.id" +
                "  inner join HC_Role_Table on[VKMPlanning_GRMEMSupport].SAP_Role = HC_Role_Table.id inner join dept_table on" +
                " [VKMPlanning_GRMEMSupport].Department_ID = dept_table.id where [VKMPlanning_GRMEMSupport].Year = '" + year + "'  order by Employee_Name";
                //"where ISNULL([VKMPlanning_GRMEMSupport].SAP_Role, '') <> ''";
                //inner join HC_Role_Table on[VKMPlanning_GRMEMSupport].SAP_Role = HC_Role_Table.ID"

                BudgetingOpenConnection();
                SqlCommand cmd = new SqlCommand(Query, budgetingcon);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
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
            return result1;
        }

        [HttpGet]
        public ActionResult GenerateGRMEM(string year)
        {

            return Json(new { data = Getdata(year) }, JsonRequestBehavior.AllowGet);
        }


        // POST: GRMEMSupport/Edit/5
        [HttpPost]
        public ActionResult Savedata(GRMEMSupportView req)
        {
            DataTable dt = new DataTable();
            string Query = "";
            int isSkillSet_exist, isHCentry_exist = 0;
            string ids_list = "";
            SqlCommand cmd;
            connection();
            BudgetingOpenConnection();
            foreach (var i in req.VKM_Role)
            {
               // Query = "IF EXISTS(SELECT ID FROM [HC_Role_Table] WHERE RoleName = @RoleName_i)SELECT 1 ELSE SELECT 0";
                Query = "IF EXISTS(SELECT ID FROM [HC_SkillSet_Table] WHERE SkillSetName = @SkillSetName_i)SELECT 1 ELSE SELECT 0";
                cmd = new SqlCommand(Query, budgetingcon);
                cmd.Parameters.AddWithValue("@SkillSetName_i ", i);
                try
                {
                    BudgetingOpenConnection();
                    isSkillSet_exist = int.Parse(cmd.ExecuteScalar().ToString());
                    BudgetingCloseConnection();

                    if (isSkillSet_exist == 0)
                    {
                        //insert i - req.role
                        Query = "INSERT INTO [HC_SkillSet_Table] (SkillSetName) VALUES(@SkillSetName)";
                        cmd = new SqlCommand(Query, budgetingcon);
                        cmd.Parameters.AddWithValue("@SkillSetName ", i);
                        try
                        {
                            BudgetingOpenConnection();
                            cmd.ExecuteNonQuery();
                            Console.WriteLine("Records Inserted Successfully");
                        }
                        catch (Exception ex)
                        {
                            //MessageBox.Show(" Not Updated");
                        }
                        finally
                        {
                            BudgetingCloseConnection();
                        }
                    }
                }
                catch (Exception ex)
                {
                    //handle exception
                }

            }
            //inserting entries in grmem_tbl if req.SNo == 0
            //else updte entry
            //while updating append the ids of the req.roles with , 
            string SkillSetNames_list = "\'" + String.Join("\',\'", req.VKM_Role.ToArray()) + "\'";
            Query = "SELECT string_agg(id, ',') as SkillSetIDs FROM[HC_SkillSet_Table] WHERE SkillSetName in (" + SkillSetNames_list + ")";
            //SELECT ID FROM [HC_Role_Table] WHERE RoleName in (" + RoleNames_list +")";
            cmd = new SqlCommand(Query, budgetingcon);
            //cmd.Parameters.AddWithValue("@RoleNames_list ", "\'"  +  String.Join("\',\'", req.Role.ToArray()).Replace("\r\n","") + "\'");
            try
            {
                BudgetingOpenConnection();
                //SqlDataAdapter da = new SqlDataAdapter(cmd);
                //da.Fill(dt);
                //var y = String.Join(",", dt.AsEnumerable().Select(x => x.ItemArray.ToString()));
                ids_list = cmd.ExecuteScalar().ToString();
                BudgetingCloseConnection();


            }
            catch (Exception ex)
            {
                //MessageBox.Show(" Not Updated");
            }
            finally
            {
                BudgetingCloseConnection();
            }
            if (req.SNo != 0)
            {
                Query = "UPDATE [VKMPlanning_GRMEMSupport] SET [NTID] = @NTID ," +
                    " Employee_Number= @Employee_Number, " +
                    "[Employee_Name]= @Employee_Name," +
                    "[Product_Area_ID] =@Product_Area," +
                    " Section_ID =@Section, [Year] = @Year, " +
                    "Department_ID = @Department,[Group_ID] = @Group, " +
                    "Level = @Level, SAP_Role = @SAP_Role , VKM_Role = @VKM_Role ," +
                    " Remarks = @Remarks , Plan_Sum = @Plan_Sum, " +
                    "Jan = @Jan, Feb = @Feb, Mar = @Mar, Apr = @Apr, May = @May , Jun = @Jun , Jul = @Jul , Aug = @Aug , Sep = @Sep , Oct = @Oct , Nov = @Nov, Dec = @Dec , " +
                    "PYO = @PYO , Updated_By = @Updated_By , " +
                    "Updated_At = @Updated_At   WHERE SNo = @SNo";
            }
            else
            {
                //insert (SkillSet, Role, Year, [Plan],  Utilize) VALUES(@SkillSet , @Role, @Year, @Plan, @Utilize
                Query = "INSERT INTO [VKMPlanning_GRMEMSupport] " //+
                    //"([SNo],"
                    + "([NTID],"
                    + "[Employee_Number],"
                    + "[Employee_Name],"
                    + "[Product_Area_ID],"
                    + "[Section_ID],"
                    + "[Department_ID],"
                    + "[Group_ID],"
                    + "[Level],"
                    + "[SAP_Role],"
                    + "[VKM_Role],"
                    + "[Remarks],"
                    + "[Plan_Sum],"
                    + "[Jan],"
                    + "[Feb],"
                    + "[Mar],"
                    + "[Apr],"
                    + "[May],"
                    + "[Jun],"
                    + "[Jul],"
                    + "[Aug],"
                    + "[Sep],"
                    + "[Oct],"
                    + "[Nov],"
                    + "[Dec],"
                    + "[PYO],"
                    + "[Updated_By],"
                    + "[Updated_At],"
                    + "[Year])"

                    + "VALUES"
                    //+ "(@SNo,"
                    + "(@NTID,"
                    + "@Employee_Number,"
                    + "@Employee_Name,"
                    + "@Product_Area,"
                    + "@Section,"
                    + "@Department,"
                    + "@Group,"
                    + "@Level,"
                    + "@SAP_Role  ,"
                    + "@VKM_Role,"
                    + "@Remarks,"
                    + "@Plan_Sum,"
                    + "@Jan,"
                    + "@Feb,"
                    + "@Mar,"
                    + "@Apr,"
                    + "@May,"
                    + "@Jun,"
                    + "@Jul,"
                    + "@Aug,"
                    + "@Sep,"
                    + "@Oct,"
                    + "@Nov,"
                    + "@Dec,"
                    + "@PYO,"
                    + "@Updated_By,"
                    + "@Updated_At,"
                    + "@Year)";
            }
            //SELECT STRING_AGG(CONVERT(NVARCHAR(max), chkjoin ), ',') AS csv FROM Person.Person;
            cmd = new SqlCommand(Query, budgetingcon);
            cmd.Parameters.AddWithValue("@NTID ", req.NTID.ToUpper().Trim());
            cmd.Parameters.AddWithValue("@Employee_Number", req.Employee_Number);
            cmd.Parameters.AddWithValue("@Employee_Name", req.Employee_Name);
            cmd.Parameters.AddWithValue("@Product_Area", req.Product_Area);
            cmd.Parameters.AddWithValue("@Section ", req.Section);
            cmd.Parameters.AddWithValue("@Department", req.Department);
            cmd.Parameters.AddWithValue("@Group", req.Group);
            cmd.Parameters.AddWithValue("@Level", req.Level);
            cmd.Parameters.AddWithValue("@SAP_Role", req.SAP_Role);/*String.Join(",", req.Role.ToArray())*/
            cmd.Parameters.AddWithValue("@VKM_Role ", ids_list );
            cmd.Parameters.AddWithValue("@Remarks", req.Remarks == null ? string.Empty : req.Remarks);
            cmd.Parameters.AddWithValue("@Plan_Sum", req.Plan_Sum);//@Year
            cmd.Parameters.AddWithValue("@Year", req.Year);
            cmd.Parameters.AddWithValue("@PYO", req.PYO);

          
            cmd.Parameters.AddWithValue("@Jan", req.Jan);
            cmd.Parameters.AddWithValue("@Feb", req.Feb);
            cmd.Parameters.AddWithValue("@Mar ", req.Mar);
            cmd.Parameters.AddWithValue("@Apr", req.Apr);
            cmd.Parameters.AddWithValue("@May", req.May);
            cmd.Parameters.AddWithValue("@Jun", req.Jun);
            cmd.Parameters.AddWithValue("@Jul", req.Jul);
            cmd.Parameters.AddWithValue("@Aug ", req.Aug);
            cmd.Parameters.AddWithValue("@Sep", req.Sep);
            cmd.Parameters.AddWithValue("@Oct", req.Oct);
            cmd.Parameters.AddWithValue("@Nov", req.Nov);
            cmd.Parameters.AddWithValue("@Dec", req.Dec);

            cmd.Parameters.AddWithValue("@Updated_By", System.Web.HttpContext.Current.User.Identity.Name.Split('\\')[1].ToUpper());
            cmd.Parameters.AddWithValue("@Updated_At", DateTime.Now);



            //if (req.SNo != 0)
                cmd.Parameters.AddWithValue("@SNo", req.SNo);


            try
            {
                BudgetingOpenConnection();
                cmd.ExecuteNonQuery();
                Console.WriteLine("Records Inserted Successfully");
            }
            catch (Exception ex)
            {
                //MessageBox.Show(" Not Updated");
            }
            finally
            {
                BudgetingCloseConnection();
            }


            //string Query = " SELECT ID FROM[HC_SkillSet_Table] WHERE SkillSetName = '" + row[1] + "'";
            //SqlCommand cmd = new SqlCommand(Query, conn);
            //SqlDataAdapter da = new SqlDataAdapter(cmd);
            //da.Fill(dt);

            //CloseConnection();
            //var skillsetid = dt.Rows[0].ItemArray[0];
            //OpenConnection();

            //////////////////automatic update of hc table based on edit/add in grm em support
            ///
            Query = "IF EXISTS(SELECT ID FROM [HC_Table] WHERE Role = @Role and SkillSet = @SkillSet)SELECT 1 ELSE SELECT 0";
            cmd = new SqlCommand(Query, budgetingcon);
            cmd.Parameters.AddWithValue("@Role ", req.SAP_Role);
            cmd.Parameters.AddWithValue("@SkillSet ", ids_list);
            try
            {
                BudgetingOpenConnection();
                isHCentry_exist = int.Parse(cmd.ExecuteScalar().ToString());
                BudgetingCloseConnection();
               

                
                //role, skillset, yr , ishcentry_exists paramss
                //if sap role - vkm role combo not preent in hc table -> create new entry in hc_table ith pyo for req.year & plan
                //else fetch all rows from grmemsupport ith sap role - vkm role combo and year == req. year-> add PYO of the filtered ros
                //->update the row in hc table with the combo with pyo total for hat yr


                //if (isHCentry_exist == 0)
                //{
                //    //insert i - req.role
                //    Query = "INSERT INTO [HC_Role_Table] (RoleName) VALUES(@RoleName)";
                //    cmd = new SqlCommand(Query, budgetingcon);
                //    cmd.Parameters.AddWithValue("@RoleName ", i);
                //    try
                //    {
                //        BudgetingOpenConnection();
                //        cmd.ExecuteNonQuery();
                //        Console.WriteLine("Records Inserted Successfully");
                //    }
                //    catch (Exception ex)
                //    {
                //        //MessageBox.Show(" Not Updated");
                //    }
                //    finally
                //    {
                //        BudgetingCloseConnection();
                //    }
                //}
            }
            catch (Exception ex)
            {
                //handle exception
            }

            Query = "exec[dbo].[InsertHCTable] '" + req.SAP_Role + "','" + ids_list + "','" + req.Year + "','" + isHCentry_exist + "'  ";
            cmd = new SqlCommand(Query, budgetingcon);
            try
            {
                BudgetingOpenConnection();
                cmd.ExecuteNonQuery();
                BudgetingCloseConnection(); 
            }
            catch (Exception ex)
            {
                //handle exception
            }
            return Json(new { data = Getdata((DateTime.Now.Year)+1.ToString()), success = true, msg = "Updated successfully" }, JsonRequestBehavior.AllowGet);


        }


        //POST: GRMEMSupport/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {

            DataTable dt = new DataTable();
            connection();
            dt = new DataTable();
            string Query = "";
            int year = 0;
            string sap_role = "", vkm_role = "";
            //Query = "UPDATE [TravelCost_Table] SET [Cmmt_Item] = '" + req.Cmmt_Item + "', [Year] = '" + req.Year + "',[Budget_Plan] = '" + req.Budget_Plan + "',[Invoice] = '" + req.Invoice + "',[Open] = '" + req.Open + "',  WHERE ID = '" + req.ID +  "'";
            Query = "IF EXISTS(SELECT SNo FROM [VKMPlanning_GRMEMSupport] WHERE SNo = @SNo)begin select year,sap_role,vkm_role from [VKMPlanning_GRMEMSupport] WHERE SNo = @SNo;DELETE FROM [VKMPlanning_GRMEMSupport] WHERE SNo = @SNo;end";

            SqlCommand cmd = new SqlCommand(Query, budgetingcon);

            cmd.Parameters.AddWithValue("@SNo", id);


            try
            {
                BudgetingOpenConnection();
                //cmd = new SqlCommand(Query, budgetingcon);
                //cmd.Parameters.AddWithValue("@User ", User);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    year = Convert.ToInt32(dr["Year"].ToString());
                    sap_role = dr["SAP_Role"].ToString();
                    vkm_role = dr["VKM_Role"].ToString();

                }
                dr.Close();
                Query = "IF EXISTS(SELECT SNo FROM [VKMPlanning_GRMEMSupport] WHERE SNo = @SNo)DELETE FROM [VKMPlanning_GRMEMSupport] WHERE SNo = @SNo;";
                cmd = new SqlCommand(Query, budgetingcon);
                cmd.Parameters.AddWithValue("@SNo", id);
                cmd.ExecuteNonQuery();
                Query = "exec[dbo].[InsertHCTable] '" + sap_role + "','" + vkm_role + "','" + year + "','" + 1 + "'  ";
                cmd = new SqlCommand(Query, budgetingcon);
                try
                {
                    cmd.ExecuteNonQuery();

                }
                catch (Exception ex)
                {

                }

                Console.WriteLine("Records Deleted Successfully");
            }
            catch (Exception ex)
            {
                //MessageBox.Show(" Not Updated");
            }
            finally
            {
                BudgetingCloseConnection();
            }
            return Json(new { data = Getdata(DateTime.Now.Year + 1.ToString()), success = true }, JsonRequestBehavior.AllowGet);

        }



        //public ActionResult Delete(int id)
        //{

        //    DataTable dt = new DataTable();
        //    connection();
        //    dt = new DataTable();
        //    string Query = "";

        //    //Query = "UPDATE [TravelCost_Table] SET [Cmmt_Item] = '" + req.Cmmt_Item + "', [Year] = '" + req.Year + "',[Budget_Plan] = '" + req.Budget_Plan + "',[Invoice] = '" + req.Invoice + "',[Open] = '" + req.Open + "',  WHERE ID = '" + req.ID +  "'";
        //    Query = "IF EXISTS(SELECT SNo FROM [VKMPlanning_GRMEMSupport] WHERE SNo = @SNo)DELETE FROM [VKMPlanning_GRMEMSupport] WHERE SNo = @SNo";

        //    SqlCommand cmd = new SqlCommand(Query, budgetingcon);

        //    cmd.Parameters.AddWithValue("@SNo", id);


        //    try
        //    {
        //        BudgetingOpenConnection();
        //        cmd.ExecuteNonQuery();
        //        Console.WriteLine("Records Deleted Successfully");
        //    }
        //    catch (Exception ex)
        //    {
        //        //MessageBox.Show(" Not Updated");
        //    }
        //    finally
        //    {
        //        BudgetingCloseConnection();
        //    }
        //    return Json(new { data = Getdata(DateTime.Now.Year + 1 .ToString()), success = true }, JsonRequestBehavior.AllowGet);

        //}

        [HttpPost]
        public ActionResult GetNTID_basedDetails(string Ntid) ////// Employee name is passed instead of NTID
        {
            
            DataTable dt = new DataTable();
            connection();

            string Employee_Name = "", Employee_Number = "", Year = "";
            int Section =0, Department=0 , Group=0 , BU=0 , Level=0  , SAP_Role=0, Product_Area = 0; ;
            string NTID1 = "" ;

            //string Query = " select Employee_Name, Employee_Number, [Section_Table].ID, [DEPT_Table].ID,[Groups_Table_Test].ID, [HC_Role_Table].ID ," + (DateTime.Now.Year + 1) + " from SPOTON_Table_2021 inner join [Section_Table] on Section_Table.Section = SPOTON_Table_2021.Section inner join [DEPT_Table] on DEPT_Table.DEPT = SPOTON_Table_2021.Department inner join [Groups_Table_Test] on Groups_Table_Test.Group = SPOTON_Table_2021.Group inner join [HC_Role_Table] on HC_Role_Table.RoleName = SPOTON_Table_2021.SAP_Role" + " Where NTID = " + Ntid + "  ";
            //string Query = " IF EXISTS(SELECT NTID FROM VKMPlanning_GRMEMSupport Where NTID = '" + Ntid + "')SELECT 0 ELSE select EmployeeName, EmployeeNumber, Level,[Section_Table].ID as Section, [DEPT_Table].ID as Department, [Groups_Table_Test].ID as [Group], [HC_Role_Table].RoleName as [SAP_Role] ,'" + (DateTime.Now.Year + 1) + "' as Year from [SPOTONData_Table_2022]  inner join [Section_Table] on  Section_table.Section=[SPOTONData_Table_2022].Section inner join [DEPT_Table] on DEPT_Table.DEPT = [SPOTONData_Table_2022].Department inner join [Groups_Table_Test] on Groups_Table_Test.[Group] = [SPOTONData_Table_2022].[Group] inner join [HC_Role_Table] on HC_Role_Table.RoleName = [SPOTONData_Table_2022].[Role] " + " Where NTID = '" + Ntid + "' ";
            /////// select year
            ///

            //string Query = " IF EXISTS(SELECT NTID FROM VKMPlanning_GRMEMSupport Where NTID = '" + Ntid + "')SELECT 0 ELSE select EmployeeName, EmployeeNumber, Level,[Section_Table].ID as Section, [DEPT_Table].ID as Department, [Groups_Table_Test].ID as [Group], [HC_Role_Table].ID as [SAP_Role] ,'" + (DateTime.Now.Year + 1) + "' as Year from [SPOTONData_Table_2022]  inner join [Section_Table] on  Section_table.Section=[SPOTONData_Table_2022].Section inner join [DEPT_Table] on DEPT_Table.DEPT = [SPOTONData_Table_2022].Department inner join [Groups_Table_Test] on Groups_Table_Test.[Group] = [SPOTONData_Table_2022].[Group] inner join [HC_Role_Table] on HC_Role_Table.RoleName = [SPOTONData_Table_2022].[Role] " + " Where NTID = '" + Ntid + "' ";

            string Query = " IF EXISTS(SELECT NTID FROM VKMPlanning_GRMEMSupport Where Employee_Name = '" + Ntid + "' and Year = '"+(DateTime.Now.Year + 1)+"')SELECT 0 ELSE select Top 1 NTID,EmployeeName, EmployeeNumber, Level,[Section_Table].ID as Section, [DEPT_Table].ID as Department, [Groups_Table_Test].ID as [Group], [HC_Role_Table].ID as [SAP_Role] ,'" + (DateTime.Now.Year + 1) + "' as Year from [SPOTONData_Table_2022]  inner join [Section_Table] on  Section_table.Section=[SPOTONData_Table_2022].Section inner join [DEPT_Table] on DEPT_Table.DEPT = [SPOTONData_Table_2022].Department inner join [Groups_Table_Test] on Groups_Table_Test.[Group] = [SPOTONData_Table_2022].[Group] inner join [HC_Role_Table] on HC_Role_Table.RoleName = [SPOTONData_Table_2022].[Role] " + " Where [SPOTONData_Table_2022].EmployeeName = '" + Ntid + "' ";


            //CHECK RETURN
            //CHECK CASE WHEN - same table/inner join values chk n return
            try
            {
                BudgetingOpenConnection();
                SqlCommand cmd = new SqlCommand(Query, budgetingcon);
                SqlDataReader dr = cmd.ExecuteReader();
                //if (dr.HasRows)
                if(dr.FieldCount == 1)
                    return Json(new { success = false, msg = "The NTID details are already available in VKM Input Collection list, Please check again!", JsonRequestBehavior.AllowGet });
                else if(dr.HasRows == false)
                    return Json(new { success = false, msg = "", JsonRequestBehavior.AllowGet });
                    //return Json(new { success = false, msg = "The NTID is InValid, Please check again!", JsonRequestBehavior.AllowGet });

                while ( dr.Read())
                {
                    //dr.Read();
                    NTID1 = dr["NTID"].ToString();
                    Employee_Name = dr["EmployeeName"].ToString();
                    Employee_Number = dr["EmployeeNumber"].ToString();
                    Section = Convert.ToInt32(dr["Section"].ToString());
                    Department = Convert.ToInt32(dr["Department"].ToString());
                    Group = Convert.ToInt32(dr["Group"].ToString());
                    //BU = dr["BU"].ToString();
                    SAP_Role = Convert.ToInt32(dr["SAP_Role"].ToString());
                    Year = dr["Year"].ToString();
                    Level = Convert.ToInt32(dr["Level"].ToString());
                }
                dr.Close();

                var User = System.Web.HttpContext.Current.User.Identity.Name.Split('\\')[1].ToUpper();
                
                //connection();
                Query = "IF EXISTS(SELECT Updated_By from VKMPlanning_GRMEMSupport where Updated_By = @User)SELECT TOP 1 Product_Area_ID  from VKMPlanning_GRMEMSupport where Updated_By = @User order by Updated_At desc ELSE SELECT 0";
                cmd = new SqlCommand(Query, budgetingcon);
                cmd.Parameters.AddWithValue("@User ", User);
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    Product_Area = Convert.ToInt32(dr["Product_Area_ID"].ToString());
                }
                dr.Close();
                BudgetingCloseConnection();

            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
            return Json(new {success = true, Level = Level, NTID = NTID1, Employee_Name = Employee_Name, Employee_Number = Employee_Number, Section = Section, Department = Department, Group = Group, BU = BU, SAP_Role = SAP_Role, Year = Year, Product_Area = Product_Area, JsonRequestBehavior.AllowGet });

        }

        [HttpGet]
        public ActionResult Get_IsVKMSpoc()
        {
            DataTable dt = new DataTable();
            try
            {
                string is_VKMSpoc = "", query = "";

                //string presentUserDept = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).Department;
                //string is_CCXC = string.Empty;
                //if (presentUserDept.Contains("XC"))
                //    is_CCXC = "XC";
                //else
                //    is_CCXC = "CC";

                connection();
                BudgetingOpenConnection();
                query = " select VKMspoc as NTID from BU_SPOCS where VKMspoc = '" + User.Identity.Name.Split('\\')[1].ToUpper() + "' and BU in (1,2,3,4,5) union select HOE_NTID as NTID from Planning_HOE_Table where HOE_NTID = '" + User.Identity.Name.Split('\\')[1].ToUpper() + "' union select Proxy_NTID as NTID from Planning_HOE_Table where Proxy_NTID = '" + User.Identity.Name.Split('\\')[1].ToUpper() + "' union select NTID from Spotondata_table_2022 where[Role] = 'SECTION HEAD' and NTID = '" + User.Identity.Name.Split('\\')[1].ToUpper() + "' ";
                //query = " select top 1 VKMSpoc from BU_SPOCS where VKMspoc = '" + User.Identity.Name.Split('\\')[1].ToUpper() + "' and bu in (1,2,3,4,5) ";
                //query = " select top 1 VKMSpoc from BU_SPOCS where VKMspoc = 'din2cob' and bu in (1,2,3,4,5) ";
                SqlCommand cmd = new SqlCommand(query, budgetingcon);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                BudgetingCloseConnection();

                if (dt.Rows.Count > 0)
                {
                    is_VKMSpoc = "1";
                }
                else
                {
                    is_VKMSpoc = "0";
                }

                return Json(new { success = true, data = is_VKMSpoc }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //dr.Close();
                return Json(new { success = false, data = "0" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult GetProductArea_basedonUser()
        {
            var User = System.Web.HttpContext.Current.User.Identity.Name.Split('\\')[1].ToUpper();
            string Query = "";
            int Product_Area = 0;
            connection();
            Query = "IF EXISTS(SELECT Updated_By from VKMPlanning_GRMEMSupport where Updated_By = @User)SELECT TOP 1 Product_Area_ID  from VKMPlanning_GRMEMSupport where Updated_By = @User order by Updated_At desc ELSE SELECT 0";
            BudgetingOpenConnection();
            SqlCommand cmd = new SqlCommand(Query, budgetingcon);
            cmd.Parameters.AddWithValue("@User ", User);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                dr.Read();
                Product_Area = Convert.ToInt32(dr["Product_Area_ID"].ToString());
                BudgetingCloseConnection();
                if (Product_Area == 0)
                    return Json(new { success = false }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { success = true, data = Product_Area }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);

        }

        //[HttpPost]
        //public ActionResult GetPYO(decimal Plan_Sum)
        //{

        //   decimal PYO = 0;
        //    if (Plan_Sum > 0)
        //        PYO = Plan_Sum / 12;
        //    else
        //        PYO = 0;
        //    return Json(new { PYO = PYO, JsonRequestBehavior.AllowGet });

        //}

        [HttpPost]
        public ActionResult GetPYO_PlanSum(string Jan, string Feb, string Mar, string Apr, string May, string Jun, string Jul, string Aug, string Sep, string Oct, string Nov, string Dec)
        {

            decimal PYO , Plan_Sum = 0;
            if (Jan != null && Jan.Trim() != "")
                Plan_Sum += decimal.Parse(Jan);
            if (Feb != null && Feb.Trim() != "")
                Plan_Sum += decimal.Parse(Feb);
            if (Mar != null && Mar.Trim() != "")
                Plan_Sum += decimal.Parse(Mar);
            if (Apr != null && Apr.Trim() != "")
                Plan_Sum += decimal.Parse(Apr);
            if (May != null && May.Trim() != "")
                Plan_Sum += decimal.Parse(May);
            if (Jun != null && Jun.Trim() != "")
                Plan_Sum += decimal.Parse(Jun);
            if (Jul != null && Jul.Trim() != "")
                Plan_Sum += decimal.Parse(Jul);
            if (Aug != null && Aug.Trim() != "")
                Plan_Sum += decimal.Parse(Aug);
            if (Sep != null && Sep.Trim() != "")
                Plan_Sum += decimal.Parse(Sep);
            if (Oct != null && Oct.Trim() != "")
                Plan_Sum += decimal.Parse(Oct);
            if (Nov != null && Nov.Trim() != "")
                Plan_Sum += decimal.Parse(Nov);
            if (Dec != null && Dec.Trim() != "")
                Plan_Sum += decimal.Parse(Dec);

            if (Plan_Sum > 0)
                PYO = (Plan_Sum / 12);
            else
                PYO = 0;
            return Json(new {  Plan_Sum = Plan_Sum ,PYO = PYO, JsonRequestBehavior.AllowGet });

        }


        [HttpGet]
        public ActionResult Lookup_GRMEM()
        {
            var obj = "";
            DataSet dt = new DataSet();

            try
            {
                //create instanace of database connection
                // SqlConnection conn = new SqlConnection(connString);
                //product_area_list =
                //section_list = JSON.
                //department_area_list
                //group_list = JSON.pa
                //sap_role_list1 = JSO
                //vkm_role_list1 = JSO
                connection();
                dt = new DataSet();
                //conn.Open();
                BudgetingOpenConnection();
                //string Query = "Select * from [BU_Table]; Select * from [Section_Table];Select * from [DEPT_Table]; Select * from [Groups_Table_Test]; Select * from [HC_Role_Table]; Select * from [HC_Skillset_Table]; select distinct NTID , EmployeeName  as Employee_Name from [SPOTONData_Table_2022] where BU = 'MS/BE1' order by EmployeeName ";
                string Query = "Select * from [BU_Table]; Select * from [Section_Table];Select * from [DEPT_Table]; Select * from [Groups_Table_Test]; Select * from [HC_Role_Table]; Select * from [HC_Skillset_Table] where (isdeleted is null or isDeleted = 0); select distinct NTID , EmployeeName  as Employee_Name from [SPOTONData_Table_2022] where BU = 'MS/BE1' order by EmployeeName ";

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
            //BU lookup
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

            JsonResult result_bu = Json(result);
            result_bu.MaxJsonLength = Int32.MaxValue;
            result_bu.JsonRequestBehavior = JsonRequestBehavior.AllowGet;


            //Section lookup
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

            JsonResult result_section = Json(result1);
            result_section.MaxJsonLength = Int32.MaxValue;
            result_section.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            //dept
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

            JsonResult result_dept = Json(result2);
            result_dept.MaxJsonLength = Int32.MaxValue;
            result_dept.JsonRequestBehavior = JsonRequestBehavior.AllowGet;


            //grp lookup
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

            JsonResult result_group = Json(result3);
            result_group.MaxJsonLength = Int32.MaxValue;
            result_group.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            //sp role lookup
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

            JsonResult result_sprole = Json(result4);
            result_sprole.MaxJsonLength = Int32.MaxValue;
            result_sprole.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            //vkm role lookup
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

            JsonResult result_vkmrole = Json(result5);
            result_vkmrole.MaxJsonLength = Int32.MaxValue;
            result_vkmrole.JsonRequestBehavior = JsonRequestBehavior.AllowGet;


            //spoton data lookup
            List<Dictionary<string, object>> parentRow6 = new List<Dictionary<string, object>>();
            Dictionary<string, object> childRow6;
            //foreach (DataRow row in dt.Rows)
            foreach (DataRow row in dt.Tables[6].Rows)
            {

                childRow6 = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Tables[6].Columns)
                {
                    childRow6.Add(col.ColumnName, row[col]);
                }
                parentRow6.Add(childRow6);
            }

            jsSerializer.MaxJsonLength = Int32.MaxValue;

            var resultData6 = parentRow6;
            var result6 = new ContentResult
            {
                Content = jsSerializer.Serialize(resultData6),
                ContentType = "application/json"
            };

            JsonResult result_spemp = Json(result6);
            result_spemp.MaxJsonLength = Int32.MaxValue;
            result_spemp.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            return Json(new { data_bu = result_bu, data_section = result_section, data_dept = result_dept, data_grp = result_group, data_saprole = result_sprole, data_vkmrole = result_vkmrole, data_spemp = result_spemp }, JsonRequestBehavior.AllowGet);
        }


        public class GRMEMSupportView
        {
            public int SNo { get; set; }
            public int Year { get; set; }
            public string NTID { get; set; }
            public string Employee_Number { get; set; }
            public string Employee_Name { get; set; }
            public string Product_Area { get; set; }
            public string Section { get; set; }
            public string Department { get; set; }
            public string Group { get; set; }
            public string Level { get; set; }
            public string SAP_Role { get; set; }
            public List<string> VKM_Role { get; set; }
            public string Remarks { get; set; }
            public decimal Plan_Sum { get; set; }
            public decimal Jan { get; set; }
            public decimal Feb { get; set; }
            public decimal Mar { get; set; }
            public decimal Apr { get; set; }
            public decimal May { get; set; }
            public decimal Jun { get; set; }
            public decimal Jul { get; set; }
            public decimal Aug { get; set; }
            public decimal Sep { get; set; }
            public decimal Oct { get; set; }
            public decimal Nov { get; set; }
            public decimal Dec { get; set; }
            public decimal PYO { get; set; }
            public string Updated_By { get; set; }
            public DateTime Updated_At { get; set; }
        }
    }
}