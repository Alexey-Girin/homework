using System;

namespace MyNUnit.SupportingClasses
{
    /// <summary>
    /// Информация о результате выполнения теста со статусом True/False.
    /// </summary>
    public class DefaultTestExecutionInfo : ITestExecutionInfo, IComparable<DefaultTestExecutionInfo>
    {
        /// <summary>
        /// Результат теста.
        /// </summary>
        public string Result
        {
            get
            {
                return (Exception == null).ToString();
            }
        }

        /// <summary>
        /// Имя теста.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Время выполнения теста.
        /// </summary>
        public long RunTime { get; }

        /// <summary>
        /// Возможное исключение.
        /// </summary>
        public Exception Exception { get; }

        /// <summary>
        /// Конструктор экземпляра класса <see cref="DefaultTestExecutionInfo"/>.
        /// </summary>
        /// <param name="name">Имя теста.</param>
        /// <param name="runTime">Время выполнения теста.</param>
        /// <param name="exception">Возможное исключение.</param>
        public DefaultTestExecutionInfo(string name, long runTime, Exception exception)
        {
            Name = name;
            RunTime = runTime;
            Exception = exception;
        }

        /// <summary>
        /// Метод, сравнивающий текущий экземпляр с другим объектом того же типа.
        /// </summary>
        /// <param name="other">Объект для сравнения с данным экземпляром.</param>
        /// <returns>Значение, указывающее, каков относительный порядок сравниваемых объектов.</returns>
        public int CompareTo(DefaultTestExecutionInfo other)
            => string.Compare(Name, other.Name);
    }
}
