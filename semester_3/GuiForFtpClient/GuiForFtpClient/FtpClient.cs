using System;
using System.IO;
using System.Net.Sockets;
using SimpleFtpClient.Exceptions;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SimpleFtpClient
{
    /// <summary>
    /// FTP-клиент.
    /// </summary>
    public class FtpClient
    {
        /// <summary>
        /// Коллекция скачиваемых файлов.
        /// </summary>
        public ObservableCollection<string> FilesWhichDownloadingNow { get; } =
            new ObservableCollection<string>();

        /// <summary>
        /// Коллекция скачанных файлов.
        /// </summary>
        public ObservableCollection<string> FilesWhichDownloaded { get; } =
            new ObservableCollection<string>();

        /// <summary>
        /// Коллекция ошибок, возникших при скачивании файлов.
        /// </summary>
        public ObservableCollection<FileDownloadError> DownloadErrors { get; } =
            new ObservableCollection<FileDownloadError>();

        /// <summary>
        /// Удаление информации о результатах скачивания.
        /// </summary>
        public void Reset()
        {
            FilesWhichDownloaded.Clear();
            DownloadErrors.Clear();
        }

        /// <summary>
        /// Запрос на листинг файлов и папок в директории на сервере.
        /// </summary>
        /// <param name="path">Путь к директории на сервере.</param>
        /// <param name="serverInfo">Информация, необходимая для подключения к серверу.</param>
        /// <returns>Информация о файлах и папках.</returns>
        public async Task<List<FileInfo>> List(string path, ServerInfo serverInfo)
        {
            TcpClient client = null;
            const char request = '1';

            try
            {
                client = await Task.Factory.StartNew(()
                    => new TcpClient(serverInfo.HostName, serverInfo.HostPort));
            }
            catch (SocketException exception)
            {
                throw new ConnectException("Не удалось подключиться к серверу", exception);
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
            catch (ArgumentNullException exception)
            {
                client.Close();
                throw new ServerErrorException("Не удалось выполнить запрос", exception);
            }

            if (size == -1)
            {
                client.Close();
                throw new DirectoryNotExistException("Не удалось выполнить запрос");
            }

            var listOfFiles = new List<FileInfo>();

            for (int i = 0; i < size; i++)
            {
                string fileName = await streamReader.ReadLineAsync();
                bool IsDir = "True" == await streamReader.ReadLineAsync();
                listOfFiles.Add(new FileInfo(fileName, IsDir));
            }

            client.Close();
            return listOfFiles;
        }

        /// <summary>
        /// Запрос на скачивание файла с сервера.
        /// </summary>
        /// <param name="fileInfo">Информация о файле.</param>
        /// <param name="serverInfo">Информация, необходимая для скачивания файла.</param>
        public async Task Get(FileInfo fileInfo, ServerInfo serverInfo)
        {
            TcpClient client = null;
            const char request = '2';

            try
            {
                client = new TcpClient(serverInfo.HostName, serverInfo.HostPort);
            }
            catch (SocketException exception)
            {
                await serverInfo.Dispatcher.InvokeAsync(()
                    => DownloadErrors.Add(new FileDownloadError(exception,
                        $"Не удалось подключиться к серверу. Файл: {fileInfo.Path}")));
                return;
            }

            var streamWriter = new StreamWriter(client.GetStream()) { AutoFlush = true };
            var streamReader = new StreamReader(client.GetStream());

            await streamWriter.WriteAsync(request);
            await streamWriter.WriteLineAsync(fileInfo.Path);

            long size = -1;

            try
            {
                size = long.Parse(streamReader.ReadLine());
            }
            catch (ArgumentNullException exception)
            {
                client.Close();
                await serverInfo.Dispatcher.InvokeAsync(()
                    => DownloadErrors.Add(new FileDownloadError(exception,
                        $"Не удалось выполнить запрос для {fileInfo.Path}")));
                return;
            }

            if (size == -1)
            {
                client.Close();
                await serverInfo.Dispatcher.InvokeAsync(()
                    => DownloadErrors.Add(new FileDownloadError(null,
                        $"Не удалось выполнить запрос для {fileInfo.Path}")));
                return;
            }

            await DownloadFile(serverInfo, client, fileInfo);

            client.Close();
        }

        /// <summary>
        /// Скачивание файла.
        /// </summary>
        /// <param name="pathToDownload">Путь к месту скачивания файла.</param>
        /// <param name="client">Клиент.</param>
        /// <param name="fileInfo">Информация, необходимая для скачивания файла.</param>
        private async Task DownloadFile(
            ServerInfo serverInfo,
            TcpClient client,
            FileInfo fileInfo)
        {
            FileStream fileStream = null;
            string pathToDownload = serverInfo.PathToDownload;

            if (pathToDownload != string.Empty)
            {
                pathToDownload = $"{serverInfo.PathToDownload}\\{fileInfo.FileName}";
            }

            try
            {
                fileStream = File.OpenWrite(pathToDownload);
            }
            catch (Exception exception)
            {
                await serverInfo.Dispatcher.InvokeAsync(()
                    => DownloadErrors.Add(new FileDownloadError(exception,
                        $"Не удалось скачать файл - {fileInfo.Path}\n{exception.Message}")));
                return;
            }

            serverInfo.Dispatcher.Invoke(() => FilesWhichDownloadingNow.Add(fileInfo.Path));

            await client.GetStream().CopyToAsync(fileStream);
            await fileStream.FlushAsync();

            fileStream.Close();

            serverInfo.Dispatcher.Invoke(() => FilesWhichDownloadingNow.Remove(fileInfo.Path));
            serverInfo.Dispatcher.Invoke(() => FilesWhichDownloaded.Add(fileInfo.Path));
        }
    }
}
