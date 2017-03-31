namespace ListNamespace
{
    using System;

    /// <summary>
    /// Исключение - удаление несуществующего в списке элемента.
    /// </summary>
    public class DeleteListException : ApplicationException
    {
        public DeleteListException()
        {
        }

        public DeleteListException(string message)
            : base(message)
        {
        }
    }
}
