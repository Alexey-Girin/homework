namespace SetNamespace
{
    using ListNamespace;

    /// <summary>
    /// Множество на списке.
    /// </summary>
    /// <typeparam name="T">Тип элементов множетсва.</typeparam>
    public class Set<T>
    {
        /// <summary>
        /// Операции над множествами.
        /// </summary>
        public static class SetOperations
        {
            /// <summary>
            /// Объединение двух множеств.
            /// </summary>
            /// <param name="firstSet">Первое множество.</param>
            /// <param name="secondSet">Второе множество.</param>
            /// <returns>Объединение firstSet и secondSet.</returns>
            public static Set<T> Union(Set<T> firstSet, Set<T> secondSet)
            {
                var resultSet = new Set<T>();

                foreach (T element in firstSet.List)
                {
                    resultSet.Add(element);
                }

                foreach (T element in secondSet.List)
                {
                    if (!firstSet.IsBelong(element))
                    {
                        resultSet.Add(element);
                    }
                }

                return resultSet;
            }


            /// <summary>
            /// Пересечние двух множеств.
            /// </summary>
            /// <param name="firstSet">Первое множество.</param>
            /// <param name="secondSet">Второе множество.</param>
            /// <returns>Пересечение firstSet и secondSet.</returns>
            public static Set<T> Intersection(Set<T> firstSet, Set<T> secondSet)
            {
                var resultSet = new Set<T>();

                foreach (T element in firstSet.List)
                {
                    if (secondSet.IsBelong(element))
                    {
                        resultSet.Add(element);
                    }
                }

                return resultSet;
            }

            /// <summary>
            /// Проверка двух множеств на равенство.
            /// </summary>
            /// <param name="firstSet">Первое множество.</param>
            /// <param name="secondSet">Второе множество.</param>
            /// <returns>true если равны.</returns>
            public static bool AreEqual(Set<T> firstSet, Set<T> secondSet)
            {
                foreach (T element in firstSet.List)
                {
                    if (!secondSet.IsBelong(element))
                    {
                        return false;
                    }
                }

                foreach (T element in secondSet.List)
                {
                    if (!firstSet.IsBelong(element))
                    {
                        return false;
                    }
                }

                return true;
            }
        }

        /// <summary>
        /// Список, реализующий множество.
        /// </summary>
        private List<T> List { get; set; }

        /// <summary>
        /// Конструктор экземпляра класса <see cref="Set{T}"/>.
        /// </summary>
        public Set()
        {
            List = new List<T>();
        }

        /// <summary>
        /// Добавление элемента в множество.
        /// </summary>
        /// <param name="value">Значение добавляемого элемента.</param>
        public void Add(T value)
        {
            if (IsBelong(value))
            {
                return;
            }

            List.Add(value, 0);
        }

        /// <summary>
        /// Удаление элемента из множества.
        /// </summary>
        /// <param name="value">Значение удаляемого элемента.</param>
        public void Delete(T value) => List.Delete(value);

        /// <summary>
        /// Проверка на принадлежность множеству.
        /// </summary>
        /// <param name="value">Проверяемое значение.</param>
        /// <returns>true если принадлежит.</returns>
        public bool IsBelong(T value) => List.IsBelong(value);
    }
}
