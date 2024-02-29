using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LC_Reports_V1.Models;
using Newtonsoft.Json;

namespace LC_Reports_V1.Controllers
{
    public class WWInventoryController : Controller
    {
        // GET: WWInventory
        //Loading the WW Inventory Page

        public ActionResult WWInventory()
        {
            return View();
        }

        //function to check authorization for particular users
        //public ActionResult checkAuth()
        //{
        //    string NTID = "";
        //    string todelete = "";
        //    string tomodify = "";
        //    string islablnventory = "";
        //    string ispmtlnventory = "";

        //    connection();
        //    //to check the NTID
        //    string qry = " Select isnull(NTID,'') as NTID,toDelete,toModify,isLabInventory,isPMTInventory from Inventory_Authority where NTID = '" + User.Identity.Name.Split('\\')[1].Trim() + "'";
        //    //  string qry = " Select  [ID]" +
        //    //",[NTID] " +
        //    //"from [Inventory_Authority] order by ID";
        //    OpenConnection();
        //    SqlCommand command = new SqlCommand(qry, conn);
        //    SqlDataReader dr = command.ExecuteReader();
        //    if (dr.HasRows)
        //    {
        //        dr.Read();
        //        NTID = dr["NTID"].ToString();
        //        //delete and modify flags
        //        todelete = dr["toDelete"].ToString();
        //        tomodify = dr["toModify"].ToString();
        //        islablnventory = dr["isLabInventory"].ToString();
        //        ispmtlnventory = dr["isPMTInventory"].ToString();


        //        var checkflag = "";
        //        //var labflag = "";
        //        //var pmtflag = "";

        //        //if (islablnventory == "1" )
        //        //{
        //        //    labflag = "1";
        //        //}
        //        //else
        //        //{
        //        //    labflag = "0";
        //        ////}
        //        //if (todelete == "1" && tomodify == "1")
        //        //{
        //        //    checkflag = "1";
        //        //}
        //        //else if (todelete == "0" && tomodify == "1")
        //        //{
        //        //    checkflag = "2";
        //        //}
        //        return Json(new
        //        {
        //            data = todelete,tomodify,checkflag,islablnventory,ispmtlnventory,
        //            success = true,
        //        }, JsonRequestBehavior.AllowGet);

        //    }
        //    else
        //    {
        //        NTID = "";
        //        return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        //    }
        //    dr.Close();
        //    CloseConnection();

        //}

        private SqlConnection conn,bookingconn;
       /// <summary>
       /// Establishing database connections and common functions for handling connection open and close
       /// </summary>
        private void connection()
        {

            string connString = ConfigurationManager.ConnectionStrings["BudgetingdbConnection"].ToString();
            conn = new SqlConnection(connString);
            string bookingconnString = ConfigurationManager.ConnectionStrings["dbConnection"].ToString();
            bookingconn = new SqlConnection(bookingconnString);
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

        private void OpenConnection_BookingServer()
        {
            if (bookingconn.State == ConnectionState.Closed)
            {
                bookingconn.Open();
            }
        }

        private void CloseConnection_BookingServer()
        {
            if (bookingconn.State == ConnectionState.Open)
            {
                bookingconn.Close();
            }
        }

        //To Get data and display in Hardware inventory
        public ActionResult GetHWData()
        {
        
            List<WW_HardwareInventory> viewList = new List<WW_HardwareInventory>();
            viewList = GetHWData1();
            return Json(new
            {
                success = true,
                data = viewList
            }, JsonRequestBehavior.AllowGet);
        }

        //List for storing the retrieved data to display in HW Inventory
        public List<WW_HardwareInventory> GetHWData1()
        {
            List<WW_HardwareInventory> viewList = new List<WW_HardwareInventory>();

            DataTable dt = new DataTable();
            try
            {

                connection();

                string Query = "Select [dbo].[HWInventory].*,[dbo].[HILL_List].HILGeneration, [dbo].[HILL_List].ACInputType, "+
                    "[dbo].[HILL_List].Type, "+
                    "[dbo].[HILL_List].MonitorAssetNumber1, " +
                    "[dbo].[HILL_List].MonitorAssetNumber2, "+
                    "[dbo].[HILL_List].PCAssetNumber from   "+
                    "[dbo].[HWInventory] left join[dbo].[HILL_List] on "+
                    "[dbo].[HILL_List].HIL_ID = [dbo].[HWInventory].HIL_ID  order by HW_ID ";

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
                WW_HardwareInventory item = new WW_HardwareInventory();
                item.HW_ID = int.Parse(row["HW_ID"].ToString());
                item.BU = int.Parse(row["BU"].ToString());
                item.OEM = int.Parse(row["OEM"].ToString());
                item.HW_Type = int.Parse(row["HW_Type"].ToString());
                if (row["UOM"].ToString().Trim() != "" && row["UOM"].ToString().Trim() != "0")
                    item.UOM = Convert.ToInt32(row["UOM"]);
                else
                    item.UOM = null;
                if (row["InventoryType"].ToString().Trim() != "" && row["InventoryType"].ToString().Trim() != "0")
                    item.InventoryType = Convert.ToInt32(row["InventoryType"]);
                else
                    item.InventoryType = null;
                item.SerialNumber = row["SerialNumber"].ToString();
                item.BondNumber = row["BondNumber"].ToString();
                item.BondDate = row["BondDate"].ToString() != "" ? ((DateTime)row["BondDate"]).ToString("yyyy-MM-dd") : string.Empty;
                
                item.AssetNumber = row["AssetNumber"].ToString();
                if (row["Mode"].ToString().Trim() != "" && row["Mode"].ToString().Trim() != "0")
                    item.Mode = Convert.ToInt32(row["Mode"]);
                else
                    item.Mode = null;
                item.Usage = int.Parse(row["Usage"].ToString());
                item.Remarks = row["Remarks"].ToString();
               
                if (row["OtherPlace_ID"].ToString() != "")
                    item.OtherPlace_ID = Convert.ToInt32(row["OtherPlace_ID"]);
                if (row["HIL_ID"].ToString() != "")
                    item.HIL_ID = Convert.ToInt32(row["HIL_ID"]);
                if (row["Diagnostic_HIL_ID"].ToString() != "")
                    item.Diagnostics_HIL_ID = Convert.ToInt32(row["Diagnostics_HIL_ID"]);
                if(row["Location"].ToString().Trim() != "" && row["Location"].ToString().Trim() != "0")
                    item.Location = int.Parse(row["Location"].ToString()) ;

                item.BU_Name = (row["BU_Name"].ToString());
                item.HW_Type_Name = (row["HW_Type_Name"].ToString());
                item.InventoryType_Name = (row["InventoryType_Name"].ToString());
                item.Location_Name = (row["Location_Name"].ToString());
                item.Mode_Name = (row["Mode_Name"].ToString());
                item.OEM_Name = (row["OEM_Name"].ToString());
                item.UOM_Name = (row["UOM_Name"].ToString());
                item.Usage_Name = (row["Usage_Name"].ToString());

                //item.HIL_Name = (row["HIL_Name"].ToString());
                if (row["Type"].ToString().Trim() != "" && row["Type"].ToString().Trim() != "0")
                    item.Type = int.Parse(row["Type"].ToString());
                item.PC_Asset_Number = (row["PCAssetNumber"].ToString());
                item.Monitor_Asset_Number1 = (row["MonitorAssetNumber1"].ToString());
                item.Monitor_Asset_Number2 = (row["MonitorAssetNumber2"].ToString());
                if (row["ACInputType"].ToString().Trim() != "" && row["ACInputType"].ToString().Trim() != "0")
                    item.AC_InputType = int.Parse(row["ACInputType"].ToString());
                if (row["HILGeneration"].ToString().Trim() != "" && row["HILGeneration"].ToString().Trim() != "0")
                    item.HIL_Generation = int.Parse(row["HILGeneration"].ToString());
               
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

        //Result for getting data from Whereabout (Usage) table
        public ActionResult GetWhereabout()
        {
            List<WhereaboutInventory_class> usageList = new List<WhereaboutInventory_class>();
            DataTable dt = new DataTable();
            try
            {

                connection();

                string Query = " Select  [ID]" +
          ",[Usage] " +
          "from [Whereabout] order by ID";

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
                WhereaboutInventory_class item = new WhereaboutInventory_class();
                item.ID = int.Parse(row["ID"].ToString());
                item.Usage = row["Usage"].ToString();

                usageList.Add(item);
            }
            return Json(new { data = usageList }, JsonRequestBehavior.AllowGet);
        }

        //Result for getting data from Location table
        public ActionResult GetLocation()
        {
            List<SiteInventory_class> siteList = new List<SiteInventory_class>();
            DataTable dt = new DataTable();
            try
            {
                connection();
                string Query = " Select  [ID]" +
                  ",[RBCode] " +
                  "from [dbo].[Sites] order by ID";

                OpenConnection_BookingServer();
                SqlCommand cmd = new SqlCommand(Query, bookingconn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                CloseConnection_BookingServer();
            }
            catch (Exception e)
            {
            }
            foreach (DataRow row in dt.Rows)
            {
                SiteInventory_class item = new SiteInventory_class();
                item.ID = int.Parse(row["ID"].ToString());
                item.Location = row["RbCode"].ToString();
                siteList.Add(item);
            }
            return Json(new { data = siteList }, JsonRequestBehavior.AllowGet);
        }

        //Result for getting data from Unit of Measure table
        public ActionResult GetUOM()
        {
            List<UOMBGSW> uomList = new List<UOMBGSW>();
            DataTable dt = new DataTable();
            try
            {
                connection();

                string Query = " Select  [ID]" +
          ",[Units] " +
          "from [BGSW_UOM_Table] order by ID";

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
                UOMBGSW item = new UOMBGSW();
                item.ID = int.Parse(row["ID"].ToString());
                item.UOM = row["Units"].ToString();

                uomList.Add(item);
            }
            return Json(new { data = uomList }, JsonRequestBehavior.AllowGet);
        }

        //Result for getting data from PlaceType table
        public ActionResult GetPlaceType()
        {
            List<PlaceTypeInventory_class> placeTypeList = new List<PlaceTypeInventory_class>();
            DataTable dt = new DataTable();
            try
            {
                connection();

                string Query = " Select  [ID]" +
          ",[PlaceType] " +
          "from [Place_Type] order by ID";

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
                PlaceTypeInventory_class item = new PlaceTypeInventory_class();
                item.ID = int.Parse(row["ID"].ToString());
                item.PlaceType = row["PlaceType"].ToString();

                placeTypeList.Add(item);
            }
            return Json(new { data = placeTypeList }, JsonRequestBehavior.AllowGet);
        }

        //Result for getting data from LabType table
        public ActionResult GetLabType()
        {
            List<LabTypeInventory_class> placeTypeList = new List<LabTypeInventory_class>();
            DataTable dt = new DataTable();
            try
            {
                connection();

                string Query = " Select  [ID]" +
          ",[DisplayName] " +
          "from LabTypes where ID in (8,9,57,58) order by DisplayName";

                OpenConnection();
                SqlCommand cmd = new SqlCommand(Query, bookingconn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                CloseConnection();
            }
            catch (Exception e)
            {

            }
            foreach (DataRow row in dt.Rows)
            {
                LabTypeInventory_class item = new LabTypeInventory_class();
                item.ID = int.Parse(row["ID"].ToString());
                item.LabType = row["DisplayName"].ToString();

                placeTypeList.Add(item);
            }
            return Json(new { data = placeTypeList }, JsonRequestBehavior.AllowGet);
        }

        //Result for getting data from HilGeneration table
        public ActionResult GetHILGeneration()
        {
            List<HILGenerationInventory_class> hilGenerationList = new List<HILGenerationInventory_class>();
            DataTable dt = new DataTable();
            try
            {
                connection();

                string Query = " Select  [ID]" +
          ",[HILGeneration] " +
          "from HILGeneration_table";

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
                HILGenerationInventory_class item = new HILGenerationInventory_class();
                item.ID = int.Parse(row["ID"].ToString());
                item.HILGeneration = row["HILGeneration"].ToString();

                hilGenerationList.Add(item);
            }
            return Json(new { data = hilGenerationList }, JsonRequestBehavior.AllowGet);
        }

        //Result for getting data from ACInputType table
        public ActionResult GetACInputType()
        {
            List<ACInputTypeInventory_class> acInputTypeList = new List<ACInputTypeInventory_class>();
            DataTable dt = new DataTable();
            try
            {
                connection();

                string Query = " Select  [ID]" +
          ",[ACInputType] " +
          "from [ACInputType_Table] ";

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
                ACInputTypeInventory_class item = new ACInputTypeInventory_class();
                item.ID = int.Parse(row["ID"].ToString());
                item.ACInputType = row["ACInputType"].ToString();

                acInputTypeList.Add(item);
            }
            return Json(new { data = acInputTypeList }, JsonRequestBehavior.AllowGet);
        }


        //Result for getting data on WW LabNames for different locations which belongs to CCHIL and ET Lab Types
        public ActionResult GetLabNames()
        {
            List<LabNameInventory_class> labList = new List<LabNameInventory_class>();
            DataTable dt = new DataTable();
            try
            {
                connection();
                string Query = "select BookingServerReplica.dbo.LabInfo.TypeId as [Type], dbo.HILL_List.PCAssetNumber, dbo.HILL_List.MonitorAssetNumber1, dbo.HILL_List.MonitorAssetNumber2, " +
                    "dbo.HILL_List.ACInputType, dbo.HILL_List.HILGeneration, BookingServerReplica.dbo.labinfo.ID,BookingServerReplica.dbo.sites.ID as SiteID,RBCode,BookingServerReplica.dbo.labinfo.DisplayName as LabName from BookingServerReplica.dbo.sites " +
                    "inner join BookingServerReplica.dbo.locations on BookingServerReplica.dbo.sites.id = BookingServerReplica.dbo.locations.siteid "+
                    "inner join BookingServerReplica.dbo.labinfo on BookingServerReplica.dbo.labinfo.locationid = BookingServerReplica.dbo.locations.id " +
                    "left join dbo.HILL_List on dbo.HILL_List.HIL_ID = BookingServerReplica.dbo.labinfo.ID " +
                    "where typeid in (8, 9, 57, 58) order by LabName";
                //58 - ccsil, 8 - cchil, 9 - et, 57 - otb
                OpenConnection_BookingServer();
                SqlCommand cmd = new SqlCommand(Query, conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                CloseConnection_BookingServer();
            }
            catch (Exception e)
            {
            }
            foreach (DataRow row in dt.Rows)
            {
                LabNameInventory_class item = new LabNameInventory_class();
                item.ID = int.Parse(row["ID"].ToString());
                item.SiteID = int.Parse(row["SiteID"].ToString());
                item.LabName = row["LabName"].ToString();
                if (row["Type"].ToString().Trim() != "" && row["Type"].ToString().Trim() != "0")
                item.Type                   = int.Parse(row["Type"].ToString());
                item.PC_Asset_Number         = (row["PCAssetNumber"].ToString());
                item.Monitor_Asset_Number1 = (row["MonitorAssetNumber1"].ToString());
                item.Monitor_Asset_Number2 = (row["MonitorAssetNumber2"].ToString());
                if (row["ACInputType"].ToString().Trim() != "" && row["ACInputType"].ToString().Trim() != "0")
                    item.AC_InputType            = int.Parse(row["ACInputType"].ToString());
                if (row["HILGeneration"].ToString().Trim() != "" && row["HILGeneration"].ToString().Trim() != "0")
                    item.HIL_Generation         = int.Parse(row["HILGeneration"].ToString());

                labList.Add(item);
            }
            return Json(new { data = labList }, JsonRequestBehavior.AllowGet);
        }


        //Action Result for saving the details after adding or editing

        [HttpPost]
        public ActionResult AddOrEdit(WW_HardwareInventory req)
        {
            DataTable dt = new DataTable();

            connection();
            dt = new DataTable();
            string Query = "";
            var UserNTID = System.Web.HttpContext.Current.User.Identity.Name.Split('\\')[1].ToUpper();


            Query = " Exec [dbo].[Add_WWHWInventory] '" + req.BU + "', '" + req.OEM + "', '" + req.HW_Type + "', '" + req.UOM
                + "', '" + req.InventoryType + "', '" + req.SerialNumber + "', '" + req.BondNumber
                + "', '" + req.BondDate + "', '" + req.AssetNumber + "', '" + req.Mode
                + "', '" + req.Remarks + "', '" + req.Usage + "', '" + req.Location + "', '" + req.HW_ID
                 + "', '" + req.Receiver + "', '" + req.RxDept + "', '" + (req.Start_date!=null ? DateTime.Parse(new string(req.Start_date.Take(24).ToArray())).ToString("yyyy-MM-dd") : req.Start_date)
                + "', '" + (req.End_date != null ? DateTime.Parse(new string(req.End_date.Take(24).ToArray())).ToString("yyyy-MM-dd") : req.End_date) + "', '" + (req.Planned_end_date != null ? DateTime.Parse(new string(req.Planned_end_date.Take(24).ToArray())).ToString("yyyy-MM-dd") : req.Planned_end_date) + "', '" + req.Place_type + "', '" + req.Info + "', '" + UserNTID
               
                 + "', '" + req.HIL_ID + "', '" + req.Type
                  + "', '" + req.PC_Asset_Number + "', '" + req.Monitor_Asset_Number1 + "', '" + req.Monitor_Asset_Number2 + "', '" + req.AC_InputType
                 + "', '" + req.HIL_Generation //+ "', '" + req.RxDept
                + "' ";
           

            try
            {
                connection();
                OpenConnection();
                SqlCommand cmd = new SqlCommand(Query, conn);
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

        public ActionResult GetLog(int HW_ID)
        {
             
            List<WW_HardwareInventory> OtherPlace_viewList = new List<WW_HardwareInventory>();
            List<WW_HardwareInventory> HIL_viewList = new List<WW_HardwareInventory>();
            DataSet ds = new DataSet();
            try
            {

                connection();

                string Query = " Select  * from [OtherPlace_List] where HW_ID = " + HW_ID + " order by OtherPlaceID;"
                 + " Select * from HILL_List where HIL_ID = (Select HIL_ID from [HWInventory] where HW_ID = " + HW_ID + " )";

                OpenConnection();
                SqlCommand cmd = new SqlCommand(Query, conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                CloseConnection();
            }
            catch (Exception e)
            {

            }

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                WW_HardwareInventory item = new WW_HardwareInventory();



                item.Receiver = row["ReceiverName"].ToString();
                item.RxDept = row["ReceiverDept"].ToString();
                item.Start_date = row["Start_Date"].ToString() != "" ? ((DateTime)row["Start_Date"]).ToString("yyyy-MM-dd") : string.Empty;
                item.End_date = row["End_Date"].ToString() != "" ? ((DateTime)row["End_Date"]).ToString("yyyy-MM-dd") : string.Empty;
                item.Planned_end_date = row["Planned_End_Date"].ToString() != "" ? ((DateTime)row["Planned_End_Date"]).ToString("yyyy-MM-dd") : string.Empty;
                item.Place_type = int.Parse(row["Placetype"].ToString());
                item.Info = row["Info"].ToString();
                item.Updated_By = row["Updated_By"].ToString();


                OtherPlace_viewList.Add(item);
            }
            foreach (DataRow row in ds.Tables[1].Rows)
            {
                WW_HardwareInventory item = new WW_HardwareInventory();

                item.HIL_Name = (row["HIL_Name"].ToString());
                if (row["Type"].ToString().Trim() != "" && row["Type"].ToString().Trim() != "0")
                    item.Type = int.Parse(row["Type"].ToString());
                item.PC_Asset_Number = (row["PCAssetNumber"].ToString());
                item.Monitor_Asset_Number1 = (row["MonitorAssetNumber1"].ToString());
                item.Monitor_Asset_Number2 = (row["MonitorAssetNumber2"].ToString());
                if (row["ACInputType"].ToString().Trim() != "" && row["ACInputType"].ToString().Trim() != "0")
                    item.AC_InputType = int.Parse(row["ACInputType"].ToString());
                if (row["HILGeneration"].ToString().Trim() != "" && row["HILGeneration"].ToString().Trim() != "0")
                    item.HIL_Generation = int.Parse(row["HILGeneration"].ToString());


                HIL_viewList.Add(item);
            }
            return Json(new
            {
                success = true,
                data = OtherPlace_viewList
                ,data1 = HIL_viewList
            }, JsonRequestBehavior.AllowGet);
        }

        //List for storing the retrieved data to display

        //public List<WW_HardwareInventory> GetLog1(int HW_ID)
        //{
        //    List<WW_HardwareInventory> viewList = new List<WW_HardwareInventory>();

        //    DataSet ds = new DataSet();
        //    try
        //    {

        //        connection();

        //        string Query = " Select  * from [OtherPlace_List] where HW_ID = " + HW_ID + " order by OtherPlaceID;" +
        //        " Select * from HILL_List where HIL_ID = (Select HIL_ID from [HWInventory] where HW_ID = " + HW_ID + " )";

        //        OpenConnection();
        //        SqlCommand cmd = new SqlCommand(Query, conn);
        //        SqlDataAdapter da = new SqlDataAdapter(cmd);
        //        da.Fill(ds);
        //        CloseConnection();
        //    }
        //    catch (Exception e)
        //    {

        //    }

        //    foreach (DataRow row in ds.Tables[0].Rows)
        //    {
        //        WW_HardwareInventory item = new WW_HardwareInventory();
               


        //        item.Receiver = row["ReceiverName"].ToString();
        //        item.RxDept = row["ReceiverDept"].ToString();
        //        item.Start_date = row["Start_Date"].ToString() != "" ? ((DateTime)row["Start_Date"]).ToString("yyyy-MM-dd") : string.Empty;
        //        item.End_date = row["End_Date"].ToString() != "" ? ((DateTime)row["End_Date"]).ToString("yyyy-MM-dd") : string.Empty;
        //        item.Planned_end_date = row["Planned_End_Date"].ToString() != "" ? ((DateTime)row["Planned_End_Date"]).ToString("yyyy-MM-dd") : string.Empty;
        //        item.Place_type = int.Parse(row["Placetype"].ToString());
        //        item.Info = row["Info"].ToString();
        //        item.Updated_By = row["Updated_By"].ToString();


        //        viewList.Add(item);
        //    }
        //    foreach (DataRow row in ds.Tables[1].Rows)
        //    {
        //        WW_HardwareInventory item = new WW_HardwareInventory();

        //        item.HIL_Name = (row["HIL_Name"].ToString());
        //        if (row["Type"].ToString().Trim() != "" && row["Type"].ToString().Trim() != "0")
        //            item.Type = int.Parse(row["Type"].ToString());
        //        item.PC_Asset_Number = (row["PCAssetNumber"].ToString());
        //        item.Monitor_Asset_Number1 = (row["MonitorAssetNumber1"].ToString());
        //        item.Monitor_Asset_Number2 = (row["MonitorAssetNumber2"].ToString());
        //        item.ACInputType = (row["ACInputType"].ToString());
        //        item.HIL_Generation = (row["HILGeneration"].ToString());


        //        viewList.Add(item);
        //    }
        //    return viewList;
        //}

        [HttpPost]
        public ActionResult GetUserSettings(string FormName)
        {

            string JSONresult = "";
            DataTable dt = new DataTable();
            try
            {
                connection();
                string Query = " Select FormName,ColumnName, Visibility from Inventory_UserSettings Where NTID = '" + User.Identity.Name.Split('\\')[1].ToUpper().ToString() + "' and FormName = '" + FormName + "' order by ID ";

                OpenConnection();
                SqlCommand cmd = new SqlCommand(Query, conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                CloseConnection();

                //foreach (DataRow row in dt.Rows)
                //{
                //    HWSettings item = new HWSettings();
                //    item.FormName = row["FormName"].ToString();
                //    item.ColumnName = row["ColumnName"].ToString();
                //    item.Visibility = int.Parse(row["Visibility"].ToString());

                //    viewList.Add(item);
                //}


                JSONresult = JsonConvert.SerializeObject(dt);
                return Json(new { success = true, data = JSONresult }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                CloseConnection();
                return Json(new { success = false, data = JSONresult }, JsonRequestBehavior.AllowGet);
            }



        }

        [HttpPost]
        public ActionResult GetColumnValues(string FormName)
        {

            //List<HWSettings> viewList = new List<HWSettings>();
            string JSONresult = "";
            DataTable dt = new DataTable();
            int isDBValue = 0;
            try
            {
                connection();
                string Query = " Select FormName,ColumnName, case when Visibility = 0 then convert(bit,Visibility) when Visibility = 1 then convert(bit,Visibility) else convert(bit,Visibility) end as Visibility from Inventory_UserSettings Where NTID = '" + User.Identity.Name.Split('\\')[1].ToUpper().ToString() + "' and FormName = '" + FormName + "' order by ID ";

                OpenConnection();
                SqlCommand cmd = new SqlCommand(Query, conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                CloseConnection();

                //foreach (DataRow row in dt.Rows)
                //{
                //    HWSettings item = new HWSettings();
                //    item.FormName = row["FormName"].ToString();
                //    item.ColumnName = row["ColumnName"].ToString();
                //    item.Visibility = int.Parse(row["Visibility"].ToString());

                //    viewList.Add(item);
                //}
                if (dt.Rows.Count > 0)
                    isDBValue = 1;
                else
                    isDBValue = 0;

                JSONresult = JsonConvert.SerializeObject(dt);
                return Json(new { success = true, data = JSONresult, isDBValue = isDBValue }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                CloseConnection();
                return Json(new { success = false, data = JSONresult }, JsonRequestBehavior.AllowGet);
            }


        }

        [HttpPost]
        public ActionResult SaveUserSettings(List<HWSettings> items) // Save UserSettings
        {
            DataTable dt = new DataTable();
            try
            {
                dt.Columns.Add("FormName", typeof(string));
                dt.Columns.Add("Column", typeof(String));
                dt.Columns.Add("Visibility", typeof(int));

                DataRow row1;
                for (int i = 0; i < items.Count; i++)
                {
                    row1 = dt.NewRow();
                    row1[0] = items[i].FormName.ToString();
                    row1[1] = items[i].ColumnName;
                    row1[2] = items[i].Visibility;
                    dt.Rows.Add(row1);
                }



                connection();

                // Stored Procedure call

                SqlCommand command = new SqlCommand();

                command.Connection = conn;
                command.CommandText = "dbo.[SaveUserSettings]";
                command.CommandType = CommandType.StoredProcedure;

                SqlParameter parameter1 = new SqlParameter();
                parameter1.ParameterName = "@UserSettings";
                parameter1.SqlDbType = SqlDbType.Structured;
                parameter1.TypeName = "dbo.UserSettings";
                parameter1.Direction = ParameterDirection.Input;
                parameter1.Value = dt;

                SqlParameter parameter2 = new SqlParameter();
                parameter2.ParameterName = "@UserNTID";
                parameter2.SqlDbType = SqlDbType.NVarChar;
                parameter2.Direction = ParameterDirection.Input;
                parameter2.Value = User.Identity.Name.Split('\\')[1].ToUpper().ToString();

                command.Parameters.Add(parameter1);
                command.Parameters.Add(parameter2);

                OpenConnection();
                command.CommandTimeout = 300; //5 min
                command.ExecuteNonQuery();

                CloseConnection();


                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult GetBookMarksDetails(string formName)
        {
            List<WW_HardwareInventoryBookMarks> bmList = new List<WW_HardwareInventoryBookMarks>();
            DataTable dt = new DataTable();
            try
            {

                connection();
                //EXEC UpdateBookMarks 0,'BVQ1COB','HW_inventory','BM1','BU:1,LOCATION:COB',1,'Insert'
                string Query = "Select * from Inventory_UserBookmarks where NTID='" + User.Identity.Name.Split('\\')[1].Trim().ToUpper() + "' and FormName='" + formName + "' order by isDefault DESC";

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
                WW_HardwareInventoryBookMarks item = new WW_HardwareInventoryBookMarks();
                item.ID = int.Parse(row["ID"].ToString());
                item.NTID = row["NTID"].ToString();
                item.FormName = row["FormName"].ToString();
                item.BookmarkName = row["BookmarkName"].ToString();
                item.BookmarkValue = row["BookmarkValue"].ToString();
                item.DefaultValue = row["isDefault"].ToString();
                bmList.Add(item);
            }
            return Json(new { data = bmList }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddorRemoveBookMarksDetails(string details, string bmName, bool defaultValue,string formName)
        {
            DataTable dt = new DataTable();
            connection();
            dt = new DataTable();
            string Query = "";
            System.Security.Principal.IPrincipal User = System.Web.HttpContext.Current.User;
            OpenConnection();
            if (defaultValue)
            {
                string QueryBM = "Update [Inventory_UserBookmarks] set [isDefault]=0 where NTID='" + User.Identity.Name.Split('\\')[1].Trim().ToUpper() + "'";
                SqlCommand cmd2 = new SqlCommand(QueryBM, conn);                
                cmd2.ExecuteNonQuery();
            }
            else
            {
                string checkBMExist = "Select * from Inventory_UserBookmarks where NTID='" + User.Identity.Name.Split('\\')[1].Trim().ToUpper() + "' and FormName='" + formName + "' order by isDefault DESC";
                SqlCommand checkBMExistcmd = new SqlCommand(checkBMExist, conn);
                SqlDataAdapter da = new SqlDataAdapter(checkBMExistcmd);
                da.Fill(dt);
                if(dt.Rows.Count == 0)
                {
                    defaultValue = true;
                }
            }
            //Insert query 
            Query = "INSERT into[Inventory_UserBookmarks]" +
                      "(" +
                       "[NTID]" +
                       ",[FormName]" +
                       ",[BookmarkName]" +
                       ",[BookmarkValue]" +
                       ",[isDefault]" +
                       ")" +
                       "values" +
                       "(" +
                       " @NTID  " +
                       ", @FormName  " +
                       ", @BookmarkName" +
                       ", @BookmarkValue" +
                       ", @Default " +
                       ")";

            // }


            SqlCommand cmd = new SqlCommand(Query, conn);
            cmd.Parameters.AddWithValue("@NTID", User.Identity.Name.Split('\\')[1].Trim().ToUpper());
            cmd.Parameters.AddWithValue("@FormName", "HW_Inventory");
            cmd.Parameters.AddWithValue("@BookmarkName", bmName);
            cmd.Parameters.AddWithValue("@BookmarkValue", details);
            cmd.Parameters.AddWithValue("@Default", defaultValue);
            try
            {
                OpenConnection();
                cmd.ExecuteNonQuery();
                Console.WriteLine("Records Updated Successfully");
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
            finally
            {
                CloseConnection();
            }

            //} 

        }

        [HttpPost]
        public ActionResult DeleteBookMarks(int id)
        {
            DataTable dt = new DataTable();

            connection();

            dt = new DataTable();
            string Query = "";
            System.Security.Principal.IPrincipal User = System.Web.HttpContext.Current.User;

            Query = "Delete from [Inventory_UserBookmarks] where ID = @Id";


            SqlCommand cmd = new SqlCommand(Query, conn);
            cmd.Parameters.AddWithValue("@Id", id);
            try
            {
                OpenConnection();
                cmd.ExecuteNonQuery();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
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


        //Creating Lookup list and getting data from various tables in the Lookup for dropdown

        public ActionResult Lookup()
        {
            using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())
            {

                DataSet ds = new DataSet();
                connection();
                OpenConnection();
                string Query = " Select * from BU_Table; Select * from OEM_Table; Select * from Groups_Table_Test; Select * from ItemsCostList_Table where BU = 3 and Deleted = 0; Select * from Currency_Table; ";
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

                //lookupData.Groups_test = new List<Groups_Table_Test>();
                //foreach (DataRow row in ds.Tables[2].Rows)
                //{
                //    Groups_Table_Test item = new Groups_Table_Test();
                //    item.ID = Convert.ToInt32(row["ID"]);
                //    item.Group = row["Group"].ToString();
                //    item.Dept = Convert.ToInt32(row["Dept"]);
                //    item.Outdated = Convert.ToBoolean(row["Outdated"]);
                //    item.Rep_ID = row["Rep_ID"].ToString() != "" ? Convert.ToInt32(row["Rep_ID"]) : 0;
                //    lookupData.Groups_test.Add(item);
                //}


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

                //lookupData.Currency_List = new List<Currency_Table>();
                //foreach (DataRow row in ds.Tables[4].Rows)
                //{
                //    Currency_Table item = new Currency_Table();
                //    item.ID = Convert.ToInt32(row["ID"]);
                //    item.Currency = row["Currency"].ToString();
                //    lookupData.Currency_List.Add(item);
                //}

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

                //foreach (DataRow item in dt_for_headerRow.Tables[2].Rows)
                //{
                //    Inventory_HeaderFilter_Table i_header = new Inventory_HeaderFilter_Table();
                //    i_header.text = item["Group"].ToString();
                //    i_header.value = Convert.ToInt32(item["ID"].ToString());
                //    lookupData.Group_HeaderFilter.Add(i_header);
                //}

                foreach (DataRow item in dt_for_headerRow.Tables[3].Rows)
                {
                    Inventory_HeaderFilter_Table i_header = new Inventory_HeaderFilter_Table();
                    i_header.text = item["Item Name"].ToString();
                    i_header.value = Convert.ToInt32(item["S#No"].ToString());
                    lookupData.InventoryType_HeaderFilter.Add(i_header);
                }

                //foreach (DataRow item in dt_for_headerRow.Tables[4].Rows)
                //{
                //    Inventory_HeaderFilter_Table i_header = new Inventory_HeaderFilter_Table();
                //    i_header.text = item["Item Name"].ToString();
                //    i_header.value = Convert.ToInt32(item["S#No"].ToString());
                //    lookupData.SpareHW_HeaderFilter.Add(i_header);
                //}

                return Json(new { data = lookupData }, JsonRequestBehavior.AllowGet);

            }
        }


        [HttpPost]
        public ActionResult Index_import(HttpPostedFileBase postedFile)
        {
            int save = 0;
            string ErrorMsg = "";
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
                        //}

                        int errcount = 0;

                        System.Security.Principal.IPrincipal User = System.Web.HttpContext.Current.User;

                        using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

                        {
                            //string presentUserName = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).EmployeeName;
                            //decimal conversionINRate = db.Currency_Conversion_Table.FirstOrDefault(x => x.Currency.Contains("INR")).ConversionRate_to_USD;
                            //decimal conversionEURate = db.Currency_Conversion_Table.FirstOrDefault(x => x.Currency.Contains("EUR")).ConversionRate_to_USD;
                            string NTID = User.Identity.Name.Split('\\')[1].ToUpper().ToString();
                            DataTable dt1 = dt;
                            DateTime defaultDate = new DateTime(1900, 01, 01);
                            //dt1.Rows[0].Delete();
                            //dt1.AcceptChanges();

                            //WriteLog("header 1 deleted");
                            DataTable dt_new = new DataTable();
                            DataTable dt_grpeddetails = new DataTable();
                            //WriteLog("new datatable created; adding columns .....");


                            dt_new.Columns.Add("BU", typeof(string));
                            dt_new.Columns.Add("OEM", typeof(string));
                            dt_new.Columns.Add("Location", typeof(string));
                            dt_new.Columns.Add("HardwareType", typeof(string));
                            dt_new.Columns.Add("InventoryType", typeof(string));
                            dt_new.Columns.Add("UOM", typeof(string));
                            dt_new.Columns.Add("SerialNumber", typeof(string));
                            dt_new.Columns.Add("BondNumber", typeof(string));
                            dt_new.Columns.Add("BondDate", typeof(string));
                            dt_new.Columns.Add("AssetNumber", typeof(string));
                            dt_new.Columns.Add("Mode", typeof(string));
                            dt_new.Columns.Add("Remarks", typeof(string));
                            dt_new.Columns.Add("Usage", typeof(string));
                            dt_new.Columns.Add("HWResponsible", typeof(string));
                            dt_new.Columns.Add("HandOverTo", typeof(string));

                            dt_new.Columns.Add("Quantity", typeof(float));
                            dt_new.Columns.Add("UserNTID", typeof(string));



                            //WriteLog("columns added ; copying data ...");
                            DataRow row1;
                            //WriteLog("rows count" + dt1.Rows.Count);
                            //WriteLog("[0][0] val: " + dt1.Rows[0][0].ToString());
                            //WriteLog("[0][1] val: " + dt1.Rows[0][1].ToString());
                            for (int i = 0; i < dt1.Rows.Count; i++)
                            {
                                //WriteLog("copying data : ..." + i);
                                row1 = dt_new.NewRow();


                                row1[0] = dt1.Rows[i][0].ToString();
                                row1[1] = dt1.Rows[i][1].ToString();
                                row1[2] = dt1.Rows[i][2].ToString();
                                row1[3] = dt1.Rows[i][3].ToString();
                                row1[4] = dt1.Rows[i][4].ToString();
                                row1[5] = dt1.Rows[i][5].ToString();
                                row1[6] = dt1.Rows[i][6].ToString();
                                row1[7] = dt1.Rows[i][7].ToString();
                                row1[8] = (dt1.Rows[i][8].ToString().Trim() == "") ? string.Empty : dt1.Rows[i][8].ToString().Replace(".", "/");
                                row1[9] = dt1.Rows[i][9].ToString();
                                row1[10] = dt1.Rows[i][10].ToString();
                                row1[11] = dt1.Rows[i][11].ToString();
                                row1[12] = dt1.Rows[i][12].ToString();
                                row1[13] = dt1.Rows[i][13].ToString();
                                row1[14] = dt1.Rows[i][14].ToString();
                                row1[15] = (dt1.Rows[i][15] == DBNull.Value) ? 0 : Convert.ToDouble(dt1.Rows[i][15]);
                                row1[16] = dt1.Rows[i][16].ToString();

                                dt_new.Rows.Add(row1);
                            }

                            connection();

                            SqlCommand command = new SqlCommand();

                            command.Connection = conn;
                            command.CommandText = "dbo.[Import_WWHWInventory]";
                            command.CommandType = CommandType.StoredProcedure;


                            // Add the input parameter and set its properties.

                            SqlParameter parameter2 = new SqlParameter();
                            parameter2.ParameterName = "@NTID";
                            parameter2.SqlDbType = SqlDbType.NVarChar;
                            parameter2.Direction = ParameterDirection.Input;
                            parameter2.Value = NTID;


                            SqlParameter parameter1 = new SqlParameter();
                            parameter1.ParameterName = "@HWInventory";
                            parameter1.SqlDbType = SqlDbType.Structured;
                            parameter1.TypeName = "dbo.HWInventory";
                            parameter1.Direction = ParameterDirection.Input;
                            parameter1.Value = dt_new;

                            // Add the parameter to the Parameters collection.
                            command.Parameters.Add(parameter1);
                            command.Parameters.Add(parameter2);

                            OpenConnection();
                            //WriteLog("Executing STORED PROCEDURE");
                            command.CommandTimeout = 300; //5 min

                            //ErrorMsg = command.ExecuteScalar().ToString();
                            //WriteLog("ErrorMsg: " + ErrorMsg);
                            command.ExecuteNonQuery();

                            command = new SqlCommand("select top 1  convert(nvarchar(100),Msg) as ErrorMsg from HWInventoryLog where Msg like '%Error Msg%' order by logtime desc", conn);


                            SqlDataAdapter da = new SqlDataAdapter(command);
                            DataTable dt2 = new DataTable();
                            da.Fill(dt2);
                            CloseConnection();
                            //string ErrorMsg = new List<string>();
                            List<string> ErrorMsg1 = new List<string>();
                            foreach (DataRow item in dt2.Rows)
                            {
                                try
                                {

                                    string err = Convert.ToString(item["ErrorMsg"]);
                                    ErrorMsg1.Add(err);

                                }
                                catch (Exception ex)
                                {

                                }

                            }
                            if (ErrorMsg1.Count() > 0)
                            {
                                //WriteLog("ErrorMsg: " + ErrorMsg1[0]);
                                ErrorMsg = ErrorMsg1[0];
                            }

                            CloseConnection();


                            //WriteLog("before groupby");

                            //WriteLog("after groupby");


                        }
                    }
                }
                if (ErrorMsg == "")
                    return Json(new { success = true, save, errormsg = ErrorMsg }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { success = false, save, errormsg = ErrorMsg }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //WriteErrorLog(ex.Message);

                //WriteLog("Error - Index Import : NTID - " + User.Identity.Name.Split('\\')[1].Trim().ToUpper() + ", ErrorMsg : " + ex + ", At - " + DateTime.Now);

                return Json(new { success = false, errormsg = ex.Message.ToString() });
            }
            finally
            {

            }

        }

        public static void WriteLog(string Message)
        {
            string execPath = AppDomain.CurrentDomain.BaseDirectory;
            execPath = execPath + "Inventory_Log\\log" + DateTime.Now.ToString("dd_MM_yyyy") + ".txt";
            StreamWriter file = new StreamWriter(execPath, append: true);
            file.WriteLine(Message + "\r\n");
            file.Close();
        }



        //delete function
        //[HttpPost]
        //public ActionResult Delete(int id)
        //{
        //    DataTable dt = new DataTable();

        //    connection();

        //    dt = new DataTable();
        //    string Query = "";
        //    System.Security.Principal.IPrincipal User = System.Web.HttpContext.Current.User;

        //    Query = "Delete from [HWInventory_Table] where ID = @Id";


        //    SqlCommand cmd = new SqlCommand(Query, conn);
        //    cmd.Parameters.AddWithValue("@Id", id);
        //    try
        //    {
        //        OpenConnection();
        //        cmd.ExecuteNonQuery();
        //        return Json(new { data = GetHWData(), success = true }, JsonRequestBehavior.AllowGet);
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
    }



}


