using System;
using System.IO;
using System.Text;
using BackupsExtra.Entities;
using Newtonsoft.Json;

namespace BackupsExtra.SavingState
{
    public static class BackupJobLoader
    {
        public static void SaveBackupJob(BackupJobX backupJob, string path)
        {
            if (backupJob == null)
                throw new ArgumentNullException(nameof(backupJob));

            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException($"{nameof(path)} is null or white space");

            string json = JsonConvert.SerializeObject(backupJob, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto,
            });

            File.WriteAllText(path, json, Encoding.Unicode);
        }

        public static BackupJobX LoadBackupJob(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                throw new ArgumentException($"{nameof(path)} is null or white space");

            string json = Encoding.Unicode.GetString(File.ReadAllBytes(path));
            return JsonConvert.DeserializeObject<BackupJobX>(json, new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Auto,
            });
        }
    }
}