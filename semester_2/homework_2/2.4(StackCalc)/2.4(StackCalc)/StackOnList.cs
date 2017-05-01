namespace StackCalculator
{
    /// <summary>
    /// Стек на списке. 
    /// </summary>
    public class StackOnList : IStack
    {
        /// <summary>
        /// Элемент <see cref="StackOnList"/>.
        /// </summary>
        private class StackElement
        {
            public double Value { get; set; }
            public StackElement Next { get; set; }

            /// <summary>
            /// Конструктор экземпляра класса <see cref="StackElement"/>.
            /// </summary>
            /// <param name="value">Значение элемента.</param>
            /// <param name="next">Следующий за данным элемент.</param>
            public StackElement(double value, StackElement next)
            {
                this.Next = next;
                this.Value = value;
            }
        }

        private StackElement head;

        /// <summary>
        /// Конструктор экземпляра класса <see cref="StackOnList"/>.
        /// </summary>
        public StackOnList()
        {
        }

        /// <summary>
        /// Добавление элемента в стек.
        /// </summary>
        /// <param name="value">Значение добавляемого элемента.</param>
        public void Push(double value)
        {
            StackElement newElement = new StackElement(value, head);
            head = newElement;
        }

        /// <summary>
        /// Изъятие головного элемента стека.
        /// </summary>
        /// <returns>Значение головного элемента стека.</returns>
        public double Pop()
        {
            if (IsEmpty())
            {
                throw new EmptyStackExeption("попытка обращения к элементу путого стека");
            }

            double popElement = head.Value;
            head = head.Next;
            return popElement;
        }

        /// <summary>
        /// Получение значения головного элемента стека.
        /// </summary>
        /// <returns>Значение головного элемента стека.</returns>
        public double Peek()
        {
            if (IsEmpty())
            {
                throw new EmptyStackExeption("попытка обращения к элементу путого стека");
            }

            return head.Value;
        }

        /// <summary>
        /// Проверить стек на пустоту.
        /// </summary>
        /// <returns>True если пуст.</returns>
        public bool IsEmpty() => head == null;

        /// <summary>
        /// Размер стека.
        /// </summary>
        /// <returns>Число элементов стека.</returns>
        public int Size()
        {
            int stackSize = 0;

            StackElement position = head;
            while (position != null)
            {
                position = position.Next;
                stackSize++;
            }

            return stackSize;
        }
    }
}