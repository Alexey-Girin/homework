namespace StackCalculator
{
    using System;

    /// <summary>
    /// Стек на списке. 
    /// </summary>
    public class StackOnList : IStack
    {
        private class StackElement
        {
            public double Value;
            public StackElement Next;

            public StackElement(double value, StackElement next)
            {
                this.Next = next;
                this.Value = value;
            }
        }

        private StackElement head;

        public StackOnList()
        {
            this.head = null;
        }

        /// <summary>
        /// Добавить значение в стек.
        /// </summary>
        /// <param name="value">Добавляемое значение.</param>
        public void Push(double value)
        {
            StackElement newElement = new StackElement(value, head);
            head = newElement;
        }

        /// <summary>
        /// Взять значение из стека.
        /// </summary>
        /// <returns>Изъятое значение.</returns>
        public double Pop()
        {
            if (IsEmpty())
            {
                throw new Exception("Попытка изъять элемент из пустого стека");
            }

            double popElement = head.Value;
            head = head.Next;
            return popElement;
        }

        /// <summary>
        /// Прочитать головной элемент стека.
        /// </summary>
        /// <returns>Головное значение стека.</returns>
        public double Peek()
        {
            if (IsEmpty())
            {
                throw new Exception("Попытка прочитать голвной элемент пустого стека");
            }

            return head.Value;
        }

        /// <summary>
        /// Проверить стек на пустоту.
        /// </summary>
        /// <returns>True если пуст.</returns>
        public bool IsEmpty()
        {
            return head == null;
        }

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