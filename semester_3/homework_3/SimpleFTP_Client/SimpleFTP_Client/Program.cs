namespace SimpleFTP_Client
{
    class Program
    {
        static void Main(string[] args)
        {
            FtpClient client = new FtpClient("127.0.0.1", 8888);

            var dataGet = client.Get(@"2C:\Users\Алексей\Documents\1.txt");
            System.Console.WriteLine(System.Text.Encoding.Default.GetString(dataGet.Сontent));

            System.Console.WriteLine();
           
            var dataList = client.List(@"1C:\Users\Алексей\Documents");
            foreach (var listName in dataList.FileNames)
            {
                System.Console.WriteLine(listName);
            }
        }
    }
}
