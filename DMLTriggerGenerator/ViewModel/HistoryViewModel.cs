using DMLTriggerGenerator.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DMLTriggerGenerator.ViewModel
{
    public class HistoryViewModel
    {
        public List<string> ColumnNames { get; set; }
        public List<RecordModel> Records { get; set; }
    }
}