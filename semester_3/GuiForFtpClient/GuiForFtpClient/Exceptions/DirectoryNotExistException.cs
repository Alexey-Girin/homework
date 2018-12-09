using System;

namespace SimpleFTP_Client.Exceptions
{
    /// <summary>
    /// Исключение - директории не существует.
    /// </summary>
    public class DirectoryNotExistException : Exception
    {
        /// <summary>
        /// Конструктор экземпляра класса <see cref="DirectoryNotExistException"/>.
        /// </summary>
        public DirectoryNotExistException()
        {
        }

        /// <summary>
        /// Конструктор экземпляра класса <see cref="DirectoryNotExistException"/>.
        /// </summary>
        /// <param name="message">Сообщение об исключении.</param>
        public DirectoryNotExistException(string message)
            : base(message)
        {
        }
    }
}
