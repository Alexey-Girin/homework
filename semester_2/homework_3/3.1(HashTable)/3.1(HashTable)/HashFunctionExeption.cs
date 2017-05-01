namespace HashTableNamespace
{
    using System;

    /// <summary>
    /// Исключение - некорректная работа хеш-функции.
    /// </summary>
    public class HashFunctionExeption : ApplicationException
    {
        /// <summary>
        /// Конструктор экземпляра класса <see cref="HashFunctionExeption"/>.
        /// </summary>
        public HashFunctionExeption()
        {
        }

        /// <summary>
        /// Конструктор экземпляра класса <see cref="HashFunctionExeption"/>.
        /// </summary>
        /// <param name="message"></param>
        public HashFunctionExeption(string message)
            : base(message)
        {
        }
    }
}

