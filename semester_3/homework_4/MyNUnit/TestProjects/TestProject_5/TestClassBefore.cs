using System;
using MyNUnit;

namespace TestProject_5
{
    public class TestClassBefore
    {
        private bool isBeforeMethodExecuted = false;

        [Before]
        public void BeforeMethod()
        {
            isBeforeMethodExecuted = true;
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
