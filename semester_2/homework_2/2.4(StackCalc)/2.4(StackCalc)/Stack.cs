namespace StackNamespace
{
    interface Stack
    {
        void Push(int value);
        int Pop();
        int Peek();
        bool IsEmpty();
    }
}
