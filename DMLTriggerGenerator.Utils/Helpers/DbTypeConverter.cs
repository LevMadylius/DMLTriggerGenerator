using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMLTriggerGenerator.Utils.Helpers
{
    public static class DbTypeConverter
    {
        public static List<string> TablesNamesFromDataTable(DataTable dt)
        {
            if(dt == null)
            {
                return null;
            }

            var tableNames = new List<string>();
            foreach (DataRow row in dt.Rows)
            {
                tableNames.Add(row["TABLE_NAME"] as string);
            }

            return tableNames;
        }

    }

    
}
