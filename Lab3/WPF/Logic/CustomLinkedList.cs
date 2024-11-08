using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class Node<T>
    {
        public T Data { get; set; }
        public Node<T> Next { get; set; }

        public Node(T data)
        {
            Data = data;
            Next = null;
        }
    }

    public class CustomLinkedList<T>
    {
        private Node<T> head;

        public CustomLinkedList()
        {
            head = null;
        }

        // Добавление в начало списка
        public void AddFirst(T data)
        {
            Node<T> newNode = new Node<T>(data);
            newNode.Next = head;
            head = newNode;
        }

        // Удаление и возврат первого элемента
        public T RemoveFirst()
        {
            if (head == null) throw new InvalidOperationException("Список пуст.");

            T data = head.Data;
            head = head.Next;
            return data;
        }

        // Получение первого элемента без удаления
        public T GetFirst()
        {
            if (head == null) throw new InvalidOperationException("Список пуст.");
            return head.Data;
        }

        // Проверка, пуст ли список
        public bool IsEmpty()
        {
            return head == null;
        }

        // Вывод всех элементов списка
        public void Display()
        {
            Node<T> current = head;
            while (current != null)
            {
                Console.Write(current.Data + " -> ");
                current = current.Next;
            }
            Console.WriteLine("null");
        }
    }
}

