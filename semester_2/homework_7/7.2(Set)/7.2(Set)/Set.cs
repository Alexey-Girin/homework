namespace SetNamespace
{
    using ListNamespace;

    /// <summary>
    /// Множество на списке.
    /// </summary>
    /// <typeparam name="T">Тип элементов множетсва.</typeparam>
    public partial class Set<T>
    {
        /// <summary>
        /// Список, реализующий множество.
        /// </summary>
        public List<T> List { get; private set; }

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
                throw new AddSetExeption("добавление уже существующего в Set элемента");
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
