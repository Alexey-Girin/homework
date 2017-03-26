namespace StackCalculator
{
    using System;

    /// <summary>
    /// Стек на массиве. 
    /// </summary>
    public class StackOnArray : IStack
    {
        private int size = 32;
        private int head;
        public double[] mas;

        public StackOnArray()
        {
            this.head = 0;
            this.mas = new double[size]; 
        }

        /// <summary>
        /// Добавить значение в стек.
        /// </summary>
        /// <param name="value">Добавляемое значение.</param>
        public void Push(double value)
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
        /// <returns>Изъятое значение.</returns>
        public double Pop()
        {
            if (head == 0)
            {
                throw new Exception("Попытка изъять элемент из пустого стека");
            }

            double value = mas[head - 1];
            head--;
            return value;

        }

        /// <summary>
        /// Прочитать головной элемент стека.
        /// </summary>
        /// <returns>Головное значение стека.</returns>
        public double Peek()
        {
            if (head == 0)
            {
                throw new Exception("Попытка прочитать голвной элемент пустого стека");
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

        /// <summary>
        /// Размер стека.
        /// </summary>
        /// <returns>Число элементов стека.</returns>
        public int Size()
        {
            return head;
        }
    }
}
