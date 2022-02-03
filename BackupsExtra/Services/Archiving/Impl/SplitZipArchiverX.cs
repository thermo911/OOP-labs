using System.IO;
using System.IO.Compression;
using System.Linq;
using Backups.Services.Archiving;
using Backups.Services.Archiving.Impl;

namespace BackupsExtra.Services.Archiving.Impl
{
    public class SplitZipArchiverX : SplitZipArchiver, IZipArchiverX
    {
        public ZipArchiverMode Mode => ZipArchiverMode.ToSplitZips;

        public void ExtractFileFromZipData(byte[] zipData, string fileName, string destination)
        {
            using var memoryStream = new MemoryStream(zipData);
            using var outerArchive = new ZipArchive(memoryStream, ZipArchiveMode.Read);

            ZipArchiveEntry outerEntry = outerArchive.Entries.First(entry => entry.Name == fileName);

            using Stream innerStream = outerEntry.Open();
            using var innerArchive = new ZipArchive(innerStream, ZipArchiveMode.Read);

            ZipArchiveEntry innerEntry = innerArchive.Entries.First();
            innerEntry.ExtractToFile(destination, true);
        }

        public byte[] MergeZipEntries(byte[] baseZipData, byte[] mergedZipData)
        {
            using var baseOuterMemoryStream = new MemoryStream();
            baseOuterMemoryStream.Write(baseZipData);
            baseOuterMemoryStream.Seek(0, SeekOrigin.Begin);

            var baseOuterArchive = new ZipArchive(baseOuterMemoryStream, ZipArchiveMode.Update, false);

            using (baseOuterArchive)
            {
                using var mergedOuterMemoryStream = new MemoryStream(mergedZipData);
                using var mergedOuterArchive = new ZipArchive(mergedOuterMemoryStream, ZipArchiveMode.Read);

                var baseOuterEntryNames = baseOuterArchive.Entries.Select(entry => entry.Name).ToHashSet();

                foreach (var mergedOuterEntry in mergedOuterArchive.Entries)
                {
                    if (baseOuterEntryNames.Contains(mergedOuterEntry.Name))
                        continue;

                    using Stream mergedEntryStream = mergedOuterEntry.Open();
                    ZipArchiveEntry newBaseOuterEntry = baseOuterArchive.CreateEntry(mergedOuterEntry.Name);
                    using Stream newEntryStream = newBaseOuterEntry.Open();

                    mergedEntryStream.CopyTo(newEntryStream);
                }
            }

            return baseOuterMemoryStream.ToArray();
        }
    }
}