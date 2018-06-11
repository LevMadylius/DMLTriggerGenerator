using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DMLTriggerGenerator.DAL.Model;
namespace DMLTriggerGenerator.ViewModel
{

    public class ColumnsViewModel
    {
        public List<ColumnsElementViewModel> ColumnsElements { get; set; }
        public List<TriggerModel> Triggers { get; set; }
    }
    public class ColumnsElementViewModel : ColumnModel
    {
        public bool Insert { get; set; }
        public bool Update { get; set; }
        public bool Delete { get; set; }
    }

    public static class ColumnsViewModelHelper
    {
        public static List<ColumnsElementViewModel> FormList(List<ColumnModel> colsList, List<TrackingColumn> trackingList)
        {
            var resultList = new List<ColumnsElementViewModel>();
            if (trackingList == null || trackingList.Count == 0)
            {
                resultList = colsList.Select(el => new ColumnsElementViewModel
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
                    resultList.Add(new ColumnsElementViewModel
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