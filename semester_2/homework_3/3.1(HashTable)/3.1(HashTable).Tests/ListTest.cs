namespace HashTableNamespace.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

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
            list.Add("RZA");
            Assert.IsFalse(list.IsEmpty());
        }

        [TestMethod]
        public void DeleteTest1()
        {
            list.Add("RZA");
            list.Delete("RZA");
            Assert.AreEqual(list.IsEmpty(), true);
        }

        [TestMethod]
        public void DeleteTest2()
        {
            list.Add("RZA1");
            list.Delete("RZA2");
            Assert.AreEqual(list.IsEmpty(), false);
        }

        [TestMethod]
        public void DeleteTest3()
        {
            list.Add("RZA1");
            list.Add("RZA2");
            list.Delete("RZA1");
            list.Delete("RZA2");
            Assert.AreEqual(list.IsEmpty(), true);
        }

        [TestMethod]
        public void DeleteElementFromEmptyList()
        {
            try
            {
                list.Delete("RZA");
            }
            catch(Exception e)
            {
                Assert.AreEqual(e.Message, "значение не найдено");
            }
        }

        [TestMethod]
        public void FindTest1()
        {
            list.Add("RZA1");
            list.Add("RZA2");
            list.Add("RZA3");
            list.Add("RZA4");
            Assert.AreEqual(list.IsBelong("RZA2"), true);
        }

        [TestMethod]
        public void FindTest2()
        {
            list.Add("RZA1");
            list.Add("RZA3");
            list.Add("RZA4");
            Assert.AreEqual(list.IsBelong("RZA2"), false);
        }

        private List list;
    }
}
