using System;
using MyNUnit;

namespace TestProject_6
{
    public class TestClass_Before1
    {
        public bool MethodExecutionCheck
        {
            get
            {
                return counter[0] == 3 && counter[1] == 3;
            }
        }

        private int[] counter = new int[2] { 0, 0 };

        [Before]
        public void TestMethod_0()
        {
            counter[0]++;
        }

        [Before]
        public void TestMethod_1()
        {
            counter[1]++;
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
