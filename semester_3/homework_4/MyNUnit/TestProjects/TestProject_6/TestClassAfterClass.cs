using System;
using MyNUnit;

namespace TestProject_6
{
    public class TestClass_Before2
    {
        private bool isTestExecuted = false;
        private bool isAfterMethodExecuted = false;

        [Test]
        public void TrueTest()
        {
            isTestExecuted = true;
        }

        [After]
        public void AfterMethod()
        {
            if (isTestExecuted)
            {
                isAfterMethodExecuted = true;
            }
        }

        [AfterClass]
        public void AfterClassMethod()
        {
            if (!isAfterMethodExecuted)
            {
                throw new Exception();
            }
        }
    }
}