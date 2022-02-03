using System;
using System.Collections.Generic;
using Shops.Entities;

namespace Shops.Repositories
{
    public interface IRepository<T>
        where T : IIdable
    {
        bool ExistsWithId(uint id);
        void DeleteById(uint id);
        void Save(T item);

        bool TryGetById(uint id, out T result);

        IReadOnlyCollection<T> GetAll();
    }
}