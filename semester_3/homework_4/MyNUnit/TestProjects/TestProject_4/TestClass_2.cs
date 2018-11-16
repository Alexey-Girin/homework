using System;
using MyNUnit;

namespace TestProject_4
{
    public class TestClass_2
    {
        [Test]
        public void TestMethod_0()
        {
        }

        [Test]
        public void TestMethod_1()
        {
        }

        [Test(Ignore = "reason")]
        public void TestMethod_2()
        {
        }

        [Test(Ignore = "reason")]
        public void TestMethod_3()
        {
        }
        
        [Test]
        public void TestMethod_4()
        {
            throw new Exception();
        }

        [Test]
        public void TestMethod_5()
        {
            throw new Exception();
        }
    }
}
