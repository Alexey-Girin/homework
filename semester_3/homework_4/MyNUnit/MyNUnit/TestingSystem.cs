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
        public class TestCollections
        {
            public ConcurrentBag<string> TrueTestCollection { get; private set; } =
               new ConcurrentBag<string>();

            public ConcurrentBag<string> FalseTestCollection { get; private set; } =
                new ConcurrentBag<string>();

            public ConcurrentBag<string> IgnoreTestCollection { get; private set; } =
                new ConcurrentBag<string>();

            public ConcurrentBag<string> IndefiniteTestCollection { get; private set; } =
                new ConcurrentBag<string>();
        }

        private static TestCollections testCollections = new TestCollections();

        /// <summary>
        /// Выполнение тестов во всех сборках, расположенных по заданному пути.
        /// </summary>
        /// <param name="path">Путь, по которому выполняется обход сборок.</param>
        public static TestCollections RunTests(string path)
        {
            AssembliesEnumeration(path);
            return testCollections;
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
            Assembly assembly = Assembly.LoadFile(assemblyPath);

            foreach (var type in assembly.GetExportedTypes())
            {
                MethodsEnumeration(type);
            }
        }

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

            if (methods.TestMethods.Count != 0)
            {
                Execution(methods);
                DisplayTestResults(methods);
            }
        }

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
                    methods.AuxiliaryMethodException = exception;
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
                    methods.AuxiliaryMethodException = exception;
                }
            }
        }

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

        private static void TestExecution(TestMetadata metadata, Methods methods)
        {
            try
            {
                MethodsExecution(methods.BeforeMethods);
            }
            catch (Exception methodException)
            {
                metadata.BeforeOrAfterMethodException = methodException;
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

        private static bool AfterMethodsExecution(TestMetadata metadata, Methods methods)
        {
            try
            {
                MethodsExecution(methods.AfterMethods);
            }
            catch (Exception methodException)
            {
                metadata.BeforeOrAfterMethodException = methodException;
                return false;
            }

            return true;
        }

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

        private static void DisplayTestResults(Methods methods)
        {
            foreach (var method in methods.TestMethods)
            {
                Display(method, methods);
            }
        }

        private static void Display(TestMetadata metadata, Methods methods)
        {
            if (methods.AuxiliaryMethodException != null)
            {
                DisplayIndefinite(metadata, methods.AuxiliaryMethodException);
                testCollections.IndefiniteTestCollection.Add(GetFullNameOfMethod(metadata));
            }
            else if (metadata.BeforeOrAfterMethodException != null)
            {
                DisplayIndefinite(metadata, metadata.BeforeOrAfterMethodException);
                testCollections.IndefiniteTestCollection.Add(GetFullNameOfMethod(metadata));
            }
            else if (metadata.Attribute.Ignore != null)
            {
                DisplayIgnore(metadata);
                testCollections.IgnoreTestCollection.Add(GetFullNameOfMethod(metadata));
            }
            else
            {
                DisplayTrueFalse(metadata);

                if (metadata.TestException == null)
                {
                    testCollections.TrueTestCollection.Add(GetFullNameOfMethod(metadata));
                    return;
                }

                testCollections.FalseTestCollection.Add(GetFullNameOfMethod(metadata));
            }
        }

        private static void DisplayIndefinite(TestMetadata metadata, Exception exception)
        {
            Console.WriteLine("Result:\tIndefinite");
            Console.WriteLine($"Test:\t{GetFullNameOfMethod(metadata)}");
            Console.WriteLine($"Reason:\tброшено исключение\n{exception.InnerException}\n");
        }

        private static void DisplayIgnore(TestMetadata metadata)
        {
            Console.WriteLine("Result:\tIgnore");
            Console.WriteLine($"Test:\t{GetFullNameOfMethod(metadata)}");
            Console.WriteLine($"Reason:\t{metadata.Attribute.Ignore}\n");
        }

        private static void DisplayTrueFalse(TestMetadata metadata)
        {
            Console.WriteLine($"Result:\t{metadata.TestException == null}");
            Console.WriteLine($"Test:\t{GetFullNameOfMethod(metadata)}");

            if (metadata.TestException != null)
            {
                Console.WriteLine(metadata.TestException);
            }

            Console.WriteLine($"Time:\t{metadata.RunTime} ms\n");
        }

        private static string GetFullNameOfMethod(Metadata metadata)
            => $"{metadata.Type.Namespace}.{metadata.Type.Name}.{metadata.MethodInfo.Name}";

        private class Metadata
        {
            public Type Type { get; set; }
            public MethodInfo MethodInfo { get; set; }
            public object InstanceOfType { get; set; }
        }

        private class TestMetadata : Metadata
        {
            public TestAttribute Attribute { get; set; }
    
            public string RunTime { get; set; }
            public Exception TestException { get; set; }
            public Exception BeforeOrAfterMethodException { get; set; }
        }

        private class Methods
        {
            public List<TestMetadata> TestMethods { get; set; } = new List<TestMetadata>();
            public List<Metadata> BeforeClassMethods { get; set; } = new List<Metadata>();
            public List<Metadata> AfterClassMethods { get; set; } = new List<Metadata>();
            public ConcurrentBag<Metadata> BeforeMethods { get; set; } = new ConcurrentBag<Metadata>();
            public ConcurrentBag<Metadata> AfterMethods { get; set; } = new ConcurrentBag<Metadata>();

            public Exception AuxiliaryMethodException { get; set; }
        }
    }
}