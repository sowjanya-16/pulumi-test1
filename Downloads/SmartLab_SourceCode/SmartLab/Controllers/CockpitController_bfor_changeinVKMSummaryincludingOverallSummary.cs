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

namespace LC_Reports_V1.Controllers
{
    [Authorize(Users = @"apac\din2cob,apac\jov6cob,apac\mta2cob,apac\muu4cob,apac\sbr2kor,apac\chb1kor,apac\oig1cob,apac\mae9cob,apac\rau2kor,apac\rma5cob, apac\pch2kor, apac\mxk8kor,  apac\ghb1cob, apac\vvs2kor, apac\SIF1COB")]
    public class CockpitController : Controller
    {
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
        public static List<Order_Status_Table> lstOrderStatus = new List<Order_Status_Table>();

        /// <summary>
        /// /////////////////////////WITH REQUEST TABLE
        /// </summary>
        public ActionResult Cockpit_Options()
        {

            return View();
        }

        public ActionResult VKM_Cockpit()
        {
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
                //if (lstBU_SPOCs == null || lstBU_SPOCs.Count == 0)
                    lstBU_SPOCs = db.BU_SPOCS.ToList<BU_SPOCS>();
                if (lstGroups_test == null || lstGroups_test.Count == 0)
                    lstGroups_test = db.Groups_Table_Test.ToList<Groups_Table_Test>();

                if (lstOrderStatus == null || lstOrderStatus.Count == 0)
                    lstOrderStatus = db.Order_Status_Table.ToList<Order_Status_Table>();

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
                lstOrderStatus.Sort((a, b) => a.OrderStatus.CompareTo(b.OrderStatus));

            }
            //if (lstUsers == null || lstUsers.Count == 0)
            return View();
        }

        [HttpPost]
        public ActionResult GetInitData()
        {
            using (BudgetingToolServerDBEntities1 db = new BudgetingToolServerDBEntities1())
            {
                //if (lstUsers == null || lstUsers.Count == 0)
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
                //if (lstBU_SPOCs == null || lstBU_SPOCs.Count == 0)
                    lstBU_SPOCs = db.BU_SPOCS.ToList<BU_SPOCS>();
                if (lstGroups_test == null || lstGroups_test.Count == 0)
                    lstGroups_test = db.Groups_Table_Test.ToList<Groups_Table_Test>();

                if (lstOrderStatus == null || lstOrderStatus.Count == 0)
                    lstOrderStatus = db.Order_Status_Table.ToList<Order_Status_Table>();


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
                lstOrderStatus.Sort((a, b) => a.OrderStatus.CompareTo(b.OrderStatus));

            }
            //if (lstUsers == null || lstUsers.Count == 0)
            //{
            //    return RedirectToAction("Index", "Budgeting");
            //}

            //return View();
            return Json(new { data = true }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult QuarterlyUtilization(string years_str, string buList_str, bool chart = false, string selected_ccxc = "")
        {
            List<string> years = new List<string>();
            List<string> buList = new List<string>();
            List<RequestItems_Table> rlist = new List<RequestItems_Table>();


            using (BudgetingToolServerDBEntities1 db = new BudgetingToolServerDBEntities1())
            {
                List<RequestItems_Table> reqList = new List<RequestItems_Table>();
                List<RequestItems_Table> reqList_forquery = new List<RequestItems_Table>();
                
                if (years_str != null)
                {
                    years = (years_str.Split(',')).ToList();

                }
                if (buList_str != null)
                {
                    buList = (buList_str.Split(',')).ToList();

                }
                foreach (var bu_item in buList)
                {
                    reqList.AddRange(db.RequestItems_Table.ToList<RequestItems_Table>().FindAll(ss => ss.BU.Contains(bu_item)).FindAll(v => v.Fund == null || v.Fund.Trim().Contains("2")));
                }

                foreach (string yr in years)
                {

                    //vkmSummary(years, buList, chart, selected_ccxc);
                    if (int.Parse(yr) - 1 > 2020)
                    {

                        string is_CCXC = string.Empty;
                        bool CCXCflag = false;
                        string presentUserDept = string.Empty;
                        // if (selected_ccxc != "") //DATA FILTER BASED ON CCXC DROPDOWM - NOT USER'S NTID
                        // {
                        if (selected_ccxc.ToUpper().Contains("CC"))
                        {
                            is_CCXC = "CC";
                            CCXCflag = true;
                        }
                        else if (selected_ccxc.ToUpper().Contains("XC"))
                        {
                            is_CCXC = "XC";
                            CCXCflag = true;
                        }
                        //   }
                        else                    //DATA FILTER BASED ON USER'S NTID
                        {
                            presentUserDept = lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).Department;

                            if (presentUserDept.ToUpper().Contains("CC"))
                            {
                                is_CCXC = "CC";
                                CCXCflag = true;
                            }
                            else if (presentUserDept.ToUpper().Contains("XC"))
                            {
                                is_CCXC = "XC";
                                CCXCflag = true;
                            }

                        }

                        if (CCXCflag)
                        {
                            if (is_CCXC.Contains("XC"))
                            {
                                //XC includes 2WP 
                                reqList_forquery.AddRange(db.RequestItems_Table.ToList().FindAll(ss => ss.BU.Contains(lstBUs.Find(bu => bu.BU.Contains("2WP")).ID.ToString())).FindAll(v => v.Fund == null || v.Fund.Trim().Contains("2")));
                                reqList_forquery = reqList.FindAll(dpt => lstDEPTs.Find(x => x.ID.ToString().Contains(dpt.DEPT)).DEPT.Contains(is_CCXC));

                            }
                            else
                                reqList_forquery = reqList.FindAll(dpt => !lstDEPTs.Find(x => x.ID.ToString().Contains(dpt.DEPT)).DEPT.Contains("XC"));

                            CCXCflag = false;
                        }



                    }
                    else
                    {
                        if (buList.Contains("2") && buList.Contains("4") /*&& selected_ccxc.ToUpper().Contains("Custom") || selected_ccxc.ToUpper().Contains("XC"))*/ && yr == "2021" && reqList.FindAll(c => c.BU == "1").Count == 0) //XC VKM2021 - include 2WP Whole Totals also
                        {
                            reqList.AddRange(db.RequestItems_Table.ToList().FindAll(ss => ss.BU.Contains(lstBUs.Find(bu => bu.BU.Contains("2WP")).ID.ToString())).FindAll(v => v.Fund == null || v.Fund.Trim().Contains("2")));
                        }
                        reqList_forquery = reqList;


                    }
                    // return Json(new { data = reqList_forquery.FindAll(y=>y.ApprovalSH == true), message = selected_ccxc }, JsonRequestBehavior.AllowGet);
                    rlist.AddRange(reqList_forquery.FindAll(y => y.ApprovalSH == true && y.RequestDate.ToString().Contains((int.Parse(yr) - 1).ToString())));
                }
            }
            return View(rlist);
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


            //chart = 0 (false - table), 1(table + graph), 2(graph)
            if (lstUsers == null)
            {
                return RedirectToAction("Index", "Budgeting");
            }

            using (BudgetingToolServerDBEntities1 db = new BudgetingToolServerDBEntities1())
            {
                List<RequestItems_Table> reqList = new List<RequestItems_Table>();
                List<RequestItems_Table> reqList_forquery = new List<RequestItems_Table>();
                List<vkmSummary> viewList = new List<vkmSummary>();

                var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                BudgetParam t = new BudgetParam();
                List<columnsinfo> _col = new List<columnsinfo>();


                decimal OP_MAE_Totals = 0, OP_NMAE_Totals = 0, OP_SoftwareTotals = 0;
                decimal OU_MAE_Totals = 0, OU_NMAE_Totals = 0, OU_SoftwareTotals = 0;
                decimal Ch_P_MAE_Totals = 0, Ch_P_NMAE_Totals = 0, Ch_P_SoftwareTotals = 0, Ch_P_OverallTotals = 0;
                decimal Ch_U_MAE_Totals = 0, Ch_U_NMAE_Totals = 0, Ch_U_SoftwareTotals = 0, Ch_U_OverallTotals = 0;
                decimal Pr_P_MAE_Totals = 0, Pr_P_NMAE_Totals = 0, Pr_P_SoftwareTotals = 0, Pr_P_OverallTotals = 0;
                decimal Pr_U_MAE_Totals = 0, Pr_U_NMAE_Totals = 0, Pr_U_SoftwareTotals = 0, Pr_U_OverallTotals = 0;

                // List<RequestItemsList_2019> reqList2019 = new List<RequestItemsList_2019>();
                //reqList2019 = db.RequestItemsList_2019.ToList<RequestItemsList_2019>();



                //  if (!(chart == false && years.Count() == 3)) //if number of years selected = 3, then no need to show table
                // {


                //get reqlist data for the BUs based on SPOC's BU / CC XC Level BUs
                foreach (var bu_item in buList)
                {
                    reqList.AddRange(db.RequestItems_Table.ToList<RequestItems_Table>().FindAll(ss => ss.BU.Contains(bu_item)).FindAll(v => v.Fund == null || v.Fund.Trim().Contains("2")));
                }

                foreach (string yr in years)
                {
                    //CC XC CHECK


                    //for 2020 reqList_forquery has relevant data under the BU filtering ; but >2020 needs dept FIltering based on CC XC
                    //if(int.Parse(yr)-1 == 2020)
                    //{
                    //    reqList_forquery.AddRange(reqList.FindAll(ss => ss.BU.Contains(lstBUs.Find(bu => bu.BU.Contains("DA")).ID.ToString())));
                    //}
                    if (int.Parse(yr) - 1 > 2020)
                    {

                        string is_CCXC = string.Empty;
                        bool CCXCflag = false;
                        string presentUserDept = string.Empty;
                        // if (selected_ccxc != "") //DATA FILTER BASED ON CCXC DROPDOWM - NOT USER'S NTID
                        // {
                        if (selected_ccxc.ToUpper().Contains("CC"))
                        {
                            is_CCXC = "CC";
                            CCXCflag = true;
                        }
                        else if (selected_ccxc.ToUpper().Contains("XC"))
                        {
                            is_CCXC = "XC";
                            CCXCflag = true;
                        }
                        //   }
                        else                    //DATA FILTER BASED ON USER'S NTID
                        {
                            presentUserDept = lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).Department;

                            if (presentUserDept.ToUpper().Contains("CC"))
                            {
                                is_CCXC = "CC";
                                CCXCflag = true;
                            }
                            else if (presentUserDept.ToUpper().Contains("XC"))
                            {
                                is_CCXC = "XC";
                                CCXCflag = true;
                            }

                        }

                        if (CCXCflag)
                        {
                            if (is_CCXC.Contains("XC"))
                            {
                                //XC includes 2WP 
                                reqList_forquery.AddRange(db.RequestItems_Table.ToList().FindAll(ss => ss.BU.Contains(lstBUs.Find(bu => bu.BU.Contains("2WP")).ID.ToString())).FindAll(v => v.Fund == null || v.Fund.Trim().Contains("2")));
                                reqList_forquery = reqList.FindAll(dpt => lstDEPTs.Find(x => x.ID.ToString().Contains(dpt.DEPT)).DEPT.Contains(is_CCXC));

                            }
                            else
                                reqList_forquery = reqList.FindAll(dpt => !lstDEPTs.Find(x => x.ID.ToString().Contains(dpt.DEPT)).DEPT.Contains("XC"));

                            CCXCflag = false;
                        }



                    }
                    else
                    {
                        if (buList.Contains("2") && buList.Contains("4") /*&& selected_ccxc.ToUpper().Contains("Custom") || selected_ccxc.ToUpper().Contains("XC"))*/ && yr == "2021" && reqList.FindAll(c => c.BU == "1").Count == 0) //XC VKM2021 - include 2WP Whole Totals also
                        {
                            reqList.AddRange(db.RequestItems_Table.ToList().FindAll(ss => ss.BU.Contains(lstBUs.Find(bu => bu.BU.Contains("2WP")).ID.ToString())).FindAll(v => v.Fund == null || v.Fund.Trim().Contains("2")));
                        }
                        reqList_forquery = reqList;
                        
                       
                    }
                   // return Json(new { data = reqList_forquery.FindAll(y=>y.ApprovalSH == true), message = selected_ccxc }, JsonRequestBehavior.AllowGet);
                    //CODE TO GET GROUPS OF COST ELEMENT
                    IEnumerable<IGrouping<string, RequestItems_Table>> query = reqList_forquery.GroupBy(item => item.CostElement);

                    decimal P_MAE_Totals = 0, P_NMAE_Totals = 0, P_SoftwareTotals = 0;
                    decimal U_MAE_Totals = 0, U_NMAE_Totals = 0, U_SoftwareTotals = 0;
                    decimal Cancelled_MAE_Totals = 0, Cancelled_NMAE_Totals = 0, Cancelled_Software_Totals = 0;

                    Cancelled_MAE_Totals = reqList_forquery.FindAll(X=>X.CostElement.Trim() == "1" && X.OrderStatus != null).Where(item => item.OrderStatus.Trim() == lstOrderStatus.Find(status => status.OrderStatus.Trim().Equals("Cancelled")).ID.ToString()).Count() >0 ?
                        (decimal)reqList_forquery.FindAll(X => X.CostElement.Trim() == "1" && X.OrderStatus != null).Where(item => item.OrderStatus.Trim() == lstOrderStatus.Find(status => status.OrderStatus.Trim().Equals("Cancelled")).ID.ToString()).Sum(x => Math.Round((double)x.ApprCost,MidpointRounding.AwayFromZero)):
                        0;
                    Cancelled_NMAE_Totals = reqList_forquery.FindAll(X => X.CostElement.Trim() == "2" && X.OrderStatus != null).Where(item => item.OrderStatus.Trim() == lstOrderStatus.Find(status => status.OrderStatus.Trim().Equals("Cancelled")).ID.ToString()).Count() > 0 ?
                        (decimal)reqList_forquery.FindAll(X => X.CostElement.Trim() == "2" && X.OrderStatus != null).Where(item => item.OrderStatus.Trim() == lstOrderStatus.Find(status => status.OrderStatus.Trim().Equals("Cancelled")).ID.ToString()).Sum(x => Math.Round((double)x.ApprCost, MidpointRounding.AwayFromZero)) :
                        0;
                    Cancelled_Software_Totals = reqList_forquery.FindAll(X => X.CostElement.Trim() == "3" && X.OrderStatus != null).Where(item => item.OrderStatus.Trim() == lstOrderStatus.Find(status => status.OrderStatus.Trim().Equals("Cancelled")).ID.ToString()).Count() > 0 ?
                        (decimal)reqList_forquery.FindAll(X => X.CostElement.Trim() == "3" && X.OrderStatus != null).Where(item => item.OrderStatus.Trim() == lstOrderStatus.Find(status => status.OrderStatus.Trim().Equals("Cancelled")).ID.ToString()).Sum(x => Math.Round((double)x.ApprCost, MidpointRounding.AwayFromZero)) :
                        0;
                    //var danger1 = string.Empty;
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
                                    if (item.SubmitDate.ToString().Contains((int.Parse(yr) - 1).ToString()) && item.ApprovalSH == true)  //yr is VKM Year; if yr == 2020 - 2021 Planning (2020 sh apprvd - 2021 planning)
                                    {

                                        P_MAE_Totals += Math.Round((decimal)item.ApprCost, MidpointRounding.AwayFromZero);

                                        U_MAE_Totals += item.OrderPrice == null ? (decimal)0.0 : Math.Round((decimal)item.OrderPrice, MidpointRounding.AwayFromZero); ;


                                    }
                                }
                            }
                            else if (lstCostElement.Find(cost => cost.ID.Equals(Int32.Parse(CostGroup.Key))).CostElement == "Non-MAE")
                            {
                                foreach (RequestItems_Table item in CostGroup)
                                {
                                    if (item.SubmitDate.ToString().Contains((int.Parse(yr) - 1).ToString()) && item.ApprovalSH == true)
                                    {

                                        P_NMAE_Totals += Math.Round((decimal)item.ApprCost, MidpointRounding.AwayFromZero);

                                        U_NMAE_Totals += item.OrderPrice == null ? (decimal)0.0 : Math.Round((decimal)item.OrderPrice, MidpointRounding.AwayFromZero); ;

                                    }
                                }

                            }
                            else if (lstCostElement.Find(cost => cost.ID.Equals(Int32.Parse(CostGroup.Key))).CostElement == "Software")
                            {
                                foreach (RequestItems_Table item in CostGroup)
                                {
                                    if (item.SubmitDate.ToString().Contains((int.Parse(yr) - 1).ToString()) && item.ApprovalSH == true)
                                    {


                                        P_SoftwareTotals += Math.Round((decimal)item.ApprCost, MidpointRounding.AwayFromZero);

                                        U_SoftwareTotals += item.OrderPrice == null ? (decimal)0.0 : Math.Round((decimal)item.OrderPrice, MidpointRounding.AwayFromZero); ;

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

                    tempobj.Cancelled_MAE_Totals = Cancelled_MAE_Totals;
                    tempobj.Cancelled_NMAE_Totals = Cancelled_NMAE_Totals;
                    tempobj.Cancelled_Software_Totals = Cancelled_Software_Totals;

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
                    viewList.Add(tempobj);

                }





                if (!(chart == true && years.Count() == 3)) //if years selected are 3, then no need to calculate change in values
                {
                    Ch_P_MAE_Totals = (viewList.ElementAt(viewList.Count - 1).P_MAE_Totals) - (viewList.ElementAt(viewList.Count - 2).P_MAE_Totals);
                    Ch_P_NMAE_Totals = (viewList.ElementAt(viewList.Count - 1).P_NMAE_Totals) - (viewList.ElementAt(viewList.Count - 2).P_NMAE_Totals);
                    Ch_P_SoftwareTotals = (viewList.ElementAt(viewList.Count - 1).P_Software_Totals) - (viewList.ElementAt(viewList.Count - 2).P_Software_Totals);
                    Ch_P_OverallTotals = (viewList.ElementAt(viewList.Count - 1).P_Overall_Totals) - (viewList.ElementAt(viewList.Count - 2).P_Overall_Totals);

                    //var refTotals_forpercent_change = Int32.Parse(viewList[0].vkmyear) < Int32.Parse(viewList[1].vkmyear) ? viewList.ElementAt(viewList.Count - 2) : viewList.ElementAt(viewList.Count - 1);
                    Pr_P_MAE_Totals = (viewList.ElementAt(viewList.Count - 2).P_MAE_Totals) != 0 ? (Ch_P_MAE_Totals / viewList.ElementAt(viewList.Count - 2).P_MAE_Totals) * 100 : 0;
                    Pr_P_NMAE_Totals = (viewList.ElementAt(viewList.Count - 2).P_NMAE_Totals) != 0 ? (Ch_P_NMAE_Totals / viewList.ElementAt(viewList.Count - 2).P_NMAE_Totals) * 100 : 0;
                    Pr_P_SoftwareTotals = (viewList.ElementAt(viewList.Count - 2).P_Software_Totals) != 0 ? (Ch_P_SoftwareTotals / viewList.ElementAt(viewList.Count - 2).P_Software_Totals) * 100 : 0;
                    Pr_P_OverallTotals = (viewList.ElementAt(viewList.Count - 2).P_Overall_Totals) != 0 ? (Ch_P_OverallTotals / viewList.ElementAt(viewList.Count - 2).P_Overall_Totals) * 100 : 0;

                    Ch_U_MAE_Totals = (viewList.ElementAt(viewList.Count - 1).U_MAE_Totals) - (viewList.ElementAt(viewList.Count - 2).U_MAE_Totals);
                    Ch_U_NMAE_Totals = (viewList.ElementAt(viewList.Count - 1).U_NMAE_Totals) - (viewList.ElementAt(viewList.Count - 2).U_NMAE_Totals);
                    Ch_U_SoftwareTotals = (viewList.ElementAt(viewList.Count - 1).U_Software_Totals) - (viewList.ElementAt(viewList.Count - 2).U_Software_Totals);
                    Ch_U_OverallTotals = (viewList.ElementAt(viewList.Count - 1).U_Overall_Totals) - (viewList.ElementAt(viewList.Count - 2).U_Overall_Totals);
                    Pr_U_MAE_Totals = (viewList.ElementAt(viewList.Count - 2).U_MAE_Totals) != 0 ? (Ch_U_MAE_Totals / viewList.ElementAt(viewList.Count - 2).U_MAE_Totals) * 100 : 0;
                    Pr_U_NMAE_Totals = (viewList.ElementAt(viewList.Count - 2).U_NMAE_Totals) != 0 ? (Ch_U_NMAE_Totals / viewList.ElementAt(viewList.Count - 2).U_NMAE_Totals) * 100 : 0;
                    Pr_U_SoftwareTotals = (viewList.ElementAt(viewList.Count - 2).U_Software_Totals) != 0 ? (Ch_U_SoftwareTotals / viewList.ElementAt(viewList.Count - 2).U_Software_Totals) * 100 : 0;
                    Pr_U_OverallTotals = (viewList.ElementAt(viewList.Count - 2).U_Overall_Totals) != 0 ? (Ch_U_OverallTotals / viewList.ElementAt(viewList.Count - 2).U_Overall_Totals) * 100 : 0;
                }
                if (chart)
                {
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
                    //dt.Columns.Add("VKM 2020", typeof(string));
                    string yrs = string.Empty;
                    foreach (string yr in years)
                    {

                        dt.Columns.Add(yr + " Plan", typeof(string)); //add vkm text to yr
                        dt.Columns.Add(yr + " Used", typeof(string));
                        dt.Columns.Add(yr + " Projected", typeof(string));

                    }

                    dt.Columns.Add("Plan 🠕🠗", typeof(string)); //"Plan(" + String.Join("-", years.ToArray()) + ") Change"
                    dt.Columns.Add("Plan % 🠕🠗 ", typeof(string));
                    dt.Columns.Add("Used 🠕🠗", typeof(string));
                    dt.Columns.Add("Used % 🠕🠗", typeof(string));


                    DataRow dr = dt.NewRow();
                    dr[0] = "MAE";
                    int rcnt = 1;
                    int rcnt1 = 2; //2
                    int rcnt2 = 3;

                    foreach (var info in viewList)
                    {
                        dr[rcnt] = info.P_MAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
                        dr[rcnt1] = info.U_MAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
                        dr[rcnt2] = (info.P_MAE_Totals - (info.U_MAE_Totals + info.Cancelled_MAE_Totals)).ToString("C0", CultureInfo.CurrentCulture);
                        //rcnt++;
                        //rcnt = rcnt+ 2;
                        //rcnt1 = rcnt1 + 2;

                        if ((viewList.Count * 3 != rcnt2))
                        {
                            rcnt = rcnt + 3;
                            rcnt1 = rcnt1 + 3;
                            rcnt2 = rcnt2 + 3;
                        }
                        else
                        {
                            rcnt2 = rcnt2 + 1;
                        }

                    }
                    dr[rcnt2] = Ch_P_MAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
                    dr[rcnt2 + 1] = Math.Round(Pr_P_MAE_Totals, 2) + "%";
                    dr[rcnt2 + 2] = Ch_U_MAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
                    dr[rcnt2 + 3] = Math.Round(Pr_U_MAE_Totals, 2) + "%";
                    dt.Rows.Add(dr);

                    dr = dt.NewRow();
                    dr[0] = "Non-MAE";
                    int r1cnt = 1;
                    int r1cnt1 = 2;
                    int r1cnt2 = 3;
                    foreach (var info in viewList)
                    {
                        dr[r1cnt] = info.P_NMAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
                        dr[r1cnt1] = info.U_NMAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
                        dr[r1cnt2] = (info.P_NMAE_Totals - (info.U_NMAE_Totals + info.Cancelled_NMAE_Totals)).ToString("C0", CultureInfo.CurrentCulture);
                        //rcnt++;
                        //rcnt = rcnt+ 2;
                        //rcnt1 = rcnt1 + 2;

                        if ((viewList.Count * 3 != r1cnt2))
                        {
                            r1cnt = r1cnt + 3;
                            r1cnt1 = r1cnt1 + 3;
                            r1cnt2 = r1cnt2 + 3;
                        }
                        else
                        {
                            r1cnt2 = r1cnt2 + 1;
                        }
                    }
                    dr[r1cnt2] = Ch_P_NMAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
                    dr[r1cnt2 + 1] = Math.Round(Pr_P_NMAE_Totals, 2) + "%";
                    dr[r1cnt2 + 2] = Ch_U_NMAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
                    dr[r1cnt2 + 3] = Math.Round(Pr_U_NMAE_Totals, 2) + "%";
                    dt.Rows.Add(dr);

                    dr = dt.NewRow();
                    dr[0] = "Software";
                    int r2cnt = 1;
                    int r2cnt1 = 2;
                    int r2cnt2 = 3;
                    foreach (var info in viewList)
                    {
                        dr[r2cnt] = info.P_Software_Totals.ToString("C0", CultureInfo.CurrentCulture);
                        dr[r2cnt1] = info.U_Software_Totals.ToString("C0", CultureInfo.CurrentCulture);
                        dr[r2cnt2] = (info.P_Software_Totals - (info.U_Software_Totals + info.Cancelled_Software_Totals)).ToString("C0", CultureInfo.CurrentCulture);
                        //rcnt++;
                        //rcnt = rcnt+ 2;
                        //rcnt1 = rcnt1 + 2;

                        if ((viewList.Count * 3 != r2cnt2))
                        {
                            r2cnt = r2cnt + 3;
                            r2cnt1 = r2cnt1 + 3;
                            r2cnt2 = r2cnt2 + 3;
                        }
                        else
                        {
                            r2cnt2 = r2cnt2 + 1;
                        }
                    }
                    dr[r2cnt2] = Ch_P_SoftwareTotals.ToString("C0", CultureInfo.CurrentCulture);
                    dr[r2cnt2 + 1] = Math.Round(Pr_P_SoftwareTotals, 2) + "%";
                    dr[r2cnt2 + 2] = Ch_U_SoftwareTotals.ToString("C0", CultureInfo.CurrentCulture);
                    dr[r2cnt2 + 3] = Math.Round(Pr_U_SoftwareTotals, 2) + "%";
                    dt.Rows.Add(dr);

                    dr = dt.NewRow();
                    dr[0] = "Totals";
                    int r3cnt = 1;
                    int r3cnt1 = 2;
                    int r3cnt2 = 3;
                    foreach (var info in viewList)
                    {
                        dr[r3cnt] = (info.P_MAE_Totals + info.P_NMAE_Totals + info.P_Software_Totals).ToString("C0", CultureInfo.CurrentCulture);
                        dr[r3cnt1] = (info.U_MAE_Totals + info.U_NMAE_Totals + info.U_Software_Totals).ToString("C0", CultureInfo.CurrentCulture);
                        dr[r3cnt2] = ((info.P_MAE_Totals - (info.U_MAE_Totals + info.Cancelled_MAE_Totals)) +
                            (info.P_NMAE_Totals - (info.U_NMAE_Totals + info.Cancelled_NMAE_Totals)) +
                            (info.P_Software_Totals - (info.U_Software_Totals + info.Cancelled_Software_Totals))).ToString("C0", CultureInfo.CurrentCulture);
                        //rcnt++;
                        //rcnt = rcnt+ 2;
                        //rcnt1 = rcnt1 + 2;
                        if ((viewList.Count * 3 != r3cnt2))
                        {
                            r3cnt = r3cnt + 3;
                            r3cnt1 = r3cnt1 + 3;
                            r3cnt2 = r3cnt2 + 3;
                        }
                        else
                        {
                            r3cnt2 = r3cnt2 + 1;
                        }
                    }
                    dr[r3cnt2] = Ch_P_OverallTotals.ToString("C0", CultureInfo.CurrentCulture);
                    dr[r3cnt2 + 1] = Math.Round(Pr_P_OverallTotals, 2) + "%";
                    dr[r3cnt2 + 2] = Ch_U_OverallTotals.ToString("C0", CultureInfo.CurrentCulture);
                    dr[r3cnt2 + 3] = Math.Round(Pr_U_OverallTotals, 2) + "%";
                    dt.Rows.Add(dr);
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

                return Json(new { data = t, message = selected_ccxc }, JsonRequestBehavior.AllowGet);

                // return View();
            }
        }



        public ActionResult CostElementDrillDown_comparison(string years_str, string CostElement_Chosen, string buList_str, bool chart = false, string selected_ccxc = "")
        {
           
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

            TempData["years_str"] = years_str;
            TempData["buList_str"] = buList_str;
            TempData["CostElement_Chosen"] = CostElement_Chosen;

            if (lstUsers == null)
            {
                return RedirectToAction("Index", "Budgeting");
            }

            using (BudgetingToolServerDBEntities1 db = new BudgetingToolServerDBEntities1())
            {
                List<RequestItems_Table> reqList = new List<RequestItems_Table>();
                List<RequestItems_Table> reqList_forquery = new List<RequestItems_Table>();
                List<CategoryBudget> categoryBudgets = new List<CategoryBudget>();
                List<CategoryBudget> Top70_CategoryBudget = new List<CategoryBudget>();
                List<costelement_drilldownSummary> viewList = new List<costelement_drilldownSummary>();
                string costelement_chosen_id = string.Empty;
                reqList = db.RequestItems_Table.ToList<RequestItems_Table>().FindAll(v => v.Fund == null || v.Fund.Trim().Contains("2"));
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
                costelement_chosen_id = lstCostElement.Find(cost => cost.CostElement.ToUpper().Trim() == CostElement_Chosen.ToUpper().Trim()).ID.ToString().Trim();

                foreach (var bu_item in buList)
                {
                    reqList_forquery.AddRange(reqList.FindAll(item => item.CostElement.Trim() == costelement_chosen_id).FindAll(ss => ss.BU.Contains(bu_item)));
                }

                //CODE TO GET GROUPS OF CATEGORY


                foreach (string yr in years)
                {
                    BaseYear_toCompare++;
                    //CC XC CHECK

                    if ((int.Parse(yr) - 1) > 2020)  //for 2020 reqList_forquery has relevant data
                    {
                        string is_CCXC = string.Empty;
                        bool CCXCflag = false;
                        string presentUserDept = string.Empty;

                        if (selected_ccxc.ToUpper().Contains("CC"))
                        {
                            is_CCXC = "CC";
                            CCXCflag = true;
                        }
                        else if (selected_ccxc.ToUpper().Contains("XC"))
                        {
                            is_CCXC = "XC";
                            CCXCflag = true;
                        }


                        else                    //DATA FILTER BASED ON USER'S NTID
                        {
                            presentUserDept = lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).Department;

                            if (presentUserDept.ToUpper().Contains("CC"))
                            {
                                is_CCXC = "CC";
                                CCXCflag = true;
                            }
                            else if (presentUserDept.ToUpper().Contains("XC"))
                            {
                                is_CCXC = "XC";
                                CCXCflag = true;
                            }

                        }

                        if (CCXCflag)
                        {
                            if (is_CCXC.Contains("XC"))
                            {
                                reqList_forquery.AddRange(reqList.FindAll(ss => ss.BU.Contains(lstBUs.Find(bu => bu.BU.Contains("2WP")).ID.ToString())));
                                reqList_forquery = reqList_forquery.FindAll(dpt => lstDEPTs.Find(x => x.ID.ToString().Contains(dpt.DEPT)).DEPT.Contains(is_CCXC))/*.FindAll(ss => ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("AS")).ID.ToString()) || ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("2WP")).ID.ToString()))*/;

                            }
                            else
                            {
                                reqList_forquery = reqList_forquery.FindAll(dpt => !lstDEPTs.Find(x => x.ID.ToString().Contains(dpt.DEPT)).DEPT.Contains("XC"))/*.FindAll(ss => ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("AS")).ID.ToString()) || ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("2WP")).ID.ToString()))*/;

                            }
                            CCXCflag = false;
                        }



                    }


                    decimal[] P_Category_Totals = new decimal[categoryBudgets.Count()];
                    decimal[] U_Category_Totals = new decimal[categoryBudgets.Count()];
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
                                if (item.SubmitDate.ToString().Contains((int.Parse(yr) - 1).ToString()) && item.ApprovalSH == true)   // 2020 sh apprvd - 2021 planning
                                {
                                    // if ((int.Parse(yr) - 1).ToString() == "2020")
                                    if (BaseYear_toCompare == 1 | (BaseYear_toCompare == 2 && Top70_CategoryBudget.Count() == 0))
                                    {

                                        if (int.Parse(item.Category) == catbudget.CategoryID)
                                        {


                                            if (item.ApprCost != null)
                                            {

                                                catbudget.CategoryTotals +=  Math.Round((decimal)item.ApprCost, MidpointRounding.AwayFromZero);

                                              
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
                            if (item.SubmitDate.ToString().Contains((int.Parse(yr) - 1).ToString()) && item.ApprovalSH == true)
                            {
                                if (int.Parse(item.Category) == topcat.CategoryID)
                                {
                                    P_Category_Totals[catcount] += Math.Round((decimal)item.ApprCost, MidpointRounding.AwayFromZero);
                                    U_Category_Totals[catcount] += item.OrderPrice == null ? (decimal)0.0 : Math.Round((decimal)item.OrderPrice, MidpointRounding.AwayFromZero);

                                    Cancelled_Category_Totals[catcount] += item.OrderStatus != null ? (item.OrderStatus.Trim() == lstOrderStatus.Find(status => status.OrderStatus.Trim().Equals("Cancelled")).ID.ToString() ? Math.Round((decimal)item.ApprCost, MidpointRounding.AwayFromZero) : (decimal)0.0) : (decimal)0.0;

                                }
                            }
                        }
                    }

                    tempobj.P_Category_Totals = P_Category_Totals;
                    tempobj.U_Category_Totals = U_Category_Totals;
                    tempobj.Cancelled_Category_Totals = Cancelled_Category_Totals;
                    viewList.Add(tempobj);

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
                    dt.Columns.Add("VKM" + " " + year + "(Planned)", typeof(string)); //add vkm text to yr
                    dt.Columns.Add("VKM" + " " + year + "(Used)", typeof(string));
                    dt.Columns.Add("VKM" + " " + year + "(Projected)", typeof(string));

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
                    foreach (var info in viewList)
                    {



                        dr[rcnt] = info.P_Category_Totals[i].ToString("C", CultureInfo.CurrentCulture);
                        dr[rcnt1] = info.U_Category_Totals[i].ToString("C", CultureInfo.CurrentCulture);
                        dr[rcnt2] = (info.P_Category_Totals[i] - info.U_Category_Totals[i] -  info.Cancelled_Category_Totals[i]).ToString("C", CultureInfo.CurrentCulture);
                        //rcnt++;
                        rcnt = rcnt + 3;
                        rcnt1 = rcnt1 + 3;
                        rcnt2 = rcnt2 + 3;

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

            if (lstUsers == null)
            {
                return RedirectToAction("Index", "Budgeting");
            }

            using (BudgetingToolServerDBEntities1 db = new BudgetingToolServerDBEntities1())
            {
                List<RequestItems_Table> reqList = new List<RequestItems_Table>();
                List<RequestItems_Table> reqList_forquery = new List<RequestItems_Table>();
                reqList = db.RequestItems_Table.ToList<RequestItems_Table>().FindAll(v => v.Fund == null || v.Fund.Trim().Contains("2"));
                List<RequestItemsList_2019> reqList2019 = new List<RequestItemsList_2019>();
                List<ItemBudget> itemBudgets = new List<ItemBudget>();
                List<ItemBudget> Top_ItemBudget = new List<ItemBudget>();
                List<category_drilldownSummary> viewList = new List<category_drilldownSummary>();
                string costelement_chosen_id = string.Empty;
                int BaseYear_toCompare = 0;

                //CostElement_Chosen is costelt name -> fetch its id
                costelement_chosen_id = lstCostElement.Find(cost => cost.CostElement.ToUpper().Trim() == CostElement_Chosen.ToUpper().Trim()).ID.ToString().Trim();

                //Category_Chosen is costelt name -> fetch its id
                Category_Chosen = lstPrdCateg.Find(cat => cat.Category.ToUpper().Trim() == Category_Chosen.ToUpper().Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", "")).ID.ToString().Trim();

                foreach (var item in lstItems.Where(x => x.Category.Contains(Category_Chosen.ToString().Trim())))
                {
                    ItemBudget ibud = new ItemBudget();
                    ibud.ItemID = item.S_No;
                    ibud.ItemName = item.Item_Name;


                    if (itemBudgets.Count > 0 && itemBudgets.FindAll(ib => ib.ItemName.Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", "") == (ibud.ItemName.Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", ""))).Count != 0)
                        continue;

                    itemBudgets.Add(ibud);
                }
                foreach (var bu_item in buList)
                {

                    reqList_forquery.AddRange(reqList.FindAll(item => item.CostElement.Trim() == costelement_chosen_id).FindAll(ss => ss.BU.Contains(bu_item)).FindAll(item => item.Category.Trim() == Category_Chosen.ToString().Trim()));

                }

                //CODE TO GET GROUPS OF COST ELEMENT


                foreach (string yr in years)
                {
                    BaseYear_toCompare++;
                    //CC XC CHECK
                    //if(yr == "2020")
                    //{
                    //    reqList_forquery = reqList_forquery/*.FindAll(ss => ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("AS")).ID.ToString()) || ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("2WP")).ID.ToString()))*/;
                    //}
                    //else
                    if ((int.Parse(yr) - 1) > 2020)  //for 2020 reqList_forquery has relevant data
                    {
                        string is_CCXC = string.Empty;
                        bool CCXCflag = false;
                        string presentUserDept = string.Empty;

                        if (selected_ccxc.ToUpper().Contains("CC"))
                        {
                            is_CCXC = "CC";
                            CCXCflag = true;
                        }
                        else if (selected_ccxc.ToUpper().Contains("XC"))
                        {
                            is_CCXC = "XC";
                            CCXCflag = true;
                        }

                        else                    //DATA FILTER BASED ON USER'S NTID
                        {
                            presentUserDept = lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).Department;

                            if (presentUserDept.ToUpper().Contains("CC"))
                            {
                                is_CCXC = "CC";
                                CCXCflag = true;
                            }
                            else if (presentUserDept.ToUpper().Contains("XC"))
                            {
                                is_CCXC = "XC";
                                CCXCflag = true;
                            }

                        }

                        if (CCXCflag)
                        {
                            if (is_CCXC.Contains("XC"))
                            {
                                reqList_forquery.AddRange(reqList.FindAll(ss => ss.BU.Contains(lstBUs.Find(bu => bu.BU.Contains("2WP")).ID.ToString())));
                            }
                            reqList_forquery = reqList_forquery.FindAll(dpt => lstDEPTs.Find(x => x.ID.ToString().Contains(dpt.DEPT)).DEPT.Contains(is_CCXC))/*.FindAll(ss => ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("AS")).ID.ToString()) || ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("2WP")).ID.ToString()))*/;
                            CCXCflag = false;
                        }



                    }


                    // decimal Category_Totals[10] = new decimal[10];
                    decimal[] P_Item_Totals = new decimal[itemBudgets.Count()];
                    decimal[] U_Item_Totals = new decimal[itemBudgets.Count()];
                    decimal[] Cancelled_Item_Totals = new decimal[itemBudgets.Count()];
                    
                    category_drilldownSummary tempobj = new category_drilldownSummary();
                    tempobj.vkmyear = yr;


                    foreach (var itembudget in itemBudgets)
                    {
                        foreach (RequestItems_Table item in reqList_forquery)
                        {
                            if (item.SubmitDate.ToString().Contains((int.Parse(yr) - 1).ToString()) &&  item.ApprovalSH == true)   // 2020 dh apprvd - 2021 planning
                            {
                                //if ((int.Parse(yr) - 1).ToString() == "2020")
                                if (BaseYear_toCompare == 1 || (BaseYear_toCompare == 2 && Top_ItemBudget.Count() == 0))
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
                            if (item.SubmitDate.ToString().Contains((int.Parse(yr) - 1).ToString()) && item.ApprovalSH == true)
                            {
                                if (lstItems.Find(x => x.S_No == int.Parse(item.ItemName)).Item_Name.Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", "") == topitem.ItemName.Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", ""))
                                //if (int.Parse(item.ItemName) == topitem.ItemID)
                                {
                                    P_Item_Totals[itemcount] += Math.Round((decimal)item.ApprCost, MidpointRounding.AwayFromZero);
                                    U_Item_Totals[itemcount] += item.OrderPrice == null ? (decimal)0.0 : Math.Round((decimal)item.OrderPrice, MidpointRounding.AwayFromZero);
                                    Cancelled_Item_Totals[itemcount] += item.OrderStatus != null ? (item.OrderStatus.Trim() == lstOrderStatus.Find(status => status.OrderStatus.Trim().Equals("Cancelled")).ID.ToString() ? Math.Round((decimal)item.ApprCost, MidpointRounding.AwayFromZero) : (decimal)0.0) : (decimal)0.0;

                                }


                            }
                        }
                    }


                    tempobj.P_Item_Totals = P_Item_Totals;
                    tempobj.U_Item_Totals = U_Item_Totals;
                    tempobj.Cancelled_Item_Totals = Cancelled_Item_Totals;
                    viewList.Add(tempobj);

                }


                var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                BudgetParam t = new BudgetParam();
                List<columnsinfo> _col = new List<columnsinfo>();
                System.Data.DataTable dt = new System.Data.DataTable();
                dt.Columns.Add("Item Name", typeof(string)); /// Planned + (year variable) / Utilized  + (year variable)
                //dt.Columns.Add("VKM 2020", typeof(string));
                foreach (string year in years)
                {
                    dt.Columns.Add("VKM" + " " + year + "(Planned)", typeof(string)); //add vkm text to yr
                    dt.Columns.Add("VKM" + " " + year + "(Used)", typeof(string));
                    dt.Columns.Add("VKM" + " " + year + "(Projected)", typeof(string));
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
                    foreach (var info in viewList)
                    {



                        dr[rcnt] = info.P_Item_Totals[i].ToString("C", CultureInfo.CurrentCulture);
                        dr[rcnt1] = info.U_Item_Totals[i].ToString("C", CultureInfo.CurrentCulture);
                        dr[rcnt2] = (info.P_Item_Totals[i] - info.U_Item_Totals[i] - info.Cancelled_Item_Totals[i]).ToString("C", CultureInfo.CurrentCulture);
                        //rcnt++;
                        rcnt = rcnt + 3;
                        rcnt1 = rcnt1 + 3;
                        rcnt2 = rcnt2 + 3;
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

        //    using (BudgetingToolServerDBEntities1 db = new BudgetingToolServerDBEntities1())
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

        //    using (BudgetingToolServerDBEntities1 db = new BudgetingToolServerDBEntities1())
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

        //    using (BudgetingToolServerDBEntities1 db = new BudgetingToolServerDBEntities1())
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

        //    using (BudgetingToolServerDBEntities1 db = new BudgetingToolServerDBEntities1())
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

        //    using (BudgetingToolServerDBEntities1 db = new BudgetingToolServerDBEntities1())
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

        //    using (BudgetingToolServerDBEntities1 db = new BudgetingToolServerDBEntities1())
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

        //    using (BudgetingToolServerDBEntities1 db = new BudgetingToolServerDBEntities1())
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

        //    using (BudgetingToolServerDBEntities1 db = new BudgetingToolServerDBEntities1())
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

        //    using (BudgetingToolServerDBEntities1 db = new BudgetingToolServerDBEntities1())
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

        //    using (BudgetingToolServerDBEntities1 db = new BudgetingToolServerDBEntities1())
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



        public ActionResult BUs_forpresentNTID_CCXC(bool ccxc = false, string selected_ccxc = "")
        {
            string presentNTID = User.Identity.Name.Split('\\')[1].ToUpper();
            string presentUserDept = string.Empty;
            List<string> allowedBUs = new List<string>();
            List<string> cc = new List<string>()
                { "MB", "2WP", "OSS"};
            List<string> xc = new List<string>()
                { "DA", "AD"};

            if (ccxc == true)
            {
                if (selected_ccxc.ToUpper().Trim().Contains("CC"))
                {
                    foreach (var bu_cc in cc)
                    {
                        allowedBUs.Add(lstBUs.Find(bu => bu.BU.Contains(bu_cc)).ID.ToString());
                    }
                }
                else if (selected_ccxc.ToUpper().Trim().Contains("XC"))
                {
                    foreach (var bu_xc in xc)
                    {
                        allowedBUs.Add(lstBUs.Find(bu => bu.BU.Contains(bu_xc)).ID.ToString());
                    }
                }
                else
                {

                    if (lstBU_SPOCs.Find(person => person.VKMspoc.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())) != null)
                    {
                        var VKMSPOC = lstBU_SPOCs.FindAll(e => e.VKMspoc.ToUpper().Trim().Equals(User.Identity.Name.Split('\\')[1].ToUpper()));
                        foreach (var i in VKMSPOC)
                        {
                            allowedBUs.Add(i.BU.ToString());
                        }



                    }
                    else if (lstPrivileged.Find(person => person.ADSID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())) != null)
                    {
                        var BU_of_VKMSPOC = lstPrivileged.Find(e => e.ADSID.ToUpper().Trim().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).BU;
                        if (BU_of_VKMSPOC != null)
                        {
                            allowedBUs = (BU_of_VKMSPOC.Split(',')).ToList();

                        }
                    }
                    else
                    {
                        presentUserDept = lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).Department;

                        if (presentUserDept.ToUpper().Contains("CC"))
                        {
                            foreach (var bu_cc in cc)
                            {
                                allowedBUs.Add(lstBUs.Find(bu => bu.BU.Contains(bu_cc)).ID.ToString());
                            }
                        }
                        else if (presentUserDept.ToUpper().Contains("XC"))
                        {
                            foreach (var bu_xc in xc)
                            {
                                allowedBUs.Add(lstBUs.Find(bu => bu.BU.Contains(bu_xc)).ID.ToString());
                            }
                        }
                    }
                    //else
                    //{
                    //    if (presentNTID == "MAE9COB")
                    //    {
                    //        allowedBUs.Add("1");
                    //        allowedBUs.Add("3");
                    //    }
                    //    else
                    //    {
                    //        return Json(new { success = false }, JsonRequestBehavior.AllowGet);
                    //    }
                    //}


                }
            }

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

        //    using (BudgetingToolServerDBEntities1 db = new BudgetingToolServerDBEntities1())
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
        public ActionResult ItemDrillDown_Index(string Years_Listasstring, string CostElement_Chosen, string Category_Chosen, string Item_Chosen/*, List<string> buList, List<string> years*/, string BuList_Listasstring, string selected_ccxc = "")
        {

            List<string> years = new List<string>();
            List<string> buList = new List<string>();
            string costelement_chosen_id = string.Empty;

            RequestListAttributes viewList_itemdrilldown1 = new RequestListAttributes()
            {
                RequestItemsRepoView_model = new List<RequestItemsRepoView>()
            };

            if (Years_Listasstring != null)
            {
                years = (Years_Listasstring.Split(',')).ToList();

            }
            if (BuList_Listasstring != null)
            {
                buList = (BuList_Listasstring.Split(',')).ToList();

            }
            //if CostElement_Chosen is costelt name -> fetch its id
            costelement_chosen_id = lstCostElement.Find(cost => cost.CostElement.ToUpper().Trim() == CostElement_Chosen.ToUpper().Trim()).ID.ToString().Trim();
            Category_Chosen = lstPrdCateg.Find(cat => cat.Category.ToUpper().Trim() == Category_Chosen.ToUpper().Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", "")).ID.ToString().Trim();
            Item_Chosen = lstItems.Find(item => item.S_No == int.Parse(Item_Chosen)).Item_Name;



            using (BudgetingToolServerDBEntities1 db = new BudgetingToolServerDBEntities1())
            {
                List<RequestItems_Table> reqList = new List<RequestItems_Table>();



                foreach (var bu_item in buList)
                {
                    //reqList_forquery ;
                    reqList.AddRange(db.RequestItems_Table.ToList<RequestItems_Table>().FindAll(item => item.ApprovedSH == true).FindAll(item => item.CostElement.Trim() == costelement_chosen_id).FindAll(ss => ss.BU.Contains(bu_item)).FindAll(item => item.Category.Trim() == Category_Chosen).FindAll(v => v.Fund == null || v.Fund.Trim().Contains("2")).FindAll(item => lstItems.Find(i => i.S_No.ToString().Trim() == item.ItemName.Trim()).Item_Name.ToUpper().Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", "").Replace("+", "").Replace(" ", string.Empty) == Item_Chosen.ToUpper().Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", "").Replace("+", "").Replace(" ", string.Empty)));//lstItems.Find(item => item.Item_Name.ToUpper().Trim() == Item_Chosen.ToUpper().Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", "")).S_No.ToString().Trim();



                }

                foreach (string yr in years)
                {
                    if (reqList.FindAll(z => z.ApprovedSH == true && z.SubmitDate.Value.Year.ToString() == (int.Parse(yr) - 1).ToString().Trim()).Count != 0)
                        reqList = reqList.FindAll(z => z.SubmitDate.Value.Year.ToString() == (int.Parse(yr) - 1).ToString().Trim());
                    else
                        continue;
                    //CC XC CHECK
                    //if(yr == "2020")
                    //{
                    //    reqList_forquery = reqList_forquery/*.FindAll(ss => ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("AS")).ID.ToString()) || ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("2WP")).ID.ToString()))*/;
                    //}
                    //else
                    if ((int.Parse(yr) - 1) > 2020)  //for 2020 reqList_forquery has relevant data
                    {
                        string is_CCXC = string.Empty;
                        bool CCXCflag = false;
                        string presentUserDept = string.Empty;
                        if (selected_ccxc != "") //DATA FILTER BASED ON CCXC DROPDOWM - NOT USER'S NTID
                        {
                            if (selected_ccxc.ToUpper().Contains("CC"))
                            {
                                is_CCXC = "CC";
                                CCXCflag = true;
                            }
                            else if (selected_ccxc.ToUpper().Contains("XC"))
                            {
                                is_CCXC = "XC";
                                CCXCflag = true;
                            }

                        }
                        else                    //DATA FILTER BASED ON USER'S NTID
                        {
                            presentUserDept = lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).Department;

                            if (presentUserDept.ToUpper().Contains("CC"))
                            {
                                is_CCXC = "CC";
                                CCXCflag = true;
                            }
                            else if (presentUserDept.ToUpper().Contains("XC"))
                            {
                                is_CCXC = "XC";
                                CCXCflag = true;
                            }

                        }

                        if (CCXCflag)
                        {
                            if (is_CCXC.Contains("XC"))
                            {
                                reqList = reqList.FindAll(ss => ss.BU.Contains(lstBUs.Find(bu => bu.BU.Contains("2WP")).ID.ToString()));
                            }
                            reqList = reqList.FindAll(dpt => lstDEPTs.Find(x => x.ID.ToString().Contains(dpt.DEPT)).DEPT.Contains(is_CCXC))/*.FindAll(ss => ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("AS")).ID.ToString()) || ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("2WP")).ID.ToString()))*/;
                            CCXCflag = false;
                        }



                    }
                    //reqList = db.RequestItems_Table.ToList<RequestItems_Table>().FindAll(z=>z.CostElement.Contains(costelement_chosen_id.Trim())).FindAll(z=>z.Category.Contains(Category_Chosen.ToString())).FindAll(z=>z.ItemName.Contains(Item_Chosen.ToString()))/*.FindAll(x=>x.ApprovalDH == false)*/;

                    db.Database.CommandTimeout = 500;


                    foreach (RequestItems_Table item in reqList)
                    {

                        RequestItemsRepoView ritem = new RequestItemsRepoView();

                        ritem.Category = int.Parse(item.Category);
                        ritem.Comments = item.Comments;
                        ritem.Cost_Element = int.Parse(item.CostElement);
                        ritem.BU = int.Parse(item.BU);

                        ritem.DEPT = int.Parse(item.DEPT);
                        ritem.Group = int.Parse(item.Group);
                        ritem.Item_Name = int.Parse(item.ItemName);
                        ritem.OEM = int.Parse(item.OEM);
                        ritem.Required_Quantity = item.ReqQuantity;
                        ritem.Reviewed_Quantity = item.ApprQuantity;
                        ritem.RequestID = item.RequestID;

                        ritem.Requestor = item.RequestorNT;
                        ritem.Total_Price = item.TotalPrice;
                        ritem.Unit_Price = item.UnitPrice;
                        ritem.ApprovalHoE = item.ApprovalDH;
                        ritem.ApprovalSH = item.ApprovalSH;
                        ritem.ApprovedHoE = item.ApprovedDH;
                        ritem.ApprovedSH = item.ApprovedSH;
                        if (item.isCancelled != null)
                        {
                            ritem.isCancelled = (int)item.isCancelled;
                        }


                        ritem.Total_Price = item.TotalPrice;
                        ritem.Reviewed_Cost = item.ApprCost;
                        ritem.OrderedQuantity = item.OrderedQuantity;
                        ritem.OrderPrice = item.OrderPrice;

                        if (item.OrderStatus != null && item.OrderStatus.Trim() != "")
                        {
                            ritem.OrderStatus = int.Parse(item.OrderStatus);

                        }
                        else
                        {
                            ritem.OrderStatus = null;


                        }
                        ritem.OrderID = item.OrderID;
                        ritem.Reviewer_1 = item.DHNT;
                        ritem.Reviewer_2 = item.SHNT;

                        ritem.RequestDate = item.RequestDate != null ? ((DateTime)item.RequestDate).ToString("yyyy-MM-dd") : string.Empty;
                        ritem.SubmitDate = item.SubmitDate != null ? ((DateTime)item.SubmitDate).ToString("yyyy-MM-dd") : string.Empty;
                        ritem.Review1_Date = item.DHAppDate != null ? ((DateTime)item.DHAppDate).ToString("yyyy-MM-dd") : string.Empty;
                        ritem.Review2_Date = item.SHAppDate != null ? ((DateTime)item.SHAppDate).ToString("yyyy-MM-dd") : string.Empty;


                        if (item.OrderStatus != null && item.OrderStatus.Trim() != "")
                        {
                            ritem.OrderStatus = int.Parse(item.OrderStatus);

                        }
                        else
                        {
                            ritem.OrderStatus = null;


                        }

                        //Checking Request Status
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









                        viewList_itemdrilldown1.RequestItemsRepoView_model.Add(ritem);
                        viewList_itemdrilldown1.Item_Name = Item_Chosen.Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", "");
                        // viewList_itemdrilldown1.Add(ritem);


                    }
                }
                // return viewList.FindAll(xi => xi.RequestDate.ToString().Contains(year))/*.FindAll(x => x.ApprovalHoE == false)*/;


            }

            return View(viewList_itemdrilldown1);

        }



        [HttpPost]
        public ActionResult ItemDrillDown(string CostElement_Chosen, string Category_Chosen, string Item_Chosen, List<string> buList, List<string> years, string selected_ccxc = "")
        {

            string costelement_chosen_id = string.Empty;
            List<RequestItemsRepoView> viewList_itemdrilldown1 = new List<RequestItemsRepoView>();

            costelement_chosen_id = lstCostElement.Find(cost => cost.CostElement.ToUpper().Trim() == CostElement_Chosen.ToUpper().Trim()).ID.ToString().Trim();
            Category_Chosen = lstPrdCateg.Find(cat => cat.Category.ToUpper().Trim() == Category_Chosen.ToUpper().Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", "")).ID.ToString().Trim();
            Item_Chosen = lstItems.Find(item => item.S_No == int.Parse(Item_Chosen)).Item_Name;

            using (BudgetingToolServerDBEntities1 db = new BudgetingToolServerDBEntities1())
            {
                List<RequestItems_Table> reqList = new List<RequestItems_Table>();
                List<RequestItems_Table> reqList1 = new List<RequestItems_Table>();



                foreach (var bu_item in buList)
                {
                    reqList.AddRange(db.RequestItems_Table.ToList<RequestItems_Table>().FindAll(item => item.ApprovedSH == true).FindAll(item => item.CostElement.Trim() == costelement_chosen_id).FindAll(v => v.Fund == null || v.Fund.Trim().Contains("2")).FindAll(ss => ss.BU.Contains(bu_item)).FindAll(item => item.Category.Trim() == Category_Chosen).FindAll(item => lstItems.Find(i => i.S_No.ToString().Trim() == item.ItemName.Trim()).Item_Name.ToUpper().Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", "").Replace("+", "").Replace(" ", string.Empty) == Item_Chosen.ToUpper().Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", "").Replace("+", "").Replace(" ", string.Empty)));//lstItems.Find(item => item.Item_Name.ToUpper().Trim() == Item_Chosen.ToUpper().Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", "")).S_No.ToString().Trim();

                }

                foreach (string yr in years)
                {
                    if (reqList.FindAll(z => z.ApprovedSH == true && z.SubmitDate.Value.Year.ToString() == (int.Parse(yr) - 1).ToString().Trim()).Count != 0)
                        reqList1 = reqList.FindAll(z => z.SubmitDate.Value.Year.ToString() == (int.Parse(yr) - 1).ToString().Trim());
                    else
                        continue;
                    if ((int.Parse(yr) - 1) > 2020)  //for 2020 reqList_forquery has relevant data
                    {
                        string is_CCXC = string.Empty;
                        bool CCXCflag = false;
                        string presentUserDept = string.Empty;
                        if (selected_ccxc != "") //DATA FILTER BASED ON CCXC DROPDOWM - NOT USER'S NTID
                        {
                            if (selected_ccxc.ToUpper().Contains("CC"))
                            {
                                is_CCXC = "CC";
                                CCXCflag = true;
                            }
                            else if (selected_ccxc.ToUpper().Contains("XC"))
                            {
                                is_CCXC = "XC";
                                CCXCflag = true;
                            }

                        }
                        else                    //DATA FILTER BASED ON USER'S NTID
                        {
                            presentUserDept = lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).Department;

                            if (presentUserDept.ToUpper().Contains("CC"))
                            {
                                is_CCXC = "CC";
                                CCXCflag = true;
                            }
                            else if (presentUserDept.ToUpper().Contains("XC"))
                            {
                                is_CCXC = "XC";
                                CCXCflag = true;
                            }

                        }

                        if (CCXCflag)
                        {
                            if (is_CCXC.Contains("XC"))
                            {
                                reqList1 = reqList.FindAll(ss => ss.BU.Contains(lstBUs.Find(bu => bu.BU.Contains("2WP")).ID.ToString()));
                            }
                            reqList1 = reqList.FindAll(dpt => lstDEPTs.Find(x => x.ID.ToString().Contains(dpt.DEPT)).DEPT.Contains(is_CCXC))/*.FindAll(ss => ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("AS")).ID.ToString()) || ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("2WP")).ID.ToString()))*/;
                            CCXCflag = false;
                        }



                    }
                    //reqList = db.RequestItems_Table.ToList<RequestItems_Table>().FindAll(z=>z.CostElement.Contains(costelement_chosen_id.Trim())).FindAll(z=>z.Category.Contains(Category_Chosen.ToString())).FindAll(z=>z.ItemName.Contains(Item_Chosen.ToString()))/*.FindAll(x=>x.ApprovalDH == false)*/;

                    db.Database.CommandTimeout = 500;


                    foreach (RequestItems_Table item in reqList1)
                    {

                        RequestItemsRepoView ritem = new RequestItemsRepoView();

                        ritem.Category = int.Parse(item.Category);
                        ritem.Comments = item.Comments;
                        ritem.Cost_Element = int.Parse(item.CostElement);
                        ritem.BU = int.Parse(item.BU);

                        ritem.DEPT = int.Parse(item.DEPT);
                        ritem.Group = int.Parse(item.Group);
                        ritem.Item_Name = int.Parse(item.ItemName);
                        ritem.OEM = int.Parse(item.OEM);
                        ritem.Required_Quantity = item.ReqQuantity;
                        ritem.RequestID = item.RequestID;
                        ritem.Reviewed_Quantity = item.ApprQuantity;

                        ritem.Requestor = item.RequestorNT;
                        ritem.Total_Price = item.TotalPrice;
                        ritem.Unit_Price = item.UnitPrice;
                        ritem.ApprovalHoE = item.ApprovalDH;
                        ritem.ApprovalSH = item.ApprovalSH;
                        ritem.ApprovedHoE = item.ApprovedDH;
                        ritem.ApprovedSH = item.ApprovedSH;
                        if (item.isCancelled != null)
                        {
                            ritem.isCancelled = (int)item.isCancelled;
                        }


                        ritem.Total_Price = item.TotalPrice;
                        ritem.Reviewer_1 = item.DHNT;
                        ritem.Reviewer_2 = item.SHNT;

                        ritem.RequestDate = item.RequestDate != null ? ((DateTime)item.RequestDate).ToString("yyyy-MM-dd") : string.Empty;
                        ritem.SubmitDate = item.SubmitDate != null ? ((DateTime)item.SubmitDate).ToString("yyyy-MM-dd") : string.Empty;
                        ritem.Review1_Date = item.DHAppDate != null ? ((DateTime)item.DHAppDate).ToString("yyyy-MM-dd") : string.Empty;
                        ritem.Review2_Date = item.SHAppDate != null ? ((DateTime)item.SHAppDate).ToString("yyyy-MM-dd") : string.Empty;


                        if (item.OrderStatus != null && item.OrderStatus.Trim() != "")
                        {
                            ritem.OrderStatus = int.Parse(item.OrderStatus);

                        }
                        else
                        {
                            ritem.OrderStatus = null;


                        }

                        //Checking Request Status
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

                        ritem.Reviewed_Cost = item.ApprCost;
                        ritem.OrderedQuantity = item.OrderedQuantity;
                        ritem.OrderPrice = item.OrderPrice;

                        if (item.OrderStatus != null && item.OrderStatus.Trim() != "")
                        {
                            ritem.OrderStatus = int.Parse(item.OrderStatus);

                        }
                        else
                        {
                            ritem.OrderStatus = null;
                        }
                        ritem.OrderID = item.OrderID;

                        viewList_itemdrilldown1.Add(ritem);


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
        //    //using(BudgetingToolServerDBEntities1 db = new BudgetingToolServerDBEntities1())
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
        //    using (BudgetingToolServerDBEntities1 db = new BudgetingToolServerDBEntities1())
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
        //    using (BudgetingToolServerDBEntities1 db = new BudgetingToolServerDBEntities1())
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

        //    using (BudgetingToolServerDBEntities1 db = new BudgetingToolServerDBEntities1())
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

        //    using (BudgetingToolServerDBEntities1 db = new BudgetingToolServerDBEntities1())
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

        ////    using (BudgetingToolServerDBEntities1 db = new BudgetingToolServerDBEntities1())
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

        //    using (BudgetingToolServerDBEntities1 db = new BudgetingToolServerDBEntities1())
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

        ////    using (BudgetingToolServerDBEntities1 db = new BudgetingToolServerDBEntities1())
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

        ////    using (BudgetingToolServerDBEntities1 db = new BudgetingToolServerDBEntities1())
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



        //    using (BudgetingToolServerDBEntities1 db = new BudgetingToolServerDBEntities1())
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

        //    using (BudgetingToolServerDBEntities1 db = new BudgetingToolServerDBEntities1())
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



        public partial class RequestListAttributes
        {

            public List<RequestItemsRepoView> RequestItemsRepoView_model { get; set; }
            public string Item_Name { get; set; }
        }

        [HttpGet]
        public ActionResult Lookup()
        {
            LookupData lookupData = new LookupData();



            string presentUserDept = lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).Department;
            string is_CCXC = string.Empty; //For Clear CC XC Segregation

            lookupData.DEPT_List = lstDEPTs.ToList();

            lookupData.BU_List = lstBUs.OrderBy(x => x.ID).ToList();
            lookupData.BU_List[2].BU = "MB";
            lookupData.BU_List[4].BU = "OSS";
            lookupData.OEM_List = lstOEMs;
            //lookupData.Groups_oldList = lstGroups_old;
            //using(BudgetingToolServerDBEntities1 db = new BudgetingToolServerDBEntities1())
            lookupData.Groups_test = lstGroups_test;
            // lookupData.Groups_List = lstGroups;
            lookupData.Item_List = lstItems;
            lookupData.Category_List = lstPrdCateg;
            lookupData.CostElement_List = lstCostElement;

            return Json(new { data = lookupData }, JsonRequestBehavior.AllowGet);

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




        }

        public partial class costelement_drilldownSummary
        {
            public string vkmyear { get; set; }
            public decimal[] P_Category_Totals { get; set; }
            public decimal[] U_Category_Totals { get; set; }
            public decimal[] Cancelled_Category_Totals { get; set; }

        }

        public partial class category_drilldownSummary
        {
            public string vkmyear { get; set; }
            public decimal[] P_Item_Totals { get; set; }
            public decimal[] U_Item_Totals { get; set; }
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
//            using (BudgetingToolServerDBEntities1 db = new BudgetingToolServerDBEntities1())
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

//            using (BudgetingToolServerDBEntities1 db = new BudgetingToolServerDBEntities1())
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

//            using (BudgetingToolServerDBEntities1 db = new BudgetingToolServerDBEntities1())
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

//        //    using (BudgetingToolServerDBEntities1 db = new BudgetingToolServerDBEntities1())
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

//            using (BudgetingToolServerDBEntities1 db = new BudgetingToolServerDBEntities1())
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

//        //    using (BudgetingToolServerDBEntities1 db = new BudgetingToolServerDBEntities1())
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


//        //public ActionResult ItemDrillDown_Index()
//        //{
//        //    return View();
//        //}

//        // [HttpPost]
//        //public ActionResult ItemDrillDown( /*string Years_Listasstring,*/ string CostElement_Chosen, int Category_Chosen, int Item_Chosen, List<string> buList, List<string> years,/*string BuList_Listasstring,*/ string selected_ccxc = "")
//        //{

//        //    //List<string> years = new List<string>();
//        //    //List<string> buList = new List<string>();
//        //    string costelement_chosen_id = string.Empty;

//        //    //RequestListAttributes viewList = new 


//        //    List<RequestItemsRepoView> viewList_itemdrilldown1 = new List<RequestItemsRepoView>();
//        //    //if (Years_Listasstring != null)
//        //    //{
//        //    //    years = (Years_Listasstring.Split(',')).ToList();

//        //    //}
//        //    //if (BuList_Listasstring != null)
//        //    //{
//        //    //    buList = (BuList_Listasstring.Split(',')).ToList();

//        //    //}
//        //    //if CostElement_Chosen is costelt name -> fetch its id
//        //    costelement_chosen_id = lstCostElement.Find(cost => cost.CostElement.ToUpper().Trim() == CostElement_Chosen.ToUpper().Trim()).ID.ToString().Trim();

//        //    using (BudgetingToolServerDBEntities1 db = new BudgetingToolServerDBEntities1())
//        //    {
//        //        List<RequestItems_Table_TEST> reqList = new List<RequestItems_Table_TEST>();



//        //        foreach (var bu_item in buList)
//        //        {
//        //            //reqList_forquery ;
//        //            reqList.AddRange(db.RequestItems_Table_TEST.ToList<RequestItems_Table_TEST>().FindAll(item => item.CostElement.Trim() == costelement_chosen_id).FindAll(ss => ss.BU.Contains(bu_item)).FindAll(item => item.Category.Trim() == Category_Chosen.ToString().Trim()).FindAll(z => z.ItemName.Contains(Item_Chosen.ToString())));

//        //        }

//        //        foreach (string yr in years)
//        //        {
//        //            reqList = reqList.FindAll(z => z.SubmitDate.Value.Year.ToString() == yr.Trim());
//        //            //CC XC CHECK
//        //            //if(yr == "2020")
//        //            //{
//        //            //    reqList_forquery = reqList_forquery/*.FindAll(ss => ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("AS")).ID.ToString()) || ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("2WP")).ID.ToString()))*/;
//        //            //}
//        //            //else
//        //            if (yr != "2020")  //for 2020 reqList_forquery has relevant data
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
//        //                        reqList = reqList.FindAll(ss => ss.BU.Contains(lstBUs.Find(bu => bu.BU.Contains("2WP")).ID.ToString()));
//        //                    }
//        //                    reqList = reqList.FindAll(dpt => lstDEPTs.Find(x => x.ID.ToString().Contains(dpt.DEPT)).DEPT.Contains(is_CCXC))/*.FindAll(ss => ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("AS")).ID.ToString()) || ss.BU.Equals(lstBUs.Find(bu => bu.BU.Equals("2WP")).ID.ToString()))*/;
//        //                    CCXCflag = false;
//        //                }



//        //            }
//        //            //reqList = db.RequestItems_Table_TEST.ToList<RequestItems_Table_TEST>().FindAll(z=>z.CostElement.Contains(costelement_chosen_id.Trim())).FindAll(z=>z.Category.Contains(Category_Chosen.ToString())).FindAll(z=>z.ItemName.Contains(Item_Chosen.ToString()))/*.FindAll(x=>x.ApprovalDH == false)*/;

//        //            db.Database.CommandTimeout = 500;


//        //            foreach (RequestItems_Table_TEST item in reqList)
//        //            {

//        //                RequestItemsRepoView ritem = new RequestItemsRepoView();

//        //                ritem.Category = int.Parse(item.Category);
//        //                ritem.Comments = item.Comments;
//        //                ritem.Cost_Element = int.Parse(item.CostElement);
//        //                ritem.BU = int.Parse(item.BU);

//        //                ritem.DEPT = int.Parse(item.DEPT);
//        //                ritem.Group = int.Parse(item.Group);
//        //                ritem.Item_Name = int.Parse(item.ItemName);
//        //                ritem.OEM = int.Parse(item.OEM);
//        //                ritem.Required_Quantity = item.ReqQuantity;
//        //                ritem.RequestID = item.RequestID;

//        //                ritem.Requestor = item.RequestorNT;
//        //                ritem.Total_Price = item.TotalPrice;
//        //                ritem.Unit_Price = item.UnitPrice;
//        //                ritem.ApprovalHoE = item.ApprovalDH;
//        //                ritem.ApprovalSH = item.ApprovalSH;
//        //                ritem.ApprovedHoE = item.ApprovedDH;
//        //                ritem.ApprovedSH = item.ApprovedSH;
//        //                if (item.isCancelled != null)
//        //                {
//        //                    ritem.isCancelled = (int)item.isCancelled;
//        //                }


//        //                ritem.Total_Price = item.TotalPrice;
//        //                ritem.Reviewer_1 = item.DHNT;
//        //                ritem.Reviewer_2 = item.SHNT;

//        //                ritem.RequestDate = item.RequestDate != null ? ((DateTime)item.RequestDate).ToString("yyyy-MM-dd") : string.Empty;
//        //                ritem.SubmitDate = item.SubmitDate != null ? ((DateTime)item.SubmitDate).ToString("yyyy-MM-dd") : string.Empty;
//        //                ritem.Review1_Date = item.DHAppDate != null ? ((DateTime)item.DHAppDate).ToString("yyyy-MM-dd") : string.Empty;
//        //                ritem.Review2_Date = item.SHAppDate != null ? ((DateTime)item.SHAppDate).ToString("yyyy-MM-dd") : string.Empty;


//        //                if (item.OrderStatus != null && item.OrderStatus.Trim() != "")
//        //                {
//        //                    ritem.OrderStatus = int.Parse(item.OrderStatus);

//        //                }
//        //                else
//        //                {
//        //                    ritem.OrderStatus = null;


//        //                }

//        //                //Checking Request Status
//        //                if (ritem.ApprovalHoE == true && ritem.ApprovalSH == false)
//        //                {
//        //                    ritem.Request_Status = "In Review with HoE";
//        //                }
//        //                else if (ritem.ApprovalHoE == true && ritem.ApprovalSH == true && ritem.ApprovedSH == false)
//        //                {
//        //                    ritem.Request_Status = "In Review with VKM SPOC";
//        //                }
//        //                else if (ritem.ApprovalHoE == true && ritem.ApprovalSH == true && ritem.ApprovedSH == true)
//        //                {
//        //                    ritem.Request_Status = "Reviewed by VKM SPOC";
//        //                }
//        //                else if (ritem.ApprovalHoE == false && ritem.ApprovalSH == false && ritem.isCancelled == 1)
//        //                {
//        //                    ritem.Request_Status = "SentBack by HoE";
//        //                }
//        //                else if (ritem.ApprovalHoE == false && ritem.ApprovalSH == false && ritem.isCancelled == 2)
//        //                {
//        //                    ritem.Request_Status = "SentBack by VKM SPOC";
//        //                }
//        //                else
//        //                {
//        //                    ritem.Request_Status = "In Requestor's Queue";
//        //                }











//        //                viewList_itemdrilldown1.Add(ritem);


//        //            }
//        //        }
//        //        // return viewList.FindAll(xi => xi.RequestDate.ToString().Contains(year))/*.FindAll(x => x.ApprovalHoE == false)*/;


//        //    }
//        //    viewList_itemdrilldown = viewList_itemdrilldown1;
//        //    return Json(new { data = viewList_itemdrilldown1 }, JsonRequestBehavior.AllowGet);

//        //}

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



//            using (BudgetingToolServerDBEntities1 db = new BudgetingToolServerDBEntities1())
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
//            using(BudgetingToolServerDBEntities1 db = new BudgetingToolServerDBEntities1())
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

//        //public class BudgetParam
//        //{
//        //    public string jsondata { get; set; }
//        //    public string columns { get; set; }
//        //    public string data { get; set; }
//        //}
//        //public class columnsinfo
//        //{
//        //    public string title { get; set; }
//        //    public string data { get; set; }
//        //}


//        //}
//    }
//}