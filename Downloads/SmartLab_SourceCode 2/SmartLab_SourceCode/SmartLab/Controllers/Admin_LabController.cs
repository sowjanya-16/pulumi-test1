using LC_Reports_V1.Models;
using Newtonsoft.Json;
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
    public class Admin_LabController : Controller
    {
        public static string LabOEMCsvPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/LAB_OEM_Mapping.csv");
        public static string LabOEMJsonPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/LAB_OEM_Mapping.json");
        // GET: Admin_Lab
        public ActionResult LabAdmin()
        {
            using (BookingServerReplicaEntities db = new BookingServerReplicaEntities())
            {
                try
                {
                    //List<laboemjson> laboemmapping = new List<laboemjson>();
                    //convertcsvtojson(LabOEMCsvPath, LabOEMJsonPath);

                    //string jsonLabOEM = System.IO.File.ReadAllText(LabOEMJsonPath);
                    //jsonLabOEM = jsonLabOEM.Replace("\\", string.Empty);
                    //jsonLabOEM = jsonLabOEM.Trim('"');
                    //laboemmapping = JsonConvert.DeserializeObject<List<laboemjson>>(jsonLabOEM);

                    //foreach (laboemjson lab in laboemmapping)
                    //{
                    //    LabInfo tempLab = new LabInfo();
                    //    tempLab = db.LabInfoes.Where(x => x.DisplayName.Equals(lab.LABID)).FirstOrDefault();
                    //    if(tempLab!=null)
                    //    {
                    //        tempLab.OEM = db.LabOEMs.Where(x => x.OEM == lab.OEM).FirstOrDefault().ID.ToString();

                    //        db.Entry(tempLab).State = EntityState.Modified;

                    //        db.SaveChanges();
                    //    }
                        
                    //}



                }
                catch (Exception ex)
                { }
            }
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// function to get data lab view table
        /// </summary>
        /// <returns></returns>
        /// 
        private SqlConnection conn;

        private void connection()
        {

            string connstring = ConfigurationManager.ConnectionStrings["dbConnection"].ToString();
            conn = new SqlConnection(connstring);

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
        public ActionResult GetLabData()
        {
            using (BookingServerReplicaEntities db = new BookingServerReplicaEntities())
            {

                List<OEMSPOC> spocList = new List<OEMSPOC>();
                DataTable dt = new DataTable();
                try
                {
                   
                    
                    List<LabInfoView> LabList = new List<LabInfoView>();
                    foreach (LabInfo lab in  db.LabInfoes)
                    {
                        LabInfoView labview = new LabInfoView();
                        labview.Description = lab.Description;
                        labview.DisplayName = lab.DisplayName;
                        labview.Id = lab.Id;
                        labview.LocationId = lab.LocationId;
                        int oem;
                        labview.OEM = Int32.TryParse(lab.OEM,out oem)?oem:0;
                        labview.TypeId = lab.TypeId;
                        LabList.Add(labview);
                    }

                    connection();
                    string Query = "Select OEM,SPOC from OEMSPOC_Table";
                    OpenConnection();
                    SqlCommand cmd = new SqlCommand(Query, conn);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    CloseConnection();

                    foreach (DataRow row in dt.Rows)
                    {
                        OEMSPOC item = new OEMSPOC();
                        item.OEM = row["OEM"].ToString();
                        item.Spoc = row["SPOC"].ToString();

                        spocList.Add(item);
                    }
                    return Json(new
                    {
                        data = LabList,spocList
                    }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception ex)
                {
                    
                    return null;
                }
            }
        }

        /// <summary>
        /// Function to help the ajax request to get the list of OEMs
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult LookupOEM()
        {

            using (BookingServerReplicaEntities db = new BookingServerReplicaEntities())
            {
                try
                {
                    return Json(new
                    {
                        data = db.LabOEMs.ToList()
                    }, JsonRequestBehavior.AllowGet); ;
                }
                catch (Exception ex)
                {
                    return null;
                }
            }
            

        }

        /// <summary>
        /// function to support the csv to json conversion (legacy)
        /// </summary>
        /// <param name="csvpath"></param>
        /// <param name="jsonpath"></param>
        private void convertcsvtojson(string csvpath, string jsonpath)
        {
            try
            {
                var csv = new List<laboemjson>(); // or, List<YourClass>
                var lines = System.IO.File.ReadAllLines(csvpath);

                foreach (string line in lines)
                    csv.Add(new laboemjson { LABID = line.Split(',')[0], OEM = line.Split(',')[1], RESP = line.Split(',')[2] }); // or, populate YourClass          
                string json = new
                    System.Web.Script.Serialization.JavaScriptSerializer().Serialize(csv); // revisit use of lib

                System.IO.File.WriteAllText(jsonpath, json);
            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// function to support the ajax request to update the OEM
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdateOEM(LabInfo req)
        {
           
                using (BookingServerReplicaEntities db = new BookingServerReplicaEntities())
                {
                    LabInfo item = new LabInfo();
                    item.Description = req.Description;
                    item.DisplayName = req.DisplayName;
                    item.Id = req.Id;
                    item.LocationId = req.LocationId;
                    item.OEM = req.OEM;
                    item.TypeId = req.TypeId;

                    db.Entry(item).State = EntityState.Modified;

                    db.SaveChanges();

                    return Json(new { success = true, message = "Updated Successfully" }, JsonRequestBehavior.AllowGet);
                }
           
        }

        public ActionResult AddSpoc(OEMSPOC req)
        {
            string Query = "";
            System.Security.Principal.IPrincipal User = System.Web.HttpContext.Current.User;
            connection();
            

                
                Query = "INSERT into[OEMSPOC_Table]" +
                          "(" +
                           "[OEM]" +
                           ",[SPOC]" +
                           ")" +
                           "values" +
                           "(" +
                           " @OEM  " +
                           ",@Spoc" +
                           ")";
            

            SqlCommand cmd = new SqlCommand(Query, conn);
            cmd.Parameters.AddWithValue("@Spoc", req.Spoc);
            cmd.Parameters.AddWithValue("@OEM", req.OEM);
            cmd.Parameters.AddWithValue("@ID", req.ID);

            try
            {
                OpenConnection();
                cmd.ExecuteNonQuery();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // alert()
                // MessageBox.Show(" Not Updated");
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
            finally
            {
                CloseConnection();
            }
        }
        public ActionResult EditSpoc(OEMSPOC req)
        {
            string Query = "";
            System.Security.Principal.IPrincipal User = System.Web.HttpContext.Current.User;
            connection();


            Query = "UPDATE [OEMSPOC_Table] SET " +

                    "[OEM]= @OEM," +
                    "[SPOC]= @Spoc " +
                    "WHERE OEM = @OEM";

            SqlCommand cmd = new SqlCommand(Query, conn);
            cmd.Parameters.AddWithValue("@Spoc", req.Spoc);
            cmd.Parameters.AddWithValue("@OEM", req.OEM);
            cmd.Parameters.AddWithValue("@ID", req.ID);

            try
            {
                OpenConnection();
                cmd.ExecuteNonQuery();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // alert()
                // MessageBox.Show(" Not Updated");
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
            finally
            {
                CloseConnection();
            }
        }

        public ActionResult DeleteSpoc(string id)
        {
            DataTable dt = new DataTable();

            connection();

            dt = new DataTable();
            string Query = "";
            System.Security.Principal.IPrincipal User = System.Web.HttpContext.Current.User;

            Query = "Delete from [OEMSPOC_Table] where OEM = @OEM";


            SqlCommand cmd = new SqlCommand(Query, conn);
            cmd.Parameters.AddWithValue("@OEM", id);
            try
            {
                OpenConnection();
                cmd.ExecuteNonQuery();
                return Json(new { success = true }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
            finally
            {
                CloseConnection();

            }


        }
    }

    

    public class OEMSPOC
    {
        public int ID { get; set; }
        public string OEM { get; set; }
        public string Spoc { get; set; }

    }
    public class LabInfoView
    {
        public int Id { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public Nullable<int> TypeId { get; set; }
        public Nullable<int> LocationId { get; set; }
        public int OEM { get; set; }

    }
}