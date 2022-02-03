using System;
using System.Collections;
using System.Runtime.Serialization;

namespace Shops.Tools.Exceptions
{
    public class ShopException : Exception
    {
        public ShopException()
        {
        }

        public ShopException(string message)
            : base(message)
        {
        }

        public ShopException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}