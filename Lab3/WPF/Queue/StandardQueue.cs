namespace Lab3.Queue
{
    public class StandardQueue<T>
    {
        private Queue<T> queue;

        public StandardQueue()
        {
            queue = new Queue<T>();
        }
        
        public void Enqueue(T item) => queue.Enqueue(item);
        public bool IsEmpty() => queue.Count == 0;
        public T Dequeue()
        {
            if (queue.Count == 0) throw new InvalidOperationException("Очередь пуста.");
            return queue.Dequeue();
        }
        
        public void PrintQueue(Action<string> output)
        {
            foreach (var item in queue)
            {
                output(item.ToString());
            }
        }
        
        public T Peek()
        {
            if (queue.Count == 0) throw new InvalidOperationException("Очередь пуста.");
            return queue.Peek();
        }
    }
}
