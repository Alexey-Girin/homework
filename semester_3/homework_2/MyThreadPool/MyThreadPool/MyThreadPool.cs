using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace MyThreadPool
{
    public class MyThreadPool
    {
        private CancellationTokenSource cts = new CancellationTokenSource();

        private Queue<Action> tasks = new Queue<Action>();

        private AutoResetEvent resetEvent = new AutoResetEvent(true);

        private Thread[] threads;

        public MyThreadPool(int numOfPools)
        {
            threads = new Thread[numOfPools];

            for (int i = 0; i < threads.Length; i++)
            {
                threads[i] = new Thread(new ThreadStart(WorkForThreads));
                threads[i].Name = $"{i}";
            }

            foreach (var thread in threads)
            {
                thread.Start();
            }
        }

        public void WorkForThreads()
        {
            while (!cts.IsCancellationRequested)
            {
                resetEvent.WaitOne();

                if (tasks.Count == 0)
                {
                    resetEvent.Set();
                    continue;
                }

                tasks.Dequeue()();

                resetEvent.Set();
            }
        }

        public MyTask<TResult> AddTask<TResult>(Func<TResult> newFunc)
        {
            resetEvent.WaitOne();

            MyTask<TResult> newTask = new MyTask<TResult>(newFunc);
            tasks.Enqueue(newTask.PerformTask);

            resetEvent.Set();

            return newTask;
        }

        public void Shutdown()
            => cts.Cancel();
    }
}
