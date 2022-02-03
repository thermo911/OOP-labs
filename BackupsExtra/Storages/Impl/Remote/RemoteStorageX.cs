using System;
using BackupsExtra.Storages.Impl.Remote.Client;

namespace BackupsExtra.Storages.Impl.Remote
{
    public class RemoteStorageX : IBackupStorageX
    {
        public RemoteStorageX(string hostname, int port)
        {
            Client = new BackupClientX(hostname, port);
        }

        public BackupClientX Client { get; }

        public byte[] GetZipData(Guid backupJobId, Guid restorePointId)
            => Client.GetZipDataFromServer(backupJobId, restorePointId);

        public void SaveZipData(Guid backupJobId, Guid restorePointId, byte[] zipData)
            => Client.SendZipDataToServer(backupJobId, restorePointId, zipData);

        public void DeletePoint(Guid backupJobId, Guid restorePointId)
            => Client.DeletePointAtServer(backupJobId, restorePointId);
    }
}