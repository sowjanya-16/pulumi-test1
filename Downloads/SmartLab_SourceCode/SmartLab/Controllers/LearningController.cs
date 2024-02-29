using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LC_Reports_V1.Controllers
{
    [Authorize(Users = @"apac\din2cob,de\add2abt,de\let2abt,de\ton2abt,apac\mta2cob,apac\gph2hc,apac\oig1cob,apac\mae9cob")]
    public class LearningController : Controller
    {
        // GET: Learning
        public ActionResult LearningModules()
        {
            return View();
        }
    }
}