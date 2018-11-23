using System;
using MyNUnit;

namespace TestProject_9
{
    public class TestClass_AfterClass2
    {
        [AfterClass]
        public void TestMethod_0()
        {
            throw new Exception();
        }

        [Test]
        public void TestMethod_1()
        {
        }

        [Test]
        public void TestMethod_2()
        {
            throw new Exception();
        }

        [Test(Ignore = "reason")]
        public void TestMethod_3()
        {
        }
    }
}
