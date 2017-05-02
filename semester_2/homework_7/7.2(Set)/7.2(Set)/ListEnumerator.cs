namespace ListNamespace
{
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// Список.
    /// </summary>
    /// <typeparam name="T">Тип хранимых в списке значений.</typeparam>
    public partial class List<T>
    {
        /// <summary>
        /// Перечислитель элементов списка.
        /// </summary>
        private class ListEnumerator : IEnumerator<T>
        {
            private ListElement position;

            private ListElement head;

            private bool isFinished;

            object IEnumerator.Current
            {
                get
                {
                    return Current;
                }
            }

            /// <summary>
            /// Конструктор экземпляра класса <see cref="ListEnumerator"/>.
            /// </summary>
            /// <param name="list"></param>
            public ListEnumerator(List<T> list)
            {
                head = list.head;

                isFinished = head == null;
            }

            /// <summary>
            /// Получение значения текущего элемента списка.
            /// </summary>
            public T Current
            {
                get
                {
                    return position.Value;
                }
            }

            /// <summary>
            /// Переход перечислителя к следущему элементу списка.
            /// </summary>
            /// <returns>true если переход успешен.</returns>
            public bool MoveNext()
            {
                if (position == null)
                {
                    if (isFinished)
                    {
                        return false;
                    }

                    position = head;
                    return true;
                }

                if (position.Next == null)
                {
                    isFinished = true;
                    position = position.Next;
                    return false;
                }

                position = position.Next;
                return true;
            }

            /// <summary>
            /// Установка перечислителя в начальное положение.
            /// </summary>
            public void Reset()
            {
                position = null;
            }

            public void Dispose()
            { 
            }
        }
    }
}
