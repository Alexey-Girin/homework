namespace StackOnListNamespace
{
    using System;
    using StackNamespace;

    /// <summary>
    /// Стек на списке. 
    /// </summary>
    public class StackOnList : Stack
    {
        private class StackElement
        {
            public int Value;
            public StackElement Next;

            public StackElement(int value, StackElement next)
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
        public void Push(int value)
        {
            StackElement newElement = new StackElement(value, head);
            head = newElement;
        }

        /// <summary>
        /// Взять значение из стека.
        /// </summary>
        /// <returns>Возвращеаемое значение.</returns>
        public int Pop()
        {
            if (IsEmpty())
            {
                return -1;
            }

            int popElement = head.Value;
            head = head.Next;
            return popElement;
        }

        /// <summary>
        /// Прочитать головной элемент стека.
        /// </summary>
        /// <returns>Головное значение стека.</returns>
        public int Peek()
        {
            if (IsEmpty())
            {
                return -1;
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
    }
}