namespace StackCalculator.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class StackOnListTest
    {
        [TestInitialize]
        public void Initialize()
        {
            stack = new StackOnList();
        }

        [TestMethod]
        public void PushTest()
        {
            stack.Push(1);
            Assert.IsFalse(stack.IsEmpty());
        }

        [TestMethod]
        public void PopTest()
        {
            stack.Push(1);
            Assert.AreEqual(1, stack.Pop());
        }

        [TestMethod]
        public void PeekTest()
        {
            stack.Push(1);
            Assert.AreEqual(1, stack.Peek());
        }

        [TestMethod]
        public void TwoElementsPopTest()
        {
            stack.Push(1);
            stack.Push(2);
            Assert.AreEqual(2, stack.Pop());
            Assert.AreEqual(1, stack.Pop());
        }

        [TestMethod]
        [ExpectedException(typeof(EmptyStackExeption))]
        public void PopFromEmptyStackTest()
        {
            stack.Pop();
        }

        private IStack stack;
    }
}

