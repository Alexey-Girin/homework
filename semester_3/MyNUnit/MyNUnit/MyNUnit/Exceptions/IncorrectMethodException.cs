using System;
using System.Reflection;

namespace MyNUnit.Exceptions
{
    /// <summary>
    /// Представляет ошибку некорректной записи метода.
    /// </summary>
    public class IncorrectMethodException : Exception
    {
        public string IncorrectMethodName { get; }

        public IncorrectMethodException(string methodName)
        {
            IncorrectMethodName = methodName;
        }
    }
}