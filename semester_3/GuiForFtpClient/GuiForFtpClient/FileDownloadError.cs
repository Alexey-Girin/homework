using System;

namespace SimpleFtpClient
{
    /// <summary>
    /// Информация об ошибке скачивания файла.
    /// </summary>
    public class FileDownloadError
    {
        /// <summary>
        /// Исключение, возникающее при скачивании файла. null, если исключение не возникло.
        /// </summary>
        public Exception Exception { get; }

        /// <summary>
        /// Дополнительная информация об ошибке.
        /// </summary>
        public string FollowUp { get; }

        /// <summary>
        /// Конструктор экзмепляра класса <see cref="FileDownloadError"/>.
        /// </summary>
        /// <param name="exception">Исключение, возникающее при скачивании файла. null,
        /// если исключение не возникло.</param>
        /// <param name="message">Дополнительная информация об ошибке.</param>
        public FileDownloadError(Exception exception, string message)
        {
            Exception = exception;
            FollowUp = message;
        }
    }
}
