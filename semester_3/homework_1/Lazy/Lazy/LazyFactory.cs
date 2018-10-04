namespace Lazy
{
    using System;

    /// <summary>
    /// Создает экземпляры классов <see cref="Lazy{T}"/>,
    /// <see cref="MultiThreadedLazy{T}"/>.
    /// </summary>
    public static class LazyFactory
    {
        /// <summary>
        /// Метод, создающий экземпляр класса <see cref="Lazy{T}"/>. 
        /// </summary>
        /// <typeparam name="T">Тип результата вычисления.</typeparam>
        /// <param name="supplier">Вычисление.</param>
        /// <returns>Экземпляр класса <see cref="Lazy{T}"/>.</returns>
        public static ILazy<T> CreateSingleThreadedLazy<T>(Func<T> supplier)
            => new Lazy<T>(supplier);

        /// <summary>
        /// Метод, создающий экземпляр класса <see cref="MultiThreadedLazy{T}"/>. 
        /// </summary>
        /// <typeparam name="T">Тип результата вычисления.</typeparam>
        /// <param name="supplier">Вычисление.</param>
        /// <returns>Экземпляр класса <see cref="MultiThreadedLazy{T}"/>.</returns>
        public static ILazy<T> CreateMultiThreadedLazy<T>(Func<T> supplier)
            => new MultiThreadedLazy<T>(supplier);
    }
}
