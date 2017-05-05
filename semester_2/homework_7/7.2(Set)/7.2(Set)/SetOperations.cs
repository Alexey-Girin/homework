namespace SetNamespace
{
    /// <summary>
    /// Операции над множествами.
    /// </summary>
    public static class SetOperations<T>
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
}
    
 