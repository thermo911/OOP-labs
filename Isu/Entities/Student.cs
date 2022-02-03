using System;
using Isu.Tools;

namespace Isu.Entities
{
    public class Student : IEquatable<Student>
    {
        private static int _counter = 0;
        private Group _group;

        public Student(string name, Group group)
        {
            _group = group ?? throw new ArgumentNullException(nameof(group));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Id = ++_counter;
        }

        public int Id { get; }
        public string Name { get; }

        public Group Group
        {
            get => _group;
            set => _group = value ?? throw new IsuException("group is null");
        }

        public override bool Equals(object obj)
        {
            if (obj is not Student item)
                return false;
            return Id == item.Id;
        }

        public override int GetHashCode() => Id;

        public bool Equals(Student other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id == other.Id;
        }
    }
}