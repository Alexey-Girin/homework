namespace SimpleFTP_Server
{
    class Program
    {
        static void Main(string[] args)
        {
            FtpServer server = new FtpServer(8888);
            server.Start();
            System.Threading.Thread.Sleep(100);
            server.Stop();
     
            System.Console.ReadKey();
        }
    }
}
