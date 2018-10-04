namespace Lazy.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class LazyTests
    {
        [TestMethod]
        public void SingleThreadedLazyShouldCalculateOnce()
        {
            int counter = 0;
            var func = new Func<int>(() => 
            {
                counter++;
                return 1;
            });

            var lazy = LazyFactory.CreateSingleThreadedLazy(func);
     
            const int trueResult = 1;
            const int num = 100;
            for(int i = 0; i < num; i++)
            {
                Assert.AreEqual(trueResult, lazy.Get());
                Assert.AreEqual(trueResult, counter);
            }
        }

        [TestMethod]
        public void SingleThreadedLazyShouldReturnNull()
        {
            var func = new Func<object>(() => { return null; });
            var lazy = LazyFactory.CreateSingleThreadedLazy(func);

            Assert.IsNull(lazy.Get());
        }

        [TestMethod]
        public void SingleThreadedLazyShouldReturnCorrectValue()
        {
            int n = 2;
            var func = new Func<int>(() => n * n);
            var lazy = LazyFactory.CreateSingleThreadedLazy(func);

            const int trueResult = 4;
            Assert.AreEqual(trueResult, lazy.Get());
        }
    }
}
