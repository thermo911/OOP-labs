using System;
using System.Collections.Generic;
using Banks.Entities;
using Banks.Exceptions;

namespace Banks.Repositories.Impl.InMemory
{
    public class InMemRepository<T> : IRepository<T>
        where T : IEquatable<T>, IIdentifiable
    {
        protected Dictionary<Guid, T> Entities { get; } = new ();

        public T GetById(Guid id)
        {
            if (!Entities.TryGetValue(id, out T value))
                throw new NotFoundException($"element {nameof(value.GetType)} with id {id} not found");

            return value;
        }

        public bool ExistsWithId(Guid id) => Entities.ContainsKey(id);

        public IEnumerable<T> GetAll() => Entities.Values;

        public void Save(T value) => Entities[value.Id] = value;
    }
}