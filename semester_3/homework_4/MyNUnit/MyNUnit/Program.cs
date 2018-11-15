namespace MyNUnit
{
    using System;

    class Program
    {
        static void Main(string[] args)
        {
            //if (args.Length == 0)
            //{
            //    Console.WriteLine("Путь не задан");
            //    Console.ReadKey();
            //    return;
            //}

            string path = @"C:\Users\Алексей\Desktop\homework\semester_3\homework_4\MyNUnit\TestProjects\TestProject_5\bin\Debug";
            Console.WriteLine();
            TestingSystem.RunTests(path);
            Console.ReadKey();
        }
    }
}
