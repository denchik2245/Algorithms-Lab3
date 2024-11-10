using System.IO;

namespace Logic
{
    public class QueueCommandExecutor<T>
    {
        private readonly IQueue<T> queue;

        public QueueCommandExecutor(IQueue<T> queue)
        {
            this.queue = queue;
        }
        
        public void ExecuteQueueCommandsFromFile(string filePath, Action<string> output)
        {
            try
            {
                string[] operations = File.ReadAllText(filePath).Split(' ');

                foreach (string operation in operations)
                {
                    if (operation.StartsWith("1,"))
                    {
                        string value = operation.Substring(2);
                        queue.Enqueue((T)Convert.ChangeType(value, typeof(T)));
                        output($"Элемент '{value}' добавлен в очередь.");
                    }
                    else if (operation == "2")
                    {
                        if (!queue.IsEmpty())
                        {
                            T removed = queue.Dequeue();
                            output($"Элемент '{removed}' удален из очереди.");
                        }
                        else
                        {
                            output("Ошибка: Очередь пуста, невозможно удалить элемент.");
                        }
                    }
                    else if (operation == "3")
                    {
                        if (!queue.IsEmpty())
                        {
                            output($"Первый элемент в очереди: {queue.Peek()}");
                        }
                        else
                        {
                            output("Очередь пуста.");
                        }
                    }
                    else if (operation == "4")
                    {
                        output(queue.IsEmpty() ? "Очередь пуста." : "Очередь не пуста.");
                    }
                    else if (operation == "5")
                    {
                        output("Содержимое очереди:");
                        queue.PrintQueue(output);
                    }
                    else
                    {
                        output($"Неизвестная операция: {operation}");
                    }
                }
            }
            catch (FileNotFoundException)
            {
                output("Ошибка: Файл не найден.");
            }
            catch (Exception ex)
            {
                output($"Ошибка: {ex.Message}");
            }
        }
    }
}
