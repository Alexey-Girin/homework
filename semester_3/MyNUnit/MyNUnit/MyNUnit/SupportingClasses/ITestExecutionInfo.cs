namespace MyNUnit.SupportingClasses
{
    /// <summary>
    /// Информация о результате выполнения теста.
    /// </summary>
    public interface ITestExecutionInfo
    {
        /// <summary>
        /// Результат теста.
        /// </summary>
        string Result { get; }

        /// <summary>
        /// Имя теста.
        /// </summary>
        string Name { get; }
    }
}
