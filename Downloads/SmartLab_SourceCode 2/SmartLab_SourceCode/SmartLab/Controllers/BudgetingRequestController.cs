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
using System.Web;
using System.Text;
using System.Data.OleDb;
using System.Text.RegularExpressions;
using System.Threading;
using System.Data.SqlClient;
using System.Configuration;
using LC_Reports_V1.Controllers;
using System.Reflection;

namespace LC_Reports_V1.Controllers
{
    public class BudgetingRequestController : Controller
    {
        private SqlConnection budgetingcon;
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
            string budgeting_constring = ConfigurationManager.ConnectionStrings["BudgetingdbConnection"].ToString();
            budgetingcon = new SqlConnection(budgeting_constring);
        }

        //static StringBuilder logs1 = new StringBuilder();

        // GET: RequestItems
        public ActionResult Index()
        {
            {
                WriteLog("************** Budgeting Request ************ " + DateTime.Now.ToString());
                string NTID = LEPlanner_authorise();

                if (NTID == "")
                {
                    // throw new HttpException(404, "Sorry! Current user is not authorised to access this view!");
                    return Content("Sorry! Current user is not authorised. Kindly contact SmartLab Tools Team for access! !");
                    //return RedirectToAction("Index", "Dashboard");
                }
                else
                {
                    BudgetingController.InitialiseBudgeting(); //to detect changes made in in masterlists
                    if (BudgetingController.lstUsers == null || BudgetingController.lstUsers.Count == 0)
                    {
                        return RedirectToAction("Index", "Budgeting");
                    }
                }


                return View();
            }
        }

        /// <summary>
        ///  // LE Planner authorization - if authorized,return the NTID and load the entire page; else notify the user to contact SmartLab Team for access
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult LEPlanner_Authorise()
        {
            string NTID = LEPlanner_authorise();

            return Json(new { data = NTID }, JsonRequestBehavior.AllowGet);
        }


        public string LEPlanner_authorise()
        {

            string NTID = "";
            connection();
            BudgetingOpenConnection();
            string qry = " Exec [dbo].[LEPlanner_Authorize] '" + User.Identity.Name.Split('\\')[1].Trim() + "' ";

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


        /// <summary>
        /// function to fetch the Request Items made by the Requestor during the year chosen for view
        /// /// <param name="year"></param>
        /// </summary>
        //[HttpGet]
        //public ActionResult GetData(string year)
        //{


        //    using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //    //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //    {
        //        try
        //        {

        //            List<RequestItemsRepoView> viewList = new List<RequestItemsRepoView>();
        //            viewList = GetData1(year);

        //            if (db.Planning_EM_Table.ToList().Find(person => person.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].Trim().ToUpper())) != null ||
        //                db.Planning_EM_Table.ToList().Find(person => person.Proxy_NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].Trim().ToUpper())) != null)
        //                return Json(new
        //                {
        //                   success = true, data = viewList
        //                }, JsonRequestBehavior.AllowGet);
        //            else
        //                return Json(new { success = false, message = "Sorry! Current user is not authorised to request Items in VKM Planner!" }, JsonRequestBehavior.AllowGet);


        //        }
        //        catch (Exception ex)
        //        {
        //            return Json(new { success = true, message = "Unable to load the Item Requests, Please Try again later!" }, JsonRequestBehavior.AllowGet);

        //        }

        //    }
        //}
        public ActionResult GetData(string year)
        {
            try
            {

                List<RequestItemsRepoView> viewList = new List<RequestItemsRepoView>();
                viewList = GetData1(year);

                return Json(new { success = true, data = viewList }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { success = true, message = "Unable to load the Item Requests, Please Try again later!" }, JsonRequestBehavior.AllowGet);

            }
        }



        public List<RequestItemsRepoView> GetData1(string year)
        {

            List<RequestItemsRepoView> viewList = new List<RequestItemsRepoView>();
            try
            {

                using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

                //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

                {

                    System.Security.Principal.IPrincipal User = System.Web.HttpContext.Current.User;
                    var PresentUserNT1 = string.Empty;
                    var PresentUserNT2 = string.Empty;

                    //if (Convert.ToInt32(year) != DateTime.Now.Year + 1)
                    //{
                    //    DataTable dt = new DataTable();
                    //    connection();
                    //    BudgetingOpenConnection();
                    //     string Query = " Exec [dbo].[GetReqItemsList] '" + year + "', '" + User.Identity.Name.Split('\\')[1].ToUpper()  + "' ";
                    //    SqlCommand cmd = new SqlCommand(Query, budgetingcon);
                    //    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    //    da.Fill(dt);
                    //    BudgetingCloseConnection();

                    //    foreach (DataRow item in dt.Rows)
                    //    {
                    //        try
                    //        {

                    //                RequestItemsRepoView ritem = new RequestItemsRepoView();

                    //                ritem.VKM_Year = Convert.ToInt32(item["VKM_Year"]);
                    //                if(item["UpdatedAt"].ToString() != "")
                    //                    ritem.UpdatedAt = Convert.ToDateTime(item["UpdatedAt"]);
                    //                ritem.Category =  Convert.ToInt32(item["Category"]);
                    //                ritem.Comments = item["Comments"].ToString();
                    //                ritem.Project = item["Project"].ToString();
                    //                if (item["ActualAvailableQuantity"].ToString() == "")
                    //                    ritem.ActualAvailableQuantity = "NA";
                    //                else
                    //                    ritem.ActualAvailableQuantity = item["ActualAvailableQuantity"].ToString();

                    //                ritem.Cost_Element = Convert.ToInt32(item["CostElement"]);
                    //                ritem.BU = Convert.ToInt32(item["BU"]);

                    //                ritem.DEPT = Convert.ToInt32(item["DEPT"]);
                    //                ritem.Group = Convert.ToInt32(item["Group"]);
                    //                ritem.Item_Name = Convert.ToInt32(item["ItemName"]);
                    //                ritem.OEM = Convert.ToInt32(item["OEM"]);
                    //                ritem.Required_Quantity = Convert.ToInt32(item["ReqQuantity"]);
                    //                ritem.RequestID = Convert.ToInt32(item["RequestID"]);

                    //                ritem.Requestor = item["RequestorNT"].ToString();
                    //                ritem.Total_Price = Convert.ToDecimal(item["TotalPrice"]);
                    //                ritem.Unit_Price = Convert.ToDecimal(item["UnitPrice"]);
                    //                ritem.ApprovalHoE = Convert.ToBoolean(item["ApprovalDH"]);
                    //                ritem.ApprovalSH = Convert.ToBoolean(item["ApprovalSH"]);
                    //                ritem.ApprovedHoE = Convert.ToBoolean(item["ApprovedDH"]);
                    //                ritem.ApprovedSH = Convert.ToBoolean(item["ApprovedSH"]);
                    //                if(item["IsCancelled"].ToString() != "")
                    //                    ritem.isCancelled = Convert.ToInt32(item["IsCancelled"]);

                    //                ritem.Reviewer_1 = item["DHNT"].ToString();
                    //                ritem.Reviewer_2 = item["SHNT"].ToString();

                    //                ritem.RequestDate = item["RequestDate"].ToString() != "" ?    ((DateTime)item["RequestDate"]).ToString("yyyy-MM-dd") : string.Empty;
                    //                ritem.SubmitDate =   item["SubmitDate"].ToString() != "" ?  ((DateTime)item["SubmitDate"]).ToString("yyyy-MM-dd") : string.Empty;
                    //                ritem.Review1_Date =  item["DHAppDate"].ToString() != "" ? ((DateTime)item["DHAppDate"]).ToString("yyyy-MM-dd") : string.Empty;
                    //                ritem.Review2_Date =  item["SHAppDate"].ToString() != "" ? ((DateTime)item["SHAppDate"]).ToString("yyyy-MM-dd") : string.Empty;


                    //                if (item["OrderStatus"].ToString() != "")
                    //                {
                    //                    ritem.OrderStatus = Convert.ToInt32(item["OrderStatus"]);

                    //                }
                    //                else
                    //                {
                    //                    ritem.OrderStatus = null;


                    //                }
                    //                if (item["Project"].ToString() == "")
                    //                    ritem.Project = string.Empty;
                    //                else
                    //                    ritem.Project = item["Project"].ToString();

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


                    //                viewList.Add(ritem);

                    //        }
                    //        catch (Exception ex)
                    //        {

                    //        }



                    //    }

                    //}
                    //else
                    //{
                    //if (db.Planning_EM_Table.ToList().FindAll(xi => xi.NTID == User.Identity.Name.Split('\\')[1].ToUpper()).Count() != 0) //Present user is EM
                    //{
                    //    PresentUserNT1 = User.Identity.Name.Split('\\')[1].ToUpper();
                    //    //Get his Proxy Name if Proxy Present
                    //    if (db.Planning_EM_Table.ToList().Find(xi => xi.NTID == User.Identity.Name.Split('\\')[1].ToUpper()).Proxy_NTID != null)
                    //    {
                    //        PresentUserNT2 = db.Planning_EM_Table.ToList().Find(xi => xi.NTID == User.Identity.Name.Split('\\')[1].ToUpper()).Proxy_NTID.Trim();

                    //    }
                    //}
                    //else if (db.Planning_EM_Table.ToList().FindAll(xi => xi.Proxy_NTID == User.Identity.Name.Split('\\')[1].ToUpper()).Count() != 0) //Present user is Proxy
                    //{
                    //    PresentUserNT1 = User.Identity.Name.Split('\\')[1].ToUpper();
                    //    //Get his EM Name 
                    //    PresentUserNT2 = db.Planning_EM_Table.ToList().Find(xi => xi.Proxy_NTID == User.Identity.Name.Split('\\')[1].ToUpper()).NTID.Trim();

                    //}
                    //else
                    //    PresentUserNT1 = User.Identity.Name.Split('\\')[1].ToUpper();


                    //List<RequestItems_Table> reqList = new List<RequestItems_Table>();
                    //reqList = db.RequestItems_Table.ToList<RequestItems_Table>().FindAll(x => x.CostElement != "0" && x.CostElement != null && x.Category != null).OrderBy(item => item.ApprovalDH == true).ToList()/*.FindAll(x=>x.ApprovalDH == false)*/;
                    //db.Database.CommandTimeout = 500;
                    //string presentUserDept = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).Department;
                    //string is_CCXC = string.Empty; //For Clear CC XC Segregation
                    //if (presentUserDept.Contains("XC"))
                    //    is_CCXC = "XC";
                    //else
                    //    is_CCXC = "CC";

                    DataTable dt = new DataTable();
                    connection();
                    BudgetingOpenConnection();
                    string Query = " Exec [dbo].[GetReqItemsList] '" + year + "', '" + User.Identity.Name.Split('\\')[1].ToUpper() + "' ";
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

                            ritem.Category = Convert.ToInt32(item["Category"]);
                            ritem.Comments = item["Comments"].ToString();
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
                            ritem.RequestID = Convert.ToInt32(item["RequestID"]);
                            ritem.Requestor = item["RequestorNT"].ToString();
                            ritem.Total_Price = Convert.ToDecimal(item["TotalPrice"]);
                            ritem.Reviewed_Cost = Convert.ToDecimal(item["ApprCost"]);
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


                            if (item["OrderStatus"].ToString().Trim() != "")
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
                            

                            viewList.Add(ritem);

                        }
                        catch (Exception ex)
                        {
                            WriteLog("Error - GetData -Stored Proc - GetReqItemsList : NTID - " + User.Identity.Name.Split('\\')[1].Trim().ToUpper() + ", ErrorMsg : " + ex + ", At - " + DateTime.Now);
                        }



                    }


                    //foreach (RequestItems_Table item in reqList)
                    //{
                    //    try
                    //    {
                    //        string NTRequestor = string.Empty;
                    //        if (BudgetingController.lstUsers.FirstOrDefault(user => user.EmployeeName.Trim().Equals(item.RequestorNT.Trim())) != null)
                    //            NTRequestor = BudgetingController.lstUsers.FirstOrDefault(user => user.EmployeeName.Trim().Equals(item.RequestorNT.Trim())).NTID.Trim().ToUpper();
                    //        else
                    //            NTRequestor = BudgetingController.lstUsers_2020.FirstOrDefault(user => user.EmployeeName.Trim().Equals(item.RequestorNT.Trim())).NTID.Trim().ToUpper();

                    //        if (NTRequestor == PresentUserNT1 || NTRequestor == PresentUserNT2)
                    //        {
                    //            RequestItemsRepoView ritem = new RequestItemsRepoView();

                    //            ritem.VKM_Year = int.Parse(item.VKM_Year != null ? item.VKM_Year.ToString() : "0");
                    //            ritem.UpdatedAt = item.UpdatedAt;
                    //            ritem.Category = int.Parse(item.Category);
                    //            ritem.Comments = item.Comments;
                    //            ritem.Project = item.Project;
                    //            if (item.ActualAvailableQuantity == null || item.ActualAvailableQuantity.Trim() == string.Empty)
                    //                ritem.ActualAvailableQuantity = "NA";
                    //            else
                    //                ritem.ActualAvailableQuantity = item.ActualAvailableQuantity;

                    //            ritem.Cost_Element = int.Parse(item.CostElement);
                    //            ritem.BU = int.Parse(item.BU);

                    //            ritem.DEPT = int.Parse(item.DEPT);
                    //            ritem.Group = int.Parse(item.Group);
                    //            ritem.Item_Name = int.Parse(item.ItemName);
                    //            ritem.OEM = int.Parse(item.OEM);
                    //            ritem.Required_Quantity = item.ReqQuantity;
                    //            ritem.RequestID = item.RequestID;

                    //            ritem.Requestor = item.RequestorNT;
                    //            ritem.Total_Price = item.TotalPrice;
                    //            ritem.Unit_Price = item.UnitPrice;
                    //            ritem.ApprovalHoE = item.ApprovalDH;
                    //            ritem.ApprovalSH = item.ApprovalSH;
                    //            ritem.ApprovedHoE = item.ApprovedDH;
                    //            ritem.ApprovedSH = item.ApprovedSH;
                    //            if (item.isCancelled != null)
                    //            {
                    //                ritem.isCancelled = (int)item.isCancelled;
                    //            }


                    //            ritem.Total_Price = item.TotalPrice;
                    //            ritem.Reviewer_1 = item.DHNT;
                    //            ritem.Reviewer_2 = item.SHNT;

                    //            ritem.RequestDate = item.RequestDate != null ? ((DateTime)item.RequestDate).ToString("yyyy-MM-dd") : string.Empty;
                    //            ritem.SubmitDate = item.SubmitDate != null ? ((DateTime)item.SubmitDate).ToString("yyyy-MM-dd") : string.Empty;
                    //            ritem.Review1_Date = item.DHAppDate != null ? ((DateTime)item.DHAppDate).ToString("yyyy-MM-dd") : string.Empty;
                    //            ritem.Review2_Date = item.SHAppDate != null ? ((DateTime)item.SHAppDate).ToString("yyyy-MM-dd") : string.Empty;


                    //            if (item.OrderStatus != null && item.OrderStatus.Trim() != "")
                    //            {
                    //                ritem.OrderStatus = int.Parse(item.OrderStatus);

                    //            }
                    //            else
                    //            {
                    //                ritem.OrderStatus = null;


                    //            }
                    //            if (item.Project == null)
                    //                ritem.Project = string.Empty;
                    //            else
                    //                ritem.Project = item.Project;

                    //            //Checking Request Status
                    //            if (ritem.ApprovalHoE == true && ritem.ApprovalSH == false)
                    //            {
                    //                ritem.Request_Status = "In Review with HoE";
                    //            }
                    //            else if (ritem.ApprovalHoE == true && ritem.ApprovalSH == true && ritem.ApprovedSH == false)
                    //            {
                    //                ritem.Request_Status = "In Review with VKM SPOC";
                    //            }
                    //            else if (ritem.ApprovalHoE == true && ritem.ApprovalSH == true && ritem.ApprovedSH == true)
                    //            {
                    //                ritem.Request_Status = "Reviewed by VKM SPOC";
                    //            }
                    //            else if (ritem.ApprovalHoE == false && ritem.ApprovalSH == false && ritem.isCancelled == 1)
                    //            {
                    //                ritem.Request_Status = "SentBack by HoE";
                    //            }
                    //            else if (ritem.ApprovalHoE == false && ritem.ApprovalSH == false && ritem.isCancelled == 2)
                    //            {
                    //                ritem.Request_Status = "SentBack by VKM SPOC";
                    //            }
                    //            else
                    //            {
                    //                ritem.Request_Status = "In Requestor's Queue";
                    //            }


                    //            viewList.Add(ritem);
                    //        }
                    //    }
                    //    catch (Exception ex)
                    //    {

                    //    }

                    //    //y.Sort();

                    //    //for (int i = 0; i < x.Count(); i++)  
                    //    //{
                    //    //    if(x[i] == y[i])
                    //    //    {
                    //    //        ;
                    //    //    }
                    //    //    if (x[i] != y[i])
                    //    //    {
                    //    //        isPresentUser1 = false;
                    //    //        break;

                    //    //    }

                    //    //}
                    //    //if (z.Count != 0)
                    //    //{
                    //    //    for (int j = 0; j < z.Count(); j++)
                    //    //    {
                    //    //        if (z[j] != y[j])
                    //    //        {
                    //    //            isPresentUser2 = false;
                    //    //            break;
                    //    //        }
                    //    //    }
                    //    //}
                    //    //if (z.Count !=0 && isPresentUser1 == false && isPresentUser2 == false)
                    //    //{
                    //    //    isPresentUser1 = true;
                    //    //    isPresentUser2 = true;
                    //    //    continue;
                    //    //}
                    //    //else if (z.Count != 0 && isPresentUser1 == true && isPresentUser2 == false)
                    //    //{
                    //    //    isPresentUser2 = true;
                    //    //}
                    //    //else if (z.Count != 0 && isPresentUser1 == false && isPresentUser2 == true)
                    //    //{
                    //    //    isPresentUser1 = true;
                    //    //}
                    //    //else if(z.Count == 0)
                    //    //{
                    //    //    if(isPresentUser1 == false)
                    //    //    {
                    //    //        isPresentUser1 = true;
                    //    //        continue;
                    //    //    }
                    //    //}



                    //}

                    //}
                    //WriteLog("GetData - BudgetingRequest completed : NTID - " + User.Identity.Name.Split('\\')[1].Trim().ToUpper() + " At - " + DateTime.Now);
                    return viewList;//.FindAll(xi => xi.VKM_Year.ToString().Contains(year));/*.FindAll(x => x.ApprovalHoE == false)*/;


                }




            }
            catch (Exception ex)
            {
                WriteLog("Error - GetData : NTID - " + User.Identity.Name.Split('\\')[1].Trim().ToUpper() + ", ErrorMsg : " + ex + ", At - " + DateTime.Now);

                return viewList.FindAll(xi => xi.VKM_Year.ToString().Contains(year));/*.FindAll(x => x.ApprovalHoE == false)*/;

            }

        }


        /// <summary>
        /// function to get BU summary table
        /// </summary>
        /// <returns></returns>
        //public ActionResult GetBUSummaryData(string year)
        //{

        //    using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //    //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //    {
        //        System.Security.Principal.IPrincipal User = System.Web.HttpContext.Current.User;

        //        string presentUserName = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).EmployeeName;
        //        string presentUserDept = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).Department;

        //        List<RequestItems_Table> reqList = new List<RequestItems_Table>();
        //        reqList = db.RequestItems_Table.ToList<RequestItems_Table>().FindAll(x => x.VKM_Year.ToString().Contains(year));
        //        List<SummaryView> viewList = new List<SummaryView>();

        //        string message = String.Empty;
        //        decimal AS_MAE_Totals = 0, AS_NMAE_Totals = 0, AS_SoftwareTotals = 0;
        //        decimal OSS_MAE_Totals = 0, OSS_NMAE_Totals = 0, OSS_SoftwareTotals = 0;
        //        decimal DA_MAE_Totals = 0, DA_NMAE_Totals = 0, DA_SoftwareTotals = 0;
        //        decimal AD_MAE_Totals = 0, AD_NMAE_Totals = 0, AD_SoftwareTotals = 0;
        //        decimal TwoWP_MAE_Totals = 0, TwoWP_NMAE_Totals = 0, TwoWP_SoftwareTotals = 0;
        //        if (presentUserDept.Contains("CC"))
        //        {
        //            foreach (RequestItems_Table item in reqList.FindAll(x => x.BU.Trim().Equals(BudgetingController.lstBUs.Find(bu => bu.BU.Trim().Equals("AS")).ID.ToString())).FindAll(x => x.RequestorNT.Trim().Contains(presentUserName)))
        //            {
        //                switch (BudgetingController.lstCostElement.Find(cost => cost.ID.Equals(Int32.Parse(item.CostElement.Trim()))).CostElement)
        //                {
        //                    case "MAE": //MAE
        //                        {

        //                            AS_MAE_Totals += (decimal)item.TotalPrice;

        //                            break;
        //                        }
        //                    case "Non-MAE": //NON-MAE
        //                        {

        //                            AS_NMAE_Totals += (decimal)item.TotalPrice;

        //                            break;
        //                        }
        //                    case "Software": //Software
        //                        {

        //                            AS_SoftwareTotals += (decimal)item.TotalPrice;

        //                            break;
        //                        }
        //                    default:
        //                        continue;
        //                }
        //            }
        //            foreach (RequestItems_Table item in reqList.FindAll(x => x.BU.Trim().Equals(BudgetingController.lstBUs.Find(bu => bu.BU.Trim().Equals("OSS")).ID.ToString())).FindAll(x => x.RequestorNT.Trim().Contains(presentUserName)))
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
        //            foreach (RequestItems_Table item in reqList.FindAll(x => x.BU.Trim().Equals(BudgetingController.lstBUs.Find(bu => bu.BU.Trim().Equals("2WP")).ID.ToString())).FindAll(x => x.RequestorNT.Trim().Contains(presentUserName)).FindAll(dpt => BudgetingController.lstDEPTs.Find(x => x.ID.ToString().Contains(dpt.DEPT)).DEPT.Contains("CC")))
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
        //            foreach (RequestItems_Table item in reqList.FindAll(x => x.BU.Trim().Equals(BudgetingController.lstBUs.Find(bu => bu.BU.Trim().Equals("DA")).ID.ToString())).FindAll(x => x.RequestorNT.Trim().Contains(presentUserName)))
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
        //            foreach (RequestItems_Table item in reqList.FindAll(x => x.BU.Trim().Equals(BudgetingController.lstBUs.Find(bu => bu.BU.Trim().Equals("AD")).ID.ToString())).FindAll(x => x.RequestorNT.Trim().Contains(presentUserName)))
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
        //            foreach (RequestItems_Table item in reqList.FindAll(x => x.BU.Trim().Equals(BudgetingController.lstBUs.Find(bu => bu.BU.Trim().Equals("2WP")).ID.ToString())).FindAll(x => x.RequestorNT.Trim().Contains(presentUserName)).FindAll(dpt => BudgetingController.lstDEPTs.Find(x => x.ID.ToString().Contains(dpt.DEPT)).DEPT.Contains("XC")))
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
        //            foreach (RequestItems_Table item in reqList.FindAll(x => x.BU.Trim().Equals(BudgetingController.lstBUs.Find(bu => bu.BU.Trim().Equals("AS")).ID.ToString())).FindAll(x => x.RequestorNT.Trim().Contains(presentUserName)))
        //            {
        //                switch (BudgetingController.lstCostElement.Find(cost => cost.ID.Equals(Int32.Parse(item.CostElement.Trim()))).CostElement)
        //                {
        //                    case "MAE": //MAE
        //                        {

        //                            AS_MAE_Totals += (decimal)item.TotalPrice;

        //                            break;
        //                        }
        //                    case "Non-MAE": //NON-MAE
        //                        {

        //                            AS_NMAE_Totals += (decimal)item.TotalPrice;

        //                            break;
        //                        }
        //                    case "Software": //Software
        //                        {

        //                            AS_SoftwareTotals += (decimal)item.TotalPrice;

        //                            break;
        //                        }
        //                    default:
        //                        continue;
        //                }
        //            }
        //            foreach (RequestItems_Table item in reqList.FindAll(x => x.BU.Trim().Equals(BudgetingController.lstBUs.Find(bu => bu.BU.Trim().Equals("OSS")).ID.ToString())).FindAll(x => x.RequestorNT.Trim().Contains(presentUserName)))
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
        //            foreach (RequestItems_Table item in reqList.FindAll(x => x.BU.Trim().Equals(BudgetingController.lstBUs.Find(bu => bu.BU.Trim().Equals("DA")).ID.ToString())).FindAll(x => x.RequestorNT.Trim().Contains(presentUserName)))
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
        //            foreach (RequestItems_Table item in reqList.FindAll(x => x.BU.Trim().Equals(BudgetingController.lstBUs.Find(bu => bu.BU.Trim().Equals("AD")).ID.ToString())).FindAll(x => x.RequestorNT.Trim().Contains(presentUserName)))
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
        //            foreach (RequestItems_Table item in reqList.FindAll(x => x.BU.Trim().Equals(BudgetingController.lstBUs.Find(bu => bu.BU.Trim().Equals("2WP")).ID.ToString())).FindAll(x => x.RequestorNT.Trim().Contains(presentUserName))/*.FindAll(dpt => BudgetingController.lstDEPTs.Find(x => x.ID.ToString().Contains(dpt.DEPT)).DEPT.Contains("XC")))*/)
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

        //        viewList.Add(Category_View);
        //        Category_View = new SummaryView();
        //        Category_View.Category = "Non-MAE";

        //        Category_View.AS = AS_NMAE_Totals.ToString("C0", CultureInfo.CurrentCulture);

        //        Category_View.OSS = OSS_NMAE_Totals.ToString("C0", CultureInfo.CurrentCulture);

        //        //Category_View.DA = DA_NMAE_Totals.ToString("C0", CultureInfo.CurrentCulture);

        //        //Category_View.AD = AD_NMAE_Totals.ToString("C0", CultureInfo.CurrentCulture);

        //        Category_View.Two_Wheeler = TwoWP_NMAE_Totals.ToString("C0", CultureInfo.CurrentCulture);

        //        viewList.Add(Category_View);
        //        Category_View = new SummaryView();
        //        Category_View.Category = "Software";

        //        Category_View.AS = AS_SoftwareTotals.ToString("C0", CultureInfo.CurrentCulture);

        //        Category_View.OSS = OSS_SoftwareTotals.ToString("C0", CultureInfo.CurrentCulture);

        //        //Category_View.DA = DA_SoftwareTotals.ToString("C0", CultureInfo.CurrentCulture);

        //        //Category_View.AD = AD_SoftwareTotals.ToString("C0", CultureInfo.CurrentCulture);

        //        Category_View.Two_Wheeler = TwoWP_SoftwareTotals.ToString("C0", CultureInfo.CurrentCulture);

        //        viewList.Add(Category_View);
        //        Category_View = new SummaryView();
        //        Category_View.Category = "Totals";

        //        Category_View.AS = (AS_MAE_Totals + AS_NMAE_Totals + AS_SoftwareTotals).ToString("C0", CultureInfo.CurrentCulture);

        //        Category_View.OSS = (OSS_MAE_Totals + OSS_NMAE_Totals + OSS_SoftwareTotals).ToString("C0", CultureInfo.CurrentCulture);

        //        //Category_View.DA = (DA_MAE_Totals + DA_NMAE_Totals + DA_SoftwareTotals).ToString("C0", CultureInfo.CurrentCulture);

        //        //Category_View.AD = (AD_MAE_Totals + AD_NMAE_Totals + AD_SoftwareTotals).ToString("C0", CultureInfo.CurrentCulture);

        //        Category_View.Two_Wheeler = (TwoWP_MAE_Totals + TwoWP_NMAE_Totals + TwoWP_SoftwareTotals).ToString("C0", CultureInfo.CurrentCulture);

        //        viewList.Add(Category_View);

        //        if (presentUserDept.Contains("CC"))
        //            message = "CC";
        //        else if (presentUserDept.Contains("XC"))
        //            message = "XC";
        //        else
        //            message = "";


        //        return Json(new { data = viewList, message }, JsonRequestBehavior.AllowGet);
        //    }
        //}


        public ActionResult GetBUSummaryData(string year)
        {
            try
            {
                string presentUserDept = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).Department;
                //presentUserDept = "XC";

                System.Data.DataTable dt = new System.Data.DataTable();
                dt = BUSummaryData(year);

                //if (presentUserDept.Contains("XC"))
                //    dt = XC_BUSummary(year);
                //else
                //    dt = CC_BUSummary(year);


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
            catch (Exception ex)
            {
                return Json(new { data = "" }, JsonRequestBehavior.AllowGet);
            }


        }


        public System.Data.DataTable BUSummaryData(string year)
        {
            try
            {

                using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

                {
                    System.Security.Principal.IPrincipal User = System.Web.HttpContext.Current.User;

                    //List<RequestItems_Table> reqList = db.RequestItems_Table.Where(x => x.VKM_Year.ToString().Contains(year)).ToList<RequestItems_Table>();
                    List<BUSummary> viewList = new List<BUSummary>();
                    List<BUSummary> viewList1 = new List<BUSummary>();
                    //List<string> userSectionDeptList = new List<string>();

                    //getting NTID of USer
                    string presentUserNTID = User.Identity.Name.Split('\\')[1].ToUpper().Trim();
                    //string presentUserNTID = "NHH6KOR";
                    //year = "2023";
                    //List<string> h = new List<string>();

                    //DataTable dtproxy = new DataTable();
                    //if (db.Planning_EM_Table.ToList().FindAll(xi => xi.NTID == User.Identity.Name.Split('\\')[1].ToUpper()).Count() != 0) //Present user is HOE
                    //{
                    //    //x = presentUserName.TrimEnd().TrimStart().Split(' ').ToList();

                    //    h.Add(presentUserNTID.Trim().ToString());

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

                    //}
                    //if (db.Planning_EM_Table.ToList().FindAll(xi => xi.Proxy_NTID == User.Identity.Name.Split('\\')[1].ToUpper()).Count() != 0)
                    //{
                    //    //x = db.Planning_HOE_Table.ToList().Find(xi => xi.Proxy_NTID == User.Identity.Name.Split('\\')[1].ToUpper()).HOE_FullName.TrimEnd().TrimStart().Split(' ').ToList();

                    //    connection();
                    //    BudgetingOpenConnection();
                    //    string qry = " Select HOE_NTID from Planning_HOE_Table where Proxy_NTID = '" + User.Identity.Name.Split('\\')[1].ToUpper() + "' ";
                    //    SqlCommand cmd = new SqlCommand(qry, budgetingcon);
                    //    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    //    da.Fill(dtproxy);
                    //    BudgetingCloseConnection();

                    //    if (dtproxy.Rows.Count > 0)
                    //    {
                    //        for (int i = 0; i < dtproxy.Rows.Count; i++)
                    //        {
                    //            h.Add(dtproxy.Rows[i]["HOE_NTID"].ToString());
                    //        }
                    //    }


                    //}
                    //if (h.Count() == 0)
                    //    //x = presentUserName.TrimEnd().TrimStart().Split(' ').ToList();
                    //    h.Add(presentUserNTID.Trim().ToString());

                    decimal OMAE_Totals = 0, ONMAE_Totals = 0, OSoftwareTotals = 0;
                    DataTable dt1 = new DataTable();
                    //if (h.Count > 0)
                    //{
                    connection();
                    BudgetingOpenConnection();

                    //for (int i = 0; i < h.Count; i++)
                    //{
                    dt1 = new DataTable();
                    string Query = " Exec [dbo].[LEPlanner_BUSummary] '" + presentUserNTID + "', '" + year + "'";
                    SqlCommand cmd = new SqlCommand(Query, budgetingcon);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt1);

                    foreach (DataRow item in dt1.Rows)
                    {
                        BUSummary tempobj = new BUSummary();
                        tempobj.BuName = item["BuName"].ToString();
                        tempobj.MAE_Totals = item["MAE_Totals"].ToString() != "" ? Convert.ToDecimal(item["MAE_Totals"]) : 0;
                        OMAE_Totals += tempobj.MAE_Totals;
                        tempobj.NMAE_Totals = item["NMAE_Totals"].ToString() != "" ? Convert.ToDecimal(item["NMAE_Totals"]) : 0;
                        ONMAE_Totals += tempobj.NMAE_Totals;
                        tempobj.Software_Totals = item["Software_Totals"].ToString() != "" ? Convert.ToDecimal(item["Software_Totals"]) : 0;
                        OSoftwareTotals += tempobj.Software_Totals;
                        tempobj.Overall_Totals = tempobj.MAE_Totals + tempobj.NMAE_Totals + tempobj.Software_Totals;
                        viewList1.Add(tempobj);
                    }
                    //}


                    viewList = viewList1
                    .GroupBy(l => l.BuName)
                    .Select(cl => new BUSummary
                    {
                        BuName = cl.FirstOrDefault().BuName,
                        MAE_Totals = cl.Sum(c => c.MAE_Totals),
                        NMAE_Totals = cl.Sum(c => c.NMAE_Totals),
                        Software_Totals = cl.Sum(c => c.Software_Totals),
                        Overall_Totals = cl.Sum(c => c.Overall_Totals),
                    }).ToList();


                    //    userSectionDeptList = db.SPOTONData_Table_2022.ToList<SPOTONData_Table_2022>().FindAll(x => x.Section == BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(presentUserNTID.Trim().ToUpper())).Section).Select(item => item.Department).Distinct().ToList();
                    BudgetingCloseConnection();
                    //}


                    ///NEW CODE TO GET DEPT SUMMARY


                    System.Data.DataTable dt = new System.Data.DataTable();
                    dt.Columns.Add("CostElement", typeof(string));
                    dt.Columns.Add("Totals", typeof(string));
                    //foreach (DataRow item in dt1.Rows)
                    foreach (var item in viewList)
                    {

                        dt.Columns.Add(item.BuName.ToString(), typeof(string));
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


        //public System.Data.DataTable CC_BUSummary(string year)
        //{
        //    System.Data.DataTable dt = new System.Data.DataTable();
        //    try
        //    {
        //        string presentUserName = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).EmployeeName; //EM or Proxy
        //        string presentUserName1 = string.Empty;

        //        using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //        //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //        {

        //            if (db.Planning_EM_Table.ToList().FindAll(xi => xi.NTID == User.Identity.Name.Split('\\')[1].ToUpper()).Count() != 0) //Present user is EM
        //            {
        //                //Get his Proxy Name if Proxy Present
        //                if (db.Planning_EM_Table.ToList().Find(xi => xi.NTID == User.Identity.Name.Split('\\')[1].ToUpper()).Proxy_NTID != null)
        //                {
        //                    presentUserName1 = db.Planning_EM_Table.ToList().Find(xi => xi.NTID == User.Identity.Name.Split('\\')[1].ToUpper()).Proxy_FullName.Trim();

        //                }
        //            }
        //            else if (db.Planning_EM_Table.ToList().FindAll(xi => xi.Proxy_NTID == User.Identity.Name.Split('\\')[1].ToUpper()).Count() != 0) //Present user is Proxy
        //            {
        //                //Get his EM Name 
        //                presentUserName1 = db.Planning_EM_Table.ToList().Find(xi => xi.Proxy_NTID == User.Identity.Name.Split('\\')[1].ToUpper()).FullName.Trim();

        //            }


        // List<BUSummary_CC> viewList = new List<BUSummary_CC>();


        //            decimal AS_MAE_Totals = 0, AS_NMAE_Totals = 0, AS_SoftwareTotals = 0;
        //            decimal OSS_MAE_Totals = 0, OSS_NMAE_Totals = 0, OSS_SoftwareTotals = 0;
        //            decimal TwoWP_MAE_Totals = 0, TwoWP_NMAE_Totals = 0, TwoWP_SoftwareTotals = 0;

        //            List<RequestItems_Table> reqList = db.RequestItems_Table.ToList().FindAll(x => x.CostElement != null && x.CostElement != "0").FindAll(x => x.VKM_Year.ToString().Contains(year)).FindAll(dpt => !BudgetingController.lstDEPTs.Find(x => x.ID.ToString().Contains(dpt.DEPT)).DEPT.Contains("XC"));
        //            if (presentUserName1 != null && presentUserName1 != string.Empty)
        //                reqList = reqList.FindAll(x => x.RequestorNT.Trim().Contains(presentUserName.Trim()) || x.RequestorNT.Trim().Contains(presentUserName1.Trim()));
        //            else
        //                reqList = reqList.FindAll(x => x.RequestorNT.Trim().Contains(presentUserName.Trim()));


        //            foreach (RequestItems_Table item in reqList.FindAll(x => x.BU.Equals(BudgetingController.lstBUs.Find(bu => bu.BU.Trim().Equals("MB")).ID.ToString())))
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
        //            foreach (RequestItems_Table item in reqList.FindAll(x => x.BU.Trim().Equals(BudgetingController.lstBUs.Find(bu => bu.BU.Equals("OSS")).ID.ToString())))
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
        //            foreach (RequestItems_Table item in reqList.FindAll(x => x.BU.Trim().Equals(BudgetingController.lstBUs.Find(bu => bu.BU.Equals("2WP")).ID.ToString())))
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

        //            BUSummary_CC tempobj = new BUSummary_CC();
        //            tempobj.vkmyear = DateTime.Now.Year.ToString();
        //            tempobj.AS_MAE_Totals = AS_MAE_Totals;
        //            tempobj.AS_NMAE_Totals = AS_NMAE_Totals;
        //            tempobj.AS_Software_Totals = AS_SoftwareTotals;
        //            tempobj.AS_Overall_Totals = AS_MAE_Totals + AS_NMAE_Totals + AS_SoftwareTotals;
        //            tempobj.OSS_MAE_Totals = OSS_MAE_Totals;
        //            tempobj.OSS_NMAE_Totals = OSS_NMAE_Totals;
        //            tempobj.OSS_Software_Totals = OSS_SoftwareTotals;
        //            tempobj.OSS_Overall_Totals = OSS_MAE_Totals + OSS_NMAE_Totals + OSS_SoftwareTotals;
        //            tempobj.TwoWP_MAE_Totals = TwoWP_MAE_Totals;
        //            tempobj.TwoWP_NMAE_Totals = TwoWP_NMAE_Totals;
        //            tempobj.TwoWP_Software_Totals = TwoWP_SoftwareTotals;
        //            tempobj.TwoWP_Overall_Totals = TwoWP_MAE_Totals + TwoWP_NMAE_Totals + TwoWP_SoftwareTotals;

        //            viewList.Add(tempobj);



        //            dt.Columns.Add("Cost Element", typeof(string)); /// Planned + (year variable) / Utilized  + (year variable)

        //            dt.Columns.Add("MB", typeof(string)); //add vkm text to yr
        //            dt.Columns.Add("OSS", typeof(string));
        //            dt.Columns.Add("2WP", typeof(string));
        //            dt.Columns.Add("Totals", typeof(string));




        //            DataRow dr = dt.NewRow();
        //            dr[0] = "MAE";
        //            int rcnt = 1;


        //            foreach (var info in viewList)
        //            {
        //                dr[rcnt++] = info.AS_MAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //                dr[rcnt++] = info.OSS_MAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //                dr[rcnt++] = info.TwoWP_MAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //                dr[rcnt++] = (info.AS_MAE_Totals + info.OSS_MAE_Totals + info.TwoWP_MAE_Totals).ToString("C0", CultureInfo.CurrentCulture);
        //            }
        //            dt.Rows.Add(dr);

        //            dr = dt.NewRow();
        //            dr[0] = "Non-MAE";
        //            int r1cnt = 1;
        //            foreach (var info in viewList)
        //            {
        //                dr[r1cnt++] = info.AS_NMAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //                dr[r1cnt++] = info.OSS_NMAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //                dr[r1cnt++] = info.TwoWP_NMAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //                dr[r1cnt++] = (info.AS_NMAE_Totals + info.OSS_NMAE_Totals + info.TwoWP_NMAE_Totals).ToString("C0", CultureInfo.CurrentCulture);
        //            }

        //            dt.Rows.Add(dr);

        //            dr = dt.NewRow();
        //            dr[0] = "Software";
        //            int r2cnt = 1;
        //            foreach (var info in viewList)
        //            {
        //                dr[r2cnt++] = info.AS_Software_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //                dr[r2cnt++] = info.OSS_Software_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //                dr[r2cnt++] = info.TwoWP_Software_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //                dr[r2cnt++] = (info.AS_Software_Totals + info.OSS_Software_Totals + info.TwoWP_Software_Totals).ToString("C0", CultureInfo.CurrentCulture);
        //            }

        //            dt.Rows.Add(dr);

        //            dr = dt.NewRow();
        //            dr[0] = "Totals";
        //            int r3cnt = 1;

        //            foreach (var info in viewList)
        //            {
        //                dr[r3cnt++] = (info.AS_Overall_Totals).ToString("C0", CultureInfo.CurrentCulture);
        //                dr[r3cnt++] = (info.OSS_Overall_Totals).ToString("C0", CultureInfo.CurrentCulture);
        //                dr[r3cnt++] = (info.TwoWP_Overall_Totals).ToString("C0", CultureInfo.CurrentCulture);
        //                dr[r3cnt++] = (info.AS_Overall_Totals + info.OSS_Overall_Totals + info.TwoWP_Overall_Totals).ToString("C0", CultureInfo.CurrentCulture);

        //            }

        //            dt.Rows.Add(dr);


        //        }
        //        return dt;
        //    }
        //    catch(Exception ex)
        //    {
        //        WriteLog("Error - CC_BUSummary : NTID - " + User.Identity.Name.Split('\\')[1].Trim().ToUpper() + ", ErrorMsg : " + ex + ", At - " + DateTime.Now);

        //        return dt;
        //    }
        //}

        //public System.Data.DataTable XC_BUSummary(string year)
        //{
        //    System.Data.DataTable dt = new System.Data.DataTable();
        //    try
        //    {


        //        //string presentUserDept = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).Department;
        //        string presentUserName = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).EmployeeName;
        //        string presentUserName1 = string.Empty;

        //        using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //        //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //        {
        //            //var EMlist = db.Planning_EM_Table.ToList();
        //            //if (EMlist.FindAll(x => x.Proxy_NTID != null).Count != 0)
        //            //{
        //            //    if (EMlist.FindAll(xi => xi.Proxy_NTID == User.Identity.Name.Split('\\')[1].ToUpper()).Count() != 0)
        //            //    {
        //            //        var EM = db.Planning_EM_Table.ToList().Find(xi => xi.Proxy_NTID == User.Identity.Name.Split('\\')[1].ToUpper()).NTID.Trim().ToUpper();
        //            //        presentUserName = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(EM)).EmployeeName;
        //            //    }


        //            //}

        //            if (db.Planning_EM_Table.ToList().FindAll(xi => xi.NTID == User.Identity.Name.Split('\\')[1].ToUpper()).Count() != 0) //Present user is EM
        //            {
        //                //Get his Proxy Name if Proxy Present
        //                if (db.Planning_EM_Table.ToList().Find(xi => xi.NTID == User.Identity.Name.Split('\\')[1].ToUpper()).Proxy_NTID != null)
        //                {
        //                    presentUserName1 = db.Planning_EM_Table.ToList().Find(xi => xi.NTID == User.Identity.Name.Split('\\')[1].ToUpper()).Proxy_FullName.Trim();

        //                }
        //            }
        //            else if (db.Planning_EM_Table.ToList().FindAll(xi => xi.Proxy_NTID == User.Identity.Name.Split('\\')[1].ToUpper()).Count() != 0) //Present user is Proxy
        //            {
        //                //Get his EM Name 
        //                presentUserName1 = db.Planning_EM_Table.ToList().Find(xi => xi.Proxy_NTID == User.Identity.Name.Split('\\')[1].ToUpper()).FullName.Trim();

        //            }



        //            List<BUSummary_XC> viewList = new List<BUSummary_XC>();

        //            decimal DA_MAE_Totals = 0, DA_NMAE_Totals = 0, DA_SoftwareTotals = 0;
        //            decimal AD_MAE_Totals = 0, AD_NMAE_Totals = 0, AD_SoftwareTotals = 0;
        //            decimal TwoWP_MAE_Totals = 0, TwoWP_NMAE_Totals = 0, TwoWP_SoftwareTotals = 0;

        //            List<RequestItems_Table> reqList = db.RequestItems_Table.ToList<RequestItems_Table>().FindAll(x => x.VKM_Year.ToString().Contains(year)).FindAll(dpt => !BudgetingController.lstDEPTs.Find(x => x.ID.ToString().Contains(dpt.DEPT)).DEPT.Contains("XC"));

        //            if (presentUserName1 != null && presentUserName1 != string.Empty)
        //                reqList = reqList.FindAll(x => x.RequestorNT.Trim().Contains(presentUserName.Trim()) || x.RequestorNT.Trim().Contains(presentUserName1.Trim()));
        //            else
        //                reqList = reqList.FindAll(x => x.RequestorNT.Trim().Contains(presentUserName.Trim()));


        //            foreach (RequestItems_Table item in reqList.FindAll(x => x.BU.Equals(BudgetingController.lstBUs.Find(bu => bu.BU.Trim().Equals("DA")).ID.ToString())))
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
        //            foreach (RequestItems_Table item in reqList.FindAll(x => x.BU.Trim().Equals(BudgetingController.lstBUs.Find(bu => bu.BU.Equals("AD")).ID.ToString())))
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
        //            foreach (RequestItems_Table item in reqList.FindAll(x => x.BU.Trim().Equals(BudgetingController.lstBUs.Find(bu => bu.BU.Equals("2WP")).ID.ToString())))
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

        //            BUSummary_XC tempobj = new BUSummary_XC();
        //            tempobj.vkmyear = DateTime.Now.Year.ToString();
        //            tempobj.DA_MAE_Totals = DA_MAE_Totals;
        //            tempobj.DA_NMAE_Totals = DA_NMAE_Totals;
        //            tempobj.DA_Software_Totals = DA_SoftwareTotals;
        //            tempobj.DA_Overall_Totals = DA_MAE_Totals + DA_NMAE_Totals + DA_SoftwareTotals;
        //            tempobj.AD_MAE_Totals = AD_MAE_Totals;
        //            tempobj.AD_NMAE_Totals = AD_NMAE_Totals;
        //            tempobj.AD_Software_Totals = AD_SoftwareTotals;
        //            tempobj.AD_Overall_Totals = AD_MAE_Totals + AD_NMAE_Totals + AD_SoftwareTotals;
        //            tempobj.TwoWP_MAE_Totals = TwoWP_MAE_Totals;
        //            tempobj.TwoWP_NMAE_Totals = TwoWP_NMAE_Totals;
        //            tempobj.TwoWP_Software_Totals = TwoWP_SoftwareTotals;
        //            tempobj.TwoWP_Overall_Totals = TwoWP_MAE_Totals + TwoWP_NMAE_Totals + TwoWP_SoftwareTotals;

        //            viewList.Add(tempobj);




        //            dt.Columns.Add("Cost Element", typeof(string)); /// Planned + (year variable) / Utilized  + (year variable)

        //            dt.Columns.Add("DA", typeof(string)); //add vkm text to yr
        //            dt.Columns.Add("AD", typeof(string));
        //            dt.Columns.Add("2WP", typeof(string));
        //            dt.Columns.Add("Totals", typeof(string));



        //            DataRow dr = dt.NewRow();
        //            dr[0] = "MAE";
        //            int rcnt = 1;

        //            foreach (var info in viewList)
        //            {
        //                dr[rcnt++] = info.DA_MAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //                dr[rcnt++] = info.AD_MAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //                dr[rcnt++] = info.TwoWP_MAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //                dr[rcnt++] = (info.DA_MAE_Totals + info.AD_MAE_Totals + info.TwoWP_MAE_Totals).ToString("C0", CultureInfo.CurrentCulture);
        //            }
        //            dt.Rows.Add(dr);

        //            dr = dt.NewRow();
        //            dr[0] = "Non-MAE";
        //            int r1cnt = 1;
        //            foreach (var info in viewList)
        //            {
        //                dr[r1cnt++] = info.DA_NMAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //                dr[r1cnt++] = info.AD_NMAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //                dr[r1cnt++] = info.TwoWP_NMAE_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //                dr[r1cnt++] = (info.DA_NMAE_Totals + info.AD_NMAE_Totals + info.TwoWP_NMAE_Totals).ToString("C0", CultureInfo.CurrentCulture);
        //            }

        //            dt.Rows.Add(dr);

        //            dr = dt.NewRow();
        //            dr[0] = "Software";
        //            int r2cnt = 1;
        //            foreach (var info in viewList)
        //            {
        //                dr[r2cnt++] = info.DA_Software_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //                dr[r2cnt++] = info.AD_Software_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //                dr[r2cnt++] = info.TwoWP_Software_Totals.ToString("C0", CultureInfo.CurrentCulture);
        //                dr[r2cnt++] = (info.DA_Software_Totals + info.AD_Software_Totals + info.TwoWP_Software_Totals).ToString("C0", CultureInfo.CurrentCulture);
        //            }

        //            dt.Rows.Add(dr);

        //            dr = dt.NewRow();
        //            dr[0] = "Totals";
        //            int r3cnt = 1;

        //            foreach (var info in viewList)
        //            {

        //                dr[r3cnt++] = (info.DA_Overall_Totals).ToString("C0", CultureInfo.CurrentCulture);
        //                dr[r3cnt++] = (info.AD_Overall_Totals).ToString("C0", CultureInfo.CurrentCulture);
        //                dr[r3cnt++] = (info.TwoWP_Overall_Totals).ToString("C0", CultureInfo.CurrentCulture);
        //                dr[r3cnt++] = (info.DA_Overall_Totals + info.AD_Overall_Totals + info.TwoWP_Overall_Totals).ToString("C0", CultureInfo.CurrentCulture);

        //            }

        //            dt.Rows.Add(dr);
        //            return dt;
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        WriteLog("Error - XC_BUSummary : NTID - " + User.Identity.Name.Split('\\')[1].Trim().ToUpper() + ", ErrorMsg : " + ex + ", At - " + DateTime.Now);
        //        return dt;
        //    }
        //}


        /// <summary>
        /// function to generate Dept Summary
        /// </summary>
        /// <returns></returns>
        public ActionResult GetDeptSummaryData(string year)
        {
            BudgetParam t = new BudgetParam();
            try
            {

                using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

                //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

                {

                    System.Security.Principal.IPrincipal User = System.Web.HttpContext.Current.User;
                    string presentUserDept = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).Department;
                    List<RequestItems_Table> reqList = new List<RequestItems_Table>();
                    reqList = db.RequestItems_Table.ToList<RequestItems_Table>().FindAll(x => x.VKM_Year.ToString().Contains(year));
                    List<DeptSummary> viewList = new List<DeptSummary>();
                    string userSectionDeptList;
                    userSectionDeptList = BudgetingController.lstDEPTs.Find(dept => dept.DEPT.Trim().Equals(presentUserDept)).DEPT;

                    //CODE TO GET GROUOSS OF COST ELEMENT
                    IEnumerable<IGrouping<string, RequestItems_Table>> query = reqList.FindAll(item => item.CostElement != null && item.CostElement != "0").GroupBy(item => item.CostElement);
                    decimal OMAE_Totals = 0, ONMAE_Totals = 0, OSoftwareTotals = 0;

                    decimal MAE_Totals = 0, NMAE_Totals = 0, SoftwareTotals = 0;
                    DeptSummary tempobj = new DeptSummary();
                    tempobj.deptName = userSectionDeptList;

                    //CODE TO GET THE TOTALS OF EACH COST ELEMENT
                    foreach (IGrouping<string, RequestItems_Table> CostGroup in query)
                    {

                        // Iterate over each value in the
                        // IGrouping and print the value.
                        if (BudgetingController.lstCostElement.Find(cost => cost.ID.Equals(Int32.Parse(CostGroup.Key))).CostElement == "MAE")
                        {
                            foreach (RequestItems_Table item in CostGroup)
                            {
                                if (BudgetingController.lstDEPTs.Find(departmnt => departmnt.ID.Equals(Int32.Parse(item.DEPT))).DEPT == userSectionDeptList/*dept*/)
                                {

                                    MAE_Totals += (decimal)item.TotalPrice;

                                }
                            }
                        }
                        else if (BudgetingController.lstCostElement.Find(cost => cost.ID.Equals(Int32.Parse(CostGroup.Key))).CostElement == "Non-MAE")
                        {
                            foreach (RequestItems_Table item in CostGroup)
                            {
                                if (BudgetingController.lstDEPTs.Find(departmnt => departmnt.ID.Equals(Int32.Parse(item.DEPT))).DEPT == userSectionDeptList)
                                {

                                    NMAE_Totals += (decimal)item.TotalPrice;

                                }
                            }

                        }
                        else if (BudgetingController.lstCostElement.Find(cost => cost.ID.Equals(Int32.Parse(CostGroup.Key))).CostElement == "Software")
                        {
                            foreach (RequestItems_Table item in CostGroup)
                            {
                                if (BudgetingController.lstDEPTs.Find(departmnt => departmnt.ID.Equals(Int32.Parse(item.DEPT))).DEPT == userSectionDeptList)
                                {

                                    SoftwareTotals += (decimal)item.TotalPrice;

                                }
                            }

                        }
                    }
                    tempobj.MAE_Totals = MAE_Totals;
                    OMAE_Totals += MAE_Totals;
                    tempobj.NMAE_Totals = NMAE_Totals;
                    ONMAE_Totals += NMAE_Totals;
                    tempobj.Software_Totals = SoftwareTotals;
                    OSoftwareTotals += SoftwareTotals;
                    tempobj.Overall_Totals = MAE_Totals + NMAE_Totals + SoftwareTotals;
                    viewList.Add(tempobj);


                    /////NEW CODE TO GET DEPT SUMMARY
                    var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();

                    List<columnsinfo> _col = new List<columnsinfo>();

                    System.Data.DataTable dt = new System.Data.DataTable();
                    dt.Columns.Add("CostElement", typeof(string));
                    dt.Columns.Add(userSectionDeptList, typeof(string));

                    DataRow dr = dt.NewRow();
                    dr[0] = "MAE";
                    dr[1] = OMAE_Totals.ToString("C0", CultureInfo.CurrentCulture);

                    dt.Rows.Add(dr);

                    dr = dt.NewRow();
                    dr[0] = "Non-MAE";
                    dr[1] = ONMAE_Totals.ToString("C0", CultureInfo.CurrentCulture);

                    dt.Rows.Add(dr);

                    dr = dt.NewRow();
                    dr[0] = "Software";
                    dr[1] = OSoftwareTotals.ToString("C0", CultureInfo.CurrentCulture);

                    dt.Rows.Add(dr);

                    dr = dt.NewRow();
                    dr[0] = "Totals";
                    dr[1] = (OMAE_Totals + ONMAE_Totals + OSoftwareTotals).ToString("C0", CultureInfo.CurrentCulture);

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
                    return Json(new { data = t }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                WriteLog("Error - DeptSummary : NTID - " + User.Identity.Name.Split('\\')[1].Trim().ToUpper() + ", ErrorMsg : " + ex + ", At - " + DateTime.Now);
                return Json(new { data = t }, JsonRequestBehavior.AllowGet);
            }
        }
         

        
        /// <summary>
        /// Function to help the export to excel function - commented since dxgrid export is enabled
        /// Path to export - default or input from User
        /// Feedback to user after saving
        /// </summary>
        public ActionResult ExportDataToExcel(string useryear)
        {
            try
            {
                string presentNTID = User.Identity.Name.Split('\\')[1].ToUpper();
                string filename = @"VKM " + useryear + " Request_List_" + DateTime.Now.ToShortDateString() + ".xlsx";

                using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

                //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

                {
                    System.Security.Principal.IPrincipal User = System.Web.HttpContext.Current.User;
                    //Proxy Dept == EM Dept
                    string presentUserDept = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).Department;
                    string is_CCXC = string.Empty;
                    //if (presentUserDept.Contains("XC"))
                    //    is_CCXC = "XC";
                    //else
                    //    is_CCXC = "CC";
                    //string presentUserName = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).EmployeeName;               
                    //var EMlist = db.Planning_EM_Table.ToList();
                    //if (EMlist.FindAll(x => x.Proxy_NTID != null).Count != 0)
                    //{
                    //    if (EMlist.FindAll(xi => xi.Proxy_NTID == User.Identity.Name.Split('\\')[1].ToUpper()).Count() != 0)
                    //    {
                    //        var EM = db.Planning_HOE_Table.ToList().Find(xi => xi.Proxy_NTID == User.Identity.Name.Split('\\')[1].ToUpper()).HOE_NTID.Trim().ToUpper();
                    //        presentUserName = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(EM)).EmployeeName;
                    //        presentUserDept = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(EM)).Department;

                    //    }


                    //}

                    System.Data.DataTable dt = new System.Data.DataTable("Request_List");
                    dt.Columns.AddRange(new DataColumn[24] { new DataColumn("Business Unit"),
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
                                             new DataColumn("Review Quantity",typeof(int)),
                                            new DataColumn("Review Price",typeof(decimal)),
                                            new DataColumn("Comments"),
                                            new DataColumn("Requestor"),
                                            new DataColumn("Request Date"),
                                            new DataColumn("Reviewer 1"),
                                            new DataColumn("Review 1 Date"),
                                            new DataColumn("Reviewer 2"),
                                            new DataColumn("Review 2 Date"),
                                            new DataColumn("Plan Status"),
                                            new DataColumn("Order Status")});
                    if (Convert.ToInt32(useryear) != DateTime.Now.Year + 1)
                    {
                        DataTable dt1 = new DataTable();
                        connection();
                        BudgetingOpenConnection();
                        string Query = " Exec [dbo].[GetReqItemsList] '" + useryear + "', '" + User.Identity.Name.Split('\\')[1].ToUpper() + "' ";
                        SqlCommand cmd = new SqlCommand(Query, budgetingcon);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt1);
                        BudgetingCloseConnection();

                        foreach (DataRow item in dt1.Rows)
                        {
                            var ApprovalDH = item["ApprovalDH"].ToString() != "" ? Convert.ToBoolean(item["ApprovalDH"].ToString()) : false;
                            var ApprovedDH = item["ApprovedDH"].ToString() != "" ? Convert.ToBoolean(item["ApprovedDH"].ToString()) : false;
                            var ApprovalSH = item["ApprovalSH"].ToString() != "" ? Convert.ToBoolean(item["ApprovalSH"].ToString()) : false;
                            var ApprovedSH = item["ApprovedSH"].ToString() != "" ? Convert.ToBoolean(item["ApprovedSH"].ToString()) : false;
                            var isCancelled = item["isCancelled"].ToString() != "" ? Convert.ToInt32(item["isCancelled"].ToString()) : 0;
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
                               Convert.ToInt32(item["ApprQuantity"]),
                               Math.Round(Convert.ToDecimal(item["ApprCost"])),
                               item["Comments"].ToString(),
                               item["RequestorNT"].ToString(),
                               item["RequestDate"].ToString() != "" ? ((DateTime)item["RequestDate"]).ToString("dd-MM-yyyy") : string.Empty,
                               item["DHNT"].ToString(),
                               item["DHAppDate"].ToString() != "" ? ((DateTime)item["DHAppDate"]).ToString("dd-MM-yyyy") : string.Empty,
                               item["SHNT"].ToString(),
                               item["SHAppDate"].ToString() != "" ? ((DateTime)item["SHAppDate"]).ToString("dd-MM-yyyy") : string.Empty,

                               //Checking Request Status
                               //    if (ritem.ApprovalHoE == true && ritem.ApprovalSH == false)
                               //{
                               //    ritem.Request_Status = "In Review with HoE";
                               //}
                               //else if (ritem.ApprovalHoE == true && ritem.ApprovalSH == true && ritem.ApprovedSH == false)
                               //{
                               //    ritem.Request_Status = "In Review with VKM SPOC";
                               //}
                               //else if (ritem.ApprovalHoE == true && ritem.ApprovalSH == true && ritem.ApprovedSH == true)
                               //{
                               //    ritem.Request_Status = "Reviewed by VKM SPOC";
                               //}
                               //else if (ritem.ApprovalHoE == false && ritem.ApprovalSH == false && ritem.isCancelled == 1)
                               //{
                               //    ritem.Request_Status = "SentBack by HoE";
                               //}
                               //else if (ritem.ApprovalHoE == false && ritem.ApprovalSH == false && ritem.isCancelled == 2)
                               //{
                               //    ritem.Request_Status = "SentBack by VKM SPOC";
                               //}
                               //else
                               //{
                               //    ritem.Request_Status = "In Requestor's Queue";

                               //}
                               (ApprovalDH == true && ApprovalSH == false) ? "In Review with HoE" : (ApprovalDH == true && ApprovalSH == true && ApprovedSH == false ? "In Review with VKM SPOC" : (ApprovalDH == true && ApprovalSH == true && ApprovedSH == true ? "Reviewed by VKM SPOC" : (ApprovalDH == false && ApprovalSH == false && isCancelled == 1 ? "SentBack by HoE" : (ApprovalDH == false && ApprovalSH == false && isCancelled == 2 ? "SentBack by VKM SPOC" : "In Requestor's Queue")))),
                               (item["OrderStatus"].ToString().Trim() != "" && item["OrderStatus"].ToString().Trim() != "0") ? BudgetingController.lstOrderStatus.Find(cost => cost.ID.ToString().Equals(item["OrderStatus"].ToString().Trim())).OrderStatus : string.Empty
                               );

                        }
                    }
                    else
                    {
                        string PresentUserDept_RequestTable = BudgetingController.lstDEPTs.Find(dept => dept.DEPT.Equals(presentUserDept) && dept.Outdated == false).ID.ToString();
                        var requests1 = db.RequestItems_Table.Where(x => x.VKM_Year.ToString().Contains(useryear)).Select(x => new { x.RequestorNTID, x.VKM_Year, x.ApprovalDH, x.ApprovalSH, x.ApprovedDH, x.ApprovedSH, x.isCancelled, x.BU, x.OEM, x.DEPT, x.Group, x.Project, x.ItemName, x.Category, x.CostElement, x.UnitPrice, /*x.Currency,*/ x.ReqQuantity, x.ActualAvailableQuantity, x.TotalPrice,x.ApprQuantity,x.ApprCost, x.Comments, x.RequestorNT, x.RequestDate, x.DHNT, x.DHAppDate, x.SHNT, x.SHAppDate, x.OrderStatus, x.BudgetCode }).ToList();
                        //var requests = requests1.Where(x => x.DEPT == PresentUserDept_RequestTable).Select(x => new { x.BU, x.OEM, x.DEPT, x.Group, x.ItemName, x.Category, x.CostElement, x.UnitPrice, /*x.Currency,*/ x.ReqQuantity, x.TotalPrice, x.Comments, x.RequestorNT, x.RequestDate, x.DHNT, x.DHAppDate, x.SHNT, x.SHAppDate, x.OrderStatus }).ToList();

                        var requests = requests1.Select(x => new
                        {
                            x.VKM_Year,
                            x.BU,
                            x.OEM,
                            x.DEPT,
                            x.Group,
                            x.Project,
                            x.ItemName,
                            x.Category,
                            x.CostElement,
                            x.BudgetCode,
                            x.UnitPrice, /*x.Currency,*/
                            x.ReqQuantity,
                            x.ActualAvailableQuantity,
                            x.TotalPrice,
                            x.ApprQuantity,
                            x.ApprCost,
                            x.Comments,
                            x.RequestorNT,
                            x.RequestDate,
                            x.DHNT,
                            x.DHAppDate,
                            x.SHNT,
                            x.SHAppDate,
                            x.isCancelled,
                            x.ApprovalDH,
                            x.ApprovalSH,
                            x.ApprovedDH,
                            x.ApprovedSH,
                            x.OrderStatus,
                            x.RequestorNTID
                        }).ToList()//.FindAll(x => x.VKM_Year == 2023);
                        .FindAll(x => x.RequestorNTID.ToUpper().Contains(presentNTID.Trim()) || x.DEPT.Contains(PresentUserDept_RequestTable));

                        //            if (item.isCancelled != null)
                        //            {
                        //                ritem.isCancelled = (int)item.isCancelled;
                        //            }
                        foreach (var request in requests)
                        {
                            var isCancelled = request.isCancelled != null ? (request.isCancelled) : 0;

                            dt.Rows.Add(
                                BudgetingController.lstBUs.Find(bu => bu.ID.ToString().Equals(request.BU)).BU,
                                BudgetingController.lstOEMs.Find(oem => oem.ID.ToString().Equals(request.OEM)).OEM,
                                BudgetingController.lstDEPTs.Find(dept => dept.ID.ToString().Equals(request.DEPT)).DEPT,
                                BudgetingController.lstGroups_test.Find(grp => grp.ID.ToString().Equals(request.Group)).Group,
                                request.Project != null ? request.Project : "",
                                BudgetingController.lstItems.Find(item => item.S_No.ToString().Equals(request.ItemName)).Item_Name,
                                BudgetingController.lstPrdCateg.Find(cat => cat.ID.ToString().Equals(request.Category)).Category.Trim(),
                                BudgetingController.lstCostElement.Find(cost => cost.ID.ToString().Equals(request.CostElement)).CostElement,
                                request.BudgetCode.ToString(),
                                Math.Round((decimal)request.UnitPrice),
                                request.ActualAvailableQuantity != null ? request.ActualAvailableQuantity : "NA",
                                request.ReqQuantity,
                                Math.Round((decimal)request.TotalPrice),
                                 request.ApprQuantity,
                                Math.Round((decimal)request.ApprCost),
                               request.Comments != null ? request.Comments : "",
                                request.RequestorNT,
                                request.RequestDate.HasValue ? request.RequestDate.Value.ToString("dd-MM-yyyy") : "",
                                request.DHNT, request.DHAppDate.HasValue ? request.DHAppDate.Value.ToString("dd-MM-yyyy") : "",
                                request.SHNT, request.SHAppDate.HasValue ? request.SHAppDate.Value.ToString("dd-MM-yyyy") : "",
                                (request.ApprovalDH == true && request.ApprovalSH == false) ? "In Review with HoE" : (request.ApprovalDH == true && request.ApprovalSH == true && request.ApprovedSH == false ? "In Review with VKM SPOC" : (request.ApprovalDH == true && request.ApprovalSH == true && request.ApprovedSH == true ? "Reviewed by VKM SPOC" : (request.ApprovalDH == false && request.ApprovalSH == false && request.isCancelled == 1 ? "SentBack by HoE" : (request.ApprovalDH == false && request.ApprovalSH == false && request.isCancelled == 2 ? "SentBack by VKM SPOC" : "In Requestor's Queue")))),

                            (request.OrderStatus != null && request.OrderStatus.Trim() != "") ? BudgetingController.lstOrderStatus.Find(status => status.ID.ToString().Equals(request.OrderStatus.Trim())).OrderStatus : "");
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
            catch (Exception ex)
            {
                WriteLog("Error - Export : NTID - " + User.Identity.Name.Split('\\')[1].Trim().ToUpper() + ", ErrorMsg : " + ex + ", At - " + DateTime.Now);
                return Json(ex, JsonRequestBehavior.AllowGet);
            }

        }



        /// <summary>
        /// function to enable update of an existing item and add a new item request
        /// </summary>
        /// <param name="req"></param>
        /// <param name="useryear"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddOrEdit(RequestItemsRepoEdit1 req, string useryear)
        {
            List<RequestItemsRepoView> viewList = new List<RequestItemsRepoView>();
            connection();
            try
            {
                using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

                //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

                {
                    System.Security.Principal.IPrincipal User = System.Web.HttpContext.Current.User;

                    string presentNTID = User.Identity.Name.Split('\\')[1].ToUpper();
                    //string presentUserName = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).EmployeeName;
                    if (req.RequestID != 0)
                    {
                        if (db.RequestItems_Table.AsNoTracking().ToList().FindAll(x => x.RequestID == req.RequestID).Count() == 0)
                        {
                            return Json(new { success = false, message = "The Item is unavailable. Please check your Request items queue !" }, JsonRequestBehavior.AllowGet);

                        }
                        RequestItems_Table item1 = db.RequestItems_Table.AsNoTracking().Where(x => x.RequestID == req.RequestID).FirstOrDefault<RequestItems_Table>();
                        if (item1.ApprovalDH == true)
                        {
                            viewList = GetData1(useryear);
                            return Json(new { success = false, data = viewList, message = "The Item Request has already been Submitted for HOE Review !" }, JsonRequestBehavior.AllowGet);

                        }

                    }

                    string Query = " Exec [dbo].[LEPlanner_AddOrEdit] '" + presentNTID + "'," + useryear +
                        ",'" + req.BU + "','" + req.Item_Name + "','" + req.Comments + "','" + req.Project + "','" + req.DEPT +
                        "','" + req.Group + "','" + req.OEM + "'," + req.Required_Quantity + "," + req.RequestID;
                    BudgetingOpenConnection();
                    SqlCommand cmd = new SqlCommand(Query, budgetingcon);
                    SqlDataReader dr = cmd.ExecuteReader();
                    string msg = "";
                    if (dr.HasRows)
                    {
                        dr.Read();
                        msg = dr["Msg"].ToString();
                    }

                    viewList = GetData1(useryear);
                    if (req.RequestID == 0)
                    {
                        //db.RequestItems_Table.Add(item);
                        //db.SaveChanges();
                        return Json(new { data = viewList, success = true, message = msg.Trim() == "" ? "Saved Successfully" : "Saved Successfully ! Unable to find your " + msg + " details. Kindly contact SmartLab Team for assistance" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        //db.Entry(item).State = EntityState.Modified;

                        //db.SaveChanges();
                        return Json(new { data = viewList, success = true, message = msg.Trim() == "" ? "Updated Successfully" : "Updated Successfully ! Unable to find your " + msg + " details. Kindly contact SmartLab Team for assistance" }, JsonRequestBehavior.AllowGet);
                    }
                }

            }
            catch (Exception ex)
            {
                WriteLog("Error - AddorEdit : NTID - " + User.Identity.Name.Split('\\')[1].Trim().ToUpper() + ", ErrorMsg : " + ex + ", At - " + DateTime.Now);
                return Json(new { success = false, message = "Kindly retry after sometime" }, JsonRequestBehavior.AllowGet);
            }
            finally
            {
                BudgetingCloseConnection();
            }


        }


        /// <summary>
        /// function to move the item to L2 review
        /// </summary>
        /// <param name="id">Item ID</param>
        /// <param name="useryear"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult HoEApprove(int id, string useryear)
        {
            //logs1.AppendLine("Request ID:" + id.ToString());
            //var path = System.Web.HttpContext.Current.Server.MapPath(@"~/Logs/LEP_Logs_DeleteIssueTracker.txt");
            //var file = System.IO.File.Create(path);
            //file.Close();

            //TextWriter sw = new StreamWriter(path, true); //have 2 logs; critical events & errors - clear biweekly - prevent a step of local checking
            //sw.Write(logs1.ToString());
            //sw.Close();
            try
            {
                if (id.ToString() == null)
                {
                    return Json(new { success = false, message = "The Item is unavailable. Please check your Request items queue !" }, JsonRequestBehavior.AllowGet);

                }
                Emailnotify emailnotify = new Emailnotify();
                List<RequestItemsRepoView> viewList = new List<RequestItemsRepoView>();

                using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

                //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

                {
                    System.Security.Principal.IPrincipal User = System.Web.HttpContext.Current.User;
                    string presentUserName = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).EmployeeName;
                    string presentUserName1 = string.Empty;
                    if (db.Planning_EM_Table.ToList().FindAll(xi => xi.NTID == User.Identity.Name.Split('\\')[1].ToUpper()).Count() != 0) //Present user is EM
                    {
                        //Get his Proxy Name if Proxy Present
                        if (db.Planning_EM_Table.ToList().Find(xi => xi.NTID == User.Identity.Name.Split('\\')[1].ToUpper()).Proxy_NTID != null)
                        {
                            presentUserName1 = db.Planning_EM_Table.ToList().Find(xi => xi.NTID == User.Identity.Name.Split('\\')[1].ToUpper()).Proxy_FullName.Trim();

                        }
                    }
                    else if (db.Planning_EM_Table.ToList().FindAll(xi => xi.Proxy_NTID == User.Identity.Name.Split('\\')[1].ToUpper()).Count() != 0) //Present user is Proxy
                    {
                        //Get his EM Name 
                        presentUserName1 = db.Planning_EM_Table.ToList().Find(xi => xi.Proxy_NTID == User.Identity.Name.Split('\\')[1].ToUpper()).FullName.Trim();

                    }

                    //if (id == 1999999999)
                    //{
                    //    List<RequestItems_Table> templist = new List<RequestItems_Table>();//////useryear?
                    //    templist = db.RequestItems_Table.ToList();
                    //    Nullable<decimal> totalAmount = 0.0M;
                    //    string HoENT = "";
                    //    string ReqNT = "";

                    //    if (presentUserName1 != null || presentUserName1 != string.Empty)
                    //        templist = templist.FindAll(x => x.RequestorNT.Trim().Contains(presentUserName.Trim()) || x.RequestorNT.Trim().Contains(presentUserName1.Trim())).FindAll(item => item.ApprovalDH == false).FindAll(items => items.RequestDate.ToString().Contains(DateTime.Now.Year.ToString()));
                    //    else
                    //        templist = templist.FindAll(x => x.RequestorNT.Trim().Contains(presentUserName.Trim())).FindAll(items => items.RequestDate.ToString().Contains(DateTime.Now.Year.ToString()));


                    //    int totalCount = templist.Count;
                    //    if (totalCount == 0)
                    //        return Json(new { success = false, message = "The Item Requests are already Submitted for HOE Review !" }, JsonRequestBehavior.AllowGet);

                    //    bool ApprovalDH = false;
                    //    bool ApprovalSH = false;
                    //    foreach (RequestItems_Table item in templist)
                    //    {
                    //        RequestItems_Table changeItem = db.RequestItems_Table.Where(x => x.RequestID == item.RequestID).FirstOrDefault<RequestItems_Table>();
                    //        totalAmount += item.TotalPrice;
                    //        HoENT = item.DHNT;
                    //        ReqNT = item.RequestorNT;
                    //        changeItem.ApprovalDH = true;
                    //        changeItem.SubmitDate = DateTime.Now.Date;
                    //        ApprovalDH = (bool)changeItem.ApprovalDH;
                    //        ApprovalSH = (bool)changeItem.ApprovalSH;

                    //        db.Entry(changeItem).State = EntityState.Modified;
                    //        db.SaveChanges();
                    //    }

                    //    //email attributes
                    //    emailnotify.RequestID_foremail = id;
                    //    emailnotify.ReviewLevel = "L1";
                    //    emailnotify.Count = totalCount;
                    //    emailnotify.TotalAmount = totalAmount;
                    //    emailnotify.NTID_toEmail = HoENT;
                    //    emailnotify.NTID_ccEmail = ReqNT;
                    //    emailnotify.is_ApprovalorSendback = (bool)ApprovalDH || (bool)ApprovalSH;
                    //     return Json(new { data = emailnotify, success = true, message = totalCount.ToString() + " Item(s) Submitted Successfully" }, JsonRequestBehavior.AllowGet);

                    //}
                    if (id == 1999999999)
                    {
                        List<RequestItems_Table> templist = new List<RequestItems_Table>();//////useryear?
                        templist = db.RequestItems_Table.ToList().FindAll(x=>x.ApprovalDH == false && x.RequestorNT != null);
                        Nullable<decimal> totalAmount = 0.0M;
                        string HoENT = "";
                        string ReqNT = "";

                        if (presentUserName1.Trim() != null && presentUserName1.Trim() != string.Empty)
                            templist = templist.FindAll(x => x.RequestorNT.Trim().Equals(presentUserName.Trim()) || x.RequestorNT.Trim().Equals(presentUserName1.Trim())).FindAll(item => item.ApprovalDH == false).FindAll(items => items.RequestDate.ToString().Contains(DateTime.Now.Year.ToString()));
                        else
                            templist = templist.FindAll(x => x.RequestorNT.Trim().Equals(presentUserName.Trim())).FindAll(item => item.ApprovalDH == false).FindAll(items => items.RequestDate.ToString().Contains(DateTime.Now.Year.ToString()));


                        int totalCount = templist.Count;
                        if (totalCount == 0)
                            return Json(new { success = false, message = "The Item Requests are already Submitted for HOE Review !" }, JsonRequestBehavior.AllowGet);

                        bool ApprovalDH = false;
                        bool ApprovalSH = false;
                        foreach (RequestItems_Table item in templist)
                        {
                            RequestItems_Table changeItem = db.RequestItems_Table.Where(x => x.RequestID == item.RequestID).FirstOrDefault<RequestItems_Table>();

                            changeItem.ApprCost = changeItem.TotalPrice;
                            changeItem.ApprQuantity = changeItem.ReqQuantity;
                            totalAmount += item.TotalPrice;
                            HoENT += HoENT.Contains(item.DHNT) ? "" : "|" + item.DHNT;
                            ReqNT = item.RequestorNT;
                            changeItem.ApprovalDH = true;
                            changeItem.SubmitDate = DateTime.Now.Date;
                            ApprovalDH = (bool)changeItem.ApprovalDH;
                            ApprovalSH = (bool)changeItem.ApprovalSH;

                            db.Entry(changeItem).State = EntityState.Modified;
                            var success_int = db.SaveChanges();
                        }

                        //email attributes
                        emailnotify.Requests_foremail = templist;
                        emailnotify.RequestID_foremail = id;
                        emailnotify.ReviewLevel = "L1";
                        emailnotify.Count = totalCount;
                        emailnotify.TotalAmount = totalAmount;
                        emailnotify.NTID_toEmail = HoENT;
                        emailnotify.NTID_ccEmail = ReqNT;
                        emailnotify.is_ApprovalorSendback = (bool)ApprovalDH || (bool)ApprovalSH;
                        return Json(new { data = emailnotify, success = true, message = totalCount.ToString() + " Item(s) Submitted Successfully" }, JsonRequestBehavior.AllowGet);

                    }
                    else
                    {

                        if (db.RequestItems_Table.AsNoTracking().ToList().FindAll(x => x.RequestID == id).Count() == 0)
                        {
                            return Json(new { success = false, message = "The Item is unavailable. Please check your Request items queue !" }, JsonRequestBehavior.AllowGet);

                        }
                        RequestItems_Table item = db.RequestItems_Table.AsNoTracking().Where(x => x.RequestID == id).FirstOrDefault<RequestItems_Table>();
                        if (item.ApprovalDH == true)
                            return Json(new { success = false, message = "The Item Request has already been Submitted for HOE Review !" }, JsonRequestBehavior.AllowGet);

                        item.ApprCost = item.TotalPrice;
                        item.ApprQuantity = item.ReqQuantity;

                        item.ApprovalDH = true;
                        item.SubmitDate = DateTime.Now.Date;
                        db.Entry(item).State = EntityState.Modified;
                        var success_int = db.SaveChanges();
                        //email attributes
                        emailnotify.RequestID_foremail = id;
                        emailnotify.ReviewLevel = "L1";
                        emailnotify.is_ApprovalorSendback = (bool)item.ApprovalDH || (bool)item.ApprovalSH;
                        return Json(new { data = emailnotify, success = true, message = "Submitted Successfully" }, JsonRequestBehavior.AllowGet);
                    }


                }


            }
            catch (Exception ex)
            {
                WriteLog("Error - HOEApprove : NTID - " + User.Identity.Name.Split('\\')[1].Trim().ToUpper() + ", ErrorMsg : " + ex + ", At - " + DateTime.Now);
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
                    if (item.ApprovalDH == true)
                    {
                        viewList = GetData1(useryear);
                        return Json(new { data = viewList, success = false, message = "The Item Request has already been Submitted for HOE Review !" }, JsonRequestBehavior.AllowGet);

                    }
                    else
                    {
                        db.RequestItems_Table.Remove(item);
                        db.SaveChanges();
                        viewList = GetData1(useryear);
                        return Json(new { data = viewList, success = true, message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
                    }

                }
            }
            catch (Exception ex)
            {
                WriteLog("Error - Delete : NTID - " + User.Identity.Name.Split('\\')[1].Trim().ToUpper() + ", ErrorMsg : " + ex + ", At - " + DateTime.Now);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }


        }



        [HttpPost]
        public ActionResult Notify_ifalready_ordered(RequestItemsRepoEdit1 req)
        {
            try
            {

                using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

                //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

                {
                    System.Security.Principal.IPrincipal User = System.Web.HttpContext.Current.User;
                    string presentUserName = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).EmployeeName;
                    List<RequestItems_Table> reqList = new List<RequestItems_Table>();
                    int prev_sameitem_reqid = 0;
                    var ReqItem = BudgetingController.lstItems.Find(x => x.S_No == req.Item_Name).Item_Name.Trim().ToUpper().Replace("\r\n", "").Replace("\r", "").Replace("\n", "");
                    reqList = db.RequestItems_Table.ToList<RequestItems_Table>().FindAll(item => item.RequestorNT == presentUserName).FindAll(item => item.RequestDate.Value.Year == DateTime.Now.AddYears(-1).Year).FindAll(item => item.ApprovalSH == true);
                    if (reqList != null && reqList.Count() > 0)
                    {
                        //if (reqList.FirstOrDefault(i => i.ItemName == req.Item_Name.ToString()) != null) - ItemID comparison
                        //reqList.FirstOrDefault(i => i.ItemName == req.Item_Name.ToString()).RequestID;

                        //Item Name comparison
                        if (reqList.Find(item => BudgetingController.lstItems.Find(i => i.S_No.ToString().Trim() == item.ItemName.Trim()).Item_Name.ToUpper().Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", "") == ReqItem.Trim()) != null)
                        {
                            var matchrequest = reqList.Find(item => BudgetingController.lstItems.Find(i => i.S_No.ToString().Trim() == item.ItemName.Trim()).Item_Name.ToUpper().Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", "") == ReqItem.Trim());
                            //prev_sameitem_reqid = matchrequest.FirstOrDefault(item => BudgetingController.lstItems.Find(i => i.S_No.ToString().Trim() == item.ItemName.Trim()).Item_Name.ToUpper().Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", "") == ReqItem.Trim()).RequestID;
                            if (matchrequest != null)
                                prev_sameitem_reqid = matchrequest.RequestID;
                        }
                        else
                            return Json(new { message = "", }, JsonRequestBehavior.AllowGet);

                    }
                    else
                        return Json(new { message = "", }, JsonRequestBehavior.AllowGet);


                    if (reqList.Find(previtem => previtem.RequestID == prev_sameitem_reqid).ApprovalSH == true)
                    {
                        return Json(new
                        {
                            message = "INFORMATION:\n \n Please note that you have preplanned " + reqList.Find(z => z.RequestID == prev_sameitem_reqid).ApprQuantity + " quantity of this item in VKM " + reqList.Find(z => z.RequestID == prev_sameitem_reqid).RequestDate.Value.AddYears(1).Year + "."
                        }, JsonRequestBehavior.AllowGet);
                    }
                    else
                        return Json(new { message = "", }, JsonRequestBehavior.AllowGet);






                }
            }
            catch (Exception ex)
            {
                WriteLog("Error - Notify_ifalready_ordered : NTID - " + User.Identity.Name.Split('\\')[1].Trim().ToUpper() + ", ErrorMsg : " + ex + ", At - " + DateTime.Now);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }

        }


        //[HttpPost]
        //public ActionResult GetAlreadyAvailableQuantity(int ItemName)
        //{

        //    using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //    //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //    {
        //        List<RequestItems_Table> reqList = new List<RequestItems_Table>();
        //        var itemname = BudgetingController.lstItems.Find(x => x.S_No == ItemName).Item_Name;
        //        int avail_quantity = 0;
        //        reqList = db.RequestItems_Table.ToList().
        //            FindAll(item => BudgetingController.lstItems.Find(i => i.S_No.ToString().Trim() ==
        //            item.ItemName.Trim()).Item_Name.ToUpper().Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", "").Replace("+", "").Replace(" ", string.Empty) ==
        //            itemname.ToUpper().Trim().Replace("\r\n", "").Replace("\r", "").Replace("\n", "").Replace("+", "").Replace(" ", string.Empty));
        //        foreach(var i in reqList)
        //        {
        //            avail_quantity += (int)i.ApprQuantity;
        //        }
        //        return Json(avail_quantity, JsonRequestBehavior.AllowGet);
        //    }



        //}


        /// <summary>
        /// function to get the Initial values to be filled automatically when a new request is created
        /// </summary>
        /// <returns></returns>
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
                Query = " Exec [dbo].[LEPlanner_InitRowValues] '" + UserNTID + "' ";
                //"IF EXISTS(SELECT RequestorNTID from RequestItems_Table where RequestorNTID = @User)SELECT TOP 1 BU , OEM from RequestItems_Table where RequestorNTID = @User order by UpdatedAt desc";
                BudgetingOpenConnection();
                SqlCommand cmd = new SqlCommand(Query, budgetingcon);
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
                }
                else
                {
                    temp.Requestor = "";
                    temp.DEPT = 0;
                    temp.Group = 0;
                    temp.Reviewer_1 = "NA";
                    temp.BU = 0;
                    temp.OEM = 0;
                    temp.Reviewer_2 = "";//not NA because this is on load auto-fill, where VKM SPOC may not be determined


                }
                if (temp.Reviewer_1.Trim() != "NA" && (( temp.BU != 0 && temp.Reviewer_2.Trim() != "NA") || (temp.BU == 0))) //if bu = 0 ; on page load, vkm apoc cannot be determined hence 'na' not checked ; if bu != 0, then vkm spoc should be present hence 'na' checked
                                                                                                                             //vkm spoc if "na" not checked bcoz - here, auto filled on pg load - vkm spoc may not b determined until bu selected
                {

                    return Json(new { data = temp, success = true }, JsonRequestBehavior.AllowGet);
                }
                else
                {

                    return Json(new { userNT = UserNTID, success = false , data = temp}, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                WriteLog("Error - InitRowValues : NTID - " + User.Identity.Name.Split('\\')[1].Trim().ToUpper() + ", ErrorMsg : " + ex + ", At - " + DateTime.Now);
                return Json(new { success = false, userNT = UserNTID }, JsonRequestBehavior.AllowGet);
            }
            finally
            {
                BudgetingCloseConnection();
            }

        }


        /// <summary>
        /// function to get L3 reviewer name based on BU
        /// </summary>
        /// <param name="BU"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetReviewer_VKMSPOC(int DEPT, int BU)
        {
            connection();
            string UserNTID = System.Web.HttpContext.Current.User.Identity.Name.Split('\\')[1].ToUpper();
            try
            {
                string SHNT = string.Empty;
                //SHNT = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(BudgetingController.lstBU_SPOCs.Find(spoc => spoc.BU.Equals(BU)).VKMspoc.ToUpper().Trim())).EmployeeName;
                string Query = " Exec [dbo].[LEPlanner_GetHOEorVKMSPOC] '"
                    + UserNTID + "','VKMSPOC'," + DEPT + ", '"+BU+"' ,'',0";
                BudgetingOpenConnection();
                SqlCommand cmd = new SqlCommand(Query, budgetingcon);
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
                WriteLog("Error - GetReviewer_VKMSPOC : NTID - " + User.Identity.Name.Split('\\')[1].Trim().ToUpper() + ", ErrorMsg : " + ex + ", At - " + DateTime.Now);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
            finally
            {
                BudgetingCloseConnection();
            }
        }

        ////GetReviewer_HoE

        /// <summary>
        /// function to get L2 reviewer name based on DEPT
        /// </summary>
        /// <param name="DEPT"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetReviewer_HoE(int DEPT)
        {
            string L2ReviewerName = string.Empty;
            string UserNTID = System.Web.HttpContext.Current.User.Identity.Name.Split('\\')[1].ToUpper();
            connection();
            try
            {
                string Query = " Exec [dbo].[LEPlanner_GetHOEorVKMSPOC] '"
                   + UserNTID + "','HOE'," + DEPT + ",'','',0";
                BudgetingOpenConnection();
                SqlCommand cmd = new SqlCommand(Query, budgetingcon);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    L2ReviewerName = dr["Reviewer"].ToString();

                }
                else
                {
                    L2ReviewerName = "NA";

                }
                if (L2ReviewerName.Trim() != "NA")
                {

                    return Json(new { data = L2ReviewerName, success = true }, JsonRequestBehavior.AllowGet);
                }
                else
                {

                    return Json(new { data = UserNTID, success = false }, JsonRequestBehavior.AllowGet);
                }
                //string Selected_Dept = BudgetingController.lstDEPTs.Find(dpt => dpt.ID.Equals(DEPT)).DEPT.Trim().ToUpper();

                ////var HoE = BudgetingController.lstUsers.FindAll(user => user.Department.Trim().Equals(Selected_Dept).Equals(user.Group.ToUpper())).FirstOrDefault(head => head.Role.ToUpper().Contains("HEAD")).EmployeeName;

                //L2ReviewerName = BudgetingController.lstUsers.FindAll(user => Selected_Dept.Equals(user.Group.ToUpper())).FirstOrDefault(head => head.Role.ToUpper().Contains("HEAD")).EmployeeName;


            }
            catch (Exception ex)
            {
                L2ReviewerName = "NA";

                WriteLog("Error - InitRowValues : NTID - " + User.Identity.Name.Split('\\')[1].Trim().ToUpper() + ", ErrorMsg : " + ex + ", At - " + DateTime.Now);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);

            }
            finally
            {
                BudgetingCloseConnection();
            }



        }

        //[HttpPost]
        //public ActionResult GetMasterList(int BU)
        //{


        //   if(BU == 1 || BU == 3) //AS, 2WP - DELETED 
        //    {
        //        var itemlist = BudgetingController.lstItems.FindAll(item => item.Deleted == true);
        //        return Json(itemlist, JsonRequestBehavior.AllowGet);

        //    }
        //   if(BU == 2 || BU == 4) //DA - newtable
        //    {
        //        var itemlist = BudgetingController.lstItems1;
        //        return Json(itemlist, JsonRequestBehavior.AllowGet);
        //    }
        //   else //correct data
        //    {
        //        var itemlist = BudgetingController.lstItems.FindAll(item => item.Deleted != true);
        //        return Json(itemlist, JsonRequestBehavior.AllowGet);
        //    }

        //}

        [HttpGet]
        public ActionResult Lookup(string year)
        {
            try
            {
                LookupData lookupData = new LookupData();

                lookupData.DEPT_List = BudgetingController.lstDEPTs_FilteredForUsers;
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

                lookupData.Groups_test = BudgetingController.lstGroups_test.ToList();
                //lookupData.Item_List = BudgetingController.lstItems.FindAll(x => x.Category != null && x.Category.Trim() != "").FindAll(x=>x.VKM_Year == int.Parse(year));

                lookupData.Category_List = BudgetingController.lstPrdCateg;
                lookupData.CostElement_List = BudgetingController.lstCostElement;
                lookupData.OrderStatus_List = BudgetingController.lstOrderStatus;
                lookupData.Order_Type_List = BudgetingController.lstOrderType;

                //lookupData.VendorCategory_List = BudgetingController.lstVendor;
                lookupData.BudgetCodeList = BudgetingController.BudgetCodeList;

                return Json(new { data = lookupData }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                WriteLog("Error - Lookup : NTID - " + User.Identity.Name.Split('\\')[1].Trim().ToUpper() + ", ErrorMsg : " + ex + ", At - " + DateTime.Now);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
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
                            dt_new.Columns.Add("ReqQuantity", typeof(int));

                            dt_new.Columns.Add("Justification", typeof(string));
                            dt_new.Columns.Add("HOERemarks", typeof(string));
                            dt_new.Columns.Add("VKMSPOCRemarks", typeof(string));
                            dt_new.Columns.Add("Fund", typeof(string));
                            dt_new.Columns.Add("BudgetCenter", typeof(string));
                            dt_new.Columns.Add("RequestDt", typeof(string));
                            dt_new.Columns.Add("HOEApprovedDt", typeof(string));
                            dt_new.Columns.Add("VKMApprovedDt", typeof(string));
                            dt_new.Columns.Add("TentativeDelDt", typeof(string));
                            dt_new.Columns.Add("ActualDelDt", typeof(string));





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
                                row1[1] = (DateTime.Now.Year + 1).ToString();
                                row1[2] = dt1.Rows[i][1].ToString();
                                row1[3] = dt1.Rows[i][2].ToString();
                                row1[4] = dt1.Rows[i][3].ToString();
                                row1[5] = dt1.Rows[i][4].ToString();
                                row1[6] = dt1.Rows[i][5].ToString();
                                row1[7] = dt1.Rows[i][6].ToString();
                                row1[8] = (dt1.Rows[i][7] == DBNull.Value) ? 0 : Convert.ToDouble(dt1.Rows[i][7]);
                                row1[9] = dt1.Rows[i][8].ToString();
                                row1[10] = dt1.Rows[i][9].ToString();
                                row1[11] = dt1.Rows[i][10].ToString();
                                row1[12] = dt1.Rows[i][11].ToString();

                                row1[13] = (dt1.Rows[i][12].ToString().Trim() == "") ? string.Empty : dt1.Rows[i][12].ToString().Replace(".", "/");
                                //var date_replace = dt1.Rows[i][14].ToString().Replace(".", "/");


                                //WriteLog("date replace:" + date_replace);
                                //  row1[12] = (dt1.Rows[i][37].ToString().Trim() == "") ? defaultDate : Convert.ToDateTime(dt1.Rows[i][37].ToString().Replace(".","/"));
                                row1[14] = (dt1.Rows[i][13].ToString().Trim() == "") ? string.Empty : dt1.Rows[i][13].ToString().Replace(".", "/");
                                row1[15] = (dt1.Rows[i][14].ToString().Trim() == "") ? string.Empty : dt1.Rows[i][14].ToString().Replace(".", "/");
                                row1[16] = (dt1.Rows[i][15].ToString().Trim() == "") ? string.Empty : dt1.Rows[i][15].ToString().Replace(".", "/");
                                row1[17] = (dt1.Rows[i][16].ToString().Trim() == "") ? string.Empty : dt1.Rows[i][16].ToString().Replace(".", "/");


                                dt_new.Rows.Add(row1);
                            }

                            connection();

                            SqlCommand command = new SqlCommand();

                            command.Connection = budgetingcon;
                            command.CommandText = "dbo.[LEPlanner_AddOrEdit_Upload]";
                            command.CommandType = CommandType.StoredProcedure;

                            // Add the input parameter and set its properties.

                            SqlParameter parameter2 = new SqlParameter();
                            parameter2.ParameterName = "@UserName";
                            parameter2.SqlDbType = SqlDbType.NVarChar;
                            parameter2.Direction = ParameterDirection.Input;
                            parameter2.Value = presentUserName;


                            SqlParameter parameter1 = new SqlParameter();
                            parameter1.ParameterName = "@LEPlanner_ReqList";
                            parameter1.SqlDbType = SqlDbType.Structured;
                            parameter1.TypeName = "dbo.LEPlanner_ReqList";
                            parameter1.Direction = ParameterDirection.Input;
                            parameter1.Value = dt_new;

                            // Add the parameter to the Parameters collection.
                            command.Parameters.Add(parameter1);
                            command.Parameters.Add(parameter2);

                            BudgetingOpenConnection();
                            //WriteLog("Executing STORED PROCEDURE");
                            command.CommandTimeout = 300; //5 min

                            //ErrorMsg = command.ExecuteScalar().ToString();
                            //WriteLog("ErrorMsg: " + ErrorMsg);
                            command.ExecuteNonQuery();

                            command = new SqlCommand("select top 1  convert(nvarchar(100),Msg) as ErrorMsg from LEPlannerLog where Msg like '%Error Msg%' order by logtime desc", budgetingcon);


                            SqlDataAdapter da = new SqlDataAdapter(command);
                            DataTable dt2 = new DataTable();
                            da.Fill(dt2);
                            BudgetingCloseConnection();
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


                            BudgetingCloseConnection();


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
            return Json(new { success = true, save, errormsg = ErrorMsg }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Post method for importing users 
        /// </summary>
        /// <param name="postedFile"></param>
        /// <returns></returns>
        //[HttpPost]
        //public ActionResult Index(HttpPostedFileBase postedFile)
        //{
        //    string exceptionmsg = "";
        //    if (postedFile != null)
        //    {
        //        try
        //        {
        //            string fileExtension = Path.GetExtension(postedFile.FileName);

        //            //Validate uploaded file and return error.
        //            if (fileExtension != ".xls" && fileExtension != ".xlsx")
        //            {

        //                return Json(new { errormsg = "Please select the excel file with.xls or.xlsx extension" });

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

        //            using (OleDbConnection excelOledbConnection = new OleDbConnection(excelConString))
        //            {
        //                using (OleDbCommand excelDbCommand = new OleDbCommand())
        //                {
        //                    using (OleDbDataAdapter excelDataAdapter = new OleDbDataAdapter())
        //                    {
        //                        excelDbCommand.Connection = excelOledbConnection;

        //                        excelOledbConnection.Open();
        //                        //Get schema from excel sheet
        //                        DataTable excelSchema = GetSchemaFromExcel(excelOledbConnection);
        //                        //Get sheet name
        //                        string sheetName = excelSchema.Rows[0]["TABLE_NAME"].ToString();//give worksheetnames
        //                        excelOledbConnection.Close();

        //                        //Read Data from First Sheet.
        //                        excelOledbConnection.Open();
        //                        excelDbCommand.CommandText = "SELECT * From [" + sheetName + "]";
        //                        excelDataAdapter.SelectCommand = excelDbCommand;
        //                        //Fill datatable from adapter
        //                        excelDataAdapter.Fill(dt);
        //                        excelOledbConnection.Close();
        //                    }
        //                }
        //            }


        //            int errcount = 0;
        //            string msg = "";

        //            using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //            //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //            {
        //                System.Security.Principal.IPrincipal User = System.Web.HttpContext.Current.User;
        //                string presentUserName = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).EmployeeName;

        //                //Loop through datatable and add data to table. 
        //                foreach (DataRow row in dt.Rows)
        //                {
        //                    RequestItems_Table item = new RequestItems_Table();
        //                    try
        //                    {

        //                        //to remove unwanted spaces
        //                        RegexOptions options = RegexOptions.None;
        //                        Regex regex = new Regex("[ ]{2,}", options);

        //                        if (((row[0] == DBNull.Value) && (row[1] == DBNull.Value) && (row[2] == DBNull.Value) && (row[3] == DBNull.Value) && (row[4] == DBNull.Value) && (row[5] == DBNull.Value) && (row[6] == DBNull.Value)) || (String.IsNullOrWhiteSpace(row[0].ToString()) && String.IsNullOrWhiteSpace(row[1].ToString()) && String.IsNullOrWhiteSpace(row[2].ToString()) && String.IsNullOrWhiteSpace(row[3].ToString()) && String.IsNullOrWhiteSpace(row[4].ToString()) && String.IsNullOrWhiteSpace(row[5].ToString()) && String.IsNullOrWhiteSpace(row[6].ToString())))
        //                        {
        //                            continue;
        //                        }
        //                        else
        //                        {

        //                            if (String.IsNullOrWhiteSpace(row[4].ToString()) || row[4] == DBNull.Value)
        //                            {
        //                                errcount++;
        //                                if (errcount > 1)
        //                                    msg += "| \n" + "Please enter Item Name";
        //                                else
        //                                    msg += "Please enter Item Name";
        //                                continue;

        //                            }
        //                            if (String.IsNullOrWhiteSpace(row[2].ToString()) || row[2] == DBNull.Value)
        //                            {
        //                                errcount++;
        //                                if (errcount > 1)
        //                                    msg += "| \n" + "Please enter Department Details";
        //                                else
        //                                    msg += "Please enter Department Details";
        //                                continue;

        //                            }

        //                            item.BU = BudgetingController.lstBUs.Find(cat => cat.BU.Trim().Replace(" ", "").ToLower().Equals(row[0].ToString().Replace(" ", "").Trim().ToLower())).ID.ToString();
        //                            item.OEM = BudgetingController.lstOEMs.Find(cat => cat.OEM.Trim().Replace(" ", "").ToLower().Equals(row[1].ToString().Replace(" ", "").Trim().ToLower())).ID.ToString();
        //                            item.DEPT = BudgetingController.lstDEPTs.Find(cat => cat.DEPT.Trim().Replace(" ", "").ToLower().Equals(row[2].ToString().Replace(" ", "").Trim().ToLower())).ID.ToString();
        //                            item.Group = BudgetingController.lstGroups_test.Find(cat => cat.Group.Trim().Replace(" ", "").ToLower().Equals(row[3].ToString().Replace(" ", "").Trim().ToLower())).ID.ToString();
        //                            item.ItemName = BudgetingController.lstItems.FindAll(x => x.Deleted == false).Find(cat => cat.Item_Name.Trim().Replace(" ", "").ToLower().Equals(row[4].ToString().Replace(" ", "").Trim().ToLower())).S_No.ToString();
        //                            item.ReqQuantity = int.Parse(row[5].ToString());
        //                            item.Comments = row[6].ToString();

        //                            item.Category = BudgetingController.lstItems.Find(x => x.S_No == int.Parse(item.ItemName)).Category;
        //                            item.CostElement = BudgetingController.lstItems.Find(x => x.S_No == int.Parse(item.ItemName)).Cost_Element;
        //                            item.UnitPrice = (decimal?)BudgetingController.lstItems.Find(x => x.S_No == int.Parse(item.ItemName)).UnitPriceUSD;
        //                            item.TotalPrice = item.ReqQuantity * item.UnitPrice;
        //                            item.RequestorNT = presentUserName;
        //                            //string Selected_Dept = BudgetingController.lstDEPTs.Find(dpt => dpt.ID.Equals(item.DEPT)).DEPT.Trim().ToUpper();
        //                            item.DHNT = BudgetingController.lstUsers.FindAll(user => row[2].ToString().Replace(" ", "").Trim().ToUpper().Equals(user.Group.ToUpper().Trim())).FirstOrDefault(head => head.Role.ToUpper().Contains("HEAD")).EmployeeName; ;
        //                            //item.SHNT = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(BudgetingController.lstBU_SPOCs.Find(spoc => spoc.BU.Equals(item.BU)).VKMspoc.ToUpper().Trim())).EmployeeName;;


        //                            var VKMSPOC_NT = BudgetingController.lstBU_SPOCs.Find(spoc => spoc.BU.Equals(int.Parse(item.BU))).VKMspoc.ToUpper().Trim();
        //                            item.SHNT = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Contains(VKMSPOC_NT)).EmployeeName;
        //                            item.RequestDate = DateTime.Now.Date;
        //                            item.ApprovalDH = false;
        //                            item.ApprovedDH = false;
        //                            item.ApprovalSH = false;
        //                            item.ApprovedSH = false;


        //                            db.RequestItems_Table.Add(item);

        //                        }

        //                        int milliseconds = 500;
        //                        Thread.Sleep(milliseconds);
        //                        db.SaveChanges();
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        errcount++;

        //                        if (errcount > 1)
        //                            msg += " | \n" + "Empty/invalid cell found: " + "Row Details: Dept - " + row[2].ToString() + " Item - " + row[4].ToString();
        //                        else
        //                            msg += "Empty/invalid cell found: " + "Row Details: Dept - " + row[2].ToString() + " Item - " + row[4].ToString();


        //                    }

        //                }
        //            }


        //            if (errcount > 0)
        //            {
        //                return Json(new { dataerror = true, errormsg = msg + " \nThe valid requests were imported. Please find the errors listed. " });
        //            }



        //            else
        //            {
        //                return Json(new { dataerror = false, successmsg = " The requests were successfully imported. Please edit any INVALID fields in the table." });
        //            }





        //        }
        //        catch (Exception ex)
        //        {

        //            exceptionmsg += ex.Message;
        //            return Json(new { errormsg = exceptionmsg });
        //        }

        //    }
        //    else
        //    {

        //        return Json(new { dataerror = true, errormsg = "Please select the file to be uploaded." });

        //    }


        //}

        public ActionResult DownloadTemplate()
        {
            string folderPath = Server.MapPath("~/Templates/");
            string fileName = "VKMPlanner_Requests.xlsx";
            byte[] fileBytes = System.IO.File.ReadAllBytes(folderPath + fileName);

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        /// <summary>
        /// Function to get the excel layout of the imported excel file
        /// </summary>
        /// <param name="excelOledbConnection"></param>
        /// <returns></returns>
        private static DataTable GetSchemaFromExcel(OleDbConnection excelOledbConnection)
        {
            return excelOledbConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
        }




        [HttpPost]
        public ActionResult GetEM_ProxyDetails()
        {
            try
            {

                List<Planning_EM_Table> HOE_details = new List<Planning_EM_Table>();

                using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

                //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

                {

                    HOE_details = db.Planning_EM_Table.ToList().FindAll(xi => xi.NTID == User.Identity.Name.Split('\\')[1].ToUpper());

                    return Json(new
                    {
                        data = HOE_details
                    }, JsonRequestBehavior.AllowGet);

                }
            }
            catch (Exception ex)
            {
                WriteLog("Error - GetEM_ProxyDetails : NTID - " + User.Identity.Name.Split('\\')[1].Trim().ToUpper() + ", ErrorMsg : " + ex + ", At - " + DateTime.Now);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }


        }

        [HttpPost]
        public ActionResult ChangeButtonName_enabledisable()
        {

            using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

            //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

            {
                return Json(new
                {
                    data = db.Planning_EM_Table.ToList().Find(xi => xi.NTID == User.Identity.Name.Split('\\')[1].ToUpper()).Enable_Proxy
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public ActionResult ChangeFlag_enabledisable(string is_enableDisable)
        {


            using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

            //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

            {
                System.Security.Principal.IPrincipal User = System.Web.HttpContext.Current.User;
                if (db.Planning_EM_Table.ToList().FindAll(xi => xi.NTID == User.Identity.Name.Split('\\')[1].ToUpper()).Count() != 0)
                {
                    Planning_EM_Table HOEList = db.Planning_EM_Table.ToList().Find(xi => xi.NTID == User.Identity.Name.Split('\\')[1].ToUpper());
                    HOEList.Department = HOEList.Department;
                    HOEList.FullName = HOEList.FullName;
                    HOEList.NTID = HOEList.NTID;
                    HOEList.ID = HOEList.ID;
                    HOEList.Proxy_FullName = HOEList.Proxy_FullName;
                    HOEList.Proxy_NTID = HOEList.Proxy_NTID;
                    if (is_enableDisable == "Enable Proxy") // Proxy details not available and ENable Proxy is clicked
                    {
                        if (HOEList.Proxy_NTID != null)
                            HOEList.Enable_Proxy = (is_enableDisable == "Enable Proxy") ? true : false;
                        else
                            return Json(new { success = false, message = "Proxy Details are not available. Please check again" }, JsonRequestBehavior.AllowGet);

                    }
                    else
                        HOEList.Enable_Proxy = (is_enableDisable == "Enable Proxy") ? true : false;

                    db.Entry(HOEList).State = EntityState.Modified;
                    db.SaveChanges();

                    db.Database.CommandTimeout = 10000;

                    if (is_enableDisable == "Enable Proxy")
                        return Json(new { success = true /*,message = "Proxy setting is Enabled. Requests in your Queue will be redirected to your Proxy "*/ }, JsonRequestBehavior.AllowGet);
                    else
                        return Json(new { success = true, message = "Proxy setting is Disabled. Requests will be directed back to your Queue" }, JsonRequestBehavior.AllowGet);





                }
                else
                    return Json(new { success = false, message = "EM-Proxy Details are not available. Please check again" }, JsonRequestBehavior.AllowGet);

            }
        }



        [HttpGet]
        public ActionResult CountDown()
        {
            try
            {

                using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

                //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

                {
                    DataTable dtTime = new DataTable();
                    DateTime PrevDate = DateTime.Now;
                    DateTime CurDate = DateTime.Now;
                    string presentUser = User.Identity.Name.Split('\\')[1].ToUpper();

                    string query = " Exec [dbo].[LEPlanner_CountDown] '" + User.Identity.Name.Split('\\')[1].Trim() + "' ";
                    connection();
                    BudgetingOpenConnection();
                    SqlCommand cmd = new SqlCommand(query, budgetingcon);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dtTime);
                    BudgetingCloseConnection();

                    if (dtTime.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtTime.Rows.Count; i++)
                        {
                            PrevDate = Convert.ToDateTime(dtTime.Rows[i]["P_Date"].ToString());
                            CurDate = Convert.ToDateTime(dtTime.Rows[i]["C_Date"].ToString());
                        }
                    }

                    var Result = new { PrevDate = PrevDate, CurDate = CurDate };

                    return Json(new { success = true, data = Result }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, data = "" }, JsonRequestBehavior.AllowGet);
            }

        }


        public static void WriteLog(string Message)
        {
            string execPath = AppDomain.CurrentDomain.BaseDirectory;
            execPath = execPath + "Budgeting_Log\\log" + DateTime.Now.ToString("dd_MM_yyyy") + ".txt";
            StreamWriter file = new StreamWriter(execPath, append: true);
            file.WriteLine(Message + "\r\n");
            file.Close();
        }

    }




    /// <summary>
    /// template for email notification
    /// </summary>
    public partial class Emailnotify
    {
        public string ReviewLevel { get; set; }
        public bool is_ApprovalorSendback { get; set; }
        public int RequestID_foremail { get; set; }
        public Nullable<int> Count { get; set; }
        public Nullable<decimal> TotalAmount { get; set; }
        public string NTID_toEmail { get; set; }

        public string NTID_ccEmail { get; set; }
        public List<RequestItems_Table> Requests_foremail { get; set; }
        public string RFOApprover { get; set; }
        public string POSPOC_NTID { get; set; }
        public string RFOReqNTID { get; set; }
        public string getTOemail { get; set; }
        public string VKMSPOC_NTID { get; set; }
        

    }


    public partial class Emailnotify_OrderStage
    {

        public int is_RequesttoOrder { get; set; }
        public int RequestID_orderemail { get; set; }

        //for submit all feature
        public Nullable<int> Count { get; set; }
        public Nullable<decimal> TotalAmount { get; set; }
        public string NTID_ccEmail { get; set; }
        public DateTime SubmitDate_ofRequest { get; set; }

        public string RFOReqNTID { get; set; }
        public string getTOemail { get; set; }
        // public string SC_NTID { get; set; } //Section Coordinators (PO SPOCs)
        // public string RFOReqNTID { get; set; }
        public string GoodsRecipientID { get; set; }
        public string getCCemail { get; set; }
        public string POSPOC_NTID { get; set; }
        //If a new RFO Request is created and submitted, it is sent for approval to VKM SPOC ; mail needs to be triggered to the VKM SPOC in this case
        public string VKMSPOC_NTID { get; set; }


    }

    public partial class Emailnotify_RFOApprover
    {
        public int RequestID { get; set; }
        public string RFOApprover { get; set; }
        public string RFOApproverName { get; set; }
        public string Project { get; set; }
        public string ItemDescription { get; set; }
        public int Quantity { get; set; }
        public decimal TotalPrice { get; set; }
        public string Remarks { get; set; }
    }





    /// <summary>
    /// template for the Request table view
    /// </summary>
    public partial class RequestItemsRepoView
    {

        public string BudgetCode { get; set; }
        public bool isProjected { get; set; }
        public decimal Projected_Amount { get; set; }
        public bool Q1 { get; set; }
        public bool Q2 { get; set; }
        public bool Q3 { get; set; }
        public bool Q4 { get; set; }
        public decimal Unused_Amount { get; set; }
        public string RequestorNTID { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public string PORemarks { get; set; }
        public int VKM_Year { get; set; }

        public int RequestID { get; set; }
        public int BU { get; set; }
        public int DEPT { get; set; }
        public int Group { get; set; }
        public int OEM { get; set; }
        public int Item_Name { get; set; }
      
        public Nullable<int> Category { get; set; }
        public Nullable<int> Cost_Element { get; set; }
        public Nullable<decimal> Unit_Price { get; set; }

        public Nullable<int> Required_Quantity { get; set; }
        public Nullable<decimal> Total_Price { get; set; }
        public string Comments { get; set; }
        public string L3_Remarks { get; set; }
        public string Requestor { get; set; }
        public Nullable<int> Reviewed_Quantity { get; set; }
        public Nullable<decimal> Reviewed_Cost { get; set; }
        public Nullable<bool> ApprovedHoE { get; set; }
        public string Reviewer_1 { get; set; }
        public Nullable<bool> ApprovedSH { get; set; }
        public string Reviewer_2 { get; set; }
        public Nullable<bool> ApprovalHoE { get; set; }
        public Nullable<bool> ApprovalSH { get; set; }
        public string RequestDate { get; set; }
        public string SubmitDate { get; set; }
        public string Review1_Date { get; set; }
        public string Review2_Date { get; set; }
        public string OrderID { get; set; }
      
        public Nullable<int> OrderStatus { get; set; }

        public string RequiredDate { get; set; }
        public string RequestOrderDate { get; set; }
        public string OrderDate { get; set; }
        public string TentativeDeliveryDate { get; set; }
        public string ActualDeliveryDate { get; set; }
        public Nullable<int> Fund { get; set; }
        public Nullable<int> OrderedQuantity { get; set; }
        public Nullable<bool> RequestToOrder { get; set; }

        public string Request_Status { get; set; }
        public int isCancelled { get; set; }

        public string Customer_Name { get; set; }
        public string Resource_Group_Id { get; set; }
        public string Customer_Dept { get; set; }

        public string PIF_ID { get; set; }

        public string BM_Number { get; set; }
        public string Task_ID { get; set; }
        public string Project { get; set; }
        public string ActualAvailableQuantity { get; set; }

        public string HOEView_ActionHistory { get; set; }

        public int L2_Qty { get; set; }
        public string L2_Remarks { get; set; }
        public bool Is_UnplannedF02Item { get; set; }
        public string QuoteAvailable { get; set; }
        public string Quote_Vendor_Type { get; set; }
        public string Upload_File_Name { get; set; }
        public string RFOSubmittedDate { get; set; }
        public string ELOSubmittedDate { get; set; }
        public string DaysTaken { get; set; }
        public string SRSubmitted { get; set; }
        public string RFQNumber { get; set; }
        public string PONumber { get; set; }
        public string PRNumber { get; set; }
        public string POReleaseDate { get; set; }
        public string SRAwardedDate { get; set; }
        public string SRApprovalDays { get; set; }
        public string SRResponsibleBuyerNTID { get; set; }
        public string SRManagerNTID { get; set; }
        public string POSpocNTID { get; set; }

        public int UnitofMeasure { get; set; }
        public int UnloadingPoint { get; set; }
        public string BudgetCodeDescription { get; set; }
        public int OrderType { get; set; }
        public string CostCenter { get; set; }
        public int BudgetCenterID { get; set; }
        public string LabName { get; set; }
        public string RFOReqNTID { get; set; }
        public string RFOApprover { get; set; }
        public string GoodsRecID { get; set; }

        public string Material_Part_Number { get; set; }
        public string SupplierName_with_Address { get; set; }
        public int Purchase_Type { get; set; }
        public string Project_ID { get; set; }
        public string RequestSource { get; set; }
        public bool VKMSPOC_Approval { get; set; }
        public Nullable<int> OrderDescriptionID { get; set; }
        public string Description { get; set; }

        public string LinkedRequests { get; set; }
        public Nullable<int> Currency { get; set; }
        public Nullable<decimal> OrderPrice_UserInput { get; set; }
        public Nullable<decimal> SR_Value { get; set; }
        public Nullable<decimal> PR_Value { get; set; }
        public Nullable<decimal> Invoice_Value { get; set; }
        public Nullable<decimal> OrderPrice { get; set; }

     
        public string LinkedRequestID { get; set; }
    }





    /// <summary>
    /// template for the Item MasterList table view
    /// </summary>
    public partial class RequestItemsRepoView1
    {
        public int S_No { get; set; }
        public int VKMYear { get; set; }
        public string Item_Name { get; set; }
        public int Category { get; set; }
        public int Cost_Element { get; set; }
        public Nullable<decimal> Unit_Price { get; set; }
        public Nullable<decimal> Unit_PriceUSD { get; set; }
        public int Currency { get; set; }
        public int BU { get; set; }

        public string Comments { get; set; }
        public string Requestor { get; set; }
        public Nullable<bool> Deleted { get; set; }
        public Nullable<int> VendorCategory { get; set; }
        public string ActualAvailableQuantity { get; set; }
        public int BudgetCode { get; set; }
        public int Order_Type { get; set; }
        public int UOM { get; set; }
        public int Material_Group { get; set; }
        //public List<int> Material_Group { get; set; } - if this attribute is multiselect, then it should be List<int>


    }


    /// <summary>
    /// template for the Planning Stage Requestor table view
    /// </summary>
    public partial class VKMPlanningRequestorView
    {
        public int ID { get; set; }
        public string NTID { get; set; }
        public string FullName { get; set; }
        public int Department { get; set; }
        public int Group { get; set; }
        public string Proxy_NTID_EM { get; set; }
        public string Proxy_FullName_EM { get; set; }
        public string Updated_By { get; set; }

    }

    /// <summary>
    /// template for the Ordering Stage Requestor table view
    /// </summary>
    public partial class VKMOrderingRequestorView
    {
        public int ID { get; set; }
        public string NTID { get; set; }
        public string FullName { get; set; }
        public int Department { get; set; }
        public int Group { get; set; }
        public string Updated_By { get; set; }
    }

    public partial class VKMPlanning_HOEView
    {
        public int ID { get; set; }
        public string HOE_NTID { get; set; }
        public string HOE_FullName { get; set; }
        public int Department { get; set; }
        public string Proxy_NTID { get; set; }
        public string Active_NTID { get; set; }
        public string Proxy_FullName { get; set; }
        public string Updated_By { get; set; }
        public bool Enable_Proxy { get; set; }
    }

    public partial class VKMRoleView
    {
        public int ID { get; set; }
        public string SkillSetName { get; set; }

    }



    /// <summary>
    /// template for the Planning Stage Requestor table edit 
    /// </summary>
    public partial class VKMPlanningRequestorEdit
    {

        public int ID { get; set; }
        public string NTID { get; set; }
        public string FullName { get; set; }
        public int Department { get; set; }
        public int Group { get; set; }
        public string Proxy_NTID_EM { get; set; }
        public string Proxy_FullName_EM { get; set; }
        public string Updated_By { get; set; }

    }

    /// <summary>
    /// template for the Ordering Stage Requestor table edit 
    /// </summary>
    public partial class VKMOrderingRequestorEdit
    {
        public int ID { get; set; }
        public string NTID { get; set; }
        public string FullName { get; set; }
        public int Department { get; set; }
        public int Group { get; set; }
        public string Updated_By { get; set; }

    }


    public partial class VKMPlanning_HOEEdit
    {

        public int ID { get; set; }
        public string HOE_NTID { get; set; }
        public string HOE_FullName { get; set; }
        public int Department { get; set; }
        public string Proxy_NTID { get; set; }
        public string Proxy_FullName { get; set; }
        public string Updated_By { get; set; }
        public bool Enable_Proxy { get; set; }

    }

    public partial class VKMRoleEdit
    {

        public int ID { get; set; }
        public string SkillSetName { get; set; }


    }



    /// <summary>
    /// template for Summary view 
    /// </summary>
    public partial class SummaryView
    {
        public string Category { get; set; }
        public string AS { get; set; }
        public string OSS { get; set; }
        public string DA { get; set; }
        public string AD { get; set; }
        public string Two_Wheeler { get; set; }
        public string Totals { get; set; }
        //public string BEG { get; set; }
    }
    public partial class SummaryView_CC
    {
        public string Category { get; set; }
        public string AS { get; set; }
        public string OSS { get; set; }

        public string Two_Wheeler { get; set; }
        public string Totals { get; set; }
        //public string BEG { get; set; }
    }
    public partial class SummaryView_XC
    {
        public string Category { get; set; }

        public string DA { get; set; }
        public string AD { get; set; }
        public string Two_Wheeler { get; set; }
        public string Totals { get; set; }
        //public string BEG { get; set; }
    }


    public partial class BUSummary
    {
        public string BuName { get; set; }
        public decimal MAE_Totals { get; set; }
        public decimal NMAE_Totals { get; set; }
        public decimal Software_Totals { get; set; }
        public decimal Overall_Totals { get; set; }
    }


    public partial class BUSummary_CC
    {
        public string vkmyear { get; set; }
        public decimal AS_MAE_Totals { get; set; }
        public decimal AS_NMAE_Totals { get; set; }
        public decimal AS_Software_Totals { get; set; }
        public decimal AS_Overall_Totals { get; set; }
        public decimal OSS_MAE_Totals { get; set; }
        public decimal OSS_NMAE_Totals { get; set; }
        public decimal OSS_Software_Totals { get; set; }
        public decimal OSS_Overall_Totals { get; set; }

        public decimal TwoWP_MAE_Totals { get; set; }
        public decimal TwoWP_NMAE_Totals { get; set; }
        public decimal TwoWP_Software_Totals
        { get; set; }
        public decimal TwoWP_Overall_Totals
        { get; set; }


    }
    public partial class BUSummary_XC
    {
        public string vkmyear { get; set; }
        public decimal DA_MAE_Totals { get; set; }
        public decimal DA_NMAE_Totals { get; set; }
        public decimal DA_Software_Totals { get; set; }
        public decimal DA_Overall_Totals { get; set; }
        public decimal AD_MAE_Totals { get; set; }
        public decimal AD_NMAE_Totals { get; set; }
        public decimal AD_Software_Totals { get; set; }
        public decimal AD_Overall_Totals { get; set; }

        public decimal TwoWP_MAE_Totals { get; set; }
        public decimal TwoWP_NMAE_Totals { get; set; }
        public decimal TwoWP_Software_Totals
        { get; set; }
        public decimal TwoWP_Overall_Totals
        { get; set; }


    }


    public partial class BUSummary_CCXC
    {
        public string vkmyear { get; set; }
        public decimal AS_MAE_Totals { get; set; }
        public decimal AS_NMAE_Totals { get; set; }
        public decimal AS_Software_Totals { get; set; }
        public decimal AS_Overall_Totals { get; set; }
        public decimal OSS_MAE_Totals { get; set; }
        public decimal OSS_NMAE_Totals { get; set; }
        public decimal OSS_Software_Totals { get; set; }
        public decimal OSS_Overall_Totals { get; set; }

        public decimal DA_MAE_Totals { get; set; }
        public decimal DA_NMAE_Totals { get; set; }
        public decimal DA_Software_Totals { get; set; }
        public decimal DA_Overall_Totals { get; set; }
        public decimal AD_MAE_Totals { get; set; }
        public decimal AD_NMAE_Totals { get; set; }
        public decimal AD_Software_Totals { get; set; }
        public decimal AD_Overall_Totals { get; set; }

        public decimal TwoWP_MAE_Totals { get; set; }
        public decimal TwoWP_NMAE_Totals { get; set; }
        public decimal TwoWP_Software_Totals
        { get; set; }
        public decimal TwoWP_Overall_Totals
        { get; set; }


    }











    /// <summary>
    /// template for summary generator
    /// </summary>
    public partial class SummaryHelper
    {
        public List<DeptSummary> deptSummary = new List<DeptSummary>();
    }


    /// <summary>
    /// template for department summary
    /// </summary>
    public partial class DeptSummary
    {
        public string deptName { get; set; }
        public decimal MAE_Totals { get; set; }
        public decimal NMAE_Totals { get; set; }
        public decimal Software_Totals { get; set; }
        public decimal Overall_Totals { get; set; }
    }


    /// <summary>
    /// template for the pending queue summary table
    /// </summary>
    public partial class SummaryPendingHelper
    {
        public List<PendingSummary> deptSummary = new List<PendingSummary>();
    }

    public partial class PendingSummary
    {
        public string BUName { get; set; }
        public decimal L1_Totals { get; set; }
        public decimal L2_Totals { get; set; }
        public decimal L3_Totals { get; set; }
    }

    public class BudgetParam
    {
        public string jsondata { get; set; }
        public string columns { get; set; }
        public string data { get; set; }
    }
    public class columnsinfo
    {
        public string title { get; set; }
        public string data { get; set; }
    }


    /// <summary>
    /// template for edit item
    /// </summary>
       //public partial class RequestItemsRepoEdit1
    //{
    //    public string BudgetCode { get; set; }
    //    public bool isProjected { get; set; }
    //    public decimal Projected_Amount { get; set; }
    //    public bool Q1 { get; set; }
    //    public bool Q2 { get; set; }
    //    public bool Q3 { get; set; }
    //    public bool Q4 { get; set; }
    //    public decimal Unused_Amount { get; set; }
    //    public string RequestorNTID { get; set; }
    //    public Nullable<DateTime> UpdatedAt { get; set; }
    //    public string PORemarks { get; set; }
    //    public int VKM_Year { get; set; }
    //    public int RequestID { get; set; }

    //    public int BU { get; set; }

    //    public int DEPT { get; set; }

    //    public int Group { get; set; }

    //    public int OEM { get; set; }

    //    public int Item_Name { get; set; }

    //    public int Category { get; set; }

    //    public int Cost_Element { get; set; }
    //    public decimal Unit_Price { get; set; }

    //    public int Required_Quantity { get; set; }

    //    public decimal Total_Price { get; set; }

    //    public int Reviewed_Quantity { get; set; }

    //    public decimal Reviewed_Cost { get; set; }
    //    public string Comments { get; set; }
    //    public string Requestor { get; set; }
    //    public string Reviewer_1 { get; set; }
    //    public string Reviewer_2 { get; set; }
    //    public string RequestDate { get; set; }
    //    public string SubmitDate { get; set; }
    //    public string Review1_Date { get; set; }
    //    public string Review2_Date { get; set; }


    //    public string OrderID { get; set; }
    //    public string OrderStatus { get; set; }
    //    public decimal OrderPrice { get; set; }
    //    public string RequiredDate { get; set; }
    //    public string RequestOrderDate { get; set; }
    //    public string OrderDate { get; set; }
    //    public string TentativeDeliveryDate { get; set; }
    //    public string ActualDeliveryDate { get; set; }
    //    public int OrderedQuantity { get; set; }
    //    public bool RequestToOrder { get; set; }
    //    public bool ApprovalHoE { get; set; }
    //    public bool ApprovalSH { get; set; }
    //    public bool ApprovedHoE { get; set; }
    //    public bool ApprovedSH { get; set; }
    //    public int Fund { get; set; }

    //    public string Project { get; set; }
    //    public string ActualAvailableQuantity { get; set; }

    //    public string Customer_Name { get; set; }
    //    public string Customer_Dept { get; set; }
    //    public string BM_Number { get; set; }
    //    public string Task_ID { get; set; }
    //    public string Resource_Group_Id { get; set; }
    //    public string PIF_ID { get; set; }

    //    public string L2_Remarks { get; set; }
    //    public string L3_Remarks { get; set; }
    //    public int L2_Qty { get; set; }
    //    public bool Is_UnplannedF02Item { get; set; }

    //}

    public partial class RequestItemsRepoEdit1
    {
        public string BudgetCode { get; set; }
        public bool isProjected { get; set; }
        public decimal Projected_Amount { get; set; }
        public bool Q1 { get; set; }
        public bool Q2 { get; set; }
        public bool Q3 { get; set; }
        public bool Q4 { get; set; }
        public decimal Unused_Amount { get; set; }
        public string RequestorNTID { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public string PORemarks { get; set; }
        public int VKM_Year { get; set; }
        public int RequestID { get; set; }



        public int BU { get; set; }



        public int DEPT { get; set; }



        public int Group { get; set; }



        public int OEM { get; set; }



        public int Item_Name { get; set; }



        public int Category { get; set; }



        public int Cost_Element { get; set; }
        public decimal Unit_Price { get; set; }



        public int Required_Quantity { get; set; }



        public decimal Total_Price { get; set; }



        public int Reviewed_Quantity { get; set; }



        public decimal Reviewed_Cost { get; set; }
        public string Comments { get; set; }
        public string Requestor { get; set; }
        public string Reviewer_1 { get; set; }
        public string Reviewer_2 { get; set; }
        public string RequestDate { get; set; }
        public string SubmitDate { get; set; }
        public string Review1_Date { get; set; }
        public string Review2_Date { get; set; }




        public string OrderID { get; set; }
        public string OrderStatus { get; set; }
      
        public string RequiredDate { get; set; }
        public string RequestOrderDate { get; set; }
        public string OrderDate { get; set; }
        public string TentativeDeliveryDate { get; set; }
        public string ActualDeliveryDate { get; set; }
        public int OrderedQuantity { get; set; }
        public bool RequestToOrder { get; set; }
        public bool ApprovalHoE { get; set; }
        public bool ApprovalSH { get; set; }
        public bool ApprovedHoE { get; set; }
        public bool ApprovedSH { get; set; }
        public int Fund { get; set; }



        public string Project { get; set; }
        public string ActualAvailableQuantity { get; set; }



        public string Customer_Name { get; set; }
        public string Customer_Dept { get; set; }
        public string BM_Number { get; set; }
        public string Task_ID { get; set; }
        public string Resource_Group_Id { get; set; }
        public string PIF_ID { get; set; }



        public string L2_Remarks { get; set; }
        public string L3_Remarks { get; set; }
        public int L2_Qty { get; set; }
        public bool Is_UnplannedF02Item { get; set; }

        public string RFOReqNTID { get; set; }
        public string GoodsRecID { get; set; }
        public string BudgetCenterID { get; set; }
        public string UnloadingPoint { get; set; }
        public string RFOApprover { get; set; }
        public string BudgetCodeDescription { get; set; }
        public string UnitofMeasure { get; set; }
        public string QuoteAvailable { get; set; }
        public string LabName { get; set; }
        public string OrderType { get; set; }
        public string CostCenter { get; set; }
        public string RFOSubmittedDate { get; set; }
        public string ELOSubmittedDate { get; set; }
        public string DaysTaken { get; set; }
        public string SRSubmitted { get; set; }
        public string RFQNumber { get; set; }
        public string PONumber { get; set; }
        public string PRNumber { get; set; }
        public string POReleaseDate { get; set; }
        public string SRAwardedDate { get; set; }
        public string SRApprovalDays { get; set; }
        public string SRResponsibleBuyerNTID { get; set; }
        public string SRManagerNTID { get; set; }
        public string POSpocNTID { get; set; }
        public string HOEView_ActionHistory { get; set; }
        public List<OrderTypeBGSW> BudgetcenterList { get; set; } //to populate init values of budgetcenter list
        public string Material_Part_Number { get; set; }
        public string SupplierName_with_Address { get; set; }
        public string Purchase_Type { get; set; }
        public string Project_ID { get; set; }
        public string Quote_Vendor_Type { get; set; }
        public string Upload_File_Name { get; set; }
        public string RequestSource { get; set; }
        public string Description { get; set; }
        public int Currency { get; set; }
        public Nullable<decimal> OrderPrice_UserInput { get; set; }
        public Nullable<decimal> SR_Value { get; set; }
        public Nullable<decimal> PR_Value { get; set; }
        public Nullable<decimal> Invoice_Value { get; set; }
        public Nullable<decimal> OrderPrice { get; set; }
    }


    public partial class HeaderFilter_Table
    {
        public string text { get; set; }
        public int value { get; set; }
    }

    public class LookupData
    {
        //public List<ItemsCostList_Table> Item_FilterRow{ get; set; }
        public List<HeaderFilter_Table> Item_HeaderFilter { get; set; }
        public List<HeaderFilter_Table> BU_HeaderFilter { get; set; }
        public List<HeaderFilter_Table> DEPT_HeaderFilter { get; set; }
        public List<HeaderFilter_Table> Group_HeaderFilter { get; set; }
        public List<HeaderFilter_Table> OEM_HeaderFilter { get; set; }
        public List<HeaderFilter_Table> Category_HeaderFilter { get; set; }
        public List<HeaderFilter_Table> CostElement_HeaderFilter { get; set; }
        public List<HeaderFilter_Table> OrderStatus_HeaderFilter { get; set; }
        public List<HeaderFilter_Table> BudgetCode_HeaderFilter { get; set; }
        public List<HeaderFilter_Table> OrderDescription_HeaderFilter { get; set; }

        public List<BU_Table> BU_List { get; set; }
        public List<OEM_Table> OEM_List { get; set; }
        public List<DEPT_Table> DEPT_List { get; set; }
        public List<Groups_Table> Groups_List { get; set; }
        public List<Groups_Table_Aug> Groups_oldList { get; set; }
        public List<Groups_Table_Test> Groups_test { get; set; }

        public List<CostElement_Table> CostElement_List { get; set; }
        public List<Category_Table> Category_List { get; set; }
        public List<ItemsCostList_Table> Item_List { get; set; }
        public List<Currency_Table> Currency_List { get; set; }
        public List<Order_Status_Table> OrderStatus_List { get; set; }

        public List<OrderStatusDescription> OrderDescription_List { get; set; }

        public List<LeadTime_Table> VendorCategory_List { get; set; }
        public List<Fund_Table> Fund_List { get; set; }

        public List<BudgetCodeMaster> BudgetCodeList { get; set; }
        public List<Order_Type_list> Order_Type_List { get; set; }
        public List<Material_Group_list> Material_Group_list { get; set; }
        public List<UOM_list> UOM_List { get; set; }
        public List<BGSW_F05F06_PurchaseType_Table> PurchaseType_List { get; set; }
        public List<SR_Responsible_Buyer_table> SRBuyerList { get; set; }
        public List<SR_Responsible_Manager_table> SRManagerList { get; set; }
        public List<UnloadingPointBGSW> UnloadingPoint_List { get; set; }
        public List<RFOApprover> RFOApprover_List { get; set; }
        public List<CostCenterBGSW> CostCenter_List { get; set; }
        public List<BGSW_BudgetCenter_Table> BudgetCenter_List { get; set; }


    }

    public partial class ReqList
    {
        public string LinkedRequestID { get; set; }
        public bool isSelected { get; set; }
    }



}
