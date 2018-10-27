namespace SimpleFTP_Client
{
    using System;

    class Program
    {
        static void Main(string[] args)
        {
            FtpClient ftpClient = new FtpClient("localhost", 8888);
            var fileStructs = ftpClient.List(@"C:\");

            foreach (var fileStruct in fileStructs)
            {
                Console.WriteLine($"{fileStruct.IsDirectory}: {fileStruct.Name}");
            }
        }
    }
}
