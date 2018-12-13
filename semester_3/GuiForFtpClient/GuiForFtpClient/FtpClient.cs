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

        public List<string> FilesWhichDownloadNow { get; } =
            new List<string>();

        public List<string> FilesWhichDownloaded { get; } =
           new List<string>();

        public static readonly DependencyProperty CurrentDirectoryProperty;

        public string CurrentDirectory
        {
            get { return (string)GetValue(CurrentDirectoryProperty); }
            set { SetValue(CurrentDirectoryProperty, value); }
        }

        private object locker = new object();

        private object saveLocker = new object();

        private object filesWhichDownloadNowLocker = new object();

        private object filesWhichDownloadedLocker = new object();

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
                Reset();
                throw new ConnectException("Не удалось подключиться к серверу");
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
                string fileName = streamReader.ReadLine();
                bool IsDir = "True" == streamReader.ReadLine();
                Files.Add(new FileInfo(fileName, IsDir));
            }

            client.Close();
        }

        public string DownloadAllFilesInDirectory()
        {
            var listOfFiles = new List<FileInfo>();
            InfoForDownload infoForDownload = null;

            lock (locker)
            {
                infoForDownload = new InfoForDownload(HostName, HostPort, PathToDownload);
            }

            foreach (var file in Files)
            {
                if (!file.IsDirectory)
                {
                    listOfFiles.Add(file);
                }
            }

            var tasks = new Task[listOfFiles.Count];

            for (int i = 0; i < listOfFiles.Count; i++)
            {
                int j = i;

                tasks[j] = Task.Factory.StartNew(() => Get(
                    listOfFiles[j].Name, new InfoForDownload(
                        infoForDownload.FixedHostName,
                        infoForDownload.FixedHostPort,
                        infoForDownload.FixedPathToDownload)));
            }

            try
            {
                Task.WaitAll(tasks);
            }
            catch (AggregateException exception)
            {
                return GenerateErrorReport(exception);
            }

            return null;
        }

        public void DownloadFile(string path)
        {
            lock (locker)
            {
                Get(path, new InfoForDownload(HostName, HostPort, PathToDownload));
            }
        }

        public void Get(string path, InfoForDownload infoForDownload)
        {
            lock (filesWhichDownloadNowLocker)
            {
                FilesWhichDownloadNow.Add(path);
            }

            TcpClient client = null;
            const int request = 2;

            try
            {
                client = new TcpClient(
                    infoForDownload.FixedHostName,
                    int.Parse(infoForDownload.FixedHostPort));
            }
            catch (SocketException)
            {
                lock (filesWhichDownloadNowLocker)
                {
                    FilesWhichDownloadNow.RemoveAt(FilesWhichDownloadNow.IndexOf(path));
                }

                throw new ConnectException("Не удалось подключиться к серверу - " +
                    $"{path}");
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

                lock (filesWhichDownloadNowLocker)
                {
                    FilesWhichDownloadNow.RemoveAt(FilesWhichDownloadNow.IndexOf(path));
                }

                throw new ServerErrorException("Не удалось выполнить запрос для " +
                    $"{path}");
            }

            if (size == -1)
            {
                client.Close();

                lock (filesWhichDownloadNowLocker)
                {
                    FilesWhichDownloadNow.RemoveAt(FilesWhichDownloadNow.IndexOf(path));
                }

                throw new FileNotExistException("Не удалось выполнить запрос для " +
                    $"{path}");
            }

            try
            {
                Download(infoForDownload.FixedPathToDownload, client, path);
            }
            catch (Exception)
            {
                lock (filesWhichDownloadNowLocker)
                {
                    FilesWhichDownloadNow.RemoveAt(FilesWhichDownloadNow.IndexOf(path));
                }

                throw;
            }

            client.Close();

            lock (filesWhichDownloadNowLocker)
            {
                FilesWhichDownloadNow.RemoveAt(FilesWhichDownloadNow.IndexOf(path));
            }

            lock (filesWhichDownloadedLocker)
            {
                FilesWhichDownloaded.Add(path);
            }
        }

        private void Download(string pathToDownload, TcpClient client, string path)
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

            client.GetStream().CopyTo(fileStream);

            lock (saveLocker)
            {
                fileStream.Flush();
            }

            fileStream.Close();
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
