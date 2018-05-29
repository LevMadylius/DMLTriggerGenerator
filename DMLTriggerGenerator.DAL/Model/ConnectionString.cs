using DMLTriggerGenerator.Utils.CustomExceptions;
using System.Net;


namespace DMLTriggerGenerator.DAL.Model
{
    public class ConnectionString
    {
        public string IP { get; set; }
        public int Port { get; set; }
        public string InitialCatalog { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }

        public string GetConnectionString()
        {
            if (IP == null || Port == 0 || string.IsNullOrEmpty(InitialCatalog) || string.IsNullOrEmpty(UserId) || string.IsNullOrEmpty(Password))
            {
                throw new ConnectionStringInvalidException("One of parameters is empty");
            }

            string connectionString = $"Data Source={IP},{Port};Network Library=DBMSSOCN;Initial Catalog={InitialCatalog};User ID={UserId};Password={Password}";

            return connectionString;
        }
    }
}
