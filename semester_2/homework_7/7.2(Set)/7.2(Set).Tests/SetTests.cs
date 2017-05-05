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
            firstSet = new Set<int>();
            secondSet = new Set<int>();

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
            firstSet.Add(1);
            firstSet.Add(2);

            secondSet.Add(2);
            secondSet.Add(3);

            var trueResultSet = new Set<int>();
            trueResultSet.Add(1);
            trueResultSet.Add(2);
            trueResultSet.Add(3);

            var resultSet = SetOperations<int>.Union(firstSet, secondSet);
            Assert.IsTrue(SetOperations<int>.AreEqual(trueResultSet, resultSet));
        }

        [TestMethod]
        public void UnionTest2()
        {
            firstSet.Add(1);
            firstSet.Add(2);

            var trueResultSet = new Set<int>();
            trueResultSet.Add(1);
            trueResultSet.Add(2);

            var resultSet = SetOperations<int>.Union(firstSet, secondSet);
            Assert.IsTrue(SetOperations<int>.AreEqual(trueResultSet, resultSet));
        }

        [TestMethod]
        public void IntersectionTest1()
        {
            firstSet.Add(1);
            firstSet.Add(2);

            secondSet.Add(2);
            secondSet.Add(3);

            var trueResultSet = new Set<int>();
            trueResultSet.Add(2);

            var resultSet = SetOperations<int>.Intersection(firstSet, secondSet);
            Assert.IsTrue(SetOperations<int>.AreEqual(trueResultSet, resultSet));
        }

        [TestMethod]
        public void IntersectionTest2()
        {
            firstSet.Add(1);
            firstSet.Add(2);

            secondSet.Add(3);
            secondSet.Add(4);

            var trueResultSet = new Set<int>();

            var resultSet = SetOperations<int>.Intersection(firstSet, secondSet);
            Assert.IsTrue(SetOperations<int>.AreEqual(trueResultSet, resultSet));
        }

        private Set<int> set;
        private Set<int> firstSet;
        private Set<int> secondSet;
    }
}
