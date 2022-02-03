using System;
using System.Collections.Generic;
using System.Linq;
using Banks.Entities.Accounts;
using Banks.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace Banks.Repositories.Impl.EFCore
{
    public class EfAccountRepository : IRepository<BankAccount>
    {
        private BanksApplicationContext context = BanksApplicationContext.Context;

        public BankAccount GetById(Guid id)
        {
            BankAccount result = context.DebitAccounts
                .Include(acc => acc.History)
                .Include(acc => acc.Owner)
                .FirstOrDefault(acc => acc.Id == id);

            result ??= context.DepositAccounts
                .Include(acc => acc.History)
                .Include(acc => acc.Owner)
                .FirstOrDefault(acc => acc.Id == id);

            result ??= context.CreditAccounts
                .Include(acc => acc.History)
                .Include(acc => acc.Owner)
                .FirstOrDefault(acc => acc.Id == id);

            result = result ?? throw new NotFoundException(
                $"account with id {id} not found");

            return result;
        }

        public bool ExistsWithId(Guid id)
        {
            return context.DebitAccounts.Any(acc => acc.Id == id)
                   || context.DepositAccounts.Any(acc => acc.Id == id)
                   || context.CreditAccounts.Any(acc => acc.Id == id);
        }

        public IEnumerable<BankAccount> GetAll()
        {
            var result = new List<BankAccount>();
            result.AddRange(context.DebitAccounts
                .Include(acc => acc.History)
                .Include(acc => acc.Owner));

            result.AddRange(context.DepositAccounts
                .Include(acc => acc.History)
                .Include(acc => acc.Owner));

            result.AddRange(context.CreditAccounts
                .Include(acc => acc.History)
                .Include(acc => acc.Owner));
            return result;
        }

        public void Save(BankAccount value)
        {
            if (value == null)
                throw new ArgumentException(nameof(value));

            context.SaveChanges();

            switch (value.Type)
            {
                case AccountType.Debit:
                    context.DebitAccounts.Update((DebitAccount)value);
                    break;
                case AccountType.Deposit:
                    context.DepositAccounts.Update((DepositAccount)value);
                    break;
                case AccountType.Credit:
                    context.CreditAccounts.Update((CreditAccount)value);
                    break;
                default:
                    throw new ArgumentException(
                        $"Account type {value.Type} is unknown.");
            }

            context.SaveChanges();
        }
    }
}
