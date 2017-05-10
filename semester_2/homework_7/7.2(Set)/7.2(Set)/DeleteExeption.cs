namespace ListNamespace
{
    using System;

    /// <summary>
    /// Исключение - удаление несуществующего элемента.
    /// </summary>
    public class DeleteExeption : ApplicationException
    {
        /// <summary>
        /// Конструктор экземпляра класса <see cref="DeleteExeption"/>.
        /// </summary>
        public DeleteExeption()
        {
        }

        /// <summary>
        /// Конструктор экземпляра класса <see cref="DeleteExeption"/>.
        /// </summary>
        /// <param name="message">Сообщение.</param>
        public DeleteExeption(string message)
            : base(message)
        {
        }
    }
}

