namespace HashTableNamespace.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class HashTableTest
    {
        [TestInitialize]
        public void Initialize()
        {
            hashTable = new HashTable(new HashFunction_1());
        }

        [TestMethod]
        public void Add()
        {
            hashTable.Add("RZA");
            hashTable.Add("TechN9ne");
            hashTable.Add("Jay-Z");

            Assert.AreEqual(hashTable.IsBelong("TechN9ne"), true);
        }

        [TestMethod]
        public void Delete()
        {
            hashTable.Add("RZA");
            hashTable.Add("TechN9ne");
            hashTable.Add("Jay-Z");
            hashTable.Delete("TechN9ne");

            Assert.AreEqual(hashTable.IsBelong("TechN9ne"), false);
        }

        [TestMethod]
        public void DeleteEmpty()
        {
            try
            {
                hashTable.Delete("TechN9ne");
            }
            catch (Exception e)
            {
                Assert.AreEqual(e.Message, "значение не найдено");
            }
        }

        private HashTable hashTable;
    }
}
