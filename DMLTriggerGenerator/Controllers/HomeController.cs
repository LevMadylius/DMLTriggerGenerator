using DMLTriggerGenerator.Attributes;
using DMLTriggerGenerator.DAL.DBManipulations;
using DMLTriggerGenerator.DAL.Model;
using DMLTriggerGenerator.Utils;
using DMLTriggerGenerator.Utils.Helpers;
using System.Net;
using System.Web.Mvc;
using DMLTriggerGenerator.ViewModel;

namespace DMLTriggerGenerator.Controllers
{
    public class HomeController : Controller
    {
        private HttpContextSessionWrapper _sessionWrapper;

        public HomeController()
        {
            _sessionWrapper = new HttpContextSessionWrapper();
        }
        [Connected]
        public ActionResult Index()
        {
            //var connectionString = new ConnectionString();
            //connectionString.IP = "127.0.0.1";
            //connectionString.InitialCatalog = "simma";
            //connectionString.Port = 1433;
            //connectionString.UserId = "sa";
            //connectionString.Password = "Qwerty123$";

            var tableNames = LoadData.GetTableNames();

            return View(tableNames);
        }
        [Connected]
        public PartialViewResult GetColumns(string tableName)
        {
            var tableModel = LoadData.GetTableModelByName(tableName);
            var trackingModel = LoadData.GetTrackingModelForTable(tableName);
            var viewModel = ColumnsViewModelHelper.FormList(tableModel.Columns, (trackingModel != null)? trackingModel.Columns : null);
            return PartialView("_columns", viewModel);
        }
    }
}
