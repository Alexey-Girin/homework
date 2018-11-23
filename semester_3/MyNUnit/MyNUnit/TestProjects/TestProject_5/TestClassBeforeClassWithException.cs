using MyNUnit;
using System;

namespace TestProject_5
{
    public class TestClassBeforeClassWithException
    {
        [BeforeClass]
        public static void BeforeClassMethod()
        {
            throw new AggregateException();
        }

        [Test]
        public void IndefiniteTest0()
        {
        }

        [Test]
        public void IndefiniteTest1()
        {
            throw new Exception();
        }

        [Test(Ignore = "reason")]
        public void IndefiniteTest2()
        {
        }
    }
}
