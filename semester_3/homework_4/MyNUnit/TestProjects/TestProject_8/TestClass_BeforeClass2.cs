using System;
using MyNUnit;

namespace TestProject_8
{
    public class TestClass_BeforeClass2
    {
        [BeforeClass]
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
