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
    public class CommentService : ICommentService
    {
        private IRepository<Comment> _commentRepository;

        private IRepository<Ticket> _ticketRepository;
        //private ITicketService _ticketService;
        private IMapper _mapper;

        public CommentService(
            IRepository<Comment> commentRepository,
            IMapper mapper,
            IRepository<Ticket> ticketRepository)
        {
            _commentRepository = commentRepository;
            //_ticketService = ticketService;
            _mapper = mapper;
            _ticketRepository = ticketRepository;
        }
        
        public async Task<IEnumerable<CommentDto>> GetAllAsync()
        {
            var all = await _commentRepository.GetAllAsync();
            return all.Select(c => _mapper.Map<CommentDto>(c));
        }

        public async Task<CommentDto> FindByIdAsync(int id)
        {
            return _mapper.Map<CommentDto>(await _commentRepository.FindByIdAsync(id));
        }

        public async Task AddAsync(CommentDto value)
        {
            Task addCommentTask  = _commentRepository.AddAsync(_mapper.Map<Comment>(value));
            Ticket ticket = await _ticketRepository.FindByIdAsync(value.TicketId);
            Task updateTicketTask = _ticketRepository.UpdateAsync(ticket);
            Task.WaitAll(updateTicketTask, addCommentTask);
        }

        public async Task UpdateAsync(CommentDto value)
        {
            Task updateCommentTask  = _commentRepository.AddAsync(_mapper.Map<Comment>(value));
            Ticket ticket = await _ticketRepository.FindByIdAsync(value.TicketId);
            Task updateTicketTask = _ticketRepository.UpdateAsync(ticket);
            Task.WaitAll(updateTicketTask, updateCommentTask);
        }

        public async Task DeleteByIdAsync(int id)
        {
            Task deleteCommentTask =  _commentRepository.DeleteByIdAsync(id);
            var comment = await _commentRepository.FindByIdAsync(id);
            Ticket ticket = await _ticketRepository.FindByIdAsync(comment.TicketId);
            Task updateTicketTask = _ticketRepository.UpdateAsync(ticket);
            Task.WaitAll(updateTicketTask, deleteCommentTask);
        }

        public async Task<IEnumerable<CommentDto>> GetByAuthorIdAsync(int id)
        {
            var comments = await _commentRepository
                .GetWhereAsync(c => c.AuthorId == id);
            return comments.Select(c => _mapper.Map<CommentDto>(c));
        }

        public async Task<IEnumerable<CommentDto>> GetByAuthorAndDateAsync(int id, DateTime from, DateTime upTo)
        {
            var comments = await _commentRepository
                .GetWhereAsync(c => 
                    c.AuthorId == id && from <= c.CreationDateTime && c.CreationDateTime < upTo);
            return comments.Select(c => _mapper.Map<CommentDto>(c));
        }
    }
}