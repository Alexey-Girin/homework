namespace Lazy.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class LazyTests
    {
        [TestMethod]
        public void SingleThreadedLazy_1()
        {
            var func = new Func<object>(() => { return null; });
            var lazy = LazyFactory.CreateSingleThreadedLazy(func);

            Assert.IsNull(lazy.Get());
        }

        [TestMethod]
        public void SingleThreadedLazy_2()
        {
            var func = new Func<int>(() => { return 1; });
            var lazy = LazyFactory.CreateSingleThreadedLazy(func);

            const int trueResult = 1;
            Assert.AreEqual(trueResult, lazy.Get());
        }

        [TestMethod]
        public void SingleThreadedLazy_3()
        {
            int n = 2;
            var func = new Func<int>(() => n * n);
            var lazy = LazyFactory.CreateSingleThreadedLazy(func);

            const int trueResult = 4;
            Assert.AreEqual(trueResult, lazy.Get());
        }

        [TestMethod]
        public void SingleThreadedLazy_4()
        {
            var func = new Func<int>(() => { return 1; });
            var lazy = LazyFactory.CreateSingleThreadedLazy(func);
            var firstResult = lazy.Get();
            var secondResult = lazy.Get();

            Assert.AreEqual(firstResult, secondResult);
        }
    }
}
