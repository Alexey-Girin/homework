using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Chat
{
    /// <summary>
    /// Сервер чата.
    /// </summary>
    public class Server
    {
        /// <summary>
        /// Прослушиваемый порт.
        /// </summary>
        private int port { get; }

        /// <summary>
        /// IP-адрес для прослушивания.
        /// </summary>
        private IPAddress ip { get; } = IPAddress.Any;

        /// <summary>
        /// Прослушивает входящие подключения.
        /// </summary>
        private TcpListener listener;

        /// <summary>
        /// Объект, необходимый для заверешния работы сервера.
        /// </summary>
        private ManualResetEvent stopListening = new ManualResetEvent(false);

        /// <summary>
        /// Подключенный клиент.
        /// </summary>
        private volatile TcpClient client;

        /// <summary>
        /// Конструктор экземпляра класса <see cref="Server"/>.
        /// </summary>
        /// <param name="port">Прослушиваемый порт.</param>
        public Server(int port)
        {
            this.port = port;
            listener = new TcpListener(ip, port);
        }

        /// <summary>
        /// Метод, запускающий сервер.
        /// </summary>
        public async void Start()
        {
            listener.Start();

            try
            {
                client = await listener.AcceptTcpClientAsync();
            }
            catch (InvalidOperationException)
            {
                Console.WriteLine("ошибка подключения");
                return;
            }

            var tasks = new Task[2];

            tasks[0] = Task.Factory.StartNew(() => Writer());
            tasks[1] = Task.Factory.StartNew(() => Reader());

            stopListening.WaitOne();
        }

        /// <summary>
        /// Метод, осуществляющий передачу сообщений клиенту.
        /// </summary>
        private async void Writer()
        {
            while (true)
            {
                var writer = new StreamWriter(client.GetStream()) { AutoFlush = true };
                string message = Console.ReadLine();
                await writer.WriteLineAsync(message);

                if (message == "exit")
                {
                    client.Close();
                    listener.Stop();
                    stopListening.Set();
                    return;
                }
            }
        }

        /// <summary>
        /// Метод, осуществляющий получение сообщений от клиента.
        /// </summary>
        private async void Reader()
        {
            while (true)
            {
                var reader = new StreamReader(client.GetStream());
                string message = await reader.ReadLineAsync();
                Console.WriteLine(message);

                if (message == "exit")
                {
                    client.Close();
                    listener.Stop();
                    stopListening.Set();
                    return;
                }
            }
        }
    }
}
