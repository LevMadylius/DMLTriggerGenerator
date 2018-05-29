﻿using DMLTriggerGenerator.DAL.Model;
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
        private static string GetCreateTableString(TableModel model)
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

        private static string ColumnSetEnum(List<ColumnModel> listCols)
        {
            StringBuilder builder = new StringBuilder();

            foreach(var column in listCols)
            {
                builder.Append($"{column.ColumnName},");
            }
            builder.Length--;

            return builder.ToString();
        }
        // for creating of altering trigger
        private static string GetCreateTriggerString(TableModel trackingModel, string operation)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append($"CREATE TRIGGER {trackingModel.TableName}{operation}Triggger ");
            builder.Append($"ON {trackingModel.TableName} ");
            builder.Append($"AFTER {operation.ToUpper()} ");
            builder.Append($"AS BEGIN ");
            builder.Append($"INSERT INTO {trackingModel.TableName}_History (");
            builder.Append($"{ColumnSetEnum(trackingModel.Columns)}, UserNameChanged, DateChanged)");
            builder.Append($"SELECT {ColumnSetEnum(trackingModel.Columns)},SYSTEM_USER, GETDATE() FROM {(string.Equals(operation.ToUpper(), "DELETE")? "deleted":"inserted ")} ");
            builder.Append("END");

            return builder.ToString();
        }

        public static void CreateTrackingMechanism(TableModel model, string operation)
        {
            var tableCreateString = GetCreateTableString(model);
            var triggerCreateString = GetCreateTriggerString(model, operation);

          //  SQLDatabase.CreateCommand(tableCreateString, System.Data.CommandType.Text);
          //  SQLDatabase.CreateCommand(triggerCreateString, System.Data.CommandType.Text);
        }

        public static bool CheckTableExists(string tableName)
        {
            string query = string.Format(@"SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{0}'", tableName);

            return SQLDatabase.ExecuteScalar<int>(query) > 0;
        }

        public static void CheckHistoryExists()
        {

        }

        public static List<ColumnModel> CheckTableHistoryChanges(TableModel currentTable, TableModel newModel)
        {
            if(currentTable.Columns.Count != newModel.Columns.Count)
            {
                List<ColumnModel> list = new List<ColumnModel>();
                if (newModel.Columns.Count > currentTable.Columns.Count)
                {   
                    var colNames = currentTable.Columns.Select(item => item.ColumnName);
                    var rejectList = newModel.Columns.Where(item => colNames.Contains(item.ColumnName));
                    var col = newModel.Columns.Except(rejectList);

                    list.AddRange(col);
                    list.AddRange(ChangedColumns(currentTable, newModel));

                }
                return list;
            }
            else
            {
                return ChangedColumns(currentTable, newModel);
            }


        }

        private static List<ColumnModel> ChangedColumns(TableModel currentTable, TableModel newModel)
        {
            List<ColumnModel> result = new List<ColumnModel>();

            for (int i = 0; i < currentTable.Columns.Count; i++)
            {
                if (currentTable.Columns[i].ColumnName != newModel.Columns[i].ColumnName ||
                   currentTable.Columns[i].DataType != newModel.Columns[i].DataType ||
                   currentTable.Columns[i].CharacterMaxLength != newModel.Columns[i].CharacterMaxLength)
                {
                    result.Add(newModel.Columns[i]);
                }
            }

            return result;
        }
    }
}
