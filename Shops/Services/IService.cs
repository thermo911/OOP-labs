using System;
using System.Collections.Generic;
using Shops.Entities;

namespace Shops.Services
{
    public interface IService<T>
        where T : IIdable
    {
        void Save(T item);
        void DeleteById(uint id);
        bool ExistsWithId(uint id);
        bool TryGetById(uint id, out T result);

        IReadOnlyCollection<T> GetAll();
    }
}