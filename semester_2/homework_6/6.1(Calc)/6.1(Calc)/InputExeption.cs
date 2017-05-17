namespace CalcNamespace
{
    using System;

    /// <summary>
    /// Исключение - ошибка ввода.
    /// </summary>
    public class InputExeption : ApplicationException
    {
        /// <summary>
        /// Конструктор экземпляра класса <see cref="InputExeption"/>.
        /// </summary>
        public InputExeption()
        {
        }

        /// <summary>
        /// Конструктор экземпляра класса <see cref="InputExeption"/>.
        /// </summary>
        /// <param name="message"></param>
        public InputExeption(string message)
            : base(message)
        {
        }
    }
}
