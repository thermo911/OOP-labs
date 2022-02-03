using System;

namespace BackupsExtra.Storages.Impl.Remote.Tools
{
    public class CommandHeader
    {
        public CommandHeader(CommandType type, Guid backupJobId, Guid restorePointId)
        {
            Type = type;
            BackupJobId = backupJobId;
            RestorePointId = restorePointId;
        }

        public CommandType Type { get; }
        public Guid BackupJobId { get; }
        public Guid RestorePointId { get; }
    }
}