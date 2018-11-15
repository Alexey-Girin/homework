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
        public void TestingSystemShouldTestAllTestMethods()
        {
            var path = $@"{GetTestProjectsPath()}\TestProjects\TestProject_1\bin\Debug";
            var testsExecutionInfo = TestingSystem.RunTests(path);

            Assert.AreEqual(1, testsExecutionInfo.Count);
            CheckTestCount(3, 0, 0, 0, testsExecutionInfo[0].TestsCountInfo);

            Assert.IsTrue((bool)testsExecutionInfo[0].Type.GetProperty("MethodExecutionCheck")
                .GetValue(testsExecutionInfo[0].InstanceOfType));
        }

        [TestMethod]
        public void CheckTestingSystemWithFalseTestMethods()
        {
            var path = $@"{GetTestProjectsPath()}\TestProjects\TestProject_2\bin\Debug";
            var resultList = TestingSystem.RunTests(path);

            Assert.AreEqual(1, resultList.Count);
            CheckTestCount(1, 1, 0, 0, resultList[0].TestsCountInfo);
        }

        [TestMethod]
        public void CheckTestingSystemWithIgnoreTestMethods()
        {
            var path = $@"{GetTestProjectsPath()}\TestProjects\TestProject_3\bin\Debug";
            var resultList = TestingSystem.RunTests(path);

            Assert.AreEqual(1, resultList.Count);
            CheckTestCount(1, 0, 2, 0, resultList[0].TestsCountInfo);

            Assert.IsTrue((bool)resultList[0].Type.GetProperty("MethodExecutionCheck")
                .GetValue(resultList[0].InstanceOfType));
        }

        [TestMethod]
        public void CheckTestingSystemWithWithSeveralTypes()
        {
            var path = $@"{GetTestProjectsPath()}\TestProjects\TestProject_4\bin\Debug";
            var resultList = TestingSystem.RunTests(path);

            Assert.AreEqual(2, resultList.Count);
            CheckTestCount(1, 1, 1, 0, resultList[0].TestsCountInfo);
            CheckTestCount(2, 2, 2, 0, resultList[1].TestsCountInfo);
        }

        [TestMethod]
        public void CheckTestingSystemWithExpectedExceptionTestMethods()
        {
            var path = $@"{GetTestProjectsPath()}\TestProjects\TestProject_5\bin\Debug";
            var resultList = TestingSystem.RunTests(path);

            Assert.AreEqual(2, resultList.Count);
            CheckTestCount(1, 0, 0, 0, resultList[0].TestsCountInfo);
            CheckTestCount(0, 1, 0, 0, resultList[1].TestsCountInfo);
        }

        [TestMethod]
        public void CheckTestingSystemWithBeforeMethods()
        {
            var path = $@"{GetTestProjectsPath()}\TestProjects\TestProject_6\bin\Debug";
            CheckTestingSystemWithAuxiliaryMethods(path);
        }

        [TestMethod]
        public void CheckTestingSystemWithAfterTestMethods()
        {
            var path = $@"{GetTestProjectsPath()}\TestProjects\TestProject_7\bin\Debug";
            CheckTestingSystemWithAuxiliaryMethods(path);
        }

        [TestMethod]
        public void CheckTestingSystemWithBeforeClassMethods()
        {
            var path = $@"{GetTestProjectsPath()}\TestProjects\TestProject_8\bin\Debug";
            CheckTestingSystemWithAuxiliaryMethods(path);
        }

        [TestMethod]
        public void CheckTestingSystemWithAfterClassMethods()
        {
            var path = $@"{GetTestProjectsPath()}\TestProjects\TestProject_9\bin\Debug";
            CheckTestingSystemWithAuxiliaryMethods(path);
        }

        private static string GetTestProjectsPath()
            => new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName;

        private static void CheckTestCount(int TrueTestCount, int FalseTestCount,
            int IgnoreTestCount, int IndefiniteTestCount,
            TestMethodsInTypeExecutionInfo.TestMethodsCountInfo testsCountInfo)
        {
            Assert.IsTrue(testsCountInfo.TrueTestCount == TrueTestCount);
            Assert.IsTrue(testsCountInfo.FalseTestCount == FalseTestCount);
            Assert.IsTrue(testsCountInfo.IgnoreTestCount == IgnoreTestCount);
            Assert.IsTrue(testsCountInfo.IndefiniteTestCount == IndefiniteTestCount);
        }

        private static void CheckTestingSystemWithAuxiliaryMethods(string path)
        {
            var resultList = TestingSystem.RunTests(path);

            Assert.AreEqual(2, resultList.Count);
            CheckTestCount(1, 1, 1, 0, resultList[0].TestsCountInfo);
            CheckTestCount(0, 0, 0, 3, resultList[1].TestsCountInfo);

            Assert.IsTrue((bool)resultList[0].Type.GetProperty("MethodExecutionCheck")
                .GetValue(resultList[0].InstanceOfType));
        }
    }
}
