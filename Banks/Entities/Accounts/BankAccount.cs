using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Banks.Entities.Banks;
using Banks.Entities.Clients;
using Banks.Entities.Transactions;

namespace Banks.Entities.Accounts
{
    public abstract class BankAccount : IEquatable<BankAccount>, IIdentifiable
    {
        protected BankAccount(Client owner)
        {
            Id = Guid.NewGuid();
            Owner = owner ?? throw new ArgumentNullException(nameof(owner));
            History = new AccountHistory();
            Balance = 0.0m;
        }

        protected BankAccount()
        {
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DatabaseId { get; init; }

        public Guid Id { get; init; }
        public AccountType Type { get; init; }
        public Client Owner { get; init; }
        public AccountHistory History { get; init; }
        public InterestType InterestType { get; init; }
        public decimal InterestRate { get; init; }
        public decimal Fee { get; init; }
        public decimal Balance { get; protected set; }

        public abstract Transaction Withdraw(decimal moneyAmount);
        public abstract Transaction TopUp(decimal moneyAmount);
        public abstract Transaction Transfer(decimal moneyAmount, BankAccount receiver);
        public abstract Transaction WithdrawFee(decimal moneyAmount);

        public abstract bool FeeRequired();
        public abstract bool InterestRequired();

        public Transaction ForceWithdraw(decimal moneyAmount)
        {
            Balance -= moneyAmount;
            History.AddEntry(DateTime.Now, Balance);
            return Transaction.CreateWithdrawal(this, moneyAmount);
        }

        public override string ToString() => Id.ToString();

        public bool Equals(BankAccount other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id.Equals(other.Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((BankAccount)obj);
        }

        public override int GetHashCode() => Id.GetHashCode();
    }
}