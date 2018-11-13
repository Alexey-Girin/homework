using System;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Threading;

namespace MyNUnit
{
    public static class TestingSystem
    {
        private static volatile bool enumerationIsOver = false;

        private static volatile int countOfActiveTestExecution = 0;

        private static object countLocker = new object();

        private static object displayLocker = new object();

        private static ManualResetEvent resetEvent = new ManualResetEvent(true);

        public static void RunTests(string path)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            string[] assemblyPaths = Directory.GetFiles(path, "*.exe");

            foreach (var assemblyPath in assemblyPaths)
            {
                TypesEnumeration(assemblyPath);
            }

            enumerationIsOver = true;
            resetEvent.WaitOne();
        }

        private static void TypesEnumeration(string assemblyPath)
        {
            Assembly assembly = Assembly.LoadFile(assemblyPath);
            Type[] types = assembly.GetExportedTypes();

            foreach (var type in types)
            {
                MethodsEnumeration(type);
            }
        }

        private static void MethodsEnumeration(Type type)
        {
            foreach (var methodInfo in type.GetMethods())
            {
                Attribute testAttribute = null;

                foreach (var attribute in Attribute.GetCustomAttributes(methodInfo))
                {
                    if (attribute.GetType() == typeof(TestAttribute))
                    {
                        testAttribute = attribute;
                        break;
                    }
                }

                if (testAttribute == null)
                {
                    continue;
                }

                resetEvent.Reset();
                ThreadPool.QueueUserWorkItem(TestExecution, new Metadata(type, methodInfo));
            }
        }

        private static void TestExecution(object metaData)
        {
            lock (countLocker)
            {
                countOfActiveTestExecution++;
            }

            var methodInfo = (metaData as Metadata).MethodInfo;
            var type = (metaData as Metadata).Type;
            var stopwatch = new Stopwatch();

            Exception exception = null;

            try
            {
                stopwatch.Start();
                methodInfo.Invoke(Activator.CreateInstance(type), null);
                stopwatch.Stop();
            }
            catch (Exception methodException)
            {
                exception = methodException;
            }

            lock (displayLocker)
            {
                DisplayTestResult(type, methodInfo, exception, stopwatch);
            }

            lock (countLocker)
            {
                countOfActiveTestExecution--;
            }

            if (enumerationIsOver && countOfActiveTestExecution == 0)
            {
                resetEvent.Set();
            }
        }

        private static void DisplayTestResult(Type type, MethodInfo methodInfo,
            Exception exception, Stopwatch stopwatch)
        {
            Console.Write($"{type.Namespace}.{type.Name}.{methodInfo.Name}\t");
            Console.WriteLine($"result: {exception == null}");

            if (exception != null)
            {
                Console.WriteLine(exception.InnerException);
            }

            Console.WriteLine($"time: {stopwatch.ElapsedMilliseconds} ms\n");
        }

        private class Metadata
        {
            public Type Type { get; }
            public MethodInfo MethodInfo { get; }

            public Metadata(Type type, MethodInfo methodInfo)
            {
                Type = type;
                MethodInfo = methodInfo;
            }
        }
    }
}
