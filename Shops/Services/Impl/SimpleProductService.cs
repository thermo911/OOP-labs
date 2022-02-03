using Shops.Entities;
using Shops.Repositories;

namespace Shops.Services.Impl
{
    public class SimpleProductService : AbstractService<Product>
    {
        public SimpleProductService(IRepository<Product> repository)
            : base(repository)
        {
        }
    }
}