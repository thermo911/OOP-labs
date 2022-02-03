using System;
using System.Collections.Generic;
using System.IO;
using Backups.Entities;
using Backups.Services.Backup;
using Backups.Storages;
using Backups.Tools.Exceptions;
using NUnit.Framework;

namespace Backups.Tests
{
    public class BackupServiceTest
    {
        private const string TestFileName = "test.txt";
        private BackupService _backupService;
        private IBackupStorage _storage = new BackupStorageMock();

        [SetUp]
        public void Setup()
        {
            if (!File.Exists(TestFileName))
                File.Create(TestFileName);
            _backupService = new BackupService("backup");
        }

        [Test]
        public void CreateBackupJob_BackupJobCreated()
        {
            BackupJob backupJob = _backupService.CreateAndRegisterBackupJob("job1", _storage);
            CollectionAssert.Contains(_backupService.BackupJobs, backupJob);
        }

        [Test]
        public void CreateBackupJob_ExceptionThrown()
        {
            BackupJob backupJob1 = _backupService.CreateAndRegisterBackupJob("job1", _storage);
            Assert.Catch<BackupException>(() =>
            {
                BackupJob backupJob2 = _backupService.CreateAndRegisterBackupJob("job1", _storage);
            });
        }

        [Test]
        public void AddJobObjectToBackupJob_JobObjectAdded()
        {
            var jobObject = new JobObject(TestFileName);
            Console.WriteLine(jobObject);
            BackupJob backupJob = _backupService.CreateAndRegisterBackupJob("job1", _storage);
            backupJob.AddJobObject(jobObject);
            CollectionAssert.Contains(backupJob.TrackedJobObjects, jobObject);
        }
    }

    internal class BackupStorageMock : IBackupStorage
    {
        public string SaveRestorePoint(string backupJobId, string restorePointId, IReadOnlyCollection<JobObject> jobObjects)
        {
            throw new NotImplementedException();
        }
    }
}