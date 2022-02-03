using System;
using System.Collections.Generic;
using System.IO;
using Backups.Entities;
using Backups.Storages;
using Backups.Tools.Exceptions;

namespace Backups.Services.Backup
{
    public class BackupService
    {
        private HashSet<BackupJob> _backupJobs;

        public BackupService(string workingDirectory)
        {
            if (workingDirectory == null)
                throw new ArgumentNullException(nameof(workingDirectory));

            WorkingDirectory = new DirectoryInfo(workingDirectory);
            if (!WorkingDirectory.Exists)
                WorkingDirectory.Create();

            _backupJobs = new HashSet<BackupJob>();
        }

        public DirectoryInfo WorkingDirectory { get; }
        public IReadOnlyCollection<BackupJob> BackupJobs => _backupJobs;

        public BackupJob CreateAndRegisterBackupJob(string id, IBackupStorage backupStorage)
        {
            if (backupStorage == null)
                throw new ArgumentNullException(nameof(backupStorage));

            var backupJob = new BackupJob(id, backupStorage);
            if (_backupJobs.Contains(backupJob))
                throw new BackupException($"BackupJob with id {id} already added");

            _backupJobs.Add(backupJob);
            return backupJob;
        }
    }
}