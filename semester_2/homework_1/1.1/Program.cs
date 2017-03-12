using System;

namespace _1._1
{
    class Program
    {
        static int Factorial(int num)
        {
            if (num == 1)
            {
                return 1;
            }

            return num * Factorial(num - 1);
        }

        static void Test()
        {
            const int num = 5;
            const int testResult = 120;

            if (Factorial(num) == testResult)
            {
                Console.WriteLine("test true");
                return;
            }

            Console.WriteLine("test false");
        }

        static void Main(string[] args)
        {
            Test();
            Console.WriteLine("введите число");
            int num = int.Parse(Console.ReadLine());
            Console.WriteLine("факториал числа: " + Factorial(num));
        }
    }
}
