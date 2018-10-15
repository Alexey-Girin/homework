namespace MyThreadPool
{
    public interface IMyTask<TResult>
    {
        bool IsCompleted { get; }

        TResult Result { get; }
    }
}
