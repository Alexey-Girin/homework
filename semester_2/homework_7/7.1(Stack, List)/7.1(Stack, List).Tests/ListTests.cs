namespace _7thHomework.Task1.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ListTests
    {
        private GenericList<string> list;

        [TestInitialize]
        public void Initialize()
        {
            list = new GenericList<string>();
        }

        [TestMethod]
        public void AddTest()
        {
            list.Add("RZA");
            Assert.IsTrue(list.IsBelong("RZA"));
        }

        [TestMethod]
        public void DeleteTest()
        {
            list.Add("RZA");
            list.Delete("RZA");
            Assert.IsTrue(list.IsEmpty());
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void DeleteNonValueTest()
        {
            list.Add("RZA1");
            list.Delete("RZA2");
        }

        [TestMethod]
        public void DeleteSeveralTest()
        {
            list.Add("RZA1");
            list.Add("RZA2");
            list.Delete("RZA1");
            list.Delete("RZA2");
            Assert.IsTrue(list.IsEmpty());
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void DeleteElementFromEmptyList()
        {
            list.Delete("RZA");
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
            Assert.IsFalse(list.IsBelong("RZA2"));
        }
    }
}
