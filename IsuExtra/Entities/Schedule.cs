using System;
using System.Collections.Generic;
using System.Linq;
using Isu.Tools;

namespace IsuExtra.Entities
{
    public class Schedule
    {
        private readonly List<StudyClass> _classes;

        public Schedule(params StudyClass[] classes)
        {
            if (HaveCollision(classes))
            {
                throw new IsuException(
                    "Schedule constructor: time collision between study classes");
            }

            _classes = new List<StudyClass>(classes);
        }

        public IReadOnlyCollection<StudyClass> Classes => _classes;

        public bool HasCollision(Schedule other)
        {
            if (other == null)
                throw new ArgumentNullException(nameof(other));

            return _classes.Any(thisClass =>
                other._classes.Any(otherClass =>
                    otherClass.HasCollision(thisClass)));
        }

        private static bool HaveCollision(params StudyClass[] classes)
        {
            for (int i = 0; i < classes.Length - 1; ++i)
            {
                for (int j = i + 1; j < classes.Length; ++j)
                {
                    if (classes[i].HasCollision(classes[j]))
                        return true;
                }
            }

            return false;
        }
    }
}