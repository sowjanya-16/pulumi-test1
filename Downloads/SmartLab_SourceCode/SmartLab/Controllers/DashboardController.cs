using ClosedXML.Excel;
using LabBookingAPI.Objects;
using LabBookingWrap;
using LC_Reports_V1.Models;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using static LC_Reports_V1.Models.LCfilterInfo;

namespace LC_Reports_V1.Controllers
{
    public class DashboardController : Controller
    {
        public static bool ServerTimeout = false;
        System.Timers.Timer timeout_check = new System.Timers.Timer();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Dashboard_Options()
        {

            LCfilterInfo lcfilterInfo = new Models.LCfilterInfo();
            try
            {

                using (BookingServerReplicaEntities db = new BookingServerReplicaEntities())
                {
                    List<Models.Site> lstSites = db.Sites.ToList();
                    List<Models.Location> lstLocations = db.Locations.ToList();
                    List<Models.LabType> labTypes = db.LabTypes.ToList();
                    List<LabInfo> labInfo = db.LabInfoes.ToList();


                    lcfilterInfo.startDate = DateTime.Now.AddMonths(-1).Date;
                    lcfilterInfo.endDate = DateTime.Now.AddDays(-1).Date;
                    lcfilterInfo.LCLabType = "NA";
                    lcfilterInfo.LCLocationName = "NA";
                    lcfilterInfo.LCLabID = "NA";
                    lcfilterInfo.LabCartypes = new List<SelectListItem>();


                    labTypes = labTypes.GroupBy(x => x.DisplayName).Select(x => x.First()).ToList();
                    foreach (Models.LabType labattr in labTypes)
                    {
                        lcfilterInfo.LabCartypes.Add(new SelectListItem { Text = labattr.DisplayName.Trim(), Value = labattr.DisplayName.Trim() });
                    }
                    lcfilterInfo.LabCartypes.Sort((a, b) => a.Value.CompareTo(b.Value));
                    lcfilterInfo.Locations = new List<SelectListItem>();
                    foreach (Models.Site sitesattr in lstSites)
                    {
                        lcfilterInfo.Locations.Add(new SelectListItem { Text = sitesattr.DisplayName, Value = sitesattr.RbCode });

                    }
                    lcfilterInfo.Locations.Sort((a, b) => a.Value.CompareTo(b.Value));
                    lcfilterInfo.LabidViews = GetViewsByUser(); // returns scheduler views list
                    lcfilterInfo.scheduledLabidsCnt = lcfilterInfo.LabidViews.Count();//number of Scheuler Views            
                    lcfilterInfo.listofScheduledLabIds = new List<SelectListItem>();
                    lcfilterInfo.listofScheduledLabIds = lcfilterInfo.LabidViews.Select(x => new SelectListItem
                    {
                        Value = x.Split('|')[1].Trim(),
                        Text = x.Split('|')[0].Trim()
                    });
                    lcfilterInfo.LabIDs = new List<SelectListItem>();
                    foreach (var lab in lcfilterInfo.listofScheduledLabIds)
                    {
                        lcfilterInfo.LabIDs.Add(new SelectListItem { Text = lab.Text, Value = lab.Value });
                    }
                    lcfilterInfo.LabIDs.Sort((a, b) => a.Text.CompareTo(b.Text));
                    labInfo = labInfo.OrderBy(item => item.DisplayName).ToList();
                    foreach (LabInfo lab in labInfo)
                    {
                        //if (lab.TypeId == 8 || lab.TypeId == 9 || lab.TypeId == 10 || lab.TypeId == 20 || lab.TypeId == 49 || lab.TypeId == 50 || lab.TypeId == 69)
                        //{
                            if (!lcfilterInfo.LabIDs.Select(x => x.Value).ToList().Contains(lab.Id.ToString()))
                                lcfilterInfo.LabIDs.Add(new SelectListItem { Text = lab.DisplayName, Value = lab.Id.ToString() });
                       // }
                    }
                }


            }
            catch (Exception ex)
            {
                HomeController.logger.Error(ex, "Dashboard Options");
                return RedirectToAction("Index", "Home");
            }


            return View(lcfilterInfo);
        }
        public List<String> GetViewsByUser()
        {

            string apistatus = LabBookingWrapper.APIInit("tracker");
            try
            {
                List<String> schedulerViews = new List<string>();
                if (apistatus.Contains("SUCCESS"))
                {

                    WrapperUserInfo userinfo = new WrapperUserInfo();
                    userinfo = LabBookingWrapper.GetUserIdByName("mta2cob");
                    //userinfo = LabBookingWrapper.GetUserIdByName(User.Identity.Name.Split('\\')[1].ToLower());
                    if (userinfo.successMsg.Contains("SUCCESS") && userinfo.UserInfo != null)
                    {
                        WrapperViews viewinfo = new WrapperViews();
                        viewinfo = LabBookingWrapper.GetSchedulerViewsByUser(userinfo.UserInfo.Id);
                        string viewids = viewinfo.SelectedLabIds;
                        //schedulerViews = viewids.Split(',').ToList();

                        foreach (string id in viewids.Split(',').ToList().Distinct())
                        {
                            if (id != null && id != string.Empty && id != "")
                            {
                                // use db here
                                using (BookingServerReplicaEntities db = new BookingServerReplicaEntities())
                                {

                                    LabInfo labInfo = new LabInfo();
                                    int labid = Int32.Parse(id);
                                    labInfo = db.LabInfoes.Where(item => item.Id == labid).FirstOrDefault<LabInfo>();
                                    if (labInfo != null)
                                        schedulerViews.Add(labInfo.DisplayName + "|" + id);

                                }
                                //WrapperLab labinfo = new WrapperLab();
                                //labinfo = LabBookingWrapper.GetLabByLabID(Convert.ToInt32(id));
                                //schedulerViews.Add(labinfo.DisplayName + "|" + id);
                            }
                            else
                                continue;
                        }

                    }

                }

                return schedulerViews;
            }
            catch (Exception ex)
            {
                HomeController.logger.Error(ex, "GetViewsByUser");
                throw ex;
            }
            finally { LabBookingWrapper.APIDispose(); }

        }

        [HttpPost]
        public ActionResult TSOU_Chart(int[] labIdvalue, string startdate, string enddate)
        {

            DateTime SDate = DateTime.Parse(startdate);
            DateTime EDate = DateTime.Parse(enddate);
            LCfilterInfo.LabBookingExport objLabExport = new LCfilterInfo.LabBookingExport();

            using (BookingServerReplicaEntities db = new BookingServerReplicaEntities())
            {

                //List<Models.LabInfo> wrapperLabs = db.LabInfoes.ToList();
                List<Models.LabType> objLabType = db.LabTypes.ToList();
                List<Models.Location> lstLabLocs = db.Locations.ToList();
                List<Models.Site> objSites = db.Sites.ToList();
                objLabExport = new LCfilterInfo.LabBookingExport();
                objLabExport.Labs = new LCfilterInfo.LabBookingExportLab[labIdvalue.Length];
                objLabExport.StartDate = SDate;
                //objLabExport.EndDate = EDate.AddDays(1).AddSeconds(-(EDate.TimeOfDay.TotalSeconds + 1));

                //objLabExport.StartDate_UI = SDate.ToShortDateString();
                //objLabExport.EndDate_UI = EDate.ToShortDateString();

                if (EDate.Date < DateTime.Now.Date)
                {
                    objLabExport.EndDate = EDate;
                    objLabExport.StartDate_UI = SDate.ToShortDateString();
                    objLabExport.EndDate_UI = EDate.ToShortDateString();
                }
                else
                {
                    objLabExport.EndDate = EDate.AddSeconds(-(1));
                    objLabExport.StartDate_UI = SDate.ToShortDateString();
                    objLabExport.EndDate_UI = EDate.AddSeconds(-1).ToShortDateString();
                }
                try
                {
                    int labcount = 0;
                    double totalManhours = 0;
                    double totalManCaplhours = 0;
                    double totalAutohours = 0;
                    double totalAutoCaplhours = 0;

                    foreach (var lab in labIdvalue)
                    {
                        objLabExport.Labs[labcount] = new  LCfilterInfo.LabBookingExportLab();
                        List<LabComputersPr> labComputers = new List<LabComputersPr>();
                       // objLabExport.Labs[labcount].PCname = string.Empty;
                        labComputers = db.LabComputersPrs.Where(x => x.LabId.ToString() == lab.ToString() && x.IsActive == true).ToList();


                        if (labComputers != null)
                        {
                            int labid = lab;
                            int pccnt = 0, daycount = 0;
                            double ManualTotalHours = 0;
                            double AutomatedTotalHours = 0;
                            double ManualCaplTotalHours = 0;
                            double AutomatedCaplTotalHours = 0;
                            List<DailySummaryTable_v3> dailysummary = new List<DailySummaryTable_v3>();
                            SDate = DateTime.Parse(startdate);

                            while (SDate <= EDate)
                            {
                                foreach (var computer in labComputers)
                                {
                                    //if (labComputers == null)
                                    //{
                                    //    continue;
                                    //}
                                    //else
                                    //{
                                    //    objLabExport.Labs[labcount].PCname = computer.FQDN + objLabExport.Labs[labcount].PCname;
                                    //}
                                    int compid = computer.Id;
                                    string sDt = SDate.ToString("yyyy-MM-dd");
                                    dailysummary = db.DailySummaryTable_v3.Where(x => x.ComputerID.ToString() == compid.ToString() && x.Date == sDt).ToList();
                                    foreach (var summary in dailysummary)
                                    {
                                        ManualTotalHours += Convert.ToDouble(summary.ManualTotalHours);
                                        AutomatedTotalHours += Convert.ToDouble(summary.AutomatedTotalHours);
                                        ManualCaplTotalHours += Convert.ToDouble(summary.ManualCaplTotalHours);
                                        AutomatedCaplTotalHours += Convert.ToDouble(summary.AutomatedCaplTotalHours);
                                    }
                                    pccnt++;
                                }
                                daycount++;

                                SDate = SDate.AddDays(1);

                            }
                            objLabExport.Labs[labcount].ManualTotalHours = ManualTotalHours;
                            objLabExport.Labs[labcount].AutomatedTotalHours = AutomatedTotalHours;
                            objLabExport.Labs[labcount].ManualCaplTotalHours = ManualCaplTotalHours;
                            objLabExport.Labs[labcount].AutomatedCaplTotalHours = AutomatedCaplTotalHours;
                            objLabExport.Labs[labcount].TotalSum = ManualCaplTotalHours + ManualTotalHours + AutomatedCaplTotalHours + AutomatedTotalHours;

                            totalManhours += ManualTotalHours;
                            totalManCaplhours += ManualCaplTotalHours;
                            totalAutohours += AutomatedTotalHours;
                            totalAutoCaplhours += AutomatedCaplTotalHours;

                            LabInfo labInfo = new LabInfo();
                            labInfo = db.LabInfoes.Where(item => item.Id == lab).FirstOrDefault();
                            if (labInfo != null)
                            {
                                objLabExport.Labs[labcount].Inventory = db.LabInfoes.FirstOrDefault(i => i.Id == lab).DisplayName;
                                objLabExport.Labs[labcount].Model = db.LabTypes.FirstOrDefault(x => x.Id == labInfo.TypeId).DisplayName;
                                objLabExport.Labs[labcount].OEM = db.LabOEMs.FirstOrDefault(item => item.ID.ToString() == db.LabInfoes.FirstOrDefault(x => x.Id == lab).OEM) == null ? "_" : db.LabOEMs.FirstOrDefault(item => item.ID.ToString() == db.LabInfoes.FirstOrDefault(x => x.Id == lab).OEM).OEM.Trim();

                            }

                            List<Models.Site> sites = db.Sites.ToList();
                            if (sites != null)
                            {
                                objLabExport.Labs[labcount].Location = sites.First(x => x.Id == db.Locations.First(loc => loc.Id == labInfo.LocationId).SiteId).RbCode;

                            }

                            if (objLabExport.Labs[labcount].Model.Contains("DA"))
                            {
                                objLabExport.Labs[labcount].TSOULabel = objLabExport.Labs[labcount].Inventory;
                            }
                            else
                            {
                                objLabExport.Labs[labcount].TSOULabel = string.Concat(objLabExport.Labs[labcount].Model, " ", objLabExport.Labs[labcount].Inventory, " ", objLabExport.Labs[labcount].Location);
                            }

                            labcount++;

                            //objLabExport.Labs[labcount].PCname = string.Empty;
                        }


                    }

                    objLabExport.OverallManualHours = totalManhours;
                    objLabExport.OverallAutomatedHours = totalAutohours;
                    objLabExport.OverallManualCaplHours = totalManCaplhours;
                    objLabExport.OverallAutomatedCaplHours = totalAutoCaplhours;

                }

                catch (Exception ex)
                {
                    HomeController.logger.Error(ex, "TSOU Chart");
                    return Json(new { success = false, message = "TSOU Unsuccessful" }, JsonRequestBehavior.AllowGet);
                }


                return Json(new { data = objLabExport }, JsonRequestBehavior.AllowGet);
            }
           
        }

        [HttpPost]
        public ActionResult ExportTSOUDataToExcel(List<TSOUExcel> excelobjtsou, string sdate, string edate, string loc, string type)
        {
            try
            {

                string filename = @"TSOU_List_" + loc + "_" + type + "_" + sdate + "_to_" + edate + ".xlsx";
                System.Data.DataTable dt = new System.Data.DataTable("TSOU Info");
                dt.Columns.AddRange(new DataColumn[9] {new DataColumn("LabID"),
                                            new DataColumn("LabType"),
                                            new DataColumn("Location"),
                                            new DataColumn("OEM" ),
                                            new DataColumn("Manual Hours Usage", typeof(double)),
                                            new DataColumn("Automated Hours Usage", typeof(double)),
                                            new DataColumn("Manual Capl Hours Usage", typeof(double)),
                                            new DataColumn("Automated Capl Hours Usage", typeof(double)),
                                            new DataColumn("Overall Time", typeof(double))});
                if (excelobjtsou == null)
                {
                    return Json(new { success = false, message = "TSOU Excel Unsuccessful" }, JsonRequestBehavior.AllowGet);
                }
                foreach (var info in excelobjtsou)
                {
                    dt.Rows.Add(info.Inventory, info.Model, info.Location, info.OEM, Math.Round(info.ManualTotalHours, 2), Math.Round(info.AutomatedTotalHours, 2), Math.Round(info.ManualCaplTotalHours, 2), Math.Round(info.AutomatedCaplTotalHours, 2), Math.Round(info.TotalSum, 2));
                }
                using (XLWorkbook wb = new XLWorkbook())
                {
                    var ws = wb.Worksheets.Add("TSOU Info");
                    ws.Range("A1:B2").Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    ws.Range("A1:B2").Style.Border.InsideBorder = XLBorderStyleValues.Dotted;
                    ws.Range("A1:B2").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    ws.Range("A1:B2").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    ws.Range("A1:B2").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    ws.Range("A1:B2").Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    ws.Range("A1:B2").Style.Font.Bold = true;
                    ws.Cell(1, 1).Style.Fill.BackgroundColor = XLColor.LightBlue;
                    ws.Cell(2, 1).Style.Fill.BackgroundColor = XLColor.LightBlue;
                    ws.Cell(1, 2).Style.Fill.BackgroundColor = XLColor.LightBlue;
                    ws.Cell(2, 2).Style.Fill.BackgroundColor = XLColor.LightBlue;

                    // ws.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGreen);
                    ws.Cell(1, 1).Value = "Start Date";
                    ws.Cell(1, 2).Value = sdate;
                    ws.Cell(2, 1).Value = "End Date";
                    ws.Cell(2, 2).Value = edate;
                    //ws.Cell(3, 1).Value = "Total Manual Hours";
                    //ws.Cell(3, 2).Value = tsouobj.OverallManualHours;
                    //ws.Cell(4, 1).Value = "Total Automated Hours";
                    //ws.Cell(4, 2).Value = tsouobj.OverallAutomatedHours;
                    //ws.Cell(5, 1).Value = "Total CAPL Manual Hours";
                    //ws.Cell(5, 2).Value = tsouobj.OverallManualCaplHours;
                    //ws.Cell(6, 1).Value = "Total CAPL Automated Hours";
                    //ws.Cell(6, 2).Value = tsouobj.OverallAutomatedCaplHours;


                    ws.Cell(4, 1).InsertTable(dt);
                    //wb.Worksheets.Add(dt);
                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);
                        var exobj = File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
                        return Json(exobj, JsonRequestBehavior.AllowGet);
                    }
                }
            }

            catch (Exception ex)
            {
                HomeController.logger.Error(ex, "ExportTSOUDataToExcel");
                return Json(new { success = false, message = "TSOU Excel Export Unsuccessful" }, JsonRequestBehavior.AllowGet);

            }

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
        public ActionResult TSIU_Chart(int id, DateTime startdate, DateTime enddate)
        {
            try
            {
                List<TSIU_Controller> labitem = GetLCData(id, startdate, enddate).FindAll(s => s.ID_key == id.ToString());
                if (labitem.Count == 0)
                {
                    throw new Exception("labitem is null");
                }
                labitem.Sort((a, b) => a.startTime.CompareTo(b.startTime));

                var serializer = new JavaScriptSerializer();

                // For simplicity just use Int32's max value.
                // You could always read the value from the config section mentioned above.
                serializer.MaxJsonLength = Int32.MaxValue;


                var result = new ContentResult
                {
                    Content = serializer.Serialize(labitem),
                    ContentType = "application/json"
                };
                JsonResult result1 = Json(result);
                result1.MaxJsonLength = Int32.MaxValue;
                result1.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

                return Json(new { data = result1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                HomeController.logger.Error(ex, "ExportTSIUDataToExcel");
                return Json(new { success = false, message = "TSIU Excel Export Unsuccessful" }, JsonRequestBehavior.AllowGet);
            }

        }
        public List<TSIU_Controller> GetLCData(int id, DateTime StartDate, DateTime EndDate)
        {
            ReportParameters_TSIU objLabReport = new ReportParameters_TSIU();
            Model tempModelObj = new Model();
            TSIU_Controller tempTSIUObj = new TSIU_Controller();
            List<TSIUActivity> lstDash = new List<TSIUActivity>();


            if (EndDate.ToShortDateString() == DateTime.Now.ToShortDateString())
            {
                EndDate = DateTime.Now.AddDays(-1);
            }

            try
            {
                using (BookingServerReplicaEntities db = new BookingServerReplicaEntities())
                {


                    List<Models.Site> objSites = new List<Models.Site>();
                    objSites = db.Sites.ToList();
                    LabInfo labInfo = new LabInfo();
                    labInfo = db.LabInfoes.Where(item => item.Id == id).FirstOrDefault();

                    lstDash.Add(GetLabActivity(id, StartDate, EndDate));
                    if (lstDash != null && lstDash.Count > 0)
                    {
                        if (objLabReport.LCParams == null)
                            objLabReport.LCParams = new List<Model>();
                        if (objLabReport.LCTSIUParams == null)
                        {
                            objLabReport.LCTSIUParams = new List<TSIU_Controller>();

                        }
                    }
                    foreach (var lab in lstDash)
                    {
                        if (lab.LabID == 0)
                        {
                            continue;
                        }
                        //Report ground work
                        tempModelObj.LCID = lab.LabID.ToString();
                        tempModelObj.LCLocation = lab.LabLocation;
                        tempModelObj.LCModel = lab.LabType;

                        if (lab.manualActivities != null && lab.manualActivities.Count > 0)
                        {
                            #region  code for individual split of activity for more that 24 hrs
                            foreach (LCfilterInfo.ExportManSessSpan manTime in lab.manualActivities)
                            {
                                DateTime manstartdt = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                                manstartdt = manstartdt.AddSeconds(manTime.start).ToLocalTime();
                                System.DateTime manenddt = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                                manenddt = manenddt.AddSeconds(manTime.end).ToLocalTime();



                                if (manenddt.Date < StartDate.Date || manstartdt.Date > EndDate.Date)
                                    continue;

                                //remove overlapping times on the boundaries of chosen start and end date
                                List<LabBookingExportManualSessSpan> templist = new List<LabBookingExportManualSessSpan>();
                                bool dayChangeFlag = false;
                                if (manstartdt.Day == manenddt.Day)
                                {
                                    dayChangeFlag = false;
                                }
                                else
                                {
                                    dayChangeFlag = true;
                                }
                                if (!dayChangeFlag)
                                {

                                }
                                else
                                {
                                    double daydiff = manenddt.Subtract(manstartdt).TotalDays;
                                    DateTime tempsDate = manstartdt;
                                    double daydiffer = manenddt.Subtract(manstartdt).TotalDays;
                                    do
                                    {
                                        if (manenddt.Date == tempsDate.Date)
                                        {
                                            LabBookingExportManualSessSpan tempobj = new LabBookingExportManualSessSpan();
                                            tempobj.isActive = manTime.isActive;
                                            tempobj.trigger = manTime.trigger;
                                            tempobj.Value = manTime.Value;
                                            tempobj.start = manenddt.AddMinutes(-manenddt.TimeOfDay.TotalMinutes);
                                            tempobj.end = manenddt;
                                            templist.Add(tempobj);
                                            break;
                                        }
                                        else if (daydiff == manenddt.Subtract(tempsDate).TotalDays)
                                        {
                                            LabBookingExportManualSessSpan tempobj = new LabBookingExportManualSessSpan();
                                            tempobj.isActive = manTime.isActive;
                                            tempobj.trigger = manTime.trigger;
                                            tempobj.Value = manTime.Value;
                                            tempobj.start = manstartdt;
                                            tempobj.end = manstartdt.AddDays(1).AddSeconds(-(manstartdt.TimeOfDay.TotalSeconds + 1));
                                            templist.Add(tempobj);
                                            tempsDate = manstartdt.AddDays(1).AddMinutes(-(manstartdt.TimeOfDay.TotalMinutes));
                                        }
                                        else
                                        {
                                            LabBookingExportManualSessSpan tempobj = new LabBookingExportManualSessSpan();
                                            tempobj.isActive = manTime.isActive;
                                            tempobj.trigger = manTime.trigger;
                                            tempobj.Value = manTime.Value;
                                            tempobj.start = new DateTime(manenddt.AddDays(-daydiff).Year, manenddt.AddDays(-daydiff).Month, manenddt.AddDays(-daydiff).Day, 0, 0, 0);
                                            tempobj.end = new DateTime(manenddt.AddDays(-daydiff).Year, manenddt.AddDays(-daydiff).Month, manenddt.AddDays(-daydiff).Day, 23, 59, 59);
                                            templist.Add(tempobj);
                                            tempsDate = new DateTime(manenddt.AddDays(-daydiff).Year, manenddt.AddDays(-daydiff).Month, manenddt.AddDays(-daydiff).Day, 23, 59, 59).AddSeconds(1);
                                        }
                                        daydiff--;
                                    }
                                    while (daydiffer > 0);
                                }
                                if (dayChangeFlag)
                                {
                                    foreach (LabBookingExportManualSessSpan manspan in templist)
                                    {
                                        if (manspan.end.Date < StartDate.Date || manspan.start.Date > EndDate.Date)
                                            continue;

                                        //tempModelObj.LCManualTestTotalSpan += LCreportClass.UnixTimeStampsToTimeSpan(span.start, span.end);
                                        tempModelObj.LCManualTestTotalSpan += manspan.end.Subtract(manspan.start);
                                        //tempTSIUObj.LC_TotalManualHours = LCreportClass.UnixTimeStampsToTimeSpan(span.start, span.end);
                                        // tempTSIUObj.LC_TotalManualHours = manspan.end.Subtract(manspan.start);
                                        //tempTSIUObj.endTime = LCreportClass.UnixTimeStampsToDate(span.end);
                                        tempTSIUObj.endTime = manspan.end;
                                        //tempTSIUObj.startTime = LCreportClass.UnixTimeStampsToDate(span.start);
                                        tempTSIUObj.startTime = manspan.start;

                                        if (manspan.Value.ToLower().Contains("canoe") || manspan.Value.ToLower().Contains("canw"))
                                        {
                                            tempTSIUObj.TypeofUsage = "Manual_CAPL";
                                            tempTSIUObj.LC_TotalManualCAPLHours = manspan.end.Subtract(manspan.start);
                                        }
                                        else
                                        {
                                            tempTSIUObj.TypeofUsage = "Manual";
                                            tempTSIUObj.LC_TotalManualHours = manspan.end.Subtract(manspan.start);
                                        }
                                        tempTSIUObj.ID_key = lab.LabID.ToString();

                                        //tempTSIUObj.LC_ProjectName_TSIU = projectname;
                                        //tempTSIUObj.LC_Location = lab.Location + lab.SubLocation;
                                        tempTSIUObj.LC_Location = lab.LabLocation;
                                        //tempTSIUObj.LC_Name = lab.Inventory;
                                        tempTSIUObj.LC_Lab_Type = lab.LabType;
                                        objLabReport.LCTSIUParams.Add(tempTSIUObj);
                                        tempTSIUObj = null;
                                        tempTSIUObj = new TSIU_Controller();
                                    }
                                    templist.Clear();
                                }
                                else
                                {
                                    //tempModelObj.LCManualTestTotalSpan += LCreportClass.UnixTimeStampsToTimeSpan(span.start, span.end);
                                    tempModelObj.LCManualTestTotalSpan += manenddt.Subtract(manstartdt);
                                    //tempTSIUObj.LC_TotalManualHours = LCreportClass.UnixTimeStampsToTimeSpan(span.start, span.end);
                                    //tempTSIUObj.LC_TotalManualHours = manenddt.Subtract(manstartdt);
                                    //tempTSIUObj.endTime = LCreportClass.UnixTimeStampsToDate(span.end);
                                    tempTSIUObj.endTime = manenddt;
                                    //tempTSIUObj.startTime = LCreportClass.UnixTimeStampsToDate(span.start);
                                    tempTSIUObj.startTime = manstartdt;

                                    if (manTime.Value.ToLower().Contains("canoe") || manTime.Value.ToLower().Contains("canw"))
                                    {
                                        tempTSIUObj.TypeofUsage = "Manual_CAPL";
                                        tempTSIUObj.LC_TotalManualCAPLHours = manenddt.Subtract(manstartdt);
                                    }
                                    else
                                    {
                                        tempTSIUObj.TypeofUsage = "Manual";
                                        tempTSIUObj.LC_TotalManualHours = manenddt.Subtract(manstartdt);
                                    }
                                    tempTSIUObj.ID_key = lab.LabID.ToString();

                                    //  tempTSIUObj.LC_ProjectName_TSIU = projectname;
                                    //tempTSIUObj.LC_Location = lab.Location + lab.SubLocation;
                                    tempTSIUObj.LC_Location = lab.LabLocation;
                                    //tempTSIUObj.LC_Name = lab.Inventory;
                                    tempTSIUObj.LC_Lab_Type = lab.LabType;
                                    objLabReport.LCTSIUParams.Add(tempTSIUObj);
                                    tempTSIUObj = null;
                                    tempTSIUObj = new TSIU_Controller();
                                }

                            }

                            #endregion
                        }
                        if (lab.automatedActivities != null && lab.automatedActivities.Count > 0)
                        {
                            #region  code to split for activities more than 24 hours
                            foreach (LCfilterInfo.ExportAutoSessSpan autoTime in lab.automatedActivities)
                            {

                                DateTime autostartdt = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                                autostartdt = autostartdt.AddSeconds(autoTime.start).ToLocalTime();
                                System.DateTime autoenddt = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
                                autoenddt = autoenddt.AddSeconds(autoTime.end).ToLocalTime();


                                if (autoenddt.Date < StartDate.Date || autostartdt.Date > EndDate.Date)
                                    continue;

                                //remove overlapping times on the boundaries of chosen start and end date
                                List<LabBookingExportManualSessSpan> templist = new List<LabBookingExportManualSessSpan>();
                                bool dayChangeFlag = false;
                                if (autostartdt.Day == autoenddt.Day)
                                {
                                    dayChangeFlag = false;
                                }
                                else
                                {
                                    dayChangeFlag = true;
                                }
                                if (!dayChangeFlag)
                                {

                                }
                                else
                                {
                                    double daydiff = autoenddt.Subtract(autostartdt).TotalDays;
                                    DateTime tempsDate = autostartdt;
                                    double daydiffer = autoenddt.Subtract(autostartdt).TotalDays;
                                    do
                                    {
                                        if (autoenddt.Date == tempsDate.Date)
                                        {
                                            LabBookingExportManualSessSpan tempobj = new LabBookingExportManualSessSpan();
                                            tempobj.isActive = autoTime.isActive;
                                            tempobj.trigger = autoTime.trigger;
                                            tempobj.Value = autoTime.Value;
                                            tempobj.start = autoenddt.AddMinutes(-autoenddt.TimeOfDay.TotalMinutes);
                                            tempobj.end = autoenddt;
                                            templist.Add(tempobj);
                                            break;
                                        }
                                        else if (daydiff == autoenddt.Subtract(tempsDate).TotalDays)
                                        {
                                            LabBookingExportManualSessSpan tempobj = new LabBookingExportManualSessSpan();
                                            tempobj.isActive = autoTime.isActive;
                                            tempobj.trigger = autoTime.trigger;
                                            tempobj.Value = autoTime.Value;
                                            tempobj.start = autostartdt;
                                            tempobj.end = autostartdt.AddDays(1).AddSeconds(-(autostartdt.TimeOfDay.TotalSeconds + 1));
                                            templist.Add(tempobj);
                                            tempsDate = autostartdt.AddDays(1).AddMinutes(-(autostartdt.TimeOfDay.TotalMinutes));
                                        }
                                        else
                                        {
                                            LabBookingExportManualSessSpan tempobj = new LabBookingExportManualSessSpan();
                                            tempobj.isActive = autoTime.isActive;
                                            tempobj.trigger = autoTime.trigger;
                                            tempobj.Value = autoTime.Value;
                                            tempobj.start = new DateTime(autoenddt.AddDays(-daydiff).Year, autoenddt.AddDays(-daydiff).Month, autoenddt.AddDays(-daydiff).Day, 0, 0, 0);
                                            tempobj.end = new DateTime(autoenddt.AddDays(-daydiff).Year, autoenddt.AddDays(-daydiff).Month, autoenddt.AddDays(-daydiff).Day, 23, 59, 59);
                                            templist.Add(tempobj);
                                            tempsDate = new DateTime(autoenddt.AddDays(-daydiff).Year, autoenddt.AddDays(-daydiff).Month, autoenddt.AddDays(-daydiff).Day, 23, 59, 59).AddSeconds(1);
                                        }
                                        daydiff--;
                                    }
                                    while (daydiffer > 0);
                                }
                                if (dayChangeFlag)
                                {
                                    foreach (LabBookingExportManualSessSpan autospan in templist)
                                    {
                                        if (autospan.end.Date < StartDate.Date || autospan.start.Date > EndDate.Date)
                                            continue;

                                        //tempModelObj.LCAutomatedTestTotalSpan += LCreportClass.UnixTimeStampsToTimeSpan(span.start, span.end);
                                        tempModelObj.LCAutomatedTestTotalSpan += autospan.end.Subtract(autospan.start);
                                        //tempTSIUObj.LC_AutomatedTotalHours = LCreportClass.UnixTimeStampsToTimeSpan(span.start, span.end);
                                        //tempTSIUObj.LC_AutomatedTotalHours = autospan.end.Subtract(autospan.start);
                                        //tempTSIUObj.endTime = LCreportClass.UnixTimeStampsToDate(span.end);
                                        tempTSIUObj.endTime = autospan.end;
                                        //tempTSIUObj.startTime = LCreportClass.UnixTimeStampsToDate(span.start);
                                        tempTSIUObj.startTime = autospan.start;

                                        if (autospan.Value.ToLower().Contains("canoe") || autospan.Value.ToLower().Contains("canw"))
                                        {
                                            tempTSIUObj.TypeofUsage = "Automated_CAPL";
                                            tempTSIUObj.LC_AutomatedCAPLTotalHours = autospan.end.Subtract(autospan.start);
                                        }
                                        else
                                        {
                                            tempTSIUObj.TypeofUsage = "Automated";
                                            tempTSIUObj.LC_AutomatedTotalHours = autospan.end.Subtract(autospan.start);
                                        }

                                        tempTSIUObj.ID_key = lab.LabID.ToString();
                                        // tempTSIUObj.TypeofUsage = "Automated";
                                        // tempTSIUObj.ID_key = lab.LabID.ToString();

                                        //tempTSIUObj.LC_ProjectName_TSIU = projectname;
                                        //tempTSIUObj.LC_Location = lab.Location + lab.SubLocation;
                                        //tempTSIUObj.LC_Name = lab.Inventory;
                                        tempTSIUObj.LC_Location = lab.LabLocation;
                                        //tempTSIUObj.LC_Name = lab.Inventory;
                                        tempTSIUObj.LC_Lab_Type = lab.LabType;
                                        objLabReport.LCTSIUParams.Add(tempTSIUObj);
                                        tempTSIUObj = null;
                                        tempTSIUObj = new TSIU_Controller();
                                    }
                                    templist.Clear();
                                }
                                else
                                {
                                    //tempModelObj.LCAutomatedTestTotalSpan += LCreportClass.UnixTimeStampsToTimeSpan(span.start, span.end);
                                    tempModelObj.LCAutomatedTestTotalSpan += autoenddt.Subtract(autostartdt);
                                    //tempTSIUObj.LC_AutomatedTotalHours = LCreportClass.UnixTimeStampsToTimeSpan(span.start, span.end);
                                    //tempTSIUObj.LC_AutomatedTotalHours = autoenddt.Subtract(autostartdt);
                                    //tempTSIUObj.endTime = LCreportClass.UnixTimeStampsToDate(span.end);
                                    tempTSIUObj.endTime = autoenddt;
                                    //tempTSIUObj.startTime = LCreportClass.UnixTimeStampsToDate(span.start);
                                    tempTSIUObj.startTime = autostartdt;


                                    if (autoTime.Value.ToLower().Contains("canoe") || autoTime.Value.ToLower().Contains("canw"))
                                    {
                                        tempTSIUObj.TypeofUsage = "Automated_CAPL";
                                        tempTSIUObj.LC_AutomatedCAPLTotalHours = autoenddt.Subtract(autostartdt);
                                    }
                                    else
                                    {
                                        tempTSIUObj.TypeofUsage = "Automated";
                                        tempTSIUObj.LC_AutomatedTotalHours = autoenddt.Subtract(autostartdt);
                                    }

                                    //tempTSIUObj.TypeofUsage = "Automated";
                                    tempTSIUObj.ID_key = lab.LabID.ToString();

                                    //tempTSIUObj.LC_ProjectName_TSIU = projectname;
                                    //tempTSIUObj.LC_Location = lab.Location + lab.SubLocation;
                                    //tempTSIUObj.LC_Name = lab.Inventory;
                                    tempTSIUObj.LC_Location = lab.LabLocation;
                                    //tempTSIUObj.LC_Name = lab.Inventory;
                                    tempTSIUObj.LC_Lab_Type = lab.LabType;
                                    objLabReport.LCTSIUParams.Add(tempTSIUObj);
                                    tempTSIUObj = null;
                                    tempTSIUObj = new TSIU_Controller();
                                }
                            }
                            #endregion
                        }
                        objLabReport.LCParams.Add(tempModelObj);
                        tempModelObj = null;
                        tempModelObj = new Model();
                    }
                }
            }
            catch (Exception ex)
            {
                HomeController.logger.Error(ex, "GetLCData");
                //redirectPage("Error in fetching Lab Activities");
                throw ex;
            }
            return objLabReport.LCTSIUParams;
        }

        public ActionResult ExportTSIUDataToExcel(List<TSIU> excel_obj, string loc, string type, decimal mantotalhrs, decimal autototalhrs, decimal mancapltotalhrs, decimal autocapltotalhrs)
        {
            //time difference in hours : start
            try
            {
                string sdate = excel_obj[0].Startdate;
                string edate = excel_obj[excel_obj.Count - 1].Enddate;
                string filename = @"TSIU_List_" + loc + "_" + type + "_" + sdate + "_to_" + edate + ".xlsx";
                System.Data.DataTable dt = new System.Data.DataTable("TSIU_List");
                dt.Columns.AddRange(new DataColumn[6] { new DataColumn("Date"),
                                              new DataColumn("Day"),
                                            new DataColumn("Start Time"),
                                            new DataColumn("End Time"),
                                            new DataColumn("Time Difference in hours"),
                                            new DataColumn("Type of Usage")});

                if (excel_obj == null)
                {
                    return Json(new { success = false, message = "TSIU Excel Unsuccessful" }, JsonRequestBehavior.AllowGet);
                }

                foreach (TSIU exp in excel_obj)
                {
                    dt.Rows.Add(exp.Startdate, exp.s_Day, exp.startTime, exp.endTime, exp.diffTime, exp.TypeofUsage);

                }
                using (XLWorkbook wb = new XLWorkbook())
                {


                    var ws = wb.Worksheets.Add("TSIU Info");
                    ws.Range("A1:B8").Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    ws.Range("A1:B8").Style.Border.InsideBorder = XLBorderStyleValues.Dotted;
                    ws.Range("A1:B8").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                    ws.Range("A1:B8").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                    ws.Range("A1:B8").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                    ws.Range("A1:B8").Style.Border.TopBorder = XLBorderStyleValues.Thin;
                    ws.Range("A1:B8").Style.Font.Bold = true;
                    ws.Cell(1, 1).Style.Fill.BackgroundColor = XLColor.LightBlue;
                    ws.Cell(2, 1).Style.Fill.BackgroundColor = XLColor.LightBlue;
                    ws.Cell(1, 2).Style.Fill.BackgroundColor = XLColor.LightBlue;
                    ws.Cell(2, 2).Style.Fill.BackgroundColor = XLColor.LightBlue;
                    ws.Cell(3, 1).Style.Fill.BackgroundColor = XLColor.LightBlue;
                    ws.Cell(3, 2).Style.Fill.BackgroundColor = XLColor.LightBlue;
                    ws.Cell(4, 1).Style.Fill.BackgroundColor = XLColor.LightBlue;
                    ws.Cell(4, 2).Style.Fill.BackgroundColor = XLColor.LightBlue;
                    ws.Cell(5, 1).Style.Fill.BackgroundColor = XLColor.LightBlue;
                    ws.Cell(5, 2).Style.Fill.BackgroundColor = XLColor.LightBlue;
                    ws.Cell(6, 1).Style.Fill.BackgroundColor = XLColor.LightBlue;
                    ws.Cell(6, 2).Style.Fill.BackgroundColor = XLColor.LightBlue;
                    ws.Cell(7, 1).Style.Fill.BackgroundColor = XLColor.LightBlue;
                    ws.Cell(7, 2).Style.Fill.BackgroundColor = XLColor.LightBlue;
                    ws.Cell(8, 1).Style.Fill.BackgroundColor = XLColor.LightBlue;
                    ws.Cell(8, 2).Style.Fill.BackgroundColor = XLColor.LightBlue;
                    // ws.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGreen);


                    ws.Cell(1, 1).Value = "Lab Name";
                    ws.Cell(1, 2).Value = excel_obj[0].LC_Name;
                    //ws.Cell(2, 1).Value = "PC Name ";
                    //ws.Cell(2, 2).Value = "";
                    //ws.Cell(3, 1).Value = "Start Date";
                    //ws.Cell(3, 2).Value = excel_obj[0].Startdate;
                    //ws.Cell(4, 1).Value = "End Date";
                    //ws.Cell(4, 2).Value = excel_obj[excel_obj.Count - 1].Enddate;

                    //ws.Cell(5, 1).Value = "Manual Total Hours";
                    //ws.Cell(5, 2).Value = Math.Round(manual, 2);
                    //ws.Cell(6, 1).Value = "Automated Total Hours";
                    //ws.Cell(6, 2).Value = Math.Round(auto, 2);
                    //ws.Cell(7, 1).Value = "Manual CAPL Total Hours";
                    //ws.Cell(7, 2).Value = Math.Round(manualcapl, 2);
                    //ws.Cell(8, 1).Value = "Automated CAPL Total Hours";
                    //ws.Cell(8, 2).Value = Math.Round(automatedcapl, 2);

                    ws.Cell(2, 1).Value = "Start Date";
                    ws.Cell(2, 2).Value = excel_obj[0].Startdate;
                    ws.Cell(3, 1).Value = "End Date";
                    ws.Cell(3, 2).Value = excel_obj[excel_obj.Count - 1].Enddate;

                    ws.Cell(4, 1).Value = "Manual Total Hours";
                    ws.Cell(4, 2).Value = mantotalhrs;
                    ws.Cell(5, 1).Value = "Automated Total Hours";
                    ws.Cell(5, 2).Value = autototalhrs;
                    ws.Cell(6, 1).Value = "Manual CAPL Total Hours";
                    ws.Cell(6, 2).Value = mancapltotalhrs;
                    ws.Cell(7, 1).Value = "Automated CAPL Total Hours";
                    ws.Cell(7, 2).Value = autocapltotalhrs;

                    ws.Cell(10, 1).InsertTable(dt);
                    using (MemoryStream stream = new MemoryStream())
                    {

                        wb.SaveAs(stream);
                        var fileobj = File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
                        return Json(fileobj, JsonRequestBehavior.AllowGet);

                    }
                }
            }

            catch (Exception ex)
            {
                HomeController.logger.Error(ex, "ExportTSIUDataToExcel");
                return Json(new { success = false, message = "TSIU Excel Export Unsuccessful" }, JsonRequestBehavior.AllowGet);
            }

        }

        public TSIUActivity GetLabActivity(int labid, DateTime StartDate, DateTime EndDate)
        {
            TSIUActivity tempObj = new TSIUActivity();
            try
            {

                tempObj.LabID = labid;
                using (BookingServerReplicaEntities db = new BookingServerReplicaEntities())
                {
                    List<Models.Site> objSites = db.Sites.ToList();
                    List<Models.Location> lstLabLocs = db.Locations.ToList();
                    List<Models.LabType> objLabType = db.LabTypes.ToList();
                    List<LabInfo> lstLabDef = db.LabInfoes.ToList();
                    //foreach (LabInfo lab in lstLabDef)
                    //{
                    //    //int lab_index = Array.FindIndex(objLabExport.Labs, row => row.id == lab.Id);
                    //    if (objLabType.First(s => s.DisplayName.Equals(LabType)) && objSites.First(s=>s.Id.Equals(lstLabLocs.First(x=>x.Id))))
                    //    {
                    List<LabComputersPr> labComputers = new List<LabComputersPr>();
                    labComputers = db.LabComputersPrs.Where(x => x.LabId == labid && x.IsActive == true).ToList();
                    if (labComputers != null)
                    {
                        int countManual = 0, countAutomated = 0;
                        //int pccnt = 0;
                        List<GetTActivitiesUpdate_Result> labActivities = new List<GetTActivitiesUpdate_Result>();
                        foreach (var computer in labComputers)
                        {

                            int compid = computer.Id;
                            //List<tactivitiesUpdate> labActivity = new List<tactivitiesUpdate>();
                            //labActivity = db.tactivitiesUpdates.Where(x => x.ComputerId == compid).ToList().Where(items => items.StartDate > (Int32)StartDate.AddDays(-7).Subtract(new DateTime(1970, 1, 1)).TotalSeconds).ToList().Where(items => items.EndDate < (Int32)EndDate.AddDays(7).Subtract(new DateTime(1970, 1, 1)).TotalSeconds).ToList();
                            var actList = db.GetTActivitiesUpdate(StartDate, EndDate, compid);
                            List<GetTActivitiesUpdate_Result> labActivity = actList.ToList();
                            ///to retry in case of timeout
                            ServerTimeout = false;
                            System.Timers.Timer timeout_check = new System.Timers.Timer();
                            timeout_check.Interval = 5000;
                            timeout_check.Elapsed += Timeout_check_Elapsed;
                            timeout_check.Start();

                            while ((labActivity == null) && !ServerTimeout)
                            {
                                actList = db.GetTActivitiesUpdate(StartDate, EndDate, compid);
                                labActivity = actList.ToList();
                                // labActivity = db.tactivitiesUpdates.Where(x => x.ComputerId == compid).ToList().Where(items => items.StartDate > (Int32)StartDate.AddDays(-7).Subtract(new DateTime(1970, 1, 1)).TotalSeconds).ToList().Where(items => items.EndDate < (Int32)EndDate.AddDays(7).Subtract(new DateTime(1970, 1, 1)).TotalSeconds).ToList();
                            }
                            if (labActivity != null)
                            {

                                countManual += labActivity.FindAll(item => item.Type.Equals((int)WrapperActivityType.Manual)).Count + labActivity.FindAll(item => item.Type.Equals((int)WrapperActivityType.ManualTest)).Count;
                                countAutomated += labActivity.FindAll(item => item.Type.Equals((int)WrapperActivityType.AutomatedTest)).Count;
                                foreach (var activity in labActivity)
                                {
                                    labActivities.Add(activity);
                                }
                            }
                            //  pccnt++;

                        }
                        tempObj.manualActivities = new List<ExportManSessSpan>();
                        tempObj.automatedActivities = new List<ExportAutoSessSpan>();
                        foreach (var activity in labActivities)
                        {

                            if ((activity.Type == 1) || (activity.Type == 2))
                            {
                                ExportManSessSpan msp = new ExportManSessSpan();
                                msp.isActive = activity.IsActive;
                                //if (msp.isActive == 1)
                                //    msp.end = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
                                //else
                                msp.end = activity.EndDate;
                                msp.start = activity.StartDate;
                                msp.trigger = activity.Type.ToString();
                                msp.Value = activity.ComputerId.ToString() + "_" + activity.Info + "_" + activity.SessionId;
                                // totalmanhrs += msp.end.Subtract(msp.start).TotalHours;
                                //  tempObj.ManualTotalTime = totalmanhrs;
                                tempObj.manualActivities.Add(msp);

                            }

                            else if (activity.Type == 3)
                            {
                                ExportAutoSessSpan asp = new ExportAutoSessSpan();

                                asp.isActive = activity.IsActive;
                                //if (asp.isActive == 1)
                                //    asp.end = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;

                                //else
                                asp.end = activity.EndDate;
                                asp.start = activity.StartDate;
                                asp.trigger = activity.Type.ToString();
                                asp.Value = activity.ComputerId.ToString() + "_" + activity.Info + "_" + activity.SessionId;
                                //  totalautohrs += asp.end.Subtract(asp.start).TotalHours;
                                // tempObj.ManualTotalTime = totalautohrs;
                                tempObj.automatedActivities.Add(asp);
                            }
                        }


                        LabInfo labInfo = new LabInfo();
                        labInfo = db.LabInfoes.Where(item => item.Id == labid).FirstOrDefault();
                        if (labInfo != null)
                        {
                            tempObj.LabType = db.LabTypes.FirstOrDefault(x => x.Id == labInfo.TypeId).DisplayName;
                        }

                        List<Models.Site> sites = db.Sites.ToList();
                        if (sites != null)
                        {
                            tempObj.LabLocation = sites.First(x => x.Id == db.Locations.First(loc => loc.Id == labInfo.LocationId).SiteId).RbCode;

                        }

                    }
                    //}

                    //}
                }


            }
            catch (Exception ex)
            {
                HomeController.logger.Error(ex, "GetLabActivity");
                throw ex;
                //redirectPage("Error in fetching Lab Activities");

            }

            return tempObj;
        }

        private void Timeout_check_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {

            ServerTimeout = true;
            timeout_check.Stop();

        }

        //#region Helper functions
        ///// <summary>
        ///// To help in List.Exist function to avoid duplicates in list
        ///// </summary>
        ///// <param name="lab"></param>
        ///// <returns></returns>
        //private static Predicate<SelectListItem> CheckNameAndId(string key, DashBoardActivity lab)
        //{
        //    if (key.Equals("labid"))
        //        return x => x.Text == lab.LabID.ToString() && x.Value == lab.LabID.ToString();
        //    else
        //        return null;
        //}
        ///// <summary>
        ///// Function to convert timestamp from UNIX base to DateTime
        ///// </summary>
        ///// <param name="start">Start Date in UNIX</param>
        ///// <param name="end">End Date in UNIX</param>
        ///// <returns></returns>
        static double GetUtcOffsetFromcountryCode(string countryCode)
        {
            switch (countryCode.ToUpper())
            {
                case "AU": return 10;
                case "CN": return 8;
                case "DE": return 1.0;
                case "FR": return 1.0;
                case "IN": return 5.5;
                case "JP": return 9;
                case "MX": return -6.0;
                case "PT": return 0.0;
                case "US": return -5.0;
                case "VN": return 7.0;
                // To be completed with all known Countries
                default: return 0.0;
            }
        }


        //private void convertcsvtojson(string csvpath, string jsonpath)
        //{
        //    try
        //    {
        //        var csv = new List<laboemjson>(); // or, List<YourClass>
        //        var lines = System.IO.File.ReadAllLines(csvpath);

        //        foreach (string line in lines)
        //            csv.Add(new laboemjson { LABID = line.Split(',')[0], OEM = line.Split(',')[1], RESP = line.Split(',')[2] }); // or, populate YourClass          
        //        string json = new
        //            System.Web.Script.Serialization.JavaScriptSerializer().Serialize(csv); // revisit use of lib

        //        System.IO.File.WriteAllText(jsonpath, json);
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}
        //#endregion

    }
    public class DashBoardActivity
    {

        public List<ExportManSessSpan> manualActivities;
        public List<ExportAutoSessSpan> automatedActivities;
        public int LabID { get; set; }
        public string LabLocation { get; set; }
        public string LabType { get; set; }
        public string Startdate { get; set; }
        public string Inventory { get; set; }
        public string s_day { get; set; }

    }

    public class ChartInfo
    {


        public List<string> ChartLocation { get; set; }
        public List<string> ChartLabType { get; set; }
        public List<int> SiteID { get; set; }
    }


    public class TSOUExcel
    {
        private string explabtype;
        private string explocation;

        private string inventoryField;

        private double sumField;
        private int idField;
        private string startDateField;
        private string endDateField;
        private string oemField;


        public string OEM
        {
            get { return this.oemField; }
            set { this.oemField = value; }
        }


        public string StartDate
        {
            get { return this.startDateField; }
            set { this.startDateField = value; }
        }
        public string EndDate
        {
            get { return this.endDateField; }
            set { this.endDateField = value; }
        }



        public string Inventory
        {
            get { return this.inventoryField; }
            set { this.inventoryField = value; }
        }



        public string Model
        {
            get { return this.explabtype; }
            set { this.explabtype = value; }
        }

        public string Location
        {
            get { return this.explocation; }
            set { this.explocation = value; }
        }

        public double ManualTotalHours { get; set; }
        public double AutomatedTotalHours { get; set; }
        public double ManualCaplTotalHours { get; set; }
        public double AutomatedCaplTotalHours { get; set; }



        //sum of manual and auto
        public double TotalSum
        {
            get { return this.sumField; }
            set { this.sumField = value; }
        }
        public int id
        {
            get { return this.idField; }
            set { this.idField = value; }
        }


    }


    public class TSIU
    {
        private string startTimeField;
        private string endTimeField;
        private string TypeofUsageField;
        private int ID_Field;
        private string LC_NameField;
        private string LC_LocationField;
        private string LC_TotalManualHoursField;
        private string LC_TotalAutomatedHoursField;
        private string LC_ProjectNameField;
        private string LC_LabType;
        private string StartDate;
        private string EndDate;
        private string s_day;
        private string e_day;
        private string timediff; //time difference in hours : start

        public string diffTime //time difference in hours : start
        {
            get
            {
                return this.timediff;
            }
            set
            {
                this.timediff = value;
            }
        }

        public string startTime
        {
            get
            {
                return this.startTimeField;
            }
            set
            {
                this.startTimeField = value;
            }
        }
        public string endTime
        {
            get
            {
                return this.endTimeField;
            }
            set
            {
                this.endTimeField = value;
            }
        }
        public string TypeofUsage
        {
            get
            {
                return this.TypeofUsageField;
            }
            set
            {
                this.TypeofUsageField = value;
            }
        }
        public int ID_key
        {
            get
            {
                return this.ID_Field;
            }
            set
            {
                this.ID_Field = value;
            }
        }
        public string LC_Name
        {
            get
            {
                return this.LC_NameField;
            }
            set
            {
                this.LC_NameField = value;
            }
        }
        public string LC_Location
        {
            get
            {
                return this.LC_LocationField;
            }
            set
            {
                this.LC_LocationField = value;
            }
        }
        public string LC_TotalManualHours
        {
            get
            {
                return this.LC_TotalManualHoursField;
            }
            set
            {
                this.LC_TotalManualHoursField = value;
            }
        }
        public string LC_AutomatedTotalHours
        {
            get
            {
                return this.LC_TotalAutomatedHoursField;
            }
            set
            {
                this.LC_TotalAutomatedHoursField = value;
            }
        }

        public string LC_ProjectName_TSIU
        {
            get
            {
                return this.LC_ProjectNameField;
            }

            set
            {
                this.LC_ProjectNameField = value;
            }
        }
        public string LC_Lab_Type
        {
            get
            {
                return this.LC_LabType;
            }

            set
            {
                this.LC_LabType = value;
            }
        }
        public string Startdate
        {
            get
            {
                return this.StartDate;
            }
            set
            {
                this.StartDate = value;
            }
        }

        public string Enddate
        {
            get
            {
                return this.EndDate;
            }
            set
            {
                this.EndDate = value;
            }
        }

        public string s_Day
        {
            get
            {
                return this.s_day;
            }
            set
            {
                this.s_day = value;
            }
        }
        public string e_Day
        {
            get
            {
                return this.e_day;
            }
            set
            {
                this.e_day = value;
            }
        }
    }

    public class ReportParameters_TSIU
    {
        private TimeSpan periodField;

        private List<Model> LCParamsField;


        private List<TSIU_Controller> _LCTSIUField; // if needed
        public TimeSpan Period
        {
            get
            {
                return this.periodField;
            }
            set
            {
                this.periodField = value;
            }
        }
        public List<Model> LCParams
        {
            get
            {
                return this.LCParamsField;
            }
            set
            {
                this.LCParamsField = value;
            }
        }
        public List<TSIU_Controller> LCTSIUParams
        {
            get
            {
                return this._LCTSIUField;
            }
            set
            {
                this._LCTSIUField = value;
            }
        }

    }


    /// <summary>
    /// Model for each LabCar in the reported List
    /// </summary>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    public class Model
    {
        private string lcIdField;

        private string lcNameField;

        private string lcModelField;

        private string lcInventoryField;

        private string lcLocationField;

        private string lcSubLocationField;

        private string lcOwnerField;

        private string lcPcNodeField;

        private TimeSpan lcDefectiveField;

        private TimeSpan lcReservedField;

        private TimeSpan lcManualTestField;

        private TimeSpan lcAutomatedTestField;

        public string LCID
        {
            get
            {
                return this.lcIdField;
            }
            set
            {
                this.lcIdField = value;
            }
        }
        public string LCName
        {
            get
            {
                return this.lcNameField;
            }
            set
            {
                this.lcNameField = value;
            }
        }
        public string LCModel
        {
            get
            {
                return this.lcModelField;
            }
            set
            {
                this.lcModelField = value;
            }
        }
        public string LCInventory
        {
            get
            {
                return this.lcInventoryField;
            }
            set
            {
                this.lcInventoryField = value;
            }
        }
        public string LCLocation
        {
            get
            {
                return this.lcLocationField;
            }
            set
            {
                this.lcLocationField = value;
            }
        }
        public string LCSubLocation
        {
            get
            {
                return this.lcSubLocationField;
            }
            set
            {
                this.lcSubLocationField = value;
            }
        }
        public string LCOwner
        {
            get
            {
                return this.lcOwnerField;
            }
            set
            {
                this.lcOwnerField = value;
            }
        }
        public string LCPCNode
        {
            get
            {
                return this.lcPcNodeField;
            }
            set
            {
                this.lcPcNodeField = value;
            }
        }
        public TimeSpan LCDefectiveTotalSpan
        {
            get
            {
                return this.lcDefectiveField;
            }
            set
            {
                this.lcDefectiveField = value;
            }
        }
        public TimeSpan LCReservedTotalSpan
        {
            get
            {
                return this.lcReservedField;
            }
            set
            {
                this.lcReservedField = value;
            }
        }
        public TimeSpan LCManualTestTotalSpan
        {
            get
            {
                return this.lcManualTestField;
            }
            set
            {
                this.lcManualTestField = value;
            }
        }
        public TimeSpan LCAutomatedTestTotalSpan
        {
            get
            {
                return this.lcAutomatedTestField;
            }
            set
            {
                this.lcAutomatedTestField = value;
            }
        }
    }



    /// <summary>
    /// Template for the TSIU plots
    /// </summary>
    public class TSIU_Controller
    {
        private DateTime startTimeField;
        private DateTime endTimeField;
        private string TypeofUsageField;
        private string ID_Field;
        private string LC_NameField;
        private string LC_LocationField;
        private TimeSpan LC_TotalManualHoursField;
        private TimeSpan LC_TotalAutomatedHoursField;
        private TimeSpan LC_TotalManualCAPLHoursField;
        private TimeSpan LC_TotalAutomatedCAPLHoursField;
        private string LC_ProjectNameField;
        private string LC_LabType;
        private string pcField;
        public DateTime startTime
        {
            get
            {
                return this.startTimeField;
            }
            set
            {
                this.startTimeField = value;
            }
        }
        public DateTime endTime
        {
            get
            {
                return this.endTimeField;
            }
            set
            {
                this.endTimeField = value;
            }
        }
        public string TypeofUsage
        {
            get
            {
                return this.TypeofUsageField;
            }
            set
            {
                this.TypeofUsageField = value;
            }
        }
        public string ID_key
        {
            get
            {
                return this.ID_Field;
            }
            set
            {
                this.ID_Field = value;
            }
        }
        public string LC_Name
        {
            get
            {
                return this.LC_NameField;
            }
            set
            {
                this.LC_NameField = value;
            }
        }
        public string LC_Location
        {
            get
            {
                return this.LC_LocationField;
            }
            set
            {
                this.LC_LocationField = value;
            }
        }
        public TimeSpan LC_TotalManualHours
        {
            get
            {
                return this.LC_TotalManualHoursField;
            }
            set
            {
                this.LC_TotalManualHoursField = value;
            }
        }
        public TimeSpan LC_AutomatedTotalHours
        {
            get
            {
                return this.LC_TotalAutomatedHoursField;
            }
            set
            {
                this.LC_TotalAutomatedHoursField = value;
            }
        }
        public TimeSpan LC_TotalManualCAPLHours
        {
            get
            {
                return this.LC_TotalManualCAPLHoursField;
            }
            set
            {
                this.LC_TotalManualCAPLHoursField = value;
            }
        }
        public TimeSpan LC_AutomatedCAPLTotalHours
        {
            get
            {
                return this.LC_TotalAutomatedCAPLHoursField;
            }
            set
            {
                this.LC_TotalAutomatedCAPLHoursField = value;
            }
        }

        public string LC_ProjectName_TSIU
        {
            get
            {
                return this.LC_ProjectNameField;
            }

            set
            {
                this.LC_ProjectNameField = value;
            }
        }
        public string LC_Lab_Type
        {
            get
            {
                return this.LC_LabType;
            }

            set
            {
                this.LC_LabType = value;
            }
        }
        public string PCName
        {
            get
            {
                return this.pcField;
            }
            set
            {
                this.pcField = value;
            }
        }
    }


}