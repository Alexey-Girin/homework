namespace _5thHomework.Task1.Tests
{
    using System;
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;


    [TestClass]
    public class FunctionsTests
    {
        Functions func;

        [TestInitialize]
        public void Initialize()
        {
            func = new Functions();
        }

        [TestMethod]
        public void MapTest()
        {
            List<int> resultTrue = new List<int>() { 2, 4, 6 };
            var result = func.Map(new List<int>() { 1, 2, 3 }, x => x * 2);

            Assert.AreEqual(resultTrue.Count, result.Count);

            for (int i = 0; i < resultTrue.Count; i++)
            {
                Assert.AreEqual(resultTrue[i], result[i]);
            }
        }

        [TestMethod]
        public void FilterTest()
        {
            List<int> resultTrue = new List<int>() { 2, 4, 6 };
             
            var result = func.Filter(new List<int>() { 2, 3, 4, 5, 6 }, x => x % 2 == 0);

            Assert.AreEqual(resultTrue.Count, result.Count);

            for (int i = 0; i < resultTrue.Count; i++)
            {
                Assert.AreEqual(resultTrue[i], result[i]);
            }
        }

        [TestMethod]
        public void FoldTest()
        {
            int resultTrue = 6;
            var result = func.Fold(new List<int>() { 1, 2, 3 }, 1, (acc, elem) => acc * elem);

            Assert.AreEqual(resultTrue, result);
        }
    }
}
