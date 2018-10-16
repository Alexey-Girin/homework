using System;
using System.Threading;

namespace MyThreadPool
{
    /// <summary>
    /// Представляет задачи, принятые к исполнению.
    /// </summary>
    /// <typeparam name="TResult">Тип результата исполнения задачи.</typeparam>
    public class MyTask<TResult> : IMyTask<TResult>
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
        public MyTask(Func<TResult> receivedFunc, MyThreadPool receivedThreadPool)
        {
            func = receivedFunc;
            threadPool = receivedThreadPool;
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
}
