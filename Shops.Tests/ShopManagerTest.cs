using NUnit.Framework;
using Shops.Config;
using Shops.Entities;
using Shops.Managers;

namespace Shops.Tests
{
    public class Tests
    {
        private ShopManager _shopManager;
        private PersonManager _personManager;
        private ProductManager _productManager;

        [SetUp]
        public void Setup()
        {
            _shopManager = ManagerConfig.ShopManager;
            _personManager = ManagerConfig.PersonManager;
            _productManager = ManagerConfig.ProductManager;
        }

        [TestCase(15u)]
        public void CreateShopAndAddProducts_CanBuyProducts(uint productAmount)
        {
            var shop = new Shop("Green's", new ShopAddress("Moscow"));
            _shopManager.AddShop(shop);
            var product = new Product("banana");
            _productManager.AddProduct(product);
            _shopManager.AddProductsToShop(shop.Id, product.Id, 15.0, productAmount);
            
            Assert.True(shop.HasEnoughProducts(product.Id, productAmount));
        }

        [TestCase(15.0, 20.0)]
        public void ChangePrice_PriceChanged(double oldPrice, double newPrice)
        {
            var shop = new Shop("Green's", new ShopAddress("Moscow"));
            _shopManager.AddShop(shop);
            var product = new Product("banana");
            _productManager.AddProduct(product);
            _shopManager.AddProductsToShop(shop.Id, product.Id, oldPrice, 15);
            _shopManager.UpdateProductPrice(shop.Id, product.Id, newPrice);

            Assert.AreEqual(newPrice, shop.GetProductInfo(product.Id).Price.Value);
        }

        [Test]
        public void TryGetCheapestShopId_ShopFound()
        {
            var product = new Product("banana");
            _productManager.AddProduct(product);

            var shop1 = new Shop("1", new ShopAddress("1"));
            _shopManager.AddShop(shop1);
            _shopManager.AddProductsToShop(shop1.Id, product.Id, 15.0, 15);
            
            var shop2 = new Shop("2", new ShopAddress("2"));
            _shopManager.AddShop(shop2);
            _shopManager.AddProductsToShop(shop2.Id, product.Id, 10.0, 15);
            
            var shop3 = new Shop("3", new ShopAddress("3"));
            _shopManager.AddShop(shop3);
            _shopManager.AddProductsToShop(shop3.Id, product.Id, 5.0, 10);

            Assert.True(_shopManager.TryGetCheapestShopId(product.Id, 11, out uint bestShop));
            Assert.AreEqual(shop2.Id, bestShop);
            
            Assert.False(_shopManager.TryGetCheapestShopId(product.Id, 100, out bestShop));
        }

        [TestCase(15u, 10u, 15.0, 10000.0)]
        public void PurchaseTest_ProductCountAndPersonBalanceChanged(
            uint amountBefore, 
            uint amountRequired, 
            double productPrice,
            double personBalance)
        {
            var shop = new Shop("Green's", new ShopAddress("Moscow"));
            _shopManager.AddShop(shop);
            var product = new Product("banana");
            _productManager.AddProduct(product);
            _shopManager.AddProductsToShop(shop.Id, product.Id, productPrice, amountBefore);
        
            var person = new Person("Tom", new Money(personBalance));
            _personManager.AddPerson(person);
        
            _shopManager.PerformPurchase(person.Id, shop.Id, product.Id, amountRequired);
            
            Assert.AreEqual(amountBefore - amountRequired, shop.GetProductInfo(product.Id).Amount);
            Assert.AreEqual(personBalance - amountRequired * productPrice, person.Balance.Value);
        }
    }
}