using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LC_Reports_V1.Controllers
{
    public class Downtime_ASController : Controller
    {
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


        // GET: Downtime_AS
        public ActionResult Downtime_AS()
        {
            return View();
        }
        [HttpGet]
        public ActionResult LoadDropdowns()
        {
            connection();
            DataSet ds = new DataSet();
            //string Query = " select ComputerID, Comp_DisplayName from OSS_LabAdmin order by Comp_DisplayName, ComputerID ; Select 'TSG4' as CatID, 'TSG4' as CatName, 0 as DisplayName Union Select 'Pbox' as CatID, 'Pbox' as CatName, 1 as DisplayName Union Select 'MLC' as CatID, 'MLC' as CatName, 2 as DisplayName Union Select 'SW' as CatID, 'SW' as CatName, 3 as DisplayName Union Select 'TurboLIFT' as CatID, 'TurboLIFT' as CatName, 4 as DisplayName Union Select 'PC' as CatID, 'TurboLIFT' as CatName, 5 as DisplayName Union Select 'Others' as CatID, 'Others' as CatName , 6 as DisplayName order by DisplayName ;   ";
            string Query = "Select LabComputersPr.Id  as ComputerID, LabComputersPr.DisplayName as Comp_DisplayName from labinfo inner join Locations on LabInfo.LocationId = Locations.Id inner join LabTypes on LabInfo.TypeId = LabTypes.Id inner join Sites on  Locations.SiteId = Sites.Id  inner join LabComputersPr on LabInfo.id = LabComputersPr.LabId where LabComputersPr.isActive =1 and LabComputersPr.Id in (Select LabComputersPr.Id from labinfo inner join Locations on LabInfo.LocationId = Locations.Id inner join LabTypes on LabInfo.TypeId = LabTypes.Id inner join Sites on  Locations.SiteId = Sites.Id  inner join LabComputersPr on LabInfo.id = LabComputersPr.LabId where LabInfo.TypeId not in (14,26,73,74)) order by LabComputersPr.Id; Select Cat_ID as CatID, Cat_Name as CatName from AS_DownTime_Category order by Cat_ID";
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

            return Json(new { CompList = Compresult, CatList = Catresult }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult GetDownTimeDetails()
        {
            connection();
            DataTable dt = new DataTable();
            //string Query = " Select SNo,Lab_ID,Lab_Name,ComputerID,Comp_DisplayName,FQDN,Location,Category,Start_Time,End_Time,ALM_imp_ID,Remarks,ModifiedUser from OSS_DownTime order by Comp_DisplayName,ComputerID  ";
            //change query
            string Query = " Select A.SNo,A.Lab_ID,A.Lab_Name,A.ComputerID,A.Comp_DisplayName,A.FQDN,A.Location,A.Category,Start_Time,End_Time,Ticket_ID,Remarks,ModifiedUser , D.OEM as Project from AS_DownTime A inner join LabComputersPr B on A.ComputerID = B.ID inner join LabInfo C on C.id= B.LabID inner join LabOEMs D on D.id=C.OEM where B.isActive = 1 order by A.SNo desc ";
            OpenConnection();
            SqlCommand cmd = new SqlCommand(Query, con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            CloseConnection();

            List<Get_DownTimeDetails_AS> DownTimeresult = new List<Get_DownTimeDetails_AS>();

            foreach (DataRow row in dt.Rows)
            {
                Get_DownTimeDetails_AS item = new Get_DownTimeDetails_AS();
                item.SNo = Convert.ToInt32(row["SNo"].ToString());
                item.ComputerID = row["ComputerID"].ToString();
                //item.ComputerName = row["Comp_DisplayName"].ToString();
                item.FQDN = row["FQDN"].ToString();
                item.Location = row["Location"].ToString();
                item.Category = row["Category"].ToString();
                item.StartTime = row["Start_Time"].ToString();
                if (row["End_Time"].ToString() == "1/1/1900 12:00:00 AM")
                    item.EndTime = "";
                else
                    item.EndTime = row["End_Time"].ToString();
                item.Ticket_ID = row["Ticket_ID"].ToString();
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
            string Query = " select A.FQDN,  D. RbCode as Location , E.OEM as Project from LabComputersPr A inner join Labinfo B on A.labid=B.id inner join Locations C on B.LocationID= C.id inner join Sites D on c.siteid=D.id inner join LabOEMs E on B.OEM= E.id  Where A.isActive=1 and A.ID = " + CompID.ToString() + "  ";
            OpenConnection();
            SqlCommand cmd = new SqlCommand(Query, con);
            SqlDataReader dr = cmd.ExecuteReader();
            string FQDN = "", Location = "", Project = "";
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
        public ActionResult SaveDownTimeDetails(Get_DownTimeDetails_AS item)
        {
            //DateTime sdate = DateTime.ParseExact(item.StartTime.Substring(0, 18), "yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);

            // "ddd MMM yyyy HH:mm:ss",
            string user = User.Identity.Name.Split('\\')[1].ToUpper();
            DateTime sdate = Convert.ToDateTime(item.StartTime);
            DateTime edate;
            DateTime Modified_date = new DateTime(1900, 01, 01);
            Modified_date = DateTime.Now;
            if (item.EndTime == null || item.EndTime == "")
                edate = new DateTime(1900, 01, 01);
            else
            {
                edate = Convert.ToDateTime(item.EndTime);
                //edate = DateTime.ParseExact(item.EndTime.Substring(0, 18), "yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);
                if (Convert.ToDateTime(edate.ToShortDateString()) > Convert.ToDateTime(DateTime.Now.ToShortDateString()))
                    return Json(new { data = GetDownTimeDetails(), success = false, msg = "End Date should not be greater than current date, Please check!" }, JsonRequestBehavior.AllowGet);
                else if (Convert.ToDateTime(edate.ToShortDateString()) < Convert.ToDateTime(sdate.ToShortDateString()))
                    return Json(new { data = GetDownTimeDetails(), success = false, msg = "End Date should be greater than start date, Please check!" }, JsonRequestBehavior.AllowGet);


            }



            string Query = "";
            int result = 0;
            connection();
            if (item.Ticket_ID == null)
                return Json(new { data = GetDownTimeDetails(), success = false, msg = "Please enter ALM Impediment ID!" }, JsonRequestBehavior.AllowGet);

            if (item.Remarks == null)
                item.Remarks = string.Empty;

            Query = " Exec [dbo].[InsertASDownTime] '" + item.ComputerID.ToString() + "','" + item.Category.ToString() + "','" + sdate + "','" + edate + "','" + Modified_date + "','" + item.Ticket_ID.ToString() + "','" + item.Remarks.ToString() + "','" + user + "','" + item.SNo.ToString() + "' ";

            OpenConnection();
            SqlCommand command = new SqlCommand(Query, con);
            command.CommandTimeout = 3000;
            command.ExecuteNonQuery();
            CloseConnection();
            return Json(new { data = GetDownTimeDetails(), success = true, msg = "Updated successsfully!" }, JsonRequestBehavior.AllowGet);
            // }


        }

    }
    public class Get_DownTimeDetails_AS
    {

        private int _SNo;
        private string _ComputerId;
        //private string _CompName;
        private string _FQDN;
        private string _Location;
        private string _Category;
        private string _StartTime;
        private string _EndTime;
        private string _Ticket_ID;
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

        public string Ticket_ID
        {
            get { return this._Ticket_ID; }
            set { this._Ticket_ID = value; }
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
}