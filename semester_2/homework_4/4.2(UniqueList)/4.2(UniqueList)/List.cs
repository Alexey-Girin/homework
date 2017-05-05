namespace ListNamespace
{
    /// <summary>
    /// Список.
    /// </summary>
    /// <typeparam name="T">Тип элементов списка.</typeparam>
    public class List<T>
    {
        /// <summary>
        /// Элемент списка.
        /// </summary>
        private class ListElement
        {
            /// <summary>
            /// Значение элемента списка.
            /// </summary>
            public T Value { get; set; }

            /// <summary>
            /// Следующий за данным элемент списка.
            /// </summary>
            public ListElement Next { get; set; }

            /// <summary>
            /// Конструктор экземпляра класса <see cref="ListElement"/>.
            /// </summary>
            /// <param name="value">Значение элемента.</param>
            /// <param name="next">Следующий за данным элемент.</param>
            public ListElement(T value, ListElement next)
            {
                this.Value = value;
                this.Next = next;
            }
        }

        /// <summary>
        /// Головной элемент списка.
        /// </summary>
        private ListElement head;

        /// <summary>
        /// Размер списка;
        /// </summary>
        public int Size { get; private set; }

        /// <summary>
        /// Конструктор экземпляра класса <see cref="List"/>.
        /// </summary>
        public List()
        {
        }

        /// <summary>
        /// Добавление элемента в список.
        /// </summary>
        /// <param name="value">Значение добавляемого элемента.</param>
        /// <param name="addPosition">Позиция добавляемого элемента.</param>
        public virtual void Add(T value, int addPosition)
        {
            ListElement newElement;

            if (addPosition == 0)
            {
                newElement = new ListElement(value, head);
                head = newElement;
                Size++;
                return;
            }

            if (addPosition > Size || addPosition < 0)
            {
                throw new ActionPositionExeption("некорректная позиция");
            }

            ListElement position = head;

            for (int i = 0; i < addPosition - 1; i++)
            {
                position = position.Next;
            }

            newElement = new ListElement(value, position.Next);
            position.Next = newElement;
            Size++;
        }

        /// <summary>
        /// Удаление элемента из списка по позиции.
        /// </summary>
        /// <param name="deletePosition">Позиция удаляемого элемента.</param>
        public virtual void DeleteByPosition(int deletePosition)
        {
            if (Size == 0)
            {
                throw new EmptyListExeption("попытка обращения к элементу путого списка");
            }

            if (deletePosition >= Size || deletePosition < 0)
            {
                throw new ActionPositionExeption("некорректная позиция");
            }

            if (deletePosition == 0)
            {
                head = head.Next;
                Size--;
                return;
            }

            ListElement position = head;

            for (int i = 0; i < deletePosition - 1; i++)
            {
                position = position.Next;
            }

            position.Next = position.Next.Next;
            Size--;
        }

        /// <summary>
        /// Удаление элемента из списка по значению.
        /// </summary>
        /// <param name="value">Значение удаляемого элемента.</param>
        public void Delete(T value)
        {
            if (!IsBelong(value))
            {
                throw new DeleteListException("удаление несуществующего в списке элемента");
            }

            if (Equals(head.Value, value))
            {
                head = head.Next;
                Size--;
                return;
            }

            ListElement position = head;

            while (position.Next != null)
            {
                if (Equals(position.Next.Value, value))
                {
                    position.Next = position.Next.Next;
                    Size--;
                    return;
                }

                position = position.Next;
            }
        }

        /// <summary>
        /// Получение значения элемента в заданной позиции.
        /// </summary>
        /// <param name="getPosition">Позиция элемента.</param>
        /// <returns>Полученное значение элемента.</returns>
        public T Get(int getPosition)
        {
            if (Size == 0)
            {
                throw new EmptyListExeption("попытка обращения к элементу путого списка");
            }

            if (getPosition >= Size || getPosition < 0)
            {
                throw new ActionPositionExeption("некорректная позиция");
            }

            ListElement position = head;

            for (int i = 0; i < getPosition; i++)
            {
                position = position.Next;
            }

            return position.Value;
        }

        /// <summary>
        /// Изменение значения элемента в заданной позиции.
        /// </summary>
        /// <param name="value">Новое значение.</param>
        /// <param name="changePosition">Позиция изменяемого элемента.</param>
        public void Change(T value, int changePosition)
        {
            if (Size == 0)
            {
                throw new EmptyListExeption("попытка обращения к элементу путого списка");
            }

            if (changePosition >= Size || changePosition < 0)
            {
                throw new ActionPositionExeption("некорректная позиция");
            }

            ListElement position = head;

            for (int i = 0; i < changePosition; i++)
            {
                position = position.Next;
            }

            position.Value = value;
        }

        /// <summary>
        /// Проверка на принадлежность значения списку.
        /// </summary>
        /// <param name="value">Проверяемое значение.</param>
        /// <returns>true если принадлежит.</returns>
        public bool IsBelong(T value)
        {
            ListElement position = head;

            while (position != null)
            {
                if(Equals(position.Value, value))
                {
                    return true;
                }

                position = position.Next;
            }

            return false;
        }
    }
}