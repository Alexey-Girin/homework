namespace StackCalculator
{
    /// <summary>
    /// Интерфейс стек.
    /// </summary>
    public interface IStack
    {
        /// <summary>
        /// Добавить значение в стек.
        /// </summary>
        /// <param name="value">Добавляемое значение.</param>
        void Push(double value);

        /// <summary>
        /// Взять значение из стека.
        /// </summary>
        /// <returns>Изъятое значение.</returns>
        double Pop();

        /// <summary>
        /// Прочитать головной элемент стека.
        /// </summary>
        /// <returns>Головное значение стека.</returns>
        double Peek();

        /// <summary>
        /// Проверить стек на пустоту.
        /// </summary>
        /// <returns>True если пуст.</returns>
        bool IsEmpty();

        /// <summary>
        /// Размер стека.
        /// </summary>
        /// <returns>Число элементов стека.</returns>
        int Size();
    }
}
