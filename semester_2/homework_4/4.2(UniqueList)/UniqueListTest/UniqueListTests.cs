namespace ListNamespace.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class UniqueListTest
    {
        [TestInitialize]
        public void Initialize()
        {
            list = new UniqueList<int>();
        }

        [TestMethod]
        public void AddTest()
        {
            list.Add(1, 0);

            Assert.IsTrue(list.IsBelong(1));
        }

        [TestMethod]
        [ExpectedException(typeof(AddListException))]
        public void AddExistingValueTest()
        {
            list.Add(1, 0);
            list.Add(2, 1);
           
            list.Add(2, 2);
        }

        private UniqueList<int> list;
    }
}
