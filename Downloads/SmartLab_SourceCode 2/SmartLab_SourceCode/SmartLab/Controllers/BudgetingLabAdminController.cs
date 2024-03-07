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
using System.Data.OleDb;
using System.Threading;
using System.Web;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Data.SqlClient;

namespace LC_Reports_V1.Controllers
{

    //[Authorize(Users = @"apac\jov6cob,apac\rba3cob,apac\din2cob,apac\MTA2COB,apac\muu4cob,apac\nnj6kor,apac\pks5cob,apac\chb1kor,apac\sbr2kor,apac\rau2kor,apac\bbv5kor,apac\rmm7kor,apac\gpa3kor,apac\mae9cob,apac\oig1cob,apac\rma5cob,apac\mxk8kor,apac\SIF1COB")]

    public class BudgetingLabAdminController : Controller
    {
        private SqlConnection budgetingcon;
        // GET: RequestItems
        public ActionResult Index()
        {
            BudgetingController.InitialiseBudgeting(); //to detect changes made in in masterlists
            if (BudgetingController.lstUsers == null || BudgetingController.lstUsers.Count == 0)
            {

                return RedirectToAction("Index", "Budgeting");
            }


            return View();
        }

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

        /// <summary>
        /// function to fetch the Request Items made by the Requestor during the year chosen for view
        /// </summary>
        public ActionResult GetData_VKMPlanning()
        {


            using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

            //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

            {
                try
                {

                    List<VKMPlanningRequestorView> viewList = new List<VKMPlanningRequestorView>();
                    viewList = GetData1_VKMPlanning();

                    return Json(new
                    {

                        data = viewList
                    }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(new { success = true, message = "Unable to load the Planning Stage Requestor List, Please Try again later!" }, JsonRequestBehavior.AllowGet);

                }

            }
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
                query = " select VKMspoc as NTID from BU_SPOCS where VKMspoc = '" + User.Identity.Name.Split('\\')[1].ToUpper() + "' and BU in (1,2,3,4,5) ";
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


        public List<VKMPlanningRequestorView> GetData1_VKMPlanning()
        {
            //string PresentUserDept_RequestTable = BudgetingController.lstDEPTs.Find(dept => dept.DEPT.Equals(presentUserDept)).ID.ToString();


            using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

            //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

            {

                //System.Security.Principal.IPrincipal User = System.Web.HttpContext.Current.User;
                // string presentUserName = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).EmployeeName;
                List<Planning_EM_Table> reqList = new List<Planning_EM_Table>();
                //reqList = db.Planning_EM_Table.ToList();
                reqList = db.Planning_EM_Table.Where(x => x.Year == DateTime.Now.Year).ToList();
                List<VKMPlanningRequestorView> viewList = new List<VKMPlanningRequestorView>();
                foreach (Planning_EM_Table item in reqList)
                {
                    VKMPlanningRequestorView ritem = new VKMPlanningRequestorView();


                    ritem.Department = int.Parse(item.Department);
                    ritem.Group = int.Parse(item.Group);
                    ritem.ID = item.ID;
                    ritem.NTID = item.NTID;
                    ritem.FullName = item.FullName;
                    ritem.Updated_By = item.Updated_By;
                    ritem.Proxy_NTID_EM = item.Proxy_NTID;
                    ritem.Proxy_FullName_EM = item.Proxy_FullName;
                    

                    viewList.Add(ritem);
                }
                return viewList;

            }
        }


        ///// <summary>
        ///// function to enable update Planning Requestor details and add a new entry of Requestor
        ///// </summary>
        ///// <param name="req"></param>
        ///// <returns></returns>
        [HttpPost]
        public ActionResult AddOrEdit_VKMPlanning(VKMPlanningRequestorEdit req)
        {
            List<VKMPlanningRequestorEdit> viewList = new List<VKMPlanningRequestorEdit>();


            using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

            //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

            {
                System.Security.Principal.IPrincipal User = System.Web.HttpContext.Current.User;
                string presentUserName = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).EmployeeName;
                var EM_Table = db.Planning_EM_Table.AsNoTracking().ToList();
                //Validate if this is a new entry - the details of which is already present

                //duplicate entry check
                if (req.ID == 0)
                {
                    //if ((EM_Table.Find(x => x.NTID.ToUpper().Trim() == req.NTID.ToUpper().Trim()) != null) &&
                    //        (EM_Table.Find(x => x.NTID.ToUpper().Trim() == req.NTID.ToUpper().Trim()).Year == DateTime.Now.Year))
                    if ((EM_Table.Find(x => x.NTID.ToUpper().Trim() == req.NTID.ToUpper().Trim()) != null) &&
                          (EM_Table.Find(x => x.NTID.ToUpper().Trim() == req.NTID.ToUpper().Trim() && x.Year == DateTime.Now.Year) != null))
                    {
                        return Json(new
                        {
                            success = false,
                            message = "The Requestor details for " + req.NTID + " are already present in the EM List. Please check again! "
                        }, JsonRequestBehavior.AllowGet);

                    }
                }
                else
                {
                        if ((EM_Table.Find(x => x.NTID.ToUpper().Trim() == req.NTID.ToUpper().Trim()) != null && req.ID != EM_Table.Find(x => x.NTID.ToUpper().Trim() == req.NTID.ToUpper().Trim()).ID) &&
                            (EM_Table.Find(x => x.NTID.ToUpper().Trim() == req.NTID.ToUpper().Trim()).Year == DateTime.Now.Year))
                        {
                            return Json(new
                            {
                                success = false,
                                message = "The Requestor details for " + req.NTID + " are already present in the EM List. Please check again! "
                            }, JsonRequestBehavior.AllowGet);
                        }

                }
                
                   




                Planning_EM_Table item = new Planning_EM_Table();

                if (BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToLower().Equals(req.NTID.Trim().ToLower())) != null)
                {
                    item.Department = req.Department.ToString();
                    item.Group = req.Group.ToString();
                    item.FullName = req.FullName;
                    item.ID = req.ID;
                    item.NTID = req.NTID.ToUpper();
                    item.Proxy_FullName = req.Proxy_FullName_EM;
                    if (req.Proxy_NTID_EM != null)
                        item.Proxy_NTID = req.Proxy_NTID_EM.ToUpper();
                    else
                        item.Proxy_NTID = req.Proxy_NTID_EM;
                    item.Updated_By = presentUserName;
                    item.Year = DateTime.Now.Year;

                    if (req.ID == 0)
                    {

                        db.Planning_EM_Table.Add(item);
                        db.SaveChanges();
                        BudgetingController.lstEMs = db.Planning_EM_Table.ToList();


                        //viewList = GetData1(useryear);


                        return Json(new { success = true,/*, data = viewList,*/ message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);

                    }
                    else
                    {


                        db.Entry(item).State = EntityState.Modified;

                        db.SaveChanges();
                        BudgetingController.lstEMs = db.Planning_EM_Table.ToList();

                        //viewList = GetData1(useryear);

                        return Json(new { success = true, message = "Updated Successfully" }, JsonRequestBehavior.AllowGet);

                    }
                }
                else
                {
                    return Json(new { success = false, message = "The NTID is not valid, Please check again!" }, JsonRequestBehavior.AllowGet);
                }
            }


        }

        ///// <summary>
        ///// function to validate if the NTID input is already presnt in the List/not - Planning Stage
        ///// </summary>
        ///// <param name="NTID"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public ActionResult ValidateNTID_ifalready_exist_Planning(string NTID)
        //{

        //    using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //    //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //    {

        //        if (db.Planning_EM_Table.ToList().Find(x=>x.NTID.ToUpper().Trim() == NTID.ToUpper().Trim()) == null)
        //        {

        //            return Json(new { success = true, message = "" }, JsonRequestBehavior.AllowGet);

        //        }
        //        else
        //        {
        //            return Json(new { success = false, message = "The Requestor details are already present in the Planning Stage Requestor List, Please check again!" }, JsonRequestBehavior.AllowGet);
        //        }
        //    }


           
        //}


        /// <summary>
        /// function to delete an existing item
        /// </summary>
        /// <param name="ntid"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete_VKMPlanning(string ntid)
        {
            List<VKMPlanningRequestorView> viewList = new List<VKMPlanningRequestorView>();


            using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

            //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

            {
                Planning_EM_Table item = db.Planning_EM_Table.Where(x => x.NTID == ntid && x.Year == DateTime.Now.Year).FirstOrDefault<Planning_EM_Table>();
                db.Planning_EM_Table.Remove(item);
                db.SaveChanges();
                BudgetingController.lstEMs = db.Planning_EM_Table.ToList();
                viewList = GetData1_VKMPlanning();
                return Json(new { data = viewList, success = true, message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Post method for importing Planning users 
        /// </summary>
        /// <param name="postedFile"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Index(HttpPostedFileBase postedFile)
        {
            string exceptionmsg = "";
            if (postedFile != null)
            {
                try
                {
                    string fileExtension = Path.GetExtension(postedFile.FileName);

                    //Validate uploaded file and return error.
                    if (fileExtension != ".xls" && fileExtension != ".xlsx")
                    {

                        return Json(new { errormsg = "Please select the excel file with.xls or.xlsx extension" });

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
                                DataTable excelSchema = GetSchemaFromExcel(excelOledbConnection);
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
                    }


                    int errcount = 0;
                    string msg = "";

                    using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

                    //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

                    {
                        System.Security.Principal.IPrincipal User = System.Web.HttpContext.Current.User;
                        string presentUserName = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).EmployeeName;

                        //Loop through datatable and add data to table. 
                        foreach (DataRow row in dt.Rows)
                        {
                            Planning_EM_Table item = new Planning_EM_Table();

                            try
                            {

                                if (row[0] == DBNull.Value || String.IsNullOrWhiteSpace(row[0].ToString()))
                                {
                                    continue;
                                }
                                else
                                {

                                    //if (String.IsNullOrWhiteSpace(row[0].ToString()) || row[0] == DBNull.Value)
                                    //{
                                    //    errcount++;
                                    //    msg += "Please enter Item Name";
                                    //    continue;

                                    //}
                                    if (BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToLower().Equals(row[0].ToString().Trim().ToLower())) != null)
                                    {
                                        var ntid = row[0].ToString();
                                        item.NTID = ntid.ToUpper();
                                        item.FullName = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToLower().Equals(row[0].ToString().Trim().ToLower())).EmployeeName;
                                        //var dptname = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToLower().Equals(row[0].ToString().Trim().ToLower())).Department;
                                        //var grpname = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToLower().Equals(row[0].ToString().Trim().ToLower())).Group;
                                        DEPT_Table dEPT_Table = BudgetingController.lstDEPTs.Find(departmnt => departmnt.DEPT.Equals(BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToLower().Equals(row[0].ToString().Trim().ToLower())).Department));
                                        item.Department = dEPT_Table.ID.ToString();
                                        var ReqGroup = BudgetingController.lstUsers.Find(x => x.NTID.Trim().ToUpper() == row[0].ToString().Trim().ToUpper()).Group;
                                        Groups_Table_Test gROUP_Table = BudgetingController.lstGroups_test.Find(grp => grp.Group.Trim().Equals(ReqGroup));
                                        item.Group = gROUP_Table.ID.ToString();
                                        item.Updated_By = presentUserName;
                                        item.Year = DateTime.Now.Year;

                                        if (row[1] != DBNull.Value && !String.IsNullOrWhiteSpace(row[1].ToString()))
                                        {
                                            if (BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToLower().Equals(row[1].ToString().Trim().ToLower())) != null)
                                            {
                                                var proxy_ntid = row[1].ToString();
                                                item.Proxy_NTID = proxy_ntid.ToUpper();
                                                item.Proxy_FullName = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToLower().Equals(row[1].ToString().Trim().ToLower())).EmployeeName;
                                            }
                                            else
                                            {
                                                errcount++;
                                                if (errcount > 1)
                                                {
                                                    msg += " | \n" + "The EM's Proxy NTID: " + row[1].ToString() + " is invalid. Please check again! ";

                                                }
                                                else
                                                    msg += "The EM's Proxy NTID: " + row[1].ToString() + " is invalid. Please check again! ";


                                                continue;
                                            }


                                        }

                                        if (db.Planning_EM_Table.AsNoTracking().ToList().Find(x => x.NTID.ToUpper().Trim() == ntid.ToUpper().Trim()) == null)
                                        {
                                            db.Planning_EM_Table.Add(item);
                                        }
                                        else if (db.Planning_EM_Table.AsNoTracking().ToList().Find(x => x.NTID.ToUpper().Trim() == ntid.ToUpper().Trim() && x.Year == DateTime.Now.Year) == null)
                                        {
                                            db.Planning_EM_Table.Add(item);

                                        }                                     
                                        else
                                        {
                                            errcount++;
                                            if (errcount > 1)
                                                msg += " | \n" + "The Requestor details for " + row[0].ToString() + " are already present in the Planning Stage Requestor List. Please check again! ";
                                            else
                                                msg += "The Requestor details for " + row[0].ToString() + " are already present in the Planning Stage Requestor List. Please check again! ";
                                            continue;
                                        }


                                    }
                                    else
                                    {
                                        errcount++;
                                        if (errcount > 1)
                                        {
                                            msg +=  " | \n" + "The NTID: " + row[0].ToString() + " is invalid. Please check again! ";

                                        }
                                        else
                                            msg += "The NTID: " + row[0].ToString() + " is invalid. Please check again! ";
                                        continue;
                                    }

                                   
                                }

                                int milliseconds = 500;
                                Thread.Sleep(milliseconds);
                                db.SaveChanges();
                                BudgetingController.lstEMs = db.Planning_EM_Table.ToList();
                            }
                            catch (Exception ex)
                            {
                                errcount++;

                                if (errcount > 1)
                                    msg += " | \n" + "Empty/invalid cell found :" + row[0].ToString();
                                else
                                    msg += "Empty/invalid cell found :" + row[0].ToString();


                            }

                        }
                    }


                    if (errcount > 0)
                    {
                        return Json(new { dataerror = true, errormsg = msg + " \nThe valid details were imported. Please find the errors listed. " });
                    }



                    else
                    {
                        return Json(new { dataerror = false, successmsg = " The details were successfully imported. Please edit any INVALID fields in the table." });
                    }





                }
                catch (Exception ex)
                {

                    exceptionmsg += ex.Message;
                    return Json(new { errormsg = exceptionmsg });
                }

            }
            else
            {

                return Json(new { dataerror = true, errormsg = "Please select the file to be uploaded." });

            }


        }

        public ActionResult DownloadTemplate()
        {
            string folderPath = Server.MapPath("~/Templates/");
            string fileName = "VKMEM_ProxyList.xlsx";
            byte[] fileBytes = System.IO.File.ReadAllBytes(folderPath + fileName);

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }





        //*****VKM Ordering Stage Operations*****//

        /// <summary>
        /// function to fetch the Request Items made by the Requestor during the year chosen for view
        /// </summary>
        public ActionResult GetData_VKMOrdering()
        {


            using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

            //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

            {
                try
                {

                    List<VKMOrderingRequestorView> viewList = new List<VKMOrderingRequestorView>();
                    viewList = GetData1_VKMOrdering();

                    return Json(new
                    {

                        data = viewList
                    }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(new { success = true, message = "Unable to load the Planning Stage Requestor List, Please Try again later!" }, JsonRequestBehavior.AllowGet);

                }

            }
        }


        public List<VKMOrderingRequestorView> GetData1_VKMOrdering()
        {
            //string PresentUserDept_RequestTable = BudgetingController.lstDEPTs.Find(dept => dept.DEPT.Equals(presentUserDept)).ID.ToString();


            using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

            //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

            {

                //System.Security.Principal.IPrincipal User = System.Web.HttpContext.Current.User;
                // string presentUserName = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).EmployeeName;
                List<OrderingRequestor_Table> reqList = new List<OrderingRequestor_Table>();
                reqList = db.OrderingRequestor_Table.ToList();
                List<VKMOrderingRequestorView> viewList = new List<VKMOrderingRequestorView>();
                foreach (OrderingRequestor_Table item in reqList)
                {
                    VKMOrderingRequestorView ritem = new VKMOrderingRequestorView();


                    ritem.Department = int.Parse(item.Department);
                    ritem.Group = int.Parse(item.Group);
                    ritem.ID = item.ID;
                    ritem.NTID = item.NTID;
                    ritem.FullName = item.FullName;
                    ritem.Updated_By = item.Updated_By;

                    viewList.Add(ritem);
                }
                return viewList;

            }
        }


        ///// <summary>
        ///// function to enable update Planning Requestor details and add a new entry of Requestor
        ///// </summary>
        ///// <param name="req"></param>
        ///// <returns></returns>
        [HttpPost]
        public ActionResult AddOrEdit_VKMOrdering(VKMOrderingRequestorEdit req)
        {
            List<VKMOrderingRequestorEdit> viewList = new List<VKMOrderingRequestorEdit>();


            using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

            //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

            {
                System.Security.Principal.IPrincipal User = System.Web.HttpContext.Current.User;
                string presentUserName = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).EmployeeName;

                //Validate if this is a new entry - the details of which is already present

                if (req.ID == 0) //new entry
                {
                    if (db.OrderingRequestor_Table.ToList().Find(x => x.NTID.ToUpper().Trim() == req.NTID.ToUpper().Trim()) != null)
                    {
                        return Json(new { success = false, message = "The Requestor details for " + req.NTID + " are already present in the Ordering Stage Requestor List. Please check again! "
                        }, JsonRequestBehavior.AllowGet);

                    }
                }


                OrderingRequestor_Table item = new OrderingRequestor_Table();

                if (BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToLower().Equals(req.NTID.Trim().ToLower())) != null)
                {
                    item.Department = req.Department.ToString();
                    item.Group = req.Group.ToString();
                    item.FullName = req.FullName;
                    item.ID = req.ID;
                    item.NTID = req.NTID;
                    item.Updated_By = presentUserName;

                    if (req.ID == 0)
                    {

                        db.OrderingRequestor_Table.Add(item);
                        db.SaveChanges();


                        //viewList = GetData1(useryear);


                        return Json(new { success = true,/*, data = viewList,*/ message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);

                    }
                    else
                    {


                        db.Entry(item).State = EntityState.Modified;

                        db.SaveChanges();

                        //viewList = GetData1(useryear);

                        return Json(new { success = true, message = "Updated Successfully" }, JsonRequestBehavior.AllowGet);

                    }
                }
                else
                {
                    return Json(new { success = false, message = "The NTID is not valid, Please check again!" }, JsonRequestBehavior.AllowGet);
                }
            }


        }


        ///// <summary>
        ///// function to validate if the NTID input is already presnt in the List/not - Planning Stage
        ///// </summary>
        ///// <param name="NTID"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public ActionResult ValidateNTID_ifalready_exist_Ordering(string NTID)
        //{

        //    using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //    //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //    {
        //        if (db.OrderingRequestor_Table.Where(x => x.NTID.Trim().Contains(NTID.Trim())) == null)
        //        {

        //            return Json(new { success = true, message = "" }, JsonRequestBehavior.AllowGet);

        //        }
        //        else
        //        {
        //            return Json(new { success = false, message = "The Requestor details are already present in the Ordering Stage Requestor List, Please check again!" }, JsonRequestBehavior.AllowGet);
        //        }
        //    }



        //}


        /// <summary>
        /// function to delete an existing item
        /// </summary>
        /// <param name="ntid"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete_VKMOrdering(string ntid)
        {
            List<VKMOrderingRequestorView> viewList = new List<VKMOrderingRequestorView>();


            using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

            //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

            {
                OrderingRequestor_Table item = db.OrderingRequestor_Table.Where(x => x.NTID.Trim().Contains(ntid)).FirstOrDefault<OrderingRequestor_Table>();
                db.OrderingRequestor_Table.Remove(item);
                db.SaveChanges();
                viewList = GetData1_VKMOrdering();
                return Json(new { data = viewList, success = true, message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
            }
        }



        /// <summary>
        /// Post method for importing Ordering users 
        /// </summary>
        /// <param name="postedFile1"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Index_OrderingStage(HttpPostedFileBase postedFileRFO)
        {
            string exceptionmsg = "";
            if (postedFileRFO != null)
            {
                try
                {
                    string fileExtension = Path.GetExtension(postedFileRFO.FileName);

                    //Validate uploaded file and return error.
                    if (fileExtension != ".xls" && fileExtension != ".xlsx")
                    {

                        return Json(new { errormsg = "Please select the excel file with.xls or.xlsx extension" });

                    }

                    string folderPath = Server.MapPath("~/UploadedFiles/");
                    //Check Directory exists else create one
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    //Save file to folder
                    var filePath = folderPath + Path.GetFileName(postedFileRFO.FileName);
                    postedFileRFO.SaveAs(filePath);

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
                                DataTable excelSchema = GetSchemaFromExcel(excelOledbConnection);
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
                    }


                    int errcount = 0;
                    string msg = "";

                    using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

                    //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

                    {
                        System.Security.Principal.IPrincipal User = System.Web.HttpContext.Current.User;
                        string presentUserName = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).EmployeeName;

                        //Loop through datatable and add data to table. 
                        foreach (DataRow row in dt.Rows)
                        {
                            OrderingRequestor_Table item = new OrderingRequestor_Table();
                            try
                            {

                                if (row[0] == DBNull.Value || String.IsNullOrWhiteSpace(row[0].ToString()))
                                {
                                    continue;
                                }
                                else
                                {

                                    //if (String.IsNullOrWhiteSpace(row[0].ToString()) || row[0] == DBNull.Value)
                                    //{
                                    //    errcount++;
                                    //    msg += "Please enter Item Name";
                                    //    continue;

                                    //}

                                    //since if want to switch to 2020 ordering, user ssould be in 2020 list
                                    //if (BudgetingController.lstUsers_2020.FirstOrDefault(user => user.NTID.Trim().ToLower().Equals(row[0].ToString().Trim().ToLower())) != null )//||
                                    //   //BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToLower().Equals(row[0].ToString().Trim().ToLower())) != null)
                                    //{
                                    //    var ntid = row[0].ToString();
                                    //    item.NTID = ntid.ToUpper();
                                    //    //row[2] -> dept table -> get id
                                    //    //item.Department = BudgetingController.lstDEPTs.Find(departmnt => departmnt.DEPT.Equals(row[1].ToString())).ID.ToString();
                                    //    //item.Group = BudgetingController.lstGroups_test.Find(grp => grp.Group.Equals(row[2].ToString())).ID.ToString();


                                    //    item.FullName = BudgetingController.lstUsers_2020.FirstOrDefault(user => user.NTID.Trim().ToLower().Equals(row[0].ToString().Trim().ToLower())).EmployeeName;

                                    //    DEPT_Table dEPT_Table = BudgetingController.lstDEPTs.Find(departmnt => departmnt.DEPT.Equals(BudgetingController.lstUsers_2020.FirstOrDefault(user => user.NTID.Trim().ToLower().Equals(row[0].ToString().Trim().ToLower())).Department));
                                    //    item.Department = dEPT_Table.ID.ToString();
                                    //    var ReqGroup = BudgetingController.lstUsers_2020.Find(x => x.NTID.Trim().ToUpper() == row[0].ToString().Trim().ToUpper()).Group;
                                    //    Groups_Table_Test gROUP_Table = BudgetingController.lstGroups_test.Find(grp => grp.Group.Trim().Equals(ReqGroup));
                                    //    item.Group = gROUP_Table.ID.ToString();



                                    //    //item.Department = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToLower().Equals(row[0].ToString().Trim().ToLower())).Department;
                                    //    //item.Group = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToLower().Equals(row[0].ToString().Trim().ToLower())).Group;
                                    //    item.Updated_By = presentUserName;

                                    //    if (db.OrderingRequestor_Table.ToList().Find(x => x.NTID.ToUpper().Trim() == ntid.ToUpper().Trim()) == null)
                                    //    {
                                    //        db.OrderingRequestor_Table.Add(item);
                                    //    }
                                    //    else
                                    //    {
                                    //        errcount++;

                                    //        if (errcount > 1)
                                    //            msg += " | \n" + "The Requestor details for " + row[0].ToString() + " are already present in the Ordering Stage Requestor List. Please check again! ";
                                    //        else
                                    //            msg += "The Requestor details for " + row[0].ToString() + " are already present in the Ordering Stage Requestor List. Please check again! ";
                                    //        continue;
                                           
                                    //    }

                                           
                                    //}
                                    //else
                                    
                                    if (BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToLower().Equals(row[0].ToString().Trim().ToLower())) != null)//||
                                                                                                                                                                                //BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToLower().Equals(row[0].ToString().Trim().ToLower())) != null)
                                    {
                                        var ntid = row[0].ToString();
                                        item.NTID = ntid.ToUpper();
                                        //row[2] -> dept table -> get id
                                        //item.Department = BudgetingController.lstDEPTs.Find(departmnt => departmnt.DEPT.Equals(row[1].ToString())).ID.ToString();
                                        //item.Group = BudgetingController.lstGroups_test.Find(grp => grp.Group.Equals(row[2].ToString())).ID.ToString();


                                        item.FullName = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToLower().Equals(row[0].ToString().Trim().ToLower())).EmployeeName;

                                        DEPT_Table dEPT_Table = BudgetingController.lstDEPTs.Find(departmnt => departmnt.DEPT.Equals(BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToLower().Equals(row[0].ToString().Trim().ToLower())).Department));
                                        item.Department = dEPT_Table.ID.ToString();
                                        var ReqGroup = BudgetingController.lstUsers.Find(x => x.NTID.Trim().ToUpper() == row[0].ToString().Trim().ToUpper()).Group;
                                        Groups_Table_Test gROUP_Table = BudgetingController.lstGroups_test.Find(grp => grp.Group.Trim().Equals(ReqGroup));
                                        item.Group = gROUP_Table.ID.ToString();



                                        //item.Department = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToLower().Equals(row[0].ToString().Trim().ToLower())).Department;
                                        //item.Group = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToLower().Equals(row[0].ToString().Trim().ToLower())).Group;
                                        item.Updated_By = presentUserName;

                                        if (db.OrderingRequestor_Table.ToList().Find(x => x.NTID.ToUpper().Trim() == ntid.ToUpper().Trim()) == null)
                                        {
                                            db.OrderingRequestor_Table.Add(item);
                                        }
                                        else
                                        {
                                            errcount++;

                                            if (errcount > 1)
                                                msg += " | \n" + "The Requestor details for " + row[0].ToString() + " are already present in the Ordering Stage Requestor List. Please check again! ";
                                            else
                                                msg += "The Requestor details for " + row[0].ToString() + " are already present in the Ordering Stage Requestor List. Please check again! ";
                                            continue;

                                        }


                                    }
                                    else
                                    {
                                        errcount++;
                                        if (errcount > 1)
                                        {
                                            msg += " | \n" + "The NTID: " + row[0].ToString() + " is invalid. Please check again! ";

                                        }
                                        else
                                            msg += "The NTID: " + row[0].ToString() + " is invalid. Please check again! ";

                                        
                                        continue;
                                    }


                                }

                                int milliseconds = 500;
                                Thread.Sleep(milliseconds);
                                db.SaveChanges();
                            }
                            catch (Exception ex)
                            {
                                errcount++;

                                if (errcount > 1)
                                    msg += " | \n" + "Empty/invalid cell found :" + row[0].ToString();
                                else
                                    msg += "Empty/invalid cell found :" + row[0].ToString();


                            }

                        }
                    }


                    if (errcount > 0)
                    {
                        return Json(new { dataerror = true, errormsg = msg + " \nThe valid details were imported. Please find the errors listed. " });
                    }



                    else
                    {
                        return Json(new { dataerror = false, successmsg = " The details were successfully imported. Please edit any INVALID fields in the table." });
                    }





                }
                catch (Exception ex)
                {

                    exceptionmsg += ex.Message;
                    return Json(new { errormsg = exceptionmsg });
                }

            }
            else
            {

                return Json(new { dataerror = true, errormsg = "Please select the file to be uploaded." });

            }


        }

        public ActionResult DownloadTemplate_Ordering()
        {
            string folderPath = Server.MapPath("~/Templates/");
            string fileName = "VKMOrderingStage_Requestors.xlsx";
            byte[] fileBytes = System.IO.File.ReadAllBytes(folderPath + fileName);

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }










        ///// <summary>
        ///// Function to help the export to excel function 
        ///// Path to export - default or input from User
        ///// Feedback to user after saving
        ///// </summary>
        //public ActionResult ExportDataToExcel()
        //{

        //    string filename = @"Order_List_" + DateTime.Now.ToShortDateString() + ".xlsx";

        //    using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //    //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //    {
        //        System.Security.Principal.IPrincipal User = System.Web.HttpContext.Current.User;
        //        string presentUserName = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).EmployeeName;
        //        string presentUserDept = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).Department;

        //        System.Data.DataTable dt = new System.Data.DataTable("Request_List");
        //        dt.Columns.AddRange(new DataColumn[29] { new DataColumn("Business Unit"),
        //                                    new DataColumn("OEM"),
        //                                    new DataColumn("Department"),
        //                                    new DataColumn("Group"),
        //                                    new DataColumn("Item Name"),
        //                                    new DataColumn("Category"),
        //                                    new DataColumn("Cost Element"),
        //                                    new DataColumn("Unit Price",typeof(decimal)),
        //                                    new DataColumn("Required Quantity",typeof(int)),
        //                                    new DataColumn("Total Price",typeof(decimal)),
        //                                    new DataColumn("Reviewed Quantity",typeof(Int32)),
        //                                    new DataColumn("Reviewed Price",typeof(decimal)),
        //                                    new DataColumn("Comments"),
        //                                    new DataColumn("Requestor"),
        //                                    new DataColumn("Submit Date"),
        //                                    new DataColumn("Reviewer 1"),
        //                                    new DataColumn("Review 1 Date"),
        //                                    new DataColumn("Reviewer 2"),
        //                                    new DataColumn("Review 2 Date"),
        //                                    new DataColumn("Required Date"),
        //                                    new DataColumn("Request Order Date"),
        //                                    new DataColumn("Order Date"),
        //                                    new DataColumn("Tentative Delivery Date"),
        //                                    new DataColumn("Actual Delivery Date"),
        //                                    new DataColumn("Fund"),
        //                                    new DataColumn("Order ID"),
        //                                    new DataColumn("Ordered Quantity",typeof(Int32)),
        //                                    new DataColumn("Order Price",typeof(decimal)),
        //                                    new DataColumn("Order Status")});
        //        string PresentUserDept_RequestTable = BudgetingController.lstDEPTs.Find(dept => dept.DEPT.Equals(presentUserDept)).ID.ToString();
        //        var requests1 = db.RequestItems_Table.Where(x => x.SubmitDate.ToString().Contains(useryear)).Select(x => new { x.BU, x.OEM, x.DEPT, x.Group, x.ItemName, x.Category, x.CostElement, x.UnitPrice, /*x.Currency,*/ x.ReqQuantity, x.TotalPrice, x.ApprQuantity, x.ApprCost, x.Comments, x.RequestorNT, x.SubmitDate, x.DHNT, x.DHAppDate, x.SHNT, x.SHAppDate, x.RequiredDate, x.RequestOrderDate, x.OrderDate, x.TentativeDeliveryDate, x.ActualDeliveryDate,
        //            x.OrderID,
        //            x.OrderedQuantity,
        //            x.OrderPrice,
        //            x.OrderStatus, x.Fund,
        //            x.ApprovedDH,
        //            x.ApprovedSH,

        //        }).ToList().FindAll(x=>x.ApprovedSH == true);
        //        var requests = requests1.Where(x => x.DEPT == PresentUserDept_RequestTable).Select(x => new { x.BU, x.OEM, x.DEPT, x.Group, x.ItemName, x.Category, x.CostElement, x.UnitPrice, /*x.Currency,*/ x.ReqQuantity, x.TotalPrice, x.ApprQuantity, x.ApprCost, x.Comments, x.RequestorNT, x.SubmitDate, x.DHNT, x.DHAppDate, x.SHNT, x.SHAppDate, x.RequiredDate, x.RequestOrderDate, x.OrderDate, x.TentativeDeliveryDate, x.ActualDeliveryDate,
        //            x.OrderID,
        //            x.OrderedQuantity,
        //            x.OrderPrice,
        //            x.OrderStatus, x.Fund }).ToList();

        //        foreach (var request in requests)
        //        {

        //            dt.Rows.Add(
        //                BudgetingController.lstBUs.Find(bu => bu.ID.ToString().Equals(request.BU)).BU,
        //                BudgetingController.lstOEMs.Find(oem => oem.ID.ToString().Equals(request.OEM)).OEM,
        //                BudgetingController.lstDEPTs.Find(dept => dept.ID.ToString().Equals(request.DEPT)).DEPT,
        //                BudgetingController.lstGroups_test.Find(grp => grp.ID.ToString().Equals(request.Group)).Group,
        //                BudgetingController.lstItems.Find(item => item.S_No.ToString().Equals(request.ItemName)).Item_Name,
        //                BudgetingController.lstPrdCateg.Find(cat => cat.ID.ToString().Equals(request.Category)).Category.Trim(),
        //                BudgetingController.lstCostElement.Find(cost => cost.ID.ToString().Equals(request.CostElement)).CostElement,
        //                request.UnitPrice,
        //                request.ReqQuantity,
        //                request.TotalPrice,
        //                request.ApprQuantity,
        //                request.ApprCost,
        //                request.Comments,
        //                request.RequestorNT,
        //                request.SubmitDate.HasValue ? request.SubmitDate.Value.ToString("dd-MM-yyyy") : "",
        //                request.DHNT, request.DHAppDate.HasValue ? request.DHAppDate.Value.ToString("dd-MM-yyyy") : "",
        //                request.SHNT, request.SHAppDate.HasValue ? request.SHAppDate.Value.ToString("dd-MM-yyyy") : "",
        //                request.RequiredDate.HasValue ? request.RequiredDate.Value.ToString("dd-MM-yyyy") : "",
        //                request.RequestOrderDate.HasValue ? request.RequestOrderDate.Value.ToString("dd-MM-yyyy") : "",
        //                request.OrderDate.HasValue ? request.OrderDate.Value.ToString("dd-MM-yyyy") : "",
        //                request.TentativeDeliveryDate.HasValue ? request.TentativeDeliveryDate.Value.ToString("dd-MM-yyyy") : "",
        //                request.ActualDeliveryDate.HasValue ? request.ActualDeliveryDate.Value.ToString("dd-MM-yyyy") : "",
        //                 request.Fund!=null ? BudgetingController.lstFund.Find(fun => fun.ID.ToString().Equals(request.Fund)).Fund: BudgetingController.lstFund.Find(fun => fun.Fund.Equals("F02")).Fund,
        //                 request.OrderID,
        //                request.OrderedQuantity != null ? (int)request.OrderedQuantity : (int)0,
        //                request.OrderPrice,
        //                (request.OrderStatus != null && request.OrderStatus.Trim() != "") ? BudgetingController.lstOrderStatus.Find(status => status.ID.ToString().Equals(request.OrderStatus.Trim())).OrderStatus : ""
        //                );
        //        }

        //        using (XLWorkbook wb = new XLWorkbook())
        //        {
        //            wb.Worksheets.Add(dt);
        //            using (MemoryStream stream = new MemoryStream())
        //            {
        //                wb.SaveAs(stream);
        //                var robj = File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
        //                return Json(robj, JsonRequestBehavior.AllowGet);
        //            }
        //        }

        //    }

        //}







        /// <summary>
        /// function to get the Initial values to be filled automatically when a new request is created
        /// </summary>
        /// <returns></returns>
        //[HttpGet]
        //public ActionResult InitRowValues_VKMPlanning()
        //{

        //    using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //    //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //    {

        //        RequestItemsRepoEdit1 temp = new RequestItemsRepoEdit1();

        //        System.Security.Principal.IPrincipal User = System.Web.HttpContext.Current.User;
        //        SPOTONData_Table_2021 PresentUser = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper()));
        //        string presentUserName = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).EmployeeName;
        //        string L2ReviewerName = string.Empty;
        //        string L3ReviewerName = string.Empty;
        //        try
        //        {
        //            L2ReviewerName = BudgetingController.lstUsers.FindAll(user => PresentUser.Department.ToUpper().Equals(user.Group.ToUpper())).FirstOrDefault(head => head.Role.ToUpper().Contains("HEAD")).EmployeeName;
        //        }
        //        catch (Exception ex)
        //        {
        //            L2ReviewerName = "NA";
        //        }
        //        try
        //        {
        //            L3ReviewerName = BudgetingController.lstUsers.FindAll(user => PresentUser.Section.ToUpper().Contains(user.Section.ToUpper())).FirstOrDefault(head => head.Role.ToUpper().Contains("SECTION")).EmployeeName;
        //        }
        //        catch (Exception ex)
        //        {
        //            L3ReviewerName = "NA";
        //        }

        //        temp.Requestor = presentUserName;
        //        temp.Reviewer_1 = L2ReviewerName;
        //        temp.Reviewer_2 = L3ReviewerName;
        //        DEPT_Table dEPT_Table = BudgetingController.lstDEPTs.Find(departmnt => departmnt.DEPT.Equals(BudgetingController.lstUsers.Find(x => x.EmployeeName == presentUserName).Department));
        //        temp.DEPT = dEPT_Table.ID;


        //        var PresentUserGroup = BudgetingController.lstUsers.Find(x => x.EmployeeName == presentUserName).Group;
        //        Groups_Table gROUP_Table = BudgetingController.lstGroups_test.Find(grp => grp.Group.Trim().Equals(PresentUserGroup));
        //        temp.Group = gROUP_Table.ID;

        //        temp.RequestDate = DateTime.Now.ToString("yyyy-MM-dd");
        //        return Json(new { data = temp }, JsonRequestBehavior.AllowGet);
        //    }

        //}




        

       






        /// <summary>
        /// function to get Requestor Details name based on NTID - for VKM Planning
        /// </summary>
        /// <param name="NTID"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetRequestorDetails_Planning(string NTID)
        {
            //get Full Name, Department and Group
            VKMPlanningRequestorView data = new VKMPlanningRequestorView();

            if (BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToLower().Equals(NTID.Trim().ToLower())) != null)
            {
                data.NTID = NTID.Trim().ToUpper();
                data.FullName = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToLower().Equals(NTID.Trim().ToLower())).EmployeeName;
                //DEPT_Table dEPT = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToLower().Equals(NTID.Trim().ToLower())).Department;
                //data.Group = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToLower().Equals(NTID.Trim().ToLower())).Group;
                DEPT_Table dEPT_Table = BudgetingController.lstDEPTs.Find(departmnt => departmnt.DEPT.Equals(BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToLower().Equals(NTID.Trim().ToLower())).Department.Trim()));
                data.Department = dEPT_Table.ID;
                var ReqGroup = BudgetingController.lstUsers.Find(x=>x.NTID.Trim().ToUpper() == NTID.Trim().ToUpper()).Group.Trim();
                Groups_Table_Test gROUP_Table = BudgetingController.lstGroups_test.Find(grp => grp.Group.Trim().Equals(ReqGroup));
                data.Group = gROUP_Table.ID;
                return Json(new { success = true, data }, JsonRequestBehavior.AllowGet);

            }
            else
            {
                return Json(new { success = false, message = "The NTID is not valid, Please check again!" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult GetRequestorDetails_Planning_EMProxy(string NTID)
        {
            //get Full Name, Department and Group
            VKMPlanningRequestorView data = new VKMPlanningRequestorView();
            if (NTID == "")
            {
                data.NTID = "";
                data.FullName = "";
                return Json(new { success = true, data }, JsonRequestBehavior.AllowGet);
            }
            if (BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToLower().Equals(NTID.Trim().ToLower())) != null)
            {
                data.NTID = NTID.Trim().ToUpper();
                data.FullName = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToLower().Equals(NTID.Trim().ToLower())).EmployeeName;
                return Json(new { success = true, data }, JsonRequestBehavior.AllowGet);

            }
            else
            {
                return Json(new { success = false, message = "The NTID is not valid, Please check again!" }, JsonRequestBehavior.AllowGet);
            }
        }


        /// <summary>
        /// function to get Requestor Details name based on NTID - for VKM Ordering
        /// </summary>
        /// <param name="NTID"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetRequestorDetails_Ordering(string NTID)
        {
            //get Full Name, Department and Group
            VKMPlanningRequestorView data = new VKMPlanningRequestorView();

            
            if (BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToLower().Equals(NTID.Trim().ToLower())) != null)
            {
                data.NTID = NTID.Trim().ToUpper();
                data.FullName = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToLower().Equals(NTID.Trim().ToLower())).EmployeeName;
                //DEPT_Table dEPT = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToLower().Equals(NTID.Trim().ToLower())).Department;
                //data.Group = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToLower().Equals(NTID.Trim().ToLower())).Group;
                DEPT_Table dEPT_Table = BudgetingController.lstDEPTs.Find(departmnt => departmnt.DEPT.Equals(BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToLower().Equals(NTID.Trim().ToLower())).Department));
                data.Department = dEPT_Table.ID;
                var ReqGroup = BudgetingController.lstUsers.Find(x => x.NTID.Trim().ToUpper() == NTID.Trim().ToUpper()).Group;
                Groups_Table_Test gROUP_Table = BudgetingController.lstGroups_test.Find(grp => grp.Group.Trim().Equals(ReqGroup));
                data.Group = gROUP_Table.ID;
                return Json(new { success = true, data }, JsonRequestBehavior.AllowGet);

            }
            //else if (BudgetingController.lstUsers_2020.FirstOrDefault(user => user.NTID.Trim().ToLower().Equals(NTID.Trim().ToLower())) != null)
            //{
            //    data.NTID = NTID.Trim().ToUpper();
            //    data.FullName = BudgetingController.lstUsers_2020.FirstOrDefault(user => user.NTID.Trim().ToLower().Equals(NTID.Trim().ToLower())).EmployeeName;
            //    //DEPT_Table dEPT = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToLower().Equals(NTID.Trim().ToLower())).Department;
            //    //data.Group = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToLower().Equals(NTID.Trim().ToLower())).Group;
            //    DEPT_Table dEPT_Table = BudgetingController.lstDEPTs.Find(departmnt => departmnt.DEPT.Equals(BudgetingController.lstUsers_2020.FirstOrDefault(user => user.NTID.Trim().ToLower().Equals(NTID.Trim().ToLower())).Department));
            //    data.Department = dEPT_Table.ID;
            //    var ReqGroup = BudgetingController.lstUsers_2020.Find(x => x.NTID.Trim().ToUpper() == NTID.Trim().ToUpper()).Group;
            //    Groups_Table_Test gROUP_Table = BudgetingController.lstGroups_test.Find(grp => grp.Group.Trim().Equals(ReqGroup));
            //    data.Group = gROUP_Table.ID;
            //    return Json(new { success = true, data }, JsonRequestBehavior.AllowGet);

            //}
            else
            {
                return Json(new { success = false, message = "The NTID is not valid, Please check again!" }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpGet]
        public ActionResult Lookup()
        {
            LookupData lookupData = new LookupData();

            lookupData.DEPT_List = BudgetingController.lstDEPTs.OrderBy(x => x.Outdated).ToList();
            lookupData.Groups_test = BudgetingController.lstGroups_test.OrderBy(x => x.Outdated).ToList();
            
            return Json(new { data = lookupData }, JsonRequestBehavior.AllowGet);

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

        //***************************************************************  HOE - Proxy List ****************************************************************//


        /******************************************   HOE & HOE's PROXY DETAILS   ***********************************/


        /// <summary>
        /// function to fetch the Request Items made by the Requestor during the year chosen for view
        /// </summary>
        public ActionResult GetData_VKMPlanning_HOE()
        {


            using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

            //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

            {
                try
                {

                    List<VKMPlanning_HOEView> viewList = new List<VKMPlanning_HOEView>();
                    viewList = GetData1_VKMPlanning_HOE();

                    return Json(new
                    {

                        data = viewList
                    }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(new { success = true, message = "Unable to load the HOE List, Please Try again later!" }, JsonRequestBehavior.AllowGet);

                }

            }
        }


        public List<VKMPlanning_HOEView> GetData1_VKMPlanning_HOE()
        {
            //string PresentUserDept_RequestTable = BudgetingController.lstDEPTs.Find(dept => dept.DEPT.Equals(presentUserDept)).ID.ToString();


            using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

            //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

            {

                //System.Security.Principal.IPrincipal User = System.Web.HttpContext.Current.User;
                // string presentUserName = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).EmployeeName;
                List<Planning_HOE_Table> reqList = new List<Planning_HOE_Table>();
                reqList = db.Planning_HOE_Table.ToList();
                List<VKMPlanning_HOEView> viewList = new List<VKMPlanning_HOEView>();
                foreach (Planning_HOE_Table item in reqList)
                {
                    VKMPlanning_HOEView ritem = new VKMPlanning_HOEView();


                    ritem.Department = int.Parse(item.Department);
                    ritem.ID = item.ID;
                    ritem.HOE_NTID = item.HOE_NTID;
                    ritem.HOE_FullName = item.HOE_FullName;
                    ritem.Proxy_NTID = item.Proxy_NTID;
                    ritem.Proxy_FullName = item.Proxy_FullName;
                    ritem.Updated_By = item.Updated_By;
                    if (item.Enable_Proxy == null)
                        ritem.Enable_Proxy = false;
                    else
                        ritem.Enable_Proxy = (bool)item.Enable_Proxy;

                    viewList.Add(ritem);
                }
                return viewList;

            }
        }


        ///// <summary>
        ///// function to enable update Planning Requestor details and add a new entry of Requestor
        ///// </summary>
        ///// <param name="req"></param>
        ///// <returns></returns>
        [HttpPost]
        public ActionResult AddOrEdit_VKMPlanning_HOE(VKMPlanning_HOEEdit req)
        {
            List<VKMPlanning_HOEEdit> viewList = new List<VKMPlanning_HOEEdit>();


            using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

            //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

            {
                System.Security.Principal.IPrincipal User = System.Web.HttpContext.Current.User;
                string presentUserName = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).EmployeeName;

                //Validate if this is a new entry - the details of which is already present
                var HOE_Table = db.Planning_HOE_Table.AsNoTracking().ToList();
                if (req.ID == 0) //new entry
                {
                    if (HOE_Table.Find(x => x.HOE_NTID.ToUpper().Trim() == req.HOE_NTID.ToUpper().Trim()) != null) 
                    {
                        return Json(new
                        {
                            success = false,
                            message = "The HOE details for " + req.HOE_NTID + " are already present in the HOE List. Please check again! "
                        }, JsonRequestBehavior.AllowGet);

                    }
                }
                else
                {
                    if (HOE_Table.Find(x => x.HOE_NTID.ToUpper().Trim() == req.HOE_NTID.ToUpper().Trim()) != null && req.ID != HOE_Table.Find(x => x.HOE_NTID.ToUpper().Trim() == req.HOE_NTID.ToUpper().Trim()).ID)
                    {
                        return Json(new
                        {
                            success = false,
                            message = "The HOE details for " + req.HOE_NTID + " are already present in the HOE List. Please check again! "
                        }, JsonRequestBehavior.AllowGet);
                    }
                            
                }


                Planning_HOE_Table item = new Planning_HOE_Table();

                if (BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToLower().Equals(req.HOE_NTID.Trim().ToLower())) != null)
                {
                    item.Department = req.Department.ToString();
                    item.HOE_FullName = req.HOE_FullName;
                    item.ID = req.ID;
                    item.HOE_NTID = req.HOE_NTID;
                    item.Proxy_FullName = req.Proxy_FullName;
                    item.Proxy_NTID = req.Proxy_NTID;
                    item.Updated_By = presentUserName;
                    if (req.Enable_Proxy == true)
                        item.Enable_Proxy = true;
                    else
                        item.Enable_Proxy = false;

                    if (req.ID == 0)
                    {

                        db.Planning_HOE_Table.Add(item);
                        db.SaveChanges();
                        BudgetingController.lstHOEs = db.Planning_HOE_Table.ToList();

                        //viewList = GetData1(useryear);


                        return Json(new { success = true,/*, data = viewList,*/ message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);

                    }
                    else
                    {


                        db.Entry(item).State = EntityState.Modified;

                        db.SaveChanges();
                        BudgetingController.lstHOEs = db.Planning_HOE_Table.ToList();

                        //viewList = GetData1(useryear);

                        return Json(new { success = true, message = "Updated Successfully" }, JsonRequestBehavior.AllowGet);

                    }
                }
                else
                {
                    return Json(new { success = false, message = "The NTID is not valid, Please check again!" }, JsonRequestBehavior.AllowGet);
                }
            }


        }


        ///// <summary>
        ///// function to validate if the NTID input is already presnt in the List/not - Planning Stage
        ///// </summary>
        ///// <param name="NTID"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public ActionResult ValidateNTID_ifalready_exist_Planning_HOE(string NTID)
        //{

        //    using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //    //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //    {
        //        if (db.Planning_HOERequestor_Table.Where(x => x.NTID.Trim().Contains(NTID.Trim())) == null)
        //        {

        //            return Json(new { success = true, message = "" }, JsonRequestBehavior.AllowGet);

        //        }
        //        else
        //        {
        //            return Json(new { success = false, message = "The Requestor details are already present in the Planning_HOE Stage Requestor List, Please check again!" }, JsonRequestBehavior.AllowGet);
        //        }
        //    }



        //}


        /// <summary>
        /// function to delete an existing item
        /// </summary>
        /// <param name="ntid"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete_VKMPlanning_HOE(string ntid)
        {
            List<VKMPlanning_HOEView> viewList = new List<VKMPlanning_HOEView>();


            using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

            //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

            {
                Planning_HOE_Table item = db.Planning_HOE_Table.Where(x => x.HOE_NTID.Trim().Contains(ntid)).FirstOrDefault<Planning_HOE_Table>();
                db.Planning_HOE_Table.Remove(item);
                db.SaveChanges();
                BudgetingController.lstHOEs = db.Planning_HOE_Table.ToList();
                viewList = GetData1_VKMPlanning_HOE();
                return Json(new { data = viewList, success = true, message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
            }
        }



        /// <summary>
        /// Post method for importing Planning_HOE users 
        /// </summary>
        /// <param name="postedFile1"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Index_Planning_HOE(HttpPostedFileBase postedFile1)
        {
            string exceptionmsg = "";
            if (postedFile1 != null)
            {
                try
                {
                    string fileExtension = Path.GetExtension(postedFile1.FileName);

                    //Validate uploaded file and return error.
                    if (fileExtension != ".xls" && fileExtension != ".xlsx")
                    {

                        return Json(new { errormsg = "Please select the excel file with.xls or.xlsx extension" });

                    }

                    string folderPath = Server.MapPath("~/UploadedFiles/");
                    //Check Directory exists else create one
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    //Save file to folder
                    var filePath = folderPath + Path.GetFileName(postedFile1.FileName);
                    postedFile1.SaveAs(filePath);

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
                                DataTable excelSchema = GetSchemaFromExcel(excelOledbConnection);
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
                    }


                    int errcount = 0;
                    string msg = "";

                    using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

                    //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

                    {
                        System.Security.Principal.IPrincipal User = System.Web.HttpContext.Current.User;
                        string presentUserName = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).EmployeeName;

                        //Loop through datatable and add data to table. 
                        foreach (DataRow row in dt.Rows)
                        {
                            Planning_HOE_Table item = new Planning_HOE_Table();
                            try
                            {

                                if (row[0] == DBNull.Value || String.IsNullOrWhiteSpace(row[0].ToString()))
                                {
                                    continue;
                                }
                                else
                                {


                                    if (BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToLower().Equals(row[0].ToString().Trim().ToLower())) != null)
                                    {
                                        var ntid = row[0].ToString();
                                        item.HOE_NTID = ntid.ToUpper();
                                        item.HOE_FullName = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToLower().Equals(row[0].ToString().Trim().ToLower())).EmployeeName;
                                        DEPT_Table dEPT_Table = BudgetingController.lstDEPTs.Find(departmnt => departmnt.DEPT.Equals(BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToLower().Equals(row[0].ToString().Trim().ToLower())).Department));
                                        item.Department = dEPT_Table.ID.ToString();
                                        item.Updated_By = presentUserName;
                                        if (row[1] != DBNull.Value && !String.IsNullOrWhiteSpace(row[1].ToString()))
                                        {
                                            if (BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToLower().Equals(row[1].ToString().Trim().ToLower())) != null)
                                            {
                                                var proxy_ntid = row[1].ToString();
                                                item.Proxy_NTID = proxy_ntid.ToUpper();
                                                item.Proxy_FullName = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToLower().Equals(row[1].ToString().Trim().ToLower())).EmployeeName;
                                            }
                                            else
                                            {
                                                errcount++;
                                                if (errcount > 1)
                                                {
                                                    msg += " | \n" + "The HOE's Proxy NTID: " + row[1].ToString() + " is invalid. Please check again! ";

                                                }
                                                else
                                                    msg += "The HOE's Proxy NTID: " + row[1].ToString() + " is invalid. Please check again! ";


                                                continue;
                                            }


                                        }

                                        if (db.Planning_HOE_Table.ToList().Find(x => x.HOE_NTID.ToUpper().Trim() == ntid.ToUpper().Trim()) == null)
                                        {
                                            db.Planning_HOE_Table.Add(item);
                                        }
                                        else
                                        {
                                            errcount++;

                                            if (errcount > 1)
                                                msg += " | \n" + "The HOE details for " + row[0].ToString() + " are already present in the HOE List. Please check again! ";
                                            else
                                                msg += "The HOE details for " + row[0].ToString() + " are already present in the HOE List. Please check again! ";
                                            continue;

                                        }


                                    }
                                    else
                                    {
                                        errcount++;
                                        if (errcount > 1)
                                        {
                                            msg += " | \n" + "The HOE NTID: " + row[0].ToString() + " is invalid. Please check again! ";

                                        }
                                        else
                                            msg += "The HOE NTID: " + row[0].ToString() + " is invalid. Please check again! ";


                                        continue;
                                    }


                                }

                                int milliseconds = 500;
                                Thread.Sleep(milliseconds);
                                db.SaveChanges();
                                BudgetingController.lstHOEs = db.Planning_HOE_Table.ToList();
                            }
                            catch (Exception ex)
                            {
                                errcount++;

                                if (errcount > 1)
                                    msg += " | \n" + "Empty/invalid cell found :" + row[0].ToString();
                                else
                                    msg += "Empty/invalid cell found :" + row[0].ToString();


                            }

                        }
                    }


                    if (errcount > 0)
                    {
                        return Json(new { dataerror = true, errormsg = msg + " \nThe valid details were imported. Please find the errors listed. " });
                    }



                    else
                    {
                        return Json(new { dataerror = false, successmsg = " The details were successfully imported. Please edit any INVALID fields in the table." });
                    }





                }
                catch (Exception ex)
                {

                    exceptionmsg += ex.Message;
                    return Json(new { errormsg = exceptionmsg });
                }

            }
            else
            {

                return Json(new { dataerror = true, errormsg = "Please select the file to be uploaded." });

            }


        }

        public ActionResult DownloadTemplate_HOE()
        {
            string folderPath = Server.MapPath("~/Templates/");
            string fileName = "VKMHOE_ProxyList.xlsx";
            byte[] fileBytes = System.IO.File.ReadAllBytes(folderPath + fileName);

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }


        /// <summary>
        /// function to get Requestor Details name based on NTID - for VKM Planning_HOE
        /// </summary>
        /// <param name="NTID"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult GetRequestorDetails_Planning_HOE(string NTID)
        {
            //get Full Name, Department and Group
            VKMPlanningRequestorView data = new VKMPlanningRequestorView();

            if (BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToLower().Equals(NTID.Trim().ToLower())) != null)
            {
                data.NTID = NTID.Trim().ToUpper();
                data.FullName = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToLower().Equals(NTID.Trim().ToLower())).EmployeeName;
                //DEPT_Table dEPT = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToLower().Equals(NTID.Trim().ToLower())).Department;
                //data.Group = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToLower().Equals(NTID.Trim().ToLower())).Group;
                DEPT_Table dEPT_Table = BudgetingController.lstDEPTs.Find(departmnt => departmnt.DEPT.Equals(BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToLower().Equals(NTID.Trim().ToLower())).Department));
                data.Department = dEPT_Table.ID;

                return Json(new { success = true, data }, JsonRequestBehavior.AllowGet);

            }
            else
            {
                return Json(new { success = false, message = "The HOE NTID is not valid, Please check again!" }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult GetRequestorDetails_Planning_HOEProxy(string NTID)
        {
            //get Full Name, Department and Group
            VKMPlanningRequestorView data = new VKMPlanningRequestorView();
            if (NTID == "")
            {
                data.NTID = "";
                data.FullName = "";
                return Json(new { success = true, data }, JsonRequestBehavior.AllowGet);
            }
            if (BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToLower().Equals(NTID.Trim().ToLower())) != null)
            {
                data.NTID = NTID.Trim().ToUpper();
                data.FullName = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToLower().Equals(NTID.Trim().ToLower())).EmployeeName;
                return Json(new { success = true, data }, JsonRequestBehavior.AllowGet);

            }
            else
            {
                return Json(new { success = false, message = "The NTID is not valid, Please check again!" }, JsonRequestBehavior.AllowGet);
            }
        }






        //***************************************************************  VKM Role List ****************************************************************//


        /******************************************   VKM ROLE DETAILS   ***********************************/


        /// <summary>
        /// function to fetch the Request Items made by the Requestor during the year chosen for view
        /// </summary>
        public ActionResult GetData_VKMRole()
        {


            using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

            //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

            {
                try
                {

                    List<VKMRoleView> viewList = new List<VKMRoleView>();
                    viewList = GetData1_VKMRole();

                    return Json(new
                    {

                        data = viewList
                    }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    return Json(new { success = true, message = "Unable to load the VKM Role List, Please Try again later!" }, JsonRequestBehavior.AllowGet);

                }

            }
        }


        public List<VKMRoleView> GetData1_VKMRole()
        {
            //string PresentUserDept_RequestTable = BudgetingController.lstDEPTs.Find(dept => dept.DEPT.Equals(presentUserDept)).ID.ToString();


            using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

            //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

            {

                //System.Security.Principal.IPrincipal User = System.Web.HttpContext.Current.User;
                // string presentUserName = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).EmployeeName;
                List<HC_SkillSet_Table> reqList = new List<HC_SkillSet_Table>();
                reqList = db.HC_SkillSet_Table.ToList().FindAll(x => x.isDeleted != true);
                //reqList = db.HC_SkillSet_Table.ToList();
                List<VKMRoleView> viewList = new List<VKMRoleView>();
                foreach (HC_SkillSet_Table item in reqList)
                {
                    VKMRoleView ritem = new VKMRoleView();


                    
                    ritem.ID = item.ID;
                    ritem.SkillSetName = item.SkillSetName;
                   

                    viewList.Add(ritem);
                }
                return viewList;

            }
        }


        ///// <summary>
        ///// function to enable update Planning Requestor details and add a new entry of Requestor
        ///// </summary>
        ///// <param name="req"></param>
        ///// <returns></returns>
        [HttpPost]
        public ActionResult AddOrEdit_VKMRole(VKMRoleEdit req)
        {
            List<VKMRoleEdit> viewList = new List<VKMRoleEdit>();
            string Query = "";

            using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

            //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

            {
                System.Security.Principal.IPrincipal User = System.Web.HttpContext.Current.User;
                string presentUserName = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).EmployeeName;

                //Validate if this is a new entry - the details of which is already present
                var VKMRole_Table = db.HC_SkillSet_Table.AsNoTracking().ToList();
                if (req.ID == 0) //new entry
                {
                    if (VKMRole_Table.Find(x => x.SkillSetName.ToUpper().Trim() == req.SkillSetName.ToUpper().Trim()) != null)
                    {
                        return Json(new
                        {
                            success = false,
                            message = "The VKM Role details for " + req.SkillSetName + " are already present in the VKM Role List. Please check again! "
                        }, JsonRequestBehavior.AllowGet);

                    }
                    else
                    {
                        //HC_SkillSet_Table item = new HC_SkillSet_Table();
                        //item.SkillSetName = req.SkillSetName;
                        //db.HC_SkillSet_Table.Add(item);
                        //db.SaveChanges();
                        Query = " Insert into HC_SkillSet_Table (SkillSetName,isDeleted) Values ('" + req.SkillSetName.ToString() + "',0)";
                        //Query = " Insert into HC_SkillSet_Table (SkillSetName) Values ('" + req.SkillSetName.ToString() + "')";
                        connection();
                        BudgetingOpenConnection();
                        SqlCommand command = new SqlCommand(Query, budgetingcon);
                        command.ExecuteNonQuery();
                        BudgetingCloseConnection();

                        return Json(new { success = true,/*, data = viewList,*/ message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    if (VKMRole_Table.Find(x => x.SkillSetName.ToUpper().Trim() == req.SkillSetName.ToUpper().Trim()) != null && req.ID != VKMRole_Table.Find(x => x.SkillSetName.ToUpper().Trim() == req.SkillSetName.ToUpper().Trim()).ID)
                    {
                        return Json(new
                        {
                            success = false,
                            message = "The VKM Role details for " + req.SkillSetName + " are already present in the VKM Role List. Please check again! "
                        }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        //HC_SkillSet_Table item = new HC_SkillSet_Table();
                        //item.ID = req.ID;
                        //item.SkillSetName = req.SkillSetName;
                        //db.Entry(item).State = EntityState.Modified;

                        //db.SaveChanges();

                        //viewList = GetData1(useryear);

                        Query = " Update HC_SkillSet_Table Set SkillSetName = '" + req.SkillSetName.ToString() + "' where ID = " + req.ID + " ";
                        connection();
                        BudgetingOpenConnection();
                        SqlCommand command = new SqlCommand(Query, budgetingcon);
                        command.ExecuteNonQuery();
                        BudgetingCloseConnection();

                        return Json(new { success = true, message = "Updated Successfully" }, JsonRequestBehavior.AllowGet);
                    }

                }


               
            }


        }


        ///// <summary>
        ///// function to validate if the NTID input is already presnt in the List/not - Planning Stage
        ///// </summary>
        ///// <param name="NTID"></param>
        ///// <returns></returns>
        //[HttpPost]
        //public ActionResult ValidateNTID_ifalready_exist_Planning_HOE(string NTID)
        //{

        //    using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //    //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //    {
        //        if (db.Planning_HOERequestor_Table.Where(x => x.NTID.Trim().Contains(NTID.Trim())) == null)
        //        {

        //            return Json(new { success = true, message = "" }, JsonRequestBehavior.AllowGet);

        //        }
        //        else
        //        {
        //            return Json(new { success = false, message = "The Requestor details are already present in the Planning_HOE Stage Requestor List, Please check again!" }, JsonRequestBehavior.AllowGet);
        //        }
        //    }



        //}


        /// <summary>
        /// function to delete an existing item
        /// </summary>
        /// <param name="ntid"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Delete_VKMRole(int ID)
        {
            List<VKMRoleView> viewList = new List<VKMRoleView>();
            string Query = "";

            using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

            //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

            {
                //HC_SkillSet_Table item = db.HC_SkillSet_Table.Where(x => x.ID == ID).FirstOrDefault<HC_SkillSet_Table>();
                //db.HC_SkillSet_Table.Remove(item);
                //db.SaveChanges();

                //Query = " Delete from HC_SkillSet_Table where ID = " + ID + " ";
                Query = " Update HC_SkillSet_Table Set isDeleted = '" + 1 + "' where ID = " + ID + " ";
                connection();
                BudgetingOpenConnection();
                SqlCommand command = new SqlCommand(Query, budgetingcon);
                command.ExecuteNonQuery();
                BudgetingCloseConnection();

                viewList = GetData1_VKMRole();
                return Json(new { data = viewList, success = true, message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
            }
        }



        /// <summary>
        /// Post method for importing Planning_HOE users 
        /// </summary>
        /// <param name="postedFile1"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Index_VKMRole(HttpPostedFileBase postedFileVKMRole)
        {
            string exceptionmsg = "";
            string Query = "";
            if (postedFileVKMRole != null)
            {
                try
                {
                    string fileExtension = Path.GetExtension(postedFileVKMRole.FileName);

                    //Validate uploaded file and return error.
                    if (fileExtension != ".xls" && fileExtension != ".xlsx")
                    {

                        return Json(new { errormsg = "Please select the excel file with.xls or.xlsx extension" });

                    }

                    string folderPath = Server.MapPath("~/UploadedFiles/");
                    //Check Directory exists else create one
                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    //Save file to folder
                    var filePath = folderPath + Path.GetFileName(postedFileVKMRole.FileName);
                    postedFileVKMRole.SaveAs(filePath);

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
                                DataTable excelSchema = GetSchemaFromExcel(excelOledbConnection);
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
                    }


                    int errcount = 0;
                    string msg = "";

                    using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

                    //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

                    {
                        System.Security.Principal.IPrincipal User = System.Web.HttpContext.Current.User;
                        string presentUserName = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).EmployeeName;

                        //Loop through datatable and add data to table. 
                        foreach (DataRow row in dt.Rows)
                        {
                            //HC_SkillSet_Table item = new HC_SkillSet_Table();
                            
                            try
                            {

                                if (row[0] == DBNull.Value || String.IsNullOrWhiteSpace(row[0].ToString()))
                                {
                                    continue;
                                }
                                else
                                {


                                    //if (BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToLower().Equals(row[0].ToString().Trim().ToLower())) != null)
                                    //{
                                        
                                        //var SkillSetName = "";


                                        if (row[0] != DBNull.Value && !String.IsNullOrWhiteSpace(row[0].ToString()))
                                        {
                                        //SkillSetName = row[0].ToString();
                                        Query += "EXEC [dbo].[InsertHCSkillSetTable] '" + row[0].ToString() + "';";   
                                           
                                        }



                                        //if (db.HC_SkillSet_Table.ToList().Find(x => x.SkillSetName.ToUpper().Trim() == SkillSetName.ToUpper().Trim()) == null)
                                        //{
                                        //    item.SkillSetName = SkillSetName;
                                        //    db.HC_SkillSet_Table.Add(item);
                                        //}
                                        //else
                                        //{
                                        //    errcount++;

                                        //    if (errcount > 1)
                                        //        msg += " | \n" + "The VKM Role details for " + row[0].ToString() + " are already present in the VKM Role List. Please check again! ";
                                        //    else
                                        //        msg += "The VKM Role details for " + row[0].ToString() + " are already present in the VKM Role List. Please check again! ";
                                        //    continue;

                                        //}


                                    //}
                                    //else
                                    //{
                                    //    errcount++;
                                    //    if (errcount > 1)
                                    //    {
                                    //        msg += " | \n" + "The VKM Role : " + row[1].ToString() + " is invalid. Please check again! ";

                                    //    }
                                    //    else
                                    //        msg += "The VKM Role: " + row[1].ToString() + " is invalid. Please check again! ";


                                    //    continue;
                                    //}


                                }
                                
                                //int milliseconds = 500;
                                //Thread.Sleep(milliseconds);
                                //db.SaveChanges();

                            }
                            catch (Exception ex)
                            {
                                errcount++;

                                if (errcount > 1)
                                    msg += " | \n" + "Empty/invalid cell found :" + row[0].ToString();
                                else
                                    msg += "Empty/invalid cell found :" + row[0].ToString();


                            }

                        }

                        connection();
                        BudgetingOpenConnection();
                        SqlCommand command = new SqlCommand(Query, budgetingcon);
                        command.ExecuteNonQuery();
                        BudgetingCloseConnection();
                    }


                    if (errcount > 0)
                    {
                        return Json(new { dataerror = true, errormsg = msg + " \nThe valid details were imported. Please find the errors listed. " });
                    }



                    else
                    {
                        return Json(new { dataerror = false, successmsg = " The details were successfully imported. Please edit any INVALID fields in the table." });
                    }





                }
                catch (Exception ex)
                {

                    exceptionmsg += ex.Message;
                    return Json(new { errormsg = exceptionmsg });
                }

            }
            else
            {

                return Json(new { dataerror = true, errormsg = "Please select the file to be uploaded." });

            }


        }

        public ActionResult DownloadTemplate_VKMRole()
        {
            string folderPath = Server.MapPath("~/Templates/");
            string fileName = "VKMRoleList.xlsx";
            byte[] fileBytes = System.IO.File.ReadAllBytes(folderPath + fileName);

            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }

        


    }







}
