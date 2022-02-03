using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Backups.Entities;
using Backups.Services.Archiving;

namespace Backups.Storages.Impl
{
    public class LocalBackupStorage : IBackupStorage
    {
        private readonly IZipArchiver _zipArchiver;

        public LocalBackupStorage(string workingDirectory, IZipArchiver zipArchiver)
        {
            if (string.IsNullOrWhiteSpace(workingDirectory))
            {
                throw new ArgumentException($"" +
                                            $"{nameof(LocalBackupStorage)} constructor: " +
                                            $"'{nameof(workingDirectory)}' is invalid");
            }

            _zipArchiver = zipArchiver ?? throw new ArgumentNullException(nameof(zipArchiver));

            WorkingDirectory = new DirectoryInfo(workingDirectory);
            if (!WorkingDirectory.Exists)
                WorkingDirectory.Create();
        }

        public DirectoryInfo WorkingDirectory { get; }

        public string SaveRestorePoint(string backupJobId, string restorePointId, IReadOnlyCollection<JobObject> jobObjects)
        {
            string path = Path.Combine(
                WorkingDirectory.FullName,
                backupJobId,
                restorePointId);

            List<FileInfo> files = GetFilesFromJobObjects(jobObjects);

            byte[] zipBytes = _zipArchiver.GetZipBytes(files, restorePointId);
            ExtractZipToDirectory(zipBytes, path);
            return path;
        }

        private static List<FileInfo> GetFilesFromJobObjects(IEnumerable<JobObject> jobObjects)
            => jobObjects.Select(jobObj => jobObj.FileInfo).ToList();

        private static void ExtractZipToDirectory(byte[] zipBytes, string path)
        {
            var memoryStream = new MemoryStream(zipBytes);
            var zipArchive = new ZipArchive(memoryStream);
            zipArchive.ExtractToDirectory(path);
        }
    }
}