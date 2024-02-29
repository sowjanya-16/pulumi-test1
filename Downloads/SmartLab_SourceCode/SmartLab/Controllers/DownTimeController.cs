using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Threading;
using ClosedXML.Excel;
using System.Globalization;

using System.Configuration;
using System.Web.Script.Serialization;
using System.Data.Entity;
using System.Text;

namespace LC_Reports_V1.Controllers
{
    public class DownTimeController : Controller
    {
        // GET: DownTime

        private SqlConnection con;
        

        private void connection()
        {

            //var datasource = @"BANEN1093154\SQLEXPRESSESPIS5";//your server
            //var database = "BookingServerReplica"; //your database name
            //var username = "jov6cob"; //username of server to connect
            //var password = "serveruser@123"; //password

            ////your connection string 
            //string connString = @"Data Source=" + datasource + ";Initial Catalog="
            //            + database + ";Persist Security Info=True;User ID=" + username + ";Password=" + password;

            string constring = ConfigurationManager.ConnectionStrings["dbConnection"].ToString();
            con = new SqlConnection(constring);
        }

        private void OpenConnection()
        {
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
        }

        private void CloseConnection()
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult LoadDropdowns()
        {
            connection();
            DataSet ds = new DataSet();
            string Query = " select ComputerID, Comp_DisplayName from OSS_LabAdmin order by Comp_DisplayName, ComputerID ; Select 'TSG4' as CatID, 'TSG4' as CatName, 0 as DisplayName Union Select 'Pbox' as CatID, 'Pbox' as CatName, 1 as DisplayName Union Select 'MLC' as CatID, 'MLC' as CatName, 2 as DisplayName Union Select 'SW' as CatID, 'SW' as CatName, 3 as DisplayName Union Select 'TurboLIFT' as CatID, 'TurboLIFT' as CatName, 4 as DisplayName Union Select 'PC' as CatID, 'TurboLIFT' as CatName, 5 as DisplayName Union Select 'Others' as CatID, 'Others' as CatName , 6 as DisplayName order by DisplayName ;   ";
            OpenConnection();
            SqlCommand cmd = new SqlCommand(Query, con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            CloseConnection();

            List<LookUpData.LookUp_CompList> Compresult = new List<LookUpData.LookUp_CompList>();

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                LookUpData.LookUp_CompList item = new LookUpData.LookUp_CompList();
                item.CompID = row["ComputerID"].ToString();
                item.CompName = row["Comp_DisplayName"].ToString();
                Compresult.Add(item);
            }

            List<LookUpData.LookUp_CatList> Catresult = new List<LookUpData.LookUp_CatList>();

            foreach (DataRow row in ds.Tables[1].Rows)
            {
                LookUpData.LookUp_CatList item = new LookUpData.LookUp_CatList();
                item.CatID = row["CatID"].ToString();
                item.CatName = row["CatName"].ToString();
                Catresult.Add(item);
            }

            return Json(new { CompList = Compresult, CatList = Catresult } , JsonRequestBehavior.AllowGet);
        }

       
        [HttpPost]
        public ActionResult GetDownTimeDetails()
        {
            connection();
            DataTable dt = new DataTable();
            //string Query = " Select SNo,Lab_ID,Lab_Name,ComputerID,Comp_DisplayName,FQDN,Location,Category,Start_Time,End_Time,ALM_imp_ID,Remarks,ModifiedUser from OSS_DownTime order by Comp_DisplayName,ComputerID  ";
            string Query = " Select A.SNo,A.Lab_ID,A.Lab_Name,A.ComputerID,A.Comp_DisplayName,A.FQDN,A.Location,A.Category,Start_Time,End_Time,ALM_imp_ID,Remarks,ModifiedUser , B.Current_Project as Project from OSS_DownTime A inner join oss_LabAdmin B on A.ComputerID = B.ComputerID order by A.Comp_DisplayName,A.ComputerID ";
            OpenConnection();
            SqlCommand cmd = new SqlCommand(Query, con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            CloseConnection();

            List<Get_DownTimeDetails> DownTimeresult = new List<Get_DownTimeDetails>();

            foreach (DataRow row in dt.Rows)
            {
                Get_DownTimeDetails item = new Get_DownTimeDetails();
                item.SNo = Convert.ToInt32(row["SNo"].ToString());
                item.ComputerID = row["ComputerID"].ToString();
                //item.ComputerName = row["Comp_DisplayName"].ToString();
                item.FQDN = row["FQDN"].ToString();
                item.Location = row["Location"].ToString();
                item.Category = row["Category"].ToString();
                item.StartTime = row["Start_Time"].ToString();
                if (row["End_Time"].ToString() == "1/1/1900 12:00:00 AM")
                    item.EndTime = null;
                else
                    item.EndTime = row["End_Time"].ToString();
                item.ALM_Impediment_ID = row["ALM_imp_ID"].ToString();
                item.Remarks = row["Remarks"].ToString();
                item.ModifiedUser = row["ModifiedUser"].ToString(); 
                item.Project = row["Project"].ToString();
                DownTimeresult.Add(item);
            }

            return Json(new { data = DownTimeresult, JsonRequestBehavior.AllowGet });
        }

        [HttpPost]
        public ActionResult GetFQDN(string CompID)
        {
            connection();
            DataTable dt = new DataTable();
            string Query = " select FQDN, Location, Current_Project as Project from OSS_LabAdmin Where ComputerID = " + CompID.ToString() + "  ";
            OpenConnection();
            SqlCommand cmd = new SqlCommand(Query, con);
            SqlDataReader dr = cmd.ExecuteReader();
            string FQDN = "",Location = "", Project = "";
            if (dr.HasRows)
            {
                dr.Read();
                FQDN = dr["FQDN"].ToString();
                Location = dr["Location"].ToString();
                Project = dr["Project"].ToString();
            }
            CloseConnection();
            return Json(new { FQDN = FQDN, Location = Location, Project = Project, JsonRequestBehavior.AllowGet });
        }

        [HttpPost]
        public ActionResult SaveDownTimeDetails(Get_DownTimeDetails item)
        {
            //DateTime sdate = DateTime.ParseExact(item.StartTime.Substring(0, 18), "yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);
            // "ddd MMM yyyy HH:mm:ss",
            string user = User.Identity.Name.Split('\\')[1].ToUpper();
            DateTime sdate = DateTime.Parse(item.StartTime);
            DateTime edate;
            if (item.EndTime == null || item.EndTime == "")
                edate = new DateTime(1900, 01, 01);
            else
            {
                edate = DateTime.Parse(item.EndTime);
                if (Convert.ToDateTime(edate.ToShortDateString()) > Convert.ToDateTime(DateTime.Now.ToShortDateString()))
                    return Json(new { data = GetDownTimeDetails(), success = false, msg = "End Date should not be greater than current date, Please check!" }, JsonRequestBehavior.AllowGet);
                else if (Convert.ToDateTime(edate.ToShortDateString()) < Convert.ToDateTime(sdate.ToShortDateString()))
                    return Json(new { data = GetDownTimeDetails(), success = false, msg = "End Date should be greater than start date, Please check!" }, JsonRequestBehavior.AllowGet);


            }



            string Query = "";
                int result = 0;
                connection();
                if (item.ALM_Impediment_ID == null)
                    return Json(new { data = GetDownTimeDetails(), success = false, msg = "Please enter ALM Impediment ID!" }, JsonRequestBehavior.AllowGet);

            if (item.Remarks == null)
                    item.Remarks = string.Empty;
                
                Query = " Exec [dbo].[InsertOssDownTime] '" + item.ComputerID.ToString() + "','" + item.Category.ToString() + "','" + sdate + "','" + edate + "','" + item.ALM_Impediment_ID.ToString() + "','" + item.Remarks.ToString() + "','" + user + "','" + item.SNo.ToString() + "' ";

                OpenConnection();
                SqlCommand command = new SqlCommand(Query, con);
                 command.CommandTimeout = 300;
                 command.ExecuteNonQuery();
                CloseConnection();
                return Json(new { data = GetDownTimeDetails(), success = true, msg = "Updated successsfully!" }, JsonRequestBehavior.AllowGet);
           // }

            
        }

    }

    public class Get_DownTimeDetails
    {

            private int _SNo;
            private string _ComputerId;
            //private string _CompName;
            private string _FQDN;
            private string _Location;
            private string _Category;            
            private string _StartTime;
            private string _EndTime;
            private string _ALM_Impediment_ID;
            private string _Remarks;
            private string _ModifiedUser;
            private string _Project;

        public int SNo
            {
                get { return this._SNo; }
                set { this._SNo = value; }
            }
            public string ComputerID
            {
                get { return this._ComputerId; }
                set { this._ComputerId = value; }
            }

            //public string ComputerName
            //{
            //    get { return this._CompName; }
            //    set { this._CompName = value; }
            //}

            public string FQDN
            {
                get { return this._FQDN; }
                set { this._FQDN = value; }
            }

            public string Location
            {
                get { return this._Location; }
                set { this._Location = value; }
            }

            public string Category
            {
                get { return this._Category; }
                set { this._Category = value; }
            }

            
            public string StartTime
            {
                get { return this._StartTime; }
                set { this._StartTime = value; }
            }

            public string EndTime
            {
                get { return this._EndTime; }
                set { this._EndTime = value; }
            }
       
            public string ALM_Impediment_ID
        {
                get { return this._ALM_Impediment_ID; }
                set { this._ALM_Impediment_ID = value; }
            }
            public string Remarks
            {
                get { return this._Remarks; }
                set { this._Remarks = value; }
            }

            public string ModifiedUser
            {
                get { return this._ModifiedUser; }
                set { this._ModifiedUser = value; }
            }

            public string Project
            {
                get { return this._Project; }
                set { this._Project = value; }
            }
    }

    public class AddEdit_DownTimeDetails
    {
        private string _ModifiedUser;
        private int _SNo;
        private string _ComputerId;
       // private string _CompName;
        private string _FQDN;
        private string _Location;
        private string _Category;
        private DateTime _StartTime;
        private DateTime _EndTime;
        private string _ALM_Impediment_ID;
        private string _Remarks;
        private string _Project;

        public int SNo
        {
            get { return this._SNo; }
            set { this._SNo = value; }
        }
        public string ComputerID
        {
            get { return this._ComputerId; }
            set { this._ComputerId = value; }
        }

        //public string ComputerName
        //{
        //    get { return this._CompName; }
        //    set { this._CompName = value; }
        //}

        public string FQDN
        {
            get { return this._FQDN; }
            set { this._FQDN = value; }
        }

        public string Location
        {
            get { return this._Location; }
            set { this._Location = value; }
        }

        public string Category
        {
            get { return this._Category; }
            set { this._Category = value; }
        }


        public DateTime StartTime
        {
            get { return this._StartTime; }
            set { this._StartTime = value; }
        }

        public DateTime EndTime
        {
            get { return this._EndTime; }
            set { this._EndTime = value; }
        }

        public string ALM_Impediment_ID
        {
            get { return this._ALM_Impediment_ID; }
            set { this._ALM_Impediment_ID = value; }
        }
        public string Remarks
        {
            get { return this._Remarks; }
            set { this._Remarks = value; }
        }

        public string ModifiedUser
        {
            get { return this._ModifiedUser; }
            set { this._ModifiedUser = value; }
        }

        public string Project
        {
            get { return this._Project; }
            set { this._Project = value; }
        }
    }




    public class LookUpData
    {
        public List<LookUp_CompList> ComputerList { get; set; }
        public List<LookUp_CatList> CategoryList { get; set; }

        public class LookUp_CompList
        {
            private string _CompID;
            private string _CompName;

            public string CompID
            {
                get { return this._CompID; }
                set { this._CompID = value; }
            }

            public string CompName
            {
                get { return this._CompName; }
                set { this._CompName = value; }
            }
        }

        public class LookUp_CatList
        {
            private string _CatID;
            private string _CatName;


            public string CatID
            {
                get { return this._CatID; }
                set { this._CatID = value; }
            }

            public string CatName
            {
                get { return this._CatName; }
                set { this._CatName = value; }
            }
        }
    }
    
}