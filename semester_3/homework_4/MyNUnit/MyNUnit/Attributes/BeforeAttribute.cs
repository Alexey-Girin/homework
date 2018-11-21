using System;

namespace MyNUnit
{
    /// <summary>
    /// Атрибут, которым помечается вспомогательный метод, запускаемый перед выполнением каждого тестового метода.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class BeforeAttribute : Attribute
    {
    }
}
