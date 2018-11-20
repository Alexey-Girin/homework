using System;
using MyNUnit;

namespace TestProject_2
{
    public class TestClass
    {
        [Test]
        public void FalseTest0()
        {
            throw new AggregateException("message");
        }

        [Test(Expected =typeof(Exception))]
        public void FalseTest1()
        {
            throw new AggregateException("message");
        }

        [Test(Expected = typeof(Exception))]
        public void FalseTest2()
        {
        }

        [Test]
        public void TrueTest()
        {
        }
    }
}
