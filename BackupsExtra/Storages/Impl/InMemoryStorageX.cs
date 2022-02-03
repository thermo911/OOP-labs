using System;
using System.Collections.Generic;
using Backups.Tools.Exceptions;

namespace BackupsExtra.Storages.Impl
{
    public class InMemoryStorageX : IBackupStorageX
    {
        private Dictionary<Guid, Dictionary<Guid, byte[]>> _hierarchy;

        public InMemoryStorageX()
        {
            _hierarchy = new Dictionary<Guid, Dictionary<Guid, byte[]>>();
        }

        public byte[] GetZipData(Guid backupJobId, Guid restorePointId)
        {
            if (!_hierarchy.TryGetValue(backupJobId, out Dictionary<Guid, byte[]> points))
                throw new BackupException($"Backup job with id {backupJobId} not found");

            if (!points.TryGetValue(restorePointId, out byte[] data))
                throw new BackupException($"Data of restore point with id {restorePointId} not found");

            return data;
        }

        public void SaveZipData(Guid backupJobId, Guid restorePointId, byte[] zipData)
        {
            if (_hierarchy.TryGetValue(backupJobId, out Dictionary<Guid, byte[]> points))
            {
                points[restorePointId] = zipData;
            }
            else
            {
                _hierarchy[backupJobId] = new Dictionary<Guid, byte[]>();
                _hierarchy[backupJobId][restorePointId] = zipData;
            }
        }

        public void DeletePoint(Guid backupJobId, Guid restorePointId)
        {
            if (!_hierarchy.TryGetValue(backupJobId, out Dictionary<Guid, byte[]> points))
                throw new BackupException($"Backup job with id {backupJobId} not found");

            if (!points.Remove(restorePointId))
                throw new BackupException($"Data of restore point with id {restorePointId} not found");
        }
    }
}