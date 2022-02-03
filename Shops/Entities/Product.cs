using System;
using Shops.Tools.Exceptions;

namespace Shops.Entities
{
    public class Product : IIdable, IEquatable<Product>
    {
        private static uint _counter;
        public Product(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Id = ++_counter;
        }

        public Product(Product other)
        {
            Name = other.Name;
            Id = other.Id;
        }

        public string Name { get; }
        public uint Id { get; }

        public override bool Equals(object obj) => obj is Product item && Id == item.Id;

        public override int GetHashCode() => Id.GetHashCode();

        public override string ToString() => $"{Name}({Id})";

        public bool Equals(Product other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id == other.Id;
        }
    }
}