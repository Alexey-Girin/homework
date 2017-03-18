using System;

namespace _1._4
{
    class Program
    {
        private static void ReadMas(int[,] mas)
        {
            int size = mas.GetLength(1);
            Console.Write("введите матрицу (разделение элементов одной строки пробелами)\n");
            for (int i = 0; i < size; i++)
            {
                var strMas = Console.ReadLine().Split(' ');
                for (int t = 0; t < size; t++)
                {
                    mas[i, t] = int.Parse(strMas[t]);
                }
            }
        }

        private static void TraversingMasSpirally(int[,] mas, int[] resultMas)
        {
            int size = mas.GetLength(1);
            int x = (size + 1) / 2 - 1;
            int y = x;
            int step = 2;

            int currentElementOfResultMas = 0;
            resultMas[currentElementOfResultMas] = mas[x, y];
            currentElementOfResultMas++;

            for (int i = 0; i < (size - 1) / 2; i++)
            {
                y--;
                for (int t = 0; t < step - 1; t++)
                {
                    resultMas[currentElementOfResultMas] = mas[x, y];
                    currentElementOfResultMas++;
                    x++;
                }
                resultMas[currentElementOfResultMas] = mas[x, y];
                currentElementOfResultMas++;

                y++;
                for (int t = 0; t < step - 1; t++)
                {
                    resultMas[currentElementOfResultMas] = mas[x, y];
                    currentElementOfResultMas++;
                    y++;
                }
                resultMas[currentElementOfResultMas] = mas[x, y];
                currentElementOfResultMas++;

                x--;
                for (int t = 0; t < step - 1; t++)
                {
                    resultMas[currentElementOfResultMas] = mas[x, y];
                    currentElementOfResultMas++;
                    x--;
                }
                resultMas[currentElementOfResultMas] = mas[x, y];
                currentElementOfResultMas++;

                y--;
                for (int t = 0; t < step - 1; t++)
                {
                    resultMas[currentElementOfResultMas] = mas[x, y];
                    currentElementOfResultMas++;
                    y--;
                }
                resultMas[currentElementOfResultMas] = mas[x, y];
                currentElementOfResultMas++;

                step += 2;
            }                                                                                                                                                                           
        }

        private static void WriteMas(int[] mas)
        {
            Console.Write("результат:\n");
            for (int i = 0; i < mas.Length; i++)
            {
                Console.Write(mas[i] + " ");
            }

            Console.Write("\n");
        }

        private static void Test()
        {
            const int size = 3;
            int[,] mas = new int[size, size] { { 1, 2, 3 }, { 4, 5, 6 }, { 7, 8, 9 } };
            int[] resultMas = new int[size * size];
            int[] trueResult = new int[size * size] { 5, 4, 7, 8, 9, 6, 3, 2, 1 };

            TraversingMasSpirally(mas, resultMas);
            const int resultSize = size * size;
            for (int i = 0; i < resultSize; i++)
            {
                if(resultMas[i] != trueResult[i])
                {
                    Console.Write("test false\n");
                    return;
                }
            }
            Console.Write("test true\n");
        }

        private static void Main(string[] args)
        {
            Test();
            Console.Write("введите size: ");
            int size = int.Parse(Console.ReadLine());

            int[,] mas = new int[size, size];
            ReadMas(mas);
            int[] resultMas = new int[size * size];
            TraversingMasSpirally(mas, resultMas);
            WriteMas(resultMas);
        }
    }
}
