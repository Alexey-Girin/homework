namespace ListNamespace
{
    using System;

    /// <summary>
    /// Исключение - добаление уже существующего элемента в список. 
    /// </summary>
    public class AddListException : ApplicationException
    {
        public AddListException()
        {
        }

        public AddListException(string message)
            : base(message)
        {
        }
    }
}
