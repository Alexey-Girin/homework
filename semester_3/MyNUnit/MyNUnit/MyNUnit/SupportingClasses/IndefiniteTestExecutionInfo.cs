using System;
using MyNUnit.Exceptions;

namespace MyNUnit.SupportingClasses
{
    /// <summary>
    /// Информация о результате выполнения теста со статусом Indefinite.
    /// </summary>
    public class IndefiniteTestExecutionInfo : ITestExecutionInfo, IComparable<IndefiniteTestExecutionInfo>
    {
        /// <summary>
        /// Результат теста.
        /// </summary>
        public string Result { get; } = "Indefinite";

        /// <summary>
        /// Имя теста.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Исключение вспомогательного метода, либо <see cref="IncorrectMethodException"/> для теста.
        /// </summary>
        public Exception Exception { get; }

        /// <summary>
        /// Конструктор экземпляра класса <see cref="IndefiniteTestExecutionInfo"/>.
        /// </summary>
        /// <param name="name">Имя теста.</param>
        /// <param name="exception">Исключение вспомогательного метода,
        /// либо <see cref="IncorrectMethodException"/> для теста.</param>
        public IndefiniteTestExecutionInfo(string name, Exception exception)
        {
            Name = name;
            Exception = exception;
        }

        /// <summary>
        /// Метод, сравнивающий текущий экземпляр с другим объектом того же типа.
        /// </summary>
        /// <param name="other">Объект для сравнения с данным экземпляром.</param>
        /// <returns>Значение, указывающее, каков относительный порядок сравниваемых объектов.</returns>
        public int CompareTo(IndefiniteTestExecutionInfo other)
            => string.Compare(Name, other.Name);
    }
}
