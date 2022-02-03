using System;
using System.Collections;
using System.Collections.Generic;

namespace Banks.Repositories
{
    public interface IRepository<T>
    {
        T GetById(Guid id);
        bool ExistsWithId(Guid id);
        IEnumerable<T> GetAll();
        void Save(T value);
    }
}