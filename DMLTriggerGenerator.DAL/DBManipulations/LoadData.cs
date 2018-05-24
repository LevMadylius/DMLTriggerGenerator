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

        public static TableModel GetTableByName(string tableName)
        {
            var query = Scripts.GetColumScript(tableName);
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
    }
}
