using System;
using System.Collections.Generic;
using System.Linq;
using Isu.Entities;
using Isu.Services;
using IsuExtra.Entities;
using IsuExtra.Tools.Exceptions;

namespace IsuExtra.Services.Impl
{
    public class GsaService : IGsaService
    {
        private Dictionary<GsaCourse, HashSet<GsaThread>> _threadSets;
        private Dictionary<Student, HashSet<GsaThread>> _studentThreads;
        private IIsuService _isuService;
        private IIsuExtraService _isuExtraService;

        public GsaService(
            IIsuService isuService,
            IIsuExtraService isuExtraService,
            uint maxThreadsPerStudent)
        {
            _isuService = isuService ?? throw new ArgumentNullException(nameof(isuService));
            _isuExtraService = isuExtraService ?? throw new ArgumentNullException(nameof(isuExtraService));
            MaxThreadsPerStudent = maxThreadsPerStudent;
            _threadSets = new Dictionary<GsaCourse, HashSet<GsaThread>>();
            _studentThreads = new Dictionary<Student, HashSet<GsaThread>>();
        }

        public uint MaxThreadsPerStudent { get; }

        public void AddGsaCourse(GsaCourse gsaCourse, MegaFaculty megaFaculty)
        {
            if (gsaCourse == null)
                throw new ArgumentNullException(nameof(gsaCourse));

            if (megaFaculty == null)
                throw new ArgumentNullException(nameof(megaFaculty));

            _isuExtraService.SetGsaCourseMegaFaculty(gsaCourse, megaFaculty.Id);

            if (!_threadSets.TryAdd(gsaCourse, new HashSet<GsaThread>()))
                throw new GsaException("gsaCourse already added");
        }

        public void AddGsaThread(GsaThread gsaThread)
        {
            if (gsaThread == null)
                throw new ArgumentNullException(nameof(gsaThread));

            if (!_threadSets.TryGetValue(gsaThread.GsaCourse, out HashSet<GsaThread> threads))
                throw new GsaException("unknown GsaCourse");

            if (threads.Contains(gsaThread))
                throw new GsaException("gsaThread already added");

            threads.Add(gsaThread);
        }

        public void AssignStudentToGsaThread(int studentId, int gsaThreadId)
        {
            Student student = _isuService.GetStudent(studentId);
            GsaThread gsaThread = GetGsaThreadById(gsaThreadId);

            if (HaveSameMegaFaculty(student.Group, gsaThread.GsaCourse))
                throw new GsaException("same MegaFaculty");

            if (HaveScheduleCollisions(student, gsaThread))
                throw new GsaException("have schedule collisions");

            _studentThreads.TryAdd(student, new HashSet<GsaThread>());

            if (gsaThread.Students.Contains(student))
                throw new GsaException("already assigned");

            if (_studentThreads[student].Count >= MaxThreadsPerStudent)
                throw new GsaException("max count reached");

            gsaThread.AddStudent(student);
            _studentThreads[student].Add(gsaThread);
        }

        public void RemoveStudentFromGsaThread(int studentId, int gsaThreadId)
        {
            Student student = _isuService.GetStudent(studentId);
            GsaThread gsaThread = GetGsaThreadById(gsaThreadId);

            if (!_studentThreads.TryGetValue(student, out HashSet<GsaThread> threads))
            {
                throw new GsaException(
                    "fail to remove student from thread: student not assigned");
            }

            threads.Remove(gsaThread);
            gsaThread.RemoveStudent(student);

            if (!threads.Any())
                _studentThreads.Remove(student);
        }

        public IReadOnlyCollection<GsaThread> GetThreadsOfGsaCourse(int gsaCourseId)
        {
            GsaCourse gsaCourse = GetGsaCourseById(gsaCourseId);
            return _threadSets[gsaCourse];
        }

        public IReadOnlyCollection<Student> GetStudentsOfGsaThread(int gsaThreadId)
        {
            GsaThread gsaThread = GetGsaThreadById(gsaThreadId);
            return gsaThread.Students;
        }

        public IReadOnlyCollection<Student> GetStudentsWithNotEnoughGsa(Group group)
        {
            if (group == null)
                throw new ArgumentNullException(nameof(group));

            return group.Students.Where(
                student => !_studentThreads.ContainsKey(student) ||
                           _studentThreads[student].Count < MaxThreadsPerStudent).ToList();
        }

        public IReadOnlyCollection<GsaThread> GetThreadsOfStudent(Student student)
        {
            if (student == null)
                throw new ArgumentNullException(nameof(student));

            return _studentThreads.TryGetValue(student, out HashSet<GsaThread> threads) ?
                threads : new HashSet<GsaThread>();
        }

        public GsaThread GetGsaThreadById(int gsaThreadId)
        {
            return _threadSets.Values.First(set =>
                    set.Any(gsaThread => gsaThread.Id == gsaThreadId))
                .First(gsaThread => gsaThread.Id == gsaThreadId);
        }

        public GsaCourse GetGsaCourseById(int gsaCourseId)
            => _threadSets.Keys.First(key => key.Id == gsaCourseId);

        private bool HaveScheduleCollisions(Student student, GsaThread gsaThread)
        {
            Schedule studentSchedule = _isuExtraService.GetGroupSchedule(student.Group);
            Schedule gsaThreadSchedule = _isuExtraService.GetGsaThreadSchedule(gsaThread);

            return studentSchedule.HasCollision(gsaThreadSchedule);
        }

        private bool HaveSameMegaFaculty(Group group, GsaCourse gsaCourse)
        {
            MegaFaculty groupMegaFaculty = _isuExtraService.GetGroupMegaFaculty(group);
            MegaFaculty gsaCourseMegaFaculty = _isuExtraService.GetGsaCourseMegaFaculty(gsaCourse);

            return groupMegaFaculty.Equals(gsaCourseMegaFaculty);
        }
    }
}