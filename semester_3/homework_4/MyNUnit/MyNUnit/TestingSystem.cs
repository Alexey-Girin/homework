using System;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using MyNUnit.Exceptions;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace MyNUnit
{
    /// <summary>
    /// Класс, выполняющий тесты во всех сборках, расположенных по заданному пути.
    /// </summary>
    public static class TestingSystem
    {
        /// <summary>
        /// Объект, необходимый для синхронизации потоков при выводе информации.
        /// </summary>
        private static object displayLocker = new object();

        /// <summary>
        /// Выполнение тестов во всех сборках, расположенных по заданному пути.
        /// </summary>
        /// <param name="path">Путь, по которому выполняется обход сборок.</param>
        public static void RunTests(string path)
            => AssembliesEnumeration(path);

        /// <summary>
        /// Выполнение обхода сборок по заданному пути.
        /// </summary>
        /// <param name="path">Путь, по которому выполняется обход сборок.</param>
        private static void AssembliesEnumeration(string path)
        {
            foreach (var assemblyPath in Directory.GetFiles(path, "*.exe"))
            {
                TypesEnumeration(assemblyPath);
            }
        }

        /// <summary>
        /// Выполнение обхода открытых типов в заданной сборке.
        /// </summary>
        /// <param name="assemblyPath">Сборка, в которой выполняется обход типов.</param>
        private static void TypesEnumeration(string assemblyPath)
        {
            Assembly assembly = Assembly.LoadFile(assemblyPath);

            foreach (var type in assembly.GetExportedTypes())
            {
                MethodsEnumeration(type);
            }
        }

        /// <summary>
        /// Выполнение обхода и идентификации открытых методов заданного типа.
        /// </summary>
        /// <param name="type">Тип, в котором выполняется обход.</param>
        private static void MethodsEnumeration(Type type)
        {
            object instanceOfType = null;

            try
            {
                instanceOfType = Activator.CreateInstance(type);
            }
            catch (Exception)
            {
                return;
            }

            var methods = new Methods();

            foreach (var methodInfo in type.GetMethods())
            {
                var metadata = new Metadata
                {
                    Type = type,
                    MethodInfo = methodInfo,
                    InstanceOfType = instanceOfType
                };
                AttributesEnumeration(metadata, methods);
            }

            Execution(methods);
        }

        /// <summary>
        /// Выполнение вспомогательных методов, тестовых методов, 
        /// вывод информации о результатах выполнения тестов.
        /// </summary>
        /// <param name="methods">Коллекция данных, содержащих информацию о методах.</param>
        private static void Execution(Methods methods)
        {
            if (methods.TestMethods.Count == 0)
            {
                return;
            }

            if (methods.BeforeClassMethods.Count != 0)
            {
                try
                {
                    MethodsExecution(methods.BeforeClassMethods);
                }
                catch (Exception exception)
                {
                    DisplayAfterClassOrBeforeClassMethodError(methods, exception);
                    return;
                }
            }

            Action[] displayTestResults = TestsExecution(methods);

            if (displayTestResults == null)
            {
                return;
            }

            if (methods.AfterClassMethods.Count != 0)
            {
                try
                {
                    MethodsExecution(methods.AfterClassMethods);
                }
                catch (Exception exception)
                {
                    DisplayAfterClassOrBeforeClassMethodError(methods, exception);
                    return;
                }
            }

            foreach (var displayTestResult in displayTestResults)
            {
                displayTestResult();
            }
        }

        /// <summary>
        /// Обзор атрибутов метода, добавление информации о методе в соответсвующий класс в 
        /// <see cref="Methods"/>.
        /// </summary>
        /// <param name="metadata">Информация о методе.</param>
        /// <param name="methods">Коллекция данных, содержащих информацию о методах.</param>
        private static void AttributesEnumeration(Metadata metadata, Methods methods)
        {
            foreach (var attribute in Attribute.GetCustomAttributes(metadata.MethodInfo))
            {
                var attributType = attribute.GetType();

                if (attributType == typeof(TestAttribute))
                {
                    methods.TestMethods.Add(new TestMetadata
                    {
                        Type = metadata.Type,
                        MethodInfo = metadata.MethodInfo,
                        InstanceOfType = metadata.InstanceOfType,
                        Attribute = attribute
                    });
                }
                else if (attributType == typeof(BeforeClassAttribute))
                {
                    methods.BeforeClassMethods.Add(metadata);
                }
                else if (attributType == typeof(AfterClassAttribute))
                {
                    methods.AfterClassMethods.Add(metadata);
                }
                else if (attributType == typeof(AfterAttribute))
                {
                    methods.AfterMethods.Add(metadata);
                }
                else if (attributType == typeof(BeforeAttribute))
                {
                    methods.BeforeMethods.Add(metadata);
                }
                else
                {
                    continue;
                }

                return;
            }
        }

        /// <summary>
        /// Параллельное выполнение тестов.
        /// </summary>
        /// <param name="methods">Коллекция данных, содержащих информацию о методах.</param>
        /// <returns>Массив действий - вывод информации о тестах. null, если каким-либо методом с атрибутом 
        /// <see cref="AfterAttribute"/> или <see cref="BeforeAttribute"/> было брошено исключение.</returns>
        private static Action[] TestsExecution(Methods methods)
        {
            Task<Action>[] tasks = new Task<Action>[methods.TestMethods.Count];

            for (int i = 0; i < methods.TestMethods.Count; i++)
            {
                int j = i;
                tasks[j] = Task.Factory.StartNew(() => TestExecution(methods.TestMethods[j], methods));
            }

            Task.WaitAll(tasks);

            if (tasks[0].Result == null)
            {
                return null;
            }

            Action[] results = new Action[tasks.Length];

            for (int i = 0; i < tasks.Length; i++)
            {
                results[i] = tasks[i].Result;
            }

            return results;
        }

        /// <summary>
        /// Выполнение теста.
        /// </summary>
        /// <param name="metadata">Информация о тестовом методе.</param>
        /// <param name="methods">Коллекция данных, содержащих информацию о методах.</param>
        /// <returns>Действие - вывод информации о тесте. 
        /// null, если каким-либо вспомогательным методом было брошено исключение.</returns>
        private static Action TestExecution(TestMetadata metadata, Methods methods)
        {
            try
            {
                MethodsExecution(methods.BeforeMethods);
            }
            catch (Exception methodException)
            {
                lock (displayLocker)
                {
                    DisplayMethodError(metadata, methodException);
                }

                return null;
            }

            if ((metadata.Attribute as TestAttribute).Ignore != null)
            {
                return FinishTestExecution(() => DisplayReasonForIgnoring(metadata), metadata, methods);
            }

            Exception exception = null;
            bool isCatchException = false;
            var stopwatch = new Stopwatch();

            try
            {
                stopwatch.Start();
                metadata.MethodInfo.Invoke(metadata.InstanceOfType, null);
                stopwatch.Stop();
            }
            catch (Exception e)
            {
                stopwatch.Stop();
                isCatchException = true;

                if (e.InnerException.GetType() !=
                    (metadata.Attribute as TestAttribute).Excepted)
                {
                    exception = e;
                }
            }

            if (exception == null && (metadata.Attribute as TestAttribute).Excepted != null &&
                !isCatchException)
            {
                exception = new ExpectedExceptionWasNotThrown();
            }

            return FinishTestExecution(() => DisplayTestResult(metadata, exception, stopwatch),
                metadata, methods);
        }

        /// <summary>
        /// Завершение выполнения теста.
        /// </summary>
        /// <param name="result">Результат выполнения теста.</param>
        /// <param name="metadata">Информация о тестовом методе.</param>
        /// <param name="methods">Коллекция данных, содержащих информацию о методах.</param>
        /// <returns>Действие - вывод информации о тесте. 
        /// null, если каким-либо вспомогательным методом было брошено исключение.</returns>
        private static Action FinishTestExecution(Action displayTestResult, TestMetadata metadata,
            Methods methods)
        {
            try
            {
                MethodsExecution(methods.AfterMethods);
            }
            catch (Exception methodException)
            {
                lock (displayLocker)
                {
                    DisplayMethodError(metadata, methodException);
                }

                return null;
            }

            return displayTestResult;
        }

        /// <summary>
        /// Вывод информации об отмененном тесте и причины отмены.
        /// </summary>
        /// <param name="metadata">Информация о тестовом методе.</param>
        private static void DisplayReasonForIgnoring(TestMetadata metadata)
        {
            Console.WriteLine("Result:\tIgnore");
            Console.WriteLine($"Test:\t{metadata.Type.Namespace}.{metadata.Type.Name}." +
                $"{metadata.MethodInfo.Name}");
            Console.WriteLine($"Reason:\t{(metadata.Attribute as TestAttribute).Ignore}\n");
        }

        /// <summary>
        /// Вывод результата и времени выполнения теста.
        /// </summary>
        /// <param name="metadata">Информация о тестовом методе.</param>
        /// <param name="exception">Возможное исключение, брошенное при выполнении теста.</param>
        /// <param name="testRunTime">Время выполнения теста.</param>
        private static void DisplayTestResult(TestMetadata metadata, Exception exception,
            Stopwatch testRunTime)
        {
            Console.WriteLine($"Result:\t{exception == null}");
            Console.WriteLine($"Test:\t{metadata.Type.Namespace}.{metadata.Type.Name}." +
                $"{metadata.MethodInfo.Name}");

            if (exception != null)
            {
                Console.WriteLine(exception.GetType() == typeof(ExpectedExceptionWasNotThrown) ?
                    exception : exception.InnerException);
            }

            Console.WriteLine($"Time:\t{testRunTime.ElapsedMilliseconds} ms\n");
        }

        /// <summary>
        /// Вывод информации о тесте, если каким-либо вспомогательным методом было брошено исключение.
        /// </summary>
        /// <param name="metadata">Информация о тестовом методе.</param>
        /// <param name="exception">Брошенное исключение.</param>
        private static void DisplayMethodError(TestMetadata metadata, Exception exception)
        {
            Console.WriteLine("Result:\tIndefinite");
            Console.WriteLine($"Test:\t{metadata.Type.Namespace}.{metadata.Type.Name}." +
                $"{metadata.MethodInfo.Name}");
            Console.WriteLine($"Reason:{exception.InnerException.StackTrace} было брошено исключение");
            Console.WriteLine($"{exception.InnerException.GetType()}: {exception.InnerException.Message}\n");
        }

        /// <summary>
        /// Выполнение вспомогательных методов.
        /// </summary>
        /// <param name="methods">Коллекция данных, содержащих информацию о методах.</param>
        private static void MethodsExecution(IEnumerable<Metadata> methods)
        {
            foreach (var method in methods)
            {
                try
                {
                    method.MethodInfo.Invoke(method.InstanceOfType, null);
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Вывод информации о тестах, если каким-либо методом с 
        /// <see cref="AfterClassAttribute"/> или <see cref="BeforeClassAttribute"/> было брошено исключение.
        /// </summary>
        /// <param name="methods">Коллекция данных, содержащих информацию о методах.</param>
        /// <param name="exception">Брошенное исключение.</param>
        private static void DisplayAfterClassOrBeforeClassMethodError(Methods methods, Exception exception)
        {
            foreach (var test in methods.TestMethods)
            {
                DisplayMethodError(test, exception);
            }
        }

        /// <summary>
        /// Информация о методе.
        /// </summary>
        private class Metadata
        {
            public Type Type { get; set; }
            public MethodInfo MethodInfo { get; set; }
            public object InstanceOfType { get; set; }
        }

        /// <summary>
        /// Информация о тестовом методе.
        /// </summary>
        private class TestMetadata : Metadata
        {
            public Attribute Attribute { get; set; }
        }

        /// <summary>
        /// Классификация данных, содержащих информацию о методах, по соответствующим атрибутам.
        /// </summary>
        private class Methods
        {
            public List<TestMetadata> TestMethods { get; set; } = new List<TestMetadata>();
            public List<Metadata> BeforeClassMethods { get; set; } = new List<Metadata>();
            public List<Metadata> AfterClassMethods { get; set; } = new List<Metadata>();
            public BlockingCollection<Metadata> BeforeMethods { get; set; } =
                new BlockingCollection<Metadata>();
            public BlockingCollection<Metadata> AfterMethods { get; set; } =
                new BlockingCollection<Metadata>();
        }
    }
}
