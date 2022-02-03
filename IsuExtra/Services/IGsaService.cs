using System.Collections.Generic;
using Isu.Entities;
using IsuExtra.Entities;

namespace IsuExtra.Services
{
    public interface IGsaService
    {
        void AddGsaCourse(GsaCourse gsaCourse, MegaFaculty megaFaculty);
        void AddGsaThread(GsaThread gsaThread);
        void AssignStudentToGsaThread(int studentId, int gsaThreadId);
        void RemoveStudentFromGsaThread(int studentId, int gsaThreadId);
        IReadOnlyCollection<GsaThread> GetThreadsOfGsaCourse(int gsaCourseId);
        IReadOnlyCollection<Student> GetStudentsOfGsaThread(int gsaThreadId);
        IReadOnlyCollection<Student> GetStudentsWithNotEnoughGsa(Group group);
        IReadOnlyCollection<GsaThread> GetThreadsOfStudent(Student student);

        GsaCourse GetGsaCourseById(int gsaCourseId);
        GsaThread GetGsaThreadById(int gsaThreadId);
    }
}