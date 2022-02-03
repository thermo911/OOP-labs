using System;
using System.Collections.Generic;
using System.IO;
using Backups.Entities;
using Backups.Services.Archiving;
using Backups.Tools.Exceptions;
using BackupsExtra.Entities;
using BackupsExtra.Services.Archiving;
using BackupsExtra.Services.Archiving.Impl;
using BackupsExtra.Storages.Impl;
using Moq;
using NUnit.Framework;

namespace BackupsExtra.Tests
{
    public class BackupsExtraTests
    {
        private SingleZipArchiverX _singleArchiver = new SingleZipArchiverX();
        private SplitZipArchiverX _splitZipArchiverX = new SplitZipArchiverX();
        private BackupJobX _simpleJob;
        
        [SetUp]
        public void Setup()
        {
            var mock = new Mock<IZipArchiverX>();
            mock.SetupGet(arch => arch.Mode)
                .Returns(ZipArchiverMode.ToSingleZip);

            mock.Setup(arch =>
                    arch.GetZipBytes(
                        It.IsAny<IReadOnlyCollection<FileInfo>>(), 
                        It.IsAny<string>()))
                .Returns<IReadOnlyCollection<FileInfo>, string>(
                    (x, y) => new byte[]{1, 2, 3, 4});

            mock.Setup(arch =>
                arch.ExtractFileFromZipData(
                    It.IsAny<byte[]>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()));

            mock.Setup(arch => arch.MergeZipEntries(
                    It.IsAny<byte[]>(),
                    It.IsAny<byte[]>()))
                .Returns<byte[], byte[]>((x, y) => new byte[] {1, 2, 3});
            

            _simpleJob = new BackupJobX(
                mock.Object,
                new InMemoryStorageX(),
                PointsCountSettings.NoLimitsSettings());
        }

        [Test]
        public void AddTrackedObject_TrackedObjectAdded()
        {
            var jobObject = new JobObject("a.txt");
            
            _simpleJob.AddObjectToTrack(jobObject);
            CollectionAssert.Contains(_simpleJob.TrackedJobObjects, jobObject);
        }

        [Test]
        public void CreateRestorePoint_NoTrackedObjects_ThrowBackupException()
        {
            Assert.Catch<BackupException>(() => _simpleJob.CreateAndSaveRestorePoint());
        }

        [Test]
        public void CreateRestorePoint_RestorePointCreated()
        {
            var jobObject = new JobObject("b.txt");
            
            _simpleJob.AddObjectToTrack(jobObject);
            RestorePointX point = _simpleJob.CreateAndSaveRestorePoint();
            CollectionAssert.Contains(_simpleJob.RestorePoints, point);
        }

        [Test]
        public void ChangeSettingsCount_PointsDeleted()
        {
            var jobObject = new JobObject("c.txt");
            _simpleJob.AddObjectToTrack(jobObject);
            
            var firstPoint = _simpleJob.CreateAndSaveRestorePoint();
            var secondPoint = _simpleJob.CreateAndSaveRestorePoint();
            
            Assert.AreEqual(2, _simpleJob.RestorePoints.Count);

            var newSettings = PointsCountSettings.BuildNew()
                .MaxPointsCount(1)
                .LimitMatchingMode(PointsCountSettings.LimitMatchingMode.MatchAll)
                .Build();
            
            _simpleJob.PointsCountSettings = newSettings;
            
            Assert.AreEqual(1, _simpleJob.RestorePoints.Count);
            CollectionAssert.Contains(_simpleJob.RestorePoints, secondPoint);
        }

        [Test]
        public void ChangeSettingsExistenceTime_PointsDeleted()
        {
            var jobObject = new JobObject("d.txt");
            _simpleJob.AddObjectToTrack(jobObject);
            
            var firstPoint = _simpleJob.CreateAndSaveRestorePoint(DateTime.Now.AddDays(-7));
            var secondPoint = _simpleJob.CreateAndSaveRestorePoint();
            
            Assert.AreEqual(2, _simpleJob.RestorePoints.Count);

            var newSettings = PointsCountSettings.BuildNew()
                .MaxPointExistenceTime(new TimeSpan(1, 0, 0, 0))
                .LimitMatchingMode(PointsCountSettings.LimitMatchingMode.MatchAll)
                .Build();
            
            _simpleJob.PointsCountSettings = newSettings;
            
            Assert.AreEqual(1, _simpleJob.RestorePoints.Count);
            CollectionAssert.Contains(_simpleJob.RestorePoints, secondPoint);
        }

        [Test]
        public void ChangeSettings_TryToDeleteSinglePoint_ThrowBackupException()
        {
            var jobObject = new JobObject("e.txt");
            _simpleJob.AddObjectToTrack(jobObject);
            
            var firstPoint = _simpleJob.CreateAndSaveRestorePoint(DateTime.Now.AddDays(-7));

            var newSettings = PointsCountSettings.BuildNew()
                .MaxPointExistenceTime(new TimeSpan(1, 0, 0, 0))
                .LimitMatchingMode(PointsCountSettings.LimitMatchingMode.MatchAll)
                .Build();
            
            Assert.Catch<BackupException>(() => 
                _simpleJob.PointsCountSettings = newSettings);
        }

        private string CreateFile(string name)
        {
            var info = new FileInfo(name);
            var stream = File.Create(info.FullName);
            stream.Close();
            return info.FullName;
        }
    }
}