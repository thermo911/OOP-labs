using Shops.Entities;
using Shops.Repositories;

namespace Shops.Services.Impl
{
    public class SimpleShopService : AbstractService<Shop>
    {
        public SimpleShopService(IRepository<Shop> repository)
            : base(repository)
        {
        }
    }
}