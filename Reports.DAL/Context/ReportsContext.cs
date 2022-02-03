using Microsoft.EntityFrameworkCore;
using Reports.DAL.Entities;

namespace Reports.DAL.Context
{
    public sealed class ReportsContext : DbContext
    {
        public ReportsContext()
        {
            // Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Report> Reports { get; set; }

        // public static ReportsContext Context { get; } = new ReportsContext();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=reports.db;");
        }
    }
}