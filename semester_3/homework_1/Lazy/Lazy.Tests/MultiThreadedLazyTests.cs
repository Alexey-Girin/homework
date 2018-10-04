namespace Lazy.Tests
{
    using System;
    using System.Threading;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class MultiThreadedLazyTests
    {
        [TestMethod]
        public void MultiThreadedLazyShouldCalculateOnce()
        {
            int counter = 0;
            var func = new Func<int>(() =>
            {
                counter++;
                return 1;
            });

            var lazy = LazyFactory.CreateMultiThreadedLazy(func);
            var threads = new Thread[100];

            const int num = 100;

            for (int i = 0; i < threads.Length; i++)
            {
                threads[i] = new Thread(() =>
                {
                    for (int t = 0; t < num; t++)
                    {
                        lazy.Get();
                    }
                });
            }

            foreach (var thread in threads)
            {
                thread.Start();
            }
            foreach (var thread in threads)
            {
                thread.Join();
            }

            const int trueResult = 1;
            Assert.AreEqual(trueResult, counter);
        }

        [TestMethod]
        public void MultiThreadedLazyShouldReturnNull()
        {
            var func = new Func<object>(() => { return null; });
            var lazy = LazyFactory.CreateMultiThreadedLazy(func);
            var threads = new Thread[100];

            for (int i = 0; i < threads.Length; i++)
            {
                threads[i] = new Thread(() =>
                {
                    Assert.IsNull(lazy.Get());
                    Assert.IsNull(lazy.Get());
                });
            }

            foreach (var thread in threads)
            {
                thread.Start();
            }
            foreach (var thread in threads)
            {
                thread.Join();
            }
        }

        [TestMethod]
        public void MultiThreadedLazyShouldReturnCorrectValue()
        {
            var func = new Func<int>(() => { return 1; });
            var lazy = LazyFactory.CreateMultiThreadedLazy(func);
            var threads = new Thread[100];
            const int trueResult = 1;

            for (int i = 0; i < threads.Length; i++)
            {
                threads[i] = new Thread(() =>
                {
                    Assert.AreEqual(trueResult, lazy.Get());
                    Assert.AreEqual(trueResult, lazy.Get());
                });
            }

            foreach(var thread in threads)
            {
                thread.Start();
            }
            foreach (var thread in threads)
            {
                thread.Join();
            }
        }
    }
}

