using System.Windows.Threading;

namespace SimpleFtpClient
{
    /// <summary>
    /// Информация, необходимая для выполнения запросов Get и List в <see cref="FtpClient"/>.
    /// </summary>
    public class ServerInfo
    {
        /// <summary>
        /// Адрес сервера.
        /// </summary>
        public string HostName { get; }

        /// <summary>
        /// Порт, прослушиваемый сервером.
        /// </summary>
        public int HostPort { get; }

        /// <summary>
        /// Путь к месту скачивания файла.
        /// </summary>
        public string PathToDownload { get; } = null;

        /// <summary>
        /// GUI-тред.
        /// </summary>
        public Dispatcher Dispatcher { get; } = null;

        /// <summary>
        /// Конструктор экземпляра класса <see cref="ServerInfo"/>.
        /// </summary>
        /// <param name="hostName">Имя сервера.</param>
        /// <param name="hostPort">Порт, прослушиваемый сервером.</param>
        public ServerInfo(string hostName, string hostPort)
        {
            HostName = hostName;
            HostPort = int.Parse(hostPort);
        }

        /// <summary>
        /// Конструктор экземпляра класса <see cref="ServerInfo"/>.
        /// </summary>
        /// <param name="hostName">Имя сервера.</param>
        /// <param name="hostPort">Порт, прослушиваемый сервером.</param>
        /// <param name="pathToDownload">Путь к месту скачивания файла.</param>
        public ServerInfo(string hostName, string hostPort, string pathToDownload, Dispatcher dispatcher)
        {
            HostName = hostName;
            HostPort = int.Parse(hostPort);
            PathToDownload = pathToDownload;
            Dispatcher = dispatcher;
        }
    }
}
