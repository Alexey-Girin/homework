using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MyThreadPool.Tests
{
    [TestClass]
    public class MyThreadPoolTests
    {
        [TestMethod]
        public void MyTreadPoolShouldSolveTasks()
        {
            const int numOfThreads = 4;
            const int numOfTasks = 16;

            MyThreadPool threadPool = new MyThreadPool(numOfThreads);
            IMyTask<int>[] tasks = new IMyTask<int>[numOfTasks];

            int var = 2;
            Func<int> func = new Func<int>(() => { return var * var; });
            int trueResult = var * var;

            for (int i = 0; i < numOfTasks; i++)
            {
                tasks[i] = threadPool.AddTask(func);
            }

            threadPool.Shutdown();

            for (int i = 0; i < numOfTasks; i++)
            {
                Assert.AreEqual(tasks[i].Result, trueResult);
                Assert.IsTrue(tasks[i].IsCompleted);
            }
        }

        [TestMethod]
        public void CheckCountOfActiveThreads()
        {
            const int numOfThreads = 100;
            MyThreadPool threadPool = new MyThreadPool(numOfThreads);

            Assert.AreEqual(threadPool.GetCountOfActiveThreads(), numOfThreads);

            Func<int> func = new Func<int>(() => { return 1; });
            const int numOfTasks = 100;

            for (int i = 0; i < numOfTasks; i++)
            {
                threadPool.AddTask(func);
            }

            Assert.AreEqual(threadPool.GetCountOfActiveThreads(), numOfThreads);

            threadPool.Shutdown();
            Thread.Sleep(10);
            Assert.AreEqual(threadPool.GetCountOfActiveThreads(), 0);
        }
    }
}
