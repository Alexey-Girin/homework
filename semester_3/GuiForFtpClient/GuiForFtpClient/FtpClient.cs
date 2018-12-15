using System;
using System.IO;
using System.Net.Sockets;
using SimpleFTP_Client.Exceptions;
using System.Collections.ObjectModel;
using System.Windows;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace SimpleFTP_Client
{
    public class FtpClient : DependencyObject
    {
        public ObservableCollection<string> FilesWhichDownloadNow { get; } =
            new ObservableCollection<string>();

        public ObservableCollection<string> FilesWhichDownloaded { get; } =
            new ObservableCollection<string>();

        public ObservableCollection<FileDownloadError> DownloadErrors { get; } =
           new ObservableCollection<FileDownloadError>();

        public void Reset()
        {
            FilesWhichDownloaded.Clear();
            DownloadErrors.Clear();
        }

        public async Task<List<FileInfo>> List(string path, ServerInfo serverInfo)
        {
            TcpClient client = null;
            const char request = '1';

            try
            {
                client = new TcpClient(serverInfo.HostName, serverInfo.HostPort);
            }
            catch (SocketException)
            {
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

            var Files = new List<FileInfo>() { new FileInfo("...", true) };

            for (int i = 0; i < size; i++)
            {
                string fileName = await streamReader.ReadLineAsync();
                bool IsDir = "True" == await streamReader.ReadLineAsync();
                Files.Add(new FileInfo(fileName, IsDir));
            }

            client.Close();
            return Files;
        }

        public async Task Get(string path, ServerInfo serverInfo)
        {
            TcpClient client = null;
            const char request = '2';

            try
            {
                client = new TcpClient(serverInfo.HostName, serverInfo.HostPort);
            }
            catch (SocketException exception)
            {
                await Dispatcher.InvokeAsync(()
                    => DownloadErrors.Add(new FileDownloadError(exception,
                        $"Не удалось подключиться к серверу. Файл: {path}")));
                return;
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
            catch (ArgumentNullException exception)
            {
                client.Close();
                await Dispatcher.InvokeAsync(()
                    => DownloadErrors.Add(new FileDownloadError(exception,
                        $"Не удалось выполнить запрос для {path}")));
                return;
            }

            if (size == -1)
            {
                client.Close();
                await Dispatcher.InvokeAsync(()
                    => DownloadErrors.Add(new FileDownloadError(null,
                        $"Не удалось выполнить запрос для {path}")));
                return;
            }

            await DownloadFile(serverInfo.PathToDownload, client, path);

            client.Close();
        }

        private async Task DownloadFile(string pathToDownload, TcpClient client, string path)
        {
            FileStream fileStream = null;

            if (pathToDownload != string.Empty)
            {
                pathToDownload = $"{pathToDownload}\\{GetFileName(path)}";
            }

            try
            {
                fileStream = File.OpenWrite(pathToDownload);
            }
            catch (Exception exception)
            {
                await Dispatcher.InvokeAsync(()
                    => DownloadErrors.Add(new FileDownloadError(exception,
                        $"Не удалось скачать файл - {path}\n{exception.Message}")));
                return;
            }

            Dispatcher.Invoke(() => FilesWhichDownloadNow.Add(path));

            await client.GetStream().CopyToAsync(fileStream);
            await fileStream.FlushAsync();

            fileStream.Close();

            Dispatcher.Invoke(() => FilesWhichDownloadNow.Remove(path));
            Dispatcher.Invoke(() => FilesWhichDownloaded.Add(path));
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
