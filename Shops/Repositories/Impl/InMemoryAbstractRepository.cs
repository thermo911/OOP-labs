using System.Collections.Generic;
using Shops.Entities;
using Shops.Tools.Exceptions;

namespace Shops.Repositories.Impl
{
    public abstract class InMemoryAbstractRepository<T> : IRepository<T>
        where T : IIdable
    {
        private readonly Dictionary<uint, T> _items;

        protected InMemoryAbstractRepository() => _items = new Dictionary<uint, T>();

        public bool ExistsWithId(uint id) => _items.ContainsKey(id);

        public void DeleteById(uint id) => _items.Remove(id);

        public void Save(T item) => _items.Add(item.Id, item);

        public bool TryGetById(uint id, out T result) => _items.TryGetValue(id, out result);

        public IReadOnlyCollection<T> GetAll()
        {
            return _items.Values;
        }
    }
}