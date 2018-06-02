using DMLTriggerGenerator.DAL.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMLTriggerGenerator.DAL.DBManipulations
{
    public static class TrackingInfo
    {
        public static string GetInfo(TrackingModel model, string[] operations)
        {
            StringBuilder builder = new StringBuilder();

            builder.Append($"Performing operations with table: {model.TableName} {Environment.NewLine}");
            // if table does not exist
            if (!TableOperations.CheckTableExists($"{model.TableName}_History"))
            {
                builder.Append($"Create tracking mechanism for:{Environment.NewLine}");
                foreach(var el in operations)
                {
                    //fix this stupid shitty bug
                    var resultArr = model.Columns.Where(m => (m.Insert != null && string.Equals(m.Insert.ToUpper(), el.ToUpper())) ||
                                                             (m.Update != null && string.Equals(m.Update.ToUpper(), el.ToUpper())) ||
                                                             (m.Delete != null && string.Equals(m.Delete.ToUpper(), el.ToUpper()))).ToList();
                    if(resultArr != null && resultArr.Count > 0)
                    {
                        builder.Append($"{el}:{Environment.NewLine}");
                        foreach(var res in resultArr)
                        {
                            builder.Append($"{res.ColumnName}, ");
                        }
                        builder.Length -= 2;
                        builder.Append($"{Environment.NewLine}");
                    }
                }
                return builder.ToString();
            }
            else // if table exists
            {
                var oldTrackingModel = LoadData.GetTrackingModelForTable(model.TableName);
               // var modelDifference = FindDifferencesBetweenModels(model, oldTrackingModel);
                builder.Append($"Tracking mechanism already exists.{Environment.NewLine}");

                List<List<TrackingColumn>> deletedTracking = DeletedTracking(oldTrackingModel, model);
                List<List<TrackingColumn>> addedTracking = AddedTracking(oldTrackingModel, model);

                // var addedTracking = modelDifference.Columns.Except(deletedTracking);

                if (CheckEmpty(deletedTracking) && CheckEmpty(addedTracking))
                {
                    builder.Append("You did not choose more options or removed existing ones.");
                }
                else
                {
                    if(!CheckEmpty(deletedTracking))
                    {
                        builder.Append($"Deleted actions{Environment.NewLine}");

                        if(deletedTracking[0].Count > 0)
                        {
                            builder.Append($"Insert:{Environment.NewLine}");
                            foreach(var el in deletedTracking[0])
                            {
                                builder.Append($"{el.ColumnName}, ");
                            }
                            builder.Length -= 2;
                            builder.Append($"{Environment.NewLine}");
                        }

                        if (deletedTracking[1].Count > 0)
                        {
                            builder.Append($"Update:{Environment.NewLine}");
                            foreach (var el in deletedTracking[1])
                            {
                                builder.Append($"{el.ColumnName}, ");
                            }
                            builder.Length -= 2;
                            builder.Append($"{Environment.NewLine}");
                        }

                        if (deletedTracking[2].Count > 0)
                        {
                            builder.Append($"Delete:{Environment.NewLine}");
                            foreach (var el in deletedTracking[2])
                            {
                                builder.Append($"{el.ColumnName}, ");
                            }
                            builder.Length -= 2;
                            builder.Append($"{Environment.NewLine}");
                        }


                    }
                    if(!CheckEmpty(addedTracking))
                    {
                        builder.Append($"Added actions: {Environment.NewLine}");

                        if (addedTracking[0].Count > 0)
                        {
                            builder.Append($"Insert:{Environment.NewLine}");
                            foreach (var el in addedTracking[0])
                            {
                                builder.Append($"{el.ColumnName}, ");
                            }
                            builder.Length -= 2;
                            builder.Append($"{Environment.NewLine}");
                        }

                        if (addedTracking[1].Count > 0)
                        {
                            builder.Append($"Update:{Environment.NewLine}");
                            foreach (var el in addedTracking[1])
                            {
                                builder.Append($"{el.ColumnName}, ");
                            }
                            builder.Length -= 2;
                            builder.Append($"{Environment.NewLine}");
                        }

                        if (addedTracking[2].Count > 0)
                        {
                            builder.Append($"Delete:{Environment.NewLine}");
                            foreach (var el in addedTracking[2])
                            {
                                builder.Append($"{el.ColumnName}, ");
                            }
                            builder.Length -= 2;
                            builder.Append($"{Environment.NewLine}");
                        }
                    }
                }

                return builder.ToString();
            }
        }

        //private static TrackingModel FindDifferencesBetweenModels(TrackingModel newModel, TrackingModel oldModel)
        //{
        //    var colDifference = new List<TrackingColumn>();
        //    foreach(var mod in oldModel.Columns)
        //    {
        //        var col = newModel.Columns.Where(el => string.Equals(el.ColumnName, mod.ColumnName) && el.Insert != mod.Insert &&
        //                                                                                               el.Update != mod.Update &&
        //                                                                                              el.Delete != mod.Delete).SingleOrDefault();
        //        if (col != null)
        //        {
        //            colDifference.Add(col);
        //        }
        //    }

             

        //    return new TrackingModel { TableName = newModel.TableName, Columns = colDifference};
        //}

        private static List<List<TrackingColumn>> DeletedTracking(TrackingModel oldModel, TrackingModel newModel)
        {
            List<List<TrackingColumn>> list = new List<List<TrackingColumn>>();

            List<TrackingColumn> deletedColsTrackingIns = new List<TrackingColumn>();
            foreach(var el in oldModel.Columns)
            {
                var col = newModel.Columns.Where(nm => nm.Insert == null && el.Insert != null && el.ColumnName == nm.ColumnName).SingleOrDefault();
                if(col != null)
                deletedColsTrackingIns.Add(col);
            }
            list.Add(deletedColsTrackingIns);
            List<TrackingColumn> deletedColsTrackingUpd = new List<TrackingColumn>();
            foreach (var el in oldModel.Columns)
            {
                var col = newModel.Columns.Where(nm => nm.Update == null && el.Update != null && el.ColumnName == nm.ColumnName).SingleOrDefault();
                if (col != null)
                    deletedColsTrackingUpd.Add(col);
            }
            list.Add(deletedColsTrackingUpd);
            List<TrackingColumn> deletedColsTrackingDel = new List<TrackingColumn>();
            foreach (var el in oldModel.Columns)
            {
                var col = newModel.Columns.Where(nm => nm.Delete == null && el.Delete != null && el.ColumnName == nm.ColumnName).SingleOrDefault();
                if (col != null)
                    deletedColsTrackingDel.Add(col);
            }
            list.Add(deletedColsTrackingDel);

            return list;
        }

        private static List<List<TrackingColumn>> AddedTracking(TrackingModel oldModel, TrackingModel newModel)
        {
            List<List<TrackingColumn>> list = new List<List<TrackingColumn>>();

            List<TrackingColumn> addedColsTrackingIns = new List<TrackingColumn>();
            foreach (var el in oldModel.Columns)
            {
                var col = newModel.Columns.Where(nm => nm.Insert != null && el.Insert == null && el.ColumnName == nm.ColumnName).SingleOrDefault();
                if (col != null)
                    addedColsTrackingIns.Add(col);
            }
            list.Add(addedColsTrackingIns);
            List<TrackingColumn> addedColsTrackingUpd = new List<TrackingColumn>();
            foreach (var el in oldModel.Columns)
            {
                var col = newModel.Columns.Where(nm => nm.Update != null && el.Update == null && el.ColumnName == nm.ColumnName).SingleOrDefault();
                if (col != null)
                    addedColsTrackingUpd.Add(col);
            }
            list.Add(addedColsTrackingUpd);
            List<TrackingColumn> addedColsTrackingDel = new List<TrackingColumn>();
            foreach (var el in oldModel.Columns)
            {
                var col = newModel.Columns.Where(nm => nm.Delete != null && el.Delete == null && el.ColumnName == nm.ColumnName).SingleOrDefault();
                if (col != null)
                    addedColsTrackingDel.Add(col);
            }
            list.Add(addedColsTrackingDel);

            return list;
        }

        private static bool CheckEmpty(List<List<TrackingColumn>> model)
        {
            bool res = (model[0].Count == 0 && model[1].Count == 0 && model[2].Count == 0) ? true : false;
            return res;
        }
    }
}
