using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Reports.DAL.Entities;
using Reports.DAL.Repositories;
using Reports.Shrd.Dto;
using Reports.Shrd.Mappers;

namespace Reports.Services.Services.Impl
{
    public class ReportService : IReportService
    {
        private IRepository<Report> _reportRepository;
        private IMapper _mapper;
        private IEmployeeService _employeeService;

        public ReportService(IRepository<Report> reportRepository, IMapper mapper, IEmployeeService employeeService)
        {
            _reportRepository = reportRepository;
            _mapper = mapper;
            _employeeService = employeeService;
        }

        public async Task<IEnumerable<ReportDto>> GetAllAsync()
        {
            var all = await _reportRepository.GetAllAsync();
            return all.Select(r => _mapper.Map<ReportDto>(r));
        }

        public async Task<ReportDto> FindByIdAsync(int id)
        {
            return _mapper.Map<ReportDto>(await _reportRepository.FindByIdAsync(id));
        }

        public async Task AddAsync(ReportDto value)
        {
            await _reportRepository.AddAsync(_mapper.Map<Report>(value));
        }

        public async Task UpdateAsync(ReportDto value)
        {
            await _reportRepository.UpdateAsync(_mapper.Map<Report>(value));
        }

        public async Task DeleteByIdAsync(int id)
        {
            await _reportRepository.DeleteByIdAsync(id);
        }

        public async Task<IEnumerable<ReportDto>> GetByChiefIdAsync(int chiefId, DateTime from, DateTime upTo)
        {
            IEnumerable<EmployeeDto> subordinates = await _employeeService.GetByChiefIdAsync(chiefId);
            var subIds = subordinates.Select(s => s.Id).ToHashSet();
            
            IEnumerable<Report> reports = await _reportRepository
                .GetWhereAsync(r =>
                    subIds.Contains(r.AuthorId) &&
                    from <= r.CreationDateTime &&
                    r.CreationDateTime < upTo);
            
            return reports.Select(r => _mapper.Map<ReportDto>(r));
        }
    }
}