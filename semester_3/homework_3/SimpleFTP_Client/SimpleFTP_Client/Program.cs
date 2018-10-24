namespace SimpleFTP_Client
{
    class Program
    {
        static void Main(string[] args)
        {
            FtpClient client = new FtpClient("127.0.0.1", 8888);
            string[] fileNames = client.List(@"C:\");
            foreach (var fileName in fileNames)
            {
                System.Console.WriteLine(fileName);
            }

            System.Console.WriteLine();

            System.Console.WriteLine(System.Text.Encoding.Default
                .GetString(client.Get(@"C:\Users\Алексей\Documents\1.txt")));
        }
    }
}
