namespace ListNamespace
{
    using System;

    /// <summary>
    /// Список. 
    /// </summary>
    public class List
    {
        /// <summary>
        /// Элемент списка.
        /// </summary>
        private class ListElement
        {
            public string Value;
            public ListElement Next;

            public ListElement(string value, ListElement next)
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
        public void Add(string value)
        {
            ListElement newElement = new ListElement(value, head);
            head = newElement;
        }

        /// <summary>
        /// Проверка списка на пустоту.
        /// </summary>
        /// <returns>True если пуст</returns>
        public bool IsEmpty() => head == null;

        /// <summary>
        /// Удаление значение из списка.
        /// </summary>
        /// <param name="value">Удаляемое значение.</param>
        public void Delete(string value)
        {
            if (head == null)
            {
                throw new Exception("элемент не найден");
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

            throw new Exception("элемент не найден");
        }

        /// <summary>
        /// Проверка на принадлежность списку.
        /// </summary>
        /// <param name="value">Проверяемое значение.</param>
        /// <returns>True если принадлежит.</returns>
        public bool IsBelong(string value)
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
