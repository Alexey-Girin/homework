using System;
using System.Diagnostics;

namespace MD5
{
    class Program
    {
        static void Main(string[] args)
        {
            Checker checker = new Checker();

            Stopwatch stopwatchForChecker = new Stopwatch();

            stopwatchForChecker.Start();
            checker.Check(@"C:\");
            stopwatchForChecker.Stop();

            TimeSpan timeSpanForChecker = stopwatchForChecker.Elapsed;

            MultithreadedChecker multithreadedChecker = new MultithreadedChecker();

            Stopwatch stopwatchForMultithreadedChecker = new Stopwatch();

            stopwatchForMultithreadedChecker.Start();
            multithreadedChecker.Check(@"C:\");
            stopwatchForMultithreadedChecker.Stop();

            TimeSpan timeSpanForMultithreadedChecker = stopwatchForMultithreadedChecker.Elapsed;

            if (timeSpanForChecker < timeSpanForMultithreadedChecker)
            {
                Console.WriteLine("Checker быстрее");
            }
            else
            {
                Console.WriteLine("MultithreadedChecker быстрее");
            }
        }
    }
}
