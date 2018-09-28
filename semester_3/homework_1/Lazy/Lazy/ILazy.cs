namespace Lazy
{
    /// <summary>
    /// Интерфейс, представляющий ленивое вычисление.
    /// </summary>
    /// <typeparam name="T">Тип результата вычисления.</typeparam>
    public interface ILazy<T>
    {
        /// <summary>
        /// Метод, вызывающий вычисление.
        /// </summary>
        /// <returns>Результат вычисления.</returns>
         T Get();
    }
}
