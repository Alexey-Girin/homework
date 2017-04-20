namespace _7thHomework.Task1
{
    using System;

    /// <summary>
    /// Генерик-класс список. 
    /// </summary>
    /// <typeparam name="T">Тип хранимых в списке значений.</typeparam>
    public partial class GenericList<T>
    {
        /// <summary>
        /// Элемент списка.
        /// </summary>
        private class Node
        {
            public T Data;
            public Node Next;

            public Node(T data, Node next)
            {
                Data = data;
                Next = next;
            }
        }

        static private Node head;

        public GenericList() => head = null;

        /// <summary>
        /// Метод для работы foreach в GenericList.
        /// </summary>
        /// <returns>Перечислитель элементов списка.</returns>
        public ListEnumerator GetEnumerator() => new ListEnumerator();

        /// <summary>
        /// Добавление значения в список.
        /// </summary>
        /// <param name="data">Добавляемое значение.</param>
        public void Add(T data)
        {
            Node newNode = new Node(data, head);
            head = newNode;
        }

        /// <summary>
        /// Проверка списка на пустоту.
        /// </summary>
        /// <returns>true если список пуст.</returns>
        public bool IsEmpty() => head == null;

        /// <summary>
        /// Удаление значения из списка.
        /// </summary>
        /// <param name="data">Удаляемое значение.</param>
        public void Delete(T data)
        {
            if (head == null)
            {
                throw new Exception("список пуст");
            }

            if (Equals(data, head.Data))
            {
                head = head.Next;
                return;
            }

            Node position = head;

            while (position.Next != null)
            {
                if (Equals(position.Next.Data, data))
                {
                    position.Next = position.Next.Next;
                    return;
                }

                position = position.Next;
            }

            throw new Exception("значение не найдено");
        }

        /// <summary>
        /// Проверка на принадлежность значения списку.
        /// </summary>
        /// <param name="value">true если принадлежит.</param>
        /// <returns></returns>
        public bool IsBelong(T value)
        {
            Node position = head;

            while (position != null)
            {
                if (Equals(position.Data, value))
                {
                    return true;
                }

                position = position.Next;
            }

            return false;
        }
    }
}
