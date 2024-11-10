using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class StandardQueue<T>
    {
        private Queue<T> queue;

        public StandardQueue()
        {
            queue = new Queue<T>();
        }

        // Вставка элемента в очередь
        public void Enqueue(T item) => queue.Enqueue(item);
        // Проверка на пустоту
        public bool IsEmpty() => queue.Count == 0;

        // Удаление элемента из очереди
        public T Dequeue()
        {
            if (queue.Count == 0) throw new InvalidOperationException("Очередь пуста.");
            return queue.Dequeue();
        }

        // Печать всех элементов
        public void PrintQueue(Action<string> output)
        {
            foreach (var item in queue)
            {
                output(item.ToString());
            }
        }

        // Получение первого элемента без удаления
        public T Peek()
        {
            if (queue.Count == 0) throw new InvalidOperationException("Очередь пуста.");
            return queue.Peek();
        }
    }
}
