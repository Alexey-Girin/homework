using MyNUnit;
using System;

namespace TestProject_5
{
    public class TestClassBeforeClass
    {
        private bool isBeforeClassMethodExecuted = false;
        private bool isBeforeMethodExecuted = false;

        [BeforeClass]
        public void BeforeClassMethod()
        {
            isBeforeClassMethodExecuted = true;
        }

        [Before]
        public void BeforeMethod()
        {
            if (isBeforeClassMethodExecuted)
            {
                isBeforeMethodExecuted = true;
            }
        }

        [Test]
        public void TrueTest()
        {
            if (!isBeforeMethodExecuted)
            {
                throw new Exception();
            }
        }
    }
}
