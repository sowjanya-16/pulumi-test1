using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace LC_Reports_V1.Controllers
{
    public class InventoryPMTController : Controller
    {
        private SqlConnection conn;

        private void connection()
        {

            string connString = ConfigurationManager.ConnectionStrings["BudgetingdbConnection"].ToString();
            conn = new SqlConnection(connString);

        }

        private void OpenConnection()
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
        }

        private void CloseConnection()
        {
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }
        }
        // GET: InventoryPMT
        public ActionResult PMTInventory()
        {
            return View();
        }

        public ActionResult BOM()
        {
            return View();
        }

        public ActionResult getBOMDatabase()
        {
            List<BOMDatabase> List = new List<BOMDatabase>();
            DataTable dt = new DataTable();
            try
            {

                connection();

                string Query = " Select  [ID]" +
          ",[Product] " +
          ",[ComponentName] " +
          ",[Group] " +
          ",[Subgroup] " +
          ",[UOM] " +
          ",[PartNo] " +
          ",[Make] " +
          ",[Quantity]" +
          ",[Scope] " +
          "from [BOM_Database_Table] order by ID";

                OpenConnection();
                SqlCommand cmd = new SqlCommand(Query, conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                CloseConnection();
            }
            catch (Exception e)
            {

            }
            foreach (DataRow row in dt.Rows)
            {
                BOMDatabase item = new BOMDatabase();
                item.ID = int.Parse(row["ID"].ToString());
                item.Product = row["Product"].ToString();
                item.ComponentName = row["ComponentName"].ToString();
                item.Group = row["Group"].ToString();
                item.Subgroup = row["Subgroup"].ToString();
                item.UOM = row["UOM"].ToString();
                item.Make = row["Make"].ToString();
                item.Quantity = int.Parse(row["Quantity"].ToString());
                item.PartNo = row["PartNo"].ToString();
                item.ScopeOfMaterial = row["Scope"].ToString();
               
                List.Add(item);
            }


            return Json(new { data = List }, JsonRequestBehavior.AllowGet);
        }

    }
}


public partial class BOMDatabase
{
    public int ID { get; set; }
    public string Product { get; set; }
    public string ComponentName { get; set; }
    public string Group { get; set; }
    public string Subgroup { get; set; }
    public string UOM { get; set; }
    public string PartNo { get; set; }
    public int Quantity { get; set; }
    public string Make { get; set; }
    public string ScopeOfMaterial { get; set; }
}
