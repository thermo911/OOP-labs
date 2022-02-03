using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Banks.Entities.Banks
{
    [Table("DepositRateEntries")]
    public class DepositRateEntry : IEquatable<DepositRateEntry>, IComparable<DepositRateEntry>
    {
        public DepositRateEntry()
        {
        }

        public DepositRateEntry(decimal level, decimal percentage)
        {
            if (level < 0.0m)
                throw new ArgumentException($"'{nameof(level)}' is negative");

            if (percentage < 0.0m)
                throw new ArgumentException($"'{nameof(percentage)}' is negative");

            Level = level;
            Percentage = percentage;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DatabaseId { get; init; }

        public decimal Level { get; init; }
        public decimal Percentage { get; init; }

        public bool Equals(DepositRateEntry other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Level.Equals(other.Level) && Percentage.Equals(other.Percentage);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((DepositRateEntry)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Level, Percentage);
        }

        public int CompareTo(DepositRateEntry other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            return Level.CompareTo(other.Level);
        }
    }
}