using System;

namespace SimpleFTP_Client
{
    public class FileDownloadError
    {
        public Exception Exception { get; }

        public string FollowUp { get; }

        public FileDownloadError(Exception exception, string message)
        {
            Exception = exception;
            FollowUp = message;
        }
    }
}
