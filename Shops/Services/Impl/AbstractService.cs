using System.Collections.Generic;
using Shops.Entities;
using Shops.Repositories;
using Shops.Tools.Exceptions;

namespace Shops.Services.Impl
{
    public class AbstractService<T> : IService<T>
        where T : IIdable
    {
        private readonly IRepository<T> _repository;

        protected AbstractService(IRepository<T> repository) => _repository = repository;

        public void Save(T item) => _repository.Save(item);

        public void DeleteById(uint id) => _repository.DeleteById(id);

        public bool ExistsWithId(uint id) => _repository.ExistsWithId(id);

        public bool TryGetById(uint id, out T result) => _repository.TryGetById(id, out result);

        public IReadOnlyCollection<T> GetAll()
        {
            return _repository.GetAll();
        }
    }
}