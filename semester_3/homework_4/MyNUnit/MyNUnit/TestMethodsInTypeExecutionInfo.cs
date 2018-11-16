using System;

namespace MyNUnit
{
    /// <summary>
    /// Информация о результатах выполнения тестовых методов некоторого типа.
    /// </summary>
    public class TestMethodsInTypeExecutionInfo : IComparable
    {
        /// <summary>
        /// Класс, содержащий информацию о количестве тестов для каждого статуса выполнения.
        /// </summary>
        public class TestMethodsCountInfo
        {
            public int TrueTestCount { get; set; } = 0;
            public int FalseTestCount { get; set; } = 0;
            public int IgnoreTestCount { get; set; } = 0;
            public int IndefiniteTestCount { get; set; } = 0;
        }

        /// <summary>
        /// Информация о количестве тестов для каждого статуса выполнения.
        /// </summary>
        public TestMethodsCountInfo TestsCountInfo { get; set; } = new TestMethodsCountInfo();

        /// <summary>
        /// Объект типа, в котором выполнялись тесты.
        /// </summary>
        public object InstanceOfType { get; }

        /// <summary>
        /// Тип, содержаший тестовые методы.
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// Конструктор экземпляра класса <see cref="TestMethodsInTypeExecutionInfo"/>.
        /// </summary>
        /// <param name="instanceOfType">Объект типа, в котором выполнялись тесты.</param>
        /// <param name="type">Тип, содержаший тестовые методы.</param>
        public TestMethodsInTypeExecutionInfo(object instanceOfType, Type type)
        {
            InstanceOfType = instanceOfType;
            Type = type;
        }

        /// <summary>
        /// Метод, сравнивающий текущий экземпляр с другим объектом того же типа.
        /// </summary>
        /// <param name="obj">Объект для сравнения с данным экземпляром.</param>
        /// <returns>Значение, указывающее, каков относительный порядок сравниваемых объектов.</returns>
        public int CompareTo(object obj)
            => string.Compare(Type.Name, (obj as TestMethodsInTypeExecutionInfo).Type.Name);
    }
}
