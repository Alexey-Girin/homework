using System;
using MyNUnit;

namespace TestProject_Attribute
{
    public class TestClass
    {
        public bool MethodExecutionCheck
        {
            get
            {
                return checker[0] && checker[1] && checker[2] && checker[3] && checker[4];
            }
        }

        private bool[] checker = new bool[5] { false, false, true, true, true };

        [Test]
        public void TestMethod_0()
        {
            checker[0] = true;
        }

        [Test]
        public void TestMethod_1()
        {
            checker[1] = true;
        }

        public void TestMethod_2()
        {
            checker[2] = false;
        }

        public void TestMethod_3()
        {
            checker[3] = false;
            throw new Exception();
        }

        [Obsolete("")]
        public void TestMethod_4()
        {
            checker[4] = false;
        }
    }
}
