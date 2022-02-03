using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace Backups.Services.Archiving.Impl
{
    public class SingleZipArchiver : IZipArchiver
    {
        public byte[] GetZipBytes(IReadOnlyCollection<FileInfo> files, string archiveName)
        {
            using var memoryStream = new MemoryStream();
            using (var zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Update))
            {
                ZipArchiveEntry entry = zipArchive.CreateEntry($"{archiveName}");
                using Stream entryStream = entry.Open();
                entryStream.Write(GetInnerBytes(files));
            }

            return memoryStream.ToArray();
        }

        private byte[] GetInnerBytes(IReadOnlyCollection<FileInfo> files)
        {
            using var memoryStream = new MemoryStream();
            using (var zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Update))
            {
                foreach (FileInfo file in files)
                {
                    // zipArchive.CreateEntryFromFile(file.FullName, file.Name, CompressionLevel.NoCompression);
                    byte[] data = File.ReadAllBytes(file.FullName);
                    var entry = zipArchive.CreateEntry(file.Name, CompressionLevel.NoCompression);
                    using Stream entryStream = entry.Open();
                    entryStream.Write(data);
                }
            }

            return memoryStream.ToArray();
        }
    }
}