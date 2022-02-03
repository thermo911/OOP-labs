using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using BackupsExtra.Storages.Impl;
using BackupsExtra.Storages.Impl.Remote.Tools;

namespace BackupsExtra.Storages.Impl.Remote.Server
{
    public class BackupServerX
    {
        private bool _running = false;

        public BackupServerX(string rootDirectory, TcpListener tcpListener)
        {
            RootDirectory = new DirectoryInfo(rootDirectory);
            TcpListener = tcpListener ?? throw new ArgumentNullException(nameof(tcpListener));

            if (!RootDirectory.Exists)
                RootDirectory.Create();

            BackupStorage = new LocalStorageX(RootDirectory.FullName);
            ListeningThread = new Thread(StartListening);
        }

        public DirectoryInfo RootDirectory { get; }
        public TcpListener TcpListener { get; }
        public LocalStorageX BackupStorage { get; }
        public Thread ListeningThread { get; }

        public void Run()
        {
            _running = true;
            ListeningThread.Start();
        }

        public void Stop()
        {
            TcpListener.Stop();
            _running = false;
            ListeningThread.Join();
        }

        private void PerformCommandFromClient(TcpClient client)
        {
            using NetworkStream stream = client.GetStream();
            int serializedCommandSize = stream.ReadByte();
            byte[] serializedCommandBytes = new byte[serializedCommandSize];
            stream.Read(serializedCommandBytes, 0, serializedCommandSize);

            string commandJsonString = Encoding.Unicode.GetString(serializedCommandBytes);
            CommandHeader command = JsonSerializer.Deserialize<CommandHeader>(commandJsonString);

            switch (command?.Type)
            {
                case CommandType.DeleteZipData:
                    BackupStorage.DeletePoint(command.BackupJobId, command.RestorePointId);
                    break;

                case CommandType.SaveZipData:
                    byte[] buffer = new byte[256];
                    var zipDataBytes = new List<byte>();
                    do
                    {
                        int bytesCount = stream.Read(buffer, 0, buffer.Length);
                        for (int i = 0; i < bytesCount; i++)
                            zipDataBytes.Add(buffer[i]);
                    }
                    while (stream.DataAvailable);

                    BackupStorage.SaveZipData(
                        command.BackupJobId,
                        command.RestorePointId,
                        zipDataBytes.ToArray());
                    break;

                case CommandType.GetZipData:
                    byte[] zipData = BackupStorage.GetZipData(
                        command.BackupJobId, command.RestorePointId);
                    stream.Write(zipData);
                    break;
            }
        }

        private void StartListening()
        {
            TcpListener.Start();

            while (_running)
            {
                try
                {
                    TcpClient client = TcpListener.AcceptTcpClient();
                    PerformCommandFromClient(client);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
