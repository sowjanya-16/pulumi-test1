using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LC_Reports_V1.Models;

namespace LC_Reports_V1.Controllers
{
    public class InventoryLabController : Controller
    {
        // GET: InventoryLab_
        //Loading the Inventory Page

        private static SqlConnection con, lcdetails_con;

        public static List<LCInventory_Table> labcardetails = new List<LCInventory_Table>();
        public ActionResult LabInventory()
        {
            InitialiseLabCarDetails();
            return View();
        }

        //function to check authorization for particular users
        public ActionResult checkAuth()
        {
            string NTID = "";
            string todelete = "";
            string tomodify = "";
            string islablnventory = "";
            string ispmtlnventory = "";

            connection();
            //to check the NTID
            string qry = " Select isnull(NTID,'') as NTID,toDelete,toModify,isLabInventory,isPMTInventory from Inventory_Authority where NTID = '" + User.Identity.Name.Split('\\')[1].Trim() + "'";
            //  string qry = " Select  [ID]" +
            //",[NTID] " +
            //"from [Inventory_Authority] order by ID";
            OpenConnection();
            SqlCommand command = new SqlCommand(qry, conn);
            SqlDataReader dr = command.ExecuteReader();
            if (dr.HasRows)
            {
                dr.Read();
                NTID = dr["NTID"].ToString();
                //delete and modify flags
                todelete = dr["toDelete"].ToString();
                tomodify = dr["toModify"].ToString();
                islablnventory = dr["isLabInventory"].ToString();
                ispmtlnventory = dr["isPMTInventory"].ToString();


                var checkflag = "";
                //var labflag = "";
                //var pmtflag = "";

                //if (islablnventory == "1" )
                //{
                //    labflag = "1";
                //}
                //else
                //{
                //    labflag = "0";
                ////}
                //if (todelete == "1" && tomodify == "1")
                //{
                //    checkflag = "1";
                //}
                //else if (todelete == "0" && tomodify == "1")
                //{
                //    checkflag = "2";
                //}
                return Json(new
                {
                    data = todelete,tomodify,checkflag,islablnventory,ispmtlnventory,
                    success = true,
                }, JsonRequestBehavior.AllowGet);

            }
            else
            {
                NTID = "";
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
            dr.Close();
            CloseConnection();

        }

        private SqlConnection conn;

        private void connection()
        {

            string connString = ConfigurationManager.ConnectionStrings["BudgetingdbConnection"].ToString();
            conn = new SqlConnection(connString);

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
        //To Get data and display in Hardware inventory
        public ActionResult GetHWData()
        {

            List<HardwareInventory> viewList = new List<HardwareInventory>();
            viewList = GetHWData1();
            return Json(new
            {
                success = true,
                data = viewList
            }, JsonRequestBehavior.AllowGet);
        }

        //List for storing the retrieved data to display
        public List<HardwareInventory> GetHWData1()
        {
            List<HardwareInventory> viewList = new List<HardwareInventory>();

            DataTable dt = new DataTable();
            try
            {

                connection();

                string Query = " Select  [ID]" +
          ",[BU] " +
          ",[OEM]" +
          ",[GROUP]" +
          ",[ItemName_Planner]" +
          ",[Item Name]" +
          ",[UOM]" +
          ",[Inventory Type]" +
          ",[Serial Number]" +
          ",[Bond Number]" +
          ",[Bond Date]" +
          ",[Asset Number]" +
          ",[HW Responsible]" +
          ",[Handover To]" +
          ",[Actual Delivery Date]" +
          ",[Mode]" +
          ",[Remarks]" +
          ",[ALM Number]" +
          ",[POQty]" +
          ",[Quantity]" +
          ",[Available Qty]" +
          "from [HWInventory_Table] order by ID";

                OpenConnection();
                SqlCommand cmd = new SqlCommand(Query, conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                CloseConnection();
            }
            catch (Exception e)
            {

            }

            foreach (DataRow row in dt.Rows)
            {
                HardwareInventory item = new HardwareInventory();
                item.ID = int.Parse(row["ID"].ToString());
                item.BU = int.Parse(row["BU"].ToString());
                item.OEM = int.Parse(row["OEM"].ToString());
                item.Group = int.Parse(row["Group"].ToString());
                item.ItemName_Planner = int.Parse(row["ItemName_Planner"].ToString());
                item.ItemName = row["Item Name"].ToString();
                if (row["Inventory Type"].ToString() != "")
                    item.InventoryType = Convert.ToInt32(row["Inventory Type"]);
                if (row["Inventory Type"].ToString() != "")
                    item.InventoryType2 = Convert.ToInt32(row["Inventory Type"]);
                item.SerialNo = row["Serial Number"].ToString();
                item.BondNo = row["Bond Number"].ToString();
                item.BondDate = row["Bond Date"].ToString() != "" ? ((DateTime)row["Bond Date"]).ToString("yyyy-MM-dd") : string.Empty;
                item.UOM = row["UOM"].ToString();
                item.AssetNo = row["Asset Number"].ToString();
                item.HardwareResponsible = row["HW Responsible"].ToString();
                item.HandoverTo = row["Handover To"].ToString();
                if (row["Mode"].ToString() != "")
                    item.Mode = Convert.ToInt32(row["Mode"]);
                //item.Mode = int.Parse(row["Mode"].ToString());
                item.Remarks = row["Remarks"].ToString();
                item.ALMNo = row["ALM Number"].ToString();
                item.POQty = int.Parse(row["POQty"].ToString());
                if (row["Quantity"].ToString() != "")
                    item.Quantity = Convert.ToInt32(row["Quantity"]);
                item.AvailableQty = row["Available Qty"].ToString();
                item.ActualDeliveryDate = row["Actual Delivery Date"].ToString() != "" ? ((DateTime)row["Actual Delivery Date"]).ToString("yyyy-MM-dd") : string.Empty;

                viewList.Add(item);
            }
            return viewList;
        }


        //Result for getting data from Mode table
        public ActionResult GetMode()
        {
            List<ModeHWInventory_class> modeList = new List<ModeHWInventory_class>();
            DataTable dt = new DataTable();
            try
            {

                connection();

                string Query = " Select  [ID]" +
          ",[Mode] " +
          "from [ModeHWInventory] order by ID";

                OpenConnection();
                SqlCommand cmd = new SqlCommand(Query, conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                CloseConnection();
            }
            catch (Exception e)
            {

            }
            foreach (DataRow row in dt.Rows)
            {
                ModeHWInventory_class item = new ModeHWInventory_class();
                item.ID = int.Parse(row["ID"].ToString());
                item.Mode = row["Mode"].ToString();

                modeList.Add(item);
            }


            return Json(new { data = modeList }, JsonRequestBehavior.AllowGet);
        }


        //Action Result for saving the details after adding or editing
        [HttpPost]
        public ActionResult AddOrEdit(HardwareInventory req)
        {


            DataTable dt = new DataTable();

            connection();
            dt = new DataTable();
            string Query = "";
            System.Security.Principal.IPrincipal User = System.Web.HttpContext.Current.User;


            //Update Query
            if (req.ID != 0)
            {

                Query = "UPDATE [HWInventory_Table] SET " +

                        "[BU]= @BU," +
                        "[OEM]= @OEM," +
                        "[GROUP]= @GROUP," +
                        "[Item Name]= @ItemName," +
                        "[ItemName_Planner]= @ItemName_Planner," +
                        "[UOM]= @UOM," +
                        "[Inventory Type]= @InventoryType," +
                        //"[Inventory Type]= @InventoryType2," +
                        "[Serial Number] = @SerialNo," +
                        "[Bond Number]= @BondNo," +
                        "[Bond Date] = @BondDate," +
                        "[Asset Number]= @AssetNo," +
                        "[HW Responsible] = @HWResponsible," +
                        "[Handover To]= @HandoverTo," +
                        "[Actual Delivery Date]= @ActualDeliveryDate," +
                        "[Mode] = @Mode," +
                        "[Remarks]= @Remarks," +
                        "[ALM Number]= @ALMNo," +
                        "[POQty]= @POQty, " +
                        "[Quantity]= @Quantity " +
                        //"[Available Qty] = @AvailableQty " +
                        "WHERE ID = @ID";

            }
            else
            {

                //Insert query 
                Query = "INSERT into[HWInventory_Table]" +
                          "(" +
                           "[BU]" +
                           ",[OEM]" +
                           ",[GROUP]" +
                           ",[Item Name]" +
                           ",[ItemName_Planner]" +
                           ",[Inventory Type]" +
                           ",[Serial Number]" +
                           ",[Bond Number]" +
                           ",[Bond Date]" +
                           ",[Asset Number]" +
                           ",[HW Responsible]" +
                           ",[Handover To]" +
                           ",[UOM]" +
                           ",[POQty]" +
                           ",[Quantity]" +
                           //",[Available Qty]" +
                           ",[Remarks]" +
                           ",[ALM Number]" +
                           ",[Mode]" +
                           ",[Actual Delivery Date]" +
                           ")" +
                           "values" +
                           "(" +
                           " @BU  " +
                           ", @OEM  " +
                           ", @Group" +
                           ", @ItemName" +
                           ", @ItemName_Planner " +
                           ", @InventoryType " +
                           ", @SerialNo " +
                           ", @BondNo " +
                           ", @BondDate " +
                           ", @AssetNo " +
                           ", @HWResponsible" +
                           ", @HandoverTo" +
                           ", @UOM" +
                           ", @POQty" +
                           ", @Quantity" +
                           //", @AvailableQty" +
                           ", @Remarks" +
                           ", @ALMNo" +
                           ", @Mode" +
                           ",@ActualDeliveryDate" +
                           ")";

            }


            SqlCommand cmd = new SqlCommand(Query, conn);
            cmd.Parameters.AddWithValue("@BU", req.BU);
            cmd.Parameters.AddWithValue("@OEM", req.OEM);
            cmd.Parameters.AddWithValue("@GROUP", req.Group);
            cmd.Parameters.AddWithValue("@ItemName", req.ItemName);
            //if (req.ItemName_Planner == 0)
            //{
            //    cmd.Parameters.AddWithValue("@ItemName_Planner", DBNull.Value);
            //}
            //else
            if (req.ItemName_Planner == 0)
                cmd.Parameters.AddWithValue("@ItemName_Planner", 0);
            else
                cmd.Parameters.AddWithValue("@ItemName_Planner", req.ItemName_Planner);

            cmd.Parameters.AddWithValue("@InventoryType", req.InventoryType);
            cmd.Parameters.AddWithValue("@SerialNo", req.SerialNo);
            cmd.Parameters.AddWithValue("@BondNo", req.BondNo);
            cmd.Parameters.AddWithValue("@BondDate", req.BondDate);
            cmd.Parameters.AddWithValue("@AssetNo", req.AssetNo);
            cmd.Parameters.AddWithValue("@HWResponsible", req.HardwareResponsible);
            cmd.Parameters.AddWithValue("@HandoverTo", req.HandoverTo);
            cmd.Parameters.AddWithValue("@UOM", req.UOM);
            cmd.Parameters.AddWithValue("@POQty", req.POQty);
            cmd.Parameters.AddWithValue("@Quantity", req.Quantity);
            //cmd.Parameters.AddWithValue("@AvailableQty", req.AvailableQty);
            cmd.Parameters.AddWithValue("@Remarks", req.Remarks);
            cmd.Parameters.AddWithValue("@ALMNo", req.ALMNo);
            cmd.Parameters.AddWithValue("@ActualDeliveryDate", req.ActualDeliveryDate);
            cmd.Parameters.AddWithValue("@Mode", req.Mode);
            cmd.Parameters.AddWithValue("@ID", req.ID);

            try
            {
                OpenConnection();
                cmd.ExecuteNonQuery();
                Console.WriteLine("Records Inserted Successfully");
                return Json(new { data = GetHWData(), success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // alert()
                // MessageBox.Show(" Not Updated");
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
            finally
            {
                CloseConnection();
            }

            //} 

        }

        //delete function
        [HttpPost]
        public ActionResult Delete(int id)
        {
            DataTable dt = new DataTable();

            connection();

            dt = new DataTable();
            string Query = "";
            System.Security.Principal.IPrincipal User = System.Web.HttpContext.Current.User;

            Query = "Delete from [HWInventory_Table] where ID = @Id";


            SqlCommand cmd = new SqlCommand(Query, conn);
            cmd.Parameters.AddWithValue("@Id", id);
            try
            {
                OpenConnection();
                cmd.ExecuteNonQuery();
                return Json(new { data = GetHWData(), success = true }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
            finally
            {
                CloseConnection();

            }


        }
        //Creating Lookup list and getting data from various tables in the Lookup 
        public ActionResult Lookup()
        {
            using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())
            {

                DataSet ds = new DataSet();
                connection();
                OpenConnection();
                string Query = " Select * from BU_Table; Select * from OEM_Table; Select * from Groups_Table_Test; Select * from ItemsCostList_Table; Select * from Currency_Table; ";
                SqlCommand cmd = new SqlCommand(Query, conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                CloseConnection();

                Inventory_LookupData lookupData = new Inventory_LookupData();
                //lookupData.BU_List = db.BU_Table.ToList();
                //lookupData.OEM_List = db.OEM_Table.ToList();
                ////lookupData.DEPT_List = db.DEPT_Table.ToList();
                //lookupData.Groups_test = db.Groups_Table_Test.ToList();
                //lookupData.Item_List = db.ItemsCostList_Table.ToList();
                //lookupData.Currency_List = db.Currency_Table.ToList();

                lookupData.BU_List = new List<BU_Table>();
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    BU_Table item = new BU_Table();
                    item.ID = Convert.ToInt32(row["ID"]);
                    item.BU = row["BU"].ToString();
                    lookupData.BU_List.Add(item);
                }

                lookupData.OEM_List = new List<OEM_Table>();
                foreach (DataRow row in ds.Tables[1].Rows)
                {
                    OEM_Table item = new OEM_Table();
                    item.ID = Convert.ToInt32(row["ID"]);
                    item.OEM = row["OEM"].ToString();
                    lookupData.OEM_List.Add(item);
                }

                lookupData.Groups_test = new List<Groups_Table_Test>();
                foreach (DataRow row in ds.Tables[2].Rows)
                {
                    Groups_Table_Test item = new Groups_Table_Test();
                    item.ID = Convert.ToInt32(row["ID"]);
                    item.Group = row["Group"].ToString();
                    item.Dept = Convert.ToInt32(row["Dept"]);
                    item.Outdated = Convert.ToBoolean(row["Outdated"]);
                    item.Rep_ID = row["Rep_ID"].ToString() != "" ? Convert.ToInt32(row["Rep_ID"]) : 0;
                    lookupData.Groups_test.Add(item);
                }


                lookupData.Item_List = new List<ItemsCostList_Table>();
                foreach (DataRow row in ds.Tables[3].Rows)
                {
                    ItemsCostList_Table item = new ItemsCostList_Table();
                    item.S_No = Convert.ToInt32(row["S#No"].ToString());
                    item.Item_Name = row["Item Name"].ToString();
                    item.Category = row["Category"].ToString();
                    item.Cost_Element = row["Cost Element"].ToString();
                    if (row["Unit Price"].ToString() != "")
                        item.Unit_Price = Convert.ToDouble(row["Unit Price"].ToString());
                    item.Currency = row["Currency"].ToString();
                    item.Comments = row["Comments"].ToString();
                    item.RequestorNT = row["RequestorNT"].ToString();
                    if (row["UnitPriceUSD"].ToString() != "")
                        item.UnitPriceUSD = Convert.ToDouble(row["UnitPriceUSD"].ToString());
                    item.Deleted = Convert.ToBoolean(row["Deleted"].ToString());
                    item.VendorCategory = row["VendorCategory"].ToString();
                    item.BU = row["BU"].ToString() != "" ? Convert.ToInt32(row["BU"].ToString()) : 0;
                    item.Actual_Available_Quantity = row["Actual Available Quantity"].ToString();
                    item.Repairable_Cost = row["Repairable_Cost"].ToString() != "" ? Convert.ToDouble(row["Repairable_Cost"].ToString()) : 0;
                    item.Repair_Currency = row["Repair_Currency"].ToString();
                    item.Repairable_Cost_EUR = row["Repairable_Cost"].ToString() != "" ? Convert.ToDouble(row["Repairable_Cost_EUR"].ToString()) : 0;
                    if (row["Repairable_Cost"].ToString() != "")
                        item.Repair_UpdatedAt = Convert.ToDateTime(row["Repair_UpdatedAt"].ToString());
                    item.Repair_UpdatedBy = row["Repair_UpdatedBy"].ToString();
                    if (row["UpdatedAt"].ToString() != "")
                        item.UpdatedAt = Convert.ToDateTime(row["UpdatedAt"].ToString());
                    item.VKM_Year = Convert.ToInt32(row["VKM_Year"].ToString());
                    lookupData.Item_List.Add(item);
                }

                lookupData.Currency_List = new List<Currency_Table>();
                foreach (DataRow row in ds.Tables[4].Rows)
                {
                    Currency_Table item = new Currency_Table();
                    item.ID = Convert.ToInt32(row["ID"]);
                    item.Currency = row["Currency"].ToString();
                    lookupData.Currency_List.Add(item);
                }

                //Inventory_LookupData lookupData = new Inventory_LookupData();



                DataSet dt_for_headerRow = new DataSet();
                connection();
                OpenConnection();
                Query = " Exec [dbo].[GetInventoryHeaderFilterDetails] ";
                cmd = new SqlCommand(Query, conn);
                da = new SqlDataAdapter(cmd);
                da.Fill(dt_for_headerRow);
                CloseConnection();
                lookupData.Item_HeaderFilter = new List<Inventory_HeaderFilter_Table>();
                lookupData.Group_HeaderFilter = new List<Inventory_HeaderFilter_Table>();
                lookupData.OEM_HeaderFilter = new List<Inventory_HeaderFilter_Table>();
                lookupData.InventoryType_HeaderFilter = new List<Inventory_HeaderFilter_Table>();

                lookupData.SpareHW_HeaderFilter = new List<Inventory_HeaderFilter_Table>();

                //lookupData.BU_HeaderFilter = new List<HeaderFilter_Table>();
                //lookupData.Mode_HeaderFilter = new List<HeaderFilter_Table>();



                foreach (DataRow item in dt_for_headerRow.Tables[0].Rows)
                {
                    Inventory_HeaderFilter_Table i_header = new Inventory_HeaderFilter_Table();
                    i_header.text = item["OEM"].ToString();
                    i_header.value = Convert.ToInt32(item["ID"].ToString());
                    lookupData.OEM_HeaderFilter.Add(i_header);
                }
                foreach (DataRow item in dt_for_headerRow.Tables[1].Rows)
                {
                    Inventory_HeaderFilter_Table i_header = new Inventory_HeaderFilter_Table();
                    i_header.text = item["Item Name"].ToString();
                    i_header.value = Convert.ToInt32(item["S#No"].ToString());
                    lookupData.Item_HeaderFilter.Add(i_header);
                }

                foreach (DataRow item in dt_for_headerRow.Tables[2].Rows)
                {
                    Inventory_HeaderFilter_Table i_header = new Inventory_HeaderFilter_Table();
                    i_header.text = item["Group"].ToString();
                    i_header.value = Convert.ToInt32(item["ID"].ToString());
                    lookupData.Group_HeaderFilter.Add(i_header);
                }

                foreach (DataRow item in dt_for_headerRow.Tables[3].Rows)
                {
                    Inventory_HeaderFilter_Table i_header = new Inventory_HeaderFilter_Table();
                    i_header.text = item["Item Name"].ToString();
                    i_header.value = Convert.ToInt32(item["S#No"].ToString());
                    lookupData.InventoryType_HeaderFilter.Add(i_header);
                }

                foreach (DataRow item in dt_for_headerRow.Tables[4].Rows)
                {
                    Inventory_HeaderFilter_Table i_header = new Inventory_HeaderFilter_Table();
                    i_header.text = item["Item Name"].ToString();
                    i_header.value = Convert.ToInt32(item["S#No"].ToString());
                    lookupData.SpareHW_HeaderFilter.Add(i_header);
                }

                return Json(new { data = lookupData }, JsonRequestBehavior.AllowGet);

            }
        }
        //function to get the spare data from table
        public ActionResult GetSpareData()
        {

            List<SpareInventory> viewList = new List<SpareInventory>();
            viewList = GetSpareData1();
            return Json(new
            {
                success = true,
                data = viewList
            }, JsonRequestBehavior.AllowGet);
        }

        //List for storing the retrieved data to display
        public List<SpareInventory> GetSpareData1()
        {
            List<SpareInventory> viewList = new List<SpareInventory>();

            DataTable dt = new DataTable();
            try
            {

                connection();

                string Query = " Select  [ID]" +
          ",[SpareHW]" +
          ",[BANQty]" +
          ",[COBQty]" +
          ",[TotalQty]" +
          ",[SpareCalc]" +
           ",[BANreqd]" +
          ",[COBreqd]" +
          ",[BANUnderRepair]" +
          ",[COBUnderRepair]" +
          ",[BANdiff]" +
          ",[COBdiff]" +
          ",[Status]" +
          ",[PriceOriginal]" +
          ",[Currency]" +
          ",[PriceUSD]" +
          ",[POQty]" +
          ",[BANTotalPrice]" +
          ",[COBTotalPrice]" +
       "from [SpareInventory_Table] order by ID";

                OpenConnection();
                SqlCommand cmd = new SqlCommand(Query, conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                CloseConnection();
            }
            catch (Exception e)
            {

            }

            foreach (DataRow row in dt.Rows)
            {
                SpareInventory item = new SpareInventory();
                item.ID = int.Parse(row["ID"].ToString());
                item.SpareHW = int.Parse(row["SpareHW"].ToString());
                item.BANQty = row["BANQty"].ToString();
                item.COBQty = row["COBQty"].ToString();
                item.TotalQty = row["TotalQty"].ToString();
                item.SpareCalc = row["SpareCalc"].ToString();
                item.BANreqd = row["BANreqd"].ToString();
                item.COBreqd = row["COBreqd"].ToString();
                item.BANUnderRepair = row["BANUnderRepair"].ToString();
                item.COBUnderRepair = row["COBUnderRepair"].ToString();
                item.BANdiff = row["BANdiff"].ToString();
                item.COBdiff = row["COBdiff"].ToString();
                item.Status = row["Status"].ToString();
                item.PriceOriginal = row["PriceOriginal"].ToString();
                item.Currency = int.Parse(row["Currency"].ToString());
                item.PriceUSD = row["PriceUSD"].ToString();
                //item.POQty = int.Parse(row["POQty"].ToString());
                item.BANTotalPrice = row["BANTotalPrice"].ToString();
                item.COBTotalPrice = row["COBTotalPrice"].ToString();


                viewList.Add(item);
            }
            return viewList;
        }
        public ActionResult GetSpareConfig()
        {
            List<SpareConfiguration> viewList = new List<SpareConfiguration>();

            DataTable dt = new DataTable();
            try
            {

                connection();

                string Query = " Select  [ID]" +
          ",[SpareHW]" +
          ",[SpareCount]" +
          ",[HWCount]" +
          ",[MultiplicationFactor]" +
          ",[SpareCalc]" +
       "from [Spare_Configuration_Table] order by ID";

                OpenConnection();
                SqlCommand cmd = new SqlCommand(Query, conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                CloseConnection();
            }
            catch (Exception e)
            {

            }

            foreach (DataRow row in dt.Rows)
            {
                SpareConfiguration item = new SpareConfiguration();
                item.ID = int.Parse(row["ID"].ToString());
                if (row["SpareHW"].ToString() != "")
                    item.SpareHW = Convert.ToInt32(row["SpareHW"]);
                item.SpareCount = row["SpareCount"].ToString();
                item.HWCount = row["HWCount"].ToString();
                item.MultiplicationFactor = row["MultiplicationFactor"].ToString();
                item.SpareCalc = row["SpareCalc"].ToString();

                viewList.Add(item);
            }
            return Json(new { data = viewList }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult AutoUpdateSpare(int sphw, string spcal)
        {
            connection();
            SpareInventory req = new SpareInventory();
            string Query = "IF EXISTS (Select * from SpareInventory_Table where SpareHW = @SpareHW)Select BANQty,COBQty,TotalQty,PriceOriginal,Currency,BANUnderRepair,COBUnderRepair,Status,Currency,PriceUSD from SpareInventory_Table where SpareHW = @SpareHW";
            //        connection();
            //        OpenConnection();
            //        SqlCommand cmd1 = new SqlCommand(Query1, conn);
            OpenConnection();
            SqlCommand cmd1 = new SqlCommand(Query, conn);
            cmd1.Parameters.AddWithValue("@SpareHW ", sphw);
            cmd1.Parameters.AddWithValue("@SpareCalc", spcal);
            SqlDataReader dr = cmd1.ExecuteReader();

            if (dr.HasRows)
            {
                dr.Read();
                req.BANQty = (dr["BANQty"].ToString());
                req.COBQty = dr["COBQty"].ToString();
                req.TotalQty = dr["TotalQty"].ToString();
                req.PriceOriginal = dr["PriceOriginal"].ToString();
                req.Currency = int.Parse(dr["Currency"].ToString());
                req.BANUnderRepair = dr["BANUnderRepair"].ToString();
                req.COBUnderRepair = dr["COBUnderRepair"].ToString();
                req.PriceUSD = dr["PriceUSD"].ToString();
                req.Status = dr["Status"].ToString();


                CloseConnection();

            }
            else
            {
                return Json(new { data = GetSpareData(), success = true }, JsonRequestBehavior.AllowGet);
            }
            string Query1 = "UPDATE [SpareInventory_Table] SET " +

                        "[SpareHW]= @SpareHW," +
                        "[BANQty]= @BANQty, " +
                        "[COBQty]= @COBQty, " +
                        "[TotalQty]= @TotalQty, " +
                        "[SpareCalc]= @SpareCalc, " +
                        "[BANreqd]= @BANreqd, " +
                        "[COBreqd]= @COBreqd, " +
                        "[BANUnderRepair]= @BANUnderRepair, " +
                        "[COBUnderRepair]= @COBUnderRepair, " +
                        "[BANdiff]= @BANdiff," +
                        "[COBdiff]= @COBdiff," +
                        "[Status]= @Status," +
                        "[PriceOriginal]= @PriceOriginal," +
                        "[Currency]= @Currency," +
                        "[PriceUSD]= @PriceUSD," +
                        "[BANTotalPrice]= @BANTotalPrice," +
                        "[COBTotalPrice]= @COBTotalPrice " +
                            " WHERE SpareHW = @SpareHW";


            SqlCommand cmd = new SqlCommand(Query1, conn);
            req.SpareHW = sphw;
            cmd.Parameters.AddWithValue("@SpareHW", req.SpareHW);
            cmd.Parameters.AddWithValue("@BANQty", req.BANQty);
            cmd.Parameters.AddWithValue("@COBQty", req.COBQty);
            cmd.Parameters.AddWithValue("@TotalQty", req.TotalQty);
            req.SpareCalc = spcal;
            cmd.Parameters.AddWithValue("@SpareCalc", req.SpareCalc);

            var scal = float.Parse(req.SpareCalc);
            var res1 = Math.Round(scal * 40);
            var res2 = Math.Round(scal * 50);
            req.BANreqd = res1.ToString();
            req.COBreqd = res2.ToString();

            cmd.Parameters.AddWithValue("@BANreqd", req.BANreqd);
            cmd.Parameters.AddWithValue("@COBreqd", req.COBreqd);
            cmd.Parameters.AddWithValue("@BANUnderRepair", req.BANUnderRepair);
            cmd.Parameters.AddWithValue("@COBUnderRepair", req.COBUnderRepair);
            var diff1 = res1 - int.Parse(req.BANQty);
            req.BANdiff = diff1.ToString();
            var diff2 = res2 - int.Parse(req.COBQty);
            req.COBdiff = diff2.ToString();
            cmd.Parameters.AddWithValue("@BANdiff", req.BANdiff);
            cmd.Parameters.AddWithValue("@COBdiff", req.COBdiff);

            cmd.Parameters.AddWithValue("@Status", req.Status);

            cmd.Parameters.AddWithValue("@PriceOriginal", req.PriceOriginal);
            cmd.Parameters.AddWithValue("@Currency", req.Currency);
            cmd.Parameters.AddWithValue("@PriceUSD", req.PriceUSD);
            var psud = float.Parse(req.PriceUSD);
            var tpban = psud * diff1;
            var tpcob = psud * diff2;
            cmd.Parameters.AddWithValue("@BANTotalPrice", tpban.ToString("0.00"));
            cmd.Parameters.AddWithValue("@COBTotalPrice", tpcob.ToString("0.00"));

            try
            {
                OpenConnection();
                cmd.ExecuteNonQuery();
                Console.WriteLine("Records Inserted Successfully");
                return Json(new { data = GetSpareData(), success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // alert()
                // MessageBox.Show(" Not Updated");
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
            finally
            {
                CloseConnection();
            }


        }

        [HttpPost]
        public ActionResult AddOrEditSpare(SpareInventory req)
        {
            connection();

            SqlCommand command = new SqlCommand();
            //Stored procedure specification
            command.Connection = conn;
            command.CommandText = "dbo.[ToInsertinSpare]";
            command.CommandType = CommandType.StoredProcedure;


            // Add the input parameter and set its properties.

            SqlParameter parameter1 = new SqlParameter();
            parameter1.ParameterName = "@SpareHW";
            parameter1.SqlDbType = SqlDbType.NVarChar;
            parameter1.Direction = ParameterDirection.Input;
            parameter1.Value = req.SpareHW;

            SqlParameter parameter2 = new SqlParameter();
            parameter2.ParameterName = "@BANQty";
            parameter2.SqlDbType = SqlDbType.NVarChar;
            parameter2.Direction = ParameterDirection.Input;
            parameter2.Value = req.BANQty;

            SqlParameter parameter3 = new SqlParameter();
            parameter3.ParameterName = "@COBQty";
            parameter3.SqlDbType = SqlDbType.NVarChar;
            parameter3.Direction = ParameterDirection.Input;
            parameter3.Value = req.COBQty;

            SqlParameter parameter4 = new SqlParameter();
            parameter4.ParameterName = "@TotalQty";
            parameter4.SqlDbType = SqlDbType.NVarChar;
            parameter4.Direction = ParameterDirection.Input;
            parameter4.Value = req.TotalQty;

            SqlParameter parameter5 = new SqlParameter();
            parameter5.ParameterName = "@SpareCalc";
            parameter5.SqlDbType = SqlDbType.NVarChar;
            parameter5.Direction = ParameterDirection.Input;
            parameter5.Value = req.SpareCalc;

            SqlParameter parameter6 = new SqlParameter();
            parameter6.ParameterName = "@BANreqd";
            parameter6.SqlDbType = SqlDbType.NVarChar;
            parameter6.Direction = ParameterDirection.Input;
            parameter6.Value = req.BANreqd;

            SqlParameter parameter7 = new SqlParameter();
            parameter7.ParameterName = "@COBreqd";
            parameter7.SqlDbType = SqlDbType.NVarChar;
            parameter7.Direction = ParameterDirection.Input;
            parameter7.Value = req.COBreqd;

            SqlParameter parameter8 = new SqlParameter();
            parameter8.ParameterName = "@BANUnderRepair";
            parameter8.SqlDbType = SqlDbType.NVarChar;
            parameter8.Direction = ParameterDirection.Input;
            //parameter8.Value = req.BANUnderRepair;
            if (req.BANUnderRepair == null)
            {
                parameter8.Value = "0";
            }
            else
            {
                parameter8.Value = req.BANUnderRepair;
            }


            SqlParameter parameter9 = new SqlParameter();
            parameter9.ParameterName = "@COBUnderRepair";
            parameter9.SqlDbType = SqlDbType.NVarChar;
            parameter9.Direction = ParameterDirection.Input;
            //parameter9.Value = req.COBUnderRepair;
            if (req.COBUnderRepair == null)
            {
                parameter9.Value = "0";
            }
            else
            {
                parameter9.Value = req.COBUnderRepair;
            }



            SqlParameter parameter11 = new SqlParameter();
            parameter11.ParameterName = "@BANdiff";
            parameter11.SqlDbType = SqlDbType.NVarChar;
            parameter11.Direction = ParameterDirection.Input;
            if (req.BANdiff == null)
            {
                parameter11.Value = "0";
            }
            else
            {
                parameter11.Value = req.BANdiff;
            }


            SqlParameter parameter12 = new SqlParameter();
            parameter12.ParameterName = "@COBdiff";
            parameter12.SqlDbType = SqlDbType.NVarChar;
            parameter12.Direction = ParameterDirection.Input;
            if (req.COBdiff == null)
            {
                parameter12.Value = "0";
            }
            else
            {
                parameter12.Value = req.COBdiff;
            }


            SqlParameter parameter13 = new SqlParameter();
            parameter13.ParameterName = "@Status";
            parameter13.SqlDbType = SqlDbType.NVarChar;
            parameter13.Direction = ParameterDirection.Input;
            if (req.Status == null)
            {
                parameter13.Value = string.Empty;
            }
            else
            {
                parameter13.Value = req.Status;
            }


            SqlParameter parameter14 = new SqlParameter();
            parameter14.ParameterName = "@PriceOriginal";
            parameter14.SqlDbType = SqlDbType.NVarChar;
            parameter14.Direction = ParameterDirection.Input;
            parameter14.Value = req.PriceOriginal;

            SqlParameter parameter15 = new SqlParameter();
            parameter15.ParameterName = "@Currency";
            parameter15.SqlDbType = SqlDbType.NVarChar;
            parameter15.Direction = ParameterDirection.Input;
            parameter15.Value = req.Currency;

            SqlParameter parameter16 = new SqlParameter();
            parameter16.ParameterName = "@PriceUSD";
            parameter16.SqlDbType = SqlDbType.NVarChar;
            parameter16.Direction = ParameterDirection.Input;
            parameter16.Value = req.PriceUSD;

            SqlParameter parameter17 = new SqlParameter();
            parameter17.ParameterName = "@BANTotalPrice";
            parameter17.SqlDbType = SqlDbType.NVarChar;
            parameter17.Direction = ParameterDirection.Input;
            parameter17.Value = req.BANTotalPrice;

            SqlParameter parameter18 = new SqlParameter();
            parameter18.ParameterName = "@COBTotalPrice";
            parameter18.SqlDbType = SqlDbType.NVarChar;
            parameter18.Direction = ParameterDirection.Input;
            parameter18.Value = req.COBTotalPrice;

            SqlParameter parameter19 = new SqlParameter();
            parameter19.ParameterName = "@ID";
            parameter19.SqlDbType = SqlDbType.NVarChar;
            parameter19.Direction = ParameterDirection.Input;
            parameter19.Value = req.ID;

            command.Parameters.Add(parameter1);
            command.Parameters.Add(parameter2);
            command.Parameters.Add(parameter3);
            command.Parameters.Add(parameter4);
            command.Parameters.Add(parameter5);
            command.Parameters.Add(parameter6);
            command.Parameters.Add(parameter7);
            command.Parameters.Add(parameter8);
            command.Parameters.Add(parameter9);
            //command.Parameters.Add(parameter10);
            command.Parameters.Add(parameter11);
            command.Parameters.Add(parameter12);
            command.Parameters.Add(parameter13);
            command.Parameters.Add(parameter14);
            command.Parameters.Add(parameter15);
            command.Parameters.Add(parameter16);
            command.Parameters.Add(parameter17);
            command.Parameters.Add(parameter18);
            command.Parameters.Add(parameter19);

            //OpenConnection();
            //WriteLog("Executing STORED PROCEDURE");
            //command.CommandTimeout = 300; //5 min

            //ErrorMsg = command.ExecuteScalar().ToString();
            //WriteLog("ErrorMsg: " + ErrorMsg);
            //command.ExecuteNonQuery();
            try
            {
                OpenConnection();
                command.ExecuteNonQuery();
                //if(result.Count == 0)
                //{
                //    cmd1.ExecuteNonQuery();
                //}
                Console.WriteLine("Records Inserted Successfully");
                return Json(new { data = GetSpareData(), success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // alert()
                // MessageBox.Show(" Not Updated");
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
            finally
            {
                CloseConnection();
            }


        }
        //To add all the 
        //public ActionResult AddOrEditSpare(SpareInventory req)
        //{
        //    List<SpareInventoryPO> result = new List<SpareInventoryPO>();
        //    DataTable dt = new DataTable();
        //    if (req.ID == 0)
        //    {
        //        string Query1 = "Select ID,BANQty,COBQty from [SpareInventory_Table] where SpareHW = '" + req.SpareHW + "'";
        //        connection();
        //        OpenConnection();
        //        SqlCommand cmd1 = new SqlCommand(Query1, conn);
        //        SqlDataAdapter da = new SqlDataAdapter(cmd1);

        //        da.Fill(dt);


        //        foreach (DataRow row in dt.Rows)
        //        {
        //            SpareInventoryPO item = new SpareInventoryPO();
        //            item.ID = int.Parse(row["ID"].ToString());
        //            item.BANQty = int.Parse(row["BANQty"].ToString());
        //            if (item.BANQty == 0)
        //            {
        //                req.BANQty = "0";
        //            }
        //            else
        //            {
        //                var add1 = int.Parse(req.BANQty) + item.BANQty;
        //                req.BANQty = add1.ToString();
        //            }
        //            item.COBQty = int.Parse(row["COBQty"].ToString());
        //            if (item.COBQty == 0)
        //            {
        //                req.COBQty = "0";

        //            }
        //            else
        //            {
        //                var add2 = int.Parse(req.COBQty) + item.COBQty;
        //                req.COBQty = add2.ToString();
        //            }
        //            result.Add(item);
        //        }

        //        CloseConnection();
        //    }



        //    connection();
        //    dt = new DataTable();
        //    string Query = "";
        //    System.Security.Principal.IPrincipal User = System.Web.HttpContext.Current.User;


        //    //Update Query
        //    if (req.ID != 0 || result.Count != 0)
        //    {

        //        Query = "UPDATE [SpareInventory_Table] SET " +

        //                "[SpareHW]= @SpareHW," +
        //                "[BANQty]= @BANQty, " +
        //                "[COBQty]= @COBQty, " +
        //                "[TotalQty]= @TotalQty, " +
        //                "[SpareCalc]= @SpareCalc, " +
        //                "[BANreqd]= @BANreqd, " +
        //                "[COBreqd]= @COBreqd, " +
        //                "[BANUnderRepair]= @BANUnderRepair, " +
        //                "[COBUnderRepair]= @COBUnderRepair, " +
        //                "[BANdiff]= @BANdiff," +
        //                "[COBdiff]= @COBdiff," +
        //                "[Status]= @Status," +
        //                "[PriceOriginal]= @PriceOriginal," +
        //                "[Currency]= @Currency," +
        //                "[PriceUSD]= @PriceUSD," +
        //                "[BANTotalPrice]= @BANTotalPrice," +
        //                "[COBTotalPrice]= @COBTotalPrice " +


        //                //"[Available Qty] = @AvailableQty " +
        //                " WHERE ID = @ID";

        //    }
        //    else
        //    {

        //        Query = "INSERT into[SpareInventory_Table]" +
        //                     "(" +
        //                     "[SpareHW]" +
        //                     ",[BANQty]" +
        //                     ",[COBQty]" +
        //                     ",[TotalQty]" +
        //                     ",[SpareCalc]" +
        //                     ",[BANreqd]" +
        //                     ",[COBreqd]" +
        //                     ",[BANUnderRepair]" +
        //                     ",[COBUnderRepair]" +
        //                     ",[BANdiff]" +
        //                     ",[COBdiff]" +
        //                     ",[Status]" +
        //                     ",[PriceOriginal]" +
        //                     ",[Currency]" +
        //                     ",[PriceUSD]" +
        //                     ",[BANTotalPrice]" +
        //                     ",[COBTotalPrice]" +


        //                      ")" +
        //                      "values" +
        //                      "(" +
        //                      " @SpareHW  " +
        //                      " ,@BANQty  " +
        //                      " ,@COBQty  " +
        //                      " ,@TotalQty  " +
        //                      " ,@SpareCalc  " +
        //                      ", @BANreqd " +
        //                      ", @COBreqd" +
        //                      ", @BANUnderRepair" +
        //                      ", @COBUnderRepair " +
        //                      ", @BANdiff " +
        //                      ", @COBdiff" +
        //                      ", @Status" +
        //                      ", @PriceOriginal" +
        //                      ", @Currency" +
        //                      ", @PriceUSD" +
        //                      ", @BANTotalPrice" +
        //                      ", @COBTotalPrice" +

        //                      ")";


        //    }


        //    SqlCommand cmd = new SqlCommand(Query, conn);
        //    cmd.Parameters.AddWithValue("@SpareHW", req.SpareHW);
        //    cmd.Parameters.AddWithValue("@BANQty", req.BANQty);
        //    cmd.Parameters.AddWithValue("@COBQty", req.COBQty);
        //    cmd.Parameters.AddWithValue("@TotalQty", req.TotalQty);
        //    if (req.SpareCalc == null)
        //        cmd.Parameters.AddWithValue("@SpareCalc", string.Empty);
        //    else
        //        cmd.Parameters.AddWithValue("@SpareCalc", req.SpareCalc);

        //    cmd.Parameters.AddWithValue("@BANreqd", req.BANreqd);
        //    cmd.Parameters.AddWithValue("@COBreqd", req.COBreqd);
        //    if (req.BANUnderRepair == null)
        //        cmd.Parameters.AddWithValue("@BANUnderRepair", "0");
        //    else
        //        cmd.Parameters.AddWithValue("@BANUnderRepair", req.BANUnderRepair);
        //    if (req.COBUnderRepair == null)
        //        cmd.Parameters.AddWithValue("@COBUnderRepair", "0");
        //    else
        //        cmd.Parameters.AddWithValue("@COBUnderRepair", req.COBUnderRepair);
        //    cmd.Parameters.AddWithValue("@BANdiff", req.BANdiff);
        //    cmd.Parameters.AddWithValue("@COBdiff", req.COBdiff);
        //    cmd.Parameters.AddWithValue("@Status", req.Status);
        //    cmd.Parameters.AddWithValue("@PriceOriginal", req.PriceOriginal);
        //    cmd.Parameters.AddWithValue("@Currency", req.Currency);
        //    cmd.Parameters.AddWithValue("@PriceUSD", req.PriceUSD);
        //    cmd.Parameters.AddWithValue("@BANTotalPrice", req.BANTotalPrice);
        //    cmd.Parameters.AddWithValue("@COBTotalPrice", req.COBTotalPrice);
        //    cmd.Parameters.AddWithValue("@ID", req.ID);


        //    try
        //    {
        //        OpenConnection();
        //        cmd.ExecuteNonQuery();
        //        Console.WriteLine("Records Inserted Successfully");
        //        return Json(new { data = GetSpareData(), success = true }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        // alert()
        //        // MessageBox.Show(" Not Updated");
        //        return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        //    }
        //    finally
        //    {
        //        CloseConnection();
        //    }

        //    //} 

        //}

        [HttpPost]
        public ActionResult DeleteSpare(int id)
        {
            DataTable dt = new DataTable();

            connection();

            dt = new DataTable();
            string Query = "";
            System.Security.Principal.IPrincipal User = System.Web.HttpContext.Current.User;

            Query = "Delete from [SpareInventory_Table] where ID = @Id";


            SqlCommand cmd = new SqlCommand(Query, conn);
            cmd.Parameters.AddWithValue("@Id", id);
            try
            {
                OpenConnection();
                cmd.ExecuteNonQuery();
                return Json(new { data = GetSpareData(), success = true }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
            finally
            {
                CloseConnection();

            }


        }

        public ActionResult SpareConfiguration()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddorEditConfiguration(SpareConfiguration req)
        {

            List<SpareConfiguration> result = new List<SpareConfiguration>();
            connection();
            DataTable dt = new DataTable();
            string Query = "";
            System.Security.Principal.IPrincipal User = System.Web.HttpContext.Current.User;


            //Update Query
            if (req.ID != 0)
            {

                Query = "UPDATE [Spare_Configuration_Table] SET " +

                        "[SpareHW]= @SpareHW," +
                        "[SpareCount]= @SpareCount, " +
                        "[HWCount]= @HWCount, " +
                        "[MultiplicationFactor]= @MultiplicationFactor, " +
                        "[SpareCalc]= @SpareCalc " +
                        " WHERE ID = @ID";

            }
            else
            {

                Query = "INSERT into[Spare_Configuration_Table]" +
                             "(" +
                             "[SpareHW]" +
                             ",[SpareCount]" +
                             ",[HWCount]" +
                             ",[MultiplicationFactor]" +
                             ",[SpareCalc]" +
                              ")" +
                              "values" +
                              "(" +
                              " @SpareHW  " +
                              " ,@SpareCount  " +
                              " ,@HWCount  " +
                              " ,@MultiplicationFactor  " +
                              " ,@SpareCalc  " +


                              ")";


            }


            SqlCommand cmd = new SqlCommand(Query, conn);
            cmd.Parameters.AddWithValue("@SpareHW", req.SpareHW);
            cmd.Parameters.AddWithValue("@SpareCount", req.SpareCount);
            cmd.Parameters.AddWithValue("@HWCount", req.HWCount);
            cmd.Parameters.AddWithValue("@MultiplicationFactor", req.MultiplicationFactor);
            if (req.SpareCalc == null)
                cmd.Parameters.AddWithValue("@SpareCalc", string.Empty);
            else
                cmd.Parameters.AddWithValue("@SpareCalc", req.SpareCalc);
            cmd.Parameters.AddWithValue("@ID", req.ID);

            try
            {
                OpenConnection();
                cmd.ExecuteNonQuery();
                Console.WriteLine("Records Inserted Successfully");
                return Json(new { data = GetSpareData(), success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // alert()
                // MessageBox.Show(" Not Updated");
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
            finally
            {
                CloseConnection();
            }

            //} 

        }
        [HttpPost]
        public ActionResult DeleteConfiguration(int id)
        {
            DataTable dt = new DataTable();

            connection();

            dt = new DataTable();
            string Query = "";
            System.Security.Principal.IPrincipal User = System.Web.HttpContext.Current.User;

            Query = "Delete from [Spare_Configuration_Table] where ID = @Id";


            SqlCommand cmd = new SqlCommand(Query, conn);
            cmd.Parameters.AddWithValue("@Id", id);
            try
            {
                OpenConnection();
                cmd.ExecuteNonQuery();
                return Json(new { data = GetSpareConfig(), success = true }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
            finally
            {
                CloseConnection();

            }


        }
        [HttpPost]
        public ActionResult GetSpareCalc(int id)
        {
            List<SpareConfiguration> spList = new List<SpareConfiguration>();
            DataTable dt = new DataTable();
            try
            {

                connection();

                string Query = " Select [SpareCalc] from [Spare_Configuration_Table] where [SpareHW]= @SpareHW";

                OpenConnection();
                SqlCommand cmd = new SqlCommand(Query, conn);
                cmd.Parameters.AddWithValue("@SpareHW", id);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                CloseConnection();
            }
            catch (Exception e)
            {

            }
            foreach (DataRow row in dt.Rows)
            {
                SpareConfiguration item = new SpareConfiguration();
                item.SpareCalc = row["SpareCalc"].ToString();

                spList.Add(item);
            }


            if (spList.Count == 0)
            {
                return Json(new { data = "0" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = true, data = spList[0].SpareCalc }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public ActionResult GetEURINRates(int ID)
        {
            List<CurrencyConversion> currList = new List<CurrencyConversion>();
            DataTable dt = new DataTable();
            try
            {

                connection();

                string Query = " Select  [ID]" +
          ",[Currency] " +
          ",[ConversionRate_to_USD] " +
          "from [Currency_Conversion_Table] where ID = @ID ";

                OpenConnection();
                SqlCommand cmd = new SqlCommand(Query, conn);
                cmd.Parameters.AddWithValue("@ID", ID);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                CloseConnection();
            }
            catch (Exception e)
            {

            }
            foreach (DataRow row in dt.Rows)
            {
                CurrencyConversion item = new CurrencyConversion();
                item.ID = int.Parse(row["ID"].ToString());
                item.Currency = row["Currency"].ToString();
                item.CurrencyRate = row["ConversionRate_to_USD"].ToString();

                currList.Add(item);
            }




            return Json(new { data = currList }, JsonRequestBehavior.AllowGet);

            //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())
            //{
            //    EURINR eurinr = new EURINR();
            //    eurinr.EUR = /*(decimal)1.15;*/ db.Currency_Conversion_Table.FirstOrDefault(x => x.Currency.Contains("EUR")).ConversionRate_to_USD;
            //    eurinr.INR = /*(decimal)0.014;*/db.Currency_Conversion_Table.FirstOrDefault(x => x.Currency.Contains("INR")).ConversionRate_to_USD;

            //    return Json(eurinr, JsonRequestBehavior.AllowGet);
            //}

        }






        //To add all the 
        //public ActionResult AddOrEditSpare(SpareInventory req)
        //{
        //    DataTable dt = new DataTable();

        //    connection();
        //    dt = new DataTable();
        //    string Query = "";
        //    System.Security.Principal.IPrincipal User = System.Web.HttpContext.Current.User;


        //    //Update Query
        //    if (req.ID != 0)
        //    {

        //        Query = "UPDATE [SpareInventory_Table] SET " +

        //                "[SpareHW]= @SpareHW," +
        //                "[BANQty]= @BANQty, " +
        //                "[COBQty]= @COBQty, " +
        //                "[TotalQty]= @TotalQty, " +
        //                "[SpareCalc]= @SpareCalc, " +
        //                "[BANreqd]= @BANreqd, " +
        //                "[COBreqd]= @COBreqd, " +
        //                "[BANUnderRepair]= @BANUnderRepair, " +
        //                "[COBUnderRepair]= @COBUnderRepair, " +
        //                "[BANdiff]= @BANdiff," +
        //                "[COBdiff]= @COBdiff," +
        //                "[Status]= @Status," +
        //                "[PriceOriginal]= @PriceOriginal," +
        //                "[Currency]= @Currency," +
        //                "[PriceUSD]= @PriceUSD," +
        //                "[BANTotalPrice]= @BANTotalPrice," +
        //                "[COBTotalPrice]= @COBTotalPrice " +


        //                //"[Available Qty] = @AvailableQty " +
        //                " WHERE ID = @ID";

        //    }
        //    else
        //    {

        //        Query = "INSERT into[SpareInventory_Table]" +
        //                     "(" +
        //                     "[SpareHW]" +
        //                     ",[BANQty]" +
        //                     ",[COBQty]" +
        //                     ",[TotalQty]" +
        //                     ",[SpareCalc]" +
        //                     ",[BANreqd]" +
        //                     ",[COBreqd]" +
        //                     ",[BANUnderRepair]" +
        //                     ",[COBUnderRepair]" +
        //                     ",[BANdiff]" +
        //                     ",[COBdiff]" +
        //                     ",[Status]" +
        //                     ",[PriceOriginal]" +
        //                     ",[Currency]" +
        //                     ",[PriceUSD]" +
        //                     ",[BANTotalPrice]" +
        //                     ",[COBTotalPrice]" +


        //                      ")" +
        //                      "values" +
        //                      "(" +
        //                      " @SpareHW  " +
        //                      " ,@BANQty  " +
        //                      " ,@COBQty  " +
        //                      " ,@TotalQty  " +
        //                      " ,@SpareCalc  " +
        //                      ", @BANreqd " +
        //                      ", @COBreqd" +
        //                      ", @BANUnderRepair" +
        //                      ", @COBUnderRepair " +
        //                      ", @BANdiff " +
        //                      ", @COBdiff" +
        //                      ", @Status" +
        //                      ", @PriceOriginal" +
        //                      ", @Currency" +
        //                      ", @PriceUSD" +
        //                      ", @BANTotalPrice" +
        //                      ", @COBTotalPrice" +

        //                      ")";


        //    }


        //    SqlCommand cmd = new SqlCommand(Query, conn);
        //    cmd.Parameters.AddWithValue("@SpareHW", req.SpareHW);
        //    cmd.Parameters.AddWithValue("@BANQty", req.BANQty);
        //    cmd.Parameters.AddWithValue("@COBQty", req.COBQty);
        //    cmd.Parameters.AddWithValue("@TotalQty", req.TotalQty);
        //    if (req.SpareCalc == null)
        //        cmd.Parameters.AddWithValue("@SpareCalc", string.Empty);
        //    else
        //        cmd.Parameters.AddWithValue("@SpareCalc", req.SpareCalc);

        //    cmd.Parameters.AddWithValue("@BANreqd", req.BANreqd);
        //    cmd.Parameters.AddWithValue("@COBreqd", req.COBreqd);
        //    cmd.Parameters.AddWithValue("@BANUnderRepair", req.BANUnderRepair);
        //    cmd.Parameters.AddWithValue("@COBUnderRepair", req.COBUnderRepair);
        //    cmd.Parameters.AddWithValue("@BANdiff", req.BANdiff);
        //    cmd.Parameters.AddWithValue("@COBdiff", req.COBdiff);
        //    cmd.Parameters.AddWithValue("@Status", req.Status);
        //    cmd.Parameters.AddWithValue("@PriceOriginal", req.PriceOriginal);
        //    cmd.Parameters.AddWithValue("@Currency", req.Currency);
        //    cmd.Parameters.AddWithValue("@PriceUSD", req.PriceUSD);
        //    cmd.Parameters.AddWithValue("@BANTotalPrice", req.BANTotalPrice);
        //    cmd.Parameters.AddWithValue("@COBTotalPrice", req.COBTotalPrice);
        //    cmd.Parameters.AddWithValue("@ID", req.ID);


        //    try
        //    {
        //        OpenConnection();
        //        cmd.ExecuteNonQuery();
        //        Console.WriteLine("Records Inserted Successfully");
        //        return Json(new { data = GetSpareData(), success = true }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        // alert()
        //        // MessageBox.Show(" Not Updated");
        //        return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        //    }
        //    finally
        //    {
        //        CloseConnection();
        //    }

        //    //} 

        //}






        //[HttpPost]
        //public ActionResult DeleteSpare(int id)
        //{
        //    DataTable dt = new DataTable();

        //    connection();

        //    dt = new DataTable();
        //    string Query = "";
        //    System.Security.Principal.IPrincipal User = System.Web.HttpContext.Current.User;

        //    Query = "Delete from [SpareInventory_Table] where ID = @Id";


        //    SqlCommand cmd = new SqlCommand(Query, conn);
        //    cmd.Parameters.AddWithValue("@Id", id);
        //    try
        //    {
        //        OpenConnection();
        //        cmd.ExecuteNonQuery();
        //        return Json(new { data = GetSpareData(), success = true }, JsonRequestBehavior.AllowGet);
        //    }

        //    catch (Exception ex)
        //    {
        //        return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        //    }
        //    finally
        //    {
        //        CloseConnection();

        //    }


        //}

        //public ActionResult SpareConfiguration()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public ActionResult GetSpareCalc(int id)
        //{
        //    List<SpareConfiguration> spList = new List<SpareConfiguration>();
        //    DataTable dt = new DataTable();
        //    try
        //    {

        //        connection();

        //        string Query = " Select [SpareCalc] from [Spare_Configuration_Table] where [SpareHW]= @SpareHW";

        //        OpenConnection();
        //        SqlCommand cmd = new SqlCommand(Query, conn);
        //        cmd.Parameters.AddWithValue("@SpareHW",id);
        //        SqlDataAdapter da = new SqlDataAdapter(cmd);
        //        da.Fill(dt);
        //        CloseConnection();
        //    }
        //    catch (Exception e)
        //    {

        //    }
        //    foreach (DataRow row in dt.Rows)
        //    {
        //        SpareConfiguration item = new SpareConfiguration();
        //        item.SpareCalc = row["SpareCalc"].ToString();

        //        spList.Add(item);
        //    }


        //    return Json(new { data = spList[0].SpareCalc }, JsonRequestBehavior.AllowGet);
        //}




        // Connection String Declaration.

        private static void LabCarInventoryConnection()
        {
            string labcardetailsdata_string = ConfigurationManager.ConnectionStrings["BudgetingdbConnection"].ToString();
            lcdetails_con = new SqlConnection(labcardetailsdata_string);
        }

        // Connection Open 

        private static void LabCarDetailsDataOpenConnection()
        {
            if (lcdetails_con.State == ConnectionState.Closed)
            {
                lcdetails_con.Open();
            }
        }

        //Connection Close 

        private static void LabCarDetailsDataCloseConnection()
        {
            if (lcdetails_con.State == ConnectionState.Open)
            {
                lcdetails_con.Close();
            }
        }


        // Initialise Lab Car Details Function
        // Storing the Entire data in a Form of List.

        public static bool InitialiseLabCarDetails()
        {

            DataSet ds = new DataSet();
            LabCarInventoryConnection();
            LabCarDetailsDataOpenConnection();
            string Query = "select * from LCInventory_Table";
            SqlCommand cmd = new SqlCommand(Query, lcdetails_con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            LabCarDetailsDataCloseConnection();

            labcardetails = new List<LCInventory_Table>();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                LCInventory_Table item = new LCInventory_Table();

                // 10 
                item.Location = row["Location"].ToString();
                item.LCNumber = row["LCNumber"].ToString();
                item.Type = row["Type"].ToString();
                item.BondNo = row["BondNo"].ToString();
                item.BondDate = row["BondDate"].ToString() != "" ? ((DateTime)row["BondDate"]).ToString("yyyy-MM-dd") : string.Empty;
                item.AssetNo = row["AssetNo"].ToString();
                item.InputSupply = row["InputSupply"].ToString();
                item.PCAssetNumber = row["PCAssetNumber"].ToString();
                item.MonitorAssetNumber1 = row["MonitorAssetNumber1"].ToString();
                item.MonitorAssetNumber2 = row["MonitorAssetNumber2"].ToString();


                //12
                item.RTPCProcessorType = row["RTPCProcessorType"].ToString();
                item.RTPCManufacturer = row["RTPCManufacturer"].ToString();
                item.RTPCCards = row["RTPCCards"].ToString();
                item.EB5200SerialNumber = row["EB5200SerialNumber"].ToString();
                item.IB600SerialNumber = row["IB600SerialNumber"].ToString();
                item.IB200SerialNumber = row["IB200SerialNumber"].ToString();
                item.LDUEMU = row["LDUEMU"].ToString();
                item.VSC = row["VSC"].ToString();
                item.WSS2 = row["WSS2"].ToString();
                item.LDUAECU = row["LDUAECU"].ToString();
                item.HSPlusInterfaceSerialNumber = row["HSPlusInterfaceSerialNumber"].ToString();
                item.HSXInterfaceSerialNumber = row["HSXInterfaceSerialNumber"].ToString();



                //10 (Vector)
                item.SoftwareLicenseName = row["SoftwareLicenseName"].ToString();
                item.SoftwareLicenseVersion = (int)row["SoftwareLicenseVersion"];
                item.HardwareModel = row["HardwareModel"].ToString();
                item.HardwareSerialNumber = row["HardwareSerialNumber"].ToString();
                item.HardwareLicenseName = row["HardwareLicenseName"].ToString();
                item.HardwareLicenseVersion = (int)row["HardwareLicenseVersion"];
                item.HardwareLicenseSerialNumber = row["HardwareLicenseSerialNumber"].ToString();
                item.HardwareLicenseNameOptional_1 = row["HardwareLicenseNameOptional_1"].ToString();
                item.HardwareLicenseVersionOptional_1 = (int)row["HardwareLicenseVersionOptional_1"];
                item.HardwareLicenseSerialNumberOptional_1 = row["HardwareLicenseSerialNumberOptional_1"].ToString();

                //7(Vector)
                item.HardwareLicenseNameOptional_2 = row["HardwareLicenseNameOptional_2"].ToString();
                item.HardwareLicenseVersionOptional_2 = (int)row["HardwareLicenseVersionOptional_2"];
                item.HardwareLicenseSerialNumberOptional_2 = row["HardwareLicenseSerialNumberOptional_2"].ToString();
                item.MeasurementInterfaceModel = row["MeasurementInterfaceModel"].ToString();
                item.MeasurementInterfaceSerialNumber = row["MeasurementInterfaceSerialNumber"].ToString();
                item.Motsim = row["Motsim"].ToString();
                item.PowerSupply = row["PowerSupply"].ToString();


                labcardetails.Add(item);

            }


            return true;
        }


        public ActionResult GetLabCarDetails()
        {
            return Json(new { data = labcardetails, success = true, JsonRequestBehavior.AllowGet });
        }



        // Insert Lab Car Details Function

        [HttpPost]
        public ActionResult InsertLabCarDetails(LCInventory_Table item)
        {

            string Query = "";
            int result = 0;
            LabCarInventoryConnection();


            // Insert the Values
            {



                //1
                if (item.Location == null)
                    item.Location = "";


                //2
                if (item.LCNumber == null)
                    item.LCNumber = "";


                //3
                if (item.Type == null)
                    item.Type = "";


                //4
                if (item.BondNo == null)
                    item.BondNo = "";


                //5
                if (item.BondDate.ToString() == null)
                    item.BondDate = "";

                //6
                if (item.AssetNo == null)
                    item.AssetNo = "";

                //7
                if (item.InputSupply == null)
                    item.InputSupply = "";


                //8
                if (item.PCAssetNumber == null)
                    item.PCAssetNumber = "";

                //9
                if (item.MonitorAssetNumber1 == null)
                    item.MonitorAssetNumber1 = "";


                //10
                if (item.MonitorAssetNumber2 == null)
                    item.MonitorAssetNumber2 = "";


                //11
                if (item.RTPCProcessorType == null)
                    item.RTPCProcessorType = "";


                //12
                if (item.RTPCManufacturer == null)
                    item.RTPCManufacturer = "";

                //13
                if (item.RTPCCards == null)
                    item.RTPCCards = "";

                //14
                if (item.EB5200SerialNumber == null)
                    item.EB5200SerialNumber = "";

                //15
                if (item.IB600SerialNumber == null)
                    item.IB600SerialNumber = "";

                //16
                if (item.IB200SerialNumber == null)
                    item.IB200SerialNumber = "";
                //17
                if (item.LDUEMU == null)
                    item.LDUEMU = "";

                //18
                if (item.VSC == null)
                    item.VSC = "";

                //19
                if (item.WSS2 == null)
                    item.WSS2 = "";

                //20
                if (item.LDUAECU == null)
                    item.LDUAECU = "";

                //21
                if (item.HSPlusInterfaceSerialNumber == null)
                    item.HSPlusInterfaceSerialNumber = "";

                //22
                if (item.HSXInterfaceSerialNumber == null)
                    item.HSXInterfaceSerialNumber = "";

                //23
                if (item.SoftwareLicenseName == null)
                    item.SoftwareLicenseName = "";


                //24
                if (item.SoftwareLicenseVersion.ToString() == null)
                    item.SoftwareLicenseVersion = int.Parse("");


                //25
                if (item.HardwareModel == null)
                    item.HardwareModel = "";

                //26
                if (item.HardwareSerialNumber == null)
                    item.HardwareSerialNumber = "";

                //27
                if (item.HardwareLicenseName == null)
                    item.HardwareLicenseName = "";

                //28
                if (item.HardwareLicenseVersion.ToString() == null)
                    item.HardwareLicenseVersion = int.Parse("");

                //29
                if (item.HardwareLicenseSerialNumber == null)
                    item.HardwareLicenseSerialNumber = "";


                //30
                if (item.HardwareLicenseNameOptional_1 == null)
                    item.HardwareLicenseNameOptional_1 = "";


                //31
                if (item.HardwareLicenseVersionOptional_1.ToString() == null)
                    item.HardwareLicenseVersionOptional_1 = int.Parse("");


                //32
                if (item.HardwareLicenseSerialNumberOptional_1 == null)
                    item.HardwareLicenseSerialNumberOptional_1 = "";


                //33
                if (item.HardwareLicenseNameOptional_2 == null)
                    item.HardwareLicenseNameOptional_2 = "";


                //34
                if (item.HardwareLicenseVersionOptional_2.ToString() == null)
                    item.HardwareLicenseVersionOptional_2 = int.Parse("");


                //35
                if (item.HardwareLicenseSerialNumberOptional_2 == null)
                    item.HardwareLicenseSerialNumberOptional_2 = "";


                //36
                if (item.MeasurementInterfaceModel == null)
                    item.MeasurementInterfaceModel = "";


                //37
                if (item.MeasurementInterfaceSerialNumber == null)
                    item.MeasurementInterfaceSerialNumber = "";


                //38
                if (item.Motsim == null)
                    item.Motsim = "";


                //39
                if (item.PowerSupply == null)
                    item.PowerSupply = "";
            }

            // Writing Query to Insert the values in the fields

            {

                Query += " Exec [dbo].[LCInventory_InsertDetails] '" +

                    //1
                    item.Location.ToString() + "','"

                    //2
                    + item.LCNumber.ToString() + "','"

                    //3
                    + item.Type.ToString() + "','"

                    //4
                    + item.BondNo.ToString() + "','"

                    //5
                    + item.BondDate.ToString() + "','"

                    //6
                    + item.AssetNo.ToString() + "','"

                    //7
                    + item.InputSupply.ToString() + "','"

                    //8
                    + item.PCAssetNumber.ToString() + "','"

                    //9
                    + item.MonitorAssetNumber1.ToString() + "','"

                    //10
                    + item.MonitorAssetNumber2.ToString() + "','"

                    //11
                    + item.RTPCProcessorType.ToString() + "','"

                    //12
                    + item.RTPCManufacturer.ToString() + "','"

                    //13
                    + item.RTPCCards.ToString() + "','"

                    //14
                    + item.EB5200SerialNumber.ToString() + "','"

                    //15
                    + item.IB600SerialNumber.ToString() + "','"

                    //16
                    + item.IB200SerialNumber.ToString() + "','"

                    //17
                    + item.LDUEMU.ToString() + "','"

                    //18
                    + item.VSC.ToString() + "','"

                    //19
                    + item.WSS2.ToString() + "','"

                    //20
                    + item.LDUAECU.ToString() + "','"

                    //21
                    + item.HSPlusInterfaceSerialNumber.ToString() + "','"

                    //22
                    + item.HSXInterfaceSerialNumber.ToString() + "','"

                    //23
                    + item.SoftwareLicenseName.ToString() + "','"

                    //24
                    + item.SoftwareLicenseVersion.ToString() + "','"

                    //25
                    + item.HardwareModel.ToString() + "','"

                    //26
                    + item.HardwareSerialNumber.ToString() + "','"

                    //27
                    + item.HardwareLicenseName.ToString() + "','"

                    //28
                    + item.HardwareLicenseVersion.ToString() + "','"

                    //29
                    + item.HardwareLicenseSerialNumber.ToString() + "','"

                    //30
                    + item.HardwareLicenseNameOptional_1.ToString() + "','"

                    //31
                    + item.HardwareLicenseVersionOptional_1.ToString() + "','"

                    //32
                    + item.HardwareLicenseSerialNumberOptional_1.ToString() + "','"

                    //33
                    + item.HardwareLicenseNameOptional_2.ToString() + "','"

                    //34
                    + item.HardwareLicenseVersionOptional_2.ToString() + "','"

                    //35
                    + item.HardwareLicenseSerialNumberOptional_2.ToString() + "','"

                    //36
                    + item.MeasurementInterfaceModel.ToString() + "','"

                    //37
                    + item.MeasurementInterfaceSerialNumber.ToString() + "','"

                    //38
                    + item.Motsim.ToString() + "','"

                    //39
                    + item.PowerSupply.ToString() + "'";

                LabCarDetailsDataOpenConnection();
                SqlCommand command = new SqlCommand(Query, lcdetails_con);
                command.ExecuteNonQuery();
                LabCarDetailsDataCloseConnection();
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }




        // Update Lab Car Details Function
        [HttpPost]
        public ActionResult UpdateLabCarDetails(LCInventory_Table item)
        {

            string Query = "";
            int result = 0;

            // Update the Values
            {



                //1
                if (item.Location == null)
                    item.Location = "";


                //2
                if (item.LCNumber == null)
                    item.LCNumber = "";


                //3
                if (item.Type == null)
                    item.Type = "";


                //4
                if (item.BondNo == null)
                    item.BondNo = "";


                //5
                if (item.BondDate.ToString() == null)
                    item.BondDate = "";

                //6
                if (item.AssetNo == null)
                    item.AssetNo = "";

                //7
                if (item.InputSupply == null)
                    item.InputSupply = "";


                //8
                if (item.PCAssetNumber == null)
                    item.PCAssetNumber = "";

                //9
                if (item.MonitorAssetNumber1 == null)
                    item.MonitorAssetNumber1 = "";


                //10
                if (item.MonitorAssetNumber2 == null)
                    item.MonitorAssetNumber2 = "";


                //11
                if (item.RTPCProcessorType == null)
                    item.RTPCProcessorType = "";


                //12
                if (item.RTPCManufacturer == null)
                    item.RTPCManufacturer = "";

                //13
                if (item.RTPCCards == null)
                    item.RTPCCards = "";

                //14
                if (item.EB5200SerialNumber == null)
                    item.EB5200SerialNumber = "";

                //15
                if (item.IB600SerialNumber == null)
                    item.IB600SerialNumber = "";

                //16
                if (item.IB200SerialNumber == null)
                    item.IB200SerialNumber = "";
                //17
                if (item.LDUEMU == null)
                    item.LDUEMU = "";

                //18
                if (item.VSC == null)
                    item.VSC = "";

                //19
                if (item.WSS2 == null)
                    item.WSS2 = "";

                //20
                if (item.LDUAECU == null)
                    item.LDUAECU = "";

                //21
                if (item.HSPlusInterfaceSerialNumber == null)
                    item.HSPlusInterfaceSerialNumber = "";

                //22
                if (item.HSXInterfaceSerialNumber == null)
                    item.HSXInterfaceSerialNumber = "";

                //23
                if (item.SoftwareLicenseName == null)
                    item.SoftwareLicenseName = "";


                //24
                if (item.SoftwareLicenseVersion.ToString() == null)
                    item.SoftwareLicenseVersion = int.Parse("");


                //25
                if (item.HardwareModel == null)
                    item.HardwareModel = "";

                //26
                if (item.HardwareSerialNumber == null)
                    item.HardwareSerialNumber = "";

                //27
                if (item.HardwareLicenseName == null)
                    item.HardwareLicenseName = "";

                //28
                if (item.HardwareLicenseVersion.ToString() == null)
                    item.HardwareLicenseVersion = int.Parse("");

                //29
                if (item.HardwareLicenseSerialNumber == null)
                    item.HardwareLicenseSerialNumber = "";


                //30
                if (item.HardwareLicenseNameOptional_1 == null)
                    item.HardwareLicenseNameOptional_1 = "";


                //31
                if (item.HardwareLicenseVersionOptional_1.ToString() == null)
                    item.HardwareLicenseVersionOptional_1 = int.Parse("");


                //32
                if (item.HardwareLicenseSerialNumberOptional_1 == null)
                    item.HardwareLicenseSerialNumberOptional_1 = "";


                //33
                if (item.HardwareLicenseNameOptional_2 == null)
                    item.HardwareLicenseNameOptional_2 = "";


                //34
                if (item.HardwareLicenseVersionOptional_2.ToString() == null)
                    item.HardwareLicenseVersionOptional_2 = int.Parse("");


                //35
                if (item.HardwareLicenseSerialNumberOptional_2 == null)
                    item.HardwareLicenseSerialNumberOptional_2 = "";


                //36
                if (item.MeasurementInterfaceModel == null)
                    item.MeasurementInterfaceModel = "";


                //37
                if (item.MeasurementInterfaceSerialNumber == null)
                    item.MeasurementInterfaceSerialNumber = "";


                //38
                if (item.Motsim == null)
                    item.Motsim = "";


                //39
                if (item.PowerSupply == null)
                    item.PowerSupply = "";
            }

            // Writing Query to Update the values in the fields

            {

                Query += " Exec [dbo].[LCInventory_UpdateDetails] '" +

                    //1
                    item.Location.ToString() + "','"

                    //2
                    + item.LCNumber.ToString() + "','"

                    //3
                    + item.Type.ToString() + "','"

                    //4
                    + item.BondNo.ToString() + "','"

                    //5
                    + item.BondDate.ToString() + "','"

                    //6
                    + item.AssetNo.ToString() + "','"

                    //7
                    + item.InputSupply.ToString() + "','"

                    //8
                    + item.PCAssetNumber.ToString() + "','"

                    //9
                    + item.MonitorAssetNumber1.ToString() + "','"

                    //10
                    + item.MonitorAssetNumber2.ToString() + "','"

                    //11
                    + item.RTPCProcessorType.ToString() + "','"

                    //12
                    + item.RTPCManufacturer.ToString() + "','"

                    //13
                    + item.RTPCCards.ToString() + "','"

                    //14
                    + item.EB5200SerialNumber.ToString() + "','"

                    //15
                    + item.IB600SerialNumber.ToString() + "','"

                    //16
                    + item.IB200SerialNumber.ToString() + "','"

                    //17
                    + item.LDUEMU.ToString() + "','"

                    //18
                    + item.VSC.ToString() + "','"

                    //19
                    + item.WSS2.ToString() + "','"

                    //20
                    + item.LDUAECU.ToString() + "','"

                    //21
                    + item.HSPlusInterfaceSerialNumber.ToString() + "','"

                    //22
                    + item.HSXInterfaceSerialNumber.ToString() + "','"

                    //23
                    + item.SoftwareLicenseName.ToString() + "','"

                    //24
                    + item.SoftwareLicenseVersion.ToString() + "','"

                    //25
                    + item.HardwareModel.ToString() + "','"

                    //26
                    + item.HardwareSerialNumber.ToString() + "','"

                    //27
                    + item.HardwareLicenseName.ToString() + "','"

                    //28
                    + item.HardwareLicenseVersion.ToString() + "','"

                    //29
                    + item.HardwareLicenseSerialNumber.ToString() + "','"

                    //30
                    + item.HardwareLicenseNameOptional_1.ToString() + "','"

                    //31
                    + item.HardwareLicenseVersionOptional_1.ToString() + "','"

                    //32
                    + item.HardwareLicenseSerialNumberOptional_1.ToString() + "','"

                    //33
                    + item.HardwareLicenseNameOptional_2.ToString() + "','"

                    //34
                    + item.HardwareLicenseVersionOptional_2.ToString() + "','"

                    //35
                    + item.HardwareLicenseSerialNumberOptional_2.ToString() + "','"

                    //36
                    + item.MeasurementInterfaceModel.ToString() + "','"

                    //37
                    + item.MeasurementInterfaceSerialNumber.ToString() + "','"

                    //38
                    + item.Motsim.ToString() + "','"

                    //39
                    + item.PowerSupply.ToString() + "'";

                LabCarDetailsDataOpenConnection();
                SqlCommand command = new SqlCommand(Query, lcdetails_con);
                command.ExecuteNonQuery();
                LabCarDetailsDataCloseConnection();
                return Json("", JsonRequestBehavior.AllowGet);
            }



        }



        // DeleteLabcarDetails Function
        [HttpPost]
        public ActionResult DeleteLabCarDetails(LCInventory_Table item)
        {

            string Query = "";
            int result = 0;

            // Update the Values
            {



                //1
                if (item.Location == null)
                    item.Location = "";


                //2
                if (item.LCNumber == null)
                    item.LCNumber = "";


                //3
                if (item.Type == null)
                    item.Type = "";


                //4
                if (item.BondNo == null)
                    item.BondNo = "";


                //5
                if (item.BondDate.ToString() == null)
                    item.BondDate = "";

                //6
                if (item.AssetNo == null)
                    item.AssetNo = "";

                //7
                if (item.InputSupply == null)
                    item.InputSupply = "";


                //8
                if (item.PCAssetNumber == null)
                    item.PCAssetNumber = "";

                //9
                if (item.MonitorAssetNumber1 == null)
                    item.MonitorAssetNumber1 = "";


                //10
                if (item.MonitorAssetNumber2 == null)
                    item.MonitorAssetNumber2 = "";


                //11
                if (item.RTPCProcessorType == null)
                    item.RTPCProcessorType = "";


                //12
                if (item.RTPCManufacturer == null)
                    item.RTPCManufacturer = "";

                //13
                if (item.RTPCCards == null)
                    item.RTPCCards = "";

                //14
                if (item.EB5200SerialNumber == null)
                    item.EB5200SerialNumber = "";

                //15
                if (item.IB600SerialNumber == null)
                    item.IB600SerialNumber = "";

                //16
                if (item.IB200SerialNumber == null)
                    item.IB200SerialNumber = "";
                //17
                if (item.LDUEMU == null)
                    item.LDUEMU = "";

                //18
                if (item.VSC == null)
                    item.VSC = "";

                //19
                if (item.WSS2 == null)
                    item.WSS2 = "";

                //20
                if (item.LDUAECU == null)
                    item.LDUAECU = "";

                //21
                if (item.HSPlusInterfaceSerialNumber == null)
                    item.HSPlusInterfaceSerialNumber = "";

                //22
                if (item.HSXInterfaceSerialNumber == null)
                    item.HSXInterfaceSerialNumber = "";

                //23
                if (item.SoftwareLicenseName == null)
                    item.SoftwareLicenseName = "";


                //24
                if (item.SoftwareLicenseVersion.ToString() == null)
                    item.SoftwareLicenseVersion = int.Parse("");


                //25
                if (item.HardwareModel == null)
                    item.HardwareModel = "";

                //26
                if (item.HardwareSerialNumber == null)
                    item.HardwareSerialNumber = "";

                //27
                if (item.HardwareLicenseName == null)
                    item.HardwareLicenseName = "";

                //28
                if (item.HardwareLicenseVersion.ToString() == null)
                    item.HardwareLicenseVersion = int.Parse("");

                //29
                if (item.HardwareLicenseSerialNumber == null)
                    item.HardwareLicenseSerialNumber = "";


                //30
                if (item.HardwareLicenseNameOptional_1 == null)
                    item.HardwareLicenseNameOptional_1 = "";


                //31
                if (item.HardwareLicenseVersionOptional_1.ToString() == null)
                    item.HardwareLicenseVersionOptional_1 = int.Parse("");


                //32
                if (item.HardwareLicenseSerialNumberOptional_1 == null)
                    item.HardwareLicenseSerialNumberOptional_1 = "";


                //33
                if (item.HardwareLicenseNameOptional_2 == null)
                    item.HardwareLicenseNameOptional_2 = "";


                //34
                if (item.HardwareLicenseVersionOptional_2.ToString() == null)
                    item.HardwareLicenseVersionOptional_2 = int.Parse("");


                //35
                if (item.HardwareLicenseSerialNumberOptional_2 == null)
                    item.HardwareLicenseSerialNumberOptional_2 = "";


                //36
                if (item.MeasurementInterfaceModel == null)
                    item.MeasurementInterfaceModel = "";


                //37
                if (item.MeasurementInterfaceSerialNumber == null)
                    item.MeasurementInterfaceSerialNumber = "";


                //38
                if (item.Motsim == null)
                    item.Motsim = "";


                //39
                if (item.PowerSupply == null)
                    item.PowerSupply = "";
            }

            // Writing Query to Update the values in the fields

            {

                Query += " Exec [dbo].[LCInventory_DeleteDetails] '" +

                    //1
                    item.Location.ToString() + "','"

                    //2
                    + item.LCNumber.ToString() + "','"

                    //3
                    + item.Type.ToString() + "','"

                    //4
                    + item.BondNo.ToString() + "','"

                    //5
                    + item.BondDate.ToString() + "','"

                    //6
                    + item.AssetNo.ToString() + "','"

                    //7
                    + item.InputSupply.ToString() + "','"

                    //8
                    + item.PCAssetNumber.ToString() + "','"

                    //9
                    + item.MonitorAssetNumber1.ToString() + "','"

                    //10
                    + item.MonitorAssetNumber2.ToString() + "','"

                    //11
                    + item.RTPCProcessorType.ToString() + "','"

                    //12
                    + item.RTPCManufacturer.ToString() + "','"

                    //13
                    + item.RTPCCards.ToString() + "','"

                    //14
                    + item.EB5200SerialNumber.ToString() + "','"

                    //15
                    + item.IB600SerialNumber.ToString() + "','"

                    //16
                    + item.IB200SerialNumber.ToString() + "','"

                    //17
                    + item.LDUEMU.ToString() + "','"

                    //18
                    + item.VSC.ToString() + "','"

                    //19
                    + item.WSS2.ToString() + "','"

                    //20
                    + item.LDUAECU.ToString() + "','"

                    //21
                    + item.HSPlusInterfaceSerialNumber.ToString() + "','"

                    //22
                    + item.HSXInterfaceSerialNumber.ToString() + "','"

                    //23
                    + item.SoftwareLicenseName.ToString() + "','"

                    //24
                    + item.SoftwareLicenseVersion.ToString() + "','"

                    //25
                    + item.HardwareModel.ToString() + "','"

                    //26
                    + item.HardwareSerialNumber.ToString() + "','"

                    //27
                    + item.HardwareLicenseName.ToString() + "','"

                    //28
                    + item.HardwareLicenseVersion.ToString() + "','"

                    //29
                    + item.HardwareLicenseSerialNumber.ToString() + "','"

                    //30
                    + item.HardwareLicenseNameOptional_1.ToString() + "','"

                    //31
                    + item.HardwareLicenseVersionOptional_1.ToString() + "','"

                    //32
                    + item.HardwareLicenseSerialNumberOptional_1.ToString() + "','"

                    //33
                    + item.HardwareLicenseNameOptional_2.ToString() + "','"

                    //34
                    + item.HardwareLicenseVersionOptional_2.ToString() + "','"

                    //35
                    + item.HardwareLicenseSerialNumberOptional_2.ToString() + "','"

                    //36
                    + item.MeasurementInterfaceModel.ToString() + "','"

                    //37
                    + item.MeasurementInterfaceSerialNumber.ToString() + "','"

                    //38
                    + item.Motsim.ToString() + "','"

                    //39
                    + item.PowerSupply.ToString() + "'";

                LabCarDetailsDataOpenConnection();
                SqlCommand command = new SqlCommand(Query, lcdetails_con);
                command.ExecuteNonQuery();
                LabCarDetailsDataCloseConnection();
                return Json("", JsonRequestBehavior.AllowGet);
            }
        }










    }



}



public partial class Inventory_HeaderFilter_Table
{
    public string text { get; set; }
    public int value { get; set; }
}

public class Inventory_LookupData
{
    //public List<ItemsCostList_Table> Item_FilterRow{ get; set; }

    public List<Inventory_HeaderFilter_Table> Item_HeaderFilter { get; set; }
    public List<Inventory_HeaderFilter_Table> InventoryType_HeaderFilter { get; set; }
    public List<Inventory_HeaderFilter_Table> DEPT_HeaderFilter { get; set; }
    public List<Inventory_HeaderFilter_Table> Group_HeaderFilter { get; set; }
    public List<Inventory_HeaderFilter_Table> OEM_HeaderFilter { get; set; }

    public List<Inventory_HeaderFilter_Table> SpareHW_HeaderFilter { get; set; }

    public List<BU_Table> BU_List { get; set; }
    public List<OEM_Table> OEM_List { get; set; }
    public List<Groups_Table_Test> Groups_test { get; set; }
    public List<Groups_Table> Groups_List { get; set; }

    public List<ItemsCostList_Table> Item_List { get; set; }
    public List<Currency_Table> Currency_List { get; set; }


}



public partial class HardwareInventory
{
    public int ID { get; set; }
    public int InventoryType { get; set; }
    public int InventoryType2 { get; set; }
    public string SerialNo { get; set; }
    public string BondNo { get; set; }
    public string BondDate { get; set; }
    public string AssetNo { get; set; }
    public string HardwareResponsible { get; set; }
    public string HandoverTo { get; set; }
    public int Mode { get; set; }
    public string Remarks { get; set; }
    public string ALMNo { get; set; }
    public int OEM { get; set; }
    public int BU { get; set; }
    public int Group { get; set; }
    public string ItemName { get; set; }
    public int ItemName_Planner { get; set; }
    public int POQty { get; set; }
    public int Quantity { get; set; }
    public string UOM { get; set; }
    public string AvailableQty { get; set; }
    public string ActualDeliveryDate { get; set; }
    public int PODetails_Id { get; set; }
}

public partial class SpareInventory
{
    public int ID { get; set; }
    public int SpareHW { get; set; }
    public string BANQty { get; set; }
    public string COBQty { get; set; }
    public string TotalQty { get; set; }
    public string SpareCalc { get; set; }
    public string BANreqd { get; set; }
    public string COBreqd { get; set; }
    public string BANUnderRepair { get; set; }
    public string COBUnderRepair { get; set; }
    public string BANdiff { get; set; }
    public string COBdiff { get; set; }
    public string Status { get; set; }
    public string PriceOriginal { get; set; }
    public int Currency { get; set; }
    public string PriceUSD { get; set; }
    public int POQty { get; set; }
    public string BANTotalPrice { get; set; }
    public string COBTotalPrice { get; set; }
    public string SpareCount { get; set; }
    public string HWCount { get; set; }
    public string MultiplicationFactor { get; set; }

}
public partial class SpareConfiguration
{
    public int ID { get; set; }
    public int SpareHW  { get; set; }
    public string SpareCount { get; set; }
    public string HWCount { get; set; }
    public string MultiplicationFactor { get; set; }
    public string SpareCalc { get; set; }
    public string BANLabCar { get; set; }
    public string COBLabCar { get; set; }



}

public partial class LCInventory_Table
{
    public string Location { get; set; }
    public string LCNumber { get; set; }
    public string Type { get; set; }
    public string BondNo { get; set; }
    public string BondDate { get; set; }
    public string AssetNo { get; set; }
    public string InputSupply { get; set; }
    public string PCAssetNumber { get; set; }
    public string MonitorAssetNumber1 { get; set; }
    public string MonitorAssetNumber2 { get; set; }
    public string RTPCProcessorType { get; set; }
    public string RTPCManufacturer { get; set; }
    public string RTPCCards { get; set; }
    public string EB5200SerialNumber { get; set; }
    public string IB600SerialNumber { get; set; }
    public string IB200SerialNumber { get; set; }
    public string LDUEMU { get; set; }
    public string VSC { get; set; }
    public string WSS2 { get; set; }
    public string LDUAECU { get; set; }
    public string HSPlusInterfaceSerialNumber { get; set; }
    public string HSXInterfaceSerialNumber { get; set; }
    public string SoftwareLicenseName { get; set; }
    public Nullable<int> SoftwareLicenseVersion { get; set; }
    public string HardwareModel { get; set; }
    public string HardwareSerialNumber { get; set; }
    public string HardwareLicenseName { get; set; }
    public Nullable<int> HardwareLicenseVersion { get; set; }
    public string HardwareLicenseSerialNumber { get; set; }
    public string HardwareLicenseNameOptional_1 { get; set; }
    public Nullable<int> HardwareLicenseVersionOptional_1 { get; set; }
    public string HardwareLicenseSerialNumberOptional_1 { get; set; }
    public string HardwareLicenseNameOptional_2 { get; set; }
    public Nullable<int> HardwareLicenseVersionOptional_2 { get; set; }
    public string HardwareLicenseSerialNumberOptional_2 { get; set; }
    public string MeasurementInterfaceModel { get; set; }
    public string MeasurementInterfaceSerialNumber { get; set; }
    public string Motsim { get; set; }
    public string PowerSupply { get; set; }
}

public partial class CurrencyConversion
{
    public int ID { get; set; }
    public string Currency { get; set; }
    public string CurrencyRate { get; set; }

}

