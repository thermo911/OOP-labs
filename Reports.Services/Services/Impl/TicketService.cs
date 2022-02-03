using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Reports.DAL.Entities;
using Reports.DAL.Repositories;
using Reports.DAL.Tools;
using Reports.Shrd.Dto;
using Reports.Shrd.Mappers;

namespace Reports.Services.Services.Impl
{
    public class TicketService : ITicketService
    {
        private IRepository<Ticket> _ticketRepository;
        private ICommentService _commentService;
        private IMapper _mapper;

        public TicketService(
            IRepository<Ticket> ticketRepository,
            ICommentService commentService,
            IMapper mapper)
        {
            _ticketRepository = ticketRepository;
            _commentService = commentService;
            _mapper = mapper;
        }


        public async Task<IEnumerable<TicketDto>> GetAllAsync()
        {
            var all = await _ticketRepository.GetAllAsync();
            return all.Select(e => _mapper.Map<TicketDto>(e));
        }

        public async Task<TicketDto> FindByIdAsync(int id)
        {
            return _mapper.Map<TicketDto>(await _ticketRepository.FindByIdAsync(id));
        }

        public async Task AddAsync(TicketDto value)
        {
            value.OpenDateTime = DateTime.Now;
            value.UpdateDateTime = DateTime.Now;
            await _ticketRepository.AddAsync(_mapper.Map<Ticket>(value));
        }

        public async Task UpdateAsync(TicketDto value)
        {
            value.UpdateDateTime = DateTime.Now;
            await _ticketRepository.UpdateAsync(_mapper.Map<Ticket>(value));
        }

        public async Task DeleteByIdAsync(int id)
        {
            await _ticketRepository.DeleteByIdAsync(id);
        }

        public async Task<IEnumerable<TicketDto>> GetByAssigneeIdAsync(int id)
        {
            var tickets = await _ticketRepository
                .GetWhereAsync(ticket => ticket.AssigneeId == id);
            return tickets.Select(t => _mapper.Map<TicketDto>(t));
        }

        public async Task<IEnumerable<TicketDto>> GetUpdatedByEmployeeIdAsync(int id)
        {
            var commentDtos = await _commentService.GetByAuthorIdAsync(id);
            var authorIds = commentDtos.Select(c => c.AuthorId).ToHashSet();

            var tickets = await _ticketRepository.GetWhereAsync(t => authorIds.Contains(t.Id));
            return tickets.Select(t => _mapper.Map<TicketDto>(t));
        }

        public async Task<IEnumerable<TicketDto>> GetByAuthorIdAsync(int id)
        {
            var tickets = await _ticketRepository
                .GetWhereAsync(t => t.AuthorId == id);
            return tickets.Select(t => _mapper.Map<TicketDto>(t));
        }

        public async Task<IEnumerable<TicketDto>> GetByOpenDateAsync(DateTime from, DateTime upTo)
        {
            var tickets = await _ticketRepository
                .GetWhereAsync(t => from <= t.OpenDateTime && t.OpenDateTime < upTo);
            return tickets.Select(t => _mapper.Map<TicketDto>(t));
        }

        public async Task<IEnumerable<TicketDto>> GetByUpdateDateAsync(DateTime from, DateTime upTo)
        {
            var tickets = await _ticketRepository
                .GetWhereAsync(t => from <= t.OpenDateTime && t.OpenDateTime < upTo);
            return tickets.Select(t => _mapper.Map<TicketDto>(t));
        }

        public async Task<IEnumerable<TicketDto>> GetResolvedInPeriod(DateTime from, DateTime upTo)
        {
            var tickets = await GetByUpdateDateAsync(from, upTo);
            return tickets
                .Where(t => t.Status == TicketStatus.Resolved)
                .Select(t => _mapper.Map<TicketDto>(t));
        }
    }
}