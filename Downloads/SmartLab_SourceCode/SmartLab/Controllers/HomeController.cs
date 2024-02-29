using LabBookingWrap;
using LC_Reports_V1.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LC_Reports_V1.Controllers
{
    
    public class HomeController : Controller
    {
        public static Logger logger = LogManager.GetCurrentClassLogger();
        
        public ActionResult Index()
        {
           
            logger.Trace("Logging Information");
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        //public ActionResult LandingPage()
        //{
        //    LCfilterInfo lcDates = new Models.LCfilterInfo();
        //    lcDates.startDate = DateTime.Now;
        //    lcDates.endDate = DateTime.Now.AddMonths(1);
        //    ViewBag.Message = "Your landing page.";

        //    return View(lcDates);
        //}
        //[HttpPost]
        //public ActionResult LandingPage(FormCollection LCDates)
        //{


        //    LCfilterInfo lcDates = new Models.LCfilterInfo();
        //    lcDates.startDate = DateTime.Parse(LCDates["startDate"]);
        //    lcDates.endDate = DateTime.Parse(LCDates["endDate"]);


        //    //return View("Index");

        //    return RedirectToAction("Index", "LabCar", lcDates);
        //    //return new RedirectResult(@"~\LabCar\Index");
        //}

        //public void updateDB()
        //{
        //    string apistatus = LabBookingWrapper.APIInit("tracker");

        //    try
        //    {
        //        WrapperSites siteinfo = new WrapperSites();
        //        siteinfo = LabBookingWrapper.GetSites();
        //        if (siteinfo.successMsg.Contains("SUCCESS"))
        //            using (BookingServerRecentEntities db = new BookingServerRecentEntities())
        //            {
        //                foreach (WrapperSite site in siteinfo.SitesList)
        //                {
        //                    Site s = new Site();
        //                    s.CountryCode = site.CountryCode;
        //                    s.Description = site.Description;
        //                    s.DisplayName = site.DisplayName;
        //                    s.Id = site.Id;
        //                    s.RbCode = site.RbCode;
        //                    s.TimeZone = site.TimeZone;
        //                    db.Sites.Add(s);
        //                    db.SaveChanges();
        //                }

        //            }



        //    }
        //    catch (Exception ex) { }
        //    finally { LabBookingWrapper.APIDispose(); }

        //}

        [HttpPost]
        public ActionResult GetAdmin()
        {

            using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

            //using (BudgetingToolDB_Entities db = new BudgetingToolDB_Entities())

            {
                //bool adminstatus = db.

                 List<tbl_UserIDs_Table> lstPrivileged = new List<tbl_UserIDs_Table>();
                lstPrivileged = db.tbl_UserIDs_Table.ToList<tbl_UserIDs_Table>();
                lstPrivileged.Sort((a, b) => a.ADSID.CompareTo(b.ADSID));


                System.Security.Principal.IPrincipal User = System.Web.HttpContext.Current.User;
                if (lstPrivileged.Find(person => person.ADSID.ToUpper().Equals(User.Identity.Name.Split('\\')[1].ToUpper())) != null)

                    return Json(new { successmsg = true, message = "Updated Successfully" }, JsonRequestBehavior.AllowGet);
                else
                    return Json(new { errormsg = true }, JsonRequestBehavior.AllowGet);

            }


        }
    }
}