namespace DMLTriggerGenerator.Utils.Helpers
{
    public static class Scripts
    {
        public static readonly string GetTablesNamesScript = "SELECT TABLE_NAME AS TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE' AND ((TABLE_NAME NOT LIKE '%_History') AND (TABLE_NAME NOT LIKE '%_OperationsStored')) ORDER BY TABLE_NAME";
        public static readonly string DefaultTransactionName = "DMLTriggerTransaction";
        public static string GetColumsScript(string tableName)
        {
            return $"SELECT cols.COLUMN_NAME, cols.IS_NULLABLE, cols.DATA_TYPE, cols.CHARACTER_MAXIMUM_LENGTH FROM INFORMATION_SCHEMA.COLUMNS cols WHERE TABLE_NAME = '{tableName}' ORDER BY cols.TABLE_NAME";
        }

        public static string DropTrigger(string triggerName)
        {
            return $"IF OBJECT_ID ('{triggerName}', 'TR') IS NOT NULL BEGIN  DROP TRIGGER {triggerName} END; ";
        }

        public static string GetTriggersForTable(string tableName)
        {
            return $"SELECT OBJECT_NAME(object_id) as TriggerName, is_disabled FROM sys.triggers WHERE OBJECT_NAME(parent_id) = '{tableName}'";
        }

        public static string GetTriggerStateQuery(string tableName, string triggerName, bool isDisabled)
        {
            return $"{(isDisabled? "DISABLE": "ENABLE")} TRIGGER {triggerName} ON {tableName}";
        }

        public static string GetColumsScript(string tableName, string columnNames)
        {
            return $"SELECT cols.COLUMN_NAME, cols.IS_NULLABLE, cols.DATA_TYPE, cols.CHARACTER_MAXIMUM_LENGTH FROM INFORMATION_SCHEMA.COLUMNS cols WHERE TABLE_NAME = '{tableName}' AND COLUMN_NAME IN {columnNames} ORDER BY cols.TABLE_NAME";
        }

        public static string GetTransactionWrapper(string rawQuery, string tranName = "")
        {
            return $"BEGIN TRANSACTION [{tranName}] BEGIN TRY {rawQuery} COMMIT TRANSACTION [{tranName}] END TRY BEGIN CATCH ROLLBACK TRANSACTION [{tranName}] END CATCH GO;";
        }

        public static string SelectFromTableOperations(string tableName)
        {
            return $"SELECT COL_NAME, INSERTOPER, UPDATEOPER, DELETEOPER FROM {tableName}_OperationsStored";

        }

    }
}
