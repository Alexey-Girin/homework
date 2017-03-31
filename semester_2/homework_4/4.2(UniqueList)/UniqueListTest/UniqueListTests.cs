namespace ListNamespace.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class UniqueListTest
    {
        [TestInitialize]
        public void Initialize()
        {
            list = new UniqueList();
        }

        [TestMethod]
        public void AddTest()
        {
            list.Add(1);
            Assert.IsTrue(list.IsBelong(1));
        }

        [TestMethod]
        public void AddExistingValueTest()
        {
            list.Add(1);
            list.Add(2);
            list.Add(3);
            list.Add(4);
           
            try
            {
                list.Add(2);
            }
            catch(AddListException e)
            {
                Assert.AreEqual(e.Message, "Попытка добавления существующего элемента");
            }
        }

        [TestMethod]
        public void DeleteExistingElementTest()
        {
            list.Add(1);
            list.Delete(1);
            Assert.IsTrue(list.IsEmpty());
        }

        [TestMethod]
        public void DeleteNonexistentElementTest()
        {
            list.Add(1);

            try
            {
                list.Delete(2);
            }
            catch (DeleteListException e)
            {
                Assert.AreEqual(e.Message, "Попытка удаления несуществующего элемента");
            }
        }

        [TestMethod]
        public void DeleteFromEmptyTest()
        {
            try
            {
                list.Delete(2);
            }
            catch (DeleteListException e)
            {
                Assert.AreEqual(e.Message, "Попытка удаления несуществующего элемента");
            }
        }

        private UniqueList list;
    }
}
