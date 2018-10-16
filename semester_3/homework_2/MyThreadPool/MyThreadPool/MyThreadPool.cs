using System;
using System.Collections.Concurrent;
using System.Threading;

namespace MyThreadPool
{
    /// <summary>
    /// Пул задач с фиксированным числом потоков.
    /// </summary>
    public class MyThreadPool
    {
        /// <summary>
        /// Объект, необходимый для завершения работы потоков.
        /// </summary>
        private CancellationTokenSource cts = new CancellationTokenSource();

        /// <summary>
        /// Очередь задач, принятых к исполнению.
        /// </summary>
        private ConcurrentQueue<Action> tasks = new ConcurrentQueue<Action>();

        /// <summary>
        /// Объект, необходимый для синхронизации потоков.
        /// </summary>
        private AutoResetEvent resetEvent = new AutoResetEvent(true);

        /// <summary>
        /// Массив потоков.
        /// </summary>
        private Thread[] threads;

        /// <summary>
        /// Число работающих потоков.
        /// </summary>
        private volatile int countOfActiveThreads;

        /// <summary>
        /// Конструктор экземпляра класса <see cref="MyThreadPool"/>.
        /// </summary>
        /// <param name="numOfThreads">Число потоков.</param>
        public MyThreadPool(int numOfThreads)
        {
            threads = new Thread[numOfThreads];

            for (int i = 0; i < threads.Length; i++)
            {
                threads[i] = new Thread(new ThreadStart(ThreadMethod));
            }

            foreach (var thread in threads)
            {
                thread.Start();
            }
        }

        /// <summary>
        /// Метод, который выполняется потоком.
        /// </summary>
        public void ThreadMethod()
        {
            countOfActiveThreads++;

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

            countOfActiveThreads--;
        }

        /// <summary>
        /// Добавление задачи (представляющей вычисление) в очередь.
        /// </summary>
        /// <typeparam name="TResult">Тип результата исполнения задачи.</typeparam>
        /// <param name="newFunc">Вычисление, которое представляет задача.</param>
        /// <returns>Задача (как объект <see cref="IMyTask{TResult}"/>), 
        /// принятая к исполнению.</returns>
        public IMyTask<TResult> AddTask<TResult>(Func<TResult> newFunc)
        {
            MyTask<TResult> newTask = new MyTask<TResult>(newFunc, this);
            tasks.Enqueue(newTask.PerformTask);

            return newTask;
        }

        /// <summary>
        /// Метод, завершающий работу потоков.
        /// </summary>
        public void Shutdown()
            => cts.Cancel();

        /// <summary>
        /// Метод, возвращающий число работающих потоков.
        /// </summary>
        /// <returns>Число работающих потоков.</returns>
        public int GetCountOfActiveThreads()
            => countOfActiveThreads;
    }
}
