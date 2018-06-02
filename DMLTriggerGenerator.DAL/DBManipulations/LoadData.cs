using System;
using System.Collections.Generic;
using DMLTriggerGenerator.Utils.Helpers;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DMLTriggerGenerator.DAL.Model;
using System.Data;

namespace DMLTriggerGenerator.DAL.DBManipulations
{
    public static class LoadData
    {
        public static List<string> GetTableNames()
        {
            return DbTypeConverter.TablesNamesFromDataTable(SQLDatabase.ExecuteQuery(Scripts.GetTablesNamesScript));
        }

        public static TableModel GetTableModelByName(string tableName)
        {
            var query = Scripts.GetColumsScript(tableName);
            var dataTable = SQLDatabase.ExecuteQuery(query, CommandType.Text);
            int tempCharMax = 0;
            List<ColumnModel> columnsList = new List<ColumnModel>();
            foreach(DataRow row in dataTable.Rows)
            {
                columnsList.Add(new ColumnModel
                {
                    ColumnName = row["COLUMN_NAME"].ToString(),
                    DataType = row["DATA_TYPE"].ToString(),
                    ISNullable = (string.Equals(row["IS_NULLABLE"].ToString(), "YES") ? Enums.IsNullable.YES : Enums.IsNullable.NO),
                    CharacterMaxLength =  int.TryParse(row["CHARACTER_MAXIMUM_LENGTH"].ToString(), out tempCharMax)? tempCharMax : (int?)null
                });               
            }

            return new TableModel { TableName = tableName, Columns = columnsList };
        }

        public static List<string> GetAllColumnsNamesFromTable(string tableName)
        {
            var query = Scripts.GetColumsScript(tableName);
            var dataTable = SQLDatabase.ExecuteQuery(query, CommandType.Text);
            List<string> columnList = new List<string>();

            foreach(DataRow row in dataTable.Rows)
            {
                columnList.Add(row["COLUMN_NAME"].ToString());
            }

            return columnList;
        }

        public static TrackingModel GetTrackingModelForTable(string tableName)
        {
            if(!TableOperations.CheckTableExists($"{tableName}_OperationsStored"))
            {
                return null;
            }

            var query = Scripts.SelectFromTableOperations(tableName);
            var dataTable = SQLDatabase.ExecuteQuery(query, CommandType.Text);
            var trackinColsList = new List<TrackingColumn>();

            foreach(DataRow row in dataTable.Rows)
            {
                var test = row["INSERTOPER"].ToString();
                trackinColsList.Add(new TrackingColumn
                {
                    
                    ColumnName = row["COL_NAME"].ToString(),
                    Insert = string.Equals(row["INSERTOPER"].ToString(), "True") ? "INSERT" : null,
                    Update = string.Equals(row["UPDATEOPER"].ToString(), "True") ? "UPDATE" : null,
                    Delete = string.Equals(row["DELETEOPER"].ToString(), "True")? "DELETE" : null 
                });
            }
            return new TrackingModel { TableName = tableName, Columns = trackinColsList };
        }

        public static TrackingModel GetNewTrackingModel(string tableName)
        {
            var cols = GetAllColumnsNamesFromTable(tableName);
            List<TrackingColumn> columns = cols.Select(el => new TrackingColumn { ColumnName = el }).ToList();

            return new TrackingModel { TableName = tableName, Columns = columns };
        }
    }
}
