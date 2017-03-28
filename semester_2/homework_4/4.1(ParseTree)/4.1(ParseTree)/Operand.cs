namespace ParseTreeNamespace
{
    using System;

    class Operand : NodeValue
    {
        public int value;
        
        public Operand(int addedValue)
        {
            this.value = addedValue; 
        }

        public void PrintValue()
        {
            Console.Write(value);
        }
    }
}
