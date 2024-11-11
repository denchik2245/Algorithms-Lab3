using System.Text;

namespace Lab3.Stack
{
    public class InfixToPostfixConverter
    {
        private static readonly Dictionary<char, int> OperatorPrecedence = new Dictionary<char, int>
        {
            { '+', 1 },
            { '-', 1 },
            { '*', 2 },
            { '/', 2 },
            { '^', 3 }
        };

        public string Convert(string infixExpression)
        {
            Stack<char> operatorStack = new Stack<char>();
            StringBuilder postfix = new StringBuilder();

            foreach (char token in infixExpression.Replace(" ", ""))
            {
                if (char.IsDigit(token))
                {
                    postfix.Append(token);
                }
                else if (IsOperator(token))
                {
                    while (!operatorStack.IsEmpty() && OperatorPrecedence.ContainsKey(operatorStack.Top()) &&
                           OperatorPrecedence[operatorStack.Top()] >= OperatorPrecedence[token])
                    {
                        postfix.Append(' ').Append(operatorStack.Pop());
                    }
                    postfix.Append(' ');
                    operatorStack.Push(token);
                }
                else if (token == '(')
                {
                    operatorStack.Push(token);
                }
                else if (token == ')')
                {
                    while (!operatorStack.IsEmpty() && operatorStack.Top() != '(')
                    {
                        postfix.Append(' ').Append(operatorStack.Pop());
                    }
                    if (operatorStack.IsEmpty())
                    {
                        throw new InvalidOperationException("Несбалансированные скобки в выражении.");
                    }
                    operatorStack.Pop();
                }
                else
                {
                    throw new InvalidOperationException($"Недопустимый символ: {token}");
                }
            }
            
            while (!operatorStack.IsEmpty())
            {
                char op = operatorStack.Pop();
                if (op == '(' || op == ')')
                {
                    throw new InvalidOperationException("Несбалансированные скобки в выражении.");
                }
                postfix.Append(' ').Append(op);
            }

            return postfix.ToString().Trim();
        }

        private bool IsOperator(char c)
        {
            return OperatorPrecedence.ContainsKey(c);
        }
    }
}
