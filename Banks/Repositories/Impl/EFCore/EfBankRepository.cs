using System;
using System.Collections.Generic;
using System.Linq;
using Banks.Entities.Banks;
using Banks.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Banks.Repositories.Impl.EFCore
{
    public class EfBankRepository : IRepository<Bank>
    {
        private BanksApplicationContext context = BanksApplicationContext.Context;

        public Bank GetById(Guid id)
        {
            Bank result = context.Banks
                .Include(bank => bank.Config)
                    .ThenInclude(config => config.DepositInterestRates)
                .Include(bank => bank.Subscribers)
                .Include(bank => bank.Accounts)
                    .ThenInclude(acc => acc.History)
                    .ThenInclude(history => history.Entries)
                .FirstOrDefault(bank => bank.Id == id);

            if (result == null)
                throw new NotFoundException($"bank with id {id} not found");

            return result;
        }

        public bool ExistsWithId(Guid id) => context.Banks.Any(bank => bank.Id == id);

        public IEnumerable<Bank> GetAll()
        {
            return context.Banks
                .Include(bank => bank.Config)
                    .ThenInclude(config => config.DepositInterestRates)
                .Include(bank => bank.Subscribers)
                .Include(bank => bank.Accounts)
                    .ThenInclude(acc => acc.History)
                    .ThenInclude(history => history.Entries)
                .ToList();
        }

        public void Save(Bank value)
        {
            if (value == null)
                throw new ArgumentNullException(nameof(value));

            context.Banks.Update(value);
            context.SaveChanges();
        }
    }
}