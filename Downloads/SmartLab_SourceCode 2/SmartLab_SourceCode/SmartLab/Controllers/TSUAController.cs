using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LC_Reports_V1.Models;
using System.Data.Entity;
using TestSystemUserArbitration1;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;


namespace TestSystemUserArbitration1.Controllers
{
    public class TSUAController : Controller
    {

        private SqlConnection conn;

        private SqlConnection conn1;

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

        private void connection1()
        {

            string connstring = ConfigurationManager.ConnectionStrings["dbConnection"].ToString();
            conn1 = new SqlConnection(connstring);

        }

        private void OpenConnection1()
        {
            if (conn1.State == ConnectionState.Closed)
            {
                conn1.Open();
            }
        }

        private void CloseConnection1()
        {
            if (conn1.State == ConnectionState.Open)
            {
                conn1.Close();
            }
        }
        // GET: TSUA
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ProjectAdmins()
        {
            return View();
        }


        public List<ProjectUsersdbTSUA> PUsersDbModel()
        {
            List<ProjectUsersdbTSUA> ViewList1 = new List<ProjectUsersdbTSUA>();
            try
            {
                // returns the current windows login user
                string CurrUser = User.Identity.Name.Split('\\')[1].Trim();
               
                // initialise Data table
                DataTable dt1 = new DataTable();
                string OEM = String.Empty;
                // initiate connection string 
                connection1();
                // table to return the oem of Admin from the Project_Admins_TSUA DB table
                string DQuery = "select ADMIN_OEM from BookingServerReplica.dbo.Project_Admins_TSUA where ADMIN_NTID='" + CurrUser.ToString() + "'";
                OpenConnection1();
                SqlCommand comd = new SqlCommand(DQuery, conn1);
                SqlDataReader sdr = comd.ExecuteReader();

                if (sdr.HasRows)
                {
                    sdr.Read();
                    //store the data from table into a local variable
                    OEM = sdr["ADMIN_OEM"].ToString();


                }

                CloseConnection1();


                string Query = "";
                DataSet ds = new DataSet();
                SqlConnection conn = new SqlConnection();
                connection();
                OpenConnection();
                Query = "SELECT * FROM BookingServerReplica.dbo.ProjectUsersdbTSUA WHERE USER_OEM in (SELECT ADMIN_OEM FROM BookingServerReplica.dbo.Project_Admins_TSUA WHERE ADMIN_NTID = '" + CurrUser + "')";
                SqlCommand cmd = new SqlCommand(Query, conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                CloseConnection();


                

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    ProjectUsersdbTSUA ritem = new ProjectUsersdbTSUA();
                    ritem.Id = Convert.ToInt32(row["Id"]);
                    ritem.USER_NTID = row["USER_NTID"].ToString();
                    ritem.USER_NAME = row["USER_NAME"].ToString();
                    ritem.USER_OEM = OEM;
                    ritem.PROJECT = row["PROJECT"].ToString();
                    ritem.PROJECT_RESPONSIBLE = row["PROJECT_RESPONSIBLE"].ToString();
                    ritem.LOCATION = row["LOCATION"].ToString();




                    ViewList1.Add(ritem);
                    //return "Working";
                }
                //return "dsis empty";
            }
            catch(Exception ex)
            {
                // return ex.Message;
                CloseConnection();
                CloseConnection1();
            }
            
            
            
           // return "At last";



            //List<PROJECTUSERSTSUADB1> ViewList = new List<PROJECTUSERSTSUADB1>();
            //using (tsuapracticedatabaseEntities3 db = new tsuapracticedatabaseEntities3())
            //{
            //    List<PROJECTUSERSTSUADB1> ProjectUsersList = new List<PROJECTUSERSTSUADB1>();
            //    ProjectUsersList = db.PROJECTUSERSTSUADB1.ToList();


            //    foreach (PROJECTUSERSTSUADB1 item in ProjectUsersList)
            //    {
            //        PROJECTUSERSTSUADB1 ritem = new PROJECTUSERSTSUADB1();
            //        ritem.Id = item.Id;
            //        ritem.USER_NTID = item.USER_NTID.ToUpper();
            //        ritem.USER_OEM = item.USER_OEM;
            //        ritem.USER_NAME = item.USER_NAME;
            //        ritem.PROJECT = item.PROJECT;
            //        ritem.PROJECT_RESPONSIBLE = item.PROJECT_RESPONSIBLE;
            //        ritem.LOCATION = item.LOCATION;

            //        ViewList.Add(ritem);
            //    }

            //}

            return ViewList1;

        }

        public ActionResult ProjectUsersDataGrid()
        {
            //list<projectuserstsua> userslist = new list<projectuserstsua>();
            //userslist = pusersdbmodel();
            //return view(userslist);

            List<ProjectUsersdbTSUA> viewlist = new List<ProjectUsersdbTSUA>();
            // function call to store the data coming from the function as an output into a list obj
            viewlist = PUsersDbModel();



            //return View(viewlist);

            return Json(new { data = viewlist }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult GetOemforTitle()
        {
            try
            {
                string CurrUser = User.Identity.Name.Split('\\')[1].Trim();
                string OEM = string.Empty;
                DataTable dt1 = new DataTable();
                connection();
                string DQuery1 = "select ADMIN_OEM from BookingServerReplica.dbo.Project_Admins_TSUA where ADMIN_NTID='" + CurrUser.ToString() + "'";
                OpenConnection();
                SqlCommand comd1 = new SqlCommand(DQuery1, conn);
                SqlDataReader sdr1 = comd1.ExecuteReader();

                if (sdr1.HasRows)
                {
                    sdr1.Read();
                    OEM = sdr1["ADMIN_OEM"].ToString();


                }
                CloseConnection();
                return Json(new { data = OEM }, JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                CloseConnection();
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }
            
        }


        [HttpPost]
        public ActionResult AddorEditData(ProjectUsersdbTSUA req)
        {
            try
            {

                // intiliase list object
                List<ProjectUsersdbTSUA> ViewList = new List<ProjectUsersdbTSUA>();
                // intiliase db object
                using (BookingServerReplicaEntities db = new BookingServerReplicaEntities())
                {
                    //tracking the current window login user 
                    string CurrUser = User.Identity.Name.Split('\\')[1].Trim(); 
                    // store the db table data into an object of type list
                    var tsua_Table = db.ProjectUsersdbTSUAs.AsNoTracking().ToList();
                    // if (req.USER_NTID == "")
                    // { to find if any duplicates are present
                    if ((tsua_Table.Find(x => x.Id == req.Id) != null) &&
                        (tsua_Table.Find(x => x.USER_NTID.ToUpper().Trim() == req.USER_NTID.ToUpper().Trim()) != null) &&
                        (tsua_Table.Find(x => x.USER_OEM.ToUpper().Trim() == req.USER_OEM.ToUpper().Trim()) != null) &&
                        (tsua_Table.Find(x => x.USER_NAME.ToUpper().Trim() == req.USER_NAME.ToUpper().Trim()) != null) &&
                        (tsua_Table.Find(x => x.PROJECT.ToUpper().Trim() == req.PROJECT.ToUpper().Trim()) != null) &&
                        (tsua_Table.Find(x => x.PROJECT_RESPONSIBLE.ToUpper().Trim() == req.PROJECT_RESPONSIBLE.ToUpper().Trim()) != null) &&
                        (tsua_Table.Find(x => x.LOCATION.ToUpper().Trim() == req.LOCATION.ToUpper().Trim()) != null))
                    {
                        return Json(new
                        {
                            success = false,
                            message = "the requesor details for " + req.USER_NTID + "are already present in the Tsua Users List, Please check again!"
                        }, JsonRequestBehavior.AllowGet);

                    }
                    // }
                    //SpotOnData spotOnfncall = new SpotOnData();
                    ProjectUsersdbTSUA item = new ProjectUsersdbTSUA();
                    DataTable dt1 = new DataTable();
                    item.USER_OEM = String.Empty;
                    connection();
                    // query to return the Admin OEM
                    string DQuery = "select ADMIN_OEM from BookingServerReplica.dbo.Project_Admins_TSUA where ADMIN_NTID='" + CurrUser.ToString() + "'";
                    OpenConnection();
                    SqlCommand comd = new SqlCommand(DQuery, conn);
                    SqlDataReader sdr = comd.ExecuteReader();

                    if (sdr.HasRows)
                    {
                        sdr.Read();
                        // assign OEM to the object which must contain oem data of each instance.
                        item.USER_OEM = sdr["ADMIN_OEM"].ToString();


                    }

                    CloseConnection();
                    // fill the object with the data coming from data table
                    item.Id = req.Id;
                    item.USER_NTID = req.USER_NTID.ToString().ToUpper();
                    item.PROJECT_RESPONSIBLE = req.PROJECT_RESPONSIBLE;
                    item.USER_NAME = req.USER_NAME;
                    item.PROJECT = req.PROJECT;
                    item.LOCATION = req.LOCATION;

                    //for new entry
                    if (req.Id == 0)
                    {
                        //add item into ProjectUsersdbTSUAs datatable 
                        db.ProjectUsersdbTSUAs.Add(item);
                        // save the DB
                        db.SaveChanges();
                        // return the db data onto the page after saving
                        ViewList = PUsersDbModel();
                        // return object with data to the view
                        return Json(new { data = ViewList, success = true, message = "Saved Successfully" }, JsonRequestBehavior.AllowGet);
                    }
                    // to update an entry
                    else
                    {
                        // modify item into ProjectUsersdbTSUAs datatable
                        db.Entry(item).State = EntityState.Modified;
                        // save the DB
                        db.SaveChanges();
                        // return the db data onto the page after saving
                        ViewList = PUsersDbModel();
                        // return object with data to the view
                        return Json(new { data = ViewList, success = true, message = "Updated Successfully" }, JsonRequestBehavior.AllowGet);
                    }
                }

            }
            catch (Exception ex)
            {
                CloseConnection();
                return Json(new { success = false }, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public ActionResult DeleteUserData(int Id)
        {
            List<ProjectUsersdbTSUA> ViewList = new List<ProjectUsersdbTSUA>();
            try
            {
               
                using (BookingServerReplicaEntities db = new BookingServerReplicaEntities())
                {

                    ProjectUsersdbTSUA item = db.ProjectUsersdbTSUAs.Where(x => x.Id == Id).FirstOrDefault<ProjectUsersdbTSUA>();
                    // delete item into ProjectUsersdbTSUAs datatable
                    db.ProjectUsersdbTSUAs.Remove(item);
                    // save the DB
                    db.SaveChanges();
                    // return the db data onto the page after saving
                    ViewList = PUsersDbModel();
                    // return object with data to the view
                    return Json(new { data = ViewList, success = true, message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
                    //return View(ViewList);
                }
            }
            catch (Exception ex)
            {
                CloseConnection();
                return Json(new { data = ViewList, success = true, message = "Deleted Successfully" }, JsonRequestBehavior.AllowGet);
            }
            

        }
        //Action result to fetch the Spot on data using NTID
        [HttpPost]
        public ActionResult GetSpotOnData(string NTID)
        {
            try
            {
                DataTable dt = new DataTable();
                SpotOnData spotOnData = new SpotOnData();
                // establish connection with Budgeting db
                connection();
                //query to fetch the data based on NTID given as an input in the text field of datagrid
                string Query = "select NTID, EmployeeName, BusinessArea from SPOTONData_Table_2022 where NTID='" + NTID.ToString() + "'";
                OpenConnection();
                SqlCommand cmd = new SqlCommand(Query, conn);
                SqlDataReader dr = cmd.ExecuteReader();
                // fill in the object with data table data
                if (dr.HasRows)
                {
                    dr.Read();
                    spotOnData.EmpNTID = dr["NTID"].ToString();
                    spotOnData.EmpName = dr["EmployeeName"].ToString();
                    spotOnData.EmpLocation = dr["BusinessArea"].ToString();

                }
                spotOnData.AdminName = User.Identity.Name.Split('\\')[1].Trim();
                CloseConnection();


                //DataTable dt1 = new DataTable();
                //spotOnData.OEM = string.Empty; 
                //string conn = ConfigurationManager.ConnectionStrings["dbConnection"].ToString();
                //SqlConnection sqlcon = new SqlConnection(conn);
                //string DQuery = "select ADMIN_OEM from Project_Admins_TSUA where ADMIN_NTID='" + spotOnData.AdminName.ToString() + "'";
                //sqlcon.Open();
                //SqlCommand comd = new SqlCommand(DQuery, sqlcon);
                //SqlDataReader sdr = comd.ExecuteReader();

                //if (sdr.HasRows)
                //{
                //    sdr.Read();
                //    spotOnData.OEM = sdr["ADMIN_OEM"].ToString();


                //}

                //sqlcon.Close();
                // return data to the view
                return Json(new { success = true, spotOnData }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                CloseConnection();
                return Json(new { success = false, message = "The NTID is not valid, Please check again!" }, JsonRequestBehavior.AllowGet);
            }

        }


    }
    // properties class for storing the SPOTON Data
    public class SpotOnData
    {
        private string _empName;
        private string _empLoc;
        private string _empNTID;
        private string _adminName;
        //private string _OEMName;

        //public string OEM
        //{
        //    get
        //    {
        //        return this._OEMName;
        //    }
        //    set
        //    {
        //        this._OEMName = value;
        //    }
        //}

        public string EmpName
        {
            get
            {
                return this._empName;
            }
            set
            {
                this._empName = value;
            }
        }

        public string AdminName
        {
            get
            {
                return this._adminName;
            }
            set
            {
                this._adminName = value;
            }
        }

        public string EmpNTID
        {
            get
            {
                return this._empNTID;
            }
            set
            {
                this._empNTID = value;
            }
        }
        public string EmpLocation
        {
            get
            {
                return this._empLoc;
            }
            set
            {
                this._empLoc = value;
            }
        }
    }
}