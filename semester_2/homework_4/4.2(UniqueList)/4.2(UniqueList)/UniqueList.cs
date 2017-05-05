namespace ListNamespace
{
    /// <summary>
    /// Список без повторяющихся значений. 
    /// </summary>
    public class UniqueList<T> : List<T>
    {
        /// <summary>
        /// Конструктор экземпляра класса <see cref="UniqueList"/>.
        /// </summary>
        public UniqueList()
            :base()
        {
        }

        /// <summary>
        /// Добавление элемента в <see cref="UniqueList{T}"/>.
        /// </summary>
        /// <param name="value">Значение добавляемого элемента.</param>
        /// <param name="addPosition">Позиция добавляемого элемента.</param>
        public override void Add(T value, int addPosition)
        {
            if(IsBelong(value))
            {
                throw new AddListException("добаление уже существующего элемента в список");
            }

            base.Add(value, addPosition);
        }
    }
}
