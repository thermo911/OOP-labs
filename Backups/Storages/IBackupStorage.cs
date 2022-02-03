using System.Collections.Generic;
using System.IO;
using Backups.Entities;

namespace Backups.Storages
{
    public interface IBackupStorage
    {
        string SaveRestorePoint(string backupJobId, string restorePointId, IReadOnlyCollection<JobObject> jobObjects);
    }
}