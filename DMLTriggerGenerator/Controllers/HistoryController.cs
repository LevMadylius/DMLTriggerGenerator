using DMLTriggerGenerator.Attributes;
using DMLTriggerGenerator.DAL.DBManipulations;
using DMLTriggerGenerator.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DMLTriggerGenerator.Controllers
{
    public class HistoryController : Controller
    {
        protected override void OnException(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;
            filterContext.Result = new ViewResult
            {
                ViewName = "~/Views/Error.cshtml"
            };
        }
        // GET: History
        [Connected]
        public ActionResult Index()
        {
            var tables = LoadData.GetHistoryTables();

            return View(tables);
        }
        [Connected]
        public PartialViewResult GetRecords(string tableName)
        {
            var columnNames = LoadData.GetAllColumnsNamesFromTable(tableName);
            var records = LoadData.GetRecordsForTable(tableName);

            return PartialView("_records", new HistoryViewModel { Records = records, ColumnNames = columnNames });
        }
    }
}