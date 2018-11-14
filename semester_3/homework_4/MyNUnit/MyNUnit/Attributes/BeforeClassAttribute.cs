using System;

namespace MyNUnit
{
    /// <summary>
    /// Атрибут, которым помечается вспомогательный метод, запускаемый перед выполнением всех тестовых методов.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class BeforeClassAttribute : Attribute
    {
    }
}
