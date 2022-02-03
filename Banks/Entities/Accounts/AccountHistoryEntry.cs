using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Banks.Entities.Accounts
{
    [Table("AccountHistoryEntries")]
    public class AccountHistoryEntry :
        IEquatable<AccountHistoryEntry>,
        IComparable<AccountHistoryEntry>
    {
        public AccountHistoryEntry(DateTime dateTime, decimal balance)
        {
            Id = Guid.NewGuid();
            DateTime = dateTime;
            Balance = balance;
        }

        public AccountHistoryEntry()
        {
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DatabaseId { get; init; }

        public Guid Id { get; init; }
        public DateTime DateTime { get; init; }
        public decimal Balance { get; init; }

        public bool Equals(AccountHistoryEntry other)
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
            return Equals((AccountHistoryEntry)obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public int CompareTo(AccountHistoryEntry other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            return DateTime.CompareTo(other.DateTime);
        }
    }
}