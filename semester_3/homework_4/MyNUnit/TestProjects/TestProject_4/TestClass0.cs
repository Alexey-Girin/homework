using System;
using MyNUnit;

namespace TestProject_4
{
    public class TestClass0
    {
        [Test]
        public void TrueTest0()
        {
        }

        [Test]
        public void FalseTest0()
        {
            throw new Exception();
        }
    }
}
