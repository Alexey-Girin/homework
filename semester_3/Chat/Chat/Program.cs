using System;

namespace Chat
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("ошибка с параметрами");
                return;
            }

            if (args.Length == 2)
            {
                var client = new Client(args[0], int.Parse(args[1]));
                client.Connect();
                return;
            }
            else if (args.Length == 1)
            {
                var server = new Server(int.Parse(args[0]));
                server.Start();
                return;
            }

            Console.WriteLine("ошибка с параметрами");
        }
    }
}
