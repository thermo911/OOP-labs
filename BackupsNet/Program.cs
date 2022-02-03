using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using BackupsNet.Server;

namespace BackupsNet
{
    class Program
    {
        static void Main(string[] args)
        {
            var tcpListener = new TcpListener(IPAddress.Parse("127.0.0.1"), 8000);
            var backupServer = new BackupServer(@"C:\Users\therm\Desktop\serverRoot", tcpListener);
            Console.Write("Server running time (secs): ");
            int seconds = Convert.ToInt32(Console.ReadLine());
            backupServer.Run();
            Thread.Sleep(seconds * 1000);
            backupServer.Stop();
        }
    }
}