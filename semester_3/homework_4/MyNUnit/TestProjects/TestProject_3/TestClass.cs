using MyNUnit;

namespace TestProject_3
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

        private bool[] checker = new bool[3] { false, true, true };

        [Test]
        public void TestMethod_0()
        {
            checker[0] = true;
        }

        [Test(Ignore = "reason")]
        public void TestMethod_1()
        {
            checker[1] = false;
        }

        [Test(Ignore = "reason")]
        public void TestMethod_2()
        {
            checker[2] = false;
        }
    }
}
