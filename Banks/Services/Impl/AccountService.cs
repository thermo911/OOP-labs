using System;
using System.Collections.Generic;
using Banks.Entities.Accounts;
using Banks.Repositories;

namespace Banks.Services.Impl
{
    public class AccountService : IService<BankAccount>
    {
        private IRepository<BankAccount> _accountRepository;

        public AccountService(IRepository<BankAccount> accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public BankAccount GetById(Guid id) => _accountRepository.GetById(id);

        public void Save(BankAccount value) => _accountRepository.Save(value);

        public IEnumerable<BankAccount> GetAll() => _accountRepository.GetAll();

        public bool ExistsWithId(Guid id) => _accountRepository.ExistsWithId(id);
    }
}