using System;
using MyNUnit;

namespace TestProject_7
{
    public class TestClassIncorrectTest
    {
        [Test]
        public void IndefiniteTest3(int a)
        {
        }

        [Test]
        public int IndefiniteTest4()
        {
            return 0;
        }

        [Test]
        public void IndefiniteTest5(int a)
        {
            throw new Exception();
        }

        [Test(Ignore = "reason")]
        public void IndefiniteTest6(int a)
        {
        }
    }
}
