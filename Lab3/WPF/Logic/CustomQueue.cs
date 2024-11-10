using Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class CustomQueue<T> : IQueue<T>
    {
        private CustomLinkedList<T> list = new CustomLinkedList<T>();

        // Вставка элемента в очередь
        public void Enqueue(T data) => list.AddLast(data);
        // Удаление элемента из очереди
        public T Dequeue() => list.RemoveFirst();
        // Получение первого элемента без удаления
        public T Peek() => list.GetFirst();
        // Проверка на пустоту
        public bool IsEmpty() => list.IsEmpty();
        // Печать всех элементов
        public void PrintQueue(Action<string> output)
        {
            list.Display(output);
        }
    }
}
