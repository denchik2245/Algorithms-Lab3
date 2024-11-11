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

    public class CustomLinkedList<T> where T : IComparable<T>
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

        // 1. Переворот списка
        public void Reverse()
        {
            Node<T> prev = null;
            Node<T> current = head;
            Node<T> next = null;

            while (current != null)
            {
                next = current.Next; // Сохраняем ссылку на следующий элемент
                current.Next = prev; // Меняем ссылку текущего элемента на предыдущий
                prev = current; // Продвигаем указатель prev вперед
                current = next; // Переходим к следующему элементу
            }

            head = prev; // Переназначаем head на последний элемент
        }

        // 2. Перенос последнего элемента в начало списка
        public void MoveLastToFirst()
        {
            if (head == null || head.Next == null) return; // Пустой список или список с одним элементом

            Node<T> current = head;
            Node<T> prev = null;

            while (current.Next != null)
            {
                prev = current;
                current = current.Next;
            }

            // Теперь current указывает на последний элемент, prev – на предпоследний
            prev.Next = null; // Отсоединяем последний элемент от предпоследнего
            current.Next = head; // Перемещаем последний элемент к началу списка
            head = current; // Обновляем head
        }

        // 2. Перенос первого элемента в конец списка
        public void MoveFirstToLast()
        {
            if (head == null || head.Next == null) return; // Пустой список или список с одним элементом

            Node<T> first = head;
            head = head.Next; // Обновляем head на второй элемент
            first.Next = null; // Отсоединяем первый элемент

            Node<T> current = head;
            while (current.Next != null)
            {
                current = current.Next;
            }

            current.Next = first; // Добавляем первый элемент в конец списка
        }

        // 3. Определение количества различных элементов списка
        public int CountDistinct()
        {
            HashSet<T> distinctElements = new HashSet<T>();
            Node<T> current = head;
            while (current != null)
            {
                distinctElements.Add(current.Data);
                current = current.Next;
            }
            return distinctElements.Count;
        }

        // 4. Удаление неуникальных элементов из списка
        public void RemoveNonUnique()
        {
            if (head == null) return;

            Dictionary<T, int> elementCount = new Dictionary<T, int>();
            Node<T> current = head;

            // Подсчет количества вхождений каждого элемента
            while (current != null)
            {
                if (elementCount.ContainsKey(current.Data))
                {
                    elementCount[current.Data]++;
                }
                else
                {
                    elementCount[current.Data] = 1;
                }
                current = current.Next;
            }

            // Удаление неуникальных элементов
            current = head;
            Node<T> previous = null;

            while (current != null)
            {
                if (elementCount[current.Data] > 1)
                {
                    // Удаляем текущий элемент
                    if (previous == null)
                    {
                        head = current.Next; // Удаление головы списка
                    }
                    else
                    {
                        previous.Next = current.Next;
                    }
                }
                else
                {
                    previous = current;
                }

                current = current.Next;
            }
        }

        // 5. Вставка списка самого в себя после первого вхождения числа x
        public void InsertSelfAfterFirstOccurrence(T x)
        {
            Node<T> current = head;
            while (current != null && !current.Data.Equals(x))
            {
                current = current.Next;
            }

            if (current != null)
            {
                Node<T> copyHead = CopyList(head);
                Node<T> next = current.Next;
                current.Next = copyHead;

                // Найти конец скопированного списка
                Node<T> tail = copyHead;
                while (tail.Next != null)
                {
                    tail = tail.Next;
                }
                tail.Next = next;
            }
        }

        // Вспомогательная функция для создания копии списка
        private Node<T> CopyList(Node<T> node)
        {
            if (node == null) return null;

            Node<T> copyHead = new Node<T>(node.Data);
            Node<T> copyCurrent = copyHead;
            Node<T> originalCurrent = node.Next;

            while (originalCurrent != null)
            {
                copyCurrent.Next = new Node<T>(originalCurrent.Data);
                copyCurrent = copyCurrent.Next;
                originalCurrent = originalCurrent.Next;
            }

            return copyHead;
        }

        // 6. Вставка элемента E в упорядоченный по неубыванию список
        public void InsertInOrder(T e)
        {
            Node<T> newNode = new Node<T>(e);

            if (head == null || head.Data.CompareTo(e) > 0)
            {
                // Вставка в начало списка
                newNode.Next = head;
                head = newNode;
            }
            else
            {
                Node<T> current = head;
                while (current.Next != null && current.Next.Data.CompareTo(e) <= 0)
                {
                    current = current.Next;
                }

                newNode.Next = current.Next;
                current.Next = newNode;
            }
        }

        // 7. Удаление всех элементов E
        public void RemoveAllOccurrences(T e)
        {
            while (head != null && head.Data.Equals(e))
            {
                head = head.Next;
            }

            Node<T> current = head;
            Node<T> previous = null;

            while (current != null)
            {
                if (current.Data.Equals(e))
                {
                    if (previous != null)
                    {
                        previous.Next = current.Next;
                    }
                }
                else
                {
                    previous = current;
                }
                current = current.Next;
            }
        }

        // 8. Вставка элемента F перед первым вхождением элемента E
        public void InsertBeforeFirstOccurrence(T f, T e)
        {
            Node<T> newNode = new Node<T>(f);

            if (head == null)
            {
                return; // Список пуст, ничего не делаем
            }

            if (head.Data.Equals(e))
            {
                // Вставка перед первым элементом списка
                newNode.Next = head;
                head = newNode;
                return;
            }

            Node<T> current = head;

            while (current.Next != null && !current.Next.Data.Equals(e))
            {
                current = current.Next;
            }

            if (current.Next != null)
            {
                newNode.Next = current.Next;
                current.Next = newNode;
            }
        }
    }
}

