using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DMLTriggerGenerator.DAL.Model;
using DMLTriggerGenerator.DAL.DBManipulations;
using DMLTriggerGenerator.Utils;

namespace DMLTriggerGenerator.Controllers.Api
{
    public class GenerateController : ApiController
    {
        private static TrackingModel _trackingModel;
        private static List<string> _userOperations;

        [HttpPost]
        [Route("Api/ProccessUserInfo/{tableName}")]
        public void ProccessUserInfo([FromBody]string[][] data, string tableName)
        {
            List<TrackingColumn> listColumns = new List<TrackingColumn>();
            foreach(var el in data)
            {
                listColumns.Add(new TrackingColumn()
                {
                    ColumnName = el[0],
                    Insert = (el.Contains("Insert")) ? "INSERT" : null,
                    Update = (el.Contains("Update")) ? "UPDATE" : null,
                    Delete = (el.Contains("Delete")) ? "DELETE" : null
                });                
            }

            TrackingModel model = new TrackingModel { TableName = tableName, Columns = listColumns };
            _trackingModel = model;

            string[] operations = new string[]
            {
                "INSERT",
                "UPDATE",
                "DELETE"
            };
            _userOperations = new List<string>();

            foreach (var el in operations)
            {
                if (_trackingModel.Columns.Where(tm => string.Equals(el, tm.Insert) || string.Equals(el, tm.Update) || string.Equals(el, tm.Delete)).Count() > 0)
                    _userOperations.Add(el);
            }
        }
        [HttpPost]
        [Route("Api/ClearTrackingModel")]
        public void ClearTrackingModel()
        {
            _trackingModel = null;
        }

        [HttpGet]
        [Route("Api/GetTrackingInfo")]
        public string GetTrackingInfo()
        {
           
            var info = TrackingInfo.GetInfo(_trackingModel, _userOperations.ToArray());

            return info;
        }

        [HttpPost]
        [Route("Api/GenerateTrackingMechanism")]
        public void GenerateTrackingMechanism()
        {
            TableOperations.Tracking(_userOperations.ToArray(), _trackingModel);
        }


    }
}
