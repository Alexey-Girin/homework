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

        /// <summary>
        /// Массив для <see cref="StackOnArray"/>.
        /// </summary>
        public double[] Mas { get; set; }

        /// <summary>
        /// Конструктор экземпляра класса <see cref="StackOnArray"/>.s
        /// </summary>
        public StackOnArray() => this.Mas = new double[size]; 

        /// <summary>
        /// Добавление элемента в стек.
        /// </summary>
        /// <param name="value">Значение добавляемого элемента.</param>
        public void Push(double value)
        {
            if (head == size)
            {
                throw new OverflowException("попытка добавления элемента в переполненный стек");
            }

            Mas[head] = value;
            head++;
        }

        /// <summary>
        /// Изъятие головного элемента стека.
        /// </summary>
        /// <returns>Значение головного элемента стека.</returns>
        public double Pop()
        {
            if (head == 0)
            {
                throw new EmptyStackExeption("попытка обращения к элементу путого стека");
            }

            double value = Mas[head - 1];
            head--;
            return value;
        }

        /// <summary>
        /// Получение значения головного элемента стека.
        /// </summary>
        /// <returns>Значение головного элемента стека.</returns>
        public double Peek()
        {
            if (head == 0)
            {
                throw new EmptyStackExeption("попытка обращения к элементу путого стека");
            }

            return Mas[head - 1];
        }

        /// <summary>
        /// Проверка стека на пустоту.
        /// </summary>
        /// <returns>True если пуст.</returns>
        public bool IsEmpty() => head == 0;

        /// <summary>
        /// Размер стека.
        /// </summary>
        /// <returns>Число элементов стека.</returns>
        public int Size() => head;
    }
}
