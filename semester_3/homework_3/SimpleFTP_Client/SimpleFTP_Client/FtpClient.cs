using System.IO;
using System.Net.Sockets;

namespace SimpleFTP_Client
{
    public class ReplyData
    {
        public string[] FileNames { get; private set; }
        public int Size { get; private set; }

        public ReplyData(string[] newFilesName)
        {
            FileNames = newFilesName;
            Size = FileNames.Length;
        }
    }

    public class FtpClient
    {
        private TcpClient client;

        private StreamWriter streamWriter;
        private StreamReader streamReader;

        public FtpClient(string hostName, int port)
            => client = new TcpClient(hostName, port);

        public ReplyData List(string path)
        {
            streamWriter = new StreamWriter(client.GetStream());
            streamReader = new StreamReader(client.GetStream());

            string request = path;

            streamWriter.WriteLine(request);
            streamWriter.Flush();

            int size = int.Parse(streamReader.ReadLine());

            string[] FileNames = new string[size];

            for (int i = 0; i < size; i++)
            {
                FileNames[i]= streamReader.ReadLine();
            }

            streamReader.Close();
            streamWriter.Close();

            return new ReplyData(FileNames);
        }

        public void Get(string path)
        {
        }
    }
}
