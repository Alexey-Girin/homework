using System;
using MyNUnit;

namespace TestProject_6
{
    public class TestClassAfter
    {
        private bool isTestExecuted = false;

        [Test]
        public void TrueTest()
        {
            isTestExecuted = true;
        }

        [After]
        public void AfterMethod()
        {
            if (!isTestExecuted)
            {
                throw new Exception();
            }
        }
    }
}
