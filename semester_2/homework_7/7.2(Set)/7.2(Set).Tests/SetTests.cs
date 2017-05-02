namespace SetNamespace.Tests
{
    using ListNamespace;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class SetTests
    {
        [TestInitialize]
        public void Initialize()
        {
            set = new Set<int>();
        }

        [TestMethod]
        public void AddTest()
        {
            set.Add(1);
            Assert.IsTrue(set.IsBelong(1));
        }

        [TestMethod]
        [ExpectedException(typeof(AddSetExeption))]
        public void AddExistentTest()
        {
            set.Add(1);
            set.Add(1);
        }

        [TestMethod]
        public void DeleteTest()
        {
            set.Add(1);
            set.Delete(1);
            Assert.IsFalse(set.IsBelong(1));
        }

        [TestMethod]
        [ExpectedException(typeof(DeleteExeption))]
        public void DeleteNonexistentTest()
        {
            set.Add(1);
            set.Delete(2);
        }

        [TestMethod]
        public void UnionTest1()
        {
            var secondSet = new Set<int>();
            secondSet.Add(1);
            secondSet.Add(3);

            set.Add(2);
            set.Add(4);
            set.Add(1);

            set.Union(secondSet);

            var resultSet = new Set<int>();
            resultSet.Add(1);
            resultSet.Add(2);
            resultSet.Add(3);
            resultSet.Add(4);

            Assert.IsTrue(set.AreEqual(resultSet));
        }

        [TestMethod]
        public void UnionTest2()
        {
            var secondSet = new Set<int>();

            set.Add(1);
            set.Add(2);

            set.Union(secondSet);

            var resultSet = new Set<int>();
            resultSet.Add(1);
            resultSet.Add(2);

            Assert.IsTrue(set.AreEqual(resultSet));
        }

        [TestMethod]
        public void IntersectionTest1()
        {
            var secondSet = new Set<int>();

            set.Add(1);
            set.Add(2);

            set.Intersection(secondSet);

            var resultSet = new Set<int>();

            Assert.IsTrue(set.AreEqual(resultSet));
        }

        [TestMethod]
        public void IntersectionTest2()
        {
            var secondSet = new Set<int>();
            secondSet.Add(1);
            secondSet.Add(3);

            set.Add(1);
            set.Add(2);

            set.Intersection(secondSet);

            var resultSet = new Set<int>();
            resultSet.Add(1);

            Assert.IsTrue(set.AreEqual(resultSet));
        }

        Set<int> set;
    }
}
