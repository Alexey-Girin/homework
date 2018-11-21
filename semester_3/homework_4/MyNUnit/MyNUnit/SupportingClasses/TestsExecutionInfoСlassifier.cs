using System.Collections.Concurrent;

namespace MyNUnit.SupportingClasses
{
    /// <summary>
    /// Класс, необходимый для выполнения классификации результатов тестов.
    /// </summary>
    public static class TestsExecutionInfoСlassifier
    {
        /// <summary>
        /// Выполнение классификации результатов тестов.
        /// </summary>
        /// <param name="tests">Коллекция, содержащая информацию о результатах выполнения тестов.</param>
        /// <returns>Объект, представляющий классификацию результатов выполнения тестов.</returns>
        public static TestsExecutionInfo СlassifyTestsExecutionInfo(ConcurrentBag<ITestExecutionInfo> tests)
        {
            var testsExecutionInfo = new TestsExecutionInfo();

            foreach (var test in tests)
            {
                if (test.Result == "True")
                {
                    testsExecutionInfo.TrueTests.Add(test as DefaultTestExecutionInfo);
                }
                else if (test.Result == "False")
                {
                    testsExecutionInfo.FalseTests.Add(test as DefaultTestExecutionInfo);
                }
                else if (test.Result == "Ignore")
                {
                    testsExecutionInfo.IgnoreTests.Add(test as IgnoreTestExecutionInfo);
                }
                else
                {
                    testsExecutionInfo.IndefiniteTests.Add(test as IndefiniteTestExecutionInfo);
                }
            }

            testsExecutionInfo.TrueTests.Sort();
            testsExecutionInfo.FalseTests.Sort();
            testsExecutionInfo.IgnoreTests.Sort();
            testsExecutionInfo.IndefiniteTests.Sort();

            return testsExecutionInfo;
        }
    }
}
