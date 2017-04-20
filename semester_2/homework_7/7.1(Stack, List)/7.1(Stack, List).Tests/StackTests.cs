namespace _7thHomework.Task1.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class StackTests
    {
        private GenericStack<int> stack;

        [TestInitialize]
        public void Initialize()
        {
            stack = new GenericStack<int>();
        }

        [TestMethod]
        public void PushTest()
        {
            stack.Push(1);
            Assert.IsTrue(stack.IsBelong(1));
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
        [ExpectedException(typeof(Exception))]
        public void PopFromEmptyStackTest()
        {
            stack.Pop();
        }
    }
}
