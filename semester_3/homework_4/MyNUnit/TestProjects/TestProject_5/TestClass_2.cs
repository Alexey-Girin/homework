using System;
using MyNUnit;

namespace TestProject_5
{
    public class TestClass_2
    {
        [Test(Expected = typeof(AccessViolationException))]
        public void TestMethod()
        {
        }
    }
}
