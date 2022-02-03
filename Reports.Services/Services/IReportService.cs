using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Reports.Shrd.Dto;

namespace Reports.Services.Services
{
    public interface IReportService : IBasicService<ReportDto>
    {
        Task<IEnumerable<ReportDto>> GetByChiefIdAsync(int chiefId, DateTime from, DateTime upTo);
    }
}