using System.IO;

namespace Logic
{
    public class PostfixEvaluator<T>
    {
        private readonly Stack<T> stack;

        public PostfixEvaluator()
        {
            stack = new Stack<T>();
        }
        
        public double EvaluateExpressionFromFile(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"Файл {filePath} не найден.");

            string expression = File.ReadAllText(filePath);
            return EvaluateExpression(expression);
        }
        
        public double EvaluateExpression(string expression)
        {
            stack.Clear();

            string[] tokens = expression.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            foreach (string token in tokens)
            {
                if (double.TryParse(token, out double number))
                {
                    stack.Push((T)Convert.ChangeType(number, typeof(T)));
                }
                else
                {
                    PerformOperation(token);
                }
            }

            if (stack.IsEmpty() || !typeof(T).IsAssignableFrom(typeof(double)))
            {
                throw new InvalidOperationException("Ошибка: неверное выражение.");
            }
            
            return Convert.ToDouble(stack.Pop());
        }
        
        private void PerformOperation(string token)
        {
            switch (token)
            {
                case "+":
                    stack.Push(DoubleToT(TToDouble(stack.Pop()) + TToDouble(stack.Pop())));
                    break;
                case "-":
                    double subtrahend = TToDouble(stack.Pop());
                    stack.Push(DoubleToT(TToDouble(stack.Pop()) - subtrahend));
                    break;
                case "*":
                    stack.Push(DoubleToT(TToDouble(stack.Pop()) * TToDouble(stack.Pop())));
                    break;
                case "/":
                case ":":
                    double divisor = TToDouble(stack.Pop());
                    stack.Push(DoubleToT(TToDouble(stack.Pop()) / divisor));
                    break;
                case "^":
                    double exponent = TToDouble(stack.Pop());
                    stack.Push(DoubleToT(Math.Pow(TToDouble(stack.Pop()), exponent)));
                    break;
                case "ln":
                    stack.Push(DoubleToT(Math.Log(TToDouble(stack.Pop()))));
                    break;
                case "cos":
                    stack.Push(DoubleToT(Math.Cos(TToDouble(stack.Pop()))));
                    break;
                case "sin":
                    stack.Push(DoubleToT(Math.Sin(TToDouble(stack.Pop()))));
                    break;
                case "sqrt":
                    stack.Push(DoubleToT(Math.Sqrt(TToDouble(stack.Pop()))));
                    break;
                default:
                    throw new InvalidOperationException($"Неизвестная операция: {token}");
            }
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
