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
                return;
            }

            Console.WriteLine();
            TestingSystem.Launch(args[0]);
        }
    }
}
