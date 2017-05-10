namespace _5thHomework.Task2
{
    using EventLoopNamespace;
    using System;

    class Program
    {
        static void Main(string[] args)
        {
            for (int i = 0; i < 10000; i++)
            {
                Console.Write(1);
            }
            var eventLoop = new EventLoop();
            var game = new Game();

            eventLoop.LeftHandler += game.OnLeft;
            eventLoop.RightHandler += game.OnRight;
            eventLoop.UpHandler += game.OnUp;
            eventLoop.DownHandler += game.OnDown;

            eventLoop.Run();
        }
    }
}
