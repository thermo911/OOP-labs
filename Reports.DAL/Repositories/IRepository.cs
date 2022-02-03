using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Reports.DAL.Entities;

namespace Reports.DAL.Repositories
{
    public interface IRepository<T> where T : IIdentifiable
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetWhereAsync(Func<T, bool> predicate);
        Task<T> FindByIdAsync(int id);
        Task AddAsync(T value);
        Task UpdateAsync(T value);
        Task DeleteByIdAsync(int id);
        
    }
}