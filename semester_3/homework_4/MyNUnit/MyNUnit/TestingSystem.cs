using System;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Linq;
using MyNUnit.Exceptions;
using MyNUnit.SupportingClasses;
using System.Collections.Generic;

namespace MyNUnit
{
    /// <summary>
    /// Класс, выполняющий тесты во всех сборках, расположенных по заданному пути.
    /// </summary>
    public static class TestingSystem
    {
        /// <summary>
        /// Коллекция, содержащая информацию о результатах выполнения тестов.
        /// </summary>
        private static ConcurrentBag<ITestExecutionInfo> testsExecutionInfo;

        /// <summary>
        /// Выполнение тестов во всех сборках, расположенных по заданному пути.
        /// </summary>
        /// <param name="path">Путь, по которому выполняется поиск сборок.</param>
        /// <returns>Информация о результатах выполнения тестов.</returns>
        public static TestsExecutionInfo Launch(string path)
        {
            testsExecutionInfo = new ConcurrentBag<ITestExecutionInfo>();
            RunTestsInAssemblies(path);

            var result = TestsExecutionInfoСlassifier.СlassifyTestsExecutionInfo(testsExecutionInfo);
            ConsoleOutput.Display(result);
            return result;
        }

        /// <summary>
        /// Запуск тестов во всех сборках, расположенных по заданному пути.
        /// </summary>
        /// <param name="path">Путь, по которому выполняется поиск сборок.</param>
        private static void RunTestsInAssemblies(string path)
        {
            string[] assemblyPathsDll = null;
            string[] assemblyPathsExe = null;

            try
            {
                assemblyPathsDll = Directory.GetFiles(path, "*.dll");
                assemblyPathsExe = Directory.GetFiles(path, "*.exe");
            }
            catch (Exception exception)
            {
                throw new PathErrorException("Ошибка. Путь имеет недопустимую форму", exception);
            }

            var assemblyPaths = assemblyPathsDll.Concat(assemblyPathsExe).ToArray();

            foreach (var assemblyPath in assemblyPaths)
            {
                RunTestsInTypes(assemblyPath);
            }
        }

        /// <summary>
        /// Запуск тестовых методов во всех типах заданной сборки.
        /// </summary>
        /// <param name="assemblyPath">Сборка, в которой выполняется обход типов.</param>
        private static void RunTestsInTypes(string assemblyPath)
        {
            Type[] types = null;

            try
            {
                types = Assembly.LoadFile(assemblyPath).GetExportedTypes();
            }
            catch (Exception)
            {
                return;
            }

            Task[] tasks = new Task[types.Length];

            for (int i = 0; i < tasks.Length; i++)
            {
                int j = i;
                tasks[j] = Task.Factory.StartNew(() => RunTestsInType(types[j]));
            }

            Task.WaitAll(tasks);
        }

        /// <summary>
        /// Запуск тестовых методов в заданном типе.
        /// </summary>
        /// <param name="type">Тип, в котором выполняется запуск тестовых методов.</param>
        private static void RunTestsInType(Type type)
        {
            try
            {
                Activator.CreateInstance(type);
            }
            catch (Exception)
            {
                return;
            }

            var methods = new MethodsClassification();

            foreach (var methodInfo in type.GetMethods())
            {
                AttributesEnumeration(methodInfo, methods);
            }

            if (methods.TestMethods.Count == 0 ||
                !ExecuteAuxiliaryClassMethods(methods.BeforeClassMethods, methods.TestMethods, type))
            {
                return;
            }

            var testsResultInfo = RunTests(methods, type);

            if (!ExecuteAuxiliaryClassMethods(methods.AfterClassMethods, methods.TestMethods, type))
            {
                return;
            }

            foreach (var testResultInfo in testsResultInfo)
            {
                testsExecutionInfo.Add(testResultInfo);
            }
        }

        /// <summary>
        /// Выполнение вспомогательных методов с атрибутами <see cref="AfterClassAttribute"/>, <see cref="BeforeClassAttribute"/>
        /// </summary>
        /// <param name="auxiliaryMethods">Коллекция вспомогательных методов.</param>
        /// <param name="tests">Коллекция тестов.</param>
        /// <param name="type">Тип, содержащий тестовые методы.</param>
        /// <returns>True, если выполнение прошло успешно.</returns>
        private static bool ExecuteAuxiliaryClassMethods(List<MethodInfo> auxiliaryMethods, List<TestMetadata> tests, Type type)
        {
            if (auxiliaryMethods.Count != 0)
            {
                var methodException = ExecuteAuxiliaryMethods(auxiliaryMethods, Activator.CreateInstance(type));

                if (methodException != null)
                {
                    SaveTestsInCaseOfAuxiliaryMethodExeption(tests, methodException, type);
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Добавление в <see cref="testsExecutionInfo"/> информации о тестах, в случае если какой-либо вспомогательный метод
        /// с атрибутом <see cref="AfterClassAttribute"/> или <see cref="BeforeClassAttribute"/> бросил исключение.
        /// </summary>
        /// <param name="tests">Коллекция тестов.</param>
        /// <param name="exception">Брошенное исключение.</param>
        /// <param name="type">Тип, содержащий тестовые методы.</param>
        private static void SaveTestsInCaseOfAuxiliaryMethodExeption(List<TestMetadata> tests, Exception exception, Type type)
        {
            foreach (var test in tests)
            {
                testsExecutionInfo.Add(new IndefiniteTestExecutionInfo(GetFullName(test.MethodInfo, type), exception));
            }
        }

        /// <summary>
        /// Обзор атрибутов метода, добавление информации о методе в соответсвующую коллекцию в
        /// <see cref="MethodsClassification"/>.
        /// </summary>
        /// <param name="methodInfo">Информация о методе.</param>
        /// <param name="methods">Коллекции данных, содержащих информацию о методах типа.</param>
        private static void AttributesEnumeration(MethodInfo methodInfo,
            MethodsClassification methods)
        {
            foreach (var attribute in Attribute.GetCustomAttributes(methodInfo))
            {
                var attributeType = attribute.GetType();

                if (attributeType == typeof(TestAttribute))
                {
                    methods.TestMethods.Add(new TestMetadata(methodInfo, attribute as TestAttribute));
                }
                else if (attributeType == typeof(BeforeClassAttribute))
                {
                    methods.BeforeClassMethods.Add(methodInfo);
                }
                else if (attributeType == typeof(AfterClassAttribute))
                {
                    methods.AfterClassMethods.Add(methodInfo);
                }
                else if (attributeType == typeof(AfterAttribute))
                {
                    methods.AfterMethods.Add(methodInfo);
                }
                else if (attributeType == typeof(BeforeAttribute))
                {
                    methods.BeforeMethods.Add(methodInfo);
                }
                else
                {
                    continue;
                }

                return;
            }
        }

        /// <summary>
        /// Параллельный запуск тестов.
        /// </summary>
        /// <param name="methods">Коллекции данных, содержащих информацию о методах типа.</param>
        /// <param name="type">Тип, содержащий тестовые методы.</param>
        /// <returns>Информация о результатах выполнения тестов.</returns>
        private static List<ITestExecutionInfo> RunTests(MethodsClassification methods, Type type)
        {
            var tasks = new Task<ITestExecutionInfo>[methods.TestMethods.Count];

            for (int i = 0; i < tasks.Length; i++)
            {
                int j = i;
                tasks[j] = Task.Factory.StartNew(()
                    => RunTest(methods.TestMethods[j], methods, type));
            }

            Task.WaitAll(tasks);

            var testsResultInfo = new List<ITestExecutionInfo>();

            for (int i = 0; i < tasks.Length; i++)
            {
                testsResultInfo.Add(tasks[i].Result);
            }

            return testsResultInfo;
        }

        /// <summary>
        /// Запуск теста.
        /// </summary>
        /// <param name="metadata">Информация о тестовом методе.</param>
        /// <param name="methods">Коллекции данных, содержащих информацию о методах типа.</param>
        /// <param name="type">Тип, содержащий тестовые методы.</param>
        /// <returns>Информация о результате выполнения теста.</returns>
        private static ITestExecutionInfo RunTest(TestMetadata metadata, MethodsClassification methods, Type type)
        {
            try
            {
                CheckMethod(metadata.MethodInfo);
            }
            catch (IncorrectMethodException exception)
            {
                return new IndefiniteTestExecutionInfo(GetFullName(metadata.MethodInfo, type), exception);
            }

            object instanceOfType = Activator.CreateInstance(type);

            var beforeMethodException = ExecuteAuxiliaryMethods(methods.BeforeMethods, instanceOfType);

            if (beforeMethodException != null)
            {
                return new IndefiniteTestExecutionInfo(GetFullName(metadata.MethodInfo, type), beforeMethodException);
            }

            if (metadata.Attribute.Ignore != null)
            {
                var afterMethodExceptionIgnoreTest = ExecuteAuxiliaryMethods(methods.AfterMethods, instanceOfType);

                if (afterMethodExceptionIgnoreTest != null)
                {
                    return new IndefiniteTestExecutionInfo(GetFullName(metadata.MethodInfo, type),
                        afterMethodExceptionIgnoreTest);
                }

                return new IgnoreTestExecutionInfo(GetFullName(metadata.MethodInfo, type), metadata.Attribute.Ignore);
            }

            var stopwatch = new Stopwatch();
            Exception testException = null;
            bool isCaughtException = false;

            try
            {
                stopwatch.Start();
                metadata.MethodInfo.Invoke(instanceOfType, null);
            }
            catch (Exception exception)
            {
                isCaughtException = true;

                if (metadata.Attribute.Expected != exception.InnerException.GetType())
                {
                    testException = exception.InnerException;
                }
            }
            finally
            {
                stopwatch.Stop();
            }

            if (metadata.Attribute.Expected != null && (!isCaughtException || testException != null))
            {
                testException = new ExpectedExceptionWasNotThrown();
            }

            var afterMethodException = ExecuteAuxiliaryMethods(methods.AfterMethods, instanceOfType);

            if (afterMethodException != null)
            {
                return new IndefiniteTestExecutionInfo(GetFullName(metadata.MethodInfo, type), afterMethodException);
            }

            return new DefaultTestExecutionInfo(GetFullName(metadata.MethodInfo, type), stopwatch.ElapsedMilliseconds,
                testException);
        }

        /// <summary>
        /// Выполнение вспомогательных методов.
        /// </summary>
        /// <param name="auxiliaryMethods">Коллекция вспомогательных методов.</param>
        /// <param name="instanceOfType">Экземпляр, на котором запускается тест.</param>
        /// <returns>Исключение вспомогательного метода. null, если исключение не возникло.</returns>
        private static Exception ExecuteAuxiliaryMethods(List<MethodInfo> auxiliaryMethods, object instanceOfType)
        {
            try
            {
                MethodsExecution(auxiliaryMethods, instanceOfType);
            }
            catch (IncorrectMethodException exception)
            {
                return exception;
            }
            catch (Exception exception)
            {
                return exception.InnerException;
            }

            return null;
        }

        /// <summary>
        /// Выполнение вспомогательных методов для теста.
        /// </summary>
        /// <param name="methods">Коллекция вспомогательных методов.</param>
        /// <param name="instanceOfType">Экземпляр, на котором запускается тест.</param>
        private static void MethodsExecution(List<MethodInfo> methods, object instanceOfType)
        {
            foreach (var method in methods)
            {
                CheckMethod(method);
                method.Invoke(instanceOfType, null);
            }
        }

        /// <summary>
        /// Проверка метода на корректность.
        /// </summary>
        /// <param name="methodInfo">Информация о методе.</param>
        private static void CheckMethod(MethodInfo methodInfo)
        {
            if (methodInfo.ReturnType != typeof(void) ||
                methodInfo.GetParameters().Count() != 0 ||
                Attribute.GetCustomAttributes(methodInfo).Length != 1)
            {
                throw new IncorrectMethodException(methodInfo.Name);
            }
        }
        
        /// <summary>
        /// Получение полного имени метода.
        /// </summary>
        /// <param name="methodInfo">Информация о методе.</param>
        /// <param name="type">Тип, содержащий тестовые методы.</param>
        /// <returns>Полное имя метода.</returns>
        public static string GetFullName(MethodInfo methodInfo, Type type)
            => $"{type.Namespace}.{type.Name}.{methodInfo.Name}";
    }
}
