using System;
using System.Collections.Generic;
using Isu.Entities;
using IsuExtra.Tools.Exceptions;

namespace IsuExtra.Entities
{
    public class GsaThread : IEquatable<GsaThread>
    {
        private static int _counter = 0;
        private readonly List<Student> _students;

        public GsaThread(
            GsaCourse gsaCourse,
            string name,
            uint maxStudentsCount)
        {
            GsaCourse = gsaCourse ?? throw new ArgumentNullException(nameof(gsaCourse));
            Name = name ?? throw new ArgumentNullException(nameof(name));
            MaxStudentsCount = maxStudentsCount;
            _students = new List<Student>();
            Id = ++_counter;
        }

        public int Id { get; }
        public GsaCourse GsaCourse { get; }
        public string Name { get; }
        public uint MaxStudentsCount { get; }
        public IReadOnlyCollection<Student> Students => _students;

        public void AddStudent(Student student)
        {
            if (student == null)
                throw new ArgumentNullException(nameof(student));

            if (_students.Count >= MaxStudentsCount)
            {
                throw new GsaException(
                    "attempt to assign student to GsaThread with reached max students count");
            }

            _students.Add(student);
        }

        public void RemoveStudent(Student student) => _students.Remove(student);

        public bool Equals(GsaThread other)
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
            return Equals((GsaThread)obj);
        }

        public override int GetHashCode() => Id;
    }
}