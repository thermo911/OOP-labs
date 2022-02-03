using System.Collections.Generic;
using Backups.Services.Archiving;

namespace BackupsExtra.Services.Archiving
{
    public interface IZipArchiverX : IZipArchiver
    {
        ZipArchiverMode Mode { get; }
        void ExtractFileFromZipData(byte[] zipData, string fileName, string destination);
        byte[] MergeZipEntries(byte[] baseZipData, byte[] mergedZipData);
    }
}