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
    public static class TestingSystem
    {
        private static List<TestMethodsInTypeExecutionInfo> TestsExecutionInfo;

        private static object locker = new object();

        public static List<TestMethodsInTypeExecutionInfo> RunTests(string path)
        {
            TestsExecutionInfo = new List<TestMethodsInTypeExecutionInfo>();

            AssembliesEnumeration(path);

            TestsExecutionInfo.Sort();
            return TestsExecutionInfo;
        }

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
                DisplayResults(methods);

                var executionInfo = new TestMethodsInTypeExecutionInfo(instanceOfType, type);

                SaveResults(methods, executionInfo);
                TestsExecutionInfo.Add(executionInfo);
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

        private static bool AfterMethodsExecution(TestMetadata metadata, Methods methods)
        {
            try
            {
                MethodsExecution(methods.AfterMethods);
            }
            catch (Exception methodException)
            {
                metadata.AuxiliaryMethodException = methodException;
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
                    lock (locker)
                    {
                        method.MethodInfo.Invoke(method.InstanceOfType, null);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        private static void DisplayResults(Methods methods)
        {
            foreach (var method in methods.TestMethods)
            {
                method.Display();
            }
        }

        private static void SaveResults(Methods methods, TestMethodsInTypeExecutionInfo executionInfo)
        {
            foreach (var method in methods.TestMethods)
            {
                method.Save(executionInfo);
            }
        }

        private static string GetFullNameOfMethod(Metadata metadata)
            => $"{metadata.Type.Namespace}.{metadata.Type.Name}.{metadata.MethodInfo.Name}";

        private static void ExceptionNotification(Methods methods, Exception exception)
        {
            foreach (var test in methods.TestMethods)
            {
                test.AuxiliaryMethodException = exception;
            }
        }

        private class Metadata
        {
            public Type Type { get; set; }
            public object InstanceOfType { get; set; }
            public MethodInfo MethodInfo { get; set; }
        }

        private class TestMetadata : Metadata
        {
            private TestAttribute attribute;
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

            public string RunTime { get; set; }
            public Exception TestException { get; set; }

            private Exception auxiliaryMethodException;
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

            public void Display() => status.Display(this);
            public void Save(TestMethodsInTypeExecutionInfo executionInfo) => status.Save(this, executionInfo);
        }

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
        }

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

        private class Methods
        {
            public List<TestMetadata> TestMethods { get; set; } = new List<TestMetadata>();
            public List<Metadata> BeforeClassMethods { get; set; } = new List<Metadata>();
            public List<Metadata> AfterClassMethods { get; set; } = new List<Metadata>();
            public ConcurrentBag<Metadata> BeforeMethods { get; set; } = new ConcurrentBag<Metadata>();
            public ConcurrentBag<Metadata> AfterMethods { get; set; } = new ConcurrentBag<Metadata>();
        }
    }
}