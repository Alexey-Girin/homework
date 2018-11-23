using System.Collections.Generic;
using System.Reflection;

namespace MyNUnit.SupportingClasses
{
    /// <summary>
    /// Классификации данных, содержащих информацию о методах типа.
    /// </summary>
    public class MethodsClassification
    {
        /// <summary>
        /// Коллекция тестовых методов типа.
        /// </summary>
        public List<TestMetadata> TestMethods { get; set; } = new List<TestMetadata>();

        /// <summary>
        /// Коллекция вспомогательных методов с <see cref="BeforeClassAttribute"/> типа.
        /// </summary>
        public List<MethodInfo> BeforeClassMethods { get; set; } = new List<MethodInfo>();
      
        /// <summary>
        /// Коллекция вспомогательных методов с <see cref="AfterClassAttribute"/> типа.
        /// </summary>
        public List<MethodInfo> AfterClassMethods { get; set; } = new List<MethodInfo>();

        /// <summary>
        /// Коллекция вспомогательных методов с <see cref="BeforeAttribute"/> типа.
        /// </summary>
        public List<MethodInfo> BeforeMethods { get; set; } = new List<MethodInfo>();

        /// <summary>
        /// Коллекция вспомогательных методов с <see cref="AfterAttribute"/> типа.
        /// </summary>
        public List<MethodInfo> AfterMethods { get; set; } = new List<MethodInfo>();
    }
}
