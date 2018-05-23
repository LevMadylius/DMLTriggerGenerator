using DMLTriggerGenerator.Utils.CustomExceptions;
using System.Net;


namespace DMLTriggerGenerator.DAL.Model
{
    public static class ConnectionString
    {
        public static IPAddress IP { get; set; }
        public static int Port { get; set; }
        public static string InitialCatalog { get; set; }
        public static string UserId { get; set; }
        public static string Password { get; set; }


        public static string GetConnectionString()
        {
            if (IP == null || Port == 0 || string.IsNullOrEmpty(InitialCatalog) || string.IsNullOrEmpty(UserId) || string.IsNullOrEmpty(Password))
            {
                throw new ConnectionStringInvalidException("One of parameters is empty");
            }

            string connectionString = $"Data Source={IP.ToString()},{Port};Network Library=DBMSSOCN;Initial Catalog={InitialCatalog};User ID={UserId};Password={Password}";


            return connectionString;
        }
    }
}
