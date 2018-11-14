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
    public static class TestingSystem
    {
        private static object displayLocker = new object();

        public static void RunTests(string path)
        {
            foreach (var assemblyPath in Directory.GetFiles(path, "*.exe"))
            {
                TypesEnumeration(assemblyPath);
            }
        }

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

            if (methods.TestMethods.Count == 0)
            {
                return;
            }

            if (methods.BeforeClassMethods.Count != 0)
            {
                MethodsExecution(methods.BeforeClassMethods);
            }

            TestsExecution(methods);

            if (methods.AfterClassMethods.Count != 0)
            {
                MethodsExecution(methods.AfterClassMethods);
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
            MethodsExecution(methods.BeforeMethods);

            if ((metadata.Attribute as TestAttribute).Ignore != null)
            {
                lock (displayLocker)
                {
                    DisplayReasonForIgnoring(metadata);
                }

                return;
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

            lock (displayLocker)
            {
                DisplayTestResult(metadata, exception, stopwatch);
            }

            MethodsExecution(methods.AfterMethods);
        }

        private static void DisplayReasonForIgnoring(TestMetadata metadata)
        {
            Console.WriteLine("Result:\tIgnore");
            Console.WriteLine($"Test:\t{metadata.Type.Namespace}.{metadata.Type.Name}." +
                $"{metadata.MethodInfo.Name}");
            Console.WriteLine($"Reason:\t{(metadata.Attribute as TestAttribute).Ignore}\n");
        }

        private static void DisplayTestResult(TestMetadata metadata, Exception exception,
            Stopwatch stopwatch)
        {
            Console.WriteLine($"Result:\t{exception == null}");
            Console.WriteLine($"Test:\t{metadata.Type.Namespace}.{metadata.Type.Name}." +
                $"{metadata.MethodInfo.Name}");

            if (exception != null)
            {
                Console.WriteLine(exception.GetType() == typeof(ExpectedExceptionWasNotThrown) ?
                    exception : exception.InnerException);
            }

            Console.WriteLine($"Time:\t{stopwatch.ElapsedMilliseconds} ms\n");
        }

        private static void MethodsExecution(List<Metadata> methods)
        {
            foreach (var method in methods)
            {
                method.MethodInfo.Invoke(method.InstanceOfType, null);
            }
        }

        private static void MethodsExecution(BlockingCollection<Metadata> methods)
        {
            foreach (var method in methods)
            {
                method.MethodInfo.Invoke(method.InstanceOfType, null);
            }
        }

        private class Metadata
        {
            public Type Type { get; set; }
            public MethodInfo MethodInfo { get; set; }
            public object InstanceOfType { get; set; }
        }

        private class TestMetadata : Metadata
        {
            public Attribute Attribute { get; set; }
        }

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
