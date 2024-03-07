using LC_Reports_V1.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LC_Reports_V1.Controllers
{
    //[Authorize(Users = @"apac\jov6cob,apac\rba3cob,apac\din2cob,apac\MTA2COB,apac\muu4cob,apac\nnj6kor,apac\pks5cob")]
    public class BudgetingController : Controller
    {
        private static SqlConnection con, budgetingcon;

        //public static List<SPOTONData_Table_2021> lstUsers_2021 = new List<SPOTONData_Table_2021>();
        public static List<BU_Table> lstBUs = new List<BU_Table>();
        //publicstaticc List<string> lstSections = new List<string>();
        public static List<DEPT_Table> lstDEPTs = new List<DEPT_Table>();
        public static List<DEPT_Table> lstDEPTs_FilteredForUsers = new List<DEPT_Table>();
        public static List<Groups_Table> lstGroups = new List<Groups_Table>();
        public static List<OEM_Table> lstOEMs = new List<OEM_Table>();
        public static List<DEPT_Table> lstBudgBU = new List<DEPT_Table>();
        public static List<Category_Table> lstPrdCateg = new List<Category_Table>();
        public static List<ItemsCostList_Table> lstItems = new List<ItemsCostList_Table>();
        public static List<CostElement_Table> lstCostElement = new List<CostElement_Table>();
        //public static List<tbl_UserIDs_Table> lstPrivileged = new List<tbl_UserIDs_Table>();
        public static List<POSPOC_Team> lstPrivileged = new List<POSPOC_Team>();
        //public static List<BU_SPOCS> lstBU_SPOCs = new List<BU_SPOCS>();
        public static List<BGSW_VKMSPOCs> lstBU_SPOCs = new List<BGSW_VKMSPOCs>();
        public static List<Order_Status_Table> lstOrderStatus = new List<Order_Status_Table>();
        public static List<OrderStatusDescription> lstOrderDescription = new List<OrderStatusDescription>();
        public static List<Currency_Table> lstCurrency = new List<Currency_Table>();
        public static List<LeadTime_Table> lstVendor = new List<LeadTime_Table>();
        public static List<Fund_Table> lstFund = new List<Fund_Table>();
        public static List<Planning_EM_Table> lstEMs = new List<Planning_EM_Table>();
        public static List<Planning_HOE_Table> lstHOEs = new List<Planning_HOE_Table>();
        public static List<SPOTONData_Table_2022> lstUsers = new List<SPOTONData_Table_2022>();
        public static List<BudgetCodeMaster> BudgetCodeList = new List<BudgetCodeMaster>();
        public static List<UOM_list> lstUOM = new List<UOM_list>();
        public static List<Material_Group_list> lstMaterialGroup = new List<Material_Group_list>();
        public static List<Order_Type_list> lstOrderType = new List<Order_Type_list>();
        public static List<SR_Responsible_Buyer_table> SRBuyerList = new List<SR_Responsible_Buyer_table>();
        public static List<SR_Responsible_Manager_table> SRManagerList = new List<SR_Responsible_Manager_table>();
        public static List<BGSW_BudgetCenter_Table> BudgetCenterList = new List<BGSW_BudgetCenter_Table>();


        //public static List<SPOTONData> lstUsers_2020 = new List<SPOTONData>();
        //public static List<Groups_Table_Aug> lstGroups_old = new List<Groups_Table_Aug>();
        public static List<Groups_Table_Test> lstGroups_test = new List<Groups_Table_Test>(); //with old new groups
        public static List<BGSW_F05F06_PurchaseType_Table> lstPurchaseType = new List<BGSW_F05F06_PurchaseType_Table>();
        public static List<UnloadingPointBGSW> lstUnloadingPoint = new List<UnloadingPointBGSW>();


        public static string presentUserNTID = string.Empty;

        // GET: Budgeting
        public ActionResult Index()
        {
            //string config = ConfigurationManager.AppSettings["email"];
            InitialiseBudgeting();
            //x();
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult FAQ()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }



        private static void connection()
        {
            string budgeting_constring = ConfigurationManager.ConnectionStrings["BudgetingdbConnection"].ToString();
            budgetingcon = new SqlConnection(budgeting_constring);
        }

        private static void BudgetingOpenConnection()
        {
            if (budgetingcon.State == ConnectionState.Closed)
            {
                budgetingcon.Open();
            }
        }

        private static void BudgetingCloseConnection()
        {
            if (budgetingcon.State == ConnectionState.Open)
            {
                budgetingcon.Close();
            }
        }



        [HttpPost]
        public ActionResult is_PurchaseSPOC()
        {
            System.Security.Principal.IPrincipal User = System.Web.HttpContext.Current.User;
            //string present = BudgetingController.lstUsers.Find(x => x.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).EmployeeName;
            bool is_PurchaseSPOC = false;
            var isSpoc = 0;
            if (BudgetingController.lstPrivileged.Find(person => person.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())) != null)
            {
                is_PurchaseSPOC = true;
                isSpoc = 1;
            }
            else
            {
                is_PurchaseSPOC = false;
                var CheckSPOCAvailability = VKMSPOC_Availability();
                if (CheckSPOCAvailability != "")
                {
                    isSpoc = 2;
                }
                else
                {
                    isSpoc = 0;
                }
            }

            return Json(new { success = is_PurchaseSPOC , isSpoc = isSpoc }, JsonRequestBehavior.AllowGet);
        }
        public string VKMSPOC_Availability()
        {

            string NTID = "";
            connection();
            BudgetingOpenConnection();
            string qry = " Exec [dbo].[CheckVKMSPOC_Availability] '" + User.Identity.Name.Split('\\')[1].Trim() + "' ";

            SqlCommand command = new SqlCommand(qry, budgetingcon);
            SqlDataReader dr = command.ExecuteReader();

            if (dr.HasRows)
            {
                dr.Read();
                NTID = dr["NTID"].ToString();
                //  NTID = "stp2kor";
            }
            else
            {
                NTID = "";
            }
            dr.Close();
            BudgetingCloseConnection();
            return NTID;
        }

        //[HttpPost]
        //public ActionResult is_VKMAdmin()
        //{
        //    System.Security.Principal.IPrincipal User = System.Web.HttpContext.Current.User;
        //    string present = BudgetingController.lstUsers.Find(x => x.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).EmployeeName;
        //    bool is_VKMAdmin = false;
        //    if (BudgetingController.lstBU_SPOCs.Find(person => person.VKMspoc.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())) != null)
        //    {
        //        var BU = BudgetingController.lstBU_SPOCs.Find(person => person.VKMspoc.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).BU;
        //        if (BU >= 99)
        //            is_VKMAdmin = true;

        //    }
        //    return Json(new { success = is_VKMAdmin }, JsonRequestBehavior.AllowGet);
        //}

        public static void InitialiseMaterialGroup()
        {
            connection();
            BudgetingOpenConnection();
            DataTable dt = new DataTable();
            string Query = " Select ID, MaterialGroup from [BGSW_MaterialGroup_Table] order by MaterialGroup";
            SqlCommand cmd = new SqlCommand(Query, budgetingcon);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            var lstMaterialGroup1 = new List<Material_Group_list>();

            foreach (DataRow row in dt.Rows)
            {
                Material_Group_list item = new Material_Group_list();
                item.ID = Convert.ToInt32(row["ID"]);
                item.Material_Group = row["MaterialGroup"].ToString();
                lstMaterialGroup1.Add(item);
            }
            lstMaterialGroup = lstMaterialGroup1; //this would update the Material Group list which would be used throughout
            BudgetingCloseConnection();

        }

        /// <summary>
        /// function to initialise common objects
        /// </summary>
        /// <returns></returns>
        public static bool InitialiseBudgeting()
        {
            System.Security.Principal.IPrincipal User = System.Web.HttpContext.Current.User;
            presentUserNTID = User.Identity.Name.Split('\\')[1].Trim().ToUpper();
            DataSet ds = new DataSet();
            connection();
            BudgetingOpenConnection();
            string Query = " Exec [dbo]." + ConfigurationManager.AppSettings["InitialiseBudgeting_StoredProcedure"] + " '" + presentUserNTID + "'";
            SqlCommand cmd = new SqlCommand(Query, budgetingcon);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            BudgetingCloseConnection();


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
            //Table 1 - SPOTONData	
            //lstUsers_2021 = new List<SPOTONData_Table_2021>();

            //foreach (DataRow row in ds.Tables[1].Rows)
            //{
            //    SPOTONData item = new SPOTONData();
            //    item.EmployeeNumber = row["EmployeeNumber"].ToString();
            //    item.EmployeeName = row["EmployeeName"].ToString();
            //    item.Gender = row["Gender"].ToString();
            //    item.NTID = row["NTID"].ToString();
            //    item.Level = row["Level"].ToString();
            //    item.Textbox46 = row["Textbox46"].ToString();
            //    item.IsCorporate = row["IsCorporate"].ToString();
            //    item.HeadCount1 = row["HeadCount1"].ToString();
            //    item.PersonalCapacity = row["PersonalCapacity"].ToString();
            //    item.PC_WithHoliday1 = row["PC_WithHoliday1"].ToString();
            //    item.DE__WithHoliday_StartDate = row["DE__WithHoliday_StartDate"].ToString();
            //    item.DE__WithHoliday_EndDate = row["DE__WithHoliday_EndDate"].ToString();
            //    item.PC_WithHolidayRemarks = row["PC_WithHolidayRemarks"].ToString();
            //    item.ContractEndDate = row["ContractEndDate"].ToString();
            //    item.PartTimeEndDate = row["PartTimeEndDate"].ToString();
            //    item.PartTimePercentage = row["PartTimePercentage"].ToString();
            //    item.Role = row["Role"].ToString();
            //    item.BU_Parent = row["BU_Parent"].ToString();
            //    item.Section = row["Section"].ToString();
            //    item.Department = row["Department"].ToString();
            //    item.Group = row["Group"].ToString();
            //    item.Location = row["Location"].ToString();
            //    item.PersonnelArea = row["PersonnelArea"].ToString();
            //    item.WorkPlace = row["WorkPlace"].ToString();
            //    item.DOJ1 = row["DOJ1"].ToString();
            //    item.Textbox34 = row["Textbox34"].ToString();
            //    item.Textbox35 = row["Textbox35"].ToString();

            //    lstUsers_2020.Add(item);
            //}
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
                if (row["BudgetCode"].ToString() != "")
                    item.BudgetCode = Convert.ToInt32(row["BudgetCode"].ToString());
                item.UOM = row["UOM"].ToString();
                item.Order_Type = row["Order_Type"].ToString().Trim() != "" ? Convert.ToInt32(row["Order_Type"].ToString()) : 0;
                lstItems.Add(item);
            }

            //Table 9 - tbl_UserIDs_Table;
            lstPrivileged = new List<POSPOC_Team>();
            foreach (DataRow row in ds.Tables[8].Rows)
            {
                POSPOC_Team item = new POSPOC_Team();
                item.NTID = row["NTID"].ToString();
                item.NAME = row["NAME"].ToString();
                //item.BU = row["BU"].ToString();
                item.SECTION = row["SECTION"].ToString();
                lstPrivileged.Add(item);

            }
            //foreach (DataRow row in ds.Tables[9].Rows)
            //{
            //    tbl_UserIDs_Table item = new tbl_UserIDs_Table();
            //    item.ADSID = row["ADSID"].ToString();
            //    item.FullName = row["FullName"].ToString();
            //    item.BU = row["BU"].ToString();
            //    item.emailID = row["emailID"].ToString();
            //    lstPrivileged.Add(item);

            //}

            //Table 10 -BU_SPOCS;
            lstBU_SPOCs = new List<BGSW_VKMSPOCs>();

            foreach (DataRow row in ds.Tables[9].Rows)
            {
                BGSW_VKMSPOCs item = new BGSW_VKMSPOCs();
                //item.BU_ID = Convert.ToInt32(row["BU_ID"]).ToString();
                item.VKMspoc = row["VKMspoc"].ToString();
                item.TopSection_Section = row["VKMspoc"].ToString();
                item.ID = Convert.ToInt32(row["ID"].ToString());
                item.BU_ID = row["BU_ID"].ToString();
                lstBU_SPOCs.Add(item);
            }
            //lstBU_SPOCs = new List<BU_SPOCS>();

            //foreach (DataRow row in ds.Tables[10].Rows)
            //{
            //    BU_SPOCS item = new BU_SPOCS();
            //    item.BU = Convert.ToInt32(row["BU"]);
            //    item.VKMspoc = row["VKMspoc"].ToString();
            //    lstBU_SPOCs.Add(item);
            //}
            //Table 11 -Order_Status_Table	
            lstOrderStatus = new List<Order_Status_Table>();

            foreach (DataRow row in ds.Tables[10].Rows)
            {
                Order_Status_Table item = new Order_Status_Table();
                item.ID = Convert.ToInt32(row["ID"]);
                item.OrderStatus = row["OrderStatus"].ToString();
                lstOrderStatus.Add(item);
            }

            //Table 12 -Currency_Table	
            lstCurrency = new List<Currency_Table>();

            foreach (DataRow row in ds.Tables[11].Rows)
            {
                Currency_Table item = new Currency_Table();
                item.ID = Convert.ToInt32(row["ID"]);
                item.Currency = row["Currency"].ToString();
                lstCurrency.Add(item);
            }

            //Table 13 -LeadTime_Table	
            lstVendor = new List<LeadTime_Table>();

            foreach (DataRow row in ds.Tables[12].Rows)
            {
                LeadTime_Table item = new LeadTime_Table();
                item.ID = Convert.ToInt32(row["ID"]);
                item.VendorCategory = row["VendorCategory"].ToString();
                if (row["LeadTime"].ToString() != "")
                    item.LeadTime = Convert.ToInt32(row["LeadTime"].ToString());
                lstVendor.Add(item);
            }
            //Table 14 -Fund_Table	
            lstFund = new List<Fund_Table>();

            foreach (DataRow row in ds.Tables[13].Rows)
            {
                Fund_Table item = new Fund_Table();
                item.ID = Convert.ToInt32(row["ID"]);
                item.Fund = row["Fund"].ToString();
                lstFund.Add(item);
            }
            //Table 15 -Planning_EM_Table
            lstEMs = new List<Planning_EM_Table>();

            foreach (DataRow row in ds.Tables[14].Rows)
            {
                Planning_EM_Table item = new Planning_EM_Table();
                item.ID = Convert.ToInt32(row["ID"]);
                item.NTID = row["NTID"].ToString();
                item.FullName = row["FullName"].ToString();
                item.Department = row["Department"].ToString();
                item.Group = row["Group"].ToString();
                item.Proxy_NTID = row["Proxy_NTID"].ToString();
                item.Proxy_FullName = row["Proxy_FullName"].ToString();
                item.Updated_By = row["Updated_By"].ToString();
                item.Enable_Proxy = row["Enable_Proxy"].ToString() != "" ? Convert.ToBoolean(row["Enable_Proxy"]) : false;
                lstEMs.Add(item);
            }
            //Table 16 -Planning_HOE_Table	
            lstHOEs = new List<Planning_HOE_Table>();
            foreach (DataRow row in ds.Tables[15].Rows)
            {
                Planning_HOE_Table item = new Planning_HOE_Table();
                item.ID = Convert.ToInt32(row["ID"]);
                item.HOE_NTID = row["HOE_NTID"].ToString();
                item.HOE_FullName = row["HOE_FullName"].ToString();
                item.Department = row["Department"].ToString();
                item.Proxy_NTID = row["Proxy_NTID"].ToString();
                item.Proxy_FullName = row["Proxy_FullName"].ToString();
                item.Updated_By = row["Updated_By"].ToString();
                item.Enable_Proxy = row["Enable_Proxy"].ToString() != "" ? Convert.ToBoolean(row["Enable_Proxy"]) : false;
                lstHOEs.Add(item);
            }
            //}

            //Table 17 -Material_Group_Table	
            InitialiseMaterialGroup();

            //foreach (DataRow row in ds.Tables[17].Rows)
            //{
            //    Material_Group_list item = new Material_Group_list();
            //    item.ID = Convert.ToInt32(row["ID"]);
            //    item.Material_Group = row["MaterialGroup"].ToString();
            //    lstMaterialGroup.Add(item);
            //}
            //Table 18 -Order_Type_Table	
            lstOrderType = new List<Order_Type_list>();

            foreach (DataRow row in ds.Tables[16].Rows)
            {
                Order_Type_list item = new Order_Type_list();
                item.ID = Convert.ToInt32(row["ID"]);
                item.Order_Type = row["OrderType"].ToString();
                lstOrderType.Add(item);
            }
            //Table 19 -UOM_Table	
            lstUOM = new List<UOM_list>();

            foreach (DataRow row in ds.Tables[17].Rows)
            {
                UOM_list item = new UOM_list();
                item.ID = Convert.ToInt32(row["ID"]);
                item.UOM = row["Units"].ToString();
                item.Order_Type = row["ItemOrderType"].ToString();
                lstUOM.Add(item);
            }

            //Table 20 -UOM_Table	
            BudgetCodeList = new List<BudgetCodeMaster>();

            foreach (DataRow row in ds.Tables[18].Rows)
            {
                BudgetCodeMaster item = new BudgetCodeMaster();
                item.ID = Convert.ToInt32(row["ID"]);
                item.CostElementID = Convert.ToInt32(row["CostElementID"].ToString());
                item.Budget_Code = row["Budget_Code"].ToString();
                item.Budget_Code_Description = row["Budget_Code_Description"].ToString();
                BudgetCodeList.Add(item);
            }

            //Table 21 - DEPT_Table	
            lstDEPTs_FilteredForUsers = new List<DEPT_Table>();
            foreach (DataRow row in ds.Tables[19].Rows)
            {
                DEPT_Table item = new DEPT_Table();
                item.ID = Convert.ToInt32(row["ID"]);
                item.DEPT = row["DEPT"].ToString();
                item.Outdated = Convert.ToBoolean(row["Outdated"]);
                item.Rep_ID = row["Rep_ID"].ToString() != "" ? Convert.ToInt32(row["Rep_ID"]) : 0;
                lstDEPTs_FilteredForUsers.Add(item);
            }

            lstPurchaseType = new List<BGSW_F05F06_PurchaseType_Table>();
            foreach (DataRow row in ds.Tables[20].Rows)
            {
                BGSW_F05F06_PurchaseType_Table item = new BGSW_F05F06_PurchaseType_Table();
                item.ID = Convert.ToInt32(row["ID"]);
                item.PurchaseType = row["PurchaseType"].ToString();
                lstPurchaseType.Add(item);
            }
            lstUnloadingPoint = new List<UnloadingPointBGSW>();
            foreach (DataRow row in ds.Tables[21].Rows)
            {
                UnloadingPointBGSW item = new UnloadingPointBGSW();
                item.ID = Convert.ToInt32(row["ID"]);
                item.UnloadingPoint = row["UnloadingPoint"].ToString();
                item.Location = row["Location"].ToString();
                item.Address = row["Address"].ToString();
                lstUnloadingPoint.Add(item);
            }

            SRBuyerList = new List<SR_Responsible_Buyer_table>();
            foreach (DataRow row in ds.Tables[22].Rows)
            {
                SR_Responsible_Buyer_table item = new SR_Responsible_Buyer_table();
                item.ID = Convert.ToInt32(row["ID"]);
                item.NTID = row["NTID"].ToString();
                item.BuyerName = row["BuyerName"].ToString();
                item.Section = row["Section"].ToString();
                item.Manager_ID = row["Manager_ID"].ToString();

                SRBuyerList.Add(item);
            }

            SRManagerList = new List<SR_Responsible_Manager_table>();
            foreach (DataRow row in ds.Tables[23].Rows)
            {
                SR_Responsible_Manager_table item = new SR_Responsible_Manager_table();
                item.ID = Convert.ToInt32(row["ID"]);
                item.NTID = row["NTID"].ToString();
                item.ManagerName = row["ManagerName"].ToString();
                item.Section = row["Section"].ToString();


                SRManagerList.Add(item);
            }

            BudgetCenterList = new List<BGSW_BudgetCenter_Table>();
            foreach (DataRow row in ds.Tables[24].Rows)
            {
                BGSW_BudgetCenter_Table item = new BGSW_BudgetCenter_Table();
                item.ID = Convert.ToInt32(row["ID"]);
                item.BudgetCenter = row["BudgetCenter"].ToString();
                
                BudgetCenterList.Add(item);
            }
            //BudgetCodeList = new List<BudgetCodeMaster>();

            //for (int i = 1; i < 4; i++)
            //{

            //    if (i == 1)
            //    {
            //        BudgetCodeMaster data1 = new BudgetCodeMaster();
            //        data1.CostElementID = i;
            //        data1.BudgetCode = "203";
            //        BudgetCodeList.Add(data1);
            //    }
            //    else if (i == 2)
            //    {
            //        BudgetCodeMaster data1 = new BudgetCodeMaster();
            //        data1.CostElementID = i;
            //        data1.BudgetCode = "311";
            //        BudgetCodeList.Add(data1);

            //        data1 = new BudgetCodeMaster();
            //        data1.CostElementID = i;
            //        data1.BudgetCode = "313";
            //        BudgetCodeList.Add(data1);
            //    }
            //    else if (i == 3)
            //    {
            //        BudgetCodeMaster data1 = new BudgetCodeMaster();
            //        data1.CostElementID = i;
            //        data1.BudgetCode = "511";
            //        BudgetCodeList.Add(data1);

            //        data1 = new BudgetCodeMaster();
            //        data1.CostElementID = i;
            //        data1.BudgetCode = "513";
            //        BudgetCodeList.Add(data1);

            //        data1 = new BudgetCodeMaster();
            //        data1.CostElementID = i;
            //        data1.BudgetCode = "514";
            //        BudgetCodeList.Add(data1);
            //    }
            //}

            return true;
        }



        public ActionResult DownloadTemplate()
        {
            string folderPath = Server.MapPath("~/Templates/");
            string fileName = "VKM2023_SmartLab Budgeting2.0.pdf";
            byte[] fileBytes = System.IO.File.ReadAllBytes(folderPath + fileName);

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }





        //emailNotification_VKM//
        [HttpPost]
        public ActionResult SendEmail(Emailnotify emailnotify)
        {
            string EmailFlag = ConfigurationManager.AppSettings["TestEmailFlag"].ToString();
            if (EmailFlag == "0")
            {
                LC_Reports_V1.Controllers.BudgetingRequestController.WriteLog("************* Budgeting AutoMail **************");
                Budgeting_CommonFn.emailNotification_VKM(emailnotify);
                return Json(new { success = true, message = "Email has been sent" }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
        }


        //emailNotification_Order//
        [HttpPost]
        public ActionResult SendEmail_Order(Emailnotify_OrderStage emailnotify, Emailnotify_RFOApprover emailnotifyRFOApprover)
        {
            try
            {
                string EmailFlag = ConfigurationManager.AppSettings["TestEmailFlag"].ToString();
                if (EmailFlag == "0")
                {
                    Budgeting_CommonFn.emailNotification_VKM(emailnotify);
                    if (emailnotifyRFOApprover.RFOApprover != "" && emailnotifyRFOApprover.RFOApprover != null)
                        Budgeting_CommonFn.emailNotification_RFOApprover(emailnotifyRFOApprover);
                }
                if (emailnotify.is_RequesttoOrder == 3)//unplanned f02
                    return Json(new { success = true, message = "Unplanned F02" }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { success = true, message = "Email has been sent" }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return Json(new { success = false, message = "Error in sending mail" }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public ActionResult Get_RFOUsers()
        {

            using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

            //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

            {
                List<string> present_authorized = new List<string>() { "din2cob", "rma5cob", "mae91cob", "mxk8kor", "mta2cob", "oig1cob", "ghb1cob" };



                System.Security.Principal.IPrincipal User = System.Web.HttpContext.Current.User;
                if (present_authorized.Contains(User.Identity.Name.Split('\\')[1].ToLower()))

                    return Json(new { successmsg = true }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { errormsg = true }, JsonRequestBehavior.AllowGet);

            }


        }

        //[HttpPost]
        //public ActionResult UserAccess_cockpitinput()
        //{

        //    string NTID, isLabTeam = "";
        //    connection();
        //    string qry = " Select isnull(ADSID,'') as NTID,isLabTeam from Internal_UserIDs_Table where ADSID = '" + User.Identity.Name.Split('\\')[1].Trim() + "'";
        //    BudgetingOpenConnection();
        //    SqlCommand command = new SqlCommand(qry, budgetingcon);
        //    SqlDataReader dr = command.ExecuteReader();
        //    if (dr.HasRows)
        //    {
        //        dr.Read();
        //        NTID = dr["NTID"].ToString();
        //        isLabTeam = dr["isLabTeam"].ToString();
        //    }
        //    else
        //    {
        //        NTID = "";
        //    }
        //    dr.Close();
        //    BudgetingCloseConnection();

        //    if (NTID == "")
        //    {
        //        return Json(new { errormsg = true }, JsonRequestBehavior.AllowGet);
        //    }
        //    else
        //    {
        //        if(isLabTeam.Trim() == "1")
        //            return Json(new { successmsg = true, isLabTeam = true }, JsonRequestBehavior.AllowGet);
        //        else
        //            return Json(new { successmsg = true, isLabTeam = false }, JsonRequestBehavior.AllowGet);
        //    }


        //}


        public void x()
        {

            using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

            //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

            {

                var ReqList = db.RequestItems_Table.ToList().FindAll(x => x.VKM_Year == 2023 && x.RequestID > 3730);
                foreach (RequestItems_Table item in ReqList)
                {
                    var Masterlist_entry = BudgetingController.lstItems.Find(x => x.S_No == int.Parse(item.ItemName));

                    item.Category = Masterlist_entry.Category;
                    item.CostElement = Masterlist_entry.Cost_Element;
                    item.UnitPrice = (decimal?)Masterlist_entry.UnitPriceUSD;
                    item.TotalPrice = (decimal?)(Masterlist_entry.UnitPriceUSD * item.ReqQuantity);
                    if (item.ApprCost != null)
                        item.ApprCost = (decimal?)(Masterlist_entry.UnitPriceUSD * item.ApprQuantity);
                    else
                        item.ApprCost = null;
                    item.ActualAvailableQuantity = Masterlist_entry.Actual_Available_Quantity;
                    Console.WriteLine("Updating - " + item.RequestID + " " + item.RequestorNT);
                    item.RequestID = item.RequestID;
                    item.BU = item.BU;
                    item.DEPT = item.DEPT;
                    item.Group = item.Group;
                    item.OEM = item.OEM;
                    item.ItemName = item.ItemName;
                    item.ReqQuantity = item.ReqQuantity;

                    item.Comments = item.Comments;
                    item.RequestorNT = item.RequestorNT;
                    item.RequestorNTID = item.RequestorNTID;
                    item.ApprovalDH = item.ApprovalDH;
                    item.ApprQuantity = item.ApprQuantity;

                    item.ApprovedDH = item.ApprovedDH;
                    item.DHNT = item.DHNT;
                    item.ApprovalSH = item.ApprovalSH;
                    item.ApprovedSH = item.ApprovedSH;
                    item.SHNT = item.SHNT;


                    item.RequestDate = item.RequestDate;
                    item.SubmitDate = item.SubmitDate;
                    item.DHAppDate = item.DHAppDate;
                    item.SHAppDate = item.SHAppDate;
                    item.OrderID = item.OrderID;
                    item.OrderStatus = item.OrderStatus;
                    item.OrderPrice = item.OrderPrice;
                    item.Ordered = item.Ordered;
                    item.RequiredDate = item.RequiredDate;
                    item.RequestOrderDate = item.RequestOrderDate;
                    item.RequestToOrder = item.RequestToOrder;
                    item.OrderDate = item.OrderDate;
                    item.TentativeDeliveryDate = item.TentativeDeliveryDate;
                    item.ActualDeliveryDate = item.ActualDeliveryDate;
                    item.Fund = item.Fund;
                    item.OrderedQuantity = item.OrderedQuantity;
                    item.DeliveredQuantity = item.DeliveredQuantity;
                    item.Customer_Name = item.Customer_Name;
                    item.Customer_Dept = item.Customer_Dept;
                    item.BM_Number = item.BM_Number;
                    item.Task_ID = item.Task_ID;
                    item.Resource_Group_Id = item.Resource_Group_Id;
                    item.PIF_ID = item.PIF_ID;
                    item.isCancelled = item.isCancelled;

                    item.ActualAvailableQuantity = item.ActualAvailableQuantity;
                    item.Project = item.Project;
                    item.HOEView_ActionHistory = item.HOEView_ActionHistory;

                    item.PORemarks = item.PORemarks;
                    item.PORaisedBy = item.PORaisedBy;
                    item.UpdatedAt = item.UpdatedAt;
                    item.VKM_Year = item.VKM_Year;
                    item.OrderStatus_Flag = item.OrderStatus_Flag;
                    item.UnplannedF02_Flag = item.UnplannedF02_Flag;

                    db.Entry(item).State = EntityState.Modified;

                    db.SaveChanges();








                }
            }
        }






    }




}
