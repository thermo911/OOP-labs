using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Reports.DAL.Context;
using Reports.DAL.Entities;

namespace Reports.DAL.Repositories.Impl
{
    public class ReportRepository : IRepository<Report>
    {
        private ReportsContext _context;

        public ReportRepository(ReportsContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Report>> GetAllAsync()
        {
            return await _context.Reports.ToArrayAsync();
        }
        
        public async Task<IEnumerable<Report>> GetWhereAsync(Func<Report, bool> predicate)
        {
            return await Task.Run(() => _context.Reports.Where(predicate));
        }

        public async Task<Report> FindByIdAsync(int id)
        {
            return await _context.Reports.FindAsync(id);
        }

        public async Task AddAsync(Report value)
        {
            _context.Entry(value).State = EntityState.Added;
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Report value)
        {
            _context.Entry(value).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(int id)
        {
            Report report = await FindByIdAsync(id);
            _context.Entry(report).State = EntityState.Deleted;
            await _context.SaveChangesAsync();
        }
    }
}