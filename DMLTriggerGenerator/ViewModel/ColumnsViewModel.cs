using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DMLTriggerGenerator.DAL.Model;
namespace DMLTriggerGenerator.ViewModel
{
    public class ColumnsViewModelElement: ColumnModel
    {
        public bool Insert { get; set; }
        public bool Update { get; set; }
        public bool Delete { get; set; }
    }

    public static class ColumnsViewModelHelper
    {
        public static List<ColumnsViewModelElement> FormList(List<ColumnModel> colsList, List<TrackingColumn> trackingList)
        {
            var resultList = new List<ColumnsViewModelElement>();
            if (trackingList == null || !trackingList.Any())
            {
                resultList = colsList.Select(el => new ColumnsViewModelElement
                {
                    ColumnName = el.ColumnName,
                    DataType = el.DataType,
                    CharacterMaxLength = el.CharacterMaxLength,
                    ISNullable = el.ISNullable,
                    Insert = false,
                    Update = false,
                    Delete = false
                }).ToList();
            }
            else
            {

                foreach (var col in colsList)
                {
                    var test = trackingList.Where(el => string.Equals(el.ColumnName, col.ColumnName)).Select(el => el.Insert != null).SingleOrDefault();
                    resultList.Add(new ColumnsViewModelElement
                    {                       
                        ColumnName = col.ColumnName,
                        CharacterMaxLength = col.CharacterMaxLength,
                        DataType = col.DataType,
                        ISNullable = col.ISNullable,
                        Insert = trackingList.Where(el => string.Equals(el.ColumnName, col.ColumnName)).Select(el => el.Insert != null).SingleOrDefault(),
                        Update = trackingList.Where(el => string.Equals(el.ColumnName, col.ColumnName)).Select(el => el.Update != null).SingleOrDefault(),
                        Delete = trackingList.Where(el => string.Equals(el.ColumnName, col.ColumnName)).Select(el => el.Delete != null).SingleOrDefault(),
                    });
                }
            }

            return resultList;
        }
    }
}