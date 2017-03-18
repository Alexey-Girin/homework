using System;

namespace _1._3
{
    class Program
    {
        private static void Swap(ref int firstNum, ref int secondNum)
        {
            int num = firstNum;
            firstNum = secondNum;
            secondNum = num;
        }

        private static void ReadMas(int[] mas)
        {
            Console.Write("введите элементы массива:\n");
            var strMas = Console.ReadLine().Split(' ');
            for (int i = 0; i < mas.Length; i++)
            {
                mas[i] = int.Parse(strMas[i]);
            }
        }

        private static void WriteMas(int[] mas)
        {
            Console.Write("отсортированный массив:\n");
            for (int i = 0; i < mas.Length; i++)
            {
                Console.Write(mas[i] + " ");
            }

            Console.Write("\n");
        }

        private static void InsertionSort(int[] mas)
        {
            for (int i = 1; i < mas.Length; i++)
            {
                int currentElement = 0;
                if (mas[i] < mas[i - 1])
                {
                    int t = i - 1;
                    currentElement = mas[i];
                    while (t >= 0 && currentElement < mas[t])
                    {
                        mas[t + 1] = mas[t];
                        t--;
                    }
                    mas[t + 1] = currentElement;
                }
            }
        }

        private static void Test()
        {
            int[] mas = { 4, 6, 0, 1, 2, 2, 3, 5 };
            int[] resultMas = { 0, 1, 2, 2, 3, 4, 5, 6 };
            InsertionSort(mas);
            for (int i = 0; i < mas.Length; i++)
            {
                if (mas[i] != resultMas[i])
                {
                    Console.WriteLine("test false");
                    return;
                }
            }

            Console.WriteLine("test true");
        }
    
        private static void Main(string[] args)
        {
            Test();
            Console.WriteLine("кол-во элементов массива: ");
            int num = int.Parse(Console.ReadLine());
            int[] mas = new int[num];

            ReadMas(mas);
            InsertionSort(mas);
            WriteMas(mas);  

        }
    }
}
