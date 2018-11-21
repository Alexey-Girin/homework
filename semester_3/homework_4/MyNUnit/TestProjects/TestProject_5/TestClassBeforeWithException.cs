using MyNUnit;
using System;

namespace TestProject_5
{
    public class TestClassBeforeWithException
    {
        [Before]
        public void BeforeMethod()
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
