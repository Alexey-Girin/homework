using System;
using System.Threading.Tasks;

namespace MyNUnit
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = @"C:\Users\Алексей\Desktop\homework\semester_3\" +
                @"homework_4\MyNUnit\TestingProject\bin\Debug";

            TestingSystem.RunTests(path);
        }

        public static void A(int x) { Console.WriteLine(x); }
    }
}
