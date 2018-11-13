using MyNUnit;
using System;

namespace TestingProject
{
    public class TestingClass
    {
        [Test]
        public void TestMethod()
        {
            throw new System.Exception("12345");
        }

        [Test(Excepted = typeof(NullReferenceException))]
        public void TestMethod1()
        {
            int[] mas = null;
            int a = mas[0];
        }

        [Test]
        public void TestMethod2()
        {
            System.Threading.Thread.Sleep(1000);
        }

        [Test(Excepted = typeof(Exception), Ignore = "12345")]
        public void TestMethod3()
        {
            throw new System.Exception("12345");
        }

        [Test(Excepted = typeof(Exception))]
        public void TestMethod4()
        {
        }
    }
}
