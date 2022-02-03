using System.Collections.Generic;
using System.IO;

namespace Backups.Services.Archiving
{
    public interface IZipArchiver
    {
        byte[] GetZipBytes(IReadOnlyCollection<FileInfo> files, string archiveName);
    }
}