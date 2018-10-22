using System;
using System.Net;
using System.Net.Sockets;
using System.IO;

namespace SimpleFTP_Server
{
    public class FtpServer
    {
        private IPAddress ip = IPAddress.Parse("127.0.0.1");
        private int port = 8888;

        private TcpListener server;
        private TcpClient client;

        private NetworkStream stream;
        private StreamReader streamReader;
        private StreamWriter streamWriter;

        public FtpServer()
        {
            server = new TcpListener(ip, port);
        }

        public void Start()
        {
            server.Start();
            Console.WriteLine("Слушаю...");

            while (true)
            {
                client = server.AcceptTcpClient();
                stream = client.GetStream();

                Console.WriteLine("Новое подключение");

                streamReader = new StreamReader(stream);
                streamWriter = new StreamWriter(stream)
                {
                    AutoFlush = true
                };

                string path = streamReader.ReadLine();
                string[] filesList = Directory.GetDirectories(path);

                streamWriter.WriteLine(filesList.Length);

                for (int i = 0; i < filesList.Length; i++)
                {
                    streamWriter.WriteLine(filesList[i]);
                }
            }
        }
    }
}
