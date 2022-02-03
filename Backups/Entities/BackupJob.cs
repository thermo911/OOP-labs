using System;
using System.Collections.Generic;
using System.Linq;
using Backups.Storages;
using Backups.Tools.Exceptions;

namespace Backups.Entities
{
    public class BackupJob : IEquatable<BackupJob>
    {
        private HashSet<JobObject> _trackedJobObjects;
        private List<RestorePoint> _restorePoints;
        private IBackupStorage _backupStorage;

        public BackupJob(string id, IBackupStorage backupStorage)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            _backupStorage = backupStorage ?? throw new ArgumentNullException(nameof(backupStorage));
            _trackedJobObjects = new HashSet<JobObject>();
            _restorePoints = new List<RestorePoint>();
        }

        public string Id { get; }
        public IReadOnlyCollection<JobObject> TrackedJobObjects => _trackedJobObjects;
        public IReadOnlyCollection<RestorePoint> RestorePoints => _restorePoints;

        public void AddJobObject(JobObject jobObject)
        {
            if (jobObject == null)
                throw new ArgumentNullException(nameof(jobObject));

            if (!_trackedJobObjects.Add(jobObject))
                throw new BackupException($"jobObject {jobObject} has been already added");
        }

        public void RemoveJobObject(JobObject jobObject)
        {
            if (jobObject == null)
                throw new ArgumentNullException(nameof(jobObject));

            if (!_trackedJobObjects.Remove(jobObject))
                throw new BackupException($"no such a jobObject {jobObject} in backupJob");
        }

        public RestorePoint CreateAndSaveRestorePoint(string restorePointId)
        {
            if (HasRestorePointWithId(restorePointId))
                throw new BackupException($"Restore point with id {restorePointId} already exists");

            string path = _backupStorage.SaveRestorePoint(Id, restorePointId, _trackedJobObjects);
            var restorePoint = new RestorePoint(restorePointId, _trackedJobObjects.ToList(), path, DateTime.Now);
            _restorePoints.Add(restorePoint);
            return restorePoint;
        }

        public bool Equals(BackupJob other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id.Equals(other.Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((BackupJob)obj);
        }

        public override int GetHashCode() => Id != null ? Id.GetHashCode() : 0;

        private bool HasRestorePointWithId(string restorePointId) => _restorePoints.Any(restorePoint => restorePoint.Id.Equals(restorePointId));
    }
}