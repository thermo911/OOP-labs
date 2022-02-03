using System;
using Banks.Entities.Banks;
using Banks.Entities.Clients;
using Banks.Entities.Transactions;
using Banks.Exceptions;

namespace Banks.Entities.Accounts
{
    public class DebitAccount : BankAccount
    {
        public DebitAccount()
        {
        }

        public DebitAccount(Client owner, decimal interestRate)
            : base(owner)
        {
            InterestType = InterestType.Fixed;
            InterestRate = interestRate;
            Type = AccountType.Debit;
        }

        public override Transaction Withdraw(decimal moneyAmount)
        {
            if (moneyAmount <= 0.0m)
                throw new InvalidMoneyException($"{nameof(moneyAmount)} is negative or zero");

            if (Balance < moneyAmount)
                throw new InsufficientFundsException($"required: {moneyAmount}, balance: {Balance}");

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
            throw new NotSupportedException($"Withdrawal of fee is not supported by {nameof(DebitAccount)}");
        }

        public override bool FeeRequired() => false;

        public override bool InterestRequired() => true;
    }
}
