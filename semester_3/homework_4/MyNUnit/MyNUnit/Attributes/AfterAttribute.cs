using System;

namespace MyNUnit
{
    /// <summary>
    /// Атрибут, которым помечается вспомогательный метод, запускаемый после выполнения каждого тестового метода.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class AfterAttribute : Attribute
    {
    }
}
