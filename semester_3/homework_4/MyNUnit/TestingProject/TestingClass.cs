using MyNUnit;

namespace TestingProject
{
    public class TestingClass
    {
        [Test]
        public void TestMethod()
        {
            throw new System.Exception("12345");
        }

        [Test]
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
    }
}
