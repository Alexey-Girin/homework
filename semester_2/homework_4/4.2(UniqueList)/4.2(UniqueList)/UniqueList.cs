namespace ListNamespace
{
    using System;

    /// <summary>
    /// Список без повторяющихся значений. 
    /// </summary>
    public class UniqueList : List
    {
        public UniqueList() => this.head = null;

        /// <summary>
        /// Добавление значения в UniqueList.
        /// </summary>
        /// <param name="value">Добавляемое значение.</param>
        public override void Add(int value)
        {
            ListElement position = head;
            while (position != null)
            {
                if (position.Value == value)
                {
                    throw new AddListException("Попытка добавления существующего элемента");
                }

                position = position.Next;
            }

            ListElement newElement = new ListElement(value, head);
            head = newElement;
        }

        /// <summary>
        /// Удаление знаечния из UniqueList.
        /// </summary>
        /// <param name="value">Значение удаляемого элемента.</param>
        public override void Delete(int value)
        {
            if (head == null)
            {
                throw new DeleteListException("Попытка удаления несуществующего элемента");
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

            throw new DeleteListException("Попытка удаления несуществующего элемента");
        }
    }
}
