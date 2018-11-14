using MyNUnit;
using System;

namespace TestingProject
{
    public class TestingClass
    {
        public int A { get; set; } = 0;

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
            System.Threading.Thread.Sleep(341);
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

        [Test]
        public void TestMethod5()
        {
            for (int i =0; i < 100000; i++)
            {
                for (int j = 0; j < 100; j++)
                {
                    int a = 1;
                    a = (int)Math.Asin(a * 1.1);
                }
            }
        }

        [BeforeClass]
        public void BeforeClassMethod()
        {
            A++;
        }

        [AfterClass]
        public void AfterClassMethod()
        {
            A--;
        }

        [AfterClass]
        public void AfterClassMethod1()
        {
            A++;
        }

        [AfterClass]
        public void AfterClassMethod2()
        {
            A++;
        }

        [BeforeClass]
        public void BeforeClassMethod1()
        {
            A++;
        }

        [After]
        public void AfterMethod1()
        {
           // throw new Exception("12345");
        }

        [After]
        public void AfterMethod2()
        {
        }
    }
}
