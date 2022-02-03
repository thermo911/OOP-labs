using Isu.Entities;
using IsuExtra.Entities;

namespace IsuExtra.Services
{
    public interface IIsuExtraService
    {
        void AddMegaFaculty(MegaFaculty megaFaculty);
        MegaFaculty GetMegaFacultyById(int megaFacultyId);

        void SetGroupMegaFaculty(Group group, int megaFacultyId);
        MegaFaculty GetGroupMegaFaculty(Group group);

        void SetGsaCourseMegaFaculty(GsaCourse gsaCourse, int megaFacultyId);
        MegaFaculty GetGsaCourseMegaFaculty(GsaCourse gsaCourse);

        void SetGroupSchedule(Group group, Schedule schedule);
        Schedule GetGroupSchedule(Group group);

        void SetGsaThreadSchedule(GsaThread gsaThread, Schedule schedule);
        Schedule GetGsaThreadSchedule(GsaThread gsaThread);
    }
}