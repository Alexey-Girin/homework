using System;

namespace SimpleFtpClient.Exceptions
{
    /// <summary>
    /// Исключение - файла не существует.
    /// </summary>
    public class FileNotExistException : Exception
    {
        /// <summary>
        /// Конструктор экземпляра класса <see cref="FileNotExistException"/>.
        /// </summary>
        public FileNotExistException()
        {
        }

        /// <summary>
        /// Конструктор экземпляра класса <see cref="FileNotExistException"/>.
        /// </summary>
        /// <param name="message">Сообщение об исключении.</param>
        public FileNotExistException(string message)
            :base(message)
        {
        }
    }
}
