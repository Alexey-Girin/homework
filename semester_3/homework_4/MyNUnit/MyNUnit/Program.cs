namespace MyNUnit
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = @"C:\Users\Алексей\Desktop\homework\semester_3\" +
                @"homework_4\MyNUnit\TestingProject\bin\Debug";

            TestingSystem.RunTests(path);
        }
    }
}
