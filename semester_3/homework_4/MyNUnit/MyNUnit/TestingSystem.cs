using System;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using MyNUnit.Exceptions;
using System.Collections.Generic;
using System.Threading.Tasks;

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
                    Attribute = null,
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

            MethodsExecution(methods.TestMethods);

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
                    metadata.Attribute = attribute;
                    methods.TestMethods.Add(metadata);
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

        private static void MethodsExecution(List<Metadata> methods)
        {
            Task[] tasks = new Task[methods.Count];

            for (int i = 0; i < methods.Count; i++)
            {
                int j = i;
                tasks[j] = Task.Factory.StartNew(() => Execution(methods[j]));
            }

            Task.WaitAll(tasks);
        }

        private static void Execution(Metadata metadata)
        {
            if (metadata.Attribute == null)
            {
                MethodExecution(metadata);
                return;
            }

            TestExecution(metadata);
        }

        private static void TestExecution(Metadata metadata)
        {
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
        }

        private static void DisplayReasonForIgnoring(Metadata metadata)
        {
            Console.WriteLine("Result:\tIgnore");
            Console.WriteLine($"Test:\t{metadata.Type.Namespace}.{metadata.Type.Name}.{metadata.MethodInfo.Name}");
            Console.WriteLine($"Reason:\t{(metadata.Attribute as TestAttribute).Ignore}\n");
        }

        private static void DisplayTestResult(Metadata metadata, Exception exception, Stopwatch stopwatch)
        {
            Console.WriteLine($"Result:\t{exception == null}");
            Console.WriteLine($"Test:\t{metadata.Type.Namespace}.{metadata.Type.Name}.{metadata.MethodInfo.Name}");

            if (exception != null)
            {
                Console.WriteLine(exception.GetType() == typeof(ExpectedExceptionWasNotThrown) ?
                    exception : exception.InnerException);
            }

            Console.WriteLine($"Time:\t{stopwatch.ElapsedMilliseconds} ms\n");
        }

        private static void MethodExecution(Metadata metadata)
            => metadata.MethodInfo.Invoke(metadata.InstanceOfType, null);

        private class Metadata
        {
            public Type Type { get; set; }
            public MethodInfo MethodInfo { get; set; }
            public Attribute Attribute { get; set; }
            public object InstanceOfType { get; set; }
        }

        private class Methods
        {
            public List<Metadata> TestMethods { get; set; } = new List<Metadata>();
            public List<Metadata> BeforeClassMethods { get; set; } = new List<Metadata>();
            public List<Metadata> AfterClassMethods { get; set; } = new List<Metadata>();
            public List<Metadata> BeforeMethods { get; set; } = new List<Metadata>();
            public List<Metadata> AfterMethods { get; set; } = new List<Metadata>();
        }
    }
}
