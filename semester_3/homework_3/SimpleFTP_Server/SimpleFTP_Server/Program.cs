using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace TcpListenerApp
{
    class Program
    {
        static void Main(string[] args)
        {
            const int port = 8889;
            IPAddress ip = IPAddress.Parse("127.0.0.1");

            TcpListener server = new TcpListener(ip, port);
            server.Start();
            Console.WriteLine("Слушаю...");

            while (true)
            {
                TcpClient client = server.AcceptTcpClient();
                NetworkStream stream = client.GetStream();

                byte[] data = new byte[256];
                stream.Read(data, 0, data.Length);

                string request = Encoding.Unicode.GetString(data);
                Console.WriteLine(request);
            }
        }
    }
}
