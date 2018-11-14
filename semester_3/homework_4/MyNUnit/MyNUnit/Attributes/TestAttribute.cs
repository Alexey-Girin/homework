﻿using System;

namespace MyNUnit
{
    /// <summary>
    /// Атрибут, которым помечается тестовый метод.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class TestAttribute : Attribute
    {
        public Type Excepted { get; set; }
        public string Ignore { get; set; }
    }
}
