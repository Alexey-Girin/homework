namespace ListNamespace.Test
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using ListNamespace;

    [TestClass]
    public class ListTests
    {
        [TestInitialize]
        public void Initialize()
        {
            list = new List();
        }

        [TestMethod]
        public void AddTest()
        {
            list.Add(1);
            Assert.IsFalse(list.IsEmpty());
        }

        [TestMethod]
        public void DeleteTest1()
        {
            list.Add(1);
            list.Delete(1);
            Assert.AreEqual(list.IsEmpty(), true);
        }

        [TestMethod]
        public void DeleteTest2()
        {
            list.Add(1);
            list.Delete(2);
            Assert.AreEqual(list.IsEmpty(), false);
        }

        [TestMethod]
        public void DeleteTest3()
        {
            list.Add(1);
            list.Add(2);
            list.Delete(1);
            list.Delete(2);
            Assert.AreEqual(list.IsEmpty(), true);
        }

        [TestMethod]
        public void DeleteElementFromEmptyList()
        {
            list.Delete(0);
        }

        [TestMethod]
        public void FindTest1()
        {
            list.Add(1);
            list.Add(2);
            list.Add(3);
            list.Add(4);
            Assert.AreEqual(list.Find(2), true);
        }

        [TestMethod]
        public void FindTest2()
        {
            list.Add(1);
            list.Add(3);
            list.Add(4);
            Assert.AreEqual(list.Find(2), false);
        }

        private List list;
    }
}
