using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace Backups.Services.Archiving.Impl
{
    public class SplitZipArchiver : IZipArchiver
    {
        public byte[] GetZipBytes(IReadOnlyCollection<FileInfo> files, string archiveName)
        {
            using var memoryStream = new MemoryStream();
            using (var zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Update))
            {
                foreach (FileInfo file in files)
                {
                    string entryName = $"{file.Name}";
                    ZipArchiveEntry entry = zipArchive.CreateEntry(entryName, CompressionLevel.NoCompression);
                    using Stream entryStream = entry.Open();
                    entryStream.Write(GetInnerBytes(file));
                }
            }

            return memoryStream.ToArray();
        }

        private byte[] GetInnerBytes(FileInfo file)
        {
            using var memoryStream = new MemoryStream();
            using (var zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Update))
            {
                // zipArchive.CreateEntryFromFile(file.FullName, file.Name);
                byte[] data = File.ReadAllBytes(file.FullName);
                var entry = zipArchive.CreateEntry(file.Name, CompressionLevel.NoCompression);
                using Stream entryStream = entry.Open();
                entryStream.Write(data);
            }

            return memoryStream.ToArray();
        }
    }
}