using System;
using System.IO;
using System.IO.Compression;
using Backups.Entities;
using Backups.Services.Archiving.Impl;
using Backups.Services.Backup;
using Backups.Storages;
using Backups.Storages.Impl;
using BackupsNet.Client;

namespace Backups
{
    internal class Program
    {
        private static void Main()
        {
            const string root = @"C:\Users\therm\Desktop\bcpTest";
            var bs = new BackupService(root);
            IBackupStorage storage = new LocalBackupStorage(root, new SingleZipArchiver());
            BackupJob bj = bs.CreateAndRegisterBackupJob("shit", storage);
            var jobObject = new JobObject(@"C:\Users\therm\Desktop\Rezultaty_oprosa.xlsx");
            bj.AddJobObject(jobObject);
            RestorePoint rp = bj.CreateAndSaveRestorePoint("first");
            Console.WriteLine(rp.Path);
        }
    }
}
