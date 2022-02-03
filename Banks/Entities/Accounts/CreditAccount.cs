using System;
using Banks.Entities.Banks;
using Banks.Entities.Clients;
using Banks.Entities.Transactions;
using Banks.Exceptions;

namespace Banks.Entities.Accounts
{
    public class CreditAccount : BankAccount
    {
        public CreditAccount()
        {
        }

        public CreditAccount(Client owner, decimal fee, decimal limit)
            : base(owner)
        {
            InterestType = InterestType.None;
            Fee = fee;
            Limit = Math.Abs(limit);
            Type = AccountType.Credit;
        }

        public decimal Limit { get; init; }

        public override Transaction Withdraw(decimal moneyAmount)
        {
            if (moneyAmount <= 0.0m)
                throw new InvalidMoneyException($"{nameof(moneyAmount)} is negative or zero");

            if (Balance + Limit < moneyAmount)
            {
                throw new InsufficientFundsException(
                    $"required {moneyAmount}, balance: {Balance}, limit: {Limit}");
            }

            Balance -= moneyAmount;
            History.AddEntry(DateTime.Now, Balance);
            return Transaction.CreateWithdrawal(this, moneyAmount);
        }

        public override Transaction TopUp(decimal moneyAmount)
        {
            if (moneyAmount <= 0.0m)
                throw new InvalidMoneyException($"{nameof(moneyAmount)} is negative or zero");

            Balance += moneyAmount;
            History.AddEntry(DateTime.Now, Balance);
            return Transaction.CreateToppingUp(this, moneyAmount);
        }

        public override Transaction Transfer(decimal moneyAmount, BankAccount receiver)
        {
            Withdraw(moneyAmount);
            receiver.TopUp(moneyAmount);

            History.AddEntry(DateTime.Now, Balance);
            return Transaction.CreateTransfer(this, receiver, moneyAmount);
        }

        public override Transaction WithdrawFee(decimal moneyAmount)
        {
            if (moneyAmount <= 0.0m)
                throw new InvalidMoneyException($"{nameof(moneyAmount)} is negative or zero");

            Balance -= moneyAmount;
            return Transaction.CreateWithdrawal(this, moneyAmount);
        }

        public override bool FeeRequired() => Balance < 0.0m;

        public override bool InterestRequired() => false;
    }
}