namespace DMLTriggerGenerator.Utils.Helpers
{
    public static class Scripts
    {
        public static readonly string GetTablesNamesScript = "SELECT TABLE_NAME AS TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' ORDER BY TABLE_NAME";
        public static readonly string DefaultTransactionName = "DMLTriggerTransaction";
        public static string GetColumScript(string tableName)
        {
            return $"SELECT cols.COLUMN_NAME, cols.IS_NULLABLE, cols.DATA_TYPE, cols.CHARACTER_MAXIMUM_LENGTH FROM INFORMATION_SCHEMA.COLUMNS cols WHERE TABLE_NAME = '{tableName}' ORDER BY cols.TABLE_NAME";
        }



        public static string GetTransactionWrapper(string rawQuery, string tranName = "")
        {
            return $"BEGIN TRANSACTION [{tranName}] BEGIN TRY {rawQuery} COMMIT TRANSACTION [{tranName}] END TRY BEGIN CATCH ROLLBACK TRANSACTION [{tranName}] END CATCH GO;";
        }
    }
}
