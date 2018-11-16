using System;
using MyNUnit;

namespace TestProject_5
{
    public class TestClass_1
    {
        [Test(Expected = typeof(AccessViolationException))]
        public void TestMethod()
        {
            throw new AccessViolationException();
        }
    }
}
