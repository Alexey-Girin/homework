using System;
using MyNUnit;

namespace TestProject_4
{
    public class TestClass_1
    {
        [Test]
        public void TestMethod_0()
        {
        }

        [Test(Ignore = "reason")]
        public void TestMethod_1()
        {
        }

        [Test]
        public void TestMethod_2()
        {
            throw new Exception();
        }
    }
}
