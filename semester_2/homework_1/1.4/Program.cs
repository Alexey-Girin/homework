using System;

namespace _1._4
{
    class Program
    {
        static void ReadMas(ref int[,] mas, int N)
        {
            for (int i = 0; i < N; i++)
            {
                var str = (Console.ReadLine()).Split(' ');
                for (int t = 0; t < N; t++)
                {
                    mas[i, t] = int.Parse(str[t]);
                }
            }
        }

        static void WriteMas(int[,] mas, int N)
        {
            int x = (N + 1) / 2 - 1;
            int y = x;
            int step = 2;
            Console.Write(mas[x, y] + " ");
            for(int i = 0; i < (N - 1) / 2; i++)
            {
                y--;
                for (int t = 0; t < step - 1; t++)
                {
                    Console.Write(mas[x, y] + " ");
                    x++;
                }
                Console.Write(mas[x, y] + " ");

                y++;
                for (int t = 0; t < step - 1; t++)
                {
                    Console.Write(mas[x, y] + " ");
                    y++;
                }
                Console.Write(mas[x, y] + " ");

                x--;
                for (int t = 0; t < step - 1; t++)
                {
                    Console.Write(mas[x, y] + " ");
                    x--;
                }
                Console.Write(mas[x, y] + " ");

                y--;
                for (int t = 0; t < step - 1; t++)
                {
                    Console.Write(mas[x, y] + " ");
                    y--;
                }
                Console.Write(mas[x, y] + " ");

                step += 2;
            }

            Console.Write("\n");                                                                                                                                                                                
        }

        static void Main(string[] args)
        {
            Console.Write("введите N: ");
            int N = int.Parse(Console.ReadLine());

            int[,] mas = new int[N, N];
            ReadMas(ref mas, N);
            WriteMas(mas, N);
            
        }
    }
}
