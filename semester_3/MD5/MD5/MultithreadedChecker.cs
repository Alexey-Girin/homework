using System;
using System.Collections.Concurrent;
using System.IO;
using System.Text;
using System.Threading;

namespace MD5
{
    public class MultithreadedChecker
    {
        private ConcurrentQueue<string> fileNamesQueue = new ConcurrentQueue<string>();

        private AutoResetEvent firstResetEvent = new AutoResetEvent(true);

        private AutoResetEvent secondResetEvent = new AutoResetEvent(true);

        private volatile string commonHash;

        private volatile int currentNumberOfExecutedThreads = 0;

        public string Check(string path)
        {
            commonHash = Path.GetDirectoryName(path);

            string[] fileNames;

            try
            {
                fileNames = Directory.GetDirectories(path);
            }
            catch (Exception exception)
            {
                throw new Exception(exception.Message);
            }

            Array.Sort(fileNames);

            foreach (var fileName in fileNames)
            {
                fileNamesQueue.Enqueue(path);
            }

            int totalNumber = fileNames.Length;

            Thread[] threads = new Thread[totalNumber];

            for (int i = 0; i < threads.Length; i++)
            {
                threads[i] = new Thread(new ThreadStart(ThreadMethod));
            }

            foreach(var thread in threads)
            {
                thread.Start();
            }

            foreach (var thread in threads)
            {
                thread.Join();
            }

            while (currentNumberOfExecutedThreads != totalNumber)
            {
                Thread.Sleep(1);
            }

            string hash = CalculateHash(commonHash);

            return commonHash;
        }

        private void ThreadMethod()
        {
            firstResetEvent.WaitOne();
            fileNamesQueue.TryDequeue(out string path);
            firstResetEvent.Set();

            if (IsDir(path))
            {
                var multithreadedChecker = new MultithreadedChecker();
                commonHash += multithreadedChecker.Check(path);
                return;
            }

            byte[] content = null;

            using (FileStream fileStream = File.OpenRead(path))
            {
                content = new byte[fileStream.Length];
                fileStream.Read(content, 0, content.Length);
            }

            string hash = CalculateHash(content);

            secondResetEvent.Set();
            commonHash += hash;
            secondResetEvent.Set();

            currentNumberOfExecutedThreads++;
        }

        private string CalculateHash(byte[] content)
        {
            byte[] hashBytes = null;

            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                hashBytes = md5.ComputeHash(content);
            }

            return Encoding.Default.GetString(hashBytes);
        }

        private string CalculateHash(string contentString)
        {
            byte[] content = Encoding.Default.GetBytes(contentString);

            return CalculateHash(content);
        }

        private bool IsDir(string path) => Directory.Exists(path);
    }
}
