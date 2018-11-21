using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyNUnit.Exceptions;
using MyNUnit.SupportingClasses;
using System;

namespace MyNUnit.Tests
{
    [TestClass]
    public class TestingSystemTests
    {
        [TestMethod]
        [ExpectedException(typeof(PathErrorException))]
        public void TestingSystemShouldThrowExceptionWithNullPath()
        {
            TestingSystem.Launch(null);
        }

        [TestMethod]
        [ExpectedException(typeof(PathErrorException))]
        public void TestingSystemShouldThrowExceptionWithIncorrectPath()
        {
            TestingSystem.Launch(" ");
        }

        [TestMethod]
        public void TestingSystemShouldTestAllTests()
        {
            var path = $@"{GetTestProjectsPath()}\TestProjects\TestProject_1\bin\Debug";
            var testsExecutionInfo = TestingSystem.Launch(path);

            CheckTestCount(trueTestCount: 2, falseTestCount: 0, ignoreTestCount: 0, indefiniteTestCount: 0, 
                testsExecutionInfo: testsExecutionInfo);

            CheckTrueTest(testsExecutionInfo.TrueTests[0], "TestProject_1.TestClass.TrueTest0");
            CheckTrueTest(testsExecutionInfo.TrueTests[1], "TestProject_1.TestClass.TrueTest1");
        }

        [TestMethod]
        public void CheckTestingSystemWithFalseTests()
        {
            var path = $@"{GetTestProjectsPath()}\TestProjects\TestProject_2\bin\Debug";
            var testsExecutionInfo = TestingSystem.Launch(path);

            CheckTestCount(trueTestCount: 1, falseTestCount: 3, ignoreTestCount: 0, indefiniteTestCount: 0,
                testsExecutionInfo: testsExecutionInfo);

            CheckFalseTest(testsExecutionInfo.FalseTests[0], "TestProject_2.TestClass.FalseTest0",
                typeof(AggregateException));
            CheckFalseTest(testsExecutionInfo.FalseTests[1], "TestProject_2.TestClass.FalseTest1",
                typeof(ExpectedExceptionWasNotThrown));
            CheckFalseTest(testsExecutionInfo.FalseTests[2], "TestProject_2.TestClass.FalseTest2",
                typeof(ExpectedExceptionWasNotThrown));
        }

        [TestMethod]
        public void CheckTestingSystemWithIgnoreTests()
        {
            var path = $@"{GetTestProjectsPath()}\TestProjects\TestProject_3\bin\Debug";
            var testsExecutionInfo = TestingSystem.Launch(path);

            CheckTestCount(trueTestCount: 1, falseTestCount: 0, ignoreTestCount: 2, indefiniteTestCount: 0,
                testsExecutionInfo: testsExecutionInfo);

            CheckIgnoreTest(testsExecutionInfo.IgnoreTests[0], "TestProject_3.TestClass.IgnoreTest0",
                "reason0");
            CheckIgnoreTest(testsExecutionInfo.IgnoreTests[1], "TestProject_3.TestClass.IgnoreTest1",
                "reason1");
        }

        [TestMethod]
        public void CheckTestingSystemWithSeveralTypes()
        {
            var path = $@"{GetTestProjectsPath()}\TestProjects\TestProject_4\bin\Debug";
            var testsExecutionInfo = TestingSystem.Launch(path);

            CheckTestCount(trueTestCount: 2, falseTestCount: 2, ignoreTestCount: 0, indefiniteTestCount: 0,
                testsExecutionInfo: testsExecutionInfo);

            CheckTrueTest(testsExecutionInfo.TrueTests[0], "TestProject_4.TestClass0.TrueTest0");
            CheckTrueTest(testsExecutionInfo.TrueTests[1], "TestProject_4.TestClass1.TrueTest1");
            CheckFalseTest(testsExecutionInfo.FalseTests[0], "TestProject_4.TestClass0.FalseTest0",
                typeof(Exception));
            CheckFalseTest(testsExecutionInfo.FalseTests[1], "TestProject_4.TestClass1.FalseTest1",
                typeof(Exception));
        }

        [TestMethod]
        public void CheckTestingSystemWithBeforeAndBeforeClassMethods()
        {
            var path = $@"{GetTestProjectsPath()}\TestProjects\TestProject_5\bin\Debug";
            var testsExecutionInfo = TestingSystem.Launch(path);

            CheckTestCount(trueTestCount: 1, falseTestCount: 0, ignoreTestCount: 0, indefiniteTestCount: 6, 
                testsExecutionInfo: testsExecutionInfo);

            for (int i = 0; i < 3; i++)
            {
                CheckIndefiniteTest(testsExecutionInfo.IndefiniteTests[i],
                    $"TestProject_5.TestClassBeforeClassWithException.IndefiniteTest{i}",
                    typeof(AggregateException));
            }

            for (int i = 3; i < 6; i++)
            {
                CheckIndefiniteTest(testsExecutionInfo.IndefiniteTests[i],
                    $"TestProject_5.TestClassBeforeWithException.IndefiniteTest{i}",
                    typeof(AggregateException));
            }
        }

        [TestMethod]
        public void CheckTestingSystemWithAfterAndAfterClassMethods()
        {
            var path = $@"{GetTestProjectsPath()}\TestProjects\TestProject_6\bin\Debug";
            var testsExecutionInfo = TestingSystem.Launch(path);

            CheckTestCount(trueTestCount: 1, falseTestCount: 0, ignoreTestCount: 0, indefiniteTestCount: 6,
                testsExecutionInfo: testsExecutionInfo);

            for (int i = 0; i < 3; i++)
            {
                CheckIndefiniteTest(testsExecutionInfo.IndefiniteTests[i],
                    $"TestProject_6.TestClassAfterClassWithException.IndefiniteTest{i}",
                    typeof(AggregateException));
            }

            for (int i = 3; i < 6; i++)
            {
                CheckIndefiniteTest(testsExecutionInfo.IndefiniteTests[i],
                    $"TestProject_6.TestClassAfterWithException.IndefiniteTest{i}",
                    typeof(AggregateException));
            }
        }

        [TestMethod]
        public void CheckTestingSystemWithIncorrectMethods()
        {
            var path = $@"{GetTestProjectsPath()}\TestProjects\TestProject_7\bin\Debug";
            var testsExecutionInfo = TestingSystem.Launch(path);

            CheckTestCount(trueTestCount: 0, falseTestCount: 0, ignoreTestCount: 0, indefiniteTestCount: 7,
                testsExecutionInfo: testsExecutionInfo);

            for (int i = 0; i < 3; i++)
            {
                CheckIndefiniteTest(testsExecutionInfo.IndefiniteTests[i],
                    $"TestProject_7.TestClassIncorrectMethod.IndefiniteTest{i}",
                    typeof(IncorrectMethodException));
            }

            for (int i = 3; i < 7; i++)
            {
                CheckIndefiniteTest(testsExecutionInfo.IndefiniteTests[i],
                    $"TestProject_7.TestClassIncorrectTest.IndefiniteTest{i}",
                    typeof(IncorrectMethodException));
            }
        }

        [TestMethod]
        public void TestingSystemShouldNotNoticeMethodsWithInappropriateAttributes()
        {
            var path = $@"{GetTestProjectsPath()}\TestProjects\TestProject_8\bin\Debug";
            var testsExecutionInfo = TestingSystem.Launch(path);

            CheckTestCount(trueTestCount: 1, falseTestCount: 0, ignoreTestCount: 0, indefiniteTestCount: 0,
                testsExecutionInfo: testsExecutionInfo);

            CheckTrueTest(testsExecutionInfo.TrueTests[0], "TestProject_8.TestClass.TrueTest");
        }

        private static void CheckTestCount(int trueTestCount, int falseTestCount,
            int ignoreTestCount, int indefiniteTestCount,
            TestsExecutionInfo testsExecutionInfo)
        {
            Assert.AreEqual(trueTestCount, testsExecutionInfo.TrueTests.Count);
            Assert.AreEqual(falseTestCount, testsExecutionInfo.FalseTests.Count);
            Assert.AreEqual(ignoreTestCount, testsExecutionInfo.IgnoreTests.Count);
            Assert.AreEqual(indefiniteTestCount, testsExecutionInfo.IndefiniteTests.Count);
        }

        private static void CheckTrueTest(DefaultTestExecutionInfo test, string name)
        {
            Assert.AreEqual(name, test.Name);
            Assert.AreEqual("True", test.Result);
            Assert.IsNull(test.UnexpectedException);
            Assert.IsNotNull(test.RunTime);
        }

        private static void CheckFalseTest(DefaultTestExecutionInfo test, string name,
            Type exceptionType)
        {
            Assert.AreEqual(name, test.Name);
            Assert.AreEqual("False", test.Result);
            Assert.AreEqual(exceptionType, test.UnexpectedException.GetType());
            Assert.IsNotNull(test.RunTime);
        }

        private static void CheckIgnoreTest(IgnoreTestExecutionInfo test, string name, string reason)
        {
            Assert.AreEqual(name, test.Name);
            Assert.AreEqual("Ignore", test.Result);
            Assert.AreEqual(reason, test.Reason);
        }

        private static void CheckIndefiniteTest(IndefiniteTestExecutionInfo test, string name,
            Type exceptionType)
        {
            Assert.AreEqual(name, test.Name);
            Assert.AreEqual("Indefinite", test.Result);
            Assert.AreEqual(exceptionType, test.Exception.GetType());
        }

        private static string GetTestProjectsPath()
            => new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName;
    }
}
