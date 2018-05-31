using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DMLTriggerGenerator.DAL.Model;
using DMLTriggerGenerator.DAL.DBManipulations;

namespace DMLTriggerGenerator.Controllers.Api
{
    public class GenerateController : ApiController
    {
        private static TrackingModel _trackingModel;

        [HttpPost]
        [Route("Api/ProccessUserInfo/{tableName}")]
        public void ProccessUserInfo([FromBody]string[][] data, string tableName)
        {
            IEnumerable<string[]> sortedArr = data.Where(el => el.Length > 1);

            List<TrackingColumn> listColumns = new List<TrackingColumn>();
            foreach(var el in sortedArr)
            {
                listColumns.Add(new TrackingColumn()
                {
                    ColumnName = el[0],
                    Insert = (el.Contains("Insert")) ? "Insert" : null,
                    Update = (el.Contains("Update")) ? "Update" : null,
                    Delete = (el.Contains("Delete")) ? "Delete" : null
                });                
            }

            TrackingModel model = new TrackingModel { TableName = tableName, Columns = listColumns };
            _trackingModel = model;
        }
        [HttpPost]
        public void ClearTrackingModel()
        {
            _trackingModel = null;
        }

        [HttpGet]
        [Route("Api/GetTrackingInfo")]
        public string GetTrackingInfo()
        {
            if(!TableOperations.CheckTableExists(_trackingModel.TableName))
            {

            }

            return "test";
        }
    }
}
