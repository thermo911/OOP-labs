using System;
using Banks.Entities.Accounts;

namespace Banks.UI.Info
{
    public class AccountInfo
    {
        public AccountInfo(BankAccount account)
        {
            Id = account.Id;
            Fee = account.Fee;
            InterestRate = account.InterestRate;
            Balance = account.Balance;
            Type = account.Type;
        }

        public Guid Id { get; }
        public decimal Fee { get; }
        public decimal InterestRate { get; }
        public decimal Balance { get; }
        public AccountType Type { get; }
    }
}