namespace _7thHomework.Task1
{
    using System;

    /// <summary>
    /// Стек на списках.
    /// </summary>
    public class GenericStack<T>
    {
        private class StackElement
        {
            public T Data;
            public StackElement Next;

            public StackElement(T value, StackElement next)
            {
                this.Next = next;
                this.Data = value;
            }
        }

        private StackElement head;

        public GenericStack() => head = null;

        /// <summary>
        /// Добавление значения в стек.
        /// </summary>
        /// <param name="value">Добавляемое значение.</param>
        public void Push(T data)
        {
            StackElement newElement = new StackElement(data, head);
            head = newElement;
        }

        /// <summary>
        /// Иъятие головного элемента стека.
        /// </summary>
        /// <returns>Головной элемент.</returns>
        public T Pop()
        {
            if (IsEmpty())
            {
                throw new Exception("стек пуст");
            }

            T popElement = head.Data;
            head = head.Next;
            return popElement;
        }

        /// <summary>
        /// Чтение головного элемента стека. 
        /// </summary>
        /// <returns>Головной элемент.</returns>
        public T Peek()
        {
            if (IsEmpty())
            {
                throw new Exception("стек пуст");
            }

            return head.Data;
        }

        /// <summary>
        /// Проверка стека на пустоту.
        /// </summary>
        /// <returns>True если пуст.</returns>
        public bool IsEmpty() => head == null;

        /// <summary>
        /// Проверка на принадлежность значения списку.
        /// </summary>
        /// <param name="data">Проверяемое значение.</param>
        /// <returns>true если принадлежит.</returns>
        public bool IsBelong(T data)
        {
            StackElement position = head;

            while (position != null)
            {
                if (Equals(position.Data, data))
                {
                    return true;
                }

                position = position.Next;
            }

            return false;
        }
    }
}
