namespace HashTableNamespace.Tests
{
    using ListNamespace;
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

            Assert.IsTrue(hashTable.IsBelong("TechN9ne"));
        }

        [TestMethod]
        public void Delete()
        {
            hashTable.Add("RZA");
            hashTable.Add("TechN9ne");
            hashTable.Add("Jay-Z");
            hashTable.Delete("TechN9ne");

            Assert.IsFalse(hashTable.IsBelong("TechN9ne"));
        }

        [TestMethod]
        [ExpectedException(typeof(EmptyListExeption))]
        public void DeleteEmpty()
        {
            hashTable.Delete("TechN9ne");
        }

        private HashTable hashTable;
    }
}
