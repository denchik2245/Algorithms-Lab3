using System.IO;

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

        // Добавление элемента в конец списка
        public void AddLast(T data)
        {
            Node<T> newNode = new Node<T>(data);
            if (head == null)
            {
                head = newNode;
            }
            else
            {
                Node<T> current = head;
                while (current.Next != null)
                {
                    current = current.Next;
                }
                current.Next = newNode;
            }
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
        public void Display(Action<string> output)
        {
            Node<T> current = head;
            while (current != null)
            {
                output(current.Data.ToString());
                current = current.Next;
            }
        }

        // Получение всех элементов списка в виде строки
        public string GetAllElementsAsString()
        {
            Node<T> current = head;
            StringWriter writer = new StringWriter();
            while (current != null)
            {
                writer.Write(current.Data + " -> ");
                current = current.Next;
            }
            writer.Write("null");
            return writer.ToString();
        }
    }
}

