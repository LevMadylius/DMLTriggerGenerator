﻿using DMLTriggerGenerator.DAL.DBManipulations;
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

            var result = LoadData.GetTableNames();

            var cols = LoadData.GetTableColumns("Users");
            ViewBag.Title = "Home Page";

            return View();
        }
    }
}
