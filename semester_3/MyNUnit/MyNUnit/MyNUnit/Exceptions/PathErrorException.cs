using System;

namespace MyNUnit.Exceptions
{
    /// <summary>
    /// Представляет ошибку недопустимой формы пути.
    /// </summary>
    public class PathErrorException : Exception
    {
        public override string Message { get; } = "Ошибка. Путь имеет недопустимую форму";

        public PathErrorException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}