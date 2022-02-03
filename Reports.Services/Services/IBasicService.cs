using System.Collections.Generic;
using System.Threading.Tasks;
using Reports.DAL.Entities;
using Reports.Shrd.Dto;

namespace Reports.Services.Services
{
    public interface IBasicService<T> where T : IDto
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> FindByIdAsync(int id);
        Task AddAsync(T value);
        Task UpdateAsync(T value);
        Task DeleteByIdAsync(int id);
    }
}