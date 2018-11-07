using System;
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
            Assert.AreEqual(threadPool.GetCountOfActiveThreads(), 0);
        }

        [TestMethod]
        [ExpectedException(typeof(AggregateException))]
        public void MyThreadPoolShouldWorkWithIncorrectFunction()
        {
            const int numOfThreads = 4;
            var threadPool = new MyThreadPool(numOfThreads);

            int variable = 2;
            var func = new Func<int>(() => { return variable / 0; });

            var result = threadPool.AddTask(func).Result;
        }

        [TestMethod]
        public void CheckMethodContinueWith()
        {
            const int numOfThreads = 4;
            var threadPool = new MyThreadPool(numOfThreads);

            int variable = 1;
            var firstFunc = new Func<int>(() => { return variable + variable; });

            double secondFunc(int var) => Math.Sqrt(var);

            var firstTask = threadPool.AddTask(firstFunc);
            var secondTask = firstTask.ContinueWith(secondFunc);

            double trueResult = Math.Sqrt(variable + variable);
            Assert.AreEqual(secondTask.Result, trueResult);
        }

        [TestMethod]
        public void MyThreadPoolShouldWorkWithDifferentTypes()
        {
            const int numOfThreads = 4;
            var threadPool = new MyThreadPool(numOfThreads);

            int firstVariable = 1;
            var firstFunc = new Func<int>(() => { return firstVariable + firstVariable; });
            var firstTask = threadPool.AddTask(firstFunc);

            string secondVariable = "1";
            var secondFunc = new Func<string>(() => { return secondVariable + secondVariable; });
            var secondTask = threadPool.AddTask(secondFunc);

            const int firstTrueResult = 2;
            const string secondTrueResult = "11";
            Assert.AreEqual(firstTask.Result, firstTrueResult);
            Assert.AreEqual(secondTask.Result, secondTrueResult);
        }
    }
}
