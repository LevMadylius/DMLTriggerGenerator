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

        public static TableModel GetTableModelByName(TrackingModel trackingModel)
        {
            var result = new TableModel();
            result.TableName = trackingModel.TableName;
            int tempCharMax = 0;

            var listCols = trackingModel.Columns.Select(el => el.ColumnName).ToList();
            var stringCols = TableOperations.ColumnSetFromInStatement(listCols);
            var query = Scripts.GetColumsScript(trackingModel.TableName, stringCols);
            var elements = SQLDatabase.ExecuteQuery(query);

            List<ColumnModel> columnsList = new List<ColumnModel>();
            foreach (DataRow row in elements.Rows)
            {
                columnsList.Add(new ColumnModel
                {
                    ColumnName = row["COLUMN_NAME"].ToString(),
                    DataType = row["DATA_TYPE"].ToString(),
                    ISNullable = (string.Equals(row["IS_NULLABLE"].ToString(), "YES") ? Enums.IsNullable.YES : Enums.IsNullable.NO),
                    CharacterMaxLength = int.TryParse(row["CHARACTER_MAXIMUM_LENGTH"].ToString(), out tempCharMax) ? tempCharMax : (int?)null
                });
            }
            return new TableModel { TableName = trackingModel.TableName, Columns = columnsList };
        }
    }
}
