using System.IO;
namespace Logic
{
    public class Stack<T>
    {
        public CustomLinkedList<T> list = new CustomLinkedList<T>();
        
        public void Push(T data)
        {
            list.AddFirst(data);
        }
        
        public T Pop()
        {
            return list.RemoveFirst();
        }
        
        public T Top()
        {
            return list.GetFirst();
        }
        
        public bool IsEmpty()
        {
            return list.IsEmpty();
        }
        
        public void Print(Action<string> output)
        {
            list.Display(output);
        }
        
        public void Clear()
        {
            while (!IsEmpty())
            {
                Pop();
            }
        }
        
        public void ExecuteCommandsFromFile(string filePath, Action<string> output)
        {
            string[] commands = File.ReadAllText(filePath).Split(' ');

            foreach (string command in commands)
            {
                if (command.StartsWith("1,"))
                {
                    string element = command.Substring(2);
                    Push((T)Convert.ChangeType(element, typeof(T)));
                    output($"Push({element}) выполнено.");
                }
                else if (command == "2")
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
                else if (command == "3")
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
                else if (command == "4")
                {
                    bool isEmpty = IsEmpty();
                    output($"isEmpty() -> {isEmpty}");
                }
                else if (command == "5")
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
        
        public string GetAllElementsAsString()
        {
            return list.GetAllElementsAsString();
        }
        
        private double TToDouble(T value)
        {
            return Convert.ToDouble(value);
        }
        
        private T DoubleToT(double value)
        {
            return (T)Convert.ChangeType(value, typeof(T));
        }
    }
}
