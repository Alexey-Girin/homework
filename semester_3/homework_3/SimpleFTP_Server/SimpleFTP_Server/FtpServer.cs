using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace SimpleFTP_Server
{
    public class FtpServer
    {
        private IPAddress ip = IPAddress.Parse("127.0.0.1");
        private int port = 8888;

        private TcpListener server;

        public FtpServer() => server = new TcpListener(ip, port);

        public void Start()
        {
            server.Start();
            Console.WriteLine("Слушаю...\n");

            while (true)
            {
                var client = server.AcceptTcpClient();
                ThreadPool.QueueUserWorkItem(ProcessingRequest, client);
            }
        }

        private void ProcessingRequest(object сlientObject)
        {
            TcpClient client = (TcpClient)сlientObject;
            IPAddress clientIp = ((IPEndPoint)client.Client.RemoteEndPoint).Address;

            Console.WriteLine($"{clientIp} :новое подключение");

            var reader = new StreamReader(client.GetStream());

            string request = reader.ReadLine();
            string path = request.Substring(2);

            if (request[0] == '1')
            {
                Console.WriteLine($"{clientIp} :исполнение запроса на листинг фалов в {path}");
                ListRequestExecution(client.GetStream(), path);
            }

            if (request[0] == '2')
            {
                Console.WriteLine($"{clientIp} :исполнение запроса на получение файла: {path}");
                GetRequestExecution(client.GetStream(), path);
            }

            Console.WriteLine($"{clientIp} :запрос исполнен\n");
            client.Close();
        }

        private void ListRequestExecution(NetworkStream stream, string path)
        {
            StreamWriter writer = new StreamWriter(stream) { AutoFlush = true };

            string[] fileNames = Directory.GetDirectories(path);
            writer.WriteLine(fileNames.Length);

            foreach (var fileName in fileNames)
            {
                writer.WriteLine(fileName);
            }
        }

        private void GetRequestExecution(NetworkStream stream, string path)
        {
            StreamWriter writer = new StreamWriter(stream) { AutoFlush = true };

            using (FileStream fileStream = File.OpenRead(path))
            {
                writer.WriteLine(fileStream.Length);
                fileStream.CopyTo(stream);
            }
        }
    }
}
