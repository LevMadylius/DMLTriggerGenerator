using DMLTriggerGenerator.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMLTriggerGenerator.DAL.DBManipulations
{
    public static class TrackingInfo
    {
        public static string GetInfo(TrackingModel model, string[] operations)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append($"Performing operations with table: {model.TableName} {Environment.NewLine}");
            // if table does not exist
            if(!TableOperations.CheckTableExists(model.TableName))
            {
                builder.Append("Create tracking mechanism for:");
                foreach(var el in operations)
                {
                    var resultArr = model.Columns.Where(m => string.Equals(m.ColumnName.ToUpper(), el.ToUpper()));
                    if(resultArr != null && resultArr.Any())
                    {
                        builder.Append(el);
                        foreach(var res in resultArr)
                        {
                            builder.Append($"{res}, ");
                        }
                        builder.Length -= 2;
                        builder.Append($"{Environment.NewLine}");
                    }
                }
                return builder.ToString();
            }
            else // if table exists
            {
                return "";
            }
        }
    }
}
