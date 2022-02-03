using Banks.Entities.Accounts;
using Banks.Entities.Banks;
using Banks.Entities.Clients;
using Banks.Entities.Transactions;
using Banks.Managers;
using Banks.Repositories;
using Banks.Repositories.Impl.EFCore;
using Banks.Services;
using Banks.Services.Impl;

namespace Banks.AppConfig
{
    public class EfManagerConfig
    {
        private static readonly IRepository<Bank> BankRepository = new EfBankRepository();
        private static readonly IRepository<BankAccount> AccountRepository = new EfAccountRepository();
        private static readonly IRepository<Client> ClientRepository = new EfClientRepository();
        private static readonly IRepository<Transaction> TransactionRepository = new EfTransactionRepository();

        private static readonly IService<Bank> BankService = new BankService(BankRepository);
        private static readonly IService<BankAccount> AccountService = new AccountService(AccountRepository);
        private static readonly IService<Client> ClientService = new ClientService(ClientRepository);
        private static readonly IService<Transaction> TransactionService = new TransactionService(TransactionRepository);

        private static BankManager _manager;

        public static BankManager Manager
            => _manager ??= new BankManager(AccountService, BankService, ClientService, TransactionService);
    }
}