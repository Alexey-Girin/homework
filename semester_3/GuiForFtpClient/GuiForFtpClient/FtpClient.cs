using System;
using System.IO;
using System.Net.Sockets;
using System.Collections.Generic;
using SimpleFTP_Client.Exceptions;
using System.Collections.ObjectModel;
using System.Windows;

namespace SimpleFTP_Client
{
    /// <summary>
    /// FTP-клиент.
    /// </summary>
    public class FtpClient : DependencyObject
    {
        /// <summary>
        /// TCP-клиент.
        /// </summary>
        private TcpClient client;

        public static readonly DependencyProperty HostNameProperty;

        public static readonly DependencyProperty HostPortProperty;

        public static readonly DependencyProperty PathToDownloadProperty;

        public string HostName
        {
            get { return (string)GetValue(HostNameProperty); }
            set { SetValue(HostNameProperty, value); }
        }

        public string HostPort
        {
            get { return (string)GetValue(HostPortProperty); }
            set { SetValue(HostPortProperty, value); }
        }

        public string PathToDownload
        {
            get { return (string)GetValue(PathToDownloadProperty); }
            set { SetValue(PathToDownloadProperty, value); }
        }


        /// <summary>
        /// Объект, позволяющий считывать информацию из потока.
        /// </summary>
        private StreamReader streamReader;

        /// <summary>
        /// Объект, позволяющий записывать информацию в поток.
        /// </summary>
        private StreamWriter streamWriter;

        public ObservableCollection<FileInfo> Files { get; } = new ObservableCollection<FileInfo>();

        static FtpClient()
        {
            HostNameProperty = DependencyProperty.Register(
                "HostName",
                typeof(string),
                typeof(FtpClient));

            HostPortProperty = DependencyProperty.Register(
                "HostPort",
                typeof(string),
                typeof(FtpClient));

            PathToDownloadProperty = DependencyProperty.Register(
                "PathToDownload",
                typeof(string),
                typeof(FtpClient));
        }

        /// <summary>
        /// Метод, устанавливающий соединение с сервером.
        /// </summary>
        /// <returns>True, если соединение установлено.</returns>
        private bool Connect()
        {
            try
            {
                client = new TcpClient(HostName, int.Parse(HostPort));
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
        public void List(string path)
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

            Files.Clear();
            Files.Add(new FileInfo("...", false));

            for (int i = 0; i < size; i++)
            {
                string fileName = streamReader.ReadLine();
                bool IsDir = "True" == streamReader.ReadLine();
                Files.Add(new FileInfo(fileName, IsDir));
            }

            Disconnect();
        }

        /// <summary>
        /// Запрос на скачивание файла с сервера.
        /// </summary>
        /// <param name="path">Путь к файлу на сервере.</param>
        /// <param name="pathToSave">Путь к месту скачивания файла.</param>
        public void Get(string path)
        {
            string pathToDownload = PathToDownload;
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
