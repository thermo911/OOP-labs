using System;
using System.Collections.Generic;
using System.Linq;
using Banks.Entities.Transactions;
using Banks.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Banks.Repositories.Impl.EFCore
{
    public class EfTransactionRepository : IRepository<Transaction>
    {
        private BanksApplicationContext context = BanksApplicationContext.Context;

        public Transaction GetById(Guid id)
        {
            Transaction result = context.Transactions
                .Include(transaction => transaction.Receiver)
                .Include(transaction => transaction.Source)
                .FirstOrDefault(trans => trans.Id == id);

            if (result == null)
                throw new NotFoundException($"transaction with id {id} not found");
            return result;
        }

        public bool ExistsWithId(Guid id)
        {
            return context.Transactions.Any(trans => trans.Id == id);
        }

        public IEnumerable<Transaction> GetAll()
        {
            return context.Transactions
                .Include(transaction => transaction.Receiver)
                .Include(transaction => transaction.Source);
        }

        public void Save(Transaction value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            context.Transactions.Update(value);
            context.SaveChanges();
        }
    }
}