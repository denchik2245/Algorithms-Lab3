using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    // Интерфейс, который должны реализовывать классы очереди
    public interface IQueue<T>
    {
        // Вставка элемента в очередь
        void Enqueue(T item);
        // Удаление элемента из очереди
        T Dequeue();
        // Получение первого элемента без удаления
        T Peek();
        // Проверка на пустоту
        bool IsEmpty();
        // Печать всех элементов
        void PrintQueue();
    }
}
