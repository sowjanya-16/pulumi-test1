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
using System.Web.UI;
using System.Web.UI.WebControls;
using ClosedXML.Excel;
using LC_Reports_V1.Models;

namespace LC_Reports_V1.Controllers
{
   // [Authorize(Users = @"apac\jov6cob,apac\rba3cob,apac\din2cob,apac\MTA2COB,apac\mxk8kor,apac\nnj6kor,apac\pks5cob,apac\chb1kor,apac\sbr2kor,apac\rau2kor,apac\bbv5kor,apac\rmm7kor,apac\gpa3kor,apac\mae9cob,apac\oig1cob,apac\rma5cob,apac\mow4kor,apac\SIF1COB,apac\mhv7kor,apac\rpi8kor")]
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

        public ActionResult Index()
        {


            if (BudgetingController.lstUsers == null || BudgetingController.lstUsers.Count == 0)
            {
                
                return RedirectToAction("Index", "Budgeting");
            }
           

            return View();
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
                    using (BudgetingToolServerDBEntities1 db = new BudgetingToolServerDBEntities1())
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
                                    item.Currency = BudgetingController.lstCurrency.Find(curr => curr.Currency.Trim().Replace(" ", "").ToLower().Equals(row[3].ToString().Trim().Replace(" ", "").ToLower())).ID.ToString();
                                    item.Actual_Available_Quantity = (row[4].ToString().Trim() != null && row[4].ToString().Replace(" ", "").Replace("\"", "").Trim() != "") ? row[4].ToString() : "NA";

                                    item.Unit_Price = (int)decimal.Parse(row[5].ToString());
                                    item.VendorCategory = (row[6].ToString().Trim().Replace(" ", "").Replace("\"", "").ToLower() != null && row[6].ToString().Trim().Replace(" ", "").Replace("\"", "").ToLower() != "") ? BudgetingController.lstVendor.Find(vendor => vendor.VendorCategory.Trim().Replace(" ", "").Replace("\"", "").ToLower().Equals(row[6].ToString().Trim().Replace(" ", "").Replace("\"", "").ToLower())).ID.ToString() : "";


                                    //to add the unitprice in usd
                                    if (row[3].ToString() == "USD")
                                        item.UnitPriceUSD = item.Unit_Price;
                                    else if (row[3].ToString() == "EUR")
                                        item.UnitPriceUSD = (double?)((decimal)item.Unit_Price * conversionEURate);
                                    else
                                        item.UnitPriceUSD = (double?)((decimal)item.Unit_Price * conversionINRate);

                                    item.Comments = row[7].ToString();
                                    item.BU = BudgetingController.lstBUs.Find(bu => bu.BU.Trim().Replace(" ", "").ToLower().Equals(row[8].ToString().Replace(" ", "").Trim().ToLower())).ID;
                                    item.VKM_Year = int.Parse(row[9].ToString());
                                    item.Deleted = false;
                                    item.RequestorNT = presentUserName;
                                    item.UpdatedAt = DateTime.Now;

                                    if (db.ItemsCostList_Table.ToList().Find(x => x.Item_Name.ToUpper().Trim() == item.Item_Name.ToUpper().Trim() && x.BU == item.BU && x.Deleted == false && x.VKM_Year == item.VKM_Year) == null)
                                    {
                                        db.ItemsCostList_Table.Add(item);
                                        db.SaveChanges();
                                    }
                                    else
                                    {
                                        connection();
                                        BudgetingOpenConnection();
                                        string Query = " Exec [dbo].[UpdateMasterList] '" + item.Item_Name + "', '" + item.Category + "' ,'" + item.Cost_Element + "' ,'" + item.Currency + "','" + item.Actual_Available_Quantity + "','" + item.Unit_Price + "','" + item.VendorCategory + "','" + item.UnitPriceUSD + "','" + item.Comments + "','" + item.BU + "','" + item.VKM_Year + "','" + item.Deleted + "','" + item.RequestorNT + "'";
                                        SqlCommand cmd = new SqlCommand(Query, budgetingcon);
                                        cmd.ExecuteNonQuery();
                                        BudgetingCloseConnection();
                                    }

                                }
                                BudgetingController.lstItems = db.ItemsCostList_Table.ToList();
                            }
                            catch (Exception ex)
                            {
                                errcount++;

                                if (errcount >1)
                                    msg +=   " | \n" + "Empty/invalid cell found :" + row[0].ToString() ;
                                else
                                    msg += "Empty/invalid cell found :" + row[0].ToString();

                               
                            }

                        }
                    }
               
                    
                        if (errcount > 0) {
                        return Json(new { dataerror = true, errormsg = msg + " \nThe valid requests were imported. Please find the errors listed. " });
                    }



                    else {
                        return Json(new { dataerror = false, successmsg = " The requests were successfully imported. Please edit any INVALID fields in the table." });
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
            string fileName = "MasterListTemplate.xlsx";
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

            string filename = @"Master_Item_List_" + DateTime.Now.ToShortDateString() + ".xlsx";
            using (BudgetingToolServerDBEntities1 db = new BudgetingToolServerDBEntities1())
            {
                System.Data.DataTable dt = new System.Data.DataTable("Request_List");
                dt.Columns.AddRange(new DataColumn[12] {
                                            new DataColumn("Item Name"),
                                            new DataColumn("Category"),
                                            new DataColumn("Cost Element"),
                                            new DataColumn("Currency"),
                                            new DataColumn("Actual Available Quantity"),
                                            new DataColumn("Unit Price",typeof(decimal)),
                                            new DataColumn("Unit Price (in USD)",typeof(decimal)),
                                            new DataColumn("Vendor"),
                                            new DataColumn("Comments"),
                                            new DataColumn("BU"),
                                            new DataColumn("Requestor"),
                                            new DataColumn("VKM Year"),
                });

                var requests = db.ItemsCostList_Table.Where(item => item.Deleted != true && item.Cost_Element != null && item.Category != null).Select(x => new { x.Item_Name, x.Category, x.Cost_Element, x.Currency, x.Actual_Available_Quantity, x.Unit_Price, x.UnitPriceUSD, x.VendorCategory, x.Comments,x.BU, x.RequestorNT, x.VKM_Year }).ToList(); ;

                foreach (var request in requests)
                {
                   
                    dt.Rows.Add(request.Item_Name,
                        BudgetingController.lstPrdCateg.Find(cat => cat.ID.ToString().Equals(request.Category)).Category.Trim(),
                        BudgetingController.lstCostElement.Find(cost => cost.ID.ToString().Equals(request.Cost_Element)).CostElement,
                        BudgetingController.lstCurrency.Find(curr => curr.ID.ToString().Equals(request.Currency)).Currency.Trim(),
                        request != null ? request.Actual_Available_Quantity : "NA",
                        request.Unit_Price,
                        request.UnitPriceUSD,
                        (request.VendorCategory != null && request.VendorCategory.Trim() != "") ? BudgetingController.lstVendor.Find(vendor => vendor.ID.ToString().Equals(request.VendorCategory.Trim())).VendorCategory : "",
                        request.Comments,
                        request.BU != null?BudgetingController.lstBUs.Find(bu => bu.ID.Equals(request.BU)).BU.Trim():"",
                        request.RequestorNT,
                        request.VKM_Year);
                }

                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dt);
                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);
                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
                    }
                }

            }
        }


        /// <summary>
        /// function to get the Item Masterlist 
        /// </summary>
        /// <returns></returns>
        public ActionResult GetData()
        {
            using (BudgetingToolServerDBEntities1 db = new BudgetingToolServerDBEntities1())
            {
                List<ItemsCostList_Table> TempItemsList = db.ItemsCostList_Table.Where(x => x.Deleted == false && x.Category != null && x.Cost_Element != null).OrderBy(x=>x.Item_Name).ToList<ItemsCostList_Table>();
                List<RequestItemsRepoView1> viewList = new List<RequestItemsRepoView1>();
                foreach (ItemsCostList_Table item in TempItemsList)
                {
                    RequestItemsRepoView1 ritem = new RequestItemsRepoView1();
                    if(item.Deleted == true)
                    {
                        continue;
                    }
                    ritem.VKMYear = (int)item.VKM_Year;
                    ritem.Category = int.Parse(item.Category);
                    ritem.Comments = item.Comments;
                    ritem.Cost_Element = int.Parse(item.Cost_Element);
                    ritem.Currency = int.Parse(item.Currency);
                    ritem.Item_Name = item.Item_Name;
                    if(item.BU != null)
                        ritem.BU = (int)item.BU;
                    if (item.VendorCategory != null && item.VendorCategory.Trim() != "")
                    {
                        ritem.VendorCategory = int.Parse(item.VendorCategory);
                    }
                    else
                    {
                        ritem.VendorCategory = null;
                    }
                    if (item.Actual_Available_Quantity == null || item.Actual_Available_Quantity.Trim() == string.Empty)
                        ritem.ActualAvailableQuantity = "NA";
                    else
                        ritem.ActualAvailableQuantity = item.Actual_Available_Quantity;

                    ritem.Unit_Price = (decimal?)item.Unit_Price;
                    ritem.Unit_PriceUSD = (decimal?)item.UnitPriceUSD;
                    ritem.Requestor = item.RequestorNT;
                    ritem.ActualAvailableQuantity = item.Actual_Available_Quantity;
                    ritem.S_No = item.S_No;
                    if (item.Deleted != true)
                        ritem.Deleted = false;
                    else
                        ritem.Deleted = true;

                    viewList.Add(ritem);
                }
                return Json(new { data = viewList.FindAll(item=>item.Deleted != true) }, JsonRequestBehavior.AllowGet);
            }
        }


        /// <summary>
        /// function to enable update of an existing item and add a new item
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult AddOrEdit(InventoryItemsRepoEdit req)
        {

            using (BudgetingToolServerDBEntities1 db = new BudgetingToolServerDBEntities1())
            {
                System.Security.Principal.IPrincipal User = System.Web.HttpContext.Current.User;
                string presentUserName = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())).EmployeeName;
                

                ItemsCostList_Table item = new ItemsCostList_Table();
                item.VKM_Year = req.VKMYear;
                item.Category = req.Category.ToString();
                item.Comments = req.Comments;
                item.Cost_Element = req.Cost_Element.ToString();
                item.Currency = req.Currency.ToString() ;
                item.BU = req.BU;
                item.Item_Name = req.Item_Name;
                item.RequestorNT = presentUserName;
                item.Unit_Price = (double)req.Unit_Price;
                item.UnitPriceUSD = (double)req.Unit_PriceUSD;
                item.S_No = req.S_No;
                item.Deleted = false;
                item.VendorCategory = req.VendorCategory.ToString();
                if (req.ActualAvailableQuantity == null || req.ActualAvailableQuantity.Trim() == string.Empty)
                    item.Actual_Available_Quantity = "NA";
                else
                    item.Actual_Available_Quantity = req.ActualAvailableQuantity;
                if (req.S_No == 0)
                {
                    db.ItemsCostList_Table.Add(item);
                    db.SaveChanges();
                    BudgetingController.lstItems = db.ItemsCostList_Table.ToList();
                    return Json(new { success = true, message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    
                    db.Entry(item).State = EntityState.Modified;
                    db.SaveChanges();
                    BudgetingController.lstItems = db.ItemsCostList_Table.ToList();
                    return Json(new { success = true, message = "Updated Successfully" }, JsonRequestBehavior.AllowGet);

                }
                
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
            using (BudgetingToolServerDBEntities1 db = new BudgetingToolServerDBEntities1())
            {

                ItemsCostList_Table item = db.ItemsCostList_Table.Where(x => x.S_No == id).FirstOrDefault<ItemsCostList_Table>();
                item.Deleted = true;
                db.Entry(item).State = EntityState.Modified;
                db.SaveChanges();
                BudgetingController.lstItems = db.ItemsCostList_Table.ToList<ItemsCostList_Table>().FindAll(item1 => item1.Deleted != true);
                
                return Json(new { success = true, message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
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
        //    using (BudgetingToolServerDBEntities1 db = new BudgetingToolServerDBEntities1())
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
            using (BudgetingToolServerDBEntities1 db = new BudgetingToolServerDBEntities1())
            {
                EURINR eurinr = new EURINR();
                eurinr.EUR = /*(decimal)1.15;*/ db.Currency_Conversion_Table.FirstOrDefault(x => x.Currency.Contains("EUR")).ConversionRate_to_USD;
                eurinr.INR = /*(decimal)0.014;*/db.Currency_Conversion_Table.FirstOrDefault(x => x.Currency.Contains("INR")).ConversionRate_to_USD;

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


            return Json(new { data = lookupData }, JsonRequestBehavior.AllowGet);

        }
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

    }
}