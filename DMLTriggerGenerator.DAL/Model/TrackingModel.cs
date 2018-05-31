using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMLTriggerGenerator.DAL.Model
{
    public class TrackingColumn
    {
        public string ColumnName { get; set; }
        public string Insert { get; set; }
        public string Update { get; set; }
        public string Delete { get; set; }

    }
    public class TrackingModel
    {
        public string TableName { get; set; }
        public List<TrackingColumn> Columns { get; set; }
    }
}
