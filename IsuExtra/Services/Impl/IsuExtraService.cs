using System.Collections.Generic;
using System.Linq;
using Isu.Entities;
using IsuExtra.Entities;
using IsuExtra.Tools.Exceptions;

namespace IsuExtra.Services.Impl
{
    public class IsuExtraService : IIsuExtraService
    {
        private HashSet<MegaFaculty> _megaFaculties;
        private Dictionary<Group, MegaFaculty> _groupMegaFaculty;
        private Dictionary<GsaCourse, MegaFaculty> _gsaCourseMegaFaculty;
        private Dictionary<Group, Schedule> _groupSchedule;
        private Dictionary<GsaThread, Schedule> _gsaThreadSchedule;

        public IsuExtraService()
        {
            _megaFaculties = new HashSet<MegaFaculty>();
            _groupMegaFaculty = new Dictionary<Group, MegaFaculty>();
            _gsaCourseMegaFaculty = new Dictionary<GsaCourse, MegaFaculty>();
            _groupSchedule = new Dictionary<Group, Schedule>();
            _gsaThreadSchedule = new Dictionary<GsaThread, Schedule>();
        }

        public void AddMegaFaculty(MegaFaculty megaFaculty)
        {
            if (_megaFaculties.Contains(megaFaculty))
                throw new IsuExtraException("MegaFaculty already added");

            _megaFaculties.Add(megaFaculty);
        }

        public MegaFaculty GetMegaFacultyById(int megaFacultyId)
        {
            foreach (MegaFaculty megaFaculty in _megaFaculties
                .Where(megaFaculty => megaFaculty.Id == megaFacultyId))
                return megaFaculty;

            throw new IsuExtraException($"MegaFaculty with id {megaFacultyId} not found");
        }

        public void SetGroupMegaFaculty(Group group, int megaFacultyId)
        {
            MegaFaculty megaFaculty = GetMegaFacultyById(megaFacultyId);

            if (!_groupMegaFaculty.TryAdd(group, megaFaculty))
                throw new IsuExtraException("MegaFaculty already set to Group");
        }

        public MegaFaculty GetGroupMegaFaculty(Group group)
        {
            if (_groupMegaFaculty.TryGetValue(group, out MegaFaculty megaFaculty))
                return megaFaculty;

            throw new IsuExtraException("MegaFaculty is not set to group");
        }

        public void SetGsaCourseMegaFaculty(GsaCourse gsaCourse, int megaFacultyId)
        {
            MegaFaculty megaFaculty = GetMegaFacultyById(megaFacultyId);

            if (!_gsaCourseMegaFaculty.TryAdd(gsaCourse, megaFaculty))
                throw new IsuExtraException("MegaFaculty already set to GsaCourse");
        }

        public MegaFaculty GetGsaCourseMegaFaculty(GsaCourse gsaCourse)
        {
            if (_gsaCourseMegaFaculty.TryGetValue(gsaCourse, out MegaFaculty megaFaculty))
                return megaFaculty;

            throw new IsuExtraException("Group MegaFaculty is not set to gsaCourse");
        }

        public void SetGroupSchedule(Group group, Schedule schedule)
            => _groupSchedule[group] = schedule;

        public Schedule GetGroupSchedule(Group group)
        {
            if (_groupSchedule.TryGetValue(group, out Schedule schedule))
                return schedule;

            throw new IsuExtraException("Group schedule is not set");
        }

        public void SetGsaThreadSchedule(GsaThread gsaThread, Schedule schedule)
            => _gsaThreadSchedule[gsaThread] = schedule;

        public Schedule GetGsaThreadSchedule(GsaThread gsaThread)
        {
            if (_gsaThreadSchedule.TryGetValue(gsaThread, out Schedule schedule))
                return schedule;

            throw new IsuExtraException("GsaThread schedule is not set");
        }
    }
}