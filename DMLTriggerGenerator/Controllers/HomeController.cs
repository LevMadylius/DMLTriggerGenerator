using DMLTriggerGenerator.DAL.DBManipulations;
using DMLTriggerGenerator.DAL.Model;
using DMLTriggerGenerator.Utils.Helpers;
using System.Net;
using System.Web.Mvc;

namespace DMLTriggerGenerator.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            string sql = "SELECT TABLE_NAME AS TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' ORDER BY TABLE_NAME";
            ConnectionString.IP = IPAddress.Loopback;
            ConnectionString.InitialCatalog = "simma";
            ConnectionString.Port = 1433;
            ConnectionString.UserId = "sa";
            ConnectionString.Password = "Qwerty123$";

            var tableNames = LoadData.GetTableNames();

            var table = LoadData.GetTableModelByName(tableNames[0]);

            TableOperations.CreateTrackingMechanism(table, "insert");

            bool res = TableOperations.CheckTableExists("OpenedUsers");

            var cols = TableOperations.CheckTableHistoryChanges(table, table);

            ViewBag.Title = "Home Page";

            return View(tableNames);
        }

        public PartialViewResult GetColumns(string tableName)
        {
            var columns = LoadData.GetTableModelByName(tableName);

            return PartialView("_columns", columns.Columns);
        }
    }
}
