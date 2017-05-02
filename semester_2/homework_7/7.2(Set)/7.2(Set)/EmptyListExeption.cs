namespace ListNamespace
{
    using System;

    /// <summary>
    /// Исключение - попытка обращения к элементу путого списка.
    /// </summary>
    public class EmptyListExeption : ApplicationException
    {
        /// <summary>
        /// Конструктор экземпляра класса <see cref="EmptyListExeption"/>.
        /// </summary>
        public EmptyListExeption()
        {
        }

        /// <summary>
        /// Конструктор экземпляра класса <see cref="EmptyListExeption"/>.
        /// </summary>
        /// <param name="message"></param>
        public EmptyListExeption(string message)
            : base(message)
        {
        }
    }
}
