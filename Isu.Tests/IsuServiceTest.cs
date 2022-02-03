using System;
using System.Collections;
using Isu.Entities;
using Isu.Services;
using Isu.Services.Impl;
using Isu.Tools;
using NUnit.Framework;

namespace Isu.Tests
{
    public class Tests
    {
        private IIsuService _isuService;

        [SetUp]
        public void Setup()
        {
            _isuService = new SimpleIsuService(20);
        }

        [Test]
        public void AddStudentToGroup_StudentHasGroupAndGroupContainsStudent()
        {
            const string groupName = "M3200";
            const string studentName = "Alex";
            
            Group group = _isuService.AddGroup(groupName);
            Student student = _isuService.AddStudent(group, studentName);
            
            CollectionAssert.Contains(group.Students, student);
            Assert.AreEqual(group, student.Group);
        }

        [Test]
        public void ReachMaxStudentPerGroup_ThrowException()
        {
            Assert.Catch<IsuException>(() =>
            {
                Group group = _isuService.AddGroup("M3200");
                
                for (int i = 0; i < 20; i++)
                    _isuService.AddStudent(group, "John Doe");

                _isuService.AddStudent(group, "imposter");
            });
        }

        [Test]
        public void CreateGroupWithInvalidName_ThrowException()
        {
            Assert.Catch<IsuException>(() =>
            {
                _isuService.AddGroup("X91211233");
            });
        }

        [Test]
        public void TransferStudentToAnotherGroup_GroupChanged()
        {
            const string groupName1 = "M3204";
            const string groupName2 = "M3200";

            Group group2 = _isuService.AddGroup(groupName2);
            Group group1 = _isuService.AddGroup(groupName1);

            Student student = _isuService.AddStudent(group1, "Alex");
            Assert.AreEqual(group1, student.Group);
            
            _isuService.ChangeStudentGroup(student, group2);
            
            Assert.AreEqual(group2, student.Group);
            CollectionAssert.Contains(group2.Students, student);
            CollectionAssert.DoesNotContain(group1.Students, student);
        }
    }
}