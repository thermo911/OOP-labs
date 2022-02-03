using System;
using Shops.Tools.Exceptions;

namespace Shops.Entities
{
    public class Person : IIdable, IEquatable<Person>
    {
        private static uint _counter;

        public Person(string name, Money balance)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Balance = balance;
            Id = ++_counter;
        }

        public Person(Person other)
        {
            Id = other.Id;
            Name = other.Name;
            Balance = new Money(other.Balance.Value);
        }

        public uint Id { get; }
        public string Name { get; }
        public Money Balance { get; private set; }

        public void SpendMoney(Money money)
        {
            if (!CanAffordPurchase(money))
            {
                throw new ShopException(
                    $"Person.SpendMoney: can't spend {money.Value}, balance: {Balance.Value}");
            }

            Balance = new Money(Balance.Value - money.Value);
        }

        public void AddMoney(Money money) => Balance = new Money(Balance.Value + money.Value);

        public bool CanAffordPurchase(Money cost) => Balance.Value >= cost.Value;

        public override bool Equals(object obj) => obj is Person other && Id == other.Id;

        public override int GetHashCode() => Id.GetHashCode();

        public bool Equals(Person other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id == other.Id;
        }
    }
}