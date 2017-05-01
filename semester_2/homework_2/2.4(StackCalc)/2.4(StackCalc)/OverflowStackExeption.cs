namespace StackCalculator
{
    using System;

    /// <summary>
    /// Исключение - попытка добавления элемента в переполненный стек. 
    /// </summary>
    public class OverflowStackExeption : ApplicationException
    {
        /// <summary>
        /// Конструктор экземпляра класса <see cref="OverflowStackExeption"/>.
        /// </summary>
        public OverflowStackExeption()
        {
        }

        /// <summary>
        /// Конструктор экземпляра класса <see cref="OverflowStackExeption"/>.
        /// </summary>
        /// <param name="message">Сообщение.</param>
        public OverflowStackExeption(string message)
            : base(message)
        {
        }
    }
}

