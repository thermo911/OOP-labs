using System.Collections.Generic;
using System.Threading.Tasks;
using Reports.Shrd.Dto;

namespace Reports.Services.Services
{
    public interface IEmployeeService : IBasicService<EmployeeDto>
    {
        Task<IEnumerable<EmployeeDto>> GetByChiefIdAsync(int id);
    }
}