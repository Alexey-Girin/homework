namespace SetNamespace
{
    using System;

    /// <summary>
    /// Исключение - добавление уже существующего в Set элемента.
    /// </summary>
    public class AddSetExeption : ApplicationException
    {
        /// <summary>
        /// Конструктор экземпляра класса <see cref="AddSetExeption"/>.
        /// </summary>
        public AddSetExeption()
        {
        }

        /// <summary>
        /// Конструктор экземпляра класса <see cref="AddSetExeption"/>.
        /// </summary>
        /// <param name="message"></param>
        public AddSetExeption(string message)
            : base(message)
        {
        }
    }
}
