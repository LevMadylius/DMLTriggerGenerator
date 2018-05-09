using System;
using System.Runtime.Serialization;

namespace DMLTriggerGenerator.Utils.CustomExceptions
{
    public class ConnectionStringInvalidException : Exception
    {
        public ConnectionStringInvalidException():base()
        {

        }
        public ConnectionStringInvalidException(string message) : base(message)
        {
        }

        public ConnectionStringInvalidException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ConnectionStringInvalidException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
