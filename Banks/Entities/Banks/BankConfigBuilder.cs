using System;
using System.Collections.Generic;

namespace Banks.Entities.Banks
{
    public class BankConfigBuilder
    {
        private decimal _fee;
        private decimal _debitInterestRate;
        private decimal _creditLimit;
        private SortedSet<DepositRateEntry> _depositInterestRates;

        public BankConfigBuilder()
        {
            _depositInterestRates = new SortedSet<DepositRateEntry>();
        }

        public BankConfigBuilder FeeRate(decimal value)
        {
            if (value < 0.0m)
                throw new ArgumentException($"'{nameof(value)}' is negative");

            _fee = value;
            return this;
        }

        public BankConfigBuilder DebitInterestRate(decimal percentage)
        {
            if (percentage < 0.0m)
                throw new ArgumentException($"'{nameof(percentage)}' is negative");

            _debitInterestRate = percentage;
            return this;
        }

        public BankConfigBuilder CreditLimit(decimal limit)
        {
            if (limit < 0.0m)
                throw new ArgumentException($"'{nameof(limit)}' is negative");

            _creditLimit = limit;
            return this;
        }

        public BankConfigBuilder AddDepositRateEntry(DepositRateEntry entry)
        {
            if (entry == null)
                throw new ArgumentNullException(nameof(entry));

            _depositInterestRates.Add(entry);
            return this;
        }

        public BankConfig Build()
        {
            return new BankConfig
            {
                Id = Guid.NewGuid(),
                Fee = _fee,
                CreditLimit = _creditLimit,
                DebitInterestRate = _debitInterestRate,
                DepositInterestRates = _depositInterestRates,
            };
        }
    }
}