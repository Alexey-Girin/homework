namespace HashTableNamespace.Test
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using HashTableNamespace;

    [TestClass]
    public class ListTests
    {
        [TestInitialize]
        public void Initialize()
        {
            hashTable = new HashTable();
        }

        [TestMethod]
        public void AddTest()
        {
            hashTable.Add("halfwaytonowhere");
            Assert.AreEqual(hashTable.IsBelong("halfwaytonowhere"), true);
        }

        [TestMethod]
        public void AddTest2()
        {
            hashTable.Add("halfwaytonowhere");
            hashTable.Add("antananarivo");
            hashTable.Add("rgrrgeberwgowjeg");
            Assert.AreEqual(hashTable.IsBelong("antananarivo"), true);
        }

        [TestMethod]
        public void AddTest3()
        {
            hashTable.Add("halfwaytonowhere");
            hashTable.Add("antananarivo");
            hashTable.Add("rgrrgeberwgowjeg");
            Assert.AreEqual(hashTable.IsBelong("fegwgweg"), false);
        }

        [TestMethod]
        public void DeleteTest1()
        {
            hashTable.Add("halfwaytonowhere");
            hashTable.Add("antananarivo");
            hashTable.Add("rgrrgeberwgowjeg");
            hashTable.Delete("halfwaytonowhere");
            Assert.AreEqual(hashTable.IsBelong("halfwaytonowhere"), false);
        }

        [TestMethod]
        public void DeleteTest2()
        {
            hashTable.Delete("halfwaytonowhere");
        }

        private HashTable hashTable;
    }
}
