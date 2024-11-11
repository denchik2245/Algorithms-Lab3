using Lab3.Stack;

namespace Lab3.Queue
{
    public class CustomQueue<T> : IQueue<T> where T : IComparable<T>
    {
        private CustomLinkedList<T> list = new CustomLinkedList<T>();

        public void Enqueue(T data) => list.AddLast(data);
        public T Dequeue() => list.RemoveFirst();
        public T Peek() => list.GetFirst();
        public bool IsEmpty() => list.IsEmpty();
        public void PrintQueue(Action<string> output)
        {
            list.Display(output);
        }
    }
}
