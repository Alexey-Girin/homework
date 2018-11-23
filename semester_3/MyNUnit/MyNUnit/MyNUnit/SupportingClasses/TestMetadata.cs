using System.Reflection;

namespace MyNUnit.SupportingClasses
{
    /// <summary>
    /// Информация о тестовом методе.
    /// </summary>
    public class TestMetadata
    {
        /// <summary>
        /// Объект, обеспечивающий доступ к данным теста.
        /// </summary>
        public MethodInfo MethodInfo { get; }

        /// <summary>
        /// Атрибут теста.
        /// </summary>
        public TestAttribute Attribute { get; }

        /// <summary>
        /// Конструктор экземпляра класса <see cref="TestMetadata"/>.
        /// </summary>
        /// <param name="methodInfo">Объект, обеспечивающий доступ к данным теста.</param>
        /// <param name="attribute">Атрибут теста.</param>
        public TestMetadata(MethodInfo methodInfo, TestAttribute attribute)
        {
            MethodInfo = methodInfo;
            Attribute = attribute;
        }
    }
}
