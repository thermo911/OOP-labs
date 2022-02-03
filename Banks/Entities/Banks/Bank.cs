using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Banks.Entities.Accounts;
using Banks.Entities.Clients;
using Banks.Entities.Transactions;
using Banks.Exceptions;

namespace Banks.Entities.Banks
{
    [Table("Banks")]
    public class Bank : IEquatable<Bank>, IIdentifiable
    {
        public Bank()
        {
        }

        public Bank(BankConfig config)
        {
            Config = config ?? throw new ArgumentNullException(nameof(config));
            Id = Guid.NewGuid();
            Accounts = new HashSet<BankAccount>();
            Subscribers = new HashSet<Client>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DatabaseId { get; init; }

        public Guid Id { get; init; }
        public HashSet<BankAccount> Accounts { get; init; }
        public HashSet<Client> Subscribers { get; init; }
        public BankConfig Config { get; private set; }

        public void AddSubscriber(Client client)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));

            if (!Subscribers.Add(client))
                throw new BanksException($"client with id {client.Id} already subscribed");
        }

        public void RemoveSubscriber(Client client)
        {
            if (!Subscribers.Remove(client))
                throw new BanksException($"client with id {client.Id} already unsubscribed");
        }

        public void UpdateConfig(BankConfig newConfig)
        {
            Config = newConfig ?? throw new ArgumentNullException(nameof(newConfig));

            foreach (Client subscriber in Subscribers)
            {
                subscriber.OnNotification();
            }
        }

        public DebitAccount CreateAndRegisterDebitAccount(Client client)
        {
            decimal interestRate = Config.DebitInterestRate;
            var account = new DebitAccount(client, interestRate);
            Accounts.Add(account);
            return account;
        }

        public DepositAccount CreateAndRegisterDepositAccount(
            Client client,
            decimal startBalance,
            DateTime tillDateTime)
        {
            decimal interestRate = CalculateDepositRate(startBalance);
            var account = new DepositAccount(client, tillDateTime, interestRate);
            Accounts.Add(account);
            return account;
        }

        public CreditAccount CreateAndRegisterCreditAccount(Client client)
        {
            var account = new CreditAccount(client, Config.Fee, Config.CreditLimit);
            Accounts.Add(account);
            return account;
        }

        public IEnumerable<Transaction> WithdrawFees()
        {
            return Accounts
                .Where(acc => acc.FeeRequired())
                .Select(acc => acc.WithdrawFee(Config.Fee))
                .ToList();
        }

        public IEnumerable<Transaction> PayInterests(DateTime from, DateTime to)
        {
            from = from.Date;
            to = to.Date;
            return Accounts
                .Where(acc => acc.InterestRequired())
                .Select(acc =>
                    acc.TopUp(CalculateInterestPayment(acc, from, to)));
        }

        public bool Equals(Bank other)
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
            return Equals((Bank)obj);
        }

        public override int GetHashCode() => Id.GetHashCode();

        public override string ToString() => Id.ToString();

        private static decimal CalculateInterestPayment(
            BankAccount bankAccount,
            DateTime from,
            DateTime to)
        {
            if (!bankAccount.InterestRequired())
                return 0.0m;

            from = from.Date;
            to = to.Date;

            decimal total = 0.0m;
            decimal rate = bankAccount.InterestRate;

            for (DateTime day = from; day <= to; day = day.AddDays(1))
            {
                decimal balance = bankAccount.History.GetBalanceAtDay(day);
                total += balance * rate / 100.0m;
            }

            return total;
        }

        private decimal CalculateDepositRate(decimal startBalance)
        {
            DepositRateEntry entry = Config.DepositInterestRates
                .LastOrDefault(entry => startBalance > entry.Level);
            return entry?.Percentage ?? Config.DebitInterestRate;
        }
    }
}