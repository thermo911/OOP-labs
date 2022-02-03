using Shops.Entities;
using Shops.Managers;
using Shops.Repositories;
using Shops.Repositories.Impl;
using Shops.Services;
using Shops.Services.Impl;

namespace Shops.Config
{
    public static class ManagerConfig
    {
        private static readonly IRepository<Shop> ShopRepository = new InMemoryShopRepository();
        private static readonly IRepository<Person> PersonRepository = new InMemoryPersonRepository();
        private static readonly IRepository<Product> ProductRepository = new InMemoryProductRepository();

        private static readonly IService<Shop> ShopService = new SimpleShopService(ShopRepository);
        private static readonly IService<Person> PersonService = new SimplePersonService(PersonRepository);
        private static readonly IService<Product> ProductService = new SimpleProductService(ProductRepository);

        private static ShopManager _shopManager;
        private static PersonManager _personManager;
        private static ProductManager _productManager;

        public static ShopManager ShopManager => _shopManager ??= ConfigureShopManager();
        public static PersonManager PersonManager => _personManager ??= ConfigurePersonManager();
        public static ProductManager ProductManager => _productManager ??= ConfigureProductManager();

        private static ShopManager ConfigureShopManager() => new (ShopService, PersonService, ProductService);
        private static PersonManager ConfigurePersonManager() => new (PersonService);
        private static ProductManager ConfigureProductManager() => new (ProductService);
    }
}