namespace SetNamespace
{
    using ListNamespace;

    /// <summary>
    /// Множество на списке.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Set<T>
    {
        /// <summary>
        /// Список, реализующий множество.
        /// </summary>
        private List<T> list = new List<T>();

        /// <summary>
        /// Конструктор экземпляра класса <see cref="Set{T}"/>.
        /// </summary>
        public Set()
        {
        }

        /// <summary>
        /// Добавление элемента в множество.
        /// </summary>
        /// <param name="value">Значение добавляемого элемента.</param>
        public void Add(T value)
        {
            if (IsBelong(value))
            {
                throw new AddSetExeption("добавление уже существующего в Set элемента");
            }

            list.Add(value, 0);
        }

        /// <summary>
        /// Удаление элемента из множества.
        /// </summary>
        /// <param name="value">Значение удаляемого элемента.</param>
        public void Delete(T value) => list.Delete(value);

        /// <summary>
        /// Проверка на принадлежность множеству.
        /// </summary>
        /// <param name="value">Проверяемое значение.</param>
        /// <returns>true если принадлежит.</returns>
        public bool IsBelong(T value) => list.IsBelong(value);

        /// <summary>
        /// Объединение множества с другим множеством.
        /// </summary>
        /// <param name="setForUnion">Множество для объединения.</param>
        public void Union(Set<T> setForUnion)
        {
            foreach(T element in setForUnion.list)
            {
                if(!IsBelong(element))
                {
                    Add(element);
                }
            }
        }

        /// <summary>
        /// Пересечение множества с другим множеством.
        /// </summary>
        /// <param name="setForUnion">Множество для пересечнения.</param>
        public void Intersection(Set<T> setForUnion)
        {
            foreach (T element in list)
            {
                if (!setForUnion.IsBelong(element))
                {
                    Delete(element);
                }
            }
        }

        /// <summary>
        /// Проверка множеств на равенство.
        /// </summary>
        /// <param name="checkSet">Множество для проверки.</param>
        /// <returns>true если равны.</returns>
        public bool AreEqual(Set<T> checkSet)
        {
            foreach (T element in list)
            {
                if (!checkSet.IsBelong(element))
                {
                    return false;
                }
            }

            foreach (T element in checkSet.list)
            {
                if (!IsBelong(element))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
