using System;
using System.Threading;

namespace MyThreadPool
{
    public class MyTask<TResult> : IMyTask<TResult>
    {
        public bool IsCompleted { get; private set; } = false;

        public string threadName { get; set; }

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

                return result;
            }
        }

        private Exception outerException;

        private TResult result;

        private Func<TResult> func;

        public MyTask(Func<TResult> newFunc)
            => func = newFunc;

        public void PerformTask()
        {
            try
            {
                result = func();
                threadName = Thread.CurrentThread.Name;
            }
            catch (Exception exception)
            {
                outerException = exception;
            }

            IsCompleted = true;
        }
    }
}
