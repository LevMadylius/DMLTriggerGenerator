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
        protected override void OnException(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;
            filterContext.Result = new ViewResult
            {
                ViewName = "~/Views/Error.cshtml"
            };
        }

        private HttpContextSessionWrapper _sessionWrapper;

        public HomeController()
        {
            _sessionWrapper = new HttpContextSessionWrapper();
        }
        [Connected]
        public ActionResult Index()
        {
            var tableNames = LoadData.GetTableNames();

            return View(tableNames);
        }
        [Connected]
        public PartialViewResult GetColumns(string tableName)
        {
            var tableModel = LoadData.GetTableModelByName(tableName);
            var trackingModel = LoadData.GetTrackingModelForTable(tableName);
            var columnsViewModel = ColumnsViewModelHelper.FormList(tableModel.Columns, (trackingModel != null)? trackingModel.Columns : null);
            var triggerList = LoadData.GetListTriggers(tableName);
            return PartialView("_columns", new ColumnsViewModel { ColumnsElements = columnsViewModel, Triggers = triggerList});
        }
    }
}
