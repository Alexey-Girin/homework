﻿namespace SimpleFTP_Server
{
    class Program
    {
        static void Main(string[] args)
        {
            FtpServer server = new FtpServer(8888);
            server.Start();
        }
    }
}
