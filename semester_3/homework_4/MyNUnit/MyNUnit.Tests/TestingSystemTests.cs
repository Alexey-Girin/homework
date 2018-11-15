using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyNUnit.Exceptions;
using TestProject_1;

namespace MyNUnit.Tests
{
    [TestClass]
    public class TestingSystemTests
    {
        [TestMethod]
        [ExpectedException(typeof(PathErrorException))]
        public void TestingSystemShouldThrowExceptionWithNullPath()
        {
            TestingSystem.RunTests(null);
        }

        [TestMethod]
        public void Testing()
        {
            var a = TestingSystem.RunTests(@"C:\Users\Алексей\Desktop\homework\semester_3\homework_4\MyNUnit\TestProjects\TestProject_1\bin\Debug");
            Assert.AreEqual(a.TrueTestCollection.Count, 3);
        }
    }
}
