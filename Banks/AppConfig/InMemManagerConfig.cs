using Banks.Entities.Accounts;
using Banks.Entities.Banks;
using Banks.Entities.Clients;
using Banks.Entities.Transactions;
using Banks.Managers;
using Banks.Repositories;
using Banks.Repositories.Impl.EFCore;
using Banks.Repositories.Impl.InMemory;
using Banks.Services;
using Banks.Services.Impl;

namespace Banks.AppConfig
{
    public class InMemManagerConfig
    {
        private static readonly IRepository<Bank> BankRepository = new InMemBankRepository();
        private static readonly IRepository<BankAccount> AccountRepository = new InMemAccountRepository();
        private static readonly IRepository<Client> ClientRepository = new InMemClientRepository();
        private static readonly IRepository<Transaction> TransactionRepository = new InMemRepository<Transaction>();

        private static readonly IService<Bank> BankService = new BankService(BankRepository);
        private static readonly IService<BankAccount> AccountService = new AccountService(AccountRepository);
        private static readonly IService<Client> ClientService = new ClientService(ClientRepository);
        private static readonly IService<Transaction> TransactionService = new TransactionService(TransactionRepository);

        private static BankManager _manager;

        public static BankManager Manager
            => _manager ??= new BankManager(AccountService, BankService, ClientService, TransactionService);

        public static BankManager NewManager()
        {
            IRepository<Bank> bankRepository1 = new InMemBankRepository();
            IRepository<BankAccount> accountRepository1 = new InMemAccountRepository();
            IRepository<Client> clientRepository1 = new InMemClientRepository();
            IRepository<Transaction> transactionRepository1 = new InMemRepository<Transaction>();

            IService<Bank> bankService1 = new BankService(bankRepository1);
            IService<BankAccount> accountService1 = new AccountService(accountRepository1);
            IService<Client> clientService1 = new ClientService(clientRepository1);
            IService<Transaction> transactionService1 = new TransactionService(transactionRepository1);

            return new BankManager(accountService1, bankService1, clientService1, transactionService1);
        }
    }
}