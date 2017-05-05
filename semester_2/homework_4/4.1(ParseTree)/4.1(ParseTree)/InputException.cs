namespace ParseTreeNamespace
{
    using System;

    /// <summary>
    /// Исключение - некорректное выражение.
    /// </summary>
    public class InputException : ApplicationException
    {
        /// <summary>
        /// Конструктор экземпляра класса <see cref="InputException"/>.
        /// </summary>
        public InputException()
        {
        }

        /// <summary>
        /// Конструктор экземпляра класса <see cref="InputException"/>.
        /// </summary>
        /// <param name="message"></param>
        public InputException(string message)
            : base(message)
        {
        }
    }
}
