using DMLTriggerGenerator.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMLTriggerGenerator.DAL.DBManipulations
{
    public static class TableOperations
    {
        //make private after wrapping in execute command
        public static string CreateTable(TableModel model)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append($"CREATE TABLE {model.TableName}_History (");
            builder.Append($"{model.TableName}_HistoryID INT IDENTITY(1,1),");
            foreach(var column in model.Columns)
            {
                builder.Append($"{column.ColumnName} {column.DataType}");
                if(column.CharacterMaxLength != null)
                {
                    builder.Append($"({(column.CharacterMaxLength == -1? "MAX": column.CharacterMaxLength.Value.ToString())})");
                }
                builder.Append($" NULL,");
            }
            builder.Append("UserNameChanged VARCHAR(128) NOT NULL,"); // 128 max length of username in sql server
            builder.Append("DateChanged DATETIME NOT NULL)");
            return builder.ToString();
        }
    }
}
