using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Backups.Services.Archiving;
using Backups.Services.Archiving.Impl;

namespace BackupsExtra.Services.Archiving.Impl
{
    public class SingleZipArchiverX : SingleZipArchiver, IZipArchiverX
    {
        public ZipArchiverMode Mode => ZipArchiverMode.ToSingleZip;

        public void ExtractFileFromZipData(byte[] zipData, string fileName, string destination)
        {
            using var memoryStream = new MemoryStream(zipData);
            using var outerArchive = new ZipArchive(memoryStream, ZipArchiveMode.Read);

            ZipArchiveEntry outerEntry = outerArchive.Entries.First();

            using Stream innerStream = outerEntry.Open();
            using var innerArchive = new ZipArchive(innerStream, ZipArchiveMode.Read);

            ZipArchiveEntry innerEntry = innerArchive.Entries.First(entry => entry.Name == fileName);
            innerEntry.ExtractToFile(destination, true);
        }

        public byte[] MergeZipEntries(byte[] baseZipData, byte[] mergedZipData)
        {
            throw new NotSupportedException($"Unable to merge entries created in '{Mode}' mode");
        }
    }
}