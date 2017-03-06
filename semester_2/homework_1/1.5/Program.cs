using System;

namespace _1._5
{
    class Program
    {
        static void ReadMas(ref int[,] mas)
        {
            int height = mas.GetLength(0);
            int length = mas.GetLength(1);

            Console.WriteLine("введите элементы массивва");
            for (int i = 0; i < height; i++)
            {
                var str = (Console.ReadLine()).Split(' ');
                for (int t = 0; t < length; t++)
                {
                    mas[i, t] = int.Parse(str[t]);
                }
            }
        }

        static int FindMin(int[,] mas, int startNum)
        {
            int length = mas.GetLength(1);
            int minElement = mas[0, startNum];
            int minNum = startNum;
            for (int i = startNum + 1; i < length; i++)
            {
                if (mas[0, i] < minElement)
                {
                    minElement = mas[0, i];
                    minNum = i;
                }
            }
        
            return minNum;
        }

        static void WriteMas(int[,] mas)
        {
            int height = mas.GetLength(0);
            int length = mas.GetLength(1);

            Console.WriteLine("результат");
            for (int i = 0; i < height; i++)
            {
                for (int t = 0; t < length; t++)
                {
                    Console.Write(mas[i, t] + " ");
                }
                Console.Write("\n");
            }
        }

        static void Sort(ref int[,] mas)
        {
            int height = mas.GetLength(0);
            int length = mas.GetLength(1);

            for (int i = 0; i < length; i++)
            {
                int minNum = FindMin(mas, i);

                if (minNum == i)
                {
                    continue;
                }

                for (int t = 0; t < height; t++)
                {
                    int element = mas[t, i];
                    mas[t, i] = mas[t, minNum];
                    mas[t, minNum] = element;
                }
            }
        }
   
        static void Main(string[] args)
        {
            Console.Write("введите размер массива: ");
            var str = (Console.ReadLine()).Split(' ');
            int height = int.Parse(str[0]);
            int length = int.Parse(str[1]);

            int[,] mas = new int[height, length];
            ReadMas(ref mas);
            Sort(ref mas);
            WriteMas(mas);
        }
    }
}
