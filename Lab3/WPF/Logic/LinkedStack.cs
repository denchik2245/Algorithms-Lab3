using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        //Очистка стека
        public void Clear()
        {
            while (!IsEmpty())
            {
                Pop();
            }
        }

        // Метод для выполнения команд из файла
        public void ExecuteCommandsFromFile(string filePath)
        {
            string[] commands = File.ReadAllText(filePath).Split(' ');

            foreach (string command in commands)
            {
                if (command.StartsWith("1,")) // Push
                {
                    string element = command.Substring(2);
                    Push((T)Convert.ChangeType(element, typeof(T)));
                    Console.WriteLine($"Push({element}) выполнено.");
                }
                else if (command == "2") // Pop
                {
                    try
                    {
                        T popped = Pop();
                        Console.WriteLine($"Pop() -> {popped}");
                    }
                    catch (InvalidOperationException)
                    {
                        Console.WriteLine("Pop() -> Ошибка: стек пуст.");
                    }
                }
                else if (command == "3") // Top
                {
                    try
                    {
                        T top = Top();
                        Console.WriteLine($"Top() -> {top}");
                    }
                    catch (InvalidOperationException)
                    {
                        Console.WriteLine("Top() -> Ошибка: стек пуст.");
                    }
                }
                else if (command == "4") // isEmpty
                {
                    bool isEmpty = IsEmpty();
                    Console.WriteLine($"isEmpty() -> {isEmpty}");
                }
                else if (command == "5") // Print
                {
                    Console.WriteLine("Print() -> Содержимое стека:");
                    Print();
                }
                else
                {
                    Console.WriteLine($"Неизвестная команда: {command}");
                }
            }
        }

        // Метод для вычисления постфиксного выражения из файла
        public double EvaluatePostfixExpression(string filePath)
        {
            // Очищаем стек перед началом вычислений
            Clear();

            string[] tokens = File.ReadAllText(filePath).Split(' ', StringSplitOptions.RemoveEmptyEntries);

            foreach (string token in tokens)
            {
                if (double.TryParse(token, out double number)) // Если токен - число
                {
                    Push((T)Convert.ChangeType(number, typeof(T)));
                }
                else // Если токен - операция
                {
                    switch (token)
                    {
                        case "+":
                            Push(DoubleToT(TToDouble(Pop()) + TToDouble(Pop())));
                            break;
                        case "-":
                            double subtrahend = TToDouble(Pop());
                            Push(DoubleToT(TToDouble(Pop()) - subtrahend));
                            break;
                        case "*":
                            Push(DoubleToT(TToDouble(Pop()) * TToDouble(Pop())));
                            break;
                        case "/":
                        case ":":
                            double divisor = TToDouble(Pop());
                            Push(DoubleToT(TToDouble(Pop()) / divisor));
                            break;
                        case "^":
                            double exponent = TToDouble(Pop());
                            Push(DoubleToT(Math.Pow(TToDouble(Pop()), exponent)));
                            break;
                        case "ln":
                            Push(DoubleToT(Math.Log(TToDouble(Pop()))));
                            break;
                        case "cos":
                            Push(DoubleToT(Math.Cos(TToDouble(Pop()))));
                            break;
                        case "sin":
                            Push(DoubleToT(Math.Sin(TToDouble(Pop()))));
                            break;
                        case "sqrt":
                            Push(DoubleToT(Math.Sqrt(TToDouble(Pop()))));
                            break;
                        default:
                            throw new InvalidOperationException($"Неизвестная операция: {token}");
                    }
                }
            }

            if (IsEmpty() || !typeof(T).IsAssignableFrom(typeof(double)))
            {
                throw new InvalidOperationException("Ошибка: неверное выражение.");
            }

            // Результат — единственный оставшийся элемент в стеке
            return Convert.ToDouble(Pop());
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
