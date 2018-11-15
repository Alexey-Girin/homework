using System;

namespace MyNUnit
{
    public class TestMethodsInTypeExecutionInfo : IComparable
    {
        public class TestMethodsCountInfo
        {
            public int TrueTestCount { get; set; } = 0;
            public int FalseTestCount { get; set; } = 0;
            public int IgnoreTestCount { get; set; } = 0;
            public int IndefiniteTestCount { get; set; } = 0;
        }

        public TestMethodsCountInfo TestsCountInfo { get; set; } = new TestMethodsCountInfo();
        public object InstanceOfType { get; }
        public Type Type { get; }

        public TestMethodsInTypeExecutionInfo(object instanceOfType, Type type)
        {
            InstanceOfType = instanceOfType;
            Type = type;
        }

        public int CompareTo(object obj)
            => string.Compare(Type.Name, (obj as TestMethodsInTypeExecutionInfo).Type.Name);
    }
}
