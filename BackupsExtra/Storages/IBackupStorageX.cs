using System;
using Backups.Entities;
using BackupsExtra.Entities;

namespace BackupsExtra.Storages
{
    public interface IBackupStorageX
    {
        byte[] GetZipData(Guid backupJobId, Guid restorePointId);
        void SaveZipData(Guid backupJobId, Guid restorePointId, byte[] zipData);
        void DeletePoint(Guid backupJobId, Guid restorePointId);
    }
}