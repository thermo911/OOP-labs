using System;
using Isu.Tools;

namespace IsuExtra.Entities
{
    public class MegaFaculty : IEquatable<MegaFaculty>
    {
        private static int _counter = 0;
        public MegaFaculty(string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Id = ++_counter;
        }

        public string Name { get; }
        public int Id { get; }

        public bool Equals(MegaFaculty other)
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
            return Equals((MegaFaculty)obj);
        }

        public override int GetHashCode() => Id;
    }
}