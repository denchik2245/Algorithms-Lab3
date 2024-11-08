using System.IO;
namespace Logic
{
    public class Stack<T>
    {
        public CustomLinkedList<T> list = new CustomLinkedList<T>();

        // Операция Push
        public void Push(T data)
        {
            list.AddFirst(data);
        }

        // Операция Pop
        public T Pop()
        {
            return list.RemoveFirst();
        }

        // Операция Top
        public T Top()
        {
            return list.GetFirst();
        }

        // Проверка, пуст ли стек
        public bool IsEmpty()
        {
            return list.IsEmpty();
        }

        // Вывод всех элементов стека
        public void Print()
        {
            list.Display();
        }

        // Очистка стека
        public void Clear()
        {
            while (!IsEmpty())
            {
                Pop();
            }
        }

        // Метод для выполнения команд из файла
        public void ExecuteCommandsFromFile(string filePath, Action<string> output)
        {
            string[] commands = File.ReadAllText(filePath).Split(' ');

            foreach (string command in commands)
            {
                if (command.StartsWith("1,")) // Push
                {
                    string element = command.Substring(2);
                    Push((T)Convert.ChangeType(element, typeof(T)));
                    output($"Push({element}) выполнено.");
                }
                else if (command == "2") // Pop
                {
                    try
                    {
                        T popped = Pop();
                        output($"Pop() -> {popped}");
                    }
                    catch (InvalidOperationException)
                    {
                        output("Pop() -> Ошибка: стек пуст.");
                    }
                }
                else if (command == "3") // Top
                {
                    try
                    {
                        T top = Top();
                        output($"Top() -> {top}");
                    }
                    catch (InvalidOperationException)
                    {
                        output("Top() -> Ошибка: стек пуст.");
                    }
                }
                else if (command == "4") // isEmpty
                {
                    bool isEmpty = IsEmpty();
                    output($"isEmpty() -> {isEmpty}");
                }
                else if (command == "5") // Print
                {
                    output("Print() -> Содержимое стека:");
                    output(GetAllElementsAsString());
                }
                else
                {
                    output($"Неизвестная команда: {command}");
                }
            }
        }


        // Получение всех элементов стека в виде строки
        public string GetAllElementsAsString()
        {
            return list.GetAllElementsAsString();
        }

        // Метод для преобразования значения типа T в double
        private double TToDouble(T value)
        {
            return Convert.ToDouble(value);
        }

        // Метод для преобразования значения double в тип T
        private T DoubleToT(double value)
        {
            return (T)Convert.ChangeType(value, typeof(T));
        }
    }
}
