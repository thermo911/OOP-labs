using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Backups.Entities;
using Backups.Services.Archiving;
using Backups.Tools.Exceptions;
using BackupsExtra.Logging;
using BackupsExtra.Services.Archiving;
using BackupsExtra.Storages;
using Newtonsoft.Json;

namespace BackupsExtra.Entities
{
    public class BackupJobX
    {
        private HashSet<JobObject> _trackedJobJobObjects;
        private SortedSet<RestorePointX> _restorePoints;
        private PointsCountSettings _pointsCountSettings;

        public BackupJobX(
            IZipArchiverX zipArchiver,
            IBackupStorageX backupStorage,
            PointsCountSettings pointsCountSettings)
        {
            Id = Guid.NewGuid();
            ZipArchiver = zipArchiver ?? throw new ArgumentNullException(nameof(zipArchiver));
            BackupStorage = backupStorage ?? throw new ArgumentNullException(nameof(backupStorage));
            _restorePoints = new SortedSet<RestorePointX>();
            _trackedJobJobObjects = new HashSet<JobObject>();
            PointsCountSettings = pointsCountSettings ?? throw new ArgumentNullException(nameof(pointsCountSettings));
        }

        [JsonConstructor]
        private BackupJobX(
            Guid id,
            IZipArchiverX zipArchiver,
            IBackupStorageX backupStorage,
            HashSet<JobObject> trackedJobObjects,
            SortedSet<RestorePointX> restorePoints,
            PointsCountSettings pointsCountSettings,
            ILogger logger,
            bool checkJobObjects)
        {
            Id = id;
            ZipArchiver = zipArchiver ?? throw new ArgumentNullException(nameof(zipArchiver));
            BackupStorage = backupStorage ?? throw new ArgumentNullException(nameof(backupStorage));
            _trackedJobJobObjects = trackedJobObjects ?? throw new ArgumentNullException(nameof(trackedJobObjects));
            _restorePoints = restorePoints ?? throw new ArgumentNullException(nameof(restorePoints));
            PointsCountSettings = pointsCountSettings ?? throw new ArgumentNullException();
            Logger = logger;
            CheckJobObjects = checkJobObjects;
        }

        public ILogger Logger { get; set; }

        public Guid Id { get; }
        public IZipArchiverX ZipArchiver { get; }
        public IBackupStorageX BackupStorage { get; }
        public IReadOnlyCollection<JobObject> TrackedJobObjects => _trackedJobJobObjects;
        public IReadOnlyCollection<RestorePointX> RestorePoints => _restorePoints;
        public bool CheckJobObjects { get; set; } = false;

        public PointsCountSettings PointsCountSettings
        {
            get => _pointsCountSettings;
            set
            {
                _pointsCountSettings = value ?? throw new ArgumentNullException(nameof(value));
                DeletePointsIfNeeded();
            }
        }

        public RestorePointX CreateAndSaveRestorePoint()
            => CreateAndSaveRestorePoint(DateTime.Now);

        public void AddObjectToTrack(JobObject newJobObject)
        {
            if (newJobObject == null)
                throw new ArgumentNullException(nameof(newJobObject));

            if (CheckJobObjects && !newJobObject.FileInfo.Exists)
            {
                string message = $"File corresponds to {newJobObject} does not exist";
                Logger?.Log(this.ToString(), LogType.Error, message);
                throw new BackupException(message);
            }

            if (!_trackedJobJobObjects.Add(newJobObject))
            {
                string message = $"Job object {newJobObject} already tracked by {this}";
                Logger?.Log(this.ToString(), LogType.Error, message);
                throw new BackupException(message);
            }

            Logger?.Log(this.ToString(), $"{newJobObject} successfully added to {this}");
        }

        public RestorePointX CreateAndSaveRestorePoint(DateTime creationDateTime)
        {
            if (_trackedJobJobObjects.Count == 0)
            {
                string message = $"Attempt to create restore point without files in {this}";
                Logger?.Log(this.ToString(), LogType.Error, message);
                throw new BackupException(message);
            }

            Logger?.Log(this.ToString(), "Creating new restore point...");

            var jobObjectsShot = _trackedJobJobObjects.ToHashSet();
            var point = new RestorePointX(creationDateTime, jobObjectsShot);

            List<FileInfo> fileInfos = ExtractFileInfos(jobObjectsShot);
            byte[] zipData = ZipArchiver.GetZipBytes(fileInfos, point.Id.ToString());
            BackupStorage.SaveZipData(Id, point.Id, zipData);
            _restorePoints.Add(point);

            Logger?.Log(this.ToString(), $"{point} successfully created");

            DeletePointsIfNeeded();

            return point;
        }

        public void RestoreFilesFromRestorePoint(RestorePointX restorePoint, DirectoryInfo destination = null)
        {
            Logger?.Log(this.ToString(), $"Restoring files from {restorePoint}");

            byte[] zipData = BackupStorage.GetZipData(Id, restorePoint.Id);
            foreach (var jobObject in restorePoint.StoredJobObjects)
            {
                string fileName = jobObject.FileInfo.Name;
                destination ??= jobObject.FileInfo.Directory;
                string path = Path.Combine(destination.ToString(), fileName);
                ZipArchiver.ExtractFileFromZipData(zipData, fileName, path);
            }

            Logger?.Log(this.ToString(), $"Files from {restorePoint} successfully restored");
        }

        public override string ToString() => $"{GetType().Name}(id = {Id})";

        private static List<FileInfo> ExtractFileInfos(IEnumerable<JobObject> jobObjects)
            => jobObjects.Select(o => o.FileInfo).ToList();

        private void DeletePointsIfNeeded()
        {
            if (_restorePoints.Count < 1)
                return;

            var pointsList = _restorePoints.ToList();

            if (!PointsCountSettings.IsMatchingLimits(
                pointsList[pointsList.Count - 1],
                pointsList.Count - 1,
                _restorePoints.Count))
            {
                string message = "Attempt to delete all restore points";
                Logger?.Log(this.ToString(), LogType.Error, message);
                throw new BackupException(message);
            }

            Logger?.Log(this.ToString(), "Deleting restore points...");

            for (int i = 0; i < pointsList.Count - 1; i++)
            {
                if (PointsCountSettings.IsMatchingLimits(pointsList[i], i, _restorePoints.Count))
                    continue;

                if (ZipArchiver.Mode != ZipArchiverMode.ToSingleZip
                    && _pointsCountSettings.PreferMerge)
                {
                    MergePoints(pointsList[i], pointsList[i + 1]);
                    Logger?.Log(this.ToString(), $"Merge {pointsList[i]} into {pointsList[i + 1]}");
                }

                DeletePoint(pointsList[i]);
                Logger?.Log(this.ToString(), $"{pointsList[i]} deleted");
            }
        }

        private void MergePoints(RestorePointX mergedPoint, RestorePointX to)
        {
            byte[] mergedPointZipData = BackupStorage.GetZipData(Id, mergedPoint.Id);
            byte[] toPointZipData = BackupStorage.GetZipData(Id, to.Id);
            toPointZipData = ZipArchiver.MergeZipEntries(toPointZipData, mergedPointZipData);
            BackupStorage.SaveZipData(Id, to.Id, toPointZipData);
            to.Merge(mergedPoint);
        }

        private void DeletePoint(RestorePointX point)
        {
            BackupStorage.DeletePoint(Id, point.Id);
            _restorePoints.Remove(point);
        }
    }
}