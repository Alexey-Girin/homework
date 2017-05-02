namespace ListNamespace.Tests
{   
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ListEnumeratorTests
    {
        [TestMethod]
        public void ListEnumeratorTest()
        {
            var list = new List<int>();
            var resultList = new List<int>();

            list.Add(1, 0);
            list.Add(2, 1);
            list.Add(3, 2);

            resultList.Add(1, 0);
            resultList.Add(2, 1);
            resultList.Add(3, 2);

            foreach (int element in list)
            {
                resultList.Delete(element);
            }

            Assert.AreEqual(0, resultList.Size);
        }
    }
}
