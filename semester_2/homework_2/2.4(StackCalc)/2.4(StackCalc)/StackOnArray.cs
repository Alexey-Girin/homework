namespace StackOnArrayNamespace
{
    using System;
    using StackNamespace;

    /// <summary>
    /// Стек на массиве. 
    /// </summary>
    public class StackOnArray : Stack
    {
        private int size = 32;
        private int head;
        public int[] mas;

        public StackOnArray()
        {
            this.head = 0;
            this.mas = new int[size]; 
        }

        /// <summary>
        /// Добавить значение в стек.
        /// </summary>
        /// <param name="value">Добавляемое значение.</param>
        public void Push(int value)
        {
            if (head == size)
            {
                Console.WriteLine("стек переполнен");
                return;
            }

            mas[head] = value;
            head++;
        }

        /// <summary>
        /// Взять значение из стека.
        /// </summary>
        /// <returns>Возвращеаемое значение.</returns>
        public int Pop()
        {
            if (head == 0)
            {
                return -1;
            }

            int value = mas[head - 1];
            head--;
            return value;

        }

        /// <summary>
        /// Прочитать головной элемент стека.
        /// </summary>
        /// <returns>Головное значение стека.</returns>
        public int Peek()
        {
            if (head == 0)
            {
                return -1;
            }

            return mas[head - 1];
        }

        /// <summary>
        /// Проверить стек на пустоту.
        /// </summary>
        /// <returns>True если пуст.</returns>
        public bool IsEmpty()
        {
            return head == 0;
        }
    }
}
