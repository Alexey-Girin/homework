using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace SimpleFTP_Server
{
    /// <summary>
    /// FTP-сервер.
    /// </summary>
    public class FtpServer
    {
        /// <summary>
        /// IP-адрес для прослушивания входящих подключений.
        /// </summary>
        public IPAddress Ip { get; } = IPAddress.Parse("127.0.0.1");

        /// <summary>
        /// Порт для прослушивания входящих подключений.
        /// </summary>
        public int Port { get; }

        /// <summary>
        /// Объект, прослушивающий входящие подключения.
        /// </summary>
        private TcpListener listener;

        /// <summary>
        /// Конструктор экземпляра класса <see cref="FtpServer"/>.
        /// </summary>
        /// <param name="portName">Порт для прослушивания входящих подключений.</param>
        public FtpServer(int portName)
        {
            Port = portName;
            listener = new TcpListener(Ip, Port);
        }

        /// <summary>
        /// Метод, запускающий сервер.
        /// </summary>
        public void Start()
        {
            listener.Start();

            Console.WriteLine("Ожидание подключений...\n");

            while (true)
            {
                var client = listener.AcceptTcpClient();
                ThreadPool.QueueUserWorkItem(ProcessingRequest, client);
            }
        }

        /// <summary>
        /// Метод, обрабатывающий запрос клиента.
        /// </summary>
        /// <param name="сlientObject">Подключенный клиент.</param>
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

        /// <summary>
        /// Метод, исполняющий запрос клиента на листинг файлов в директории.
        /// </summary>
        /// <param name="stream">Поток, используемый для обмена данными с клиентом.</param>
        /// <param name="path">Путь к директории.</param>
        /// <param name="clientIp">IP-адрес клиента.</param>
        /// <returns>True, если запрос исполнен успешно.</returns>
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

        /// <summary>
        /// Метод, исполняющий запрос клиента на получение файла.
        /// </summary>
        /// <param name="stream">Поток, используемый для обмена данными с клиентом.</param>
        /// <param name="path">Путь к файлу.</param>
        /// <param name="clientIp">IP-адрес клиента.</param>
        /// <returns>True, если запрос исполнен успешно.</returns>
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
