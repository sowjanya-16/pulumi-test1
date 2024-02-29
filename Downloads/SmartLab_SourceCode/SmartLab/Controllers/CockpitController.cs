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
using System.Configuration;

namespace LC_Reports_V1.Controllers
{
    //[Authorize(Users = @"apac\din2cob,apac\jov6cob,apac\mta2cob,apac\muu4cob,apac\sbr2kor,apac\chb1kor,apac\oig1cob,apac\rau2kor,apac\rma5cob, apac\pch2kor, apac\mxk8kor,  apac\ghb1cob, apac\vvs2kor, apac\SIF1COB")]
    public class CockpitController : Controller
    {
        public static List<SPOTONData_Table_2022> lstUsers = new List<SPOTONData_Table_2022>();
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
        public static List<Order_Status_Table> lstOrderStatus = new List<Order_Status_Table>();
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

        /// <summary>
        /// /////////////////////////WITH REQUEST TABLE
        /// </summary>
        public ActionResult Cockpit_Options()
        {

            return View();
        }

        public ActionResult VKM_Cockpit()
        {
            InitialiseBudgeting();

            //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

            ////using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

            //  {
            //      if (lstUsers == null || lstUsers.Count == 0)
            //          lstUsers = db.SPOTONData_Table_2022.ToList<SPOTONData_Table_2022>();
            //      if (lstOEMs == null || lstOEMs.Count == 0)
            //          lstOEMs = db.OEM_Table.ToList<OEM_Table>();
            //      if (lstBUs == null || lstBUs.Count == 0)
            //          lstBUs = db.BU_Table.ToList<BU_Table>();  
            //      if (lstDEPTs == null || lstDEPTs.Count == 0)
            //          lstDEPTs = db.DEPT_Table.ToList<DEPT_Table>();
            //      if (lstCostElement == null || lstCostElement.Count == 0)
            //          lstCostElement = db.CostElement_Table.ToList<CostElement_Table>();
            //      if (lstPrdCateg == null || lstPrdCateg.Count == 0)
            //          lstPrdCateg = db.Category_Table.ToList<Category_Table>();
            //      if (lstItems == null || lstItems.Count == 0)
            //          lstItems = db.ItemsCostList_Table.ToList<ItemsCostList_Table>()/*.FindAll(item=>item.Deleted != true)*/; //refresh this
            //      if (lstPrivileged == null || lstPrivileged.Count == 0)
            //          lstPrivileged = db.tbl_UserIDs_Table.ToList<tbl_UserIDs_Table>();
            //      //if (lstBU_SPOCs == null || lstBU_SPOCs.Count == 0)
            //          lstBU_SPOCs = db.BU_SPOCS.ToList<BU_SPOCS>();
            //      if (lstGroups_test == null || lstGroups_test.Count == 0)
            //          lstGroups_test = db.Groups_Table_Test.ToList<Groups_Table_Test>();

            //      if (lstOrderStatus == null || lstOrderStatus.Count == 0)
            //          lstOrderStatus = db.Order_Status_Table.ToList<Order_Status_Table>();

            //      lstUsers.Sort((a, b) => a.EmployeeName.CompareTo(b.EmployeeName));
            //      lstBUs.Sort((a, b) => a.BU.CompareTo(b.BU));
            //      lstOEMs.Sort((a, b) => a.OEM.CompareTo(b.OEM));
            //      lstDEPTs.Sort((a, b) => a.DEPT.CompareTo(b.DEPT));
            //      lstCostElement.Sort((a, b) => a.CostElement.CompareTo(b.CostElement));
            //      lstPrdCateg.Sort((a, b) => a.Category.CompareTo(b.Category));
            //      lstItems.Sort((a, b) => a.Item_Name.CompareTo(b.Item_Name));
            //      lstPrivileged.Sort((a, b) => a.ADSID.CompareTo(b.ADSID));
            //      lstBU_SPOCs.Sort((a, b) => a.VKMspoc.CompareTo(b.VKMspoc));
            //      lstGroups_test.Sort((a, b) => a.Group.CompareTo(b.Group));
            //      lstOrderStatus.Sort((a, b) => a.OrderStatus.CompareTo(b.OrderStatus));

            //  }

            return View();
        }


        [HttpPost]
        public ActionResult GetInitData()
        {
            InitialiseBudgeting();

            //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

            ////using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

            //{
            //    //if (lstUsers == null || lstUsers.Count == 0)
            //    lstUsers = db.SPOTONData_Table_2022.ToList<SPOTONData_Table_2022>();
            //    if (lstOEMs == null || lstOEMs.Count == 0)
            //        lstOEMs = db.OEM_Table.ToList<OEM_Table>();
            //    if (lstBUs == null || lstBUs.Count == 0)
            //        lstBUs = db.BU_Table.ToList<BU_Table>();
            //    if (lstDEPTs == null || lstDEPTs.Count == 0)
            //        lstDEPTs = db.DEPT_Table.ToList<DEPT_Table>();
            //    if (lstCostElement == null || lstCostElement.Count == 0)
            //        lstCostElement = db.CostElement_Table.ToList<CostElement_Table>();
            //    if (lstPrdCateg == null || lstPrdCateg.Count == 0)
            //        lstPrdCateg = db.Category_Table.ToList<Category_Table>();
            //    if (lstItems == null || lstItems.Count == 0)
            //        lstItems = db.ItemsCostList_Table.ToList<ItemsCostList_Table>()/*.FindAll(item=>item.Deleted != true)*/; //refresh this
            //    if (lstPrivileged == null || lstPrivileged.Count == 0)
            //        lstPrivileged = db.tbl_UserIDs_Table.ToList<tbl_UserIDs_Table>();
            //    //if (lstBU_SPOCs == null || lstBU_SPOCs.Count == 0)
            //    lstBU_SPOCs = db.BU_SPOCS.ToList<BU_SPOCS>();
            //    if (lstGroups_test == null || lstGroups_test.Count == 0)
            //        lstGroups_test = db.Groups_Table_Test.ToList<Groups_Table_Test>();

            //    if (lstOrderStatus == null || lstOrderStatus.Count == 0)
            //        lstOrderStatus = db.Order_Status_Table.ToList<Order_Status_Table>();


            //    lstUsers.Sort((a, b) => a.EmployeeName.CompareTo(b.EmployeeName));
            //    lstBUs.Sort((a, b) => a.BU.CompareTo(b.BU));
            //    lstOEMs.Sort((a, b) => a.OEM.CompareTo(b.OEM));
            //    lstDEPTs.Sort((a, b) => a.DEPT.CompareTo(b.DEPT));
            //    lstCostElement.Sort((a, b) => a.CostElement.CompareTo(b.CostElement));
            //    lstPrdCateg.Sort((a, b) => a.Category.CompareTo(b.Category));
            //    lstItems.Sort((a, b) => a.Item_Name.CompareTo(b.Item_Name));
            //    lstPrivileged.Sort((a, b) => a.ADSID.CompareTo(b.ADSID));
            //    lstBU_SPOCs.Sort((a, b) => a.VKMspoc.CompareTo(b.VKMspoc));
            //    lstGroups_test.Sort((a, b) => a.Group.CompareTo(b.Group));
            //    lstOrderStatus.Sort((a, b) => a.OrderStatus.CompareTo(b.OrderStatus));

            //}


            return Json(new { data = true }, JsonRequestBehavior.AllowGet);
        }

        public bool InitialiseBudgeting()
        {


            //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

            ////using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

            //{
            ////if (lstUsers == null || lstUsers.Count == 0)
            //    lstUsers = db.SPOTONData_Table_2022.ToList<SPOTONData_Table_2022>(); //refresh this
            ////if (lstOEMs == null || lstOEMs.Count == 0)
            //    lstOEMs = db.OEM_Table.ToList<OEM_Table>();
            //if (lstBUs == null || lstBUs.Count == 0)
            //    lstBUs = db.BU_Table.ToList<BU_Table>();
            ////if (lstSections == null || lstSections.Count == 0)
            ////    lstSections = lstUsers.Select(x => x.Section).Distinct().ToList();
            ////if (lstDEPTs == null || lstDEPTs.Count == 0)
            //    lstDEPTs = db.DEPT_Table.ToList<DEPT_Table>();
            //if (lstGroups == null || lstGroups.Count == 0)
            //    lstGroups = db.Groups_Table.ToList<Groups_Table>();
            //if (lstCostElement == null || lstCostElement.Count == 0)
            //    lstCostElement = db.CostElement_Table.ToList<CostElement_Table>();
            //if (lstPrdCateg == null || lstPrdCateg.Count == 0)
            //    lstPrdCateg = db.Category_Table.ToList<Category_Table>();
            ////if (lstItems == null || lstItems.Count == 0)
            //    lstItems = db.ItemsCostList_Table.ToList<ItemsCostList_Table>()/*.FindAll(item=>item.Deleted != true)*/; //refresh this
            ////if (lstPrivileged == null || lstPrivileged.Count == 0)
            //    lstPrivileged = db.tbl_UserIDs_Table.ToList<tbl_UserIDs_Table>();
            ////if (lstBU_SPOCs == null || lstBU_SPOCs.Count == 0)
            //    lstBU_SPOCs = db.BU_SPOCS.ToList<BU_SPOCS>();
            //if (lstOrderStatus == null || lstOrderStatus.Count == 0)
            //    lstOrderStatus = db.Order_Status_Table.ToList<Order_Status_Table>();
            //if (lstCurrency == null || lstCurrency.Count == 0)
            //    lstCurrency = db.Currency_Table.ToList<Currency_Table>();
            //if (lstVendor == null || lstVendor.Count == 0)
            //    lstVendor = db.LeadTime_Table.ToList<LeadTime_Table>();
            //if (lstFund == null || lstFund.Count == 0)
            //    lstFund = db.Fund_Table.ToList<Fund_Table>();
            ////if (lstEMs == null || lstEMs.Count == 0)
            //    lstEMs = db.Planning_EM_Table.ToList<Planning_EM_Table>();//
            ////if (lstHOEs == null || lstHOEs.Count == 0)
            //    lstHOEs = db.Planning_HOE_Table.ToList<Planning_HOE_Table>();//  


            ////if (lstUsers_2020 == null || lstUsers_2020.Count == 0)
            ////    lstUsers_2020 = db.SPOTONDatas.ToList<SPOTONData>();
            ////if (lstGroups_old == null || lstGroups_old.Count == 0)
            ////    lstGroups_old = db.Groups_Table_Aug.ToList<Groups_Table_Aug>();
            ////if (lstGroups_test == null || lstGroups_test.Count == 0)
            //    lstGroups_test = db.Groups_Table_Test.ToList<Groups_Table_Test>();




            //BudgetingController.lstUsers.Sort((a, b) => a.EmployeeName.CompareTo(b.EmployeeName));
            //BudgetingController.lstBUs.Sort((a, b) => a.BU.CompareTo(b.BU));
            //BudgetingController.lstOEMs.Sort((a, b) => a.OEM.CompareTo(b.OEM));
            //BudgetingController.lstDEPTs.Sort((a, b) => a.DEPT.CompareTo(b.DEPT));
            //BudgetingController.lstGroups.Sort((a, b) => a.Group.CompareTo(b.Group));
            //BudgetingController.lstCostElement.Sort((a, b) => a.CostElement.CompareTo(b.CostElement));
            //BudgetingController.lstPrdCateg.Sort((a, b) => a.Category.CompareTo(b.Category));
            //BudgetingController.lstItems.Sort((a, b) => a.Item_Name.CompareTo(b.Item_Name));
            //BudgetingController.lstPrivileged.Sort((a, b) => a.ADSID.CompareTo(b.ADSID));
            //BudgetingController.lstBU_SPOCs.Sort((a, b) => a.VKMspoc.CompareTo(b.VKMspoc));
            //BudgetingController.lstOrderStatus.Sort((a, b) => a.OrderStatus.CompareTo(b.OrderStatus));
            //BudgetingController.lstCurrency.Sort((a, b) => a.Currency.CompareTo(b.Currency));
            //BudgetingController.lstVendor.Sort((a, b) => a.VendorCategory.CompareTo(b.VendorCategory));
            //BudgetingController.lstFund.Sort((a, b) => a.Fund.CompareTo(b.Fund));
            //BudgetingController.lstEMs.Sort((a, b) => a.FullName.CompareTo(b.FullName));
            //BudgetingController.lstHOEs.Sort((a, b) => a.HOE_FullName.CompareTo(b.HOE_FullName));

            ////BudgetingController.lstItems1.Sort((a, b) => a.Item_Name.CompareTo(b.Item_Name));

            ////BudgetingController.lstUsers_2020.Sort((a, b) => a.EmployeeName.CompareTo(b.EmployeeName));
            ////BudgetingController.lstGroups_old.Sort((a, b) => a.Group.CompareTo(b.Group));
            //BudgetingController.lstGroups_test.Sort((a, b) => a.Group.CompareTo(b.Group));




            DataSet ds = new DataSet();
            connection();
            OpenConnection();
            string Query = " Exec [dbo].[Initialise_Budgeting] ";
            SqlCommand cmd = new SqlCommand(Query, conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            CloseConnection();


            lstUsers = new List<SPOTONData_Table_2022>();

            //Table 0 - SPOTONData_Table_2022
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                SPOTONData_Table_2022 item = new SPOTONData_Table_2022();
                item.EmployeeNumber = row["EmployeeNumber"].ToString();
                item.EmployeeName = row["EmployeeName"].ToString();
                item.Gender = row["Gender"].ToString();
                item.NTID = row["NTID"].ToString();
                item.Employee_Group = row["Employee Group"].ToString();
                item.Employee_SubGroup = row["Employee SubGroup"].ToString();
                item.Level = row["Level"].ToString();
                item.DOJ = row["DOJ"].ToString();
                item.ContractEndDate = row["ContractEndDate"].ToString();
                item.HC = row["HC"].ToString();
                item.PersonalCapacity = row["PersonalCapacity"].ToString();
                item.PersonalCapacity_ = row["PersonalCapacity*"].ToString();
                item.PC_Remarks = row["PC*Remarks"].ToString();
                item.PC_Remarks_StartDate = row["PC*Remarks_StartDate"].ToString();
                item.PC_Remarks_EndDate = row["PC*Remarks_EndDate"].ToString();
                item.PartTimePercentage = row["PartTimePercentage"].ToString();
                item.PartTimeEndDate = row["PartTimeEndDate"].ToString();
                item.Role = row["Role"].ToString();
                item.BU = row["BU"].ToString();
                item.Section = row["Section"].ToString();
                item.Department = row["Department"].ToString();
                item.Group = row["Group"].ToString();
                item.BusinessArea = row["BusinessArea"].ToString();
                item.PersonnelArea = row["PersonnelArea"].ToString();
                item.PersonnelSubArea = row["PersonnelSubArea"].ToString();

                lstUsers.Add(item);
            }

            //Table 2 - OEM_Table	

            lstOEMs = new List<OEM_Table>();

            foreach (DataRow row in ds.Tables[1].Rows)
            {
                OEM_Table item = new OEM_Table();
                item.ID = Convert.ToInt32(row["ID"]);
                item.OEM = row["OEM"].ToString();
                lstOEMs.Add(item);
            }

            //Table 3 - BU_Table	
            lstBUs = new List<BU_Table>();

            foreach (DataRow row in ds.Tables[2].Rows)
            {
                BU_Table item = new BU_Table();
                item.ID = Convert.ToInt32(row["ID"]);
                item.BU = row["BU"].ToString();
                lstBUs.Add(item);
            }
            //Table 4 - DEPT_Table	
            lstDEPTs = new List<DEPT_Table>();
            foreach (DataRow row in ds.Tables[3].Rows)
            {
                DEPT_Table item = new DEPT_Table();
                item.ID = Convert.ToInt32(row["ID"]);
                item.DEPT = row["DEPT"].ToString();
                item.Outdated = Convert.ToBoolean(row["Outdated"]);
                item.Rep_ID = row["Rep_ID"].ToString() != "" ? Convert.ToInt32(row["Rep_ID"]) : 0;
                lstDEPTs.Add(item);
            }
            //Table 5 - Groups_Table_Test	
            lstGroups_test = new List<Groups_Table_Test>();

            foreach (DataRow row in ds.Tables[4].Rows)
            {
                Groups_Table_Test item = new Groups_Table_Test();
                item.ID = Convert.ToInt32(row["ID"]);
                item.Group = row["Group"].ToString();
                item.Dept = Convert.ToInt32(row["Dept"]);
                item.Outdated = Convert.ToBoolean(row["Outdated"]);

                item.Rep_ID = row["Rep_ID"].ToString() != "" ? Convert.ToInt32(row["Rep_ID"]) : 0;
                lstGroups_test.Add(item);
            }
            //Table 6 - CostElement_Table	

            lstCostElement = new List<CostElement_Table>();

            foreach (DataRow row in ds.Tables[5].Rows)
            {
                CostElement_Table item = new CostElement_Table();
                item.ID = Convert.ToInt32(row["ID"]);
                item.CostElement = row["CostElement"].ToString();
                lstCostElement.Add(item);
            }

            //Table 7 - Category_Table	
            lstPrdCateg = new List<Category_Table>();

            foreach (DataRow row in ds.Tables[6].Rows)
            {
                Category_Table item = new Category_Table();
                item.ID = Convert.ToInt32(row["ID"]);
                item.Category = row["Category"].ToString();
                lstPrdCateg.Add(item);
            }

            //Table 8 - ItemsCostList_Table	
            lstItems = new List<ItemsCostList_Table>();

            foreach (DataRow row in ds.Tables[7].Rows)
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
                lstItems.Add(item);
            }

            //Table 9 - tbl_UserIDs_Table;
            lstPrivileged = new List<tbl_UserIDs_Table>();

            foreach (DataRow row in ds.Tables[8].Rows)
            {
                tbl_UserIDs_Table item = new tbl_UserIDs_Table();
                item.ADSID = row["ADSID"].ToString();
                item.FullName = row["FullName"].ToString();
                item.BU = row["BU"].ToString();
                item.emailID = row["emailID"].ToString();
                lstPrivileged.Add(item);

            }

            //Table 10 -BU_SPOCS;
            lstBU_SPOCs = new List<BU_SPOCS>();

            foreach (DataRow row in ds.Tables[9].Rows)
            {
                BU_SPOCS item = new BU_SPOCS();
                item.BU = Convert.ToInt32(row["BU"]);
                item.VKMspoc = row["VKMspoc"].ToString();
                lstBU_SPOCs.Add(item);
            }
            //Table 11 -Order_Status_Table	
            lstOrderStatus = new List<Order_Status_Table>();

            foreach (DataRow row in ds.Tables[10].Rows)
            {
                Order_Status_Table item = new Order_Status_Table();
                item.ID = Convert.ToInt32(row["ID"]);
                item.OrderStatus = row["OrderStatus"].ToString();
                lstOrderStatus.Add(item);
            }



            return true;
        }

        public ActionResult QuarterlyUtilization(string years_str, string buList_str, bool chart = false, string selected_ccxc = "")
        {
            List<string> years = new List<string>();
            List<string> buList = new List<string>();
            //List<Quarterly> rlist = new List<Quarterly>();
            Quarterly quarterly = new Quarterly()
            {
                Y1 = new List<Quarterly_Util>(),
                Y2 = new List<Quarterly_Util>(),
                Year1 = false,
                Year2 = false
            };

            if (years_str != null)
            {
                years = (years_str.Split(',')).ToList();

            }


            try
            {
                //create instanace of database connection
                // SqlConnection conn = new SqlConnection(connString);
                connection();

                //conn.Open();
                OpenConnection();
                //                string Query = " SELECT A.CostElement as CostElementID,a.shnt, DATEPART(YEAR,OrderDate) [Year]," +
                //"DATEPART(QUARTER, OrderDate)[Quarter], sum(OrderPrice) as OrderPrice, B.CostElement" +
                //"FROM requestitems_table A inner join CostElement_Table B on A.CostElement = B.ID" +
                //"where A.bu in (1, 3, 5) and OrderDate is not null and (Fund = 2 or Fund is null)" +
                //"GROUP BY DATEPART(YEAR, OrderDate),DATEPART(QUARTER, OrderDate),A.CostElement,B.CostElement,a.shnt";

                //                string Query = " WITH cte(dt,level)" +
                //"             AS" +
                //"             (" +
                //"                    SELECT  1 AS dt, 1 AS level" +
                //"                    UNION ALL" +
                //"                    SELECT cte.dt + 1, level + 1 from cte WHERE level < 4" +
                //"             )" +

                //"Select dt as Q, CostElementID, [Year], [Quarter], isnull(Utilized, 0) as Utilized, CostElement,BU,isnull([Plan],0) as [Plan] from cte left join (" +
                //" Select U.CostElementID, U.[Year], U.[Quarter], U.OrderPrice as Utilized, U.CostElement,U.BU,P.OrderPrice as [Plan] from                         " +
                //" (Select CostElementID,  [Year], [Quarter], sum(OrderPrice) as OrderPrice, CostElement, BU FROM(                                                 " +
                //" SELECT A.CostElement as CostElementID, DATEPART(YEAR, OrderDate)[Year],                                                                         " +
                //" DATEPART(QUARTER, OrderDate)[Quarter], sum(OrderPrice) as OrderPrice, B.CostElement, '2WP+MB' as BU                                             " +
                //" FROM requestitems_table A inner join CostElement_Table B on A.CostElement = B.ID                                                                " +
                //" inner join BU_Table C on A.BU = C.ID                                                                                                            " +
                //" where A.bu in (1, 3) and  OrderDate is not null                                                                                                 " +
                //" GROUP BY DATEPART(YEAR, OrderDate), DATEPART(QUARTER, OrderDate), A.CostElement, A.Dept, B.CostElement) M                                       " +
                //" GROUP BY CostElementID,  [Year], [Quarter], CostElement, BU) as U inner join                                                                    " +
                //"  (Select V.CostElement as CostElementID, VKM_Year as [Year], [Quarter], sum(OrderPlan_Amt) as OrderPrice, B.CostElement, '2WP+MB' as BU         " +
                //"FROM VKMOrderPlan_Table V inner join CostElement_Table B on V.CostElement = B.ID GROUP BY V.CostElement , VKM_Year , [Quarter], B.CostElement    " +
                //") as P on U.CostElementID = P.CostElementID and U.[Year] = P.[Year] and U.[Quarter] = P.[Quarter]--Order by[Year],[Quarter]                      " +

                //")a on cte.dt = a.[Quarter] order by[Year], dt                                                                                                    " +
                //"             option(maxrecursion 32767)                                                                                                          " ;

                foreach (var yr in years)
                {

                    string Query = "EXEC [dbo].[QuarterlyUtilization_Projected_new] " + yr + "";
                    SqlCommand cmd = new SqlCommand(Query, conn);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    var dt = new DataTable();
                    da.Fill(dt);
                    //conn.Close();
                    CloseConnection();
                    foreach (DataRow row in dt.Rows)
                    {

                        if (Convert.ToInt32(row["Year"]) == int.Parse(years[0]))//prev yr
                        {
                            quarterly.Year1 = years[0].Trim() == "2021" ? false : true;
                            Quarterly_Util y1 = new Quarterly_Util();
                            y1.CostElement = row["CostElementID"].ToString();
                            y1.BU = row["BU"].ToString();
                            y1.Year = Convert.ToInt32(row["Year"]);
                            y1.Quarter = row["Quarter"].ToString();
                            y1.Plan = Convert.ToDouble(row["ApprCost"].ToString());
                            y1.Projected = Convert.ToDouble(row["Projected"].ToString());
                            y1.Utilized = Convert.ToDouble(row["Utilized"]);
                            y1.BudgetCode = row["BudgetCode"].ToString();
                            quarterly.Y1.Add(y1);
                        }
                        else//present yr
                        {
                            quarterly.Year2 = years[1].Trim() == "2021" ? false : true;
                            Quarterly_Util y2 = new Quarterly_Util();
                            y2.CostElement = row["CostElementID"].ToString();
                            y2.BU = row["BU"].ToString();
                            y2.Year = Convert.ToInt32(row["Year"]);
                            y2.Quarter = row["Quarter"].ToString();
                            y2.Plan = Convert.ToDouble(row["ApprCost"].ToString());
                            y2.Projected = Convert.ToDouble(row["Projected"].ToString());
                            y2.Utilized = Convert.ToDouble(row["Utilized"]);
                            y2.BudgetCode = row["BudgetCode"].ToString();
                            quarterly.Y2.Add(y2);
                        }

                        //rlist.Add(quarterly);
                    }

                }
                // " WITH cte(dt,level) AS  (  SELECT  1 AS dt, 1 AS level UNION ALL  SELECT cte.dt+1,level + 1 from cte WHERE level < 4  )Select dt as Q, CostElementID, [Year], [Quarter], isnull(Utilized,0) as Utilized, CostElement,BU,isnull([Plan],0) as [Plan] from cte left join (Select U.CostElementID, U.[Year], U.[Quarter], U.OrderPrice as Utilized, U.CostElement,U.BU,P.OrderPrice as [Plan] from (Select CostElementID,  [Year], [Quarter], sum(OrderPrice) as OrderPrice, CostElement,BU FROM (SELECT A.CostElement as CostElementID, DATEPART(YEAR,OrderDate) [Year],DATEPART(QUARTER,OrderDate) [Quarter], sum(OrderPrice) as OrderPrice, B.CostElement, '2WP+MB' as BU FROM requestitems_table A inner join CostElement_Table B on A.CostElement = B.ID inner join BU_Table C on A.BU = C.ID  where A.bu in (1,3) and  OrderDate is not null GROUP BY DATEPART(YEAR,OrderDate),DATEPART(QUARTER,OrderDate),A.CostElement,A.Dept,B.CostElement) M GROUP BY CostElementID,  [Year], [Quarter], CostElement,BU) as U inner join (Select V.CostElement as CostElementID, VKM_Year as [Year], [Quarter], sum(OrderPlan_Amt) as OrderPrice, B.CostElement, '2WP+MB' as BU FROM VKMOrderPlan_Table V inner join CostElement_Table B on V.CostElement = B.ID GROUP BY V.CostElement , VKM_Year , [Quarter], B.CostElement ) as P on U.CostElementID= P.CostElementID and U.[Year] = P.[Year] and U.[Quarter] = P.[Quarter]  )a on cte.dt = a.[Quarter] option (maxrecursion 32767) ";


            }







            catch (Exception ex)
            {

            }
            finally
            {

            }



            //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

            ////using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

            //{
            //    List<RequestItems_Table> reqList = new List<RequestItems_Table>();
            //    List<RequestItems_Table> reqList_forquery = new List<RequestItems_Table>();

            //    if (years_str != null)
            //    {
            //        years = (years_str.Split(',')).ToList();

            //    }
            //    if (buList_str != null)
            //    {
            //        buList = (buList_str.Split(',')).ToList();

            //    }
            //    foreach (var bu_item in buList)
            //    {
            //        reqList.AddRange(db.RequestItems_Table.ToList<RequestItems_Table>().FindAll(ss => ss.BU.Contains(bu_item)).FindAll(v => v.Fund == null || v.Fund.Trim().Contains("2")));
            //    }

            //    foreach (string yr in years)
            //    {

            //        //vkmSummary(years, buList, chart, selected_ccxc);
            //        if (int.Parse(yr) - 1 > 2020)
            //        {

            //            string is_CCXC = string.Empty;
            //            bool CCXCflag = false;
            //            string presentUserDept = string.Empty;
            //            // if (selected_ccxc != "") //DATA FILTER BASED ON CCXC DROPDOWM - NOT USER'S NTID
            //            // {
            //            if (selected_ccxc.ToUpper().Contains("CC"))
            //            {
            //                is_CCXC = "CC";
            //                CCXCflag = true;
            //            }
            //            else if (selected_ccxc.ToUpper().Contains("XC"))
            //            {
            //                is_CCXC = "XC";
            //                CCXCflag = true;
            //            }
            //            //   }
            //            else                    //DATA FILTER BASED ON USER'S NTID
            //            {
            //                presentUserDept = lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).Department;

            //                if (presentUserDept.ToUpper().Contains("CC"))
            //                {
            //                    is_CCXC = "CC";
            //                    CCXCflag = true;
            //                }
            //                else if (presentUserDept.ToUpper().Contains("XC"))
            //                {
            //                    is_CCXC = "XC";
            //                    CCXCflag = true;
            //                }

            //            }

            //            if (CCXCflag)
            //            {
            //                if (is_CCXC.Contains("XC"))
            //                {
            //                    //XC includes 2WP 
            //                    reqList_forquery.AddRange(db.RequestItems_Table.ToList().FindAll(ss => ss.BU.Contains(lstBUs.Find(bu => bu.BU.Contains("2WP")).ID.ToString())).FindAll(v => v.Fund == null || v.Fund.Trim().Contains("2")));
            //                    reqList_forquery = reqList.FindAll(dpt => lstDEPTs.Find(x => x.ID.ToString().Contains(dpt.DEPT)).DEPT.Contains(is_CCXC));

            //                }
            //                else
            //                    reqList_forquery = reqList.FindAll(dpt => !lstDEPTs.Find(x => x.ID.ToString().Contains(dpt.DEPT)).DEPT.Contains("XC"));

            //                CCXCflag = false;
            //            }



            //        }
            //        else
            //        {
            //            if (buList.Contains("2") && buList.Contains("4") /*&& selected_ccxc.ToUpper().Contains("Custom") || selected_ccxc.ToUpper().Contains("XC"))*/ && yr == "2021" && reqList.FindAll(c => c.BU == "1").Count == 0) //XC VKM2021 - include 2WP Whole Totals also
            //            {
            //                reqList.AddRange(db.RequestItems_Table.ToList().FindAll(ss => ss.BU.Contains(lstBUs.Find(bu => bu.BU.Contains("2WP")).ID.ToString())).FindAll(v => v.Fund == null || v.Fund.Trim().Contains("2")));
            //            }
            //            reqList_forquery = reqList;


            //        }
            //        // return Json(new { data = reqList_forquery.FindAll(y=>y.ApprovalSH == true), message = selected_ccxc }, JsonRequestBehavior.AllowGet);
            //        rlist.AddRange(reqList_forquery.FindAll(y => y.ApprovalSH == true && y.RequestDate.ToString().Contains((int.Parse(yr) - 1).ToString())));
            //    }
            //}
            return View(quarterly);
        }

        public ActionResult OverallSummary(string years_str, string buList_str, bool chart = false, string selected_ccxc = "")
        {
            List<string> years = new List<string>();
            List<string> buList = new List<string>();

            using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

            //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

            {
                List<RequestItems_Table> reqList = new List<RequestItems_Table>();
                List<RequestItems_Table> reqList_forquery = new List<RequestItems_Table>();
                List<vkmSummary> viewList = new List<vkmSummary>();

                var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                BudgetParam t = new BudgetParam();
                List<columnsinfo> _col = new List<columnsinfo>();

                //List<TravelCost_Table> tcList = new List<TravelCost_Table>();
                //List<HC_Table> hcList = new List<HC_Table>();



                decimal OP_MAE_Totals = 0, OP_NMAE_Totals = 0, OP_SoftwareTotals = 0;
                decimal OU_MAE_Totals = 0, OU_NMAE_Totals = 0, OU_SoftwareTotals = 0;
                decimal Ch_P_MAE_Totals = 0, Ch_P_NMAE_Totals = 0, Ch_P_SoftwareTotals = 0, Ch_P_OverallTotals = 0;
                decimal Ch_U_MAE_Totals = 0, Ch_U_NMAE_Totals = 0, Ch_U_SoftwareTotals = 0, Ch_U_OverallTotals = 0;
                decimal Pr_P_MAE_Totals = 0, Pr_P_NMAE_Totals = 0, Pr_P_SoftwareTotals = 0, Pr_P_OverallTotals = 0;
                decimal Pr_U_MAE_Totals = 0, Pr_U_NMAE_Totals = 0, Pr_U_SoftwareTotals = 0, Pr_U_OverallTotals = 0;

                decimal tc_PlanTotals = 0, hc_PlanTotals = 0, tc_UtilTotals = 0, hc_UtilTotals = 0;
                string OverAll_Plan = "$0", OverAll_Util = "$0";
                if (years_str != null)
                {
                    years = (years_str.Split(',')).ToList();

                }
                //// List<RequestItemsList_2019> reqList2019 = new List<RequestItemsList_2019>();
                ////reqList2019 = db.RequestItemsList_2019.ToList<RequestItemsList_2019>();



                ////  if (!(chart == false && years.Count() == 3)) //if number of years selected = 3, then no need to show table
                //// {


                ////get reqlist data for the BUs based on SPOC's BU / CC XC Level BUs
                //foreach (var bu_item in buList)
                //{
                //    reqList.AddRange(db.RequestItems_Table.ToList<RequestItems_Table>().FindAll(ss => ss.BU.Contains(bu_item)).FindAll(v => v.Fund == null || v.Fund.Trim().Contains("2")));
                //}

                //foreach (string yr in years)
                //{
                //    //CC XC CHECK


                //    //for 2020 reqList_forquery has relevant data under the BU filtering ; but >2020 needs dept FIltering based on CC XC
                //    //if(int.Parse(yr)-1 == 2020)
                //    //{
                //    //    reqList_forquery.AddRange(reqList.FindAll(ss => ss.BU.Contains(lstBUs.Find(bu => bu.BU.Contains("DA")).ID.ToString())));
                //    //}
                //    if (int.Parse(yr) - 1 > 2020)
                //    {

                //        string is_CCXC = string.Empty;
                //        bool CCXCflag = false;
                //        string presentUserDept = string.Empty;
                //        // if (selected_ccxc != "") //DATA FILTER BASED ON CCXC DROPDOWM - NOT USER'S NTID
                //        // {
                //        if (selected_ccxc.ToUpper().Contains("CC"))
                //        {
                //            is_CCXC = "CC";
                //            CCXCflag = true;
                //        }
                //        else if (selected_ccxc.ToUpper().Contains("XC"))
                //        {
                //            is_CCXC = "XC";
                //            CCXCflag = true;
                //        }
                //        //   }
                //        else                    //DATA FILTER BASED ON USER'S NTID
                //        {
                //            presentUserDept = lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).Department;

                //            if (presentUserDept.ToUpper().Contains("CC"))
                //            {
                //                is_CCXC = "CC";
                //                CCXCflag = true;
                //            }
                //            else if (presentUserDept.ToUpper().Contains("XC"))
                //            {
                //                is_CCXC = "XC";
                //                CCXCflag = true;
                //            }

                //        }

                //        if (CCXCflag)
                //        {
                //            if (is_CCXC.Contains("XC"))
                //            {
                //                //XC includes 2WP 
                //                reqList_forquery.AddRange(db.RequestItems_Table.ToList().FindAll(ss => ss.BU.Contains(lstBUs.Find(bu => bu.BU.Contains("2WP")).ID.ToString())).FindAll(v => v.Fund == null || v.Fund.Trim().Contains("2")));
                //                reqList_forquery = reqList.FindAll(dpt => lstDEPTs.Find(x => x.ID.ToString().Contains(dpt.DEPT)).DEPT.Contains(is_CCXC));

                //            }
                //            else
                //                reqList_forquery = reqList.FindAll(dpt => !lstDEPTs.Find(x => x.ID.ToString().Contains(dpt.DEPT)).DEPT.Contains("XC"));

                //            CCXCflag = false;
                //        }



                //    }
                //    else
                //    {
                //        if (buList.Contains("2") && buList.Contains("4") /*&& selected_ccxc.ToUpper().Contains("Custom") || selected_ccxc.ToUpper().Contains("XC"))*/ && yr == "2021" && reqList.FindAll(c => c.BU == "1").Count == 0) //XC VKM2021 - include 2WP Whole Totals also
                //        {
                //            reqList.AddRange(db.RequestItems_Table.ToList().FindAll(ss => ss.BU.Contains(lstBUs.Find(bu => bu.BU.Contains("2WP")).ID.ToString())).FindAll(v => v.Fund == null || v.Fund.Trim().Contains("2")));
                //        }
                //        reqList_forquery = reqList;


                //    }
                //   // return Json(new { data = reqList_forquery.FindAll(y=>y.ApprovalSH == true), message = selected_ccxc }, JsonRequestBehavior.AllowGet);
                //    //CODE TO GET GROUPS OF COST ELEMENT
                //    IEnumerable<IGrouping<string, RequestItems_Table>> query = reqList_forquery.GroupBy(item => item.CostElement);

                //    decimal P_MAE_Totals = 0, P_NMAE_Totals = 0, P_SoftwareTotals = 0;
                //    decimal U_MAE_Totals = 0, U_NMAE_Totals = 0, U_SoftwareTotals = 0;
                //    decimal Cancelled_MAE_Totals = 0, Cancelled_NMAE_Totals = 0, Cancelled_Software_Totals = 0;

                //    Cancelled_MAE_Totals = reqList_forquery.FindAll(X=>X.CostElement.Trim() == "1" && X.OrderStatus != null).Where(item => item.OrderStatus.Trim() == lstOrderStatus.Find(status => status.OrderStatus.Trim().Equals("Cancelled")).ID.ToString()).Count() >0 ?
                //        (decimal)reqList_forquery.FindAll(X => X.CostElement.Trim() == "1" && X.OrderStatus != null).Where(item => item.OrderStatus.Trim() == lstOrderStatus.Find(status => status.OrderStatus.Trim().Equals("Cancelled")).ID.ToString()).Sum(x => Math.Round((double)x.ApprCost,MidpointRounding.AwayFromZero)):
                //        0;
                //    Cancelled_NMAE_Totals = reqList_forquery.FindAll(X => X.CostElement.Trim() == "2" && X.OrderStatus != null).Where(item => item.OrderStatus.Trim() == lstOrderStatus.Find(status => status.OrderStatus.Trim().Equals("Cancelled")).ID.ToString()).Count() > 0 ?
                //        (decimal)reqList_forquery.FindAll(X => X.CostElement.Trim() == "2" && X.OrderStatus != null).Where(item => item.OrderStatus.Trim() == lstOrderStatus.Find(status => status.OrderStatus.Trim().Equals("Cancelled")).ID.ToString()).Sum(x => Math.Round((double)x.ApprCost, MidpointRounding.AwayFromZero)) :
                //        0;
                //    Cancelled_Software_Totals = reqList_forquery.FindAll(X => X.CostElement.Trim() == "3" && X.OrderStatus != null).Where(item => item.OrderStatus.Trim() == lstOrderStatus.Find(status => status.OrderStatus.Trim().Equals("Cancelled")).ID.ToString()).Count() > 0 ?
                //        (decimal)reqList_forquery.FindAll(X => X.CostElement.Trim() == "3" && X.OrderStatus != null).Where(item => item.OrderStatus.Trim() == lstOrderStatus.Find(status => status.OrderStatus.Trim().Equals("Cancelled")).ID.ToString()).Sum(x => Math.Round((double)x.ApprCost, MidpointRounding.AwayFromZero)) :
                //        0;
                //    //var danger1 = string.Empty;
                //    vkmSummary tempobj = new vkmSummary();
                //    tempobj.vkmyear = yr;


                //    //CODE TO GET THE TOTALS OF EACH COST ELEMENT
                //    foreach (IGrouping<string, RequestItems_Table> CostGroup in query)
                //    {

                //        // Iterate over each value in the
                //        // IGrouping and print the value.

                //        if (lstCostElement.Count != 0)
                //        {
                //            if (lstCostElement.Find(cost => cost.ID.Equals(Int32.Parse(CostGroup.Key))).CostElement == "MAE")
                //            {
                //                foreach (RequestItems_Table item in CostGroup)
                //                {
                //                    if (item.VKM_Year.ToString().Contains((int.Parse(yr).ToString())) && item.ApprovalSH == true)  //yr is VKM Year; if yr == 2020 - 2021 Planning (2020 sh apprvd - 2021 planning)
                //                    {

                //                        P_MAE_Totals += Math.Round((decimal)item.ApprCost, MidpointRounding.AwayFromZero);

                //                        U_MAE_Totals += item.OrderPrice == null ? (decimal)0.0 : Math.Round((decimal)item.OrderPrice, MidpointRounding.AwayFromZero); ;


                //                    }
                //                }
                //            }
                //            else if (lstCostElement.Find(cost => cost.ID.Equals(Int32.Parse(CostGroup.Key))).CostElement == "Non-MAE")
                //            {
                //                foreach (RequestItems_Table item in CostGroup)
                //                {
                //                    if (item.VKM_Year.ToString().Contains((int.Parse(yr).ToString())) && item.ApprovalSH == true)
                //                    {

                //                        P_NMAE_Totals += Math.Round((decimal)item.ApprCost, MidpointRounding.AwayFromZero);

                //                        U_NMAE_Totals += item.OrderPrice == null ? (decimal)0.0 : Math.Round((decimal)item.OrderPrice, MidpointRounding.AwayFromZero); ;

                //                    }
                //                }

                //            }
                //            else if (lstCostElement.Find(cost => cost.ID.Equals(Int32.Parse(CostGroup.Key))).CostElement == "Software")
                //            {
                //                foreach (RequestItems_Table item in CostGroup)
                //                {
                //                    if (item.VKM_Year.ToString().Contains((int.Parse(yr).ToString())) && item.ApprovalSH == true)
                //                    {


                //                        P_SoftwareTotals += Math.Round((decimal)item.ApprCost, MidpointRounding.AwayFromZero);

                //                        U_SoftwareTotals += item.OrderPrice == null ? (decimal)0.0 : Math.Round((decimal)item.OrderPrice, MidpointRounding.AwayFromZero); ;

                //                    }
                //                }

                //            }
                //        }
                //        else
                //            continue;
                //    }
                //    tempobj.P_MAE_Totals = P_MAE_Totals;
                //    OP_MAE_Totals += P_MAE_Totals;
                //    tempobj.P_NMAE_Totals = P_NMAE_Totals;
                //    OP_NMAE_Totals += P_NMAE_Totals;
                //    tempobj.P_Software_Totals = P_SoftwareTotals;
                //    OP_SoftwareTotals += P_SoftwareTotals;

                //    tempobj.P_Overall_Totals = P_MAE_Totals + P_NMAE_Totals + P_SoftwareTotals;

                //    tempobj.U_MAE_Totals = U_MAE_Totals;
                //    OU_MAE_Totals += U_MAE_Totals;
                //    tempobj.U_NMAE_Totals = U_NMAE_Totals;
                //    OU_NMAE_Totals += U_NMAE_Totals;
                //    tempobj.U_Software_Totals = U_SoftwareTotals;
                //    OU_SoftwareTotals += U_SoftwareTotals;
                //    tempobj.U_Overall_Totals = U_MAE_Totals + U_NMAE_Totals + U_SoftwareTotals;

                //    tempobj.Cancelled_MAE_Totals = Cancelled_MAE_Totals;
                //    tempobj.Cancelled_NMAE_Totals = Cancelled_NMAE_Totals;
                //    tempobj.Cancelled_Software_Totals = Cancelled_Software_Totals;

                //    if (buList.Contains("5") && yr == "2021")
                //    {

                //        tempobj.U_MAE_Totals += db.Utilization_Table2021.ToList().Find(x => x.CostElement == (lstCostElement.Find(ct => ct.CostElement.Contains("MAE")).ID)).Utilized2021;
                //        OU_MAE_Totals += U_MAE_Totals;
                //        tempobj.U_NMAE_Totals += db.Utilization_Table2021.ToList().Find(x => x.CostElement == (lstCostElement.Find(ct => ct.CostElement.Contains("Non-MAE")).ID)).Utilized2021;
                //        OU_NMAE_Totals += U_NMAE_Totals;
                //        tempobj.U_Software_Totals += db.Utilization_Table2021.ToList().Find(x => x.CostElement == (lstCostElement.Find(ct => ct.CostElement.Contains("Software")).ID)).Utilized2021;
                //        OU_SoftwareTotals += U_SoftwareTotals;
                //        var tot = db.Utilization_Table2021.ToList().Find(x => x.CostElement == (lstCostElement.Find(ct => ct.CostElement.Contains("MAE")).ID)).Utilized2021
                //            +
                //            db.Utilization_Table2021.ToList().Find(x => x.CostElement == (lstCostElement.Find(ct => ct.CostElement.Contains("Non-MAE")).ID)).Utilized2021
                //            +
                //            db.Utilization_Table2021.ToList().Find(x => x.CostElement == (lstCostElement.Find(ct => ct.CostElement.Contains("Software")).ID)).Utilized2021;

                //        tempobj.U_Overall_Totals += tot;


                //    }
                //    viewList.Add(tempobj);

                //}
                List<string> cc_bulist = new List<string>() { "1", "3", "5" };
                viewList = GetTotals(years, cc_bulist, chart, selected_ccxc);
                int HC_Budget = 1;


                List<Models.HC_Budget_Table> hC_Budget = db.HC_Budget_Table.ToList();
                foreach (var item in hC_Budget)
                {
                    if (item.Year.ToString().Trim() == years[0].Trim())
                    {
                        HC_Budget = item.Budget;
                    }
                }

                Overall vkm_overall = new Overall()
                {
                    overall = new List<Overall_Table>(),
                    total = new List<Totals_Table>()
                };

                List<Models.HC_Table> hcList = db.HC_Table.ToList();
                foreach (var item in hcList)
                {
                    if (item.Year == years[0])
                    {

                        hc_PlanTotals += Convert.ToDecimal((item.Plan * HC_Budget * 12).ToString());
                        hc_UtilTotals += Convert.ToDecimal((item.Utilize * HC_Budget * 12).ToString());
                    }
                }
                Overall_Table overall_elt = new Overall_Table();
                overall_elt.VKM_yr = int.Parse(years[0]);
                overall_elt.VKM_elt = "HeadCount";
                overall_elt.Plan = hc_PlanTotals;
                overall_elt.Used = hc_UtilTotals;
                overall_elt.Available = hc_PlanTotals - hc_UtilTotals;
                var used_per = (hc_UtilTotals * 100) / hc_PlanTotals;
                var avail_per = ((hc_PlanTotals - hc_UtilTotals) * 100) / hc_PlanTotals;
                overall_elt.splitup_ratio = "Used: " + Math.Round(used_per, 3) + "% , Available: " + Math.Round(avail_per, 3) + " %";
                vkm_overall.overall.Add(overall_elt);

                List<Models.TravelCost_Table> tcList = db.TravelCost_Table.ToList();
                foreach (var item in tcList)
                {
                    if (item.Year == Convert.ToInt32(years[0]) && item.Cmmt_Item.Trim() == "106")
                    {

                        tc_PlanTotals += Convert.ToDecimal(item.Budget_Plan.ToString());
                        tc_UtilTotals += Convert.ToDecimal(item.Invoice.ToString());
                    }
                    //{

                    //    tc_PlanTotals += Convert.ToDecimal(item.Budget_Plan.ToString());
                    //    tc_UtilTotals += Convert.ToDecimal(item.Invoice.ToString());
                    //}
                }
                overall_elt = new Overall_Table();
                overall_elt.VKM_yr = int.Parse(years[0]);
                overall_elt.VKM_elt = "Travel Cost";
                overall_elt.Plan = tc_PlanTotals;
                overall_elt.Used = tc_UtilTotals;
                overall_elt.Available = tc_PlanTotals - tc_UtilTotals;
                var used_pertc = (tc_UtilTotals * 100) / tc_PlanTotals;
                var avail_pertc = ((tc_PlanTotals - tc_UtilTotals) * 100) / tc_PlanTotals;
                overall_elt.splitup_ratio = "Used: " + Math.Round(used_pertc, 3) + "% , Available: " + Math.Round(avail_pertc, 3) + " %";
                vkm_overall.overall.Add(overall_elt);

                overall_elt = new Overall_Table();
                overall_elt.VKM_yr = int.Parse(years[0]);
                overall_elt.VKM_elt = "Cost Element";
                overall_elt.Plan = (viewList.ElementAt(viewList.Count - 2).P_Overall_Totals);
                overall_elt.Used = (viewList.ElementAt(viewList.Count - 2).U_Overall_Totals);
                overall_elt.Available = (viewList.ElementAt(viewList.Count - 2).P_Overall_Totals) - (viewList.ElementAt(viewList.Count - 2).U_Overall_Totals);
                var used_perce = (overall_elt.Used * 100) / overall_elt.Plan;
                var avail_perce = ((overall_elt.Plan - overall_elt.Used) * 100) / overall_elt.Plan;
                overall_elt.splitup_ratio = "Used: " + Math.Round(used_perce, 3) + "% , Available: " + Math.Round(avail_perce, 3) + " %";
                vkm_overall.overall.Add(overall_elt);

                OverAll_Plan = (viewList.ElementAt(viewList.Count - 2).P_Overall_Totals + hc_PlanTotals + tc_PlanTotals).ToString("C0", CultureInfo.CurrentCulture);
                OverAll_Util = (viewList.ElementAt(viewList.Count - 2).U_Overall_Totals + hc_UtilTotals + tc_UtilTotals).ToString("C0", CultureInfo.CurrentCulture);

                // Totals_Table total_elt = new Totals_Table();
                // total_elt.is_Plan_Util = "Plan";
                // total_elt.Totals = viewList.ElementAt(viewList.Count - 2).P_Overall_Totals + hc_PlanTotals + tc_PlanTotals;
                //  vkm_overall.total.Add(total_elt);
                // total_elt = new Totals_Table();
                // total_elt.is_Plan_Util = "Used";
                // total_elt.Totals = viewList.ElementAt(viewList.Count - 2).U_Overall_Totals + hc_UtilTotals + tc_UtilTotals;
                //vkm_overall.total.Add(total_elt);


                if (selected_ccxc.ToUpper().Contains("CC"))
                {
                    //data fetched is cc based already - for vkm overall summary
                }
                else
                {
                    viewList = GetTotals(years, buList, chart, selected_ccxc);
                }
                TempData["Year"] = years[0];
                return View(vkm_overall);

                // return View();
            }
        }


        //private List<RequestItems_Table> vkmSummary(List<string> years, List<string> buList, bool chart = false, string selected_ccxc = "")
        // {
        //    return 
        // }

        /// <summary>
        /// Function to send BU summary data to view
        /// </summary>
        /// <returns></returns>
        public ActionResult VKMSummaryData(List<string> years, List<string> buList, bool chart = false, string selected_ccxc = "")
        {
            var year_string = string.Join(",", years.ToArray());
            try
            {


                using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

                //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

                {
                    List<RequestItems_Table> reqList = new List<RequestItems_Table>();
                    List<RequestItems_Table> reqList_forquery = new List<RequestItems_Table>();
                    List<vkmSummary> viewList = new List<vkmSummary>();

                    var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                    BudgetParam t = new BudgetParam();
                    List<columnsinfo> _col = new List<columnsinfo>();

                    //List<TravelCost_Table> tcList = new List<TravelCost_Table>();
                    //List<HC_Table> hcList = new List<HC_Table>();



                    decimal OP_MAE_Totals = 0, OP_NMAE_Totals = 0, OP_SoftwareTotals = 0;
                    decimal OU_MAE_Totals = 0, OU_NMAE_Totals = 0, OU_SoftwareTotals = 0;
                    decimal Ch_P_MAE_Totals = 0, Ch_P_NMAE_Totals = 0, Ch_P_SoftwareTotals = 0, Ch_P_OverallTotals = 0;
                    decimal Ch_U_MAE_Totals = 0, Ch_U_NMAE_Totals = 0, Ch_U_SoftwareTotals = 0, Ch_U_OverallTotals = 0;
                    decimal Pr_P_MAE_Totals = 0, Pr_P_NMAE_Totals = 0, Pr_P_SoftwareTotals = 0, Pr_P_OverallTotals = 0;
                    decimal Pr_U_MAE_Totals = 0, Pr_U_NMAE_Totals = 0, Pr_U_SoftwareTotals = 0, Pr_U_OverallTotals = 0;

                    decimal Pr_Used_MAE_Totals = 0, Pr_Used_NMAE_Totals = 0, Pr_Used_SoftwareTotals = 0, Pr_Used_OverallTotals = 0;
                    decimal Pr_Projected_MAE_Totals = 0, Pr_Projected_NMAE_Totals = 0, Pr_Projected_SoftwareTotals = 0, Pr_Projected_OverallTotals = 0;
                    decimal Pr_Unused_MAE_Totals = 0, Pr_Unused_NMAE_Totals = 0, Pr_Unused_SoftwareTotals = 0, Pr_Unused_OverallTotals = 0;


                    decimal tc_PlanTotals = 0, hc_PlanTotals = 0, tc_UtilTotals = 0, hc_UtilTotals = 0;
                    string OverAll_Plan = "$0", OverAll_Util = "$0";

                    DataTable dtSummary = new DataTable();

                    List<string> cc_bulist = new List<string>() { "1", "3", "5" };
                    var bu_string = string.Join(",", buList.ToArray());
                    //viewList = GetTotals(years, cc_bulist, chart, selected_ccxc);

                    Overall vkm_overall = new Overall()
                    {
                        overall = new List<Overall_Table>()
                    };

                    if (!chart)
                    {
                        connection();
                        OpenConnection();
                        string Query = "Exec [dbo].[Cockpit_GetTotals_with2023] '" + bu_string + "', '" + year_string + "', '" + "', '" + "','" + "' ";
                        SqlCommand cmd = new SqlCommand(Query, conn);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dtSummary);
                        //conn.Close();
                        CloseConnection();




                        int HC_Budget = 1;


                        List<Models.HC_Budget_Table> hC_Budget = db.HC_Budget_Table.ToList();
                        foreach (var item in hC_Budget)
                        {
                            if (item.Year.ToString().Trim() == years[0].Trim())
                            {
                                HC_Budget = item.Budget;
                            }
                        }

                        //Overall vkm_overall = new Overall()
                        //{
                        //    overall = new List<Overall_Table>()
                        //};

                        List<Models.HC_Table> hcList = db.HC_Table.ToList();
                        foreach (var item in hcList)
                        {
                            if (item.Year == years[0])
                            {

                                hc_PlanTotals += Convert.ToDecimal((item.Plan * HC_Budget * 12).ToString());
                                hc_UtilTotals += Convert.ToDecimal((item.Utilize * HC_Budget * 12).ToString());
                            }
                        }
                        Overall_Table overall_elt = new Overall_Table();
                        overall_elt.VKM_yr = int.Parse(years[0]);
                        overall_elt.VKM_elt = "HeadCount";
                        overall_elt.Plan = hc_PlanTotals;
                        overall_elt.Used = hc_UtilTotals;
                        vkm_overall.overall.Add(overall_elt);

                        List<Models.TravelCost_Table> tcList = db.TravelCost_Table.ToList();
                        foreach (var item in tcList)
                        {
                            if (item.Year == Convert.ToInt32(years[0]) && item.Cmmt_Item.Trim() == "106")
                            {

                                tc_PlanTotals += Convert.ToDecimal(item.Budget_Plan.ToString());
                                tc_UtilTotals += Convert.ToDecimal(item.Invoice.ToString());
                            }
                        }
                        overall_elt = new Overall_Table();
                        overall_elt.VKM_yr = int.Parse(years[0]);
                        overall_elt.VKM_elt = "Travel Cost";
                        overall_elt.Plan = tc_PlanTotals;
                        overall_elt.Used = tc_UtilTotals;
                        vkm_overall.overall.Add(overall_elt);

                        overall_elt = new Overall_Table();
                        overall_elt.VKM_yr = int.Parse(years[0]);
                        overall_elt.VKM_elt = "Cost Element";

                        //overall_elt.Plan = (viewList.ElementAt(viewList.Count - 2).P_Overall_Totals);
                        //overall_elt.Used = (viewList.ElementAt(viewList.Count - 2).U_Overall_Totals);

                        overall_elt.Plan = Math.Round((decimal)dtSummary.Rows[dtSummary.Rows.Count - 1][2], MidpointRounding.AwayFromZero);
                        overall_elt.Used = Math.Round((decimal)dtSummary.Rows[dtSummary.Rows.Count - 1][3], MidpointRounding.AwayFromZero);

                        vkm_overall.overall.Add(overall_elt);

                        //OverAll_Plan = (viewList.ElementAt(viewList.Count - 2).P_Overall_Totals + hc_PlanTotals + tc_PlanTotals).ToString("C0", CultureInfo.CurrentCulture);
                        //OverAll_Util = (viewList.ElementAt(viewList.Count - 2).U_Overall_Totals + hc_UtilTotals + tc_UtilTotals).ToString("C0", CultureInfo.CurrentCulture);

                        OverAll_Plan = (Math.Round((decimal)dtSummary.Rows[dtSummary.Rows.Count - 1][2], MidpointRounding.AwayFromZero) + hc_PlanTotals + tc_PlanTotals).ToString("C0", CultureInfo.CurrentCulture);
                        OverAll_Util = (Math.Round((decimal)dtSummary.Rows[dtSummary.Rows.Count - 1][3], MidpointRounding.AwayFromZero) + hc_UtilTotals + tc_UtilTotals).ToString("C0", CultureInfo.CurrentCulture);


                        //if (selected_ccxc.ToUpper().Contains("CC"))
                        //{
                        //    //data fetched is cc based already - for vkm overall summary
                        //}
                        //else
                        //{
                        //    viewList = GetTotals(years, buList, chart, selected_ccxc);
                        //}

                        //if (!(chart == true && years.Count() == 3)) //if years selected are 3, then no need to calculate change in values
                        //{
                        //    Ch_P_MAE_Totals = (viewList.ElementAt(viewList.Count - 1).P_MAE_Totals) - (viewList.ElementAt(viewList.Count - 2).P_MAE_Totals);
                        //    Ch_P_NMAE_Totals = (viewList.ElementAt(viewList.Count - 1).P_NMAE_Totals) - (viewList.ElementAt(viewList.Count - 2).P_NMAE_Totals);
                        //    Ch_P_SoftwareTotals = (viewList.ElementAt(viewList.Count - 1).P_Software_Totals) - (viewList.ElementAt(viewList.Count - 2).P_Software_Totals);
                        //    Ch_P_OverallTotals = (viewList.ElementAt(viewList.Count - 1).P_Overall_Totals) - (viewList.ElementAt(viewList.Count - 2).P_Overall_Totals);

                        //    //var refTotals_forpercent_change = Int32.Parse(viewList[0].vkmyear) < Int32.Parse(viewList[1].vkmyear) ? viewList.ElementAt(viewList.Count - 2) : viewList.ElementAt(viewList.Count - 1);
                        //    Pr_P_MAE_Totals = (viewList.ElementAt(viewList.Count - 2).P_MAE_Totals) != 0 ? (Ch_P_MAE_Totals / viewList.ElementAt(viewList.Count - 2).P_MAE_Totals) * 100 : 0;
                        //    Pr_P_NMAE_Totals = (viewList.ElementAt(viewList.Count - 2).P_NMAE_Totals) != 0 ? (Ch_P_NMAE_Totals / viewList.ElementAt(viewList.Count - 2).P_NMAE_Totals) * 100 : 0;
                        //    Pr_P_SoftwareTotals = (viewList.ElementAt(viewList.Count - 2).P_Software_Totals) != 0 ? (Ch_P_SoftwareTotals / viewList.ElementAt(viewList.Count - 2).P_Software_Totals) * 100 : 0;
                        //    Pr_P_OverallTotals = (viewList.ElementAt(viewList.Count - 2).P_Overall_Totals) != 0 ? (Ch_P_OverallTotals / viewList.ElementAt(viewList.Count - 2).P_Overall_Totals) * 100 : 0;

                        //    Ch_U_MAE_Totals = (viewList.ElementAt(viewList.Count - 1).U_MAE_Totals) - (viewList.ElementAt(viewList.Count - 2).U_MAE_Totals);
                        //    Ch_U_NMAE_Totals = (viewList.ElementAt(viewList.Count - 1).U_NMAE_Totals) - (viewList.ElementAt(viewList.Count - 2).U_NMAE_Totals);
                        //    Ch_U_SoftwareTotals = (viewList.ElementAt(viewList.Count - 1).U_Software_Totals) - (viewList.ElementAt(viewList.Count - 2).U_Software_Totals);
                        //    Ch_U_OverallTotals = (viewList.ElementAt(viewList.Count - 1).U_Overall_Totals) - (viewList.ElementAt(viewList.Count - 2).U_Overall_Totals);

                        //    Pr_U_MAE_Totals = (viewList.ElementAt(viewList.Count - 2).U_MAE_Totals) != 0 ? (Ch_U_MAE_Totals / viewList.ElementAt(viewList.Count - 2).U_MAE_Totals) * 100 : 0;
                        //    Pr_U_NMAE_Totals = (viewList.ElementAt(viewList.Count - 2).U_NMAE_Totals) != 0 ? (Ch_U_NMAE_Totals / viewList.ElementAt(viewList.Count - 2).U_NMAE_Totals) * 100 : 0;
                        //    Pr_U_SoftwareTotals = (viewList.ElementAt(viewList.Count - 2).U_Software_Totals) != 0 ? (Ch_U_SoftwareTotals / viewList.ElementAt(viewList.Count - 2).U_Software_Totals) * 100 : 0;
                        //    Pr_U_OverallTotals = (viewList.ElementAt(viewList.Count - 2).U_Overall_Totals) != 0 ? (Ch_U_OverallTotals / viewList.ElementAt(viewList.Count - 2).U_Overall_Totals) * 100 : 0;


                        //    Pr_Used_MAE_Totals = (viewList.ElementAt(viewList.Count - 2).U_MAE_Totals) != 0 ? ((viewList.ElementAt(viewList.Count - 2).U_MAE_Totals) / viewList.ElementAt(viewList.Count - 2).P_MAE_Totals) * 100 : 0;
                        //    Pr_Used_NMAE_Totals = (viewList.ElementAt(viewList.Count - 2).U_NMAE_Totals) != 0 ? ((viewList.ElementAt(viewList.Count - 2).U_NMAE_Totals) / viewList.ElementAt(viewList.Count - 2).P_NMAE_Totals) * 100 : 0;
                        //    Pr_Used_SoftwareTotals = (viewList.ElementAt(viewList.Count - 2).U_Software_Totals) != 0 ? ((viewList.ElementAt(viewList.Count - 2).U_Software_Totals) / viewList.ElementAt(viewList.Count - 2).P_Software_Totals) * 100 : 0;
                        //    Pr_Used_OverallTotals = (viewList.ElementAt(viewList.Count - 2).U_Overall_Totals) != 0 ? ((viewList.ElementAt(viewList.Count - 2).U_Overall_Totals) / viewList.ElementAt(viewList.Count - 2).P_Overall_Totals) * 100 : 0;

                        //    Pr_Projected_MAE_Totals = (viewList.ElementAt(viewList.Count - 2).Proj_MAE_Totals) != 0 ? ((viewList.ElementAt(viewList.Count - 2).Proj_MAE_Totals) / viewList.ElementAt(viewList.Count - 2).P_MAE_Totals) * 100 : 0;
                        //    Pr_Projected_NMAE_Totals = (viewList.ElementAt(viewList.Count - 2).Proj_NMAE_Totals) != 0 ? ((viewList.ElementAt(viewList.Count - 2).Proj_NMAE_Totals) / viewList.ElementAt(viewList.Count - 2).P_NMAE_Totals) * 100 : 0;
                        //    Pr_Projected_SoftwareTotals = (viewList.ElementAt(viewList.Count - 2).Proj_Software_Totals) != 0 ? ((viewList.ElementAt(viewList.Count - 2).Proj_Software_Totals) / viewList.ElementAt(viewList.Count - 2).P_Software_Totals) * 100 : 0;
                        //    Pr_Projected_OverallTotals = (viewList.ElementAt(viewList.Count - 2).Proj_Overall_Totals) != 0 ? ((viewList.ElementAt(viewList.Count - 2).Proj_Overall_Totals) / viewList.ElementAt(viewList.Count - 2).P_Overall_Totals) * 100 : 0;

                        //    Pr_Unused_MAE_Totals = (viewList.ElementAt(viewList.Count - 2).Unused_MAE_Totals) != 0 ? ((viewList.ElementAt(viewList.Count - 2).Unused_MAE_Totals) / viewList.ElementAt(viewList.Count - 2).P_MAE_Totals) * 100 : 0;
                        //    Pr_Unused_NMAE_Totals = (viewList.ElementAt(viewList.Count - 2).Unused_NMAE_Totals) != 0 ? ((viewList.ElementAt(viewList.Count - 2).Unused_NMAE_Totals) / viewList.ElementAt(viewList.Count - 2).P_NMAE_Totals) * 100 : 0;
                        //    Pr_Unused_SoftwareTotals = (viewList.ElementAt(viewList.Count - 2).Unused_Software_Totals) != 0 ? ((viewList.ElementAt(viewList.Count - 2).Unused_Software_Totals) / viewList.ElementAt(viewList.Count - 2).P_Software_Totals) * 100 : 0;
                        //    Pr_Unused_OverallTotals = (viewList.ElementAt(viewList.Count - 2).Unused_Overall_Totals) != 0 ? ((viewList.ElementAt(viewList.Count - 2).Unused_Overall_Totals) / viewList.ElementAt(viewList.Count - 2).P_Overall_Totals) * 100 : 0;

                        //}
                    }
                    if (chart)
                    {
                        viewList = GetTotals(years, buList, chart, selected_ccxc);

                        System.Data.DataTable dt = new System.Data.DataTable();
                        dt.Columns.Add("Year", typeof(string));
                        dt.Columns.Add("Planned", typeof(decimal));
                        dt.Columns.Add("Utilized", typeof(decimal));
                        dt.Columns.Add("Percentage_Utilization", typeof(decimal));

                        //DataRow dr = dt.NewRow();

                        int rcnt = 0;

                        foreach (var info in viewList)
                        {
                            DataRow dr = dt.NewRow();
                            //dr = dt.NewRow();
                            dr[rcnt++] = "MAE" + " " + info.vkmyear;
                            dr[rcnt++] = info.P_MAE_Totals;
                            dr[rcnt++] = info.U_MAE_Totals;
                            dr[rcnt++] = info.P_MAE_Totals != 0 ? (info.U_MAE_Totals / info.P_MAE_Totals) * 100 : 0;
                            rcnt = 0;
                            dt.Rows.Add(dr);
                            dr = dt.NewRow();
                            dr[rcnt++] = "NMAE" + " " + info.vkmyear;
                            dr[rcnt++] = info.P_NMAE_Totals;
                            dr[rcnt++] = info.U_NMAE_Totals;
                            dr[rcnt++] = info.P_NMAE_Totals != 0 ? (info.U_NMAE_Totals / info.P_NMAE_Totals) * 100 : 0;
                            rcnt = 0;
                            dt.Rows.Add(dr);
                            dr = dt.NewRow();
                            dr[rcnt++] = "SW" + " " + info.vkmyear;
                            dr[rcnt++] = info.P_Software_Totals;
                            dr[rcnt++] = info.U_Software_Totals;
                            dr[rcnt++] = info.P_Software_Totals != 0 ? (info.U_Software_Totals / info.P_Software_Totals) * 100 : 0;
                            rcnt = 0;
                            dt.Rows.Add(dr);
                            dr = dt.NewRow();
                            dr[rcnt++] = "Totals" + " " + info.vkmyear;
                            dr[rcnt++] = info.P_Overall_Totals;
                            dr[rcnt++] = info.U_Overall_Totals;
                            dr[rcnt++] = info.P_Overall_Totals != 0 ? (info.U_Overall_Totals / info.P_Overall_Totals) * 100 : 0;


                            //rcnt++; 
                            dt.Rows.Add(dr);
                            // dr = dt.NewRow();
                            rcnt = 0;
                        }


                        //dt.Rows.Add(dr);
                        for (int i = 0; i <= dt.Columns.Count - 1; i++)
                        {
                            _col.Add(new columnsinfo { title = dt.Columns[i].ColumnName, data = dt.Columns[i].ColumnName });
                        }

                        string col = (string)serializer.Serialize(_col);
                        t.columns = col;


                        var lst = dt.AsEnumerable()
                        .Select(r => r.Table.Columns.Cast<DataColumn>()
                                .Select(c => new KeyValuePair<string, object>(c.ColumnName, r[c.Ordinal])
                               ).ToDictionary(z => z.Key, z => z.Value)
                        ).ToList();

                        string data = serializer.Serialize(lst);
                        t.data = data;
                    }
                    else
                    {
                        System.Data.DataTable dt = new System.Data.DataTable();
                        dt.Columns.Add("CostElement", typeof(string)); /// Planned + (year variable) / Utilized  + (year variable)
                        dt.Columns.Add("BudgetCode", typeof(string));
                        //dt.Columns.Add("VKM 2020", typeof(string));
                        string yrs = string.Empty;
                        foreach (string yr in years)
                        {

                            dt.Columns.Add(yr + " Plan", typeof(string)); //add vkm text to yr
                            dt.Columns.Add(yr + " Used", typeof(string));
                            dt.Columns.Add(yr + " Projected", typeof(string));
                            dt.Columns.Add(yr + " Unused", typeof(string));

                        }

                        dt.Columns.Add("Plan 🠕🠗", typeof(string)); //"Plan(" + String.Join("-", years.ToArray()) + ") Change"
                        dt.Columns.Add("Plan % 🠕🠗 ", typeof(string));
                        //dt.Columns.Add("Used 🠕🠗", typeof(string));
                        //dt.Columns.Add("Used % 🠕🠗", typeof(string));
                        DataRow dr;
                        string prevCostElement = "";
                        if (dtSummary.Rows.Count > 0)
                        {
                            for (int rownum = 0; rownum < dtSummary.Rows.Count; rownum++)
                            {
                                dr = dt.NewRow();
                                for (int colnum = 0; colnum < dtSummary.Columns.Count; colnum++)
                                {
                                    if (colnum == 0 || colnum == 1)
                                    {
                                        if (colnum == 0)
                                        {
                                            if (prevCostElement != dtSummary.Rows[rownum][colnum].ToString())
                                                dr[colnum] = dtSummary.Rows[rownum][colnum].ToString();
                                            else
                                                dr[colnum] = "";
                                        }
                                        else
                                        {
                                            dr[colnum] = dtSummary.Rows[rownum][colnum].ToString();
                                        }
                                        prevCostElement = dtSummary.Rows[rownum][0].ToString();
                                    }
                                    else if (colnum == 11)
                                    {
                                        if (rownum == dtSummary.Rows.Count - 1)
                                        {
                                            if (Convert.ToDecimal(dtSummary.Rows[rownum][2]) > 0)
                                                dr[colnum] = (Math.Round((((decimal)(dtSummary.Rows[rownum][6]) - (decimal)(dtSummary.Rows[rownum][2])) / (decimal)(dtSummary.Rows[rownum][2])) * 100, MidpointRounding.AwayFromZero)).ToString() + "%";
                                            else
                                                dr[colnum] = "0.00%";
                                        }
                                        else
                                        {
                                            //dr[colnum] = Convert.ToDecimal(dtSummary.Rows[rownum][colnum]).ToString("C0", CultureInfo.CurrentCulture) + '%';
                                            dr[colnum] = Convert.ToDecimal(dtSummary.Rows[rownum][colnum]).ToString() + '%';
                                        }
                                    }
                                    else if (colnum == 3 || colnum == 4 || colnum == 5)
                                    {
                                        //dr[colnum] = Convert.ToDecimal(dtSummary.Rows[rownum][colnum]).ToString("C0", CultureInfo.CurrentCulture);
                                        //dr[colnum] = Math.Round(Convert.ToDecimal(dtSummary.Rows[rownum][colnum]), MidpointRounding.AwayFromZero).ToString("C0", CultureInfo.CurrentCulture) + " (" + (Math.Round((Convert.ToDecimal(dtSummary.Rows[rownum][colnum]) / Convert.ToDecimal(dtSummary.Rows[rownum][2])) * 100, MidpointRounding.AwayFromZero)).ToString() + "%) ";
                                        if (Convert.ToDecimal(dtSummary.Rows[rownum][2]) > 0)
                                            dr[colnum] = Math.Round((decimal)(dtSummary.Rows[rownum][colnum]), MidpointRounding.AwayFromZero).ToString("C0", CultureInfo.CurrentCulture) + " (" + (Math.Round(((decimal)(dtSummary.Rows[rownum][colnum]) / (decimal)(dtSummary.Rows[rownum][2])) * 100, MidpointRounding.AwayFromZero)).ToString() + "%) ";
                                        else
                                            dr[colnum] = Math.Round((decimal)(dtSummary.Rows[rownum][colnum]), MidpointRounding.AwayFromZero).ToString("C0", CultureInfo.CurrentCulture) + " (0.00%) ";
                                    }
                                    
                                    else
                                    {
                                        dr[colnum] = Math.Round((decimal)(dtSummary.Rows[rownum][colnum]), MidpointRounding.AwayFromZero).ToString("C0", CultureInfo.CurrentCulture);
                                    }
                                }
                                dt.Rows.Add(dr);
                            }
                        }


                        //dr = dt.NewRow();
                        //dr[0] = "MAE";
                        //int rcnt = 1;
                        //int rcnt1 = 2; //2
                        //int rcnt2 = 3;
                        //int rcnt3 = 4;

                        //foreach (var info in viewList)
                        //{
                        //   if(rcnt == 1)//loop begins - the upcoming data would be for year[0] - hence need to have % detail
                        //    {
                        //        dr[rcnt] = info.P_MAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
                        //        dr[rcnt1] = info.U_MAE_Totals.ToString("C0", CultureInfo.CurrentCulture)        + " (" + Math.Round(Pr_Used_MAE_Totals) + "%)";
                        //        dr[rcnt2] = info.Proj_MAE_Totals.ToString("C0", CultureInfo.CurrentCulture)     + " (" + Math.Round(Pr_Projected_MAE_Totals) + "%)";
                        //        dr[rcnt3] = info.Unused_MAE_Totals.ToString("C0", CultureInfo.CurrentCulture)   + " (" + Math.Round(Pr_Unused_MAE_Totals) + "%)";

                        //    }
                        //    else
                        //    {
                        //        dr[rcnt] = info.P_MAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
                        //        dr[rcnt1] = info.U_MAE_Totals.ToString("C0", CultureInfo.CurrentCulture)     ;
                        //        dr[rcnt2] = info.Proj_MAE_Totals.ToString("C0", CultureInfo.CurrentCulture)  ;
                        //        dr[rcnt3] = info.Unused_MAE_Totals.ToString("C0", CultureInfo.CurrentCulture);

                        //    }

                        //    if ((viewList.Count * 4 != rcnt3))
                        //    {
                        //        rcnt = rcnt + 4;
                        //        rcnt1 = rcnt1 + 4;
                        //        rcnt2 = rcnt2 + 4;
                        //        rcnt3 = rcnt3 + 4;
                        //    }
                        //    else
                        //    {
                        //        rcnt3 = rcnt3 + 1;
                        //    }
                        //}
                        //dr[rcnt3] = Ch_P_MAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
                        //dr[rcnt3 + 1] = Math.Round(Pr_P_MAE_Totals, 2) + "%";
                        ////dr[rcnt3 + 2] = Ch_U_MAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
                        ////dr[rcnt3 + 3] = Math.Round(Pr_U_MAE_Totals, 2) + "%";
                        //dt.Rows.Add(dr);

                        //dr = dt.NewRow();
                        //dr[0] = "Non-MAE";
                        //int r1cnt = 1;
                        //int r1cnt1 = 2;
                        //int r1cnt2 = 3;
                        //int r1cnt3 = 4;
                        //foreach (var info in viewList)
                        //{
                        //    if(r1cnt == 1)
                        //    {
                        //        dr[r1cnt] = info.P_NMAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
                        //        dr[r1cnt1] = info.U_NMAE_Totals.ToString("C0", CultureInfo.CurrentCulture)       + " (" + Math.Round(Pr_Used_NMAE_Totals) + "%)";
                        //        dr[r1cnt2] = info.Proj_NMAE_Totals.ToString("C0", CultureInfo.CurrentCulture)    + " (" + Math.Round(Pr_Projected_NMAE_Totals) + "%)";
                        //        dr[r1cnt3] = info.Unused_NMAE_Totals.ToString("C0", CultureInfo.CurrentCulture) + " (" + Math.Round(Pr_Unused_NMAE_Totals) + "%)";

                        //    }
                        //    else
                        //    {
                        //        dr[r1cnt] = info.P_NMAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
                        //        dr[r1cnt1] = info.U_NMAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
                        //        dr[r1cnt2] = info.Proj_NMAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
                        //        dr[r1cnt3] = info.Unused_NMAE_Totals.ToString("C0", CultureInfo.CurrentCulture);

                        //    }

                        //    //if (r1cnt2 == 6)
                        //    //{
                        //    //    //column in vhich prev yr projected occur - not to sho proj for prev yr - hence skip
                        //    //}
                        //    //else
                        //    //{
                        //    //    dr[r1cnt2] = (info.P_NMAE_Totals - (info.U_NMAE_Totals + info.Cancelled_NMAE_Totals)).ToString("C0", CultureInfo.CurrentCulture);
                        //    //}

                        //    if ((viewList.Count * 4 != r1cnt3))
                        //    {
                        //        r1cnt = r1cnt + 4;
                        //        r1cnt1 = r1cnt1 + 4;
                        //        r1cnt2 = r1cnt2 + 4;
                        //        r1cnt3 = r1cnt3 + 4;
                        //    }
                        //    else
                        //    {
                        //        r1cnt3 = r1cnt3 + 1;
                        //    }
                        //}
                        //dr[r1cnt3] = Ch_P_NMAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
                        //dr[r1cnt3 + 1] = Math.Round(Pr_P_NMAE_Totals, 2) + "%";
                        ////dr[r1cnt3 + 2] = Ch_U_NMAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
                        ////dr[r1cnt3 + 3] = Math.Round(Pr_U_NMAE_Totals, 2) + "%";
                        //dt.Rows.Add(dr);




                        //dr = dt.NewRow();
                        //dr[0] = "Non-MAE";
                        // r1cnt = 1;
                        // r1cnt1 = 2;
                        // r1cnt2 = 3;
                        // r1cnt3 = 4;
                        //foreach (var info in viewList)
                        //{
                        //    if (r1cnt == 1)
                        //    {
                        //        dr[r1cnt] = info.P_NMAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
                        //        dr[r1cnt1] = info.U_NMAE_Totals.ToString("C0", CultureInfo.CurrentCulture) + " (" + Math.Round(Pr_Used_NMAE_Totals) + "%)";
                        //        dr[r1cnt2] = info.Proj_NMAE_Totals.ToString("C0", CultureInfo.CurrentCulture) + " (" + Math.Round(Pr_Projected_NMAE_Totals) + "%)";
                        //        dr[r1cnt3] = info.Unused_NMAE_Totals.ToString("C0", CultureInfo.CurrentCulture) + " (" + Math.Round(Pr_Unused_NMAE_Totals) + "%)";

                        //    }
                        //    else
                        //    {
                        //        dr[r1cnt] = info.P_NMAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
                        //        dr[r1cnt1] = info.U_NMAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
                        //        dr[r1cnt2] = info.Proj_NMAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
                        //        dr[r1cnt3] = info.Unused_NMAE_Totals.ToString("C0", CultureInfo.CurrentCulture);

                        //    }

                        //    //if (r1cnt2 == 6)
                        //    //{
                        //    //    //column in vhich prev yr projected occur - not to sho proj for prev yr - hence skip
                        //    //}
                        //    //else
                        //    //{
                        //    //    dr[r1cnt2] = (info.P_NMAE_Totals - (info.U_NMAE_Totals + info.Cancelled_NMAE_Totals)).ToString("C0", CultureInfo.CurrentCulture);
                        //    //}

                        //    if ((viewList.Count * 4 != r1cnt3))
                        //    {
                        //        r1cnt = r1cnt + 4;
                        //        r1cnt1 = r1cnt1 + 4;
                        //        r1cnt2 = r1cnt2 + 4;
                        //        r1cnt3 = r1cnt3 + 4;
                        //    }
                        //    else
                        //    {
                        //        r1cnt3 = r1cnt3 + 1;
                        //    }
                        //}
                        //dr[r1cnt3] = Ch_P_NMAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
                        //dr[r1cnt3 + 1] = Math.Round(Pr_P_NMAE_Totals, 2) + "%";
                        ////dr[r1cnt3 + 2] = Ch_U_NMAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
                        ////dr[r1cnt3 + 3] = Math.Round(Pr_U_NMAE_Totals, 2) + "%";
                        //dt.Rows.Add(dr);

                        //dr = dt.NewRow();
                        //dr[0] = "Software";
                        //int r2cnt = 1;
                        //int r2cnt1 = 2;
                        //int r2cnt2 = 3;
                        //int r2cnt3 = 4;
                        //foreach (var info in viewList)
                        //{
                        //    if(r2cnt == 1)
                        //    {
                        //        dr[r2cnt] = info.P_Software_Totals.ToString("C0", CultureInfo.CurrentCulture);
                        //        dr[r2cnt1] = info.U_Software_Totals.ToString("C0", CultureInfo.CurrentCulture) + " (" + Math.Round(Pr_Used_SoftwareTotals) + "%)";
                        //        dr[r2cnt2] = info.Proj_Software_Totals.ToString("C0", CultureInfo.CurrentCulture)   + " (" + Math.Round(Pr_Projected_SoftwareTotals) + "%)";
                        //        dr[r2cnt3] = info.Unused_Software_Totals.ToString("C0", CultureInfo.CurrentCulture) + " (" + Math.Round(Pr_Unused_SoftwareTotals) + "%)";

                        //    }
                        //    else
                        //    {
                        //        dr[r2cnt] = info.P_Software_Totals.ToString("C0", CultureInfo.CurrentCulture);
                        //        dr[r2cnt1] = info.U_Software_Totals.ToString("C0", CultureInfo.CurrentCulture);
                        //        dr[r2cnt2] = info.Proj_Software_Totals.ToString("C0", CultureInfo.CurrentCulture);
                        //        dr[r2cnt3] = info.Unused_Software_Totals.ToString("C0", CultureInfo.CurrentCulture);

                        //    }
                        //    //dr[r2cnt2] = (info.P_Software_Totals - (info.U_Software_Totals + info.Cancelled_Software_Totals)).ToString("C0", CultureInfo.CurrentCulture);
                        //    //rcnt++;
                        //    //rcnt = rcnt+ 2;
                        //    //rcnt1 = rcnt1 + 2;
                        //    //if (r2cnt2 == 6)
                        //    //{
                        //    //    //column in vhich prev yr projected occur - not to sho proj for prev yr - hence skip
                        //    //}
                        //    //else
                        //    //{
                        //    //    dr[r2cnt2] = (info.P_Software_Totals - (info.U_Software_Totals + info.Cancelled_Software_Totals)).ToString("C0", CultureInfo.CurrentCulture);
                        //    //}
                        //    if ((viewList.Count * 4 != r2cnt3))
                        //    {
                        //        r2cnt = r2cnt + 4;
                        //        r2cnt1 = r2cnt1 + 4;
                        //        r2cnt2 = r2cnt2 + 4;
                        //        r2cnt3 = r2cnt3 + 4;
                        //    }
                        //    else
                        //    {
                        //        r2cnt3 = r2cnt3 + 1;
                        //    }
                        //}
                        //dr[r2cnt3] = Ch_P_SoftwareTotals.ToString("C0", CultureInfo.CurrentCulture);
                        //dr[r2cnt3 + 1] = Math.Round(Pr_P_SoftwareTotals, 2) + "%";
                        ////dr[r2cnt3 + 2] = Ch_U_SoftwareTotals.ToString("C0", CultureInfo.CurrentCulture);
                        ////dr[r2cnt3 + 3] = Math.Round(Pr_U_SoftwareTotals, 2) + "%";
                        //dt.Rows.Add(dr);

                        //dr = dt.NewRow();
                        //dr[0] = "Totals";
                        //int r3cnt = 1;
                        //int r3cnt1 = 2;
                        //int r3cnt2 = 3;
                        //int r3cnt3 = 4;
                        //foreach (var info in viewList)
                        //{
                        //    if(r3cnt == 1)
                        //    {
                        //        dr[r3cnt] = (info.P_MAE_Totals + info.P_NMAE_Totals + info.P_Software_Totals).ToString("C0", CultureInfo.CurrentCulture);
                        //        dr[r3cnt1] = (info.U_MAE_Totals + info.U_NMAE_Totals + info.U_Software_Totals).ToString("C0", CultureInfo.CurrentCulture) + " (" + Math.Round(Pr_Used_OverallTotals) + "%)";
                        //        dr[r3cnt2] = info.Proj_Overall_Totals.ToString("C0", CultureInfo.CurrentCulture)                                          + " (" + Math.Round(Pr_Projected_OverallTotals) + "%)";
                        //        dr[r3cnt3] = info.Unused_Overall_Totals.ToString("C0", CultureInfo.CurrentCulture) + " (" + Math.Round(Pr_Unused_OverallTotals) + "%)";

                        //    }
                        //    else
                        //    {
                        //        dr[r3cnt] = (info.P_MAE_Totals + info.P_NMAE_Totals + info.P_Software_Totals).ToString("C0", CultureInfo.CurrentCulture);
                        //        dr[r3cnt1] = (info.U_MAE_Totals + info.U_NMAE_Totals + info.U_Software_Totals).ToString("C0", CultureInfo.CurrentCulture);
                        //        dr[r3cnt2] = info.Proj_Overall_Totals.ToString("C0", CultureInfo.CurrentCulture);
                        //        dr[r3cnt3] = info.Unused_Overall_Totals.ToString("C0", CultureInfo.CurrentCulture);

                        //    }

                        //    //dr[r3cnt2] = ((info.P_MAE_Totals - (info.U_MAE_Totals + info.Cancelled_MAE_Totals)) +
                        //    //    (info.P_NMAE_Totals - (info.U_NMAE_Totals + info.Cancelled_NMAE_Totals)) +
                        //    //    (info.P_Software_Totals - (info.U_Software_Totals + info.Cancelled_Software_Totals))).ToString("C0", CultureInfo.CurrentCulture);
                        //    //if (r3cnt2 == 6)
                        //    //{
                        //    //    //column in vhich prev yr projected occur - not to sho proj for prev yr - hence skip
                        //    //}
                        //    //else
                        //    //{
                        //    //    dr[r3cnt2] = ((info.P_MAE_Totals - (info.U_MAE_Totals + info.Cancelled_MAE_Totals)) +
                        //    //   (info.P_NMAE_Totals - (info.U_NMAE_Totals + info.Cancelled_NMAE_Totals)) +
                        //    //   (info.P_Software_Totals - (info.U_Software_Totals + info.Cancelled_Software_Totals))).ToString("C0", CultureInfo.CurrentCulture);
                        //    //}
                        //    //rcnt++;
                        //    //rcnt = rcnt+ 2;
                        //    //rcnt1 = rcnt1 + 2;
                        //    if ((viewList.Count * 4 != r3cnt3))
                        //    {
                        //        r3cnt = r3cnt + 4;
                        //        r3cnt1 = r3cnt1 + 4;
                        //        r3cnt2 = r3cnt2 + 4;
                        //        r3cnt3 = r3cnt3 + 4;
                        //    }
                        //    else
                        //    {
                        //        r3cnt3 = r3cnt3 + 1;
                        //    }
                        //}
                        //dr[r3cnt3] = Ch_P_OverallTotals.ToString("C0", CultureInfo.CurrentCulture);
                        //dr[r3cnt3 + 1] = Math.Round(Pr_P_OverallTotals, 2) + "%";
                        ////dr[r3cnt3 + 2] = Ch_U_OverallTotals.ToString("C0", CultureInfo.CurrentCulture);
                        ////dr[r3cnt3 + 3] = Math.Round(Pr_U_OverallTotals, 2) + "%";
                        //dt.Rows.Add(dr);


                        for (int i = 0; i <= dt.Columns.Count - 1; i++)
                        {
                            _col.Add(new columnsinfo { title = dt.Columns[i].ColumnName, data = dt.Columns[i].ColumnName });
                        }

                        string col = (string)serializer.Serialize(_col);
                        t.columns = col;


                        var lst = dt.AsEnumerable()
                        .Select(r => r.Table.Columns.Cast<DataColumn>()
                                .Select(c => new KeyValuePair<string, object>(c.ColumnName, r[c.Ordinal])
                               ).ToDictionary(z => z.Key, z => z.Value)
                        ).ToList();

                        string data = serializer.Serialize(lst);
                        t.data = data;


                    }

                    //}
                    //else
                    //{

                    //    t.data = string.Empty;
                    //    t.columns = string.Empty;
                    //    t.jsondata = string.Empty;


                    //}

                    return Json(new { data = t, success = true, message = selected_ccxc, PlanTotal = OverAll_Plan, UtilTotal = OverAll_Util, vkm_overall = vkm_overall }, JsonRequestBehavior.AllowGet);

                    // return View();
                }
            }
            catch(Exception ex)
            {
                
                return Json(new { data = "", success = false, message = ex.Message.ToString(), PlanTotal = 0, UtilTotal = 0, vkm_overall = 0 }, JsonRequestBehavior.AllowGet);
            }
        }


        private List<vkmSummary> GetTotals(List<string> years, List<string> buList, bool chart = false, string selected_ccxc = "")
        {
            //At times, the list may empty on staying in a pg longer / verification of the lists !=  empty
            if (lstBUs == null || lstBUs.Count == 0)
                InitialiseBudgeting();
            List<vkmSummary> viewSummaryList = new List<vkmSummary>();
            var bu_string = string.Join(",", buList.ToArray());



            using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

            //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

            {
                List<RequestItems_Table> reqList = new List<RequestItems_Table>();
                List<RequestItems_Table> reqList_forquery = new List<RequestItems_Table>();



                decimal OP_MAE_Totals = 0, OP_NMAE_Totals = 0, OP_SoftwareTotals = 0;
                decimal OU_MAE_Totals = 0, OU_NMAE_Totals = 0, OU_SoftwareTotals = 0;
                decimal OProj_MAE_Totals = 0, OProj_NMAE_Totals = 0, OProj_SoftwareTotals = 0;
                decimal OUnused_MAE_Totals = 0, OUnused_NMAE_Totals = 0, OUnused_SoftwareTotals = 0;
                //foreach (var bu_item in buList)
                //{
                //    reqList.AddRange(db.RequestItems_Table.ToList<RequestItems_Table>().FindAll(ss => ss.BU.Contains(bu_item)).FindAll(v => v.Fund == null || v.Fund.Trim().Contains("2")).OrderBy(x=>x.RequestID));
                //}

                foreach (string yr in years)
                {
                    //CC XC CHECK


                    //for 2020 reqList_forquery has relevant data under the BU filtering ; but >2020 needs dept FIltering based on CC XC
                    //if(int.Parse(yr)-1 == 2020)
                    //{
                    //    reqList_forquery.AddRange(reqList.FindAll(ss => ss.BU.Contains(lstBUs.Find(bu => bu.BU.Contains("DA")).ID.ToString())));
                    //}
                    //if (yr.Trim() != "" && int.Parse(yr) - 1 > 2020)
                    //{

                    //string is_CCXC = string.Empty;
                    //bool CCXCflag = true;
                    //string presentUserDept = string.Empty;
                    //// if (selected_ccxc != "") //DATA FILTER BASED ON CCXC DROPDOWM - NOT USER'S NTID
                    //// {
                    //if (selected_ccxc.ToUpper().Contains("CC"))
                    //{
                    //    is_CCXC = "CC";
                    //    CCXCflag = true;
                    //}
                    //else if (selected_ccxc.ToUpper().Contains("XC"))
                    //{
                    //    is_CCXC = "XC";
                    //    CCXCflag = true;
                    //}
                    ////   }
                    //else                    //DATA FILTER BASED ON USER'S NTID
                    //{
                    //    presentUserDept = lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).Department;

                    //    if (presentUserDept.ToUpper().Contains("CC"))
                    //    {
                    //        is_CCXC = "CC";
                    //        CCXCflag = true;
                    //    }
                    //    else if (presentUserDept.ToUpper().Contains("XC"))
                    //    {
                    //        is_CCXC = "XC";
                    //        CCXCflag = true;
                    //    }

                    //}

                    //if (CCXCflag)
                    //{
                    //    if (is_CCXC.Contains("XC"))
                    //    {
                    //        //XC includes 2WP 
                    //        reqList_forquery.AddRange(db.RequestItems_Table.ToList().FindAll(ss => ss.BU.Contains(lstBUs.Find(bu => bu.BU.Contains("2WP")).ID.ToString())).FindAll(v => v.Fund == null || v.Fund.Trim().Contains("2")));
                    //        reqList_forquery = reqList.FindAll(dpt => lstDEPTs.Find(x => x.ID.ToString().Contains(dpt.DEPT)).DEPT.Contains(is_CCXC));

                    //    }
                    //    else
                    //    {
                    //reqList_forquery = reqList.FindAll(dpt => !lstDEPTs.Find(x => x.ID.ToString().Contains(dpt.DEPT)).DEPT.Contains("XC"));
                    //var chk_viasection = reqList.FindAll(y => y.VKM_Year == 2022);
                    //var xxx = chk_viasection.FindAll(dpt => lstUsers.Find(x => x.Department.ToUpper().Trim() == lstDEPTs.Find(xi => xi.ID.ToString().Contains(dpt.DEPT)).DEPT).Section.Contains("CC"));

                    ////var xxx = chk_viasection.FindAll(dpt => lstUsers.Find(x => x.Department.ToUpper().Trim() == lstDEPTs.Find(xi => xi.ID.ToString().Contains(dpt.DEPT)).DEPT).Section.Contains("CC"));
                    //var chk_ccdept =  reqList.FindAll(dpt => lstDEPTs.Find(x => x.ID.ToString().Contains(dpt.DEPT)).DEPT.Contains(is_CCXC));
                    DataTable dt = new DataTable();
                    connection();

                    string SelTotal = "",
                    PrevTotal = "";
                    if (bu_string != "")
                    {
                        string Query = "";
                        if (chart == true)
                        {
                            Query = "Exec [dbo].[Cockpit_GetTotals_with2023_Chart] '" + bu_string + "', '" + int.Parse(yr.Trim()) + "', '" + "', '" + "','" + "' ";
                        }
                        else
                        {
                            Query = "Exec [dbo].[Cockpit_GetTotals_with2023] '" + bu_string + "', '" + int.Parse(yr.Trim()) + "', '" + "', '" + "','" + "' ";
                        }
                        
                        //"Select * from RequestItems_Table where BU in ('" + bu_str_sql + "') and (Fund is null or Fund = 2) and ApprovedSH = 1 and DEPT in (Select id from DEPT_Table where DEPT not like '%-XC%')"; //and VKM_Year = '" + yr + "'";
                        OpenConnection();
                        SqlCommand cmd = new SqlCommand(Query, conn);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                        CloseConnection();
                        foreach (DataRow item in dt.Rows)
                        {
                            try
                            {

                                RequestItems_Table ritem = new RequestItems_Table();
                                ritem.Projected_Amount = item["Projected_Amount"].ToString() != "" ? Convert.ToDecimal(item["Projected_Amount"]) : 0;
                                ritem.Unused_Amount = item["Unused_Amount"].ToString() != "" ? Convert.ToDecimal(item["Unused_Amount"]) : 0;



                                ritem.ApprQuantity = Convert.ToInt32(item["ApprQuantity"]);
                                ritem.ApprCost = Convert.ToDecimal(item["ApprCost"]);

                                ritem.VKM_Year = Convert.ToInt32(item["VKM_Year"]);
                                if (item["UpdatedAt"].ToString() != "")
                                    ritem.UpdatedAt = Convert.ToDateTime(item["UpdatedAt"]);
                                if (item["RequestorNTID"].ToString() != "")
                                    ritem.RequestorNTID = item["RequestorNTID"].ToString();
                                ritem.Category = item["Category"].ToString();
                                ritem.Comments = item["Comments"].ToString();
                                ritem.Project = item["Project"].ToString();
                                if (item["ActualAvailableQuantity"].ToString() == "")
                                    ritem.ActualAvailableQuantity = "NA";
                                else
                                    ritem.ActualAvailableQuantity = item["ActualAvailableQuantity"].ToString();

                                ritem.CostElement = item["CostElement"].ToString();
                                ritem.BU = item["BU"].ToString();

                                ritem.DEPT = item["DEPT"].ToString();
                                ritem.Group = item["Group"].ToString();
                                ritem.ItemName = item["ItemName"].ToString();
                                ritem.OEM = item["OEM"].ToString();
                                ritem.ReqQuantity = Convert.ToInt32(item["ReqQuantity"]);
                                ritem.RequestID = Convert.ToInt32(item["RequestID"]);

                                ritem.RequestorNT = item["RequestorNT"].ToString();
                                ritem.TotalPrice = Convert.ToDecimal(item["TotalPrice"]);
                                ritem.OrderPrice = item["OrderPrice"].ToString() != "" ? Convert.ToDecimal(item["OrderPrice"]) : 0;
                                ritem.UnitPrice = Convert.ToDecimal(item["UnitPrice"]);
                                ritem.ApprovalDH = Convert.ToBoolean(item["ApprovalDH"]);
                                ritem.ApprovalSH = Convert.ToBoolean(item["ApprovalSH"]);
                                ritem.ApprovedDH = Convert.ToBoolean(item["ApprovedDH"]);
                                ritem.ApprovedSH = Convert.ToBoolean(item["ApprovedSH"]);
                                if (item["IsCancelled"].ToString() != "")
                                    ritem.isCancelled = Convert.ToInt32(item["IsCancelled"]);

                                ritem.DHNT = item["DHNT"].ToString();
                                ritem.SHNT = item["SHNT"].ToString();

                                if (item["RequestDate"].ToString() != "")
                                    ritem.RequestDate = (DateTime)item["RequestDate"];

                                if (item["SubmitDate"].ToString() != "")
                                    ritem.SubmitDate = (DateTime)item["SubmitDate"];

                                if (item["DHAppDate"].ToString() != "")
                                    ritem.DHAppDate = (DateTime)item["DHAppDate"];

                                if (item["SHAppDate"].ToString() != "")
                                    ritem.SHAppDate = (DateTime)item["SHAppDate"];


                                if (item["OrderStatus"].ToString().Trim() != "")
                                {
                                    ritem.OrderStatus = (item["OrderStatus"]).ToString();

                                }
                                else
                                {
                                    ritem.OrderStatus = null;


                                }
                                if (item["Project"].ToString() == "")
                                    ritem.Project = string.Empty;
                                else
                                    ritem.Project = item["Project"].ToString();

                                ritem.BudgetCode = item["BudgetCode"].ToString();

                                reqList_forquery.Add(ritem);

                            }
                            catch (Exception ex)
                            {

                            }

                        }

                    }

                    //        }

                    //        CCXCflag = false;
                    //    }



                    //}
                    //else
                    //{
                    //    if (buList.Contains("2") && buList.Contains("4") /*&& selected_ccxc.ToUpper().Contains("Custom") || selected_ccxc.ToUpper().Contains("XC"))*/ && yr == "2021" && reqList.FindAll(c => c.BU == "1").Count == 0) //XC VKM2021 - include 2WP Whole Totals also
                    //    {
                    //        reqList.AddRange(db.RequestItems_Table.ToList().FindAll(ss => ss.BU.Contains(lstBUs.Find(bu => bu.BU.Contains("2WP")).ID.ToString())).FindAll(v => v.Fund == null || v.Fund.Trim().Contains("2")));
                    //    }
                    //    reqList_forquery = reqList;


                    //}
                    // return Json(new { data = reqList_forquery.FindAll(y=>y.ApprovalSH == true), message = selected_ccxc }, JsonRequestBehavior.AllowGet);
                    //CODE TO GET GROUPS OF COST ELEMENT

                    IEnumerable<IGrouping<string, RequestItems_Table>> query = reqList_forquery.FindAll(item => item.CostElement.Trim() != "0").GroupBy(item => item.CostElement);

                    //IEnumerable<IGrouping<string, RequestItems_Table>> query = reqList_forquery.FindAll(item => item.CostElement.Trim() != "0").GroupBy(item => new { item.CostElement.ToString(), item.BudgetCode.ToString() });

                    decimal P_MAE_Totals = 0, P_NMAE_Totals = 0, P_SoftwareTotals = 0;
                    decimal U_MAE_Totals = 0, U_NMAE_Totals = 0, U_SoftwareTotals = 0;
                    decimal Proj_MAE_Totals = 0, Proj_NMAE_Totals = 0, Proj_SoftwareTotals = 0;
                    decimal Unused_MAE_Totals = 0, Unused_NMAE_Totals = 0, Unused_SoftwareTotals = 0;
                    decimal Cancelled_MAE_Totals = 0, Cancelled_NMAE_Totals = 0, Cancelled_Software_Totals = 0;

                    //Cancelled_MAE_Totals = reqList_forquery.FindAll(X => X.CostElement.Trim() == "1" && X.OrderStatus != null).Where(item => item.OrderStatus.Trim() == lstOrderStatus.Find(status => status.OrderStatus.Trim().Equals("Cancelled")).ID.ToString()).Count() > 0 ?
                    //    (decimal)reqList_forquery.FindAll(X => X.CostElement.Trim() == "1" && X.OrderStatus != null).Where(item => item.OrderStatus.Trim() == lstOrderStatus.Find(status => status.OrderStatus.Trim().Equals("Cancelled")).ID.ToString()).Sum(x => Math.Round((double)x.ApprCost, MidpointRounding.AwayFromZero)) :
                    //    0;
                    //Cancelled_NMAE_Totals = reqList_forquery.FindAll(X => X.CostElement.Trim() == "2" && X.OrderStatus != null).Where(item => item.OrderStatus.Trim() == lstOrderStatus.Find(status => status.OrderStatus.Trim().Equals("Cancelled")).ID.ToString()).Count() > 0 ?
                    //    (decimal)reqList_forquery.FindAll(X => X.CostElement.Trim() == "2" && X.OrderStatus != null).Where(item => item.OrderStatus.Trim() == lstOrderStatus.Find(status => status.OrderStatus.Trim().Equals("Cancelled")).ID.ToString()).Sum(x => Math.Round((double)x.ApprCost, MidpointRounding.AwayFromZero)) :
                    //    0;
                    //Cancelled_Software_Totals = reqList_forquery.FindAll(X => X.CostElement.Trim() == "3" && X.OrderStatus != null).Where(item => item.OrderStatus.Trim() == lstOrderStatus.Find(status => status.OrderStatus.Trim().Equals("Cancelled")).ID.ToString()).Count() > 0 ?
                    //    (decimal)reqList_forquery.FidAll(X => X.CostElement.Trim() == "3" && X.OrderStatus != null).Where(item => item.OrderStatus.Trim() == lstOrderStatus.Find(status => status.OrderStatus.Trim().Equals("Cancelled")).ID.ToString()).Sum(x => Math.Round((double)x.ApprCost, MidpointRounding.AwayFromZero)) :
                    //    0;
                    ////var danger1 = string.Empty;
                    vkmSummary tempobj = new vkmSummary();
                    tempobj.vkmyear = yr;


                    //CODE TO GET THE TOTALS OF EACH COST ELEMENT
                    foreach (IGrouping<string, RequestItems_Table> CostGroup in query)
                    {

                        // Iterate over each value in the
                        // IGrouping and print the value.

                        if (lstCostElement.Count != 0)
                        {
                            if (lstCostElement.Find(cost => cost.ID.Equals(Int32.Parse(CostGroup.Key))).CostElement == "MAE")
                            {
                                foreach (RequestItems_Table item in CostGroup)
                                {
                                    if (item.VKM_Year.ToString().Contains((int.Parse(yr).ToString())) && item.ApprovalSH == true)  //yr is VKM Year; if yr == 2020 - 2021 Planning (2020 sh apprvd - 2021 planning)
                                    {

                                        P_MAE_Totals += Math.Round((decimal)item.ApprCost, MidpointRounding.AwayFromZero);
                                        U_MAE_Totals += item.OrderPrice == null ? (decimal)0.0 : Math.Round((decimal)item.OrderPrice, MidpointRounding.AwayFromZero); ;
                                        Proj_MAE_Totals += item.Projected_Amount == null ? (decimal)0.0 : Math.Round((decimal)item.Projected_Amount, MidpointRounding.AwayFromZero); ;
                                        Unused_MAE_Totals += item.Unused_Amount == null ? (decimal)0.0 : Math.Round((decimal)item.Unused_Amount, MidpointRounding.AwayFromZero); ;


                                    }
                                }
                            }
                            else if (lstCostElement.Find(cost => cost.ID.Equals(Int32.Parse(CostGroup.Key))).CostElement == "Non-MAE")
                            {
                                foreach (RequestItems_Table item in CostGroup)
                                {
                                    if (item.VKM_Year.ToString().Contains((int.Parse(yr).ToString())) && item.ApprovalSH == true)
                                    {
                                        if (item.Projected_Amount < 0
                                            )
                                        {

                                        }
                                        P_NMAE_Totals += Math.Round((decimal)item.ApprCost, MidpointRounding.AwayFromZero);

                                        U_NMAE_Totals += item.OrderPrice == null ? (decimal)0.0 : Math.Round((decimal)item.OrderPrice, MidpointRounding.AwayFromZero); ;
                                        Proj_NMAE_Totals += item.Projected_Amount == null ? (decimal)0.0 : Math.Round((decimal)item.Projected_Amount, MidpointRounding.AwayFromZero); ;
                                        Unused_NMAE_Totals += item.Unused_Amount == null ? (decimal)0.0 : Math.Round((decimal)item.Unused_Amount, MidpointRounding.AwayFromZero); ;

                                    }
                                }

                            }
                            else if (lstCostElement.Find(cost => cost.ID.Equals(Int32.Parse(CostGroup.Key))).CostElement == "Software")
                            {
                                foreach (RequestItems_Table item in CostGroup)
                                {
                                    if (item.VKM_Year.ToString().Contains((int.Parse(yr).ToString())) && item.ApprovalSH == true)
                                    {
                                        if (item.Projected_Amount < 0
                                            )
                                        {

                                        }

                                        P_SoftwareTotals += Math.Round((decimal)item.ApprCost, MidpointRounding.AwayFromZero);

                                        U_SoftwareTotals += item.OrderPrice == null ? (decimal)0.0 : Math.Round((decimal)item.OrderPrice, MidpointRounding.AwayFromZero); ;
                                        Proj_SoftwareTotals += item.Projected_Amount == null ? (decimal)0.0 : Math.Round((decimal)item.Projected_Amount, MidpointRounding.AwayFromZero); ;
                                        Unused_SoftwareTotals += item.Unused_Amount == null ? (decimal)0.0 : Math.Round((decimal)item.Unused_Amount, MidpointRounding.AwayFromZero); ;

                                    }
                                }

                            }
                        }
                        else
                            continue;
                    }

                    tempobj.P_MAE_Totals = P_MAE_Totals;
                    OP_MAE_Totals += P_MAE_Totals;
                    tempobj.P_NMAE_Totals = P_NMAE_Totals;
                    OP_NMAE_Totals += P_NMAE_Totals;
                    tempobj.P_Software_Totals = P_SoftwareTotals;
                    OP_SoftwareTotals += P_SoftwareTotals;

                    tempobj.P_Overall_Totals = P_MAE_Totals + P_NMAE_Totals + P_SoftwareTotals;

                    tempobj.U_MAE_Totals = U_MAE_Totals;
                    OU_MAE_Totals += U_MAE_Totals;
                    tempobj.U_NMAE_Totals = U_NMAE_Totals;
                    OU_NMAE_Totals += U_NMAE_Totals;
                    tempobj.U_Software_Totals = U_SoftwareTotals;
                    OU_SoftwareTotals += U_SoftwareTotals;
                    tempobj.U_Overall_Totals = U_MAE_Totals + U_NMAE_Totals + U_SoftwareTotals;


                    tempobj.Proj_MAE_Totals = Proj_MAE_Totals + U_MAE_Totals;
                    OProj_MAE_Totals += Proj_MAE_Totals + U_MAE_Totals;
                    tempobj.Proj_NMAE_Totals = Proj_NMAE_Totals + U_NMAE_Totals;
                    OProj_NMAE_Totals += Proj_NMAE_Totals + U_NMAE_Totals;
                    tempobj.Proj_Software_Totals = Proj_SoftwareTotals + U_SoftwareTotals;
                    OProj_SoftwareTotals += Proj_SoftwareTotals + U_SoftwareTotals;
                    tempobj.Proj_Overall_Totals = Proj_MAE_Totals + Proj_NMAE_Totals + Proj_SoftwareTotals + U_MAE_Totals + U_NMAE_Totals + U_SoftwareTotals;


                    tempobj.Unused_MAE_Totals = Unused_MAE_Totals;
                    OUnused_MAE_Totals += Unused_MAE_Totals;
                    tempobj.Unused_NMAE_Totals = Unused_NMAE_Totals;
                    OUnused_NMAE_Totals += Unused_NMAE_Totals;
                    tempobj.Unused_Software_Totals = Unused_SoftwareTotals;
                    OUnused_SoftwareTotals += Unused_SoftwareTotals;
                    tempobj.Unused_Overall_Totals = Unused_MAE_Totals + Unused_NMAE_Totals + Unused_SoftwareTotals;

                    //tempobj.Cancelled_MAE_Totals = Cancelled_MAE_Totals;
                    //tempobj.Cancelled_NMAE_Totals = Cancelled_NMAE_Totals;
                    //tempobj.Cancelled_Software_Totals = Cancelled_Software_Totals;

                    if (buList.Contains("5") && yr == "2021")
                    {

                        tempobj.U_MAE_Totals += db.Utilization_Table2021.ToList().Find(x => x.CostElement == (lstCostElement.Find(ct => ct.CostElement.Contains("MAE")).ID)).Utilized2021;
                        OU_MAE_Totals += U_MAE_Totals;
                        tempobj.U_NMAE_Totals += db.Utilization_Table2021.ToList().Find(x => x.CostElement == (lstCostElement.Find(ct => ct.CostElement.Contains("Non-MAE")).ID)).Utilized2021;
                        OU_NMAE_Totals += U_NMAE_Totals;
                        tempobj.U_Software_Totals += db.Utilization_Table2021.ToList().Find(x => x.CostElement == (lstCostElement.Find(ct => ct.CostElement.Contains("Software")).ID)).Utilized2021;
                        OU_SoftwareTotals += U_SoftwareTotals;
                        var tot = db.Utilization_Table2021.ToList().Find(x => x.CostElement == (lstCostElement.Find(ct => ct.CostElement.Contains("MAE")).ID)).Utilized2021
                            +
                            db.Utilization_Table2021.ToList().Find(x => x.CostElement == (lstCostElement.Find(ct => ct.CostElement.Contains("Non-MAE")).ID)).Utilized2021
                            +
                            db.Utilization_Table2021.ToList().Find(x => x.CostElement == (lstCostElement.Find(ct => ct.CostElement.Contains("Software")).ID)).Utilized2021;

                        tempobj.U_Overall_Totals += tot;


                    }
                    viewSummaryList.Add(tempobj);
                }

            }
            return viewSummaryList;
        }


        public ActionResult CostElementDrillDown_comparison(string years_str, string CostElement_Chosen, string buList_str, bool chart = false, string selected_ccxc = "")
        {
            if (lstBUs == null || lstBUs.Count == 0)
                InitialiseBudgeting();
            List<string> years = new List<string>();
            List<string> buList = new List<string>();

            if (years_str != null)
            {
                years = (years_str.Split(',')).ToList();

            }
            if (buList_str != null)
            {
                buList = (buList_str.Split(',')).ToList();

            }
            var bu_string = string.Join(",", buList.ToArray());


            TempData["years_str"] = years_str;
            TempData["buList_str"] = buList_str;
            TempData["CostElement_Chosen"] = CostElement_Chosen;

            if (lstUsers == null)
            {
                return RedirectToAction("Index", "Budgeting");
            }


            using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

            //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

            {
                List<RequestItems_Table> reqList = new List<RequestItems_Table>();
                List<RequestItems_Table> reqList_forquery = new List<RequestItems_Table>();
                List<CategoryBudget> categoryBudgets = new List<CategoryBudget>();
                List<CategoryBudget> Top70_CategoryBudget = new List<CategoryBudget>();
                List<costelement_drilldownSummary> viewList = new List<costelement_drilldownSummary>();
                //string costelement_chosen_id = string.Empty;
                //reqList = db.RequestItems_Table.ToList<RequestItems_Table>().FindAll(v => v.Fund == null || v.Fund.Trim().Contains("2"));
                int BaseYear_toCompare = 0;
                //List<RequestItemsList_2019> reqList2019 = new List<RequestItemsList_2019>();

                //Fetching the Category List
                foreach (var category in lstPrdCateg)
                {
                    CategoryBudget catbud = new CategoryBudget();
                    catbud.CategoryID = category.ID;
                    catbud.CategoryName = category.Category;
                    categoryBudgets.Add(catbud);
                }


                //CostElement_Chosen is costelt name -> fetch its id
                //costelement_chosen_id = lstCostElement.Find(cost => cost.CostElement.ToUpper().Trim() == CostElement_Chosen.ToUpper().Trim()).ID.ToString().Trim();

                //foreach (var bu_item in buList)
                //{
                //    reqList_forquery.AddRange(reqList.FindAll(item => item.CostElement.Trim() == costelement_chosen_id).FindAll(ss => ss.BU.Contains(bu_item)));
                //}

                //CODE TO GET GROUPS OF CATEGORY


                foreach (string yr in years)
                {
                    BaseYear_toCompare++;
                    //CC XC CHECK

                    //if ((int.Parse(yr) - 1) > 2020)  //for 2020 reqList_forquery has relevant data
                    //{
                    //    string is_CCXC = string.Empty;
                    //    bool CCXCflag = false;
                    //    string presentUserDept = string.Empty;

                    //    if (selected_ccxc.ToUpper().Contains("CC"))
                    //    {
                    //        is_CCXC = "CC";
                    //        CCXCflag = true;
                    //    }
                    //    else if (selected_ccxc.ToUpper().Contains("XC"))
                    //    {
                    //        is_CCXC = "XC";
                    //        CCXCflag = true;
                    //    }


                    //    else                    //DATA FILTER BASED ON USER'S NTID
                    //    {
                    //        presentUserDept = lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).Department;

                    //        if (presentUserDept.ToUpper().Contains("CC"))
                    //        {
                    //            is_CCXC = "CC";
                    //            CCXCflag = true;
                    //        }
                    //        else if (presentUserDept.ToUpper().Contains("XC"))
                    //        {
                    //            is_CCXC = "XC";
                    //            CCXCflag = true;
                    //        }

                    //    }

                    //    if (CCXCflag)
                    //    {
                    //        if (is_CCXC.Contains("XC"))
                    //        {
                    //            reqList_forquery.AddRange(reqList.FindAll(ss => ss.BU.Contains(lstBUs.Find(bu => bu.BU.Contains("2WP")).ID.ToString())));
                    //            reqList_forquery = reqList_forquery.FindAll(dpt => lstDEPTs.Find(x => x.ID.ToString().Contains(dpt.DEPT)).DEPT.Contains(is_CCXC))/*.FindAll(ss => ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("AS")).ID.ToString()) || ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("2WP")).ID.ToString()))*/;

                    //        }
                    //        else
                    //        {
                    //            reqList_forquery = reqList_forquery.FindAll(dpt => !lstDEPTs.Find(x => x.ID.ToString().Contains(dpt.DEPT)).DEPT.Contains("XC"))/*.FindAll(ss => ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("AS")).ID.ToString()) || ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("2WP")).ID.ToString()))*/;
                    if (bu_string != "")
                    {
                        DataTable dt1 = new DataTable();
                        connection();
                        string Query = "Exec [dbo].[Cockpit_GetTotals_with2023] '" + bu_string + "', '" + int.Parse(yr.Trim()) + "','" + CostElement_Chosen.Trim() + "', '" + "','" + "' ";

                        //"Select * from RequestItems_Table where BU in (1, 3, 5) and (Fund is null or Fund = 2) and ApprovedDH = 1 and DEPT in (Select id from DEPT_Table where DEPT not like '%-XC%')";
                        OpenConnection();
                        SqlCommand cmd = new SqlCommand(Query, conn);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt1);
                        CloseConnection();
                        reqList_forquery = new List<RequestItems_Table>();
                        foreach (DataRow item in dt1.Rows)
                        {
                            try
                            {

                                RequestItems_Table ritem = new RequestItems_Table();
                                ritem.Projected_Amount = item["Projected_Amount"].ToString() != "" ? Convert.ToDecimal(item["Projected_Amount"]) : 0;
                                ritem.Unused_Amount = item["Unused_Amount"].ToString() != "" ? Convert.ToDecimal(item["Unused_Amount"]) : 0;

                                ritem.ApprQuantity = Convert.ToInt32(item["ApprQuantity"]);
                                ritem.ApprCost = Convert.ToDecimal(item["ApprCost"]);

                                ritem.VKM_Year = Convert.ToInt32(item["VKM_Year"]);
                                if (item["UpdatedAt"].ToString() != "")
                                    ritem.UpdatedAt = Convert.ToDateTime(item["UpdatedAt"]);
                                if (item["RequestorNTID"].ToString() != "")
                                    ritem.RequestorNTID = item["RequestorNTID"].ToString();
                                ritem.Category = item["Category"].ToString();
                                ritem.Comments = item["Comments"].ToString();
                                ritem.Project = item["Project"].ToString();
                                if (item["ActualAvailableQuantity"].ToString() == "")
                                    ritem.ActualAvailableQuantity = "NA";
                                else
                                    ritem.ActualAvailableQuantity = item["ActualAvailableQuantity"].ToString();

                                ritem.CostElement = item["CostElement"].ToString();
                                ritem.BU = item["BU"].ToString();

                                ritem.DEPT = item["DEPT"].ToString();
                                ritem.Group = item["Group"].ToString();
                                ritem.ItemName = item["ItemName"].ToString();
                                ritem.OEM = item["OEM"].ToString();
                                ritem.ReqQuantity = Convert.ToInt32(item["ReqQuantity"]);
                                ritem.RequestID = Convert.ToInt32(item["RequestID"]);

                                ritem.RequestorNT = item["RequestorNT"].ToString();
                                ritem.TotalPrice = Convert.ToDecimal(item["TotalPrice"]);
                                ritem.OrderPrice = item["OrderPrice"].ToString() != "" ? Convert.ToDecimal(item["OrderPrice"]) : 0;
                                ritem.UnitPrice = Convert.ToDecimal(item["UnitPrice"]);
                                ritem.ApprovalDH = Convert.ToBoolean(item["ApprovalDH"]);
                                ritem.ApprovalSH = Convert.ToBoolean(item["ApprovalSH"]);
                                ritem.ApprovedDH = Convert.ToBoolean(item["ApprovedDH"]);
                                ritem.ApprovedSH = Convert.ToBoolean(item["ApprovedSH"]);
                                if (item["IsCancelled"].ToString() != "")
                                    ritem.isCancelled = Convert.ToInt32(item["IsCancelled"]);

                                ritem.DHNT = item["DHNT"].ToString();
                                ritem.SHNT = item["SHNT"].ToString();

                                if (item["RequestDate"].ToString() != "")
                                    ritem.RequestDate = (DateTime)item["RequestDate"];

                                if (item["SubmitDate"].ToString() != "")
                                    ritem.SubmitDate = (DateTime)item["SubmitDate"];

                                if (item["DHAppDate"].ToString() != "")
                                    ritem.DHAppDate = (DateTime)item["DHAppDate"];

                                if (item["SHAppDate"].ToString() != "")
                                    ritem.SHAppDate = (DateTime)item["SHAppDate"];


                                if (item["OrderStatus"].ToString().Trim() != "")
                                {
                                    ritem.OrderStatus = (item["OrderStatus"]).ToString();

                                }
                                else
                                {
                                    ritem.OrderStatus = null;


                                }
                                if (item["Project"].ToString() == "")
                                    ritem.Project = string.Empty;
                                else
                                    ritem.Project = item["Project"].ToString();


                                reqList_forquery.Add(ritem);

                            }
                            catch (Exception ex)
                            {

                            }
                        }



                        //}

                        //}
                        //CCXCflag = false;
                        //}



                        //}


                        decimal[] P_Category_Totals = new decimal[categoryBudgets.Count()];
                        decimal[] U_Category_Totals = new decimal[categoryBudgets.Count()];
                        decimal[] Proj_Category_Totals = new decimal[categoryBudgets.Count()];
                        decimal[] Unused_Category_Totals = new decimal[categoryBudgets.Count()];
                        decimal[] Cancelled_Category_Totals = new decimal[categoryBudgets.Count()];
                        costelement_drilldownSummary tempobj = new costelement_drilldownSummary();
                        tempobj.vkmyear = yr;


                        //CODE TO GET THE TOTALS OF Top Category in 2020 Year - to compare with the other years Totals
                        if (lstPrdCateg.Count != 0)
                        {

                            foreach (var catbudget in categoryBudgets)
                            {
                                foreach (RequestItems_Table item in reqList_forquery)
                                {
                                    if (item.VKM_Year.ToString().Contains((int.Parse(yr).ToString())) && item.ApprovalSH == true)   // 2020 sh apprvd - 2021 planning
                                    {
                                        // if ((int.Parse(yr) - 1).ToString() == "2020")
                                        if (BaseYear_toCompare == 1 || (BaseYear_toCompare == 2 && Top70_CategoryBudget.FindAll(i => i.CategoryTotals == 0).Count == Top70_CategoryBudget.Count))
                                        {

                                            if (int.Parse(item.Category) == catbudget.CategoryID)
                                            {


                                                if (item.ApprCost != null)
                                                {

                                                    catbudget.CategoryTotals += Math.Round((decimal)item.ApprCost, MidpointRounding.AwayFromZero);


                                                }


                                            }

                                        }



                                    }

                                }
                            }
                            Top70_CategoryBudget = categoryBudgets.OrderByDescending(x => x.CategoryTotals).Take(70).ToList();

                        }

                        //Code to get the Planned and Utilized Category Totals
                        int catcount = -1;
                        foreach (var topcat in Top70_CategoryBudget)
                        {
                            catcount++;
                            foreach (RequestItems_Table item in reqList_forquery)
                            {
                                if (item.VKM_Year.ToString().Contains((int.Parse(yr).ToString())) && item.ApprovalSH == true)
                                {
                                    if (int.Parse(item.Category) == topcat.CategoryID)
                                    {
                                        P_Category_Totals[catcount] += Math.Round((decimal)item.ApprCost, MidpointRounding.AwayFromZero);
                                        U_Category_Totals[catcount] += item.OrderPrice == null ? (decimal)0.0 : Math.Round((decimal)item.OrderPrice, MidpointRounding.AwayFromZero);
                                        Proj_Category_Totals[catcount] += item.Projected_Amount == null ? (decimal)0.0 : Math.Round((decimal)item.Projected_Amount, MidpointRounding.AwayFromZero);
                                        Unused_Category_Totals[catcount] += item.Unused_Amount == null ? (decimal)0.0 : Math.Round((decimal)item.Unused_Amount, MidpointRounding.AwayFromZero);

                                        // Cancelled_Category_Totals[catcount] += item.OrderStatus != null ? (item.OrderStatus.Trim() == lstOrderStatus.Find(status => status.OrderStatus.Trim().Equals("Cancelled")).ID.ToString() ? Math.Round((decimal)item.ApprCost, MidpointRounding.AwayFromZero) : (decimal)0.0) : (decimal)0.0;

                                    }
                                }
                            }
                        }

                        tempobj.P_Category_Totals = P_Category_Totals;
                        tempobj.U_Category_Totals = U_Category_Totals;
                        tempobj.Proj_Category_Totals = Proj_Category_Totals;
                        tempobj.Unused_Category_Totals = Unused_Category_Totals;
                        // tempobj.Cancelled_Category_Totals = Cancelled_Category_Totals;
                        viewList.Add(tempobj);
                    }
                }




                ///NEW CODE TO GET CATEGORY SUMMARY
                var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                BudgetParam t = new BudgetParam();
                List<columnsinfo> _col = new List<columnsinfo>();
                System.Data.DataTable dt = new System.Data.DataTable();
                dt.Columns.Add("Category Name", typeof(string)); /// Planned + (year variable) / Utilized  + (year variable)
                //dt.Columns.Add("VKM 2020", typeof(string));
                foreach (string year in years)
                {
                    dt.Columns.Add(" " + year + " Plan", typeof(string)); //add vkm text to yr
                    dt.Columns.Add(" " + year + " Used", typeof(string));

                    dt.Columns.Add(" " + year + " Projected", typeof(string));
                    dt.Columns.Add(" " + year + " Unused", typeof(string));

                }


                //DataRow dr = dt.NewRow();
                //dr[0] = "MAE";
                //int rcnt = 1;
                //int rcnt1 = 2; //2
                for (var i = 0; i < Top70_CategoryBudget.Count(); i++)
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = Top70_CategoryBudget[i].CategoryName.Trim().Replace("\r\n", "").Replace("\r", "").Replace("\r", "");
                    int rcnt = 1;
                    int rcnt1 = 2;
                    int rcnt2 = 3;
                    int rcnt3 = 4;
                    foreach (var info in viewList)
                    {



                        dr[rcnt] = info.P_Category_Totals[i].ToString("C", CultureInfo.CurrentCulture);
                        dr[rcnt1] = info.U_Category_Totals[i].ToString("C", CultureInfo.CurrentCulture);

                        dr[rcnt2] = info.Proj_Category_Totals[i].ToString("C", CultureInfo.CurrentCulture);
                        dr[rcnt3] = info.Unused_Category_Totals[i].ToString("C", CultureInfo.CurrentCulture);
                        //if(rcnt2 == 6)
                        //{

                        //}
                        //else
                        //   dr[rcnt2] = (info.P_Category_Totals[i] - info.U_Category_Totals[i] -  info.Cancelled_Category_Totals[i]).ToString("C", CultureInfo.CurrentCulture);
                        ////rcnt++;
                        rcnt = rcnt + 4;
                        rcnt1 = rcnt1 + 4;
                        rcnt2 = rcnt2 + 4;
                        rcnt3 = rcnt3 + 4;

                    }
                    dt.Rows.Add(dr);

                }
                //dt.Rows.Add(dr);
                // dt.Rows.Add(dr);

                //dr = dt.NewRow();
                // dr[0] = "Non-MAE";


                // dt.Rows.Add(dr);
                for (int i = 0; i <= dt.Columns.Count - 1; i++)
                {
                    _col.Add(new columnsinfo { title = dt.Columns[i].ColumnName, data = dt.Columns[i].ColumnName });
                }

                string col = (string)serializer.Serialize(_col);
                t.columns = col;


                var lst = dt.AsEnumerable()
                .Select(r => r.Table.Columns.Cast<DataColumn>()
                        .Select(c => new KeyValuePair<string, object>(c.ColumnName, r[c.Ordinal])
                       ).ToDictionary(z => z.Key, z => z.Value)
                ).ToList();

                string data = serializer.Serialize(lst);
                t.data = data;




                //return Json(new { data = t, message = selected_ccxc }, JsonRequestBehavior.AllowGet);
                TempData["data"] = t;
                return View(t);

            }
        }

        public ActionResult CategoryDrillDown_comparison(List<string> years, string CostElement_Chosen, string Category_Chosen, List<string> buList, bool chart = false, string selected_ccxc = "")
        {

            if (lstBUs == null || lstBUs.Count == 0)
                InitialiseBudgeting();


            using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

            //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

            {
                //List<RequestItems_Table> reqList = new List<RequestItems_Table>();
                List<RequestItems_Table> reqList_forquery = new List<RequestItems_Table>();
                //reqList = db.RequestItems_Table.ToList<RequestItems_Table>().FindAll(v => v.Fund == null || v.Fund.Trim().Contains("2"));
                //List<RequestItemsList_2019> reqList2019 = new List<RequestItemsList_2019>();
                List<ItemBudget> itemBudgets = new List<ItemBudget>();
                List<ItemBudget> Top_ItemBudget = new List<ItemBudget>();
                List<category_drilldownSummary> viewList = new List<category_drilldownSummary>();
                //string costelement_chosen_id = string.Empty;
                int BaseYear_toCompare = 0;
                var bu_string = string.Join(",", buList.ToArray());

                //CostElement_Chosen is costelt name -> fetch its id
                //costelement_chosen_id = lstCostElement.Find(cost => cost.CostElement.ToUpper().Trim() == CostElement_Chosen.ToUpper().Trim()).ID.ToString().Trim();

                //Category_Chosen is costelt name -> fetch its id
                var Category_Chosen_id = lstPrdCateg.Find(cat => cat.Category.ToUpper().Trim() == Category_Chosen.ToUpper().Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", "")).ID.ToString().Trim();

                foreach (var item in lstItems.FindAll(x => x.Category != null).Where(x => x.Category.Contains(Category_Chosen_id.ToString().Trim())))
                {
                    ItemBudget ibud = new ItemBudget();
                    ibud.ItemID = item.S_No;
                    ibud.ItemName = item.Item_Name;


                    if (itemBudgets.Count > 0 && itemBudgets.FindAll(ib => ib.ItemName.Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", "") == (ibud.ItemName.Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", ""))).Count != 0)
                        continue;

                    itemBudgets.Add(ibud);
                }


                //CODE TO GET GROUPS OF COST ELEMENT


                foreach (string yr in years)
                {
                    reqList_forquery = new List<RequestItems_Table>();
                    //foreach (var bu_item in buList)
                    //{

                    //    reqList_forquery.AddRange(reqList.FindAll(item => item.CostElement.Trim() == costelement_chosen_id).FindAll(ss => ss.BU.Contains(bu_item)).FindAll(item => item.Category.Trim() == Category_Chosen.ToString().Trim()));

                    //}
                    BaseYear_toCompare++;
                    //CC XC CHECK
                    //if(yr == "2020")
                    //{
                    //    reqList_forquery = reqList_forquery/*.FindAll(ss => ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("AS")).ID.ToString()) || ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("2WP")).ID.ToString()))*/;
                    //}
                    //else
                    //if ((int.Parse(yr) - 1) > 2020)  //for 2020 reqList_forquery has relevant data
                    //{
                    //    string is_CCXC = string.Empty;
                    //    bool CCXCflag = false;
                    //    string presentUserDept = string.Empty;

                    //    if (selected_ccxc.ToUpper().Contains("CC"))
                    //    {
                    //        is_CCXC = "CC";
                    //        CCXCflag = true;
                    //    }
                    //    else if (selected_ccxc.ToUpper().Contains("XC"))
                    //    {
                    //        is_CCXC = "XC";
                    //        CCXCflag = true;
                    //    }

                    //    else                    //DATA FILTER BASED ON USER'S NTID
                    //    {
                    //        presentUserDept = lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).Department;

                    //        if (presentUserDept.ToUpper().Contains("CC"))
                    //        {
                    //            is_CCXC = "CC";
                    //            CCXCflag = true;
                    //        }
                    //        else if (presentUserDept.ToUpper().Contains("XC"))
                    //        {
                    //            is_CCXC = "XC";
                    //            CCXCflag = true;
                    //        }

                    //    }

                    //    if (CCXCflag)
                    //    {
                    //        if (is_CCXC.Contains("XC"))
                    //        {
                    //            reqList_forquery.AddRange(reqList.FindAll(ss => ss.BU.Contains(lstBUs.Find(bu => bu.BU.Contains("2WP")).ID.ToString())));
                    //            reqList_forquery = reqList_forquery.FindAll(dpt => lstDEPTs.Find(x => x.ID.ToString().Contains(dpt.DEPT)).DEPT.Contains(is_CCXC))/*.FindAll(ss => ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("AS")).ID.ToString()) || ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("2WP")).ID.ToString()))*/;

                    //        }
                    //        else
                    //        {
                    //            reqList_forquery = reqList_forquery.FindAll(dpt => !lstDEPTs.Find(x => x.ID.ToString().Contains(dpt.DEPT)).DEPT.Contains("XC"))/*.FindAll(ss => ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("AS")).ID.ToString()) || ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("2WP")).ID.ToString()))*/;
                    if (bu_string.Trim() != "")
                    {


                        DataTable dt1 = new DataTable();
                        connection();
                        string Query = "Exec [dbo].[Cockpit_GetTotals_with2023] '" + bu_string + "', '" + int.Parse(yr.Trim()) + "','" + CostElement_Chosen.Trim() + "','" + Category_Chosen.Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", "") + "','" + "' ";

                        //"Select * from RequestItems_Table where BU in (1, 3, 5) and (Fund is null or Fund = 2) and ApprovedDH = 1 and DEPT in (Select id from DEPT_Table where DEPT not like '%-XC%')";
                        OpenConnection();
                        SqlCommand cmd = new SqlCommand(Query, conn);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt1);
                        CloseConnection();
                        reqList_forquery = new List<RequestItems_Table>();
                        foreach (DataRow item in dt1.Rows)
                        {
                            try
                            {

                                RequestItems_Table ritem = new RequestItems_Table();

                                ritem.Projected_Amount = item["Projected_Amount"].ToString() != "" ? Convert.ToDecimal(item["Projected_Amount"]) : 0;
                                ritem.Unused_Amount = item["Unused_Amount"].ToString() != "" ? Convert.ToDecimal(item["Unused_Amount"]) : 0;


                                ritem.ApprQuantity = Convert.ToInt32(item["ApprQuantity"]);
                                ritem.ApprCost = Convert.ToDecimal(item["ApprCost"]);

                                ritem.VKM_Year = Convert.ToInt32(item["VKM_Year"]);
                                if (item["UpdatedAt"].ToString() != "")
                                    ritem.UpdatedAt = Convert.ToDateTime(item["UpdatedAt"]);
                                if (item["RequestorNTID"].ToString() != "")
                                    ritem.RequestorNTID = item["RequestorNTID"].ToString();
                                ritem.Category = item["Category"].ToString();
                                ritem.Comments = item["Comments"].ToString();
                                ritem.Project = item["Project"].ToString();
                                if (item["ActualAvailableQuantity"].ToString() == "")
                                    ritem.ActualAvailableQuantity = "NA";
                                else
                                    ritem.ActualAvailableQuantity = item["ActualAvailableQuantity"].ToString();

                                ritem.CostElement = item["CostElement"].ToString();
                                ritem.BU = item["BU"].ToString();

                                ritem.DEPT = item["DEPT"].ToString();
                                ritem.Group = item["Group"].ToString();
                                ritem.ItemName = item["ItemName"].ToString();
                                ritem.OEM = item["OEM"].ToString();
                                ritem.ReqQuantity = Convert.ToInt32(item["ReqQuantity"]);
                                ritem.RequestID = Convert.ToInt32(item["RequestID"]);

                                ritem.RequestorNT = item["RequestorNT"].ToString();
                                ritem.TotalPrice = Convert.ToDecimal(item["TotalPrice"]);
                                ritem.OrderPrice = item["OrderPrice"].ToString() != "" ? Convert.ToDecimal(item["OrderPrice"]) : 0;
                                ritem.UnitPrice = Convert.ToDecimal(item["UnitPrice"]);
                                ritem.ApprovalDH = Convert.ToBoolean(item["ApprovalDH"]);
                                ritem.ApprovalSH = Convert.ToBoolean(item["ApprovalSH"]);
                                ritem.ApprovedDH = Convert.ToBoolean(item["ApprovedDH"]);
                                ritem.ApprovedSH = Convert.ToBoolean(item["ApprovedSH"]);
                                if (item["IsCancelled"].ToString() != "")
                                    ritem.isCancelled = Convert.ToInt32(item["IsCancelled"]);

                                ritem.DHNT = item["DHNT"].ToString();
                                ritem.SHNT = item["SHNT"].ToString();

                                if (item["RequestDate"].ToString() != "")
                                    ritem.RequestDate = (DateTime)item["RequestDate"];

                                if (item["SubmitDate"].ToString() != "")
                                    ritem.SubmitDate = (DateTime)item["SubmitDate"];

                                if (item["DHAppDate"].ToString() != "")
                                    ritem.DHAppDate = (DateTime)item["DHAppDate"];

                                if (item["SHAppDate"].ToString() != "")
                                    ritem.SHAppDate = (DateTime)item["SHAppDate"];


                                if (item["OrderStatus"].ToString().Trim() != "")
                                {
                                    ritem.OrderStatus = (item["OrderStatus"]).ToString();

                                }
                                else
                                {
                                    ritem.OrderStatus = null;


                                }
                                if (item["Project"].ToString() == "")
                                    ritem.Project = string.Empty;
                                else
                                    ritem.Project = item["Project"].ToString();


                                reqList_forquery.Add(ritem);

                            }
                            catch (Exception ex)
                            {

                            }

                            //}

                        }
                        //        CCXCflag = false;
                        //    }



                        //}
                        //else
                        //{
                        //    if (buList.Contains("2") && buList.Contains("4") /*&& selected_ccxc.ToUpper().Contains("Custom") || selected_ccxc.ToUpper().Contains("XC"))*/ && yr == "2021" && reqList.FindAll(c => c.BU == "1").Count == 0) //XC VKM2021 - include 2WP Whole Totals also
                        //    {
                        //        reqList.AddRange(db.RequestItems_Table.ToList().FindAll(ss => ss.BU.Contains(lstBUs.Find(bu => bu.BU.Contains("2WP")).ID.ToString())).FindAll(v => v.Fund == null || v.Fund.Trim().Contains("2")));
                        //    }



                        //}

                        // decimal Category_Totals[10] = new decimal[10];
                        decimal[] P_Item_Totals = new decimal[itemBudgets.Count()];
                        decimal[] U_Item_Totals = new decimal[itemBudgets.Count()];
                        decimal[] Proj_Item_Totals = new decimal[itemBudgets.Count()];
                        decimal[] Unused_Item_Totals = new decimal[itemBudgets.Count()];
                        //decimal[] Cancelled_Item_Totals = new decimal[itemBudgets.Count()];

                        category_drilldownSummary tempobj = new category_drilldownSummary();
                        tempobj.vkmyear = yr;


                        foreach (var itembudget in itemBudgets)
                        {
                            foreach (RequestItems_Table item in reqList_forquery)
                            {
                                if (item.VKM_Year.ToString().Contains((int.Parse(yr).ToString())) && item.ApprovalSH == true)   // 2020 dh apprvd - 2021 planning
                                {
                                    //if ((int.Parse(yr) - 1).ToString() == "2020")
                                    if (BaseYear_toCompare == 1 || (BaseYear_toCompare == 2 && Top_ItemBudget.FindAll(i => i.ItemTotals == 0).Count == Top_ItemBudget.Count))
                                    {

                                        if (lstItems.Find(x => x.S_No == int.Parse(item.ItemName)).Item_Name.Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", "") == itembudget.ItemName.Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", ""))
                                        // if (int.Parse(item.ItemName) == itembudget.ItemID)
                                        {
                                            if (item.ApprCost != null)
                                                itembudget.ItemTotals += Math.Round((decimal)item.ApprCost, MidpointRounding.AwayFromZero);

                                        }

                                    }

                                }

                            }
                        }
                        Top_ItemBudget = itemBudgets.OrderByDescending(x => x.ItemTotals).Take(200).ToList();


                        int itemcount = -1;
                        foreach (var topitem in Top_ItemBudget)
                        {
                            itemcount++;
                            foreach (RequestItems_Table item in reqList_forquery)
                            {
                                if (item.VKM_Year.ToString().Contains((int.Parse(yr).ToString())) && item.ApprovalSH == true)
                                {
                                    if (lstItems.Find(x => x.S_No == int.Parse(item.ItemName)).Item_Name.Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", "") == topitem.ItemName.Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", ""))
                                    //if (int.Parse(item.ItemName) == topitem.ItemID)
                                    {
                                        P_Item_Totals[itemcount] += Math.Round((decimal)item.ApprCost, MidpointRounding.AwayFromZero);
                                        U_Item_Totals[itemcount] += item.OrderPrice == null ? (decimal)0.0 : Math.Round((decimal)item.OrderPrice, MidpointRounding.AwayFromZero);
                                        Proj_Item_Totals[itemcount] += item.Projected_Amount == null ? (decimal)0.0 : Math.Round((decimal)item.Projected_Amount, MidpointRounding.AwayFromZero);
                                        Unused_Item_Totals[itemcount] += item.Unused_Amount == null ? (decimal)0.0 : Math.Round((decimal)item.Unused_Amount, MidpointRounding.AwayFromZero);


                                        //Cancelled_Item_Totals[itemcount] += item.OrderStatus != null ? (item.OrderStatus.Trim() == lstOrderStatus.Find(status => status.OrderStatus.Trim().Equals("Cancelled")).ID.ToString() ? Math.Round((decimal)item.ApprCost, MidpointRounding.AwayFromZero) : (decimal)0.0) : (decimal)0.0;

                                    }


                                }
                            }
                        }


                        tempobj.P_Item_Totals = P_Item_Totals;
                        tempobj.U_Item_Totals = U_Item_Totals;

                        tempobj.Proj_Item_Totals = Proj_Item_Totals;
                        tempobj.Unused_Item_Totals = Unused_Item_Totals;
                        // tempobj.Cancelled_Item_Totals = Cancelled_Item_Totals;
                        viewList.Add(tempobj);

                    }
                }


                var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                BudgetParam t = new BudgetParam();
                List<columnsinfo> _col = new List<columnsinfo>();
                System.Data.DataTable dt = new System.Data.DataTable();
                dt.Columns.Add("Item Name", typeof(string)); /// Planned + (year variable) / Utilized  + (year variable)
                //dt.Columns.Add("VKM 2020", typeof(string));
                foreach (string year in years)
                {
                    dt.Columns.Add(" " + year + " Plan", typeof(string)); //add vkm text to yr
                    dt.Columns.Add(" " + year + " Used", typeof(string));
                    //if (year == years[0])
                    dt.Columns.Add(" " + year + " Projected", typeof(string));
                    dt.Columns.Add(" " + year + " Unused", typeof(string));
                }


                //DataRow dr = dt.NewRow();
                //dr[0] = "MAE";
                //int rcnt = 1;
                //int rcnt1 = 2; //2
                for (var i = 0; i < Top_ItemBudget.Count(); i++)
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = Top_ItemBudget[i].ItemName.Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", "");
                    int rcnt = 1;
                    int rcnt1 = 2;
                    int rcnt2 = 3;
                    int rcnt3 = 4;
                    foreach (var info in viewList)
                    {



                        dr[rcnt] = info.P_Item_Totals[i].ToString("C", CultureInfo.CurrentCulture);
                        dr[rcnt1] = info.U_Item_Totals[i].ToString("C", CultureInfo.CurrentCulture);
                        dr[rcnt2] = info.Proj_Item_Totals[i].ToString("C", CultureInfo.CurrentCulture);
                        dr[rcnt3] = info.Unused_Item_Totals[i].ToString("C", CultureInfo.CurrentCulture);

                        //if(rcnt2 == 6)
                        //{

                        //}
                        //else
                        //   dr[rcnt2] = (info.P_Item_Totals[i] - info.U_Item_Totals[i] - info.Cancelled_Item_Totals[i]).ToString("C", CultureInfo.CurrentCulture);
                        //rcnt++;
                        rcnt = rcnt + 4;
                        rcnt1 = rcnt1 + 4;
                        rcnt2 = rcnt2 + 4;
                        rcnt3 = rcnt3 + 4;
                    }
                    dt.Rows.Add(dr);

                }
                //dt.Rows.Add(dr);
                // dt.Rows.Add(dr);

                //dr = dt.NewRow(); 
                // dr[0] = "Non-MAE";


                // dt.Rows.Add(dr);
                for (int i = 0; i <= dt.Columns.Count - 1; i++)
                {
                    _col.Add(new columnsinfo { title = dt.Columns[i].ColumnName, data = dt.Columns[i].ColumnName });
                }

                string col = (string)serializer.Serialize(_col);
                t.columns = col;


                var lst = dt.AsEnumerable()
                .Select(r => r.Table.Columns.Cast<DataColumn>()
                        .Select(c => new KeyValuePair<string, object>(c.ColumnName, r[c.Ordinal])
                       ).ToDictionary(z => z.Key, z => z.Value)
                ).ToList();

                string data = serializer.Serialize(lst);
                t.data = data;




                return Json(new { data = t, message = selected_ccxc }, JsonRequestBehavior.AllowGet);

            }
        }

        //public ActionResult CostElementDrillDown_comparison(List<string> years, string CostElement_Chosen, List<string> buList, bool chart = false, string selected_ccxc = "")
        //{

        //    if (lstUsers == null)
        //    {
        //        return RedirectToAction("Index", "Budgeting");
        //    }


        //    using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //    //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //    {
        //        List<RequestItems_Table> reqList = new List<RequestItems_Table>();
        //        List<RequestItems_Table> reqList_forquery = new List<RequestItems_Table>();
        //        List<CategoryBudget> categoryBudgets = new List<CategoryBudget>();
        //        List<CategoryBudget> Top70_CategoryBudget = new List<CategoryBudget>();
        //        List<costelement_drilldownSummary> viewList = new List<costelement_drilldownSummary>();
        //        string costelement_chosen_id = string.Empty;
        //        reqList = db.RequestItems_Table.ToList<RequestItems_Table>().FindAll(v => v.Fund == null || v.Fund.Trim().Contains("2"));
        //        int BaseYear_toCompare = 0;
        //        //List<RequestItemsList_2019> reqList2019 = new List<RequestItemsList_2019>();

        //        //Fetching the Category List
        //        foreach (var category in lstPrdCateg)
        //        {
        //            CategoryBudget catbud = new CategoryBudget();
        //            catbud.CategoryID = category.ID;
        //            catbud.CategoryName = category.Category;
        //            categoryBudgets.Add(catbud);
        //        }


        //        //CostElement_Chosen is costelt name -> fetch its id
        //        costelement_chosen_id = lstCostElement.Find(cost => cost.CostElement.ToUpper().Trim() == CostElement_Chosen.ToUpper().Trim()).ID.ToString().Trim();

        //        foreach (var bu_item in buList)
        //        {
        //            reqList_forquery.AddRange(reqList.FindAll(item => item.CostElement.Trim() == costelement_chosen_id).FindAll(ss => ss.BU.Contains(bu_item)));
        //        }

        //        //CODE TO GET GROUPS OF CATEGORY


        //        foreach (string yr in years)
        //        {
        //            BaseYear_toCompare++;
        //            //CC XC CHECK

        //            if ((int.Parse(yr) - 1) > 2020)  //for 2020 reqList_forquery has relevant data
        //            {
        //                string is_CCXC = string.Empty;
        //                bool CCXCflag = false;
        //                string presentUserDept = string.Empty;

        //                if (selected_ccxc.ToUpper().Contains("CC"))
        //                {
        //                    is_CCXC = "CC";
        //                    CCXCflag = true;
        //                }
        //                else if (selected_ccxc.ToUpper().Contains("XC"))
        //                {
        //                    is_CCXC = "XC";
        //                    CCXCflag = true;
        //                }


        //                else                    //DATA FILTER BASED ON USER'S NTID
        //                {
        //                    presentUserDept = lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).Department;

        //                    if (presentUserDept.ToUpper().Contains("CC"))
        //                    {
        //                        is_CCXC = "CC";
        //                        CCXCflag = true;
        //                    }
        //                    else if (presentUserDept.ToUpper().Contains("XC"))
        //                    {
        //                        is_CCXC = "XC";
        //                        CCXCflag = true;
        //                    }

        //                }

        //                if (CCXCflag)
        //                {
        //                    if (is_CCXC.Contains("XC"))
        //                    {
        //                        reqList_forquery.AddRange(reqList.FindAll(ss => ss.BU.Contains(lstBUs.Find(bu => bu.BU.Contains("2WP")).ID.ToString())));
        //                    }
        //                    reqList_forquery = reqList_forquery.FindAll(dpt => lstDEPTs.Find(x => x.ID.ToString().Contains(dpt.DEPT)).DEPT.Contains(is_CCXC))/*.FindAll(ss => ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("AS")).ID.ToString()) || ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("2WP")).ID.ToString()))*/;
        //                    CCXCflag = false;
        //                }



        //            }


        //            decimal[] P_Category_Totals = new decimal[categoryBudgets.Count()];
        //            decimal[] U_Category_Totals = new decimal[categoryBudgets.Count()];
        //            costelement_drilldownSummary tempobj = new costelement_drilldownSummary();
        //            tempobj.vkmyear = yr;


        //            //CODE TO GET THE TOTALS OF Top Category in 2020 Year - to compare with the other years Totals
        //            if (lstPrdCateg.Count != 0)
        //            {

        //                foreach (var catbudget in categoryBudgets)
        //                {
        //                    foreach (RequestItems_Table item in reqList_forquery)
        //                    {
        //                        if (item.DHAppDate.ToString().Contains((int.Parse(yr) - 1).ToString()))   // 2020 sh apprvd - 2021 planning
        //                        {
        //                            // if ((int.Parse(yr) - 1).ToString() == "2020")
        //                            if (BaseYear_toCompare == 1 | (BaseYear_toCompare == 2 && Top70_CategoryBudget.Count() == 0))
        //                            {

        //                                if (int.Parse(item.Category) == catbudget.CategoryID)
        //                                {


        //                                    if (item.ApprCost != null)
        //                                    {

        //                                        catbudget.CategoryTotals += (decimal)item.ApprCost;
        //                                    }


        //                                }

        //                            }



        //                        }

        //                    }
        //                }
        //                Top70_CategoryBudget = categoryBudgets.OrderByDescending(x => x.CategoryTotals).Take(70).ToList();

        //            }

        //            //Code to get the Planned and Utilized Category Totals
        //            int catcount = -1;
        //            foreach (var topcat in Top70_CategoryBudget)
        //            {
        //                catcount++;
        //                foreach (RequestItems_Table item in reqList_forquery)
        //                {
        //                    if (item.DHAppDate.ToString().Contains((int.Parse(yr) - 1).ToString()))
        //                    {
        //                        if (int.Parse(item.Category) == topcat.CategoryID)
        //                        {
        //                            P_Category_Totals[catcount] += (decimal)item.ApprCost;
        //                            U_Category_Totals[catcount] += item.OrderPrice == null ? (decimal)0.0 : (decimal)item.OrderPrice;

        //                        }
        //                    }
        //                }
        //            }

        //            tempobj.P_Category_Totals = P_Category_Totals;
        //            tempobj.U_Category_Totals = U_Category_Totals;

        //            viewList.Add(tempobj);

        //        }




        //        ///NEW CODE TO GET CATEGORY SUMMARY
        //        var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
        //        BudgetParam t = new BudgetParam();
        //        List<columnsinfo> _col = new List<columnsinfo>();
        //        System.Data.DataTable dt = new System.Data.DataTable();
        //        dt.Columns.Add("Category Name", typeof(string)); /// Planned + (year variable) / Utilized  + (year variable)
        //        //dt.Columns.Add("VKM 2020", typeof(string));
        //        foreach (string year in years)
        //        {
        //            dt.Columns.Add("VKM" + " " + year + "(Planned)", typeof(string)); //add vkm text to yr
        //            dt.Columns.Add("VKM" + " " + year + "(Utilized)", typeof(string));

        //        }


        //        //DataRow dr = dt.NewRow();
        //        //dr[0] = "MAE";
        //        //int rcnt = 1;
        //        //int rcnt1 = 2; //2
        //        for (var i = 0; i < Top70_CategoryBudget.Count(); i++)
        //        {
        //            DataRow dr = dt.NewRow();
        //            dr[0] = Top70_CategoryBudget[i].CategoryName.Trim().Replace("\r\n", "").Replace("\r", "").Replace("\r", "");
        //            int rcnt = 1;
        //            int rcnt1 = 2;
        //            foreach (var info in viewList)
        //            {



        //                dr[rcnt] = info.P_Category_Totals[i].ToString("C", CultureInfo.CurrentCulture);
        //                dr[rcnt1] = info.U_Category_Totals[i];
        //                //rcnt++;
        //                rcnt = rcnt + 2;
        //                rcnt1 = rcnt1 + 2;

        //            }
        //            dt.Rows.Add(dr);

        //        }
        //        //dt.Rows.Add(dr);
        //        // dt.Rows.Add(dr);

        //        //dr = dt.NewRow();
        //        // dr[0] = "Non-MAE";


        //        // dt.Rows.Add(dr);
        //        for (int i = 0; i <= dt.Columns.Count - 1; i++)
        //        {
        //            _col.Add(new columnsinfo { title = dt.Columns[i].ColumnName, data = dt.Columns[i].ColumnName });
        //        }

        //        string col = (string)serializer.Serialize(_col);
        //        t.columns = col;


        //        var lst = dt.AsEnumerable()
        //        .Select(r => r.Table.Columns.Cast<DataColumn>()
        //                .Select(c => new KeyValuePair<string, object>(c.ColumnName, r[c.Ordinal])
        //               ).ToDictionary(z => z.Key, z => z.Value)
        //        ).ToList();

        //        string data = serializer.Serialize(lst);
        //        t.data = data;




        //        return Json(new { data = t, message = selected_ccxc }, JsonRequestBehavior.AllowGet);

        //        // return View();

        //    }
        //}





        ///// <summary>
        ///// Function to send BU summary data to view
        ///// </summary>
        ///// <returns></returns> 
        //public ActionResult CostElementDrillDown(string Year_isPO, string CostElement_Chosen, List<string> buList, string selected_ccxc = "")
        //{


        //    using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //    //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //    {
        //        List<CategoryBudget> categoryBudgets = new List<CategoryBudget>();
        //        List<RequestItems_Table> reqList = new List<RequestItems_Table>();


        //        foreach (var category in lstPrdCateg)
        //        {
        //            CategoryBudget catbud = new CategoryBudget();
        //            catbud.CategoryID = category.ID;
        //            catbud.CategoryName = category.Category;
        //            categoryBudgets.Add(catbud);
        //        }
        //        //categoryBudgets will comprise of all the Categories - ID and Name
        //        //Year_isPO - 
        //        string YearX = Year_isPO.Split('(')[0];
        //        int Year = int.Parse(YearX.Substring(4));
        //        var is_PO = Year_isPO.Split('(')[1];
        //        decimal OverallTotals = 0;
        //        string costelement_chosen_id = string.Empty;
        //        List<CategoryBudget> Whole_CategoryBudget = new List<CategoryBudget>();
        //        List<CategoryBudget> Top10_CategoryBudget = new List<CategoryBudget>();


        //        //if CostElement_Chosen is costelt name -> fetch its id
        //        costelement_chosen_id = lstCostElement.Find(cost => cost.CostElement.ToUpper().Trim() == CostElement_Chosen.ToUpper().Trim()).ID.ToString().Trim();
        //        //else costelement_chosen_id = CostElement_Chosen

        //        foreach (var bu_item in buList)
        //        {
        //            //reqList_forquery ;
        //            reqList.AddRange(db.RequestItems_Table.Where(item => item.CostElement.Trim() == costelement_chosen_id).ToList().FindAll(ss => ss.BU.Contains(bu_item)/*(lstBUs.Find(bu => bu.BU.Trim().Equals(bu_item.Trim())).ID.ToString())*/));
        //        }


        //            if (Year != 2020 && !(Year == 2021 && is_PO.Contains("Planned")))  //for 2020 reqList_forquery has relevant data 2021planned - no check 2021utili - check 2022plan check
        //            {
        //                string is_CCXC = string.Empty;
        //                bool CCXCflag = false;
        //                string presentUserDept = string.Empty;
        //                if (selected_ccxc != "") //DATA FILTER BASED ON CCXC DROPDOWM - NOT USER'S NTID
        //                {
        //                    if (selected_ccxc.ToUpper().Contains("CC"))
        //                    {
        //                        is_CCXC = "CC";
        //                        CCXCflag = true;
        //                    }
        //                    else if (selected_ccxc.ToUpper().Contains("XC"))
        //                    {
        //                        is_CCXC = "XC";
        //                        CCXCflag = true;
        //                    }

        //                }
        //                else                    //DATA FILTER BASED ON USER'S NTID
        //                {
        //                    presentUserDept = lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).Department;

        //                    if (presentUserDept.ToUpper().Contains("CC"))
        //                    {
        //                        is_CCXC = "CC";
        //                        CCXCflag = true;
        //                    }
        //                    else if (presentUserDept.ToUpper().Contains("XC"))
        //                    {
        //                        is_CCXC = "XC";
        //                        CCXCflag = true;
        //                    }

        //                }

        //                if (CCXCflag)
        //                {
        //                    if (is_CCXC.Contains("XC"))
        //                    {
        //                        reqList.AddRange(reqList.FindAll(ss => ss.BU.Contains(lstBUs.Find(bu => bu.BU.Contains("2WP")).ID.ToString())));
        //                    }
        //                    reqList = reqList.FindAll(dpt => lstDEPTs.Find(x => x.ID.ToString().Contains(dpt.DEPT)).DEPT.Contains(is_CCXC))/*.FindAll(ss => ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("AS")).ID.ToString()) || ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("2WP")).ID.ToString()))*/;
        //                    CCXCflag = false;
        //                }



        //            }
        //        if (is_PO.Contains("Planned"))
        //        {
        //            //use dhappdate == Year-1 check (since apprvd cost includes dh & sh)
        //            //get all reqlist items with chosen cost elt & DHApprvDate = Year-1
        //            reqList = reqList.Where(item=>item.ApprovalDH == true && item.DHAppDate != null).Where(item => item.DHAppDate.Value.Year == (Year-1)).OrderBy(item => item.Category).ToList()/*.FindAll(ss => ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("AS")).ID.ToString()) || ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("2WP")).ID.ToString()))*/;
        //            foreach (var catbudget in categoryBudgets)
        //            {
        //                foreach (var item in reqList)
        //                {
        //                    if (int.Parse(item.Category) == catbudget.CategoryID)
        //                    {
        //                        if (item.ApprCost != null)
        //                            catbudget.CategoryTotals += (decimal)item.ApprCost;
        //                        OverallTotals += (decimal)item.ApprCost;
        //                    }
        //                }
        //            }

        //            Whole_CategoryBudget = categoryBudgets.OrderByDescending(x => x.CategoryTotals).ToList();
        //            Top10_CategoryBudget = categoryBudgets.Take(10).ToList(); //check these categories in utilized

        //        }
        //        if (is_PO.Contains("Utilized"))
        //        {
        //            //use orderdate == Year check
        //            //get all reqlist items with chosen cost elt & OrderDate = Year
        //            reqList = reqList.Where(x=>x.OrderDate != null).Where(item => item.OrderDate.Value.Year == Year).OrderBy(item => item.Category).ToList();
        //            foreach (var catbud in categoryBudgets)
        //            {
        //                foreach (var item in reqList)
        //                {
        //                    if (int.Parse(item.Category) == catbud.CategoryID)
        //                    {
        //                        if (item.OrderPrice != null)
        //                            catbud.CategoryTotals += (decimal)item.OrderPrice;
        //                    }
        //                }
        //            }

        //            Whole_CategoryBudget = categoryBudgets.OrderByDescending(x => x.CategoryTotals).ToList();
        //            Top10_CategoryBudget = categoryBudgets.Take(10).ToList();


        //        }




        //        return Json(new { data = Whole_CategoryBudget, success = true, message = selected_ccxc }, JsonRequestBehavior.AllowGet);


        //    }
        //}


        //public ActionResult CategoryDrillDown_comparison(List<string> years, string CostElement_Chosen, string Category_Chosen, List<string> buList, bool chart = false, string selected_ccxc = "")
        //{

        //    if (lstUsers == null)
        //    {
        //        return RedirectToAction("Index", "Budgeting");
        //    }


        //    using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //    //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //    {
        //        List<RequestItems_Table> reqList = new List<RequestItems_Table>();
        //        List<RequestItems_Table> reqList_forquery = new List<RequestItems_Table>();
        //        reqList = db.RequestItems_Table.ToList<RequestItems_Table>().FindAll(v => v.Fund == null || v.Fund.Trim().Contains("2"));
        //        List<RequestItemsList_2019> reqList2019 = new List<RequestItemsList_2019>();
        //        List<ItemBudget> itemBudgets = new List<ItemBudget>();
        //        List<ItemBudget> Top_ItemBudget = new List<ItemBudget>();
        //        List<category_drilldownSummary> viewList = new List<category_drilldownSummary>();
        //        string costelement_chosen_id = string.Empty;
        //        int BaseYear_toCompare = 0;

        //        //CostElement_Chosen is costelt name -> fetch its id
        //        costelement_chosen_id = lstCostElement.Find(cost => cost.CostElement.ToUpper().Trim() == CostElement_Chosen.ToUpper().Trim()).ID.ToString().Trim();

        //        //Category_Chosen is costelt name -> fetch its id
        //        Category_Chosen = lstPrdCateg.Find(cat => cat.Category.ToUpper().Trim() == Category_Chosen.ToUpper().Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", "")).ID.ToString().Trim();

        //        foreach (var item in lstItems.Where(x => x.Category.Contains(Category_Chosen.ToString().Trim())))
        //        {
        //            ItemBudget ibud = new ItemBudget();
        //            ibud.ItemID = item.S_No;
        //            ibud.ItemName = item.Item_Name;


        //            if (itemBudgets.Count > 0 && itemBudgets.FindAll(ib => ib.ItemName.Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", "") == (ibud.ItemName.Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", ""))).Count != 0)
        //                continue;

        //            itemBudgets.Add(ibud);
        //        }
        //        foreach (var bu_item in buList)
        //        {

        //            reqList_forquery.AddRange(reqList.FindAll(item => item.CostElement.Trim() == costelement_chosen_id).FindAll(ss => ss.BU.Contains(bu_item)).FindAll(item => item.Category.Trim() == Category_Chosen.ToString().Trim()));

        //        }

        //        //CODE TO GET GROUPS OF COST ELEMENT


        //        foreach (string yr in years)
        //        {
        //            BaseYear_toCompare++;
        //            //CC XC CHECK
        //            //if(yr == "2020")
        //            //{
        //            //    reqList_forquery = reqList_forquery/*.FindAll(ss => ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("AS")).ID.ToString()) || ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("2WP")).ID.ToString()))*/;
        //            //}
        //            //else
        //            if ((int.Parse(yr) - 1) > 2020)  //for 2020 reqList_forquery has relevant data
        //            {
        //                string is_CCXC = string.Empty;
        //                bool CCXCflag = false;
        //                string presentUserDept = string.Empty;

        //                if (selected_ccxc.ToUpper().Contains("CC"))
        //                {
        //                    is_CCXC = "CC";
        //                    CCXCflag = true;
        //                }
        //                else if (selected_ccxc.ToUpper().Contains("XC"))
        //                {
        //                    is_CCXC = "XC";
        //                    CCXCflag = true;
        //                }

        //                else                    //DATA FILTER BASED ON USER'S NTID
        //                {
        //                    presentUserDept = lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).Department;

        //                    if (presentUserDept.ToUpper().Contains("CC"))
        //                    {
        //                        is_CCXC = "CC";
        //                        CCXCflag = true;
        //                    }
        //                    else if (presentUserDept.ToUpper().Contains("XC"))
        //                    {
        //                        is_CCXC = "XC";
        //                        CCXCflag = true;
        //                    }

        //                }

        //                if (CCXCflag)
        //                {
        //                    if (is_CCXC.Contains("XC"))
        //                    {
        //                        reqList_forquery.AddRange(reqList.FindAll(ss => ss.BU.Contains(lstBUs.Find(bu => bu.BU.Contains("2WP")).ID.ToString())));
        //                    }
        //                    reqList_forquery = reqList_forquery.FindAll(dpt => lstDEPTs.Find(x => x.ID.ToString().Contains(dpt.DEPT)).DEPT.Contains(is_CCXC))/*.FindAll(ss => ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("AS")).ID.ToString()) || ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("2WP")).ID.ToString()))*/;
        //                    CCXCflag = false;
        //                }



        //            }


        //            // decimal Category_Totals[10] = new decimal[10];
        //            decimal[] P_Item_Totals = new decimal[200];
        //            decimal[] U_Item_Totals = new decimal[200];
        //            category_drilldownSummary tempobj = new category_drilldownSummary();
        //            tempobj.vkmyear = yr;


        //            foreach (var itembudget in itemBudgets)
        //            {
        //                foreach (RequestItems_Table item in reqList_forquery)
        //                {
        //                    if (item.DHAppDate.ToString().Contains((int.Parse(yr) - 1).ToString()))   // 2020 dh apprvd - 2021 planning
        //                    {
        //                        //if ((int.Parse(yr) - 1).ToString() == "2020")
        //                        if (BaseYear_toCompare == 1 || (BaseYear_toCompare == 2 && Top_ItemBudget.Count() == 0))
        //                        {

        //                            if (lstItems.Find(x => x.S_No == int.Parse(item.ItemName)).Item_Name.Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", "") == itembudget.ItemName.Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", ""))
        //                            // if (int.Parse(item.ItemName) == itembudget.ItemID)
        //                            {
        //                                if (item.ApprCost != null)
        //                                    itembudget.ItemTotals += (decimal)item.ApprCost;

        //                            }

        //                        }

        //                    }

        //                }
        //            }
        //            Top_ItemBudget = itemBudgets.OrderByDescending(x => x.ItemTotals).Take(200).ToList();


        //            int itemcount = -1;
        //            foreach (var topitem in Top_ItemBudget)
        //            {
        //                itemcount++;
        //                foreach (RequestItems_Table item in reqList_forquery)
        //                {
        //                    if (item.DHAppDate.ToString().Contains((int.Parse(yr) - 1).ToString()))
        //                    {
        //                        if (lstItems.Find(x => x.S_No == int.Parse(item.ItemName)).Item_Name.Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", "") == topitem.ItemName.Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", ""))
        //                        //if (int.Parse(item.ItemName) == topitem.ItemID)
        //                        {
        //                            P_Item_Totals[itemcount] += (decimal)item.ApprCost;
        //                            U_Item_Totals[itemcount] += item.OrderPrice == null ? (decimal)0.0 : (decimal)item.OrderPrice;

        //                        }


        //                    }
        //                }
        //            }


        //            tempobj.P_Item_Totals = P_Item_Totals;
        //            tempobj.U_Item_Totals = U_Item_Totals;

        //            viewList.Add(tempobj);

        //        }


        //        var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
        //        BudgetParam t = new BudgetParam();
        //        List<columnsinfo> _col = new List<columnsinfo>();
        //        System.Data.DataTable dt = new System.Data.DataTable();
        //        dt.Columns.Add("Item Name", typeof(string)); /// Planned + (year variable) / Utilized  + (year variable)
        //        //dt.Columns.Add("VKM 2020", typeof(string));
        //        foreach (string year in years)
        //        {
        //            dt.Columns.Add("VKM" + " " + year + "(Planned)", typeof(string)); //add vkm text to yr
        //            dt.Columns.Add("VKM" + " " + year + "(Utilized)", typeof(string));

        //        }


        //        //DataRow dr = dt.NewRow();
        //        //dr[0] = "MAE";
        //        //int rcnt = 1;
        //        //int rcnt1 = 2; //2
        //        for (var i = 0; i < Top_ItemBudget.Count(); i++)
        //        {
        //            DataRow dr = dt.NewRow();
        //            dr[0] = Top_ItemBudget[i].ItemName.Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", "");
        //            int rcnt = 1;
        //            int rcnt1 = 2;
        //            foreach (var info in viewList)
        //            {



        //                dr[rcnt] = info.P_Item_Totals[i].ToString("C", CultureInfo.CurrentCulture);
        //                dr[rcnt1] = info.U_Item_Totals[i];
        //                //rcnt++;
        //                rcnt = rcnt + 2;
        //                rcnt1 = rcnt1 + 2;

        //            }
        //            dt.Rows.Add(dr);

        //        }
        //        //dt.Rows.Add(dr);
        //        // dt.Rows.Add(dr);

        //        //dr = dt.NewRow(); 
        //        // dr[0] = "Non-MAE";


        //        // dt.Rows.Add(dr);
        //        for (int i = 0; i <= dt.Columns.Count - 1; i++)
        //        {
        //            _col.Add(new columnsinfo { title = dt.Columns[i].ColumnName, data = dt.Columns[i].ColumnName });
        //        }

        //        string col = (string)serializer.Serialize(_col);
        //        t.columns = col;


        //        var lst = dt.AsEnumerable()
        //        .Select(r => r.Table.Columns.Cast<DataColumn>()
        //                .Select(c => new KeyValuePair<string, object>(c.ColumnName, r[c.Ordinal])
        //               ).ToDictionary(z => z.Key, z => z.Value)
        //        ).ToList();

        //        string data = serializer.Serialize(lst);
        //        t.data = data;




        //        return Json(new { data = t, message = selected_ccxc }, JsonRequestBehavior.AllowGet);

        //    }
        //}




        ///// <summary>
        ///// Function to send BU summary data to view
        ///// </summary>
        ///// <returns></returns> 
        //public ActionResult CategoryDrillDown(string Year_isPO, string CostElement_Chosen, int Category_Chosen, List<string> buList, string selected_ccxc = "")
        //{


        //    using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //    //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //    {
        //        List<ItemBudget> itemBudgets = new List<ItemBudget>();
        //        List<RequestItems_Table> reqList = new List<RequestItems_Table>();


        //        foreach (var item in lstItems.Where(x => x.Category.Contains(Category_Chosen.ToString().Trim())))
        //        {
        //            ItemBudget ibud = new ItemBudget();
        //            ibud.ItemID = item.S_No;
        //            ibud.ItemName = item.Item_Name;
        //            itemBudgets.Add(ibud);
        //        }
        //        //categoryBudgets will comprise of all the Categories - ID and Name
        //        //Year_isPO - 
        //        string YearX = Year_isPO.Split('(')[0];
        //        int Year = int.Parse(YearX.Substring(4));
        //        var is_PO = Year_isPO.Split('(')[1];
        //        decimal OverallItemTotals = 0;
        //        string costelement_chosen_id = string.Empty;
        //        List<ItemBudget> Whole_ItemBudget = new List<ItemBudget>();
        //        List<ItemBudget> Top10_ItemBudget = new List<ItemBudget>();


        //        costelement_chosen_id = lstCostElement.Find(cost => cost.CostElement.ToUpper().Trim() == CostElement_Chosen.ToUpper().Trim()).ID.ToString().Trim();

        //        foreach (var bu_item in buList)
        //        {
        //            //reqList_forquery ;
        //            reqList.AddRange(db.RequestItems_Table.Where(item => item.CostElement.Trim() == costelement_chosen_id).ToList().FindAll(ss => ss.BU.Contains(bu_item)).Where(item => item.Category.Trim() == Category_Chosen.ToString().Trim()));
        //        }

        //        var zzz = Year != 2020;
        //        var aaa = !(Year == 2021 && is_PO.Contains("Planned"));
        //        var xxx = Year != 2020 && !(Year == 2021 && is_PO.Contains("Planned"));
        //        if (Year != 2020 && !(Year == 2021 && is_PO.Contains("Planned")))   //for 2020 reqList_forquery has relevant data
        //        {
        //            string is_CCXC = string.Empty;
        //            bool CCXCflag = false;
        //            string presentUserDept = string.Empty;
        //            if (selected_ccxc != "") //DATA FILTER BASED ON CCXC DROPDOWM - NOT USER'S NTID
        //            {
        //                if (selected_ccxc.ToUpper().Contains("CC"))
        //                {
        //                    is_CCXC = "CC";
        //                    CCXCflag = true;
        //                }
        //                else if (selected_ccxc.ToUpper().Contains("XC"))
        //                {
        //                    is_CCXC = "XC";
        //                    CCXCflag = true;
        //                }

        //            }
        //            else                    //DATA FILTER BASED ON USER'S NTID
        //            {
        //                presentUserDept = lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).Department;

        //                if (presentUserDept.ToUpper().Contains("CC"))
        //                {
        //                    is_CCXC = "CC";
        //                    CCXCflag = true;
        //                }
        //                else if (presentUserDept.ToUpper().Contains("XC"))
        //                {
        //                    is_CCXC = "XC";
        //                    CCXCflag = true;
        //                }

        //            }

        //            if (CCXCflag)
        //            {
        //                if (is_CCXC.Contains("XC"))
        //                {
        //                    reqList.AddRange(reqList.FindAll(ss => ss.BU.Contains(lstBUs.Find(bu => bu.BU.Contains("2WP")).ID.ToString())));
        //                }
        //                reqList = reqList.FindAll(dpt => lstDEPTs.Find(x => x.ID.ToString().Contains(dpt.DEPT)).DEPT.Contains(is_CCXC))/*.FindAll(ss => ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("AS")).ID.ToString()) || ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("2WP")).ID.ToString()))*/;
        //                CCXCflag = false;
        //            }



        //        }

        //        if (is_PO.Contains("Planned"))
        //        {
        //            //use dhappdate == Year-1 check (since apprvd cost includes dh & sh)
        //            //get all reqlist items with chosen cost elt & DHApprvDate = Year-1
        //            reqList = reqList.Where(x => x.DHAppDate != null).Where(item => item.DHAppDate.Value.Year == (Year-1)).OrderBy(item => item.ItemName).ToList()/*.FindAll(ss => ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("AS")).ID.ToString()) || ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("2WP")).ID.ToString()))*/;
        //            foreach (var itembudget in itemBudgets)
        //            {
        //                foreach (var item in reqList)
        //                {
        //                    if (int.Parse(item.ItemName) == itembudget.ItemID)
        //                    {
        //                        if (item.ApprCost != null)
        //                            itembudget.ItemTotals += (decimal)item.ApprCost;
        //                        OverallItemTotals += (decimal)item.ApprCost;
        //                    }
        //                }
        //            }

        //            Whole_ItemBudget = itemBudgets.OrderByDescending(x => x.ItemTotals).ToList();
        //            Top10_ItemBudget = itemBudgets.Take(10).ToList();

        //        }
        //        if (is_PO.Contains("Utilized"))
        //        {
        //            //use orderdate == Year check
        //            //get all reqlist items with chosen cost elt & OrderDate = Year
        //            reqList = reqList.Where(x => x.OrderDate != null).Where(item => item.OrderDate.Value.Year == Year).OrderBy(item => item.Category).ToList();
        //            foreach (var catbud in itemBudgets)
        //            {
        //                foreach (var item in reqList)
        //                {
        //                    if (int.Parse(item.ItemName) == catbud.ItemID)
        //                    {
        //                        if (item.OrderPrice != null)
        //                            catbud.ItemTotals += (decimal)item.OrderPrice;
        //                    }
        //                }
        //            }

        //            Whole_ItemBudget = itemBudgets.OrderByDescending(x => x.ItemTotals).ToList();
        //            Top10_ItemBudget = itemBudgets.Take(10).ToList();


        //        }




        //        return Json(new { data = Whole_ItemBudget, success = true, message = selected_ccxc }, JsonRequestBehavior.AllowGet);


        //    }
        //}





        //public ActionResult CategoryDrillDown_comparison(List<string> years, string CostElement_Chosen, string Category_Chosen, List<string> buList, bool chart = false, string selected_ccxc = "")
        //{

        //    if (lstUsers == null)
        //    {
        //        return RedirectToAction("Index", "Budgeting");
        //    }


        //    using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //    //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //    {
        //        List<RequestItems_Table> reqList = new List<RequestItems_Table>();
        //        List<RequestItems_Table> reqList_forquery = new List<RequestItems_Table>();
        //        reqList = db.RequestItems_Table.ToList<RequestItems_Table>().FindAll(v => v.Fund == null || v.Fund.Trim().Contains("2"));
        //        List<RequestItemsList_2019> reqList2019 = new List<RequestItemsList_2019>();
        //        List<ItemBudget> itemBudgets = new List<ItemBudget>();
        //        List<ItemBudget> Top_ItemBudget = new List<ItemBudget>();
        //        List<category_drilldownSummary> viewList = new List<category_drilldownSummary>();
        //        string costelement_chosen_id = string.Empty;
        //        int BaseYear_toCompare = 0;

        //        //CostElement_Chosen is costelt name -> fetch its id
        //        costelement_chosen_id = lstCostElement.Find(cost => cost.CostElement.ToUpper().Trim() == CostElement_Chosen.ToUpper().Trim()).ID.ToString().Trim();

        //        //Category_Chosen is costelt name -> fetch its id
        //        Category_Chosen = lstPrdCateg.Find(cat => cat.Category.ToUpper().Trim() == Category_Chosen.ToUpper().Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", "")).ID.ToString().Trim();

        //        foreach (var item in lstItems.Where(x => x.Category.Contains(Category_Chosen.ToString().Trim())))
        //        {
        //            ItemBudget ibud = new ItemBudget();
        //            ibud.ItemID = item.S_No;
        //            ibud.ItemName = item.Item_Name;


        //            if (itemBudgets.Count > 0 && itemBudgets.FindAll(ib => ib.ItemName.Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", "") == (ibud.ItemName.Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", ""))).Count != 0)
        //                continue;

        //            itemBudgets.Add(ibud);
        //        }
        //        foreach (var bu_item in buList)
        //        {

        //            reqList_forquery.AddRange(reqList.FindAll(item => item.CostElement.Trim() == costelement_chosen_id).FindAll(ss => ss.BU.Contains(bu_item)).FindAll(item => item.Category.Trim() == Category_Chosen.ToString().Trim()));

        //        }

        //        //CODE TO GET GROUPS OF COST ELEMENT


        //        foreach (string yr in years)
        //        {
        //            BaseYear_toCompare++;
        //            //CC XC CHECK
        //            //if(yr == "2020")
        //            //{
        //            //    reqList_forquery = reqList_forquery/*.FindAll(ss => ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("AS")).ID.ToString()) || ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("2WP")).ID.ToString()))*/;
        //            //}
        //            //else
        //            if ((int.Parse(yr) - 1) > 2020)  //for 2020 reqList_forquery has relevant data
        //            {
        //                string is_CCXC = string.Empty;
        //                bool CCXCflag = false;
        //                string presentUserDept = string.Empty;

        //                if (selected_ccxc.ToUpper().Contains("CC"))
        //                {
        //                    is_CCXC = "CC";
        //                    CCXCflag = true;
        //                }
        //                else if (selected_ccxc.ToUpper().Contains("XC"))
        //                {
        //                    is_CCXC = "XC";
        //                    CCXCflag = true;
        //                }

        //                else                    //DATA FILTER BASED ON USER'S NTID
        //                {
        //                    presentUserDept = lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).Department;

        //                    if (presentUserDept.ToUpper().Contains("CC"))
        //                    {
        //                        is_CCXC = "CC";
        //                        CCXCflag = true;
        //                    }
        //                    else if (presentUserDept.ToUpper().Contains("XC"))
        //                    {
        //                        is_CCXC = "XC";
        //                        CCXCflag = true;
        //                    }

        //                }

        //                if (CCXCflag)
        //                {
        //                    if (is_CCXC.Contains("XC"))
        //                    {
        //                        reqList_forquery.AddRange(reqList.FindAll(ss => ss.BU.Contains(lstBUs.Find(bu => bu.BU.Contains("2WP")).ID.ToString())));
        //                    }
        //                    reqList_forquery = reqList_forquery.FindAll(dpt => lstDEPTs.Find(x => x.ID.ToString().Contains(dpt.DEPT)).DEPT.Contains(is_CCXC))/*.FindAll(ss => ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("AS")).ID.ToString()) || ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("2WP")).ID.ToString()))*/;
        //                    CCXCflag = false;
        //                }



        //            }


        //            // decimal Category_Totals[10] = new decimal[10];
        //            decimal[] P_Item_Totals = new decimal[200];
        //            decimal[] U_Item_Totals = new decimal[200];
        //            category_drilldownSummary tempobj = new category_drilldownSummary();
        //            tempobj.vkmyear = yr;


        //            foreach (var itembudget in itemBudgets)
        //            {
        //                foreach (RequestItems_Table item in reqList_forquery)
        //                {
        //                    if (item.DHAppDate.ToString().Contains((int.Parse(yr) - 1).ToString()))   // 2020 dh apprvd - 2021 planning
        //                    {
        //                        //if ((int.Parse(yr) - 1).ToString() == "2020")
        //                        if (BaseYear_toCompare == 1 || (BaseYear_toCompare == 2 && Top_ItemBudget.Count() == 0))
        //                        {

        //                            if (lstItems.Find(x => x.S_No == int.Parse(item.ItemName)).Item_Name.Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", "") == itembudget.ItemName.Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", ""))
        //                            // if (int.Parse(item.ItemName) == itembudget.ItemID)
        //                            {
        //                                if (item.ApprCost != null)
        //                                    itembudget.ItemTotals += (decimal)item.ApprCost;

        //                            }

        //                        }

        //                    }

        //                }
        //            }
        //            Top_ItemBudget = itemBudgets.OrderByDescending(x => x.ItemTotals).Take(200).ToList();


        //            int itemcount = -1;
        //            foreach (var topitem in Top_ItemBudget)
        //            {
        //                itemcount++;
        //                foreach (RequestItems_Table item in reqList_forquery)
        //                {
        //                    if (item.DHAppDate.ToString().Contains((int.Parse(yr) - 1).ToString()))
        //                    {
        //                        if (lstItems.Find(x => x.S_No == int.Parse(item.ItemName)).Item_Name.Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", "") == topitem.ItemName.Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", ""))
        //                        //if (int.Parse(item.ItemName) == topitem.ItemID)
        //                        {
        //                            P_Item_Totals[itemcount] += (decimal)item.ApprCost;
        //                            U_Item_Totals[itemcount] += item.OrderPrice == null ? (decimal)0.0 : (decimal)item.OrderPrice;

        //                        }


        //                    }
        //                }
        //            }


        //            tempobj.P_Item_Totals = P_Item_Totals;
        //            tempobj.U_Item_Totals = U_Item_Totals;

        //            viewList.Add(tempobj);

        //        }


        //        var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
        //        BudgetParam t = new BudgetParam();
        //        List<columnsinfo> _col = new List<columnsinfo>();
        //        System.Data.DataTable dt = new System.Data.DataTable();
        //        dt.Columns.Add("Item Name", typeof(string)); /// Planned + (year variable) / Utilized  + (year variable)
        //        //dt.Columns.Add("VKM 2020", typeof(string));
        //        foreach (string year in years)
        //        {
        //            dt.Columns.Add("VKM" + " " + year + "(Planned)", typeof(string)); //add vkm text to yr
        //            dt.Columns.Add("VKM" + " " + year + "(Utilized)", typeof(string));

        //        }


        //        //DataRow dr = dt.NewRow();
        //        //dr[0] = "MAE";
        //        //int rcnt = 1;
        //        //int rcnt1 = 2; //2
        //        for (var i = 0; i < Top_ItemBudget.Count(); i++)
        //        {
        //            DataRow dr = dt.NewRow();
        //            dr[0] = Top_ItemBudget[i].ItemName.Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", "");
        //            int rcnt = 1;
        //            int rcnt1 = 2;
        //            foreach (var info in viewList)
        //            {



        //                dr[rcnt] = info.P_Item_Totals[i].ToString("C", CultureInfo.CurrentCulture);
        //                dr[rcnt1] = info.U_Item_Totals[i];
        //                //rcnt++;
        //                rcnt = rcnt + 2;
        //                rcnt1 = rcnt1 + 2;

        //            }
        //            dt.Rows.Add(dr);

        //        }
        //        //dt.Rows.Add(dr);
        //        // dt.Rows.Add(dr);

        //        //dr = dt.NewRow(); 
        //        // dr[0] = "Non-MAE";


        //        // dt.Rows.Add(dr);
        //        for (int i = 0; i <= dt.Columns.Count - 1; i++)
        //        {
        //            _col.Add(new columnsinfo { title = dt.Columns[i].ColumnName, data = dt.Columns[i].ColumnName });
        //        }

        //        string col = (string)serializer.Serialize(_col);
        //        t.columns = col;


        //        var lst = dt.AsEnumerable()
        //        .Select(r => r.Table.Columns.Cast<DataColumn>()
        //                .Select(c => new KeyValuePair<string, object>(c.ColumnName, r[c.Ordinal])
        //               ).ToDictionary(z => z.Key, z => z.Value)
        //        ).ToList();

        //        string data = serializer.Serialize(lst);
        //        t.data = data;




        //        return Json(new { data = t, message = selected_ccxc }, JsonRequestBehavior.AllowGet);

        //    }
        //}

        //public ActionResult CostElementDrillDown_comparison(List<string> years, string CostElement_Chosen, List<string> buList, bool chart = false, string selected_ccxc = "")
        //{

        //    if (lstUsers == null)
        //    {
        //        return RedirectToAction("Index", "Budgeting");
        //    }


        //    using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //    //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //    {
        //        List<RequestItems_Table> reqList = new List<RequestItems_Table>();
        //        List<RequestItems_Table> reqList_forquery = new List<RequestItems_Table>();
        //        List<CategoryBudget> categoryBudgets = new List<CategoryBudget>();
        //        List<CategoryBudget> Top70_CategoryBudget = new List<CategoryBudget>();
        //        List<costelement_drilldownSummary> viewList = new List<costelement_drilldownSummary>();
        //        string costelement_chosen_id = string.Empty;
        //        reqList = db.RequestItems_Table.ToList<RequestItems_Table>().FindAll(v => v.Fund == null || v.Fund.Trim().Contains("2"));
        //        int BaseYear_toCompare = 0;
        //        //List<RequestItemsList_2019> reqList2019 = new List<RequestItemsList_2019>();

        //        //Fetching the Category List
        //        foreach (var category in lstPrdCateg)
        //        {
        //            CategoryBudget catbud = new CategoryBudget();
        //            catbud.CategoryID = category.ID;
        //            catbud.CategoryName = category.Category;
        //            categoryBudgets.Add(catbud);
        //        }


        //        //CostElement_Chosen is costelt name -> fetch its id
        //        costelement_chosen_id = lstCostElement.Find(cost => cost.CostElement.ToUpper().Trim() == CostElement_Chosen.ToUpper().Trim()).ID.ToString().Trim();

        //        foreach (var bu_item in buList)
        //        {
        //            reqList_forquery.AddRange(reqList.FindAll(item => item.CostElement.Trim() == costelement_chosen_id).FindAll(ss => ss.BU.Contains(bu_item)));
        //        }

        //        //CODE TO GET GROUPS OF CATEGORY


        //        foreach (string yr in years)
        //        {
        //            BaseYear_toCompare++;
        //            //CC XC CHECK

        //            if ((int.Parse(yr) - 1) > 2020)  //for 2020 reqList_forquery has relevant data
        //            {
        //                string is_CCXC = string.Empty;
        //                bool CCXCflag = false;
        //                string presentUserDept = string.Empty;

        //                if (selected_ccxc.ToUpper().Contains("CC"))
        //                {
        //                    is_CCXC = "CC";
        //                    CCXCflag = true;
        //                }
        //                else if (selected_ccxc.ToUpper().Contains("XC"))
        //                {
        //                    is_CCXC = "XC";
        //                    CCXCflag = true;
        //                }


        //                else                    //DATA FILTER BASED ON USER'S NTID
        //                {
        //                    presentUserDept = lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).Department;

        //                    if (presentUserDept.ToUpper().Contains("CC"))
        //                    {
        //                        is_CCXC = "CC";
        //                        CCXCflag = true;
        //                    }
        //                    else if (presentUserDept.ToUpper().Contains("XC"))
        //                    {
        //                        is_CCXC = "XC";
        //                        CCXCflag = true;
        //                    }

        //                }

        //                if (CCXCflag)
        //                {
        //                    if (is_CCXC.Contains("XC"))
        //                    {
        //                        reqList_forquery.AddRange(reqList.FindAll(ss => ss.BU.Contains(lstBUs.Find(bu => bu.BU.Contains("2WP")).ID.ToString())));
        //                    }
        //                    reqList_forquery = reqList_forquery.FindAll(dpt => lstDEPTs.Find(x => x.ID.ToString().Contains(dpt.DEPT)).DEPT.Contains(is_CCXC))/*.FindAll(ss => ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("AS")).ID.ToString()) || ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("2WP")).ID.ToString()))*/;
        //                    CCXCflag = false;
        //                }



        //            }


        //            decimal[] P_Category_Totals = new decimal[categoryBudgets.Count()];
        //            decimal[] U_Category_Totals = new decimal[categoryBudgets.Count()];
        //            costelement_drilldownSummary tempobj = new costelement_drilldownSummary();
        //            tempobj.vkmyear = yr;


        //            //CODE TO GET THE TOTALS OF Top Category in 2020 Year - to compare with the other years Totals
        //            if (lstPrdCateg.Count != 0)
        //            {

        //                foreach (var catbudget in categoryBudgets)
        //                {
        //                    foreach (RequestItems_Table item in reqList_forquery)
        //                    {
        //                        if (item.DHAppDate.ToString().Contains((int.Parse(yr) - 1).ToString()))   // 2020 sh apprvd - 2021 planning
        //                        {
        //                            // if ((int.Parse(yr) - 1).ToString() == "2020")
        //                            if (BaseYear_toCompare == 1 | (BaseYear_toCompare == 2 && Top70_CategoryBudget.Count() == 0))
        //                            {

        //                                if (int.Parse(item.Category) == catbudget.CategoryID)
        //                                {


        //                                    if (item.ApprCost != null)
        //                                    {

        //                                        catbudget.CategoryTotals += (decimal)item.ApprCost;
        //                                    }


        //                                }

        //                            }



        //                        }

        //                    }
        //                }
        //                Top70_CategoryBudget = categoryBudgets.OrderByDescending(x => x.CategoryTotals).Take(70).ToList();

        //            }

        //            //Code to get the Planned and Utilized Category Totals
        //            int catcount = -1;
        //            foreach (var topcat in Top70_CategoryBudget)
        //            {
        //                catcount++;
        //                foreach (RequestItems_Table item in reqList_forquery)
        //                {
        //                    if (item.DHAppDate.ToString().Contains((int.Parse(yr) - 1).ToString()))
        //                    {
        //                        if (int.Parse(item.Category) == topcat.CategoryID)
        //                        {
        //                            P_Category_Totals[catcount] += (decimal)item.ApprCost;
        //                            U_Category_Totals[catcount] += item.OrderPrice == null ? (decimal)0.0 : (decimal)item.OrderPrice;

        //                        }
        //                    }
        //                }
        //            }

        //            tempobj.P_Category_Totals = P_Category_Totals;
        //            tempobj.U_Category_Totals = U_Category_Totals;

        //            viewList.Add(tempobj);

        //        }




        //        ///NEW CODE TO GET CATEGORY SUMMARY
        //        var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
        //        BudgetParam t = new BudgetParam();
        //        List<columnsinfo> _col = new List<columnsinfo>();
        //        System.Data.DataTable dt = new System.Data.DataTable();
        //        dt.Columns.Add("Category Name", typeof(string)); /// Planned + (year variable) / Utilized  + (year variable)
        //        //dt.Columns.Add("VKM 2020", typeof(string));
        //        foreach (string year in years)
        //        {
        //            dt.Columns.Add("VKM" + " " + year + "(Planned)", typeof(string)); //add vkm text to yr
        //            dt.Columns.Add("VKM" + " " + year + "(Utilized)", typeof(string));

        //        }


        //        //DataRow dr = dt.NewRow();
        //        //dr[0] = "MAE";
        //        //int rcnt = 1;
        //        //int rcnt1 = 2; //2
        //        for (var i = 0; i < Top70_CategoryBudget.Count(); i++)
        //        {
        //            DataRow dr = dt.NewRow();
        //            dr[0] = Top70_CategoryBudget[i].CategoryName.Trim().Replace("\r\n", "").Replace("\r", "").Replace("\r", "");
        //            int rcnt = 1;
        //            int rcnt1 = 2;
        //            foreach (var info in viewList)
        //            {



        //                dr[rcnt] = info.P_Category_Totals[i].ToString("C", CultureInfo.CurrentCulture);
        //                dr[rcnt1] = info.U_Category_Totals[i];
        //                //rcnt++;
        //                rcnt = rcnt + 2;
        //                rcnt1 = rcnt1 + 2;

        //            }
        //            dt.Rows.Add(dr);

        //        }
        //        //dt.Rows.Add(dr);
        //        // dt.Rows.Add(dr);

        //        //dr = dt.NewRow();
        //        // dr[0] = "Non-MAE";


        //        // dt.Rows.Add(dr);
        //        for (int i = 0; i <= dt.Columns.Count - 1; i++)
        //        {
        //            _col.Add(new columnsinfo { title = dt.Columns[i].ColumnName, data = dt.Columns[i].ColumnName });
        //        }

        //        string col = (string)serializer.Serialize(_col);
        //        t.columns = col;


        //        var lst = dt.AsEnumerable()
        //        .Select(r => r.Table.Columns.Cast<DataColumn>()
        //                .Select(c => new KeyValuePair<string, object>(c.ColumnName, r[c.Ordinal])
        //               ).ToDictionary(z => z.Key, z => z.Value)
        //        ).ToList();

        //        string data = serializer.Serialize(lst);
        //        t.data = data;




        //        return Json(new { data = t, message = selected_ccxc }, JsonRequestBehavior.AllowGet);

        //        // return View();

        //    }
        //}





        ///// <summary>
        ///// Function to send BU summary data to view
        ///// </summary>
        ///// <returns></returns> 
        //public ActionResult CostElementDrillDown(string Year_isPO, string CostElement_Chosen, List<string> buList, string selected_ccxc = "")
        //{


        //    using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //    //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //    {
        //        List<CategoryBudget> categoryBudgets = new List<CategoryBudget>();
        //        List<RequestItems_Table> reqList = new List<RequestItems_Table>();


        //        foreach (var category in lstPrdCateg)
        //        {
        //            CategoryBudget catbud = new CategoryBudget();
        //            catbud.CategoryID = category.ID;
        //            catbud.CategoryName = category.Category;
        //            categoryBudgets.Add(catbud);
        //        }
        //        //categoryBudgets will comprise of all the Categories - ID and Name
        //        //Year_isPO - 
        //        string YearX = Year_isPO.Split('(')[0];
        //        int Year = int.Parse(YearX.Substring(4));
        //        var is_PO = Year_isPO.Split('(')[1];
        //        decimal OverallTotals = 0;
        //        string costelement_chosen_id = string.Empty;
        //        List<CategoryBudget> Whole_CategoryBudget = new List<CategoryBudget>();
        //        List<CategoryBudget> Top10_CategoryBudget = new List<CategoryBudget>();


        //        //if CostElement_Chosen is costelt name -> fetch its id
        //        costelement_chosen_id = lstCostElement.Find(cost => cost.CostElement.ToUpper().Trim() == CostElement_Chosen.ToUpper().Trim()).ID.ToString().Trim();
        //        //else costelement_chosen_id = CostElement_Chosen

        //        foreach (var bu_item in buList)
        //        {
        //            //reqList_forquery ;
        //            reqList.AddRange(db.RequestItems_Table.Where(item => item.CostElement.Trim() == costelement_chosen_id).ToList().FindAll(ss => ss.BU.Contains(bu_item)/*(lstBUs.Find(bu => bu.BU.Trim().Equals(bu_item.Trim())).ID.ToString())*/));
        //        }


        //            if (Year != 2020 && !(Year == 2021 && is_PO.Contains("Planned")))  //for 2020 reqList_forquery has relevant data 2021planned - no check 2021utili - check 2022plan check
        //            {
        //                string is_CCXC = string.Empty;
        //                bool CCXCflag = false;
        //                string presentUserDept = string.Empty;
        //                if (selected_ccxc != "") //DATA FILTER BASED ON CCXC DROPDOWM - NOT USER'S NTID
        //                {
        //                    if (selected_ccxc.ToUpper().Contains("CC"))
        //                    {
        //                        is_CCXC = "CC";
        //                        CCXCflag = true;
        //                    }
        //                    else if (selected_ccxc.ToUpper().Contains("XC"))
        //                    {
        //                        is_CCXC = "XC";
        //                        CCXCflag = true;
        //                    }

        //                }
        //                else                    //DATA FILTER BASED ON USER'S NTID
        //                {
        //                    presentUserDept = lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).Department;

        //                    if (presentUserDept.ToUpper().Contains("CC"))
        //                    {
        //                        is_CCXC = "CC";
        //                        CCXCflag = true;
        //                    }
        //                    else if (presentUserDept.ToUpper().Contains("XC"))
        //                    {
        //                        is_CCXC = "XC";
        //                        CCXCflag = true;
        //                    }

        //                }

        //                if (CCXCflag)
        //                {
        //                    if (is_CCXC.Contains("XC"))
        //                    {
        //                        reqList.AddRange(reqList.FindAll(ss => ss.BU.Contains(lstBUs.Find(bu => bu.BU.Contains("2WP")).ID.ToString())));
        //                    }
        //                    reqList = reqList.FindAll(dpt => lstDEPTs.Find(x => x.ID.ToString().Contains(dpt.DEPT)).DEPT.Contains(is_CCXC))/*.FindAll(ss => ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("AS")).ID.ToString()) || ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("2WP")).ID.ToString()))*/;
        //                    CCXCflag = false;
        //                }



        //            }
        //        if (is_PO.Contains("Planned"))
        //        {
        //            //use dhappdate == Year-1 check (since apprvd cost includes dh & sh)
        //            //get all reqlist items with chosen cost elt & DHApprvDate = Year-1
        //            reqList = reqList.Where(item=>item.ApprovalDH == true && item.DHAppDate != null).Where(item => item.DHAppDate.Value.Year == (Year-1)).OrderBy(item => item.Category).ToList()/*.FindAll(ss => ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("AS")).ID.ToString()) || ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("2WP")).ID.ToString()))*/;
        //            foreach (var catbudget in categoryBudgets)
        //            {
        //                foreach (var item in reqList)
        //                {
        //                    if (int.Parse(item.Category) == catbudget.CategoryID)
        //                    {
        //                        if (item.ApprCost != null)
        //                            catbudget.CategoryTotals += (decimal)item.ApprCost;
        //                        OverallTotals += (decimal)item.ApprCost;
        //                    }
        //                }
        //            }

        //            Whole_CategoryBudget = categoryBudgets.OrderByDescending(x => x.CategoryTotals).ToList();
        //            Top10_CategoryBudget = categoryBudgets.Take(10).ToList(); //check these categories in utilized

        //        }
        //        if (is_PO.Contains("Utilized"))
        //        {
        //            //use orderdate == Year check
        //            //get all reqlist items with chosen cost elt & OrderDate = Year
        //            reqList = reqList.Where(x=>x.OrderDate != null).Where(item => item.OrderDate.Value.Year == Year).OrderBy(item => item.Category).ToList();
        //            foreach (var catbud in categoryBudgets)
        //            {
        //                foreach (var item in reqList)
        //                {
        //                    if (int.Parse(item.Category) == catbud.CategoryID)
        //                    {
        //                        if (item.OrderPrice != null)
        //                            catbud.CategoryTotals += (decimal)item.OrderPrice;
        //                    }
        //                }
        //            }

        //            Whole_CategoryBudget = categoryBudgets.OrderByDescending(x => x.CategoryTotals).ToList();
        //            Top10_CategoryBudget = categoryBudgets.Take(10).ToList();


        //        }




        //        return Json(new { data = Whole_CategoryBudget, success = true, message = selected_ccxc }, JsonRequestBehavior.AllowGet);


        //    }
        //}


        //public ActionResult CategoryDrillDown_comparison(List<string> years, string CostElement_Chosen, string Category_Chosen, List<string> buList, bool chart = false, string selected_ccxc = "")
        //{

        //    if (lstUsers == null)
        //    {
        //        return RedirectToAction("Index", "Budgeting");
        //    }


        //    using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //    //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //    {
        //        List<RequestItems_Table> reqList = new List<RequestItems_Table>();
        //        List<RequestItems_Table> reqList_forquery = new List<RequestItems_Table>();
        //        reqList = db.RequestItems_Table.ToList<RequestItems_Table>().FindAll(v => v.Fund == null || v.Fund.Trim().Contains("2"));
        //        List<RequestItemsList_2019> reqList2019 = new List<RequestItemsList_2019>();
        //        List<ItemBudget> itemBudgets = new List<ItemBudget>();
        //        List<ItemBudget> Top_ItemBudget = new List<ItemBudget>();
        //        List<category_drilldownSummary> viewList = new List<category_drilldownSummary>();
        //        string costelement_chosen_id = string.Empty;
        //        int BaseYear_toCompare = 0;

        //        //CostElement_Chosen is costelt name -> fetch its id
        //        costelement_chosen_id = lstCostElement.Find(cost => cost.CostElement.ToUpper().Trim() == CostElement_Chosen.ToUpper().Trim()).ID.ToString().Trim();

        //        //Category_Chosen is costelt name -> fetch its id
        //        Category_Chosen = lstPrdCateg.Find(cat => cat.Category.ToUpper().Trim() == Category_Chosen.ToUpper().Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", "")).ID.ToString().Trim();

        //        foreach (var item in lstItems.Where(x => x.Category.Contains(Category_Chosen.ToString().Trim())))
        //        {
        //            ItemBudget ibud = new ItemBudget();
        //            ibud.ItemID = item.S_No;
        //            ibud.ItemName = item.Item_Name;


        //            if (itemBudgets.Count > 0 && itemBudgets.FindAll(ib => ib.ItemName.Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", "") == (ibud.ItemName.Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", ""))).Count != 0)
        //                continue;

        //            itemBudgets.Add(ibud);
        //        }
        //        foreach (var bu_item in buList)
        //        {

        //            reqList_forquery.AddRange(reqList.FindAll(item => item.CostElement.Trim() == costelement_chosen_id).FindAll(ss => ss.BU.Contains(bu_item)).FindAll(item => item.Category.Trim() == Category_Chosen.ToString().Trim()));

        //        }

        //        //CODE TO GET GROUPS OF COST ELEMENT


        //        foreach (string yr in years)
        //        {
        //            BaseYear_toCompare++;
        //            //CC XC CHECK
        //            //if(yr == "2020")
        //            //{
        //            //    reqList_forquery = reqList_forquery/*.FindAll(ss => ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("AS")).ID.ToString()) || ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("2WP")).ID.ToString()))*/;
        //            //}
        //            //else
        //            if ((int.Parse(yr) - 1) > 2020)  //for 2020 reqList_forquery has relevant data
        //            {
        //                string is_CCXC = string.Empty;
        //                bool CCXCflag = false;
        //                string presentUserDept = string.Empty;

        //                if (selected_ccxc.ToUpper().Contains("CC"))
        //                {
        //                    is_CCXC = "CC";
        //                    CCXCflag = true;
        //                }
        //                else if (selected_ccxc.ToUpper().Contains("XC"))
        //                {
        //                    is_CCXC = "XC";
        //                    CCXCflag = true;
        //                }

        //                else                    //DATA FILTER BASED ON USER'S NTID
        //                {
        //                    presentUserDept = lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).Department;

        //                    if (presentUserDept.ToUpper().Contains("CC"))
        //                    {
        //                        is_CCXC = "CC";
        //                        CCXCflag = true;
        //                    }
        //                    else if (presentUserDept.ToUpper().Contains("XC"))
        //                    {
        //                        is_CCXC = "XC";
        //                        CCXCflag = true;
        //                    }

        //                }

        //                if (CCXCflag)
        //                {
        //                    if (is_CCXC.Contains("XC"))
        //                    {
        //                        reqList_forquery.AddRange(reqList.FindAll(ss => ss.BU.Contains(lstBUs.Find(bu => bu.BU.Contains("2WP")).ID.ToString())));
        //                    }
        //                    reqList_forquery = reqList_forquery.FindAll(dpt => lstDEPTs.Find(x => x.ID.ToString().Contains(dpt.DEPT)).DEPT.Contains(is_CCXC))/*.FindAll(ss => ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("AS")).ID.ToString()) || ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("2WP")).ID.ToString()))*/;
        //                    CCXCflag = false;
        //                }



        //            }


        //            // decimal Category_Totals[10] = new decimal[10];
        //            decimal[] P_Item_Totals = new decimal[200];
        //            decimal[] U_Item_Totals = new decimal[200];
        //            category_drilldownSummary tempobj = new category_drilldownSummary();
        //            tempobj.vkmyear = yr;


        //            foreach (var itembudget in itemBudgets)
        //            {
        //                foreach (RequestItems_Table item in reqList_forquery)
        //                {
        //                    if (item.DHAppDate.ToString().Contains((int.Parse(yr) - 1).ToString()))   // 2020 dh apprvd - 2021 planning
        //                    {
        //                        //if ((int.Parse(yr) - 1).ToString() == "2020")
        //                        if (BaseYear_toCompare == 1 || (BaseYear_toCompare == 2 && Top_ItemBudget.Count() == 0))
        //                        {

        //                            if (lstItems.Find(x => x.S_No == int.Parse(item.ItemName)).Item_Name.Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", "") == itembudget.ItemName.Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", ""))
        //                            // if (int.Parse(item.ItemName) == itembudget.ItemID)
        //                            {
        //                                if (item.ApprCost != null)
        //                                    itembudget.ItemTotals += (decimal)item.ApprCost;

        //                            }

        //                        }

        //                    }

        //                }
        //            }
        //            Top_ItemBudget = itemBudgets.OrderByDescending(x => x.ItemTotals).Take(200).ToList();


        //            int itemcount = -1;
        //            foreach (var topitem in Top_ItemBudget)
        //            {
        //                itemcount++;
        //                foreach (RequestItems_Table item in reqList_forquery)
        //                {
        //                    if (item.DHAppDate.ToString().Contains((int.Parse(yr) - 1).ToString()))
        //                    {
        //                        if (lstItems.Find(x => x.S_No == int.Parse(item.ItemName)).Item_Name.Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", "") == topitem.ItemName.Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", ""))
        //                        //if (int.Parse(item.ItemName) == topitem.ItemID)
        //                        {
        //                            P_Item_Totals[itemcount] += (decimal)item.ApprCost;
        //                            U_Item_Totals[itemcount] += item.OrderPrice == null ? (decimal)0.0 : (decimal)item.OrderPrice;

        //                        }


        //                    }
        //                }
        //            }


        //            tempobj.P_Item_Totals = P_Item_Totals;
        //            tempobj.U_Item_Totals = U_Item_Totals;

        //            viewList.Add(tempobj);

        //        }


        //        var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
        //        BudgetParam t = new BudgetParam();
        //        List<columnsinfo> _col = new List<columnsinfo>();
        //        System.Data.DataTable dt = new System.Data.DataTable();
        //        dt.Columns.Add("Item Name", typeof(string)); /// Planned + (year variable) / Utilized  + (year variable)
        //        //dt.Columns.Add("VKM 2020", typeof(string));
        //        foreach (string year in years)
        //        {
        //            dt.Columns.Add("VKM" + " " + year + "(Planned)", typeof(string)); //add vkm text to yr
        //            dt.Columns.Add("VKM" + " " + year + "(Utilized)", typeof(string));

        //        }


        //        //DataRow dr = dt.NewRow();
        //        //dr[0] = "MAE";
        //        //int rcnt = 1;
        //        //int rcnt1 = 2; //2
        //        for (var i = 0; i < Top_ItemBudget.Count(); i++)
        //        {
        //            DataRow dr = dt.NewRow();
        //            dr[0] = Top_ItemBudget[i].ItemName.Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", "");
        //            int rcnt = 1;
        //            int rcnt1 = 2;
        //            foreach (var info in viewList)
        //            {



        //                dr[rcnt] = info.P_Item_Totals[i].ToString("C", CultureInfo.CurrentCulture);
        //                dr[rcnt1] = info.U_Item_Totals[i];
        //                //rcnt++;
        //                rcnt = rcnt + 2;
        //                rcnt1 = rcnt1 + 2;

        //            }
        //            dt.Rows.Add(dr);

        //        }
        //        //dt.Rows.Add(dr);
        //        // dt.Rows.Add(dr);

        //        //dr = dt.NewRow(); 
        //        // dr[0] = "Non-MAE";


        //        // dt.Rows.Add(dr);
        //        for (int i = 0; i <= dt.Columns.Count - 1; i++)
        //        {
        //            _col.Add(new columnsinfo { title = dt.Columns[i].ColumnName, data = dt.Columns[i].ColumnName });
        //        }

        //        string col = (string)serializer.Serialize(_col);
        //        t.columns = col;


        //        var lst = dt.AsEnumerable()
        //        .Select(r => r.Table.Columns.Cast<DataColumn>()
        //                .Select(c => new KeyValuePair<string, object>(c.ColumnName, r[c.Ordinal])
        //               ).ToDictionary(z => z.Key, z => z.Value)
        //        ).ToList();

        //        string data = serializer.Serialize(lst);
        //        t.data = data;




        //        return Json(new { data = t, message = selected_ccxc }, JsonRequestBehavior.AllowGet);

        //    }
        //}




        ///// <summary>
        ///// Function to send BU summary data to view
        ///// </summary>
        ///// <returns></returns> 
        //public ActionResult CategoryDrillDown(string Year_isPO, string CostElement_Chosen, int Category_Chosen, List<string> buList, string selected_ccxc = "")
        //{


        //    using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //    //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //    {
        //        List<ItemBudget> itemBudgets = new List<ItemBudget>();
        //        List<RequestItems_Table> reqList = new List<RequestItems_Table>();


        //        foreach (var item in lstItems.Where(x => x.Category.Contains(Category_Chosen.ToString().Trim())))
        //        {
        //            ItemBudget ibud = new ItemBudget();
        //            ibud.ItemID = item.S_No;
        //            ibud.ItemName = item.Item_Name;
        //            itemBudgets.Add(ibud);
        //        }
        //        //categoryBudgets will comprise of all the Categories - ID and Name
        //        //Year_isPO - 
        //        string YearX = Year_isPO.Split('(')[0];
        //        int Year = int.Parse(YearX.Substring(4));
        //        var is_PO = Year_isPO.Split('(')[1];
        //        decimal OverallItemTotals = 0;
        //        string costelement_chosen_id = string.Empty;
        //        List<ItemBudget> Whole_ItemBudget = new List<ItemBudget>();
        //        List<ItemBudget> Top10_ItemBudget = new List<ItemBudget>();


        //        costelement_chosen_id = lstCostElement.Find(cost => cost.CostElement.ToUpper().Trim() == CostElement_Chosen.ToUpper().Trim()).ID.ToString().Trim();

        //        foreach (var bu_item in buList)
        //        {
        //            //reqList_forquery ;
        //            reqList.AddRange(db.RequestItems_Table.Where(item => item.CostElement.Trim() == costelement_chosen_id).ToList().FindAll(ss => ss.BU.Contains(bu_item)).Where(item => item.Category.Trim() == Category_Chosen.ToString().Trim()));
        //        }

        //        var zzz = Year != 2020;
        //        var aaa = !(Year == 2021 && is_PO.Contains("Planned"));
        //        var xxx = Year != 2020 && !(Year == 2021 && is_PO.Contains("Planned"));
        //        if (Year != 2020 && !(Year == 2021 && is_PO.Contains("Planned")))   //for 2020 reqList_forquery has relevant data
        //        {
        //            string is_CCXC = string.Empty;
        //            bool CCXCflag = false;
        //            string presentUserDept = string.Empty;
        //            if (selected_ccxc != "") //DATA FILTER BASED ON CCXC DROPDOWM - NOT USER'S NTID
        //            {
        //                if (selected_ccxc.ToUpper().Contains("CC"))
        //                {
        //                    is_CCXC = "CC";
        //                    CCXCflag = true;
        //                }
        //                else if (selected_ccxc.ToUpper().Contains("XC"))
        //                {
        //                    is_CCXC = "XC";
        //                    CCXCflag = true;
        //                }

        //            }
        //            else                    //DATA FILTER BASED ON USER'S NTID
        //            {
        //                presentUserDept = lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).Department;

        //                if (presentUserDept.ToUpper().Contains("CC"))
        //                {
        //                    is_CCXC = "CC";
        //                    CCXCflag = true;
        //                }
        //                else if (presentUserDept.ToUpper().Contains("XC"))
        //                {
        //                    is_CCXC = "XC";
        //                    CCXCflag = true;
        //                }

        //            }

        //            if (CCXCflag)
        //            {
        //                if (is_CCXC.Contains("XC"))
        //                {
        //                    reqList.AddRange(reqList.FindAll(ss => ss.BU.Contains(lstBUs.Find(bu => bu.BU.Contains("2WP")).ID.ToString())));
        //                }
        //                reqList = reqList.FindAll(dpt => lstDEPTs.Find(x => x.ID.ToString().Contains(dpt.DEPT)).DEPT.Contains(is_CCXC))/*.FindAll(ss => ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("AS")).ID.ToString()) || ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("2WP")).ID.ToString()))*/;
        //                CCXCflag = false;
        //            }



        //        }

        //        if (is_PO.Contains("Planned"))
        //        {
        //            //use dhappdate == Year-1 check (since apprvd cost includes dh & sh)
        //            //get all reqlist items with chosen cost elt & DHApprvDate = Year-1
        //            reqList = reqList.Where(x => x.DHAppDate != null).Where(item => item.DHAppDate.Value.Year == (Year-1)).OrderBy(item => item.ItemName).ToList()/*.FindAll(ss => ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("AS")).ID.ToString()) || ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("2WP")).ID.ToString()))*/;
        //            foreach (var itembudget in itemBudgets)
        //            {
        //                foreach (var item in reqList)
        //                {
        //                    if (int.Parse(item.ItemName) == itembudget.ItemID)
        //                    {
        //                        if (item.ApprCost != null)
        //                            itembudget.ItemTotals += (decimal)item.ApprCost;
        //                        OverallItemTotals += (decimal)item.ApprCost;
        //                    }
        //                }
        //            }

        //            Whole_ItemBudget = itemBudgets.OrderByDescending(x => x.ItemTotals).ToList();
        //            Top10_ItemBudget = itemBudgets.Take(10).ToList();

        //        }
        //        if (is_PO.Contains("Utilized"))
        //        {
        //            //use orderdate == Year check
        //            //get all reqlist items with chosen cost elt & OrderDate = Year
        //            reqList = reqList.Where(x => x.OrderDate != null).Where(item => item.OrderDate.Value.Year == Year).OrderBy(item => item.Category).ToList();
        //            foreach (var catbud in itemBudgets)
        //            {
        //                foreach (var item in reqList)
        //                {
        //                    if (int.Parse(item.ItemName) == catbud.ItemID)
        //                    {
        //                        if (item.OrderPrice != null)
        //                            catbud.ItemTotals += (decimal)item.OrderPrice;
        //                    }
        //                }
        //            }

        //            Whole_ItemBudget = itemBudgets.OrderByDescending(x => x.ItemTotals).ToList();
        //            Top10_ItemBudget = itemBudgets.Take(10).ToList();


        //        }




        //        return Json(new { data = Whole_ItemBudget, success = true, message = selected_ccxc }, JsonRequestBehavior.AllowGet);


        //    }
        //}



        //public ActionResult CategoryDrillDown_comparison(List<string> years, string CostElement_Chosen, string Category_Chosen, List<string> buList, bool chart = false, string selected_ccxc = "")
        //{

        //    if (lstUsers == null)
        //    {
        //        return RedirectToAction("Index", "Budgeting");
        //    }


        //    using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //    //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //    {
        //        List<RequestItems_Table> reqList = new List<RequestItems_Table>();
        //        List<RequestItems_Table> reqList_forquery = new List<RequestItems_Table>();
        //        reqList = db.RequestItems_Table.ToList<RequestItems_Table>().FindAll(v => v.Fund == null || v.Fund.Trim().Contains("2"));
        //        List<RequestItemsList_2019> reqList2019 = new List<RequestItemsList_2019>();
        //        List<ItemBudget> itemBudgets = new List<ItemBudget>();
        //        List<ItemBudget> Top_ItemBudget = new List<ItemBudget>();
        //        List<category_drilldownSummary> viewList = new List<category_drilldownSummary>();
        //        string costelement_chosen_id = string.Empty;
        //        int BaseYear_toCompare = 0;

        //        //CostElement_Chosen is costelt name -> fetch its id
        //        costelement_chosen_id = lstCostElement.Find(cost => cost.CostElement.ToUpper().Trim() == CostElement_Chosen.ToUpper().Trim()).ID.ToString().Trim();

        //        //Category_Chosen is costelt name -> fetch its id
        //        Category_Chosen = lstPrdCateg.Find(cat => cat.Category.ToUpper().Trim() == Category_Chosen.ToUpper().Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", "")).ID.ToString().Trim();

        //        foreach (var item in lstItems.Where(x => x.Category.Contains(Category_Chosen.ToString().Trim())))
        //        {
        //            ItemBudget ibud = new ItemBudget();
        //            ibud.ItemID = item.S_No;
        //            ibud.ItemName = item.Item_Name;


        //            if (itemBudgets.Count > 0 && itemBudgets.FindAll(ib => ib.ItemName.Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", "") == (ibud.ItemName.Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", ""))).Count != 0)
        //                continue;

        //            itemBudgets.Add(ibud);
        //        }
        //        foreach (var bu_item in buList)
        //        {

        //            reqList_forquery.AddRange(reqList.FindAll(item => item.CostElement.Trim() == costelement_chosen_id).FindAll(ss => ss.BU.Contains(bu_item)).FindAll(item => item.Category.Trim() == Category_Chosen.ToString().Trim()));

        //        }

        //        //CODE TO GET GROUPS OF COST ELEMENT


        //        foreach (string yr in years)
        //        {
        //            BaseYear_toCompare++;
        //            //CC XC CHECK
        //            //if(yr == "2020")
        //            //{
        //            //    reqList_forquery = reqList_forquery/*.FindAll(ss => ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("AS")).ID.ToString()) || ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("2WP")).ID.ToString()))*/;
        //            //}
        //            //else
        //            if ((int.Parse(yr) - 1) > 2020)  //for 2020 reqList_forquery has relevant data
        //            {
        //                string is_CCXC = string.Empty;
        //                bool CCXCflag = false;
        //                string presentUserDept = string.Empty;

        //                if (selected_ccxc.ToUpper().Contains("CC"))
        //                {
        //                    is_CCXC = "CC";
        //                    CCXCflag = true;
        //                }
        //                else if (selected_ccxc.ToUpper().Contains("XC"))
        //                {
        //                    is_CCXC = "XC";
        //                    CCXCflag = true;
        //                }

        //                else                    //DATA FILTER BASED ON USER'S NTID
        //                {
        //                    presentUserDept = lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).Department;

        //                    if (presentUserDept.ToUpper().Contains("CC"))
        //                    {
        //                        is_CCXC = "CC";
        //                        CCXCflag = true;
        //                    }
        //                    else if (presentUserDept.ToUpper().Contains("XC"))
        //                    {
        //                        is_CCXC = "XC";
        //                        CCXCflag = true;
        //                    }

        //                }

        //                if (CCXCflag)
        //                {
        //                    if (is_CCXC.Contains("XC"))
        //                    {
        //                        reqList_forquery.AddRange(reqList.FindAll(ss => ss.BU.Contains(lstBUs.Find(bu => bu.BU.Contains("2WP")).ID.ToString())));
        //                    }
        //                    reqList_forquery = reqList_forquery.FindAll(dpt => lstDEPTs.Find(x => x.ID.ToString().Contains(dpt.DEPT)).DEPT.Contains(is_CCXC))/*.FindAll(ss => ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("AS")).ID.ToString()) || ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("2WP")).ID.ToString()))*/;
        //                    CCXCflag = false;
        //                }



        //            }


        //            // decimal Category_Totals[10] = new decimal[10];
        //            decimal[] P_Item_Totals = new decimal[200];
        //            decimal[] U_Item_Totals = new decimal[200];
        //            category_drilldownSummary tempobj = new category_drilldownSummary();
        //            tempobj.vkmyear = yr;


        //            foreach (var itembudget in itemBudgets)
        //            {
        //                foreach (RequestItems_Table item in reqList_forquery)
        //                {
        //                    if (item.DHAppDate.ToString().Contains((int.Parse(yr) - 1).ToString()))   // 2020 dh apprvd - 2021 planning
        //                    {
        //                        //if ((int.Parse(yr) - 1).ToString() == "2020")
        //                        if (BaseYear_toCompare == 1 || (BaseYear_toCompare == 2 && Top_ItemBudget.Count() == 0))
        //                        {

        //                            if (lstItems.Find(x => x.S_No == int.Parse(item.ItemName)).Item_Name.Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", "") == itembudget.ItemName.Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", ""))
        //                            // if (int.Parse(item.ItemName) == itembudget.ItemID)
        //                            {
        //                                if (item.ApprCost != null)
        //                                    itembudget.ItemTotals += (decimal)item.ApprCost;

        //                            }

        //                        }

        //                    }

        //                }
        //            }
        //            Top_ItemBudget = itemBudgets.OrderByDescending(x => x.ItemTotals).Take(200).ToList();


        //            int itemcount = -1;
        //            foreach (var topitem in Top_ItemBudget)
        //            {
        //                itemcount++;
        //                foreach (RequestItems_Table item in reqList_forquery)
        //                {
        //                    if (item.DHAppDate.ToString().Contains((int.Parse(yr) - 1).ToString()))
        //                    {
        //                        if (lstItems.Find(x => x.S_No == int.Parse(item.ItemName)).Item_Name.Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", "") == topitem.ItemName.Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", ""))
        //                        //if (int.Parse(item.ItemName) == topitem.ItemID)
        //                        {
        //                            P_Item_Totals[itemcount] += (decimal)item.ApprCost;
        //                            U_Item_Totals[itemcount] += item.OrderPrice == null ? (decimal)0.0 : (decimal)item.OrderPrice;

        //                        }


        //                    }
        //                }
        //            }


        //            tempobj.P_Item_Totals = P_Item_Totals;
        //            tempobj.U_Item_Totals = U_Item_Totals;

        //            viewList.Add(tempobj);

        //        }


        //        var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
        //        BudgetParam t = new BudgetParam();
        //        List<columnsinfo> _col = new List<columnsinfo>();
        //        System.Data.DataTable dt = new System.Data.DataTable();
        //        dt.Columns.Add("Item Name", typeof(string)); /// Planned + (year variable) / Utilized  + (year variable)
        //        //dt.Columns.Add("VKM 2020", typeof(string));
        //        foreach (string year in years)
        //        {
        //            dt.Columns.Add("VKM" + " " + year + "(Planned)", typeof(string)); //add vkm text to yr
        //            dt.Columns.Add("VKM" + " " + year + "(Utilized)", typeof(string));

        //        }


        //        //DataRow dr = dt.NewRow();
        //        //dr[0] = "MAE";
        //        //int rcnt = 1;
        //        //int rcnt1 = 2; //2
        //        for (var i = 0; i < Top_ItemBudget.Count(); i++)
        //        {
        //            DataRow dr = dt.NewRow();
        //            dr[0] = Top_ItemBudget[i].ItemName.Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", "");
        //            int rcnt = 1;
        //            int rcnt1 = 2;
        //            foreach (var info in viewList)
        //            {



        //                dr[rcnt] = info.P_Item_Totals[i].ToString("C", CultureInfo.CurrentCulture);
        //                dr[rcnt1] = info.U_Item_Totals[i];
        //                //rcnt++;
        //                rcnt = rcnt + 2;
        //                rcnt1 = rcnt1 + 2;

        //            }
        //            dt.Rows.Add(dr);

        //        }
        //        //dt.Rows.Add(dr);
        //        // dt.Rows.Add(dr);

        //        //dr = dt.NewRow(); 
        //        // dr[0] = "Non-MAE";


        //        // dt.Rows.Add(dr);
        //        for (int i = 0; i <= dt.Columns.Count - 1; i++)
        //        {
        //            _col.Add(new columnsinfo { title = dt.Columns[i].ColumnName, data = dt.Columns[i].ColumnName });
        //        }

        //        string col = (string)serializer.Serialize(_col);
        //        t.columns = col;


        //        var lst = dt.AsEnumerable()
        //        .Select(r => r.Table.Columns.Cast<DataColumn>()
        //                .Select(c => new KeyValuePair<string, object>(c.ColumnName, r[c.Ordinal])
        //               ).ToDictionary(z => z.Key, z => z.Value)
        //        ).ToList();

        //        string data = serializer.Serialize(lst);
        //        t.data = data;




        //        return Json(new { data = t, message = selected_ccxc }, JsonRequestBehavior.AllowGet);

        //    }
        //}



        public ActionResult BUs_forpresentNTID_CCXC(List<string> selected_ccxc, bool ccxc = false)
        {

            if (lstBUs == null || lstBUs.Count == 0)
                InitialiseBudgeting();
            string presentNTID = User.Identity.Name.Split('\\')[1].ToUpper();
            string presentUserDept = string.Empty;
            List<string> allowedBUs = new List<string>();
            List<string> cc = new List<string>()
                { "MB", "2WP", "OSS"};
            List<string> xc = new List<string>()
                { "DA", "AD"};
            foreach (var bu in selected_ccxc)
            {
                if (bu.Trim() == "MB")
                {
                    selected_ccxc.Add("2WP");
                    break;
                }

            }
            foreach (var bu in selected_ccxc)
            {

                using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

                //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

                {

                    allowedBUs.Add(lstBUs.Find(bu1 => bu1.BU.Contains(bu)).ID.ToString());
                }

            }

            //if (ccxc == true)
            //{
            //    if (selected_ccxc.ToUpper().Trim().Contains("CC"))
            //    {
            //        foreach (var bu_cc in cc)
            //        {
            //            allowedBUs.Add(lstBUs.Find(bu => bu.BU.Contains(bu_cc)).ID.ToString());
            //        }
            //    }
            //    else if (selected_ccxc.ToUpper().Trim().Contains("XC"))
            //    {
            //        foreach (var bu_xc in xc)
            //        {
            //            allowedBUs.Add(lstBUs.Find(bu => bu.BU.Contains(bu_xc)).ID.ToString());
            //        }
            //    }
            //    else
            //    {

            //        //if (lstBU_SPOCs.Find(person => person.VKMspoc.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())) != null)
            //        //{
            //        //    var VKMSPOC = lstBU_SPOCs.FindAll(e => e.VKMspoc.ToUpper().Trim().Equals(User.Identity.Name.Split('\\')[1].ToUpper()));
            //        //    foreach (var i in VKMSPOC)
            //        //    {
            //        //        allowedBUs.Add(i.BU.ToString());
            //        //    }



            //            //}
            //            //else if (lstPrivileged.Find(person => person.ADSID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())) != null)
            //            //{
            //            //    var BU_of_VKMSPOC = lstPrivileged.Find(e => e.ADSID.ToUpper().Trim().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).BU;
            //            //    if (BU_of_VKMSPOC != null)
            //            //    {
            //            //        allowedBUs = (BU_of_VKMSPOC.Split(',')).ToList();

            //            //    }
            //            //}

            //            using(BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

            //            using(BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

            //            {
            //                if (db.Cockpit_UserIDs_Table.ToList().Find(person => person.ADSID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).BU != null)
            //                {
            //                    var BU_of_SPOC = db.Cockpit_UserIDs_Table.ToList().Find(e => e.ADSID.ToUpper().Trim().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).BU;
            //                    if (BU_of_SPOC != null)
            //                    {
            //                        allowedBUs = (BU_of_SPOC.Split(',')).ToList();

            //                    }
            //                }
            //                else
            //                {
            //                    presentUserDept = lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).Department;

            //                    if (presentUserDept.ToUpper().Contains("CC"))
            //                    {
            //                        foreach (var bu_cc in cc)
            //                        {
            //                            allowedBUs.Add(lstBUs.Find(bu => bu.BU.Contains(bu_cc)).ID.ToString());
            //                        }
            //                    }
            //                    else if (presentUserDept.ToUpper().Contains("XC"))
            //                    {
            //                        foreach (var bu_xc in xc)
            //                        {
            //                            allowedBUs.Add(lstBUs.Find(bu => bu.BU.Contains(bu_xc)).ID.ToString());
            //                        }
            //                    }
            //                }

            //        }

            //        //else
            //        //{
            //        //    if (presentNTID == "MAE9COB")
            //        //    {
            //        //        allowedBUs.Add("1");
            //        //        allowedBUs.Add("3");
            //        //    }
            //        //    else
            //        //    {
            //        //        return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            //        //    }
            //        //}


            //    }
            //}

            return Json(new { data = allowedBUs, message = selected_ccxc, success = true }, JsonRequestBehavior.AllowGet);

        }


        [HttpGet]
        public ActionResult ItemID(string Item_Chosen)
        {

            return Json(new { data = lstItems.Find(x => x.Item_Name.Trim().ToUpper() == Item_Chosen.Trim().ToUpper()).S_No }, JsonRequestBehavior.AllowGet);

        }

        //[HttpGet]
        //public ActionResult CategoryName(int Category_Chosen)
        //{

        //    return Json(new { data = lstPrdCateg.Find(x => x.ID == Category_Chosen).Category }, JsonRequestBehavior.AllowGet);

        //}


        //public ActionResult ItemDrillDown_Index()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public ActionResult ItemDrillDown( /*string Years_Listasstring,*/ string CostElement_Chosen, int Category_Chosen, int Item_Chosen, List<string> buList, List<string> years,/*string BuList_Listasstring,*/ string selected_ccxc = "")
        //{

        //    //List<string> years = new List<string>();
        //    //List<string> buList = new List<string>();
        //    string costelement_chosen_id = string.Empty;

        //    //RequestListAttributes viewList = new 


        //    List<RequestItemsRepoView> viewList_itemdrilldown1 = new List<RequestItemsRepoView>();
        //    //if (Years_Listasstring != null)
        //    //{
        //    //    years = (Years_Listasstring.Split(',')).ToList();

        //    //}
        //    //if (BuList_Listasstring != null)
        //    //{
        //    //    buList = (BuList_Listasstring.Split(',')).ToList();

        //    //}
        //    //if CostElement_Chosen is costelt name -> fetch its id
        //    costelement_chosen_id = lstCostElement.Find(cost => cost.CostElement.ToUpper().Trim() == CostElement_Chosen.ToUpper().Trim()).ID.ToString().Trim();


        //    using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //    //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //    {
        //        List<RequestItems_Table> reqList = new List<RequestItems_Table>();



        //        foreach (var bu_item in buList)
        //        {
        //            //reqList_forquery ;
        //            reqList.AddRange(db.RequestItems_Table.ToList<RequestItems_Table>().FindAll(item => item.CostElement.Trim() == costelement_chosen_id).FindAll(ss => ss.BU.Contains(bu_item)).FindAll(item => item.Category.Trim() == Category_Chosen.ToString().Trim()).FindAll(z => z.ItemName.Contains(Item_Chosen.ToString())));

        //        }

        //        foreach (string yr in years)
        //        {
        //            reqList = reqList.FindAll(z => z.SubmitDate.Value.Year.ToString() == yr.Trim());
        //            //CC XC CHECK
        //            //if(yr == "2020")
        //            //{
        //            //    reqList_forquery = reqList_forquery/*.FindAll(ss => ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("AS")).ID.ToString()) || ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("2WP")).ID.ToString()))*/;
        //            //}
        //            //else
        //            if (yr != "2020")  //for 2020 reqList_forquery has relevant data
        //            {
        //                string is_CCXC = string.Empty;
        //                bool CCXCflag = false;
        //                string presentUserDept = string.Empty;
        //                if (selected_ccxc != "") //DATA FILTER BASED ON CCXC DROPDOWM - NOT USER'S NTID
        //                {
        //                    if (selected_ccxc.ToUpper().Contains("CC"))
        //                    {
        //                        is_CCXC = "CC";
        //                        CCXCflag = true;
        //                    }
        //                    else if (selected_ccxc.ToUpper().Contains("XC"))
        //                    {
        //                        is_CCXC = "XC";
        //                        CCXCflag = true;
        //                    }

        //                }
        //                else                    //DATA FILTER BASED ON USER'S NTID
        //                {
        //                    presentUserDept = lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).Department;

        //                    if (presentUserDept.ToUpper().Contains("CC"))
        //                    {
        //                        is_CCXC = "CC";
        //                        CCXCflag = true;
        //                    }
        //                    else if (presentUserDept.ToUpper().Contains("XC"))
        //                    {
        //                        is_CCXC = "XC";
        //                        CCXCflag = true;
        //                    }

        //                }

        //                if (CCXCflag)
        //                {
        //                    if (is_CCXC.Contains("XC"))
        //                    {
        //                        reqList = reqList.FindAll(ss => ss.BU.Contains(lstBUs.Find(bu => bu.BU.Contains("2WP")).ID.ToString()));
        //                    }
        //                    reqList = reqList.FindAll(dpt => lstDEPTs.Find(x => x.ID.ToString().Contains(dpt.DEPT)).DEPT.Contains(is_CCXC))/*.FindAll(ss => ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("AS")).ID.ToString()) || ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("2WP")).ID.ToString()))*/;
        //                    CCXCflag = false;
        //                }



        //            }
        //            //reqList = db.RequestItems_Table.ToList<RequestItems_Table>().FindAll(z=>z.CostElement.Contains(costelement_chosen_id.Trim())).FindAll(z=>z.Category.Contains(Category_Chosen.ToString())).FindAll(z=>z.ItemName.Contains(Item_Chosen.ToString()))/*.FindAll(x=>x.ApprovalDH == false)*/;

        //            db.Database.CommandTimeout = 500;


        //            foreach (RequestItems_Table item in reqList)
        //            {

        //                RequestItemsRepoView ritem = new RequestItemsRepoView();

        //                ritem.Category = int.Parse(item.Category);
        //                ritem.Comments = item.Comments;
        //                ritem.Cost_Element = int.Parse(item.CostElement);
        //                ritem.BU = int.Parse(item.BU);

        //                ritem.DEPT = int.Parse(item.DEPT);
        //                ritem.Group = int.Parse(item.Group);
        //                ritem.Item_Name = int.Parse(item.ItemName);
        //                ritem.OEM = int.Parse(item.OEM);
        //                ritem.Required_Quantity = item.ReqQuantity;
        //                ritem.RequestID = item.RequestID;

        //                ritem.Requestor = item.RequestorNT;
        //                ritem.Total_Price = item.TotalPrice;
        //                ritem.Unit_Price = item.UnitPrice;
        //                ritem.ApprovalHoE = item.ApprovalDH;
        //                ritem.ApprovalSH = item.ApprovalSH;
        //                ritem.ApprovedHoE = item.ApprovedDH;
        //                ritem.ApprovedSH = item.ApprovedSH;
        //                if (item.isCancelled != null)
        //                {
        //                    ritem.isCancelled = (int)item.isCancelled;
        //                }


        //                ritem.Total_Price = item.TotalPrice;
        //                ritem.Reviewer_1 = item.DHNT;
        //                ritem.Reviewer_2 = item.SHNT;

        //                ritem.RequestDate = item.RequestDate != null ? ((DateTime)item.RequestDate).ToString("yyyy-MM-dd") : string.Empty;
        //                ritem.SubmitDate = item.SubmitDate != null ? ((DateTime)item.SubmitDate).ToString("yyyy-MM-dd") : string.Empty;
        //                ritem.Review1_Date = item.DHAppDate != null ? ((DateTime)item.DHAppDate).ToString("yyyy-MM-dd") : string.Empty;
        //                ritem.Review2_Date = item.SHAppDate != null ? ((DateTime)item.SHAppDate).ToString("yyyy-MM-dd") : string.Empty;


        //                if (item.OrderStatus != null && item.OrderStatus.Trim() != "")
        //                {
        //                    ritem.OrderStatus = int.Parse(item.OrderStatus);

        //                }
        //                else
        //                {
        //                    ritem.OrderStatus = null;


        //                }

        //                //Checking Request Status
        //                if (ritem.ApprovalHoE == true && ritem.ApprovalSH == false)
        //                {
        //                    ritem.Request_Status = "In Review with HoE";
        //                }
        //                else if (ritem.ApprovalHoE == true && ritem.ApprovalSH == true && ritem.ApprovedSH == false)
        //                {
        //                    ritem.Request_Status = "In Review with VKM SPOC";
        //                }
        //                else if (ritem.ApprovalHoE == true && ritem.ApprovalSH == true && ritem.ApprovedSH == true)
        //                {
        //                    ritem.Request_Status = "Reviewed by VKM SPOC";
        //                }
        //                else if (ritem.ApprovalHoE == false && ritem.ApprovalSH == false && ritem.isCancelled == 1)
        //                {
        //                    ritem.Request_Status = "SentBack by HoE";
        //                }
        //                else if (ritem.ApprovalHoE == false && ritem.ApprovalSH == false && ritem.isCancelled == 2)
        //                {
        //                    ritem.Request_Status = "SentBack by VKM SPOC";
        //                }
        //                else
        //                {
        //                    ritem.Request_Status = "In Requestor's Queue";
        //                }











        //                viewList_itemdrilldown1.Add(ritem);


        //            }
        //        }
        //        // return viewList.FindAll(xi => xi.RequestDate.ToString().Contains(year))/*.FindAll(x => x.ApprovalHoE == false)*/;


        //    }
        //    viewList_itemdrilldown = viewList_itemdrilldown1;
        //    return Json(new { data = viewList_itemdrilldown1 }, JsonRequestBehavior.AllowGet);

        //}

        //public ActionResult GetData_ItemDrillDown()
        //{

        //    return Json(new { data = viewList_itemdrilldown }, JsonRequestBehavior.AllowGet);

        //}



        //[HttpPost]
        //public ActionResult ItemDrillDown_Index(string Years_Listasstring, string CostElement_Chosen, string Category_Chosen, string Item_Chosen/*, List<string> buList, List<string> years*/, string BuList_Listasstring, string selected_ccxc = "")
        //{

        //    List<string> years = new List<string>();
        //    List<string> buList = new List<string>();
        //    string costelement_chosen_id = string.Empty;

        //    RequestListAttributes viewList_itemdrilldown1 = new RequestListAttributes()
        //    {
        //        RequestItemsRepoView_model = new List<RequestItemsRepoView>()
        //    };

        //    if (Years_Listasstring != null)
        //    {
        //        years = (Years_Listasstring.Split(',')).ToList();

        //    }
        //    if (BuList_Listasstring != null)
        //    {
        //        buList = (BuList_Listasstring.Split(',')).ToList();

        //    }
        //    //if CostElement_Chosen is costelt name -> fetch its id
        //    costelement_chosen_id = lstCostElement.Find(cost => cost.CostElement.ToUpper().Trim() == CostElement_Chosen.ToUpper().Trim()).ID.ToString().Trim();
        //    Category_Chosen = lstPrdCateg.Find(cat => cat.Category.ToUpper().Trim() == Category_Chosen.ToUpper().Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", "")).ID.ToString().Trim();
        //    Item_Chosen = lstItems.Find(item => item.S_No == int.Parse(Item_Chosen)).Item_Name.ToUpper().Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", "").Replace("+", "").Replace(" ", string.Empty);




        //    using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //    //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //    {
        //        List<RequestItems_Table> reqList = new List<RequestItems_Table>();



        //        foreach (var bu_item in buList)
        //        {
        //            //reqList_forquery ;
        //            reqList.AddRange(db.RequestItems_Table.ToList<RequestItems_Table>().FindAll(item => item.ApprovedSH == true).FindAll(item => item.CostElement.Trim() == costelement_chosen_id).FindAll(ss => ss.BU.Contains(bu_item)).FindAll(item => item.Category.Trim() == Category_Chosen).FindAll(v => v.Fund == null || v.Fund.Trim().Contains("2")).FindAll(item => lstItems.Find(i => i.S_No.ToString().Trim() == item.ItemName.Trim()).Item_Name.ToUpper().Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", "").Replace("+", "").Replace(" ", string.Empty) == Item_Chosen.ToUpper().Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", "").Replace("+", "").Replace(" ", string.Empty)));//lstItems.Find(item => item.Item_Name.ToUpper().Trim() == Item_Chosen.ToUpper().Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", "")).S_No.ToString().Trim();
        //            //string Query = "Select * from RequestItems_Table where BU in (1, 3, 5) and (Fund is null or Fund = 2) and ApprovedSH = 1 and CostElement = "+ costelement_chosen_id +" and Category = "+Category_Chosen +" and DEPT in (Select id from DEPT_Table where DEPT not like '%-XC%')";



        //        }

        //        foreach (string yr in years)
        //        {
        //            if (reqList.FindAll(z => z.ApprovedSH == true && z.SubmitDate.Value.Year.ToString() == (int.Parse(yr) - 1).ToString().Trim()).Count != 0)
        //                reqList = reqList.FindAll(z => z.SubmitDate.Value.Year.ToString() == (int.Parse(yr) - 1).ToString().Trim());
        //            else
        //                continue;
        //            //CC XC CHECK
        //            //if(yr == "2020")
        //            //{
        //            //    reqList_forquery = reqList_forquery/*.FindAll(ss => ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("AS")).ID.ToString()) || ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("2WP")).ID.ToString()))*/;
        //            //}
        //            //else
        //            if ((int.Parse(yr) - 1) > 2020)  //for 2020 reqList_forquery has relevant data
        //            {
        //                string is_CCXC = string.Empty;
        //                bool CCXCflag = false;
        //                string presentUserDept = string.Empty;
        //                if (selected_ccxc != "") //DATA FILTER BASED ON CCXC DROPDOWM - NOT USER'S NTID
        //                {
        //                    if (selected_ccxc.ToUpper().Contains("CC"))
        //                    {
        //                        is_CCXC = "CC";
        //                        CCXCflag = true;
        //                    }
        //                    else if (selected_ccxc.ToUpper().Contains("XC"))
        //                    {
        //                        is_CCXC = "XC";
        //                        CCXCflag = true;
        //                    }

        //                }
        //                else                    //DATA FILTER BASED ON USER'S NTID
        //                {
        //                    presentUserDept = lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).Department;

        //                    if (presentUserDept.ToUpper().Contains("CC"))
        //                    {
        //                        is_CCXC = "CC";
        //                        CCXCflag = true;
        //                    }
        //                    else if (presentUserDept.ToUpper().Contains("XC"))
        //                    {
        //                        is_CCXC = "XC";
        //                        CCXCflag = true;
        //                    }

        //                }

        //                //if (CCXCflag)
        //                //{
        //                //    if (is_CCXC.Contains("XC"))
        //                //    {
        //                //        reqList = reqList.FindAll(ss => ss.BU.Contains(lstBUs.Find(bu => bu.BU.Contains("2WP")).ID.ToString()));
        //                //    }
        //                //    reqList = reqList.FindAll(dpt => lstDEPTs.Find(x => x.ID.ToString().Contains(dpt.DEPT)).DEPT.Contains(is_CCXC))/*.FindAll(ss => ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("AS")).ID.ToString()) || ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("2WP")).ID.ToString()))*/;
        //                //    CCXCflag = false;
        //                //}
        //                if (is_CCXC.Contains("XC"))
        //                {
        //                    reqList.AddRange(reqList.FindAll(ss => ss.BU.Contains(lstBUs.Find(bu => bu.BU.Contains("2WP")).ID.ToString())));
        //                    reqList = reqList.FindAll(dpt => lstDEPTs.Find(x => x.ID.ToString().Contains(dpt.DEPT)).DEPT.Contains(is_CCXC))/*.FindAll(ss => ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("AS")).ID.ToString()) || ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("2WP")).ID.ToString()))*/;

        //                }
        //                else
        //                {
        //                    //reqList_forquery = reqList_forquery.FindAll(dpt => !lstDEPTs.Find(x => x.ID.ToString().Contains(dpt.DEPT)).DEPT.Contains("XC"))/*.FindAll(ss => ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("AS")).ID.ToString()) || ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("2WP")).ID.ToString()))*/;
        //                    DataTable dt1 = new DataTable();
        //                    connection();
        //                    string Query = " Select * from RequestItems_Table where BU in (1, 3, 5) and (Fund is null or Fund = 2) and ApprovedSH = 1 and CostElement = "+costelement_chosen_id +" and Category = "+Category_Chosen+" and DEPT in (Select id from DEPT_Table where DEPT not like '%-XC%') and ItemName in (Select S#No from ItemsCostList_Table where UPPER(TRIM(REPLACE(REPLACE(REPLACE(REPLACE(REPLACE(ItemName,'\r\n',''),'\r',''),'\n',''),'+',''),' ',''))) = "+Item_Chosen +" and VKM_Year = " + yr  ;
        //                    OpenConnection();
        //                    SqlCommand cmd = new SqlCommand(Query, conn);
        //                    SqlDataAdapter da = new SqlDataAdapter(cmd);
        //                    da.Fill(dt1);
        //                    CloseConnection();
        //                    reqList = new List<RequestItems_Table>();
        //                    foreach (DataRow item in dt1.Rows)
        //                    {
        //                        try
        //                        {

        //                            RequestItems_Table ritem = new RequestItems_Table();
        //                            ritem.ApprQuantity = Convert.ToInt32(item["ApprQuantity"]);
        //                            ritem.ApprCost = Convert.ToDecimal(item["ApprCost"]);

        //                            ritem.VKM_Year = Convert.ToInt32(item["VKM_Year"]);
        //                            if (item["UpdatedAt"].ToString() != "")
        //                                ritem.UpdatedAt = Convert.ToDateTime(item["UpdatedAt"]);
        //                            if (item["RequestorNTID"].ToString() != "")
        //                                ritem.RequestorNTID = item["RequestorNTID"].ToString();
        //                            ritem.Category = item["Category"].ToString();
        //                            ritem.Comments = item["Comments"].ToString();
        //                            ritem.Project = item["Project"].ToString();
        //                            if (item["ActualAvailableQuantity"].ToString() == "")
        //                                ritem.ActualAvailableQuantity = "NA";
        //                            else
        //                                ritem.ActualAvailableQuantity = item["ActualAvailableQuantity"].ToString();

        //                            ritem.CostElement = item["CostElement"].ToString();
        //                            ritem.BU = item["BU"].ToString();

        //                            ritem.DEPT = item["DEPT"].ToString();
        //                            ritem.Group = item["Group"].ToString();
        //                            ritem.ItemName = item["ItemName"].ToString();
        //                            ritem.OEM = item["OEM"].ToString();
        //                            ritem.ReqQuantity = Convert.ToInt32(item["ReqQuantity"]);
        //                            ritem.RequestID = Convert.ToInt32(item["RequestID"]);

        //                            ritem.RequestorNT = item["RequestorNT"].ToString();
        //                            ritem.TotalPrice = Convert.ToDecimal(item["TotalPrice"]);
        //                            ritem.OrderPrice = item["OrderPrice"].ToString() != "" ? Convert.ToDecimal(item["OrderPrice"]) : 0;
        //                            ritem.UnitPrice = Convert.ToDecimal(item["UnitPrice"]);
        //                            ritem.ApprovalDH = Convert.ToBoolean(item["ApprovalDH"]);
        //                            ritem.ApprovalSH = Convert.ToBoolean(item["ApprovalSH"]);
        //                            ritem.ApprovedDH = Convert.ToBoolean(item["ApprovedDH"]);
        //                            ritem.ApprovedSH = Convert.ToBoolean(item["ApprovedSH"]);
        //                            if (item["IsCancelled"].ToString() != "")
        //                                ritem.isCancelled = Convert.ToInt32(item["IsCancelled"]);

        //                            ritem.DHNT = item["DHNT"].ToString();
        //                            ritem.SHNT = item["SHNT"].ToString();

        //                            if (item["RequestDate"].ToString() != "")
        //                                ritem.RequestDate = (DateTime)item["RequestDate"];

        //                            if (item["SubmitDate"].ToString() != "")
        //                                ritem.SubmitDate = (DateTime)item["SubmitDate"];

        //                            if (item["DHAppDate"].ToString() != "")
        //                                ritem.DHAppDate = (DateTime)item["DHAppDate"];

        //                            if (item["SHAppDate"].ToString() != "")
        //                                ritem.SHAppDate = (DateTime)item["SHAppDate"];


        //                            if (item["OrderStatus"].ToString().Trim() != "")
        //                            {
        //                                ritem.OrderStatus = (item["OrderStatus"]).ToString();

        //                            }
        //                            else
        //                            {
        //                                ritem.OrderStatus = null;


        //                            }
        //                            if (item["Project"].ToString() == "")
        //                                ritem.Project = string.Empty;
        //                            else
        //                                ritem.Project = item["Project"].ToString();


        //                            reqList.Add(ritem);

        //                        }
        //                        catch (Exception ex)
        //                        {

        //                        }

        //                    }

        //                }


        //            }
        //            //reqList = db.RequestItems_Table.ToList<RequestItems_Table>().FindAll(z=>z.CostElement.Contains(costelement_chosen_id.Trim())).FindAll(z=>z.Category.Contains(Category_Chosen.ToString())).FindAll(z=>z.ItemName.Contains(Item_Chosen.ToString()))/*.FindAll(x=>x.ApprovalDH == false)*/;

        //            db.Database.CommandTimeout = 500;


        //            foreach (RequestItems_Table item in reqList)
        //            {

        //                RequestItemsRepoView ritem = new RequestItemsRepoView();

        //                ritem.Category = int.Parse(item.Category);
        //                ritem.Comments = item.Comments;
        //                ritem.Cost_Element = int.Parse(item.CostElement);
        //                ritem.BU = int.Parse(item.BU);

        //                ritem.DEPT = int.Parse(item.DEPT);
        //                ritem.Group = int.Parse(item.Group);
        //                ritem.Item_Name = int.Parse(item.ItemName);
        //                ritem.OEM = int.Parse(item.OEM);
        //                ritem.Required_Quantity = item.ReqQuantity;
        //                ritem.Reviewed_Quantity = item.ApprQuantity;
        //                ritem.RequestID = item.RequestID;

        //                ritem.Requestor = item.RequestorNT;
        //                ritem.Total_Price = item.TotalPrice;
        //                ritem.Unit_Price = item.UnitPrice;
        //                ritem.ApprovalHoE = item.ApprovalDH;
        //                ritem.ApprovalSH = item.ApprovalSH;
        //                ritem.ApprovedHoE = item.ApprovedDH;
        //                ritem.ApprovedSH = item.ApprovedSH;
        //                if (item.isCancelled != null)
        //                {
        //                    ritem.isCancelled = (int)item.isCancelled;
        //                }


        //                ritem.Total_Price = item.TotalPrice;
        //                ritem.Reviewed_Cost = item.ApprCost;
        //                ritem.OrderedQuantity = item.OrderedQuantity;
        //                ritem.OrderPrice = item.OrderPrice;

        //                if (item.OrderStatus != null && item.OrderStatus.Trim() != "")
        //                {
        //                    ritem.OrderStatus = int.Parse(item.OrderStatus);

        //                }
        //                else
        //                {
        //                    ritem.OrderStatus = null;


        //                }
        //                ritem.OrderID = item.OrderID;
        //                ritem.Reviewer_1 = item.DHNT;
        //                ritem.Reviewer_2 = item.SHNT;

        //                ritem.RequestDate = item.RequestDate != null ? ((DateTime)item.RequestDate).ToString("yyyy-MM-dd") : string.Empty;
        //                ritem.SubmitDate = item.SubmitDate != null ? ((DateTime)item.SubmitDate).ToString("yyyy-MM-dd") : string.Empty;
        //                ritem.Review1_Date = item.DHAppDate != null ? ((DateTime)item.DHAppDate).ToString("yyyy-MM-dd") : string.Empty;
        //                ritem.Review2_Date = item.SHAppDate != null ? ((DateTime)item.SHAppDate).ToString("yyyy-MM-dd") : string.Empty;


        //                if (item.OrderStatus != null && item.OrderStatus.Trim() != "")
        //                {
        //                    ritem.OrderStatus = int.Parse(item.OrderStatus);

        //                }
        //                else
        //                {
        //                    ritem.OrderStatus = null;


        //                }

        //                //Checking Request Status
        //                if (ritem.ApprovalHoE == true && ritem.ApprovalSH == false)
        //                {
        //                    ritem.Request_Status = "In Review with HoE";
        //                }
        //                else if (ritem.ApprovalHoE == true && ritem.ApprovalSH == true && ritem.ApprovedSH == false)
        //                {
        //                    ritem.Request_Status = "In Review with VKM SPOC";
        //                }
        //                else if (ritem.ApprovalHoE == true && ritem.ApprovalSH == true && ritem.ApprovedSH == true)
        //                {
        //                    ritem.Request_Status = "Reviewed by VKM SPOC";
        //                }
        //                else if (ritem.ApprovalHoE == false && ritem.ApprovalSH == false && ritem.isCancelled == 1)
        //                {
        //                    ritem.Request_Status = "SentBack by HoE";
        //                }
        //                else if (ritem.ApprovalHoE == false && ritem.ApprovalSH == false && ritem.isCancelled == 2)
        //                {
        //                    ritem.Request_Status = "SentBack by VKM SPOC";
        //                }
        //                else
        //                {
        //                    ritem.Request_Status = "In Requestor's Queue";
        //                }









        //                viewList_itemdrilldown1.RequestItemsRepoView_model.Add(ritem);
        //                viewList_itemdrilldown1.Item_Name = Item_Chosen.Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", "");
        //                // viewList_itemdrilldown1.Add(ritem);


        //            }
        //        }
        //        // return viewList.FindAll(xi => xi.RequestDate.ToString().Contains(year))/*.FindAll(x => x.ApprovalHoE == false)*/;


        //    }

        //    return View(viewList_itemdrilldown1);

        //}



        [HttpPost]
        public ActionResult ItemDrillDown(string CostElement_Chosen, string Category_Chosen, string Item_Chosen, List<string> buList, List<string> years, string selected_ccxc = "")
        {
            if (lstBUs == null || lstBUs.Count == 0)
                InitialiseBudgeting();
            //string costelement_chosen_id = string.Empty;
            List<RequestItemsRepoView> viewList_itemdrilldown1 = new List<RequestItemsRepoView>();

            //costelement_chosen_id = lstCostElement.Find(cost => cost.CostElement.ToUpper().Trim() == CostElement_Chosen.ToUpper().Trim()).ID.ToString().Trim();
            //Category_Chosen = lstPrdCateg.Find(cat => cat.Category.ToUpper().Trim() == Category_Chosen.ToUpper().Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", "")).ID.ToString().Trim();
            var bu_string = string.Join(",", buList.ToArray());
            Item_Chosen = lstItems.Find(item => item.S_No == int.Parse(Item_Chosen)).Item_Name;


            using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

            //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

            {
                List<RequestItems_Table> reqList = new List<RequestItems_Table>();
                List<RequestItems_Table> reqList1 = new List<RequestItems_Table>();
                //foreach (var bu_item in buList)
                //{
                //    reqList.AddRange(db.RequestItems_Table.ToList<RequestItems_Table>().FindAll(item => item.ApprovedSH == true).FindAll(v => v.Fund == null || v.Fund.Trim().Contains("2")).FindAll(item => item.CostElement.Trim() == costelement_chosen_id).FindAll(ss => ss.BU.Contains(bu_item)).FindAll(item => item.Category.Trim() == Category_Chosen).FindAll(item => lstItems.Find(i => i.S_No.ToString().Trim() == item.ItemName.Trim()).Item_Name.ToUpper().Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", "").Replace("+", "").Replace(" ", string.Empty) == Item_Chosen.ToUpper().Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", "").Replace("+", "").Replace(" ", string.Empty)));
                //    //lstItems.Find(item => item.Item_Name.ToUpper().Trim() == Item_Chosen.ToUpper().Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", "")).S_No.ToString().Trim();

                //}

                foreach (string yr in years)
                {
                    //if (reqList.FindAll(z => z.ApprovedSH == true && z.SubmitDate.Value.Year.ToString() == (int.Parse(yr) - 1).ToString().Trim()).Count != 0)
                    //    reqList1 = reqList.FindAll(z => z.SubmitDate.Value.Year.ToString() == (int.Parse(yr) - 1).ToString().Trim());
                    //else
                    //    continue;
                    //if ((int.Parse(yr) - 1) > 2020)  //for 2020 reqList_forquery has relevant data
                    //{
                    //    string is_CCXC = string.Empty;
                    //    bool CCXCflag = false;
                    //    string presentUserDept = string.Empty;
                    //    if (selected_ccxc != "") //DATA FILTER BASED ON CCXC DROPDOWM - NOT USER'S NTID
                    //    {
                    //        if (selected_ccxc.ToUpper().Contains("CC"))
                    //        {
                    //            is_CCXC = "CC";
                    //            CCXCflag = true;
                    //        }
                    //        else if (selected_ccxc.ToUpper().Contains("XC"))
                    //        {
                    //            is_CCXC = "XC";
                    //            CCXCflag = true;
                    //        }

                    //    }
                    //    else                    //DATA FILTER BASED ON USER'S NTID
                    //    {
                    //        presentUserDept = lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).Department;

                    //        if (presentUserDept.ToUpper().Contains("CC"))
                    //        {
                    //            is_CCXC = "CC";
                    //            CCXCflag = true;
                    //        }
                    //        else if (presentUserDept.ToUpper().Contains("XC"))
                    //        {
                    //            is_CCXC = "XC";
                    //            CCXCflag = true;
                    //        }

                    //    }

                    //    if (CCXCflag)
                    //    {
                    //        if (is_CCXC.Contains("XC"))
                    //        {
                    //            reqList1 = reqList1.FindAll(ss => ss.BU.Contains(lstBUs.Find(bu => bu.BU.Contains("2WP")).ID.ToString()));
                    //            reqList1 = reqList1.FindAll(dpt => lstDEPTs.Find(x => x.ID.ToString().Contains(dpt.DEPT)).DEPT.Contains("XC"))/*.FindAll(ss => ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("AS")).ID.ToString()) || ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("2WP")).ID.ToString()))*/;

                    //        }
                    //        else
                    //        {
                    //            reqList1 = reqList1.FindAll(dpt => !lstDEPTs.Find(x => x.ID.ToString().Contains(dpt.DEPT)).DEPT.Contains("XC"))/*.FindAll(ss => ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("AS")).ID.ToString()) || ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("2WP")).ID.ToString()))*/;

                    //        }
                    //        CCXCflag = false;
                    //    }



                    //}

                    //db.Database.CommandTimeout = 500;
                    if (bu_string != "")
                    {
                        DataTable dt1 = new DataTable();
                        connection();
                        string Query = "Exec [dbo].[Cockpit_GetTotals_with2023] '" + bu_string + "', '" + int.Parse(yr.Trim()) + "','" + CostElement_Chosen.Trim() + "','" + Category_Chosen.Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", "") + "','" + Item_Chosen + "' ";

                        //"Select * from RequestItems_Table where BU in (1, 3, 5) and (Fund is null or Fund = 2) and ApprovedDH = 1 and DEPT in (Select id from DEPT_Table where DEPT not like '%-XC%')";
                        OpenConnection();
                        SqlCommand cmd = new SqlCommand(Query, conn);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt1);
                        CloseConnection();
                        //viewList_itemdrilldown1 = new List<RequestItemsRepoView>();
                        foreach (DataRow item in dt1.Rows)
                        {
                            try
                            {


                                RequestItemsRepoView ritem = new RequestItemsRepoView();
                                ritem.Projected_Amount = item["Projected_Amount"].ToString() != "" ? Convert.ToDecimal(item["Projected_Amount"]) : 0;
                                ritem.Unused_Amount = item["Unused_Amount"].ToString() != "" ? Convert.ToDecimal(item["Unused_Amount"]) : 0;

                                ritem.OrderedQuantity = item["OrderedQuantity"].ToString() != "" ? Convert.ToInt32(item["OrderedQuantity"]) : 0;
                                ritem.OrderID = item["OrderID"].ToString();
                                ritem.Reviewed_Quantity = Convert.ToInt32(item["ApprQuantity"]);
                                ritem.Reviewed_Cost = Convert.ToDecimal(item["ApprCost"]);
                                ritem.isCancelled = item["isCancelled"].ToString() != "" ? Convert.ToInt32(item["isCancelled"]) : 0;
                                ritem.VKM_Year = Convert.ToInt32(item["VKM_Year"]);
                                if (item["UpdatedAt"].ToString() != "")
                                    ritem.UpdatedAt = Convert.ToDateTime(item["UpdatedAt"]);
                                if (item["RequestorNTID"].ToString() != "")
                                    ritem.RequestorNTID = item["RequestorNTID"].ToString();
                                ritem.Category = int.Parse(item["Category"].ToString());
                                ritem.Comments = item["Comments"].ToString();
                                ritem.Project = item["Project"].ToString();
                                if (item["ActualAvailableQuantity"].ToString() == "")
                                    ritem.ActualAvailableQuantity = "NA";
                                else
                                    ritem.ActualAvailableQuantity = item["ActualAvailableQuantity"].ToString();

                                ritem.Cost_Element = int.Parse(item["CostElement"].ToString());
                                ritem.BU = int.Parse(item["BU"].ToString());

                                ritem.DEPT = int.Parse(item["DEPT"].ToString());
                                ritem.Group = int.Parse(item["Group"].ToString());
                                ritem.Item_Name = int.Parse(item["ItemName"].ToString());
                                ritem.OEM = int.Parse(item["OEM"].ToString());
                                ritem.Required_Quantity = Convert.ToInt32(item["ReqQuantity"]);
                                ritem.RequestID = Convert.ToInt32(item["RequestID"]);

                                ritem.Requestor = item["RequestorNT"].ToString();
                                ritem.Total_Price = Convert.ToDecimal(item["TotalPrice"]);
                                ritem.OrderPrice = item["OrderPrice"].ToString() != "" ? Convert.ToDecimal(item["OrderPrice"]) : 0;
                                ritem.Unit_Price = Convert.ToDecimal(item["UnitPrice"]);
                                ritem.ApprovalHoE = Convert.ToBoolean(item["ApprovalDH"]);
                                ritem.ApprovalSH = Convert.ToBoolean(item["ApprovalSH"]);
                                ritem.ApprovedHoE = Convert.ToBoolean(item["ApprovedDH"]);
                                ritem.ApprovedSH = Convert.ToBoolean(item["ApprovedSH"]);
                                if (item["IsCancelled"].ToString() != "")
                                    ritem.isCancelled = Convert.ToInt32(item["IsCancelled"]);

                                ritem.Reviewer_1 = item["DHNT"].ToString();
                                ritem.Reviewer_2 = item["SHNT"].ToString();

                                if (item["RequestDate"].ToString() != "")
                                    ritem.RequestDate = item["RequestDate"].ToString();

                                if (item["SubmitDate"].ToString() != "")
                                    ritem.SubmitDate = item["SubmitDate"].ToString();

                                if (item["DHAppDate"].ToString() != "")
                                    ritem.Review1_Date = item["DHAppDate"].ToString();

                                if (item["SHAppDate"].ToString() != "")
                                    ritem.Review2_Date = item["SHAppDate"].ToString();


                                if (item["OrderStatus"].ToString().Trim() != "")
                                {
                                    ritem.OrderStatus = int.Parse((item["OrderStatus"]).ToString());

                                }
                                else
                                {
                                    ritem.OrderStatus = null;


                                }
                                if (item["Project"].ToString() == "")
                                    ritem.Project = string.Empty;
                                else
                                    ritem.Project = item["Project"].ToString();
                                if (ritem.ApprovalHoE == true && ritem.ApprovalSH == false)
                                {
                                    ritem.Request_Status = "In Review with HoE";
                                }
                                else if (ritem.ApprovalHoE == true && ritem.ApprovalSH == true && ritem.ApprovedSH == false)
                                {
                                    ritem.Request_Status = "In Review with VKM SPOC";
                                }
                                else if (ritem.ApprovalHoE == true && ritem.ApprovalSH == true && ritem.ApprovedSH == true)
                                {
                                    ritem.Request_Status = "Reviewed by VKM SPOC";
                                }
                                else if (ritem.ApprovalHoE == false && ritem.ApprovalSH == false && ritem.isCancelled == 1)
                                {
                                    ritem.Request_Status = "SentBack by HoE";
                                }
                                else if (ritem.ApprovalHoE == false && ritem.ApprovalSH == false && ritem.isCancelled == 2)
                                {
                                    ritem.Request_Status = "SentBack by VKM SPOC";
                                }
                                else
                                {
                                    ritem.Request_Status = "In Requestor's Queue";
                                }

                                viewList_itemdrilldown1.Add(ritem);
                            }
                            catch (Exception ex)
                            {

                            }

                            //}

                        }
                        //foreach (RequestItems_Table item in reqList1.FindAll(x=>x.ApprovalSH==true))
                        //{

                        //    RequestItemsRepoView ritem = new requestItemsRepoView();

                        //    ritem.Category = int.Parse(item.Category);
                        //    ritem.Comments = item.Comments;
                        //    ritem.Cost_Element = int.Parse(item.CostElement);
                        //    ritem.BU = int.Parse(item.BU);

                        //    ritem.DEPT = int.Parse(item.DEPT);
                        //    ritem.Group = int.Parse(item.Group);
                        //    ritem.Item_Name = int.Parse(item.ItemName);
                        //    ritem.OEM = int.Parse(item.OEM);
                        //    ritem.Required_Quantity = item.ReqQuantity;
                        //    ritem.RequestID = item.RequestID;
                        //    ritem.Reviewed_Quantity = item.ApprQuantity;

                        //    ritem.Requestor = item.RequestorNT;
                        //    ritem.Total_Price = item.TotalPrice;
                        //    ritem.Unit_Price = item.UnitPrice;
                        //    ritem.ApprovalHoE = item.ApprovalDH;
                        //    ritem.ApprovalSH = item.ApprovalSH;
                        //    ritem.ApprovedHoE = item.ApprovedDH;
                        //    ritem.ApprovedSH = item.ApprovedSH;
                        //    if (item.isCancelled != null)
                        //    {
                        //        ritem.isCancelled = (int)item.isCancelled;
                        //    }


                        //    ritem.Total_Price = item.TotalPrice;
                        //    ritem.Reviewer_1 = item.DHNT;
                        //    ritem.Reviewer_2 = item.SHNT;

                        //    ritem.RequestDate = item.RequestDate != null ? ((DateTime)item.RequestDate).ToString("yyyy-MM-dd") : string.Empty;
                        //    ritem.SubmitDate = item.SubmitDate != null ? ((DateTime)item.SubmitDate).ToString("yyyy-MM-dd") : string.Empty;
                        //    ritem.Review1_Date = item.DHAppDate != null ? ((DateTime)item.DHAppDate).ToString("yyyy-MM-dd") : string.Empty;
                        //    ritem.Review2_Date = item.SHAppDate != null ? ((DateTime)item.SHAppDate).ToString("yyyy-MM-dd") : string.Empty;


                        //    if (item.OrderStatus != null && item.OrderStatus.Trim() != "")
                        //    {
                        //        ritem.OrderStatus = int.Parse(item.OrderStatus);

                        //    }
                        //    else
                        //    {
                        //        ritem.OrderStatus = null;


                        //    }

                        //    //Checking Request Status
                        //    if (ritem.ApprovalHoE == true && ritem.ApprovalSH == false)
                        //    {
                        //        ritem.Request_Status = "In Review with HoE";
                        //    }
                        //    else if (ritem.ApprovalHoE == true && ritem.ApprovalSH == true && ritem.ApprovedSH == false)
                        //    {
                        //        ritem.Request_Status = "In Review with VKM SPOC";
                        //    }
                        //    else if (ritem.ApprovalHoE == true && ritem.ApprovalSH == true && ritem.ApprovedSH == true)
                        //    {
                        //        ritem.Request_Status = "Reviewed by VKM SPOC";
                        //    }
                        //    else if (ritem.ApprovalHoE == false && ritem.ApprovalSH == false && ritem.isCancelled == 1)
                        //    {
                        //        ritem.Request_Status = "SentBack by HoE";
                        //    }
                        //    else if (ritem.ApprovalHoE == false && ritem.ApprovalSH == false && ritem.isCancelled == 2)
                        //    {
                        //        ritem.Request_Status = "SentBack by VKM SPOC";
                        //    }
                        //    else
                        //    {
                        //        ritem.Request_Status = "In Requestor's Queue";
                        //    }

                        //    ritem.Reviewed_Cost = item.ApprCost;
                        //    ritem.OrderedQuantity = item.OrderedQuantity;
                        //    ritem.OrderPrice = item.OrderPrice;

                        //    if (item.OrderStatus != null && item.OrderStatus.Trim() != "")
                        //    {
                        //        ritem.OrderStatus = int.Parse(item.OrderStatus);

                        //    }
                        //    else
                        //    {
                        //        ritem.OrderStatus = null;
                        //    }
                        //    ritem.OrderID = item.OrderID;

                        //    viewList_itemdrilldown1.Add(ritem);


                        //}
                    }
                }
                // return viewList.FindAll(xi => xi.RequestDate.ToString().Contains(year))/*.FindAll(x => x.ApprovalHoE == false)*/;


            }

            return Json(new { data = viewList_itemdrilldown1 }, JsonRequestBehavior.AllowGet);

        }



        //public partial class RequestListAttributes
        //{

        //    public List<RequestItemsRepoView> RequestItemsRepoView_model { get; set; }
        //    public string Item_Name { get; set; }
        //}

        //[HttpGet]
        //public ActionResult Lookup()
        //{
        //    LookupData lookupData = new LookupData();



        //    string presentUserDept = lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).Department;
        //    string is_CCXC = string.Empty; //For Clear CC XC Segregation

        //    lookupData.DEPT_List = lstDEPTs.ToList();

        //    lookupData.BU_List = lstBUs.OrderBy(x => x.ID).ToList();
        //    lookupData.BU_List[2].BU = "MB";
        //    lookupData.BU_List[4].BU = "OSS";
        //    lookupData.OEM_List = lstOEMs;
        //    //lookupData.Groups_oldList = lstGroups_old;

        //    //using(BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //    //using(BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //    lookupData.Groups_test = lstGroups_test;
        //    // lookupData.Groups_List = lstGroups;
        //    lookupData.Item_List = lstItems;
        //    lookupData.Category_List = lstPrdCateg;
        //    lookupData.CostElement_List = lstCostElement;

        //    return Json(new { data = lookupData }, JsonRequestBehavior.AllowGet);

        //}



        /////////////////////








        ///WITH REQUESTTABLE_TEST
        //public ActionResult Cockpit_Options()
        //{

        //    return View();
        //}

        //public ActionResult VKM_Cockpit()
        //{

        //    using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //    //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //    {
        //        if (lstUsers == null || lstUsers.Count == 0)
        //            lstUsers = db.SPOTONData_Table_2021.ToList<SPOTONData_Table_2021>();
        //        if (lstOEMs == null || lstOEMs.Count == 0)
        //            lstOEMs = db.OEM_Table.ToList<OEM_Table>();
        //        if (lstBUs == null || lstBUs.Count == 0)
        //            lstBUs = db.BU_Table.ToList<BU_Table>();
        //        if (lstDEPTs == null || lstDEPTs.Count == 0)
        //            lstDEPTs = db.DEPT_Table.ToList<DEPT_Table>();
        //        if (lstCostElement == null || lstCostElement.Count == 0)
        //            lstCostElement = db.CostElement_Table.ToList<CostElement_Table>();
        //        if (lstPrdCateg == null || lstPrdCateg.Count == 0)
        //            lstPrdCateg = db.Category_Table.ToList<Category_Table>();
        //        if (lstItems == null || lstItems.Count == 0)
        //            lstItems = db.ItemsCostList_Table.ToList<ItemsCostList_Table>()/*.FindAll(item=>item.Deleted != true)*/; //refresh this
        //        if (lstPrivileged == null || lstPrivileged.Count == 0)
        //            lstPrivileged = db.tbl_UserIDs_Table.ToList<tbl_UserIDs_Table>();
        //        if (lstBU_SPOCs == null || lstBU_SPOCs.Count == 0)
        //            lstBU_SPOCs = db.BU_SPOCS.ToList<BU_SPOCS>();
        //        if (lstGroups_test == null || lstGroups_test.Count == 0)
        //            lstGroups_test = db.Groups_Table_Test.ToList<Groups_Table_Test>();




        //        lstUsers.Sort((a, b) => a.EmployeeName.CompareTo(b.EmployeeName));
        //        lstBUs.Sort((a, b) => a.BU.CompareTo(b.BU));
        //        lstOEMs.Sort((a, b) => a.OEM.CompareTo(b.OEM));
        //        lstDEPTs.Sort((a, b) => a.DEPT.CompareTo(b.DEPT));
        //        lstCostElement.Sort((a, b) => a.CostElement.CompareTo(b.CostElement));
        //        lstPrdCateg.Sort((a, b) => a.Category.CompareTo(b.Category));
        //        lstItems.Sort((a, b) => a.Item_Name.CompareTo(b.Item_Name));
        //        lstPrivileged.Sort((a, b) => a.ADSID.CompareTo(b.ADSID));
        //        lstBU_SPOCs.Sort((a, b) => a.VKMspoc.CompareTo(b.VKMspoc));
        //        lstGroups_test.Sort((a, b) => a.Group.CompareTo(b.Group));

        //    }
        //    //if (lstUsers == null || lstUsers.Count == 0)
        //    return View();
        //}

        //[HttpPost]
        //public ActionResult GetInitData()
        //{

        //    using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //    //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //    {
        //        if (lstUsers == null || lstUsers.Count == 0)
        //            lstUsers = db.SPOTONData_Table_2021.ToList<SPOTONData_Table_2021>();
        //        if (lstOEMs == null || lstOEMs.Count == 0)
        //            lstOEMs = db.OEM_Table.ToList<OEM_Table>();
        //        if (lstBUs == null || lstBUs.Count == 0)
        //            lstBUs = db.BU_Table.ToList<BU_Table>();
        //        if (lstDEPTs == null || lstDEPTs.Count == 0)
        //            lstDEPTs = db.DEPT_Table.ToList<DEPT_Table>();
        //        if (lstCostElement == null || lstCostElement.Count == 0)
        //            lstCostElement = db.CostElement_Table.ToList<CostElement_Table>();
        //        if (lstPrdCateg == null || lstPrdCateg.Count == 0)
        //            lstPrdCateg = db.Category_Table.ToList<Category_Table>();
        //        if (lstItems == null || lstItems.Count == 0)
        //            lstItems = db.ItemsCostList_Table.ToList<ItemsCostList_Table>()/*.FindAll(item=>item.Deleted != true)*/; //refresh this
        //        if (lstPrivileged == null || lstPrivileged.Count == 0)
        //            lstPrivileged = db.tbl_UserIDs_Table.ToList<tbl_UserIDs_Table>();
        //        if (lstBU_SPOCs == null || lstBU_SPOCs.Count == 0)
        //            lstBU_SPOCs = db.BU_SPOCS.ToList<BU_SPOCS>();
        //        if (lstGroups_test == null || lstGroups_test.Count == 0)
        //            lstGroups_test = db.Groups_Table_Test.ToList<Groups_Table_Test>();




        //        lstUsers.Sort((a, b) => a.EmployeeName.CompareTo(b.EmployeeName));
        //        lstBUs.Sort((a, b) => a.BU.CompareTo(b.BU));
        //        lstOEMs.Sort((a, b) => a.OEM.CompareTo(b.OEM));
        //        lstDEPTs.Sort((a, b) => a.DEPT.CompareTo(b.DEPT));
        //        lstCostElement.Sort((a, b) => a.CostElement.CompareTo(b.CostElement));
        //        lstPrdCateg.Sort((a, b) => a.Category.CompareTo(b.Category));
        //        lstItems.Sort((a, b) => a.Item_Name.CompareTo(b.Item_Name));
        //        lstPrivileged.Sort((a, b) => a.ADSID.CompareTo(b.ADSID));
        //        lstBU_SPOCs.Sort((a, b) => a.VKMspoc.CompareTo(b.VKMspoc));
        //        lstGroups_test.Sort((a, b) => a.Group.CompareTo(b.Group));

        //    }
        //    //if (lstUsers == null || lstUsers.Count == 0)
        //    //{
        //    //    return RedirectToAction("Index", "Budgeting");
        //    //}

        //    //return View();
        //    return Json(new { data = true }, JsonRequestBehavior.AllowGet);
        //}






        ///// <summary>
        ///// Function to send BU summary data to view
        ///// </summary>
        ///// <returns></returns>
        //public ActionResult VKMSummaryData(List<string> years, List<string> buList, bool chart = false, string selected_ccxc = "")
        //{


        //    //chart = 0 (false - table), 1(table + graph), 2(graph)
        //    if (lstUsers == null)
        //    {
        //        return RedirectToAction("Index", "Budgeting");
        //    }


        //    using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //    //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //    {
        //        List<RequestItems_Table_TEST> reqList = new List<RequestItems_Table_TEST>();
        //        List<RequestItems_Table_TEST> reqList_forquery = new List<RequestItems_Table_TEST>();
        //        List<vkmSummary> viewList = new List<vkmSummary>();

        //        var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
        //        BudgetParam t = new BudgetParam();
        //        List<columnsinfo> _col = new List<columnsinfo>();


        //        decimal OP_MAE_Totals = 0, OP_NMAE_Totals = 0, OP_SoftwareTotals = 0;
        //        decimal OU_MAE_Totals = 0, OU_NMAE_Totals = 0, OU_SoftwareTotals = 0;
        //        decimal Ch_P_MAE_Totals = 0, Ch_P_NMAE_Totals = 0, Ch_P_SoftwareTotals = 0, Ch_P_OverallTotals = 0;
        //        decimal Ch_U_MAE_Totals = 0, Ch_U_NMAE_Totals = 0, Ch_U_SoftwareTotals = 0, Ch_U_OverallTotals = 0;
        //        decimal Pr_P_MAE_Totals = 0, Pr_P_NMAE_Totals = 0, Pr_P_SoftwareTotals = 0, Pr_P_OverallTotals = 0;
        //        decimal Pr_U_MAE_Totals = 0, Pr_U_NMAE_Totals = 0, Pr_U_SoftwareTotals = 0, Pr_U_OverallTotals = 0;

        //        // List<RequestItemsList_2019> reqList2019 = new List<RequestItemsList_2019>();
        //        //reqList2019 = db.RequestItemsList_2019.ToList<RequestItemsList_2019>();



        //        //  if (!(chart == false && years.Count() == 3)) //if number of years selected = 3, then no need to show table
        //        // {


        //        //get reqlist data for the BUs based on SPOC's BU / CC XC Level BUs
        //        foreach (var bu_item in buList)
        //        {
        //            reqList.AddRange(db.RequestItems_Table_TEST.ToList<RequestItems_Table_TEST>().FindAll(ss => ss.BU.Contains(bu_item)).FindAll(v => v.Fund == null || v.Fund.Trim().Contains("2")));
        //        }

        //        foreach (string yr in years)
        //        {
        //            //CC XC CHECK


        //            //for 2020 reqList_forquery has relevant data under the BU filtering ; but >2020 needs dept FIltering based on CC XC
        //            //if(int.Parse(yr)-1 == 2020)
        //            //{
        //            //    reqList_forquery.AddRange(reqList.FindAll(ss => ss.BU.Contains(lstBUs.Find(bu => bu.BU.Contains("DA")).ID.ToString())));
        //            //}
        //            if (int.Parse(yr) - 1 > 2020)
        //            {

        //                string is_CCXC = string.Empty;
        //                bool CCXCflag = false;
        //                string presentUserDept = string.Empty;
        //                // if (selected_ccxc != "") //DATA FILTER BASED ON CCXC DROPDOWM - NOT USER'S NTID
        //                // {
        //                if (selected_ccxc.ToUpper().Contains("CC"))
        //                {
        //                    is_CCXC = "CC";
        //                    CCXCflag = true;
        //                }
        //                else if (selected_ccxc.ToUpper().Contains("XC"))
        //                {
        //                    is_CCXC = "XC";
        //                    CCXCflag = true;
        //                }
        //                //   }
        //                else                    //DATA FILTER BASED ON USER'S NTID
        //                {
        //                    presentUserDept = lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).Department;

        //                    if (presentUserDept.ToUpper().Contains("CC"))
        //                    {
        //                        is_CCXC = "CC";
        //                        CCXCflag = true;
        //                    }
        //                    else if (presentUserDept.ToUpper().Contains("XC"))
        //                    {
        //                        is_CCXC = "XC";
        //                        CCXCflag = true;
        //                    }

        //                }

        //                if (CCXCflag)
        //                {
        //                    if (is_CCXC.Contains("XC"))
        //                    {
        //                        //XC includes 2WP 
        //                        reqList_forquery.AddRange(db.RequestItems_Table_TEST.ToList().FindAll(ss => ss.BU.Contains(lstBUs.Find(bu => bu.BU.Contains("2WP")).ID.ToString())).FindAll(v => v.Fund == null || v.Fund.Trim().Contains("2")));
        //                    }
        //                    reqList_forquery = reqList.FindAll(dpt => lstDEPTs.Find(x => x.ID.ToString().Contains(dpt.DEPT)).DEPT.Contains(is_CCXC));
        //                    CCXCflag = false;
        //                }



        //            }
        //            else
        //            {
        //                if (buList.Contains("2") && buList.Contains("4") /*&& selected_ccxc.ToUpper().Contains("Custom") || selected_ccxc.ToUpper().Contains("XC"))*/ && yr == "2021" && reqList.FindAll(c => c.BU == "1").Count == 0) //XC VKM2021 - include 2WP Whole Totals also
        //                {
        //                    reqList.AddRange(db.RequestItems_Table_TEST.ToList().FindAll(ss => ss.BU.Contains(lstBUs.Find(bu => bu.BU.Contains("2WP")).ID.ToString())).FindAll(v => v.Fund == null || v.Fund.Trim().Contains("2")));
        //                }
        //                reqList_forquery = reqList;
        //            }
        //            //CODE TO GET GROUPS OF COST ELEMENT
        //            IEnumerable<IGrouping<string, RequestItems_Table_TEST>> query = reqList_forquery.GroupBy(item => item.CostElement);

        //            decimal P_MAE_Totals = 0, P_NMAE_Totals = 0, P_SoftwareTotals = 0;
        //            decimal U_MAE_Totals = 0, U_NMAE_Totals = 0, U_SoftwareTotals = 0;
        //            //var danger1 = string.Empty;
        //            vkmSummary tempobj = new vkmSummary();
        //            tempobj.vkmyear = yr;


        //            //CODE TO GET THE TOTALS OF EACH COST ELEMENT
        //            foreach (IGrouping<string, RequestItems_Table_TEST> CostGroup in query)
        //            {

        //                // Iterate over each value in the
        //                // IGrouping and print the value.

        //                if (lstCostElement.Count != 0)
        //                {
        //                    if (lstCostElement.Find(cost => cost.ID.Equals(Int32.Parse(CostGroup.Key))).CostElement == "MAE")
        //                    {
        //                        foreach (RequestItems_Table_TEST item in CostGroup)
        //                        {
        //                            if (item.SHAppDate.ToString().Contains((int.Parse(yr) - 1).ToString()))  //yr is VKM Year; if yr == 2020 - 2021 Planning (2020 sh apprvd - 2021 planning)
        //                            {

        //                                P_MAE_Totals += (decimal)item.ApprCost;

        //                                U_MAE_Totals += item.OrderPrice == null ? (decimal)0.0 : (decimal)item.OrderPrice;


        //                            }
        //                        }
        //                    }
        //                    else if (lstCostElement.Find(cost => cost.ID.Equals(Int32.Parse(CostGroup.Key))).CostElement == "Non-MAE")
        //                    {
        //                        foreach (RequestItems_Table_TEST item in CostGroup)
        //                        {
        //                            if (item.SHAppDate.ToString().Contains((int.Parse(yr) - 1).ToString()))
        //                            {

        //                                P_NMAE_Totals += (decimal)item.ApprCost;
        //                                U_NMAE_Totals += item.OrderPrice == null ? (decimal)0.0 : (decimal)item.OrderPrice;
        //                            }
        //                        }

        //                    }
        //                    else if (lstCostElement.Find(cost => cost.ID.Equals(Int32.Parse(CostGroup.Key))).CostElement == "Software")
        //                    {
        //                        foreach (RequestItems_Table_TEST item in CostGroup)
        //                        {
        //                            if (item.SHAppDate.ToString().Contains((int.Parse(yr) - 1).ToString()))
        //                            {


        //                                P_SoftwareTotals += (decimal)item.ApprCost;
        //                                U_SoftwareTotals += item.OrderPrice == null ? (decimal)0.0 : (decimal)item.OrderPrice;
        //                            }
        //                        }

        //                    }
        //                }
        //                else
        //                    continue;
        //            }
        //            tempobj.P_MAE_Totals = P_MAE_Totals;
        //            OP_MAE_Totals += P_MAE_Totals;
        //            tempobj.P_NMAE_Totals = P_NMAE_Totals;
        //            OP_NMAE_Totals += P_NMAE_Totals;
        //            tempobj.P_Software_Totals = P_SoftwareTotals;
        //            OP_SoftwareTotals += P_SoftwareTotals;
        //            tempobj.P_Overall_Totals = P_MAE_Totals + P_NMAE_Totals + P_SoftwareTotals;

        //            tempobj.U_MAE_Totals = U_MAE_Totals;
        //            OU_MAE_Totals += U_MAE_Totals;
        //            tempobj.U_NMAE_Totals = U_NMAE_Totals;
        //            OU_NMAE_Totals += U_NMAE_Totals;
        //            tempobj.U_Software_Totals = U_SoftwareTotals;
        //            OU_SoftwareTotals += U_SoftwareTotals;
        //            tempobj.U_Overall_Totals = U_MAE_Totals + U_NMAE_Totals + U_SoftwareTotals;
        //            //if(buList.Contains("1") && buList.Contains("3") && yr == "2021")
        //            //{

        //            //        tempobj.U_MAE_Totals = db.Utilization_Table2021.ToList().Find(x=>x.CostElement == (lstCostElement.Find(ct => ct.CostElement.Contains("MAE")).ID)).Utilized2021;
        //            //        OU_MAE_Totals += U_MAE_Totals;
        //            //        tempobj.U_NMAE_Totals = db.Utilization_Table2021.ToList().Find(x => x.CostElement == (lstCostElement.Find(ct => ct.CostElement.Contains("Non-MAE")).ID)).Utilized2021;
        //            //        OU_NMAE_Totals += U_NMAE_Totals;
        //            //        tempobj.U_Software_Totals = db.Utilization_Table2021.ToList().Find(x => x.CostElement == (lstCostElement.Find(ct => ct.CostElement.Contains("Software")).ID)).Utilized2021;
        //            //        OU_SoftwareTotals += U_SoftwareTotals;
        //            //        tempobj.U_Overall_Totals = tempobj.U_MAE_Totals + tempobj.U_NMAE_Totals + tempobj.U_Software_Totals;

        //            //}
        //            viewList.Add(tempobj);

        //        }





        //        if (!(chart == true && years.Count() == 3)) //if years selected are 3, then no need to calculate change in values
        //        {
        //            Ch_P_MAE_Totals = (viewList.ElementAt(viewList.Count - 1).P_MAE_Totals) - (viewList.ElementAt(viewList.Count - 2).P_MAE_Totals);
        //            Ch_P_NMAE_Totals = (viewList.ElementAt(viewList.Count - 1).P_NMAE_Totals) - (viewList.ElementAt(viewList.Count - 2).P_NMAE_Totals);
        //            Ch_P_SoftwareTotals = (viewList.ElementAt(viewList.Count - 1).P_Software_Totals) - (viewList.ElementAt(viewList.Count - 2).P_Software_Totals);
        //            Ch_P_OverallTotals = (viewList.ElementAt(viewList.Count - 1).P_Overall_Totals) - (viewList.ElementAt(viewList.Count - 2).P_Overall_Totals);

        //            //var refTotals_forpercent_change = Int32.Parse(viewList[0].vkmyear) < Int32.Parse(viewList[1].vkmyear) ? viewList.ElementAt(viewList.Count - 2) : viewList.ElementAt(viewList.Count - 1);
        //            Pr_P_MAE_Totals = (viewList.ElementAt(viewList.Count - 2).P_MAE_Totals) != 0 ? (Ch_P_MAE_Totals / viewList.ElementAt(viewList.Count - 2).P_MAE_Totals) * 100 : 0;
        //            Pr_P_NMAE_Totals = (viewList.ElementAt(viewList.Count - 2).P_NMAE_Totals) != 0 ? (Ch_P_NMAE_Totals / viewList.ElementAt(viewList.Count - 2).P_NMAE_Totals) * 100 : 0;
        //            Pr_P_SoftwareTotals = (viewList.ElementAt(viewList.Count - 2).P_Software_Totals) != 0 ? (Ch_P_SoftwareTotals / viewList.ElementAt(viewList.Count - 2).P_Software_Totals) * 100 : 0;
        //            Pr_P_OverallTotals = (viewList.ElementAt(viewList.Count - 2).P_Overall_Totals) != 0 ? (Ch_P_OverallTotals / viewList.ElementAt(viewList.Count - 2).P_Overall_Totals) * 100 : 0;

        //            Ch_U_MAE_Totals = (viewList.ElementAt(viewList.Count - 1).U_MAE_Totals) - (viewList.ElementAt(viewList.Count - 2).U_MAE_Totals);
        //            Ch_U_NMAE_Totals = (viewList.ElementAt(viewList.Count - 1).U_NMAE_Totals) - (viewList.ElementAt(viewList.Count - 2).U_NMAE_Totals);
        //            Ch_U_SoftwareTotals = (viewList.ElementAt(viewList.Count - 1).U_Software_Totals) - (viewList.ElementAt(viewList.Count - 2).U_Software_Totals);
        //            Ch_U_OverallTotals = (viewList.ElementAt(viewList.Count - 1).U_Overall_Totals) - (viewList.ElementAt(viewList.Count - 2).U_Overall_Totals);
        //            Pr_U_MAE_Totals = (viewList.ElementAt(viewList.Count - 2).U_MAE_Totals) != 0 ? (Ch_U_MAE_Totals / viewList.ElementAt(viewList.Count - 2).U_MAE_Totals) * 100 : 0;
        //            Pr_U_NMAE_Totals = (viewList.ElementAt(viewList.Count - 2).U_NMAE_Totals) != 0 ? (Ch_U_NMAE_Totals / viewList.ElementAt(viewList.Count - 2).U_NMAE_Totals) * 100 : 0;
        //            Pr_U_SoftwareTotals = (viewList.ElementAt(viewList.Count - 2).U_Software_Totals) != 0 ? (Ch_U_SoftwareTotals / viewList.ElementAt(viewList.Count - 2).U_Software_Totals) * 100 : 0;
        //            Pr_U_OverallTotals = (viewList.ElementAt(viewList.Count - 2).U_Overall_Totals) != 0 ? (Ch_U_OverallTotals / viewList.ElementAt(viewList.Count - 2).U_Overall_Totals) * 100 : 0;
        //        }
        //        if (chart)
        //        {
        //            System.Data.DataTable dt = new System.Data.DataTable();
        //            dt.Columns.Add("Year", typeof(string));
        //            dt.Columns.Add("Planned", typeof(decimal));
        //            dt.Columns.Add("Utilized", typeof(decimal));
        //            dt.Columns.Add("Percentage_Utilization", typeof(decimal));

        //            //DataRow dr = dt.NewRow();

        //            int rcnt = 0;

        //            foreach (var info in viewList)
        //            {
        //                DataRow dr = dt.NewRow();
        //                //dr = dt.NewRow();
        //                dr[rcnt++] = "MAE" + " " + info.vkmyear;
        //                dr[rcnt++] = info.P_MAE_Totals;
        //                dr[rcnt++] = info.U_MAE_Totals;
        //                dr[rcnt++] = info.P_MAE_Totals != 0 ? (info.U_MAE_Totals / info.P_MAE_Totals) * 100 : 0;
        //                rcnt = 0;
        //                dt.Rows.Add(dr);
        //                dr = dt.NewRow();
        //                dr[rcnt++] = "NMAE" + " " + info.vkmyear;
        //                dr[rcnt++] = info.P_NMAE_Totals;
        //                dr[rcnt++] = info.U_NMAE_Totals;
        //                dr[rcnt++] = info.P_NMAE_Totals != 0 ? (info.U_NMAE_Totals / info.P_NMAE_Totals) * 100 : 0;
        //                rcnt = 0;
        //                dt.Rows.Add(dr);
        //                dr = dt.NewRow();
        //                dr[rcnt++] = "SW" + " " + info.vkmyear;
        //                dr[rcnt++] = info.P_Software_Totals;
        //                dr[rcnt++] = info.U_Software_Totals;
        //                dr[rcnt++] = info.P_Software_Totals != 0 ? (info.U_Software_Totals / info.P_Software_Totals) * 100 : 0;
        //                rcnt = 0;
        //                dt.Rows.Add(dr);
        //                dr = dt.NewRow();
        //                dr[rcnt++] = "Totals" + " " + info.vkmyear;
        //                dr[rcnt++] = info.P_Overall_Totals;
        //                dr[rcnt++] = info.U_Overall_Totals;
        //                dr[rcnt++] = info.P_Overall_Totals != 0 ? (info.U_Overall_Totals / info.P_Overall_Totals) * 100 : 0;


        //                //rcnt++; 
        //                dt.Rows.Add(dr);
        //                // dr = dt.NewRow();
        //                rcnt = 0;
        //            }
        //            //dt.Rows.Add(dr);
        //            for (int i = 0; i <= dt.Columns.Count - 1; i++)
        //            {
        //                _col.Add(new columnsinfo { title = dt.Columns[i].ColumnName, data = dt.Columns[i].ColumnName });
        //            }

        //            string col = (string)serializer.Serialize(_col);
        //            t.columns = col;


        //            var lst = dt.AsEnumerable()
        //            .Select(r => r.Table.Columns.Cast<DataColumn>()
        //                    .Select(c => new KeyValuePair<string, object>(c.ColumnName, r[c.Ordinal])
        //                   ).ToDictionary(z => z.Key, z => z.Value)
        //            ).ToList();

        //            string data = serializer.Serialize(lst);
        //            t.data = data;
        //        }
        //        else
        //        {
        //            System.Data.DataTable dt = new System.Data.DataTable();
        //            dt.Columns.Add("CostElement", typeof(string)); /// Planned + (year variable) / Utilized  + (year variable)
        //            //dt.Columns.Add("VKM 2020", typeof(string));
        //            string yrs = string.Empty;
        //            foreach (string yr in years)
        //            {

        //                dt.Columns.Add(yr + " Plan", typeof(string)); //add vkm text to yr
        //                dt.Columns.Add(yr + " Utilize", typeof(string));

        //            }

        //            dt.Columns.Add("Plan Change", typeof(string)); //"Plan(" + String.Join("-", years.ToArray()) + ") Change"
        //            dt.Columns.Add("Plan % Change ", typeof(string));
        //            dt.Columns.Add("Utilize Change", typeof(string));
        //            dt.Columns.Add("Utilize % Change", typeof(string));


        //            DataRow dr = dt.NewRow();
        //            dr[0] = "MAE";
        //            int rcnt = 1;
        //            int rcnt1 = 2; //2

        //            foreach (var info in viewList)
        //            {
        //                dr[rcnt] = info.P_MAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //                dr[rcnt1] = info.U_MAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //                //rcnt++;
        //                //rcnt = rcnt+ 2;
        //                //rcnt1 = rcnt1 + 2;

        //                if ((viewList.Count * 2 != rcnt1))
        //                {
        //                    rcnt = rcnt + 2;
        //                    rcnt1 = rcnt1 + 2;
        //                }
        //                else
        //                {
        //                    rcnt1 = rcnt1 + 1;
        //                }

        //            }
        //            dr[rcnt1] = Ch_P_MAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //            dr[rcnt1 + 1] = Math.Round(Pr_P_MAE_Totals, 2) + "%";
        //            dr[rcnt1 + 2] = Ch_U_MAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //            dr[rcnt1 + 3] = Math.Round(Pr_U_MAE_Totals, 2) + "%";
        //            dt.Rows.Add(dr);

        //            dr = dt.NewRow();
        //            dr[0] = "Non-MAE";
        //            int r1cnt = 1;
        //            int r1cnt1 = 2;
        //            foreach (var info in viewList)
        //            {
        //                dr[r1cnt] = info.P_NMAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //                dr[r1cnt1] = info.U_NMAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //                //rcnt++;
        //                //rcnt = rcnt+ 2;
        //                //rcnt1 = rcnt1 + 2;

        //                if ((viewList.Count * 2 != r1cnt1))
        //                {
        //                    r1cnt = r1cnt + 2;
        //                    r1cnt1 = r1cnt1 + 2;
        //                }
        //                else
        //                {
        //                    r1cnt1 = r1cnt1 + 1;
        //                }
        //            }
        //            dr[r1cnt1] = Ch_P_NMAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //            dr[r1cnt1 + 1] = Math.Round(Pr_P_NMAE_Totals, 2) + "%";
        //            dr[rcnt1 + 2] = Ch_U_NMAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //            dr[rcnt1 + 3] = Math.Round(Pr_U_NMAE_Totals, 2) + "%";
        //            dt.Rows.Add(dr);

        //            dr = dt.NewRow();
        //            dr[0] = "Software";
        //            int r2cnt = 1;
        //            int r2cnt1 = 2;
        //            foreach (var info in viewList)
        //            {
        //                dr[r2cnt] = info.P_Software_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //                dr[r2cnt1] = info.U_Software_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //                //rcnt++;
        //                //rcnt = rcnt+ 2;
        //                //rcnt1 = rcnt1 + 2;

        //                if ((viewList.Count * 2 != r2cnt1))
        //                {
        //                    r2cnt = r2cnt + 2;
        //                    r2cnt1 = r2cnt1 + 2;
        //                }
        //                else
        //                {
        //                    r2cnt1 = r2cnt1 + 1;
        //                }
        //            }
        //            dr[r2cnt1] = Ch_P_SoftwareTotals.ToString("C0", CultureInfo.CurrentCulture);
        //            dr[r2cnt1 + 1] = Math.Round(Pr_P_SoftwareTotals, 2) + "%";
        //            dr[rcnt1 + 2] = Ch_U_SoftwareTotals.ToString("C0", CultureInfo.CurrentCulture);
        //            dr[rcnt1 + 3] = Math.Round(Pr_U_SoftwareTotals, 2) + "%";
        //            dt.Rows.Add(dr);

        //            dr = dt.NewRow();
        //            dr[0] = "Totals";
        //            int r3cnt = 1;
        //            int r3cnt1 = 2;
        //            foreach (var info in viewList)
        //            {
        //                dr[r3cnt] = (info.P_MAE_Totals + info.P_NMAE_Totals + info.P_Software_Totals).ToString("C0", CultureInfo.CurrentCulture);
        //                dr[r3cnt1] = (info.U_MAE_Totals + info.U_NMAE_Totals + info.U_Software_Totals).ToString("C0", CultureInfo.CurrentCulture);
        //                //rcnt++;
        //                //rcnt = rcnt+ 2;
        //                //rcnt1 = rcnt1 + 2;

        //                if ((viewList.Count * 2 != r3cnt1))
        //                {
        //                    r3cnt = r3cnt + 2;
        //                    r3cnt1 = r3cnt1 + 2;
        //                }
        //                else
        //                {
        //                    r3cnt1 = r3cnt1 + 1;
        //                }
        //            }
        //            dr[r3cnt1] = Ch_P_OverallTotals.ToString("C0", CultureInfo.CurrentCulture);
        //            dr[r3cnt1 + 1] = Math.Round(Pr_P_OverallTotals, 2) + "%";
        //            dr[rcnt1 + 2] = Ch_U_OverallTotals.ToString("C0", CultureInfo.CurrentCulture);
        //            dr[rcnt1 + 3] = Math.Round(Pr_U_OverallTotals, 2) + "%";
        //            dt.Rows.Add(dr);
        //            for (int i = 0; i <= dt.Columns.Count - 1; i++)
        //            {
        //                _col.Add(new columnsinfo { title = dt.Columns[i].ColumnName, data = dt.Columns[i].ColumnName });
        //            }

        //            string col = (string)serializer.Serialize(_col);
        //            t.columns = col;


        //            var lst = dt.AsEnumerable()
        //            .Select(r => r.Table.Columns.Cast<DataColumn>()
        //                    .Select(c => new KeyValuePair<string, object>(c.ColumnName, r[c.Ordinal])
        //                   ).ToDictionary(z => z.Key, z => z.Value)
        //            ).ToList();

        //            string data = serializer.Serialize(lst);
        //            t.data = data;


        //        }

        //        //}
        //        //else
        //        //{

        //        //    t.data = string.Empty;
        //        //    t.columns = string.Empty;
        //        //    t.jsondata = string.Empty;


        //        //}

        //        return Json(new { data = t, message = selected_ccxc }, JsonRequestBehavior.AllowGet);

        //        // return View();
        //    }
        //}





        //public ActionResult CostElementDrillDown_comparison(List<string> years, string CostElement_Chosen, List<string> buList, bool chart = false, string selected_ccxc = "")
        //{

        //    if (lstUsers == null)
        //    {
        //        return RedirectToAction("Index", "Budgeting");
        //    }


        //    using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //    //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //    {
        //        List<RequestItems_Table_TEST> reqList = new List<RequestItems_Table_TEST>();
        //        List<RequestItems_Table_TEST> reqList_forquery = new List<RequestItems_Table_TEST>();
        //        List<CategoryBudget> categoryBudgets = new List<CategoryBudget>();
        //        List<CategoryBudget> Top70_CategoryBudget = new List<CategoryBudget>();
        //        List<costelement_drilldownSummary> viewList = new List<costelement_drilldownSummary>();
        //        string costelement_chosen_id = string.Empty;
        //        reqList = db.RequestItems_Table_TEST.ToList<RequestItems_Table_TEST>().FindAll(v => v.Fund == null || v.Fund.Trim().Contains("2"));
        //        int BaseYear_toCompare = 0;
        //        //List<RequestItemsList_2019> reqList2019 = new List<RequestItemsList_2019>();

        //        //Fetching the Category List
        //        foreach (var category in lstPrdCateg)
        //        {
        //            CategoryBudget catbud = new CategoryBudget();
        //            catbud.CategoryID = category.ID;
        //            catbud.CategoryName = category.Category;
        //            categoryBudgets.Add(catbud);
        //        }


        //        //CostElement_Chosen is costelt name -> fetch its id
        //        costelement_chosen_id = lstCostElement.Find(cost => cost.CostElement.ToUpper().Trim() == CostElement_Chosen.ToUpper().Trim()).ID.ToString().Trim();

        //        foreach (var bu_item in buList)
        //        {
        //            reqList_forquery.AddRange(reqList.FindAll(item => item.CostElement.Trim() == costelement_chosen_id).FindAll(ss => ss.BU.Contains(bu_item)));
        //        }

        //        //CODE TO GET GROUPS OF CATEGORY


        //        foreach (string yr in years)
        //        {
        //            BaseYear_toCompare++;
        //            //CC XC CHECK

        //            if ((int.Parse(yr) - 1) > 2020)  //for 2020 reqList_forquery has relevant data
        //            {
        //                string is_CCXC = string.Empty;
        //                bool CCXCflag = false;
        //                string presentUserDept = string.Empty;

        //                if (selected_ccxc.ToUpper().Contains("CC"))
        //                {
        //                    is_CCXC = "CC";
        //                    CCXCflag = true;
        //                }
        //                else if (selected_ccxc.ToUpper().Contains("XC"))
        //                {
        //                    is_CCXC = "XC";
        //                    CCXCflag = true;
        //                }


        //                else                    //DATA FILTER BASED ON USER'S NTID
        //                {
        //                    presentUserDept = lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).Department;

        //                    if (presentUserDept.ToUpper().Contains("CC"))
        //                    {
        //                        is_CCXC = "CC";
        //                        CCXCflag = true;
        //                    }
        //                    else if (presentUserDept.ToUpper().Contains("XC"))
        //                    {
        //                        is_CCXC = "XC";
        //                        CCXCflag = true;
        //                    }

        //                }

        //                if (CCXCflag)
        //                {
        //                    if (is_CCXC.Contains("XC"))
        //                    {
        //                        reqList_forquery.AddRange(reqList.FindAll(ss => ss.BU.Contains(lstBUs.Find(bu => bu.BU.Contains("2WP")).ID.ToString())));
        //                    }
        //                    reqList_forquery = reqList_forquery.FindAll(dpt => lstDEPTs.Find(x => x.ID.ToString().Contains(dpt.DEPT)).DEPT.Contains(is_CCXC))/*.FindAll(ss => ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("AS")).ID.ToString()) || ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("2WP")).ID.ToString()))*/;
        //                    CCXCflag = false;
        //                }



        //            }


        //            decimal[] P_Category_Totals = new decimal[categoryBudgets.Count()];
        //            decimal[] U_Category_Totals = new decimal[categoryBudgets.Count()];
        //            costelement_drilldownSummary tempobj = new costelement_drilldownSummary();
        //            tempobj.vkmyear = yr;


        //            //CODE TO GET THE TOTALS OF Top Category in 2020 Year - to compare with the other years Totals
        //            if (lstPrdCateg.Count != 0)
        //            {

        //                foreach (var catbudget in categoryBudgets)
        //                {
        //                    foreach (RequestItems_Table_TEST item in reqList_forquery)
        //                    {
        //                        if (item.SHAppDate.ToString().Contains((int.Parse(yr) - 1).ToString()))   // 2020 sh apprvd - 2021 planning
        //                        {
        //                            // if ((int.Parse(yr) - 1).ToString() == "2020")
        //                            if (BaseYear_toCompare == 1 | (BaseYear_toCompare == 2 && Top70_CategoryBudget.Count() == 0))
        //                            {

        //                                if (int.Parse(item.Category) == catbudget.CategoryID)
        //                                {


        //                                    if (item.ApprCost != null)
        //                                    {

        //                                        catbudget.CategoryTotals += (decimal)item.ApprCost;
        //                                    }


        //                                }

        //                            }



        //                        }

        //                    }
        //                }
        //                Top70_CategoryBudget = categoryBudgets.OrderByDescending(x => x.CategoryTotals).Take(70).ToList();

        //            }

        //            //Code to get the Planned and Utilized Category Totals
        //            int catcount = -1;
        //            foreach (var topcat in Top70_CategoryBudget)
        //            {
        //                catcount++;
        //                foreach (RequestItems_Table_TEST item in reqList_forquery)
        //                {
        //                    if (item.SHAppDate.ToString().Contains((int.Parse(yr) - 1).ToString()))
        //                    {
        //                        if (int.Parse(item.Category) == topcat.CategoryID)
        //                        {
        //                            P_Category_Totals[catcount] += (decimal)item.ApprCost;
        //                            U_Category_Totals[catcount] += item.OrderPrice == null ? (decimal)0.0 : (decimal)item.OrderPrice;

        //                        }
        //                    }
        //                }
        //            }

        //            tempobj.P_Category_Totals = P_Category_Totals;
        //            tempobj.U_Category_Totals = U_Category_Totals;

        //            viewList.Add(tempobj);

        //        }




        //        ///NEW CODE TO GET CATEGORY SUMMARY
        //        var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
        //        BudgetParam t = new BudgetParam();
        //        List<columnsinfo> _col = new List<columnsinfo>();
        //        System.Data.DataTable dt = new System.Data.DataTable();
        //        dt.Columns.Add("Category Name", typeof(string)); /// Planned + (year variable) / Utilized  + (year variable)
        //        //dt.Columns.Add("VKM 2020", typeof(string));
        //        foreach (string year in years)
        //        {
        //            dt.Columns.Add("VKM" + " " + year + "(Planned)", typeof(string)); //add vkm text to yr
        //            dt.Columns.Add("VKM" + " " + year + "(Utilized)", typeof(string));

        //        }


        //        //DataRow dr = dt.NewRow();
        //        //dr[0] = "MAE";
        //        //int rcnt = 1;
        //        //int rcnt1 = 2; //2
        //        for (var i = 0; i < Top70_CategoryBudget.Count(); i++)
        //        {
        //            DataRow dr = dt.NewRow();
        //            dr[0] = Top70_CategoryBudget[i].CategoryName.Trim().Replace("\r\n", "").Replace("\r", "").Replace("\r", "");
        //            int rcnt = 1;
        //            int rcnt1 = 2;
        //            foreach (var info in viewList)
        //            {



        //                dr[rcnt] = info.P_Category_Totals[i].ToString("C", CultureInfo.CurrentCulture);
        //                dr[rcnt1] = info.U_Category_Totals[i];
        //                //rcnt++;
        //                rcnt = rcnt + 2;
        //                rcnt1 = rcnt1 + 2;

        //            }
        //            dt.Rows.Add(dr);

        //        }
        //        //dt.Rows.Add(dr);
        //        // dt.Rows.Add(dr);

        //        //dr = dt.NewRow();
        //        // dr[0] = "Non-MAE";


        //        // dt.Rows.Add(dr);
        //        for (int i = 0; i <= dt.Columns.Count - 1; i++)
        //        {
        //            _col.Add(new columnsinfo { title = dt.Columns[i].ColumnName, data = dt.Columns[i].ColumnName });
        //        }

        //        string col = (string)serializer.Serialize(_col);
        //        t.columns = col;


        //        var lst = dt.AsEnumerable()
        //        .Select(r => r.Table.Columns.Cast<DataColumn>()
        //                .Select(c => new KeyValuePair<string, object>(c.ColumnName, r[c.Ordinal])
        //               ).ToDictionary(z => z.Key, z => z.Value)
        //        ).ToList();

        //        string data = serializer.Serialize(lst);
        //        t.data = data;




        //        return Json(new { data = t, message = selected_ccxc }, JsonRequestBehavior.AllowGet);

        //        // return View();

        //    }
        //}





        /////// <summary>
        /////// Function to send BU summary data to view
        /////// </summary>
        /////// <returns></returns> 
        ////public ActionResult CostElementDrillDown(string Year_isPO, string CostElement_Chosen, List<string> buList, string selected_ccxc = "")
        ////{


        ////    using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        ////    //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        ////    {
        ////        List<CategoryBudget> categoryBudgets = new List<CategoryBudget>();
        ////        List<RequestItems_Table_TEST> reqList = new List<RequestItems_Table_TEST>();


        ////        foreach (var category in lstPrdCateg)
        ////        {
        ////            CategoryBudget catbud = new CategoryBudget();
        ////            catbud.CategoryID = category.ID;
        ////            catbud.CategoryName = category.Category;
        ////            categoryBudgets.Add(catbud);
        ////        }
        ////        //categoryBudgets will comprise of all the Categories - ID and Name
        ////        //Year_isPO - 
        ////        string YearX = Year_isPO.Split('(')[0];
        ////        int Year = int.Parse(YearX.Substring(4));
        ////        var is_PO = Year_isPO.Split('(')[1];
        ////        decimal OverallTotals = 0;
        ////        string costelement_chosen_id = string.Empty;
        ////        List<CategoryBudget> Whole_CategoryBudget = new List<CategoryBudget>();
        ////        List<CategoryBudget> Top10_CategoryBudget = new List<CategoryBudget>();


        ////        //if CostElement_Chosen is costelt name -> fetch its id
        ////        costelement_chosen_id = lstCostElement.Find(cost => cost.CostElement.ToUpper().Trim() == CostElement_Chosen.ToUpper().Trim()).ID.ToString().Trim();
        ////        //else costelement_chosen_id = CostElement_Chosen

        ////        foreach (var bu_item in buList)
        ////        {
        ////            //reqList_forquery ;
        ////            reqList.AddRange(db.RequestItems_Table_TEST.Where(item => item.CostElement.Trim() == costelement_chosen_id).ToList().FindAll(ss => ss.BU.Contains(bu_item)/*(lstBUs.Find(bu => bu.BU.Trim().Equals(bu_item.Trim())).ID.ToString())*/));
        ////        }


        ////            if (Year != 2020 && !(Year == 2021 && is_PO.Contains("Planned")))  //for 2020 reqList_forquery has relevant data 2021planned - no check 2021utili - check 2022plan check
        ////            {
        ////                string is_CCXC = string.Empty;
        ////                bool CCXCflag = false;
        ////                string presentUserDept = string.Empty;
        ////                if (selected_ccxc != "") //DATA FILTER BASED ON CCXC DROPDOWM - NOT USER'S NTID
        ////                {
        ////                    if (selected_ccxc.ToUpper().Contains("CC"))
        ////                    {
        ////                        is_CCXC = "CC";
        ////                        CCXCflag = true;
        ////                    }
        ////                    else if (selected_ccxc.ToUpper().Contains("XC"))
        ////                    {
        ////                        is_CCXC = "XC";
        ////                        CCXCflag = true;
        ////                    }

        ////                }
        ////                else                    //DATA FILTER BASED ON USER'S NTID
        ////                {
        ////                    presentUserDept = lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).Department;

        ////                    if (presentUserDept.ToUpper().Contains("CC"))
        ////                    {
        ////                        is_CCXC = "CC";
        ////                        CCXCflag = true;
        ////                    }
        ////                    else if (presentUserDept.ToUpper().Contains("XC"))
        ////                    {
        ////                        is_CCXC = "XC";
        ////                        CCXCflag = true;
        ////                    }

        ////                }

        ////                if (CCXCflag)
        ////                {
        ////                    if (is_CCXC.Contains("XC"))
        ////                    {
        ////                        reqList.AddRange(reqList.FindAll(ss => ss.BU.Contains(lstBUs.Find(bu => bu.BU.Contains("2WP")).ID.ToString())));
        ////                    }
        ////                    reqList = reqList.FindAll(dpt => lstDEPTs.Find(x => x.ID.ToString().Contains(dpt.DEPT)).DEPT.Contains(is_CCXC))/*.FindAll(ss => ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("AS")).ID.ToString()) || ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("2WP")).ID.ToString()))*/;
        ////                    CCXCflag = false;
        ////                }



        ////            }
        ////        if (is_PO.Contains("Planned"))
        ////        {
        ////            //use dhappdate == Year-1 check (since apprvd cost includes dh & sh)
        ////            //get all reqlist items with chosen cost elt & DHApprvDate = Year-1
        ////            reqList = reqList.Where(item=>item.ApprovalDH == true && item.DHAppDate != null).Where(item => item.DHAppDate.Value.Year == (Year-1)).OrderBy(item => item.Category).ToList()/*.FindAll(ss => ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("AS")).ID.ToString()) || ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("2WP")).ID.ToString()))*/;
        ////            foreach (var catbudget in categoryBudgets)
        ////            {
        ////                foreach (var item in reqList)
        ////                {
        ////                    if (int.Parse(item.Category) == catbudget.CategoryID)
        ////                    {
        ////                        if (item.ApprCost != null)
        ////                            catbudget.CategoryTotals += (decimal)item.ApprCost;
        ////                        OverallTotals += (decimal)item.ApprCost;
        ////                    }
        ////                }
        ////            }

        ////            Whole_CategoryBudget = categoryBudgets.OrderByDescending(x => x.CategoryTotals).ToList();
        ////            Top10_CategoryBudget = categoryBudgets.Take(10).ToList(); //check these categories in utilized

        ////        }
        ////        if (is_PO.Contains("Utilized"))
        ////        {
        ////            //use orderdate == Year check
        ////            //get all reqlist items with chosen cost elt & OrderDate = Year
        ////            reqList = reqList.Where(x=>x.OrderDate != null).Where(item => item.OrderDate.Value.Year == Year).OrderBy(item => item.Category).ToList();
        ////            foreach (var catbud in categoryBudgets)
        ////            {
        ////                foreach (var item in reqList)
        ////                {
        ////                    if (int.Parse(item.Category) == catbud.CategoryID)
        ////                    {
        ////                        if (item.OrderPrice != null)
        ////                            catbud.CategoryTotals += (decimal)item.OrderPrice;
        ////                    }
        ////                }
        ////            }

        ////            Whole_CategoryBudget = categoryBudgets.OrderByDescending(x => x.CategoryTotals).ToList();
        ////            Top10_CategoryBudget = categoryBudgets.Take(10).ToList();


        ////        }




        ////        return Json(new { data = Whole_CategoryBudget, success = true, message = selected_ccxc }, JsonRequestBehavior.AllowGet);


        ////    }
        ////}


        //public ActionResult CategoryDrillDown_comparison(List<string> years, string CostElement_Chosen, string Category_Chosen, List<string> buList, bool chart = false, string selected_ccxc = "")
        //{

        //    if (lstUsers == null)
        //    {
        //        return RedirectToAction("Index", "Budgeting");
        //    }


        //    using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //    //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //    {
        //        List<RequestItems_Table_TEST> reqList = new List<RequestItems_Table_TEST>();
        //        List<RequestItems_Table_TEST> reqList_forquery = new List<RequestItems_Table_TEST>();
        //        reqList = db.RequestItems_Table_TEST.ToList<RequestItems_Table_TEST>().FindAll(v => v.Fund == null || v.Fund.Trim().Contains("2"));
        //        List<RequestItemsList_2019> reqList2019 = new List<RequestItemsList_2019>();
        //        List<ItemBudget> itemBudgets = new List<ItemBudget>();
        //        List<ItemBudget> Top_ItemBudget = new List<ItemBudget>();
        //        List<category_drilldownSummary> viewList = new List<category_drilldownSummary>();
        //        string costelement_chosen_id = string.Empty;
        //        int BaseYear_toCompare = 0;

        //        //CostElement_Chosen is costelt name -> fetch its id
        //        costelement_chosen_id = lstCostElement.Find(cost => cost.CostElement.ToUpper().Trim() == CostElement_Chosen.ToUpper().Trim()).ID.ToString().Trim();

        //        //Category_Chosen is costelt name -> fetch its id
        //        Category_Chosen = lstPrdCateg.Find(cat => cat.Category.ToUpper().Trim() == Category_Chosen.ToUpper().Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", "")).ID.ToString().Trim();

        //        foreach (var item in lstItems.Where(x => x.Category.Contains(Category_Chosen.ToString().Trim())))
        //        {
        //            ItemBudget ibud = new ItemBudget();
        //            ibud.ItemID = item.S_No;
        //            ibud.ItemName = item.Item_Name;


        //            if (itemBudgets.Count > 0 && itemBudgets.FindAll(ib => ib.ItemName.Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", "") == (ibud.ItemName.Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", ""))).Count != 0)
        //                continue;

        //            itemBudgets.Add(ibud);
        //        }
        //        foreach (var bu_item in buList)
        //        {

        //            reqList_forquery.AddRange(reqList.FindAll(item => item.CostElement.Trim() == costelement_chosen_id).FindAll(ss => ss.BU.Contains(bu_item)).FindAll(item => item.Category.Trim() == Category_Chosen.ToString().Trim()));

        //        }

        //        //CODE TO GET GROUPS OF COST ELEMENT


        //        foreach (string yr in years)
        //        {
        //            BaseYear_toCompare++;
        //            //CC XC CHECK
        //            //if(yr == "2020")
        //            //{
        //            //    reqList_forquery = reqList_forquery/*.FindAll(ss => ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("AS")).ID.ToString()) || ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("2WP")).ID.ToString()))*/;
        //            //}
        //            //else
        //            if ((int.Parse(yr) - 1) > 2020)  //for 2020 reqList_forquery has relevant data
        //            {
        //                string is_CCXC = string.Empty;
        //                bool CCXCflag = false;
        //                string presentUserDept = string.Empty;

        //                if (selected_ccxc.ToUpper().Contains("CC"))
        //                {
        //                    is_CCXC = "CC";
        //                    CCXCflag = true;
        //                }
        //                else if (selected_ccxc.ToUpper().Contains("XC"))
        //                {
        //                    is_CCXC = "XC";
        //                    CCXCflag = true;
        //                }

        //                else                    //DATA FILTER BASED ON USER'S NTID
        //                {
        //                    presentUserDept = lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).Department;

        //                    if (presentUserDept.ToUpper().Contains("CC"))
        //                    {
        //                        is_CCXC = "CC";
        //                        CCXCflag = true;
        //                    }
        //                    else if (presentUserDept.ToUpper().Contains("XC"))
        //                    {
        //                        is_CCXC = "XC";
        //                        CCXCflag = true;
        //                    }

        //                }

        //                if (CCXCflag)
        //                {
        //                    if (is_CCXC.Contains("XC"))
        //                    {
        //                        reqList_forquery.AddRange(reqList.FindAll(ss => ss.BU.Contains(lstBUs.Find(bu => bu.BU.Contains("2WP")).ID.ToString())));
        //                    }
        //                    reqList_forquery = reqList_forquery.FindAll(dpt => lstDEPTs.Find(x => x.ID.ToString().Contains(dpt.DEPT)).DEPT.Contains(is_CCXC))/*.FindAll(ss => ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("AS")).ID.ToString()) || ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("2WP")).ID.ToString()))*/;
        //                    CCXCflag = false;
        //                }



        //            }


        //            // decimal Category_Totals[10] = new decimal[10];
        //            decimal[] P_Item_Totals = new decimal[200];
        //            decimal[] U_Item_Totals = new decimal[200];
        //            category_drilldownSummary tempobj = new category_drilldownSummary();
        //            tempobj.vkmyear = yr;


        //            foreach (var itembudget in itemBudgets)
        //            {
        //                foreach (RequestItems_Table_TEST item in reqList_forquery)
        //                {
        //                    if (item.SHAppDate.ToString().Contains((int.Parse(yr) - 1).ToString()))   // 2020 dh apprvd - 2021 planning
        //                    {
        //                        //if ((int.Parse(yr) - 1).ToString() == "2020")
        //                        if (BaseYear_toCompare == 1 || (BaseYear_toCompare == 2 && Top_ItemBudget.Count() == 0))
        //                        {

        //                            if (lstItems.Find(x => x.S_No == int.Parse(item.ItemName)).Item_Name.Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", "") == itembudget.ItemName.Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", ""))
        //                            // if (int.Parse(item.ItemName) == itembudget.ItemID)
        //                            {
        //                                if (item.ApprCost != null)
        //                                    itembudget.ItemTotals += (decimal)item.ApprCost;

        //                            }

        //                        }

        //                    }

        //                }
        //            }
        //            Top_ItemBudget = itemBudgets.OrderByDescending(x => x.ItemTotals).Take(200).ToList();


        //            int itemcount = -1;
        //            foreach (var topitem in Top_ItemBudget)
        //            {
        //                itemcount++;
        //                foreach (RequestItems_Table_TEST item in reqList_forquery)
        //                {
        //                    if (item.SHAppDate.ToString().Contains((int.Parse(yr) - 1).ToString()))
        //                    {
        //                        if (lstItems.Find(x => x.S_No == int.Parse(item.ItemName)).Item_Name.Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", "") == topitem.ItemName.Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", ""))
        //                        //if (int.Parse(item.ItemName) == topitem.ItemID)
        //                        {
        //                            P_Item_Totals[itemcount] += (decimal)item.ApprCost;
        //                            U_Item_Totals[itemcount] += item.OrderPrice == null ? (decimal)0.0 : (decimal)item.OrderPrice;

        //                        }


        //                    }
        //                }
        //            }


        //            tempobj.P_Item_Totals = P_Item_Totals;
        //            tempobj.U_Item_Totals = U_Item_Totals;

        //            viewList.Add(tempobj);

        //        }


        //        var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
        //        BudgetParam t = new BudgetParam();
        //        List<columnsinfo> _col = new List<columnsinfo>();
        //        System.Data.DataTable dt = new System.Data.DataTable();
        //        dt.Columns.Add("Item Name", typeof(string)); /// Planned + (year variable) / Utilized  + (year variable)
        //        //dt.Columns.Add("VKM 2020", typeof(string));
        //        foreach (string year in years)
        //        {
        //            dt.Columns.Add("VKM" + " " + year + "(Planned)", typeof(string)); //add vkm text to yr
        //            dt.Columns.Add("VKM" + " " + year + "(Utilized)", typeof(string));

        //        }


        //        //DataRow dr = dt.NewRow();
        //        //dr[0] = "MAE";
        //        //int rcnt = 1;
        //        //int rcnt1 = 2; //2
        //        for (var i = 0; i < Top_ItemBudget.Count(); i++)
        //        {
        //            DataRow dr = dt.NewRow();
        //            dr[0] = Top_ItemBudget[i].ItemName.Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", "");
        //            int rcnt = 1;
        //            int rcnt1 = 2;
        //            foreach (var info in viewList)
        //            {



        //                dr[rcnt] = info.P_Item_Totals[i].ToString("C", CultureInfo.CurrentCulture);
        //                dr[rcnt1] = info.U_Item_Totals[i];
        //                //rcnt++;
        //                rcnt = rcnt + 2;
        //                rcnt1 = rcnt1 + 2;

        //            }
        //            dt.Rows.Add(dr);

        //        }
        //        //dt.Rows.Add(dr);
        //        // dt.Rows.Add(dr);

        //        //dr = dt.NewRow(); 
        //        // dr[0] = "Non-MAE";


        //        // dt.Rows.Add(dr);
        //        for (int i = 0; i <= dt.Columns.Count - 1; i++)
        //        {
        //            _col.Add(new columnsinfo { title = dt.Columns[i].ColumnName, data = dt.Columns[i].ColumnName });
        //        }

        //        string col = (string)serializer.Serialize(_col);
        //        t.columns = col;


        //        var lst = dt.AsEnumerable()
        //        .Select(r => r.Table.Columns.Cast<DataColumn>()
        //                .Select(c => new KeyValuePair<string, object>(c.ColumnName, r[c.Ordinal])
        //               ).ToDictionary(z => z.Key, z => z.Value)
        //        ).ToList();

        //        string data = serializer.Serialize(lst);
        //        t.data = data;




        //        return Json(new { data = t, message = selected_ccxc }, JsonRequestBehavior.AllowGet);

        //    }
        //}




        /////// <summary>
        /////// Function to send BU summary data to view
        /////// </summary>
        /////// <returns></returns> 
        ////public ActionResult CategoryDrillDown(string Year_isPO, string CostElement_Chosen, int Category_Chosen, List<string> buList, string selected_ccxc = "")
        ////{


        ////    using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        ////    //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        ////    {
        ////        List<ItemBudget> itemBudgets = new List<ItemBudget>();
        ////        List<RequestItems_Table_TEST> reqList = new List<RequestItems_Table_TEST>();


        ////        foreach (var item in lstItems.Where(x => x.Category.Contains(Category_Chosen.ToString().Trim())))
        ////        {
        ////            ItemBudget ibud = new ItemBudget();
        ////            ibud.ItemID = item.S_No;
        ////            ibud.ItemName = item.Item_Name;
        ////            itemBudgets.Add(ibud);
        ////        }
        ////        //categoryBudgets will comprise of all the Categories - ID and Name
        ////        //Year_isPO - 
        ////        string YearX = Year_isPO.Split('(')[0];
        ////        int Year = int.Parse(YearX.Substring(4));
        ////        var is_PO = Year_isPO.Split('(')[1];
        ////        decimal OverallItemTotals = 0;
        ////        string costelement_chosen_id = string.Empty;
        ////        List<ItemBudget> Whole_ItemBudget = new List<ItemBudget>();
        ////        List<ItemBudget> Top10_ItemBudget = new List<ItemBudget>();


        ////        costelement_chosen_id = lstCostElement.Find(cost => cost.CostElement.ToUpper().Trim() == CostElement_Chosen.ToUpper().Trim()).ID.ToString().Trim();

        ////        foreach (var bu_item in buList)
        ////        {
        ////            //reqList_forquery ;
        ////            reqList.AddRange(db.RequestItems_Table_TEST.Where(item => item.CostElement.Trim() == costelement_chosen_id).ToList().FindAll(ss => ss.BU.Contains(bu_item)).Where(item => item.Category.Trim() == Category_Chosen.ToString().Trim()));
        ////        }

        ////        var zzz = Year != 2020;
        ////        var aaa = !(Year == 2021 && is_PO.Contains("Planned"));
        ////        var xxx = Year != 2020 && !(Year == 2021 && is_PO.Contains("Planned"));
        ////        if (Year != 2020 && !(Year == 2021 && is_PO.Contains("Planned")))   //for 2020 reqList_forquery has relevant data
        ////        {
        ////            string is_CCXC = string.Empty;
        ////            bool CCXCflag = false;
        ////            string presentUserDept = string.Empty;
        ////            if (selected_ccxc != "") //DATA FILTER BASED ON CCXC DROPDOWM - NOT USER'S NTID
        ////            {
        ////                if (selected_ccxc.ToUpper().Contains("CC"))
        ////                {
        ////                    is_CCXC = "CC";
        ////                    CCXCflag = true;
        ////                }
        ////                else if (selected_ccxc.ToUpper().Contains("XC"))
        ////                {
        ////                    is_CCXC = "XC";
        ////                    CCXCflag = true;
        ////                }

        ////            }
        ////            else                    //DATA FILTER BASED ON USER'S NTID
        ////            {
        ////                presentUserDept = lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).Department;

        ////                if (presentUserDept.ToUpper().Contains("CC"))
        ////                {
        ////                    is_CCXC = "CC";
        ////                    CCXCflag = true;
        ////                }
        ////                else if (presentUserDept.ToUpper().Contains("XC"))
        ////                {
        ////                    is_CCXC = "XC";
        ////                    CCXCflag = true;
        ////                }

        ////            }

        ////            if (CCXCflag)
        ////            {
        ////                if (is_CCXC.Contains("XC"))
        ////                {
        ////                    reqList.AddRange(reqList.FindAll(ss => ss.BU.Contains(lstBUs.Find(bu => bu.BU.Contains("2WP")).ID.ToString())));
        ////                }
        ////                reqList = reqList.FindAll(dpt => lstDEPTs.Find(x => x.ID.ToString().Contains(dpt.DEPT)).DEPT.Contains(is_CCXC))/*.FindAll(ss => ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("AS")).ID.ToString()) || ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("2WP")).ID.ToString()))*/;
        ////                CCXCflag = false;
        ////            }



        ////        }

        ////        if (is_PO.Contains("Planned"))
        ////        {
        ////            //use dhappdate == Year-1 check (since apprvd cost includes dh & sh)
        ////            //get all reqlist items with chosen cost elt & DHApprvDate = Year-1
        ////            reqList = reqList.Where(x => x.DHAppDate != null).Where(item => item.DHAppDate.Value.Year == (Year-1)).OrderBy(item => item.ItemName).ToList()/*.FindAll(ss => ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("AS")).ID.ToString()) || ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("2WP")).ID.ToString()))*/;
        ////            foreach (var itembudget in itemBudgets)
        ////            {
        ////                foreach (var item in reqList)
        ////                {
        ////                    if (int.Parse(item.ItemName) == itembudget.ItemID)
        ////                    {
        ////                        if (item.ApprCost != null)
        ////                            itembudget.ItemTotals += (decimal)item.ApprCost;
        ////                        OverallItemTotals += (decimal)item.ApprCost;
        ////                    }
        ////                }
        ////            }

        ////            Whole_ItemBudget = itemBudgets.OrderByDescending(x => x.ItemTotals).ToList();
        ////            Top10_ItemBudget = itemBudgets.Take(10).ToList();

        ////        }
        ////        if (is_PO.Contains("Utilized"))
        ////        {
        ////            //use orderdate == Year check
        ////            //get all reqlist items with chosen cost elt & OrderDate = Year
        ////            reqList = reqList.Where(x => x.OrderDate != null).Where(item => item.OrderDate.Value.Year == Year).OrderBy(item => item.Category).ToList();
        ////            foreach (var catbud in itemBudgets)
        ////            {
        ////                foreach (var item in reqList)
        ////                {
        ////                    if (int.Parse(item.ItemName) == catbud.ItemID)
        ////                    {
        ////                        if (item.OrderPrice != null)
        ////                            catbud.ItemTotals += (decimal)item.OrderPrice;
        ////                    }
        ////                }
        ////            }

        ////            Whole_ItemBudget = itemBudgets.OrderByDescending(x => x.ItemTotals).ToList();
        ////            Top10_ItemBudget = itemBudgets.Take(10).ToList();


        ////        }




        ////        return Json(new { data = Whole_ItemBudget, success = true, message = selected_ccxc }, JsonRequestBehavior.AllowGet);


        ////    }
        ////}


        //public ActionResult BUs_forpresentNTID_CCXC(bool ccxc = false, string selected_ccxc = "")
        //{
        //    string presentNTID = User.Identity.Name.Split('\\')[1].ToUpper();
        //    string presentUserDept = string.Empty;
        //    List<string> allowedBUs = new List<string>();
        //    List<string> cc = new List<string>()
        //        { "MB", "2WP", "OSS"};
        //    List<string> xc = new List<string>()
        //        { "DA", "AD"};

        //    if (ccxc == true)
        //    {
        //        if (selected_ccxc.ToUpper().Trim().Contains("CC"))
        //        {
        //            foreach (var bu_cc in cc)
        //            {
        //                allowedBUs.Add(lstBUs.Find(bu => bu.BU.Contains(bu_cc)).ID.ToString());
        //            }
        //        }
        //        else if (selected_ccxc.ToUpper().Trim().Contains("XC"))
        //        {
        //            foreach (var bu_xc in xc)
        //            {
        //                allowedBUs.Add(lstBUs.Find(bu => bu.BU.Contains(bu_xc)).ID.ToString());
        //            }
        //        }
        //        else
        //        {

        //            if (lstBU_SPOCs.Find(person => person.VKMspoc.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())) != null)
        //            {
        //                var VKMSPOC = lstBU_SPOCs.FindAll(e => e.VKMspoc.ToUpper().Trim().Equals(User.Identity.Name.Split('\\')[1].ToUpper()));
        //                foreach (var i in VKMSPOC)
        //                {
        //                    allowedBUs.Add(i.BU.ToString());
        //                }



        //            }
        //            else if (lstPrivileged.Find(person => person.ADSID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())) != null)
        //            {
        //                var BU_of_VKMSPOC = lstPrivileged.Find(e => e.ADSID.ToUpper().Trim().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).BU;
        //                if (BU_of_VKMSPOC != null)
        //                {
        //                    allowedBUs = (BU_of_VKMSPOC.Split(',')).ToList();

        //                }
        //            }
        //            else
        //            {
        //                presentUserDept = lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).Department;

        //                if (presentUserDept.ToUpper().Contains("CC"))
        //                {
        //                    foreach (var bu_cc in cc)
        //                    {
        //                        allowedBUs.Add(lstBUs.Find(bu => bu.BU.Contains(bu_cc)).ID.ToString());
        //                    }
        //                }
        //                else if (presentUserDept.ToUpper().Contains("XC"))
        //                {
        //                    foreach (var bu_xc in xc)
        //                    {
        //                        allowedBUs.Add(lstBUs.Find(bu => bu.BU.Contains(bu_xc)).ID.ToString());
        //                    }
        //                }
        //            }
        //            //else
        //            //{
        //            //    if (presentNTID == "MAE9COB")
        //            //    {
        //            //        allowedBUs.Add("1");
        //            //        allowedBUs.Add("3");
        //            //    }
        //            //    else
        //            //    {
        //            //        return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        //            //    }
        //            //}


        //        }
        //    }

        //    return Json(new { data = allowedBUs, message = selected_ccxc, success = true }, JsonRequestBehavior.AllowGet);

        //}


        //[HttpGet]
        //public ActionResult ItemID(string Item_Chosen)
        //{

        //    return Json(new { data = lstItems.Find(x => x.Item_Name.Trim().ToUpper() == Item_Chosen.Trim().ToUpper()).S_No }, JsonRequestBehavior.AllowGet);

        //}

        ////[HttpGet]
        ////public ActionResult CategoryName(int Category_Chosen)
        ////{

        ////    return Json(new { data = lstPrdCateg.Find(x => x.ID == Category_Chosen).Category }, JsonRequestBehavior.AllowGet);

        ////}


        ////public ActionResult ItemDrillDown_Index()
        ////{
        ////    return View();
        ////}

        ////[HttpPost]
        ////public ActionResult ItemDrillDown( /*string Years_Listasstring,*/ string CostElement_Chosen, int Category_Chosen, int Item_Chosen, List<string> buList, List<string> years,/*string BuList_Listasstring,*/ string selected_ccxc = "")
        ////{

        ////    //List<string> years = new List<string>();
        ////    //List<string> buList = new List<string>();
        ////    string costelement_chosen_id = string.Empty;

        ////    //RequestListAttributes viewList = new 


        ////    List<RequestItemsRepoView> viewList_itemdrilldown1 = new List<RequestItemsRepoView>();
        ////    //if (Years_Listasstring != null)
        ////    //{
        ////    //    years = (Years_Listasstring.Split(',')).ToList();

        ////    //}
        ////    //if (BuList_Listasstring != null)
        ////    //{
        ////    //    buList = (BuList_Listasstring.Split(',')).ToList();

        ////    //}
        ////    //if CostElement_Chosen is costelt name -> fetch its id
        ////    costelement_chosen_id = lstCostElement.Find(cost => cost.CostElement.ToUpper().Trim() == CostElement_Chosen.ToUpper().Trim()).ID.ToString().Trim();


        ////    using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        ////    //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        ////    {
        ////        List<RequestItems_Table_TEST> reqList = new List<RequestItems_Table_TEST>();



        ////        foreach (var bu_item in buList)
        ////        {
        ////            //reqList_forquery ;
        ////            reqList.AddRange(db.RequestItems_Table_TEST.ToList<RequestItems_Table_TEST>().FindAll(item => item.CostElement.Trim() == costelement_chosen_id).FindAll(ss => ss.BU.Contains(bu_item)).FindAll(item => item.Category.Trim() == Category_Chosen.ToString().Trim()).FindAll(z => z.ItemName.Contains(Item_Chosen.ToString())));

        ////        }

        ////        foreach (string yr in years)
        ////        {
        ////            reqList = reqList.FindAll(z => z.SubmitDate.Value.Year.ToString() == yr.Trim());
        ////            //CC XC CHECK
        ////            //if(yr == "2020")
        ////            //{
        ////            //    reqList_forquery = reqList_forquery/*.FindAll(ss => ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("AS")).ID.ToString()) || ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("2WP")).ID.ToString()))*/;
        ////            //}
        ////            //else
        ////            if (yr != "2020")  //for 2020 reqList_forquery has relevant data
        ////            {
        ////                string is_CCXC = string.Empty;
        ////                bool CCXCflag = false;
        ////                string presentUserDept = string.Empty;
        ////                if (selected_ccxc != "") //DATA FILTER BASED ON CCXC DROPDOWM - NOT USER'S NTID
        ////                {
        ////                    if (selected_ccxc.ToUpper().Contains("CC"))
        ////                    {
        ////                        is_CCXC = "CC";
        ////                        CCXCflag = true;
        ////                    }
        ////                    else if (selected_ccxc.ToUpper().Contains("XC"))
        ////                    {
        ////                        is_CCXC = "XC";
        ////                        CCXCflag = true;
        ////                    }

        ////                }
        ////                else                    //DATA FILTER BASED ON USER'S NTID
        ////                {
        ////                    presentUserDept = lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).Department;

        ////                    if (presentUserDept.ToUpper().Contains("CC"))
        ////                    {
        ////                        is_CCXC = "CC";
        ////                        CCXCflag = true;
        ////                    }
        ////                    else if (presentUserDept.ToUpper().Contains("XC"))
        ////                    {
        ////                        is_CCXC = "XC";
        ////                        CCXCflag = true;
        ////                    }

        ////                }

        ////                if (CCXCflag)
        ////                {
        ////                    if (is_CCXC.Contains("XC"))
        ////                    {
        ////                        reqList = reqList.FindAll(ss => ss.BU.Contains(lstBUs.Find(bu => bu.BU.Contains("2WP")).ID.ToString()));
        ////                    }
        ////                    reqList = reqList.FindAll(dpt => lstDEPTs.Find(x => x.ID.ToString().Contains(dpt.DEPT)).DEPT.Contains(is_CCXC))/*.FindAll(ss => ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("AS")).ID.ToString()) || ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("2WP")).ID.ToString()))*/;
        ////                    CCXCflag = false;
        ////                }



        ////            }
        ////            //reqList = db.RequestItems_Table_TEST.ToList<RequestItems_Table_TEST>().FindAll(z=>z.CostElement.Contains(costelement_chosen_id.Trim())).FindAll(z=>z.Category.Contains(Category_Chosen.ToString())).FindAll(z=>z.ItemName.Contains(Item_Chosen.ToString()))/*.FindAll(x=>x.ApprovalDH == false)*/;

        ////            db.Database.CommandTimeout = 500;


        ////            foreach (RequestItems_Table_TEST item in reqList)
        ////            {

        ////                RequestItemsRepoView ritem = new RequestItemsRepoView();

        ////                ritem.Category = int.Parse(item.Category);
        ////                ritem.Comments = item.Comments;
        ////                ritem.Cost_Element = int.Parse(item.CostElement);
        ////                ritem.BU = int.Parse(item.BU);

        ////                ritem.DEPT = int.Parse(item.DEPT);
        ////                ritem.Group = int.Parse(item.Group);
        ////                ritem.Item_Name = int.Parse(item.ItemName);
        ////                ritem.OEM = int.Parse(item.OEM);
        ////                ritem.Required_Quantity = item.ReqQuantity;
        ////                ritem.RequestID = item.RequestID;

        ////                ritem.Requestor = item.RequestorNT;
        ////                ritem.Total_Price = item.TotalPrice;
        ////                ritem.Unit_Price = item.UnitPrice;
        ////                ritem.ApprovalHoE = item.ApprovalDH;
        ////                ritem.ApprovalSH = item.ApprovalSH;
        ////                ritem.ApprovedHoE = item.ApprovedDH;
        ////                ritem.ApprovedSH = item.ApprovedSH;
        ////                if (item.isCancelled != null)
        ////                {
        ////                    ritem.isCancelled = (int)item.isCancelled;
        ////                }


        ////                ritem.Total_Price = item.TotalPrice;
        ////                ritem.Reviewer_1 = item.DHNT;
        ////                ritem.Reviewer_2 = item.SHNT;

        ////                ritem.RequestDate = item.RequestDate != null ? ((DateTime)item.RequestDate).ToString("yyyy-MM-dd") : string.Empty;
        ////                ritem.SubmitDate = item.SubmitDate != null ? ((DateTime)item.SubmitDate).ToString("yyyy-MM-dd") : string.Empty;
        ////                ritem.Review1_Date = item.DHAppDate != null ? ((DateTime)item.DHAppDate).ToString("yyyy-MM-dd") : string.Empty;
        ////                ritem.Review2_Date = item.SHAppDate != null ? ((DateTime)item.SHAppDate).ToString("yyyy-MM-dd") : string.Empty;


        ////                if (item.OrderStatus != null && item.OrderStatus.Trim() != "")
        ////                {
        ////                    ritem.OrderStatus = int.Parse(item.OrderStatus);

        ////                }
        ////                else
        ////                {
        ////                    ritem.OrderStatus = null;


        ////                }

        ////                //Checking Request Status
        ////                if (ritem.ApprovalHoE == true && ritem.ApprovalSH == false)
        ////                {
        ////                    ritem.Request_Status = "In Review with HoE";
        ////                }
        ////                else if (ritem.ApprovalHoE == true && ritem.ApprovalSH == true && ritem.ApprovedSH == false)
        ////                {
        ////                    ritem.Request_Status = "In Review with VKM SPOC";
        ////                }
        ////                else if (ritem.ApprovalHoE == true && ritem.ApprovalSH == true && ritem.ApprovedSH == true)
        ////                {
        ////                    ritem.Request_Status = "Reviewed by VKM SPOC";
        ////                }
        ////                else if (ritem.ApprovalHoE == false && ritem.ApprovalSH == false && ritem.isCancelled == 1)
        ////                {
        ////                    ritem.Request_Status = "SentBack by HoE";
        ////                }
        ////                else if (ritem.ApprovalHoE == false && ritem.ApprovalSH == false && ritem.isCancelled == 2)
        ////                {
        ////                    ritem.Request_Status = "SentBack by VKM SPOC";
        ////                }
        ////                else
        ////                {
        ////                    ritem.Request_Status = "In Requestor's Queue";
        ////                }











        ////                viewList_itemdrilldown1.Add(ritem);


        ////            }
        ////        }
        ////        // return viewList.FindAll(xi => xi.RequestDate.ToString().Contains(year))/*.FindAll(x => x.ApprovalHoE == false)*/;


        ////    }
        ////    viewList_itemdrilldown = viewList_itemdrilldown1;
        ////    return Json(new { data = viewList_itemdrilldown1 }, JsonRequestBehavior.AllowGet);

        ////}

        ////public ActionResult GetData_ItemDrillDown()
        ////{

        ////    return Json(new { data = viewList_itemdrilldown }, JsonRequestBehavior.AllowGet);

        ////}



        ////[HttpPost]
        //public ActionResult ItemDrillDown_Index(string Years_Listasstring, string CostElement_Chosen, string Category_Chosen, string Item_Chosen/*, List<string> buList, List<string> years*/, string BuList_Listasstring, string selected_ccxc = "")
        //{

        //    List<string> years = new List<string>();
        //    List<string> buList = new List<string>();
        //    string costelement_chosen_id = string.Empty;

        //    RequestListAttributes viewList_itemdrilldown1 = new RequestListAttributes()
        //    {
        //        RequestItemsRepoView_model = new List<RequestItemsRepoView>()
        //    };

        //    if (Years_Listasstring != null)
        //    {
        //        years = (Years_Listasstring.Split(',')).ToList();

        //    }
        //    if (BuList_Listasstring != null)
        //    {
        //        buList = (BuList_Listasstring.Split(',')).ToList();

        //    }
        //    //if CostElement_Chosen is costelt name -> fetch its id
        //    costelement_chosen_id = lstCostElement.Find(cost => cost.CostElement.ToUpper().Trim() == CostElement_Chosen.ToUpper().Trim()).ID.ToString().Trim();
        //    Category_Chosen = lstPrdCateg.Find(cat => cat.Category.ToUpper().Trim() == Category_Chosen.ToUpper().Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", "")).ID.ToString().Trim();
        //    Item_Chosen = lstItems.Find(item => item.S_No == int.Parse(Item_Chosen)).Item_Name;




        //    using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //    //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //    {
        //        List<RequestItems_Table_TEST> reqList = new List<RequestItems_Table_TEST>();



        //        foreach (var bu_item in buList)
        //        {
        //            //reqList_forquery ;
        //            reqList.AddRange(db.RequestItems_Table_TEST.ToList<RequestItems_Table_TEST>().FindAll(item => item.ApprovedSH == true).FindAll(item => item.CostElement.Trim() == costelement_chosen_id).FindAll(ss => ss.BU.Contains(bu_item)).FindAll(item => item.Category.Trim() == Category_Chosen).FindAll(v => v.Fund == null || v.Fund.Trim().Contains("2")).FindAll(item => lstItems.Find(i => i.S_No.ToString().Trim() == item.ItemName.Trim()).Item_Name.ToUpper().Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", "").Replace("+", "").Replace(" ", string.Empty) == Item_Chosen.ToUpper().Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", "").Replace("+", "").Replace(" ", string.Empty)));//lstItems.Find(item => item.Item_Name.ToUpper().Trim() == Item_Chosen.ToUpper().Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", "")).S_No.ToString().Trim();



        //        }

        //        foreach (string yr in years)
        //        {
        //            if (reqList.FindAll(z => z.ApprovedSH == true && z.SubmitDate.Value.Year.ToString() == (int.Parse(yr) - 1).ToString().Trim()).Count != 0)
        //                reqList = reqList.FindAll(z => z.SubmitDate.Value.Year.ToString() == (int.Parse(yr) - 1).ToString().Trim());
        //            else
        //                continue;
        //            //CC XC CHECK
        //            //if(yr == "2020")
        //            //{
        //            //    reqList_forquery = reqList_forquery/*.FindAll(ss => ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("AS")).ID.ToString()) || ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("2WP")).ID.ToString()))*/;
        //            //}
        //            //else
        //            if ((int.Parse(yr) - 1) > 2020)  //for 2020 reqList_forquery has relevant data
        //            {
        //                string is_CCXC = string.Empty;
        //                bool CCXCflag = false;
        //                string presentUserDept = string.Empty;
        //                if (selected_ccxc != "") //DATA FILTER BASED ON CCXC DROPDOWM - NOT USER'S NTID
        //                {
        //                    if (selected_ccxc.ToUpper().Contains("CC"))
        //                    {
        //                        is_CCXC = "CC";
        //                        CCXCflag = true;
        //                    }
        //                    else if (selected_ccxc.ToUpper().Contains("XC"))
        //                    {
        //                        is_CCXC = "XC";
        //                        CCXCflag = true;
        //                    }

        //                }
        //                else                    //DATA FILTER BASED ON USER'S NTID
        //                {
        //                    presentUserDept = lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).Department;

        //                    if (presentUserDept.ToUpper().Contains("CC"))
        //                    {
        //                        is_CCXC = "CC";
        //                        CCXCflag = true;
        //                    }
        //                    else if (presentUserDept.ToUpper().Contains("XC"))
        //                    {
        //                        is_CCXC = "XC";
        //                        CCXCflag = true;
        //                    }

        //                }

        //                if (CCXCflag)
        //                {
        //                    if (is_CCXC.Contains("XC"))
        //                    {
        //                        reqList = reqList.FindAll(ss => ss.BU.Contains(lstBUs.Find(bu => bu.BU.Contains("2WP")).ID.ToString()));
        //                    }
        //                    reqList = reqList.FindAll(dpt => lstDEPTs.Find(x => x.ID.ToString().Contains(dpt.DEPT)).DEPT.Contains(is_CCXC))/*.FindAll(ss => ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("AS")).ID.ToString()) || ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("2WP")).ID.ToString()))*/;
        //                    CCXCflag = false;
        //                }



        //            }
        //            //reqList = db.RequestItems_Table_TEST.ToList<RequestItems_Table_TEST>().FindAll(z=>z.CostElement.Contains(costelement_chosen_id.Trim())).FindAll(z=>z.Category.Contains(Category_Chosen.ToString())).FindAll(z=>z.ItemName.Contains(Item_Chosen.ToString()))/*.FindAll(x=>x.ApprovalDH == false)*/;

        //            db.Database.CommandTimeout = 500;


        //            foreach (RequestItems_Table_TEST item in reqList)
        //            {

        //                RequestItemsRepoView ritem = new RequestItemsRepoView();

        //                ritem.Category = int.Parse(item.Category);
        //                ritem.Comments = item.Comments;
        //                ritem.Cost_Element = int.Parse(item.CostElement);
        //                ritem.BU = int.Parse(item.BU);

        //                ritem.DEPT = int.Parse(item.DEPT);
        //                ritem.Group = int.Parse(item.Group);
        //                ritem.Item_Name = int.Parse(item.ItemName);
        //                ritem.OEM = int.Parse(item.OEM);
        //                ritem.Required_Quantity = item.ReqQuantity;
        //                ritem.Reviewed_Quantity = item.ApprQuantity;
        //                ritem.RequestID = item.RequestID;

        //                ritem.Requestor = item.RequestorNT;
        //                ritem.Total_Price = item.TotalPrice;
        //                ritem.Unit_Price = item.UnitPrice;
        //                ritem.ApprovalHoE = item.ApprovalDH;
        //                ritem.ApprovalSH = item.ApprovalSH;
        //                ritem.ApprovedHoE = item.ApprovedDH;
        //                ritem.ApprovedSH = item.ApprovedSH;
        //                if (item.isCancelled != null)
        //                {
        //                    ritem.isCancelled = (int)item.isCancelled;
        //                }


        //                ritem.Total_Price = item.TotalPrice;
        //                ritem.Reviewed_Cost = item.ApprCost;
        //                ritem.OrderedQuantity = item.OrderedQuantity;
        //                ritem.OrderPrice = item.OrderPrice;

        //                if (item.OrderStatus != null && item.OrderStatus.Trim() != "")
        //                {
        //                    ritem.OrderStatus = int.Parse(item.OrderStatus);

        //                }
        //                else
        //                {
        //                    ritem.OrderStatus = null;


        //                }
        //                ritem.OrderID = item.OrderID;
        //                ritem.Reviewer_1 = item.DHNT;
        //                ritem.Reviewer_2 = item.SHNT;

        //                ritem.RequestDate = item.RequestDate != null ? ((DateTime)item.RequestDate).ToString("yyyy-MM-dd") : string.Empty;
        //                ritem.SubmitDate = item.SubmitDate != null ? ((DateTime)item.SubmitDate).ToString("yyyy-MM-dd") : string.Empty;
        //                ritem.Review1_Date = item.DHAppDate != null ? ((DateTime)item.DHAppDate).ToString("yyyy-MM-dd") : string.Empty;
        //                ritem.Review2_Date = item.SHAppDate != null ? ((DateTime)item.SHAppDate).ToString("yyyy-MM-dd") : string.Empty;


        //                if (item.OrderStatus != null && item.OrderStatus.Trim() != "")
        //                {
        //                    ritem.OrderStatus = int.Parse(item.OrderStatus);

        //                }
        //                else
        //                {
        //                    ritem.OrderStatus = null;


        //                }

        //                //Checking Request Status
        //                if (ritem.ApprovalHoE == true && ritem.ApprovalSH == false)
        //                {
        //                    ritem.Request_Status = "In Review with HoE";
        //                }
        //                else if (ritem.ApprovalHoE == true && ritem.ApprovalSH == true && ritem.ApprovedSH == false)
        //                {
        //                    ritem.Request_Status = "In Review with VKM SPOC";
        //                }
        //                else if (ritem.ApprovalHoE == true && ritem.ApprovalSH == true && ritem.ApprovedSH == true)
        //                {
        //                    ritem.Request_Status = "Reviewed by VKM SPOC";
        //                }
        //                else if (ritem.ApprovalHoE == false && ritem.ApprovalSH == false && ritem.isCancelled == 1)
        //                {
        //                    ritem.Request_Status = "SentBack by HoE";
        //                }
        //                else if (ritem.ApprovalHoE == false && ritem.ApprovalSH == false && ritem.isCancelled == 2)
        //                {
        //                    ritem.Request_Status = "SentBack by VKM SPOC";
        //                }
        //                else
        //                {
        //                    ritem.Request_Status = "In Requestor's Queue";
        //                }









        //                viewList_itemdrilldown1.RequestItemsRepoView_model.Add(ritem);
        //                viewList_itemdrilldown1.Item_Name = Item_Chosen.Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", "");
        //                // viewList_itemdrilldown1.Add(ritem);


        //            }
        //        }
        //        // return viewList.FindAll(xi => xi.RequestDate.ToString().Contains(year))/*.FindAll(x => x.ApprovalHoE == false)*/;


        //    }

        //    return View(viewList_itemdrilldown1);

        //}



        //[HttpPost]
        //public ActionResult ItemDrillDown(string CostElement_Chosen, string Category_Chosen, string Item_Chosen, List<string> buList, List<string> years, string selected_ccxc = "")
        //{

        //    string costelement_chosen_id = string.Empty;
        //    List<RequestItemsRepoView> viewList_itemdrilldown1 = new List<RequestItemsRepoView>();

        //    costelement_chosen_id = lstCostElement.Find(cost => cost.CostElement.ToUpper().Trim() == CostElement_Chosen.ToUpper().Trim()).ID.ToString().Trim();
        //    Category_Chosen = lstPrdCateg.Find(cat => cat.Category.ToUpper().Trim() == Category_Chosen.ToUpper().Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", "")).ID.ToString().Trim();
        //    Item_Chosen = lstItems.Find(item => item.S_No == int.Parse(Item_Chosen)).Item_Name;


        //    using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //    //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //    {
        //        List<RequestItems_Table_TEST> reqList = new List<RequestItems_Table_TEST>();
        //        List<RequestItems_Table_TEST> reqList1 = new List<RequestItems_Table_TEST>();



        //        foreach (var bu_item in buList)
        //        {
        //            reqList.AddRange(db.RequestItems_Table_TEST.ToList<RequestItems_Table_TEST>().FindAll(item => item.ApprovedSH == true).FindAll(item => item.CostElement.Trim() == costelement_chosen_id).FindAll(v => v.Fund == null || v.Fund.Trim().Contains("2")).FindAll(ss => ss.BU.Contains(bu_item)).FindAll(item => item.Category.Trim() == Category_Chosen).FindAll(item => lstItems.Find(i => i.S_No.ToString().Trim() == item.ItemName.Trim()).Item_Name.ToUpper().Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", "").Replace("+", "").Replace(" ", string.Empty) == Item_Chosen.ToUpper().Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", "").Replace("+", "").Replace(" ", string.Empty)));//lstItems.Find(item => item.Item_Name.ToUpper().Trim() == Item_Chosen.ToUpper().Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", "")).S_No.ToString().Trim();

        //        }

        //        foreach (string yr in years)
        //        {
        //            if (reqList.FindAll(z => z.ApprovedSH == true && z.SubmitDate.Value.Year.ToString() == (int.Parse(yr) - 1).ToString().Trim()).Count != 0)
        //                reqList1 = reqList.FindAll(z => z.SubmitDate.Value.Year.ToString() == (int.Parse(yr) - 1).ToString().Trim());
        //            else
        //                continue;
        //            if ((int.Parse(yr) - 1) > 2020)  //for 2020 reqList_forquery has relevant data
        //            {
        //                string is_CCXC = string.Empty;
        //                bool CCXCflag = false;
        //                string presentUserDept = string.Empty;
        //                if (selected_ccxc != "") //DATA FILTER BASED ON CCXC DROPDOWM - NOT USER'S NTID
        //                {
        //                    if (selected_ccxc.ToUpper().Contains("CC"))
        //                    {
        //                        is_CCXC = "CC";
        //                        CCXCflag = true;
        //                    }
        //                    else if (selected_ccxc.ToUpper().Contains("XC"))
        //                    {
        //                        is_CCXC = "XC";
        //                        CCXCflag = true;
        //                    }

        //                }
        //                else                    //DATA FILTER BASED ON USER'S NTID
        //                {
        //                    presentUserDept = lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).Department;

        //                    if (presentUserDept.ToUpper().Contains("CC"))
        //                    {
        //                        is_CCXC = "CC";
        //                        CCXCflag = true;
        //                    }
        //                    else if (presentUserDept.ToUpper().Contains("XC"))
        //                    {
        //                        is_CCXC = "XC";
        //                        CCXCflag = true;
        //                    }

        //                }

        //                if (CCXCflag)
        //                {
        //                    if (is_CCXC.Contains("XC"))
        //                    {
        //                        reqList1 = reqList.FindAll(ss => ss.BU.Contains(lstBUs.Find(bu => bu.BU.Contains("2WP")).ID.ToString()));
        //                    }
        //                    reqList1 = reqList.FindAll(dpt => lstDEPTs.Find(x => x.ID.ToString().Contains(dpt.DEPT)).DEPT.Contains(is_CCXC))/*.FindAll(ss => ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("AS")).ID.ToString()) || ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("2WP")).ID.ToString()))*/;
        //                    CCXCflag = false;
        //                }



        //            }
        //            //reqList = db.RequestItems_Table_TEST.ToList<RequestItems_Table_TEST>().FindAll(z=>z.CostElement.Contains(costelement_chosen_id.Trim())).FindAll(z=>z.Category.Contains(Category_Chosen.ToString())).FindAll(z=>z.ItemName.Contains(Item_Chosen.ToString()))/*.FindAll(x=>x.ApprovalDH == false)*/;

        //            db.Database.CommandTimeout = 500;


        //            foreach (RequestItems_Table_TEST item in reqList1)
        //            {

        //                RequestItemsRepoView ritem = new RequestItemsRepoView();

        //                ritem.Category = int.Parse(item.Category);
        //                ritem.Comments = item.Comments;
        //                ritem.Cost_Element = int.Parse(item.CostElement);
        //                ritem.BU = int.Parse(item.BU);

        //                ritem.DEPT = int.Parse(item.DEPT);
        //                ritem.Group = int.Parse(item.Group);
        //                ritem.Item_Name = int.Parse(item.ItemName);
        //                ritem.OEM = int.Parse(item.OEM);
        //                ritem.Required_Quantity = item.ReqQuantity;
        //                ritem.RequestID = item.RequestID;
        //                ritem.Reviewed_Quantity = item.ApprQuantity;

        //                ritem.Requestor = item.RequestorNT;
        //                ritem.Total_Price = item.TotalPrice;
        //                ritem.Unit_Price = item.UnitPrice;
        //                ritem.ApprovalHoE = item.ApprovalDH;
        //                ritem.ApprovalSH = item.ApprovalSH;
        //                ritem.ApprovedHoE = item.ApprovedDH;
        //                ritem.ApprovedSH = item.ApprovedSH;
        //                if (item.isCancelled != null)
        //                {
        //                    ritem.isCancelled = (int)item.isCancelled;
        //                }


        //                ritem.Total_Price = item.TotalPrice;
        //                ritem.Reviewer_1 = item.DHNT;
        //                ritem.Reviewer_2 = item.SHNT;

        //                ritem.RequestDate = item.RequestDate != null ? ((DateTime)item.RequestDate).ToString("yyyy-MM-dd") : string.Empty;
        //                ritem.SubmitDate = item.SubmitDate != null ? ((DateTime)item.SubmitDate).ToString("yyyy-MM-dd") : string.Empty;
        //                ritem.Review1_Date = item.DHAppDate != null ? ((DateTime)item.DHAppDate).ToString("yyyy-MM-dd") : string.Empty;
        //                ritem.Review2_Date = item.SHAppDate != null ? ((DateTime)item.SHAppDate).ToString("yyyy-MM-dd") : string.Empty;


        //                if (item.OrderStatus != null && item.OrderStatus.Trim() != "")
        //                {
        //                    ritem.OrderStatus = int.Parse(item.OrderStatus);

        //                }
        //                else
        //                {
        //                    ritem.OrderStatus = null;


        //                }

        //                //Checking Request Status
        //                if (ritem.ApprovalHoE == true && ritem.ApprovalSH == false)
        //                {
        //                    ritem.Request_Status = "In Review with HoE";
        //                }
        //                else if (ritem.ApprovalHoE == true && ritem.ApprovalSH == true && ritem.ApprovedSH == false)
        //                {
        //                    ritem.Request_Status = "In Review with VKM SPOC";
        //                }
        //                else if (ritem.ApprovalHoE == true && ritem.ApprovalSH == true && ritem.ApprovedSH == true)
        //                {
        //                    ritem.Request_Status = "Reviewed by VKM SPOC";
        //                }
        //                else if (ritem.ApprovalHoE == false && ritem.ApprovalSH == false && ritem.isCancelled == 1)
        //                {
        //                    ritem.Request_Status = "SentBack by HoE";
        //                }
        //                else if (ritem.ApprovalHoE == false && ritem.ApprovalSH == false && ritem.isCancelled == 2)
        //                {
        //                    ritem.Request_Status = "SentBack by VKM SPOC";
        //                }
        //                else
        //                {
        //                    ritem.Request_Status = "In Requestor's Queue";
        //                }

        //                ritem.Reviewed_Cost = item.ApprCost;
        //                ritem.OrderedQuantity = item.OrderedQuantity;
        //                ritem.OrderPrice = item.OrderPrice;

        //                if (item.OrderStatus != null && item.OrderStatus.Trim() != "")
        //                {
        //                    ritem.OrderStatus = int.Parse(item.OrderStatus);

        //                }
        //                else
        //                {
        //                    ritem.OrderStatus = null;
        //                }
        //                ritem.OrderID = item.OrderID;

        //                viewList_itemdrilldown1.Add(ritem);


        //            }
        //        }
        //        // return viewList.FindAll(xi => xi.RequestDate.ToString().Contains(year))/*.FindAll(x => x.ApprovalHoE == false)*/;


        //    }

        //    return Json(new { data = viewList_itemdrilldown1 }, JsonRequestBehavior.AllowGet);

        //}
        public partial class Overall
        {

            public List<Overall_Table> overall { get; set; }
            public List<Totals_Table> total { get; set; }
        }


        public partial class Overall_Table
        {
            public string VKM_elt { get; set; }
            public int VKM_yr { get; set; }
            public decimal Plan { get; set; }
            public decimal Used { get; set; }
            public decimal Available { get; set; }
            public string splitup_ratio { get; set; }
        }

        public partial class Totals_Table
        {
            public string is_Plan_Util { get; set; }
            public decimal Totals { get; set; }
            //public string splitup_ratio { get; set; }
        }
        public partial class RequestListAttributes
        {

            public List<RequestItemsRepoView> RequestItemsRepoView_model { get; set; }
            public string Item_Name { get; set; }
        }

        [HttpGet]
        public ActionResult Lookup()
        {
            LookupData lookupData = new LookupData();



            //string presentUserDept = lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).Department;
            string is_CCXC = string.Empty; //For Clear CC XC Segregation

            lookupData.DEPT_List = lstDEPTs.ToList();

            lookupData.BU_List = lstBUs.OrderBy(x => x.ID).ToList();
            lookupData.BU_List[2].BU = "MB";
            lookupData.BU_List[4].BU = "OSS";
            lookupData.OEM_List = lstOEMs;
            //lookupData.Groups_oldList = lstGroups_old;

            //using(BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

            //using(BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

            lookupData.Groups_test = lstGroups_test;
            // lookupData.Groups_List = lstGroups;
            lookupData.Item_List = lstItems;
            lookupData.Category_List = lstPrdCateg;
            lookupData.CostElement_List = lstCostElement;

            return Json(new { data = lookupData }, JsonRequestBehavior.AllowGet);

        }

        public class Quarterly
        {
            public List<Quarterly_Util> Y1 { get; set; }
            public List<Quarterly_Util> Y2 { get; set; }

            public bool Year1 { get; set; }
            public bool Year2 { get; set; }
        }

        public partial class Quarterly_Util {
            public string CostElement { get; set; }
            public string BU { get; set; }
            public string BudgetCode { get; set; }
            public int Year { get; set; }
            public string Quarter { get; set; }
            public double Plan { get; set; }
            public double Projected { get; set; }
            public double Utilized { get; set; }
        }


        public partial class vkmSummary
        {
            public string vkmyear { get; set; }
            public decimal P_MAE_Totals { get; set; }
            public decimal P_NMAE_Totals { get; set; }
            public decimal P_Software_Totals { get; set; }
            public decimal Cancelled_MAE_Totals { get; set; }
            public decimal P_Overall_Totals { get; set; }
            public decimal U_MAE_Totals { get; set; }
            public decimal U_NMAE_Totals { get; set; }
            public decimal Cancelled_NMAE_Totals { get; set; }
            public decimal U_Software_Totals { get; set; }
            public decimal Cancelled_Software_Totals { get; set; }
            public decimal U_Overall_Totals { get; set; }


            public decimal Proj_MAE_Totals { get; set; }
            public decimal Proj_NMAE_Totals { get; set; }
            public decimal Proj_Software_Totals { get; set; }
            public decimal Proj_Overall_Totals { get; set; }


            public decimal Unused_MAE_Totals { get; set; }
            public decimal Unused_NMAE_Totals { get; set; }
            public decimal Unused_Software_Totals { get; set; }
            public decimal Unused_Overall_Totals { get; set; }




        }

        public partial class costelement_drilldownSummary
        {
            public string vkmyear { get; set; }
            public decimal[] P_Category_Totals { get; set; }
            public decimal[] U_Category_Totals { get; set; }
            public decimal[] Proj_Category_Totals { get; set; }
            public decimal[] Unused_Category_Totals { get; set; }
            public decimal[] Cancelled_Category_Totals { get; set; }

        }

        public partial class category_drilldownSummary
        {
            public string vkmyear { get; set; }
            public decimal[] P_Item_Totals { get; set; }
            public decimal[] U_Item_Totals { get; set; }
            public decimal[] Proj_Item_Totals { get; set; }
            public decimal[] Unused_Item_Totals { get; set; }
            public decimal[] Cancelled_Item_Totals { get; set; }


        }


        public partial class CategoryBudget
        {
            public int CategoryID { get; set; }
            public string CategoryName { get; set; }
            public decimal CategoryTotals { get; set; }
        }

        public partial class ItemBudget
        {
            public int ItemID { get; set; }
            public string ItemName { get; set; }
            public decimal ItemTotals { get; set; }
        }


        //public class LookupData
        //{

        //    public List<BU_Table> BU_List { get; set; }
        //    public List<OEM_Table> OEM_List { get; set; }
        //    public List<DEPT_Table> DEPT_List { get; set; }
        //    public List<Groups_Table> Groups_List { get; set; }
        //    public List<Groups_Table_Aug> Groups_oldList { get; set; }
        //    public List<Groups_Table_Test> Groups_test { get; set; }

        //    public List<CostElement_Table> CostElement_List { get; set; }
        //    public List<Category_Table> Category_List { get; set; }
        //    public List<ItemsCostList_Table> Item_List { get; set; }
        //    public List<Currency_Table> Currency_List { get; set; }
        //    public List<Order_Status_Table> OrderStatus_List { get; set; }
        //    public List<LeadTime_Table> VendorCategory_List { get; set; }
        //    public List<Fund_Table> Fund_List { get; set; }


        //}

        //public class BudgetParam
        //{
        //    public string jsondata { get; set; }
        //    public string columns { get; set; }
        //    public string data { get; set; }
        //}
        //public class columnsinfo
        //{
        //    public string title { get; set; }
        //    public string data { get; set; }
        //}
    
    }
}






// COCKPIT V1 : OLD DESIGN - With scrolling of pg
//namespace LC_Reports_V1.Controllers
//{
//    [Authorize(Users = @"apac\din2cob,apac\jov6cob,apac\mta2cob,apac\muu4cob,apac\sbr2kor,apac\chb1kor,apac\oig1cob,apac\mae9cob,apac\rau2kor,apac\rma5cob, apac\pch2kor")]
//    public class CockpitController : Controller
//    {
//        public static List<SPOTONData_Table_2021> lstUsers = new List<SPOTONData_Table_2021>();
//        public static List<BU_Table> lstBUs = new List<BU_Table>();
//        public static List<string> lstSections = new List<string>();
//        public static List<DEPT_Table> lstDEPTs = new List<DEPT_Table>();
//        public static List<Groups_Table> lstGroups = new List<Groups_Table>();
//        public static List<OEM_Table> lstOEMs = new List<OEM_Table>();
//        public static List<DEPT_Table> lstBudgBU = new List<DEPT_Table>();
//        public static List<Category_Table> lstPrdCateg = new List<Category_Table>();
//        public static List<ItemsCostList_Table> lstItems = new List<ItemsCostList_Table>();
//        public static List<CostElement_Table> lstCostElement = new List<CostElement_Table>();
//        public static List<tbl_UserIDs_Table> lstPrivileged = new List<tbl_UserIDs_Table>();
//        public static List<BU_SPOCS> lstBU_SPOCs = new List<BU_SPOCS>();
//        public static List<Order_Status_Table> lstOrderStatus = new List<Order_Status_Table>();
//        public static List<Currency_Table> lstCurrency = new List<Currency_Table>();
//        public static List<LeadTime_Table> lstVendor = new List<LeadTime_Table>();
//        public static List<Fund_Table> lstFund = new List<Fund_Table>();
//        public static List<Planning_EM_Table> lstEMs = new List<Planning_EM_Table>();
//        public static List<Planning_HOE_Table> lstHOEs = new List<Planning_HOE_Table>();
//        public static List<SPOTONData> lstUsers_2020 = new List<SPOTONData>();
//        public static List<Groups_Table_Aug> lstGroups_old = new List<Groups_Table_Aug>();
//        public static List<Groups_Table_Test> lstGroups_test = new List<Groups_Table_Test>(); //with old new groups


//        public ActionResult Cockpit_Options()
//        {

//            return View();
//        }

//        public ActionResult VKMCockpit()
//        {

//            using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

//            //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

//            {
//                if (lstUsers == null || lstUsers.Count == 0)
//                    lstUsers = db.SPOTONData_Table_2021.ToList<SPOTONData_Table_2021>();
//                lstUsers.Sort((a, b) => a.EmployeeName.CompareTo(b.EmployeeName));
//                if (lstUsers == null || lstUsers.Count == 0)
//                    lstUsers = db.SPOTONData_Table_2021.ToList<SPOTONData_Table_2021>(); //refresh this
//                if (lstOEMs == null || lstOEMs.Count == 0)
//                    lstOEMs = db.OEM_Table.ToList<OEM_Table>();
//                if (lstBUs == null || lstBUs.Count == 0)
//                    lstBUs = db.BU_Table.ToList<BU_Table>();
//                if (lstSections == null || lstSections.Count == 0)
//                    lstSections = lstUsers.Select(x => x.Section).Distinct().ToList();
//                if (lstDEPTs == null || lstDEPTs.Count == 0)
//                    lstDEPTs = db.DEPT_Table.ToList<DEPT_Table>();
//                if (lstGroups == null || lstGroups.Count == 0)
//                    lstGroups = db.Groups_Table.ToList<Groups_Table>();
//                if (lstCostElement == null || lstCostElement.Count == 0)
//                    lstCostElement = db.CostElement_Table.ToList<CostElement_Table>();
//                if (lstPrdCateg == null || lstPrdCateg.Count == 0)
//                    lstPrdCateg = db.Category_Table.ToList<Category_Table>();
//                if (lstItems == null || lstItems.Count == 0)
//                    lstItems = db.ItemsCostList_Table.ToList<ItemsCostList_Table>()/*.FindAll(item=>item.Deleted != true)*/; //refresh this
//                if (lstPrivileged == null || lstPrivileged.Count == 0)
//                    lstPrivileged = db.tbl_UserIDs_Table.ToList<tbl_UserIDs_Table>();
//                if (lstBU_SPOCs == null || lstBU_SPOCs.Count == 0)
//                    lstBU_SPOCs = db.BU_SPOCS.ToList<BU_SPOCS>();
//                if (lstOrderStatus == null || lstOrderStatus.Count == 0)
//                    lstOrderStatus = db.Order_Status_Table.ToList<Order_Status_Table>();
//                if (lstCurrency == null || lstCurrency.Count == 0)
//                    lstCurrency = db.Currency_Table.ToList<Currency_Table>();
//                if (lstVendor == null || lstVendor.Count == 0)
//                    lstVendor = db.LeadTime_Table.ToList<LeadTime_Table>();
//                if (lstFund == null || lstFund.Count == 0)
//                    lstFund = db.Fund_Table.ToList<Fund_Table>();
//                if (lstEMs == null || lstEMs.Count == 0)
//                    lstEMs = db.Planning_EM_Table.ToList<Planning_EM_Table>();//
//                if (lstHOEs == null || lstHOEs.Count == 0)
//                    lstHOEs = db.Planning_HOE_Table.ToList<Planning_HOE_Table>();//


//                if (lstUsers_2020 == null || lstUsers_2020.Count == 0)
//                    lstUsers_2020 = db.SPOTONDatas.ToList<SPOTONData>();
//                if (lstGroups_old == null || lstGroups_old.Count == 0)
//                    lstGroups_old = db.Groups_Table_Aug.ToList<Groups_Table_Aug>();
//                if (lstGroups_test == null || lstGroups_test.Count == 0)
//                    lstGroups_test = db.Groups_Table_Test.ToList<Groups_Table_Test>();




//                lstUsers.Sort((a, b) => a.EmployeeName.CompareTo(b.EmployeeName));
//                lstBUs.Sort((a, b) => a.BU.CompareTo(b.BU));
//                lstOEMs.Sort((a, b) => a.OEM.CompareTo(b.OEM));
//                lstDEPTs.Sort((a, b) => a.DEPT.CompareTo(b.DEPT));
//                lstGroups.Sort((a, b) => a.Group.CompareTo(b.Group));
//                lstCostElement.Sort((a, b) => a.CostElement.CompareTo(b.CostElement));
//                lstPrdCateg.Sort((a, b) => a.Category.CompareTo(b.Category));
//                lstItems.Sort((a, b) => a.Item_Name.CompareTo(b.Item_Name));
//                lstPrivileged.Sort((a, b) => a.ADSID.CompareTo(b.ADSID));
//                lstBU_SPOCs.Sort((a, b) => a.VKMspoc.CompareTo(b.VKMspoc));
//                lstOrderStatus.Sort((a, b) => a.OrderStatus.CompareTo(b.OrderStatus));
//                lstCurrency.Sort((a, b) => a.Currency.CompareTo(b.Currency));
//                lstVendor.Sort((a, b) => a.VendorCategory.CompareTo(b.VendorCategory));
//                lstFund.Sort((a, b) => a.Fund.CompareTo(b.Fund));
//                lstEMs.Sort((a, b) => a.FullName.CompareTo(b.FullName));
//                lstHOEs.Sort((a, b) => a.HOE_FullName.CompareTo(b.HOE_FullName));

//                //r.lstItems1.Sort((a, b) => a.Item_Name.CompareTo(b.Item_Name));

//                lstUsers_2020.Sort((a, b) => a.EmployeeName.CompareTo(b.EmployeeName));
//                lstGroups_old.Sort((a, b) => a.Group.CompareTo(b.Group));
//                lstGroups_test.Sort((a, b) => a.Group.CompareTo(b.Group));

//            }
//            //if (lstUsers == null || lstUsers.Count == 0)
//            //{
//            //    return RedirectToAction("Index", "Budgeting");
//            //}

//            return View();
//        }




//        /// <summary>
//        /// Function to send BU summary data to view
//        /// </summary>
//        /// <returns></returns>
//        public ActionResult VKMSummaryData(List<string> years, List<string> buList, bool chart = false, string selected_ccxc = "")
//        {


//            //chart = 0 (false - table), 1(table + graph), 2(graph)
//            //if (lstUsers == null)
//            //{
//            //    return RedirectToAction("Index", "Budgeting");
//            //}


//            using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

//            //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

//            {
//                List<RequestItems_Table_TEST> reqList = new List<RequestItems_Table_TEST>();
//                List<RequestItems_Table_TEST> reqList_forquery = new List<RequestItems_Table_TEST>();
//                List<vkmSummary> viewList = new List<vkmSummary>();

//                var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
//                BudgetParam t = new BudgetParam();
//                List<columnsinfo> _col = new List<columnsinfo>();


//                decimal OP_MAE_Totals = 0, OP_NMAE_Totals = 0, OP_SoftwareTotals = 0;
//                decimal OU_MAE_Totals = 0, OU_NMAE_Totals = 0, OU_SoftwareTotals = 0;
//                decimal Ch_P_MAE_Totals = 0, Ch_P_NMAE_Totals = 0, Ch_P_SoftwareTotals = 0, Ch_P_OverallTotals = 0;
//                decimal Ch_U_MAE_Totals = 0, Ch_U_NMAE_Totals = 0, Ch_U_SoftwareTotals = 0, Ch_U_OverallTotals = 0;
//                decimal Pr_P_MAE_Totals = 0, Pr_P_NMAE_Totals = 0, Pr_P_SoftwareTotals = 0, Pr_P_OverallTotals = 0;
//                decimal Pr_U_MAE_Totals = 0, Pr_U_NMAE_Totals = 0, Pr_U_SoftwareTotals = 0, Pr_U_OverallTotals = 0;

//                // List<RequestItemsList_2019> reqList2019 = new List<RequestItemsList_2019>();
//                //reqList2019 = db.RequestItemsList_2019.ToList<RequestItemsList_2019>();



//                if (!(chart == false && years.Count() == 3)) //if number of years selected = 3, then no need to show table
//                {


//                    //get reqlist data for the BUs based on SPOC's BU / CC XC Level BUs
//                    foreach (var bu_item in buList)
//                    {
//                        reqList.AddRange(db.RequestItems_Table_TEST.ToList<RequestItems_Table_TEST>().FindAll(ss => ss.BU.Contains(bu_item)));
//                    }

//                    foreach (string yr in years)
//                    {
//                        //CC XC CHECK


//                        //for 2020 reqList_forquery has relevant data under the BU filtering ; but >2020 needs dept FIltering based on CC XC
//                        //if(int.Parse(yr)-1 == 2020)
//                        //{
//                        //    reqList_forquery.AddRange(reqList.FindAll(ss => ss.BU.Contains(lstBUs.Find(bu => bu.BU.Contains("DA")).ID.ToString())));
//                        //}
//                        if (int.Parse(yr)-1 > 2020)
//                        {

//                            string is_CCXC = string.Empty;
//                            bool CCXCflag = false;
//                            string presentUserDept = string.Empty;
//                            if (selected_ccxc != "") //DATA FILTER BASED ON CCXC DROPDOWM - NOT USER'S NTID
//                            {
//                                if (selected_ccxc.ToUpper().Contains("CC"))
//                                {
//                                    is_CCXC = "CC";
//                                    CCXCflag = true;
//                                }
//                                else if (selected_ccxc.ToUpper().Contains("XC"))
//                                {
//                                    is_CCXC = "XC";
//                                    CCXCflag = true;
//                                }

//                            }
//                            else                    //DATA FILTER BASED ON USER'S NTID
//                            {
//                                presentUserDept = lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).Department;

//                                if (presentUserDept.ToUpper().Contains("CC"))
//                                {
//                                    is_CCXC = "CC";
//                                    CCXCflag = true;
//                                }
//                                else if (presentUserDept.ToUpper().Contains("XC"))
//                                {
//                                    is_CCXC = "XC";
//                                    CCXCflag = true;
//                                }

//                            }

//                            if (CCXCflag)
//                            {
//                                if (is_CCXC.Contains("XC"))
//                                {
//                                    //XC includes 2WP 
//                                    reqList_forquery.AddRange(db.RequestItems_Table_TEST.ToList().FindAll(ss => ss.BU.Contains(lstBUs.Find(bu => bu.BU.Contains("2WP")).ID.ToString())));
//                                }
//                                reqList_forquery = reqList.FindAll(dpt => lstDEPTs.Find(x => x.ID.ToString().Contains(dpt.DEPT)).DEPT.Contains(is_CCXC));
//                                CCXCflag = false;
//                            }



//                        }
//                        else
//                            reqList_forquery = reqList;
//                        //CODE TO GET GROUPS OF COST ELEMENT
//                        IEnumerable<IGrouping<string, RequestItems_Table_TEST>> query = reqList_forquery.GroupBy(item => item.CostElement);

//                        decimal P_MAE_Totals = 0, P_NMAE_Totals = 0, P_SoftwareTotals = 0;
//                        decimal U_MAE_Totals = 0, U_NMAE_Totals = 0, U_SoftwareTotals = 0;
//                        var danger1 = string.Empty;
//                        vkmSummary tempobj = new vkmSummary();
//                        tempobj.vkmyear = yr;


//                        //CODE TO GET THE TOTALS OF EACH COST ELEMENT
//                        foreach (IGrouping<string, RequestItems_Table_TEST> CostGroup in query)
//                        {

//                            // Iterate over each value in the
//                            // IGrouping and print the value.

//                            if (lstCostElement.Count != 0)
//                            {
//                                if (lstCostElement.Find(cost => cost.ID.Equals(Int32.Parse(CostGroup.Key))).CostElement == "MAE")
//                                {
//                                    foreach (RequestItems_Table_TEST item in CostGroup)
//                                    {
//                                        if (item.SHAppDate.ToString().Contains((int.Parse(yr) - 1).ToString()))  //yr is VKM Year; if yr == 2020 - 2021 Planning (2020 sh apprvd - 2021 planning)
//                                        {

//                                            P_MAE_Totals += (decimal)item.ApprCost;

//                                            U_MAE_Totals += item.OrderPrice == null ? (decimal)0.0 : (decimal)item.OrderPrice;


//                                        }
//                                    }
//                                }
//                                else if (lstCostElement.Find(cost => cost.ID.Equals(Int32.Parse(CostGroup.Key))).CostElement == "Non-MAE")
//                                {
//                                    foreach (RequestItems_Table_TEST item in CostGroup)
//                                    {
//                                        if (item.SHAppDate.ToString().Contains((int.Parse(yr) - 1).ToString()))
//                                        {

//                                            P_NMAE_Totals += (decimal)item.ApprCost;
//                                            U_NMAE_Totals += item.OrderPrice == null ? (decimal)0.0 : (decimal)item.OrderPrice;
//                                        }
//                                    }

//                                }
//                                else if (lstCostElement.Find(cost => cost.ID.Equals(Int32.Parse(CostGroup.Key))).CostElement == "Software")
//                                {
//                                    foreach (RequestItems_Table_TEST item in CostGroup)
//                                    {
//                                        if (item.SHAppDate.ToString().Contains((int.Parse(yr) - 1).ToString()))
//                                        {


//                                            P_SoftwareTotals += (decimal)item.ApprCost;
//                                            U_SoftwareTotals += item.OrderPrice == null ? (decimal)0.0 : (decimal)item.OrderPrice;
//                                        }
//                                    }

//                                }
//                            }
//                            else
//                                continue;
//                        }
//                        tempobj.P_MAE_Totals = P_MAE_Totals;
//                        OP_MAE_Totals += P_MAE_Totals;
//                        tempobj.P_NMAE_Totals = P_NMAE_Totals;
//                        OP_NMAE_Totals += P_NMAE_Totals;
//                        tempobj.P_Software_Totals = P_SoftwareTotals;
//                        OP_SoftwareTotals += P_SoftwareTotals;
//                        tempobj.P_Overall_Totals = P_MAE_Totals + P_NMAE_Totals + P_SoftwareTotals;

//                        tempobj.U_MAE_Totals = U_MAE_Totals;
//                        OU_MAE_Totals += U_MAE_Totals;
//                        tempobj.U_NMAE_Totals = U_NMAE_Totals;
//                        OU_NMAE_Totals += U_NMAE_Totals;
//                        tempobj.U_Software_Totals = U_SoftwareTotals;
//                        OU_SoftwareTotals += U_SoftwareTotals;
//                        tempobj.U_Overall_Totals = U_MAE_Totals + U_NMAE_Totals + U_SoftwareTotals;

//                        viewList.Add(tempobj);

//                    }





//                    if (!(chart == true && years.Count() == 3)) //if years selected are 3, then no need to calculate change in values
//                    {
//                        Ch_P_MAE_Totals = (viewList.ElementAt(viewList.Count - 1).P_MAE_Totals) - (viewList.ElementAt(viewList.Count - 2).P_MAE_Totals);
//                        Ch_P_NMAE_Totals = (viewList.ElementAt(viewList.Count - 1).P_NMAE_Totals) - (viewList.ElementAt(viewList.Count - 2).P_NMAE_Totals);
//                        Ch_P_SoftwareTotals = (viewList.ElementAt(viewList.Count - 1).P_Software_Totals) - (viewList.ElementAt(viewList.Count - 2).P_Software_Totals);
//                        Ch_P_OverallTotals = (viewList.ElementAt(viewList.Count - 1).P_Overall_Totals) - (viewList.ElementAt(viewList.Count - 2).P_Overall_Totals);
//                        Pr_P_MAE_Totals = (viewList.ElementAt(viewList.Count - 2).P_MAE_Totals) != 0 ? (Ch_P_MAE_Totals / viewList.ElementAt(viewList.Count - 2).P_MAE_Totals) * 100 : 0;
//                        Pr_P_NMAE_Totals = (viewList.ElementAt(viewList.Count - 2).P_NMAE_Totals) != 0 ? (Ch_P_NMAE_Totals / viewList.ElementAt(viewList.Count - 2).P_NMAE_Totals) * 100 : 0;
//                        Pr_P_SoftwareTotals = (viewList.ElementAt(viewList.Count - 2).P_Software_Totals) != 0 ? (Ch_P_SoftwareTotals / viewList.ElementAt(viewList.Count - 2).P_Software_Totals) * 100 : 0;
//                        Pr_P_OverallTotals = (viewList.ElementAt(viewList.Count - 2).P_Overall_Totals) != 0 ? (Ch_P_OverallTotals / viewList.ElementAt(viewList.Count - 2).P_Overall_Totals) * 100 : 0;

//                        Ch_U_MAE_Totals = (viewList.ElementAt(viewList.Count - 1).U_MAE_Totals) - (viewList.ElementAt(viewList.Count - 2).U_MAE_Totals);
//                        Ch_U_NMAE_Totals = (viewList.ElementAt(viewList.Count - 1).U_NMAE_Totals) - (viewList.ElementAt(viewList.Count - 2).U_NMAE_Totals);
//                        Ch_U_SoftwareTotals = (viewList.ElementAt(viewList.Count - 1).U_Software_Totals) - (viewList.ElementAt(viewList.Count - 2).U_Software_Totals);
//                        Ch_U_OverallTotals = (viewList.ElementAt(viewList.Count - 1).U_Overall_Totals) - (viewList.ElementAt(viewList.Count - 2).U_Overall_Totals);
//                        Pr_U_MAE_Totals = (viewList.ElementAt(viewList.Count - 2).U_MAE_Totals) != 0 ? (Ch_U_MAE_Totals / viewList.ElementAt(viewList.Count - 2).U_MAE_Totals) * 100 : 0;
//                        Pr_U_NMAE_Totals = (viewList.ElementAt(viewList.Count - 2).U_NMAE_Totals) != 0 ? (Ch_U_NMAE_Totals / viewList.ElementAt(viewList.Count - 2).U_NMAE_Totals) * 100 : 0;
//                        Pr_U_SoftwareTotals = (viewList.ElementAt(viewList.Count - 2).U_Software_Totals) != 0 ? (Ch_U_SoftwareTotals / viewList.ElementAt(viewList.Count - 2).U_Software_Totals) * 100 : 0;
//                        Pr_U_OverallTotals = (viewList.ElementAt(viewList.Count - 2).U_Overall_Totals) != 0 ? (Ch_U_OverallTotals / viewList.ElementAt(viewList.Count - 2).U_Overall_Totals) * 100 : 0;
//                    }
//                    if (chart)
//                    {
//                        System.Data.DataTable dt = new System.Data.DataTable();
//                        dt.Columns.Add("Year", typeof(string));
//                        dt.Columns.Add("Planned", typeof(decimal));
//                        dt.Columns.Add("Utilized", typeof(decimal));
//                        dt.Columns.Add("Percentage_Utilization", typeof(decimal));

//                        //DataRow dr = dt.NewRow();

//                        int rcnt = 0;

//                        foreach (var info in viewList)
//                        {
//                            DataRow dr = dt.NewRow();
//                            //dr = dt.NewRow();
//                            dr[rcnt++] = "MAE" + " " + info.vkmyear;
//                            dr[rcnt++] = info.P_MAE_Totals;
//                            dr[rcnt++] = info.U_MAE_Totals;
//                            dr[rcnt++] = info.P_MAE_Totals != 0 ? (info.U_MAE_Totals / info.P_MAE_Totals) * 100 : 0;
//                            rcnt = 0;
//                            dt.Rows.Add(dr);
//                            dr = dt.NewRow();
//                            dr[rcnt++] = "Non-MAE" + " " + info.vkmyear;
//                            dr[rcnt++] = info.P_NMAE_Totals;
//                            dr[rcnt++] = info.U_NMAE_Totals;
//                            dr[rcnt++] = info.P_NMAE_Totals != 0 ? (info.U_NMAE_Totals / info.P_NMAE_Totals) * 100 : 0;
//                            rcnt = 0;
//                            dt.Rows.Add(dr);
//                            dr = dt.NewRow();
//                            dr[rcnt++] = "Software" + " " + info.vkmyear;
//                            dr[rcnt++] = info.P_Software_Totals;
//                            dr[rcnt++] = info.U_Software_Totals;
//                            dr[rcnt++] = info.P_Software_Totals != 0 ? (info.U_Software_Totals / info.P_Software_Totals) * 100 : 0;
//                            rcnt = 0;
//                            dt.Rows.Add(dr);
//                            dr = dt.NewRow();
//                            dr[rcnt++] = "Totals" + " " + info.vkmyear;
//                            dr[rcnt++] = info.P_Overall_Totals;
//                            dr[rcnt++] = info.U_Overall_Totals;
//                            dr[rcnt++] = info.P_Overall_Totals != 0 ? (info.U_Overall_Totals / info.P_Overall_Totals) * 100 : 0;


//                            //rcnt++; 
//                            dt.Rows.Add(dr);
//                            // dr = dt.NewRow();
//                            rcnt = 0;
//                        }
//                        //dt.Rows.Add(dr);
//                        for (int i = 0; i <= dt.Columns.Count - 1; i++)
//                        {
//                            _col.Add(new columnsinfo { title = dt.Columns[i].ColumnName, data = dt.Columns[i].ColumnName });
//                        }

//                        string col = (string)serializer.Serialize(_col);
//                        t.columns = col;


//                        var lst = dt.AsEnumerable()
//                        .Select(r => r.Table.Columns.Cast<DataColumn>()
//                                .Select(c => new KeyValuePair<string, object>(c.ColumnName, r[c.Ordinal])
//                               ).ToDictionary(z => z.Key, z => z.Value)
//                        ).ToList();

//                        string data = serializer.Serialize(lst);
//                        t.data = data;
//                    }
//                    else
//                    {
//                        System.Data.DataTable dt = new System.Data.DataTable();
//                        dt.Columns.Add("Cost Element", typeof(string)); /// Planned + (year variable) / Utilized  + (year variable)
//                        //dt.Columns.Add("VKM 2020", typeof(string));
//                        foreach (string yr in years)
//                        {
//                            dt.Columns.Add("VKM" + " " + yr + "(Planned)", typeof(string)); //add vkm text to yr
//                            dt.Columns.Add("VKM" + " " + yr + "(Utilized)", typeof(string));

//                        }
//                        dt.Columns.Add("Change in Value (Planned)", typeof(string));
//                        dt.Columns.Add("Change in Percentage (Planned)", typeof(string));
//                        dt.Columns.Add("Change in Value (Utilized)", typeof(string));
//                        dt.Columns.Add("Change in Percentage (Utilized)", typeof(string));


//                        DataRow dr = dt.NewRow();
//                        dr[0] = "MAE";
//                        int rcnt = 1;
//                        int rcnt1 = 2; //2

//                        foreach (var info in viewList)
//                        {
//                            dr[rcnt] = info.P_MAE_Totals.ToString("C", CultureInfo.CurrentCulture);
//                            dr[rcnt1] = info.U_MAE_Totals.ToString("C", CultureInfo.CurrentCulture);
//                            //rcnt++;
//                            //rcnt = rcnt+ 2;
//                            //rcnt1 = rcnt1 + 2;

//                            if ((viewList.Count * 2 != rcnt1))
//                            {
//                                rcnt = rcnt + 2;
//                                rcnt1 = rcnt1 + 2;
//                            }
//                            else
//                            {
//                                rcnt1 = rcnt1 + 1;
//                            }

//                        }
//                        dr[rcnt1] = Ch_P_MAE_Totals.ToString("C", CultureInfo.CurrentCulture);
//                        dr[rcnt1 + 1] = Math.Round(Pr_P_MAE_Totals, 2) + "%";
//                        dr[rcnt1 + 2] = Ch_U_MAE_Totals.ToString("C", CultureInfo.CurrentCulture);
//                        dr[rcnt1 + 3] = Math.Round(Pr_U_MAE_Totals, 2) + "%";
//                        dt.Rows.Add(dr);

//                        dr = dt.NewRow();
//                        dr[0] = "Non-MAE";
//                        int r1cnt = 1;
//                        int r1cnt1 = 2;
//                        foreach (var info in viewList)
//                        {
//                            dr[r1cnt] = info.P_NMAE_Totals.ToString("C", CultureInfo.CurrentCulture);
//                            dr[r1cnt1] = info.U_NMAE_Totals.ToString("C", CultureInfo.CurrentCulture);
//                            //rcnt++;
//                            //rcnt = rcnt+ 2;
//                            //rcnt1 = rcnt1 + 2;

//                            if ((viewList.Count * 2 != r1cnt1))
//                            {
//                                r1cnt = r1cnt + 2;
//                                r1cnt1 = r1cnt1 + 2;
//                            }
//                            else
//                            {
//                                r1cnt1 = r1cnt1 + 1;
//                            }
//                        }
//                        dr[r1cnt1] = Ch_P_NMAE_Totals.ToString("C", CultureInfo.CurrentCulture);
//                        dr[r1cnt1 + 1] = Math.Round(Pr_P_NMAE_Totals, 2) + "%";
//                        dr[rcnt1 + 2] = Ch_U_NMAE_Totals.ToString("C", CultureInfo.CurrentCulture);
//                        dr[rcnt1 + 3] = Math.Round(Pr_U_NMAE_Totals, 2) + "%";
//                        dt.Rows.Add(dr);

//                        dr = dt.NewRow();
//                        dr[0] = "Software";
//                        int r2cnt = 1;
//                        int r2cnt1 = 2;
//                        foreach (var info in viewList)
//                        {
//                            dr[r2cnt] = info.P_Software_Totals.ToString("C", CultureInfo.CurrentCulture);
//                            dr[r2cnt1] = info.U_Software_Totals.ToString("C", CultureInfo.CurrentCulture);
//                            //rcnt++;
//                            //rcnt = rcnt+ 2;
//                            //rcnt1 = rcnt1 + 2;

//                            if ((viewList.Count * 2 != r2cnt1))
//                            {
//                                r2cnt = r2cnt + 2;
//                                r2cnt1 = r2cnt1 + 2;
//                            }
//                            else
//                            {
//                                r2cnt1 = r2cnt1 + 1;
//                            }
//                        }
//                        dr[r2cnt1] = Ch_P_SoftwareTotals.ToString("C", CultureInfo.CurrentCulture);
//                        dr[r2cnt1 + 1] = Math.Round(Pr_P_SoftwareTotals, 2) + "%";
//                        dr[rcnt1 + 2] = Ch_U_SoftwareTotals.ToString("C", CultureInfo.CurrentCulture);
//                        dr[rcnt1 + 3] = Math.Round(Pr_U_SoftwareTotals, 2) + "%";
//                        dt.Rows.Add(dr);

//                        dr = dt.NewRow();
//                        dr[0] = "Totals";
//                        int r3cnt = 1;
//                        int r3cnt1 = 2;
//                        foreach (var info in viewList)
//                        {
//                            dr[r3cnt] = (info.P_MAE_Totals + info.P_NMAE_Totals + info.P_Software_Totals).ToString("C", CultureInfo.CurrentCulture);
//                            dr[r3cnt1] = (info.U_MAE_Totals + info.U_NMAE_Totals + info.U_Software_Totals).ToString("C", CultureInfo.CurrentCulture);
//                            //rcnt++;
//                            //rcnt = rcnt+ 2;
//                            //rcnt1 = rcnt1 + 2;

//                            if ((viewList.Count * 2 != r3cnt1))
//                            {
//                                r3cnt = r3cnt + 2;
//                                r3cnt1 = r3cnt1 + 2;
//                            }
//                            else
//                            {
//                                r3cnt1 = r3cnt1 + 1;
//                            }
//                        }
//                        dr[r3cnt1] = Ch_P_OverallTotals.ToString("C", CultureInfo.CurrentCulture);
//                        dr[r3cnt1 + 1] = Math.Round(Pr_P_OverallTotals, 2) + "%";
//                        dr[rcnt1 + 2] = Ch_U_OverallTotals.ToString("C", CultureInfo.CurrentCulture);
//                        dr[rcnt1 + 3] = Math.Round(Pr_U_OverallTotals, 2) + "%";
//                        dt.Rows.Add(dr);
//                        for (int i = 0; i <= dt.Columns.Count - 1; i++)
//                        {
//                            _col.Add(new columnsinfo { title = dt.Columns[i].ColumnName, data = dt.Columns[i].ColumnName });
//                        }

//                        string col = (string)serializer.Serialize(_col);
//                        t.columns = col;


//                        var lst = dt.AsEnumerable()
//                        .Select(r => r.Table.Columns.Cast<DataColumn>()
//                                .Select(c => new KeyValuePair<string, object>(c.ColumnName, r[c.Ordinal])
//                               ).ToDictionary(z => z.Key, z => z.Value)
//                        ).ToList();

//                        string data = serializer.Serialize(lst);
//                        t.data = data;


//                    }

//                }
//                else
//                {

//                    t.data = string.Empty;
//                    t.columns = string.Empty;
//                    t.jsondata = string.Empty;


//                }

//                return Json(new { data = t, message = selected_ccxc }, JsonRequestBehavior.AllowGet);

//                // return View();
//            }
//        }





//        public ActionResult CostElementDrillDown_comparison(List<string> years, string CostElement_Chosen, List<string> buList, bool chart = false, string selected_ccxc = "")
//        {

//            if (lstUsers == null)
//            {
//                return RedirectToAction("Index", "Budgeting");
//            }


//            using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

//            //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

//            {
//                List<RequestItems_Table_TEST> reqList = new List<RequestItems_Table_TEST>();
//                List<RequestItems_Table_TEST> reqList_forquery = new List<RequestItems_Table_TEST>();
//                List<CategoryBudget> categoryBudgets = new List<CategoryBudget>();
//                List<CategoryBudget> Top70_CategoryBudget = new List<CategoryBudget>();
//                List<costelement_drilldownSummary> viewList = new List<costelement_drilldownSummary>();
//                string costelement_chosen_id = string.Empty;
//                reqList = db.RequestItems_Table_TEST.ToList<RequestItems_Table_TEST>();
//                int BaseYear_toCompare = 0;
//                //List<RequestItemsList_2019> reqList2019 = new List<RequestItemsList_2019>();

//                //Fetching the Category List
//                foreach (var category in lstPrdCateg)
//                {
//                    CategoryBudget catbud = new CategoryBudget();
//                    catbud.CategoryID = category.ID;
//                    catbud.CategoryName = category.Category;
//                    categoryBudgets.Add(catbud);
//                }


//                //CostElement_Chosen is costelt name -> fetch its id
//                costelement_chosen_id = lstCostElement.Find(cost => cost.CostElement.ToUpper().Trim() == CostElement_Chosen.ToUpper().Trim()).ID.ToString().Trim();

//                foreach (var bu_item in buList)
//                {
//                    reqList_forquery.AddRange(reqList.FindAll(item => item.CostElement.Trim() == costelement_chosen_id).FindAll(ss => ss.BU.Contains(bu_item)));
//                }

//                //CODE TO GET GROUPS OF CATEGORY


//                foreach (string yr in years)
//                {
//                    BaseYear_toCompare++;
//                    //CC XC CHECK

//                    if ((int.Parse(yr) - 1) > 2020)  //for 2020 reqList_forquery has relevant data
//                    {
//                        string is_CCXC = string.Empty;
//                        bool CCXCflag = false;
//                        string presentUserDept = string.Empty;
//                        if (selected_ccxc != "") //DATA FILTER BASED ON CCXC DROPDOWM - NOT USER'S NTID
//                        {
//                            if (selected_ccxc.ToUpper().Contains("CC"))
//                            {
//                                is_CCXC = "CC";
//                                CCXCflag = true;
//                            }
//                            else if (selected_ccxc.ToUpper().Contains("XC"))
//                            {
//                                is_CCXC = "XC";
//                                CCXCflag = true;
//                            }

//                        }
//                        else                    //DATA FILTER BASED ON USER'S NTID
//                        {
//                            presentUserDept = lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).Department;

//                            if (presentUserDept.ToUpper().Contains("CC"))
//                            {
//                                is_CCXC = "CC";
//                                CCXCflag = true;
//                            }
//                            else if (presentUserDept.ToUpper().Contains("XC"))
//                            {
//                                is_CCXC = "XC";
//                                CCXCflag = true;
//                            }

//                        }

//                        if (CCXCflag)
//                        {
//                            if (is_CCXC.Contains("XC"))
//                            {
//                                reqList_forquery.AddRange(reqList.FindAll(ss => ss.BU.Contains(lstBUs.Find(bu => bu.BU.Contains("2WP")).ID.ToString())));
//                            }
//                            reqList_forquery = reqList_forquery.FindAll(dpt => lstDEPTs.Find(x => x.ID.ToString().Contains(dpt.DEPT)).DEPT.Contains(is_CCXC))/*.FindAll(ss => ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("AS")).ID.ToString()) || ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("2WP")).ID.ToString()))*/;
//                            CCXCflag = false;
//                        }



//                    }


//                    decimal[] P_Category_Totals = new decimal[categoryBudgets.Count()];
//                    decimal[] U_Category_Totals = new decimal[categoryBudgets.Count()];
//                    costelement_drilldownSummary tempobj = new costelement_drilldownSummary();
//                    tempobj.vkmyear = yr;


//                    //CODE TO GET THE TOTALS OF Top Category in 2020 Year - to compare with the other years Totals
//                    if (lstPrdCateg.Count != 0)
//                    {

//                        foreach (var catbudget in categoryBudgets)
//                        {
//                            foreach (RequestItems_Table_TEST item in reqList_forquery)
//                            {
//                                if (item.SHAppDate.ToString().Contains((int.Parse(yr) - 1).ToString()))   // 2020 sh apprvd - 2021 planning
//                                {
//                                    // if ((int.Parse(yr) - 1).ToString() == "2020")
//                                    if (BaseYear_toCompare == 1 | (BaseYear_toCompare == 2 && Top70_CategoryBudget.Count() == 0))
//                                    {

//                                        if (int.Parse(item.Category) == catbudget.CategoryID)
//                                        {


//                                            if (item.ApprCost != null)
//                                            {

//                                                catbudget.CategoryTotals += (decimal)item.ApprCost;
//                                            }


//                                        }

//                                    }



//                                }

//                            }
//                        }
//                        Top70_CategoryBudget = categoryBudgets.OrderByDescending(x => x.CategoryTotals).Take(70).ToList();

//                    }

//                    //Code to get the Planned and Utilized Category Totals
//                    int catcount = -1;
//                    foreach (var topcat in Top70_CategoryBudget)
//                    {
//                        catcount++;
//                        foreach (RequestItems_Table_TEST item in reqList_forquery)
//                        {
//                            if (item.SHAppDate.ToString().Contains((int.Parse(yr) - 1).ToString()))
//                            {
//                                if (int.Parse(item.Category) == topcat.CategoryID)
//                                {
//                                    P_Category_Totals[catcount] += (decimal)item.ApprCost;
//                                    U_Category_Totals[catcount] += item.OrderPrice == null ? (decimal)0.0 : (decimal)item.OrderPrice;

//                                }
//                            }
//                        }
//                    }

//                    tempobj.P_Category_Totals = P_Category_Totals;
//                    tempobj.U_Category_Totals = U_Category_Totals;

//                    viewList.Add(tempobj);

//                }




//                ///NEW CODE TO GET CATEGORY SUMMARY
//                var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
//                BudgetParam t = new BudgetParam();
//                List<columnsinfo> _col = new List<columnsinfo>();
//                System.Data.DataTable dt = new System.Data.DataTable();
//                dt.Columns.Add("Category Name", typeof(string)); /// Planned + (year variable) / Utilized  + (year variable)
//                //dt.Columns.Add("VKM 2020", typeof(string));
//                foreach (string year in years)
//                {
//                    dt.Columns.Add("VKM" + " " + year + "(Planned)", typeof(string)); //add vkm text to yr
//                    dt.Columns.Add("VKM" + " " + year + "(Utilized)", typeof(string));

//                }


//                //DataRow dr = dt.NewRow();
//                //dr[0] = "MAE";
//                //int rcnt = 1;
//                //int rcnt1 = 2; //2
//                for (var i = 0; i < Top70_CategoryBudget.Count(); i++)
//                {
//                    DataRow dr = dt.NewRow();
//                    dr[0] = Top70_CategoryBudget[i].CategoryName.Trim().Replace("\r\n", "").Replace("\r", "").Replace("\r", "");
//                    int rcnt = 1;
//                    int rcnt1 = 2;
//                    foreach (var info in viewList)
//                    {



//                        dr[rcnt] = info.P_Category_Totals[i].ToString("C", CultureInfo.CurrentCulture);
//                        dr[rcnt1] = info.U_Category_Totals[i];
//                        //rcnt++;
//                        rcnt = rcnt + 2;
//                        rcnt1 = rcnt1 + 2;

//                    }
//                    dt.Rows.Add(dr);

//                }
//                //dt.Rows.Add(dr);
//                // dt.Rows.Add(dr);

//                //dr = dt.NewRow();
//                // dr[0] = "Non-MAE";


//                // dt.Rows.Add(dr);
//                for (int i = 0; i <= dt.Columns.Count - 1; i++)
//                {
//                    _col.Add(new columnsinfo { title = dt.Columns[i].ColumnName, data = dt.Columns[i].ColumnName });
//                }

//                string col = (string)serializer.Serialize(_col);
//                t.columns = col;


//                var lst = dt.AsEnumerable()
//                .Select(r => r.Table.Columns.Cast<DataColumn>()
//                        .Select(c => new KeyValuePair<string, object>(c.ColumnName, r[c.Ordinal])
//                       ).ToDictionary(z => z.Key, z => z.Value)
//                ).ToList();

//                string data = serializer.Serialize(lst);
//                t.data = data;




//                return Json(new { data = t, message = selected_ccxc }, JsonRequestBehavior.AllowGet);

//                // return View();

//            }
//        }





//        ///// <summary>
//        ///// Function to send BU summary data to view
//        ///// </summary>
//        ///// <returns></returns> 
//        //public ActionResult CostElementDrillDown(string Year_isPO, string CostElement_Chosen, List<string> buList, string selected_ccxc = "")
//        //{


//        //    using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

//        //    //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

//        //    {
//        //        List<CategoryBudget> categoryBudgets = new List<CategoryBudget>();
//        //        List<RequestItems_Table_TEST> reqList = new List<RequestItems_Table_TEST>();


//        //        foreach (var category in lstPrdCateg)
//        //        {
//        //            CategoryBudget catbud = new CategoryBudget();
//        //            catbud.CategoryID = category.ID;
//        //            catbud.CategoryName = category.Category;
//        //            categoryBudgets.Add(catbud);
//        //        }
//        //        //categoryBudgets will comprise of all the Categories - ID and Name
//        //        //Year_isPO - 
//        //        string YearX = Year_isPO.Split('(')[0];
//        //        int Year = int.Parse(YearX.Substring(4));
//        //        var is_PO = Year_isPO.Split('(')[1];
//        //        decimal OverallTotals = 0;
//        //        string costelement_chosen_id = string.Empty;
//        //        List<CategoryBudget> Whole_CategoryBudget = new List<CategoryBudget>();
//        //        List<CategoryBudget> Top10_CategoryBudget = new List<CategoryBudget>();


//        //        //if CostElement_Chosen is costelt name -> fetch its id
//        //        costelement_chosen_id = lstCostElement.Find(cost => cost.CostElement.ToUpper().Trim() == CostElement_Chosen.ToUpper().Trim()).ID.ToString().Trim();
//        //        //else costelement_chosen_id = CostElement_Chosen

//        //        foreach (var bu_item in buList)
//        //        {
//        //            //reqList_forquery ;
//        //            reqList.AddRange(db.RequestItems_Table_TEST.Where(item => item.CostElement.Trim() == costelement_chosen_id).ToList().FindAll(ss => ss.BU.Contains(bu_item)/*(lstBUs.Find(bu => bu.BU.Trim().Equals(bu_item.Trim())).ID.ToString())*/));
//        //        }


//        //            if (Year != 2020 && !(Year == 2021 && is_PO.Contains("Planned")))  //for 2020 reqList_forquery has relevant data 2021planned - no check 2021utili - check 2022plan check
//        //            {
//        //                string is_CCXC = string.Empty;
//        //                bool CCXCflag = false;
//        //                string presentUserDept = string.Empty;
//        //                if (selected_ccxc != "") //DATA FILTER BASED ON CCXC DROPDOWM - NOT USER'S NTID
//        //                {
//        //                    if (selected_ccxc.ToUpper().Contains("CC"))
//        //                    {
//        //                        is_CCXC = "CC";
//        //                        CCXCflag = true;
//        //                    }
//        //                    else if (selected_ccxc.ToUpper().Contains("XC"))
//        //                    {
//        //                        is_CCXC = "XC";
//        //                        CCXCflag = true;
//        //                    }

//        //                }
//        //                else                    //DATA FILTER BASED ON USER'S NTID
//        //                {
//        //                    presentUserDept = lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).Department;

//        //                    if (presentUserDept.ToUpper().Contains("CC"))
//        //                    {
//        //                        is_CCXC = "CC";
//        //                        CCXCflag = true;
//        //                    }
//        //                    else if (presentUserDept.ToUpper().Contains("XC"))
//        //                    {
//        //                        is_CCXC = "XC";
//        //                        CCXCflag = true;
//        //                    }

//        //                }

//        //                if (CCXCflag)
//        //                {
//        //                    if (is_CCXC.Contains("XC"))
//        //                    {
//        //                        reqList.AddRange(reqList.FindAll(ss => ss.BU.Contains(lstBUs.Find(bu => bu.BU.Contains("2WP")).ID.ToString())));
//        //                    }
//        //                    reqList = reqList.FindAll(dpt => lstDEPTs.Find(x => x.ID.ToString().Contains(dpt.DEPT)).DEPT.Contains(is_CCXC))/*.FindAll(ss => ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("AS")).ID.ToString()) || ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("2WP")).ID.ToString()))*/;
//        //                    CCXCflag = false;
//        //                }



//        //            }
//        //        if (is_PO.Contains("Planned"))
//        //        {
//        //            //use dhappdate == Year-1 check (since apprvd cost includes dh & sh)
//        //            //get all reqlist items with chosen cost elt & DHApprvDate = Year-1
//        //            reqList = reqList.Where(item=>item.ApprovalDH == true && item.DHAppDate != null).Where(item => item.DHAppDate.Value.Year == (Year-1)).OrderBy(item => item.Category).ToList()/*.FindAll(ss => ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("AS")).ID.ToString()) || ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("2WP")).ID.ToString()))*/;
//        //            foreach (var catbudget in categoryBudgets)
//        //            {
//        //                foreach (var item in reqList)
//        //                {
//        //                    if (int.Parse(item.Category) == catbudget.CategoryID)
//        //                    {
//        //                        if (item.ApprCost != null)
//        //                            catbudget.CategoryTotals += (decimal)item.ApprCost;
//        //                        OverallTotals += (decimal)item.ApprCost;
//        //                    }
//        //                }
//        //            }

//        //            Whole_CategoryBudget = categoryBudgets.OrderByDescending(x => x.CategoryTotals).ToList();
//        //            Top10_CategoryBudget = categoryBudgets.Take(10).ToList(); //check these categories in utilized

//        //        }
//        //        if (is_PO.Contains("Utilized"))
//        //        {
//        //            //use orderdate == Year check
//        //            //get all reqlist items with chosen cost elt & OrderDate = Year
//        //            reqList = reqList.Where(x=>x.OrderDate != null).Where(item => item.OrderDate.Value.Year == Year).OrderBy(item => item.Category).ToList();
//        //            foreach (var catbud in categoryBudgets)
//        //            {
//        //                foreach (var item in reqList)
//        //                {
//        //                    if (int.Parse(item.Category) == catbud.CategoryID)
//        //                    {
//        //                        if (item.OrderPrice != null)
//        //                            catbud.CategoryTotals += (decimal)item.OrderPrice;
//        //                    }
//        //                }
//        //            }

//        //            Whole_CategoryBudget = categoryBudgets.OrderByDescending(x => x.CategoryTotals).ToList();
//        //            Top10_CategoryBudget = categoryBudgets.Take(10).ToList();


//        //        }




//        //        return Json(new { data = Whole_CategoryBudget, success = true, message = selected_ccxc }, JsonRequestBehavior.AllowGet);


//        //    }
//        //}


//        public ActionResult CategoryDrillDown_comparison(List<string> years, string CostElement_Chosen, string Category_Chosen, List<string> buList, bool chart = false, string selected_ccxc = "")
//        {

//            if (lstUsers == null)
//            {
//                return RedirectToAction("Index", "Budgeting");
//            }


//            using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

//            //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

//            {
//                List<RequestItems_Table_TEST> reqList = new List<RequestItems_Table_TEST>();
//                List<RequestItems_Table_TEST> reqList_forquery = new List<RequestItems_Table_TEST>();
//                reqList = db.RequestItems_Table_TEST.ToList<RequestItems_Table_TEST>();
//                List<RequestItemsList_2019> reqList2019 = new List<RequestItemsList_2019>();
//                List<ItemBudget> itemBudgets = new List<ItemBudget>();
//                List<ItemBudget> Top_ItemBudget = new List<ItemBudget>();
//                List<category_drilldownSummary> viewList = new List<category_drilldownSummary>();
//                string costelement_chosen_id = string.Empty;
//                int BaseYear_toCompare = 0;

//                //CostElement_Chosen is costelt name -> fetch its id
//                costelement_chosen_id = lstCostElement.Find(cost => cost.CostElement.ToUpper().Trim() == CostElement_Chosen.ToUpper().Trim()).ID.ToString().Trim();

//                //Category_Chosen is costelt name -> fetch its id
//                Category_Chosen = lstPrdCateg.Find(cat=>cat.Category.ToUpper().Trim() == Category_Chosen.ToUpper().Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", "")).ID.ToString().Trim();

//                foreach (var item in lstItems.Where(x => x.Category.Contains(Category_Chosen.ToString().Trim())))
//                {
//                    ItemBudget ibud = new ItemBudget();
//                    ibud.ItemID = item.S_No;
//                    ibud.ItemName = item.Item_Name;


//                    if (itemBudgets.Count>0 && itemBudgets.FindAll(ib => ib.ItemName.Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", "") == (ibud.ItemName.Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", ""))).Count != 0)
//                        continue;

//                    itemBudgets.Add(ibud);
//                }
//                foreach (var bu_item in buList)
//                {

//                    reqList_forquery.AddRange(reqList.FindAll(item => item.CostElement.Trim() == costelement_chosen_id).FindAll(ss => ss.BU.Contains(bu_item)).FindAll(item => item.Category.Trim() == Category_Chosen.ToString().Trim()));

//                }

//                //CODE TO GET GROUPS OF COST ELEMENT


//                foreach (string yr in years)
//                {
//                    BaseYear_toCompare++;
//                    //CC XC CHECK
//                    //if(yr == "2020")
//                    //{
//                    //    reqList_forquery = reqList_forquery/*.FindAll(ss => ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("AS")).ID.ToString()) || ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("2WP")).ID.ToString()))*/;
//                    //}
//                    //else
//                    if ((int.Parse(yr) - 1) > 2020)  //for 2020 reqList_forquery has relevant data
//                    {
//                        string is_CCXC = string.Empty;
//                        bool CCXCflag = false;
//                        string presentUserDept = string.Empty;
//                        if (selected_ccxc != "") //DATA FILTER BASED ON CCXC DROPDOWM - NOT USER'S NTID
//                        {
//                            if (selected_ccxc.ToUpper().Contains("CC"))
//                            {
//                                is_CCXC = "CC";
//                                CCXCflag = true;
//                            }
//                            else if (selected_ccxc.ToUpper().Contains("XC"))
//                            {
//                                is_CCXC = "XC";
//                                CCXCflag = true;
//                            }

//                        }
//                        else                    //DATA FILTER BASED ON USER'S NTID
//                        {
//                            presentUserDept = lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).Department;

//                            if (presentUserDept.ToUpper().Contains("CC"))
//                            {
//                                is_CCXC = "CC";
//                                CCXCflag = true;
//                            }
//                            else if (presentUserDept.ToUpper().Contains("XC"))
//                            {
//                                is_CCXC = "XC";
//                                CCXCflag = true;
//                            }

//                        }

//                        if (CCXCflag)
//                        {
//                            if (is_CCXC.Contains("XC"))
//                            {
//                                reqList_forquery.AddRange(reqList.FindAll(ss => ss.BU.Contains(lstBUs.Find(bu => bu.BU.Contains("2WP")).ID.ToString())));
//                            }
//                            reqList_forquery = reqList_forquery.FindAll(dpt => lstDEPTs.Find(x => x.ID.ToString().Contains(dpt.DEPT)).DEPT.Contains(is_CCXC))/*.FindAll(ss => ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("AS")).ID.ToString()) || ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("2WP")).ID.ToString()))*/;
//                            CCXCflag = false;
//                        }



//                    }


//                    // decimal Category_Totals[10] = new decimal[10];
//                    decimal[] P_Item_Totals = new decimal[200];
//                    decimal[] U_Item_Totals = new decimal[200];
//                    category_drilldownSummary tempobj = new category_drilldownSummary();
//                    tempobj.vkmyear = yr;


//                        foreach (var itembudget in itemBudgets)
//                        {
//                            foreach (RequestItems_Table_TEST item in reqList_forquery)
//                            {
//                                if (item.SHAppDate.ToString().Contains((int.Parse(yr) - 1).ToString()))   // 2020 dh apprvd - 2021 planning
//                                {
//                                //if ((int.Parse(yr) - 1).ToString() == "2020")
//                                if (BaseYear_toCompare == 1 || (BaseYear_toCompare == 2 && Top_ItemBudget.Count() == 0))
//                                {

//                                        if (lstItems.Find(x => x.S_No == int.Parse(item.ItemName)).Item_Name.Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", "") == itembudget.ItemName.Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", ""))
//                                       // if (int.Parse(item.ItemName) == itembudget.ItemID)
//                                        {
//                                            if (item.ApprCost != null)
//                                                itembudget.ItemTotals += (decimal)item.ApprCost;

//                                        }

//                                    }

//                                }

//                            }
//                        }
//                        Top_ItemBudget = itemBudgets.OrderByDescending(x => x.ItemTotals).Take(200).ToList();


//                    int itemcount = -1;
//                    foreach (var topitem in Top_ItemBudget)
//                    {
//                        itemcount++;
//                        foreach (RequestItems_Table_TEST item in reqList_forquery)
//                        {
//                            if (item.SHAppDate.ToString().Contains((int.Parse(yr) - 1).ToString()))
//                            {
//                                if (lstItems.Find(x => x.S_No == int.Parse(item.ItemName)).Item_Name.Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", "") == topitem.ItemName.Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", ""))
//                                //if (int.Parse(item.ItemName) == topitem.ItemID)
//                                {
//                                    P_Item_Totals[itemcount] += (decimal)item.ApprCost;
//                                    U_Item_Totals[itemcount] += item.OrderPrice == null ? (decimal)0.0 : (decimal)item.OrderPrice;

//                                }


//                            }
//                        }
//                    }


//                    tempobj.P_Item_Totals = P_Item_Totals;
//                    tempobj.U_Item_Totals = U_Item_Totals;

//                    viewList.Add(tempobj);

//                }


//                var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
//                BudgetParam t = new BudgetParam();
//                List<columnsinfo> _col = new List<columnsinfo>();
//                System.Data.DataTable dt = new System.Data.DataTable();
//                dt.Columns.Add("Item Name", typeof(string)); /// Planned + (year variable) / Utilized  + (year variable)
//                //dt.Columns.Add("VKM 2020", typeof(string));
//                foreach (string year in years)
//                {
//                    dt.Columns.Add("VKM" + " " + year + "(Planned)", typeof(string)); //add vkm text to yr
//                    dt.Columns.Add("VKM" + " " + year + "(Utilized)", typeof(string));

//                }


//                //DataRow dr = dt.NewRow();
//                //dr[0] = "MAE";
//                //int rcnt = 1;
//                //int rcnt1 = 2; //2
//                for (var i = 0; i < Top_ItemBudget.Count(); i++)
//                {
//                    DataRow dr = dt.NewRow();
//                    dr[0] = Top_ItemBudget[i].ItemName.Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", "");
//                    int rcnt = 1;
//                    int rcnt1 = 2;
//                    foreach (var info in viewList)
//                    {



//                        dr[rcnt] = info.P_Item_Totals[i].ToString("C", CultureInfo.CurrentCulture);
//                        dr[rcnt1] = info.U_Item_Totals[i];
//                        //rcnt++;
//                        rcnt = rcnt + 2;
//                        rcnt1 = rcnt1 + 2;

//                    }
//                    dt.Rows.Add(dr);

//                }
//                //dt.Rows.Add(dr);
//                // dt.Rows.Add(dr);

//                //dr = dt.NewRow(); 
//                // dr[0] = "Non-MAE";


//                // dt.Rows.Add(dr);
//                for (int i = 0; i <= dt.Columns.Count - 1; i++)
//                {
//                    _col.Add(new columnsinfo { title = dt.Columns[i].ColumnName, data = dt.Columns[i].ColumnName });
//                }

//                string col = (string)serializer.Serialize(_col);
//                t.columns = col;


//                var lst = dt.AsEnumerable()
//                .Select(r => r.Table.Columns.Cast<DataColumn>()
//                        .Select(c => new KeyValuePair<string, object>(c.ColumnName, r[c.Ordinal])
//                       ).ToDictionary(z => z.Key, z => z.Value)
//                ).ToList();

//                string data = serializer.Serialize(lst);
//                t.data = data;




//                return Json(new { data = t, message = selected_ccxc }, JsonRequestBehavior.AllowGet);

//            }
//        }




//        ///// <summary>
//        ///// Function to send BU summary data to view
//        ///// </summary>
//        ///// <returns></returns> 
//        //public ActionResult CategoryDrillDown(string Year_isPO, string CostElement_Chosen, int Category_Chosen, List<string> buList, string selected_ccxc = "")
//        //{


//        //    using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

//        //    //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

//        //    {
//        //        List<ItemBudget> itemBudgets = new List<ItemBudget>();
//        //        List<RequestItems_Table_TEST> reqList = new List<RequestItems_Table_TEST>();


//        //        foreach (var item in lstItems.Where(x => x.Category.Contains(Category_Chosen.ToString().Trim())))
//        //        {
//        //            ItemBudget ibud = new ItemBudget();
//        //            ibud.ItemID = item.S_No;
//        //            ibud.ItemName = item.Item_Name;
//        //            itemBudgets.Add(ibud);
//        //        }
//        //        //categoryBudgets will comprise of all the Categories - ID and Name
//        //        //Year_isPO - 
//        //        string YearX = Year_isPO.Split('(')[0];
//        //        int Year = int.Parse(YearX.Substring(4));
//        //        var is_PO = Year_isPO.Split('(')[1];
//        //        decimal OverallItemTotals = 0;
//        //        string costelement_chosen_id = string.Empty;
//        //        List<ItemBudget> Whole_ItemBudget = new List<ItemBudget>();
//        //        List<ItemBudget> Top10_ItemBudget = new List<ItemBudget>();


//        //        costelement_chosen_id = lstCostElement.Find(cost => cost.CostElement.ToUpper().Trim() == CostElement_Chosen.ToUpper().Trim()).ID.ToString().Trim();

//        //        foreach (var bu_item in buList)
//        //        {
//        //            //reqList_forquery ;
//        //            reqList.AddRange(db.RequestItems_Table_TEST.Where(item => item.CostElement.Trim() == costelement_chosen_id).ToList().FindAll(ss => ss.BU.Contains(bu_item)).Where(item => item.Category.Trim() == Category_Chosen.ToString().Trim()));
//        //        }

//        //        var zzz = Year != 2020;
//        //        var aaa = !(Year == 2021 && is_PO.Contains("Planned"));
//        //        var xxx = Year != 2020 && !(Year == 2021 && is_PO.Contains("Planned"));
//        //        if (Year != 2020 && !(Year == 2021 && is_PO.Contains("Planned")))   //for 2020 reqList_forquery has relevant data
//        //        {
//        //            string is_CCXC = string.Empty;
//        //            bool CCXCflag = false;
//        //            string presentUserDept = string.Empty;
//        //            if (selected_ccxc != "") //DATA FILTER BASED ON CCXC DROPDOWM - NOT USER'S NTID
//        //            {
//        //                if (selected_ccxc.ToUpper().Contains("CC"))
//        //                {
//        //                    is_CCXC = "CC";
//        //                    CCXCflag = true;
//        //                }
//        //                else if (selected_ccxc.ToUpper().Contains("XC"))
//        //                {
//        //                    is_CCXC = "XC";
//        //                    CCXCflag = true;
//        //                }

//        //            }
//        //            else                    //DATA FILTER BASED ON USER'S NTID
//        //            {
//        //                presentUserDept = lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).Department;

//        //                if (presentUserDept.ToUpper().Contains("CC"))
//        //                {
//        //                    is_CCXC = "CC";
//        //                    CCXCflag = true;
//        //                }
//        //                else if (presentUserDept.ToUpper().Contains("XC"))
//        //                {
//        //                    is_CCXC = "XC";
//        //                    CCXCflag = true;
//        //                }

//        //            }

//        //            if (CCXCflag)
//        //            {
//        //                if (is_CCXC.Contains("XC"))
//        //                {
//        //                    reqList.AddRange(reqList.FindAll(ss => ss.BU.Contains(lstBUs.Find(bu => bu.BU.Contains("2WP")).ID.ToString())));
//        //                }
//        //                reqList = reqList.FindAll(dpt => lstDEPTs.Find(x => x.ID.ToString().Contains(dpt.DEPT)).DEPT.Contains(is_CCXC))/*.FindAll(ss => ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("AS")).ID.ToString()) || ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("2WP")).ID.ToString()))*/;
//        //                CCXCflag = false;
//        //            }



//        //        }

//        //        if (is_PO.Contains("Planned"))
//        //        {
//        //            //use dhappdate == Year-1 check (since apprvd cost includes dh & sh)
//        //            //get all reqlist items with chosen cost elt & DHApprvDate = Year-1
//        //            reqList = reqList.Where(x => x.DHAppDate != null).Where(item => item.DHAppDate.Value.Year == (Year-1)).OrderBy(item => item.ItemName).ToList()/*.FindAll(ss => ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("AS")).ID.ToString()) || ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("2WP")).ID.ToString()))*/;
//        //            foreach (var itembudget in itemBudgets)
//        //            {
//        //                foreach (var item in reqList)
//        //                {
//        //                    if (int.Parse(item.ItemName) == itembudget.ItemID)
//        //                    {
//        //                        if (item.ApprCost != null)
//        //                            itembudget.ItemTotals += (decimal)item.ApprCost;
//        //                        OverallItemTotals += (decimal)item.ApprCost;
//        //                    }
//        //                }
//        //            }

//        //            Whole_ItemBudget = itemBudgets.OrderByDescending(x => x.ItemTotals).ToList();
//        //            Top10_ItemBudget = itemBudgets.Take(10).ToList();

//        //        }
//        //        if (is_PO.Contains("Utilized"))
//        //        {
//        //            //use orderdate == Year check
//        //            //get all reqlist items with chosen cost elt & OrderDate = Year
//        //            reqList = reqList.Where(x => x.OrderDate != null).Where(item => item.OrderDate.Value.Year == Year).OrderBy(item => item.Category).ToList();
//        //            foreach (var catbud in itemBudgets)
//        //            {
//        //                foreach (var item in reqList)
//        //                {
//        //                    if (int.Parse(item.ItemName) == catbud.ItemID)
//        //                    {
//        //                        if (item.OrderPrice != null)
//        //                            catbud.ItemTotals += (decimal)item.OrderPrice;
//        //                    }
//        //                }
//        //            }

//        //            Whole_ItemBudget = itemBudgets.OrderByDescending(x => x.ItemTotals).ToList();
//        //            Top10_ItemBudget = itemBudgets.Take(10).ToList();


//        //        }




//        //        return Json(new { data = Whole_ItemBudget, success = true, message = selected_ccxc }, JsonRequestBehavior.AllowGet);


//        //    }
//        //}


//        public ActionResult BUs_forpresentNTID_CCXC(bool ccxc = false, string selected_ccxc = "", string Ajax_call_made_for_BuList_check = "")
//        {
//            string presentNTID = User.Identity.Name.Split('\\')[1].ToUpper();
//            List<string> allowedBUs = new List<string>();
//            List<string> cc = new List<string>()
//                { "MB", "2WP", "OSS"};
//            List<string> xc = new List<string>()
//                { "DA", "AD"};
//            string presentUserDept = string.Empty;

//            if (ccxc == true)
//            {
//                if(selected_ccxc.ToUpper().Trim().Contains("CC"))
//                {
//                    foreach(var bu_cc in cc)
//                    {
//                         allowedBUs.Add(lstBUs.Find(bu => bu.BU.Contains(bu_cc)).ID.ToString());
//                    }
//                }
//                else
//                {
//                    foreach (var bu_xc in xc)
//                    {
//                        allowedBUs.Add(lstBUs.Find(bu => bu.BU.Contains(bu_xc)).ID.ToString());
//                    }
//                }
//            }
//            else
//            {

//                if (lstBU_SPOCs.Find(person => person.VKMspoc.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())) != null)
//                {
//                    var VKMSPOC = lstBU_SPOCs.FindAll(e => e.VKMspoc.ToUpper().Trim().Equals(User.Identity.Name.Split('\\')[1].ToUpper()));
//                    foreach (var i in VKMSPOC)
//                    {
//                        allowedBUs.Add(i.BU.ToString());
//                    }



//                }
//                else if (lstPrivileged.Find(person => person.ADSID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())) != null)
//                {
//                    var BU_of_VKMSPOC = lstPrivileged.Find(e => e.ADSID.ToUpper().Trim().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).BU;
//                    if (BU_of_VKMSPOC != null)
//                    {
//                        allowedBUs = (BU_of_VKMSPOC.Split(',')).ToList();

//                    }
//                }
//                else
//                {
//                    presentUserDept = lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).Department;

//                    if (presentUserDept.ToUpper().Contains("CC"))
//                    {
//                        foreach (var bu_cc in cc)
//                        {
//                            allowedBUs.Add(lstBUs.Find(bu => bu.BU.Contains(bu_cc)).ID.ToString());
//                        }
//                    }
//                    else if (presentUserDept.ToUpper().Contains("XC"))
//                    {
//                        foreach (var bu_xc in xc)
//                        {
//                            allowedBUs.Add(lstBUs.Find(bu => bu.BU.Contains(bu_xc)).ID.ToString());
//                        }
//                    }
//                }

//            }
//           if(Ajax_call_made_for_BuList_check == string.Empty)
//                return Json(new { data = allowedBUs, message = selected_ccxc, success = true}, JsonRequestBehavior.AllowGet);
//           else
//                return Json(new { data = allowedBUs, message = Ajax_call_made_for_BuList_check, success = true }, JsonRequestBehavior.AllowGet);

//        }


//        [HttpGet]
//        public ActionResult ItemID(string Item_Chosen)
//        {

//            return Json(new { data = lstItems.Find(x => x.Item_Name.Trim().ToUpper() == Item_Chosen.Trim().ToUpper()).S_No }, JsonRequestBehavior.AllowGet);

//        }

//        //[HttpGet]
//        //public ActionResult CategoryName(int Category_Chosen)
//        //{

//        //    return Json(new { data = lstPrdCateg.Find(x => x.ID == Category_Chosen).Category }, JsonRequestBehavior.AllowGet);

//        //}


//public ActionResult ItemDrillDown_Index()
//{
//    return View();
//}

//[HttpPost]
//public ActionResult ItemDrillDown( /*string Years_Listasstring,*/ string CostElement_Chosen, int Category_Chosen, int Item_Chosen, List<string> buList, List<string> years,/*string BuList_Listasstring,*/ string selected_ccxc = "")
//{

//    //List<string> years = new List<string>();
//    //List<string> buList = new List<string>();
//    string costelement_chosen_id = string.Empty;

//    //RequestListAttributes viewList = new 


//    List<RequestItemsRepoView> viewList_itemdrilldown1 = new List<RequestItemsRepoView>();
//    //if (Years_Listasstring != null)
//    //{
//    //    years = (Years_Listasstring.Split(',')).ToList();

//    //}
//    //if (BuList_Listasstring != null)
//    //{
//    //    buList = (BuList_Listasstring.Split(',')).ToList();

//    //}
//    //if CostElement_Chosen is costelt name -> fetch its id
//    costelement_chosen_id = lstCostElement.Find(cost => cost.CostElement.ToUpper().Trim() == CostElement_Chosen.ToUpper().Trim()).ID.ToString().Trim();


//    using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

//    //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

//    {
//        List<RequestItems_Table_TEST> reqList = new List<RequestItems_Table_TEST>();



//        foreach (var bu_item in buList)
//        {
//            //reqList_forquery ;
//            reqList.AddRange(db.RequestItems_Table_TEST.ToList<RequestItems_Table_TEST>().FindAll(item => item.CostElement.Trim() == costelement_chosen_id).FindAll(ss => ss.BU.Contains(bu_item)).FindAll(item => item.Category.Trim() == Category_Chosen.ToString().Trim()).FindAll(z => z.ItemName.Contains(Item_Chosen.ToString())));

//        }

//        foreach (string yr in years)
//        {
//            reqList = reqList.FindAll(z => z.SubmitDate.Value.Year.ToString() == yr.Trim());
//            //CC XC CHECK
//            //if(yr == "2020")
//            //{
//            //    reqList_forquery = reqList_forquery/*.FindAll(ss => ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("AS")).ID.ToString()) || ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("2WP")).ID.ToString()))*/;
//            //}
//            //else
//            if (yr != "2020")  //for 2020 reqList_forquery has relevant data
//            {
//                string is_CCXC = string.Empty;
//                bool CCXCflag = false;
//                string presentUserDept = string.Empty;
//                if (selected_ccxc != "") //DATA FILTER BASED ON CCXC DROPDOWM - NOT USER'S NTID
//                {
//                    if (selected_ccxc.ToUpper().Contains("CC"))
//                    {
//                        is_CCXC = "CC";
//                        CCXCflag = true;
//                    }
//                    else if (selected_ccxc.ToUpper().Contains("XC"))
//                    {
//                        is_CCXC = "XC";
//                        CCXCflag = true;
//                    }

//                }
//                else                    //DATA FILTER BASED ON USER'S NTID
//                {
//                    presentUserDept = lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).Department;

//                    if (presentUserDept.ToUpper().Contains("CC"))
//                    {
//                        is_CCXC = "CC";
//                        CCXCflag = true;
//                    }
//                    else if (presentUserDept.ToUpper().Contains("XC"))
//                    {
//                        is_CCXC = "XC";
//                        CCXCflag = true;
//                    }

//                }

//                if (CCXCflag)
//                {
//                    if (is_CCXC.Contains("XC"))
//                    {
//                        reqList = reqList.FindAll(ss => ss.BU.Contains(lstBUs.Find(bu => bu.BU.Contains("2WP")).ID.ToString()));
//                    }
//                    reqList = reqList.FindAll(dpt => lstDEPTs.Find(x => x.ID.ToString().Contains(dpt.DEPT)).DEPT.Contains(is_CCXC))/*.FindAll(ss => ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("AS")).ID.ToString()) || ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("2WP")).ID.ToString()))*/;
//                    CCXCflag = false;
//                }



//            }
//            //reqList = db.RequestItems_Table_TEST.ToList<RequestItems_Table_TEST>().FindAll(z=>z.CostElement.Contains(costelement_chosen_id.Trim())).FindAll(z=>z.Category.Contains(Category_Chosen.ToString())).FindAll(z=>z.ItemName.Contains(Item_Chosen.ToString()))/*.FindAll(x=>x.ApprovalDH == false)*/;

//            db.Database.CommandTimeout = 500;


//            foreach (RequestItems_Table_TEST item in reqList)
//            {

//                RequestItemsRepoView ritem = new RequestItemsRepoView();

//                ritem.Category = int.Parse(item.Category);
//                ritem.Comments = item.Comments;
//                ritem.Cost_Element = int.Parse(item.CostElement);
//                ritem.BU = int.Parse(item.BU);

//                ritem.DEPT = int.Parse(item.DEPT);
//                ritem.Group = int.Parse(item.Group);
//                ritem.Item_Name = int.Parse(item.ItemName);
//                ritem.OEM = int.Parse(item.OEM);
//                ritem.Required_Quantity = item.ReqQuantity;
//                ritem.RequestID = item.RequestID;

//                ritem.Requestor = item.RequestorNT;
//                ritem.Total_Price = item.TotalPrice;
//                ritem.Unit_Price = item.UnitPrice;
//                ritem.ApprovalHoE = item.ApprovalDH;
//                ritem.ApprovalSH = item.ApprovalSH;
//                ritem.ApprovedHoE = item.ApprovedDH;
//                ritem.ApprovedSH = item.ApprovedSH;
//                if (item.isCancelled != null)
//                {
//                    ritem.isCancelled = (int)item.isCancelled;
//                }


//                ritem.Total_Price = item.TotalPrice;
//                ritem.Reviewer_1 = item.DHNT;
//                ritem.Reviewer_2 = item.SHNT;

//                ritem.RequestDate = item.RequestDate != null ? ((DateTime)item.RequestDate).ToString("yyyy-MM-dd") : string.Empty;
//                ritem.SubmitDate = item.SubmitDate != null ? ((DateTime)item.SubmitDate).ToString("yyyy-MM-dd") : string.Empty;
//                ritem.Review1_Date = item.DHAppDate != null ? ((DateTime)item.DHAppDate).ToString("yyyy-MM-dd") : string.Empty;
//                ritem.Review2_Date = item.SHAppDate != null ? ((DateTime)item.SHAppDate).ToString("yyyy-MM-dd") : string.Empty;


//                if (item.OrderStatus != null && item.OrderStatus.Trim() != "")
//                {
//                    ritem.OrderStatus = int.Parse(item.OrderStatus);

//                }
//                else
//                {
//                    ritem.OrderStatus = null;


//                }

//                //Checking Request Status
//                if (ritem.ApprovalHoE == true && ritem.ApprovalSH == false)
//                {
//                    ritem.Request_Status = "In Review with HoE";
//                }
//                else if (ritem.ApprovalHoE == true && ritem.ApprovalSH == true && ritem.ApprovedSH == false)
//                {
//                    ritem.Request_Status = "In Review with VKM SPOC";
//                }
//                else if (ritem.ApprovalHoE == true && ritem.ApprovalSH == true && ritem.ApprovedSH == true)
//                {
//                    ritem.Request_Status = "Reviewed by VKM SPOC";
//                }
//                else if (ritem.ApprovalHoE == false && ritem.ApprovalSH == false && ritem.isCancelled == 1)
//                {
//                    ritem.Request_Status = "SentBack by HoE";
//                }
//                else if (ritem.ApprovalHoE == false && ritem.ApprovalSH == false && ritem.isCancelled == 2)
//                {
//                    ritem.Request_Status = "SentBack by VKM SPOC";
//                }
//                else
//                {
//                    ritem.Request_Status = "In Requestor's Queue";
//                }











//                viewList_itemdrilldown1.Add(ritem);


//            }
//        }
//        // return viewList.FindAll(xi => xi.RequestDate.ToString().Contains(year))/*.FindAll(x => x.ApprovalHoE == false)*/;


//    }
//    viewList_itemdrilldown = viewList_itemdrilldown1;
//    return Json(new { data = viewList_itemdrilldown1 }, JsonRequestBehavior.AllowGet);

//}

//        //public ActionResult GetData_ItemDrillDown()
//        //{

//        //    return Json(new { data = viewList_itemdrilldown }, JsonRequestBehavior.AllowGet);

//        //}



//        //[HttpPost]
//        //[HttpPost]
//        public ActionResult ItemDrillDown_Index(string Years_Listasstring, string CostElement_Chosen, string Category_Chosen, string Item_Chosen/*, List<string> buList, List<string> years*/, string BuList_Listasstring, string selected_ccxc = "")
//        {

//            List<string> years = new List<string>();
//            List<string> buList = new List<string>();
//            string costelement_chosen_id = string.Empty;

//            RequestListAttributes viewList_itemdrilldown1 = new RequestListAttributes()
//            {
//                RequestItemsRepoView_model = new List<RequestItemsRepoView>()
//            };

//            if (Years_Listasstring != null)
//            {
//                years = (Years_Listasstring.Split(',')).ToList();

//            }
//            if (BuList_Listasstring != null)
//            {
//                buList = (BuList_Listasstring.Split(',')).ToList();

//            }
//            //if CostElement_Chosen is costelt name -> fetch its id
//            costelement_chosen_id = lstCostElement.Find(cost => cost.CostElement.ToUpper().Trim() == CostElement_Chosen.ToUpper().Trim()).ID.ToString().Trim();
//            Category_Chosen = lstPrdCateg.Find(cat => cat.Category.ToUpper().Trim() == Category_Chosen.ToUpper().Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", "")).ID.ToString().Trim();
//            Item_Chosen = lstItems.Find(item => item.S_No == int.Parse(Item_Chosen)).Item_Name;




//            using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

//            //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

//            {
//                List<RequestItems_Table_TEST> reqList = new List<RequestItems_Table_TEST>();
//                List<RequestItems_Table_TEST> reqList1 = new List<RequestItems_Table_TEST>();



//                foreach (var bu_item in buList)
//                {
//                    //reqList_forquery ;
//                    reqList.AddRange(db.RequestItems_Table_TEST.ToList<RequestItems_Table_TEST>().FindAll(item => item.ApprovedSH == true).FindAll(item => item.CostElement.Trim() == costelement_chosen_id).FindAll(ss => ss.BU.Contains(bu_item)).FindAll(item => item.Category.Trim() == Category_Chosen).FindAll(item => lstItems.Find(i => i.S_No.ToString().Trim() == item.ItemName.Trim()).Item_Name.ToUpper().Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", "").Replace("+", "").Replace(" ", string.Empty) == Item_Chosen.ToUpper().Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", "").Replace("+", "").Replace(" ", string.Empty)));//lstItems.Find(item => item.Item_Name.ToUpper().Trim() == Item_Chosen.ToUpper().Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", "")).S_No.ToString().Trim();



//                }

//                foreach (string yr in years)
//                {
//                    if (reqList.FindAll(z => z.ApprovedSH == true && z.SubmitDate.Value.Year.ToString() == (int.Parse(yr) - 1).ToString().Trim()).Count != 0)
//                        reqList1 = reqList.FindAll(z => z.SubmitDate.Value.Year.ToString() == (int.Parse(yr) - 1).ToString().Trim());
//                    else
//                        continue;
//                    //CC XC CHECK
//                    //if(yr == "2020")
//                    //{
//                    //    reqList_forquery = reqList_forquery/*.FindAll(ss => ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("AS")).ID.ToString()) || ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("2WP")).ID.ToString()))*/;
//                    //}
//                    //else
//                    if ((int.Parse(yr) - 1) > 2020)  //for 2020 reqList_forquery has relevant data
//                    {
//                        string is_CCXC = string.Empty;
//                        bool CCXCflag = false;
//                        string presentUserDept = string.Empty;
//                        if (selected_ccxc != "") //DATA FILTER BASED ON CCXC DROPDOWM - NOT USER'S NTID
//                        {
//                            if (selected_ccxc.ToUpper().Contains("CC"))
//                            {
//                                is_CCXC = "CC";
//                                CCXCflag = true;
//                            }
//                            else if (selected_ccxc.ToUpper().Contains("XC"))
//                            {
//                                is_CCXC = "XC";
//                                CCXCflag = true;
//                            }

//                        }
//                        else                    //DATA FILTER BASED ON USER'S NTID
//                        {
//                            presentUserDept = lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).Department;

//                            if (presentUserDept.ToUpper().Contains("CC"))
//                            {
//                                is_CCXC = "CC";
//                                CCXCflag = true;
//                            }
//                            else if (presentUserDept.ToUpper().Contains("XC"))
//                            {
//                                is_CCXC = "XC";
//                                CCXCflag = true;
//                            }

//                        }

//                        if (CCXCflag)
//                        {
//                            if (is_CCXC.Contains("XC"))
//                            {
//                                reqList1 = reqList.FindAll(ss => ss.BU.Contains(lstBUs.Find(bu => bu.BU.Contains("2WP")).ID.ToString()));
//                            }
//                            reqList1 = reqList.FindAll(dpt => lstDEPTs.Find(x => x.ID.ToString().Contains(dpt.DEPT)).DEPT.Contains(is_CCXC))/*.FindAll(ss => ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("AS")).ID.ToString()) || ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("2WP")).ID.ToString()))*/;
//                            CCXCflag = false;
//                        }



//                    }
//                    //reqList = db.RequestItems_Table_TEST.ToList<RequestItems_Table_TEST>().FindAll(z=>z.CostElement.Contains(costelement_chosen_id.Trim())).FindAll(z=>z.Category.Contains(Category_Chosen.ToString())).FindAll(z=>z.ItemName.Contains(Item_Chosen.ToString()))/*.FindAll(x=>x.ApprovalDH == false)*/;

//                    db.Database.CommandTimeout = 500;


//                    foreach (RequestItems_Table_TEST item in reqList1)
//                    {

//                        RequestItemsRepoView ritem = new RequestItemsRepoView();

//                        ritem.Category = int.Parse(item.Category);
//                        ritem.Comments = item.Comments;
//                        ritem.Cost_Element = int.Parse(item.CostElement);
//                        ritem.BU = int.Parse(item.BU);

//                        ritem.DEPT = int.Parse(item.DEPT);
//                        ritem.Group = int.Parse(item.Group);
//                        ritem.Item_Name = int.Parse(item.ItemName);
//                        ritem.OEM = int.Parse(item.OEM);
//                        ritem.Required_Quantity = item.ReqQuantity;
//                        ritem.Reviewed_Quantity = item.ApprQuantity;
//                        ritem.RequestID = item.RequestID;

//                        ritem.Requestor = item.RequestorNT;
//                        ritem.Total_Price = item.TotalPrice;
//                        ritem.Unit_Price = item.UnitPrice;
//                        ritem.ApprovalHoE = item.ApprovalDH;
//                        ritem.ApprovalSH = item.ApprovalSH;
//                        ritem.ApprovedHoE = item.ApprovedDH;
//                        ritem.ApprovedSH = item.ApprovedSH;
//                        if (item.isCancelled != null)
//                        {
//                            ritem.isCancelled = (int)item.isCancelled;
//                        }


//                        ritem.Total_Price = item.TotalPrice;
//                        ritem.Reviewed_Cost = item.ApprCost;
//                        ritem.OrderedQuantity = item.OrderedQuantity;
//                        ritem.OrderPrice = item.OrderPrice;

//                        if (item.OrderStatus != null && item.OrderStatus.Trim() != "")
//                        {
//                            ritem.OrderStatus = int.Parse(item.OrderStatus);

//                        }
//                        else
//                        {
//                            ritem.OrderStatus = null;


//                        }
//                        ritem.OrderID = item.OrderID;
//                        ritem.Reviewer_1 = item.DHNT;
//                        ritem.Reviewer_2 = item.SHNT;

//                        ritem.RequestDate = item.RequestDate != null ? ((DateTime)item.RequestDate).ToString("yyyy-MM-dd") : string.Empty;
//                        ritem.SubmitDate = item.SubmitDate != null ? ((DateTime)item.SubmitDate).ToString("yyyy-MM-dd") : string.Empty;
//                        ritem.Review1_Date = item.DHAppDate != null ? ((DateTime)item.DHAppDate).ToString("yyyy-MM-dd") : string.Empty;
//                        ritem.Review2_Date = item.SHAppDate != null ? ((DateTime)item.SHAppDate).ToString("yyyy-MM-dd") : string.Empty;


//                        if (item.OrderStatus != null && item.OrderStatus.Trim() != "")
//                        {
//                            ritem.OrderStatus = int.Parse(item.OrderStatus);

//                        }
//                        else
//                        {
//                            ritem.OrderStatus = null;


//                        }

//                        //Checking Request Status
//                        if (ritem.ApprovalHoE == true && ritem.ApprovalSH == false)
//                        {
//                            ritem.Request_Status = "In Review with HoE";
//                        }
//                        else if (ritem.ApprovalHoE == true && ritem.ApprovalSH == true && ritem.ApprovedSH == false)
//                        {
//                            ritem.Request_Status = "In Review with VKM SPOC";
//                        }
//                        else if (ritem.ApprovalHoE == true && ritem.ApprovalSH == true && ritem.ApprovedSH == true)
//                        {
//                            ritem.Request_Status = "Reviewed by VKM SPOC";
//                        }
//                        else if (ritem.ApprovalHoE == false && ritem.ApprovalSH == false && ritem.isCancelled == 1)
//                        {
//                            ritem.Request_Status = "SentBack by HoE";
//                        }
//                        else if (ritem.ApprovalHoE == false && ritem.ApprovalSH == false && ritem.isCancelled == 2)
//                        {
//                            ritem.Request_Status = "SentBack by VKM SPOC";
//                        }
//                        else
//                        {
//                            ritem.Request_Status = "In Requestor's Queue";
//                        }









//                        viewList_itemdrilldown1.RequestItemsRepoView_model.Add(ritem);
//                        viewList_itemdrilldown1.Item_Name = Item_Chosen.Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", "");
//                        // viewList_itemdrilldown1.Add(ritem);


//                    }
//                }
//                // return viewList.FindAll(xi => xi.RequestDate.ToString().Contains(year))/*.FindAll(x => x.ApprovalHoE == false)*/;


//            }

//            return View(viewList_itemdrilldown1);

//        }


//        public partial class RequestListAttributes
//        {

//            public List<RequestItemsRepoView> RequestItemsRepoView_model { get; set; }
//            public string Item_Name { get; set; }
//        }




//        [HttpGet]
//        public ActionResult Lookup()
//        {
//            LookupData lookupData = new LookupData();

//            string presentUserDept = lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).Department;
//            string is_CCXC = string.Empty; //For Clear CC XC Segregation

//            lookupData.DEPT_List = lstDEPTs.ToList();

//            lookupData.BU_List = lstBUs.OrderBy(x => x.ID).ToList();
//            lookupData.BU_List[2].BU = "MB";
//            lookupData.BU_List[4].BU = "OSS";
//            lookupData.OEM_List = lstOEMs;
//            //lookupData.Groups_oldList = lstGroups_old;

//            using(BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

//            using(BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

//                lookupData.Groups_test = db.Groups_Table_Test.ToList<Groups_Table_Test>();//lstGroups_test;
//            // lookupData.Groups_List = lstGroups;
//            lookupData.Item_List = lstItems;
//            lookupData.Category_List = lstPrdCateg;
//            lookupData.CostElement_List = lstCostElement;
//            lookupData.OrderStatus_List = lstOrderStatus;
//            lookupData.VendorCategory_List = lstVendor;

//            return Json(new { data = lookupData }, JsonRequestBehavior.AllowGet);

//        }
//        public partial class vkmSummary
//        {
//            public string vkmyear { get; set; }
//            public decimal P_MAE_Totals { get; set; }
//            public decimal P_NMAE_Totals { get; set; }
//            public decimal P_Software_Totals { get; set; }
//            public decimal P_Overall_Totals { get; set; }
//            public decimal U_MAE_Totals { get; set; }
//            public decimal U_NMAE_Totals { get; set; }
//            public decimal U_Software_Totals { get; set; }
//            public decimal U_Overall_Totals { get; set; }




//        }

//        public partial class costelement_drilldownSummary
//        {
//            public string vkmyear { get; set; }
//            public decimal[] P_Category_Totals { get; set; }
//            public decimal[] U_Category_Totals { get; set; }


//        }

//        public partial class category_drilldownSummary
//        {
//            public string vkmyear { get; set; }
//            public decimal[] P_Item_Totals { get; set; }
//            public decimal[] U_Item_Totals { get; set; }


//        }


//        public partial class CategoryBudget
//        {
//            public int CategoryID { get; set; }
//            public string CategoryName { get; set; }
//            public decimal CategoryTotals { get; set; }
//        }

//        public partial class ItemBudget
//        {
//            public int ItemID { get; set; }
//            public string ItemName { get; set; }
//            public decimal ItemTotals { get; set; }
//        }

//public class BudgetParam
//{
//    public string jsondata { get; set; }
//    public string columns { get; set; }
//    public string data { get; set; }
//}
//public class columnsinfo
//{
//    public string title { get; set; }
//    public string data { get; set; }
//}


//        //}
//    }
//}