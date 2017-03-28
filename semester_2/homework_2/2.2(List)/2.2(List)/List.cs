namespace ListNamespace
{
    using System;

    /// <summary>
    /// Список. 
    /// </summary>
    public class List
    {
        private class ListElement
        {
            public int Value;
            public ListElement Next;

            public ListElement(int value, ListElement next)
            {
                this.Value = value;
                this.Next = next;
            }
        }

        private ListElement head;

        public List() => this.head = null;

        /// <summary>
        /// Добавление значения в список.
        /// </summary>
        /// <param name="value">Добавляемое значение.</param>
        public void Add(int value)
        {
            ListElement newElement = new ListElement(value, head);
            head = newElement;
        }

        /// <summary>
        /// Проверка списка на пустоту.
        /// </summary>
        /// <returns>True если пусть.</returns>
        public bool IsEmpty() => head == null;

        /// <summary>
        /// Удаление знаечния из списка.
        /// </summary>
        /// <param name="value">Значение удаляемого элемента.</param>
        public void Delete(int value)
        {
            if (head == null)
            {
                return;
            }

            if (head.Value == value)
            {
                head = head.Next;
                return;
            }

            ListElement position = head;
            while (position.Next != null)
            {
                if (position.Next.Value == value)
                {
                    position.Next = position.Next.Next;
                    return;
                }

                position = position.Next;
            }

            Console.WriteLine("Value not found");
        }

        /// <summary>
        /// Проверка на принадлежность значения списку.
        /// </summary>
        /// <param name="value">Проверяемое значение.</param>
        /// <returns>True если принадлежит</returns>
        public bool IsBelong(int value)
        {
            ListElement position = head;
            while (position != null)
            {
                if (position.Value == value)
                {
                    return true;
                }

                position = position.Next;
            }

            return false;
        }
    }
}
