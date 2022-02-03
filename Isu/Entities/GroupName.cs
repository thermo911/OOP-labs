using System;
using Isu.Tools;
using Isu.Types;

namespace Isu.Entities
{
    public class GroupName
    {
        private readonly string _name;
        private readonly CourseNumber _courseNumber;
        private readonly int _groupNumber;

        public GroupName(string name)
        {
            if (!IsValidGroupName(name))
                throw new IsuException($"invalid group name: {name}");

            _name = name;
            _courseNumber = ParseCourseNumber(name);
            _groupNumber = ParseGroupNumber(name);
        }

        public string Name => _name;
        public CourseNumber CourseNumber => _courseNumber;
        public int GroupNumber => _groupNumber;

        public override bool Equals(object obj)
            => obj is GroupName item && _name.Equals(item.Name);

        public override int GetHashCode() => _name.GetHashCode();

        private static bool IsValidGroupName(string name)
        {
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            if (name.Length != 5)
                return false;

            if (name[0] < 'A' || name[0] > 'Z')
                return false;

            if (name[2] < '1' || name[2] > '6')
                return false;

            for (int i = 3; i < 5; i++)
            {
                if (name[i] < '0' || name[i] > '9')
                    return false;
            }

            return true;
        }

        private static CourseNumber ParseCourseNumber(string name)
        {
            char c = name[2];
            int t = Convert.ToInt32(c);
            return (CourseNumber)t;
        }

        private static int ParseGroupNumber(string name) => Convert.ToInt32(name[3..]);
    }
}