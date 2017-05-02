namespace HashTableNamespace.Test
{
    using ListNamespace;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class HashTableTest
    {
        [TestInitialize]
        public void Initialize()
        {
            hashTable = new HashTable();
        }

        [TestMethod]
        public void AddOne()
        {
            hashTable.Add("halfwaytonowhere");
            Assert.IsTrue(hashTable.IsBelong("halfwaytonowhere"));
        }

        [TestMethod]
        public void AddSome1()
        {
            hashTable.Add("halfwaytonowhere");
            hashTable.Add("antananarivo");
            hashTable.Add("rgrrgeberwgowjeg");
            Assert.IsTrue(hashTable.IsBelong("antananarivo"));
        }

        [TestMethod]
        public void AddSome2()
        {
            hashTable.Add("halfwaytonowhere");
            hashTable.Add("antananarivo");
            hashTable.Add("rgrrgeberwgowjeg");
            hashTable.Add("halfwaytonowhere");
            hashTable.Add("antananarivo");
            hashTable.Add("rgrrgeberwgowjeg");
            Assert.IsTrue(hashTable.IsBelong("halfwaytonowhere"));
        }

        [TestMethod]
        public void AddSomeAndDelete()
        {
            hashTable.Add("halfwaytonowhere");
            hashTable.Add("antananarivo");
            hashTable.Add("rgrrgeberwgowjeg");
            hashTable.Delete("halfwaytonowhere");
            Assert.IsFalse(hashTable.IsBelong("halfwaytonowhere"));
        }

        [TestMethod]
        [ExpectedException(typeof(EmptyListExeption))]
        public void DeleteEmpty()
        {
           hashTable.Delete("halfwaytonowhere");
        }

        private HashTable hashTable;
    }
}
