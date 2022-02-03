using System.IO;
using System.IO.Compression;

namespace BackupsNet.Services
{
    public class ZipDataUnarchiver
    {
        private const string TempFilename = "temp.zip";
        
        public ZipDataUnarchiver(string workingDirectory)
        {
            WorkingDirectory = new DirectoryInfo(workingDirectory);
            if (!WorkingDirectory.Exists)
                WorkingDirectory.Create();
        }

        public DirectoryInfo WorkingDirectory { get; }

        public void UnarchiveData(byte[] data)
        {
            string tempPath = Path.Combine(WorkingDirectory.FullName, TempFilename);
            File.WriteAllBytes(tempPath, data);
            ZipFile.ExtractToDirectory(tempPath, WorkingDirectory.FullName);
            File.Delete(tempPath);
        }
    }
}