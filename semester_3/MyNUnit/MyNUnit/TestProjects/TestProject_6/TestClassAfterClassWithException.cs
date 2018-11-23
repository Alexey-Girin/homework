using MyNUnit;
using System;

namespace TestProject_6
{
    public class TestClassAfterClassWithException
    {
        [AfterClass]
        public static void AfterClassMethod()
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
