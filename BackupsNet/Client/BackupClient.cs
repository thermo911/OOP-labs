using System;
using System.Net.Sockets;
using BackupsNet.Server;

namespace BackupsNet.Client
{
    public class BackupClient
    {

        public BackupClient(string hostname, int port)
        {
            Hostname = hostname ?? throw new ArgumentNullException(nameof(hostname));;
            Port = port;
        }

        public string Hostname { get; }
        public int Port { get; }

        public void SendData(BackupServerCommand command, byte[] data)
        {
            using var tcpClient = new TcpClient(Hostname, Port);
            using NetworkStream networkStream = tcpClient.GetStream();
            networkStream.WriteByte((byte)command);
            networkStream.Write(data);
        }
    }
}