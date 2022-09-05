using System;

namespace Data.Exceptions
{
    public class DatabaseException : DataException
    {
        public DatabaseException(string message) : base(message) { }
        public DatabaseException(string message, Exception innerException) : base(message, innerException) { }
    }
}
