using System;
using Isu.Tools;

namespace IsuExtra.Entities
{
    public class StudyClass
    {
        public StudyClass(
            DateTime start,
            DateTime end,
            string name)
        {
            Start = start;
            End = end;

            if (Start.Date != End.Date)
            {
                throw new IsuException(
                    "StudyClass cannot last more than 24 hours (not for OOP and OS)");
            }

            if (End < Start)
                throw new IsuException("End of StudyClass is earlier than its start");

            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public DateTime Start { get; }
        public DateTime End { get; }
        public string Name { get; }

        public string Room { get; set; }
        public string TeacherName { get; set; }

        public bool HasCollision(StudyClass other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            if (other.End < Start)
                return false;

            if (End < other.Start)
                return false;

            return true;
        }
    }
}