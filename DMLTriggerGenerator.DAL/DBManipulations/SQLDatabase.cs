using DMLTriggerGenerator.DAL.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMLTriggerGenerator.DAL.DBManipulations
{
    public static class SQLDatabase
    {
        private const int CommandTimeout = 180;

        public static void CreateCommand(string query, CommandType commandType = CommandType.Text, params SqlParameter[] arrParam)
        {
            string connectionString = ConnectionString.GetConnectionString();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    command.CommandTimeout = CommandTimeout;
                    command.CommandType = commandType;

                    if (arrParam != null)
                    {
                        foreach (SqlParameter param in arrParam)
                        {
                            command.Parameters.Add(param);
                        }
                    }

                    command.Connection.OpenAsync();
                    command.ExecuteNonQueryAsync();
                    command.Connection.Close();

                }
            }
        }

        public static DataTable ExecuteQuery(string query, CommandType commandType = CommandType.Text, params SqlParameter[] arrParam)
        {
            DataTable dt = new DataTable();
            
            string connectionString = ConnectionString.GetConnectionString();

            using (var connect = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand(query, connect))
                {

                    cmd.CommandTimeout = CommandTimeout;
                    cmd.CommandType = commandType;

                    if (arrParam != null)
                    {
                        foreach (SqlParameter param in arrParam)
                        {
                            cmd.Parameters.Add(param);
                        }
                    }

                    connect.Open();
                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }               

                    connect.Close();
                }
            }

            return dt;
        }
        // unfinished
        //    public static List<string> GetCollectionItems(string query, CommandType commandType = CommandType.Text, params SqlParameter[] arrParam)
        //    {
        //        List<string>searchResult = new List<string>();

        //        using (DataTable resultTable = ExecuteQuery(query, CommandType.Text, arrParam))
        //        {
        //            resultTable.
        //            foreach (DataRow row in resultTable.Rows)
        //            {
        //                row.
        //                var entity = new T();
        //                if (entity != null)
        //                    searchResult.Add(entity);
        //            }
        //        }
        //        return searchResult;
        //    }
        //}
    }
}
