using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Banks.Entities.Accounts
{
    [Table("AccountHistories")]
    public class AccountHistory
    {
        public AccountHistory()
        {
            Id = Guid.NewGuid();
            Entries = new SortedSet<AccountHistoryEntry>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DatabaseId { get; init; }

        public Guid Id { get; init; }
        public SortedSet<AccountHistoryEntry> Entries { get; init; }

        public void AddEntry(DateTime dateTime, decimal balance)
            => Entries.Add(new AccountHistoryEntry(dateTime, balance));

        public decimal GetBalanceAtDay(DateTime day)
        {
            AccountHistoryEntry entry = Entries
                .LastOrDefault(entry => entry.DateTime.Date < day.Date.AddDays(1));

            return entry?.Balance ?? 0.0m;
        }
    }
}