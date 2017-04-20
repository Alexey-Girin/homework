namespace _7thHomework.Task1
{
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// Генерик-класс список. 
    /// </summary>
    /// <typeparam name="T">Тип хранимых в списке значений.</typeparam>
    public partial class GenericList<T>
    {
        /// <summary>
        /// Перечислитель элементов GenericList.
        /// </summary>
        public class ListEnumerator : IEnumerator<T>
        {
            private Node position;

            object IEnumerator.Current
            {
                get
                {
                    return Current;
                }
            }

            /// <summary>
            /// Возвращение текущего элемента списка.
            /// </summary>
            public T Current
            {
                get
                {
                    return position.Data;
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
                    position = head;
                    return true;
                }

                if (position.Next == null)
                {
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
