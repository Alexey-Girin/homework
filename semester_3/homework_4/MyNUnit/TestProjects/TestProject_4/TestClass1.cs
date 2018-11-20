using System;
using MyNUnit;

namespace TestProject_4
{
    public class TestClass1
    {
        [Test]
        public void TrueTest1()
        {
        }

        [Test]
        public void FalseTest1()
        {
            throw new Exception();
        }
    }
}
