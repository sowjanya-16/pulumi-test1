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
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Configuration;
using System.Threading;
using DocumentFormat.OpenXml.InkML;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace LC_Reports_V1.Controllers
{
    //[Authorize(Users = @"apac\jov6cob,apac\rba3cob,apac\din2cob, apac\MTA2COB,apac\muu4cob,apac\nnj6kor,apac\pks5cob,apac\chb1kor,apac\sbr2kor,apac\rau2kor,apac\bbv5kor,apac\rmm7kor,apac\mae9cob,apac\oig1cob, apac\mxk8kor,apac\SIF1COB,apac\ghb1cob, apac\hti1kor, apac\GPA3KOR, apac\pbr3kor, apac\SRU1PU")]
    public class BudgetingVKMController : Controller
    {
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

        public ActionResult Index_BetaProjected()
        {
            WriteLog("*********** Budgeting VKM - Beta Projected *********" + DateTime.Now.ToString());
            BudgetingController.InitialiseBudgeting();
            if (BudgetingController.lstUsers == null || BudgetingController.lstUsers.Count == 0)
            {

                return RedirectToAction("Index", "Budgeting");
            }

            //BulkApprove("DA","ESD","ESD-ST3");
            //BulkApprove("Shivalingayya Kalmath");

            return View();
        }
        // GET: BudgetingVKM
        public ActionResult Index()
        {
            WriteLog("*********** Budgeting VKM  *********" + DateTime.Now.ToString());
            // BudgetingController.InitialiseBudgeting();
            if (BudgetingController.lstUsers == null || BudgetingController.lstUsers.Count == 0)
            {

                return RedirectToAction("Index", "Budgeting");
            }

            //BulkApprove("DA","ESD","ESD-ST3");
            //BulkApprove("Shivalingayya Kalmath");

            return View();
        }
        /// <summary>
        /// Function to export data to excel sheet - only L3 items
        /// uses ClosedXML.dll
        /// accesses other summary generating functions
        ///  /// /// <param name="year"></param>
        /// </summary>
        /// <returns></returns>
        //ROUNDING OFF MIDPOINT AVAY FROM ZERO - HERE 85.5 = 86

        public ActionResult ExportDataToExcel(string useryear)
        {

            try
            {
                var presentUserNTID = User.Identity.Name.Split('\\')[1].ToUpper();
                string filename = @"VKM " + (int.Parse(useryear) + 1).ToString() + " "+presentUserNTID +" "+"Approval_List_" + DateTime.Now.ToShortDateString() + ".xlsx";

                using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

                //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

                {
                    System.Data.DataTable dt = new System.Data.DataTable("Request_List");
                    string present = BudgetingController.lstUsers.Find(x => x.NTID.Trim().ToUpper().Equals(presentUserNTID)).EmployeeName;
                    //string presentUserName_2020 = BudgetingController.lstUsers_2020.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).EmployeeName;

                    if (useryear.Contains("2020"))
                    {
                        dt.Columns.AddRange(new DataColumn[35] { new DataColumn("Business Unit"),
                                            new DataColumn("OEM"),
                                            new DataColumn("Department"),
                                            new DataColumn("Group"),
                                            new DataColumn("Project"),
                                            new DataColumn("Item Name"),
                                            new DataColumn("Category"),
                                            new DataColumn("Cost Element"),
                                            new DataColumn("Budget Code"),
                                            new DataColumn("Unit Price", typeof(decimal)),
                                            new DataColumn("Actual Available Quantity"),
                                            new DataColumn("Required Quantity", typeof(Int32)),
                                            new DataColumn("Total Price", typeof(decimal)),
                                            new DataColumn("Reviewed Quantity", typeof(Int32)),
                                            new DataColumn("Reviewed Price", typeof(decimal)),
                                            new DataColumn("Total Budgeted Cost (USD)", typeof(decimal)),
                                            new DataColumn("Comments"),
                                            new DataColumn("Requestor"),
                                            new DataColumn("Submit Date"),
                                            new DataColumn("Reviewer L2"),
                                            new DataColumn("L2 Review Date"),
                                            new DataColumn("Reviewer L3"),
                                            new DataColumn("L3 Review Date"),
                                            new DataColumn("L2 Review Status"),
                                            new DataColumn("L3 Review Status"),
                                            new DataColumn("Required Date"),
                                            new DataColumn("Request Order Date"),
                                            new DataColumn("Order Date"),
                                            new DataColumn("Tentative Delivery Date"),
                                            new DataColumn("Actual Delivery Date"),
                                            new DataColumn("Fund"),
                                            new DataColumn("Order ID"),
                                            new DataColumn("Order Status"),
                                            new DataColumn("Ordered Quantity",typeof(Int32)),
                                            new DataColumn("Order Price",typeof(decimal))

                });
                        if (BudgetingController.lstBU_SPOCs.Find(person => person.VKMspoc.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())) != null
                            //&& BudgetingController.lstBU_SPOCs.Find(person => person.VKMspoc.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).BU < 99 
                            && User.Identity.Name.Split('\\')[1].ToUpper() != "DIN2COB")
                        {
                            var requests = db.RequestItems_Table.AsEnumerable()./*Where(x => x.SubmitDate.ToString().Contains(useryear))*/Where(y => y.VKM_Year == int.Parse(useryear) + 1).Select(x => new
                            {
                                x.BU,
                                x.OEM,
                                x.DEPT,
                                x.Group,
                                x.Project,
                                x.ItemName,
                                x.Category,
                                x.CostElement,
                                x.BudgetCode,
                                x.UnitPrice,
                                x.ActualAvailableQuantity,
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
                                x.ApprovedDH,
                                x.ApprovedSH,
                                x.RequiredDate,
                                x.RequestOrderDate,
                                x.OrderDate,
                                x.TentativeDeliveryDate,
                                x.ActualDeliveryDate,
                                x.Fund,
                                x.OrderID,
                                x.OrderStatus,
                                x.OrderedQuantity,
                                x.OrderPrice

                            }).ToList().FindAll(x => x.ApprovedDH == true).FindAll(x => x.SHNT.Trim().Equals(present.Trim()) /*|| x.SHNT.Trim().Equals(presentUserName_2020.Trim())*/);
                            foreach (var request in requests)
                            {
                                string BU = string.Empty;
                                if (BudgetingController.lstBUs.Find(bu => bu.ID.ToString().Equals(request.BU)).BU == "MB")
                                    BU = "AS";
                                else if (BudgetingController.lstBUs.Find(bu => bu.ID.ToString().Equals(request.BU)).BU == "OSS")
                                    BU = "PS";
                                else
                                    BU = BudgetingController.lstBUs.Find(bu => bu.ID.ToString().Equals(request.BU)).BU;



                                dt.Rows.Add(
                                    BU,
                                    BudgetingController.lstOEMs.Find(oem => oem.ID.ToString().Equals(request.OEM)).OEM,
                                    BudgetingController.lstDEPTs.Find(dept => dept.ID.ToString().Equals(request.DEPT)).DEPT,
                                    BudgetingController.lstGroups_test.Find(grp => grp.ID.ToString().Equals(request.Group)).Group,
                                    request.Project,
                                    BudgetingController.lstItems.Find(item => item.S_No.ToString().Equals(request.ItemName)).Item_Name,
                                    request.Category != null && request.Category != "0" ? BudgetingController.lstPrdCateg.Find(cat => cat.ID.ToString().Equals(request.Category)).Category.Trim() : "",
                                    request.CostElement != null && request.CostElement != "0" ? BudgetingController.lstCostElement.Find(cost => cost.ID.ToString().Equals(request.CostElement)).CostElement : "",
                                    request.BudgetCode,
                                    request.UnitPrice != null ? Math.Round((decimal)request.UnitPrice, MidpointRounding.AwayFromZero) : 0,
                                    request.ActualAvailableQuantity != null ? request.ActualAvailableQuantity : "NA",
                                    request.ReqQuantity != null ? (int)request.ReqQuantity : 0,
                                    request.TotalPrice != null ? Math.Round((decimal)request.TotalPrice, MidpointRounding.AwayFromZero) : 0,
                                    request.ApprQuantity != null ? (int)request.ApprQuantity : 0,
                                    request.ApprCost != null ? Math.Round((decimal)request.ApprCost, MidpointRounding.AwayFromZero) : 0,
                                    request.ApprCost != null ? Math.Round((decimal)request.ApprCost, MidpointRounding.AwayFromZero) : 0,
                                    request.Comments,
                                    request.RequestorNT,
                                    request.SubmitDate.HasValue ? request.SubmitDate.Value.ToString("dd-MM-yyyy") : "",
                                    request.DHNT,
                                    request.DHAppDate.HasValue ? request.DHAppDate.Value.ToString("dd-MM-yyyy") : "",
                                    request.SHNT,
                                    request.SHAppDate.HasValue ? request.SHAppDate.Value.ToString("dd-MM-yyyy") : "",
                                    request.ApprovedDH,
                                    request.ApprovedSH,
                                    request.RequiredDate.HasValue ? request.RequiredDate.Value.ToString("dd-MM-yyyy") : "",
                                    request.RequestOrderDate.HasValue ? request.RequestOrderDate.Value.ToString("dd-MM-yyyy") : "",
                                    request.OrderDate.HasValue ? request.OrderDate.Value.ToString("dd-MM-yyyy") : "",
                                    request.TentativeDeliveryDate.HasValue ? request.TentativeDeliveryDate.Value.ToString("dd-MM-yyyy") : "",
                                    request.ActualDeliveryDate.HasValue ? request.ActualDeliveryDate.Value.ToString("dd-MM-yyyy") : "",
                                    request.Fund != null ? BudgetingController.lstFund.Find(fun => fun.ID.ToString().Equals(request.Fund)).Fund : BudgetingController.lstFund.Find(fun => fun.Fund.Trim().Equals("F02")).Fund,
                                    request.OrderID,
                                    (request.OrderStatus != null && request.OrderStatus.Trim() != "" && request.OrderStatus.Trim() != "0") ? BudgetingController.lstOrderStatus.Find(status => status.ID.ToString().Equals(request.OrderStatus.Trim())).OrderStatus : "",
                                    request.OrderedQuantity != null ? (int)request.OrderedQuantity : (int)0,
                                    request.OrderPrice);


                            }
                        }
                        else
                        {
                            var requests = db.RequestItems_Table.AsEnumerable()./*Where(x => x.SubmitDate.ToString().Contains(useryear))*/Where(y => y.VKM_Year == int.Parse(useryear) + 1).Select(x => new
                            {
                                x.BU,
                                x.OEM,
                                x.DEPT,
                                x.Group,
                                x.Project,
                                x.ItemName,
                                x.Category,
                                x.CostElement,
                                x.BudgetCode,
                                x.UnitPrice,
                                x.ActualAvailableQuantity,
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
                                x.ApprovedDH,
                                x.ApprovedSH,
                                x.RequiredDate,
                                x.RequestOrderDate,
                                x.OrderDate,
                                x.TentativeDeliveryDate,
                                x.ActualDeliveryDate,
                                x.Fund,
                                x.OrderID,
                                x.OrderStatus,
                                x.OrderedQuantity,
                                x.OrderPrice

                            }).ToList().FindAll(x => x.ApprovedDH == true);
                            foreach (var request in requests)
                            {
                                string BU = string.Empty;
                                if (BudgetingController.lstBUs.Find(bu => bu.ID.ToString().Equals(request.BU)).BU == "MB")
                                    BU = "AS";
                                else if (BudgetingController.lstBUs.Find(bu => bu.ID.ToString().Equals(request.BU)).BU == "OSS")
                                    BU = "PS";
                                else
                                    BU = BudgetingController.lstBUs.Find(bu => bu.ID.ToString().Equals(request.BU)).BU;


                                dt.Rows.Add(
                                    BU,
                                    BudgetingController.lstOEMs.Find(oem => oem.ID.ToString().Equals(request.OEM)).OEM,
                                    BudgetingController.lstDEPTs.Find(dept => dept.ID.ToString().Equals(request.DEPT)).DEPT,
                                    BudgetingController.lstGroups_test.Find(grp => grp.ID.ToString().Equals(request.Group)).Group,
                                    request.Project,
                                    BudgetingController.lstItems.Find(item => item.S_No.ToString().Equals(request.ItemName)).Item_Name,
                                    request.Category != null && request.Category != "0" ? BudgetingController.lstPrdCateg.Find(cat => cat.ID.ToString().Equals(request.Category)).Category.Trim() : "",
                                    request.CostElement != null && request.CostElement != "0" ? BudgetingController.lstCostElement.Find(cost => cost.ID.ToString().Equals(request.CostElement)).CostElement : "",
                                    request.BudgetCode,
                                    request.UnitPrice != null ? Math.Round((decimal)request.UnitPrice, MidpointRounding.AwayFromZero) : 0,
                                    request.ActualAvailableQuantity != null ? request.ActualAvailableQuantity : "NA",
                                    request.ReqQuantity != null ? (int)request.ReqQuantity : 0,
                                    request.TotalPrice != null ? Math.Round((decimal)request.TotalPrice, MidpointRounding.AwayFromZero) : 0,
                                    request.ApprQuantity != null ? (int)request.ApprQuantity : 0,
                                    request.ApprCost != null ? Math.Round((decimal)request.ApprCost, MidpointRounding.AwayFromZero) : 0,
                                    request.ApprCost != null ? Math.Round((decimal)request.ApprCost, MidpointRounding.AwayFromZero) : 0,
                                    request.Comments,
                                    request.RequestorNT,
                                    request.SubmitDate.HasValue ? request.SubmitDate.Value.ToString("dd-MM-yyyy") : "",
                                    request.DHNT,
                                    request.DHAppDate.HasValue ? request.DHAppDate.Value.ToString("dd-MM-yyyy") : "",
                                    request.SHNT,
                                    request.SHAppDate.HasValue ? request.SHAppDate.Value.ToString("dd-MM-yyyy") : "",
                                    request.ApprovedDH,
                                    request.ApprovedSH,
                                    request.RequiredDate.HasValue ? request.RequiredDate.Value.ToString("dd-MM-yyyy") : "",
                                    request.RequestOrderDate.HasValue ? request.RequestOrderDate.Value.ToString("dd-MM-yyyy") : "",
                                    request.OrderDate.HasValue ? request.OrderDate.Value.ToString("dd-MM-yyyy") : "",
                                    request.TentativeDeliveryDate.HasValue ? request.TentativeDeliveryDate.Value.ToString("dd-MM-yyyy") : "",
                                    request.ActualDeliveryDate.HasValue ? request.ActualDeliveryDate.Value.ToString("dd-MM-yyyy") : "",
                                    request.Fund != null ? BudgetingController.lstFund.Find(fun => fun.ID.ToString().Equals(request.Fund)).Fund : BudgetingController.lstFund.Find(fun => fun.Fund.Trim().Equals("F02")).Fund,
                                    request.OrderID,
                                    (request.OrderStatus != null && request.OrderStatus.Trim() != "" && request.OrderStatus.Trim() != "0") ? BudgetingController.lstOrderStatus.Find(status => status.ID.ToString().Equals(request.OrderStatus.Trim())).OrderStatus : "",
                                    request.OrderedQuantity != null ? (int)request.OrderedQuantity : (int)0,
                                    request.OrderPrice);
                            }
                        }
                        using (XLWorkbook wb = new XLWorkbook())
                        {
                            var ws = wb.Worksheets.Add("VKM_Items_List_2020");
                            ws.Cell(2, 1).Value = "Dept Summary";
                            ws.Cell(3, 1).InsertTable(DeptSummaryData(true, useryear));
                            ws.Cell(8, 1).Value = "Section Summary";
                            ws.Cell(9, 1).InsertTable(SectionSummaryData(useryear));
                            ws.Cell(15, 1).InsertTable(dt);
                            using (MemoryStream stream = new MemoryStream())
                            {

                                wb.SaveAs(stream);
                                var robj = File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
                                return Json(robj, JsonRequestBehavior.AllowGet);
                            }
                        }

                    }


                    string presentUserDept = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).Department;
                    //string is_CCXC = string.Empty;
                    //if (presentUserDept.Contains("XC"))
                    //    is_CCXC = "XC";
                    //else
                    //    is_CCXC = "CC";

                    if (BudgetingController.lstBU_SPOCs.Find(person => person.VKMspoc.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())) == null) //Can be Lab Admins / Other VKM Authorized users
                    {


                        dt.Columns.AddRange(new DataColumn[59] { new DataColumn("Business Unit"),
                                            new DataColumn("OEM"),
                                            new DataColumn("Department"),
                                            new DataColumn("Group"),
                                            new DataColumn("Project"),
                                            new DataColumn("Item Name"),
                                            new DataColumn("Category"),
                                            new DataColumn("Cost Element"),
                                            new DataColumn("Budget Code"),
                                            new DataColumn("Unit Price",typeof(decimal)),
                                            new DataColumn("Actual Available Quantity"),
                                            new DataColumn("Required Quantity",typeof(Int32)),
                                            new DataColumn("Total Price",typeof(decimal)),
                                            new DataColumn("Reviewed Quantity",typeof(Int32)),
                                            new DataColumn("Reviewed Price",typeof(decimal)),
                                            new DataColumn("Total Budgeted Cost (USD)",typeof(decimal)),
                                            new DataColumn("Comments"),
                                            new DataColumn("Requestor"),
                                            new DataColumn("Submit Date"),
                                            new DataColumn("Reviewer L2"),
                                            new DataColumn("L2 Review Date"),
                                            new DataColumn("Reviewer L3"),
                                            new DataColumn("L3 Review Date"),
                                            new DataColumn("L2 Review Status"),
                                            new DataColumn("L3 Review Status"),
                                            new DataColumn("Required Date"),
                                            new DataColumn("Request Order Date"),
                                            new DataColumn("Order Date"),
                                            new DataColumn("Tentative Delivery Date"),
                                            new DataColumn("Actual Delivery Date"),
                                            new DataColumn("Fund"),
                                            new DataColumn("Order ID"),
                                            new DataColumn("Order Status"),
                                            new DataColumn("Ordered Quantity",typeof(Int32)),
                                            new DataColumn("Order Price",typeof(decimal)),
                                            new DataColumn("ELOSubmittedDate"),
                                            new DataColumn("DaysTaken"),
                                            new DataColumn("SRSubmitted"),
                                            new DataColumn("RFQnumber"),
                                            new DataColumn("PRnumber"),
                                            new DataColumn("SRawardedDate"),
                                            new DataColumn("SRApprovalDays"),
                                            new DataColumn("SR_responsibleBuyerNTID"),
                                            new DataColumn("SR_ManagerNTID"),
                                            new DataColumn("POSpocNTID"),
                                            new DataColumn("BudgetCodeDescription"),
                                            new DataColumn("Order_Type"),
                                            new DataColumn("CostCenter"),
                                            new DataColumn("BudgetCenterID"),
                                            new DataColumn("LabName"),
                                            new DataColumn("RFOReqNTID"),
                                            new DataColumn("RFOApprover"),
                                            new DataColumn("UnitofMeasure"),
                                            new DataColumn("UnloadingPoint"),
                                            new DataColumn("QuoteAvailable"),
                                            new DataColumn("Material_Part_Number"),
                                            new DataColumn("Supplier_Name_with_Address"),
                                            new DataColumn("Purchase_Type"),
                                            new DataColumn("Order Status Description")

                });

                        DataTable dt1 = new DataTable();
                        connection();
                        OpenConnection();
                        string Query = " Exec [dbo].[GetReqItemsList_VKMSPOCReviews_View] '" + (int.Parse(useryear) + 1).ToString() + "', '" + User.Identity.Name.Split('\\')[1].ToUpper() + "', 'Export' ";
                        SqlCommand cmd = new SqlCommand(Query, conn);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt1);
                        CloseConnection();

                        foreach (DataRow request in dt1.Rows)
                        {
                            dt.Rows.Add(
                                BudgetingController.lstBUs.Find(bu => bu.ID.ToString().Equals(request["BU"].ToString())).BU,
                                BudgetingController.lstOEMs.Find(oem => oem.ID.ToString().Equals(request["OEM"].ToString())).OEM,
                                BudgetingController.lstDEPTs.Find(dept => dept.ID.ToString().Equals(request["DEPT"].ToString())).DEPT,
                                BudgetingController.lstGroups_test.Find(grp => grp.ID.ToString().Equals(request["Group"].ToString())).Group,
                                request["Project"].ToString(),
                                BudgetingController.lstItems.Find(item => item.S_No.ToString().Equals(request["ItemName"].ToString())).Item_Name,
                                request["Category"] != null && request["Category"].ToString() != "0" ? BudgetingController.lstPrdCateg.Find(cat => cat.ID.ToString().Equals(request["Category"])).Category.Trim() : "",
                                request["CostElement"] != null && request["CostElement"].ToString() != "0" ? BudgetingController.lstCostElement.Find(cost => cost.ID.ToString().Equals(request["CostElement"].ToString())).CostElement : "",
                                request["BudgetCode"].ToString(),
                                request["UnitPrice"] != null ? Math.Round((decimal)request["UnitPrice"], MidpointRounding.AwayFromZero) : 0,
                                request["ActualAvailableQuantity"] != null ? request["ActualAvailableQuantity"].ToString() : "NA",
                                request["ReqQuantity"] != null ? (int)request["ReqQuantity"] : 0,
                                request["TotalPrice"] != null ? Math.Round((decimal)request["TotalPrice"], MidpointRounding.AwayFromZero) : 0,
                                request["ApprQuantity"] != null ? (int)request["ApprQuantity"] : 0,
                                request["ApprCost"] != null ? Math.Round((decimal)request["ApprCost"], MidpointRounding.AwayFromZero) : 0,
                                request["ApprCost"] != null ? Math.Round((decimal)request["ApprCost"], MidpointRounding.AwayFromZero) : 0,
                                request["Comments"].ToString(),
                                request["RequestorNT"].ToString(),
                                request["SubmitDate"].ToString() != "" ? ((DateTime)request["SubmitDate"]).ToString("dd-MM-yyyy") : string.Empty,
                                request["DHNT"].ToString(),
                                request["DHAppDate"].ToString() != "" ? ((DateTime)request["DHAppDate"]).ToString("dd-MM-yyyy") : string.Empty,
                                request["SHNT"].ToString(),
                                request["SHAppDate"].ToString() != "" ? ((DateTime)request["SHAppDate"]).ToString("dd-MM-yyyy") : string.Empty,
                                (bool)request["ApprovedSH"],
                                (bool)request["ApprovedDH"],
                                request["RequiredDate"].ToString() != "" ? ((DateTime)request["RequiredDate"]).ToString("dd-MM-yyyy") : string.Empty,
                                request["RequestOrderDate"].ToString() != "" ? ((DateTime)request["RequestOrderDate"]).ToString("dd-MM-yyyy") : string.Empty,
                                request["OrderDate"].ToString() != "" ? ((DateTime)request["OrderDate"]).ToString("dd-MM-yyyy") : string.Empty,
                                request["TentativeDeliveryDate"].ToString() != "" ? ((DateTime)request["TentativeDeliveryDate"]).ToString("dd-MM-yyyy") : string.Empty,
                                request["ActualDeliveryDate"].ToString() != "" ? ((DateTime)request["ActualDeliveryDate"]).ToString("dd-MM-yyyy") : string.Empty,
                                (request["Fund"] != null && request["Fund"].ToString().Trim() != "") ? BudgetingController.lstFund.Find(fun => fun.ID.ToString().Equals(request["Fund"])).Fund : BudgetingController.lstFund.Find(fun => fun.Fund.Trim().Equals("F02")).Fund,
                                request["OrderID"].ToString(),
                                (request["OrderStatus"] != null && request["OrderStatus"].ToString().Trim() != "" && request["OrderStatus"].ToString().Trim() != "0") ? BudgetingController.lstOrderStatus.Find(status => status.ID.ToString().Equals(request["OrderStatus"].ToString().Trim())).OrderStatus : "",
                                request["OrderedQuantity"] != null ? (int)request["OrderedQuantity"] : (int)0,
                                request["OrderPrice"],

                                request["ELOSubmittedDate"].ToString() != "1900-01-01" && request["ELOSubmittedDate"].ToString() != "" ? ((DateTime)request["ELOSubmittedDate"]).ToString("dd-MM-yyyy") : string.Empty,
                                request["DaysTaken"].ToString() != "" ? request["DaysTaken"].ToString() : "",
                                request["SRSubmitted"].ToString() != "1900-01-01" && request["SRSubmitted"].ToString() != "" ? ((DateTime)request["SRSubmitted"]).ToString("dd-MM-yyyy") : string.Empty,
                                request["RFQnumber"].ToString() != "" ? request["RFQnumber"].ToString() : "",
                                request["PRnumber"].ToString() != "" ? request["PRnumber"].ToString() : "",
                                request["SRawardedDate"].ToString() != "1900-01-01" && request["SRawardedDate"].ToString() != "" ? ((DateTime)request["SRawardedDate"]).ToString("dd-MM-yyyy") : string.Empty,
                                request["SRApprovalDays"].ToString() != "" ? request["SRApprovalDays"].ToString() : "",
                                request["SR_responsibleBuyerNTID"].ToString() != "" ? request["SR_responsibleBuyerNTID"].ToString() : "",
                                request["SR_ManagerNTID"].ToString() != "" ? request["SR_ManagerNTID"].ToString() : "",
                                request["POSpocNTID"].ToString() != "" ? request["POSpocNTID"].ToString() : "",
                                request["BudgetCodeDescription"].ToString().Trim() != "" ? request["BudgetCodeDescription"].ToString().Trim() : "",
                                request["Order_Type"].ToString() != "" ? request["Order_Type"].ToString() : "",
                                request["CostCenter"].ToString() != "" ? request["CostCenter"].ToString() : "",
                                request["BudgetCenterID"].ToString() != "" ? request["BudgetCenterID"].ToString() : "",
                                request["LabName"].ToString() != "" ? request["LabName"].ToString() : "",
                                request["RFOReqNTID"].ToString() != "" ? request["RFOReqNTID"].ToString() : "",
                                request["RFOApprover"].ToString() != "" ? request["RFOApprover"].ToString() : "",
                                request["UnitofMeasure"].ToString() != "" ? request["UnitofMeasure"].ToString() : "",
                                request["UnloadingPoint"].ToString() != "" ? request["UnloadingPoint"].ToString() : "",
                                request["QuoteAvailable"].ToString() != "" ? request["QuoteAvailable"].ToString() : "",
                                request["Material_Part_Number"].ToString() != "" ? request["Material_Part_Number"].ToString() : "",
                                request["Supplier_Name_with_Address"].ToString() != "" ? request["Supplier_Name_with_Address"].ToString() : "",
                                request["Purchase_Type"].ToString() != "" ? request["Purchase_Type"].ToString() : "",
                                (request["Description"].ToString().Trim() != "" && request["Description"].ToString().Trim() != "0") ? BudgetingController.lstOrderDescription.Find(x => x.ID.Equals(int.Parse(request["Description"].ToString()))).Description.ToString() : ""
                                );

                        }

                    }
                    else // VKM SPOC or VKM Admin of CC/XC
                    {

                        dt.Columns.AddRange(new DataColumn[28] { new DataColumn("Business Unit"),
                                            new DataColumn("OEM"),
                                            new DataColumn("Department"),
                                            new DataColumn("Group"),
                                            new DataColumn("Project"),
                                            new DataColumn("Item Name"),
                                            new DataColumn("Category"),
                                            new DataColumn("Cost Element"),
                                            new DataColumn("Budget Code"),
                                            new DataColumn("Unit Price",typeof(decimal)),
                                            new DataColumn("Actual Available Quantity"),
                                            new DataColumn("Required Quantity",typeof(Int32)),
                                            new DataColumn("Total Price",typeof(decimal)),
                                            new DataColumn("Reviewed Quantity",typeof(Int32)),
                                            new DataColumn("Reviewed Price",typeof(decimal)),
                                            new DataColumn("Total Budgeted Cost (USD)",typeof(decimal)),
                                            new DataColumn("Comments"),
                                            new DataColumn("Requestor"),
                                            new DataColumn("Submit Date"),
                                            new DataColumn("Reviewer L2"),
                                            new DataColumn("L2 Review Date"),
                                            new DataColumn("Reviewer L3"),
                                            new DataColumn("L3 Review Date"),
                                            new DataColumn("L2 Review Status"),
                                            new DataColumn("L3 Review Status"),
                                            new DataColumn("Order ID"),
                                            new DataColumn("Order Status"),
                                            new DataColumn("Order Price",typeof(decimal))

                });

                        DataTable dt1 = new DataTable();
                        connection();
                        OpenConnection();
                        string Query = " Exec [dbo].[GetReqItemsList_VKMSPOCReviews_View] '" + (int.Parse(useryear) + 1).ToString() + "', '" + User.Identity.Name.Split('\\')[1].ToUpper() + "', 'Export' ";
                        SqlCommand cmd = new SqlCommand(Query, conn);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt1);
                        CloseConnection();

                        foreach (DataRow request in dt1.Rows)
                        {
                            dt.Rows.Add(
                            BudgetingController.lstBUs.Find(bu => bu.ID.ToString().Equals(request["BU"].ToString())).BU,
                            BudgetingController.lstOEMs.Find(oem => oem.ID.ToString().Equals(request["OEM"].ToString())).OEM,
                            BudgetingController.lstDEPTs.Find(dept => dept.ID.ToString().Equals(request["DEPT"].ToString())).DEPT,
                            BudgetingController.lstGroups_test.Find(grp => grp.ID.ToString().Equals(request["Group"].ToString())).Group,
                            request["Project"].ToString(),
                            BudgetingController.lstItems.Find(item => item.S_No.ToString().Equals(request["ItemName"].ToString())).Item_Name,
                            request["Category"] != null && request["Category"].ToString() != "0" ? BudgetingController.lstPrdCateg.Find(cat => cat.ID.ToString().Equals(request["Category"].ToString())).Category.Trim() : "",
                            request["CostElement"] != null && request["CostElement"].ToString() != "0" ? BudgetingController.lstCostElement.Find(cost => cost.ID.ToString().Equals(request["CostElement"].ToString())).CostElement : "",
                            request["BudgetCode"].ToString(),
                            request["UnitPrice"] != null ? Math.Round((decimal)request["UnitPrice"], MidpointRounding.AwayFromZero) : 0,
                            request["ActualAvailableQuantity"] != null ? request["ActualAvailableQuantity"] : "NA",
                            request["ReqQuantity"] != null ? (int)request["ReqQuantity"] : 0,
                            request["TotalPrice"] != null ? Math.Round((decimal)request["TotalPrice"], MidpointRounding.AwayFromZero) : 0,
                            request["ApprQuantity"] != null ? (int)request["ApprQuantity"] : 0,
                            request["ApprCost"] != null ? Math.Round((decimal)request["ApprCost"], MidpointRounding.AwayFromZero) : 0,
                            request["ApprCost"] != null ? Math.Round((decimal)request["ApprCost"], MidpointRounding.AwayFromZero) : 0,
                            request["Comments"].ToString(),
                            request["RequestorNT"].ToString(),
                            request["SubmitDate"].ToString() != "" ? ((DateTime)request["SubmitDate"]).ToString("dd-MM-yyyy") : string.Empty,
                            request["DHNT"].ToString(),
                            request["DHAppDate"].ToString() != "" ? ((DateTime)request["DHAppDate"]).ToString("dd-MM-yyyy") : string.Empty,
                            request["SHNT"].ToString(),
                            request["SHAppDate"].ToString() != "" ? ((DateTime)request["SHAppDate"]).ToString("dd-MM-yyyy") : string.Empty,
                            (bool)request["ApprovedDH"],
                            (bool)request["ApprovedSH"],
                            request["OrderID"].ToString(),
                            (request["OrderStatus"] != null && request["OrderStatus"].ToString().Trim() != "" && request["OrderStatus"].ToString().Trim() != "0") ? BudgetingController.lstOrderStatus.Find(status => status.ID.ToString().Equals(request["OrderStatus"].ToString().Trim())).OrderStatus : "",
                            request["OrderPrice"]
                            );

                        }
                    }

                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        var ws = wb.Worksheets.Add("VKM_Items_List");
                        ws.Cell(1, 1).Value = "BU Summary";


                        List<string> years = new List<string>()
               { DateTime.Now.Year.ToString(), DateTime.Now.AddYears(1).Year.ToString() }; //VKM Yr , VKM Yr-1 (Previous VKM Year)
                        string presentUserDept1 = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).Department;
                        //presentUserDept = "XC";
                        if (presentUserDept1.Contains("XC"))
                            ws.Cell(2, 1).InsertTable(XC_BUSummaryComparison(years));
                        else
                            ws.Cell(2, 1).InsertTable(CC_BUSummaryComparison(years));
                        //ws.Cell(2, 1).InsertTable(BUSummaryData_CC(useryear));

                        ws.Cell(8, 1).Value = "Dept Summary";
                        ws.Cell(9, 1).InsertTable(DeptSummaryData(true, useryear));
                        ws.Cell(1, 15).Value = "Section Summary";
                        ws.Cell(2, 15).InsertTable(SectionSummaryData(useryear));
                        ws.Cell(15, 1).InsertTable(dt);
                        using (MemoryStream stream = new MemoryStream())
                        {

                            wb.SaveAs(stream);
                            var robj = File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
                            return Json(robj, JsonRequestBehavior.AllowGet);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                WriteLog("Error - ExportDataToExcel : NTID - " + User.Identity.Name.Split('\\')[1].Trim().ToUpper() + ", ErrorMsg : " + ex + ", At - " + DateTime.Now);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// Function to export all items to Excel - filtering of CC dept / XC dept based on user's dept 
        ///  /// /// <param name="year"></param>
        /// </summary>
        /// <returns></returns>
        public ActionResult ExportDataToExcelAll(string useryear)
        {
            try
            {
                var presentUserNTID = User.Identity.Name.Split('\\')[1].ToUpper();
                string filename = @"VKM " + (int.Parse(useryear) + 1).ToString() + " " + presentUserNTID + " AllRequest_List_" + DateTime.Now.ToShortDateString() + ".xlsx";

                using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

                //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

                {


                    System.Data.DataTable dt = new System.Data.DataTable("Request_List");
                    if (useryear.Contains("2020"))
                    {

                        dt.Columns.AddRange(new DataColumn[42] { new DataColumn("Business Unit"),
                                            new DataColumn("OEM"),
                                            new DataColumn("Department"),
                                            new DataColumn("Group"),
                                            new DataColumn("Project"),
                                            new DataColumn("Item Name"),
                                            new DataColumn("Category"),
                                            new DataColumn("Cost Element"),
                                            new DataColumn("Budget Code"),
                                            new DataColumn("Unit Price", typeof(decimal)),
                                            new DataColumn("Actual Available Quantity"),
                                            new DataColumn("Required Quantity", typeof(Int32)),
                                            new DataColumn("Total Price", typeof(decimal)),
                                            new DataColumn("Reviewed Quantity", typeof(Int32)),
                                            new DataColumn("Reviewed Price", typeof(decimal)),
                                            new DataColumn("Total Budgeted Cost (USD)", typeof(decimal)),
                                            new DataColumn("Comments"),
                                            new DataColumn("Requestor"),
                                            new DataColumn("Submit Date"),
                                            new DataColumn("Reviewer L2"),
                                            new DataColumn("L2 Review Date"),
                                            new DataColumn("Reviewer L3"),
                                            new DataColumn("L3 Review Date"),
                                            new DataColumn("L2 Review Status"),
                                            new DataColumn("L3 Review Status"),
                                            new DataColumn("Required Date"),
                                            new DataColumn("Request Order Date"),
                                            new DataColumn("Order Date"),
                                            new DataColumn("Tentative Delivery Date"),
                                            new DataColumn("Actual Delivery Date"),
                                            new DataColumn("Fund"),
                                            new DataColumn("Order ID"),
                                            new DataColumn("Order Status"),
                                            new DataColumn("Ordered Quantity",typeof(Int32)),
                                            new DataColumn("Order Price ($)",typeof(decimal)),
                                            new DataColumn("Projected_Amount ($)",typeof(decimal)),
                                            new DataColumn("Unused Amount ($)",typeof(decimal)),
                                            new DataColumn("Is Projected"),
                                            new DataColumn("Q1"),
                                            new DataColumn("Q2"),
                                            new DataColumn("Q3"),
                                            new DataColumn("Q4")


                });


                        DataTable dt1 = new DataTable();
                        connection();
                        OpenConnection();
                        string Query = " Exec [dbo].[GetReqItemsList_VKMSPOCReviews_View] '" + (int.Parse(useryear) + 1).ToString() + "', '" + User.Identity.Name.Split('\\')[1].ToUpper() + "', 'Export All' ";
                        SqlCommand cmd = new SqlCommand(Query, conn);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt1);
                        CloseConnection();

                        foreach (DataRow request in dt1.Rows)
                        {
                            string BU = string.Empty;
                            if (BudgetingController.lstBUs.Find(bu => bu.ID.ToString().Equals(request["BU"])).BU == "MB")
                                BU = "AS";
                            else if (BudgetingController.lstBUs.Find(bu => bu.ID.ToString().Equals(request["BU"])).BU == "OSS")
                                BU = "PS";
                            else
                                BU = BudgetingController.lstBUs.Find(bu => bu.ID.ToString().Equals(request["BU"])).BU;

                            dt.Rows.Add(
                                BU,
                                BudgetingController.lstOEMs.Find(oem => oem.ID.ToString().Equals(request["OEM"])).OEM,
                                BudgetingController.lstDEPTs.Find(dept => dept.ID.ToString().Equals(request["DEPT"])).DEPT,
                                BudgetingController.lstGroups_test.Find(grp => grp.ID.ToString().Equals(request["Group"])).Group,
                                request["Project"],
                                BudgetingController.lstItems.Find(item => item.S_No.ToString().Equals(request["ItemName"])).Item_Name,
                                request["Category"] != null && request["Category"].ToString() != "0" ? BudgetingController.lstPrdCateg.Find(cat => cat.ID.ToString().Equals(request["Category"])).Category.Trim() : "",
                                request["CostElement"] != null && request["CostElement"].ToString() != "0" ? BudgetingController.lstCostElement.Find(cost => cost.ID.ToString().Equals(request["CostElement"])).CostElement : "",
                                request["BudgetCode"],
                                //Math.Round((decimal)request.UnitPrice) 
                                request["UnitPrice"] != null ? Math.Round((decimal)request["UnitPrice"], MidpointRounding.AwayFromZero) : 0,
                                request["ActualAvailableQuantity"] != null ? request["ActualAvailableQuantity"] : "NA",
                                request["ReqQuantity"] != null ? (int)request["ReqQuantity"] : 0,
                                request["TotalPrice"] != null ? Math.Round((decimal)request["TotalPrice"], MidpointRounding.AwayFromZero) : 0,
                                request["ApprQuantity"] != null ? request["ApprQuantity"] : null,
                                request["ApprCost"] != null ? Math.Round((decimal)request["ApprCost"], MidpointRounding.AwayFromZero) : 0,
                                request["ApprCost"] != null ? Math.Round((decimal)request["ApprCost"], MidpointRounding.AwayFromZero) : 0,
                                request["Comments"],
                                request["RequestorNT"],
                                request["SubmitDate"].ToString() != "" ? ((DateTime)request["SubmitDate"]).ToString("dd-MM-yyyy") : string.Empty,
                                request["DHNT"],
                                request["DHAppDate"].ToString() != "" ? ((DateTime)request["DHAppDate"]).ToString("dd-MM-yyyy") : string.Empty,
                                request["SHNT"],
                                request["SHAppDate"].ToString() != "" ? ((DateTime)request["SHAppDate"]).ToString("dd-MM-yyyy") : string.Empty,
                                request["ApprovedDH"],
                                request["ApprovedSH"],
                                request["RequiredDate"].ToString() != "" ? ((DateTime)request["RequiredDate"]).ToString("dd-MM-yyyy") : string.Empty,

                                    request["RequestOrderDate"].ToString() != "" ? ((DateTime)request["RequestOrderDate"]).ToString("dd-MM-yyyy") : string.Empty,

                                    request["OrderDate"].ToString() != "" ? ((DateTime)request["OrderDate"]).ToString("dd-MM-yyyy") : string.Empty,

                                    request["TentativeDeliveryDate"].ToString() != "" ? ((DateTime)request["TentativeDeliveryDate"]).ToString("dd-MM-yyyy") : string.Empty,

                                    request["ActualDeliveryDate"].ToString() != "" ? ((DateTime)request["ActualDeliveryDate"]).ToString("dd-MM-yyyy") : string.Empty,

                                    request["Fund"] != null ? BudgetingController.lstFund.Find(fun => fun.ID.ToString().Equals(request["Fund"])).Fund : BudgetingController.lstFund.Find(fun => fun.Fund.Trim().Equals("F02")).Fund,
                                    request["OrderID"],
                                    (request["OrderStatus"] != null && request["OrderStatus"].ToString().Trim() != "" && request["OrderStatus"].ToString().Trim() != "0") ? BudgetingController.lstOrderStatus.Find(status => status.ID.ToString().Equals(request["OrderStatus"].ToString().Trim())).OrderStatus : "",
                                    request["OrderedQuantity"] != null ? (int)request["OrderedQuantity"] : (int)0,
                                    request["OrderPrice"],
                                     request["Projected_Amount"] != null ? Math.Round((decimal)request["Projected_Amount"], MidpointRounding.AwayFromZero) : 0,
                                request["Unused_Amount"] != null ? Math.Round((decimal)request["Unused_Amount"], MidpointRounding.AwayFromZero) : 0,
                                request["isProjected"] != null ? request["isProjected"] : false,
                                 request["Q1"] != null ? request["Q1"] : false,
                                  request["Q2"] != null ? request["Q2"] : false,
                                   request["Q3"] != null ? request["Q3"] : false,
                                   request["Q4"] != null ? request["Q4"] : false
                                );

                        }

                        using (XLWorkbook wb = new XLWorkbook())
                        {
                            var ws = wb.Worksheets.Add("VKM_Items_List_2020");

                            ws.Cell(3, 1).InsertTable(dt);
                            using (MemoryStream stream = new MemoryStream())
                            {

                                wb.SaveAs(stream);
                                var robj = File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
                                return Json(robj, JsonRequestBehavior.AllowGet);
                            }
                        }
                    }

                    string presentUserDept = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).Department;
                    //string is_CCXC = string.Empty;
                    //if (presentUserDept.Contains("XC"))
                    //    is_CCXC = "XC";
                    //else
                    //    is_CCXC = "CC";

                    //is_CCXC = "XC";

                    if (BudgetingController.lstBU_SPOCs.Find(person => person.VKMspoc.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())) == null)
                    {

                        dt.Columns.AddRange(new DataColumn[68] { new DataColumn("Business Unit"),
                                            new DataColumn("OEM"),
                                            new DataColumn("Department"),
                                            new DataColumn("Group"),
                                            new DataColumn("Project"),
                                            new DataColumn("Item Name"),
                                            new DataColumn("Category"),
                                            new DataColumn("Cost Element"),
                                            new DataColumn("Budget Code"),
                                            new DataColumn("Unit Price",typeof(decimal)),
                                            new DataColumn("Actual Available Quantity"),
                                            new DataColumn("Required Quantity",typeof(Int32)),
                                            new DataColumn("Total Price",typeof(decimal)),
                                            new DataColumn("Total Requested in USD",typeof(decimal)),
                                            new DataColumn("Reviewed Quantity",typeof(Int32)),
                                            new DataColumn("Reviewed Price",typeof(decimal)),
                                            new DataColumn("Total Budgeted Cost (USD)",typeof(decimal)),
                                            new DataColumn("Comments"),
                                            new DataColumn("Requestor"),
                                            new DataColumn("Submit Date"),
                                            new DataColumn("Reviewer L2"),
                                            new DataColumn("L2 Review Date"),
                                            new DataColumn("Reviewer L3"),
                                            new DataColumn("L3 Review Date"),
                                            new DataColumn("Item At Requestor"),
                                            new DataColumn("Pending L2 Review"),
                                            new DataColumn("Pending L3 Review"),
                                            new DataColumn("Required Date"),
                                            new DataColumn("Request Order Date"),
                                            new DataColumn("Order Date"),
                                            new DataColumn("Tentative Delivery Date"),
                                            new DataColumn("Actual Delivery Date"),
                                            new DataColumn("Fund"),
                                            new DataColumn("Order ID"),
                                            new DataColumn("Order Status"),
                                            new DataColumn("Ordered Quantity",typeof(Int32)),
                                            new DataColumn("Order Price",typeof(decimal)),
                                            new DataColumn("Projected_Amount ($)",typeof(decimal)),
                                            new DataColumn("Unused Amount ($)",typeof(decimal)),
                                            new DataColumn("Is Projected"),
                                            new DataColumn("Q1"),
                                            new DataColumn("Q2"),
                                            new DataColumn("Q3"),
                                            new DataColumn("Q4"),
                                            new DataColumn("ELOSubmittedDate"),
                                            new DataColumn("DaysTaken"),
                                            new DataColumn("SRSubmitted"),
                                            new DataColumn("RFQnumber"),
                                            new DataColumn("PRnumber"),
                                            new DataColumn("SRawardedDate"),
                                            new DataColumn("SRApprovalDays"),
                                            new DataColumn("SR_responsibleBuyerNTID"),
                                            new DataColumn("SR_ManagerNTID"),
                                            new DataColumn("POSpocNTID"),
                                            new DataColumn("BudgetCodeDescription"),
                                            new DataColumn("Order_Type"),
                                            new DataColumn("CostCenter"),
                                            new DataColumn("BudgetCenterID"),
                                            new DataColumn("LabName"),
                                            new DataColumn("RFOReqNTID"),
                                            new DataColumn("RFOApprover"),
                                            new DataColumn("UnitofMeasure"),
                                            new DataColumn("UnloadingPoint"),
                                            new DataColumn("QuoteAvailable"),
                                            new DataColumn("Material_Part_Number"),
                                            new DataColumn("Supplier_Name_with_Address"),
                                            new DataColumn("Purchase_Type"),
                                            new DataColumn("Order Status Description")


                });

                        DataTable dt1 = new DataTable();
                        connection();
                        OpenConnection();
                        string Query = " Exec [dbo].[GetReqItemsList_VKMSPOCReviews_View] '" + (int.Parse(useryear) + 1).ToString() + "', '" + User.Identity.Name.Split('\\')[1].ToUpper() + "', 'Export All' ";
                        SqlCommand cmd = new SqlCommand(Query, conn);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt1);
                        CloseConnection();

                        foreach (DataRow request in dt1.Rows)
                        {
                            ////////////////////////////////////////////////////////////////////////////////////////////////////////
                            var ApprovalDH = request["ApprovalDH"].ToString() != "" ? Convert.ToBoolean(request["ApprovalDH"].ToString()) : false;
                            var ApprovedDH = request["ApprovedDH"].ToString() != "" ? Convert.ToBoolean(request["ApprovedDH"].ToString()) : false;
                            var ApprovalSH = request["ApprovalSH"].ToString() != "" ? Convert.ToBoolean(request["ApprovalSH"].ToString()) : false;
                            var ApprovedSH = request["ApprovedSH"].ToString() != "" ? Convert.ToBoolean(request["ApprovedSH"].ToString()) : false;
                            var isCancelled = request["isCancelled"].ToString() != "" ? Convert.ToInt32(request["isCancelled"].ToString()) : 0;
                            dt.Rows.Add(
                                BudgetingController.lstBUs.Find(bu => bu.ID.ToString().Equals(request["BU"].ToString())).BU,
                                BudgetingController.lstOEMs.Find(oem => oem.ID.ToString().Equals(request["OEM"].ToString())).OEM,
                                BudgetingController.lstDEPTs.Find(dept => dept.ID.ToString().Equals(request["DEPT"].ToString())).DEPT,
                                BudgetingController.lstGroups_test.Find(grp => grp.ID.ToString().Equals(request["Group"].ToString())).Group,
                                request["Project"].ToString(),
                                BudgetingController.lstItems.Find(item => item.S_No.ToString().Equals(request["ItemName"].ToString())).Item_Name,
                                request["Category"] != null && request["Category"].ToString() != "0" ? BudgetingController.lstPrdCateg.Find(cat => cat.ID.ToString().Equals(request["Category"].ToString())).Category.Trim() : "",
                                request["CostElement"] != null && request["CostElement"].ToString() != "0" ? BudgetingController.lstCostElement.Find(cost => cost.ID.ToString().Equals(request["CostElement"].ToString())).CostElement : "",
                                request["BudgetCode"].ToString(),
                                request["UnitPrice"] != null ? Math.Round((decimal)request["UnitPrice"], MidpointRounding.AwayFromZero) : 0,
                                request["ActualAvailableQuantity"] != null ? request["ActualAvailableQuantity"] : "NA",
                                request["ReqQuantity"] != null ? (int)request["ReqQuantity"] : 0,
                                request["TotalPrice"] != null ? Math.Round((decimal)request["TotalPrice"], MidpointRounding.AwayFromZero) : 0,
                                request["TotalPrice"] != null ? Math.Round((decimal)request["TotalPrice"], MidpointRounding.AwayFromZero) : 0,
                                request["ApprQuantity"] != null ? request["ApprQuantity"] : 0,
                                 request["ApprCost"] != null ? Math.Round((decimal)request["ApprCost"], MidpointRounding.AwayFromZero) : 0,
                                request["ApprCost"] != null ? Math.Round((decimal)request["ApprCost"], MidpointRounding.AwayFromZero) : 0,
                                request["Comments"],
                                request["RequestorNT"],
                                request["SubmitDate"].ToString() != "" ? ((DateTime)request["SubmitDate"]).ToString("dd-MM-yyyy") : string.Empty,
                                request["DHNT"],
                                request["DHAppDate"].ToString() != "" ? ((DateTime)request["DHAppDate"]).ToString("dd-MM-yyyy") : string.Empty,
                                request["SHNT"],
                                request["SHAppDate"].ToString() != "" ? ((DateTime)request["SHAppDate"]).ToString("dd-MM-yyyy") : string.Empty,
                                ApprovalDH,
                                ((bool)request["ApprovalDH"] && !((bool)(request["ApprovedDH"]))),
                                ((bool)request["ApprovalSH"] && !((bool)(request["ApprovedSH"]))),
                                request["RequiredDate"].ToString() != "1900-01-01" ? ((DateTime)request["RequiredDate"]).ToString("dd-MM-yyyy") : string.Empty,
                                request["RequestOrderDate"].ToString() != "1900-01-01" ? ((DateTime)request["RequestOrderDate"]).ToString("dd-MM-yyyy") : string.Empty,
                                request["OrderDate"].ToString() != "1900-01-01" ? ((DateTime)request["RequestOrderDate"]).ToString("dd-MM-yyyy") : string.Empty,
                                request["TentativeDeliveryDate"].ToString() != "1900-01-01" ? ((DateTime)request["RequestOrderDate"]).ToString("dd-MM-yyyy") : string.Empty,
                                request["ActualDeliveryDate"].ToString() != "1900-01-01" ? ((DateTime)request["RequestOrderDate"]).ToString("dd-MM-yyyy") : string.Empty,
                                (request["Fund"] != null && request["Fund"].ToString().Trim() != "") ? BudgetingController.lstFund.Find(fun => fun.ID.ToString().Equals(request["Fund"].ToString())).Fund : BudgetingController.lstFund.Find(fun => fun.Fund.Trim().Equals("F02")).Fund,
                                request["OrderID"],
                                (request["OrderStatus"] != null && request["OrderStatus"].ToString().Trim() != "" && request["OrderStatus"].ToString().Trim() != "0") ? BudgetingController.lstOrderStatus.Find(status => status.ID.ToString().Equals(request["OrderStatus"].ToString().Trim())).OrderStatus : "",
                                request["OrderedQuantity"] != null ? (int)request["OrderedQuantity"] : (int)0,
                                request["OrderPrice"],
                                   request["Projected_Amount"] != null ? Math.Round((decimal)request["Projected_Amount"], MidpointRounding.AwayFromZero) : 0,
                            request["Unused_Amount"] != null ? Math.Round((decimal)request["Unused_Amount"], MidpointRounding.AwayFromZero) : 0,
                            request["isProjected"] != null ? request["isProjected"] : false,
                             request["Q1"] != null ? request["Q1"] : false,
                              request["Q2"] != null ? request["Q2"] : false,
                               request["Q3"] != null ? request["Q3"] : false,
                               request["Q4"] != null ? request["Q4"] : false,

                               request["ELOSubmittedDate"].ToString() != "1900-01-01" && request["ELOSubmittedDate"].ToString() != "" ? ((DateTime)request["ELOSubmittedDate"]).ToString("dd-MM-yyyy") : string.Empty,
                               request["DaysTaken"].ToString() != "" ? request["DaysTaken"].ToString() : "",
                               request["SRSubmitted"].ToString() != "1900-01-01" && request["SRSubmitted"].ToString() != "" ? ((DateTime)request["SRSubmitted"]).ToString("dd-MM-yyyy") : string.Empty,
                               request["RFQnumber"].ToString() != "" ? request["RFQnumber"].ToString() : "",
                               request["PRnumber"].ToString() != "" ? request["PRnumber"].ToString() : "",
                               request["SRawardedDate"].ToString() != "1900-01-01" && request["SRawardedDate"].ToString() != "" ? ((DateTime)request["SRawardedDate"]).ToString("dd-MM-yyyy") : string.Empty,
                               request["SRApprovalDays"].ToString() != "" ? request["SRApprovalDays"].ToString() : "",
                               request["SR_responsibleBuyerNTID"].ToString() != "" ? request["SR_responsibleBuyerNTID"].ToString() : "",
                               request["SR_ManagerNTID"].ToString() != "" ? request["SR_ManagerNTID"].ToString() : "",
                               request["POSpocNTID"].ToString() != "" ? request["POSpocNTID"].ToString() : "",
                               request["BudgetCodeDescription"].ToString().Trim() != "" ? request["BudgetCodeDescription"].ToString().Trim() : "",
                               request["Order_Type"].ToString() != "" ? request["Order_Type"].ToString() : "",
                               request["CostCenter"].ToString() != "" ? request["CostCenter"].ToString() : "",
                               request["BudgetCenterID"].ToString() != "" ? request["BudgetCenterID"].ToString() : "",
                               request["LabName"].ToString() != "" ? request["LabName"].ToString() : "",
                               request["RFOReqNTID"].ToString() != "" ? request["RFOReqNTID"].ToString() : "",
                               request["RFOApprover"].ToString() != "" ? request["RFOApprover"].ToString() : "",
                               request["UnitofMeasure"].ToString() != "" ? request["UnitofMeasure"].ToString() : "",
                               request["UnloadingPoint"].ToString() != "" ? request["UnloadingPoint"].ToString() : "",
                               request["QuoteAvailable"].ToString() != "" ? request["QuoteAvailable"].ToString() : "",
                               request["Material_Part_Number"].ToString() != "" ? request["Material_Part_Number"].ToString() : "",
                               request["Supplier_Name_with_Address"].ToString() != "" ? request["Supplier_Name_with_Address"].ToString() : "",
                               request["Purchase_Type"].ToString() != "" ? request["Purchase_Type"].ToString() : "",
                               (request["Description"].ToString().Trim() != "" && request["Description"].ToString().Trim() != "0") ? BudgetingController.lstOrderDescription.Find(x => x.ID.Equals(int.Parse(request["Description"].ToString()))).Description.ToString() : ""
                               );

                        }
                    }

                    else
                    {
                        dt.Columns.AddRange(new DataColumn[37] { new DataColumn("Business Unit"),
                                            new DataColumn("OEM"),
                                            new DataColumn("Department"),
                                            new DataColumn("Group"),
                                            new DataColumn("Project"),
                                            new DataColumn("Item Name"),
                                            new DataColumn("Category"),
                                            new DataColumn("Cost Element"),
                                            new DataColumn("Budget Code"),
                                            new DataColumn("Unit Price",typeof(decimal)),
                                            new DataColumn("Actual Available Quantity"),
                                            new DataColumn("Required Quantity",typeof(Int32)),
                                            new DataColumn("Total Price",typeof(decimal)),
                                            new DataColumn("Total Requested in USD",typeof(decimal)),
                                            new DataColumn("Reviewed Quantity",typeof(Int32)),
                                            new DataColumn("Reviewed Price",typeof(decimal)),
                                            new DataColumn("Total Budgeted Cost (USD)",typeof(decimal)),
                                            new DataColumn("Comments"),
                                            new DataColumn("Requestor"),
                                            new DataColumn("Submit Date"),
                                            new DataColumn("Reviewer L2"),
                                            new DataColumn("L2 Review Date"),
                                            new DataColumn("Reviewer L3"),
                                            new DataColumn("L3 Review Date"),
                                            new DataColumn("Item At Requestor"),
                                            new DataColumn("Pending L2 Review"),
                                            new DataColumn("Pending L3 Review"),
                                            new DataColumn("Order ID"),
                                            new DataColumn("Order Status"),
                                            new DataColumn("Order Price",typeof(decimal)),
                                             new DataColumn("Projected_Amount ($)",typeof(decimal)),
                                            new DataColumn("Unused Amount ($)",typeof(decimal)),
                                            new DataColumn("Is Projected"),
                                            new DataColumn("Q1"),
                                            new DataColumn("Q2"),
                                            new DataColumn("Q3"),
                                            new DataColumn("Q4")


                });


                        DataTable dt1 = new DataTable();
                        connection();
                        OpenConnection();
                        //string Query = " Exec [dbo].[GetReqItemsList_VKMSPOCReviews_View] '" + useryear + "', 'MAE9COB', 'Export All' ";
                        string Query = " Exec [dbo].[GetReqItemsList_VKMSPOCReviews_View] '" + (int.Parse(useryear) + 1).ToString() + "', '" + User.Identity.Name.Split('\\')[1].ToUpper() + "', 'Export All' ";
                        SqlCommand cmd = new SqlCommand(Query, conn);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt1);
                        CloseConnection();

                        foreach (DataRow request in dt1.Rows)
                        {
                            ////////////////////////////////////////////////////////////////////////////////////////////////////////
                            var ApprovalDH = request["ApprovalDH"].ToString() != "" ? Convert.ToBoolean(request["ApprovalDH"].ToString()) : false;
                            var ApprovedDH = request["ApprovedDH"].ToString() != "" ? Convert.ToBoolean(request["ApprovedDH"].ToString()) : false;
                            var ApprovalSH = request["ApprovalSH"].ToString() != "" ? Convert.ToBoolean(request["ApprovalSH"].ToString()) : false;
                            var ApprovedSH = request["ApprovedSH"].ToString() != "" ? Convert.ToBoolean(request["ApprovedSH"].ToString()) : false;
                            var isCancelled = request["isCancelled"].ToString() != "" ? Convert.ToInt32(request["isCancelled"].ToString()) : 0;


                            dt.Rows.Add(
                                BudgetingController.lstBUs.Find(bu => bu.ID.ToString().Equals(request["BU"].ToString())).BU,
                                BudgetingController.lstOEMs.Find(oem => oem.ID.ToString().Equals(request["OEM"].ToString())).OEM,
                                BudgetingController.lstDEPTs.Find(dept => dept.ID.ToString().Equals(request["DEPT"].ToString())).DEPT,
                                BudgetingController.lstGroups_test.Find(grp => grp.ID.ToString().Equals(request["Group"].ToString())).Group,
                                request["Project"].ToString(),
                                BudgetingController.lstItems.Find(item => item.S_No.ToString().Equals(request["ItemName"].ToString())).Item_Name,
                                request["Category"].ToString() != "" && request["Category"].ToString() != "0" ? BudgetingController.lstPrdCateg.Find(cat => cat.ID.ToString().Equals(request["Category"].ToString())).Category.Trim() : "",
                                request["CostElement"].ToString() != "" && request["CostElement"].ToString() != "0" ? BudgetingController.lstCostElement.Find(cost => cost.ID.ToString().Equals(request["CostElement"].ToString())).CostElement : "",
                                request["BudgetCode"].ToString(),
                                request["UnitPrice"].ToString() != "" ? Math.Round((decimal)request["UnitPrice"], MidpointRounding.AwayFromZero) : 0,
                                request["ActualAvailableQuantity"].ToString() != "" ? request["ActualAvailableQuantity"] : "NA",
                                request["ReqQuantity"].ToString() != "" ? request["ReqQuantity"] : 0,
                                request["TotalPrice"].ToString() != "" ? Math.Round((decimal)request["TotalPrice"], MidpointRounding.AwayFromZero) : 0,
                                request["TotalPrice"].ToString() != "" ? Math.Round((decimal)request["TotalPrice"], MidpointRounding.AwayFromZero) : 0,
                                request["ApprQuantity"].ToString() != "" ? request["ApprQuantity"] : 0,
                                request["ApprCost"].ToString() != "" ? Math.Round((decimal)request["ApprCost"], MidpointRounding.AwayFromZero) : 0,
                                request["ApprCost"].ToString() != "" ? Math.Round((decimal)request["ApprCost"], MidpointRounding.AwayFromZero) : 0,

                                request["Comments"].ToString() != "" ? request["Comments"].ToString() : "",
                                request["RequestorNT"].ToString(),
                                request["SubmitDate"].ToString() != "" ? ((DateTime)request["SubmitDate"]).ToString("dd-MM-yyyy") : string.Empty,
                                request["DHNT"].ToString(),
                                request["DHAppDate"].ToString() != "" ? ((DateTime)request["DHAppDate"]).ToString("dd-MM-yyyy") : string.Empty,
                                request["SHNT"].ToString(),
                                request["SHAppDate"].ToString() != "" ? ((DateTime)request["SHAppDate"]).ToString("dd-MM-yyyy") : string.Empty,
                                !(bool)request["ApprovalDH"],
                                ((bool)request["ApprovalDH"] && !((bool)request["ApprovedDH"])),
                                ((bool)request["ApprovalSH"] && !((bool)request["ApprovedSH"])),
                                request["OrderID"].ToString(),
                                (request["OrderStatus"].ToString().Trim() != "" && request["OrderStatus"].ToString().Trim() != "0") ? BudgetingController.lstOrderStatus.Find(status => status.ID.ToString().Equals(request["OrderStatus"].ToString().Trim())).OrderStatus : "",
                                request["OrderPrice"].ToString() != "" ? decimal.Parse(request["OrderPrice"].ToString()) : 0,
                                   request["Projected_Amount"].ToString() != "" ? Math.Round((decimal)request["Projected_Amount"], MidpointRounding.AwayFromZero) : 0,
                            request["Unused_Amount"].ToString() != "" ? Math.Round((decimal)request["Unused_Amount"], MidpointRounding.AwayFromZero) : 0,
                            request["isProjected"] != null ? request["isProjected"] : false,
                             request["Q1"] != null ? request["Q1"] : false,
                              request["Q2"] != null ? request["Q2"] : false,
                               request["Q3"] != null ? request["Q3"] : false,
                               request["Q4"] != null ? request["Q4"] : false
                                );

                        }

                    }

                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        var ws = wb.Worksheets.Add("VKM_Items_List");
                        ws.Cell(1, 1).Value = "BU Pending Summary";
                        ws.Cell(2, 1).InsertTable(SectionPendingSummaryData(useryear));
                        ws.Cell(9, 1).Value = "Dept Summary(includes all items at L2 & L3 review levels)";
                        ws.Cell(10, 1).InsertTable(DeptSummaryAllData(true, useryear));
                        ws.Cell(17, 1).InsertTable(dt);
                        using (MemoryStream stream = new MemoryStream())
                        {

                            wb.SaveAs(stream);
                            var robj = File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
                            return Json(robj, JsonRequestBehavior.AllowGet);
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                WriteLog("Error - ExportALL : NTID - " + User.Identity.Name.Split('\\')[1].Trim().ToUpper() + ", ErrorMsg : " + ex + ", At - " + DateTime.Now);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }


        //VITHOUT ROUNDING OFF MIDPOINT AVAY FROM ZERO (HERE 85.5 = 85)

        //public ActionResult ExportDataToExcel(string useryear)
        //{


        //    string filename = @"Approval_List_" + DateTime.Now.ToShortDateString() + ".xlsx";

        //    using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //    //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //    {
        //        System.Data.DataTable dt = new System.Data.DataTable("Request_List");
        //        string present = BudgetingController.lstUsers.Find(x => x.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).EmployeeName;
        //        //string presentUserName_2020 = BudgetingController.lstUsers_2020.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).EmployeeName;

        //        if (useryear.Contains("2020"))
        //        {
        //            dt.Columns.AddRange(new DataColumn[34] { new DataColumn("Business Unit"),
        //                                    new DataColumn("OEM"),
        //                                    new DataColumn("Department"),
        //                                    new DataColumn("Group"),
        //                                    new DataColumn("Project"),
        //                                    new DataColumn("Item Name"),
        //                                    new DataColumn("Category"),
        //                                    new DataColumn("Cost Element"),
        //                                    new DataColumn("Unit Price", typeof(decimal)),
        //                                    new DataColumn("Actual Available Quantity"),
        //                                    new DataColumn("Required Quantity", typeof(Int32)),
        //                                    new DataColumn("Total Price", typeof(decimal)),
        //                                    new DataColumn("Reviewed Quantity", typeof(Int32)),
        //                                    new DataColumn("Reviewed Price", typeof(decimal)),
        //                                    new DataColumn("Total Budgeted Cost (USD)", typeof(decimal)),
        //                                    new DataColumn("Comments"),
        //                                    new DataColumn("Requestor"),
        //                                    new DataColumn("Submit Date"),
        //                                    new DataColumn("Reviewer L2"),
        //                                    new DataColumn("L2 Review Date"),
        //                                    new DataColumn("Reviewer L3"),
        //                                    new DataColumn("L3 Review Date"),
        //                                    new DataColumn("L2 Review Status"),
        //                                    new DataColumn("L3 Review Status"),
        //                                    new DataColumn("Required Date"),
        //                                    new DataColumn("Request Order Date"),
        //                                    new DataColumn("Order Date"),
        //                                    new DataColumn("Tentative Delivery Date"),
        //                                    new DataColumn("Actual Delivery Date"),
        //                                    new DataColumn("Fund"),
        //                                    new DataColumn("Order ID"),
        //                                    new DataColumn("Order Status"),
        //                                    new DataColumn("Ordered Quantity",typeof(Int32)),
        //                                    new DataColumn("Order Price",typeof(decimal))

        //        });
        //            if (BudgetingController.lstBU_SPOCs.Find(person => person.VKMspoc.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())) != null &&
        //                BudgetingController.lstBU_SPOCs.Find(person => person.VKMspoc.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).BU <99 && User.Identity.Name.Split('\\')[1].ToUpper() != "DIN2COB")
        //            {
        //                var requests = db.RequestItems_Table.AsEnumerable().Where(x => x.SubmitDate.ToString().Contains(useryear)).Select(x => new
        //                {
        //                    x.BU,
        //                    x.OEM,
        //                    x.DEPT,
        //                    x.Group,
        //                    x.Project,
        //                    x.ItemName,
        //                    x.Category,
        //                    x.CostElement,
        //                    x.UnitPrice,
        //                    x.ActualAvailableQuantity,
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
        //                    x.ApprovedDH,
        //                    x.ApprovedSH,
        //                    x.RequiredDate,
        //                    x.RequestOrderDate,
        //                    x.OrderDate,
        //                    x.TentativeDeliveryDate,
        //                    x.ActualDeliveryDate,
        //                    x.Fund,
        //                    x.OrderID,
        //                    x.OrderStatus,
        //                    x.OrderedQuantity,
        //                    x.OrderPrice

        //                }).ToList().FindAll(x => x.ApprovedDH == true).FindAll(x => x.SHNT.Trim().Equals(present.Trim()) /*|| x.SHNT.Trim().Equals(presentUserName_2020.Trim())*/);
        //                foreach (var request in requests)
        //                {
        //                    string BU = string.Empty;
        //                    if (BudgetingController.lstBUs.Find(bu => bu.ID.ToString().Equals(request.BU)).BU == "MB")
        //                        BU = "AS";
        //                    else if (BudgetingController.lstBUs.Find(bu => bu.ID.ToString().Equals(request.BU)).BU == "OSS")
        //                        BU = "PS";
        //                    else
        //                        BU = BudgetingController.lstBUs.Find(bu => bu.ID.ToString().Equals(request.BU)).BU;



        //                    dt.Rows.Add(
        //                        BU,
        //                        BudgetingController.lstOEMs.Find(oem => oem.ID.ToString().Equals(request.OEM)).OEM,
        //                        BudgetingController.lstDEPTs.Find(dept => dept.ID.ToString().Equals(request.DEPT)).DEPT,
        //                        BudgetingController.lstGroups_test.Find(grp => grp.ID.ToString().Equals(request.Group)).Group,
        //                        request.Project,
        //                        BudgetingController.lstItems.Find(item => item.S_No.ToString().Equals(request.ItemName)).Item_Name,
        //                        BudgetingController.lstPrdCateg.Find(cat => cat.ID.ToString().Equals(request.Category)).Category.Trim(),
        //                        BudgetingController.lstCostElement.Find(cost => cost.ID.ToString().Equals(request.CostElement)).CostElement,
        //                        Math.Round((decimal)request.UnitPrice),
        //                        request.ActualAvailableQuantity != null ? request.ActualAvailableQuantity : "NA",
        //                        (int)request.ReqQuantity,
        //                         Math.Round((decimal)request.TotalPrice),
        //                        (int)request.ApprQuantity,
        //                         Math.Round((decimal)request.ApprCost),
        //                         Math.Round((decimal)request.ApprCost),
        //                        request.Comments,
        //                        request.RequestorNT,
        //                        request.SubmitDate.HasValue ? request.SubmitDate.Value.ToString("dd-MM-yyyy") : "",
        //                        request.DHNT,
        //                        request.DHAppDate.HasValue ? request.DHAppDate.Value.ToString("dd-MM-yyyy") : "",
        //                        request.SHNT,
        //                        request.SHAppDate.HasValue ? request.SHAppDate.Value.ToString("dd-MM-yyyy") : "",
        //                        request.ApprovedDH,
        //                        request.ApprovedSH,
        //                        request.RequiredDate.HasValue ? request.RequiredDate.Value.ToString("dd-MM-yyyy") : "",
        //                        request.RequestOrderDate.HasValue ? request.RequestOrderDate.Value.ToString("dd-MM-yyyy") : "",
        //                        request.OrderDate.HasValue ? request.OrderDate.Value.ToString("dd-MM-yyyy") : "",
        //                        request.TentativeDeliveryDate.HasValue ? request.TentativeDeliveryDate.Value.ToString("dd-MM-yyyy") : "",
        //                        request.ActualDeliveryDate.HasValue ? request.ActualDeliveryDate.Value.ToString("dd-MM-yyyy") : "",
        //                        request.Fund != null ? BudgetingController.lstFund.Find(fun => fun.ID.ToString().Equals(request.Fund)).Fund : BudgetingController.lstFund.Find(fun => fun.Fund.Trim().Equals("F02")).Fund,
        //                        request.OrderID,
        //                        (request.OrderStatus != null && request.OrderStatus.Trim() != "" && request.OrderStatus.Trim() != "0") ? BudgetingController.lstOrderStatus.Find(status => status.ID.ToString().Equals(request.OrderStatus.Trim())).OrderStatus : "",
        //                        request.OrderedQuantity != null ? (int)request.OrderedQuantity : (int)0,
        //                        request.OrderPrice);


        //                }
        //            }
        //            else
        //            {
        //                var requests = db.RequestItems_Table.AsEnumerable().Where(x => x.SubmitDate.ToString().Contains(useryear)).Select(x => new
        //                {
        //                    x.BU,
        //                    x.OEM,
        //                    x.DEPT,
        //                    x.Group,
        //                    x.Project,
        //                    x.ItemName,
        //                    x.Category,
        //                    x.CostElement,
        //                    x.UnitPrice,
        //                    x.ActualAvailableQuantity,
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
        //                    x.ApprovedDH,
        //                    x.ApprovedSH,
        //                    x.RequiredDate,
        //                    x.RequestOrderDate,
        //                    x.OrderDate,
        //                    x.TentativeDeliveryDate,
        //                    x.ActualDeliveryDate,
        //                    x.Fund,
        //                    x.OrderID,
        //                    x.OrderStatus,
        //                    x.OrderedQuantity,
        //                    x.OrderPrice

        //                }).ToList().FindAll(x => x.ApprovedDH == true);
        //                foreach (var request in requests)
        //                {
        //                    string BU = string.Empty;
        //                    if (BudgetingController.lstBUs.Find(bu => bu.ID.ToString().Equals(request.BU)).BU == "MB")
        //                        BU = "AS";
        //                    else if (BudgetingController.lstBUs.Find(bu => bu.ID.ToString().Equals(request.BU)).BU == "OSS")
        //                        BU = "PS";
        //                    else
        //                        BU = BudgetingController.lstBUs.Find(bu => bu.ID.ToString().Equals(request.BU)).BU;


        //                    dt.Rows.Add(
        //                        BU,
        //                        BudgetingController.lstOEMs.Find(oem => oem.ID.ToString().Equals(request.OEM)).OEM,
        //                        BudgetingController.lstDEPTs.Find(dept => dept.ID.ToString().Equals(request.DEPT)).DEPT,
        //                        BudgetingController.lstGroups_test.Find(grp => grp.ID.ToString().Equals(request.Group)).Group,
        //                        request.Project,
        //                        BudgetingController.lstItems.Find(item => item.S_No.ToString().Equals(request.ItemName)).Item_Name,
        //                        BudgetingController.lstPrdCateg.Find(cat => cat.ID.ToString().Equals(request.Category)).Category.Trim(),
        //                        BudgetingController.lstCostElement.Find(cost => cost.ID.ToString().Equals(request.CostElement)).CostElement,
        //                        Math.Round((decimal)request.UnitPrice),
        //                        request.ActualAvailableQuantity != null ? request.ActualAvailableQuantity : "NA",
        //                        (int)request.ReqQuantity,
        //                         Math.Round((decimal)request.TotalPrice),
        //                        (int)request.ApprQuantity,
        //                         Math.Round((decimal)request.ApprCost),
        //                         Math.Round((decimal)request.ApprCost),
        //                        request.Comments,
        //                        request.RequestorNT,
        //                        request.SubmitDate.HasValue ? request.SubmitDate.Value.ToString("dd-MM-yyyy") : "",
        //                        request.DHNT,
        //                        request.DHAppDate.HasValue ? request.DHAppDate.Value.ToString("dd-MM-yyyy") : "",
        //                        request.SHNT,
        //                        request.SHAppDate.HasValue ? request.SHAppDate.Value.ToString("dd-MM-yyyy") : "",
        //                        request.ApprovedDH,
        //                        request.ApprovedSH,
        //                        request.RequiredDate.HasValue ? request.RequiredDate.Value.ToString("dd-MM-yyyy") : "",
        //                        request.RequestOrderDate.HasValue ? request.RequestOrderDate.Value.ToString("dd-MM-yyyy") : "",
        //                        request.OrderDate.HasValue ? request.OrderDate.Value.ToString("dd-MM-yyyy") : "",
        //                        request.TentativeDeliveryDate.HasValue ? request.TentativeDeliveryDate.Value.ToString("dd-MM-yyyy") : "",
        //                        request.ActualDeliveryDate.HasValue ? request.ActualDeliveryDate.Value.ToString("dd-MM-yyyy") : "",
        //                        request.Fund != null ? BudgetingController.lstFund.Find(fun => fun.ID.ToString().Equals(request.Fund)).Fund : BudgetingController.lstFund.Find(fun => fun.Fund.Trim().Equals("F02")).Fund,
        //                        request.OrderID,
        //                        (request.OrderStatus != null && request.OrderStatus.Trim() != "" && request.OrderStatus.Trim() != "0") ? BudgetingController.lstOrderStatus.Find(status => status.ID.ToString().Equals(request.OrderStatus.Trim())).OrderStatus : "",
        //                        request.OrderedQuantity != null ? (int)request.OrderedQuantity : (int)0,
        //                        request.OrderPrice);
        //                }
        //            }
        //            using (XLWorkbook wb = new XLWorkbook())
        //            {
        //                var ws = wb.Worksheets.Add("VKM_Items_List_2020");
        //                ws.Cell(2, 1).Value = "Dept Summary";
        //                ws.Cell(3, 1).InsertTable(DeptSummaryData(true, useryear));
        //                ws.Cell(8, 1).Value = "Section Summary";
        //                ws.Cell(9, 1).InsertTable(SectionSummaryData(useryear));
        //                ws.Cell(15, 1).InsertTable(dt);
        //                using (MemoryStream stream = new MemoryStream())
        //                {

        //                    wb.SaveAs(stream);
        //                    var robj = File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
        //                    return Json(robj, JsonRequestBehavior.AllowGet);
        //                }
        //            }

        //        }


        //        string presentUserDept = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).Department;
        //        string is_CCXC = string.Empty;
        //        if (presentUserDept.Contains("XC"))
        //            is_CCXC = "XC";
        //        else
        //            is_CCXC = "CC";

        //        if (BudgetingController.lstBU_SPOCs.Find(person => person.VKMspoc.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())) == null) //Can be Lab Admins / Other VKM Authorized users
        //        {


        //            dt.Columns.AddRange(new DataColumn[34] { new DataColumn("Business Unit"),
        //                                    new DataColumn("OEM"),
        //                                    new DataColumn("Department"),
        //                                    new DataColumn("Group"),
        //                                    new DataColumn("Project"),
        //                                    new DataColumn("Item Name"),
        //                                    new DataColumn("Category"),
        //                                    new DataColumn("Cost Element"),
        //                                    new DataColumn("Unit Price",typeof(decimal)),
        //                                    new DataColumn("Actual Available Quantity"),
        //                                    new DataColumn("Required Quantity",typeof(Int32)),
        //                                    new DataColumn("Total Price",typeof(decimal)),
        //                                    new DataColumn("Reviewed Quantity",typeof(Int32)),
        //                                    new DataColumn("Reviewed Price",typeof(decimal)),
        //                                    new DataColumn("Total Budgeted Cost (USD)",typeof(decimal)),
        //                                    new DataColumn("Comments"),
        //                                    new DataColumn("Requestor"),
        //                                    new DataColumn("Submit Date"),
        //                                    new DataColumn("Reviewer L2"),
        //                                    new DataColumn("L2 Review Date"),
        //                                    new DataColumn("Reviewer L3"),
        //                                    new DataColumn("L3 Review Date"),
        //                                    new DataColumn("L2 Review Status"),
        //                                    new DataColumn("L3 Review Status"),
        //                                    new DataColumn("Required Date"),
        //                                    new DataColumn("Request Order Date"),
        //                                    new DataColumn("Order Date"),
        //                                    new DataColumn("Tentative Delivery Date"),
        //                                    new DataColumn("Actual Delivery Date"),
        //                                    new DataColumn("Fund"),
        //                                    new DataColumn("Order ID"),
        //                                    new DataColumn("Order Status"),
        //                                    new DataColumn("Ordered Quantity",typeof(Int32)),
        //                                    new DataColumn("Order Price",typeof(decimal))

        //        });
        //            if(BudgetingController.lstPrivileged.FindAll(e => e.ADSID.ToUpper().Trim().Equals(User.Identity.Name.Split('\\')[1].ToUpper().Trim())).Count() != 0) //Lab admin
        //            {
        //                var purspoc = BudgetingController.lstPrivileged.FindAll(e => e.ADSID.ToUpper().Trim().Equals(User.Identity.Name.Split('\\')[1].ToUpper()));

        //                var BU_of_PurchaseSPOC = BudgetingController.lstPrivileged.Find(e => e.ADSID.ToUpper().Trim().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).BU;
        //                List<string> allowedBUs = new List<string>();
        //                if (BU_of_PurchaseSPOC != null)
        //                {
        //                    allowedBUs = (BU_of_PurchaseSPOC.Split(',')).ToList();

        //                }

        //                if (presentUserDept.Contains("XC"))
        //                {
        //                    var requests = db.RequestItems_Table.AsEnumerable().Where(x => x.SubmitDate.ToString().Contains(useryear)).Select(x => new
        //                    {
        //                        x.BU,
        //                        x.OEM,
        //                        x.DEPT,
        //                        x.Group,
        //                        x.Project,
        //                        x.ItemName,
        //                        x.Category,
        //                        x.CostElement,
        //                        x.UnitPrice,
        //                        x.ActualAvailableQuantity,
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
        //                        x.ApprovedDH,
        //                        x.ApprovedSH,
        //                        x.RequiredDate,
        //                        x.RequestOrderDate,
        //                        x.OrderDate,
        //                        x.TentativeDeliveryDate,
        //                        x.ActualDeliveryDate,
        //                        x.Fund,
        //                        x.OrderID,
        //                        x.OrderStatus,
        //                        x.OrderedQuantity,
        //                        x.OrderPrice


        //                    }).ToList().FindAll(x => x.ApprovedDH == true).FindAll(dpt => BudgetingController.lstDEPTs.Find(x => x.ID.ToString().Contains(dpt.DEPT)).DEPT.Contains("XC"));

        //                    foreach (var request in requests)
        //                    {
        //                        if (allowedBUs.Contains(request.BU))
        //                        {


        //                            dt.Rows.Add(
        //                                BudgetingController.lstBUs.Find(bu => bu.ID.ToString().Equals(request.BU)).BU,
        //                                BudgetingController.lstOEMs.Find(oem => oem.ID.ToString().Equals(request.OEM)).OEM,
        //                                BudgetingController.lstDEPTs.Find(dept => dept.ID.ToString().Equals(request.DEPT)).DEPT,
        //                                BudgetingController.lstGroups_test.Find(grp => grp.ID.ToString().Equals(request.Group)).Group,
        //                                request.Project,
        //                                BudgetingController.lstItems.Find(item => item.S_No.ToString().Equals(request.ItemName)).Item_Name,
        //                                BudgetingController.lstPrdCateg.Find(cat => cat.ID.ToString().Equals(request.Category)).Category.Trim(),
        //                                BudgetingController.lstCostElement.Find(cost => cost.ID.ToString().Equals(request.CostElement)).CostElement,
        //                                Math.Round((decimal)request.UnitPrice),
        //                                request.ActualAvailableQuantity != null ? request.ActualAvailableQuantity : "NA",
        //                                (int)request.ReqQuantity,
        //                                Math.Round((decimal)request.TotalPrice),
        //                                (int)request.ApprQuantity,
        //                                 Math.Round((decimal)request.ApprCost),
        //                                 Math.Round((decimal)request.ApprCost),
        //                                request.Comments,
        //                                request.RequestorNT,
        //                                request.SubmitDate.HasValue ? request.SubmitDate.Value.ToString("dd-MM-yyyy") : "",
        //                                request.DHNT,
        //                                request.DHAppDate.HasValue ? request.DHAppDate.Value.ToString("dd-MM-yyyy") : "",
        //                                request.SHNT,
        //                                request.SHAppDate.HasValue ? request.SHAppDate.Value.ToString("dd-MM-yyyy") : "",
        //                                request.ApprovedDH,
        //                                request.ApprovedSH,
        //                                request.RequiredDate.HasValue ? request.RequiredDate.Value.ToString("dd-MM-yyyy") : "",
        //                                request.RequestOrderDate.HasValue ? request.RequestOrderDate.Value.ToString("dd-MM-yyyy") : "",
        //                                request.OrderDate.HasValue ? request.OrderDate.Value.ToString("dd-MM-yyyy") : "",
        //                                request.TentativeDeliveryDate.HasValue ? request.TentativeDeliveryDate.Value.ToString("dd-MM-yyyy") : "",
        //                                request.ActualDeliveryDate.HasValue ? request.ActualDeliveryDate.Value.ToString("dd-MM-yyyy") : "",
        //                                request.Fund != null ? BudgetingController.lstFund.Find(fun => fun.ID.ToString().Equals(request.Fund)).Fund : BudgetingController.lstFund.Find(fun => fun.Fund.Trim().Equals("F02")).Fund

        //                                ,request.OrderID,
        //                                (request.OrderStatus != null && request.OrderStatus.Trim() != "" && request.OrderStatus.Trim() != "0") ? BudgetingController.lstOrderStatus.Find(status => status.ID.ToString().Equals(request.OrderStatus.Trim())).OrderStatus : "",
        //                                request.OrderedQuantity != null ? (int)request.OrderedQuantity : (int)0,
        //                                request.OrderPrice
        //                                );
        //                        }


        //                    }

        //                }

        //                else
        //                {
        //                    var requests = db.RequestItems_Table.AsEnumerable().Where(x => x.SubmitDate.ToString().Contains(useryear)).Select(x => new
        //                    {
        //                        x.BU,
        //                        x.OEM,
        //                        x.DEPT,
        //                        x.Group,
        //                        x.Project,
        //                        x.ItemName,
        //                        x.Category,
        //                        x.CostElement,
        //                        x.UnitPrice,
        //                        x.ActualAvailableQuantity,
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
        //                        x.ApprovedDH,
        //                        x.ApprovedSH,
        //                        x.RequiredDate,
        //                        x.RequestOrderDate,
        //                        x.OrderDate,
        //                        x.TentativeDeliveryDate,
        //                        x.ActualDeliveryDate,
        //                        x.Fund,
        //                        x.OrderID,
        //                        x.OrderStatus,
        //                        x.OrderedQuantity,
        //                        x.OrderPrice


        //                    }).ToList().FindAll(x => x.ApprovedDH == true).FindAll(dpt => !BudgetingController.lstDEPTs.Find(x => x.ID.ToString().Contains(dpt.DEPT)).DEPT.Contains("XC"));

        //                    foreach (var request in requests)
        //                    {
        //                        if (allowedBUs.Contains(request.BU))
        //                        {


        //                            dt.Rows.Add(
        //                                BudgetingController.lstBUs.Find(bu => bu.ID.ToString().Equals(request.BU)).BU,
        //                                BudgetingController.lstOEMs.Find(oem => oem.ID.ToString().Equals(request.OEM)).OEM,
        //                                BudgetingController.lstDEPTs.Find(dept => dept.ID.ToString().Equals(request.DEPT)).DEPT,
        //                                BudgetingController.lstGroups_test.Find(grp => grp.ID.ToString().Equals(request.Group)).Group,
        //                                request.Project,
        //                                BudgetingController.lstItems.Find(item => item.S_No.ToString().Equals(request.ItemName)).Item_Name,
        //                                BudgetingController.lstPrdCateg.Find(cat => cat.ID.ToString().Equals(request.Category)).Category.Trim(),
        //                                BudgetingController.lstCostElement.Find(cost => cost.ID.ToString().Equals(request.CostElement)).CostElement,
        //                                Math.Round((decimal)request.UnitPrice),
        //                                request.ActualAvailableQuantity != null ? request.ActualAvailableQuantity : "NA",
        //                                (int)request.ReqQuantity,
        //                                Math.Round((decimal)request.TotalPrice),
        //                                (int)request.ApprQuantity,
        //                                 Math.Round((decimal)request.ApprCost),
        //                                 Math.Round((decimal)request.ApprCost),
        //                                request.Comments,
        //                                request.RequestorNT,
        //                                request.SubmitDate.HasValue ? request.SubmitDate.Value.ToString("dd-MM-yyyy") : "",
        //                                request.DHNT,
        //                                request.DHAppDate.HasValue ? request.DHAppDate.Value.ToString("dd-MM-yyyy") : "",
        //                                request.SHNT,
        //                                request.SHAppDate.HasValue ? request.SHAppDate.Value.ToString("dd-MM-yyyy") : "",
        //                                request.ApprovedDH,
        //                                request.ApprovedSH,
        //                                request.RequiredDate.HasValue ? request.RequiredDate.Value.ToString("dd-MM-yyyy") : "",
        //                                request.RequestOrderDate.HasValue ? request.RequestOrderDate.Value.ToString("dd-MM-yyyy") : "",
        //                                request.OrderDate.HasValue ? request.OrderDate.Value.ToString("dd-MM-yyyy") : "",
        //                                request.TentativeDeliveryDate.HasValue ? request.TentativeDeliveryDate.Value.ToString("dd-MM-yyyy") : "",
        //                                request.ActualDeliveryDate.HasValue ? request.ActualDeliveryDate.Value.ToString("dd-MM-yyyy") : "",
        //                                request.Fund != null ? BudgetingController.lstFund.Find(fun => fun.ID.ToString().Equals(request.Fund)).Fund : BudgetingController.lstFund.Find(fun => fun.Fund.Trim().Equals("F02")).Fund

        //                                ,request.OrderID,
        //                                (request.OrderStatus != null && request.OrderStatus.Trim() != "" && request.OrderStatus.Trim() != "0") ? BudgetingController.lstOrderStatus.Find(status => status.ID.ToString().Equals(request.OrderStatus.Trim())).OrderStatus : "",
        //                                request.OrderedQuantity != null ? (int)request.OrderedQuantity : (int)0,
        //                                request.OrderPrice
        //                                );
        //                        }


        //                    }

        //                }



        //            }
        //            else //Other Authorized Users of VKM SPOC view
        //            {
        //                if (presentUserDept.Contains("XC"))  //CCXC segregation based on XC and not XC instead of CC XC is due to the presence of dept like EB-2WP in CC
        //                {
        //                    var requests = db.RequestItems_Table.AsEnumerable().Where(x => x.SubmitDate.ToString().Contains(useryear)).Select(x => new
        //                    {
        //                        x.BU,
        //                        x.OEM,
        //                        x.DEPT,
        //                        x.Group,
        //                        x.Project,
        //                        x.ItemName,
        //                        x.Category,
        //                        x.CostElement,
        //                        x.UnitPrice,
        //                        x.ActualAvailableQuantity,
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
        //                        x.ApprovedDH,
        //                        x.ApprovedSH,
        //                        x.RequiredDate,
        //                        x.RequestOrderDate,
        //                        x.OrderDate,
        //                        x.TentativeDeliveryDate,
        //                        x.ActualDeliveryDate,
        //                        x.Fund,
        //                        x.OrderID,
        //                        x.OrderStatus,
        //                        x.OrderedQuantity,
        //                        x.OrderPrice


        //                    }).ToList().FindAll(x => x.ApprovedDH == true).FindAll(dpt => BudgetingController.lstDEPTs.Find(x => x.ID.ToString().Contains(dpt.DEPT)).DEPT.Contains("XC"));
        //                    ;
        //                    foreach (var request in requests)
        //                    {


        //                        dt.Rows.Add(
        //                            BudgetingController.lstBUs.Find(bu => bu.ID.ToString().Equals(request.BU)).BU,
        //                            BudgetingController.lstOEMs.Find(oem => oem.ID.ToString().Equals(request.OEM)).OEM,
        //                            BudgetingController.lstDEPTs.Find(dept => dept.ID.ToString().Equals(request.DEPT)).DEPT,
        //                            BudgetingController.lstGroups_test.Find(grp => grp.ID.ToString().Equals(request.Group)).Group,
        //                            request.Project,
        //                            BudgetingController.lstItems.Find(item => item.S_No.ToString().Equals(request.ItemName)).Item_Name,
        //                            BudgetingController.lstPrdCateg.Find(cat => cat.ID.ToString().Equals(request.Category)).Category.Trim(),
        //                            BudgetingController.lstCostElement.Find(cost => cost.ID.ToString().Equals(request.CostElement)).CostElement,
        //                            Math.Round((decimal)request.UnitPrice),
        //                            request.ActualAvailableQuantity != null ? request.ActualAvailableQuantity : "NA",
        //                            (int)request.ReqQuantity,
        //                            Math.Round((decimal)request.TotalPrice),
        //                            (int)request.ApprQuantity,
        //                             Math.Round((decimal)request.ApprCost),
        //                             Math.Round((decimal)request.ApprCost),
        //                            request.Comments,
        //                            request.RequestorNT,
        //                            request.SubmitDate.HasValue ? request.SubmitDate.Value.ToString("dd-MM-yyyy") : "",
        //                            request.DHNT,
        //                            request.DHAppDate.HasValue ? request.DHAppDate.Value.ToString("dd-MM-yyyy") : "",
        //                            request.SHNT,
        //                            request.SHAppDate.HasValue ? request.SHAppDate.Value.ToString("dd-MM-yyyy") : "",
        //                            request.ApprovedDH,
        //                            request.ApprovedSH,
        //                            request.RequiredDate.HasValue ? request.RequiredDate.Value.ToString("dd-MM-yyyy") : "",
        //                            request.RequestOrderDate.HasValue ? request.RequestOrderDate.Value.ToString("dd-MM-yyyy") : "",
        //                            request.OrderDate.HasValue ? request.OrderDate.Value.ToString("dd-MM-yyyy") : "",
        //                            request.TentativeDeliveryDate.HasValue ? request.TentativeDeliveryDate.Value.ToString("dd-MM-yyyy") : "",
        //                            request.ActualDeliveryDate.HasValue ? request.ActualDeliveryDate.Value.ToString("dd-MM-yyyy") : "",
        //                            request.Fund != null ? BudgetingController.lstFund.Find(fun => fun.ID.ToString().Equals(request.Fund)).Fund : BudgetingController.lstFund.Find(fun => fun.Fund.Trim().Equals("F02")).Fund

        //                            ,request.OrderID,
        //                            (request.OrderStatus != null && request.OrderStatus.Trim() != "" && request.OrderStatus.Trim() != "0") ? BudgetingController.lstOrderStatus.Find(status => status.ID.ToString().Equals(request.OrderStatus.Trim())).OrderStatus : "",
        //                            request.OrderedQuantity != null ? (int)request.OrderedQuantity : (int)0,
        //                            request.OrderPrice
        //                            );

        //                    }
        //                }

        //                else
        //                {
        //                    var requests = db.RequestItems_Table.AsEnumerable().Where(x => x.SubmitDate.ToString().Contains(useryear)).Select(x => new
        //                {
        //                    x.BU,
        //                    x.OEM,
        //                    x.DEPT,
        //                    x.Group,
        //                    x.Project,
        //                    x.ItemName,
        //                    x.Category,
        //                    x.CostElement,
        //                    x.UnitPrice,
        //                    x.ActualAvailableQuantity,
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
        //                    x.ApprovedDH,
        //                    x.ApprovedSH,
        //                    x.RequiredDate,
        //                    x.RequestOrderDate,
        //                    x.OrderDate,
        //                    x.TentativeDeliveryDate,
        //                    x.ActualDeliveryDate,
        //                    x.Fund,
        //                    x.OrderID,
        //                    x.OrderStatus,
        //                    x.OrderedQuantity,
        //                    x.OrderPrice


        //                }).ToList().FindAll(x => x.ApprovedDH == true).FindAll(dpt => !BudgetingController.lstDEPTs.Find(x => x.ID.ToString().Contains(dpt.DEPT)).DEPT.Contains("XC"));
        //                ;
        //                foreach (var request in requests)
        //                {


        //                        dt.Rows.Add(
        //                            BudgetingController.lstBUs.Find(bu => bu.ID.ToString().Equals(request.BU)).BU,
        //                            BudgetingController.lstOEMs.Find(oem => oem.ID.ToString().Equals(request.OEM)).OEM,
        //                            BudgetingController.lstDEPTs.Find(dept => dept.ID.ToString().Equals(request.DEPT)).DEPT,
        //                            BudgetingController.lstGroups_test.Find(grp => grp.ID.ToString().Equals(request.Group)).Group,
        //                            request.Project,
        //                            BudgetingController.lstItems.Find(item => item.S_No.ToString().Equals(request.ItemName)).Item_Name,
        //                            BudgetingController.lstPrdCateg.Find(cat => cat.ID.ToString().Equals(request.Category)).Category.Trim(),
        //                            BudgetingController.lstCostElement.Find(cost => cost.ID.ToString().Equals(request.CostElement)).CostElement,
        //                            Math.Round((decimal)request.UnitPrice),
        //                            request.ActualAvailableQuantity != null ? request.ActualAvailableQuantity : "NA",
        //                            (int)request.ReqQuantity,
        //                            Math.Round((decimal)request.TotalPrice),
        //                            (int)request.ApprQuantity,
        //                             Math.Round((decimal)request.ApprCost),
        //                             Math.Round((decimal)request.ApprCost),
        //                            request.Comments,
        //                            request.RequestorNT,
        //                            request.SubmitDate.HasValue ? request.SubmitDate.Value.ToString("dd-MM-yyyy") : "",
        //                            request.DHNT,
        //                            request.DHAppDate.HasValue ? request.DHAppDate.Value.ToString("dd-MM-yyyy") : "",
        //                            request.SHNT,
        //                            request.SHAppDate.HasValue ? request.SHAppDate.Value.ToString("dd-MM-yyyy") : "",
        //                            request.ApprovedDH,
        //                            request.ApprovedSH,
        //                            request.RequiredDate.HasValue ? request.RequiredDate.Value.ToString("dd-MM-yyyy") : "",
        //                            request.RequestOrderDate.HasValue ? request.RequestOrderDate.Value.ToString("dd-MM-yyyy") : "",
        //                            request.OrderDate.HasValue ? request.OrderDate.Value.ToString("dd-MM-yyyy") : "",
        //                            request.TentativeDeliveryDate.HasValue ? request.TentativeDeliveryDate.Value.ToString("dd-MM-yyyy") : "",
        //                            request.ActualDeliveryDate.HasValue ? request.ActualDeliveryDate.Value.ToString("dd-MM-yyyy") : "",
        //                            request.Fund != null ? BudgetingController.lstFund.Find(fun => fun.ID.ToString().Equals(request.Fund)).Fund : BudgetingController.lstFund.Find(fun => fun.Fund.Trim().Equals("F02")).Fund

        //                            ,request.OrderID,
        //                            (request.OrderStatus != null && request.OrderStatus.Trim() != "" && request.OrderStatus.Trim() != "0") ? BudgetingController.lstOrderStatus.Find(status => status.ID.ToString().Equals(request.OrderStatus.Trim())).OrderStatus : "",
        //                            request.OrderedQuantity != null ? (int)request.OrderedQuantity : (int)0,
        //                            request.OrderPrice
        //                            );

        //                }
        //            }

        //                }


        //        }
        //        else // VKM SPOC or VKM Admin of CC/XC
        //        {

        //            dt.Columns.AddRange(new DataColumn[27] { new DataColumn("Business Unit"),
        //                                    new DataColumn("OEM"),
        //                                    new DataColumn("Department"),
        //                                    new DataColumn("Group"),
        //                                    new DataColumn("Project"),
        //                                    new DataColumn("Item Name"),
        //                                    new DataColumn("Category"),
        //                                    new DataColumn("Cost Element"),
        //                                    new DataColumn("Unit Price",typeof(decimal)),
        //                                    new DataColumn("Actual Available Quantity"),
        //                                    new DataColumn("Required Quantity",typeof(Int32)),
        //                                    new DataColumn("Total Price",typeof(decimal)),
        //                                    new DataColumn("Reviewed Quantity",typeof(Int32)),
        //                                    new DataColumn("Reviewed Price",typeof(decimal)),
        //                                    new DataColumn("Total Budgeted Cost (USD)",typeof(decimal)),
        //                                    new DataColumn("Comments"),
        //                                    new DataColumn("Requestor"),
        //                                    new DataColumn("Submit Date"),
        //                                    new DataColumn("Reviewer L2"),
        //                                    new DataColumn("L2 Review Date"),
        //                                    new DataColumn("Reviewer L3"),
        //                                    new DataColumn("L3 Review Date"),
        //                                    new DataColumn("L2 Review Status"),
        //                                    new DataColumn("L3 Review Status"),
        //                                    new DataColumn("Order ID"),
        //                                    new DataColumn("Order Status"),
        //                                    new DataColumn("Order Price",typeof(decimal))

        //        });
        //            var BU = BudgetingController.lstBU_SPOCs.Find(person => person.VKMspoc.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).BU;
        //            //IEnumerable<RequestItems_Table> requests = new IEnumerable<RequestItems_Table>();
        //            //var requests = db.RequestItems_Table.AsEnumerable().ToList();
        //            //List<dynamic> reques = new List<dynamic>();
        //            if (BU >= 99 || User.Identity.Name.Split('\\')[1].ToUpper() == "DIN2COB")
        //            {//VKM ADMIN and din2cob
        //                if (presentUserDept.Contains("XC"))
        //                {
        //                    var requests = db.RequestItems_Table.AsEnumerable().Where(x => x.SubmitDate.ToString().Contains(useryear)).Select(x => new
        //                    {
        //                        x.BU,
        //                        x.OEM,
        //                        x.DEPT,
        //                        x.Group,
        //                        x.Project,
        //                        x.ItemName,
        //                        x.Category,
        //                        x.CostElement,
        //                        x.UnitPrice,
        //                        x.ActualAvailableQuantity,
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
        //                        x.ApprovedDH,
        //                        x.ApprovedSH,
        //                        x.OrderID,
        //                        x.OrderStatus,
        //                        x.OrderPrice

        //                    }).ToList().FindAll(x => x.ApprovedDH == true).FindAll(dpt => BudgetingController.lstDEPTs.Find(x => x.ID.ToString().Contains(dpt.DEPT)).DEPT.Contains("XC"));
        //                    ;
        //                    //IEnumerable<RequestItems_Table> xi = (IEnumerable<RequestItems_Table>)requests.ToList();
        //                    //reques = (List<dynamic>)xi;

        //                    foreach (var request in requests)
        //                    {

        //                        dt.Rows.Add(
        //                            BudgetingController.lstBUs.Find(bu => bu.ID.ToString().Equals(request.BU)).BU,
        //                            BudgetingController.lstOEMs.Find(oem => oem.ID.ToString().Equals(request.OEM)).OEM,
        //                            BudgetingController.lstDEPTs.Find(dept => dept.ID.ToString().Equals(request.DEPT)).DEPT,
        //                            BudgetingController.lstGroups_test.Find(grp => grp.ID.ToString().Equals(request.Group)).Group,
        //                            request.Project,
        //                            BudgetingController.lstItems.Find(item => item.S_No.ToString().Equals(request.ItemName)).Item_Name,
        //                            BudgetingController.lstPrdCateg.Find(cat => cat.ID.ToString().Equals(request.Category)).Category.Trim(),
        //                            BudgetingController.lstCostElement.Find(cost => cost.ID.ToString().Equals(request.CostElement)).CostElement,
        //                            Math.Round((decimal)request.UnitPrice),
        //                            request.ActualAvailableQuantity != null ? request.ActualAvailableQuantity : "NA",
        //                            (int)request.ReqQuantity,
        //                             Math.Round((decimal)request.TotalPrice),
        //                            (int)request.ApprQuantity,
        //                             Math.Round((decimal)request.ApprCost),
        //                             Math.Round((decimal)request.ApprCost),
        //                            request.Comments,
        //                            request.RequestorNT,
        //                            request.SubmitDate.HasValue ? request.SubmitDate.Value.ToString("dd-MM-yyyy") : "",
        //                            request.DHNT,
        //                            request.DHAppDate.HasValue ? request.DHAppDate.Value.ToString("dd-MM-yyyy") : "",
        //                            request.SHNT,
        //                            request.SHAppDate.HasValue ? request.SHAppDate.Value.ToString("dd-MM-yyyy") : "",
        //                            request.ApprovedDH,
        //                            request.ApprovedSH,
        //                            request.OrderID,
        //                            (request.OrderStatus != null && request.OrderStatus.Trim() != "" && request.OrderStatus.Trim() != "0") ? BudgetingController.lstOrderStatus.Find(status => status.ID.ToString().Equals(request.OrderStatus.Trim())).OrderStatus : "",
        //                            request.OrderPrice
        //                            );

        //                    }
        //                }
        //                else
        //                {
        //                    var requests = db.RequestItems_Table.AsEnumerable().Where(x => x.SubmitDate.ToString().Contains(useryear)).Select(x => new
        //                    {
        //                        x.BU,
        //                        x.OEM,
        //                        x.DEPT,
        //                        x.Group,
        //                        x.Project,
        //                        x.ItemName,
        //                        x.Category,
        //                        x.CostElement,
        //                        x.UnitPrice,
        //                        x.ActualAvailableQuantity,
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
        //                        x.ApprovedDH,
        //                        x.ApprovedSH,
        //                        x.OrderID,
        //                        x.OrderStatus,
        //                        x.OrderPrice

        //                    }).ToList().FindAll(x => x.ApprovedDH == true).FindAll(dpt => !BudgetingController.lstDEPTs.Find(x => x.ID.ToString().Contains(dpt.DEPT)).DEPT.Contains("XC"));

        //                    //IEnumerable<RequestItems_Table> xi = (IEnumerable<RequestItems_Table>)requests.ToList();
        //                    //reques = (List<dynamic>)xi;

        //                    foreach (var request in requests)
        //                    {

        //                        dt.Rows.Add(
        //                            BudgetingController.lstBUs.Find(bu => bu.ID.ToString().Equals(request.BU)).BU,
        //                            BudgetingController.lstOEMs.Find(oem => oem.ID.ToString().Equals(request.OEM)).OEM,
        //                            BudgetingController.lstDEPTs.Find(dept => dept.ID.ToString().Equals(request.DEPT)).DEPT,
        //                            BudgetingController.lstGroups_test.Find(grp => grp.ID.ToString().Equals(request.Group)).Group,
        //                            request.Project,
        //                            BudgetingController.lstItems.Find(item => item.S_No.ToString().Equals(request.ItemName)).Item_Name,
        //                            BudgetingController.lstPrdCateg.Find(cat => cat.ID.ToString().Equals(request.Category)).Category.Trim(),
        //                            BudgetingController.lstCostElement.Find(cost => cost.ID.ToString().Equals(request.CostElement)).CostElement,
        //                            Math.Round((decimal)request.UnitPrice),
        //                            request.ActualAvailableQuantity != null ? request.ActualAvailableQuantity : "NA",
        //                            (int)request.ReqQuantity,
        //                             Math.Round((decimal)request.TotalPrice),
        //                            (int)request.ApprQuantity,
        //                             Math.Round((decimal)request.ApprCost),
        //                             Math.Round((decimal)request.ApprCost),
        //                            request.Comments,
        //                            request.RequestorNT,
        //                            request.SubmitDate.HasValue ? request.SubmitDate.Value.ToString("dd-MM-yyyy") : "",
        //                            request.DHNT,
        //                            request.DHAppDate.HasValue ? request.DHAppDate.Value.ToString("dd-MM-yyyy") : "",
        //                            request.SHNT,
        //                            request.SHAppDate.HasValue ? request.SHAppDate.Value.ToString("dd-MM-yyyy") : "",
        //                            request.ApprovedDH,
        //                            request.ApprovedSH,
        //                            request.OrderID,
        //                            (request.OrderStatus != null && request.OrderStatus.Trim() != "" && request.OrderStatus.Trim() != "0") ? BudgetingController.lstOrderStatus.Find(status => status.ID.ToString().Equals(request.OrderStatus.Trim())).OrderStatus : "",
        //                            request.OrderPrice
        //                            );

        //                    }
        //                }

        //            }
        //            else
        //            {//VKM SPOC - other than din2cob
        //               var requests = db.RequestItems_Table.AsEnumerable().Where(x => x.SubmitDate.ToString().Contains(useryear)).Select(x => new
        //                {
        //                    x.BU,
        //                    x.OEM,
        //                    x.DEPT,
        //                    x.Group,
        //                    x.Project,
        //                    x.ItemName,
        //                    x.Category,
        //                    x.CostElement,
        //                    x.UnitPrice,
        //                    x.ActualAvailableQuantity,
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
        //                    x.ApprovedDH,
        //                    x.ApprovedSH,
        //                    x.OrderID,
        //                   x.OrderStatus,
        //                   x.OrderPrice

        //               }).ToList().FindAll(x => x.ApprovedDH == true).FindAll(x => x.SHNT.Trim().Equals(present.Trim()) /*|| x.SHNT.Trim().Equals(presentUserName_2020.Trim())*/);

        //                //IEnumerable<RequestItems_Table> xi = (IEnumerable<RequestItems_Table>)requests.ToList();
        //                //reques = requests;

        //                foreach (var request in requests)
        //                {

        //                    dt.Rows.Add(
        //                        BudgetingController.lstBUs.Find(bu => bu.ID.ToString().Equals(request.BU)).BU,
        //                        BudgetingController.lstOEMs.Find(oem => oem.ID.ToString().Equals(request.OEM)).OEM,
        //                        BudgetingController.lstDEPTs.Find(dept => dept.ID.ToString().Equals(request.DEPT)).DEPT,
        //                        BudgetingController.lstGroups_test.Find(grp => grp.ID.ToString().Equals(request.Group)).Group,
        //                        request.Project,
        //                        BudgetingController.lstItems.Find(item => item.S_No.ToString().Equals(request.ItemName)).Item_Name,
        //                        BudgetingController.lstPrdCateg.Find(cat => cat.ID.ToString().Equals(request.Category)).Category.Trim(),
        //                        BudgetingController.lstCostElement.Find(cost => cost.ID.ToString().Equals(request.CostElement)).CostElement,
        //                        Math.Round((decimal)request.UnitPrice),
        //                        request.ActualAvailableQuantity != null ? request.ActualAvailableQuantity : "NA",
        //                        (int)request.ReqQuantity,
        //                         Math.Round((decimal)request.TotalPrice),
        //                        (int)request.ApprQuantity,
        //                         Math.Round((decimal)request.ApprCost),
        //                         Math.Round((decimal)request.ApprCost),
        //                        request.Comments,
        //                        request.RequestorNT,
        //                        request.SubmitDate.HasValue ? request.SubmitDate.Value.ToString("dd-MM-yyyy") : "",
        //                        request.DHNT,
        //                        request.DHAppDate.HasValue ? request.DHAppDate.Value.ToString("dd-MM-yyyy") : "",
        //                        request.SHNT,
        //                        request.SHAppDate.HasValue ? request.SHAppDate.Value.ToString("dd-MM-yyyy") : "",
        //                        request.ApprovedDH,
        //                        request.ApprovedSH,
        //                        request.OrderID,
        //                        (request.OrderStatus != null && request.OrderStatus.Trim() != "" && request.OrderStatus.Trim() != "0") ? BudgetingController.lstOrderStatus.Find(status => status.ID.ToString().Equals(request.OrderStatus.Trim())).OrderStatus : "",
        //                        request.OrderPrice
        //                        );

        //                }
        //            }


        //        }

        //        using (XLWorkbook wb = new XLWorkbook())
        //        {
        //            var ws = wb.Worksheets.Add("VKM_Items_List");
        //            ws.Cell(1, 1).Value = "BU Summary";


        //            List<string> years = new List<string>()
        //       { DateTime.Now.Year.ToString(), DateTime.Now.AddYears(1).Year.ToString() }; //VKM Yr , VKM Yr-1 (Previous VKM Year)
        //            string presentUserDept1 = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).Department;
        //            //presentUserDept = "XC";
        //            if (presentUserDept1.Contains("XC"))
        //                ws.Cell(2, 1).InsertTable(XC_BUSummaryComparison(years));
        //            else
        //                ws.Cell(2, 1).InsertTable(CC_BUSummaryComparison(years));
        //        //ws.Cell(2, 1).InsertTable(BUSummaryData_CC(useryear));

        //            ws.Cell(8, 1).Value = "Dept Summary";
        //            ws.Cell(9, 1).InsertTable(DeptSummaryData(true, useryear));
        //            ws.Cell(1, 15).Value = "Section Summary";
        //            ws.Cell(2, 15).InsertTable(SectionSummaryData(useryear));
        //            ws.Cell(15, 1).InsertTable(dt);
        //            using (MemoryStream stream = new MemoryStream())
        //            {

        //                wb.SaveAs(stream);
        //                var robj = File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
        //                return Json(robj, JsonRequestBehavior.AllowGet);
        //            }
        //        }

        //    }
        //}
        ///// <summary>
        ///// Function to export all items to Excel - filtering of CC dept / XC dept based on user's dept 
        /////  /// /// <param name="year"></param>
        ///// </summary>
        ///// <returns></returns>
        //public ActionResult ExportDataToExcelAll(string useryear)
        //{
        //    string filename = @"AllRequest_List_" + DateTime.Now.ToShortDateString() + ".xlsx";

        //    using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //    //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //    {


        //        System.Data.DataTable dt = new System.Data.DataTable("Request_List");
        //        if (useryear.Contains("2020"))
        //        {

        //            dt.Columns.AddRange(new DataColumn[34] { new DataColumn("Business Unit"),
        //                                    new DataColumn("OEM"),
        //                                    new DataColumn("Department"),
        //                                    new DataColumn("Group"),
        //                                    new DataColumn("Project"),
        //                                    new DataColumn("Item Name"),
        //                                    new DataColumn("Category"),
        //                                    new DataColumn("Cost Element"),
        //                                    new DataColumn("Unit Price", typeof(decimal)),
        //                                    new DataColumn("Actual Available Quantity"),
        //                                    new DataColumn("Required Quantity", typeof(Int32)),
        //                                    new DataColumn("Total Price", typeof(decimal)),
        //                                    new DataColumn("Reviewed Quantity", typeof(Int32)),
        //                                    new DataColumn("Reviewed Price", typeof(decimal)),
        //                                    new DataColumn("Total Budgeted Cost (USD)", typeof(decimal)),
        //                                    new DataColumn("Comments"),
        //                                    new DataColumn("Requestor"),
        //                                    new DataColumn("Submit Date"),
        //                                    new DataColumn("Reviewer L2"),
        //                                    new DataColumn("L2 Review Date"),
        //                                    new DataColumn("Reviewer L3"),
        //                                    new DataColumn("L3 Review Date"),
        //                                    new DataColumn("L2 Review Status"),
        //                                    new DataColumn("L3 Review Status"),
        //                                    new DataColumn("Required Date"),
        //                                    new DataColumn("Request Order Date"),
        //                                    new DataColumn("Order Date"),
        //                                    new DataColumn("Tentative Delivery Date"),
        //                                    new DataColumn("Actual Delivery Date"),
        //                                    new DataColumn("Fund"),
        //                                    new DataColumn("Order ID"),
        //                                    new DataColumn("Order Status"),
        //                                    new DataColumn("Ordered Quantity",typeof(Int32)),
        //                                    new DataColumn("Order Price",typeof(decimal))


        //        });

        //            var requests = db.RequestItems_Table.AsEnumerable().Where(x => x.RequestDate.ToString().Contains(useryear)).Select(x => new
        //            {
        //                x.BU,
        //                x.OEM,
        //                x.DEPT,
        //                x.Group,
        //                x.Project,
        //                x.ItemName,
        //                x.Category,
        //                x.CostElement,
        //                x.UnitPrice,
        //                x.ActualAvailableQuantity,
        //                x.ReqQuantity,
        //                x.TotalPrice,
        //                x.ApprQuantity,
        //                x.ApprCost,
        //                x.Comments,
        //                x.RequestorNT,
        //                x.SubmitDate,
        //                x.DHNT,
        //                x.DHAppDate,
        //                x.SHNT,
        //                x.SHAppDate,
        //                x.ApprovedDH,
        //                x.ApprovedSH


        //            }).ToList();
        //            foreach (var request in requests)
        //            {
        //                string BU = string.Empty;
        //                if (BudgetingController.lstBUs.Find(bu => bu.ID.ToString().Equals(request.BU)).BU == "MB")
        //                    BU = "AS";
        //                else if (BudgetingController.lstBUs.Find(bu => bu.ID.ToString().Equals(request.BU)).BU == "OSS")
        //                    BU = "PS";
        //                else
        //                    BU = BudgetingController.lstBUs.Find(bu => bu.ID.ToString().Equals(request.BU)).BU;

        //                dt.Rows.Add(
        //                    BU,
        //                    BudgetingController.lstOEMs.Find(oem => oem.ID.ToString().Equals(request.OEM)).OEM,
        //                    BudgetingController.lstDEPTs.Find(dept => dept.ID.ToString().Equals(request.DEPT)).DEPT,
        //                    BudgetingController.lstGroups_test.Find(grp => grp.ID.ToString().Equals(request.Group)).Group,
        //                    request.Project,
        //                    BudgetingController.lstItems.Find(item => item.S_No.ToString().Equals(request.ItemName)).Item_Name,
        //                    BudgetingController.lstPrdCateg.Find(cat => cat.ID.ToString().Equals(request.Category)).Category.Trim(),
        //                    BudgetingController.lstCostElement.Find(cost => cost.ID.ToString().Equals(request.CostElement)).CostElement,
        //                    Math.Round((decimal)request.UnitPrice),
        //                    request.ActualAvailableQuantity != null ? request.ActualAvailableQuantity : "NA",
        //                    (int)request.ReqQuantity,
        //                    Math.Round((decimal)request.TotalPrice),
        //                    request.ApprQuantity != null ? request.ApprQuantity : null,
        //                    request.ApprCost != null ? Math.Round((decimal)request.ApprCost) : 0,
        //                    request.ApprCost != null ? Math.Round((decimal)request.ApprCost) : 0,
        //                    request.Comments,
        //                    request.RequestorNT,
        //                    request.SubmitDate.HasValue ? request.SubmitDate.Value.ToString("dd-MM-yyyy") : "",
        //                    request.DHNT,
        //                    request.DHAppDate.HasValue ? request.DHAppDate.Value.ToString("dd-MM-yyyy") : "",
        //                    request.SHNT,
        //                    request.SHAppDate.HasValue ? request.SHAppDate.Value.ToString("dd-MM-yyyy") : "",
        //                    request.ApprovedDH,
        //                    request.ApprovedSH
        //                    //request.OrderID,
        //                    //(request.OrderStatus != null && request.OrderStatus.Trim() != "" && request.OrderStatus.Trim() != "0") ? BudgetingController.lstOrderStatus.Find(status => status.ID.ToString().Equals(request.OrderStatus.Trim())).OrderStatus : "",
        //                    //request.OrderPrice
        //                    );

        //            }

        //            using (XLWorkbook wb = new XLWorkbook())
        //            {
        //                var ws = wb.Worksheets.Add("VKM_Items_List_2020");

        //                ws.Cell(3, 1).InsertTable(dt);
        //                using (MemoryStream stream = new MemoryStream())
        //                {

        //                    wb.SaveAs(stream);
        //                    var robj = File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
        //                    return Json(robj, JsonRequestBehavior.AllowGet);
        //                }
        //            }
        //        }






        //        string presentUserDept = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).Department;
        //        string is_CCXC = string.Empty;
        //        if (presentUserDept.Contains("XC"))
        //            is_CCXC = "XC";
        //        else
        //            is_CCXC = "CC";

        //        //is_CCXC = "XC";

        //        if (BudgetingController.lstBU_SPOCs.Find(person => person.VKMspoc.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())) == null)
        //        {

        //            dt.Columns.AddRange(new DataColumn[36] { new DataColumn("Business Unit"),
        //                                    new DataColumn("OEM"),
        //                                    new DataColumn("Department"),
        //                                    new DataColumn("Group"),
        //                                    new DataColumn("Project"),
        //                                    new DataColumn("Item Name"),
        //                                    new DataColumn("Category"),
        //                                    new DataColumn("Cost Element"),
        //                                    new DataColumn("Unit Price",typeof(decimal)),
        //                                    new DataColumn("Actual Available Quantity"),
        //                                    new DataColumn("Required Quantity",typeof(Int32)),
        //                                    new DataColumn("Total Price",typeof(decimal)),
        //                                    new DataColumn("Total Requested in USD",typeof(decimal)),
        //                                    new DataColumn("Reviewed Quantity",typeof(Int32)),
        //                                    new DataColumn("Reviewed Price",typeof(decimal)),
        //                                    new DataColumn("Total Budgeted Cost (USD)",typeof(decimal)),
        //                                    new DataColumn("Comments"),
        //                                    new DataColumn("Requestor"),
        //                                    new DataColumn("Submit Date"),
        //                                    new DataColumn("Reviewer L2"),
        //                                    new DataColumn("L2 Review Date"),
        //                                    new DataColumn("Reviewer L3"),
        //                                    new DataColumn("L3 Review Date"),
        //                                    new DataColumn("Item At Requestor"),
        //                                    new DataColumn("Pending L2 Review"),
        //                                    new DataColumn("Pending L3 Review"),
        //                                    new DataColumn("Required Date"),
        //                                    new DataColumn("Request Order Date"),
        //                                    new DataColumn("Order Date"),
        //                                    new DataColumn("Tentative Delivery Date"),
        //                                    new DataColumn("Actual Delivery Date"),
        //                                    new DataColumn("Fund"),
        //                                    new DataColumn("Order ID"),
        //                                    new DataColumn("Order Status"),
        //                                    new DataColumn("Ordered Quantity",typeof(Int32)),
        //                                    new DataColumn("Order Price",typeof(decimal))
        //        });
        //        if (presentUserDept.Contains("XC"))
        //        {
        //            var requests = db.RequestItems_Table.AsEnumerable().Where(x => x.RequestDate.ToString().Contains(useryear)).Select(x => new
        //            {
        //                x.BU,
        //                x.OEM,
        //                x.DEPT,
        //                x.Group,
        //                x.Project,
        //                x.ItemName,
        //                x.Category,
        //                x.CostElement,
        //                x.UnitPrice,
        //                x.ActualAvailableQuantity,
        //                x.ReqQuantity,
        //                x.TotalPrice,
        //                x.ApprQuantity,
        //                x.ApprCost,
        //                x.Comments,
        //                x.RequestorNT,
        //                x.SubmitDate,
        //                x.DHNT,
        //                x.DHAppDate,
        //                x.SHNT,
        //                x.SHAppDate,
        //                x.ApprovedDH,
        //                x.ApprovedSH,
        //                x.ApprovalDH,
        //                x.ApprovalSH,
        //                x.RequiredDate,
        //                x.RequestOrderDate,
        //                x.OrderDate,
        //                x.TentativeDeliveryDate,
        //                x.ActualDeliveryDate,
        //                x.Fund,
        //                x.OrderID,
        //                x.OrderStatus,
        //                x.OrderedQuantity,
        //                x.OrderPrice
        //            }).ToList().FindAll(dpt => BudgetingController.lstDEPTs.Find(x => x.ID.ToString().Contains(dpt.DEPT)).DEPT.Contains("XC"));

        //            foreach (var request in requests)
        //            {
        //                ////////////////////////////////////////////////////////////////////////////////////////////////////////
        //                dt.Rows.Add(
        //                    BudgetingController.lstBUs.Find(bu => bu.ID.ToString().Equals(request.BU)).BU,
        //                    BudgetingController.lstOEMs.Find(oem => oem.ID.ToString().Equals(request.OEM)).OEM,
        //                    BudgetingController.lstDEPTs.Find(dept => dept.ID.ToString().Equals(request.DEPT)).DEPT,
        //                    BudgetingController.lstGroups_test.Find(grp => grp.ID.ToString().Equals(request.Group)).Group,
        //                    request.Project,
        //                    BudgetingController.lstItems.Find(item => item.S_No.ToString().Equals(request.ItemName)).Item_Name,
        //                    BudgetingController.lstPrdCateg.Find(cat => cat.ID.ToString().Equals(request.Category)).Category.Trim(),
        //                    BudgetingController.lstCostElement.Find(cost => cost.ID.ToString().Equals(request.CostElement)).CostElement,
        //                     Math.Round((decimal)request.UnitPrice),
        //                    request.ActualAvailableQuantity != null ? request.ActualAvailableQuantity : "NA",
        //                    (int)request.ReqQuantity,
        //                     Math.Round((decimal)request.TotalPrice),
        //                    request.TotalPrice == null ? 0 : Math.Round((decimal)request.TotalPrice),
        //                    request.ApprQuantity,
        //                     request.ApprCost == null ? 0 : Math.Round((decimal)request.ApprCost),
        //                    request.ApprCost == null ? 0 : Math.Round((decimal)request.ApprCost),
        //                    request.Comments,
        //                    request.RequestorNT,
        //                    request.SubmitDate.HasValue ? request.SubmitDate.Value.ToString("dd-MM-yyyy") : "",
        //                    request.DHNT,
        //                    request.DHAppDate.HasValue ? request.DHAppDate.Value.ToString("dd-MM-yyyy") : "",
        //                    request.SHNT,
        //                    request.SHAppDate.HasValue ? request.SHAppDate.Value.ToString("dd-MM-yyyy") : "",
        //                    (bool)!request.ApprovalDH,
        //                    ((bool)request.ApprovalDH && (bool)!request.ApprovedDH),
        //                    ((bool)request.ApprovalSH && (bool)!request.ApprovedSH),
        //                     request.RequiredDate.HasValue ? request.RequiredDate.Value.ToString("dd-MM-yyyy") : "",
        //                    request.RequestOrderDate.HasValue ? request.RequestOrderDate.Value.ToString("dd-MM-yyyy") : "",
        //                    request.OrderDate.HasValue ? request.OrderDate.Value.ToString("dd-MM-yyyy") : "",
        //                    request.TentativeDeliveryDate.HasValue ? request.TentativeDeliveryDate.Value.ToString("dd-MM-yyyy") : "",
        //                    request.ActualDeliveryDate.HasValue ? request.ActualDeliveryDate.Value.ToString("dd-MM-yyyy") : "",
        //                    request.Fund != null ? BudgetingController.lstFund.Find(fun => fun.ID.ToString().Equals(request.Fund)).Fund : BudgetingController.lstFund.Find(fun => fun.Fund.Trim().Equals("F02")).Fund,
        //                    request.OrderID,
        //                    (request.OrderStatus != null && request.OrderStatus.Trim() != "" && request.OrderStatus.Trim() != "0") ? BudgetingController.lstOrderStatus.Find(status => status.ID.ToString().Equals(request.OrderStatus.Trim())).OrderStatus : "",
        //                    request.OrderedQuantity != null ? (int)request.OrderedQuantity : (int)0,
        //                    request.OrderPrice
        //                    );

        //            }
        //        }
        //        else
        //        {
        //            var requests = db.RequestItems_Table.AsEnumerable().Where(x => x.RequestDate.ToString().Contains(useryear)).Select(x => new
        //            {
        //                x.BU,
        //                x.OEM,
        //                x.DEPT,
        //                x.Group,
        //                x.Project,
        //                x.ItemName,
        //                x.Category,
        //                x.CostElement,
        //                x.UnitPrice,
        //                x.ActualAvailableQuantity,
        //                x.ReqQuantity,
        //                x.TotalPrice,
        //                x.ApprQuantity,
        //                x.ApprCost,
        //                x.Comments,
        //                x.RequestorNT,
        //                x.SubmitDate,
        //                x.DHNT,
        //                x.DHAppDate,
        //                x.SHNT,
        //                x.SHAppDate,
        //                x.ApprovedDH,
        //                x.ApprovedSH,
        //                x.ApprovalDH,
        //                x.ApprovalSH,
        //                x.RequiredDate,
        //                x.RequestOrderDate,
        //                x.OrderDate,
        //                x.TentativeDeliveryDate,
        //                x.ActualDeliveryDate,
        //                x.Fund,
        //                x.OrderID,
        //                x.OrderStatus,
        //                x.OrderedQuantity,
        //                x.OrderPrice
        //            }).ToList().FindAll(dpt => !BudgetingController.lstDEPTs.Find(x => x.ID.ToString().Contains(dpt.DEPT)).DEPT.Contains("XC"));

        //            foreach (var request in requests)
        //            {
        //                ////////////////////////////////////////////////////////////////////////////////////////////////////////
        //                dt.Rows.Add(
        //                    BudgetingController.lstBUs.Find(bu => bu.ID.ToString().Equals(request.BU)).BU,
        //                    BudgetingController.lstOEMs.Find(oem => oem.ID.ToString().Equals(request.OEM)).OEM,
        //                    BudgetingController.lstDEPTs.Find(dept => dept.ID.ToString().Equals(request.DEPT)).DEPT,
        //                    BudgetingController.lstGroups_test.Find(grp => grp.ID.ToString().Equals(request.Group)).Group,
        //                    request.Project,
        //                    BudgetingController.lstItems.Find(item => item.S_No.ToString().Equals(request.ItemName)).Item_Name,
        //                    BudgetingController.lstPrdCateg.Find(cat => cat.ID.ToString().Equals(request.Category)).Category.Trim(),
        //                    BudgetingController.lstCostElement.Find(cost => cost.ID.ToString().Equals(request.CostElement)).CostElement,
        //                     Math.Round((decimal)request.UnitPrice),
        //                    request.ActualAvailableQuantity != null ? request.ActualAvailableQuantity : "NA",
        //                    (int)request.ReqQuantity,
        //                     Math.Round((decimal)request.TotalPrice),
        //                    request.TotalPrice == null ? 0 : Math.Round((decimal)request.TotalPrice),
        //                    request.ApprQuantity,
        //                     request.ApprCost == null ? 0 : Math.Round((decimal)request.ApprCost),
        //                    request.ApprCost == null ? 0 : Math.Round((decimal)request.ApprCost),
        //                    request.Comments,
        //                    request.RequestorNT,
        //                    request.SubmitDate.HasValue ? request.SubmitDate.Value.ToString("dd-MM-yyyy") : "",
        //                    request.DHNT,
        //                    request.DHAppDate.HasValue ? request.DHAppDate.Value.ToString("dd-MM-yyyy") : "",
        //                    request.SHNT,
        //                    request.SHAppDate.HasValue ? request.SHAppDate.Value.ToString("dd-MM-yyyy") : "",
        //                    (bool)!request.ApprovalDH,
        //                    ((bool)request.ApprovalDH && (bool)!request.ApprovedDH),
        //                    ((bool)request.ApprovalSH && (bool)!request.ApprovedSH),
        //                     request.RequiredDate.HasValue ? request.RequiredDate.Value.ToString("dd-MM-yyyy") : "",
        //                    request.RequestOrderDate.HasValue ? request.RequestOrderDate.Value.ToString("dd-MM-yyyy") : "",
        //                    request.OrderDate.HasValue ? request.OrderDate.Value.ToString("dd-MM-yyyy") : "",
        //                    request.TentativeDeliveryDate.HasValue ? request.TentativeDeliveryDate.Value.ToString("dd-MM-yyyy") : "",
        //                    request.ActualDeliveryDate.HasValue ? request.ActualDeliveryDate.Value.ToString("dd-MM-yyyy") : "",
        //                    request.Fund != null ? BudgetingController.lstFund.Find(fun => fun.ID.ToString().Equals(request.Fund)).Fund : BudgetingController.lstFund.Find(fun => fun.Fund.Trim().Equals("F02")).Fund,
        //                    request.OrderID,
        //                    (request.OrderStatus != null && request.OrderStatus.Trim() != "" && request.OrderStatus.Trim() != "0") ? BudgetingController.lstOrderStatus.Find(status => status.ID.ToString().Equals(request.OrderStatus.Trim())).OrderStatus : "",
        //                    request.OrderedQuantity != null ? (int)request.OrderedQuantity : (int)0,
        //                    request.OrderPrice
        //                    );

        //            }
        //        }

        //        }
        //        else
        //        {
        //            dt.Columns.AddRange(new DataColumn[29] { new DataColumn("Business Unit"),
        //                                    new DataColumn("OEM"),
        //                                    new DataColumn("Department"),
        //                                    new DataColumn("Group"),
        //                                    new DataColumn("Project"),
        //                                    new DataColumn("Item Name"),
        //                                    new DataColumn("Category"),
        //                                    new DataColumn("Cost Element"),
        //                                    new DataColumn("Unit Price",typeof(decimal)),
        //                                    new DataColumn("Actual Available Quantity"),
        //                                    new DataColumn("Required Quantity",typeof(Int32)),
        //                                    new DataColumn("Total Price",typeof(decimal)),
        //                                    new DataColumn("Total Requested in USD",typeof(decimal)),
        //                                    new DataColumn("Reviewed Quantity",typeof(Int32)),
        //                                    new DataColumn("Reviewed Price",typeof(decimal)),
        //                                    new DataColumn("Total Budgeted Cost (USD)",typeof(decimal)),
        //                                    new DataColumn("Comments"),
        //                                    new DataColumn("Requestor"),
        //                                    new DataColumn("Submit Date"),
        //                                    new DataColumn("Reviewer L2"),
        //                                    new DataColumn("L2 Review Date"),
        //                                    new DataColumn("Reviewer L3"),
        //                                    new DataColumn("L3 Review Date"),
        //                                    new DataColumn("Item At Requestor"),
        //                                    new DataColumn("Pending L2 Review"),
        //                                    new DataColumn("Pending L3 Review"),
        //                                    new DataColumn("Order ID"),
        //                                    new DataColumn("Order Status"),
        //                                    new DataColumn("Order Price",typeof(decimal))
        //        });
        //            var requests = db.RequestItems_Table.AsEnumerable().Where(x => x.RequestDate.ToString().Contains(useryear)).Select(x => new
        //            {
        //                x.BU,
        //                x.OEM,
        //                x.DEPT,
        //                x.Group,
        //                x.Project,
        //                x.ItemName,
        //                x.Category,
        //                x.CostElement,
        //                x.UnitPrice,
        //                x.ActualAvailableQuantity,
        //                x.ReqQuantity,
        //                x.TotalPrice,
        //                x.ApprQuantity,
        //                x.ApprCost,
        //                x.Comments,
        //                x.RequestorNT,
        //                x.SubmitDate,
        //                x.DHNT,
        //                x.DHAppDate,
        //                x.SHNT,
        //                x.SHAppDate,
        //                x.ApprovedDH,
        //                x.ApprovedSH,
        //                x.ApprovalDH,
        //                x.ApprovalSH,
        //                x.OrderID,
        //                x.OrderStatus,
        //                x.OrderPrice
        //            }).ToList().FindAll(dpt => BudgetingController.lstDEPTs.Find(x => x.ID.ToString().Contains(dpt.DEPT)).DEPT.Contains(is_CCXC));

        //            foreach (var request in requests)
        //            {

        //                dt.Rows.Add(
        //                    BudgetingController.lstBUs.Find(bu => bu.ID.ToString().Equals(request.BU)).BU,
        //                    BudgetingController.lstOEMs.Find(oem => oem.ID.ToString().Equals(request.OEM)).OEM,
        //                    BudgetingController.lstDEPTs.Find(dept => dept.ID.ToString().Equals(request.DEPT)).DEPT,
        //                    BudgetingController.lstGroups_test.Find(grp => grp.ID.ToString().Equals(request.Group)).Group,
        //                    request.Project,
        //                    BudgetingController.lstItems.Find(item => item.S_No.ToString().Equals(request.ItemName)).Item_Name,
        //                    BudgetingController.lstPrdCateg.Find(cat => cat.ID.ToString().Equals(request.Category)).Category.Trim(),
        //                    BudgetingController.lstCostElement.Find(cost => cost.ID.ToString().Equals(request.CostElement)).CostElement,
        //                     Math.Round((decimal)request.UnitPrice),
        //                    request.ActualAvailableQuantity != null ? request.ActualAvailableQuantity : "NA",
        //                    request.ReqQuantity,
        //                     Math.Round((decimal)request.TotalPrice),
        //                    request.TotalPrice == null ? 0 : Math.Round((decimal)request.TotalPrice),
        //                    request.ApprQuantity,
        //                     request.ApprCost == null ? 0 : Math.Round((decimal)request.ApprCost),
        //                    request.ApprCost == null ? 0 : Math.Round((decimal)request.ApprCost),
        //                    request.Comments,
        //                    request.RequestorNT,
        //                    request.SubmitDate.HasValue ? request.SubmitDate.Value.ToString("dd-MM-yyyy") : "",
        //                    request.DHNT,
        //                    request.DHAppDate.HasValue ? request.DHAppDate.Value.ToString("dd-MM-yyyy") : "",
        //                    request.SHNT,
        //                    request.SHAppDate.HasValue ? request.SHAppDate.Value.ToString("dd-MM-yyyy") : "",
        //                    (bool)!request.ApprovalDH,
        //                    ((bool)request.ApprovalDH && (bool)!request.ApprovedDH),
        //                    ((bool)request.ApprovalSH && (bool)!request.ApprovedSH),
        //                    request.OrderID,
        //                    (request.OrderStatus != null && request.OrderStatus.Trim() != "" && request.OrderStatus.Trim() != "0") ? BudgetingController.lstOrderStatus.Find(status => status.ID.ToString().Equals(request.OrderStatus.Trim())).OrderStatus : "",
        //                    request.OrderPrice
        //                    );

        //            }
        //        }

        //        using (XLWorkbook wb = new XLWorkbook())
        //        {
        //            var ws = wb.Worksheets.Add("VKM_Items_List");
        //            ws.Cell(1, 1).Value = "BU Pending Summary";
        //            ws.Cell(2, 1).InsertTable(SectionPendingSummaryData(useryear));
        //            ws.Cell(9, 1).Value = "Dept Summary(includes all items at L2 & L3 review levels): Please note the Totals here is the total of the Total Price - not the Reviewed Price";
        //            ws.Cell(10, 1).InsertTable(DeptSummaryAllData(true, useryear));
        //            ws.Cell(17, 1).InsertTable(dt);
        //            using (MemoryStream stream = new MemoryStream())
        //            {

        //                wb.SaveAs(stream);
        //                var robj = File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
        //                return Json(robj, JsonRequestBehavior.AllowGet);
        //            }
        //        }

        //    }
        //}
        ///// <summary>
        /// Function to get the dept cost summary data
        /// </summary>
        /// <param name="forexcel">can be used to add or remove columns for different sources</param>
        /// <returns></returns>


        //public System.Data.DataTable DeptSummaryAllData(bool forexcel, string year)
        //{
        //    System.Data.DataTable dt = new System.Data.DataTable();
        //    try
        //    {

        //        using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //        //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //        {
        //            System.Security.Principal.IPrincipal User = System.Web.HttpContext.Current.User;
        //            List<DeptSummary> viewList = new List<DeptSummary>();
        //            if (year.Contains("2020"))
        //            {
        //                DeptSummary tempobj = new DeptSummary();
        //                viewList.Add(tempobj);
        //            }

        //            List<RequestItems_Table> reqList = db.RequestItems_Table.ToList<RequestItems_Table>().FindAll(x => x.ApprovalDH == true).FindAll(x => x.RequestDate.ToString().Contains(year)).FindAll(v => v.Fund == null || v.Fund.Trim().Contains("2"));

        //            List<string> userSectionDeptList = new List<string>();
        //            List<string> approvedSections = new List<string>();
        //            //approvedSections.Add("NE2-VS");
        //            //approvedSections.Add("NE1-VS");
        //            //approvedSections.Add("ESD");v
        //            //approvedSections.Add("MS/ESP-CC");
        //            //approvedSections.Add("MS/ESC-CC");
        //            //approvedSections.Add("MS/ESA-CC");
        //            //approvedSections.Add("MS/ESO-CC");
        //            string presentUserDept = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).Department;

        //            string is_CCXC = string.Empty;
        //            if (presentUserDept.Contains("XC"))
        //                is_CCXC = "XC";
        //            else
        //                is_CCXC = "CC";

        //            //is_CCXC = "XC";

        //            if (is_CCXC == "CC")
        //            {
        //                approvedSections.Add("MS/NE-CC");
        //                approvedSections.Add("MS/ESP-CC");
        //                approvedSections.Add("MS/ESC-CC");
        //                approvedSections.Add("MS/ESA-CC");
        //                approvedSections.Add("MS/ESO-CC");

        //            }
        //            else if (is_CCXC == "XC")
        //            {
        //                approvedSections.Add("MS/NE-XC");
        //                approvedSections.Add("MS/NE3-XC");
        //                //approvedSections.Add("MS/NE4-XC");
        //                approvedSections.Add("MS/EXA-XC");
        //                approvedSections.Add("MS/EVC-XC"); //NE1,NE2-XC?

        //            }


        //            userSectionDeptList = BudgetingController.lstUsers.FindAll(x => approvedSections.Contains(x.Section)).Select(item => item.Department.Trim()).Distinct().ToList();
        //            userSectionDeptList.Sort();
        //            userSectionDeptList.Reverse();
        //            //CODE TO GET GROUPS OF COST ELEMENT
        //            IEnumerable<IGrouping<string, RequestItems_Table>> query = reqList.GroupBy(item => item.CostElement);
        //            decimal OMAE_Totals = 0, ONMAE_Totals = 0, OSoftwareTotals = 0;
        //            foreach (string dept in userSectionDeptList)
        //            {
        //                decimal MAE_Totals = 0, NMAE_Totals = 0, SoftwareTotals = 0;
        //                DeptSummary tempobj = new DeptSummary();
        //                tempobj.deptName = dept;
        //                //CODE TO GET THE TOTALS OF EACH COST ELEMENT
        //                foreach (IGrouping<string, RequestItems_Table> CostGroup in query)
        //                {

        //                    // Iterate over each value in the
        //                    // IGrouping and print the value.
        //                    if (BudgetingController.lstCostElement.Find(cost => cost.ID.Equals(Int32.Parse(CostGroup.Key))).CostElement == "MAE")
        //                    {
        //                        foreach (RequestItems_Table item in CostGroup)
        //                        {
        //                            if (BudgetingController.lstDEPTs.Find(departmnt => departmnt.ID.Equals(Int32.Parse(item.DEPT))).DEPT == dept)
        //                            {

        //                                MAE_Totals += (decimal)item.TotalPrice;

        //                            }
        //                        }
        //                    }
        //                    else if (BudgetingController.lstCostElement.Find(cost => cost.ID.Equals(Int32.Parse(CostGroup.Key))).CostElement == "Non-MAE")

        //                    {
        //                        foreach (RequestItems_Table item in CostGroup)
        //                        {
        //                            if (BudgetingController.lstDEPTs.Find(departmnt => departmnt.ID.Equals(Int32.Parse(item.DEPT))).DEPT == dept)
        //                            {

        //                                NMAE_Totals += (decimal)item.TotalPrice;

        //                            }
        //                        }

        //                    }
        //                    else if (BudgetingController.lstCostElement.Find(cost => cost.ID.Equals(Int32.Parse(CostGroup.Key))).CostElement == "Software")

        //                    {
        //                        foreach (RequestItems_Table item in CostGroup)
        //                        {
        //                            if (BudgetingController.lstDEPTs.Find(departmnt => departmnt.ID.Equals(Int32.Parse(item.DEPT))).DEPT == dept)
        //                            {

        //                                SoftwareTotals += (decimal)item.TotalPrice;

        //                            }
        //                        }

        //                    }
        //                }
        //                tempobj.MAE_Totals = MAE_Totals;
        //                OMAE_Totals += MAE_Totals;
        //                tempobj.NMAE_Totals = NMAE_Totals;
        //                ONMAE_Totals += NMAE_Totals;
        //                tempobj.Software_Totals = SoftwareTotals;
        //                OSoftwareTotals += SoftwareTotals;
        //                tempobj.Overall_Totals = MAE_Totals + NMAE_Totals + SoftwareTotals;
        //                viewList.Add(tempobj);
        //            }
        //            #region commented
        //            //List<SummaryDeptView> DTviewList = new List<SummaryDeptView>();
        //            //SummaryDeptView objbind = new SummaryDeptView();
        //            //objbind.COLUMNS = tempColDefn;
        //            ////objbind.DATA = "";

        //            //List<DATADEFN> listdata = new List<DATADEFN>();

        //            //string[] tempMAEData = new string[userSectionDeptList.Count];
        //            //int count = 0;
        //            //foreach (var item in viewList)
        //            //{
        //            //    tempMAEData[count] = item.MAE_Totals.ToString();
        //            //    count++;
        //            //}

        //            //string[] tempNMAEData = new string[userSectionDeptList.Count];
        //            //count = 0;
        //            //foreach (var item in viewList)
        //            //{
        //            //    tempNMAEData[count] = item.NMAE_Totals.ToString();
        //            //    count++;
        //            //}
        //            //string[] tempSoftData = new string[userSectionDeptList.Count];
        //            //count = 0;
        //            //foreach (var item in viewList)
        //            //{
        //            //    tempSoftData[count] = item.Software_Totals.ToString();
        //            //    count++;
        //            //}
        //            #endregion

        //            ///NEW CODE TO GET DEPT SUMMARY



        //            dt.Columns.Add("CostElement", typeof(string));
        //            dt.Columns.Add("All Depts", typeof(string));
        //            foreach (string dept in userSectionDeptList)
        //            {
        //                dt.Columns.Add(dept, typeof(string));
        //            }


        //            DataRow dr = dt.NewRow();
        //            dr[0] = "MAE";
        //            dr[1] = OMAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //            int count = 2;
        //            foreach (var item in viewList)
        //            {
        //                dr[count] = item.MAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //                count++;
        //            }
        //            dt.Rows.Add(dr);

        //            dr = dt.NewRow();
        //            dr[0] = "Non-MAE";
        //            dr[1] = ONMAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //            count = 2;
        //            foreach (var item in viewList)
        //            {
        //                dr[count] = item.NMAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //                count++;
        //            }
        //            dt.Rows.Add(dr);

        //            dr = dt.NewRow();
        //            dr[0] = "Software";
        //            dr[1] = OSoftwareTotals.ToString("C0", CultureInfo.CurrentCulture);
        //            count = 2;
        //            foreach (var item in viewList)
        //            {
        //                dr[count] = item.Software_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //                count++;
        //            }
        //            dt.Rows.Add(dr);

        //            dr = dt.NewRow();
        //            dr[0] = "Totals";
        //            dr[1] = (OMAE_Totals + ONMAE_Totals + OSoftwareTotals).ToString("C0", CultureInfo.CurrentCulture);
        //            count = 2;
        //            foreach (var item in viewList)
        //            {
        //                dr[count] = item.Overall_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //                count++;
        //            }
        //            dt.Rows.Add(dr);

        //            return dt;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        WriteLog("Error - DeptSummaryAllData : NTID - " + User.Identity.Name.Split('\\')[1].Trim().ToUpper() + ", ErrorMsg : " + ex + ", At - " + DateTime.Now);
        //        return dt;
        //        //return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        //    }
        //}
        public System.Data.DataTable DeptSummaryAllData(bool forexcel, string year)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            try
            {

                using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())
                {
                    System.Security.Principal.IPrincipal User = System.Web.HttpContext.Current.User;
                    List<DeptSummary> viewList = new List<DeptSummary>();
                    List<string> userSectionDeptList = new List<string>();
                    List<string> approvedSections = new List<string>();
                    decimal OMAE_Totals = 0, ONMAE_Totals = 0, OSoftwareTotals = 0;
                    string qry = " exec [dbo].[BGSW_DeptAllSummary] " + year + ",'" + User.Identity.Name.Split('\\')[1].ToUpper() + "' ";

                    DataSet ds = new DataSet();

                    OpenConnection();
                    SqlCommand cmd1 = new SqlCommand(qry, conn);
                    SqlDataAdapter da = new SqlDataAdapter(cmd1);
                    da.Fill(ds);
                    CloseConnection();

                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        userSectionDeptList.Add(item["Department"].ToString());
                    }

                    userSectionDeptList.Sort();
                    userSectionDeptList.Reverse();

                    foreach (DataRow item in ds.Tables[1].Rows)
                    {
                        decimal MAE_Totals = 0, NMAE_Totals = 0, SoftwareTotals = 0;
                        DeptSummary tempobj = new DeptSummary();
                        tempobj.deptName = item["DEPT"].ToString();
                        MAE_Totals = (decimal)item["MAE"];
                        NMAE_Totals = (decimal)item["Non-MAE"];
                        SoftwareTotals = (decimal)item["Software"];
                        tempobj.MAE_Totals = MAE_Totals;
                        OMAE_Totals += MAE_Totals;
                        tempobj.NMAE_Totals = NMAE_Totals;
                        ONMAE_Totals += NMAE_Totals;
                        tempobj.Software_Totals = SoftwareTotals;
                        OSoftwareTotals += SoftwareTotals;
                        tempobj.Overall_Totals = MAE_Totals + NMAE_Totals + SoftwareTotals;
                        viewList.Add(tempobj);
                    }


                    ///NEW CODE TO GET DEPT SUMMARY



                    dt.Columns.Add("CostElement", typeof(string));
                    dt.Columns.Add("All Depts", typeof(string));
                    foreach (string dept in userSectionDeptList)
                    {
                        dt.Columns.Add(dept, typeof(string));
                    }


                    DataRow dr = dt.NewRow();
                    dr[0] = "MAE";
                    dr[1] = OMAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
                    int count = 2;
                    foreach (var item in viewList)
                    {
                        dr[count] = item.MAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
                        count++;
                    }
                    dt.Rows.Add(dr);

                    dr = dt.NewRow();
                    dr[0] = "Non-MAE";
                    dr[1] = ONMAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
                    count = 2;
                    foreach (var item in viewList)
                    {
                        dr[count] = item.NMAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
                        count++;
                    }
                    dt.Rows.Add(dr);

                    dr = dt.NewRow();
                    dr[0] = "Software";
                    dr[1] = OSoftwareTotals.ToString("C0", CultureInfo.CurrentCulture);
                    count = 2;
                    foreach (var item in viewList)
                    {
                        dr[count] = item.Software_Totals.ToString("C0", CultureInfo.CurrentCulture);
                        count++;
                    }
                    dt.Rows.Add(dr);

                    dr = dt.NewRow();
                    dr[0] = "Totals";
                    dr[1] = (OMAE_Totals + ONMAE_Totals + OSoftwareTotals).ToString("C0", CultureInfo.CurrentCulture);
                    count = 2;
                    foreach (var item in viewList)
                    {
                        dr[count] = item.Overall_Totals.ToString("C0", CultureInfo.CurrentCulture);
                        count++;
                    }
                    dt.Rows.Add(dr);

                    return dt;
                }
            }
            catch (Exception ex)
            {
                WriteLog("Error - DeptSummaryAllData : NTID - " + User.Identity.Name.Split('\\')[1].Trim().ToUpper() + ", ErrorMsg : " + ex + ", At - " + DateTime.Now);
                return dt;
                //return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }



        /// <summary>
        /// Function to get BU cost Summary as list
        /// </summary>
        /// <returns></returns>
        //public List<SummaryView> BUSummaryData(string year)
        //{
        //    string presentUserDept = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).Department;

        //    //presentUserDept = "XC";

        //    using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //    //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //    {
        //        List<RequestItems_Table> reqList = db.RequestItems_Table.ToList<RequestItems_Table>().FindAll(x => x.RequestDate.ToString().Contains(year));
        //        List<SummaryView> viewList = new List<SummaryView>();
        //        List<SummaryView_CC> viewList_CC = new List<SummaryView_CC>();
        //        List<SummaryView_XC> viewList_XC = new List<SummaryView_XC>();

        //        decimal AS_MAE_Totals = 0, AS_NMAE_Totals = 0, AS_SoftwareTotals = 0;
        //        decimal OSS_MAE_Totals = 0, OSS_NMAE_Totals = 0, OSS_SoftwareTotals = 0;
        //        decimal DA_MAE_Totals = 0, DA_NMAE_Totals = 0, DA_SoftwareTotals = 0;
        //        decimal AD_MAE_Totals = 0, AD_NMAE_Totals = 0, AD_SoftwareTotals = 0;
        //        decimal TwoWP_MAE_Totals = 0, TwoWP_NMAE_Totals = 0, TwoWP_SoftwareTotals = 0;


        //        if (presentUserDept.Contains("CC"))
        //        {
        //            foreach (RequestItems_Table item in reqList.FindAll(x => x.BU.Equals(BudgetingController.lstBUs.Find(bu => bu.BU.Trim().Equals("MB")).ID.ToString())).FindAll(x => x.ApprovedDH == true))
        //            {
        //                switch (BudgetingController.lstCostElement.Find(cost => cost.ID.Equals(Int32.Parse(item.CostElement))).CostElement)
        //                {
        //                    case "MAE":
        //                        {

        //                            AS_MAE_Totals += Math.Round((decimal)item.ApprCost, MidpointRounding.AwayFromZero);

        //                            break;
        //                        }
        //                    case "Non-MAE":
        //                        {

        //                            AS_NMAE_Totals += Math.Round((decimal)item.ApprCost, MidpointRounding.AwayFromZero);

        //                            break;
        //                        }
        //                    case "Software":
        //                        {

        //                            AS_SoftwareTotals += Math.Round((decimal)item.ApprCost, MidpointRounding.AwayFromZero);

        //                            break;
        //                        }
        //                    default:
        //                        continue;
        //                }
        //            }
        //            foreach (RequestItems_Table item in reqList.FindAll(x => x.BU.Trim().Equals(BudgetingController.lstBUs.Find(bu => bu.BU.Equals("OSS")).ID.ToString())).FindAll(x => x.ApprovedDH == true))
        //            {
        //                switch (BudgetingController.lstCostElement.Find(cost => cost.ID.Equals(Int32.Parse(item.CostElement))).CostElement)
        //                {
        //                    case "MAE":
        //                        {

        //                            OSS_MAE_Totals += Math.Round((decimal)item.ApprCost, MidpointRounding.AwayFromZero);

        //                            break;
        //                        }
        //                    case "Non-MAE":
        //                        {

        //                            OSS_NMAE_Totals += Math.Round((decimal)item.ApprCost, MidpointRounding.AwayFromZero);

        //                            break;
        //                        }
        //                    case "Software":
        //                        {

        //                            OSS_SoftwareTotals += Math.Round((decimal)item.ApprCost, MidpointRounding.AwayFromZero);

        //                            break;
        //                        }
        //                    default:
        //                        continue;
        //                }
        //            }
        //            foreach (RequestItems_Table item in reqList.FindAll(x => x.BU.Trim().Equals(BudgetingController.lstBUs.Find(bu => bu.BU.Equals("2WP")).ID.ToString())).FindAll(x => x.ApprovedDH == true).FindAll(dpt => BudgetingController.lstDEPTs.Find(x => x.ID.ToString().Contains(dpt.DEPT)).DEPT.Contains("CC")))
        //            {
        //                switch (BudgetingController.lstCostElement.Find(cost => cost.ID.Equals(Int32.Parse(item.CostElement))).CostElement)
        //                {
        //                    case "MAE":
        //                        {

        //                            TwoWP_MAE_Totals += Math.Round((decimal)item.ApprCost, MidpointRounding.AwayFromZero);

        //                            break;
        //                        }
        //                    case "Non-MAE":
        //                        {

        //                            TwoWP_NMAE_Totals += Math.Round((decimal)item.ApprCost, MidpointRounding.AwayFromZero);

        //                            break;
        //                        }
        //                    case "Software":
        //                        {

        //                            TwoWP_SoftwareTotals += Math.Round((decimal)item.ApprCost, MidpointRounding.AwayFromZero);

        //                            break;
        //                        }
        //                    default:
        //                        continue;
        //                }
        //            }

        //            SummaryView Category_View = new SummaryView();
        //            Category_View.Category = "MAE";
        //            Category_View.AS = AS_MAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //            Category_View.OSS = OSS_MAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //            //Category_View.DA = DA_MAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //            //Category_View.AD = AD_MAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //            Category_View.Two_Wheeler = TwoWP_MAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //            Category_View.Totals = (AS_MAE_Totals + OSS_MAE_Totals + DA_MAE_Totals + AD_MAE_Totals + TwoWP_MAE_Totals).ToString("C0", CultureInfo.CurrentCulture);

        //            viewList.Add(Category_View);
        //            Category_View = new SummaryView();
        //            Category_View.Category = "Non-MAE";
        //            Category_View.AS = AS_NMAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //            Category_View.OSS = OSS_NMAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //            //Category_View.DA = DA_NMAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //            //Category_View.AD = AD_NMAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //            Category_View.Two_Wheeler = TwoWP_NMAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //            Category_View.Totals = (AS_NMAE_Totals + OSS_NMAE_Totals + DA_NMAE_Totals + AD_NMAE_Totals + TwoWP_NMAE_Totals).ToString("C0", CultureInfo.CurrentCulture);

        //            viewList.Add(Category_View);
        //            Category_View = new SummaryView();
        //            Category_View.Category = "Software";
        //            Category_View.AS = AS_SoftwareTotals.ToString("C0", CultureInfo.CurrentCulture);
        //            Category_View.OSS = OSS_SoftwareTotals.ToString("C0", CultureInfo.CurrentCulture);
        //            //Category_View.DA = DA_SoftwareTotals.ToString("C0", CultureInfo.CurrentCulture);
        //            //Category_View.AD = AD_SoftwareTotals.ToString("C0", CultureInfo.CurrentCulture);
        //            Category_View.Two_Wheeler = TwoWP_SoftwareTotals.ToString("C0", CultureInfo.CurrentCulture);
        //            Category_View.Totals = (AS_SoftwareTotals + OSS_SoftwareTotals + DA_SoftwareTotals + AD_SoftwareTotals + TwoWP_SoftwareTotals).ToString("C0", CultureInfo.CurrentCulture);

        //            viewList.Add(Category_View);
        //            Category_View = new SummaryView();
        //            Category_View.Category = "Totals";
        //            Category_View.AS = (AS_MAE_Totals + AS_NMAE_Totals + AS_SoftwareTotals).ToString("C0", CultureInfo.CurrentCulture);
        //            Category_View.OSS = (OSS_MAE_Totals + OSS_NMAE_Totals + OSS_SoftwareTotals).ToString("C0", CultureInfo.CurrentCulture);
        //            //Category_View.DA = (DA_MAE_Totals + DA_NMAE_Totals + DA_SoftwareTotals).ToString("C0", CultureInfo.CurrentCulture);
        //            //Category_View.AD = (AD_MAE_Totals + AD_NMAE_Totals + AD_SoftwareTotals).ToString("C0", CultureInfo.CurrentCulture);
        //            Category_View.Two_Wheeler = (TwoWP_MAE_Totals + TwoWP_NMAE_Totals + TwoWP_SoftwareTotals).ToString("C0", CultureInfo.CurrentCulture);
        //            Category_View.Totals = ((AS_MAE_Totals + AS_NMAE_Totals + AS_SoftwareTotals) + (OSS_MAE_Totals + OSS_NMAE_Totals + OSS_SoftwareTotals) + (DA_MAE_Totals + DA_NMAE_Totals + DA_SoftwareTotals) +
        //                (AD_MAE_Totals + AD_NMAE_Totals + AD_SoftwareTotals) + (TwoWP_MAE_Totals + TwoWP_NMAE_Totals + TwoWP_SoftwareTotals)).ToString("C0", CultureInfo.CurrentCulture);

        //            viewList.Add(Category_View);


        //        }
        //        else if (presentUserDept.Contains("XC"))
        //        {
        //            foreach (RequestItems_Table item in reqList.FindAll(x => x.BU.Trim().Equals(BudgetingController.lstBUs.Find(bu => bu.BU.Equals("DA")).ID.ToString())).FindAll(x => x.ApprovedDH == true))
        //            {
        //                switch (BudgetingController.lstCostElement.Find(cost => cost.ID.Equals(Int32.Parse(item.CostElement))).CostElement)
        //                {
        //                    case "MAE":
        //                        {

        //                            DA_MAE_Totals += Math.Round((decimal)item.ApprCost, MidpointRounding.AwayFromZero);

        //                            break;
        //                        }
        //                    case "Non-MAE":
        //                        {

        //                            DA_NMAE_Totals += Math.Round((decimal)item.ApprCost, MidpointRounding.AwayFromZero);

        //                            break;
        //                        }
        //                    case "Software":
        //                        {

        //                            DA_SoftwareTotals += Math.Round((decimal)item.ApprCost, MidpointRounding.AwayFromZero);

        //                            break;
        //                        }
        //                    default:
        //                        continue;
        //                }
        //            }
        //            foreach (RequestItems_Table item in reqList.FindAll(x => x.BU.Trim().Equals(BudgetingController.lstBUs.Find(bu => bu.BU.Equals("AD")).ID.ToString())).FindAll(x => x.ApprovedDH == true))
        //            {
        //                switch (BudgetingController.lstCostElement.Find(cost => cost.ID.Equals(Int32.Parse(item.CostElement))).CostElement)
        //                {
        //                    case "MAE":
        //                        {

        //                            AD_MAE_Totals += Math.Round((decimal)item.ApprCost, MidpointRounding.AwayFromZero);

        //                            break;
        //                        }
        //                    case "Non-MAE":
        //                        {

        //                            AD_NMAE_Totals += Math.Round((decimal)item.ApprCost, MidpointRounding.AwayFromZero);

        //                            break;
        //                        }
        //                    case "Software":
        //                        {

        //                            AD_SoftwareTotals += Math.Round((decimal)item.ApprCost, MidpointRounding.AwayFromZero);

        //                            break;
        //                        }
        //                    default:
        //                        continue;
        //                }
        //            }
        //            foreach (RequestItems_Table item in reqList.FindAll(x => x.BU.Trim().Equals(BudgetingController.lstBUs.Find(bu => bu.BU.Equals("2WP")).ID.ToString())).FindAll(x => x.ApprovedDH == true).FindAll(dpt => BudgetingController.lstDEPTs.Find(x => x.ID.ToString().Contains(dpt.DEPT)).DEPT.Contains("XC")))
        //            {
        //                switch (BudgetingController.lstCostElement.Find(cost => cost.ID.Equals(Int32.Parse(item.CostElement))).CostElement)
        //                {
        //                    case "MAE":
        //                        {

        //                            TwoWP_MAE_Totals += Math.Round((decimal)item.ApprCost, MidpointRounding.AwayFromZero);

        //                            break;
        //                        }
        //                    case "Non-MAE":
        //                        {

        //                            TwoWP_NMAE_Totals += (decimal)item.ApprCost;

        //                            break;
        //                        }
        //                    case "Software":
        //                        {

        //                            TwoWP_SoftwareTotals += (decimal)item.ApprCost;

        //                            break;
        //                        }
        //                    default:
        //                        continue;
        //                }
        //            }

        //            SummaryView Category_View = new SummaryView();
        //            Category_View.Category = "MAE";
        //            //Category_View.AS = AS_MAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //            //Category_View.OSS = OSS_MAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //            Category_View.DA = DA_MAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //            Category_View.AD = AD_MAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //            Category_View.Two_Wheeler = TwoWP_MAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //            Category_View.Totals = (AS_MAE_Totals + OSS_MAE_Totals + DA_MAE_Totals + AD_MAE_Totals + TwoWP_MAE_Totals).ToString("C0", CultureInfo.CurrentCulture);

        //            viewList.Add(Category_View);
        //            Category_View = new SummaryView();
        //            Category_View.Category = "Non-MAE";
        //            //Category_View.AS = AS_NMAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //            //Category_View.OSS = OSS_NMAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //            Category_View.DA = DA_NMAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //            Category_View.AD = AD_NMAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //            Category_View.Two_Wheeler = TwoWP_NMAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //            Category_View.Totals = (AS_NMAE_Totals + OSS_NMAE_Totals + DA_NMAE_Totals + AD_NMAE_Totals + TwoWP_NMAE_Totals).ToString("C0", CultureInfo.CurrentCulture);

        //            viewList.Add(Category_View);
        //            Category_View = new SummaryView();
        //            Category_View.Category = "Software";
        //            //Category_View.AS = AS_SoftwareTotals.ToString("C0", CultureInfo.CurrentCulture);
        //            //Category_View.OSS = OSS_SoftwareTotals.ToString("C0", CultureInfo.CurrentCulture);
        //            Category_View.DA = DA_SoftwareTotals.ToString("C0", CultureInfo.CurrentCulture);
        //            Category_View.AD = AD_SoftwareTotals.ToString("C0", CultureInfo.CurrentCulture);
        //            Category_View.Two_Wheeler = TwoWP_SoftwareTotals.ToString("C0", CultureInfo.CurrentCulture);
        //            Category_View.Totals = (AS_SoftwareTotals + OSS_SoftwareTotals + DA_SoftwareTotals + AD_SoftwareTotals + TwoWP_SoftwareTotals).ToString("C0", CultureInfo.CurrentCulture);

        //            viewList.Add(Category_View);
        //            Category_View = new SummaryView();
        //            Category_View.Category = "Totals";
        //            //Category_View.AS = (AS_MAE_Totals + AS_NMAE_Totals + AS_SoftwareTotals).ToString("C0", CultureInfo.CurrentCulture);
        //            //Category_View.OSS = (OSS_MAE_Totals + OSS_NMAE_Totals + OSS_SoftwareTotals).ToString("C0", CultureInfo.CurrentCulture);
        //            Category_View.DA = (DA_MAE_Totals + DA_NMAE_Totals + DA_SoftwareTotals).ToString("C0", CultureInfo.CurrentCulture);
        //            Category_View.AD = (AD_MAE_Totals + AD_NMAE_Totals + AD_SoftwareTotals).ToString("C0", CultureInfo.CurrentCulture);
        //            Category_View.Two_Wheeler = (TwoWP_MAE_Totals + TwoWP_NMAE_Totals + TwoWP_SoftwareTotals).ToString("C0", CultureInfo.CurrentCulture);
        //            Category_View.Totals = ((AS_MAE_Totals + AS_NMAE_Totals + AS_SoftwareTotals) + (OSS_MAE_Totals + OSS_NMAE_Totals + OSS_SoftwareTotals) + (DA_MAE_Totals + DA_NMAE_Totals + DA_SoftwareTotals) +
        //                (AD_MAE_Totals + AD_NMAE_Totals + AD_SoftwareTotals) + (TwoWP_MAE_Totals + TwoWP_NMAE_Totals + TwoWP_SoftwareTotals)).ToString("C0", CultureInfo.CurrentCulture);

        //            viewList.Add(Category_View);

        //        }
        //        else
        //        {
        //            foreach (RequestItems_Table item in reqList.FindAll(x => x.BU.Equals(BudgetingController.lstBUs.Find(bu => bu.BU.Trim().Equals("MB")).ID.ToString())).FindAll(x => x.ApprovedDH == true))
        //            {
        //                switch (BudgetingController.lstCostElement.Find(cost => cost.ID.Equals(Int32.Parse(item.CostElement))).CostElement)
        //                {
        //                    case "MAE":
        //                        {

        //                            AS_MAE_Totals += (decimal)item.ApprCost;

        //                            break;
        //                        }
        //                    case "Non-MAE":
        //                        {

        //                            AS_NMAE_Totals += (decimal)item.ApprCost;

        //                            break;
        //                        }
        //                    case "Software":
        //                        {

        //                            AS_SoftwareTotals += (decimal)item.ApprCost;

        //                            break;
        //                        }
        //                    default:
        //                        continue;
        //                }
        //            }
        //            foreach (RequestItems_Table item in reqList.FindAll(x => x.BU.Trim().Equals(BudgetingController.lstBUs.Find(bu => bu.BU.Equals("OSS")).ID.ToString())).FindAll(x => x.ApprovedDH == true))
        //            {
        //                switch (BudgetingController.lstCostElement.Find(cost => cost.ID.Equals(Int32.Parse(item.CostElement))).CostElement)
        //                {
        //                    case "MAE":
        //                        {

        //                            OSS_MAE_Totals += (decimal)item.ApprCost;

        //                            break;
        //                        }
        //                    case "Non-MAE":
        //                        {

        //                            OSS_NMAE_Totals += (decimal)item.ApprCost;

        //                            break;
        //                        }
        //                    case "Software":
        //                        {

        //                            OSS_SoftwareTotals += (decimal)item.ApprCost;

        //                            break;
        //                        }
        //                    default:
        //                        continue;
        //                }
        //            }
        //            foreach (RequestItems_Table item in reqList.FindAll(x => x.BU.Trim().Equals(BudgetingController.lstBUs.Find(bu => bu.BU.Equals("DA")).ID.ToString())).FindAll(x => x.ApprovedDH == true))
        //            {
        //                switch (BudgetingController.lstCostElement.Find(cost => cost.ID.Equals(Int32.Parse(item.CostElement))).CostElement)
        //                {
        //                    case "MAE":
        //                        {

        //                            DA_MAE_Totals += (decimal)item.ApprCost;

        //                            break;
        //                        }
        //                    case "Non-MAE":
        //                        {

        //                            DA_NMAE_Totals += (decimal)item.ApprCost;

        //                            break;
        //                        }
        //                    case "Software":
        //                        {

        //                            DA_SoftwareTotals += (decimal)item.ApprCost;

        //                            break;
        //                        }
        //                    default:
        //                        continue;
        //                }
        //            }
        //            foreach (RequestItems_Table item in reqList.FindAll(x => x.BU.Trim().Equals(BudgetingController.lstBUs.Find(bu => bu.BU.Equals("AD")).ID.ToString())).FindAll(x => x.ApprovedDH == true))
        //            {
        //                switch (BudgetingController.lstCostElement.Find(cost => cost.ID.Equals(Int32.Parse(item.CostElement))).CostElement)
        //                {
        //                    case "MAE":
        //                        {

        //                            AD_MAE_Totals += (decimal)item.ApprCost;

        //                            break;
        //                        }
        //                    case "Non-MAE":
        //                        {

        //                            AD_NMAE_Totals += (decimal)item.ApprCost;

        //                            break;
        //                        }
        //                    case "Software":
        //                        {

        //                            AD_SoftwareTotals += (decimal)item.ApprCost;

        //                            break;
        //                        }
        //                    default:
        //                        continue;
        //                }
        //            }
        //            foreach (RequestItems_Table item in reqList.FindAll(x => x.BU.Trim().Equals(BudgetingController.lstBUs.Find(bu => bu.BU.Equals("2WP")).ID.ToString())).FindAll(x => x.ApprovedDH == true))
        //            {
        //                switch (BudgetingController.lstCostElement.Find(cost => cost.ID.Equals(Int32.Parse(item.CostElement))).CostElement)
        //                {
        //                    case "MAE":
        //                        {

        //                            TwoWP_MAE_Totals += (decimal)item.ApprCost;

        //                            break;
        //                        }
        //                    case "Non-MAE":
        //                        {

        //                            TwoWP_NMAE_Totals += (decimal)item.ApprCost;

        //                            break;
        //                        }
        //                    case "Software":
        //                        {

        //                            TwoWP_SoftwareTotals += (decimal)item.ApprCost;

        //                            break;
        //                        }
        //                    default:
        //                        continue;
        //                }
        //            }

        //            SummaryView Category_View = new SummaryView();
        //            Category_View.Category = "MAE";
        //            Category_View.AS = AS_MAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //            Category_View.OSS = OSS_MAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //            Category_View.DA = DA_MAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //            Category_View.AD = AD_MAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //            Category_View.Two_Wheeler = TwoWP_MAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //            Category_View.Totals = (AS_MAE_Totals + OSS_MAE_Totals + DA_MAE_Totals + AD_MAE_Totals + TwoWP_MAE_Totals).ToString("C0", CultureInfo.CurrentCulture);

        //            viewList.Add(Category_View);
        //            Category_View = new SummaryView();
        //            Category_View.Category = "Non-MAE";
        //            Category_View.AS = AS_NMAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //            Category_View.OSS = OSS_NMAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //            Category_View.DA = DA_NMAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //            Category_View.AD = AD_NMAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //            Category_View.Two_Wheeler = TwoWP_NMAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //            Category_View.Totals = (AS_NMAE_Totals + OSS_NMAE_Totals + DA_NMAE_Totals + AD_NMAE_Totals + TwoWP_NMAE_Totals).ToString("C0", CultureInfo.CurrentCulture);

        //            viewList.Add(Category_View);
        //            Category_View = new SummaryView();
        //            Category_View.Category = "Software";
        //            Category_View.AS = AS_SoftwareTotals.ToString("C0", CultureInfo.CurrentCulture);
        //            Category_View.OSS = OSS_SoftwareTotals.ToString("C0", CultureInfo.CurrentCulture);
        //            Category_View.DA = DA_SoftwareTotals.ToString("C0", CultureInfo.CurrentCulture);
        //            Category_View.AD = AD_SoftwareTotals.ToString("C0", CultureInfo.CurrentCulture);
        //            Category_View.Two_Wheeler = TwoWP_SoftwareTotals.ToString("C0", CultureInfo.CurrentCulture);
        //            Category_View.Totals = (AS_SoftwareTotals + OSS_SoftwareTotals + DA_SoftwareTotals + AD_SoftwareTotals + TwoWP_SoftwareTotals).ToString("C0", CultureInfo.CurrentCulture);

        //            viewList.Add(Category_View);
        //            Category_View = new SummaryView();
        //            Category_View.Category = "Totals";
        //            Category_View.AS = (AS_MAE_Totals + AS_NMAE_Totals + AS_SoftwareTotals).ToString("C0", CultureInfo.CurrentCulture);
        //            Category_View.OSS = (OSS_MAE_Totals + OSS_NMAE_Totals + OSS_SoftwareTotals).ToString("C0", CultureInfo.CurrentCulture);
        //            Category_View.DA = (DA_MAE_Totals + DA_NMAE_Totals + DA_SoftwareTotals).ToString("C0", CultureInfo.CurrentCulture);
        //            Category_View.AD = (AD_MAE_Totals + AD_NMAE_Totals + AD_SoftwareTotals).ToString("C0", CultureInfo.CurrentCulture);
        //            Category_View.Two_Wheeler = (TwoWP_MAE_Totals + TwoWP_NMAE_Totals + TwoWP_SoftwareTotals).ToString("C0", CultureInfo.CurrentCulture);
        //            Category_View.Totals = ((AS_MAE_Totals + AS_NMAE_Totals + AS_SoftwareTotals) + (OSS_MAE_Totals + OSS_NMAE_Totals + OSS_SoftwareTotals) + (DA_MAE_Totals + DA_NMAE_Totals + DA_SoftwareTotals) +
        //                (AD_MAE_Totals + AD_NMAE_Totals + AD_SoftwareTotals) + (TwoWP_MAE_Totals + TwoWP_NMAE_Totals + TwoWP_SoftwareTotals)).ToString("C0", CultureInfo.CurrentCulture);

        //            viewList.Add(Category_View);

        //        }
        //        return viewList;




        //    }
        //}







        ////for CC BU Summary in EXPORT 
        //public List<SummaryView_CC> BUSummaryData_CC(string year)
        //{
        //    string presentUserDept = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).Department;

        //    //presentUserDept = "XC";

        //    using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //    //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //    {
        //        List<RequestItems_Table> reqList = db.RequestItems_Table.ToList<RequestItems_Table>().FindAll(x => x.RequestDate.ToString().Contains(year));
        //        List<SummaryView> viewList = new List<SummaryView>();
        //        List<SummaryView_CC> viewList_CC = new List<SummaryView_CC>();
        //        List<SummaryView_XC> viewList_XC = new List<SummaryView_XC>();

        //        decimal AS_MAE_Totals = 0, AS_NMAE_Totals = 0, AS_SoftwareTotals = 0;
        //        decimal OSS_MAE_Totals = 0, OSS_NMAE_Totals = 0, OSS_SoftwareTotals = 0;
        //        decimal DA_MAE_Totals = 0, DA_NMAE_Totals = 0, DA_SoftwareTotals = 0;
        //        decimal AD_MAE_Totals = 0, AD_NMAE_Totals = 0, AD_SoftwareTotals = 0;
        //        decimal TwoWP_MAE_Totals = 0, TwoWP_NMAE_Totals = 0, TwoWP_SoftwareTotals = 0;



        //            foreach (RequestItems_Table item in reqList.FindAll(x => x.BU.Equals(BudgetingController.lstBUs.Find(bu => bu.BU.Trim().Equals("AS")).ID.ToString())).FindAll(x => x.ApprovedDH == true))
        //            {
        //                switch (BudgetingController.lstCostElement.Find(cost => cost.ID.Equals(Int32.Parse(item.CostElement))).CostElement)
        //                {
        //                    case "MAE":
        //                        {

        //                            AS_MAE_Totals += (decimal)item.ApprCost;

        //                            break;
        //                        }
        //                    case "Non-MAE":
        //                        {

        //                            AS_NMAE_Totals += (decimal)item.ApprCost;

        //                            break;
        //                        }
        //                    case "Software":
        //                        {

        //                            AS_SoftwareTotals += (decimal)item.ApprCost;

        //                            break;
        //                        }
        //                    default:
        //                        continue;
        //                }
        //            }
        //            foreach (RequestItems_Table item in reqList.FindAll(x => x.BU.Trim().Equals(BudgetingController.lstBUs.Find(bu => bu.BU.Equals("OSS")).ID.ToString())).FindAll(x => x.ApprovedDH == true))
        //            {
        //                switch (BudgetingController.lstCostElement.Find(cost => cost.ID.Equals(Int32.Parse(item.CostElement))).CostElement)
        //                {
        //                    case "MAE":
        //                        {

        //                            OSS_MAE_Totals += (decimal)item.ApprCost;

        //                            break;
        //                        }
        //                    case "Non-MAE":
        //                        {

        //                            OSS_NMAE_Totals += (decimal)item.ApprCost;

        //                            break;
        //                        }
        //                    case "Software":
        //                        {

        //                            OSS_SoftwareTotals += (decimal)item.ApprCost;

        //                            break;
        //                        }
        //                    default:
        //                        continue;
        //                }
        //            }
        //            foreach (RequestItems_Table item in reqList.FindAll(x => x.BU.Trim().Equals(BudgetingController.lstBUs.Find(bu => bu.BU.Equals("2WP")).ID.ToString())).FindAll(x => x.ApprovedDH == true).FindAll(dpt => BudgetingController.lstDEPTs.Find(x => x.ID.ToString().Contains(dpt.DEPT)).DEPT.Contains("CC")))
        //            {
        //                switch (BudgetingController.lstCostElement.Find(cost => cost.ID.Equals(Int32.Parse(item.CostElement))).CostElement)
        //                {
        //                    case "MAE":
        //                        {

        //                            TwoWP_MAE_Totals += (decimal)item.ApprCost;

        //                            break;
        //                        }
        //                    case "Non-MAE":
        //                        {

        //                            TwoWP_NMAE_Totals += (decimal)item.ApprCost;

        //                            break;
        //                        }
        //                    case "Software":
        //                        {

        //                            TwoWP_SoftwareTotals += (decimal)item.ApprCost;

        //                            break;
        //                        }
        //                    default:
        //                        continue;
        //                }
        //            }

        //            SummaryView_CC Category_View = new SummaryView_CC();
        //            Category_View.Category = "MAE";
        //            Category_View.AS = AS_MAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //            Category_View.OSS = OSS_MAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //            //Category_View.DA = DA_MAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //            //Category_View.AD = AD_MAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //            Category_View.Two_Wheeler = TwoWP_MAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //            Category_View.Totals = (AS_MAE_Totals + OSS_MAE_Totals + DA_MAE_Totals + AD_MAE_Totals + TwoWP_MAE_Totals).ToString("C0", CultureInfo.CurrentCulture);

        //            viewList_CC.Add(Category_View);
        //            Category_View = new SummaryView_CC();
        //            Category_View.Category = "Non-MAE";
        //            Category_View.AS = AS_NMAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //            Category_View.OSS = OSS_NMAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //            //Category_View.DA = DA_NMAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //            //Category_View.AD = AD_NMAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //            Category_View.Two_Wheeler = TwoWP_NMAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //            Category_View.Totals = (AS_NMAE_Totals + OSS_NMAE_Totals + DA_NMAE_Totals + AD_NMAE_Totals + TwoWP_NMAE_Totals).ToString("C0", CultureInfo.CurrentCulture);

        //            viewList_CC.Add(Category_View);
        //            Category_View = new SummaryView_CC();
        //            Category_View.Category = "Software";
        //            Category_View.AS = AS_SoftwareTotals.ToString("C0", CultureInfo.CurrentCulture);
        //            Category_View.OSS = OSS_SoftwareTotals.ToString("C0", CultureInfo.CurrentCulture);
        //            //Category_View.DA = DA_SoftwareTotals.ToString("C0", CultureInfo.CurrentCulture);
        //            //Category_View.AD = AD_SoftwareTotals.ToString("C0", CultureInfo.CurrentCulture);
        //            Category_View.Two_Wheeler = TwoWP_SoftwareTotals.ToString("C0", CultureInfo.CurrentCulture);
        //            Category_View.Totals = (AS_SoftwareTotals + OSS_SoftwareTotals + DA_SoftwareTotals + AD_SoftwareTotals + TwoWP_SoftwareTotals).ToString("C0", CultureInfo.CurrentCulture);

        //            viewList_CC.Add(Category_View);
        //            Category_View = new SummaryView_CC();
        //            Category_View.Category = "Totals";
        //            Category_View.AS = (AS_MAE_Totals + AS_NMAE_Totals + AS_SoftwareTotals).ToString("C0", CultureInfo.CurrentCulture);
        //            Category_View.OSS = (OSS_MAE_Totals + OSS_NMAE_Totals + OSS_SoftwareTotals).ToString("C0", CultureInfo.CurrentCulture);
        //            //Category_View.DA = (DA_MAE_Totals + DA_NMAE_Totals + DA_SoftwareTotals).ToString("C0", CultureInfo.CurrentCulture);
        //            //Category_View.AD = (AD_MAE_Totals + AD_NMAE_Totals + AD_SoftwareTotals).ToString("C0", CultureInfo.CurrentCulture);
        //            Category_View.Two_Wheeler = (TwoWP_MAE_Totals + TwoWP_NMAE_Totals + TwoWP_SoftwareTotals).ToString("C0", CultureInfo.CurrentCulture);
        //            Category_View.Totals = ((AS_MAE_Totals + AS_NMAE_Totals + AS_SoftwareTotals) + (OSS_MAE_Totals + OSS_NMAE_Totals + OSS_SoftwareTotals) + (DA_MAE_Totals + DA_NMAE_Totals + DA_SoftwareTotals) +
        //                (AD_MAE_Totals + AD_NMAE_Totals + AD_SoftwareTotals) + (TwoWP_MAE_Totals + TwoWP_NMAE_Totals + TwoWP_SoftwareTotals)).ToString("C0", CultureInfo.CurrentCulture);

        //            viewList_CC.Add(Category_View);
        //            return viewList_CC;








        //    }
        //}
        ////for XC BU Summary in EXPORT
        //public List<SummaryView_XC> BUSummaryData_XC(string year)
        //{
        //    string presentUserDept = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).Department;

        //    //presentUserDept = "XC";

        //    using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //    //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //    {
        //        List<RequestItems_Table> reqList = db.RequestItems_Table.ToList<RequestItems_Table>().FindAll(x => x.RequestDate.ToString().Contains(year));

        //        List<SummaryView_XC> viewList_XC = new List<SummaryView_XC>();

        //        decimal AS_MAE_Totals = 0, AS_NMAE_Totals = 0, AS_SoftwareTotals = 0;
        //        decimal OSS_MAE_Totals = 0, OSS_NMAE_Totals = 0, OSS_SoftwareTotals = 0;
        //        decimal DA_MAE_Totals = 0, DA_NMAE_Totals = 0, DA_SoftwareTotals = 0;
        //        decimal AD_MAE_Totals = 0, AD_NMAE_Totals = 0, AD_SoftwareTotals = 0;
        //        decimal TwoWP_MAE_Totals = 0, TwoWP_NMAE_Totals = 0, TwoWP_SoftwareTotals = 0;


        //            foreach (RequestItems_Table item in reqList.FindAll(x => x.BU.Trim().Equals(BudgetingController.lstBUs.Find(bu => bu.BU.Equals("DA")).ID.ToString())).FindAll(x => x.ApprovedDH == true))
        //            {
        //                switch (BudgetingController.lstCostElement.Find(cost => cost.ID.Equals(Int32.Parse(item.CostElement))).CostElement)
        //                {
        //                    case "MAE":
        //                        {

        //                            DA_MAE_Totals += (decimal)item.ApprCost;

        //                            break;
        //                        }
        //                    case "Non-MAE":
        //                        {

        //                            DA_NMAE_Totals += (decimal)item.ApprCost;

        //                            break;
        //                        }
        //                    case "Software":
        //                        {

        //                            DA_SoftwareTotals += (decimal)item.ApprCost;

        //                            break;
        //                        }
        //                    default:
        //                        continue;
        //                }
        //            }
        //            foreach (RequestItems_Table item in reqList.FindAll(x => x.BU.Trim().Equals(BudgetingController.lstBUs.Find(bu => bu.BU.Equals("AD")).ID.ToString())).FindAll(x => x.ApprovedDH == true))
        //            {
        //                switch (BudgetingController.lstCostElement.Find(cost => cost.ID.Equals(Int32.Parse(item.CostElement))).CostElement)
        //                {
        //                    case "MAE":
        //                        {

        //                            AD_MAE_Totals += (decimal)item.ApprCost;

        //                            break;
        //                        }
        //                    case "Non-MAE":
        //                        {

        //                            AD_NMAE_Totals += (decimal)item.ApprCost;

        //                            break;
        //                        }
        //                    case "Software":
        //                        {

        //                            AD_SoftwareTotals += (decimal)item.ApprCost;

        //                            break;
        //                        }
        //                    default:
        //                        continue;
        //                }
        //            }
        //            foreach (RequestItems_Table item in reqList.FindAll(x => x.BU.Trim().Equals(BudgetingController.lstBUs.Find(bu => bu.BU.Equals("2WP")).ID.ToString())).FindAll(x => x.ApprovedDH == true).FindAll(dpt => BudgetingController.lstDEPTs.Find(x => x.ID.ToString().Contains(dpt.DEPT)).DEPT.Contains("XC")))
        //            {
        //                switch (BudgetingController.lstCostElement.Find(cost => cost.ID.Equals(Int32.Parse(item.CostElement))).CostElement)
        //                {
        //                    case "MAE":
        //                        {

        //                            TwoWP_MAE_Totals += (decimal)item.ApprCost;

        //                            break;
        //                        }
        //                    case "Non-MAE":
        //                        {

        //                            TwoWP_NMAE_Totals += (decimal)item.ApprCost;

        //                            break;
        //                        }
        //                    case "Software":
        //                        {

        //                            TwoWP_SoftwareTotals += (decimal)item.ApprCost;

        //                            break;
        //                        }
        //                    default:
        //                        continue;
        //                }
        //            }

        //            SummaryView_XC Category_View = new SummaryView_XC();
        //            Category_View.Category = "MAE";
        //            //Category_View.AS = AS_MAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //            //Category_View.OSS = OSS_MAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //            Category_View.DA = DA_MAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //            Category_View.AD = AD_MAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //            Category_View.Two_Wheeler = TwoWP_MAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //            Category_View.Totals = (AS_MAE_Totals + OSS_MAE_Totals + DA_MAE_Totals + AD_MAE_Totals + TwoWP_MAE_Totals).ToString("C0", CultureInfo.CurrentCulture);

        //            viewList_XC.Add(Category_View);
        //            Category_View = new SummaryView_XC();
        //            Category_View.Category = "Non-MAE";
        //            //Category_View.AS = AS_NMAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //            //Category_View.OSS = OSS_NMAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //            Category_View.DA = DA_NMAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //            Category_View.AD = AD_NMAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //            Category_View.Two_Wheeler = TwoWP_NMAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //            Category_View.Totals = (AS_NMAE_Totals + OSS_NMAE_Totals + DA_NMAE_Totals + AD_NMAE_Totals + TwoWP_NMAE_Totals).ToString("C0", CultureInfo.CurrentCulture);

        //            viewList_XC.Add(Category_View);
        //            Category_View = new SummaryView_XC();
        //            Category_View.Category = "Software";
        //            //Category_View.AS = AS_SoftwareTotals.ToString("C0", CultureInfo.CurrentCulture);
        //            //Category_View.OSS = OSS_SoftwareTotals.ToString("C0", CultureInfo.CurrentCulture);
        //            Category_View.DA = DA_SoftwareTotals.ToString("C0", CultureInfo.CurrentCulture);
        //            Category_View.AD = AD_SoftwareTotals.ToString("C0", CultureInfo.CurrentCulture);
        //            Category_View.Two_Wheeler = TwoWP_SoftwareTotals.ToString("C0", CultureInfo.CurrentCulture);
        //            Category_View.Totals = (AS_SoftwareTotals + OSS_SoftwareTotals + DA_SoftwareTotals + AD_SoftwareTotals + TwoWP_SoftwareTotals).ToString("C0", CultureInfo.CurrentCulture);

        //            viewList_XC.Add(Category_View);
        //            Category_View = new SummaryView_XC();
        //            Category_View.Category = "Totals";
        //            //Category_View.AS = (AS_MAE_Totals + AS_NMAE_Totals + AS_SoftwareTotals).ToString("C0", CultureInfo.CurrentCulture);
        //            //Category_View.OSS = (OSS_MAE_Totals + OSS_NMAE_Totals + OSS_SoftwareTotals).ToString("C0", CultureInfo.CurrentCulture);
        //            Category_View.DA = (DA_MAE_Totals + DA_NMAE_Totals + DA_SoftwareTotals).ToString("C0", CultureInfo.CurrentCulture);
        //            Category_View.AD = (AD_MAE_Totals + AD_NMAE_Totals + AD_SoftwareTotals).ToString("C0", CultureInfo.CurrentCulture);
        //            Category_View.Two_Wheeler = (TwoWP_MAE_Totals + TwoWP_NMAE_Totals + TwoWP_SoftwareTotals).ToString("C0", CultureInfo.CurrentCulture);
        //            Category_View.Totals = ((AS_MAE_Totals + AS_NMAE_Totals + AS_SoftwareTotals) + (OSS_MAE_Totals + OSS_NMAE_Totals + OSS_SoftwareTotals) + (DA_MAE_Totals + DA_NMAE_Totals + DA_SoftwareTotals) +
        //                (AD_MAE_Totals + AD_NMAE_Totals + AD_SoftwareTotals) + (TwoWP_MAE_Totals + TwoWP_NMAE_Totals + TwoWP_SoftwareTotals)).ToString("C0", CultureInfo.CurrentCulture);

        //            viewList_XC.Add(Category_View);
        //        return viewList_XC;


        //    }
        //}






        /// <summary>
        /// Function to send BU summary data to view
        /// </summary>
        /// <returns></returns>
        public ActionResult GetBUSummaryData(string year, bool chart = false)
        {
            BudgetParam t = new BudgetParam();
            string message = string.Empty;
            System.Data.DataTable dt = new System.Data.DataTable();
            var isCC_XC = string.Empty;
            try
            {
                if (year.Contains("2020"))
                    return Json(new { data = t, message = "" }, JsonRequestBehavior.AllowGet);

                string presentUserDept = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).Department;
                //presentUserDept = "XC";


                List<string> years = new List<string>()
                { DateTime.Now.Year.ToString(), DateTime.Now.AddYears(1).Year.ToString() }; //VKM Yr , VKM Yr-1 (Previous VKM Year)

                if (presentUserDept.Contains("XC"))
                {
                    dt = XC_BUSummaryComparison(years, chart);
                    isCC_XC = "XC";
                }
                else
                {
                    dt = CC_BUSummaryComparison(years, chart);
                    isCC_XC = "CC";
                }





                var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                //BudgetParam t = new BudgetParam();
                List<columnsinfo> _col = new List<columnsinfo>();

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
                return Json(new { data = t, message = isCC_XC }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                WriteLog("Error - DeptSummaryAllData : NTID - " + User.Identity.Name.Split('\\')[1].Trim().ToUpper() + ", ErrorMsg : " + ex + ", At - " + DateTime.Now);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }


            // return Json(new { data = BUSummaryData(year), message }, JsonRequestBehavior.AllowGet);
        }

        public System.Data.DataTable CC_BUSummaryComparison(List<string> years, bool chart = false)//years = vkmyears 2021 and 2022(current)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            try
            {
                //string presentUserDept = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).Department;

                using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

                //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

                {

                    List<BUSummary_CC> viewList = new List<BUSummary_CC>();



                    foreach (string yr in years)
                    {
                        decimal AS_MAE_Totals = 0, AS_NMAE_Totals = 0, AS_SoftwareTotals = 0;
                        decimal OSS_MAE_Totals = 0, OSS_NMAE_Totals = 0, OSS_SoftwareTotals = 0;
                        decimal TwoWP_MAE_Totals = 0, TwoWP_NMAE_Totals = 0, TwoWP_SoftwareTotals = 0;

                        List<RequestItems_Table> reqList = db.RequestItems_Table.ToList<RequestItems_Table>().FindAll(x => x.RequestDate.ToString().Contains((int.Parse(yr) - 1).ToString())).FindAll(v => v.Fund == null || v.Fund.Trim().Contains("2")); //only f02 to be included
                                                                                                                                                                                                                                                           // var mmmmmm = db.RequestItems_Table.ToList().FindAll(v => v.Project != null && v.Project.Trim().Contains("test"));
                        string Query = "";                                                                                                                                                                                                                                   // var mmmmmm = db.RequestItems_Table.ToList().FindAll(v => v.Project != null && v.Project.Trim().Contains("test"));
                        if (int.Parse(yr) - 1 > 2020)
                        {
                            //var xxxx = reqList.FindAll(v => v.Project != null && v.Project.Trim().Contains("test"));
                            //var yyyy = reqList.FindAll(v => v.Fund != null && v.Fund.Trim().Contains("3"));
                            Query = "Select * from RequestItems_Table where VKM_Year = " + yr + " and BU in (1, 3, 5) and (Fund is null or Fund = 2) and ApprovedDH = 1 and DEPT in (Select id from DEPT_Table where DEPT not like '%-XC%')";
                        }
                        else
                        {
                            //List<RequestItems_Table> reqList = db.RequestItems_Table.ToList<RequestItems_Table>().FindAll(x => x.RequestDate.ToString().Contains((int.Parse(yr) - 1).ToString())).FindAll(v => v.Fund == null || v.Fund.Trim().Contains("2")); //only f02 to be included
                            Query = "Select * from RequestItems_Table where VKM_Year = " + (Convert.ToInt32(yr) - 1) + " and (Fund is null or Fund = 2) "; //only f02 to be included
                        }
                        //reqList = reqList.FindAll(dpt => !BudgetingController.lstDEPTs.Find(x => x.ID.ToString().Contains(dpt.DEPT)).DEPT.Contains("XC"));
                        DataTable dt1 = new DataTable();
                        connection();

                        OpenConnection();
                        SqlCommand cmd = new SqlCommand(Query, conn);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt1);
                        CloseConnection();
                        reqList = new List<RequestItems_Table>();
                        foreach (DataRow item in dt1.Rows)
                        {
                            try
                            {

                                RequestItems_Table ritem = new RequestItems_Table();
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
                                ritem.BudgetCode = item["BudgetCode"].ToString();
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


                                reqList.Add(ritem);

                            }
                            catch (Exception ex)
                            {

                            }

                        }
                        //!lstDEPTs.Find(x => x.ID.ToString().Contains(dpt.DEPT)).DEPT.Contains("XC"))

                        //reqList = reqList.FindAll(dpt => !BudgetingController.lstDEPTs.Find(x => x.ID.ToString().Contains(dpt.DEPT)).DEPT.Contains("XC")).FindAll(fun => fun.Fund == null || fun.Fund.Trim() == "2");
                        //reqList = reqList.FindAll(dpt => !BudgetingController.lstDEPTs.Find(x => x.ID.ToString().Contains(dpt.DEPT)).DEPT.Contains("XC")).FindAll(fun => !fun.Fund.Contains("3") || !fun.Fund.Contains("1"));



                        //foreach (RequestItems_Table item in reqList.FindAll(x => x.BU.Equals(BudgetingController.lstBUs.Find(bu => bu.BU.Trim().Equals("MB")).ID.ToString())).FindAll(x => x.ApprovedDH == true))
                        //{
                        //    switch (BudgetingController.lstCostElement.Find(cost => cost.ID.Equals(Int32.Parse(item.CostElement))).CostElement)
                        //    {
                        //        case "MAE":
                        //            {

                        //                AS_MAE_Totals += Math.Round((decimal)item.ApprCost, MidpointRounding.AwayFromZero);

                        //                break;
                        //            }
                        //        case "Non-MAE":
                        //            {

                        //                AS_NMAE_Totals += Math.Round((decimal)item.ApprCost, MidpointRounding.AwayFromZero);

                        //                break;
                        //            }
                        //        case "Software":
                        //            {

                        //                AS_SoftwareTotals += Math.Round((decimal)item.ApprCost, MidpointRounding.AwayFromZero);

                        //                break;
                        //            }
                        //        default:
                        //            continue;
                        //    }
                        //}
                        //foreach (RequestItems_Table item in reqList.FindAll(x => x.BU.Trim().Equals(BudgetingController.lstBUs.Find(bu => bu.BU.Equals("OSS")).ID.ToString())).FindAll(x => x.ApprovedDH == true))
                        //{
                        //    switch (BudgetingController.lstCostElement.Find(cost => cost.ID.Equals(Int32.Parse(item.CostElement))).CostElement)
                        //    {
                        //        case "MAE":
                        //            {

                        //                OSS_MAE_Totals += Math.Round((decimal)item.ApprCost, MidpointRounding.AwayFromZero);

                        //                break;
                        //            }
                        //        case "Non-MAE":
                        //            {

                        //                OSS_NMAE_Totals += Math.Round((decimal)item.ApprCost, MidpointRounding.AwayFromZero);

                        //                break;
                        //            }
                        //        case "Software":
                        //            {

                        //                OSS_SoftwareTotals += Math.Round((decimal)item.ApprCost, MidpointRounding.AwayFromZero);

                        //                break;
                        //            }
                        //        default:
                        //            continue;
                        //    }
                        //}
                        //foreach (RequestItems_Table item in reqList.FindAll(x => x.BU.Trim().Equals(BudgetingController.lstBUs.Find(bu => bu.BU.Equals("2WP")).ID.ToString())).FindAll(x => x.ApprovedDH == true))
                        //{
                        //    switch (BudgetingController.lstCostElement.Find(cost => cost.ID.Equals(Int32.Parse(item.CostElement))).CostElement)
                        //    {
                        //        case "MAE":
                        //            {

                        //                TwoWP_MAE_Totals += Math.Round((decimal)item.ApprCost, MidpointRounding.AwayFromZero);

                        //                break;
                        //            }
                        //        case "Non-MAE":
                        //            {

                        //                TwoWP_NMAE_Totals += Math.Round((decimal)item.ApprCost, MidpointRounding.AwayFromZero);

                        //                break;
                        //            }
                        //        case "Software":
                        //            {

                        //                TwoWP_SoftwareTotals += Math.Round((decimal)item.ApprCost, MidpointRounding.AwayFromZero);

                        //                break;
                        //            }
                        //        default:
                        //            continue;
                        //    }
                        //}


                        DataTable dtTotals = new DataTable();
                        connection();
                        string tQuery = "Select A.BU as BUID,B.BU,A.CostElement as CEID,C.CostElement,sum(round(ApprCost,0)) as ApprCost from RequestItems_Table A inner join BU_Table B on A .BU = B.ID inner join CostElement_Table C on A.CostElement = C.ID where VKM_Year = " + yr + " and A.BU in (1, 3, 5) and(Fund is null or Fund = 2) and ApprovedDH = 1 and DEPT in (Select id from DEPT_Table where DEPT not like '%-XC%') group by A.BU,B.BU,A.CostElement,C.CostElement Order by A.BU";
                        OpenConnection();
                        SqlCommand cmd1 = new SqlCommand(tQuery, conn);
                        SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
                        da1.Fill(dtTotals);
                        CloseConnection();

                        foreach (DataRow item in dtTotals.Rows)
                        {
                            switch (item["BU"].ToString().ToLower())
                            {
                                case "mb":
                                    {
                                        if (item["CostElement"].ToString().ToLower() == "mae")
                                        {
                                            AS_MAE_Totals = (decimal)item["ApprCost"];
                                        }
                                        else if (item["CostElement"].ToString().ToLower() == "non-mae")
                                        {
                                            AS_NMAE_Totals = (decimal)item["ApprCost"];
                                        }
                                        else if (item["CostElement"].ToString().ToLower() == "software")
                                        {
                                            AS_SoftwareTotals = (decimal)item["ApprCost"];
                                        }
                                        break;
                                    }
                                case "oss":
                                    {
                                        if (item["CostElement"].ToString().ToLower() == "mae")
                                        {
                                            OSS_MAE_Totals = (decimal)item["ApprCost"];
                                        }
                                        else if (item["CostElement"].ToString().ToLower() == "non-mae")
                                        {
                                            OSS_NMAE_Totals = (decimal)item["ApprCost"];
                                        }
                                        else if (item["CostElement"].ToString().ToLower() == "software")
                                        {
                                            OSS_SoftwareTotals = (decimal)item["ApprCost"];
                                        }
                                        break;
                                    }
                                case "2wp":
                                    {
                                        if (item["CostElement"].ToString().ToLower() == "mae")
                                        {
                                            TwoWP_MAE_Totals = (decimal)item["ApprCost"];
                                        }
                                        else if (item["CostElement"].ToString().ToLower() == "non-mae")
                                        {
                                            TwoWP_NMAE_Totals = (decimal)item["ApprCost"];
                                        }
                                        else if (item["CostElement"].ToString().ToLower() == "software")
                                        {
                                            TwoWP_SoftwareTotals = (decimal)item["ApprCost"];
                                        }
                                        break;
                                    }
                                default:
                                    continue;
                            }
                        }

                        BUSummary_CC tempobj = new BUSummary_CC();
                        tempobj.vkmyear = yr;
                        tempobj.AS_MAE_Totals = AS_MAE_Totals;
                        tempobj.AS_NMAE_Totals = AS_NMAE_Totals;
                        tempobj.AS_Software_Totals = AS_SoftwareTotals;
                        tempobj.AS_Overall_Totals = AS_MAE_Totals + AS_NMAE_Totals + AS_SoftwareTotals;
                        tempobj.OSS_MAE_Totals = OSS_MAE_Totals;
                        tempobj.OSS_NMAE_Totals = OSS_NMAE_Totals;
                        tempobj.OSS_Software_Totals = OSS_SoftwareTotals;
                        tempobj.OSS_Overall_Totals = OSS_MAE_Totals + OSS_NMAE_Totals + OSS_SoftwareTotals;
                        tempobj.TwoWP_MAE_Totals = TwoWP_MAE_Totals;
                        tempobj.TwoWP_NMAE_Totals = TwoWP_NMAE_Totals;
                        tempobj.TwoWP_Software_Totals = TwoWP_SoftwareTotals;
                        tempobj.TwoWP_Overall_Totals = TwoWP_MAE_Totals + TwoWP_NMAE_Totals + TwoWP_SoftwareTotals;

                        viewList.Add(tempobj);

                    }

                    if (chart)
                    {

                        dt = new System.Data.DataTable();
                        dt.Columns.Add("Year", typeof(string));
                        dt.Columns.Add("MB", typeof(decimal));
                        dt.Columns.Add("OSS", typeof(decimal));
                        dt.Columns.Add("2WP", typeof(decimal));
                        dt.Columns.Add("Totals", typeof(decimal));
                        //dt.Columns.Add("Percentage_Utilization", typeof(decimal));

                        //DataRow dr = dt.NewRow();

                        int rcnt = 0;

                        foreach (var info in viewList)
                        {
                            DataRow dr = dt.NewRow();
                            //dr = dt.NewRow();
                            dr[rcnt++] = "MAE" + " " + info.vkmyear;
                            dr[rcnt++] = info.AS_MAE_Totals;
                            dr[rcnt++] = info.OSS_MAE_Totals;
                            dr[rcnt++] = info.TwoWP_MAE_Totals;
                            dr[rcnt++] = info.AS_MAE_Totals + info.OSS_MAE_Totals + info.TwoWP_MAE_Totals;
                            //dr[rcnt++] = info.P_MAE_Totals != 0 ? (info.U_MAE_Totals / info.P_MAE_Totals) * 100 : 0;
                            rcnt = 0;
                            dt.Rows.Add(dr);
                            dr = dt.NewRow();
                            dr[rcnt++] = "Non-MAE" + " " + info.vkmyear;
                            dr[rcnt++] = info.AS_NMAE_Totals;
                            dr[rcnt++] = info.OSS_NMAE_Totals;
                            dr[rcnt++] = info.TwoWP_NMAE_Totals;
                            dr[rcnt++] = info.AS_NMAE_Totals + info.OSS_NMAE_Totals + info.TwoWP_NMAE_Totals;
                            rcnt = 0;
                            dt.Rows.Add(dr);
                            dr = dt.NewRow();
                            dr[rcnt++] = "Software" + " " + info.vkmyear;
                            dr[rcnt++] = info.AS_Software_Totals;
                            dr[rcnt++] = info.OSS_Software_Totals;
                            dr[rcnt++] = info.TwoWP_Software_Totals;
                            dr[rcnt++] = info.AS_Software_Totals + info.OSS_Software_Totals + info.TwoWP_Software_Totals;

                            rcnt = 0;
                            dt.Rows.Add(dr);
                            dr = dt.NewRow();
                            dr[rcnt++] = "Totals" + " " + info.vkmyear;
                            dr[rcnt++] = info.AS_Overall_Totals;
                            dr[rcnt++] = info.OSS_Overall_Totals;
                            dr[rcnt++] = info.TwoWP_Overall_Totals;
                            dr[rcnt++] = info.AS_Overall_Totals + info.OSS_Overall_Totals + info.TwoWP_Overall_Totals;

                            //rcnt++; 
                            dt.Rows.Add(dr);
                            // dr = dt.NewRow();
                            rcnt = 0;
                        }
                        return dt;
                        //dt.Rows.Add(dr);
                        //for (int i = 0; i <= dt.Columns.Count - 1; i++)
                        //{
                        //    _col.Add(new columnsinfo { title = dt.Columns[i].ColumnName, data = dt.Columns[i].ColumnName });
                        //}

                        //string col = (string)serializer.Serialize(_col);
                        //t.columns = col;


                        //var lst = dt.AsEnumerable()
                        //.Select(r => r.Table.Columns.Cast<DataColumn>()
                        //        .Select(c => new KeyValuePair<string, object>(c.ColumnName, r[c.Ordinal])
                        //       ).ToDictionary(z => z.Key, z => z.Value)
                        //).ToList();

                        //string data = serializer.Serialize(lst);
                        //t.data = data;
                    }
                    else
                    {

                        dt.Columns.Add("Cost Element", typeof(string)); /// Planned + (year variable) / Utilized  + (year variable)
                        //dt.Columns.Add("VKM 2020", typeof(string));
                        foreach (string yr in years)
                        {
                            dt.Columns.Add("VKM" + " " + yr + " MB", typeof(string)); //add vkm text to yr
                            dt.Columns.Add("VKM" + " " + yr + " OSS", typeof(string));
                            dt.Columns.Add("VKM" + " " + yr + " 2WP", typeof(string));
                            dt.Columns.Add("VKM" + " " + yr + " Totals", typeof(string));

                        }


                        DataRow dr = dt.NewRow();
                        dr[0] = "MAE";
                        int rcnt = 1;


                        foreach (var info in viewList)
                        {
                            dr[rcnt++] = info.AS_MAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
                            dr[rcnt++] = info.OSS_MAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
                            dr[rcnt++] = info.TwoWP_MAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
                            dr[rcnt++] = (info.AS_MAE_Totals + info.OSS_MAE_Totals + info.TwoWP_MAE_Totals).ToString("C0", CultureInfo.CurrentCulture);
                        }
                        dt.Rows.Add(dr);

                        dr = dt.NewRow();
                        dr[0] = "Non-MAE";
                        int r1cnt = 1;
                        foreach (var info in viewList)
                        {
                            dr[r1cnt++] = info.AS_NMAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
                            dr[r1cnt++] = info.OSS_NMAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
                            dr[r1cnt++] = info.TwoWP_NMAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
                            dr[r1cnt++] = (info.AS_NMAE_Totals + info.OSS_NMAE_Totals + info.TwoWP_NMAE_Totals).ToString("C0", CultureInfo.CurrentCulture);
                        }

                        dt.Rows.Add(dr);

                        dr = dt.NewRow();
                        dr[0] = "Software";
                        int r2cnt = 1;
                        foreach (var info in viewList)
                        {
                            dr[r2cnt++] = info.AS_Software_Totals.ToString("C0", CultureInfo.CurrentCulture);
                            dr[r2cnt++] = info.OSS_Software_Totals.ToString("C0", CultureInfo.CurrentCulture);
                            dr[r2cnt++] = info.TwoWP_Software_Totals.ToString("C0", CultureInfo.CurrentCulture);
                            dr[r2cnt++] = (info.AS_Software_Totals + info.OSS_Software_Totals + info.TwoWP_Software_Totals).ToString("C0", CultureInfo.CurrentCulture);
                        }

                        dt.Rows.Add(dr);

                        dr = dt.NewRow();
                        dr[0] = "Totals";
                        int r3cnt = 1;

                        foreach (var info in viewList)
                        {
                            //dr[r3cnt++] = (info.AS_MAE_Totals + info.AS_NMAE_Totals + info.AS_Software_Totals + info.AS_Overall_Totals).ToString("C0", CultureInfo.CurrentCulture);
                            //dr[r3cnt++] = (info.OSS_MAE_Totals + info.OSS_NMAE_Totals + info.OSS_Software_Totals + info.OSS_Overall_Totals).ToString("C0", CultureInfo.CurrentCulture);
                            //dr[r3cnt++] = (info.TwoWP_MAE_Totals + info.TwoWP_NMAE_Totals + info.TwoWP_Software_Totals + info.TwoWP_Overall_Totals).ToString("C0", CultureInfo.CurrentCulture);
                            //dr[r3cnt++] = (info.AS_MAE_Totals + info.OSS_MAE_Totals + info.TwoWP_MAE_Totals +
                            //               info.AS_NMAE_Totals + info.OSS_NMAE_Totals + info.TwoWP_NMAE_Totals +
                            //               info.AS_Software_Totals + info.OSS_Software_Totals + info.TwoWP_Software_Totals
                            //                ).ToString("C0", CultureInfo.CurrentCulture);
                            dr[r3cnt++] = (info.AS_Overall_Totals).ToString("C0", CultureInfo.CurrentCulture);
                            dr[r3cnt++] = (info.OSS_Overall_Totals).ToString("C0", CultureInfo.CurrentCulture);
                            dr[r3cnt++] = (info.TwoWP_Overall_Totals).ToString("C0", CultureInfo.CurrentCulture);
                            dr[r3cnt++] = (info.AS_Overall_Totals + info.OSS_Overall_Totals + info.TwoWP_Overall_Totals).ToString("C0", CultureInfo.CurrentCulture);

                        }

                        dt.Rows.Add(dr);
                        return dt;
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog("Error - CC_BUSummaryComparison : NTID - " + User.Identity.Name.Split('\\')[1].Trim().ToUpper() + ", ErrorMsg : " + ex + ", At - " + DateTime.Now);
                return dt;
            }
        }

        public System.Data.DataTable XC_BUSummaryComparison(List<string> years, bool chart = false)//years = vkmyears 2021 and 2022(current)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            try
            {
                //string presentUserDept = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).Department;

                using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

                //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

                {

                    List<BUSummary_XC> viewList = new List<BUSummary_XC>();



                    foreach (string yr in years)
                    {
                        decimal DA_MAE_Totals = 0, DA_NMAE_Totals = 0, DA_SoftwareTotals = 0;
                        decimal AD_MAE_Totals = 0, AD_NMAE_Totals = 0, AD_SoftwareTotals = 0;
                        decimal TwoWP_MAE_Totals = 0, TwoWP_NMAE_Totals = 0, TwoWP_SoftwareTotals = 0;

                        List<RequestItems_Table> reqList = db.RequestItems_Table.ToList<RequestItems_Table>().FindAll(x => x.RequestDate.ToString().Contains((int.Parse(yr) - 1).ToString())).FindAll(v => v.Fund == null || v.Fund.Trim().Contains("2"));
                        //if (int.Parse(yr) - 1 > 2020)
                        //{
                        //    //reqList = reqList.FindAll(dpt => BudgetingController.lstDEPTs.Find(x => x.ID.ToString().Contains(dpt.DEPT)).DEPT.Contains("XC"));

                        //    DataTable dt1 = new DataTable();
                        //    connection();
                        //    string Query = "Select * from RequestItems_Table where VKM_Year = " + yr + " and BU in (1, 2, 4) and (Fund is null or Fund = 2) and ApprovedDH = 1 and DEPT in (Select id from DEPT_Table where DEPT like '%-XC%')";
                        //    OpenConnection();
                        //    SqlCommand cmd = new SqlCommand(Query, conn);
                        //    SqlDataAdapter da = new SqlDataAdapter(cmd);
                        //    da.Fill(dt1);
                        //    CloseConnection();
                        //    reqList = new List<RequestItems_Table>();
                        //    foreach (DataRow item in dt1.Rows)
                        //    {
                        //        try
                        //        {

                        //            RequestItems_Table ritem = new RequestItems_Table();
                        //            ritem.ApprQuantity = Convert.ToInt32(item["ApprQuantity"]);
                        //            ritem.ApprCost = Convert.ToDecimal(item["ApprCost"]);

                        //            ritem.VKM_Year = Convert.ToInt32(item["VKM_Year"]);
                        //            if (item["UpdatedAt"].ToString() != "")
                        //                ritem.UpdatedAt = Convert.ToDateTime(item["UpdatedAt"]);
                        //            if (item["RequestorNTID"].ToString() != "")
                        //                ritem.RequestorNTID = item["RequestorNTID"].ToString();
                        //            ritem.Category = item["Category"].ToString();
                        //            ritem.Comments = item["Comments"].ToString();
                        //            ritem.Project = item["Project"].ToString();
                        //            if (item["ActualAvailableQuantity"].ToString() == "")
                        //                ritem.ActualAvailableQuantity = "NA";
                        //            else
                        //                ritem.ActualAvailableQuantity = item["ActualAvailableQuantity"].ToString();

                        //            ritem.CostElement = item["CostElement"].ToString();
                        //            ritem.BudgetCode = item["BudgetCode"].ToString();
                        //            ritem.BU = item["BU"].ToString();

                        //            ritem.DEPT = item["DEPT"].ToString();
                        //            ritem.Group = item["Group"].ToString();
                        //            ritem.ItemName = item["ItemName"].ToString();
                        //            ritem.OEM = item["OEM"].ToString();
                        //            ritem.ReqQuantity = Convert.ToInt32(item["ReqQuantity"]);
                        //            ritem.RequestID = Convert.ToInt32(item["RequestID"]);

                        //            ritem.RequestorNT = item["RequestorNT"].ToString();
                        //            ritem.TotalPrice = Convert.ToDecimal(item["TotalPrice"]);
                        //            ritem.OrderPrice = item["OrderPrice"].ToString() != "" ? Convert.ToDecimal(item["OrderPrice"]) : 0;
                        //            ritem.UnitPrice = Convert.ToDecimal(item["UnitPrice"]);
                        //            ritem.ApprovalDH = Convert.ToBoolean(item["ApprovalDH"]);
                        //            ritem.ApprovalSH = Convert.ToBoolean(item["ApprovalSH"]);
                        //            ritem.ApprovedDH = Convert.ToBoolean(item["ApprovedDH"]);
                        //            ritem.ApprovedSH = Convert.ToBoolean(item["ApprovedSH"]);
                        //            if (item["IsCancelled"].ToString() != "")
                        //                ritem.isCancelled = Convert.ToInt32(item["IsCancelled"]);

                        //            ritem.DHNT = item["DHNT"].ToString();
                        //            ritem.SHNT = item["SHNT"].ToString();

                        //            if (item["RequestDate"].ToString() != "")
                        //                ritem.RequestDate = (DateTime)item["RequestDate"];

                        //            if (item["SubmitDate"].ToString() != "")
                        //                ritem.SubmitDate = (DateTime)item["SubmitDate"];

                        //            if (item["DHAppDate"].ToString() != "")
                        //                ritem.DHAppDate = (DateTime)item["DHAppDate"];

                        //            if (item["SHAppDate"].ToString() != "")
                        //                ritem.SHAppDate = (DateTime)item["SHAppDate"];


                        //            if (item["OrderStatus"].ToString().Trim() != "")
                        //            {
                        //                ritem.OrderStatus = (item["OrderStatus"]).ToString();

                        //            }
                        //            else
                        //            {
                        //                ritem.OrderStatus = null;


                        //            }
                        //            if (item["Project"].ToString() == "")
                        //                ritem.Project = string.Empty;
                        //            else
                        //                ritem.Project = item["Project"].ToString();


                        //            reqList.Add(ritem);

                        //        }
                        //        catch (Exception ex)
                        //        {

                        //        }

                        //    }
                        //}

                        //foreach (RequestItems_Table item in reqList.FindAll(x => x.BU.Equals(BudgetingController.lstBUs.Find(bu => bu.BU.Trim().Equals("DA")).ID.ToString())).FindAll(x => x.ApprovedDH == true))
                        //{
                        //    switch (BudgetingController.lstCostElement.Find(cost => cost.ID.Equals(Int32.Parse(item.CostElement))).CostElement)
                        //    {
                        //        case "MAE":
                        //            {

                        //                DA_MAE_Totals += Math.Round((decimal)item.ApprCost, MidpointRounding.AwayFromZero);

                        //                break;
                        //            }
                        //        case "Non-MAE":
                        //            {

                        //                DA_NMAE_Totals += Math.Round((decimal)item.ApprCost, MidpointRounding.AwayFromZero);

                        //                break;
                        //            }
                        //        case "Software":
                        //            {

                        //                DA_SoftwareTotals += Math.Round((decimal)item.ApprCost, MidpointRounding.AwayFromZero);

                        //                break;
                        //            }
                        //        default:
                        //            continue;
                        //    }
                        //}
                        //foreach (RequestItems_Table item in reqList.FindAll(x => x.BU.Trim().Equals(BudgetingController.lstBUs.Find(bu => bu.BU.Equals("AD")).ID.ToString())).FindAll(x => x.ApprovedDH == true))
                        //{
                        //    switch (BudgetingController.lstCostElement.Find(cost => cost.ID.Equals(Int32.Parse(item.CostElement))).CostElement)
                        //    {
                        //        case "MAE":
                        //            {

                        //                AD_MAE_Totals += Math.Round((decimal)item.ApprCost, MidpointRounding.AwayFromZero);

                        //                break;
                        //            }
                        //        case "Non-MAE":
                        //            {

                        //                AD_NMAE_Totals += Math.Round((decimal)item.ApprCost, MidpointRounding.AwayFromZero);

                        //                break;
                        //            }
                        //        case "Software":
                        //            {

                        //                AD_SoftwareTotals += Math.Round((decimal)item.ApprCost, MidpointRounding.AwayFromZero);

                        //                break;
                        //            }
                        //        default:
                        //            continue;
                        //    }
                        //}
                        //foreach (RequestItems_Table item in reqList.FindAll(x => x.BU.Trim().Equals(BudgetingController.lstBUs.Find(bu => bu.BU.Equals("2WP")).ID.ToString())).FindAll(x => x.ApprovedDH == true))
                        //{
                        //    switch (BudgetingController.lstCostElement.Find(cost => cost.ID.Equals(Int32.Parse(item.CostElement))).CostElement)
                        //    {
                        //        case "MAE":
                        //            {

                        //                TwoWP_MAE_Totals += Math.Round((decimal)item.ApprCost, MidpointRounding.AwayFromZero);

                        //                break;
                        //            }
                        //        case "Non-MAE":
                        //            {

                        //                TwoWP_NMAE_Totals += Math.Round((decimal)item.ApprCost, MidpointRounding.AwayFromZero);

                        //                break;
                        //            }
                        //        case "Software":
                        //            {

                        //                TwoWP_SoftwareTotals += Math.Round((decimal)item.ApprCost, MidpointRounding.AwayFromZero);

                        //                break;
                        //            }
                        //        default:
                        //            continue;
                        //    }
                        //}


                        DataTable dtTotals = new DataTable();
                        connection();
                        string tQuery = "Select A.BU as BUID,B.BU,A.CostElement as CEID,C.CostElement,sum(round(ApprCost,0)) as ApprCost from RequestItems_Table A inner join BU_Table B on A .BU = B.ID inner join CostElement_Table C on A.CostElement = C.ID where VKM_Year = " + yr + " and A.BU in (1, 2, 4) and(Fund is null or Fund = 2) and ApprovedDH = 1 and DEPT in (Select id from DEPT_Table where DEPT like '%-XC%') group by A.BU,B.BU,A.CostElement,C.CostElement Order by A.BU";
                        OpenConnection();
                        SqlCommand cmd1 = new SqlCommand(tQuery, conn);
                        SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
                        da1.Fill(dtTotals);
                        CloseConnection();

                        foreach (DataRow item in dtTotals.Rows)
                        {
                            switch (item["BU"].ToString().ToLower())
                            {
                                case "ad":
                                    {
                                        if (item["CostElement"].ToString().ToLower() == "mae")
                                        {
                                            AD_MAE_Totals = (decimal)item["ApprCost"];
                                        }
                                        else if (item["CostElement"].ToString().ToLower() == "non-mae")
                                        {
                                            AD_NMAE_Totals = (decimal)item["ApprCost"];
                                        }
                                        else if (item["CostElement"].ToString().ToLower() == "software")
                                        {
                                            AD_SoftwareTotals = (decimal)item["ApprCost"];
                                        }
                                        break;
                                    }
                                case "da":
                                    {
                                        if (item["CostElement"].ToString().ToLower() == "mae")
                                        {
                                            DA_MAE_Totals = (decimal)item["ApprCost"];
                                        }
                                        else if (item["CostElement"].ToString().ToLower() == "non-mae")
                                        {
                                            DA_NMAE_Totals = (decimal)item["ApprCost"];
                                        }
                                        else if (item["CostElement"].ToString().ToLower() == "software")
                                        {
                                            DA_SoftwareTotals = (decimal)item["ApprCost"];
                                        }
                                        break;
                                    }
                                case "2wp":
                                    {
                                        if (item["CostElement"].ToString().ToLower() == "mae")
                                        {
                                            TwoWP_MAE_Totals = (decimal)item["ApprCost"];
                                        }
                                        else if (item["CostElement"].ToString().ToLower() == "non-mae")
                                        {
                                            TwoWP_NMAE_Totals = (decimal)item["ApprCost"];
                                        }
                                        else if (item["CostElement"].ToString().ToLower() == "software")
                                        {
                                            TwoWP_SoftwareTotals = (decimal)item["ApprCost"];
                                        }
                                        break;
                                    }
                                default:
                                    continue;
                            }
                        }

                        BUSummary_XC tempobj = new BUSummary_XC();
                        tempobj.vkmyear = yr;
                        tempobj.DA_MAE_Totals = DA_MAE_Totals;
                        tempobj.DA_NMAE_Totals = DA_NMAE_Totals;
                        tempobj.DA_Software_Totals = DA_SoftwareTotals;
                        tempobj.DA_Overall_Totals = DA_MAE_Totals + DA_NMAE_Totals + DA_SoftwareTotals;
                        tempobj.AD_MAE_Totals = AD_MAE_Totals;
                        tempobj.AD_NMAE_Totals = AD_NMAE_Totals;
                        tempobj.AD_Software_Totals = AD_SoftwareTotals;
                        tempobj.AD_Overall_Totals = AD_MAE_Totals + AD_NMAE_Totals + AD_SoftwareTotals;
                        tempobj.TwoWP_MAE_Totals = TwoWP_MAE_Totals;
                        tempobj.TwoWP_NMAE_Totals = TwoWP_NMAE_Totals;
                        tempobj.TwoWP_Software_Totals = TwoWP_SoftwareTotals;
                        tempobj.TwoWP_Overall_Totals = TwoWP_MAE_Totals + TwoWP_NMAE_Totals + TwoWP_SoftwareTotals;

                        viewList.Add(tempobj);

                    }

                    if (chart)
                    {

                        dt = new System.Data.DataTable();
                        dt.Columns.Add("Year", typeof(string));
                        dt.Columns.Add("DA", typeof(decimal));
                        dt.Columns.Add("AD", typeof(decimal));
                        dt.Columns.Add("2WP", typeof(decimal));
                        dt.Columns.Add("Totals", typeof(decimal));
                        //dt.Columns.Add("Percentage_Utilization", typeof(decimal));

                        //DataRow dr = dt.NewRow();

                        int rcnt = 0;

                        foreach (var info in viewList)
                        {
                            DataRow dr = dt.NewRow();
                            //dr = dt.NewRow();
                            dr[rcnt++] = "MAE" + " " + info.vkmyear;
                            dr[rcnt++] = info.DA_MAE_Totals;
                            dr[rcnt++] = info.AD_MAE_Totals;
                            dr[rcnt++] = info.TwoWP_MAE_Totals;
                            dr[rcnt++] = info.DA_MAE_Totals + info.AD_MAE_Totals + info.TwoWP_MAE_Totals;
                            rcnt = 0;
                            dt.Rows.Add(dr);
                            dr = dt.NewRow();
                            dr[rcnt++] = "Non-MAE" + " " + info.vkmyear;
                            dr[rcnt++] = info.DA_NMAE_Totals;
                            dr[rcnt++] = info.AD_NMAE_Totals;
                            dr[rcnt++] = info.TwoWP_NMAE_Totals;
                            dr[rcnt++] = info.DA_NMAE_Totals + info.AD_NMAE_Totals + info.TwoWP_NMAE_Totals;
                            rcnt = 0;
                            dt.Rows.Add(dr);
                            dr = dt.NewRow();
                            dr[rcnt++] = "Software" + " " + info.vkmyear;
                            dr[rcnt++] = info.DA_Software_Totals;
                            dr[rcnt++] = info.AD_Software_Totals;
                            dr[rcnt++] = info.TwoWP_Software_Totals;
                            dr[rcnt++] = info.DA_Software_Totals + info.AD_Software_Totals + info.TwoWP_Software_Totals;

                            rcnt = 0;
                            dt.Rows.Add(dr);
                            dr = dt.NewRow();
                            dr[rcnt++] = "Totals" + " " + info.vkmyear;
                            dr[rcnt++] = info.DA_Overall_Totals;
                            dr[rcnt++] = info.AD_Overall_Totals;
                            dr[rcnt++] = info.TwoWP_Overall_Totals;
                            dr[rcnt++] = info.DA_Overall_Totals + info.AD_Overall_Totals + info.TwoWP_Overall_Totals;

                            //rcnt++; 
                            dt.Rows.Add(dr);
                            // dr = dt.NewRow();
                            rcnt = 0;
                        }
                        return dt;

                    }
                    else
                    {
                        dt = new System.Data.DataTable();
                        dt.Columns.Add("Cost Element", typeof(string)); /// Planned + (year variable) / Utilized  + (year variable)
                        //dt.Columns.Add("VKM 2020", typeof(string));
                        foreach (string yr in years)
                        {
                            dt.Columns.Add("VKM" + " " + yr + " DA", typeof(string)); //add vkm text to yr
                            dt.Columns.Add("VKM" + " " + yr + " AD", typeof(string));
                            dt.Columns.Add("VKM" + " " + yr + " 2WP", typeof(string));
                            dt.Columns.Add("VKM" + " " + yr + " Totals", typeof(string));

                        }


                        DataRow dr = dt.NewRow();
                        dr[0] = "MAE";
                        int rcnt = 1;

                        foreach (var info in viewList)
                        {
                            dr[rcnt++] = info.DA_MAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
                            dr[rcnt++] = info.AD_MAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
                            dr[rcnt++] = info.TwoWP_MAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
                            dr[rcnt++] = (info.DA_MAE_Totals + info.AD_MAE_Totals + info.TwoWP_MAE_Totals).ToString("C0", CultureInfo.CurrentCulture);
                        }
                        dt.Rows.Add(dr);

                        dr = dt.NewRow();
                        dr[0] = "Non-MAE";
                        int r1cnt = 1;
                        foreach (var info in viewList)
                        {
                            dr[r1cnt++] = info.DA_NMAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
                            dr[r1cnt++] = info.AD_NMAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
                            dr[r1cnt++] = info.TwoWP_NMAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
                            dr[r1cnt++] = (info.DA_NMAE_Totals + info.AD_NMAE_Totals + info.TwoWP_NMAE_Totals).ToString("C0", CultureInfo.CurrentCulture);
                        }

                        dt.Rows.Add(dr);

                        dr = dt.NewRow();
                        dr[0] = "Software";
                        int r2cnt = 1;
                        foreach (var info in viewList)
                        {
                            dr[r2cnt++] = info.DA_Software_Totals.ToString("C0", CultureInfo.CurrentCulture);
                            dr[r2cnt++] = info.AD_Software_Totals.ToString("C0", CultureInfo.CurrentCulture);
                            dr[r2cnt++] = info.TwoWP_Software_Totals.ToString("C0", CultureInfo.CurrentCulture);
                            dr[r2cnt++] = (info.DA_Software_Totals + info.AD_Software_Totals + info.TwoWP_Software_Totals).ToString("C0", CultureInfo.CurrentCulture);
                        }

                        dt.Rows.Add(dr);

                        dr = dt.NewRow();
                        dr[0] = "Totals";
                        int r3cnt = 1;

                        foreach (var info in viewList)
                        {

                            dr[r3cnt++] = (info.DA_Overall_Totals).ToString("C0", CultureInfo.CurrentCulture);
                            dr[r3cnt++] = (info.AD_Overall_Totals).ToString("C0", CultureInfo.CurrentCulture);
                            dr[r3cnt++] = (info.TwoWP_Overall_Totals).ToString("C0", CultureInfo.CurrentCulture);
                            dr[r3cnt++] = (info.DA_Overall_Totals + info.AD_Overall_Totals + info.TwoWP_Overall_Totals).ToString("C0", CultureInfo.CurrentCulture);

                        }


                        dt.Rows.Add(dr);
                        return dt;
                    }
                }
            }
            catch (Exception ex)
            {
                WriteLog("Error - XC_BUSummaryComparison : NTID - " + User.Identity.Name.Split('\\')[1].Trim().ToUpper() + ", ErrorMsg : " + ex + ", At - " + DateTime.Now);
                return dt;
            }
        }


        /// <summary>
        /// Function to get the dept cost summary data
        /// </summary>
        /// <param name="forexcel">can be used to add or remove columns for different sources</param>
        /// <returns></returns>
        public System.Data.DataTable DeptSummaryData(bool forexcel, string year)
        {
            DataTable dt = new DataTable();
            try
            {

                using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

                //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

                {
                    System.Security.Principal.IPrincipal User = System.Web.HttpContext.Current.User;
                    dt = new System.Data.DataTable();

                    List<RequestItems_Table> reqList = db.RequestItems_Table.ToList<RequestItems_Table>().FindAll(x => x.ApprovedDH == true).FindAll(x => x.RequestDate.ToString().Contains(year)).FindAll(v => v.Fund == null || v.Fund.Trim().Contains("2"));
                    List<DeptSummary> viewList = new List<DeptSummary>();
                    List<string> userSectionDeptList = new List<string>();
                    List<string> approvedSections = new List<string>();
                    string presentUserDept = string.Empty;
                    decimal OMAE_Totals = 0, ONMAE_Totals = 0, OSoftwareTotals = 0;
                    if (year.Contains("2020"))
                    {
                        //presentUserDept = BudgetingController.lstUsers_2020.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).Department;
                        approvedSections.Add("NE2-VS");
                        approvedSections.Add("NE1-VS");
                        approvedSections.Add("ESD");
                        //userSectionDeptList = BudgetingController.lstUsers_2020.FindAll(x => approvedSections.Contains(x.Section)).Select(item => item.Department).Distinct().ToList();
                        //userSectionDeptList.Sort();
                        //userSectionDeptList.Reverse();

                        connection();
                        string qry = "select Department from SPOTONData where NTID = '" + User.Identity.Name.Split('\\')[1].ToUpper() + "' ";

                        SqlDataReader dr1;
                        OpenConnection();
                        SqlCommand cmd1 = new SqlCommand(qry, conn);
                        dr1 = cmd1.ExecuteReader();
                        if (dr1.HasRows)
                        {
                            dr1.Read();
                            presentUserDept = dr1["Department"].ToString();
                        }
                        dr1.Close();
                        CloseConnection();

                        DataSet ds = new DataSet();
                        qry = " select Distinct Department from SPOTONData Where Section in ('NE2-VS', 'NE1-VS', 'ESD') ; select DEPT_Table.DEPT, CostElement_Table.CostElement, round(isnull(sum(ApprCost), 0), 0) as ApprCost from RequestItems_Table inner join DEPT_Table on RequestItems_Table.DEPT = DEPT_Table.ID inner join CostElement_Table on RequestItems_Table.CostElement = CostElement_Table.ID where VKM_Year = '" + year + "' and DEPT_Table.DEPT in (select Department from SPOTONData where Section in ('NE2-VS', 'NE1-VS', 'ESD')) group by DEPT_Table.DEPT, CostElement_Table.CostElement order by DEPT_Table.DEPT, CostElement_Table.CostElement";
                        OpenConnection();
                        cmd1 = new SqlCommand(qry, conn);
                        SqlDataAdapter da = new SqlDataAdapter(cmd1);
                        da.Fill(ds);
                        CloseConnection();

                        foreach (DataRow item in ds.Tables[0].Rows)
                        {
                            userSectionDeptList.Add(item["Department"].ToString());
                        }

                        userSectionDeptList.Sort();
                        userSectionDeptList.Reverse();


                        foreach (DataRow item in ds.Tables[1].Rows)
                        {
                            decimal MAE_Totals = 0, NMAE_Totals = 0, SoftwareTotals = 0;
                            DeptSummary tempobj = new DeptSummary();
                            tempobj.deptName = item["DEPT"].ToString();
                            if (item["CostElement"].ToString().ToLower() == "mae")
                            {
                                MAE_Totals = (decimal)item["ApprCost"];
                            }
                            else if (item["CostElement"].ToString().ToLower() == "non-mae")
                            {
                                NMAE_Totals = (decimal)item["ApprCost"];
                            }
                            else if (item["CostElement"].ToString().ToLower() == "software")
                            {
                                SoftwareTotals = (decimal)item["ApprCost"];
                            }

                            tempobj.MAE_Totals = MAE_Totals;
                            OMAE_Totals += MAE_Totals;
                            tempobj.NMAE_Totals = NMAE_Totals;
                            ONMAE_Totals += NMAE_Totals;
                            tempobj.Software_Totals = SoftwareTotals;
                            OSoftwareTotals += SoftwareTotals;
                            tempobj.Overall_Totals = MAE_Totals + NMAE_Totals + SoftwareTotals;
                            viewList.Add(tempobj);
                        }

                        //CODE TO GET GROUPS OF COST ELEMENT
                        //IEnumerable<IGrouping<string, RequestItems_Table>> query = reqList.GroupBy(item => item.CostElement);

                        //foreach (string dept in userSectionDeptList)
                        //{/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        //    decimal MAE_Totals = 0, NMAE_Totals = 0, SoftwareTotals = 0;
                        //    DeptSummary tempobj = new DeptSummary();
                        //    tempobj.deptName = dept;
                        //    //CODE TO GET THE TOTALS OF EACH COST ELEMENT
                        //    foreach (IGrouping<string, RequestItems_Table> CostGroup in query)
                        //    {

                        //        // Iterate over each value in the
                        //        // IGrouping and print the value.
                        //        if (BudgetingController.lstCostElement.Find(cost => cost.ID.Equals(Int32.Parse(CostGroup.Key))).CostElement == "MAE")
                        //        {
                        //            foreach (RequestItems_Table item in CostGroup)
                        //            {
                        //                if (BudgetingController.lstDEPTs.Find(departmnt => departmnt.ID.Equals(Int32.Parse(item.DEPT))).DEPT == dept)
                        //                {

                        //                    MAE_Totals += Math.Round((decimal)item.ApprCost, MidpointRounding.AwayFromZero);

                        //                }
                        //            }
                        //        }
                        //        else if (BudgetingController.lstCostElement.Find(cost => cost.ID.Equals(Int32.Parse(CostGroup.Key))).CostElement == "Non-MAE")

                        //        {
                        //            foreach (RequestItems_Table item in CostGroup)
                        //            {
                        //                if (BudgetingController.lstDEPTs.Find(departmnt => departmnt.ID.Equals(Int32.Parse(item.DEPT))).DEPT == dept)
                        //                {

                        //                    NMAE_Totals += Math.Round((decimal)item.ApprCost, MidpointRounding.AwayFromZero);

                        //                }
                        //            }

                        //        }
                        //        else if (BudgetingController.lstCostElement.Find(cost => cost.ID.Equals(Int32.Parse(CostGroup.Key))).CostElement == "Software")

                        //        {
                        //            foreach (RequestItems_Table item in CostGroup)
                        //            {
                        //                if (BudgetingController.lstDEPTs.Find(departmnt => departmnt.ID.Equals(Int32.Parse(item.DEPT))).DEPT == dept)
                        //                {

                        //                    SoftwareTotals += Math.Round((decimal)item.ApprCost, MidpointRounding.AwayFromZero);

                        //                }
                        //            }

                        //        }
                        //    }
                        //tempobj.MAE_Totals = MAE_Totals;
                        //OMAE_Totals += MAE_Totals;
                        //tempobj.NMAE_Totals = NMAE_Totals;
                        //ONMAE_Totals += NMAE_Totals;
                        //tempobj.Software_Totals = SoftwareTotals;
                        //OSoftwareTotals += SoftwareTotals;
                        //tempobj.Overall_Totals = MAE_Totals + NMAE_Totals + SoftwareTotals;
                        //viewList.Add(tempobj);
                        //}

                    }
                    else
                    {
                        //presentUserDept = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).Department;
                        connection();
                        //string qry = "select Department from SPOTONData_Table_2022 where NTID = '" + User.Identity.Name.Split('\\')[1].ToUpper() + "' ";

                        //SqlDataReader dr1;
                        //OpenConnection();
                        //SqlCommand cmd1 = new SqlCommand(qry, conn);
                        //dr1 = cmd1.ExecuteReader();
                        //if (dr1.HasRows)
                        //{
                        //    dr1.Read();
                        //    presentUserDept = dr1["Department"].ToString();
                        //}
                        //dr1.Close();
                        //CloseConnection();


                        //string is_CCXC = string.Empty;
                        //if (presentUserDept.Contains("XC"))
                        //    is_CCXC = "XC";
                        //else
                        //    is_CCXC = "CC";

                        ////is_CCXC = "XC";

                        //if (is_CCXC == "CC")
                        //{
                        //    approvedSections.Add("MS/NE-CC");
                        //    approvedSections.Add("MS/ESP-CC");
                        //    approvedSections.Add("MS/ESC-CC");
                        //    approvedSections.Add("MS/ESA-CC");
                        //    approvedSections.Add("MS/ESO-CC");

                        //    //qry = " select Distinct Department from SPOTONData_Table_2022 Where Section in ('MS/NE-CC', 'MS/ESP-CC', 'MS/ESC-CC', 'MS/ESA-CC', 'MS/ESO-CC') ; select DEPT_Table.DEPT, CostElement_Table.CostElement, round(isnull(sum(ApprCost), 0), 0) as ApprCost from RequestItems_Table inner join DEPT_Table on RequestItems_Table.DEPT = DEPT_Table.ID inner join CostElement_Table on RequestItems_Table.CostElement = CostElement_Table.ID where VKM_Year = '" + year + "' and DEPT_Table.DEPT in (select Department from SPOTONData_Table_2022 where Section in ('MS/NE-CC', 'MS/ESP-CC', 'MS/ESC-CC', 'MS/ESA-CC', 'MS/ESO-CC')) group by DEPT_Table.DEPT, CostElement_Table.CostElement order by DEPT_Table.DEPT, CostElement_Table.CostElement";

                        //    //qry = " select Distinct Department from SPOTONData_Table_2022 Where Section in ('MS/NE-CC', 'MS/ESP-CC', 'MS/ESC-CC', 'MS/ESA-CC', 'MS/ESO-CC') ; Select DEPT, isnull(CostElement,'') as CostElement, round(isnull(sum(ApprCost),0),0) as ApprCost from ((select Distinct Id, DEPT  from DEPT_Table where dept in (select Department from SPOTONData_Table_2022 where Section in ('MS/NE-CC', 'MS/ESP-CC', 'MS/ESC-CC', 'MS/ESA-CC', 'MS/ESO-CC'))) as A left join (select RequestItems_Table.Dept as ID,CostElement_Table.CostElement, isnull(ApprCost, 0) as ApprCost from RequestItems_Table inner join DEPT_Table on RequestItems_Table.DEPT = DEPT_Table.ID inner join CostElement_Table on RequestItems_Table.CostElement = CostElement_Table.ID where VKM_Year = '2022') as B on A.Id = B.ID )  group by DEPT, CostElement order by CostElement asc,DEPT desc ";

                        //    qry = " select Distinct Department from SPOTONData_Table_2022 Where Section in ('MS/NE-CC', 'MS/ESP-CC', 'MS/ESC-CC', 'MS/ESA-CC', 'MS/ESO-CC') order by Department ; exec [dbo].[DeptSummary] " + year + ",'CC','" + User.Identity.Name.Split('\\')[1].ToUpper() + "' ";
                        //}
                        //else if (is_CCXC == "XC")
                        //{
                        //    approvedSections.Add("MS/NE-XC");
                        //    approvedSections.Add("MS/NE3-XC");
                        //    //approvedSections.Add("MS/NE4-XC");
                        //    approvedSections.Add("MS/EXA-XC");
                        //    approvedSections.Add("MS/EVC-XC"); //NE1,NE2-XC?

                        //    //qry = " select Distinct Department from SPOTONData_Table_2022 Where Section in ('MS/NE-XC', 'MS/NE3-XC', 'MS/EXA-XC', 'MS/EVC-XC') ; select DEPT_Table.DEPT, CostElement_Table.CostElement, round(isnull(sum(ApprCost), 0), 0) as ApprCost from RequestItems_Table inner join DEPT_Table on RequestItems_Table.DEPT = DEPT_Table.ID inner join CostElement_Table on RequestItems_Table.CostElement = CostElement_Table.ID where VKM_Year = '" + year + "' and DEPT_Table.DEPT in (select Department from SPOTONData_Table_2022 where Section in ('MS/NE-XC', 'MS/NE3-XC', 'MS/EXA-XC', 'MS/EVC-XC')) group by DEPT_Table.DEPT, CostElement_Table.CostElement order by DEPT_Table.DEPT, CostElement_Table.CostElement";
                        //    qry = " select Distinct Department from SPOTONData_Table_2022 Where Section in ('MS/NE-XC', 'MS/NE3-XC', 'MS/EXA-XC', 'MS/EVC-XC') order by Department ; exec [dbo].[DeptSummary] " + year + ",'XC','" + User.Identity.Name.Split('\\')[1].ToUpper() + "' ";
                        //}


                        string qry = " exec [dbo].[BGSW_DeptSummary] " + year + ",'" + User.Identity.Name.Split('\\')[1].ToUpper() + "' ";

                        DataSet ds = new DataSet();

                        OpenConnection();
                        SqlCommand cmd1 = new SqlCommand(qry, conn);
                        SqlDataAdapter da = new SqlDataAdapter(cmd1);
                        da.Fill(ds);
                        CloseConnection();

                        foreach (DataRow item in ds.Tables[0].Rows)
                        {
                            userSectionDeptList.Add(item["Department"].ToString());
                        }

                        userSectionDeptList.Sort();
                        userSectionDeptList.Reverse();

                        foreach (DataRow item in ds.Tables[1].Rows)
                        {
                            decimal MAE_Totals = 0, NMAE_Totals = 0, SoftwareTotals = 0;
                            DeptSummary tempobj = new DeptSummary();
                            tempobj.deptName = item["DEPT"].ToString();
                            //if (item["CostElement"].ToString().ToLower() == "mae")
                            //{
                            MAE_Totals = (decimal)item["MAE"];
                            //}
                            //else if (item["CostElement"].ToString().ToLower() == "non-mae")
                            //{
                            NMAE_Totals = (decimal)item["Non-MAE"];
                            //}
                            //else if (item["CostElement"].ToString().ToLower() == "software")
                            //{
                            SoftwareTotals = (decimal)item["Software"];
                            //}

                            tempobj.MAE_Totals = MAE_Totals;
                            OMAE_Totals += MAE_Totals;
                            tempobj.NMAE_Totals = NMAE_Totals;
                            ONMAE_Totals += NMAE_Totals;
                            tempobj.Software_Totals = SoftwareTotals;
                            OSoftwareTotals += SoftwareTotals;
                            tempobj.Overall_Totals = MAE_Totals + NMAE_Totals + SoftwareTotals;
                            viewList.Add(tempobj);
                        }




                        //    userSectionDeptList = BudgetingController.lstUsers.FindAll(x => approvedSections.Contains(x.Section)).Select(item => item.Department).Distinct().ToList();
                        //    userSectionDeptList.Sort();
                        //    userSectionDeptList.Reverse();
                        //    //CODE TO GET GROUPS OF COST ELEMENT
                        //    IEnumerable<IGrouping<string, RequestItems_Table>> query = reqList.GroupBy(item => item.CostElement);

                        //    foreach (string dept in userSectionDeptList)
                        //    {/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
                        //        decimal MAE_Totals = 0, NMAE_Totals = 0, SoftwareTotals = 0;
                        //        DeptSummary tempobj = new DeptSummary();
                        //        tempobj.deptName = dept;
                        //        //CODE TO GET THE TOTALS OF EACH COST ELEMENT
                        //        foreach (IGrouping<string, RequestItems_Table> CostGroup in query)
                        //        {

                        //            // Iterate over each value in the
                        //            // IGrouping and print the value.
                        //            if (BudgetingController.lstCostElement.Find(cost => cost.ID.Equals(Int32.Parse(CostGroup.Key))).CostElement == "MAE")
                        //            {
                        //                foreach (RequestItems_Table item in CostGroup)
                        //                {
                        //                    if (BudgetingController.lstDEPTs.Find(departmnt => departmnt.ID.Equals(Int32.Parse(item.DEPT))).DEPT == dept)
                        //                    {

                        //                        MAE_Totals += Math.Round((decimal)item.ApprCost, MidpointRounding.AwayFromZero);

                        //                    }
                        //                }
                        //            }
                        //            else if (BudgetingController.lstCostElement.Find(cost => cost.ID.Equals(Int32.Parse(CostGroup.Key))).CostElement == "Non-MAE")

                        //            {
                        //                foreach (RequestItems_Table item in CostGroup)
                        //                {
                        //                    if (BudgetingController.lstDEPTs.Find(departmnt => departmnt.ID.Equals(Int32.Parse(item.DEPT))).DEPT == dept)
                        //                    {

                        //                        NMAE_Totals += Math.Round((decimal)item.ApprCost, MidpointRounding.AwayFromZero);

                        //                    }
                        //                }

                        //            }
                        //            else if (BudgetingController.lstCostElement.Find(cost => cost.ID.Equals(Int32.Parse(CostGroup.Key))).CostElement == "Software")

                        //            {
                        //                foreach (RequestItems_Table item in CostGroup)
                        //                {
                        //                    if (BudgetingController.lstDEPTs.Find(departmnt => departmnt.ID.Equals(Int32.Parse(item.DEPT))).DEPT == dept)
                        //                    {

                        //                        SoftwareTotals += Math.Round((decimal)item.ApprCost, MidpointRounding.AwayFromZero);

                        //                    }
                        //                }

                        //            }
                        //        }
                        //        tempobj.MAE_Totals = MAE_Totals;
                        //        OMAE_Totals += MAE_Totals;
                        //        tempobj.NMAE_Totals = NMAE_Totals;
                        //        ONMAE_Totals += NMAE_Totals;
                        //        tempobj.Software_Totals = SoftwareTotals;
                        //        OSoftwareTotals += SoftwareTotals;
                        //        tempobj.Overall_Totals = MAE_Totals + NMAE_Totals + SoftwareTotals;
                        //        viewList.Add(tempobj);
                        //    }
                        //}

                    }


                    #region commented
                    //List<SummaryDeptView> DTviewList = new List<SummaryDeptView>();
                    //SummaryDeptView objbind = new SummaryDeptView();
                    //objbind.COLUMNS = tempColDefn;
                    ////objbind.DATA = "";

                    //List<DATADEFN> listdata = new List<DATADEFN>();

                    //string[] tempMAEData = new string[userSectionDeptList.Count];
                    //int count = 0;
                    //foreach (var item in viewList)
                    //{
                    //    tempMAEData[count] = item.MAE_Totals.ToString();
                    //    count++;
                    //}

                    //string[] tempNMAEData = new string[userSectionDeptList.Count];
                    //count = 0;
                    //foreach (var item in viewList)
                    //{
                    //    tempNMAEData[count] = item.NMAE_Totals.ToString();
                    //    count++;
                    //}
                    //string[] tempSoftData = new string[userSectionDeptList.Count];
                    //count = 0;
                    //foreach (var item in viewList)
                    //{
                    //    tempSoftData[count] = item.Software_Totals.ToString();
                    //    count++;
                    //}
                    #endregion

                    ///NEW CODE TO GET DEPT SUMMARY



                    dt.Columns.Add("CostElement", typeof(string));
                    dt.Columns.Add("All Depts", typeof(string));
                    foreach (string dept in userSectionDeptList)
                    {
                        dt.Columns.Add(dept, typeof(string));
                    }


                    DataRow dr = dt.NewRow();
                    dr[0] = "MAE";
                    dr[1] = OMAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
                    int count = 2;
                    foreach (var item in viewList)
                    {
                        dr[count] = item.MAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
                        count++;
                    }
                    dt.Rows.Add(dr);

                    dr = dt.NewRow();
                    dr[0] = "Non-MAE";
                    dr[1] = ONMAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
                    count = 2;
                    foreach (var item in viewList)
                    {
                        dr[count] = item.NMAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
                        count++;
                    }
                    dt.Rows.Add(dr);

                    dr = dt.NewRow();
                    dr[0] = "Software";
                    dr[1] = OSoftwareTotals.ToString("C0", CultureInfo.CurrentCulture);
                    count = 2;
                    foreach (var item in viewList)
                    {
                        dr[count] = item.Software_Totals.ToString("C0", CultureInfo.CurrentCulture);
                        count++;
                    }
                    dt.Rows.Add(dr);

                    dr = dt.NewRow();
                    dr[0] = "Totals";
                    dr[1] = (OMAE_Totals + ONMAE_Totals + OSoftwareTotals).ToString("C0", CultureInfo.CurrentCulture);
                    count = 2;
                    foreach (var item in viewList)
                    {
                        dr[count] = item.Overall_Totals.ToString("C0", CultureInfo.CurrentCulture);
                        count++;
                    }
                    dt.Rows.Add(dr);

                    return dt;
                }
            }
            catch (Exception ex)
            {
                WriteLog("Error - deptSummaryComparison : NTID - " + User.Identity.Name.Split('\\')[1].Trim().ToUpper() + ", ErrorMsg : " + ex + ", At - " + DateTime.Now);
                return dt;
            }
        }

        /// <summary>
        /// function to get the Section cost summary data
        /// </summary>
        /// <returns></returns>
        public System.Data.DataTable SectionSummaryData(string year)
        {
            DataTable dt = new DataTable();
            try
            {


                using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

                //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

                {
                    System.Security.Principal.IPrincipal User = System.Web.HttpContext.Current.User;
                    dt = new System.Data.DataTable();


                    List<DeptSummary> viewList = new List<DeptSummary>();
                    List<string> userSectionDeptList = new List<string>();
                    List<string> userSectionList = new List<string>();
                    List<string> approvedSections = new List<string>();
                    string presentUserDept = string.Empty;
                    List<RequestItems_Table> reqList1 = new List<RequestItems_Table>(); //Created when the section summary wasn't loading - when DEPT's SECTION != approvedSections (eg:ECF)
                    decimal OMAE_Totals = 0, ONMAE_Totals = 0, OSoftwareTotals = 0;

                    if (year.Contains("2020"))
                    {

                        //presentUserDept = BudgetingController.lstUsers_2020.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).Department;
                        //approvedSections.Add("NE2-VS");
                        //approvedSections.Add("NE1-VS");
                        //approvedSections.Add("ESD");


                        //List<KeyValuePair<string, string>> dept_section = new List<KeyValuePair<string, string>>();
                        //userSectionDeptList = BudgetingController.lstUsers_2020.FindAll(x => approvedSections.Contains(x.Section)).Select(item => item.Department).Distinct().ToList();
                        //userSectionDeptList.Sort();
                        //userSectionDeptList.Reverse();


                        //List<RequestItems_Table> reqList = db.RequestItems_Table.ToList<RequestItems_Table>().FindAll(x => x.ApprovedDH == true).FindAll(x => x.RequestDate.ToString().Contains(year)).FindAll(v => v.Fund == null || v.Fund.Trim().Contains("2"));

                        //foreach (RequestItems_Table item in reqList)
                        //{
                        //    if (userSectionDeptList.Contains(BudgetingController.lstDEPTs.Find(departmnt => departmnt.ID.Equals(Int32.Parse(item.DEPT))).DEPT))
                        //    {
                        //        reqList1.Add(item);
                        //    }
                        //}


                        //userSectionList = BudgetingController.lstUsers_2020.FindAll(x => approvedSections.Contains(x.Section)).Select(item => item.Section).Distinct().ToList();
                        //foreach (string dept in userSectionDeptList)
                        //{
                        //    KeyValuePair<string, string> dept_sec = new KeyValuePair<string, string>(dept, BudgetingController.lstUsers_2020.FirstOrDefault(x => x.Department.Equals(dept)).Section);
                        //    dept_section.Add(dept_sec);
                        //}
                        ////CODE TO GET GROUPS OF COST ELEMENT
                        //IEnumerable<IGrouping<string, RequestItems_Table>> query = reqList1.GroupBy(item => item.CostElement); //Created when the section summary wasn't loading - when DEPT's SECTION != approvedSections (eg:ECF)

                        //foreach (string section in userSectionList)
                        //{
                        //    decimal MAE_Totals = 0, NMAE_Totals = 0, SoftwareTotals = 0;
                        //    DeptSummary tempobj = new DeptSummary();
                        //    tempobj.deptName = section;
                        //    //CODE TO GET THE TOTALS OF EACH COST ELEMENT
                        //    foreach (IGrouping<string, RequestItems_Table> CostGroup in query)
                        //    {

                        //        // Iterate over each value in the
                        //        // IGrouping and print the value.
                        //        if (BudgetingController.lstCostElement.Find(cost => cost.ID.Equals(Int32.Parse(CostGroup.Key))).CostElement == "MAE")
                        //        {
                        //            foreach (RequestItems_Table item in CostGroup)
                        //            {
                        //                if (dept_section.Find(x => x.Key.Equals(BudgetingController.lstDEPTs.Find(departmnt => departmnt.ID.Equals(Int32.Parse(item.DEPT))).DEPT)).Value.Equals(section))
                        //                {

                        //                    MAE_Totals += Math.Round((decimal)item.ApprCost, MidpointRounding.AwayFromZero);

                        //                }
                        //            }
                        //        }
                        //        else if (BudgetingController.lstCostElement.Find(cost => cost.ID.Equals(Int32.Parse(CostGroup.Key))).CostElement == "Non-MAE")
                        //        {
                        //            foreach (RequestItems_Table item in CostGroup)
                        //            {
                        //                if (dept_section.Find(x => x.Key.Equals(BudgetingController.lstDEPTs.Find(departmnt => departmnt.ID.Equals(Int32.Parse(item.DEPT))).DEPT)).Value.Equals(section))
                        //                {

                        //                    NMAE_Totals += Math.Round((decimal)item.ApprCost, MidpointRounding.AwayFromZero);

                        //                }
                        //            }

                        //        }
                        //        else if (BudgetingController.lstCostElement.Find(cost => cost.ID.Equals(Int32.Parse(CostGroup.Key))).CostElement == "Software")
                        //        {
                        //            foreach (RequestItems_Table item in CostGroup)
                        //            {
                        //                if (dept_section.Find(x => x.Key.Equals(BudgetingController.lstDEPTs.Find(departmnt => departmnt.ID.Equals(Int32.Parse(item.DEPT))).DEPT)).Value.Equals(section))
                        //                {

                        //                    SoftwareTotals += Math.Round((decimal)item.ApprCost, MidpointRounding.AwayFromZero);

                        //                }
                        //            }

                        //        }
                        //    }
                        //    tempobj.MAE_Totals = MAE_Totals;
                        //    OMAE_Totals += MAE_Totals;
                        //    tempobj.NMAE_Totals = NMAE_Totals;
                        //    ONMAE_Totals += NMAE_Totals;
                        //    tempobj.Software_Totals = SoftwareTotals;
                        //    OSoftwareTotals += SoftwareTotals;
                        //    tempobj.Overall_Totals = MAE_Totals + NMAE_Totals + SoftwareTotals;
                        //    viewList.Add(tempobj);

                        //}



                        //presentUserDept = BudgetingController.lstUsers_2020.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).Department;
                        approvedSections.Add("NE2-VS");
                        approvedSections.Add("NE1-VS");
                        approvedSections.Add("ESD");
                        //userSectionDeptList = BudgetingController.lstUsers_2020.FindAll(x => approvedSections.Contains(x.Section)).Select(item => item.Department).Distinct().ToList();
                        //userSectionDeptList.Sort();
                        //userSectionDeptList.Reverse();

                        connection();
                        string qry = "select Department from SPOTONData where NTID = '" + User.Identity.Name.Split('\\')[1].ToUpper() + "' ";

                        SqlDataReader dr1;
                        OpenConnection();
                        SqlCommand cmd1 = new SqlCommand(qry, conn);
                        dr1 = cmd1.ExecuteReader();
                        if (dr1.HasRows)
                        {
                            dr1.Read();
                            presentUserDept = dr1["Department"].ToString();
                        }
                        dr1.Close();
                        CloseConnection();

                        DataSet ds = new DataSet();
                        qry = " select Distinct Section from SPOTONData Where Section in ('NE2-VS', 'NE1-VS', 'ESD') ; exec [dbo].[SectionSummary] " + year + ",'','" + User.Identity.Name.Split('\\')[1].ToUpper() + "'  ";
                        OpenConnection();
                        cmd1 = new SqlCommand(qry, conn);
                        SqlDataAdapter da = new SqlDataAdapter(cmd1);
                        da.Fill(ds);
                        CloseConnection();

                        foreach (DataRow item in ds.Tables[0].Rows)
                        {
                            userSectionList.Add(item["Section"].ToString());
                        }

                        userSectionList.Sort();
                        userSectionList.Reverse();


                        foreach (DataRow item in ds.Tables[1].Rows)
                        {
                            decimal MAE_Totals = 0, NMAE_Totals = 0, SoftwareTotals = 0;
                            DeptSummary tempobj = new DeptSummary();
                            tempobj.deptName = item["DEPT"].ToString();
                            //if (item["CostElement"].ToString().ToLower() == "mae")
                            //{
                            //    MAE_Totals = (decimal)item["ApprCost"];
                            //}
                            //else if (item["CostElement"].ToString().ToLower() == "non-mae")
                            //{
                            //    NMAE_Totals = (decimal)item["ApprCost"];
                            //}
                            //else if (item["CostElement"].ToString().ToLower() == "software")
                            //{
                            //    SoftwareTotals = (decimal)item["ApprCost"];
                            //}


                            MAE_Totals = (decimal)item["MAE"];
                            NMAE_Totals = (decimal)item["Non-MAE"];
                            SoftwareTotals = (decimal)item["Software"];

                            tempobj.MAE_Totals = MAE_Totals;
                            OMAE_Totals += MAE_Totals;
                            tempobj.NMAE_Totals = NMAE_Totals;
                            ONMAE_Totals += NMAE_Totals;
                            tempobj.Software_Totals = SoftwareTotals;
                            OSoftwareTotals += SoftwareTotals;
                            tempobj.Overall_Totals = MAE_Totals + NMAE_Totals + SoftwareTotals;
                            viewList.Add(tempobj);

                        }
                    }
                    else
                    {
                        //presentUserDept = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).Department;
                        //string is_CCXC = string.Empty;
                        //if (presentUserDept.Contains("XC"))
                        //    is_CCXC = "XC";
                        //else
                        //    is_CCXC = "CC";

                        ////is_CCXC = "XC";



                        //if (is_CCXC == "CC")
                        //{
                        //    approvedSections.Add("MS/NE-CC");
                        //    approvedSections.Add("MS/ESP-CC");
                        //    approvedSections.Add("MS/ESC-CC");
                        //    approvedSections.Add("MS/ESA-CC");
                        //    approvedSections.Add("MS/ESO-CC");
                        //}
                        //else if (is_CCXC == "XC")
                        //{
                        //    approvedSections.Add("MS/NE-XC");
                        //    approvedSections.Add("MS/NE3-XC");
                        //    //approvedSections.Add("MS/NE4-XC");
                        //    approvedSections.Add("MS/EXA-XC");
                        //    approvedSections.Add("MS/EVC-XC"); //NE1,NE2-XC?
                        //}

                        //List<KeyValuePair<string, string>> dept_section = new List<KeyValuePair<string, string>>();
                        //userSectionDeptList = BudgetingController.lstUsers.FindAll(x => approvedSections.Contains(x.Section)).Select(item => item.Department).Distinct().ToList();
                        //userSectionDeptList.Sort();
                        //userSectionDeptList.Reverse();


                        //List<RequestItems_Table> reqList = db.RequestItems_Table.ToList<RequestItems_Table>().FindAll(x => x.ApprovedDH == true).FindAll(x => x.RequestDate.ToString().Contains(year)).FindAll(v => v.Fund == null || v.Fund.Trim().Contains("2"));

                        //foreach (RequestItems_Table item in reqList)
                        //{
                        //    if (userSectionDeptList.Contains(BudgetingController.lstDEPTs.Find(departmnt => departmnt.ID.Equals(Int32.Parse(item.DEPT))).DEPT))
                        //    {
                        //        reqList1.Add(item);
                        //    }
                        //}


                        //userSectionList = BudgetingController.lstUsers.FindAll(x => approvedSections.Contains(x.Section)).Select(item => item.Section).Distinct().ToList();
                        //foreach (string dept in userSectionDeptList)
                        //{
                        //    KeyValuePair<string, string> dept_sec = new KeyValuePair<string, string>(dept, BudgetingController.lstUsers.FirstOrDefault(x => x.Department.Equals(dept)).Section);
                        //    dept_section.Add(dept_sec);
                        //}
                        ////CODE TO GET GROUPS OF COST ELEMENT
                        //IEnumerable<IGrouping<string, RequestItems_Table>> query = reqList1.GroupBy(item => item.CostElement); //Created when the section summary wasn't loading - when DEPT's SECTION != approvedSections (eg:ECF)

                        //foreach (string section in userSectionList)
                        //{
                        //    decimal MAE_Totals = 0, NMAE_Totals = 0, SoftwareTotals = 0;
                        //    DeptSummary tempobj = new DeptSummary();
                        //    tempobj.deptName = section;
                        //    //CODE TO GET THE TOTALS OF EACH COST ELEMENT
                        //    foreach (IGrouping<string, RequestItems_Table> CostGroup in query)
                        //    {

                        //        // Iterate over each value in the
                        //        // IGrouping and print the value.
                        //        if (BudgetingController.lstCostElement.Find(cost => cost.ID.Equals(Int32.Parse(CostGroup.Key))).CostElement == "MAE")
                        //        {
                        //            foreach (RequestItems_Table item in CostGroup)
                        //            {
                        //                if (dept_section.Find(x => x.Key.Equals(BudgetingController.lstDEPTs.Find(departmnt => departmnt.ID.Equals(Int32.Parse(item.DEPT))).DEPT)).Value.Equals(section))
                        //                {

                        //                    MAE_Totals += Math.Round((decimal)item.ApprCost, MidpointRounding.AwayFromZero);

                        //                }
                        //            }
                        //        }
                        //        else if (BudgetingController.lstCostElement.Find(cost => cost.ID.Equals(Int32.Parse(CostGroup.Key))).CostElement == "Non-MAE")
                        //        {
                        //            foreach (RequestItems_Table item in CostGroup)
                        //            {
                        //                if (dept_section.Find(x => x.Key.Equals(BudgetingController.lstDEPTs.Find(departmnt => departmnt.ID.Equals(Int32.Parse(item.DEPT))).DEPT)).Value.Equals(section))
                        //                {

                        //                    NMAE_Totals += Math.Round((decimal)item.ApprCost, MidpointRounding.AwayFromZero);

                        //                }
                        //            }

                        //        }
                        //        else if (BudgetingController.lstCostElement.Find(cost => cost.ID.Equals(Int32.Parse(CostGroup.Key))).CostElement == "Software")
                        //        {
                        //            foreach (RequestItems_Table item in CostGroup)
                        //            {
                        //                if (dept_section.Find(x => x.Key.Equals(BudgetingController.lstDEPTs.Find(departmnt => departmnt.ID.Equals(Int32.Parse(item.DEPT))).DEPT)).Value.Equals(section))
                        //                {

                        //                    SoftwareTotals += Math.Round((decimal)item.ApprCost, MidpointRounding.AwayFromZero);

                        //                }
                        //            }

                        //        }
                        //    }
                        //    tempobj.MAE_Totals = MAE_Totals;
                        //    OMAE_Totals += MAE_Totals;
                        //    tempobj.NMAE_Totals = NMAE_Totals;
                        //    ONMAE_Totals += NMAE_Totals;
                        //    tempobj.Software_Totals = SoftwareTotals;
                        //    OSoftwareTotals += SoftwareTotals;
                        //    tempobj.Overall_Totals = MAE_Totals + NMAE_Totals + SoftwareTotals;
                        //    viewList.Add(tempobj);

                        //}


                        ////////////////////////////////////////////////

                        connection();
                        //string qry = "select Department from SPOTONData_Table_2022 where NTID = '" + User.Identity.Name.Split('\\')[1].ToUpper() + "' ";

                        //SqlDataReader dr1;
                        //OpenConnection();
                        //SqlCommand cmd1 = new SqlCommand(qry, conn);
                        //dr1 = cmd1.ExecuteReader();
                        //if (dr1.HasRows)
                        //{
                        //    dr1.Read();
                        //    presentUserDept = dr1["Department"].ToString();
                        //}
                        //dr1.Close();
                        //CloseConnection();


                        //string is_CCXC = string.Empty;
                        //if (presentUserDept.Contains("XC"))
                        //    is_CCXC = "XC";
                        //else
                        //    is_CCXC = "CC";

                        ////is_CCXC = "XC";



                        //if (is_CCXC == "CC")
                        //{
                        //    approvedSections.Add("MS/NE-CC");
                        //    approvedSections.Add("MS/ESP-CC");
                        //    approvedSections.Add("MS/ESC-CC");
                        //    approvedSections.Add("MS/ESA-CC");
                        //    approvedSections.Add("MS/ESO-CC");

                        //    //qry = " select Distinct Department from SPOTONData_Table_2022 Where Section in ('MS/NE-CC', 'MS/ESP-CC', 'MS/ESC-CC', 'MS/ESA-CC', 'MS/ESO-CC') ; select DEPT_Table.DEPT, CostElement_Table.CostElement, round(isnull(sum(ApprCost), 0), 0) as ApprCost from RequestItems_Table inner join DEPT_Table on RequestItems_Table.DEPT = DEPT_Table.ID inner join CostElement_Table on RequestItems_Table.CostElement = CostElement_Table.ID where VKM_Year = '" + year + "' and DEPT_Table.DEPT in (select Department from SPOTONData_Table_2022 where Section in ('MS/NE-CC', 'MS/ESP-CC', 'MS/ESC-CC', 'MS/ESA-CC', 'MS/ESO-CC')) group by DEPT_Table.DEPT, CostElement_Table.CostElement order by DEPT_Table.DEPT, CostElement_Table.CostElement";

                        //    //qry = " select Distinct Department from SPOTONData_Table_2022 Where Section in ('MS/NE-CC', 'MS/ESP-CC', 'MS/ESC-CC', 'MS/ESA-CC', 'MS/ESO-CC') ; Select DEPT, isnull(CostElement,'') as CostElement, round(isnull(sum(ApprCost),0),0) as ApprCost from ((select Distinct Id, DEPT  from DEPT_Table where dept in (select Department from SPOTONData_Table_2022 where Section in ('MS/NE-CC', 'MS/ESP-CC', 'MS/ESC-CC', 'MS/ESA-CC', 'MS/ESO-CC'))) as A left join (select RequestItems_Table.Dept as ID,CostElement_Table.CostElement, isnull(ApprCost, 0) as ApprCost from RequestItems_Table inner join DEPT_Table on RequestItems_Table.DEPT = DEPT_Table.ID inner join CostElement_Table on RequestItems_Table.CostElement = CostElement_Table.ID where VKM_Year = '2022') as B on A.Id = B.ID )  group by DEPT, CostElement order by CostElement asc,DEPT desc ";

                        //    qry = " select Distinct Section from SPOTONData_Table_2022 Where Section in ('MS/NE-CC', 'MS/ESP-CC', 'MS/ESC-CC', 'MS/ESA-CC', 'MS/ESO-CC') order by Section ; exec [dbo].[SectionSummary] " + int.Parse(year) + 1 + ",'CC','" + User.Identity.Name.Split('\\')[1].ToUpper() + "' ";
                        //}
                        //else if (is_CCXC == "XC")
                        //{
                        //    approvedSections.Add("MS/NE-XC");
                        //    approvedSections.Add("MS/NE3-XC");
                        //    //approvedSections.Add("MS/NE4-XC");
                        //    approvedSections.Add("MS/EXA-XC");
                        //    approvedSections.Add("MS/EVC-XC"); //NE1,NE2-XC?

                        //    //qry = " select Distinct Department from SPOTONData_Table_2022 Where Section in ('MS/NE-XC', 'MS/NE3-XC', 'MS/EXA-XC', 'MS/EVC-XC') ; select DEPT_Table.DEPT, CostElement_Table.CostElement, round(isnull(sum(ApprCost), 0), 0) as ApprCost from RequestItems_Table inner join DEPT_Table on RequestItems_Table.DEPT = DEPT_Table.ID inner join CostElement_Table on RequestItems_Table.CostElement = CostElement_Table.ID where VKM_Year = '" + year + "' and DEPT_Table.DEPT in (select Department from SPOTONData_Table_2022 where Section in ('MS/NE-XC', 'MS/NE3-XC', 'MS/EXA-XC', 'MS/EVC-XC')) group by DEPT_Table.DEPT, CostElement_Table.CostElement order by DEPT_Table.DEPT, CostElement_Table.CostElement";
                        //    qry = " select Distinct Section from SPOTONData_Table_2022 Where Section in ('MS/NE-XC', 'MS/NE3-XC', 'MS/EXA-XC', 'MS/EVC-XC') order by Section ; exec [dbo].[SectionSummary] " + int.Parse(year)+1 + ",'XC','" + User.Identity.Name.Split('\\')[1].ToUpper() + "' ";
                        //}


                        string qry = " exec [dbo].[BGSW_SectionSummary] " + year + ",'" + User.Identity.Name.Split('\\')[1].ToUpper() + "' ";

                        DataSet ds = new DataSet();

                        OpenConnection();
                        SqlCommand cmd1 = new SqlCommand(qry, conn);
                        SqlDataAdapter da = new SqlDataAdapter(cmd1);
                        da.Fill(ds);
                        CloseConnection();

                        foreach (DataRow item in ds.Tables[0].Rows)
                        {
                            userSectionList.Add(item["Section"].ToString());
                        }

                        userSectionList.Sort();
                        userSectionList.Reverse();

                        foreach (DataRow item in ds.Tables[1].Rows)
                        {
                            decimal MAE_Totals = 0, NMAE_Totals = 0, SoftwareTotals = 0;
                            DeptSummary tempobj = new DeptSummary();
                            tempobj.deptName = item["DEPT"].ToString();
                            //if (item["CostElement"].ToString().ToLower() == "mae")
                            //{
                            MAE_Totals = (decimal)item["MAE"];
                            //}
                            //else if (item["CostElement"].ToString().ToLower() == "non-mae")
                            //{
                            NMAE_Totals = (decimal)item["Non-MAE"];
                            //}
                            //else if (item["CostElement"].ToString().ToLower() == "software")
                            //{
                            SoftwareTotals = (decimal)item["Software"];
                            //}

                            tempobj.MAE_Totals = MAE_Totals;
                            OMAE_Totals += MAE_Totals;
                            tempobj.NMAE_Totals = NMAE_Totals;
                            ONMAE_Totals += NMAE_Totals;
                            tempobj.Software_Totals = SoftwareTotals;
                            OSoftwareTotals += SoftwareTotals;
                            tempobj.Overall_Totals = MAE_Totals + NMAE_Totals + SoftwareTotals;
                            viewList.Add(tempobj);

                        }
                    }





                    #region commented

                    #endregion

                    ///NEW CODE TO GET DEPT SUMMARY



                    dt.Columns.Add("CostElement", typeof(string));
                    dt.Columns.Add("All Sections", typeof(string));
                    foreach (string section in userSectionList)
                    {
                        dt.Columns.Add(section, typeof(string));
                    }


                    DataRow dr = dt.NewRow();
                    dr[0] = "MAE";
                    dr[1] = OMAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
                    int count = 2;
                    foreach (var item in viewList)
                    {
                        dr[count] = item.MAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
                        count++;
                    }
                    dt.Rows.Add(dr);

                    dr = dt.NewRow();
                    dr[0] = "Non-MAE";
                    dr[1] = ONMAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
                    count = 2;
                    foreach (var item in viewList)
                    {
                        dr[count] = item.NMAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
                        count++;
                    }
                    dt.Rows.Add(dr);

                    dr = dt.NewRow();
                    dr[0] = "Software";
                    dr[1] = OSoftwareTotals.ToString("C0", CultureInfo.CurrentCulture);
                    count = 2;
                    foreach (var item in viewList)
                    {
                        dr[count] = item.Software_Totals.ToString("C0", CultureInfo.CurrentCulture);
                        count++;
                    }
                    dt.Rows.Add(dr);

                    dr = dt.NewRow();
                    dr[0] = "Totals";
                    dr[1] = (OMAE_Totals + ONMAE_Totals + OSoftwareTotals).ToString("C0", CultureInfo.CurrentCulture);
                    count = 2;
                    foreach (var item in viewList)
                    {
                        dr[count] = item.Overall_Totals.ToString("C0", CultureInfo.CurrentCulture);
                        count++;
                    }
                    dt.Rows.Add(dr);

                    return dt;
                }
            }
            catch (Exception ex)
            {
                WriteLog("Error - SectionSummaryComparison : NTID - " + User.Identity.Name.Split('\\')[1].Trim().ToUpper() + ", ErrorMsg : " + ex + ", At - " + DateTime.Now);
                return dt;
            }
        }

        /// <summary>
        /// function to get summary of Pending queue data - filter for CC depts/ XC depts based on user's dept
        /// </summary>
        /// <returns></returns>
        //public System.Data.DataTable SectionPendingSummaryData(string year)
        //{
        //    System.Data.DataTable dt = new System.Data.DataTable();
        //    try
        //    {
        //        if (year.Contains("2020"))
        //            return dt;

        //        using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //        //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //        {
        //            System.Security.Principal.IPrincipal User = System.Web.HttpContext.Current.User;

        //            string presentUserDept = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).Department;
        //            string is_CCXC = string.Empty;
        //            List<RequestItems_Table> reqList = new List<RequestItems_Table>();
        //            if (presentUserDept.Contains("XC"))
        //            {
        //                is_CCXC = "XC";
        //                reqList = db.RequestItems_Table.ToList<RequestItems_Table>().FindAll(x => x.RequestDate.ToString().Contains(year)).FindAll(dpt => BudgetingController.lstDEPTs.Find(x => x.ID.ToString().Contains(dpt.DEPT)).DEPT.Contains("XC")).FindAll(v => v.Fund == null || v.Fund.Trim().Contains("2"));

        //            }
        //            else
        //            {
        //                is_CCXC = "CC";
        //                reqList = db.RequestItems_Table.ToList<RequestItems_Table>().FindAll(x => x.RequestDate.ToString().Contains(year)).FindAll(dpt => !BudgetingController.lstDEPTs.Find(x => x.ID.ToString().Contains(dpt.DEPT)).DEPT.Contains("XC")).FindAll(v => v.Fund == null || v.Fund.Trim().Contains("2"));

        //            }



        //            List<PendingSummary> viewList = new List<PendingSummary>();

        //            IEnumerable<IGrouping<string, RequestItems_Table>> query = reqList.GroupBy(item => item.BU);

        //            decimal L1_Totals = 0, L2_Totals = 0, L3_Totals = 0;
        //            foreach (IGrouping<string, RequestItems_Table> BU in query)
        //            {
        //                if (BudgetingController.lstBUs.Find(bu => bu.ID.Equals(Int32.Parse(BU.Key))).BU == "MB")
        //                {
        //                    L1_Totals = 0; L2_Totals = 0; L3_Totals = 0;
        //                    PendingSummary tempobj = new PendingSummary();
        //                    tempobj.BUName = BudgetingController.lstBUs.Find(bu => bu.ID.Equals(Int32.Parse(BU.Key))).BU;

        //                    foreach (RequestItems_Table item in BU)
        //                    {
        //                        if (!(bool)item.ApprovalDH)
        //                        {
        //                            L1_Totals++;
        //                        }
        //                        else if ((bool)item.ApprovalDH && (bool)!item.ApprovedDH)
        //                        {
        //                            L2_Totals++;
        //                        }
        //                        else if ((bool)item.ApprovalSH && (bool)!item.ApprovedSH)
        //                        {
        //                            L3_Totals++;
        //                        }
        //                    }

        //                    tempobj.L1_Totals = L1_Totals;
        //                    tempobj.L2_Totals = L2_Totals;
        //                    tempobj.L3_Totals = L3_Totals;
        //                    viewList.Add(tempobj);

        //                }

        //            }

        //            foreach (IGrouping<string, RequestItems_Table> BU in query)
        //            {
        //                if (BudgetingController.lstBUs.Find(bu => bu.ID.Equals(Int32.Parse(BU.Key))).BU != "MB")
        //                {
        //                    L1_Totals = 0; L2_Totals = 0; L3_Totals = 0;
        //                    PendingSummary tempobj = new PendingSummary();
        //                    tempobj.BUName = BudgetingController.lstBUs.Find(bu => bu.ID.Equals(Int32.Parse(BU.Key))).BU;

        //                    foreach (RequestItems_Table item in BU)
        //                    {
        //                        if (!(bool)item.ApprovalDH)
        //                        {
        //                            L1_Totals++;
        //                        }
        //                        else if ((bool)item.ApprovalDH && (bool)!item.ApprovedDH)
        //                        {
        //                            L2_Totals++;
        //                        }
        //                        else if ((bool)item.ApprovalSH && (bool)!item.ApprovedSH)
        //                        {
        //                            L3_Totals++;
        //                        }
        //                    }

        //                    tempobj.L1_Totals = L1_Totals;
        //                    tempobj.L2_Totals = L2_Totals;
        //                    tempobj.L3_Totals = L3_Totals;
        //                    viewList.Add(tempobj);
        //                }

        //            }





        //            dt.Columns.Add("BU", typeof(string));
        //            dt.Columns.Add("L1 (Requestor)", typeof(Int32));
        //            dt.Columns.Add("L2 (HoEs review)", typeof(Int32));
        //            dt.Columns.Add("L3 (VKM SPOCs review)", typeof(Int32));



        //            foreach (var item in viewList)
        //            {
        //                DataRow dr = dt.NewRow();
        //                int count = 0;
        //                dr[count++] = item.BUName;
        //                dr[count++] = item.L1_Totals;
        //                dr[count++] = item.L2_Totals;
        //                dr[count++] = item.L3_Totals;
        //                dt.Rows.Add(dr);
        //            }
        //            return dt;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        WriteLog("Error - SectionPendingComparison : NTID - " + User.Identity.Name.Split('\\')[1].Trim().ToUpper() + ", ErrorMsg : " + ex + ", At - " + DateTime.Now);
        //        return dt;
        //    }
        //}


        public System.Data.DataTable SectionPendingSummaryData(string year)
        {
            DataTable dt = new DataTable();
            DataTable dtSection = new DataTable();
            try
            {
                if (year.Contains("2020"))
                    return dt;

                System.Security.Principal.IPrincipal User = System.Web.HttpContext.Current.User;

                List<PendingSummary> viewList = new List<PendingSummary>();

                decimal L1_Totals = 0, L2_Totals = 0, L3_Totals = 0;

                string qry = "";
                connection();
                qry = " exec [dbo].[BGSW_SectionPendingSummary] " + year + ",'" + User.Identity.Name.Split('\\')[1].ToUpper() + "' ";



                OpenConnection();
                SqlCommand cmd1 = new SqlCommand(qry, conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd1);
                da.Fill(dtSection);
                CloseConnection();

                foreach (DataRow dr in dtSection.Rows)
                {
                    PendingSummary tempobj = new PendingSummary();
                    tempobj.BUName = dr["BU"].ToString();
                    tempobj.L1_Totals = Convert.ToDecimal(dr["L1"]);
                    tempobj.L2_Totals = Convert.ToDecimal(dr["L2"]);
                    tempobj.L3_Totals = Convert.ToDecimal(dr["L3"]);
                    viewList.Add(tempobj);
                }



                dt.Columns.Add("BU", typeof(string));
                dt.Columns.Add("L1 (Requestor)", typeof(Int32));
                dt.Columns.Add("L2 (HoEs review)", typeof(Int32));
                dt.Columns.Add("L3 (VKM SPOCs review)", typeof(Int32));





                foreach (var item in viewList)
                {
                    DataRow dr = dt.NewRow();
                    int count = 0;
                    dr[count++] = item.BUName;
                    dr[count++] = item.L1_Totals;
                    dr[count++] = item.L2_Totals;
                    dr[count++] = item.L3_Totals;
                    dt.Rows.Add(dr);
                }
                return dt;
                //}
            }
            catch (Exception ex)
            {
                WriteLog("Error - SectionPendingComparison : NTID - " + User.Identity.Name.Split('\\')[1].Trim().ToUpper() + ", ErrorMsg : " + ex + ", At - " + DateTime.Now);
                return dt;
            }
        }

        ///// <summary>
        ///// function to get summary of Pending queue data
        ///// </summary>
        ///// <returns></returns>
        //public System.Data.DataTable SectionPendingSummaryData(string year)
        //{

        //    using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //    //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //    {
        //        System.Security.Principal.IPrincipal User = System.Web.HttpContext.Current.User;
        //        List<RequestItems_Table> reqList = db.RequestItems_Table.ToList<RequestItems_Table>().FindAll(x => x.RequestDate.ToString().Contains(year));
        //        List<PendingSummary> viewList = new List<PendingSummary>();

        //        IEnumerable<IGrouping<string, RequestItems_Table>> query = reqList.GroupBy(item => item.BU);

        //        decimal L1_Totals = 0, L2_Totals = 0, L3_Totals = 0;
        //        foreach (IGrouping<string, RequestItems_Table> BU in query)
        //        {
        //            if (BudgetingController.lstBUs.Find(bu => bu.ID.Equals(Int32.Parse(BU.Key))).BU == "AS")
        //            {
        //                L1_Totals = 0; L2_Totals = 0; L3_Totals = 0;
        //                PendingSummary tempobj = new PendingSummary();
        //                tempobj.BUName = BudgetingController.lstBUs.Find(bu => bu.ID.Equals(Int32.Parse(BU.Key))).BU;

        //                foreach (RequestItems_Table item in BU)
        //                {
        //                    if (!(bool)item.ApprovalDH)
        //                    {
        //                        L1_Totals++;
        //                    }
        //                    else if ((bool)item.ApprovalDH && (bool)!item.ApprovedDH)
        //                    {
        //                        L2_Totals++;
        //                    }
        //                    else if ((bool)item.ApprovalSH && (bool)!item.ApprovedSH)
        //                    {
        //                        L3_Totals++;
        //                    }
        //                }

        //                tempobj.L1_Totals = L1_Totals;
        //                tempobj.L2_Totals = L2_Totals;
        //                tempobj.L3_Totals = L3_Totals;
        //                viewList.Add(tempobj);

        //            }

        //        }

        //        foreach (IGrouping<string, RequestItems_Table> BU in query)
        //        {
        //            if (BudgetingController.lstBUs.Find(bu => bu.ID.Equals(Int32.Parse(BU.Key))).BU != "AS")
        //            {
        //                L1_Totals = 0; L2_Totals = 0; L3_Totals = 0;
        //                PendingSummary tempobj = new PendingSummary();
        //                tempobj.BUName = BudgetingController.lstBUs.Find(bu => bu.ID.Equals(Int32.Parse(BU.Key))).BU;

        //                foreach (RequestItems_Table item in BU)
        //                {
        //                    if (!(bool)item.ApprovalDH)
        //                    {
        //                        L1_Totals++;
        //                    }
        //                    else if ((bool)item.ApprovalDH && (bool)!item.ApprovedDH)
        //                    {
        //                        L2_Totals++;
        //                    }
        //                    else if ((bool)item.ApprovalSH && (bool)!item.ApprovedSH)
        //                    {
        //                        L3_Totals++;
        //                    }
        //                }

        //                tempobj.L1_Totals = L1_Totals;
        //                tempobj.L2_Totals = L2_Totals;
        //                tempobj.L3_Totals = L3_Totals;
        //                viewList.Add(tempobj);
        //            }

        //        }



        //        System.Data.DataTable dt = new System.Data.DataTable();

        //        dt.Columns.Add("BU", typeof(string));
        //        dt.Columns.Add("L1 (Requestor)", typeof(Int32));
        //        dt.Columns.Add("L2 (HoEs review)", typeof(Int32));
        //        dt.Columns.Add("L3 (VKM SPOCs review)", typeof(Int32));



        //        foreach (var item in viewList)
        //        {
        //            DataRow dr = dt.NewRow();
        //            int count = 0;
        //            dr[count++] = item.BUName;
        //            dr[count++] = item.L1_Totals;
        //            dr[count++] = item.L2_Totals;
        //            dr[count++] = item.L3_Totals;
        //            dt.Rows.Add(dr);
        //        }
        //        return dt;
        //    }
        //}
        /// <summary>
        /// function to get the Dept Summary data to View
        /// </summary>
        /// <returns></returns>
        public ActionResult GetDeptSummaryData(string year)
        {
            System.Data.DataTable dt = new System.Data.DataTable();

            dt = DeptSummaryData(false, year);
            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            BudgetParam t = new BudgetParam();
            try
            {
                List<columnsinfo> _col = new List<columnsinfo>();

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
            catch (Exception ex)
            {
                WriteLog("Error - DeptSummary : NTID - " + User.Identity.Name.Split('\\')[1].Trim().ToUpper() + ", ErrorMsg : " + ex + ", At - " + DateTime.Now);

            }
            return Json(new { data = t }, JsonRequestBehavior.AllowGet);

        }
        /// <summary>
        /// function to get the section summary data to view
        /// </summary>
        /// <returns></returns>
        public ActionResult GetSectionSummaryData(string year)
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            dt = SectionSummaryData(year);
            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();

            BudgetParam t = new BudgetParam();
            try
            {
                List<columnsinfo> _col = new List<columnsinfo>();

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
            catch (Exception ex)
            {
                WriteLog("Error - SectionSummary : NTID - " + User.Identity.Name.Split('\\')[1].Trim().ToUpper() + ", ErrorMsg : " + ex + ", At - " + DateTime.Now);

            }

            return Json(new { data = t }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// function to get the L3 review items to view
        ///  /// /// <param name="year"></param>
        /// </summary>
        /// <returns></returns>
        public List<RequestItemsRepoView> GetData1(string year)
        {
            List<RequestItemsRepoView> viewList = new List<RequestItemsRepoView>();
            List<RequestItemsRepoView> viewList1 = new List<RequestItemsRepoView>();
            DataTable dt = new DataTable();
            DataTable dtmain = new DataTable();
            try
            {
                System.Security.Principal.IPrincipal User = System.Web.HttpContext.Current.User;
                List<string> allowedBUs = new List<string>();

                using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())
                    
                //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())
                {

                    //List<RequestItems_Table> reqList = db.RequestItems_Table.OrderBy(x=>x.RequestID)/*OrderBy(x => x.ApprovedSH == true)*/.ToList<RequestItems_Table>()./*FindAll(dt=>dt.SubmitDate.ToString().Contains(year))*/FindAll(y => y.VKM_Year == int.Parse(year) + 1).FindAll(x => x.ApprovalSH == true);
                    DataTable dtOrder = new DataTable();
                    DataSet ds = new DataSet();
                    connection();
                    OpenConnection();
                    string Query = " Exec [dbo].[GetReqItemsList_VKMSPOCReviews_View] '" + (int.Parse(year) + 1) + "', '" + User.Identity.Name.Split('\\')[1].ToUpper() + "','View' ; Select ID, ltrim(rtrim(Description)) as Description from OrderStatusDescription Where IsVisible =1 Order by Description ; ";
                    //string Query = " Exec [dbo].[GetReqItemsList_VKMSPOCReviews_View] '" + year + "', 'MAE9COB','View' ";
                    SqlCommand cmd1 = new SqlCommand(Query, conn);
                    SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
                    da1.Fill(ds);
                    CloseConnection();

                    dtmain = ds.Tables[0];
                    dtOrder = ds.Tables[1];

                    foreach (DataRow row in dtOrder.Rows)
                    {
                        OrderStatusDescription item = new OrderStatusDescription();
                        item.Description = row["Description"].ToString().Trim();
                        item.ID = Convert.ToInt32(row["ID"].ToString());
                        BudgetingController.lstOrderDescription.Add(item);
                    }

                    

                    foreach (DataRow item in dtmain.Rows)
                    {
                        try
                        {
                            RequestItemsRepoView ritem = new RequestItemsRepoView();

                            //restrict data pertaining to BUs only to the BU Purchase SPOC - all BU related ReqList should not be shown to all Purchase SPOCs
                            //if (BudgetingController.lstPrivileged.Find(person => person.ADSID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())) != null)
                            //{

                            //    var BU_of_PurchaseSPOC = BudgetingController.lstPrivileged.Find(e => e.ADSID.ToUpper().Trim().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).BU;
                            //    if (BU_of_PurchaseSPOC != null)
                            //    {
                            //        allowedBUs = (BU_of_PurchaseSPOC.Split(',')).ToList();

                            //    }
                            //    if (allowedBUs.Contains(item.BU))
                            //    {
                            //        ritem.BU = int.Parse(item.BU);

                            //    }
                            //    else
                            //    {
                            //        continue;
                            //    }
                            //}
                            //else
                            //{
                            //    ritem.BU = int.Parse(item.BU);
                            //}

                            ritem.BU = int.Parse(item["BU"].ToString());
                            ritem.BudgetCode = item["BudgetCode"].ToString();
                            ritem.RequestorNTID = item["RequestorNTID"].ToString();


                            ritem.isProjected = (bool)((item["isProjected"] != null && item["isProjected"].ToString() != string.Empty) ? item["isProjected"] : false);
                            ritem.Q1 = (bool)((item["Q1"] != null && item["Q1"].ToString() != string.Empty) ? item["Q1"] : false);
                            ritem.Q2 = (bool)((item["Q2"] != null && item["Q2"].ToString() != string.Empty) ? item["Q2"] : false);
                            ritem.Q3 = (bool)((item["Q3"] != null && item["Q3"].ToString() != string.Empty) ? item["Q3"] : false);
                            ritem.Q4 = (bool)((item["Q4"] != null && item["Q4"].ToString() != string.Empty) ? item["Q4"] : false);
                            //ritem.Projected_Amount = item["Projected_Amount"] != null ? Convert.ToDecimal(item["Projected_Amount"].ToString()) : 0;
                            //ritem.Unused_Amount = item["Unused_Amount"] != null ? Convert.ToDecimal(item["Unused_Amount"].ToString()) : 0;



                            if (item["Projected_Amount"].ToString() != "" && item["Projected_Amount"] != null)
                                ritem.Projected_Amount = (item["Projected_Amount"].ToString() != "" && item["Projected_Amount"] != null) ? Math.Round((decimal)item["Projected_Amount"], MidpointRounding.AwayFromZero) : 0;

                            if (item["Unused_Amount"].ToString() != "" && item["Unused_Amount"] != null)
                                ritem.Unused_Amount = item["Unused_Amount"] != null ? Math.Round((decimal)item["Unused_Amount"], MidpointRounding.AwayFromZero) : 0;

                            if (item["UpdatedAt"].ToString() != "" && item["UpdatedAt"] != null)
                                ritem.UpdatedAt = Convert.ToDateTime(item["UpdatedAt"]);
                            ritem.VKM_Year = int.Parse(item["VKM_Year"].ToString());
                            //ritem.UpdatedAt = Convert.ToDateTime(item["UpdatedAt"].ToString());
                            ritem.PORemarks = item["PORemarks"].ToString();
                            ritem.VKM_Year = (int)item["VKM_Year"];
                            if (item["Category"] != null)
                                ritem.Category = int.Parse(item["Category"].ToString());
                            if (item["L3_Remarks"] != null)
                                ritem.Comments = item["L3_Remarks"].ToString();
                            if (item["CostElement"] != null)
                                ritem.Cost_Element = int.Parse(item["CostElement"].ToString());
                            ritem.DEPT = int.Parse(item["DEPT"].ToString());
                            ritem.Group = int.Parse(item["Group"].ToString());
                            ritem.Item_Name = int.Parse(item["ItemName"].ToString());
                            ritem.OEM = int.Parse(item["OEM"].ToString());
                            ritem.Required_Quantity = int.Parse(item["ReqQuantity"].ToString());
                            ritem.RequestID = int.Parse(item["RequestID"].ToString());
                            ritem.Requestor = item["RequestorNT"].ToString();
                            ritem.Reviewer_1 = item["DHNT"].ToString();
                            ritem.Reviewer_2 = item["SHNT"].ToString();
                            ritem.Total_Price = Convert.ToDecimal(item["TotalPrice"].ToString());
                            ritem.Unit_Price = Convert.ToDecimal(item["UnitPrice"].ToString());
                            ritem.Reviewed_Quantity = item["ApprQuantity"] == null ? (int)0 : (int)item["ApprQuantity"];
                            ritem.Reviewed_Cost = item["ApprCost"] == null ? 0 : (decimal)item["ApprCost"];
                            ritem.ApprovalHoE = (bool)item["ApprovalDH"];
                            ritem.ApprovalSH = (bool)item["ApprovalSH"];
                            ritem.ApprovedHoE = (bool)item["ApprovedDH"];
                            ritem.ApprovedSH = (bool)item["ApprovedSH"];
                            ritem.RequestDate = item["RequestDate"].ToString() != "" ? ((DateTime)item["RequestDate"]).ToString("yyyy-MM-dd") : string.Empty;
                            ritem.SubmitDate = item["SubmitDate"].ToString() != "" ? ((DateTime)item["SubmitDate"]).ToString("yyyy-MM-dd") : string.Empty;
                            ritem.Review1_Date = item["DHAppDate"].ToString() != "" ? ((DateTime)item["DHAppDate"]).ToString("yyyy-MM-dd") : string.Empty;
                            ritem.Review2_Date = item["SHAppDate"].ToString() != "" ? ((DateTime)item["SHAppDate"]).ToString("yyyy-MM-dd") : string.Empty;

                            ritem.ELOSubmittedDate = item["ELOSubmittedDate"].ToString() != "" ? ((DateTime)item["ELOSubmittedDate"]).ToString("yyyy-MM-dd") : string.Empty;
                            ritem.DaysTaken = item["DaysTaken"].ToString(); //!= "" ? item["DaysTaken"].ToString() : "-";
                            ritem.SRSubmitted = item["SRSubmitted"].ToString() != "" ? ((DateTime)item["SRSubmitted"]).ToString("yyyy-MM-dd") : string.Empty;
                            ritem.RFQNumber = item["RFQNumber"].ToString();
                            ritem.PRNumber = item["PRNumber"].ToString();
                            ritem.SRAwardedDate = item["SRawardedDate"].ToString() != "" ? ((DateTime)item["SRawardedDate"]).ToString("yyyy-MM-dd") : string.Empty;
                            ritem.SRResponsibleBuyerNTID = item["SR_responsibleBuyerNTID"].ToString();
                            ritem.SRManagerNTID = item["SR_ManagerNTID"].ToString();
                            ritem.POSpocNTID = item["POSpocNTID"].ToString();
                            ritem.SRApprovalDays = item["SRApprovalDays"].ToString(); //!= "" ? item["SRApprovalDays"].ToString() : "0";


                            ritem.BudgetCodeDescription = item["BudgetCodeDescription"].ToString();
                            ritem.OrderType = item["Order_Type"].ToString().Trim() != "" ? int.Parse(item["Order_Type"].ToString()) : 0;
                            ritem.CostCenter = item["CostCenter"].ToString();
                            ritem.BudgetCenterID = item["BudgetCenterID"].ToString().Trim() != "" ? int.Parse(item["BudgetCenterID"].ToString()) : 0;

                            ritem.LabName = item["LabName"].ToString();
                            ritem.RFOReqNTID = item["RFOReqNTID"].ToString();
                            ritem.RFOApprover = item["RFOApprover"].ToString();
                            ritem.GoodsRecID = item["GoodsRecID"].ToString();
                            ritem.UnitofMeasure = item["UnitofMeasure"].ToString().Trim() != "" ? int.Parse(item["UnitofMeasure"].ToString()) : 0;
                            ritem.UnloadingPoint = item["UnloadingPoint"].ToString().Trim() != "" ? int.Parse(item["UnloadingPoint"].ToString()) : 0;

                            ritem.QuoteAvailable = item["QuoteAvailable"].ToString();
                            ritem.Quote_Vendor_Type = item["Quote_Vendor_Type"].ToString();
                            ritem.Upload_File_Name = item["Upload_File_Name"].ToString();
                            ritem.Material_Part_Number = item["Material_Part_Number"].ToString();
                            ritem.SupplierName_with_Address = item["Supplier_Name_with_Address"].ToString();
                            ritem.Purchase_Type =  item["Purchase_Type"].ToString().Trim() != "" ? int.Parse(item["Purchase_Type"].ToString()) : 0;
                            ritem.Project_ID = item["Project_ID"].ToString();

                            ritem.OrderID = item["OrderID"].ToString();
                            
                            if(item["Currency"].ToString() != "" && item["Currency"].ToString().Trim() != "0")
                                ritem.Currency = int.Parse(item["Currency"].ToString()); //1; //OrderPrice Currency conversion - USD
                            if (item["OrderPrice"].ToString() != "")
                                ritem.OrderPrice = decimal.Parse(item["OrderPrice"].ToString());
                            if (item["OrderPrice_UserInput"].ToString() != "")
                                ritem.OrderPrice_UserInput = decimal.Parse(item["OrderPrice_UserInput"].ToString());
                            if (item["SR_Value"].ToString() != "")
                                ritem.SR_Value = decimal.Parse(item["SR_Value"].ToString());
                            if (item["PR_Value"].ToString() != "")
                                ritem.PR_Value = decimal.Parse(item["PR_Value"].ToString());
                            if (item["Invoice_Value"].ToString() != "")
                                ritem.Invoice_Value = decimal.Parse(item["Invoice_Value"].ToString());
                            //ritem.OrderPrice = Convert.ToDecimal(item["OrderPrice"].ToString().Trim()); //(decimal?)item["OrderPrice"];
                            //ritem.OrderPrice = decimal.Parse(item["OrderPrice"].ToString(), CultureInfo.InvariantCulture);
                            ritem.Project = item["Project"].ToString();
                            if (item["ActualAvailableQuantity"] == null || item["ActualAvailableQuantity"].ToString().Trim() == string.Empty)
                                ritem.ActualAvailableQuantity = "NA";
                            else
                                ritem.ActualAvailableQuantity = item["ActualAvailableQuantity"].ToString();

                            if (item["OrderStatus"].ToString().Trim() != "" && item["OrderStatus"].ToString().Trim() != "0")
                            {
                                ritem.OrderStatus = int.Parse(item["OrderStatus"].ToString());

                            }
                            else
                            {
                                ritem.OrderStatus = null;
                            }

                            ritem.RequestOrderDate = item["RequestOrderDate"].ToString() != "" ? ((DateTime)item["RequestOrderDate"]).ToString("yyyy-MM-dd") : string.Empty;
                            ritem.OrderID = item["OrderID"].ToString();
                            ritem.OrderDate = item["OrderDate"].ToString() != "" ? ((DateTime)item["OrderDate"]).ToString("yyyy-MM-dd") : string.Empty;
                            ritem.ActualDeliveryDate = item["ActualDeliveryDate"].ToString() != "" ? ((DateTime)item["ActualDeliveryDate"]).ToString("yyyy-MM-dd") : string.Empty;
                            ritem.RequiredDate = item["RequiredDate"].ToString() != "" ? ((DateTime)item["RequiredDate"]).ToString("yyyy-MM-dd") : string.Empty;
                            ritem.RequestOrderDate = item["RequestOrderDate"].ToString() != "" ? ((DateTime)item["RequestOrderDate"]).ToString("yyyy-MM-dd") : string.Empty;
                            ritem.OrderDate = item["OrderDate"].ToString() != "" ? ((DateTime)item["OrderDate"]).ToString("yyyy-MM-dd") : string.Empty;
                            ritem.TentativeDeliveryDate = item["TentativeDeliveryDate"].ToString() != "" ? ((DateTime)item["TentativeDeliveryDate"]).ToString("yyyy-MM-dd") : string.Empty;

                            if (item["Fund"].ToString() != "")
                            {
                                ritem.Fund = int.Parse(item["Fund"].ToString());
                            }
                            else
                            {
                                ritem.Fund = BudgetingController.lstFund.Find(fund => fund.Fund.Trim().Equals("F02")).ID;
                            }

                            if (item["RequestOrderDate"].ToString() != "")
                            {
                                ritem.RequestToOrder = true;
                            }
                            else
                            {
                                ritem.RequestToOrder = false;
                            }
                            if (item["OrderedQuantity"].ToString() != "")
                            {
                                ritem.OrderedQuantity = (int)item["OrderedQuantity"];
                            }
                            else
                            {
                                ritem.OrderedQuantity = null;
                            }


                            ritem.Customer_Dept = item["Customer_Dept"].ToString();

                            ritem.Customer_Name = item["Customer_Name"].ToString();

                            ritem.BM_Number = item["BM_Number"].ToString();

                            ritem.PIF_ID = item["PIF_ID"].ToString();

                            ritem.Resource_Group_Id = item["Resource_Group_Id"].ToString();

                            ritem.Task_ID = item["Task_ID"].ToString();

                            ritem.RequestSource = item["RequestSource"].ToString();
                            ritem.VKMSPOC_Approval = Convert.ToBoolean(item["VKMSPOC_Approval"]);

                            if (item["Description"].ToString().Trim() != "" && item["Description"].ToString().Trim() != "0")
                            {
                                ritem.Description = BudgetingController.lstOrderDescription.Find(x => x.ID.Equals(int.Parse(item["Description"].ToString()))).Description.ToString();

                                
                            }
                            else
                            {
                                ritem.Description = null;


                            }

                            if (item["LinkedRequests"].ToString().Trim() != "")
                            {
                                ritem.LinkedRequests = item["LinkedRequests"].ToString().Trim();
                            }
                            else
                            {
                                ritem.LinkedRequests = null;
                            }

                            ritem.LinkedRequestID = item["LinkedRequestID"].ToString();

                            viewList.Add(ritem);
                        }
                        catch (Exception ex)
                        {

                            WriteLog("Error - GetData1 - data fetch loop : NTID - " + User.Identity.Name.Split('\\')[1].Trim().ToUpper() + ", ErrorMsg : " + ex + ", At - " + DateTime.Now);

                            return viewList;
                        }

                    }


                    //string present = BudgetingController.lstUsers.Find(x => x.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).EmployeeName;
                    ////string presentUserName_2020 = BudgetingController.lstUsers_2020.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).EmployeeName;
                    //if (year.Contains("2020"))
                    //{
                    //    if (BudgetingController.lstBU_SPOCs.Find(person => person.VKMspoc.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())) != null &&
                    //       BudgetingController.lstBU_SPOCs.Find(person => person.VKMspoc.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).BU < 99)
                    //        return viewList.FindAll(y => y.VKM_Year == int.Parse(year) + 1) /*FindAll(x => x.SubmitDate.ToString().Contains(year))*/.FindAll(x => x.ApprovalSH == true).FindAll(x => x.Reviewer_2 != null).FindAll(x => x.Reviewer_2.Trim().Equals(present.Trim()) /*|| x.Reviewer_2.Trim().Equals(presentUserName_2020.Trim())*/);
                    //    else
                    //        return viewList.FindAll(y => y.VKM_Year == int.Parse(year) + 1)/*FindAll(x => x.SubmitDate.ToString().Contains(year))*/.FindAll(x => x.ApprovalSH == true);
                    //}

                    //string presentUserDept = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).Department;
                    //string is_CCXC = string.Empty;
                    //if (presentUserDept.Contains("XC"))
                    //    is_CCXC = "XC";
                    //else
                    //    is_CCXC = "CC";


                    ////not to be filtered as already based on bu filtered 
                    //if (BudgetingController.lstPrivileged.Find(person => person.ADSID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())) != null) //Purchase SPOCs - BU specific view 
                    //{
                    //    return viewList./*FindAll(x => x.SubmitDate.ToString().Contains(year))*/FindAll(y => y.VKM_Year == int.Parse(year) + 1).FindAll(x => x.ApprovalSH == true);
                    //    //    if (is_CCXC == "XC")
                    //    //        return viewList./*FindAll(x => x.SubmitDate.ToString().Contains(year))*/FindAll(y => y.VKM_Year == int.Parse(year) + 1).FindAll(x => x.ApprovalSH == true);   //.FindAll(dpt => BudgetingController.lstDEPTs.Find(xi => xi.ID.Equals(dpt.DEPT)).DEPT.Contains("XC")); //filter by CC/XC Dept
                    //    //    else
                    //    //        return viewList./*FindAll(x => x.SubmitDate.ToString().Contains(year))*/FindAll(y => y.VKM_Year == int.Parse(year) + 1).FindAll(x => x.ApprovalSH == true);//.FindAll(dpt => !BudgetingController.lstDEPTs.Find(xi => xi.ID.Equals(dpt.DEPT)).DEPT.Contains("XC")); //filter by CC/XC Dept


                    //}

                    //else if (BudgetingController.lstBU_SPOCs.Find(person => person.VKMspoc.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())) != null)
                    //{

                    //    //string query = " select distinct VKM_Spoc from VKMSpocHead where dept='" + is_CCXC + "' and  head= '" + User.Identity.Name.Split('\\')[1].ToUpper() + "' ";
                    //    string query = " select distinct VKM_Spoc from VKMSpocHead where dept='" + is_CCXC + "' and  head= '" + User.Identity.Name.Split('\\')[1].ToUpper() + "' and '" + User.Identity.Name.Split('\\')[1].ToUpper() + "' in (Select VKM_Spoc from VKMSpocHead where Ishead =1) ";
                    //    connection();
                    //    OpenConnection();
                    //    SqlCommand cmd = new SqlCommand(query, conn);
                    //    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    //    da.Fill(dt);
                    //    CloseConnection();

                    //    if (dt.Rows.Count == 1 || dt.Rows.Count == 0)
                    //    {
                    //        var BU = BudgetingController.lstBU_SPOCs.Find(person => person.VKMspoc.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).BU;
                    //        if (BU >= 99)                                                                                                                                  //VKM Admins of CC/XC
                    //        {
                    //            if (is_CCXC == "XC")
                    //            {
                    //                return viewList./*FindAll(x => x.SubmitDate.ToString().Contains(year))*/FindAll(y => y.VKM_Year == int.Parse(year) + 1).FindAll(x => x.ApprovalSH == true).FindAll(dpt => BudgetingController.lstDEPTs.Find(xi => xi.ID.Equals(dpt.DEPT)).DEPT.Contains("XC"))/*.FindAll(x => x.ApprovedSH != true)*/;

                    //        }
                    //        else
                    //        {
                    //            return viewList./*FindAll(x => x.SubmitDate.ToString().Contains(year))*/FindAll(y => y.VKM_Year == int.Parse(year) + 1).FindAll(x => x.ApprovalSH == true).FindAll(dpt => !BudgetingController.lstDEPTs.Find(xi => xi.ID.Equals(dpt.DEPT)).DEPT.Contains("XC"))/*.FindAll(x => x.ApprovedSH != true)*/;

                    //        }
                    //    }
                    //    else                                                                                                                                           //VKM SPOCs
                    //    {
                    //        return viewList./*FindAll(x => x.SubmitDate.ToString().Contains(year))*/FindAll(y => y.VKM_Year == int.Parse(year) + 1).FindAll(x => x.Reviewer_2 != null).FindAll(x => x.Reviewer_2.Trim().Contains(present.Trim()) /*|| x.Reviewer_2.Trim().Equals(presentUserName_2020.Trim())*/).FindAll(x => x.ApprovalSH == true)/*.FindAll(x => x.ApprovedSH != true)*/;
                    //    }
                    //    }
                    //    else
                    //    {
                    //        string present1 = "";
                    //        for (int i = 0; i < dt.Rows.Count; i++)
                    //        {
                    //            present1 = BudgetingController.lstUsers.Find(x => x.NTID.Trim().ToUpper().Equals(dt.Rows[i]["VKM_Spoc"].ToString().ToUpper())).EmployeeName;

                    //            viewList1.AddRange(viewList./*FindAll(x => x.SubmitDate.ToString().Contains(year))*/FindAll(y => y.VKM_Year == int.Parse(year) + 1).FindAll(x => x.Reviewer_2 != null).FindAll(x => x.Reviewer_2.Trim().Contains(present1.Trim()) /*|| x.Reviewer_2.Trim().Equals(presentUserName_2020.Trim())*/).FindAll(x => x.ApprovalSH == true).ToList());
                    //        }
                    //        //return viewList./*FindAll(x => x.SubmitDate.ToString().Contains(year))*/FindAll(y => y.VKM_Year == int.Parse(year) + 1).FindAll(x => x.Reviewer_2 != null).FindAll(x => x.Reviewer_2.Trim().Contains(present.Trim()) /*|| x.Reviewer_2.Trim().Equals(presentUserName_2020.Trim())*/).FindAll(x => x.ApprovalSH == true)/*.FindAll(x => x.ApprovedSH != true)*/;
                    //        return viewList1;
                    //    }
                    //}                                                                                                                                                  //VKM SPOC View Authorized users
                    //else //other autorized users
                    //{
                    //    if (is_CCXC == "XC")
                    //    {
                    //        return viewList./*FindAll(x => x.SubmitDate.ToString().Contains(year))*/FindAll(y => y.VKM_Year == int.Parse(year) + 1).FindAll(x => x.ApprovalSH == true).FindAll(bu=>bu.BU == 2 || bu.BU == 4);

                    //    }
                    //    else
                    //    {
                    //        return viewList./*FindAll(x => x.SubmitDate.ToString().Contains(year))*/FindAll(y => y.VKM_Year == int.Parse(year) + 1).FindAll(x => x.ApprovalSH == true).FindAll(dpt => !BudgetingController.lstDEPTs.Find(xi => xi.ID.Equals(dpt.DEPT)).DEPT.Contains("XC"));

                    //    }
                    //}

                    return viewList;
                }

            }
            catch (Exception ex)
            {

                WriteLog("Error - GetData1 : NTID - " + User.Identity.Name.Split('\\')[1].Trim().ToUpper() + ", ErrorMsg : " + ex + ", At - " + DateTime.Now);

                return viewList;
            }

        }

        /// <summary>
        /// /////// Getting details of L2
        /// </summary>
        /// <param name="RequestID"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetL2Details(string RequestID)
        {

            DataTable dt = new DataTable();
            string DHAppDate = "", L2Remarks = "", SubmitDate = "", L1Remarks = "", L2Qty = "", L1Qty = "";

            try
            {
                string query = " select convert(nvarchar,DHAppDate) as ApprovedDate,L2_Remarks as L2Remarks,Comments as L1Remarks,convert(nvarchar,SubmitDate) as SubmitDate, ReqQuantity,L2_Qty from RequestItems_Table where RequestID = '" + RequestID + "' ";

                connection();
                OpenConnection();
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                CloseConnection();

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DHAppDate = dt.Rows[i]["ApprovedDate"].ToString();
                        L2Remarks = dt.Rows[i]["L2Remarks"].ToString();
                        SubmitDate = dt.Rows[i]["SubmitDate"].ToString();
                        L1Remarks = dt.Rows[i]["L1Remarks"].ToString();
                        L2Qty = dt.Rows[i]["L2_Qty"].ToString();
                        L1Qty = dt.Rows[i]["ReqQuantity"].ToString();
                    }
                }


                var result = new { L2ReviewDate = DHAppDate, L2Remarks = L2Remarks, L1Remarks = L1Remarks, L1SubmitDate = SubmitDate, L1Qty = L1Qty, L2Qty = L2Qty };
                return Json(new { success = true, data = result }, JsonRequestBehavior.AllowGet);
                //return Json(new { data = new { RequestDate = RequestDate, L1Remarks = L1Remarks } }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {

                return Json(new { success = false, data = "" }, JsonRequestBehavior.AllowGet);
            }


        }

        public ActionResult ConfigureRFOMail()
        {
            return View();
        }

        public ActionResult ReportHomePage()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetMainReportData (string budgetcode, string currency)
        {
            try
            {
                DataSet ds = new DataSet();
                DataSet dsBGSW = new DataSet();
                connection();
                string Query = " EXEC [dbo].[GetBudgetSummary] '" + User.Identity.Name.Split('\\')[1].ToUpper() + "','" + budgetcode + "','" + currency + "','TopSection'  ";
                OpenConnection();
                SqlCommand cmd = new SqlCommand(Query, conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);

                Query = " EXEC [dbo].[GetBGSWBudgetSummary] '" + User.Identity.Name.Split('\\')[1].ToUpper() + "','" + budgetcode + "','" + currency + "','BGSW'  ";
                OpenConnection();
                cmd = new SqlCommand(Query, conn);
                da = new SqlDataAdapter(cmd);
                da.Fill(dsBGSW);

                CloseConnection();


                Dictionary<string, List<Dictionary<string, object>>> simplifiedData = new Dictionary<string, List<Dictionary<string, object>>>();

                for (int i = 0; i < dsBGSW.Tables.Count; i++)
                {
                    string tableName = "Table" + (i + 1); // Generate dynamic table name
                    List<Dictionary<string, object>> tableData = new List<Dictionary<string, object>>();
                    foreach (DataRow row in dsBGSW.Tables[i].Rows)
                    {
                        var rowData = new Dictionary<string, object>();
                        foreach (DataColumn col in dsBGSW.Tables[i].Columns)
                        {
                            rowData[col.ColumnName] = row[col];
                        }
                        tableData.Add(rowData);
                    }

                    simplifiedData.Add(tableName, tableData);
                }

                JavaScriptSerializer serializer = new JavaScriptSerializer();
                serializer.MaxJsonLength = Int32.MaxValue;
                serializer.RecursionLimit = 100;
                string bgswjsonResult = serializer.Serialize(simplifiedData);

                simplifiedData = new Dictionary<string, List<Dictionary<string, object>>>();

                for (int i = 0; i < ds.Tables.Count; i++)
                {
                    string tableName = "Table" + (i + 1); // Generate dynamic table name
                    List<Dictionary<string, object>> tableData = new List<Dictionary<string, object>>();
                    foreach (DataRow row in ds.Tables[i].Rows)
                    {
                        var rowData = new Dictionary<string, object>();
                        foreach (DataColumn col in ds.Tables[i].Columns)
                        {
                            rowData[col.ColumnName] = row[col];
                        }
                        tableData.Add(rowData);
                    }

                    simplifiedData.Add(tableName, tableData);
                }

                serializer = new JavaScriptSerializer();
                serializer.MaxJsonLength = Int32.MaxValue;
                serializer.RecursionLimit = 100;
                string jsonResult = serializer.Serialize(simplifiedData);


                return Json(new { success = true, data = jsonResult, bgswdata = bgswjsonResult , tables = ds.Tables.Count }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                CloseConnection();
                return Json(new { success = false , data = "", bgswdata ="" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult BudgetCodeList()
        {
            try

            {
                List<BudgetCodeList> budgetcodelist = new List<BudgetCodeList>();
                connection();
                //string Query = "Select Budget_Code as Code, Budget_Code_Description as Description from BGSW_BudgetCode_Table Where CostElementID = 1 order by Budget_Code_Description ";
                string Query = "Select ID as Code, CostElement as Description from CostElement_Table Where ID = 1 order by ID ";
                OpenConnection();
                SqlCommand cmd = new SqlCommand(Query, conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                CloseConnection();
                var presentUserNTID = User.Identity.Name.Split('\\')[1].ToUpper();
                foreach (DataRow row in dt.Rows)
                {
                    BudgetCodeList item = new BudgetCodeList();
                    //dr.Read();

                    item.Code = row["Code"].ToString();
                    item.Description = row["Description"].ToString();

                    budgetcodelist.Add(item);

                }
                CloseConnection();

                return Json(budgetcodelist, JsonRequestBehavior.AllowGet);
                //dr.Close();
                //CloseConnection();

            }
            catch (Exception ex)
            {
                CloseConnection();
                //WriteLog("Error - GetBudgetCode : NTID - " + User.Identity.Name.Split('\\')[1].Trim().ToUpper() + ", ErrorMsg : " + ex + ", At - " + DateTime.Now);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult GetCurrencyList()
        {
            try

            {
                List<CurrencyList> currencylist = new List<CurrencyList>();
                connection();
                string Query = " Select ID , Currency from Currency_Table Order by Currency; ";
                OpenConnection();
                SqlCommand cmd = new SqlCommand(Query, conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                CloseConnection();
                var presentUserNTID = User.Identity.Name.Split('\\')[1].ToUpper();
                foreach (DataRow row in dt.Rows)
                {
                    CurrencyList item = new CurrencyList();
                    //dr.Read();

                    item.ID = row["ID"].ToString();
                    item.Currency = row["Currency"].ToString();

                    currencylist.Add(item);

                }
                CloseConnection();

                return Json(currencylist, JsonRequestBehavior.AllowGet);
                //dr.Close();
                //CloseConnection();

            }
            catch (Exception ex)
            {
                CloseConnection();
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

        [HttpGet]
        public ActionResult SectionList()
        {
            try

            {
                List<SectionList> sectionlist = new List<SectionList>();
                connection();
                string Query = "select distinct section from SPOTONData_Table_2022 union select distinct Department from SPOTONData_Table_2022 ";
                OpenConnection();
                SqlCommand cmd = new SqlCommand(Query, conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                CloseConnection();
                var presentUserNTID = User.Identity.Name.Split('\\')[1].ToUpper();
                foreach (DataRow row in dt.Rows)
                {
                    SectionList item = new SectionList();
                    //dr.Read();

                    item.Section = row["Section"].ToString();

                    sectionlist.Add(item);

                }
                CloseConnection();

                return Json(sectionlist, JsonRequestBehavior.AllowGet);
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
        public ActionResult GetEmployeeName(string Ntid) ////// NTID is passed instead of Employee name
        {

            DataTable dt = new DataTable();
            connection();

            string NTID1 = "", EmployeeName = "";
            string Query = " select Top 1 NTID,EmployeeName from [SPOTONData_Table_2022] Where NTID = '" + Ntid + "'  ";


            //CHECK RETURN
            //CHECK CASE WHEN - same table/inner join values chk n return
            try
            {
                OpenConnection();
                SqlCommand cmd = new SqlCommand(Query, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                //if (dr.HasRows)
                if (dr.FieldCount == 1)
                    return Json(new { success = false, msg = "The NTID details are already available in VKM Input Collection list, Please check again!", JsonRequestBehavior.AllowGet });
                else if (dr.HasRows == false)
                    return Json(new { success = false, msg = "", JsonRequestBehavior.AllowGet });
                //return Json(new { success = false, msg = "The NTID is InValid, Please check again!", JsonRequestBehavior.AllowGet });

                while (dr.Read())
                {
                    //dr.Read();
                    NTID1 = dr["NTID"].ToString();
                    EmployeeName = dr["EmployeeName"].ToString();
                }
                dr.Close();
                CloseConnection();

            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
            return Json(new { success = true, NTID = NTID1, EmployeeName = EmployeeName , JsonRequestBehavior.AllowGet });

        }

        [HttpPost]
        public ActionResult GetNTID(string EmpName) ////// Employee name is passed instead of NTID
        {

            DataTable dt = new DataTable();
            connection();

            string NTID1 = "", EmployeeName = "";
            string Query = " select Top 1 NTID,EmployeeName from [SPOTONData_Table_2022] Where EmployeeName = '" + EmpName + "'  ";


            //CHECK RETURN
            //CHECK CASE WHEN - same table/inner join values chk n return
            try
            {
                OpenConnection();
                SqlCommand cmd = new SqlCommand(Query, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                //if (dr.HasRows)
                if (dr.FieldCount == 1)
                    return Json(new { success = false, msg = "The NTID details are already available in VKM Input Collection list, Please check again!", JsonRequestBehavior.AllowGet });
                else if (dr.HasRows == false)
                    return Json(new { success = false, msg = "", JsonRequestBehavior.AllowGet });
                //return Json(new { success = false, msg = "The NTID is InValid, Please check again!", JsonRequestBehavior.AllowGet });

                while (dr.Read())
                {
                    //dr.Read();
                    NTID1 = dr["NTID"].ToString();
                    EmployeeName = dr["EmployeeName"].ToString();
                }
                dr.Close();
                CloseConnection();

            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
            return Json(new { success = true, NTID = NTID1, EmployeeName = EmployeeName, JsonRequestBehavior.AllowGet });

        }

        [HttpGet]
        public ActionResult GetRFOMailDetails()
        {
            try
            {
                List<RFOApproverDetails> rfoApproverDetails = new List<RFOApproverDetails>();
                connection();
                //string Query = "select distinct section from SPOTONData_Table_2022 union select distinct Department from SPOTONData_Table_2022 union select distinct [group] from SPOTONData_Table_2022";
                string Query = "Select ID, Section, NTID, EmployeeName, IsPaused from BGSW_RFOMailConfiguration where 1=1 ";
                OpenConnection();
                SqlCommand cmd = new SqlCommand(Query, conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                CloseConnection();

                if (dt.Rows.Count > 0)
                {
                    for (int i=0; i< dt.Rows.Count; i++)
                    {
                        RFOApproverDetails rfo = new RFOApproverDetails();
                        rfo.ID =Convert.ToInt32(dt.Rows[i]["ID"].ToString());
                        rfo.Section = dt.Rows[i]["Section"].ToString();
                        rfo.NTID = dt.Rows[i]["NTID"].ToString();
                        rfo.EmployeeName = dt.Rows[i]["EmployeeName"].ToString();
                        rfo.IsPaused = Convert.ToBoolean(dt.Rows[i]["IsPaused"]);
                        rfoApproverDetails.Add(rfo);
                    }
                }


                return Json(new { success = true, data = rfoApproverDetails }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                CloseConnection();
                return Json(new { success = false, data = "" }, JsonRequestBehavior.AllowGet);
            }

        }


        /// <summary>
        /// CTG Validation - to check whether the Approved budget is sufficient or not
        /// </summary>
        /// <param name="NTID"></param>
        /// <param name="ApprovedAmount"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ValidateCTGAmount(string NTID, decimal ApprovedAmount, string Dept)
        {
            bool isExceeded = false;
            decimal CTGAmount = 0;
            try
            {
                connection();
                string Query = "";
                Query = "Declare @CTGBalance decimal(18,2); Exec [dbo].[GetCTGBalance] '" + NTID + "','" + Dept + "',@CTGBalance OUTPUT ; Select @CTGBalance as CTGBalance; ";
                OpenConnection();
                SqlCommand cmd = new SqlCommand(Query, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    CTGAmount = Convert.ToDecimal(dr["CTGBalance"]);
                }
                dr.Close();
                CloseConnection();

                if (CTGAmount < ApprovedAmount)
                {
                    isExceeded = true;
                }
                else
                {
                    isExceeded = false;
                }
                return Json(new { success = true, isExceeded, JsonRequestBehavior.AllowGet });

            }
            catch (Exception ex)
            {
                CloseConnection();
                return Json(new { success = false, isExceeded, JsonRequestBehavior.AllowGet });

            }
            
        }

        /// <summary>
        /// Get CTG details from the database maintained in dept wise and section wise 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetCTGDetails()
        {
            try
            {
                List<CTGDetails> ctgdetails = new List<CTGDetails>();
                connection();
                //string Query = "select distinct section from SPOTONData_Table_2022 union select distinct Department from SPOTONData_Table_2022 union select distinct [group] from SPOTONData_Table_2022";
                //string Query = "Select ID, Section_Dept as Section, Amount,isnull(Approved,0) as Approved, isnull(Utilized,0) as Utilized,isnull(Balance,0) as Balance from BGSW_CTG where 1=1 ";
                string Query = "EXEC [dbo].[GetCTGDetails] '1' ";
                OpenConnection();
                SqlCommand cmd = new SqlCommand(Query, conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                CloseConnection();

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        CTGDetails item = new CTGDetails();
                        item.ID = Convert.ToInt32(dt.Rows[i]["ID"].ToString());
                        item.Section = dt.Rows[i]["Section"].ToString();
                        item.Amount = Convert.ToDecimal(dt.Rows[i]["Amount"]);
                        item.Approved = Convert.ToDecimal(dt.Rows[i]["Approved"]);
                        item.Utilized = Convert.ToDecimal(dt.Rows[i]["Utilized"]);
                        item.Balance = Convert.ToDecimal(dt.Rows[i]["Balance"]);
                        ctgdetails.Add(item);
                    }
                }


                return Json(new { success = true, data = ctgdetails }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                CloseConnection();
                return Json(new { success = false, data = "" }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        public ActionResult Get_IsVKMSpoc()
        {
            DataTable dt = new DataTable();
            try
            {
                string is_VKMSpoc = "", query = "";
                string popupedit = "";
                string gridedit = "";
                string gridadd = "";

                //string presentUserDept = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).Department;
                string presentUserDept = "", presentUserTopSection = "", presentUserSection = "";
                string is_CCXC = string.Empty;
                //if (presentUserDept.Contains("XC"))
                //    is_CCXC = "XC";
                //else
                //    is_CCXC = "CC";



                connection();
                string qry = "select TopSection,Section, Department from SPOTONData_Table_2022 where NTID = '" + User.Identity.Name.Split('\\')[1].ToUpper() + "' ";

                SqlDataReader dr1;
                OpenConnection();
                SqlCommand cmd1 = new SqlCommand(qry, conn);
                dr1 = cmd1.ExecuteReader();
                if (dr1.HasRows)
                {
                    dr1.Read();
                    presentUserDept = dr1["Department"].ToString();
                    presentUserSection = dr1["Section"].ToString();
                    presentUserTopSection = dr1["TopSection"].ToString();
                }
                dr1.Close();




                OpenConnection();
                query = " Select VKM_Spoc from BGSW_VKMSpocHead where VKM_Spoc = '" + User.Identity.Name.Split('\\')[1].ToUpper() + "' and TopSection = '" + presentUserTopSection + "' and isHead = 1 ";
                //query = " Select VKM_Spoc from VKMSpocHead where VKM_Spoc = '" + User.Identity.Name.Split('\\')[1].ToUpper() + "' and Dept = '" + is_CCXC + "' and isHead = 1 ";
                //query = " select top 1 VKMSpoc from BU_SPOCS where VKMspoc = '" + User.Identity.Name.Split('\\')[1].ToUpper() + "' and bu in (1,2,3,4,5) ";
                //query = " select top 1 VKMSpoc from BU_SPOCS where VKMspoc = 'din2cob' and bu in (1,2,3,4,5) ";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                CloseConnection();

                if (dt.Rows.Count > 0)
                {
                    is_VKMSpoc = "1";
                }
                else
                {
                    is_VKMSpoc = "0";
                }

                string Query1 = "  select top(1) [PurchaseSpoc Add],[PurchaseSpoc Edit],[PurchaseSpoc Edit Grid] from Mail_RFOAuthorization_Table where Section in (Select value from STRING_SPLIT((select Section from POSPOC_Team where NTID = '" + User.Identity.Name.Split('\\')[1].ToUpper() + "'),',')) OR TopSection in (Select value from STRING_SPLIT((select Section from POSPOC_Team where NTID = '" + User.Identity.Name.Split('\\')[1].ToUpper() + "'),','))";
                OpenConnection();
                cmd1 = new SqlCommand(Query1, conn);
                SqlDataReader dr = cmd1.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    gridadd = dr["PurchaseSpoc Add"].ToString();
                    popupedit = dr["PurchaseSpoc Edit"].ToString();
                    gridedit = dr["PurchaseSpoc Edit Grid"].ToString();

                }
                dr.Close();

                CloseConnection();
                return Json(new { success = true, data = is_VKMSpoc, popupedit, gridedit, gridadd }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                //dr.Close();
                return Json(new { success = false, data = "0" }, JsonRequestBehavior.AllowGet);
            }
        }

        //[HttpPost]
        //public ActionResult UpdateTimeline(string Days, string Hours, string Minutes)
        //{
        //    try
        //    {
        //        DataTable dt = new DataTable();
        //        string is_VKMSpoc = "", query = "";

        //        string is_CCXC = string.Empty;
        //        string presentUserDept = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).Department;

        //        if (presentUserDept.Contains("XC"))
        //            is_CCXC = "XC";
        //        else
        //            is_CCXC = "CC";

        //        connection();
        //        OpenConnection();
        //        query = " Select VKM_Spoc from VKMSpocHead where VKM_Spoc = '" + User.Identity.Name.Split('\\')[1].ToUpper() + "' and Dept = '" + is_CCXC + "' and isHead = 1 ";
        //        //query = " select top 1 VKMSpoc from BU_SPOCS where VKMspoc = '" + User.Identity.Name.Split('\\')[1].ToUpper() + "' and bu in (1,2,3,4,5) ";
        //        SqlCommand cmd = new SqlCommand(query, conn);
        //        SqlDataAdapter da = new SqlDataAdapter(cmd);
        //        da.Fill(dt);
        //        CloseConnection();

        //        if (dt.Rows.Count > 0)
        //        {
        //            is_VKMSpoc = "1";
        //        }
        //        else
        //        {
        //            is_VKMSpoc = "0";
        //        }

        //        if (is_VKMSpoc == "1")
        //        {
        //            string qry = " EXEC [dbo].[UpdateTimelineSettings] '" + Days + "', '" + Hours + "', '" + Minutes + "','" + is_CCXC + "' ";

        //            connection();
        //            OpenConnection();
        //            cmd = new SqlCommand(qry, conn);
        //            cmd.ExecuteNonQuery();
        //            CloseConnection();
        //            return Json(new { success = true, data = "1" }, JsonRequestBehavior.AllowGet);
        //        }
        //        else
        //        {
        //            return Json(new { success = false, data = "0" }, JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { success = false, data = "" }, JsonRequestBehavior.AllowGet);
        //    }
        //}


        [HttpPost]
        public ActionResult UpdateTimeline(string Days, string Hours, string Minutes)
        {
            try
            {
                DataTable dt = new DataTable();
                string is_VKMSpoc = "", query = "";

                //string is_CCXC = string.Empty;
                //string presentUserDept = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).Department;
                //string presentUserTopSection = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).TopSection;

                string presentUserDept = "", presentUserTopSection = "", presentUserSection = "";
                //string is_CCXC = string.Empty;
                //if (presentUserDept.Contains("XC"))
                //    is_CCXC = "XC";
                //else
                //    is_CCXC = "CC";



                connection();
                string qry = "select TopSection,Section, Department from SPOTONData_Table_2022 where NTID = '" + User.Identity.Name.Split('\\')[1].ToUpper() + "' ";

                SqlDataReader dr1;
                OpenConnection();
                SqlCommand cmd1 = new SqlCommand(qry, conn);
                dr1 = cmd1.ExecuteReader();
                if (dr1.HasRows)
                {
                    dr1.Read();
                    presentUserDept = dr1["Department"].ToString();
                    presentUserSection = dr1["Section"].ToString();
                    presentUserTopSection = dr1["TopSection"].ToString();
                }
                dr1.Close();
                CloseConnection();

                //connection();
                OpenConnection();
                query = " Select VKM_Spoc from BGSW_VKMSpocHead where VKM_Spoc = '" + User.Identity.Name.Split('\\')[1].ToUpper() + "' and TopSection = '" + presentUserTopSection + "' and isHead = 1 ";
                //query = " select top 1 VKMSpoc from BU_SPOCS where VKMspoc = '" + User.Identity.Name.Split('\\')[1].ToUpper() + "' and bu in (1,2,3,4,5) ";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                CloseConnection();

                if (dt.Rows.Count > 0)
                {
                    is_VKMSpoc = "1";
                }
                else
                {
                    is_VKMSpoc = "0";
                }

                if (is_VKMSpoc == "1")
                {
                    qry = " EXEC [dbo].[BGSW_UpdateTimelineSettings] '" + Days + "', '" + Hours + "', '" + Minutes + "','" + presentUserTopSection + "' ";

                    connection();
                    OpenConnection();
                    cmd = new SqlCommand(qry, conn);
                    cmd.ExecuteNonQuery();
                    CloseConnection();
                    return Json(new { success = true, data = "1" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new { success = false, data = "0" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, data = "" }, JsonRequestBehavior.AllowGet);
            }
        }




        /// <summary>
        /// function to get the list of items for L3 approval
        /// filtered by user name and year chosen by user
        /// /// /// <param name="year"></param>
        /// </summary>
        /// <returns></returns>
        public ActionResult GetData(string year)
        {

            using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

            //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

            {
                try
                {

                    List<RequestItemsRepoView> viewList = new List<RequestItemsRepoView>();
                    viewList = GetData1(year);


                    string present = BudgetingController.lstUsers.Find(x => x.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).EmployeeName;

                    if (BudgetingController.lstPrivileged.Find(person => person.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())) != null)
                    {
                        return Json(new { data = viewList, message = "PurchaseSPOC" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    if (BudgetingController.lstBU_SPOCs.Find(person => person.VKMspoc.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())) != null)
                    {
                        var BU = BudgetingController.lstBU_SPOCs.Find(person => person.VKMspoc.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).BU_ID;
                        int res;
                        if (int.TryParse(BU, out res) && int.Parse(BU) >= 99)
                        {
                            return Json(new { data = viewList, message = "VKMAdmin" }, JsonRequestBehavior.AllowGet);
                        }
                        else
                        {
                            return Json(new
                            {
                                data = viewList,
                                message = "VKM SPOC"
                            }, JsonRequestBehavior.AllowGet);

                        }



                    }
                    else
                    {
                        return Json(new
                        {
                            data = viewList,
                            message = "VKMAdmin"
                        }, JsonRequestBehavior.AllowGet);

                    }
                }
                catch (Exception ex)
                {

                    WriteLog("Error - GetData: NTID - " + User.Identity.Name.Split('\\')[1].Trim().ToUpper() + ", ErrorMsg : " + ex + ", At - " + DateTime.Now);

                    return Json(new { success = false, message = "Unable to load the Item Requests, Please Try again later!" }, JsonRequestBehavior.AllowGet);

                }

            }
        }

        [HttpGet]
        public ActionResult InitRowValues()
        {

            var UserNTID = System.Web.HttpContext.Current.User.Identity.Name.Split('\\')[1].ToUpper();
            string Query = "";
            try
            {
                connection();
                RequestItemsRepoEdit1 temp = new RequestItemsRepoEdit1();

                temp.RequestDate = DateTime.Now.ToString("yyyy-MM-dd");
                temp.POSpocNTID = User.Identity.Name.Split('\\')[1].ToUpper();
                Query = " Exec [dbo].[LEPlanner_InitRowValues] '" + UserNTID + "' ";
                //"IF EXISTS(SELECT RequestorNTID from RequestItems_Table where RequestorNTID = @User)SELECT TOP 1 BU , OEM from RequestItems_Table where RequestorNTID = @User order by UpdatedAt desc";
                OpenConnection();
                SqlCommand cmd = new SqlCommand(Query, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    temp.Requestor = dr["UserName"].ToString();
                    temp.DEPT = int.Parse(dr["UserDept_ID"].ToString());
                    temp.Group = int.Parse(dr["UserGroup_ID"].ToString());
                    temp.Reviewer_1 = dr["UserHOE_Name"].ToString();
                    temp.BU = int.Parse(dr["BU"].ToString());
                    temp.OEM = int.Parse(dr["OEM"].ToString());
                    temp.Reviewer_2 = dr["UserVKMSPOC_Name"].ToString();
                    temp.Reviewed_Quantity = 0;
                    temp.Reviewed_Cost = 0;
                }
                else
                {
                    temp.Requestor = "";
                    temp.DEPT = 0;
                    temp.Group = 0;
                    temp.Reviewer_1 = "NA";
                    temp.BU = 0;
                    temp.OEM = 0;
                    temp.Reviewer_2 = "";
                    temp.Reviewed_Quantity = 0;
                    temp.Reviewed_Cost = 0;

                }
                if (temp.Reviewer_1.Trim() != "NA")
                {

                    return Json(new { data = temp, success = true }, JsonRequestBehavior.AllowGet);
                }
                else
                {

                    return Json(new { data = UserNTID, success = false }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                WriteLog("Error - InitRowValues : NTID - " + User.Identity.Name.Split('\\')[1].Trim().ToUpper() + ", ErrorMsg : " + ex + ", At - " + DateTime.Now);
                return Json(new { success = false, data = UserNTID }, JsonRequestBehavior.AllowGet);
            }
            finally
            {
                CloseConnection();
            }
            //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

            ////using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

            //{

            //    RequestItemsRepoEdit1 temp = new RequestItemsRepoEdit1();
            //    try
            //    {
            //        System.Security.Principal.IPrincipal User = System.Web.HttpContext.Current.User;
            //        SPOTONData_Table_2022 PresentUser = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper()));
            //        string presentUserName = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).EmployeeName;
            //        //string L2ReviewerName = string.Empty;
            //        //string L3ReviewerName = string.Empty;
            //        //try
            //        //{
            //        //    L2ReviewerName = BudgetingController.lstUsers.FindAll(user => PresentUser.Department.Trim().ToUpper().Equals(user.Group.ToUpper())).FirstOrDefault(head => head.Role.ToUpper().Contains("HEAD")).EmployeeName;
            //        //}
            //        //catch (Exception ex)
            //        //{
            //        //    L2ReviewerName = "NA";
            //        //}
            //        //try
            //        //{
            //        //    L3ReviewerName = BudgetingController.lstUsers.FindAll(user => PresentUser.Section.Trim().ToUpper().Contains(user.Section.ToUpper())).FirstOrDefault(head => head.Role.ToUpper().Contains("SECTION")).EmployeeName;
            //        //}
            //        //catch (Exception ex)
            //        //{
            //        //    L3ReviewerName = "NA";
            //        //}
            //        temp.POSpocNTID = User.Identity.Name.Split('\\')[1].ToUpper();
            //        temp.Requestor = presentUserName;
            //        //temp.Reviewer_1 = L2ReviewerName;
            //        //temp.Reviewer_2 = L3ReviewerName;
            //        DEPT_Table dEPT_Table = BudgetingController.lstDEPTs.Find(departmnt => departmnt.DEPT.Equals(BudgetingController.lstUsers.Find(x => x.EmployeeName == presentUserName).Department));
            //        temp.DEPT = dEPT_Table.ID;
            //        string Selected_Dept = BudgetingController.lstDEPTs.Find(dpt => dpt.ID.Equals(dEPT_Table.ID)).DEPT.Trim().ToUpper();
            //        try
            //        {
            //            temp.Reviewer_1 = BudgetingController.lstUsers.FindAll(user => Selected_Dept.Equals(user.Group.ToUpper())).FirstOrDefault(head => head.Role.ToUpper().Contains("HEAD")).EmployeeName;
            //        }
            //        catch (Exception ex)
            //        {
            //            temp.Reviewer_1 = "NA";
            //        }

            //        var PresentUserGroup = BudgetingController.lstUsers.Find(x => x.EmployeeName == presentUserName).Group;
            //        Groups_Table_Test gROUP_Table = BudgetingController.lstGroups_test.Find(grp => grp.Group.Trim().Equals(PresentUserGroup));
            //        temp.Group = gROUP_Table.ID;
            //        //temp.Group = ;
            //        temp.RequestDate = DateTime.Now.ToString("yyyy-MM-dd");

            //        var UserNTID = System.Web.HttpContext.Current.User.Identity.Name.Split('\\')[1].ToUpper();
            //        string Query = "";
            //        bool success = false;
            //        connection();
            //        Query = "IF EXISTS(SELECT RequestorNTID from RequestItems_Table where RequestorNTID = @User)SELECT TOP 1 BU , OEM from RequestItems_Table where RequestorNTID = @User order by UpdatedAt desc";
            //        OpenConnection();
            //        SqlCommand cmd = new SqlCommand(Query, conn);
            //        cmd.Parameters.AddWithValue("@User ", UserNTID);
            //        SqlDataReader dr = cmd.ExecuteReader();
            //        if (dr.HasRows)
            //        {
            //            dr.Read();
            //            temp.BU = int.Parse(dr["BU"].ToString());
            //            temp.OEM = int.Parse(dr["OEM"].ToString());
            //            temp.Reviewed_Quantity = 0;
            //            temp.Reviewed_Cost = 0;
            //            temp.Reviewer_2 = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(BudgetingController.lstBU_SPOCs.Find(spoc => spoc.BU.Equals(temp.BU)).VKMspoc.ToUpper().Trim())).EmployeeName;
            //            ;
            //            CloseConnection();

            //        }
            //        else
            //        {
            //            temp.BU = 0;
            //            temp.OEM = 0;
            //            temp.Reviewer_2 = "";
            //            temp.Reviewed_Quantity = 0;
            //            temp.Reviewed_Cost = 0;
            //        }


            //        return Json(new { data = temp }, JsonRequestBehavior.AllowGet);
            //    }
            //    catch (Exception ex)
            //    {
            //        WriteLog("Error - InitValues : NTID - " + User.Identity.Name.Split('\\')[1].Trim().ToUpper() + ", ErrorMsg : " + ex + ", At - " + DateTime.Now);
            //        return Json(new { data = temp }, JsonRequestBehavior.AllowGet);
            //    }
            //}

        }


        //public ActionResult Lookup_ItemList(string year)
        //{
        //    connection();
        //    OpenConnection();
        //    string Query = " Exec[dbo].[RFO_LookUp_Item] " + (int.Parse(year) + 1);
        //    SqlCommand cmd = new SqlCommand(Query, conn);
        //    SqlDataAdapter da = new SqlDataAdapter(cmd);
        //    DataTable dt = new DataTable();
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



        //    return new ContentResult { Content = JsonConvert.SerializeObject(parentRow), ContentType = "application/json" };
        //}


        //[HttpGet]
        //public ActionResult InitRowValues()
        //{


        //    using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //    //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //    {

        //        RequestItemsRepoEdit1 temp = new RequestItemsRepoEdit1();
        //        try
        //        {
        //            System.Security.Principal.IPrincipal User = System.Web.HttpContext.Current.User;
        //            SPOTONData_Table_2022 PresentUser = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper()));
        //            string presentUserName = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).EmployeeName;
        //            //string L2ReviewerName = string.Empty;
        //            //string L3ReviewerName = string.Empty;
        //            //try
        //            //{
        //            //    L2ReviewerName = BudgetingController.lstUsers.FindAll(user => PresentUser.Department.Trim().ToUpper().Equals(user.Group.ToUpper())).FirstOrDefault(head => head.Role.ToUpper().Contains("HEAD")).EmployeeName;
        //            //}
        //            //catch (Exception ex)
        //            //{
        //            //    L2ReviewerName = "NA";
        //            //}
        //            //try
        //            //{
        //            //    L3ReviewerName = BudgetingController.lstUsers.FindAll(user => PresentUser.Section.Trim().ToUpper().Contains(user.Section.ToUpper())).FirstOrDefault(head => head.Role.ToUpper().Contains("SECTION")).EmployeeName;
        //            //}
        //            //catch (Exception ex)
        //            //{
        //            //    L3ReviewerName = "NA";
        //            //}
        //            temp.POSpocNTID = User.Identity.Name.Split('\\')[1].ToUpper();
        //            temp.Requestor = presentUserName;
        //            //temp.Reviewer_1 = L2ReviewerName;
        //            //temp.Reviewer_2 = L3ReviewerName;
        //            DEPT_Table dEPT_Table = BudgetingController.lstDEPTs.Find(departmnt => departmnt.DEPT.Equals(BudgetingController.lstUsers.Find(x => x.EmployeeName == presentUserName).Department));
        //            temp.DEPT = dEPT_Table.ID;
        //            string Selected_Dept = BudgetingController.lstDEPTs.Find(dpt => dpt.ID.Equals(dEPT_Table.ID)).DEPT.Trim().ToUpper();
        //            try
        //            {
        //                temp.Reviewer_1 = BudgetingController.lstUsers.FindAll(user => Selected_Dept.Equals(user.Group.ToUpper())).FirstOrDefault(head => head.Role.ToUpper().Contains("HEAD")).EmployeeName;
        //            }
        //            catch (Exception ex)
        //            {
        //                temp.Reviewer_1 = "NA";
        //            }

        //            var PresentUserGroup = BudgetingController.lstUsers.Find(x => x.EmployeeName == presentUserName).Group;
        //            Groups_Table_Test gROUP_Table = BudgetingController.lstGroups_test.Find(grp => grp.Group.Trim().Equals(PresentUserGroup));
        //            temp.Group = gROUP_Table.ID;
        //            //temp.Group = ;
        //            temp.RequestDate = DateTime.Now.ToString("yyyy-MM-dd");

        //            var UserNTID = System.Web.HttpContext.Current.User.Identity.Name.Split('\\')[1].ToUpper();
        //            string Query = "";
        //            bool success = false;
        //            connection();
        //            Query = "IF EXISTS(SELECT RequestorNTID from RequestItems_Table where RequestorNTID = @User)SELECT TOP 1 BU , OEM from RequestItems_Table where RequestorNTID = @User order by UpdatedAt desc";
        //            OpenConnection();
        //            SqlCommand cmd = new SqlCommand(Query, conn);
        //            cmd.Parameters.AddWithValue("@User ", UserNTID);
        //            SqlDataReader dr = cmd.ExecuteReader();
        //            if (dr.HasRows)
        //            {
        //                dr.Read();
        //                temp.BU = int.Parse(dr["BU"].ToString());
        //                temp.OEM = int.Parse(dr["OEM"].ToString());
        //                temp.Reviewed_Quantity = 0;
        //                temp.Reviewed_Cost = 0;
        //                temp.Reviewer_2 = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(BudgetingController.lstBU_SPOCs.Find(spoc => spoc.BU.Equals(temp.BU)).VKMspoc.ToUpper().Trim())).EmployeeName;
        //                ;
        //                CloseConnection();

        //            }
        //            else
        //            {
        //                temp.BU = 0;
        //                temp.OEM = 0;
        //                temp.Reviewer_2 = "";
        //                temp.Reviewed_Quantity = 0;
        //                temp.Reviewed_Cost = 0;
        //            }


        //            return Json(new { data = temp }, JsonRequestBehavior.AllowGet);
        //        }
        //        catch (Exception ex)
        //        {
        //            WriteLog("Error - InitValues : NTID - " + User.Identity.Name.Split('\\')[1].Trim().ToUpper() + ", ErrorMsg : " + ex + ", At - " + DateTime.Now);
        //            return Json(new { data = temp }, JsonRequestBehavior.AllowGet);
        //        }
        //    }

        //}



        [HttpGet]
        public ActionResult DeptID_toName(int DeptID)
        {

            using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

            //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

            {

                string DeptName = BudgetingController.lstDEPTs.Find(departmnt => departmnt.ID.Equals(DeptID)).DEPT;

                return Json(new { data = DeptName.Trim() }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        public ActionResult GrpID_toName(int GrpID)
        {

            using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

            //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

            {

                string GrpName = BudgetingController.lstGroups_test.Find(grp => grp.ID.Equals(GrpID)).Group;

                return Json(new { data = GrpName.Trim() }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public ActionResult GetRevCost(int Reviewed_Quantity, double Unit_Price)
        {

            double RevCost = 0.0;
            try
            {
                RevCost = Reviewed_Quantity * Unit_Price;
            }
            catch (Exception ex)
            {
                WriteLog("Error - GetRevCost : NTID - " + User.Identity.Name.Split('\\')[1].Trim().ToUpper() + ", ErrorMsg : " + ex + ", At - " + DateTime.Now);

            }
            return Json(new { RevCost = RevCost, JsonRequestBehavior.AllowGet });
        }


        /// <summary>
        /// Function to edit existing Item
        /// </summary>
        /// <param name="req"></param>
        /// <param name="useryear"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddOrEdit(RequestItemsRepoEdit1 req, string useryear)
        {

            List<RequestItemsRepoView> viewList = new List<RequestItemsRepoView>();
            Emailnotify_OrderStage emailnotify = new Emailnotify_OrderStage();
            bool is_MailTrigger = false;
            string Description = "";
            try
            {
                BudgetingOrderController rfo = new BudgetingOrderController();
                string getEmailcc = rfo.GetCommonMail(req.RequestID);
                using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())
                {

                    System.Security.Principal.IPrincipal User = System.Web.HttpContext.Current.User;
                    string presentUserName = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).EmployeeName;
                    string PreviousOrderStatus = "";
                    // Nullable<bool> isRequestToOrder = false;
                    int isRequestToOrder = 0;
                    RequestItems_Table item = new RequestItems_Table();

                    //string L1Remarks = db.RequestItems_Table.AsNoTracking().FirstOrDefault(x => x.RequestID == req.RequestID).Comments;
                    //string L2Remarks = db.RequestItems_Table.AsNoTracking().FirstOrDefault(x => x.RequestID == req.RequestID).L2_Remarks;
                    //int L2_Qty = (int)db.RequestItems_Table.AsNoTracking().FirstOrDefault(x => x.RequestID == req.RequestID).L2_Qty;

                    string L1Remarks = "", L2Remarks = "";
                    int L2_Qty = 0;

                    if (req.RequestID != 0)
                    {

                        PreviousOrderStatus = db.RequestItems_Table.AsNoTracking().ToList().Find(x => x.RequestID == req.RequestID).OrderStatus;
                        var isUnplannedF02Item = db.RequestItems_Table.AsNoTracking().ToList().Find(x => x.RequestID == req.RequestID).Is_UnplannedF02Item;
                        var isRequestToOrder_flag = db.RequestItems_Table.AsNoTracking().ToList().Find(x => x.RequestID == req.RequestID).RequestToOrder;
                        isRequestToOrder = 2;//always from lbtem to requetor mAil should go- if order status change
                    }

                    if (BudgetingController.lstPrivileged.Find(person => person.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())) != null)//by purchase spoc
                    {
                        if (req.OrderedQuantity > req.Reviewed_Quantity && BudgetingController.lstFund.Find(fun => fun.ID.Equals(req.Fund)).Fund == "F02")
                        {
                            // viewList = GetData1(useryear);
                            return Json(new { success = false, message = "Ordered Quantity cannot be greater than Reviewed Quantity, Please check again!" }, JsonRequestBehavior.AllowGet);
                        }

                        if (PreviousOrderStatus == null)
                            PreviousOrderStatus = "";
                        if (req.RequestID != 0)
                        {
                            if (PreviousOrderStatus.ToString().Trim() == BudgetingController.lstOrderStatus.Find(status => status.OrderStatus.Trim().Equals("In Progress")).ID.ToString() && (req.OrderStatus.Trim() == BudgetingController.lstOrderStatus.Find(status => status.OrderStatus.Trim().Equals("Ordered")).ID.ToString() || req.OrderStatus.Trim() == BudgetingController.lstOrderStatus.Find(status => status.OrderStatus.Trim().Equals("Partially Ordered")).ID.ToString()))
                            {
                                viewList = GetData1(useryear);
                                if (req.OrderID == null || req.OrderID == "")
                                    return Json(new { data = viewList, success = false, message = "Please enter Order ID!" }, JsonRequestBehavior.AllowGet);
                                if (req.OrderPrice == 0)
                                    return Json(new { data = viewList, success = false, message = "Please enter Order Price!" }, JsonRequestBehavior.AllowGet);
                                if (req.OrderedQuantity == 0)
                                    return Json(new { data = viewList, success = false, message = "Please enter Ordered Quantity!" }, JsonRequestBehavior.AllowGet);
                                //req.OrderDate = DateTime.Now.Date;


                            }
                            if (PreviousOrderStatus.Trim() != req.OrderStatus)
                            {
                                is_MailTrigger = true;
                                emailnotify.is_RequesttoOrder = isRequestToOrder; ////
                                emailnotify.RequestID_orderemail = req.RequestID;
                                emailnotify.RFOReqNTID = req.RFOReqNTID;
                                emailnotify.GoodsRecipientID = req.GoodsRecID;
                                emailnotify.getCCemail = getEmailcc;
                                emailnotify.POSPOC_NTID = User.Identity.Name.Split('\\')[1].ToUpper();
                            }
                            //if (req.OrderStatus.Trim() == BudgetingController.lstOrderStatus.Find(status => status.OrderStatus.Trim().Equals("Cancelled")).ID.ToString())
                            //{
                            //    item.RequestToOrder = false; //if cancelled, user should be able to edit the item request & submit if needed
                            //                                 //item.RequiredDate = null;

                            //    item.RequestOrderDate = null; //reset the dts

                            //    item.OrderDate = null;

                            //    item.TentativeDeliveryDate = null;

                            //    item.ActualDeliveryDate = null;

                            //}

                        }


                    }
                    //else
                    //{
                    //    item.ApprovalDH = true;
                    //    item.ApprovalSH = true;
                    //    item.ApprovedDH = true;
                    //    item.ApprovedSH = false;
                    //    item.OrderStatus = req.OrderStatus != null ? req.OrderStatus.ToString() : " ";
                    //    item.DHAppDate = req.Review1_Date != null ? DateTime.Parse(req.Review1_Date) : DateTime.Now.Date;

                    //    if (req.OrderStatus != null)
                    //    {
                    //        item.ApprovedSH = true;
                    //        item.SHAppDate = DateTime.Now.Date;


                    //    }

                    //}
                    Description = req.Description != null ? req.Description.ToString() : "";
                    int Result = 0;
                    try
                    {
                        connection();
                        OpenConnection();
                        string Query = " EXEC [dbo].[VKMSPOC_AddOrEdit] '" + User.Identity.Name.Split('\\')[1].ToUpper() + "','" + req.BudgetCode + "'," + req.isProjected + ", " + req.Projected_Amount + "," + req.Q1 + "," + req.Q2 + ", " +
                                        req.Q3 + "," + req.Q4 + "," + req.Unused_Amount + ",'" + req.RequestorNTID + "','" + req.UpdatedAt + "','" + req.PORemarks + "','" + req.VKM_Year + "','" + req.RequestID + "','" + req.BU + "','" +
                                        req.DEPT + "','" + req.Group + "','" + req.OEM + "','" + req.Item_Name + "','" + req.Category + "','" + req.Cost_Element + "'," + req.Unit_Price + "," + req.Required_Quantity + "," + req.Total_Price + "," +
                                        req.Reviewed_Quantity + "," + req.Reviewed_Cost + ",'" + req.Comments + "', '" + req.Requestor + "','" + req.Reviewer_1 + "','" + req.Reviewer_2 + "','" + req.RequestDate + "','" + req.SubmitDate + "','" +
                                        req.Review1_Date + "','" + req.Review2_Date + "','" + req.OrderID + "','" + req.OrderStatus + "','" + req.RequiredDate + "','" + req.RequestOrderDate + "','" + req.OrderDate + "','" +
                                        req.TentativeDeliveryDate + "','" + req.ActualDeliveryDate + "'," + req.OrderedQuantity + ",'" + req.RequestToOrder + "','" + req.ApprovalHoE + "','" + req.ApprovalSH + "','" + req.ApprovedHoE + "','" +
                                        req.ApprovedSH + "','" + req.Fund + "','" + req.Project + "','" + req.ActualAvailableQuantity + "','" + req.Customer_Name + "','" + req.Customer_Dept + "','" + req.BM_Number + "','" + req.Task_ID + "','" +
                                        req.Resource_Group_Id + "','" + req.PIF_ID + "','" + req.Is_UnplannedF02Item + "','" + req.RFOReqNTID + "','" + req.GoodsRecID + "','" + req.BudgetCenterID + "','" + req.UnloadingPoint + "','" +
                                        req.RFOApprover + "','" + req.BudgetCodeDescription + "','" + req.UnitofMeasure + "','" + req.QuoteAvailable + "','" + req.LabName + "','" + req.OrderType + "','" + req.CostCenter + "','" + req.RFOSubmittedDate + "','" +
                                        req.ELOSubmittedDate + "','" + req.DaysTaken + "','" + req.SRSubmitted + "','" + req.RFQNumber + "','" + req.PONumber + "','" + req.PRNumber + "','" + req.POReleaseDate + "','" + req.SRAwardedDate + "','" +
                                        req.SRApprovalDays + "','" + req.SRResponsibleBuyerNTID + "','" + req.SRManagerNTID + "','" + (req.POSpocNTID == null ? User.Identity.Name.Split('\\')[1].ToUpper() : req.POSpocNTID) + "','" + req.HOEView_ActionHistory + "','" + req.Material_Part_Number + "','" + req.SupplierName_with_Address + "','" +
                                        req.Purchase_Type + "','" + req.Project_ID + "','" + req.Upload_File_Name + "','" + Description + "' ," +
                                        //req.OrderPrice + ","
                                        + (req.OrderPrice_UserInput==null?0:req.OrderPrice_UserInput) + "," + (req.SR_Value == null ? 0 : req.SR_Value) + "," + (req.PR_Value == null ? 0 : req.PR_Value) + "," + (req.Invoice_Value == null ? 0 : req.Invoice_Value) + "," + req.Currency;
                     
                        SqlCommand cmd = new SqlCommand(Query, conn);
                        Result = cmd.ExecuteNonQuery();
                        CloseConnection();


                    }
                    catch (Exception ex)
                    {
                        Result = 0;
                        CloseConnection();
                    }

                    if (Result > 0)
                    {
                        if (req.RequestID == 0)
                        {
                            if (BudgetingController.lstFund.Find(fun => fun.ID.Equals(Int32.Parse(req.Fund.ToString()))).Fund == "F02" && req.VKM_Year == DateTime.Now.Year) //checking if new unplanned f02 item raised (not by vkmspoc - when current yr == vkm yr -> raised by lab spoc)
                            {


                                isRequestToOrder = 3; //unplanned F02


                                is_MailTrigger = true;
                                emailnotify.is_RequesttoOrder = isRequestToOrder; ////

                                emailnotify.RequestID_orderemail = req.RequestID;

                                return Json(new { data = emailnotify, is_MailTrigger, success = true, message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                return Json(new { /*data = viewList, */success = true, message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);

                            }

                        }
                        else
                        {

                            return Json(new { data = emailnotify, is_MailTrigger, success = true, RequestID = req.RequestID, message = "Updated Successfully" }, JsonRequestBehavior.AllowGet);

                        }

                    }
                    else
                    {
                        return Json(new { success = false, message = "Unable to update the details" }, JsonRequestBehavior.AllowGet);
                    }

                }
            }
            catch (Exception ex)
            {
                WriteLog("Error - AddOrEdit : NTID - " + User.Identity.Name.Split('\\')[1].Trim().ToUpper() + ", ErrorMsg : " + ex + ", At - " + DateTime.Now);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult SaveRFOMail(RFOApproverEdit rfo)
        {
            try
            {
                connection();
                OpenConnection();
                string Query = " EXEC [dbo].[SaveRFOMail] " + rfo.ID + ", '" + rfo.Section + "', '" + rfo.NTID + "', '" + rfo.EmployeeName +"'," + rfo.IsPaused + " ";
                SqlCommand cmd = new SqlCommand(Query, conn);
                cmd.ExecuteNonQuery();
                CloseConnection();
                return Json(new {  success = true, message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                CloseConnection();
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public ActionResult SaveCTGDetails(CTGEdit ctg)
        {
            try
            {
                connection();
                OpenConnection();
                string Query = " EXEC [dbo].[SaveCTG] " + ctg.ID + ", '" + ctg.Section + "', " + ctg.Amount + " ";
                SqlCommand cmd = new SqlCommand(Query, conn);
                cmd.ExecuteNonQuery();
                CloseConnection();
                return Json(new { success = true, message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                CloseConnection();
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }



        /// <summary>
        /// function to delete an existing item
        /// </summary>
        /// <param name="id"></param>
        /// <param name="useryear"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete(int id, string useryear)
        {
            List<RequestItemsRepoView> viewList = new List<RequestItemsRepoView>();
            try
            {

                using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

                //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

                {
                    if (db.RequestItems_Table.AsNoTracking().ToList().FindAll(x => x.RequestID == id).Count() == 0)
                    {
                        viewList = GetData1(useryear);
                        return Json(new { data = viewList, success = false, message = "The Item is unavailable. Please check your Request items queue !" }, JsonRequestBehavior.AllowGet);

                    }
                    RequestItems_Table item = db.RequestItems_Table.Where(x => x.RequestID == id).FirstOrDefault<RequestItems_Table>();

                    db.RequestItems_Table.Remove(item);
                    db.SaveChanges();
                    viewList = GetData1(useryear);
                    connection();
                    var Query = "IF EXISTS(SELECT RequestID FROM [PODetails_Table] WHERE RequestID = @reqid)DELETE FROM [PODetails_Table] WHERE Requestid = @reqid";

                    SqlCommand cmd = new SqlCommand(Query, conn);

                    cmd.Parameters.AddWithValue("@reqid", id);


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
                }
            }
            catch (Exception ex)
            {
                WriteLog("Error - Delete : NTID - " + User.Identity.Name.Split('\\')[1].Trim().ToUpper() + ", ErrorMsg : " + ex + ", At - " + DateTime.Now);

            }
            return Json(new { data = viewList, success = true, message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);



        }

        /// <summary>
        /// Function to send back the item to L1 requestor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="useryear"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SendBack(int id, string useryear)
        {
            Emailnotify emailnotify = new Emailnotify();
            List<RequestItemsRepoView> viewList = new List<RequestItemsRepoView>();
            try
            {

                using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())
                {
                    string appr_msg = "";
                    RequestItems_Table item = db.RequestItems_Table.Where(x => x.RequestID == id).FirstOrDefault<RequestItems_Table>();

                    if (BudgetingController.lstBU_SPOCs.Find(person => person.VKMspoc.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())) == null)
                    {

                        appr_msg = "Sorry! Current user is not authorised to sendback this Item";
                        return Json(new { success = false, message = appr_msg }, JsonRequestBehavior.AllowGet);

                    }
                    else
                    {
                        //item.isCancelled = 2; //Cancelled by VKM SPOC

                        //item.ApprovalSH = false;
                        //item.ApprovalDH = false;
                        //item.ApprovedDH = false;
                        //item.DHAppDate = null;
                        //item.ApprQuantity = null;
                        //item.ApprCost = null;
                        //item.SubmitDate = null;
                        //db.Entry(item).State = EntityState.Modified;
                        //db.SaveChanges();   //before ommit

                        //email attributes
                        emailnotify.RequestID_foremail = id;
                        emailnotify.ReviewLevel = "L3";
                        emailnotify.is_ApprovalorSendback = (bool)item.ApprovalDH || (bool)item.ApprovalSH;
                        emailnotify.RFOReqNTID = item.RFOReqNTID;
                        emailnotify.VKMSPOC_NTID = User.Identity.Name.Split('\\')[1].ToUpper();
                        //qry = " Update RequestItems_Table set isProjected = 1, Q1 = 1, Q2 = 1, Q3 = 1, Q4 = 1,ApprCost = case when ApprCost is null then TotalPrice else ApprCost end, Projected_Amount = case when ApprCost is null then 0 else ApprCost end, Unused_Amount =0,ApprQuantity = case when ApprQuantity is null then ReqQuantity else ApprQuantity end,  ApprovedSH = 1,SHAppDate = '" + DateTime.Now.Date + "' Where RequestID = " + id + ""; //,ApprQuantity = " + item. + ",ApprCost"; 

                        string qry = "";
                        connection();
                        if (item.RequestSource == "RFO")
                        {
                            qry = " Update RequestItems_Table set isCancelled=2, OrderStatus = 6, OrderDescriptionID = 31, RequestToOrder = 0 , SubmitDate=null, VKMSPOC_Approval =0 where RequestID = " + id + " "; //,ApprQuantity = " + item. + ",ApprCost"; 
                        }
                        else
                        {
                            qry = " Update RequestItems_Table set isCancelled=2,ApprovalSH = 0,ApprovalDH = 0,ApprovedDH=0,DHAppDate = null, ApprQuantity =null,ApprCost=null, SubmitDate=null where RequestID = " + id + " "; //,ApprQuantity = " + item. + ",ApprCost"; 
                        }

                        OpenConnection();
                        SqlCommand cmd1 = new SqlCommand(qry, conn);
                        cmd1.ExecuteNonQuery();
                        CloseConnection();



                    }



                    return Json(new { data = emailnotify, success = true, message = appr_msg + "Sent Back to " + item.RequestorNT + " Successfully" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                WriteLog("Error - Sendback : NTID - " + User.Identity.Name.Split('\\')[1].Trim().ToUpper() + ", ErrorMsg : " + ex + ", At - " + DateTime.Now);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// function to mark as L3 approved
        /// </summary>
        /// <param name="id"></param>
        /// <param name="useryear"></param>
        /// <returns></returns>
        /// 
        [HttpPost]
        public ActionResult SHApprove(int[] id, string useryear)
        {
            try
            {
                string presentUserName = "", qry = "", Requests = "";
                int totalCount = 0;
                DataSet ds = new DataSet();
                DataTable dt1 = new DataTable();
                DataTable dt2 = new DataTable();
                DataTable dt3 = new DataTable();


                Requests = string.Join(",", id); //"1999999999";

                List<RequestItemsRepoView> viewList = new List<RequestItemsRepoView>();

                using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())
                {
                    if (BudgetingController.lstBU_SPOCs.Find(person => person.VKMspoc.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())) == null)
                    {

                        viewList = GetData1(useryear);
                        return Json(new { data = viewList, success = false, email = false, message = "Sorry! Current user is not authorised to submit this Item" }, JsonRequestBehavior.AllowGet);

                    }
                    else
                    {
                        System.Security.Principal.IPrincipal User = System.Web.HttpContext.Current.User;
                        connection();
                        qry = " EXEC [dbo].[VKMSPOC_Approve] '" + Requests + "','" + User.Identity.Name.Split('\\')[1].ToUpper() + "', '" + useryear + "'";
                        OpenConnection();
                        SqlCommand cmd1 = new SqlCommand(qry, conn);
                        SqlDataAdapter da = new SqlDataAdapter(cmd1);
                        da.Fill(ds); // ds will have two tables. one is for templist and another one is to get the total requests count
                        CloseConnection();
                        dt1 = ds.Tables[0];
                        dt2 = ds.Tables[1];
                        dt3 = ds.Tables[2];
                        viewList = GetData1(useryear);
                        if (dt1.Rows.Count > 0)
                        {                           
                            if (dt1.Rows[0]["Msg"].ToString() != "")
                            {                                
                                return Json(new { success = false, message = dt1.Rows[0]["Msg"].ToString(), data = viewList }, JsonRequestBehavior.AllowGet);

                            }
                            else
                            {
                                //there are two scenarios of VKM SPOC Submit :
                                //1. submit during vkm planning (no mail should get triggered) 
                                //2. submit rfo requests for which mail should get sent to elo
                                //hence, dt2 will have only requests for which mail to be triggered - if contains value send mail - submit all / multi submit / single submit
                                if(dt2.Rows.Count > 0) //there are requests for which mail to be sent to elo
                                {
                                    Emailnotify emailnotify = new Emailnotify();
                                    List<RequestItems_Table> reqlist = new List<RequestItems_Table>();
                                    //email attributes
                                    foreach (DataRow row in dt2.Rows)
                                    {
                                        RequestItems_Table req = new RequestItems_Table();
                                        req.RequestID = int.Parse(row["RequestID"].ToString());
                                        req.ItemName = row["ItemName"].ToString();
                                        req.TotalPrice = decimal.Parse(row["TotalPrice"].ToString());  
                                        req.ReqQuantity = int.Parse(row["ReqQuantity"].ToString()); 
                                        req.ApprCost = decimal.Parse(row["ApprCost"].ToString());
                                        req.ApprQuantity = int.Parse(row["ApprQuantity"].ToString());
                                        req.RFOReqNTID = row["RFOReqNTID"].ToString();
                                        req.DEPT = row["DEPT"].ToString();
                                        req.RequiredDate = DateTime.Parse(row["RequiredDate"].ToString());
                                        req.PORemarks = row["PORemarks"].ToString();
                                        req.Fund = row["Fund"].ToString();
                                        reqlist.Add(req);
                                    }
                                    emailnotify.Requests_foremail = reqlist;
                                    emailnotify.ReviewLevel = "L3";
                                    emailnotify.is_ApprovalorSendback = true; //(bool)request.ApprovalDH || (bool)request.ApprovalSH; - commented since this might contain multiple requests and if submit, value is always true
                                    BudgetingOrderController rfo = new BudgetingOrderController();
                                    //all the commented data will be segregated amoungst the multiple submitted requests in mail method
                                    //emailnotify.POSPOC_NTID = rfo.GetSectionCoordinatorsNTID(request.DEPT);
                                    //emailnotify.getTOemail = rfo.GetCommonMail(id);
                                    //emailnotify.RFOReqNTID = request.RFOReqNTID;
                                    emailnotify.VKMSPOC_NTID = User.Identity.Name.Split('\\')[1].ToUpper();

                                    BudgetingController budg = new BudgetingController();
                                    string email_message = "";
                                    try
                                    {
                                        email_message = "Mail has been sent to the ELO Team to proceed for Item Procurement!";
                                    }
                                    catch
                                    {
                                        email_message = "";
                                    }
                                    budg.SendEmail(emailnotify);

                                    if (Requests.Contains("1999999999"))
                                    {
                                        if (dt3.Rows[0]["Cnt"].ToString() != "")
                                        {
                                            totalCount = int.Parse(dt3.Rows[0]["Cnt"].ToString());

                                        }
                                        return Json(new { email_message = email_message ,data = viewList, success = true, email = true, message = totalCount.ToString() + " Item(s) Reviewed Successfully" }, JsonRequestBehavior.AllowGet);

                                    }
                                    else
                                    {

                                         return Json(new { email_message = email_message, data = viewList, success = true, email = true, message = "Submitted Successfully to ELO Team for procurement !" }, JsonRequestBehavior.AllowGet);

                                        
                                    }
                                }
                                else //no mail to be sent
                                {
                                    if (Requests.Contains("1999999999"))
                                    {
                                        if (dt3.Rows[0]["Cnt"].ToString() != "")
                                        {
                                            totalCount = int.Parse(dt3.Rows[0]["Cnt"].ToString());

                                        }
                                        return Json(new { data = viewList, success = true, email = false, message = totalCount.ToString() + " Item(s) Reviewed Successfully" }, JsonRequestBehavior.AllowGet);

                                    }
                                    else
                                    {
                                        
                                        return Json(new { data = viewList, success = true, email = false, message = "Submitted Successfully" }, JsonRequestBehavior.AllowGet);
                                        
                                    }
                                }
                               
                            }
                        }
                        return Json(new { data = viewList, success = true, email = false, message = "Submitted Successfully" }, JsonRequestBehavior.AllowGet);

                    }

                }

            }
            catch (Exception ex)
            {
                WriteLog("Error - SHApprove : NTID - " + User.Identity.Name.Split('\\')[1].Trim().ToUpper() + ", ErrorMsg : " + ex + ", At - " + DateTime.Now);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }




        //[HttpPost]
        //public ActionResult SHApprove(int id, string useryear)
        //{
        //    try
        //    {
        //        string presentUserName = "", qry = "", Requests = "";
        //        int totalCount = 0;
        //        DataSet ds = new DataSet();
        //        DataTable dt1 = new DataTable();
        //        DataTable dt2 = new DataTable();
        //        DataTable dt3 = new DataTable();

        //        Requests = "1999999999";

        //        List<RequestItemsRepoView> viewList = new List<RequestItemsRepoView>();

        //        using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //        //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //        {
        //            if (BudgetingController.lstBU_SPOCs.Find(person => person.VKMspoc.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())) == null)
        //            {

        //                viewList = GetData1(useryear);
        //                return Json(new { data = viewList, success = false, email = false, message = "Sorry! Current user is not authorised to submit this Item" }, JsonRequestBehavior.AllowGet);

        //            }
        //            else
        //            {

        //                if (id == 1999999999)
        //                {
        //                    System.Security.Principal.IPrincipal User = System.Web.HttpContext.Current.User;

        //                    connection();
        //                    qry = " EXEC [dbo].[VKMSPOC_Approve] '" + Requests + "','" + User.Identity.Name.Split('\\')[1].ToUpper() + "', '" + useryear + "'";
        //                    OpenConnection();
        //                    SqlCommand cmd1 = new SqlCommand(qry, conn);
        //                    SqlDataAdapter da = new SqlDataAdapter(cmd1);
        //                    da.Fill(ds); // ds will have two tables. one is for templist and another one is to get the total requests count
        //                    CloseConnection();

        //                    dt1 = ds.Tables[0];
        //                    dt2 = ds.Tables[1];
        //                    dt3 = ds.Tables[2];

        //                    if (dt1.Rows.Count > 0)
        //                    {
        //                        if (dt1.Rows[0]["Msg"].ToString() != "")
        //                        {
        //                            viewList = GetData1(useryear);
        //                            return Json(new { success = false, message = dt1.Rows[0]["Msg"].ToString() }, JsonRequestBehavior.AllowGet);

        //                        }

        //                        else
        //                        {
        //                            //if (request.RequestSource != null && request.RequestSource.Trim() == "RFO")
        //                            //{
        //                            Emailnotify emailnotify = new Emailnotify();
        //                            //    //email attributes
        //                            //    emailnotify.RequestID_foremail = id;
        //                            //    emailnotify.ReviewLevel = "L3";
        //                            //    emailnotify.is_ApprovalorSendback = (bool)request.ApprovalDH || (bool)request.ApprovalSH;
        //                            //    BudgetingOrderController rfo = new BudgetingOrderController();
        //                            //    emailnotify.POSPOC_NTID = rfo.GetSectionCoordinatorsNTID(request.DEPT);
        //                            //    emailnotify.getTOemail = rfo.GetCommonMail(id);
        //                            //    emailnotify.RFOReqNTID = request.RFOReqNTID;
        //                            //    emailnotify.VKMSPOC_NTID = User.Identity.Name.Split('\\')[1].ToUpper();

        //                            //    return Json(new { data1 = emailnotify, data = viewList, success = true, email = true, message = "Submitted Successfully to ELO Team for procurement !" }, JsonRequestBehavior.AllowGet);

        //                            //}
        //                            //viewList = GetData1(useryear);
        //                            //return Json(new { data = viewList, success = true, email = false, message = totalCount.ToString() + " Item(s) Reviewed Successfully" }, JsonRequestBehavior.AllowGet);
        //                        }
        //                    }


        //                    viewList = GetData1(useryear);
        //                    return Json(new { data = viewList, success = true, email = false, message = totalCount.ToString() + " Item(s) Reviewed Successfully" }, JsonRequestBehavior.AllowGet);

        //                }
        //                else
        //                {
        //                    connection();
        //                    qry = " EXEC [dbo].[VKMSPOC_Approve] '" + Requests + "','" + User.Identity.Name.Split('\\')[1].ToUpper() + "', '" + useryear + "'";
        //                    OpenConnection();
        //                    SqlCommand cmd1 = new SqlCommand(qry, conn);
        //                    SqlDataAdapter da = new SqlDataAdapter(cmd1);
        //                    da.Fill(ds); // ds will have two tables. one is for templist and another one is to get the total requests count
        //                    CloseConnection();

        //                    dt1 = ds.Tables[0];
        //                    dt2 = ds.Tables[1];
        //                    dt3 = ds.Tables[2];

        //                    if (dt1.Rows.Count > 0)
        //                    {
        //                        if (dt1.Rows[0]["Msg"].ToString() != "")
        //                        {
        //                            viewList = GetData1(useryear);
        //                            return Json(new { success = false, message = dt1.Rows[0]["Msg"].ToString() }, JsonRequestBehavior.AllowGet);

        //                        }
        //                        else

        //                        {
        //                            viewList = GetData1(useryear);
        //                            var request = db.RequestItems_Table.Where(x => x.RequestID == id).FirstOrDefault<RequestItems_Table>();
        //                            if (request.RequestSource != null && request.RequestSource.Trim() == "RFO")
        //                            {
        //                                Emailnotify emailnotify = new Emailnotify();
        //                                //email attributes
        //                                emailnotify.RequestID_foremail = id;
        //                                emailnotify.ReviewLevel = "L3";
        //                                emailnotify.is_ApprovalorSendback = (bool)request.ApprovalDH || (bool)request.ApprovalSH;
        //                                BudgetingOrderController rfo = new BudgetingOrderController();
        //                                emailnotify.POSPOC_NTID = rfo.GetSectionCoordinatorsNTID(request.DEPT);
        //                                emailnotify.getTOemail = rfo.GetCommonMail(id);
        //                                emailnotify.RFOReqNTID = request.RFOReqNTID;
        //                                emailnotify.VKMSPOC_NTID = User.Identity.Name.Split('\\')[1].ToUpper();

        //                                return Json(new { data1 = emailnotify, data = viewList, success = true, email = true, message = "Submitted Successfully to ELO Team for procurement !" }, JsonRequestBehavior.AllowGet);

        //                            }
        //                            else
        //                            {
        //                                return Json(new { data = viewList, success = true, email = false, message = "Submitted Successfully" }, JsonRequestBehavior.AllowGet);
        //                            }
        //                        }
        //                    }


        //                    return Json(new { data = viewList, success = true, email = false, message = "Submitted Successfully" }, JsonRequestBehavior.AllowGet);
        //                }
        //            }

        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        WriteLog("Error - SHApprove : NTID - " + User.Identity.Name.Split('\\')[1].Trim().ToUpper() + ", ErrorMsg : " + ex + ", At - " + DateTime.Now);
        //        return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        //    }
        //}

        public ActionResult CurrencyConversion()
        {
            List<CurrencyConversionView> viewList = new List<CurrencyConversionView>();
            try
            {

                using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

                //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

                {
                    List<Currency_Conversion_Table> data = db.Currency_Conversion_Table.ToList<Currency_Conversion_Table>();

                    foreach (Currency_Conversion_Table item in data)
                    {
                        CurrencyConversionView citem = new CurrencyConversionView();
                        citem.ID = item.ID;
                        citem.Currency = item.Currency;
                        citem.ConversionRate = item.ConversionRate_to_USD;

                        viewList.Add(citem);
                    }




                    return Json(new { data = viewList }, JsonRequestBehavior.AllowGet);

                }
            }
            catch (Exception ex)
            {
                WriteLog("Error - Curr Conv : NTID - " + User.Identity.Name.Split('\\')[1].Trim().ToUpper() + ", ErrorMsg : " + ex + ", At - " + DateTime.Now);
                return Json(new { data = viewList }, JsonRequestBehavior.AllowGet);
            }

        }
        /// <summary>
        /// Function to edit existing Item
        /// </summary>
        /// <param name="req"></param>
        /// <param name="useryear"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddOrEdit_CurrConv(CurrencyConversionView curritem)
        {

            try
            {


                using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

                //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

                {


                    Currency_Conversion_Table item1 = new Currency_Conversion_Table();

                    item1.ID = curritem.ID;
                    item1.Currency = curritem.Currency;
                    item1.ConversionRate_to_USD = curritem.ConversionRate;

                    db.Entry(item1).State = EntityState.Modified;

                    db.SaveChanges();



                    return Json(new { message = "Updated Successfully" }, JsonRequestBehavior.AllowGet);



                }
            }
            catch (Exception ex)
            {
                WriteLog("Error - AddOrEdit -Curr Conv : NTID - " + User.Identity.Name.Split('\\')[1].Trim().ToUpper() + ", ErrorMsg : " + ex + ", At - " + DateTime.Now);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Function to update Currency Conversion Rate
        /// </summary>
        /// <param name="curritem"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdateMasterList_CurrConv(CurrencyConversionView curritem)
        {
            try
            {


                using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

                //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

                {

                    db.Database.CommandTimeout = 10000;
                    //code to update the unitpriceusd for all thhe items present so far
                    List<ItemsCostList_Table> item_toupdate = db.ItemsCostList_Table.ToList().FindAll(x => x.Currency == BudgetingController.lstCurrency.Find(curr => curr.Currency.Equals(curritem.Currency)).ID.ToString());
                    foreach (ItemsCostList_Table item in item_toupdate)
                    {

                        item.Category = item.Category.ToString();
                        item.Comments = item.Comments;
                        item.Cost_Element = item.Cost_Element.ToString();
                        item.Currency = item.Currency.ToString();
                        item.Item_Name = item.Item_Name;
                        item.RequestorNT = item.RequestorNT;
                        item.Unit_Price = (double)item.Unit_Price;
                        item.UnitPriceUSD = (double)item.Unit_Price * (double)curritem.ConversionRate;
                        item.S_No = item.S_No;
                        item.Deleted = item.Deleted;
                        if (item.VendorCategory != null)
                        {
                            item.VendorCategory = item.VendorCategory.ToString();
                        }

                        db.Entry(item).State = EntityState.Modified;
                        db.SaveChanges();

                        db.Database.CommandTimeout = 10000;
                    }
                    return Json(new { message = "Updated Masterlist Successfully" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                WriteLog("Error - UpdateMasterList_CurrConv : NTID - " + User.Identity.Name.Split('\\')[1].Trim().ToUpper() + ", ErrorMsg : " + ex + ", At - " + DateTime.Now);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }

        }


        /// <summary>
        /// function to get the unit price in USD of Item
        /// </summary>
        /// <param name="UnitPrice"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetOrderPriceinUSD(double OrderPrice, int Currency)
        {
            try
            {



                using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

                //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

                {
                    var OrderPriceUSD = 0.0;

                    decimal conversionEURate = /*(decimal)1.15;*/ db.Currency_Conversion_Table.FirstOrDefault(x => x.Currency.Contains("EUR")).ConversionRate_to_USD;

                    decimal conversionINRate = /*(decimal)0.014;*/db.Currency_Conversion_Table.FirstOrDefault(x => x.Currency.Contains("INR")).ConversionRate_to_USD;

                    //update the price in USD

                    if (BudgetingController.lstCurrency.Find(curr => curr.ID.Equals(Currency)).Currency.ToUpper().Trim() == "USD")
                        OrderPriceUSD = OrderPrice;
                    else if (BudgetingController.lstCurrency.Find(curr => curr.ID.Equals(Currency)).Currency.ToUpper().Trim() == "INR")
                        OrderPriceUSD = (double)((decimal)OrderPrice * conversionINRate);
                    else if (BudgetingController.lstCurrency.Find(curr => curr.ID.Equals(Currency)).Currency.ToUpper().Trim() == "EUR")
                        OrderPriceUSD = (double)((decimal)OrderPrice * conversionEURate);
                    else
                        OrderPriceUSD = 0.0;
                    return Json(OrderPriceUSD, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                WriteLog("Error - GetOrderPriceinUSD : NTID - " + User.Identity.Name.Split('\\')[1].Trim().ToUpper() + ", ErrorMsg : " + ex + ", At - " + DateTime.Now);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }

        }



        /// <summary>
        /// Function to update Dept Details in Reviewed Request List
        /// </summary>
        /// <param name="reqids"></param>
        /// /// <param name="deptselected"></param>
        /// <returns></returns>
        //[HttpPost]
        //public ActionResult UpdateRequestList_DeptGrpChange(int[] reqids, int deptselected, int grpselected)
        //{


        //    using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //    //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //    {

        //        db.Database.CommandTimeout = 10000;
        //        List<RequestItems_Table> item_toupdate = new List<RequestItems_Table>();
        //        //    //code to update the dept (ordering stage dept update) for all the request present so far in the reviewed list
        //        foreach (var requestid in reqids)
        //        {
        //            var req = db.RequestItems_Table.ToList().Find(x => x.RequestID == requestid);
        //            item_toupdate.Add(req);
        //        }

        //        foreach (RequestItems_Table item in item_toupdate)
        //        {

        //            item.BU = item.BU;
        //            item.Category = item.Category;
        //            item.Comments = item.Comments;
        //            item.CostElement = item.CostElement;
        //            item.BudgetCode = item.BudgetCode;
        //            item.DEPT = deptselected.ToString();
        //            item.Group = grpselected.ToString();
        //            item.ItemName = item.ItemName;
        //            item.OEM = item.OEM;
        //            item.ReqQuantity = item.ReqQuantity;
        //            item.RequestID = item.RequestID;
        //            item.RequestorNT = item.RequestorNT;
        //            item.DHNT = item.DHNT;
        //            item.SHNT = item.SHNT;
        //            item.TotalPrice = item.TotalPrice;
        //            item.UnitPrice = item.UnitPrice;
        //            item.ApprQuantity = item.ApprQuantity == null ? (int)0 : (int)item.ApprQuantity;
        //            item.ApprCost = item.ApprCost == null ? (int)0 : (int)item.ApprCost;
        //            item.ApprovalDH = item.ApprovalDH;
        //            item.ApprovalSH = item.ApprovalSH;
        //            item.ApprovedDH = item.ApprovedDH;
        //            item.ApprovedSH = item.ApprovedSH;


        //            item.RequestDate = item.RequestDate != null ? item.RequestDate : DateTime.Now;
        //            item.SubmitDate = item.SubmitDate != null ? item.SubmitDate : DateTime.Now;
        //            item.DHAppDate = item.DHAppDate != null ? item.DHAppDate : DateTime.Now;
        //            item.SHAppDate = item.SHAppDate != null ? item.SHAppDate : DateTime.Now;

        //            item.OrderID = item.OrderID;
        //            item.OrderPrice = item.OrderPrice;
        //            if (item.OrderStatus != null && item.OrderStatus.Trim() != "")
        //            {
        //                item.OrderStatus = item.OrderStatus;

        //            }
        //            else
        //            {
        //                item.OrderStatus = null;


        //            }
        //            if (item.ActualDeliveryDate != null)
        //            {
        //                item.ActualDeliveryDate = item.ActualDeliveryDate;
        //            }
        //            else
        //                item.ActualDeliveryDate = null;
        //            if (item.RequiredDate != null)
        //            {
        //                item.RequiredDate = item.RequiredDate;
        //            }
        //            else
        //                item.RequiredDate = null;
        //            if (item.RequestOrderDate != null)
        //            {
        //                item.RequestOrderDate = item.RequestOrderDate;
        //            }
        //            else
        //                item.RequestOrderDate = null;
        //            if (item.OrderDate != null)
        //            {
        //                item.OrderDate = item.OrderDate;
        //            }
        //            else
        //                item.OrderDate = null;
        //            if (item.TentativeDeliveryDate != null)
        //            {
        //                item.TentativeDeliveryDate = item.TentativeDeliveryDate;
        //            }
        //            else
        //                item.TentativeDeliveryDate = null;




        //            if (item.Fund != null)
        //            {
        //                item.Fund = item.Fund;
        //            }
        //            else
        //            {
        //                item.Fund = BudgetingController.lstFund.Find(fund => fund.Fund.Trim().Equals("F02")).ID.ToString();
        //            }

        //            if (item.RequestOrderDate != null)
        //            {
        //                item.RequestToOrder = true;
        //            }
        //            else
        //            {
        //                item.RequestToOrder = false;
        //            }
        //            if (item.OrderedQuantity != null)
        //            {
        //                item.OrderedQuantity = (int)item.OrderedQuantity;
        //            }
        //            else
        //            {
        //                item.OrderedQuantity = null;
        //            }


        //            item.Customer_Dept = item.Customer_Dept;

        //            item.Customer_Name = item.Customer_Name;

        //            item.BM_Number = item.BM_Number;

        //            item.PIF_ID = item.PIF_ID;

        //            item.Resource_Group_Id = item.Resource_Group_Id;

        //            item.Task_ID = item.Task_ID;



        //            db.Entry(item).State = EntityState.Modified;
        //            db.SaveChanges();

        //            db.Database.CommandTimeout = 10000;
        //        }
        //        return Json(new { message = "Updated Reviewed Requestlist Successfully" }, JsonRequestBehavior.AllowGet);




        //    }

        //}



        ////*******************DEPT DETAILS***************

        ///// <summary>
        ///// function to fetch the Request Items made by the Requestor during the year chosen for view
        ///// </summary>
        //public ActionResult GetData_DeptDetails()
        //{


        //    using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //    //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //    {
        //        try
        //        {

        //            List<DeptView> viewList = new List<DeptView>();
        //            viewList = GetData1_DeptDetails();

        //            return Json(new
        //            {

        //                data = viewList
        //            }, JsonRequestBehavior.AllowGet);
        //        }
        //        catch (Exception ex)
        //        {
        //            return Json(new { success = true, message = "Unable to load Department Mapping List, Please Try again later!" }, JsonRequestBehavior.AllowGet);

        //        }

        //    }
        //}


        //public List<DeptView> GetData1_DeptDetails()
        //{

        //    using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //    //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //    {
        //        List<DEPTMapping_Table> reqList = new List<DEPTMapping_Table>();
        //        reqList = db.DEPTMapping_Table.ToList();
        //        List<DeptView> viewList = new List<DeptView>();
        //        foreach (DEPTMapping_Table item in reqList)
        //        {
        //            DeptView ritem = new DeptView();


        //            ritem.PlanningDept = int.Parse(item.PlanningDEPT);
        //            ritem.OrderingDept = int.Parse(item.OrderingDEPT);
        //            ritem.Updated_By = item.UpdatedBy;
        //            ritem.ID = item.ID;



        //            viewList.Add(ritem);
        //        }
        //        return viewList;

        //    }
        //}



        //[HttpPost]
        //public ActionResult AddOrEdit_DeptDetails(DeptView req)
        //{
        //    List<DeptView> viewList = new List<DeptView>();


        //    using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //    //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //    {
        //        System.Security.Principal.IPrincipal User = System.Web.HttpContext.Current.User;
        //        string presentUserName = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).EmployeeName;

        //        DEPTMapping_Table item = new DEPTMapping_Table();

        //        item.PlanningDEPT = req.PlanningDept.ToString();
        //        item.OrderingDEPT = req.OrderingDept.ToString();
        //        item.ID = req.ID;
        //        item.UpdatedBy = presentUserName;



        //        db.Entry(item).State = EntityState.Modified;

        //        db.SaveChanges();

        //        //viewList = GetData1(useryear);

        //        return Json(new { success = true }, JsonRequestBehavior.AllowGet);

        //    }


        //}



        ///// <summary>
        ///// Function to update Dept Details in Reviewed Request List
        ///// </summary>
        ///// <param name="reqitem"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public ActionResult UpdateRequestList_DeptChange(DeptView reqitemlist, int prev_orderingDept)
        //{


        //    using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //    //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //    {

        //        db.Database.CommandTimeout = 10000;
        //        //    //code to update the dept (ordering stage dept update) for all the request present so far in the reviewed list
        //        List<RequestItems_Table> item_toupdate = db.RequestItems_Table.ToList().FindAll(x => x.ApprovedSH == true).FindAll(x => x.DEPT.Trim().Contains(prev_orderingDept.ToString()));
        //        foreach (RequestItems_Table item in item_toupdate)
        //        {

        //            item.BU = item.BU;
        //            item.Category = item.Category;
        //            item.Comments = item.Comments;
        //            item.CostElement = item.CostElement;
        //            item.BudgetCode = item.BudgetCode;
        //            item.DEPT = reqitemlist.OrderingDept.ToString();

        //            //get old group name
        //            //check for groups with dept == new dept 
        //            //var oldgroupname1 = BudgetingController.lstGroups.Find(a => a.ID == int.Parse(item.Group)).Group;
        //            //var groups_ofnewdept = BudgetingController.lstGroups.FindAll(g => g.Dept == int.Parse(item.DEPT));
        //            //string newgroupname1 = String.Empty;
        //            //if (groups_ofnewdept.Find(b => b.Group.Trim().Contains(oldgroupname1.Trim())) != null || groups_ofnewdept.Find(c => oldgroupname1.Trim().Contains(c.Group.Trim())) != null)
        //            //{
        //            //    if (groups_ofnewdept/*.FindAll(a => a.Outdated == false)*/.Find(b => b.Group.Trim().Contains(oldgroupname1.Trim())) == null)
        //            //    {
        //            //        newgroupname1 = groups_ofnewdept/*.FindAll(a => a.Outdated == false)*/.Find(c => oldgroupname1.Trim().Contains(c.Group.Trim())).Group;

        //            //    }
        //            //    else
        //            //    {
        //            //        newgroupname1 = groups_ofnewdept/*.FindAll(a => a.Outdated == false)*/.Find(b => b.Group.Trim().Contains(oldgroupname1.Trim())).Group;

        //            //    }


        //            //    var newgroupid = BudgetingController.lstGroups.Find(a => a.Group.Trim().Contains(newgroupname1.Trim())).ID;
        //            //    item.Group = newgroupid.ToString();
        //            //}
        //            //else
        //            item.Group = item.Group;






        //            ////group id -> group name							
        //            ////(not always valid - outdated =0) check the groups not an exact match of old group but similar to old group : group names -> == group name; check whether that has dept as the new dept id-> new group id
        //            ////new dept id
        //            //var oldgroupname = BudgetingController.lstGroups.Find(a => a.ID == int.Parse(item.Group)).Group;
        //            //var groups_notexactto_oldgrp = BudgetingController.lstGroups/*.FindAll(a => a.Outdated == false)*/.FindAll(a => a.Group.Trim() != oldgroupname.Trim());
        //            //string newgroupname = String.Empty;
        //            ////  if (BudgetingController.lstGroups/*.FindAll(a => a.Outdated == false)*/.FindAll(a=>a.Group.Trim() != oldgroupname.Trim()).Find(b => b.Group.Contains(oldgroupname.Trim())) != null)

        //            ////ESP IS1 -> MS ESP IS1   MS ESP IS2 -> ESP IS2
        //            //if (groups_notexactto_oldgrp.Find(b => b.Group.Trim().Contains(oldgroupname.Trim())) != null  || groups_notexactto_oldgrp.Find(c=>oldgroupname.Trim().Contains(c.Group.Trim())) != null)
        //            //{
        //            //    if (groups_notexactto_oldgrp/*.FindAll(a => a.Outdated == false)*/.Find(b => b.Group.Trim().Contains(oldgroupname.Trim())) == null)
        //            //    {
        //            //        newgroupname = groups_notexactto_oldgrp/*.FindAll(a => a.Outdated == false)*/.Find(c => oldgroupname.Trim().Contains(c.Group.Trim())).Group;

        //            //    }
        //            //    else
        //            //    {
        //            //        newgroupname = groups_notexactto_oldgrp/*.FindAll(a => a.Outdated == false)*/.Find(b => b.Group.Trim().Contains(oldgroupname.Trim())).Group;

        //            //    }


        //            //    var newgroupid = BudgetingController.lstGroups.Find(a => a.Group.Trim().Contains(newgroupname.Trim())).ID;

        //            //    //check if this new grp has dept as the new dept
        //            //    if (BudgetingController.lstGroups.Find(a => a.ID == newgroupid).Dept == int.Parse(item.DEPT) != false)
        //            //    {
        //            //        item.Group = newgroupid.ToString();
        //            //    }
        //            //    else
        //            //        item.Group = item.Group;


        //            //}
        //            //else
        //            //    item.Group = item.Group;

        //            item.ItemName = item.ItemName;
        //            item.OEM = item.OEM;
        //            item.ReqQuantity = item.ReqQuantity;
        //            item.RequestID = item.RequestID;
        //            item.RequestorNT = item.RequestorNT;
        //            item.DHNT = item.DHNT;
        //            item.SHNT = item.SHNT;
        //            item.TotalPrice = item.TotalPrice;
        //            item.UnitPrice = item.UnitPrice;
        //            item.ApprQuantity = item.ApprQuantity == null ? (int)0 : (int)item.ApprQuantity;
        //            item.ApprCost = item.ApprCost == null ? (int)0 : (int)item.ApprCost;
        //            item.ApprovalDH = item.ApprovalDH;
        //            item.ApprovalSH = item.ApprovalSH;
        //            item.ApprovedDH = item.ApprovedDH;
        //            item.ApprovedSH = item.ApprovedSH;


        //            item.RequestDate = item.RequestDate != null ? item.RequestDate : DateTime.Now;
        //            item.SubmitDate = item.SubmitDate != null ? item.SubmitDate : DateTime.Now;
        //            item.DHAppDate = item.DHAppDate != null ? item.DHAppDate : DateTime.Now;
        //            item.SHAppDate = item.SHAppDate != null ? item.SHAppDate : DateTime.Now;

        //            item.OrderID = item.OrderID;
        //            item.OrderPrice = item.OrderPrice;
        //            if (item.OrderStatus != null && item.OrderStatus.Trim() != "")
        //            {
        //                item.OrderStatus = item.OrderStatus;

        //            }
        //            else
        //            {
        //                item.OrderStatus = null;


        //            }
        //            //item.ActualDeliveryDate = item.ActualDeliveryDate != null ? item.ActualDeliveryDate : DateTime.Now;
        //            //item.RequiredDate = item.RequiredDate != null ? item.RequiredDate : DateTime.Now;
        //            //item.RequestOrderDate = item.RequestOrderDate != null ? item.RequestOrderDate : DateTime.Now;
        //            //item.OrderDate = item.OrderDate != null ? item.OrderDate : DateTime.Now;
        //            //item.TentativeDeliveryDate = item.TentativeDeliveryDate != null ? item.TentativeDeliveryDate : DateTime.Now;
        //            if (item.ActualDeliveryDate != null)
        //            {
        //                item.ActualDeliveryDate = item.ActualDeliveryDate;
        //            }
        //            else
        //                item.ActualDeliveryDate = null;
        //            if (item.RequiredDate != null)
        //            {
        //                item.RequiredDate = item.RequiredDate;
        //            }
        //            else
        //                item.RequiredDate = null;
        //            if (item.RequestOrderDate != null)
        //            {
        //                item.RequestOrderDate = item.RequestOrderDate;
        //            }
        //            else
        //                item.RequestOrderDate = null;
        //            if (item.OrderDate != null)
        //            {
        //                item.OrderDate = item.OrderDate;
        //            }
        //            else
        //                item.OrderDate = null;
        //            if (item.TentativeDeliveryDate != null)
        //            {
        //                item.TentativeDeliveryDate = item.TentativeDeliveryDate;
        //            }
        //            else
        //                item.TentativeDeliveryDate = null;



        //            if (item.Fund != null)
        //            {
        //                item.Fund = item.Fund;
        //            }
        //            else
        //            {
        //                item.Fund = BudgetingController.lstFund.Find(fund => fund.Fund.Trim().Equals("F02")).ID.ToString();
        //            }

        //            if (item.RequestOrderDate != null)
        //            {
        //                item.RequestToOrder = true;
        //            }
        //            else
        //            {
        //                item.RequestToOrder = false;
        //            }
        //            if (item.OrderedQuantity != null)
        //            {
        //                item.OrderedQuantity = (int)item.OrderedQuantity;
        //            }
        //            else
        //            {
        //                item.OrderedQuantity = null;
        //            }


        //            item.Customer_Dept = item.Customer_Dept;

        //            item.Customer_Name = item.Customer_Name;

        //            item.BM_Number = item.BM_Number;

        //            item.PIF_ID = item.PIF_ID;

        //            item.Resource_Group_Id = item.Resource_Group_Id;

        //            item.Task_ID = item.Task_ID;



        //            db.Entry(item).State = EntityState.Modified;
        //            db.SaveChanges();

        //            db.Database.CommandTimeout = 10000;
        //        }
        //        return Json(new { message = "Updated Reviewed Requestlist Successfully" }, JsonRequestBehavior.AllowGet);



        //    }
        //}


        ////***************************GROUP MAPPING DETAILS*********************************
        ////*******************DEPT DETAILS***************

        ///// <summary>
        ///// function to fetch the Request Items made by the Requestor during the year chosen for view
        ///// </summary>
        //[HttpGet]
        //public ActionResult GetData_GroupDetails()
        //{


        //    using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //    //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //    {
        //        try
        //        {

        //            List<GroupView> viewList = new List<GroupView>();
        //            viewList = GetData1_GroupDetails();

        //            return Json(new
        //            {

        //                data = viewList
        //            }, JsonRequestBehavior.AllowGet);
        //        }
        //        catch (Exception ex)
        //        {
        //            return Json(new { success = true, message = "Unable to load Group Mapping List, Please Try again later!" }, JsonRequestBehavior.AllowGet);

        //        }

        //    }
        //}


        //public List<GroupView> GetData1_GroupDetails()
        //{

        //    using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //    //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //    {
        //        List<GroupMapping_Table> reqList = new List<GroupMapping_Table>();
        //        reqList = db.GroupMapping_Table.ToList();
        //        List<GroupView> viewList = new List<GroupView>();
        //        foreach (GroupMapping_Table item in reqList)
        //        {
        //            GroupView ritem = new GroupView();


        //            ritem.PlanningGroup = int.Parse(item.PlanningGroup);
        //            ritem.OrderingGroup = int.Parse(item.OrderingGroup);
        //            ritem.Updated_By = item.UpdatedBy;
        //            ritem.ID = item.ID;



        //            viewList.Add(ritem);
        //        }
        //        return viewList;

        //    }
        //}



        //[HttpPost]
        //public ActionResult AddOrEdit_GroupDetails(GroupView req)
        //{
        //    List<GroupView> viewList = new List<GroupView>();


        //    using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //    //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //    {
        //        System.Security.Principal.IPrincipal User = System.Web.HttpContext.Current.User;
        //        string presentUserName = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).EmployeeName;

        //        GroupMapping_Table item = new GroupMapping_Table();

        //        item.PlanningGroup = req.PlanningGroup.ToString();
        //        item.OrderingGroup = req.OrderingGroup.ToString();
        //        item.ID = req.ID;
        //        item.UpdatedBy = presentUserName;



        //        db.Entry(item).State = EntityState.Modified;

        //        db.SaveChanges();

        //        //viewList = GetData1(useryear);

        //        return Json(new { success = true }, JsonRequestBehavior.AllowGet);

        //    }


        //}



        ///// <summary>
        ///// Function to update Group Details in Reviewed Request List
        ///// </summary>
        ///// <param name="reqitem"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public ActionResult UpdateRequestList_GroupChange(GroupView reqitem, int prev_orderingGroup)
        //{


        //    using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //    //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //    {

        //        db.Database.CommandTimeout = 10000;
        //        //    //code to update the Group (ordering stage Group update) for all the request present so far in the reviewed list
        //        List<RequestItems_Table> item_toupdate = db.RequestItems_Table.ToList().FindAll(x => x.ApprovedSH == true).FindAll(x => x.Group.Trim().Contains(prev_orderingGroup.ToString()));
        //        foreach (RequestItems_Table item in item_toupdate)
        //        {

        //            item.BU = item.BU;
        //            item.Category = item.Category;
        //            item.Comments = item.Comments;
        //            item.CostElement = item.CostElement;
        //            item.BudgetCode = item.BudgetCode;
        //            item.Group = reqitem.OrderingGroup.ToString();

        //            item.DEPT = item.DEPT;

        //            item.ItemName = item.ItemName;
        //            item.OEM = item.OEM;
        //            item.ReqQuantity = item.ReqQuantity;
        //            item.RequestID = item.RequestID;
        //            item.RequestorNT = item.RequestorNT;
        //            item.DHNT = item.DHNT;
        //            item.SHNT = item.SHNT;
        //            item.TotalPrice = item.TotalPrice;
        //            item.UnitPrice = item.UnitPrice;
        //            item.ApprQuantity = item.ApprQuantity == null ? (int)0 : (int)item.ApprQuantity;
        //            item.ApprCost = item.ApprCost == null ? (int)0 : (int)item.ApprCost;
        //            item.ApprovalDH = item.ApprovalDH;
        //            item.ApprovalSH = item.ApprovalSH;
        //            item.ApprovedDH = item.ApprovedDH;
        //            item.ApprovedSH = item.ApprovedSH;


        //            item.RequestDate = item.RequestDate != null ? item.RequestDate : DateTime.Now;
        //            item.SubmitDate = item.SubmitDate != null ? item.SubmitDate : DateTime.Now;
        //            item.DHAppDate = item.DHAppDate != null ? item.DHAppDate : DateTime.Now;
        //            item.SHAppDate = item.SHAppDate != null ? item.SHAppDate : DateTime.Now;

        //            item.OrderID = item.OrderID;
        //            item.OrderPrice = item.OrderPrice;
        //            if (item.OrderStatus != null && item.OrderStatus.Trim() != "")
        //            {
        //                item.OrderStatus = item.OrderStatus;

        //            }
        //            else
        //            {
        //                item.OrderStatus = null;


        //            }
        //            //item.ActualDeliveryDate = item.ActualDeliveryDate != null ? item.ActualDeliveryDate : DateTime.Now;
        //            //item.RequiredDate = item.RequiredDate != null ? item.RequiredDate : DateTime.Now;
        //            //item.RequestOrderDate = item.RequestOrderDate != null ? item.RequestOrderDate : DateTime.Now;
        //            //item.OrderDate = item.OrderDate != null ? item.OrderDate : DateTime.Now;
        //            //item.TentativeDeliveryDate = item.TentativeDeliveryDate != null ? item.TentativeDeliveryDate : DateTime.Now;
        //            if (item.ActualDeliveryDate != null)
        //            {
        //                item.ActualDeliveryDate = item.ActualDeliveryDate;
        //            }
        //            else
        //                item.ActualDeliveryDate = null;
        //            if (item.RequiredDate != null)
        //            {
        //                item.RequiredDate = item.RequiredDate;
        //            }
        //            else
        //                item.RequiredDate = null;
        //            if (item.RequestOrderDate != null)
        //            {
        //                item.RequestOrderDate = item.RequestOrderDate;
        //            }
        //            else
        //                item.RequestOrderDate = null;
        //            if (item.OrderDate != null)
        //            {
        //                item.OrderDate = item.OrderDate;
        //            }
        //            else
        //                item.OrderDate = null;
        //            if (item.TentativeDeliveryDate != null)
        //            {
        //                item.TentativeDeliveryDate = item.TentativeDeliveryDate;
        //            }
        //            else
        //                item.TentativeDeliveryDate = null;



        //            if (item.Fund != null)
        //            {
        //                item.Fund = item.Fund;
        //            }
        //            else
        //            {
        //                item.Fund = BudgetingController.lstFund.Find(fund => fund.Fund.Trim().Equals("F02")).ID.ToString();
        //            }

        //            if (item.RequestOrderDate != null)
        //            {
        //                item.RequestToOrder = true;
        //            }
        //            else
        //            {
        //                item.RequestToOrder = false;
        //            }
        //            if (item.OrderedQuantity != null)
        //            {
        //                item.OrderedQuantity = (int)item.OrderedQuantity;
        //            }
        //            else
        //            {
        //                item.OrderedQuantity = null;
        //            }


        //            item.Customer_Dept = item.Customer_Dept;

        //            item.Customer_Name = item.Customer_Name;

        //            item.BM_Number = item.BM_Number;

        //            item.PIF_ID = item.PIF_ID;

        //            item.Resource_Group_Id = item.Resource_Group_Id;

        //            item.Task_ID = item.Task_ID;



        //            db.Entry(item).State = EntityState.Modified;
        //            db.SaveChanges();

        //            db.Database.CommandTimeout = 10000;
        //        }
        //        return Json(new { message = "Updated Reviewed Requestlist Successfully" }, JsonRequestBehavior.AllowGet);



        //    }
        //}


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
                            decimal conversionINRate = db.Currency_Conversion_Table.FirstOrDefault(x => x.Currency.Contains("INR")).ConversionRate_to_USD;
                            decimal conversionEURate = db.Currency_Conversion_Table.FirstOrDefault(x => x.Currency.Contains("EUR")).ConversionRate_to_USD;


                            //var types = dt.AsEnumerable().Skip(1)

                            //   .GroupBy(
                            //    d => new {
                            //        VKM_Year1 = d.Field<string>("VKM_Year").GetType(),
                            //        BU1 = d.Field<string>("BU").GetType(),
                            //        OEM1= d.Field<string>("OEM").GetType(),
                            //        GROUP1 = d.Field<string>("GROUP").GetType(),
                            //        Dept1 = d.Field<string>("Dept").GetType(),
                            //        ItemName1 = d.Field<string>("Item Name").GetType()
                            //    })
                            //     .Select(g => new
                            //     {
                            //         VKM_Year1 = g.Select(pr => pr.Field<string>("VKM_Year")).Last().GetType(),
                            //         BU1 = g.Select(pr => pr.Field<string>("BU")).Last().GetType(),
                            //         OEM1 = g.Select(pr => pr.Field<string>("OEM")).Last().GetType(),
                            //         GROUP1 = g.Select(pr => pr.Field<string>("GROUP")).Last().GetType(),
                            //         Dept1 = g.Select(pr => pr.Field<string>("Dept")).Last().GetType(),
                            //         ItemName1 = g.Select(pr => pr.Field<string>("Item Name")).Last().GetType(),


                            //         OrderID1 = string.Join(",", g.Select(gn => gn.Field<string>("PO Number"))).GetType(),
                            //         POQuantity1 = g.Sum(pr => pr.Field<int>("PO Quantity")).GetType(),
                            //         PIFID1 = g.Select(pr => pr.Field<string>("PIF ID")).Last().GetType(),
                            //         Fund1 = g.Select(pr => pr.Field<string>("Fund")).Last().GetType(),
                            //         CustomerDept1 = g.Select(pr => pr.Field<string>("Fund Center")).Last().GetType(),
                            //         //BudgetCode = g.Select(pr => pr.Field<string>("Budget Code")).Last(),
                            //         Currency1 = g.Select(pr => pr.Field<string>("Currency")).Last().GetType(),
                            //         Netvalue1 = g.Select(pr=>pr.Field<string>("Netvalue")).Last().GetType(),
                            //         POCreatedOn1 = g.Select(pr => pr.Field<string>("PO Created On")).Last().GetType(),
                            //         top = g.Key


                            //     });
                            DataTable dt1 = dt;
                            DateTime defaultDate = new DateTime(1900, 01, 01);
                            //dt1.Rows[0].Delete();
                            //dt1.AcceptChanges();

                           // WriteLog("header 1 deleted");
                            DataTable dt_new = new DataTable();
                            DataTable dt_grpeddetails = new DataTable();
                            WriteLog("new datatable created; adding columns .....");



                            //ORDER SHOULD BE SAME AS IN USER DEFINED TABLE IN SQL
                            dt_new.Columns.Add("VKM_Year", typeof(string));
                            dt_new.Columns.Add("BU", typeof(string));
                            dt_new.Columns.Add("OEM", typeof(string));
                            dt_new.Columns.Add("GROUP", typeof(string));
                            dt_new.Columns.Add("Dept", typeof(string));
                            dt_new.Columns.Add("ItemName", typeof(string));
                            dt_new.Columns.Add("PONumber", typeof(string));
                            dt_new.Columns.Add("OrderedQuantity", typeof(float));
                            dt_new.Columns.Add("PIFID", typeof(string));
                            dt_new.Columns.Add("Fund", typeof(string));
                            dt_new.Columns.Add("FundCenter", typeof(string));
                            dt_new.Columns.Add("Currency", typeof(string));
                            dt_new.Columns.Add("TentativeDt", typeof(string));
                            dt_new.Columns.Add("POCreatedOn", typeof(string));

                            dt_new.Columns.Add("Netvalue", typeof(string));
                            dt_new.Columns.Add("PORemarks", typeof(string));

                            //PO SUB ITEM VIEW ADDITIONAL COLUMNS
                            dt_new.Columns.Add("ItemDescription", typeof(string));
                            dt_new.Columns.Add("POQuantity", typeof(int));
                            dt_new.Columns.Add("UOM", typeof(string));
                            dt_new.Columns.Add("BudgetCode", typeof(string));
                            dt_new.Columns.Add("Plant", typeof(string));
                            dt_new.Columns.Add("VendorName", typeof(string));
                            dt_new.Columns.Add("ActualDt", typeof(string));
                            dt_new.Columns.Add("RequiredDt", typeof(string));
                            dt_new.Columns.Add("POStatus", typeof(string));

                            WriteLog("columns added ; copying data ...");
                            DataRow row1;
                            WriteLog("rows count" + dt1.Rows.Count);
                            WriteLog("[0][0] val: " + dt1.Rows[0][0].ToString());
                            WriteLog("[0][1] val: " + dt1.Rows[0][1].ToString());
                            for (int i = 0; i < dt1.Rows.Count; i++)
                            {
                                WriteLog("copying data : ..." + i);
                                row1 = dt_new.NewRow();


                                row1[0] = dt1.Rows[i][0].ToString();
                                row1[1] = dt1.Rows[i][1].ToString();
                                row1[2] = dt1.Rows[i][2].ToString();
                                row1[3] = dt1.Rows[i][4].ToString();
                                row1[4] = dt1.Rows[i][3].ToString();
                                row1[5] = dt1.Rows[i][5].ToString();
                                row1[6] = dt1.Rows[i][7].ToString();
                                row1[7] = (dt1.Rows[i][6] == DBNull.Value) ? 0 : Convert.ToDouble(dt1.Rows[i][6]);
                                row1[8] = dt1.Rows[i][10].ToString();
                                row1[9] = dt1.Rows[i][12].ToString();
                                row1[10] = dt1.Rows[i][13].ToString();
                                WriteLog("fnd center:" + row1[10]);
                                row1[11] = dt1.Rows[i][21].ToString();

                                var date_replace = dt1.Rows[i][32].ToString().Replace(".", "/");


                                WriteLog("date replace:" + date_replace);
                                //  row1[12] = (dt1.Rows[i][37].ToString().Trim() == "") ? defaultDate : Convert.ToDateTime(dt1.Rows[i][37].ToString().Replace(".","/"));
                                row1[12] = (dt1.Rows[i][37].ToString().Trim() == "") ? string.Empty : dt1.Rows[i][37].ToString().Replace(".", "/");
                                WriteLog("Date" + row1[12]); //37
                                                             // row1[13] = date_replace.Trim() == "" ? defaultDate : Convert.ToDateTime(dt1.Rows[i][32].ToString().Replace(".", "/"));
                                row1[13] = (dt1.Rows[i][32].ToString().Trim() == "") ? string.Empty : dt1.Rows[i][32].ToString().Replace(".", "/");
                                WriteLog("Date" + row1[13]); //37

                                row1[14] = dt1.Rows[i][20].ToString();
                                row1[15] = dt1.Rows[i][44].ToString();

                                row1[16] = dt1.Rows[i][17].ToString();
                                row1[17] = (dt1.Rows[i][18] == DBNull.Value) ? 0 : Convert.ToInt32(dt1.Rows[i][18].ToString());
                                WriteLog("PO Q" + row1[18]); //37

                                row1[18] = dt1.Rows[i][19].ToString();
                                row1[19] = dt1.Rows[i][14].ToString();
                                row1[20] = dt1.Rows[i][25].ToString();
                                row1[21] = dt1.Rows[i][34].ToString();
                                row1[22] =//dt1.Rows[i][38].ToString();
                                (dt1.Rows[i][38].ToString().Trim() == "") ? string.Empty : dt1.Rows[i][38].ToString().Replace(".", "/");
                                row1[23] = (dt1.Rows[i][35].ToString().Trim() == "") ? string.Empty : dt1.Rows[i][35].ToString().Replace(".", "/");
                                WriteLog("Reqd Dt" + row1[23]);

                                row1[24] = dt1.Rows[i][43].ToString();

                                dt_new.Rows.Add(row1);
                            }

                            connection();

                            SqlCommand command = new SqlCommand();

                            command.Connection = conn;
                            command.CommandText = "dbo.[ImportBonaparte]";
                            command.CommandType = CommandType.StoredProcedure;


                            // Add the input parameter and set its properties.

                            SqlParameter parameter2 = new SqlParameter();
                            parameter2.ParameterName = "@UserName";
                            parameter2.SqlDbType = SqlDbType.NVarChar;
                            parameter2.Direction = ParameterDirection.Input;
                            parameter2.Value = presentUserName;


                            SqlParameter parameter1 = new SqlParameter();
                            parameter1.ParameterName = "@Bonaparte_orderlist";
                            parameter1.SqlDbType = SqlDbType.Structured;
                            parameter1.TypeName = "dbo.Bonaparte_orderlist";
                            parameter1.Direction = ParameterDirection.Input;
                            parameter1.Value = dt_new;

                            // Add the parameter to the Parameters collection.
                            command.Parameters.Add(parameter1);
                            command.Parameters.Add(parameter2);

                            OpenConnection();
                            WriteLog("Executing STORED PROCEDURE");
                            command.CommandTimeout = 300; //5 min

                            //ErrorMsg = command.ExecuteScalar().ToString();
                            //WriteLog("ErrorMsg: " + ErrorMsg);
                            command.ExecuteNonQuery();

                            command = new SqlCommand("select top 1  convert(nvarchar(100),Msg) as ErrorMsg from BonaparteLog where Msg like '%ErrorMsg%' order by logtime desc", conn);


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
                                WriteLog("ErrorMsg: " + ErrorMsg1[0]);
                                ErrorMsg = ErrorMsg1[0];
                            }
                            //SqlDataReader dr = command.ExecuteReader();

                            //if (dr.HasRows)
                            //{
                            //    dr.Read();
                            //    ErrorMsg = dr["ErrorMsg"].ToString();
                            //    WriteLog("ErrorMsg: " + ErrorMsg);

                            //}
                            //dr.Close();

                            CloseConnection();

                            WriteLog("data copy from old dt to new dt to avoid datatype ambiguity finished");

                            WriteLog("before groupby");
                            //dt_new.DefaultView.Sort = "PIF ID DESC";
                            //dt_new = dt_new.DefaultView.ToTable();
                            //var xsort = dt_new.Rows[0][12].ToString();
                            //WriteLog("sorted: " + xsort);
                            //var grouped_details = (from DataRow dr in dt_grpeddetails.Rows
                            //                       select new BonaparteGroup()
                            //                       {
                            //                           PONumber = dr["PONumber"].ToString(),
                            //                           OrderedQuantity = (float)dr["OrderedQuantity"],
                            //                           PIFID = dr["PIFID"].ToString(),
                            //                           Fund = dr["Fund"].ToString(),
                            //                           //CustomerDept = g.Select(pr => pr.Field<string>("Fund Center")).ToString(),
                            //                           //CustomerDept_1 = g.Select(pr => pr.Field<double>("Fund Center")).ToString(),
                            //                           CustomerDept = dr["FundCenter"].ToString(),
                            //                           //vvvvv = g.Select(pr => pr.GetType()=="double"),
                            //                           //CustomerDept = g.Select(pr =>(pr.Field<string>("Fund Center")).First().ToString(),
                            //                           //CustomerDept_3 = g.Select(pr => pr.Field<double>("Fund Center")).First().ToString(),
                            //                           //BudgetCode = g.Select(pr => pr.Field<string>("Budget Code")).Last(),
                            //                           //vvv = dt_new.Columns[12].DataType.ToString() == "string" ? "cc" : "bb",

                            //                           OrderPrice = (decimal)dr["Netvalue"],
                            //                           POCreatedOn = (DateTime)dr["POCreatedOn"],
                            //                           OrderStatus = "Ordered",
                            //                           VKM_Year = (int?)dr["VKMYear"],
                            //                           BU = dr["BU"].ToString(),
                            //                           OEM = (string)dr["OEM"].ToString(),
                            //                           GROUP = dr["GROUP"].ToString(),
                            //                           Dept = dr["Dept"].ToString(),
                            //                           ItemName = dr["ItemName"].ToString()

                            //                       });
                            //var grouped_details = dt_new.AsEnumerable()

                            //   .GroupBy(
                            //    d => new
                            //    {
                            //        VKM_Year = d.Field<double>("VKMYear"),
                            //        BU = d.Field<string>("BU"),
                            //        OEM = d.Field<string>("OEM"),
                            //        GROUP = d.Field<string>("GROUP"),
                            //        Dept = d.Field<string>("Dept"),
                            //        ItemName = d.Field<string>("Item Name")

                            //    })
                            //     .Select(g => new
                            //     {
                            //         PONumber = string.Join(",", g.Select(gn => gn.Field<double>("PO Number").ToString())),
                            //         OrderedQuantity = g.Select(pr => pr.Field<double>("Ordered Quantity")).First(),
                            //         PIFID = g.Select(pr => pr.Field<string>("PIF ID")).Last(),
                            //         Fund = g.Select(pr => pr.Field<string>("Fund")).Last(),
                            //         //CustomerDept = g.Select(pr => pr.Field<string>("Fund Center")).ToString(),
                            //         //CustomerDept_1 = g.Select(pr => pr.Field<double>("Fund Center")).ToString(),
                            //         CustomerDept = g.Select(pr => pr.Field<string>("Fund Center")).First().ToString(),
                            //         //vvvvv = g.Select(pr => pr.GetType()=="double"),
                            //         //CustomerDept = g.Select(pr =>(pr.Field<string>("Fund Center")).First().ToString(),
                            //         //CustomerDept_3 = g.Select(pr => pr.Field<double>("Fund Center")).First().ToString(),
                            //         //BudgetCode = g.Select(pr => pr.Field<string>("Budget Code")).Last(),
                            //         vvv = dt_new.Columns[12].DataType.ToString() == "string" ? "cc" : "bb",

                            //         Netvalue = g.Sum(pr => pr.Field<string>("Currency") == "EUR" ? decimal.Parse(pr.Field<string>("Netvalue").Replace(",", ".").Replace(".", "")) * conversionEURate : (pr.Field<string>("Currency") == "USD" ? decimal.Parse(pr.Field<string>("Netvalue").Replace(",", "").Replace(".", "")) : decimal.Parse(pr.Field<string>("Netvalue").Replace(",", "").Replace(".", "")) * conversionINRate)),
                            //         POCreatedOn = g.Select(pr => pr.Field<DateTime>("PO Created On")).Last(),
                            //         OrderStatus = "Ordered",
                            //         ReqGrp = g.Key
                            //     });
                            WriteLog("after groupby");
                            //WriteLog("vkm yr" + grouped_details.First().ReqGrp.VKM_Year);


                            //WriteLog("req fundcenter" + grouped_details.Where(x => x.Fund == "F03").First());
                            //foreach (var req in grouped_details)
                            //{
                            //    WriteLog("dt.Rows.count" + dt_new.Rows.Count);
                            //    WriteLog("req" + req);
                            //    RequestItems_Table_orderview_test1 item = new RequestItems_Table_orderview_test1();
                            //    try
                            //    {
                            //        //item.Fund = BudgetingController.lstFund.Find(cat => cat.Fund.Trim().Replace(" ", "").ToLower().Equals(req.Fund.ToString().Replace(" ", "").Trim().ToLower())).ID.ToString();

                            //        //item.VKM_Year = req.VKM_Year;
                            //        //item.BU = BudgetingController.lstBUs.Find(cat => cat.BU.Trim().Replace(" ", "").ToLower().Equals(req/*.ReqGrp*/.BU.ToString().Replace(" ", "").Trim().ToLower())).ID.ToString();
                            //        //WriteLog("BU entered");
                            //        //item.OEM = BudgetingController.lstOEMs.Find(cat => cat.OEM.Trim().Replace(" ", "").ToLower().Equals(req/*.ReqGrp*/.OEM.ToString().Replace(" ", "").Trim().ToLower())).ID.ToString();
                            //        //WriteLog("OEM");
                            //        //item.DEPT = BudgetingController.lstDEPTs.Find(cat => cat.DEPT.Trim().Replace(" ", "").ToLower().Equals(req/*.ReqGrp*/.Dept.ToString().Replace(" ", "").Trim().ToLower())).ID.ToString();
                            //        //WriteLog("DEPT");
                            //        //item.Group = BudgetingController.lstGroups.Find(cat => cat.Group.Trim().Replace(" ", "").ToLower().Equals(req/*.ReqGrp*/.GROUP.ToString().Replace(" ", "").Trim().ToLower())).ID.ToString();
                            //        //WriteLog("GRP");

                            //        //item.DHNT = BudgetingController.lstUsers.FindAll(user => req/*.ReqGrp*/.Dept.ToString().Replace(" ", "").Trim().ToUpper().Equals(user.Group.ToUpper().Trim())).FirstOrDefault(head => head.Role.ToUpper().Contains("HEAD")).EmployeeName; ;
                            //        //WriteLog("DHNT:");
                            //        //var VKMSPOC_NT = BudgetingController.lstBU_SPOCs.Find(spoc => spoc.BU.Equals(int.Parse(item.BU))).VKMspoc.ToUpper().Trim();
                            //        //item.SHNT = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Contains(VKMSPOC_NT)).EmployeeName;

                            //        //WriteLog("SHNT:");


                            //        //item.OrderedQuantity = (int?)req.OrderedQuantity;
                            //        //(int?)(item.OrderedQuantity == null ? req.POQuantity : item.OrderedQuantity + req.POQuantity);

                            //        WriteLog("order q:");

                            //        item.OrderDate = req.POCreatedOn;

                            //        WriteLog("order date:");
                            //        item.OrderStatus = BudgetingController.lstOrderStatus.Find(cat => cat.OrderStatus.Trim().Replace(" ", "").ToLower().Equals(req.OrderStatus.ToString().Replace(" ", "").Trim().ToLower())).ID.ToString();


                            //        WriteLog("orderstatus:");

                            //        WriteLog("fund:");
                            //        item.PIF_ID = req.PIFID;

                            //        WriteLog("pifid:");
                            //        if (item.Fund.Trim() == "1" || item.Fund.Trim() == "3")
                            //        {
                            //            item.Customer_Dept = req.CustomerDept.ToString();
                            //            item.ItemName = req/*.ReqGrp*/.ItemName;
                            //            item.Category = string.Empty;
                            //            item.CostElement = string.Empty;
                            //        }
                            //        else
                            //        {
                            //            item.ItemName = BudgetingController.lstItems.FindAll(x => x.Deleted == false).Find(cat => cat.Item_Name.Trim().Replace(" ", "").ToLower().Equals(req.ItemName.ToString().Replace(" ", "").Trim().ToLower())).S_No.ToString();
                            //            WriteLog("item:");
                            //            item.Category = BudgetingController.lstItems.Find(x => x.S_No == int.Parse(item.ItemName)).Category;
                            //            WriteLog("category");
                            //            item.CostElement = BudgetingController.lstItems.Find(x => x.S_No == int.Parse(item.ItemName)).Cost_Element;
                            //            WriteLog("COST ELT:");
                            //            item.UnitPrice = (decimal?)BudgetingController.lstItems.Find(x => x.S_No == int.Parse(item.ItemName)).UnitPriceUSD;
                            //            WriteLog("ITEM PRICE:");
                            //        }

                            //        WriteLog("cust dept: & Item details:");

                            //        WriteLog("2.PO Details extracted:");
                            //        WriteLog("3.ItemID: " + item.ItemName + ", BU ID: " + item.BU + ",Dept ID: " + item.DEPT + ",Grp: " + item.Group);
                            //        WriteLog("4.OrderID: " + item.OrderID + ", OrderedQuantity: " + item.OrderedQuantity + ", OrderPrice(in $): " + item.OrderPrice + ",OrderDate: " + item.OrderDate + ", Fund: " + item.Fund);

                            //        if (db.RequestItems_Table_orderview_test1.AsNoTracking().ToList().FindAll(i => i.BU == item.BU && i.OEM == item.OEM && i.DEPT == item.DEPT && i.Group == item.Group && i.ItemName == item.ItemName && i.VKM_Year == item.VKM_Year).Count > 0)
                            //        {
                            //            WriteLog("5.Existing Request fetched");
                            //            //existing item 
                            //            var existing_request = db.RequestItems_Table_orderview_test1.AsNoTracking().ToList().Find(i => i.BU == item.BU && i.OEM == item.OEM && i.DEPT == item.DEPT && i.Group == item.Group && i.ItemName == item.ItemName && i.VKM_Year == item.VKM_Year);
                            //            WriteLog("6.Existing Request ID:" + existing_request.RequestID);

                            //            item.ActualAvailableQuantity = existing_request.ActualAvailableQuantity;
                            //            //                                        item.ActualDeliveryDate = ;
                            //            item.ApprCost = existing_request.ApprCost;
                            //            item.ApprovalDH = existing_request.ApprovalDH;
                            //            item.ApprovalSH = existing_request.ApprovalSH;
                            //            item.ApprovedDH = existing_request.ApprovedDH;
                            //            item.ApprovedSH = existing_request.ApprovedSH;
                            //            item.ApprQuantity = existing_request.ApprQuantity;
                            //            //                                        item.BM_Number = existing_request.BM_Number;
                            //            //item.BU
                            //            //item.Category
                            //            item.Comments = existing_request.Comments;
                            //            //item.CostElement
                            //            //item.Customer_Dept 
                            //            //                                      item.Customer_Name = existing_request.Customer_Name;

                            //            //item.DEPT
                            //            item.DHAppDate = existing_request.DHAppDate;
                            //            item.DHNT = existing_request.DHNT;
                            //            //item.Fund
                            //            //item.Group
                            //            item.HOEView_ActionHistory = existing_request.HOEView_ActionHistory;
                            //            item.isCancelled = existing_request.isCancelled; //sentback
                            //                                                             //item.ItemName
                            //                                                             //item.OEM
                            //                                                             //item.OrderDate  // same po - diff quarter order date ????????
                            //            item.Ordered = existing_request.Ordered;

                            //            WriteLog(".Existing OrderedQ:" + existing_request.OrderedQuantity);

                            //            //item.OrderedQuantity = existing_request.OrderedQuantity != null ? existing_request.OrderedQuantity + item.OrderedQuantity : item.OrderedQuantity;
                            //            WriteLog(".New OrderedQ:" + item.OrderedQuantity);
                            //            WriteLog(".Existing OrderID:" + existing_request.OrderID);
                            //            item.OrderID = existing_request.OrderID == null ? req.PONumber.ToString() : existing_request.OrderID + ", " + req.PONumber.ToString();

                            //            WriteLog("po no:");
                            //            //item.OrderID = existing_request.OrderID.Trim() != null && existing_request.OrderID.Trim() != "" ? existing_request.OrderID + "," + item.OrderID : item.OrderID;
                            //            WriteLog(".NEW OrderID" + item.OrderID);
                            //            WriteLog(".Existing OrderPRICE:" + existing_request.OrderPrice);
                            //            item.OrderPrice = existing_request.OrderPrice == null ? req.OrderPrice : existing_request.OrderPrice + req.OrderPrice;

                            //            WriteLog("order price:");
                            //            //item.OrderPrice = existing_request.OrderPrice != null ? existing_request.OrderPrice + item.OrderPrice : item.OrderPrice;
                            //            WriteLog(".NEW OrderPRICE:" + item.OrderPrice);

                            //            //                                      item.OrderStatus = 
                            //            //item.PIF_ID
                            //            //                                        item.PORaisedBy
                            //            //item.PORemarks = existing_request.PORemarks;
                            //            item.Project = existing_request.Project;
                            //            item.ReqQuantity = existing_request.ReqQuantity;
                            //            item.RequestDate = existing_request.RequestDate;
                            //            item.RequestID = existing_request.RequestID;
                            //            item.RequestOrderDate = existing_request.RequestOrderDate;
                            //            item.RequestorNT = existing_request.RequestorNT;
                            //            item.RequestToOrder = existing_request.RequestToOrder;
                            //            item.RequiredDate = existing_request.RequiredDate;
                            //            //                                        item.Resource_Group_Id
                            //            item.SHAppDate = existing_request.SHAppDate;
                            //            item.SHNT = existing_request.SHNT;
                            //            item.SubmitDate = existing_request.SubmitDate;
                            //            //                                        item.Task_IDs
                            //            //                                        item.TentativeDeliveryDate
                            //            item.TotalPrice = existing_request.TotalPrice;
                            //            item.UnitPrice = existing_request.UnitPrice;
                            //            //item.VKM_Year = existing_request.VKM_Year;
                            //            //DOUBT - DELIVERY VS REQUESTEED -SHOULD REQUIRED DT BE USED OR TENTATIVE DEL DT - NNED TO CHANGe FROM VKM2022 ; required date for unplAnned f02?
                            //            WriteLog("7.All details mapped");
                            //            //db.Entry(item).State = EntityState.Modified;
                            //            //db.RequestItems_Table_orderview_test1.Attach(item);
                            //            //if (item != null)
                            //            //{
                            //            //    WriteLog("GOING TO DETACH");
                            //            //    db.Entry(item).State = EntityState.Detached;
                            //            //    WriteLog(" DETACHed");
                            //            //}
                            //            db.Entry(item).State = EntityState.Modified;
                            //            WriteLog("modified");
                            //            save = db.SaveChanges();
                            //            WriteLog("8.Saved" + save);






                            //        }
                            //        else
                            //        {
                            //            WriteLog("9.New PO entry");
                            //            //new item - f03/f01 or unplanned f02
                            //            //item.ActualAvailableQuantity = ;
                            //            //                                        item.ActualDeliveryDate = ;
                            //            item.ApprCost = 0;
                            //            item.ApprovalDH = true;
                            //            item.ApprovalSH = true;
                            //            item.ApprovedDH = true;
                            //            item.ApprovedSH = true;
                            //            item.ApprQuantity = 0;
                            //            //                                       item.BM_Number = existing_request.BM_Number;
                            //            //item.BU
                            //            //item.Category
                            //            //item.Comments = existing_request.Comments;
                            //            //item.CostElement
                            //            //                                        item.Customer_Dept
                            //            //                                       item.Customer_Name
                            //            //item.DEPT
                            //            item.DHAppDate = item.Fund.Trim() == "2" ? DateTime.Now.AddYears(-1) : DateTime.Now;
                            //            //item.DHNT = item.DHNT;
                            //            //item.Group
                            //            //item.HOEView_ActionHistory = existing_request.HOEView_ActionHistory;
                            //            //item.isCancelled = existing_request.isCancelled;
                            //            //item.ItemName
                            //            //item.OEM
                            //            //item.OrderDate
                            //            //item.Ordered
                            //            //item.OrderedQuantity


                            //            //item.OrderID = existing_request.OrderID.Trim() != null && existing_request.OrderID.Trim() != "" ? existing_request.OrderID + "," + item.OrderID : item.OrderID;
                            //            //item.OrderPrice = existing_request.OrderPrice != null ? existing_request.OrderPrice + item.OrderPrice : item.OrderPrice;

                            //            //                                        item.OrderStatus
                            //            //item.PIF_ID
                            //            //                                       item.PORaisedBy
                            //            //item.PORemarks
                            //            //item.Project = existing_request.Project;
                            //            //item.ReqQuantity = existing_request.ReqQuantity;
                            //            item.RequestDate = item.Fund.Trim() == "2" ? DateTime.Now.AddYears(-1) : DateTime.Now;
                            //            //item.RequestID = existing_request.RequestID;
                            //            item.RequestOrderDate = item.Fund.Trim() == "2" ? DateTime.Now.AddYears(-1) : DateTime.Now;
                            //            item.RequestorNT = presentUserName;
                            //            item.RequestToOrder = true;
                            //            //                                       item.RequiredDate
                            //            //                                       item.Resource_Group_Id
                            //            item.SHAppDate = item.Fund.Trim() == "2" ? DateTime.Now.AddYears(-1) : DateTime.Now;
                            //            //item.SHNT = existing_request.SHNT;
                            //            item.SubmitDate = item.Fund.Trim() == "2" ? DateTime.Now.AddYears(-1) : DateTime.Now;
                            //            //                                        item.Task_ID
                            //            //                                       item.TentativeDeliveryDate
                            //            item.TotalPrice = 0;
                            //            //item.UnitPrice = existing_request.UnitPrice;
                            //            //item.VKM_Year = existing_request.VKM_Year;
                            //            WriteLog("10.PO Details entered");
                            //            int milliseconds = 500;
                            //            Thread.Sleep(milliseconds);
                            //            db.RequestItems_Table_orderview_test1.Add(item);
                            //            db.SaveChanges();
                            //            WriteLog("11.Saved");
                            //        }



                            //    }
                            //    catch (Exception ex)
                            //    {
                            //        errcount++;
                            //        WriteErrorLog("Error: " + ex.Message.ToString());
                            //        WriteErrorLog("Error: " + ex.InnerException.ToString());



                            //    }

                            //}


                            //foreach (var req in grouped_details)
                            //{
                            //    WriteLog("dt.Rows.count" + dt_new.Rows.Count);
                            //    WriteLog("req" + req);
                            //    RequestItems_Table_orderview_test1 item = new RequestItems_Table_orderview_test1();
                            //    try
                            //    {
                            //        item.Fund = BudgetingController.lstFund.Find(cat => cat.Fund.Trim().Replace(" ", "").ToLower().Equals(req.Fund.ToString().Replace(" ", "").Trim().ToLower())).ID.ToString();

                            //        item.VKM_Year = (int?)req.ReqGrp.VKM_Year;
                            //        item.BU = BudgetingController.lstBUs.Find(cat => cat.BU.Trim().Replace(" ", "").ToLower().Equals(req.ReqGrp.BU.ToString().Replace(" ", "").Trim().ToLower())).ID.ToString();
                            //        WriteLog("BU entered");
                            //        item.OEM = BudgetingController.lstOEMs.Find(cat => cat.OEM.Trim().Replace(" ", "").ToLower().Equals(req.ReqGrp.OEM.ToString().Replace(" ", "").Trim().ToLower())).ID.ToString();
                            //        WriteLog("OEM");
                            //        item.DEPT = BudgetingController.lstDEPTs.Find(cat => cat.DEPT.Trim().Replace(" ", "").ToLower().Equals(req.ReqGrp.Dept.ToString().Replace(" ", "").Trim().ToLower())).ID.ToString();
                            //        WriteLog("DEPT");
                            //        item.Group = BudgetingController.lstGroups.Find(cat => cat.Group.Trim().Replace(" ", "").ToLower().Equals(req.ReqGrp.GROUP.ToString().Replace(" ", "").Trim().ToLower())).ID.ToString();
                            //        WriteLog("GRP");

                            //item.DHNT = BudgetingController.lstUsers.FindAll(user => req.ReqGrp.Dept.ToString().Replace(" ", "").Trim().ToUpper().Equals(user.Group.ToUpper().Trim())).FirstOrDefault(head => head.Role.ToUpper().Contains("HEAD")).EmployeeName; ;
                            //WriteLog("DHNT:");
                            //var VKMSPOC_NT = BudgetingController.lstBU_SPOCs.Find(spoc => spoc.BU.Equals(int.Parse(item.BU))).VKMspoc.ToUpper().Trim();
                            //item.SHNT = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Contains(VKMSPOC_NT)).EmployeeName;

                            //        WriteLog("SHNT:");


                            //        item.OrderedQuantity = (int?)req.OrderedQuantity;
                            //            //(int?)(item.OrderedQuantity == null ? req.POQuantity : item.OrderedQuantity + req.POQuantity);

                            //        WriteLog("order q:");

                            //        item.OrderDate = req.POCreatedOn;

                            //        WriteLog("order date:");
                            //        item.OrderStatus = BudgetingController.lstOrderStatus.Find(cat => cat.OrderStatus.Trim().Replace(" ", "").ToLower().Equals(req.OrderStatus.ToString().Replace(" ", "").Trim().ToLower())).ID.ToString();


                            //        WriteLog("orderstatus:");

                            //        WriteLog("fund:");
                            //        item.PIF_ID = req.PIFID;

                            //        WriteLog("pifid:");
                            //        if (item.Fund.Trim() == "1" || item.Fund.Trim() == "3")
                            //        {
                            //            item.Customer_Dept = req.CustomerDept.ToString();
                            //            item.ItemName = req.ReqGrp.ItemName;
                            //            item.Category = string.Empty;
                            //            item.CostElement = string.Empty;
                            //        }
                            //        else
                            //        {
                            //            item.ItemName = BudgetingController.lstItems.FindAll(x => x.Deleted == false).Find(cat => cat.Item_Name.Trim().Replace(" ", "").ToLower().Equals(req.ReqGrp.ItemName.ToString().Replace(" ", "").Trim().ToLower())).S_No.ToString();
                            //            WriteLog("item:");
                            //            item.Category = BudgetingController.lstItems.Find(x => x.S_No == int.Parse(item.ItemName)).Category;
                            //            WriteLog("category");
                            //            item.CostElement = BudgetingController.lstItems.Find(x => x.S_No == int.Parse(item.ItemName)).Cost_Element;
                            //            WriteLog("COST ELT:");
                            //            item.UnitPrice = (decimal?)BudgetingController.lstItems.Find(x => x.S_No == int.Parse(item.ItemName)).UnitPriceUSD;
                            //            WriteLog("ITEM PRICE:");
                            //        }

                            //        WriteLog("cust dept: & Item details:");

                            //        WriteLog("2.PO Details extracted:");
                            //        WriteLog("3.ItemID: " + item.ItemName + ", BU ID: " + item.BU + ",Dept ID: " + item.DEPT + ",Grp: " + item.Group);
                            //        WriteLog("4.OrderID: " + item.OrderID + ", OrderedQuantity: " + item.OrderedQuantity + ", OrderPrice(in $): " + item.OrderPrice + ",OrderDate: " + item.OrderDate + ", Fund: " + item.Fund);

                            //        if (db.RequestItems_Table_orderview_test1.AsNoTracking().ToList().FindAll(i => i.BU == item.BU && i.OEM == item.OEM && i.DEPT == item.DEPT && i.Group == item.Group && i.ItemName == item.ItemName && i.VKM_Year == item.VKM_Year).Count > 0)
                            //        {
                            //            WriteLog("5.Existing Request fetched");
                            //            //existing item 
                            //            var existing_request = db.RequestItems_Table_orderview_test1.AsNoTracking().ToList().Find(i => i.BU == item.BU && i.OEM == item.OEM && i.DEPT == item.DEPT && i.Group == item.Group && i.ItemName == item.ItemName && i.VKM_Year == item.VKM_Year);
                            //            WriteLog("6.Existing Request ID:" + existing_request.RequestID);

                            //            item.ActualAvailableQuantity = existing_request.ActualAvailableQuantity;
                            //            //                                        item.ActualDeliveryDate = ;
                            //            item.ApprCost = existing_request.ApprCost;
                            //            item.ApprovalDH = existing_request.ApprovalDH;
                            //            item.ApprovalSH = existing_request.ApprovalSH;
                            //            item.ApprovedDH = existing_request.ApprovedDH;
                            //            item.ApprovedSH = existing_request.ApprovedSH;
                            //            item.ApprQuantity = existing_request.ApprQuantity;
                            //            //                                        item.BM_Number = existing_request.BM_Number;
                            //            //item.BU
                            //            //item.Category
                            //            item.Comments = existing_request.Comments;
                            //            //item.CostElement
                            //            //item.Customer_Dept 
                            //            //                                      item.Customer_Name = existing_request.Customer_Name;

                            //            //item.DEPT
                            //            item.DHAppDate = existing_request.DHAppDate;
                            //            item.DHNT = existing_request.DHNT;
                            //            //item.Fund
                            //            //item.Group
                            //            item.HOEView_ActionHistory = existing_request.HOEView_ActionHistory;
                            //            item.isCancelled = existing_request.isCancelled; //sentback
                            //                                                             //item.ItemName
                            //                                                             //item.OEM
                            //                                                             //item.OrderDate  // same po - diff quarter order date ????????
                            //            item.Ordered = existing_request.Ordered;

                            //            WriteLog(".Existing OrderedQ:" + existing_request.OrderedQuantity);

                            //            //item.OrderedQuantity = existing_request.OrderedQuantity != null ? existing_request.OrderedQuantity + item.OrderedQuantity : item.OrderedQuantity;
                            //            WriteLog(".New OrderedQ:" + item.OrderedQuantity);
                            //            WriteLog(".Existing OrderID:" + existing_request.OrderID);
                            //            item.OrderID = existing_request.OrderID == null ? req.PONumber.ToString() : existing_request.OrderID + ", " + req.PONumber.ToString();

                            //            WriteLog("po no:");
                            //            //item.OrderID = existing_request.OrderID.Trim() != null && existing_request.OrderID.Trim() != "" ? existing_request.OrderID + "," + item.OrderID : item.OrderID;
                            //            WriteLog(".NEW OrderID" + item.OrderID);
                            //            WriteLog(".Existing OrderPRICE:" + existing_request.OrderPrice);
                            //            item.OrderPrice = existing_request.OrderPrice == null ? req.Netvalue : existing_request.OrderPrice + req.Netvalue;

                            //            WriteLog("order price:");
                            //            //item.OrderPrice = existing_request.OrderPrice != null ? existing_request.OrderPrice + item.OrderPrice : item.OrderPrice;
                            //            WriteLog(".NEW OrderPRICE:" + item.OrderPrice);

                            //            //                                      item.OrderStatus = 
                            //            //item.PIF_ID
                            //            //                                        item.PORaisedBy
                            //            //item.PORemarks = existing_request.PORemarks;
                            //            item.Project = existing_request.Project;
                            //            item.ReqQuantity = existing_request.ReqQuantity;
                            //            item.RequestDate = existing_request.RequestDate;
                            //            item.RequestID = existing_request.RequestID;
                            //            item.RequestOrderDate = existing_request.RequestOrderDate;
                            //            item.RequestorNT = existing_request.RequestorNT;
                            //            item.RequestToOrder = existing_request.RequestToOrder;
                            //            item.RequiredDate = existing_request.RequiredDate;
                            //            //                                        item.Resource_Group_Id
                            //            item.SHAppDate = existing_request.SHAppDate;
                            //            item.SHNT = existing_request.SHNT;
                            //            item.SubmitDate = existing_request.SubmitDate;
                            //            //                                        item.Task_IDs
                            //            //                                        item.TentativeDeliveryDate
                            //            item.TotalPrice = existing_request.TotalPrice;
                            //            item.UnitPrice = existing_request.UnitPrice;
                            //            //item.VKM_Year = existing_request.VKM_Year;
                            //            //DOUBT - DELIVERY VS REQUESTEED -SHOULD REQUIRED DT BE USED OR TENTATIVE DEL DT - NNED TO CHANGe FROM VKM2022 ; required date for unplAnned f02?
                            //            WriteLog("7.All details mapped");
                            //            //db.Entry(item).State = EntityState.Modified;
                            //            //db.RequestItems_Table_orderview_test1.Attach(item);
                            //            //if (item != null)
                            //            //{
                            //            //    WriteLog("GOING TO DETACH");
                            //            //    db.Entry(item).State = EntityState.Detached;
                            //            //    WriteLog(" DETACHed");
                            //            //}
                            //            db.Entry(item).State = EntityState.Modified;
                            //            WriteLog("modified");
                            //            save = db.SaveChanges();
                            //            WriteLog("8.Saved" + save);






                            //        }
                            //        else
                            //        {
                            //            WriteLog("9.New PO entry");
                            //            //new item - f03/f01 or unplanned f02
                            //            //item.ActualAvailableQuantity = ;
                            //            //                                        item.ActualDeliveryDate = ;
                            //            item.ApprCost = 0;
                            //            item.ApprovalDH = true;
                            //            item.ApprovalSH = true;
                            //            item.ApprovedDH = true;
                            //            item.ApprovedSH = true;
                            //            item.ApprQuantity = 0;
                            //            //                                       item.BM_Number = existing_request.BM_Number;
                            //            //item.BU
                            //            //item.Category
                            //            //item.Comments = existing_request.Comments;
                            //            //item.CostElement
                            //            //                                        item.Customer_Dept
                            //            //                                       item.Customer_Name
                            //            //item.DEPT
                            //            item.DHAppDate = item.Fund.Trim() == "2" ? DateTime.Now.AddYears(-1) : DateTime.Now;
                            //            //item.DHNT = item.DHNT;
                            //            //item.Group
                            //            //item.HOEView_ActionHistory = existing_request.HOEView_ActionHistory;
                            //            //item.isCancelled = existing_request.isCancelled;
                            //            //item.ItemName
                            //            //item.OEM
                            //            //item.OrderDate
                            //            //item.Ordered
                            //            //item.OrderedQuantity


                            //            //item.OrderID = existing_request.OrderID.Trim() != null && existing_request.OrderID.Trim() != "" ? existing_request.OrderID + "," + item.OrderID : item.OrderID;
                            //            //item.OrderPrice = existing_request.OrderPrice != null ? existing_request.OrderPrice + item.OrderPrice : item.OrderPrice;

                            //            //                                        item.OrderStatus
                            //            //item.PIF_ID
                            //            //                                       item.PORaisedBy
                            //            //item.PORemarks
                            //            //item.Project = existing_request.Project;
                            //            //item.ReqQuantity = existing_request.ReqQuantity;
                            //            item.RequestDate = item.Fund.Trim() == "2" ? DateTime.Now.AddYears(-1) : DateTime.Now;
                            //            //item.RequestID = existing_request.RequestID;
                            //            item.RequestOrderDate = item.Fund.Trim() == "2" ? DateTime.Now.AddYears(-1) : DateTime.Now;
                            //            item.RequestorNT = presentUserName;
                            //            item.RequestToOrder = true;
                            //            //                                       item.RequiredDate
                            //            //                                       item.Resource_Group_Id
                            //            item.SHAppDate = item.Fund.Trim() == "2" ? DateTime.Now.AddYears(-1) : DateTime.Now;
                            //            //item.SHNT = existing_request.SHNT;
                            //            item.SubmitDate = item.Fund.Trim() == "2" ? DateTime.Now.AddYears(-1) : DateTime.Now;
                            //            //                                        item.Task_ID
                            //            //                                       item.TentativeDeliveryDate
                            //            item.TotalPrice = 0;
                            //            //item.UnitPrice = existing_request.UnitPrice;
                            //            //item.VKM_Year = existing_request.VKM_Year;
                            //            WriteLog("10.PO Details entered");
                            //            int milliseconds = 500;
                            //            Thread.Sleep(milliseconds);
                            //            db.RequestItems_Table_orderview_test1.Add(item);
                            //            db.SaveChanges();
                            //            WriteLog("11.Saved");
                            //        }



                            //    }
                            //    catch (Exception ex)
                            //    {
                            //        errcount++;
                            //        WriteErrorLog("Error: " + ex.Message.ToString());
                            //        WriteErrorLog("Error: " + ex.InnerException.ToString());



                            //    }

                            //}


                            //        foreach (DataRow row in dt.AsEnumerable().Skip(1))
                            //        {
                            //            WriteLog("dt.Rows.count" + dt.Rows.Count);
                            //            WriteLog("row" + row);
                            //            RequestItems_Table_orderview_test1 item = new RequestItems_Table_orderview_test1();
                            //            try
                            //            {
                            //                WriteLog("1.Start Transferring data to PO Details module ...");

                            //                if (((row[0] == DBNull.Value) && (row[1] == DBNull.Value) && (row[2] == DBNull.Value) && (row[3] == DBNull.Value) && (row[4] == DBNull.Value) && (row[5] == DBNull.Value) && (row[6] == DBNull.Value)) || (String.IsNullOrWhiteSpace(row[0].ToString()) && String.IsNullOrWhiteSpace(row[1].ToString()) && String.IsNullOrWhiteSpace(row[2].ToString()) && String.IsNullOrWhiteSpace(row[3].ToString()) && String.IsNullOrWhiteSpace(row[4].ToString()) && String.IsNullOrWhiteSpace(row[5].ToString()) && String.IsNullOrWhiteSpace(row[6].ToString())))
                            //                {
                            //                    WriteLog("Empty row");
                            //                    continue;
                            //                }
                            //                else
                            //                {
                            //                    if (String.IsNullOrWhiteSpace(row[0].ToString()) || row[0] == DBNull.Value)
                            //                    {
                            //                        errcount++;
                            //                        if (errcount > 1)
                            //                            msg += "| \n" + "Please enter VKM Year";
                            //                        else
                            //                            msg += "Please enter VKM Year";
                            //                        continue;

                            //                    }
                            //                    if (String.IsNullOrWhiteSpace(row[5].ToString()) || row[5] == DBNull.Value)
                            //                    {
                            //                        errcount++;
                            //                        if (errcount > 1)
                            //                            msg += "| \n" + "Please enter Item Name";
                            //                        else
                            //                            msg += "Please enter Item Name";
                            //                        continue;

                            //                    }
                            //                    if (String.IsNullOrWhiteSpace(row[4].ToString()) || row[3] == DBNull.Value)
                            //                    {
                            //                        errcount++;
                            //                        if (errcount > 1)
                            //                            msg += "| \n" + "Please enter Department Details";
                            //                        else
                            //                            msg += "Please enter Department Details";
                            //                        continue;

                            //                    }
                            //                    item.VKM_Year = int.Parse(row[0].ToString());
                            //                    item.BU = BudgetingController.lstBUs.Find(cat => cat.BU.Trim().Replace(" ", "").ToLower().Equals(row[1].ToString().Replace(" ", "").Trim().ToLower())).ID.ToString();

                            //                    var OEM = BudgetingController.lstOEMs.Find(cat => cat.OEM.Trim().Replace(" ", "").ToLower().Equals(row[2].ToString().Replace(" ", "").Trim().ToLower())).ID.ToString();

                            //                    var DEPT = BudgetingController.lstDEPTs.Find(cat => cat.DEPT.Trim().Replace(" ", "").ToLower().Equals(row[3].ToString().Replace(" ", "").Trim().ToLower())).ID.ToString();

                            //                    var Group = BudgetingController.lstGroups.Find(cat => cat.Group.Trim().Replace(" ", "").ToLower().Equals(row[4].ToString().Replace(" ", "").Trim().ToLower())).ID.ToString();

                            //                    var ItemName = BudgetingController.lstItems.FindAll(x => x.Deleted == false).Find(cat => cat.Item_Name.Trim().Replace(" ", "").ToLower().Equals(row[5].ToString().Replace(" ", "").Trim().ToLower())).S_No.ToString();

                            //                    var requestid_map = db.RequestItems_Table_orderview_test1.AsNoTracking().ToList().Find(i => i.BU == item.BU && i.OEM == OEM && i.DEPT == DEPT && i.Group == Group && i.ItemName == ItemName && i.VKM_Year == item.VKM_Year).RequestID;
                            //                    item.RequestID = requestid_map;
                            //                    item.Budget_Code = row[14].ToString();
                            //                    item.Fund = BudgetingController.lstFund.Find(cat => cat.Fund.Trim().Replace(" ", "").ToLower().Equals(row[11].ToString().Replace(" ", "").Trim().ToLower())).ID.ToString();
                            //                    item.ItemDescription = row[17].ToString();
                            //                    item.PONo = row[7].ToString();
                            //                    item.OrderedQuantity = int.Parse(row[18].ToString());
                            //                    item.UOM = row[19].ToString();
                            //                    item.Currency = row[21].ToString();
                            //                    item.Netvalue = row[20].ToString();
                            //                    item.UnitPrice = row[].ToString();
                            //                    item.ActualAmount = row[39].ToString();
                            //                    item.NegotiatedAmount = row[40].ToString();
                            //                    item.Savings = row[41].ToString();
                            //                    item.Plant = row[25].ToString();
                            //                    item.OrderDate = DateTime.Parse(row[32].ToString());
                            //                    item.Vendor = row[34].ToString();
                            //                    item.DeliveredQuantityRequested = row[35].ToString();
                            //                    item.TentativeDeliveryDate = row[36].ToString();
                            //                    item.ActualDeliveryDate = row[37].ToString();
                            //                    item.DifferenceInDelivery = row[38].ToString();
                            //                    item.OrderStatus = BudgetingController.lstOrderStatus.Find(cat => cat.OrderStatus.Trim().Replace(" ", "").ToLower().Equals(row[42].ToString().Replace(" ", "").Trim().ToLower())).ID.ToString();




                            //                    item.OrderPrice = decimal.Parse(row[17].ToString().Replace(",", ".").Replace(".", "")) * conversionINRate;

                            //                    item.PIF_ID = row[7].ToString();
                            //                    if (item.Fund.Trim() == "1" || item.Fund.Trim() == "3")
                            //                        item.Customer_Dept = row[10].ToString();

                            //                    WriteLog("2.PO Details extracted:");
                            //                    WriteLog("3.ItemID: " + item.ItemName + ", BU ID: " + item.BU + ",Dept ID: " + item.DEPT + ",Grp: " + item.Group);
                            //                    WriteLog("4.OrderID: " + item.OrderID + ", OrderedQuantity: " + item.OrderedQuantity + ", OrderPrice(in $): " + item.OrderPrice + ",OrderDate: " + item.OrderDate + ", Fund: " + item.Fund);
                            //                }
                            //                    //            if (db.RequestItems_Table_orderview_test1.AsNoTracking().ToList().FindAll(i => i.BU == item.BU && i.OEM == item.OEM && i.DEPT == item.DEPT && i.Group == item.Group && i.ItemName == item.ItemName && i.VKM_Year == item.VKM_Year).Count > 0)
                            //                    //            {
                            //                    //                WriteLog("5.Existing Request fetched");
                            //                    //                //existing item 
                            //                    //                var existing_request = db.RequestItems_Table_orderview_test1.AsNoTracking().ToList().Find(i => i.BU == item.BU && i.OEM == item.OEM && i.DEPT == item.DEPT && i.Group == item.Group && i.ItemName == item.ItemName && i.VKM_Year == item.VKM_Year);
                            //                    //                //var existing_request_list = similar_requestlist.Where(i => i.BU == item.BU && i.OEM == item.OEM && i.DEPT == item.DEPT && i.Group == item.Group && i.ItemName == item.ItemName && i.VKM_Year == item.VKM_Year).ToList() ;
                            //                    //                //var existing_request = db.RequestItems_Table_orderview_test1.Find(existing_requestid);
                            //                    //                WriteLog("6.Existing Request ID:" + existing_request.RequestID);

                            //                    //                item.ActualAvailableQuantity = existing_request.ActualAvailableQuantity;
                            //                    //                //                                        item.ActualDeliveryDate = ;
                            //                    //                item.ApprCost = existing_request.ApprCost;
                            //                    //                item.ApprovalDH = existing_request.ApprovalDH;
                            //                    //                item.ApprovalSH = existing_request.ApprovalSH;
                            //                    //                item.ApprovedDH = existing_request.ApprovedDH;
                            //                    //                item.ApprovedSH = existing_request.ApprovedSH;
                            //                    //                item.ApprQuantity = existing_request.ApprQuantity;
                            //                    //                //                                        item.BM_Number = existing_request.BM_Number;
                            //                    //                //item.BU
                            //                    //                //item.Category
                            //                    //                item.Comments = existing_request.Comments;
                            //                    //                //item.CostElement
                            //                    //                //item.Customer_Dept 
                            //                    //                //                                      item.Customer_Name = existing_request.Customer_Name;

                            //                    //                //item.DEPT
                            //                    //                item.DHAppDate = existing_request.DHAppDate;
                            //                    //                item.DHNT = existing_request.DHNT;
                            //                    //                //item.Fund
                            //                    //                //item.Group
                            //                    //                item.HOEView_ActionHistory = existing_request.HOEView_ActionHistory;
                            //                    //                item.isCancelled = existing_request.isCancelled; //sentback
                            //                    //                //item.ItemName
                            //                    //                //item.OEM
                            //                    //                //item.OrderDate  // same po - diff quarter order date ????????
                            //                    //                item.Ordered = existing_request.Ordered;

                            //                    //                WriteLog(".Existing OrderedQ:" + existing_request.OrderedQuantity);

                            //                    //                item.OrderedQuantity = existing_request.OrderedQuantity != null ? existing_request.OrderedQuantity + item.OrderedQuantity : item.OrderedQuantity;
                            //                    //                WriteLog(".New OrderedQ:" + item.OrderedQuantity);
                            //                    //                WriteLog(".Existing OrderID:" + existing_request.OrderID);

                            //                    //                item.OrderID = existing_request.OrderID.Trim() != null && existing_request.OrderID.Trim() != "" ? existing_request.OrderID + "," + item.OrderID : item.OrderID;
                            //                    //                WriteLog(".NEW OrderID" + item.OrderID);
                            //                    //                WriteLog(".Existing OrderPRICE:" + existing_request.OrderPrice);

                            //                    //                item.OrderPrice = existing_request.OrderPrice != null ? existing_request.OrderPrice + item.OrderPrice : item.OrderPrice;
                            //                    //                WriteLog(".NEW OrderPRICE:" + item.OrderPrice);

                            //                    //                //                                      item.OrderStatus = 
                            //                    //                //item.PIF_ID
                            //                    //                //                                        item.PORaisedBy
                            //                    //                //item.PORemarks = existing_request.PORemarks;
                            //                    //                item.Project = existing_request.Project;
                            //                    //                item.ReqQuantity = existing_request.ReqQuantity;
                            //                    //                item.RequestDate = existing_request.RequestDate;
                            //                    //                item.RequestID = existing_request.RequestID;
                            //                    //                item.RequestOrderDate = existing_request.RequestOrderDate;
                            //                    //                item.RequestorNT = existing_request.RequestorNT;
                            //                    //                item.RequestToOrder = existing_request.RequestToOrder;
                            //                    //                item.RequiredDate = existing_request.RequiredDate;
                            //                    //                //                                        item.Resource_Group_Id
                            //                    //                item.SHAppDate = existing_request.SHAppDate;
                            //                    //                item.SHNT = existing_request.SHNT;
                            //                    //                item.SubmitDate = existing_request.SubmitDate;
                            //                    //                //                                        item.Task_IDs
                            //                    //                //                                        item.TentativeDeliveryDate
                            //                    //                item.TotalPrice = existing_request.TotalPrice;
                            //                    //                item.UnitPrice = existing_request.UnitPrice;
                            //                    //                //item.VKM_Year = existing_request.VKM_Year;
                            //                    //                //DOUBT - DELIVERY VS REQUESTEED -SHOULD REQUIRED DT BE USED OR TENTATIVE DEL DT - NNED TO CHANGe FROM VKM2022 ; required date for unplAnned f02?
                            //                    //                WriteLog("7.All details mapped");
                            //                    //                //db.Entry(item).State = EntityState.Modified;
                            //                    //                //db.RequestItems_Table_orderview_test1.Attach(item);
                            //                    //                //if (item != null)
                            //                    //                //{
                            //                    //                //    WriteLog("GOING TO DETACH");
                            //                    //                //    db.Entry(item).State = EntityState.Detached;
                            //                    //                //    WriteLog(" DETACHed");
                            //                    //                //}
                            //                    //                db.Entry(item).State = EntityState.Modified;
                            //                    //                WriteLog("modified");
                            //                    //                save = db.SaveChanges();
                            //                    //                WriteLog("8.Saved" + save);

                            //                    //            }
                            //                    //            else
                            //                    //            {
                            //                    //                WriteLog("9.New PO entry");
                            //                    //                //new item - f03/f01 or unplanned f02
                            //                    //                //item.ActualAvailableQuantity = ;
                            //                    //                //                                        item.ActualDeliveryDate = ;
                            //                    //                item.ApprCost = 0;
                            //                    //                item.ApprovalDH = true;
                            //                    //                item.ApprovalSH = true;
                            //                    //                item.ApprovedDH = true;
                            //                    //                item.ApprovedSH = true;
                            //                    //                item.ApprQuantity = 0;
                            //                    //                //                                       item.BM_Number = existing_request.BM_Number;
                            //                    //                //item.BU
                            //                    //                //item.Category
                            //                    //                //item.Comments = existing_request.Comments;
                            //                    //                //item.CostElement
                            //                    //                //                                        item.Customer_Dept
                            //                    //                //                                       item.Customer_Name
                            //                    //                //item.DEPT
                            //                    //                item.DHAppDate = item.Fund.Trim() == "2" ? DateTime.Now.AddYears(-1) : DateTime.Now;
                            //                    //                //item.DHNT = item.DHNT;
                            //                    //                //item.Group
                            //                    //                //item.HOEView_ActionHistory = existing_request.HOEView_ActionHistory;
                            //                    //                //item.isCancelled = existing_request.isCancelled;
                            //                    //                //item.ItemName
                            //                    //                //item.OEM
                            //                    //                //item.OrderDate
                            //                    //                //item.Ordered
                            //                    //                //item.OrderedQuantity


                            //                    //                //item.OrderID = existing_request.OrderID.Trim() != null && existing_request.OrderID.Trim() != "" ? existing_request.OrderID + "," + item.OrderID : item.OrderID;
                            //                    //                //item.OrderPrice = existing_request.OrderPrice != null ? existing_request.OrderPrice + item.OrderPrice : item.OrderPrice;

                            //                    //                //                                        item.OrderStatus
                            //                    //                //item.PIF_ID
                            //                    //                //                                       item.PORaisedBy
                            //                    //                //item.PORemarks
                            //                    //                //item.Project = existing_request.Project;
                            //                    //                //item.ReqQuantity = existing_request.ReqQuantity;
                            //                    //                item.RequestDate = item.Fund.Trim() == "2" ? DateTime.Now.AddYears(-1) : DateTime.Now;
                            //                    //                //item.RequestID = existing_request.RequestID;
                            //                    //                item.RequestOrderDate = item.Fund.Trim() == "2" ? DateTime.Now.AddYears(-1) : DateTime.Now;
                            //                    //                item.RequestorNT = presentUserName;
                            //                    //                item.RequestToOrder = true;
                            //                    //                //                                       item.RequiredDate
                            //                    //                //                                       item.Resource_Group_Id
                            //                    //                item.SHAppDate = item.Fund.Trim() == "2" ? DateTime.Now.AddYears(-1) : DateTime.Now;
                            //                    //                //item.SHNT = existing_request.SHNT;
                            //                    //                item.SubmitDate = item.Fund.Trim() == "2" ? DateTime.Now.AddYears(-1) : DateTime.Now;
                            //                    //                //                                        item.Task_ID
                            //                    //                //                                       item.TentativeDeliveryDate
                            //                    //                item.TotalPrice = 0;
                            //                    //                //item.UnitPrice = existing_request.UnitPrice;
                            //                    //                //item.VKM_Year = existing_request.VKM_Year;
                            //                    //                WriteLog("10.PO Details entered");
                            //                    //                int milliseconds = 500;
                            //                    //                Thread.Sleep(milliseconds);
                            //                    //                db.RequestItems_Table_orderview_test1.Add(item);
                            //                    //                db.SaveChanges();
                            //                    //                WriteLog("11.Saved");
                            //                    //            }


                            //                    //        }


                            //                    //    }
                            //                    //    catch (Exception ex)
                            //                    //    {
                            //                    //        errcount++;
                            //                    //        WriteErrorLog("Error: " + ex.Message.ToString());
                            //                    //        WriteErrorLog("Error: " + ex.InnerException.ToString());

                            //                    //        if (errcount > 1)
                            //                    //            msg += " | \n" + "Empty/invalid cell found: " + "Row Details: Dept - " + row[3].ToString() + " Item - " + row[5].ToString();
                            //                    //        else
                            //                    //            msg += "Empty/invalid cell found: " + "Row Details: Dept - " + row[3].ToString() + " Item - " + row[5].ToString();


                            //                    //    }

                            //                    //}


                            //                }
                            //}
                        }
                    }
                }
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
            return Json(new { success = true, save, errormsg = ErrorMsg }, JsonRequestBehavior.AllowGet);
        }





        //[HttpPost]
        //public ActionResult Index_import(HttpPostedFileBase postedFile)
        //{
        //    int save = 0;
        //    try
        //    {

        //        if (postedFile != null)
        //        {

        //            string fileExtension = Path.GetExtension(postedFile.FileName);

        //            //Validate uploaded file and return error.
        //            if (fileExtension != ".xls" && fileExtension != ".xlsx")
        //            {

        //                return Json(new { success = false, errormsg = "Please select the excel file with.xls or.xlsx extension" });

        //            }

        //            string folderPath = Server.MapPath("~/UploadedFiles/");
        //            //Check Directory exists else create one
        //            if (!Directory.Exists(folderPath))
        //            {
        //                Directory.CreateDirectory(folderPath);
        //            }

        //            //Save file to folder
        //            var filePath = folderPath + Path.GetFileName(postedFile.FileName);
        //            postedFile.SaveAs(filePath);

        //            //Get file extension

        //            string excelConString = "";

        //            //Get connection string using extension 
        //            switch (fileExtension)
        //            {
        //                //If uploaded file is Excel 1997-2007.
        //                case ".xls":
        //                    excelConString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR=YES'";
        //                    break;
        //                //If uploaded file is Excel 2007 and above
        //                case ".xlsx":
        //                    excelConString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 8.0;HDR=YES'";
        //                    break;
        //            }

        //            //Read data from first sheet of excel into datatable
        //            DataTable dt = new DataTable();
        //            excelConString = string.Format(excelConString, filePath);

        //            //using (OleDbConnection excelOledbConnection = new OleDbConnection(excelConString))
        //            //{
        //            //    using (OleDbCommand excelDbCommand = new OleDbCommand())
        //            //    {
        //            //        using (OleDbDataAdapter excelDataAdapter = new OleDbDataAdapter())
        //            //        {
        //            //            excelDbCommand.Connection = excelOledbConnection;

        //            //            excelOledbConnection.Open();
        //            //            //Get schema from excel sheet
        //            //            //DataTable excelSchema = GetSchemaFromExcel(excelOledbConnection);
        //            //            DataTable excelSchema = excelOledbConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
        //            //            //Get sheet name
        //            //            string sheetName = excelSchema.Rows[0]["TABLE_NAME"].ToString();//give worksheetnames
        //            //            excelOledbConnection.Close();

        //            //            //Read Data from First Sheet.
        //            //            excelOledbConnection.Open();
        //            //            excelDbCommand.CommandText = "SELECT * From [" + sheetName + "]";
        //            //            excelDataAdapter.SelectCommand = excelDbCommand;
        //            //            //Fill datatable from adapter
        //            //            excelDataAdapter.Fill(dt);
        //            //            excelOledbConnection.Close();
        //            //        }
        //            //    }
        //            //}

        //            int errcount = 0;
        //            string Query = "";
        //            int result = 0;
        //            string msg = "";
        //            DataTable dt1 = new DataTable();
        //            dt.Columns.Add("VKM Year", typeof(string));
        //            dt.Columns.Add("GROUP", typeof(string));
        //            dt.Columns.Add("Item Name", typeof(string));
        //            dt.Columns.Add("PO Number", typeof(string));
        //            dt.Columns.Add("Currency", typeof(string));
        //            dt.Columns.Add("PO Price", typeof(decimal));
        //            dt.Columns.Add("PO Date", typeof(string));

        //            dt.Rows.Add("2022", "MS/ESO1-CC", "VM Servers", "85028882","INR","30000","2021-02-05");
        //            dt.Rows.Add("2022", "MS/ESP-IS5-CC", "Maintenace and upgrade of Labcar cards,measuring instruments,repair charges", "85029005","EUR","500","2022-02-05");
        //            dt.Rows.Add("2022", "MS/ESO1-CC", "VM Servers", "85028883", "USD", "1000","2022-02-05");
        //            System.Security.Principal.IPrincipal User = System.Web.HttpContext.Current.User;

        //            using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //            //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //            {
        //                string presentUserName = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).EmployeeName;
        //                decimal conversionINRate = db.Currency_Conversion_Table.FirstOrDefault(x => x.Currency.Contains("INR")).ConversionRate_to_USD;
        //                decimal conversionEURate = /*(decimal)0.014;*/db.Currency_Conversion_Table.FirstOrDefault(x => x.Currency.Contains("EUR")).ConversionRate_to_USD;

        //                var grouped_details = dt.AsEnumerable()
        //                     .GroupBy( d => new{ itemname = d.Field<string>("Item Name"), groupname = d.Field<string>("GROUP"), vkmyear = d.Field<string>("VKM Year")  })
        //                     .Select(g => new
        //                     {
        //                         PO = string.Join(",",
        //                            g.Select(gn => gn.Field<string>("PO Number"))),
        //                         POPrice = g.Sum(pr => pr.Field<string>("Currency") == "EUR" ? pr.Field<decimal>("PO Price") * conversionEURate : pr.Field<decimal>("PO Price") * conversionINRate),
        //                         PODate = g.Select(pr => pr.Field<string>("PO Date")).Last(),
        //                         ReqGrp = g.Key
        //                         //,
        //                         //Teacher = string.Join(",",
        //                         //   g.Select(gt => gt.Field<string>("Qty")))
        //                     });
        //                foreach (var req in grouped_details)
        //                {
        //                    WriteLog("dt.Rows.count" + dt.Rows.Count);
        //                    WriteLog("req" + req);
        //                    RequestItems_Table_orderview_test1 item = new RequestItems_Table_orderview_test1();
        //                    try
        //                    {
        //                        WriteLog("1.Start");



        //                        item.VKM_Year = int.Parse(req.ReqGrp.vkmyear);
        //                                                               WriteLog("vkm yr");
        //                            item.Group = BudgetingController.lstGroups.Find(cat => cat.Group.Trim().Replace(" ", "").ToLower().Equals(req.ReqGrp.groupname.Replace(" ", "").Trim().ToLower())).ID.ToString();
        //                            WriteLog("GRP");
        //                            item.ItemName = BudgetingController.lstItems.FindAll(x => x.Deleted == false).Find(cat => cat.Item_Name.Trim().Replace(" ", "").ToLower().Equals(req.ReqGrp.itemname.Replace(" ", "").Trim().ToLower())).S_No.ToString();
        //                            WriteLog("item:");


        //                        item.OrderID = req.PO;
        //                        //item.OrderPrice = decimal.Parse(req.POPrice);
        //                        item.OrderDate = DateTime.Parse(req.PODate);

        //                        WriteLog("2.PO Details extracted:");
        //                            WriteLog("3.ItemID: " + item.ItemName + ", BU ID: " + item.BU + ",Dept ID: " + item.DEPT + ",Grp: " + item.Group);
        //                            WriteLog("4.OrderID: " + item.OrderID + ", OrderedQuantity: " + item.OrderedQuantity + ", OrderPrice(in $): " + item.OrderPrice + ",OrderDate: " + item.OrderDate + ", Fund: " + item.Fund);

        //                            if (db.RequestItems_Table_orderview_test1.AsNoTracking().ToList().FindAll(i => i.Group == item.Group && i.ItemName == item.ItemName && i.VKM_Year == item.VKM_Year).Count > 0)
        //                            {
        //                                WriteLog("5.Existing Request fetched");
        //                                //existing item 
        //                                var existing_request = db.RequestItems_Table_orderview_test1.AsNoTracking().ToList().Find(i =>  i.Group == item.Group && i.ItemName == item.ItemName && i.VKM_Year == item.VKM_Year);
        //                                //var existing_request_list = similar_requestlist.Where(i => i.BU == item.BU && i.OEM == item.OEM && i.DEPT == item.DEPT && i.Group == item.Group && i.ItemName == item.ItemName && i.VKM_Year == item.VKM_Year).ToList() ;
        //                                //var existing_request = db.RequestItems_Table_orderview_test1.Find(existing_requestid);
        //                                WriteLog("6.Existing Request ID:" + existing_request.RequestID);

        //                                item.ActualAvailableQuantity = existing_request.ActualAvailableQuantity;
        //                                //                                        item.ActualDeliveryDate = ;
        //                                item.ApprCost = existing_request.ApprCost;
        //                                item.ApprovalDH = existing_request.ApprovalDH;
        //                                item.ApprovalSH = existing_request.ApprovalSH;
        //                                item.ApprovedDH = existing_request.ApprovedDH;
        //                                item.ApprovedSH = existing_request.ApprovedSH;
        //                                item.ApprQuantity = existing_request.ApprQuantity;
        //                                //                                        item.BM_Number = existing_request.BM_Number;
        //                                //item.BU
        //                                //item.Category
        //                                item.Comments = existing_request.Comments;
        //                                //item.CostElement
        //                                //item.Customer_Dept 
        //                                //                                      item.Customer_Name = existing_request.Customer_Name;

        //                                //item.DEPT
        //                                item.DHAppDate = existing_request.DHAppDate;
        //                                item.DHNT = existing_request.DHNT;
        //                                //item.Fund
        //                                //item.Group
        //                                item.HOEView_ActionHistory = existing_request.HOEView_ActionHistory;
        //                                item.isCancelled = existing_request.isCancelled; //sentback
        //                                //item.ItemName
        //                                //item.OEM
        //                                //item.OrderDate  // same po - diff quarter order date ????????
        //                                item.Ordered = existing_request.Ordered;


        //                                //                                      item.OrderStatus = 
        //                                //item.PIF_ID
        //                                //                                        item.PORaisedBy
        //                                //item.PORemarks = existing_request.PORemarks;
        //                                item.Project = existing_request.Project;
        //                                item.ReqQuantity = existing_request.ReqQuantity;
        //                                item.RequestDate = existing_request.RequestDate;
        //                                item.RequestID = existing_request.RequestID;
        //                                item.RequestOrderDate = existing_request.RequestOrderDate;
        //                                item.RequestorNT = existing_request.RequestorNT;
        //                                item.RequestToOrder = existing_request.RequestToOrder;
        //                                item.RequiredDate = existing_request.RequiredDate;
        //                                //                                        item.Resource_Group_Id
        //                                item.SHAppDate = existing_request.SHAppDate;
        //                                item.SHNT = existing_request.SHNT;
        //                                item.SubmitDate = existing_request.SubmitDate;
        //                                //                                        item.Task_IDs
        //                                //                                        item.TentativeDeliveryDate
        //                                item.TotalPrice = existing_request.TotalPrice;
        //                                item.UnitPrice = existing_request.UnitPrice;
        //                                //item.VKM_Year = existing_request.VKM_Year;
        //                                //DOUBT - DELIVERY VS REQUESTEED -SHOULD REQUIRED DT BE USED OR TENTATIVE DEL DT - NNED TO CHANGe FROM VKM2022 ; required date for unplAnned f02?
        //                                WriteLog("7.All details mapped");
        //                                //db.Entry(item).State = EntityState.Modified;
        //                                //db.RequestItems_Table_orderview_test1.Attach(item);
        //                                //if (item != null)
        //                                //{
        //                                //    WriteLog("GOING TO DETACH");
        //                                //    db.Entry(item).State = EntityState.Detached;
        //                                //    WriteLog(" DETACHed");
        //                                //}
        //                                db.Entry(item).State = EntityState.Modified;
        //                                WriteLog("modified");
        //                                save = db.SaveChanges();
        //                                WriteLog("8.Saved" + save);

        //                            }
        //                            else
        //                            {
        //                                WriteLog("9.New PO entry");
        //                                //new item - f03/f01 or unplanned f02
        //                                //item.ActualAvailableQuantity = ;
        //                                //                                        item.ActualDeliveryDate = ;
        //                                item.ApprCost = 0;
        //                                item.ApprovalDH = true;
        //                                item.ApprovalSH = true;
        //                                item.ApprovedDH = true;
        //                                item.ApprovedSH = true;
        //                                item.ApprQuantity = 0;
        //                                //                                       item.BM_Number = existing_request.BM_Number;
        //                                //item.BU
        //                                //item.Category
        //                                //item.Comments = existing_request.Comments;
        //                                //item.CostElement
        //                                //                                        item.Customer_Dept
        //                                //                                       item.Customer_Name
        //                                //item.DEPT
        //                                item.DHAppDate = item.Fund.Trim() == "2" ? DateTime.Now.AddYears(-1) : DateTime.Now;
        //                                //item.DHNT = item.DHNT;
        //                                //item.Group
        //                                //item.HOEView_ActionHistory = existing_request.HOEView_ActionHistory;
        //                                //item.isCancelled = existing_request.isCancelled;
        //                                //item.ItemName
        //                                //item.OEM
        //                                //item.OrderDate
        //                                //item.Ordered
        //                                //item.OrderedQuantity


        //                                //item.OrderID = existing_request.OrderID.Trim() != null && existing_request.OrderID.Trim() != "" ? existing_request.OrderID + "," + item.OrderID : item.OrderID;
        //                                //item.OrderPrice = existing_request.OrderPrice != null ? existing_request.OrderPrice + item.OrderPrice : item.OrderPrice;

        //                                //                                        item.OrderStatus
        //                                //item.PIF_ID
        //                                //                                       item.PORaisedBy
        //                                //item.PORemarks
        //                                //item.Project = existing_request.Project;
        //                                //item.ReqQuantity = existing_request.ReqQuantity;
        //                                item.RequestDate = item.Fund.Trim() == "2" ? DateTime.Now.AddYears(-1) : DateTime.Now;
        //                                //item.RequestID = existing_request.RequestID;
        //                                item.RequestOrderDate = item.Fund.Trim() == "2" ? DateTime.Now.AddYears(-1) : DateTime.Now;
        //                                item.RequestorNT = presentUserName;
        //                                item.RequestToOrder = true;
        //                                //                                       item.RequiredDate
        //                                //                                       item.Resource_Group_Id
        //                                item.SHAppDate = item.Fund.Trim() == "2" ? DateTime.Now.AddYears(-1) : DateTime.Now;
        //                                //item.SHNT = existing_request.SHNT;
        //                                item.SubmitDate = item.Fund.Trim() == "2" ? DateTime.Now.AddYears(-1) : DateTime.Now;
        //                                //                                        item.Task_ID
        //                                //                                       item.TentativeDeliveryDate
        //                                item.TotalPrice = 0;
        //                                //item.UnitPrice = existing_request.UnitPrice;
        //                                //item.VKM_Year = existing_request.VKM_Year;
        //                                WriteLog("10.PO Details entered");
        //                                int milliseconds = 500;
        //                                Thread.Sleep(milliseconds);
        //                                db.RequestItems_Table_orderview_test1.Add(item);
        //                                db.SaveChanges();
        //                                WriteLog("11.Saved");
        //                            }



        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        errcount++;
        //                        WriteErrorLog("Error: " + ex.Message.ToString());
        //                        WriteErrorLog("Error: " + ex.InnerException.ToString());



        //                    }

        //                }


        //                //foreach (DataRow row in dt.AsEnumerable().Skip(1))
        //                //{
        //                //    WriteLog("dt.Rows.count" + dt.Rows.Count);
        //                //    WriteLog("row" + row);
        //                //    RequestItems_Table_orderview_test1 item = new RequestItems_Table_orderview_test1();
        //                //    try
        //                //    {
        //                //        WriteLog("1.Start");

        //                //        if (((row[0] == DBNull.Value) && (row[1] == DBNull.Value) && (row[2] == DBNull.Value) && (row[3] == DBNull.Value) && (row[4] == DBNull.Value) && (row[5] == DBNull.Value) && (row[6] == DBNull.Value)) || (String.IsNullOrWhiteSpace(row[0].ToString()) && String.IsNullOrWhiteSpace(row[1].ToString()) && String.IsNullOrWhiteSpace(row[2].ToString()) && String.IsNullOrWhiteSpace(row[3].ToString()) && String.IsNullOrWhiteSpace(row[4].ToString()) && String.IsNullOrWhiteSpace(row[5].ToString()) && String.IsNullOrWhiteSpace(row[6].ToString())))
        //                //        {
        //                //            WriteLog("Empty row");
        //                //            continue;
        //                //        }
        //                //        else
        //                //        {
        //                //            if (String.IsNullOrWhiteSpace(row[0].ToString()) || row[0] == DBNull.Value)
        //                //            {
        //                //                errcount++;
        //                //                if (errcount > 1)
        //                //                    msg += "| \n" + "Please enter VKM Year";
        //                //                else
        //                //                    msg += "Please enter VKM Year";
        //                //                continue;

        //                //            }
        //                //            if (String.IsNullOrWhiteSpace(row[5].ToString()) || row[5] == DBNull.Value)
        //                //            {
        //                //                errcount++;
        //                //                if (errcount > 1)
        //                //                    msg += "| \n" + "Please enter Item Name";
        //                //                else
        //                //                    msg += "Please enter Item Name";
        //                //                continue;

        //                //            }
        //                //            if (String.IsNullOrWhiteSpace(row[3].ToString()) || row[3] == DBNull.Value)
        //                //            {
        //                //                errcount++;
        //                //                if (errcount > 1)
        //                //                    msg += "| \n" + "Please enter Department Details";
        //                //                else
        //                //                    msg += "Please enter Department Details";
        //                //                continue;

        //                //            }
        //                //            item.VKM_Year = int.Parse(row[0].ToString());
        //                //            item.BU = BudgetingController.lstBUs.Find(cat => cat.BU.Trim().Replace(" ", "").ToLower().Equals(row[1].ToString().Replace(" ", "").Trim().ToLower())).ID.ToString();
        //                //            WriteLog("BU entered");
        //                //            item.OEM = BudgetingController.lstOEMs.Find(cat => cat.OEM.Trim().Replace(" ", "").ToLower().Equals(row[2].ToString().Replace(" ", "").Trim().ToLower())).ID.ToString();
        //                //            WriteLog("OEM");
        //                //            item.DEPT = BudgetingController.lstDEPTs.Find(cat => cat.DEPT.Trim().Replace(" ", "").ToLower().Equals(row[3].ToString().Replace(" ", "").Trim().ToLower())).ID.ToString();
        //                //            WriteLog("DEPT");
        //                //            item.Group = BudgetingController.lstGroups.Find(cat => cat.Group.Trim().Replace(" ", "").ToLower().Equals(row[4].ToString().Replace(" ", "").Trim().ToLower())).ID.ToString();
        //                //            WriteLog("GRP");
        //                //            item.ItemName = BudgetingController.lstItems.FindAll(x => x.Deleted == false).Find(cat => cat.Item_Name.Trim().Replace(" ", "").ToLower().Equals(row[5].ToString().Replace(" ", "").Trim().ToLower())).S_No.ToString();
        //                //            WriteLog("item:");
        //                //            item.Category = BudgetingController.lstItems.Find(x => x.S_No == int.Parse(item.ItemName)).Category;
        //                //            WriteLog("category");
        //                //            item.CostElement = BudgetingController.lstItems.Find(x => x.S_No == int.Parse(item.ItemName)).Cost_Element;
        //                //            WriteLog("COST ELT:");
        //                //            item.UnitPrice = (decimal?)BudgetingController.lstItems.Find(x => x.S_No == int.Parse(item.ItemName)).UnitPriceUSD;
        //                //            WriteLog("ITEM PRICE:");
        //                //            item.DHNT = BudgetingController.lstUsers.FindAll(user => row[3].ToString().Replace(" ", "").Trim().ToUpper().Equals(user.Group.ToUpper().Trim())).FirstOrDefault(head => head.Role.ToUpper().Contains("HEAD")).EmployeeName; ;
        //                //            WriteLog("DHNT:");
        //                //            var VKMSPOC_NT = BudgetingController.lstBU_SPOCs.Find(spoc => spoc.BU.Equals(int.Parse(item.BU))).VKMspoc.ToUpper().Trim();
        //                //            item.SHNT = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Contains(VKMSPOC_NT)).EmployeeName;

        //                //            WriteLog("SHNT:");



        //                //            item.OrderID = row[6].ToString();
        //                //            item.OrderedQuantity = int.Parse(row[13].ToString());
        //                //            item.OrderPrice = decimal.Parse(row[17].ToString().Replace(",", ".").Replace(".", "")) * conversionINRate;
        //                //            item.OrderDate = DateTime.Parse(row[19].ToString());
        //                //            item.OrderStatus = BudgetingController.lstOrderStatus.Find(cat => cat.OrderStatus.Trim().Replace(" ", "").ToLower().Equals(row[20].ToString().Replace(" ", "").Trim().ToLower())).ID.ToString();

        //                //            item.Fund = BudgetingController.lstFund.Find(cat => cat.Fund.Trim().Replace(" ", "").ToLower().Equals(row[9].ToString().Replace(" ", "").Trim().ToLower())).ID.ToString();
        //                //            item.PIF_ID = row[7].ToString();
        //                //            if (item.Fund.Trim() == "1" || item.Fund.Trim() == "3")
        //                //                item.Customer_Dept = row[10].ToString();

        //                //            WriteLog("2.PO Details extracted:");
        //                //            WriteLog("3.ItemID: " + item.ItemName + ", BU ID: " + item.BU + ",Dept ID: " + item.DEPT + ",Grp: " + item.Group);
        //                //            WriteLog("4.OrderID: " + item.OrderID + ", OrderedQuantity: " + item.OrderedQuantity + ", OrderPrice(in $): " + item.OrderPrice + ",OrderDate: " + item.OrderDate + ", Fund: " + item.Fund);

        //                //            if (db.RequestItems_Table_orderview_test1.AsNoTracking().ToList().FindAll(i => i.BU == item.BU && i.OEM == item.OEM && i.DEPT == item.DEPT && i.Group == item.Group && i.ItemName == item.ItemName && i.VKM_Year == item.VKM_Year).Count > 0)
        //                //            {
        //                //                WriteLog("5.Existing Request fetched");
        //                //                //existing item 
        //                //                var existing_request = db.RequestItems_Table_orderview_test1.AsNoTracking().ToList().Find(i => i.BU == item.BU && i.OEM == item.OEM && i.DEPT == item.DEPT && i.Group == item.Group && i.ItemName == item.ItemName && i.VKM_Year == item.VKM_Year);
        //                //                //var existing_request_list = similar_requestlist.Where(i => i.BU == item.BU && i.OEM == item.OEM && i.DEPT == item.DEPT && i.Group == item.Group && i.ItemName == item.ItemName && i.VKM_Year == item.VKM_Year).ToList() ;
        //                //                //var existing_request = db.RequestItems_Table_orderview_test1.Find(existing_requestid);
        //                //                WriteLog("6.Existing Request ID:" + existing_request.RequestID);

        //                //                item.ActualAvailableQuantity = existing_request.ActualAvailableQuantity;
        //                //                //                                        item.ActualDeliveryDate = ;
        //                //                item.ApprCost = existing_request.ApprCost;
        //                //                item.ApprovalDH = existing_request.ApprovalDH;
        //                //                item.ApprovalSH = existing_request.ApprovalSH;
        //                //                item.ApprovedDH = existing_request.ApprovedDH;
        //                //                item.ApprovedSH = existing_request.ApprovedSH;
        //                //                item.ApprQuantity = existing_request.ApprQuantity;
        //                //                //                                        item.BM_Number = existing_request.BM_Number;
        //                //                //item.BU
        //                //                //item.Category
        //                //                item.Comments = existing_request.Comments;
        //                //                //item.CostElement
        //                //                //item.Customer_Dept 
        //                //                //                                      item.Customer_Name = existing_request.Customer_Name;

        //                //                //item.DEPT
        //                //                item.DHAppDate = existing_request.DHAppDate;
        //                //                item.DHNT = existing_request.DHNT;
        //                //                //item.Fund
        //                //                //item.Group
        //                //                item.HOEView_ActionHistory = existing_request.HOEView_ActionHistory;
        //                //                item.isCancelled = existing_request.isCancelled; //sentback
        //                //                //item.ItemName
        //                //                //item.OEM
        //                //                //item.OrderDate  // same po - diff quarter order date ????????
        //                //                item.Ordered = existing_request.Ordered;

        //                //                WriteLog(".Existing OrderedQ:" + existing_request.OrderedQuantity);

        //                //                item.OrderedQuantity = existing_request.OrderedQuantity != null ? existing_request.OrderedQuantity + item.OrderedQuantity : item.OrderedQuantity;
        //                //                WriteLog(".New OrderedQ:" + item.OrderedQuantity);
        //                //                WriteLog(".Existing OrderID:" + existing_request.OrderID);

        //                //                item.OrderID = existing_request.OrderID.Trim() != null && existing_request.OrderID.Trim() != "" ? existing_request.OrderID + "," + item.OrderID : item.OrderID;
        //                //                WriteLog(".NEW OrderID" + item.OrderID);
        //                //                WriteLog(".Existing OrderPRICE:" + existing_request.OrderPrice);

        //                //                item.OrderPrice = existing_request.OrderPrice != null ? existing_request.OrderPrice + item.OrderPrice : item.OrderPrice;
        //                //                WriteLog(".NEW OrderPRICE:" + item.OrderPrice);

        //                //                //                                      item.OrderStatus = 
        //                //                //item.PIF_ID
        //                //                //                                        item.PORaisedBy
        //                //                //item.PORemarks = existing_request.PORemarks;
        //                //                item.Project = existing_request.Project;
        //                //                item.ReqQuantity = existing_request.ReqQuantity;
        //                //                item.RequestDate = existing_request.RequestDate;
        //                //                item.RequestID = existing_request.RequestID;
        //                //                item.RequestOrderDate = existing_request.RequestOrderDate;
        //                //                item.RequestorNT = existing_request.RequestorNT;
        //                //                item.RequestToOrder = existing_request.RequestToOrder;
        //                //                item.RequiredDate = existing_request.RequiredDate;
        //                //                //                                        item.Resource_Group_Id
        //                //                item.SHAppDate = existing_request.SHAppDate;
        //                //                item.SHNT = existing_request.SHNT;
        //                //                item.SubmitDate = existing_request.SubmitDate;
        //                //                //                                        item.Task_IDs
        //                //                //                                        item.TentativeDeliveryDate
        //                //                item.TotalPrice = existing_request.TotalPrice;
        //                //                item.UnitPrice = existing_request.UnitPrice;
        //                //                //item.VKM_Year = existing_request.VKM_Year;
        //                //                //DOUBT - DELIVERY VS REQUESTEED -SHOULD REQUIRED DT BE USED OR TENTATIVE DEL DT - NNED TO CHANGe FROM VKM2022 ; required date for unplAnned f02?
        //                //                WriteLog("7.All details mapped");
        //                //                //db.Entry(item).State = EntityState.Modified;
        //                //                //db.RequestItems_Table_orderview_test1.Attach(item);
        //                //                //if (item != null)
        //                //                //{
        //                //                //    WriteLog("GOING TO DETACH");
        //                //                //    db.Entry(item).State = EntityState.Detached;
        //                //                //    WriteLog(" DETACHed");
        //                //                //}
        //                //                db.Entry(item).State = EntityState.Modified;
        //                //                WriteLog("modified");
        //                //                save = db.SaveChanges();
        //                //                WriteLog("8.Saved" + save);

        //                //            }
        //                //            else
        //                //            {
        //                //                WriteLog("9.New PO entry");
        //                //                //new item - f03/f01 or unplanned f02
        //                //                //item.ActualAvailableQuantity = ;
        //                //                //                                        item.ActualDeliveryDate = ;
        //                //                item.ApprCost = 0;
        //                //                item.ApprovalDH = true;
        //                //                item.ApprovalSH = true;
        //                //                item.ApprovedDH = true;
        //                //                item.ApprovedSH = true;
        //                //                item.ApprQuantity = 0;
        //                //                //                                       item.BM_Number = existing_request.BM_Number;
        //                //                //item.BU
        //                //                //item.Category
        //                //                //item.Comments = existing_request.Comments;
        //                //                //item.CostElement
        //                //                //                                        item.Customer_Dept
        //                //                //                                       item.Customer_Name
        //                //                //item.DEPT
        //                //                item.DHAppDate = item.Fund.Trim() == "2" ? DateTime.Now.AddYears(-1) : DateTime.Now;
        //                //                //item.DHNT = item.DHNT;
        //                //                //item.Group
        //                //                //item.HOEView_ActionHistory = existing_request.HOEView_ActionHistory;
        //                //                //item.isCancelled = existing_request.isCancelled;
        //                //                //item.ItemName
        //                //                //item.OEM
        //                //                //item.OrderDate
        //                //                //item.Ordered
        //                //                //item.OrderedQuantity


        //                //                //item.OrderID = existing_request.OrderID.Trim() != null && existing_request.OrderID.Trim() != "" ? existing_request.OrderID + "," + item.OrderID : item.OrderID;
        //                //                //item.OrderPrice = existing_request.OrderPrice != null ? existing_request.OrderPrice + item.OrderPrice : item.OrderPrice;

        //                //                //                                        item.OrderStatus
        //                //                //item.PIF_ID
        //                //                //                                       item.PORaisedBy
        //                //                //item.PORemarks
        //                //                //item.Project = existing_request.Project;
        //                //                //item.ReqQuantity = existing_request.ReqQuantity;
        //                //                item.RequestDate = item.Fund.Trim() == "2" ? DateTime.Now.AddYears(-1) : DateTime.Now;
        //                //                //item.RequestID = existing_request.RequestID;
        //                //                item.RequestOrderDate = item.Fund.Trim() == "2" ? DateTime.Now.AddYears(-1) : DateTime.Now;
        //                //                item.RequestorNT = presentUserName;
        //                //                item.RequestToOrder = true;
        //                //                //                                       item.RequiredDate
        //                //                //                                       item.Resource_Group_Id
        //                //                item.SHAppDate = item.Fund.Trim() == "2" ? DateTime.Now.AddYears(-1) : DateTime.Now;
        //                //                //item.SHNT = existing_request.SHNT;
        //                //                item.SubmitDate = item.Fund.Trim() == "2" ? DateTime.Now.AddYears(-1) : DateTime.Now;
        //                //                //                                        item.Task_ID
        //                //                //                                       item.TentativeDeliveryDate
        //                //                item.TotalPrice = 0;
        //                //                //item.UnitPrice = existing_request.UnitPrice;
        //                //                //item.VKM_Year = existing_request.VKM_Year;
        //                //                WriteLog("10.PO Details entered");
        //                //                int milliseconds = 500;
        //                //                Thread.Sleep(milliseconds);
        //                //                db.RequestItems_Table_orderview_test1.Add(item);
        //                //                db.SaveChanges();
        //                //                WriteLog("11.Saved");
        //                //            }


        //                //        }


        //                //    }
        //                //    catch (Exception ex)
        //                //    {
        //                //        errcount++;
        //                //        WriteErrorLog("Error: " + ex.Message.ToString());
        //                //        WriteErrorLog("Error: " + ex.InnerException.ToString());

        //                //        if (errcount > 1)
        //                //            msg += " | \n" + "Empty/invalid cell found: " + "Row Details: Dept - " + row[3].ToString() + " Item - " + row[5].ToString();
        //                //        else
        //                //            msg += "Empty/invalid cell found: " + "Row Details: Dept - " + row[3].ToString() + " Item - " + row[5].ToString();


        //                //    }

        //                //}


        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        return Json(new { success = false, errormsg = ex.Message.ToString() });
        //    }
        //    finally
        //    {

        //    }
        //    return Json(new { success = true, save }, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult DownloadTemplate()
        {
            string folderPath = Server.MapPath("~/Templates/");
            string filename = "PODetails_Template_new.xlsx";
            var filePath = folderPath + filename;
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, filename);
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
        public ActionResult LookupVKM(string year)
        {
            try
            {
                System.Security.Principal.IPrincipal User = System.Web.HttpContext.Current.User;
                string presentUserDept = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).Department;

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
                lookupData.OEM_List = BudgetingController.lstOEMs;
                lookupData.DEPT_List = BudgetingController.lstDEPTs;
                //if (year.Contains("2020"))
                //    lookupData.Groups_oldList = BudgetingController.lstGroups_old;
                //else
                lookupData.Groups_test = BudgetingController.lstGroups_test;

                using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

                //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

                {
                   // lookupData.Item_List = db.ItemsCostList_Table.ToList().FindAll(x => x.VKM_Year == (int.Parse(year) + 1));
                    //var items = (db.RequestItems_Table.ToList().FindAll(t => t.VKM_Year == 2023 && (t.BU.Contains("2") || t.BU.Contains("4")) && t.ApprovalSH == true).Select(c => c.ItemName)).ToArray();
                    //var list = lookupData.Item_List.Where(x => items.Contains(x.S_No.ToString())).Select(x => new Item_HeaderFilter_Table { value = x.S_No, text = x.Item_Name }).OrderBy(o => o.text).ToList();
                    //lookupData.Item_HeaderFilter = list;
                    //lookupData.itemids = string.Join(",",items);

                }



                lookupData.Category_List = BudgetingController.lstPrdCateg;
                lookupData.CostElement_List = BudgetingController.lstCostElement;
                lookupData.OrderStatus_List = BudgetingController.lstOrderStatus;
                lookupData.Currency_List = BudgetingController.lstCurrency;
                lookupData.Fund_List = BudgetingController.lstFund;
                lookupData.BudgetCodeList = BudgetingController.BudgetCodeList;
                lookupData.SRBuyerList = BudgetingController.SRBuyerList;
                lookupData.SRManagerList = BudgetingController.SRManagerList;
                lookupData.PurchaseType_List = BudgetingController.lstPurchaseType;
                lookupData.UOM_List = BudgetingController.lstUOM;
                lookupData.UnloadingPoint_List = BudgetingController.lstUnloadingPoint;
                lookupData.Order_Type_List = BudgetingController.lstOrderType;
                //lookupData.OrderDescription_List = BudgetingController.lstOrderDescription;


                connection();
                OpenConnection();
                string Query = "Select * from BGSW_BudgetCenter_Table; Select ID, ltrim(rtrim(Description)) as Description from OrderStatusDescription where IsVisible =1  Order  by Description ";

                SqlCommand cmd = new SqlCommand(Query, conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                da.Fill(ds);

                //var presentUserNTID = User.Identity.Name.Split('\\')[1].ToUpper();

                lookupData.BudgetCenter_List = new List<BGSW_BudgetCenter_Table>();
                dt = ds.Tables[0];
                foreach (DataRow row in dt.Rows)
                {
                    BGSW_BudgetCenter_Table item = new BGSW_BudgetCenter_Table();
                    item.BudgetCenter = row["BudgetCenter"].ToString();
                    item.ID = Convert.ToInt32(row["ID"].ToString());
                    lookupData.BudgetCenter_List.Add(item);
                }

                /////// Creation of Order status description list

                lookupData.OrderDescription_List = new List<OrderStatusDescription>();
                dt = new DataTable();
                dt = ds.Tables[1];

                foreach (DataRow row in dt.Rows)
                {
                    OrderStatusDescription item = new OrderStatusDescription();
                    item.Description = row["Description"].ToString().Trim();
                    item.ID = Convert.ToInt32(row["ID"].ToString());
                    lookupData.OrderDescription_List.Add(item);
                }

                BudgetingController.lstOrderDescription = lookupData.OrderDescription_List;
                //lookupData.Item_FilterRow = new List<ItemsCostList_Table>();
                //ItemsCostList_Table i_filterRow1 = new ItemsCostList_Table();


                DataSet dt_for_headerRow = new DataSet();

                //string Query = " Exec [dbo].[GetReqItemsList_VKMSPOCReviews_View] '" + (int.Parse(year) + 1) + "', 'MAE9COB', '" + "LookUp" + "' ";
                Query = " Exec [dbo].[GetReqItemsList_VKMSPOCReviews_View] '" + (int.Parse(year) + 1) + "', '" + User.Identity.Name.Split('\\')[1].ToUpper() + "', '" + "LookUp" + "' ";
                cmd = new SqlCommand(Query, conn);
                da = new SqlDataAdapter(cmd);
                cmd.CommandTimeout = 300;
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
                    lookupData.OrderStatus_HeaderFilter.Add(i_header);
                }

                //foreach (DataRow item in dt_for_headerRow.Tables[8].Rows)
                //{
                //    HeaderFilter_Table i_header = new HeaderFilter_Table();
                //    i_header.text = item["Budget_Code"].ToString();
                //    i_header.value = Convert.ToInt32(item["Budget_Code"].ToString());
                //    lookupData.BudgetCode_HeaderFilter.Add(i_header);
                //}

                //    ItemsCostList_Table i_filterRow = new ItemsCostList_Table();
                //    i_filterRow.Item_Name = item["Item Name"].ToString();
                //    i_filterRow.S_No = Convert.ToInt32(item["S#No"].ToString());
                //    lookupData.Item_FilterRow.Add(i_filterRow);
                //}
                return Json(new { data = lookupData }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                WriteLog("Error - Lookup : NTID - " + User.Identity.Name.Split('\\')[1].Trim().ToUpper() + ", ErrorMsg : " + ex + ", At - " + DateTime.Now);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        public ActionResult Lookup_SpotOnList()
        {
            var obj = "";
            DataTable dt = new DataTable();
            List<EmployeeDetails> EmpDetails = new List<EmployeeDetails>();

            try
            {
                connection();
                dt = new DataTable();
                OpenConnection();
                string Query = " select distinct NTID , EmployeeName from [SPOTONData_Table_2022] where BU = 'MS/BE1' order by EmployeeName ";
                SqlCommand cmd = new SqlCommand(Query, conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                //conn.Close();
                CloseConnection();

                foreach (DataRow row in dt.Rows)
                {
                    EmployeeDetails item = new EmployeeDetails();
                    //dr.Read();

                    item.NTID = row["NTID"].ToString();
                    item.EmployeeName = row["EmployeeName"].ToString();

                    EmpDetails.Add(item);

                }
            }

            catch (Exception ex)
            {

            }
            finally
            {

            }




            return Json(new { EmpDetails, success = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult OnValueChange_inProjectedProcess(int RequestID, int isProjected, string useryear, int isChange_amount = 0, decimal ord_price = 0, decimal proj_price = 0, decimal unused_price = 0, string currency = "''", int OrdStatus = 0)
        { //when proj change - unused changes ; next unused changed - proj not changing & viceversa
            string Query = "";
            bool isproj = false;
            bool q1 = false, q2 = false, q3 = false, q4 = false;
            decimal proj_amt = 0, unused_amt = 0, order_amt = 0;
            string curr = "";
            List<RequestItemsRepoView> viewList = new List<RequestItemsRepoView>();

            connection();
            OpenConnection();
            try
            {

                Query = "Exec [dbo].[OnValueChange_inProjectedProcess] " + RequestID + "," + isProjected + "," + isChange_amount + "," + ord_price + "," + proj_price + "," + unused_price + "," + currency + "," + OrdStatus;
                SqlCommand sqlcommand = new SqlCommand(Query, conn);
                sqlcommand.ExecuteNonQuery();
                SqlDataReader dr = sqlcommand.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    isproj = bool.Parse(dr["isProjected"].ToString());
                    q1 = bool.Parse(dr["Q1"].ToString());
                    q2 = bool.Parse(dr["Q2"].ToString());
                    q3 = bool.Parse(dr["Q3"].ToString());
                    q4 = bool.Parse(dr["Q4"].ToString());
                    proj_amt = decimal.Parse(dr["Projected_Amount"].ToString());
                    unused_amt = decimal.Parse(dr["Unused_Amount"].ToString());
                    order_amt = (dr["OrderPrice"].ToString().Trim() == "") ? 0 : decimal.Parse(dr["OrderPrice"].ToString());
                    curr = "1";

                }
                else
                {
                    //
                }
                dr.Close();
                CloseConnection();
                //if(isProjected == 0 || isProjected == 1)
                //viewList = GetData1(useryear);
                return Json(new { isproj, q1, q2, q3, q4, proj_amt, unused_amt, order_amt, data = viewList, success = true }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                WriteLog("Error - OnValueChange_inProjectedProcess : NTID - " + User.Identity.Name.Split('\\')[1].Trim().ToUpper() + ", ErrorMsg : " + ex + ", At - " + DateTime.Now);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);

            }


        }

        [HttpPost]
        public ActionResult GetUnusedAvailability(int CostElement, int BU, int ItemName, int Dept, int VKMYear)
        {
            string Query = "";
            decimal AvailableUnUsedAmt = 0;
            try
            {
                connection();
                OpenConnection();
                Query = "EXEC [dbo].[GetUnusedAvailability] " + CostElement + "," + BU + "," + ItemName + "," + Dept + "," + Convert.ToInt32(VKMYear + 1);
                SqlCommand sqlcommand = new SqlCommand(Query, conn);
                sqlcommand.ExecuteNonQuery();
                SqlDataReader dr = sqlcommand.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    AvailableUnUsedAmt = decimal.Parse(dr["Unused_Amount"].ToString());
                }
                else
                {
                    AvailableUnUsedAmt = 0;
                }
                dr.Close();

                CloseConnection();
                //AvailableUnUsedAmt = 1000;
                return Json(new { AvailableUnUsedAmt, success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                AvailableUnUsedAmt = 0;
                WriteLog("Error - GetUnusedAvailability : NTID - " + User.Identity.Name.Split('\\')[1].Trim().ToUpper() + ", ErrorMsg : " + ex + ", At - " + DateTime.Now);
                return Json(new { AvailableUnUsedAmt, success = false }, JsonRequestBehavior.AllowGet);
            }

        }


        public int BulkApprove(string BU, string dept, string group)
        {
            int count = 0;
            try
            {

                using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

                //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

                {

                    List<RequestItems_Table> items = db.RequestItems_Table.ToList().FindAll(x => x.BU == BudgetingController.lstBUs.Find(bu => bu.BU.Trim().Equals(BU)).BU/*BU*/).FindAll(x => x.DEPT == BudgetingController.lstDEPTs.Find(departmnt => departmnt.DEPT.Trim().Equals(dept)).DEPT)/*dept*/.FindAll(x => x.Group == BudgetingController.lstGroups_test.Find(grp => grp.Group.Trim().Equals(group)).Group)/*group*/.FindAll(x => x.ApprovalDH == false);
                    foreach (RequestItems_Table item in items)
                    {
                        item.ApprovedDH = true;
                        item.ApprovalSH = true;
                        item.DHAppDate = DateTime.Now.Date;
                        item.ApprovalDH = true;
                        item.SubmitDate = DateTime.Now.Date;
                        if (item.ApprQuantity == null)
                            item.ApprQuantity = item.ReqQuantity;
                        if (item.ApprCost == null)
                            item.ApprCost = item.TotalPrice;


                        db.Entry(item).State = EntityState.Modified;
                        db.SaveChanges();
                        count++;
                    }
                }
                return count;
            }
            catch (Exception ex)
            {
                return count;
            }

        }

        public int BulkApprove(string DHuser)
        {
            int count = 0;
            try
            {

                using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

                //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

                {

                    List<RequestItems_Table> items = db.RequestItems_Table.ToList().FindAll(x => x.DHNT == DHuser).FindAll(x => x.ApprovalDH == true).FindAll(x => x.ApprovedDH == false);
                    foreach (RequestItems_Table item in items)
                    {
                        item.ApprovedDH = true;
                        item.ApprovalSH = true;
                        item.DHAppDate = DateTime.Now.Date;
                        if (item.ApprQuantity == null)
                            item.ApprQuantity = item.ReqQuantity;
                        if (item.ApprCost == null)
                            item.ApprCost = item.TotalPrice;


                        db.Entry(item).State = EntityState.Modified;
                        db.SaveChanges();
                        count++;
                    }
                }
                return count;
            }
            catch (Exception ex)
            {
                return count;
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
            // string qry = "select TopSection,Section, Department from SPOTONData_Table_2022 where NTID = '" + User.Identity.Name.Split('\\')[1].ToUpper() + "' ";
            string qry = "select TopSection,Section, Department from SPOTONData_Table_2022 where Department = (Select Dept from DEPT_Table where ID = (Select DEPT from RequestItems_Table where RequestID = '" + id + "'))";



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
                    Request.Files[0].SaveAs(path);
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



        [HttpPost]
        public ActionResult EncodeURL(string RequestID)
        {
            //string encoded = HttpUtility.UrlEncode(url);
            string HostURL = "";
            string Host = ConfigurationManager.AppSettings["Host"].ToString();



            string presentUserDept1 = "", presentUserSection1 = "", presentUserTopSection1 = "";
            string presentUserDept = "", presentUserSection = "", presentUserTopSection = "";
            connection();
            string qry = "select TopSection,Section, Department from SPOTONData_Table_2022 where NTID = '" + User.Identity.Name.Split('\\')[1].ToUpper() + "'; Select isnull(LinkedRequestID,'') as LinkedRequestID from RequestItems_Table where RequestID = " + RequestID + " ; ";

            DataSet ds = new DataSet();

            SqlDataReader dr1;
            OpenConnection();
            SqlCommand cmd1 = new SqlCommand(qry, conn);
            //dr1 = cmd1.ExecuteReader();
            //if (dr1.HasRows)
            //{
            //    dr1.Read();
            //    presentUserDept1 = dr1["Department"].ToString();
            //    presentUserSection1 = dr1["Section"].ToString();
            //    presentUserTopSection1 = dr1["TopSection"].ToString();
            //}
            //dr1.Close();
            //CloseConnection();

            //presentUserSection = presentUserSection1.Replace("/", "_");
            //presentUserTopSection = presentUserTopSection1.Replace("/", "_");


            SqlDataAdapter da = new SqlDataAdapter(cmd1);
            da.Fill(ds);
            CloseConnection();

            if (ds.Tables[0].Rows.Count > 0)
            {
                presentUserDept1 = ds.Tables[0].Rows[0]["Department"].ToString();
                presentUserSection1 = ds.Tables[0].Rows[0]["Section"].ToString();
                presentUserTopSection1 = ds.Tables[0].Rows[0]["TopSection"].ToString();
            }

            presentUserSection = presentUserSection1.Replace("/", "_");
            presentUserTopSection = presentUserTopSection1.Replace("/", "_");


            if (ds.Tables[1].Rows.Count > 0)
            {
                if (ds.Tables[1].Rows[0]["LinkedRequestID"].ToString() != "")
                {
                    RequestID = ds.Tables[1].Rows[0]["LinkedRequestID"].ToString();
                }
            }


            try
            {
                string Path = "/BGSW_Ordering/BGSW_VKM" + DateTime.Now.Year.ToString() + "/" + presentUserTopSection + "/" + presentUserSection + "/" + RequestID;
                if (!System.IO.Directory.Exists(Server.MapPath(Path)))
                {
                    System.IO.Directory.CreateDirectory(Server.MapPath(Path));
                }
                HostURL = Host + Path;
            }
            catch (Exception ex)
            {



            }
            return Json(new { success = true, Result = HostURL }, JsonRequestBehavior.AllowGet);
        }
        public partial class CurrencyConversionView
        {

            public int ID { get; set; }
            public string Currency { get; set; }
            public decimal ConversionRate { get; set; }
        }

        public partial class RFOApproverDetails
        {
            public int ID { get; set; }
            public string Section { get; set; }
            public string NTID { get; set; }
            public string EmployeeName { get; set; }
            public bool IsPaused { get; set; }

        }

        public partial class CTGDetails
        {
            public int ID { get; set; }
            public string Section { get; set; }
            public decimal Amount { get; set; }
            public decimal Approved { get; set; }
            public decimal Utilized { get; set; }
            public decimal Balance { get; set; }

        }

        public partial class RFOApproverEdit
        {
            public int ID { get; set; }
            public string Section { get; set; }
            public string NTID { get; set; }
            public string EmployeeName { get; set; }
            public bool IsPaused { get; set; }
        }


        public partial class CTGEdit
        {
            public int ID { get; set; }
            public string Section { get; set; }
            public decimal Amount { get; set; }
        }

        public partial class EmployeeDetails
        {
            public string NTID { get; set; }
            public string EmployeeName { get; set; }
        }
        public class BonaparteGroup
        {
            public string PONumber { get; set; }
            public float OrderedQuantity { get; set; }
            public string PIFID { get; set; }
            public string Fund { get; set; }
            public string CustomerDept { get; set; }

            public decimal OrderPrice { get; set; }

            public DateTime POCreatedOn { get; set; }

            public Nullable<int> VKM_Year { get; set; }

            public string BU { get; set; }

            public string OEM { get; set; }

            public string GROUP { get; set; }

            public string Dept { get; set; }
            public string ItemName { get; set; }
            public string OrderStatus { get; set; }


        }
        //public class SR_Responsible_Buyer
        //{
        //    public int ID { get; set; }
        //    public string BuyerName { get; set; }
        //    public string ManagerID { get; set; }
        //}
    }
}
