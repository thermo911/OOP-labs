using System;
using System.Collections.Generic;
using System.Linq;
using Shops.Entities;
using Shops.Services;
using Shops.Tools.Exceptions;

namespace Shops.Managers
{
    public class ShopManager
    {
        private readonly IService<Shop> _shopService;
        private readonly IService<Person> _personService;
        private readonly IService<Product> _productService;

        public ShopManager(
            IService<Shop> shopService,
            IService<Person> personService,
            IService<Product> productService)
        {
            _shopService = shopService;
            _personService = personService;
            _productService = productService;
        }

        public void AddShop(Shop shop) => _shopService.Save(shop);

        public void UpdateProductPrice(uint shopId, uint productId, double newPrice)
        {
            if (!_shopService.TryGetById(shopId, out Shop shop))
                throw new ShopNotFoundException($"Shop with id {shopId} not found");

            if (!_productService.ExistsWithId(productId))
                throw new ShopException($"Product with id {productId} not found");

            var price = new Money(newPrice);
            shop.SetPrice(productId, price);
        }

        public void AddProductsToShop(uint shopId, uint productId, double productPrice, uint productAmount)
        {
            if (!_shopService.TryGetById(shopId, out Shop shop))
                throw new ShopNotFoundException($"Shop with id {shopId} not found");

            if (!_productService.TryGetById(productId, out Product product))
                throw new ProductNotFoundException($"Product with id {productId} not found");

            var price = new Money(productPrice);
            var productInfo = new ProductInfo(product, price, productAmount);
            shop.AddProducts(productInfo);
        }

        public bool TryGetCheapestShopId(uint productId, uint amount, out uint result)
        {
            if (!_productService.ExistsWithId(productId))
                throw new ProductNotFoundException($"Product with id {productId} not found");
            result = 0;

            try
            {
                Shop shop = _shopService.GetAll()
                    .Where(s => s.HasEnoughProducts(productId, amount))
                    .Aggregate((s1, s2) =>
                        s1.GetCost(productId, amount).Value < s2.GetCost(productId, amount).Value ? s1 : s2);
                result = shop.Id;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool ShopHasProduct(uint shopId, uint productId)
            => ShopHasEnoughProducts(shopId, productId, 1U);

        public bool ShopHasEnoughProducts(uint shopId, uint productId, uint amount)
        {
            if (_shopService.TryGetById(shopId, out Shop shop))
                throw new ShopNotFoundException($"Shop with id {shopId} not found");

            if (!_shopService.ExistsWithId(productId))
                throw new ProductNotFoundException($"Product with id {productId} not found");

            return shop.HasEnoughProducts(productId, amount);
        }

        public IReadOnlyCollection<Shop> GetAllShops() => _shopService.GetAll();

        public bool ShopExists(uint shopId) => _shopService.ExistsWithId(shopId);

        public double GetProductCostAtShop(uint shopId, uint productId, uint amount)
        {
            if (_shopService.TryGetById(shopId, out Shop shop))
                throw new ShopNotFoundException($"Shop with id {shopId} not found");

            if (!_shopService.ExistsWithId(productId))
                throw new ProductNotFoundException($"Product with id {productId} not found");

            return shop.GetCost(productId, amount).Value;
        }

        public bool TryGetShop(uint shopId, out Shop result)
            => _shopService.TryGetById(shopId, out result);

        public void PerformPurchase(uint personId, uint shopId, uint productId, uint amount)
        {
            if (!_productService.ExistsWithId(productId))
                throw new ProductNotFoundException($"Product with id {productId} not found");

            if (!_shopService.TryGetById(shopId, out Shop shop))
                throw new ShopNotFoundException($"Shop with id {shopId} not found");

            if (!shop.HasEnoughProducts(productId, amount))
            {
                throw new ShopException(
                    $"Shop with id {productId} do not have enough products with id {productId}");
            }

            if (!_personService.TryGetById(personId, out Person person))
                throw new PersonNotFoundException($"Person woth id {personId} not found");

            Money cost = shop.GetCost(productId, amount);
            if (!person.CanAffordPurchase(cost))
                throw new ShopException($"Person with id {personId} have balance lower than {cost.Value}");

            person.SpendMoney(cost);
            shop.SellProduct(productId, amount);
        }
    }
}