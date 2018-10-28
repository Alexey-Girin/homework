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
        public IPAddress Ip { get; } = IPAddress.Any;

        /// <summary>
        /// Порт для прослушивания входящих подключений.
        /// </summary>
        public int Port { get; }

        /// <summary>
        /// Объект, прослушивающий входящие подключения.
        /// </summary>
        private TcpListener listener;

        /// <summary>
        /// Количество входящих подключений.
        /// </summary>
        private volatile int countOfСonnectedСlients = 0;

        private volatile bool isAbleToContinue = true;

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
        public async void Start()
        {
            listener.Start();

            Console.WriteLine("Ожидание подключений...\n");

            while (isAbleToContinue)
            {
                TcpClient client = null;

                try
                {
                    client = await listener.AcceptTcpClientAsync();
                }
                catch (InvalidOperationException)
                {
                    break;
                }

                ThreadPool.QueueUserWorkItem(ProcessingRequest, client);
            }

            while (countOfСonnectedСlients != 0)
            {
                Thread.Sleep(1);
            }
        }

        /// <summary>
        /// Метод, обрабатывающий запрос клиента.
        /// </summary>
        /// <param name="сlientObject">Подключенный клиент.</param>
        private void ProcessingRequest(object сlientObject)
        {
            TcpClient client = (TcpClient)сlientObject;
            IPAddress clientIp = GetIPAddress(client);

            Console.WriteLine($"{clientIp} :новое подключение");
            countOfСonnectedСlients++;

            StreamReader reader = new StreamReader(client.GetStream());

            char request = (char)reader.Read();
            string path = reader.ReadLine();

            if (request == '1')
            {
                Console.WriteLine($"{clientIp} :исполнение запроса на листинг фалов в {path}");
                ListRequestExecution(client, path);
            }
            else if (request == '2')
            {
                Console.WriteLine($"{clientIp} :исполнение запроса на получение файла: {path}");
                GetRequestExecution(client, path);
            }
            else
            {
                Console.WriteLine($"{clientIp} :некорректный запрос. клиент отключен\n");
                Disconnect(client);
            }
        }

        /// <summary>
        /// Метод, исполняющий запрос клиента на листинг файлов в директории.
        /// </summary>
        /// <param name="client">Подключенный клиент.</param>
        /// <param name="path">Путь к директории.</param>
        private void ListRequestExecution(TcpClient client, string path)
        {
            StreamWriter writer = new StreamWriter(client.GetStream()) { AutoFlush = true };
            IPAddress clientIp = GetIPAddress(client);

            string[] directoryObjectNames = null;

            try
            {
                directoryObjectNames = GetDirectoryObjectNames(path);
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine($"{clientIp} :директория не найдена. клиент отключен\n");
                writer.WriteLine(-1);

                Disconnect(client);
                return;
            }
            catch (Exception exception)
            {
                Console.WriteLine($"{clientIp} :ошибка, клиент отключен - {exception.Message}\n");

                Disconnect(client);
                return;
            }

            writer.WriteLine(directoryObjectNames.Length);

            foreach (var fileName in directoryObjectNames)
            {
                writer.WriteLine(fileName);
                writer.WriteLine(Directory.Exists(fileName));
            }

            Console.WriteLine($"{clientIp} :запрос исполнен. клиент отключен\n");
            Disconnect(client);
        }

        /// <summary>
        /// Метод, исполняющий запрос клиента на получение файла.
        /// </summary>
        /// <param name="client">Подключенный клиент.</param>
        /// <param name="path">Путь к файлу.</param>
        private void GetRequestExecution(TcpClient client, string path)
        {
            StreamWriter writer = new StreamWriter(client.GetStream()) { AutoFlush = true };
            IPAddress clientIp = GetIPAddress(client);

            try
            {
                using (FileStream fileStream = File.OpenRead(path))
                {
                    writer.WriteLine(fileStream.Length);
                    fileStream.CopyTo(client.GetStream());
                }
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine($"{clientIp} :файл не найден. клиент отключен\n");
                writer.WriteLine(-1);
            }
            catch (Exception exception)
            {
                Console.WriteLine($"{clientIp} :ошибка, клиент отключен - {exception.Message}\n");
            }

            Console.WriteLine($"{clientIp} :запрос исполнен. клиент отключен\n");
            Disconnect(client);
        }

        /// <summary>
        /// Метод, возвращающий список подкаталогов и файлов в директории.
        /// </summary>
        /// <param name="path">Путь к директории.</param>
        /// <returns>Список подкаталогов и файлов в директории.</returns>
        private string[] GetDirectoryObjectNames(string path)
        {
            string[] subdirectoryNames = Directory.GetDirectories(path);
            string[] fileNames = Directory.GetFiles(path);

            string[] directoryObjectNames = new string[subdirectoryNames.Length + fileNames.Length];

            subdirectoryNames.CopyTo(directoryObjectNames, 0);
            fileNames.CopyTo(directoryObjectNames, subdirectoryNames.Length);

            return directoryObjectNames;
        }

        /// <summary>
        /// Метод, получающий IP-адрес клиента.
        /// </summary>
        /// <param name="client">Подключенный клиент.</param>
        /// <returns>IP-адрес клиента.</returns>
        private IPAddress GetIPAddress(TcpClient client)
            => ((IPEndPoint)client.Client.RemoteEndPoint).Address;

        /// <summary>
        /// Метод, завершающий соединений с клиентом.
        /// </summary>
        /// <param name="client">Подключенный клиент.</param>
        private void Disconnect(TcpClient client)
        {
            client.Close();
            countOfСonnectedСlients--;
        }

        public void Stop()
        {
            isAbleToContinue = false;
            listener.Stop();
        }
    }
}