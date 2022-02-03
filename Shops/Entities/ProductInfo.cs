using System;
using Shops.Tools.Exceptions;

namespace Shops.Entities
{
    public class ProductInfo
    {
        private uint _amount;
        public ProductInfo(Product product, Money price, uint amount)
        {
            Product = product ?? throw new ArgumentNullException(nameof(product));
            Price = price;
            if (!IsValidProductAmount(amount))
                throw new InvalidArgumentException("ProductInfo constructor: argument 'amount' equals to 0");
            Amount = amount;
        }

        public ProductInfo(ProductInfo productInfo)
        {
            Product = productInfo.Product;
            Price = productInfo.Price;
            Amount = productInfo.Amount;
        }

        public Product Product { get; }

        public Money Price { get; set; }

        public uint Amount
        {
            get => _amount;
            set
            {
                if (!IsValidProductAmount(value))
                    throw new ShopException("Illegal state of ProductInfo: Amount equals to 0");
                _amount = value;
            }
        }

        private static bool IsValidProductAmount(uint amount) => amount != 0;
    }
}