using System;
using System.IO;
using System.Net.Sockets;

namespace SimpleFTP_Client
{
    public class ReplyDataList
    {
        public int Size { get; }

        private readonly string[] fileNames;

        public string[] FileNames
        {
            get
            {
                if (Size != -1)
                {
                    return fileNames;
                }

                throw new Exception("ошибка. директория не существует");
            }
        }

        public ReplyDataList(string[] filesName)
        {
            fileNames = filesName;
            Size = FileNames.Length;
        }
    }

    public class ReplyDataGet
    {
        public long Size { get; }

        private readonly byte[] fileContent;

        public byte[] Сontent
        {
            get
            {
                if (Size != -1)
                {
                    return fileContent;
                }

                throw new Exception("ошибка. файл не существует");
            }
        }

        public ReplyDataGet(byte[] content, long size)
        {
            Size = size;
            fileContent = content;
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
            const int request = 1;

            client = new TcpClient(HostName, HostPort);

            streamWriter = new StreamWriter(client.GetStream()) { AutoFlush = true };
            streamReader = new StreamReader(client.GetStream());

            streamWriter.Write(request);
            streamWriter.WriteLine(path);

            int size = -1;

            try
            {
                size = int.Parse(streamReader.ReadLine());
            }
            catch (ArgumentNullException)
            {
                throw new Exception("ошибка исполнения запроса сервером");
            }

            string[] FileNames = null;

            if (size != -1)
            {
                FileNames = new string[size];

                for (int i = 0; i < size; i++)
                {
                    FileNames[i] = streamReader.ReadLine();
                }
            }

            client.Close();

            return new ReplyDataList(FileNames);
        }

        public ReplyDataGet Get(string path)
        {
            const int request = 2;

            client = new TcpClient(HostName, HostPort);

            streamWriter = new StreamWriter(client.GetStream()) { AutoFlush = true };
            streamReader = new StreamReader(client.GetStream());

            streamWriter.Write(request);
            streamWriter.WriteLine(path);

            long size = -1;

            try
            {
                size = long.Parse(streamReader.ReadLine());
            }
            catch (ArgumentNullException)
            {
                throw new Exception("ошибка исполнения запроса сервером");
            }

            byte[] fileContent = null;

            if (size != -1)
            {
                fileContent = new byte[size];
                client.GetStream().Read(fileContent, 0, fileContent.Length);
            }

            client.Close();

            return new ReplyDataGet(fileContent, size);
        }
    }
}
