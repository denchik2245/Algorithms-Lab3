using System.Globalization;
using System.IO;

namespace Lab3.Stack
{
    public class PostfixEvaluator<T> where T : IComparable<T>
    {
        private readonly Lab3.Stack.Stack<T> stack;

        public PostfixEvaluator()
        {
            stack = new Lab3.Stack.Stack<T>();
        }
        
        public double EvaluateExpressionFromFile(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"Файл {filePath} не найден.");

            string expression = File.ReadAllText(filePath);
            return EvaluateExpression(expression);
        }
        
        private string NormalizeToken(string token)
        {
            if (token == null)
                return null;
            
            var replacements = new Dictionary<string, string>
            {
                { "−", "-" },
                { "–", "-" },
                { "—", "-" },
                { "×", "*" },
                { "÷", "/" },
                { ":", "/" },
            };

            foreach (var kvp in replacements)
            {
                token = token.Replace(kvp.Key, kvp.Value);
            }
            
            token = token.Replace(',', '.');

            return token.Trim();
        }
        
        public double EvaluateExpression(string expression)
        {
            stack.Clear();

            string[] tokens = expression.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            foreach (string rawToken in tokens)
            {
                string token = NormalizeToken(rawToken);

                if (double.TryParse(token, NumberStyles.Any, CultureInfo.InvariantCulture, out double number))
                {
                    stack.Push((T)Convert.ChangeType(number, typeof(T)));
                }

                else if (IsFunctionWithParentheses(token))
                {
                    double result = PerformFunctionWithParentheses(token);
                    stack.Push((T)Convert.ChangeType(result, typeof(T)));
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
        
        private bool IsFunctionWithParentheses(string token)
        {
            return token.StartsWith("sin(") || token.StartsWith("cos(") || 
                   token.StartsWith("ln(") || token.StartsWith("sqrt(") || 
                   token.StartsWith("tg(") || token.StartsWith("ctg(") || 
                   token.StartsWith("log(");
        }
        
        private double PerformFunctionWithParentheses(string token)
        {
            int startIndex = token.IndexOf('(') + 1;
            int endIndex = token.IndexOf(')');
    
            if (startIndex == -1 || endIndex == -1 || endIndex <= startIndex)
            {
                throw new InvalidOperationException($"Неверный формат функции: {token}");
            }

            string numberPart = token.Substring(startIndex, endIndex - startIndex);
            string[] args = numberPart.Split(',');

            double[] parsedArgs = new double[args.Length];
            for (int i = 0; i < args.Length; i++)
            {
                if (!double.TryParse(args[i], NumberStyles.Any, CultureInfo.InvariantCulture, out double number))
                {
                    throw new InvalidOperationException($"Невозможно преобразовать '{args[i]}' в число.");
                }
                parsedArgs[i] = number;
            }

            return token.StartsWith("sin(") ? Math.Sin(parsedArgs[0]) :
                   token.StartsWith("cos(") ? Math.Cos(parsedArgs[0]) :
                   token.StartsWith("ln(") ? Math.Log(parsedArgs[0]) :
                   token.StartsWith("sqrt(") ? Math.Sqrt(parsedArgs[0]) :
                   token.StartsWith("tg(") ? Math.Tan(parsedArgs[0] * (Math.PI / 180)) :
                   token.StartsWith("ctg(") ? 1 / Math.Tan(parsedArgs[0] * (Math.PI / 180)) :
                   token.StartsWith("log(") && parsedArgs.Length == 2 ? Math.Log(parsedArgs[1], parsedArgs[0]) :
                   throw new InvalidOperationException($"Неизвестная или неподдерживаемая функция: {token}");
        }
        
        private void PerformOperation(string token)
        {
            if (stack.IsEmpty() || (stack.list.Count < 2 && 
                                    (token == "+" || token == "-" || token == "*" || token == "/" || token == ":" || token == "^")))
            {
                throw new InvalidOperationException($"Недостаточно операндов в стеке для операции {token}");
            }

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
                    double divisor = TToDouble(stack.Pop());
                    if (divisor == 0)
                        throw new DivideByZeroException("Деление на ноль.");
                    stack.Push(DoubleToT(TToDouble(stack.Pop()) / divisor));
                    break;
                case "^":
                    double exponent = TToDouble(stack.Pop());
                    stack.Push(DoubleToT(Math.Pow(TToDouble(stack.Pop()), exponent)));
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
