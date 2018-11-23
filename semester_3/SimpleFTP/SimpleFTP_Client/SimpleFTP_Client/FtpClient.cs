using System;
using System.IO;
using System.Net.Sockets;
using System.Collections.Generic;
using SimpleFTP_Client.Exceptions;

namespace SimpleFTP_Client
{
    /// <summary>
    /// FTP-клиент.
    /// </summary>
    public class FtpClient
    {
        /// <summary>
        /// TCP-клиент.
        /// </summary>
        private TcpClient client;

        /// <summary>
        /// Адрес сервера.
        /// </summary>
        public string HostName { get; }

        /// <summary>
        /// Порт, на котором сервер прослушивает подключения.
        /// </summary>
        public int HostPort { get; }

        /// <summary>
        /// Объект, позволяющий считывать информацию из потока.
        /// </summary>
        private StreamReader streamReader;

        /// <summary>
        /// Объект, позволяющий записывать информацию в поток.
        /// </summary>
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

        /// <summary>
        /// Метод, устанавливающий соединение с сервером.
        /// </summary>
        /// <returns>True, если соединение установлено.</returns>
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

        /// <summary>
        /// Запрос на листинг файлов в директории на сервере.
        /// </summary>
        /// <param name="path">Путь к директории на сервере.</param>
        /// <returns>Информация о файлах.</returns>
        public List<FileInfo> List(string path)
        {
            const int request = 1;

            if (!Connect())
            {
                throw new ConnectException();
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
                throw new ServerErrorException();
            }

            if (size == -1)
            {
                Disconnect();
                throw new DirectoryNotExistException();
            }

            var fileStructs = new List<FileInfo>();

            for (int i = 0; i < size; i++)
            {
                string fileName = streamReader.ReadLine();
                bool IsDir = "True" == streamReader.ReadLine();
                fileStructs.Add(new FileInfo(fileName, IsDir));
            }

            Disconnect();

            fileStructs.Sort();
            return fileStructs;
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
                throw new ConnectException();
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
                throw new ServerErrorException();
            }

            if (size == -1)
            {
                Disconnect();
                throw new FileNotExistException();
            }

            try
            {
                DownloadFile(pathToDownload);
            }
            catch (Exception)
            {
                throw;
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
            catch (Exception exception)
            {
                throw new Exception("ошибка скачивания файла", exception);
            }

            client.GetStream().CopyTo(fileStream);
            fileStream.Flush();

            fileStream.Close();
        }

        /// <summary>
        /// Метод, разрывающий соединение с сервером.
        /// </summary>
        public void Disconnect() => client.Close();
    }
}
