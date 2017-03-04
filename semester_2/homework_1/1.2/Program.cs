using System;

namespace _1._2
{
    class Program
    {
        static int Fibonachi(int num)
        {
            if (num == 1 || num == 2)
            {
                return 1;
            }

            int []masFib = new int[3];
            masFib[0] = 1;
            masFib[1] = 1;

            for (int i = 2; i < num; i++)
            {
                if (i % 3 == 0)
                {
                    masFib[0] = masFib[1] + masFib[2];
                    continue;
                }

                if (i % 3 == 1)
                {
                    masFib[1] = masFib[0] + masFib[2];
                    continue;
                }

                masFib[2] = masFib[0] + masFib[1];
            }

            return masFib[(num - 1) % 3];
        }

        static void Test()
        {
            int num = 10;
            int testResult = 55;
            if (Fibonachi(num) == testResult)
            {
                Console.WriteLine("test true");
                return;
            }

            Console.WriteLine("test false");
        }

        static void Main(string[] args)
        {
            Test();
            Console.WriteLine("введите номер");
            int num = int.Parse(Console.ReadLine());

            if (num < 1)
            {
                Console.WriteLine("ошибка ввода");
                return;
            }

            Console.WriteLine("число Фибоначчи: " + Fibonachi(num));
        }
    }
}
