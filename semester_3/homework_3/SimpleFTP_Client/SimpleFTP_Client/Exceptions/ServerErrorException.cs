using System;

namespace SimpleFTP_Client.Exceptions
{
    /// <summary>
    /// Исключение - ошибка исполнения запроса сервером.
    /// </summary>
    public class ServerErrorException : Exception
    {
        /// <summary>
        /// Конструктор экземпляра класса <see cref="ServerErrorException"/>.
        /// </summary>
        public ServerErrorException()
        {
        }

        /// <summary>
        /// Конструктор экземпляра класса <see cref="ServerErrorException"/>.
        /// </summary>
        /// <param name="message">Сообщение об исключении.</param>
        public ServerErrorException(string message)
            : base(message)
        {
        }
    }
}
