using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Banks.Entities.Banks
{
    [Table("BankConfigs")]
    public class BankConfig : IEquatable<BankConfig>
    {
        public BankConfig()
        {
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DatabaseId { get; init; }

        public Guid Id { get; init; }
        public decimal Fee { get; init; }
        public decimal DebitInterestRate { get; init; }
        public decimal CreditLimit { get; init; }
        public SortedSet<DepositRateEntry> DepositInterestRates { get; init; }

        public bool Equals(BankConfig other)
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
            return Equals((BankConfig)obj);
        }

        public override int GetHashCode() => Id.GetHashCode();
    }
}