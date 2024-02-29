using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using ClosedXML.Excel;
using LC_Reports_V1.Models;
using Newtonsoft.Json;

namespace LC_Reports_V1.Controllers
{
    // [Authorize(Users = @"apac\jov6cob,apac\rba3cob,apac\din2cob,apac\MTA2COB,apac\mxk8kor,apac\nnj6kor,apac\pks5cob,apac\chb1kor,apac\sbr2kor,apac\rau2kor,apac\bbv5kor,apac\rmm7kor,apac\gpa3kor,apac\mae9cob,apac\oig1cob,apac\rma5cob,apac\mow4kor,apac\SIF1COB,apac\mhv7kor,apac\rpi8kor, apac\DYD1KOR")]
    public class BudgetingInventoryController : Controller
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

        public ActionResult Index()
        {

            WriteLog("****************** Budgeting Inventory *********************");
            string NTID = ItemMaster_authorise();
            if (NTID == "")
            {
                // throw new HttpException(404, "Sorry! Current user is not authorised to access this view!");
                return Content("Sorry! Current user is not authorised to access this view !");
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

        public string ItemMaster_authorise()
        {
            
            string NTID = "";
            connection();
            BudgetingOpenConnection();
            string qry = " Exec [dbo].[ItemMaster_Authorize] '" + User.Identity.Name.Split('\\')[1].Trim() + "' ";

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



        /// <summary>
        /// Post method for importing users 
        /// </summary>
        /// <param name="postedFile"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult Index(HttpPostedFileBase postedFile)
        {
           
            string exceptionmsg = "";
            connection();
            BudgetingOpenConnection();
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

                        {
                            System.Security.Principal.IPrincipal User = System.Web.HttpContext.Current.User;
                            string presentUserName = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).EmployeeName;

                            //Loop through datatable and add data to table. 
                            foreach (DataRow row in dt.Rows)
                            {
                                ItemsCostList_Table item = new ItemsCostList_Table();
                                try
                                {
                                    decimal conversionEURate = /*(decimal)1.15;*/ db.Currency_Conversion_Table.FirstOrDefault(x => x.Currency.Contains("EUR")).ConversionRate_to_USD;

                                    decimal conversionINRate = /*(decimal)0.014;*/db.Currency_Conversion_Table.FirstOrDefault(x => x.Currency.Contains("INR")).ConversionRate_to_USD;

                                    //to remove unwanted spaces
                                    RegexOptions options = RegexOptions.None;
                                    Regex regex = new Regex("[ ]{2,}", options);

                                    if (((row[0] == DBNull.Value) && (row[1] == DBNull.Value) && (row[2] == DBNull.Value) && (row[3] == DBNull.Value) && (row[4] == DBNull.Value) && (row[5] == DBNull.Value) && (row[6] == DBNull.Value)) || (String.IsNullOrWhiteSpace(row[0].ToString()) && String.IsNullOrWhiteSpace(row[1].ToString()) && String.IsNullOrWhiteSpace(row[2].ToString()) && String.IsNullOrWhiteSpace(row[3].ToString()) && String.IsNullOrWhiteSpace(row[4].ToString()) && String.IsNullOrWhiteSpace(row[5].ToString()) && String.IsNullOrWhiteSpace(row[6].ToString())))
                                    {
                                        continue;
                                    }
                                    else
                                    {

                                        if (String.IsNullOrWhiteSpace(row[0].ToString()) || row[0] == DBNull.Value)
                                        {
                                            errcount++;
                                            msg += "Please enter Item Name";
                                            continue;

                                        }

                                        item.Item_Name = row[0].ToString();

                                        item.Category = BudgetingController.lstPrdCateg.Find(cat => cat.Category.Trim().Replace(" ", "").ToLower().Equals(row[1].ToString().Replace(" ", "").Trim().ToLower())).ID.ToString();
                                        item.Cost_Element = BudgetingController.lstCostElement.Find(cost => cost.CostElement.Trim().Replace(" ", "").ToLower().Equals(row[2].ToString().Trim().Replace(" ", "").ToLower())).ID.ToString();
                                        item.BudgetCode = Convert.ToInt32(row[3].ToString());
                                        if(BudgetingController.lstMaterialGroup.Find(cat => cat.Material_Group.Trim().Replace(" ", "").ToLower().Equals(row[4].ToString().Replace(" ", "").Trim().ToLower())) == null)
                                        {
                                            InsertMaterialGroup(row[4].ToString().Trim());
                                            BudgetingController.InitialiseMaterialGroup();
                                        }
                                        item.Material_Group = BudgetingController.lstMaterialGroup.Find(cat => cat.Material_Group.Trim().Replace(" ", "").ToLower().Equals(row[4].ToString().Replace(" ", "").Trim().ToLower())).ID.ToString();
                                        item.Order_Type = BudgetingController.lstOrderType.Find(cost => cost.Order_Type.Trim().Replace(" ", "").ToLower().Equals(row[5].ToString().Trim().Replace(" ", "").ToLower())).ID;

                                        item.Currency = BudgetingController.lstCurrency.Find(curr => curr.Currency.Trim().Replace(" ", "").ToLower().Equals(row[6].ToString().Trim().Replace(" ", "").ToLower())).ID.ToString();

                                        item.Unit_Price = (int)decimal.Parse(row[7].ToString());
                                        item.UOM = BudgetingController.lstUOM.Find(cost => cost.UOM.Trim().Replace(" ", "").ToLower().Equals(row[8].ToString().Trim().Replace(" ", "").ToLower())).ID.ToString();

                                        item.Actual_Available_Quantity = (row[9].ToString().Trim() != null && row[9].ToString().Replace(" ", "").Replace("\"", "").Trim() != "") ? row[9].ToString() : "NA";

                                        item.VendorCategory = (row[10].ToString().Trim().Replace(" ", "").Replace("\"", "").ToLower() != null && row[10].ToString().Trim().Replace(" ", "").Replace("\"", "").ToLower() != "") ? BudgetingController.lstVendor.Find(vendor => vendor.VendorCategory.Trim().Replace(" ", "").Replace("\"", "").ToLower().Equals(row[10].ToString().Trim().Replace(" ", "").Replace("\"", "").ToLower())).ID.ToString() : "";


                                        //to add the unitprice in usd
                                        if (row[6].ToString() == "USD")
                                            item.UnitPriceUSD = item.Unit_Price;
                                        else if (row[6].ToString() == "EUR")
                                            item.UnitPriceUSD = (double?)((decimal)item.Unit_Price * conversionEURate);
                                        else
                                            item.UnitPriceUSD = (double?)((decimal)item.Unit_Price * conversionINRate);

                                        item.Comments = row[11].ToString();
                                        item.BU = BudgetingController.lstBUs.Find(bu => bu.BU.Trim().Replace(" ", "").ToLower().Equals(row[12].ToString().Replace(" ", "").Trim().ToLower())).ID;
                                        item.VKM_Year = int.Parse(row[13].ToString());
                                        item.Deleted = false;
                                        item.RequestorNT = presentUserName;
                                        item.UpdatedAt = DateTime.Now;

                                        //item.BudgetCode = Convert.ToInt32(row[10].ToString());
                                        if (db.ItemsCostList_Table.ToList().Find(x => x.Item_Name.ToUpper().Trim() == item.Item_Name.ToUpper().Trim() && x.BU == item.BU && x.Deleted == false && x.VKM_Year == item.VKM_Year) == null)
                                        {
                                            db.ItemsCostList_Table.Add(item);
                                            db.SaveChanges();
                                        }
                                        else
                                        {
                                            
                                            string Query = " Exec [dbo].[UpdateMasterList] '" + item.Item_Name + "', '" + item.Category + "' ,'" + item.Cost_Element + "' ,'" + item.Currency + "','" + item.Actual_Available_Quantity + "','" + item.Unit_Price + "','" + item.VendorCategory + "','" + item.UnitPriceUSD + "','" + item.Comments + "','" + item.BU + "','" + item.VKM_Year + "','" + item.Deleted + "','" + item.RequestorNT + "','" + item.BudgetCode + "','" + item.Material_Group + "','" + item.Order_Type + "','" + item.UOM + "'";
                                            SqlCommand cmd = new SqlCommand(Query, budgetingcon);
                                            cmd.ExecuteNonQuery();
                                           
                                        }

                                    }
                                    BudgetingController.lstItems = db.ItemsCostList_Table.ToList();
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
                            WriteLog("Import File error: " + msg.ToString() + " " + DateTime.Now.ToString());
                            return Json(new { dataerror = true, errormsg = msg + " \nThe valid requests were imported. Please find the errors listed. " });
                        }



                        else
                        {
                            WriteLog("Import File:Success " + DateTime.Now.ToString());
                            return Json(new { dataerror = false, successmsg = " The requests were successfully imported. Please edit any INVALID fields in the table." });
                        }





                    }
                    catch (Exception ex)
                    {
                        WriteLog("Import File error from catch: " + ex.Message.ToString() + " " + DateTime.Now.ToString());
                        exceptionmsg += ex.Message;
                        return Json(new { errormsg = exceptionmsg });
                    }
                    finally
                    {
                        BudgetingCloseConnection();
                    }


                }
                else
                {
                    WriteLog("Import File error from else: Please select the file to be uploaded " + DateTime.Now.ToString());
                    return Json(new { dataerror = true, errormsg = "Please select the file to be uploaded." });

                }

            }
           
           

        

        public ActionResult DownloadTemplate()
        {
            string folderPath = Server.MapPath("~/Templates/");
            string fileName = "BGSW_MasterListTemplate.xlsx";
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


        /// <summary>
        /// Function to help the export to excel function 
        /// Path to export - default or input from User
        /// Feedback to user after saving
        /// </summary>
        public ActionResult ExportDataToExcel()
        {
            DataTable dt1 = new DataTable();
            try
            {
                string filename = @"Master_Item_List_" + DateTime.Now.ToShortDateString() + ".xlsx";

                using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

                ////using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

                {
                    System.Data.DataTable dt = new System.Data.DataTable("Request_List");
                    dt.Columns.AddRange(new DataColumn[16] {
                                            new DataColumn("Item Name"),
                                            new DataColumn("Category"),
                                            new DataColumn("Cost Element"),
                                            new DataColumn("Budget Code"),
                                            new DataColumn("Material Group"),
                                            new DataColumn("Order Type"),
                                            new DataColumn("Currency"),
                                            new DataColumn("Unit Price",typeof(decimal)),
                                            new DataColumn("Unit Price (in USD)",typeof(decimal)),
                                            new DataColumn("Units"),
                                            new DataColumn("Actual Available Quantity"),
                                            new DataColumn("Vendor"),
                                            new DataColumn("Comments"),
                                            new DataColumn("BU"),
                                            new DataColumn("Requestor"),
                                            new DataColumn("VKM Year"),
                                            

                });
                    string Query = "Select [Item Name],Category,[Cost Element],BudgetCode,isnull([Material_Group],'81') as Material_Group,isnull(Order_Type,'3') as Order_Type,Currency,[Unit Price],UnitPriceUSD,isnull(UOM,'29') as UOM,[Actual Available Quantity],[VendorCategory],Comments,BU,RequestorNT,VKM_Year from ItemsCostList_Table where Category is not null and [Cost Element] is not null and Deleted = 0";
                    connection();
                    BudgetingOpenConnection();
                    SqlCommand cmd = new SqlCommand(Query, budgetingcon);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt1);
                    //var requests = db.ItemsCostList_Table.Where(item => item.Deleted != true && item.Cost_Element != null && item.Category != null).Select(x => new { x.Item_Name, x.Category, x.Cost_Element, x.Currency, x.Actual_Available_Quantity, x.Unit_Price, x.UnitPriceUSD, x.VendorCategory, x.Comments, x.BU, x.RequestorNT, x.VKM_Year, x.BudgetCode }).ToList(); ;

                    foreach (DataRow item in dt1.Rows)
                    {
                        dt.Rows.Add(
                               item["Item Name"].ToString(),
                               BudgetingController.lstPrdCateg.Find(cat => cat.ID.ToString().Equals(item["Category"].ToString())).Category.Trim(),
                               BudgetingController.lstCostElement.Find(cost => cost.ID.ToString().Equals(item["Cost Element"].ToString())).CostElement,
                              item["BudgetCode"].ToString(),
                               // BudgetingController.BudgetCodeList.Find(cost => cost.ID.ToString().Equals(item["BudgetCode"].ToString())).Budget_Code,
                               BudgetingController.lstMaterialGroup.Find(curr => curr.ID.ToString().Equals(item["Material_Group"].ToString())).Material_Group,
                               BudgetingController.lstOrderType.Find(curr => curr.ID.ToString().Equals(item["Order_Type"].ToString())).Order_Type,

                               BudgetingController.lstCurrency.Find(curr => curr.ID.ToString().Equals(item["Currency"].ToString())).Currency,

                               Math.Round(Convert.ToDecimal(item["Unit Price"])),
                               Math.Round(Convert.ToDecimal(item["UnitPriceUSD"])),
                               BudgetingController.lstUOM.Find(curr => curr.ID.ToString().Equals(item["UOM"].ToString())).UOM,

                               item["Actual Available Quantity"].ToString() != "" ? item["Actual Available Quantity"].ToString() : "NA",
                               (item["VendorCategory"].ToString() != "" && item["VendorCategory"].ToString() != "0") ? BudgetingController.lstVendor.Find(curr => curr.ID.ToString().Equals(item["VendorCategory"].ToString())).VendorCategory : "NA",
                               item["Comments"].ToString(),
                               BudgetingController.lstBUs.Find(cat => cat.ID.ToString().Equals(item["BU"].ToString())).BU.Trim(),

                               item["RequestorNT"].ToString(),
                               item["VKM_Year"].ToString()

                               );
//);
//                        dt.Rows.Add(request.Item_Name,
//                            BudgetingController.lstPrdCateg.Find(cat => cat.ID.ToString().Equals(request.Category)).Category.Trim(),
//                            BudgetingController.lstCostElement.Find(cost => cost.ID.ToString().Equals(request.Cost_Element)).CostElement,
//                            request.BudgetCode,
//                            BudgetingController.lstCurrency.Find(curr => curr.ID.ToString().Equals(request.Currency)).Currency.Trim(),
//                            request.Actual_Available_Quantity != null ? request.Actual_Available_Quantity : "NA",
//                            request.Unit_Price,
//                            request.UnitPriceUSD,
//                            (request.VendorCategory != null && request.VendorCategory.Trim() != "" && request.VendorCategory.Trim() != "0") ? BudgetingController.lstVendor.Find(vendor => vendor.ID.ToString().Equals(request.VendorCategory.Trim())).VendorCategory : "",
//                            request.Comments,
//                            request.BU != null ? BudgetingController.lstBUs.Find(bu => bu.ID.Equals(request.BU)).BU.Trim() : "",
//                            request.RequestorNT,
//                            request.VKM_Year);
                    }

                    using (XLWorkbook wb = new XLWorkbook())
                    {
                        wb.Worksheets.Add(dt);
                        using (MemoryStream stream = new MemoryStream())
                        {
                            wb.SaveAs(stream);
                            //WriteLog("Export data to excel: Success " + DateTime.Now.ToString());
                            return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
                        }
                    }

                }
                
            }
            catch (Exception ex)
            {
                WriteLog("Export data to excel: " + ex.Message.ToString() + " " + DateTime.Now.ToString());
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
            finally
            {
                BudgetingCloseConnection();
            }
        }


        /// <summary>
        /// function to get the Item Masterlist 
        /// </summary>
        /// <returns></returns>
        /// 

        public ActionResult GetData()
        {

            DataTable dt = new DataTable();
            List<RequestItemsRepoView1> viewList = new List<RequestItemsRepoView1>();
            try
            {
                string Query = "Select VKM_Year as VKMYear,Category,Comments,[Cost Element] as Cost_Element,Currency,[Item Name] as Item_Name,BU,[VendorCategory],[Actual Available Quantity] as ActualAvailableQuantity,[Unit Price] as Unit_Price,UnitPriceUSD as Unit_PriceUSD,RequestorNT,[S#No] as S_No,BudgetCode,isnull([Material_Group],'81') as Material_Group,isnull(UOM,'29') as UOM,isnull(Order_Type,'3') as Order_Type from ItemsCostList_Table where Category is not null and [Cost Element] is not null and Deleted = 0 and VKM_Year = 2023";
                connection();
                BudgetingOpenConnection();
                SqlCommand cmd = new SqlCommand(Query, budgetingcon);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
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
            catch (Exception ex)
            {
                return Json(new { data = viewList }, JsonRequestBehavior.AllowGet);

            }
            finally
            {
                BudgetingCloseConnection();
            }

            ////
            ///Linq Query
            ///

            //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

            ////using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

            //{
            //    List<ItemsCostList_Table> TempItemsList = db.ItemsCostList_Table.Where(x => x.Deleted == false && x.Category != null && x.Cost_Element != null).OrderBy(x=>x.Item_Name).ToList<ItemsCostList_Table>();
            //    List<RequestItemsRepoView1> viewList = new List<RequestItemsRepoView1>();
            //    foreach (ItemsCostList_Table item in TempItemsList)
            //    {
            //        RequestItemsRepoView1 ritem = new RequestItemsRepoView1();
            //        if(item.Deleted == true)
            //        {
            //            continue;
            //        }
            //        ritem.VKMYear = (int)item.VKM_Year;
            //        ritem.Category = int.Parse(item.Category);
            //        ritem.Comments = item.Comments;
            //        ritem.Cost_Element = int.Parse(item.Cost_Element);
            //        ritem.Currency = int.Parse(item.Currency);
            //        ritem.Item_Name = item.Item_Name;
            //        if(item.BU != null)
            //            ritem.BU = (int)item.BU;
            //        if (item.VendorCategory != null && item.VendorCategory.Trim() != "")
            //        {
            //            ritem.VendorCategory = int.Parse(item.VendorCategory);
            //        }
            //        else
            //        {
            //            ritem.VendorCategory = null;
            //        }
            //        if (item.Actual_Available_Quantity == null || item.Actual_Available_Quantity.Trim() == string.Empty)
            //            ritem.ActualAvailableQuantity = "NA";
            //        else
            //            ritem.ActualAvailableQuantity = item.Actual_Available_Quantity;

            //        ritem.Unit_Price = (decimal?)item.Unit_Price;
            //        ritem.Unit_PriceUSD = (decimal?)item.UnitPriceUSD;
            //        ritem.Requestor = item.RequestorNT;
            //        ritem.ActualAvailableQuantity = item.Actual_Available_Quantity;
            //        ritem.S_No = item.S_No;
            //        if (item.Deleted != true)
            //            ritem.Deleted = false;
            //        else
            //            ritem.Deleted = true;

            //        ritem.BudgetCode = item.BudgetCode.ToString();

            //        viewList.Add(ritem);
            //    }
            //return Json(new { data = viewList }, JsonRequestBehavior.AllowGet);
            // }
        }



        //public ActionResult GetData()
        //{

        //    DataTable dt = new DataTable();
        //    List<RequestItemsRepoView1> viewList = new List<RequestItemsRepoView1>();
        //    try
        //    {
        //        string Query = "Select VKM_Year,Category,Comments,[Cost Element],Currency,[Item Name],BU,[VendorCategory],[Actual Available Quantity],[Unit Price],UnitPriceUSD,RequestorNT,[S#No],BudgetCode,isnull([Material_Group],'81') as Material_Group,isnull(UOM,'29') as UOM,isnull(Order_Type,'3') as Order_Type from ItemsCostList_Table where Category is not null and [Cost Element] is not null and Deleted = 0 and VKM_Year = 2023";
        //        connection();
        //        BudgetingOpenConnection();
        //        SqlCommand cmd = new SqlCommand(Query,budgetingcon);
        //        SqlDataAdapter da = new SqlDataAdapter(cmd);
        //        da.Fill(dt);
        //        foreach(DataRow row in dt.Rows)
        //        {
        //            RequestItemsRepoView1 ritem = new RequestItemsRepoView1();

        //            ritem.VKMYear = Convert.ToInt32(row["VKM_Year"].ToString());
        //            ritem.Category = Convert.ToInt32(row["Category"].ToString());
        //            ritem.Comments = row["Comments"].ToString();
        //            ritem.Cost_Element = Convert.ToInt32(row["Cost Element"].ToString());
        //            ritem.Currency = Convert.ToInt32(row["Currency"].ToString());
        //            ritem.Item_Name = row["Item Name"].ToString();
        //            ritem.BU = row["BU"].ToString().Trim() != "" ? Convert.ToInt32(row["BU"].ToString()) : 0;
        //            ritem.VendorCategory = row["VendorCategory"].ToString().Trim() != "" ? Convert.ToInt32(row["VendorCategory"].ToString()) : 0;
        //            ritem.ActualAvailableQuantity = row["Actual Available Quantity"].ToString().Trim() != "" ? row["Actual Available Quantity"].ToString() : "NA";
        //            ritem.Unit_Price = Convert.ToDecimal(row["Unit Price"].ToString());
        //            ritem.Unit_PriceUSD = Convert.ToDecimal(row["UnitPriceUSD"].ToString());
        //            ritem.Requestor = row["RequestorNT"].ToString();
        //            ritem.S_No = Convert.ToInt32(row["S#No"].ToString());
        //            ritem.BudgetCode = Convert.ToInt32(row["BudgetCode"].ToString()); 
        //            ritem.Material_Group = Convert.ToInt32(row["Material_Group"].ToString()/*.Split(',').ToList()*/); //Split the comma-separated material grps and pass as list (if dynamic multiselected material group option exists)
        //            ritem.UOM = Convert.ToInt32(row["UOM"].ToString());
        //            ritem.Order_Type = Convert.ToInt32(row["Order_Type"].ToString());

        //            viewList.Add(ritem);

        //        }
        //        return Json(new { data = viewList }, JsonRequestBehavior.AllowGet);

        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { data = viewList }, JsonRequestBehavior.AllowGet);

        //    }
        //    finally
        //    {
        //        BudgetingCloseConnection();
        //    }

        //    ////
        //    ///Linq Query
        //    ///

        //    //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //    ////using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //    //{
        //    //    List<ItemsCostList_Table> TempItemsList = db.ItemsCostList_Table.Where(x => x.Deleted == false && x.Category != null && x.Cost_Element != null).OrderBy(x=>x.Item_Name).ToList<ItemsCostList_Table>();
        //    //    List<RequestItemsRepoView1> viewList = new List<RequestItemsRepoView1>();
        //    //    foreach (ItemsCostList_Table item in TempItemsList)
        //    //    {
        //    //        RequestItemsRepoView1 ritem = new RequestItemsRepoView1();
        //    //        if(item.Deleted == true)
        //    //        {
        //    //            continue;
        //    //        }
        //    //        ritem.VKMYear = (int)item.VKM_Year;
        //    //        ritem.Category = int.Parse(item.Category);
        //    //        ritem.Comments = item.Comments;
        //    //        ritem.Cost_Element = int.Parse(item.Cost_Element);
        //    //        ritem.Currency = int.Parse(item.Currency);
        //    //        ritem.Item_Name = item.Item_Name;
        //    //        if(item.BU != null)
        //    //            ritem.BU = (int)item.BU;
        //    //        if (item.VendorCategory != null && item.VendorCategory.Trim() != "")
        //    //        {
        //    //            ritem.VendorCategory = int.Parse(item.VendorCategory);
        //    //        }
        //    //        else
        //    //        {
        //    //            ritem.VendorCategory = null;
        //    //        }
        //    //        if (item.Actual_Available_Quantity == null || item.Actual_Available_Quantity.Trim() == string.Empty)
        //    //            ritem.ActualAvailableQuantity = "NA";
        //    //        else
        //    //            ritem.ActualAvailableQuantity = item.Actual_Available_Quantity;

        //    //        ritem.Unit_Price = (decimal?)item.Unit_Price;
        //    //        ritem.Unit_PriceUSD = (decimal?)item.UnitPriceUSD;
        //    //        ritem.Requestor = item.RequestorNT;
        //    //        ritem.ActualAvailableQuantity = item.Actual_Available_Quantity;
        //    //        ritem.S_No = item.S_No;
        //    //        if (item.Deleted != true)
        //    //            ritem.Deleted = false;
        //    //        else
        //    //            ritem.Deleted = true;

        //    //        ritem.BudgetCode = item.BudgetCode.ToString();

        //    //        viewList.Add(ritem);
        //    //    }
        //        //return Json(new { data = viewList }, JsonRequestBehavior.AllowGet);
        //   // }
        //}


        /// <summary>
        /// function to enable update of an existing item and add a new item
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddOrEdit(InventoryItemsRepoEdit req)
        {

            try
            {
                using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

                {
                    System.Security.Principal.IPrincipal User = System.Web.HttpContext.Current.User;
                    //fetch the current user name
                    string presentUserName = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).EmployeeName;
                    string Query = "";
                    connection();
                    BudgetingOpenConnection();

                    if (req.S_No != 0)
                    {
                        Query = "UPDATE [ItemsCostList_Table] SET VKM_Year=@VKM_Year,Category=@Category,Comments=@Comments,[Cost Element]=@Cost_Element,Currency=@Currency,[Item Name]=@ItemName,BU=@BU,[VendorCategory]=@Vendorcategory,[Actual Available Quantity]=@ActualAvailableQuantity,[Unit Price]=@UnitPrice,UnitPriceUSD=@UnitPriceUSD,RequestorNT=@RequestorNT,RequestorNTID=@RequestorNTID,BudgetCode=@BudgetCode,[Material_Group]=@Material_Group,UOM=@UOM,Order_Type=@Order_Type,Deleted=@Deleted,UpdatedAt=@UpdatedAt  WHERE [S#No] = @ID";
                    }
                    else
                    {
                        //insert
                        Query = "INSERT INTO [ItemsCostList_Table] (VKM_Year,Category,Comments,[Cost Element],Currency,[Item Name],BU,[VendorCategory],[Actual Available Quantity],[Unit Price],UnitPriceUSD,RequestorNT,RequestorNTID,BudgetCode,[Material_Group],UOM,Order_Type,Deleted,UpdatedAt) VALUES(@VKM_Year,@Category,@Comments,@Cost_Element,@Currency,@ItemName,@BU,@VendorCategory,@ActualAvailableQuantity,@UnitPrice,@UnitPriceUSD,@RequestorNT,@RequestorNTID,@BudgetCode,@Material_Group,@UOM,@Order_Type,@Deleted,@UpdatedAt)";
                    }
                    SqlCommand cmd = new SqlCommand(Query, budgetingcon);
                    cmd.Parameters.AddWithValue("@VKM_Year", req.VKMYear);
                    cmd.Parameters.AddWithValue("@Category", req.Category);
                    cmd.Parameters.AddWithValue("@Comments", req.Comments != null ? req.Comments : "");
                    cmd.Parameters.AddWithValue("@Cost_Element", req.Cost_Element);
                    cmd.Parameters.AddWithValue("@Currency", req.Currency);
                    cmd.Parameters.AddWithValue("@ItemName", req.Item_Name);
                    cmd.Parameters.AddWithValue("@BU", req.BU);

                    cmd.Parameters.AddWithValue("@VendorCategory", req.VendorCategory != null ? req.VendorCategory : 0);
                    cmd.Parameters.AddWithValue("@ActualAvailableQuantity", req.ActualAvailableQuantity != null ? req.ActualAvailableQuantity : "" );
                    cmd.Parameters.AddWithValue("@UnitPrice", req.Unit_Price);
                    cmd.Parameters.AddWithValue("@UnitPriceUSD", req.Unit_PriceUSD);
                    cmd.Parameters.AddWithValue("@RequestorNT", presentUserName);
                    cmd.Parameters.AddWithValue("@RequestorNTID", User.Identity.Name.Split('\\')[1].ToUpper());
                    
                    cmd.Parameters.AddWithValue("@BudgetCode", req.BudgetCode);
                    cmd.Parameters.AddWithValue("@Material_Group", string.Join(",",req.Material_Group.ToArray()));
                    cmd.Parameters.AddWithValue("@UOM", req.UOM);
                    cmd.Parameters.AddWithValue("@Order_Type", req.Order_Type);
                    cmd.Parameters.AddWithValue("@Deleted", false); 
                    cmd.Parameters.AddWithValue("@UpdatedAt", DateTime.Now);
                    if (req.S_No != 0)
                        cmd.Parameters.AddWithValue("@ID", req.S_No);

                    cmd.ExecuteNonQuery();
                    BudgetingController.lstItems = db.ItemsCostList_Table.ToList();
                    return Json(new { success = true, message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);

                    //ItemsCostList_Table item = new ItemsCostList_Table();
                    //item.VKM_Year = req.VKMYear;
                    //item.Category = req.Category.ToString();
                    //item.Comments = req.Comments;
                    //item.Cost_Element = req.Cost_Element.ToString();
                    //item.Currency = req.Currency.ToString() ;
                    //item.BU = req.BU;
                    //item.Item_Name = req.Item_Name;
                    //item.RequestorNT = presentUserName;
                    //item.Unit_Price = (double)req.Unit_Price;
                    //item.UnitPriceUSD = (double)req.Unit_PriceUSD;
                    //item.S_No = req.S_No;
                    //item.Deleted = false;
                    //item.VendorCategory = req.VendorCategory.ToString();
                    //item.BudgetCode = Convert.ToInt32(req.BudgetCode.ToString());

                    //if (req.ActualAvailableQuantity == null || req.ActualAvailableQuantity.Trim() == string.Empty)
                    //    item.Actual_Available_Quantity = "NA";
                    //else
                    //    item.Actual_Available_Quantity = req.ActualAvailableQuantity;
                    //if (req.S_No == 0)
                    //{
                    //    db.ItemsCostList_Table.Add(item);
                    //    db.SaveChanges();
                    //    BudgetingController.lstItems = db.ItemsCostList_Table.ToList();
                    //    return Json(new { success = true, message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
                    //}
                    //else
                    //{

                    //    db.Entry(item).State = EntityState.Modified;
                    //    db.SaveChanges();
                    //    BudgetingController.lstItems = db.ItemsCostList_Table.ToList();
                    //    return Json(new { success = true, message = "Updated Successfully" }, JsonRequestBehavior.AllowGet);

                    //}

                }
            }
            catch(Exception ex)
            {
                return Json(new { success = true, message = "Please Try Again" }, JsonRequestBehavior.AllowGet);

            }
            finally
            {
                BudgetingCloseConnection();
            }
          
        }


        /// <summary>
        /// function to delete an existing item
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteItem(int id)
        {
            try
            {

                using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

                //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

                {

                    ItemsCostList_Table item = db.ItemsCostList_Table.Where(x => x.S_No == id).FirstOrDefault<ItemsCostList_Table>();
                    item.Deleted = true;
                    db.Entry(item).State = EntityState.Modified;
                    db.SaveChanges();
                    BudgetingController.lstItems = db.ItemsCostList_Table.ToList<ItemsCostList_Table>().FindAll(item1 => item1.Deleted != true);

                    return Json(new { success = true, message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
                }
                //WriteLog("Item deleted successfully " + DateTime.Now.ToString());
            }
            catch(Exception ex)
            {
                WriteLog("Item deletion error: " + ex.Message.ToString() + " " + DateTime.Now.ToString());
                return Json(new { success = false}, JsonRequestBehavior.AllowGet);
            }
        }


        /// <summary>
        /// function to get the unit price in USD of Item
        /// </summary>
        /// <param name="UnitPrice"></param>
        /// <returns></returns>
        //[HttpGet]
        //public ActionResult GetUnitPriceinUSD(double UnitPrice, int Currency)
        //{

        //    using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //    //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

        //    {
        //        var UnitPriceUSD = 0.0;
               
        //        decimal conversionEURate = /*(decimal)1.15;*/ db.Currency_Conversion_Table.FirstOrDefault(x => x.Currency.Contains("EUR")).ConversionRate_to_USD;

        //        decimal conversionINRate = /*(decimal)0.014;*/db.Currency_Conversion_Table.FirstOrDefault(x => x.Currency.Contains("INR")).ConversionRate_to_USD;

        //        //update the price in USD

        //        if (BudgetingController.lstCurrency.Find(curr => curr.ID.Equals(Currency)).Currency.ToUpper().Trim() == "USD")
        //            UnitPriceUSD = UnitPrice;
        //        else if (BudgetingController.lstCurrency.Find(curr => curr.ID.Equals(Currency)).Currency.ToUpper().Trim() == "INR")
        //            UnitPriceUSD = (double)((decimal)UnitPrice * conversionINRate);
        //        else if (BudgetingController.lstCurrency.Find(curr => curr.ID.Equals(Currency)).Currency.ToUpper().Trim() == "EUR")
        //            UnitPriceUSD = (double)((decimal)UnitPrice * conversionEURate);
        //        else
        //            UnitPriceUSD = 0.0;
        //        return Json(UnitPriceUSD, JsonRequestBehavior.AllowGet);
        //    }

        //}



        [HttpGet]
        public ActionResult GetEURINRates()
        {

            using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

            //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

            {
                EURINR eurinr = new EURINR();
                eurinr.EUR = /*(decimal)1.15;*/ db.Currency_Conversion_Table.FirstOrDefault(x => x.Currency.Contains("EUR")).ConversionRate_to_USD;
                eurinr.INR = /*(decimal)0.014;*/db.Currency_Conversion_Table.FirstOrDefault(x => x.Currency.Contains("INR")).ConversionRate_to_USD;
                eurinr.LB = /*(decimal)1.15;*/ db.Currency_Conversion_Table.FirstOrDefault(x => x.Currency.Contains("LB")).ConversionRate_to_USD;
                eurinr.JPY = /*(decimal)0.014;*/db.Currency_Conversion_Table.FirstOrDefault(x => x.Currency.Contains("JPY")).ConversionRate_to_USD;

                return Json(eurinr, JsonRequestBehavior.AllowGet);
            }

        }


        [HttpGet]
        public ActionResult Lookup()
        {
            
            LookupData lookupData = new LookupData();
            
            lookupData.Category_List = BudgetingController.lstPrdCateg;
            lookupData.CostElement_List = BudgetingController.lstCostElement;
            lookupData.Currency_List = BudgetingController.lstCurrency;
            lookupData.VendorCategory_List = BudgetingController.lstVendor;
            lookupData.BU_List = BudgetingController.lstBUs;
            lookupData.Order_Type_List = BudgetingController.lstOrderType;
            lookupData.UOM_List = BudgetingController.lstUOM;
            lookupData.BudgetCodeList = BudgetingController.BudgetCodeList;
            //List<BudgetCodeMaster> MasterRes = new List<BudgetCodeMaster>();

            //for (int i = 1; i < 4; i++)
            //{
                
            //    if (i == 1)
            //    {
            //        BudgetCodeMaster data1 = new BudgetCodeMaster();
            //        data1.CostElementID = i;
            //        data1.BudgetCode = "203";
            //        MasterRes.Add(data1);
            //    }
            //    else if (i == 2)
            //    {
            //        BudgetCodeMaster data1 = new BudgetCodeMaster();
            //        data1.CostElementID = i;
            //        data1.BudgetCode = "311";
            //        MasterRes.Add(data1);

            //        data1 = new BudgetCodeMaster();
            //        data1.CostElementID = i;
            //        data1.BudgetCode = "313";
            //        MasterRes.Add(data1);
            //    }
            //    else if (i == 3)
            //    {
            //        BudgetCodeMaster data1 = new BudgetCodeMaster();
            //        data1.CostElementID = i;
            //        data1.BudgetCode = "511";
            //        MasterRes.Add(data1);

            //        data1 = new BudgetCodeMaster();
            //        data1.CostElementID = i;
            //        data1.BudgetCode = "513";
            //        MasterRes.Add(data1);

            //        data1 = new BudgetCodeMaster();
            //        data1.CostElementID = i;
            //        data1.BudgetCode = "514";
            //        MasterRes.Add(data1);
            //    }
            //}

           // lookupData.BudgetCodeList = MasterRes;


            return Json(new { data = lookupData }, JsonRequestBehavior.AllowGet);

        }


        [HttpGet]
        public ActionResult MaterialGroup_Lookup() //created separately since dynamic insertion of Material Group by the user happens in Item Master
        {

            LookupData lookupData = new LookupData();
            DataTable dt = new DataTable();
            try
            {
                BudgetingController.InitialiseMaterialGroup(); //refresh the list with updates
                lookupData.Material_Group_list = BudgetingController.lstMaterialGroup;
                return Json(new { data = lookupData }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { data = lookupData }, JsonRequestBehavior.AllowGet);

            }
           
           

           
        }

        public void InsertMaterialGroup(string newMaterialGrpValue)
        {
            try
            {
                connection();
                BudgetingOpenConnection();
                string Query = "INSERT INTO [BGSW_MaterialGroup_Table] (MaterialGroup) VALUES(@newMaterialGrp)";

                SqlCommand cmd = new SqlCommand(Query, budgetingcon);
                cmd.Parameters.AddWithValue("@newMaterialGrp", newMaterialGrpValue);

                cmd.ExecuteNonQuery();
            }
            catch(Exception ex)
            {

            }
           
        }

        [HttpPost]
        public ActionResult DynamicInsert_MaterialGroup(string newMaterialGrpValue)
        {

            try
            {
                using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

                {
                    System.Security.Principal.IPrincipal User = System.Web.HttpContext.Current.User;
                    //fetch the current user name
                    string presentUserName = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).EmployeeName;
                    InsertMaterialGroup(newMaterialGrpValue);
                    string Query = "";
                    connection();
                    BudgetingOpenConnection();

                    DataTable dt = new DataTable();
                    Query = "SELECT * FROM [BGSW_MaterialGroup_Table] where MaterialGroup = @newMaterialGrp";
                    SqlCommand cmd = new SqlCommand(Query, budgetingcon);
                    cmd.Parameters.AddWithValue("@newMaterialGrp", newMaterialGrpValue);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    var lstMaterialGroup = new List<Material_Group_list>();

                    foreach (DataRow row in dt.Rows)
                    {
                        Material_Group_list item = new Material_Group_list();
                        item.ID = Convert.ToInt32(row["ID"]);
                        item.Material_Group = row["MaterialGroup"].ToString();
                        lstMaterialGroup.Add(item);
                    }
                    //BudgetingController.lstItems = .ToList();
                    return Json(new { data = lstMaterialGroup, success = true, message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);

                    //ItemsCostList_Table item = new ItemsCostList_Table();
                    //item.VKM_Year = req.VKMYear;
                    //item.Category = req.Category.ToString();
                    //item.Comments = req.Comments;
                    //item.Cost_Element = req.Cost_Element.ToString();
                    //item.Currency = req.Currency.ToString() ;
                    //item.BU = req.BU;
                    //item.Item_Name = req.Item_Name;
                    //item.RequestorNT = presentUserName;
                    //item.Unit_Price = (double)req.Unit_Price;
                    //item.UnitPriceUSD = (double)req.Unit_PriceUSD;
                    //item.S_No = req.S_No;
                    //item.Deleted = false;
                    //item.VendorCategory = req.VendorCategory.ToString();
                    //item.BudgetCode = Convert.ToInt32(req.BudgetCode.ToString());

                    //if (req.ActualAvailableQuantity == null || req.ActualAvailableQuantity.Trim() == string.Empty)
                    //    item.Actual_Available_Quantity = "NA";
                    //else
                    //    item.Actual_Available_Quantity = req.ActualAvailableQuantity;
                    //if (req.S_No == 0)
                    //{
                    //    db.ItemsCostList_Table.Add(item);
                    //    db.SaveChanges();
                    //    BudgetingController.lstItems = db.ItemsCostList_Table.ToList();
                    //    return Json(new { success = true, message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
                    //}
                    //else
                    //{

                    //    db.Entry(item).State = EntityState.Modified;
                    //    db.SaveChanges();
                    //    BudgetingController.lstItems = db.ItemsCostList_Table.ToList();
                    //    return Json(new { success = true, message = "Updated Successfully" }, JsonRequestBehavior.AllowGet);

                    //}

                }
            }
            catch (Exception ex)
            {
                return Json(new { success = true, message = "Please Try Again" }, JsonRequestBehavior.AllowGet);

            }
            finally
            {
                BudgetingCloseConnection();
            }

        }


        /// <summary>
        /// function to get the Initial values to be filled automatically when a new request is created, based on the previous input by the user
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult InitRowValues()
        {
            try
            {

                using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

                //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

                {

                    RequestItemsRepoView1 ritem = new RequestItemsRepoView1();
                    var UserNTID = System.Web.HttpContext.Current.User.Identity.Name.Split('\\')[1].ToUpper();
                    string Query = "";
                    bool success = false;
                    connection();
                    Query = "IF EXISTS(SELECT RequestorNTID from [ItemsCostList_Table] where RequestorNTID = @User)SELECT TOP 1 VKM_Year,Category,[Cost Element],Currency,BU,[VendorCategory],BudgetCode,isnull([Material_Group],'81') as Material_Group,isnull(UOM,'29') as UOM,isnull(Order_Type,'3') as Order_Type from [ItemsCostList_Table] where RequestorNTID = @User order by UpdatedAt desc";
                    BudgetingOpenConnection();
                    SqlCommand cmd = new SqlCommand(Query, budgetingcon);
                    cmd.Parameters.AddWithValue("@User ", UserNTID);
                    SqlDataReader row = cmd.ExecuteReader();
                    if (row.HasRows)
                    {
                        row.Read();
                        ritem.VKMYear = Convert.ToInt32(row["VKM_Year"].ToString());
                        ritem.Category = Convert.ToInt32(row["Category"].ToString());
                        ritem.Cost_Element = Convert.ToInt32(row["Cost Element"].ToString());
                        ritem.Currency = Convert.ToInt32(row["Currency"].ToString());
                        ritem.BU = Convert.ToInt32(row["BU"].ToString());
                        ritem.VendorCategory = Convert.ToInt32(row["VendorCategory"].ToString());
                        ritem.BudgetCode = Convert.ToInt32(row["BudgetCode"].ToString());
                        ritem.Material_Group = Convert.ToInt32(row["Material_Group"].ToString());
                        ritem.UOM = Convert.ToInt32(row["UOM"].ToString());
                        ritem.Order_Type = Convert.ToInt32(row["Order_Type"].ToString());
                       

                    }
                    //else
                    //{
                    //    temp.BU = 0;
                    //    temp.OEM = 0;
                    //    temp.Reviewer_2 = "";
                    //}


                    return Json(new { data = ritem }, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                WriteLog("Error - InitRowValues : NTID - " + User.Identity.Name.Split('\\')[1].Trim().ToUpper() + ", ErrorMsg : " + ex + ", At - " + DateTime.Now);
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
            finally
            {
                BudgetingCloseConnection();
            }

        }

    }

    public partial class BudgetCodeMaster
    {
        public int ID { get; set; }
        public int CostElementID { get; set; }
        public string Budget_Code { get; set; }
        public string Budget_Code_Description { get; set; }
    }
    public partial class UOM_list
    {
        public int ID { get; set; }
        public string UOM { get; set; }
        public string Order_Type { get; set; }
    }
    public partial class Order_Type_list
    {
        public int ID { get; set; }
        public string Order_Type { get; set; }
    }
    public partial class Material_Group_list
    {
        public int ID { get; set; }
        public string Material_Group { get; set; }
    }

    public partial class InventoryItemsRepoEdit
    {
        public int S_No { get; set; }
       public int VKMYear { get; set; }
        public string Item_Name { get; set; }
        
        public int Category { get; set; }
      
        public int Cost_Element { get; set; }

        public int BU { get; set; }
        public decimal Unit_Price { get; set; }
       
        public int Currency { get; set; }

        public string Comments { get; set; }
      
        public string Requestor { get; set; }

        public decimal Unit_PriceUSD { get; set; }
        public Nullable<int> VendorCategory { get; set; }
        public string ActualAvailableQuantity { get; set; }

        public string BudgetCode { get; set; }
        public string Order_Type { get; set; }
        public string UOM { get; set; }
        public List<string> Material_Group { get; set; }

    }

    public class ItemvendorList
    {
        private string csvitem;
        private string csvvendor;
     

        public string CSVItem
        {
            get { return this.csvitem; }
            set { this.csvitem = value; }
        }
        public string CSVVendor
        {
            get { return this.csvvendor; }
            set { this.csvvendor = value; }
        }
        
    }

    public class EURINR
    {
       public decimal EUR { get; set; }
       public decimal INR { get; set; }

        public decimal LB { get; set; }
        public decimal JPY { get; set; }
    }
}
