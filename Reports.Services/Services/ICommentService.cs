using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Reports.Shrd.Dto;

namespace Reports.Services.Services
{
    public interface ICommentService : IBasicService<CommentDto>
    {
        Task<IEnumerable<CommentDto>> GetByAuthorIdAsync(int id);
        Task<IEnumerable<CommentDto>> GetByAuthorAndDateAsync(int id, DateTime from, DateTime upTo);
    }
}