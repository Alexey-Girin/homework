using MyNUnit;
using System;

namespace TestProject_6
{
    public class TestClassAfterWithException
    {
        [After]
        public void AfterMethod()
        {
            throw new AggregateException();
        }

        [Test]
        public void IndefiniteTest3()
        {
        }

        [Test]
        public void IndefiniteTest4()
        {
            throw new Exception();
        }

        [Test(Ignore = "reason")]
        public void IndefiniteTest5()
        {
        }
    }
}
