using System;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Threading;
using MyNUnit.Exceptions;

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
                ThreadPool.QueueUserWorkItem(TestExecution, 
                    new Metadata(type, methodInfo, testAttribute));
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
            var attribute = (metaData as Metadata).Attribute;

            if ((attribute as TestAttribute).Ignore != null)
            {
                lock (displayLocker)
                {
                    DisplayReasonForIgnoring($"{type.Namespace}.{type.Name}.{methodInfo.Name}\t",
                    attribute);
                }

                FinishTestExecution();
                return;
            }

            Exception exception = null;
            bool isCatchException = false;
            var stopwatch = new Stopwatch();

            try
            {
                stopwatch.Start();
                methodInfo.Invoke(Activator.CreateInstance(type), null);
                stopwatch.Stop();
            }
            catch (Exception methodException)
            {
                isCatchException = true; 

                if (methodException.InnerException.GetType() != 
                    (attribute as TestAttribute).Excepted)
                {
                    exception = methodException;
                }
            }

            if (exception == null && (attribute as TestAttribute).Excepted != null &&
                !isCatchException)
            {
                exception = new ExpectedExceptionWasNotThrown();
            }

            lock (displayLocker)
            {
                DisplayTestResult($"{type.Namespace}.{type.Name}.{methodInfo.Name}\t",
                    exception, stopwatch);
            }

            FinishTestExecution();
        }

        private static void DisplayTestResult(string methodName,
            Exception exception, Stopwatch stopwatch)
        {
            Console.WriteLine($"{methodName} result: {exception == null}");

            if (exception != null)
            {
                Console.WriteLine(exception.GetType() == typeof(ExpectedExceptionWasNotThrown) ?
                    exception : exception.InnerException);
            }

            Console.WriteLine($"time: {stopwatch.ElapsedMilliseconds} ms\n");
        }

        private static void FinishTestExecution()
        {
            lock (countLocker)
            {
                countOfActiveTestExecution--;
            }

            if (enumerationIsOver && countOfActiveTestExecution == 0)
            {
                resetEvent.Set();

            }
        }

        private static void DisplayReasonForIgnoring(string methodName, Attribute attribute)
        {
            Console.WriteLine($"{methodName} result: Ignore");
            Console.WriteLine($"reason: {(attribute as TestAttribute).Ignore}\n");
        }

        private class Metadata
        {
            public Type Type { get; }
            public MethodInfo MethodInfo { get; }
            public Attribute Attribute { get; }

            public Metadata(Type type, MethodInfo methodInfo, Attribute attribute)
            {
                Type = type;
                MethodInfo = methodInfo;
                Attribute = attribute;
            }
        }
    }
}
