namespace Lab3.Queue
{
    public interface IQueue<T>
    {
        void Enqueue(T item);
        T Dequeue();
        T Peek();
        bool IsEmpty();
        void PrintQueue(Action<string> output);
    }
}
