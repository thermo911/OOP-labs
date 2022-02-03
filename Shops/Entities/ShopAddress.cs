using System;
using System.Runtime.InteropServices;
using Shops.Tools.Exceptions;

namespace Shops.Entities
{
    public class ShopAddress
    {
        public ShopAddress(string address)
        {
            Value = address ?? throw new ArgumentNullException(nameof(address));
        }

        public string Value { get; }
    }
}