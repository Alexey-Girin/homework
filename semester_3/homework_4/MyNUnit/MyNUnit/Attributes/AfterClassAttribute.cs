using System;

namespace MyNUnit
{
    /// <summary>
    /// Атрибут, которым помечается вспомогательный метод, запускаемый после выполнения всех тестовых методов.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class AfterClassAttribute : Attribute
    {
    }
}
