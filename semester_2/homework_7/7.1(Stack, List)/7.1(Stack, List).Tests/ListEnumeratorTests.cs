namespace _7thHomework.Task1.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ListEnumeratorTests
    {
        [TestMethod]
        public void ListEnumeratorTest()
        {
            var resultList = new GenericList<int>();
            var list = new GenericList<int>();

            list.Add(1);
            list.Add(2);
            list.Add(3);

            resultList.Add(1);
            resultList.Add(2);
            resultList.Add(3);

            foreach (var element in list)
            {
                resultList.Delete(element);
            }

            Assert.IsTrue(resultList.IsEmpty());
        }
    }
}
