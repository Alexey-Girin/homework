using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using SimpleFtpClient;
using SimpleFtpClient.Exceptions;
using GuiForFtpClient.Extentions;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace GuiForFtpClient
{
    /// <summary>
    /// View Model для FTP-клиента.
    /// </summary>
    public class ClientViewModel : DependencyObject, INotifyPropertyChanged
    {
        /// <summary>
        /// FTP-клиент.
        /// </summary>
        public FtpClient Client { get; } = new FtpClient();

        /// <summary>
        /// Коллекция файлов и папок.
        /// </summary>
        public ObservableCollection<FileInfo> Files { get; } =
            new ObservableCollection<FileInfo>();

        /// <summary>
        /// Коллекция скачиваемых файлов.
        /// </summary>
        public ObservableCollection<string> FilesWhichDownloadingNow { get; }

        /// <summary>
        /// Коллекция скачанных файлов.
        /// </summary>
        public ObservableCollection<string> FilesWhichDownloaded { get; }

        /// <summary>
        /// Коллекция ошибок, возникших при скачивании файлов.
        /// </summary>
        public ObservableCollection<FileDownloadError> DownloadErrors { get; } 

        /// <summary>
        /// Объект, необходимый для уведомления об изменении свойств. 
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Директория, на которую "смотрит" клиент.
        /// </summary>
        public string CurrentDirectory
        {
            get => currentDirectory;
            set
            {
                currentDirectory = value;
                PropertyChanged(this, new PropertyChangedEventArgs("CurrentDirectory"));
            }
        }

        private string currentDirectory;

        /// <summary>
        /// Адрес сервера.
        /// </summary>
        public string HostName
        {
            get => hostName;
            set
            {
                hostName = value;
                PropertyChanged(this, new PropertyChangedEventArgs("HostName"));
            }
        }

        private string hostName;

        /// <summary>
        /// Порт, прослушиваемый сервером.
        /// </summary>
        public string HostPort
        {
            get => hostPort;
            set
            {
                hostPort = value;
                PropertyChanged(this, new PropertyChangedEventArgs("HostPort"));
            }
        }

        private string hostPort;

        /// <summary>
        /// Путь к месту скачивания файлов.
        /// </summary>
        public string PathToDownload
        {
            get => pathToDownload;
            set
            {
                pathToDownload = value;
                PropertyChanged(this, new PropertyChangedEventArgs("PathToDownload"));
            }
        }

        private string pathToDownload;

        /// <summary>
        /// Стек родительских папок для папки, на которую "смотрит" клиент.
        /// </summary>
        private Stack<string> pathHistory = new Stack<string>();

        /// <summary>
        /// Конструктор экзмепляра класса <see cref="ClientViewModel"/>.
        /// </summary>
        public ClientViewModel()
        {
            FilesWhichDownloaded = Client.FilesWhichDownloaded;
            FilesWhichDownloadingNow = Client.FilesWhichDownloadingNow;
            DownloadErrors = Client.DownloadErrors;
        }

        /// <summary>
        /// Удаление информации о файлах и папках текущей коллекции.
        /// </summary>
        public void Reset()
        {
            Files.Clear();
            CurrentDirectory = null;
            Client.Reset();
        }

        /// <summary>
        /// Запрос на получение списка файлов и папок по заданному пути.
        /// </summary>
        /// <param name="path">Заданный путь.</param>
        public async Task GetDirectory(string path)
        {
            if (FilesWhichDownloadingNow.Count != 0)
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
                listOfFiles = await Client.List(path, new ServerInfo(HostName, HostPort, Dispatcher));
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

            Files.Update();
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

        /// <summary>
        /// Запрос на скачивание файла. Если параметр null, скачивание всех файлов в папке,
        /// на которую смотрит клиент.
        /// </summary>
        /// <param name="fileInfo">Информация о файле.</param>
        public void DownloadFiles(FileInfo fileInfo)
        {
            var listOfFiles = new List<FileInfo>();
            ServerInfo serverInfo = new ServerInfo(HostName, HostPort, PathToDownload, Dispatcher);

            if (fileInfo != null)
            {
                new Task(async () => await Client.Get(fileInfo, serverInfo)).Start();
                return;
            }

            foreach (var file in Files)
            {
                if (!file.IsDirectory)
                {
                    listOfFiles.Add(file);
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
