using System;
using System.Collections.Concurrent;
using System.Threading;

namespace MyThreadPool
{
    public class MyThreadPool
    {
        private CancellationTokenSource cts = new CancellationTokenSource();

        private ConcurrentQueue<Action> tasks = new ConcurrentQueue<Action>();
        
        private AutoResetEvent resetEvent = new AutoResetEvent(true);

        private Thread[] threads;

        public MyThreadPool(int numOfPools)
        {
            threads = new Thread[numOfPools];

            for (int i = 0; i < threads.Length; i++)
            {
                threads[i] = new Thread(new ThreadStart(ThreadMethod))
                {
                    Name = $"{i}"
                };
            }

            foreach (var thread in threads)
            {
                thread.Start();
            }
        }

        public void ThreadMethod()
        {
            while (!cts.IsCancellationRequested)
            {
                resetEvent.WaitOne();

                if (tasks.Count == 0)
                {
                    resetEvent.Set();
                    continue;
                }

                tasks.TryDequeue(out Action task);

                resetEvent.Set();

                task();
            }
        }

        public MyTask<TResult> AddTask<TResult>(Func<TResult> newFunc)
        {
            MyTask<TResult> newTask = new MyTask<TResult>(newFunc, this);
            tasks.Enqueue(newTask.PerformTask);

            return newTask;
        }

        public void Shutdown()
            => cts.Cancel();
    }
}
