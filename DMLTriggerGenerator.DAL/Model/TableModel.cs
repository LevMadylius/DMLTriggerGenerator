using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMLTriggerGenerator.DAL.Model
{
    public class TableModel
    {
        public string TableName { get; set; }
        public List<ColumnModel> Columns { get; set; }
    }
}
