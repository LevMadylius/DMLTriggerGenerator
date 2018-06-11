using DMLTriggerGenerator.DAL.Model;
using DMLTriggerGenerator.Utils.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMLTriggerGenerator.DAL.DBManipulations
{
    public static class TriggerOperations
    {
        public static void SetTriggerState(TriggerModel model, string tableName)
        {
            var query =  Scripts.GetTriggerStateQuery(tableName, model.Name, model.IsDisabled);
            SQLDatabase.CreateCommand(query);
        }
    }
}
