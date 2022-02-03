using System;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Threading;
using BackupsNet.Dto;
using BackupsNet.Services;

namespace BackupsNet.Server
{
    public class BackupServer
    {
        private bool _running = false;
        
        public BackupServer(string rootDirectory, TcpListener tcpListener)
        {
            RootDirectory = new DirectoryInfo(rootDirectory);
            TcpListener = tcpListener ?? throw new ArgumentNullException(nameof(tcpListener));
            
            if (!RootDirectory.Exists)
                RootDirectory.Create();
            
            ListeningThread = new Thread(StartListening);
        }

        public DirectoryInfo RootDirectory { get; }
        public TcpListener TcpListener { get; }
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

        private static ZipDto GetZipDtoFromClientStream(NetworkStream networkStream)
        {
            string jsonString = new StreamReader(networkStream).ReadToEnd();
            Console.WriteLine($"accepted string: {jsonString}");
            ZipDto zipDto = JsonSerializer.Deserialize<ZipDto>(jsonString) ??
                            throw new SerializationException("failed to deserialize ZipDto");
            return zipDto;
        }

        private void SaveDataFromZipDto(ZipDto zipDto)
        {
            string path = Path.Combine(RootDirectory.FullName, zipDto.DestinationFolder);
            var unarchiver = new ZipDataUnarchiver(path);
            unarchiver.UnarchiveData(zipDto.Data);
        }

        private void PerformCommandFromClient(TcpClient client)
        {
            using NetworkStream stream = client.GetStream();
            var command = (BackupServerCommand)(byte)stream.ReadByte();

            switch (command)
            {
                case BackupServerCommand.Save:
                    ZipDto zipDto = GetZipDtoFromClientStream(stream);
                    SaveDataFromZipDto(zipDto);
                    break;
                default:
                    Console.WriteLine($"unknown command with code {(byte)command}");
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
                    Console.WriteLine("Connected");
                    PerformCommandFromClient(client);
                }
                catch (SocketException)
                {
                    break;
                }
                catch (InvalidOperationException)
                {
                    break;
                }
            }
        }
    }
}