using MyNUnit;

namespace TestProject_1
{
    public class TestClass
    {
        public bool MethodExecutionCheck
        {
            get
            {
                return checker[0] && checker[1] && checker[2];
            }
        }

        private bool[] checker = new bool[3] { false, false, false };

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

        [Test]
        public void TestMethod_2()
        {
            checker[2] = true;
        }
    }
}