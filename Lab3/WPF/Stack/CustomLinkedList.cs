using System.IO;

namespace Lab3.Stack
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
        private int count;

        public CustomLinkedList()
        {
            head = null;
            count = 0;
        }
        
        public int Count
        {
            get { return count; }
        }
        
        public void AddFirst(T data)
        {
            Node<T> newNode = new Node<T>(data);
            newNode.Next = head;
            head = newNode;
            count++;
        }

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
            count++;
        }

        public T RemoveFirst()
        {
            if (head == null) throw new InvalidOperationException("Список пуст.");

            T data = head.Data;
            head = head.Next;
            count--;
            return data;
        }
        
        public T GetFirst()
        {
            if (head == null) throw new InvalidOperationException("Список пуст.");
            return head.Data;
        }
        
        public bool IsEmpty()
        {
            return head == null;
        }
        
        public void Display(Action<string> output)
        {
            Node<T> current = head;
            while (current != null)
            {
                output(current.Data.ToString());
                current = current.Next;
            }
        }
        
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
                next = current.Next;
                current.Next = prev;
                prev = current;
                current = next;
            }

            head = prev;
        }

        // 2. Перенос последнего элемента в начало списка
        public void MoveLastToFirst()
        {
            if (head == null || head.Next == null) return;

            Node<T> current = head;
            Node<T> prev = null;

            while (current.Next != null)
            {
                prev = current;
                current = current.Next;
            }
            
            prev.Next = null;
            current.Next = head;
            head = current;
        }

        // 2. Перенос первого элемента в конец списка
        public void MoveFirstToLast()
        {
            if (head == null || head.Next == null) return;

            Node<T> first = head;
            head = head.Next;
            first.Next = null;

            Node<T> current = head;
            while (current.Next != null)
            {
                current = current.Next;
            }

            current.Next = first;
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
        
        // 9. Дописывает к списку L список E
        public void AppendList(CustomLinkedList<T> otherList)
        {
            if (head == null)
            {
                head = otherList.head;
                return;
            }

            Node<T> current = head;
            while (current.Next != null)
            {
                current = current.Next;
            }
            current.Next = otherList.head;
        }

        // 10. Разбивает список по первому вхождению заданного числа
        public (CustomLinkedList<T>, CustomLinkedList<T>) SplitAtFirstOccurrence(T e)
        {
            var firstList = new CustomLinkedList<T>();
            var secondList = new CustomLinkedList<T>();

            Node<T> current = head;
            bool found = false;

            while (current != null)
            {
                if (!found && current.Data.Equals(e))
                {
                    found = true;
                    secondList.head = current.Next;
                    break;
                }

                firstList.AddLast(current.Data);
                current = current.Next;
            }

            if (!found)
            {
                return (this, secondList);
            }

            return (firstList, secondList);
        }

        // 11. Удваивает список, добавляя его копию в конец
        public void Duplicate()
        {
            if (head == null) return;

            var copyList = new CustomLinkedList<T>();
            Node<T> current = head;

            while (current != null)
            {
                copyList.AddLast(current.Data);
                current = current.Next;
            }

            AppendList(copyList);
        }

        // 12. Меняет местами два элемента списка
        public void SwapElements(T firstElement, T secondElement)
        {
            if (head == null) return;

            Node<T> firstNode = null, secondNode = null, current = head;

            while (current != null)
            {
                if (current.Data.Equals(firstElement)) firstNode = current;
                if (current.Data.Equals(secondElement)) secondNode = current;

                if (firstNode != null && secondNode != null) break;

                current = current.Next;
            }

            if (firstNode != null && secondNode != null)
            {
                T temp = firstNode.Data;
                firstNode.Data = secondNode.Data;
                secondNode.Data = temp;
            }
        }
    }
}

