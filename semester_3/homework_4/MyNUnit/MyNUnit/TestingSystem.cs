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

            RunTests(methods, type);
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
        private static void RunTests(MethodsClassification methods, Type type)
        {
            var tasks = new Task[methods.TestMethods.Count];

            for (int i = 0; i < methods.TestMethods.Count; i++)
            {
                int j = i;
                tasks[j] = Task.Factory.StartNew(()
                    => RunTest(methods.TestMethods[j], methods, type));
            }

            Task.WaitAll(tasks);
        }

        /// <summary>
        /// Запуск теста.
        /// </summary>
        /// <param name="metadata">Информация о тестовом методе.</param>
        /// <param name="methods">Коллекции данных, содержащих информацию о методах типа.</param>
        /// <param name="type">Тип, содержащий тестовые методы.</param>
        private static void RunTest(TestMetadata metadata, MethodsClassification methods, Type type)
        {
            try
            {
                CheckMethod(metadata.MethodInfo);
            }
            catch (IncorrectMethodException exception)
            {
                SaveExecutuinInfo(new IndefiniteTestExecutionInfo(GetFullName(metadata.MethodInfo, type), exception));
                return;
            }

            object instanceOfType = Activator.CreateInstance(type);
            ITestExecutionInfo executionInfo = null;

            ExecuteAuxiliaryMethods(methods.BeforeClassMethods, methods.BeforeMethods, metadata.MethodInfo,
                ref executionInfo, instanceOfType, type);

            if (executionInfo != null)
            {
                return;
            }

            if (metadata.Attribute.Ignore != null)
            {
                executionInfo = new IgnoreTestExecutionInfo(GetFullName(metadata.MethodInfo, type),
                    metadata.Attribute.Ignore);
                ExecuteAuxiliaryMethods(methods.AfterMethods, methods.AfterClassMethods, metadata.MethodInfo,
                    ref executionInfo, instanceOfType, type);
                return;
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

            executionInfo = new DefaultTestExecutionInfo(GetFullName(metadata.MethodInfo, type),
                stopwatch.ElapsedMilliseconds, testException);
            ExecuteAuxiliaryMethods(methods.AfterMethods, methods.AfterClassMethods, metadata.MethodInfo,
                ref executionInfo, instanceOfType, type);
        }

        /// <summary>
        /// Выполнение вспомогательных методов.
        /// </summary>
        /// <param name="firstMethods">Коллекция методов, которые будут выполнены в первую очередь.</param>
        /// <param name="secondMethods">Коллекция методов, которые будут выполнены во вторую очередь.</param>
        /// <param name="methodInfo">Информация о тестовом методе.</param>
        /// <param name="executionInfo">Объект, содержащий информацию о результате выполения теста.</param>
        /// <param name="instanceOfType">Экземпляр, на котором запускается тест.</param>
        /// <param name="type">Тип, содержащий тестовые методы.</param>
        private static void ExecuteAuxiliaryMethods(List<MethodInfo> firstMethods, List<MethodInfo> secondMethods,
            MethodInfo methodInfo, ref ITestExecutionInfo executionInfo, object instanceOfType, Type type)
        {
            try
            {
                MethodsExecution(firstMethods, instanceOfType);
                MethodsExecution(secondMethods, instanceOfType);
            }
            catch (IncorrectMethodException exception)
            {
                executionInfo = new IndefiniteTestExecutionInfo(GetFullName(methodInfo, type), exception);
            }
            catch (Exception exception)
            {
                executionInfo = new IndefiniteTestExecutionInfo(GetFullName(methodInfo, type),
                    exception.InnerException);
            }

            if (executionInfo != null)
            {
                SaveExecutuinInfo(executionInfo);
            }
        }

        /// <summary>
        /// Добавление информации о результате выполнения теста в <see cref="testsExecutionInfo"/>.
        /// </summary>
        /// <param name="executionInfo">Информация о результате выполнения теста.</param>
        private static void SaveExecutuinInfo(ITestExecutionInfo executionInfo)
            => testsExecutionInfo.Add(executionInfo);

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
            if (methodInfo.ReturnType != typeof(void) || methodInfo.GetParameters().Count() != 0)
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
