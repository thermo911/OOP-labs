using System;
using System.Collections.Generic;
using Banks.Entities.Accounts;
using Banks.Entities.Banks;
using Banks.Repositories;

namespace Banks.Services.Impl
{
    public class BankService : IService<Bank>
    {
        private IRepository<Bank> _bankRepository;

        public BankService(IRepository<Bank> bankRepository)
        {
            _bankRepository = bankRepository;
        }

        public Bank GetById(Guid id) => _bankRepository.GetById(id);

        public void Save(Bank value) => _bankRepository.Save(value);

        public IEnumerable<Bank> GetAll() => _bankRepository.GetAll();

        public bool ExistsWithId(Guid id) => _bankRepository.ExistsWithId(id);
    }
}