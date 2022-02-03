using System;
using System.Collections.Generic;

namespace Isu.Entities
{
    public class Group : IEquatable<Group>
    {
        private readonly List<Student> _students;

        public Group(string name)
        {
            Name = new GroupName(name);
            _students = new List<Student>();
        }

        public GroupName Name { get; }

        public ICollection<Student> Students => _students;

        public override bool Equals(object obj)
            => obj is Group item && Name.Equals(item.Name);

        public override string ToString() => Name.Name;

        public override int GetHashCode()
        {
            return Name != null ? Name.GetHashCode() : 0;
        }

        public bool Equals(Group other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Name.Equals(other.Name);
        }
    }
}