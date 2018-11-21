using System;
using MyNUnit.Exceptions;

namespace MyNUnit.SupportingClasses
{
    /// <summary>
    /// Класс, необходимый для вывода на консоль информации о результатах выполнения тестов.
    /// </summary>
    public static class ConsoleOutput
    {
        /// <summary>
        /// Вывод на консоль информации о результатах выполнения тестов.
        /// </summary>
        /// <param name="tests">Информации о результатах выполнения тестов.</param>
        public static void Display(TestsExecutionInfo tests)
        {
            foreach (var trueTest in tests.TrueTests)
            {
                DefaultTestExecutionDisplay(trueTest);
            }

            foreach (var falseTest in tests.FalseTests)
            {
                DefaultTestExecutionDisplay(falseTest);
            }

            foreach (var ignoreTest in tests.IgnoreTests)
            {
                IgnoreTestExecutionDisplay(ignoreTest);
            }

            foreach (var indefiniteTest in tests.IndefiniteTests)
            {
                IndefiniteTestExecutionDisplay(indefiniteTest);
            }
        }

        private static void DefaultTestExecutionDisplay(DefaultTestExecutionInfo executionInfo)
        {
            Console.WriteLine($"Result:\t{executionInfo.Result}");
            Console.WriteLine($"Name:\t{executionInfo.Name}");

            if (executionInfo.UnexpectedException != null)
            {
                Console.WriteLine(executionInfo.UnexpectedException);
            }

            Console.WriteLine($"Time:\t{executionInfo.RunTime} ms\n");
        }

        private static void IgnoreTestExecutionDisplay(IgnoreTestExecutionInfo executionInfo)
        {
            Console.WriteLine($"Result:\t{executionInfo.Result}");
            Console.WriteLine($"Name:\t{executionInfo.Name}");
            Console.WriteLine($"Reason:\t{executionInfo.Reason}\n");
        }

        private static void IndefiniteTestExecutionDisplay(IndefiniteTestExecutionInfo executionInfo)
        {
            Console.WriteLine($"Result:\t{executionInfo.Result}");
            Console.WriteLine($"Name:\t{executionInfo.Name}");

            Console.Write($"Reason:\t");

            if (executionInfo.Exception.GetType() == typeof(IncorrectMethodException))
            {
                Console.WriteLine($"Некорректная запись метода - " +
                    $"{(executionInfo.Exception as IncorrectMethodException).IncorrectMethodName}\n");
                return;
            }

            Console.WriteLine($"Брошено исключение\n{executionInfo.Exception}\n");
        }
    }
}
