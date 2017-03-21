namespace SecondHomework.Task1.Tests
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using SecondHomework.Task1;

    [TestClass]
    public class StackTest
    {
        [TestMethod]
        public void PushTest()
        {
            var stack = new Stack();
            Assert.IsTrue(stack.IsEmpty());
            stack.Push(1);
            Assert.IsFalse(stack.IsEmpty());
        }
    }
}
