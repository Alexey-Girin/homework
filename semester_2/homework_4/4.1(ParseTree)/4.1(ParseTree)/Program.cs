namespace _4thHomework.Task1
{
    using ParseTreeNamespace;
    using System;

    class Program
    {
        static void Main(string[] args)
        {
            ParseTree tree = new ParseTree();
            Console.Write(tree.Calculate("( + 2 2 )"));
        }
    }
}
