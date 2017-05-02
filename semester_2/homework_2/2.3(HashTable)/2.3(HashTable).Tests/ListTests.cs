namespace ListNamespace.Test
{
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
        public void AddToBeginningOfEmptyListTest()
        {
            list.Add("0", 0);
            Assert.AreEqual("0", list.Get(0));
        }

        [TestMethod]
        public void AddToBeginningOfListTest()
        {
            list.Add("0", 0);
            list.Add("1", 0);
            Assert.AreEqual("1", list.Get(0));
        }

        [TestMethod]
        public void AddToEndOfListTest()
        {
            list.Add("0", 0);
            list.Add("1", 1);
            Assert.AreEqual("1", list.Get(1));
        }

        [TestMethod]
        public void AddTest()
        {
            list.Add("0", 0);
            list.Add("1", 1);
            list.Add("2", 1);
            Assert.AreEqual("2", list.Get(1));
            Assert.AreEqual("1", list.Get(2));
        }

        [TestMethod]
        [ExpectedException(typeof(ActionPositionExeption))]
        public void AddWithErrorPositionTest()
        {
            list.Add("0", 0);
            list.Add("2", 2);
        }

        [TestMethod]
        [ExpectedException(typeof(EmptyListExeption))]
        public void DeleteFromEmptyListTest()
        {
            list.Delete(0);
        }

        [TestMethod]
        [ExpectedException(typeof(EmptyListExeption))]
        public void DeleteFromBeginningOfListTest()
        {
            list.Add("0", 0);
            list.Delete(0);
            list.Get(0);
        }

        [TestMethod]
        [ExpectedException(typeof(ActionPositionExeption))]
        public void DeleteFromEndOfListTest()
        {
            list.Add("0", 0);
            list.Add("1", 1);
            list.Delete(1);
            list.Get(1);
        }

        [TestMethod]
        public void DeleteTest()
        {
            list.Add("0", 0);
            list.Add("1", 1);
            list.Add("2", 2);

            list.Delete(1);
            Assert.AreEqual("2", list.Get(1));
        }

        [TestMethod]
        [ExpectedException(typeof(ActionPositionExeption))]
        public void DeleteWithErrorPositionTest()
        {
            list.Add("0", 0);
            list.Delete(1);
        }

        [TestMethod]
        [ExpectedException(typeof(EmptyListExeption))]
        public void ChangeInEmptyListTest()
        {
            list.Change("0", 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ActionPositionExeption))]
        public void ChangeWithErrorPositionTest()
        {
            list.Add("0", 0);
            list.Change("0", 1);
        }

        [TestMethod]
        public void ChangeTest()
        {
            list.Add("0", 0);
            list.Change("1", 0);
            Assert.AreEqual("1", list.Get(0));
        }

        [TestMethod]
        [ExpectedException(typeof(EmptyListExeption))]
        public void GetFromEmptyListTest()
        {
            list.Get(0);
        }

        [TestMethod]
        [ExpectedException(typeof(ActionPositionExeption))]
        public void GetWithErrorPositionTest()
        {
            list.Add("0", 0);
            list.Get(1);
        }

        [TestMethod]
        public void GetTest()
        {
            list.Add("0", 0);
            Assert.AreEqual("0", list.Get(0));
        }

        [TestMethod]
        [ExpectedException(typeof(EmptyListExeption))]
        public void DeleteByValueTest()
        {
            list.Add("0", 0);
            list.Delete("0");
            list.Get(0);
        }

        [TestMethod]
        public void IsBelongTest()
        {
            list.Add("0", 0);
            Assert.IsTrue(list.IsBelong("0"));
        }

        private List list;
    }
}
