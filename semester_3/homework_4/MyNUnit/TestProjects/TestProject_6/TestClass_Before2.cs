using System;
using MyNUnit;

namespace TestProject_6
{
    public class TestClass_Before2
    {
        [Before]
        public void TestMethod_0()
        {
            throw new Exception();
        }

        [Test]
        public void TestMethod_2()
        {
        }

        [Test]
        public void TestMethod_3()
        {
            throw new Exception();
        }

        [Test(Ignore = "reason")]
        public void TestMethod_4()
        {
        }
    }
}