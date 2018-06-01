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
            if (!TableOperations.CheckTableExists($"{model.TableName}_History{Environment.NewLine}"))
            {
                builder.Append($"Create tracking mechanism for:{Environment.NewLine}");
                foreach(var el in operations)
                {
                    //fix this stupid shitty bug
                    var resultArr = model.Columns.Where(m => (m.Insert != null && string.Equals(m.Insert.ToUpper(), el.ToUpper())) ||
                                                             (m.Update != null && string.Equals(m.Update.ToUpper(), el.ToUpper())) ||
                                                             (m.Delete != null && string.Equals(m.Delete.ToUpper(), el.ToUpper()))).ToList();
                    if(resultArr != null && resultArr.Any())
                    {
                        builder.Append($"{el}:{Environment.NewLine}");
                        foreach(var res in resultArr)
                        {
                            builder.Append($"{res.ColumnName}, ");
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
