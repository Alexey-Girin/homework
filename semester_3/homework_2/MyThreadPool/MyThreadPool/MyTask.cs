using System;
using System.Threading;

namespace MyThreadPool
{
    public class MyTask<TResult> : IMyTask<TResult>
    {
        public bool IsCompleted { get; private set; } = false;

        public string ThreadName { get; set; }

        public TResult Result
        {
            get
            {
                if (!IsCompleted)
                {
                    Thread.CurrentThread.Join();
                }

                while (!IsCompleted)
                {
                    Thread.Sleep(10);
                }

                if (outerException != null)
                {
                    throw new AggregateException("Outer exception", outerException);
                }

                return intermediateResult;
            }
        }

        private Exception outerException;

        private TResult intermediateResult;

        private Func<TResult> func;

        private MyThreadPool threadPool;

        public MyTask(Func<TResult> _Func, MyThreadPool _ThreadPool)
        {
            func = _Func;
            threadPool = _ThreadPool;
        }

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

            ThreadName = Thread.CurrentThread.Name;
            IsCompleted = true;
        }
    }
}
