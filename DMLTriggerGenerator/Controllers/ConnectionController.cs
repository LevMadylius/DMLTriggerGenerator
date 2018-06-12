using DMLTriggerGenerator.DAL.DBManipulations;
using DMLTriggerGenerator.DAL.Model;
using DMLTriggerGenerator.Utils;
using DMLTriggerGenerator.Utils.CustomExceptions;
using System;
using System.Web.Mvc;

namespace DMLTriggerGenerator.Controllers
{
    public class ConnectionController : Controller
    {
        protected override void OnException(ExceptionContext filterContext)
        {
            filterContext.ExceptionHandled = true;
            filterContext.Result = new ViewResult
            {
                ViewName = "~/Views/Error.cshtml"
            };
        }

        private HttpContextSessionWrapper _sessionWrapper = new HttpContextSessionWrapper();

        // GET: Connection
        public ActionResult Index()
        {
            // reset user connection parameters
            _sessionWrapper.ConnectionString = null;
            return View();
        }
        [HttpPost]
        public ActionResult Connect(ConnectionString connectionString)
        {
            _sessionWrapper.ConnectionString = connectionString.GetConnectionString();
            if(SQLDatabase.VerifyConnectivity(_sessionWrapper.ConnectionString))
            {
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View("ConnectionError");
            }
        }
    }
}