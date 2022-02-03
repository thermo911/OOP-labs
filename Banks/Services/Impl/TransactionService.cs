using System;
using System.Collections.Generic;
using Banks.Entities.Transactions;
using Banks.Repositories;

namespace Banks.Services.Impl
{
    public class TransactionService : IService<Transaction>
    {
        private IRepository<Transaction> _transactionRepository;

        public TransactionService(IRepository<Transaction> transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public Transaction GetById(Guid id) => _transactionRepository.GetById(id);

        public void Save(Transaction value) => _transactionRepository.Save(value);

        public IEnumerable<Transaction> GetAll() => _transactionRepository.GetAll();

        public bool ExistsWithId(Guid id) => _transactionRepository.ExistsWithId(id);
    }
}