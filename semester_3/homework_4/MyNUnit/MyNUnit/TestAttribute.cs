using System;

namespace MyNUnit
{
    public class TestAttribute : Attribute
    {
        public Type Excepted { get; set; }
        public string Ignore { get; set; }
    }
}
