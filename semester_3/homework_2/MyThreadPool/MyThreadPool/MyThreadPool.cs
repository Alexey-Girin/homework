using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MyThreadPool
{
    public class MyThreadPool
    {
        private Queue<Action> tasks = new Queue<Action>();

        private Thread[] threads;

        public MyThreadPool(int numOfPools)
        {
            threads = new Thread[numOfPools];

            for (int i = 0; i < threads.Length; i++)
            {
                threads[i] = new Thread(new ThreadStart(WorkForThreads));
            }

            foreach (var thread in threads)
            {
                thread.Start();
            }
        }

        public void WorkForThreads()
        {
            while (tasks.Count == 0)
            {
                Thread.Sleep(100);
            }

            var task = tasks.Dequeue();
            task();
        }

        public MyTask<TResult> AddTask<TResult>(Func<TResult> newFunc)
        {
            MyTask<TResult> newTask = new MyTask<TResult>(newFunc);

            tasks.Enqueue(newTask.PerformTask);

            return newTask;
        }
    }
}
