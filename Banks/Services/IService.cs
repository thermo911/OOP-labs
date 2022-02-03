using System;
using System.Collections;
using System.Collections.Generic;

namespace Banks.Services
{
    public interface IService<T>
    {
        T GetById(Guid id);
        void Save(T value);
        IEnumerable<T> GetAll();
        bool ExistsWithId(Guid id);
    }
}