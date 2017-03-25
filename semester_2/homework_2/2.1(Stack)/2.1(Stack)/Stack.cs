namespace StackNamespace
{
    using System;

    /// <summary>
    /// Стек на списках.
    /// </summary>
    public class Stack
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

        public Stack()
        {
            this.head = null;
        }

        /// <summary>
        /// Добавление значение в стек.
        /// </summary>
        /// <param name="value">Добавляемое значение.</param>
        public void Push(int value)
        {
            StackElement newElement = new StackElement(value, head);
            head = newElement;
        }

        /// <summary>
        /// Иъятие головного элемента стека.
        /// </summary>
        /// <returns>Головной элемент.</returns>
        public int Pop()
        {
            if (IsEmpty())
            {
                throw new Exception("стек пуст");
            }

            int popElement = head.Value;
            head = head.Next;
            return popElement;
        }

        /// <summary>
        /// Чтение головного элемента стека. 
        /// </summary>
        /// <returns>Головной элемент.</returns>
        public int Peek()
        {
            if (IsEmpty())
            {
                throw new Exception("стек пуст");
            }

            return head.Value;
        }

        /// <summary>
        /// Проверка стека на пустоту.
        /// </summary>
        /// <returns>True если пуст.</returns>
        public bool IsEmpty()
        {
            return head == null;
        }
    }
}
