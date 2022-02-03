using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using BackupsExtra.Storages.Impl.Remote.Tools;

namespace BackupsExtra.Storages.Impl.Remote.Client
{
    public class BackupClientX
    {
        public BackupClientX(string hostname, int port)
        {
            Hostname = hostname ?? throw new ArgumentNullException(nameof(hostname));
            Port = port;
        }

        public string Hostname { get; }
        public int Port { get; }

        public void SendZipDataToServer(Guid backupJobId, Guid restorePointId, byte[] zipData)
        {
            byte[] serializedCommandBytes = GetSerializedCommandBytes(
                CommandType.SaveZipData, backupJobId, restorePointId);

            using var client = new TcpClient();
            client.Connect(Hostname, Port);

            using NetworkStream stream = client.GetStream();

            stream.WriteByte((byte)serializedCommandBytes.Length);
            stream.Write(serializedCommandBytes);
            stream.Write(zipData);
        }

        public void DeletePointAtServer(Guid backupJobId, Guid restorePointId)
        {
            byte[] serializedCommandBytes = GetSerializedCommandBytes(
                CommandType.SaveZipData, backupJobId, restorePointId);

            using var client = new TcpClient();
            client.Connect(Hostname, Port);

            using NetworkStream stream = client.GetStream();

            stream.WriteByte((byte)serializedCommandBytes.Length);
            stream.Write(serializedCommandBytes);
        }

        public byte[] GetZipDataFromServer(Guid backupJobId, Guid restorePointId)
        {
            byte[] serializedCommandBytes = GetSerializedCommandBytes(
                CommandType.GetZipData, backupJobId, restorePointId);

            var zipDataBytes = new List<byte>();

            using var client = new TcpClient();
            client.Connect(Hostname, Port);

            using NetworkStream stream = client.GetStream();

            stream.WriteByte((byte)serializedCommandBytes.Length);
            stream.Write(serializedCommandBytes);

            byte[] buffer = new byte[256];

            do
            {
                int bytesReadCount = stream.Read(buffer, 0, buffer.Length);
                for (int i = 0; i < bytesReadCount; i++)
                    zipDataBytes.Add(buffer[i]);
            }
            while (stream.DataAvailable);

            return zipDataBytes.ToArray();
        }

        private byte[] GetSerializedCommandBytes(CommandType type, Guid backupJobId, Guid restorePointId)
        {
            var commandHeader = new CommandHeader(CommandType.SaveZipData, backupJobId, restorePointId);
            string serializedCommand = JsonSerializer.Serialize(commandHeader);
            byte[] serializedCommandBytes = Encoding.Unicode.GetBytes(serializedCommand);
            return serializedCommandBytes;
        }
    }
}