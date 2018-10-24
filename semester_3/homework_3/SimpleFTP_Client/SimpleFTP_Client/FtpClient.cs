using System;
using System.IO;
using System.Net.Sockets;

namespace SimpleFTP_Client
{
    /// <summary>
    /// FTP-клиент.
    /// </summary>
    public class FtpClient
    {
        private TcpClient client;

        private StreamWriter streamWriter;
        private StreamReader streamReader;

        public string HostName { get; private set; }
        public int HostPort { get; private set; }

        /// <summary>
        /// Конструктор экземпляра класса <see cref="FtpClient"/>.
        /// </summary>
        /// <param name="host">Адрес, прослушиваемый сервером.</param>
        /// <param name="port">Порт, прослушиваемый сервером.</param>
        public FtpClient(string host, int port)
        {
            HostName = host;
            HostPort = port;
        }

        /// <summary>
        /// Листинг файлов в директории на сервере.
        /// </summary>
        /// <param name="path">Путь к директории.</param>
        /// <returns>Список файлов в директории на сервере.</returns>
        public string[] List(string path)
        {
            const int request = 1;

            client = new TcpClient(HostName, HostPort);

            streamWriter = new StreamWriter(client.GetStream()) { AutoFlush = true };
            streamReader = new StreamReader(client.GetStream());

            streamWriter.Write(request);
            streamWriter.WriteLine(path);

            int size = -1;

            try
            {
                size = int.Parse(streamReader.ReadLine());
            }
            catch (ArgumentNullException)
            {
                client.Close();
                throw new Exception("ошибка исполнения запроса сервером");
            }

            if (size == -1)
            {
                client.Close();
                throw new Exception("ошибка. директория не существует");
            }

            string[] fileNames = new string[size];

            for (int i = 0; i < size; i++)
            {
                fileNames[i] = streamReader.ReadLine();
            }

            client.Close();
            return fileNames;
        }

        /// <summary>
        /// Получение файла с сервера
        /// </summary>
        /// <param name="path">Путь к файлу.</param>
        /// <returns>Данные файла.</returns>
        public byte[] Get(string path)
        {
            const int request = 2;

            client = new TcpClient(HostName, HostPort);

            streamWriter = new StreamWriter(client.GetStream()) { AutoFlush = true };
            streamReader = new StreamReader(client.GetStream());

            streamWriter.Write(request);
            streamWriter.WriteLine(path);

            long size = -1;

            try
            {
                size = long.Parse(streamReader.ReadLine());
            }
            catch (ArgumentNullException)
            {
                client.Close();
                throw new Exception("ошибка исполнения запроса сервером");
            }

            if (size == -1)
            {
                client.Close();
                throw new Exception("ошибка. файл не существует");
            }

            byte[] fileContent = new byte[size];
            client.GetStream().Read(fileContent, 0, fileContent.Length);

            client.Close();
            return fileContent;
        }
    }
}
