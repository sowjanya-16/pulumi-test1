using LC_Reports_V1.Models;
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
    public class InventoryController : Controller
    {
        // GET: Inventory
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult FAQ()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

    }
}