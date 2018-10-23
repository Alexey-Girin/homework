using System.IO;
using System.Net.Sockets;

namespace SimpleFTP_Client
{
    public class ReplyDataList
    {
        public string[] FileNames { get; private set; }
        public int Size { get; private set; }

        public ReplyDataList(string[] newFilesName)
        {
            FileNames = newFilesName;
            Size = FileNames.Length;
        }
    }

    public class ReplyDataGet
    {
        public byte[] Сontent { get; private set; }

        public ReplyDataGet(byte[] newContent)
        {
            Сontent = newContent;
        }
    }

    public class FtpClient
    {
        private TcpClient client;

        private StreamWriter streamWriter;
        private StreamReader streamReader;

        public string HostName { get; private set; }
        public int HostPort { get; private set; }

        public FtpClient(string host, int port)
        {
            HostName = host;
            HostPort = port;
        }

        public ReplyDataList List(string path)
        {
            client = new TcpClient(HostName, HostPort);

            string request = '1' + path;

            streamWriter = new StreamWriter(client.GetStream());
            streamReader = new StreamReader(client.GetStream());

            streamWriter.WriteLine(request);
            streamWriter.Flush();

            int size = int.Parse(streamReader.ReadLine());

            string[] FileNames = new string[size];

            for (int i = 0; i < size; i++)
            {
                FileNames[i]= streamReader.ReadLine();
            }

            client.Close();

            return new ReplyDataList(FileNames);
        }

        public ReplyDataGet Get(string path)
        {
            client = new TcpClient(HostName, HostPort);

            string request = '2' + path;

            streamWriter = new StreamWriter(client.GetStream());
            streamReader = new StreamReader(client.GetStream());

            streamWriter.WriteLine(request);
            streamWriter.Flush();

            long size = long.Parse(streamReader.ReadLine());
            byte[] fileContent = new byte[size];

            client.GetStream().Read(fileContent, 0, fileContent.Length);

            client.Close();

            return new ReplyDataGet(fileContent);
        }
    }
}
