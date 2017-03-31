namespace ListNamespace.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ListTest
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
        public void DeleteExistingElement()
        {
            list.Add(1);
            list.Delete(1);
            Assert.IsTrue(list.IsEmpty());
        }

        [TestMethod]
        public void DeleteNonexistentElement()
        {
            list.Add(1);
            list.Delete(2);
            Assert.IsFalse(list.IsEmpty());
        }

        [TestMethod]
        public void DeleteSome()
        {
            list.Add(1);
            list.Add(2);
            list.Delete(1);
            list.Delete(2);
            Assert.IsTrue(list.IsEmpty());
        }

        [TestMethod]
        public void DeleteElementFromEmptyList()
        {
            list.Delete(0);
        }

        [TestMethod]
        public void FindExistentElement()
        {
            list.Add(1);
            list.Add(2);
            list.Add(3);
            list.Add(4);
            Assert.IsTrue(list.IsBelong(2));
        }

        [TestMethod]
        public void FindNonexistentElement()
        {
            list.Add(1);
            list.Add(3);
            list.Add(4);
            Assert.IsFalse(list.IsBelong(2));
        }

        private List list;
    }
}
