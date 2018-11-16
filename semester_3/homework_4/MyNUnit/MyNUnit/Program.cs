namespace MyNUnit
{
    using System;

    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Путь не задан");
                Console.ReadKey();
                return;
            }

            Console.WriteLine();
            TestingSystem.RunTests(args[0]);
            Console.ReadKey();
        }
    }
}
