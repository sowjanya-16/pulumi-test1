using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Web.Script.Serialization;
using LC_Reports_V1.Models;

namespace LC_Reports_V1.Controllers
{
    public class POSubItemController : Controller
    {
        // GET: POSubItem
        public ActionResult Index()
        {

            return View();
        }


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

        [HttpPost]
        public ActionResult GetPODetails()
        {
            DataTable dt = new DataTable();
            try
            {

                connection();

                //      string Query = " Select  [ID]" +
                //",[RequestID]" +
                //",[VKMYear]" +
                //",[BU] " +
                //",[OEM]" +
                //",[GROUP]" +
                //",[Dept]" +
                //",[Item Name]" +
                //",[Ordered Quantity]" +
                //",[PO Number]" +
                //",[PIF ID]" +
                //",[Fund]" +
                //",[Fund Center]" +
                //",BudgetCode" +
                //",ItemDescription" +
                //",[PO Quantity]" +
                //",[UOM]" +
                //",[UnitPrice]" +
                //",[Netvalue]" +
                //",[Netvalue_USD]" +
                //",[Currency]" +
                //",[Plant]" +
                //",[PO Created On]" +
                //",VendorName" +
                //",[CW]" +
                //",[Tentative Delivery Date]" +
                //",[Actual Delivery Date]" +
                //",[Difference in DeliveRy Date]" +
                //",[Actal Amt]" +
                //",[Negotiated Amt]" +
                //",[Savings]" +
                //",[Current status]" +
                //",[PO Remarks]" +
                //"from [PODetails_Table] where VKMYear is not null order by ID";

                string Query = " Exec [dbo].[POData_BonaparteView] '" + User.Identity.Name.Split('\\')[1].Trim() + "' ";

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









            //JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            //List<Dictionary<string, object>> parentRow = new List<Dictionary<string, object>>();
            //Dictionary<string, object> childRow;
            //foreach (DataRow row in dt.Rows)
            //{
            //    childRow = new Dictionary<string, object>();
            //    foreach (DataColumn col in dt.Columns)
            //    {
            //        childRow.Add(col.ColumnName, row[col]);
            //    }
            //    parentRow.Add(childRow);
            //}

            //jsSerializer.MaxJsonLength = Int32.MaxValue;

            //var resultData = parentRow;
            //var result = new ContentResult
            //{
            //    Content = jsSerializer.Serialize(resultData),
            //    ContentType = "application/json"
            //};

            //JsonResult result1 = Json(result);
            //result1.MaxJsonLength = Int32.MaxValue;
            //result1.JsonRequestBehavior = JsonRequestBehavior.AllowGet;


            //return Json(new { data = result1, JsonRequestBehavior.AllowGet });

            return Json(new { data = result, success = true, JsonRequestBehavior.AllowGet });
            //}

        }

        //Function to add the data from Bonaparte view to HW Inventory
        [HttpPost]
        public ActionResult LinkToInventory(HardwareInventoryPO req)
        {
            DataTable dt = new DataTable();

            connection();
            dt = new DataTable();
            string Query = "";
            System.Security.Principal.IPrincipal User = System.Web.HttpContext.Current.User;


            //Add fields to the Hardware table from PODetails
            Query = "INSERT into[HWInventory_Table]" +
                              "(" +
                               "[BU]" +
                               ",[OEM]" +
                               ",[GROUP]" +
                               ",[Item Name]" +
                               ",[ItemName_Planner]" +
                               ",[Inventory Type]" +
                               ",[Serial Number]" +
                               ",[Bond Number]" +
                               ",[Bond Date]" +
                               ",[Asset Number]" +
                               ",[HW Responsible]" +
                               ",[Handover To]" +
                               ",[UOM]" +
                               ",[POQty]" +
                                ",[Quantity]" +
                               //",[Available Qty]" +
                               ",[Remarks]" +
                               ",[ALM Number]" +
                               ",[Mode]" +
                               ",[Actual Delivery Date]" +
                               ")" +
                               "values" +
                               "(" +
                               " @BU  " +
                               ", @OEM  " +
                               ", @Group" +
                               ", @ItemName" +
                               ", @ItemName_Planner " +
                               ", @InventoryType " +
                               ", @SerialNo " +
                               ", @BondNo " +
                               ", @BondDate " +
                               ", @AssetNo " +
                               ", @HWResponsible" +
                               ", @HandoverTo" +
                               ", @UOM" +
                               ", @POQty" +
                               ", @Quantity" +
                               //", @AvailableQty" +
                               ", @Remarks" +
                               ", @ALMNo" +
                               ", @Mode" +
                               ",@ActualDeliveryDate" +
                               ")";

            SqlCommand cmd = new SqlCommand(Query, conn);
            cmd.Parameters.AddWithValue("@BU", req.BU);
            cmd.Parameters.AddWithValue("@OEM", req.OEM);
            cmd.Parameters.AddWithValue("@GROUP", req.GROUP);
            cmd.Parameters.AddWithValue("@ItemName", req.ItemDescription);
            //if (req.ItemName_Planner == 0)
            //{
            //    cmd.Parameters.AddWithValue("@ItemName_Planner", DBNull.Value);
            //}
            //else
            cmd.Parameters.AddWithValue("@ItemName_Planner", req.ItemName);

            cmd.Parameters.AddWithValue("@InventoryType", req.InventoryType);
            cmd.Parameters.AddWithValue("@SerialNo", req.SerialNo);
            cmd.Parameters.AddWithValue("@BondNo", req.BondNo);
            cmd.Parameters.AddWithValue("@BondDate", req.BondDate);
            cmd.Parameters.AddWithValue("@AssetNo", req.AssetNo);
            cmd.Parameters.AddWithValue("@HWResponsible", req.HardwareResponsible);
            cmd.Parameters.AddWithValue("@HandoverTo", req.HandoverTo);
            cmd.Parameters.AddWithValue("@UOM", req.UOM);
            cmd.Parameters.AddWithValue("@POQty", req.POQuantity);
            cmd.Parameters.AddWithValue("@Quantity", req.Quantity);
            //cmd.Parameters.AddWithValue("@AvailableQty", req.AvailableQty);
            cmd.Parameters.AddWithValue("@Remarks", req.Remarks);
            cmd.Parameters.AddWithValue("@ALMNo", req.ALMNo);
            cmd.Parameters.AddWithValue("@ActualDeliveryDate", req.ActualDeliveryDate);
            cmd.Parameters.AddWithValue("@Mode", req.Mode);
            cmd.Parameters.AddWithValue("@ID", req.ID);

            try
            {
                OpenConnection();
                cmd.ExecuteNonQuery();
                Console.WriteLine("Records Inserted Successfully");
            }
            catch (Exception ex)
            {
                // MessageBox.Show(" Not Updated");
            }
            finally
            {
                CloseConnection();
            }
            return Json(new { data = GetPODetails(), success = true }, JsonRequestBehavior.AllowGet);
        }

        //query to insert in the Spare inventory
        [HttpPost]
        public ActionResult LinkToSpare(SpareInventoryPO req)
        {

            List<SpareInventoryPO> result = new List<SpareInventoryPO>();
            List<SpareInventoryPO> result2 = new List<SpareInventoryPO>();
            List<CurrencyConversion> currList = new List<CurrencyConversion>();
            var spcalc = "";
            var BANLabcar = 40; 
            var COBLabcar = 50;
            var conversion = 0.00;
            DataTable dt = new DataTable();
            //string cobq,banq;
            try
            {
                
                DataSet dt_for_all = new DataSet();
                connection();
                OpenConnection();
                //check for the banqty and cobqty if the qty is present for the incoming sparee hw
                //Get the SpareCalc from configuration\
                //Get conversion rate from Conversion table
                string Queryall = "Select  [ID], BANQty,COBQty,SpareHW from [SpareInventory_Table] where SpareHW = '" + req.SpareHW + "';Select [SpareCalc] from [Spare_Configuration_Table] where [SpareHW]= '" + req.SpareHW + "';Select  [ID],[ConversionRate_to_USD] from [Currency_Conversion_Table] where ID = '" + req.Currency + "'  ";
                //check for the banqty and cobqty if the qty is present for the incoming sparee hw
                //    string Query1 = " Select  [ID]" +
                //      ",[BANQty]" +
                //      ",[COBQty]" +
                //      ",[SpareHW]" +


                //"from [SpareInventory_Table] where SpareHW = '" + req.SpareHW + "'";.

                //    string Query2 = " Select [SpareCalc] from [Spare_Configuration_Table] where [SpareHW]= '" + req.SpareHW + "' ";
                //string Query3 = "Select []";


                //SqlCommand cmd1 = new SqlCommand(Query1, conn);
                //SqlCommand cmd2 = new SqlCommand(Query2, conn);
                //spcalc = cmd2.ExecuteScalar().ToString();
                //SqlDataAdapter da = new SqlDataAdapter(cmd1);
                //SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                //da.Fill(dt);
                ////da2.Fill(dt);
                //CloseConnection();
                //Dataset for Quantity
                SqlCommand cmdall = new SqlCommand(Queryall, conn);
                SqlDataAdapter da = new SqlDataAdapter(cmdall);
                da.Fill(dt_for_all);
                CloseConnection();
                foreach (DataRow row in dt_for_all.Tables[0].Rows)

                {
                    SpareInventoryPO item = new SpareInventoryPO();
                    item.ID = int.Parse(row["ID"].ToString());
                    item.BANQty = int.Parse(row["BANQty"].ToString());
                    //Assign the BANQty to zero while sending the data from POSubItem
                    if (item.BANQty == 0)
                    {
                        req.BANQty = 0;
                    }
                    else
                    {
                        //Same entry exists hence the new qty is added to the previous qty
                        req.BANQty = req.BANQty + item.BANQty;
                    }
                    item.COBQty = int.Parse(row["COBQty"].ToString());
                    if (item.COBQty == 0)
                    {
                        req.COBQty = 0;

                    }
                    else
                    {
                        //Same entry exists hence the new qty is added to the previous qty
                        req.COBQty = req.COBQty + item.COBQty;
                    }
                    item.SpareHW = int.Parse(row["SpareHW"].ToString());
                    result.Add(item);
                }
                //SpareInventory datatable for getting Spare calc
                foreach (DataRow row in dt_for_all.Tables[1].Rows)
                {
                    SpareInventoryPO item = new SpareInventoryPO();
                    item.SpareCalc = row["SpareCalc"].ToString();
                    spcalc = item.SpareCalc;
                    result2.Add(item);
                }
                //getting comversion rate
                foreach (DataRow row in dt_for_all.Tables[2].Rows)
                {                   
                    CurrencyConversion item = new CurrencyConversion();
                    item.ID = int.Parse(row["ID"].ToString());
                    //item.Currency = row["Currency"].ToString();
                    item.CurrencyRate = row["ConversionRate_to_USD"].ToString();
                    conversion = float.Parse(item.CurrencyRate);

                    currList.Add(item);

                }

                ////storing selected data in the datatable,, quantities are added and stored locationwise
                //foreach (DataRow row in dt.Rows)
                //{
                //    SpareInventoryPO item = new SpareInventoryPO();
                //    item.ID = int.Parse(row["ID"].ToString());
                //    item.BANQty = int.Parse(row["BANQty"].ToString());
                //    if (item.BANQty == 0)
                //    {
                //        req.BANQty = 0;
                //    }
                //    else
                //    {
                //        req.BANQty = req.BANQty+item.BANQty;
                //    }
                //    item.COBQty = int.Parse(row["COBQty"].ToString());
                //    if (item.COBQty == 0)
                //    {
                //        req.COBQty = 0;

                //    }
                //    else
                //    {
                //        req.COBQty = req.COBQty+item.COBQty;
                //    }
                //    item.SpareHW = int.Parse(row["SpareHW"].ToString());
                //    result.Add(item);
                //}
                DataTable dt1 = new DataTable();

                connection();
                dt1 = new DataTable();
                string Query = "";
                System.Security.Principal.IPrincipal User = System.Web.HttpContext.Current.User;






                //if there is any data present in the datatable, previous entry for spare hw exists.
                //if new entry then insert
                if (result.Count == 0)

                {
                    Query = "INSERT into[SpareInventory_Table]" +
                                  "(" +
                                   "[Currency]" +
                                   ",[PriceOriginal]" +
                                   ",[POQty]" +
                                   ",[SpareHW]" +
                                   ",[BANQty]" +
                                   ",[COBQty]" +
                                   ",[TotalQty]" +
                                   ",[SpareCalc]" +
                                   ",[BANreqd]" +
                                   ",[COBreqd]" +
                                   ",[BANUnderRepair]" +
                                   ",[COBUnderRepair]" +
                                   ",[PriceUSD]" +
                                   ",[Status]" +
                                   ",[BANdiff]" +
                                   ",[COBdiff]" +
                                   ",[BANTotalPrice]" +
                                   ",[COBTotalPrice]" +


                                   ")" +
                                   "values" +
                                   "(" +
                                   " @Currency  " +
                                   ", @PriceOriginal " +
                                   ", @POQty" +
                                   ", @SpareHW" +
                                   ", @BANQuantity " +
                                   ", @COBQuantity " +
                                   ", @TotalQuantity" +
                                   ", @SpareCalc" +
                                   ", @BANreqd" +
                                   ", @COBreqd" +
                                   ", @BANUnderRepair" +
                                   ", @COBUnderRepair" +
                                   ", @PriceUSD" +
                                   ", @Status" +
                                   ", @BANdiff" +
                                   ", @COBdiff" +
                                   ", @BANTotalPrice" +
                                   ", @COBTotalPrice" +

                                   ")";
                    //simultaneously inserting into the spare config table

                    string Query2 = "INSERT into[Spare_Configuration_Table]" +
                             "(" +
                             "[SpareHW]" +
                             ",[SpareCount]" +
                             ",[HWCount]" +
                             ",[MultiplicationFactor]" +
                             ",[SpareCalc]" +
                              ")" +
                              "values" +
                              "(" +
                              " @SpareHW  " +
                              " ,@SpareCount  " +
                              " ,@HWCount  " +
                              " ,@MultiplicationFactor  " +
                              " ,@SpareCalc  " +


                              ")";

                    SqlCommand commd = new SqlCommand(Query2, conn);
                    List<SpareConfiguration> req1 = new List<SpareConfiguration>();
                    commd.Parameters.AddWithValue("@SpareHW", req.SpareHW);
                    commd.Parameters.AddWithValue("@SpareCount", string.Empty);
                    commd.Parameters.AddWithValue("@HWCount", string.Empty);
                    commd.Parameters.AddWithValue("@MultiplicationFactor", string.Empty);
                    commd.Parameters.AddWithValue("@SpareCalc", "0");
                    try
                    {
                        OpenConnection();
                        commd.ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {

                    }
                    CloseConnection();

                }

                else
                {
                    Query = "UPDATE [SpareInventory_Table] SET " +

                           "[Currency]= @Currency," +
                           "[PriceOriginal]= @PriceOriginal," +
                           "[POQty]= @POQty, " +
                           "[BANQty]= @BANQuantity, " +
                           "[COBQty]= @COBQuantity, " +
                           "[TotalQty]= @TotalQuantity, " +
                           "[SpareCalc]= @SpareCalc, " +
                           "[BANreqd]= @BANreqd, " +
                           "[COBreqd]= @COBreqd, " +
                           "[BANdiff]= @BANdiff, " +
                           "[COBdiff]= @COBdiff, " +
                           "[BANUnderRepair]= @BANUnderRepair, " +
                           "[COBUnderRepair]= @COBUnderRepair, " +
                           "[Status]= @Status, " +
                           "[PriceUSD]= @PriceUSD, " +
                           "[BANTotalPrice]= @BANTotalPrice, " +
                           "[COBTotalPrice]= @COBTotalPrice " +

                           //"[Available Qty] = @AvailableQty " +
                           "WHERE SpareHW = @SpareHW";
                }



                //if(req.Plant == "COB")
                //{
                //    Query = "INSERT into[SpareInventory_Table]" +
                //                  "(" +
                //                   "[Currency]" +
                //                   ",[PriceOriginal]" +
                //                   ",[POQty]" +
                //                   ",[SpareHW]" +
                //                   ",[COBQty]" +
                //                   ",[BANQty]" +
                //                   ",[TotalQty]" +


                //                   ")" +
                //                   "values" +
                //                   "(" +
                //                   " @Currency  " +
                //                   ", @PriceOriginal " +
                //                   ", @POQty" +
                //                   ", @SpareHW" +
                //                   ", @SpareQuantity " +
                //                   ", @BANQuantity" +
                //                   ", @TotalQuantity" +



                //                   ")";

                //}

                //Add fields to the Hardware table from PODetails

                SqlCommand cmd = new SqlCommand(Query, conn);
                cmd.Parameters.AddWithValue("@Currency", req.Currency);
                cmd.Parameters.AddWithValue("@PriceOriginal", req.UnitPrice);
                cmd.Parameters.AddWithValue("@POQty", req.POQuantity);
                cmd.Parameters.AddWithValue("@SpareHW", req.SpareHW);

                if (req.Plant == "BAN")
                {
                    req.BANQty = req.BANQty + req.SpareQuantity;
                    cmd.Parameters.AddWithValue("@BANQuantity", req.BANQty);

                    cmd.Parameters.AddWithValue("@COBQuantity", req.COBQty);
                    req.TotalQty = req.COBQty + req.BANQty;
                    cmd.Parameters.AddWithValue("@TotalQuantity", req.TotalQty);
                }
                if (req.Plant == "COB")
                {
                    req.COBQty = req.COBQty + req.SpareQuantity;
                    cmd.Parameters.AddWithValue("@COBQuantity", req.COBQty);

                    cmd.Parameters.AddWithValue("@BANQuantity", req.BANQty);
                    req.TotalQty = req.COBQty + req.BANQty;
                    cmd.Parameters.AddWithValue("@TotalQuantity", req.TotalQty);
                }





                if (result2.Count == 0)
                {
                    spcalc = "0";
                }

                cmd.Parameters.AddWithValue("@SpareCalc", spcalc);


                var scal = float.Parse(spcalc);
                var res1 = Math.Round(scal * BANLabcar);
                var res2 = Math.Round(scal * COBLabcar);
                req.BANreqd = res1.ToString();
                req.COBreqd = res2.ToString();

                cmd.Parameters.AddWithValue("@BANreqd", res1.ToString());
                cmd.Parameters.AddWithValue("@COBreqd", res2.ToString());
                var diff1 = res1 - req.BANQty;
                req.BANdiff = diff1.ToString();
                var diff2 = res2 - req.COBQty;
                req.COBdiff = diff2.ToString();
                cmd.Parameters.AddWithValue("@BANdiff", req.BANdiff);
                cmd.Parameters.AddWithValue("@COBdiff", req.COBdiff);
                cmd.Parameters.AddWithValue("@BANUnderRepair", string.Empty);
                cmd.Parameters.AddWithValue("@COBUnderRepair", string.Empty);
                cmd.Parameters.AddWithValue("@Status", string.Empty);
                var psud = (float.Parse(req.UnitPrice) * conversion);
                req.PriceUSD = psud.ToString("0.00");
                cmd.Parameters.AddWithValue("@PriceUSD", req.PriceUSD);
                var tpban = psud * diff1;
                var tpcob = psud * diff2;
                cmd.Parameters.AddWithValue("@BANTotalPrice", tpban.ToString("0.00"));
                cmd.Parameters.AddWithValue("@COBTotalPrice", tpcob.ToString("0.00"));

                try
                {
                    OpenConnection();
                    cmd.ExecuteNonQuery();
                    Console.WriteLine("Records Inserted Successfully");
                }
                catch (Exception ex)
                {
                    // MessageBox.Show(" Not Updated");
                }
                finally
                {
                    CloseConnection();
                }
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }

            

            catch (Exception e)
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
           

               

        }
            
            
           

        //function to get the spare inventory details
        [HttpPost]
        public ActionResult getPOdetails_sp(SpareInvDetails req)
        {
            List<SpareInvDetails> result = new List<SpareInvDetails>();
            DataTable dt = new DataTable();
            try
            {

                connection();

                string Query = " Select  [ID]" +
                  ",[Currency]" +
                  ",[Plant]" +
                  ",[UnitPrice]" +
                  ",[PO Quantity]" +

            "from [PODetails_Table] where ID = '" + req.ID + "'";

                OpenConnection();
                SqlCommand cmd = new SqlCommand(Query, conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                CloseConnection();
            }
            catch (Exception e)
            {

            }
            foreach (DataRow row in dt.Rows)
            {
                SpareInvDetails item = new SpareInvDetails();
                item.ID = int.Parse(row["ID"].ToString());
                item.Plant = row["Plant"].ToString();
                item.POQuantity = int.Parse(row["PO Quantity"].ToString());
                item.Currency = int.Parse(row["Currency"].ToString());
                item.UnitPrice = row["UnitPrice"].ToString();

                result.Add(item);
            }


            return Json(new { data = result }, JsonRequestBehavior.AllowGet);
        }

        //function to get inventory details
        [HttpPost]
        public ActionResult getPOdetails_inv(HWInvDetails req)
        {
            List<HWInvDetails> result = new List<HWInvDetails>();
            DataTable dt = new DataTable();
            try
            {

                connection();

                string Query = " Select  [ID]" +
                  ",[BU]" +
                  ",[OEM]" +
                  ",[GROUP]" +
                  ",[Item Name]" +
                  ",[ItemDescription]" +
                  ",[PO Quantity]" +
                  ",[UOM]" +
                  ",[Actual Delivery Date]" +

            "from [PODetails_Table] where ID = '" + req.ID + "'";

                OpenConnection();
                SqlCommand cmd = new SqlCommand(Query, conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                CloseConnection();
            }
            catch (Exception e)
            {

            }
            foreach (DataRow row in dt.Rows)
            {
                HWInvDetails item = new HWInvDetails();
                item.ID = int.Parse(row["ID"].ToString());
                item.BU = int.Parse(row["BU"].ToString());
                item.OEM = int.Parse(row["OEM"].ToString());
                item.GROUP = int.Parse(row["GROUP"].ToString());
                item.ItemName = int.Parse(row["Item Name"].ToString());
                //if (row["ItemDescription"].ToString() != "")
                //    item.ItemDescription = Convert.ToInt32(row["ItemDescription"]);
                //item.ItemDescription = int.Parse(row["ItemDescription"].ToString());
                item.ItemDescription = row["ItemDescription"].ToString();
                item.POQuantity = int.Parse(row["PO Quantity"].ToString());
                item.UOM = row["UOM"].ToString();
                item.ActualDeliveryDate = row["Actual Delivery Date"].ToString();

                result.Add(item);
            }


            return Json(new { data = result }, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult GetMode()
        //{
        //    List<ModeHWInventory_class> modeList = new List<ModeHWInventory_class>();
        //    DataTable dt = new DataTable();
        //    try
        //    {

        //        connection();

        //        string Query = " Select  [ID]" +
        //  ",[Mode] " +
        //  "from [ModeHWInventory] order by ID";

        //        OpenConnection();
        //        SqlCommand cmd = new SqlCommand(Query, conn);
        //        SqlDataAdapter da = new SqlDataAdapter(cmd);
        //        da.Fill(dt);
        //        CloseConnection();
        //    }
        //    catch (Exception e)
        //    {

        //    }
        //    foreach (DataRow row in dt.Rows)
        //    {
        //        ModeHWInventory_class item = new ModeHWInventory_class();
        //        item.ID = int.Parse(row["ID"].ToString());
        //        item.Mode = row["Mode"].ToString();

        //        modeList.Add(item);
        //    }


        //    return Json(new { data = modeList }, JsonRequestBehavior.AllowGet);
        //}


        [HttpPost]
        public ActionResult Save_POdata(PO_Data_update req)
        {
            DataTable dt = new DataTable();

            connection();
            dt = new DataTable();
            string Query = "";
            System.Security.Principal.IPrincipal User = System.Web.HttpContext.Current.User;

            //SPOTONData_Table_2021 PresentUser = BudgetingController.lstUsers.FirstOrDefault(user => user.NTID.Trim().ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper()));
            //           CREATE TABLE[dbo].[HwDamageCost_Table](
            //  [ID][int] IDENTITY(1, 1) NOT NULL,
            //  [Location] [varchar](255) NULL,
            //[Month] [nvarchar](10) NULL,
            //[Item_Name] [varchar](255) NULL,
            //[Qty] [varchar](255) NULL,
            //[Project_Team] [varchar](100) NULL,
            //[Cost_inEUR] [float] NULL,
            //[Total_Price] [float] NULL,
            //[RequestorNT] [nvarchar](50) NULL



            if (req.ID != 0)
            {
                //Query = "UPDATE [TravelCost_Table] SET [Cmmt_Item] = '" + req.Cmmt_Item + "', [Year] = '" + req.Year + "',[Budget_Plan] = '" + req.Budget_Plan + "',[Invoice] = '" + req.Invoice + "',[Open] = '" + req.Open + "',  WHERE ID = '" + req.ID +  "'";
                Query = "UPDATE [PODetails_Table] SET " +

                        "[VKMYear]= @VKMYear," +
                        "[BU]= @BU," +
                        "[OEM]= @OEM," +
                        "[GROUP]= @GROUP," +
                        "[Dept]= @Dept," +
                        "[Item Name]= @ItemName," +
                        "[PO Number]= @PONumber," +
                        "[PIF ID]= @PIFID," +
                        "[Fund]= @Fund," +
                        "[Fund Center]= @FundCenter," +
                        "[BudgetCode]= @BudgetCode," +
                        "[ItemDescription]= @ItemDescription," +
                        "[PO Quantity]= @POQuantity," +
                        "[UOM]= @UOM," +
                        "[UnitPrice]= @UnitPrice," +
                        "[Netvalue]= @Netvalue," +
                        "[Netvalue_USD]= @Netvalue_USD," +
                        "[Currency]= @Currency," +
                        "[Plant]= @Plant," +
                        "[PO Created On]= @POCreatedOn," +
                        "[VendorName]= @VendorName," +
                        "[CW]= @CW," +
                        "[Tentative Delivery Date]= @TentativeDeliveryDate," +
                        "[Actual Delivery Date]= @ActualDeliveryDate," +
                        "[Difference in DeliveRy Date]= @DifferenceinDeliveryDate," +
                        "[Actal Amt]= @ActalAmt," +
                        "[Negotiated Amt]= @NegotiatedAmt," +
                        "[Savings]= @Savings," +
                        "[Current status]= @Currentstatus," +
                        "[PO Remarks]= @PORemarks " +
                        "WHERE ID = @ID";

            }
            //else
            //{
            //    //insert

            //    Query =
            //    "INSERT into[PODetails_Table]" +

            //     "(" +

            //      "[RequestID]" +
            //      ",[VKMYear]" +
            //      ",[BU]" +
            //      ",[OEM]" +
            //      ",[GROUP]" +
            //      ",[Dept]" +
            //      ",[Item Name]" +

            //      ",[PO Number]" +
            //      ",[PIF ID]" +
            //      ",[Fund]" +
            //      ",[Fund Center]" +
            //      ", BudgetCode" +
            //      ", ItemDescription" +
            //      ",[PO Quantity]" +
            //      ", UOM" +
            //      ", UnitPrice" +
            //      ", Netvalue," +
            //      "Netvalue_USD" +
            //      ", Currency" +
            //      ",[Plant]" +
            //      ", VendorName" +
            //      ",[PO Created On]" +
            //      ",[Current status]" +
            //      ",[PO Remarks]" +
            //      ",[Tentative Delivery Date]" +
            //      ",[Actual Delivery Date]" +
            //      "[DifferenceinDeliveRyDate]" +
            //       "[ActalAmt]" +
            //       "[NegotiatedAmt]" +
            //       "[Savings]" +
            //      ")" +
            //      "values" +
            //      "(" +
            //      " @RequestID" +
            //      ", @VKM_Year" +
            //      ", @BU  " +
            //      ", @OEM  " +
            //      ", @Group" +
            //      ", @Dept " +
            //      ", @Item " +
            //      ", @OrderedQuantity" +
            //      ", @PONumber" +
            //      ", @PIFID" +
            //      ", Convert(nvarchar, @Fund)" +
            //      ", @FundCenter" +
            //      ", @BudgetCode" +
            //      ", @ItemDescription" +
            //      ", @POQuantity" +
            //      ", @UOM" +
            //      ", @UnitPrice" +
            //      ", convert(float, replace(REPLACE(@Netvalue, '.', ''), ',', '.'))," +
            //      "  @Netvalue_USD" +
            //      ", @Currency" +
            //      ", @Plant      " +
            //      ", @VendorName " +
            //      ", convert(date, convert(date, @POCreatedOn, 103))" +
            //      ", @Currentstatus" +
            //      ", @PORemarks," +
            //      "@TentativeDeliveryDate," +
            //      "@ActualDeliveryDate" +
            //      "@DifferenceinDeliveryDate" +
            //      "@Savings" +
            //      "@NegotiatedAmt" +
            //      "@ActalAmt" +
            //      ")";
            //}
            SqlCommand cmd = new SqlCommand(Query, conn);

            //if (req.ID != 0)

            cmd.Parameters.AddWithValue("@VKMYear", req.VKMYear);
            cmd.Parameters.AddWithValue("@BU", req.BU);
            cmd.Parameters.AddWithValue("@OEM", req.OEM);
            cmd.Parameters.AddWithValue("@GROUP", req.GROUP);
            cmd.Parameters.AddWithValue("@Dept", req.Dept);
            cmd.Parameters.AddWithValue("@ItemName", req.ItemName);
            cmd.Parameters.AddWithValue("@OrderedQuantity", req.OrderedQuantity);
            cmd.Parameters.AddWithValue("@PONumber", req.PONumber);

            cmd.Parameters.AddWithValue("@Fund", req.Fund);
            //cmd.Parameters.AddWithValue("@FundCenter",req.FundCenter);
            cmd.Parameters.AddWithValue("@BudgetCode", req.BudgetCode);
            cmd.Parameters.AddWithValue("@ItemDescription", req.ItemDescription);
            cmd.Parameters.AddWithValue("@POQuantity", req.POQuantity);
            cmd.Parameters.AddWithValue("@UOM", req.UOM);
            cmd.Parameters.AddWithValue("@UnitPrice", req.UnitPrice);
            cmd.Parameters.AddWithValue("@Netvalue", req.Netvalue);
            cmd.Parameters.AddWithValue("@Netvalue_USD", req.Netvalue_USD);
            cmd.Parameters.AddWithValue("@Currency", req.Currency);

            cmd.Parameters.AddWithValue("@POCreatedOn", req.POCreatedOn);
            cmd.Parameters.AddWithValue("@VendorName", req.VendorName);
            if (req.Plant != null)
                cmd.Parameters.AddWithValue("@Plant", req.Plant);
            else
                cmd.Parameters.AddWithValue("@Plant", string.Empty);
            if (req.FundCenter != null)
                cmd.Parameters.AddWithValue("@FundCenter", req.FundCenter);
            else
                cmd.Parameters.AddWithValue("@FundCenter", string.Empty);
            if (req.PIFID != null)
                cmd.Parameters.AddWithValue("@PIFID", req.PIFID);
            else
                cmd.Parameters.AddWithValue("@PIFID", string.Empty);
            if (req.CW != null)
                cmd.Parameters.AddWithValue("@CW", req.CW);
            else
                cmd.Parameters.AddWithValue("@CW", string.Empty);
            if (req.TentativeDeliveryDate != null)
                cmd.Parameters.AddWithValue("@TentativeDeliveryDate", req.TentativeDeliveryDate);
            else
                cmd.Parameters.AddWithValue("@TentativeDeliveryDate", DBNull.Value);
            if (req.ActualDeliveryDate != null)
                cmd.Parameters.AddWithValue("@ActualDeliveryDate", req.ActualDeliveryDate);
            else
                cmd.Parameters.AddWithValue("@ActualDeliveryDate", DBNull.Value);
            if (req.DifferenceinDeliveRyDate != null)
                cmd.Parameters.AddWithValue("@DifferenceinDeliveRyDate", req.DifferenceinDeliveRyDate);
            else
                cmd.Parameters.AddWithValue("@DifferenceinDeliveRyDate", "");

            if (req.ActalAmt != null)
                cmd.Parameters.AddWithValue("@ActalAmt", req.ActalAmt);
            else
                cmd.Parameters.AddWithValue("@ActalAmt", string.Empty);
            if (req.NegotiatedAmt != null)
                cmd.Parameters.AddWithValue("@NegotiatedAmt", req.NegotiatedAmt);
            else
                cmd.Parameters.AddWithValue("@NegotiatedAmt", string.Empty);

            cmd.Parameters.AddWithValue("@Savings", req.Savings);
            cmd.Parameters.AddWithValue("@Currentstatus", req.Currentstatus);
            if (req.PORemarks != null)
                cmd.Parameters.AddWithValue("@PORemarks", req.PORemarks);
            else
                cmd.Parameters.AddWithValue("@PORemarks", string.Empty);
            cmd.Parameters.AddWithValue("@ID", req.ID);

            try
            {
                OpenConnection();
                cmd.ExecuteNonQuery();
                Console.WriteLine("Records Inserted Successfully");
            }
            catch (Exception ex)
            {
                // MessageBox.Show(" Not Updated");
            }
            finally
            {
                CloseConnection();
            }

            return Json(new { data = GetPODetails(), success = true }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult UpdateDetailsinVKM(/*DateTime Tentative*/ string RequestID)
        {
            DataTable dt = new DataTable();
            string Query = "";
            try
            {
                if (RequestID != "")
                {
                    connection();
                    OpenConnection();
                    Query = " Exec [dbo].[UpdateVKM_fromPO] '" + RequestID + "'";
                    SqlCommand cmd = new SqlCommand(Query, conn);
                    cmd.ExecuteNonQuery();
                    CloseConnection();
                }

            }
            catch (Exception ex)
            {

            }
            return Json(new { data = 0, JsonRequestBehavior.AllowGet });
        }

        [HttpPost]
        public ActionResult GetDateDifference(string Actual, string Tentative)
        {

            string Query = "";
            //System.DateTime actualDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            //actualDateTime = actualDateTime.AddMilliseconds(Actual).ToLocalTime();
            var actualdate = DateTime.Parse(new string(Actual.Take(24).ToArray()));
            var actual = DateTime.Parse(actualdate.ToString("MM/dd/yyyy"));
            var tentdate = DateTime.Parse(new string(Tentative.Take(24).ToArray()));
            var tent = DateTime.Parse(tentdate.ToString("MM/dd/yyyy"));
            //Convert.ToDateTime(Actual.ToString());
            //DateTime tentative = DateTime.Parse(Tentative.ToString());
            //int Difference = 0;
            DataTable dt = new DataTable();
            try
            {

                var Difference = (actual - tent).TotalDays;


                return Json(new { data = Difference, JsonRequestBehavior.AllowGet });
            }
            catch (Exception ex)
            {

            }
            return Json(new { data = "", JsonRequestBehavior.AllowGet });
        }

        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonResult()
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior,
                MaxJsonLength = Int32.MaxValue
            };
        }

        [HttpGet]
        public ActionResult Lookup()
        {

            using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

            //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

            {
                LookupData lookupData = new LookupData();


                lookupData.BU_List = db.BU_Table.ToList();

                lookupData.OEM_List = db.OEM_Table.ToList();
                lookupData.DEPT_List = db.DEPT_Table.ToList();
                //if (year.Contains("2020"))
                //    lookupData.Groups_oldList = BudgetingController.lstGroups_old;
                //else
                lookupData.Groups_test = db.Groups_Table_Test.ToList();
                lookupData.Item_List = db.ItemsCostList_Table.ToList()/*.FindAll(item => item.Deleted != true)*/;
                lookupData.OrderStatus_List = db.Order_Status_Table.ToList();
                lookupData.Currency_List = db.Currency_Table.ToList();
                lookupData.Fund_List = db.Fund_Table.ToList();


                return Json(new { data = lookupData }, JsonRequestBehavior.AllowGet);
            }
        }



    }

    public partial class PO_Data
    {
        public int ID { get; set; }
        public int RequestID { get; set; }
        public string VKMYear { get; set; }
        public string BU { get; set; }
        public string OEM { get; set; }
        public string GROUP { get; set; }
        public string Dept { get; set; }
        public string ItemName { get; set; }
        public string OrderedQuantity { get; set; }
        public string PONumber { get; set; }
        public string PIFID { get; set; }
        public string Fund { get; set; }
        public string FundCenter { get; set; }
        public string BudgetCode { get; set; }
        public string ItemDescription { get; set; }
        public int POQuantity { get; set; }
        public string UOM { get; set; }
        public string UnitPrice { get; set; }
        public string Netvalue { get; set; }
        public string Netvalue_USD { get; set; }
        public string Currency { get; set; }
        public string Plant { get; set; }
        public string POCreatedOn { get; set; }
        public string VendorName { get; set; }
        public string CW { get; set; }
        public string TentativeDeliveryDate { get; set; }
        public string ActualDeliveryDate { get; set; }
        public string DifferenceinDeliveRyDate { get; set; }
        public string ActalAmt { get; set; }
        public string NegotiatedAmt { get; set; }
        public string Savings { get; set; }
        public string Currentstatus { get; set; }
        public string PORemarks { get; set; }

    }

    public partial class HWInvDetails
    {
        public int ID { get; set; }
        public int BU { get; set; }
        public int OEM { get; set; }
        public int GROUP { get; set; }
        public int ItemName { get; set; }
        public string ItemDescription { get; set; }
        public int POQuantity { get; set; }
        public string UOM { get; set; }
        public string ActualDeliveryDate { get; set; }
    }

    public partial class SpareInvDetails
    {
        public int ID { get; set; }
        public string Plant { get; set; }
        public int Currency { get; set; }
        public int POQuantity { get; set; }
        public string UnitPrice { get; set; }

    }

    public partial class SpareInventoryPO
    {
        public int ID { get; set; }
        public string Plant { get; set; }
        public int Currency { get; set; }
        public int POQuantity { get; set; }
        public string UnitPrice { get; set; }
        public int SpareHW { get; set; }
        public string SpareCalc { get; set; }
        public int BANQty { get; set; }
        public int COBQty { get; set; }
        public int TotalQty { get; set; }
        public int SpareQuantity { get; set; }
        public string BANreqd { get; set; }
        public string COBreqd { get; set; }
        public string BANdiff { get; set; }
        public string COBdiff { get; set; }
        public string BANUnderRepair { get; set; }
        public string COBUnderRepair { get; set; }
        public string Status { get; set; }
        public string PriceUSD { get; set; }
        public string TotalPriceBAN { get; set; }
        public string TotalPriceCOB { get; set; }

    }

    public partial class PO_Data_update
    {
        public int ID { get; set; }
        public int RequestID { get; set; }
        public int VKMYear { get; set; }
        public string BU { get; set; }
        public string OEM { get; set; }
        public string GROUP { get; set; }
        public string Dept { get; set; }
        public string ItemName { get; set; }
        public int OrderedQuantity { get; set; }
        public string PONumber { get; set; }
        public string PIFID { get; set; }
        public string Fund { get; set; }
        public string FundCenter { get; set; }
        public string BudgetCode { get; set; }
        public string ItemDescription { get; set; }
        public int POQuantity { get; set; }
        public string UOM { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Netvalue { get; set; }
        public decimal Netvalue_USD { get; set; }
        public string Currency { get; set; }
        public string Plant { get; set; }
        public string POCreatedOn { get; set; }
        public string VendorName { get; set; }
        public string CW { get; set; }
        public Nullable<DateTime> TentativeDeliveryDate { get; set; }
        public Nullable<DateTime> ActualDeliveryDate { get; set; }
        public string DifferenceinDeliveRyDate { get; set; }
        public string ActalAmt { get; set; }
        public string NegotiatedAmt { get; set; }
        public decimal Savings { get; set; }
        public string Currentstatus { get; set; }
        public string PORemarks { get; set; }

    }
    public partial class HardwareInventoryPO
    {
        public int ID { get; set; }
        public int InventoryType { get; set; }
        //public int InventoryType2 { get; set; }
        public string SerialNo { get; set; }
        public string BondNo { get; set; }
        public string BondDate { get; set; }
        public string AssetNo { get; set; }
        public string HardwareResponsible { get; set; }
        public string HandoverTo { get; set; }
        public int Mode { get; set; }
        public string Remarks { get; set; }
        public string ALMNo { get; set; }
        public int BU { get; set; }
        public int OEM { get; set; }
        public int GROUP { get; set; }
        public int ItemName { get; set; }
        public string ItemDescription { get; set; }
        public int POQuantity { get; set; }
        public int Quantity { get; set; }
        public string UOM { get; set; }
        public string ActualDeliveryDate { get; set; }
        public int PODetails_Id { get; set; }
    }
}