using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Reports.DAL.Context;
using Reports.DAL.Entities;

namespace Reports.DAL.Repositories.Impl
{
    public class CommentRepository : IRepository<Comment>
    {
        private ReportsContext _context;

        public CommentRepository(ReportsContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Comment>> GetAllAsync()
        {
            return await _context.Comments.ToArrayAsync();
        }

        public async Task<IEnumerable<Comment>> GetWhereAsync(Func<Comment, bool> predicate)
        {
            return await Task.Run(() => _context.Comments.Where(predicate));
        }

        public async Task<Comment> FindByIdAsync(int id)
        {
            return await _context.Comments.FindAsync(id);
        }

        public async Task AddAsync(Comment value)
        {
            _context.Entry(value).State = EntityState.Added;
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Comment value)
        {
            _context.Entry(value).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(int id)
        {
            Comment comment = await FindByIdAsync(id);
            _context.Entry(comment).State = EntityState.Deleted;
            await _context.SaveChangesAsync();
        }
    }
}