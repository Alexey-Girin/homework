using System;
using MyNUnit;

namespace TestProject_3
{
    public class TestClass
    {
        [Test(Ignore = "reason0")]
        public void IgnoreTest0()
        {
        }

        [Test(Ignore = "reason1")]
        public void IgnoreTest1()
        {
            throw new Exception();
        }

        [Test]
        public void TrueTese()
        {
        }
    }
}
