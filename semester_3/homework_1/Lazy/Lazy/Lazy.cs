namespace Lazy
{
    using System;

    /// <summary>
    /// Класс, представляющий ленивое вычиление.
    /// </summary>
    /// <typeparam name="T">Тип результата вычисления.</typeparam>
    public class Lazy<T> : ILazy<T>
    {
        private Func<T> func;
        private bool isFirstCall = true;
        private T resultOfCalculation;
        public Lazy(Func<T> supplier) => func = supplier;

        /// <summary>
        /// Метод, вызывающих вычисление.
        /// </summary>
        /// <returns>Результат вычисления.</returns>
        public T Get()
        {
            if(isFirstCall)
            {
                isFirstCall = false;
                resultOfCalculation = func();
            }

            return resultOfCalculation;
        }
    }
}
