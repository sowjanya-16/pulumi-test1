using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.IO;

namespace LC_Reports_V1.Models
{
    public class OssLabClass
    {

        public partial class OssLabDetails
        {
            
            private string sLabId;
            private string sFQDN;
            private string sLocation;
            private string sLab;
            private string sProjectChange;
            private string sCurrentProject;
            private string sCategory;
            private string sType;
            private string sResponsible;
            private string sSetupType;
            private string sFromWeek;
            private string sToWeek;

            public string LabId
            {
                get { return this.sLabId; }
                set { this.sLabId = value; }
            }

            public string FQDN
            {
                get { return this.sFQDN; }
                set { this.sFQDN = value; }
            }

            public string Location
            {
                get { return this.sLocation; }
                set { this.sLocation = value; }
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

            public string FromWeek
            {
                get { return this.sFromWeek; }
                set { this.sFromWeek = value; }
            }

            public string ToWeek
            {
                get { return this.sToWeek; }
                set { this.sToWeek = value; }
            }
        }


        public partial class OssTimeConfig
        {

            private string sNight_From_Time;
            private string sNight_To_Time;
            private string sWeekend_From_Time;
            private string sWeekend_To_Time;
            private string sDate_Modified;

            public string Night_From_Time
            {
                get { return this.sNight_From_Time; }
                set { this.sNight_From_Time = value; }
            }

            public string Night_To_Time
            {
                get { return this.sNight_To_Time; }
                set { this.sNight_To_Time = value; }
            }

            public string Weekend_From_Time
            {
                get { return this.sWeekend_From_Time; }
                set { this.sWeekend_From_Time = value; }
            }

            public string Weekend_To_Time
            {
                get { return this.sWeekend_To_Time; }
                set { this.sWeekend_To_Time = value; }
            }

            public string Date_Modified
            {
                get { return this.sDate_Modified; }
                set { this.sDate_Modified = value; }
            }




        }

      
    }

    public class Locations
    {
        public int Nos { get; set; }
        public string Loc { get; set; }
        public string WeekNo { get; set; }
        public double Value { get; set; }
        public double Average { get; set; }
        public string SetupType { get; set; }

    }

    public class Projects
    {
        public int Nos { get; set; }
        public string ComputerIds { get; set; }
        public string Project { get; set; }
        public string WeekNo { get; set; }
        public double Value { get; set; }
        public double Average { get; set; }
        public string Location { get; internal set; }
        public string SetupType { get; set; }
    }

    public class AverageRegionChart
    {
        public string WeekNo { get; set; }
        public double CN { get; set; }
        public double EU { get; set; }
        public double NA { get; set; }
        public double RBEI { get; set; }
        public double RBJP { get; set; }
        public double RBVH { get; set; }

    }

    public class ProjectAverageIN
    {
        public string Project { get; set; }
        public double TAverage { get; set; }
        public double DAverage { get; set; }
    }

    public class ProjectAverageCN
    {
        public string Project { get; set; }
        public double TAverage { get; set; }
        public double DAverage { get; set; }
    }

    public class LocationAverage
    {
        public int LAverage { get; set; }
    }



    public class AverageMasterDetails
    {

        public List<Locations> LocationTSG4 { get; set; }
        public List<Locations> LocationPBOX { get; set; }
        public List<Locations> LocationMLC { get; set; }
        public List<Locations> LocationACUROT { get; set; }


        public List<Projects> ProjectTSG4 { get; set; }
        public List<Projects> ProjectPBOX { get; set; }
        public List<Projects> ProjectMLC { get; set; }
        public List<Projects> ProjectACUROT { get; set; }


        //public List<AverageRegionChart> AvgRegionChart { get; set; }

        public List<AverageRegionChart> AvgRegionChart_TSG4 { get; set; }
        public List<AverageRegionChart> AvgRegionChart_PBOX { get; set; }
        public List<AverageRegionChart> AvgRegionChart_MLC { get; set; }
        public List<AverageRegionChart> AvgRegionChart_ACUROT { get; set; }


        //public List<ProjectAverageIN> PrgAvgDE { get; set; }
        //public List<ProjectAverageIN> PrgAvgJP { get; set; }
        //public List<ProjectAverageIN> PrgAvgCN { get; set; }
        //public List<ProjectAverageIN> PrgAvgIN { get; set; }
        //public List<ProjectAverageIN> PrgAvgVN { get; set; }
        //public List<ProjectAverageIN> PrgAvgUS { get; set; }


        public List<ProjectAverageIN> PrgAvgIN_TSG4 { get; set; }
        public List<ProjectAverageIN> PrgAvgIN_PBOX { get; set; }
        public List<ProjectAverageIN> PrgAvgIN_MLC { get; set; }
        public List<ProjectAverageIN> PrgAvgIN_ACUROT { get; set; }


        //public List<LocationAverage> LocAverageDE { get; set; }
        //public List<LocationAverage> LocAverageJP { get; set; }
        //public List<LocationAverage> LocAverageCN { get; set; }
        //public List<LocationAverage> LocAverageIN { get; set; }
        //public List<LocationAverage> LocAverageVN { get; set; }
        //public List<LocationAverage> LocAverageUS { get; set; }

        public List<LocationAverage> LocAverage_TSG4 { get; set; }
        public List<LocationAverage> LocAverage_PBOX { get; set; }
        public List<LocationAverage> LocAverage_MLC { get; set; }
        public List<LocationAverage> LocAverage_ACUROT { get; set; }
        public string Loc { get; set; }


    }

    public class EmailConfig
    {
        public int ComputerID { get; set; }
        public string DisplayName { get; set; }
        public string Responsible { get; set; }
        public string ResNTID { get; set; }
        public string LMT { get; set; }
        public string EM { get; set; }
        public string SWPCM { get; set; }
        public string SWTC { get; set; }
        public string SysTC { get; set; }
        public string DH1 { get; set; }
        public string DH2 { get; set; }
        //public string ModifiedUser { get; set; }

    }

    public class EmailDetails
    {
        public List<EmailConfig> ConfigList { get; set; }
    }
}