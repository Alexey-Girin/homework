using System;
using System.IO;
using System.Net.Sockets;
using GuiForFtpClient.Extentions;
using SimpleFTP_Client.Exceptions;
using System.Collections.ObjectModel;
using System.Windows;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Threading;

namespace SimpleFTP_Client
{
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

        public static readonly DependencyProperty CurrentDirectoryProperty;

        public string CurrentDirectory
        {
            get { return (string)GetValue(CurrentDirectoryProperty); }
            set { SetValue(CurrentDirectoryProperty, value); }
        }

        private object updateProgressLocker = new object();

        public ObservableCollection<string> FilesWhichDownloadNow { get; } =
            new ObservableCollection<string>();

        public ObservableCollection<string> FilesWhichDownloaded { get; } =
            new ObservableCollection<string>();

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

            CurrentDirectoryProperty = DependencyProperty.Register(
                "CurrentDirectory",
                typeof(string),
                typeof(FtpClient));
        }

        public void Reset()
        {
            Files.Clear();
            CurrentDirectory = null;
            FilesWhichDownloaded.Clear();
        }

        public async Task List(string path)
        {
            TcpClient client = null;
            const char request = '1';

            try
            {
                client = new TcpClient(HostName, int.Parse(HostPort));
            }
            catch (SocketException)
            {
                Reset();
                throw new ConnectException("Не удалось подключиться к серверу");
            }

            var streamWriter = new StreamWriter(client.GetStream()) { AutoFlush = true };
            var streamReader = new StreamReader(client.GetStream());

            await streamWriter.WriteAsync(request);
            await streamWriter.WriteLineAsync(path);

            int size = -1;

            try
            {
                size = int.Parse(await streamReader.ReadLineAsync());
            }
            catch (ArgumentNullException)
            {
                client.Close();
                throw new ServerErrorException("Не удалось выполнить запрос");
            }

            if (size == -1)
            {
                client.Close();
                throw new DirectoryNotExistException("Не удалось выполнить запрос");
            }

            Files.Update();
            CurrentDirectory = path;

            for (int i = 0; i < size; i++)
            {
                string fileName = await streamReader.ReadLineAsync();
                bool IsDir = "True" == await streamReader.ReadLineAsync();
                Files.Add(new FileInfo(fileName, IsDir));
            }

            client.Close();
        }

        public async Task DownloadFiles(string path)
        {
            var listOfFiles = new List<string>();
            InfoForDownload infoForDownload = new InfoForDownload(HostName, HostPort, PathToDownload);

            if (path != null)
            {
                await Get(path, infoForDownload);
                return;
            }

            foreach (var file in Files)
            {
                if (!file.IsDirectory)
                {
                    listOfFiles.Add(file.Name);
                }
            }

            for (int i = 0; i < listOfFiles.Count; i++)
            {
                int j = i;
                await Task.Factory.StartNew(() => Get(listOfFiles[j], infoForDownload));
            }
        }

        public async Task Get(string path, InfoForDownload infoForDownload)
        {
            TcpClient client = null;
            const char request = '2';

            try
            {
                client = new TcpClient(
                    infoForDownload.FixedHostName,
                    int.Parse(infoForDownload.FixedHostPort));
            }
            catch (SocketException)
            {
                throw new ConnectException("Не удалось подключиться к серверу - " +
                    $"{path}");
            }

            var streamWriter = new StreamWriter(client.GetStream()) { AutoFlush = true };
            var streamReader = new StreamReader(client.GetStream());

            await streamWriter.WriteAsync(request);
            await streamWriter.WriteLineAsync(path);

            long size = -1;

            try
            {
                size = long.Parse(streamReader.ReadLine());
            }
            catch (ArgumentNullException)
            {
                client.Close();

                throw new ServerErrorException("Не удалось выполнить запрос для " +
                    $"{path}");
            }

            if (size == -1)
            {
                client.Close();

                throw new FileNotExistException("Не удалось выполнить запрос для " +
                    $"{path}");
            }

            try
            {
                await DownloadFile(infoForDownload.FixedPathToDownload, client, path);
            }
            catch (Exception)
            {
                throw;
            }

            client.Close();
        }

        private async Task DownloadFile(string pathToDownload, TcpClient client, string path)
        {
            FileStream fileStream = null;

            try
            {
                fileStream = File.OpenWrite($"{pathToDownload}\\{GetFileName(path)}");
            }
            catch (Exception exception)
            {
                throw new Exception($"Не удалось скачать файл - {path}\n" +
                    $"{exception.Message}", exception);
            }

            lock (updateProgressLocker)
            {
                Dispatcher.Invoke(() => FilesWhichDownloadNow.Add(path));
            }

            await client.GetStream().CopyToAsync(fileStream);
            await fileStream.FlushAsync();
            fileStream.Close();

            lock (updateProgressLocker)
            {
                Dispatcher.Invoke(() => FilesWhichDownloadNow.Remove(path));
                Dispatcher.Invoke(() => FilesWhichDownloaded.Add(path));
            }
        }

        private string GenerateErrorReport(AggregateException exception)
        {
            string report = "";

            foreach (var e in exception.InnerExceptions)
            {
                report += $"{e.Message}\n\n";
            }

            return report;
        }

        private string GetFileName(string path)
        {
            int index = -1;

            for (int i = 0; i < path.Length; i++)
            {
                if (path[i] == '\\')
                {
                    index = i;
                }
            }

            return path.Substring(index + 1);
        }
    }
}
