namespace _4thHomework.Task1
{
    using ParseTreeNamespace;

    class Program
    {
        static void Main(string[] args)
        {
            ParseTree tree = new ParseTree();
            tree.Calculate("( + 2 2 )");
        }
    }
}
