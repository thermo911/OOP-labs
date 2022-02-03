using System;
using System.Collections.Generic;
using System.Linq;
using Isu.Entities;
using Isu.Services;
using Isu.Services.Impl;
using IsuExtra.Entities;
using IsuExtra.Services;
using IsuExtra.Services.Impl;
using IsuExtra.Tools.Exceptions;
using NUnit.Framework;

namespace IsuExtra.Tests
{
    public class GsaServiceTests
    {
        private IGsaService _gsaService;
        private IIsuService _isuService;
        private IIsuExtraService _isuExtraService;

        private readonly MegaFaculty _tintMegaFaculty = new("TINT");
        private readonly MegaFaculty _ktuMegaFaculty = new("KTU");

        private Group _groupM3200;

        private readonly GsaCourse _gsaCourseMl = new("ML");

        private readonly Schedule _scheduleA = new(new StudyClass(
            default(DateTime), default(DateTime), "some"));

        private readonly Schedule _scheduleB = new(new StudyClass(
            default(DateTime).AddHours(1),
            default(DateTime).AddHours(3),
            "some"));

        private readonly Schedule _scheduleC = new(new StudyClass(
            default(DateTime).AddHours(2),
            default(DateTime).AddHours(4),
            "some"));

        [SetUp]
        public void Setup()
        {
            _isuService = new SimpleIsuService(25);
            _isuExtraService = new IsuExtraService();
            _gsaService = new GsaService(_isuService, _isuExtraService,1);
            
            _isuExtraService.AddMegaFaculty(_tintMegaFaculty);
            _isuExtraService.AddMegaFaculty(_ktuMegaFaculty);

            _groupM3200 = _isuService.AddGroup("M3200");
            _isuExtraService.SetGroupMegaFaculty(_groupM3200, _tintMegaFaculty.Id);
            
            _gsaService.AddGsaCourse(_gsaCourseMl, _ktuMegaFaculty);
        }
        
        [Test]
        public void AddGsaCourse_GsaCourseAdded()
        {
            var gsaCourse = new GsaCourse("Some");
            _gsaService.AddGsaCourse(gsaCourse, _ktuMegaFaculty);

            GsaCourse innerGsaCourse = _gsaService.GetGsaCourseById(gsaCourse.Id);
            Assert.AreEqual(gsaCourse, innerGsaCourse);
        }

        [Test]
        public void AddGsaThread_GsaThreadAdded()
        {
            var gsaCourse = new GsaCourse("ML");
            _gsaService.AddGsaCourse(gsaCourse, _tintMegaFaculty);
            
            var gsaThread = new GsaThread(gsaCourse, "Some name", 20);
            
            _gsaService.AddGsaThread(gsaThread);

            GsaThread innerGsaThread = _gsaService.GetGsaThreadById(gsaThread.Id);
            Assert.AreEqual(gsaThread, innerGsaThread);
        }

        [Test]
        public void AssignStudentToGsaThread_StudentAssignedAndThreadHasStudent()
        {
            _isuExtraService.SetGroupSchedule(_groupM3200, _scheduleA);
            Student student = _isuService.AddStudent(_groupM3200, "Alex");

            var gsaThread = new GsaThread(_gsaCourseMl, "Some", 20);
            _gsaService.AddGsaThread(gsaThread);
            _isuExtraService.SetGsaThreadSchedule(gsaThread, _scheduleB);

            _gsaService.AssignStudentToGsaThread(student.Id, gsaThread.Id);
            Assert.Contains(student, gsaThread.Students.ToList());
            CollectionAssert.Contains(_gsaService.GetThreadsOfStudent(student), gsaThread);
        }

        [Test]
        public void TryToAssignStudentToGsaThreadWithScheduleCollisions_GsaExceptionThrown()
        {
            _isuExtraService.SetGroupSchedule(_groupM3200, _scheduleB);
            Student student = _isuService.AddStudent(_groupM3200, "Alex");

            var gsaThread = new GsaThread(_gsaCourseMl, "Some", 20);
            _gsaService.AddGsaThread(gsaThread);
            _isuExtraService.SetGsaThreadSchedule(gsaThread, _scheduleC);

            Assert.Catch<GsaException>(() =>
            _gsaService.AssignStudentToGsaThread(student.Id, gsaThread.Id));
        }

        [Test]
        public void TryToAssignUserWithSameMegaFaculty_GsaExceptionThrown()
        {
            Group group = _isuService.AddGroup("MX228");
            _isuExtraService.SetGroupMegaFaculty(group, _ktuMegaFaculty.Id);
            _isuExtraService.SetGroupSchedule(group, _scheduleA);
            
            Student student = _isuService.AddStudent(group, "Boris");

            var gsaThread = new GsaThread(_gsaCourseMl, "Some", 20);
            _gsaService.AddGsaThread(gsaThread);
            _isuExtraService.SetGsaThreadSchedule(gsaThread, _scheduleB);

            Assert.Catch<GsaException>(()
                => _gsaService.AssignStudentToGsaThread(student.Id, gsaThread.Id));
        }

        [Test]
        public void TryToAssignStudentToFullGsaThread_GsaExceptionThrown()
        {
            _isuExtraService.SetGroupSchedule(_groupM3200, _scheduleA);
            Student student = _isuService.AddStudent(_groupM3200, "Alex");

            var gsaThread = new GsaThread(_gsaCourseMl, "Some", 0);
            _gsaService.AddGsaThread(gsaThread);
            _isuExtraService.SetGsaThreadSchedule(gsaThread, _scheduleB);
            
            Assert.Catch<GsaException>(()
                => _gsaService.AssignStudentToGsaThread(student.Id, gsaThread.Id));
        }

        [Test]
        public void TryToAssignStudentWhoHasMaxThreadCount_GsaExceptionThrown()
        {
            _isuExtraService.SetGroupSchedule(_groupM3200, _scheduleA);
            Student student = _isuService.AddStudent(_groupM3200, "Alex");
            
            var gsaCourseOther = new GsaCourse("Other");
            _gsaService.AddGsaCourse(gsaCourseOther, _ktuMegaFaculty);
            
            var gsaThread1 = new GsaThread(_gsaCourseMl, "Some", 20);
            var gsaThread2 = new GsaThread(gsaCourseOther, "Other", 20);
            
            _gsaService.AddGsaThread(gsaThread1);
            _gsaService.AddGsaThread(gsaThread2);
            _isuExtraService.SetGsaThreadSchedule(gsaThread1, _scheduleB);
            _isuExtraService.SetGsaThreadSchedule(gsaThread2, _scheduleC);
            
            _gsaService.AssignStudentToGsaThread(student.Id, gsaThread1.Id);
            Assert.Catch<GsaException>(() => 
                _gsaService.AssignStudentToGsaThread(student.Id, gsaThread2.Id));
        }

        [Test]
        public void RemoveUserFromGsaThread_NoSuchStudentInGsaThread()
        { 
            _isuExtraService.SetGroupSchedule(_groupM3200, _scheduleA);
            Student student = _isuService.AddStudent(_groupM3200, "Alex");

            var gsaThread = new GsaThread(_gsaCourseMl, "Some", 20);
            _gsaService.AddGsaThread(gsaThread);
            _isuExtraService.SetGsaThreadSchedule(gsaThread, _scheduleB);
            
            _gsaService.AssignStudentToGsaThread(student.Id, gsaThread.Id);
            
            _gsaService.RemoveStudentFromGsaThread(student.Id, gsaThread.Id);
            CollectionAssert.DoesNotContain(gsaThread.Students, student);
            Assert.True(!_gsaService.GetThreadsOfStudent(student).Any());
        }

        [Test]
        public void GetThreadsOfGsaCourse_AllThreadsReturned()
        {
            var gsaCourse = new GsaCourse("ML");
            _gsaService.AddGsaCourse(gsaCourse, _tintMegaFaculty);

            var expectedThreadList = new List<GsaThread>();
            for (int i = 0; i < 5; i++)
            {
                var gsaThread = new GsaThread(gsaCourse, "Some", 20);
                expectedThreadList.Add(gsaThread);
                _gsaService.AddGsaThread(gsaThread);
            }

            var actualThreadList = _gsaService.GetThreadsOfGsaCourse(gsaCourse.Id).ToList();
            CollectionAssert.AreEquivalent(expectedThreadList, actualThreadList);
        }

        [Test]
        public void GetStudentsOfGsaThread_AllThreadStudentsReturned()
        {
            var gsaThread = new GsaThread(_gsaCourseMl, "Some", 20);
            _gsaService.AddGsaThread(gsaThread);
            _isuExtraService.SetGsaThreadSchedule(gsaThread, _scheduleB);

            _isuExtraService.SetGroupSchedule(_groupM3200, _scheduleA);
            for (int i = 0; i < 5; i++)
            {
                Student student = _isuService.AddStudent(_groupM3200, $"Tom{i}");
                _gsaService.AssignStudentToGsaThread(student.Id, gsaThread.Id);
            }

            IReadOnlyCollection<Student> actualStudentList = _gsaService.GetStudentsOfGsaThread(gsaThread.Id);

            CollectionAssert.AreEquivalent(_groupM3200.Students, actualStudentList);
        }

        [Test]
        public void GetStudentsWithoutGsa_CorrectStudentsReturned()
        {
            _isuExtraService.SetGroupSchedule(_groupM3200, _scheduleA);

            var gsaThread = new GsaThread(_gsaCourseMl, "Some", 20);
            _gsaService.AddGsaThread(gsaThread);
            _isuExtraService.SetGsaThreadSchedule(gsaThread, _scheduleB);

            for (int i = 0; i < 5; i++)
            {
                Student student = _isuService.AddStudent(_groupM3200, $"Tom{i}");
                _gsaService.AssignStudentToGsaThread(student.Id, gsaThread.Id);
            }

            var studentsWithoutGsa = new List<Student>();
            for (int i = 0; i < 2; i++)
            {
                Student student = _isuService.AddStudent(_groupM3200, $"Loshara{i}");
                studentsWithoutGsa.Add(student);
            }

            var returnedList = _gsaService.GetStudentsWithNotEnoughGsa(_groupM3200).ToList();
            
            CollectionAssert.AreEquivalent(studentsWithoutGsa, returnedList);
        }
    }
}