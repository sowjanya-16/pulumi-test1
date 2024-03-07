using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using System.Web.Mvc;

namespace LC_Reports_V1.Models
{
    /// <summary>
    /// Template for the filter of XML information
    /// </summary>
    public class LCfilterInfo
    {
        private DateTime startField;

        private DateTime endField;

        private string ntid;

        private int scheduledLabidsCntField;

        //private string category;

        //private string type;

        //private string responsible;

        //private string setuptype;

        private string Lab;
        private DateTime ProjectChange;
        private string CurrentProject;
        private string Category;
        private string Type;
        private string Responsible;
        private string SetupType;
        private string fWeek;
        private string tWeek;


        [DisplayName("Start Date")]
        [DataType(DataType.Date), Required]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime startDate
        {
            get
            {
                return this.startField;
            }
            set
            {
                this.startField = value;
            }
        }
        [DisplayName("End Date")]
        [DataType(DataType.Date), Required]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime endDate
        {
            get
            {
                return this.endField;
            }
            set
            {
                this.endField = value;
            }
        }

        public string NTid
        {
            get
            {
                return this.ntid;
            }
            set
            {
                this.ntid = value;
            }
        }

        public int scheduledLabidsCnt
        {
            get
            {
                return this.scheduledLabidsCntField;
            }
            set
            {
                this.scheduledLabidsCntField = value;
            }
        }

        public string sLab
        {
            get
            {
                return this.Lab;
            }
            set
            {
                this.Lab = value;
            }
        }

        [DisplayName("Project Change")]
        [DataType(DataType.Date), Required]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime sProjectChange
        {
            get
            {
                return this.ProjectChange;
            }
            set
            {
                this.ProjectChange = value;
            }
        }

        public string sCurrentProject
        {
            get
            {
                return this.CurrentProject;
            }
            set
            {
                this.CurrentProject = value;
            }
        }

        public string sCategory
        {
            get
            {
                return this.Category;
            }
            set
            {
                this.Category = value;
            }
        }

        public string sType
        {
            get
            {
                return this.Type;
            }
            set
            {
                this.Type = value;
            }
        }

        public string sResponsible
        {
            get
            {
                return this.Responsible;
            }
            set
            {
                this.Responsible = value;
            }
        }

        public string sSetupType
        {
            get
            {
                return this.SetupType;
            }
            set
            {
                this.SetupType = value;
            }
        }

        public string FromWeek
        {
            get
            {
                return this.fWeek;
            }
            set
            {
                this.fWeek = value;
            }
        }

        public string ToWeek
        {
            get
            {
                return this.tWeek;
            }
            set
            {
                this.tWeek = value;
            }
        }

        //public List<LabcarId> LabID { get; set; }

        //public class LabcarId
        //{
        //    public ushort labcarID { get; set; }
        //}

        //public ushort[] LCID
        //{
        //    get
        //    {
        //        return this.lcIdField;
        //    }
        //    set
        //    {
        //        this.lcIdField = value;
        //    }
        //}



        public LCfilterInfo()
        {
            this.Locations = new List<SelectListItem>();
            this.LabCartypes = new List<SelectListItem>();
            this.LabIDs = new List<SelectListItem>();
            this.LabidViews = new List<String>();
        }

        public List<SelectListItem> Locations { get; set; }
        public List<SelectListItem> LabCartypes { get; set; }
        public List<SelectListItem> LabIDs { get; set; }
        public List<SelectListItem> selectedListofLabIDs { get; set; }
        public List<String> LabidViews { get; set; }
        public IEnumerable<SelectListItem> listofScheduledLabIds { get; set; }

        //public int LocationId { get; set; }
        //public int Labtype { get; set; }
        //public int LabCarId { get; set; }

        //LabTypes

        public List<LabTypeAttributesLab> LabTypes { get; set; }


        public class LabTypeAttributesLab
        {

            private int snoField;
            private string labTypeField;


            public int SNo
            {
                get { return this.snoField; }
                set { this.snoField = value; }
            }

            public string LabType
            {
                get { return this.labTypeField; }
                set { this.labTypeField = value; }
            }


        }
        public List<Sitesattributeslab> SitesAttributesLab { get; set; }


        public class Sitesattributeslab
        {
            public string Id { get; set; }
            public string DisplayName { get; set; }
            public string RbCode { get; set; }
            public string CountryCode { get; set; }
            public string CreatedAt { get; set; }
            public string Created { get; set; }
            public string UpdatedAt { get; set; }
            public string Updated { get; set; }
        }

        //LabBookingExport

        public List<LabInfo> objFilterPageExpInfo;

        //XML export - Area
        /// <summary>
        /// Template for the imported XML from LC Server Tool
        /// </summary>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
        public class LabBookingExport
        {

            private LabBookingExportLab[] labsField;

            private uint createdField;

            private byte monthField;

            private ushort yearField;

            private decimal versionField;

            private double overallManualhrs;
            private double overallAutomatedhrs;
            private double overallAutomatedCaplhrs;
            private double overallManualCaplhrs;
            private DateTime startDate;
            private DateTime endDate;

            private string ui_startDate;
            private string ui_endDate;
            public string StartDate_UI
            {
                get { return this.ui_startDate; }
                set { this.ui_startDate = value; }
            }

            public string EndDate_UI
            {
                get { return this.ui_endDate; }
                set { this.ui_endDate = value; }
            }


            public DateTime StartDate
            {
                get { return this.startDate; }
                set { this.startDate = value; }
            }

            public DateTime EndDate
            {
                get { return this.endDate; }
                set { this.endDate = value; }
            }

            public double OverallManualHours
            {
                get { return this.overallManualhrs; }
                set { this.overallManualhrs = value; }
            }
            public double OverallManualCaplHours
            {
                get { return this.overallManualCaplhrs; }
                set { this.overallManualCaplhrs = value; }
            }

            public double OverallAutomatedHours
            {
                get { return this.overallAutomatedhrs; }
                set { this.overallAutomatedhrs = value; }
            }
            public double OverallAutomatedCaplHours
            {
                get { return this.overallAutomatedCaplhrs; }
                set { this.overallAutomatedCaplhrs = value; }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("Lab", IsNullable = false)]
            public LabBookingExportLab[] Labs
            {
                get
                {
                    return this.labsField;
                }
                set
                {
                    this.labsField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public uint created
            {
                get
                {
                    return this.createdField;
                }
                set
                {
                    this.createdField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public byte month
            {
                get
                {
                    return this.monthField;
                }
                set
                {
                    this.monthField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public ushort year
            {
                get
                {
                    return this.yearField;
                }
                set
                {
                    this.yearField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public decimal version
            {
                get
                {
                    return this.versionField;
                }
                set
                {
                    this.versionField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class LabBookingExportLab
        {

            private string modelField;

            private string ownerField;

            private string descriptionField;

            private string inventoryField;

            private string locationField;

            private string subLocationField;
            private string tsoulabelField;

            private LabBookingExportLabPC[] pCsField;

            private LabBookingExportLabDefectSpan[] defectivesField;

            private LabBookingExportLabReserveSpan[] reservationsField;

            private LabBookingExportManualSessSpan[] manualSessionsField;

            private LabBookingExportAutoSessSpan[] automatedSessionsField;

            private ushort idField;

            private string nameField;

            private string oemField;

            

            private double ManualTotalHoursField = 0;

            private double AutomatedTotalHoursField = 0;

            private double ManualCaplTotalHoursField = 0;

            private double AutomatedCaplTotalHoursField = 0;

            private double totalSumField;

            private string pcField;

            public string tsoulabel
            {
                get
                {
                    return this.tsoulabelField;
                }
                set
                {
                    this.tsoulabelField = value;
                }
            }
            public string PCname
            {
                get { return this.pcField; }
                set { this.pcField = value; }
            }

            public double TotalSum
            {
                get
                {
                    return this.totalSumField;
                }
                set
                {
                    this.totalSumField = value;
                }
            }

            public string TSOULabel
            {
                get { return this.tsoulabelField; }
                set
                {
                    this.tsoulabelField = value;
                }
            }
            public double ManualTotalHours
            {
                get
                {
                    return this.ManualTotalHoursField;
                }
                set
                {
                    this.ManualTotalHoursField = value;
                }
            }

            public double AutomatedTotalHours
            {
                get
                {
                    return this.AutomatedTotalHoursField;
                }
                set
                {
                    this.AutomatedTotalHoursField = value;
                }
            }

            public double ManualCaplTotalHours
            {
                get
                {
                    return this.ManualCaplTotalHoursField;
                }
                set
                {
                    this.ManualCaplTotalHoursField = value;
                }
            }

            public double AutomatedCaplTotalHours
            {
                get
                {
                    return this.AutomatedCaplTotalHoursField;
                }
                set
                {
                    this.AutomatedCaplTotalHoursField = value;
                }
            }

            /// <remarks/>
            public string Model
            {
                get
                {
                    return this.modelField;
                }
                set
                {
                    this.modelField = value;
                }
            }

            /// <remarks/>
            public string Owner
            {
                get
                {
                    return this.ownerField;
                }
                set
                {
                    this.ownerField = value;
                }
            }

            public string OEM
            {
                get
                {
                    return this.oemField;
                }
                set
                {
                    this.oemField = value;

                }
            }
            /// <remarks/>
            public string Description
            {
                get
                {
                    return this.descriptionField;
                }
                set
                {
                    this.descriptionField = value;
                }
            }

            /// <remarks/>
            public string Inventory
            {
                get
                {
                    return this.inventoryField;
                }
                set
                {
                    this.inventoryField = value;
                }
            }

            /// <remarks/>
            public string Location
            {
                get
                {
                    return this.locationField;
                }
                set
                {
                    this.locationField = value;
                }
            }

            /// <remarks/>
            public string SubLocation
            {
                get
                {
                    return this.subLocationField;
                }
                set
                {
                    this.subLocationField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("PC", IsNullable = false)]
            public LabBookingExportLabPC[] PCs
            {
                get
                {
                    return this.pCsField;
                }
                set
                {
                    this.pCsField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("Span", IsNullable = false)]
            public LabBookingExportLabDefectSpan[] Defectives
            {
                get
                {
                    return this.defectivesField;
                }
                set
                {
                    this.defectivesField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("Span", IsNullable = false)]
            public LabBookingExportLabReserveSpan[] Reservations
            {
                get
                {
                    return this.reservationsField;
                }
                set
                {
                    this.reservationsField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("Span", IsNullable = false)]
            public LabBookingExportManualSessSpan[] ManualSessions
            {
                get
                {
                    return this.manualSessionsField;
                }
                set
                {
                    this.manualSessionsField = value;
                }
            }


            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("Span", IsNullable = false)]
            public LabBookingExportAutoSessSpan[] AutomatedSessions
            {
                get
                {
                    return this.automatedSessionsField;
                }
                set
                {
                    this.automatedSessionsField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public ushort id
            {
                get
                {
                    return this.idField;
                }
                set
                {
                    this.idField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string name
            {
                get
                {
                    return this.nameField;
                }
                set
                {
                    this.nameField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class LabBookingExportLabPC
        {
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string FQDN { get; set; }
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public int LocationId { get; set; }
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public int LabId { get; set; }
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public int TrackerConfigId { get; set; }
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public int Updated { get; set; }
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public DateTime UpdatedAt { get; set; }
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public int Created { get; set; }
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public DateTime CreatedAt { get; set; }
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string DisplayName { get; set; }
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Description { get; set; }
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public int Id { get; set; }


            //private string[] itemsField;

            //private byte idField;

            //private string nodeField;

            ///// <remarks/>
            //[System.Xml.Serialization.XmlArrayItemAttribute("Item", IsNullable = false)]
            //public string[] Items
            //{
            //    get
            //    {
            //        return this.itemsField;
            //    }
            //    set
            //    {
            //        this.itemsField = value;
            //    }
            //}

            ///// <remarks/>
            //[System.Xml.Serialization.XmlAttributeAttribute()]
            //public byte id
            //{
            //    get
            //    {
            //        return this.idField;
            //    }
            //    set
            //    {
            //        this.idField = value;
            //    }
            //}

            ///// <remarks/>
            //[System.Xml.Serialization.XmlAttributeAttribute()]
            //public string node
            //{
            //    get
            //    {
            //        return this.nodeField;
            //    }
            //    set
            //    {
            //        this.nodeField = value;
            //    }
            //}
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class LabBookingExportLabDefectSpan
        {

            private uint startField;

            private uint endField;

            private string valueField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public uint start
            {
                get
                {
                    return this.startField;
                }
                set
                {
                    this.startField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public uint end
            {
                get
                {
                    return this.endField;
                }
                set
                {
                    this.endField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlTextAttribute()]
            public string Value
            {
                get
                {
                    return this.valueField;
                }
                set
                {
                    this.valueField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class LabBookingExportLabReserveSpan
        {

            private uint startField;

            private uint endField;

            private string valueField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public uint start
            {
                get
                {
                    return this.startField;
                }
                set
                {
                    this.startField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public uint end
            {
                get
                {
                    return this.endField;
                }
                set
                {
                    this.endField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlTextAttribute()]
            public string Value
            {
                get
                {
                    return this.valueField;
                }
                set
                {
                    this.valueField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class LabBookingExportManualSessSpan
        {

            private DateTime startField;

            private DateTime endField;

            private string triggerField;

            private string valueField;

            private int isActiveField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public DateTime start
            {
                get
                {
                    return this.startField;
                }
                set
                {
                    this.startField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public DateTime end
            {
                get
                {
                    return this.endField;
                }
                set
                {
                    this.endField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string trigger
            {
                get
                {
                    return this.triggerField;
                }
                set
                {
                    this.triggerField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlTextAttribute()]
            public string Value
            {
                get
                {
                    return this.valueField;
                }
                set
                {
                    this.valueField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlTextAttribute()]
            public int isActive
            {
                get
                {
                    return this.isActiveField;
                }
                set
                {
                    this.isActiveField = isActive;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class LabBookingExportAutoSessSpan
        {

            private DateTime startField;

            private DateTime endField;

            private string triggerField;

            private string valueField;

            private int isActiveField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public DateTime start
            {
                get
                {
                    return this.startField;
                }
                set
                {
                    this.startField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public DateTime end
            {
                get
                {
                    return this.endField;
                }
                set
                {
                    this.endField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string trigger
            {
                get
                {
                    return this.triggerField;
                }
                set
                {
                    this.triggerField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlTextAttribute()]
            public string Value
            {
                get
                {
                    return this.valueField;
                }
                set
                {
                    this.valueField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlTextAttribute()]
            public int isActive
            {
                get
                {
                    return this.isActiveField;
                }
                set
                {
                    this.isActiveField = isActive;
                }
            }
        }

        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class ExportManSessSpan
        {

            private int startField;

            private int endField;

            private string triggerField;

            private string valueField;

            private int isActiveField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public int start
            {
                get
                {
                    return this.startField;
                }
                set
                {
                    this.startField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public int end
            {
                get
                {
                    return this.endField;
                }
                set
                {
                    this.endField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string trigger
            {
                get
                {
                    return this.triggerField;
                }
                set
                {
                    this.triggerField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlTextAttribute()]
            public string Value
            {
                get
                {
                    return this.valueField;
                }
                set
                {
                    this.valueField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlTextAttribute()]
            public int isActive
            {
                get
                {
                    return this.isActiveField;
                }
                set
                {
                    this.isActiveField = isActive;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class ExportAutoSessSpan
        {

            private int startField;

            private int endField;

            private string triggerField;

            private string valueField;

            private int isActiveField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public int start
            {
                get
                {
                    return this.startField;
                }
                set
                {
                    this.startField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public int end
            {
                get
                {
                    return this.endField;
                }
                set
                {
                    this.endField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string trigger
            {
                get
                {
                    return this.triggerField;
                }
                set
                {
                    this.triggerField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlTextAttribute()]
            public string Value
            {
                get
                {
                    return this.valueField;
                }
                set
                {
                    this.valueField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlTextAttribute()]
            public int isActive
            {
                get
                {
                    return this.isActiveField;
                }
                set
                {
                    this.isActiveField = isActive;
                }
            }
        }

        //Sites

        //public partial class SitesAttributes
        //{

        //    private SitesAttributesLab[] _SitesFields;


        //    public SitesAttributesLab[] SitesFields
        //    {
        //        get
        //        {
        //            return this._SitesFields;
        //        }
        //        set
        //        {
        //            this._SitesFields = value;
        //        }
        //    }



        //}

        //public partial class SitesAttributesLab
        //{

        //    private int[] s_idField;
        //    private string[] DisplayName;


        //    public int[] Id
        //    {
        //        get { return this.s_idField; }
        //        set { this.s_idField = value; }
        //    }

        //    public string[] LocationName
        //    {
        //        get { return this.DisplayName; }
        //        set { this.DisplayName = value; }
        //    }


        //}


        //public List<PostBackAttributeslab> PostBackAttributesLab { get; set; }


        //public class PostBackAttributeslab
        //{
        //    public string LCLocationName { get; set; }
        //    public string LCLabType { get; set; }
        //    public int LCLabID { get; set; }
        //    public bool Submit { get; set; }
        //}




        public string LCLocationName { get; set; }
        public string LCLabType { get; set; }
        public string LCLabID { get; set; }
        public bool Submit { get; set; }




    }

    public class LCreportClass
    {
        public static TimeSpan UnixTimeStampsToTimeSpan(double unixTimeStrtStamp, double unixTimeEndStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime strtdtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            strtdtDateTime = strtdtDateTime.AddSeconds(unixTimeStrtStamp).ToLocalTime();

            System.DateTime enddtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            enddtDateTime = enddtDateTime.AddSeconds(unixTimeEndStamp).ToLocalTime();
            return enddtDateTime.Subtract(strtdtDateTime);
        }

        public static DateTime UnixTimeStampsToDate(double unixTimeStrtStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime strtdtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            strtdtDateTime = strtdtDateTime.AddSeconds(unixTimeStrtStamp).ToLocalTime();
            
            return strtdtDateTime;
        }
    }

    /// <summary>
    /// Template for the Report to be generated.
    /// </summary>
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
    public class ReportParameters
    {
        private TimeSpan periodField;

        private List<LC_Model> LCParamsField;


        private List<LC_TSIU> _LCTSIUField; // if needed
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
        public List<LC_Model> LCParams
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
        public List<LC_TSIU> LCTSIUParams
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
    public class LC_Model
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
    public class LC_TSIU
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

    //TSOU
    public partial class TsouChartAttributes
    {

        private TsouChartAttributesLab[] _LabFields;
        private string modelField;
        private string locationField;
        private DateTime dtstartField;
        private DateTime dtendField;
        private string titleField;
        private double overallManualhrs;
        private double overallAutomatedhrs;
        private double overallAutomatedCaplhrs;
        private double overallManualCaplhrs;
        private string LC_ProjectNameField;
        //private string[] name;

        public TsouChartAttributesLab[] LabFields
        {
            get
            {
                return this._LabFields;
            }
            set
            {
                this._LabFields = value;
            }
        }

        public string Model
        {
            get { return this.modelField; }
            set { this.modelField = value; }
        }

        public string Location
        {
            get { return this.locationField; }
            set { this.locationField = value; }
        }

        public DateTime StartTime
        {
            get { return this.dtstartField; }
            set { this.dtstartField = value; }
        }

        public DateTime EndTime
        {
            get { return this.dtendField; }
            set { this.dtendField = value; }
        }
        public string Title
        {
            get { return this.titleField; }
            set { this.titleField = value; }
        }

        public double OverallManualHours
        {
            get { return this.overallManualhrs; }
            set { this.overallManualhrs = value; }
        }
        public double OverallManualCaplHours
        {
            get { return this.overallManualCaplhrs; }
            set { this.overallManualCaplhrs = value; }
        }

        public double OverallAutomatedHours
        {
            get { return this.overallAutomatedhrs; }
            set { this.overallAutomatedhrs = value; }
        }
        public double OverallAutomatedCaplHours
        {
            get { return this.overallAutomatedCaplhrs; }
            set { this.overallAutomatedCaplhrs = value; }
        }
        //public string[] Name
        //{
        //    get { return this.name; }
        //    set { this.name = value; }
        //}

        public string LC_ProjectName_TSOU
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

    }

    public partial class TsouChartAttributesLab
    {

        private string inventoryField;
        private int idField;
        private string explabtype; // Export to Excel- oig1cob
        private string explocation;// Export to Excel- oig1cob
        private double manualTotalTimeField;
        private double manual_capl_TotalTimeField;
        private double automatedTotalTimeField;
        private double automated_capl_TotalTimeField;
        private double sumField;
        private string tsoulabelField;
        private string LabOemField;
        private string pcField;



        public string Inventory
        {
            get { return this.inventoryField; }
            set { this.inventoryField = value; }
        }

        public string TSOULabel
        {
            get { return this.tsoulabelField; }
            set
            {
                this.tsoulabelField = value;
            }
        }
        /// <summary>
        /// for Excel Export - oig1cob
        /// </summary>
        public string ExpLabtype
        {
            get { return this.explabtype; }
            set { this.explabtype = value; }
        }
        /// <summary>
        /// for Excel Export - oig1cob
        /// </summary>
        public string ExpLocation
        {
            get { return this.explocation; }
            set { this.explocation = value; }
        }

        public int id
        {
            get { return this.idField; }
            set { this.idField = value; }
        }

        public double ManualTotalTime
        {
            get { return this.manualTotalTimeField; }
            set { this.manualTotalTimeField = value; }
        }
        public double Manual_capl_TotalTime
        {
            get { return this.manual_capl_TotalTimeField; }
            set { this.manual_capl_TotalTimeField = value; }
        }
        public double AutomatedTotalTime
        {
            get { return this.automatedTotalTimeField; }
            set { this.automatedTotalTimeField = value; }
        }
        public double Automated_capl_TotalTimeField
        {
            get { return this.automated_capl_TotalTimeField; }
            set { this.automated_capl_TotalTimeField = value; }
        }
        //sum of manual and auto
        public double TotalSum
        {
            get { return this.sumField; }
            set { this.sumField = value; }
        }

        public string LabOEM
        {
            get { return this.LabOemField; }
            set { this.LabOemField = value; }
        }
        public string PCname
        {
            get { return this.pcField; }
            set { this.pcField = value; }
        }
    }

    //TSIS


    //TSNLS

    public partial class TsnlsChartAttributes
    {
        public TsnlsChartAttributesLab[] Fields
        {
            get
            {
                return this.Fields;
            }
            set
            {
                this.Fields = value;
            }
        }

        public partial class TsnlsChartAttributesLab
        {
            private TimeSpan notactivetime;
        }

    }








    ////mae9cob
    ////Diagnostics Interface
    //public class DiagnosticsParam
    //{

    //    public DiagnosticsParam()
    //    {
    //        this.LabIDs = new List<SelectListItem>();
    //        this.StartTime = DateTime.Now.AddMonths(-1);
    //        this.EndTime = DateTime.Now;
    //        this.Sites = new List<SelectListItem>();
    //    }
    //    private DateTime dtstartField;
    //    private DateTime dtendField;
    //    private string labid;
    //    private string site;



    //    public string LabID
    //    {
    //        get { return this.labid; }
    //        set { this.labid = value; }
    //    }

    //    public string Site
    //    {
    //        get { return this.site; }
    //        set { this.site = value; }
    //    }

    //    public DateTime StartTime
    //    {
    //        get { return this.dtstartField; }
    //        set { this.dtstartField = value; }
    //    }

    //    public DateTime EndTime
    //    {
    //        get { return this.dtendField; }
    //        set { this.dtendField = value; }
    //    }



    //    public List<SelectListItem> LabIDs { get; set; }
    //    public List<SelectListItem> Sites { get; set; }


    //}


    //mae9cob
    //Diagnostics Interface
    public class DiagnosticsParam
    {

        public DiagnosticsParam()
        {
            this.LabIDs = new List<SelectListItem>();
            this.StartTime = DateTime.Now.AddMonths(-1);
            this.EndTime = DateTime.Now;
            this.Sites = new List<SelectListItem>();
        }
        private DateTime dtstartField;
        private DateTime dtendField;
        private string labid;
        private string site;


        //LabBookingExport

        public LabBookingExport objFilterPageExpInfo;
        public class LabBookingExport
        {

            private LabBookingExportLab[] labsField;


            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("Lab", IsNullable = false)]
            public LabBookingExportLab[] Labs
            {
                get
                {
                    return this.labsField;
                }
                set
                {
                    this.labsField = value;
                }
            }

        }

        public partial class LabBookingExportLab
        {


            private string locationField;

            private string subLocationField;

            private ushort idField;

            private string nameField;


            /// <remarks/>
            public string Location
            {
                get
                {
                    return this.locationField;
                }
                set
                {
                    this.locationField = value;
                }
            }

            /// <remarks/>
            public string SubLocation
            {
                get
                {
                    return this.subLocationField;
                }
                set
                {
                    this.subLocationField = value;
                }
            }



            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public ushort id
            {
                get
                {
                    return this.idField;
                }
                set
                {
                    this.idField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string name
            {
                get
                {
                    return this.nameField;
                }
                set
                {
                    this.nameField = value;
                }
            }
        }


        public string LabID
        {
            get { return this.labid; }
            set { this.labid = value; }
        }

        public string LCLocationName { get; set; }
        public string Site
        {
            get { return this.site; }
            set { this.site = value; }
        }

        public DateTime StartTime
        {
            get { return this.dtstartField; }
            set { this.dtstartField = value; }
        }

        public DateTime EndTime
        {
            get { return this.dtendField; }
            set { this.dtendField = value; }
        }



        public List<SelectListItem> LabIDs { get; set; }
        public List<SelectListItem> Sites { get; set; }
        public List<SelectListItem> Locations { get; set; }


    }


    public class DiagnosticsData
    {
        public List<LC_Count> LC_Counts { get; set; }
    }

    public class LC_Count
    {
        public string Month { get; set; }  
        public string Year { get; set; }
        public string Location { get; set; }
        public int CCSILcnt_distinctpcs { get; set; }
        public int ETcnt_distinctpcs { get; set; }
        public int CCSILcnt_of_pc_instances { get; set; }
        public int ETcnt_of_pc_instances { get; set; }
        public int ETCCSILcnt_of_distinctPCs { get; set; }
        public int et_cnt_yr { get; set; }
        public int ccsil_cnt_yr { get; set; }
        public string Month_Location { get; set; }


    }

    //public class UniquePC_LCTypecount
    //{       
    //    public string Month { get; set; }
    //    public string Year { get; set; }
    //    public int Countet_distinctpcs { get; set; }

    //    public int Countccsil_distinctpcs { get; set; }

    //    public int Countet_ccsil_distinctpcs { get; set; }

    //    public int Countet_pcinstances { get; set; }

    //    public int Countccsil_pcinstances { get; set; }

    //    public int et_cnt_eachYr { get; set; }
    //    public int ccsil_cnt_eachYr { get; set; }
    //}





    public class laboemjson
    {
        private string LABIDField;
        private string OEMField;
        private string ResponsibleField;

        public string LABID
        {
            get { return this.LABIDField; }
            set { this.LABIDField = value; }
        }
        public string OEM
        {
            get { return this.OEMField; }
            set { this.OEMField = value; }
        }
        public string RESP
        {
            get { return this.ResponsibleField; }
            set { this.ResponsibleField = value; }
        }
    }
    public class DashboardChart
    {
        private DashboardChartAttributes[] _LabFields;
        private DateTime dtstartField;
        private DateTime dtendField;
        private string datestartField;
        private string dateendField;
        private double overallManualhrs;
        private double overallAutomatedhrs;
        private double overallManualCaplHours;
        private double overallAutomatedCaplHours;
        public DashboardChartAttributes[] LabFields
        {
            get
            {
                return this._LabFields;
            }
            set
            {
                this._LabFields = value;
            }
        }

        public DateTime StartTime
        {
            get { return this.dtstartField; }
            set { this.dtstartField = value; }
        }

        public DateTime EndTime
        {
            get { return this.dtendField; }
            set { this.dtendField = value; }
        }

        public string StartDate
        {
            get { return this.datestartField; }
            set { this.datestartField = value; }
        }

        public string EndDate
        {
            get { return this.dateendField; }
            set { this.dateendField = value; }
        }

        public double OverallManualHours
        {
            get { return this.overallManualhrs; }
            set { this.overallManualhrs = value; }
        }

        public double OverallAutomatedHours
        {
            get { return this.overallAutomatedhrs; }
            set { this.overallAutomatedhrs = value; }
        }        

        public double OverallManualCaplHours
        {
            get { return this.overallManualCaplHours; }
            set { this.overallManualCaplHours = value; }
        }

        
        public double OverallAutomatedCaplHours
        {
            get { return this.overallAutomatedCaplHours; }
            set { this.overallAutomatedCaplHours = value; }
        }

    }


    public partial class DashboardChartAttributes
    {

        // private string inventoryField;
        private int idField;
        private string labtype; // Export to Excel- oig1cob
        private string location;// Export to Excel- oig1cob
        private string inventory;
        private double manualTotalTimeField;
        private double automatedTotalTimeField;
        private double manual_capl_TotalTimeField;
        private double automated_capl_TotalTimeField;
        private double sumField;
        private string tsoulabelField;
        //    //private string LabOemField;



        //public string Inventory
        //{
        //    get { return this.inventoryField; }
        //    set { this.inventoryField = value; }
        //}

        public string TSOULabel
        {
            get { return this.tsoulabelField; }
            set
            {
                this.tsoulabelField = value;
            }
        }

        public string Labtype
        {
            get { return this.labtype; }
            set { this.labtype = value; }
        }
        /// <summary>
        /// for Excel Export - oig1cob
        /// </summary>
        public string Location
        {
            get { return this.location; }
            set { this.location = value; }
        }
        public string Inventory
        {
            get { return this.inventory; }
            set { this.inventory = value; }
        }
        
        public int id
        {
            get { return this.idField; }
            set { this.idField = value; }
        }


        public double ManualTotalTime
        {
            get { return this.manualTotalTimeField; }
            set { this.manualTotalTimeField = value; }
        }
        public double Manual_capl_TotalTime
        {
            get { return this.manual_capl_TotalTimeField; }
            set { this.manual_capl_TotalTimeField = value; }
        }
        public double AutomatedTotalTime
        {
            get { return this.automatedTotalTimeField; }
            set { this.automatedTotalTimeField = value; }
        }
        public double Automated_capl_TotalTime
        {
            get { return this.automated_capl_TotalTimeField; }
            set { this.automated_capl_TotalTimeField = value; }
        }
        //sum of manual and auto
        public double TotalSum
        {
            get { return this.sumField; }
            set { this.sumField = value; }
        }

        //    //public string LabOEM
        //    //{
        //    //    get { return this.LabOemField; }
        //    //    set { this.LabOemField = value; }
        //    //}

    }

    public class OssLabAdminModel
    {
        public string Lab { get; set; }
        public DateTime ProjectChange { get; set; }
        public string CurrentProject { get; set; }
        public string Category { get; set; }
        public string Type { get; set; }
        public string Responsible { get; set; }
        public string SetupType { get; set; }
        public string LCLocationName { get; set; }
        public string LCLabType { get; set; }
        public string LCLabID { get; set; }
        public bool Submit { get; set; }




        //LabBookingExport

        public LabBookingExport objFilterPageExpInfo;

        //XML export - Area
        /// <summary>
        /// Template for the imported XML from LC Server Tool
        /// </summary>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        [System.Xml.Serialization.XmlRootAttribute(Namespace = "", IsNullable = false)]
        public class LabBookingExport
        {

            private LabBookingExportLab[] labsField;

            private uint createdField;

            private byte monthField;

            private ushort yearField;

            private decimal versionField;

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("Lab", IsNullable = false)]
            public LabBookingExportLab[] Labs
            {
                get
                {
                    return this.labsField;
                }
                set
                {
                    this.labsField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public uint created
            {
                get
                {
                    return this.createdField;
                }
                set
                {
                    this.createdField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public byte month
            {
                get
                {
                    return this.monthField;
                }
                set
                {
                    this.monthField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public ushort year
            {
                get
                {
                    return this.yearField;
                }
                set
                {
                    this.yearField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public decimal version
            {
                get
                {
                    return this.versionField;
                }
                set
                {
                    this.versionField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class LabBookingExportLab
        {

            private string modelField;

            private string ownerField;

            private string descriptionField;

            private string inventoryField;

            private string locationField;

            private string subLocationField;

            private LabBookingExportLabPC[] pCsField;

            private LabBookingExportLabDefectSpan[] defectivesField;

            private LabBookingExportLabReserveSpan[] reservationsField;

            private LabBookingExportManualSessSpan[] manualSessionsField;

            private LabBookingExportAutoSessSpan[] automatedSessionsField;

            private ushort idField;

            private string nameField;

            /// <remarks/>
            public string Model
            {
                get
                {
                    return this.modelField;
                }
                set
                {
                    this.modelField = value;
                }
            }

            /// <remarks/>
            public string Owner
            {
                get
                {
                    return this.ownerField;
                }
                set
                {
                    this.ownerField = value;
                }
            }

            /// <remarks/>
            public string Description
            {
                get
                {
                    return this.descriptionField;
                }
                set
                {
                    this.descriptionField = value;
                }
            }

            /// <remarks/>
            public string Inventory
            {
                get
                {
                    return this.inventoryField;
                }
                set
                {
                    this.inventoryField = value;
                }
            }

            /// <remarks/>
            public string Location
            {
                get
                {
                    return this.locationField;
                }
                set
                {
                    this.locationField = value;
                }
            }

            /// <remarks/>
            public string SubLocation
            {
                get
                {
                    return this.subLocationField;
                }
                set
                {
                    this.subLocationField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("PC", IsNullable = false)]
            public LabBookingExportLabPC[] PCs
            {
                get
                {
                    return this.pCsField;
                }
                set
                {
                    this.pCsField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("Span", IsNullable = false)]
            public LabBookingExportLabDefectSpan[] Defectives
            {
                get
                {
                    return this.defectivesField;
                }
                set
                {
                    this.defectivesField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("Span", IsNullable = false)]
            public LabBookingExportLabReserveSpan[] Reservations
            {
                get
                {
                    return this.reservationsField;
                }
                set
                {
                    this.reservationsField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("Span", IsNullable = false)]
            public LabBookingExportManualSessSpan[] ManualSessions
            {
                get
                {
                    return this.manualSessionsField;
                }
                set
                {
                    this.manualSessionsField = value;
                }
            }


            /// <remarks/>
            [System.Xml.Serialization.XmlArrayItemAttribute("Span", IsNullable = false)]
            public LabBookingExportAutoSessSpan[] AutomatedSessions
            {
                get
                {
                    return this.automatedSessionsField;
                }
                set
                {
                    this.automatedSessionsField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public ushort id
            {
                get
                {
                    return this.idField;
                }
                set
                {
                    this.idField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string name
            {
                get
                {
                    return this.nameField;
                }
                set
                {
                    this.nameField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class LabBookingExportLabPC
        {
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string FQDN { get; set; }
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public int LocationId { get; set; }
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public int LabId { get; set; }
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public int TrackerConfigId { get; set; }
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public int Updated { get; set; }
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public DateTime UpdatedAt { get; set; }
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public int Created { get; set; }
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public DateTime CreatedAt { get; set; }
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string DisplayName { get; set; }
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string Description { get; set; }
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public int Id { get; set; }


            //private string[] itemsField;

            //private byte idField;

            //private string nodeField;

            ///// <remarks/>
            //[System.Xml.Serialization.XmlArrayItemAttribute("Item", IsNullable = false)]
            //public string[] Items
            //{
            //    get
            //    {
            //        return this.itemsField;
            //    }
            //    set
            //    {
            //        this.itemsField = value;
            //    }
            //}

            ///// <remarks/>
            //[System.Xml.Serialization.XmlAttributeAttribute()]
            //public byte id
            //{
            //    get
            //    {
            //        return this.idField;
            //    }
            //    set
            //    {
            //        this.idField = value;
            //    }
            //}

            ///// <remarks/>
            //[System.Xml.Serialization.XmlAttributeAttribute()]
            //public string node
            //{
            //    get
            //    {
            //        return this.nodeField;
            //    }
            //    set
            //    {
            //        this.nodeField = value;
            //    }
            //}
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class LabBookingExportLabDefectSpan
        {

            private uint startField;

            private uint endField;

            private string valueField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public uint start
            {
                get
                {
                    return this.startField;
                }
                set
                {
                    this.startField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public uint end
            {
                get
                {
                    return this.endField;
                }
                set
                {
                    this.endField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlTextAttribute()]
            public string Value
            {
                get
                {
                    return this.valueField;
                }
                set
                {
                    this.valueField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class LabBookingExportLabReserveSpan
        {

            private uint startField;

            private uint endField;

            private string valueField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public uint start
            {
                get
                {
                    return this.startField;
                }
                set
                {
                    this.startField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public uint end
            {
                get
                {
                    return this.endField;
                }
                set
                {
                    this.endField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlTextAttribute()]
            public string Value
            {
                get
                {
                    return this.valueField;
                }
                set
                {
                    this.valueField = value;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class LabBookingExportManualSessSpan
        {

            private DateTime startField;

            private DateTime endField;

            private string triggerField;

            private string valueField;

            private bool isActiveField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public DateTime start
            {
                get
                {
                    return this.startField;
                }
                set
                {
                    this.startField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public DateTime end
            {
                get
                {
                    return this.endField;
                }
                set
                {
                    this.endField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string trigger
            {
                get
                {
                    return this.triggerField;
                }
                set
                {
                    this.triggerField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlTextAttribute()]
            public string Value
            {
                get
                {
                    return this.valueField;
                }
                set
                {
                    this.valueField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlTextAttribute()]
            public bool isActive
            {
                get
                {
                    return this.isActiveField;
                }
                set
                {
                    this.isActiveField = isActive;
                }
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true)]
        public partial class LabBookingExportAutoSessSpan
        {

            private DateTime startField;

            private DateTime endField;

            private string triggerField;

            private string valueField;

            private bool isActiveField;

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public DateTime start
            {
                get
                {
                    return this.startField;
                }
                set
                {
                    this.startField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public DateTime end
            {
                get
                {
                    return this.endField;
                }
                set
                {
                    this.endField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlAttributeAttribute()]
            public string trigger
            {
                get
                {
                    return this.triggerField;
                }
                set
                {
                    this.triggerField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlTextAttribute()]
            public string Value
            {
                get
                {
                    return this.valueField;
                }
                set
                {
                    this.valueField = value;
                }
            }

            /// <remarks/>
            [System.Xml.Serialization.XmlTextAttribute()]
            public bool isActive
            {
                get
                {
                    return this.isActiveField;
                }
                set
                {
                    this.isActiveField = isActive;
                }
            }
        }

        public OssLabAdminModel()
        {
            this.Locations = new List<SelectListItem>();
            this.LabCartypes = new List<SelectListItem>();
            this.LabIDs = new List<SelectListItem>();
            this.LabidViews = new List<String>();
        }

        public List<SelectListItem> Locations { get; set; }
        public List<SelectListItem> LabCartypes { get; set; }
        public List<SelectListItem> LabIDs { get; set; }
        public List<SelectListItem> selectedListofLabIDs { get; set; }
        public List<String> LabidViews { get; set; }
        public IEnumerable<SelectListItem> listofScheduledLabIds { get; set; }

    }

    public partial class CPCReportAttributes
    {
        private CPCReportAttributesLab[] _LabFields;
        

        //private string[] name;
        public CPCReportAttributesLab[] LabFields
        {
            get
            {
                return this._LabFields;
            }
            set
            {
                this._LabFields = value;
            }
        }

    }
    public partial class CPCReportAttributesLab
    {
        private string sLabId;
        private string sPCName;
        private string sLocation1;
        private string sLab;
        private string sProjectChange;
        private string sCurrentProject;
        private string sCategory;
        private string sType;
        private string sResponsible;
        private string sSetupType;
        private string sDate;
        private string sAction;
        private string sValue;
        public string LabId
        {
            get { return this.sLabId; }
            set { this.sLabId = value; }
        }

        public string PCName
        {
            get { return this.sPCName; }
            set { this.sPCName = value; }
        }

        public string Location1
        {
            get { return this.sLocation1; }
            set { this.sLocation1 = value; }
        }
        public string Lab
        {
            get { return this.sLab; }
            set { this.sLab = value; }
        }
        public string ProjectChange
        {
            get { return this.sProjectChange; }
            set { this.sProjectChange = value; }
        }
        public string CurrentProject
        {
            get { return this.sCurrentProject; }
            set { this.sCurrentProject = value; }
        }
        public string Category
        {
            get { return this.sCategory; }
            set { this.sCategory = value; }
        }

        public string Type
        {
            get { return this.sType; }
            set { this.sType = value; }
        }

        public string Responsible
        {
            get { return this.sResponsible; }
            set { this.sResponsible = value; }
        }

        public string SetupType
        {
            get { return this.sSetupType; }
            set { this.sSetupType = value; }
        }

        public string Date
        {
            get { return this.sDate; }
            set { this.sDate = value; }
        }

        public string Act
        {
            get { return this.sAction; }
            set { this.sAction = value; }
        }

        public string Value
        {
            get { return this.sValue; }
            set { this.sValue = value; }
        }

    }

}