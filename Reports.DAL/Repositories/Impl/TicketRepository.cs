using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Reports.DAL.Context;
using Reports.DAL.Entities;

namespace Reports.DAL.Repositories.Impl
{
    public class TicketRepository : IRepository<Ticket>
    {
        private ReportsContext _context;

        public TicketRepository(ReportsContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Ticket>> GetAllAsync()
        {
            return await _context.Tickets.ToArrayAsync();
        }
        
        public async Task<IEnumerable<Ticket>> GetWhereAsync(Func<Ticket, bool> predicate)
        {
            return await Task.Run(() => _context.Tickets.Where(predicate));
        }

        public async Task<Ticket> FindByIdAsync(int id)
        {
            return await _context.Tickets.FindAsync(id);
        }

        public async Task AddAsync(Ticket value)
        {
            _context.Entry(value).State = EntityState.Added;
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Ticket value)
        {
            _context.Entry(value).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(int id)
        {
            Ticket ticket = await FindByIdAsync(id);
            _context.Entry(ticket).State = EntityState.Deleted;
            await _context.SaveChangesAsync();
        }
    }
}