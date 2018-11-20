using MyNUnit;
using System;

namespace TestProject_1
{
    public class TestClass
    {
        [Test]
        public void TrueTest0()
        {
        }

        [Test(Expected = typeof(AggregateException))]
        public void TrueTest1()
        {
            throw new AggregateException("message");
        }
    }
}