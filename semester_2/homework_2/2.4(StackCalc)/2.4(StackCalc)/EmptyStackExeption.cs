namespace StackCalculator
{
    using System;

    /// <summary>
    /// Исключение - попытка обращения к элементу путого стека. 
    /// </summary>
    public class EmptyStackExeption : ApplicationException
    {
        /// <summary>
        /// Конструктор экземпляра класса <see cref="EmptyStackExeption"/>.
        /// </summary>
        public EmptyStackExeption()
        {
        }

        /// <summary>
        /// Конструктор экземпляра класса <see cref="EmptyStackExeption"/>.
        /// </summary>
        /// <param name="message">Сообщение.</param>
        public EmptyStackExeption(string message)
            : base(message)
        {
        }
    }
}
