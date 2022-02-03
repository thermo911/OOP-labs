using System;

namespace Shops.Tools.Exceptions
{
    public class InvalidArgumentException : ArgumentException
    {
        public InvalidArgumentException()
        {
        }

        public InvalidArgumentException(string message)
            : base(message)
        {
        }

        public InvalidArgumentException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}