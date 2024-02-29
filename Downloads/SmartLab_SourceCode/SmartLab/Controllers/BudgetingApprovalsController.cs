using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;
using LC_Reports_V1.Models;
using System.Globalization;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using ClosedXML.Excel;
using System.Data.SqlClient;
using System.Configuration;

namespace LC_Reports_V1.Controllers
{
    public class BudgetingApprovalsController : Controller
    {
        private SqlConnection con, budgetingcon;
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

        private void connection()
        {
 			string constring = ConfigurationManager.ConnectionStrings["dbConnection"].ToString();
            string budgeting_constring = ConfigurationManager.ConnectionStrings["BudgetingdbConnection"].ToString();
            con = new SqlConnection(constring);
            budgetingcon = new SqlConnection(budgeting_constring);
        }
        // GET: ApproveItems
        public ActionResult Index()
        {
            WriteLog("********Budgeting Approval ************" + DateTime.Now.ToString());
            string NTID = HOE_authorise();

            if (NTID == "")
            {
                // throw new HttpException(404, "Sorry! Current user is not authorised to access this view!");
                return Content("Sorry! Current user is not authorised. Kindly contact SmartLab Tools Team for access! !");
                //return RedirectToAction("Index", "Dashboard");
            }
            else
            {
                if (BudgetingController.lstUsers == null || BudgetingController.lstUsers.Count == 0)
                {

                    return RedirectToAction("Index", "Budgeting");
                }
            }

            return View();
        }

        public string HOE_authorise()
        {

            string NTID = "";
            connection();
            BudgetingOpenConnection();
            string qry = " Exec [dbo].[HOE_Authorize] '" + User.Identity.Name.Split('\\')[1].Trim() + "' ";

            SqlCommand command = new SqlCommand(qry, budgetingcon);
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
            BudgetingCloseConnection();
            return NTID;
        }

        [HttpPost]
        public ActionResult HOEReviews_Authorise()
        {
            string NTID = HOE_authorise();

            return Json(new { data = NTID }, JsonRequestBehavior.AllowGet);
        }

        private void WriteLog(string Message)
        {
            string execPath = AppDomain.CurrentDomain.BaseDirectory;
            execPath = execPath + "Budgeting_Log\\log" + DateTime.Now.ToString("dd_MM_yyyy") + ".txt";
            StreamWriter file = new StreamWriter(execPath, append: true);
            file.WriteLine(Message + "\r\n");
            file.Close();
        }

        //private void WriteLog(string Message)
        //{
        //    string execPath = AppDomain.CurrentDomain.BaseDirectory;
        //    execPath = execPath + "ErrorLog\\log" + DateTime.Now.ToString("ddMMyyyy") + ".txt";
        //    StreamWriter file = new StreamWriter(execPath, append: true);
        //    file.WriteLine(Message + "\r\n");
        //    file.Close();
        //}

        //***NOTE**//
        //Have production DB & test db in same soln
        //#define BETA

        //#if BETA
        //    Test DB code
        //else
        //    PRODUCTION DB code


        /// <summary>
        /// function to get the list of items for L2 approval
        /// filtered by user name
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetData(string year)
        {

            using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

            {
                try
                {

                    List<RequestItemsRepoView> viewList = new List<RequestItemsRepoView>();
                    viewList = GetData1(year);
                    //Using Semapphore - ActiveNTID - access to only 1 at a time**************
                    //if (db.Planning_HOE_Table.ToList().FindAll(xi => xi.HOE_NTID == User.Identity.Name.Split('\\')[1].ToUpper()).Count() != 0) //Present user is HOE
                    //{

                    //    var hoelist = db.Planning_HOE_Table.ToList().FirstOrDefault(xi => xi.HOE_NTID == User.Identity.Name.Split('\\')[1].ToUpper());
                    //    if (hoelist.Active_NTID == null || !(hoelist.Active_NTID.Contains(hoelist.Proxy_NTID)))
                    //    {
                    //        viewList = GetData1();
                    //        hoelist.Active_NTID = hoelist.HOE_NTID;
                    //    }
                    //    else
                    //        return Json(new { success = false, data = viewList, message = "Requests in your Queue is currently being accessed by your Proxy! Please check after a while" }, JsonRequestBehavior.AllowGet);

                    //}
                    //else if (db.Planning_HOE_Table.ToList().FindAll(xi => xi.Proxy_NTID == User.Identity.Name.Split('\\')[1].ToUpper()).Count() != 0) //Present user is Proxy
                    //{

                    //    var proxylist = db.Planning_HOE_Table.ToList().FirstOrDefault(xi => xi.Proxy_NTID == User.Identity.Name.Split('\\')[1].ToUpper());
                    //    if (proxylist.Active_NTID == null || !(proxylist.Active_NTID.Contains(proxylist.HOE_NTID)))
                    //    {
                    //        viewList = GetData1();
                    //        proxylist.Active_NTID = proxylist.Proxy_NTID;
                    //    }
                    //    else
                    //        return Json(new { success = false, data = viewList, message = "Requests to be reviewed is currently being accessed by your HOE! Please check after a while" }, JsonRequestBehavior.AllowGet);

                    //}
                    //else
                    //    return Json(new { data = viewList }, JsonRequestBehavior.AllowGet);

                    //Using Proxy Settings Button******************************
                    //if (db.Planning_HOE_Table.ToList().FindAll(xi => xi.HOE_NTID == User.Identity.Name.Split('\\')[1].ToUpper()).Count() != 0) //Present user is HOE
                    //{
                    //    if (db.Planning_HOE_Table.ToList().Find(xi => xi.HOE_NTID == User.Identity.Name.Split('\\')[1].ToUpper()).Enable_Proxy == false
                    //        || db.Planning_HOE_Table.ToList().Find(xi => xi.HOE_NTID == User.Identity.Name.Split('\\')[1].ToUpper()).Enable_Proxy == null)
                    //        viewList = GetData1();
                    //    else
                    //        return Json(new { success = false, data = viewList, is_ProxyButtonHide = false, message = "Proxy Setting is enabled. Requests in your queue have been redirected to your Proxy. \n Please use \" Disable Proxy \" option to direct requests back to your queue!" }, JsonRequestBehavior.AllowGet);

                    //}
                    //else if (db.Planning_HOE_Table.ToList().FindAll(xi => xi.Proxy_NTID == User.Identity.Name.Split('\\')[1].ToUpper()).Count() != 0)
                    //{
                    //    if (db.Planning_HOE_Table.ToList().Find(xi => xi.Proxy_NTID == User.Identity.Name.Split('\\')[1].ToUpper()).Enable_Proxy == false
                    //        || db.Planning_HOE_Table.ToList().Find(xi => xi.Proxy_NTID == User.Identity.Name.Split('\\')[1].ToUpper()).Enable_Proxy == null)
                    //        return Json(new { success = true, data = viewList, is_ProxyButtonHide = true }, JsonRequestBehavior.AllowGet);
                    //    else
                    //    {
                    //        viewList = GetData1();
                    //        return Json(new {
                    //            success = true, data = viewList, is_ProxyButtonHide = true,
                    //            message = "Proxy Setting is enabled. Requests to \" " + db.Planning_HOE_Table.ToList().Find(xi => xi.Proxy_NTID == User.Identity.Name.Split('\\')[1].ToUpper()).HOE_FullName+  " \" have been redirected to your queue"
                    //        }, JsonRequestBehavior.AllowGet);
                    //    }
                    //}
                    //else
                    //    viewList = GetData1();


                    //********view to all ; but edit control to 1 at a time ; if disable proxy; hoe gets back access
                    //if (db.Planning_HOE_Table.ToList().FindAll(xi => xi.HOE_NTID == User.Identity.Name.Split('\\')[1].ToUpper()).Count() != 0) //Present user is HOE
                    //{
                    //    if (db.Planning_HOE_Table.ToList().Find(xi => xi.HOE_NTID == User.Identity.Name.Split('\\')[1].ToUpper()).Enable_Proxy == false
                    //        || db.Planning_HOE_Table.ToList().Find(xi => xi.HOE_NTID == User.Identity.Name.Split('\\')[1].ToUpper()).Enable_Proxy == null)
                    //    {
                    //        viewList = GetData1();
                    //        return Json(new { success = true, data = viewList, is_EditControlsgiven = true,  message = "Requests to be Reviewed in your Queue is fetched successfully!" }, JsonRequestBehavior.AllowGet);

                    //    }
                    //    else
                    //    {
                    //        viewList = GetData1();
                    //        return Json(new { success = true, data = viewList, is_EditControlsgiven = false,  message = "Requests in your Queue is fetched successfully.!" }, JsonRequestBehavior.AllowGet);

                    //    }

                    //}
                    //else if (db.Planning_HOE_Table.ToList().FindAll(xi => xi.Proxy_NTID == User.Identity.Name.Split('\\')[1].ToUpper()).Count() != 0)
                    //{
                    //    if (db.Planning_HOE_Table.ToList().Find(xi => xi.Proxy_NTID == User.Identity.Name.Split('\\')[1].ToUpper()).Enable_Proxy == false
                    //        || db.Planning_HOE_Table.ToList().Find(xi => xi.Proxy_NTID == User.Identity.Name.Split('\\')[1].ToUpper()).Enable_Proxy == null)
                    //    {
                    //        viewList = GetData1();
                    //        return Json(new { success = true, data = viewList, is_EditControlsgiven = false, message = "Requests under your HOE: \"" + db.Planning_HOE_Table.ToList().Find(xi => xi.Proxy_NTID == User.Identity.Name.Split('\\')[1].ToUpper()).HOE_FullName + " \" is fetched successfully.!" }, JsonRequestBehavior.AllowGet);
                    //    }

                    //    else
                    //    {
                    //        viewList = GetData1();
                    //        return Json(new
                    //        {
                    //            success = true,
                    //            data = viewList,
                    //            is_EditControlsgiven = true,
                    //            message = "Proxy Setting is enabled. Requests to be reviewed under \" " + db.Planning_HOE_Table.ToList().Find(xi => xi.Proxy_NTID == User.Identity.Name.Split('\\')[1].ToUpper()).HOE_FullName + " \" have been redirected to your queue"
                    //        }, JsonRequestBehavior.AllowGet);
                    //    }
                    //}
                    //else
                    //{
                    //    viewList = GetData1();
                    //    return Json(new { success = true, data = viewList, is_EditControlsgiven = true, message = "Requests to be Reviewed in your Queue is fetched successfully!" }, JsonRequestBehavior.AllowGet);

                    //}


                    //if (db.Planning_HOE_Table.ToList().FindAll(xi => xi.Proxy_NTID == User.Identity.Name.Split('\\')[1].ToUpper()).Count() != 0)
                    //{
                    //    return Json(new { success = true,data = viewList/*, is_ProxyButtonHide = true*/ }, JsonRequestBehavior.AllowGet);
                    //}
                    //else
                    //{
                    //    return Json(new { success = true,data = viewList/*, is_ProxyButtonHide = false*/ }, JsonRequestBehavior.AllowGet);
                    //}
                    //WriteLog("GetData function: Data fetched successfully " + DateTime.Now.ToString());
                    return Json(new { success = true, data = viewList }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    WriteLog("Error - GetData function : NTID - " + User.Identity.Name.Split('\\')[1].Trim().ToUpper() + ", ErrorMsg : " + ex.Message.ToString() + ", At - " + DateTime.Now.ToString());
                    return Json(new { success = false, message = "Unable to load the Item Requests, Please Try again later!" }, JsonRequestBehavior.AllowGet);

                }

            }
        }

        [HttpPost]
        public ActionResult GetL1Details(string RequestID)
        {

            DataTable dt = new DataTable();
            string RequestDate = "", Remarks = "", Qty= "";
            
            try
            {
                string query = " select convert(nvarchar,SubmitDate) as SubmitDate,Comments as L1Remarks,ReqQuantity as Qty from RequestItems_Table where RequestID = '" + RequestID + "' ";

                connection();
                BudgetingOpenConnection();
                SqlCommand cmd = new SqlCommand(query, budgetingcon);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                BudgetingCloseConnection();

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        RequestDate = dt.Rows[i]["SubmitDate"].ToString();
                        Remarks = dt.Rows[i]["L1Remarks"].ToString();
                        Qty = dt.Rows[i]["Qty"].ToString();
                    }
                }
               

                var result = new { SubmitDate = RequestDate, Remarks = Remarks,Qty = Qty };
                return Json(new { success= true, data = result }, JsonRequestBehavior.AllowGet);
                //return Json(new { data = new { RequestDate = RequestDate, L1Remarks = L1Remarks } }, JsonRequestBehavior.AllowGet);
            }
            
            catch(Exception ex)
            {
                
                return Json(new { success = false, data = "" }, JsonRequestBehavior.AllowGet);
            }

            
        }


        public List<RequestItemsRepoView> GetData1(string year)
        {

            using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

            {

                //Proxy Settings Button**********************
                //if (db.Planning_HOE_Table.ToList().FindAll(xi => xi.HOE_NTID == User.Identity.Name.Split('\\')[1].ToUpper()).Count() != 0) //Present user is HOE
                //{
                //    if (db.Planning_HOE_Table.ToList().Find(xi => xi.HOE_NTID == User.Identity.Name.Split('\\')[1].ToUpper()).Enable_Proxy == false
                //        || db.Planning_HOE_Table.ToList().Find(xi => xi.HOE_NTID == User.Identity.Name.Split('\\')[1].ToUpper()).Enable_Proxy == null)
                //        x = presentUserName.TrimEnd().TrimStart().Split(' ').ToList();
                //    else
                //        return viewList;
                //}

                //else if (db.Planning_HOE_Table.ToList().FindAll(xi => xi.Proxy_NTID == User.Identity.Name.Split('\\')[1].ToUpper()).Count() != 0)
                //{
                //    if (db.Planning_HOE_Table.ToList().Find(xi => xi.Proxy_NTID == User.Identity.Name.Split('\\')[1].ToUpper()).Enable_Proxy == false
                //        || db.Planning_HOE_Table.ToList().Find(xi => xi.Proxy_NTID == User.Identity.Name.Split('\\')[1].ToUpper()).Enable_Proxy == null)
                //        return viewList; 
                //    else
                //        x = db.Planning_HOE_Table.ToList().Find(xi => xi.Proxy_NTID == User.Identity.Name.Split('\\')[1].ToUpper()).HOE_FullName.TrimEnd().TrimStart().Split(' ').ToList();
                //}

                //else
                //    x = presentUserName.TrimEnd().TrimStart().Split(' ').ToList();




                //Semaphore approach*********************
                //if (Session["ActiveNTID"] == null || Session["ActiveNTID"].ToString() == string.Empty)
                //{
                //}
                // Session["ActiveNTID"] = User.Identity.Name.Split('\\')[1].ToUpper();  // wont reset until application closes
                //empty check
                //Session["ActiveNTID"] = User.Identity.Name.Split('\\')[1].ToUpper();
                //session var - if hoe enter -> hoe id saved - login 
                //if come out - clear session - form closing event

                List<RequestItems_Table> reqList = db.RequestItems_Table.ToList<RequestItems_Table>().FindAll(xi => xi.ApprovalDH == true).FindAll(xi => xi.ApprovedDH != true && xi.VKM_Year == int.Parse(year));
                List<RequestItemsRepoView> viewList = new List<RequestItemsRepoView>();
                System.Security.Principal.IPrincipal User = System.Web.HttpContext.Current.User;

                if (Convert.ToInt32(year) != DateTime.Now.Year + 1)
                {
                    DataTable dt = new DataTable();
                    connection();
                    BudgetingOpenConnection();
                    string Query = " Exec [dbo].[GetHOEItemsList] '" + year + "', '" + User.Identity.Name.Split('\\')[1].ToUpper() + "' ";
                    SqlCommand cmd = new SqlCommand(Query, budgetingcon);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    BudgetingCloseConnection();

                    foreach (DataRow item in dt.Rows)
                    {
                        try
                        {

                            RequestItemsRepoView ritem = new RequestItemsRepoView();
                            ritem.BudgetCodeDescription = item["BudgetCodeDescription"].ToString();
                            ritem.OrderType = item["Order_Type"].ToString().Trim() != "" ? int.Parse(item["Order_Type"].ToString()) : 0;
                            ritem.UnitofMeasure = item["UnitofMeasure"].ToString().Trim() != "" ? int.Parse(item["UnitofMeasure"].ToString()) : 0;

                            ritem.VKM_Year = Convert.ToInt32(item["VKM_Year"]);
                                    if(item["UpdatedAt"].ToString() != "")
                                        ritem.UpdatedAt = Convert.ToDateTime(item["UpdatedAt"]);
                                    ritem.Category =  Convert.ToInt32(item["Category"]);
                                    ritem.Comments = item["L2_Remarks"].ToString();
                                    ritem.Project = item["Project"].ToString();
                                    if (item["ActualAvailableQuantity"].ToString() == "")
                                        ritem.ActualAvailableQuantity = "NA";
                                    else
                                        ritem.ActualAvailableQuantity = item["ActualAvailableQuantity"].ToString();

                            ritem.Cost_Element = Convert.ToInt32(item["CostElement"]);
                            ritem.BU = Convert.ToInt32(item["BU"]);

                            ritem.DEPT = Convert.ToInt32(item["DEPT"]);
                            ritem.Group = Convert.ToInt32(item["Group"]);
                            ritem.Item_Name = Convert.ToInt32(item["ItemName"]);
                            ritem.OEM = Convert.ToInt32(item["OEM"]);
                            ritem.Required_Quantity = Convert.ToInt32(item["ReqQuantity"]);
                            ritem.Reviewed_Quantity = Convert.ToInt32(item["ApprQuantity"]);
                            ritem.Reviewed_Cost = Convert.ToDecimal(item["ApprCost"]);

                            ritem.RequestID = Convert.ToInt32(item["RequestID"]);

                            ritem.Requestor = item["RequestorNT"].ToString();
                            ritem.Total_Price = Convert.ToDecimal(item["TotalPrice"]);
                            ritem.Unit_Price = Convert.ToDecimal(item["UnitPrice"]);
                            ritem.ApprovalHoE = Convert.ToBoolean(item["ApprovalDH"]);
                            ritem.ApprovalSH = Convert.ToBoolean(item["ApprovalSH"]);
                            ritem.ApprovedHoE = Convert.ToBoolean(item["ApprovedDH"]);
                            ritem.ApprovedSH = Convert.ToBoolean(item["ApprovedSH"]);
                            if (item["IsCancelled"].ToString() != "")
                                ritem.isCancelled = Convert.ToInt32(item["IsCancelled"]);

                            ritem.Reviewer_1 = item["DHNT"].ToString();
                            ritem.Reviewer_2 = item["SHNT"].ToString();

                            ritem.RequestDate = item["RequestDate"].ToString() != "" ? ((DateTime)item["RequestDate"]).ToString("yyyy-MM-dd") : string.Empty;
                            ritem.SubmitDate = item["SubmitDate"].ToString() != "" ? ((DateTime)item["SubmitDate"]).ToString("yyyy-MM-dd") : string.Empty;
                            ritem.Review1_Date = item["DHAppDate"].ToString() != "" ? ((DateTime)item["DHAppDate"]).ToString("yyyy-MM-dd") : string.Empty;
                            ritem.Review2_Date = item["SHAppDate"].ToString() != "" ? ((DateTime)item["SHAppDate"]).ToString("yyyy-MM-dd") : string.Empty;


                            if (item["OrderStatus"].ToString() != "")
                            {
                                ritem.OrderStatus = Convert.ToInt32(item["OrderStatus"]);

                            }
                            else
                            {
                                ritem.OrderStatus = null;


                            }
                            if (item["Project"].ToString() == "")
                                ritem.Project = string.Empty;
                            else
                                ritem.Project = item["Project"].ToString();

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

                            ritem.BudgetCode = item["BudgetCode"].ToString();
                            ritem.RequestSource = item["RequestSource"].ToString();
                            viewList.Add(ritem);
                            //WriteLog("Getdata function: Data added to the list " + DateTime.Now.ToString());


                        }
                        catch (Exception ex)
                        {

                            WriteLog("Error - GetData1 : NTID - " + User.Identity.Name.Split('\\')[1].Trim().ToUpper() + ", ErrorMsg : " + ex.Message.ToString() + ", At - " + DateTime.Now.ToString());
                        }



                    }
                    return viewList.FindAll(xi => xi.ApprovedHoE == true);
                }
                else
                {
                    DataTable dtproxy = new DataTable();
                    string presentUserDept = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).Department;

                   

                    List<string> x = new List<string>();
                    string presentUserName = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).EmployeeName;
                    //Approach-3 Both HOE & Proxy can view at the sametime - if conflict occurs check & notify
                    if (db.Planning_HOE_Table.ToList().FindAll(xi => xi.HOE_NTID == User.Identity.Name.Split('\\')[1].ToUpper()).Count() != 0) //Present user is HOE
                    {
                        //x = presentUserName.TrimEnd().TrimStart().Split(' ').ToList();

                        x.Add(presentUserName.Trim().ToString());

                        //connection();
                        //BudgetingOpenConnection();
                        //string qry = " Select HOE_FullName from Planning_HOE_Table where Proxy_NTID = '" + User.Identity.Name.Split('\\')[1].ToUpper() + "' ";
                        //SqlCommand cmd = new SqlCommand(qry, budgetingcon);
                        //SqlDataAdapter da = new SqlDataAdapter(cmd);
                        //da.Fill(dtproxy);
                        //BudgetingCloseConnection();

                        //if (dtproxy.Rows.Count > 0)
                        //{
                        //    for (int i = 0; i < dtproxy.Rows.Count; i++)
                        //    {
                        //        x.Add(dtproxy.Rows[i]["HOE_FullName"].ToString());
                        //    }
                        //}

                        //Semaphore Approach-2 //if(db.Planning_HOE_Table.ToList().Find(xi=>xi.Active_NTID.Contains(db)))

                    }
                    if (db.Planning_HOE_Table.ToList().FindAll(xi => xi.Proxy_NTID == User.Identity.Name.Split('\\')[1].ToUpper()).Count() != 0)
                    {
                        //x = db.Planning_HOE_Table.ToList().Find(xi => xi.Proxy_NTID == User.Identity.Name.Split('\\')[1].ToUpper()).HOE_FullName.TrimEnd().TrimStart().Split(' ').ToList();

                        connection();
                        BudgetingOpenConnection();
                        string qry = " Select HOE_FullName from Planning_HOE_Table where Proxy_NTID = '" + User.Identity.Name.Split('\\')[1].ToUpper() + "' ";
                        SqlCommand cmd = new SqlCommand(qry, budgetingcon);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dtproxy);
                        BudgetingCloseConnection();

                        if (dtproxy.Rows.Count > 0 )
                        {
                            for (int i = 0; i< dtproxy.Rows.Count; i++)
                            {
                                x.Add(dtproxy.Rows[i]["HOE_FullName"].ToString());
                            }
                        }


                    }
                    if (x.Count() == 0)
                        //x = presentUserName.TrimEnd().TrimStart().Split(' ').ToList();
                        x.Add(presentUserName.Trim().ToString());

                    x.Sort();
                    var isPresentUser = true;
                    string y = "";
                    foreach (RequestItems_Table item in reqList)
                    {
                        try
                        {

                            //var y = item.DHNT.TrimStart().TrimEnd().Split(' ').ToList();
                            //y.Sort();
                            
                            y = item.DHNT.TrimStart().TrimEnd().ToString();
                            //for (int i = 0; i < x.Count(); i++)
                            //{
                            //if (x[i] != y[i])
                            //{
                            //    isPresentUser = false;
                            //    break;
                            //}
                            
                            var matchingvalues = x.Where(stringToCheck => stringToCheck.Contains(y));
                            //if (matchingvalues.Count() == 0)
                            //{
                            //    isPresentUser = false;
                            //    break;
                            //}
                            ////}
                            //if (isPresentUser == false)
                            //{
                            //    isPresentUser = true;
                            //    continue;
                            //}

                            if (matchingvalues.Count() > 0)
                            {
                                RequestItemsRepoView ritem = new RequestItemsRepoView();
                                if (item.UpdatedAt.ToString() != "" && item.UpdatedAt.ToString() != null)
                                    ritem.UpdatedAt = Convert.ToDateTime(item.UpdatedAt);
                                ritem.RequestorNTID = item.RequestorNTID;
                                ritem.VKM_Year = int.Parse(item.VKM_Year.ToString());
                                ritem.BU = int.Parse(item.BU);
                                ritem.Category = int.Parse(item.Category);
                                //ritem.Comments = item.Comments;
                                ritem.Comments = item.L2_Remarks;
                                ritem.Cost_Element = int.Parse(item.CostElement);
                                ritem.DEPT = int.Parse(item.DEPT);
                                ritem.Group = int.Parse(item.Group);
                                ritem.Item_Name = int.Parse(item.ItemName);
                                ritem.OEM = int.Parse(item.OEM);
                                ritem.Required_Quantity = item.ReqQuantity;
                                ritem.RequestID = item.RequestID;
                                ritem.Requestor = item.RequestorNT;
                                ritem.Reviewer_1 = item.DHNT;
                                ritem.Reviewer_2 = item.SHNT;
                                ritem.Total_Price = item.TotalPrice;
                                ritem.Unit_Price = item.UnitPrice;
                                ritem.Reviewed_Quantity = item.ApprQuantity == null ? (int)0 : (int)item.ApprQuantity;
                                ritem.Reviewed_Cost = item.ApprCost == null ? (int)0 : (int)item.ApprCost;
                                ritem.ApprovalHoE = item.ApprovalDH;
                                ritem.ApprovalSH = item.ApprovalSH;
                                ritem.ApprovedHoE = item.ApprovedDH;
                                ritem.ApprovedSH = item.ApprovedSH;
                                if (item.OrderStatus != null && item.OrderStatus.Trim() != "")
                                    ritem.OrderStatus = int.Parse(item.OrderStatus);
                                ritem.RequestDate = item.RequestDate != null ? ((DateTime)item.RequestDate).ToString("yyyy-MM-dd") : string.Empty;
                                ritem.SubmitDate = item.SubmitDate != null ? ((DateTime)item.SubmitDate).ToString("yyyy-MM-dd") : string.Empty;
                                ritem.Review1_Date = item.DHAppDate != null ? ((DateTime)item.DHAppDate).ToString("yyyy-MM-dd") : string.Empty;
                                ritem.Review2_Date = item.SHAppDate != null ? ((DateTime)item.SHAppDate).ToString("yyyy-MM-dd") : string.Empty;
                                ritem.Project = item.Project;
                                ritem.HOEView_ActionHistory = item.HOEView_ActionHistory;
                                if (item.ActualAvailableQuantity == null || item.ActualAvailableQuantity.Trim() == string.Empty)
                                    ritem.ActualAvailableQuantity = "NA";
                                else
                                    ritem.ActualAvailableQuantity = item.ActualAvailableQuantity;
                                ritem.BudgetCode = item.BudgetCode;
                                viewList.Add(ritem);
                            }
                        }

                        catch (Exception ex)
                        {
                           // return viewList.FindAll(xi => xi.ApprovalHoE == true).FindAll(xi => xi.ApprovedHoE != true);

                        }

                        
                    }
                    return viewList.FindAll(xi => xi.ApprovalHoE == true).FindAll(xi => xi.ApprovedHoE != true);



                }
            }
        }



        /// <summary>
        /// function to enable export of data to excel format
        /// used ClosedXMl.dll for the formatting - open source for non commercial apps
        /// accesses other summary tables to build the cost summary tables on the sheet
        /// </summary>
        /// <returns></returns>
        public ActionResult ExportDataToExcel(string useryear)
        {
            try
            {
                System.Security.Principal.IPrincipal User = System.Web.HttpContext.Current.User;


                string presentUserNTID = User.Identity.Name.Split('\\')[1].ToUpper();
                string presentUserDept = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).Department;
                string presentUserName = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).EmployeeName;
               
                string filename = @"VKM " +useryear + " " + presentUserNTID+ " Approval_List_" + DateTime.Now.ToShortDateString() + ".xlsx";

                using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

                {
                    var HOElist = db.Planning_HOE_Table.ToList();
                    List<string> y = new List<string>();
                    DataTable dtproxy = new DataTable();
                    if (HOElist.FindAll(x => x.Proxy_NTID != null).Count != 0)
                    {
                        if (HOElist.FindAll(xi => xi.Proxy_NTID == presentUserNTID).Count() != 0)
                        {
                            var HOE = db.Planning_HOE_Table.ToList().Find(xi => xi.Proxy_NTID == presentUserNTID).HOE_NTID.Trim().ToUpper();
                            //presentUserName = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(HOE)).EmployeeName;
                            //presentUserDept = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(HOE)).Department;

                            y.Add(presentUserName.Trim().ToString());

                            connection();
                            BudgetingOpenConnection();
                            string qry = " Select HOE_FullName from Planning_HOE_Table where Proxy_NTID = '" + User.Identity.Name.Split('\\')[1].ToUpper() + "' ";
                            SqlCommand cmd = new SqlCommand(qry, budgetingcon);
                            SqlDataAdapter da = new SqlDataAdapter(cmd);
                            da.Fill(dtproxy);
                            BudgetingCloseConnection();

                            if (dtproxy.Rows.Count > 0)
                            {
                                for (int i = 0; i < dtproxy.Rows.Count; i++)
                                {
                                    y.Add(dtproxy.Rows[i]["HOE_FullName"].ToString());
                                }
                            }

                        }
                        else
                        {
                            y.Add(presentUserName.Trim().ToString());
                        }

                    }

                    else
                    {
                        y.Add(presentUserName.Trim().ToString());
                    }



                  

                    System.Data.DataTable dt = new System.Data.DataTable("Request_List");
                    dt.Columns.AddRange(new DataColumn[23] { new DataColumn("Business Unit"),
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
                                            new DataColumn("Required Quantity",typeof(int)),
                                            new DataColumn("Total Price",typeof(decimal)),
                                            new DataColumn("Reviewed Quantity",typeof(int)),
                                            new DataColumn("Reviewed Price",typeof(decimal)),
                                            new DataColumn("Comments"),
                                            new DataColumn("Requestor"),
                                            new DataColumn("Submit Date"),
                                            new DataColumn("Reviewer 1"),
                                            new DataColumn("L1 Review Date"),
                                            new DataColumn("Reviewer 2"),
                                            new DataColumn("L2 Review Date"),
                                            new DataColumn("OrderStatus")});
                    if (Convert.ToInt32(useryear) != DateTime.Now.Year + 1)
                    {
                        DataTable dt1 = new DataTable();
                        connection();
                        BudgetingOpenConnection();
                        string Query = " Exec [dbo].[GetHOEItemsList] '" + useryear + "', '" + User.Identity.Name.Split('\\')[1].ToUpper() + "' ";
                        SqlCommand cmd = new SqlCommand(Query, budgetingcon);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt1);
                        BudgetingCloseConnection();

                        foreach (DataRow item in dt1.Rows)
                        {

                            dt.Rows.Add(
                               BudgetingController.lstBUs.Find(bu => bu.ID.ToString().Equals(item["BU"].ToString())).BU,
                               BudgetingController.lstOEMs.Find(oem => oem.ID.ToString().Equals(item["OEM"].ToString())).OEM,
                               BudgetingController.lstDEPTs.Find(dept => dept.ID.ToString().Equals(item["DEPT"].ToString())).DEPT,
                               BudgetingController.lstGroups_test.Find(grp => grp.ID.ToString().Equals(item["Group"].ToString())).Group,
                               item["Project"].ToString(),
                               BudgetingController.lstItems.Find(item1 => item1.S_No.ToString().Equals(item["ItemName"].ToString())).Item_Name,
                               BudgetingController.lstPrdCateg.Find(cat => cat.ID.ToString().Equals(item["Category"].ToString())).Category.Trim(),
                               BudgetingController.lstCostElement.Find(cost => cost.ID.ToString().Equals(item["CostElement"].ToString())).CostElement,
                               item["BudgetCode"].ToString(),
                               Math.Round(Convert.ToDecimal(item["UnitPrice"])),
                               item["ActualAvailableQuantity"].ToString() != "" ? item["ActualAvailableQuantity"].ToString() : "NA",
                               Convert.ToInt32(item["ReqQuantity"]),
                               Math.Round(Convert.ToDecimal(item["TotalPrice"])),
                               item["ApprQuantity"].ToString() != "" ? Convert.ToInt32(item["ApprQuantity"]) : 0,
                               item["ApprCost"].ToString() != "" ? Math.Round(Convert.ToDecimal(item["ApprCost"])) : 0,
                               item["Comments"].ToString(),
                               item["RequestorNT"].ToString(),
                               item["SubmitDate"].ToString() != "" ? ((DateTime)item["SubmitDate"]).ToString("dd-MM-yyyy") : string.Empty,
                               item["DHNT"].ToString(),
                               item["DHAppDate"].ToString() != "" ? ((DateTime)item["DHAppDate"]).ToString("dd-MM-yyyy") : string.Empty,
                               item["SHNT"].ToString(),
                               item["SHAppDate"].ToString() != "" ? ((DateTime)item["SHAppDate"]).ToString("dd-MM-yyyy") : string.Empty,
                               (item["OrderStatus"].ToString().Trim() != "" && item["OrderStatus"].ToString().Trim() != "0") ? BudgetingController.lstOrderStatus.Find(cost => cost.ID.ToString().Equals(item["OrderStatus"].ToString().Trim())).OrderStatus : string.Empty);



                        }
                    }
                    else
                    {
                        foreach (string u in y)
                        {
                            var requests = db.RequestItems_Table.Where(x => x.DHNT != null).Select(x => new
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
                                x.OrderStatus,
                                x.VKM_Year
                            }).Where(x => x.VKM_Year == (DateTime.Now.Year + 1)).ToList().//.Where(x => x.SubmitDate.ToString().Contains(DateTime.Now.Year.ToString())).ToList().
                            FindAll(x => x.DHNT.Trim().Equals(u.Trim()) /*|| x.DHNT.Trim().Equals(presentUserName_2020.Trim())*/);

                            foreach (var request in requests)
                            {
                                dt.Rows.Add(
                                    BudgetingController.lstBUs.Find(bu => bu.ID.ToString().Equals(request.BU)).BU,
                                    BudgetingController.lstOEMs.Find(oem => oem.ID.ToString().Equals(request.OEM)).OEM,
                                    BudgetingController.lstDEPTs.Find(dept => dept.ID.ToString().Equals(request.DEPT)).DEPT,
                                    BudgetingController.lstGroups_test.Find(grp => grp.ID.ToString().Equals(request.Group)).Group,
                                    request.Project,
                                    BudgetingController.lstItems.Find(item => item.S_No.ToString().Equals(request.ItemName)).Item_Name,
                                    BudgetingController.lstPrdCateg.Find(cat => cat.ID.ToString().Equals(request.Category)).Category.Trim(),
                                    BudgetingController.lstCostElement.Find(cost => cost.ID.ToString().Equals(request.CostElement)).CostElement,
                                    request.BudgetCode,
                                    Math.Round((decimal)request.UnitPrice),
                                    request.ActualAvailableQuantity != null ? request.ActualAvailableQuantity : "NA",
                                    request.ReqQuantity,
                                    Math.Round((decimal)request.TotalPrice),
                                    request.ApprQuantity,
                                    request.ApprCost,
                                    request.Comments,
                                    request.RequestorNT,
                                    request.SubmitDate.HasValue ? request.SubmitDate.Value.ToString("dd-MM-yyyy") : "",
                                    request.DHNT,
                                    request.DHAppDate.HasValue ? request.DHAppDate.Value.ToString("dd-MM-yyyy") : "",
                                    request.SHNT,
                                    request.SHAppDate.HasValue ? request.SHAppDate.Value.ToString("dd-MM-yyyy") : "",
                                   (request.OrderStatus != null && request.OrderStatus.Trim() != "") ? BudgetingController.lstOrderStatus.Find(status => status.ID.ToString().Equals(request.OrderStatus.Trim())).OrderStatus : "");

                            }
                        }
                    }
                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        var ws = wb.Worksheets.Add("VKM " + useryear + " Approval List");
                       
                        if (Convert.ToInt32(useryear) == DateTime.Now.Year + 1)
                        {
                            ws.Cell(1, 1).Value = "BU Summary";

                            //if (presentUserDept.Contains("XC"))
                            //    ws.Cell(2, 1).InsertTable(XC_BUSummary(useryear));
                            //else
                            //    ws.Cell(2, 1).InsertTable(CC_BUSummary(useryear));
                          
                                ws.Cell(2, 1).InsertTable(BUSummary(useryear));
                            ws.Cell(8, 1).Value = "Dept Summary";
                            ws.Cell(9, 1).InsertTable(DeptSummaryData(useryear));
                            ws.Cell(15, 1).InsertTable(dt);
                        }
                        else
                        {
                            ws.Cell(2, 1).InsertTable(dt);
                        }
                       
                        using (MemoryStream stream = new MemoryStream())
                        {
                            wb.SaveAs(stream);
                            var robj = File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
                            //WriteLog("Exporting Data into excel " + DateTime.Now.ToString());
                            return Json(new { success = true, data = robj }, JsonRequestBehavior.AllowGet);
                        }
                    }

                }
            }
            catch (Exception e)
            { 
                WriteLog("Error - Exporting Data : NTID - " + User.Identity.Name.Split('\\')[1].Trim().ToUpper() + ", ErrorMsg : " + e.Message.ToString() + ", At - " + DateTime.Now.ToString());

                return Json(new { success = false, message = e.Message.ToString() }, JsonRequestBehavior.AllowGet);
            }
        }


        /// <summary>
        /// function to get the summary based on BU
        /// </summary>
        /// <returns>A list of summary amounts for BU</returns>
        //public List<SummaryView> BUSummaryData()
        //{
        //    using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())


        //    {
        //        System.Security.Principal.IPrincipal User = System.Web.HttpContext.Current.User;
        //        string presentUserName = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).EmployeeName;
        //        string presentUserDept = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).Department;


        //        List<RequestItems_Table> reqList = db.RequestItems_Table.Where(x => x.SubmitDate.ToString().Contains(DateTime.Now.Year.ToString())).ToList<RequestItems_Table>();
        //        List<SummaryView> viewList = new List<SummaryView>();

        //        decimal AS_MAE_Totals = 0, AS_NMAE_Totals = 0, AS_SoftwareTotals = 0;
        //        decimal OSS_MAE_Totals = 0, OSS_NMAE_Totals = 0, OSS_SoftwareTotals = 0;
        //        decimal DA_MAE_Totals = 0, DA_NMAE_Totals = 0, DA_SoftwareTotals = 0;
        //        decimal AD_MAE_Totals = 0, AD_NMAE_Totals = 0, AD_SoftwareTotals = 0;
        //        decimal TwoWP_MAE_Totals = 0, TwoWP_NMAE_Totals = 0, TwoWP_SoftwareTotals = 0;

        //        if (presentUserDept.Contains("CC"))
        //        {
        //            foreach (RequestItems_Table item in reqList.FindAll(x => x.BU.Equals(BudgetingController.lstBUs.Find(bu => bu.BU.Equals("AS")).ID.ToString())).FindAll(x => x.DHNT.Trim().Contains(/*L2ReviewerName*/presentUserName)).FindAll(x => x.ApprovalDH == true).FindAll(x => x.ApprovedDH == false))
        //            {
        //                switch (BudgetingController.lstCostElement.Find(cost => cost.ID.Equals(Int32.Parse(item.CostElement))).CostElement)
        //                {
        //                    case "MAE":
        //                        {

        //                            AS_MAE_Totals += (decimal)item.TotalPrice;
        //                            break;
        //                        }
        //                    case "Non-MAE":
        //                        {

        //                            AS_NMAE_Totals += (decimal)item.TotalPrice;
        //                            break;
        //                        }
        //                    case "Software":
        //                        {

        //                            AS_SoftwareTotals += (decimal)item.TotalPrice;
        //                            break;
        //                        }
        //                    default:
        //                        continue;
        //                }
        //            }
        //            foreach (RequestItems_Table item in reqList.FindAll(x => x.BU.Equals(BudgetingController.lstBUs.Find(bu => bu.BU.Equals("OSS")).ID.ToString())).FindAll(x => x.DHNT.Trim().Contains(/*L2ReviewerName*/presentUserName)).FindAll(x => x.ApprovalDH == true).FindAll(x => x.ApprovedDH == false))
        //            {
        //                switch (BudgetingController.lstCostElement.Find(cost => cost.ID.Equals(Int32.Parse(item.CostElement))).CostElement)
        //                {
        //                    case "MAE":
        //                        {

        //                            OSS_MAE_Totals += (decimal)item.TotalPrice;
        //                            break;
        //                        }
        //                    case "Non-MAE":
        //                        {

        //                            OSS_NMAE_Totals += (decimal)item.TotalPrice;
        //                            break;
        //                        }
        //                    case "Software":
        //                        {

        //                            OSS_SoftwareTotals += (decimal)item.TotalPrice;
        //                            break;
        //                        }
        //                    default:
        //                        continue;
        //                }
        //            }
        //            foreach (RequestItems_Table item in reqList.FindAll(x => x.BU.Equals(BudgetingController.lstBUs.Find(bu => bu.BU.Equals("2WP")).ID.ToString())).FindAll(x => x.DHNT.Trim().Contains(/*L2ReviewerName*/presentUserName)).FindAll(x => x.ApprovalDH == true).FindAll(x => x.ApprovedDH == false).FindAll(dpt => BudgetingController.lstDEPTs.Find(x => x.ID.ToString().Contains(dpt.DEPT)).DEPT.Contains("CC")))
        //            {
        //                switch (BudgetingController.lstCostElement.Find(cost => cost.ID.Equals(Int32.Parse(item.CostElement))).CostElement)
        //                {
        //                    case "MAE":
        //                        {

        //                            TwoWP_MAE_Totals += (decimal)item.TotalPrice;
        //                            break;
        //                        }
        //                    case "Non-MAE":
        //                        {

        //                            TwoWP_NMAE_Totals += (decimal)item.TotalPrice;
        //                            break;
        //                        }
        //                    case "Software":
        //                        {

        //                            TwoWP_SoftwareTotals += (decimal)item.TotalPrice;
        //                            break;
        //                        }
        //                    default:
        //                        continue;
        //                }
        //            }

        //        }
        //        else if (presentUserDept.Contains("XC"))
        //        {
        //            foreach (RequestItems_Table item in reqList.FindAll(x => x.BU.Equals(BudgetingController.lstBUs.Find(bu => bu.BU.Equals("DA")).ID.ToString())).FindAll(x => x.DHNT.Trim().Contains(/*L2ReviewerName*/presentUserName)).FindAll(x => x.ApprovalDH == true).FindAll(x => x.ApprovedDH == false))
        //            {
        //                switch (BudgetingController.lstCostElement.Find(cost => cost.ID.Equals(Int32.Parse(item.CostElement))).CostElement)
        //                {
        //                    case "MAE":
        //                        {

        //                            DA_MAE_Totals += (decimal)item.TotalPrice;
        //                            break;
        //                        }
        //                    case "Non-MAE":
        //                        {

        //                            DA_NMAE_Totals += (decimal)item.TotalPrice;
        //                            break;
        //                        }
        //                    case "Software":
        //                        {

        //                            DA_SoftwareTotals += (decimal)item.TotalPrice;
        //                            break;
        //                        }
        //                    default:
        //                        continue;
        //                }
        //            }
        //            foreach (RequestItems_Table item in reqList.FindAll(x => x.BU.Equals(BudgetingController.lstBUs.Find(bu => bu.BU.Equals("AD")).ID.ToString())).FindAll(x => x.DHNT.Trim().Contains(/*L2ReviewerName*/presentUserName)).FindAll(x => x.ApprovalDH == true).FindAll(x => x.ApprovedDH == false))
        //            {
        //                switch (BudgetingController.lstCostElement.Find(cost => cost.ID.Equals(Int32.Parse(item.CostElement))).CostElement)
        //                {
        //                    case "MAE":
        //                        {

        //                            AD_MAE_Totals += (decimal)item.TotalPrice;
        //                            break;
        //                        }
        //                    case "Non-MAE":
        //                        {

        //                            AD_NMAE_Totals += (decimal)item.TotalPrice;
        //                            break;
        //                        }
        //                    case "Software":
        //                        {

        //                            AD_SoftwareTotals += (decimal)item.TotalPrice;
        //                            break;
        //                        }
        //                    default:
        //                        continue;
        //                }
        //            }
        //            foreach (RequestItems_Table item in reqList.FindAll(x => x.BU.Equals(BudgetingController.lstBUs.Find(bu => bu.BU.Equals("2WP")).ID.ToString())).FindAll(x => x.DHNT.Trim().Contains(/*L2ReviewerName*/presentUserName)).FindAll(x => x.ApprovalDH == true).FindAll(x => x.ApprovedDH == false).FindAll(dpt => BudgetingController.lstDEPTs.Find(x => x.ID.ToString().Contains(dpt.DEPT)).DEPT.Contains("XC")))
        //            {
        //                switch (BudgetingController.lstCostElement.Find(cost => cost.ID.Equals(Int32.Parse(item.CostElement))).CostElement)
        //                {
        //                    case "MAE":
        //                        {

        //                            TwoWP_MAE_Totals += (decimal)item.TotalPrice;
        //                            break;
        //                        }
        //                    case "Non-MAE":
        //                        {

        //                            TwoWP_NMAE_Totals += (decimal)item.TotalPrice;
        //                            break;
        //                        }
        //                    case "Software":
        //                        {

        //                            TwoWP_SoftwareTotals += (decimal)item.TotalPrice;
        //                            break;
        //                        }
        //                    default:
        //                        continue;
        //                }
        //            }

        //        }
        //        else
        //        {

        //            foreach (RequestItems_Table item in reqList.FindAll(x => x.BU.Equals(BudgetingController.lstBUs.Find(bu => bu.BU.Equals("AS")).ID.ToString())).FindAll(x => x.DHNT.Trim().Contains(/*L2ReviewerName*/presentUserName)).FindAll(x => x.ApprovalDH == true).FindAll(x => x.ApprovedDH == false))
        //            {
        //                switch (BudgetingController.lstCostElement.Find(cost => cost.ID.Equals(Int32.Parse(item.CostElement))).CostElement)
        //                {
        //                    case "MAE":
        //                        {

        //                            AS_MAE_Totals += (decimal)item.TotalPrice;
        //                            break;
        //                        }
        //                    case "Non-MAE":
        //                        {

        //                            AS_NMAE_Totals += (decimal)item.TotalPrice;
        //                            break;
        //                        }
        //                    case "Software":
        //                        {

        //                            AS_SoftwareTotals += (decimal)item.TotalPrice;
        //                            break;
        //                        }
        //                    default:
        //                        continue;
        //                }
        //            }
        //            foreach (RequestItems_Table item in reqList.FindAll(x => x.BU.Equals(BudgetingController.lstBUs.Find(bu => bu.BU.Equals("OSS")).ID.ToString())).FindAll(x => x.DHNT.Trim().Contains(/*L2ReviewerName*/presentUserName)).FindAll(x => x.ApprovalDH == true).FindAll(x => x.ApprovedDH == false))
        //            {
        //                switch (BudgetingController.lstCostElement.Find(cost => cost.ID.Equals(Int32.Parse(item.CostElement))).CostElement)
        //                {
        //                    case "MAE":
        //                        {

        //                            OSS_MAE_Totals += (decimal)item.TotalPrice;
        //                            break;
        //                        }
        //                    case "Non-MAE":
        //                        {

        //                            OSS_NMAE_Totals += (decimal)item.TotalPrice;
        //                            break;
        //                        }
        //                    case "Software":
        //                        {

        //                            OSS_SoftwareTotals += (decimal)item.TotalPrice;
        //                            break;
        //                        }
        //                    default:
        //                        continue;
        //                }
        //            }
        //            foreach (RequestItems_Table item in reqList.FindAll(x => x.BU.Equals(BudgetingController.lstBUs.Find(bu => bu.BU.Equals("DA")).ID.ToString())).FindAll(x => x.DHNT.Trim().Contains(/*L2ReviewerName*/presentUserName)).FindAll(x => x.ApprovalDH == true).FindAll(x => x.ApprovedDH == false))
        //            {
        //                switch (BudgetingController.lstCostElement.Find(cost => cost.ID.Equals(Int32.Parse(item.CostElement))).CostElement)
        //                {
        //                    case "MAE":
        //                        {

        //                            DA_MAE_Totals += (decimal)item.TotalPrice;
        //                            break;
        //                        }
        //                    case "Non-MAE":
        //                        {

        //                            DA_NMAE_Totals += (decimal)item.TotalPrice;
        //                            break;
        //                        }
        //                    case "Software":
        //                        {

        //                            DA_SoftwareTotals += (decimal)item.TotalPrice;
        //                            break;
        //                        }
        //                    default:
        //                        continue;
        //                }
        //            }
        //            foreach (RequestItems_Table item in reqList.FindAll(x => x.BU.Equals(BudgetingController.lstBUs.Find(bu => bu.BU.Equals("AD")).ID.ToString())).FindAll(x => x.DHNT.Trim().Contains(/*L2ReviewerName*/presentUserName)).FindAll(x => x.ApprovalDH == true).FindAll(x => x.ApprovedDH == false))
        //            {
        //                switch (BudgetingController.lstCostElement.Find(cost => cost.ID.Equals(Int32.Parse(item.CostElement))).CostElement)
        //                {
        //                    case "MAE":
        //                        {

        //                            AD_MAE_Totals += (decimal)item.TotalPrice;
        //                            break;
        //                        }
        //                    case "Non-MAE":
        //                        {

        //                            AD_NMAE_Totals += (decimal)item.TotalPrice;
        //                            break;
        //                        }
        //                    case "Software":
        //                        {

        //                            AD_SoftwareTotals += (decimal)item.TotalPrice;
        //                            break;
        //                        }
        //                    default:
        //                        continue;
        //                }
        //            }
        //            foreach (RequestItems_Table item in reqList.FindAll(x => x.BU.Equals(BudgetingController.lstBUs.Find(bu => bu.BU.Equals("2WP")).ID.ToString())).FindAll(x => x.DHNT.Trim().Contains(/*L2ReviewerName*/presentUserName)).FindAll(x => x.ApprovalDH == true).FindAll(x => x.ApprovedDH == false))
        //            {
        //                switch (BudgetingController.lstCostElement.Find(cost => cost.ID.Equals(Int32.Parse(item.CostElement))).CostElement)
        //                {
        //                    case "MAE":
        //                        {

        //                            TwoWP_MAE_Totals += (decimal)item.TotalPrice;
        //                            break;
        //                        }
        //                    case "Non-MAE":
        //                        {

        //                            TwoWP_NMAE_Totals += (decimal)item.TotalPrice;
        //                            break;
        //                        }
        //                    case "Software":
        //                        {

        //                            TwoWP_SoftwareTotals += (decimal)item.TotalPrice;
        //                            break;
        //                        }
        //                    default:
        //                        continue;
        //                }
        //            }

        //        }

        //        SummaryView Category_View = new SummaryView();
        //        Category_View.Category = "MAE";
        //        Category_View.AS = AS_MAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //        Category_View.OSS = OSS_MAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //        //Category_View.DA = DA_MAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //        //Category_View.AD = AD_MAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //        Category_View.Two_Wheeler = TwoWP_MAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //        Category_View.Totals = (AS_MAE_Totals + OSS_MAE_Totals + DA_MAE_Totals + AD_MAE_Totals + TwoWP_MAE_Totals).ToString("C0", CultureInfo.CurrentCulture);
        //        viewList.Add(Category_View);
        //        Category_View = new SummaryView();
        //        Category_View.Category = "Non-MAE";
        //        Category_View.AS = AS_NMAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //        Category_View.OSS = OSS_NMAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //        //Category_View.DA = DA_NMAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //        //Category_View.AD = AD_NMAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //        Category_View.Two_Wheeler = TwoWP_NMAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //        Category_View.Totals = (AS_NMAE_Totals + OSS_NMAE_Totals + DA_NMAE_Totals + AD_NMAE_Totals + TwoWP_NMAE_Totals).ToString("C0", CultureInfo.CurrentCulture);
        //        viewList.Add(Category_View);
        //        Category_View = new SummaryView();
        //        Category_View.Category = "Software";
        //        Category_View.AS = AS_SoftwareTotals.ToString("C0", CultureInfo.CurrentCulture);
        //        Category_View.OSS = OSS_SoftwareTotals.ToString("C0", CultureInfo.CurrentCulture);
        //        //Category_View.DA = DA_SoftwareTotals.ToString("C0", CultureInfo.CurrentCulture);
        //        //Category_View.AD = AD_SoftwareTotals.ToString("C0", CultureInfo.CurrentCulture);
        //        Category_View.Two_Wheeler = TwoWP_SoftwareTotals.ToString("C0", CultureInfo.CurrentCulture);
        //        Category_View.Totals = (AS_SoftwareTotals + OSS_SoftwareTotals + DA_SoftwareTotals + AD_SoftwareTotals + TwoWP_SoftwareTotals).ToString("C0", CultureInfo.CurrentCulture);
        //        viewList.Add(Category_View);
        //        Category_View = new SummaryView();
        //        Category_View.Category = "Totals";
        //        Category_View.AS = (AS_MAE_Totals + AS_NMAE_Totals + AS_SoftwareTotals).ToString("C0", CultureInfo.CurrentCulture);
        //        Category_View.OSS = (OSS_MAE_Totals + OSS_NMAE_Totals + OSS_SoftwareTotals).ToString("C0", CultureInfo.CurrentCulture);
        //        //Category_View.DA = (DA_MAE_Totals + DA_NMAE_Totals + DA_SoftwareTotals).ToString("C0", CultureInfo.CurrentCulture);
        //        //Category_View.AD = (AD_MAE_Totals + AD_NMAE_Totals + AD_SoftwareTotals).ToString("C0", CultureInfo.CurrentCulture);
        //        Category_View.Two_Wheeler = (TwoWP_MAE_Totals + TwoWP_NMAE_Totals + TwoWP_SoftwareTotals).ToString("C0", CultureInfo.CurrentCulture);
        //        Category_View.Totals = ((AS_MAE_Totals + AS_NMAE_Totals + AS_SoftwareTotals) + (OSS_MAE_Totals + OSS_NMAE_Totals + OSS_SoftwareTotals) + (DA_MAE_Totals + DA_NMAE_Totals + DA_SoftwareTotals) +
        //            (AD_MAE_Totals + AD_NMAE_Totals + AD_SoftwareTotals) + (TwoWP_MAE_Totals + TwoWP_NMAE_Totals + TwoWP_SoftwareTotals)).ToString("C0", CultureInfo.CurrentCulture);
        //        viewList.Add(Category_View);

        //        return viewList;
        //    }


        //}
        /// <summary>
        /// function to get the Summary of Bu costs to View
        /// </summary>
        /// <returns></returns>
        //public ActionResult GetBUSummaryData()
        //{
        //    string message = string.Empty;
        //    string presentUserDept = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).Department;
        //    if (presentUserDept.Contains("CC"))
        //        message = "CC";
        //    else if (presentUserDept.Contains("XC"))
        //        message = "XC";
        //    else
        //        message = "";

        //    return Json(new { data = BUSummaryData(), message }, JsonRequestBehavior.AllowGet);
        //}






        //cc xc in view & export

        /// <summary>
        /// Function to send BU summary data to view
        /// </summary>
        /// <returns></returns>
        public ActionResult GetBUSummaryData(string year)
        {
            string message = string.Empty;

            string presentUserDept = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).Department;
            //presentUserDept = "XC";

            System.Data.DataTable dt = new System.Data.DataTable();

            //if (presentUserDept.Contains("XC"))
            //    dt = XC_BUSummary(year);
            //else
                dt = BUSummary(year);
            //dt = BUSummary(year);


            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            BudgetParam t = new BudgetParam();
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

            return Json(new { data = t }, JsonRequestBehavior.AllowGet);

            // return Json(new { data = BUSummaryData(year), message }, JsonRequestBehavior.AllowGet);
        }

        //public System.Data.DataTable BUSummary(string year)
        //{
        //    string presentUserName = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).EmployeeName;


        //    using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //    {
        //        //var HOElist = db.Planning_HOE_Table.ToList();
        //        //if (HOElist.FindAll(x => x.Proxy_NTID != null).Count != 0)
        //        //{
        //        //    if (HOElist.FindAll(xi => xi.Proxy_NTID == User.Identity.Name.Split('\\')[1].ToUpper()).Count() != 0)
        //        //    {
        //        //        var HOE = db.Planning_HOE_Table.ToList().Find(xi => xi.Proxy_NTID == User.Identity.Name.Split('\\')[1].ToUpper()).HOE_NTID.Trim().ToUpper();
        //        //        presentUserName = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(HOE)).EmployeeName;
        //        //    }


        //        //}
        //        List<string> h = new List<string>();
        //        DataTable dtproxy = new DataTable();
        //        if (db.Planning_HOE_Table.ToList().FindAll(xi => xi.HOE_NTID == User.Identity.Name.Split('\\')[1].ToUpper()).Count() != 0) //Present user is HOE
        //        {
        //            //x = presentUserName.TrimEnd().TrimStart().Split(' ').ToList();

        //            h.Add(presentUserName.Trim().ToString());

        //            //connection();
        //            //BudgetingOpenConnection();
        //            //string qry = " Select HOE_FullName from Planning_HOE_Table where Proxy_NTID = '" + User.Identity.Name.Split('\\')[1].ToUpper() + "' ";
        //            //SqlCommand cmd = new SqlCommand(qry, budgetingcon);
        //            //SqlDataAdapter da = new SqlDataAdapter(cmd);
        //            //da.Fill(dtproxy);
        //            //BudgetingCloseConnection();

        //            //if (dtproxy.Rows.Count > 0)
        //            //{
        //            //    for (int i = 0; i < dtproxy.Rows.Count; i++)
        //            //    {
        //            //        h.Add(dtproxy.Rows[i]["HOE_FullName"].ToString());
        //            //    }
        //            //}

        //            //Semaphore Approach-2 //if(db.Planning_HOE_Table.ToList().Find(xi=>xi.Active_NTID.Contains(db)))

        //        }
        //        if (db.Planning_HOE_Table.ToList().FindAll(xi => xi.Proxy_NTID == User.Identity.Name.Split('\\')[1].ToUpper()).Count() != 0)
        //        {
        //            //x = db.Planning_HOE_Table.ToList().Find(xi => xi.Proxy_NTID == User.Identity.Name.Split('\\')[1].ToUpper()).HOE_FullName.TrimEnd().TrimStart().Split(' ').ToList();

        //            connection();
        //            BudgetingOpenConnection();
        //            string qry = " Select HOE_FullName from Planning_HOE_Table where Proxy_NTID = '" + User.Identity.Name.Split('\\')[1].ToUpper() + "' ";
        //            SqlCommand cmd = new SqlCommand(qry, budgetingcon);
        //            SqlDataAdapter da = new SqlDataAdapter(cmd);
        //            da.Fill(dtproxy);
        //            BudgetingCloseConnection();

        //            if (dtproxy.Rows.Count > 0)
        //            {
        //                for (int i = 0; i < dtproxy.Rows.Count; i++)
        //                {
        //                    h.Add(dtproxy.Rows[i]["HOE_FullName"].ToString());
        //                }
        //            }


        //        }
        //        if (h.Count() == 0)
        //            //x = presentUserName.TrimEnd().TrimStart().Split(' ').ToList();
        //            h.Add(presentUserName.Trim().ToString());



        //        List<BUSummary_CC> viewList = new List<BUSummary_CC>();


        //        decimal AS_MAE_Totals = 0, AS_NMAE_Totals = 0, AS_SoftwareTotals = 0;
        //        decimal OSS_MAE_Totals = 0, OSS_NMAE_Totals = 0, OSS_SoftwareTotals = 0;
        //        decimal TwoWP_MAE_Totals = 0, TwoWP_NMAE_Totals = 0, TwoWP_SoftwareTotals = 0;

        //        List<RequestItems_Table> reqList = db.RequestItems_Table.Where(x => x.VKM_Year.ToString().Contains(year) && (x.Fund.Trim().Contains("2") || x.Fund == null)).ToList<RequestItems_Table>();

        //        reqList = reqList.FindAll(dpt => !BudgetingController.lstDEPTs.Find(x => x.ID.ToString().Contains(dpt.DEPT)).DEPT.Contains("XC")); ;

        //        string y = "";
        //        bool isPresentUser = true;
        //        var matchingvalues1 = h.Where(stringToCheck => stringToCheck.Contains(presentUserName.Trim().ToString()));
        //        if (matchingvalues1.Count() == 0)
        //        {
        //            h.Add(presentUserName.Trim().ToString());
        //        }
        //        //foreach (RequestItems_Table item in reqList.FindAll(x => x.BU.Equals(BudgetingController.lstBUs.Find(bu => bu.BU.Trim().Equals("MB")).ID.ToString())).FindAll(x => x.DHNT.Trim().Contains(/*L2ReviewerName*/presentUserName)).FindAll(x => x.ApprovalDH == true).FindAll(x => x.ApprovedDH == false))
        //        foreach (RequestItems_Table item in reqList.FindAll(x => x.BU.Equals(BudgetingController.lstBUs.Find(bu => bu.BU.Trim().Equals("MB")).ID.ToString())).FindAll(x => x.ApprovalDH == true).FindAll(x => x.ApprovedDH == false))
        //        {
        //            y = item.DHNT.TrimStart().TrimEnd().ToString();



        //            var matchingvalues = h.Where(stringToCheck => stringToCheck.Contains(y));
        //            if (matchingvalues.Count() == 0)
        //            {
        //                isPresentUser = false;
        //                break;
        //            }
        //            //}
        //            if (isPresentUser == false)
        //            {
        //                isPresentUser = true;
        //                continue;
        //            }
        //            switch (BudgetingController.lstCostElement.Find(cost => cost.ID.Equals(Int32.Parse(item.CostElement))).CostElement)
        //            {
        //                case "MAE":
        //                    {

        //                        AS_MAE_Totals += (decimal)item.TotalPrice;

        //                        break;
        //                    }
        //                case "Non-MAE":
        //                    {

        //                        AS_NMAE_Totals += (decimal)item.TotalPrice;

        //                        break;
        //                    }
        //                case "Software":
        //                    {

        //                        AS_SoftwareTotals += (decimal)item.TotalPrice;

        //                        break;
        //                    }
        //                default:
        //                    continue;
        //            }
        //        }
        //        //foreach (RequestItems_Table item in reqList.FindAll(x => x.BU.Trim().Equals(BudgetingController.lstBUs.Find(bu => bu.BU.Equals("OSS")).ID.ToString())).FindAll(x => x.DHNT.Trim().Contains(/*L2ReviewerName*/presentUserName)).FindAll(x => x.ApprovalDH == true).FindAll(x => x.ApprovedDH == false))
        //        foreach (RequestItems_Table item in reqList.FindAll(x => x.BU.Trim().Equals(BudgetingController.lstBUs.Find(bu => bu.BU.Equals("OSS")).ID.ToString())).FindAll(x => x.ApprovalDH == true).FindAll(x => x.ApprovedDH == false))
        //        {
        //            y = item.DHNT.TrimStart().TrimEnd().ToString();


        //            var matchingvalues = h.Where(stringToCheck => stringToCheck.Contains(y));
        //            if (matchingvalues.Count() == 0)
        //            {
        //                isPresentUser = false;
        //                break;
        //            }
        //            //}
        //            if (isPresentUser == false)
        //            {
        //                isPresentUser = true;
        //                continue;
        //            }
        //            switch (BudgetingController.lstCostElement.Find(cost => cost.ID.Equals(Int32.Parse(item.CostElement))).CostElement)
        //            {
        //                case "MAE":
        //                    {

        //                        OSS_MAE_Totals += (decimal)item.TotalPrice;

        //                        break;
        //                    }
        //                case "Non-MAE":
        //                    {

        //                        OSS_NMAE_Totals += (decimal)item.TotalPrice;

        //                        break;
        //                    }
        //                case "Software":
        //                    {

        //                        OSS_SoftwareTotals += (decimal)item.TotalPrice;

        //                        break;
        //                    }
        //                default:
        //                    continue;
        //            }
        //        }
        //        //foreach (RequestItems_Table item in reqList.FindAll(x => x.BU.Trim().Equals(BudgetingController.lstBUs.Find(bu => bu.BU.Equals("2WP")).ID.ToString())).FindAll(x => x.DHNT.Trim().Contains(/*L2ReviewerName*/presentUserName)).FindAll(x => x.ApprovalDH == true).FindAll(x => x.ApprovedDH == false))
        //        foreach (RequestItems_Table item in reqList.FindAll(x => x.BU.Trim().Equals(BudgetingController.lstBUs.Find(bu => bu.BU.Equals("2WP")).ID.ToString())).FindAll(x => x.ApprovalDH == true).FindAll(x => x.ApprovedDH == false))
        //        {
        //            y = item.DHNT.TrimStart().TrimEnd().ToString();


        //            var matchingvalues = h.Where(stringToCheck => stringToCheck.Contains(y));
        //            if (matchingvalues.Count() == 0)
        //            {
        //                isPresentUser = false;
        //                break;
        //            }
        //            //}
        //            if (isPresentUser == false)
        //            {
        //                isPresentUser = true;
        //                continue;
        //            }
        //            switch (BudgetingController.lstCostElement.Find(cost => cost.ID.Equals(Int32.Parse(item.CostElement))).CostElement)
        //            {
        //                case "MAE":
        //                    {

        //                        TwoWP_MAE_Totals += (decimal)item.TotalPrice;

        //                        break;
        //                    }
        //                case "Non-MAE":
        //                    {

        //                        TwoWP_NMAE_Totals += (decimal)item.TotalPrice;

        //                        break;
        //                    }
        //                case "Software":
        //                    {

        //                        TwoWP_SoftwareTotals += (decimal)item.TotalPrice;

        //                        break;
        //                    }
        //                default:
        //                    continue;
        //            }
        //        }

        //        BUSummary_CC tempobj = new BUSummary_CC();
        //        tempobj.vkmyear = DateTime.Now.Year.ToString();
        //        tempobj.AS_MAE_Totals = AS_MAE_Totals;
        //        tempobj.AS_NMAE_Totals = AS_NMAE_Totals;
        //        tempobj.AS_Software_Totals = AS_SoftwareTotals;
        //        tempobj.AS_Overall_Totals = AS_MAE_Totals + AS_NMAE_Totals + AS_SoftwareTotals;
        //        tempobj.OSS_MAE_Totals = OSS_MAE_Totals;
        //        tempobj.OSS_NMAE_Totals = OSS_NMAE_Totals;
        //        tempobj.OSS_Software_Totals = OSS_SoftwareTotals;
        //        tempobj.OSS_Overall_Totals = OSS_MAE_Totals + OSS_NMAE_Totals + OSS_SoftwareTotals;
        //        tempobj.TwoWP_MAE_Totals = TwoWP_MAE_Totals;
        //        tempobj.TwoWP_NMAE_Totals = TwoWP_NMAE_Totals;
        //        tempobj.TwoWP_Software_Totals = TwoWP_SoftwareTotals;
        //        tempobj.TwoWP_Overall_Totals = TwoWP_MAE_Totals + TwoWP_NMAE_Totals + TwoWP_SoftwareTotals;

        //        viewList.Add(tempobj);


        //        System.Data.DataTable dt = new System.Data.DataTable();
        //        dt.Columns.Add("Cost Element", typeof(string)); /// Planned + (year variable) / Utilized  + (year variable)

        //        dt.Columns.Add("MB", typeof(string)); //add vkm text to yr
        //        dt.Columns.Add("OSS", typeof(string));
        //        dt.Columns.Add("2WP", typeof(string));
        //        dt.Columns.Add("Totals", typeof(string));




        //        DataRow dr = dt.NewRow();
        //        dr[0] = "MAE";
        //        int rcnt = 1;


        //        foreach (var info in viewList)
        //        {
        //            dr[rcnt++] = info.AS_MAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //            dr[rcnt++] = info.OSS_MAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //            dr[rcnt++] = info.TwoWP_MAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //            dr[rcnt++] = (info.AS_MAE_Totals + info.OSS_MAE_Totals + info.TwoWP_MAE_Totals).ToString("C0", CultureInfo.CurrentCulture);
        //        }
        //        dt.Rows.Add(dr);

        //        dr = dt.NewRow();
        //        dr[0] = "Non-MAE";
        //        int r1cnt = 1;
        //        foreach (var info in viewList)
        //        {
        //            dr[r1cnt++] = info.AS_NMAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //            dr[r1cnt++] = info.OSS_NMAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //            dr[r1cnt++] = info.TwoWP_NMAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //            dr[r1cnt++] = (info.AS_NMAE_Totals + info.OSS_NMAE_Totals + info.TwoWP_NMAE_Totals).ToString("C0", CultureInfo.CurrentCulture);
        //        }

        //        dt.Rows.Add(dr);

        //        dr = dt.NewRow();
        //        dr[0] = "Software";
        //        int r2cnt = 1;
        //        foreach (var info in viewList)
        //        {
        //            dr[r2cnt++] = info.AS_Software_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //            dr[r2cnt++] = info.OSS_Software_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //            dr[r2cnt++] = info.TwoWP_Software_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //            dr[r2cnt++] = (info.AS_Software_Totals + info.OSS_Software_Totals + info.TwoWP_Software_Totals).ToString("C0", CultureInfo.CurrentCulture);
        //        }

        //        dt.Rows.Add(dr);

        //        dr = dt.NewRow();
        //        dr[0] = "Totals";
        //        int r3cnt = 1;

        //        foreach (var info in viewList)
        //        {
        //            dr[r3cnt++] = (info.AS_Overall_Totals).ToString("C0", CultureInfo.CurrentCulture);
        //            dr[r3cnt++] = (info.OSS_Overall_Totals).ToString("C0", CultureInfo.CurrentCulture);
        //            dr[r3cnt++] = (info.TwoWP_Overall_Totals).ToString("C0", CultureInfo.CurrentCulture);
        //            dr[r3cnt++] = (info.AS_Overall_Totals + info.OSS_Overall_Totals + info.TwoWP_Overall_Totals).ToString("C0", CultureInfo.CurrentCulture);

        //        }

        //        dt.Rows.Add(dr);
        //        return dt;

        //    }
        //}

        public System.Data.DataTable BUSummary(string year)
        {
            string presentUserName = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).EmployeeName;

            DataTable dtBUSummary = new DataTable();

            using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

            {


                connection();
                BudgetingOpenConnection();
                string qry = " EXEC [dbo].[HOE_BUSummary] '" + year + "','" + User.Identity.Name.Split('\\')[1].ToUpper() + "' ";
                SqlCommand cmd = new SqlCommand(qry, budgetingcon);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dtBUSummary);
                BudgetingCloseConnection();

                System.Data.DataTable dt = new System.Data.DataTable();

                for (int i = 0; i < dtBUSummary.Columns.Count; i++)
                {
                    dt.Columns.Add(dtBUSummary.Columns[i].ColumnName, typeof(string)); /// Planned + (year variable) / Utilized  + (year variable)

                }
                dt.Columns.Add("Totals", typeof(string));

                DataRow dr;
                decimal rowTotals = 0, colTotals = 0;

                for (int rownum = 0; rownum < dtBUSummary.Rows.Count; rownum++)
                {
                    dr = dt.NewRow();
                    //if (rownum < dtBUSummary.Rows.Count)
                    //{
                    colTotals = 0;
                    for (int colnum = 0; colnum < dtBUSummary.Columns.Count + 1; colnum++)
                    {
                        if (colnum == 0)
                        {
                            dr[colnum] = dtBUSummary.Rows[rownum][colnum].ToString();
                        }
                        else if (colnum < dtBUSummary.Columns.Count && colnum > 0)
                        {
                            dr[colnum] = Math.Round((decimal)(dtBUSummary.Rows[rownum][colnum]), MidpointRounding.AwayFromZero).ToString("C0", CultureInfo.CurrentCulture);

                            if (colnum > 0)
                            {
                                colTotals = colTotals + Convert.ToDecimal(dtBUSummary.Rows[rownum][colnum].ToString());
                            }
                        }
                        else
                        {
                            dr[colnum] = Math.Round((decimal)(colTotals), MidpointRounding.AwayFromZero).ToString("C0", CultureInfo.CurrentCulture);

                        }
                    }

                    dt.Rows.Add(dr);



                }

                return dt;

            }
        }

        public System.Data.DataTable XC_BUSummary(string year)
        {
            //string presentUserDept = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).Department;
            string presentUserName = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).EmployeeName;


            using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

            {
                var HOElist = db.Planning_HOE_Table.ToList();
                List<string> h = new List<string>();
                //if (HOElist.FindAll(p => p.Proxy_NTID != null).Count != 0)
                //{
                    //if (HOElist.FindAll(xi => xi.Proxy_NTID == User.Identity.Name.Split('\\')[1].ToUpper()).Count() != 0)
                    //{
                    //    var HOE = db.Planning_HOE_Table.ToList().Find(xi => xi.Proxy_NTID == User.Identity.Name.Split('\\')[1].ToUpper()).HOE_NTID.Trim().ToUpper();
                    //    presentUserName = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(HOE)).EmployeeName;
                    //}
                    
                    DataTable dtproxy = new DataTable();
                    if (db.Planning_HOE_Table.ToList().FindAll(xi => xi.HOE_NTID == User.Identity.Name.Split('\\')[1].ToUpper()).Count() != 0) //Present user is HOE
                    {
                        //x = presentUserName.TrimEnd().TrimStart().Split(' ').ToList();

                        //h.Add(presentUserName.Trim().ToString());

                        connection();
                        BudgetingOpenConnection();
                        string qry = " Select HOE_FullName from Planning_HOE_Table where Proxy_NTID = '" + User.Identity.Name.Split('\\')[1].ToUpper() + "' ";
                        SqlCommand cmd = new SqlCommand(qry, budgetingcon);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dtproxy);
                        BudgetingCloseConnection();

                        if (dtproxy.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtproxy.Rows.Count; i++)
                            {
                                h.Add(dtproxy.Rows[i]["HOE_FullName"].ToString());
                            }
                        }

                        //Semaphore Approach-2 //if(db.Planning_HOE_Table.ToList().Find(xi=>xi.Active_NTID.Contains(db)))

                    }
                    else if (db.Planning_HOE_Table.ToList().FindAll(xi => xi.Proxy_NTID == User.Identity.Name.Split('\\')[1].ToUpper()).Count() != 0)
                    {
                        //x = db.Planning_HOE_Table.ToList().Find(xi => xi.Proxy_NTID == User.Identity.Name.Split('\\')[1].ToUpper()).HOE_FullName.TrimEnd().TrimStart().Split(' ').ToList();

                        connection();
                        BudgetingOpenConnection();
                        string qry = " Select HOE_FullName from Planning_HOE_Table where Proxy_NTID = '" + User.Identity.Name.Split('\\')[1].ToUpper() + "' ";
                        SqlCommand cmd = new SqlCommand(qry, budgetingcon);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dtproxy);
                        BudgetingCloseConnection();

                        if (dtproxy.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtproxy.Rows.Count; i++)
                            {
                                h.Add(dtproxy.Rows[i]["HOE_FullName"].ToString());
                            }
                        }


                    }
                    else
                        //x = presentUserName.TrimEnd().TrimStart().Split(' ').ToList();
                        h.Add(presentUserName.Trim().ToString());

                //}
                List<BUSummary_XC> viewList = new List<BUSummary_XC>();

                decimal DA_MAE_Totals = 0, DA_NMAE_Totals = 0, DA_SoftwareTotals = 0;
                decimal AD_MAE_Totals = 0, AD_NMAE_Totals = 0, AD_SoftwareTotals = 0;
                decimal TwoWP_MAE_Totals = 0, TwoWP_NMAE_Totals = 0, TwoWP_SoftwareTotals = 0;

                List<RequestItems_Table> reqList = db.RequestItems_Table.Where(x => x.VKM_Year.ToString().Contains(year)).ToList<RequestItems_Table>();

                reqList = reqList.FindAll(dpt => BudgetingController.lstDEPTs.Find(x => x.ID.Equals(int.Parse(dpt.DEPT.Trim()))).DEPT.Contains("XC"));

                string y = "";
                bool isPresentUser = true;
                //foreach (RequestItems_Table item in reqList.FindAll(x => x.BU.Equals(BudgetingController.lstBUs.Find(bu => bu.BU.Trim().Equals("DA")).ID.ToString())).FindAll(x => x.DHNT.Trim().Contains(/*L2ReviewerName*/presentUserName)).FindAll(x => x.ApprovalDH == true).FindAll(x => x.ApprovedDH == false))
                foreach (RequestItems_Table item in reqList.FindAll(p => p.BU.Equals(BudgetingController.lstBUs.Find(bu => bu.BU.Trim().Equals("DA")).ID.ToString())).FindAll(p=> p.ApprovalDH == true).FindAll(p => p.ApprovedDH == false))
                {
                    y = item.DHNT.TrimStart().TrimEnd().ToString();
                    

                    var matchingvalues = h.Where(stringToCheck => stringToCheck.Contains(y));
                    if (matchingvalues.Count() == 0)
                    {
                        isPresentUser = false;
                        break;
                    }
                    //}
                    if (isPresentUser == false)
                    {
                        isPresentUser = true;
                        continue;
                    }
                    switch (BudgetingController.lstCostElement.Find(cost => cost.ID.Equals(Int32.Parse(item.CostElement))).CostElement)
                    {
                        case "MAE":
                            {

                                DA_MAE_Totals += (decimal)item.TotalPrice;

                                break;
                            }
                        case "Non-MAE":
                            {

                                DA_NMAE_Totals += (decimal)item.TotalPrice;

                                break;
                            }
                        case "Software":
                            {

                                DA_SoftwareTotals += (decimal)item.TotalPrice;

                                break;
                            }
                        default:
                            continue;
                    }
                }
                foreach (RequestItems_Table item in reqList.FindAll(x => x.BU.Trim().Equals(BudgetingController.lstBUs.Find(bu => bu.BU.Equals("AD")).ID.ToString())).FindAll(x => x.DHNT.Trim().Contains(/*L2ReviewerName*/presentUserName)).FindAll(x => x.ApprovalDH == true).FindAll(x => x.ApprovedDH == false))
                {
                    switch (BudgetingController.lstCostElement.Find(cost => cost.ID.Equals(Int32.Parse(item.CostElement))).CostElement)
                    {
                        case "MAE":
                            {

                                AD_MAE_Totals += (decimal)item.TotalPrice;

                                break;
                            }
                        case "Non-MAE":
                            {

                                AD_NMAE_Totals += (decimal)item.TotalPrice;

                                break;
                            }
                        case "Software":
                            {

                                AD_SoftwareTotals += (decimal)item.TotalPrice;

                                break;
                            }
                        default:
                            continue;
                    }
                }
                foreach (RequestItems_Table item in reqList.FindAll(x => x.BU.Trim().Equals(BudgetingController.lstBUs.Find(bu => bu.BU.Equals("2WP")).ID.ToString())).FindAll(x => x.DHNT.Trim().Contains(/*L2ReviewerName*/presentUserName)).FindAll(x => x.ApprovalDH == true).FindAll(x => x.ApprovedDH == false))
                {
                    switch (BudgetingController.lstCostElement.Find(cost => cost.ID.Equals(Int32.Parse(item.CostElement))).CostElement)
                    {
                        case "MAE":
                            {

                                TwoWP_MAE_Totals += (decimal)item.TotalPrice;

                                break;
                            }
                        case "Non-MAE":
                            {

                                TwoWP_NMAE_Totals += (decimal)item.TotalPrice;

                                break;
                            }
                        case "Software":
                            {

                                TwoWP_SoftwareTotals += (decimal)item.TotalPrice;

                                break;
                            }
                        default:
                            continue;
                    }
                }

                BUSummary_XC tempobj = new BUSummary_XC();
                tempobj.vkmyear = DateTime.Now.Year.ToString();
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



                System.Data.DataTable dt = new System.Data.DataTable();
                dt.Columns.Add("Cost Element", typeof(string)); /// Planned + (year variable) / Utilized  + (year variable)

                dt.Columns.Add("DA", typeof(string)); //add vkm text to yr
                dt.Columns.Add("AD", typeof(string));
                dt.Columns.Add("2WP", typeof(string));
                dt.Columns.Add("Totals", typeof(string));



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
                    //dr[r3cnt++] = (info.DA_MAE_Totals + info.DA_NMAE_Totals + info.DA_Software_Totals + info.DA_Overall_Totals).ToString("C0", CultureInfo.CurrentCulture);
                    //dr[r3cnt++] = (info.AD_MAE_Totals + info.AD_NMAE_Totals + info.AD_Software_Totals + info.AD_Overall_Totals).ToString("C0", CultureInfo.CurrentCulture);
                    //dr[r3cnt++] = (info.TwoWP_MAE_Totals + info.TwoWP_NMAE_Totals + info.TwoWP_Software_Totals + info.TwoWP_Overall_Totals).ToString("C0", CultureInfo.CurrentCulture);
                    //dr[r3cnt++] = (info.DA_MAE_Totals + info.AD_MAE_Totals + info.TwoWP_MAE_Totals +
                    //               info.DA_NMAE_Totals + info.AD_NMAE_Totals + info.TwoWP_NMAE_Totals +
                    //               info.DA_Software_Totals + info.AD_Software_Totals + info.TwoWP_Software_Totals
                    //                ).ToString("C0", CultureInfo.CurrentCulture);
                    dr[r3cnt++] = (info.DA_Overall_Totals).ToString("C0", CultureInfo.CurrentCulture);
                    dr[r3cnt++] = (info.AD_Overall_Totals).ToString("C0", CultureInfo.CurrentCulture);
                    dr[r3cnt++] = (info.TwoWP_Overall_Totals).ToString("C0", CultureInfo.CurrentCulture);
                    dr[r3cnt++] = (info.DA_Overall_Totals + info.AD_Overall_Totals + info.TwoWP_Overall_Totals).ToString("C0", CultureInfo.CurrentCulture);

                }

                dt.Rows.Add(dr);
                return dt;
            }
        }

        public System.Data.DataTable CCXC_BUSummary(string year)
        {
            //string presentUserDept = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).Department;
            string presentUserName = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).EmployeeName;


            using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

            {
                var HOElist = db.Planning_HOE_Table.ToList();
                //if (HOElist.FindAll(x => x.Proxy_NTID != null).Count != 0)
                //{
                //    if (HOElist.FindAll(xi => xi.Proxy_NTID == User.Identity.Name.Split('\\')[1].ToUpper()).Count() != 0)
                //    {
                //        var HOE = db.Planning_HOE_Table.ToList().Find(xi => xi.Proxy_NTID == User.Identity.Name.Split('\\')[1].ToUpper()).HOE_NTID.Trim().ToUpper();
                //        presentUserName = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(HOE)).EmployeeName;
                //    }


                //}

                List<string> h = new List<string>();
                //if (HOElist.FindAll(p => p.Proxy_NTID != null).Count != 0)
                //{
                //if (HOElist.FindAll(xi => xi.Proxy_NTID == User.Identity.Name.Split('\\')[1].ToUpper()).Count() != 0)
                //{
                //    var HOE = db.Planning_HOE_Table.ToList().Find(xi => xi.Proxy_NTID == User.Identity.Name.Split('\\')[1].ToUpper()).HOE_NTID.Trim().ToUpper();
                //    presentUserName = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(HOE)).EmployeeName;
                //}

                DataTable dtproxy = new DataTable();
                if (db.Planning_HOE_Table.ToList().FindAll(xi => xi.HOE_NTID == User.Identity.Name.Split('\\')[1].ToUpper()).Count() != 0) //Present user is HOE
                {
                    //x = presentUserName.TrimEnd().TrimStart().Split(' ').ToList();

                    h.Add(presentUserName.Trim().ToString());

                    //connection();
                    //BudgetingOpenConnection();
                    //string qry = " Select HOE_FullName from Planning_HOE_Table where Proxy_NTID = '" + User.Identity.Name.Split('\\')[1].ToUpper() + "' ";
                    //SqlCommand cmd = new SqlCommand(qry, budgetingcon);
                    //SqlDataAdapter da = new SqlDataAdapter(cmd);
                    //da.Fill(dtproxy);
                    //BudgetingCloseConnection();

                    //if (dtproxy.Rows.Count > 0)
                    //{
                    //    for (int i = 0; i < dtproxy.Rows.Count; i++)
                    //    {
                    //        h.Add(dtproxy.Rows[i]["HOE_FullName"].ToString());
                    //    }
                    //}

                    //Semaphore Approach-2 //if(db.Planning_HOE_Table.ToList().Find(xi=>xi.Active_NTID.Contains(db)))

                }
                if (db.Planning_HOE_Table.ToList().FindAll(xi => xi.Proxy_NTID == User.Identity.Name.Split('\\')[1].ToUpper()).Count() != 0)
                {
                    //x = db.Planning_HOE_Table.ToList().Find(xi => xi.Proxy_NTID == User.Identity.Name.Split('\\')[1].ToUpper()).HOE_FullName.TrimEnd().TrimStart().Split(' ').ToList();

                    connection();
                    BudgetingOpenConnection();
                    string qry = " Select HOE_FullName from Planning_HOE_Table where Proxy_NTID = '" + User.Identity.Name.Split('\\')[1].ToUpper() + "' ";
                    SqlCommand cmd = new SqlCommand(qry, budgetingcon);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dtproxy);
                    BudgetingCloseConnection();

                    if (dtproxy.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtproxy.Rows.Count; i++)
                        {
                            h.Add(dtproxy.Rows[i]["HOE_FullName"].ToString());
                        }
                    }


                }
                if (h.Count() ==0)
                    //x = presentUserName.TrimEnd().TrimStart().Split(' ').ToList();
                    h.Add(presentUserName.Trim().ToString());






                List<BUSummary_CCXC> viewList = new List<BUSummary_CCXC>();
                decimal AS_MAE_Totals = 0, AS_NMAE_Totals = 0, AS_SoftwareTotals = 0;
                decimal OSS_MAE_Totals = 0, OSS_NMAE_Totals = 0, OSS_SoftwareTotals = 0;
                decimal DA_MAE_Totals = 0, DA_NMAE_Totals = 0, DA_SoftwareTotals = 0;
                decimal AD_MAE_Totals = 0, AD_NMAE_Totals = 0, AD_SoftwareTotals = 0;
                decimal TwoWP_MAE_Totals = 0, TwoWP_NMAE_Totals = 0, TwoWP_SoftwareTotals = 0;

                List<RequestItems_Table> reqList = db.RequestItems_Table.Where(x => x.SubmitDate.ToString().Contains(DateTime.Now.Year.ToString())).ToList<RequestItems_Table>();

                string y = "";
                bool isPresentUser = true;
                //foreach (RequestItems_Table item in reqList.FindAll(x => x.BU.Equals(BudgetingController.lstBUs.Find(bu => bu.BU.Trim().Equals("MB")).ID.ToString())).FindAll(x => x.DHNT.Trim().Contains(/*L2ReviewerName*/presentUserName)).FindAll(x => x.ApprovalDH == true).FindAll(x => x.ApprovedDH == false))
                foreach (RequestItems_Table item in reqList.FindAll(x => x.BU.Equals(BudgetingController.lstBUs.Find(bu => bu.BU.Trim().Equals("MB")).ID.ToString())).FindAll(x => x.ApprovalDH == true).FindAll(x => x.ApprovedDH == false))
                {
                    y = item.DHNT.TrimStart().TrimEnd().ToString();


                    var matchingvalues = h.Where(stringToCheck => stringToCheck.Contains(y));
                    if (matchingvalues.Count() == 0)
                    {
                        isPresentUser = false;
                        break;
                    }
                    //}
                    if (isPresentUser == false)
                    {
                        isPresentUser = true;
                        continue;
                    }
                    switch (BudgetingController.lstCostElement.Find(cost => cost.ID.Equals(Int32.Parse(item.CostElement))).CostElement)
                    {
                        case "MAE":
                            {

                                AS_MAE_Totals += (decimal)item.TotalPrice;

                                break;
                            }
                        case "Non-MAE":
                            {

                                AS_NMAE_Totals += (decimal)item.TotalPrice;

                                break;
                            }
                        case "Software":
                            {

                                AS_SoftwareTotals += (decimal)item.TotalPrice;

                                break;
                            }
                        default:
                            continue;
                    }
                }
                //foreach (RequestItems_Table item in reqList.FindAll(x => x.BU.Trim().Equals(BudgetingController.lstBUs.Find(bu => bu.BU.Equals("OSS")).ID.ToString())).FindAll(x => x.DHNT.Trim().Contains(/*L2ReviewerName*/presentUserName)).FindAll(x => x.ApprovalDH == true).FindAll(x => x.ApprovedDH == false))
                foreach (RequestItems_Table item in reqList.FindAll(x => x.BU.Trim().Equals(BudgetingController.lstBUs.Find(bu => bu.BU.Equals("OSS")).ID.ToString())).FindAll(x => x.ApprovalDH == true).FindAll(x => x.ApprovedDH == false))
                    {
                    y = item.DHNT.TrimStart().TrimEnd().ToString();


                    var matchingvalues = h.Where(stringToCheck => stringToCheck.Contains(y));
                    if (matchingvalues.Count() == 0)
                    {
                        isPresentUser = false;
                        break;
                    }
                    //}
                    if (isPresentUser == false)
                    {
                        isPresentUser = true;
                        continue;
                    }
                    switch (BudgetingController.lstCostElement.Find(cost => cost.ID.Equals(Int32.Parse(item.CostElement))).CostElement)
                    {
                        case "MAE":
                            {

                                OSS_MAE_Totals += (decimal)item.TotalPrice;

                                break;
                            }
                        case "Non-MAE":
                            {

                                OSS_NMAE_Totals += (decimal)item.TotalPrice;

                                break;
                            }
                        case "Software":
                            {

                                OSS_SoftwareTotals += (decimal)item.TotalPrice;

                                break;
                            }
                        default:
                            continue;
                    }
                }


                //foreach (RequestItems_Table item in reqList.FindAll(x => x.BU.Equals(BudgetingController.lstBUs.Find(bu => bu.BU.Trim().Equals("DA")).ID.ToString())).FindAll(x => x.DHNT.Trim().Contains(/*L2ReviewerName*/presentUserName)).FindAll(x => x.ApprovalDH == true).FindAll(x => x.ApprovedDH == false))
                foreach (RequestItems_Table item in reqList.FindAll(x => x.BU.Equals(BudgetingController.lstBUs.Find(bu => bu.BU.Trim().Equals("DA")).ID.ToString())).FindAll(x => x.ApprovalDH == true).FindAll(x => x.ApprovedDH == false))
                {
                    y = item.DHNT.TrimStart().TrimEnd().ToString();


                    var matchingvalues = h.Where(stringToCheck => stringToCheck.Contains(y));
                    if (matchingvalues.Count() == 0)
                    {
                        isPresentUser = false;
                        break;
                    }
                    //}
                    if (isPresentUser == false)
                    {
                        isPresentUser = true;
                        continue;
                    }
                    switch (BudgetingController.lstCostElement.Find(cost => cost.ID.Equals(Int32.Parse(item.CostElement))).CostElement)
                    {
                        case "MAE":
                            {

                                DA_MAE_Totals += (decimal)item.TotalPrice;

                                break;
                            }
                        case "Non-MAE":
                            {

                                DA_NMAE_Totals += (decimal)item.TotalPrice;

                                break;
                            }
                        case "Software":
                            {

                                DA_SoftwareTotals += (decimal)item.TotalPrice;

                                break;
                            }
                        default:
                            continue;
                    }
                }
                //foreach (RequestItems_Table item in reqList.FindAll(x => x.BU.Trim().Equals(BudgetingController.lstBUs.Find(bu => bu.BU.Equals("AD")).ID.ToString())).FindAll(x => x.DHNT.Trim().Contains(/*L2ReviewerName*/presentUserName)).FindAll(x => x.ApprovalDH == true).FindAll(x => x.ApprovedDH == false))
                foreach (RequestItems_Table item in reqList.FindAll(x => x.BU.Trim().Equals(BudgetingController.lstBUs.Find(bu => bu.BU.Equals("AD")).ID.ToString())).FindAll(x => x.ApprovalDH == true).FindAll(x => x.ApprovedDH == false))
                {
                    y = item.DHNT.TrimStart().TrimEnd().ToString();


                    var matchingvalues = h.Where(stringToCheck => stringToCheck.Contains(y));
                    if (matchingvalues.Count() == 0)
                    {
                        isPresentUser = false;
                        break;
                    }
                    //}
                    if (isPresentUser == false)
                    {
                        isPresentUser = true;
                        continue;
                    }
                    switch (BudgetingController.lstCostElement.Find(cost => cost.ID.Equals(Int32.Parse(item.CostElement))).CostElement)
                    {
                        case "MAE":
                            {

                                AD_MAE_Totals += (decimal)item.TotalPrice;

                                break;
                            }
                        case "Non-MAE":
                            {

                                AD_NMAE_Totals += (decimal)item.TotalPrice;

                                break;
                            }
                        case "Software":
                            {

                                AD_SoftwareTotals += (decimal)item.TotalPrice;

                                break;
                            }
                        default:
                            continue;
                    }
                }
                //foreach (RequestItems_Table item in reqList.FindAll(x => x.BU.Trim().Equals(BudgetingController.lstBUs.Find(bu => bu.BU.Equals("2WP")).ID.ToString())).FindAll(x => x.DHNT.Trim().Contains(/*L2ReviewerName*/presentUserName)).FindAll(x => x.ApprovalDH == true).FindAll(x => x.ApprovedDH == false))
                foreach (RequestItems_Table item in reqList.FindAll(x => x.BU.Trim().Equals(BudgetingController.lstBUs.Find(bu => bu.BU.Equals("2WP")).ID.ToString())).FindAll(x => x.ApprovalDH == true).FindAll(x => x.ApprovedDH == false))
                {
                    y = item.DHNT.TrimStart().TrimEnd().ToString();


                    var matchingvalues = h.Where(stringToCheck => stringToCheck.Contains(y));
                    if (matchingvalues.Count() == 0)
                    {
                        isPresentUser = false;
                        break;
                    }
                    //}
                    if (isPresentUser == false)
                    {
                        isPresentUser = true;
                        continue;
                    }
                    switch (BudgetingController.lstCostElement.Find(cost => cost.ID.Equals(Int32.Parse(item.CostElement))).CostElement)
                    {
                        case "MAE":
                            {

                                TwoWP_MAE_Totals += (decimal)item.TotalPrice;

                                break;
                            }
                        case "Non-MAE":
                            {

                                TwoWP_NMAE_Totals += (decimal)item.TotalPrice;

                                break;
                            }
                        case "Software":
                            {

                                TwoWP_SoftwareTotals += (decimal)item.TotalPrice;

                                break;
                            }
                        default:
                            continue;
                    }
                }

                BUSummary_CCXC tempobj = new BUSummary_CCXC();
                tempobj.vkmyear = DateTime.Now.Year.ToString();
                tempobj.AS_MAE_Totals = AS_MAE_Totals;
                tempobj.AS_NMAE_Totals = AS_NMAE_Totals;
                tempobj.AS_Software_Totals = AS_SoftwareTotals;
                tempobj.AS_Overall_Totals = AS_MAE_Totals + AS_NMAE_Totals + AS_SoftwareTotals;
                tempobj.OSS_MAE_Totals = OSS_MAE_Totals;
                tempobj.OSS_NMAE_Totals = OSS_NMAE_Totals;
                tempobj.OSS_Software_Totals = OSS_SoftwareTotals;
                tempobj.OSS_Overall_Totals = OSS_MAE_Totals + OSS_NMAE_Totals + OSS_SoftwareTotals;

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



                System.Data.DataTable dt = new System.Data.DataTable();
                dt.Columns.Add("Cost Element", typeof(string)); /// Planned + (year variable) / Utilized  + (year variable)
                dt.Columns.Add("MB", typeof(string)); //add vkm text to yr
                dt.Columns.Add("OSS", typeof(string));
                dt.Columns.Add("DA", typeof(string));
                dt.Columns.Add("AD", typeof(string));
                dt.Columns.Add("2WP", typeof(string));
                dt.Columns.Add("Totals", typeof(string));



                DataRow dr = dt.NewRow();
                dr[0] = "MAE";
                int rcnt = 1;

                foreach (var info in viewList)
                {
                    dr[rcnt++] = info.AS_MAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
                    dr[rcnt++] = info.OSS_MAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
                    dr[rcnt++] = info.DA_MAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
                    dr[rcnt++] = info.AD_MAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
                    dr[rcnt++] = info.TwoWP_MAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
                    dr[rcnt++] = (info.AS_MAE_Totals + info.OSS_MAE_Totals + info.DA_MAE_Totals + info.AD_MAE_Totals + info.TwoWP_MAE_Totals).ToString("C0", CultureInfo.CurrentCulture);
                }
                dt.Rows.Add(dr);

                dr = dt.NewRow();
                dr[0] = "Non-MAE";
                int r1cnt = 1;
                foreach (var info in viewList)
                {
                    dr[rcnt++] = info.AS_NMAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
                    dr[rcnt++] = info.OSS_NMAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
                    dr[r1cnt++] = info.DA_NMAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
                    dr[r1cnt++] = info.AD_NMAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
                    dr[r1cnt++] = info.TwoWP_NMAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
                    dr[r1cnt++] = (info.AS_NMAE_Totals + info.OSS_NMAE_Totals + info.DA_NMAE_Totals + info.AD_NMAE_Totals + info.TwoWP_NMAE_Totals).ToString("C0", CultureInfo.CurrentCulture);
                }

                dt.Rows.Add(dr);

                dr = dt.NewRow();
                dr[0] = "Software";
                int r2cnt = 1;
                foreach (var info in viewList)
                {
                    dr[rcnt++] = info.AS_Software_Totals.ToString("C0", CultureInfo.CurrentCulture);
                    dr[rcnt++] = info.OSS_Software_Totals.ToString("C0", CultureInfo.CurrentCulture);
                    dr[r2cnt++] = info.DA_Software_Totals.ToString("C0", CultureInfo.CurrentCulture);
                    dr[r2cnt++] = info.AD_Software_Totals.ToString("C0", CultureInfo.CurrentCulture);
                    dr[r2cnt++] = info.TwoWP_Software_Totals.ToString("C0", CultureInfo.CurrentCulture);
                    dr[r2cnt++] = (info.AS_Software_Totals + info.OSS_Software_Totals + info.DA_Software_Totals + info.AD_Software_Totals + info.TwoWP_Software_Totals).ToString("C0", CultureInfo.CurrentCulture);
                }

                dt.Rows.Add(dr);

                dr = dt.NewRow();
                dr[0] = "Totals";
                int r3cnt = 1;

                foreach (var info in viewList)
                {
                    //dr[r3cnt++] = (info.DA_MAE_Totals + info.DA_NMAE_Totals + info.DA_Software_Totals + info.DA_Overall_Totals).ToString("C0", CultureInfo.CurrentCulture);
                    //dr[r3cnt++] = (info.AD_MAE_Totals + info.AD_NMAE_Totals + info.AD_Software_Totals + info.AD_Overall_Totals).ToString("C0", CultureInfo.CurrentCulture);
                    //dr[r3cnt++] = (info.TwoWP_MAE_Totals + info.TwoWP_NMAE_Totals + info.TwoWP_Software_Totals + info.TwoWP_Overall_Totals).ToString("C0", CultureInfo.CurrentCulture);
                    //dr[r3cnt++] = (info.DA_MAE_Totals + info.AD_MAE_Totals + info.TwoWP_MAE_Totals +
                    //               info.DA_NMAE_Totals + info.AD_NMAE_Totals + info.TwoWP_NMAE_Totals +
                    //               info.DA_Software_Totals + info.AD_Software_Totals + info.TwoWP_Software_Totals
                    //                ).ToString("C0", CultureInfo.CurrentCulture);
                    dr[rcnt++] = info.AS_Overall_Totals.ToString("C0", CultureInfo.CurrentCulture);
                    dr[rcnt++] = info.OSS_Overall_Totals.ToString("C0", CultureInfo.CurrentCulture);
                    dr[r3cnt++] = (info.DA_Overall_Totals).ToString("C0", CultureInfo.CurrentCulture);
                    dr[r3cnt++] = (info.AD_Overall_Totals).ToString("C0", CultureInfo.CurrentCulture);
                    dr[r3cnt++] = (info.TwoWP_Overall_Totals).ToString("C0", CultureInfo.CurrentCulture);
                    dr[r3cnt++] = (info.AS_Overall_Totals + info.OSS_Overall_Totals + info.DA_Overall_Totals + info.AD_Overall_Totals + info.TwoWP_Overall_Totals).ToString("C0", CultureInfo.CurrentCulture);

                }

                dt.Rows.Add(dr);
                return dt;
            }
        }

        /// <summary>
        /// Function to generate the cost data for Depts
        /// </summary>
        /// <returns>datatable to be used</returns>
        public System.Data.DataTable DeptSummaryData(string year)
        {
            try
            {

                using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

                {
                    System.Security.Principal.IPrincipal User = System.Web.HttpContext.Current.User;
                    
                    //List<RequestItems_Table> reqList = db.RequestItems_Table.Where(x => x.VKM_Year.ToString().Contains(year)).ToList<RequestItems_Table>();
                    List<DeptSummary> viewList = new List<DeptSummary>();
                    List<DeptSummary> viewList1 = new List<DeptSummary>();
                    //List<string> userSectionDeptList = new List<string>();
                    string presentUserNTID = User.Identity.Name.Split('\\')[1].ToUpper().Trim();
                    List<string> h = new List<string>();
                    



                    //var HOElist = db.Planning_HOE_Table.ToList();
                    //if (HOElist.FindAll(x => x.Proxy_NTID != null).Count != 0)
                    //{
                    //    if (HOElist.FindAll(xi => xi.Proxy_NTID == User.Identity.Name.Split('\\')[1].ToUpper()).Count() != 0)
                    //    {
                    //        presentUserNTID = db.Planning_HOE_Table.ToList().Find(xi => xi.Proxy_NTID == User.Identity.Name.Split('\\')[1].ToUpper()).HOE_NTID.Trim().ToUpper();

                    //    }


                    //}

                    DataTable dtproxy = new DataTable();
                    if (db.Planning_HOE_Table.ToList().FindAll(xi => xi.HOE_NTID == User.Identity.Name.Split('\\')[1].ToUpper()).Count() != 0) //Present user is HOE
                    {
                        //x = presentUserName.TrimEnd().TrimStart().Split(' ').ToList();

                        h.Add(presentUserNTID.Trim().ToString());

                        //connection();
                        //BudgetingOpenConnection();
                        //string qry = " Select HOE_NTID from Planning_HOE_Table where Proxy_NTID = '" + User.Identity.Name.Split('\\')[1].ToUpper() + "' ";
                        //SqlCommand cmd = new SqlCommand(qry, budgetingcon);
                        //SqlDataAdapter da = new SqlDataAdapter(cmd);
                        //da.Fill(dtproxy);
                        //BudgetingCloseConnection();

                        //if (dtproxy.Rows.Count > 0)
                        //{
                        //    for (int i = 0; i < dtproxy.Rows.Count; i++)
                        //    {
                        //        h.Add(dtproxy.Rows[i]["HOE_NTID"].ToString());
                        //    }
                        //}

                        //Semaphore Approach-2 //if(db.Planning_HOE_Table.ToList().Find(xi=>xi.Active_NTID.Contains(db)))

                    }
                    if (db.Planning_HOE_Table.ToList().FindAll(xi => xi.Proxy_NTID == User.Identity.Name.Split('\\')[1].ToUpper()).Count() != 0)
                    {
                        //x = db.Planning_HOE_Table.ToList().Find(xi => xi.Proxy_NTID == User.Identity.Name.Split('\\')[1].ToUpper()).HOE_FullName.TrimEnd().TrimStart().Split(' ').ToList();

                        connection();
                        BudgetingOpenConnection();
                        string qry = " Select HOE_NTID from Planning_HOE_Table where Proxy_NTID = '" + User.Identity.Name.Split('\\')[1].ToUpper() + "' ";
                        SqlCommand cmd = new SqlCommand(qry, budgetingcon);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dtproxy);
                        BudgetingCloseConnection();

                        if (dtproxy.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtproxy.Rows.Count; i++)
                            {
                                h.Add(dtproxy.Rows[i]["HOE_NTID"].ToString());
                            }
                        }


                    }
                    if (h.Count() == 0)
                        //x = presentUserName.TrimEnd().TrimStart().Split(' ').ToList();
                        h.Add(presentUserNTID.Trim().ToString());

                    decimal OMAE_Totals = 0, ONMAE_Totals = 0, OSoftwareTotals = 0;
                    DataTable dt1 = new DataTable();
                    if (h.Count > 0)
                    {
                        connection();
                        BudgetingOpenConnection();

                        for (int i = 0; i < h.Count; i++)
                        {
                            dt1 = new DataTable();
                            string Query = " Exec [dbo].[HOE_DeptSummary] '" + h[i].ToString() + "', '" + year + "'";
                            SqlCommand cmd = new SqlCommand(Query, budgetingcon);
                            SqlDataAdapter da = new SqlDataAdapter(cmd);
                            da.Fill(dt1);

                            foreach (DataRow item in dt1.Rows)
                            {
                                DeptSummary tempobj = new DeptSummary();
                                tempobj.deptName = item["DeptName"].ToString();
                                tempobj.MAE_Totals = item["MAE_Totals"].ToString() != "" ? Convert.ToDecimal(item["MAE_Totals"]) : 0;
                                OMAE_Totals += tempobj.MAE_Totals;
                                tempobj.NMAE_Totals = item["NMAE_Totals"].ToString() != "" ? Convert.ToDecimal(item["NMAE_Totals"]) : 0;
                                ONMAE_Totals += tempobj.NMAE_Totals;
                                tempobj.Software_Totals = item["Software_Totals"].ToString() != "" ? Convert.ToDecimal(item["Software_Totals"]) : 0;
                                OSoftwareTotals += tempobj.Software_Totals;
                                tempobj.Overall_Totals = tempobj.MAE_Totals + tempobj.NMAE_Totals + tempobj.Software_Totals;
                                viewList1.Add(tempobj);
                            }
                        }


                        viewList = viewList1
                        .GroupBy(l => l.deptName)
                        .Select(cl => new DeptSummary
                        {
                            deptName = cl.FirstOrDefault().deptName,
                            MAE_Totals = cl.Sum(c => c.MAE_Totals),
                            NMAE_Totals = cl.Sum(c => c.NMAE_Totals),
                            Software_Totals = cl.Sum(c => c.Software_Totals),
                            Overall_Totals = cl.Sum(c => c.Overall_Totals),
                        }).ToList();


                        //    userSectionDeptList = db.SPOTONData_Table_2022.ToList<SPOTONData_Table_2022>().FindAll(x => x.Section == BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(presentUserNTID.Trim().ToUpper())).Section).Select(item => item.Department).Distinct().ToList();
                        BudgetingCloseConnection();
                    }

                    ////CODE TO GET GROUOSS OF COST ELEMENT
                    //IEnumerable<IGrouping<string, RequestItems_Table>> query = reqList.GroupBy(item => item.CostElement);
                    //decimal OMAE_Totals = 0, ONMAE_Totals = 0, OSoftwareTotals = 0;
                    //foreach (string dept in userSectionDeptList)
                    //{
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

                    //                    MAE_Totals += (decimal)item.TotalPrice;
                    //                }
                    //            }
                    //        }
                    //        else if (BudgetingController.lstCostElement.Find(cost => cost.ID.Equals(Int32.Parse(CostGroup.Key))).CostElement == "Non-MAE")
                    //        {
                    //            foreach (RequestItems_Table item in CostGroup)
                    //            {
                    //                if (BudgetingController.lstDEPTs.Find(departmnt => departmnt.ID.Equals(Int32.Parse(item.DEPT))).DEPT == dept)
                    //                {

                    //                    NMAE_Totals += (decimal)item.TotalPrice;
                    //                }
                    //            }

                    //        }
                    //        else if (BudgetingController.lstCostElement.Find(cost => cost.ID.Equals(Int32.Parse(CostGroup.Key))).CostElement == "Software")
                    //        {
                    //            foreach (RequestItems_Table item in CostGroup)
                    //            {
                    //                if (BudgetingController.lstDEPTs.Find(departmnt => departmnt.ID.Equals(Int32.Parse(item.DEPT))).DEPT == dept)
                    //                {

                    //                    SoftwareTotals += (decimal)item.TotalPrice;
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


                    System.Data.DataTable dt = new System.Data.DataTable();
                    dt.Columns.Add("CostElement", typeof(string));
                    dt.Columns.Add("All Depts", typeof(string));
                    //foreach (DataRow item in dt1.Rows)
                    foreach (var item in viewList)
                    {

                        dt.Columns.Add(item.deptName.ToString(), typeof(string));
                    }

                    //foreach (string dept in userSectionDeptList)
                    //{
                    //    dt.Columns.Add(dept, typeof(string));
                    //}

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
                return null;

            }
        }
        /// <summary>
        /// function to get the summary of Dept Costs to View
        /// </summary>
        /// <returns></returns>
        public ActionResult GetDeptSummaryData(string year)
        {
            try
            {
                System.Data.DataTable dt = new System.Data.DataTable();
                dt = DeptSummaryData(year);
                var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
                BudgetParam t = new BudgetParam();
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

                return Json(new { data = t }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                return Json(new { data = "" }, JsonRequestBehavior.AllowGet);
            }
        }



        /// <summary>
        /// function to edit the items at L2 level
        /// Expected actions - edit the approved quantity
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddOrEdit(RequestItemsRepoEdit1 req)
        {
            try
            {
                List<RequestItemsRepoView> viewList = new List<RequestItemsRepoView>();


                using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

                {
                    RequestItems_Table item1 = db.RequestItems_Table.AsNoTracking().Where(x => x.RequestID == req.RequestID).FirstOrDefault<RequestItems_Table>();
                    int year = (DateTime.Now.Year + 1);
                    if (item1.ApprovalDH == false)
                    {
                        viewList = GetData1(year.ToString());
                        return Json(new { success = false, data = viewList, message = "The Item Request has already been Sent Back !" }, JsonRequestBehavior.AllowGet);

                    }
                    else if (item1.ApprovedDH == true && item1.ApprovalSH == true)
                    {
                        viewList = GetData1(year.ToString());
                        return Json(new { success = false, data = viewList, message = "The Item Request has already been Submitted for VKM SPOC Review !" }, JsonRequestBehavior.AllowGet);
                    }

                    System.Security.Principal.IPrincipal User = System.Web.HttpContext.Current.User;
                    string presentUserName = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).EmployeeName;
                    string L1Remarks = db.RequestItems_Table.AsNoTracking().FirstOrDefault(x => x.RequestID == req.RequestID).Comments;

                    RequestItems_Table item = new RequestItems_Table();
                    item.Order_Type = BudgetingController.lstItems.Find(x => x.S_No == req.Item_Name).Order_Type.ToString().Trim();
                    item.UnitofMeasure = BudgetingController.lstItems.Find(x => x.S_No == req.Item_Name).UOM.ToString().Trim();
                    item.BudgetCodeDescription = BudgetingController.BudgetCodeList.Find(x => x.Budget_Code == req.BudgetCode).Budget_Code_Description.Trim();
                    item.RequestorNTID = req.RequestorNTID;
                    item.UpdatedAt = DateTime.Now;
                    item.VKM_Year = req.VKM_Year;
                    item.UpdatedAt = DateTime.Now;
                    item.BU = req.BU.ToString();
                    item.ItemName = req.Item_Name.ToString();
                    item.Category = req.Category.ToString();
                    item.Comments = L1Remarks;
                    item.L2_Remarks = req.Comments;
                    item.CostElement = req.Cost_Element.ToString();

                    item.DEPT = req.DEPT.ToString();
                    item.Group = req.Group.ToString();

                    item.OEM = req.OEM.ToString();
                    item.ReqQuantity = req.Required_Quantity;
                    item.RequestID = req.RequestID;
                    item.RequestorNT = req.Requestor;
                    item.DHNT = req.Reviewer_1;
                    item.SHNT = req.Reviewer_2;
                    item.TotalPrice = req.Total_Price;
                    item.UnitPrice = req.Unit_Price;
                    item.ApprQuantity = req.Reviewed_Quantity;
                    item.L2_Qty = req.Reviewed_Quantity;
                    item.ApprCost = req.Reviewed_Cost;
                    item.ApprovalDH = true;
                    item.ApprovalSH = false;
                    item.ApprovedDH = false;
                    item.ApprovedSH = false;
                    item.RequestDate = req.RequestDate != null ? DateTime.Parse(req.RequestDate) : DateTime.Now;
                    item.SubmitDate = req.SubmitDate != null ? DateTime.Parse(req.SubmitDate) : DateTime.Now;
                    // RequestResorde is flag to identify the approval flow, where its comes from
                    item.RequestSource = "HOE";
                    item.Project = req.Project;
                    item.BudgetCode = req.BudgetCode;

                    if (req.ActualAvailableQuantity == null || req.ActualAvailableQuantity.Trim() == string.Empty)
                        item.ActualAvailableQuantity = "NA";
                    else
                        item.ActualAvailableQuantity = req.ActualAvailableQuantity;
                    db.Entry(item).State = EntityState.Modified;
                    db.SaveChanges();
                    viewList = GetData1(year.ToString());
                    //WriteLog("AddOrEdit: Updated Successfully " + DateTime.Now.ToString());
                    return Json(new { data = viewList, success = true, message = "Updated Successfully" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch(Exception ex)
            {
               
                WriteLog("Error - AddOrEdit : NTID - " + User.Identity.Name.Split('\\')[1].Trim().ToUpper() + ", ErrorMsg : " + ex.Message.ToString() + ", At - " + DateTime.Now.ToString());

                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }

}
        /// <summary>
        /// function to send back the item requested to the L1 requestor
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SendBack(int id)
        {
            try
            {
                Emailnotify emailnotify = new Emailnotify();
                List<RequestItemsRepoView> viewList = new List<RequestItemsRepoView>();

                using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

                {
                    string presentUserName = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).EmployeeName.Trim();
                    RequestItems_Table item = db.RequestItems_Table.Where(x => x.RequestID == id).FirstOrDefault<RequestItems_Table>();
                    if (item.ApprovalDH == false)
                        return Json(new { success = false, message = "The Item Request has already been Sent Back !" }, JsonRequestBehavior.AllowGet);
                    else if (item.ApprovedDH == true && item.ApprovalSH == true)
                        return Json(new { success = false, message = "The Item Request has already been Submitted for VKM SPOC Review !" }, JsonRequestBehavior.AllowGet);


                    item.isCancelled = 1; //HoE cancelled
                    item.HOEView_ActionHistory += (System.Environment.NewLine) /*"\n\r"*/ + " SentBack by: " + presentUserName + " at: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ",";
                    item.ApprovalDH = false;
                    item.ApprQuantity = null;
                    item.ApprCost = null;
                    item.SubmitDate = null;

                    db.Entry(item).State = EntityState.Modified;
                    db.SaveChanges();

                    //email attributes
                    emailnotify.RequestID_foremail = id;
                    emailnotify.ReviewLevel = "L2";
                    emailnotify.is_ApprovalorSendback = (bool)item.ApprovalDH || (bool)item.ApprovalSH;

                    //WriteLog("SendBack: Success "  + DateTime.Now.ToString());
                    return Json(new { data = emailnotify, success = true, message = "Sent Back Successfully" }, JsonRequestBehavior.AllowGet);
                }
            }
            catch(Exception ex)
            {
                WriteLog("Error - Sendback : NTID - " + User.Identity.Name.Split('\\')[1].Trim().ToUpper() + ", ErrorMsg : " + ex.Message.ToString() + ", At - " + DateTime.Now.ToString());

                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }


        /// <summary>
        /// function to move the item to L3 approval queue
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SHApprove(int id)
        {
            try
            {
                Emailnotify emailnotify = new Emailnotify();
                List<RequestItemsRepoView> viewList = new List<RequestItemsRepoView>();


                using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

                {
                   
                    List<string> y = new List<string>();


                        string presentUserName = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).EmployeeName.Trim();

                        y.Add(presentUserName.Trim().ToString());


                    if (id == 1999999999)
                    {
                        System.Security.Principal.IPrincipal User = System.Web.HttpContext.Current.User;
                        if (db.Planning_HOE_Table.ToList().FindAll(xi => xi.Proxy_NTID == User.Identity.Name.Split('\\')[1].ToUpper()).Count() != 0) // if present user is proxy 
                        {
                            //presentUserName = db.Planning_HOE_Table.ToList().Find(xi => xi.Proxy_NTID == User.Identity.Name.Split('\\')[1].ToUpper()).HOE_FullName.Trim();
                            //var HOE_NT = db.Planning_HOE_Table.ToList().Find(xi => xi.Proxy_NTID == User.Identity.Name.Split('\\')[1].ToUpper()).HOE_NTID.Trim().ToUpper();
                            //y.Add(presentUserName.Trim().ToString());
                            DataTable dtproxy = new DataTable();
                            connection();
                            BudgetingOpenConnection();
                            string qry = " Select HOE_FullName from Planning_HOE_Table where Proxy_NTID = '" + User.Identity.Name.Split('\\')[1].ToUpper() + "' ";
                            SqlCommand cmd = new SqlCommand(qry, budgetingcon);
                            SqlDataAdapter da = new SqlDataAdapter(cmd);
                            da.Fill(dtproxy);
                            BudgetingCloseConnection();



                            if (dtproxy.Rows.Count > 0)
                            {
                                for (int i = 0; i < dtproxy.Rows.Count; i++)
                                {
                                    y.Add(dtproxy.Rows[i]["HOE_FullName"].ToString());
                                }
                            }

                        }

                        Nullable<decimal> totalAmount = 0.0M;
                        string SHNT = "";
                        string HoENT = "";
                        List<RequestItems_Table> templist = new List<RequestItems_Table>().FindAll(v => v.Fund == null || v.Fund.Trim().Contains("2"));
                        var xii = db.RequestItems_Table.ToList();


                        foreach (string u in y) //u contains individual dhname or proxy name

                        {
                            var templist1 = xii.FindAll(item => item.ApprovalDH == true).FindAll(items => items.ApprovedDH == false && items.DHNT.Trim().Equals(u.Trim())).FindAll(items => items.RequestDate.ToString().Contains(DateTime.Now.Year.ToString()));
                            templist.AddRange(templist1);
                        }






                        int totalCount = templist.Count();
                        if (totalCount == 0)
                            return Json(new { success = false, message = "The Item Requests are already been Submitted for VKM SPOC Review !" }, JsonRequestBehavior.AllowGet);





                        bool ApprovalDH = false;
                        bool ApprovalSH = false;


                        try
                        {
                            foreach (RequestItems_Table item in templist)
                            {
                                RequestItems_Table changeItem = db.RequestItems_Table.Where(x => x.RequestID == item.RequestID).FirstOrDefault<RequestItems_Table>();

                                changeItem.ApprovedDH = true;
                                changeItem.ApprovalSH = true;
                                changeItem.HOEView_ActionHistory += "\n Submitted by: " + User.Identity.Name.Split('\\')[1].ToUpper() + " at: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " , ";

                                changeItem.DHAppDate = DateTime.Now.Date;
                                if (changeItem.ApprQuantity == null)
                                    changeItem.ApprQuantity = item.ReqQuantity;
                                if (changeItem.ApprCost == null)
                                    changeItem.ApprCost = item.TotalPrice;
                                if (changeItem.ApprQuantity == null)
                                    changeItem.L2_Qty = item.ReqQuantity;
                                else
                                    changeItem.L2_Qty = item.ApprQuantity;

                                totalAmount += item.ApprCost;
                                SHNT = item.SHNT;
                                HoENT = item.DHNT;
                                item.RequestSource = "HOE";

                                ApprovalDH = (bool)changeItem.ApprovalDH;
                                ApprovalSH = (bool)changeItem.ApprovalSH;
                                db.Entry(changeItem).State = EntityState.Modified;
                                db.SaveChanges();

                            }
                            //WriteLog("SHApprove Function " + DateTime.Now.ToString());
                        }
                        catch (Exception ex)
                        {
                          
                            WriteLog("Error - SHApprove - Submit All : NTID - " + User.Identity.Name.Split('\\')[1].Trim().ToUpper() + ", ErrorMsg : " + ex.Message.ToString() + ", At - " + DateTime.Now.ToString());

                            return Json(new { success = false, message = "The Item Requests could not be updated. Please check later !" }, JsonRequestBehavior.AllowGet);

                        }

                        //email attributes
                        emailnotify.Requests_foremail = templist;
                        emailnotify.RequestID_foremail = id;
                        emailnotify.ReviewLevel = "L2";
                        emailnotify.Count = totalCount;
                        emailnotify.TotalAmount = totalAmount;
                        emailnotify.NTID_toEmail = SHNT;
                        emailnotify.NTID_ccEmail = HoENT;
                        emailnotify.is_ApprovalorSendback = (bool)ApprovalDH || (bool)ApprovalSH;

                        return Json(new { data = emailnotify, success = true, message = totalCount.ToString() + " Item(s) Reviewed Successfully" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        RequestItems_Table item = db.RequestItems_Table.Where(x => x.RequestID == id).FirstOrDefault<RequestItems_Table>();
                        if (item.ApprovalDH == false)
                            return Json(new { success = false, message = "The Item Request has already been Sent Back !" }, JsonRequestBehavior.AllowGet);
                        else if (item.ApprovedDH == true && item.ApprovalSH == true)
                            return Json(new { success = false, message = "The Item Request has already been Submitted for VKM SPOC Review !" }, JsonRequestBehavior.AllowGet);

                       

                        item.ApprovedDH = true;
                        item.ApprovalSH = true;
                        item.HOEView_ActionHistory += "\n Submitted by: " + presentUserName + " at: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " , ";
                        item.DHAppDate = DateTime.Now.Date;
                        if (item.ApprQuantity == null)
                            item.ApprQuantity = item.ReqQuantity;
                        if (item.ApprCost == null)
                            item.ApprCost = item.TotalPrice;
                        if (item.ApprQuantity == null)
                            item.L2_Qty = item.ReqQuantity;
                        else
                            item.L2_Qty = item.ApprQuantity;

                        item.RequestSource = "HOE";
                        db.Entry(item).State = EntityState.Modified;
                        db.SaveChanges();

                        //email attributes
                        emailnotify.RequestID_foremail = id;
                        emailnotify.ReviewLevel = "L2";
                        emailnotify.is_ApprovalorSendback = (bool)item.ApprovalDH || (bool)item.ApprovalSH;

                        //WriteLog("SHApprove: Submitted Successfully " + DateTime.Now.ToString());
                        return Json(new { data = emailnotify, success = true, message = "Submitted Successfully" }, JsonRequestBehavior.AllowGet);
                    }

                }
            }
            catch(Exception ex)
            {

                WriteLog("Error - SHApprove - Single Submit : NTID - " + User.Identity.Name.Split('\\')[1].Trim().ToUpper() + ", ErrorMsg : " + ex.Message.ToString() + ", At - " + DateTime.Now.ToString());

                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult LookupApprovals(string year)
        {
            LookupData lookupData = new LookupData();
            lookupData.BU_List = BudgetingController.lstBUs.OrderBy(x => x.ID).ToList();
            if (year.Contains("2021"))
            {
                lookupData.BU_List[2].BU = "AS";
                lookupData.BU_List[4].BU = "PS";
            }
            else
            {
                lookupData.BU_List[2].BU = "MB";
                lookupData.BU_List[4].BU = "OSS";
            }
            lookupData.OEM_List = BudgetingController.lstOEMs;
            lookupData.DEPT_List = BudgetingController.lstDEPTs;
            lookupData.Groups_test = BudgetingController.lstGroups_test;
           // lookupData.Item_List = BudgetingController.lstItems.ToList().FindAll(x => x.VKM_Year == (int.Parse(year)))/*.FindAll(item => item.Deleted != true)*/;
            lookupData.Category_List = BudgetingController.lstPrdCateg;
            lookupData.OrderStatus_List = BudgetingController.lstOrderStatus;
			lookupData.CostElement_List = BudgetingController.lstCostElement;
            lookupData.BudgetCodeList = BudgetingController.BudgetCodeList;

            return Json(new { data = lookupData }, JsonRequestBehavior.AllowGet);

        }

        //[HttpPost]
        //public ActionResult GetHOE_ProxyDetails()
        //{
        //    List<Planning_HOE_Table> HOE_details = new List<Planning_HOE_Table>();

        //    using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //    {

        //        HOE_details = db.Planning_HOE_Table.ToList().FindAll(xi => xi.HOE_NTID == User.Identity.Name.Split('\\')[1].ToUpper());
        //        if (HOE_details.Count() == 0)
        //        { // head => head.Role.ToUpper().Contains("HEAD")
        //            Planning_HOE_Table item = new Planning_HOE_Table();

        //            if (BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToLower().Equals(User.Identity.Name.Split('\\')[1].ToLower())) != null)
        //            {
        //                if(BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToLower().Equals(User.Identity.Name.Split('\\')[1].ToLower())).Role.ToUpper().Contains("HEAD"))
        //                {
        //                    DEPT_Table dEPT_Table = BudgetingController.lstDEPTs.Find(departmnt => departmnt.DEPT.Equals(BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToLower().Equals(User.Identity.Name.Split('\\')[1].ToLower())).Department));
        //                    var Department = dEPT_Table.ID;
        //                    item.Department = Department.ToString();
        //                    item.HOE_FullName = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToLower().Equals(User.Identity.Name.Split('\\')[1].ToLower())).EmployeeName;
        //                    item.HOE_NTID = User.Identity.Name.Split('\\')[1].ToUpper();
        //                    item.Enable_Proxy = false;
        //                    item.Updated_By = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToLower().Equals(User.Identity.Name.Split('\\')[1].ToLower())).EmployeeName;
        //                    db.Planning_HOE_Table.Add(item);
        //                    db.SaveChanges();
        //                }

        //            }
        //            HOE_details = db.Planning_HOE_Table.ToList().FindAll(xi => xi.HOE_NTID == User.Identity.Name.Split('\\')[1].ToUpper());
        //        }
        //        return Json(new
        //        {
        //            data = HOE_details
        //        }, JsonRequestBehavior.AllowGet);

        //    }

        //}

        //[HttpPost]
        //public ActionResult ChangeButtonName_enabledisable()
        //{

        //    using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //    {
        //        return Json(new
        //        {
        //            data = db.Planning_HOE_Table.ToList().Find(xi => xi.HOE_NTID == User.Identity.Name.Split('\\')[1].ToUpper()).Enable_Proxy
        //        }, JsonRequestBehavior.AllowGet);
        //    }
        //}
        //[HttpPost]
        //public ActionResult ChangeFlag_enabledisable(string is_enableDisable)
        //{


        //    using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //    {
        //        System.Security.Principal.IPrincipal User = System.Web.HttpContext.Current.User;
        //        if (db.Planning_HOE_Table.ToList().FindAll(xi => xi.HOE_NTID == User.Identity.Name.Split('\\')[1].ToUpper()).Count() != 0)
        //        {
        //            Planning_HOE_Table HOEList = db.Planning_HOE_Table.ToList().Find(xi => xi.HOE_NTID == User.Identity.Name.Split('\\')[1].ToUpper());
        //            HOEList.Department = HOEList.Department;
        //            HOEList.HOE_FullName = HOEList.HOE_FullName;
        //            HOEList.HOE_NTID = HOEList.HOE_NTID;
        //            HOEList.ID = HOEList.ID;
        //            HOEList.Proxy_FullName = HOEList.Proxy_FullName;
        //            HOEList.Proxy_NTID = HOEList.Proxy_NTID;
        //            if (is_enableDisable == "Enable Proxy") // Proxy details not available and ENable Proxy is clicked
        //            {
        //                if(HOEList.Proxy_NTID != null)
        //                    HOEList.Enable_Proxy = (is_enableDisable == "Enable Proxy") ? true : false;
        //                else
        //                    return Json(new { success = false, message = "Proxy Details are not available. Please check again" }, JsonRequestBehavior.AllowGet);

        //            }
        //            else
        //                HOEList.Enable_Proxy = (is_enableDisable == "Enable Proxy") ? true : false;

        //            db.Entry(HOEList).State = EntityState.Modified;
        //            db.SaveChanges();

        //            db.Database.CommandTimeout = 10000;

        //                if (is_enableDisable == "Enable Proxy")
        //                    return Json(new { success = true /*,message = "Proxy setting is Enabled. Requests in your Queue will be redirected to your Proxy "*/ }, JsonRequestBehavior.AllowGet);
        //                else
        //                    return Json(new { success = true, message = "Proxy setting is Disabled. Requests will be directed back to your Queue" }, JsonRequestBehavior.AllowGet);





        //        }
        //        else
        //            return Json(new { success = false, message = "HOE-Proxy Details are not available. Please check again" }, JsonRequestBehavior.AllowGet);

        //    }
        //}



    }
}
