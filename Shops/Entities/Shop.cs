using System;
using System.Collections.Generic;
using Shops.Tools.Exceptions;

namespace Shops.Entities
{
    public class Shop : IIdable, IEquatable<Shop>
    {
        private static uint _counter;
        private readonly Dictionary<uint, ProductInfo> _productInfos;

        public Shop(string name, ShopAddress shopAddress)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Address = shopAddress ?? throw new ArgumentNullException(nameof(shopAddress));
            _productInfos = new Dictionary<uint, ProductInfo>();
            Id = ++_counter;
        }

        public Shop(Shop other)
        {
            Id = other.Id;
            Name = other.Name;
            Address = new ShopAddress(other.Address.Value);
            _productInfos = new Dictionary<uint, ProductInfo>(other._productInfos);
        }

        public uint Id { get; }

        public string Name { get; }

        public ShopAddress Address { get; }

        public ProductInfo GetProductInfo(uint productId)
        {
            if (!_productInfos.TryGetValue(productId, out ProductInfo productInfo))
                throw new ShopException($"Shop.GetProductInfo: no product with such 'productId' ({productId})");
            return productInfo;
        }

        public IReadOnlyCollection<ProductInfo> GetAllProductInfos() => _productInfos.Values;

        public void SetPrice(uint productId, Money price)
        {
            if (!_productInfos.TryGetValue(productId, out ProductInfo productInfo))
                throw new ShopException($"Shop.SetPrice: no product with such productId ({productId})");

            Product product = _productInfos[productId].Product;
            uint amount = _productInfos[productId].Amount;
            _productInfos[productId] = new ProductInfo(product, price, amount);
        }

        public Money GetCost(uint productId, uint amount)
        {
            Money price = GetProductInfo(productId).Price;
            return new Money(price.Value * amount);
        }

        public void AddProducts(ProductInfo productInfo)
        {
            ProductInfo newInfo = GetProductInfoAfterDelivery(productInfo);
            _productInfos[newInfo.Product.Id] = newInfo;
        }

        public void SellProduct(uint productId, uint amount) => ShipProduct(productId, amount);

        public bool HasEnoughProducts(uint productId, uint amount)
        {
            if (!_productInfos.ContainsKey(productId))
                return false;

            ProductInfo productInfo = GetProductInfo(productId);
            return productInfo.Amount >= amount;
        }

        public override bool Equals(object obj) => obj is Shop other && Id == other.Id;

        public bool Equals(Shop other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id == other.Id;
        }

        public override int GetHashCode() => Id.GetHashCode();

        private void ShipProduct(uint productId, uint amount)
        {
            if (!_productInfos.TryGetValue(productId, out ProductInfo productInfo))
                throw new ShopException();

            if (productInfo.Amount < amount)
                throw new ShopException("Shop.ShipProduct: unable to ship such amount of product");

            if (productInfo.Amount == amount)
            {
                _productInfos.Remove(productId);
            }
            else
            {
                productInfo.Amount -= amount;
            }
        }

        private ProductInfo GetProductInfoAfterDelivery(ProductInfo deliveryProductInfo)
        {
            Product product = deliveryProductInfo.Product;
            Money newPrice = deliveryProductInfo.Price;
            uint newAmount = deliveryProductInfo.Amount;

            if (_productInfos.TryGetValue(product.Id, out ProductInfo oldInfo))
                newAmount += oldInfo.Amount;

            return new ProductInfo(product, newPrice, newAmount);
        }
    }
}