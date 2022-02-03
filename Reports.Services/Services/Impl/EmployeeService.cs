using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Reports.DAL.Entities;
using Reports.DAL.Repositories;
using Reports.Shrd.Dto;
using Reports.Shrd.Mappers;

namespace Reports.Services.Services.Impl
{
    public class EmployeeService : IEmployeeService
    {
        private IRepository<Employee> _employeeRepository;
        private IMapper _mapper;

        public EmployeeService(IRepository<Employee> employeeRepository, IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<EmployeeDto>> GetAllAsync()
        {
            var all = await _employeeRepository.GetAllAsync();
            return all.Select(e => _mapper.Map<EmployeeDto>(e));
        }

        public async Task<EmployeeDto> FindByIdAsync(int id)
        {
            return _mapper.Map<EmployeeDto>(await _employeeRepository.FindByIdAsync(id));
        }

        public async Task AddAsync(EmployeeDto value)
        {
            await _employeeRepository.AddAsync(_mapper.Map<Employee>(value));
        }

        public async Task UpdateAsync(EmployeeDto value)
        {
            await _employeeRepository.UpdateAsync(_mapper.Map<Employee>(value));
        }

        public async Task DeleteByIdAsync(int id)
        {
            await _employeeRepository.DeleteByIdAsync(id);
        }

        public async Task<IEnumerable<EmployeeDto>> GetByChiefIdAsync(int id)
        {
            var employees = await _employeeRepository
                .GetWhereAsync(e => e.ChiefId == id);
            return employees.Select(e => _mapper.Map<EmployeeDto>(e));
        }
    }
}