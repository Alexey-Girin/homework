using System;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using MyNUnit.Exceptions;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Linq;

namespace MyNUnit
{
    /// <summary>
    /// Класс, выполняющий тесты во всех сборках, расположенных по заданному пути.
    /// </summary>
    public static class TestingSystem
    {
        /// <summary>
        /// Коллекция, содержащая информацию о результатах выполнения тестов для каждого типа.
        /// </summary>
        private static List<TestMethodsInTypeExecutionInfo> TestsExecutionInfo;

        /// <summary>
        /// Выполнение тестов во всех сборках, расположенных по заданному пути.
        /// </summary>
        /// <param name="path">Путь, по которому выполняется обход сборок.</param>
        /// <returns></returns>
        public static List<TestMethodsInTypeExecutionInfo> RunTests(string path)
        {
            TestsExecutionInfo = new List<TestMethodsInTypeExecutionInfo>();

            AssembliesEnumeration(path);

            TestsExecutionInfo.Sort();
            return TestsExecutionInfo;
        }

        /// <summary>
        /// Выполнение обхода сборок по заданному пути.
        /// </summary>
        /// <param name="path">Путь, по которому выполняется обход сборок.</param>
        private static void AssembliesEnumeration(string path)
        {
            string[] assemblyPaths_dll = null;
            string[] assemblyPaths_exe = null;

            try
            {
                assemblyPaths_dll = Directory.GetFiles(path, "*.dll");
                assemblyPaths_exe = Directory.GetFiles(path, "*.exe");
            }
            catch (Exception)
            {
                throw new PathErrorException();
            }

            var assemblyPaths = assemblyPaths_dll.Concat(assemblyPaths_exe).ToArray();

            foreach (var assemblyPath in assemblyPaths)
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
            Type[] types = Assembly.LoadFile(assemblyPath).GetExportedTypes();
            Task[] tasks = new Task[types.Length];

            for (int i = 0; i < tasks.Length; i++)
            {
                int j = i;
                tasks[j] = Task.Factory.StartNew(() => MethodsEnumeration(types[j]));
            }

            Task.WaitAll(tasks);
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

            if (methods.TestMethods.Count == 0)
            {
                return;
            }

            Execution(methods);
            DisplayResults(methods);

            var executionInfo = new TestMethodsInTypeExecutionInfo(instanceOfType, type);

            SaveResults(methods, executionInfo);
            TestsExecutionInfo.Add(executionInfo);
        }

        /// <summary>
        /// Обзор атрибутов метода, добавление информации о методе в соответствующую  
        /// коллекцию в <see cref="Methods"/>.
        /// </summary>
        /// <param name="metadata">Информация о методе.</param>
        /// <param name="methods">Коллекции данных, содержащих информацию о методах типа.</param>
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
                        Attribute = attribute as TestAttribute
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
        /// Поэтапное выполнение вспомогательных и тестовых методов.
        /// </summary>
        /// <param name="methods">Коллекции данных, содержащих информацию о методах типа.</param>
        private static void Execution(Methods methods)
        {
            if (methods.BeforeClassMethods.Count != 0)
            {
                try
                {
                    MethodsExecution(methods.BeforeClassMethods);
                }
                catch (Exception exception)
                {
                    ExceptionNotification(methods, exception);
                    return;
                }
            }

            TestsExecution(methods);

            if (methods.AfterClassMethods.Count != 0)
            {
                try
                {
                    MethodsExecution(methods.AfterClassMethods);
                }
                catch (Exception exception)
                {
                    ExceptionNotification(methods, exception);
                }
            }
        }

        /// <summary>
        /// Уведомление тестовых методов об исключении, пойманном при выполнении вспомогательного метода.
        /// </summary>
        /// <param name="methods">Коллекции данных, содержащих информацию о методах типа.</param>
        /// <param name="exception">Пойманное исключение.</param>
        private static void ExceptionNotification(Methods methods, Exception exception)
        {
            foreach (var test in methods.TestMethods)
            {
                test.AuxiliaryMethodException = exception;
            }
        }

        /// <summary>
        /// Параллельное выполнение тестов.
        /// </summary>
        /// <param name="methods">Коллекции данных, содержащих информацию о методах типа.</param>
        private static void TestsExecution(Methods methods)
        {
            Task[] tasks = new Task[methods.TestMethods.Count];

            for (int i = 0; i < methods.TestMethods.Count; i++)
            {
                int j = i;
                tasks[j] = Task.Factory.StartNew(() => TestExecution(methods.TestMethods[j], methods));
            }

            Task.WaitAll(tasks);
        }

        /// <summary>
        /// Выполнение теста.
        /// </summary>
        /// <param name="metadata">Информация о тестовом методе.</param>
        /// <param name="methods">Коллекции данных, содержащих информацию о методах типа.</param>
        private static void TestExecution(TestMetadata metadata, Methods methods)
        {
            try
            {
                lock (methods.Locker)
                {
                    MethodsExecution(methods.BeforeMethods);
                }
            }
            catch (Exception methodException)
            {
                metadata.AuxiliaryMethodException = methodException;
                return;
            }

            if (metadata.Attribute.Ignore != null)
            {
                AfterMethodsExecution(metadata, methods);
                return;
            }

            var stopwatch = new Stopwatch();

            try
            {
                stopwatch.Start();
                metadata.MethodInfo.Invoke(metadata.InstanceOfType, null);
                stopwatch.Stop();
            }
            catch (Exception exception)
            {
                stopwatch.Stop();

                if (exception.InnerException.GetType() == metadata.Attribute.Expected)
                {
                    if (AfterMethodsExecution(metadata, methods))
                    {
                        metadata.RunTime = stopwatch.ElapsedMilliseconds.ToString();
                        return;
                    }
                    return;
                }

                if (AfterMethodsExecution(metadata, methods))
                {
                    metadata.TestException = exception.InnerException;
                    metadata.RunTime = stopwatch.ElapsedMilliseconds.ToString();
                    return;
                }
                return;
            }

            if (metadata.Attribute.Expected != null)
            {
                if (AfterMethodsExecution(metadata, methods))
                {
                    metadata.TestException = new ExpectedExceptionWasNotThrown();
                    metadata.RunTime = stopwatch.ElapsedMilliseconds.ToString();
                }
                return;
            }

            if (AfterMethodsExecution(metadata, methods))
            {
                metadata.RunTime = stopwatch.ElapsedMilliseconds.ToString();
            }
        }

        /// <summary>
        /// Выполнение вспомогательных методов с атрибутом <see cref="AfterAttribute"/>.
        /// </summary>
        /// <param name="metadata">Информация о тестовом методе.</param>
        /// <param name="methods">Коллекции данных, содержащих информацию о методах типа.</param>
        /// <returns>True, если выполнение прошло успешно.</returns>
        private static bool AfterMethodsExecution(TestMetadata metadata, Methods methods)
        {
            try
            {
                lock (methods.Locker)
                {
                    MethodsExecution(methods.AfterMethods);
                }
            }
            catch (Exception methodException)
            {
                metadata.AuxiliaryMethodException = methodException;
                return false;
            }

            return true;
        }

        /// <summary>
        /// Выполнение вспомогательных методов.
        /// </summary>
        /// <param name="methods">Коллекции данных, содержащих информацию о методах типа.</param>
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
        /// Вывод отчета о выполнении тестов.
        /// </summary>
        /// <param name="methods">Коллекции данных, содержащих информацию о методах типа.</param>
        private static void DisplayResults(Methods methods)
        {
            foreach (var method in methods.TestMethods)
            {
                method.Display();
            }
        }

        /// <summary>
        /// Добавление информации о результатах выполнения тестовых методов текущего типа в 
        /// <see cref="TestMethodsInTypeExecutionInfo"/>.
        /// </summary>
        /// <param name="methods">Коллекции данных, содержащих информацию о методах типа.</param>
        /// <param name="executionInfo">Экземляр <see cref="TestMethodsInTypeExecutionInfo"/>
        /// для текущего типа.</param>
        private static void SaveResults(Methods methods, TestMethodsInTypeExecutionInfo executionInfo)
        {
            foreach (var method in methods.TestMethods)
            {
                method.Save(executionInfo);
            }
        }

        /// <summary>
        /// Информация о методе.
        /// </summary>
        private class Metadata
        {
            public Type Type { get; set; }
            public object InstanceOfType { get; set; }
            public MethodInfo MethodInfo { get; set; }
        }

        /// <summary>
        /// Информация о тестовом методе.
        /// </summary>
        private class TestMetadata : Metadata
        {
            private TestAttribute attribute;

            /// <summary>
            /// Атрибут метода.
            /// </summary>
            public TestAttribute Attribute
            {
                get
                {
                    return attribute;
                }
                set
                {
                    if (value.Ignore != null && auxiliaryMethodException == null)
                    {
                        status = new IgnoreTestStatus();
                    }
                    attribute = value;
                }
            }

            /// <summary>
            /// Время выполнения метода.
            /// </summary>
            public string RunTime { get; set; }

            /// <summary>
            /// Возможное исключение.
            /// </summary>
            public Exception TestException { get; set; }

            private Exception auxiliaryMethodException;

            /// <summary>
            /// Возможное исключение вспомогательного метода.
            /// </summary>
            public Exception AuxiliaryMethodException
            {
                get
                {
                    return auxiliaryMethodException;
                }
                set
                {
                    status = new IndefiniteTestStatus();
                    auxiliaryMethodException = value;
                }
            }

            private DefaultTestStatus status = new DefaultTestStatus();

            /// <summary>
            /// Вывод отчета о выполнении теста.
            /// </summary>
            public void Display() => status.Display(this);

            /// <summary>
            /// Добавление инофрмации о результате выполнения теста.
            /// </summary>
            /// <param name="executionInfo"></param>
            public void Save(TestMethodsInTypeExecutionInfo executionInfo)
                => status.Save(this, executionInfo);
            
            /// <summary>
            /// Статус тестового метода с результатом выполнения True или False.
            /// </summary>
            private class DefaultTestStatus
            {
                public virtual void Display(TestMetadata metadata)
                {
                    Console.WriteLine($"Result:\t{metadata.TestException == null}");
                    Console.WriteLine($"Test:\t{GetFullNameOfMethod(metadata)}");

                    if (metadata.TestException != null)
                    {
                        Console.WriteLine(metadata.TestException);
                    }

                    Console.WriteLine($"Time:\t{metadata.RunTime} ms\n");
                }

                public virtual void Save(TestMetadata metadata, TestMethodsInTypeExecutionInfo executionInfo)
                {
                    if (metadata.TestException == null)
                    {
                        executionInfo.TestsCountInfo.TrueTestCount++;
                        return;
                    }

                    executionInfo.TestsCountInfo.FalseTestCount++;
                }

                public static string GetFullNameOfMethod(Metadata metadata)
                    => $"{metadata.Type.Namespace}.{metadata.Type.Name}.{metadata.MethodInfo.Name}";
            }

            /// <summary>
            /// Статус тестового метода с результатом выполнения Ignore.
            /// </summary>
            private class IgnoreTestStatus : DefaultTestStatus
            {
                public override void Display(TestMetadata metadata)
                {
                    Console.WriteLine("Result:\tIgnore");
                    Console.WriteLine($"Test:\t{GetFullNameOfMethod(metadata)}");
                    Console.WriteLine($"Reason:\t{metadata.Attribute.Ignore}\n");
                }

                public override void Save(TestMetadata metadata, TestMethodsInTypeExecutionInfo executionInfo)
                    => executionInfo.TestsCountInfo.IgnoreTestCount++;
            }

            /// <summary>
            /// Статус тестового метода с результатом выполнения Indefinite.
            /// </summary>
            private class IndefiniteTestStatus : DefaultTestStatus
            {
                public override void Display(TestMetadata metadata)
                {
                    Console.WriteLine("Result:\tIndefinite");
                    Console.WriteLine($"Test:\t{GetFullNameOfMethod(metadata)}");
                    Console.WriteLine($"Reason:\tброшено исключение\n{metadata.AuxiliaryMethodException.InnerException}\n");
                }

                public override void Save(TestMetadata metadata, TestMethodsInTypeExecutionInfo executionInfo)
                    => executionInfo.TestsCountInfo.IndefiniteTestCount++;
            }
        }

        /// <summary>
        /// Классификация данных, содержащих информацию о методах типа, по соответствующим атрибутам.
        /// </summary>
        private class Methods
        {
            /// <summary>
            /// Коллекция тестовых методов некоторого типа.
            /// </summary>
            public List<TestMetadata> TestMethods { get; set; } = new List<TestMetadata>();

            /// <summary>
            /// Коллекция вспомогательных методов с <see cref="BeforeClassAttribute"/> некоторого типа.
            /// </summary>
            public List<Metadata> BeforeClassMethods { get; set; } = new List<Metadata>();

            /// <summary>
            /// Коллекция вспомогательных методов с <see cref="AfterClassAttribute"/> некоторого типа.
            /// </summary>
            public List<Metadata> AfterClassMethods { get; set; } = new List<Metadata>();

            /// <summary>
            /// Коллекция вспомогательных методов с <see cref="BeforeAttribute"/> некоторого типа.
            /// </summary>
            public ConcurrentBag<Metadata> BeforeMethods { get; set; } = new ConcurrentBag<Metadata>();

            /// <summary>
            /// Коллекция вспомогательных методов с <see cref="AfterAttribute"/> некоторого типа.
            /// </summary>
            public ConcurrentBag<Metadata> AfterMethods { get; set; } = new ConcurrentBag<Metadata>();

            /// <summary>
            /// Объект, необходимый для синхронизации тестовых потоков при выполнении вспомогательных
            /// методов с <see cref="AfterAttribute"/> или <see cref="BeforeAttribute"/>.
            /// </summary>
            public object Locker { get; set; } = new object();
        }
    }
}
