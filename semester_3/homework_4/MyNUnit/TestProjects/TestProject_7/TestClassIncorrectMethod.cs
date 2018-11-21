using System;
using MyNUnit;

namespace TestProject_7
{
    public class TestClassIncorrectMethod
    {
        [After]
        public int AfterMethod(int a)
        {
            return 0;
        }

        [Test]
        public void IndefiniteTest0()
        {
        }

        [Test]
        public void IndefiniteTest1()
        {
            throw new Exception();
        }

        [Test(Ignore = "reason")]
        public void IndefiniteTest2()
        {
        }
    }
}
