using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Banks.Entities.Accounts;
using Banks.Exceptions;

namespace Banks.Entities.Transactions
{
    [Table("Transactions")]
    public class Transaction : IEquatable<Transaction>, IIdentifiable
    {
        public Transaction()
        {
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DatabaseId { get; init; }

        public Guid Id { get; init; }
        public BankAccount Source { get; init; }
        public BankAccount Receiver { get; init; }
        public decimal MoneyAmount { get; init; }
        public TransactionType Type { get; init; }
        public TransactionStatus Status { get; set; }
        public DateTime DateTime { get; init; }

        public static Transaction CreateWithdrawal(BankAccount source, decimal moneyAmount)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (moneyAmount <= 0.0m)
                throw new ArgumentException($"{nameof(moneyAmount)} is negative or zero");

            return new Transaction
            {
                Id = Guid.NewGuid(),
                MoneyAmount = moneyAmount,
                Source = source,
                Type = TransactionType.Withdrawal,
                Status = TransactionStatus.Performed,
                DateTime = DateTime.Now,
            };
        }

        public static Transaction CreateToppingUp(BankAccount receiver, decimal moneyAmount)
        {
            if (receiver == null)
                throw new ArgumentNullException(nameof(receiver));

            if (moneyAmount <= 0.0m)
                throw new ArgumentException($"{nameof(moneyAmount)} is negative or zero");

            return new Transaction
            {
                Id = Guid.NewGuid(),
                MoneyAmount = moneyAmount,
                Receiver = receiver,
                Type = TransactionType.ToppingUp,
                Status = TransactionStatus.Performed,
                DateTime = DateTime.Now,
            };
        }

        public static Transaction CreateTransfer(BankAccount source, BankAccount receiver, decimal moneyAmount)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (receiver == null)
                throw new ArgumentNullException(nameof(receiver));

            if (moneyAmount <= 0.0m)
                throw new ArgumentException($"{nameof(moneyAmount)} is negative or zero");

            return new Transaction
            {
                Id = Guid.NewGuid(),
                MoneyAmount = moneyAmount,
                Source = source,
                Receiver = receiver,
                Type = TransactionType.Transfer,
                Status = TransactionStatus.Performed,
                DateTime = DateTime.Now,
            };
        }

        public static void CancelTransfer(
            Transaction transfer,
            out Transaction topUp,
            out Transaction withdrawal)
        {
            if (transfer == null)
                throw new ArgumentNullException(nameof(transfer));

            if (transfer.Type != TransactionType.Transfer)
                throw new BanksException($"Unable to cancel transaction of type {transfer.Type}");

            if (transfer.Status == TransactionStatus.Canceled)
                throw new BanksException($"Transfer {transfer.Id} already cancelled");

            BankAccount source = transfer.Source;
            BankAccount receiver = transfer.Receiver;
            decimal moneyAmount = transfer.MoneyAmount;

            topUp = source.TopUp(moneyAmount);
            withdrawal = receiver.ForceWithdraw(moneyAmount);

            transfer.Status = TransactionStatus.Canceled;
        }

        public bool Equals(Transaction other)
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
            return Equals((Transaction)obj);
        }

        public override int GetHashCode() => Id.GetHashCode();
    }
}