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

        public string HostName { get; }
        public int HostPort { get; }

        private StreamReader streamReader;
        private StreamWriter streamWriter;

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

        private bool Connect()
        {
            try
            {
                client = new TcpClient(HostName, HostPort);
            }
            catch (SocketException)
            {
                return false;
            }

            streamWriter = new StreamWriter(client.GetStream()) { AutoFlush = true };
            streamReader = new StreamReader(client.GetStream());

            return true;
        }

        public void Disconnect() => client.Close();

        /// <summary>
        /// Запрос на листинг файлов в директории на сервере.
        /// </summary>
        /// <param name="path">Путь к директории на сервере.</param>
        /// <returns>Список файлов в директории на сервере.</returns>
        public string[] List(string path)
        {
            const int request = 1;

            if(!Connect())
            {
                throw new Exception("ошибка подключения");
            }

            streamWriter.Write(request);
            streamWriter.WriteLine(path);

            int size = -1;

            try
            {
                size = int.Parse(streamReader.ReadLine());
            }
            catch (ArgumentNullException)
            {
                Disconnect();
                throw new Exception("ошибка исполнения запроса сервером");
            }

            if (size == -1)
            {
                Disconnect();
                throw new Exception("ошибка. директория не существует");
            }

            string[] fileNames = new string[size];

            for (int i = 0; i < size; i++)
            {
                fileNames[i] = streamReader.ReadLine();
            }

            Disconnect();
            return fileNames;
        }

        /// <summary>
        /// Запрос на скачивание файла с сервера.
        /// </summary>
        /// <param name="path">Путь к файлу на сервере.</param>
        /// <param name="pathToSave">Путь к месту скачивания файла.</param>
        public void Get(string path, string pathToDownload)
        {
            const int request = 2;

            if (!Connect())
            {
                throw new Exception("ошибка подключения");
            }

            streamWriter.Write(request);
            streamWriter.WriteLine(path);

            long size = -1;

            try
            {
                size = long.Parse(streamReader.ReadLine());
            }
            catch (ArgumentNullException)
            {
                Disconnect();
                throw new Exception("ошибка исполнения запроса сервером");
            }

            if (size == -1)
            {
                Disconnect();
                throw new Exception("ошибка. файл не существует");
            }

            try
            {
                DownloadFile(pathToDownload);
            }
            catch(Exception exception)
            {
                Disconnect();
                throw new Exception($"ошибка скачивания файла: {exception.Message}");
            }
            
            Disconnect();
        }

        /// <summary>
        /// Скачивание файла.
        /// </summary>
        /// <param name="pathToDownload">Путь к месту скачивания файла.</param>
        private void DownloadFile(string pathToDownload)
        {
            FileStream fileStream = null;

            try
            {
                fileStream = File.OpenWrite(pathToDownload);
            }
            catch(Exception exception)
            {
                throw new Exception(exception.Message);
            }

            client.GetStream().CopyTo(fileStream);
            fileStream.Flush();

            fileStream.Close();
        }
    }
}
