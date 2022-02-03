using System.Collections.Generic;
using Shops.Entities;
using Shops.Services;

namespace Shops.Managers
{
    public class ProductManager
    {
        private readonly IService<Product> _productService;

        public ProductManager(IService<Product> productService)
        {
            _productService = productService;
        }

        public void AddProduct(Product product) => _productService.Save(product);

        public bool TryGetProduct(uint productId, out Product result)
            => _productService.TryGetById(productId, out result);

        public IReadOnlyCollection<Product> GetAllProducts() => _productService.GetAll();

        public bool ProductExists(uint productId) => _productService.ExistsWithId(productId);
    }
}