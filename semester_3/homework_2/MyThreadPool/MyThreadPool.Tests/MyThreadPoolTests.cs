﻿using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MyThreadPool.Tests
{
    [TestClass]
    public class MyThreadPoolTests
    {
        [TestMethod]
        public void MyThreadPoolShouldSolveTasks()
        {
            const int numOfThreads = 4;
            const int numOfTasks = 16;

            var threadPool = new MyThreadPool(numOfThreads);
            var tasks = new IMyTask<int>[numOfTasks];

            int variable = 2;
            var func = new Func<int>(() => { return variable * variable; });
            int trueResult = variable * variable;

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
            var threadPool = new MyThreadPool(numOfThreads);

            Assert.AreEqual(threadPool.GetCountOfActiveThreads(), numOfThreads);

            var func = new Func<int>(() => { return 1; });
            const int numOfTasks = 100;

            for (int i = 0; i < numOfTasks; i++)
            {
                threadPool.AddTask(func);
            }

            Assert.AreEqual(threadPool.GetCountOfActiveThreads(), numOfThreads);

            threadPool.Shutdown();
            Thread.Sleep(50);
            Assert.AreEqual(threadPool.GetCountOfActiveThreads(), 0);
        }

        [TestMethod]
        [ExpectedException(typeof(AggregateException))]
        public void MyThreadPoolShouldWorkWithIncorrectFunction()
        {
            const int numOfThreads = 4;
            var threadPool = new MyThreadPool(numOfThreads);

            int variable = 2;
            Func<int> func = new Func<int>(() => { return variable / 0; });

            var result = threadPool.AddTask(func).Result;
            threadPool.Shutdown();
        }

        [TestMethod]
        public void CheckMethodContinueWith()
        {
            const int numOfThreads = 4;
            var threadPool = new MyThreadPool(numOfThreads);

            int variable = 1;
            Func<int> firstFunc = new Func<int>(() => { return variable + variable; });

            double func(int var) => Math.Sqrt(var);
            Func<int, double> secondFunc = func;

            var firstTask = threadPool.AddTask(firstFunc);
            var secondTask = firstTask.ContinueWith(secondFunc);

            double trueResult = Math.Sqrt(variable + variable);
            Assert.AreEqual(secondTask.Result, trueResult);
        }
    }
}
