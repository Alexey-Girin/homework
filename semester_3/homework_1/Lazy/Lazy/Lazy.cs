namespace Lazy
{
    using System;

    /// <summary>
    /// Класс, представляющий ленивое вычисление (в однопоточном режиме).
    /// </summary>
    /// <typeparam name="T">Тип результата вычисления.</typeparam>
    public class Lazy<T> : ILazy<T>
    {
        private Func<T> func;
        private bool isFirstCall = true;
        private T resultOfCalculation;

        /// <summary>
        /// Конструктор экземпляра класса <see cref="Lazy{T}"/>.
        /// </summary>
        /// <param name="supplier">Вычисление.</param>
        public Lazy(Func<T> supplier) => func = supplier;

        /// <summary>
        /// Метод, первый вызов которого вызывает вычисление,
        /// а последующие вызовы возвращают тот же объект, что и первый.
        /// </summary>
        /// <returns>Результат вычисления.</returns>
        public T Get()
        {
            if (isFirstCall)
            {
                isFirstCall = false;
                resultOfCalculation = func();
                func = null;
            }

            return resultOfCalculation;
        }
    }
}
