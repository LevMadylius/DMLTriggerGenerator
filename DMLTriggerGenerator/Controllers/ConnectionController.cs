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
            try
            {
                _sessionWrapper.ConnectionString = connectionString.GetConnectionString();
                SQLDatabase.VerifyConnectivity(_sessionWrapper.ConnectionString);
                return RedirectToAction("Index", "Home");
            }
            //improve
            catch (ConnectionStringInvalidException ex)
            {
                return View();
            }
            catch(Exception ex)
            {
                return View();
            }

          //  return View();
        }
    }
}