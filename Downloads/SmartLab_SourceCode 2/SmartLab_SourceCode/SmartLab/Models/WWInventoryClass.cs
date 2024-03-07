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
    /// Template for the Inventory attributes - Mode, Whereabout (Usage), PlaceType, LabType, Site, LabName
    /// </summary>
    #region Inventory Attributes
    public partial class ModeHWInventory_class
    {
        public int ID { get; set; }
        public string Mode { get; set; }

    }

    public partial class WhereaboutInventory_class
    {
        public int ID { get; set; }
        public string Usage { get; set; }

    }

    public partial class PlaceTypeInventory_class
    {
        public int ID { get; set; }
        public string PlaceType { get; set; }

    }

    public partial class LabTypeInventory_class
    {
        public int ID { get; set; }
        public string LabType { get; set; }

    }

    public partial class HILGenerationInventory_class
    {
        public int ID { get; set; }
        public string HILGeneration { get; set; }

    }

    public partial class ACInputTypeInventory_class
    {
        public int ID { get; set; }
        public string ACInputType { get; set; }

    }

    public partial class SiteInventory_class
    {
        public int ID { get; set; }
        public string Location { get; set; }

    }

    public partial class LabNameInventory_class
    {
        public int ID { get; set; }
        public string LabName { get; set; }
        public int SiteID { get; set; }
        public int Type { get; set; }
        public string PC_Asset_Number { get; set; }
        public string Monitor_Asset_Number1 { get; set; }
        public string Monitor_Asset_Number2 { get; set; }
        public int AC_InputType { get; set; }
        public int HIL_Generation { get; set; }
    }
    #endregion

    /// <summary>
    /// Template for the Worldwide HW Inventory
    /// </summary>
    public partial class WW_HardwareInventory
    {
        public int HW_ID { get; set; }
        public int OEM { get; set; }
        public int BU { get; set; }
        public int HW_Type { get; set; }
        public Nullable<int> UOM { get; set; }      
        public Nullable<int> InventoryType { get; set; }
        public string SerialNumber { get; set; }
        public string BondNumber { get; set; }
        public string BondDate { get; set; }
        public string AssetNumber { get; set; }
        public Nullable<int> Mode { get; set; }
        public string Remarks { get; set; }
        public int Usage { get; set; }
        public int OtherPlace_ID { get; set; }
        public int HIL_ID { get; set; }
        public int Diagnostics_HIL_ID { get; set; }
        public Nullable<int> Location { get; set; }

        /*All values are also stored, other than keys - since, required in Bookmark setting to retrieve & display stored filter values*/
        public string OEM_Name { get; set; }
        public string BU_Name { get; set; }
        public string HW_Type_Name { get; set; }
        public string UOM_Name { get; set; }
        public string InventoryType_Name { get; set; }
        public string Mode_Name { get; set; }
        public string Usage_Name { get; set; }
        public string Location_Name { get; set; }

        /* For saving details to Repair/Borrow  */
        public int Place_type { get; set; }
        public string Receiver { get; set; }
        public string RxDept { get; set; }
        public string Start_date { get; set; }
        public string End_date { get; set; }
        public string Planned_end_date { get; set; }
        public string Info { get; set; }
        public string Updated_By { get; set; }

        /* For saving details to  HIL */

        public string HIL_Name { get; set; }
        public int Type { get; set; }
        public string PC_Asset_Number { get; set; }
        public string Monitor_Asset_Number1 { get; set; }
        public string Monitor_Asset_Number2 { get; set; }
        public int AC_InputType { get; set; }
        public int HIL_Generation { get; set; }
    }

    /// <summary>
    /// Template for the Bookmark & Column visibility settings
    /// </summary>
    #region 
    public partial class HWSettings
    {
        public string FormName { get; set; }
        public string ColumnName { get; set; }
        public int Visibility { get; set; }
    }
    public partial class WW_HardwareInventoryBookMarks
    {
        public int ID { get; set; }
        public string NTID { get; set; }
        public string FormName { get; set; }
        public string BookmarkName { get; set; }
        public string BookmarkValue { get; set; }
        public string DefaultValue { get; set; }
        public string operation { get; set; }

    }
    #endregion
}