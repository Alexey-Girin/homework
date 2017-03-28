namespace ParseTreeNamespace
{
    using System;

    class Operator : NodeValue
    {
        public char value;

        public Operator(char addedValue) => this.value = addedValue;

        public void PrintValue() => Console.Write(value);

        public int GetValue() => value;
    }
}
