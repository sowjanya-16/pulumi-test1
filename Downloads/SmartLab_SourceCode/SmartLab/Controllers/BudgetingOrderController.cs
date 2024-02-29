using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using LC_Reports_V1.Models;
using System.Data;
using System.IO;
using System.Globalization;
using System.Web.UI.WebControls;
using ClosedXML.Excel;
using System.Configuration;
using System.Data.SqlClient;
using System.Web;
using System.Web.Script.Serialization;
using System.Text;
using Newtonsoft.Json;
using System.Data.OleDb;

namespace LC_Reports_V1.Controllers
{
    public class BudgetingOrderController : Controller
    {
        //public static List<SPOTONData_Table_2022> lstUsers = new List<SPOTONData_Table_2022>();
        //public static List<BU_Table> lstBUs = new List<BU_Table>();

        //public static List<DEPT_Table> lstDEPTs = new List<DEPT_Table>();
        ////public static List<Groups_Table> lstGroups = new List<Groups_Table>();
        //public static List<OEM_Table> lstOEMs = new List<OEM_Table>();

        //public static List<Category_Table> lstPrdCateg = new List<Category_Table>();
        //public static List<ItemsCostList_Table> lstItems = new List<ItemsCostList_Table>();
        //public static List<CostElement_Table> lstCostElement = new List<CostElement_Table>();
        //public static List<tbl_UserIDs_Table> lstPrivileged = new List<tbl_UserIDs_Table>();
        //public static List<BU_SPOCS> lstBU_SPOCs = new List<BU_SPOCS>();
        //public static List<Groups_Table_Test> lstGroups_test = new List<Groups_Table_Test>(); //with old new groups
        //public static List<Order_Status_Table> lstOrderStatus = new List<Order_Status_Table>();
        //public static List<Fund_Table> lstFund = new List<Fund_Table>();
        //public static List<LeadTime_Table> lstVendor = new List<LeadTime_Table>();
        public static bool isDashboard_flag = false;

        //public static List<BudgetCodeMaster> BudgetCodeList = new List<BudgetCodeMaster>();

        // GET: RequestItems
        public ActionResult Index(string isDashboard = "Lab RFO")
        {

            //if (lstUsers == null || lstUsers.Count == 0)
            //{

            //    return RedirectToAction("Index", "Budgeting");
            //}
            OEMList oemlist = new OEMList();

            BudgetingController.InitialiseBudgeting();

            //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

            ////using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

            //{
            //    if (lstUsers == null || lstUsers.Count == 0)
            //        lstUsers = db.SPOTONData_Table_2022.ToList<SPOTONData_Table_2022>();
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
            //    if (lstBU_SPOCs == null || lstBU_SPOCs.Count == 0)
            //        lstBU_SPOCs = db.BU_SPOCS.ToList<BU_SPOCS>();
            //    if (lstGroups_test == null || lstGroups_test.Count == 0)
            //        lstGroups_test = db.Groups_Table_Test.ToList<Groups_Table_Test>();
            //    if (lstOrderStatus == null || lstOrderStatus.Count == 0)
            //        lstOrderStatus = db.Order_Status_Table.ToList<Order_Status_Table>();
            //    if (lstVendor == null || lstVendor.Count == 0)
            //        lstVendor = db.LeadTime_Table.ToList<LeadTime_Table>();
            //    if (lstFund == null || lstFund.Count == 0)
            //        lstFund = db.Fund_Table.ToList<Fund_Table>();

            //    BudgetCodeList = BudgetingController.BudgetCodeList;

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
            //    lstVendor.Sort((a, b) => a.VendorCategory.CompareTo(b.VendorCategory));
            //    lstFund.Sort((a, b) => a.Fund.CompareTo(b.Fund));

            //    foreach (var oem in lstOEMs)
            //    {
            //        oemlist.OEMSelectList.Add(new SelectListItem { Text = oem.OEM, Value = oem.ID.ToString() });
            //    }
            //    oemlist.OEMSelectList.Sort((a, b) => a.Text.CompareTo(b.Text));

            //}
            oemlist.isDashboard = isDashboard;
            if (isDashboard.Contains("RFO"))
                isDashboard_flag = false;
            else
                isDashboard_flag = true;

            return View(oemlist);
        }


        private SqlConnection conn;

        private void connection()
        {

            string connString = ConfigurationManager.ConnectionStrings["BudgetingdbConnection"].ToString();
            conn = new SqlConnection(connString);

        }

        private  void OpenConnection()
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
        /// function to fetch the Request Items made by the Requestor during the year chosen for view
        /// /// <param name="year"></param>
        /// </summary>

        [HttpPost]
        public ActionResult GetData(string year/*string[] oem*/)
        {


            using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

            //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

            {
                bool popupadd = false;
                bool popupedit = false,
                   gridedit = false;
                var UserNTID = System.Web.HttpContext.Current.User.Identity.Name.Split('\\')[1].ToUpper();
                try
                {

                    List<RequestItemsRepoView> viewList = new List<RequestItemsRepoView>();
                    viewList = GetData1(year);
                    string Query = "select [AddRFO],[Edit Popup RFO],[Edit Row RFO] from Mail_RFOAuthorization_Table where Section in (Select distinct Section from SPOTONData_Table_2022 where NTID = '" + UserNTID + "') OR TopSection in (Select distinct Section from SPOTONData_Table_2022 where NTID = '" + UserNTID + "')";
                    OpenConnection();
                    var cmd = new SqlCommand(Query, conn);
                    var dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        dr.Read();
                        popupadd = (Convert.ToBoolean(Convert.ToInt32(dr["AddRFO"].ToString())));
                        popupedit = (Convert.ToBoolean(Convert.ToInt32(dr["Edit Popup RFO"].ToString())));
                        gridedit = (Convert.ToBoolean(Convert.ToInt32(dr["Edit Row RFO"].ToString())));
                    }
                    dr.Close();

                    if (db.OrderingRequestor_Table.ToList().Find(person => person.NTID.ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())) != null)
                    {
                        //code commented sinc elogic handled in SP
                        //if (db.OrderingRequestor_Table.ToList().Find(person => person.NTID.ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).isSuperUser != null)
                        //{
                        //    if (db.OrderingRequestor_Table.ToList().Find(person => person.NTID.ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).isSuperUser == true)
                        //    {
                        //        List<string> BUs_rfo = db.OrderingRequestor_Table.ToList().Find(person => person.NTID.ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).BU.Split(',').ToList();
                        //        var detail_superuser = viewList.FindAll(x => BUs_rfo.Contains(x.BU.ToString().Trim()));
                        //        return Json(new { success = true, data = detail_superuser, isDashboard_flag }, JsonRequestBehavior.AllowGet);
                        //    }

                        //}
                        //var DEPT = db.OrderingRequestor_Table.ToList().Find(person => person.NTID.ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).Department.Trim();
                        //var detail = viewList.FindAll(dpt => dpt.DEPT.ToString().Trim().Equals(DEPT));

                        return Json(new { success = true, data = viewList, isDashboard_flag, popupadd, popupedit, gridedit, UserNTID }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        //code commented sinc elogic handled in SP
                        //var spoton_dept = db.SPOTONData_Table_2022.ToList().Find(person => person.NTID.ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).Department.Trim();
                        //string dept_id =BudgetingController.lstDEPTs.Find(dpt => dpt.DEPT == spoton_dept).ID.ToString();
                        //var detail = viewList.FindAll(dpt => dpt.DEPT.ToString().Trim().Equals(dept_id));
                        return Json(new { success = false, data = viewList, isDashboard_flag, popupadd, popupedit, gridedit, UserNTID }, JsonRequestBehavior.AllowGet);
                        //return Json(new { success = false, message = "Sorry! Current user is not authorised to view the Orders!" }, JsonRequestBehavior.AllowGet);
                    }
                }

                catch (Exception ex)
                {
                    return Json(new { success = true, popupadd, popupedit, gridedit, UserNTID, message = "Unable to load the Item Requests, Please Try again later!" }, JsonRequestBehavior.AllowGet);

                }

            }
        }

        public List<RequestItemsRepoView> GetData1(string year/*string[] oem*/)
        {
            List<RequestItemsRepoView> viewList = new List<RequestItemsRepoView>();
            //string PresentUserDept_RequestTable = lstDEPTs.Find(dept => dept.DEPT.Equals(presentUserDept)).ID.ToString();
            try
            {

                using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

                //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

                {

                    System.Security.Principal.IPrincipal User = System.Web.HttpContext.Current.User;
                    bool is_Authorized = false;
                    string presentUserName = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).EmployeeName;
                    //var x = presentUserName.TrimEnd().TrimStart().Split(' ').ToList();
                    ////string presentUserNTID = string.Empty;
                    //x.Sort();
                    //List<RequestItems_Table> reqList = new List<RequestItems_Table>();
                    //reqList = db.RequestItems_Table.ToList<RequestItems_Table>().FindAll(xi => xi.ApprovedSH == true);

                    DataTable dt1 = new DataTable();
                    connection();
                    OpenConnection();
                    //string Query = " Exec [dbo].[GetReqItemsList_VKMSPOCReviews_View] '" + useryear + "', 'MAE9COB', 'Export All' ";
                    string Query = " Exec [dbo].[GetReqItemsList_RFO_View] '" + (Convert.ToInt32(year) + 1) + "', '" + User.Identity.Name.Split('\\')[1].ToUpper() + "', 'View' ";
                    SqlCommand cmd = new SqlCommand(Query, conn);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt1);
                    CloseConnection();

                    foreach (DataRow item in dt1.Rows)
                    {
                        //var y = item.RequestorNT.TrimStart().TrimEnd().Split(' ').ToList();
                        //y.Sort();
                        //for (int i = 0; i < x.Count(); i++)
                        //{
                        //    if (x[i] != y[i])
                        //        break;
                        //}

                        RequestItemsRepoView ritem = new RequestItemsRepoView();

                        ritem.BU = int.Parse(item["BU"].ToString());
                        ritem.BudgetCode = item["BudgetCode"].ToString();
                        ritem.RequestorNTID = item["RequestorNTID"].ToString();


                        ritem.isProjected = (bool)((item["isProjected"] != null && item["isProjected"].ToString() != string.Empty) ? item["isProjected"] : false);
                        ritem.Q1 = (bool)((item["Q1"] != null && item["Q1"].ToString() != string.Empty) ? item["Q1"] : false);
                        ritem.Q2 = (bool)((item["Q2"] != null && item["Q2"].ToString() != string.Empty) ? item["Q2"] : false);
                        ritem.Q3 = (bool)((item["Q3"] != null && item["Q3"].ToString() != string.Empty) ? item["Q3"] : false);
                        ritem.Q4 = (bool)((item["Q4"] != null && item["Q4"].ToString() != string.Empty) ? item["Q4"] : false);

                        ritem.Is_UnplannedF02Item = (bool)((item["Is_UnplannedF02Item"] != null && item["Is_UnplannedF02Item"].ToString() != string.Empty) ? item["Is_UnplannedF02Item"] : false);
                        //ritem.Projected_Amount = item["Projected_Amount"] != null ? Convert.ToDecimal(item["Projected_Amount"].ToString()) : 0;
                        //ritem.Unused_Amount = item["Unused_Amount"] != null ? Convert.ToDecimal(item["Unused_Amount"].ToString()) : 0;



                        if (item["Projected_Amount"].ToString() != "" && item["Projected_Amount"] != null)
                            ritem.Projected_Amount = (item["Projected_Amount"].ToString() != "" && item["Projected_Amount"] != null) ? Math.Round((decimal)item["Projected_Amount"], MidpointRounding.AwayFromZero) : 0;

                        if (item["Unused_Amount"].ToString() != "" && item["Unused_Amount"] != null)
                            ritem.Unused_Amount = item["Unused_Amount"] != null ? Math.Round((decimal)item["Unused_Amount"], MidpointRounding.AwayFromZero) : 0;

                        if (item["UpdatedAt"].ToString() != "" && item["UpdatedAt"] != null)
                            ritem.UpdatedAt = Convert.ToDateTime(item["UpdatedAt"]);

                        ritem.Requestor = item["RequestorNT"].ToString();

                        if (item["ActualAvailableQuantity"] == null || item["ActualAvailableQuantity"].ToString().Trim() == string.Empty)
                            ritem.ActualAvailableQuantity = "NA";
                        else
                            ritem.ActualAvailableQuantity = item["ActualAvailableQuantity"].ToString();

                        ritem.VKM_Year = (int)item["VKM_Year"];
                        ritem.PORemarks = item["PORemarks"].ToString();
                        ritem.BU = int.Parse(item["BU"].ToString());
                        if (item["Category"] != null && item["Category"].ToString() != "")
                            ritem.Category = int.Parse(item["Category"].ToString());

                        ritem.Comments = item["Comments"].ToString();
                        if (item["CostElement"] != null && item["CostElement"].ToString() != "")
                            ritem.Cost_Element = int.Parse(item["CostElement"].ToString());

                        ritem.DEPT = int.Parse(item["DEPT"].ToString());
                        ritem.Group = int.Parse(item["Group"].ToString());
                        ritem.Item_Name = int.Parse(item["ItemName"].ToString() != "" ? item["ItemName"].ToString() : "0");
                        ritem.OEM = int.Parse(item["OEM"].ToString());
                        ritem.Required_Quantity = int.Parse(item["ReqQuantity"].ToString() != "" ? item["ReqQuantity"].ToString() : "0");
                        ritem.Total_Price = decimal.Parse(item["TotalPrice"].ToString() != "" ? item["TotalPrice"].ToString() : "0");
                        ritem.Reviewed_Quantity = int.Parse(item["ApprQuantity"].ToString() != "" ? item["ApprQuantity"].ToString() : "0");
                        ritem.RequestID = int.Parse(item["RequestID"].ToString());
                        ritem.Requestor = item["RequestorNT"].ToString();
                        ritem.Reviewed_Cost = decimal.Parse(item["ApprCost"].ToString() != "" ? item["ApprCost"].ToString() : "0");
                        ritem.Unit_Price = decimal.Parse(item["UnitPrice"].ToString() != "" ? item["UnitPrice"].ToString() : "0");
                        ritem.ApprovalHoE = (bool)item["ApprovalDH"];
                        ritem.ApprovalSH = (bool)item["ApprovalSH"];
                        ritem.ApprovedHoE = (bool)item["ApprovedDH"];
                        ritem.ApprovedSH = (bool)item["ApprovedSH"];


                        ritem.Reviewer_1 = item["DHNT"].ToString();
                        ritem.Reviewer_2 = item["SHNT"].ToString();

                        ritem.RequestDate = item["RequestDate"].ToString() != "" ? ((DateTime)item["RequestDate"]).ToString("yyyy-MM-dd") : string.Empty;
                        ritem.SubmitDate = item["SubmitDate"].ToString() != "" ? ((DateTime)item["SubmitDate"]).ToString("yyyy-MM-dd") : string.Empty;
                        ritem.Review1_Date = item["DHAppDate"].ToString() != "" ? ((DateTime)item["DHAppDate"]).ToString("yyyy-MM-dd") : string.Empty;
                        ritem.Review2_Date = item["SHAppDate"].ToString() != "" ? ((DateTime)item["SHAppDate"]).ToString("yyyy-MM-dd") : string.Empty;

                        ritem.ActualDeliveryDate = item["ActualDeliveryDate"].ToString() != "" ? ((DateTime)item["ActualDeliveryDate"]).ToString("yyyy-MM-dd") : string.Empty;
                        ritem.RequiredDate = item["RequiredDate"].ToString() != "" ? ((DateTime)item["RequiredDate"]).ToString("yyyy-MM-dd") : string.Empty;
                        ritem.RequestOrderDate = item["RequestOrderDate"].ToString() != "" ? ((DateTime)item["RequestOrderDate"]).ToString("yyyy-MM-dd") : string.Empty;
                        ritem.OrderDate = item["OrderDate"].ToString() != "" ? ((DateTime)item["OrderDate"]).ToString("yyyy-MM-dd") : string.Empty;
                        ritem.TentativeDeliveryDate = item["TentativeDeliveryDate"].ToString() != "" ? ((DateTime)item["TentativeDeliveryDate"]).ToString("yyyy-MM-dd") : string.Empty;


                        if (item["Fund"] != null && item["Fund"].ToString() != "")
                        {
                            ritem.Fund = int.Parse(item["Fund"].ToString());
                        }
                        else
                        {
                            ritem.Fund = BudgetingController.lstFund.Find(fund => fund.Fund.Equals("F02")).ID;
                        }

                        if (item["RequestToOrder"].ToString() == "" || (bool)item["RequestToOrder"] != true)
                        {
                            ritem.RequestToOrder = false;
                        }
                        else
                        {
                            ritem.RequestToOrder = true;
                        }
                        if (item["OrderedQuantity"] != null && item["OrderedQuantity"].ToString() != "")
                        {
                            ritem.OrderedQuantity = (int)item["OrderedQuantity"];
                        }
                        else
                        {
                            ritem.OrderedQuantity = null;
                        }
                        ritem.OrderPrice = decimal.Parse(item["OrderPrice"].ToString() != "" ? item["OrderPrice"].ToString() : "0");
                        ritem.OrderID = item["OrderID"].ToString();




                        if (item["OrderStatus"] != null && item["OrderStatus"].ToString().Trim() != "" && item["OrderStatus"].ToString().Trim() != "0")
                        {
                            ritem.OrderStatus = int.Parse(item["OrderStatus"].ToString());

                        }
                        else
                        {
                            ritem.OrderStatus = null;


                        }

                        ritem.BudgetCode = item["BudgetCode"].ToString();
                        ritem.Comments = item["Comments"].ToString();
                        ritem.L2_Remarks = item["L2_Remarks"].ToString();
                        ritem.L3_Remarks = item["L3_Remarks"].ToString();
                        if (item["L2_Qty"] != DBNull.Value && item["L2_Qty"].ToString().Trim() != "0")
                            ritem.L2_Qty = int.Parse(item["L2_Qty"].ToString());


                        ritem.CostCenter = item["CostCenter"].ToString();
                        ritem.BudgetCenterID = item["BudgetCenterID"].ToString().Trim() != "" ? int.Parse(item["BudgetCenterID"].ToString()) : 0;

                        ritem.LabName = item["LabName"].ToString();
                        ritem.RFOReqNTID = item["RFOReqNTID"].ToString();
                        ritem.RFOApprover = item["RFOApprover"].ToString().Trim();
                        ritem.GoodsRecID = item["GoodsRecID"].ToString().Trim();
                        ritem.BudgetCodeDescription = item["BudgetCodeDescription"].ToString();
                        ritem.OrderType = item["OrderType"].ToString().Trim() != "" ? int.Parse(item["OrderType"].ToString()) : 0;
                        ritem.UnitofMeasure = item["UnitofMeasure"].ToString().Trim() != "" ? int.Parse(item["UnitofMeasure"].ToString()) : 0;
                        ritem.UnloadingPoint = item["UnloadingPoint"].ToString().Trim() != "" ? int.Parse(item["UnloadingPoint"].ToString()) : 0;

                        ritem.QuoteAvailable = item["QuoteAvailable"].ToString();
                        ritem.Quote_Vendor_Type = item["Quote_Vendor_Type"].ToString();
                        ritem.Upload_File_Name = item["Upload_File_Name"].ToString();
                        ritem.Material_Part_Number = item["Material_Part_Number"].ToString();
                        ritem.SupplierName_with_Address = item["SupplierName_with_Address"].ToString();
                        ritem.Purchase_Type = item["Purchase_Type"].ToString().Trim() != "" ? int.Parse(item["Purchase_Type"].ToString()) : 0;
                        ritem.Project_ID = item["Project_ID"].ToString();
                        ritem.BM_Number = item["BM_Number"].ToString();
                        ritem.Task_ID = item["Task_ID"].ToString();
                        ritem.PIF_ID = item["PIF_ID"].ToString();
                        ritem.Resource_Group_Id = item["Resource_Group_Id"].ToString();

                        ritem.PRNumber = item["PRNumber"].ToString();
                        ritem.RequestSource = item["RequestSource"].ToString();

                        if (item["Description"].ToString().Trim() != "" && item["Description"].ToString().Trim() != "0")
                        {
                            ritem.Description = BudgetingController.lstOrderDescription.Find(x => x.ID.Equals(int.Parse(item["Description"].ToString()))).Description.ToString();


                        }
                        else
                        {
                            ritem.Description = null;


                        }
                        ritem.LinkedRequestID = item["LinkedRequestID"].ToString();
                        viewList.Add(ritem);
                    }


                    //************************************************************************************************************
                    ////check whether the present user is a ordered stage dept spoc
                    ////if no -> "NOT AUTHORIZED"
                    ////if yes -> check the depts in orderlist == present user's dept ! = null -> show the data
                    ////else get the user's dept -> find its dept id -> check whether it is present in  rep id -> if present -> show that dept's orderlist

                    ////if (db.OrderingRequestor_Table.Find(person => person..Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())) != null)
                    //if(db.OrderingRequestor_Table.ToList().Find(person=>person.NTID.ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())) !=null)
                    //{
                    //    var presentUserNTID = User.Identity.Name.Split('\\')[1].ToUpper();
                    //    is_Authorized = true;
                    //    viewList = viewList1.FindAll(xi => xi.SubmitDate.ToString().Contains(year)).FindAll(xi => xi.ApprovedSH == true).FindAll(xi => xi.OrderStatus != lstOrderStatus.Find(status => status.OrderStatus.Equals("Closed")).ID);

                    //    var presentUserDeptID = lstDEPTs.Find(departmnt => departmnt.DEPT.Equals(lstUsers.Find(xi =>xi.NTID.Trim().ToUpper().Contains(presentUserNTID.Trim())).Department)).ID;
                    //    var presentUserDeptName = lstUsers.Find(xi => xi.NTID.Trim().ToUpper().Contains(presentUserNTID.Trim())).Department;
                    //   // if (viewList.FindAll(y=>y.DEPT.Equals(lstDEPTs.Find(dpt=>dpt.ID).DEPT)).Contains(presentUserDeptName))
                    //   //foreach (var item in viewList)
                    //   //{
                    //   //     var c = lstDEPTs.Find(dept => dept.ID.Equals(item.DEPT)).DEPT;

                    //   //}

                    //   var z = viewList.FindAll(y => y.DEPT.Equals(presentUserDeptID)).Count();     //check the depts in orderlist == present user's dept ! = null

                    //    if (z == 0)    //!=null -> show the data
                    //    {
                    //        return viewList.FindAll(dpt => dpt.DEPT.Equals(presentUserDeptID));
                    //    }
                    //    else          // == null -> check whether userdeptid is present in rep id of DEPT TABLE
                    //    {
                    //        var m = lstDEPTs.FindAll(v => v.Rep_ID.Equals(presentUserDeptID)).Count();
                    //        if (m != 0)  //presentdeptid is a repid of an old dept
                    //        {
                    //            var presentuser_olddeptID = lstDEPTs.Find(v => v.Rep_ID.Equals(presentUserDeptID)).ID;

                    //            var p = viewList.FindAll(y => y.DEPT.Equals(presentuser_olddeptID)).Count();

                    //            return viewList.FindAll(dpt => dpt.DEPT.Equals(presentuser_olddeptID));
                    //        }

                    //        if(m == 0)
                    //        {
                    //            //check the prevdept
                    //        }

                    //    }

                    //}
                    //else
                    //{
                    //    is_Authorized = false;

                    //}

                    //********************************************************************************************************************************



                }
            }
            catch (Exception ex)
            {

            }
            //return viewList.FindAll(y => y.VKM_Year == int.Parse(year) + 1).FindAll(xi => xi.ApprovedSH == true).FindAll(xi => xi.OrderStatus != BudgetingController.lstOrderStatus.Find(status => status.OrderStatus.Equals("Closed")).ID);
            return viewList.FindAll(y => y.VKM_Year == int.Parse(year) + 1).FindAll(xi => xi.OrderStatus != BudgetingController.lstOrderStatus.Find(status => status.OrderStatus.Equals("Closed")).ID);


        }


        public ActionResult ValidateGoodsRecID(string NTID)
        {
            //get Full Name, Department and Group
            VKMPlanningRequestorView data = new VKMPlanningRequestorView();

            if (BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToLower().Equals(NTID.Trim().ToLower())) != null)
            {
                data.NTID = NTID.Trim().ToUpper();

                return Json(new { success = true, data }, JsonRequestBehavior.AllowGet);

            }
            else
            {
                return Json(new { success = false, message = "The NTID is not valid, Please check again!" }, JsonRequestBehavior.AllowGet);
            }
        }
        ///// <summary>
        ///// function to enable update of an existing item and add a new item request
        ///// </summary>
        ///// <param name="req"></param>
        ///// <param name="useryear"></param>
        ///// <returns></returns>
        [HttpPost]
        public ActionResult AddOrEdit(RequestItemsRepoEdit1 req, string useryear/*string[] oem*/, bool popupedit, string Requests) //if popupedit is true => it is bgsw ordering wherein Reqd Dt in not filled - then auto fill with Current+10 weeks; else no need to auto-fill for CC

        {
            //List<RequestItemsRepoView> viewList = new List<RequestItemsRepoView>();


            //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

            ////using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

            //{
            //    System.Security.Principal.IPrincipal User = System.Web.HttpContext.Current.User;
            //    string presentUserName = lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).EmployeeName;

            //    RequestItems_Table item = new RequestItems_Table();
            //    item.PORemarks = req.PORemarks;
            //    item.VKM_Year = req.VKM_Year;
            //    item.RequestorNTID = req.RequestorNTID;
            //    item.BU = req.BU.ToString();
            //    item.ItemName = req.Item_Name.ToString();
            //    item.Category = req.Category.ToString();
            //    item.Comments = req.Comments;
            //    item.CostElement = req.Cost_Element.ToString();
            //    item.DEPT = req.DEPT.ToString();
            //    item.Group = req.Group.ToString();
            //    item.OEM = req.OEM.ToString();
            //    item.ReqQuantity = req.Required_Quantity != 0 ? req.Required_Quantity : req.Reviewed_Quantity; //F03 & F01 items - rev Quantity is given
            //    item.RequestID = req.RequestID;
            //    item.RequestorNT = req.Requestor;
            //    item.DHNT = req.Reviewer_1;
            //    item.SHNT = req.Reviewer_2;


            //    item.UnitPrice = req.Unit_Price;
            //    item.TotalPrice = item.ReqQuantity * req.Unit_Price;
            //    item.ApprQuantity = req.Reviewed_Quantity;
            //    item.OrderedQuantity = req.OrderedQuantity;
            //    item.ApprCost = req.Reviewed_Cost == 0 ? req.Reviewed_Quantity * req.Unit_Price : req.Reviewed_Cost;
            //    item.RequestDate = req.RequestDate != null ? DateTime.Parse(req.RequestDate).Date : DateTime.Now.Date;
            //    item.SubmitDate = req.SubmitDate != null ? DateTime.Parse(req.SubmitDate).Date : DateTime.Now.Date;
            //    item.OrderID = req.OrderID != null ? req.OrderID : "";
            //    item.OrderPrice = req.OrderPrice;
            //    item.ApprovalDH = true;
            //    item.ApprovalSH = true;
            //    item.ApprovedDH = true;
            //    item.ApprovedSH = true;
            //    item.DHAppDate = req.Review1_Date != null ? DateTime.Parse(req.Review1_Date).Date : DateTime.Now.Date;
            //    item.SHAppDate = req.Review2_Date != null ? DateTime.Parse(req.Review2_Date).Date : DateTime.Now.Date;

            //    item.OrderStatus = req.OrderStatus != null ? req.OrderStatus.ToString() : " ";

            //    item.RequestToOrder = req.RequestToOrder != true ? false : true;
            //    item.Fund = req.Fund != 0 ? req.Fund.ToString() : lstFund.Find(fund => fund.Fund.Equals("F02")).ID.ToString();

            //    if (req.RequiredDate != null)
            //    {
            //        item.RequiredDate = DateTime.Parse(req.RequiredDate).Date;
            //    }
            //    if (req.RequestOrderDate != null)
            //    {
            //        item.RequestOrderDate = DateTime.Parse(req.RequestOrderDate).Date;
            //    }
            //    if (req.OrderDate != null)
            //    {
            //        item.OrderDate = DateTime.Parse(req.OrderDate).Date;
            //    }
            //    if (req.TentativeDeliveryDate != null)
            //    {
            //        item.TentativeDeliveryDate = DateTime.Parse(req.TentativeDeliveryDate).Date;
            //    }
            //    if (req.ActualDeliveryDate != null)
            //    {
            //        item.ActualDeliveryDate = DateTime.Parse(req.ActualDeliveryDate).Date;
            //    }

            //    item.BudgetCode = db.ItemsCostList_Table.AsNoTracking().FirstOrDefault(x => x.S_No == req.Item_Name).BudgetCode.ToString();
            //    item.isProjected = req.isProjected;
            //    item.Q1 = req.Q1;
            //    item.Q2 = req.Q2;
            //    item.Q3 = req.Q3;
            //    item.Q4 = req.Q4;
            //    item.Projected_Amount = req.Projected_Amount;
            //    item.Unused_Amount = req.Unused_Amount;
            //    item.RequestorNTID = req.RequestorNTID;
            //    item.L2_Remarks = req.L2_Remarks;
            //    item.L3_Remarks = req.L3_Remarks;
            //    item.L2_Qty = req.L2_Qty;
            //    item.Is_UnplannedF02Item = req.Is_UnplannedF02Item;



            //    if (req.RequestID == 0)
            //    {
            //        if (item.Fund == lstFund.Find(fund => fund.Fund.Equals("F02")).ID.ToString())
            //        {
            //            viewList = GetData1(useryear);
            //            return Json(new { success = false, data = viewList, message = "Cannot add F02 items right now since Request Window has been closed." + "\n" + " Only F01/F03 items can be added at this stage!" }, JsonRequestBehavior.AllowGet);
            //        }
            //        else
            //        {
            //            db.RequestItems_Table.Add(item);
            //            db.SaveChanges();


            //            //viewList = GetData1(useryear);


            //            return Json(new { success = true,/*, data = viewList,*/ message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
            //        }

            //    }
            //    else
            //    {


            //        db.Entry(item).State = EntityState.Modified;

            //        db.SaveChanges();

            //        //viewList = GetData1(useryear);

            //        return Json(new { success = true, message = "Updated Successfully" }, JsonRequestBehavior.AllowGet);
            //    }
            //}

            connection();
            SqlCommand command = new SqlCommand();
            //Stored procedure specification
            command.Connection = conn;
            command.CommandText = "dbo.[RFO_AddorEdit]";
            command.CommandType = CommandType.StoredProcedure;


            // Add the input parameter and set its properties.

            SqlParameter parameter1 = new SqlParameter();
            parameter1.ParameterName = "@NTID";
            parameter1.SqlDbType = SqlDbType.NVarChar;
            parameter1.Direction = ParameterDirection.Input;
            parameter1.Value = User.Identity.Name.Split('\\')[1].ToUpper();

            SqlParameter parameter2 = new SqlParameter();
            parameter2.ParameterName = "@VKMYear";
            parameter2.SqlDbType = SqlDbType.NVarChar;
            parameter2.Direction = ParameterDirection.Input;
            parameter2.Value = (int.Parse(useryear) + 1);

            SqlParameter parameter3 = new SqlParameter();
            parameter3.ParameterName = "@BU";
            parameter3.SqlDbType = SqlDbType.NVarChar;
            parameter3.Direction = ParameterDirection.Input;
            parameter3.Value = req.BU;

            SqlParameter parameter4 = new SqlParameter();
            parameter4.ParameterName = "@ItemName";
            parameter4.SqlDbType = SqlDbType.NVarChar;
            parameter4.Direction = ParameterDirection.Input;
            parameter4.Value = req.Item_Name;

            SqlParameter parameter5 = new SqlParameter();
            parameter5.ParameterName = "@Comments";
            parameter5.SqlDbType = SqlDbType.Text;
            parameter5.Direction = ParameterDirection.Input;
            parameter5.Value = req.PORemarks == null ? "-" : req.PORemarks;
            string PORemarks = req.PORemarks == null ? "-" : req.PORemarks;

            SqlParameter parameter6 = new SqlParameter();
            parameter6.ParameterName = "@Dept";
            parameter6.SqlDbType = SqlDbType.NVarChar;
            parameter6.Direction = ParameterDirection.Input;
            parameter6.Value = req.DEPT;

            SqlParameter parameter7 = new SqlParameter();
            parameter7.ParameterName = "@Group";
            parameter7.SqlDbType = SqlDbType.NVarChar;
            parameter7.Direction = ParameterDirection.Input;
            parameter7.Value = req.Group;

            SqlParameter parameter8 = new SqlParameter();
            parameter8.ParameterName = "@OEM";
            parameter8.SqlDbType = SqlDbType.NVarChar;
            parameter8.Direction = ParameterDirection.Input;
            parameter8.Value = req.OEM;


            SqlParameter parameter9 = new SqlParameter();
            parameter9.ParameterName = "@ReqQuantity";
            parameter9.SqlDbType = SqlDbType.NVarChar;
            parameter9.Direction = ParameterDirection.Input;
            parameter9.Value = req.Reviewed_Quantity;


            SqlParameter parameter10 = new SqlParameter();
            parameter10.ParameterName = "@RequestID";
            parameter10.SqlDbType = SqlDbType.NVarChar;
            parameter10.Direction = ParameterDirection.Input;
            parameter10.Value = req.RequestID;

            SqlParameter parameter11 = new SqlParameter();
            parameter11.ParameterName = "@GoodsRecID";
            parameter11.SqlDbType = SqlDbType.NVarChar;
            parameter11.Direction = ParameterDirection.Input;
            parameter11.Value = req.GoodsRecID ?? "";


            SqlParameter parameter12 = new SqlParameter();
            parameter12.ParameterName = "@RFOReqNTID";
            parameter12.SqlDbType = SqlDbType.NVarChar;
            parameter12.Direction = ParameterDirection.Input;
            parameter12.Value = req.RFOReqNTID ?? User.Identity.Name.Split('\\')[1].ToUpper();
            string rfontid = req.RFOReqNTID ?? User.Identity.Name.Split('\\')[1].ToUpper();


            SqlParameter parameter13 = new SqlParameter();
            parameter13.ParameterName = "@BudgetCenterID";
            parameter13.SqlDbType = SqlDbType.NVarChar;
            parameter13.Direction = ParameterDirection.Input;
            parameter13.Value = req.BudgetCenterID == null ? "" : req.BudgetCenterID;


            SqlParameter parameter14 = new SqlParameter();
            parameter14.ParameterName = "@UnloadingPoint";
            parameter14.SqlDbType = SqlDbType.NVarChar;
            parameter14.Direction = ParameterDirection.Input;
            parameter14.Value = req.UnloadingPoint ?? "";

            SqlParameter parameter15 = new SqlParameter();
            parameter15.ParameterName = "@RFOApprover";
            parameter15.SqlDbType = SqlDbType.NVarChar;
            parameter15.Direction = ParameterDirection.Input;
            parameter15.Value = req.RFOApprover ?? "";

            SqlParameter parameter16 = new SqlParameter();
            parameter16.ParameterName = "@BudgetCodeDescription";
            parameter16.SqlDbType = SqlDbType.NVarChar;
            parameter16.Direction = ParameterDirection.Input;
            parameter16.Value = req.BudgetCodeDescription ?? "";

            SqlParameter parameter17 = new SqlParameter();
            parameter17.ParameterName = "@UnitofMeasure";
            parameter17.SqlDbType = SqlDbType.NVarChar;
            parameter17.Direction = ParameterDirection.Input;
            parameter17.Value = req.UnitofMeasure ?? "";

            SqlParameter parameter18 = new SqlParameter();
            parameter18.ParameterName = "@QuoteAvailable";
            parameter18.SqlDbType = SqlDbType.NVarChar;
            parameter18.Direction = ParameterDirection.Input;
            parameter18.Value = req.QuoteAvailable == null ? "" : req.QuoteAvailable;

            SqlParameter parameter19 = new SqlParameter();
            parameter19.ParameterName = "@LabName";
            parameter19.SqlDbType = SqlDbType.NVarChar;
            parameter19.Direction = ParameterDirection.Input;
            parameter19.Value = req.LabName == null ? "" : req.LabName;
            string labname = req.LabName == null ? "" : req.LabName;

            SqlParameter parameter20 = new SqlParameter();
            parameter20.ParameterName = "@OrderType";
            parameter20.SqlDbType = SqlDbType.NVarChar;
            parameter20.Direction = ParameterDirection.Input;
            parameter20.Value = req.OrderType ?? "";

            SqlParameter parameter21 = new SqlParameter();
            parameter21.ParameterName = "@CostCenter";
            parameter21.SqlDbType = SqlDbType.NVarChar;
            parameter21.Direction = ParameterDirection.Input;
            parameter21.Value = req.CostCenter ?? "";

            SqlParameter parameter22 = new SqlParameter();
            parameter22.ParameterName = "@Fund";
            parameter22.SqlDbType = SqlDbType.NVarChar;
            parameter22.Direction = ParameterDirection.Input;
            parameter22.Value = req.Fund;

            SqlParameter parameter23 = new SqlParameter();
            parameter23.ParameterName = "@Material_Part_Number";
            parameter23.SqlDbType = SqlDbType.NVarChar;
            parameter23.Direction = ParameterDirection.Input;
            parameter23.Value = req.Material_Part_Number == null? "" : req.Material_Part_Number;
            string materialno = req.Material_Part_Number == null ? "" : req.Material_Part_Number;

            SqlParameter parameter24 = new SqlParameter();
            parameter24.ParameterName = "@SupplierName_with_Address";
            parameter24.SqlDbType = SqlDbType.NVarChar;
            parameter24.Direction = ParameterDirection.Input;
            parameter24.Value = req.SupplierName_with_Address == null? "" : req.SupplierName_with_Address;
            string suppliernameaddress = req.SupplierName_with_Address == null ? "" : req.SupplierName_with_Address;

            SqlParameter parameter25 = new SqlParameter();
            parameter25.ParameterName = "@Purchase_Type";
            parameter25.SqlDbType = SqlDbType.NVarChar;
            parameter25.Direction = ParameterDirection.Input;
            parameter25.Value = req.Purchase_Type == null? "" : req.Purchase_Type;
            string purchasetype = req.Purchase_Type == null ? "" : req.Purchase_Type;

            SqlParameter parameter26 = new SqlParameter();
            parameter26.ParameterName = "@Project_ID";
            parameter26.SqlDbType = SqlDbType.NVarChar;
            parameter26.Direction = ParameterDirection.Input;
            parameter26.Value = req.Project_ID == null? "" : req.Project_ID;
            string projectid = req.Project_ID == null ? "" : req.Project_ID;

            SqlParameter parameter27 = new SqlParameter();
            parameter27.ParameterName = "@BM_Number";
            parameter27.SqlDbType = SqlDbType.NVarChar;
            parameter27.Direction = ParameterDirection.Input;
            parameter27.Value = req.BM_Number == null? "" : req.BM_Number;
            string bmnumber = req.BM_Number == null ? "" : req.BM_Number;

            SqlParameter parameter28 = new SqlParameter();
            parameter28.ParameterName = "@Task_ID";
            parameter28.SqlDbType = SqlDbType.NVarChar;
            parameter28.Direction = ParameterDirection.Input;
            parameter28.Value = req.Task_ID == null ? "" : req.Task_ID;
            string taskid = req.Task_ID == null ? "" : req.Task_ID;

            SqlParameter parameter29 = new SqlParameter();
            parameter29.ParameterName = "@PIF_ID";
            parameter29.SqlDbType = SqlDbType.NVarChar;
            parameter29.Direction = ParameterDirection.Input;
            parameter29.Value = req.PIF_ID == null ? "" : req.PIF_ID;
            string pifid = req.PIF_ID == null ? "" : req.PIF_ID;

            SqlParameter parameter30 = new SqlParameter();
            parameter30.ParameterName = "@Resource_Group_Id";
            parameter30.SqlDbType = SqlDbType.NVarChar;
            parameter30.Direction = ParameterDirection.Input;
            parameter30.Value = req.Resource_Group_Id == null ? "" : req.Resource_Group_Id;
            string resourcegroupid = req.Resource_Group_Id == null ? "" : req.Resource_Group_Id;

            SqlParameter parameter31 = new SqlParameter();
            parameter31.ParameterName = "@RequiredDate";
            parameter31.SqlDbType = SqlDbType.NVarChar;
            parameter31.Direction = ParameterDirection.Input;
            ////parameter31.Value = req.RequiredDate != null ? req.RequiredDate : (popupedit == false ? "" : DateTime.Now.AddDays(10*7).ToString("yyyy-MM-dd")); //Store Reqd Dt as Current date + 10 weeks for BGSW Requests other than CC, since it will only be filled by CC Users
            //parameter31.Value = req.RequiredDate != null ? req.RequiredDate : ""; //Store Reqd Dt as Current date + 10 weeks for BGSW Requests other than CC, since it will only be filled by CC Users
            //string requireddate = req.RequiredDate != null ? req.RequiredDate : "";
            parameter31.Value = (req.RequiredDate != null ? DateTime.Parse(new string(req.RequiredDate.Take(24).ToArray())).ToString("yyyy-MM-dd") : "");
            string requireddate = (req.RequiredDate != null ? DateTime.Parse(new string(req.RequiredDate.Take(24).ToArray())).ToString("yyyy-MM-dd") : req.RequiredDate);

            SqlParameter parameter32 = new SqlParameter();
            parameter32.ParameterName = "@HOE";
            parameter32.SqlDbType = SqlDbType.NVarChar;
            parameter32.Direction = ParameterDirection.Input;
            parameter32.Value = req.Reviewer_1 == null ? "" : req.Reviewer_1;
            string reviewer1 = req.Reviewer_1 == null ? "" : req.Reviewer_1;

            SqlParameter parameter33 = new SqlParameter();
            parameter33.ParameterName = "@VKMSPOC";
            parameter33.SqlDbType = SqlDbType.NVarChar;
            parameter33.Direction = ParameterDirection.Input;
            parameter33.Value = req.Reviewer_2 == null ? "" : req.Reviewer_2;
            string reviewer2 = req.Reviewer_2 == null ? "" : req.Reviewer_2;

            SqlParameter parameter34 = new SqlParameter();
            parameter34.SqlDbType = SqlDbType.Int;
            parameter34.ParameterName = "@OutputID";
            parameter34.Direction = ParameterDirection.Output;
            parameter34.Value = req.RequestID;

            SqlParameter parameter35 = new SqlParameter();
            parameter35.ParameterName = "@Quote_Vendor_Type";
            parameter35.SqlDbType = SqlDbType.NVarChar;
            parameter35.Direction = ParameterDirection.Input;
            parameter35.Value = req.Quote_Vendor_Type ?? "";
            string Quote_Vendor_Type = req.Quote_Vendor_Type ?? "";

            SqlParameter parameter36 = new SqlParameter();
            parameter36.ParameterName = "@Upload_File_Name";
            parameter36.SqlDbType = SqlDbType.NVarChar;
            parameter36.Direction = ParameterDirection.Input;
            parameter36.Value = req.Upload_File_Name ?? "";
            string Upload_File_Name = req.Upload_File_Name ?? "";
            // RequestResorde is flag to identify the approval flow, where its comes from
            
            SqlParameter parameter37 = new SqlParameter();
            parameter37.ParameterName = "@RequestSource";
            parameter37.SqlDbType = SqlDbType.NVarChar;
            parameter37.Direction = ParameterDirection.Input;
            parameter37.Value = req.RequestSource == null ? "RFO" : req.RequestSource;

            SqlParameter parameter38 = new SqlParameter();
            parameter38.ParameterName = "@Requests";
            parameter38.SqlDbType = SqlDbType.NVarChar;
            parameter38.Direction = ParameterDirection.Input;
            parameter38.Value = Requests;


            command.Parameters.Add(parameter1);
            command.Parameters.Add(parameter2);
            command.Parameters.Add(parameter3);
            command.Parameters.Add(parameter4);
            command.Parameters.Add(parameter5);
            command.Parameters.Add(parameter6);
            command.Parameters.Add(parameter7);
            command.Parameters.Add(parameter8);
            command.Parameters.Add(parameter9);
            command.Parameters.Add(parameter10);
            command.Parameters.Add(parameter11);
            command.Parameters.Add(parameter12);
            command.Parameters.Add(parameter13);
            command.Parameters.Add(parameter14);
            command.Parameters.Add(parameter15);
            command.Parameters.Add(parameter16);
            command.Parameters.Add(parameter17);
            command.Parameters.Add(parameter18);

            command.Parameters.Add(parameter35);
            command.Parameters.Add(parameter36);

            command.Parameters.Add(parameter19);
            command.Parameters.Add(parameter20);
            command.Parameters.Add(parameter21);
            command.Parameters.Add(parameter22);
            command.Parameters.Add(parameter23);

            command.Parameters.Add(parameter24);
            command.Parameters.Add(parameter25);
            command.Parameters.Add(parameter26);
            command.Parameters.Add(parameter27);
            command.Parameters.Add(parameter28);
            command.Parameters.Add(parameter29);
            command.Parameters.Add(parameter30);
            command.Parameters.Add(parameter31);
            command.Parameters.Add(parameter32);
            command.Parameters.Add(parameter33);
            command.Parameters.Add(parameter34);
            command.Parameters.Add(parameter37);
            command.Parameters.Add(parameter38);

            //OpenConnection();
            //WriteLog("Executing STORED PROCEDURE");
            //command.CommandTimeout = 300; //5 min

            //ErrorMsg = command.ExecuteScalar().ToString();
            //WriteLog("ErrorMsg: " + ErrorMsg);
            //command.ExecuteNonQuery();
            try
            {
                //string query = "EXEC dbo.[RFO_AddorEdit] '" + User.Identity.Name.Split('\\')[1].ToUpper() + "'," + (int.Parse(useryear) + 1) + ",'" + req.BU.ToString() + "','" + req.Item_Name.ToString() + "','" + PORemarks + "','" + req.DEPT.ToString() + "','" + req.Group.ToString() + "','" + req.OEM.ToString() + "'," + req.Required_Quantity + "," + req.RequestID + ",'" + req.GoodsRecID.ToString() + "','" + rfontid + "','" + req.BudgetCenterID.ToString() + "','" + req.UnloadingPoint.ToString() + "','" + req.RFOApprover.ToString() + "','" + req.BudgetCodeDescription + "','" + req.UnitofMeasure + "','" + req.QuoteAvailable + "','" + Quote_Vendor_Type + "','" + Upload_File_Name + "','" + labname + "','" + req.OrderType + "','" + req.CostCenter + "','" + req.Fund + "','" + materialno + "','" + suppliernameaddress + "','" + purchasetype + "','" + projectid.ToString() + "','" + bmnumber.ToString() + "','" + taskid.ToString() + "','" + pifid.ToString() + "','" + resourcegroupid.ToString() + "','" + requireddate + "','" + reviewer1 + "','" + reviewer2 + "'," + req.RequestID + "";
                OpenConnection();
                
                command.ExecuteNonQuery();
                //SqlCommand cmd = new SqlCommand(query, conn);
                //int RequestID = cmd.ExecuteNonQuery();
                int RequestID = Convert.ToInt32(command.Parameters["@OutputID"].Value.ToString());
                
                CloseConnection();



                //if(result.Count == 0)
                //{
                //    cmd1.ExecuteNonQuery();
                //}

                if (RequestID > 0)
                {
                    Console.WriteLine("Records Inserted Successfully");
                    return Json(new { /*data = GetSpareData(),*/ success = true, RequestID = RequestID }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    Console.WriteLine("Plan amount should not be exceeded the CTG approved amount");
                    return Json(new { message = "Budget exceeded. Contact ELO!", RequestID = RequestID, success = false }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                // alert()
                // MessageBox.Show(" Not Updated");
                CloseConnection();
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
            finally
            {
                CloseConnection();
            }

        }

        [HttpPost]
        // "file" is the value of the FileUploader's "name" property
        public ActionResult AsyncFileUpload(FormCollection formdata) //(FormCollection formdata, string id="")
        {
            string presentUserDept1 = "", presentUserSection1 = "", presentUserTopSection1 = "";
            string presentUserDept = "", presentUserSection = "", presentUserTopSection = "", id = "";
            id = formdata["id"].ToString();



            connection();
            //string qry = "select TopSection,Section, Department from SPOTONData_Table_2022 where NTID = '" + User.Identity.Name.Split('\\')[1].ToUpper() + "' ";

            string qry = "select TopSection,Section, Department from SPOTONData_Table_2022 where Department = (Select Dept from DEPT_Table where ID = (Select DEPT from RequestItems_Table where RequestID = '"+id+"'))";


            SqlDataReader dr1;
            OpenConnection();
            SqlCommand cmd1 = new SqlCommand(qry, conn);
            dr1 = cmd1.ExecuteReader();
            if (dr1.HasRows)
            {
                dr1.Read();
                presentUserDept1 = dr1["Department"].ToString();
                presentUserSection1 = dr1["Section"].ToString();
                presentUserTopSection1 = dr1["TopSection"].ToString();
            }
            dr1.Close();
            CloseConnection();



            presentUserSection = presentUserSection1.Replace("/", "_");
            presentUserTopSection = presentUserTopSection1.Replace("/", "_");



            if (!System.IO.Directory.Exists(Server.MapPath("~/BGSW_Ordering/BGSW_VKM" + DateTime.Now.Year.ToString() + "/" + presentUserTopSection + "/" + presentUserSection + "/" + id)))
            {
                System.IO.Directory.CreateDirectory(Server.MapPath("~/BGSW_Ordering/BGSW_VKM" + DateTime.Now.Year.ToString() + "/" + presentUserTopSection + "/" + presentUserSection + "/" + id));
            }





            // Specifies the target location for the uploaded files
            //string targetLocation = Server.MapPath("~/UploadedFiles/");
            string targetLocation = Server.MapPath("~/BGSW_Ordering/BGSW_VKM" + DateTime.Now.Year.ToString() + "/" + presentUserTopSection + "/" + presentUserSection + "/" + id);



            // Specifies the maximum size allowed for the uploaded files (700 kb)
            //int maxFileSize = 1024 * 700;



            // Checks whether or not the request contains a file and if this file is empty or not
            //if (formdata == null || formdata.ContentLength <= 0)
            //{
            //    throw new HttpException("File is not specified");
            //}



            // Checks that the file size does not exceed the allowed size
            //if (file.ContentLength > maxFileSize)
            //{
            //    throw new HttpException("File is too big");
            //}



            // Checks that the file is an image
            //if (!file.ContentType.Contains("image"))
            //{
            //    throw new HttpException("Invalid file type");
            //}



            try
            {
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    //string path = System.IO.Path.Combine(targetLocation, Request.Files[0].FileName);
                    //if (!Request.Files[i].ContentType.Contains("pdf")) //here, checked for pdf
                    //{
                    //    //return flag to js and use the flag to notify user that the file "xxx" is invalid
                    //}
                    string path = System.IO.Path.Combine(targetLocation, Request.Files[i].FileName);
                    // Here, make sure that the file will be saved to the required directory.
                    // Also, ensure that the client has not uploaded files with malicious content.
                    // If all checks are passed, save the file.
                    //formdata.SaveAs(path);
                    Request.Files[i].SaveAs(path);
                }



                //trial to check if file name can be saved
                //connection();
                //string Query = "";
                //Query = "UPDATE [RequestItems_Table] SET POCopy_FileName = CASE WHEN(POCopy_FileName IS null OR TRIM(POCopy_FileName) = '') THEN @POFileName ELSE CONCAT([POCopy_FileName],',',@POFileName) END WHERE RequestID = @ID";



                //SqlCommand cmd = new SqlCommand(Query, conn);
                //cmd.Parameters.AddWithValue("@POFileName", Request.Files[0].FileName);
                ////cmd.Parameters.AddWithValue("@ID", id);



                //OpenConnection();
                //cmd.ExecuteNonQuery();



            }
            catch (Exception e)
            {
                throw new HttpException("Invalid file name");

            }
            return new EmptyResult();



        }


        /// <summary>
        /// function to move the item to L2 review
        /// </summary>
        /// <param name="id">Item ID</param>
        /// <param name="useryear"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult LabAdminApprove(int id, string useryear)//yes
        {

            Emailnotify_OrderStage emailnotify = new Emailnotify_OrderStage();
            Emailnotify_RFOApprover emailnotifyRFOApprover = new Emailnotify_RFOApprover();
            bool is_MailTrigger = false;
            // Nullable<bool> isRequestToOrder = false;
            int isRequestToOrder = 0;
            DateTime dateTime = new DateTime();
            List<RequestItemsRepoView> viewList = new List<RequestItemsRepoView>();

            using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

            //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

            {
                System.Security.Principal.IPrincipal User = System.Web.HttpContext.Current.User;
                string presentUserName = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).EmployeeName;

                if (id == 1999999999)
                {
                    List<RequestItems_Table> templist = new List<RequestItems_Table>().FindAll(y => y.VKM_Year == int.Parse(useryear) + 1).FindAll(items => items.ApprovedSH == true);
                    templist = db.RequestItems_Table.ToList();
                    Nullable<decimal> totalAmount = 0.0M;
                    string RequestNT = "";




                    int totalCount = templist.FindAll(x => x.RequestorNT == presentUserName).FindAll(item => item.RequestToOrder == false).FindAll(items => items.RequiredDate != null).Count;

                    foreach (RequestItems_Table item in templist.FindAll(x => x.RequestorNT == presentUserName).FindAll(items => items.RequestToOrder == false).FindAll(items => items.RequiredDate != null).FindAll(items => items.RequestDate.ToString().Contains(DateTime.Now.Year.ToString())))
                    {

                        RequestItems_Table changeItem = db.RequestItems_Table.Where(x => x.RequestID == item.RequestID).FirstOrDefault<RequestItems_Table>();
                        totalAmount += item.ApprCost;
                        RequestNT = item.RequestorNT;
                        var isRequestToOrder_flag = item.RequestToOrder;  //request to order flag state at the time of Request to Order Trigger
                                                                          //if (isRequestToOrder_flag == false)
                                                                          //{
                        isRequestToOrder = 1; //Always from Requestor->LAbteam
                        //}
                        //if (isRequestToOrder_flag == true)
                        //{
                        //    isRequestToOrder = 2;
                        //}
                        is_MailTrigger = true;
                        dateTime = (DateTime)item.SubmitDate;

                        changeItem.RequestToOrder = true;
                        changeItem.RequestOrderDate = DateTime.Now.Date;
                        item.OrderStatus = BudgetingController.lstOrderStatus.Find(status => status.OrderStatus.Equals("Open")).ID.ToString();



                        db.Entry(changeItem).State = EntityState.Modified;
                        db.SaveChanges();


                    }
                    emailnotify.RequestID_orderemail = id;
                    emailnotify.is_RequesttoOrder = isRequestToOrder;
                    emailnotify.NTID_ccEmail = RequestNT;
                    emailnotify.TotalAmount = totalAmount;
                    emailnotify.Count = totalCount;
                    emailnotify.SubmitDate_ofRequest = dateTime;




                    return Json(new { data = emailnotify, is_MailTrigger, emailnotifyRFOApprover, success = true, message = totalCount.ToString() + " Request to Order sent for the Item(s) Successfully" }, JsonRequestBehavior.AllowGet);

                }
                else
                {

                    RequestItems_Table item = db.RequestItems_Table.Where(x => x.RequestID == id).FirstOrDefault<RequestItems_Table>();

                    if (item.RequiredDate != null)
                    {
                        var isRequestToOrder_flag = item.RequestToOrder;  //request to order flag state at the time of Request to Order Trigger
                        if (isRequestToOrder_flag == false)
                        {
                            if(item.RequestSource != null && item.RequestSource.Trim() == "RFO" && (item.Fund.Trim() == "2")  && item.BudgetCode.Trim().StartsWith("2")) //200 series
                            {
                                isRequestToOrder = 1;
                                
                                if (BudgetingController.lstUsers.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(item.SHNT.ToLower().Trim())) != null)
                                {
                                    emailnotify.VKMSPOC_NTID = BudgetingController.lstUsers.FirstOrDefault(user => user.EmployeeName.ToLower().Trim().Equals(item.SHNT.ToLower().Trim())).NTID.ToLower().Trim();

                                }
                            }
                            else
                            {
                                isRequestToOrder = 1;
                                emailnotify.POSPOC_NTID = GetSectionCoordinatorsNTID(item.DEPT);
                                emailnotify.getTOemail = GetCommonMail(id);
                                
                            }
                            emailnotify.RFOReqNTID = item.RFOReqNTID;

                        }
                        if (isRequestToOrder_flag == true)
                        {
                            isRequestToOrder = 2;
                        }
                        is_MailTrigger = true;

                        item.RequestToOrder = true;
                        item.RequestOrderDate = DateTime.Now.Date;
                        item.isCancelled = null;
                        item.OrderDescriptionID = null;
                        item.OrderStatus = BudgetingController.lstOrderStatus.Find(status => status.OrderStatus.Equals("Open")).ID.ToString();

                        emailnotify.is_RequesttoOrder = isRequestToOrder;
                        emailnotify.RequestID_orderemail = item.RequestID;

                        string RFOApproverNTID = GetRFOApprover(item.RequestID);
                        if (RFOApproverNTID != "")
                        {
                            emailnotifyRFOApprover.RequestID = item.RequestID;
                            emailnotifyRFOApprover.Project = (item.Project != null) ? item.Project.ToString() : "";
                            emailnotifyRFOApprover.ItemDescription = BudgetingController.lstItems.Find(x => x.S_No.ToString().Equals(item.ItemName.ToString())).Item_Name;
                            emailnotifyRFOApprover.Quantity = (int)item.ReqQuantity;
                            emailnotifyRFOApprover.TotalPrice = (item.TotalPrice != null) ? (decimal)item.TotalPrice : 0;
                            emailnotifyRFOApprover.Remarks = (item.Comments != null) ? item.Comments.ToString() : "";
                            emailnotifyRFOApprover.RFOApprover = RFOApproverNTID;
                            emailnotifyRFOApprover.RFOApproverName = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(RFOApproverNTID)).EmployeeName;
                        }



                        db.Entry(item).State = EntityState.Modified;
                        db.SaveChanges();

                        return Json(new { data = emailnotify, is_MailTrigger, emailnotifyRFOApprover, success = true, message = "Request to Order sent Successfully" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                        return Json(new { success = false, message = "Please fill in the Required Date!" }, JsonRequestBehavior.AllowGet);


                }


            }



        }


        /// <summary>
        /// function to delete an existing item
        /// </summary>
        /// <param name="id"></param>
        /// <param name="useryear"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(int id, string useryear/*string[] oem*/)
        {
            List<RequestItemsRepoView> viewList = new List<RequestItemsRepoView>();


            using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

            //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

            {
                RequestItems_Table item = db.RequestItems_Table.Where(x => x.RequestID == id).FirstOrDefault<RequestItems_Table>();
                db.RequestItems_Table.Remove(item);
                db.SaveChanges();
                viewList = GetData1(useryear);
                return Json(new { data = viewList, success = true, message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
            }
        }


        /// <summary>
        /// Function to help the export to excel function 
        /// Path to export - default or input from User
        /// Feedback to user after saving
        /// </summary>
        public ActionResult ExportDataToExcel(string useryear)
        {

            string filename = @"Order_List_" + (int.Parse(useryear) + 1) + ".xlsx";

            using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

            //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

            {
                System.Security.Principal.IPrincipal User = System.Web.HttpContext.Current.User;
                string presentUserName = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).EmployeeName;
                //string userdept_inorderreqtable = db.OrderingRequestor_Table.ToList().Find(person => person.NTID.ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).Department.Trim();

                //string presentUserDept = lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).Department;

                System.Data.DataTable dt = new System.Data.DataTable("Request_List");
                dt.Columns.AddRange(new DataColumn[50] { new DataColumn("Business Unit"),
                                            new DataColumn("OEM"),
                                            new DataColumn("Department"),
                                            new DataColumn("Group"),
                                            new DataColumn("Item Name"),
                                            new DataColumn("Category"),
                                            new DataColumn("Cost Element"),
                                            new DataColumn("Budget Code"),
                                            new DataColumn("Unit Price",typeof(decimal)),
                                            new DataColumn("Required Quantity",typeof(int)),
                                            new DataColumn("Total Price",typeof(decimal)),
                                            new DataColumn("Reviewed Quantity",typeof(Int32)),
                                            new DataColumn("Reviewed Price",typeof(decimal)),
                                            new DataColumn("Comments"),
                                            new DataColumn("Requestor"),
                                            new DataColumn("Submit Date"),
                                            new DataColumn("Reviewer 1"),
                                            new DataColumn("Review 1 Date"),
                                            new DataColumn("Reviewer 2"),
                                            new DataColumn("Review 2 Date"),
                                            new DataColumn("Required Date"),
                                            new DataColumn("Request Order Date"),
                                            new DataColumn("Order Date"),
                                            new DataColumn("Tentative Delivery Date"),
                                            new DataColumn("Actual Delivery Date"),
                                            new DataColumn("Fund"),
                                            new DataColumn("Order ID"),
                                            new DataColumn("Ordered Quantity",typeof(Int32)),
                                            new DataColumn("Order Price",typeof(decimal)),
                                            new DataColumn("Order Status"),
                                            new DataColumn("BudgetCodeDescription"),
                                            new DataColumn("Order_Type"),
                                            new DataColumn("CostCenter"),
                                           // new DataColumn("BudgetCenterID"),
                                            new DataColumn("LabName"),
                                            new DataColumn("RFOReqNTID"),
                                            new DataColumn("RFOApprover"),
                                            new DataColumn("GoodsRecID"),
                                            new DataColumn("UnitofMeasure"),
                                            new DataColumn("UnloadingPoint"),
                                            new DataColumn("QuoteAvailable"),
                                            new DataColumn("Material_Part_Number"),
                                            new DataColumn("Supplier_Name_with_Address"),
                                            new DataColumn("Purchase_Type"),
                                            new DataColumn("Project_ID"),
                                            new DataColumn("BM_Number"),
                                            new DataColumn("Task_ID"),
                                            new DataColumn("PIF_ID"),
                                            new DataColumn("Resource_Group_Id"),
                                            new DataColumn("PR Number"),
                                            new DataColumn("Order Status Description")



                });
                // string PresentUserDept_RequestTable = lstDEPTs.Find(dept => dept.DEPT.Equals(userdept_inorderreqtable)).ID.ToString();
                var requests1 = db.RequestItems_Table.ToList().FindAll(y => y.VKM_Year == int.Parse(useryear) + 1).Select(x => new
                {
                    x.BU,
                    x.OEM,
                    x.DEPT,
                    x.Group,
                    x.ItemName,
                    x.Category,
                    x.CostElement,
                    x.BudgetCode,
                    x.UnitPrice, /*x.Currency,*/
                    x.ReqQuantity,
                    x.TotalPrice,
                    x.ApprQuantity,
                    x.ApprCost,
                    x.Comments,
                    x.RequestorNT,
                    x.SubmitDate,
                    x.DHNT,
                    x.DHAppDate,
                    x.SHNT,
                    x.SHAppDate,
                    x.RequiredDate,
                    x.RequestOrderDate,
                    x.OrderDate,
                    x.TentativeDeliveryDate,
                    x.ActualDeliveryDate,
                    x.OrderID,
                    x.OrderedQuantity,
                    x.OrderPrice,
                    x.OrderStatus,
                    x.Fund,
                    x.ApprovedDH,
                    x.ApprovedSH,
                    x.BudgetCodeDescription,
                    x.Order_Type,
                    x.CostCenter,
                    x.BudgetCenterID,
                    x.LabName,
                    x.RFOReqNTID,
                    x.RFOApprover,
                    x.GoodsRecID,
                    x.UnitofMeasure,
                    x.UnloadingPoint,
                    x.QuoteAvailable,
                    x.Material_Part_Number,
                    x.Supplier_Name_with_Address,
                    x.Purchase_Type,
                    x.Project_ID,
                    x.BM_Number,
                    x.Task_ID,
                    x.PIF_ID,
                    x.Resource_Group_Id,
                    x.PRnumber,
                    x.OrderDescriptionID

                }).ToList().FindAll(x => x.ApprovedSH == true);
                var puser = User.Identity.Name.Split('\\')[1].ToUpper().Trim();
                if (db.OrderingRequestor_Table.ToList().Find(person => person.NTID.ToUpper().Equals(puser)) != null)
                {
                    if (db.OrderingRequestor_Table.ToList().Find(person => person.NTID.ToUpper().Equals(puser)).isSuperUser != null && db.OrderingRequestor_Table.ToList().Find(person => person.NTID.ToUpper().Equals(puser)).isSuperUser == true)
                    {

                        List<string> BUs_rfo = db.OrderingRequestor_Table.ToList().Find(person => person.NTID.ToUpper().Equals(puser)).BU.Split(',').ToList();
                        // var detail_superuser = viewList.FindAll(x => BUs_rfo.Contains(x.BU.ToString().Trim()));
                        var requests = requests1.FindAll(x => BUs_rfo.Contains(x.BU.ToString().Trim())).Select(x => new
                        {
                            x.BU,
                            x.OEM,
                            x.DEPT,
                            x.Group,
                            x.ItemName,
                            x.Category,
                            x.CostElement,
                            x.BudgetCode,
                            x.UnitPrice, /*x.Currency,*/
                            x.ReqQuantity,
                            x.TotalPrice,
                            x.ApprQuantity,
                            x.ApprCost,
                            x.Comments,
                            x.RequestorNT,
                            x.SubmitDate,
                            x.DHNT,
                            x.DHAppDate,
                            x.SHNT,
                            x.SHAppDate,
                            x.RequiredDate,
                            x.RequestOrderDate,
                            x.OrderDate,
                            x.TentativeDeliveryDate,
                            x.ActualDeliveryDate,
                            x.OrderID,
                            x.OrderedQuantity,
                            x.OrderPrice,
                            x.OrderStatus,
                            x.Fund,
                            x.PRnumber,
                            x.OrderDescriptionID
                        }).ToList();

                        foreach (var request in requests)
                        {

                            dt.Rows.Add(
                                BudgetingController.lstBUs.Find(bu => bu.ID.ToString().Equals(request.BU)).BU,
                                BudgetingController.lstOEMs.Find(oem => oem.ID.ToString().Equals(request.OEM)).OEM,
                                BudgetingController.lstDEPTs.Find(dept => dept.ID.ToString().Equals(request.DEPT)).DEPT,
                                BudgetingController.lstGroups_test.Find(grp => grp.ID.ToString().Equals(request.Group)).Group,
                                request.ItemName != null && request.ItemName != "0" && request.ItemName != "" ? BudgetingController.lstItems.Find(item => item.S_No.ToString().Equals(request.ItemName)).Item_Name : "",
                                request.Category != null && request.Category != "0" && request.Category != "" ? BudgetingController.lstPrdCateg.Find(cat => cat.ID.ToString().Equals(request.Category)).Category.Trim() : "",
                            request.CostElement != null && request.CostElement != "0" ? BudgetingController.lstCostElement.Find(cost => cost.ID.ToString().Equals(request.CostElement)).CostElement : "",
                            request.BudgetCode,
                            request.UnitPrice,
                                request.ReqQuantity,
                                request.TotalPrice,
                                request.ApprQuantity,
                                request.ApprCost,
                                request.Comments,
                                request.RequestorNT,
                                request.SubmitDate.HasValue ? request.SubmitDate.Value.ToString("dd-MM-yyyy") : "",
                                request.DHNT, request.DHAppDate.HasValue ? request.DHAppDate.Value.ToString("dd-MM-yyyy") : "",
                                request.SHNT, request.SHAppDate.HasValue ? request.SHAppDate.Value.ToString("dd-MM-yyyy") : "",
                                request.RequiredDate.HasValue ? request.RequiredDate.Value.ToString("dd-MM-yyyy") : "",
                                request.RequestOrderDate.HasValue ? request.RequestOrderDate.Value.ToString("dd-MM-yyyy") : "",
                                request.OrderDate.HasValue ? request.OrderDate.Value.ToString("dd-MM-yyyy") : "",
                                request.TentativeDeliveryDate.HasValue ? request.TentativeDeliveryDate.Value.ToString("dd-MM-yyyy") : "",
                                request.ActualDeliveryDate.HasValue ? request.ActualDeliveryDate.Value.ToString("dd-MM-yyyy") : "",
                                 request.Fund != null ? BudgetingController.lstFund.Find(fun => fun.ID.ToString().Equals(request.Fund)).Fund : BudgetingController.lstFund.Find(fun => fun.Fund.Equals("F02")).Fund,
                                 request.OrderID,
                                request.OrderedQuantity != null ? (int)request.OrderedQuantity : (int)0,
                                request.OrderPrice,
                                (request.OrderStatus != null && request.OrderStatus.Trim() != "" && request.OrderStatus.Trim() != "0") ? BudgetingController.lstOrderStatus.Find(status => status.ID.ToString().Equals(request.OrderStatus.Trim())).OrderStatus : "",
                                request.PRnumber,
                                (request.OrderDescriptionID.ToString().Trim() != "" && request.OrderDescriptionID.ToString().Trim() != "0") ? BudgetingController.lstOrderDescription.Find(x => x.ID.Equals(int.Parse(request.OrderDescriptionID.ToString()))).Description.ToString() : ""
                                );

                        }

                    }
                    else
                    {
                        var DEPT = db.OrderingRequestor_Table.ToList().Find(person => person.NTID.ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).Department.Trim();
                        var requests = requests1.FindAll(dpt => dpt.DEPT.ToString().Trim().Equals(DEPT)).Select(x => new
                        {
                            x.BU,
                            x.OEM,
                            x.DEPT,
                            x.Group,
                            x.ItemName,
                            x.Category,
                            x.CostElement,
                            x.BudgetCode,
                            x.UnitPrice, /*x.Currency,*/
                            x.ReqQuantity,
                            x.TotalPrice,
                            x.ApprQuantity,
                            x.ApprCost,
                            x.Comments,
                            x.RequestorNT,
                            x.SubmitDate,
                            x.DHNT,
                            x.DHAppDate,
                            x.SHNT,
                            x.SHAppDate,
                            x.RequiredDate,
                            x.RequestOrderDate,
                            x.OrderDate,
                            x.TentativeDeliveryDate,
                            x.ActualDeliveryDate,
                            x.OrderID,
                            x.OrderedQuantity,
                            x.OrderPrice,
                            x.OrderStatus,
                            x.Fund,
                            x.BudgetCodeDescription,
                            x.Order_Type,
                            x.CostCenter,
                            //x.BudgetCenterID,
                            x.LabName,
                            x.RFOReqNTID,
                            x.RFOApprover,
                            x.GoodsRecID,
                            x.UnitofMeasure,
                            x.UnloadingPoint,
                            x.QuoteAvailable,
                            x.Material_Part_Number,
                            x.Supplier_Name_with_Address,
                            x.Purchase_Type,
                            x.Project_ID,
                            x.BM_Number,
                            x.Task_ID,
                            x.PIF_ID,
                            x.Resource_Group_Id,
                            x.PRnumber,
                            x.OrderDescriptionID

                        }).ToList();

                        foreach (var request in requests)
                        {
                            try
                            {
                                dt.Rows.Add(
                                    BudgetingController.lstBUs.Find(bu => bu.ID.ToString().Equals(request.BU)).BU,
                                    BudgetingController.lstOEMs.Find(oem => oem.ID.ToString().Equals(request.OEM)).OEM,
                                    BudgetingController.lstDEPTs.Find(dept => dept.ID.ToString().Equals(request.DEPT)).DEPT,
                                    BudgetingController.lstGroups_test.Find(grp => grp.ID.ToString().Equals(request.Group)).Group,
                                     request.ItemName != null && request.ItemName != "0" && request.ItemName != "" ? BudgetingController.lstItems.Find(item => item.S_No.ToString().Equals(request.ItemName)).Item_Name : "",
                                    request.Category != null && request.Category != "0" ? BudgetingController.lstPrdCateg.Find(cat => cat.ID.ToString().Equals(request.Category)).Category.Trim() : "",
                                    request.CostElement != null && request.CostElement != "0" ? BudgetingController.lstCostElement.Find(cost => cost.ID.ToString().Equals(request.CostElement)).CostElement : "",
                                    request.BudgetCode,
                                    request.UnitPrice,
                                    request.ReqQuantity,
                                    request.TotalPrice,
                                    request.ApprQuantity,
                                    request.ApprCost,
                                    request.Comments,
                                    request.RequestorNT,
                                    request.SubmitDate.HasValue ? request.SubmitDate.Value.ToString("dd-MM-yyyy") : "",
                                    request.DHNT, request.DHAppDate.HasValue ? request.DHAppDate.Value.ToString("dd-MM-yyyy") : "",
                                    request.SHNT, request.SHAppDate.HasValue ? request.SHAppDate.Value.ToString("dd-MM-yyyy") : "",
                                    request.RequiredDate.HasValue ? request.RequiredDate.Value.ToString("dd-MM-yyyy") : "",
                                    request.RequestOrderDate.HasValue ? request.RequestOrderDate.Value.ToString("dd-MM-yyyy") : "",
                                    request.OrderDate.HasValue ? request.OrderDate.Value.ToString("dd-MM-yyyy") : "",
                                    request.TentativeDeliveryDate.HasValue ? request.TentativeDeliveryDate.Value.ToString("dd-MM-yyyy") : "",
                                    request.ActualDeliveryDate.HasValue ? request.ActualDeliveryDate.Value.ToString("dd-MM-yyyy") : "",
                                     request.Fund != null ? BudgetingController.lstFund.Find(fun => fun.ID.ToString().Equals(request.Fund)).Fund : BudgetingController.lstFund.Find(fun => fun.Fund.Equals("F02")).Fund,
                                     request.OrderID,
                                    request.OrderedQuantity != null ? (int)request.OrderedQuantity : (int)0,
                                    request.OrderPrice,
                                    (request.OrderStatus != null && request.OrderStatus.Trim() != "" && request.OrderStatus.Trim() != "0") ? BudgetingController.lstOrderStatus.Find(status => status.ID.ToString().Equals(request.OrderStatus.Trim())).OrderStatus : "",
                                    request.BudgetCodeDescription,
                                     request.Order_Type != null && request.Order_Type.Trim() != "" ? BudgetingController.lstOrderType.Find(fun => fun.ID.ToString().Equals(request.Order_Type)).Order_Type : BudgetingController.lstOrderType.Find(fun => fun.Order_Type.Equals("NA")).Order_Type,
                                     request.CostCenter,
                                     // request.BudgetCenterID != null ? BUDGETCE.Find(fun => fun.ID.ToString().Equals(request.Order_Type)).Order_Type : BudgetingController.lstOrderType.Find(fun => fun.Order_Type.Equals("NA")).Order_Type,
                                     request.LabName,
                                     request.RFOReqNTID,
                                     request.RFOApprover,
                                     request.GoodsRecID,
                                      request.UnitofMeasure != null && request.UnitofMeasure.Trim() != "" ? BudgetingController.lstUOM.Find(fun => fun.ID.ToString().Equals(request.UnitofMeasure)).UOM : BudgetingController.lstUOM.Find(fun => fun.UOM.Equals("NA")).UOM,
                                      request.UnloadingPoint != null && request.UnloadingPoint.Trim() != "" ? BudgetingController.lstUnloadingPoint.Find(fun => fun.ID.ToString().Equals(request.UnloadingPoint)).UnloadingPoint : "",

                                     request.QuoteAvailable,
                                     request.Material_Part_Number,
                                     request.Supplier_Name_with_Address,
                                     request.Purchase_Type != null && request.Purchase_Type.Trim() != "" ? BudgetingController.lstPurchaseType.Find(fun => fun.ID.ToString().Equals(request.Purchase_Type)).PurchaseType : "",

                                     request.Project_ID,
                                     request.BM_Number,
                                     request.Task_ID,
                                     request.PIF_ID,
                                     request.Resource_Group_Id,
                                     request.PRnumber, (request.OrderDescriptionID.ToString().Trim() != "" && request.OrderDescriptionID.ToString().Trim() != "0") ? BudgetingController.lstOrderDescription.Find(x => x.ID.Equals(int.Parse(request.OrderDescriptionID.ToString()))).Description.ToString() : ""


                                    );
                            }
                            catch (Exception ex)
                            {

                            }
                        }

                    }

                    // return Json(new { success = true, data = detail, isDashboard_flag }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var spoton_dept = db.SPOTONData_Table_2022.ToList().Find(person => person.NTID.ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).Department.Trim();
                    string dept_id = BudgetingController.lstDEPTs.Find(dpt => dpt.DEPT == spoton_dept).ID.ToString();
                    var requests = requests1.FindAll(dpt => dpt.DEPT.ToString().Trim().Equals(dept_id)).Select(x => new
                    {
                        x.BU,
                        x.OEM,
                        x.DEPT,
                        x.Group,
                        x.ItemName,
                        x.Category,
                        x.CostElement,
                        x.BudgetCode,
                        x.UnitPrice, /*x.Currency,*/
                        x.ReqQuantity,
                        x.TotalPrice,
                        x.ApprQuantity,
                        x.ApprCost,
                        x.Comments,
                        x.RequestorNT,
                        x.SubmitDate,
                        x.DHNT,
                        x.DHAppDate,
                        x.SHNT,
                        x.SHAppDate,
                        x.RequiredDate,
                        x.RequestOrderDate,
                        x.OrderDate,
                        x.TentativeDeliveryDate,
                        x.ActualDeliveryDate,
                        x.OrderID,
                        x.OrderedQuantity,
                        x.OrderPrice,
                        x.OrderStatus,
                        x.Fund,
                        x.PRnumber,
                        x.OrderDescriptionID
                    }).ToList();

                    foreach (var request in requests)
                    {

                        dt.Rows.Add(
                            BudgetingController.lstBUs.Find(bu => bu.ID.ToString().Equals(request.BU)).BU,
                            BudgetingController.lstOEMs.Find(oem => oem.ID.ToString().Equals(request.OEM)).OEM,
                            BudgetingController.lstDEPTs.Find(dept => dept.ID.ToString().Equals(request.DEPT)).DEPT,
                            BudgetingController.lstGroups_test.Find(grp => grp.ID.ToString().Equals(request.Group)).Group,
                            BudgetingController.lstItems.Find(item => item.S_No.ToString().Equals(request.ItemName)).Item_Name,
                            request.Category != null && request.Category != "0" ? BudgetingController.lstPrdCateg.Find(cat => cat.ID.ToString().Equals(request.Category)).Category.Trim() : "",
                                request.CostElement != null && request.CostElement != "0" ? BudgetingController.lstCostElement.Find(cost => cost.ID.ToString().Equals(request.CostElement)).CostElement : "",
                            request.BudgetCode,
                            request.UnitPrice,
                            request.ReqQuantity,
                            request.TotalPrice,
                            request.ApprQuantity,
                            request.ApprCost,
                            request.Comments,
                            request.RequestorNT,
                            request.SubmitDate.HasValue ? request.SubmitDate.Value.ToString("dd-MM-yyyy") : "",
                            request.DHNT, request.DHAppDate.HasValue ? request.DHAppDate.Value.ToString("dd-MM-yyyy") : "",
                            request.SHNT, request.SHAppDate.HasValue ? request.SHAppDate.Value.ToString("dd-MM-yyyy") : "",
                            request.RequiredDate.HasValue ? request.RequiredDate.Value.ToString("dd-MM-yyyy") : "",
                            request.RequestOrderDate.HasValue ? request.RequestOrderDate.Value.ToString("dd-MM-yyyy") : "",
                            request.OrderDate.HasValue ? request.OrderDate.Value.ToString("dd-MM-yyyy") : "",
                            request.TentativeDeliveryDate.HasValue ? request.TentativeDeliveryDate.Value.ToString("dd-MM-yyyy") : "",
                            request.ActualDeliveryDate.HasValue ? request.ActualDeliveryDate.Value.ToString("dd-MM-yyyy") : "",
                             request.Fund != null ? BudgetingController.lstFund.Find(fun => fun.ID.ToString().Equals(request.Fund)).Fund : BudgetingController.lstFund.Find(fun => fun.Fund.Equals("F02")).Fund,
                             request.OrderID,
                            request.OrderedQuantity != null ? (int)request.OrderedQuantity : (int)0,
                            request.OrderPrice,
                            (request.OrderStatus != null && request.OrderStatus.Trim() != "" && request.OrderStatus.Trim() != "0") ? BudgetingController.lstOrderStatus.Find(status => status.ID.ToString().Equals(request.OrderStatus.Trim())).OrderStatus : "",
                            request.PRnumber,
                            (request.OrderDescriptionID.ToString().Trim() != "" && request.OrderDescriptionID.ToString().Trim() != "0") ? BudgetingController.lstOrderDescription.Find(x => x.ID.Equals(int.Parse(request.OrderDescriptionID.ToString()))).Description.ToString() : ""
                            );
                    }
                }



                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dt);
                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);
                        var robj = File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
                        return Json(robj, JsonRequestBehavior.AllowGet);
                    }
                }

            }

        }


        ///// <summary>
        ///// function to get the Initial values to be filled automatically when a new request is created
        ///// </summary>
        ///// <returns></returns>
        //[HttpGet]
        //public ActionResult InitRowValues()
        //{

        //    using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //    //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //    {

        //        RequestItemsRepoEdit1 temp = new RequestItemsRepoEdit1();

        //        System.Security.Principal.IPrincipal User = System.Web.HttpContext.Current.User;
        //        SPOTONData_Table_2021 PresentUser = lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper()));
        //        string presentUserName = lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).EmployeeName;
        //        string L2ReviewerName = string.Empty;
        //        string L3ReviewerName = string.Empty;
        //        try
        //        {
        //            L2ReviewerName = lstUsers.FindAll(user => PresentUser.Department.ToUpper().Equals(user.Group.ToUpper())).FirstOrDefault(head => head.Role.ToUpper().Contains("HEAD")).EmployeeName;
        //        }
        //        catch (Exception ex)
        //        {
        //            L2ReviewerName = "NA";
        //        }
        //        try
        //        {
        //            L3ReviewerName = lstUsers.FindAll(user => PresentUser.Section.ToUpper().Contains(user.Section.ToUpper())).FirstOrDefault(head => head.Role.ToUpper().Contains("SECTION")).EmployeeName;
        //        }
        //        catch (Exception ex)
        //        {
        //            L3ReviewerName = "NA";
        //        }

        //        temp.Requestor = presentUserName;
        //        temp.Reviewer_1 = L2ReviewerName;
        //        temp.Reviewer_2 = L3ReviewerName;
        //        DEPT_Table dEPT_Table = lstDEPTs.Find(departmnt => departmnt.DEPT.Equals(lstUsers.Find(x => x.EmployeeName == presentUserName).Department));
        //        temp.DEPT = dEPT_Table.ID;


        //        var PresentUserGroup = lstUsers.Find(x => x.EmployeeName == presentUserName).Group;
        //        Groups_Table gROUP_Table = lstGroups.Find(grp => grp.Group.Trim().Equals(PresentUserGroup));
        //        temp.Group = gROUP_Table.ID;

        //        temp.RequestDate = DateTime.Now.ToString("yyyy-MM-dd");
        //        return Json(new { data = temp }, JsonRequestBehavior.AllowGet);
        //    }

        //}
        [HttpGet]
        public ActionResult InitRowValues()
        {
            var UserNTID = System.Web.HttpContext.Current.User.Identity.Name.Split('\\')[1].ToUpper();
            string Query = "";
            bool popupadd = false;
            bool popupedit = false,
               gridedit = false;
            try
            {
                connection();
                RequestItemsRepoEdit1 temp = new RequestItemsRepoEdit1();

                temp.RequestDate = DateTime.Now.ToString("yyyy-MM-dd");
                Query = " Exec [dbo].[LEPlanner_InitRowValues] '" + UserNTID + "' ";
                //"IF EXISTS(SELECT RequestorNTID from RequestItems_Table where RequestorNTID = @User)SELECT TOP 1 BU , OEM from RequestItems_Table where RequestorNTID = @User order by UpdatedAt desc";
                OpenConnection();
                SqlCommand cmd = new SqlCommand(Query, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    temp.Requestor = dr["UserName"].ToString();
                    temp.RequestorNTID = UserNTID;
                    temp.DEPT = int.Parse(dr["UserDept_ID"].ToString());
                    temp.Group = int.Parse(dr["UserGroup_ID"].ToString());
                    temp.Reviewer_1 = dr["UserHOE_Name"].ToString();
                    temp.BU = int.Parse(dr["BU"].ToString());
                    temp.OEM = int.Parse(dr["OEM"].ToString());
                    temp.Reviewer_2 = dr["UserVKMSPOC_Name"].ToString();
                    //temp.BudgetCenterID =
                    temp.BudgetcenterList = fn_GetRFOBudgetCenter(temp.DEPT);
                    //new List<OrderTypeBGSW>();
                }
                else
                {
                    temp.Requestor = "";
                    temp.RequestorNTID = UserNTID;
                    temp.DEPT = 0;
                    temp.Group = 0;
                    temp.Reviewer_1 = "NA";
                    temp.BU = 0;
                    temp.OEM = 0;
                    temp.Reviewer_2 = "";
                    temp.BudgetcenterList = new List<OrderTypeBGSW>();

                }
                //Query = "select [AddRFO],[Edit Popup RFO],[Edit Row RFO] from Mail_RFOAuthorization_Table where Section in (Select distinct Section from SPOTONData_Table_2022 where NTID = '"+ UserNTID + "') OR TopSection in (Select distinct Section from SPOTONData_Table_2022 where NTID = '" + UserNTID + "')";
                //OpenConnection();
                //cmd = new SqlCommand(Query, conn);
                //dr = cmd.ExecuteReader();
                //if (dr.HasRows)
                //{
                //    dr.Read();
                //    popupadd = (Convert.ToBoolean(Convert.ToInt32(dr["AddRFO"].ToString()))); 
                //    popupedit = (Convert.ToBoolean(Convert.ToInt32(dr["Edit Popup RFO"].ToString())));
                //    gridedit = (Convert.ToBoolean(Convert.ToInt32(dr["Edit Row RFO"].ToString())));
                //}
                //dr.Close();



                CloseConnection();


                if (temp.Reviewer_1.Trim() != "NA")
                {

                    return Json(new { data = temp, success = true}, JsonRequestBehavior.AllowGet);
                }
                else
                {

                    return Json(new { data = UserNTID, success = false  }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                return Json(new { success = false, data = UserNTID }, JsonRequestBehavior.AllowGet);
            }
            finally
            {
                CloseConnection();
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

                        //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

                        {
                            string presentUserName = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).EmployeeName;
                            //decimal conversionINRate = db.Currency_Conversion_Table.FirstOrDefault(x => x.Currency.Contains("INR")).ConversionRate_to_USD;
                            //decimal conversionEURate = db.Currency_Conversion_Table.FirstOrDefault(x => x.Currency.Contains("EUR")).ConversionRate_to_USD;

                            DataTable dt1 = dt;
                            DateTime defaultDate = new DateTime(1900, 01, 01);
                            //dt1.Rows[0].Delete();
                            //dt1.AcceptChanges();

                            //WriteLog("header 1 deleted");
                            DataTable dt_new = new DataTable();
                            DataTable dt_grpeddetails = new DataTable();
                            //WriteLog("new datatable created; adding columns .....");



                            //ORDER SHOULD BE SAME AS IN USER DEFINED TABLE IN SQL
                            dt_new.Columns.Add("NTID", typeof(string));
                            dt_new.Columns.Add("VKM_Year", typeof(string));
                            dt_new.Columns.Add("BU", typeof(string));
                            dt_new.Columns.Add("OEM", typeof(string));
                            dt_new.Columns.Add("GROUP", typeof(string));
                            dt_new.Columns.Add("Dept", typeof(string));
                            dt_new.Columns.Add("ItemDescription", typeof(string));
                            dt_new.Columns.Add("Project", typeof(string));
                            dt_new.Columns.Add("Fund", typeof(string));
                            dt_new.Columns.Add("BudgetCenter", typeof(string));
                            dt_new.Columns.Add("ReqQuantity", typeof(int));
                            dt_new.Columns.Add("RequestDt", typeof(string));
                            dt_new.Columns.Add("EMRemarks", typeof(string));
                            dt_new.Columns.Add("HOEApprovedDt", typeof(string));
                            dt_new.Columns.Add("HOERemarks", typeof(string));
                            dt_new.Columns.Add("VKMApprovedDt", typeof(string));
                            dt_new.Columns.Add("VKMSPOCRemarks", typeof(string));
                            dt_new.Columns.Add("LabName", typeof(string));
                            dt_new.Columns.Add("GoodsRecipientID", typeof(string));
                            dt_new.Columns.Add("UnloadingPoint", typeof(string));





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
                                row1[1] = dt1.Rows[i][19].ToString();
                                row1[2] = dt1.Rows[i][1].ToString();
                                row1[3] = dt1.Rows[i][2].ToString();
                                row1[4] = dt1.Rows[i][3].ToString();
                                row1[5] = dt1.Rows[i][4].ToString();
                                row1[6] = dt1.Rows[i][5].ToString();
                                row1[7] = dt1.Rows[i][6].ToString();
                                row1[8] = dt1.Rows[i][7].ToString();
                                row1[9] = dt1.Rows[i][8].ToString();
                                row1[10] = (dt1.Rows[i][9] == DBNull.Value) ? 0 : Convert.ToDouble(dt1.Rows[i][9]);
                                row1[11] = (dt1.Rows[i][10].ToString().Trim() == "") ? string.Empty : dt1.Rows[i][10].ToString().Replace(".", "/");

                                
                                row1[12] = dt1.Rows[i][11].ToString();
                                //var date_replace = dt1.Rows[i][14].ToString().Replace(".", "/");
                                row1[13] = (dt1.Rows[i][12].ToString().Trim() == "") ? string.Empty : dt1.Rows[i][12].ToString().Replace(".", "/");

                                //WriteLog("date replace:" + date_replace);
                                //  row1[12] = (dt1.Rows[i][37].ToString().Trim() == "") ? defaultDate : Convert.ToDateTime(dt1.Rows[i][37].ToString().Replace(".","/"));
                                row1[14] = dt1.Rows[i][13].ToString();
                                row1[15] = (dt1.Rows[i][14].ToString().Trim() == "") ? string.Empty : dt1.Rows[i][14].ToString().Replace(".", "/");
                                row1[16] = dt1.Rows[i][15].ToString();
                                row1[17] = dt1.Rows[i][16].ToString();
                                row1[18] = dt1.Rows[i][17].ToString();
                                row1[19] = dt1.Rows[i][18].ToString();


                                dt_new.Rows.Add(row1);
                            }

                            connection();

                            SqlCommand command = new SqlCommand();

                            command.Connection = conn;
                            command.CommandText = "dbo.[RFO_AddOrEdit_Upload]";
                            command.CommandType = CommandType.StoredProcedure;

                            // Add the input parameter and set its properties.

                            SqlParameter parameter2 = new SqlParameter();
                            parameter2.ParameterName = "@UserName";
                            parameter2.SqlDbType = SqlDbType.NVarChar;
                            parameter2.Direction = ParameterDirection.Input;
                            parameter2.Value = presentUserName;


                            SqlParameter parameter1 = new SqlParameter();
                            parameter1.ParameterName = "@RFO_ReqList";
                            parameter1.SqlDbType = SqlDbType.Structured;
                            parameter1.TypeName = "dbo.RFO_ReqList";
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

                            command = new SqlCommand("select  convert(nvarchar(100),Msg) as ErrorMsg from RFOLog where Msg like '%Error Msg%' order by logtime desc", conn);


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

                                    ErrorMsg += Convert.ToString(item["ErrorMsg"]);
                                    //ErrorMsg1.Add(ErrorMsg);

                                }
                                catch (Exception ex)
                                {

                                }

                            }
                            //if (ErrorMsg1.Count() > 0)
                            //{
                            //    //WriteLog("ErrorMsg: " + ErrorMsg1[0]);
                            //    ErrorMsg = ErrorMsg1[0];
                            //}


                            CloseConnection();


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

                WriteLog("Error - Index Import : NTID - " + User.Identity.Name.Split('\\')[1].Trim().ToUpper() + ", ErrorMsg : " + ex + ", At - " + DateTime.Now);

                return Json(new { success = false, errormsg = ex.Message.ToString() });
            }
            finally
            {

            }
            //return Json(new { success = true, save, errormsg = ErrorMsg }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DownloadTemplate()
        {
            string folderPath = Server.MapPath("~/Templates/");
            string fileName = "RFO_Requests.xlsx";
            byte[] fileBytes = System.IO.File.ReadAllBytes(folderPath + fileName);

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }


        public static void WriteLog(string Message)
        {
            string execPath = AppDomain.CurrentDomain.BaseDirectory;
            execPath = execPath + "Budgeting_Log\\log" + DateTime.Now.ToString("dd_MM_yyyy") + ".txt";
            StreamWriter file = new StreamWriter(execPath, append: true);
            file.WriteLine(Message + "\r\n");
            file.Close();
        }

        [HttpGet]
        public ActionResult GetlinkedRequests(int RequestID, double StartDate, double EndDate)
        {
            try
            {
                System.DateTime SDate = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                SDate = SDate.AddMilliseconds(StartDate).ToLocalTime();

                System.DateTime EDate = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                EDate = EDate.AddMilliseconds(EndDate).ToLocalTime();

                DateTime date1 = DateTime.Parse(SDate.ToShortDateString());
                DateTime date2 = DateTime.Parse(EDate.ToShortDateString());

                DataTable dt = new DataTable();
                connection();
                OpenConnection();
                string Query = "select distinct LinkedRequestID, isSelected, DisplayNo  from (Select RequestID as LinkedRequestID, 1 as isSelected, 0 as DisplayNo from RequestItems_Table Where LinkedRequestID = " + RequestID + " and RequestSource = 'RFO' union select RequestID as LinkedRequestID , 0 as isSelected, 1 as DisplayNo from RequestItems_Table where RequestSource = 'RFO' and RequestID not in (" + RequestID + ") and isnull(LinkedRequestID,'') = '' and convert(date, RequestDate) >= convert(date, '" + date1 + "') and convert(date, RequestDate) <= convert(date, '" + date2 + "') ) A Order by DisplayNo,LinkedRequestID";
                //string Query = "select distinct LinkedRequestID, isSelected, DisplayNo  from (Select RequestID as LinkedRequestID, 1 as isSelected, 0 as DisplayNo from RequestItems_Table Where LinkedRequestID = " + RequestID + " and RequestSource = 'RFO' union select RequestID as LinkedRequestID , 0 as isSelected, 1 as DisplayNo from RequestItems_Table where RequestSource = 'RFO' and RequestID not in (Select isnull(LinkedRequestID,'') from RequestItems_Table where RequestSource = 'RFO') and isnull(LinkedRequestID,'') = '' and convert(date, RequestDate) >= convert(date, '" + date1 + "') and convert(date, RequestDate) <= convert(date, '" + date2 + "') ) A Order by DisplayNo,LinkedRequestID";
                SqlCommand cmd = new SqlCommand(Query,conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                CloseConnection();

                List<ReqList> reqlist = new List<ReqList>();
                //ReqList reqlist = new ReqList();
                if (dt.Rows.Count > 0)
                {
                    for(int i=0; i< dt.Rows.Count; i++)
                    {
                        ReqList item = new ReqList();
                        item.LinkedRequestID = dt.Rows[i]["LinkedRequestID"].ToString();
                        if (dt.Rows[i]["isSelected"].ToString() == "1")
                            item.isSelected = true;
                        else
                            item.isSelected = false;

                        reqlist.Add(item);
                    }
                }

                return Json(new { data = reqlist, success = true }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                CloseConnection();
                return Json(new { data = "", success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// function to get L3 reviewer name based on BU
        /// </summary>
        /// <param name="BU"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetReviewer(int DEPT)
        {

            //string SHNT = string.Empty;
            //var x = lstBU_SPOCs;
            //SHNT = lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(lstBU_SPOCs.Find(spoc => spoc.BU.Equals(BU)).VKMspoc.ToUpper().Trim())).EmployeeName;

            //return Json(SHNT, JsonRequestBehavior.AllowGet);

            connection();

            string UserNTID = System.Web.HttpContext.Current.User.Identity.Name.Split('\\')[1].ToUpper();

            try

            {

                string SHNT = string.Empty;

                //SHNT = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(BudgetingController.lstBU_SPOCs.Find(spoc => spoc.BU.Equals(BU)).VKMspoc.ToUpper().Trim())).EmployeeName;

                string Query = " Exec [dbo].[LEPlanner_GetHOEorVKMSPOC] '"
                    + UserNTID + "','VKMSPOC'," + DEPT + ", '' ,'',0";

                OpenConnection();

                SqlCommand cmd = new SqlCommand(Query, conn);

                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)

                {

                    dr.Read();

                    SHNT = dr["Reviewer"].ToString();


                }

                else

                {

                    SHNT = "NA";



                }

                if (SHNT.Trim() != "NA")

                {



                    return Json(new { data = SHNT, success = true }, JsonRequestBehavior.AllowGet);

                }

                else

                {

                    return Json(new { data = UserNTID, success = false }, JsonRequestBehavior.AllowGet);

                }


            }

            catch (Exception ex)

            {

                //WriteLog("Error - GetReviewer_VKMSPOC : NTID - " + User.Identity.Name.Split('\\')[1].Trim().ToUpper() + ", ErrorMsg : " + ex + ", At - " + DateTime.Now);

                return Json(new { success = false }, JsonRequestBehavior.AllowGet);

            }

            finally

            {

                CloseConnection();

            }
        }
        /// <summary>
        /// function to get the unit price of selected Item
        /// </summary>
        /// <param name="ItemName"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetUnitPrice(int ItemName)
        {
            return Json(BudgetingController.lstItems.Find(x => x.S_No == ItemName).UnitPriceUSD, JsonRequestBehavior.AllowGet);

        }
        /// <summary>
        /// function to get category of selected Item
        /// </summary>
        /// <param name="ItemName"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetCategory(int ItemName)
        {

            int Catitem = int.Parse(BudgetingController.lstItems.Find(x => x.S_No == ItemName).Category);

            return Json(Catitem, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// function to get the cost elements of selected Item
        /// </summary>
        /// <param name="ItemName"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetCostElement(int ItemName)
        {

            int Costeltitem = int.Parse(BudgetingController.lstItems.Find(x => x.S_No == ItemName).Cost_Element);

            return Json(Costeltitem, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// function to get category of selected Item
        /// </summary>
        /// <param name="ItemName"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetBudgetCode(int ItemName)
        {
            try
            {
                string BudgetCode = "";
                string BudgetCodedesc = "";
                connection();
                string Query = " Select Isnull(BudgetCode,'') as BudgetCode from ItemsCostList_Table where S#No = " + ItemName + " ";
                OpenConnection();
                SqlCommand cmd = new SqlCommand(Query, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    BudgetCode = dr["BudgetCode"].ToString();

                }
                dr.Close();

                string Query1 = " Select ID,Budget_Code_Description  from [BGSW_BudgetCode_Table] where Budget_Code = " + BudgetCode + " ";
                SqlCommand cmd1 = new SqlCommand(Query1, conn);
                SqlDataReader dr1 = cmd1.ExecuteReader();
                if (dr1.HasRows)
                {
                    dr1.Read();
                    BudgetCodedesc = dr1["Budget_Code_Description"].ToString();

                }
                dr1.Close();
                CloseConnection();


                return Json(new { data = BudgetCode, BudgetCodedesc, success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //WriteLog("Error - GetBudgetCode : NTID - " + User.Identity.Name.Split('\\')[1].Trim().ToUpper() + ", ErrorMsg : " + ex + ", At - " + DateTime.Now);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult GetCostCenter(int deptid, int upid)
        {
            try
            {
                string CostCenter = "";

                connection();
                //string Query = " select max(CostCenter) as CostCenter from SPOTONData_Table_2022 where Department in(select DEPT from DEPT_Table where ID = " + deptid + ") and PersonnelArea in (select Location from BGSW_UnloadingPoint_Table where ID = " + upid + ")";
                string Query = "   select top (1)CostCenter from SPOTONData_Table_2022 where Department in(select DEPT from DEPT_Table where ID = " + deptid + ") and PersonnelArea in (select Location from BGSW_UnloadingPoint_Table where ID = " + upid + ") group by CostCenter order by count(CostCenter) desc";
                OpenConnection();
                SqlCommand cmd = new SqlCommand(Query, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    CostCenter = dr["CostCenter"].ToString();

                }
                dr.Close();

                CloseConnection();


                return Json(new { data = CostCenter, success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //WriteLog("Error - GetBudgetCode : NTID - " + User.Identity.Name.Split('\\')[1].Trim().ToUpper() + ", ErrorMsg : " + ex + ", At - " + DateTime.Now);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public ActionResult GetItemName(int buselected, int ordert)
        {
            try
            {
                List<ItemsCostList_Table> itemlist_custom = new List<ItemsCostList_Table>();

                DataTable dt = new DataTable();
                connection();
                string Query = " Select S#No,BU,Order_Type,[Item Name],BudgetCode,UOM,[Cost Element],UnitPriceUSD from ItemsCostList_Table where BU = @BU and Order_Type = @OrderType order by S#No ";
                OpenConnection();
                SqlCommand cmd = new SqlCommand(Query, conn);
                cmd.Parameters.AddWithValue("@BU", buselected);
                cmd.Parameters.AddWithValue("@OrderType", ordert);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                CloseConnection();
                var presentUserNTID = User.Identity.Name.Split('\\')[1].ToUpper();
                foreach (DataRow row in dt.Rows)
                {
                    ItemsCostList_Table item = new ItemsCostList_Table();
                    //dr.Read();
                    //item.ID = int.Parse(row["ID"].ToString());
                    item.Item_Name = row["Item Name"].ToString();
                    item.S_No = int.Parse(row["S#No"].ToString());
                    item.Order_Type = int.Parse(row["Order_Type"].ToString());
                    item.BU = int.Parse(row["BU"].ToString());
                    item.BudgetCode = int.Parse(row["BudgetCode"].ToString());
                    item.UOM = (row["UOM"].ToString());
                    item.Cost_Element = (row["Cost Element"].ToString());
                    item.UnitPriceUSD = double.Parse(row["UnitPriceUSD"].ToString());
                    itemlist_custom.Add(item);

                }
                //dr.Close();
                //CloseConnection();
                return Json(new
                {
                    data = itemlist_custom,

                    success = true,
                }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                //WriteLog("Error - GetBudgetCode : NTID - " + User.Identity.Name.Split('\\')[1].Trim().ToUpper() + ", ErrorMsg : " + ex + ", At - " + DateTime.Now);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpPost]
        public ActionResult GetOrderType()
        {
            try
            {
                List<OrderTypeBGSW> OrderTypeList = new List<OrderTypeBGSW>();
                connection();
                string Query = " Select ID,OrderType from BGSW_ItemOrderType_Table order by ID ";
                OpenConnection();
                SqlCommand cmd = new SqlCommand(Query, conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                CloseConnection();
                var presentUserNTID = User.Identity.Name.Split('\\')[1].ToUpper();
                foreach (DataRow row in dt.Rows)
                {
                    OrderTypeBGSW item = new OrderTypeBGSW();
                    //dr.Read();
                    item.ID = int.Parse(row["ID"].ToString());
                    item.OrderType = row["OrderType"].ToString();

                    OrderTypeList.Add(item);

                }
                //dr.Close();
                //CloseConnection();
                return Json(new
                {
                    data = OrderTypeList,
                    presentUserNTID,
                    success = true,
                }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                //WriteLog("Error - GetBudgetCode : NTID - " + User.Identity.Name.Split('\\')[1].Trim().ToUpper() + ", ErrorMsg : " + ex + ", At - " + DateTime.Now);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        public List<OrderTypeBGSW> fn_GetRFOBudgetCenter(int deptid)
        {
            List<OrderTypeBGSW> BudgetcenterList = new List<OrderTypeBGSW>();
            connection();
            OpenConnection();
            string Query = " Exec [dbo].[GetRFOBudgetCenter] '" + deptid + "', '" + User.Identity.Name.Split('\\')[1].ToUpper() + "' ";
            SqlCommand cmd = new SqlCommand(Query, conn);

            DataTable dt = new DataTable();

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            CloseConnection();

            foreach (DataRow row in dt.Rows)
            {
                OrderTypeBGSW item = new OrderTypeBGSW();
                item.ID = Convert.ToInt32(row["ID"].ToString());
                item.BudgetCenter = row["BudgetCenter"].ToString();
                //dr.Read();
                //item.ID = int.Parse(row["ID"].ToString());
                BudgetcenterList.Add(item);

            }
            CloseConnection();
            return BudgetcenterList;
        }

        public ActionResult GetRFOBudgetCenter(int deptid)
        {
            try
            {
                List<OrderTypeBGSW> BudgetcenterList = fn_GetRFOBudgetCenter(deptid);
                return Json(new
                {
                    data = BudgetcenterList,

                    success = true,
                }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                //WriteLog("Error - GetBudgetCode : NTID - " + User.Identity.Name.Split('\\')[1].Trim().ToUpper() + ", ErrorMsg : " + ex + ", At - " + DateTime.Now);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult GetUOM(int ordert)
        {
            try
            {
                List<UOMBGSW> UOMList = new List<UOMBGSW>();
                connection();
                string Query = " Select ID,Units from BGSW_UOM_Table where ItemOrderType = '" + ordert + "' order by ID ";
                OpenConnection();
                SqlCommand cmd = new SqlCommand(Query, conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                CloseConnection();
                //var presentUserNTID = User.Identity.Name.Split('\\')[1].ToUpper();
                foreach (DataRow row in dt.Rows)
                {
                    UOMBGSW item = new UOMBGSW();
                    //dr.Read();
                    item.ID = int.Parse(row["ID"].ToString());
                    item.UOM = row["Units"].ToString();



                    UOMList.Add(item);

                }
                //dr.Close();
                //CloseConnection();
                return Json(new
                {
                    data = UOMList,

                    success = true,
                }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                //WriteLog("Error - GetBudgetCode : NTID - " + User.Identity.Name.Split('\\')[1].Trim().ToUpper() + ", ErrorMsg : " + ex + ", At - " + DateTime.Now);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public ActionResult RFOApprover()
        {
            try

            {
                List<RFOApprover> rfoapproverlist = new List<RFOApprover>();
                connection();
                string Query = "select distinct section from SPOTONData_Table_2022 union select distinct Department from SPOTONData_Table_2022 union select distinct [group] from SPOTONData_Table_2022";
                OpenConnection();
                SqlCommand cmd = new SqlCommand(Query, conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                CloseConnection();
                var presentUserNTID = User.Identity.Name.Split('\\')[1].ToUpper();
                foreach (DataRow row in dt.Rows)
                {
                    RFOApprover item = new RFOApprover();
                    //dr.Read();

                    item.Section_Dept_Grp = row["Section"].ToString();

                    rfoapproverlist.Add(item);

                }
                CloseConnection();

                return Json(rfoapproverlist, JsonRequestBehavior.AllowGet);
                //dr.Close();
                //CloseConnection();

            }
            catch (Exception ex)
            {
                //WriteLog("Error - GetBudgetCode : NTID - " + User.Identity.Name.Split('\\')[1].Trim().ToUpper() + ", ErrorMsg : " + ex + ", At - " + DateTime.Now);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult GetUnloadingPoint()
        {
            try
            {
                List<UnloadingPointBGSW> UnloadingPointList = new List<UnloadingPointBGSW>();
                connection();
                string Query = " Select ID,UnloadingPoint from BGSW_UnloadingPoint_Table order by ID ";
                OpenConnection();
                SqlCommand cmd = new SqlCommand(Query, conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                CloseConnection();
                //var presentUserNTID = User.Identity.Name.Split('\\')[1].ToUpper();
                foreach (DataRow row in dt.Rows)
                {
                    UnloadingPointBGSW item = new UnloadingPointBGSW();
                    //dr.Read();
                    item.ID = int.Parse(row["ID"].ToString());
                    item.UnloadingPoint = row["UnloadingPoint"].ToString();

                    UnloadingPointList.Add(item);

                }
                //dr.Close();
                //CloseConnection();
                return Json(new
                {
                    data = UnloadingPointList,
                    success = true,
                }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                //WriteLog("Error - GetBudgetCode : NTID - " + User.Identity.Name.Split('\\')[1].Trim().ToUpper() + ", ErrorMsg : " + ex + ", At - " + DateTime.Now);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// function to get the Lead Time of selected Item
        /// </summary>
        /// <param name="ItemName"></param>
        /// <returns></returns>
        //[HttpGet]
        //public ActionResult GetLeadTime(int ItemName)
        //{
        //    int leadtime_vendor;
        //    if (lstItems.Find(x => x.S_No == ItemName).VendorCategory != null && lstItems.Find(x => x.S_No == ItemName).VendorCategory != string.Empty)
        //    {
        //        int vendor_item = int.Parse(lstItems.Find(x => x.S_No == ItemName).VendorCategory);

        //        if ((lstVendor.Find(x => x.ID == vendor_item).LeadTime) != null)
        //            leadtime_vendor = ((int)lstVendor.Find(x => x.ID == vendor_item).LeadTime);
        //        else
        //            leadtime_vendor = 0;


        //    }
        //    else
        //        leadtime_vendor = 0;

        //    return Json(leadtime_vendor, JsonRequestBehavior.AllowGet);
        //}




        ////ValidateRequiredDate

        /// <summary>
        /// function to validate the Required Date of selected Item
        /// </summary>
        /// <param name="LeadTime"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult ValidateRequiredDate(/*int LeadTime, */DateTime RequiredDate)
        {
            //var ExpectedReqdDate = DateTime.Now.AddDays(LeadTime - 1).ToShortDateString();
            var RequiredDt = RequiredDate.ToShortDateString();
            //var ExpectedReqdDate1 = DateTime.Parse(ExpectedReqdDate);
            var RequiredDt1 = DateTime.Parse(RequiredDt);
            var PresentDt = DateTime.Now.ToShortDateString();
            var PresentDt1 = DateTime.Parse(PresentDt);
            string error = String.Empty;
            if (RequiredDt1 < PresentDt1)
            {
                error = "The Required by Date cannot be less than Today's Date. Please choose appropriate date!";
            }
            return Json(error, JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public ActionResult GetPODetails()
        {
            DataTable dt = new DataTable();
            try
            {

                connection();

                string Query = " Select  [ID]" +
          ",[RequestID]" +
          ",[VKMYear]" +
          ",[BU] " +
          ",[OEM]" +
          ",[GROUP]" +
          ",[Dept]" +
          ",[Item Name]" +
          ",[Ordered Quantity]" +
          ",[PO Number]" +
          ",[PIF ID]" +
          ",[Fund]" +
          ",[Fund Center]" +
          ",BudgetCode" +
          ",ItemDescription" +
          ",[PO Quantity]" +
          ",[UOM]" +
          ",[UnitPrice]" +
          ",[Netvalue]" +
          ",[Netvalue_USD]" +
          ",[Currency]" +
          ",[Plant]" +
          ",[PO Created On]" +
          ",VendorName" +
          ",[CW]" +
          ",[Tentative Delivery Date]" +
          ",[Actual Delivery Date]" +
          ",[Difference in DeliveRy Date]" +
          ",[Actal Amt]" +
          ",[Negotiated Amt]" +
          ",[Savings]" +
          ",[Current status]" +
          ",[PO Remarks]" +
          "from [PODetails_Table] where VKMYear is not null order by ID";

                OpenConnection();
                SqlCommand cmd = new SqlCommand(Query, conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                CloseConnection();
            }
            catch (Exception ex)
            {

            }
            List<PO_Data> result = new List<PO_Data>();

            foreach (DataRow row in dt.Rows)
            {
                PO_Data item = new PO_Data();
                //item.ProjectChange = Convert.ToDateTime(row["ProjectChange"]).ToString("yyyy-MM-dd");
                //if (row["ProjectChange"].ToString() == "1900-01-01")
                //{
                //    item.ProjectChange = "";
                //}
                //else
                //{
                //    item.ProjectChange = row["ProjectChange"].ToString();
                //}
                item.ID = int.Parse(row["ID"].ToString());
                item.RequestID = int.Parse(row["RequestID"].ToString());
                item.VKMYear = row["VKMYear"].ToString();
                item.BU = row["BU"].ToString();
                item.OEM = row["OEM"].ToString();
                item.GROUP = row["GROUP"].ToString();
                item.Dept = row["Dept"].ToString();
                item.ItemName = row["Item Name"].ToString();
                item.OrderedQuantity = row["Ordered Quantity"].ToString();
                item.PONumber = row["PO Number"].ToString();
                item.PIFID = row["PIF ID"].ToString();
                item.Fund = row["Fund"].ToString();
                item.FundCenter = row["Fund Center"].ToString();
                item.BudgetCode = row["BudgetCode"].ToString();
                item.ItemDescription = row["ItemDescription"].ToString();
                item.POQuantity = int.Parse(row["PO Quantity"].ToString());
                item.UOM = row["UOM"].ToString();
                item.UnitPrice = row["UnitPrice"].ToString();
                item.Netvalue = row["Netvalue"].ToString();
                item.Netvalue_USD = row["Netvalue_USD"].ToString(); ;
                item.Currency = row["Currency"].ToString(); ;
                item.Plant = row["Plant"].ToString();
                item.POCreatedOn = row["PO Created On"].ToString();
                item.VendorName = row["VendorName"].ToString();
                item.CW = row["CW"].ToString();
                item.TentativeDeliveryDate = row["Tentative Delivery Date"].ToString();
                item.ActualDeliveryDate = row["Actual Delivery Date"].ToString();
                item.DifferenceinDeliveRyDate = row["Difference in DeliveRy Date"].ToString();
                item.ActalAmt = row["Actal Amt"].ToString();
                item.NegotiatedAmt = row["Negotiated Amt"].ToString();
                item.Savings = row["Savings"].ToString();
                item.Currentstatus = row["Current status"].ToString();
                item.PORemarks = row["PO Remarks"].ToString();

                result.Add(item);
            }


            return Json(new { data = result, success = true, JsonRequestBehavior.AllowGet });
            //}

        }

        [HttpGet]
        public ActionResult Lookup(string year)
        {
            LookupData lookupData = new LookupData();

            lookupData.BU_List = BudgetingController.lstBUs.OrderBy(x => x.ID).ToList();
            //if (year.Contains("2020"))
            //{
            //    lookupData.BU_List[2].BU = "AS";
            //    lookupData.BU_List[4].BU = "PS";
            //}
            //else
            //{
            //    lookupData.BU_List[2].BU = "MB";
            //    lookupData.BU_List[4].BU = "OSS";
            //}
            //lookupData.BU_List = lstBUs;
            lookupData.OEM_List = BudgetingController.lstOEMs;
            lookupData.DEPT_List = BudgetingController.lstDEPTs;//.FindAll(x => x.Outdated == false);
            lookupData.Groups_test = BudgetingController.lstGroups_test;//.FindAll(x => x.Outdated == false);
            //lookupData.Groups_List = lstGroups/*.FindAll(x => x.Outdated == false)*/;
            //lookupData.Item_List = BudgetingController.lstItems;//.FindAll(item => item.Deleted != true);
            lookupData.Category_List = BudgetingController.lstPrdCateg;
            lookupData.CostElement_List = BudgetingController.lstCostElement;
            lookupData.OrderStatus_List = BudgetingController.lstOrderStatus;
            lookupData.VendorCategory_List = BudgetingController.lstVendor;
            lookupData.Fund_List = BudgetingController.lstFund;

            lookupData.BudgetCodeList = BudgetingController.BudgetCodeList;
            lookupData.PurchaseType_List = BudgetingController.lstPurchaseType;
            lookupData.UOM_List = BudgetingController.lstUOM;
            lookupData.UnloadingPoint_List = BudgetingController.lstUnloadingPoint;
            lookupData.Order_Type_List = BudgetingController.lstOrderType;
            
            

            DataSet dt_for_headerRow = new DataSet();
            connection();
            OpenConnection();
            string Query = "Select distinct section as RFOApprover from SPOTONData_Table_2022 union select distinct Department as RFOApprover from SPOTONData_Table_2022 union select distinct [group] as RFOApprover from SPOTONData_Table_2022 order by RFOApprover";
            SqlCommand cmd = new SqlCommand(Query, conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            var presentUserNTID = User.Identity.Name.Split('\\')[1].ToUpper();

            lookupData.RFOApprover_List = new List<RFOApprover>();
            foreach (DataRow row in dt.Rows)
            {
                RFOApprover item = new RFOApprover();
                item.Section_Dept_Grp = row["RFOApprover"].ToString();
                lookupData.RFOApprover_List.Add(item);

            }

            Query = "select top 1 with ties " +
            "CostCenter, Department, Dept_Table.ID as DeptID, PersonnelArea, count(CostCenter) " +
            "from SPOTONData_Table_2022 inner join Dept_Table on Dept_Table.Dept = Department " +
            "group by CostCenter,Department, Dept_Table.ID, PersonnelArea " +
            "order by " +
            "ROW_NUMBER() OVER( " +
            "partition by Department, PersonnelArea " +
            "order by Department, PersonnelArea, count(CostCenter) desc " +
            ")";
            cmd = new SqlCommand(Query, conn);
            da = new SqlDataAdapter(cmd);
            dt = new DataTable();
            da.Fill(dt);

            lookupData.CostCenter_List = new List<CostCenterBGSW>();

            foreach (DataRow row in dt.Rows)
            {
                CostCenterBGSW item = new CostCenterBGSW();
                item.CostCenter = row["CostCenter"].ToString();
                item.Dept = row["Department"].ToString();
                item.DeptID = Convert.ToInt32(row["DeptID"].ToString());
                item.UnloadingPoint_Location = row["PersonnelArea"].ToString();
                lookupData.CostCenter_List.Add(item);
            }

            Query = "Select * from BGSW_BudgetCenter_Table ";

            cmd = new SqlCommand(Query, conn);
            da = new SqlDataAdapter(cmd);
            dt = new DataTable();
            da.Fill(dt);

            //var presentUserNTID = User.Identity.Name.Split('\\')[1].ToUpper();

            lookupData.BudgetCenter_List = new List<BGSW_BudgetCenter_Table>();

            foreach (DataRow row in dt.Rows)
            {
                BGSW_BudgetCenter_Table item = new BGSW_BudgetCenter_Table();
                item.BudgetCenter = row["BudgetCenter"].ToString();
                item.ID = Convert.ToInt32(row["ID"].ToString());
                lookupData.BudgetCenter_List.Add(item);
            }


            Query = " Exec [dbo].[GetReqItemsList_RFO_View] '" + (int.Parse(year) + 1) + "', '" + User.Identity.Name.Split('\\')[1].ToUpper() + "', '" + "LookUp" + "' ";
            cmd = new SqlCommand(Query, conn);
            da = new SqlDataAdapter(cmd);
            da.Fill(dt_for_headerRow);
            CloseConnection();
            lookupData.Item_HeaderFilter = new List<HeaderFilter_Table>();
            lookupData.BU_HeaderFilter = new List<HeaderFilter_Table>();
            lookupData.DEPT_HeaderFilter = new List<HeaderFilter_Table>();
            lookupData.Group_HeaderFilter = new List<HeaderFilter_Table>();
            lookupData.OEM_HeaderFilter = new List<HeaderFilter_Table>();
            lookupData.Category_HeaderFilter = new List<HeaderFilter_Table>();
            lookupData.CostElement_HeaderFilter = new List<HeaderFilter_Table>();
            lookupData.OrderStatus_HeaderFilter = new List<HeaderFilter_Table>();

            lookupData.BudgetCode_HeaderFilter = new List<HeaderFilter_Table>();
            lookupData.OrderDescription_HeaderFilter = new List<HeaderFilter_Table>();
            lookupData.OrderDescription_List = new List<OrderStatusDescription>();

            foreach (DataRow item in dt_for_headerRow.Tables[0].Rows)
            {
                HeaderFilter_Table i_header = new HeaderFilter_Table();
                i_header.text = item["Item Name"].ToString();
                i_header.value = Convert.ToInt32(item["S#No"].ToString());
                lookupData.Item_HeaderFilter.Add(i_header);
            }
            foreach (DataRow item in dt_for_headerRow.Tables[1].Rows)
            {
                HeaderFilter_Table i_header = new HeaderFilter_Table();
                i_header.text = item["BU"].ToString();
                i_header.value = Convert.ToInt32(item["ID"].ToString());
                if (year.Contains("2020") && i_header.text.Contains("MB"))
                {
                    i_header.text = "AS";

                }
                if (year.Contains("2020") && i_header.text.Contains("OSS"))
                {
                    i_header.text = "PS";

                }
                lookupData.BU_HeaderFilter.Add(i_header);
            }
            foreach (DataRow item in dt_for_headerRow.Tables[2].Rows)
            {
                HeaderFilter_Table i_header = new HeaderFilter_Table();
                i_header.text = item["DEPT"].ToString();
                i_header.value = Convert.ToInt32(item["ID"].ToString());
                lookupData.DEPT_HeaderFilter.Add(i_header);
            }
            foreach (DataRow item in dt_for_headerRow.Tables[3].Rows)
            {
                HeaderFilter_Table i_header = new HeaderFilter_Table();
                i_header.text = item["Group"].ToString();
                i_header.value = Convert.ToInt32(item["ID"].ToString());
                lookupData.Group_HeaderFilter.Add(i_header);
            }
            foreach (DataRow item in dt_for_headerRow.Tables[4].Rows)
            {
                HeaderFilter_Table i_header = new HeaderFilter_Table();
                i_header.text = item["OEM"].ToString();
                i_header.value = Convert.ToInt32(item["ID"].ToString());
                lookupData.OEM_HeaderFilter.Add(i_header);
            }

            foreach (DataRow item in dt_for_headerRow.Tables[5].Rows)
            {
                HeaderFilter_Table i_header = new HeaderFilter_Table();
                i_header.text = item["Category"].ToString();
                i_header.value = Convert.ToInt32(item["ID"].ToString());
                lookupData.Category_HeaderFilter.Add(i_header);
            }
            foreach (DataRow item in dt_for_headerRow.Tables[6].Rows)
            {
                HeaderFilter_Table i_header = new HeaderFilter_Table();
                i_header.text = item["CostElement"].ToString();
                i_header.value = Convert.ToInt32(item["ID"].ToString());
                lookupData.CostElement_HeaderFilter.Add(i_header);
            }
            foreach (DataRow item in dt_for_headerRow.Tables[7].Rows)
            {
                HeaderFilter_Table i_header = new HeaderFilter_Table();
                i_header.text = item["OrderStatus"].ToString();
                i_header.value = Convert.ToInt32(item["ID"].ToString());
                lookupData.OrderStatus_HeaderFilter.Add(i_header);
            }

            foreach (DataRow item in dt_for_headerRow.Tables[9].Rows)
            {
                HeaderFilter_Table i_header = new HeaderFilter_Table();
                i_header.text = item["Description"].ToString();
                i_header.value = Convert.ToInt32(item["ID"].ToString());
                lookupData.OrderDescription_HeaderFilter.Add(i_header);
            }

            foreach (DataRow row in dt_for_headerRow.Tables[9].Rows)
            {
                OrderStatusDescription item = new OrderStatusDescription();
                item.Description = row["Description"].ToString().Trim();
                item.ID = Convert.ToInt32(row["ID"].ToString());
                lookupData.OrderDescription_List.Add(item);
            }

            BudgetingController.lstOrderDescription = lookupData.OrderDescription_List;
            //foreach (DataRow item in dt_for_headerRow.Tables[8].Rows)
            //{
            //    HeaderFilter_Table i_header = new HeaderFilter_Table();
            //    i_header.text = item["BudgetCode"].ToString();
            //    i_header.value = Convert.ToInt32(item["BudgetCode"].ToString());
            //    lookupData.BudgetCode_HeaderFilter.Add(i_header);
            //}



            return Json(new { data = lookupData }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public ActionResult Lookup___(string year)
        {
            LookupData lookupData = new LookupData();

            lookupData.BU_List = BudgetingController.lstBUs.OrderBy(x => x.ID).ToList();
            //if (year.Contains("2020"))
            //{
            //    lookupData.BU_List[2].BU = "AS";
            //    lookupData.BU_List[4].BU = "PS";
            //}
            //else
            //{
            //    lookupData.BU_List[2].BU = "MB";
            //    lookupData.BU_List[4].BU = "OSS";
            //}
            //lookupData.BU_List = lstBUs;
            lookupData.OEM_List = BudgetingController.lstOEMs;
            lookupData.DEPT_List = BudgetingController.lstDEPTs;//.FindAll(x => x.Outdated == false);
                                                                // lookupData.Groups_test = BudgetingController.lstGroups_test;//.FindAll(x => x.Outdated == false);
                                                                //lookupData.Groups_List = lstGroups/*.FindAll(x => x.Outdated == false)*/;
            lookupData.Item_List = BudgetingController.lstItems/*.FindAll(item=>item.Deleted !=true)*/;
            lookupData.Category_List = BudgetingController.lstPrdCateg;
            lookupData.CostElement_List = BudgetingController.lstCostElement;
            lookupData.OrderStatus_List = BudgetingController.lstOrderStatus;
            lookupData.VendorCategory_List = BudgetingController.lstVendor;
            lookupData.Fund_List = BudgetingController.lstFund;

            lookupData.BudgetCodeList = BudgetingController.BudgetCodeList;
            lookupData.PurchaseType_List = BudgetingController.lstPurchaseType;
            lookupData.UOM_List = BudgetingController.lstUOM;
            lookupData.UnloadingPoint_List = BudgetingController.lstUnloadingPoint;
            lookupData.Order_Type_List = BudgetingController.lstOrderType;

            DataSet dt_for_headerRow = new DataSet();
            connection();
            OpenConnection();
            string Query = "Select distinct section as RFOApprover from SPOTONData_Table_2022 union select distinct Department as RFOApprover from SPOTONData_Table_2022 union select distinct [group] as RFOApprover from SPOTONData_Table_2022 order by RFOApprover";
            SqlCommand cmd = new SqlCommand(Query, conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            var presentUserNTID = User.Identity.Name.Split('\\')[1].ToUpper();

            lookupData.RFOApprover_List = new List<RFOApprover>();
            foreach (DataRow row in dt.Rows)
            {
                RFOApprover item = new RFOApprover();
                item.Section_Dept_Grp = row["RFOApprover"].ToString();
                lookupData.RFOApprover_List.Add(item);

            }

            Query = "select top 1 with ties " +
            "CostCenter, Department, Dept_Table.ID as DeptID, PersonnelArea, count(CostCenter) " +
            "from SPOTONData_Table_2022 inner join Dept_Table on Dept_Table.Dept = Department " +
            "group by CostCenter,Department, Dept_Table.ID, PersonnelArea " +
            "order by " +
            "ROW_NUMBER() OVER( " +
            "partition by Department, PersonnelArea " +
            "order by Department, PersonnelArea, count(CostCenter) desc " +
            ")";
            cmd = new SqlCommand(Query, conn);
            da = new SqlDataAdapter(cmd);
            dt = new DataTable();
            da.Fill(dt);

            lookupData.CostCenter_List = new List<CostCenterBGSW>();

            foreach (DataRow row in dt.Rows)
            {
                CostCenterBGSW item = new CostCenterBGSW();
                item.CostCenter = row["CostCenter"].ToString();
                item.Dept = row["Department"].ToString();
                item.DeptID = Convert.ToInt32(row["DeptID"].ToString());
                item.UnloadingPoint_Location = row["PersonnelArea"].ToString();
                lookupData.CostCenter_List.Add(item);
            }

            Query = "Select * from BGSW_BudgetCenter_Table ";

            cmd = new SqlCommand(Query, conn);
            da = new SqlDataAdapter(cmd);
            dt = new DataTable();
            da.Fill(dt);

            //var presentUserNTID = User.Identity.Name.Split('\\')[1].ToUpper();

            lookupData.BudgetCenter_List = new List<BGSW_BudgetCenter_Table>();

            foreach (DataRow row in dt.Rows)
            {
                BGSW_BudgetCenter_Table item = new BGSW_BudgetCenter_Table();
                item.BudgetCenter = row["BudgetCenter"].ToString();
                item.ID = Convert.ToInt32(row["ID"].ToString());
                lookupData.BudgetCenter_List.Add(item);
            }

            ////////////////////////
            Query = "Select * from ItemsCostList_Table_2023_08_29_ELO where 1=1";

            cmd = new SqlCommand(Query, conn);
            da = new SqlDataAdapter(cmd);
            dt = new DataTable();
            da.Fill(dt);

            //var presentUserNTID = User.Identity.Name.Split('\\')[1].ToUpper();

            lookupData.Item_List = new List<ItemsCostList_Table>();

            foreach (DataRow row in dt.Rows)
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
                lookupData.Item_List.Add(item);
            }

            List<Dictionary<string, object>> parentRow6 = new List<Dictionary<string, object>>();
            Dictionary<string, object> childRow6;
            //foreach (DataRow row in dt.Rows)
            foreach (DataRow row in dt.Rows)
            {

                childRow6 = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns)
                {
                    childRow6.Add(col.ColumnName, row[col]);
                }
                parentRow6.Add(childRow6);
            }
            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            jsSerializer.MaxJsonLength = Int32.MaxValue;

            var resultData6 = parentRow6;
            var result6 = new ContentResult
            {
                Content = jsSerializer.Serialize(resultData6),
                ContentType = "application/json"
            };

            Query = "Select * from Groups_Table_Test";

            cmd = new SqlCommand(Query, conn);
            da = new SqlDataAdapter(cmd);
            dt = new DataTable();
            da.Fill(dt);
            List<Dictionary<string, object>> parentRow7 = new List<Dictionary<string, object>>();
            Dictionary<string, object> childRow7;
            //foreach (DataRow row in dt.Rows)
            foreach (DataRow row in dt.Rows)
            {

                childRow7 = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns)
                {
                    childRow7.Add(col.ColumnName, row[col]);
                }
                parentRow6.Add(childRow7);
            }
            jsSerializer = new JavaScriptSerializer();
            jsSerializer.MaxJsonLength = Int32.MaxValue;

            var resultData7 = parentRow6;
            var result7 = new ContentResult
            {
                Content = jsSerializer.Serialize(resultData6),
                ContentType = "application/json"
            };


            /// 
            /// 
            /// 
            /// 
            /// 
            ///


            Query = " Exec [dbo].[GetReqItemsList_RFO_View] '" + (int.Parse(year) + 1) + "', '" + User.Identity.Name.Split('\\')[1].ToUpper() + "', '" + "LookUp" + "' ";
            cmd = new SqlCommand(Query, conn);
            da = new SqlDataAdapter(cmd);
            da.Fill(dt_for_headerRow);
            CloseConnection();
            lookupData.Item_HeaderFilter = new List<HeaderFilter_Table>();
            lookupData.BU_HeaderFilter = new List<HeaderFilter_Table>();
            lookupData.DEPT_HeaderFilter = new List<HeaderFilter_Table>();
            lookupData.Group_HeaderFilter = new List<HeaderFilter_Table>();
            lookupData.OEM_HeaderFilter = new List<HeaderFilter_Table>();
            lookupData.Category_HeaderFilter = new List<HeaderFilter_Table>();
            lookupData.CostElement_HeaderFilter = new List<HeaderFilter_Table>();
            lookupData.OrderStatus_HeaderFilter = new List<HeaderFilter_Table>();

            lookupData.BudgetCode_HeaderFilter = new List<HeaderFilter_Table>();

            foreach (DataRow item in dt_for_headerRow.Tables[0].Rows)
            {
                HeaderFilter_Table i_header = new HeaderFilter_Table();
                i_header.text = item["Item Name"].ToString();
                i_header.value = Convert.ToInt32(item["S#No"].ToString());
                lookupData.Item_HeaderFilter.Add(i_header);
            }
            foreach (DataRow item in dt_for_headerRow.Tables[1].Rows)
            {
                HeaderFilter_Table i_header = new HeaderFilter_Table();
                i_header.text = item["BU"].ToString();
                i_header.value = Convert.ToInt32(item["ID"].ToString());
                if (year.Contains("2020") && i_header.text.Contains("MB"))
                {
                    i_header.text = "AS";

                }
                if (year.Contains("2020") && i_header.text.Contains("OSS"))
                {
                    i_header.text = "PS";

                }
                lookupData.BU_HeaderFilter.Add(i_header);
            }
            foreach (DataRow item in dt_for_headerRow.Tables[2].Rows)
            {
                HeaderFilter_Table i_header = new HeaderFilter_Table();
                i_header.text = item["DEPT"].ToString();
                i_header.value = Convert.ToInt32(item["ID"].ToString());
                lookupData.DEPT_HeaderFilter.Add(i_header);
            }
            foreach (DataRow item in dt_for_headerRow.Tables[3].Rows)
            {
                HeaderFilter_Table i_header = new HeaderFilter_Table();
                i_header.text = item["Group"].ToString();
                i_header.value = Convert.ToInt32(item["ID"].ToString());
                lookupData.Group_HeaderFilter.Add(i_header);
            }
            foreach (DataRow item in dt_for_headerRow.Tables[4].Rows)
            {
                HeaderFilter_Table i_header = new HeaderFilter_Table();
                i_header.text = item["OEM"].ToString();
                i_header.value = Convert.ToInt32(item["ID"].ToString());
                lookupData.OEM_HeaderFilter.Add(i_header);
            }

            foreach (DataRow item in dt_for_headerRow.Tables[5].Rows)
            {
                HeaderFilter_Table i_header = new HeaderFilter_Table();
                i_header.text = item["Category"].ToString();
                i_header.value = Convert.ToInt32(item["ID"].ToString());
                lookupData.Category_HeaderFilter.Add(i_header);
            }
            foreach (DataRow item in dt_for_headerRow.Tables[6].Rows)
            {
                HeaderFilter_Table i_header = new HeaderFilter_Table();
                i_header.text = item["CostElement"].ToString();
                i_header.value = Convert.ToInt32(item["ID"].ToString());
                lookupData.CostElement_HeaderFilter.Add(i_header);
            }
            foreach (DataRow item in dt_for_headerRow.Tables[7].Rows)
            {
                HeaderFilter_Table i_header = new HeaderFilter_Table();
                i_header.text = item["OrderStatus"].ToString();
                i_header.value = Convert.ToInt32(item["ID"].ToString());
                lookupData.OrderStatus_HeaderFilter.Add(i_header);
            }

            //foreach (DataRow item in dt_for_headerRow.Tables[8].Rows)
            //{
            //    HeaderFilter_Table i_header = new HeaderFilter_Table();
            //    i_header.text = item["BudgetCode"].ToString();
            //    i_header.value = Convert.ToInt32(item["BudgetCode"].ToString());
            //    lookupData.BudgetCode_HeaderFilter.Add(i_header);
            //}

            return Json(new { data = lookupData, /*itemList = result6 ,*/ /*grpList = result7*/}, JsonRequestBehavior.AllowGet);

        }


        public ActionResult Lookup_ItemList(string year)
        {
            connection();
            OpenConnection();
            string Query = " Exec[dbo].[RFO_LookUp_Item] " + (int.Parse(year)+1);
            SqlCommand cmd = new SqlCommand(Query, conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
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



            return new ContentResult { Content = JsonConvert.SerializeObject(parentRow), ContentType = "application/json" };
        }

        //[HttpPost]
        //public ActionResult Lookup_ItemList(string year)
        //{
        //    connection();
        //    OpenConnection();
        //    string Query = " Exec[dbo].[RFO_LookUp_Item] ";
        //   SqlCommand  cmd = new SqlCommand(Query, conn);
        //   SqlDataAdapter da = new SqlDataAdapter(cmd);
        //   DataTable dt = new DataTable();
        //    da.Fill(dt);
        //    CloseConnection();
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
        //        ContentType = "application/json",
        //        ContentEncoding = Encoding.UTF8
        //    };
        //    JsonResult result1 = Json(result);
        //   // result1.MaxJsonLength = Int32.MaxValue;
        //    result1.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
        //    result1.ContentEncoding = Encoding.UTF8;
        //    result1.ContentType = "application/json";

        //    //return Json(new {  Data = result1 }, JsonRequestBehavior.AllowGet);
        //    return result1;
        //}

        public ActionResult Lookup_GroupList(string year)
        {
            connection();
            OpenConnection();
            string Query = " Exec[dbo].[RFO_LookUp_Group] ";

            SqlCommand cmd = new SqlCommand(Query, conn);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            CloseConnection();

            List<Dictionary<string, object>> parentRow6 = new List<Dictionary<string, object>>();
            Dictionary<string, object> childRow6;
            //foreach (DataRow row in dt.Rows)
            foreach (DataRow row in dt.Rows)
            {

                childRow6 = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns)
                {
                    childRow6.Add(col.ColumnName, row[col]);
                }
                parentRow6.Add(childRow6);
            }
            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            jsSerializer.MaxJsonLength = Int32.MaxValue;

            var resultData6 = parentRow6;
            var result6 = new ContentResult
            {
                Content = jsSerializer.Serialize(resultData6),
                ContentType = "application/json"
            };
            return Json(new { groupList = result6 }, JsonRequestBehavior.AllowGet);

        }


        public string GetSectionCoordinatorsNTID(string deptID)
        {

            string NTID = "";
            try
            {
                DataTable dt1 = new DataTable();
                connection();
                OpenConnection();
                string Query = " Exec [dbo].[GetSectionCoordinatorsDetails] '" + (Convert.ToInt32(deptID)) + "' ";
                SqlCommand cmd = new SqlCommand(Query, conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt1);
                int count = 0;
                foreach (DataRow item in dt1.Rows)
                {
                    if (count == 0)
                    {
                        NTID = item["NTID"].ToString();
                    }
                    else
                    {
                        NTID = NTID + ',' + item["NTID"].ToString();
                    }
                    count++;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                CloseConnection();
            }



            return NTID;



        }
        public string GetCommonMail(int requestID)
        {
            string getEmailcc = "";
            try
            {
                connection();



                string Query = "select [Common Mailbox] from Mail_RFOAuthorization_Table where Section in (Select distinct Section from SPOTONData_Table_2022 where Department in (Select Dept from DEPT_Table where ID in (Select DEPT from RequestItems_Table where RequestID = " + requestID + "))) OR TopSection in (Select distinct TopSection from SPOTONData_Table_2022 where Department in (Select Dept from DEPT_Table where ID in (Select DEPT from RequestItems_Table where RequestID = " + requestID + ")))";
                OpenConnection();
                SqlCommand cmd = new SqlCommand(Query, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    getEmailcc = dr["Common Mailbox"].ToString();
                }
                dr.Close();
                CloseConnection();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                CloseConnection();
            }



            return getEmailcc;
        }

        public string GetRFOApprover(int requestID)
        {
            string getEmailcc = "";
            try
            {
                connection();

                string Query = "select isnull(NTID,'') as RFOApprover from BGSW_RFOMailConfiguration Where IsPaused = 0 and Section in (Select ltrim(rtrim(RFOApprover)) from RequestItems_Table where RequestID = " + requestID + ") ";
                OpenConnection();
                SqlCommand cmd = new SqlCommand(Query, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    getEmailcc = dr["RFOApprover"].ToString();
                }
                dr.Close();
                CloseConnection();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                CloseConnection();
            }



            return getEmailcc;
        }
    }

    public class OEMList
    {
        public OEMList()
        {
            this.OEMSelectList = new List<SelectListItem>();
        }
        public List<SelectListItem> OEMSelectList { get; set; }
        public string isDashboard { get; set; }

    }

    public partial class OrderTypeBGSW
    {
        public int ID { get; set; }
        public string OrderType { get; set; }
        public string BudgetCenter { get; set; }
        public string CostCenter { get; set; }


    }
    public partial class RFOApprover

    {

        public string Section_Dept_Grp { get; set; }
    }

    public partial class SectionList
    {
        public string Section { get; set; }
    }

    public partial class UOMBGSW
    {
        public int ID { get; set; }
        public string UOM { get; set; }
    }

    public partial class UnloadingPointBGSW
    {
        public int ID { get; set; }
        public string Address { get; set; }
        public string UnloadingPoint { get; set; }
        public string Location { get; set; }
    }

    public partial class CostCenterBGSW
    {
        public int DeptID { get; set; }
        public string Dept { get; set; }
        public string UnloadingPoint_Location { get; set; }
        public string CostCenter { get; set; }
    }

    public partial class BudgetCodeList
    {
        public string Code { get; set; }
        public string Description { get; set; }
    }

    public partial class CurrencyList
    {
        public string ID { get; set; }
        public string Currency { get; set; }
    }

}


// WORKiNG solution
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web.Mvc;
//using System.Data.Entity;
//using LC_Reports_V1.Models;
//using System.Data;
//using System.IO;
//using System.Globalization;
//using System.Web.UI.WebControls;
//using ClosedXML.Excel;
//using System.Configuration;
//using System.Data.SqlClient;

//namespace LC_Reports_V1.Controllers
//{
//    public class BudgetingOrderController : Controller
//    {
//        public static List<SPOTONData_Table_2022> lstUsers = new List<SPOTONData_Table_2022>();
//        public static List<BU_Table> lstBUs = new List<BU_Table>();

//        public static List<DEPT_Table> lstDEPTs = new List<DEPT_Table>();
//        //public static List<Groups_Table> lstGroups = new List<Groups_Table>();
//        public static List<OEM_Table> lstOEMs = new List<OEM_Table>();

//        public static List<Category_Table> lstPrdCateg = new List<Category_Table>();
//        public static List<ItemsCostList_Table> lstItems = new List<ItemsCostList_Table>();
//        public static List<CostElement_Table> lstCostElement = new List<CostElement_Table>();
//        public static List<tbl_UserIDs_Table> lstPrivileged = new List<tbl_UserIDs_Table>();
//        public static List<BU_SPOCS> lstBU_SPOCs = new List<BU_SPOCS>();
//        public static List<Groups_Table_Test> lstGroups_test = new List<Groups_Table_Test>(); //with old new groups
//        public static List<Order_Status_Table> lstOrderStatus = new List<Order_Status_Table>();
//        public static List<Fund_Table> lstFund = new List<Fund_Table>();
//        public static List<LeadTime_Table> lstVendor = new List<LeadTime_Table>();
//        public static bool isDashboard_flag = false;

//        public static List<BudgetCodeMaster> BudgetCodeList = new List<BudgetCodeMaster>();

//        // GET: RequestItems
//        public ActionResult Index(string isDashboard = "Lab RFO")
//        {

//            //if (lstUsers == null || lstUsers.Count == 0)
//            //{

//            //    return RedirectToAction("Index", "Budgeting");
//            //}
//            OEMList oemlist = new OEMList();



//            using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

//            //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

//            {
//                if (lstUsers == null || lstUsers.Count == 0)
//                    lstUsers = db.SPOTONData_Table_2022.ToList<SPOTONData_Table_2022>();
//                if (lstOEMs == null || lstOEMs.Count == 0)
//                    lstOEMs = db.OEM_Table.ToList<OEM_Table>();
//                if (lstBUs == null || lstBUs.Count == 0)
//                    lstBUs = db.BU_Table.ToList<BU_Table>();
//                if (lstDEPTs == null || lstDEPTs.Count == 0)
//                    lstDEPTs = db.DEPT_Table.ToList<DEPT_Table>();
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
//                if (lstGroups_test == null || lstGroups_test.Count == 0)
//                    lstGroups_test = db.Groups_Table_Test.ToList<Groups_Table_Test>();
//                if (lstOrderStatus == null || lstOrderStatus.Count == 0)
//                    lstOrderStatus = db.Order_Status_Table.ToList<Order_Status_Table>();
//                if (lstVendor == null || lstVendor.Count == 0)
//                    lstVendor = db.LeadTime_Table.ToList<LeadTime_Table>();
//                if (lstFund == null || lstFund.Count == 0)
//                    lstFund = db.Fund_Table.ToList<Fund_Table>();

//                BudgetCodeList = BudgetingController.BudgetCodeList;

//                lstUsers.Sort((a, b) => a.EmployeeName.CompareTo(b.EmployeeName));
//                lstBUs.Sort((a, b) => a.BU.CompareTo(b.BU));
//                lstOEMs.Sort((a, b) => a.OEM.CompareTo(b.OEM));
//                lstDEPTs.Sort((a, b) => a.DEPT.CompareTo(b.DEPT));
//                lstCostElement.Sort((a, b) => a.CostElement.CompareTo(b.CostElement));
//                lstPrdCateg.Sort((a, b) => a.Category.CompareTo(b.Category));
//                lstItems.Sort((a, b) => a.Item_Name.CompareTo(b.Item_Name));
//                lstPrivileged.Sort((a, b) => a.ADSID.CompareTo(b.ADSID));
//                lstBU_SPOCs.Sort((a, b) => a.VKMspoc.CompareTo(b.VKMspoc));
//                lstGroups_test.Sort((a, b) => a.Group.CompareTo(b.Group));
//                lstOrderStatus.Sort((a, b) => a.OrderStatus.CompareTo(b.OrderStatus));
//                lstVendor.Sort((a, b) => a.VendorCategory.CompareTo(b.VendorCategory));
//                lstFund.Sort((a, b) => a.Fund.CompareTo(b.Fund));

//                foreach (var oem in lstOEMs)
//                {
//                    oemlist.OEMSelectList.Add(new SelectListItem { Text = oem.OEM, Value = oem.ID.ToString() });
//                }
//                oemlist.OEMSelectList.Sort((a, b) => a.Text.CompareTo(b.Text));

//            }
//            oemlist.isDashboard = isDashboard;
//            if (isDashboard.Contains("RFO"))
//                isDashboard_flag = false;
//            else
//                isDashboard_flag = true;

//            return View(oemlist);
//        }


//        private SqlConnection conn;

//        private void connection()
//        {

//            string connString = ConfigurationManager.ConnectionStrings["BudgetingdbConnection"].ToString();
//            conn = new SqlConnection(connString);

//        }

//        private void OpenConnection()
//        {
//            if (conn.State == ConnectionState.Closed)
//            {
//                conn.Open();
//            }
//        }

//        private void CloseConnection()
//        {
//            if (conn.State == ConnectionState.Open)
//            {
//                conn.Close();
//            }
//        }






//        /// <summary>
//        /// function to fetch the Request Items made by the Requestor during the year chosen for view
//        /// /// <param name="year"></param>
//        /// </summary>

//        [HttpPost]
//        public ActionResult GetData(string year/*string[] oem*/)
//        {


//            using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

//            //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

//            {
//                try
//                {

//                    List<RequestItemsRepoView> viewList = new List<RequestItemsRepoView>();
//                    viewList = GetData1(year);
//                    if (db.OrderingRequestor_Table.ToList().Find(person => person.NTID.ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())) != null)
//                    {   if(db.OrderingRequestor_Table.ToList().Find(person => person.NTID.ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).isSuperUser != null)
//                        {
//                            if(db.OrderingRequestor_Table.ToList().Find(person => person.NTID.ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).isSuperUser == true)
//                            {
//                                List<string> BUs_rfo = db.OrderingRequestor_Table.ToList().Find(person => person.NTID.ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).BU.Split(',').ToList();
//                                var detail_superuser = viewList.FindAll(x => BUs_rfo.Contains(x.BU.ToString().Trim()));
//                                return Json(new { success = true, data = detail_superuser , isDashboard_flag}, JsonRequestBehavior.AllowGet);
//                            }

//                        }
//                         var DEPT = db.OrderingRequestor_Table.ToList().Find(person => person.NTID.ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).Department.Trim();
//                         var detail = viewList.FindAll(dpt => dpt.DEPT.ToString().Trim().Equals(DEPT));

//                        return Json(new { success = true, data = detail, isDashboard_flag }, JsonRequestBehavior.AllowGet);
//                    }
//                    else
//                    {
//                        var spoton_dept = db.SPOTONData_Table_2022.ToList().Find(person => person.NTID.ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).Department.Trim();
//                        string dept_id = lstDEPTs.Find(dpt => dpt.DEPT == spoton_dept).ID.ToString();
//                        var detail = viewList.FindAll(dpt => dpt.DEPT.ToString().Trim().Equals(dept_id));
//                        return Json(new { success = false, data = detail, isDashboard_flag }, JsonRequestBehavior.AllowGet);
//                        //return Json(new { success = false, message = "Sorry! Current user is not authorised to view the Orders!" }, JsonRequestBehavior.AllowGet);
//                    }
//                }

//                catch (Exception ex)
//                {
//                    return Json(new { success = true, message = "Unable to load the Item Requests, Please Try again later!" }, JsonRequestBehavior.AllowGet);

//                }

//            }
//        }


//        public List<RequestItemsRepoView> GetData1(string year/*string[] oem*/)
//        {
//            List<RequestItemsRepoView> viewList = new List<RequestItemsRepoView>();
//            //string PresentUserDept_RequestTable = lstDEPTs.Find(dept => dept.DEPT.Equals(presentUserDept)).ID.ToString();
//            try
//            {

//                using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

//                //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

//                {

//                    System.Security.Principal.IPrincipal User = System.Web.HttpContext.Current.User;
//                    bool is_Authorized = false;
//                    string presentUserName = lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).EmployeeName;
//                    var x = presentUserName.TrimEnd().TrimStart().Split(' ').ToList();
//                    //string presentUserNTID = string.Empty;
//                    x.Sort();
//                    //List<RequestItems_Table> reqList = new List<RequestItems_Table>();
//                    //reqList = db.RequestItems_Table.ToList<RequestItems_Table>().FindAll(xi => xi.ApprovedSH == true);

//                    DataTable dt1 = new DataTable();
//                    connection();
//                    OpenConnection();
//                    //string Query = " Exec [dbo].[GetReqItemsList_VKMSPOCReviews_View] '" + useryear + "', 'MAE9COB', 'Export All' ";
//                    string Query = " Exec [dbo].[GetReqItemsList_RFO_View] '" + (Convert.ToInt32(year) + 1) + "', '" + User.Identity.Name.Split('\\')[1].ToUpper() + "', 'View' ";
//                    SqlCommand cmd = new SqlCommand(Query, conn);
//                    SqlDataAdapter da = new SqlDataAdapter(cmd);
//                    da.Fill(dt1);
//                    CloseConnection();

//                    foreach (DataRow item in dt1.Rows)
//                    {
//                        //var y = item.RequestorNT.TrimStart().TrimEnd().Split(' ').ToList();
//                        //y.Sort();
//                        //for (int i = 0; i < x.Count(); i++)
//                        //{
//                        //    if (x[i] != y[i])
//                        //        break;
//                        //}

//                        RequestItemsRepoView ritem = new RequestItemsRepoView();

//                        ritem.BU = int.Parse(item["BU"].ToString());
//                        ritem.BudgetCode = item["BudgetCode"].ToString();
//                        ritem.RequestorNTID = item["RequestorNTID"].ToString();


//                        ritem.isProjected = (bool)((item["isProjected"] != null && item["isProjected"].ToString() != string.Empty) ? item["isProjected"] : false);
//                        ritem.Q1 = (bool)((item["Q1"] != null && item["Q1"].ToString() != string.Empty) ? item["Q1"] : false);
//                        ritem.Q2 = (bool)((item["Q2"] != null && item["Q2"].ToString() != string.Empty) ? item["Q2"] : false);
//                        ritem.Q3 = (bool)((item["Q3"] != null && item["Q3"].ToString() != string.Empty) ? item["Q3"] : false);
//                        ritem.Q4 = (bool)((item["Q4"] != null && item["Q4"].ToString() != string.Empty) ? item["Q4"] : false);

//                        ritem.Is_UnplannedF02Item = (bool)((item["Is_UnplannedF02Item"] != null && item["Is_UnplannedF02Item"].ToString() != string.Empty) ? item["Is_UnplannedF02Item"] : false);
//                        //ritem.Projected_Amount = item["Projected_Amount"] != null ? Convert.ToDecimal(item["Projected_Amount"].ToString()) : 0;
//                        //ritem.Unused_Amount = item["Unused_Amount"] != null ? Convert.ToDecimal(item["Unused_Amount"].ToString()) : 0;



//                        if (item["Projected_Amount"].ToString() != "" && item["Projected_Amount"] != null)
//                            ritem.Projected_Amount = (item["Projected_Amount"].ToString() != "" && item["Projected_Amount"] != null) ? Math.Round((decimal)item["Projected_Amount"], MidpointRounding.AwayFromZero) : 0;

//                        if (item["Unused_Amount"].ToString() != "" && item["Unused_Amount"] != null)
//                            ritem.Unused_Amount = item["Unused_Amount"] != null ? Math.Round((decimal)item["Unused_Amount"], MidpointRounding.AwayFromZero) : 0;

//                        if (item["UpdatedAt"].ToString() != "" && item["UpdatedAt"] != null)
//                            ritem.UpdatedAt = Convert.ToDateTime(item["UpdatedAt"]);

//                        ritem.Requestor = item["RequestorNT"].ToString();

//                        if (item["ActualAvailableQuantity"] == null || item["ActualAvailableQuantity"].ToString().Trim() == string.Empty)
//                            ritem.ActualAvailableQuantity = "NA";
//                        else
//                            ritem.ActualAvailableQuantity = item["ActualAvailableQuantity"].ToString();

//                                ritem.VKM_Year = (int)item["VKM_Year"];
//                                ritem.PORemarks = item["PORemarks"].ToString();
//                                ritem.BU = int.Parse(item["BU"].ToString());
//                                if (item["Category"] != null && item["Category"].ToString() != "")
//                                    ritem.Category = int.Parse(item["Category"].ToString());

//                                ritem.Comments = item["Comments"].ToString();
//                                if (item["CostElement"] != null && item["CostElement"].ToString() != "")
//                                    ritem.Cost_Element = int.Parse(item["CostElement"].ToString());

//                                ritem.DEPT = int.Parse(item["DEPT"].ToString());
//                                ritem.Group = int.Parse(item["Group"].ToString());
//                                ritem.Item_Name = int.Parse(item["ItemName"].ToString() != "" ? item["ItemName"].ToString() : "0");
//                                ritem.OEM = int.Parse(item["OEM"].ToString());
//                                ritem.Required_Quantity = int.Parse(item["ReqQuantity"].ToString() != "" ? item["ReqQuantity"].ToString() : "0");
//                                ritem.Total_Price = decimal.Parse(item["TotalPrice"].ToString() != "" ? item["TotalPrice"].ToString() : "0");
//                                ritem.Reviewed_Quantity = int.Parse(item["ApprQuantity"].ToString() != "" ? item["ApprQuantity"].ToString() : "0");
//                                ritem.RequestID = int.Parse(item["RequestID"].ToString());
//                                ritem.Requestor = item["RequestorNT"].ToString();
//                                ritem.Reviewed_Cost = decimal.Parse(item["ApprCost"].ToString() != "" ? item["ApprCost"].ToString() : "0");
//                                ritem.Unit_Price = decimal.Parse(item["UnitPrice"].ToString() != "" ? item["UnitPrice"].ToString() : "0");
//                                ritem.ApprovalHoE = (bool)item["ApprovalDH"];
//                                ritem.ApprovalSH = (bool)item["ApprovalSH"];
//                                ritem.ApprovedHoE = (bool)item["ApprovedDH"];
//                                ritem.ApprovedSH = (bool)item["ApprovedSH"];


//                        ritem.Reviewer_1 = item["DHNT"].ToString();
//                        ritem.Reviewer_2 = item["SHNT"].ToString();

//                                ritem.RequestDate = item["RequestDate"].ToString() != "" ? ((DateTime)item["RequestDate"]).ToString("yyyy-MM-dd") : string.Empty;
//                                ritem.SubmitDate = item["SubmitDate"].ToString() != "" ? ((DateTime)item["SubmitDate"]).ToString("yyyy-MM-dd") : string.Empty;
//                                ritem.Review1_Date = item["DHAppDate"].ToString() != "" ? ((DateTime)item["DHAppDate"]).ToString("yyyy-MM-dd") : string.Empty;
//                                ritem.Review2_Date = item["SHAppDate"].ToString() != "" ? ((DateTime)item["SHAppDate"]).ToString("yyyy-MM-dd") : string.Empty;

//                        ritem.ActualDeliveryDate = item["ActualDeliveryDate"].ToString() != "" ? ((DateTime)item["ActualDeliveryDate"]).ToString("yyyy-MM-dd") : string.Empty;
//                        ritem.RequiredDate = item["RequiredDate"].ToString() != "" ? ((DateTime)item["RequiredDate"]).ToString("yyyy-MM-dd") : string.Empty;
//                        ritem.RequestOrderDate = item["RequestOrderDate"].ToString() != "" ? ((DateTime)item["RequestOrderDate"]).ToString("yyyy-MM-dd") : string.Empty;
//                        ritem.OrderDate = item["OrderDate"].ToString() != "" ? ((DateTime)item["OrderDate"]).ToString("yyyy-MM-dd") : string.Empty;
//                        ritem.TentativeDeliveryDate = item["TentativeDeliveryDate"].ToString() != "" ? ((DateTime)item["TentativeDeliveryDate"]).ToString("yyyy-MM-dd") : string.Empty;


//                                if (item["Fund"] != null && item["Fund"].ToString() != "")
//                                {
//                                    ritem.Fund = int.Parse(item["Fund"].ToString());
//                                }
//                                else
//                                {
//                                    ritem.Fund = lstFund.Find(fund => fund.Fund.Equals("F02")).ID;
//                                }

//                                if (item["RequestToOrder"].ToString() == "" || (bool)item["RequestToOrder"] != true)
//                                {
//                                    ritem.RequestToOrder = false;
//                                }
//                                else
//                                {
//                                    ritem.RequestToOrder = true;
//                                }
//                                if (item["OrderedQuantity"] != null && item["OrderedQuantity"].ToString() != "")
//                                {
//                                    ritem.OrderedQuantity = (int)item["OrderedQuantity"];
//                                }
//                                else
//                                {
//                                    ritem.OrderedQuantity = null;
//                                }
//                                ritem.OrderPrice = decimal.Parse(item["OrderPrice"].ToString() != "" ? item["OrderPrice"].ToString() : "0");
//                                ritem.OrderID = item["OrderID"].ToString();




//                        if (item["OrderStatus"] != null && item["OrderStatus"].ToString().Trim() != "" && item["OrderStatus"].ToString().Trim() != "0")
//                        {
//                            ritem.OrderStatus = int.Parse(item["OrderStatus"].ToString());

//                        }
//                        else
//                        {
//                            ritem.OrderStatus = null;


//                        }

//                        ritem.BudgetCode = item["BudgetCode"].ToString();
//                        ritem.Comments = item["Comments"].ToString();
//                        ritem.L2_Remarks = item["L2_Remarks"].ToString();
//                        ritem.L3_Remarks = item["L3_Remarks"].ToString();
//                        if (item["L2_Qty"] != DBNull.Value && item["L2_Qty"].ToString().Trim() != "0")
//                        ritem.L2_Qty = int.Parse(item["L2_Qty"].ToString());

//                        viewList.Add(ritem);
//                    }


//                    //************************************************************************************************************
//                    ////check whether the present user is a ordered stage dept spoc
//                    ////if no -> "NOT AUTHORIZED"
//                    ////if yes -> check the depts in orderlist == present user's dept ! = null -> show the data
//                    ////else get the user's dept -> find its dept id -> check whether it is present in  rep id -> if present -> show that dept's orderlist

//                    ////if (db.OrderingRequestor_Table.Find(person => person..Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())) != null)
//                    //if(db.OrderingRequestor_Table.ToList().Find(person=>person.NTID.ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())) !=null)
//                    //{
//                    //    var presentUserNTID = User.Identity.Name.Split('\\')[1].ToUpper();
//                    //    is_Authorized = true;
//                    //    viewList = viewList1.FindAll(xi => xi.SubmitDate.ToString().Contains(year)).FindAll(xi => xi.ApprovedSH == true).FindAll(xi => xi.OrderStatus != lstOrderStatus.Find(status => status.OrderStatus.Equals("Closed")).ID);

//                    //    var presentUserDeptID = lstDEPTs.Find(departmnt => departmnt.DEPT.Equals(lstUsers.Find(xi =>xi.NTID.Trim().ToUpper().Contains(presentUserNTID.Trim())).Department)).ID;
//                    //    var presentUserDeptName = lstUsers.Find(xi => xi.NTID.Trim().ToUpper().Contains(presentUserNTID.Trim())).Department;
//                    //   // if (viewList.FindAll(y=>y.DEPT.Equals(lstDEPTs.Find(dpt=>dpt.ID).DEPT)).Contains(presentUserDeptName))
//                    //   //foreach (var item in viewList)
//                    //   //{
//                    //   //     var c = lstDEPTs.Find(dept => dept.ID.Equals(item.DEPT)).DEPT;

//                    //   //}

//                    //   var z = viewList.FindAll(y => y.DEPT.Equals(presentUserDeptID)).Count();     //check the depts in orderlist == present user's dept ! = null

//                    //    if (z == 0)    //!=null -> show the data
//                    //    {
//                    //        return viewList.FindAll(dpt => dpt.DEPT.Equals(presentUserDeptID));
//                    //    }
//                    //    else          // == null -> check whether userdeptid is present in rep id of DEPT TABLE
//                    //    {
//                    //        var m = lstDEPTs.FindAll(v => v.Rep_ID.Equals(presentUserDeptID)).Count();
//                    //        if (m != 0)  //presentdeptid is a repid of an old dept
//                    //        {
//                    //            var presentuser_olddeptID = lstDEPTs.Find(v => v.Rep_ID.Equals(presentUserDeptID)).ID;

//                    //            var p = viewList.FindAll(y => y.DEPT.Equals(presentuser_olddeptID)).Count();

//                    //            return viewList.FindAll(dpt => dpt.DEPT.Equals(presentuser_olddeptID));
//                    //        }

//                    //        if(m == 0)
//                    //        {
//                    //            //check the prevdept
//                    //        }

//                    //    }

//                    //}
//                    //else
//                    //{
//                    //    is_Authorized = false;

//                    //}

//                    //********************************************************************************************************************************



//                }
//            }
//            catch (Exception ex)
//            {

//            }
//            return viewList.FindAll(y => y.VKM_Year == int.Parse(year) + 1).FindAll(xi => xi.ApprovedSH == true).FindAll(xi => xi.OrderStatus != lstOrderStatus.Find(status => status.OrderStatus.Equals("Closed")).ID);


//        }


//        ///// <summary>
//        ///// function to enable update of an existing item and add a new item request
//        ///// </summary>
//        ///// <param name="req"></param>
//        ///// <param name="useryear"></param>
//        ///// <returns></returns>
//        [HttpPost]
//        public ActionResult AddOrEdit(RequestItemsRepoEdit1 req, string useryear/*string[] oem*/)
//        {
//            List<RequestItemsRepoView> viewList = new List<RequestItemsRepoView>();


//            using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

//            //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

//            {
//                System.Security.Principal.IPrincipal User = System.Web.HttpContext.Current.User;
//                string presentUserName = lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).EmployeeName;

//                RequestItems_Table item = new RequestItems_Table();
//                item.PORemarks = req.PORemarks;
//                item.VKM_Year = req.VKM_Year;
//                item.RequestorNTID = req.RequestorNTID;
//                item.BU = req.BU.ToString();
//                item.ItemName = req.Item_Name.ToString();
//                item.Category = req.Category.ToString();
//                item.Comments = req.Comments;
//                item.CostElement = req.Cost_Element.ToString();
//                item.DEPT = req.DEPT.ToString();
//                item.Group = req.Group.ToString();
//                item.OEM = req.OEM.ToString();
//                item.ReqQuantity = req.Required_Quantity != 0 ? req.Required_Quantity : req.Reviewed_Quantity; //F03 & F01 items - rev Quantity is given
//                item.RequestID = req.RequestID;
//                item.RequestorNT = req.Requestor;
//                item.DHNT = req.Reviewer_1;
//                item.SHNT = req.Reviewer_2;


//                item.UnitPrice = req.Unit_Price;
//                item.TotalPrice = item.ReqQuantity * req.Unit_Price;
//                item.ApprQuantity = req.Reviewed_Quantity;
//                item.OrderedQuantity = req.OrderedQuantity;
//                item.ApprCost = req.Reviewed_Cost == 0 ? req.Reviewed_Quantity * req.Unit_Price : req.Reviewed_Cost;
//                item.RequestDate = req.RequestDate != null ? DateTime.Parse(req.RequestDate).Date : DateTime.Now.Date;
//                item.SubmitDate = req.SubmitDate != null ? DateTime.Parse(req.SubmitDate).Date : DateTime.Now.Date;
//                item.OrderID = req.OrderID != null ? req.OrderID : "";
//                item.OrderPrice = req.OrderPrice;
//                item.ApprovalDH = true;
//                item.ApprovalSH = true;
//                item.ApprovedDH = true;
//                item.ApprovedSH = true;
//                item.DHAppDate = req.Review1_Date != null ? DateTime.Parse(req.Review1_Date).Date : DateTime.Now.Date;
//                item.SHAppDate = req.Review2_Date != null ? DateTime.Parse(req.Review2_Date).Date : DateTime.Now.Date;

//                item.OrderStatus = req.OrderStatus != null ? req.OrderStatus.ToString() : " ";

//                item.RequestToOrder = req.RequestToOrder != true ? false : true;
//                item.Fund = req.Fund != 0 ? req.Fund.ToString() : lstFund.Find(fund => fund.Fund.Equals("F02")).ID.ToString();

//                if (req.RequiredDate != null)
//                {
//                    item.RequiredDate = DateTime.Parse(req.RequiredDate).Date;
//                }
//                if (req.RequestOrderDate != null)
//                {
//                    item.RequestOrderDate = DateTime.Parse(req.RequestOrderDate).Date;
//                }
//                if (req.OrderDate != null)
//                {
//                    item.OrderDate = DateTime.Parse(req.OrderDate).Date;
//                }
//                if (req.TentativeDeliveryDate != null)
//                {
//                    item.TentativeDeliveryDate = DateTime.Parse(req.TentativeDeliveryDate).Date;
//                }
//                if (req.ActualDeliveryDate != null)
//                {
//                    item.ActualDeliveryDate = DateTime.Parse(req.ActualDeliveryDate).Date;
//                }

//                item.BudgetCode = db.ItemsCostList_Table.AsNoTracking().FirstOrDefault(x => x.S_No == req.Item_Name).BudgetCode.ToString();
//                item.isProjected = req.isProjected;
//                item.Q1 = req.Q1;
//                item.Q2 = req.Q2;
//                item.Q3 = req.Q3;
//                item.Q4 = req.Q4;
//                item.Projected_Amount = req.Projected_Amount;
//                item.Unused_Amount = req.Unused_Amount;
//                item.RequestorNTID = req.RequestorNTID;
//                item.L2_Remarks = req.L2_Remarks;
//                item.L3_Remarks = req.L3_Remarks;
//                item.L2_Qty = req.L2_Qty;
//                item.Is_UnplannedF02Item = req.Is_UnplannedF02Item;



//                if (req.RequestID == 0)
//                {
//                    if (item.Fund == lstFund.Find(fund => fund.Fund.Equals("F02")).ID.ToString())
//                    {
//                        viewList = GetData1(useryear);
//                        return Json(new { success = false, data = viewList, message = "Cannot add F02 items right now since Request Window has been closed." + "\n" + " Only F01/F03 items can be added at this stage!" }, JsonRequestBehavior.AllowGet);
//                    }
//                    else
//                    {
//                        db.RequestItems_Table.Add(item);
//                        db.SaveChanges();


//                        //viewList = GetData1(useryear);


//                        return Json(new { success = true,/*, data = viewList,*/ message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
//                    }

//                }
//                else
//                {


//                    db.Entry(item).State = EntityState.Modified;

//                    db.SaveChanges();

//                    //viewList = GetData1(useryear);

//                    return Json(new { success = true, message = "Updated Successfully" }, JsonRequestBehavior.AllowGet);
//                }
//            }


//        }




//        /// <summary>
//        /// function to move the item to L2 review
//        /// </summary>
//        /// <param name="id">Item ID</param>
//        /// <param name="useryear"></param>
//        /// <returns></returns>
//        [HttpPost]
//        public ActionResult LabAdminApprove(int id, string useryear)//yes
//        {

//            Emailnotify_OrderStage emailnotify = new Emailnotify_OrderStage();
//            bool is_MailTrigger = false;
//            // Nullable<bool> isRequestToOrder = false;
//            int isRequestToOrder = 0;
//            DateTime dateTime = new DateTime();
//            List<RequestItemsRepoView> viewList = new List<RequestItemsRepoView>();

//            using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

//            //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

//            {
//                System.Security.Principal.IPrincipal User = System.Web.HttpContext.Current.User;
//                string presentUserName = lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).EmployeeName;

//                if (id == 1999999999)
//                {
//                    List<RequestItems_Table> templist = new List<RequestItems_Table>().FindAll(y => y.VKM_Year == int.Parse(useryear) + 1).FindAll(items => items.ApprovedSH == true);
//                    templist = db.RequestItems_Table.ToList();
//                    Nullable<decimal> totalAmount = 0.0M;
//                    string RequestNT = "";




//                    int totalCount = templist.FindAll(x => x.RequestorNT == presentUserName).FindAll(item => item.RequestToOrder == false).FindAll(items => items.RequiredDate != null).Count;

//                    foreach (RequestItems_Table item in templist.FindAll(x => x.RequestorNT == presentUserName).FindAll(items => items.RequestToOrder == false).FindAll(items => items.RequiredDate != null).FindAll(items => items.RequestDate.ToString().Contains(DateTime.Now.Year.ToString())))
//                    {

//                        RequestItems_Table changeItem = db.RequestItems_Table.Where(x => x.RequestID == item.RequestID).FirstOrDefault<RequestItems_Table>();
//                        totalAmount += item.ApprCost;
//                        RequestNT = item.RequestorNT;
//                        var isRequestToOrder_flag = item.RequestToOrder;  //request to order flag state at the time of Request to Order Trigger
//                                                                          //if (isRequestToOrder_flag == false)
//                                                                          //{
//                        isRequestToOrder = 1; //Always from Requestor->LAbteam
//                        //}
//                        //if (isRequestToOrder_flag == true)
//                        //{
//                        //    isRequestToOrder = 2;
//                        //}
//                        is_MailTrigger = true;
//                        dateTime = (DateTime)item.SubmitDate;

//                        changeItem.RequestToOrder = true;
//                        changeItem.RequestOrderDate = DateTime.Now.Date;
//                        item.OrderStatus = lstOrderStatus.Find(status => status.OrderStatus.Equals("Open")).ID.ToString();



//                        db.Entry(changeItem).State = EntityState.Modified;
//                        db.SaveChanges();


//                    }
//                    emailnotify.RequestID_orderemail = id;
//                    emailnotify.is_RequesttoOrder = isRequestToOrder;
//                    emailnotify.NTID_ccEmail = RequestNT;
//                    emailnotify.TotalAmount = totalAmount;
//                    emailnotify.Count = totalCount;
//                    emailnotify.SubmitDate_ofRequest = dateTime;

//                    return Json(new { data = emailnotify, is_MailTrigger, success = true, message = totalCount.ToString() + " Request to Order sent for the Item(s) Successfully" }, JsonRequestBehavior.AllowGet);

//                }
//                else
//                {


//                    RequestItems_Table item = db.RequestItems_Table.Where(x => x.RequestID == id).FirstOrDefault<RequestItems_Table>();

//                    if (item.RequiredDate != null)
//                    {
//                        var isRequestToOrder_flag = item.RequestToOrder;  //request to order flag state at the time of Request to Order Trigger
//                        if (isRequestToOrder_flag == false)
//                        {
//                            isRequestToOrder = 1;
//                        }
//                        if (isRequestToOrder_flag == true)
//                        {
//                            isRequestToOrder = 2;
//                        }
//                        is_MailTrigger = true;

//                        item.RequestToOrder = true;
//                        item.RequestOrderDate = DateTime.Now.Date;
//                        item.OrderStatus = lstOrderStatus.Find(status => status.OrderStatus.Equals("Open")).ID.ToString();

//                        emailnotify.is_RequesttoOrder = isRequestToOrder;
//                        emailnotify.RequestID_orderemail = item.RequestID;


//                        db.Entry(item).State = EntityState.Modified;
//                        db.SaveChanges();

//                        return Json(new { data = emailnotify, is_MailTrigger, success = true, message = "Request to Order sent Successfully" }, JsonRequestBehavior.AllowGet);
//                    }
//                    else
//                        return Json(new { success = false, message = "Please fill in the Required Date!" }, JsonRequestBehavior.AllowGet);


//                }


//            }



//        }


//        /// <summary>
//        /// function to delete an existing item
//        /// </summary>
//        /// <param name="id"></param>
//        /// <param name="useryear"></param>
//        /// <returns></returns>
//        [HttpPost]
//        public ActionResult Delete(int id, string useryear/*string[] oem*/)
//        {
//            List<RequestItemsRepoView> viewList = new List<RequestItemsRepoView>();


//            using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

//            //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

//            {
//                RequestItems_Table item = db.RequestItems_Table.Where(x => x.RequestID == id).FirstOrDefault<RequestItems_Table>();
//                db.RequestItems_Table.Remove(item);
//                db.SaveChanges();
//                viewList = GetData1(useryear);
//                return Json(new { data = viewList, success = true, message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
//            }
//        }


//        /// <summary>
//        /// Function to help the export to excel function 
//        /// Path to export - default or input from User
//        /// Feedback to user after saving
//        /// </summary>
//        public ActionResult ExportDataToExcel(string useryear)
//        {

//            string filename = @"Order_List_" + (int.Parse(useryear) + 1) + ".xlsx";

//            using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

//            //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

//            {
//                System.Security.Principal.IPrincipal User = System.Web.HttpContext.Current.User;
//                string presentUserName = lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).EmployeeName;
//                //string userdept_inorderreqtable = db.OrderingRequestor_Table.ToList().Find(person => person.NTID.ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).Department.Trim();

//                //string presentUserDept = lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).Department;

//                System.Data.DataTable dt = new System.Data.DataTable("Request_List");
//                dt.Columns.AddRange(new DataColumn[30] { new DataColumn("Business Unit"),
//                                            new DataColumn("OEM"),
//                                            new DataColumn("Department"),
//                                            new DataColumn("Group"),
//                                            new DataColumn("Item Name"),
//                                            new DataColumn("Category"),
//                                            new DataColumn("Cost Element"),
//                                            new DataColumn("Budget Code"),
//                                            new DataColumn("Unit Price",typeof(decimal)),
//                                            new DataColumn("Required Quantity",typeof(int)),
//                                            new DataColumn("Total Price",typeof(decimal)),
//                                            new DataColumn("Reviewed Quantity",typeof(Int32)),
//                                            new DataColumn("Reviewed Price",typeof(decimal)),
//                                            new DataColumn("Comments"),
//                                            new DataColumn("Requestor"),
//                                            new DataColumn("Submit Date"),
//                                            new DataColumn("Reviewer 1"),
//                                            new DataColumn("Review 1 Date"),
//                                            new DataColumn("Reviewer 2"),
//                                            new DataColumn("Review 2 Date"),
//                                            new DataColumn("Required Date"),
//                                            new DataColumn("Request Order Date"),
//                                            new DataColumn("Order Date"),
//                                            new DataColumn("Tentative Delivery Date"),
//                                            new DataColumn("Actual Delivery Date"),
//                                            new DataColumn("Fund"),
//                                            new DataColumn("Order ID"),
//                                            new DataColumn("Ordered Quantity",typeof(Int32)),
//                                            new DataColumn("Order Price",typeof(decimal)),
//                                            new DataColumn("Order Status")});
//                // string PresentUserDept_RequestTable = lstDEPTs.Find(dept => dept.DEPT.Equals(userdept_inorderreqtable)).ID.ToString();
//                var requests1 = db.RequestItems_Table.ToList().FindAll(y => y.VKM_Year == int.Parse(useryear) + 1).Select(x => new
//                {
//                    x.BU,
//                    x.OEM,
//                    x.DEPT,
//                    x.Group,
//                    x.ItemName,
//                    x.Category,
//                    x.CostElement,
//                    x.BudgetCode,
//                    x.UnitPrice, /*x.Currency,*/
//                    x.ReqQuantity,
//                    x.TotalPrice,
//                    x.ApprQuantity,
//                    x.ApprCost,
//                    x.Comments,
//                    x.RequestorNT,
//                    x.SubmitDate,
//                    x.DHNT,
//                    x.DHAppDate,
//                    x.SHNT,
//                    x.SHAppDate,
//                    x.RequiredDate,
//                    x.RequestOrderDate,
//                    x.OrderDate,
//                    x.TentativeDeliveryDate,
//                    x.ActualDeliveryDate,
//                    x.OrderID,
//                    x.OrderedQuantity,
//                    x.OrderPrice,
//                    x.OrderStatus,
//                    x.Fund,
//                    x.ApprovedDH,
//                    x.ApprovedSH,

//                }).ToList().FindAll(x => x.ApprovedSH == true);
//                if (db.OrderingRequestor_Table.ToList().Find(person => person.NTID.ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())) != null)
//                {
//                    if (db.OrderingRequestor_Table.ToList().Find(person => person.NTID.ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).isSuperUser != null)
//                    {
//                        if (db.OrderingRequestor_Table.ToList().Find(person => person.NTID.ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).isSuperUser == true)
//                        {
//                            List<string> BUs_rfo = db.OrderingRequestor_Table.ToList().Find(person => person.NTID.ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).BU.Split(',').ToList();
//                           // var detail_superuser = viewList.FindAll(x => BUs_rfo.Contains(x.BU.ToString().Trim()));
//                            var requests = requests1.FindAll(x => BUs_rfo.Contains(x.BU.ToString().Trim())).Select(x => new
//                            {
//                                x.BU,
//                                x.OEM,
//                                x.DEPT,
//                                x.Group,
//                                x.ItemName,
//                                x.Category,
//                                x.CostElement,
//                                x.BudgetCode,
//                                x.UnitPrice, /*x.Currency,*/
//                                x.ReqQuantity,
//                                x.TotalPrice,
//                                x.ApprQuantity,
//                                x.ApprCost,
//                                x.Comments,
//                                x.RequestorNT,
//                                x.SubmitDate,
//                                x.DHNT,
//                                x.DHAppDate,
//                                x.SHNT,
//                                x.SHAppDate,
//                                x.RequiredDate,
//                                x.RequestOrderDate,
//                                x.OrderDate,
//                                x.TentativeDeliveryDate,
//                                x.ActualDeliveryDate,
//                                x.OrderID,
//                                x.OrderedQuantity,
//                                x.OrderPrice,
//                                x.OrderStatus,
//                                x.Fund
//                            }).ToList();

//                            foreach (var request in requests)
//                            {

//                                dt.Rows.Add(
//                                    lstBUs.Find(bu => bu.ID.ToString().Equals(request.BU)).BU,
//                                    lstOEMs.Find(oem => oem.ID.ToString().Equals(request.OEM)).OEM,
//                                    lstDEPTs.Find(dept => dept.ID.ToString().Equals(request.DEPT)).DEPT,
//                                    lstGroups_test.Find(grp => grp.ID.ToString().Equals(request.Group)).Group,
//                                    request.ItemName != null && request.ItemName != "0" && request.ItemName != "" ? lstItems.Find(item => item.S_No.ToString().Equals(request.ItemName)).Item_Name : "",
//                                    request.Category != null && request.Category != "0" && request.Category != "" ? lstPrdCateg.Find(cat => cat.ID.ToString().Equals(request.Category)).Category.Trim() : "",
//                                request.CostElement != null && request.CostElement != "0" ? lstCostElement.Find(cost => cost.ID.ToString().Equals(request.CostElement)).CostElement : "",
//                                request.BudgetCode, 
//                                request.UnitPrice,
//                                    request.ReqQuantity,
//                                    request.TotalPrice,
//                                    request.ApprQuantity,
//                                    request.ApprCost,
//                                    request.Comments,
//                                    request.RequestorNT,
//                                    request.SubmitDate.HasValue ? request.SubmitDate.Value.ToString("dd-MM-yyyy") : "",
//                                    request.DHNT, request.DHAppDate.HasValue ? request.DHAppDate.Value.ToString("dd-MM-yyyy") : "",
//                                    request.SHNT, request.SHAppDate.HasValue ? request.SHAppDate.Value.ToString("dd-MM-yyyy") : "",
//                                    request.RequiredDate.HasValue ? request.RequiredDate.Value.ToString("dd-MM-yyyy") : "",
//                                    request.RequestOrderDate.HasValue ? request.RequestOrderDate.Value.ToString("dd-MM-yyyy") : "",
//                                    request.OrderDate.HasValue ? request.OrderDate.Value.ToString("dd-MM-yyyy") : "",
//                                    request.TentativeDeliveryDate.HasValue ? request.TentativeDeliveryDate.Value.ToString("dd-MM-yyyy") : "",
//                                    request.ActualDeliveryDate.HasValue ? request.ActualDeliveryDate.Value.ToString("dd-MM-yyyy") : "",
//                                     request.Fund != null ? lstFund.Find(fun => fun.ID.ToString().Equals(request.Fund)).Fund : lstFund.Find(fun => fun.Fund.Equals("F02")).Fund,
//                                     request.OrderID,
//                                    request.OrderedQuantity != null ? (int)request.OrderedQuantity : (int)0,
//                                    request.OrderPrice,
//                                    (request.OrderStatus != null && request.OrderStatus.Trim() != "" && request.OrderStatus.Trim() != "0") ? lstOrderStatus.Find(status => status.ID.ToString().Equals(request.OrderStatus.Trim())).OrderStatus : ""
//                                    );
//                            }
//                        }

//                    }
//                    else
//                    {
//                        var DEPT = db.OrderingRequestor_Table.ToList().Find(person => person.NTID.ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).Department.Trim();
//                        var requests = requests1.FindAll(dpt => dpt.DEPT.ToString().Trim().Equals(DEPT)).Select(x => new
//                        {
//                            x.BU,
//                            x.OEM,
//                            x.DEPT,
//                            x.Group,
//                            x.ItemName,
//                            x.Category,
//                            x.CostElement,
//                            x.BudgetCode,
//                            x.UnitPrice, /*x.Currency,*/
//                            x.ReqQuantity,
//                            x.TotalPrice,
//                            x.ApprQuantity,
//                            x.ApprCost,
//                            x.Comments,
//                            x.RequestorNT,
//                            x.SubmitDate,
//                            x.DHNT,
//                            x.DHAppDate,
//                            x.SHNT,
//                            x.SHAppDate,
//                            x.RequiredDate,
//                            x.RequestOrderDate,
//                            x.OrderDate,
//                            x.TentativeDeliveryDate,
//                            x.ActualDeliveryDate,
//                            x.OrderID,
//                            x.OrderedQuantity,
//                            x.OrderPrice,
//                            x.OrderStatus,
//                            x.Fund
//                        }).ToList();

//                        foreach (var request in requests)
//                        {
//                            try
//                            {
//                                dt.Rows.Add(
//                                    lstBUs.Find(bu => bu.ID.ToString().Equals(request.BU)).BU,
//                                    lstOEMs.Find(oem => oem.ID.ToString().Equals(request.OEM)).OEM,
//                                    lstDEPTs.Find(dept => dept.ID.ToString().Equals(request.DEPT)).DEPT,
//                                    lstGroups_test.Find(grp => grp.ID.ToString().Equals(request.Group)).Group,
//                                     request.ItemName != null && request.ItemName != "0" && request.ItemName != "" ? lstItems.Find(item => item.S_No.ToString().Equals(request.ItemName)).Item_Name : "",
//                                    request.Category != null && request.Category != "0" ? lstPrdCateg.Find(cat => cat.ID.ToString().Equals(request.Category)).Category.Trim() : "",
//                                    request.CostElement != null && request.CostElement != "0" ? lstCostElement.Find(cost => cost.ID.ToString().Equals(request.CostElement)).CostElement : "",
//                                    request.BudgetCode,
//                                    request.UnitPrice,
//                                    request.ReqQuantity,
//                                    request.TotalPrice,
//                                    request.ApprQuantity,
//                                    request.ApprCost,
//                                    request.Comments,
//                                    request.RequestorNT,
//                                    request.SubmitDate.HasValue ? request.SubmitDate.Value.ToString("dd-MM-yyyy") : "",
//                                    request.DHNT, request.DHAppDate.HasValue ? request.DHAppDate.Value.ToString("dd-MM-yyyy") : "",
//                                    request.SHNT, request.SHAppDate.HasValue ? request.SHAppDate.Value.ToString("dd-MM-yyyy") : "",
//                                    request.RequiredDate.HasValue ? request.RequiredDate.Value.ToString("dd-MM-yyyy") : "",
//                                    request.RequestOrderDate.HasValue ? request.RequestOrderDate.Value.ToString("dd-MM-yyyy") : "",
//                                    request.OrderDate.HasValue ? request.OrderDate.Value.ToString("dd-MM-yyyy") : "",
//                                    request.TentativeDeliveryDate.HasValue ? request.TentativeDeliveryDate.Value.ToString("dd-MM-yyyy") : "",
//                                    request.ActualDeliveryDate.HasValue ? request.ActualDeliveryDate.Value.ToString("dd-MM-yyyy") : "",
//                                     request.Fund != null ? lstFund.Find(fun => fun.ID.ToString().Equals(request.Fund)).Fund : lstFund.Find(fun => fun.Fund.Equals("F02")).Fund,
//                                     request.OrderID,
//                                    request.OrderedQuantity != null ? (int)request.OrderedQuantity : (int)0,
//                                    request.OrderPrice,
//                                    (request.OrderStatus != null && request.OrderStatus.Trim() != "" && request.OrderStatus.Trim() != "0") ? lstOrderStatus.Find(status => status.ID.ToString().Equals(request.OrderStatus.Trim())).OrderStatus : ""
//                                    );
//                            }
//                            catch(Exception ex)
//                            {

//                            }
//                        }

//                    }

//                    // return Json(new { success = true, data = detail, isDashboard_flag }, JsonRequestBehavior.AllowGet);
//                }
//                else
//                {
//                    var spoton_dept = db.SPOTONData_Table_2022.ToList().Find(person => person.NTID.ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).Department.Trim();
//                    string dept_id = lstDEPTs.Find(dpt => dpt.DEPT == spoton_dept).ID.ToString();
//                    var requests = requests1.FindAll(dpt => dpt.DEPT.ToString().Trim().Equals(dept_id)).Select(x => new
//                    {
//                        x.BU,
//                        x.OEM,
//                        x.DEPT,
//                        x.Group,
//                        x.ItemName,
//                        x.Category,
//                        x.CostElement,
//                        x.BudgetCode,
//                        x.UnitPrice, /*x.Currency,*/
//                        x.ReqQuantity,
//                        x.TotalPrice,
//                        x.ApprQuantity,
//                        x.ApprCost,
//                        x.Comments,
//                        x.RequestorNT,
//                        x.SubmitDate,
//                        x.DHNT,
//                        x.DHAppDate,
//                        x.SHNT,
//                        x.SHAppDate,
//                        x.RequiredDate,
//                        x.RequestOrderDate,
//                        x.OrderDate,
//                        x.TentativeDeliveryDate,
//                        x.ActualDeliveryDate,
//                        x.OrderID,
//                        x.OrderedQuantity,
//                        x.OrderPrice,
//                        x.OrderStatus,
//                        x.Fund
//                    }).ToList();

//                    foreach (var request in requests)
//                    {

//                        dt.Rows.Add(
//                            lstBUs.Find(bu => bu.ID.ToString().Equals(request.BU)).BU,
//                            lstOEMs.Find(oem => oem.ID.ToString().Equals(request.OEM)).OEM,
//                            lstDEPTs.Find(dept => dept.ID.ToString().Equals(request.DEPT)).DEPT,
//                            lstGroups_test.Find(grp => grp.ID.ToString().Equals(request.Group)).Group,
//                            lstItems.Find(item => item.S_No.ToString().Equals(request.ItemName)).Item_Name,
//                            request.Category != null && request.Category != "0" ? lstPrdCateg.Find(cat => cat.ID.ToString().Equals(request.Category)).Category.Trim() : "",
//                                request.CostElement != null && request.CostElement != "0" ? lstCostElement.Find(cost => cost.ID.ToString().Equals(request.CostElement)).CostElement : "",
//                            request.BudgetCode,
//                            request.UnitPrice,
//                            request.ReqQuantity,
//                            request.TotalPrice,
//                            request.ApprQuantity,
//                            request.ApprCost,
//                            request.Comments,
//                            request.RequestorNT,
//                            request.SubmitDate.HasValue ? request.SubmitDate.Value.ToString("dd-MM-yyyy") : "",
//                            request.DHNT, request.DHAppDate.HasValue ? request.DHAppDate.Value.ToString("dd-MM-yyyy") : "",
//                            request.SHNT, request.SHAppDate.HasValue ? request.SHAppDate.Value.ToString("dd-MM-yyyy") : "",
//                            request.RequiredDate.HasValue ? request.RequiredDate.Value.ToString("dd-MM-yyyy") : "",
//                            request.RequestOrderDate.HasValue ? request.RequestOrderDate.Value.ToString("dd-MM-yyyy") : "",
//                            request.OrderDate.HasValue ? request.OrderDate.Value.ToString("dd-MM-yyyy") : "",
//                            request.TentativeDeliveryDate.HasValue ? request.TentativeDeliveryDate.Value.ToString("dd-MM-yyyy") : "",
//                            request.ActualDeliveryDate.HasValue ? request.ActualDeliveryDate.Value.ToString("dd-MM-yyyy") : "",
//                             request.Fund != null ? lstFund.Find(fun => fun.ID.ToString().Equals(request.Fund)).Fund : lstFund.Find(fun => fun.Fund.Equals("F02")).Fund,
//                             request.OrderID,
//                            request.OrderedQuantity != null ? (int)request.OrderedQuantity : (int)0,
//                            request.OrderPrice,
//                            (request.OrderStatus != null && request.OrderStatus.Trim() != "" && request.OrderStatus.Trim() != "0") ? lstOrderStatus.Find(status => status.ID.ToString().Equals(request.OrderStatus.Trim())).OrderStatus : ""
//                            );
//                    }
//                }



//                using (XLWorkbook wb = new XLWorkbook())
//                {
//                    wb.Worksheets.Add(dt);
//                    using (MemoryStream stream = new MemoryStream())
//                    {
//                        wb.SaveAs(stream);
//                        var robj = File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
//                        return Json(robj, JsonRequestBehavior.AllowGet);
//                    }
//                }

//            }

//        }







//        ///// <summary>
//        ///// function to get the Initial values to be filled automatically when a new request is created
//        ///// </summary>
//        ///// <returns></returns>
//        //[HttpGet]
//        //public ActionResult InitRowValues()
//        //{

//        //    using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

//        //    //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

//        //    {

//        //        RequestItemsRepoEdit1 temp = new RequestItemsRepoEdit1();

//        //        System.Security.Principal.IPrincipal User = System.Web.HttpContext.Current.User;
//        //        SPOTONData_Table_2021 PresentUser = lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper()));
//        //        string presentUserName = lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).EmployeeName;
//        //        string L2ReviewerName = string.Empty;
//        //        string L3ReviewerName = string.Empty;
//        //        try
//        //        {
//        //            L2ReviewerName = lstUsers.FindAll(user => PresentUser.Department.ToUpper().Equals(user.Group.ToUpper())).FirstOrDefault(head => head.Role.ToUpper().Contains("HEAD")).EmployeeName;
//        //        }
//        //        catch (Exception ex)
//        //        {
//        //            L2ReviewerName = "NA";
//        //        }
//        //        try
//        //        {
//        //            L3ReviewerName = lstUsers.FindAll(user => PresentUser.Section.ToUpper().Contains(user.Section.ToUpper())).FirstOrDefault(head => head.Role.ToUpper().Contains("SECTION")).EmployeeName;
//        //        }
//        //        catch (Exception ex)
//        //        {
//        //            L3ReviewerName = "NA";
//        //        }

//        //        temp.Requestor = presentUserName;
//        //        temp.Reviewer_1 = L2ReviewerName;
//        //        temp.Reviewer_2 = L3ReviewerName;
//        //        DEPT_Table dEPT_Table = lstDEPTs.Find(departmnt => departmnt.DEPT.Equals(lstUsers.Find(x => x.EmployeeName == presentUserName).Department));
//        //        temp.DEPT = dEPT_Table.ID;


//        //        var PresentUserGroup = lstUsers.Find(x => x.EmployeeName == presentUserName).Group;
//        //        Groups_Table gROUP_Table = lstGroups.Find(grp => grp.Group.Trim().Equals(PresentUserGroup));
//        //        temp.Group = gROUP_Table.ID;

//        //        temp.RequestDate = DateTime.Now.ToString("yyyy-MM-dd");
//        //        return Json(new { data = temp }, JsonRequestBehavior.AllowGet);
//        //    }

//        //}


//        /// <summary>
//        /// function to get L3 reviewer name based on BU
//        /// </summary>
//        /// <param name="BU"></param>
//        /// <returns></returns>
//        [HttpPost]
//        public ActionResult GetReviewer(int BU)
//        {

//            string SHNT = string.Empty;
//            var x = lstBU_SPOCs;
//            SHNT = lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(lstBU_SPOCs.Find(spoc => spoc.BU.Equals(BU)).VKMspoc.ToUpper().Trim())).EmployeeName;

//            return Json(SHNT, JsonRequestBehavior.AllowGet);
//        }




//        /// <summary>
//        /// function to get the unit price of selected Item
//        /// </summary>
//        /// <param name="ItemName"></param>
//        /// <returns></returns>
//        [HttpPost]
//        public ActionResult GetUnitPrice(int ItemName)
//        {
//            return Json(lstItems.Find(x => x.S_No == ItemName).UnitPriceUSD, JsonRequestBehavior.AllowGet);

//        }




//        /// <summary>
//        /// function to get category of selected Item
//        /// </summary>
//        /// <param name="ItemName"></param>
//        /// <returns></returns>
//        [HttpPost]
//        public ActionResult GetCategory(int ItemName)
//        {

//            int Catitem = int.Parse(lstItems.Find(x => x.S_No == ItemName).Category);

//            return Json(Catitem, JsonRequestBehavior.AllowGet);
//        }








//        /// <summary>
//        /// function to get the cost elements of selected Item
//        /// </summary>
//        /// <param name="ItemName"></param>
//        /// <returns></returns>
//        [HttpPost]
//        public ActionResult GetCostElement(int ItemName)
//        {

//            int Costeltitem = int.Parse(lstItems.Find(x => x.S_No == ItemName).Cost_Element);

//            return Json(Costeltitem, JsonRequestBehavior.AllowGet);
//        }


//        /// <summary>
//        /// function to get category of selected Item
//        /// </summary>
//        /// <param name="ItemName"></param>
//        /// <returns></returns>
//        [HttpPost]
//        public ActionResult GetBudgetCode(int ItemName)
//        {
//            try
//            {
//                string BudgetCode = "";
//                connection();
//                string Query = " Select Isnull(BudgetCode,'') as BudgetCode from ItemsCostList_Table where S#No = " + ItemName + " ";
//                OpenConnection();
//                SqlCommand cmd = new SqlCommand(Query, conn);
//                SqlDataReader dr = cmd.ExecuteReader();
//                if (dr.HasRows)
//                {
//                    dr.Read();
//                    BudgetCode = dr["BudgetCode"].ToString();

//                }
//                dr.Close();
//                CloseConnection();

//                return Json(BudgetCode, JsonRequestBehavior.AllowGet);
//            }
//            catch (Exception ex)
//            {
//                //WriteLog("Error - GetBudgetCode : NTID - " + User.Identity.Name.Split('\\')[1].Trim().ToUpper() + ", ErrorMsg : " + ex + ", At - " + DateTime.Now);
//                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
//            }
//        }

//        /// <summary>
//        /// function to get the Lead Time of selected Item
//        /// </summary>
//        /// <param name="ItemName"></param>
//        /// <returns></returns>
//        [HttpGet]
//        public ActionResult GetLeadTime(int ItemName)
//        {
//            int leadtime_vendor;
//            if (lstItems.Find(x => x.S_No == ItemName).VendorCategory != null && lstItems.Find(x => x.S_No == ItemName).VendorCategory != string.Empty)
//            {
//                int vendor_item = int.Parse(lstItems.Find(x => x.S_No == ItemName).VendorCategory);

//                if ((lstVendor.Find(x => x.ID == vendor_item).LeadTime) != null)
//                    leadtime_vendor = ((int)lstVendor.Find(x => x.ID == vendor_item).LeadTime);
//                else
//                    leadtime_vendor = 0;


//            }
//            else
//                leadtime_vendor = 0;

//            return Json(leadtime_vendor, JsonRequestBehavior.AllowGet);
//        }




//        ////ValidateRequiredDate

//        /// <summary>
//        /// function to validate the Required Date of selected Item
//        /// </summary>
//        /// <param name="LeadTime"></param>
//        /// <returns></returns>
//        [HttpGet]
//        public ActionResult ValidateRequiredDate(/*int LeadTime, */DateTime RequiredDate)
//        {
//            //var ExpectedReqdDate = DateTime.Now.AddDays(LeadTime - 1).ToShortDateString();
//            var RequiredDt = RequiredDate.ToShortDateString();
//            //var ExpectedReqdDate1 = DateTime.Parse(ExpectedReqdDate);
//            var RequiredDt1 = DateTime.Parse(RequiredDt);
//            var PresentDt = DateTime.Now.ToShortDateString();
//            var PresentDt1 = DateTime.Parse(PresentDt);
//            string error = String.Empty;
//            if (RequiredDt1 < PresentDt1)
//            {
//                error = "The Required by Date cannot be less than Today's Date. Please choose appropriate date!";
//            }
//            return Json(error, JsonRequestBehavior.AllowGet);
//        }



//        [HttpPost]
//        public ActionResult GetPODetails()
//        {
//            DataTable dt = new DataTable();
//            try
//            {

//                connection();

//                string Query = " Select  [ID]" +
//          ",[RequestID]" +
//          ",[VKMYear]" +
//          ",[BU] " +
//          ",[OEM]" +
//          ",[GROUP]" +
//          ",[Dept]" +
//          ",[Item Name]" +
//          ",[Ordered Quantity]" +
//          ",[PO Number]" +
//          ",[PIF ID]" +
//          ",[Fund]" +
//          ",[Fund Center]" +
//          ",BudgetCode" +
//          ",ItemDescription" +
//          ",[PO Quantity]" +
//          ",[UOM]" +
//          ",[UnitPrice]" +
//          ",[Netvalue]" +
//          ",[Netvalue_USD]" +
//          ",[Currency]" +
//          ",[Plant]" +
//          ",[PO Created On]" +
//          ",VendorName" +
//          ",[CW]" +
//          ",[Tentative Delivery Date]" +
//          ",[Actual Delivery Date]" +
//          ",[Difference in DeliveRy Date]" +
//          ",[Actal Amt]" +
//          ",[Negotiated Amt]" +
//          ",[Savings]" +
//          ",[Current status]" +
//          ",[PO Remarks]" +
//          "from [PODetails_Table] where VKMYear is not null order by ID";

//                OpenConnection();
//                SqlCommand cmd = new SqlCommand(Query, conn);
//                SqlDataAdapter da = new SqlDataAdapter(cmd);
//                da.Fill(dt);
//                CloseConnection();
//            }
//            catch (Exception ex)
//            {

//            }
//            List<PO_Data> result = new List<PO_Data>();

//            foreach (DataRow row in dt.Rows)
//            {
//                PO_Data item = new PO_Data();
//                //item.ProjectChange = Convert.ToDateTime(row["ProjectChange"]).ToString("yyyy-MM-dd");
//                //if (row["ProjectChange"].ToString() == "1900-01-01")
//                //{
//                //    item.ProjectChange = "";
//                //}
//                //else
//                //{
//                //    item.ProjectChange = row["ProjectChange"].ToString();
//                //}
//                item.ID = int.Parse(row["ID"].ToString());
//                item.RequestID = int.Parse(row["RequestID"].ToString());
//                item.VKMYear = row["VKMYear"].ToString();
//                item.BU = row["BU"].ToString();
//                item.OEM = row["OEM"].ToString();
//                item.GROUP = row["GROUP"].ToString();
//                item.Dept = row["Dept"].ToString();
//                item.ItemName = row["Item Name"].ToString();
//                item.OrderedQuantity = row["Ordered Quantity"].ToString();
//                item.PONumber = row["PO Number"].ToString();
//                item.PIFID = row["PIF ID"].ToString();
//                item.Fund = row["Fund"].ToString();
//                item.FundCenter = row["Fund Center"].ToString();
//                item.BudgetCode = row["BudgetCode"].ToString();
//                item.ItemDescription = row["ItemDescription"].ToString();
//                item.POQuantity = int.Parse(row["PO Quantity"].ToString());
//                item.UOM = row["UOM"].ToString();
//                item.UnitPrice = row["UnitPrice"].ToString();
//                item.Netvalue = row["Netvalue"].ToString();
//                item.Netvalue_USD = row["Netvalue_USD"].ToString(); ;
//                item.Currency = row["Currency"].ToString(); ;
//                item.Plant = row["Plant"].ToString();
//                item.POCreatedOn = row["PO Created On"].ToString();
//                item.VendorName = row["VendorName"].ToString();
//                item.CW = row["CW"].ToString();
//                item.TentativeDeliveryDate = row["Tentative Delivery Date"].ToString();
//                item.ActualDeliveryDate = row["Actual Delivery Date"].ToString();
//                item.DifferenceinDeliveRyDate = row["Difference in DeliveRy Date"].ToString();
//                item.ActalAmt = row["Actal Amt"].ToString();
//                item.NegotiatedAmt = row["Negotiated Amt"].ToString();
//                item.Savings = row["Savings"].ToString();
//                item.Currentstatus = row["Current status"].ToString();
//                item.PORemarks = row["PO Remarks"].ToString();

//                result.Add(item);
//            }


//            return Json(new { data = result, success = true, JsonRequestBehavior.AllowGet });
//            //}

//        }





//        [HttpGet]
//        public ActionResult Lookup(string year)
//        {
//            LookupData lookupData = new LookupData();

//            lookupData.BU_List = lstBUs.OrderBy(x => x.ID).ToList();
//            if (year.Contains("2020"))
//            {
//                lookupData.BU_List[2].BU = "AS";
//                lookupData.BU_List[4].BU = "PS";
//            }
//            else
//            {
//                lookupData.BU_List[2].BU = "MB";
//                lookupData.BU_List[4].BU = "OSS";
//            }
//            //lookupData.BU_List = lstBUs;
//            lookupData.OEM_List = lstOEMs;
//            lookupData.DEPT_List = lstDEPTs.FindAll(x => x.Outdated == false);
//            lookupData.Groups_test = lstGroups_test.FindAll(x => x.Outdated == false);
//            //lookupData.Groups_List = lstGroups/*.FindAll(x => x.Outdated == false)*/;
//            lookupData.Item_List = lstItems/*.FindAll(item=>item.Deleted !=true)*/;
//            lookupData.Category_List = lstPrdCateg;
//            lookupData.CostElement_List = lstCostElement;
//            lookupData.OrderStatus_List = lstOrderStatus;
//            lookupData.VendorCategory_List = lstVendor;
//            lookupData.Fund_List = lstFund;

//            lookupData.BudgetCodeList = BudgetCodeList;


//            DataSet dt_for_headerRow = new DataSet();
//            connection();
//            OpenConnection();
//            string Query = " Exec [dbo].[GetReqItemsList_RFO_View] '" + (int.Parse(year) + 1) + "', '" + User.Identity.Name.Split('\\')[1].ToUpper() + "', '" + "LookUp" + "' ";
//            SqlCommand cmd = new SqlCommand(Query, conn);
//            SqlDataAdapter da = new SqlDataAdapter(cmd);
//            da.Fill(dt_for_headerRow);
//            CloseConnection();
//            lookupData.Item_HeaderFilter = new List<HeaderFilter_Table>();
//            lookupData.BU_HeaderFilter = new List<HeaderFilter_Table>();
//            lookupData.DEPT_HeaderFilter = new List<HeaderFilter_Table>();
//            lookupData.Group_HeaderFilter = new List<HeaderFilter_Table>();
//            lookupData.OEM_HeaderFilter = new List<HeaderFilter_Table>();
//            lookupData.Category_HeaderFilter = new List<HeaderFilter_Table>();
//            lookupData.CostElement_HeaderFilter = new List<HeaderFilter_Table>();
//            lookupData.OrderStatus_HeaderFilter = new List<HeaderFilter_Table>();

//            lookupData.BudgetCode_HeaderFilter = new List<HeaderFilter_Table>();

//            foreach (DataRow item in dt_for_headerRow.Tables[0].Rows)
//            {
//                HeaderFilter_Table i_header = new HeaderFilter_Table();
//                i_header.text = item["Item Name"].ToString();
//                i_header.value = Convert.ToInt32(item["S#No"].ToString());
//                lookupData.Item_HeaderFilter.Add(i_header);
//            }
//            foreach (DataRow item in dt_for_headerRow.Tables[1].Rows)
//            {
//                HeaderFilter_Table i_header = new HeaderFilter_Table();
//                i_header.text = item["BU"].ToString();
//                i_header.value = Convert.ToInt32(item["ID"].ToString());
//                if (year.Contains("2020") && i_header.text.Contains("MB"))
//                {
//                    i_header.text = "AS";

//                }
//                if (year.Contains("2020") && i_header.text.Contains("OSS"))
//                {
//                    i_header.text = "PS";

//                }
//                lookupData.BU_HeaderFilter.Add(i_header);
//            }
//            foreach (DataRow item in dt_for_headerRow.Tables[2].Rows)
//            {
//                HeaderFilter_Table i_header = new HeaderFilter_Table();
//                i_header.text = item["DEPT"].ToString();
//                i_header.value = Convert.ToInt32(item["ID"].ToString());
//                lookupData.DEPT_HeaderFilter.Add(i_header);
//            }
//            foreach (DataRow item in dt_for_headerRow.Tables[3].Rows)
//            {
//                HeaderFilter_Table i_header = new HeaderFilter_Table();
//                i_header.text = item["Group"].ToString();
//                i_header.value = Convert.ToInt32(item["ID"].ToString());
//                lookupData.Group_HeaderFilter.Add(i_header);
//            }
//            foreach (DataRow item in dt_for_headerRow.Tables[4].Rows)
//            {
//                HeaderFilter_Table i_header = new HeaderFilter_Table();
//                i_header.text = item["OEM"].ToString();
//                i_header.value = Convert.ToInt32(item["ID"].ToString());
//                lookupData.OEM_HeaderFilter.Add(i_header);
//            }

//            foreach (DataRow item in dt_for_headerRow.Tables[5].Rows)
//            {
//                HeaderFilter_Table i_header = new HeaderFilter_Table();
//                i_header.text = item["Category"].ToString();
//                i_header.value = Convert.ToInt32(item["ID"].ToString());
//                lookupData.Category_HeaderFilter.Add(i_header);
//            }
//            foreach (DataRow item in dt_for_headerRow.Tables[6].Rows)
//            {
//                HeaderFilter_Table i_header = new HeaderFilter_Table();
//                i_header.text = item["CostElement"].ToString();
//                i_header.value = Convert.ToInt32(item["ID"].ToString());
//                lookupData.CostElement_HeaderFilter.Add(i_header);
//            }
//            foreach (DataRow item in dt_for_headerRow.Tables[7].Rows)
//            {
//                HeaderFilter_Table i_header = new HeaderFilter_Table();
//                i_header.text = item["OrderStatus"].ToString();
//                i_header.value = Convert.ToInt32(item["ID"].ToString());
//                lookupData.OrderStatus_HeaderFilter.Add(i_header);
//            }

//            foreach (DataRow item in dt_for_headerRow.Tables[8].Rows)
//            {
//                HeaderFilter_Table i_header = new HeaderFilter_Table();
//                i_header.text = item["BudgetCode"].ToString();
//                i_header.value = Convert.ToInt32(item["BudgetCode"].ToString());
//                lookupData.BudgetCode_HeaderFilter.Add(i_header);
//            }

//            return Json(new { data = lookupData }, JsonRequestBehavior.AllowGet);

//        }

//    }

//    public class OEMList
//    {
//        public OEMList()
//        {
//            this.OEMSelectList = new List<SelectListItem>();
//        }
//        public List<SelectListItem> OEMSelectList { get; set; }
//        public string isDashboard { get; set; }

//    }






//}
