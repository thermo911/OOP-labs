using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Backups.Entities;
using BackupsExtra.Entities;
using BackupsExtra.SavingState;
using BackupsExtra.Services.Archiving;
using BackupsExtra.Services.Archiving.Impl;
using BackupsExtra.Storages.Impl;
using Newtonsoft.Json;

namespace BackupsExtra
{
    internal class Program
    {
        private static void Main()
        {
            IZipArchiverX zipArchiverX = new SplitZipArchiverX();

            var list1 = new List<FileInfo>
            {
                new FileInfo(@"C:\Users\therm\Desktop\files\1\a.txt"),
                new FileInfo(@"C:\Users\therm\Desktop\files\1\b.txt"),
            };

            var job = new BackupJobX(
                zipArchiverX,
                new LocalStorageX(@"C:\Users\therm\Desktop\files\job"),
                PointsCountSettings.NoLimitsSettings());

            job.AddObjectToTrack(new JobObject(list1[0].FullName));
            job.CreateAndSaveRestorePoint();

            Console.WriteLine(job.Id);

            BackupJobLoader.SaveBackupJob(job, @"C:\Users\therm\Desktop\files\job.json");
            job = BackupJobLoader.LoadBackupJob(@"C:\Users\therm\Desktop\files\job.json");

            Console.WriteLine(job.Id);
        }
    }
}