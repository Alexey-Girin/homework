using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
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
            /// Свойство, которое возвращает результат выполнения задачи.
            /// </summary>
            public TResult Result
            {
                get
                {
                    taskСompleted.WaitOne();

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
            private ManualResetEvent taskСompleted = new ManualResetEvent(false);

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
            /// Очередь делегатов на добавление в <see cref="threadPool"/>,
            /// каждый из которых указывает на выполнение задачи, представляющей вычисление,
            /// примененное к результату текущей задачи.
            /// </summary>
            private Queue<Action> actions = new Queue<Action>();

            /// <summary>
            /// Объект, необходимый для синхронизации потоков. 
            /// </summary>
            object locker = new object();

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
                lock (locker)
                {
                    if (IsCompleted)
                    {
                        return threadPool.AddTask(() => { return newFunc(Result); });
                    }

                    var newTask = new MyTask<TNewResult>(() => { return newFunc(Result); }, threadPool);
                    actions.Enqueue(newTask.PerformTask);

                    return newTask;
                }
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
                taskСompleted.Set();

                AddActionsToThreadPool();
                func = null;
            }

            /// <summary>
            /// Добавление элементов <see cref="actions"/> в <see cref="threadPool"/>.
            /// </summary>
            private void AddActionsToThreadPool()
            {
                lock (locker)
                {
                    foreach (var action in actions)
                    {
                        threadPool.AddAction(action);
                    }
                }
            }
        }

        /// <summary>
        /// Объект, необходимый для завершения работы потоков.
        /// </summary>
        private CancellationTokenSource cts = new CancellationTokenSource();

        /// <summary>
        /// Очередь задач, принятых к исполнению.
        /// </summary>
        private Queue<Action> tasks = new Queue<Action>();

        /// <summary>
        /// Объект, необходимый для синхронизации потоков.
        /// </summary>
        private object locker = new object();

        /// <summary>
        /// Объект, принимающий сигнальное состояние, если <see cref="tasks"/> не пуст.
        /// </summary>
        private ManualResetEvent tasksNotEmpty = new ManualResetEvent(false);

        /// <summary>
        /// Объект, необходимый для избежания гонки при инкременте и декременте
        /// <see cref="countOfActiveThreads"/>.
        /// </summary>
        private object lockerCountOfActiveThreads = new object();

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
            lock (lockerCountOfActiveThreads)
            {
                countOfActiveThreads++;
            }

            while (true)
            {
                tasksNotEmpty.WaitOne();

                if (cts.IsCancellationRequested)
                {
                    break;
                }

                Action task = null;

                lock (locker)
                {
                    if (tasks.Count == 0 || cts.IsCancellationRequested)
                    {
                        continue;
                    }

                    task = tasks.Dequeue();

                    if (tasks.Count == 0)
                    {
                        tasksNotEmpty.Reset();
                    }
                }

                task();
            }

            lock (lockerCountOfActiveThreads)
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
            tasksNotEmpty.Set();

            return newTask;
        }

        /// <summary>
        /// Добавление делегата <paramref name="actionToAdd"/> в очередь.
        /// </summary>
        /// <param name="actionToAdd">Делегат, указывающий на выполение задачи.</param>
        public void AddAction(Action actionToAdd)
        {
            tasks.Enqueue(actionToAdd);
            tasksNotEmpty.Set();
        }

        /// <summary>
        /// Метод, завершающий работу потоков.
        /// </summary>
        public void Shutdown()
        {
            cts.Cancel();
            tasksNotEmpty.Set();
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