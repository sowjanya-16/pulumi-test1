using LabBookingWrap;
using LC_Reports_V1.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Newtonsoft.Json;
using ClosedXML.Excel;
using System.IO;
using System.Web.UI.WebControls;
using System.Web.UI;

namespace LC_Reports_V1.Controllers
{
    /// <summary>
    /// Controller class for Diagnostics View
    /// </summary>
    [Authorize(Users = @"apac\din2cob,de\add2abt,de\let2abt,de\ton2abt,apac\jov6cob,apac\mta2cob,apac\muu4cob,apac\gph2hc,APAC\NYB1HC,us\lpr1ga,apac\chb1kor,apac\sat1dz,apac\KUH2YK,apac\KOS2YK,apac\ITO2YK,apac\oig1cob,de\hhr1lr,de\utk4fe,de\dau2abt,de\bji2si,us\stj3ply,de\sth2abt,apac\whe1szh,apac\mae9cob,apac\rma5cob ,de\rud2abt, de\has2abt ")]
    public class DiagnosticsController : Controller
    {
        public static string LabInfoPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/LC_LabInfo.json");
        public static string LabSitesPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/LC_LabSites.json");
        public static string LabLocationsPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/LC_LabLocations.json");
        public List<LabComputersPr> lstPCs = new List<LabComputersPr>();


        /// <summary>
        /// GET: Diagnostics
        /// Function to load the landing page of Diagnostics controller
        /// </summary>       
        public ActionResult DiagnosticsView()
        {
            DiagnosticsParam diagnosticsInfo = new DiagnosticsParam();
            List<WrapperLab> lstLabDef = new List<WrapperLab>();
            List<WrapperSite> objSites = new List<WrapperSite>();
            List<WrapperLocation> lstLabLocs = new List<WrapperLocation>();
            DiagnosticsParam.LabBookingExport objLabExport = new DiagnosticsParam.LabBookingExport();
            lstLabDef = loadJsonObjects_LabID();
            objSites = loadJsonObjects_Sites();
            lstLabLocs = loadJsonObjects_Locations();
            objLabExport = initDataObject();

            //Rb Code corresponding to Sites on ActiveSafety 
            List<string> rbcode_AS = new List<string>()
                { "Abt","Ban","Cob","Ga","Hc","Ply","Szh","Yh"};

            //HW/Project Details filter controls in UI
            if (lstLabDef != null)
            {
                diagnosticsInfo.StartTime = DateTime.Now.AddMonths(-1).Date;
                diagnosticsInfo.EndTime = DateTime.Now.Date;
                diagnosticsInfo.LCLocationName = "NA";
                diagnosticsInfo.LabID = "NA";

                diagnosticsInfo.LabIDs = new List<SelectListItem>();
                lstLabDef = lstLabDef.OrderBy(item => item.DisplayName).ToList();
                foreach (WrapperLab lab in lstLabDef.FindAll(x => x.TypeId == 8 || x.TypeId == 9))
                {
                    diagnosticsInfo.LabIDs.Add(new SelectListItem { Text = lab.DisplayName, Value = lab.Id.ToString() });
                }
                diagnosticsInfo.LabIDs.Sort((a, b) => a.Text.CompareTo(b.Text));


                //diagnosticsInfo.Locations = new List<SelectListItem>();
                //foreach (WrapperSite sitesattr in objSites)
                //{
                //    diagnosticsInfo.Locations.Add(new SelectListItem { Text = sitesattr.DisplayName, Value = sitesattr.RbCode });
                //}
                //diagnosticsInfo.Locations.Sort((a, b) => a.Value.CompareTo(b.Value));
            }

            //LC type filter controls in UI
            if (objSites != null)
            {

                diagnosticsInfo.Sites = new List<SelectListItem>();
                foreach (WrapperSite sitesattr in objSites)
                {
                    if (rbcode_AS.Contains(sitesattr.RbCode))
                    {
                        diagnosticsInfo.Sites.Add(new SelectListItem { Text = sitesattr.DisplayName, Value = sitesattr.RbCode.ToUpper() });
                    }
                }
                diagnosticsInfo.Sites.Sort((a, b) => a.Value.CompareTo(b.Value));
            }

            diagnosticsInfo.objFilterPageExpInfo = new DiagnosticsParam.LabBookingExport();
            diagnosticsInfo.objFilterPageExpInfo = objLabExport;

            ViewBag.Message = "Your Diagnostics Options page.";
            return View(diagnosticsInfo);
        }


        public List<WrapperLab> loadJsonObjects_LabID()
        {
            try
            {
                List<WrapperLab> lstLabDef = new List<WrapperLab>();
                string jsonlabs = System.IO.File.ReadAllText(LabInfoPath);
                jsonlabs = jsonlabs.Replace("\\", string.Empty);
                jsonlabs = jsonlabs.Trim('"');
                lstLabDef = JsonConvert.DeserializeObject<List<WrapperLab>>(jsonlabs);


                return lstLabDef;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public List<WrapperSite> loadJsonObjects_Sites()
        {
            try
            {
                List<WrapperSite> objSites = new List<WrapperSite>();
                string jsonSites = System.IO.File.ReadAllText(LabSitesPath);
                jsonSites = jsonSites.Replace("\\", string.Empty);
                jsonSites = jsonSites.Trim('"');
                objSites = JsonConvert.DeserializeObject<List<WrapperSite>>(jsonSites);
                return objSites;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //Added- 7.6.2021 Monday - for Location Lab ID Filtering
        public List<WrapperLocation> loadJsonObjects_Locations()
        {
            try
            {
                List<WrapperLocation> lstLabLocs = new List<WrapperLocation>();
                string jsonLocs = System.IO.File.ReadAllText(LabLocationsPath);
                jsonLocs = jsonLocs.Replace("\\", string.Empty);
                jsonLocs = jsonLocs.Trim('"');
                lstLabLocs = JsonConvert.DeserializeObject<List<WrapperLocation>>(jsonLocs);

                return lstLabLocs;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// Function to initialise the main data objects for lab information
        /// </summary>
        public DiagnosticsParam.LabBookingExport initDataObject()
        {
            #region Fill main Export Object
            DiagnosticsParam.LabBookingExport objLabExport = new DiagnosticsParam.LabBookingExport();
            List<WrapperLab> lstLabDef = new List<WrapperLab>();
            List<WrapperSite> objSites = new List<WrapperSite>();
            List<WrapperLocation> lstLabLocs = new List<WrapperLocation>();
            lstLabDef = loadJsonObjects_LabID();
            objSites = loadJsonObjects_Sites();
            lstLabLocs = loadJsonObjects_Locations();
            if (objLabExport.Labs == null)
            {
                objLabExport.Labs = new DiagnosticsParam.LabBookingExportLab[lstLabDef.FindAll(x => x.TypeId == 8 || x.TypeId == 9).Count];
                int count = 0;
                foreach (WrapperLab lab in lstLabDef.FindAll(x => x.TypeId == 8 || x.TypeId == 9))
                {

                    objLabExport.Labs[count] = new DiagnosticsParam.LabBookingExportLab();
                    objLabExport.Labs[count].id = (ushort)lab.Id;
                    var temp1 = objSites.FirstOrDefault(site => site.Id == lstLabLocs.First(s => s.Id == lab.LocationId).SiteId);
                    if (temp1 != null)
                        objLabExport.Labs[count].Location = temp1.RbCode;
                    else
                        objLabExport.Labs[count].Location = "NA";

                    objLabExport.Labs[count].name = lab.DisplayName;
                    objLabExport.Labs[count].SubLocation = lab.Description;


                    count++;
                }
                objLabExport.Labs = objLabExport.Labs.OrderBy(item => item.id).ToArray();



            }
            return objLabExport;
            #endregion
        }











        //using LabBookingWrap;
        //using LC_Reports_V1.Models;
        //using System;
        //using System.Collections.Generic;
        //using System.Data;
        //using System.Linq;
        //using System.Web.Mvc;
        //using DevExtreme.AspNet.Data;
        //using DevExtreme.AspNet.Mvc;
        //using Newtonsoft.Json;
        //using ClosedXML.Excel;
        //using System.IO;
        //using System.Web.UI.WebControls;
        //using System.Web.UI;

        //namespace LC_Reports_V1.Controllers
        //{
        //    /// <summary>
        //    /// Controller class for Diagnostics View
        //    /// </summary>
        //    [Authorize(Users = @"apac\din2cob,de\add2abt,de\let2abt,de\ton2abt,apac\mta2cob,apac\muu4cob,apac\gph2hc,APAC\NYB1HC,us\lpr1ga,apac\chb1kor,apac\sat1dz,apac\KUH2YK,apac\KOS2YK,apac\ITO2YK,apac\oig1cob,de\hhr1lr,de\utk4fe,de\dau2abt,de\bji2si,us\stj3ply,de\sth2abt,apac\whe1szh,apac\mae9cob")]
        //    public class DiagnosticsController : Controller
        //    {
        //        public static string LabInfoPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/LC_LabInfo.json");
        //        public static string LabSitesPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/LC_LabSites.json");
        //        public List<string> lstPCs = new List<string>();


        //        /// <summary>
        //        /// GET: Diagnostics
        //        /// Function to load the landing page of Diagnostics controller
        //        /// </summary>       
        //        public ActionResult DiagnosticsView()
        //        {
        //            DiagnosticsParam diagnosticsInfo = new DiagnosticsParam();
        //            List<WrapperLab> lstLabDef = new List<WrapperLab>();
        //            List<WrapperSite> objSites = new List<WrapperSite>();
        //            lstLabDef = loadJsonObjects_LabID();
        //            objSites = loadJsonObjects_Sites();


        //            //Rb Code corresponding to Sites on ActiveSafety 
        //            List<string> rbcode_AS = new List<string>()
        //                { "Abt","Ban","Cob","Ga","Hc","Ply","Szh","Yh"};

        //            //HW/Project Details filter controls in UI
        //            if (lstLabDef != null)
        //            {
        //                diagnosticsInfo.StartTime = DateTime.Now.AddMonths(-1).Date;
        //                diagnosticsInfo.EndTime = DateTime.Now.Date;
        //                diagnosticsInfo.LabID = "NA";

        //                diagnosticsInfo.LabIDs = new List<SelectListItem>();
        //                lstLabDef = lstLabDef.OrderBy(item => item.DisplayName).ToList();
        //                foreach (WrapperLab lab in lstLabDef.FindAll(x => x.TypeId == 8 || x.TypeId == 9))
        //                {
        //                    diagnosticsInfo.LabIDs.Add(new SelectListItem { Text = lab.DisplayName, Value = lab.Id.ToString() });
        //                }
        //                diagnosticsInfo.LabIDs.Sort((a, b) => a.Text.CompareTo(b.Text));
        //            }

        //            //LC type filter controls in UI
        //            if (objSites != null)
        //            {
        //                diagnosticsInfo.Sites = new List<SelectListItem>();
        //                foreach (WrapperSite sitesattr in objSites)
        //                {
        //                    if (rbcode_AS.Contains(sitesattr.RbCode))
        //                    {
        //                        diagnosticsInfo.Sites.Add(new SelectListItem { Text = sitesattr.DisplayName, Value = sitesattr.RbCode.ToUpper() });
        //                    }
        //                }
        //                diagnosticsInfo.Sites.Sort((a, b) => a.Value.CompareTo(b.Value));
        //            }
        //            ViewBag.Message = "Your Diagnostics Options page.";
        //            return View(diagnosticsInfo);
        //        }


        //        public List<WrapperLab> loadJsonObjects_LabID()
        //        {
        //            try
        //            {
        //                List<WrapperLab> lstLabDef = new List<WrapperLab>();
        //                string jsonlabs = System.IO.File.ReadAllText(LabInfoPath);
        //                jsonlabs = jsonlabs.Replace("\\", string.Empty);
        //                jsonlabs = jsonlabs.Trim('"');
        //                lstLabDef = JsonConvert.DeserializeObject<List<WrapperLab>>(jsonlabs);

        //                return lstLabDef;
        //            }
        //            catch (Exception ex)
        //            {
        //                return null;
        //            }
        //        }
        //        public List<WrapperSite> loadJsonObjects_Sites()
        //        {
        //            try
        //            {
        //                List<WrapperSite> objSites = new List<WrapperSite>();
        //                string jsonSites = System.IO.File.ReadAllText(LabSitesPath);
        //                jsonSites = jsonSites.Replace("\\", string.Empty);
        //                jsonSites = jsonSites.Trim('"');
        //                objSites = JsonConvert.DeserializeObject<List<WrapperSite>>(jsonSites);
        //                return objSites;
        //            }
        //            catch (Exception ex)
        //            {
        //                return null;
        //            }
        //        }


        /// <summary>
        /// Function to fetch the PC names from LabIDs using Lab Booking API
        /// /// <param name="labid_to_pcname"></param>
        /// </summary>
        [HttpPost]
        public ActionResult GetPCnamebyLabID(int[] labid_to_pcname)
        {
            List<LabComputersPr> comp = new List<LabComputersPr>();
            try
            {
                //string apistatus = LabBookingWrapper.APIInit("tracker");
                //if (apistatus.Contains("SUCCESS"))
                //{
                //    foreach (var id in labid_to_pcname)
                //    {
                //        WrapperComputers wrapperComputers = LabBookingWrapper.GetComputersByLab(id);

                //        if (wrapperComputers != null)
                //        {
                //            if (wrapperComputers.successMsg.Contains("SUCCESS"))
                //            {
                //                foreach (WrapperComputer computer in wrapperComputers.ComputersList)
                //                {
                //                    var comp = computer.FQDN.Split('.')[0];
                //                    lstPCs.Add(comp);


                //                }
                //            }
                //        }

                //    }
                //}


                using (BookingServerReplicaEntities db1 = new BookingServerReplicaEntities())
                {
                    foreach (var id in labid_to_pcname)
                    {
                        //var comp = db1.LabComputersPrs.FirstOrDefault(x => x.LabId == id).FQDN;
                        //var compname = comp.Split('.')[0].ToUpper();

                        comp = db1.LabComputersPrs.Where(x => x.LabId == id).ToList();
                        foreach (var info in comp)
                            lstPCs.Add(info);
                    }


                }
            }
            catch (Exception ex)
            {
                return View("Unable to fetch data from Lab Booking API. Please try again later!!");
            }
            finally { LabBookingWrapper.APIDispose(); }
            return Json(new { data = lstPCs/*lstPCs.ToArray()*/ }, JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// Functions to fetch the LC type Location-wise Monthly usage Stats
        /// /// /// <param name="location_to_rbcode"></param>
        /// </summary>
        [HttpPost]
        public ActionResult GetYoY_LCTypeData(string[] location_to_rbcode)
        {
            try
            {

                using (PromasterImport_HWInfoEntities db = new PromasterImport_HWInfoEntities())
                {
                    //Monthly and Location-based split
                    DiagnosticsData diagnosticsdata = new DiagnosticsData()
                    {
                        LC_Counts = new List<LC_Count>()
                    };

                    //8.2.2021 Monday- changed to generate excel data for the stats of distinct PCs used for LCType ET only, CCSIL only and ET CCSIL both

                    if (location_to_rbcode != null && location_to_rbcode.Count() > 0)
                    {

                        string location_chosen2;

                        foreach (var location_chosen in location_to_rbcode)
                        {
                            //List<UniquePC_LCTypecount> listccsil_count_2020months_distinctpcs = new List<UniquePC_LCTypecount>();

                            string datestr;
                            int dateyr, datemonth;
                            int et_cnt_distinctPCs_allyrs, ccsil_cnt_distinctPCs_allyrs, et_ccsil_cnt_distinctPCs_allyrs;

                            int et_cnt, ccsil_cnt, etandccsil_cnt;

                            int etcount_yr, ccsilcount_yr;

                            for (int cntyr = 3; cntyr > 0; cntyr--)
                            {
                                for (int cnt = 0; cnt < 12; cnt++)
                                {
                                    dateyr = DateTime.Now.AddYears(-cntyr).AddMonths(cnt).Year;
                                    datemonth = DateTime.Now.AddYears(-cntyr).AddMonths(cnt).Month;
                                    if (datemonth < 10)
                                    {
                                        datestr = dateyr.ToString() + "-" + "0" + datemonth.ToString();
                                    }
                                    else
                                    {
                                        datestr = dateyr.ToString() + "-" + datemonth.ToString();
                                    }
                                    if (location_chosen == "BAN")
                                    {
                                        string location_chosen3 = "KOR";
                                        location_chosen2 = "BMH";

                                        et_cnt_distinctPCs_allyrs = (from item in db.Hardware_Desc_Pr where item.LabCarType.Contains("ET") && (item.System_name.StartsWith(location_chosen) || item.System_name.StartsWith(location_chosen2) || item.System_name.StartsWith(location_chosen3)) && ((item.Date.Contains(datestr))) orderby item.Date select item.System_name).Distinct().Count();
                                        ccsil_cnt_distinctPCs_allyrs = (from item in db.Hardware_Desc_Pr where item.LabCarType.Contains("CCSIL") && (item.System_name.StartsWith(location_chosen) || item.System_name.StartsWith(location_chosen2) || item.System_name.StartsWith(location_chosen3)) && ((item.Date.Contains(datestr))) orderby item.Date select item.System_name).Distinct().Count();
                                        et_ccsil_cnt_distinctPCs_allyrs = (from item in db.Hardware_Desc_Pr where (item.LabCarType.Contains("ET") || item.LabCarType.Contains("CCSIL")) && (item.System_name.StartsWith(location_chosen) || item.System_name.StartsWith(location_chosen2) || item.System_name.StartsWith(location_chosen3)) && ((item.Date.Contains(datestr))) orderby item.Date select item.System_name).Distinct().Count();

                                        et_cnt = (from item in db.Hardware_Desc_Pr orderby item.Date where item.LabCarType.Contains("ET") && (item.System_name.StartsWith(location_chosen) || item.System_name.StartsWith(location_chosen2) || item.System_name.StartsWith(location_chosen3)) && (item.Date.Contains(datestr)) orderby item.Date select item.System_name).Count();
                                        ccsil_cnt = (from item in db.Hardware_Desc_Pr orderby item.Date where item.LabCarType.Contains("CCSIL") && (item.System_name.StartsWith(location_chosen) || item.System_name.StartsWith(location_chosen2) || item.System_name.StartsWith(location_chosen3)) && (item.Date.Contains(datestr)) orderby item.Date select item.System_name).Count();
                                        etandccsil_cnt = (from item in db.Hardware_Desc_Pr orderby item.Date where (item.LabCarType.Contains("ET") || item.LabCarType.Contains("CCSIL")) && (item.System_name.StartsWith(location_chosen) || item.System_name.StartsWith(location_chosen2) || item.System_name.StartsWith(location_chosen3)) && (item.Date.Contains(datestr)) orderby item.Date select item.System_name).Count();

                                        etcount_yr = (from item in db.Hardware_Desc_Pr orderby item.Date where item.LabCarType.Contains("ET") && (item.System_name.StartsWith(location_chosen) || item.System_name.StartsWith(location_chosen2) || item.System_name.StartsWith(location_chosen3)) && (item.Date.Contains(dateyr.ToString())) orderby item.Date select item.System_name).Count();
                                        ccsilcount_yr = (from item in db.Hardware_Desc_Pr orderby item.Date where item.LabCarType.Contains("CCSIL") && (item.System_name.StartsWith(location_chosen) || item.System_name.StartsWith(location_chosen2) || item.System_name.StartsWith(location_chosen3)) && (item.Date.Contains(dateyr.ToString())) orderby item.Date select item.System_name).Count();

                                    }
                                    else if (location_chosen == "SZH")
                                    {
                                        location_chosen2 = "SGHZ";

                                        et_cnt_distinctPCs_allyrs = (from item in db.Hardware_Desc_Pr where item.LabCarType.Contains("ET") && (item.System_name.StartsWith(location_chosen) || item.System_name.StartsWith(location_chosen2)) && ((item.Date.Contains(datestr))) orderby item.Date select item.System_name).Distinct().Count();
                                        ccsil_cnt_distinctPCs_allyrs = (from item in db.Hardware_Desc_Pr where item.LabCarType.Contains("CCSIL") && (item.System_name.StartsWith(location_chosen) || item.System_name.StartsWith(location_chosen2)) && ((item.Date.Contains(datestr))) orderby item.Date select item.System_name).Distinct().Count();
                                        et_ccsil_cnt_distinctPCs_allyrs = (from item in db.Hardware_Desc_Pr where (item.LabCarType.Contains("ET") || item.LabCarType.Contains("CCSIL")) && (item.System_name.StartsWith(location_chosen) || item.System_name.StartsWith(location_chosen2)) && ((item.Date.Contains(datestr))) orderby item.Date select item.System_name).Distinct().Count();

                                        et_cnt = (from item in db.Hardware_Desc_Pr orderby item.Date where item.LabCarType.Contains("ET") && (item.System_name.StartsWith(location_chosen) || item.System_name.StartsWith(location_chosen2)) && (item.Date.Contains(datestr)) orderby item.Date select item.System_name).Count();
                                        ccsil_cnt = (from item in db.Hardware_Desc_Pr orderby item.Date where item.LabCarType.Contains("CCSIL") && (item.System_name.StartsWith(location_chosen) || item.System_name.StartsWith(location_chosen2)) && (item.Date.Contains(datestr)) orderby item.Date select item.System_name).Count();
                                        etandccsil_cnt = (from item in db.Hardware_Desc_Pr orderby item.Date where (item.LabCarType.Contains("ET") || item.LabCarType.Contains("CCSIL")) && (item.System_name.StartsWith(location_chosen) || item.System_name.StartsWith(location_chosen2)) && (item.Date.Contains(datestr)) orderby item.Date select item.System_name).Count();

                                        etcount_yr = (from item in db.Hardware_Desc_Pr orderby item.Date where item.LabCarType.Contains("ET") && (item.System_name.StartsWith(location_chosen) || item.System_name.StartsWith(location_chosen2)) && (item.Date.Contains(dateyr.ToString())) orderby item.Date select item.System_name).Count();
                                        ccsilcount_yr = (from item in db.Hardware_Desc_Pr orderby item.Date where item.LabCarType.Contains("CCSIL") && (item.System_name.StartsWith(location_chosen) || item.System_name.StartsWith(location_chosen2)) && (item.Date.Contains(dateyr.ToString())) orderby item.Date select item.System_name).Count();


                                    }
                                    else
                                    {

                                        et_cnt_distinctPCs_allyrs = (from item in db.Hardware_Desc_Pr where item.LabCarType.Contains("ET") && item.System_name.StartsWith(location_chosen) && ((item.Date.Contains(datestr))) orderby item.Date select item.System_name).Distinct().Count();
                                        ccsil_cnt_distinctPCs_allyrs = (from item in db.Hardware_Desc_Pr where item.LabCarType.Contains("CCSIL") && item.System_name.StartsWith(location_chosen) && ((item.Date.Contains(datestr))) orderby item.Date select item.System_name).Distinct().Count();
                                        et_ccsil_cnt_distinctPCs_allyrs = (from item in db.Hardware_Desc_Pr where (item.LabCarType.Contains("ET") || item.LabCarType.Contains("CCSIL")) && item.System_name.StartsWith(location_chosen) && ((item.Date.Contains(datestr))) orderby item.Date select item.System_name).Distinct().Count();

                                        et_cnt = (from item in db.Hardware_Desc_Pr orderby item.Date where item.LabCarType.Contains("ET") && item.System_name.StartsWith(location_chosen) && (item.Date.Contains(datestr)) orderby item.Date select item.System_name).Count();
                                        ccsil_cnt = (from item in db.Hardware_Desc_Pr orderby item.Date where item.LabCarType.Contains("CCSIL") && item.System_name.StartsWith(location_chosen) && (item.Date.Contains(datestr)) orderby item.Date select item.System_name).Count();
                                        etandccsil_cnt = (from item in db.Hardware_Desc_Pr orderby item.Date where (item.LabCarType.Contains("ET") || item.LabCarType.Contains("CCSIL")) && item.System_name.StartsWith(location_chosen) && (item.Date.Contains(datestr)) orderby item.Date select item.System_name).Count();

                                        etcount_yr = (from item in db.Hardware_Desc_Pr orderby item.Date where item.LabCarType.Contains("ET") && item.System_name.StartsWith(location_chosen) && (item.Date.Contains(dateyr.ToString())) orderby item.Date select item.System_name).Count();
                                        ccsilcount_yr = (from item in db.Hardware_Desc_Pr orderby item.Date where item.LabCarType.Contains("CCSIL") && item.System_name.StartsWith(location_chosen) && (item.Date.Contains(dateyr.ToString())) orderby item.Date select item.System_name).Count();

                                        //TO GET PC PROJECT MAPPING
                                        //var et_project_instances = ((from item in db.Hardware_Desc_Pr orderby item.EEPName where item.LabCarType.Contains("ET") && item.System_name.StartsWith(location_chosen) && (item.Date.Contains(datestr)) orderby item.EEPName select new { Project = item.EEPName, PC = item.System_name } /*select item.EEPName*/).ToList());
                                        //var ccsil_project_instances = ((from item in db.Hardware_Desc_Pr orderby item.EEPName where item.LabCarType.Contains("CCSIL") && item.System_name.StartsWith(location_chosen) && (item.Date.Contains(datestr)) orderby item.EEPName select new { Project = item.EEPName, PC = item.System_name } /*select item.EEPName*/).ToList());
                                        //et_list.Clear();
                                        //ccsil_list.Clear();
                                        //foreach (var project in et_project_instances)
                                        //{
                                        //    et_list.Add(project);
                                        //}

                                        //foreach (var project in ccsil_project_instances)
                                        //{
                                        //    ccsil_list.Add(project);
                                        //}


                                    }
                                    //TO GET PC PROJECT MAPPING
                                    //var et_project_details = et_list.GroupBy(x => x).Select(x => new { key = x.Key, val = x.Count() });
                                    //var ccsil_project_details = ccsil_list.GroupBy(x => x).Select(x => new { key = x.Key, val = x.Count() });
                                    ////Dictionary<object, int> et_project_details1 = new Dictionary<object, int>();
                                    ////Dictionary<object, int> ccsil_project_details1 = new Dictionary<object, int>();

                                    //List<Project_pc_distinct> et_project_details1 = new List<Project_pc_distinct>();
                                    //List<Project_pc_distinct> ccsil_project_details1 = new List<Project_pc_distinct>();
                                    //foreach (var proj in et_project_details)
                                    //{
                                    //    //int countInstance = proj.val;

                                    //    Project_pc_distinct obj = new Project_pc_distinct();
                                    //    obj.project = proj.key.Project;
                                    //    obj.PC = proj.key.PC;
                                    //    obj.proj_instance_count = proj.val;
                                    //    et_project_details1.Add(obj/*, countInstance*/);
                                    //}
                                    //foreach (var proj in ccsil_project_details)
                                    //{

                                    //    //int countInstance = proj.val;

                                    //    Project_pc_distinct obj = new Project_pc_distinct();
                                    //    obj.project = proj.key.Project;
                                    //    obj.PC = proj.key.PC;
                                    //    obj.proj_instance_count = proj.val;
                                    //    ccsil_project_details1.Add(obj/*, countInstance*/);
                                    //}
                                    ////dynamic type- automatic typecasting not working; created a temp class 
                                    //diagnosticsdata./*LC_Counts.*/Add(new DiagnosticsData/*LC_Count*/ { Month = datestr, Year = dateyr.ToString(), ETcnt_distinctpcs = et_cnt_distinctPCs_allyrs, CCSILcnt_distinctpcs = ccsil_cnt_distinctPCs_allyrs, ETCCSILcnt_of_distinctPCs = et_ccsil_cnt_distinctPCs_allyrs, ETcnt_of_pc_instances = et_cnt, CCSILcnt_of_pc_instances = ccsil_cnt, et_cnt_yr = etcount_yr, ccsil_cnt_yr = ccsilcount_yr, Month_Location = datestr + " " + location_chosen, Location = location_chosen, ET_Project_Details = et_project_details1, CCSIL_Project_Details = ccsil_project_details1 });




                                    //listccsil_count_2020months_distinctpcs.Add(new UniquePC_LCTypecount { Month = datestr, Year = dateyr.ToString(), Countet_distinctpcs = et_cnt_distinctPCs_allyrs, Countccsil_distinctpcs = ccsil_cnt_distinctPCs_allyrs, Countet_ccsil_distinctpcs = et_ccsil_cnt_distinctPCs_allyrs, Countet_pcinstances = et_cnt, Countccsil_pcinstances = ccsil_cnt, et_cnt_eachYr = etcount_yr, ccsil_cnt_eachYr = ccsilcount_yr });
                                    diagnosticsdata.LC_Counts.Add(new LC_Count { Month = datestr, Year = dateyr.ToString(), ETcnt_distinctpcs = et_cnt_distinctPCs_allyrs, CCSILcnt_distinctpcs = ccsil_cnt_distinctPCs_allyrs, ETCCSILcnt_of_distinctPCs = et_ccsil_cnt_distinctPCs_allyrs, ETcnt_of_pc_instances = et_cnt, CCSILcnt_of_pc_instances = ccsil_cnt, et_cnt_yr = etcount_yr, ccsil_cnt_yr = ccsilcount_yr, Month_Location = datestr + " " + location_chosen, Location = location_chosen });

                                }
                            }

                            //foreach (var res in listccsil_count_2020months_distinctpcs)
                            //{
                            //    diagnosticsdata.LC_Counts.Add(new LC_Count { Month = res.Month, Year = res.Year, ccsil_cnt_yr = res.ccsil_cnt_eachYr, et_cnt_yr = res.et_cnt_eachYr, CCSILcnt_distinctpcs = res.Countccsil_distinctpcs, ETcnt_distinctpcs = res.Countet_distinctpcs, CCSILcnt_of_pc_instances = res.Countccsil_pcinstances, ETcnt_of_pc_instances = res.Countet_pcinstances, ETCCSILcnt_of_distinctPCs = res.Countet_ccsil_distinctpcs, Location = location_chosen, Month_Location = res.Month + " " + location_chosen });
                            //}

                        }
                    }
                    //TO GET PC PROJECT MAPPING
                    //return Json(new { data = diagnosticsdata/*.LC_Counts*/ }, JsonRequestBehavior.AllowGet);
                    return Json(new { data = diagnosticsdata.LC_Counts }, JsonRequestBehavior.AllowGet);
                }
            }

            catch (Exception Ex)
            {
                return View("Unable to retrieve data from database.Please try again later!!");
            }

        }


        //TO GET PC PROJECT MAPPING
        //[HttpPost]
        //public ActionResult ExportYoYDetailsToExcel(List<DiagnosticsData> excel_obj) //Datatype of the objects returned from view is LC_Count type
        //{
        //    try
        //    {
        //        List<DiagnosticsData> objdata = new List<DiagnosticsData>();//instantiation (creating object) for the DiagnosticsData class
        //        //objdata.LC_Counts = new List<LC_Count>();
        //        //objdata.LC_Counts = excel_obj;//assigning the object returned from view to the LC_Counts object
        //        objdata = excel_obj;
        //        string CCHIL_project_pc_mapping = null;
        //        string filename = @"YoY_Details_" + DateTime.Now.ToShortDateString() /*+ "_" +  ".xlsx"*/;
        //        System.Data.DataTable dt = new System.Data.DataTable("YoY_Details");
        //        dt.Columns.AddRange(new DataColumn[9] {new DataColumn("Month"),
        //                                    new DataColumn("Location"),
        //                                    new DataColumn("CCSIL Project instances",typeof(int)),
        //                                    new DataColumn("CCHIL Project instances",typeof(int)),
        //                                    new DataColumn("Distinct PCs for CCSIL",typeof(int)),
        //                                    new DataColumn("Distinct PCs for CCHIL",typeof(int)),
        //                                    new DataColumn("Total PCs used for Projects",typeof(int)),
        //                                    new DataColumn("Common PCs used for CCHIL & CCSIL",typeof(int)),
        //                                     new DataColumn("Project-PC mapping for CCHIL",typeof(string)),
        //                                     // new DataColumn("Project-PC mapping for CCSIL",typeof(string))
        //                                   });

        //        foreach (var request in objdata)
        //        {
        //            CCHIL_project_pc_mapping = null;
        //            if (request.ET_Project_Details != null)
        //            {
        //                foreach (var project_pc in request.ET_Project_Details)
        //                {
        //                    CCHIL_project_pc_mapping += (project_pc.project + ":" + project_pc.PC + "( " + project_pc.proj_instance_count + "),"); /*(project_pc.project + " run on " + project_pc.PC + " for " + project_pc.proj_instance_count + " times , \n" );*/
        //                }
        //            }

        //            dt.Rows.Add(request.Month, request.Location, request.CCSILcnt_of_pc_instances, request.ETcnt_of_pc_instances, request.CCSILcnt_distinctpcs, request.ETcnt_distinctpcs, request.ETCCSILcnt_of_distinctPCs, request.ETcnt_distinctpcs + request.CCSILcnt_distinctpcs - request.ETCCSILcnt_of_distinctPCs, CCHIL_project_pc_mapping/*request.ET_Project_Details*//*, request.CCSIL_Project_Details*/);
        //        }



        //        using (XLWorkbook wb = new XLWorkbook())
        //        {
        //            wb.Worksheets.Add(dt);
        //            using (MemoryStream stream = new MemoryStream())
        //            {
        //                wb.SaveAs(stream);
        //                var robj = File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
        //                return Json(robj, JsonRequestBehavior.AllowGet);
        //            }
        //        }
        //    }
        //    catch (Exception Ex)
        //    {
        //        return View("Unable to export the YoY Details To Excel now. Please try again later!");
        //    }

        //}



        public ActionResult ExportYoYDetailsToExcel(List<LC_Count> excel_obj) //Datatype of the objects returned from view is LC_Count type
        {
            try
            {
                DiagnosticsData objdata = new DiagnosticsData();//instantiation (creating object) for the DiagnosticsData class
                objdata.LC_Counts = new List<LC_Count>();
                objdata.LC_Counts = excel_obj;//assigning the object returned from view to the LC_Counts object

                string filename = @"YoY_Details_" + DateTime.Now.ToShortDateString();
                System.Data.DataTable dt = new System.Data.DataTable("YoY_Details");
                dt.Columns.AddRange(new DataColumn[8] {new DataColumn("Month"),
                                            new DataColumn("Location"),
                                            new DataColumn("CCSIL - Count of Project runs", typeof(int)),
                                            new DataColumn("CCHIL - Count of Project runs", typeof(int)),
                                            new DataColumn("CCSIL - Count of PCs", typeof(int)),
                                            new DataColumn("CCHIL - Count of PCs", typeof(int)),
                                            new DataColumn("Total - Count of PCs", typeof(int)),
                                            new DataColumn("Common - Count of PCs", typeof(int))
                                           });

                foreach (LC_Count request in objdata.LC_Counts)
                {
                    dt.Rows.Add(request.Month, request.Location, request.CCSILcnt_of_pc_instances, request.ETcnt_of_pc_instances, request.CCSILcnt_distinctpcs, request.ETcnt_distinctpcs, request.ETCCSILcnt_of_distinctPCs, request.ETcnt_distinctpcs + request.CCSILcnt_distinctpcs - request.ETCCSILcnt_of_distinctPCs);
                }



                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dt);
                    using (MemoryStream stream = new MemoryStream())
                    {
                        wb.SaveAs(stream);
                        var robj = File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
                        return Json(robj, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception Ex)
            {
                return View("Unable to export the YoY Details To Excel now. Please try again later!");
            }

        }

        /// <summary>
        /// Functions to fetch the HW Details, Project Details and Component Details using PC name and Dates
        /// /// <param name="pc"></param>
        /// /// <param name="sdate"></param>
        /// /// <param name="edate"></param>
        /// </summary>
        [HttpPost]
        public ActionResult GetTABLE1_HWDescData(List<LabComputersPr> pc, DateTime sdate, DateTime edate)
        {
            List<TABLE1_HwDataView> hwDataViews = new List<TABLE1_HwDataView>();
            if (pc != null && pc.Count() > 0)
            {
                try
                {
                    using (PromasterImport_HWInfoEntities db = new PromasterImport_HWInfoEntities())
                    {
                        db.Database.CommandTimeout = 500;
                        string date = string.Empty;
                        for (var dt = sdate; dt <= edate; dt = dt.AddDays(1))
                        {
                            date = dt.ToString("yyyy-MM-dd");

                            foreach (var pcname1 in pc)
                            {
                                var pcname = pcname1.FQDN.Split('.')[0].ToUpper().Trim();
                                if (pcname != null && pcname != string.Empty && pcname.Count() > 0)
                                {

                                    var hwItems = (from item in db.Hardware_Desc_Pr orderby item.Date where item.System_name.Contains(pcname) && item.Date.Contains(date) select new { item.EEPName, item.Date, item.EEPBuildDate, item.EEPDatabaseVersion, item.LabCarType, item.PCHostName, item.System_name, item.RBCCAFVersion, item.RTPCName, item.RTPCSoftwareVersion, item.ToolChainVersion }).ToList();
                                    //var hwItems_1 = (from item in db.Hardware_Desc_Pr
                                    //               orderby item.Date
                                    //               where item.System_name.Contains(pcname) && item.Date.Contains(date)
                                    //               select item).Distinct().ToList();
                                    //var x = db.Hardware_Desc_Pr.ToList().FindAll(xi=>xi.System_name.Contains(pcname)).FindAll(ci=>ci.Date.Contains(date)).ToList()
                                    //var hwItems_wodistinct_specificcol = (from item in db.Hardware_Desc_Pr orderby item.Date where item.System_name.Contains(pcname) && item.Date.Contains(date) select item.EEPName).ToList();

                                    foreach (var item in hwItems)
                                    {

                                        //var result = (from b in db.Hardware_Desc_Pr
                                        //                  //join h in db.LabTypes on a.TypeId equals h.Id
                                        //                  //join c in db.Locations on a.LocationId equals c.Id
                                        //                  //join s in db.Sites on c.SiteId equals s.Id
                                        //              where b.EEPName == j && b.Date == date
                                        //              orderby b.Date
                                        //              select new
                                        //              {
                                        //                  EEPBuildDate = b.EEPBuildDate,
                                        //                  EEPName = b.EEPName,

                                        //              }).Distinct();


                                        TABLE1_HwDataView obj = new TABLE1_HwDataView();
                                        obj.System_name = item.System_name;

                                        //////string apistatus = LabBookingWrapper.APIInit("tracker");
                                        //////if (apistatus.Contains("SUCCESS"))
                                        //////{
                                        //////    WrapperComputer wrapperComputers = new WrapperComputer();
                                        //////    wrapperComputers = LabBookingWrapper.GetComputerByFQDN(pcname1);
                                        //////    if (wrapperComputers.successMsg.Contains("SUCCESS"))
                                        //////    {
                                        //////        var labid = wrapperComputers.LabId;

                                        //////        WrapperLab wrapperlab = new WrapperLab();
                                        //////        wrapperlab = LabBookingWrapper.GetLabByLabID(labid);
                                        //////        if (wrapperlab.successMsg.Contains("SUCCESS"))
                                        //////        {
                                        //////            var labname = wrapperlab.DisplayName;
                                        //////            obj.LC_Name = labname;
                                        //////        }
                                        //////    }

                                        //////}


                                        using (BookingServerReplicaEntities db1 = new BookingServerReplicaEntities())
                                        {
                                            var chk = db1.LabComputersPrs.ToList();
                                            var chk1 = db1.LabComputersPrs.Select(v => v.FQDN).ToList();

                                            var labid = db1.LabComputersPrs.ToList().Find(x => x.FQDN.Split('.')[0].ToUpper().Contains(obj.System_name)).LabId;//Find(x => x.DisplayName.ToString() == obj.System_name.ToString()).LabId;                                     
                                            obj.LC_Name = db1.LabInfoes.FirstOrDefault(i => i.Id == labid).DisplayName;
                                            //var chkid8 = db1.LabInfoes.FirstOrDefault(i => i.DisplayName == "LC0008").Id;
                                            //var chklabid8 = db1.LabComputersPrs.ToList().Find(x => x.LabId == (chkid8)).FQDN;
                                        }


                                        //////WrapperComputers CompinfoTest = new WrapperComputers();
                                        //////                                    CompinfoTest = LabBookingWrapper.GetComputersByLab(lab.Id);
                                        //////                                    //CompinfoTest.successMsg = string.Empty;

                                        //////                                    if (CompinfoTest.successMsg.Contains("SUCCESS"))
                                        //////                                        foreach (WrapperComputer comp in CompinfoTest.ComputersList)
                                        //////                                        {

                                        //////                                            LabComputersPr c = new LabComputersPr();
                                        //////                                            c.Description = comp.Description;
                                        //////                                            c.DisplayName = comp.DisplayName;
                                        //////                                            c.Id = comp.Id;
                                        //////                                            c.LocationId = comp.LocationId;
                                        //////                                            c.FQDN = comp.FQDN;
                                        //////                                            c.LabId = comp.LabId;
                                        //////                                            c.TrackerConfigId = comp.TrackerConfigId;
                                        //////                                            if (db.LabComputersPr.Where(item => item.Id == comp.Id).Count() == 0)
                                        //////                                            {
                                        //////                                                Console.WriteLine("Adding COMPUTER entry for : " + comp.DisplayName + "-" + comp.Id);
                                        //////                                                db.LabComputersPr.Add(c);
                                        //////                                                db.SaveChanges();
                                        //////                                            }
                                        //////                                            //else if (db.LabComputersPr.Where(item => item.LabId == comp.LabId).First() != c)
                                        //////                                            //{
                                        //////                                            //    Console.WriteLine("Editing LAB entry for : " + c.DisplayName + "-" + c.Id);
                                        //////                                            //    db.Entry(c).State = EntityState.Modified;
                                        //////                                            //    db.SaveChanges();
                                        //////                                            //}
                                        //////                                            //WrapperActivities Actinfo = new WrapperActivities();
                                        //////                                            //Actinfo.successMsg = string.Empty;
                                        //////                                            //Actinfo = LabBookingWrapper.GetActivitiesByComputer(comp.Id, DateTime.Now.AddDays(-10), DateTime.Now);
                                        //////                                            //if (Actinfo.successMsg.Contains("SUCCESS"))
                                        //////                                            //    foreach (WrapperActivity act in Actinfo.ActivitiesList)
                                        //////                                            //    {

                                        //////                                            //        tactivities a = new tactivities();
                                        //////                                            //        a.ComputerId = act.ComputerId;
                                        //////                                            //        a.EndDate = (int)UnixBasedUTCDateConverter.ToUnixTimestamp(act.EndDate);
                                        //////                                            //        a.Expired = act.Expired ? 1 : 0; ;
                                        //////                                            //        a.Info = act.Info;
                                        //////                                            //        a.IsActive = act.IsActive?1:0;
                                        //////                                            //        a.SessionId = act.SessionId;
                                        //////                                            //        a.StartDate = (int)UnixBasedUTCDateConverter.ToUnixTimestamp(act.StartDate);
                                        //////                                            //        a.Type = (int)act.Type;
                                        //////                                            //        if (!(db.tactivities.Where(x => x.ComputerId == a.ComputerId && x.StartDate == a.StartDate && x.EndDate == a.EndDate).Count()>0))
                                        //////                                            //        {
                                        //////                                            //            Console.WriteLine("Adding ACTIVITY entry for : " + act.Type.ToString() + "-" + act.SessionId);
                                        //////                                            //            db.tactivities.Add(a);
                                        //////                                            //            db.SaveChanges();
                                        //////                                            //        }
                                        //////                                            //    }
                                        //////                                        }




                                        obj.PCHostName = item.PCHostName;
                                        obj.Date = DateTime.Parse(item.Date).ToShortDateString();
                                        obj.EEPName = item.EEPName;
                                        obj.EEPBuildDate = DateTime.Parse(item.EEPBuildDate).ToShortDateString();
                                        obj.EEPDatabaseVersion = item.EEPDatabaseVersion;
                                        obj.RTPCName = item.RTPCName;
                                        obj.RBCCAFVersion = item.RBCCAFVersion;
                                        obj.ToolChainVersion = item.ToolChainVersion;
                                        obj.RTPCSoftwareVersion = item.RTPCSoftwareVersion;
                                        obj.LabCarType = item.LabCarType;
                                        hwDataViews.Add(obj);
                                    }
                                }

                            }

                        }
                    }
                    return Json(new { data = hwDataViews }, JsonRequestBehavior.AllowGet);

                }
                catch (Exception Ex)
                {
                    return View("Unable to retrieve data from database. Please try again later!!");
                }
            }
            else
                return Json(new { success = false, message = "Try again" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult TABLE2_GetPrjDescData(List<LabComputersPr> pc, DateTime sdate, DateTime edate)
        {
            List<TABLE2_ProjectDescriptionView> PrjDataViews = new List<TABLE2_ProjectDescriptionView>();
            if (pc != null && pc.Count() > 0)
            {
                try
                {
                    using (PromasterImport_ProjectDescEntities db = new PromasterImport_ProjectDescEntities())
                    {
                        db.Database.CommandTimeout = 500;
                        string date = string.Empty;
                        for (var dt = sdate; dt <= edate; dt = dt.AddDays(1))
                        {
                            date = dt.ToString("yyyy-MM-dd");

                            foreach (var pcname1 in pc)
                            {
                                var pcname = pcname1.FQDN.Split('.')[0].ToUpper().Trim();
                                if (pcname != null && pcname != string.Empty && pcname.Count() > 0)
                                {

                                    var PrjItems = (from item in db.ProjectDescription_Pr_1 orderby item.Date where item.System_name.Contains(pcname) && item.Date.Contains(date) select new { item.AECUCable, item.Ascet_Ver, item.Component_name, item.Date, item.Db_Version, item.Details, item.EMUCable, item.MetaEditor_Ver, item.ProjectBuilder_Ver, item.ProjectEditor_Ver, item.System_name, item.ToolVersion, item.Version });

                                    //var PrjItems_takingUniqueComponent_Details = (from item in db.ProjectDescription_Pr_1 orderby item.Date where item.System_name.Contains(pcname) && item.Date.Contains(date) select new { item.Component_name, item.Details}).Distinct();
                                    //var PrjItems_takingUniqueComponent = (from item in db.ProjectDescription_Pr_1 orderby item.Date where item.System_name.Contains(pcname) && item.Date.Contains(date) select new { item.Component_name}).Distinct().ToList();
                                    //var PrjItems_takingUniqueDetail = (from item in db.ProjectDescription_Pr_1 orderby item.Date where item.System_name.Contains(pcname) && item.Date.Contains(date) select new { item.Details }).Distinct().ToList();



                                    foreach (var item in PrjItems)
                                    {
                                        TABLE2_ProjectDescriptionView obj = new TABLE2_ProjectDescriptionView();

                                        obj.Component_name = item.Component_name;
                                        obj.Date = item.Date;
                                        obj.Db_Version = item.Db_Version;
                                        obj.Details = item.Details;
                                        obj.System_name = item.System_name;

                                        using (BookingServerReplicaEntities db1 = new BookingServerReplicaEntities())
                                        {

                                            var labid = db1.LabComputersPrs.ToList().Find(x => x.FQDN.Split('.')[0].ToUpper().Contains(obj.System_name)).LabId;
                                            obj.LC_Name = db1.LabInfoes.FirstOrDefault(i => i.Id == labid).DisplayName;
                                        }

                                        obj.Version = item.Version;

                                        obj.ToolVersion = item.ToolVersion;
                                        obj.EMUCable = item.EMUCable;
                                        obj.AECUCable = item.AECUCable;
                                        obj.MetaEditor_Ver = item.MetaEditor_Ver;
                                        obj.ProjectBuilder_Ver = item.ProjectBuilder_Ver;
                                        obj.Ascet_Ver = item.Ascet_Ver;
                                        obj.ProjectEditor_Ver = item.ProjectEditor_Ver;


                                        PrjDataViews.Add(obj);
                                    }
                                }
                            }
                        }
                        return Json(new { data = PrjDataViews }, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception Ex)
                {
                    return View("Unable to retrieve data from database. Please try again later!!");
                }
            }
            else
                return Json(new { success = false, message = "Try again" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetTABLE3_APB_BOB_Cable_EB_ES4441_1_ES4441_2_HAP1Data(List<LabComputersPr> pc, DateTime sdate, DateTime edate)
        {
            List<TABLE3_APB_BOB_Cable_EB_ES4441_1_ES4441_2_HAP1View> table3DataViews = new List<TABLE3_APB_BOB_Cable_EB_ES4441_1_ES4441_2_HAP1View>();
            if (pc != null && pc.Count() > 0)
            {
                try
                {
                    using (PromasterImport_HW_CoGrpsEntities db = new PromasterImport_HW_CoGrpsEntities())
                    {
                        db.Database.CommandTimeout = 500;
                        string date = string.Empty;
                        for (var dt = sdate; dt <= edate; dt = dt.AddDays(1))
                        {
                            date = dt.ToString("yyyy-MM-dd");
                            foreach (var pcname1 in pc)
                            {
                                var pcname = pcname1.FQDN.Split('.')[0].ToUpper().Trim();
                                if (pcname != null && pcname != string.Empty && pcname.Count() > 0)
                                {

                                    var apbItems = (from item in db.APB_Pr orderby item.Date where item.System_name.Contains(pcname) && item.Date.Contains(date) select new { item.Date, item.ModelSpecific_EID_Switch_APB_APB_Module_GIO2_Signature_GIO2, item.System_name });
                                    var bobItems = (from item in db.BreakOutBox_Pr orderby item.Date where item.System_name.Contains(pcname) && item.Date.Contains(date) select new { item.Date, item.Kernel_Loadbox_Card_ECC_BreakOutBox_BOB_DeviceFunction, item.Kernel_Loadbox_Card_ECC_BreakOutBox_BOB_DeviceRevision, item.Kernel_Loadbox_Card_ECC_BreakOutBox_BOB_OptionalDescription, item.Kernel_Loadbox_Card_ECC_BreakOutBox_BOB_OrderNumber, item.Kernel_Loadbox_Card_ECC_BreakOutBox_BOB_SerialNumber, item.System_name });
                                    var cableItems = (from item in db.Cable_Pr orderby item.Date where item.System_name.Contains(pcname) && item.Date.Contains(date) select new { item.Date, item.Kernel_Loadbox_Card_ECC_Cable_CABLE_DeviceFunction, item.Kernel_Loadbox_Card_ECC_Cable_CABLE_DeviceRevision, item.Kernel_Loadbox_Card_ECC_Cable_CABLE_OptionalDescription, item.Kernel_Loadbox_Card_ECC_Cable_CABLE_OrderNumber, item.Kernel_Loadbox_Card_ECC_Cable_CABLE_SerialNumber, item.System_name });
                                    var ebItems = (from item in db.EB_Cards_Pr orderby item.Date where item.System_name.Contains(pcname) && item.Date.Contains(date) select new { item.Date, item.Kernel_EB_Cards_EB5100, item.Kernel_EB_Cards_EB5200, item.System_name });
                                    var es4441_1Items = (from item in db.ES4441_1_Pr orderby item.Date where item.System_name.Contains(pcname) && item.Date.Contains(date) select new { item.Date, item.Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441FWVersionMain, item.Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441FWVersionMajor, item.Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441FWVersionMinor, item.Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441PLDVersion, item.Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441SN, item.System_name });
                                    var es4441_2Items = (from item in db.ES4441_2_Pr orderby item.Date where item.System_name.Contains(pcname) && item.Date.Contains(date) select new { item.Date, item.Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441HWRevision, item.Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441HWVersion, item.System_name });
                                    var hap1Items = (from item in db.Harnesadapter_1_Pr orderby item.Date where item.System_name.Contains(pcname) && item.Date.Contains(date) select new { item.Date, item.Kernel_Loadbox_Card_ECC_Harnesadapter_HAP_SerialNumber, item.System_name });


                                    for (int i = 0; i < apbItems.Count(); i++)
                                    {
                                        TABLE3_APB_BOB_Cable_EB_ES4441_1_ES4441_2_HAP1View obj = new TABLE3_APB_BOB_Cable_EB_ES4441_1_ES4441_2_HAP1View();
                                        obj.Date = DateTime.Parse(apbItems.ToArray()[i].Date).ToShortDateString();
                                        obj.System_name = apbItems.ToArray()[i].System_name;

                                        using (BookingServerReplicaEntities db1 = new BookingServerReplicaEntities())
                                        {

                                            var labid = db1.LabComputersPrs.ToList().Find(x => x.FQDN.Split('.')[0].ToUpper().Contains(obj.System_name)).LabId;
                                            obj.LC_Name = db1.LabInfoes.FirstOrDefault(j => j.Id == labid).DisplayName;
                                        }

                                        obj.ModelSpecific_EID_Switch_APB_APB_Module_GIO2_Signature_GIO2 = apbItems.ToArray()[i].ModelSpecific_EID_Switch_APB_APB_Module_GIO2_Signature_GIO2;
                                        obj.Kernel_Loadbox_Card_ECC_Harnesadapter_HAP_SerialNumber = hap1Items.ToArray()[i].Kernel_Loadbox_Card_ECC_Harnesadapter_HAP_SerialNumber;
                                        obj.Kernel_Loadbox_Card_ECC_Cable_CABLE_SerialNumber = cableItems.ToArray()[i].Kernel_Loadbox_Card_ECC_Cable_CABLE_SerialNumber;
                                        obj.Kernel_Loadbox_Card_ECC_Cable_CABLE_DeviceRevision = cableItems.ToArray()[i].Kernel_Loadbox_Card_ECC_Cable_CABLE_DeviceRevision;
                                        obj.Kernel_Loadbox_Card_ECC_BreakOutBox_BOB_SerialNumber = bobItems.ToArray()[i].Kernel_Loadbox_Card_ECC_BreakOutBox_BOB_SerialNumber;
                                        obj.Kernel_Loadbox_Card_ECC_BreakOutBox_BOB_DeviceRevision = bobItems.ToArray()[i].Kernel_Loadbox_Card_ECC_BreakOutBox_BOB_DeviceRevision;
                                        obj.Kernel_EB_Cards_EB5200 = ebItems.ToArray()[i].Kernel_EB_Cards_EB5200;
                                        obj.Kernel_EB_Cards_EB5100 = ebItems.ToArray()[i].Kernel_EB_Cards_EB5100;
                                        obj.Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441SN = es4441_1Items.ToArray()[i].Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441SN;
                                        obj.Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441FWVersionMinor = es4441_1Items.ToArray()[i].Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441FWVersionMinor;
                                        obj.Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441FWVersionMajor = es4441_1Items.ToArray()[i].Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441FWVersionMajor;
                                        obj.Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441FWVersionMain = es4441_1Items.ToArray()[i].Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441FWVersionMain;
                                        obj.Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441HWRevision = es4441_2Items.ToArray()[i].Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441HWRevision;
                                        obj.Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441HWVersion = es4441_2Items.ToArray()[i].Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441HWVersion;

                                        table3DataViews.Add(obj);
                                    }
                                }
                            }
                        }
                        return Json(new { data = table3DataViews }, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception Ex)
                {
                    return View("Unable to retrieve data from database. Please try again later!!");
                }
            }
            else
                return Json(new { success = false, message = "Try again" }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult GetTABLE4_OTSO_BOB1_Cable1_PowerSupply_HAP_IXXATData(List<LabComputersPr> pc, DateTime sdate, DateTime edate)
        {
            List<TABLE4_OTSO_BOB1_Cable1_PowerSupply_HAP_IXXATView> table4DataViews = new List<TABLE4_OTSO_BOB1_Cable1_PowerSupply_HAP_IXXATView>();
            if (pc != null && pc.Count() > 0)
            {
                try
                {
                    using (PromasterImport_HW_CoGrpsEntities db = new PromasterImport_HW_CoGrpsEntities())
                    {
                        db.Database.CommandTimeout = 500;
                        string date = string.Empty;
                        for (var dt = sdate; dt <= edate; dt = dt.AddDays(1))
                        {
                            date = dt.ToString("yyyy-MM-dd");
                            foreach (var pcname1 in pc)
                            {
                                var pcname = pcname1.FQDN.Split('.')[0].ToUpper().Trim();
                                if (pcname != null && pcname != string.Empty && pcname.Count() > 0)
                                {

                                    var otsoItems = (from item in db.OTSO_Pr orderby item.Date where item.System_name.Contains(pcname) && item.Date.Contains(date) select new { item.Date, item.Kernel_Component_IPB_TemperatureReceive_OTSO1Available, item.Kernel_Component_IPB_TemperatureReceive_OTSO2Available, item.System_name });
                                    var bob1Items = (from item in db.BreakOutBox_1_Pr orderby item.Date where item.System_name.Contains(pcname) && item.Date.Contains(date) select new { item.Date, item.Kernel_Loadbox_Card_ECC_BreakOutBox_BOB_SerialNumber, item.System_name });
                                    var cable1Items = (from item in db.Cable_1_Pr orderby item.Date where item.System_name.Contains(pcname) && item.Date.Contains(date) select new { item.Date, item.Kernel_Loadbox_Card_ECC_Cable_CABLE_SerialNumber, item.System_name });
                                    var hapItems = (from item in db.Harnesadapter_Pr orderby item.Date where item.System_name.Contains(pcname) && item.Date.Contains(date) select new { item.Date, item.Kernel_Loadbox_Card_ECC_Harnesadapter_HAP_DeviceFunction, item.Kernel_Loadbox_Card_ECC_Harnesadapter_HAP_DeviceRevision, item.Kernel_Loadbox_Card_ECC_Harnesadapter_HAP_OptionalDescription, item.Kernel_Loadbox_Card_ECC_Harnesadapter_HAP_OrderNumber, item.Kernel_Loadbox_Card_ECC_Harnesadapter_HAP_SerialNumber, item.System_name });
                                    var ixxatItems = (from item in db.IXXAT_Config_Pr orderby item.Date where item.System_name.Contains(pcname) && item.Date.Contains(date) select new { item.Date, item.ModelSpecific_EID_NET_CAN_Settings_CAN_Settings_rtpc_ixxat_config, item.System_name });
                                    var powersupplyItems = (from item in db.Power_Supply_Pr orderby item.Date where item.System_name.Contains(pcname) && item.Date.Contains(date) select new { item.Date, item.Kernel_Loadbox_Card_PSC_Power_Supply_Configuration_PS_Configuration, item.Kernel_Loadbox_Card_PSC_Power_Supply_Configuration_PS_Factor_ECU, item.Kernel_Loadbox_Card_PSC_Power_Supply_Configuration_PS_Factor_MR, item.Kernel_Loadbox_Card_PSC_Power_Supply_Configuration_PS_Factor_VR, item.System_name });


                                    for (int i = 0; i < otsoItems.Count(); i++)
                                    {
                                        TABLE4_OTSO_BOB1_Cable1_PowerSupply_HAP_IXXATView obj = new TABLE4_OTSO_BOB1_Cable1_PowerSupply_HAP_IXXATView();
                                        obj.Date = DateTime.Parse(otsoItems.ToArray()[i].Date).ToShortDateString();
                                        obj.System_name = otsoItems.ToArray()[i].System_name;

                                        using (BookingServerReplicaEntities db1 = new BookingServerReplicaEntities())
                                        {

                                            var labid = db1.LabComputersPrs.ToList().Find(x => x.FQDN.Split('.')[0].ToUpper().Contains(obj.System_name)).LabId;//Find(x => x.DisplayName.ToString() == obj.System_name.ToString()).LabId;
                                            obj.LC_Name = db1.LabInfoes.FirstOrDefault(j => j.Id == labid).DisplayName;
                                        }

                                        obj.Kernel_Component_IPB_TemperatureReceive_OTSO1Available = otsoItems.ToArray()[i].Kernel_Component_IPB_TemperatureReceive_OTSO1Available;
                                        obj.Kernel_Component_IPB_TemperatureReceive_OTSO2Available = otsoItems.ToArray()[i].Kernel_Component_IPB_TemperatureReceive_OTSO2Available;

                                        obj.Kernel_Loadbox_Card_ECC_BreakOutBox_BOB_SerialNumber = bob1Items.ToArray()[i].Kernel_Loadbox_Card_ECC_BreakOutBox_BOB_SerialNumber;
                                        obj.Kernel_Loadbox_Card_ECC_Cable_CABLE_SerialNumber = cable1Items.ToArray()[i].Kernel_Loadbox_Card_ECC_Cable_CABLE_SerialNumber;

                                        obj.Kernel_Loadbox_Card_PSC_Power_Supply_Configuration_PS_Configuration = powersupplyItems.ToArray()[i].Kernel_Loadbox_Card_PSC_Power_Supply_Configuration_PS_Configuration;
                                        obj.Kernel_Loadbox_Card_PSC_Power_Supply_Configuration_PS_Factor_ECU = powersupplyItems.ToArray()[i].Kernel_Loadbox_Card_PSC_Power_Supply_Configuration_PS_Factor_ECU;
                                        obj.Kernel_Loadbox_Card_PSC_Power_Supply_Configuration_PS_Factor_MR = powersupplyItems.ToArray()[i].Kernel_Loadbox_Card_PSC_Power_Supply_Configuration_PS_Factor_MR;
                                        obj.Kernel_Loadbox_Card_PSC_Power_Supply_Configuration_PS_Factor_VR = powersupplyItems.ToArray()[i].Kernel_Loadbox_Card_PSC_Power_Supply_Configuration_PS_Factor_VR;

                                        obj.Kernel_Loadbox_Card_ECC_Harnesadapter_HAP_SerialNumber = hapItems.ToArray()[i].Kernel_Loadbox_Card_ECC_Harnesadapter_HAP_SerialNumber;
                                        obj.Kernel_Loadbox_Card_ECC_Harnesadapter_HAP_DeviceRevision = hapItems.ToArray()[i].Kernel_Loadbox_Card_ECC_Harnesadapter_HAP_DeviceRevision;
                                        obj.ModelSpecific_EID_NET_CAN_Settings_CAN_Settings_rtpc_ixxat_config = ixxatItems.ToArray()[i].ModelSpecific_EID_NET_CAN_Settings_CAN_Settings_rtpc_ixxat_config;
                                        table4DataViews.Add(obj);
                                    }
                                }
                            }
                        }
                        return Json(new { data = table4DataViews }, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception Ex)
                {
                    return View("Unable to retrieve data from database. Please try again later!!");
                }
            }
            else
                return Json(new { success = false, message = "Try again" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetTABLE5_ECC_IBData(List<LabComputersPr> pc, DateTime sdate, DateTime edate)
        {
            List<TABLE5_ECC_IBView> table5DataViews = new List<TABLE5_ECC_IBView>();
            if (pc != null && pc.Count() > 0)
            {
                try
                {
                    using (PromasterImport_HW_CoGrpsEntities db = new PromasterImport_HW_CoGrpsEntities())
                    {
                        db.Database.CommandTimeout = 500;
                        string date = string.Empty;
                        for (var dt = sdate; dt <= edate; dt = dt.AddDays(1))
                        {
                            date = dt.ToString("yyyy-MM-dd");
                            foreach (var pcname1 in pc)
                            {
                                var pcname = pcname1.FQDN.Split('.')[0].ToUpper().Trim();
                                if (pcname != null && pcname != string.Empty && pcname.Count() > 0)
                                {

                                    var eccItems = (from item in db.ECC_Card_Pr orderby item.Date where item.System_name.Contains(pcname) && item.Date.Contains(date) select new { item.Date, item.Kernel_Loadbox_Card_ECC_BuildVersion, item.Kernel_Loadbox_Card_ECC_Calibration_Day, item.Kernel_Loadbox_Card_ECC_Calibration_Month, item.Kernel_Loadbox_Card_ECC_Calibration_Year, item.Kernel_Loadbox_Card_ECC_CARD_BoardRevision, item.Kernel_Loadbox_Card_ECC_CARD_BoardType, item.Kernel_Loadbox_Card_ECC_CARD_Description, item.Kernel_Loadbox_Card_ECC_CARD_VER_TAG_ARRAY, item.Kernel_Loadbox_Card_ECC_Day_of_Creation, item.Kernel_Loadbox_Card_ECC_Firmware_Card_Major, item.Kernel_Loadbox_Card_ECC_Firmware_Card_Minor, item.Kernel_Loadbox_Card_ECC_Firmware_FPGA_Major, item.Kernel_Loadbox_Card_ECC_Firmware_FPGA_Minor, item.Kernel_Loadbox_Card_ECC_Month_of_Creation, item.Kernel_Loadbox_Card_ECC_Next_Calibration_Day, item.Kernel_Loadbox_Card_ECC_Next_Calibration_Month, item.Kernel_Loadbox_Card_ECC_Next_Calibration_Year, item.Kernel_Loadbox_Card_ECC_PowerOnTime, item.Kernel_Loadbox_Card_ECC_RepairCounter, item.Kernel_Loadbox_Card_ECC_Serial_No, item.Kernel_Loadbox_Card_ECC_Year_of_Creation, item.System_name });
                                    var ibItems = (from item in db.IB_Card_Pr orderby item.Date where item.System_name.Contains(pcname) && item.Date.Contains(date) select new { item.Date, item.ModelSpecific_EID_NET_CAN_Settings_CAN_Settings_rtpc_ixxat_config, item.System_name });

                                    for (int i = 0; i < eccItems.Count(); i++)
                                    {
                                        TABLE5_ECC_IBView obj = new TABLE5_ECC_IBView();
                                        obj.Date = DateTime.Parse(eccItems.ToArray()[i].Date).ToShortDateString();
                                        obj.System_name = eccItems.ToArray()[i].System_name;

                                        using (BookingServerReplicaEntities db1 = new BookingServerReplicaEntities())
                                        {

                                            var labid = db1.LabComputersPrs.ToList().Find(x => x.FQDN.Split('.')[0].ToUpper().Contains(obj.System_name)).LabId;//Find(x => x.DisplayName.ToString() == obj.System_name.ToString()).LabId;
                                            LabInfo labInfo = new LabInfo();
                                            //labInfo = db.LabInfoes.Where(item => item.Id == labid).FirstOrDefault();
                                            //if (labInfo != null)
                                            // {
                                            obj.LC_Name = db1.LabInfoes.FirstOrDefault(j => j.Id == labid).DisplayName;
                                        }

                                        db.Database.CommandTimeout = 500;
                                        obj.Kernel_Loadbox_Card_ECC_CARD_BoardType = eccItems.ToArray()[i].Kernel_Loadbox_Card_ECC_CARD_BoardType;
                                        obj.Kernel_Loadbox_Card_ECC_CARD_BoardRevision = eccItems.ToArray()[i].Kernel_Loadbox_Card_ECC_CARD_BoardRevision;
                                        obj.Kernel_Loadbox_Card_ECC_CARD_VER_TAG_ARRAY = eccItems.ToArray()[i].Kernel_Loadbox_Card_ECC_CARD_VER_TAG_ARRAY;
                                        obj.Kernel_Loadbox_Card_ECC_Serial_No = eccItems.ToArray()[i].Kernel_Loadbox_Card_ECC_Serial_No;
                                        obj.Kernel_Loadbox_Card_ECC_Firmware_Card_Major = eccItems.ToArray()[i].Kernel_Loadbox_Card_ECC_Firmware_Card_Major;
                                        obj.Kernel_Loadbox_Card_ECC_Firmware_Card_Minor = eccItems.ToArray()[i].Kernel_Loadbox_Card_ECC_Firmware_Card_Minor;
                                        obj.Kernel_Loadbox_Card_ECC_Firmware_FPGA_Major = eccItems.ToArray()[i].Kernel_Loadbox_Card_ECC_Firmware_FPGA_Major;
                                        obj.Kernel_Loadbox_Card_ECC_Firmware_FPGA_Minor = eccItems.ToArray()[i].Kernel_Loadbox_Card_ECC_Firmware_FPGA_Minor;
                                        obj.Kernel_Loadbox_Card_ECC_Calibration_Day = eccItems.ToArray()[i].Kernel_Loadbox_Card_ECC_Calibration_Day;
                                        obj.Kernel_Loadbox_Card_ECC_Calibration_Month = eccItems.ToArray()[i].Kernel_Loadbox_Card_ECC_Calibration_Month;
                                        obj.Kernel_Loadbox_Card_ECC_Calibration_Year = eccItems.ToArray()[i].Kernel_Loadbox_Card_ECC_Calibration_Year;
                                        obj.Kernel_Loadbox_Card_ECC_Next_Calibration_Day = eccItems.ToArray()[i].Kernel_Loadbox_Card_ECC_Next_Calibration_Day;
                                        obj.Kernel_Loadbox_Card_ECC_Next_Calibration_Month = eccItems.ToArray()[i].Kernel_Loadbox_Card_ECC_Next_Calibration_Month;
                                        obj.Kernel_Loadbox_Card_ECC_Next_Calibration_Year = eccItems.ToArray()[i].Kernel_Loadbox_Card_ECC_Next_Calibration_Year;
                                        obj.Kernel_Loadbox_Card_ECC_Day_of_Creation = eccItems.ToArray()[i].Kernel_Loadbox_Card_ECC_Day_of_Creation;
                                        obj.Kernel_Loadbox_Card_ECC_Month_of_Creation = eccItems.ToArray()[i].Kernel_Loadbox_Card_ECC_Month_of_Creation;
                                        obj.Kernel_Loadbox_Card_ECC_Year_of_Creation = eccItems.ToArray()[i].Kernel_Loadbox_Card_ECC_Year_of_Creation;
                                        obj.Kernel_Loadbox_Card_ECC_BuildVersion = eccItems.ToArray()[i].Kernel_Loadbox_Card_ECC_BuildVersion;

                                        if (ibItems.Count() > 0)
                                        {
                                            obj.ModelSpecific_EID_NET_CAN_Settings_CAN_Settings_rtpc_ixxat_config = ibItems.ToArray()[i].ModelSpecific_EID_NET_CAN_Settings_CAN_Settings_rtpc_ixxat_config;

                                        }

                                        table5DataViews.Add(obj);
                                    }
                                }
                            }
                        }
                        return Json(new { data = table5DataViews }, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception Ex)
                {
                    return View("Unable to retrieve data from database. Please try again later!!");
                }
            }
            else
                return Json(new { success = false, message = "Try again" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetTABLE6_GIO1Data(List<LabComputersPr> pc, DateTime sdate, DateTime edate)
        {
            List<TABLE6_GIO1View> table6DataViews = new List<TABLE6_GIO1View>();
            if (pc != null && pc.Count() > 0)
            {
                try
                {
                    using (PromasterImport_HW_CoGrpsEntities db = new PromasterImport_HW_CoGrpsEntities())
                    {
                        db.Database.CommandTimeout = 500;
                        string date = string.Empty;
                        for (var dt = sdate; dt <= edate; dt = dt.AddDays(1))
                        {
                            date = dt.ToString("yyyy-MM-dd");
                            foreach (var pcname1 in pc)
                            {
                                var pcname = pcname1.FQDN.Split('.')[0].ToUpper().Trim();
                                if (pcname != null && pcname != string.Empty && pcname.Count() > 0)
                                {

                                    var gio1Items = (from item in db.GIO1_Card_Pr orderby item.Date where item.System_name.Contains(pcname) && item.Date.Contains(date) select new { item.Date, item.Kernel_Loadbox_Card_GIO1_BuildVersion, item.Kernel_Loadbox_Card_GIO1_Calibration_Day, item.Kernel_Loadbox_Card_GIO1_Calibration_Month, item.Kernel_Loadbox_Card_GIO1_Calibration_Year, item.Kernel_Loadbox_Card_GIO1_CARD_BoardRevision, item.Kernel_Loadbox_Card_GIO1_CARD_BoardType, item.Kernel_Loadbox_Card_GIO1_CARD_Description, item.Kernel_Loadbox_Card_GIO1_CARD_VER_TAG_ARRAY, item.Kernel_Loadbox_Card_GIO1_Day_of_Creation, item.Kernel_Loadbox_Card_GIO1_Firmware_Card_Major, item.Kernel_Loadbox_Card_GIO1_Firmware_Card_Minor, item.Kernel_Loadbox_Card_GIO1_Firmware_FPGA_Major, item.Kernel_Loadbox_Card_GIO1_Firmware_FPGA_Minor, item.Kernel_Loadbox_Card_GIO1_Month_of_Creation, item.Kernel_Loadbox_Card_GIO1_Next_Calibration_Day, item.Kernel_Loadbox_Card_GIO1_Next_Calibration_Month, item.Kernel_Loadbox_Card_GIO1_Next_Calibration_Year, item.Kernel_Loadbox_Card_GIO1_PowerOnTime, item.Kernel_Loadbox_Card_GIO1_RepairCounter, item.Kernel_Loadbox_Card_GIO1_Serial_No, item.Kernel_Loadbox_Card_GIO1_Year_of_Creation, item.System_name });

                                    for (int i = 0; i < gio1Items.Count(); i++)
                                    {
                                        TABLE6_GIO1View obj = new TABLE6_GIO1View();
                                        obj.Date = DateTime.Parse(gio1Items.ToArray()[i].Date).ToShortDateString();
                                        obj.System_name = gio1Items.ToArray()[i].System_name;

                                        using (BookingServerReplicaEntities db1 = new BookingServerReplicaEntities())
                                        {

                                            var labid = db1.LabComputersPrs.ToList().Find(x => x.FQDN.Split('.')[0].ToUpper().Contains(obj.System_name)).LabId;//Find(x => x.DisplayName.ToString() == obj.System_name.ToString()).LabId;
                                            LabInfo labInfo = new LabInfo();
                                            //labInfo = db.LabInfoes.Where(item => item.Id == labid).FirstOrDefault();
                                            //if (labInfo != null)
                                            // {
                                            obj.LC_Name = db1.LabInfoes.FirstOrDefault(j => j.Id == labid).DisplayName;
                                        }

                                        obj.Kernel_Loadbox_Card_GIO1_CARD_BoardType = gio1Items.ToArray()[i].Kernel_Loadbox_Card_GIO1_CARD_BoardType;
                                        obj.Kernel_Loadbox_Card_GIO1_CARD_BoardRevision = gio1Items.ToArray()[i].Kernel_Loadbox_Card_GIO1_CARD_BoardRevision;
                                        obj.Kernel_Loadbox_Card_GIO1_CARD_VER_TAG_ARRAY = gio1Items.ToArray()[i].Kernel_Loadbox_Card_GIO1_CARD_VER_TAG_ARRAY;
                                        obj.Kernel_Loadbox_Card_GIO1_Serial_No = gio1Items.ToArray()[i].Kernel_Loadbox_Card_GIO1_Serial_No;
                                        obj.Kernel_Loadbox_Card_GIO1_Firmware_Card_Major = gio1Items.ToArray()[i].Kernel_Loadbox_Card_GIO1_Firmware_Card_Major;
                                        obj.Kernel_Loadbox_Card_GIO1_Firmware_Card_Minor = gio1Items.ToArray()[i].Kernel_Loadbox_Card_GIO1_Firmware_Card_Minor;
                                        obj.Kernel_Loadbox_Card_GIO1_Firmware_FPGA_Major = gio1Items.ToArray()[i].Kernel_Loadbox_Card_GIO1_Firmware_FPGA_Major;
                                        obj.Kernel_Loadbox_Card_GIO1_Firmware_FPGA_Minor = gio1Items.ToArray()[i].Kernel_Loadbox_Card_GIO1_Firmware_FPGA_Minor;
                                        obj.Kernel_Loadbox_Card_GIO1_Calibration_Day = gio1Items.ToArray()[i].Kernel_Loadbox_Card_GIO1_Calibration_Day;
                                        obj.Kernel_Loadbox_Card_GIO1_Calibration_Month = gio1Items.ToArray()[i].Kernel_Loadbox_Card_GIO1_Calibration_Month;
                                        obj.Kernel_Loadbox_Card_GIO1_Calibration_Year = gio1Items.ToArray()[i].Kernel_Loadbox_Card_GIO1_Calibration_Year;
                                        obj.Kernel_Loadbox_Card_GIO1_Next_Calibration_Day = gio1Items.ToArray()[i].Kernel_Loadbox_Card_GIO1_Next_Calibration_Day;
                                        obj.Kernel_Loadbox_Card_GIO1_Next_Calibration_Month = gio1Items.ToArray()[i].Kernel_Loadbox_Card_GIO1_Next_Calibration_Month;
                                        obj.Kernel_Loadbox_Card_GIO1_Next_Calibration_Year = gio1Items.ToArray()[i].Kernel_Loadbox_Card_GIO1_Next_Calibration_Year;
                                        obj.Kernel_Loadbox_Card_GIO1_Day_of_Creation = gio1Items.ToArray()[i].Kernel_Loadbox_Card_GIO1_Day_of_Creation;
                                        obj.Kernel_Loadbox_Card_GIO1_Month_of_Creation = gio1Items.ToArray()[i].Kernel_Loadbox_Card_GIO1_Month_of_Creation;
                                        obj.Kernel_Loadbox_Card_GIO1_Year_of_Creation = gio1Items.ToArray()[i].Kernel_Loadbox_Card_GIO1_Year_of_Creation;
                                        obj.Kernel_Loadbox_Card_GIO1_BuildVersion = gio1Items.ToArray()[i].Kernel_Loadbox_Card_GIO1_BuildVersion;

                                        table6DataViews.Add(obj);
                                    }
                                }
                            }
                        }
                        return Json(new { data = table6DataViews }, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception Ex)
                {
                    return View("Unable to retrieve data from database. Please try again later!!");
                }
            }
            else
                return Json(new { success = false, message = "Try again" }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult GetTABLE7_GIO2Data(List<LabComputersPr> pc, DateTime sdate, DateTime edate)
        {
            List<TABLE7_GIO2View> table7DataViews = new List<TABLE7_GIO2View>();
            if (pc != null && pc.Count() > 0)
            {
                try
                {
                    using (PromasterImport_HW_CoGrpsEntities db = new PromasterImport_HW_CoGrpsEntities())
                    {
                        db.Database.CommandTimeout = 500;
                        string date = string.Empty;
                        for (var dt = sdate; dt <= edate; dt = dt.AddDays(1))
                        {
                            date = dt.ToString("yyyy-MM-dd");
                            foreach (var pcname1 in pc)
                            {
                                var pcname = pcname1.FQDN.Split('.')[0].ToUpper().Trim();
                                {
                                    if (pcname != null && pcname != string.Empty && pcname.Count() > 0)
                                    {


                                        var gio2Items = (from item in db.GIO2_Card_Pr orderby item.Date where item.System_name.Contains(pcname) && item.Date.Contains(date) select new { item.Date, item.Kernel_Loadbox_Card_GIO2_BuildVersion, item.Kernel_Loadbox_Card_GIO2_Calibration_Day, item.Kernel_Loadbox_Card_GIO2_Calibration_Month, item.Kernel_Loadbox_Card_GIO2_Calibration_Year, item.Kernel_Loadbox_Card_GIO2_CARD_BoardRevision, item.Kernel_Loadbox_Card_GIO2_CARD_BoardType, item.Kernel_Loadbox_Card_GIO2_CARD_Description, item.Kernel_Loadbox_Card_GIO2_CARD_VER_TAG_ARRAY, item.Kernel_Loadbox_Card_GIO2_Day_of_Creation, item.Kernel_Loadbox_Card_GIO2_Firmware_Card_Major, item.Kernel_Loadbox_Card_GIO2_Firmware_Card_Minor, item.Kernel_Loadbox_Card_GIO2_Firmware_FPGA_Major, item.Kernel_Loadbox_Card_GIO2_Firmware_FPGA_Minor, item.Kernel_Loadbox_Card_GIO2_Month_of_Creation, item.Kernel_Loadbox_Card_GIO2_Next_Calibration_Day, item.Kernel_Loadbox_Card_GIO2_Next_Calibration_Month, item.Kernel_Loadbox_Card_GIO2_Next_Calibration_Year, item.Kernel_Loadbox_Card_GIO2_PowerOnTime, item.Kernel_Loadbox_Card_GIO2_RepairCounter, item.Kernel_Loadbox_Card_GIO2_Serial_No, item.Kernel_Loadbox_Card_GIO2_Year_of_Creation, item.System_name });

                                        for (int i = 0; i < gio2Items.Count(); i++)
                                        {
                                            TABLE7_GIO2View obj = new TABLE7_GIO2View();
                                            obj.Date = DateTime.Parse(gio2Items.ToArray()[i].Date).ToShortDateString();
                                            obj.System_name = gio2Items.ToArray()[i].System_name;

                                            using (BookingServerReplicaEntities db1 = new BookingServerReplicaEntities())
                                            {

                                                var labid = db1.LabComputersPrs.ToList().Find(x => x.FQDN.Split('.')[0].ToUpper().Contains(obj.System_name)).LabId;//Find(x => x.DisplayName.ToString() == obj.System_name.ToString()).LabId;
                                                LabInfo labInfo = new LabInfo();
                                                //labInfo = db.LabInfoes.Where(item => item.Id == labid).FirstOrDefault();
                                                //if (labInfo != null)
                                                // {
                                                obj.LC_Name = db1.LabInfoes.FirstOrDefault(j => j.Id == labid).DisplayName;
                                            }


                                            obj.Kernel_Loadbox_Card_GIO2_CARD_BoardType = gio2Items.ToArray()[i].Kernel_Loadbox_Card_GIO2_CARD_BoardType;
                                            obj.Kernel_Loadbox_Card_GIO2_CARD_BoardRevision = gio2Items.ToArray()[i].Kernel_Loadbox_Card_GIO2_CARD_BoardRevision;
                                            obj.Kernel_Loadbox_Card_GIO2_CARD_VER_TAG_ARRAY = gio2Items.ToArray()[i].Kernel_Loadbox_Card_GIO2_CARD_VER_TAG_ARRAY;
                                            obj.Kernel_Loadbox_Card_GIO2_Serial_No = gio2Items.ToArray()[i].Kernel_Loadbox_Card_GIO2_Serial_No;
                                            obj.Kernel_Loadbox_Card_GIO2_Firmware_Card_Major = gio2Items.ToArray()[i].Kernel_Loadbox_Card_GIO2_Firmware_Card_Major;
                                            obj.Kernel_Loadbox_Card_GIO2_Firmware_Card_Minor = gio2Items.ToArray()[i].Kernel_Loadbox_Card_GIO2_Firmware_Card_Minor;
                                            obj.Kernel_Loadbox_Card_GIO2_Firmware_FPGA_Major = gio2Items.ToArray()[i].Kernel_Loadbox_Card_GIO2_Firmware_FPGA_Major;
                                            obj.Kernel_Loadbox_Card_GIO2_Firmware_FPGA_Minor = gio2Items.ToArray()[i].Kernel_Loadbox_Card_GIO2_Firmware_FPGA_Minor;
                                            obj.Kernel_Loadbox_Card_GIO2_Calibration_Day = gio2Items.ToArray()[i].Kernel_Loadbox_Card_GIO2_Calibration_Day;
                                            obj.Kernel_Loadbox_Card_GIO2_Calibration_Month = gio2Items.ToArray()[i].Kernel_Loadbox_Card_GIO2_Calibration_Month;
                                            obj.Kernel_Loadbox_Card_GIO2_Calibration_Year = gio2Items.ToArray()[i].Kernel_Loadbox_Card_GIO2_Calibration_Year;
                                            obj.Kernel_Loadbox_Card_GIO2_Next_Calibration_Day = gio2Items.ToArray()[i].Kernel_Loadbox_Card_GIO2_Next_Calibration_Day;
                                            obj.Kernel_Loadbox_Card_GIO2_Next_Calibration_Month = gio2Items.ToArray()[i].Kernel_Loadbox_Card_GIO2_Next_Calibration_Month;
                                            obj.Kernel_Loadbox_Card_GIO2_Next_Calibration_Year = gio2Items.ToArray()[i].Kernel_Loadbox_Card_GIO2_Next_Calibration_Year;
                                            obj.Kernel_Loadbox_Card_GIO2_Day_of_Creation = gio2Items.ToArray()[i].Kernel_Loadbox_Card_GIO2_Day_of_Creation;
                                            obj.Kernel_Loadbox_Card_GIO2_Month_of_Creation = gio2Items.ToArray()[i].Kernel_Loadbox_Card_GIO2_Month_of_Creation;
                                            obj.Kernel_Loadbox_Card_GIO2_Year_of_Creation = gio2Items.ToArray()[i].Kernel_Loadbox_Card_GIO2_Year_of_Creation;
                                            obj.Kernel_Loadbox_Card_GIO2_BuildVersion = gio2Items.ToArray()[i].Kernel_Loadbox_Card_GIO2_BuildVersion;

                                            table7DataViews.Add(obj);
                                        }
                                    }
                                }
                            }
                        }
                        return Json(new { data = table7DataViews }, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception Ex)
                {
                    return View("Unable to retrieve data from database. Please try again later!!");
                }
            }
            else
                return Json(new { success = false, message = "Try again" }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult GetTABLE8_LDUData(List<LabComputersPr> pc, DateTime sdate, DateTime edate)
        {
            List<TABLE8_LDUView> table8DataViews = new List<TABLE8_LDUView>();
            if (pc != null && pc.Count() > 0)
            {
                try
                {
                    using (PromasterImport_HW_CoGrpsEntities db = new PromasterImport_HW_CoGrpsEntities())
                    {
                        db.Database.CommandTimeout = 500;
                        string date = string.Empty;
                        for (var dt = sdate; dt <= edate; dt = dt.AddDays(1))
                        {
                            date = dt.ToString("yyyy-MM-dd");
                            foreach (var pcname1 in pc)
                            {
                                var pcname = pcname1.FQDN.Split('.')[0].ToUpper().Trim();
                                if (pcname != null && pcname != string.Empty && pcname.Count() > 0)
                                {

                                    var lduItems = (from item in db.LDU_Card_Pr orderby item.Date where item.System_name.Contains(pcname) && item.Date.Contains(date) select new { item.Date, item.Kernel_Loadbox_Card_LDU_BuildVersion, item.Kernel_Loadbox_Card_LDU_Calibration_Day, item.Kernel_Loadbox_Card_LDU_Calibration_Month, item.Kernel_Loadbox_Card_LDU_Calibration_Year, item.Kernel_Loadbox_Card_LDU_CARD_BoardRevision, item.Kernel_Loadbox_Card_LDU_CARD_BoardType, item.Kernel_Loadbox_Card_LDU_CARD_Description, item.Kernel_Loadbox_Card_LDU_CARD_VER_TAG_ARRAY, item.Kernel_Loadbox_Card_LDU_Day_of_Creation, item.Kernel_Loadbox_Card_LDU_Firmware_Card_Major, item.Kernel_Loadbox_Card_LDU_Firmware_Card_Minor, item.Kernel_Loadbox_Card_LDU_Firmware_FPGA_Major, item.Kernel_Loadbox_Card_LDU_Firmware_FPGA_Minor, item.Kernel_Loadbox_Card_LDU_Month_of_Creation, item.Kernel_Loadbox_Card_LDU_Next_Calibration_Day, item.Kernel_Loadbox_Card_LDU_Next_Calibration_Month, item.Kernel_Loadbox_Card_LDU_Next_Calibration_Year, item.Kernel_Loadbox_Card_LDU_PowerOnTime, item.Kernel_Loadbox_Card_LDU_RepairCounter, item.Kernel_Loadbox_Card_LDU_Serial_No, item.Kernel_Loadbox_Card_LDU_Year_of_Creation, item.System_name });

                                    for (int i = 0; i < lduItems.Count(); i++)
                                    {
                                        TABLE8_LDUView obj = new TABLE8_LDUView();
                                        obj.Date = DateTime.Parse(lduItems.ToArray()[i].Date).ToShortDateString();
                                        obj.System_name = lduItems.ToArray()[i].System_name;

                                        using (BookingServerReplicaEntities db1 = new BookingServerReplicaEntities())
                                        {

                                            var labid = db1.LabComputersPrs.ToList().Find(x => x.FQDN.Split('.')[0].ToUpper().Contains(obj.System_name)).LabId;//Find(x => x.DisplayName.ToString() == obj.System_name.ToString()).LabId;
                                            LabInfo labInfo = new LabInfo();
                                            //labInfo = db.LabInfoes.Where(item => item.Id == labid).FirstOrDefault();
                                            //if (labInfo != null)
                                            // {
                                            obj.LC_Name = db1.LabInfoes.FirstOrDefault(j => j.Id == labid).DisplayName;
                                        }

                                        obj.Kernel_Loadbox_Card_LDU_CARD_BoardType = lduItems.ToArray()[i].Kernel_Loadbox_Card_LDU_CARD_BoardType;
                                        obj.Kernel_Loadbox_Card_LDU_CARD_BoardRevision = lduItems.ToArray()[i].Kernel_Loadbox_Card_LDU_CARD_BoardRevision;
                                        obj.Kernel_Loadbox_Card_LDU_CARD_VER_TAG_ARRAY = lduItems.ToArray()[i].Kernel_Loadbox_Card_LDU_CARD_VER_TAG_ARRAY;
                                        obj.Kernel_Loadbox_Card_LDU_Serial_No = lduItems.ToArray()[i].Kernel_Loadbox_Card_LDU_Serial_No;
                                        obj.Kernel_Loadbox_Card_LDU_Firmware_Card_Major = lduItems.ToArray()[i].Kernel_Loadbox_Card_LDU_Firmware_Card_Major;
                                        obj.Kernel_Loadbox_Card_LDU_Firmware_Card_Minor = lduItems.ToArray()[i].Kernel_Loadbox_Card_LDU_Firmware_Card_Minor;
                                        obj.Kernel_Loadbox_Card_LDU_Firmware_FPGA_Major = lduItems.ToArray()[i].Kernel_Loadbox_Card_LDU_Firmware_FPGA_Major;
                                        obj.Kernel_Loadbox_Card_LDU_Firmware_FPGA_Minor = lduItems.ToArray()[i].Kernel_Loadbox_Card_LDU_Firmware_FPGA_Minor;
                                        obj.Kernel_Loadbox_Card_LDU_Calibration_Day = lduItems.ToArray()[i].Kernel_Loadbox_Card_LDU_Calibration_Day;
                                        obj.Kernel_Loadbox_Card_LDU_Calibration_Month = lduItems.ToArray()[i].Kernel_Loadbox_Card_LDU_Calibration_Month;
                                        obj.Kernel_Loadbox_Card_LDU_Calibration_Year = lduItems.ToArray()[i].Kernel_Loadbox_Card_LDU_Calibration_Year;
                                        obj.Kernel_Loadbox_Card_LDU_Next_Calibration_Day = lduItems.ToArray()[i].Kernel_Loadbox_Card_LDU_Next_Calibration_Day;
                                        obj.Kernel_Loadbox_Card_LDU_Next_Calibration_Month = lduItems.ToArray()[i].Kernel_Loadbox_Card_LDU_Next_Calibration_Month;
                                        obj.Kernel_Loadbox_Card_LDU_Next_Calibration_Year = lduItems.ToArray()[i].Kernel_Loadbox_Card_LDU_Next_Calibration_Year;
                                        obj.Kernel_Loadbox_Card_LDU_Day_of_Creation = lduItems.ToArray()[i].Kernel_Loadbox_Card_LDU_Day_of_Creation;
                                        obj.Kernel_Loadbox_Card_LDU_Month_of_Creation = lduItems.ToArray()[i].Kernel_Loadbox_Card_LDU_Month_of_Creation;
                                        obj.Kernel_Loadbox_Card_LDU_Year_of_Creation = lduItems.ToArray()[i].Kernel_Loadbox_Card_LDU_Year_of_Creation;
                                        obj.Kernel_Loadbox_Card_LDU_BuildVersion = lduItems.ToArray()[i].Kernel_Loadbox_Card_LDU_BuildVersion;

                                        table8DataViews.Add(obj);
                                    }
                                }
                            }
                        }
                        return Json(new { data = table8DataViews }, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception Ex)
                {
                    return View("Unable to retrieve data from database. Please try again later!!");
                }
            }
            else
                return Json(new { success = false, message = "Try again" }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult GetTABLE9_PSCData(List<LabComputersPr> pc, DateTime sdate, DateTime edate)
        {
            List<TABLE9_PSCView> table9DataViews = new List<TABLE9_PSCView>();
            if (pc != null && pc.Count() > 0)
            {
                try
                {
                    using (PromasterImport_HW_CoGrpsEntities db = new PromasterImport_HW_CoGrpsEntities())
                    {
                        db.Database.CommandTimeout = 500;
                        string date = string.Empty;
                        for (var dt = sdate; dt <= edate; dt = dt.AddDays(1))
                        {
                            date = dt.ToString("yyyy-MM-dd");
                            foreach (var pcname1 in pc)
                            {
                                var pcname = pcname1.FQDN.Split('.')[0].ToUpper().Trim();
                                if (pcname != null && pcname != string.Empty && pcname.Count() > 0)
                                {

                                    var pscItems = (from item in db.PSC_Card_Pr orderby item.Date where item.System_name.Contains(pcname) && item.Date.Contains(date) select new { item.Date, item.Kernel_Loadbox_Card_PSC_BuildVersion, item.Kernel_Loadbox_Card_PSC_Calibration_Day, item.Kernel_Loadbox_Card_PSC_Calibration_Month, item.Kernel_Loadbox_Card_PSC_Calibration_Year, item.Kernel_Loadbox_Card_PSC_CARD_BoardRevision, item.Kernel_Loadbox_Card_PSC_CARD_BoardType, item.Kernel_Loadbox_Card_PSC_CARD_Description, item.Kernel_Loadbox_Card_PSC_CARD_VER_TAG_ARRAY, item.Kernel_Loadbox_Card_PSC_Day_of_Creation, item.Kernel_Loadbox_Card_PSC_Firmware_Card_Major, item.Kernel_Loadbox_Card_PSC_Firmware_Card_Minor, item.Kernel_Loadbox_Card_PSC_Firmware_FPGA_Major, item.Kernel_Loadbox_Card_PSC_Firmware_FPGA_Minor, item.Kernel_Loadbox_Card_PSC_Month_of_Creation, item.Kernel_Loadbox_Card_PSC_Next_Calibration_Day, item.Kernel_Loadbox_Card_PSC_Next_Calibration_Month, item.Kernel_Loadbox_Card_PSC_Next_Calibration_Year, item.Kernel_Loadbox_Card_PSC_PowerOnTime, item.Kernel_Loadbox_Card_PSC_RepairCounter, item.Kernel_Loadbox_Card_PSC_Serial_No, item.Kernel_Loadbox_Card_PSC_Year_of_Creation, item.System_name });


                                    for (int i = 0; i < pscItems.Count(); i++)
                                    {
                                        TABLE9_PSCView obj = new TABLE9_PSCView();
                                        obj.Date = DateTime.Parse(pscItems.ToArray()[i].Date).ToShortDateString();
                                        obj.System_name = pscItems.ToArray()[i].System_name;

                                        using (BookingServerReplicaEntities db1 = new BookingServerReplicaEntities())
                                        {

                                            var labid = db1.LabComputersPrs.ToList().Find(x => x.FQDN.Split('.')[0].ToUpper().Contains(obj.System_name)).LabId;//Find(x => x.DisplayName.ToString() == obj.System_name.ToString()).LabId;
                                            LabInfo labInfo = new LabInfo();
                                            //labInfo = db.LabInfoes.Where(item => item.Id == labid).FirstOrDefault();
                                            //if (labInfo != null)
                                            // {
                                            obj.LC_Name = db1.LabInfoes.FirstOrDefault(j => j.Id == labid).DisplayName;
                                        }

                                        obj.Kernel_Loadbox_Card_PSC_CARD_BoardType = pscItems.ToArray()[i].Kernel_Loadbox_Card_PSC_CARD_BoardType;
                                        obj.Kernel_Loadbox_Card_PSC_CARD_BoardRevision = pscItems.ToArray()[i].Kernel_Loadbox_Card_PSC_CARD_BoardRevision;
                                        obj.Kernel_Loadbox_Card_PSC_CARD_VER_TAG_ARRAY = pscItems.ToArray()[i].Kernel_Loadbox_Card_PSC_CARD_VER_TAG_ARRAY;
                                        obj.Kernel_Loadbox_Card_PSC_Serial_No = pscItems.ToArray()[i].Kernel_Loadbox_Card_PSC_Serial_No;
                                        obj.Kernel_Loadbox_Card_PSC_Firmware_Card_Major = pscItems.ToArray()[i].Kernel_Loadbox_Card_PSC_Firmware_Card_Major;
                                        obj.Kernel_Loadbox_Card_PSC_Firmware_Card_Minor = pscItems.ToArray()[i].Kernel_Loadbox_Card_PSC_Firmware_Card_Minor;
                                        obj.Kernel_Loadbox_Card_PSC_Firmware_FPGA_Major = pscItems.ToArray()[i].Kernel_Loadbox_Card_PSC_Firmware_FPGA_Major;
                                        obj.Kernel_Loadbox_Card_PSC_Firmware_FPGA_Minor = pscItems.ToArray()[i].Kernel_Loadbox_Card_PSC_Firmware_FPGA_Minor;
                                        obj.Kernel_Loadbox_Card_PSC_Calibration_Day = pscItems.ToArray()[i].Kernel_Loadbox_Card_PSC_Calibration_Day;
                                        obj.Kernel_Loadbox_Card_PSC_Calibration_Month = pscItems.ToArray()[i].Kernel_Loadbox_Card_PSC_Calibration_Month;
                                        obj.Kernel_Loadbox_Card_PSC_Calibration_Year = pscItems.ToArray()[i].Kernel_Loadbox_Card_PSC_Calibration_Year;
                                        obj.Kernel_Loadbox_Card_PSC_Next_Calibration_Day = pscItems.ToArray()[i].Kernel_Loadbox_Card_PSC_Next_Calibration_Day;
                                        obj.Kernel_Loadbox_Card_PSC_Next_Calibration_Month = pscItems.ToArray()[i].Kernel_Loadbox_Card_PSC_Next_Calibration_Month;
                                        obj.Kernel_Loadbox_Card_PSC_Next_Calibration_Year = pscItems.ToArray()[i].Kernel_Loadbox_Card_PSC_Next_Calibration_Year;
                                        obj.Kernel_Loadbox_Card_PSC_Day_of_Creation = pscItems.ToArray()[i].Kernel_Loadbox_Card_PSC_Day_of_Creation;
                                        obj.Kernel_Loadbox_Card_PSC_Month_of_Creation = pscItems.ToArray()[i].Kernel_Loadbox_Card_PSC_Month_of_Creation;
                                        obj.Kernel_Loadbox_Card_PSC_Year_of_Creation = pscItems.ToArray()[i].Kernel_Loadbox_Card_PSC_Year_of_Creation;
                                        obj.Kernel_Loadbox_Card_PSC_BuildVersion = pscItems.ToArray()[i].Kernel_Loadbox_Card_PSC_BuildVersion;

                                        table9DataViews.Add(obj);
                                    }
                                }
                            }
                        }
                        return Json(new { data = table9DataViews }, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception Ex)
                {
                    return View("Unable to retrieve data from database. Please try again later!!");
                }
            }
            else
                return Json(new { success = false, message = "Try again" }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult GetTABLE10_VSCData(List<LabComputersPr> pc, DateTime sdate, DateTime edate)
        {
            List<TABLE10_VSCView> table10DataViews = new List<TABLE10_VSCView>();
            if (pc != null && pc.Count() > 0)
            {
                try
                {
                    using (PromasterImport_HW_CoGrpsEntities db = new PromasterImport_HW_CoGrpsEntities())
                    {
                        db.Database.CommandTimeout = 500;
                        string date = string.Empty;
                        for (var dt = sdate; dt <= edate; dt = dt.AddDays(1))
                        {
                            date = dt.ToString("yyyy-MM-dd");
                            foreach (var pcname1 in pc)
                            {
                                var pcname = pcname1.FQDN.Split('.')[0].ToUpper().Trim();
                                if (pcname != null && pcname != string.Empty && pcname.Count() > 0)
                                {

                                    var vscItems = (from item in db.VSC_Card_Pr orderby item.Date where item.System_name.Contains(pcname) && item.Date.Contains(date) select new { item.Date, item.Kernel_Loadbox_Card_VSC_BuildVersion, item.Kernel_Loadbox_Card_VSC_Calibration_Day, item.Kernel_Loadbox_Card_VSC_Calibration_Month, item.Kernel_Loadbox_Card_VSC_Calibration_Year, item.Kernel_Loadbox_Card_VSC_CARD_BoardRevision, item.Kernel_Loadbox_Card_VSC_CARD_BoardType, item.Kernel_Loadbox_Card_VSC_CARD_Description, item.Kernel_Loadbox_Card_VSC_CARD_VER_TAG_ARRAY, item.Kernel_Loadbox_Card_VSC_Day_of_Creation, item.Kernel_Loadbox_Card_VSC_Firmware_Card_Major, item.Kernel_Loadbox_Card_VSC_Firmware_Card_Minor, item.Kernel_Loadbox_Card_VSC_Firmware_FPGA_Major, item.Kernel_Loadbox_Card_VSC_Firmware_FPGA_Minor, item.Kernel_Loadbox_Card_VSC_Month_of_Creation, item.Kernel_Loadbox_Card_VSC_Next_Calibration_Day, item.Kernel_Loadbox_Card_VSC_Next_Calibration_Month, item.Kernel_Loadbox_Card_VSC_Next_Calibration_Year, item.Kernel_Loadbox_Card_VSC_PowerOnTime, item.Kernel_Loadbox_Card_VSC_RepairCounter, item.Kernel_Loadbox_Card_VSC_Serial_No, item.Kernel_Loadbox_Card_VSC_Year_of_Creation, item.System_name });

                                    for (int i = 0; i < vscItems.Count(); i++)
                                    {
                                        TABLE10_VSCView obj = new TABLE10_VSCView();
                                        obj.Date = DateTime.Parse(vscItems.ToArray()[i].Date).ToShortDateString();
                                        obj.System_name = vscItems.ToArray()[i].System_name;

                                        using (BookingServerReplicaEntities db1 = new BookingServerReplicaEntities())
                                        {

                                            var labid = db1.LabComputersPrs.ToList().Find(x => x.FQDN.Split('.')[0].ToUpper().Contains(obj.System_name)).LabId;//Find(x => x.DisplayName.ToString() == obj.System_name.ToString()).LabId;
                                            LabInfo labInfo = new LabInfo();
                                            //labInfo = db.LabInfoes.Where(item => item.Id == labid).FirstOrDefault();
                                            //if (labInfo != null)
                                            // {
                                            obj.LC_Name = db1.LabInfoes.FirstOrDefault(j => j.Id == labid).DisplayName;
                                        }


                                        obj.Kernel_Loadbox_Card_VSC_CARD_BoardType = vscItems.ToArray()[i].Kernel_Loadbox_Card_VSC_CARD_BoardType;
                                        obj.Kernel_Loadbox_Card_VSC_CARD_BoardRevision = vscItems.ToArray()[i].Kernel_Loadbox_Card_VSC_CARD_BoardRevision;
                                        obj.Kernel_Loadbox_Card_VSC_CARD_VER_TAG_ARRAY = vscItems.ToArray()[i].Kernel_Loadbox_Card_VSC_CARD_VER_TAG_ARRAY;
                                        obj.Kernel_Loadbox_Card_VSC_Serial_No = vscItems.ToArray()[i].Kernel_Loadbox_Card_VSC_Serial_No;
                                        obj.Kernel_Loadbox_Card_VSC_Firmware_Card_Major = vscItems.ToArray()[i].Kernel_Loadbox_Card_VSC_Firmware_Card_Major;
                                        obj.Kernel_Loadbox_Card_VSC_Firmware_Card_Minor = vscItems.ToArray()[i].Kernel_Loadbox_Card_VSC_Firmware_Card_Minor;
                                        obj.Kernel_Loadbox_Card_VSC_Firmware_FPGA_Major = vscItems.ToArray()[i].Kernel_Loadbox_Card_VSC_Firmware_FPGA_Major;
                                        obj.Kernel_Loadbox_Card_VSC_Firmware_FPGA_Minor = vscItems.ToArray()[i].Kernel_Loadbox_Card_VSC_Firmware_FPGA_Minor;
                                        obj.Kernel_Loadbox_Card_VSC_Calibration_Day = vscItems.ToArray()[i].Kernel_Loadbox_Card_VSC_Calibration_Day;
                                        obj.Kernel_Loadbox_Card_VSC_Calibration_Month = vscItems.ToArray()[i].Kernel_Loadbox_Card_VSC_Calibration_Month;
                                        obj.Kernel_Loadbox_Card_VSC_Calibration_Year = vscItems.ToArray()[i].Kernel_Loadbox_Card_VSC_Calibration_Year;
                                        obj.Kernel_Loadbox_Card_VSC_Next_Calibration_Day = vscItems.ToArray()[i].Kernel_Loadbox_Card_VSC_Next_Calibration_Day;
                                        obj.Kernel_Loadbox_Card_VSC_Next_Calibration_Month = vscItems.ToArray()[i].Kernel_Loadbox_Card_VSC_Next_Calibration_Month;
                                        obj.Kernel_Loadbox_Card_VSC_Next_Calibration_Year = vscItems.ToArray()[i].Kernel_Loadbox_Card_VSC_Next_Calibration_Year;
                                        obj.Kernel_Loadbox_Card_VSC_Day_of_Creation = vscItems.ToArray()[i].Kernel_Loadbox_Card_VSC_Day_of_Creation;
                                        obj.Kernel_Loadbox_Card_VSC_Month_of_Creation = vscItems.ToArray()[i].Kernel_Loadbox_Card_VSC_Month_of_Creation;
                                        obj.Kernel_Loadbox_Card_VSC_Year_of_Creation = vscItems.ToArray()[i].Kernel_Loadbox_Card_VSC_Year_of_Creation;
                                        obj.Kernel_Loadbox_Card_VSC_BuildVersion = vscItems.ToArray()[i].Kernel_Loadbox_Card_VSC_BuildVersion;
                                        table10DataViews.Add(obj);
                                    }
                                }
                            }
                        }
                        return Json(new { data = table10DataViews }, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception Ex)
                {
                    return View("Unable to retrieve data from database. Please try again later!!");
                }
            }
            else
                return Json(new { success = false, message = "Try again" }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetTABLE11_WSSData(List<LabComputersPr> pc, DateTime sdate, DateTime edate)
        {
            List<TABLE11_WSSView> table11DataViews = new List<TABLE11_WSSView>();
            if (pc != null && pc.Count() > 0)
            {
                try
                {
                    using (PromasterImport_HW_CoGrpsEntities db = new PromasterImport_HW_CoGrpsEntities())
                    {
                        db.Database.CommandTimeout = 500;
                        string date = string.Empty;
                        for (var dt = sdate; dt <= edate; dt = dt.AddDays(1))
                        {
                            date = dt.ToString("yyyy-MM-dd");
                            foreach (var pcname1 in pc)
                            {
                                var pcname = pcname1.FQDN.Split('.')[0].ToUpper().Trim();
                                if (pcname != null && pcname != string.Empty && pcname.Count() > 0)
                                {

                                    var wssItems = (from item in db.WSS_Card_Pr orderby item.Date where item.System_name.Contains(pcname) && item.Date.Contains(date) select new { item.Date, item.Kernel_Loadbox_Card_WSS_BuildVersion, item.Kernel_Loadbox_Card_WSS_Calibration_Day, item.Kernel_Loadbox_Card_WSS_Calibration_Month, item.Kernel_Loadbox_Card_WSS_Calibration_Year, item.Kernel_Loadbox_Card_WSS_CARD_BoardRevision, item.Kernel_Loadbox_Card_WSS_CARD_BoardType, item.Kernel_Loadbox_Card_WSS_CARD_Description, item.Kernel_Loadbox_Card_WSS_CARD_VER_TAG_ARRAY, item.Kernel_Loadbox_Card_WSS_Day_of_Creation, item.Kernel_Loadbox_Card_WSS_Firmware_Card_Major, item.Kernel_Loadbox_Card_WSS_Firmware_Card_Minor, item.Kernel_Loadbox_Card_WSS_Firmware_FPGA_Major, item.Kernel_Loadbox_Card_WSS_Firmware_FPGA_Minor, item.Kernel_Loadbox_Card_WSS_Month_of_Creation, item.Kernel_Loadbox_Card_WSS_Next_Calibration_Day, item.Kernel_Loadbox_Card_WSS_Next_Calibration_Month, item.Kernel_Loadbox_Card_WSS_Next_Calibration_Year, item.Kernel_Loadbox_Card_WSS_PowerOnTime, item.Kernel_Loadbox_Card_WSS_RepairCounter, item.Kernel_Loadbox_Card_WSS_Serial_No, item.Kernel_Loadbox_Card_WSS_Year_of_Creation, item.System_name });

                                    for (int i = 0; i < wssItems.Count(); i++)
                                    {
                                        TABLE11_WSSView obj = new TABLE11_WSSView();
                                        obj.Date = DateTime.Parse(wssItems.ToArray()[i].Date).ToShortDateString();
                                        obj.System_name = wssItems.ToArray()[i].System_name;

                                        using (BookingServerReplicaEntities db1 = new BookingServerReplicaEntities())
                                        {

                                            var labid = db1.LabComputersPrs.ToList().Find(x => x.FQDN.Split('.')[0].ToUpper().Contains(obj.System_name)).LabId;//Find(x => x.DisplayName.ToString() == obj.System_name.ToString()).LabId;
                                            LabInfo labInfo = new LabInfo();
                                            //labInfo = db.LabInfoes.Where(item => item.Id == labid).FirstOrDefault();
                                            //if (labInfo != null)
                                            // {
                                            obj.LC_Name = db1.LabInfoes.FirstOrDefault(j => j.Id == labid).DisplayName;
                                        }


                                        obj.Kernel_Loadbox_Card_WSS_CARD_BoardType = wssItems.ToArray()[i].Kernel_Loadbox_Card_WSS_CARD_BoardType;
                                        obj.Kernel_Loadbox_Card_WSS_CARD_BoardRevision = wssItems.ToArray()[i].Kernel_Loadbox_Card_WSS_CARD_BoardRevision;
                                        obj.Kernel_Loadbox_Card_WSS_CARD_VER_TAG_ARRAY = wssItems.ToArray()[i].Kernel_Loadbox_Card_WSS_CARD_VER_TAG_ARRAY;
                                        obj.Kernel_Loadbox_Card_WSS_Serial_No = wssItems.ToArray()[i].Kernel_Loadbox_Card_WSS_Serial_No;
                                        obj.Kernel_Loadbox_Card_WSS_Firmware_Card_Major = wssItems.ToArray()[i].Kernel_Loadbox_Card_WSS_Firmware_Card_Major;
                                        obj.Kernel_Loadbox_Card_WSS_Firmware_Card_Minor = wssItems.ToArray()[i].Kernel_Loadbox_Card_WSS_Firmware_Card_Minor;
                                        obj.Kernel_Loadbox_Card_WSS_Firmware_FPGA_Major = wssItems.ToArray()[i].Kernel_Loadbox_Card_WSS_Firmware_FPGA_Major;
                                        obj.Kernel_Loadbox_Card_WSS_Firmware_FPGA_Minor = wssItems.ToArray()[i].Kernel_Loadbox_Card_WSS_Firmware_FPGA_Minor;
                                        obj.Kernel_Loadbox_Card_WSS_Calibration_Day = wssItems.ToArray()[i].Kernel_Loadbox_Card_WSS_Calibration_Day;
                                        obj.Kernel_Loadbox_Card_WSS_Calibration_Month = wssItems.ToArray()[i].Kernel_Loadbox_Card_WSS_Calibration_Month;
                                        obj.Kernel_Loadbox_Card_WSS_Calibration_Year = wssItems.ToArray()[i].Kernel_Loadbox_Card_WSS_Calibration_Year;
                                        obj.Kernel_Loadbox_Card_WSS_Next_Calibration_Day = wssItems.ToArray()[i].Kernel_Loadbox_Card_WSS_Next_Calibration_Day;
                                        obj.Kernel_Loadbox_Card_WSS_Next_Calibration_Month = wssItems.ToArray()[i].Kernel_Loadbox_Card_WSS_Next_Calibration_Month;
                                        obj.Kernel_Loadbox_Card_WSS_Next_Calibration_Year = wssItems.ToArray()[i].Kernel_Loadbox_Card_WSS_Next_Calibration_Year;
                                        obj.Kernel_Loadbox_Card_WSS_Day_of_Creation = wssItems.ToArray()[i].Kernel_Loadbox_Card_WSS_Day_of_Creation;
                                        obj.Kernel_Loadbox_Card_WSS_Month_of_Creation = wssItems.ToArray()[i].Kernel_Loadbox_Card_WSS_Month_of_Creation;
                                        obj.Kernel_Loadbox_Card_WSS_Year_of_Creation = wssItems.ToArray()[i].Kernel_Loadbox_Card_WSS_Year_of_Creation;
                                        obj.Kernel_Loadbox_Card_WSS_BuildVersion = wssItems.ToArray()[i].Kernel_Loadbox_Card_WSS_BuildVersion;
                                        table11DataViews.Add(obj);
                                    }
                                }
                            }
                        }
                        return Json(new { data = table11DataViews }, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception Ex)
                {
                    return View("Unable to retrieve data from database. Please try again later!!");
                }
            }
            else
                return Json(new { success = false, message = "Try again" }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult GetTABLE12_WSS2Data(List<LabComputersPr> pc, DateTime sdate, DateTime edate)
        {
            List<TABLE12_WSS2View> table12DataViews = new List<TABLE12_WSS2View>();
            if (pc != null && pc.Count() > 0)
            {
                try
                {
                    using (PromasterImport_HW_CoGrpsEntities db = new PromasterImport_HW_CoGrpsEntities())
                    {
                        db.Database.CommandTimeout = 500;
                        string date = string.Empty;
                        for (var dt = sdate; dt <= edate; dt = dt.AddDays(1))
                        {
                            date = dt.ToString("yyyy-MM-dd");
                            foreach (var pcname1 in pc)
                            {
                                var pcname = pcname1.FQDN.Split('.')[0].ToUpper().Trim();
                                if (pcname != null && pcname != string.Empty && pcname.Count() > 0)
                                {

                                    var wss2Items = (from item in db.WSS2_Card_Pr orderby item.Date where item.System_name.Contains(pcname) && item.Date.Contains(date) select new { item.Date, item.Kernel_Loadbox_Card_WSS2_BuildVersion, item.Kernel_Loadbox_Card_WSS2_Calibration_Day, item.Kernel_Loadbox_Card_WSS2_Calibration_Month, item.Kernel_Loadbox_Card_WSS2_Calibration_Year, item.Kernel_Loadbox_Card_WSS2_CARD_BoardRevision, item.Kernel_Loadbox_Card_WSS2_CARD_BoardType, item.Kernel_Loadbox_Card_WSS2_CARD_Description, item.Kernel_Loadbox_Card_WSS2_CARD_VER_TAG_ARRAY, item.Kernel_Loadbox_Card_WSS2_Day_of_Creation, item.Kernel_Loadbox_Card_WSS2_Firmware_Card_Major, item.Kernel_Loadbox_Card_WSS2_Firmware_Card_Minor, item.Kernel_Loadbox_Card_WSS2_Firmware_FPGA_Major, item.Kernel_Loadbox_Card_WSS2_Firmware_FPGA_Minor, item.Kernel_Loadbox_Card_WSS2_Month_of_Creation, item.Kernel_Loadbox_Card_WSS2_Next_Calibration_Day, item.Kernel_Loadbox_Card_WSS2_Next_Calibration_Month, item.Kernel_Loadbox_Card_WSS2_Next_Calibration_Year, item.Kernel_Loadbox_Card_WSS2_PowerOnTime, item.Kernel_Loadbox_Card_WSS2_RepairCounter, item.Kernel_Loadbox_Card_WSS2_Serial_No, item.Kernel_Loadbox_Card_WSS2_Year_of_Creation, item.System_name });


                                    for (int i = 0; i < wss2Items.Count(); i++)
                                    {
                                        TABLE12_WSS2View obj = new TABLE12_WSS2View();
                                        obj.Date = DateTime.Parse(wss2Items.ToArray()[i].Date).ToShortDateString();
                                        obj.System_name = wss2Items.ToArray()[i].System_name;

                                        using (BookingServerReplicaEntities db1 = new BookingServerReplicaEntities())
                                        {

                                            var labid = db1.LabComputersPrs.ToList().Find(x => x.FQDN.Split('.')[0].ToUpper().Contains(obj.System_name)).LabId;//Find(x => x.DisplayName.ToString() == obj.System_name.ToString()).LabId;
                                            LabInfo labInfo = new LabInfo();
                                            //labInfo = db.LabInfoes.Where(item => item.Id == labid).FirstOrDefault();
                                            //if (labInfo != null)
                                            // {
                                            obj.LC_Name = db1.LabInfoes.FirstOrDefault(j => j.Id == labid).DisplayName;
                                        }


                                        obj.Kernel_Loadbox_Card_WSS2_CARD_BoardType = wss2Items.ToArray()[i].Kernel_Loadbox_Card_WSS2_CARD_BoardType;
                                        obj.Kernel_Loadbox_Card_WSS2_CARD_BoardRevision = wss2Items.ToArray()[i].Kernel_Loadbox_Card_WSS2_CARD_BoardRevision;
                                        obj.Kernel_Loadbox_Card_WSS2_CARD_VER_TAG_ARRAY = wss2Items.ToArray()[i].Kernel_Loadbox_Card_WSS2_CARD_VER_TAG_ARRAY;
                                        obj.Kernel_Loadbox_Card_WSS2_Serial_No = wss2Items.ToArray()[i].Kernel_Loadbox_Card_WSS2_Serial_No;
                                        obj.Kernel_Loadbox_Card_WSS2_Firmware_Card_Major = wss2Items.ToArray()[i].Kernel_Loadbox_Card_WSS2_Firmware_Card_Major;
                                        obj.Kernel_Loadbox_Card_WSS2_Firmware_Card_Minor = wss2Items.ToArray()[i].Kernel_Loadbox_Card_WSS2_Firmware_Card_Minor;
                                        obj.Kernel_Loadbox_Card_WSS2_Firmware_FPGA_Major = wss2Items.ToArray()[i].Kernel_Loadbox_Card_WSS2_Firmware_FPGA_Major;
                                        obj.Kernel_Loadbox_Card_WSS2_Firmware_FPGA_Minor = wss2Items.ToArray()[i].Kernel_Loadbox_Card_WSS2_Firmware_FPGA_Minor;
                                        obj.Kernel_Loadbox_Card_WSS2_Calibration_Day = wss2Items.ToArray()[i].Kernel_Loadbox_Card_WSS2_Calibration_Day;
                                        obj.Kernel_Loadbox_Card_WSS2_Calibration_Month = wss2Items.ToArray()[i].Kernel_Loadbox_Card_WSS2_Calibration_Month;
                                        obj.Kernel_Loadbox_Card_WSS2_Calibration_Year = wss2Items.ToArray()[i].Kernel_Loadbox_Card_WSS2_Calibration_Year;
                                        obj.Kernel_Loadbox_Card_WSS2_Next_Calibration_Day = wss2Items.ToArray()[i].Kernel_Loadbox_Card_WSS2_Next_Calibration_Day;
                                        obj.Kernel_Loadbox_Card_WSS2_Next_Calibration_Month = wss2Items.ToArray()[i].Kernel_Loadbox_Card_WSS2_Next_Calibration_Month;
                                        obj.Kernel_Loadbox_Card_WSS2_Next_Calibration_Year = wss2Items.ToArray()[i].Kernel_Loadbox_Card_WSS2_Next_Calibration_Year;
                                        obj.Kernel_Loadbox_Card_WSS2_Day_of_Creation = wss2Items.ToArray()[i].Kernel_Loadbox_Card_WSS2_Day_of_Creation;
                                        obj.Kernel_Loadbox_Card_WSS2_Month_of_Creation = wss2Items.ToArray()[i].Kernel_Loadbox_Card_WSS2_Month_of_Creation;
                                        obj.Kernel_Loadbox_Card_WSS2_Year_of_Creation = wss2Items.ToArray()[i].Kernel_Loadbox_Card_WSS2_Year_of_Creation;
                                        obj.Kernel_Loadbox_Card_WSS2_BuildVersion = wss2Items.ToArray()[i].Kernel_Loadbox_Card_WSS2_BuildVersion;
                                        table12DataViews.Add(obj);
                                    }
                                }
                            }
                        }
                        return Json(new { data = table12DataViews }, JsonRequestBehavior.AllowGet);
                    }
                }
                catch (Exception Ex)
                {
                    return View("Unable to retrieve data from database. Please try again later!!");
                }
            }
            else
                return Json(new { success = false, message = "Try again" }, JsonRequestBehavior.AllowGet);
        }


        //[HttpPost]
        //public ActionResult GetVSCLDUSummaryData(List<LabComputersPr> pc, DateTime sdate, DateTime edate)
        //{
        //    List<TABLE_VSCLDUSummary> table10DataViews = new List<TABLE_VSCLDUSummary>();
        //    if (pc != null && pc.Count() > 0)
        //    {
        //        try
        //        {
        //            using (PromasterImport_HW_CoGrpsEntities db = new PromasterImport_HW_CoGrpsEntities())
        //            {
        //                db.Database.CommandTimeout = 500;
        //                string date = string.Empty;
        //                for (var dt = sdate; dt <= edate; dt = dt.AddDays(1))
        //                {
        //                    date = dt.ToString("yyyy-MM-dd");
        //                    foreach (var pcname1 in pc)
        //                    {
        //                        var pcname = pcname1.FQDN.Split('.')[0].ToUpper().Trim();
        //                        if (pcname != null && pcname != string.Empty && pcname.Count() > 0)
        //                        {

        //                            var vscItems = (from item in db.VSC_Card_Pr orderby item.Date where item.System_name.Contains(pcname) && item.Date.Contains(date) select new { item.Date, item.Kernel_Loadbox_Card_VSC_BuildVersion, item.Kernel_Loadbox_Card_VSC_Calibration_Day, item.Kernel_Loadbox_Card_VSC_Calibration_Month, item.Kernel_Loadbox_Card_VSC_Calibration_Year, item.Kernel_Loadbox_Card_VSC_CARD_BoardRevision, item.Kernel_Loadbox_Card_VSC_CARD_BoardType, item.Kernel_Loadbox_Card_VSC_CARD_Description, item.Kernel_Loadbox_Card_VSC_CARD_VER_TAG_ARRAY, item.Kernel_Loadbox_Card_VSC_Day_of_Creation, item.Kernel_Loadbox_Card_VSC_Firmware_Card_Major, item.Kernel_Loadbox_Card_VSC_Firmware_Card_Minor, item.Kernel_Loadbox_Card_VSC_Firmware_FPGA_Major, item.Kernel_Loadbox_Card_VSC_Firmware_FPGA_Minor, item.Kernel_Loadbox_Card_VSC_Month_of_Creation, item.Kernel_Loadbox_Card_VSC_Next_Calibration_Day, item.Kernel_Loadbox_Card_VSC_Next_Calibration_Month, item.Kernel_Loadbox_Card_VSC_Next_Calibration_Year, item.Kernel_Loadbox_Card_VSC_PowerOnTime, item.Kernel_Loadbox_Card_VSC_RepairCounter, item.Kernel_Loadbox_Card_VSC_Serial_No, item.Kernel_Loadbox_Card_VSC_Year_of_Creation, item.System_name });
        //                            var lduItems = (from item in db.LDU_Card_Pr orderby item.Date where item.System_name.Contains(pcname) && item.Date.Contains(date) select new { item.Date, item.Kernel_Loadbox_Card_LDU_BuildVersion, item.Kernel_Loadbox_Card_LDU_Calibration_Day, item.Kernel_Loadbox_Card_LDU_Calibration_Month, item.Kernel_Loadbox_Card_LDU_Calibration_Year, item.Kernel_Loadbox_Card_LDU_CARD_BoardRevision, item.Kernel_Loadbox_Card_LDU_CARD_BoardType, item.Kernel_Loadbox_Card_LDU_CARD_Description, item.Kernel_Loadbox_Card_LDU_CARD_VER_TAG_ARRAY, item.Kernel_Loadbox_Card_LDU_Day_of_Creation, item.Kernel_Loadbox_Card_LDU_Firmware_Card_Major, item.Kernel_Loadbox_Card_LDU_Firmware_Card_Minor, item.Kernel_Loadbox_Card_LDU_Firmware_FPGA_Major, item.Kernel_Loadbox_Card_LDU_Firmware_FPGA_Minor, item.Kernel_Loadbox_Card_LDU_Month_of_Creation, item.Kernel_Loadbox_Card_LDU_Next_Calibration_Day, item.Kernel_Loadbox_Card_LDU_Next_Calibration_Month, item.Kernel_Loadbox_Card_LDU_Next_Calibration_Year, item.Kernel_Loadbox_Card_LDU_PowerOnTime, item.Kernel_Loadbox_Card_LDU_RepairCounter, item.Kernel_Loadbox_Card_LDU_Serial_No, item.Kernel_Loadbox_Card_LDU_Year_of_Creation, item.System_name });

        //                            for (int i = 0; i < vscItems.Count(); i++)
        //                            {
        //                                TABLE_VSCLDUSummary obj = new TABLE_VSCLDUSummary();
        //                                obj.Date = DateTime.Parse(vscItems.ToArray()[i].Date).ToShortDateString();
        //                                obj.System_name = vscItems.ToArray()[i].System_name;

        //                                using (BookingServerReplicaEntities db1 = new BookingServerReplicaEntities())
        //                                {

        //                                    var labid = db1.LabComputersPrs.ToList().Find(x => x.FQDN.Split('.')[0].ToUpper().Contains(obj.System_name)).LabId;//Find(x => x.DisplayName.ToString() == obj.System_name.ToString()).LabId;
        //                                    LabInfo labInfo = new LabInfo();
        //                                    //labInfo = db.LabInfoes.Where(item => item.Id == labid).FirstOrDefault();
        //                                    //if (labInfo != null)
        //                                    // {
        //                                    obj.LC_Name = db1.LabInfoes.FirstOrDefault(j => j.Id == labid).DisplayName;
        //                                }


        //                                if (vscItems.ToArray()[i].Kernel_Loadbox_Card_VSC_CARD_BoardType == "VSC")
        //                                    obj.VSC_present = "Yes";
        //                                else
        //                                    obj.VSC_present = "No";

        //                                if (lduItems.ToArray()[i].Kernel_Loadbox_Card_LDU_CARD_BoardType == "LDU")
        //                                    obj.LDU_present = "Yes";
        //                                else
        //                                    obj.LDU_present = "No";


        //                                table10DataViews.Add(obj);
        //                            }
        //                        }
        //                    }
        //                }
        //                return Json(new { data = table10DataViews }, JsonRequestBehavior.AllowGet);
        //            }
        //        }
        //        catch (Exception Ex)
        //        {
        //            return View("Unable to retrieve data from database. Please try again later!!");
        //        }
        //    }
        //    else
        //        return Json(new { success = false, message = "Try again" }, JsonRequestBehavior.AllowGet);
        //}

    }



    public partial class TABLE1_HwDataView
    {
        public string LC_Name { get; set; }
        public string System_name { get; set; }
        public string Date { get; set; }
        public string EEPName { get; set; }
        public string EEPBuildDate { get; set; }
        public string EEPDatabaseVersion { get; set; }
        public string PCHostName { get; set; }
        public string RTPCSoftwareVersion { get; set; }
        public string RTPCName { get; set; }
        public string ToolChainVersion { get; set; }
        public string RBCCAFVersion { get; set; }
        public string LabCarType { get; set; }
    }

    public partial class TABLE2_ProjectDescriptionView
    {
        public string LC_Name { get; set; }
        public string System_name { get; set; }
        public string Component_name { get; set; }
        public string Version { get; set; }
        public string Db_Version { get; set; }
        public string Date { get; set; }
        public string Details { get; set; }
        public string ToolVersion { get; set; }
        public string EMUCable { get; set; }
        public string AECUCable { get; set; }
        public string MetaEditor_Ver { get; set; }

        public string ProjectBuilder_Ver { get; set; }
        public string Ascet_Ver { get; set; }
        public string ProjectEditor_Ver { get; set; }
    }

    public partial class TABLE3_APB_BOB_Cable_EB_ES4441_1_ES4441_2_HAP1View
    {
        public string LC_Name { get; set; }
        public string System_name { get; set; }
        public string Date { get; set; }
        public string ModelSpecific_EID_Switch_APB_APB_Module_GIO2_Signature_GIO2 { get; set; }


        public string Kernel_Loadbox_Card_ECC_BreakOutBox_BOB_SerialNumber { get; set; }
        public string Kernel_Loadbox_Card_ECC_BreakOutBox_BOB_DeviceRevision { get; set; }


        public string Kernel_Loadbox_Card_ECC_Cable_CABLE_SerialNumber { get; set; }
        public string Kernel_Loadbox_Card_ECC_Cable_CABLE_DeviceRevision { get; set; }

        public string Kernel_EB_Cards_EB5100 { get; set; }
        public string Kernel_EB_Cards_EB5200 { get; set; }

        public string Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441SN { get; set; }
        public string Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441FWVersionMain { get; set; }
        public string Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441FWVersionMajor { get; set; }
        public string Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441FWVersionMinor { get; set; }

        public string Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441HWVersion { get; set; }
        public string Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441HWRevision { get; set; }

        public string Kernel_Loadbox_Card_ECC_Harnesadapter_HAP_SerialNumber { get; set; }

    }



    public partial class TABLE4_OTSO_BOB1_Cable1_PowerSupply_HAP_IXXATView
    {
        public string LC_Name { get; set; }
        public string System_name { get; set; }
        public string Date { get; set; }


        public string Kernel_Component_IPB_TemperatureReceive_OTSO1Available { get; set; }
        public string Kernel_Component_IPB_TemperatureReceive_OTSO2Available { get; set; }
        public string Kernel_Loadbox_Card_ECC_BreakOutBox_BOB_SerialNumber { get; set; }
        public string Kernel_Loadbox_Card_ECC_Cable_CABLE_SerialNumber { get; set; }
        public string Kernel_Loadbox_Card_PSC_Power_Supply_Configuration_PS_Configuration { get; set; }
        public string Kernel_Loadbox_Card_PSC_Power_Supply_Configuration_PS_Factor_ECU { get; set; }
        public string Kernel_Loadbox_Card_PSC_Power_Supply_Configuration_PS_Factor_MR { get; set; }
        public string Kernel_Loadbox_Card_PSC_Power_Supply_Configuration_PS_Factor_VR { get; set; }
        public string Kernel_Loadbox_Card_ECC_Harnesadapter_HAP_SerialNumber { get; set; }
        public string Kernel_Loadbox_Card_ECC_Harnesadapter_HAP_DeviceRevision { get; set; }
        public string ModelSpecific_EID_NET_CAN_Settings_CAN_Settings_rtpc_ixxat_config { get; set; }
    }

    public partial class TABLE5_ECC_IBView
    {
        public string LC_Name { get; set; }
        public string System_name { get; set; }
        public string Date { get; set; }
        public string ModelSpecific_EID_NET_CAN_Settings_CAN_Settings_rtpc_ixxat_config { get; set; }

        public string Kernel_Loadbox_Card_ECC_CARD_BoardType { get; set; }
        public string Kernel_Loadbox_Card_ECC_CARD_BoardRevision { get; set; }
        public string Kernel_Loadbox_Card_ECC_CARD_VER_TAG_ARRAY { get; set; }
        public string Kernel_Loadbox_Card_ECC_Serial_No { get; set; }
        public string Kernel_Loadbox_Card_ECC_Firmware_Card_Major { get; set; }
        public string Kernel_Loadbox_Card_ECC_Firmware_Card_Minor { get; set; }
        public string Kernel_Loadbox_Card_ECC_Firmware_FPGA_Major { get; set; }
        public string Kernel_Loadbox_Card_ECC_Firmware_FPGA_Minor { get; set; }
        public string Kernel_Loadbox_Card_ECC_Calibration_Day { get; set; }
        public string Kernel_Loadbox_Card_ECC_Calibration_Month { get; set; }
        public string Kernel_Loadbox_Card_ECC_Calibration_Year { get; set; }
        public string Kernel_Loadbox_Card_ECC_Next_Calibration_Day { get; set; }
        public string Kernel_Loadbox_Card_ECC_Next_Calibration_Month { get; set; }
        public string Kernel_Loadbox_Card_ECC_Next_Calibration_Year { get; set; }
        public string Kernel_Loadbox_Card_ECC_Day_of_Creation { get; set; }
        public string Kernel_Loadbox_Card_ECC_Month_of_Creation { get; set; }
        public string Kernel_Loadbox_Card_ECC_Year_of_Creation { get; set; }
        public string Kernel_Loadbox_Card_ECC_BuildVersion { get; set; }

    }

    public partial class TABLE6_GIO1View
    {
        public string LC_Name { get; set; }
        public string System_name { get; set; }
        public string Date { get; set; }
        public string Kernel_Loadbox_Card_GIO1_CARD_BoardType { get; set; }
        public string Kernel_Loadbox_Card_GIO1_CARD_BoardRevision { get; set; }
        public string Kernel_Loadbox_Card_GIO1_CARD_VER_TAG_ARRAY { get; set; }
        public string Kernel_Loadbox_Card_GIO1_Serial_No { get; set; }
        public string Kernel_Loadbox_Card_GIO1_Firmware_Card_Major { get; set; }
        public string Kernel_Loadbox_Card_GIO1_Firmware_Card_Minor { get; set; }
        public string Kernel_Loadbox_Card_GIO1_Firmware_FPGA_Major { get; set; }
        public string Kernel_Loadbox_Card_GIO1_Firmware_FPGA_Minor { get; set; }
        public string Kernel_Loadbox_Card_GIO1_Calibration_Day { get; set; }
        public string Kernel_Loadbox_Card_GIO1_Calibration_Month { get; set; }
        public string Kernel_Loadbox_Card_GIO1_Calibration_Year { get; set; }
        public string Kernel_Loadbox_Card_GIO1_Next_Calibration_Day { get; set; }
        public string Kernel_Loadbox_Card_GIO1_Next_Calibration_Month { get; set; }
        public string Kernel_Loadbox_Card_GIO1_Next_Calibration_Year { get; set; }
        public string Kernel_Loadbox_Card_GIO1_Day_of_Creation { get; set; }
        public string Kernel_Loadbox_Card_GIO1_Month_of_Creation { get; set; }
        public string Kernel_Loadbox_Card_GIO1_Year_of_Creation { get; set; }
        public string Kernel_Loadbox_Card_GIO1_BuildVersion { get; set; }
    }

    public partial class TABLE7_GIO2View
    {
        public string LC_Name { get; set; }
        public string System_name { get; set; }
        public string Date { get; set; }

        public string Kernel_Loadbox_Card_GIO2_CARD_BoardType { get; set; }
        public string Kernel_Loadbox_Card_GIO2_CARD_BoardRevision { get; set; }
        public string Kernel_Loadbox_Card_GIO2_CARD_VER_TAG_ARRAY { get; set; }
        public string Kernel_Loadbox_Card_GIO2_Serial_No { get; set; }
        public string Kernel_Loadbox_Card_GIO2_Firmware_Card_Major { get; set; }
        public string Kernel_Loadbox_Card_GIO2_Firmware_Card_Minor { get; set; }
        public string Kernel_Loadbox_Card_GIO2_Firmware_FPGA_Major { get; set; }
        public string Kernel_Loadbox_Card_GIO2_Firmware_FPGA_Minor { get; set; }
        public string Kernel_Loadbox_Card_GIO2_Calibration_Day { get; set; }
        public string Kernel_Loadbox_Card_GIO2_Calibration_Month { get; set; }
        public string Kernel_Loadbox_Card_GIO2_Calibration_Year { get; set; }
        public string Kernel_Loadbox_Card_GIO2_Next_Calibration_Day { get; set; }
        public string Kernel_Loadbox_Card_GIO2_Next_Calibration_Month { get; set; }
        public string Kernel_Loadbox_Card_GIO2_Next_Calibration_Year { get; set; }
        public string Kernel_Loadbox_Card_GIO2_Day_of_Creation { get; set; }
        public string Kernel_Loadbox_Card_GIO2_Month_of_Creation { get; set; }
        public string Kernel_Loadbox_Card_GIO2_Year_of_Creation { get; set; }
        public string Kernel_Loadbox_Card_GIO2_BuildVersion { get; set; }
    }


    public partial class TABLE8_LDUView
    {
        public string LC_Name { get; set; }
        public string System_name { get; set; }
        public string Date { get; set; }
        public string Kernel_Loadbox_Card_LDU_CARD_BoardType { get; set; }
        public string Kernel_Loadbox_Card_LDU_CARD_BoardRevision { get; set; }
        public string Kernel_Loadbox_Card_LDU_CARD_VER_TAG_ARRAY { get; set; }
        public string Kernel_Loadbox_Card_LDU_Serial_No { get; set; }
        public string Kernel_Loadbox_Card_LDU_Firmware_Card_Major { get; set; }
        public string Kernel_Loadbox_Card_LDU_Firmware_Card_Minor { get; set; }
        public string Kernel_Loadbox_Card_LDU_Firmware_FPGA_Major { get; set; }
        public string Kernel_Loadbox_Card_LDU_Firmware_FPGA_Minor { get; set; }
        public string Kernel_Loadbox_Card_LDU_Calibration_Day { get; set; }
        public string Kernel_Loadbox_Card_LDU_Calibration_Month { get; set; }
        public string Kernel_Loadbox_Card_LDU_Calibration_Year { get; set; }
        public string Kernel_Loadbox_Card_LDU_Next_Calibration_Day { get; set; }
        public string Kernel_Loadbox_Card_LDU_Next_Calibration_Month { get; set; }
        public string Kernel_Loadbox_Card_LDU_Next_Calibration_Year { get; set; }
        public string Kernel_Loadbox_Card_LDU_Day_of_Creation { get; set; }
        public string Kernel_Loadbox_Card_LDU_Month_of_Creation { get; set; }
        public string Kernel_Loadbox_Card_LDU_Year_of_Creation { get; set; }
        public string Kernel_Loadbox_Card_LDU_BuildVersion { get; set; }

    }

    public partial class TABLE9_PSCView
    {
        public string LC_Name { get; set; }
        public string System_name { get; set; }
        public string Date { get; set; }
        public string Kernel_Loadbox_Card_PSC_CARD_BoardType { get; set; }
        public string Kernel_Loadbox_Card_PSC_CARD_BoardRevision { get; set; }
        public string Kernel_Loadbox_Card_PSC_CARD_VER_TAG_ARRAY { get; set; }
        public string Kernel_Loadbox_Card_PSC_Serial_No { get; set; }
        public string Kernel_Loadbox_Card_PSC_Firmware_Card_Major { get; set; }
        public string Kernel_Loadbox_Card_PSC_Firmware_Card_Minor { get; set; }
        public string Kernel_Loadbox_Card_PSC_Firmware_FPGA_Major { get; set; }
        public string Kernel_Loadbox_Card_PSC_Firmware_FPGA_Minor { get; set; }
        public string Kernel_Loadbox_Card_PSC_Calibration_Day { get; set; }
        public string Kernel_Loadbox_Card_PSC_Calibration_Month { get; set; }
        public string Kernel_Loadbox_Card_PSC_Calibration_Year { get; set; }
        public string Kernel_Loadbox_Card_PSC_Next_Calibration_Day { get; set; }
        public string Kernel_Loadbox_Card_PSC_Next_Calibration_Month { get; set; }
        public string Kernel_Loadbox_Card_PSC_Next_Calibration_Year { get; set; }
        public string Kernel_Loadbox_Card_PSC_Day_of_Creation { get; set; }
        public string Kernel_Loadbox_Card_PSC_Month_of_Creation { get; set; }
        public string Kernel_Loadbox_Card_PSC_Year_of_Creation { get; set; }
        public string Kernel_Loadbox_Card_PSC_BuildVersion { get; set; }

    }

    public partial class TABLE10_VSCView
    {
        public string LC_Name { get; set; }
        public string System_name { get; set; }
        public string Date { get; set; }

        public string Kernel_Loadbox_Card_VSC_CARD_BoardType { get; set; }
        public string Kernel_Loadbox_Card_VSC_CARD_BoardRevision { get; set; }
        public string Kernel_Loadbox_Card_VSC_CARD_VER_TAG_ARRAY { get; set; }
        public string Kernel_Loadbox_Card_VSC_Serial_No { get; set; }
        public string Kernel_Loadbox_Card_VSC_Firmware_Card_Major { get; set; }
        public string Kernel_Loadbox_Card_VSC_Firmware_Card_Minor { get; set; }
        public string Kernel_Loadbox_Card_VSC_Firmware_FPGA_Major { get; set; }
        public string Kernel_Loadbox_Card_VSC_Firmware_FPGA_Minor { get; set; }
        public string Kernel_Loadbox_Card_VSC_Calibration_Day { get; set; }
        public string Kernel_Loadbox_Card_VSC_Calibration_Month { get; set; }
        public string Kernel_Loadbox_Card_VSC_Calibration_Year { get; set; }
        public string Kernel_Loadbox_Card_VSC_Next_Calibration_Day { get; set; }
        public string Kernel_Loadbox_Card_VSC_Next_Calibration_Month { get; set; }
        public string Kernel_Loadbox_Card_VSC_Next_Calibration_Year { get; set; }
        public string Kernel_Loadbox_Card_VSC_Day_of_Creation { get; set; }
        public string Kernel_Loadbox_Card_VSC_Month_of_Creation { get; set; }
        public string Kernel_Loadbox_Card_VSC_Year_of_Creation { get; set; }
        public string Kernel_Loadbox_Card_VSC_BuildVersion { get; set; }
    }

    public partial class TABLE11_WSSView
    {
        public string LC_Name { get; set; }
        public string System_name { get; set; }
        public string Date { get; set; }
        public string Kernel_Loadbox_Card_WSS_CARD_BoardType { get; set; }
        public string Kernel_Loadbox_Card_WSS_CARD_BoardRevision { get; set; }
        public string Kernel_Loadbox_Card_WSS_CARD_VER_TAG_ARRAY { get; set; }
        public string Kernel_Loadbox_Card_WSS_Serial_No { get; set; }
        public string Kernel_Loadbox_Card_WSS_Firmware_Card_Major { get; set; }
        public string Kernel_Loadbox_Card_WSS_Firmware_Card_Minor { get; set; }
        public string Kernel_Loadbox_Card_WSS_Firmware_FPGA_Major { get; set; }
        public string Kernel_Loadbox_Card_WSS_Firmware_FPGA_Minor { get; set; }
        public string Kernel_Loadbox_Card_WSS_Calibration_Day { get; set; }
        public string Kernel_Loadbox_Card_WSS_Calibration_Month { get; set; }
        public string Kernel_Loadbox_Card_WSS_Calibration_Year { get; set; }
        public string Kernel_Loadbox_Card_WSS_Next_Calibration_Day { get; set; }
        public string Kernel_Loadbox_Card_WSS_Next_Calibration_Month { get; set; }
        public string Kernel_Loadbox_Card_WSS_Next_Calibration_Year { get; set; }
        public string Kernel_Loadbox_Card_WSS_Day_of_Creation { get; set; }
        public string Kernel_Loadbox_Card_WSS_Month_of_Creation { get; set; }
        public string Kernel_Loadbox_Card_WSS_Year_of_Creation { get; set; }
        public string Kernel_Loadbox_Card_WSS_BuildVersion { get; set; }
    }

    public partial class TABLE12_WSS2View
    {
        public string LC_Name { get; set; }
        public string System_name { get; set; }
        public string Date { get; set; }

        public string Kernel_Loadbox_Card_WSS2_CARD_BoardType { get; set; }
        public string Kernel_Loadbox_Card_WSS2_CARD_BoardRevision { get; set; }
        public string Kernel_Loadbox_Card_WSS2_CARD_VER_TAG_ARRAY { get; set; }
        public string Kernel_Loadbox_Card_WSS2_Serial_No { get; set; }
        public string Kernel_Loadbox_Card_WSS2_Firmware_Card_Major { get; set; }
        public string Kernel_Loadbox_Card_WSS2_Firmware_Card_Minor { get; set; }
        public string Kernel_Loadbox_Card_WSS2_Firmware_FPGA_Major { get; set; }
        public string Kernel_Loadbox_Card_WSS2_Firmware_FPGA_Minor { get; set; }
        public string Kernel_Loadbox_Card_WSS2_Calibration_Day { get; set; }
        public string Kernel_Loadbox_Card_WSS2_Calibration_Month { get; set; }
        public string Kernel_Loadbox_Card_WSS2_Calibration_Year { get; set; }
        public string Kernel_Loadbox_Card_WSS2_Next_Calibration_Day { get; set; }
        public string Kernel_Loadbox_Card_WSS2_Next_Calibration_Month { get; set; }
        public string Kernel_Loadbox_Card_WSS2_Next_Calibration_Year { get; set; }
        public string Kernel_Loadbox_Card_WSS2_Day_of_Creation { get; set; }
        public string Kernel_Loadbox_Card_WSS2_Month_of_Creation { get; set; }
        public string Kernel_Loadbox_Card_WSS2_Year_of_Creation { get; set; }
        public string Kernel_Loadbox_Card_WSS2_BuildVersion { get; set; }
    }

    //public partial class TABLE_VSCLDUSummary
    //{
    //    public string LC_Name { get; set; }
    //    public string System_name { get; set; }
    //    public string Date { get; set; }

    //    public string VSC_present { get; set; }
    //    public string LDU_present { get; set; }
    //}
}





