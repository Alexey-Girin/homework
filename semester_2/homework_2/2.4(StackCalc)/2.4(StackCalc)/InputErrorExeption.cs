namespace StackCalculator
{
    using System;

    /// <summary>
    /// Исключение - ошибка ввода. 
    /// </summary>
    public class InputErrorExeption : ApplicationException
    {
        /// <summary>
        /// Конструктор экземпляра класса <see cref="InputErrorExeption"/>.
        /// </summary>
        public InputErrorExeption()
        {
        }

        /// <summary>
        /// Конструктор экземпляра класса <see cref="InputErrorExeption"/>.
        /// </summary>
        /// <param name="message">Сообщение.</param>
        public InputErrorExeption(string message)
            : base(message)
        {
        }
    }
}
