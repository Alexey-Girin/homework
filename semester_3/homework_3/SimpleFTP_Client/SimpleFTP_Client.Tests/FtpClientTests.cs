using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleFTP_Server;

namespace SimpleFTP_Client.Tests
{
    [TestClass]
    public class FtpClientTests
    {
        [TestMethod]
        public void IfServerOffClientShouldNotCrash()
        {
            var client = new FtpClient("localhost", 8888);

            string path = Directory.GetCurrentDirectory();
            string firstExceptionMessage = null;
            string secondExceptionMessage = null;

            try
            {
                client.List(path);
            }
            catch (Exception exception)
            {
                firstExceptionMessage = exception.Message;
            }

            try
            {
                client.Get(path, path);
            }
            catch (Exception exception)
            {
                secondExceptionMessage = exception.Message;
            }

            Assert.AreEqual(firstExceptionMessage, secondExceptionMessage, "ошибка подключения");
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
            var server = new FtpServer(8888);

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

            string path = Directory.GetCurrentDirectory() + 
                @"\TestData\GetTestData.txt";
            string resultPath = Directory.GetCurrentDirectory() + 
                @"\TestData\GetResult.txt";

            var client = new FtpClient("localhost", 8888);
            var server = new FtpServer(8888);

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
        public void ListShouldWorkWithIncorrectPath()
        {
            var client = new FtpClient("localhost", 8888);
            var server = new FtpServer(8888);

            string firstPath = Directory.GetCurrentDirectory() + @"\Teeest\";
            string secondPath = null;

            string firstExceptionMessage = null;
            string secondExceptionMessage = null;

            server.Start();

            try
            {
                client.List(firstPath);
            }
            catch(Exception exception)
            {
                firstExceptionMessage = exception.Message;
            }

            try
            {
                client.List(secondPath);
            }
            catch (Exception exception)
            {
                secondExceptionMessage = exception.Message;
            }

            server.Stop();

            Assert.AreEqual(firstExceptionMessage,
                "ошибка. директории не существует");
            Assert.AreEqual(secondExceptionMessage,
               "ошибка исполнения запроса сервером");
        }

        [TestMethod]
        public void GetShouldWorkWithIncorrectPath()
        {
            File.Delete(Directory.GetCurrentDirectory() + @"\1.txt");
            var client = new FtpClient("localhost", 8888);
            var server = new FtpServer(8888);

            string firstPath = Directory.GetCurrentDirectory() + @"\1.txt";
            string secondPath = null;

            string firstExceptionMessage = null;
            string secondExceptionMessage = null;

            server.Start();

            try
            {
                client.Get(firstPath, secondPath);
            }
            catch (Exception exception)
            {
                firstExceptionMessage = exception.Message;
            }

            try
            {
                client.Get(secondPath, firstPath);
            }
            catch (Exception exception)
            {
                secondExceptionMessage = exception.Message;
            }

            server.Stop();

            Assert.AreEqual(firstExceptionMessage,
                "ошибка. файла не существует");
            Assert.AreEqual(secondExceptionMessage,
               "ошибка исполнения запроса сервером");
        }
    }
}
