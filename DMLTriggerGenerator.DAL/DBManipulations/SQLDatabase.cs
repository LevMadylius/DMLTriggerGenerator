using DMLTriggerGenerator.DAL.Model;
using DMLTriggerGenerator.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DMLTriggerGenerator.DAL.DBManipulations
{
    public static class SQLDatabase
    {
        private const int CommandTimeout = 180;
        private static HttpContextSessionWrapper _sessionWrapper = new HttpContextSessionWrapper();


        public static void CreateCommand(string query, CommandType commandType = CommandType.Text, params SqlParameter[] arrParam)
        {
            string connectionString = _sessionWrapper.ConnectionString;

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

                    command.Connection.Open();
                    command.ExecuteNonQuery();
                    command.Connection.Close();

                }
            }
        }

        public static DataTable ExecuteQuery(string query, CommandType commandType = CommandType.Text, params SqlParameter[] arrParam)
        {
            DataTable dt = new DataTable();
            
            string connectionString = _sessionWrapper.ConnectionString;

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

        internal static T ExecuteScalar<T>(string query, CommandType commandType = CommandType.Text, params SqlParameter[] arrParam)
        {
            T result;

            string connectionString = _sessionWrapper.ConnectionString;

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
                    var value = cmd.ExecuteScalar();
                    connect.Close();
                    if (value == DBNull.Value || value == null)
                        result = default(T);
                    else if (value is INullable)
                        result = ((INullable)value).IsNull ? default(T) : (T)value;
                    else
                        result = (T)value;
                } 
            }
            return result;
        }

        public static bool VerifyConnectivity(string connectionString)
        {
            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = connectionString;
                try
                {
                    connection.Open();
                    connection.Close();
                }
                catch (SqlException ex)
                {
                    throw;
                }
                return true;
            }
        }
    }
}
