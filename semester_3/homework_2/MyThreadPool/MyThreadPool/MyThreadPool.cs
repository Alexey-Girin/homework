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
        /// Представляет задачи, принятые к исполнению.
        /// </summary>
        /// <typeparam name="TResult">Тип результата исполнения задачи.</typeparam>
        private class MyTask<TResult> : IMyTask<TResult>
        {
            /// <summary>
            /// Свойство, которое возвращает true, если задача выполнена.
            /// </summary>
            public bool IsCompleted { get; private set; } = false;

            /// <summary>
            /// Возвращает результат выполнения задачи.
            /// </summary>
            public TResult Result
            {
                get
                {
                    resetEvent.WaitOne();

                    if (outerException != null)
                    {
                        throw new AggregateException("Outer exception", outerException);
                    }

                    return intermediateResult;
                }
            }

            /// <summary>
            /// После выплнения задачи устанавливается сигнальное состояние.
            /// </summary>
            private ManualResetEvent resetEvent = new ManualResetEvent(false);

            /// <summary>
            /// Исключение, которое могло возникнуть в процессе выполнения задачи.
            /// </summary>
            private Exception outerException;

            /// <summary>
            /// Результат выполнения задачи.
            /// </summary>
            private TResult intermediateResult;

            /// <summary>
            /// Вычисление, которое представляет задача.
            /// </summary>
            private Func<TResult> func;

            /// <summary>
            /// Объект класса <see cref="MyThreadPool"/>, в котором выполняется задача.
            /// </summary>
            private MyThreadPool threadPool;

            /// <summary>
            /// Конструктор экземпляра класса <see cref="MyTask{TResult}"/>.
            /// </summary>
            /// <param name="receivedFunc">Вычисление, которое представляет задача.</param>
            /// <param name="receivedThreadPool">Объект класса <see cref="MyThreadPool"/>,
            /// в котором выполняется задача.</param>
            public MyTask(Func<TResult> func, MyThreadPool threadPool)
            {
                this.func = func;
                this.threadPool = threadPool;
            }

            /// <summary>
            /// Возвращает новую задачу, представляющую вычисление, 
            /// примененное к результату текущей задачи.
            /// </summary>
            /// <typeparam name="TNewResult">Тип результата исполнения новой задачи.</typeparam>
            /// <param name="newFunc">Вычисление, которое представляет новая задача.</param>
            /// <returns>Новая задача, принятая к исполнению.</returns>
            public IMyTask<TNewResult> ContinueWith<TNewResult>(Func<TResult, TNewResult> newFunc)
            {
                resetEvent.WaitOne();

                return threadPool.AddTask(() => { return newFunc(Result); });
            }

            /// <summary>
            /// Выполнение задачи.
            /// </summary>
            public void PerformTask()
            {
                try
                {
                    intermediateResult = func();
                }
                catch (Exception exception)
                {
                    outerException = exception;
                }

                IsCompleted = true;
                resetEvent.Set();
            }
        }

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
        /// Объект, необходимый для избежания гонки при инкременте и декременте
        /// <see cref="countOfActiveThreads"/>.
        /// </summary>
        private object locker = new object();

        /// <summary>
        /// Объект, принимающий сигнальное состояние после завершения работы потоков.
        /// </summary>
        private ManualResetEvent resetShutdown = new ManualResetEvent(false);

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
            lock (locker)
            {
                countOfActiveThreads++;
            }

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

            lock (locker)
            {
                countOfActiveThreads--;
            }

            if (countOfActiveThreads == 0)
            {
                resetShutdown.Set();
            }
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
            var newTask = new MyTask<TResult>(newFunc, this);
            tasks.Enqueue(newTask.PerformTask);

            return newTask;
        }

        /// <summary>
        /// Метод, завершающий работу потоков.
        /// </summary>
        public void Shutdown()
        {
            cts.Cancel();
            resetShutdown.WaitOne();
        }

        /// <summary>
        /// Метод, возвращающий число работающих потоков.
        /// </summary>
        /// <returns>Число работающих потоков.</returns>
        public int GetCountOfActiveThreads()
            => countOfActiveThreads;
    }
}
