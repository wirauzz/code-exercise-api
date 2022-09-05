using System;

namespace Data.Exceptions
{
    public class DBConnetionException : Exception
    {
        public DBConnetionException(string message) : base(message) { }
        public DBConnetionException(string message, Exception innerException) : base(string.Format(message), innerException) { }
    }
}