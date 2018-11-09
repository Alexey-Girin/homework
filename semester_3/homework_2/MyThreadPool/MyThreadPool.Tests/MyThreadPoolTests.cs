using System;
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
            int trueResult = variable * variable;

            for (int i = 0; i < numOfTasks; i++)
            {
                tasks[i] = threadPool.AddTask(() => variable * variable);
            }

            for (int i = 0; i < numOfTasks; i++)
            {
                Assert.AreEqual(trueResult, tasks[i].Result);
                Assert.IsTrue(tasks[i].IsCompleted);
            }
        }

        [TestMethod]
        public void CheckCountOfActiveThreads()
        {
            const int numOfThreads = 100;
            var threadPool = new MyThreadPool(numOfThreads);

            Assert.AreEqual(numOfThreads, threadPool.GetCountOfActiveThreads());

            const int numOfTasks = 100;

            for (int i = 0; i < numOfTasks; i++)
            {
                threadPool.AddTask(() => 1);
            }

            Assert.AreEqual(numOfThreads, threadPool.GetCountOfActiveThreads());

            threadPool.Shutdown();
            Assert.AreEqual(0, threadPool.GetCountOfActiveThreads());
        }

        [TestMethod]
        [ExpectedException(typeof(AggregateException))]
        public void MyThreadPoolShouldWorkWithIncorrectFunction()
        {
            const int numOfThreads = 4;
            var threadPool = new MyThreadPool(numOfThreads);

            int variable = 2;
            var result = threadPool.AddTask(() => variable / 0).Result;
        }

        [TestMethod]
        public void CheckMethodContinueWith()
        {
            const int numOfThreads = 4;
            var threadPool = new MyThreadPool(numOfThreads);

            int variable = 1;
            var firstTask = threadPool.AddTask(() => variable + variable);
            var secondTask = firstTask.ContinueWith((int var) => Math.Sqrt(var));

            double trueResult = Math.Sqrt(variable + variable);
            Assert.AreEqual(trueResult, secondTask.Result);
        }

        [TestMethod]
        public void MyThreadPoolShouldWorkWithDifferentTypes()
        {
            const int numOfThreads = 4;
            var threadPool = new MyThreadPool(numOfThreads);

            int firstVariable = 1;
            var firstTask = threadPool.AddTask(() => firstVariable + firstVariable);

            string secondVariable = "1";
            var secondTask = threadPool.AddTask(() => secondVariable + secondVariable);

            const int firstTrueResult = 2;
            const string secondTrueResult = "11";

            Assert.AreEqual(firstTrueResult, firstTask.Result);
            Assert.AreEqual(secondTrueResult, secondTask.Result);
        }
    }
}
