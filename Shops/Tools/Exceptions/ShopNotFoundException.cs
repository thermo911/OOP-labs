using System;

namespace Shops.Tools.Exceptions
{
    public class ShopNotFoundException : Exception
    {
        public ShopNotFoundException()
        {
        }

        public ShopNotFoundException(string message)
            : base(message)
        {
        }

        public ShopNotFoundException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}