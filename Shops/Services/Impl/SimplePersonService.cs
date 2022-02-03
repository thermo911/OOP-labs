using Shops.Entities;
using Shops.Repositories;

namespace Shops.Services.Impl
{
    public class SimplePersonService : AbstractService<Person>
    {
        public SimplePersonService(IRepository<Person> repository)
            : base(repository)
        {
        }
    }
}