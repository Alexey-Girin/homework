using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SimpleFTP_Server
{
    class Program
    {
        static void Main(string[] args)
        {
            FtpServer server = new FtpServer(8888, 100);
            server.Start();

            System.Console.ReadKey();
        }
    }
}
