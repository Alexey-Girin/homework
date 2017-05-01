namespace ListNamespace
{
    using System;

    /// <summary>
    /// Исключение - некорректная позиция.
    /// </summary>
    public class ActionPositionExeption : ApplicationException
    {
        /// <summary>
        /// Конструктор экземпляра класса <see cref="ActionPositionExeption"/>.
        /// </summary>
        public ActionPositionExeption()
        {
        }

        /// <summary>
        /// Конструктор экземпляра класса <see cref="ActionPositionExeption"/>.
        /// </summary>
        /// <param name="message"></param>
        public ActionPositionExeption(string message)
            : base(message)
        {
        }
    }
}
