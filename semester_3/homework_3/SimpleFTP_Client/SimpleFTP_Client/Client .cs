using System;
using System.Text;
using System.Net.Sockets;

namespace SimpleFTP_Client
{
    public class Client
    {
        private TcpClient client;
        private NetworkStream stream;

        public Client(string hostName, int port)
        {
            client = new TcpClient(hostName, port);
            stream = client.GetStream();
        }

        public void List(string path)
        {
            string request = path;

            byte[] data = Encoding.Unicode.GetBytes(request);
            stream.Write(data, 0, data.Length);

            byte[] answerData = new byte[256];
            int bytes = stream.Read(answerData, 0, answerData.Length);

            string answer = Encoding.Unicode.GetString(answerData, 0, bytes);
            Console.WriteLine(answer);
        }

        public void Get(string path)
        {
        }
    }
}
