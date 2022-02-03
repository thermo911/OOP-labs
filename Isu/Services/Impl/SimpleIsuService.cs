using System;
using System.Collections.Generic;
using System.Linq;
using Isu.Entities;
using Isu.Tools;
using Isu.Types;

namespace Isu.Services.Impl
{
    public class SimpleIsuService : IIsuService
    {
        private readonly List<Group> _groups;
        private readonly List<Student> _students;
        private readonly int _maxStudentsPerGroup;

        public SimpleIsuService(int maxStudentsPerGroup)
        {
            _groups = new List<Group>();
            _students = new List<Student>();
            _maxStudentsPerGroup = maxStudentsPerGroup;
        }

        public Group AddGroup(string name)
        {
            var groupName = new GroupName(name);
            Group group = _groups.FirstOrDefault(group => group.Name.Equals(groupName));
            if (group != null)
                return group;
            group = new Group(name);
            _groups.Add(group);
            return group;
        }

        public Student AddStudent(Group group, string name)
        {
            if (group.Students.Count == _maxStudentsPerGroup)
                throw new IsuException($"couldn't add student {name} to group {group}");
            var student = new Student(name, group);
            group.Students.Add(student);
            _groups.Add(group);
            _students.Add(student);
            return student;
        }

        public Student GetStudent(int id)
        {
            Student student = _students.FirstOrDefault(student => student.Id == id);
            if (student == null)
                throw new IsuException($"Student not found, id: {id}");
            return student;
        }

        public Student FindStudent(string name)
        {
            return _students.FirstOrDefault(student => student.Name.Equals(name));
        }

        public List<Student> FindStudents(string groupName)
        {
            var findingName = new GroupName(groupName);
            Group group = _groups.FirstOrDefault(group => group.Name.Equals(findingName));
            return group == null ? new List<Student>() : new List<Student>(group.Students);
        }

        public List<Student> FindStudents(CourseNumber courseNumber)
        {
            return _students.Where(student => student.Group.Name.CourseNumber == courseNumber).ToList();
        }

        public Group FindGroup(string groupName)
        {
            var groupNameToFind = new GroupName(groupName);
            return _groups.FirstOrDefault(group => group.Name.Equals(groupNameToFind));
        }

        public List<Group> FindGroups(CourseNumber courseNumber)
        {
            return _groups.Where(group => group.Name.CourseNumber == courseNumber).ToList();
        }

        public void ChangeStudentGroup(Student student, Group newGroup)
        {
            if (student == null)
                throw new IsuException("student is null");
            if (newGroup == null)
                throw new IsuException("newGroup is null");

            student.Group.Students.Remove(student);
            newGroup.Students.Add(student);
            student.Group = newGroup;
        }
    }
}