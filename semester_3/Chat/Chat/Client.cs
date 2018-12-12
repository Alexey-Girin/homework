using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Chat
{
    /// <summary>
    /// Клиент чата.
    /// </summary>
    public class Client
    {
        /// <summary>
        /// TCP-клиент.
        /// </summary>
        private TcpClient client;

        /// <summary>
        /// Адрес сервера.
        /// </summary>
        private string name { get; }

        /// <summary>
        /// Прослушиваемый порт.
        /// </summary>
        private int port { get; }

        /// <summary>
        /// Объект, необходимый для прекращения работы клиента.
        /// </summary>
        private ManualResetEvent stopListening = new ManualResetEvent(false);

        /// <summary>
        /// Конструктор экземпляра класса <see cref="Client"/>.
        /// </summary>
        /// <param name="name">Адрес сервера.</param>
        /// <param name="port">Прослушиваемый порт.</param>
        public Client(string name, int port)
        {
            this.name = name;
            this.port = port;
        }

        /// <summary>
        /// Подключение к серверу. Начало работы.
        /// </summary>
        public void Connect()
        {
            try
            {
                client = new TcpClient(name, port);
            }
            catch (SocketException)
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
        /// Метод, осуществляющий передачу сообщений серверу.
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
                    stopListening.Set();
                    return;
                }
            }
        }

        /// <summary>
        /// Метод, осуществляющий получение сообщений от сервера.
        /// </summary>
        private async void Reader()
        {
            while (true)
            {
                var reader = new StreamReader(client.GetStream());
                string message = await reader.ReadLineAsync();
                Console.WriteLine($"собеседник: {message}");

                if (message == "exit")
                {
                    client.Close();
                    stopListening.Set();
                    return;
                }
            }
        }
    }
}
