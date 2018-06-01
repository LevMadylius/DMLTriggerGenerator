using DMLTriggerGenerator.DAL.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMLTriggerGenerator.DAL.Model
{
    public class ColumnModel: IColumnModel
    {
        public string ColumnName { get; set; }
        public string DataType { get; set; }
        public IsNullable ISNullable{get;set;}
        public int? CharacterMaxLength { get; set; }
    }
}
