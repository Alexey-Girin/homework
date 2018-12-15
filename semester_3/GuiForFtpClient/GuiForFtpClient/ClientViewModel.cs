using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using SimpleFTP_Client;
using SimpleFTP_Client.Exceptions;

namespace GuiForFtpClient
{
    public class ClientViewModel : DependencyObject
    {
        public FtpClient Client = new FtpClient();

        public ObservableCollection<FileInfo> Files { get; } =
            new ObservableCollection<FileInfo>();

        public ObservableCollection<string> FilesWhichDownloadNow { get; }

        public ObservableCollection<string> FilesWhichDownloaded { get; }

        public ObservableCollection<FileDownloadError> DownloadErrors { get; }

        public ObservableCollection<string> A { get; set; }

        public static readonly DependencyProperty CurrentDirectoryProperty;

        public static readonly DependencyProperty HostNameProperty;

        public static readonly DependencyProperty HostPortProperty;

        public static readonly DependencyProperty PathToDownloadProperty;

        public string CurrentDirectory
        {
            get { return (string)GetValue(CurrentDirectoryProperty); }
            set { SetValue(CurrentDirectoryProperty, value); }
        }

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

        private Stack<string> pathHistory = new Stack<string>();

        static ClientViewModel()
        {
            CurrentDirectoryProperty = DependencyProperty.Register(
                "CurrentDirectory",
                typeof(string),
                typeof(ClientViewModel));

            HostNameProperty = DependencyProperty.Register(
               "HostName",
               typeof(string),
               typeof(ClientViewModel));

            HostPortProperty = DependencyProperty.Register(
                "HostPort",
                typeof(string),
                typeof(ClientViewModel));

            PathToDownloadProperty = DependencyProperty.Register(
                "PathToDownload",
                typeof(string),
                typeof(FtpClient));
        }

        public ClientViewModel()
        {
            FilesWhichDownloaded = Client.FilesWhichDownloaded;
            FilesWhichDownloadNow = Client.FilesWhichDownloadNow;
            DownloadErrors = Client.DownloadErrors;
        }

        public void Reset()
        {
            Files.Clear();
            CurrentDirectory = null;
            Client.Reset();
        }

        public async Task GetDirectory(string path)
        {
            if (FilesWhichDownloadNow.Count != 0)
            {
                MessageBox.Show("Дождитесь окончания загрузки");
                return;
            }

            bool isBack = path == "..."; 

            if (isBack)
            {
                if (pathHistory.Count == 1)
                {
                    return;
                }

                pathHistory.Pop();
                path = pathHistory.Peek();
            }

            List<FileInfo> listOfFiles = null;

            try
            {
                listOfFiles = await Client.List(path, new ServerInfo(HostName, HostPort));
            }
            catch (ConnectException exception)
            {
                Reset();
                MessageBox.Show(exception.Message);
                return;
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
                return;
            }

            Files.Clear();
            CurrentDirectory = path;

            foreach (var file in listOfFiles)
            {
                Files.Add(file);
            }

            if (!isBack)
            {
                pathHistory.Push(path);
            }
        }

        public void DownloadFiles(string pathToFile)
        {
            var listOfFiles = new List<string>();
            ServerInfo serverInfo = new ServerInfo(HostName, HostPort, PathToDownload);

            if (pathToFile != null)
            {
                new Task(async () => await Client.Get(pathToFile, serverInfo)).Start();
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
                new Task(async () => await Client.Get(listOfFiles[j], serverInfo)).Start();
            }
        }
    }
}
