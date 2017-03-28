namespace HashTableNamespace.Test
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using HashTableNamespace;

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
            Assert.AreEqual(hashTable.IsBelong("halfwaytonowhere"), false);
        }

        [TestMethod]
        public void DeleteEmpty()
        {
            try
            {
                hashTable.Delete("halfwaytonowhere");
            }
            catch(Exception e)
            {
                Assert.AreEqual(e.Message, "элемент не найден");
            }
        }

        private HashTable hashTable;
    }
}
