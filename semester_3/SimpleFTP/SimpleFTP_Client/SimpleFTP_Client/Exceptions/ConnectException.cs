using System;

namespace SimpleFTP_Client.Exceptions
{
    /// <summary>
    /// Исключение - не удалось подключиться к серверу.
    /// </summary>
    public class ConnectException : Exception
    {
        /// <summary>
        /// Конструктор экземпляра класса <see cref="ConnectException"/>.
        /// </summary>
        public ConnectException()
        {
        }

        /// <summary>
        /// Конструктор экземпляра класса <see cref="ConnectException"/>.
        /// </summary>
        /// <param name="message">Сообщение об исключении.</param>
        public ConnectException(string message)
            : base(message)
        {
        }
    }
}
