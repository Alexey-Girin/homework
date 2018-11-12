using System;
using System.IO;
using System.Reflection;

namespace MyNUnit
{
    public static class TestingSystem
    {
        public static void RunTests(string path)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(path);
            string[] assemblyPaths = Directory.GetFiles(path, "*.exe");

            foreach (var assemblyPath in assemblyPaths)
            {
                TypesEnumeration(assemblyPath);
            }
        }

        private static void TypesEnumeration(string assemblyPath)
        {
            Assembly assembly = Assembly.LoadFile(assemblyPath);
            Type[] types = assembly.GetExportedTypes();

            foreach (var type in types)
            {
                TestExecution(type);
            }
        }

        private static void TestExecution(Type type)
        {
            foreach (var methodInfo in type.GetMethods())
            {
                foreach (var attr in Attribute.GetCustomAttributes(methodInfo))
                {
                    if (attr.GetType() != typeof(TestAttribute))
                    {
                        continue;
                    }

                    methodInfo.Invoke(Activator.CreateInstance(type), null);
                    Console.WriteLine($"{type.Name}\t{methodInfo.Name}\t{attr}");
                }
            }
        }
    }
}
