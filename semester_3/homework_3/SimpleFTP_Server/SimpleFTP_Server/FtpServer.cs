using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace SimpleFTP_Server
{
    public class FtpServer
    {
        public IPAddress Ip { get; } = IPAddress.Parse("127.0.0.1");
        public int Port { get; }

        private TcpListener server;

        public FtpServer(int portName)
        {
            Port = portName;
            server = new TcpListener(Ip, Port);
        }

        public void Start()
        {
            server.Start();

            Console.WriteLine("Ожидание подключения...\n");

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

            StreamReader reader = new StreamReader(client.GetStream());

            char request = (char)reader.Read();

            if (request != '1' && request != '2')
            {
                Console.WriteLine($"{clientIp} :некорректный запрос. клиент отключен\n");
                client.Close();
                return;
            }

            string path = reader.ReadLine();

            bool isSuccessfulExecution = true;

            if (request == '1')
            {
                Console.WriteLine($"{clientIp} :исполнение запроса на листинг фалов в {path}");
                isSuccessfulExecution = ListRequestExecution(client.GetStream(), path, clientIp);
            }
            else 
            {
                Console.WriteLine($"{clientIp} :исполнение запроса на получение файла: {path}");
                isSuccessfulExecution = GetRequestExecution(client.GetStream(), path, clientIp);
            }

            client.Close();

            if (isSuccessfulExecution)
            {
                Console.WriteLine($"{clientIp} :запрос исполнен. клиент отключен\n");
            }
        }

        private bool ListRequestExecution(NetworkStream stream, string path, IPAddress clientIp)
        {
            StreamWriter writer = new StreamWriter(stream) { AutoFlush = true };

            string[] fileNames = null;

            try
            {
                fileNames = Directory.GetDirectories(path);
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine($"{clientIp} :директория не найдена. клиент отключен\n");
                writer.WriteLine(-1);
                return false;
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine($"{clientIp} :запрет доступа. клиент отключен\n");
                return false;
            }
            catch (PathTooLongException)
            {
                Console.WriteLine($"{clientIp} :путь слишком длинный. клиент отключен\n");
                return false;
            }
            catch (ArgumentException)
            {
                Console.WriteLine($"{clientIp} :путь имеет недопустимую форму. клиент отключен\n");
                return false;
            }
            catch (NotSupportedException)
            {
                Console.WriteLine($"{clientIp} :формат пути не поддерживается. клиент отключен\n");
                return false;
            }

            writer.WriteLine(fileNames.Length);

            foreach (var fileName in fileNames)
            {
                writer.WriteLine(fileName);
            }

            return true;
        }

        private bool GetRequestExecution(NetworkStream stream, string path, IPAddress clientIp)
        {
            StreamWriter writer = new StreamWriter(stream) { AutoFlush = true };

            try
            {
                using (FileStream fileStream = File.OpenRead(path))
                {
                    writer.WriteLine(fileStream.Length);
                    fileStream.CopyTo(stream);
                }
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine($"{clientIp} :директория не найдена. клиент отключен\n");
                return false;
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine($"{clientIp} :запрет доступа. клиент отключен\n");
                return false;
            }
            catch (PathTooLongException)
            {
                Console.WriteLine($"{clientIp} :путь слишком длинный. клиент отключен\n");
                return false;
            }
            catch (ArgumentException)
            {
                Console.WriteLine($"{clientIp} :путь имеет недопустимую форму. клиент отключен\n");
                return false;
            }
            catch (NotSupportedException)
            {
                Console.WriteLine($"{clientIp} :формат пути не поддерживается. клиент отключен\n");
                return false;
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"{clientIp} :файл не найден. клиент отключен\n");
                writer.WriteLine(-1);
                return false;
            }

            return true;
        }
    }
}
