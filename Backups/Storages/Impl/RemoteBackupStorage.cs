using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using Backups.Entities;
using Backups.Services.Archiving;
using BackupsNet.Client;
using BackupsNet.Dto;
using BackupsNet.Server;

namespace Backups.Storages.Impl
{
    public class RemoteBackupStorage : IBackupStorage
    {
        private readonly IZipArchiver _zipArchiver;
        private BackupClient _backupClient;

        public RemoteBackupStorage(IZipArchiver zipArchiver, BackupClient backupClient)
        {
            _zipArchiver = zipArchiver ?? throw new ArgumentNullException(nameof(zipArchiver));
            _backupClient = backupClient ?? throw new ArgumentNullException(nameof(backupClient));
        }

        public string SaveRestorePoint(string backupJobId, string restorePointId, IReadOnlyCollection<JobObject> jobObjects)
        {
            List<FileInfo> files = GetFilesFromJobObjects(jobObjects);

            byte[] zipBytes = _zipArchiver.GetZipBytes(files, restorePointId);

            string path = backupJobId;

            var zipDto = new ZipDto { DestinationFolder = path, Data = zipBytes };

            string zipDtoJson = JsonSerializer.Serialize(zipDto);
            byte[] data = Encoding.ASCII.GetBytes(zipDtoJson);
            _backupClient.SendData(BackupServerCommand.Save, data);
            return $"{_backupClient.Hostname}:{_backupClient.Port}";
        }

        private static List<FileInfo> GetFilesFromJobObjects(IEnumerable<JobObject> jobObjects)
            => jobObjects.Select(jobObj => jobObj.FileInfo).ToList();
    }
}