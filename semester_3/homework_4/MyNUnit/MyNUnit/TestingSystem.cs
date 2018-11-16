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
        /// <returns>Информация о результатах выполнения всех тестов.</returns>
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

            var methods = new Methods(instanceOfType, type);

            foreach (var methodInfo in type.GetMethods())
            {
                var metadata = new Metadata(methods, methodInfo);
                AttributesEnumeration(metadata, methods);
            }

            if (methods.TestMethods.Count == 0)
            {
                return;
            }

            methods.Execution();
            methods.DisplayResults();

            TestsExecutionInfo.Add(methods.SaveResults());
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
                if (methods.AddMethodByAttribute(attribute, metadata))
                {
                    return;
                }
            }
        }

        /// <summary>
        /// Информация о методе.
        /// </summary>
        private class Metadata
        {
            /// <summary>
            /// Коллекции тестовых и вспомогательных методов типа, в котором содержится данный метод.
            /// </summary>
            public Methods Methods { get; }

            /// <summary>
            /// Объект, обеспечивающий доступ к данным метода.
            /// </summary>
            public MethodInfo MethodInfo { get; }

            /// <summary>
            /// Конструктор экземпляра класса <see cref="Metadata"/>.
            /// </summary>
            /// <param name="methods">Коллекции тестовых и вспомогательных методов типа, 
            /// в котором содержится данный метод.</param>
            /// <param name="methodInfo">Объект, обеспечивающий доступ к данным метода.</param>
            public Metadata(Methods methods, MethodInfo methodInfo)
            {
                Methods = methods;
                MethodInfo = methodInfo;
            }
        }

        /// <summary>
        /// Данные тестового метода и их обработка.
        /// </summary>
        private class TestMetadata : Metadata
        {
            /// <summary>
            /// Атрибут метода.
            /// </summary>
            public TestAttribute Attribute { get; }

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
            /// Конструктор экземпляра класса <see cref="TestMetadata"/>.
            /// </summary>
            /// <param name="methods">Коллекции тестовых и вспомогательных методов типа,
            /// в котором содержится данный тестовый метод.</param>
            /// <param name="methodInfo">Объект, обеспечивающий доступ к данным тестового метода.</param>
            /// <param name="attribute">Атрибут тестового метода.</param>
            public TestMetadata(Methods methods, MethodInfo methodInfo, TestAttribute attribute)
                : base(methods, methodInfo)
            {
                if (attribute.Ignore != null)
                {
                    status = new IgnoreTestStatus();
                }

                Attribute = attribute;
            }

            /// <summary>
            /// Вывод инофрмации о результате выполнения теста.
            /// </summary>
            public void Display() => status.Display(this);

            /// <summary>
            /// Сохранение информации о результате выполнения теста.
            /// </summary>
            /// <param name="executionInfo">Экземляр <see cref="TestMethodsInTypeExecutionInfo"/>
            /// для текущего типа, в который будет сохранена информация.</param>
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
                    => $"{metadata.Methods.Type.Namespace}.{metadata.Methods.Type.Name}." +
                    $"{metadata.MethodInfo.Name}";
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
        /// Классификация и обработка данных, содержащих информацию о методах типа.
        /// </summary>
        private class Methods
        {
            /// <summary>
            /// Коллекция тестовых методов типа.
            /// </summary>
            public List<TestMetadata> TestMethods { get; set; } = new List<TestMetadata>();

            /// <summary>
            /// Коллекция вспомогательных методов с <see cref="BeforeClassAttribute"/> типа.
            /// </summary>
            public List<Metadata> BeforeClassMethods { get; set; } = new List<Metadata>();

            /// <summary>
            /// Коллекция вспомогательных методов с <see cref="AfterClassAttribute"/> типа.
            /// </summary>
            public List<Metadata> AfterClassMethods { get; set; } = new List<Metadata>();

            /// <summary>
            /// Коллекция вспомогательных методов с <see cref="BeforeAttribute"/> типа.
            /// </summary>
            public ConcurrentBag<Metadata> BeforeMethods { get; set; } = new ConcurrentBag<Metadata>();

            /// <summary>
            /// Коллекция вспомогательных методов с <see cref="AfterAttribute"/> типа.
            /// </summary>
            public ConcurrentBag<Metadata> AfterMethods { get; set; } = new ConcurrentBag<Metadata>();

            /// <summary>
            /// Объект, необходимый для синхронизации тестовых потоков при выполнении вспомогательных
            /// методов с <see cref="AfterAttribute"/> или <see cref="BeforeAttribute"/>.
            /// </summary>
            public object Locker { get; set; } = new object();

            /// <summary>
            /// Тип, содержащий тестовые методы.
            /// </summary>
            public Type Type { get; }

            /// <summary>
            /// Объект типа <see cref="Type"/>, в котором выполняются тесты.
            /// </summary>
            public object InstanceOfType { get; }

            /// <summary>
            /// Конструктор экземпляра класса <see cref="Methods"/>.
            /// </summary>
            /// <param name="instanceOfType">Объект типа <see cref="Type"/>, 
            /// в котором выполняются тесты.</param>
            /// <param name="type">Тип, содержащий тестовые методы.</param>
            public Methods(object instanceOfType, Type type)
            {
                InstanceOfType = instanceOfType;
                Type = type;
            }

            /// <summary>
            /// Идентификация и добавление метода в соответствующую коллекцию.
            /// </summary>
            /// <param name="attribute">Атрибут метода.</param>
            /// <param name="metadata">Информация о методе.</param>
            /// <returns>True, если добавление прошло успешно.</returns>
            public bool AddMethodByAttribute(Attribute attribute, Metadata metadata)
            {
                var attributType = attribute.GetType();

                if (attributType == typeof(TestAttribute))
                {
                    TestMethods.Add(new TestMetadata(this, metadata.MethodInfo, attribute as TestAttribute));
                }
                else if (attributType == typeof(BeforeClassAttribute))
                {
                    BeforeClassMethods.Add(metadata);
                }
                else if (attributType == typeof(AfterClassAttribute))
                {
                    AfterClassMethods.Add(metadata);
                }
                else if (attributType == typeof(AfterAttribute))
                {
                    AfterMethods.Add(metadata);
                }
                else if (attributType == typeof(BeforeAttribute))
                {
                    BeforeMethods.Add(metadata);
                }
                else
                {
                    return false;
                }

                return true;
            }

            /// <summary>
            /// Поэтапное выполнение вспомогательных и тестовых методов текущего типа.
            /// </summary>
            public void Execution()
            {
                if (BeforeClassMethods.Count != 0)
                {
                    try
                    {
                        MethodsExecution(BeforeClassMethods);
                    }
                    catch (Exception exception)
                    {
                        ExceptionNotification(exception);
                        return;
                    }
                }

                TestsExecution();

                if (AfterClassMethods.Count != 0)
                {
                    try
                    {
                        MethodsExecution(AfterClassMethods);
                    }
                    catch (Exception exception)
                    {
                        ExceptionNotification(exception);
                    }
                }
            }

            /// <summary>
            /// Уведомление тестовых методов об исключении, пойманном при выполнении вспомогательного метода.
            /// </summary>
            /// <param name="exception">Пойманное исключение.</param>
            private void ExceptionNotification(Exception exception)
            {
                foreach (var test in TestMethods)
                {
                    test.AuxiliaryMethodException = exception;
                }
            }

            /// <summary>
            /// Параллельное выполнение тестов.
            /// </summary>
            private void TestsExecution()
            {
                Task[] tasks = new Task[TestMethods.Count];

                for (int i = 0; i < TestMethods.Count; i++)
                {
                    int j = i;
                    tasks[j] = Task.Factory.StartNew(() => TestExecution(TestMethods[j]));
                }

                Task.WaitAll(tasks);
            }

            /// <summary>
            /// Выполнение теста.
            /// </summary>
            /// <param name="metadata">Информация о тестовом методе.</param>
            private void TestExecution(TestMetadata metadata)
            {
                try
                {
                    lock (Locker)
                    {
                        MethodsExecution(BeforeMethods);
                    }
                }
                catch (Exception methodException)
                {
                    metadata.AuxiliaryMethodException = methodException;
                    return;
                }

                if (metadata.Attribute.Ignore != null)
                {
                    AfterMethodsExecution(metadata);
                    return;
                }

                var stopwatch = new Stopwatch();

                try
                {
                    stopwatch.Start();
                    metadata.MethodInfo.Invoke(metadata.Methods.InstanceOfType, null);
                    stopwatch.Stop();
                }
                catch (Exception exception)
                {
                    stopwatch.Stop();

                    if (exception.InnerException.GetType() == metadata.Attribute.Expected)
                    {
                        if (AfterMethodsExecution(metadata))
                        {
                            metadata.RunTime = stopwatch.ElapsedMilliseconds.ToString();
                            return;
                        }
                        return;
                    }

                    if (AfterMethodsExecution(metadata))
                    {
                        metadata.TestException = exception.InnerException;
                        metadata.RunTime = stopwatch.ElapsedMilliseconds.ToString();
                        return;
                    }
                    return;
                }

                if (metadata.Attribute.Expected != null)
                {
                    if (AfterMethodsExecution(metadata))
                    {
                        metadata.TestException = new ExpectedExceptionWasNotThrown();
                        metadata.RunTime = stopwatch.ElapsedMilliseconds.ToString();
                    }
                    return;
                }

                if (AfterMethodsExecution(metadata))
                {
                    metadata.RunTime = stopwatch.ElapsedMilliseconds.ToString();
                }
            }

            /// <summary>
            /// Выполнение вспомогательных методов с атрибутом <see cref="AfterAttribute"/>.
            /// </summary>
            /// <param name="metadata">Информация о тестовом методе.</param>
            /// <returns>True, если выполнение прошло успешно.</returns>
            private bool AfterMethodsExecution(TestMetadata metadata)
            {
                try
                {
                    lock (metadata.Methods.Locker)
                    {
                        MethodsExecution(metadata.Methods.AfterMethods);
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
            /// Вывод инофрмации о результатах выполнения тестовых методов текущего типа.
            /// </summary>
            public void DisplayResults()
            {
                foreach (var method in TestMethods)
                {
                    method.Display();
                }
            }

            /// <summary>
            /// Сохранение информации о результатах выполнения тестовых методов текущего типа.
            /// </summary>
            /// <returns>Экземляр <see cref="TestMethodsInTypeExecutionInfo"/>
            /// для текущего типа, в который сохраняется информация.</returns>
            public TestMethodsInTypeExecutionInfo SaveResults()
            {
                var executionInfo = new TestMethodsInTypeExecutionInfo(InstanceOfType, Type);

                foreach (var method in TestMethods)
                {
                    method.Save(executionInfo);
                }

                return executionInfo;
            }

            /// <summary>
            /// Выполнение вспомогательных методов.
            /// </summary>
            /// <param name="methods">Коллекция данных, содержащих информацию о
            /// некотором классе вспомогательных методов.</param>
            private void MethodsExecution(IEnumerable<Metadata> methods)
            {
                foreach (var method in methods)
                {
                    try
                    {
                        method.MethodInfo.Invoke(method.Methods.InstanceOfType, null);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
        }
    }
}
