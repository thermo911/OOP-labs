using System;
using System.Collections.Generic;
using Isu.Entities;

namespace IsuExtra.Entities
{
    public class GsaCourse : IEquatable<GsaCourse>
    {
        private static int _counter = 0;

        public GsaCourse(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Id = ++_counter;
        }

        public string Name { get; }
        public int Id { get; }

        public bool Equals(GsaCourse other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((GsaCourse)obj);
        }

        public override int GetHashCode() => Id;
    }
}