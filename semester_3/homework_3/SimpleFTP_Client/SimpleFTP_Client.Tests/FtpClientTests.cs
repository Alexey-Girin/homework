using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleFTP_Server;
using SimpleFTP_Client.Exceptions;

namespace SimpleFTP_Client.Tests
{
    [TestClass]
    public class FtpClientTests
    {
        [TestMethod]
        [ExpectedException(typeof(ConnectException))]
        public void IfServerOffClientShouldNotCrash_List()
        {
            var client = new FtpClient("localhost", 8888);
            client.List(Directory.GetCurrentDirectory());
        }

        [TestMethod]
        [ExpectedException(typeof(ConnectException))]
        public void IfServerOffClientShouldNotCrash_Get()
        {
            var client = new FtpClient("localhost", 8888);
            client.Get(Directory.GetCurrentDirectory(), "");
        }

        [TestMethod]
        public void ListRequestShouldWorkCorrectly()
        {
            Directory.CreateDirectory(Directory.GetCurrentDirectory() +
                @"\TestData\directory_1");
            Directory.CreateDirectory(Directory.GetCurrentDirectory() +
                @"\TestData\directory_2");
            File.Create(Directory.GetCurrentDirectory() + @"\TestData\EmptyFile_1.txt");
            File.Create(Directory.GetCurrentDirectory() + @"\TestData\EmptyFile_2.txt");

            string path = Directory.GetCurrentDirectory() + @"\TestData";

            string[] fileNames = Directory.GetFiles(path);
            string[] subdirectoryNames = Directory.GetDirectories(path);
            string[] directoryObjectNames =
                new string[fileNames.Length + subdirectoryNames.Length];

            fileNames.CopyTo(directoryObjectNames, 0);
            subdirectoryNames.CopyTo(directoryObjectNames, fileNames.Length);

            Array.Sort(directoryObjectNames);

            var client = new FtpClient("localhost", 8888);
            var server = new FtpServer(8888, 100);

            server.Start();
            var fileStructs = client.List(path);
            server.Stop();

            fileStructs.Sort();

            Assert.AreEqual(directoryObjectNames.Length, fileStructs.Count);

            for (int i = 0; i < directoryObjectNames.Length; i++)
            {
                Assert.AreEqual(directoryObjectNames[i], fileStructs[i].Name);

                bool isDir = Directory.Exists(directoryObjectNames[i]);
                Assert.AreEqual(isDir, fileStructs[i].IsDirectory);
            }
        }

        [TestMethod]
        public void GetRequestShouldWorkCorrectly()
        {
            using (FileStream fileStream = File.OpenWrite(Directory.GetCurrentDirectory() +
                @"\TestData\GetTestData.txt"))
            {
                byte[] content = System.Text.Encoding.Default.GetBytes("Hello, World!");
                fileStream.Write(content, 0, content.Length);
            }

            File.Delete(Directory.GetCurrentDirectory() + @"\TestData\GetResult.txt");

            string path = Directory.GetCurrentDirectory() + @"\TestData\GetTestData.txt";
            string resultPath = Directory.GetCurrentDirectory() + @"\TestData\GetResult.txt";

            var client = new FtpClient("localhost", 8888);
            var server = new FtpServer(8888, 100);

            server.Start();
            client.Get(path, resultPath);
            server.Stop();

            byte[] trueResult = null;

            using (FileStream fileStream = File.OpenRead(path))
            {
                trueResult = new byte[fileStream.Length];
                fileStream.Read(trueResult, 0, trueResult.Length);
            }

            byte[] result = null;

            using (FileStream fileStream = File.OpenRead(resultPath))
            {
                result = new byte[fileStream.Length];
                fileStream.Read(result, 0, result.Length);
            }

            Assert.AreEqual(trueResult.Length, result.Length);

            for (int i = 0; i < trueResult.Length; i++)
            {
                Assert.AreEqual(trueResult[i], result[i]);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ServerErrorException))]
        public void ListShouldWorkWithNullPath()
        {
            var client = new FtpClient("localhost", 8888);
            var server = new FtpServer(8888, 100);

            server.Start();
            client.List(null);
            server.Stop();
        }

        [TestMethod]
        [ExpectedException(typeof(DirectoryNotExistException))]
        public void ListShouldWorkWithIncorrectPath()
        {
            var client = new FtpClient("localhost", 8888);
            var server = new FtpServer(8888, 100);

            string path = Directory.GetCurrentDirectory() + @"\ShouldBeDeleted";

            if (Directory.Exists(path))
            {
                Directory.Delete(path);
            }

            server.Start();
            client.List(path);
            server.Stop();
        }

        [TestMethod]
        [ExpectedException(typeof(ServerErrorException))]
        public void GetShouldWorkWithNullPath()
        {
            var client = new FtpClient("localhost", 8888);
            var server = new FtpServer(8888, 100);

            server.Start();
            client.Get(null, null);
            server.Stop();
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotExistException))]
        public void GetShouldWorkWithIncorrectPath()
        {
            var client = new FtpClient("localhost", 8888);
            var server = new FtpServer(8888, 100);

            string path = Directory.GetCurrentDirectory() + @"\ShouldBeDeleted.txt";

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            server.Start();
            client.Get(path, path);
            server.Stop();
        }
    }
}
