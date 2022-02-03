using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Reports.Shrd.Dto;

namespace Reports.Services.Services
{
    public interface ITicketService : IBasicService<TicketDto>
    {
        Task<IEnumerable<TicketDto>> GetByAssigneeIdAsync(int id);
        Task<IEnumerable<TicketDto>> GetUpdatedByEmployeeIdAsync(int id);
        Task<IEnumerable<TicketDto>> GetByAuthorIdAsync(int id);
        
        Task<IEnumerable<TicketDto>> GetByOpenDateAsync(DateTime from, DateTime upTo);
        Task<IEnumerable<TicketDto>> GetByUpdateDateAsync(DateTime from, DateTime upTo);
        Task<IEnumerable<TicketDto>> GetResolvedInPeriod(DateTime from, DateTime upTo);
    }
}