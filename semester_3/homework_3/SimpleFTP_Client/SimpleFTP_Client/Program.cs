using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace SimpleFTP_Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Client client = new Client("127.0.0.1", 8889);
            client.List("hello, server!");
        }
    }
}
