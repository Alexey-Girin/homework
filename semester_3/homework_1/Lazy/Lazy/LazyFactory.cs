namespace Lazy
{
    using System;

    /// <summary>
    /// Класс, создающий экземпляр класса <see cref="Lazy{T}"/>
    /// </summary>
    public static class LazyFactory
    {
        public static Lazy<T> CreateSingleThreadedLazy<T>(Func<T> supplier)
            => new Lazy<T>(supplier);
    }
}
