using System;
using System.IO;
using Backups.Tools.Exceptions;
using Newtonsoft.Json;

namespace BackupsExtra.Storages.Impl
{
    public class LocalStorageX : IBackupStorageX
    {
        public LocalStorageX(string rootPath)
        {
            RootDirectory = new DirectoryInfo(rootPath);
            if (!RootDirectory.Exists)
                RootDirectory.Create();
        }

        [JsonIgnore]
        public DirectoryInfo RootDirectory { get; }

        public string RootPath => RootDirectory.FullName;

        public byte[] GetZipData(Guid backupJobId, Guid restorePointId)
        {
            string path = GetPathToData(backupJobId, restorePointId);

            if (!File.Exists(path))
            {
                throw new BackupException(
                    $"file for restore point with id {restorePointId} " +
                    $"of backup job with id {backupJobId} not found");
            }

            return File.ReadAllBytes(path);
        }

        public void SaveZipData(Guid backupJobId, Guid restorePointId, byte[] zipData)
        {
            string location = GetDataLocation(backupJobId);
            if (!Directory.Exists(location))
                Directory.CreateDirectory(location);

            string path = GetPathToData(backupJobId, restorePointId);

            File.WriteAllBytes(path, zipData);
        }

        public void DeletePoint(Guid backupJobId, Guid restorePointId)
        {
            string path = GetPathToData(backupJobId, restorePointId);

            if (!File.Exists(path))
            {
                throw new BackupException(
                    $"file for restore point with id {restorePointId} " +
                    $"of backup job with id {backupJobId} not found");
            }

            File.Delete(path);
        }

        private string GetDataLocation(Guid backupJobId)
            => Path.Combine(RootDirectory.FullName, backupJobId.ToString());

        private string GetPathToData(Guid backupJobId, Guid restorePointId)
            => Path.Combine(RootDirectory.FullName, backupJobId.ToString(), restorePointId.ToString());
    }
}