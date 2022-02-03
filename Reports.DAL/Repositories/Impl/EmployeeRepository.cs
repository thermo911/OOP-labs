using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Reports.DAL.Context;
using Reports.DAL.Entities;

namespace Reports.DAL.Repositories.Impl
{
    public class EmployeeRepository : IRepository<Employee>
    {
        private ReportsContext _context;

        public EmployeeRepository(ReportsContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await _context.Employees.ToArrayAsync();
        }
        
        public async Task<IEnumerable<Employee>> GetWhereAsync(Func<Employee, bool> predicate)
        {
            return await Task.Run(() => _context.Employees.Where(predicate));
        }

        public async Task<Employee> FindByIdAsync(int id)
        {
            return await _context.Employees.FindAsync(id);
        }

        public async Task AddAsync(Employee value)
        {
            _context.Entry(value).State = EntityState.Added;
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Employee value)
        {
            _context.Entry(value).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteByIdAsync(int id)
        {
            Employee employee = await FindByIdAsync(id);
            _context.Entry(employee).State = EntityState.Deleted;
            await _context.SaveChangesAsync();
        }
    }
}