using Banks.Entities.Accounts;
using Banks.Entities.Banks;
using Banks.Entities.Clients;
using Microsoft.EntityFrameworkCore;
using Transaction = Banks.Entities.Transactions.Transaction;

namespace Banks.Repositories.Impl.EFCore
{
    public sealed class BanksApplicationContext : DbContext
    {
        private static BanksApplicationContext _context = new ();

        private BanksApplicationContext()
        {
            Database.EnsureCreated();
        }

        public static BanksApplicationContext Context => _context;

        public DbSet<Bank> Banks { get; set; }
        public DbSet<DebitAccount> DebitAccounts { get; set; }
        public DbSet<DepositAccount> DepositAccounts { get; set; }
        public DbSet<CreditAccount> CreditAccounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Client> Clients { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase("SHIT");
            optionsBuilder.EnableSensitiveDataLogging();

            // optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=banksdb;Trusted_Connection=True;");
        }
    }
}