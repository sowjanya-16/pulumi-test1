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
using System.Data.SqlClient;
using System.Configuration;
using Microsoft.XmlDiffPatch;
using System.Xml;
using System.Text;
using System.Drawing;
using System.Web.Script.Serialization;

namespace LC_Reports_V1.Controllers
{
    /// <summary>
    /// Controller class for Diagnostics View
    /// </summary>
    //[Authorize(Users = @"apac\din2cob,de\add2abt,de\let2abt,de\ton2abt,apac\jov6cob,apac\mta2cob,apac\muu4cob,apac\gph2hc,APAC\NYB1HC,us\lpr1ga,apac\chb1kor,apac\sat1dz,apac\KUH2YK,apac\KOS2YK,apac\ITO2YK,apac\oig1cob,de\hhr1lr,de\utk4fe,de\dau2abt,de\bji2si,us\stj3ply,de\sth2abt,apac\whe1szh,apac\mae9cob,apac\rma5cob ,de\rud2abt, de\has2abt , apac\ofa2abt, apac\sif1cob, apac\asr81cl")]
    public class DiagnosticsController : Controller
    {
        public static string LabInfoPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/LC_LabInfo.json");
        public static string LabSitesPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/LC_LabSites.json");
        public static string LabLocationsPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Content/LC_LabLocations.json");
        public List<string> lstPCs = new List<string>();
        private SqlConnection con;


        private void connection()
        {

            string constring = ConfigurationManager.ConnectionStrings["PromasterImport_HW_CoGrpsEntities_ado"].ToString();
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

        public void GenerateDiffGram(string originalFile, string finalFile,
                                    XmlWriter diffGramWriter)
        {
            string newFile = "";
            XmlDiff xmldiff = new XmlDiff(XmlDiffOptions.IgnoreChildOrder |
                                             XmlDiffOptions.IgnoreNamespaces |
                                             XmlDiffOptions.IgnorePrefixes);
            bool bIdentical = xmldiff.Compare(originalFile, finalFile, true, diffGramWriter);
            diffGramWriter.Close();
        }

        public string diagnostics_authorise()
        {
            //Populate();
            //var actualxml = "<title language=English country=USA>C++ Primer</title> <author>Lippmann</author> <publisher>Harper Collins</publisher> <price>4.6</price>";
            //var expectedxml =
            //"<title language=English country=India>C++ Primer</title> <author>Lippmann</author> <publisher>Harper Collins</publisher> <price>88</price>";
            //XmlWriterSettings settings = new XmlWriterSettings();
            //settings.Indent = true;
            //settings.NewLineOnAttributes = true;
            //XmlWriter diffGramWriter = XmlWriter.Create(@"C:\MAE9COB\LatestSLSoln\June2022_SLSoln_proj\LC_Reports_V1\LC_Reports_V1\Content\LCComparator.xml", settings);
            //GenerateDiffGram(actualxml, expectedxml, diffGramWriter);



            //XmlReader.Create("data.xml");
            //XmlDiff xmlcompare = new XmlDiff();
            //bool bIdentical = xmlcompare.Compare(expectedxml, actualxml);



            string NTID = "";
            string constring = ConfigurationManager.ConnectionStrings["PromasterImport_HWInfoEntities_ado"].ToString();
            SqlConnection con_diag = new SqlConnection(constring);
            con_diag.Open();
            string qry = " Exec [dbo].[Diagnostics_Authorize] '" + User.Identity.Name.Split('\\')[1].Trim() + "' ";

            SqlCommand command = new SqlCommand(qry, con_diag);
            SqlDataReader dr = command.ExecuteReader();

            if (dr.HasRows)
            {
                dr.Read();
                NTID = dr["NTID"].ToString();
                //  NTID = "stp2kor";
            }
            else
            {
                NTID = "";
            }
            dr.Close();
            con_diag.Close();
            return NTID;
        }


        private void Populate()
        {
            try
            {
                string _xml = @"C:\MAE9COB\LatestSLSoln\June2022_SLSoln_proj\LC_Reports_V1\LC_Reports_V1\Content\Hardware_COB1098715_16-07-2022_06-57-09.xml";

                // SECTION 1. Create a DOM Document and load the XML data into it.
                XmlDocument dom = new XmlDocument();
                dom.Load(_xml);

                // SECTION 2. Initialize the TreeView control.
                TreeView treeView1 = new TreeView();
                treeView1.Nodes.Clear();
                treeView1.Nodes.Add(new TreeNode(dom.DocumentElement.Name));
                TreeNode tNode = new TreeNode();
                tNode = treeView1.Nodes[0];
                //var ccc = tNode.ChildNodes.ChildNodes.Items[0].ChildNodes.Items[0].ChildNodes.Items[0].Text;            // SECTION 3. Populate the TreeView with the DOM nodes.
                AddNode(dom.DocumentElement, tNode);
                treeView1.ExpandAll();
            }
            catch (XmlException xmlEx)
            {

            }
            catch (Exception ex)
            {

            }
        }

        private void AddNode(XmlNode inXmlNode, TreeNode inTreeNode)
        {
            XmlNode xNode;
            TreeNode tNode;
            XmlNodeList nodeList;
            int i;

            // Loop through the XML nodes until the leaf is reached.
            // Add the nodes to the TreeView during the looping process.
            if (inXmlNode.HasChildNodes)
            {
                nodeList = inXmlNode.ChildNodes;
                for (i = 0; i <= nodeList.Count - 1; i++)
                {
                    xNode = inXmlNode.ChildNodes[i];
                    inTreeNode.ChildNodes.Add(new TreeNode(xNode.Name));
                    tNode = inTreeNode.ChildNodes[i];
                    AddNode(xNode, tNode);
                }
            }
            else
            {
                // Here you need to pull the data from the XmlNode based on the
                // type of node, whether attribute values are required, and so forth.
                inTreeNode.Text = (inXmlNode.OuterXml).Trim();
            }
        }

        /// <summary>
        /// GET: Diagnostics
        /// Function to load the landing page of Diagnostics controller
        /// </summary>       
        public ActionResult DiagnosticsView()
        {
            //var xmlstr_1 = "<Root><Clild>child text</Clild></Root>";
            //var xmlstr_2 = "<Root><Clild>child ttttt</Clild></Root>";
            //var xdoc1 = System.Xml.Linq.XDocument.Parse(xmlstr_1);
            //var xdoc2 = System.Xml.Linq.XDocument.Parse(xmlstr_2);

            string NTID = diagnostics_authorise();
            if (NTID == "")
            {
                // throw new HttpException(404, "Sorry! Current user is not authorised to access this view!");
                return Content("Sorry! Current user is not authorised to access this view!");
                //return RedirectToAction("Index", "Dashboard");
            }
            else
            {
                DiagnosticsParam diagnosticsInfo = new DiagnosticsParam();
                List<LabInfo> lstLabDef = new List<LabInfo>();
                List<Site> objSites = new List<Site>();
                List<Location> lstLabLocs = new List<Location>();
                List<LabType> objLabType = new List<LabType>();
                DiagnosticsParam.LabBookingExport objLabExport = new DiagnosticsParam.LabBookingExport();
                using (BookingServerReplicaEntities db = new BookingServerReplicaEntities())
                {
                    objSites = db.Sites.ToList();
                    lstLabLocs = db.Locations.ToList();
                    objLabType = db.LabTypes.ToList();
                    lstLabDef = db.LabInfoes.ToList();
                }

                //lstLabDef = loadJsonObjects_LabID();
                //objSites = loadJsonObjects_Sites();
                //lstLabLocs = loadJsonObjects_Locations();
                //objLabExport = initDataObject();

                //Rb Code corresponding to Sites on ActiveSafety 
                List<string> rbcode_AS = new List<string>()
                { "Abt","AnP","Ban","Cob","Cl","Ga","Hc","Ply","Szh","Yh"};

                //HW/Project Details filter controls in UI
                if (lstLabDef != null)
                {
                    diagnosticsInfo.StartTime = DateTime.Now.AddMonths(-1).Date;
                    diagnosticsInfo.EndTime = DateTime.Now.Date;
                    diagnosticsInfo.LCLocationName = "NA";
                    diagnosticsInfo.LabID = "NA";

                    diagnosticsInfo.LabIDs = new List<SelectListItem>();
                    lstLabDef = lstLabDef.OrderBy(item => item.DisplayName).ToList();

                    foreach (var lab in lstLabDef.FindAll(x => x.TypeId == 8 || x.TypeId == 9)) //11 - generic test lab
                    {
                        diagnosticsInfo.LabIDs.Add(new SelectListItem { Text = lab.DisplayName, Value = lab.Id.ToString() });
                    }

                    diagnosticsInfo.LabIDs.Sort((a, b) => a.Text.CompareTo(b.Text));


                }

                //LC type filter controls in UI
                //if (objSites != null)
                //{

                diagnosticsInfo.Sites = new List<SelectListItem>();
                foreach (var sitesattr in objSites)
                {
                    if (rbcode_AS.Contains(sitesattr.RbCode))
                    {
                        diagnosticsInfo.Sites.Add(new SelectListItem { Text = sitesattr.DisplayName, Value = sitesattr.RbCode.ToUpper() });
                    }
                }
                diagnosticsInfo.Sites.Sort((a, b) => a.Value.CompareTo(b.Value));


                //diagnosticsInfo.objFilterPageExpInfo = new DiagnosticsParam.LabBookingExport();
                //diagnosticsInfo.objFilterPageExpInfo = objLabExport;

                ViewBag.Message = "Your Diagnostics Options page.";
                return View(diagnosticsInfo);
            }

        }


        //Actionresult to fetch the HIL Comparator - setup configuration details
        public ActionResult LCComparator(string HIL1, string HIL2)
        {
            string Query = string.Empty;
            DataTable dt = new DataTable();
            string constring = ConfigurationManager.ConnectionStrings["PromasterImport_HWInfoEntities_ado"].ToString();
            con = new SqlConnection(constring);
            OpenConnection();
            //Query to execute stored procedure
            Query = "Exec [dbo].[LC_Comparator] '" + HIL1 + "','" + HIL2 + "'";
            SqlCommand cmd = new SqlCommand(Query, con);
            cmd.CommandTimeout = 1000;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            CloseConnection();
            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            List<Dictionary<string, object>> parentRow = new List<Dictionary<string, object>>();
            Dictionary<string, object> childRow;
            foreach (DataRow row in dt.Rows)
            {
                childRow = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Columns)
                {
                    childRow.Add(col.ColumnName, row[col]);
                }
                parentRow.Add(childRow);
            }
            jsSerializer.MaxJsonLength = Int32.MaxValue;

            var resultData = parentRow;
            var result = new ContentResult
            {
                Content = jsSerializer.Serialize(resultData),
                ContentType = "application/json"
            };

            JsonResult result1 = Json(result);
            result1.MaxJsonLength = Int32.MaxValue;
            result1.JsonRequestBehavior = JsonRequestBehavior.AllowGet;
            //return json data object with HIL details
            return Json(new { data = result1 }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult HILSnapshot(string HIL1)
        {
            string Query = string.Empty;
            DataSet dt = new DataSet();
            string constring = ConfigurationManager.ConnectionStrings["PromasterImport_HWInfoEntities_ado"].ToString();
            con = new SqlConnection(constring);
            OpenConnection();
            //Query to execute stored procedure
            Query = "Exec [dbo].[HILSnapshot] '" + HIL1 + "'";
            SqlCommand cmd = new SqlCommand(Query, con);
            cmd.CommandTimeout = 1000;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            CloseConnection();
            JavaScriptSerializer jsSerializer = new JavaScriptSerializer();
            List<Dictionary<string, object>> parentRow = new List<Dictionary<string, object>>();
            Dictionary<string, object> childRow;
            foreach (DataRow row in dt.Tables[0].Rows)
            {
                childRow = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Tables[0].Columns)
                {
                    childRow.Add(col.ColumnName, row[col]);
                }
                parentRow.Add(childRow);
            }
            jsSerializer.MaxJsonLength = Int32.MaxValue;

            var resultData = parentRow;
            var result = new ContentResult
            {
                Content = jsSerializer.Serialize(resultData),
                ContentType = "application/json"
            };

            JsonResult result1 = Json(result);
            result1.MaxJsonLength = Int32.MaxValue;
            result1.JsonRequestBehavior = JsonRequestBehavior.AllowGet;

            //************** 2
            foreach (DataRow row in dt.Tables[1].Rows)
            {
                childRow = new Dictionary<string, object>();
                foreach (DataColumn col in dt.Tables[1].Columns)
                {
                    childRow.Add(col.ColumnName, row[col]);
                }
                parentRow.Add(childRow);
            }
            jsSerializer.MaxJsonLength = Int32.MaxValue;

            resultData = parentRow;
            result = new ContentResult
            {
                Content = jsSerializer.Serialize(resultData),
                ContentType = "application/json"
            };

            JsonResult result2 = Json(result);
            result1.MaxJsonLength = Int32.MaxValue;
            result1.JsonRequestBehavior = JsonRequestBehavior.AllowGet;


            //return json data object with HIL details
            return Json(new { data1 = result1, data2 = result2 }, JsonRequestBehavior.AllowGet);
        }


        public List<LabInfo> loadJsonObjects_LabID()
        {
            try
            {
                List<LabInfo> lstLabDef = new List<LabInfo>();
                //string jsonlabs = System.IO.File.ReadAllText(LabInfoPath);
                //jsonlabs = jsonlabs.Replace("\\", string.Empty);
                //jsonlabs = jsonlabs.Trim('"');
                //lstLabDef = JsonConvert.DeserializeObject<List<WrapperLab>>(jsonlabs);
                using (BookingServerReplicaEntities db = new BookingServerReplicaEntities())
                {
                    lstLabDef = db.LabInfoes.ToList();
                }
                var chk = false;

                lstLabDef.Sort((a, b) => a.Id.CompareTo(b.Id));


                return lstLabDef;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        //public List<WrapperSite> loadJsonObjects_Sites()
        //{
        //    try
        //    {
        //        //List<WrapperSite> objSites = new List<WrapperSite>();
        //        //string jsonSites = System.IO.File.ReadAllText(LabSitesPath);
        //        //jsonSites = jsonSites.Replace("\\", string.Empty);
        //        //jsonSites = jsonSites.Trim('"');
        //        //objSites = JsonConvert.DeserializeObject<List<WrapperSite>>(jsonSites);
        //        //return objSites;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

        ////Added- 7.6.2021 Monday - for Location Lab ID Filtering
        //public List<WrapperLocation> loadJsonObjects_Locations()
        //{
        //    try
        //    {

        //        //List<WrapperLocation> lstLabLocs = new List<WrapperLocation>();
        //        //string jsonLocs = System.IO.File.ReadAllText(LabLocationsPath);
        //        //jsonLocs = jsonLocs.Replace("\\", string.Empty);
        //        //jsonLocs = jsonLocs.Trim('"');
        //        //lstLabLocs = JsonConvert.DeserializeObject<List<WrapperLocation>>(jsonLocs);

        //        //return lstLabLocs;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

        /// <summary>
        /// Function to initialise the main data objects for lab information
        /// </summary>
        public DiagnosticsParam.LabBookingExport initDataObject()
        {
            #region Fill main Export Object
            DiagnosticsParam.LabBookingExport objLabExport = new DiagnosticsParam.LabBookingExport();
            List<LabInfo> lstLabDef = new List<LabInfo>();
            List<Site> objSites = new List<Site>();
            List<Location> lstLabLocs = new List<Location>();
            using (BookingServerReplicaEntities db = new BookingServerReplicaEntities())
            {
                lstLabDef = db.LabInfoes.ToList(); /*loadJsonObjects_LabID();*/
                objSites = db.Sites.ToList(); /*loadJsonObjects_Sites();*/
                lstLabLocs = db.Locations.ToList(); /*loadJsonObjects_Locations();*/
            }


            if (objLabExport.Labs == null)
            {
                objLabExport.Labs = new DiagnosticsParam.LabBookingExportLab[lstLabDef.FindAll(x => x.TypeId == 8 || x.TypeId == 9).FindAll(x => x.LocationId != 0).Count];
                int count = 0;
                foreach (var lab in lstLabDef.FindAll(x => x.TypeId == 8 || x.TypeId == 9).FindAll(x => x.LocationId != 0))
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


        //[HttpPost]
        //public ActionResult LCComparator()
        //{
        //    string Query = string.Empty;
        //    DataTable dt = new DataTable();
        //    string constring = ConfigurationManager.ConnectionStrings["PromasterImport_HWInfoEntities_ado"].ToString();
        //    con = new SqlConnection(constring);
        //    OpenConnection();
        //    Query = "Exec [dbo].[LC_Comparator]";
        //    SqlCommand cmd = new SqlCommand(Query,con);
        //    cmd.CommandTimeout = 1000;
        //    SqlDataAdapter da = new SqlDataAdapter(cmd);
        //    da.Fill(dt);
        //    CloseConnection();
        //    return Json(new {data = Query },JsonRequestBehavior.AllowGet );
        //}

        [HttpPost]
        public ActionResult GetLabIDs(string[] LabTypes, string[] Locations)
        {
            using (BookingServerReplicaEntities db = new BookingServerReplicaEntities())
            {
                List<Models.Site> objSites = db.Sites.ToList();
                List<Models.Location> objLabLocs = db.Locations.ToList();
                List<Models.LabType> objLabTypes = db.LabTypes.ToList();
                List<Models.LabInfo> objLabInfo = db.LabInfoes.ToList();

                //List<LabInfo> labinfos = new List<LabInfo>();
                List<SelectListItem> lstLabIDs = new List<SelectListItem>();

                foreach (string vartype in LabTypes)
                {
                    foreach (string varloc in Locations)
                    {
                        if (vartype.Trim() != string.Empty && varloc.Trim() != string.Empty)
                        {
                            var result = from a in db.LabInfoes
                                         join h in db.LabTypes on a.TypeId equals h.Id
                                         join c in db.Locations on a.LocationId equals c.Id
                                         join s in db.Sites on c.SiteId equals s.Id
                                         where h.DisplayName == vartype && s.RbCode == varloc
                                         orderby a.DisplayName
                                         select a;
                            //select new
                            //{
                            //    a.Id,
                            //    a.DisplayName,
                            //};
                            IEnumerable<LabInfo> labinfos = result.ToList().OrderBy(x => x.DisplayName);

                            foreach (Models.LabInfo labattr in labinfos)
                            {
                                lstLabIDs.Add(new SelectListItem { Text = labattr.DisplayName, Value = labattr.Id.ToString() });
                            }
                        }
                    }
                }


                //lstLabIDs.Sort((a, b) => a.Value.CompareTo(b.Value));
                return Json(new { data = lstLabIDs }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpGet]
        public ActionResult GetLabTypes()
        {
            using (BookingServerReplicaEntities db = new BookingServerReplicaEntities())
            {
                //List<Models.Site> objSites = db.Sites.ToList();
                //List<Models.Location> lstLabLocs = db.Locations.ToList();
                List<Models.LabType> objLabType = db.LabTypes.ToList().FindAll(x => x.Id == 8 || x.Id == 9);
                //List<LabInfo> lstLabDef = db.LabInfoes.ToList();

                List<SelectListItem> lstLabTypes = new List<SelectListItem>();
                objLabType = objLabType.GroupBy(x => x.DisplayName).Select(x => x.First()).ToList(); ;
                foreach (Models.LabType labattr in objLabType)
                {
                    lstLabTypes.Add(new SelectListItem { Text = labattr.DisplayName.Trim(), Value = labattr.DisplayName.Trim() });

                }
                lstLabTypes.Sort((a, b) => a.Value.CompareTo(b.Value));
                return Json(new { data = lstLabTypes }, JsonRequestBehavior.AllowGet);
            }

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

                        comp = db1.LabComputersPrs.ToList().FindAll(x => x.LabId == id && x.IsActive == true).ToList();


                        foreach (var info in comp)
                        {
                            //kept isactive in db so that we can get all pcs mapped till date to the labcar selected
                            //didnt filter by isactive = 1 since if we consider this condition, we wont get old (legacy) data since there months of offset w.r.t. data
                            var pcname = info.FQDN.Split('.')[0].ToUpper().Trim();

                            lstPCs.Add(pcname);
                        }
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
        //[HttpPost]
        //public ActionResult GetTABLE1_HWDescData(List<string> pc, DateTime sdate, DateTime edate)
        //{
        //    List<TABLE1_HwDataView> hwDataViews = new List<TABLE1_HwDataView>();
        //    if (pc != null && pc.Count() > 0)
        //    {
        //        try
        //        {
        //            using (PromasterImport_HWInfoEntities db = new PromasterImport_HWInfoEntities())
        //            {
        //                db.Database.CommandTimeout = 500;
        //                string date = string.Empty;
        //                for (var dt = sdate; dt <= edate; dt = dt.AddDays(1))
        //                {
        //                    date = dt.ToString("yyyy-MM-dd");

        //                    foreach (var pcname in pc)
        //                    {
        //                        //var pcname = pcname1.FQDN.Split('.')[0].ToUpper().Trim();
        //                        if (pcname != null && pcname != string.Empty && pcname.Count() > 0)
        //                        {

        //                            var hwItems = (from item in db.Hardware_Desc_Pr orderby item.Date where item.System_name.Contains(pcname) && item.Date.Contains(date) select new { item.EEPName, item.Date, item.EEPBuildDate, item.EEPDatabaseVersion, item.LabCarType, item.PCHostName, item.System_name, item.RBCCAFVersion, item.RTPCName, item.RTPCSoftwareVersion, item.ToolChainVersion }).ToList();
        //                            //var hwItems_1 = (from item in db.Hardware_Desc_Pr
        //                            //               orderby item.Date
        //                            //               where item.System_name.Contains(pcname) && item.Date.Contains(date)
        //                            //               select item).Distinct().ToList();
        //                            //var x = db.Hardware_Desc_Pr.ToList().FindAll(xi=>xi.System_name.Contains(pcname)).FindAll(ci=>ci.Date.Contains(date)).ToList()
        //                            //var hwItems_wodistinct_specificcol = (from item in db.Hardware_Desc_Pr orderby item.Date where item.System_name.Contains(pcname) && item.Date.Contains(date) select item.EEPName).ToList();

        //                            foreach (var item in hwItems)
        //                            {

        //                                //var result = (from b in db.Hardware_Desc_Pr
        //                                //                  //join h in db.LabTypes on a.TypeId equals h.Id
        //                                //                  //join c in db.Locations on a.LocationId equals c.Id
        //                                //                  //join s in db.Sites on c.SiteId equals s.Id
        //                                //              where b.EEPName == j && b.Date == date
        //                                //              orderby b.Date
        //                                //              select new
        //                                //              {
        //                                //                  EEPBuildDate = b.EEPBuildDate,
        //                                //                  EEPName = b.EEPName,

        //                                //              }).Distinct();


        //                                TABLE1_HwDataView obj = new TABLE1_HwDataView();
        //                                obj.System_name = item.System_name;

        //                                //////string apistatus = LabBookingWrapper.APIInit("tracker");
        //                                //////if (apistatus.Contains("SUCCESS"))
        //                                //////{
        //                                //////    WrapperComputer wrapperComputers = new WrapperComputer();
        //                                //////    wrapperComputers = LabBookingWrapper.GetComputerByFQDN(pcname1);
        //                                //////    if (wrapperComputers.successMsg.Contains("SUCCESS"))
        //                                //////    {
        //                                //////        var labid = wrapperComputers.LabId;

        //                                //////        WrapperLab wrapperlab = new WrapperLab();
        //                                //////        wrapperlab = LabBookingWrapper.GetLabByLabID(labid);
        //                                //////        if (wrapperlab.successMsg.Contains("SUCCESS"))
        //                                //////        {
        //                                //////            var labname = wrapperlab.DisplayName;
        //                                //////            obj.LC_Name = labname;
        //                                //////        }
        //                                //////    }

        //                                //////}


        //                                using (BookingServerReplicaEntities db1 = new BookingServerReplicaEntities())
        //                                {
        //                                    var chk = db1.LabComputersPrs.ToList();
        //                                    var chk1 = db1.LabComputersPrs.Select(v => v.FQDN).ToList();

        //                                    var labid = db1.LabComputersPrs.ToList().Find(x => x.FQDN.Split('.')[0].ToUpper().Contains(obj.System_name)).LabId;//Find(x => x.DisplayName.ToString() == obj.System_name.ToString()).LabId;                                     
        //                                    obj.LC_Name = db1.LabInfoes.FirstOrDefault(i => i.Id == labid).DisplayName;
        //                                    //var chkid8 = db1.LabInfoes.FirstOrDefault(i => i.DisplayName == "LC0008").Id;
        //                                    //var chklabid8 = db1.LabComputersPrs.ToList().Find(x => x.LabId == (chkid8)).FQDN;
        //                                }


        //                                //////WrapperComputers CompinfoTest = new WrapperComputers();
        //                                //////                                    CompinfoTest = LabBookingWrapper.GetComputersByLab(lab.Id);
        //                                //////                                    //CompinfoTest.successMsg = string.Empty;

        //                                //////                                    if (CompinfoTest.successMsg.Contains("SUCCESS"))
        //                                //////                                        foreach (WrapperComputer comp in CompinfoTest.ComputersList)
        //                                //////                                        {

        //                                //////                                            LabComputersPr c = new LabComputersPr();
        //                                //////                                            c.Description = comp.Description;
        //                                //////                                            c.DisplayName = comp.DisplayName;
        //                                //////                                            c.Id = comp.Id;
        //                                //////                                            c.LocationId = comp.LocationId;
        //                                //////                                            c.FQDN = comp.FQDN;
        //                                //////                                            c.LabId = comp.LabId;
        //                                //////                                            c.TrackerConfigId = comp.TrackerConfigId;
        //                                //////                                            if (db.LabComputersPr.Where(item => item.Id == comp.Id).Count() == 0)
        //                                //////                                            {
        //                                //////                                                Console.WriteLine("Adding COMPUTER entry for : " + comp.DisplayName + "-" + comp.Id);
        //                                //////                                                db.LabComputersPr.Add(c);
        //                                //////                                                db.SaveChanges();
        //                                //////                                            }
        //                                //////                                            //else if (db.LabComputersPr.Where(item => item.LabId == comp.LabId).First() != c)
        //                                //////                                            //{
        //                                //////                                            //    Console.WriteLine("Editing LAB entry for : " + c.DisplayName + "-" + c.Id);
        //                                //////                                            //    db.Entry(c).State = EntityState.Modified;
        //                                //////                                            //    db.SaveChanges();
        //                                //////                                            //}
        //                                //////                                            //WrapperActivities Actinfo = new WrapperActivities();
        //                                //////                                            //Actinfo.successMsg = string.Empty;
        //                                //////                                            //Actinfo = LabBookingWrapper.GetActivitiesByComputer(comp.Id, DateTime.Now.AddDays(-10), DateTime.Now);
        //                                //////                                            //if (Actinfo.successMsg.Contains("SUCCESS"))
        //                                //////                                            //    foreach (WrapperActivity act in Actinfo.ActivitiesList)
        //                                //////                                            //    {

        //                                //////                                            //        tactivities a = new tactivities();
        //                                //////                                            //        a.ComputerId = act.ComputerId;
        //                                //////                                            //        a.EndDate = (int)UnixBasedUTCDateConverter.ToUnixTimestamp(act.EndDate);
        //                                //////                                            //        a.Expired = act.Expired ? 1 : 0; ;
        //                                //////                                            //        a.Info = act.Info;
        //                                //////                                            //        a.IsActive = act.IsActive?1:0;
        //                                //////                                            //        a.SessionId = act.SessionId;
        //                                //////                                            //        a.StartDate = (int)UnixBasedUTCDateConverter.ToUnixTimestamp(act.StartDate);
        //                                //////                                            //        a.Type = (int)act.Type;
        //                                //////                                            //        if (!(db.tactivities.Where(x => x.ComputerId == a.ComputerId && x.StartDate == a.StartDate && x.EndDate == a.EndDate).Count()>0))
        //                                //////                                            //        {
        //                                //////                                            //            Console.WriteLine("Adding ACTIVITY entry for : " + act.Type.ToString() + "-" + act.SessionId);
        //                                //////                                            //            db.tactivities.Add(a);
        //                                //////                                            //            db.SaveChanges();
        //                                //////                                            //        }
        //                                //////                                            //    }
        //                                //////                                        }




        //                                obj.PCHostName = item.PCHostName;
        //                                obj.Date = DateTime.Parse(item.Date).ToShortDateString();
        //                                obj.EEPName = item.EEPName;
        //                                obj.EEPBuildDate = DateTime.Parse(item.EEPBuildDate).ToShortDateString();
        //                                obj.EEPDatabaseVersion = item.EEPDatabaseVersion;
        //                                obj.RTPCName = item.RTPCName;
        //                                obj.RBCCAFVersion = item.RBCCAFVersion;
        //                                obj.ToolChainVersion = item.ToolChainVersion;
        //                                obj.RTPCSoftwareVersion = item.RTPCSoftwareVersion;
        //                                obj.LabCarType = item.LabCarType;
        //                                hwDataViews.Add(obj);
        //                            }
        //                        }

        //                    }

        //                }
        //            }
        //            return Json(new { data = hwDataViews }, JsonRequestBehavior.AllowGet);

        //        }
        //        catch (Exception Ex)
        //        {
        //            return View("Unable to retrieve data from database. Please try again later!!");
        //        }
        //    }
        //    else
        //        return Json(new { success = false, message = "Try again" }, JsonRequestBehavior.AllowGet);
        //}

        //[HttpPost]
        //public ActionResult TABLE2_GetPrjDescData(List<string> pc, DateTime sdate, DateTime edate)
        //{
        //    List<TABLE2_ProjectDescriptionView> PrjDataViews = new List<TABLE2_ProjectDescriptionView>();
        //    if (pc != null && pc.Count() > 0)
        //    {
        //        try
        //        {
        //            using (PromasterImport_ProjectDescEntities db = new PromasterImport_ProjectDescEntities())
        //            {
        //                db.Database.CommandTimeout = 500;
        //                string date = string.Empty;
        //                for (var dt = sdate; dt <= edate; dt = dt.AddDays(1))
        //                {
        //                    date = dt.ToString("yyyy-MM-dd");

        //                    foreach (var pcname in pc)
        //                    {
        //                        //var pcname = pcname1.FQDN.Split('.')[0].ToUpper().Trim();
        //                        if (pcname != null && pcname != string.Empty && pcname.Count() > 0)
        //                        {

        //                            var PrjItems = (from item in db.ProjectDescription_Pr_1 orderby item.Date where item.System_name.Contains(pcname) && item.Date.Contains(date) select new { item.AECUCable, item.Ascet_Ver, item.Component_name, item.Date, item.Db_Version, item.Details, item.EMUCable, item.MetaEditor_Ver, item.ProjectBuilder_Ver, item.ProjectEditor_Ver, item.System_name, item.ToolVersion, item.Version });

        //                            //var PrjItems_takingUniqueComponent_Details = (from item in db.ProjectDescription_Pr_1 orderby item.Date where item.System_name.Contains(pcname) && item.Date.Contains(date) select new { item.Component_name, item.Details}).Distinct();
        //                            //var PrjItems_takingUniqueComponent = (from item in db.ProjectDescription_Pr_1 orderby item.Date where item.System_name.Contains(pcname) && item.Date.Contains(date) select new { item.Component_name}).Distinct().ToList();
        //                            //var PrjItems_takingUniqueDetail = (from item in db.ProjectDescription_Pr_1 orderby item.Date where item.System_name.Contains(pcname) && item.Date.Contains(date) select new { item.Details }).Distinct().ToList();



        //                            foreach (var item in PrjItems)
        //                            {
        //                                TABLE2_ProjectDescriptionView obj = new TABLE2_ProjectDescriptionView();

        //                                obj.Component_name = item.Component_name;
        //                                obj.Date = item.Date;
        //                                obj.Db_Version = item.Db_Version;
        //                                obj.Details = item.Details;
        //                                obj.System_name = item.System_name;

        //                                using (BookingServerReplicaEntities db1 = new BookingServerReplicaEntities())
        //                                {

        //                                    var labid = db1.LabComputersPrs.ToList().Find(x => x.FQDN.Split('.')[0].ToUpper().Contains(obj.System_name)).LabId;
        //                                    obj.LC_Name = db1.LabInfoes.FirstOrDefault(i => i.Id == labid).DisplayName;
        //                                }

        //                                obj.Version = item.Version;

        //                                obj.ToolVersion = item.ToolVersion;
        //                                obj.EMUCable = item.EMUCable;
        //                                obj.AECUCable = item.AECUCable;
        //                                obj.MetaEditor_Ver = item.MetaEditor_Ver;
        //                                obj.ProjectBuilder_Ver = item.ProjectBuilder_Ver;
        //                                obj.Ascet_Ver = item.Ascet_Ver;
        //                                obj.ProjectEditor_Ver = item.ProjectEditor_Ver;


        //                                PrjDataViews.Add(obj);
        //                            }
        //                        }
        //                    }
        //                }
        //                return Json(new { data = PrjDataViews }, JsonRequestBehavior.AllowGet);
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

        //[HttpPost]
        //public ActionResult GetTABLE3_APB_BOB_Cable_EB_ES4441_1_ES4441_2_HAP1Data(List<string> pc, DateTime sdate, DateTime edate)
        //{
        //    List<TABLE3_APB_BOB_Cable_EB_ES4441_1_ES4441_2_HAP1View> table3DataViews = new List<TABLE3_APB_BOB_Cable_EB_ES4441_1_ES4441_2_HAP1View>();
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
        //                    foreach (var pcname in pc)
        //                    {
        //                        //var pcname = pcname1.FQDN.Split('.')[0].ToUpper().Trim();
        //                        if (pcname != null && pcname != string.Empty && pcname.Count() > 0)
        //                        {

        //                            var apbItems = (from item in db.APB_Pr orderby item.Date where item.System_name.Contains(pcname) && item.Date.Contains(date) select new { item.Date, item.ModelSpecific_EID_Switch_APB_APB_Module_GIO2_Signature_GIO2, item.System_name });
        //                            var bobItems = (from item in db.BreakOutBox_Pr orderby item.Date where item.System_name.Contains(pcname) && item.Date.Contains(date) select new { item.Date, item.Kernel_Loadbox_Card_ECC_BreakOutBox_BOB_DeviceFunction, item.Kernel_Loadbox_Card_ECC_BreakOutBox_BOB_DeviceRevision, item.Kernel_Loadbox_Card_ECC_BreakOutBox_BOB_OptionalDescription, item.Kernel_Loadbox_Card_ECC_BreakOutBox_BOB_OrderNumber, item.Kernel_Loadbox_Card_ECC_BreakOutBox_BOB_SerialNumber, item.System_name });
        //                            var cableItems = (from item in db.Cable_Pr orderby item.Date where item.System_name.Contains(pcname) && item.Date.Contains(date) select new { item.Date, item.Kernel_Loadbox_Card_ECC_Cable_CABLE_DeviceFunction, item.Kernel_Loadbox_Card_ECC_Cable_CABLE_DeviceRevision, item.Kernel_Loadbox_Card_ECC_Cable_CABLE_OptionalDescription, item.Kernel_Loadbox_Card_ECC_Cable_CABLE_OrderNumber, item.Kernel_Loadbox_Card_ECC_Cable_CABLE_SerialNumber, item.System_name });
        //                            var ebItems = (from item in db.EB_Cards_Pr orderby item.Date where item.System_name.Contains(pcname) && item.Date.Contains(date) select new { item.Date, item.Kernel_EB_Cards_EB5100, item.Kernel_EB_Cards_EB5200, item.System_name });
        //                            var es4441_1Items = (from item in db.ES4441_1_Pr orderby item.Date where item.System_name.Contains(pcname) && item.Date.Contains(date) select new { item.Date, item.Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441FWVersionMain, item.Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441FWVersionMajor, item.Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441FWVersionMinor, item.Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441PLDVersion, item.Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441SN, item.System_name });
        //                            var es4441_2Items = (from item in db.ES4441_2_Pr orderby item.Date where item.System_name.Contains(pcname) && item.Date.Contains(date) select new { item.Date, item.Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441HWRevision, item.Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441HWVersion, item.System_name });
        //                            var hap1Items = (from item in db.Harnesadapter_1_Pr orderby item.Date where item.System_name.Contains(pcname) && item.Date.Contains(date) select new { item.Date, item.Kernel_Loadbox_Card_ECC_Harnesadapter_HAP_SerialNumber, item.System_name });


        //                            for (int i = 0; i < apbItems.Count(); i++)
        //                            {
        //                                TABLE3_APB_BOB_Cable_EB_ES4441_1_ES4441_2_HAP1View obj = new TABLE3_APB_BOB_Cable_EB_ES4441_1_ES4441_2_HAP1View();
        //                                obj.Date = DateTime.Parse(apbItems.ToArray()[i].Date).ToShortDateString();
        //                                obj.System_name = apbItems.ToArray()[i].System_name;

        //                                using (BookingServerReplicaEntities db1 = new BookingServerReplicaEntities())
        //                                {

        //                                    var labid = db1.LabComputersPrs.ToList().Find(x => x.FQDN.Split('.')[0].ToUpper().Contains(obj.System_name)).LabId;
        //                                    obj.LC_Name = db1.LabInfoes.FirstOrDefault(j => j.Id == labid).DisplayName;
        //                                }

        //                                obj.ModelSpecific_EID_Switch_APB_APB_Module_GIO2_Signature_GIO2 = apbItems.ToArray()[i].ModelSpecific_EID_Switch_APB_APB_Module_GIO2_Signature_GIO2;
        //                                obj.Kernel_Loadbox_Card_ECC_Harnesadapter_HAP_SerialNumber = hap1Items.ToArray()[i].Kernel_Loadbox_Card_ECC_Harnesadapter_HAP_SerialNumber;
        //                                obj.Kernel_Loadbox_Card_ECC_Cable_CABLE_SerialNumber = cableItems.ToArray()[i].Kernel_Loadbox_Card_ECC_Cable_CABLE_SerialNumber;
        //                                obj.Kernel_Loadbox_Card_ECC_Cable_CABLE_DeviceRevision = cableItems.ToArray()[i].Kernel_Loadbox_Card_ECC_Cable_CABLE_DeviceRevision;
        //                                obj.Kernel_Loadbox_Card_ECC_BreakOutBox_BOB_SerialNumber = bobItems.ToArray()[i].Kernel_Loadbox_Card_ECC_BreakOutBox_BOB_SerialNumber;
        //                                obj.Kernel_Loadbox_Card_ECC_BreakOutBox_BOB_DeviceRevision = bobItems.ToArray()[i].Kernel_Loadbox_Card_ECC_BreakOutBox_BOB_DeviceRevision;
        //                                obj.Kernel_EB_Cards_EB5200 = ebItems.ToArray()[i].Kernel_EB_Cards_EB5200;
        //                                obj.Kernel_EB_Cards_EB5100 = ebItems.ToArray()[i].Kernel_EB_Cards_EB5100;
        //                                obj.Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441SN = es4441_1Items.ToArray()[i].Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441SN;
        //                                obj.Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441FWVersionMinor = es4441_1Items.ToArray()[i].Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441FWVersionMinor;
        //                                obj.Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441FWVersionMajor = es4441_1Items.ToArray()[i].Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441FWVersionMajor;
        //                                obj.Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441FWVersionMain = es4441_1Items.ToArray()[i].Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441FWVersionMain;
        //                                obj.Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441HWRevision = es4441_2Items.ToArray()[i].Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441HWRevision;
        //                                obj.Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441HWVersion = es4441_2Items.ToArray()[i].Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441HWVersion;

        //                                table3DataViews.Add(obj);
        //                            }
        //                        }
        //                    }
        //                }
        //                return Json(new { data = table3DataViews }, JsonRequestBehavior.AllowGet);
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

        //[HttpPost]
        //public ActionResult GetTABLE4_OTSO_BOB1_Cable1_PowerSupply_HAP_IXXATData(List<string> pc, DateTime sdate, DateTime edate)
        //{
        //    List<TABLE4_OTSO_BOB1_Cable1_PowerSupply_HAP_IXXATView> table4DataViews = new List<TABLE4_OTSO_BOB1_Cable1_PowerSupply_HAP_IXXATView>();
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
        //                    foreach (var pcname in pc)
        //                    {
        //                        //var pcname = pcname1.FQDN.Split('.')[0].ToUpper().Trim();
        //                        if (pcname != null && pcname != string.Empty && pcname.Count() > 0)
        //                        {

        //                            var otsoItems = (from item in db.OTSO_Pr orderby item.Date where item.System_name.Contains(pcname) && item.Date.Contains(date) select new { item.Date, item.Kernel_Component_IPB_TemperatureReceive_OTSO1Available, item.Kernel_Component_IPB_TemperatureReceive_OTSO2Available, item.System_name });
        //                            var bob1Items = (from item in db.BreakOutBox_1_Pr orderby item.Date where item.System_name.Contains(pcname) && item.Date.Contains(date) select new { item.Date, item.Kernel_Loadbox_Card_ECC_BreakOutBox_BOB_SerialNumber, item.System_name });
        //                            var cable1Items = (from item in db.Cable_1_Pr orderby item.Date where item.System_name.Contains(pcname) && item.Date.Contains(date) select new { item.Date, item.Kernel_Loadbox_Card_ECC_Cable_CABLE_SerialNumber, item.System_name });
        //                            var hapItems = (from item in db.Harnesadapter_Pr orderby item.Date where item.System_name.Contains(pcname) && item.Date.Contains(date) select new { item.Date, item.Kernel_Loadbox_Card_ECC_Harnesadapter_HAP_DeviceFunction, item.Kernel_Loadbox_Card_ECC_Harnesadapter_HAP_DeviceRevision, item.Kernel_Loadbox_Card_ECC_Harnesadapter_HAP_OptionalDescription, item.Kernel_Loadbox_Card_ECC_Harnesadapter_HAP_OrderNumber, item.Kernel_Loadbox_Card_ECC_Harnesadapter_HAP_SerialNumber, item.System_name });
        //                            var ixxatItems = (from item in db.IXXAT_Config_Pr orderby item.Date where item.System_name.Contains(pcname) && item.Date.Contains(date) select new { item.Date, item.ModelSpecific_EID_NET_CAN_Settings_CAN_Settings_rtpc_ixxat_config, item.System_name });
        //                            var powersupplyItems = (from item in db.Power_Supply_Pr orderby item.Date where item.System_name.Contains(pcname) && item.Date.Contains(date) select new { item.Date, item.Kernel_Loadbox_Card_PSC_Power_Supply_Configuration_PS_Configuration, item.Kernel_Loadbox_Card_PSC_Power_Supply_Configuration_PS_Factor_ECU, item.Kernel_Loadbox_Card_PSC_Power_Supply_Configuration_PS_Factor_MR, item.Kernel_Loadbox_Card_PSC_Power_Supply_Configuration_PS_Factor_VR, item.System_name });


        //                            for (int i = 0; i < otsoItems.Count(); i++)
        //                            {
        //                                TABLE4_OTSO_BOB1_Cable1_PowerSupply_HAP_IXXATView obj = new TABLE4_OTSO_BOB1_Cable1_PowerSupply_HAP_IXXATView();
        //                                obj.Date = DateTime.Parse(otsoItems.ToArray()[i].Date).ToShortDateString();
        //                                obj.System_name = otsoItems.ToArray()[i].System_name;

        //                                using (BookingServerReplicaEntities db1 = new BookingServerReplicaEntities())
        //                                {

        //                                    var labid = db1.LabComputersPrs.ToList().Find(x => x.FQDN.Split('.')[0].ToUpper().Contains(obj.System_name)).LabId;//Find(x => x.DisplayName.ToString() == obj.System_name.ToString()).LabId;
        //                                    obj.LC_Name = db1.LabInfoes.FirstOrDefault(j => j.Id == labid).DisplayName;
        //                                }

        //                                obj.Kernel_Component_IPB_TemperatureReceive_OTSO1Available = otsoItems.ToArray()[i].Kernel_Component_IPB_TemperatureReceive_OTSO1Available;
        //                                obj.Kernel_Component_IPB_TemperatureReceive_OTSO2Available = otsoItems.ToArray()[i].Kernel_Component_IPB_TemperatureReceive_OTSO2Available;

        //                                obj.Kernel_Loadbox_Card_ECC_BreakOutBox_BOB_SerialNumber = bob1Items.ToArray()[i].Kernel_Loadbox_Card_ECC_BreakOutBox_BOB_SerialNumber;
        //                                obj.Kernel_Loadbox_Card_ECC_Cable_CABLE_SerialNumber = cable1Items.ToArray()[i].Kernel_Loadbox_Card_ECC_Cable_CABLE_SerialNumber;

        //                                obj.Kernel_Loadbox_Card_PSC_Power_Supply_Configuration_PS_Configuration = powersupplyItems.ToArray()[i].Kernel_Loadbox_Card_PSC_Power_Supply_Configuration_PS_Configuration;
        //                                obj.Kernel_Loadbox_Card_PSC_Power_Supply_Configuration_PS_Factor_ECU = powersupplyItems.ToArray()[i].Kernel_Loadbox_Card_PSC_Power_Supply_Configuration_PS_Factor_ECU;
        //                                obj.Kernel_Loadbox_Card_PSC_Power_Supply_Configuration_PS_Factor_MR = powersupplyItems.ToArray()[i].Kernel_Loadbox_Card_PSC_Power_Supply_Configuration_PS_Factor_MR;
        //                                obj.Kernel_Loadbox_Card_PSC_Power_Supply_Configuration_PS_Factor_VR = powersupplyItems.ToArray()[i].Kernel_Loadbox_Card_PSC_Power_Supply_Configuration_PS_Factor_VR;

        //                                obj.Kernel_Loadbox_Card_ECC_Harnesadapter_HAP_SerialNumber = hapItems.ToArray()[i].Kernel_Loadbox_Card_ECC_Harnesadapter_HAP_SerialNumber;
        //                                obj.Kernel_Loadbox_Card_ECC_Harnesadapter_HAP_DeviceRevision = hapItems.ToArray()[i].Kernel_Loadbox_Card_ECC_Harnesadapter_HAP_DeviceRevision;
        //                                obj.ModelSpecific_EID_NET_CAN_Settings_CAN_Settings_rtpc_ixxat_config = ixxatItems.ToArray()[i].ModelSpecific_EID_NET_CAN_Settings_CAN_Settings_rtpc_ixxat_config;
        //                                table4DataViews.Add(obj);
        //                            }
        //                        }
        //                    }
        //                }
        //                return Json(new { data = table4DataViews }, JsonRequestBehavior.AllowGet);
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

        //[HttpPost]
        //public ActionResult GetTABLE5_ECC_IBData(List<string> pc, DateTime sdate, DateTime edate)
        //{
        //    List<TABLE5_ECC_IBView> table5DataViews = new List<TABLE5_ECC_IBView>();
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
        //                    foreach (var pcname in pc)
        //                    {
        //                        //var pcname = pcname1.FQDN.Split('.')[0].ToUpper().Trim();
        //                        if (pcname != null && pcname != string.Empty && pcname.Count() > 0)
        //                        {

        //                            var eccItems = (from item in db.ECC_Card_Pr orderby item.Date where item.System_name.Contains(pcname) && item.Date.Contains(date) select new { item.Date, item.Kernel_Loadbox_Card_ECC_BuildVersion, item.Kernel_Loadbox_Card_ECC_Calibration_Day, item.Kernel_Loadbox_Card_ECC_Calibration_Month, item.Kernel_Loadbox_Card_ECC_Calibration_Year, item.Kernel_Loadbox_Card_ECC_CARD_BoardRevision, item.Kernel_Loadbox_Card_ECC_CARD_BoardType, item.Kernel_Loadbox_Card_ECC_CARD_Description, item.Kernel_Loadbox_Card_ECC_CARD_VER_TAG_ARRAY, item.Kernel_Loadbox_Card_ECC_Day_of_Creation, item.Kernel_Loadbox_Card_ECC_Firmware_Card_Major, item.Kernel_Loadbox_Card_ECC_Firmware_Card_Minor, item.Kernel_Loadbox_Card_ECC_Firmware_FPGA_Major, item.Kernel_Loadbox_Card_ECC_Firmware_FPGA_Minor, item.Kernel_Loadbox_Card_ECC_Month_of_Creation, item.Kernel_Loadbox_Card_ECC_Next_Calibration_Day, item.Kernel_Loadbox_Card_ECC_Next_Calibration_Month, item.Kernel_Loadbox_Card_ECC_Next_Calibration_Year, item.Kernel_Loadbox_Card_ECC_PowerOnTime, item.Kernel_Loadbox_Card_ECC_RepairCounter, item.Kernel_Loadbox_Card_ECC_Serial_No, item.Kernel_Loadbox_Card_ECC_Year_of_Creation, item.System_name });
        //                            var ibItems = (from item in db.IB_Card_Pr orderby item.Date where item.System_name.Contains(pcname) && item.Date.Contains(date) select new { item.Date, item.ModelSpecific_EID_NET_CAN_Settings_CAN_Settings_rtpc_ixxat_config, item.System_name });

        //                            for (int i = 0; i < eccItems.Count(); i++)
        //                            {
        //                                TABLE5_ECC_IBView obj = new TABLE5_ECC_IBView();
        //                                obj.Date = DateTime.Parse(eccItems.ToArray()[i].Date).ToShortDateString();
        //                                obj.System_name = eccItems.ToArray()[i].System_name;

        //                                using (BookingServerReplicaEntities db1 = new BookingServerReplicaEntities())
        //                                {

        //                                    var labid = db1.LabComputersPrs.ToList().Find(x => x.FQDN.Split('.')[0].ToUpper().Contains(obj.System_name)).LabId;//Find(x => x.DisplayName.ToString() == obj.System_name.ToString()).LabId;
        //                                    LabInfo labInfo = new LabInfo();
        //                                    //labInfo = db.LabInfoes.Where(item => item.Id == labid).FirstOrDefault();
        //                                    //if (labInfo != null)
        //                                    // {
        //                                    obj.LC_Name = db1.LabInfoes.FirstOrDefault(j => j.Id == labid).DisplayName;
        //                                }

        //                                db.Database.CommandTimeout = 500;
        //                                obj.Kernel_Loadbox_Card_ECC_CARD_BoardType = eccItems.ToArray()[i].Kernel_Loadbox_Card_ECC_CARD_BoardType;
        //                                obj.Kernel_Loadbox_Card_ECC_CARD_BoardRevision = eccItems.ToArray()[i].Kernel_Loadbox_Card_ECC_CARD_BoardRevision;
        //                                obj.Kernel_Loadbox_Card_ECC_CARD_VER_TAG_ARRAY = eccItems.ToArray()[i].Kernel_Loadbox_Card_ECC_CARD_VER_TAG_ARRAY;
        //                                obj.Kernel_Loadbox_Card_ECC_Serial_No = eccItems.ToArray()[i].Kernel_Loadbox_Card_ECC_Serial_No;
        //                                obj.Kernel_Loadbox_Card_ECC_Firmware_Card_Major = eccItems.ToArray()[i].Kernel_Loadbox_Card_ECC_Firmware_Card_Major;
        //                                obj.Kernel_Loadbox_Card_ECC_Firmware_Card_Minor = eccItems.ToArray()[i].Kernel_Loadbox_Card_ECC_Firmware_Card_Minor;
        //                                obj.Kernel_Loadbox_Card_ECC_Firmware_FPGA_Major = eccItems.ToArray()[i].Kernel_Loadbox_Card_ECC_Firmware_FPGA_Major;
        //                                obj.Kernel_Loadbox_Card_ECC_Firmware_FPGA_Minor = eccItems.ToArray()[i].Kernel_Loadbox_Card_ECC_Firmware_FPGA_Minor;
        //                                obj.Kernel_Loadbox_Card_ECC_Calibration_Day = eccItems.ToArray()[i].Kernel_Loadbox_Card_ECC_Calibration_Day;
        //                                obj.Kernel_Loadbox_Card_ECC_Calibration_Month = eccItems.ToArray()[i].Kernel_Loadbox_Card_ECC_Calibration_Month;
        //                                obj.Kernel_Loadbox_Card_ECC_Calibration_Year = eccItems.ToArray()[i].Kernel_Loadbox_Card_ECC_Calibration_Year;
        //                                obj.Kernel_Loadbox_Card_ECC_Next_Calibration_Day = eccItems.ToArray()[i].Kernel_Loadbox_Card_ECC_Next_Calibration_Day;
        //                                obj.Kernel_Loadbox_Card_ECC_Next_Calibration_Month = eccItems.ToArray()[i].Kernel_Loadbox_Card_ECC_Next_Calibration_Month;
        //                                obj.Kernel_Loadbox_Card_ECC_Next_Calibration_Year = eccItems.ToArray()[i].Kernel_Loadbox_Card_ECC_Next_Calibration_Year;
        //                                obj.Kernel_Loadbox_Card_ECC_Day_of_Creation = eccItems.ToArray()[i].Kernel_Loadbox_Card_ECC_Day_of_Creation;
        //                                obj.Kernel_Loadbox_Card_ECC_Month_of_Creation = eccItems.ToArray()[i].Kernel_Loadbox_Card_ECC_Month_of_Creation;
        //                                obj.Kernel_Loadbox_Card_ECC_Year_of_Creation = eccItems.ToArray()[i].Kernel_Loadbox_Card_ECC_Year_of_Creation;
        //                                obj.Kernel_Loadbox_Card_ECC_BuildVersion = eccItems.ToArray()[i].Kernel_Loadbox_Card_ECC_BuildVersion;

        //                                if (ibItems.Count() > 0)
        //                                {
        //                                    obj.ModelSpecific_EID_NET_CAN_Settings_CAN_Settings_rtpc_ixxat_config = ibItems.ToArray()[i].ModelSpecific_EID_NET_CAN_Settings_CAN_Settings_rtpc_ixxat_config;

        //                                }

        //                                table5DataViews.Add(obj);
        //                            }
        //                        }
        //                    }
        //                }
        //                return Json(new { data = table5DataViews }, JsonRequestBehavior.AllowGet);
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

        //[HttpPost]
        //public ActionResult GetTABLE6_GIO1Data(List<string> pc, DateTime sdate, DateTime edate)
        //{
        //    List<TABLE6_GIO1View> table6DataViews = new List<TABLE6_GIO1View>();
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
        //                    foreach (var pcname in pc)
        //                    {
        //                        //var pcname = pcname1.FQDN.Split('.')[0].ToUpper().Trim();
        //                        if (pcname != null && pcname != string.Empty && pcname.Count() > 0)
        //                        {

        //                            var gio1Items = (from item in db.GIO1_Card_Pr orderby item.Date where item.System_name.Contains(pcname) && item.Date.Contains(date) select new { item.Date, item.Kernel_Loadbox_Card_GIO1_BuildVersion, item.Kernel_Loadbox_Card_GIO1_Calibration_Day, item.Kernel_Loadbox_Card_GIO1_Calibration_Month, item.Kernel_Loadbox_Card_GIO1_Calibration_Year, item.Kernel_Loadbox_Card_GIO1_CARD_BoardRevision, item.Kernel_Loadbox_Card_GIO1_CARD_BoardType, item.Kernel_Loadbox_Card_GIO1_CARD_Description, item.Kernel_Loadbox_Card_GIO1_CARD_VER_TAG_ARRAY, item.Kernel_Loadbox_Card_GIO1_Day_of_Creation, item.Kernel_Loadbox_Card_GIO1_Firmware_Card_Major, item.Kernel_Loadbox_Card_GIO1_Firmware_Card_Minor, item.Kernel_Loadbox_Card_GIO1_Firmware_FPGA_Major, item.Kernel_Loadbox_Card_GIO1_Firmware_FPGA_Minor, item.Kernel_Loadbox_Card_GIO1_Month_of_Creation, item.Kernel_Loadbox_Card_GIO1_Next_Calibration_Day, item.Kernel_Loadbox_Card_GIO1_Next_Calibration_Month, item.Kernel_Loadbox_Card_GIO1_Next_Calibration_Year, item.Kernel_Loadbox_Card_GIO1_PowerOnTime, item.Kernel_Loadbox_Card_GIO1_RepairCounter, item.Kernel_Loadbox_Card_GIO1_Serial_No, item.Kernel_Loadbox_Card_GIO1_Year_of_Creation, item.System_name });

        //                            for (int i = 0; i < gio1Items.Count(); i++)
        //                            {
        //                                TABLE6_GIO1View obj = new TABLE6_GIO1View();
        //                                obj.Date = DateTime.Parse(gio1Items.ToArray()[i].Date).ToShortDateString();
        //                                obj.System_name = gio1Items.ToArray()[i].System_name;

        //                                using (BookingServerReplicaEntities db1 = new BookingServerReplicaEntities())
        //                                {

        //                                    var labid = db1.LabComputersPrs.ToList().Find(x => x.FQDN.Split('.')[0].ToUpper().Contains(obj.System_name)).LabId;//Find(x => x.DisplayName.ToString() == obj.System_name.ToString()).LabId;
        //                                    LabInfo labInfo = new LabInfo();
        //                                    //labInfo = db.LabInfoes.Where(item => item.Id == labid).FirstOrDefault();
        //                                    //if (labInfo != null)
        //                                    // {
        //                                    obj.LC_Name = db1.LabInfoes.FirstOrDefault(j => j.Id == labid).DisplayName;
        //                                }

        //                                obj.Kernel_Loadbox_Card_GIO1_CARD_BoardType = gio1Items.ToArray()[i].Kernel_Loadbox_Card_GIO1_CARD_BoardType;
        //                                obj.Kernel_Loadbox_Card_GIO1_CARD_BoardRevision = gio1Items.ToArray()[i].Kernel_Loadbox_Card_GIO1_CARD_BoardRevision;
        //                                obj.Kernel_Loadbox_Card_GIO1_CARD_VER_TAG_ARRAY = gio1Items.ToArray()[i].Kernel_Loadbox_Card_GIO1_CARD_VER_TAG_ARRAY;
        //                                obj.Kernel_Loadbox_Card_GIO1_Serial_No = gio1Items.ToArray()[i].Kernel_Loadbox_Card_GIO1_Serial_No;
        //                                obj.Kernel_Loadbox_Card_GIO1_Firmware_Card_Major = gio1Items.ToArray()[i].Kernel_Loadbox_Card_GIO1_Firmware_Card_Major;
        //                                obj.Kernel_Loadbox_Card_GIO1_Firmware_Card_Minor = gio1Items.ToArray()[i].Kernel_Loadbox_Card_GIO1_Firmware_Card_Minor;
        //                                obj.Kernel_Loadbox_Card_GIO1_Firmware_FPGA_Major = gio1Items.ToArray()[i].Kernel_Loadbox_Card_GIO1_Firmware_FPGA_Major;
        //                                obj.Kernel_Loadbox_Card_GIO1_Firmware_FPGA_Minor = gio1Items.ToArray()[i].Kernel_Loadbox_Card_GIO1_Firmware_FPGA_Minor;
        //                                obj.Kernel_Loadbox_Card_GIO1_Calibration_Day = gio1Items.ToArray()[i].Kernel_Loadbox_Card_GIO1_Calibration_Day;
        //                                obj.Kernel_Loadbox_Card_GIO1_Calibration_Month = gio1Items.ToArray()[i].Kernel_Loadbox_Card_GIO1_Calibration_Month;
        //                                obj.Kernel_Loadbox_Card_GIO1_Calibration_Year = gio1Items.ToArray()[i].Kernel_Loadbox_Card_GIO1_Calibration_Year;
        //                                obj.Kernel_Loadbox_Card_GIO1_Next_Calibration_Day = gio1Items.ToArray()[i].Kernel_Loadbox_Card_GIO1_Next_Calibration_Day;
        //                                obj.Kernel_Loadbox_Card_GIO1_Next_Calibration_Month = gio1Items.ToArray()[i].Kernel_Loadbox_Card_GIO1_Next_Calibration_Month;
        //                                obj.Kernel_Loadbox_Card_GIO1_Next_Calibration_Year = gio1Items.ToArray()[i].Kernel_Loadbox_Card_GIO1_Next_Calibration_Year;
        //                                obj.Kernel_Loadbox_Card_GIO1_Day_of_Creation = gio1Items.ToArray()[i].Kernel_Loadbox_Card_GIO1_Day_of_Creation;
        //                                obj.Kernel_Loadbox_Card_GIO1_Month_of_Creation = gio1Items.ToArray()[i].Kernel_Loadbox_Card_GIO1_Month_of_Creation;
        //                                obj.Kernel_Loadbox_Card_GIO1_Year_of_Creation = gio1Items.ToArray()[i].Kernel_Loadbox_Card_GIO1_Year_of_Creation;
        //                                obj.Kernel_Loadbox_Card_GIO1_BuildVersion = gio1Items.ToArray()[i].Kernel_Loadbox_Card_GIO1_BuildVersion;

        //                                table6DataViews.Add(obj);
        //                            }
        //                        }
        //                    }
        //                }
        //                return Json(new { data = table6DataViews }, JsonRequestBehavior.AllowGet);
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

        //[HttpPost]
        //public ActionResult GetTABLE7_GIO2Data(List<string> pc, DateTime sdate, DateTime edate)
        //{
        //    List<TABLE7_GIO2View> table7DataViews = new List<TABLE7_GIO2View>();
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
        //                    foreach (var pcname in pc)
        //                    {
        //                        //var pcname = pcname1.FQDN.Split('.')[0].ToUpper().Trim();
        //                        {
        //                            if (pcname != null && pcname != string.Empty && pcname.Count() > 0)
        //                            {


        //                                var gio2Items = (from item in db.GIO2_Card_Pr orderby item.Date where item.System_name.Contains(pcname) && item.Date.Contains(date) select new { item.Date, item.Kernel_Loadbox_Card_GIO2_BuildVersion, item.Kernel_Loadbox_Card_GIO2_Calibration_Day, item.Kernel_Loadbox_Card_GIO2_Calibration_Month, item.Kernel_Loadbox_Card_GIO2_Calibration_Year, item.Kernel_Loadbox_Card_GIO2_CARD_BoardRevision, item.Kernel_Loadbox_Card_GIO2_CARD_BoardType, item.Kernel_Loadbox_Card_GIO2_CARD_Description, item.Kernel_Loadbox_Card_GIO2_CARD_VER_TAG_ARRAY, item.Kernel_Loadbox_Card_GIO2_Day_of_Creation, item.Kernel_Loadbox_Card_GIO2_Firmware_Card_Major, item.Kernel_Loadbox_Card_GIO2_Firmware_Card_Minor, item.Kernel_Loadbox_Card_GIO2_Firmware_FPGA_Major, item.Kernel_Loadbox_Card_GIO2_Firmware_FPGA_Minor, item.Kernel_Loadbox_Card_GIO2_Month_of_Creation, item.Kernel_Loadbox_Card_GIO2_Next_Calibration_Day, item.Kernel_Loadbox_Card_GIO2_Next_Calibration_Month, item.Kernel_Loadbox_Card_GIO2_Next_Calibration_Year, item.Kernel_Loadbox_Card_GIO2_PowerOnTime, item.Kernel_Loadbox_Card_GIO2_RepairCounter, item.Kernel_Loadbox_Card_GIO2_Serial_No, item.Kernel_Loadbox_Card_GIO2_Year_of_Creation, item.System_name });

        //                                for (int i = 0; i < gio2Items.Count(); i++)
        //                                {
        //                                    TABLE7_GIO2View obj = new TABLE7_GIO2View();
        //                                    obj.Date = DateTime.Parse(gio2Items.ToArray()[i].Date).ToShortDateString();
        //                                    obj.System_name = gio2Items.ToArray()[i].System_name;

        //                                    using (BookingServerReplicaEntities db1 = new BookingServerReplicaEntities())
        //                                    {

        //                                        var labid = db1.LabComputersPrs.ToList().Find(x => x.FQDN.Split('.')[0].ToUpper().Contains(obj.System_name)).LabId;//Find(x => x.DisplayName.ToString() == obj.System_name.ToString()).LabId;
        //                                        LabInfo labInfo = new LabInfo();
        //                                        //labInfo = db.LabInfoes.Where(item => item.Id == labid).FirstOrDefault();
        //                                        //if (labInfo != null)
        //                                        // {
        //                                        obj.LC_Name = db1.LabInfoes.FirstOrDefault(j => j.Id == labid).DisplayName;
        //                                    }


        //                                    obj.Kernel_Loadbox_Card_GIO2_CARD_BoardType = gio2Items.ToArray()[i].Kernel_Loadbox_Card_GIO2_CARD_BoardType;
        //                                    obj.Kernel_Loadbox_Card_GIO2_CARD_BoardRevision = gio2Items.ToArray()[i].Kernel_Loadbox_Card_GIO2_CARD_BoardRevision;
        //                                    obj.Kernel_Loadbox_Card_GIO2_CARD_VER_TAG_ARRAY = gio2Items.ToArray()[i].Kernel_Loadbox_Card_GIO2_CARD_VER_TAG_ARRAY;
        //                                    obj.Kernel_Loadbox_Card_GIO2_Serial_No = gio2Items.ToArray()[i].Kernel_Loadbox_Card_GIO2_Serial_No;
        //                                    obj.Kernel_Loadbox_Card_GIO2_Firmware_Card_Major = gio2Items.ToArray()[i].Kernel_Loadbox_Card_GIO2_Firmware_Card_Major;
        //                                    obj.Kernel_Loadbox_Card_GIO2_Firmware_Card_Minor = gio2Items.ToArray()[i].Kernel_Loadbox_Card_GIO2_Firmware_Card_Minor;
        //                                    obj.Kernel_Loadbox_Card_GIO2_Firmware_FPGA_Major = gio2Items.ToArray()[i].Kernel_Loadbox_Card_GIO2_Firmware_FPGA_Major;
        //                                    obj.Kernel_Loadbox_Card_GIO2_Firmware_FPGA_Minor = gio2Items.ToArray()[i].Kernel_Loadbox_Card_GIO2_Firmware_FPGA_Minor;
        //                                    obj.Kernel_Loadbox_Card_GIO2_Calibration_Day = gio2Items.ToArray()[i].Kernel_Loadbox_Card_GIO2_Calibration_Day;
        //                                    obj.Kernel_Loadbox_Card_GIO2_Calibration_Month = gio2Items.ToArray()[i].Kernel_Loadbox_Card_GIO2_Calibration_Month;
        //                                    obj.Kernel_Loadbox_Card_GIO2_Calibration_Year = gio2Items.ToArray()[i].Kernel_Loadbox_Card_GIO2_Calibration_Year;
        //                                    obj.Kernel_Loadbox_Card_GIO2_Next_Calibration_Day = gio2Items.ToArray()[i].Kernel_Loadbox_Card_GIO2_Next_Calibration_Day;
        //                                    obj.Kernel_Loadbox_Card_GIO2_Next_Calibration_Month = gio2Items.ToArray()[i].Kernel_Loadbox_Card_GIO2_Next_Calibration_Month;
        //                                    obj.Kernel_Loadbox_Card_GIO2_Next_Calibration_Year = gio2Items.ToArray()[i].Kernel_Loadbox_Card_GIO2_Next_Calibration_Year;
        //                                    obj.Kernel_Loadbox_Card_GIO2_Day_of_Creation = gio2Items.ToArray()[i].Kernel_Loadbox_Card_GIO2_Day_of_Creation;
        //                                    obj.Kernel_Loadbox_Card_GIO2_Month_of_Creation = gio2Items.ToArray()[i].Kernel_Loadbox_Card_GIO2_Month_of_Creation;
        //                                    obj.Kernel_Loadbox_Card_GIO2_Year_of_Creation = gio2Items.ToArray()[i].Kernel_Loadbox_Card_GIO2_Year_of_Creation;
        //                                    obj.Kernel_Loadbox_Card_GIO2_BuildVersion = gio2Items.ToArray()[i].Kernel_Loadbox_Card_GIO2_BuildVersion;

        //                                    table7DataViews.Add(obj);
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //                return Json(new { data = table7DataViews }, JsonRequestBehavior.AllowGet);
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

        //[HttpPost]
        //public ActionResult GetTABLE8_LDUData(List<string> pc, DateTime sdate, DateTime edate)
        //{
        //    List<TABLE8_LDUView> table8DataViews = new List<TABLE8_LDUView>();
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
        //                    foreach (var pcname in pc)
        //                    {
        //                        //var pcname = pcname1.FQDN.Split('.')[0].ToUpper().Trim();
        //                        if (pcname != null && pcname != string.Empty && pcname.Count() > 0)
        //                        {

        //                            var lduItems = (from item in db.LDU_Card_Pr orderby item.Date where item.System_name.Contains(pcname) && item.Date.Contains(date) select new { item.Date, item.Kernel_Loadbox_Card_LDU_BuildVersion, item.Kernel_Loadbox_Card_LDU_Calibration_Day, item.Kernel_Loadbox_Card_LDU_Calibration_Month, item.Kernel_Loadbox_Card_LDU_Calibration_Year, item.Kernel_Loadbox_Card_LDU_CARD_BoardRevision, item.Kernel_Loadbox_Card_LDU_CARD_BoardType, item.Kernel_Loadbox_Card_LDU_CARD_Description, item.Kernel_Loadbox_Card_LDU_CARD_VER_TAG_ARRAY, item.Kernel_Loadbox_Card_LDU_Day_of_Creation, item.Kernel_Loadbox_Card_LDU_Firmware_Card_Major, item.Kernel_Loadbox_Card_LDU_Firmware_Card_Minor, item.Kernel_Loadbox_Card_LDU_Firmware_FPGA_Major, item.Kernel_Loadbox_Card_LDU_Firmware_FPGA_Minor, item.Kernel_Loadbox_Card_LDU_Month_of_Creation, item.Kernel_Loadbox_Card_LDU_Next_Calibration_Day, item.Kernel_Loadbox_Card_LDU_Next_Calibration_Month, item.Kernel_Loadbox_Card_LDU_Next_Calibration_Year, item.Kernel_Loadbox_Card_LDU_PowerOnTime, item.Kernel_Loadbox_Card_LDU_RepairCounter, item.Kernel_Loadbox_Card_LDU_Serial_No, item.Kernel_Loadbox_Card_LDU_Year_of_Creation, item.System_name });

        //                            for (int i = 0; i < lduItems.Count(); i++)
        //                            {
        //                                TABLE8_LDUView obj = new TABLE8_LDUView();
        //                                obj.Date = DateTime.Parse(lduItems.ToArray()[i].Date).ToShortDateString();
        //                                obj.System_name = lduItems.ToArray()[i].System_name;

        //                                using (BookingServerReplicaEntities db1 = new BookingServerReplicaEntities())
        //                                {

        //                                    var labid = db1.LabComputersPrs.ToList().Find(x => x.FQDN.Split('.')[0].ToUpper().Contains(obj.System_name)).LabId;//Find(x => x.DisplayName.ToString() == obj.System_name.ToString()).LabId;
        //                                    LabInfo labInfo = new LabInfo();
        //                                    //labInfo = db.LabInfoes.Where(item => item.Id == labid).FirstOrDefault();
        //                                    //if (labInfo != null)
        //                                    // {
        //                                    obj.LC_Name = db1.LabInfoes.FirstOrDefault(j => j.Id == labid).DisplayName;
        //                                }

        //                                obj.Kernel_Loadbox_Card_LDU_CARD_BoardType = lduItems.ToArray()[i].Kernel_Loadbox_Card_LDU_CARD_BoardType;
        //                                obj.Kernel_Loadbox_Card_LDU_CARD_BoardRevision = lduItems.ToArray()[i].Kernel_Loadbox_Card_LDU_CARD_BoardRevision;
        //                                obj.Kernel_Loadbox_Card_LDU_CARD_VER_TAG_ARRAY = lduItems.ToArray()[i].Kernel_Loadbox_Card_LDU_CARD_VER_TAG_ARRAY;
        //                                obj.Kernel_Loadbox_Card_LDU_Serial_No = lduItems.ToArray()[i].Kernel_Loadbox_Card_LDU_Serial_No;
        //                                obj.Kernel_Loadbox_Card_LDU_Firmware_Card_Major = lduItems.ToArray()[i].Kernel_Loadbox_Card_LDU_Firmware_Card_Major;
        //                                obj.Kernel_Loadbox_Card_LDU_Firmware_Card_Minor = lduItems.ToArray()[i].Kernel_Loadbox_Card_LDU_Firmware_Card_Minor;
        //                                obj.Kernel_Loadbox_Card_LDU_Firmware_FPGA_Major = lduItems.ToArray()[i].Kernel_Loadbox_Card_LDU_Firmware_FPGA_Major;
        //                                obj.Kernel_Loadbox_Card_LDU_Firmware_FPGA_Minor = lduItems.ToArray()[i].Kernel_Loadbox_Card_LDU_Firmware_FPGA_Minor;
        //                                obj.Kernel_Loadbox_Card_LDU_Calibration_Day = lduItems.ToArray()[i].Kernel_Loadbox_Card_LDU_Calibration_Day;
        //                                obj.Kernel_Loadbox_Card_LDU_Calibration_Month = lduItems.ToArray()[i].Kernel_Loadbox_Card_LDU_Calibration_Month;
        //                                obj.Kernel_Loadbox_Card_LDU_Calibration_Year = lduItems.ToArray()[i].Kernel_Loadbox_Card_LDU_Calibration_Year;
        //                                obj.Kernel_Loadbox_Card_LDU_Next_Calibration_Day = lduItems.ToArray()[i].Kernel_Loadbox_Card_LDU_Next_Calibration_Day;
        //                                obj.Kernel_Loadbox_Card_LDU_Next_Calibration_Month = lduItems.ToArray()[i].Kernel_Loadbox_Card_LDU_Next_Calibration_Month;
        //                                obj.Kernel_Loadbox_Card_LDU_Next_Calibration_Year = lduItems.ToArray()[i].Kernel_Loadbox_Card_LDU_Next_Calibration_Year;
        //                                obj.Kernel_Loadbox_Card_LDU_Day_of_Creation = lduItems.ToArray()[i].Kernel_Loadbox_Card_LDU_Day_of_Creation;
        //                                obj.Kernel_Loadbox_Card_LDU_Month_of_Creation = lduItems.ToArray()[i].Kernel_Loadbox_Card_LDU_Month_of_Creation;
        //                                obj.Kernel_Loadbox_Card_LDU_Year_of_Creation = lduItems.ToArray()[i].Kernel_Loadbox_Card_LDU_Year_of_Creation;
        //                                obj.Kernel_Loadbox_Card_LDU_BuildVersion = lduItems.ToArray()[i].Kernel_Loadbox_Card_LDU_BuildVersion;

        //                                table8DataViews.Add(obj);
        //                            }
        //                        }
        //                    }
        //                }
        //                return Json(new { data = table8DataViews }, JsonRequestBehavior.AllowGet);
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

        //[HttpPost]
        //public ActionResult GetTABLE9_PSCData(List<string> pc, DateTime sdate, DateTime edate)
        //{
        //    List<TABLE9_PSCView> table9DataViews = new List<TABLE9_PSCView>();
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
        //                    foreach (var pcname in pc)
        //                    {
        //                        //var pcname = pcname1.FQDN.Split('.')[0].ToUpper().Trim();
        //                        if (pcname != null && pcname != string.Empty && pcname.Count() > 0)
        //                        {

        //                            var pscItems = (from item in db.PSC_Card_Pr orderby item.Date where item.System_name.Contains(pcname) && item.Date.Contains(date) select new { item.Date, item.Kernel_Loadbox_Card_PSC_BuildVersion, item.Kernel_Loadbox_Card_PSC_Calibration_Day, item.Kernel_Loadbox_Card_PSC_Calibration_Month, item.Kernel_Loadbox_Card_PSC_Calibration_Year, item.Kernel_Loadbox_Card_PSC_CARD_BoardRevision, item.Kernel_Loadbox_Card_PSC_CARD_BoardType, item.Kernel_Loadbox_Card_PSC_CARD_Description, item.Kernel_Loadbox_Card_PSC_CARD_VER_TAG_ARRAY, item.Kernel_Loadbox_Card_PSC_Day_of_Creation, item.Kernel_Loadbox_Card_PSC_Firmware_Card_Major, item.Kernel_Loadbox_Card_PSC_Firmware_Card_Minor, item.Kernel_Loadbox_Card_PSC_Firmware_FPGA_Major, item.Kernel_Loadbox_Card_PSC_Firmware_FPGA_Minor, item.Kernel_Loadbox_Card_PSC_Month_of_Creation, item.Kernel_Loadbox_Card_PSC_Next_Calibration_Day, item.Kernel_Loadbox_Card_PSC_Next_Calibration_Month, item.Kernel_Loadbox_Card_PSC_Next_Calibration_Year, item.Kernel_Loadbox_Card_PSC_PowerOnTime, item.Kernel_Loadbox_Card_PSC_RepairCounter, item.Kernel_Loadbox_Card_PSC_Serial_No, item.Kernel_Loadbox_Card_PSC_Year_of_Creation, item.System_name });


        //                            for (int i = 0; i < pscItems.Count(); i++)
        //                            {
        //                                TABLE9_PSCView obj = new TABLE9_PSCView();
        //                                obj.Date = DateTime.Parse(pscItems.ToArray()[i].Date).ToShortDateString();
        //                                obj.System_name = pscItems.ToArray()[i].System_name;

        //                                using (BookingServerReplicaEntities db1 = new BookingServerReplicaEntities())
        //                                {

        //                                    var labid = db1.LabComputersPrs.ToList().Find(x => x.FQDN.Split('.')[0].ToUpper().Contains(obj.System_name)).LabId;//Find(x => x.DisplayName.ToString() == obj.System_name.ToString()).LabId;
        //                                    LabInfo labInfo = new LabInfo();
        //                                    //labInfo = db.LabInfoes.Where(item => item.Id == labid).FirstOrDefault();
        //                                    //if (labInfo != null)
        //                                    // {
        //                                    obj.LC_Name = db1.LabInfoes.FirstOrDefault(j => j.Id == labid).DisplayName;
        //                                }

        //                                obj.Kernel_Loadbox_Card_PSC_CARD_BoardType = pscItems.ToArray()[i].Kernel_Loadbox_Card_PSC_CARD_BoardType;
        //                                obj.Kernel_Loadbox_Card_PSC_CARD_BoardRevision = pscItems.ToArray()[i].Kernel_Loadbox_Card_PSC_CARD_BoardRevision;
        //                                obj.Kernel_Loadbox_Card_PSC_CARD_VER_TAG_ARRAY = pscItems.ToArray()[i].Kernel_Loadbox_Card_PSC_CARD_VER_TAG_ARRAY;
        //                                obj.Kernel_Loadbox_Card_PSC_Serial_No = pscItems.ToArray()[i].Kernel_Loadbox_Card_PSC_Serial_No;
        //                                obj.Kernel_Loadbox_Card_PSC_Firmware_Card_Major = pscItems.ToArray()[i].Kernel_Loadbox_Card_PSC_Firmware_Card_Major;
        //                                obj.Kernel_Loadbox_Card_PSC_Firmware_Card_Minor = pscItems.ToArray()[i].Kernel_Loadbox_Card_PSC_Firmware_Card_Minor;
        //                                obj.Kernel_Loadbox_Card_PSC_Firmware_FPGA_Major = pscItems.ToArray()[i].Kernel_Loadbox_Card_PSC_Firmware_FPGA_Major;
        //                                obj.Kernel_Loadbox_Card_PSC_Firmware_FPGA_Minor = pscItems.ToArray()[i].Kernel_Loadbox_Card_PSC_Firmware_FPGA_Minor;
        //                                obj.Kernel_Loadbox_Card_PSC_Calibration_Day = pscItems.ToArray()[i].Kernel_Loadbox_Card_PSC_Calibration_Day;
        //                                obj.Kernel_Loadbox_Card_PSC_Calibration_Month = pscItems.ToArray()[i].Kernel_Loadbox_Card_PSC_Calibration_Month;
        //                                obj.Kernel_Loadbox_Card_PSC_Calibration_Year = pscItems.ToArray()[i].Kernel_Loadbox_Card_PSC_Calibration_Year;
        //                                obj.Kernel_Loadbox_Card_PSC_Next_Calibration_Day = pscItems.ToArray()[i].Kernel_Loadbox_Card_PSC_Next_Calibration_Day;
        //                                obj.Kernel_Loadbox_Card_PSC_Next_Calibration_Month = pscItems.ToArray()[i].Kernel_Loadbox_Card_PSC_Next_Calibration_Month;
        //                                obj.Kernel_Loadbox_Card_PSC_Next_Calibration_Year = pscItems.ToArray()[i].Kernel_Loadbox_Card_PSC_Next_Calibration_Year;
        //                                obj.Kernel_Loadbox_Card_PSC_Day_of_Creation = pscItems.ToArray()[i].Kernel_Loadbox_Card_PSC_Day_of_Creation;
        //                                obj.Kernel_Loadbox_Card_PSC_Month_of_Creation = pscItems.ToArray()[i].Kernel_Loadbox_Card_PSC_Month_of_Creation;
        //                                obj.Kernel_Loadbox_Card_PSC_Year_of_Creation = pscItems.ToArray()[i].Kernel_Loadbox_Card_PSC_Year_of_Creation;
        //                                obj.Kernel_Loadbox_Card_PSC_BuildVersion = pscItems.ToArray()[i].Kernel_Loadbox_Card_PSC_BuildVersion;

        //                                table9DataViews.Add(obj);
        //                            }
        //                        }
        //                    }
        //                }
        //                return Json(new { data = table9DataViews }, JsonRequestBehavior.AllowGet);
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

        //[HttpPost]
        //public ActionResult GetTABLE10_VSCData(List<string> pc, DateTime sdate, DateTime edate)
        //{
        //    List<TABLE10_VSCView> table10DataViews = new List<TABLE10_VSCView>();
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
        //                    foreach (var pcname in pc)
        //                    {
        //                        //var pcname = pcname1.FQDN.Split('.')[0].ToUpper().Trim();
        //                        if (pcname != null && pcname != string.Empty && pcname.Count() > 0)
        //                        {

        //                            var vscItems = (from item in db.VSC_Card_Pr orderby item.Date where item.System_name.Contains(pcname) && item.Date.Contains(date) select new { item.Date, item.Kernel_Loadbox_Card_VSC_BuildVersion, item.Kernel_Loadbox_Card_VSC_Calibration_Day, item.Kernel_Loadbox_Card_VSC_Calibration_Month, item.Kernel_Loadbox_Card_VSC_Calibration_Year, item.Kernel_Loadbox_Card_VSC_CARD_BoardRevision, item.Kernel_Loadbox_Card_VSC_CARD_BoardType, item.Kernel_Loadbox_Card_VSC_CARD_Description, item.Kernel_Loadbox_Card_VSC_CARD_VER_TAG_ARRAY, item.Kernel_Loadbox_Card_VSC_Day_of_Creation, item.Kernel_Loadbox_Card_VSC_Firmware_Card_Major, item.Kernel_Loadbox_Card_VSC_Firmware_Card_Minor, item.Kernel_Loadbox_Card_VSC_Firmware_FPGA_Major, item.Kernel_Loadbox_Card_VSC_Firmware_FPGA_Minor, item.Kernel_Loadbox_Card_VSC_Month_of_Creation, item.Kernel_Loadbox_Card_VSC_Next_Calibration_Day, item.Kernel_Loadbox_Card_VSC_Next_Calibration_Month, item.Kernel_Loadbox_Card_VSC_Next_Calibration_Year, item.Kernel_Loadbox_Card_VSC_PowerOnTime, item.Kernel_Loadbox_Card_VSC_RepairCounter, item.Kernel_Loadbox_Card_VSC_Serial_No, item.Kernel_Loadbox_Card_VSC_Year_of_Creation, item.System_name });

        //                            for (int i = 0; i < vscItems.Count(); i++)
        //                            {
        //                                TABLE10_VSCView obj = new TABLE10_VSCView();
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


        //                                obj.Kernel_Loadbox_Card_VSC_CARD_BoardType = vscItems.ToArray()[i].Kernel_Loadbox_Card_VSC_CARD_BoardType;
        //                                obj.Kernel_Loadbox_Card_VSC_CARD_BoardRevision = vscItems.ToArray()[i].Kernel_Loadbox_Card_VSC_CARD_BoardRevision;
        //                                obj.Kernel_Loadbox_Card_VSC_CARD_VER_TAG_ARRAY = vscItems.ToArray()[i].Kernel_Loadbox_Card_VSC_CARD_VER_TAG_ARRAY;
        //                                obj.Kernel_Loadbox_Card_VSC_Serial_No = vscItems.ToArray()[i].Kernel_Loadbox_Card_VSC_Serial_No;
        //                                obj.Kernel_Loadbox_Card_VSC_Firmware_Card_Major = vscItems.ToArray()[i].Kernel_Loadbox_Card_VSC_Firmware_Card_Major;
        //                                obj.Kernel_Loadbox_Card_VSC_Firmware_Card_Minor = vscItems.ToArray()[i].Kernel_Loadbox_Card_VSC_Firmware_Card_Minor;
        //                                obj.Kernel_Loadbox_Card_VSC_Firmware_FPGA_Major = vscItems.ToArray()[i].Kernel_Loadbox_Card_VSC_Firmware_FPGA_Major;
        //                                obj.Kernel_Loadbox_Card_VSC_Firmware_FPGA_Minor = vscItems.ToArray()[i].Kernel_Loadbox_Card_VSC_Firmware_FPGA_Minor;
        //                                obj.Kernel_Loadbox_Card_VSC_Calibration_Day = vscItems.ToArray()[i].Kernel_Loadbox_Card_VSC_Calibration_Day;
        //                                obj.Kernel_Loadbox_Card_VSC_Calibration_Month = vscItems.ToArray()[i].Kernel_Loadbox_Card_VSC_Calibration_Month;
        //                                obj.Kernel_Loadbox_Card_VSC_Calibration_Year = vscItems.ToArray()[i].Kernel_Loadbox_Card_VSC_Calibration_Year;
        //                                obj.Kernel_Loadbox_Card_VSC_Next_Calibration_Day = vscItems.ToArray()[i].Kernel_Loadbox_Card_VSC_Next_Calibration_Day;
        //                                obj.Kernel_Loadbox_Card_VSC_Next_Calibration_Month = vscItems.ToArray()[i].Kernel_Loadbox_Card_VSC_Next_Calibration_Month;
        //                                obj.Kernel_Loadbox_Card_VSC_Next_Calibration_Year = vscItems.ToArray()[i].Kernel_Loadbox_Card_VSC_Next_Calibration_Year;
        //                                obj.Kernel_Loadbox_Card_VSC_Day_of_Creation = vscItems.ToArray()[i].Kernel_Loadbox_Card_VSC_Day_of_Creation;
        //                                obj.Kernel_Loadbox_Card_VSC_Month_of_Creation = vscItems.ToArray()[i].Kernel_Loadbox_Card_VSC_Month_of_Creation;
        //                                obj.Kernel_Loadbox_Card_VSC_Year_of_Creation = vscItems.ToArray()[i].Kernel_Loadbox_Card_VSC_Year_of_Creation;
        //                                obj.Kernel_Loadbox_Card_VSC_BuildVersion = vscItems.ToArray()[i].Kernel_Loadbox_Card_VSC_BuildVersion;
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

        //[HttpPost]
        //public ActionResult GetTABLE11_WSSData(List<string> pc, DateTime sdate, DateTime edate)
        //{
        //    List<TABLE11_WSSView> table11DataViews = new List<TABLE11_WSSView>();
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
        //                    foreach (var pcname in pc)
        //                    {
        //                        //var pcname = pcname1.FQDN.Split('.')[0].ToUpper().Trim();
        //                        if (pcname != null && pcname != string.Empty && pcname.Count() > 0)
        //                        {

        //                            var wssItems = (from item in db.WSS_Card_Pr orderby item.Date where item.System_name.Contains(pcname) && item.Date.Contains(date) select new { item.Date, item.Kernel_Loadbox_Card_WSS_BuildVersion, item.Kernel_Loadbox_Card_WSS_Calibration_Day, item.Kernel_Loadbox_Card_WSS_Calibration_Month, item.Kernel_Loadbox_Card_WSS_Calibration_Year, item.Kernel_Loadbox_Card_WSS_CARD_BoardRevision, item.Kernel_Loadbox_Card_WSS_CARD_BoardType, item.Kernel_Loadbox_Card_WSS_CARD_Description, item.Kernel_Loadbox_Card_WSS_CARD_VER_TAG_ARRAY, item.Kernel_Loadbox_Card_WSS_Day_of_Creation, item.Kernel_Loadbox_Card_WSS_Firmware_Card_Major, item.Kernel_Loadbox_Card_WSS_Firmware_Card_Minor, item.Kernel_Loadbox_Card_WSS_Firmware_FPGA_Major, item.Kernel_Loadbox_Card_WSS_Firmware_FPGA_Minor, item.Kernel_Loadbox_Card_WSS_Month_of_Creation, item.Kernel_Loadbox_Card_WSS_Next_Calibration_Day, item.Kernel_Loadbox_Card_WSS_Next_Calibration_Month, item.Kernel_Loadbox_Card_WSS_Next_Calibration_Year, item.Kernel_Loadbox_Card_WSS_PowerOnTime, item.Kernel_Loadbox_Card_WSS_RepairCounter, item.Kernel_Loadbox_Card_WSS_Serial_No, item.Kernel_Loadbox_Card_WSS_Year_of_Creation, item.System_name });

        //                            for (int i = 0; i < wssItems.Count(); i++)
        //                            {
        //                                TABLE11_WSSView obj = new TABLE11_WSSView();
        //                                obj.Date = DateTime.Parse(wssItems.ToArray()[i].Date).ToShortDateString();
        //                                obj.System_name = wssItems.ToArray()[i].System_name;

        //                                using (BookingServerReplicaEntities db1 = new BookingServerReplicaEntities())
        //                                {

        //                                    var labid = db1.LabComputersPrs.ToList().Find(x => x.FQDN.Split('.')[0].ToUpper().Contains(obj.System_name)).LabId;//Find(x => x.DisplayName.ToString() == obj.System_name.ToString()).LabId;
        //                                    LabInfo labInfo = new LabInfo();
        //                                    //labInfo = db.LabInfoes.Where(item => item.Id == labid).FirstOrDefault();
        //                                    //if (labInfo != null)
        //                                    // {
        //                                    obj.LC_Name = db1.LabInfoes.FirstOrDefault(j => j.Id == labid).DisplayName;
        //                                }


        //                                obj.Kernel_Loadbox_Card_WSS_CARD_BoardType = wssItems.ToArray()[i].Kernel_Loadbox_Card_WSS_CARD_BoardType;
        //                                obj.Kernel_Loadbox_Card_WSS_CARD_BoardRevision = wssItems.ToArray()[i].Kernel_Loadbox_Card_WSS_CARD_BoardRevision;
        //                                obj.Kernel_Loadbox_Card_WSS_CARD_VER_TAG_ARRAY = wssItems.ToArray()[i].Kernel_Loadbox_Card_WSS_CARD_VER_TAG_ARRAY;
        //                                obj.Kernel_Loadbox_Card_WSS_Serial_No = wssItems.ToArray()[i].Kernel_Loadbox_Card_WSS_Serial_No;
        //                                obj.Kernel_Loadbox_Card_WSS_Firmware_Card_Major = wssItems.ToArray()[i].Kernel_Loadbox_Card_WSS_Firmware_Card_Major;
        //                                obj.Kernel_Loadbox_Card_WSS_Firmware_Card_Minor = wssItems.ToArray()[i].Kernel_Loadbox_Card_WSS_Firmware_Card_Minor;
        //                                obj.Kernel_Loadbox_Card_WSS_Firmware_FPGA_Major = wssItems.ToArray()[i].Kernel_Loadbox_Card_WSS_Firmware_FPGA_Major;
        //                                obj.Kernel_Loadbox_Card_WSS_Firmware_FPGA_Minor = wssItems.ToArray()[i].Kernel_Loadbox_Card_WSS_Firmware_FPGA_Minor;
        //                                obj.Kernel_Loadbox_Card_WSS_Calibration_Day = wssItems.ToArray()[i].Kernel_Loadbox_Card_WSS_Calibration_Day;
        //                                obj.Kernel_Loadbox_Card_WSS_Calibration_Month = wssItems.ToArray()[i].Kernel_Loadbox_Card_WSS_Calibration_Month;
        //                                obj.Kernel_Loadbox_Card_WSS_Calibration_Year = wssItems.ToArray()[i].Kernel_Loadbox_Card_WSS_Calibration_Year;
        //                                obj.Kernel_Loadbox_Card_WSS_Next_Calibration_Day = wssItems.ToArray()[i].Kernel_Loadbox_Card_WSS_Next_Calibration_Day;
        //                                obj.Kernel_Loadbox_Card_WSS_Next_Calibration_Month = wssItems.ToArray()[i].Kernel_Loadbox_Card_WSS_Next_Calibration_Month;
        //                                obj.Kernel_Loadbox_Card_WSS_Next_Calibration_Year = wssItems.ToArray()[i].Kernel_Loadbox_Card_WSS_Next_Calibration_Year;
        //                                obj.Kernel_Loadbox_Card_WSS_Day_of_Creation = wssItems.ToArray()[i].Kernel_Loadbox_Card_WSS_Day_of_Creation;
        //                                obj.Kernel_Loadbox_Card_WSS_Month_of_Creation = wssItems.ToArray()[i].Kernel_Loadbox_Card_WSS_Month_of_Creation;
        //                                obj.Kernel_Loadbox_Card_WSS_Year_of_Creation = wssItems.ToArray()[i].Kernel_Loadbox_Card_WSS_Year_of_Creation;
        //                                obj.Kernel_Loadbox_Card_WSS_BuildVersion = wssItems.ToArray()[i].Kernel_Loadbox_Card_WSS_BuildVersion;
        //                                table11DataViews.Add(obj);
        //                            }
        //                        }
        //                    }
        //                }
        //                return Json(new { data = table11DataViews }, JsonRequestBehavior.AllowGet);
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

        //[HttpPost]
        //public ActionResult GetTABLE12_WSS2Data(List<string> pc, DateTime sdate, DateTime edate)
        //{
        //    List<TABLE12_WSS2View> table12DataViews = new List<TABLE12_WSS2View>();
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
        //                    foreach (var pcname in pc)
        //                    {
        //                        //var pcname = pcname1.FQDN.Split('.')[0].ToUpper().Trim();
        //                        if (pcname != null && pcname != string.Empty && pcname.Count() > 0)
        //                        {

        //                            var wss2Items = (from item in db.WSS2_Card_Pr orderby item.Date where item.System_name.Contains(pcname) && item.Date.Contains(date) select new { item.Date, item.Kernel_Loadbox_Card_WSS2_BuildVersion, item.Kernel_Loadbox_Card_WSS2_Calibration_Day, item.Kernel_Loadbox_Card_WSS2_Calibration_Month, item.Kernel_Loadbox_Card_WSS2_Calibration_Year, item.Kernel_Loadbox_Card_WSS2_CARD_BoardRevision, item.Kernel_Loadbox_Card_WSS2_CARD_BoardType, item.Kernel_Loadbox_Card_WSS2_CARD_Description, item.Kernel_Loadbox_Card_WSS2_CARD_VER_TAG_ARRAY, item.Kernel_Loadbox_Card_WSS2_Day_of_Creation, item.Kernel_Loadbox_Card_WSS2_Firmware_Card_Major, item.Kernel_Loadbox_Card_WSS2_Firmware_Card_Minor, item.Kernel_Loadbox_Card_WSS2_Firmware_FPGA_Major, item.Kernel_Loadbox_Card_WSS2_Firmware_FPGA_Minor, item.Kernel_Loadbox_Card_WSS2_Month_of_Creation, item.Kernel_Loadbox_Card_WSS2_Next_Calibration_Day, item.Kernel_Loadbox_Card_WSS2_Next_Calibration_Month, item.Kernel_Loadbox_Card_WSS2_Next_Calibration_Year, item.Kernel_Loadbox_Card_WSS2_PowerOnTime, item.Kernel_Loadbox_Card_WSS2_RepairCounter, item.Kernel_Loadbox_Card_WSS2_Serial_No, item.Kernel_Loadbox_Card_WSS2_Year_of_Creation, item.System_name });


        //                            for (int i = 0; i < wss2Items.Count(); i++)
        //                            {
        //                                TABLE12_WSS2View obj = new TABLE12_WSS2View();
        //                                obj.Date = DateTime.Parse(wss2Items.ToArray()[i].Date).ToShortDateString();
        //                                obj.System_name = wss2Items.ToArray()[i].System_name;

        //                                using (BookingServerReplicaEntities db1 = new BookingServerReplicaEntities())
        //                                {

        //                                    var labid = db1.LabComputersPrs.ToList().Find(x => x.FQDN.Split('.')[0].ToUpper().Contains(obj.System_name)).LabId;//Find(x => x.DisplayName.ToString() == obj.System_name.ToString()).LabId;
        //                                    LabInfo labInfo = new LabInfo();
        //                                    //labInfo = db.LabInfoes.Where(item => item.Id == labid).FirstOrDefault();
        //                                    //if (labInfo != null)
        //                                    // {
        //                                    obj.LC_Name = db1.LabInfoes.FirstOrDefault(j => j.Id == labid).DisplayName;
        //                                }


        //                                obj.Kernel_Loadbox_Card_WSS2_CARD_BoardType = wss2Items.ToArray()[i].Kernel_Loadbox_Card_WSS2_CARD_BoardType;
        //                                obj.Kernel_Loadbox_Card_WSS2_CARD_BoardRevision = wss2Items.ToArray()[i].Kernel_Loadbox_Card_WSS2_CARD_BoardRevision;
        //                                obj.Kernel_Loadbox_Card_WSS2_CARD_VER_TAG_ARRAY = wss2Items.ToArray()[i].Kernel_Loadbox_Card_WSS2_CARD_VER_TAG_ARRAY;
        //                                obj.Kernel_Loadbox_Card_WSS2_Serial_No = wss2Items.ToArray()[i].Kernel_Loadbox_Card_WSS2_Serial_No;
        //                                obj.Kernel_Loadbox_Card_WSS2_Firmware_Card_Major = wss2Items.ToArray()[i].Kernel_Loadbox_Card_WSS2_Firmware_Card_Major;
        //                                obj.Kernel_Loadbox_Card_WSS2_Firmware_Card_Minor = wss2Items.ToArray()[i].Kernel_Loadbox_Card_WSS2_Firmware_Card_Minor;
        //                                obj.Kernel_Loadbox_Card_WSS2_Firmware_FPGA_Major = wss2Items.ToArray()[i].Kernel_Loadbox_Card_WSS2_Firmware_FPGA_Major;
        //                                obj.Kernel_Loadbox_Card_WSS2_Firmware_FPGA_Minor = wss2Items.ToArray()[i].Kernel_Loadbox_Card_WSS2_Firmware_FPGA_Minor;
        //                                obj.Kernel_Loadbox_Card_WSS2_Calibration_Day = wss2Items.ToArray()[i].Kernel_Loadbox_Card_WSS2_Calibration_Day;
        //                                obj.Kernel_Loadbox_Card_WSS2_Calibration_Month = wss2Items.ToArray()[i].Kernel_Loadbox_Card_WSS2_Calibration_Month;
        //                                obj.Kernel_Loadbox_Card_WSS2_Calibration_Year = wss2Items.ToArray()[i].Kernel_Loadbox_Card_WSS2_Calibration_Year;
        //                                obj.Kernel_Loadbox_Card_WSS2_Next_Calibration_Day = wss2Items.ToArray()[i].Kernel_Loadbox_Card_WSS2_Next_Calibration_Day;
        //                                obj.Kernel_Loadbox_Card_WSS2_Next_Calibration_Month = wss2Items.ToArray()[i].Kernel_Loadbox_Card_WSS2_Next_Calibration_Month;
        //                                obj.Kernel_Loadbox_Card_WSS2_Next_Calibration_Year = wss2Items.ToArray()[i].Kernel_Loadbox_Card_WSS2_Next_Calibration_Year;
        //                                obj.Kernel_Loadbox_Card_WSS2_Day_of_Creation = wss2Items.ToArray()[i].Kernel_Loadbox_Card_WSS2_Day_of_Creation;
        //                                obj.Kernel_Loadbox_Card_WSS2_Month_of_Creation = wss2Items.ToArray()[i].Kernel_Loadbox_Card_WSS2_Month_of_Creation;
        //                                obj.Kernel_Loadbox_Card_WSS2_Year_of_Creation = wss2Items.ToArray()[i].Kernel_Loadbox_Card_WSS2_Year_of_Creation;
        //                                obj.Kernel_Loadbox_Card_WSS2_BuildVersion = wss2Items.ToArray()[i].Kernel_Loadbox_Card_WSS2_BuildVersion;
        //                                table12DataViews.Add(obj);
        //                            }
        //                        }
        //                    }
        //                }
        //                return Json(new { data = table12DataViews }, JsonRequestBehavior.AllowGet);
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


        //[HttpPost]
        //public ActionResult GetVSCLDUSummaryData(List<string> pc, DateTime sdate, DateTime edate)
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
        //                    foreach (var pcname in pc)
        //                    {
        //                        //var pcname = pcname1.FQDN.Split('.')[0].ToUpper().Trim();
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





        //*********************************** UNIQUE *******************************

        //[HttpPost]
        //public ActionResult GetTABLE1_HWDescData_unique(List<string> pc, DateTime sdate, DateTime edate)
        //{
        //    List<TABLE1_HwDataView> hwDataViews = new List<TABLE1_HwDataView>();
        //    if (pc != null && pc.Count() > 0)
        //    {
        //        try
        //        {
        //            using (PromasterImport_HWInfoEntities db = new PromasterImport_HWInfoEntities())
        //            {
        //                db.Database.CommandTimeout = 500;
        //                string date = string.Empty;
        //                for (var dt = sdate; dt <= edate; dt = dt.AddDays(1))
        //                {
        //                    date = dt.ToString("yyyy-MM-dd");

        //                    foreach (var pcname in pc)
        //                    {
        //                        //var pcname = pcname1.FQDN.Split('.')[0].ToUpper().Trim();
        //                        if (pcname != null && pcname != string.Empty && pcname.Count() > 0)
        //                        {

        //                             //var hwItems = (from item in db.Hardware_Desc_Pr orderby item.Date where item.System_name.Contains(pcname) && item.Date.Contains(date) select new { item.EEPName, item.Date, item.EEPBuildDate, item.EEPDatabaseVersion, item.LabCarType, item.PCHostName, item.System_name, item.RBCCAFVersion, item.RTPCName, item.RTPCSoftwareVersion, item.ToolChainVersion }).ToList();
        //                            //var hwItems1= (from item in db.Hardware_Desc_Pr orderby item.Date where  item.Date.Contains(date) && item.System_name.Contains(pcname) select new { item.EEPName, item.Date, item.EEPBuildDate, item.EEPDatabaseVersion, item.LabCarType, item.PCHostName, item.System_name, item.RBCCAFVersion, item.RTPCName, item.RTPCSoftwareVersion, item.ToolChainVersion }).OrderByDescending(x=>x.Date).GroupBy(x=>x.EEPName).ToList();
        //                            //var hwItem2 = (from item in db.Hardware_Desc_Pr orderby item.Date where item.System_name.Contains(pcname) && item.Date.Contains(date) select new { item.EEPName, item.Date, item.EEPBuildDate, item.EEPDatabaseVersion, item.LabCarType, item.PCHostName, item.System_name, item.RBCCAFVersion, item.RTPCName, item.RTPCSoftwareVersion, item.ToolChainVersion }).Distinct().ToList();
        //                            //var hwItems3 = (from item in db.Hardware_Desc_Pr orderby item.Date where item.System_name.Contains(pcname) && item.Date.Contains(date) select new { item.EEPName, item.Date, item.EEPBuildDate, item.EEPDatabaseVersion, item.LabCarType, item.PCHostName, item.System_name, item.RBCCAFVersion, item.RTPCName, item.RTPCSoftwareVersion, item.ToolChainVersion }).OrderByDescending(x => x.Date).ToList();
        //                            //var hwItems4 = (from item in db.Hardware_Desc_Pr orderby item.Date where item.System_name.Contains(pcname) && item.Date.Contains(date) select new { item.EEPName, item.Date, item.EEPBuildDate, item.EEPDatabaseVersion, item.LabCarType, item.PCHostName, item.System_name, item.RBCCAFVersion, item.RTPCName, item.RTPCSoftwareVersion, item.ToolChainVersion }).GroupBy(x => x.EEPName).ToList();

        //                            var hwItems = (from item in db.Hardware_Desc_Pr where item.Date.Contains(date) && item.System_name.Contains(pcname) select new { item.EEPName, item.Date, item.EEPBuildDate, item.EEPDatabaseVersion, item.LabCarType, item.PCHostName, item.System_name, item.RBCCAFVersion, item.RTPCName, item.RTPCSoftwareVersion, item.ToolChainVersion }).GroupBy(x => x.EEPName).Select(x => x.FirstOrDefault()).ToList(); //since single date descorderbydate not needed

        //                            //var hwItems1 = (from item in db.Hardware_Desc_Pr where sdate <= DateTime.Parse(item.Date) && edate >= DateTime.Parse(item.Date) && item.System_name.Contains(pcname) select new { item.EEPName, item.Date, item.EEPBuildDate, item.EEPDatabaseVersion, item.LabCarType, item.PCHostName, item.System_name, item.RBCCAFVersion, item.RTPCName, item.RTPCSoftwareVersion, item.ToolChainVersion }).GroupBy(x => x.EEPName).Select(x => x.FirstOrDefault()).ToList(); //since single date descorderbydate not needed


        //                            foreach (var item in hwItems)
        //                            {

        //                                TABLE1_HwDataView obj = new TABLE1_HwDataView();
        //                                obj.System_name = item.System_name;

        //                                using (BookingServerReplicaEntities db1 = new BookingServerReplicaEntities())
        //                                {
        //                                    //var chk = db1.LabComputersPrs.ToList();
        //                                    //var chk1 = db1.LabComputersPrs.Select(v => v.FQDN).ToList();

        //                                    var labid = db1.LabComputersPrs.ToList().Find(x => x.FQDN.Split('.')[0].ToUpper().Contains(obj.System_name)).LabId;//Find(x => x.DisplayName.ToString() == obj.System_name.ToString()).LabId;                                     
        //                                    obj.LC_Name = db1.LabInfoes.FirstOrDefault(i => i.Id == labid).DisplayName;
        //                                }


        //                                obj.PCHostName = item.PCHostName;
        //                                obj.Date = DateTime.Parse(item.Date).ToShortDateString();
        //                                obj.EEPName = item.EEPName;
        //                                obj.EEPBuildDate = DateTime.Parse(item.EEPBuildDate).ToShortDateString();
        //                                obj.EEPDatabaseVersion = item.EEPDatabaseVersion;
        //                                obj.RTPCName = item.RTPCName;
        //                                obj.RBCCAFVersion = item.RBCCAFVersion;
        //                                obj.ToolChainVersion = item.ToolChainVersion;
        //                                obj.RTPCSoftwareVersion = item.RTPCSoftwareVersion;
        //                                obj.LabCarType = item.LabCarType;
        //                                hwDataViews.Add(obj);
        //                            }
        //                        }

        //                    }

        //                }
        //                hwDataViews = hwDataViews.OrderByDescending(x => x.Date).GroupBy(x => new { x.EEPName, /*x.Date,*/ x.System_name }).Select(x => x.FirstOrDefault()).OrderBy(x=>x.Date).ToList();
        //            }
        //            return Json(new { data = hwDataViews }, JsonRequestBehavior.AllowGet);

        //        }
        //        catch (Exception Ex)
        //        {
        //            return View("Unable to retrieve data from database. Please try again later!!");
        //        }
        //    }
        //    else
        //        return Json(new { success = false, message = "Try again" }, JsonRequestBehavior.AllowGet);
        //}

        //[HttpPost]
        //public ActionResult TABLE2_GetPrjDescData_unique(List<string> pc, DateTime sdate, DateTime edate)
        //{
        //    List<TABLE2_ProjectDescriptionView> PrjDataViews = new List<TABLE2_ProjectDescriptionView>();
        //    if (pc != null && pc.Count() > 0)
        //    {
        //        try
        //        {
        //            using (PromasterImport_ProjectDescEntities db = new PromasterImport_ProjectDescEntities())
        //            {
        //                db.Database.CommandTimeout = 500;
        //                string date = string.Empty;
        //                for (var dt = sdate; dt <= edate; dt = dt.AddDays(1))
        //                {
        //                    date = dt.ToString("yyyy-MM-dd");

        //                    foreach (var pcname in pc)
        //                    {
        //                        //var pcname = pcname1.FQDN.Split('.')[0].ToUpper().Trim();
        //                        if (pcname != null && pcname != string.Empty && pcname.Count() > 0)
        //                        {

        //                            var PrjItems = (from item in db.ProjectDescription_Pr_1 where item.Date.Contains(date) && item.System_name.Contains(pcname) select new { item.AECUCable, item.Ascet_Ver, item.Component_name, item.Product, item.Date, item.Db_Version, item.Details, item.EMUCable, item.MetaEditor_Ver, item.ProjectBuilder_Ver, item.ProjectEditor_Ver, item.System_name, item.ToolVersion, item.Version }).ToList();
        //                            var prj_grpby = PrjItems.GroupBy(x => new { /*x.Component_name,*/ x.Details }).ToList();
        //                            var prj_grpby_selectfirst = prj_grpby.Select(x => x.FirstOrDefault()).ToList();
        //                            //db.Database.CommandTimeout = 10000000;
        //                            // var hwItems = (from item in db.Hardware_Desc_Pr where item.Date.Contains(date) && item.System_name.Contains(pcname) select new { item.EEPName, item.Date, item.EEPBuildDate, item.EEPDatabaseVersion, item.LabCarType, item.PCHostName, item.System_name, item.RBCCAFVersion, item.RTPCName, item.RTPCSoftwareVersion, item.ToolChainVersion }).GroupBy(x => x.EEPName).Select(x => x.FirstOrDefault()).ToList(); //since single date descorderbydate not needed
        //                            //hwDataViews = hwDataViews.OrderByDescending(x => x.Date).GroupBy(x => x.EEPName).Select(x => x.FirstOrDefault()).OrderBy(x=>x.Date).ToList();
        //                            //var PrjItems_takingUniqueComponent_Details = (from item in db.ProjectDescription_Pr_1 orderby item.Date where item.System_name.Contains(pcname) && item.Date.Contains(date) select new { item.Component_name, item.Details}).Distinct();
        //                            //var PrjItems_takingUniqueComponent = (from item in db.ProjectDescription_Pr_1 orderby item.Date where item.System_name.Contains(pcname) && item.Date.Contains(date) select new { item.Component_name}).Distinct().ToList();
        //                            //var PrjItems_takingUniqueDetail = (from item in db.ProjectDescription_Pr_1 orderby item.Date where item.System_name.Contains(pcname) && item.Date.Contains(date) select new { item.Details }).Distinct().ToList();



        //                            foreach (var item in prj_grpby_selectfirst)
        //                            {
        //                                TABLE2_ProjectDescriptionView obj = new TABLE2_ProjectDescriptionView();

        //                                //obj.Component_name = item.Component_name;
        //                                obj.Date = DateTime.Parse(item.Date).ToShortDateString();
        //                                obj.Db_Version = item.Db_Version;
        //                                obj.Details = item.Details;
        //                                obj.System_name = item.System_name;

        //                                using (BookingServerReplicaEntities db1 = new BookingServerReplicaEntities())
        //                                {

        //                                    var labid = db1.LabComputersPrs.ToList().Find(x => x.FQDN.Split('.')[0].ToUpper().Contains(obj.System_name)).LabId;
        //                                    obj.LC_Name = db1.LabInfoes.FirstOrDefault(i => i.Id == labid).DisplayName;
        //                                }

        //                                //obj.Version = item.Version;
        //                                obj.Product = item.Product;
        //                                obj.ToolVersion = item.ToolVersion;
        //                                obj.EMUCable = item.EMUCable;
        //                                obj.AECUCable = item.AECUCable;
        //                                obj.MetaEditor_Ver = item.MetaEditor_Ver;
        //                                obj.ProjectBuilder_Ver = item.ProjectBuilder_Ver;
        //                                obj.Ascet_Ver = item.Ascet_Ver;
        //                                obj.ProjectEditor_Ver = item.ProjectEditor_Ver;


        //                                PrjDataViews.Add(obj);
        //                            }
        //                        }
        //                    }
        //                }
        //               var prjgrp =  PrjDataViews.OrderByDescending(x => x.Date).GroupBy(x => new { x.Details, /*x.Component_name, x.Date,*/ x.System_name});
        //               PrjDataViews = prjgrp.Select(x => x.FirstOrDefault()).OrderBy(x => x.Date).ToList();
        //                return Json(new { data = PrjDataViews }, JsonRequestBehavior.AllowGet);
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

        //[HttpPost]
        //public ActionResult TABLE2_GetCmpntData_unique(List<string> pc, DateTime sdate, DateTime edate)
        //{
        //    List<TABLE2_ProjectDescriptionView> PrjDataViews = new List<TABLE2_ProjectDescriptionView>();
        //    if (pc != null && pc.Count() > 0)
        //    {
        //        try
        //        {
        //            using (PromasterImport_ProjectDescEntities db = new PromasterImport_ProjectDescEntities())
        //            {
        //                db.Database.CommandTimeout = 500;
        //                string date = string.Empty;
        //                for (var dt = sdate; dt <= edate; dt = dt.AddDays(1))
        //                {
        //                    date = dt.ToString("yyyy-MM-dd");

        //                    foreach (var pcname in pc)
        //                    {
        //                        //var pcname = pcname1.FQDN.Split('.')[0].ToUpper().Trim();
        //                        if (pcname != null && pcname != string.Empty && pcname.Count() > 0)
        //                        {

        //                            var PrjItems = (from item in db.ProjectDescription_Pr_1 where item.Date.Contains(date) && item.System_name.Contains(pcname) select new { item.AECUCable, item.Ascet_Ver, item.Component_name, item.Product, item.Date, item.Db_Version, item.Details, item.EMUCable, item.MetaEditor_Ver, item.ProjectBuilder_Ver, item.ProjectEditor_Ver, item.System_name, item.ToolVersion, item.Version }).ToList();
        //                            var prj_grpby = PrjItems.GroupBy(x => new { x.Component_name, x.Details }).ToList();
        //                            var prj_grpby_selectfirst = prj_grpby.Select(x => x.FirstOrDefault()).ToList();
        //                            //db.Database.CommandTimeout = 10000000;
        //                            // var hwItems = (from item in db.Hardware_Desc_Pr where item.Date.Contains(date) && item.System_name.Contains(pcname) select new { item.EEPName, item.Date, item.EEPBuildDate, item.EEPDatabaseVersion, item.LabCarType, item.PCHostName, item.System_name, item.RBCCAFVersion, item.RTPCName, item.RTPCSoftwareVersion, item.ToolChainVersion }).GroupBy(x => x.EEPName).Select(x => x.FirstOrDefault()).ToList(); //since single date descorderbydate not needed
        //                            //hwDataViews = hwDataViews.OrderByDescending(x => x.Date).GroupBy(x => x.EEPName).Select(x => x.FirstOrDefault()).OrderBy(x=>x.Date).ToList();
        //                            //var PrjItems_takingUniqueComponent_Details = (from item in db.ProjectDescription_Pr_1 orderby item.Date where item.System_name.Contains(pcname) && item.Date.Contains(date) select new { item.Component_name, item.Details}).Distinct();
        //                            //var PrjItems_takingUniqueComponent = (from item in db.ProjectDescription_Pr_1 orderby item.Date where item.System_name.Contains(pcname) && item.Date.Contains(date) select new { item.Component_name}).Distinct().ToList();
        //                            //var PrjItems_takingUniqueDetail = (from item in db.ProjectDescription_Pr_1 orderby item.Date where item.System_name.Contains(pcname) && item.Date.Contains(date) select new { item.Details }).Distinct().ToList();



        //                            foreach (var item in prj_grpby_selectfirst)
        //                            {
        //                                TABLE2_ProjectDescriptionView obj = new TABLE2_ProjectDescriptionView();

        //                                obj.Component_name = item.Component_name;
        //                                obj.Date = DateTime.Parse(item.Date).ToShortDateString();
        //                                //obj.Db_Version = item.Db_Version;
        //                                obj.Details = item.Details;
        //                                obj.System_name = item.System_name;

        //                                using (BookingServerReplicaEntities db1 = new BookingServerReplicaEntities())
        //                                {

        //                                    var labid = db1.LabComputersPrs.ToList().Find(x => x.FQDN.Split('.')[0].ToUpper().Contains(obj.System_name)).LabId;
        //                                    obj.LC_Name = db1.LabInfoes.FirstOrDefault(i => i.Id == labid).DisplayName;
        //                                }

        //                                obj.Version = item.Version;
        //                                //obj.Product = item.Product;
        //                                //obj.ToolVersion = item.ToolVersion;
        //                                //obj.EMUCable = item.EMUCable;
        //                                //obj.AECUCable = item.AECUCable;
        //                                //obj.MetaEditor_Ver = item.MetaEditor_Ver;
        //                                //obj.ProjectBuilder_Ver = item.ProjectBuilder_Ver;
        //                                //obj.Ascet_Ver = item.Ascet_Ver;
        //                                //obj.ProjectEditor_Ver = item.ProjectEditor_Ver;


        //                                PrjDataViews.Add(obj);
        //                            }
        //                        }
        //                    }
        //                }
        //                var prjgrp = PrjDataViews.OrderByDescending(x => x.Date).GroupBy(x => new { x.Details, x.Component_name, /*x.Date,*/ x.System_name });
        //                PrjDataViews = prjgrp.Select(x => x.FirstOrDefault()).OrderBy(x => x.Date).ToList();
        //                return Json(new { data = PrjDataViews }, JsonRequestBehavior.AllowGet);
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


        //[HttpPost]
        //public ActionResult TABLE3_GetEBData_unique(List<string> pc, DateTime sdate, DateTime edate)
        //{
        //    List<TABLE3_EBView> table3DataViews = new List<TABLE3_EBView>();
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
        //                    foreach (var pcname in pc)
        //                    {
        //                        //var pcname = pcname1.FQDN.Split('.')[0].ToUpper().Trim();
        //                        if (pcname != null && pcname != string.Empty && pcname.Count() > 0)
        //                        {

        //                            var ebItems = (from item in db.EB_Cards_Pr where item.Date.Contains(date) && item.System_name.Contains(pcname) select new { item.Date, item.Kernel_EB_Cards_EB5100, item.Kernel_EB_Cards_EB5200, item.System_name }).ToList().GroupBy(x => new {x.Kernel_EB_Cards_EB5100, x.Kernel_EB_Cards_EB5200 }).Select(x => x.FirstOrDefault()).ToList();
        //                            // var hwItems = (from item in db.Hardware_Desc_Pr where item.Date.Contains(date) && item.System_name.Contains(pcname) select new { item.EEPName, item.Date, item.EEPBuildDate, item.EEPDatabaseVersion, item.LabCarType, item.PCHostName, item.System_name}).GroupBy(x => x.EEPName).Select(x => x.FirstOrDefault()).ToList(); //since single date descorderbydate not needed
        //                            //hwDataViews = hwDataViews.OrderByDescending(x => x.Date).GroupBy(x => x.EEPName).Select(x => x.FirstOrDefault()).OrderBy(x=>x.Date).ToList();


        //                            for (int i = 0; i < ebItems.Count(); i++)
        //                            {
        //                                TABLE3_EBView obj = new TABLE3_EBView();
        //                                obj.Date = DateTime.Parse(ebItems.ToArray()[i].Date).ToShortDateString();
        //                                obj.System_name = ebItems.ToArray()[i].System_name;

        //                                using (BookingServerReplicaEntities db1 = new BookingServerReplicaEntities())
        //                                {

        //                                    var labid = db1.LabComputersPrs.ToList().Find(x => x.FQDN.Split('.')[0].ToUpper().Contains(obj.System_name)).LabId;
        //                                    obj.LC_Name = db1.LabInfoes.FirstOrDefault(j => j.Id == labid).DisplayName;
        //                                }
        //                                obj.Kernel_EB_Cards_EB5200 = ebItems.ToArray()[i].Kernel_EB_Cards_EB5200;
        //                                obj.Kernel_EB_Cards_EB5100 = ebItems.ToArray()[i].Kernel_EB_Cards_EB5100;

        //                                table3DataViews.Add(obj);
        //                            }
        //                        }
        //                    }
        //                }
        //                table3DataViews = table3DataViews.OrderByDescending(x => x.Date).GroupBy(x => new { x.Kernel_EB_Cards_EB5100, x.Kernel_EB_Cards_EB5200, x.System_name/*, x.Date */}).Select(x => x.FirstOrDefault()).OrderBy(x => x.Date).ToList();
        //                return Json(new { data = table3DataViews }, JsonRequestBehavior.AllowGet);
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


        //[HttpPost] 
        //public ActionResult GetTABLE4_ES4441Data_unique(List<string> pc, DateTime sdate, DateTime edate)
        //{
        //    List<TABLE4_ES4441View> table3DataViews = new List<TABLE4_ES4441View>();
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
        //                    foreach (var pcname in pc)
        //                    {
        //                        //var pcname = pcname1.FQDN.Split('.')[0].ToUpper().Trim();
        //                        if (pcname != null && pcname != string.Empty && pcname.Count() > 0)
        //                        {

        //                            var es4441_1Items = (from item in db.ES4441_1_Pr where item.Date.Contains(date) && item.System_name.Contains(pcname) select new { item.Date, item.Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441FWVersionMain, item.Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441FWVersionMajor, item.Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441FWVersionMinor, item.Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441PLDVersion, item.Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441SN, item.System_name }).ToList();
        //                            var es4441_2Items = (from item in db.ES4441_2_Pr where item.Date.Contains(date) && item.System_name.Contains(pcname) select new { item.Date, item.Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441HWRevision, item.Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441HWVersion, item.System_name }).ToList();

        //                            var es4441Items = (from item in db.ES4441_Pr where item.Date.Contains(date) && item.System_name.Contains(pcname) select new { item.Date, item.Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441FWVersionMain, item.Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441FWVersionMajor, item.Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441FWVersionMinor, item.Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441PLDVersion, item.Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441SN, item.Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441HWRevision, item.Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441HWVersion, item.System_name }).ToList();

        //                            // var hwItems = (from item in db.Hardware_Desc_Pr where item.Date.Contains(date) && item.System_name.Contains(pcname) select new { item.EEPName, item.Date, item.EEPBuildDate, item.EEPDatabaseVersion, item.LabCarType, item.PCHostName, item.System_name}).GroupBy(x => x.EEPName).Select(x => x.FirstOrDefault()).ToList(); //since single date descorderbydate not needed
        //                            //hwDataViews = hwDataViews.OrderByDescending(x => x.Date).GroupBy(x => x.EEPName).Select(x => x.FirstOrDefault()).OrderBy(x=>x.Date).ToList();


        //                            for (int i = 0; i < es4441_1Items.Count(); i++)
        //                            {
        //                                TABLE4_ES4441View obj = new TABLE4_ES4441View();
        //                                obj.Date = DateTime.Parse(es4441_1Items.ToArray()[i].Date).ToShortDateString();
        //                                obj.System_name = es4441_1Items.ToArray()[i].System_name;

        //                                using (BookingServerReplicaEntities db1 = new BookingServerReplicaEntities())
        //                                {

        //                                    var labid = db1.LabComputersPrs.ToList().Find(x => x.FQDN.Split('.')[0].ToUpper().Contains(obj.System_name)).LabId;
        //                                    obj.LC_Name = db1.LabInfoes.FirstOrDefault(j => j.Id == labid).DisplayName;
        //                                }
        //                                obj.Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441SN = es4441_1Items.ToArray()[i].Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441SN;
        //                                obj.Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441FWVersionMinor = es4441_1Items.ToArray()[i].Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441FWVersionMinor;
        //                                obj.Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441FWVersionMajor = es4441_1Items.ToArray()[i].Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441FWVersionMajor;
        //                                obj.Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441FWVersionMain = es4441_1Items.ToArray()[i].Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441FWVersionMain;

        //                                    obj.Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441HWRevision = es4441_2Items.ToArray()[i].Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441HWRevision;
        //                                    obj.Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441HWVersion = es4441_2Items.ToArray()[i].Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441HWVersion;


        //                                table3DataViews.Add(obj);
        //                            }

        //                            for (int i = 0; i < es4441Items.Count(); i++)
        //                            {
        //                                TABLE4_ES4441View obj = new TABLE4_ES4441View();
        //                                obj.Date = DateTime.Parse(es4441Items.ToArray()[i].Date).ToShortDateString();
        //                                obj.System_name = es4441Items.ToArray()[i].System_name;

        //                                using (BookingServerReplicaEntities db1 = new BookingServerReplicaEntities())
        //                                {

        //                                    var labid = db1.LabComputersPrs.ToList().Find(x => x.FQDN.Split('.')[0].ToUpper().Contains(obj.System_name)).LabId;
        //                                    obj.LC_Name = db1.LabInfoes.FirstOrDefault(j => j.Id == labid).DisplayName;
        //                                }
        //                                obj.Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441SN = es4441Items.ToArray()[i].Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441SN;
        //                                obj.Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441FWVersionMinor = es4441Items.ToArray()[i].Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441FWVersionMinor;
        //                                obj.Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441FWVersionMajor = es4441Items.ToArray()[i].Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441FWVersionMajor;
        //                                obj.Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441FWVersionMain = es4441Items.ToArray()[i].Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441FWVersionMain;

        //                                obj.Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441HWRevision = es4441Items.ToArray()[i].Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441HWRevision;
        //                                obj.Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441HWVersion = es4441Items.ToArray()[i].Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441HWVersion;


        //                                table3DataViews.Add(obj);
        //                            }
        //                        }
        //                    }
        //                }
        //                table3DataViews = table3DataViews.OrderByDescending(x => x.Date).GroupBy(x => new {  /*x.Date,*/ x.System_name, x.Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441SN,x.Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441FWVersionMinor,x.Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441HWVersion, x.Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441HWRevision }).Select(x => x.FirstOrDefault()).OrderBy(x => x.Date).ToList();
        //                return Json(new { data = table3DataViews }, JsonRequestBehavior.AllowGet);
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
        //[HttpPost]
        //public ActionResult GetTABLE5_OTSOData_unique(List<string> pc, DateTime sdate, DateTime edate)
        //{
        //    List<TABLE5_OTSOView> table5DataViews = new List<TABLE5_OTSOView>();
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
        //                    foreach (var pcname in pc)
        //                    {
        //                        //var pcname = pcname1.FQDN.Split('.')[0].ToUpper().Trim();
        //                        if (pcname != null && pcname != string.Empty && pcname.Count() > 0)
        //                        {

        //                            var otsoItems = (from item in db.OTSO_Pr where item.Date.Contains(date) && item.System_name.Contains(pcname)  select new { item.Date, item.Kernel_Component_IPB_TemperatureReceive_OTSO1Available, item.Kernel_Component_IPB_TemperatureReceive_OTSO2Available, item.System_name }).GroupBy(x=>new { x.Kernel_Component_IPB_TemperatureReceive_OTSO2Available, x.Kernel_Component_IPB_TemperatureReceive_OTSO1Available }).Select(x=>x.FirstOrDefault());
        //                            // var hwItems = (from item in db.Hardware_Desc_Pr where item.Date.Contains(date) && item.System_name.Contains(pcname) select new { item.EEPName, item.Date, item.EEPBuildDate, item.EEPDatabaseVersion, item.LabCarType, item.PCHostName, item.System_name}).GroupBy(x => x.EEPName).Select(x => x.FirstOrDefault()).ToList(); //since single date descorderbydate not needed
        //                            //hwDataViews = hwDataViews.OrderByDescending(x => x.Date).GroupBy(x => x.EEPName).Select(x => x.FirstOrDefault()).OrderBy(x=>x.Date).ToList();

        //                            for (int i = 0; i < otsoItems.Count(); i++)
        //                            {
        //                                TABLE5_OTSOView obj = new TABLE5_OTSOView();
        //                                obj.Date = DateTime.Parse(otsoItems.ToArray()[i].Date).ToShortDateString();
        //                                obj.System_name = otsoItems.ToArray()[i].System_name;

        //                                using (BookingServerReplicaEntities db1 = new BookingServerReplicaEntities())
        //                                {

        //                                    var labid = db1.LabComputersPrs.ToList().Find(x => x.FQDN.Split('.')[0].ToUpper().Contains(obj.System_name)).LabId;//Find(x => x.DisplayName.ToString() == obj.System_name.ToString()).LabId;
        //                                    obj.LC_Name = db1.LabInfoes.FirstOrDefault(j => j.Id == labid).DisplayName;
        //                                }

        //                                obj.Kernel_Component_IPB_TemperatureReceive_OTSO1Available = otsoItems.ToArray()[i].Kernel_Component_IPB_TemperatureReceive_OTSO1Available;
        //                                obj.Kernel_Component_IPB_TemperatureReceive_OTSO2Available = otsoItems.ToArray()[i].Kernel_Component_IPB_TemperatureReceive_OTSO2Available;

        //                                table5DataViews.Add(obj);
        //                            }
        //                        }
        //                    }
        //                }
        //                table5DataViews = table5DataViews.OrderByDescending(x => x.Date).GroupBy(x => new { /*x.Date,*/ x.System_name, x.Kernel_Component_IPB_TemperatureReceive_OTSO2Available, x.Kernel_Component_IPB_TemperatureReceive_OTSO1Available }).Select(x => x.FirstOrDefault()).OrderBy(x => x.Date).ToList();
        //                return Json(new { data = table5DataViews }, JsonRequestBehavior.AllowGet);
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
        //[HttpPost]
        //public ActionResult GetTABLE6_PowerSupplyData_unique(List<string> pc, DateTime sdate, DateTime edate)
        //{
        //    List<TABLE6_PowerSupplyView> table6DataViews = new List<TABLE6_PowerSupplyView>();
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
        //                    foreach (var pcname in pc)
        //                    {
        //                        //var pcname = pcname1.FQDN.Split('.')[0].ToUpper().Trim();
        //                        if (pcname != null && pcname != string.Empty && pcname.Count() > 0)
        //                        {

        //                              var powersupplyItems = (from item in db.Power_Supply_Pr where item.Date.Contains(date) && item.System_name.Contains(pcname) select new { item.Date, item.Kernel_Loadbox_Card_PSC_Power_Supply_Configuration_PS_Configuration, item.Kernel_Loadbox_Card_PSC_Power_Supply_Configuration_PS_Factor_ECU, item.Kernel_Loadbox_Card_PSC_Power_Supply_Configuration_PS_Factor_MR, item.Kernel_Loadbox_Card_PSC_Power_Supply_Configuration_PS_Factor_VR, item.System_name }).GroupBy(x=> new { x.Kernel_Loadbox_Card_PSC_Power_Supply_Configuration_PS_Configuration, x.Kernel_Loadbox_Card_PSC_Power_Supply_Configuration_PS_Factor_ECU }).Select(x=>x.FirstOrDefault());
        //                            // var hwItems = (from item in db.Hardware_Desc_Pr where item.Date.Contains(date) && item.System_name.Contains(pcname) select new { item.EEPName, item.Date, item.EEPBuildDate, item.EEPDatabaseVersion, item.LabCarType, item.PCHostName, item.System_name}).GroupBy(x => x.EEPName).Select(x => x.FirstOrDefault()).ToList(); //since single date descorderbydate not needed
        //                            //hwDataViews = hwDataViews.OrderByDescending(x => x.Date).GroupBy(x => x.EEPName).Select(x => x.FirstOrDefault()).OrderBy(x=>x.Date).ToList();


        //                            for (int i = 0; i < powersupplyItems.Count(); i++)
        //                            {
        //                                TABLE6_PowerSupplyView obj = new TABLE6_PowerSupplyView();
        //                                obj.System_name = powersupplyItems.ToArray()[i].System_name;
        //                                obj.Date = DateTime.Parse(powersupplyItems.ToArray()[i].Date).ToShortDateString();

        //                                using (BookingServerReplicaEntities db1 = new BookingServerReplicaEntities())
        //                                {

        //                                    var labid = db1.LabComputersPrs.ToList().Find(x => x.FQDN.Split('.')[0].ToUpper().Contains(obj.System_name)).LabId;//Find(x => x.DisplayName.ToString() == obj.System_name.ToString()).LabId;
        //                                    obj.LC_Name = db1.LabInfoes.FirstOrDefault(j => j.Id == labid).DisplayName;
        //                                }

        //                                obj.Kernel_Loadbox_Card_PSC_Power_Supply_Configuration_PS_Configuration = powersupplyItems.ToArray()[i].Kernel_Loadbox_Card_PSC_Power_Supply_Configuration_PS_Configuration;
        //                                obj.Kernel_Loadbox_Card_PSC_Power_Supply_Configuration_PS_Factor_ECU = powersupplyItems.ToArray()[i].Kernel_Loadbox_Card_PSC_Power_Supply_Configuration_PS_Factor_ECU;
        //                                obj.Kernel_Loadbox_Card_PSC_Power_Supply_Configuration_PS_Factor_MR = powersupplyItems.ToArray()[i].Kernel_Loadbox_Card_PSC_Power_Supply_Configuration_PS_Factor_MR;
        //                                obj.Kernel_Loadbox_Card_PSC_Power_Supply_Configuration_PS_Factor_VR = powersupplyItems.ToArray()[i].Kernel_Loadbox_Card_PSC_Power_Supply_Configuration_PS_Factor_VR;
        //                                table6DataViews.Add(obj);
        //                            }
        //                        }
        //                    }
        //                }
        //                table6DataViews = table6DataViews.OrderByDescending(x => x.Date).GroupBy(x => new { /*x.Date,*/ x.System_name, x.Kernel_Loadbox_Card_PSC_Power_Supply_Configuration_PS_Configuration, x.Kernel_Loadbox_Card_PSC_Power_Supply_Configuration_PS_Factor_ECU }).Select(x => x.FirstOrDefault()).OrderBy(x => x.Date).ToList();
        //                return Json(new { data = table6DataViews }, JsonRequestBehavior.AllowGet);
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


        //[HttpPost]
        //public ActionResult GetTABLE7_BOBData_unique(List<string> pc, DateTime sdate, DateTime edate)
        //{
        //    List<TABLE7_BOBView> table3DataViews = new List<TABLE7_BOBView>();
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
        //                    foreach (var pcname in pc)
        //                    {
        //                        //var pcname = pcname1.FQDN.Split('.')[0].ToUpper().Trim();
        //                        if (pcname != null && pcname != string.Empty && pcname.Count() > 0)
        //                        {
        //                          var bobItems = (from item in db.BreakOutBox_Pr where item.Date.Contains(date) && item.System_name.Contains(pcname) select new { item.Date, item.Kernel_Loadbox_Card_ECC_BreakOutBox_BOB_DeviceFunction, item.Kernel_Loadbox_Card_ECC_BreakOutBox_BOB_DeviceRevision, item.Kernel_Loadbox_Card_ECC_BreakOutBox_BOB_OptionalDescription, item.Kernel_Loadbox_Card_ECC_BreakOutBox_BOB_OrderNumber, item.Kernel_Loadbox_Card_ECC_BreakOutBox_BOB_SerialNumber, item.System_name }).GroupBy(x => new { x.Kernel_Loadbox_Card_ECC_BreakOutBox_BOB_SerialNumber, x.Kernel_Loadbox_Card_ECC_BreakOutBox_BOB_DeviceRevision }).Select(x => x.FirstOrDefault()).ToList();
        //                            // var hwItems = (from item in db.Hardware_Desc_Pr where item.Date.Contains(date) && item.System_name.Contains(pcname) select new { item.EEPName, item.Date, item.EEPBuildDate, item.EEPDatabaseVersion, item.LabCarType, item.PCHostName, item.System_name}).GroupBy(x => x.EEPName).Select(x => x.FirstOrDefault()).ToList(); //since single date descorderbydate not needed
        //                            //table3DataViews = hwDataViews.OrderByDescending(x => x.Date).GroupBy(x => x.EEPName).Select(x => x.FirstOrDefault()).OrderBy(x=>x.Date).ToList();

        //                            for (int i = 0; i < bobItems.Count(); i++)
        //                            {
        //                                TABLE7_BOBView obj = new TABLE7_BOBView();
        //                                obj.Date = DateTime.Parse(bobItems.ToArray()[i].Date).ToShortDateString();
        //                                obj.System_name = bobItems.ToArray()[i].System_name;

        //                                using (BookingServerReplicaEntities db1 = new BookingServerReplicaEntities())
        //                                {

        //                                    var labid = db1.LabComputersPrs.ToList().Find(x => x.FQDN.Split('.')[0].ToUpper().Contains(obj.System_name)).LabId;
        //                                    obj.LC_Name = db1.LabInfoes.FirstOrDefault(j => j.Id == labid).DisplayName;
        //                                }


        //                                obj.Kernel_Loadbox_Card_ECC_BreakOutBox_BOB_SerialNumber = bobItems.ToArray()[i].Kernel_Loadbox_Card_ECC_BreakOutBox_BOB_SerialNumber;
        //                                obj.Kernel_Loadbox_Card_ECC_BreakOutBox_BOB_DeviceRevision = bobItems.ToArray()[i].Kernel_Loadbox_Card_ECC_BreakOutBox_BOB_DeviceRevision;

        //                                table3DataViews.Add(obj);
        //                            }
        //                        }
        //                    }
        //                }
        //                table3DataViews = table3DataViews.OrderByDescending(x => x.Date).GroupBy(x => new { x.System_name, /*x.Date,*/ x.Kernel_Loadbox_Card_ECC_BreakOutBox_BOB_SerialNumber, x.Kernel_Loadbox_Card_ECC_BreakOutBox_BOB_DeviceRevision }).Select(x => x.FirstOrDefault()).OrderBy(x => x.Date).ToList();
        //                return Json(new { data = table3DataViews }, JsonRequestBehavior.AllowGet);
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
        //[HttpPost]
        //public ActionResult GetTABLE8_BOB1Data_unique(List<string> pc, DateTime sdate, DateTime edate)
        //{
        //    List<TABLE8_BOB1View> table4DataViews = new List<TABLE8_BOB1View>();
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
        //                    foreach (var pcname in pc)
        //                    {
        //                        //var pcname = pcname1.FQDN.Split('.')[0].ToUpper().Trim();
        //                        if (pcname != null && pcname != string.Empty && pcname.Count() > 0)
        //                        {
        //                            var bob1Items = (from item in db.BreakOutBox_1_Pr where item.System_name.Contains(pcname) && item.Date.Contains(date) select new { item.Date, item.Kernel_Loadbox_Card_ECC_BreakOutBox_BOB_SerialNumber, item.System_name }).GroupBy(x=>x.Kernel_Loadbox_Card_ECC_BreakOutBox_BOB_SerialNumber).Select(x=>x.FirstOrDefault());
        //                            // var hwItems = (from item in db.Hardware_Desc_Pr where item.Date.Contains(date) && item.System_name.Contains(pcname) select new { item.EEPName, item.Date, item.EEPBuildDate, item.EEPDatabaseVersion, item.LabCarType, item.PCHostName, item.System_name}).GroupBy(x => x.EEPName).Select(x => x.FirstOrDefault()).ToList(); //since single date descorderbydate not needed
        //                            //table3DataViews = table3DataViews.OrderByDescending(x => x.Date).GroupBy(x => x.EEPName).Select(x => x.FirstOrDefault()).OrderBy(x=>x.Date).ToList();

        //                            for (int i = 0; i < bob1Items.Count(); i++)
        //                            {
        //                                TABLE8_BOB1View obj = new TABLE8_BOB1View();
        //                                obj.Date = DateTime.Parse(bob1Items.ToArray()[i].Date).ToShortDateString();
        //                                obj.System_name = bob1Items.ToArray()[i].System_name;

        //                                using (BookingServerReplicaEntities db1 = new BookingServerReplicaEntities())
        //                                {

        //                                    var labid = db1.LabComputersPrs.ToList().Find(x => x.FQDN.Split('.')[0].ToUpper().Contains(obj.System_name)).LabId;//Find(x => x.DisplayName.ToString() == obj.System_name.ToString()).LabId;
        //                                    obj.LC_Name = db1.LabInfoes.FirstOrDefault(j => j.Id == labid).DisplayName;
        //                                }
        //                                table4DataViews.Add(obj);
        //                            }
        //                        }
        //                    }
        //                }
        //                table4DataViews = table4DataViews.OrderByDescending(x => x.Date).GroupBy(x =>  new { x.Date, x.System_name, x.Kernel_Loadbox_Card_ECC_BreakOutBox_BOB_SerialNumber }).Select(x => x.FirstOrDefault()).OrderBy(x => x.Date).ToList();
        //                return Json(new { data = table4DataViews }, JsonRequestBehavior.AllowGet);
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

        //[HttpPost]
        //public ActionResult GetTABLE9_IBData_unique(List<string> pc, DateTime sdate, DateTime edate)
        //{
        //    List<TABLE9_IBView> table5DataViews = new List<TABLE9_IBView>();
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
        //                    foreach (var pcname in pc)
        //                    {
        //                        //var pcname = pcname1.FQDN.Split('.')[0].ToUpper().Trim();
        //                        if (pcname != null && pcname != string.Empty && pcname.Count() > 0)
        //                        {

        //                            var ibItems = (from item in db.IB_Card_Pr where item.Date.Contains(date) && item.System_name.Contains(pcname) select new { item.Date, item.ModelSpecific_EID_NET_CAN_Settings_CAN_Settings_rtpc_ixxat_config, item.System_name }).GroupBy(x=>x.ModelSpecific_EID_NET_CAN_Settings_CAN_Settings_rtpc_ixxat_config).Select(x=>x.FirstOrDefault());
        //                            // var hwItems = (from item in db.Hardware_Desc_Pr where item.Date.Contains(date) && item.System_name.Contains(pcname) select new { item.EEPName, item.Date, item.EEPBuildDate, item.EEPDatabaseVersion, item.LabCarType, item.PCHostName, item.System_name}).GroupBy(x => x.EEPName).Select(x => x.FirstOrDefault()).ToList(); //since single date descorderbydate not needed
        //                            //table3DataViews = table3DataViews.OrderByDescending(x => x.Date).GroupBy(x => x.EEPName).Select(x => x.FirstOrDefault()).OrderBy(x=>x.Date).ToList();

        //                            for (int i = 0; i < ibItems.Count(); i++)
        //                            {
        //                                TABLE9_IBView obj = new TABLE9_IBView();
        //                                obj.Date = DateTime.Parse(ibItems.ToArray()[i].Date).ToShortDateString();
        //                                obj.System_name = ibItems.ToArray()[i].System_name;

        //                                using (BookingServerReplicaEntities db1 = new BookingServerReplicaEntities())
        //                                {

        //                                    var labid = db1.LabComputersPrs.ToList().Find(x => x.FQDN.Split('.')[0].ToUpper().Contains(obj.System_name)).LabId;//Find(x => x.DisplayName.ToString() == obj.System_name.ToString()).LabId;
        //                                    LabInfo labInfo = new LabInfo();
        //                                    //labInfo = db.LabInfoes.Where(item => item.Id == labid).FirstOrDefault();
        //                                    //if (labInfo != null)
        //                                    // {
        //                                    obj.LC_Name = db1.LabInfoes.FirstOrDefault(j => j.Id == labid).DisplayName;
        //                                }

        //                                db.Database.CommandTimeout = 500;

        //                                if (ibItems.Count() > 0)
        //                                {
        //                                    obj.ModelSpecific_EID_NET_CAN_Settings_CAN_Settings_rtpc_ixxat_config = ibItems.ToArray()[i].ModelSpecific_EID_NET_CAN_Settings_CAN_Settings_rtpc_ixxat_config;

        //                                }

        //                                table5DataViews.Add(obj);
        //                            }
        //                        }
        //                    }
        //                }
        //                table5DataViews = table5DataViews.OrderByDescending(x => x.Date).GroupBy(x => new { x.Date, x.System_name, x.ModelSpecific_EID_NET_CAN_Settings_CAN_Settings_rtpc_ixxat_config }).Select(x => x.FirstOrDefault()).OrderBy(x => x.Date).ToList();
        //                return Json(new { data = table5DataViews }, JsonRequestBehavior.AllowGet);
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

        //[HttpPost]
        //public ActionResult GetTABLE10_IXXATData_unique(List<string> pc, DateTime sdate, DateTime edate)
        //{
        //    List<TABLE10_IXXAT2View> table4DataViews = new List<TABLE10_IXXAT2View>();
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
        //                    foreach (var pcname in pc)
        //                    {
        //                        //var pcname = pcname1.FQDN.Split('.')[0].ToUpper().Trim();
        //                        if (pcname != null && pcname != string.Empty && pcname.Count() > 0)
        //                        {
        //                            var ixxatItems = (from item in db.IXXAT_Config_Pr where item.Date.Contains(date) && item.System_name.Contains(pcname) select new { item.Date, item.ModelSpecific_EID_NET_CAN_Settings_CAN_Settings_rtpc_ixxat_config, item.System_name }).GroupBy(x=>x.ModelSpecific_EID_NET_CAN_Settings_CAN_Settings_rtpc_ixxat_config).Select(x=>x.FirstOrDefault());
        //                            // var hwItems = (from item in db.Hardware_Desc_Pr where item.Date.Contains(date) && item.System_name.Contains(pcname) select new { item.EEPName, item.Date, item.EEPBuildDate, item.EEPDatabaseVersion, item.LabCarType, item.PCHostName, item.System_name}).GroupBy(x => x.EEPName).Select(x => x.FirstOrDefault()).ToList(); //since single date descorderbydate not needed
        //                            //table3DataViews = table3DataViews.OrderByDescending(x => x.Date).GroupBy(x => x.EEPName).Select(x => x.FirstOrDefault()).OrderBy(x=>x.Date).ToList();


        //                            for (int i = 0; i < ixxatItems.Count(); i++)
        //                            {
        //                                TABLE10_IXXAT2View obj = new TABLE10_IXXAT2View();
        //                                obj.Date = DateTime.Parse(ixxatItems.ToArray()[i].Date).ToShortDateString();
        //                                obj.System_name = ixxatItems.ToArray()[i].System_name;

        //                                using (BookingServerReplicaEntities db1 = new BookingServerReplicaEntities())
        //                                {

        //                                    var labid = db1.LabComputersPrs.ToList().Find(x => x.FQDN.Split('.')[0].ToUpper().Contains(obj.System_name)).LabId;//Find(x => x.DisplayName.ToString() == obj.System_name.ToString()).LabId;
        //                                    obj.LC_Name = db1.LabInfoes.FirstOrDefault(j => j.Id == labid).DisplayName;
        //                                }
        //                                obj.ModelSpecific_EID_NET_CAN_Settings_CAN_Settings_rtpc_ixxat_config = ixxatItems.ToArray()[i].ModelSpecific_EID_NET_CAN_Settings_CAN_Settings_rtpc_ixxat_config;
        //                                table4DataViews.Add(obj);
        //                            }
        //                        }
        //                    }
        //                }
        //                table4DataViews = table4DataViews.OrderByDescending(x => x.Date).GroupBy(x => new { /*x.Date,*/ x.System_name, x.ModelSpecific_EID_NET_CAN_Settings_CAN_Settings_rtpc_ixxat_config }).Select(x => x.FirstOrDefault()).OrderBy(x => x.Date).ToList();
        //                return Json(new { data = table4DataViews }, JsonRequestBehavior.AllowGet);
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
        //[HttpPost]
        //public ActionResult GetTABLE11_APBData_unique(List<string> pc, DateTime sdate, DateTime edate)
        //{
        //    List<TABLE11_APBView> table3DataViews = new List<TABLE11_APBView>();
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
        //                    foreach (var pcname in pc)
        //                    {
        //                        //var pcname = pcname1.FQDN.Split('.')[0].ToUpper().Trim();
        //                        if (pcname != null && pcname != string.Empty && pcname.Count() > 0)
        //                        {

        //                            var apbItems = (from item in db.APB_Pr where item.Date.Contains(date) && item.System_name.Contains(pcname) select new { item.Date, item.ModelSpecific_EID_Switch_APB_APB_Module_GIO2_Signature_GIO2, item.System_name }).GroupBy(x => x.ModelSpecific_EID_Switch_APB_APB_Module_GIO2_Signature_GIO2).Select(x => x.FirstOrDefault()).ToList(); ;
        //                            // var hwItems = (from item in db.Hardware_Desc_Pr where item.Date.Contains(date) && item.System_name.Contains(pcname) select new { item.EEPName, item.Date, item.EEPBuildDate, item.EEPDatabaseVersion, item.LabCarType, item.PCHostName, item.System_name}).GroupBy(x => x.EEPName).Select(x => x.FirstOrDefault()).ToList(); //since single date descorderbydate not needed
        //                            //table3DataViews = table3DataViews.OrderByDescending(x => x.Date).GroupBy(x => x.EEPName).Select(x => x.FirstOrDefault()).OrderBy(x=>x.Date).ToList();


        //                            for (int i = 0; i < apbItems.Count(); i++)
        //                            {
        //                                TABLE11_APBView obj = new TABLE11_APBView();
        //                                obj.Date = DateTime.Parse(apbItems.ToArray()[i].Date).ToShortDateString();
        //                                obj.System_name = apbItems.ToArray()[i].System_name;

        //                                using (BookingServerReplicaEntities db1 = new BookingServerReplicaEntities())
        //                                {

        //                                    var labid = db1.LabComputersPrs.ToList().Find(x => x.FQDN.Split('.')[0].ToUpper().Contains(obj.System_name)).LabId;
        //                                    obj.LC_Name = db1.LabInfoes.FirstOrDefault(j => j.Id == labid).DisplayName;
        //                                }

        //                                obj.ModelSpecific_EID_Switch_APB_APB_Module_GIO2_Signature_GIO2 = apbItems.ToArray()[i].ModelSpecific_EID_Switch_APB_APB_Module_GIO2_Signature_GIO2;

        //                                table3DataViews.Add(obj);
        //                            }
        //                        }
        //                    }
        //                }
        //                table3DataViews = table3DataViews.OrderByDescending(x => x.Date).GroupBy(x => new {/* x.Date,*/ x.System_name, x.ModelSpecific_EID_Switch_APB_APB_Module_GIO2_Signature_GIO2 }).Select(x => x.FirstOrDefault()).OrderBy(x => x.Date).ToList();
        //                return Json(new { data = table3DataViews }, JsonRequestBehavior.AllowGet);
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
        //[HttpPost]
        //public ActionResult GetTABLE12_HAPData_unique(List<string> pc, DateTime sdate, DateTime edate)
        //{
        //    List<TABLE12_HAPView> table4DataViews = new List<TABLE12_HAPView>();
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
        //                    foreach (var pcname in pc)
        //                    {
        //                        //var pcname = pcname1.FQDN.Split('.')[0].ToUpper().Trim();
        //                        if (pcname != null && pcname != string.Empty && pcname.Count() > 0)
        //                        {

        //                           var hapItems = (from item in db.Harnesadapter_Pr orderby item.Date where item.System_name.Contains(pcname) && item.Date.Contains(date) select new { item.Date, item.Kernel_Loadbox_Card_ECC_Harnesadapter_HAP_DeviceFunction, item.Kernel_Loadbox_Card_ECC_Harnesadapter_HAP_DeviceRevision, item.Kernel_Loadbox_Card_ECC_Harnesadapter_HAP_OptionalDescription, item.Kernel_Loadbox_Card_ECC_Harnesadapter_HAP_OrderNumber, item.Kernel_Loadbox_Card_ECC_Harnesadapter_HAP_SerialNumber, item.System_name }).GroupBy(x=>new { x.Kernel_Loadbox_Card_ECC_Harnesadapter_HAP_SerialNumber, x.Kernel_Loadbox_Card_ECC_Harnesadapter_HAP_DeviceRevision }).Select(x=>x.FirstOrDefault());

        //                            for (int i = 0; i < hapItems.Count(); i++)
        //                            {
        //                                TABLE12_HAPView obj = new TABLE12_HAPView();
        //                                obj.Date = DateTime.Parse(hapItems.ToArray()[i].Date).ToShortDateString();
        //                                obj.System_name = hapItems.ToArray()[i].System_name;

        //                                using (BookingServerReplicaEntities db1 = new BookingServerReplicaEntities())
        //                                {

        //                                    var labid = db1.LabComputersPrs.ToList().Find(x => x.FQDN.Split('.')[0].ToUpper().Contains(obj.System_name)).LabId;//Find(x => x.DisplayName.ToString() == obj.System_name.ToString()).LabId;
        //                                    obj.LC_Name = db1.LabInfoes.FirstOrDefault(j => j.Id == labid).DisplayName;
        //                                }

        //                                obj.Kernel_Loadbox_Card_ECC_Harnesadapter_HAP_SerialNumber = hapItems.ToArray()[i].Kernel_Loadbox_Card_ECC_Harnesadapter_HAP_SerialNumber;
        //                                obj.Kernel_Loadbox_Card_ECC_Harnesadapter_HAP_DeviceRevision = hapItems.ToArray()[i].Kernel_Loadbox_Card_ECC_Harnesadapter_HAP_DeviceRevision;
        //                                table4DataViews.Add(obj);
        //                            }
        //                        }
        //                    }
        //                }
        //                table4DataViews = table4DataViews.OrderByDescending(x => x.Date).GroupBy(x => new { /*x.Date,*/ x.System_name, x.Kernel_Loadbox_Card_ECC_Harnesadapter_HAP_SerialNumber, x.Kernel_Loadbox_Card_ECC_Harnesadapter_HAP_DeviceRevision }).Select(x => x.FirstOrDefault()).OrderBy(x => x.Date).ToList();
        //                return Json(new { data = table4DataViews }, JsonRequestBehavior.AllowGet);
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

        //[HttpPost]
        //public ActionResult GetTABLE13_HAP1Data_unique(List<string> pc, DateTime sdate, DateTime edate)
        //{
        //    List<TABLE13_HAP1View> table3DataViews = new List<TABLE13_HAP1View>();
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
        //                    foreach (var pcname in pc)
        //                    {
        //                        //var pcname = pcname1.FQDN.Split('.')[0].ToUpper().Trim();
        //                        if (pcname != null && pcname != string.Empty && pcname.Count() > 0)
        //                        {

        //                             var hap1Items = (from item in db.Harnesadapter_1_Pr where item.Date.Contains(date) && item.System_name.Contains(pcname) select new { item.Date, item.Kernel_Loadbox_Card_ECC_Harnesadapter_HAP_SerialNumber, item.System_name }).GroupBy(x => x.Kernel_Loadbox_Card_ECC_Harnesadapter_HAP_SerialNumber).Select(x => x.FirstOrDefault()).ToList();
        //                            // var hwItems = (from item in db.Hardware_Desc_Pr where item.Date.Contains(date) && item.System_name.Contains(pcname) select new { item.EEPName, item.Date, item.EEPBuildDate, item.EEPDatabaseVersion, item.LabCarType, item.PCHostName, item.System_name}).GroupBy(x => x.EEPName).Select(x => x.FirstOrDefault()).ToList(); //since single date descorderbydate not needed
        //                            //table3DataViews = table3DataViews.OrderByDescending(x => x.Date).GroupBy(x => x.EEPName).Select(x => x.FirstOrDefault()).OrderBy(x=>x.Date).ToList();

        //                            for (int i = 0; i < hap1Items.Count(); i++)
        //                            {
        //                                TABLE13_HAP1View obj = new TABLE13_HAP1View();
        //                                obj.Date = DateTime.Parse(hap1Items.ToArray()[i].Date).ToShortDateString();
        //                                obj.System_name = hap1Items.ToArray()[i].System_name;

        //                                using (BookingServerReplicaEntities db1 = new BookingServerReplicaEntities())
        //                                {

        //                                    var labid = db1.LabComputersPrs.ToList().Find(x => x.FQDN.Split('.')[0].ToUpper().Contains(obj.System_name)).LabId;
        //                                    obj.LC_Name = db1.LabInfoes.FirstOrDefault(j => j.Id == labid).DisplayName;
        //                                }

        //                                obj.Kernel_Loadbox_Card_ECC_Harnesadapter_HAP_SerialNumber = hap1Items.ToArray()[i].Kernel_Loadbox_Card_ECC_Harnesadapter_HAP_SerialNumber;

        //                                table3DataViews.Add(obj);
        //                            }
        //                        }
        //                    }
        //                }
        //                table3DataViews = table3DataViews.OrderByDescending(x => x.Date).GroupBy(x => new { x.Date, x.System_name, x.Kernel_Loadbox_Card_ECC_Harnesadapter_HAP_SerialNumber }).Select(x => x.FirstOrDefault()).OrderBy(x => x.Date).ToList();
        //                return Json(new { data = table3DataViews }, JsonRequestBehavior.AllowGet);
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

        //[HttpPost]
        //public ActionResult GetTABLE14_CableData_unique(List<string> pc, DateTime sdate, DateTime edate)
        //{
        //    List<TABLE14_CableView> table3DataViews = new List<TABLE14_CableView>();
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
        //                    foreach (var pcname in pc)
        //                    {
        //                        //var pcname = pcname1.FQDN.Split('.')[0].ToUpper().Trim();
        //                        if (pcname != null && pcname != string.Empty && pcname.Count() > 0)
        //                        {
        //                            var cableItems = (from item in db.Cable_Pr where item.Date.Contains(date) && item.System_name.Contains(pcname) select new { item.Date, item.Kernel_Loadbox_Card_ECC_Cable_CABLE_DeviceFunction, item.Kernel_Loadbox_Card_ECC_Cable_CABLE_DeviceRevision, item.Kernel_Loadbox_Card_ECC_Cable_CABLE_OptionalDescription, item.Kernel_Loadbox_Card_ECC_Cable_CABLE_OrderNumber, item.Kernel_Loadbox_Card_ECC_Cable_CABLE_SerialNumber, item.System_name }).GroupBy(x => new { x.Kernel_Loadbox_Card_ECC_Cable_CABLE_SerialNumber, x.Kernel_Loadbox_Card_ECC_Cable_CABLE_DeviceRevision }).Select(x => x.FirstOrDefault()).ToList();

        //                            for (int i = 0; i < cableItems.Count(); i++)
        //                            {
        //                                TABLE14_CableView obj = new TABLE14_CableView();
        //                                obj.Date = DateTime.Parse(cableItems.ToArray()[i].Date).ToShortDateString();
        //                                obj.System_name = cableItems.ToArray()[i].System_name;

        //                                using (BookingServerReplicaEntities db1 = new BookingServerReplicaEntities())
        //                                {

        //                                    var labid = db1.LabComputersPrs.ToList().Find(x => x.FQDN.Split('.')[0].ToUpper().Contains(obj.System_name)).LabId;
        //                                    obj.LC_Name = db1.LabInfoes.FirstOrDefault(j => j.Id == labid).DisplayName;
        //                                }

        //                                obj.Kernel_Loadbox_Card_ECC_Cable_CABLE_SerialNumber = cableItems.ToArray()[i].Kernel_Loadbox_Card_ECC_Cable_CABLE_SerialNumber;
        //                                obj.Kernel_Loadbox_Card_ECC_Cable_CABLE_DeviceRevision = cableItems.ToArray()[i].Kernel_Loadbox_Card_ECC_Cable_CABLE_DeviceRevision;

        //                                table3DataViews.Add(obj);
        //                            }
        //                        }
        //                    }
        //                }
        //                table3DataViews = table3DataViews.OrderByDescending(x => x.Date).GroupBy(x => new {/* x.Date,*/ x.System_name, x.Kernel_Loadbox_Card_ECC_Cable_CABLE_SerialNumber, x.Kernel_Loadbox_Card_ECC_Cable_CABLE_DeviceRevision }).Select(x => x.FirstOrDefault()).OrderBy(x => x.Date).ToList();
        //                return Json(new { data = table3DataViews }, JsonRequestBehavior.AllowGet);
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
        //[HttpPost]
        //public ActionResult GetTABLE15_Cable1Data_unique(List<string> pc, DateTime sdate, DateTime edate)
        //{
        //    List<TABLE15_Cable1View> table4DataViews = new List<TABLE15_Cable1View>();
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
        //                    foreach (var pcname in pc)
        //                    {
        //                        //var pcname = pcname1.FQDN.Split('.')[0].ToUpper().Trim();
        //                        if (pcname != null && pcname != string.Empty && pcname.Count() > 0)
        //                        {

        //                            var cable1Items = (from item in db.Cable_1_Pr where item.Date.Contains(date) && item.System_name.Contains(pcname)  select new { item.Date, item.Kernel_Loadbox_Card_ECC_Cable_CABLE_SerialNumber, item.System_name }).GroupBy(x=>x.Kernel_Loadbox_Card_ECC_Cable_CABLE_SerialNumber).Select(x=>x.FirstOrDefault());                                   

        //                            for (int i = 0; i < cable1Items.Count(); i++)
        //                            {
        //                                TABLE15_Cable1View obj = new TABLE15_Cable1View();
        //                                obj.Date = DateTime.Parse(cable1Items.ToArray()[i].Date).ToShortDateString();
        //                                obj.System_name = cable1Items.ToArray()[i].System_name;

        //                                using (BookingServerReplicaEntities db1 = new BookingServerReplicaEntities())
        //                                {

        //                                    var labid = db1.LabComputersPrs.ToList().Find(x => x.FQDN.Split('.')[0].ToUpper().Contains(obj.System_name)).LabId;//Find(x => x.DisplayName.ToString() == obj.System_name.ToString()).LabId;
        //                                    obj.LC_Name = db1.LabInfoes.FirstOrDefault(j => j.Id == labid).DisplayName;
        //                                }

        //                                obj.Kernel_Loadbox_Card_ECC_Cable_CABLE_SerialNumber = cable1Items.ToArray()[i].Kernel_Loadbox_Card_ECC_Cable_CABLE_SerialNumber;
        //                                table4DataViews.Add(obj);
        //                            }
        //                        }
        //                    }
        //                }
        //                table4DataViews = table4DataViews.OrderByDescending(x => x.Date).GroupBy(x =>new { x.Date, x.System_name, x.Kernel_Loadbox_Card_ECC_Cable_CABLE_SerialNumber }).Select(x => x.FirstOrDefault()).OrderBy(x => x.Date).ToList();
        //                return Json(new { data = table4DataViews }, JsonRequestBehavior.AllowGet);
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
        //[HttpPost]
        //public ActionResult GetTABLE16_ECCData_unique(List<string> pc, DateTime sdate, DateTime edate)
        //{
        //    List<TABLE16_ECCView> table5DataViews = new List<TABLE16_ECCView>();
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
        //                    foreach (var pcname in pc)
        //                    {
        //                        //var pcname = pcname1.FQDN.Split('.')[0].ToUpper().Trim();
        //                        if (pcname != null && pcname != string.Empty && pcname.Count() > 0)
        //                        {

        //                            var eccItems = (from item in db.ECC_Card_Pr where item.Date.Contains(date) && item.System_name.Contains(pcname) select new { item.Date, item.Kernel_Loadbox_Card_ECC_BuildVersion, item.Kernel_Loadbox_Card_ECC_Calibration_Day, item.Kernel_Loadbox_Card_ECC_Calibration_Month, item.Kernel_Loadbox_Card_ECC_Calibration_Year, item.Kernel_Loadbox_Card_ECC_CARD_BoardRevision, item.Kernel_Loadbox_Card_ECC_CARD_BoardType, item.Kernel_Loadbox_Card_ECC_CARD_Description, item.Kernel_Loadbox_Card_ECC_CARD_VER_TAG_ARRAY, item.Kernel_Loadbox_Card_ECC_Day_of_Creation, item.Kernel_Loadbox_Card_ECC_Firmware_Card_Major, item.Kernel_Loadbox_Card_ECC_Firmware_Card_Minor, item.Kernel_Loadbox_Card_ECC_Firmware_FPGA_Major, item.Kernel_Loadbox_Card_ECC_Firmware_FPGA_Minor, item.Kernel_Loadbox_Card_ECC_Month_of_Creation, item.Kernel_Loadbox_Card_ECC_Next_Calibration_Day, item.Kernel_Loadbox_Card_ECC_Next_Calibration_Month, item.Kernel_Loadbox_Card_ECC_Next_Calibration_Year, item.Kernel_Loadbox_Card_ECC_PowerOnTime, item.Kernel_Loadbox_Card_ECC_RepairCounter, item.Kernel_Loadbox_Card_ECC_Serial_No, item.Kernel_Loadbox_Card_ECC_Year_of_Creation, item.System_name }).GroupBy(x=>new { x.Kernel_Loadbox_Card_ECC_Serial_No, x.Kernel_Loadbox_Card_ECC_Firmware_Card_Minor }).Select(x=>x.FirstOrDefault());

        //                            for (int i = 0; i < eccItems.Count(); i++)
        //                            {
        //                                TABLE16_ECCView obj = new TABLE16_ECCView();
        //                                obj.Date = DateTime.Parse(eccItems.ToArray()[i].Date).ToShortDateString();
        //                                obj.System_name = eccItems.ToArray()[i].System_name;

        //                                using (BookingServerReplicaEntities db1 = new BookingServerReplicaEntities())
        //                                {

        //                                    var labid = db1.LabComputersPrs.ToList().Find(x => x.FQDN.Split('.')[0].ToUpper().Contains(obj.System_name)).LabId;//Find(x => x.DisplayName.ToString() == obj.System_name.ToString()).LabId;
        //                                    LabInfo labInfo = new LabInfo();
        //                                    //labInfo = db.LabInfoes.Where(item => item.Id == labid).FirstOrDefault();
        //                                    //if (labInfo != null)
        //                                    // {
        //                                    obj.LC_Name = db1.LabInfoes.FirstOrDefault(j => j.Id == labid).DisplayName;
        //                                }

        //                                db.Database.CommandTimeout = 500;
        //                                obj.Kernel_Loadbox_Card_ECC_CARD_BoardType = eccItems.ToArray()[i].Kernel_Loadbox_Card_ECC_CARD_BoardType;
        //                                obj.Kernel_Loadbox_Card_ECC_CARD_BoardRevision = eccItems.ToArray()[i].Kernel_Loadbox_Card_ECC_CARD_BoardRevision;
        //                                obj.Kernel_Loadbox_Card_ECC_CARD_VER_TAG_ARRAY = eccItems.ToArray()[i].Kernel_Loadbox_Card_ECC_CARD_VER_TAG_ARRAY;
        //                                obj.Kernel_Loadbox_Card_ECC_Serial_No = eccItems.ToArray()[i].Kernel_Loadbox_Card_ECC_Serial_No;
        //                                obj.Kernel_Loadbox_Card_ECC_Firmware_Card_Major = eccItems.ToArray()[i].Kernel_Loadbox_Card_ECC_Firmware_Card_Major;
        //                                obj.Kernel_Loadbox_Card_ECC_Firmware_Card_Minor = eccItems.ToArray()[i].Kernel_Loadbox_Card_ECC_Firmware_Card_Minor;
        //                                obj.Kernel_Loadbox_Card_ECC_Firmware_FPGA_Major = eccItems.ToArray()[i].Kernel_Loadbox_Card_ECC_Firmware_FPGA_Major;
        //                                obj.Kernel_Loadbox_Card_ECC_Firmware_FPGA_Minor = eccItems.ToArray()[i].Kernel_Loadbox_Card_ECC_Firmware_FPGA_Minor;
        //                                obj.Kernel_Loadbox_Card_ECC_Calibration_Day = eccItems.ToArray()[i].Kernel_Loadbox_Card_ECC_Calibration_Day;
        //                                obj.Kernel_Loadbox_Card_ECC_Calibration_Month = eccItems.ToArray()[i].Kernel_Loadbox_Card_ECC_Calibration_Month;
        //                                obj.Kernel_Loadbox_Card_ECC_Calibration_Year = eccItems.ToArray()[i].Kernel_Loadbox_Card_ECC_Calibration_Year;
        //                                obj.Kernel_Loadbox_Card_ECC_Next_Calibration_Day = eccItems.ToArray()[i].Kernel_Loadbox_Card_ECC_Next_Calibration_Day;
        //                                obj.Kernel_Loadbox_Card_ECC_Next_Calibration_Month = eccItems.ToArray()[i].Kernel_Loadbox_Card_ECC_Next_Calibration_Month;
        //                                obj.Kernel_Loadbox_Card_ECC_Next_Calibration_Year = eccItems.ToArray()[i].Kernel_Loadbox_Card_ECC_Next_Calibration_Year;
        //                                obj.Kernel_Loadbox_Card_ECC_Day_of_Creation = eccItems.ToArray()[i].Kernel_Loadbox_Card_ECC_Day_of_Creation;
        //                                obj.Kernel_Loadbox_Card_ECC_Month_of_Creation = eccItems.ToArray()[i].Kernel_Loadbox_Card_ECC_Month_of_Creation;
        //                                obj.Kernel_Loadbox_Card_ECC_Year_of_Creation = eccItems.ToArray()[i].Kernel_Loadbox_Card_ECC_Year_of_Creation;
        //                                obj.Kernel_Loadbox_Card_ECC_BuildVersion = eccItems.ToArray()[i].Kernel_Loadbox_Card_ECC_BuildVersion;



        //                                table5DataViews.Add(obj);
        //                            }
        //                        }
        //                    }
        //                }
        //                table5DataViews = table5DataViews.OrderByDescending(x => x.Date).GroupBy(x => new {/* x.Date,*/ x.System_name, x.Kernel_Loadbox_Card_ECC_Serial_No, x.Kernel_Loadbox_Card_ECC_Firmware_Card_Minor }).Select(x => x.FirstOrDefault()).OrderBy(x => x.Date).ToList();
        //                return Json(new { data = table5DataViews }, JsonRequestBehavior.AllowGet);
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

        //[HttpPost]
        //public ActionResult GetTABLE17_GIO1Data_unique(List<string> pc, DateTime sdate, DateTime edate)
        //{
        //    List<TABLE6_GIO1View> table6DataViews = new List<TABLE6_GIO1View>();
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
        //                    foreach (var pcname in pc)
        //                    {
        //                        //var pcname = pcname1.FQDN.Split('.')[0].ToUpper().Trim();
        //                        if (pcname != null && pcname != string.Empty && pcname.Count() > 0)
        //                        {

        //                            var gio1Items = (from item in db.GIO1_Card_Pr  where item.Date.Contains(date) && item.System_name.Contains(pcname)   select new { item.Date, item.Kernel_Loadbox_Card_GIO1_BuildVersion, item.Kernel_Loadbox_Card_GIO1_Calibration_Day, item.Kernel_Loadbox_Card_GIO1_Calibration_Month, item.Kernel_Loadbox_Card_GIO1_Calibration_Year, item.Kernel_Loadbox_Card_GIO1_CARD_BoardRevision, item.Kernel_Loadbox_Card_GIO1_CARD_BoardType, item.Kernel_Loadbox_Card_GIO1_CARD_Description, item.Kernel_Loadbox_Card_GIO1_CARD_VER_TAG_ARRAY, item.Kernel_Loadbox_Card_GIO1_Day_of_Creation, item.Kernel_Loadbox_Card_GIO1_Firmware_Card_Major, item.Kernel_Loadbox_Card_GIO1_Firmware_Card_Minor, item.Kernel_Loadbox_Card_GIO1_Firmware_FPGA_Major, item.Kernel_Loadbox_Card_GIO1_Firmware_FPGA_Minor, item.Kernel_Loadbox_Card_GIO1_Month_of_Creation, item.Kernel_Loadbox_Card_GIO1_Next_Calibration_Day, item.Kernel_Loadbox_Card_GIO1_Next_Calibration_Month, item.Kernel_Loadbox_Card_GIO1_Next_Calibration_Year, item.Kernel_Loadbox_Card_GIO1_PowerOnTime, item.Kernel_Loadbox_Card_GIO1_RepairCounter, item.Kernel_Loadbox_Card_GIO1_Serial_No, item.Kernel_Loadbox_Card_GIO1_Year_of_Creation, item.System_name }).GroupBy(x=>new { x.Kernel_Loadbox_Card_GIO1_Serial_No, x.Kernel_Loadbox_Card_GIO1_Firmware_Card_Minor }).Select(x=>x.FirstOrDefault());
        //                            // var hwItems = (from item in db.Hardware_Desc_Pr where item.Date.Contains(date) && item.System_name.Contains(pcname) select new { item.EEPName, item.Date, item.EEPBuildDate, item.EEPDatabaseVersion, item.LabCarType, item.PCHostName, item.System_name, item.RBCCAFVersion, item.RTPCName, item.RTPCSoftwareVersion, item.ToolChainVersion }).GroupBy(x => x.EEPName).Select(x => x.FirstOrDefault()).ToList(); //since single date descorderbydate not needed
        //                            //hwDataViews = hwDataViews.OrderByDescending(x => x.Date).GroupBy(x => x.EEPName).Select(x => x.FirstOrDefault()).OrderBy(x=>x.Date).ToList();

        //                            for (int i = 0; i < gio1Items.Count(); i++)
        //                            {
        //                                TABLE6_GIO1View obj = new TABLE6_GIO1View();
        //                                obj.Date = DateTime.Parse(gio1Items.ToArray()[i].Date).ToShortDateString();
        //                                obj.System_name = gio1Items.ToArray()[i].System_name;

        //                                using (BookingServerReplicaEntities db1 = new BookingServerReplicaEntities())
        //                                {

        //                                    var labid = db1.LabComputersPrs.ToList().Find(x => x.FQDN.Split('.')[0].ToUpper().Contains(obj.System_name)).LabId;//Find(x => x.DisplayName.ToString() == obj.System_name.ToString()).LabId;
        //                                    LabInfo labInfo = new LabInfo();
        //                                    //labInfo = db.LabInfoes.Where(item => item.Id == labid).FirstOrDefault();
        //                                    //if (labInfo != null)
        //                                    // {
        //                                    obj.LC_Name = db1.LabInfoes.FirstOrDefault(j => j.Id == labid).DisplayName;
        //                                }

        //                                obj.Kernel_Loadbox_Card_GIO1_CARD_BoardType = gio1Items.ToArray()[i].Kernel_Loadbox_Card_GIO1_CARD_BoardType;
        //                                obj.Kernel_Loadbox_Card_GIO1_CARD_BoardRevision = gio1Items.ToArray()[i].Kernel_Loadbox_Card_GIO1_CARD_BoardRevision;
        //                                obj.Kernel_Loadbox_Card_GIO1_CARD_VER_TAG_ARRAY = gio1Items.ToArray()[i].Kernel_Loadbox_Card_GIO1_CARD_VER_TAG_ARRAY;
        //                                obj.Kernel_Loadbox_Card_GIO1_Serial_No = gio1Items.ToArray()[i].Kernel_Loadbox_Card_GIO1_Serial_No;
        //                                obj.Kernel_Loadbox_Card_GIO1_Firmware_Card_Major = gio1Items.ToArray()[i].Kernel_Loadbox_Card_GIO1_Firmware_Card_Major;
        //                                obj.Kernel_Loadbox_Card_GIO1_Firmware_Card_Minor = gio1Items.ToArray()[i].Kernel_Loadbox_Card_GIO1_Firmware_Card_Minor;
        //                                obj.Kernel_Loadbox_Card_GIO1_Firmware_FPGA_Major = gio1Items.ToArray()[i].Kernel_Loadbox_Card_GIO1_Firmware_FPGA_Major;
        //                                obj.Kernel_Loadbox_Card_GIO1_Firmware_FPGA_Minor = gio1Items.ToArray()[i].Kernel_Loadbox_Card_GIO1_Firmware_FPGA_Minor;
        //                                obj.Kernel_Loadbox_Card_GIO1_Calibration_Day = gio1Items.ToArray()[i].Kernel_Loadbox_Card_GIO1_Calibration_Day;
        //                                obj.Kernel_Loadbox_Card_GIO1_Calibration_Month = gio1Items.ToArray()[i].Kernel_Loadbox_Card_GIO1_Calibration_Month;
        //                                obj.Kernel_Loadbox_Card_GIO1_Calibration_Year = gio1Items.ToArray()[i].Kernel_Loadbox_Card_GIO1_Calibration_Year;
        //                                obj.Kernel_Loadbox_Card_GIO1_Next_Calibration_Day = gio1Items.ToArray()[i].Kernel_Loadbox_Card_GIO1_Next_Calibration_Day;
        //                                obj.Kernel_Loadbox_Card_GIO1_Next_Calibration_Month = gio1Items.ToArray()[i].Kernel_Loadbox_Card_GIO1_Next_Calibration_Month;
        //                                obj.Kernel_Loadbox_Card_GIO1_Next_Calibration_Year = gio1Items.ToArray()[i].Kernel_Loadbox_Card_GIO1_Next_Calibration_Year;
        //                                obj.Kernel_Loadbox_Card_GIO1_Day_of_Creation = gio1Items.ToArray()[i].Kernel_Loadbox_Card_GIO1_Day_of_Creation;
        //                                obj.Kernel_Loadbox_Card_GIO1_Month_of_Creation = gio1Items.ToArray()[i].Kernel_Loadbox_Card_GIO1_Month_of_Creation;
        //                                obj.Kernel_Loadbox_Card_GIO1_Year_of_Creation = gio1Items.ToArray()[i].Kernel_Loadbox_Card_GIO1_Year_of_Creation;
        //                                obj.Kernel_Loadbox_Card_GIO1_BuildVersion = gio1Items.ToArray()[i].Kernel_Loadbox_Card_GIO1_BuildVersion;

        //                                table6DataViews.Add(obj);
        //                            }
        //                        }
        //                    }
        //                }
        //                table6DataViews = table6DataViews.OrderByDescending(x => x.Date).GroupBy(x => new { /*x.Date,*/ x.System_name, x.Kernel_Loadbox_Card_GIO1_Serial_No, x.Kernel_Loadbox_Card_GIO1_Firmware_Card_Minor }).Select(x => x.FirstOrDefault()).OrderBy(x => x.Date).ToList();
        //                return Json(new { data = table6DataViews }, JsonRequestBehavior.AllowGet);
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

        //[HttpPost]
        //public ActionResult GetTABLE18_GIO2Data_unique(List<string> pc, DateTime sdate, DateTime edate)
        //{
        //    List<TABLE7_GIO2View> table7DataViews = new List<TABLE7_GIO2View>();
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
        //                    foreach (var pcname in pc)
        //                    {
        //                        //var pcname = pcname1.FQDN.Split('.')[0].ToUpper().Trim();
        //                        {
        //                            if (pcname != null && pcname != string.Empty && pcname.Count() > 0)
        //                            {


        //                                var gio2Items = (from item in db.GIO2_Card_Pr where item.Date.Contains(date) && item.System_name.Contains(pcname) select new { item.Date, item.Kernel_Loadbox_Card_GIO2_BuildVersion, item.Kernel_Loadbox_Card_GIO2_Calibration_Day, item.Kernel_Loadbox_Card_GIO2_Calibration_Month, item.Kernel_Loadbox_Card_GIO2_Calibration_Year, item.Kernel_Loadbox_Card_GIO2_CARD_BoardRevision, item.Kernel_Loadbox_Card_GIO2_CARD_BoardType, item.Kernel_Loadbox_Card_GIO2_CARD_Description, item.Kernel_Loadbox_Card_GIO2_CARD_VER_TAG_ARRAY, item.Kernel_Loadbox_Card_GIO2_Day_of_Creation, item.Kernel_Loadbox_Card_GIO2_Firmware_Card_Major, item.Kernel_Loadbox_Card_GIO2_Firmware_Card_Minor, item.Kernel_Loadbox_Card_GIO2_Firmware_FPGA_Major, item.Kernel_Loadbox_Card_GIO2_Firmware_FPGA_Minor, item.Kernel_Loadbox_Card_GIO2_Month_of_Creation, item.Kernel_Loadbox_Card_GIO2_Next_Calibration_Day, item.Kernel_Loadbox_Card_GIO2_Next_Calibration_Month, item.Kernel_Loadbox_Card_GIO2_Next_Calibration_Year, item.Kernel_Loadbox_Card_GIO2_PowerOnTime, item.Kernel_Loadbox_Card_GIO2_RepairCounter, item.Kernel_Loadbox_Card_GIO2_Serial_No, item.Kernel_Loadbox_Card_GIO2_Year_of_Creation, item.System_name }).GroupBy(x=>new { x.Kernel_Loadbox_Card_GIO2_Serial_No, x.Kernel_Loadbox_Card_GIO2_Firmware_Card_Minor }).Select(x=>x.FirstOrDefault());
        //                                // var hwItems = (from item in db.Hardware_Desc_Pr where item.Date.Contains(date) && item.System_name.Contains(pcname) select new { item.EEPName, item.Date, item.EEPBuildDate, item.EEPDatabaseVersion, item.LabCarType, item.PCHostName, item.System_name, item.RBCCAFVersion, item.RTPCName, item.RTPCSoftwareVersion, item.ToolChainVersion }).GroupBy(x => x.EEPName).Select(x => x.FirstOrDefault()).ToList(); //since single date descorderbydate not needed
        //                                //hwDataViews = hwDataViews.OrderByDescending(x => x.Date).GroupBy(x => x.EEPName).Select(x => x.FirstOrDefault()).OrderBy(x=>x.Date).ToList();

        //                                for (int i = 0; i < gio2Items.Count(); i++)
        //                                {
        //                                    TABLE7_GIO2View obj = new TABLE7_GIO2View();
        //                                    obj.Date = DateTime.Parse(gio2Items.ToArray()[i].Date).ToShortDateString();
        //                                    obj.System_name = gio2Items.ToArray()[i].System_name;

        //                                    using (BookingServerReplicaEntities db1 = new BookingServerReplicaEntities())
        //                                    {

        //                                        var labid = db1.LabComputersPrs.ToList().Find(x => x.FQDN.Split('.')[0].ToUpper().Contains(obj.System_name)).LabId;//Find(x => x.DisplayName.ToString() == obj.System_name.ToString()).LabId;
        //                                        LabInfo labInfo = new LabInfo();
        //                                        //labInfo = db.LabInfoes.Where(item => item.Id == labid).FirstOrDefault();
        //                                        //if (labInfo != null)
        //                                        // {
        //                                        obj.LC_Name = db1.LabInfoes.FirstOrDefault(j => j.Id == labid).DisplayName;
        //                                    }


        //                                    obj.Kernel_Loadbox_Card_GIO2_CARD_BoardType = gio2Items.ToArray()[i].Kernel_Loadbox_Card_GIO2_CARD_BoardType;
        //                                    obj.Kernel_Loadbox_Card_GIO2_CARD_BoardRevision = gio2Items.ToArray()[i].Kernel_Loadbox_Card_GIO2_CARD_BoardRevision;
        //                                    obj.Kernel_Loadbox_Card_GIO2_CARD_VER_TAG_ARRAY = gio2Items.ToArray()[i].Kernel_Loadbox_Card_GIO2_CARD_VER_TAG_ARRAY;
        //                                    obj.Kernel_Loadbox_Card_GIO2_Serial_No = gio2Items.ToArray()[i].Kernel_Loadbox_Card_GIO2_Serial_No;
        //                                    obj.Kernel_Loadbox_Card_GIO2_Firmware_Card_Major = gio2Items.ToArray()[i].Kernel_Loadbox_Card_GIO2_Firmware_Card_Major;
        //                                    obj.Kernel_Loadbox_Card_GIO2_Firmware_Card_Minor = gio2Items.ToArray()[i].Kernel_Loadbox_Card_GIO2_Firmware_Card_Minor;
        //                                    obj.Kernel_Loadbox_Card_GIO2_Firmware_FPGA_Major = gio2Items.ToArray()[i].Kernel_Loadbox_Card_GIO2_Firmware_FPGA_Major;
        //                                    obj.Kernel_Loadbox_Card_GIO2_Firmware_FPGA_Minor = gio2Items.ToArray()[i].Kernel_Loadbox_Card_GIO2_Firmware_FPGA_Minor;
        //                                    obj.Kernel_Loadbox_Card_GIO2_Calibration_Day = gio2Items.ToArray()[i].Kernel_Loadbox_Card_GIO2_Calibration_Day;
        //                                    obj.Kernel_Loadbox_Card_GIO2_Calibration_Month = gio2Items.ToArray()[i].Kernel_Loadbox_Card_GIO2_Calibration_Month;
        //                                    obj.Kernel_Loadbox_Card_GIO2_Calibration_Year = gio2Items.ToArray()[i].Kernel_Loadbox_Card_GIO2_Calibration_Year;
        //                                    obj.Kernel_Loadbox_Card_GIO2_Next_Calibration_Day = gio2Items.ToArray()[i].Kernel_Loadbox_Card_GIO2_Next_Calibration_Day;
        //                                    obj.Kernel_Loadbox_Card_GIO2_Next_Calibration_Month = gio2Items.ToArray()[i].Kernel_Loadbox_Card_GIO2_Next_Calibration_Month;
        //                                    obj.Kernel_Loadbox_Card_GIO2_Next_Calibration_Year = gio2Items.ToArray()[i].Kernel_Loadbox_Card_GIO2_Next_Calibration_Year;
        //                                    obj.Kernel_Loadbox_Card_GIO2_Day_of_Creation = gio2Items.ToArray()[i].Kernel_Loadbox_Card_GIO2_Day_of_Creation;
        //                                    obj.Kernel_Loadbox_Card_GIO2_Month_of_Creation = gio2Items.ToArray()[i].Kernel_Loadbox_Card_GIO2_Month_of_Creation;
        //                                    obj.Kernel_Loadbox_Card_GIO2_Year_of_Creation = gio2Items.ToArray()[i].Kernel_Loadbox_Card_GIO2_Year_of_Creation;
        //                                    obj.Kernel_Loadbox_Card_GIO2_BuildVersion = gio2Items.ToArray()[i].Kernel_Loadbox_Card_GIO2_BuildVersion;

        //                                    table7DataViews.Add(obj);
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //                table7DataViews = table7DataViews.OrderByDescending(x => x.Date).GroupBy(x => new { /*x.Date,*/ x.System_name, x.Kernel_Loadbox_Card_GIO2_Serial_No, x.Kernel_Loadbox_Card_GIO2_Firmware_Card_Minor }).Select(x => x.FirstOrDefault()).OrderBy(x => x.Date).ToList();
        //                return Json(new { data = table7DataViews }, JsonRequestBehavior.AllowGet);
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

        //[HttpPost]
        //public ActionResult GetTABLE19_LDUData_unique(List<string> pc, DateTime sdate, DateTime edate)
        //{
        //    List<TABLE8_LDUView> table8DataViews = new List<TABLE8_LDUView>();
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
        //                    foreach (var pcname in pc)
        //                    {
        //                        //var pcname = pcname1.FQDN.Split('.')[0].ToUpper().Trim();
        //                        if (pcname != null && pcname != string.Empty && pcname.Count() > 0)
        //                        {

        //                            var lduItems = (from item in db.LDU_Card_Pr where item.Date.Contains(date) && item.System_name.Contains(pcname) select new { item.Date, item.Kernel_Loadbox_Card_LDU_BuildVersion, item.Kernel_Loadbox_Card_LDU_Calibration_Day, item.Kernel_Loadbox_Card_LDU_Calibration_Month, item.Kernel_Loadbox_Card_LDU_Calibration_Year, item.Kernel_Loadbox_Card_LDU_CARD_BoardRevision, item.Kernel_Loadbox_Card_LDU_CARD_BoardType, item.Kernel_Loadbox_Card_LDU_CARD_Description, item.Kernel_Loadbox_Card_LDU_CARD_VER_TAG_ARRAY, item.Kernel_Loadbox_Card_LDU_Day_of_Creation, item.Kernel_Loadbox_Card_LDU_Firmware_Card_Major, item.Kernel_Loadbox_Card_LDU_Firmware_Card_Minor, item.Kernel_Loadbox_Card_LDU_Firmware_FPGA_Major, item.Kernel_Loadbox_Card_LDU_Firmware_FPGA_Minor, item.Kernel_Loadbox_Card_LDU_Month_of_Creation, item.Kernel_Loadbox_Card_LDU_Next_Calibration_Day, item.Kernel_Loadbox_Card_LDU_Next_Calibration_Month, item.Kernel_Loadbox_Card_LDU_Next_Calibration_Year, item.Kernel_Loadbox_Card_LDU_PowerOnTime, item.Kernel_Loadbox_Card_LDU_RepairCounter, item.Kernel_Loadbox_Card_LDU_Serial_No, item.Kernel_Loadbox_Card_LDU_Year_of_Creation, item.System_name }).GroupBy(x=>new { x.Kernel_Loadbox_Card_LDU_Serial_No, x.Kernel_Loadbox_Card_LDU_Firmware_Card_Minor }).Select(x=>x.FirstOrDefault());
        //                            // var hwItems = (from item in db.Hardware_Desc_Pr where item.Date.Contains(date) && item.System_name.Contains(pcname) select new { item.EEPName, item.Date, item.EEPBuildDate, item.EEPDatabaseVersion, item.LabCarType, item.PCHostName, item.System_name, item.RBCCAFVersion, item.RTPCName, item.RTPCSoftwareVersion, item.ToolChainVersion }).GroupBy(x => x.EEPName).Select(x => x.FirstOrDefault()).ToList(); //since single date descorderbydate not needed
        //                            //hwDataViews = hwDataViews.OrderByDescending(x => x.Date).GroupBy(x => x.EEPName).Select(x => x.FirstOrDefault()).OrderBy(x=>x.Date).ToList();

        //                            for (int i = 0; i < lduItems.Count(); i++)
        //                            {
        //                                TABLE8_LDUView obj = new TABLE8_LDUView();
        //                                obj.Date = DateTime.Parse(lduItems.ToArray()[i].Date).ToShortDateString();
        //                                obj.System_name = lduItems.ToArray()[i].System_name;

        //                                using (BookingServerReplicaEntities db1 = new BookingServerReplicaEntities())
        //                                {

        //                                    var labid = db1.LabComputersPrs.ToList().Find(x => x.FQDN.Split('.')[0].ToUpper().Contains(obj.System_name)).LabId;//Find(x => x.DisplayName.ToString() == obj.System_name.ToString()).LabId;
        //                                    LabInfo labInfo = new LabInfo();
        //                                    //labInfo = db.LabInfoes.Where(item => item.Id == labid).FirstOrDefault();
        //                                    //if (labInfo != null)
        //                                    // {
        //                                    obj.LC_Name = db1.LabInfoes.FirstOrDefault(j => j.Id == labid).DisplayName;
        //                                }

        //                                obj.Kernel_Loadbox_Card_LDU_CARD_BoardType = lduItems.ToArray()[i].Kernel_Loadbox_Card_LDU_CARD_BoardType;
        //                                obj.Kernel_Loadbox_Card_LDU_CARD_BoardRevision = lduItems.ToArray()[i].Kernel_Loadbox_Card_LDU_CARD_BoardRevision;
        //                                obj.Kernel_Loadbox_Card_LDU_CARD_VER_TAG_ARRAY = lduItems.ToArray()[i].Kernel_Loadbox_Card_LDU_CARD_VER_TAG_ARRAY;
        //                                obj.Kernel_Loadbox_Card_LDU_Serial_No = lduItems.ToArray()[i].Kernel_Loadbox_Card_LDU_Serial_No;
        //                                obj.Kernel_Loadbox_Card_LDU_Firmware_Card_Major = lduItems.ToArray()[i].Kernel_Loadbox_Card_LDU_Firmware_Card_Major;
        //                                obj.Kernel_Loadbox_Card_LDU_Firmware_Card_Minor = lduItems.ToArray()[i].Kernel_Loadbox_Card_LDU_Firmware_Card_Minor;
        //                                obj.Kernel_Loadbox_Card_LDU_Firmware_FPGA_Major = lduItems.ToArray()[i].Kernel_Loadbox_Card_LDU_Firmware_FPGA_Major;
        //                                obj.Kernel_Loadbox_Card_LDU_Firmware_FPGA_Minor = lduItems.ToArray()[i].Kernel_Loadbox_Card_LDU_Firmware_FPGA_Minor;
        //                                obj.Kernel_Loadbox_Card_LDU_Calibration_Day = lduItems.ToArray()[i].Kernel_Loadbox_Card_LDU_Calibration_Day;
        //                                obj.Kernel_Loadbox_Card_LDU_Calibration_Month = lduItems.ToArray()[i].Kernel_Loadbox_Card_LDU_Calibration_Month;
        //                                obj.Kernel_Loadbox_Card_LDU_Calibration_Year = lduItems.ToArray()[i].Kernel_Loadbox_Card_LDU_Calibration_Year;
        //                                obj.Kernel_Loadbox_Card_LDU_Next_Calibration_Day = lduItems.ToArray()[i].Kernel_Loadbox_Card_LDU_Next_Calibration_Day;
        //                                obj.Kernel_Loadbox_Card_LDU_Next_Calibration_Month = lduItems.ToArray()[i].Kernel_Loadbox_Card_LDU_Next_Calibration_Month;
        //                                obj.Kernel_Loadbox_Card_LDU_Next_Calibration_Year = lduItems.ToArray()[i].Kernel_Loadbox_Card_LDU_Next_Calibration_Year;
        //                                obj.Kernel_Loadbox_Card_LDU_Day_of_Creation = lduItems.ToArray()[i].Kernel_Loadbox_Card_LDU_Day_of_Creation;
        //                                obj.Kernel_Loadbox_Card_LDU_Month_of_Creation = lduItems.ToArray()[i].Kernel_Loadbox_Card_LDU_Month_of_Creation;
        //                                obj.Kernel_Loadbox_Card_LDU_Year_of_Creation = lduItems.ToArray()[i].Kernel_Loadbox_Card_LDU_Year_of_Creation;
        //                                obj.Kernel_Loadbox_Card_LDU_BuildVersion = lduItems.ToArray()[i].Kernel_Loadbox_Card_LDU_BuildVersion;

        //                                table8DataViews.Add(obj);
        //                            }
        //                        }
        //                    }
        //                }
        //                table8DataViews = table8DataViews.OrderByDescending(x => x.Date).GroupBy(x => new { /*x.Date,*/ x.System_name, x.Kernel_Loadbox_Card_LDU_Serial_No, x.Kernel_Loadbox_Card_LDU_Firmware_Card_Minor }).Select(x => x.FirstOrDefault()).OrderBy(x => x.Date).ToList();
        //                return Json(new { data = table8DataViews }, JsonRequestBehavior.AllowGet);
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

        //[HttpPost]
        //public ActionResult GetTABLE20_PSCData_unique(List<string> pc, DateTime sdate, DateTime edate)
        //{
        //    List<TABLE9_PSCView> table9DataViews = new List<TABLE9_PSCView>();
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
        //                    foreach (var pcname in pc)
        //                    {
        //                        //var pcname = pcname1.FQDN.Split('.')[0].ToUpper().Trim();
        //                        if (pcname != null && pcname != string.Empty && pcname.Count() > 0)
        //                        {

        //                            var pscItems = (from item in db.PSC_Card_Pr where item.Date.Contains(date) && item.System_name.Contains(pcname) select new { item.Date, item.Kernel_Loadbox_Card_PSC_BuildVersion, item.Kernel_Loadbox_Card_PSC_Calibration_Day, item.Kernel_Loadbox_Card_PSC_Calibration_Month, item.Kernel_Loadbox_Card_PSC_Calibration_Year, item.Kernel_Loadbox_Card_PSC_CARD_BoardRevision, item.Kernel_Loadbox_Card_PSC_CARD_BoardType, item.Kernel_Loadbox_Card_PSC_CARD_Description, item.Kernel_Loadbox_Card_PSC_CARD_VER_TAG_ARRAY, item.Kernel_Loadbox_Card_PSC_Day_of_Creation, item.Kernel_Loadbox_Card_PSC_Firmware_Card_Major, item.Kernel_Loadbox_Card_PSC_Firmware_Card_Minor, item.Kernel_Loadbox_Card_PSC_Firmware_FPGA_Major, item.Kernel_Loadbox_Card_PSC_Firmware_FPGA_Minor, item.Kernel_Loadbox_Card_PSC_Month_of_Creation, item.Kernel_Loadbox_Card_PSC_Next_Calibration_Day, item.Kernel_Loadbox_Card_PSC_Next_Calibration_Month, item.Kernel_Loadbox_Card_PSC_Next_Calibration_Year, item.Kernel_Loadbox_Card_PSC_PowerOnTime, item.Kernel_Loadbox_Card_PSC_RepairCounter, item.Kernel_Loadbox_Card_PSC_Serial_No, item.Kernel_Loadbox_Card_PSC_Year_of_Creation, item.System_name }).GroupBy(x=> new { x.Kernel_Loadbox_Card_PSC_Serial_No, x.Kernel_Loadbox_Card_PSC_Firmware_Card_Minor }).Select(x=>x.FirstOrDefault());
        //                            // var hwItems = (from item in db.Hardware_Desc_Pr where item.Date.Contains(date) && item.System_name.Contains(pcname) select new { item.EEPName, item.Date, item.EEPBuildDate, item.EEPDatabaseVersion, item.LabCarType, item.PCHostName, item.System_name, item.RBCCAFVersion, item.RTPCName, item.RTPCSoftwareVersion, item.ToolChainVersion }).GroupBy(x => x.EEPName).Select(x => x.FirstOrDefault()).ToList(); //since single date descorderbydate not needed
        //                            //hwDataViews = hwDataViews.OrderByDescending(x => x.Date).GroupBy(x => x.EEPName).Select(x => x.FirstOrDefault()).OrderBy(x=>x.Date).ToList();


        //                            for (int i = 0; i < pscItems.Count(); i++)
        //                            {
        //                                TABLE9_PSCView obj = new TABLE9_PSCView();
        //                                obj.Date = DateTime.Parse(pscItems.ToArray()[i].Date).ToShortDateString();
        //                                obj.System_name = pscItems.ToArray()[i].System_name;

        //                                using (BookingServerReplicaEntities db1 = new BookingServerReplicaEntities())
        //                                {

        //                                    var labid = db1.LabComputersPrs.ToList().Find(x => x.FQDN.Split('.')[0].ToUpper().Contains(obj.System_name)).LabId;//Find(x => x.DisplayName.ToString() == obj.System_name.ToString()).LabId;
        //                                    LabInfo labInfo = new LabInfo();
        //                                    //labInfo = db.LabInfoes.Where(item => item.Id == labid).FirstOrDefault();
        //                                    //if (labInfo != null)
        //                                    // {
        //                                    obj.LC_Name = db1.LabInfoes.FirstOrDefault(j => j.Id == labid).DisplayName;
        //                                }

        //                                obj.Kernel_Loadbox_Card_PSC_CARD_BoardType = pscItems.ToArray()[i].Kernel_Loadbox_Card_PSC_CARD_BoardType;
        //                                obj.Kernel_Loadbox_Card_PSC_CARD_BoardRevision = pscItems.ToArray()[i].Kernel_Loadbox_Card_PSC_CARD_BoardRevision;
        //                                obj.Kernel_Loadbox_Card_PSC_CARD_VER_TAG_ARRAY = pscItems.ToArray()[i].Kernel_Loadbox_Card_PSC_CARD_VER_TAG_ARRAY;
        //                                obj.Kernel_Loadbox_Card_PSC_Serial_No = pscItems.ToArray()[i].Kernel_Loadbox_Card_PSC_Serial_No;
        //                                obj.Kernel_Loadbox_Card_PSC_Firmware_Card_Major = pscItems.ToArray()[i].Kernel_Loadbox_Card_PSC_Firmware_Card_Major;
        //                                obj.Kernel_Loadbox_Card_PSC_Firmware_Card_Minor = pscItems.ToArray()[i].Kernel_Loadbox_Card_PSC_Firmware_Card_Minor;
        //                                obj.Kernel_Loadbox_Card_PSC_Firmware_FPGA_Major = pscItems.ToArray()[i].Kernel_Loadbox_Card_PSC_Firmware_FPGA_Major;
        //                                obj.Kernel_Loadbox_Card_PSC_Firmware_FPGA_Minor = pscItems.ToArray()[i].Kernel_Loadbox_Card_PSC_Firmware_FPGA_Minor;
        //                                obj.Kernel_Loadbox_Card_PSC_Calibration_Day = pscItems.ToArray()[i].Kernel_Loadbox_Card_PSC_Calibration_Day;
        //                                obj.Kernel_Loadbox_Card_PSC_Calibration_Month = pscItems.ToArray()[i].Kernel_Loadbox_Card_PSC_Calibration_Month;
        //                                obj.Kernel_Loadbox_Card_PSC_Calibration_Year = pscItems.ToArray()[i].Kernel_Loadbox_Card_PSC_Calibration_Year;
        //                                obj.Kernel_Loadbox_Card_PSC_Next_Calibration_Day = pscItems.ToArray()[i].Kernel_Loadbox_Card_PSC_Next_Calibration_Day;
        //                                obj.Kernel_Loadbox_Card_PSC_Next_Calibration_Month = pscItems.ToArray()[i].Kernel_Loadbox_Card_PSC_Next_Calibration_Month;
        //                                obj.Kernel_Loadbox_Card_PSC_Next_Calibration_Year = pscItems.ToArray()[i].Kernel_Loadbox_Card_PSC_Next_Calibration_Year;
        //                                obj.Kernel_Loadbox_Card_PSC_Day_of_Creation = pscItems.ToArray()[i].Kernel_Loadbox_Card_PSC_Day_of_Creation;
        //                                obj.Kernel_Loadbox_Card_PSC_Month_of_Creation = pscItems.ToArray()[i].Kernel_Loadbox_Card_PSC_Month_of_Creation;
        //                                obj.Kernel_Loadbox_Card_PSC_Year_of_Creation = pscItems.ToArray()[i].Kernel_Loadbox_Card_PSC_Year_of_Creation;
        //                                obj.Kernel_Loadbox_Card_PSC_BuildVersion = pscItems.ToArray()[i].Kernel_Loadbox_Card_PSC_BuildVersion;

        //                                table9DataViews.Add(obj);
        //                            }
        //                        }
        //                    }
        //                }
        //                table9DataViews = table9DataViews.OrderByDescending(x => x.Date).GroupBy(x => new { /*x.Date,*/ x.System_name, x.Kernel_Loadbox_Card_PSC_Serial_No, x.Kernel_Loadbox_Card_PSC_Firmware_Card_Minor }).Select(x => x.FirstOrDefault()).OrderBy(x => x.Date).ToList();
        //                return Json(new { data = table9DataViews }, JsonRequestBehavior.AllowGet);
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

        //[HttpPost]
        //public ActionResult GetTABLE21_VSCData_unique(List<string> pc, DateTime sdate, DateTime edate)
        //{
        //    List<TABLE10_VSCView> table10DataViews = new List<TABLE10_VSCView>();
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
        //                    foreach (var pcname in pc)
        //                    {
        //                        //var pcname = pcname1.FQDN.Split('.')[0].ToUpper().Trim();
        //                        if (pcname != null && pcname != string.Empty && pcname.Count() > 0)
        //                        {

        //                            var vscItems = (from item in db.VSC_Card_Pr where item.Date.Contains(date) && item.System_name.Contains(pcname) select new { item.Date, item.Kernel_Loadbox_Card_VSC_BuildVersion, item.Kernel_Loadbox_Card_VSC_Calibration_Day, item.Kernel_Loadbox_Card_VSC_Calibration_Month, item.Kernel_Loadbox_Card_VSC_Calibration_Year, item.Kernel_Loadbox_Card_VSC_CARD_BoardRevision, item.Kernel_Loadbox_Card_VSC_CARD_BoardType, item.Kernel_Loadbox_Card_VSC_CARD_Description, item.Kernel_Loadbox_Card_VSC_CARD_VER_TAG_ARRAY, item.Kernel_Loadbox_Card_VSC_Day_of_Creation, item.Kernel_Loadbox_Card_VSC_Firmware_Card_Major, item.Kernel_Loadbox_Card_VSC_Firmware_Card_Minor, item.Kernel_Loadbox_Card_VSC_Firmware_FPGA_Major, item.Kernel_Loadbox_Card_VSC_Firmware_FPGA_Minor, item.Kernel_Loadbox_Card_VSC_Month_of_Creation, item.Kernel_Loadbox_Card_VSC_Next_Calibration_Day, item.Kernel_Loadbox_Card_VSC_Next_Calibration_Month, item.Kernel_Loadbox_Card_VSC_Next_Calibration_Year, item.Kernel_Loadbox_Card_VSC_PowerOnTime, item.Kernel_Loadbox_Card_VSC_RepairCounter, item.Kernel_Loadbox_Card_VSC_Serial_No, item.Kernel_Loadbox_Card_VSC_Year_of_Creation, item.System_name }).GroupBy(x=>new { x.Kernel_Loadbox_Card_VSC_Serial_No, x.Kernel_Loadbox_Card_VSC_Firmware_Card_Minor }).Select(x=>x.FirstOrDefault());
        //                            // var hwItems = (from item in db.Hardware_Desc_Pr where item.Date.Contains(date) && item.System_name.Contains(pcname) select new { item.EEPName, item.Date, item.EEPBuildDate, item.EEPDatabaseVersion, item.LabCarType, item.PCHostName, item.System_name, item.RBCCAFVersion, item.RTPCName, item.RTPCSoftwareVersion, item.ToolChainVersion }).GroupBy(x => x.EEPName).Select(x => x.FirstOrDefault()).ToList(); //since single date descorderbydate not needed
        //                            //hwDataViews = hwDataViews.OrderByDescending(x => x.Date).GroupBy(x => x.EEPName).Select(x => x.FirstOrDefault()).OrderBy(x=>x.Date).ToList();

        //                            for (int i = 0; i < vscItems.Count(); i++)
        //                            {
        //                                TABLE10_VSCView obj = new TABLE10_VSCView();
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


        //                                obj.Kernel_Loadbox_Card_VSC_CARD_BoardType = vscItems.ToArray()[i].Kernel_Loadbox_Card_VSC_CARD_BoardType;
        //                                obj.Kernel_Loadbox_Card_VSC_CARD_BoardRevision = vscItems.ToArray()[i].Kernel_Loadbox_Card_VSC_CARD_BoardRevision;
        //                                obj.Kernel_Loadbox_Card_VSC_CARD_VER_TAG_ARRAY = vscItems.ToArray()[i].Kernel_Loadbox_Card_VSC_CARD_VER_TAG_ARRAY;
        //                                obj.Kernel_Loadbox_Card_VSC_Serial_No = vscItems.ToArray()[i].Kernel_Loadbox_Card_VSC_Serial_No;
        //                                obj.Kernel_Loadbox_Card_VSC_Firmware_Card_Major = vscItems.ToArray()[i].Kernel_Loadbox_Card_VSC_Firmware_Card_Major;
        //                                obj.Kernel_Loadbox_Card_VSC_Firmware_Card_Minor = vscItems.ToArray()[i].Kernel_Loadbox_Card_VSC_Firmware_Card_Minor;
        //                                obj.Kernel_Loadbox_Card_VSC_Firmware_FPGA_Major = vscItems.ToArray()[i].Kernel_Loadbox_Card_VSC_Firmware_FPGA_Major;
        //                                obj.Kernel_Loadbox_Card_VSC_Firmware_FPGA_Minor = vscItems.ToArray()[i].Kernel_Loadbox_Card_VSC_Firmware_FPGA_Minor;
        //                                obj.Kernel_Loadbox_Card_VSC_Calibration_Day = vscItems.ToArray()[i].Kernel_Loadbox_Card_VSC_Calibration_Day;
        //                                obj.Kernel_Loadbox_Card_VSC_Calibration_Month = vscItems.ToArray()[i].Kernel_Loadbox_Card_VSC_Calibration_Month;
        //                                obj.Kernel_Loadbox_Card_VSC_Calibration_Year = vscItems.ToArray()[i].Kernel_Loadbox_Card_VSC_Calibration_Year;
        //                                obj.Kernel_Loadbox_Card_VSC_Next_Calibration_Day = vscItems.ToArray()[i].Kernel_Loadbox_Card_VSC_Next_Calibration_Day;
        //                                obj.Kernel_Loadbox_Card_VSC_Next_Calibration_Month = vscItems.ToArray()[i].Kernel_Loadbox_Card_VSC_Next_Calibration_Month;
        //                                obj.Kernel_Loadbox_Card_VSC_Next_Calibration_Year = vscItems.ToArray()[i].Kernel_Loadbox_Card_VSC_Next_Calibration_Year;
        //                                obj.Kernel_Loadbox_Card_VSC_Day_of_Creation = vscItems.ToArray()[i].Kernel_Loadbox_Card_VSC_Day_of_Creation;
        //                                obj.Kernel_Loadbox_Card_VSC_Month_of_Creation = vscItems.ToArray()[i].Kernel_Loadbox_Card_VSC_Month_of_Creation;
        //                                obj.Kernel_Loadbox_Card_VSC_Year_of_Creation = vscItems.ToArray()[i].Kernel_Loadbox_Card_VSC_Year_of_Creation;
        //                                obj.Kernel_Loadbox_Card_VSC_BuildVersion = vscItems.ToArray()[i].Kernel_Loadbox_Card_VSC_BuildVersion;
        //                                table10DataViews.Add(obj);
        //                            }
        //                        }
        //                    }
        //                }
        //                table10DataViews = table10DataViews.OrderByDescending(x => x.Date).GroupBy(x => new { /*x.Date,*/ x.System_name, x.Kernel_Loadbox_Card_VSC_Serial_No, x.Kernel_Loadbox_Card_VSC_Firmware_Card_Minor }).Select(x => x.FirstOrDefault()).OrderBy(x => x.Date).ToList();
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

        //[HttpPost]
        //public ActionResult GetTABLE22_WSSData_unique(List<string> pc, DateTime sdate, DateTime edate)
        //{
        //    List<TABLE11_WSSView> table11DataViews = new List<TABLE11_WSSView>();
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
        //                    foreach (var pcname in pc)
        //                    {
        //                        //var pcname = pcname1.FQDN.Split('.')[0].ToUpper().Trim();
        //                        if (pcname != null && pcname != string.Empty && pcname.Count() > 0)
        //                        {

        //                            var wssItems = (from item in db.WSS_Card_Pr where item.Date.Contains(date) && item.System_name.Contains(pcname)  select new { item.Date, item.Kernel_Loadbox_Card_WSS_BuildVersion, item.Kernel_Loadbox_Card_WSS_Calibration_Day, item.Kernel_Loadbox_Card_WSS_Calibration_Month, item.Kernel_Loadbox_Card_WSS_Calibration_Year, item.Kernel_Loadbox_Card_WSS_CARD_BoardRevision, item.Kernel_Loadbox_Card_WSS_CARD_BoardType, item.Kernel_Loadbox_Card_WSS_CARD_Description, item.Kernel_Loadbox_Card_WSS_CARD_VER_TAG_ARRAY, item.Kernel_Loadbox_Card_WSS_Day_of_Creation, item.Kernel_Loadbox_Card_WSS_Firmware_Card_Major, item.Kernel_Loadbox_Card_WSS_Firmware_Card_Minor, item.Kernel_Loadbox_Card_WSS_Firmware_FPGA_Major, item.Kernel_Loadbox_Card_WSS_Firmware_FPGA_Minor, item.Kernel_Loadbox_Card_WSS_Month_of_Creation, item.Kernel_Loadbox_Card_WSS_Next_Calibration_Day, item.Kernel_Loadbox_Card_WSS_Next_Calibration_Month, item.Kernel_Loadbox_Card_WSS_Next_Calibration_Year, item.Kernel_Loadbox_Card_WSS_PowerOnTime, item.Kernel_Loadbox_Card_WSS_RepairCounter, item.Kernel_Loadbox_Card_WSS_Serial_No, item.Kernel_Loadbox_Card_WSS_Year_of_Creation, item.System_name }).GroupBy(x=> new{ x.Kernel_Loadbox_Card_WSS_Serial_No, x.Kernel_Loadbox_Card_WSS_Firmware_Card_Minor}).Select(x=>x.FirstOrDefault());
        //                            // var hwItems = (from item in db.Hardware_Desc_Pr where item.Date.Contains(date) && item.System_name.Contains(pcname) select new { item.EEPName, item.Date, item.EEPBuildDate, item.EEPDatabaseVersion, item.LabCarType, item.PCHostName, item.System_name, item.RBCCAFVersion, item.RTPCName, item.RTPCSoftwareVersion, item.ToolChainVersion }).GroupBy(x => x.EEPName).Select(x => x.FirstOrDefault()).ToList(); //since single date descorderbydate not needed
        //                            //hwDataViews = hwDataViews.OrderByDescending(x => x.Date).GroupBy(x => x.EEPName).Select(x => x.FirstOrDefault()).OrderBy(x=>x.Date).ToList();

        //                            for (int i = 0; i < wssItems.Count(); i++)
        //                            {
        //                                TABLE11_WSSView obj = new TABLE11_WSSView();
        //                                obj.Date = DateTime.Parse(wssItems.ToArray()[i].Date).ToShortDateString();
        //                                obj.System_name = wssItems.ToArray()[i].System_name;

        //                                using (BookingServerReplicaEntities db1 = new BookingServerReplicaEntities())
        //                                {

        //                                    var labid = db1.LabComputersPrs.ToList().Find(x => x.FQDN.Split('.')[0].ToUpper().Contains(obj.System_name)).LabId;//Find(x => x.DisplayName.ToString() == obj.System_name.ToString()).LabId;
        //                                    LabInfo labInfo = new LabInfo();
        //                                    //labInfo = db.LabInfoes.Where(item => item.Id == labid).FirstOrDefault();
        //                                    //if (labInfo != null)
        //                                    // {
        //                                    obj.LC_Name = db1.LabInfoes.FirstOrDefault(j => j.Id == labid).DisplayName;
        //                                }


        //                                obj.Kernel_Loadbox_Card_WSS_CARD_BoardType = wssItems.ToArray()[i].Kernel_Loadbox_Card_WSS_CARD_BoardType;
        //                                obj.Kernel_Loadbox_Card_WSS_CARD_BoardRevision = wssItems.ToArray()[i].Kernel_Loadbox_Card_WSS_CARD_BoardRevision;
        //                                obj.Kernel_Loadbox_Card_WSS_CARD_VER_TAG_ARRAY = wssItems.ToArray()[i].Kernel_Loadbox_Card_WSS_CARD_VER_TAG_ARRAY;
        //                                obj.Kernel_Loadbox_Card_WSS_Serial_No = wssItems.ToArray()[i].Kernel_Loadbox_Card_WSS_Serial_No;
        //                                obj.Kernel_Loadbox_Card_WSS_Firmware_Card_Major = wssItems.ToArray()[i].Kernel_Loadbox_Card_WSS_Firmware_Card_Major;
        //                                obj.Kernel_Loadbox_Card_WSS_Firmware_Card_Minor = wssItems.ToArray()[i].Kernel_Loadbox_Card_WSS_Firmware_Card_Minor;
        //                                obj.Kernel_Loadbox_Card_WSS_Firmware_FPGA_Major = wssItems.ToArray()[i].Kernel_Loadbox_Card_WSS_Firmware_FPGA_Major;
        //                                obj.Kernel_Loadbox_Card_WSS_Firmware_FPGA_Minor = wssItems.ToArray()[i].Kernel_Loadbox_Card_WSS_Firmware_FPGA_Minor;
        //                                obj.Kernel_Loadbox_Card_WSS_Calibration_Day = wssItems.ToArray()[i].Kernel_Loadbox_Card_WSS_Calibration_Day;
        //                                obj.Kernel_Loadbox_Card_WSS_Calibration_Month = wssItems.ToArray()[i].Kernel_Loadbox_Card_WSS_Calibration_Month;
        //                                obj.Kernel_Loadbox_Card_WSS_Calibration_Year = wssItems.ToArray()[i].Kernel_Loadbox_Card_WSS_Calibration_Year;
        //                                obj.Kernel_Loadbox_Card_WSS_Next_Calibration_Day = wssItems.ToArray()[i].Kernel_Loadbox_Card_WSS_Next_Calibration_Day;
        //                                obj.Kernel_Loadbox_Card_WSS_Next_Calibration_Month = wssItems.ToArray()[i].Kernel_Loadbox_Card_WSS_Next_Calibration_Month;
        //                                obj.Kernel_Loadbox_Card_WSS_Next_Calibration_Year = wssItems.ToArray()[i].Kernel_Loadbox_Card_WSS_Next_Calibration_Year;
        //                                obj.Kernel_Loadbox_Card_WSS_Day_of_Creation = wssItems.ToArray()[i].Kernel_Loadbox_Card_WSS_Day_of_Creation;
        //                                obj.Kernel_Loadbox_Card_WSS_Month_of_Creation = wssItems.ToArray()[i].Kernel_Loadbox_Card_WSS_Month_of_Creation;
        //                                obj.Kernel_Loadbox_Card_WSS_Year_of_Creation = wssItems.ToArray()[i].Kernel_Loadbox_Card_WSS_Year_of_Creation;
        //                                obj.Kernel_Loadbox_Card_WSS_BuildVersion = wssItems.ToArray()[i].Kernel_Loadbox_Card_WSS_BuildVersion;
        //                                table11DataViews.Add(obj);
        //                            }
        //                        }
        //                    }
        //                }
        //                table11DataViews = table11DataViews.OrderByDescending(x => x.Date).GroupBy(x => new { /*x.Date,*/ x.System_name, x.Kernel_Loadbox_Card_WSS_Serial_No, x.Kernel_Loadbox_Card_WSS_Firmware_Card_Minor }).Select(x => x.FirstOrDefault()).OrderBy(x => x.Date).ToList();
        //                return Json(new { data = table11DataViews }, JsonRequestBehavior.AllowGet);
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

        //[HttpPost]
        //public ActionResult GetTABLE23_WSS2Data_unique(List<string> pc, DateTime sdate, DateTime edate)
        //{
        //    List<TABLE12_WSS2View> table12DataViews = new List<TABLE12_WSS2View>();
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
        //                    foreach (var pcname in pc)
        //                    {
        //                        //var pcname = pcname1.FQDN.Split('.')[0].ToUpper().Trim();
        //                        if (pcname != null && pcname != string.Empty && pcname.Count() > 0)
        //                        {

        //                            var wss2Items = (from item in db.WSS2_Card_Pr where item.Date.Contains(date) && item.System_name.Contains(pcname) select new { item.Date, item.Kernel_Loadbox_Card_WSS2_BuildVersion, item.Kernel_Loadbox_Card_WSS2_Calibration_Day, item.Kernel_Loadbox_Card_WSS2_Calibration_Month, item.Kernel_Loadbox_Card_WSS2_Calibration_Year, item.Kernel_Loadbox_Card_WSS2_CARD_BoardRevision, item.Kernel_Loadbox_Card_WSS2_CARD_BoardType, item.Kernel_Loadbox_Card_WSS2_CARD_Description, item.Kernel_Loadbox_Card_WSS2_CARD_VER_TAG_ARRAY, item.Kernel_Loadbox_Card_WSS2_Day_of_Creation, item.Kernel_Loadbox_Card_WSS2_Firmware_Card_Major, item.Kernel_Loadbox_Card_WSS2_Firmware_Card_Minor, item.Kernel_Loadbox_Card_WSS2_Firmware_FPGA_Major, item.Kernel_Loadbox_Card_WSS2_Firmware_FPGA_Minor, item.Kernel_Loadbox_Card_WSS2_Month_of_Creation, item.Kernel_Loadbox_Card_WSS2_Next_Calibration_Day, item.Kernel_Loadbox_Card_WSS2_Next_Calibration_Month, item.Kernel_Loadbox_Card_WSS2_Next_Calibration_Year, item.Kernel_Loadbox_Card_WSS2_PowerOnTime, item.Kernel_Loadbox_Card_WSS2_RepairCounter, item.Kernel_Loadbox_Card_WSS2_Serial_No, item.Kernel_Loadbox_Card_WSS2_Year_of_Creation, item.System_name }).GroupBy(x=>new { x.Kernel_Loadbox_Card_WSS2_Serial_No, x.Kernel_Loadbox_Card_WSS2_Firmware_Card_Minor }).Select(x=>x.FirstOrDefault());
        //                            // var hwItems = (from item in db.Hardware_Desc_Pr where item.Date.Contains(date) && item.System_name.Contains(pcname) select new { item.EEPName, item.Date, item.EEPBuildDate, item.EEPDatabaseVersion, item.LabCarType, item.PCHostName, item.System_name, item.RBCCAFVersion, item.RTPCName, item.RTPCSoftwareVersion, item.ToolChainVersion }).GroupBy(x => x.EEPName).Select(x => x.FirstOrDefault()).ToList(); //since single date descorderbydate not needed
        //                            //hwDataViews = hwDataViews.OrderByDescending(x => x.Date).GroupBy(x => x.EEPName).Select(x => x.FirstOrDefault()).OrderBy(x=>x.Date).ToList();


        //                            for (int i = 0; i < wss2Items.Count(); i++)
        //                            {
        //                                TABLE12_WSS2View obj = new TABLE12_WSS2View();
        //                                obj.Date = DateTime.Parse(wss2Items.ToArray()[i].Date).ToShortDateString();
        //                                obj.System_name = wss2Items.ToArray()[i].System_name;

        //                                using (BookingServerReplicaEntities db1 = new BookingServerReplicaEntities())
        //                                {

        //                                    var labid = db1.LabComputersPrs.ToList().Find(x => x.FQDN.Split('.')[0].ToUpper().Contains(obj.System_name)).LabId;//Find(x => x.DisplayName.ToString() == obj.System_name.ToString()).LabId;
        //                                    LabInfo labInfo = new LabInfo();
        //                                    //labInfo = db.LabInfoes.Where(item => item.Id == labid).FirstOrDefault();
        //                                    //if (labInfo != null)
        //                                    // {
        //                                    obj.LC_Name = db1.LabInfoes.FirstOrDefault(j => j.Id == labid).DisplayName;
        //                                }


        //                                obj.Kernel_Loadbox_Card_WSS2_CARD_BoardType = wss2Items.ToArray()[i].Kernel_Loadbox_Card_WSS2_CARD_BoardType;
        //                                obj.Kernel_Loadbox_Card_WSS2_CARD_BoardRevision = wss2Items.ToArray()[i].Kernel_Loadbox_Card_WSS2_CARD_BoardRevision;
        //                                obj.Kernel_Loadbox_Card_WSS2_CARD_VER_TAG_ARRAY = wss2Items.ToArray()[i].Kernel_Loadbox_Card_WSS2_CARD_VER_TAG_ARRAY;
        //                                obj.Kernel_Loadbox_Card_WSS2_Serial_No = wss2Items.ToArray()[i].Kernel_Loadbox_Card_WSS2_Serial_No;
        //                                obj.Kernel_Loadbox_Card_WSS2_Firmware_Card_Major = wss2Items.ToArray()[i].Kernel_Loadbox_Card_WSS2_Firmware_Card_Major;
        //                                obj.Kernel_Loadbox_Card_WSS2_Firmware_Card_Minor = wss2Items.ToArray()[i].Kernel_Loadbox_Card_WSS2_Firmware_Card_Minor;
        //                                obj.Kernel_Loadbox_Card_WSS2_Firmware_FPGA_Major = wss2Items.ToArray()[i].Kernel_Loadbox_Card_WSS2_Firmware_FPGA_Major;
        //                                obj.Kernel_Loadbox_Card_WSS2_Firmware_FPGA_Minor = wss2Items.ToArray()[i].Kernel_Loadbox_Card_WSS2_Firmware_FPGA_Minor;
        //                                obj.Kernel_Loadbox_Card_WSS2_Calibration_Day = wss2Items.ToArray()[i].Kernel_Loadbox_Card_WSS2_Calibration_Day;
        //                                obj.Kernel_Loadbox_Card_WSS2_Calibration_Month = wss2Items.ToArray()[i].Kernel_Loadbox_Card_WSS2_Calibration_Month;
        //                                obj.Kernel_Loadbox_Card_WSS2_Calibration_Year = wss2Items.ToArray()[i].Kernel_Loadbox_Card_WSS2_Calibration_Year;
        //                                obj.Kernel_Loadbox_Card_WSS2_Next_Calibration_Day = wss2Items.ToArray()[i].Kernel_Loadbox_Card_WSS2_Next_Calibration_Day;
        //                                obj.Kernel_Loadbox_Card_WSS2_Next_Calibration_Month = wss2Items.ToArray()[i].Kernel_Loadbox_Card_WSS2_Next_Calibration_Month;
        //                                obj.Kernel_Loadbox_Card_WSS2_Next_Calibration_Year = wss2Items.ToArray()[i].Kernel_Loadbox_Card_WSS2_Next_Calibration_Year;
        //                                obj.Kernel_Loadbox_Card_WSS2_Day_of_Creation = wss2Items.ToArray()[i].Kernel_Loadbox_Card_WSS2_Day_of_Creation;
        //                                obj.Kernel_Loadbox_Card_WSS2_Month_of_Creation = wss2Items.ToArray()[i].Kernel_Loadbox_Card_WSS2_Month_of_Creation;
        //                                obj.Kernel_Loadbox_Card_WSS2_Year_of_Creation = wss2Items.ToArray()[i].Kernel_Loadbox_Card_WSS2_Year_of_Creation;
        //                                obj.Kernel_Loadbox_Card_WSS2_BuildVersion = wss2Items.ToArray()[i].Kernel_Loadbox_Card_WSS2_BuildVersion;
        //                                table12DataViews.Add(obj);
        //                            }
        //                        }
        //                    }
        //                }
        //                table12DataViews = table12DataViews.OrderByDescending(x => x.Date).GroupBy(x => new{/*x.Date,*/ x.System_name, x.Kernel_Loadbox_Card_WSS2_Serial_No, x.Kernel_Loadbox_Card_WSS2_Firmware_Card_Minor}).Select(x => x.FirstOrDefault()).OrderBy(x => x.Date).ToList();
        //                return Json(new { data = table12DataViews }, JsonRequestBehavior.AllowGet);
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



        //***********************  UNIQUE - ADO .NET  **************************

        //**********************ADO .NET ***********************************************


        [HttpPost]
        public ActionResult GetTABLE1_HWDescData_unique(List<string> pc, DateTime sdate, DateTime edate, int diff_UTCandLocal)
        {
            List<TABLE1_HwDataView> hwDataViews = new List<TABLE1_HwDataView>();

            if (pc != null && pc.Count() > 0)
            {
                string pclist = string.Empty;
                try
                {
                    using (PromasterImport_HWInfoEntities db = new PromasterImport_HWInfoEntities())
                    {
                        db.Database.CommandTimeout = 500;
                        string date = string.Empty;
                        foreach (var p in pc)
                        {
                            pclist += ("'" + p.Trim() + "',");
                            //pclist.Add(p.FQDN.Split('.')[0].ToUpper().Trim());
                        }

                        pclist = pclist.Remove(pclist.Length - 1, 1);
                        //connection();
                        string constring = ConfigurationManager.ConnectionStrings["PromasterImport_HWInfoEntities_ado"].ToString();
                        con = new SqlConnection(constring);
                        DataTable dtable = new DataTable();

                        string Query = " Select " + ConfigurationManager.AppSettings["BookingServerReplica"] + " [LabInfo].DisplayName, [System_name],Date = max(Date),[EEPName] ,[EEPBuildDate] ,[EEPDatabaseVersion] ,[PCHostName] ,[RTPCSoftwareVersion] ,[RTPCName],[RBCCAFVersion],[LabCarType],[ToolChainVersion],CreationDate = max([CreationDate]) from [Hardware_Desc_Pr] inner join  " + ConfigurationManager.AppSettings["BookingServerReplica"] + "LabComputersPr on substring( " + ConfigurationManager.AppSettings["BookingServerReplica"] + "LabComputersPr.FQDN, 1, len(System_name)) = [Hardware_Desc_Pr].System_name inner join " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo] on  " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo].id = LabComputersPr.LabId where System_name in (" + pclist + ") AND CONVERT(DATETIME, Date) > = '" + sdate + "' AND CONVERT(DATETIME, Date) < = '" + edate + "' GROUP BY   " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo].DisplayName , [System_name],[EEPName] ,[EEPBuildDate] ,[EEPDatabaseVersion],[PCHostName],[RTPCSoftwareVersion] ,[RTPCName],[RBCCAFVersion],[LabCarType],[ToolChainVersion] order by CreationDate";


                        OpenConnection();

                        SqlCommand cmd = new SqlCommand(Query, con);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dtable);

                        CloseConnection();

                        foreach (DataRow row in dtable.Rows)
                        {
                            TABLE1_HwDataView obj = new TABLE1_HwDataView();
                            obj.System_name = row["System_name"].ToString();
                            obj.LC_Name = row["DisplayName"].ToString();

                            obj.PCHostName = row["PCHostName"].ToString();
                            obj.Date = /*DateTime.Parse(item.Date).ToShortDateString();*/ row["Date"].ToString();
                            obj.CreationDate = /*DateTime.Parse(item.Date).ToShortDateString();*/ DateTime.Parse(row["CreationDate"].ToString()).AddMinutes(diff_UTCandLocal).ToLongTimeString();

                            obj.EEPName = /*item.EEPName;*/ row["EEPName"].ToString();
                            obj.EEPBuildDate =/* DateTime.Parse(item.EEPBuildDate).ToShortDateString();*/ row["EEPBuildDate"].ToString();
                            obj.EEPDatabaseVersion =/* item.EEPDatabaseVersion;*/ row["EEPDatabaseVersion"].ToString();
                            obj.RTPCName = /*item.RTPCName;*/ row["RTPCName"].ToString();
                            obj.RBCCAFVersion = /*item.RBCCAFVersion;*/ row["RBCCAFVersion"].ToString();
                            obj.ToolChainVersion = /*item.ToolChainVersion;*/ row["ToolChainVersion"].ToString();
                            obj.RTPCSoftwareVersion = /*item.RTPCSoftwareVersion;*/ row["RTPCSoftwareVersion"].ToString();
                            obj.LabCarType = /*item.LabCarType;*/ row["LabCarType"].ToString();
                            hwDataViews.Add(obj);
                        }
                        //hwDataViews = hwDataViews.OrderByDescending(x => x.Date).GroupBy(x => new { x.EEPName, x.Date, x.System_name }).Select(x => x.FirstOrDefault()).OrderBy(x => x.Date).ToList();


                        var result = Json(new { data = hwDataViews }, JsonRequestBehavior.AllowGet); result.MaxJsonLength = int.MaxValue; return result;
                    }
                }
                catch (Exception Ex)
                {
                    return View("Unable to retrieve data from database. Please try again later!!");
                }
            }
            else
            {
                var result = Json(new { success = false, message = "Try again" }, JsonRequestBehavior.AllowGet); result.MaxJsonLength = int.MaxValue; return result;
            }
        }

        [HttpPost]
        public ActionResult TABLE2_GetPrjDescData_unique(List<string> pc, DateTime sdate, DateTime edate, int diff_UTCandLocal)
        {
            List<TABLE2_ProjectDescriptionView> PrjDataViews = new List<TABLE2_ProjectDescriptionView>();
            string pclist = string.Empty;
            if (pc != null && pc.Count() > 0)
            {
                try
                {
                    using (PromasterImport_HWInfoEntities db = new PromasterImport_HWInfoEntities())
                    {
                        db.Database.CommandTimeout = 500;
                        string date = string.Empty;
                        foreach (var p in pc)
                        {
                            pclist += ("'" + p.Trim() + "',");

                        }
                        pclist = pclist.Remove(pclist.Length - 1, 1);
                        //connection();
                        string constring = ConfigurationManager.ConnectionStrings["PromasterImport_ProjectDescEntities_ado"].ToString();
                        con = new SqlConnection(constring);
                        DataTable dtable = new DataTable();
                        //max([CreationDate]) to convert to local time in sql
                        string Query = " Select " + ConfigurationManager.AppSettings["BookingServerReplica"] + " [LabInfo].DisplayName,[LabOEMs].OEM,[System_name],[Db_Version] ," +
                            "Date = max(Date) ,[Details],[ToolVersion],[EMUCable],[AECUCable],[Product],[MetaEditor_Ver] ,[ProjectBuilder_Ver]," +
                            "[Ascet_Ver] ,[ProjectEditor_Ver],CreationDate = max([CreationDate])from ProjectDescription_Pr_1 inner join" +
                            "  " + ConfigurationManager.AppSettings["BookingServerReplica"] + "LabComputersPr on substring( " + ConfigurationManager.AppSettings["BookingServerReplica"] + "LabComputersPr.FQDN, 1, len(System_name)) = " +
                            "[ProjectDescription_Pr_1].System_name inner join " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo] on" +
                            " " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo].id = LabComputersPr.LabId inner join [BookingServerReplica].[dbo].[LabOEMs] on [BookingServerReplica].[dbo].[LabInfo].OEM =  [BookingServerReplica].[dbo].[LabOEMs].ID where System_name in (" + pclist + ")" +
                            " AND CONVERT(DATETIME, Date) > = '" + sdate + "' AND CONVERT(DATETIME, Date) < = '" + edate + "' GROUP BY" +
                            "   " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo].DisplayName ,[BookingServerReplica].[dbo].[LabOEMs].OEM,[System_name],[Db_Version] ,[Details],[ToolVersion]," +
                            "[EMUCable],[AECUCable],[Product],[MetaEditor_Ver] ,[ProjectBuilder_Ver],[Ascet_Ver] ,[ProjectEditor_Ver] order by CreationDate";


                        OpenConnection();

                        SqlCommand cmd = new SqlCommand(Query, con);
                        cmd.CommandTimeout = 0;
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dtable);


                        CloseConnection();

                        foreach (DataRow row in dtable.Rows)
                        {
                            TABLE2_ProjectDescriptionView obj = new TABLE2_ProjectDescriptionView();
                            obj.System_name = row["System_name"].ToString();
                            obj.LC_Name = row["DisplayName"].ToString();
                            obj.Date = row["Date"].ToString(); obj.CreationDate = DateTime.Parse(row["CreationDate"].ToString()).AddMinutes(diff_UTCandLocal).ToLongTimeString();


                            obj.Product = row["Product"].ToString();
                            obj.Db_Version = row["Db_Version"].ToString();
                            obj.Details = row["Details"].ToString();



                            obj.ToolVersion = row["ToolVersion"].ToString();
                            obj.EMUCable = row["EMUCable"].ToString();
                            obj.AECUCable = row["AECUCable"].ToString();
                            obj.MetaEditor_Ver = row["MetaEditor_Ver"].ToString();
                            obj.ProjectBuilder_Ver = row["ProjectBuilder_Ver"].ToString();
                            obj.Ascet_Ver = row["Ascet_Ver"].ToString();
                            obj.ProjectEditor_Ver = row["ProjectEditor_Ver"].ToString();
                            obj.System_name = row["System_name"].ToString();
                            obj.LabOEM = row["OEM"].ToString();
                            PrjDataViews.Add(obj);
                        }
                        //hwDataViews = hwDataViews.OrderByDescending(x => x.Date).GroupBy(x => new { x.EEPName, x.Date, x.System_name }).Select(x => x.FirstOrDefault()).OrderBy(x => x.Date).ToList();

                        var result = Json(new { data = PrjDataViews }, JsonRequestBehavior.AllowGet); result.MaxJsonLength = int.MaxValue; return result;
                    }
                    //using (PromasterImport_ProjectDescEntities db = new PromasterImport_ProjectDescEntities())
                    //{
                    //    db.Database.CommandTimeout = 500;
                    //    string date = string.Empty;
                    //    for (var dt = sdate; dt <= edate; dt = dt.AddDays(1))
                    //    {
                    //        date = dt.ToString("yyyy-MM-dd");

                    //        foreach (var pcname in pc)
                    //        {
                    //            //var pcname = pcname1.FQDN.Split('.')[0].ToUpper().Trim();
                    //            if (pcname != null && pcname != string.Empty && pcname.Count() > 0)
                    //            {

                    //                var PrjItems = (from item in db.ProjectDescription_Pr_1 where item.Date.Contains(date) && item.System_name.Contains(pcname) select new { item.AECUCable, item.Ascet_Ver, item.Component_name, item.Product, item.Date, item.Db_Version, item.Details, item.EMUCable, item.MetaEditor_Ver, item.ProjectBuilder_Ver, item.ProjectEditor_Ver, item.System_name, item.ToolVersion, item.Version }).ToList();
                    //                var prj_grpby = PrjItems.GroupBy(x => new { /*x.Component_name,*/ x.Details }).ToList();
                    //                var prj_grpby_selectfirst = prj_grpby.Select(x => x.FirstOrDefault()).ToList();
                    //                //db.Database.CommandTimeout = 10000000;
                    //                // var hwItems = (from item in db.Hardware_Desc_Pr where item.Date.Contains(date) && item.System_name.Contains(pcname) select new { item.EEPName, item.Date, item.EEPBuildDate, item.EEPDatabaseVersion, item.LabCarType, item.PCHostName, item.System_name, item.RBCCAFVersion, item.RTPCName, item.RTPCSoftwareVersion, item.ToolChainVersion }).GroupBy(x => x.EEPName).Select(x => x.FirstOrDefault()).ToList(); //since single date descorderbydate not needed
                    //                //hwDataViews = hwDataViews.OrderByDescending(x => x.Date).GroupBy(x => x.EEPName).Select(x => x.FirstOrDefault()).OrderBy(x=>x.Date).ToList();
                    //                //var PrjItems_takingUniqueComponent_Details = (from item in db.ProjectDescription_Pr_1 orderby item.Date where item.System_name.Contains(pcname) && item.Date.Contains(date) select new { item.Component_name, item.Details}).Distinct();
                    //                //var PrjItems_takingUniqueComponent = (from item in db.ProjectDescription_Pr_1 orderby item.Date where item.System_name.Contains(pcname) && item.Date.Contains(date) select new { item.Component_name}).Distinct().ToList();
                    //                //var PrjItems_takingUniqueDetail = (from item in db.ProjectDescription_Pr_1 orderby item.Date where item.System_name.Contains(pcname) && item.Date.Contains(date) select new { item.Details }).Distinct().ToList();



                    //                foreach (var item in prj_grpby_selectfirst)
                    //                {
                    //                    TABLE2_ProjectDescriptionView obj = new TABLE2_ProjectDescriptionView();

                    //                    //obj.Component_name = item.Component_name;
                    //                    obj.Date = DateTime.Parse(item.Date).ToShortDateString();
                    //                    obj.Db_Version = item.Db_Version;
                    //                    obj.Details = item.Details;
                    //                    obj.System_name = item.System_name;

                    //                    using (BookingServerReplicaEntities db1 = new BookingServerReplicaEntities())
                    //                    {

                    //                        var labid = db1.LabComputersPrs.ToList().Find(x => x.FQDN.Split('.')[0].ToUpper().Contains(obj.System_name)).LabId;
                    //                        obj.LC_Name = db1.LabInfoes.FirstOrDefault(i => i.Id == labid).DisplayName;
                    //                    }

                    //                    //obj.Version = item.Version;
                    //                    obj.Product = item.Product;
                    //                    obj.ToolVersion = item.ToolVersion;
                    //                    obj.EMUCable = item.EMUCable;
                    //                    obj.AECUCable = item.AECUCable;
                    //                    obj.MetaEditor_Ver = item.MetaEditor_Ver;
                    //                    obj.ProjectBuilder_Ver = item.ProjectBuilder_Ver;
                    //                    obj.Ascet_Ver = item.Ascet_Ver;
                    //                    obj.ProjectEditor_Ver = item.ProjectEditor_Ver;


                    //                    PrjDataViews.Add(obj);
                    //                }
                    //            }
                    //        }
                    //    }
                    //    var prjgrp = PrjDataViews.OrderByDescending(x => x.Date).GroupBy(x => new { x.Details, /*x.Component_name,*/ x.Date, x.System_name });
                    //    PrjDataViews = prjgrp.Select(x => x.FirstOrDefault()).OrderBy(x => x.Date).ToList();
                    //    var result = Json(new { data = PrjDataViews },  JsonRequestBehavior.AllowGet);result.MaxJsonLength = int.MaxValue;return result;
                    //}
                }
                catch (Exception Ex)
                {
                    return View("Unable to retrieve data from database. Please try again later!!");
                }
            }
            else
            {
                var result = Json(new { success = false, message = "Try again" }, JsonRequestBehavior.AllowGet); result.MaxJsonLength = int.MaxValue; return result;
            }
        }

        [HttpPost]
        public ActionResult TABLE2_GetCmpntData_unique(List<string> pc, DateTime sdate, DateTime edate, int diff_UTCandLocal, string components_type = "")
        {
            List<TABLE2_ProjectDescriptionView> PrjDataViews = new List<TABLE2_ProjectDescriptionView>();
            if (pc != null && pc.Count() > 0)
            {
                string pclist = string.Empty;
                try
                {
                    using (PromasterImport_HWInfoEntities db = new PromasterImport_HWInfoEntities())
                    {
                        db.Database.CommandTimeout = 500;
                        string date = string.Empty;
                        foreach (var p in pc)
                        {
                            pclist += ("'" + p.Trim() + "',");
                            //pclist.Add(p.FQDN.Split('.')[0].ToUpper().Trim());
                        }

                        pclist = pclist.Remove(pclist.Length - 1, 1);
                        //connection();
                        string constring = ConfigurationManager.ConnectionStrings["PromasterImport_ProjectDescEntities_ado"].ToString();
                        con = new SqlConnection(constring);

                        DataTable dtable = new DataTable();

                        string Query = "";
                        if (components_type.Contains("All"))
                        {
                            //Query = " Select " + ConfigurationManager.AppSettings["BookingServerReplica"] + " [LabInfo].DisplayName, [System_name],[Component_name],[Version] ,Date = max(Date) ,[Details], CreationDate = max([CreationDate]) from ProjectDescription_Pr_1  inner join  " + ConfigurationManager.AppSettings["BookingServerReplica"] + "LabComputersPr on substring( " + ConfigurationManager.AppSettings["BookingServerReplica"] + "LabComputersPr.FQDN, 1, len(System_name)) = [ProjectDescription_Pr_1 ].System_name inner join " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo] on " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo].id = LabComputersPr.LabId where System_name in (" + pclist + ") AND CONVERT(DATETIME, Date) > = '" + sdate + "' AND CONVERT(DATETIME, Date) < = '" + edate + "'  GROUP BY   " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo].DisplayName ,  [System_name],[Component_name],[Version]  ,[Details] order by CreationDate  ";
                            Query = " Select " + ConfigurationManager.AppSettings["BookingServerReplica"] + " [LabInfo].DisplayName, [System_name],[Component_name],[Version] ,Date = max(Date) ,[Details], CreationDate = max([CreationDate]), case when Component_name like '%ma_df%' then 'WSS' when Component_name like '%ma_ds%' then 'Pressure' when (Component_name like '%ma_pts%' or Component_name like '%ma_pwg%') then 'Brake Pedal' when Component_name like '%ma_tem%' then 'Temperature' when Component_name like '%ma_vp%' then 'Vacuum Pressure' end as 'SensorType' from ProjectDescription_Pr_1  inner join  " + ConfigurationManager.AppSettings["BookingServerReplica"] + "LabComputersPr on substring( " + ConfigurationManager.AppSettings["BookingServerReplica"] + "LabComputersPr.FQDN, 1, len(System_name)) = [ProjectDescription_Pr_1 ].System_name inner join " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo] on " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo].id = LabComputersPr.LabId where System_name in (" + pclist + ") AND CONVERT(DATETIME, Date) > = '" + sdate + "' AND CONVERT(DATETIME, Date) < = '" + edate + "' GROUP BY   " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo].DisplayName ,  [System_name],[Component_name],[Version]  ,[Details] order by CreationDate ";

                        }
                        else
                        {
                            Query = " Select " + ConfigurationManager.AppSettings["BookingServerReplica"] + " [LabInfo].DisplayName, [System_name],[Component_name],[Version] ,Date = max(Date) ,[Details], CreationDate = max([CreationDate]), case when Component_name like '%ma_df%' then 'WSS' when Component_name like '%ma_ds%' then 'Pressure' when (Component_name like '%ma_pts%' or Component_name like '%ma_pwg%') then 'Brake Pedal' when Component_name like '%ma_tem%' then 'Temperature' when Component_name like '%ma_vp%' then 'Vacuum Pressure' end as 'SensorType' from ProjectDescription_Pr_1  inner join  " + ConfigurationManager.AppSettings["BookingServerReplica"] + "LabComputersPr on substring( " + ConfigurationManager.AppSettings["BookingServerReplica"] + "LabComputersPr.FQDN, 1, len(System_name)) = [ProjectDescription_Pr_1 ].System_name inner join " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo] on " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo].id = LabComputersPr.LabId where System_name in (" + pclist + ") AND CONVERT(DATETIME, Date) > = '" + sdate + "' AND CONVERT(DATETIME, Date) < = '" + edate + "' and (Component_name like '%ma_df%' or  Component_name like '%ma_ds%' or (Component_name like '%ma_pts%' or Component_name like '%ma_pwg%') or Component_name like '%ma_tem%' or Component_name like '%ma_vp%') GROUP BY   " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo].DisplayName ,  [System_name],[Component_name],[Version]  ,[Details] order by CreationDate ";

                        }

                        OpenConnection();
                        SqlCommand cmd = new SqlCommand(Query, con);
                        cmd.CommandTimeout = 0;
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dtable);
                        CloseConnection();

                        foreach (DataRow row in dtable.Rows)
                        {
                            TABLE2_ProjectDescriptionView obj = new TABLE2_ProjectDescriptionView();
                            obj.System_name = row["System_name"].ToString();
                            obj.LC_Name = row["DisplayName"].ToString();
                            obj.Date = row["Date"].ToString(); obj.CreationDate = DateTime.Parse(row["CreationDate"].ToString()).AddMinutes(diff_UTCandLocal).ToLongTimeString();

                            obj.Component_name = row["Component_name"].ToString();

                            obj.Details = row["Details"].ToString();

                            obj.Version = row["Version"].ToString();

                            obj.SensorType = row["SensorType"].ToString();
                            PrjDataViews.Add(obj);
                        }
                        //hwDataViews = hwDataViews.OrderByDescending(x => x.Date).GroupBy(x => new { x.EEPName, x.Date, x.System_name }).Select(x => x.FirstOrDefault()).OrderBy(x => x.Date).ToList();

                        var result = Json(new
                        {
                            data = PrjDataViews
                        },
                     JsonRequestBehavior.AllowGet); result.MaxJsonLength = int.MaxValue; return result;
                        //var result = Json(new { data = PrjDataViews },  JsonRequestBehavior.AllowGet);result.MaxJsonLength = int.MaxValue;return result;
                    }
                }
                catch (Exception Ex)
                {
                    return View("Unable to retrieve data from database. Please try again later!!");
                }
            }
            else
            {
                var result = Json(new { success = false, message = "Try again" }, JsonRequestBehavior.AllowGet); result.MaxJsonLength = int.MaxValue; return result;
            }
        }


        [HttpPost]
        public ActionResult TABLE3_GetEBData_unique(List<string> pc, DateTime sdate, DateTime edate, int diff_UTCandLocal)
        {
            List<TABLE3_EBView> table3DataViews = new List<TABLE3_EBView>();
            if (pc != null && pc.Count() > 0)
            {
                string pclist = string.Empty;
                try
                {
                    using (PromasterImport_HWInfoEntities db = new PromasterImport_HWInfoEntities())
                    {
                        db.Database.CommandTimeout = 500;
                        string date = string.Empty;
                        foreach (var p in pc)
                        {
                            pclist += ("'" + p.Trim() + "',");
                            //pclist.Add(p.FQDN.Split('.')[0].ToUpper().Trim());
                        }

                        pclist = pclist.Remove(pclist.Length - 1, 1);
                        connection();
                        DataTable dtable = new DataTable();
                        // labtype 8 9 ; created dt of bookingserver replioca >= s dt ; created dt <= e date
                        string Query = " Select " + ConfigurationManager.AppSettings["BookingServerReplica"] + " [LabInfo].DisplayName, [System_name],Date = max(Date),[Kernel_EB_Cards_EB5100] ," +
                            "[Kernel_EB_Cards_EB5200],[Project],CreationDate = max([CreationDate])from [EB_Cards_Pr] inner join" +
                            "  " + ConfigurationManager.AppSettings["BookingServerReplica"] + "LabComputersPr on substring( " + ConfigurationManager.AppSettings["BookingServerReplica"] + "LabComputersPr.FQDN, 1, len(System_name)) " +
                            "= [EB_Cards_Pr].System_name inner join " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo] on " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo].id =" +
                            " LabComputersPr.LabId where System_name in (" + pclist + ") AND CONVERT(DATETIME, Date) > = '" + sdate + "' AND" +
                            " CONVERT(DATETIME, Date) < = '" + edate + "' GROUP BY   " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo].DisplayName ," +
                            " [System_name],[Kernel_EB_Cards_EB5100] ,[Kernel_EB_Cards_EB5200],[Project] order by CreationDate";

                        OpenConnection();
                        SqlCommand cmd = new SqlCommand(Query, con);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dtable);
                        CloseConnection();

                        foreach (DataRow row in dtable.Rows)
                        {
                            //no labcar mapping
                            TABLE3_EBView obj = new TABLE3_EBView();
                            obj.System_name = row["System_name"].ToString(); obj.Project = row["Project"].ToString();
                            obj.LC_Name = row["DisplayName"].ToString();
                            obj.Date = row["Date"].ToString(); obj.CreationDate = DateTime.Parse(row["CreationDate"].ToString()).AddMinutes(diff_UTCandLocal).ToLongTimeString();
                            obj.Kernel_EB_Cards_EB5100 = row["Kernel_EB_Cards_EB5100"].ToString();
                            obj.Kernel_EB_Cards_EB5200 = row["Kernel_EB_Cards_EB5200"].ToString();
                            table3DataViews.Add(obj);
                        }
                        //hwDataViews = hwDataViews.OrderByDescending(x => x.Date).GroupBy(x => new { x.EEPName, x.Date, x.System_name }).Select(x => x.FirstOrDefault()).OrderBy(x => x.Date).ToList();


                        var result = Json(new { data = table3DataViews }, JsonRequestBehavior.AllowGet); result.MaxJsonLength = int.MaxValue; return result;
                    }
                }
                catch (Exception Ex)
                {
                    return View("Unable to retrieve data from database. Please try again later!!");
                }
            }
            else
            {
                var result = Json(new { success = false, message = "Try again" }, JsonRequestBehavior.AllowGet); result.MaxJsonLength = int.MaxValue; return result;
            }
        }


        [HttpPost]
        public ActionResult GetTABLE4_ES4441Data_unique(List<string> pc, DateTime sdate, DateTime edate, int diff_UTCandLocal)
        {
            List<TABLE4_ES4441View> table3DataViews = new List<TABLE4_ES4441View>();
            if (pc != null && pc.Count() > 0)
            {
                string pclist = string.Empty;
                try
                {
                    using (PromasterImport_HWInfoEntities db = new PromasterImport_HWInfoEntities())
                    {
                        db.Database.CommandTimeout = 500;
                        string date = string.Empty;
                        foreach (var p in pc)
                        {
                            pclist += ("'" + p.Trim() + "',");
                            //pclist.Add(p.FQDN.Split('.')[0].ToUpper().Trim());
                        }

                        pclist = pclist.Remove(pclist.Length - 1, 1);
                        connection();
                        DataTable dtable = new DataTable();

                        string Query = "Select  " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo].DisplayName, [ES4441_1_Pr].[System_name],Date = max([ES4441_1_Pr].[Date]) ,[Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441SN]  ,[Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441FWVersionMain] ,[Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441FWVersionMajor] ,[Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441FWVersionMinor] ,[ES4441_1_Pr].[Project],CreationDate = max([ES4441_1_Pr].[CreationDate]),[Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441HWVersion],[Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441HWRevision]from [ES4441_1_Pr]inner join [ES4441_2_Pr] on [ES4441_1_Pr].System_name = [ES4441_2_Pr].System_name and [ES4441_1_Pr].CreationDate = [ES4441_2_Pr].CreationDate inner join  " + ConfigurationManager.AppSettings["BookingServerReplica"] + "LabComputersPr on substring( " + ConfigurationManager.AppSettings["BookingServerReplica"] + "LabComputersPr.FQDN, 1, len([ES4441_1_Pr].System_name)) = [ES4441_1_Pr].System_name inner join " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo] on " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo].id = LabComputersPr.LabId where [ES4441_1_Pr].System_name in (" + pclist + ")AND CONVERT(DATETIME, [ES4441_1_Pr].Date) > = '" + sdate + "'  AND CONVERT(DATETIME, [ES4441_1_Pr].Date) < = '" + edate + "' GROUP BY [ES4441_1_Pr].[System_name] ,[Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441SN]  ,[Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441PLDVersion] ,[Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441FWVersionMain] ,[Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441FWVersionMajor] ,[Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441FWVersionMinor] ,[ES4441_1_Pr].[Project],[Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441HWVersion],[Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441HWRevision], " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo].DisplayName order by CreationDate";
                        OpenConnection();
                        SqlCommand cmd = new SqlCommand(Query, con);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dtable);
                        CloseConnection();
                        foreach (DataRow row in dtable.Rows)
                        {
                            TABLE4_ES4441View obj = new TABLE4_ES4441View();
                            obj.System_name = row["System_name"].ToString(); obj.Project = row["Project"].ToString();
                            obj.LC_Name = row["DisplayName"].ToString();
                            obj.Date = row["Date"].ToString(); obj.CreationDate = DateTime.Parse(row["CreationDate"].ToString()).AddMinutes(diff_UTCandLocal).ToLongTimeString();
                            obj.Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441FWVersionMain = row["Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441FWVersionMain"].ToString();
                            obj.Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441FWVersionMajor = row["Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441FWVersionMajor"].ToString();
                            obj.Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441FWVersionMinor = row["Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441FWVersionMinor"].ToString();
                            obj.Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441HWRevision = row["Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441HWRevision"].ToString();
                            obj.Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441HWVersion = row["Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441HWVersion"].ToString();
                            obj.Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441SN = row["Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441SN"].ToString();
                            table3DataViews.Add(obj);
                        }

                        Query = "Select  " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo].DisplayName, [System_name],Date = max([Date]) ,[Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441SN]  ,[Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441FWVersionMain] ,[Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441FWVersionMajor] ,[Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441FWVersionMinor] ,[Project],CreationDate = max([CreationDate]),[Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441HWVersion],[Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441HWRevision]from [ES4441_Pr]  inner join  " + ConfigurationManager.AppSettings["BookingServerReplica"] + "LabComputersPr on substring( " + ConfigurationManager.AppSettings["BookingServerReplica"] + "LabComputersPr.FQDN, 1, len(System_name)) = System_name inner join " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo] on " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo].id = LabComputersPr.LabId where System_name in (" + pclist + ")AND CONVERT(DATETIME, Date) > = '" + sdate + "'  AND CONVERT(DATETIME,Date) < = '" + edate + "' GROUP BY [System_name] ,[Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441SN]  ,[Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441PLDVersion] ,[Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441FWVersionMain] ,[Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441FWVersionMajor] ,[Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441FWVersionMinor] ,[Project],[Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441HWVersion],[Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441HWRevision], " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo].DisplayName order by CreationDate";
                        OpenConnection();
                        cmd = new SqlCommand(Query, con);
                        da = new SqlDataAdapter(cmd);
                        DataTable dtable1 = new DataTable();
                        da.Fill(dtable1);
                        CloseConnection();

                        foreach (DataRow row in dtable1.Rows)
                        {
                            TABLE4_ES4441View obj = new TABLE4_ES4441View();
                            obj.System_name = row["System_name"].ToString(); obj.Project = row["Project"].ToString();
                            obj.LC_Name = row["DisplayName"].ToString();
                            obj.Date = row["Date"].ToString(); obj.CreationDate = DateTime.Parse(row["CreationDate"].ToString()).AddMinutes(diff_UTCandLocal).ToLongTimeString();
                            obj.Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441FWVersionMain = row["Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441FWVersionMain"].ToString();
                            obj.Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441FWVersionMajor = row["Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441FWVersionMajor"].ToString();
                            obj.Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441FWVersionMinor = row["Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441FWVersionMinor"].ToString();
                            obj.Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441HWRevision = row["Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441HWRevision"].ToString();
                            obj.Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441HWVersion = row["Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441HWVersion"].ToString();
                            obj.Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441SN = row["Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441SN"].ToString();
                            table3DataViews.Add(obj);
                        }


                        var result = Json(new { data = table3DataViews }, JsonRequestBehavior.AllowGet); result.MaxJsonLength = int.MaxValue; return result;
                    }
                }
                catch (Exception Ex)
                {
                    return View("Unable to retrieve data from database. Please try again later!!");
                }
            }
            else
            {
                var result = Json(new { success = false, message = "Try again" }, JsonRequestBehavior.AllowGet); result.MaxJsonLength = int.MaxValue; return result;
            }
        }
        [HttpPost]
        public ActionResult GetTABLE5_OTSOData_unique(List<string> pc, DateTime sdate, DateTime edate, int diff_UTCandLocal)
        {
            List<TABLE5_OTSOView> table5DataViews = new List<TABLE5_OTSOView>();
            if (pc != null && pc.Count() > 0)
            {
                string pclist = string.Empty;
                try
                {
                    using (PromasterImport_HWInfoEntities db = new PromasterImport_HWInfoEntities())
                    {
                        db.Database.CommandTimeout = 500;
                        string date = string.Empty;
                        foreach (var p in pc)
                        {
                            pclist += ("'" + p.Trim() + "',");
                            //pclist.Add(p.FQDN.Split('.')[0].ToUpper().Trim());
                        }

                        pclist = pclist.Remove(pclist.Length - 1, 1);
                        connection();
                        DataTable dtable = new DataTable();

                        string Query = " Select " + ConfigurationManager.AppSettings["BookingServerReplica"] + " [LabInfo].DisplayName, [System_name],Date = max(Date) ,[Kernel_Component_IPB_TemperatureReceive_OTSO1Available] ,[Kernel_Component_IPB_TemperatureReceive_OTSO2Available] ,[Project],CreationDate = max([CreationDate])from [OTSO_Pr] inner join  " + ConfigurationManager.AppSettings["BookingServerReplica"] + "LabComputersPr on substring( " + ConfigurationManager.AppSettings["BookingServerReplica"] + "LabComputersPr.FQDN, 1, len(System_name)) = [OTSO_Pr].System_name inner join " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo] on " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo].id = LabComputersPr.LabId where System_name in (" + pclist + ") AND CONVERT(DATETIME, Date) > = '" + sdate + "' AND CONVERT(DATETIME, Date) < = '" + edate + "' GROUP BY   " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo].DisplayName , [System_name],[Kernel_Component_IPB_TemperatureReceive_OTSO1Available] ,[Kernel_Component_IPB_TemperatureReceive_OTSO2Available] ,[Project] order by CreationDate";

                        OpenConnection();
                        SqlCommand cmd = new SqlCommand(Query, con);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dtable);
                        CloseConnection();

                        foreach (DataRow row in dtable.Rows)
                        {
                            TABLE5_OTSOView obj = new TABLE5_OTSOView();
                            obj.System_name = row["System_name"].ToString(); obj.Project = row["Project"].ToString();
                            obj.LC_Name = row["DisplayName"].ToString();
                            obj.Date = row["Date"].ToString(); obj.CreationDate = DateTime.Parse(row["CreationDate"].ToString()).AddMinutes(diff_UTCandLocal).ToLongTimeString();
                            obj.Kernel_Component_IPB_TemperatureReceive_OTSO1Available = row["Kernel_Component_IPB_TemperatureReceive_OTSO1Available"].ToString();
                            obj.Kernel_Component_IPB_TemperatureReceive_OTSO2Available = row["Kernel_Component_IPB_TemperatureReceive_OTSO2Available"].ToString();
                            table5DataViews.Add(obj);
                        }
                        //hwDataViews = hwDataViews.OrderByDescending(x => x.Date).GroupBy(x => new { x.EEPName, x.Date, x.System_name }).Select(x => x.FirstOrDefault()).OrderBy(x => x.Date).ToList();


                        var result = Json(new { data = table5DataViews }, JsonRequestBehavior.AllowGet); result.MaxJsonLength = int.MaxValue; return result;
                    }
                }
                catch (Exception Ex)
                {
                    return View("Unable to retrieve data from database. Please try again later!!");
                }
            }
            else
            {
                var result = Json(new { success = false, message = "Try again" }, JsonRequestBehavior.AllowGet); result.MaxJsonLength = int.MaxValue; return result;
            }
        }
        [HttpPost]
        public ActionResult GetTABLE6_PowerSupplyData_unique(List<string> pc, DateTime sdate, DateTime edate, int diff_UTCandLocal)
        {
            List<TABLE6_PowerSupplyView> table6DataViews = new List<TABLE6_PowerSupplyView>();
            if (pc != null && pc.Count() > 0)
            {
                string pclist = string.Empty;
                try
                {
                    using (PromasterImport_HWInfoEntities db = new PromasterImport_HWInfoEntities())
                    {
                        db.Database.CommandTimeout = 500;
                        string date = string.Empty;
                        foreach (var p in pc)
                        {
                            pclist += ("'" + p.Trim() + "',");
                            //pclist.Add(p.FQDN.Split('.')[0].ToUpper().Trim());
                        }

                        pclist = pclist.Remove(pclist.Length - 1, 1);
                        connection();
                        DataTable dtable = new DataTable();

                        string Query = " Select " + ConfigurationManager.AppSettings["BookingServerReplica"] + " [LabInfo].DisplayName, [System_name],Date = max(Date), [Kernel_Loadbox_Card_PSC_Power_Supply_Configuration_PS_Configuration],[Kernel_Loadbox_Card_PSC_Power_Supply_Configuration_PS_Factor_ECU],[Kernel_Loadbox_Card_PSC_Power_Supply_Configuration_PS_Factor_MR],[Kernel_Loadbox_Card_PSC_Power_Supply_Configuration_PS_Factor_VR] ,[Project],CreationDate = max([CreationDate])from [Power_Supply_Pr] inner join  " + ConfigurationManager.AppSettings["BookingServerReplica"] + "LabComputersPr on substring( " + ConfigurationManager.AppSettings["BookingServerReplica"] + "LabComputersPr.FQDN, 1, len(System_name)) = [Power_Supply_Pr].System_name inner join " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo] on " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo].id = LabComputersPr.LabId where System_name in (" + pclist + ") AND CONVERT(DATETIME, Date) > = '" + sdate + "' AND CONVERT(DATETIME, Date) < = '" + edate + "' GROUP BY   " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo].DisplayName , [System_name],[Kernel_Loadbox_Card_PSC_Power_Supply_Configuration_PS_Configuration],[Kernel_Loadbox_Card_PSC_Power_Supply_Configuration_PS_Factor_ECU],[Kernel_Loadbox_Card_PSC_Power_Supply_Configuration_PS_Factor_MR],[Kernel_Loadbox_Card_PSC_Power_Supply_Configuration_PS_Factor_VR] ,[Project] order by CreationDate";
                        OpenConnection();
                        SqlCommand cmd = new SqlCommand(Query, con);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dtable);
                        CloseConnection();

                        foreach (DataRow row in dtable.Rows)
                        {
                            TABLE6_PowerSupplyView obj = new TABLE6_PowerSupplyView();
                            obj.System_name = row["System_name"].ToString(); obj.Project = row["Project"].ToString();
                            obj.LC_Name = row["DisplayName"].ToString();
                            obj.Date = row["Date"].ToString(); obj.CreationDate = DateTime.Parse(row["CreationDate"].ToString()).AddMinutes(diff_UTCandLocal).ToLongTimeString();
                            obj.Kernel_Loadbox_Card_PSC_Power_Supply_Configuration_PS_Configuration = row["Kernel_Loadbox_Card_PSC_Power_Supply_Configuration_PS_Configuration"].ToString();
                            obj.Kernel_Loadbox_Card_PSC_Power_Supply_Configuration_PS_Factor_ECU = row["Kernel_Loadbox_Card_PSC_Power_Supply_Configuration_PS_Factor_ECU"].ToString();
                            obj.Kernel_Loadbox_Card_PSC_Power_Supply_Configuration_PS_Factor_MR = row["Kernel_Loadbox_Card_PSC_Power_Supply_Configuration_PS_Factor_MR"].ToString();
                            obj.Kernel_Loadbox_Card_PSC_Power_Supply_Configuration_PS_Factor_VR = row["Kernel_Loadbox_Card_PSC_Power_Supply_Configuration_PS_Factor_VR"].ToString();

                            table6DataViews.Add(obj);
                        }
                        //hwDataViews = hwDataViews.OrderByDescending(x => x.Date).GroupBy(x => new { x.EEPName, x.Date, x.System_name }).Select(x => x.FirstOrDefault()).OrderBy(x => x.Date).ToList();


                        var result = Json(new { data = table6DataViews }, JsonRequestBehavior.AllowGet); result.MaxJsonLength = int.MaxValue; return result;
                    }
                }
                catch (Exception Ex)
                {
                    return View("Unable to retrieve data from database. Please try again later!!");
                }
            }
            else
            {
                var result = Json(new { success = false, message = "Try again" }, JsonRequestBehavior.AllowGet); result.MaxJsonLength = int.MaxValue; return result;
            }
        }


        [HttpPost]
        public ActionResult GetTABLE7_BOBData_unique(List<string> pc, DateTime sdate, DateTime edate, int diff_UTCandLocal)
        {
            List<TABLE7_BOBView> table3DataViews = new List<TABLE7_BOBView>();
            if (pc != null && pc.Count() > 0)
            {
                string pclist = string.Empty;
                try
                {
                    using (PromasterImport_HWInfoEntities db = new PromasterImport_HWInfoEntities())
                    {
                        db.Database.CommandTimeout = 500;
                        string date = string.Empty;
                        foreach (var p in pc)
                        {
                            pclist += ("'" + p.Trim() + "',");
                            //pclist.Add(p.FQDN.Split('.')[0].ToUpper().Trim());
                        }

                        pclist = pclist.Remove(pclist.Length - 1, 1);
                        connection();
                        DataTable dtable = new DataTable();

                        string Query = " Select " + ConfigurationManager.AppSettings["BookingServerReplica"] + " [LabInfo].DisplayName, [System_name],Date = max(Date)   ,[Kernel_Loadbox_Card_ECC_BreakOutBox_BOB_SerialNumber],[Kernel_Loadbox_Card_ECC_BreakOutBox_BOB_DeviceRevision] ,[Kernel_Loadbox_Card_ECC_BreakOutBox_BOB_OptionalDescription],[Project],CreationDate = max([CreationDate])from [BreakOutBox_Pr] inner join  " + ConfigurationManager.AppSettings["BookingServerReplica"] + "LabComputersPr on substring( " + ConfigurationManager.AppSettings["BookingServerReplica"] + "LabComputersPr.FQDN, 1, len(System_name)) = [BreakOutBox_Pr].System_name inner join " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo] on " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo].id = LabComputersPr.LabId where System_name in (" + pclist + ") AND CONVERT(DATETIME, Date) > = '" + sdate + "' AND CONVERT(DATETIME, Date) < = '" + edate + "' GROUP BY   " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo].DisplayName , [System_name]  ,[Kernel_Loadbox_Card_ECC_BreakOutBox_BOB_SerialNumber],[Kernel_Loadbox_Card_ECC_BreakOutBox_BOB_DeviceRevision] ,[Kernel_Loadbox_Card_ECC_BreakOutBox_BOB_OptionalDescription] ,[Project] order by CreationDate";

                        OpenConnection();
                        SqlCommand cmd = new SqlCommand(Query, con);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dtable);
                        CloseConnection();

                        foreach (DataRow row in dtable.Rows)
                        {
                            TABLE7_BOBView obj = new TABLE7_BOBView();
                            obj.System_name = row["System_name"].ToString(); obj.Project = row["Project"].ToString();
                            obj.LC_Name = row["DisplayName"].ToString();
                            obj.Date = row["Date"].ToString(); obj.CreationDate = DateTime.Parse(row["CreationDate"].ToString()).AddMinutes(diff_UTCandLocal).ToLongTimeString();
                            obj.Kernel_Loadbox_Card_ECC_BreakOutBox_BOB_DeviceRevision = row["Kernel_Loadbox_Card_ECC_BreakOutBox_BOB_DeviceRevision"].ToString();
                            obj.Kernel_Loadbox_Card_ECC_BreakOutBox_BOB_SerialNumber = row["Kernel_Loadbox_Card_ECC_BreakOutBox_BOB_DeviceRevision"].ToString();
                            table3DataViews.Add(obj);
                        }
                        //hwDataViews = hwDataViews.OrderByDescending(x => x.Date).GroupBy(x => new { x.EEPName, x.Date, x.System_name }).Select(x => x.FirstOrDefault()).OrderBy(x => x.Date).ToList();


                        var result = Json(new { data = table3DataViews }, JsonRequestBehavior.AllowGet); result.MaxJsonLength = int.MaxValue; return result;
                    }
                }
                catch (Exception Ex)
                {
                    return View("Unable to retrieve data from database. Please try again later!!");
                }
            }
            else
            {
                var result = Json(new { success = false, message = "Try again" }, JsonRequestBehavior.AllowGet); result.MaxJsonLength = int.MaxValue; return result;
            }

        }

        [HttpPost]
        public ActionResult GetTABLE9_IBData_unique(List<string> pc, DateTime sdate, DateTime edate, int diff_UTCandLocal)
        {
            List<TABLE9_IBView> table5DataViews = new List<TABLE9_IBView>();
            if (pc != null && pc.Count() > 0)
            {
                string pclist = string.Empty;
                try
                {
                    using (PromasterImport_HWInfoEntities db = new PromasterImport_HWInfoEntities())
                    {
                        db.Database.CommandTimeout = 500;
                        string date = string.Empty;
                        foreach (var p in pc)
                        {
                            pclist += ("'" + p.Trim() + "',");
                            //pclist.Add(p.FQDN.Split('.')[0].ToUpper().Trim());
                        }

                        pclist = pclist.Remove(pclist.Length - 1, 1);
                        connection();
                        DataTable dtable = new DataTable();

                        string Query = " Select " + ConfigurationManager.AppSettings["BookingServerReplica"] + " [LabInfo].DisplayName, [System_name] ,Date = max(Date) ,[ModelSpecific_EID_NET_CAN_Settings_CAN_Settings_rtpc_ixxat_config], [Project],CreationDate = max([CreationDate])from [IB_Card_Pr] inner join  " + ConfigurationManager.AppSettings["BookingServerReplica"] + "LabComputersPr on substring( " + ConfigurationManager.AppSettings["BookingServerReplica"] + "LabComputersPr.FQDN, 1, len(System_name)) = [IB_Card_Pr].System_name inner join " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo] on " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo].id = LabComputersPr.LabId where System_name in (" + pclist + ") AND CONVERT(DATETIME, Date) > = '" + sdate + "' AND CONVERT(DATETIME, Date) < = '" + edate + "' GROUP BY   " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo].DisplayName , [System_name]  ,[ModelSpecific_EID_NET_CAN_Settings_CAN_Settings_rtpc_ixxat_config], [Project] order by CreationDate";

                        OpenConnection();
                        SqlCommand cmd = new SqlCommand(Query, con);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dtable);
                        CloseConnection();

                        foreach (DataRow row in dtable.Rows)
                        {
                            TABLE9_IBView obj = new TABLE9_IBView();
                            obj.System_name = row["System_name"].ToString(); obj.Project = row["Project"].ToString();
                            obj.LC_Name = row["DisplayName"].ToString();
                            obj.Date = row["Date"].ToString(); obj.CreationDate = DateTime.Parse(row["CreationDate"].ToString()).AddMinutes(diff_UTCandLocal).ToLongTimeString();
                            obj.ModelSpecific_EID_NET_CAN_Settings_CAN_Settings_rtpc_ixxat_config = row["ModelSpecific_EID_NET_CAN_Settings_CAN_Settings_rtpc_ixxat_config"].ToString();
                            table5DataViews.Add(obj);
                        }
                        //hwDataViews = hwDataViews.OrderByDescending(x => x.Date).GroupBy(x => new { x.EEPName, x.Date, x.System_name }).Select(x => x.FirstOrDefault()).OrderBy(x => x.Date).ToList();


                        var result = Json(new { data = table5DataViews }, JsonRequestBehavior.AllowGet); result.MaxJsonLength = int.MaxValue; return result;
                    }
                }
                catch (Exception Ex)
                {
                    return View("Unable to retrieve data from database. Please try again later!!");
                }
            }
            else
            {
                var result = Json(new { success = false, message = "Try again" }, JsonRequestBehavior.AllowGet); result.MaxJsonLength = int.MaxValue; return result;
            }
        }

        [HttpPost]
        public ActionResult GetTABLE10_IXXATData_unique(List<string> pc, DateTime sdate, DateTime edate, int diff_UTCandLocal)
        {
            List<TABLE10_IXXAT2View> table4DataViews = new List<TABLE10_IXXAT2View>();
            if (pc != null && pc.Count() > 0)
            {
                string pclist = string.Empty;
                try
                {
                    using (PromasterImport_HWInfoEntities db = new PromasterImport_HWInfoEntities())
                    {
                        db.Database.CommandTimeout = 500;
                        string date = string.Empty;
                        foreach (var p in pc)
                        {
                            pclist += ("'" + p.Trim() + "',");
                            //pclist.Add(p.FQDN.Split('.')[0].ToUpper().Trim());
                        }

                        pclist = pclist.Remove(pclist.Length - 1, 1);
                        connection();
                        DataTable dtable = new DataTable();

                        string Query = " Select " + ConfigurationManager.AppSettings["BookingServerReplica"] + " [LabInfo].DisplayName, [System_name] , Date = max(Date) ,[ModelSpecific_EID_NET_CAN_Settings_CAN_Settings_rtpc_ixxat_config] ,[Project],CreationDate = max([CreationDate])from [IXXAT_Config_Pr] inner join  " + ConfigurationManager.AppSettings["BookingServerReplica"] + "LabComputersPr on substring( " + ConfigurationManager.AppSettings["BookingServerReplica"] + "LabComputersPr.FQDN, 1, len(System_name)) = [IXXAT_Config_Pr].System_name inner join " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo] on " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo].id = LabComputersPr.LabId where System_name in (" + pclist + ") AND CONVERT(DATETIME, Date) > = '" + sdate + "' AND CONVERT(DATETIME, Date) < = '" + edate + "' GROUP BY   " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo].DisplayName , [System_name]  ,[ModelSpecific_EID_NET_CAN_Settings_CAN_Settings_rtpc_ixxat_config]  ,[Project] order by CreationDate";

                        OpenConnection();
                        SqlCommand cmd = new SqlCommand(Query, con);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dtable);
                        CloseConnection();

                        foreach (DataRow row in dtable.Rows)
                        {
                            TABLE10_IXXAT2View obj = new TABLE10_IXXAT2View();
                            obj.System_name = row["System_name"].ToString(); obj.Project = row["Project"].ToString();
                            obj.LC_Name = row["DisplayName"].ToString();
                            obj.Date = row["Date"].ToString(); obj.CreationDate = DateTime.Parse(row["CreationDate"].ToString()).AddMinutes(diff_UTCandLocal).ToLongTimeString();
                            obj.ModelSpecific_EID_NET_CAN_Settings_CAN_Settings_rtpc_ixxat_config = row["ModelSpecific_EID_NET_CAN_Settings_CAN_Settings_rtpc_ixxat_config"].ToString();
                            table4DataViews.Add(obj);
                        }
                        //hwDataViews = hwDataViews.OrderByDescending(x => x.Date).GroupBy(x => new { x.EEPName, x.Date, x.System_name }).Select(x => x.FirstOrDefault()).OrderBy(x => x.Date).ToList();


                        var result = Json(new { data = table4DataViews }, JsonRequestBehavior.AllowGet); result.MaxJsonLength = int.MaxValue; return result;
                    }
                }
                catch (Exception Ex)
                {
                    return View("Unable to retrieve data from database. Please try again later!!");
                }
            }
            else
            {
                var result = Json(new { success = false, message = "Try again" }, JsonRequestBehavior.AllowGet); result.MaxJsonLength = int.MaxValue; return result;
            }
        }
        [HttpPost]
        public ActionResult GetTABLE11_APBData_unique(List<string> pc, DateTime sdate, DateTime edate, int diff_UTCandLocal)
        {
            List<TABLE11_APBView> table3DataViews = new List<TABLE11_APBView>();
            if (pc != null && pc.Count() > 0)
            {
                string pclist = string.Empty;
                try
                {
                    using (PromasterImport_HWInfoEntities db = new PromasterImport_HWInfoEntities())
                    {
                        db.Database.CommandTimeout = 500;
                        string date = string.Empty;
                        foreach (var p in pc)
                        {
                            pclist += ("'" + p.Trim() + "',");
                            //pclist.Add(p.FQDN.Split('.')[0].ToUpper().Trim());
                        }

                        pclist = pclist.Remove(pclist.Length - 1, 1);
                        connection();
                        DataTable dtable = new DataTable();

                        string Query = " Select " + ConfigurationManager.AppSettings["BookingServerReplica"] + " [LabInfo].DisplayName, [System_name],Date = max(Date),[ModelSpecific_EID_Switch_APB_APB_Module_GIO2_Signature_GIO2] ,[Project], CreationDate = max([CreationDate])from [APB_Pr] inner join  " + ConfigurationManager.AppSettings["BookingServerReplica"] + "LabComputersPr on substring( " + ConfigurationManager.AppSettings["BookingServerReplica"] + "LabComputersPr.FQDN, 1, len(System_name)) = [APB_Pr].System_name inner join " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo] on " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo].id = LabComputersPr.LabId where System_name in (" + pclist + ") AND CONVERT(DATETIME, Date) > = '" + sdate + "' AND CONVERT(DATETIME, Date) < = '" + edate + "' GROUP BY   " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo].DisplayName , [System_name],[ModelSpecific_EID_Switch_APB_APB_Module_GIO2_Signature_GIO2] ,[Project] order by CreationDate";

                        OpenConnection();
                        SqlCommand cmd = new SqlCommand(Query, con);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dtable);
                        CloseConnection();

                        foreach (DataRow row in dtable.Rows)
                        {
                            TABLE11_APBView obj = new TABLE11_APBView();
                            obj.System_name = row["System_name"].ToString(); obj.Project = row["Project"].ToString();
                            obj.LC_Name = row["DisplayName"].ToString();
                            obj.Date = row["Date"].ToString(); obj.CreationDate = DateTime.Parse(row["CreationDate"].ToString()).AddMinutes(diff_UTCandLocal).ToLongTimeString();
                            obj.ModelSpecific_EID_Switch_APB_APB_Module_GIO2_Signature_GIO2 = row["ModelSpecific_EID_Switch_APB_APB_Module_GIO2_Signature_GIO2"].ToString();
                            table3DataViews.Add(obj);
                        }
                        //hwDataViews = hwDataViews.OrderByDescending(x => x.Date).GroupBy(x => new { x.EEPName, x.Date, x.System_name }).Select(x => x.FirstOrDefault()).OrderBy(x => x.Date).ToList();


                        var result = Json(new { data = table3DataViews }, JsonRequestBehavior.AllowGet); result.MaxJsonLength = int.MaxValue; return result;
                    }
                }
                catch (Exception Ex)
                {
                    return View("Unable to retrieve data from database. Please try again later!!");
                }
            }
            else
            {
                var result = Json(new { success = false, message = "Try again" }, JsonRequestBehavior.AllowGet); result.MaxJsonLength = int.MaxValue; return result;
            }

        }
        [HttpPost]
        public ActionResult GetTABLE12_HAPData_unique(List<string> pc, DateTime sdate, DateTime edate, int diff_UTCandLocal)
        {
            List<TABLE12_HAPView> table4DataViews = new List<TABLE12_HAPView>();
            if (pc != null && pc.Count() > 0)
            {
                string pclist = string.Empty;
                try
                {
                    using (PromasterImport_HWInfoEntities db = new PromasterImport_HWInfoEntities())
                    {
                        db.Database.CommandTimeout = 500;
                        string date = string.Empty;
                        foreach (var p in pc)
                        {
                            pclist += ("'" + p.Trim() + "',");
                            //pclist.Add(p.FQDN.Split('.')[0].ToUpper().Trim());
                        }

                        pclist = pclist.Remove(pclist.Length - 1, 1);
                        connection();
                        DataTable dtable = new DataTable();

                        string Query = " Select " + ConfigurationManager.AppSettings["BookingServerReplica"] + " [LabInfo].DisplayName, [System_name], Date = max(Date) ,[Kernel_Loadbox_Card_ECC_Harnesadapter_HAP_OrderNumber] ,[Kernel_Loadbox_Card_ECC_Harnesadapter_HAP_SerialNumber] ,[Kernel_Loadbox_Card_ECC_Harnesadapter_HAP_DeviceFunction] ,[Kernel_Loadbox_Card_ECC_Harnesadapter_HAP_DeviceRevision] ,[Kernel_Loadbox_Card_ECC_Harnesadapter_HAP_OptionalDescription] ,[Project],CreationDate = max([CreationDate])from [Harnesadapter_Pr] inner join  " + ConfigurationManager.AppSettings["BookingServerReplica"] + "LabComputersPr on substring( " + ConfigurationManager.AppSettings["BookingServerReplica"] + "LabComputersPr.FQDN, 1, len(System_name)) = [Harnesadapter_Pr].System_name inner join " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo] on " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo].id = LabComputersPr.LabId where System_name in (" + pclist + ") AND CONVERT(DATETIME, Date) > = '" + sdate + "' AND CONVERT(DATETIME, Date) < = '" + edate + "' GROUP BY   " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo].DisplayName , [System_name]  ,[Kernel_Loadbox_Card_ECC_Harnesadapter_HAP_OrderNumber] ,[Kernel_Loadbox_Card_ECC_Harnesadapter_HAP_SerialNumber] ,[Kernel_Loadbox_Card_ECC_Harnesadapter_HAP_DeviceFunction] ,[Kernel_Loadbox_Card_ECC_Harnesadapter_HAP_DeviceRevision] ,[Kernel_Loadbox_Card_ECC_Harnesadapter_HAP_OptionalDescription] ,[Project] order by CreationDate";

                        OpenConnection();
                        SqlCommand cmd = new SqlCommand(Query, con);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dtable);
                        CloseConnection();

                        foreach (DataRow row in dtable.Rows)
                        {
                            TABLE12_HAPView obj = new TABLE12_HAPView();
                            obj.System_name = row["System_name"].ToString(); obj.Project = row["Project"].ToString();
                            obj.LC_Name = row["DisplayName"].ToString();
                            obj.Date = row["Date"].ToString(); obj.CreationDate = DateTime.Parse(row["CreationDate"].ToString()).AddMinutes(diff_UTCandLocal).ToLongTimeString();
                            obj.Kernel_Loadbox_Card_ECC_Harnesadapter_HAP_DeviceRevision = row["Kernel_Loadbox_Card_ECC_Harnesadapter_HAP_DeviceRevision"].ToString();
                            obj.Kernel_Loadbox_Card_ECC_Harnesadapter_HAP_SerialNumber = row["Kernel_Loadbox_Card_ECC_Harnesadapter_HAP_SerialNumber"].ToString();
                            table4DataViews.Add(obj);
                        }
                        //hwDataViews = hwDataViews.OrderByDescending(x => x.Date).GroupBy(x => new { x.EEPName, x.Date, x.System_name }).Select(x => x.FirstOrDefault()).OrderBy(x => x.Date).ToList();


                        var result = Json(new { data = table4DataViews }, JsonRequestBehavior.AllowGet); result.MaxJsonLength = int.MaxValue; return result;
                    }
                }
                catch (Exception Ex)
                {
                    return View("Unable to retrieve data from database. Please try again later!!");
                }
            }
            else
            {
                var result = Json(new { success = false, message = "Try again" }, JsonRequestBehavior.AllowGet); result.MaxJsonLength = int.MaxValue; return result;
            }
        }


        [HttpPost]
        public ActionResult GetTABLE14_CableData_unique(List<string> pc, DateTime sdate, DateTime edate, int diff_UTCandLocal)
        {
            List<TABLE14_CableView> table3DataViews = new List<TABLE14_CableView>();
            if (pc != null && pc.Count() > 0)
            {
                string pclist = string.Empty;
                try
                {
                    using (PromasterImport_HWInfoEntities db = new PromasterImport_HWInfoEntities())
                    {
                        db.Database.CommandTimeout = 500;
                        string date = string.Empty;
                        foreach (var p in pc)
                        {
                            pclist += ("'" + p.Trim() + "',");
                            //pclist.Add(p.FQDN.Split('.')[0].ToUpper().Trim());
                        }

                        pclist = pclist.Remove(pclist.Length - 1, 1);
                        connection();
                        DataTable dtable = new DataTable();

                        string Query = " Select " + ConfigurationManager.AppSettings["BookingServerReplica"] + " [LabInfo].DisplayName, [System_name],Date = max(Date) ,[Kernel_Loadbox_Card_ECC_Cable_CABLE_OrderNumber] ,[Kernel_Loadbox_Card_ECC_Cable_CABLE_SerialNumber] ,[Kernel_Loadbox_Card_ECC_Cable_CABLE_DeviceFunction] ,[Kernel_Loadbox_Card_ECC_Cable_CABLE_DeviceRevision],[Kernel_Loadbox_Card_ECC_Cable_CABLE_OptionalDescription],[Project],CreationDate = max([CreationDate])from [Cable_Pr] inner join  " + ConfigurationManager.AppSettings["BookingServerReplica"] + "LabComputersPr on substring( " + ConfigurationManager.AppSettings["BookingServerReplica"] + "LabComputersPr.FQDN, 1, len(System_name)) = [Cable_Pr].System_name inner join " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo] on " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo].id = LabComputersPr.LabId where System_name in (" + pclist + ") AND CONVERT(DATETIME, Date) > = '" + sdate + "' AND CONVERT(DATETIME, Date) < = '" + edate + "' GROUP BY   " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo].DisplayName , [System_name],[Kernel_Loadbox_Card_ECC_Cable_CABLE_OrderNumber] ,[Kernel_Loadbox_Card_ECC_Cable_CABLE_SerialNumber] ,[Kernel_Loadbox_Card_ECC_Cable_CABLE_DeviceFunction] ,[Kernel_Loadbox_Card_ECC_Cable_CABLE_DeviceRevision],[Kernel_Loadbox_Card_ECC_Cable_CABLE_OptionalDescription],[Project] order by CreationDate";

                        OpenConnection();
                        SqlCommand cmd = new SqlCommand(Query, con);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dtable);
                        CloseConnection();

                        foreach (DataRow row in dtable.Rows)
                        {
                            TABLE14_CableView obj = new TABLE14_CableView();
                            obj.System_name = row["System_name"].ToString(); obj.Project = row["Project"].ToString();
                            obj.LC_Name = row["DisplayName"].ToString();
                            obj.Date = row["Date"].ToString(); obj.CreationDate = DateTime.Parse(row["CreationDate"].ToString()).AddMinutes(diff_UTCandLocal).ToLongTimeString();
                            obj.Kernel_Loadbox_Card_ECC_Cable_CABLE_DeviceRevision = row["Kernel_Loadbox_Card_ECC_Cable_CABLE_DeviceRevision"].ToString();
                            obj.Kernel_Loadbox_Card_ECC_Cable_CABLE_SerialNumber = row["Kernel_Loadbox_Card_ECC_Cable_CABLE_SerialNumber"].ToString();


                            table3DataViews.Add(obj);
                        }
                        //hwDataViews = hwDataViews.OrderByDescending(x => x.Date).GroupBy(x => new { x.EEPName, x.Date, x.System_name }).Select(x => x.FirstOrDefault()).OrderBy(x => x.Date).ToList();


                        var result = Json(new { data = table3DataViews }, JsonRequestBehavior.AllowGet); result.MaxJsonLength = int.MaxValue; return result;
                    }
                }
                catch (Exception Ex)
                {
                    return View("Unable to retrieve data from database. Please try again later!!");
                }
            }
            else
            {
                var result = Json(new { success = false, message = "Try again" }, JsonRequestBehavior.AllowGet); result.MaxJsonLength = int.MaxValue; return result;
            }

        }
        [HttpPost]
        public ActionResult GetTABLE16_ECCData_unique(List<string> pc, DateTime sdate, DateTime edate, int diff_UTCandLocal)
        {
            List<TABLE16_ECCView> table5DataViews = new List<TABLE16_ECCView>();
            if (pc != null && pc.Count() > 0)
            {
                string pclist = string.Empty;
                try
                {
                    using (PromasterImport_HWInfoEntities db = new PromasterImport_HWInfoEntities())
                    {
                        db.Database.CommandTimeout = 500;
                        string date = string.Empty;
                        foreach (var p in pc)
                        {
                            pclist += ("'" + p.Trim() + "',");
                            //pclist.Add(p.FQDN.Split('.')[0].ToUpper().Trim());
                        }

                        pclist = pclist.Remove(pclist.Length - 1, 1);
                        connection();
                        DataTable dtable = new DataTable();

                        string Query = " Select " + ConfigurationManager.AppSettings["BookingServerReplica"] + " [LabInfo].DisplayName, [System_name],Date = max(Date) , [Kernel_Loadbox_Card_ECC_AddOn_Present], [Kernel_Loadbox_Card_ECC_CARD_BoardType] ,[Kernel_Loadbox_Card_ECC_CARD_Description],[Kernel_Loadbox_Card_ECC_CARD_BoardRevision] ,[Kernel_Loadbox_Card_ECC_CARD_VER_TAG_ARRAY] ,[Kernel_Loadbox_Card_ECC_Serial_No],[Kernel_Loadbox_Card_ECC_RepairCounter],[Kernel_Loadbox_Card_ECC_Firmware_Card_Major] ,[Kernel_Loadbox_Card_ECC_Firmware_Card_Minor],[Kernel_Loadbox_Card_ECC_Firmware_FPGA_Major],[Kernel_Loadbox_Card_ECC_Firmware_FPGA_Minor] ,[Kernel_Loadbox_Card_ECC_Calibration_Day] ,[Kernel_Loadbox_Card_ECC_Calibration_Month] ,[Kernel_Loadbox_Card_ECC_Calibration_Year] ,[Kernel_Loadbox_Card_ECC_Next_Calibration_Day]  ,[Kernel_Loadbox_Card_ECC_Next_Calibration_Month],[Kernel_Loadbox_Card_ECC_Next_Calibration_Year] ,[Kernel_Loadbox_Card_ECC_Day_of_Creation] ,[Kernel_Loadbox_Card_ECC_Month_of_Creation],[Kernel_Loadbox_Card_ECC_Year_of_Creation] ,[Kernel_Loadbox_Card_ECC_BuildVersion] ,[Project],CreationDate = max([CreationDate])from [ECC_Card_Pr] inner join  " + ConfigurationManager.AppSettings["BookingServerReplica"] + "LabComputersPr on substring( " + ConfigurationManager.AppSettings["BookingServerReplica"] + "LabComputersPr.FQDN, 1, len(System_name)) = [ECC_Card_Pr].System_name inner join " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo] on " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo].id = LabComputersPr.LabId where System_name in (" + pclist + ") AND CONVERT(DATETIME, Date) > = '" + sdate + "' AND CONVERT(DATETIME, Date) < = '" + edate + "' GROUP BY   " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo].DisplayName ,  [System_name], [Kernel_Loadbox_Card_ECC_AddOn_Present], [Kernel_Loadbox_Card_ECC_CARD_BoardType] ,[Kernel_Loadbox_Card_ECC_CARD_Description],[Kernel_Loadbox_Card_ECC_CARD_BoardRevision] ,[Kernel_Loadbox_Card_ECC_CARD_VER_TAG_ARRAY] ,[Kernel_Loadbox_Card_ECC_Serial_No],[Kernel_Loadbox_Card_ECC_RepairCounter] ,[Kernel_Loadbox_Card_ECC_Firmware_Card_Major] ,[Kernel_Loadbox_Card_ECC_Firmware_Card_Minor],[Kernel_Loadbox_Card_ECC_Firmware_FPGA_Major],[Kernel_Loadbox_Card_ECC_Firmware_FPGA_Minor] ,[Kernel_Loadbox_Card_ECC_Calibration_Day] ,[Kernel_Loadbox_Card_ECC_Calibration_Month] ,[Kernel_Loadbox_Card_ECC_Calibration_Year] ,[Kernel_Loadbox_Card_ECC_Next_Calibration_Day]  ,[Kernel_Loadbox_Card_ECC_Next_Calibration_Month],[Kernel_Loadbox_Card_ECC_Next_Calibration_Year] ,[Kernel_Loadbox_Card_ECC_Day_of_Creation] ,[Kernel_Loadbox_Card_ECC_Month_of_Creation],[Kernel_Loadbox_Card_ECC_Year_of_Creation] ,[Kernel_Loadbox_Card_ECC_BuildVersion] ,[Project] order by CreationDate";

                        OpenConnection();
                        SqlCommand cmd = new SqlCommand(Query, con);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dtable);
                        CloseConnection();

                        foreach (DataRow row in dtable.Rows)
                        {
                            TABLE16_ECCView obj = new TABLE16_ECCView();
                            obj.System_name = row["System_name"].ToString(); obj.Project = row["Project"].ToString();
                            obj.LC_Name = row["DisplayName"].ToString();
                            obj.Date = row["Date"].ToString(); obj.CreationDate = DateTime.Parse(row["CreationDate"].ToString()).AddMinutes(diff_UTCandLocal).ToLongTimeString();

                            obj.Kernel_Loadbox_Card_ECC_AddOn_Present = row["Kernel_Loadbox_Card_ECC_AddOn_Present"].ToString();
                            obj.Kernel_Loadbox_Card_ECC_CARD_BoardType = row["Kernel_Loadbox_Card_ECC_CARD_BoardType"].ToString();
                            obj.Kernel_Loadbox_Card_ECC_CARD_BoardRevision = row["Kernel_Loadbox_Card_ECC_CARD_BoardRevision"].ToString();
                            obj.Kernel_Loadbox_Card_ECC_CARD_VER_TAG_ARRAY = row["Kernel_Loadbox_Card_ECC_CARD_VER_TAG_ARRAY"].ToString();
                            obj.Kernel_Loadbox_Card_ECC_Serial_No = row["Kernel_Loadbox_Card_ECC_Serial_No"].ToString();
                            obj.Kernel_Loadbox_Card_ECC_Firmware_Card_Major = row["Kernel_Loadbox_Card_ECC_Firmware_Card_Major"].ToString();
                            obj.Kernel_Loadbox_Card_ECC_Firmware_Card_Minor = row["Kernel_Loadbox_Card_ECC_Firmware_Card_Minor"].ToString();
                            obj.Kernel_Loadbox_Card_ECC_Firmware_FPGA_Major = row["Kernel_Loadbox_Card_ECC_Firmware_FPGA_Major"].ToString();

                            obj.Kernel_Loadbox_Card_ECC_Firmware_FPGA_Minor = row["Kernel_Loadbox_Card_ECC_Firmware_FPGA_Minor"].ToString();
                            obj.Kernel_Loadbox_Card_ECC_Calibration_Day = row["Kernel_Loadbox_Card_ECC_Calibration_Day"].ToString();
                            obj.Kernel_Loadbox_Card_ECC_Calibration_Month = row["Kernel_Loadbox_Card_ECC_Calibration_Month"].ToString();
                            obj.Kernel_Loadbox_Card_ECC_Calibration_Year = row["Kernel_Loadbox_Card_ECC_Calibration_Year"].ToString();

                            obj.Kernel_Loadbox_Card_ECC_Next_Calibration_Day = row["Kernel_Loadbox_Card_ECC_Next_Calibration_Day"].ToString();
                            obj.Kernel_Loadbox_Card_ECC_Next_Calibration_Month = row["Kernel_Loadbox_Card_ECC_Next_Calibration_Month"].ToString();
                            obj.Kernel_Loadbox_Card_ECC_Next_Calibration_Year = row["Kernel_Loadbox_Card_ECC_Next_Calibration_Year"].ToString();
                            obj.Kernel_Loadbox_Card_ECC_Day_of_Creation = row["Kernel_Loadbox_Card_ECC_Day_of_Creation"].ToString();
                            obj.Kernel_Loadbox_Card_ECC_Month_of_Creation = row["Kernel_Loadbox_Card_ECC_Month_of_Creation"].ToString();
                            obj.Kernel_Loadbox_Card_ECC_Year_of_Creation = row["Kernel_Loadbox_Card_ECC_Year_of_Creation"].ToString();
                            obj.Kernel_Loadbox_Card_ECC_BuildVersion = row["Kernel_Loadbox_Card_ECC_BuildVersion"].ToString();

                            table5DataViews.Add(obj);
                        }
                        //hwDataViews = hwDataViews.OrderByDescending(x => x.Date).GroupBy(x => new { x.EEPName, x.Date, x.System_name }).Select(x => x.FirstOrDefault()).OrderBy(x => x.Date).ToList();


                        var result = Json(new { data = table5DataViews }, JsonRequestBehavior.AllowGet); result.MaxJsonLength = int.MaxValue; return result;
                    }
                }
                catch (Exception Ex)
                {
                    return View("Unable to retrieve data from database. Please try again later!!");
                }
            }
            else
            {
                var result = Json(new { success = false, message = "Try again" }, JsonRequestBehavior.AllowGet); result.MaxJsonLength = int.MaxValue; return result;
            }
        }

        [HttpPost]
        public ActionResult GetTABLE17_GIO1Data_unique(List<string> pc, DateTime sdate, DateTime edate, int diff_UTCandLocal)
        {
            List<TABLE6_GIO1View> table6DataViews = new List<TABLE6_GIO1View>();
            if (pc != null && pc.Count() > 0)
            {
                string pclist = string.Empty;
                try
                {
                    using (PromasterImport_HWInfoEntities db = new PromasterImport_HWInfoEntities())
                    {
                        db.Database.CommandTimeout = 500;
                        string date = string.Empty;
                        foreach (var p in pc)
                        {
                            pclist += ("'" + p.Trim() + "',");
                            //pclist.Add(p.FQDN.Split('.')[0].ToUpper().Trim());
                        }

                        pclist = pclist.Remove(pclist.Length - 1, 1);
                        connection();
                        DataTable dtable = new DataTable();

                        string Query = " Select " + ConfigurationManager.AppSettings["BookingServerReplica"] + " [LabInfo].DisplayName, [System_name],Date = max(Date) ,[Kernel_Loadbox_Card_GIO1_CARD_BoardType] ,[Kernel_Loadbox_Card_GIO1_CARD_Description],[Kernel_Loadbox_Card_GIO1_CARD_BoardRevision] ,[Kernel_Loadbox_Card_GIO1_CARD_VER_TAG_ARRAY] ,[Kernel_Loadbox_Card_GIO1_Serial_No],[Kernel_Loadbox_Card_GIO1_RepairCounter],[Kernel_Loadbox_Card_GIO1_Firmware_Card_Major] ,[Kernel_Loadbox_Card_GIO1_Firmware_Card_Minor],[Kernel_Loadbox_Card_GIO1_Firmware_FPGA_Major],[Kernel_Loadbox_Card_GIO1_Firmware_FPGA_Minor] ,[Kernel_Loadbox_Card_GIO1_Calibration_Day] ,[Kernel_Loadbox_Card_GIO1_Calibration_Month] ,[Kernel_Loadbox_Card_GIO1_Calibration_Year] ,[Kernel_Loadbox_Card_GIO1_Next_Calibration_Day]  ,[Kernel_Loadbox_Card_GIO1_Next_Calibration_Month],[Kernel_Loadbox_Card_GIO1_Next_Calibration_Year] ,[Kernel_Loadbox_Card_GIO1_Day_of_Creation] ,[Kernel_Loadbox_Card_GIO1_Month_of_Creation],[Kernel_Loadbox_Card_GIO1_Year_of_Creation] ,[Kernel_Loadbox_Card_GIO1_BuildVersion] ,[Project],CreationDate = max([CreationDate])from [GIO1_Card_Pr] inner join  " + ConfigurationManager.AppSettings["BookingServerReplica"] + "LabComputersPr on substring( " + ConfigurationManager.AppSettings["BookingServerReplica"] + "LabComputersPr.FQDN, 1, len(System_name)) = [GIO1_Card_Pr].System_name inner join " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo] on " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo].id = LabComputersPr.LabId where System_name in (" + pclist + ") AND CONVERT(DATETIME, Date) > = '" + sdate + "' AND CONVERT(DATETIME, Date) < = '" + edate + "' GROUP BY   " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo].DisplayName , [System_name] ,[Kernel_Loadbox_Card_GIO1_CARD_BoardType] ,[Kernel_Loadbox_Card_GIO1_CARD_Description],[Kernel_Loadbox_Card_GIO1_CARD_BoardRevision] ,[Kernel_Loadbox_Card_GIO1_CARD_VER_TAG_ARRAY] ,[Kernel_Loadbox_Card_GIO1_Serial_No],[Kernel_Loadbox_Card_GIO1_RepairCounter] ,[Kernel_Loadbox_Card_GIO1_Firmware_Card_Major] ,[Kernel_Loadbox_Card_GIO1_Firmware_Card_Minor],[Kernel_Loadbox_Card_GIO1_Firmware_FPGA_Major],[Kernel_Loadbox_Card_GIO1_Firmware_FPGA_Minor] ,[Kernel_Loadbox_Card_GIO1_Calibration_Day] ,[Kernel_Loadbox_Card_GIO1_Calibration_Month] ,[Kernel_Loadbox_Card_GIO1_Calibration_Year] ,[Kernel_Loadbox_Card_GIO1_Next_Calibration_Day]  ,[Kernel_Loadbox_Card_GIO1_Next_Calibration_Month],[Kernel_Loadbox_Card_GIO1_Next_Calibration_Year] ,[Kernel_Loadbox_Card_GIO1_Day_of_Creation] ,[Kernel_Loadbox_Card_GIO1_Month_of_Creation],[Kernel_Loadbox_Card_GIO1_Year_of_Creation] ,[Kernel_Loadbox_Card_GIO1_BuildVersion],Project order by CreationDate";

                        OpenConnection();
                        SqlCommand cmd = new SqlCommand(Query, con);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dtable);
                        CloseConnection();

                        foreach (DataRow row in dtable.Rows)
                        {
                            TABLE6_GIO1View obj = new TABLE6_GIO1View();
                            obj.System_name = row["System_name"].ToString(); obj.Project = row["Project"].ToString();
                            obj.LC_Name = row["DisplayName"].ToString();
                            obj.Date = row["Date"].ToString(); obj.CreationDate = DateTime.Parse(row["CreationDate"].ToString()).AddMinutes(diff_UTCandLocal).ToLongTimeString();

                            obj.Kernel_Loadbox_Card_GIO1_CARD_BoardType = row["Kernel_Loadbox_Card_GIO1_CARD_BoardType"].ToString();
                            obj.Kernel_Loadbox_Card_GIO1_CARD_BoardRevision = row["Kernel_Loadbox_Card_GIO1_CARD_BoardRevision"].ToString();
                            obj.Kernel_Loadbox_Card_GIO1_CARD_VER_TAG_ARRAY = row["Kernel_Loadbox_Card_GIO1_CARD_VER_TAG_ARRAY"].ToString();
                            obj.Kernel_Loadbox_Card_GIO1_Serial_No = row["Kernel_Loadbox_Card_GIO1_Serial_No"].ToString();
                            obj.Kernel_Loadbox_Card_GIO1_Firmware_Card_Major = row["Kernel_Loadbox_Card_GIO1_Firmware_Card_Major"].ToString();
                            obj.Kernel_Loadbox_Card_GIO1_Firmware_Card_Minor = row["Kernel_Loadbox_Card_GIO1_Firmware_Card_Minor"].ToString();
                            obj.Kernel_Loadbox_Card_GIO1_Firmware_FPGA_Major = row["Kernel_Loadbox_Card_GIO1_Firmware_FPGA_Major"].ToString();
                            obj.Kernel_Loadbox_Card_GIO1_Firmware_FPGA_Minor = row["Kernel_Loadbox_Card_GIO1_Firmware_FPGA_Minor"].ToString();
                            obj.Kernel_Loadbox_Card_GIO1_Calibration_Day = row["Kernel_Loadbox_Card_GIO1_Calibration_Day"].ToString();
                            obj.Kernel_Loadbox_Card_GIO1_Calibration_Month = row["Kernel_Loadbox_Card_GIO1_Calibration_Month"].ToString();
                            obj.Kernel_Loadbox_Card_GIO1_Calibration_Year = row["Kernel_Loadbox_Card_GIO1_Calibration_Year"].ToString();
                            obj.Kernel_Loadbox_Card_GIO1_Next_Calibration_Day = row["Kernel_Loadbox_Card_GIO1_Next_Calibration_Day"].ToString();
                            obj.Kernel_Loadbox_Card_GIO1_Next_Calibration_Month = row["Kernel_Loadbox_Card_GIO1_Next_Calibration_Month"].ToString();
                            obj.Kernel_Loadbox_Card_GIO1_Next_Calibration_Year = row["Kernel_Loadbox_Card_GIO1_Next_Calibration_Year"].ToString();
                            obj.Kernel_Loadbox_Card_GIO1_Day_of_Creation = row["Kernel_Loadbox_Card_GIO1_Day_of_Creation"].ToString();
                            obj.Kernel_Loadbox_Card_GIO1_Month_of_Creation = row["Kernel_Loadbox_Card_GIO1_Month_of_Creation"].ToString();
                            obj.Kernel_Loadbox_Card_GIO1_Year_of_Creation = row["Kernel_Loadbox_Card_GIO1_Year_of_Creation"].ToString();
                            obj.Kernel_Loadbox_Card_GIO1_BuildVersion = row["Kernel_Loadbox_Card_GIO1_BuildVersion"].ToString();

                            table6DataViews.Add(obj);
                        }
                        //hwDataViews = hwDataViews.OrderByDescending(x => x.Date).GroupBy(x => new { x.EEPName, x.Date, x.System_name }).Select(x => x.FirstOrDefault()).OrderBy(x => x.Date).ToList();


                        var result = Json(new { data = table6DataViews }, JsonRequestBehavior.AllowGet); result.MaxJsonLength = int.MaxValue; return result;
                    }
                }
                catch (Exception Ex)
                {
                    return View("Unable to retrieve data from database. Please try again later!!");
                }
            }
            else
            {
                var result = Json(new { success = false, message = "Try again" }, JsonRequestBehavior.AllowGet); result.MaxJsonLength = int.MaxValue; return result;
            }

        }

        [HttpPost]
        public ActionResult GetTABLE18_GIO2Data_unique(List<string> pc, DateTime sdate, DateTime edate, int diff_UTCandLocal)
        {
            List<TABLE7_GIO2View> table7DataViews = new List<TABLE7_GIO2View>();
            if (pc != null && pc.Count() > 0)
            {
                string pclist = string.Empty;
                try
                {
                    using (PromasterImport_HWInfoEntities db = new PromasterImport_HWInfoEntities())
                    {
                        db.Database.CommandTimeout = 500;
                        string date = string.Empty;
                        foreach (var p in pc)
                        {
                            pclist += ("'" + p.Trim() + "',");
                            //pclist.Add(p.FQDN.Split('.')[0].ToUpper().Trim());
                        }

                        pclist = pclist.Remove(pclist.Length - 1, 1);
                        connection();
                        DataTable dtable = new DataTable();

                        string Query = " Select " + ConfigurationManager.AppSettings["BookingServerReplica"] + " [LabInfo].DisplayName, [System_name] , Date = max(Date),[Kernel_Loadbox_Card_GIO2_CARD_BoardType] ,[Kernel_Loadbox_Card_GIO2_CARD_Description],[Kernel_Loadbox_Card_GIO2_CARD_BoardRevision] ,[Kernel_Loadbox_Card_GIO2_CARD_VER_TAG_ARRAY] ,[Kernel_Loadbox_Card_GIO2_Serial_No],[Kernel_Loadbox_Card_GIO2_RepairCounter],[Kernel_Loadbox_Card_GIO2_Firmware_Card_Major] ,[Kernel_Loadbox_Card_GIO2_Firmware_Card_Minor],[Kernel_Loadbox_Card_GIO2_Firmware_FPGA_Major],[Kernel_Loadbox_Card_GIO2_Firmware_FPGA_Minor] ,[Kernel_Loadbox_Card_GIO2_Calibration_Day] ,[Kernel_Loadbox_Card_GIO2_Calibration_Month] ,[Kernel_Loadbox_Card_GIO2_Calibration_Year] ,[Kernel_Loadbox_Card_GIO2_Next_Calibration_Day]  ,[Kernel_Loadbox_Card_GIO2_Next_Calibration_Month],[Kernel_Loadbox_Card_GIO2_Next_Calibration_Year] ,[Kernel_Loadbox_Card_GIO2_Day_of_Creation] ,[Kernel_Loadbox_Card_GIO2_Month_of_Creation],[Kernel_Loadbox_Card_GIO2_Year_of_Creation] ,[Kernel_Loadbox_Card_GIO2_BuildVersion] ,[Project],CreationDate = max([CreationDate])from [GIO2_Card_Pr] inner join  " + ConfigurationManager.AppSettings["BookingServerReplica"] + "LabComputersPr on substring( " + ConfigurationManager.AppSettings["BookingServerReplica"] + "LabComputersPr.FQDN, 1, len(System_name)) = [GIO2_Card_Pr].System_name inner join " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo] on " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo].id = LabComputersPr.LabId where System_name in (" + pclist + ") AND CONVERT(DATETIME, Date) > = '" + sdate + "' AND CONVERT(DATETIME, Date) < = '" + edate + "' GROUP BY   " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo].DisplayName , [System_name] ,[Kernel_Loadbox_Card_GIO2_CARD_BoardType] ,[Kernel_Loadbox_Card_GIO2_CARD_Description],[Kernel_Loadbox_Card_GIO2_CARD_BoardRevision] ,[Kernel_Loadbox_Card_GIO2_CARD_VER_TAG_ARRAY] ,[Kernel_Loadbox_Card_GIO2_Serial_No],[Kernel_Loadbox_Card_GIO2_RepairCounter],[Kernel_Loadbox_Card_GIO2_Firmware_Card_Major] ,[Kernel_Loadbox_Card_GIO2_Firmware_Card_Minor],[Kernel_Loadbox_Card_GIO2_Firmware_FPGA_Major],[Kernel_Loadbox_Card_GIO2_Firmware_FPGA_Minor] ,[Kernel_Loadbox_Card_GIO2_Calibration_Day] ,[Kernel_Loadbox_Card_GIO2_Calibration_Month] ,[Kernel_Loadbox_Card_GIO2_Calibration_Year] ,[Kernel_Loadbox_Card_GIO2_Next_Calibration_Day]  ,[Kernel_Loadbox_Card_GIO2_Next_Calibration_Month],[Kernel_Loadbox_Card_GIO2_Next_Calibration_Year] ,[Kernel_Loadbox_Card_GIO2_Day_of_Creation] ,[Kernel_Loadbox_Card_GIO2_Month_of_Creation],[Kernel_Loadbox_Card_GIO2_Year_of_Creation] ,[Kernel_Loadbox_Card_GIO2_BuildVersion] ,[Project] order by CreationDate";

                        OpenConnection();
                        SqlCommand cmd = new SqlCommand(Query, con);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dtable);
                        CloseConnection();

                        foreach (DataRow row in dtable.Rows)
                        {
                            TABLE7_GIO2View obj = new TABLE7_GIO2View();
                            obj.System_name = row["System_name"].ToString(); obj.Project = row["Project"].ToString();
                            obj.LC_Name = row["DisplayName"].ToString();
                            obj.Date = row["Date"].ToString(); obj.CreationDate = DateTime.Parse(row["CreationDate"].ToString()).AddMinutes(diff_UTCandLocal).ToLongTimeString();

                            obj.Kernel_Loadbox_Card_GIO2_CARD_BoardType = row["Kernel_Loadbox_Card_GIO2_CARD_BoardType"].ToString();
                            obj.Kernel_Loadbox_Card_GIO2_CARD_BoardRevision = row["Kernel_Loadbox_Card_GIO2_CARD_BoardRevision"].ToString();
                            obj.Kernel_Loadbox_Card_GIO2_CARD_VER_TAG_ARRAY = row["Kernel_Loadbox_Card_GIO2_CARD_VER_TAG_ARRAY"].ToString();
                            obj.Kernel_Loadbox_Card_GIO2_Serial_No = row["Kernel_Loadbox_Card_GIO2_Serial_No"].ToString();
                            obj.Kernel_Loadbox_Card_GIO2_Firmware_Card_Major = row["Kernel_Loadbox_Card_GIO2_Firmware_Card_Major"].ToString();
                            obj.Kernel_Loadbox_Card_GIO2_Firmware_Card_Minor = row["Kernel_Loadbox_Card_GIO2_Firmware_Card_Minor"].ToString();
                            obj.Kernel_Loadbox_Card_GIO2_Firmware_FPGA_Major = row["Kernel_Loadbox_Card_GIO2_Firmware_FPGA_Major"].ToString();
                            obj.Kernel_Loadbox_Card_GIO2_Firmware_FPGA_Minor = row["Kernel_Loadbox_Card_GIO2_Firmware_FPGA_Minor"].ToString();
                            obj.Kernel_Loadbox_Card_GIO2_Calibration_Day = row["Kernel_Loadbox_Card_GIO2_Calibration_Day"].ToString();
                            obj.Kernel_Loadbox_Card_GIO2_Calibration_Month = row["Kernel_Loadbox_Card_GIO2_Calibration_Month"].ToString();
                            obj.Kernel_Loadbox_Card_GIO2_Calibration_Year = row["Kernel_Loadbox_Card_GIO2_Calibration_Year"].ToString();
                            obj.Kernel_Loadbox_Card_GIO2_Next_Calibration_Day = row["Kernel_Loadbox_Card_GIO2_Next_Calibration_Day"].ToString();
                            obj.Kernel_Loadbox_Card_GIO2_Next_Calibration_Month = row["Kernel_Loadbox_Card_GIO2_Next_Calibration_Month"].ToString();
                            obj.Kernel_Loadbox_Card_GIO2_Next_Calibration_Year = row["Kernel_Loadbox_Card_GIO2_Next_Calibration_Year"].ToString();
                            obj.Kernel_Loadbox_Card_GIO2_Day_of_Creation = row["Kernel_Loadbox_Card_GIO2_Day_of_Creation"].ToString();
                            obj.Kernel_Loadbox_Card_GIO2_Month_of_Creation = row["Kernel_Loadbox_Card_GIO2_Month_of_Creation"].ToString();
                            obj.Kernel_Loadbox_Card_GIO2_Year_of_Creation = row["Kernel_Loadbox_Card_GIO2_Year_of_Creation"].ToString();
                            obj.Kernel_Loadbox_Card_GIO2_BuildVersion = row["Kernel_Loadbox_Card_GIO2_BuildVersion"].ToString();

                            table7DataViews.Add(obj);
                        }
                        //hwDataViews = hwDataViews.OrderByDescending(x => x.Date).GroupBy(x => new { x.EEPName, x.Date, x.System_name }).Select(x => x.FirstOrDefault()).OrderBy(x => x.Date).ToList();


                        var result = Json(new { data = table7DataViews }, JsonRequestBehavior.AllowGet); result.MaxJsonLength = int.MaxValue; return result;
                    }
                }
                catch (Exception Ex)
                {
                    return View("Unable to retrieve data from database. Please try again later!!");
                }
            }
            else
            {
                var result = Json(new { success = false, message = "Try again" }, JsonRequestBehavior.AllowGet); result.MaxJsonLength = int.MaxValue; return result;
            }

        }

        [HttpPost]
        public ActionResult GetTABLE19_LDUData_unique(List<string> pc, DateTime sdate, DateTime edate, int diff_UTCandLocal)
        {
            List<TABLE8_LDUView> table8DataViews = new List<TABLE8_LDUView>();
            if (pc != null && pc.Count() > 0)
            {
                string pclist = string.Empty;
                try
                {
                    using (PromasterImport_HWInfoEntities db = new PromasterImport_HWInfoEntities())
                    {
                        db.Database.CommandTimeout = 500;
                        string date = string.Empty;
                        foreach (var p in pc)
                        {
                            pclist += ("'" + p.Trim() + "',");
                            //pclist.Add(p.FQDN.Split('.')[0].ToUpper().Trim());
                        }

                        pclist = pclist.Remove(pclist.Length - 1, 1);
                        connection();
                        DataTable dtable = new DataTable();

                        string Query = " Select " + ConfigurationManager.AppSettings["BookingServerReplica"] + " [LabInfo].DisplayName, [System_name],Date = max(Date),[Kernel_Loadbox_Card_LDU_CARD_BoardType] ,[Kernel_Loadbox_Card_LDU_CARD_Description],[Kernel_Loadbox_Card_LDU_CARD_BoardRevision] ,[Kernel_Loadbox_Card_LDU_CARD_VER_TAG_ARRAY] ,[Kernel_Loadbox_Card_LDU_Serial_No],[Kernel_Loadbox_Card_LDU_RepairCounter],[Kernel_Loadbox_Card_LDU_Firmware_Card_Major] ,[Kernel_Loadbox_Card_LDU_Firmware_Card_Minor],[Kernel_Loadbox_Card_LDU_Firmware_FPGA_Major],[Kernel_Loadbox_Card_LDU_Firmware_FPGA_Minor] ,[Kernel_Loadbox_Card_LDU_Calibration_Day] ,[Kernel_Loadbox_Card_LDU_Calibration_Month] ,[Kernel_Loadbox_Card_LDU_Calibration_Year] ,[Kernel_Loadbox_Card_LDU_Next_Calibration_Day]  ,[Kernel_Loadbox_Card_LDU_Next_Calibration_Month],[Kernel_Loadbox_Card_LDU_Next_Calibration_Year] ,[Kernel_Loadbox_Card_LDU_Day_of_Creation] ,[Kernel_Loadbox_Card_LDU_Month_of_Creation],[Kernel_Loadbox_Card_LDU_Year_of_Creation] ,[Kernel_Loadbox_Card_LDU_BuildVersion] ,[Project],CreationDate = max([CreationDate])from [LDU_Card_Pr] inner join  " + ConfigurationManager.AppSettings["BookingServerReplica"] + "LabComputersPr on substring( " + ConfigurationManager.AppSettings["BookingServerReplica"] + "LabComputersPr.FQDN, 1, len(System_name)) = [LDU_Card_Pr].System_name inner join " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo] on " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo].id = LabComputersPr.LabId where System_name in (" + pclist + ") AND CONVERT(DATETIME, Date) > = '" + sdate + "' AND CONVERT(DATETIME, Date) < = '" + edate + "' GROUP BY   " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo].DisplayName , [System_name] ,[Kernel_Loadbox_Card_LDU_CARD_BoardType] ,[Kernel_Loadbox_Card_LDU_CARD_Description],[Kernel_Loadbox_Card_LDU_CARD_BoardRevision] ,[Kernel_Loadbox_Card_LDU_CARD_VER_TAG_ARRAY] ,[Kernel_Loadbox_Card_LDU_Serial_No],[Kernel_Loadbox_Card_LDU_RepairCounter],[Kernel_Loadbox_Card_LDU_Firmware_Card_Major] ,[Kernel_Loadbox_Card_LDU_Firmware_Card_Minor],[Kernel_Loadbox_Card_LDU_Firmware_FPGA_Major],[Kernel_Loadbox_Card_LDU_Firmware_FPGA_Minor] ,[Kernel_Loadbox_Card_LDU_Calibration_Day] ,[Kernel_Loadbox_Card_LDU_Calibration_Month] ,[Kernel_Loadbox_Card_LDU_Calibration_Year] ,[Kernel_Loadbox_Card_LDU_Next_Calibration_Day]  ,[Kernel_Loadbox_Card_LDU_Next_Calibration_Month],[Kernel_Loadbox_Card_LDU_Next_Calibration_Year] ,[Kernel_Loadbox_Card_LDU_Day_of_Creation] ,[Kernel_Loadbox_Card_LDU_Month_of_Creation],[Kernel_Loadbox_Card_LDU_Year_of_Creation] ,[Kernel_Loadbox_Card_LDU_BuildVersion] ,[Project] order by CreationDate";
                        OpenConnection();
                        SqlCommand cmd = new SqlCommand(Query, con);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dtable);
                        CloseConnection();

                        foreach (DataRow row in dtable.Rows)
                        {
                            TABLE8_LDUView obj = new TABLE8_LDUView();
                            obj.System_name = row["System_name"].ToString(); obj.Project = row["Project"].ToString();
                            obj.LC_Name = row["DisplayName"].ToString();
                            obj.Date = row["Date"].ToString(); obj.CreationDate = DateTime.Parse(row["CreationDate"].ToString()).AddMinutes(diff_UTCandLocal).ToLongTimeString();

                            obj.Kernel_Loadbox_Card_LDU_CARD_BoardType = row["Kernel_Loadbox_Card_LDU_CARD_BoardType"].ToString();
                            obj.Kernel_Loadbox_Card_LDU_CARD_BoardRevision = row["Kernel_Loadbox_Card_LDU_CARD_BoardRevision"].ToString();
                            obj.Kernel_Loadbox_Card_LDU_CARD_VER_TAG_ARRAY = row["Kernel_Loadbox_Card_LDU_CARD_VER_TAG_ARRAY"].ToString();
                            obj.Kernel_Loadbox_Card_LDU_Serial_No = row["Kernel_Loadbox_Card_LDU_Serial_No"].ToString();
                            obj.Kernel_Loadbox_Card_LDU_Firmware_Card_Major = row["Kernel_Loadbox_Card_LDU_Firmware_Card_Major"].ToString();
                            obj.Kernel_Loadbox_Card_LDU_Firmware_Card_Minor = row["Kernel_Loadbox_Card_LDU_Firmware_Card_Minor"].ToString();
                            obj.Kernel_Loadbox_Card_LDU_Firmware_FPGA_Major = row["Kernel_Loadbox_Card_LDU_Firmware_FPGA_Major"].ToString();
                            obj.Kernel_Loadbox_Card_LDU_Firmware_FPGA_Minor = row["Kernel_Loadbox_Card_LDU_Firmware_FPGA_Minor"].ToString();
                            obj.Kernel_Loadbox_Card_LDU_Calibration_Day = row["Kernel_Loadbox_Card_LDU_Calibration_Day"].ToString();
                            obj.Kernel_Loadbox_Card_LDU_Calibration_Month = row["Kernel_Loadbox_Card_LDU_Calibration_Month"].ToString();
                            obj.Kernel_Loadbox_Card_LDU_Calibration_Year = row["Kernel_Loadbox_Card_LDU_Calibration_Year"].ToString();
                            obj.Kernel_Loadbox_Card_LDU_Next_Calibration_Day = row["Kernel_Loadbox_Card_LDU_Next_Calibration_Day"].ToString();
                            obj.Kernel_Loadbox_Card_LDU_Next_Calibration_Month = row["Kernel_Loadbox_Card_LDU_Next_Calibration_Month"].ToString();
                            obj.Kernel_Loadbox_Card_LDU_Next_Calibration_Year = row["Kernel_Loadbox_Card_LDU_Next_Calibration_Year"].ToString();
                            obj.Kernel_Loadbox_Card_LDU_Day_of_Creation = row["Kernel_Loadbox_Card_LDU_Day_of_Creation"].ToString();
                            obj.Kernel_Loadbox_Card_LDU_Month_of_Creation = row["Kernel_Loadbox_Card_LDU_Month_of_Creation"].ToString();
                            obj.Kernel_Loadbox_Card_LDU_Year_of_Creation = row["Kernel_Loadbox_Card_LDU_Year_of_Creation"].ToString();
                            obj.Kernel_Loadbox_Card_LDU_BuildVersion = row["Kernel_Loadbox_Card_LDU_BuildVersion"].ToString();

                            table8DataViews.Add(obj);
                        }
                        //hwDataViews = hwDataViews.OrderByDescending(x => x.Date).GroupBy(x => new { x.EEPName, x.Date, x.System_name }).Select(x => x.FirstOrDefault()).OrderBy(x => x.Date).ToList();


                        var result = Json(new { data = table8DataViews }, JsonRequestBehavior.AllowGet); result.MaxJsonLength = int.MaxValue; return result;
                    }
                }
                catch (Exception Ex)
                {
                    return View("Unable to retrieve data from database. Please try again later!!");
                }
            }
            else
            {
                var result = Json(new { success = false, message = "Try again" }, JsonRequestBehavior.AllowGet); result.MaxJsonLength = int.MaxValue; return result;
            }

        }

        [HttpPost]
        public ActionResult GetTABLE20_PSCData_unique(List<string> pc, DateTime sdate, DateTime edate, int diff_UTCandLocal)
        {
            List<TABLE9_PSCView> table9DataViews = new List<TABLE9_PSCView>();
            if (pc != null && pc.Count() > 0)
            {
                string pclist = string.Empty;
                try
                {
                    using (PromasterImport_HWInfoEntities db = new PromasterImport_HWInfoEntities())
                    {
                        db.Database.CommandTimeout = 500;
                        string date = string.Empty;
                        foreach (var p in pc)
                        {
                            pclist += ("'" + p.Trim() + "',");
                            //pclist.Add(p.FQDN.Split('.')[0].ToUpper().Trim());
                        }

                        pclist = pclist.Remove(pclist.Length - 1, 1);
                        connection();
                        DataTable dtable = new DataTable();

                        string Query = " Select " + ConfigurationManager.AppSettings["BookingServerReplica"] + " [LabInfo].DisplayName, [System_name],Date = max(Date) ,[Kernel_Loadbox_Card_PSC_CARD_BoardType] ,[Kernel_Loadbox_Card_PSC_CARD_Description],[Kernel_Loadbox_Card_PSC_CARD_BoardRevision] ,[Kernel_Loadbox_Card_PSC_CARD_VER_TAG_ARRAY] ,[Kernel_Loadbox_Card_PSC_Serial_No],[Kernel_Loadbox_Card_PSC_RepairCounter],[Kernel_Loadbox_Card_PSC_Firmware_Card_Major] ,[Kernel_Loadbox_Card_PSC_Firmware_Card_Minor],[Kernel_Loadbox_Card_PSC_Firmware_FPGA_Major],[Kernel_Loadbox_Card_PSC_Firmware_FPGA_Minor] ,[Kernel_Loadbox_Card_PSC_Calibration_Day] ,[Kernel_Loadbox_Card_PSC_Calibration_Month] ,[Kernel_Loadbox_Card_PSC_Calibration_Year] ,[Kernel_Loadbox_Card_PSC_Next_Calibration_Day]  ,[Kernel_Loadbox_Card_PSC_Next_Calibration_Month],[Kernel_Loadbox_Card_PSC_Next_Calibration_Year] ,[Kernel_Loadbox_Card_PSC_Day_of_Creation] ,[Kernel_Loadbox_Card_PSC_Month_of_Creation],[Kernel_Loadbox_Card_PSC_Year_of_Creation] ,[Kernel_Loadbox_Card_PSC_BuildVersion] ,[Project],CreationDate = max([CreationDate])from [PSC_Card_Pr] inner join  " + ConfigurationManager.AppSettings["BookingServerReplica"] + "LabComputersPr on substring( " + ConfigurationManager.AppSettings["BookingServerReplica"] + "LabComputersPr.FQDN, 1, len(System_name)) = [PSC_Card_Pr].System_name inner join " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo] on " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo].id = LabComputersPr.LabId where System_name in (" + pclist + ") AND CONVERT(DATETIME, Date) > = '" + sdate + "' AND CONVERT(DATETIME, Date) < = '" + edate + "' GROUP BY   " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo].DisplayName , [System_name] ,[Kernel_Loadbox_Card_PSC_CARD_BoardType] ,[Kernel_Loadbox_Card_PSC_CARD_Description],[Kernel_Loadbox_Card_PSC_CARD_BoardRevision] ,[Kernel_Loadbox_Card_PSC_CARD_VER_TAG_ARRAY] ,[Kernel_Loadbox_Card_PSC_Serial_No],[Kernel_Loadbox_Card_PSC_RepairCounter] ,[Kernel_Loadbox_Card_PSC_Firmware_Card_Major] ,[Kernel_Loadbox_Card_PSC_Firmware_Card_Minor],[Kernel_Loadbox_Card_PSC_Firmware_FPGA_Major],[Kernel_Loadbox_Card_PSC_Firmware_FPGA_Minor] ,[Kernel_Loadbox_Card_PSC_Calibration_Day] ,[Kernel_Loadbox_Card_PSC_Calibration_Month] ,[Kernel_Loadbox_Card_PSC_Calibration_Year] ,[Kernel_Loadbox_Card_PSC_Next_Calibration_Day]  ,[Kernel_Loadbox_Card_PSC_Next_Calibration_Month],[Kernel_Loadbox_Card_PSC_Next_Calibration_Year] ,[Kernel_Loadbox_Card_PSC_Day_of_Creation] ,[Kernel_Loadbox_Card_PSC_Month_of_Creation],[Kernel_Loadbox_Card_PSC_Year_of_Creation] ,[Kernel_Loadbox_Card_PSC_BuildVersion] ,[Project] order by CreationDate";

                        OpenConnection();
                        SqlCommand cmd = new SqlCommand(Query, con);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dtable);
                        CloseConnection();

                        foreach (DataRow row in dtable.Rows)
                        {
                            TABLE9_PSCView obj = new TABLE9_PSCView();
                            obj.System_name = row["System_name"].ToString(); obj.Project = row["Project"].ToString();
                            obj.LC_Name = row["DisplayName"].ToString();
                            obj.Date = row["Date"].ToString(); obj.CreationDate = DateTime.Parse(row["CreationDate"].ToString()).AddMinutes(diff_UTCandLocal).ToLongTimeString();

                            obj.Kernel_Loadbox_Card_PSC_CARD_BoardType = row["Kernel_Loadbox_Card_PSC_CARD_BoardType"].ToString();
                            obj.Kernel_Loadbox_Card_PSC_CARD_BoardRevision = row["Kernel_Loadbox_Card_PSC_CARD_BoardRevision"].ToString();
                            obj.Kernel_Loadbox_Card_PSC_CARD_VER_TAG_ARRAY = row["Kernel_Loadbox_Card_PSC_CARD_VER_TAG_ARRAY"].ToString();
                            obj.Kernel_Loadbox_Card_PSC_Serial_No = row["Kernel_Loadbox_Card_PSC_Serial_No"].ToString();
                            obj.Kernel_Loadbox_Card_PSC_Firmware_Card_Major = row["Kernel_Loadbox_Card_PSC_Firmware_Card_Major"].ToString();
                            obj.Kernel_Loadbox_Card_PSC_Firmware_Card_Minor = row["Kernel_Loadbox_Card_PSC_Firmware_Card_Minor"].ToString();
                            obj.Kernel_Loadbox_Card_PSC_Firmware_FPGA_Major = row["Kernel_Loadbox_Card_PSC_Firmware_FPGA_Major"].ToString();
                            obj.Kernel_Loadbox_Card_PSC_Firmware_FPGA_Minor = row["Kernel_Loadbox_Card_PSC_Firmware_FPGA_Minor"].ToString();
                            obj.Kernel_Loadbox_Card_PSC_Calibration_Day = row["Kernel_Loadbox_Card_PSC_Calibration_Day"].ToString();
                            obj.Kernel_Loadbox_Card_PSC_Calibration_Month = row["Kernel_Loadbox_Card_PSC_Calibration_Month"].ToString();
                            obj.Kernel_Loadbox_Card_PSC_Calibration_Year = row["Kernel_Loadbox_Card_PSC_Calibration_Year"].ToString();
                            obj.Kernel_Loadbox_Card_PSC_Next_Calibration_Day = row["Kernel_Loadbox_Card_PSC_Next_Calibration_Day"].ToString();
                            obj.Kernel_Loadbox_Card_PSC_Next_Calibration_Month = row["Kernel_Loadbox_Card_PSC_Next_Calibration_Month"].ToString();
                            obj.Kernel_Loadbox_Card_PSC_Next_Calibration_Year = row["Kernel_Loadbox_Card_PSC_Next_Calibration_Year"].ToString();
                            obj.Kernel_Loadbox_Card_PSC_Day_of_Creation = row["Kernel_Loadbox_Card_PSC_Day_of_Creation"].ToString();
                            obj.Kernel_Loadbox_Card_PSC_Month_of_Creation = row["Kernel_Loadbox_Card_PSC_Month_of_Creation"].ToString();
                            obj.Kernel_Loadbox_Card_PSC_Year_of_Creation = row["Kernel_Loadbox_Card_PSC_Year_of_Creation"].ToString();
                            obj.Kernel_Loadbox_Card_PSC_BuildVersion = row["Kernel_Loadbox_Card_PSC_BuildVersion"].ToString();

                            table9DataViews.Add(obj);
                        }
                        //hwDataViews = hwDataViews.OrderByDescending(x => x.Date).GroupBy(x => new { x.EEPName, x.Date, x.System_name }).Select(x => x.FirstOrDefault()).OrderBy(x => x.Date).ToList();


                        var result = Json(new { data = table9DataViews }, JsonRequestBehavior.AllowGet); result.MaxJsonLength = int.MaxValue; return result;
                    }
                }
                catch (Exception Ex)
                {
                    return View("Unable to retrieve data from database. Please try again later!!");
                }
            }
            else
            {
                var result = Json(new { success = false, message = "Try again" }, JsonRequestBehavior.AllowGet); result.MaxJsonLength = int.MaxValue; return result;
            }

        }

        [HttpPost]
        public ActionResult GetTABLE21_VSCData_unique(List<string> pc, DateTime sdate, DateTime edate, int diff_UTCandLocal)
        {
            List<TABLE10_VSCView> table10DataViews = new List<TABLE10_VSCView>();
            if (pc != null && pc.Count() > 0)
            {
                string pclist = string.Empty;
                try
                {
                    using (PromasterImport_HWInfoEntities db = new PromasterImport_HWInfoEntities())
                    {
                        db.Database.CommandTimeout = 500;
                        string date = string.Empty;
                        foreach (var p in pc)
                        {
                            pclist += ("'" + p.Trim() + "',");
                            //pclist.Add(p.FQDN.Split('.')[0].ToUpper().Trim());
                        }

                        pclist = pclist.Remove(pclist.Length - 1, 1);
                        connection();
                        DataTable dtable = new DataTable();

                        string Query = " Select " + ConfigurationManager.AppSettings["BookingServerReplica"] + " [LabInfo].DisplayName, [System_name],Date = max(Date), [Kernel_Loadbox_Card_VSC_UVRAddOnPresent] , [Kernel_Loadbox_Card_VSC_CARD_BoardType] ,[Kernel_Loadbox_Card_VSC_CARD_Description],[Kernel_Loadbox_Card_VSC_CARD_BoardRevision] ,[Kernel_Loadbox_Card_VSC_CARD_VER_TAG_ARRAY] ,[Kernel_Loadbox_Card_VSC_Serial_No],[Kernel_Loadbox_Card_VSC_RepairCounter] ,[Kernel_Loadbox_Card_VSC_Firmware_Card_Major] ,[Kernel_Loadbox_Card_VSC_Firmware_Card_Minor],[Kernel_Loadbox_Card_VSC_Firmware_FPGA_Major],[Kernel_Loadbox_Card_VSC_Firmware_FPGA_Minor] ,[Kernel_Loadbox_Card_VSC_Calibration_Day] ,[Kernel_Loadbox_Card_VSC_Calibration_Month] ,[Kernel_Loadbox_Card_VSC_Calibration_Year] ,[Kernel_Loadbox_Card_VSC_Next_Calibration_Day]  ,[Kernel_Loadbox_Card_VSC_Next_Calibration_Month],[Kernel_Loadbox_Card_VSC_Next_Calibration_Year] ,[Kernel_Loadbox_Card_VSC_Day_of_Creation] ,[Kernel_Loadbox_Card_VSC_Month_of_Creation],[Kernel_Loadbox_Card_VSC_Year_of_Creation] ,[Kernel_Loadbox_Card_VSC_BuildVersion] ,[Project],CreationDate = max([CreationDate])from [VSC_Card_Pr] inner join  " + ConfigurationManager.AppSettings["BookingServerReplica"] + "LabComputersPr on substring( " + ConfigurationManager.AppSettings["BookingServerReplica"] + "LabComputersPr.FQDN, 1, len(System_name)) = [VSC_Card_Pr].System_name inner join " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo] on " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo].id = LabComputersPr.LabId where System_name in (" + pclist + ") AND CONVERT(DATETIME, Date) > = '" + sdate + "' AND CONVERT(DATETIME, Date) < = '" + edate + "' GROUP BY   " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo].DisplayName , [System_name], [Kernel_Loadbox_Card_VSC_UVRAddOnPresent], [Kernel_Loadbox_Card_VSC_CARD_BoardType] ,[Kernel_Loadbox_Card_VSC_CARD_Description],[Kernel_Loadbox_Card_VSC_CARD_BoardRevision] ,[Kernel_Loadbox_Card_VSC_CARD_VER_TAG_ARRAY] ,[Kernel_Loadbox_Card_VSC_Serial_No],[Kernel_Loadbox_Card_VSC_RepairCounter],[Kernel_Loadbox_Card_VSC_Firmware_Card_Major] ,[Kernel_Loadbox_Card_VSC_Firmware_Card_Minor],[Kernel_Loadbox_Card_VSC_Firmware_FPGA_Major],[Kernel_Loadbox_Card_VSC_Firmware_FPGA_Minor] ,[Kernel_Loadbox_Card_VSC_Calibration_Day] ,[Kernel_Loadbox_Card_VSC_Calibration_Month] ,[Kernel_Loadbox_Card_VSC_Calibration_Year] ,[Kernel_Loadbox_Card_VSC_Next_Calibration_Day]  ,[Kernel_Loadbox_Card_VSC_Next_Calibration_Month],[Kernel_Loadbox_Card_VSC_Next_Calibration_Year] ,[Kernel_Loadbox_Card_VSC_Day_of_Creation] ,[Kernel_Loadbox_Card_VSC_Month_of_Creation],[Kernel_Loadbox_Card_VSC_Year_of_Creation] ,[Kernel_Loadbox_Card_VSC_BuildVersion] ,[Project] order by CreationDate";

                        OpenConnection();
                        SqlCommand cmd = new SqlCommand(Query, con);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dtable);
                        CloseConnection();

                        foreach (DataRow row in dtable.Rows)
                        {
                            TABLE10_VSCView obj = new TABLE10_VSCView();
                            obj.System_name = row["System_name"].ToString(); obj.Project = row["Project"].ToString();
                            obj.LC_Name = row["DisplayName"].ToString();
                            obj.Date = row["Date"].ToString(); obj.CreationDate = DateTime.Parse(row["CreationDate"].ToString()).AddMinutes(diff_UTCandLocal).ToLongTimeString();

                            obj.Kernel_Loadbox_Card_VSC_UVRAddOnPresent = row["Kernel_Loadbox_Card_VSC_UVRAddOnPresent"].ToString();
                            obj.Kernel_Loadbox_Card_VSC_CARD_BoardType = row["Kernel_Loadbox_Card_VSC_CARD_BoardType"].ToString();
                            obj.Kernel_Loadbox_Card_VSC_CARD_BoardRevision = row["Kernel_Loadbox_Card_VSC_CARD_BoardRevision"].ToString();
                            obj.Kernel_Loadbox_Card_VSC_CARD_VER_TAG_ARRAY = row["Kernel_Loadbox_Card_VSC_CARD_VER_TAG_ARRAY"].ToString();
                            obj.Kernel_Loadbox_Card_VSC_Serial_No = row["Kernel_Loadbox_Card_VSC_Serial_No"].ToString();
                            obj.Kernel_Loadbox_Card_VSC_Firmware_Card_Major = row["Kernel_Loadbox_Card_VSC_Firmware_Card_Major"].ToString();
                            obj.Kernel_Loadbox_Card_VSC_Firmware_Card_Minor = row["Kernel_Loadbox_Card_VSC_Firmware_Card_Minor"].ToString();
                            obj.Kernel_Loadbox_Card_VSC_Firmware_FPGA_Major = row["Kernel_Loadbox_Card_VSC_Firmware_FPGA_Major"].ToString();
                            obj.Kernel_Loadbox_Card_VSC_Firmware_FPGA_Minor = row["Kernel_Loadbox_Card_VSC_Firmware_FPGA_Minor"].ToString();
                            obj.Kernel_Loadbox_Card_VSC_Calibration_Day = row["Kernel_Loadbox_Card_VSC_Calibration_Day"].ToString();
                            obj.Kernel_Loadbox_Card_VSC_Calibration_Month = row["Kernel_Loadbox_Card_VSC_Calibration_Month"].ToString();
                            obj.Kernel_Loadbox_Card_VSC_Calibration_Year = row["Kernel_Loadbox_Card_VSC_Calibration_Year"].ToString();
                            obj.Kernel_Loadbox_Card_VSC_Next_Calibration_Day = row["Kernel_Loadbox_Card_VSC_Next_Calibration_Day"].ToString();
                            obj.Kernel_Loadbox_Card_VSC_Next_Calibration_Month = row["Kernel_Loadbox_Card_VSC_Next_Calibration_Month"].ToString();
                            obj.Kernel_Loadbox_Card_VSC_Next_Calibration_Year = row["Kernel_Loadbox_Card_VSC_Next_Calibration_Year"].ToString();
                            obj.Kernel_Loadbox_Card_VSC_Day_of_Creation = row["Kernel_Loadbox_Card_VSC_Day_of_Creation"].ToString();
                            obj.Kernel_Loadbox_Card_VSC_Month_of_Creation = row["Kernel_Loadbox_Card_VSC_Month_of_Creation"].ToString();
                            obj.Kernel_Loadbox_Card_VSC_Year_of_Creation = row["Kernel_Loadbox_Card_VSC_Year_of_Creation"].ToString();
                            obj.Kernel_Loadbox_Card_VSC_BuildVersion = row["Kernel_Loadbox_Card_VSC_BuildVersion"].ToString();

                            table10DataViews.Add(obj);
                        }
                        //hwDataViews = hwDataViews.OrderByDescending(x => x.Date).GroupBy(x => new { x.EEPName, x.Date, x.System_name }).Select(x => x.FirstOrDefault()).OrderBy(x => x.Date).ToList();


                        var result = Json(new { data = table10DataViews }, JsonRequestBehavior.AllowGet); result.MaxJsonLength = int.MaxValue; return result;
                    }
                }
                catch (Exception Ex)
                {
                    return View("Unable to retrieve data from database. Please try again later!!");
                }
            }
            else
            {
                var result = Json(new { success = false, message = "Try again" }, JsonRequestBehavior.AllowGet); result.MaxJsonLength = int.MaxValue; return result;
            }
        }

        [HttpPost]
        public ActionResult GetTABLE22_WSSData_unique(List<string> pc, DateTime sdate, DateTime edate, int diff_UTCandLocal)
        {
            List<TABLE11_WSSView> table11DataViews = new List<TABLE11_WSSView>();
            if (pc != null && pc.Count() > 0)
            {
                string pclist = string.Empty;
                try
                {
                    using (PromasterImport_HWInfoEntities db = new PromasterImport_HWInfoEntities())
                    {
                        db.Database.CommandTimeout = 500;
                        string date = string.Empty;
                        foreach (var p in pc)
                        {
                            pclist += ("'" + p.Trim() + "',");
                            //pclist.Add(p.FQDN.Split('.')[0].ToUpper().Trim());
                        }

                        pclist = pclist.Remove(pclist.Length - 1, 1);
                        connection();
                        DataTable dtable = new DataTable();

                        string Query = " Select " + ConfigurationManager.AppSettings["BookingServerReplica"] + " [LabInfo].DisplayName, [System_name],Date = max(Date) ,[Kernel_Loadbox_Card_WSS_CARD_BoardType] ,[Kernel_Loadbox_Card_WSS_CARD_Description],[Kernel_Loadbox_Card_WSS_CARD_BoardRevision] ,[Kernel_Loadbox_Card_WSS_CARD_VER_TAG_ARRAY] ,[Kernel_Loadbox_Card_WSS_Serial_No],[Kernel_Loadbox_Card_WSS_RepairCounter] ,[Kernel_Loadbox_Card_WSS_Firmware_Card_Major] ,[Kernel_Loadbox_Card_WSS_Firmware_Card_Minor],[Kernel_Loadbox_Card_WSS_Firmware_FPGA_Major],[Kernel_Loadbox_Card_WSS_Firmware_FPGA_Minor] ,[Kernel_Loadbox_Card_WSS_Calibration_Day] ,[Kernel_Loadbox_Card_WSS_Calibration_Month] ,[Kernel_Loadbox_Card_WSS_Calibration_Year] ,[Kernel_Loadbox_Card_WSS_Next_Calibration_Day]  ,[Kernel_Loadbox_Card_WSS_Next_Calibration_Month],[Kernel_Loadbox_Card_WSS_Next_Calibration_Year] ,[Kernel_Loadbox_Card_WSS_Day_of_Creation] ,[Kernel_Loadbox_Card_WSS_Month_of_Creation],[Kernel_Loadbox_Card_WSS_Year_of_Creation] ,[Kernel_Loadbox_Card_WSS_BuildVersion] ,[Project],CreationDate = max([CreationDate])from [WSS_Card_Pr] inner join  " + ConfigurationManager.AppSettings["BookingServerReplica"] + "LabComputersPr on substring( " + ConfigurationManager.AppSettings["BookingServerReplica"] + "LabComputersPr.FQDN, 1, len(System_name)) = [WSS_Card_Pr].System_name inner join " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo] on " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo].id = LabComputersPr.LabId where System_name in (" + pclist + ") AND CONVERT(DATETIME, Date) > = '" + sdate + "' AND CONVERT(DATETIME, Date) < = '" + edate + "' GROUP BY   " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo].DisplayName , [System_name] ,[Kernel_Loadbox_Card_WSS_CARD_BoardType] ,[Kernel_Loadbox_Card_WSS_CARD_Description],[Kernel_Loadbox_Card_WSS_CARD_BoardRevision] ,[Kernel_Loadbox_Card_WSS_CARD_VER_TAG_ARRAY] ,[Kernel_Loadbox_Card_WSS_Serial_No],[Kernel_Loadbox_Card_WSS_RepairCounter] ,[Kernel_Loadbox_Card_WSS_Firmware_Card_Major] ,[Kernel_Loadbox_Card_WSS_Firmware_Card_Minor],[Kernel_Loadbox_Card_WSS_Firmware_FPGA_Major],[Kernel_Loadbox_Card_WSS_Firmware_FPGA_Minor] ,[Kernel_Loadbox_Card_WSS_Calibration_Day] ,[Kernel_Loadbox_Card_WSS_Calibration_Month] ,[Kernel_Loadbox_Card_WSS_Calibration_Year] ,[Kernel_Loadbox_Card_WSS_Next_Calibration_Day]  ,[Kernel_Loadbox_Card_WSS_Next_Calibration_Month],[Kernel_Loadbox_Card_WSS_Next_Calibration_Year] ,[Kernel_Loadbox_Card_WSS_Day_of_Creation] ,[Kernel_Loadbox_Card_WSS_Month_of_Creation],[Kernel_Loadbox_Card_WSS_Year_of_Creation] ,[Kernel_Loadbox_Card_WSS_BuildVersion] ,[Project] order by CreationDate";
                        OpenConnection();
                        SqlCommand cmd = new SqlCommand(Query, con);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dtable);
                        CloseConnection();

                        foreach (DataRow row in dtable.Rows)
                        {
                            TABLE11_WSSView obj = new TABLE11_WSSView();

                            obj.System_name = row["System_name"].ToString(); obj.Project = row["Project"].ToString();
                            obj.LC_Name = row["DisplayName"].ToString();
                            obj.Date = row["Date"].ToString(); obj.CreationDate = DateTime.Parse(row["CreationDate"].ToString()).AddMinutes(diff_UTCandLocal).ToLongTimeString();

                            obj.Kernel_Loadbox_Card_WSS_CARD_BoardType = row["Kernel_Loadbox_Card_WSS_CARD_BoardType"].ToString();
                            obj.Kernel_Loadbox_Card_WSS_CARD_BoardRevision = row["Kernel_Loadbox_Card_WSS_CARD_BoardRevision"].ToString();
                            obj.Kernel_Loadbox_Card_WSS_CARD_VER_TAG_ARRAY = row["Kernel_Loadbox_Card_WSS_CARD_VER_TAG_ARRAY"].ToString();
                            obj.Kernel_Loadbox_Card_WSS_Serial_No = row["Kernel_Loadbox_Card_WSS_Serial_No"].ToString();
                            obj.Kernel_Loadbox_Card_WSS_Firmware_Card_Major = row["Kernel_Loadbox_Card_WSS_Firmware_Card_Major"].ToString();
                            obj.Kernel_Loadbox_Card_WSS_Firmware_Card_Minor = row["Kernel_Loadbox_Card_WSS_Firmware_Card_Minor"].ToString();
                            obj.Kernel_Loadbox_Card_WSS_Firmware_FPGA_Major = row["Kernel_Loadbox_Card_WSS_Firmware_FPGA_Major"].ToString();
                            obj.Kernel_Loadbox_Card_WSS_Firmware_FPGA_Minor = row["Kernel_Loadbox_Card_WSS_Firmware_FPGA_Minor"].ToString();
                            obj.Kernel_Loadbox_Card_WSS_Calibration_Day = row["Kernel_Loadbox_Card_WSS_Calibration_Day"].ToString();
                            obj.Kernel_Loadbox_Card_WSS_Calibration_Month = row["Kernel_Loadbox_Card_WSS_Calibration_Month"].ToString();
                            obj.Kernel_Loadbox_Card_WSS_Calibration_Year = row["Kernel_Loadbox_Card_WSS_Calibration_Year"].ToString();
                            obj.Kernel_Loadbox_Card_WSS_Next_Calibration_Day = row["Kernel_Loadbox_Card_WSS_Next_Calibration_Day"].ToString();
                            obj.Kernel_Loadbox_Card_WSS_Next_Calibration_Month = row["Kernel_Loadbox_Card_WSS_Next_Calibration_Month"].ToString();
                            obj.Kernel_Loadbox_Card_WSS_Next_Calibration_Year = row["Kernel_Loadbox_Card_WSS_Next_Calibration_Year"].ToString();
                            obj.Kernel_Loadbox_Card_WSS_Day_of_Creation = row["Kernel_Loadbox_Card_WSS_Day_of_Creation"].ToString();
                            obj.Kernel_Loadbox_Card_WSS_Month_of_Creation = row["Kernel_Loadbox_Card_WSS_Month_of_Creation"].ToString();
                            obj.Kernel_Loadbox_Card_WSS_Year_of_Creation = row["Kernel_Loadbox_Card_WSS_Year_of_Creation"].ToString();
                            obj.Kernel_Loadbox_Card_WSS_BuildVersion = row["Kernel_Loadbox_Card_WSS_BuildVersion"].ToString();
                            table11DataViews.Add(obj);

                            //hwDataViews = hwDataViews.OrderByDescending(x => x.Date).GroupBy(x => new { x.EEPName, x.Date, x.System_name }).Select(x => x.FirstOrDefault()).OrderBy(x => x.Date).ToList();



                        }
                        var result = Json(new { data = table11DataViews }, JsonRequestBehavior.AllowGet); result.MaxJsonLength = int.MaxValue; return result;
                    }
                }
                catch (Exception Ex)
                {
                    return View("Unable to retrieve data from database. Please try again later!!");
                }
            }
            else
            {
                var result = Json(new { success = false, message = "Try again" }, JsonRequestBehavior.AllowGet); result.MaxJsonLength = int.MaxValue; return result;
            }
        }

        [HttpPost]
        public ActionResult GetTABLE23_WSS2Data_unique(List<string> pc, DateTime sdate, DateTime edate, int diff_UTCandLocal)
        {
            List<TABLE12_WSS2View> table12DataViews = new List<TABLE12_WSS2View>();
            if (pc != null && pc.Count() > 0)
            {
                string pclist = string.Empty;
                try
                {
                    using (PromasterImport_HWInfoEntities db = new PromasterImport_HWInfoEntities())
                    {
                        db.Database.CommandTimeout = 500;
                        string date = string.Empty;
                        foreach (var p in pc)
                        {
                            pclist += ("'" + p.Trim() + "',");
                            //pclist.Add(p.FQDN.Split('.')[0].ToUpper().Trim());
                        }

                        pclist = pclist.Remove(pclist.Length - 1, 1);
                        connection();
                        DataTable dtable = new DataTable();

                        string Query = " Select " + ConfigurationManager.AppSettings["BookingServerReplica"] + " [LabInfo].DisplayName, [System_name], Date = max(Date),[Kernel_Loadbox_Card_WSS2_CARD_BoardType] ,[Kernel_Loadbox_Card_WSS2_CARD_Description],[Kernel_Loadbox_Card_WSS2_CARD_BoardRevision] ,[Kernel_Loadbox_Card_WSS2_CARD_VER_TAG_ARRAY] ,[Kernel_Loadbox_Card_WSS2_Serial_No],[Kernel_Loadbox_Card_WSS2_RepairCounter] ,[Kernel_Loadbox_Card_WSS2_Firmware_Card_Major] ,[Kernel_Loadbox_Card_WSS2_Firmware_Card_Minor],[Kernel_Loadbox_Card_WSS2_Firmware_FPGA_Major],[Kernel_Loadbox_Card_WSS2_Firmware_FPGA_Minor] ,[Kernel_Loadbox_Card_WSS2_Calibration_Day] ,[Kernel_Loadbox_Card_WSS2_Calibration_Month] ,[Kernel_Loadbox_Card_WSS2_Calibration_Year] ,[Kernel_Loadbox_Card_WSS2_Next_Calibration_Day]  ,[Kernel_Loadbox_Card_WSS2_Next_Calibration_Month],[Kernel_Loadbox_Card_WSS2_Next_Calibration_Year] ,[Kernel_Loadbox_Card_WSS2_Day_of_Creation] ,[Kernel_Loadbox_Card_WSS2_Month_of_Creation],[Kernel_Loadbox_Card_WSS2_Year_of_Creation] ,[Kernel_Loadbox_Card_WSS2_BuildVersion] ,[Project],CreationDate = max([CreationDate])from [WSS2_Card_Pr] inner join  " + ConfigurationManager.AppSettings["BookingServerReplica"] + "LabComputersPr on substring( " + ConfigurationManager.AppSettings["BookingServerReplica"] + "LabComputersPr.FQDN, 1, len(System_name)) = [WSS2_Card_Pr].System_name inner join " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo] on " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo].id = LabComputersPr.LabId where System_name in (" + pclist + ") AND CONVERT(DATETIME, Date) > = '" + sdate + "' AND CONVERT(DATETIME, Date) < = '" + edate + "' GROUP BY   " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo].DisplayName , [System_name] ,[Kernel_Loadbox_Card_WSS2_CARD_BoardType] ,[Kernel_Loadbox_Card_WSS2_CARD_Description],[Kernel_Loadbox_Card_WSS2_CARD_BoardRevision] ,[Kernel_Loadbox_Card_WSS2_CARD_VER_TAG_ARRAY] ,[Kernel_Loadbox_Card_WSS2_Serial_No],[Kernel_Loadbox_Card_WSS2_RepairCounter] ,[Kernel_Loadbox_Card_WSS2_Firmware_Card_Major] ,[Kernel_Loadbox_Card_WSS2_Firmware_Card_Minor],[Kernel_Loadbox_Card_WSS2_Firmware_FPGA_Major],[Kernel_Loadbox_Card_WSS2_Firmware_FPGA_Minor] ,[Kernel_Loadbox_Card_WSS2_Calibration_Day] ,[Kernel_Loadbox_Card_WSS2_Calibration_Month] ,[Kernel_Loadbox_Card_WSS2_Calibration_Year] ,[Kernel_Loadbox_Card_WSS2_Next_Calibration_Day]  ,[Kernel_Loadbox_Card_WSS2_Next_Calibration_Month],[Kernel_Loadbox_Card_WSS2_Next_Calibration_Year] ,[Kernel_Loadbox_Card_WSS2_Day_of_Creation] ,[Kernel_Loadbox_Card_WSS2_Month_of_Creation],[Kernel_Loadbox_Card_WSS2_Year_of_Creation] ,[Kernel_Loadbox_Card_WSS2_BuildVersion] ,[Project] order by CreationDate";

                        OpenConnection();
                        SqlCommand cmd = new SqlCommand(Query, con);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dtable);
                        CloseConnection();

                        foreach (DataRow row in dtable.Rows)
                        {
                            TABLE12_WSS2View obj = new TABLE12_WSS2View();
                            obj.System_name = row["System_name"].ToString(); obj.Project = row["Project"].ToString();
                            obj.LC_Name = row["DisplayName"].ToString();
                            obj.Date = row["Date"].ToString();
                            obj.CreationDate = DateTime.Parse(row["CreationDate"].ToString()).AddMinutes(diff_UTCandLocal).ToLongTimeString();
                            obj.Kernel_Loadbox_Card_WSS2_CARD_BoardType = row["Kernel_Loadbox_Card_WSS2_CARD_BoardType"].ToString();
                            obj.Kernel_Loadbox_Card_WSS2_CARD_BoardRevision = row["Kernel_Loadbox_Card_WSS2_CARD_BoardRevision"].ToString();
                            obj.Kernel_Loadbox_Card_WSS2_CARD_VER_TAG_ARRAY = row["Kernel_Loadbox_Card_WSS2_CARD_VER_TAG_ARRAY"].ToString();
                            obj.Kernel_Loadbox_Card_WSS2_Serial_No = row["Kernel_Loadbox_Card_WSS2_Serial_No"].ToString();
                            obj.Kernel_Loadbox_Card_WSS2_Firmware_Card_Major = row["Kernel_Loadbox_Card_WSS2_Firmware_Card_Major"].ToString();
                            obj.Kernel_Loadbox_Card_WSS2_Firmware_Card_Minor = row["Kernel_Loadbox_Card_WSS2_Firmware_Card_Minor"].ToString();
                            obj.Kernel_Loadbox_Card_WSS2_Firmware_FPGA_Major = row["Kernel_Loadbox_Card_WSS2_Firmware_FPGA_Major"].ToString();
                            obj.Kernel_Loadbox_Card_WSS2_Firmware_FPGA_Minor = row["Kernel_Loadbox_Card_WSS2_Firmware_FPGA_Minor"].ToString();
                            obj.Kernel_Loadbox_Card_WSS2_Calibration_Day = row["Kernel_Loadbox_Card_WSS2_Calibration_Day"].ToString();
                            obj.Kernel_Loadbox_Card_WSS2_Calibration_Month = row["Kernel_Loadbox_Card_WSS2_Calibration_Month"].ToString();
                            obj.Kernel_Loadbox_Card_WSS2_Calibration_Year = row["Kernel_Loadbox_Card_WSS2_Calibration_Year"].ToString();
                            obj.Kernel_Loadbox_Card_WSS2_Next_Calibration_Day = row["Kernel_Loadbox_Card_WSS2_Next_Calibration_Day"].ToString();
                            obj.Kernel_Loadbox_Card_WSS2_Next_Calibration_Month = row["Kernel_Loadbox_Card_WSS2_Next_Calibration_Month"].ToString();
                            obj.Kernel_Loadbox_Card_WSS2_Next_Calibration_Year = row["Kernel_Loadbox_Card_WSS2_Next_Calibration_Year"].ToString();
                            obj.Kernel_Loadbox_Card_WSS2_Day_of_Creation = row["Kernel_Loadbox_Card_WSS2_Day_of_Creation"].ToString();
                            obj.Kernel_Loadbox_Card_WSS2_Month_of_Creation = row["Kernel_Loadbox_Card_WSS2_Month_of_Creation"].ToString();
                            obj.Kernel_Loadbox_Card_WSS2_Year_of_Creation = row["Kernel_Loadbox_Card_WSS2_Year_of_Creation"].ToString();
                            obj.Kernel_Loadbox_Card_WSS2_BuildVersion = row["Kernel_Loadbox_Card_WSS2_BuildVersion"].ToString();
                            table12DataViews.Add(obj);

                        }
                        //hwDataViews = hwDataViews.OrderByDescending(x => x.Date).GroupBy(x => new { x.EEPName, x.Date, x.System_name }).Select(x => x.FirstOrDefault()).OrderBy(x => x.Date).ToList();


                        var result = Json(new { data = table12DataViews }, JsonRequestBehavior.AllowGet); result.MaxJsonLength = int.MaxValue; return result;
                    }
                }
                catch (Exception Ex)
                {
                    return View("Unable to retrieve data from database. Please try again later!!");
                }
            }
            else
            {
                var result = Json(new { success = false, message = "Try again" }, JsonRequestBehavior.AllowGet); result.MaxJsonLength = int.MaxValue; return result;
            }
        }

        //Fetch the Diagnostic Tool Version details from database and pass as Json Object to view
        [HttpPost]
        public ActionResult GetTABLE24_DiagnosticToolData_unique(List<string> pc, DateTime sdate, DateTime edate, int diff_UTCandLocal)
        {
            List<TABLE24_DiagnosticToolView> table3DataViews = new List<TABLE24_DiagnosticToolView>();
            if (pc != null && pc.Count() > 0)
            {
                string pclist = string.Empty;
                try
                {
                    using (PromasterImport_HWInfoEntities db = new PromasterImport_HWInfoEntities())
                    {
                        db.Database.CommandTimeout = 500;
                        string date = string.Empty;
                        //User chosen HILs mapped Systems are stored in string format
                        foreach (var p in pc)
                        {
                            pclist += ("'" + p.Trim() + "',");
                            //pclist.Add(p.FQDN.Split('.')[0].ToUpper().Trim());
                        }

                        pclist = pclist.Remove(pclist.Length - 1, 1);
                        connection();
                        DataTable dtable = new DataTable();
                        //SQL Query to fetch the relevant details
                        string Query = " Select " + ConfigurationManager.AppSettings["BookingServerReplica"] + " [LabInfo].DisplayName, [System_name],Date = max(Date),[TKWinX_SW_Version],[MM6_SW_Version],[XFlash_SW_Version], CreationDate = max([CreationDate])from [VectorAndDiagSWVersion_Pr] inner join  " + ConfigurationManager.AppSettings["BookingServerReplica"] + "LabComputersPr on substring( " + ConfigurationManager.AppSettings["BookingServerReplica"] + "LabComputersPr.FQDN, 1, len(System_name)) = [VectorAndDiagSWVersion_Pr].System_name inner join " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo] on " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo].id = LabComputersPr.LabId where System_name in (" + pclist + ") AND CONVERT(DATETIME, Date) > = '" + sdate + "' AND CONVERT(DATETIME, Date) < = '" + edate + "' GROUP BY   " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo].DisplayName , [System_name],[TKWinX_SW_Version],[MM6_SW_Version],[XFlash_SW_Version] order by CreationDate";

                        OpenConnection();
                        SqlCommand cmd = new SqlCommand(Query, con);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dtable);
                        CloseConnection();
                        //Store as a list
                        foreach (DataRow row in dtable.Rows)
                        {
                            TABLE24_DiagnosticToolView obj = new TABLE24_DiagnosticToolView();
                            obj.System_name = row["System_name"].ToString();
                            obj.LC_Name = row["DisplayName"].ToString();
                            obj.Date = row["Date"].ToString(); obj.CreationDate = DateTime.Parse(row["CreationDate"].ToString()).AddMinutes(diff_UTCandLocal).ToLongTimeString();
                            obj.TKWinX = row["TKWinX_SW_Version"].ToString().Replace(",", "<br/>\r\n");
                            obj.MM6 = row["MM6_SW_Version"].ToString().Replace(",", "<br/>\r\n");
                            obj.XFlash = row["XFlash_SW_Version"].ToString().Replace(",", "<br/>\r\n");
                            table3DataViews.Add(obj);
                        }

                        //Pass as Json Object to view
                        var result = Json(new { data = table3DataViews }, JsonRequestBehavior.AllowGet); result.MaxJsonLength = int.MaxValue; return result;
                    }
                }
                catch (Exception Ex)
                {
                    return View("Unable to retrieve data from database. Please try again later!!");
                }
            }
            else
            {
                var result = Json(new { success = false, message = "Try again" }, JsonRequestBehavior.AllowGet); result.MaxJsonLength = int.MaxValue; return result;
            }

        }

        //Fetch the Motsim details (Type, Component name, Version) from database and pass as Json Object to view
        [HttpPost]
        public ActionResult GetTABLE25_MotsimData_unique(List<string> pc, DateTime sdate, DateTime edate, int diff_UTCandLocal)
        {
            List<TABLE25_MotsimView> PrjDataViews = new List<TABLE25_MotsimView>();
            if (pc != null && pc.Count() > 0)
            {
                string pclist = string.Empty;
                try
                {
                    using (PromasterImport_HWInfoEntities db = new PromasterImport_HWInfoEntities())
                    {
                        db.Database.CommandTimeout = 500;
                        string date = string.Empty;
                        //User chosen HILs mapped Systems are stored in string format
                        foreach (var p in pc)
                        {
                            pclist += ("'" + p.Trim() + "',");
                            //pclist.Add(p.FQDN.Split('.')[0].ToUpper().Trim());
                        }

                        pclist = pclist.Remove(pclist.Length - 1, 1);
                        //connection();
                        string constring = ConfigurationManager.ConnectionStrings["PromasterImport_ProjectDescEntities_ado"].ToString();
                        con = new SqlConnection(constring);

                        DataTable dtable = new DataTable();

                        string Query = "";
                        //SQL Query to fetch necessary details
                        Query = "Select " + ConfigurationManager.AppSettings["BookingServerReplica"] + " [LabInfo].DisplayName, [System_name],[Component_name],[Version] ,Date = max(Date) ,[Details],CreationDate = max([CreationDate]), case when TRIM(Component_name) in ('MA_APB_MotSim4', 'MA_DC_MotSim4', 'MA_EC_MotSim4', 'MG_MotSim_PB')  then 'Motsim 4'  when TRIM(Component_name) in ('MG_APB_MotSimCon')  then 'Motsim 3' else 'NA' end as 'MotsimType' from ProjectDescription_Pr_1  inner join  " + ConfigurationManager.AppSettings["BookingServerReplica"] + "LabComputersPr on substring( " + ConfigurationManager.AppSettings["BookingServerReplica"] + "LabComputersPr.FQDN, 1, len(System_name)) = [ProjectDescription_Pr_1].System_name inner join " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo] on " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo].id = LabComputersPr.LabId where System_name in (" + pclist + ") AND CONVERT(DATETIME, Date) > = '" + sdate + "' AND CONVERT(DATETIME, Date) < = '" + edate + "' and (TRIM(Component_name) in ('MA_APB_MotSim4', 'MA_DC_MotSim4', 'MA_EC_MotSim4', 'MG_MotSim_PB','MG_APB_MotSimCon')) GROUP BY   " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo].DisplayName ,[System_name],[Component_name],[Version]  ,[Details] order by CreationDate ";

                        OpenConnection();
                        SqlCommand cmd = new SqlCommand(Query, con);
                        cmd.CommandTimeout = 0;
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dtable);
                        CloseConnection();

                        foreach (DataRow row in dtable.Rows)
                        {
                            TABLE25_MotsimView obj = new TABLE25_MotsimView();
                            obj.System_name = row["System_name"].ToString();
                            obj.LC_Name = row["DisplayName"].ToString();
                            obj.Date = row["Date"].ToString(); obj.CreationDate = DateTime.Parse(row["CreationDate"].ToString()).AddMinutes(diff_UTCandLocal).ToLongTimeString();

                            obj.Component_name = row["Component_name"].ToString();

                            obj.Project = row["Details"].ToString();

                            obj.Version = row["Version"].ToString();

                            obj.MotsimType = row["MotsimType"].ToString();
                            PrjDataViews.Add(obj);
                        }
                        //Pass as Json Object to view
                        var result = Json(new
                        {
                            data = PrjDataViews
                        },
                     JsonRequestBehavior.AllowGet); result.MaxJsonLength = int.MaxValue; return result;
                        //var result = Json(new { data = PrjDataViews },  JsonRequestBehavior.AllowGet);result.MaxJsonLength = int.MaxValue;return result;
                    }
                }
                catch (Exception Ex)
                {
                    return View("Unable to retrieve data from database. Please try again later!!");
                }
            }
            else
            {
                var result = Json(new { success = false, message = "Try again" }, JsonRequestBehavior.AllowGet); result.MaxJsonLength = int.MaxValue; return result;
            }
        }


        //Fetch the Diagnostic Tool Version details from database and pass as Json Object to view
        //[HttpPost]
        //public ActionResult GetTABLE26_VectorToolData_unique(List<string> pc, DateTime sdate, DateTime edate, int diff_UTCandLocal)
        //{
        //    List<TABLE26_VectorToolView> table3DataViews = new List<TABLE26_VectorToolView>();
        //    if (pc != null && pc.Count() > 0)
        //    {
        //        string pclist = string.Empty;
        //        try
        //        {
        //            using (PromasterImport_HWInfoEntities db = new PromasterImport_HWInfoEntities())
        //            {
        //                db.Database.CommandTimeout = 500;
        //                string date = string.Empty;
        //                //User chosen HILs mapped Systems are stored in string format
        //                foreach (var p in pc)
        //                {
        //                    pclist += ("'" + p.Trim() + "',");
        //                    //pclist.Add(p.FQDN.Split('.')[0].ToUpper().Trim());
        //                }

        //                pclist = pclist.Remove(pclist.Length - 1, 1);
        //                connection();
        //                DataTable dtable = new DataTable();
        //                //SQL Query to fetch the relevant details
        //                string Query = " Select " + ConfigurationManager.AppSettings["BookingServerReplica"] + " [LabInfo].DisplayName, [System_name],Date = max(Date),[Vector_SW_Name],[Vector_SW_Version], CreationDate = max([CreationDate])from [VectorAndDiagSWVersion_Pr] inner join  " + ConfigurationManager.AppSettings["BookingServerReplica"] + "LabComputersPr on substring( " + ConfigurationManager.AppSettings["BookingServerReplica"] + "LabComputersPr.FQDN, 1, len(System_name)) = [VectorAndDiagSWVersion_Pr].System_name inner join " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo] on " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo].id = LabComputersPr.LabId where System_name in (" + pclist + ") AND CONVERT(DATETIME, Date) > = '" + sdate + "' AND CONVERT(DATETIME, Date) < = '" + edate + "' GROUP BY   " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo].DisplayName , [System_name],[Vector_SW_Name],[Vector_SW_Version] order by CreationDate";

        //                OpenConnection();
        //                SqlCommand cmd = new SqlCommand(Query, con);
        //                SqlDataAdapter da = new SqlDataAdapter(cmd);
        //                da.Fill(dtable);
        //                CloseConnection();
        //                //Store as a list
        //                foreach (DataRow row in dtable.Rows)
        //                {
        //                    TABLE26_VectorToolView obj = new TABLE26_VectorToolView();
        //                    obj.System_name = row["System_name"].ToString();
        //                    obj.LC_Name = row["DisplayName"].ToString();
        //                    obj.Date = row["Date"].ToString(); obj.CreationDate = DateTime.Parse(row["CreationDate"].ToString()).AddMinutes(diff_UTCandLocal).ToLongTimeString();
        //                    obj.Vector_SW_Name = row["Vector_SW_Name"].ToString().Replace(",", "<br/>\r\n");
        //                    obj.Vector_SW_Version = row["Vector_SW_Version"].ToString().Replace(",", "<br/>\r\n");
        //                    table3DataViews.Add(obj);
        //                }

        //                //Pass as Json Object to view
        //                var result = Json(new { data = table3DataViews }, JsonRequestBehavior.AllowGet); result.MaxJsonLength = int.MaxValue; return result;
        //            }
        //        }
        //        catch (Exception Ex)
        //        {
        //            return View("Unable to retrieve data from database. Please try again later!!");
        //        }
        //    }
        //    else
        //    {
        //        var result = Json(new { success = false, message = "Try again" }, JsonRequestBehavior.AllowGet); result.MaxJsonLength = int.MaxValue; return result;
        //    }

        //}

        [HttpPost]
        public ActionResult GetTABLE26_VectorToolData_unique(List<string> pc, DateTime sdate, DateTime edate, int diff_UTCandLocal)
        {
            List<TABLE26_VectorToolView> table3DataViews = new List<TABLE26_VectorToolView>();
            if (pc != null && pc.Count() > 0)
            {
                string pclist = string.Empty;
                try
                {
                    using (PromasterImport_HWInfoEntities db = new PromasterImport_HWInfoEntities())
                    {
                        db.Database.CommandTimeout = 500;
                        string date = string.Empty;

                        //User chosen HILs mapped Systems are stored in string format
                        //foreach (var p in pc)
                        //{
                        //    pclist += ("'" + p.Trim() + "',");
                        //    //pclist.Add(p.FQDN.Split('.')[0].ToUpper().Trim());
                        //}

                        //pclist = pclist.Remove(pclist.Length - 1, 1);
                        pclist = string.Join(",", pc);
                        connection();
                        DataTable dtable = new DataTable();
                        //SQL Query to fetch the relevant details
                        string Query = " Exec [dbo].[Vector_GetData_Unique] '" + pclist + "', '" + sdate + "', '" + edate + "' ";


                        //string Query = " Select " + ConfigurationManager.AppSettings["BookingServerReplica"] + " [LabInfo].DisplayName, [System_name],Date = max(Date) ,[Vector_HW_Name],[Vector_DeviceSerialNumber],[Vector_SW_Name],[Vector_SW_Version],[SP],[Vector_License_Name],[Vector_License_ID],[Vector_LicenseMaxVersion],[Vector_LicenseType]," +
                        //    "  CASE WHEN LEN(FR_Vector_License_Name) < 3 THEN REPLACE([FR_Vector_License_Name],'NA','') ELSE [FR_Vector_License_Name] END AS FR_Vector_License_Name, REPLACE([FR_Vector_License_ID],'NA','') AS FR_Vector_License_ID,[FR_Vector_LicenseMaxVersion],[FR_Vector_LicenseType],[FR], CASE WHEN LEN([LIN_Vector_License_Name]) < 3 THEN  REPLACE([LIN_Vector_License_Name],'NA','') ELSE [LIN_Vector_License_Name] END AS LIN_Vector_License_Name, REPLACE([LIN_Vector_License_ID],'NA','') AS LIN_Vector_License_ID, [LIN_Vector_LicenseMaxVersion],[LIN_Vector_LicenseType],[LIN], CASE WHEN LEN([DIVA_Vector_License_Name]) < 3 THEN REPLACE([DIVA_Vector_License_Name],'NA','') ELSE [DIVA_Vector_License_Name] END AS DIVA_Vector_License_Name, REPLACE([DIVA_Vector_License_ID],'NA','') AS DIVA_Vector_License_ID,[DIVA_Vector_LicenseMaxVersion],[DIVA_Vector_LicenseType],[DIVA], CASE WHEN LEN([AMD/XCP_Vector_License_Name]) < 3 THEN REPLACE([AMD/XCP_Vector_License_Name],'NA','') ELSE [AMD/XCP_Vector_License_Name] END AS [AMD/XCP_Vector_License_Name],REPLACE([AMD/XCP_Vector_License_ID],'NA','') AS [AMD/XCP_Vector_License_ID],[AMD/XCP_Vector_LicenseMaxVersion],[AMD/XCP_Vector_LicenseType],[AMD/XCP], CreationDate = max([CreationDate]) " +
                        //    "from [VectorAndDiagSWVersion_Pr] inner join  " + ConfigurationManager.AppSettings["BookingServerReplica"] + "LabComputersPr on substring( " + ConfigurationManager.AppSettings["BookingServerReplica"] + "LabComputersPr.FQDN, 1, len(System_name)) = [VectorAndDiagSWVersion_Pr].System_name inner join " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo] on " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo].id = LabComputersPr.LabId where System_name in (" + pclist + ") AND CONVERT(DATETIME, Date) > = '" + sdate + "' AND CONVERT(DATETIME, Date) < = '" + edate + "' GROUP BY   " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo].DisplayName , [System_name] ,[Vector_HW_Name],[Vector_DeviceSerialNumber],[Vector_SW_Name],[Vector_SW_Version],[SP],[Vector_License_Name],[Vector_License_ID],[Vector_LicenseMaxVersion],[Vector_LicenseType]      ,[FR_Vector_License_Name],[FR_Vector_License_ID],[FR_Vector_LicenseMaxVersion],[FR_Vector_LicenseType],[FR],[LIN_Vector_License_Name],[LIN_Vector_License_ID],[LIN_Vector_LicenseMaxVersion],[LIN_Vector_LicenseType],[LIN],[DIVA_Vector_License_Name],[DIVA_Vector_License_ID],[DIVA_Vector_LicenseMaxVersion],[DIVA_Vector_LicenseType],[DIVA],[AMD/XCP_Vector_License_Name],[AMD/XCP_Vector_License_ID],[AMD/XCP_Vector_LicenseMaxVersion],[AMD/XCP_Vector_LicenseType],[AMD/XCP] order by CreationDate";

                        OpenConnection();
                        SqlCommand cmd = new SqlCommand(Query, con);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dtable);
                        CloseConnection();
                        //Store as a list
                        foreach (DataRow row in dtable.Rows)
                        {
                            TABLE26_VectorToolView obj = new TABLE26_VectorToolView();
                            obj.System_name = row["System_name"].ToString();
                            obj.LC_Name = row["DisplayName"].ToString();
                            obj.Date = row["Date"].ToString(); obj.CreationDate = DateTime.Parse(row["CreationDate"].ToString()).AddMinutes(diff_UTCandLocal).ToLongTimeString();
                            //obj.Vector_SW_Name = row["Vector_SW_Name"].ToString().Replace(",", "<br/>\r\n");
                            //obj.Vector_SW_Version = row["Vector_SW_Version"].ToString().Replace(",", "<br/>\r\n");
                            //obj.Vector_License_Name         = row["Vector_License_Name"].ToString();
                            //obj.Vector_License_ID           = row["Vector_License_ID"].ToString();
                            //obj.Vector_HW_Name              = row["Vector_HW_Name"].ToString();
                            //obj.Vector_LicenseMaxVersion    = row["Vector_LicenseMaxVersion"].ToString();
                            //obj.Vector_DeviceSerialNumber   = row["Vector_DeviceSerialNumber"].ToString();
                            //obj.Vector_LicenseType          = row["Vector_LicenseType"].ToString();


                            //obj.Vector_HW_Name = row["Vector_HW_Name"].ToString();
                            //obj.Vector_DeviceSerialNumber = row["Vector_DeviceSerialNumber"].ToString();
                            //obj.Vector_SW_Name = row["Vector_SW_Name"].ToString().Replace(",", "<br/>\r\n"); ;
                            //obj.Vector_SW_Version = row["Vector_SW_Version"].ToString().Replace(",", "<br/>\r\n"); ;
                            //obj.SP = row["SP"].ToString().Replace(",", "<br/>\r\n");
                            //obj.Vector_License_Name = string.Concat(row["Vector_License_Name"].ToString(), " - ", row["Vector_License_ID"].ToString()).Replace(",", "<br/>\r\n");
                            //// [Vector_License_ID]                 = row["Vector_License_ID]                 "].ToString();
                            //obj.Vector_LicenseMaxVersion = row["Vector_LicenseMaxVersion"].ToString();
                            //// [Vector_LicenseType]                = row["Vector_LicenseType]                "].ToString();
                            //obj.FR_Vector_License = string.Concat(row["FR_Vector_License_Name"].ToString(), " - ", row["FR_Vector_License_ID"].ToString());
                            //// [FR_Vector_License_ID]              = row["FR_Vector_License_ID]              "].ToString();
                            //// [FR_Vector_LicenseMaxVersion]       = row["FR_Vector_LicenseMaxVersion]       "].ToString();
                            //// [FR_Vector_LicenseType]             = row["FR_Vector_LicenseType]             "].ToString();
                            //obj.FR = row["FR"].ToString();
                            //obj.LIN_Vector_License = string.Concat(row["LIN_Vector_License_Name"].ToString(), " - ", row["LIN_Vector_License_ID"].ToString());
                            //// [LIN_Vector_License_ID]             = row["LIN_Vector_License_ID]             "].ToString();
                            //// [LIN_Vector_LicenseMaxVersion]      = row["LIN_Vector_LicenseMaxVersion]      "].ToString();
                            //// [LIN_Vector_LicenseType]            = row["LIN_Vector_LicenseType]            "].ToString();
                            //obj.LIN = row["LIN"].ToString();
                            //obj.DIVA_Vector_License = string.Concat(row["DIVA_Vector_License_Name"].ToString(), " - ", row["DIVA_Vector_License_ID"].ToString());
                            //// [DIVA_Vector_License_ID]            = row["DIVA_Vector_License_ID]            "].ToString();
                            //// [DIVA_Vector_LicenseMaxVersion]     = row["DIVA_Vector_LicenseMaxVersion]     "].ToString();
                            //// [DIVA_Vector_LicenseType]           = row["DIVA_Vector_LicenseType]           "].ToString();
                            //obj.DIVA = row["DIVA"].ToString();
                            //obj.AMD_orXCP_Vector_License = string.Concat(row["AMD/XCP_Vector_License_Name"].ToString(), " - ", row["AMD/XCP_Vector_License_ID"].ToString());
                            ////[AMD/ XCP_Vector_License_ID]        = row["AMD/ XCP_Vector_License_ID]        "].ToString();
                            ////[AMD/ XCP_Vector_LicenseMaxVersion] = row["AMD/ XCP_Vector_LicenseMaxVersion] "].ToString();
                            ////[AMD/ XCP_Vector_LicenseType]       = row["AMD/ XCP_Vector_LicenseType]       "].ToString();
                            //obj.AMD_orXCP = row["AMD/XCP"].ToString();


                            obj.Vector_HW_Name = row["Vector_HW_Name"].ToString().Replace(",", "<br/>\r\n");
                            obj.Vector_DeviceSerialNumber = row["Vector_DeviceSerialNumber"].ToString().Replace(",", "<br/>\r\n");
                            obj.Vector_SW_Name = row["Vector_SW_Name"].ToString().Replace(",", "<br/>\r\n");
                            obj.Vector_SW_Version = row["Vector_SW_Version"].ToString().Replace(",", "<br/>\r\n");
                            obj.SP = row["SP"].ToString().Replace(",", "<br/>\r\n");
                            obj.Vector_License_Name = row["Vector_License"].ToString().Replace("NA-NA", "-").Replace(",", "<br/>\r\n");
                            obj.Vector_LicenseMaxVersion = row["Vector_LicenseMaxVersion"].ToString().Replace(",", "<br/>\r\n"); ;
                            obj.FR_Vector_License = row["FR_Vector_License"].ToString().Replace("NA-NA", "-").Replace(",", "<br/>\r\n");
                            obj.FR = row["FR"].ToString();
                            obj.LIN_Vector_License = row["LIN_Vector_License"].ToString().Replace("NA-NA", "-").Replace(",", "<br/>\r\n");
                            obj.LIN = row["LIN"].ToString();
                            obj.DIVA_Vector_License = row["DIVA_Vector_License"].ToString().Replace("NA-NA", "-").Replace(",", "<br/>\r\n");
                            obj.DIVA = row["DIVA"].ToString();
                            obj.AMD_orXCP_Vector_License = row["AMD/XCP_Vector_License"].ToString().Replace("NA-NA", "-").Replace(",", "<br/>\r\n");
                            obj.AMD_orXCP = row["AMD/XCP"].ToString();

                            obj.Vector_Piggy = row["Vector_Piggy"].ToString().Replace(",", "<br/>\r\n");
                            var piggy_list = row["Vector_Piggy"].ToString().Split(',');
                            List<string> piggy_device = new List<string>();
                            List<string> piggy_application = new List<string>();
                            List<string> piggy_busType = new List<string>();
                            List<string> piggy_channel = new List<string>();
                            foreach (var x in piggy_list)
                            {
                                if (x.Trim() != "")
                                {
                                    piggy_device.Add(x.Split('-')[0] + (x.Split('-')[0].Trim() == "" ? "-" : "")  +",");
                                    piggy_application.Add(x.Split('-')[1] + (x.Split('-')[1].Trim() == "" ? "-":"")  + ",");
                                    piggy_busType.Add(x.Split('-')[2] + (x.Split('-')[2].Trim() == "" ? "-" : "" )+ ",");
                                    piggy_channel.Add(x.Split('-')[3]+ (x.Split('-')[3].Trim() == "" ? "-" : "") + ",");
                                }
                                else
                                {
                                    piggy_device.Add("-,");
                                    piggy_application.Add("-,");
                                    piggy_busType.Add("-,");
                                    piggy_channel.Add("-,");
                                }

                            }

                            obj.Vector_Piggy_Device = string.Join("", piggy_device).Replace(",", "<br/>\r\n");
                            obj.Vector_Piggy_Application = string.Join("", piggy_application).Replace(",", "<br/>\r\n");
                            obj.Vector_Piggy_BusType = string.Join("", piggy_busType).Replace(",", "<br/>\r\n");
                            obj.Vector_Piggy_Channel = string.Join("", piggy_channel).Replace(",", "<br/>\r\n");





                            table3DataViews.Add(obj);
                        }

                        //Pass as Json Object to view
                        var result = Json(new { data = table3DataViews }, JsonRequestBehavior.AllowGet); result.MaxJsonLength = int.MaxValue; return result;
                    }
                }
                catch (Exception Ex)
                {
                    return View("Unable to retrieve data from database. Please try again later!!");
                }
            }
            else
            {
                var result = Json(new { success = false, message = "Try again" }, JsonRequestBehavior.AllowGet); result.MaxJsonLength = int.MaxValue; return result;
            }

        }


        [HttpPost]
        public ActionResult GetTABLE27_VDMHYMData_unique(List<string> pc, DateTime sdate, DateTime edate, int diff_UTCandLocal)
        {
            List<TABLE2_ProjectDescriptionView> PrjDataViews = new List<TABLE2_ProjectDescriptionView>();
            if (pc != null && pc.Count() > 0)
            {
                string pclist = string.Empty;
                try
                {
                    using (PromasterImport_HWInfoEntities db = new PromasterImport_HWInfoEntities())
                    {
                        db.Database.CommandTimeout = 500;
                        string date = string.Empty;
                        foreach (var p in pc)
                        {
                            pclist += ("'" + p.Trim() + "',");
                            //pclist.Add(p.FQDN.Split('.')[0].ToUpper().Trim());
                        }

                        pclist = pclist.Remove(pclist.Length - 1, 1);
                        //connection();
                        string constring = ConfigurationManager.ConnectionStrings["PromasterImport_ProjectDescEntities_ado"].ToString();
                        con = new SqlConnection(constring);

                        DataTable dtable = new DataTable();

                        string Query = "";

                        Query = " Select " + ConfigurationManager.AppSettings["BookingServerReplica"] + " [LabInfo].DisplayName, [System_name],[Component_name],[Version] , Date = max(Date) , CreationDate = max([CreationDate]), case when Component_name like '%VDM%' then 'VDM' when Component_name like '%HYM%' then 'HYM'  end as 'ModelType' from ProjectDescription_Pr_1 inner join " + ConfigurationManager.AppSettings["BookingServerReplica"] + "LabComputersPr on substring( " + ConfigurationManager.AppSettings["BookingServerReplica"] + "LabComputersPr.FQDN, 1, len(System_name)) = [ProjectDescription_Pr_1 ].System_name inner join " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo] on " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo].id = LabComputersPr.LabId where System_name in (" + pclist + ") AND CONVERT(DATETIME, Date) > = '" + sdate + "' AND CONVERT(DATETIME, Date) < = '" + edate + "' and (Component_name like '%VDM%' or  Component_name like '%HYM%') GROUP BY   " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo].DisplayName ,  [System_name],[Component_name],[Version]  order by CreationDate";

                        OpenConnection();
                        SqlCommand cmd = new SqlCommand(Query, con);
                        cmd.CommandTimeout = 0;
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dtable);
                        CloseConnection();

                        foreach (DataRow row in dtable.Rows)
                        {
                            TABLE2_ProjectDescriptionView obj = new TABLE2_ProjectDescriptionView();
                            obj.System_name = row["System_name"].ToString();
                            obj.LC_Name = row["DisplayName"].ToString();
                            obj.Date = row["Date"].ToString(); obj.CreationDate = DateTime.Parse(row["CreationDate"].ToString()).AddMinutes(diff_UTCandLocal).ToLongTimeString();

                            obj.Component_name = row["Component_name"].ToString();

                            //obj.Details = row["Details"].ToString();

                            obj.Version = row["Version"].ToString();

                            obj.ModelType = row["ModelType"].ToString();
                            PrjDataViews.Add(obj);
                        }
                        var result = Json(new
                        {
                            data = PrjDataViews
                        },
                     JsonRequestBehavior.AllowGet); result.MaxJsonLength = int.MaxValue; return result;
                        //var result = Json(new { data = PrjDataViews },  JsonRequestBehavior.AllowGet);result.MaxJsonLength = int.MaxValue;return result;
                    }
                }
                catch (Exception Ex)
                {
                    return View("Unable to retrieve data from database. Please try again later!!");
                }
            }
            else
            {
                var result = Json(new { success = false, message = "Try again" }, JsonRequestBehavior.AllowGet); result.MaxJsonLength = int.MaxValue; return result;
            }
        }

        [HttpPost]
        public ActionResult GetTABLE28_IISBoxData_unique(List<string> pc, DateTime sdate, DateTime edate, int diff_UTCandLocal)
        {
            List<TABLE2_ProjectDescriptionView> PrjDataViews = new List<TABLE2_ProjectDescriptionView>();
            if (pc != null && pc.Count() > 0)
            {
                string pclist = string.Empty;
                try
                {
                    using (PromasterImport_HWInfoEntities db = new PromasterImport_HWInfoEntities())
                    {
                        db.Database.CommandTimeout = 500;
                        string date = string.Empty;

                        pclist = string.Join(",", pc);
                        //connection();
                        DataTable dtable = new DataTable();
                        string constring = ConfigurationManager.ConnectionStrings["PromasterImport_ProjectDescEntities_ado"].ToString();
                        con = new SqlConnection(constring);

                        //SQL Query to fetch the relevant details
                        string Query = " Exec PromasterImport_ProjectDesc.[dbo].[GetIISBox_Info] '" + pclist + "', '" + sdate + "', '" + edate + "', '" + "Latest" + "' ";


                        //foreach (var p in pc)
                        //{
                        //    pclist += ("'" + p.Trim() + "',");
                        //    //pclist.Add(p.FQDN.Split('.')[0].ToUpper().Trim());
                        //}

                        //pclist = pclist.Remove(pclist.Length - 1, 1);
                        //connection();

                        //string Query = //" Select " + ConfigurationManager.AppSettings["BookingServerReplica"] + " [LabInfo].DisplayName, [System_name],[Component_name],[Version] , Date = max(Date) , CreationDate = max([CreationDate]) from ProjectDescription_Pr_1 inner join " + ConfigurationManager.AppSettings["BookingServerReplica"] + "LabComputersPr on substring( " + ConfigurationManager.AppSettings["BookingServerReplica"] + "LabComputersPr.FQDN, 1, len(System_name)) = [ProjectDescription_Pr_1 ].System_name inner join " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo] on " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo].id = LabComputersPr.LabId where System_name in (" + pclist + ") AND CONVERT(DATETIME, Date) > = '" + sdate + "' AND CONVERT(DATETIME, Date) < = '" + edate + "' and Component_name like '%IIS%' GROUP BY   " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo].DisplayName ,  [System_name],[Component_name],[Version]   order by CreationDate";
                        //" select * from ( Select [BookingServerReplica].[dbo]. [LabInfo].DisplayName, [System_name],[Component_name],[Version] , Date = convert(nvarchar, max(Date))  , CreationDate = convert(nvarchar, max([CreationDate])) from ProjectDescription_Pr_1 inner join [BookingServerReplica].[dbo].LabComputersPr on substring( [BookingServerReplica].[dbo].LabComputersPr.FQDN, 1, len(System_name)) = [ProjectDescription_Pr_1 ].System_name inner join  [BookingServerReplica].[dbo].[LabInfo] on [BookingServerReplica].[dbo].[LabInfo].id = LabComputersPr.LabId where System_name in ('ABT-C-00099','abtz0nrj','ABT-C-000AD','abtz0nrs') AND  CONVERT(DATETIME, Date) > = '3/21/2023 12:00:00 AM' AND CONVERT(DATETIME, Date) < = '4/21/2023 12:00:00 AM' and Component_name like '%IIS%' GROUP BY  [BookingServerReplica].[dbo].[LabInfo].DisplayName ,  [System_name],[Component_name],[Version]  union  select distinct [BookingServerReplica].[dbo]. [LabInfo].DisplayName, System_name,						'-' as Component_name,'-' as version , Date = '-', CreationDate = '-' 						 from ProjectDescription_Pr_1 						inner join [BookingServerReplica].[dbo].LabComputersPr on substring( [BookingServerReplica].[dbo].LabComputersPr.FQDN, 1, len(System_name)) = [ProjectDescription_Pr_1 ].System_name inner join  [BookingServerReplica].[dbo].[LabInfo] on [BookingServerReplica].[dbo].[LabInfo].id = LabComputersPr.LabId where System_name in ('ABT-C-00099','abtz0nrj','abtz0nrs','ABT-C-000AD')  and System_name not in (					  Select distinct System_name from ProjectDescription_Pr_1 where System_name in ('ABT-C-00099','abtz0nrj','abtz0nrs') AND  CONVERT(DATETIME, Date) > = '3/21/2023 12:00:00 AM' AND CONVERT(DATETIME, Date) < = '4/21/2023 12:00:00 AM' and Component_name like '%IIS%' )  )C order by CreationDate desc";

                        OpenConnection();
                        SqlCommand cmd = new SqlCommand(Query, con);
                        cmd.CommandTimeout = 0;
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dtable);
                        CloseConnection();

                        foreach (DataRow row in dtable.Rows)
                        {
                            TABLE2_ProjectDescriptionView obj = new TABLE2_ProjectDescriptionView();
                            obj.System_name = row["System_name"].ToString();
                            obj.LC_Name = row["DisplayName"].ToString();
                            obj.Date = row["Date"].ToString(); obj.CreationDate = (row["CreationDate"].ToString().Trim() != "-") ? (DateTime.Parse(row["CreationDate"].ToString()).AddMinutes(diff_UTCandLocal).ToLongTimeString()) : row["CreationDate"].ToString();

                            obj.Component_name = row["Component_name"].ToString();


                            obj.Version = row["Version"].ToString();

                            // obj.ModelType = row["ModelType"].ToString();
                            PrjDataViews.Add(obj);
                        }
                        var result = Json(new
                        {
                            data = PrjDataViews
                        },
                     JsonRequestBehavior.AllowGet); result.MaxJsonLength = int.MaxValue; return result;
                        //var result = Json(new { data = PrjDataViews },  JsonRequestBehavior.AllowGet);result.MaxJsonLength = int.MaxValue;return result;
                    }
                }
                catch (Exception Ex)
                {
                    return View("Unable to retrieve data from database. Please try again later!!");
                }
            }
            else
            {
                var result = Json(new { success = false, message = "Try again" }, JsonRequestBehavior.AllowGet); result.MaxJsonLength = int.MaxValue; return result;
            }
        }


        //Fetch the Motsim details (Type, Component name, Version) from database and pass as Json Object to view
        //[HttpPost]
        //public ActionResult GetTABLE25_MotsimData_unique(List<string> pc, DateTime sdate, DateTime edate, int diff_UTCandLocal)
        //{
        //    List<TABLE25_MotsimView> PrjDataViews = new List<TABLE25_MotsimView>();
        //    if (pc != null && pc.Count() > 0)
        //    {
        //        string pclist = string.Empty;
        //        try
        //        {
        //            using (PromasterImport_HWInfoEntities db = new PromasterImport_HWInfoEntities())
        //            {
        //                db.Database.CommandTimeout = 500;
        //                string date = string.Empty;
        //                //User chosen HILs mapped Systems are stored in string format
        //                foreach (var p in pc)
        //                {
        //                    pclist += ("'" + p.Trim() + "',");
        //                    //pclist.Add(p.FQDN.Split('.')[0].ToUpper().Trim());
        //                }

        //                pclist = pclist.Remove(pclist.Length - 1, 1);
        //                //connection();
        //                string constring = ConfigurationManager.ConnectionStrings["PromasterImport_ProjectDescEntities_ado"].ToString();
        //                con = new SqlConnection(constring);

        //                DataTable dtable = new DataTable();

        //                string Query = "";
        //                //SQL Query to fetch necessary details
        //                Query = "Select " + ConfigurationManager.AppSettings["BookingServerReplica"] + " [LabInfo].DisplayName, [System_name],[Component_name],[Version] ,Date = max(Date) ,[Details],CreationDate = max([CreationDate]), case when TRIM(Component_name) in ('MA_APB_MotSim4', 'MA_DC_MotSim4', 'MA_EC_MotSim4', 'MG_MotSim_PB')  then 'Motsim 4'  when TRIM(Component_name) in ('MG_APB_MotSimCon')  then 'Motsim 3' else 'NA' end as 'MotsimType' from ProjectDescription_Pr_1  inner join  " + ConfigurationManager.AppSettings["BookingServerReplica"] + "LabComputersPr on substring( " + ConfigurationManager.AppSettings["BookingServerReplica"] + "LabComputersPr.FQDN, 1, len(System_name)) = [ProjectDescription_Pr_1].System_name inner join " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo] on " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo].id = LabComputersPr.LabId where System_name in (" + pclist + ") AND CONVERT(DATETIME, Date) > = '" + sdate + "' AND CONVERT(DATETIME, Date) < = '" + edate + "' and (TRIM(Component_name) in ('MA_APB_MotSim4', 'MA_DC_MotSim4', 'MA_EC_MotSim4', 'MG_MotSim_PB','MG_APB_MotSimCon')) GROUP BY   " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo].DisplayName ,[System_name],[Component_name],[Version]  ,[Details] order by CreationDate ";

        //                OpenConnection();
        //                SqlCommand cmd = new SqlCommand(Query, con);
        //                cmd.CommandTimeout = 0;
        //                SqlDataAdapter da = new SqlDataAdapter(cmd);
        //                da.Fill(dtable);
        //                CloseConnection();

        //                foreach (DataRow row in dtable.Rows)
        //                {
        //                    TABLE25_MotsimView obj = new TABLE25_MotsimView();
        //                    obj.System_name = row["System_name"].ToString();
        //                    obj.LC_Name = row["DisplayName"].ToString();
        //                    obj.Date = row["Date"].ToString(); obj.CreationDate = DateTime.Parse(row["CreationDate"].ToString()).AddMinutes(diff_UTCandLocal).ToLongTimeString();

        //                    obj.Component_name = row["Component_name"].ToString();

        //                    obj.Project = row["Details"].ToString();

        //                    obj.Version = row["Version"].ToString();

        //                    obj.MotsimType = row["MotsimType"].ToString();
        //                    PrjDataViews.Add(obj);
        //                }
        //                //Pass as Json Object to view
        //                var result = Json(new
        //                {
        //                    data = PrjDataViews
        //                },
        //             JsonRequestBehavior.AllowGet); result.MaxJsonLength = int.MaxValue; return result;
        //                //var result = Json(new { data = PrjDataViews },  JsonRequestBehavior.AllowGet);result.MaxJsonLength = int.MaxValue;return result;
        //            }
        //        }
        //        catch (Exception Ex)
        //        {
        //            return View("Unable to retrieve data from database. Please try again later!!");
        //        }
        //    }
        //    else
        //    {
        //        var result = Json(new { success = false, message = "Try again" }, JsonRequestBehavior.AllowGet); result.MaxJsonLength = int.MaxValue; return result;
        //    }
        //}








        //************************************** ALL SESSIONS  ***********************



        [HttpPost]
        public ActionResult GetTABLE1_HWDescData(List<string> pc, DateTime sdate, DateTime edate, int diff_UTCandLocal)
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

                            foreach (var pcname in pc)
                            {
                                //var pcname = pcname1.FQDN.Split('.')[0].ToUpper().Trim();
                                if (pcname != null && pcname != string.Empty && pcname.Count() > 0)
                                {

                                    //var hwItems = (from item in db.Hardware_Desc_Pr orderby item.Date where item.System_name.Contains(pcname) && item.Date.Contains(date) select new { item.EEPName, item.Date, item.EEPBuildDate, item.EEPDatabaseVersion, item.LabCarType, item.PCHostName, item.System_name, item.RBCCAFVersion, item.RTPCName, item.RTPCSoftwareVersion, item.ToolChainVersion }).ToList();
                                    //var hwItems1= (from item in db.Hardware_Desc_Pr orderby item.Date where  item.Date.Contains(date) && item.System_name.Contains(pcname) select new { item.EEPName, item.Date, item.EEPBuildDate, item.EEPDatabaseVersion, item.LabCarType, item.PCHostName, item.System_name, item.RBCCAFVersion, item.RTPCName, item.RTPCSoftwareVersion, item.ToolChainVersion }).OrderByDescending(x=>x.Date).GroupBy(x=>x.EEPName).ToList();
                                    //var hwItem2 = (from item in db.Hardware_Desc_Pr orderby item.Date where item.System_name.Contains(pcname) && item.Date.Contains(date) select new { item.EEPName, item.Date, item.EEPBuildDate, item.EEPDatabaseVersion, item.LabCarType, item.PCHostName, item.System_name, item.RBCCAFVersion, item.RTPCName, item.RTPCSoftwareVersion, item.ToolChainVersion }).Distinct().ToList();
                                    //var hwItems3 = (from item in db.Hardware_Desc_Pr orderby item.Date where item.System_name.Contains(pcname) && item.Date.Contains(date) select new { item.EEPName, item.Date, item.EEPBuildDate, item.EEPDatabaseVersion, item.LabCarType, item.PCHostName, item.System_name, item.RBCCAFVersion, item.RTPCName, item.RTPCSoftwareVersion, item.ToolChainVersion }).OrderByDescending(x => x.Date).ToList();
                                    //var hwItems4 = (from item in db.Hardware_Desc_Pr orderby item.Date where item.System_name.Contains(pcname) && item.Date.Contains(date) select new { item.EEPName, item.Date, item.EEPBuildDate, item.EEPDatabaseVersion, item.LabCarType, item.PCHostName, item.System_name, item.RBCCAFVersion, item.RTPCName, item.RTPCSoftwareVersion, item.ToolChainVersion }).GroupBy(x => x.EEPName).ToList();

                                    var hwItems = (from item in db.Hardware_Desc_Pr where item.Date.Contains(date) && item.System_name.Contains(pcname) select new { item.EEPName, item.Date, item.CreationDate, item.EEPBuildDate, item.EEPDatabaseVersion, item.LabCarType, item.PCHostName, item.System_name, item.RBCCAFVersion, item.RTPCName, item.RTPCSoftwareVersion, item.ToolChainVersion }).ToList();

                                    //var hwItems1 = (from item in db.Hardware_Desc_Pr where sdate <= DateTime.Parse(item.Date) && edate >= DateTime.Parse(item.Date) && item.System_name.Contains(pcname) select new { item.EEPName, item.Date, item.EEPBuildDate, item.EEPDatabaseVersion, item.LabCarType, item.PCHostName, item.System_name, item.RBCCAFVersion, item.RTPCName, item.RTPCSoftwareVersion, item.ToolChainVersion }).GroupBy(x => x.EEPName).Select(x => x.FirstOrDefault()).ToList(); //since single date descorderbydate not needed


                                    foreach (var item in hwItems)
                                    {

                                        TABLE1_HwDataView obj = new TABLE1_HwDataView();
                                        obj.System_name = item.System_name;

                                        using (BookingServerReplicaEntities db1 = new BookingServerReplicaEntities())
                                        {
                                            //var chk = db1.LabComputersPrs.ToList();
                                            //var chk1 = db1.LabComputersPrs.Select(v => v.FQDN).ToList();

                                            var labid = db1.LabComputersPrs.ToList().Find(x => x.FQDN.Split('.')[0].ToUpper().Contains(obj.System_name)).LabId;//Find(x => x.DisplayName.ToString() == obj.System_name.ToString()).LabId;                                     
                                            obj.LC_Name = db1.LabInfoes.FirstOrDefault(i => i.Id == labid).DisplayName;
                                        }


                                        obj.PCHostName = item.PCHostName;
                                        obj.Date = DateTime.Parse(item.Date).ToShortDateString(); obj.CreationDate = item.CreationDate.ToString().Remove(0, 10);
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
        public ActionResult TABLE2_GetPrjDescData(List<string> pc, DateTime sdate, DateTime edate, int diff_UTCandLocal)
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

                            foreach (var pcname in pc)
                            {
                                //var pcname = pcname1.FQDN.Split('.')[0].ToUpper().Trim();
                                if (pcname != null && pcname != string.Empty && pcname.Count() > 0)
                                {

                                    var PrjItems = (from item in db.ProjectDescription_Pr_1 where item.Date.Contains(date) && item.System_name.Contains(pcname) select new { item.AECUCable, item.Ascet_Ver, item.Product, item.Component_name, item.Date, item.CreationDate, item.Db_Version, item.Details, item.EMUCable, item.MetaEditor_Ver, item.ProjectBuilder_Ver, item.ProjectEditor_Ver, item.System_name, item.ToolVersion, item.Version }).ToList();

                                    db.Database.CommandTimeout = 10000000;
                                    // var hwItems = (from item in db.Hardware_Desc_Pr where item.Date.Contains(date) && item.System_name.Contains(pcname) select new { item.EEPName, item.Date,item.CreationDate, item.EEPBuildDate, item.EEPDatabaseVersion, item.LabCarType, item.PCHostName, item.System_name, item.RBCCAFVersion, item.RTPCName, item.RTPCSoftwareVersion, item.ToolChainVersion }).GroupBy(x => x.EEPName).Select(x => x.FirstOrDefault()).ToList(); //since single date descorderbydate not needed
                                    //hwDataViews = hwDataViews.OrderByDescending(x => x.Date).GroupBy(x => x.EEPName).Select(x => x.FirstOrDefault()).OrderBy(x=>x.Date).ToList();
                                    //var PrjItems_takingUniqueComponent_Details = (from item in db.ProjectDescription_Pr_1 orderby item.Date where item.System_name.Contains(pcname) && item.Date.Contains(date) select new { item.Component_name, item.Details}).Distinct();
                                    //var PrjItems_takingUniqueComponent = (from item in db.ProjectDescription_Pr_1 orderby item.Date where item.System_name.Contains(pcname) && item.Date.Contains(date) select new { item.Component_name}).Distinct().ToList();
                                    //var PrjItems_takingUniqueDetail = (from item in db.ProjectDescription_Pr_1 orderby item.Date where item.System_name.Contains(pcname) && item.Date.Contains(date) select new { item.Details }).Distinct().ToList();



                                    foreach (var item in PrjItems)
                                    {
                                        TABLE2_ProjectDescriptionView obj = new TABLE2_ProjectDescriptionView();

                                        //obj.Component_name = item.Component_name;
                                        obj.Date = DateTime.Parse(item.Date).ToShortDateString(); obj.CreationDate = item.CreationDate.ToString().Remove(0, 10);
                                        obj.Db_Version = item.Db_Version;
                                        obj.Details = item.Details;
                                        obj.System_name = item.System_name;

                                        using (BookingServerReplicaEntities db1 = new BookingServerReplicaEntities())
                                        {

                                            var labid = db1.LabComputersPrs.ToList().Find(x => x.FQDN.Split('.')[0].ToUpper().Contains(obj.System_name)).LabId;
                                            obj.LC_Name = db1.LabInfoes.FirstOrDefault(i => i.Id == labid).DisplayName;
                                        }

                                        //obj.Version = item.Version;
                                        obj.Product = item.Product;
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
        public ActionResult TABLE2_GetCmpntData(List<string> pc, DateTime sdate, DateTime edate, int diff_UTCandLocal)
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

                            foreach (var pcname in pc)
                            {
                                //var pcname = pcname1.FQDN.Split('.')[0].ToUpper().Trim();
                                if (pcname != null && pcname != string.Empty && pcname.Count() > 0)
                                {

                                    var PrjItems = (from item in db.ProjectDescription_Pr_1 where item.Date.Contains(date) && item.System_name.Contains(pcname) select new { item.AECUCable, item.Ascet_Ver, item.Component_name, item.Date, item.CreationDate, item.Db_Version, item.Details, item.EMUCable, item.MetaEditor_Ver, item.ProjectBuilder_Ver, item.ProjectEditor_Ver, item.System_name, item.ToolVersion, item.Version }).ToList();

                                    db.Database.CommandTimeout = 10000000;
                                    // var hwItems = (from item in db.Hardware_Desc_Pr where item.Date.Contains(date) && item.System_name.Contains(pcname) select new { item.EEPName, item.Date,item.CreationDate, item.EEPBuildDate, item.EEPDatabaseVersion, item.LabCarType, item.PCHostName, item.System_name, item.RBCCAFVersion, item.RTPCName, item.RTPCSoftwareVersion, item.ToolChainVersion }).GroupBy(x => x.EEPName).Select(x => x.FirstOrDefault()).ToList(); //since single date descorderbydate not needed
                                    //hwDataViews = hwDataViews.OrderByDescending(x => x.Date).GroupBy(x => x.EEPName).Select(x => x.FirstOrDefault()).OrderBy(x=>x.Date).ToList();
                                    //var PrjItems_takingUniqueComponent_Details = (from item in db.ProjectDescription_Pr_1 orderby item.Date where item.System_name.Contains(pcname) && item.Date.Contains(date) select new { item.Component_name, item.Details}).Distinct();
                                    //var PrjItems_takingUniqueComponent = (from item in db.ProjectDescription_Pr_1 orderby item.Date where item.System_name.Contains(pcname) && item.Date.Contains(date) select new { item.Component_name}).Distinct().ToList();
                                    //var PrjItems_takingUniqueDetail = (from item in db.ProjectDescription_Pr_1 orderby item.Date where item.System_name.Contains(pcname) && item.Date.Contains(date) select new { item.Details }).Distinct().ToList();



                                    foreach (var item in PrjItems)
                                    {
                                        TABLE2_ProjectDescriptionView obj = new TABLE2_ProjectDescriptionView();

                                        obj.Component_name = item.Component_name;
                                        obj.Date = DateTime.Parse(item.Date).ToShortDateString(); obj.CreationDate = item.CreationDate.ToString().Remove(0, 10);
                                        //obj.Db_Version = item.Db_Version;
                                        obj.Details = item.Details;
                                        obj.System_name = item.System_name;

                                        using (BookingServerReplicaEntities db1 = new BookingServerReplicaEntities())
                                        {

                                            var labid = db1.LabComputersPrs.ToList().Find(x => x.FQDN.Split('.')[0].ToUpper().Contains(obj.System_name)).LabId;
                                            obj.LC_Name = db1.LabInfoes.FirstOrDefault(i => i.Id == labid).DisplayName;
                                        }

                                        obj.Version = item.Version;

                                        //obj.ToolVersion = item.ToolVersion;
                                        //obj.EMUCable = item.EMUCable;
                                        //obj.AECUCable = item.AECUCable;
                                        //obj.MetaEditor_Ver = item.MetaEditor_Ver;
                                        //obj.ProjectBuilder_Ver = item.ProjectBuilder_Ver;
                                        //obj.Ascet_Ver = item.Ascet_Ver;
                                        //obj.ProjectEditor_Ver = item.ProjectEditor_Ver;


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
        public ActionResult TABLE3_GetEBData(List<string> pc, DateTime sdate, DateTime edate, int diff_UTCandLocal)
        {
            List<TABLE3_EBView> table3DataViews = new List<TABLE3_EBView>();
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
                            foreach (var pcname in pc)
                            {
                                //var pcname = pcname1.FQDN.Split('.')[0].ToUpper().Trim();
                                if (pcname != null && pcname != string.Empty && pcname.Count() > 0)
                                {

                                    var ebItems = (from item in db.EB_Cards_Pr where item.Date.Contains(date) && item.System_name.Contains(pcname) select new { item.Date, item.CreationDate, item.Project, item.Kernel_EB_Cards_EB5100, item.Kernel_EB_Cards_EB5200, item.System_name }).ToList();

                                    for (int i = 0; i < ebItems.Count(); i++)
                                    {
                                        TABLE3_EBView obj = new TABLE3_EBView();
                                        obj.Date = DateTime.Parse(ebItems.ToArray()[i].Date).ToShortDateString(); obj.CreationDate = ebItems.ToArray()[i].CreationDate.ToString().Remove(0, 10); obj.Project = ebItems.ToArray()[i].Project;
                                        obj.System_name = ebItems.ToArray()[i].System_name;

                                        using (BookingServerReplicaEntities db1 = new BookingServerReplicaEntities())
                                        {

                                            var labid = db1.LabComputersPrs.ToList().Find(x => x.FQDN.Split('.')[0].ToUpper().Contains(obj.System_name)).LabId;
                                            obj.LC_Name = db1.LabInfoes.FirstOrDefault(j => j.Id == labid).DisplayName;
                                        }
                                        obj.Kernel_EB_Cards_EB5200 = ebItems.ToArray()[i].Kernel_EB_Cards_EB5200;
                                        obj.Kernel_EB_Cards_EB5100 = ebItems.ToArray()[i].Kernel_EB_Cards_EB5100;

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
        public ActionResult GetTABLE4_ES4441Data(List<string> pc, DateTime sdate, DateTime edate, int diff_UTCandLocal)
        {
            List<TABLE4_ES4441View> table3DataViews = new List<TABLE4_ES4441View>();
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
                            foreach (var pcname in pc)
                            {
                                //var pcname = pcname1.FQDN.Split('.')[0].ToUpper().Trim();
                                if (pcname != null && pcname != string.Empty && pcname.Count() > 0)
                                {

                                    var es4441_1Items = (from item in db.ES4441_1_Pr where item.Date.Contains(date) && item.System_name.Contains(pcname) select new { item.Date, item.CreationDate, item.Project, item.Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441FWVersionMain, item.Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441FWVersionMajor, item.Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441FWVersionMinor, item.Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441PLDVersion, item.Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441SN, item.System_name }).ToList();
                                    var es4441_2Items = (from item in db.ES4441_2_Pr where item.Date.Contains(date) && item.System_name.Contains(pcname) select new { item.Date, item.CreationDate, item.Project, item.Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441HWRevision, item.Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441HWVersion, item.System_name }).ToList();

                                    var es4441_Items = (from item in db.ES4441_Pr where item.Date.Contains(date) && item.System_name.Contains(pcname) select new { item.Date, item.CreationDate, item.Project, item.Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441FWVersionMain, item.Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441FWVersionMajor, item.Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441FWVersionMinor, item.Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441PLDVersion, item.Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441SN, item.System_name, item.Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441HWRevision, item.Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441HWVersion }).ToList();

                                    for (int i = 0; i < es4441_1Items.Count(); i++)
                                    {
                                        TABLE4_ES4441View obj = new TABLE4_ES4441View();
                                        obj.Date = DateTime.Parse(es4441_1Items.ToArray()[i].Date).ToShortDateString(); obj.CreationDate = es4441_1Items.ToArray()[i].CreationDate.ToString().Remove(0, 10); obj.Project = es4441_1Items.ToArray()[i].Project;
                                        obj.System_name = es4441_1Items.ToArray()[i].System_name;

                                        using (BookingServerReplicaEntities db1 = new BookingServerReplicaEntities())
                                        {

                                            var labid = db1.LabComputersPrs.ToList().Find(x => x.FQDN.Split('.')[0].ToUpper().Contains(obj.System_name)).LabId;
                                            obj.LC_Name = db1.LabInfoes.FirstOrDefault(j => j.Id == labid).DisplayName;
                                        }
                                        obj.Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441SN = es4441_1Items.ToArray()[i].Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441SN;
                                        obj.Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441FWVersionMinor = es4441_1Items.ToArray()[i].Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441FWVersionMinor;
                                        obj.Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441FWVersionMajor = es4441_1Items.ToArray()[i].Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441FWVersionMajor;
                                        obj.Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441FWVersionMain = es4441_1Items.ToArray()[i].Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441FWVersionMain;
                                        obj.Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441HWRevision = es4441_2Items.ToArray()[i].Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441HWRevision;
                                        obj.Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441HWVersion = es4441_2Items.ToArray()[i].Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441HWVersion;

                                        table3DataViews.Add(obj);
                                    }

                                    for (int i = 0; i < es4441_Items.Count(); i++)
                                    {
                                        TABLE4_ES4441View obj = new TABLE4_ES4441View();
                                        obj.Date = DateTime.Parse(es4441_Items.ToArray()[i].Date).ToShortDateString(); obj.CreationDate = es4441_Items.ToArray()[i].CreationDate.ToString().Remove(0, 10); obj.Project = es4441_Items.ToArray()[i].Project;
                                        obj.System_name = es4441_Items.ToArray()[i].System_name;

                                        using (BookingServerReplicaEntities db1 = new BookingServerReplicaEntities())
                                        {

                                            var labid = db1.LabComputersPrs.ToList().Find(x => x.FQDN.Split('.')[0].ToUpper().Contains(obj.System_name)).LabId;
                                            obj.LC_Name = db1.LabInfoes.FirstOrDefault(j => j.Id == labid).DisplayName;
                                        }
                                        obj.Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441SN = es4441_Items.ToArray()[i].Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441SN;
                                        obj.Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441FWVersionMinor = es4441_Items.ToArray()[i].Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441FWVersionMinor;
                                        obj.Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441FWVersionMajor = es4441_Items.ToArray()[i].Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441FWVersionMajor;
                                        obj.Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441FWVersionMain = es4441_Items.ToArray()[i].Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441FWVersionMain;
                                        obj.Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441HWRevision = es4441_Items.ToArray()[i].Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441HWRevision;
                                        obj.Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441HWVersion = es4441_Items.ToArray()[i].Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441HWVersion;

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
        public ActionResult GetTABLE5_OTSOData(List<string> pc, DateTime sdate, DateTime edate, int diff_UTCandLocal)
        {
            List<TABLE5_OTSOView> table5DataViews = new List<TABLE5_OTSOView>();
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
                            foreach (var pcname in pc)
                            {
                                //var pcname = pcname1.FQDN.Split('.')[0].ToUpper().Trim();
                                if (pcname != null && pcname != string.Empty && pcname.Count() > 0)
                                {

                                    var otsoItems = (from item in db.OTSO_Pr where item.Date.Contains(date) && item.System_name.Contains(pcname) select new { item.Date, item.CreationDate, item.Project, item.Kernel_Component_IPB_TemperatureReceive_OTSO1Available, item.Kernel_Component_IPB_TemperatureReceive_OTSO2Available, item.System_name });

                                    for (int i = 0; i < otsoItems.Count(); i++)
                                    {
                                        TABLE5_OTSOView obj = new TABLE5_OTSOView();
                                        obj.Date = DateTime.Parse(otsoItems.ToArray()[i].Date).ToShortDateString(); obj.CreationDate = otsoItems.ToArray()[i].CreationDate.ToString().Remove(0, 10); obj.Project = otsoItems.ToArray()[i].Project;
                                        obj.System_name = otsoItems.ToArray()[i].System_name;

                                        using (BookingServerReplicaEntities db1 = new BookingServerReplicaEntities())
                                        {

                                            var labid = db1.LabComputersPrs.ToList().Find(x => x.FQDN.Split('.')[0].ToUpper().Contains(obj.System_name)).LabId;//Find(x => x.DisplayName.ToString() == obj.System_name.ToString()).LabId;
                                            obj.LC_Name = db1.LabInfoes.FirstOrDefault(j => j.Id == labid).DisplayName;
                                        }

                                        obj.Kernel_Component_IPB_TemperatureReceive_OTSO1Available = otsoItems.ToArray()[i].Kernel_Component_IPB_TemperatureReceive_OTSO1Available;
                                        obj.Kernel_Component_IPB_TemperatureReceive_OTSO2Available = otsoItems.ToArray()[i].Kernel_Component_IPB_TemperatureReceive_OTSO2Available;

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
        public ActionResult GetTABLE6_PowerSupplyData(List<string> pc, DateTime sdate, DateTime edate, int diff_UTCandLocal)
        {
            List<TABLE6_PowerSupplyView> table6DataViews = new List<TABLE6_PowerSupplyView>();
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
                            foreach (var pcname in pc)
                            {
                                //var pcname = pcname1.FQDN.Split('.')[0].ToUpper().Trim();
                                if (pcname != null && pcname != string.Empty && pcname.Count() > 0)
                                {

                                    var powersupplyItems = (from item in db.Power_Supply_Pr where item.Date.Contains(date) && item.System_name.Contains(pcname) select new { item.Date, item.CreationDate, item.Project, item.Kernel_Loadbox_Card_PSC_Power_Supply_Configuration_PS_Configuration, item.Kernel_Loadbox_Card_PSC_Power_Supply_Configuration_PS_Factor_ECU, item.Kernel_Loadbox_Card_PSC_Power_Supply_Configuration_PS_Factor_MR, item.Kernel_Loadbox_Card_PSC_Power_Supply_Configuration_PS_Factor_VR, item.System_name });


                                    for (int i = 0; i < powersupplyItems.Count(); i++)
                                    {
                                        TABLE6_PowerSupplyView obj = new TABLE6_PowerSupplyView();
                                        obj.System_name = powersupplyItems.ToArray()[i].System_name;
                                        obj.Date = DateTime.Parse(powersupplyItems.ToArray()[i].Date).ToShortDateString(); obj.CreationDate = powersupplyItems.ToArray()[i].CreationDate.ToString().Remove(0, 10); obj.Project = powersupplyItems.ToArray()[i].Project;
                                        using (BookingServerReplicaEntities db1 = new BookingServerReplicaEntities())
                                        {

                                            var labid = db1.LabComputersPrs.ToList().Find(x => x.FQDN.Split('.')[0].ToUpper().Contains(obj.System_name)).LabId;//Find(x => x.DisplayName.ToString() == obj.System_name.ToString()).LabId;
                                            obj.LC_Name = db1.LabInfoes.FirstOrDefault(j => j.Id == labid).DisplayName;
                                        }

                                        obj.Kernel_Loadbox_Card_PSC_Power_Supply_Configuration_PS_Configuration = powersupplyItems.ToArray()[i].Kernel_Loadbox_Card_PSC_Power_Supply_Configuration_PS_Configuration;
                                        obj.Kernel_Loadbox_Card_PSC_Power_Supply_Configuration_PS_Factor_ECU = powersupplyItems.ToArray()[i].Kernel_Loadbox_Card_PSC_Power_Supply_Configuration_PS_Factor_ECU;
                                        obj.Kernel_Loadbox_Card_PSC_Power_Supply_Configuration_PS_Factor_MR = powersupplyItems.ToArray()[i].Kernel_Loadbox_Card_PSC_Power_Supply_Configuration_PS_Factor_MR;
                                        obj.Kernel_Loadbox_Card_PSC_Power_Supply_Configuration_PS_Factor_VR = powersupplyItems.ToArray()[i].Kernel_Loadbox_Card_PSC_Power_Supply_Configuration_PS_Factor_VR;
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
        public ActionResult GetTABLE7_BOBData(List<string> pc, DateTime sdate, DateTime edate, int diff_UTCandLocal)
        {
            List<TABLE7_BOBView> table3DataViews = new List<TABLE7_BOBView>();
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
                            foreach (var pcname in pc)
                            {
                                //var pcname = pcname1.FQDN.Split('.')[0].ToUpper().Trim();
                                if (pcname != null && pcname != string.Empty && pcname.Count() > 0)
                                {
                                    var bobItems = (from item in db.BreakOutBox_Pr where item.Date.Contains(date) && item.System_name.Contains(pcname) select new { item.Date, item.CreationDate, item.Project, item.Kernel_Loadbox_Card_ECC_BreakOutBox_BOB_DeviceFunction, item.Kernel_Loadbox_Card_ECC_BreakOutBox_BOB_DeviceRevision, item.Kernel_Loadbox_Card_ECC_BreakOutBox_BOB_OptionalDescription, item.Kernel_Loadbox_Card_ECC_BreakOutBox_BOB_OrderNumber, item.Kernel_Loadbox_Card_ECC_BreakOutBox_BOB_SerialNumber, item.System_name }).ToList();

                                    for (int i = 0; i < bobItems.Count(); i++)
                                    {
                                        TABLE7_BOBView obj = new TABLE7_BOBView();
                                        obj.Date = DateTime.Parse(bobItems.ToArray()[i].Date).ToShortDateString(); obj.CreationDate = bobItems.ToArray()[i].CreationDate.ToString().Remove(0, 10); obj.Project = bobItems.ToArray()[i].Project;
                                        obj.System_name = bobItems.ToArray()[i].System_name;

                                        using (BookingServerReplicaEntities db1 = new BookingServerReplicaEntities())
                                        {

                                            var labid = db1.LabComputersPrs.ToList().Find(x => x.FQDN.Split('.')[0].ToUpper().Contains(obj.System_name)).LabId;
                                            obj.LC_Name = db1.LabInfoes.FirstOrDefault(j => j.Id == labid).DisplayName;
                                        }


                                        obj.Kernel_Loadbox_Card_ECC_BreakOutBox_BOB_SerialNumber = bobItems.ToArray()[i].Kernel_Loadbox_Card_ECC_BreakOutBox_BOB_SerialNumber;
                                        obj.Kernel_Loadbox_Card_ECC_BreakOutBox_BOB_DeviceRevision = bobItems.ToArray()[i].Kernel_Loadbox_Card_ECC_BreakOutBox_BOB_DeviceRevision;

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
        public ActionResult GetTABLE8_BOB1Data(List<string> pc, DateTime sdate, DateTime edate, int diff_UTCandLocal)
        {
            List<TABLE8_BOB1View> table4DataViews = new List<TABLE8_BOB1View>();
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
                            foreach (var pcname in pc)
                            {
                                //var pcname = pcname1.FQDN.Split('.')[0].ToUpper().Trim();
                                if (pcname != null && pcname != string.Empty && pcname.Count() > 0)
                                {
                                    var bob1Items = (from item in db.BreakOutBox_1_Pr where item.System_name.Contains(pcname) && item.Date.Contains(date) select new { item.Date, item.CreationDate, item.Project, item.Kernel_Loadbox_Card_ECC_BreakOutBox_BOB_SerialNumber, item.System_name });

                                    for (int i = 0; i < bob1Items.Count(); i++)
                                    {
                                        TABLE8_BOB1View obj = new TABLE8_BOB1View();
                                        obj.Date = DateTime.Parse(bob1Items.ToArray()[i].Date).ToShortDateString(); //obj.CreationDate = item.CreationDate.ToString().Remove(0,10); 
                                        obj.System_name = bob1Items.ToArray()[i].System_name;

                                        using (BookingServerReplicaEntities db1 = new BookingServerReplicaEntities())
                                        {

                                            var labid = db1.LabComputersPrs.ToList().Find(x => x.FQDN.Split('.')[0].ToUpper().Contains(obj.System_name)).LabId;//Find(x => x.DisplayName.ToString() == obj.System_name.ToString()).LabId;
                                            obj.LC_Name = db1.LabInfoes.FirstOrDefault(j => j.Id == labid).DisplayName;
                                        }
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
        public ActionResult GetTABLE9_IBData(List<string> pc, DateTime sdate, DateTime edate, int diff_UTCandLocal)
        {
            List<TABLE9_IBView> table5DataViews = new List<TABLE9_IBView>();
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
                            foreach (var pcname in pc)
                            {
                                //var pcname = pcname1.FQDN.Split('.')[0].ToUpper().Trim();
                                if (pcname != null && pcname != string.Empty && pcname.Count() > 0)
                                {

                                    var ibItems = (from item in db.IB_Card_Pr where item.Date.Contains(date) && item.System_name.Contains(pcname) select new { item.Date, item.CreationDate, item.Project, item.ModelSpecific_EID_NET_CAN_Settings_CAN_Settings_rtpc_ixxat_config, item.System_name });

                                    for (int i = 0; i < ibItems.Count(); i++)
                                    {
                                        TABLE9_IBView obj = new TABLE9_IBView();
                                        obj.Date = DateTime.Parse(ibItems.ToArray()[i].Date).ToShortDateString(); obj.CreationDate = ibItems.ToArray()[i].CreationDate.ToString().Remove(0, 10); obj.Project = ibItems.ToArray()[i].Project;
                                        obj.System_name = ibItems.ToArray()[i].System_name;

                                        using (BookingServerReplicaEntities db1 = new BookingServerReplicaEntities())
                                        {

                                            var labid = db1.LabComputersPrs.ToList().Find(x => x.FQDN.Split('.')[0].ToUpper().Contains(obj.System_name)).LabId;//Find(x => x.DisplayName.ToString() == obj.System_name.ToString()).LabId;
                                            LabInfo labInfo = new LabInfo();

                                            obj.LC_Name = db1.LabInfoes.FirstOrDefault(j => j.Id == labid).DisplayName;
                                        }

                                        db.Database.CommandTimeout = 500;

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
        public ActionResult GetTABLE10_IXXATData(List<string> pc, DateTime sdate, DateTime edate, int diff_UTCandLocal)
        {
            List<TABLE10_IXXAT2View> table4DataViews = new List<TABLE10_IXXAT2View>();
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
                            foreach (var pcname in pc)
                            {
                                //var pcname = pcname1.FQDN.Split('.')[0].ToUpper().Trim();
                                if (pcname != null && pcname != string.Empty && pcname.Count() > 0)
                                {
                                    var ixxatItems = (from item in db.IXXAT_Config_Pr where item.Date.Contains(date) && item.System_name.Contains(pcname) select new { item.Date, item.CreationDate, item.Project, item.ModelSpecific_EID_NET_CAN_Settings_CAN_Settings_rtpc_ixxat_config, item.System_name });


                                    for (int i = 0; i < ixxatItems.Count(); i++)
                                    {
                                        TABLE10_IXXAT2View obj = new TABLE10_IXXAT2View();
                                        obj.Date = DateTime.Parse(ixxatItems.ToArray()[i].Date).ToShortDateString(); obj.CreationDate = ixxatItems.ToArray()[i].CreationDate.ToString().Remove(0, 10); obj.Project = ixxatItems.ToArray()[i].Project;
                                        obj.System_name = ixxatItems.ToArray()[i].System_name;

                                        using (BookingServerReplicaEntities db1 = new BookingServerReplicaEntities())
                                        {

                                            var labid = db1.LabComputersPrs.ToList().Find(x => x.FQDN.Split('.')[0].ToUpper().Contains(obj.System_name)).LabId;//Find(x => x.DisplayName.ToString() == obj.System_name.ToString()).LabId;
                                            obj.LC_Name = db1.LabInfoes.FirstOrDefault(j => j.Id == labid).DisplayName;
                                        }
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

        //[HttpPost]
        //public ActionResult GetTABLE10_IXXATData(List<string> pc, DateTime sdate, DateTime edate, int diff_UTCandLocal)
        //{
        //    List<TABLE10_IXXAT2View> table4DataViews = new List<TABLE10_IXXAT2View>();
        //    if (pc != null && pc.Count() > 0)
        //    {
        //        string pclist = string.Empty;
        //        try
        //        {
        //            using (PromasterImport_HWInfoEntities db = new PromasterImport_HWInfoEntities())
        //            {
        //                db.Database.CommandTimeout = 500;
        //                string date = string.Empty;
        //                foreach (var p in pc)
        //                {
        //                    pclist += ("'" + p.Trim() + "',");
        //                    //pclist.Add(p.FQDN.Split('.')[0].ToUpper().Trim());
        //                }

        //                pclist = pclist.Remove(pclist.Length - 1, 1);
        //                connection();
        //                DataTable dtable = new DataTable();

        //                string Query = " Select " + ConfigurationManager.AppSettings["BookingServerReplica"] + " [LabInfo].DisplayName, [System_name] , Date  ,[ModelSpecific_EID_NET_CAN_Settings_CAN_Settings_rtpc_ixxat_config] ,[Project],CreationDate from [IXXAT_Config_Pr] inner join  " + ConfigurationManager.AppSettings["BookingServerReplica"] + "LabComputersPr on substring( " + ConfigurationManager.AppSettings["BookingServerReplica"] + "LabComputersPr.FQDN, 1, len(System_name)) = [IXXAT_Config_Pr].System_name inner join " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo] on " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo].id = LabComputersPr.LabId where System_name in (" + pclist + ") AND CONVERT(DATETIME, Date) > = '" + sdate + "' AND CONVERT(DATETIME, Date) < = '" + edate + "'";

        //                OpenConnection();
        //                SqlCommand cmd = new SqlCommand(Query, con);
        //                SqlDataAdapter da = new SqlDataAdapter(cmd);
        //                da.Fill(dtable);
        //                CloseConnection();

        //                foreach (DataRow row in dtable.Rows)
        //                {
        //                    TABLE10_IXXAT2View obj = new TABLE10_IXXAT2View();
        //                    obj.System_name = row["System_name"].ToString(); obj.Project = row["Project"].ToString();
        //                    obj.LC_Name = row["DisplayName"].ToString();
        //                    obj.Date = row["Date"].ToString(); obj.CreationDate = DateTime.Parse(row["CreationDate"].ToString()).AddMinutes(diff_UTCandLocal).ToLongTimeString();
        //                    obj.ModelSpecific_EID_NET_CAN_Settings_CAN_Settings_rtpc_ixxat_config = row["ModelSpecific_EID_NET_CAN_Settings_CAN_Settings_rtpc_ixxat_config"].ToString();
        //                    table4DataViews.Add(obj);
        //                }
        //                //hwDataViews = hwDataViews.OrderByDescending(x => x.Date).GroupBy(x => new { x.EEPName, x.Date, x.System_name }).Select(x => x.FirstOrDefault()).OrderBy(x => x.Date).ToList();


        //                var result = Json(new { data = table4DataViews }, JsonRequestBehavior.AllowGet); result.MaxJsonLength = int.MaxValue; return result;
        //            }
        //        }
        //        catch (Exception Ex)
        //        {
        //            return View("Unable to retrieve data from database. Please try again later!!");
        //        }
        //    }
        //    else
        //    {
        //        var result = Json(new { success = false, message = "Try again" }, JsonRequestBehavior.AllowGet); result.MaxJsonLength = int.MaxValue; return result;
        //    }
        //}



        [HttpPost]
        public ActionResult GetTABLE11_APBData(List<string> pc, DateTime sdate, DateTime edate, int diff_UTCandLocal)
        {
            List<TABLE11_APBView> table3DataViews = new List<TABLE11_APBView>();
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
                            foreach (var pcname in pc)
                            {
                                //var pcname = pcname1.FQDN.Split('.')[0].ToUpper().Trim();
                                if (pcname != null && pcname != string.Empty && pcname.Count() > 0)
                                {

                                    var apbItems = (from item in db.APB_Pr where item.Date.Contains(date) && item.System_name.Contains(pcname) select new { item.Date, item.CreationDate, item.Project, item.ModelSpecific_EID_Switch_APB_APB_Module_GIO2_Signature_GIO2, item.System_name }).ToList();


                                    for (int i = 0; i < apbItems.Count(); i++)
                                    {
                                        TABLE11_APBView obj = new TABLE11_APBView();
                                        obj.Date = DateTime.Parse(apbItems.ToArray()[i].Date).ToShortDateString(); obj.CreationDate = apbItems.ToArray()[i].CreationDate.ToString().Remove(0, 10); obj.Project = apbItems.ToArray()[i].Project;
                                        obj.System_name = apbItems.ToArray()[i].System_name;

                                        using (BookingServerReplicaEntities db1 = new BookingServerReplicaEntities())
                                        {

                                            var labid = db1.LabComputersPrs.ToList().Find(x => x.FQDN.Split('.')[0].ToUpper().Contains(obj.System_name)).LabId;
                                            obj.LC_Name = db1.LabInfoes.FirstOrDefault(j => j.Id == labid).DisplayName;
                                        }

                                        obj.ModelSpecific_EID_Switch_APB_APB_Module_GIO2_Signature_GIO2 = apbItems.ToArray()[i].ModelSpecific_EID_Switch_APB_APB_Module_GIO2_Signature_GIO2;

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
        public ActionResult GetTABLE12_HAPData(List<string> pc, DateTime sdate, DateTime edate, int diff_UTCandLocal)
        {
            List<TABLE12_HAPView> table4DataViews = new List<TABLE12_HAPView>();
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
                            foreach (var pcname in pc)
                            {
                                //var pcname = pcname1.FQDN.Split('.')[0].ToUpper().Trim();
                                if (pcname != null && pcname != string.Empty && pcname.Count() > 0)
                                {

                                    var hapItems = (from item in db.Harnesadapter_Pr orderby item.Date where item.System_name.Contains(pcname) && item.Date.Contains(date) select new { item.Date, item.CreationDate, item.Project, item.Kernel_Loadbox_Card_ECC_Harnesadapter_HAP_DeviceFunction, item.Kernel_Loadbox_Card_ECC_Harnesadapter_HAP_DeviceRevision, item.Kernel_Loadbox_Card_ECC_Harnesadapter_HAP_OptionalDescription, item.Kernel_Loadbox_Card_ECC_Harnesadapter_HAP_OrderNumber, item.Kernel_Loadbox_Card_ECC_Harnesadapter_HAP_SerialNumber, item.System_name });

                                    for (int i = 0; i < hapItems.Count(); i++)
                                    {
                                        TABLE12_HAPView obj = new TABLE12_HAPView();
                                        obj.Date = DateTime.Parse(hapItems.ToArray()[i].Date).ToShortDateString(); obj.CreationDate = hapItems.ToArray()[i].CreationDate.ToString().Remove(0, 10); obj.Project = hapItems.ToArray()[i].Project;
                                        obj.System_name = hapItems.ToArray()[i].System_name;

                                        using (BookingServerReplicaEntities db1 = new BookingServerReplicaEntities())
                                        {

                                            var labid = db1.LabComputersPrs.ToList().Find(x => x.FQDN.Split('.')[0].ToUpper().Contains(obj.System_name)).LabId;//Find(x => x.DisplayName.ToString() == obj.System_name.ToString()).LabId;
                                            obj.LC_Name = db1.LabInfoes.FirstOrDefault(j => j.Id == labid).DisplayName;
                                        }

                                        obj.Kernel_Loadbox_Card_ECC_Harnesadapter_HAP_SerialNumber = hapItems.ToArray()[i].Kernel_Loadbox_Card_ECC_Harnesadapter_HAP_SerialNumber;
                                        obj.Kernel_Loadbox_Card_ECC_Harnesadapter_HAP_DeviceRevision = hapItems.ToArray()[i].Kernel_Loadbox_Card_ECC_Harnesadapter_HAP_DeviceRevision;
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
        public ActionResult GetTABLE13_HAP1Data(List<string> pc, DateTime sdate, DateTime edate, int diff_UTCandLocal)
        {
            List<TABLE13_HAP1View> table3DataViews = new List<TABLE13_HAP1View>();
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
                            foreach (var pcname in pc)
                            {
                                //var pcname = pcname1.FQDN.Split('.')[0].ToUpper().Trim();
                                if (pcname != null && pcname != string.Empty && pcname.Count() > 0)
                                {

                                    var hap1Items = (from item in db.Harnesadapter_1_Pr where item.Date.Contains(date) && item.System_name.Contains(pcname) select new { item.Date, item.CreationDate, item.Project, item.Kernel_Loadbox_Card_ECC_Harnesadapter_HAP_SerialNumber, item.System_name }).ToList();

                                    for (int i = 0; i < hap1Items.Count(); i++)
                                    {
                                        TABLE13_HAP1View obj = new TABLE13_HAP1View();
                                        obj.Date = DateTime.Parse(hap1Items.ToArray()[i].Date).ToShortDateString(); //obj.CreationDate = item.CreationDate.ToString().Remove(0,10); 
                                        obj.System_name = hap1Items.ToArray()[i].System_name;

                                        using (BookingServerReplicaEntities db1 = new BookingServerReplicaEntities())
                                        {

                                            var labid = db1.LabComputersPrs.ToList().Find(x => x.FQDN.Split('.')[0].ToUpper().Contains(obj.System_name)).LabId;
                                            obj.LC_Name = db1.LabInfoes.FirstOrDefault(j => j.Id == labid).DisplayName;
                                        }

                                        obj.Kernel_Loadbox_Card_ECC_Harnesadapter_HAP_SerialNumber = hap1Items.ToArray()[i].Kernel_Loadbox_Card_ECC_Harnesadapter_HAP_SerialNumber;

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
        public ActionResult GetTABLE14_CableData(List<string> pc, DateTime sdate, DateTime edate, int diff_UTCandLocal)
        {
            List<TABLE14_CableView> table3DataViews = new List<TABLE14_CableView>();
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
                            foreach (var pcname in pc)
                            {
                                //var pcname = pcname1.FQDN.Split('.')[0].ToUpper().Trim();
                                if (pcname != null && pcname != string.Empty && pcname.Count() > 0)
                                {
                                    var cableItems = (from item in db.Cable_Pr where item.Date.Contains(date) && item.System_name.Contains(pcname) select new { item.Date, item.CreationDate, item.Project, item.Kernel_Loadbox_Card_ECC_Cable_CABLE_DeviceFunction, item.Kernel_Loadbox_Card_ECC_Cable_CABLE_DeviceRevision, item.Kernel_Loadbox_Card_ECC_Cable_CABLE_OptionalDescription, item.Kernel_Loadbox_Card_ECC_Cable_CABLE_OrderNumber, item.Kernel_Loadbox_Card_ECC_Cable_CABLE_SerialNumber, item.System_name }).ToList();

                                    for (int i = 0; i < cableItems.Count(); i++)
                                    {
                                        TABLE14_CableView obj = new TABLE14_CableView();
                                        obj.Date = DateTime.Parse(cableItems.ToArray()[i].Date).ToShortDateString(); obj.CreationDate = cableItems.ToArray()[i].CreationDate.ToString().Remove(0, 10); obj.Project = cableItems.ToArray()[i].Project;
                                        obj.System_name = cableItems.ToArray()[i].System_name;

                                        using (BookingServerReplicaEntities db1 = new BookingServerReplicaEntities())
                                        {

                                            var labid = db1.LabComputersPrs.ToList().Find(x => x.FQDN.Split('.')[0].ToUpper().Contains(obj.System_name)).LabId;
                                            obj.LC_Name = db1.LabInfoes.FirstOrDefault(j => j.Id == labid).DisplayName;
                                        }

                                        obj.Kernel_Loadbox_Card_ECC_Cable_CABLE_SerialNumber = cableItems.ToArray()[i].Kernel_Loadbox_Card_ECC_Cable_CABLE_SerialNumber;
                                        obj.Kernel_Loadbox_Card_ECC_Cable_CABLE_DeviceRevision = cableItems.ToArray()[i].Kernel_Loadbox_Card_ECC_Cable_CABLE_DeviceRevision;

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
        public ActionResult GetTABLE15_Cable1Data(List<string> pc, DateTime sdate, DateTime edate, int diff_UTCandLocal)
        {
            List<TABLE15_Cable1View> table4DataViews = new List<TABLE15_Cable1View>();
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
                            foreach (var pcname in pc)
                            {
                                //var pcname = pcname1.FQDN.Split('.')[0].ToUpper().Trim();
                                if (pcname != null && pcname != string.Empty && pcname.Count() > 0)
                                {

                                    var cable1Items = (from item in db.Cable_1_Pr where item.Date.Contains(date) && item.System_name.Contains(pcname) select new { item.Date, item.CreationDate, item.Project, item.Kernel_Loadbox_Card_ECC_Cable_CABLE_SerialNumber, item.System_name });

                                    for (int i = 0; i < cable1Items.Count(); i++)
                                    {
                                        TABLE15_Cable1View obj = new TABLE15_Cable1View();
                                        obj.Date = DateTime.Parse(cable1Items.ToArray()[i].Date).ToShortDateString(); //obj.CreationDate = item.CreationDate.ToString().Remove(0,10); 
                                        obj.System_name = cable1Items.ToArray()[i].System_name;

                                        using (BookingServerReplicaEntities db1 = new BookingServerReplicaEntities())
                                        {

                                            var labid = db1.LabComputersPrs.ToList().Find(x => x.FQDN.Split('.')[0].ToUpper().Contains(obj.System_name)).LabId;//Find(x => x.DisplayName.ToString() == obj.System_name.ToString()).LabId;
                                            obj.LC_Name = db1.LabInfoes.FirstOrDefault(j => j.Id == labid).DisplayName;
                                        }

                                        obj.Kernel_Loadbox_Card_ECC_Cable_CABLE_SerialNumber = cable1Items.ToArray()[i].Kernel_Loadbox_Card_ECC_Cable_CABLE_SerialNumber;
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
        public ActionResult GetTABLE16_ECCData(List<string> pc, DateTime sdate, DateTime edate, int diff_UTCandLocal)
        {
            List<TABLE16_ECCView> table5DataViews = new List<TABLE16_ECCView>();
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
                            foreach (var pcname in pc)
                            {
                                //var pcname = pcname1.FQDN.Split('.')[0].ToUpper().Trim();
                                if (pcname != null && pcname != string.Empty && pcname.Count() > 0)
                                {

                                    var eccItems = (from item in db.ECC_Card_Pr where item.Date.Contains(date) && item.System_name.Contains(pcname) select new { item.Date, item.CreationDate, item.Project, item.Kernel_Loadbox_Card_ECC_BuildVersion, item.Kernel_Loadbox_Card_ECC_Calibration_Day, item.Kernel_Loadbox_Card_ECC_Calibration_Month, item.Kernel_Loadbox_Card_ECC_Calibration_Year, item.Kernel_Loadbox_Card_ECC_CARD_BoardRevision, item.Kernel_Loadbox_Card_ECC_CARD_BoardType, item.Kernel_Loadbox_Card_ECC_CARD_Description, item.Kernel_Loadbox_Card_ECC_CARD_VER_TAG_ARRAY, item.Kernel_Loadbox_Card_ECC_Day_of_Creation, item.Kernel_Loadbox_Card_ECC_Firmware_Card_Major, item.Kernel_Loadbox_Card_ECC_Firmware_Card_Minor, item.Kernel_Loadbox_Card_ECC_Firmware_FPGA_Major, item.Kernel_Loadbox_Card_ECC_Firmware_FPGA_Minor, item.Kernel_Loadbox_Card_ECC_Month_of_Creation, item.Kernel_Loadbox_Card_ECC_Next_Calibration_Day, item.Kernel_Loadbox_Card_ECC_Next_Calibration_Month, item.Kernel_Loadbox_Card_ECC_Next_Calibration_Year, item.Kernel_Loadbox_Card_ECC_PowerOnTime, item.Kernel_Loadbox_Card_ECC_RepairCounter, item.Kernel_Loadbox_Card_ECC_Serial_No, item.Kernel_Loadbox_Card_ECC_Year_of_Creation, item.System_name });

                                    for (int i = 0; i < eccItems.Count(); i++)
                                    {
                                        TABLE16_ECCView obj = new TABLE16_ECCView();
                                        obj.Date = DateTime.Parse(eccItems.ToArray()[i].Date).ToShortDateString(); obj.CreationDate = eccItems.ToArray()[i].CreationDate.ToString().Remove(0, 10); obj.Project = eccItems.ToArray()[i].Project;
                                        obj.System_name = eccItems.ToArray()[i].System_name;

                                        using (BookingServerReplicaEntities db1 = new BookingServerReplicaEntities())
                                        {

                                            var labid = db1.LabComputersPrs.ToList().Find(x => x.FQDN.Split('.')[0].ToUpper().Contains(obj.System_name)).LabId;//Find(x => x.DisplayName.ToString() == obj.System_name.ToString()).LabId;
                                            LabInfo labInfo = new LabInfo();

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
        public ActionResult GetTABLE17_GIO1Data(List<string> pc, DateTime sdate, DateTime edate, int diff_UTCandLocal)
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
                            foreach (var pcname in pc)
                            {
                                //var pcname = pcname1.FQDN.Split('.')[0].ToUpper().Trim();
                                if (pcname != null && pcname != string.Empty && pcname.Count() > 0)
                                {

                                    var gio1Items = (from item in db.GIO1_Card_Pr where item.Date.Contains(date) && item.System_name.Contains(pcname) select new { item.Date, item.CreationDate, item.Project, item.Kernel_Loadbox_Card_GIO1_BuildVersion, item.Kernel_Loadbox_Card_GIO1_Calibration_Day, item.Kernel_Loadbox_Card_GIO1_Calibration_Month, item.Kernel_Loadbox_Card_GIO1_Calibration_Year, item.Kernel_Loadbox_Card_GIO1_CARD_BoardRevision, item.Kernel_Loadbox_Card_GIO1_CARD_BoardType, item.Kernel_Loadbox_Card_GIO1_CARD_Description, item.Kernel_Loadbox_Card_GIO1_CARD_VER_TAG_ARRAY, item.Kernel_Loadbox_Card_GIO1_Day_of_Creation, item.Kernel_Loadbox_Card_GIO1_Firmware_Card_Major, item.Kernel_Loadbox_Card_GIO1_Firmware_Card_Minor, item.Kernel_Loadbox_Card_GIO1_Firmware_FPGA_Major, item.Kernel_Loadbox_Card_GIO1_Firmware_FPGA_Minor, item.Kernel_Loadbox_Card_GIO1_Month_of_Creation, item.Kernel_Loadbox_Card_GIO1_Next_Calibration_Day, item.Kernel_Loadbox_Card_GIO1_Next_Calibration_Month, item.Kernel_Loadbox_Card_GIO1_Next_Calibration_Year, item.Kernel_Loadbox_Card_GIO1_PowerOnTime, item.Kernel_Loadbox_Card_GIO1_RepairCounter, item.Kernel_Loadbox_Card_GIO1_Serial_No, item.Kernel_Loadbox_Card_GIO1_Year_of_Creation, item.System_name });

                                    for (int i = 0; i < gio1Items.Count(); i++)
                                    {
                                        TABLE6_GIO1View obj = new TABLE6_GIO1View();
                                        obj.Date = DateTime.Parse(gio1Items.ToArray()[i].Date).ToShortDateString(); obj.CreationDate = gio1Items.ToArray()[i].CreationDate.ToString().Remove(0, 10); obj.Project = gio1Items.ToArray()[i].Project;
                                        obj.System_name = gio1Items.ToArray()[i].System_name;

                                        using (BookingServerReplicaEntities db1 = new BookingServerReplicaEntities())
                                        {

                                            var labid = db1.LabComputersPrs.ToList().Find(x => x.FQDN.Split('.')[0].ToUpper().Contains(obj.System_name)).LabId;//Find(x => x.DisplayName.ToString() == obj.System_name.ToString()).LabId;
                                            LabInfo labInfo = new LabInfo();

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
        public ActionResult GetTABLE18_GIO2Data(List<string> pc, DateTime sdate, DateTime edate, int diff_UTCandLocal)
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
                            foreach (var pcname in pc)
                            {
                                //var pcname = pcname1.FQDN.Split('.')[0].ToUpper().Trim();
                                {
                                    if (pcname != null && pcname != string.Empty && pcname.Count() > 0)
                                    {


                                        var gio2Items = (from item in db.GIO2_Card_Pr where item.Date.Contains(date) && item.System_name.Contains(pcname) select new { item.Date, item.CreationDate, item.Project, item.Kernel_Loadbox_Card_GIO2_BuildVersion, item.Kernel_Loadbox_Card_GIO2_Calibration_Day, item.Kernel_Loadbox_Card_GIO2_Calibration_Month, item.Kernel_Loadbox_Card_GIO2_Calibration_Year, item.Kernel_Loadbox_Card_GIO2_CARD_BoardRevision, item.Kernel_Loadbox_Card_GIO2_CARD_BoardType, item.Kernel_Loadbox_Card_GIO2_CARD_Description, item.Kernel_Loadbox_Card_GIO2_CARD_VER_TAG_ARRAY, item.Kernel_Loadbox_Card_GIO2_Day_of_Creation, item.Kernel_Loadbox_Card_GIO2_Firmware_Card_Major, item.Kernel_Loadbox_Card_GIO2_Firmware_Card_Minor, item.Kernel_Loadbox_Card_GIO2_Firmware_FPGA_Major, item.Kernel_Loadbox_Card_GIO2_Firmware_FPGA_Minor, item.Kernel_Loadbox_Card_GIO2_Month_of_Creation, item.Kernel_Loadbox_Card_GIO2_Next_Calibration_Day, item.Kernel_Loadbox_Card_GIO2_Next_Calibration_Month, item.Kernel_Loadbox_Card_GIO2_Next_Calibration_Year, item.Kernel_Loadbox_Card_GIO2_PowerOnTime, item.Kernel_Loadbox_Card_GIO2_RepairCounter, item.Kernel_Loadbox_Card_GIO2_Serial_No, item.Kernel_Loadbox_Card_GIO2_Year_of_Creation, item.System_name });

                                        for (int i = 0; i < gio2Items.Count(); i++)
                                        {
                                            TABLE7_GIO2View obj = new TABLE7_GIO2View();
                                            obj.Date = DateTime.Parse(gio2Items.ToArray()[i].Date).ToShortDateString(); obj.CreationDate = gio2Items.ToArray()[i].CreationDate.ToString().Remove(0, 10); obj.Project = gio2Items.ToArray()[i].Project;
                                            obj.System_name = gio2Items.ToArray()[i].System_name;

                                            using (BookingServerReplicaEntities db1 = new BookingServerReplicaEntities())
                                            {

                                                var labid = db1.LabComputersPrs.ToList().Find(x => x.FQDN.Split('.')[0].ToUpper().Contains(obj.System_name)).LabId;//Find(x => x.DisplayName.ToString() == obj.System_name.ToString()).LabId;
                                                LabInfo labInfo = new LabInfo();

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
        public ActionResult GetTABLE19_LDUData(List<string> pc, DateTime sdate, DateTime edate, int diff_UTCandLocal)
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
                            foreach (var pcname in pc)
                            {
                                //var pcname = pcname1.FQDN.Split('.')[0].ToUpper().Trim();
                                if (pcname != null && pcname != string.Empty && pcname.Count() > 0)
                                {

                                    var lduItems = (from item in db.LDU_Card_Pr where item.Date.Contains(date) && item.System_name.Contains(pcname) select new { item.Date, item.CreationDate, item.Project, item.Kernel_Loadbox_Card_LDU_BuildVersion, item.Kernel_Loadbox_Card_LDU_Calibration_Day, item.Kernel_Loadbox_Card_LDU_Calibration_Month, item.Kernel_Loadbox_Card_LDU_Calibration_Year, item.Kernel_Loadbox_Card_LDU_CARD_BoardRevision, item.Kernel_Loadbox_Card_LDU_CARD_BoardType, item.Kernel_Loadbox_Card_LDU_CARD_Description, item.Kernel_Loadbox_Card_LDU_CARD_VER_TAG_ARRAY, item.Kernel_Loadbox_Card_LDU_Day_of_Creation, item.Kernel_Loadbox_Card_LDU_Firmware_Card_Major, item.Kernel_Loadbox_Card_LDU_Firmware_Card_Minor, item.Kernel_Loadbox_Card_LDU_Firmware_FPGA_Major, item.Kernel_Loadbox_Card_LDU_Firmware_FPGA_Minor, item.Kernel_Loadbox_Card_LDU_Month_of_Creation, item.Kernel_Loadbox_Card_LDU_Next_Calibration_Day, item.Kernel_Loadbox_Card_LDU_Next_Calibration_Month, item.Kernel_Loadbox_Card_LDU_Next_Calibration_Year, item.Kernel_Loadbox_Card_LDU_PowerOnTime, item.Kernel_Loadbox_Card_LDU_RepairCounter, item.Kernel_Loadbox_Card_LDU_Serial_No, item.Kernel_Loadbox_Card_LDU_Year_of_Creation, item.System_name });

                                    for (int i = 0; i < lduItems.Count(); i++)
                                    {
                                        TABLE8_LDUView obj = new TABLE8_LDUView();
                                        obj.Date = DateTime.Parse(lduItems.ToArray()[i].Date).ToShortDateString(); obj.CreationDate = lduItems.ToArray()[i].CreationDate.ToString().Remove(0, 10); obj.Project = lduItems.ToArray()[i].Project;
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
        public ActionResult GetTABLE20_PSCData(List<string> pc, DateTime sdate, DateTime edate, int diff_UTCandLocal)
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
                            foreach (var pcname in pc)
                            {
                                //var pcname = pcname1.FQDN.Split('.')[0].ToUpper().Trim();
                                if (pcname != null && pcname != string.Empty && pcname.Count() > 0)
                                {

                                    var pscItems = (from item in db.PSC_Card_Pr where item.Date.Contains(date) && item.System_name.Contains(pcname) select new { item.Date, item.CreationDate, item.Project, item.Kernel_Loadbox_Card_PSC_BuildVersion, item.Kernel_Loadbox_Card_PSC_Calibration_Day, item.Kernel_Loadbox_Card_PSC_Calibration_Month, item.Kernel_Loadbox_Card_PSC_Calibration_Year, item.Kernel_Loadbox_Card_PSC_CARD_BoardRevision, item.Kernel_Loadbox_Card_PSC_CARD_BoardType, item.Kernel_Loadbox_Card_PSC_CARD_Description, item.Kernel_Loadbox_Card_PSC_CARD_VER_TAG_ARRAY, item.Kernel_Loadbox_Card_PSC_Day_of_Creation, item.Kernel_Loadbox_Card_PSC_Firmware_Card_Major, item.Kernel_Loadbox_Card_PSC_Firmware_Card_Minor, item.Kernel_Loadbox_Card_PSC_Firmware_FPGA_Major, item.Kernel_Loadbox_Card_PSC_Firmware_FPGA_Minor, item.Kernel_Loadbox_Card_PSC_Month_of_Creation, item.Kernel_Loadbox_Card_PSC_Next_Calibration_Day, item.Kernel_Loadbox_Card_PSC_Next_Calibration_Month, item.Kernel_Loadbox_Card_PSC_Next_Calibration_Year, item.Kernel_Loadbox_Card_PSC_PowerOnTime, item.Kernel_Loadbox_Card_PSC_RepairCounter, item.Kernel_Loadbox_Card_PSC_Serial_No, item.Kernel_Loadbox_Card_PSC_Year_of_Creation, item.System_name });


                                    for (int i = 0; i < pscItems.Count(); i++)
                                    {
                                        TABLE9_PSCView obj = new TABLE9_PSCView();
                                        obj.Date = DateTime.Parse(pscItems.ToArray()[i].Date).ToShortDateString(); obj.CreationDate = pscItems.ToArray()[i].CreationDate.ToString().Remove(0, 10); obj.Project = pscItems.ToArray()[i].Project;
                                        obj.System_name = pscItems.ToArray()[i].System_name;

                                        using (BookingServerReplicaEntities db1 = new BookingServerReplicaEntities())
                                        {

                                            var labid = db1.LabComputersPrs.ToList().Find(x => x.FQDN.Split('.')[0].ToUpper().Contains(obj.System_name)).LabId;//Find(x => x.DisplayName.ToString() == obj.System_name.ToString()).LabId;
                                            LabInfo labInfo = new LabInfo();

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
        public ActionResult GetTABLE21_VSCData(List<string> pc, DateTime sdate, DateTime edate, int diff_UTCandLocal)
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
                            foreach (var pcname in pc)
                            {
                                //var pcname = pcname1.FQDN.Split('.')[0].ToUpper().Trim();
                                if (pcname != null && pcname != string.Empty && pcname.Count() > 0)
                                {

                                    var vscItems = (from item in db.VSC_Card_Pr where item.Date.Contains(date) && item.System_name.Contains(pcname) select new { item.Date, item.CreationDate, item.Project, item.Kernel_Loadbox_Card_VSC_BuildVersion, item.Kernel_Loadbox_Card_VSC_Calibration_Day, item.Kernel_Loadbox_Card_VSC_Calibration_Month, item.Kernel_Loadbox_Card_VSC_Calibration_Year, item.Kernel_Loadbox_Card_VSC_CARD_BoardRevision, item.Kernel_Loadbox_Card_VSC_CARD_BoardType, item.Kernel_Loadbox_Card_VSC_CARD_Description, item.Kernel_Loadbox_Card_VSC_CARD_VER_TAG_ARRAY, item.Kernel_Loadbox_Card_VSC_Day_of_Creation, item.Kernel_Loadbox_Card_VSC_Firmware_Card_Major, item.Kernel_Loadbox_Card_VSC_Firmware_Card_Minor, item.Kernel_Loadbox_Card_VSC_Firmware_FPGA_Major, item.Kernel_Loadbox_Card_VSC_Firmware_FPGA_Minor, item.Kernel_Loadbox_Card_VSC_Month_of_Creation, item.Kernel_Loadbox_Card_VSC_Next_Calibration_Day, item.Kernel_Loadbox_Card_VSC_Next_Calibration_Month, item.Kernel_Loadbox_Card_VSC_Next_Calibration_Year, item.Kernel_Loadbox_Card_VSC_PowerOnTime, item.Kernel_Loadbox_Card_VSC_RepairCounter, item.Kernel_Loadbox_Card_VSC_Serial_No, item.Kernel_Loadbox_Card_VSC_Year_of_Creation, item.System_name });

                                    for (int i = 0; i < vscItems.Count(); i++)
                                    {
                                        TABLE10_VSCView obj = new TABLE10_VSCView();
                                        obj.Date = DateTime.Parse(vscItems.ToArray()[i].Date).ToShortDateString(); obj.CreationDate = vscItems.ToArray()[i].CreationDate.ToString().Remove(0, 10); obj.Project = vscItems.ToArray()[i].Project;
                                        obj.System_name = vscItems.ToArray()[i].System_name;

                                        using (BookingServerReplicaEntities db1 = new BookingServerReplicaEntities())
                                        {

                                            var labid = db1.LabComputersPrs.ToList().Find(x => x.FQDN.Split('.')[0].ToUpper().Contains(obj.System_name)).LabId;//Find(x => x.DisplayName.ToString() == obj.System_name.ToString()).LabId;
                                            LabInfo labInfo = new LabInfo();

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
        public ActionResult GetTABLE22_WSSData(List<string> pc, DateTime sdate, DateTime edate, int diff_UTCandLocal)
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
                            foreach (var pcname in pc)
                            {
                                //var pcname = pcname1.FQDN.Split('.')[0].ToUpper().Trim();
                                if (pcname != null && pcname != string.Empty && pcname.Count() > 0)
                                {

                                    var wssItems = (from item in db.WSS_Card_Pr where item.Date.Contains(date) && item.System_name.Contains(pcname) select new { item.Date, item.CreationDate, item.Project, item.Kernel_Loadbox_Card_WSS_BuildVersion, item.Kernel_Loadbox_Card_WSS_Calibration_Day, item.Kernel_Loadbox_Card_WSS_Calibration_Month, item.Kernel_Loadbox_Card_WSS_Calibration_Year, item.Kernel_Loadbox_Card_WSS_CARD_BoardRevision, item.Kernel_Loadbox_Card_WSS_CARD_BoardType, item.Kernel_Loadbox_Card_WSS_CARD_Description, item.Kernel_Loadbox_Card_WSS_CARD_VER_TAG_ARRAY, item.Kernel_Loadbox_Card_WSS_Day_of_Creation, item.Kernel_Loadbox_Card_WSS_Firmware_Card_Major, item.Kernel_Loadbox_Card_WSS_Firmware_Card_Minor, item.Kernel_Loadbox_Card_WSS_Firmware_FPGA_Major, item.Kernel_Loadbox_Card_WSS_Firmware_FPGA_Minor, item.Kernel_Loadbox_Card_WSS_Month_of_Creation, item.Kernel_Loadbox_Card_WSS_Next_Calibration_Day, item.Kernel_Loadbox_Card_WSS_Next_Calibration_Month, item.Kernel_Loadbox_Card_WSS_Next_Calibration_Year, item.Kernel_Loadbox_Card_WSS_PowerOnTime, item.Kernel_Loadbox_Card_WSS_RepairCounter, item.Kernel_Loadbox_Card_WSS_Serial_No, item.Kernel_Loadbox_Card_WSS_Year_of_Creation, item.System_name });

                                    for (int i = 0; i < wssItems.Count(); i++)
                                    {
                                        TABLE11_WSSView obj = new TABLE11_WSSView();
                                        obj.Date = DateTime.Parse(wssItems.ToArray()[i].Date).ToShortDateString(); obj.CreationDate = wssItems.ToArray()[i].CreationDate.ToString().Remove(0, 10); obj.Project = wssItems.ToArray()[i].Project;
                                        obj.System_name = wssItems.ToArray()[i].System_name;

                                        using (BookingServerReplicaEntities db1 = new BookingServerReplicaEntities())
                                        {

                                            var labid = db1.LabComputersPrs.ToList().Find(x => x.FQDN.Split('.')[0].ToUpper().Contains(obj.System_name)).LabId;//Find(x => x.DisplayName.ToString() == obj.System_name.ToString()).LabId;
                                            LabInfo labInfo = new LabInfo();

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
        public ActionResult GetTABLE23_WSS2Data(List<string> pc, DateTime sdate, DateTime edate, int diff_UTCandLocal)
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
                            foreach (var pcname in pc)
                            {
                                //var pcname = pcname1.FQDN.Split('.')[0].ToUpper().Trim();
                                if (pcname != null && pcname != string.Empty && pcname.Count() > 0)
                                {

                                    var wss2Items = (from item in db.WSS2_Card_Pr where item.Date.Contains(date) && item.System_name.Contains(pcname) select new { item.Date, item.CreationDate, item.Project, item.Kernel_Loadbox_Card_WSS2_BuildVersion, item.Kernel_Loadbox_Card_WSS2_Calibration_Day, item.Kernel_Loadbox_Card_WSS2_Calibration_Month, item.Kernel_Loadbox_Card_WSS2_Calibration_Year, item.Kernel_Loadbox_Card_WSS2_CARD_BoardRevision, item.Kernel_Loadbox_Card_WSS2_CARD_BoardType, item.Kernel_Loadbox_Card_WSS2_CARD_Description, item.Kernel_Loadbox_Card_WSS2_CARD_VER_TAG_ARRAY, item.Kernel_Loadbox_Card_WSS2_Day_of_Creation, item.Kernel_Loadbox_Card_WSS2_Firmware_Card_Major, item.Kernel_Loadbox_Card_WSS2_Firmware_Card_Minor, item.Kernel_Loadbox_Card_WSS2_Firmware_FPGA_Major, item.Kernel_Loadbox_Card_WSS2_Firmware_FPGA_Minor, item.Kernel_Loadbox_Card_WSS2_Month_of_Creation, item.Kernel_Loadbox_Card_WSS2_Next_Calibration_Day, item.Kernel_Loadbox_Card_WSS2_Next_Calibration_Month, item.Kernel_Loadbox_Card_WSS2_Next_Calibration_Year, item.Kernel_Loadbox_Card_WSS2_PowerOnTime, item.Kernel_Loadbox_Card_WSS2_RepairCounter, item.Kernel_Loadbox_Card_WSS2_Serial_No, item.Kernel_Loadbox_Card_WSS2_Year_of_Creation, item.System_name });

                                    for (int i = 0; i < wss2Items.Count(); i++)
                                    {
                                        TABLE12_WSS2View obj = new TABLE12_WSS2View();
                                        obj.Date = DateTime.Parse(wss2Items.ToArray()[i].Date).ToShortDateString(); obj.CreationDate = wss2Items.ToArray()[i].CreationDate.ToString().Remove(0, 10); obj.Project = wss2Items.ToArray()[i].Project;
                                        obj.System_name = wss2Items.ToArray()[i].System_name;

                                        using (BookingServerReplicaEntities db1 = new BookingServerReplicaEntities())
                                        {

                                            var labid = db1.LabComputersPrs.ToList().Find(x => x.FQDN.Split('.')[0].ToUpper().Contains(obj.System_name)).LabId;//Find(x => x.DisplayName.ToString() == obj.System_name.ToString()).LabId;
                                            LabInfo labInfo = new LabInfo();

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

        [HttpPost]
        public ActionResult GetTABLE24_DiagnosticToolData(List<string> pc, DateTime sdate, DateTime edate, int diff_UTCandLocal)
        {
            List<TABLE24_DiagnosticToolView> table3DataViews = new List<TABLE24_DiagnosticToolView>();
            if (pc != null && pc.Count() > 0)
            {
                string pclist = string.Empty;
                try
                {
                    using (PromasterImport_HWInfoEntities db = new PromasterImport_HWInfoEntities())
                    {
                        db.Database.CommandTimeout = 500;
                        string date = string.Empty;
                        foreach (var p in pc)
                        {
                            pclist += ("'" + p.Trim() + "',");
                            //pclist.Add(p.FQDN.Split('.')[0].ToUpper().Trim());
                        }

                        pclist = pclist.Remove(pclist.Length - 1, 1);
                        connection();
                        DataTable dtable = new DataTable();

                        string Query = " Select " + ConfigurationManager.AppSettings["BookingServerReplica"] + " [LabInfo].DisplayName, [System_name],Date = (Date),[TKWinX_SW_Version],[MM6_SW_Version],[XFlash_SW_Version], CreationDate = ([CreationDate])from [VectorAndDiagSWVersion_Pr] inner join  " + ConfigurationManager.AppSettings["BookingServerReplica"] + "LabComputersPr on substring( " + ConfigurationManager.AppSettings["BookingServerReplica"] + "LabComputersPr.FQDN, 1, len(System_name)) = [VectorAndDiagSWVersion_Pr].System_name inner join " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo] on " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo].id = LabComputersPr.LabId where System_name in (" + pclist + ") AND CONVERT(DATETIME, Date) > = '" + sdate + "' AND CONVERT(DATETIME, Date) < = '" + edate + "' order by CreationDate";

                        OpenConnection();
                        SqlCommand cmd = new SqlCommand(Query, con);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dtable);
                        CloseConnection();

                        foreach (DataRow row in dtable.Rows)
                        {
                            TABLE24_DiagnosticToolView obj = new TABLE24_DiagnosticToolView();
                            obj.System_name = row["System_name"].ToString();
                            obj.LC_Name = row["DisplayName"].ToString();
                            obj.Date = row["Date"].ToString(); obj.CreationDate = DateTime.Parse(row["CreationDate"].ToString()).AddMinutes(diff_UTCandLocal).ToLongTimeString();
                            obj.TKWinX = row["TKWinX_SW_Version"].ToString().Replace(",", "<br/>\r\n"); //\r\n for next line in excel formatting ; <br/> for formatting in dxcell
                            obj.MM6 = row["MM6_SW_Version"].ToString().Replace(",", "<br/>\r\n");
                            obj.XFlash = row["XFlash_SW_Version"].ToString().Replace(",", "<br/>\r\n");
                            table3DataViews.Add(obj);
                        }
                        //hwDataViews = hwDataViews.OrderByDescending(x => x.Date).GroupBy(x => new { x.EEPName, x.Date, x.System_name }).Select(x => x.FirstOrDefault()).OrderBy(x => x.Date).ToList();


                        var result = Json(new { data = table3DataViews }, JsonRequestBehavior.AllowGet); result.MaxJsonLength = int.MaxValue; return result;
                    }
                }
                catch (Exception Ex)
                {
                    return View("Unable to retrieve data from database. Please try again later!!");
                }
            }
            else
            {
                var result = Json(new { success = false, message = "Try again" }, JsonRequestBehavior.AllowGet); result.MaxJsonLength = int.MaxValue; return result;
            }

        }

        [HttpPost]
        public ActionResult GetTABLE25_MotsimData(List<string> pc, DateTime sdate, DateTime edate, int diff_UTCandLocal)
        {
            List<TABLE25_MotsimView> PrjDataViews = new List<TABLE25_MotsimView>();
            if (pc != null && pc.Count() > 0)
            {
                string pclist = string.Empty;
                try
                {
                    using (PromasterImport_HWInfoEntities db = new PromasterImport_HWInfoEntities())
                    {
                        db.Database.CommandTimeout = 500;
                        string date = string.Empty;
                        foreach (var p in pc)
                        {
                            pclist += ("'" + p.Trim() + "',");
                            //pclist.Add(p.FQDN.Split('.')[0].ToUpper().Trim());
                        }

                        pclist = pclist.Remove(pclist.Length - 1, 1);
                        //connection();
                        string constring = ConfigurationManager.ConnectionStrings["PromasterImport_ProjectDescEntities_ado"].ToString();
                        con = new SqlConnection(constring);

                        DataTable dtable = new DataTable();

                        string Query = "";

                        Query = "Select " + ConfigurationManager.AppSettings["BookingServerReplica"] + " [LabInfo].DisplayName, [System_name],[Component_name],[Version] ,Date = Date ,[Details],CreationDate = [CreationDate], case when TRIM(Component_name) in ('MA_APB_MotSim4', 'MA_DC_MotSim4', 'MA_EC_MotSim4', 'MG_MotSim_PB')  then 'Motsim 4'  when TRIM(Component_name) in ('MG_APB_MotSimCon')  then 'Motsim 3' else 'NA' end as 'MotsimType' from ProjectDescription_Pr_1 inner join BookingServerReplica.dbo.LabComputersPr on substring(BookingServerReplica.dbo.LabComputersPr.FQDN, 1, len(System_name)) = [ProjectDescription_Pr_1].System_name inner join " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo] on " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo].id = LabComputersPr.LabId where System_name in (" + pclist + ") AND CONVERT(DATETIME, Date) > = '" + sdate + "' AND CONVERT(DATETIME, Date) < = '" + edate + "' and(TRIM(Component_name) in ('MA_APB_MotSim4', 'MA_DC_MotSim4', 'MA_EC_MotSim4', 'MG_MotSim_PB', 'MG_APB_MotSimCon')) order by CreationDate";

                        OpenConnection();
                        SqlCommand cmd = new SqlCommand(Query, con);
                        cmd.CommandTimeout = 0;
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dtable);
                        CloseConnection();

                        foreach (DataRow row in dtable.Rows)
                        {
                            TABLE25_MotsimView obj = new TABLE25_MotsimView();
                            obj.System_name = row["System_name"].ToString();
                            obj.LC_Name = row["DisplayName"].ToString();
                            obj.Date = row["Date"].ToString(); obj.CreationDate = DateTime.Parse(row["CreationDate"].ToString()).AddMinutes(diff_UTCandLocal).ToLongTimeString();

                            obj.Component_name = row["Component_name"].ToString();

                            obj.Project = row["Details"].ToString();

                            obj.Version = row["Version"].ToString();

                            obj.MotsimType = row["MotsimType"].ToString();
                            PrjDataViews.Add(obj);
                        }
                        //hwDataViews = hwDataViews.OrderByDescending(x => x.Date).GroupBy(x => new { x.EEPName, x.Date, x.System_name }).Select(x => x.FirstOrDefault()).OrderBy(x => x.Date).ToList();

                        var result = Json(new
                        {
                            data = PrjDataViews
                        },
                     JsonRequestBehavior.AllowGet); result.MaxJsonLength = int.MaxValue; return result;
                        //var result = Json(new { data = PrjDataViews },  JsonRequestBehavior.AllowGet);result.MaxJsonLength = int.MaxValue;return result;
                    }
                }
                catch (Exception Ex)
                {
                    return View("Unable to retrieve data from database. Please try again later!!");
                }
            }
            else
            {
                var result = Json(new { success = false, message = "Try again" }, JsonRequestBehavior.AllowGet); result.MaxJsonLength = int.MaxValue; return result;
            }
        }

        //[HttpPost]
        //public ActionResult GetTABLE26_VectorToolData(List<string> pc, DateTime sdate, DateTime edate, int diff_UTCandLocal)
        //{
        //    List<TABLE26_VectorToolView> table3DataViews = new List<TABLE26_VectorToolView>();
        //    if (pc != null && pc.Count() > 0)
        //    {
        //        string pclist = string.Empty;
        //        try
        //        {
        //            using (PromasterImport_HWInfoEntities db = new PromasterImport_HWInfoEntities())
        //            {
        //                db.Database.CommandTimeout = 500;
        //                string date = string.Empty;
        //                //User chosen HILs mapped Systems are stored in string format
        //                foreach (var p in pc)
        //                {
        //                    pclist += ("'" + p.Trim() + "',");
        //                    //pclist.Add(p.FQDN.Split('.')[0].ToUpper().Trim());
        //                }

        //                pclist = pclist.Remove(pclist.Length - 1, 1);
        //                connection();
        //                DataTable dtable = new DataTable();
        //                //SQL Query to fetch the relevant details
        //                string Query = " Select " + ConfigurationManager.AppSettings["BookingServerReplica"] + " [LabInfo].DisplayName, [System_name],Date,[Vector_SW_Name],[Vector_SW_Version], CreationDate from [VectorAndDiagSWVersion_Pr] inner join  " + ConfigurationManager.AppSettings["BookingServerReplica"] + "LabComputersPr on substring( " + ConfigurationManager.AppSettings["BookingServerReplica"] + "LabComputersPr.FQDN, 1, len(System_name)) = [VectorAndDiagSWVersion_Pr].System_name inner join " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo] on " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo].id = LabComputersPr.LabId where System_name in (" + pclist + ") AND CONVERT(DATETIME, Date) > = '" + sdate + "' AND CONVERT(DATETIME, Date) < = '" + edate + "'";

        //                OpenConnection();
        //                SqlCommand cmd = new SqlCommand(Query, con);
        //                SqlDataAdapter da = new SqlDataAdapter(cmd);
        //                da.Fill(dtable);
        //                CloseConnection();
        //                //Store as a list
        //                foreach (DataRow row in dtable.Rows)
        //                {
        //                    TABLE26_VectorToolView obj = new TABLE26_VectorToolView();
        //                    obj.System_name = row["System_name"].ToString();
        //                    obj.LC_Name = row["DisplayName"].ToString();
        //                    obj.Date = row["Date"].ToString(); obj.CreationDate = DateTime.Parse(row["CreationDate"].ToString()).AddMinutes(diff_UTCandLocal).ToLongTimeString();
        //                    obj.Vector_SW_Name = row["Vector_SW_Name"].ToString().Replace(",", "<br/>\r\n");
        //                    obj.Vector_SW_Version = row["Vector_SW_Version"].ToString().Replace(",", "<br/>\r\n");
        //                    table3DataViews.Add(obj);
        //                }

        //                //Pass as Json Object to view
        //                var result = Json(new { data = table3DataViews }, JsonRequestBehavior.AllowGet); result.MaxJsonLength = int.MaxValue; return result;
        //            }
        //        }
        //        catch (Exception Ex)
        //        {
        //            return View("Unable to retrieve data from database. Please try again later!!");
        //        }
        //    }
        //    else
        //    {
        //        var result = Json(new { success = false, message = "Try again" }, JsonRequestBehavior.AllowGet); result.MaxJsonLength = int.MaxValue; return result;
        //    }

        //}



        [HttpPost]
        public ActionResult GetTABLE26_VectorToolData(List<string> pc, DateTime sdate, DateTime edate, int diff_UTCandLocal)
        {
            List<TABLE26_VectorToolView> table3DataViews = new List<TABLE26_VectorToolView>();
            if (pc != null && pc.Count() > 0)
            {
                string pclist = string.Empty;
                try
                {
                    using (PromasterImport_HWInfoEntities db = new PromasterImport_HWInfoEntities())
                    {
                        db.Database.CommandTimeout = 500;
                        string date = string.Empty;

                        //User chosen HILs mapped Systems are stored in string format
                        //foreach (var p in pc)
                        //{
                        //    pclist += ("'" + p.Trim() + "',");
                        //    //pclist.Add(p.FQDN.Split('.')[0].ToUpper().Trim());
                        //}

                        //pclist = pclist.Remove(pclist.Length - 1, 1);
                        pclist = string.Join(",", pc);
                        connection();
                        DataTable dtable = new DataTable();
                        //SQL Query to fetch the relevant details
                        string Query = " Exec [dbo].[Vector_GetData] '" + pclist + "', '" + sdate + "', '" + edate + "' ";


                        //string Query = " Select " + ConfigurationManager.AppSettings["BookingServerReplica"] + " [LabInfo].DisplayName, [System_name],Date = max(Date) ,[Vector_HW_Name],[Vector_DeviceSerialNumber],[Vector_SW_Name],[Vector_SW_Version],[SP],[Vector_License_Name],[Vector_License_ID],[Vector_LicenseMaxVersion],[Vector_LicenseType]," +
                        //    "  CASE WHEN LEN(FR_Vector_License_Name) < 3 THEN REPLACE([FR_Vector_License_Name],'NA','') ELSE [FR_Vector_License_Name] END AS FR_Vector_License_Name, REPLACE([FR_Vector_License_ID],'NA','') AS FR_Vector_License_ID,[FR_Vector_LicenseMaxVersion],[FR_Vector_LicenseType],[FR], CASE WHEN LEN([LIN_Vector_License_Name]) < 3 THEN  REPLACE([LIN_Vector_License_Name],'NA','') ELSE [LIN_Vector_License_Name] END AS LIN_Vector_License_Name, REPLACE([LIN_Vector_License_ID],'NA','') AS LIN_Vector_License_ID, [LIN_Vector_LicenseMaxVersion],[LIN_Vector_LicenseType],[LIN], CASE WHEN LEN([DIVA_Vector_License_Name]) < 3 THEN REPLACE([DIVA_Vector_License_Name],'NA','') ELSE [DIVA_Vector_License_Name] END AS DIVA_Vector_License_Name, REPLACE([DIVA_Vector_License_ID],'NA','') AS DIVA_Vector_License_ID,[DIVA_Vector_LicenseMaxVersion],[DIVA_Vector_LicenseType],[DIVA], CASE WHEN LEN([AMD/XCP_Vector_License_Name]) < 3 THEN REPLACE([AMD/XCP_Vector_License_Name],'NA','') ELSE [AMD/XCP_Vector_License_Name] END AS [AMD/XCP_Vector_License_Name],REPLACE([AMD/XCP_Vector_License_ID],'NA','') AS [AMD/XCP_Vector_License_ID],[AMD/XCP_Vector_LicenseMaxVersion],[AMD/XCP_Vector_LicenseType],[AMD/XCP], CreationDate = max([CreationDate]) " +
                        //    "from [VectorAndDiagSWVersion_Pr] inner join  " + ConfigurationManager.AppSettings["BookingServerReplica"] + "LabComputersPr on substring( " + ConfigurationManager.AppSettings["BookingServerReplica"] + "LabComputersPr.FQDN, 1, len(System_name)) = [VectorAndDiagSWVersion_Pr].System_name inner join " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo] on " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo].id = LabComputersPr.LabId where System_name in (" + pclist + ") AND CONVERT(DATETIME, Date) > = '" + sdate + "' AND CONVERT(DATETIME, Date) < = '" + edate + "' GROUP BY   " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo].DisplayName , [System_name] ,[Vector_HW_Name],[Vector_DeviceSerialNumber],[Vector_SW_Name],[Vector_SW_Version],[SP],[Vector_License_Name],[Vector_License_ID],[Vector_LicenseMaxVersion],[Vector_LicenseType]      ,[FR_Vector_License_Name],[FR_Vector_License_ID],[FR_Vector_LicenseMaxVersion],[FR_Vector_LicenseType],[FR],[LIN_Vector_License_Name],[LIN_Vector_License_ID],[LIN_Vector_LicenseMaxVersion],[LIN_Vector_LicenseType],[LIN],[DIVA_Vector_License_Name],[DIVA_Vector_License_ID],[DIVA_Vector_LicenseMaxVersion],[DIVA_Vector_LicenseType],[DIVA],[AMD/XCP_Vector_License_Name],[AMD/XCP_Vector_License_ID],[AMD/XCP_Vector_LicenseMaxVersion],[AMD/XCP_Vector_LicenseType],[AMD/XCP] order by CreationDate";

                        OpenConnection();
                        SqlCommand cmd = new SqlCommand(Query, con);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dtable);
                        CloseConnection();
                        //Store as a list
                        foreach (DataRow row in dtable.Rows)
                        {
                            TABLE26_VectorToolView obj = new TABLE26_VectorToolView();
                            obj.System_name = row["System_name"].ToString();
                            obj.LC_Name = row["DisplayName"].ToString();
                            obj.Date = row["Date"].ToString(); obj.CreationDate = DateTime.Parse(row["CreationDate"].ToString()).AddMinutes(diff_UTCandLocal).ToLongTimeString();
                            //obj.Vector_SW_Name = row["Vector_SW_Name"].ToString().Replace(",", "<br/>\r\n");
                            //obj.Vector_SW_Version = row["Vector_SW_Version"].ToString().Replace(",", "<br/>\r\n");
                            //obj.Vector_License_Name         = row["Vector_License_Name"].ToString();
                            //obj.Vector_License_ID           = row["Vector_License_ID"].ToString();
                            //obj.Vector_HW_Name              = row["Vector_HW_Name"].ToString();
                            //obj.Vector_LicenseMaxVersion    = row["Vector_LicenseMaxVersion"].ToString();
                            //obj.Vector_DeviceSerialNumber   = row["Vector_DeviceSerialNumber"].ToString();
                            //obj.Vector_LicenseType          = row["Vector_LicenseType"].ToString();


                            //obj.Vector_HW_Name = row["Vector_HW_Name"].ToString();
                            //obj.Vector_DeviceSerialNumber = row["Vector_DeviceSerialNumber"].ToString();
                            //obj.Vector_SW_Name = row["Vector_SW_Name"].ToString().Replace(",", "<br/>\r\n"); ;
                            //obj.Vector_SW_Version = row["Vector_SW_Version"].ToString().Replace(",", "<br/>\r\n"); ;
                            //obj.SP = row["SP"].ToString().Replace(",", "<br/>\r\n");
                            //obj.Vector_License_Name = string.Concat(row["Vector_License_Name"].ToString(), " - ", row["Vector_License_ID"].ToString()).Replace(",", "<br/>\r\n");
                            //// [Vector_License_ID]                 = row["Vector_License_ID]                 "].ToString();
                            //obj.Vector_LicenseMaxVersion = row["Vector_LicenseMaxVersion"].ToString();
                            //// [Vector_LicenseType]                = row["Vector_LicenseType]                "].ToString();
                            //obj.FR_Vector_License = string.Concat(row["FR_Vector_License_Name"].ToString(), " - ", row["FR_Vector_License_ID"].ToString());
                            //// [FR_Vector_License_ID]              = row["FR_Vector_License_ID]              "].ToString();
                            //// [FR_Vector_LicenseMaxVersion]       = row["FR_Vector_LicenseMaxVersion]       "].ToString();
                            //// [FR_Vector_LicenseType]             = row["FR_Vector_LicenseType]             "].ToString();
                            //obj.FR = row["FR"].ToString();
                            //obj.LIN_Vector_License = string.Concat(row["LIN_Vector_License_Name"].ToString(), " - ", row["LIN_Vector_License_ID"].ToString());
                            //// [LIN_Vector_License_ID]             = row["LIN_Vector_License_ID]             "].ToString();
                            //// [LIN_Vector_LicenseMaxVersion]      = row["LIN_Vector_LicenseMaxVersion]      "].ToString();
                            //// [LIN_Vector_LicenseType]            = row["LIN_Vector_LicenseType]            "].ToString();
                            //obj.LIN = row["LIN"].ToString();
                            //obj.DIVA_Vector_License = string.Concat(row["DIVA_Vector_License_Name"].ToString(), " - ", row["DIVA_Vector_License_ID"].ToString());
                            //// [DIVA_Vector_License_ID]            = row["DIVA_Vector_License_ID]            "].ToString();
                            //// [DIVA_Vector_LicenseMaxVersion]     = row["DIVA_Vector_LicenseMaxVersion]     "].ToString();
                            //// [DIVA_Vector_LicenseType]           = row["DIVA_Vector_LicenseType]           "].ToString();
                            //obj.DIVA = row["DIVA"].ToString();
                            //obj.AMD_orXCP_Vector_License = string.Concat(row["AMD/XCP_Vector_License_Name"].ToString(), " - ", row["AMD/XCP_Vector_License_ID"].ToString());
                            ////[AMD/ XCP_Vector_License_ID]        = row["AMD/ XCP_Vector_License_ID]        "].ToString();
                            ////[AMD/ XCP_Vector_LicenseMaxVersion] = row["AMD/ XCP_Vector_LicenseMaxVersion] "].ToString();
                            ////[AMD/ XCP_Vector_LicenseType]       = row["AMD/ XCP_Vector_LicenseType]       "].ToString();
                            //obj.AMD_orXCP = row["AMD/XCP"].ToString();


                            obj.Vector_HW_Name = row["Vector_HW_Name"].ToString().Replace(",", "<br/>\r\n");
                            obj.Vector_DeviceSerialNumber = row["Vector_DeviceSerialNumber"].ToString().Replace(",", "<br/>\r\n");
                            obj.Vector_SW_Name = row["Vector_SW_Name"].ToString().Replace(",", "<br/>\r\n");
                            obj.Vector_SW_Version = row["Vector_SW_Version"].ToString().Replace(",", "<br/>\r\n");
                            obj.SP = row["SP"].ToString().Replace(",", "<br/>\r\n");
                            obj.Vector_License_Name = row["Vector_License"].ToString().Replace("NA-NA", "-").Replace(",", "<br/>\r\n");
                            obj.Vector_LicenseMaxVersion = row["Vector_LicenseMaxVersion"].ToString().Replace(",", "<br/>\r\n"); ;
                            obj.FR_Vector_License = row["FR_Vector_License"].ToString().Replace("NA-NA", "-").Replace(",", "<br/>\r\n");
                            obj.FR = row["FR"].ToString();
                            obj.LIN_Vector_License = row["LIN_Vector_License"].ToString().Replace("NA-NA", "-").Replace(",", "<br/>\r\n");
                            obj.LIN = row["LIN"].ToString();
                            obj.DIVA_Vector_License = row["DIVA_Vector_License"].ToString().Replace("NA-NA", "-").Replace(",", "<br/>\r\n");
                            obj.DIVA = row["DIVA"].ToString();
                            obj.AMD_orXCP_Vector_License = row["AMD/XCP_Vector_License"].ToString().Replace("NA-NA", "-").Replace(",", "<br/>\r\n");
                            obj.AMD_orXCP = row["AMD/XCP"].ToString();

                            //obj.Vector_Piggy = row["Vector_Piggy"].ToString().Replace(",", "<br/>\r\n");



                            table3DataViews.Add(obj);
                        }

                        //Pass as Json Object to view
                        var result = Json(new { data = table3DataViews }, JsonRequestBehavior.AllowGet); result.MaxJsonLength = int.MaxValue; return result;
                    }
                }
                catch (Exception Ex)
                {
                    return View("Unable to retrieve data from database. Please try again later!!");
                }
            }
            else
            {
                var result = Json(new { success = false, message = "Try again" }, JsonRequestBehavior.AllowGet); result.MaxJsonLength = int.MaxValue; return result;
            }

        }





        [HttpPost]
        public ActionResult GetTABLE27_VDMHYMData(List<string> pc, DateTime sdate, DateTime edate, int diff_UTCandLocal)
        {
            List<TABLE2_ProjectDescriptionView> PrjDataViews = new List<TABLE2_ProjectDescriptionView>();
            if (pc != null && pc.Count() > 0)
            {
                string pclist = string.Empty;
                try
                {
                    using (PromasterImport_HWInfoEntities db = new PromasterImport_HWInfoEntities())
                    {
                        db.Database.CommandTimeout = 500;
                        string date = string.Empty;
                        foreach (var p in pc)
                        {
                            pclist += ("'" + p.Trim() + "',");
                            //pclist.Add(p.FQDN.Split('.')[0].ToUpper().Trim());
                        }

                        pclist = pclist.Remove(pclist.Length - 1, 1);
                        //connection();
                        string constring = ConfigurationManager.ConnectionStrings["PromasterImport_ProjectDescEntities_ado"].ToString();
                        con = new SqlConnection(constring);

                        DataTable dtable = new DataTable();

                        string Query = "";

                        Query = " Select " + ConfigurationManager.AppSettings["BookingServerReplica"] + " [LabInfo].DisplayName, [System_name],[Component_name],[Version] , Date ,[Details], CreationDate, case when Component_name like '%VDM%' then 'VDM' when Component_name like '%HYM%' then 'HYM'  end as 'ModelType' from ProjectDescription_Pr_1 inner join " + ConfigurationManager.AppSettings["BookingServerReplica"] + "LabComputersPr on substring( " + ConfigurationManager.AppSettings["BookingServerReplica"] + "LabComputersPr.FQDN, 1, len(System_name)) = [ProjectDescription_Pr_1 ].System_name inner join " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo] on " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo].id = LabComputersPr.LabId where System_name in (" + pclist + ") AND CONVERT(DATETIME, Date) > = '" + sdate + "' AND CONVERT(DATETIME, Date) < = '" + edate + "' and (Component_name like '%VDM%' or  Component_name like '%HYM%') order by CreationDate";

                        OpenConnection();
                        SqlCommand cmd = new SqlCommand(Query, con);
                        cmd.CommandTimeout = 0;
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dtable);
                        CloseConnection();

                        foreach (DataRow row in dtable.Rows)
                        {
                            TABLE2_ProjectDescriptionView obj = new TABLE2_ProjectDescriptionView();
                            obj.System_name = row["System_name"].ToString();
                            obj.LC_Name = row["DisplayName"].ToString();
                            obj.Date = row["Date"].ToString(); obj.CreationDate = DateTime.Parse(row["CreationDate"].ToString()).AddMinutes(diff_UTCandLocal).ToLongTimeString();

                            obj.Component_name = row["Component_name"].ToString();

                            obj.Details = row["Details"].ToString();

                            obj.Version = row["Version"].ToString();

                            obj.ModelType = row["ModelType"].ToString();
                            PrjDataViews.Add(obj);
                        }
                        var result = Json(new
                        {
                            data = PrjDataViews
                        },
                     JsonRequestBehavior.AllowGet); result.MaxJsonLength = int.MaxValue; return result;
                        //var result = Json(new { data = PrjDataViews },  JsonRequestBehavior.AllowGet);result.MaxJsonLength = int.MaxValue;return result;
                    }
                }
                catch (Exception Ex)
                {
                    return View("Unable to retrieve data from database. Please try again later!!");
                }
            }
            else
            {
                var result = Json(new { success = false, message = "Try again" }, JsonRequestBehavior.AllowGet); result.MaxJsonLength = int.MaxValue; return result;
            }
        }

        public ActionResult GetTABLE28_IISBoxData(List<string> pc, DateTime sdate, DateTime edate, int diff_UTCandLocal)
        {
            List<TABLE2_ProjectDescriptionView> PrjDataViews = new List<TABLE2_ProjectDescriptionView>();
            if (pc != null && pc.Count() > 0)
            {
                string pclist = string.Empty;
                try
                {
                    using (PromasterImport_HWInfoEntities db = new PromasterImport_HWInfoEntities())
                    {
                        db.Database.CommandTimeout = 500;
                        string date = string.Empty;
                        pclist = string.Join(",", pc);
                        connection();
                        DataTable dtable = new DataTable();
                        //SQL Query to fetch the relevant details
                        string Query = " Exec PromasterImport_ProjectDesc.[dbo].[GetIISBox_Info] '" + pclist + "', '" + sdate + "', '" + edate + "', '" + "All" + "'";


                        //" Select " + ConfigurationManager.AppSettings["BookingServerReplica"] + " [LabInfo].DisplayName, [System_name],[Component_name],[Version] , (Date) , ([CreationDate]) from ProjectDescription_Pr_1 inner join " + ConfigurationManager.AppSettings["BookingServerReplica"] + "LabComputersPr on substring( " + ConfigurationManager.AppSettings["BookingServerReplica"] + "LabComputersPr.FQDN, 1, len(System_name)) = [ProjectDescription_Pr_1 ].System_name inner join " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo] on " + ConfigurationManager.AppSettings["BookingServerReplica"] + "[LabInfo].id = LabComputersPr.LabId where System_name in (" + pclist + ") AND CONVERT(DATETIME, Date) > = '" + sdate + "' AND CONVERT(DATETIME, Date) < = '" + edate + "' and Component_name like '%IIS%' " ;

                        OpenConnection();
                        SqlCommand cmd = new SqlCommand(Query, con);
                        cmd.CommandTimeout = 0;
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dtable);
                        CloseConnection();

                        foreach (DataRow row in dtable.Rows)
                        {
                            TABLE2_ProjectDescriptionView obj = new TABLE2_ProjectDescriptionView();
                            obj.System_name = row["System_name"].ToString();
                            obj.LC_Name = row["DisplayName"].ToString();
                            obj.Date = row["Date"].ToString(); obj.CreationDate = (row["CreationDate"].ToString().Trim() != "-") ? (DateTime.Parse(row["CreationDate"].ToString()).AddMinutes(diff_UTCandLocal).ToLongTimeString()) : row["CreationDate"].ToString();

                            obj.Component_name = row["Component_name"].ToString();


                            obj.Version = row["Version"].ToString();

                            // obj.ModelType = row["ModelType"].ToString();
                            PrjDataViews.Add(obj);
                        }
                        var result = Json(new
                        {
                            data = PrjDataViews
                        },
                     JsonRequestBehavior.AllowGet); result.MaxJsonLength = int.MaxValue; return result;
                        //var result = Json(new { data = PrjDataViews },  JsonRequestBehavior.AllowGet);result.MaxJsonLength = int.MaxValue;return result;
                    }
                }
                catch (Exception Ex)
                {
                    return View("Unable to retrieve data from database. Please try again later!!");
                }
            }
            else
            {
                var result = Json(new { success = false, message = "Try again" }, JsonRequestBehavior.AllowGet); result.MaxJsonLength = int.MaxValue; return result;
            }
        }


        //*****************Req from Andreas to extrct WW VSC Presence report********************************************
        //[HttpPost]
        //public ActionResult GetVSCLDUSummaryData(List<string> pc, DateTime sdate, DateTime edate)
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
        //                    foreach (var pcname in pc)
        //                    {
        //                        //var pcname = pcname1.FQDN.Split('.')[0].ToUpper().Trim();
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


        //                                if (vscItems.ToArray()[i].Kernel_Loadbox_Card_VSC_CARD_BoardType.Contains("VSC"))
        //                                    obj.VSC_present = "Yes";
        //                                else
        //                                    obj.VSC_present = "No";

        //                                if (lduItems.ToArray()[i].Kernel_Loadbox_Card_LDU_CARD_BoardType.Contains("LDU"))
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
        public string Project { get; set; }
        public string System_name { get; set; }
        public string Date { get; set; }
        public string CreationDate { get; set; }
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
        public string SensorType { get; set; }
        public string ModelType { get; set; }
        public string LC_Name { get; set; }
        public string Project { get; set; }
        public string System_name { get; set; }
        public string Component_name { get; set; }
        public string Version { get; set; }
        public string Db_Version { get; set; }
        public string Date { get; set; }
        public string CreationDate { get; set; }
        public string Details { get; set; }
        public string ToolVersion { get; set; }
        public string EMUCable { get; set; }
        public string AECUCable { get; set; }
        public string MetaEditor_Ver { get; set; }
        public string Product { get; set; }

        public string ProjectBuilder_Ver { get; set; }
        public string Ascet_Ver { get; set; }
        public string ProjectEditor_Ver { get; set; }
        public string LabOEM { get; set; }
    }

    public partial class TABLE3_EBView
    {
        public string LC_Name { get; set; }
        public string Project { get; set; }
        public string System_name { get; set; }
        public string Date { get; set; }
        public string CreationDate { get; set; }
        public string Kernel_EB_Cards_EB5100 { get; set; }
        public string Kernel_EB_Cards_EB5200 { get; set; }

    }
    public partial class TABLE4_ES4441View
    {
        public string LC_Name { get; set; }
        public string Project { get; set; }
        public string System_name { get; set; }
        public string Date { get; set; }
        public string CreationDate { get; set; }
        public string Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441SN { get; set; }
        public string Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441FWVersionMain { get; set; }
        public string Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441FWVersionMajor { get; set; }
        public string Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441FWVersionMinor { get; set; }

        public string Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441HWVersion { get; set; }
        public string Kernel_Component_FaultSimulationControl_ES4441_FW_ES4441HWRevision { get; set; }

    }
    public partial class TABLE5_OTSOView
    {
        public string LC_Name { get; set; }
        public string Project { get; set; }
        public string System_name { get; set; }
        public string Date { get; set; }
        public string CreationDate { get; set; }
        public string Kernel_Component_IPB_TemperatureReceive_OTSO1Available { get; set; }
        public string Kernel_Component_IPB_TemperatureReceive_OTSO2Available { get; set; }


    }
    public partial class TABLE6_PowerSupplyView
    {
        public string LC_Name { get; set; }
        public string Project { get; set; }
        public string System_name { get; set; }
        public string Date { get; set; }
        public string CreationDate { get; set; }
        public string Kernel_Loadbox_Card_PSC_Power_Supply_Configuration_PS_Configuration { get; set; }
        public string Kernel_Loadbox_Card_PSC_Power_Supply_Configuration_PS_Factor_ECU { get; set; }
        public string Kernel_Loadbox_Card_PSC_Power_Supply_Configuration_PS_Factor_MR { get; set; }
        public string Kernel_Loadbox_Card_PSC_Power_Supply_Configuration_PS_Factor_VR { get; set; }


    }
    public partial class TABLE7_BOBView
    {
        public string LC_Name { get; set; }
        public string Project { get; set; }
        public string System_name { get; set; }
        public string Date { get; set; }
        public string CreationDate { get; set; }
        public string Kernel_Loadbox_Card_ECC_BreakOutBox_BOB_SerialNumber { get; set; }
        public string Kernel_Loadbox_Card_ECC_BreakOutBox_BOB_DeviceRevision { get; set; }

    }
    public partial class TABLE8_BOB1View
    {
        public string LC_Name { get; set; }
        public string Project { get; set; }
        public string System_name { get; set; }
        public string Date { get; set; }
        public string CreationDate { get; set; }
        public string Kernel_Loadbox_Card_ECC_BreakOutBox_BOB_SerialNumber { get; set; }
    }
    public partial class TABLE9_IBView
    {
        public string LC_Name { get; set; }
        public string Project { get; set; }
        public string System_name { get; set; }
        public string Date { get; set; }
        public string CreationDate { get; set; }
        public string ModelSpecific_EID_NET_CAN_Settings_CAN_Settings_rtpc_ixxat_config { get; set; }
    }
    public partial class TABLE10_IXXAT2View
    {
        public string LC_Name { get; set; }
        public string Project { get; set; }
        public string System_name { get; set; }
        public string Date { get; set; }
        public string CreationDate { get; set; }
        public string ModelSpecific_EID_NET_CAN_Settings_CAN_Settings_rtpc_ixxat_config { get; set; }
    }
    public partial class TABLE11_APBView
    {
        public string LC_Name { get; set; }
        public string Project { get; set; }
        public string System_name { get; set; }
        public string Date { get; set; }
        public string CreationDate { get; set; }
        public string ModelSpecific_EID_Switch_APB_APB_Module_GIO2_Signature_GIO2 { get; set; }

    }
    public partial class TABLE12_HAPView
    {
        public string LC_Name { get; set; }
        public string Project { get; set; }
        public string System_name { get; set; }
        public string Date { get; set; }
        public string CreationDate { get; set; }
        public string Kernel_Loadbox_Card_ECC_Harnesadapter_HAP_SerialNumber { get; set; }
        public string Kernel_Loadbox_Card_ECC_Harnesadapter_HAP_DeviceRevision { get; set; }

    }
    public partial class TABLE13_HAP1View
    {
        public string LC_Name { get; set; }
        public string Project { get; set; }
        public string System_name { get; set; }
        public string Date { get; set; }
        public string CreationDate { get; set; }
        public string Kernel_Loadbox_Card_ECC_Harnesadapter_HAP_SerialNumber { get; set; }
    }
    public partial class TABLE14_CableView
    {
        public string LC_Name { get; set; }
        public string Project { get; set; }
        public string System_name { get; set; }
        public string Date { get; set; }
        public string CreationDate { get; set; }
        public string Kernel_Loadbox_Card_ECC_Cable_CABLE_SerialNumber { get; set; }
        public string Kernel_Loadbox_Card_ECC_Cable_CABLE_DeviceRevision { get; set; }

    }
    public partial class TABLE15_Cable1View
    {
        public string LC_Name { get; set; }
        public string Project { get; set; }
        public string System_name { get; set; }
        public string Date { get; set; }
        public string CreationDate { get; set; }
        public string Kernel_Loadbox_Card_ECC_Cable_CABLE_SerialNumber { get; set; }
    }

    public partial class TABLE16_ECCView
    {
        public string LC_Name { get; set; }
        public string Project { get; set; }
        public string System_name { get; set; }
        public string Date { get; set; }
        public string CreationDate { get; set; }

        public string Kernel_Loadbox_Card_ECC_AddOn_Present { get; set; }
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

    public partial class TABLE17_GIO1View
    {
        public string LC_Name { get; set; }
        public string Project { get; set; }
        public string System_name { get; set; }
        public string Date { get; set; }
        public string CreationDate { get; set; }
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

    public partial class TABLE18_GIO2View
    {
        public string LC_Name { get; set; }
        public string Project { get; set; }
        public string System_name { get; set; }
        public string Date { get; set; }
        public string CreationDate { get; set; }

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


    public partial class TABLE19_LDUView
    {
        public string LC_Name { get; set; }
        public string Project { get; set; }
        public string System_name { get; set; }
        public string Date { get; set; }
        public string CreationDate { get; set; }
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

    public partial class TABLE20_PSCView
    {
        public string LC_Name { get; set; }
        public string Project { get; set; }
        public string System_name { get; set; }
        public string Date { get; set; }
        public string CreationDate { get; set; }
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

    public partial class TABLE21_VSCView
    {
        public string LC_Name { get; set; }
        public string Project { get; set; }
        public string System_name { get; set; }
        public string Date { get; set; }
        public string CreationDate { get; set; }

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

    public partial class TABLE22_WSSView
    {
        public string LC_Name { get; set; }
        public string Project { get; set; }
        public string System_name { get; set; }
        public string Date { get; set; }
        public string CreationDate { get; set; }
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

    public partial class TABLE23_WSS2View
    {
        public string LC_Name { get; set; }
        public string Project { get; set; }
        public string System_name { get; set; }
        public string Date { get; set; }
        public string CreationDate { get; set; }

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


    public partial class TABLE24_DiagnosticToolView
    {
        public string LC_Name { get; set; }
        public string System_name { get; set; }
        public string Date { get; set; }
        public string CreationDate { get; set; }
        public string TKWinX { get; set; }
        public string MM6 { get; set; }
        public string XFlash { get; set; }
    }

    public partial class TABLE25_MotsimView
    {
        public string LC_Name { get; set; }
        public string System_name { get; set; }
        public string Date { get; set; }
        public string CreationDate { get; set; }
        public string Project { get; set; }
        public string Component_name { get; set; }
        public string Version { get; set; }
        public string MotsimType { get; set; }
    }

    //public partial class TABLE26_VectorToolView
    //{
    //    public string LC_Name { get; set; }
    //    public string System_name { get; set; }
    //    public string Date { get; set; }
    //    public string CreationDate { get; set; }
    //    public string Vector_SW_Name { get; set; }
    //    public string Vector_SW_Version { get; set; }
    //    public string XFlash { get; set; }
    //}


    public partial class TABLE26_VectorToolView
    {
        public string LC_Name { get; set; }
        public string System_name { get; set; }
        public string Date { get; set; }
        public string CreationDate { get; set; }
        //sw
        //public string Vector_SW_Name { get; set; }
        //public string Vector_SW_Version { get; set; }
        //hw
        //public string Vector_License_Name       { get; set; }
        //public string Vector_License_ID         { get; set; }
        //public string Vector_HW_Name            { get; set; }
        //public string Vector_LicenseMaxVersion  { get; set; }
        //public string Vector_DeviceSerialNumber { get; set; }
        //public string Vector_LicenseType        { get; set; }

        public string Vector_HW_Name { get; set; }
        public string Vector_DeviceSerialNumber { get; set; }
        public string Vector_SW_Name { get; set; }
        public string Vector_SW_Version { get; set; }
        public string SP { get; set; }
        public string Vector_License_Name { get; set; }
        public string Vector_LicenseMaxVersion { get; set; }
        public string FR_Vector_License { get; set; }
        public string FR { get; set; }
        public string LIN_Vector_License { get; set; }
        public string LIN { get; set; }
        public string DIVA_Vector_License { get; set; }
        public string DIVA { get; set; }
        public string AMD_orXCP_Vector_License { get; set; }
        public string AMD_orXCP { get; set; }

        public string Vector_Piggy { get; set; }
        public string Vector_Piggy_Device { get; set; }
        public string Vector_Piggy_Application { get; set; }
        public string Vector_Piggy_BusType { get; set; }
        public string Vector_Piggy_Channel { get; set; }

    }

    //public partial class TABLE26_MotsimView
    //{
    //    public string LC_Name { get; set; }
    //    public string System_name { get; set; }
    //    public string Date { get; set; }
    //    public string CreationDate { get; set; }
    //    public string Project { get; set; }
    //    public string Component_name { get; set; }
    //    public string Version { get; set; }
    //    public string MotsimType { get; set; }
    //}













    public partial class TABLE3_APB_BOB_Cable_EB_ES4441_1_ES4441_2_HAP1View
    {
        public string LC_Name { get; set; }
        public string Project { get; set; }
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
        public string Project { get; set; }
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
        public string Project { get; set; }
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
        public string CreationDate { get; set; }
        public string LC_Name { get; set; }
        public string Project { get; set; }
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
        public string CreationDate { get; set; }
        public string LC_Name { get; set; }
        public string Project { get; set; }
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
        public string CreationDate { get; set; }
        public string LC_Name { get; set; }
        public string Project { get; set; }
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
        public string CreationDate { get; set; }
        public string LC_Name { get; set; }
        public string Project { get; set; }
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
        public string Project { get; set; }
        public string System_name { get; set; }
        public string Date { get; set; }
        public string CreationDate { get; set; }

        public string Kernel_Loadbox_Card_VSC_UVRAddOnPresent { get; set; }
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
        public string CreationDate { get; set; }
        public string LC_Name { get; set; }
        public string Project { get; set; }
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
        public string Project { get; set; }
        public string System_name { get; set; }
        public string Date { get; set; }
        public string CreationDate { get; set; }
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
    //    public string LC_Name { get; set; } public string Project {get; set;}
    //    public string System_name { get; set; }
    //    public string Date { get; set; }

    //    public string VSC_present { get; set; }
    //    public string LDU_present { get; set; }
    //}
}





