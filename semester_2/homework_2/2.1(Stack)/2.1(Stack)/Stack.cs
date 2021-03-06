﻿namespace StackNamespace
{
    /// <summary>
    /// Стек на списках.
    /// </summary>
    public class Stack
    {
        /// <summary>
        /// Элемент стека.
        /// </summary>
        private class StackElement
        {
            /// <summary>
            /// Значение элемента стека.
            /// </summary>
            public int Value { get; set; }

            /// <summary>
            /// Следующий за данным элемент стека.
            /// </summary>
            public StackElement Next { get; set; }

            /// <summary>
            /// Конструктор экземпляра класса <see cref="StackElement"/>.
            /// </summary>
            /// <param name="value"></param>
            /// <param name="next"></param>
            public StackElement(int value, StackElement next)
            {
                this.Next = next;
                this.Value = value;
            }
        }

        /// <summary>
        /// Головной элемент стека.
        /// </summary>
        private StackElement head;

        /// <summary>
        /// Конструктор экземпляра класса <see cref="Stack"/>.
        /// </summary>
        public Stack()
        {
        }

        /// <summary>
        /// Добавление элемента в стек.
        /// </summary>
        /// <param name="value">Значение добавляемого элемента.</param>
        public void Push(int value)
        {
            StackElement newElement = new StackElement(value, head);
            head = newElement;
        }

        /// <summary>
        /// Иъятие головного элемента стека.
        /// </summary>
        /// <returns>Значение головного элемента.</returns>
        public int Pop()
        {
            if (IsEmpty())
            {
                throw new EmptyStackExeption("попытка обращения к элементу путого стека");
            }

            int popElement = head.Value;
            head = head.Next;
            return popElement;
        }

        /// <summary>
        /// Чтение головного элемента стека. 
        /// </summary>
        /// <returns>Значение головного элемента.</returns>
        public int Peek()
        {
            if (IsEmpty())
            {
                throw new EmptyStackExeption("попытка обращения к элементу путого стека");
            }

            return head.Value;
        }

        /// <summary>
        /// Проверка стека на пустоту.
        /// </summary>
        /// <returns>True если пуст.</returns>
        public bool IsEmpty() => head == null;
    }
}
