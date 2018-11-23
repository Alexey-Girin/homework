namespace Lazy
{
    using System;

    /// <summary>
    /// Класс, представляющий ленивое вычисление (в многопоточном режиме).
    /// </summary>
    /// <typeparam name="T">Тип результата вычисления.</typeparam>
    public class MultiThreadedLazy<T> :ILazy<T>
    {
        private Func<T> func;
        private volatile bool isFirstCall = true;
        private T resultOfCalculation;
        private Object locker = new Object();

        /// <summary>
        /// Конструктор экземпляра класса <see cref="MultiThreadedLazy{T}"/>.
        /// </summary>
        /// <param name="supplier">Вычисление.</param>
        public MultiThreadedLazy(Func<T> supplier) => func = supplier;

        /// <summary>
        /// Метод, первый вызов которого вызывает вычисление,
        /// а последующие вызовы возвращают тот же объект, что и первый.
        /// </summary>
        /// <returns>Результат вычисления.</returns>
        public T Get()
        {
            if (isFirstCall)
            {
                lock (locker)
                {
                    if (isFirstCall)
                    {
                        resultOfCalculation = func();
                        isFirstCall = false;
                        func = null;
                    }
                }
            }

            return resultOfCalculation;
        }
    }
}
