using System;
using Banks.Entities.Banks;
using Banks.Entities.Clients;
using Banks.Entities.Transactions;
using Banks.Exceptions;

namespace Banks.Entities.Accounts
{
    public class DepositAccount : BankAccount
    {
        public DepositAccount()
        {
        }

        public DepositAccount(Client owner, DateTime tillDateTime, decimal interestRate)
            : base(owner)
        {
            TillDateTime = tillDateTime;
            InterestType = InterestType.DependsOnStartBalance;
            InterestRate = interestRate;
            Type = AccountType.Deposit;
        }

        public DateTime TillDateTime { get; init; }

        public override Transaction Withdraw(decimal moneyAmount)
        {
            if (moneyAmount <= 0.0m)
                throw new InvalidMoneyException($"'{nameof(moneyAmount)}' is negative or null");

            if (DateTime.Now < TillDateTime)
                throw new BanksException($"unable to withdraw before {TillDateTime.ToString()}");

            if (moneyAmount > Balance)
                throw new InsufficientFundsException($"required: {moneyAmount}, balance: {Balance}");

            Balance -= moneyAmount;
            History.AddEntry(DateTime.Now, Balance);
            return Transaction.CreateWithdrawal(this, moneyAmount);
        }

        public override Transaction TopUp(decimal moneyAmount)
        {
            if (moneyAmount <= 0.0m)
                throw new InvalidMoneyException($"'{nameof(moneyAmount)}' is negative or null");

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
            throw new NotSupportedException($"Withdrawal of fee is not supported by {nameof(DepositAccount)}");
        }

        public override bool FeeRequired() => false;

        public override bool InterestRequired() => true;
    }
}
