using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Exceptions
{
    public class DataException : Exception
    {
        public DataException(string message) : base(message) { }
        public DataException(string message, Exception innerException) : base(string.Format(message), innerException) { }
    }
}
