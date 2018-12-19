using System;

namespace SimpleFtpClient.Exceptions
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

        /// <summary>
        /// Конструктор экземпляра класса <see cref="ConnectException"/>.
        /// </summary>
        /// <param name="message">Сообщение об исключении.</param>
        /// <param name="innerException">Внутреннее исключение.</param>
        public ConnectException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
