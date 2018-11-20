using System;

namespace MyNUnit.SupportingClasses
{
    /// <summary>
    /// Информация о результате выполнения теста со статусом Ignore.
    /// </summary>
    public class IgnoreTestExecutionInfo : ITestExecutionInfo, IComparable<IgnoreTestExecutionInfo>
    {
        /// <summary>
        /// Результат теста.
        /// </summary>
        public string Result { get; } = "Ignore";

        /// <summary>
        /// Имя теста.
        /// </summary>
        public string Name { get; }
        
        /// <summary>
        /// Причина игнорирования теста.
        /// </summary>
        public string Reason { get; }

        /// <summary>
        /// Конструктор экземпляра класса <see cref="IgnoreTestExecutionInfo"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="reason"></param>
        public IgnoreTestExecutionInfo(string name, string reason)
        {
            Name = name;
            Reason = reason;
        }

        /// <summary>
        /// Метод, сравнивающий текущий экземпляр с другим объектом того же типа.
        /// </summary>
        /// <param name="other">Объект для сравнения с данным экземпляром.</param>
        /// <returns>Значение, указывающее, каков относительный порядок сравниваемых объектов.</returns>
        public int CompareTo(IgnoreTestExecutionInfo other)
            => string.Compare(Name, other.Name);
    }
}
