using System.Collections.Generic;

namespace MyNUnit.SupportingClasses
{
    /// <summary>
    /// Информация о результатах выполнения тестов.
    /// </summary>
    public class TestsExecutionInfo
    {
        /// <summary>
        ///  Информация о результатах выполнения тестов со статусом True.
        /// </summary>
        public List<DefaultTestExecutionInfo> TrueTests { get; set; } = new List<DefaultTestExecutionInfo>();

        /// <summary>
        ///  Информация о результатах выполнения тестов со статусом False.
        /// </summary>
        public List<DefaultTestExecutionInfo> FalseTests { get; set; } = new List<DefaultTestExecutionInfo>();

        /// <summary>
        ///  Информация о результатах выполнения тестов со статусом Ignore.
        /// </summary>
        public List<IgnoreTestExecutionInfo> IgnoreTests { get; set; } = new List<IgnoreTestExecutionInfo>();

        /// <summary>
        ///  Информация о результатах выполнения тестов со статусом Indefinite.
        /// </summary>
        public List<IndefiniteTestExecutionInfo> IndefiniteTests { get; set; } = new List<IndefiniteTestExecutionInfo>();
    }
}
