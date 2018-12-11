using System;
using System.IO;
using System.Net.Sockets;
using GuiForFtpClient.Extentions;
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

        public void List(string path)
        {
            TcpClient client = null;
            const int request = 1;

            try
            {
                client = new TcpClient(HostName, int.Parse(HostPort));
            }
            catch (SocketException)
            {
                throw new ConnectException();
            }

            var streamWriter = new StreamWriter(client.GetStream()) { AutoFlush = true };
            var streamReader = new StreamReader(client.GetStream());

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
                throw new ServerErrorException();
            }

            if (size == -1)
            {
                client.Close();
                throw new DirectoryNotExistException();
            }

            Files.Update();

            for (int i = 0; i < size; i++)
            {
                string fileName = streamReader.ReadLine();
                bool IsDir = "True" == streamReader.ReadLine();
                Files.Add(new FileInfo(fileName, IsDir));
            }

            client.Close();
        }

        /// <summary>
        /// Запрос на скачивание файла с сервера.
        /// </summary>
        /// <param name="path">Путь к файлу на сервере.</param>
        /// <param name="pathToSave">Путь к месту скачивания файла.</param>
        public void Get(string path)
        {
            string pathToDownload = PathToDownload;
            TcpClient client = null;
            const int request = 2;

            try
            {
                client = new TcpClient(HostName, int.Parse(HostPort));
            }
            catch (SocketException)
            {
                throw new ConnectException();
            }

            var streamWriter = new StreamWriter(client.GetStream()) { AutoFlush = true };
            var streamReader = new StreamReader(client.GetStream());

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
                throw new ServerErrorException();
            }

            if (size == -1)
            {
                client.Close();
                throw new FileNotExistException();
            }

            try
            {
                DownloadFile(pathToDownload, client);
            }
            catch (Exception)
            {
                throw;
            }

            client.Close();
        }

        /// <summary>
        /// Скачивание файла.
        /// </summary>
        /// <param name="pathToDownload">Путь к месту скачивания файла.</param>
        private void DownloadFile(string pathToDownload, TcpClient client)
        {
            FileStream fileStream = null;

            try
            {
                fileStream = File.OpenWrite(pathToDownload);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                throw new Exception("ошибка скачивания файла", exception);
            }

            client.GetStream().CopyTo(fileStream);
            fileStream.Flush();

            fileStream.Close();
        }
    }
}
