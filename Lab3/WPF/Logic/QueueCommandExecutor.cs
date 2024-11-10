using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic
{
    public class QueueCommandExecutor<T>
    {
        private readonly IQueue<T> queue;

        public QueueCommandExecutor(IQueue<T> queue)
        {
            this.queue = queue;
        }

        // Метод для выполнения команд из файла
        public void ExecuteQueueCommandsFromFile(string filePath, Action<string> output)
        {
            try
            {
                string[] operations = File.ReadAllText(filePath).Split(' ');

                foreach (string operation in operations)
                {
                    if (operation.StartsWith("1,"))
                    {
                        // Операция вставки
                        string value = operation.Substring(2); // Извлекаем значение после "1,"
                        queue.Enqueue((T)Convert.ChangeType(value, typeof(T)));
                        output($"Элемент '{value}' добавлен в очередь.");
                    }
                    else if (operation == "2")
                    {
                        // Операция удаления
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
                        // Операция просмотра начала очереди
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
                        // Операция проверки на пустоту
                        output(queue.IsEmpty() ? "Очередь пуста." : "Очередь не пуста.");
                    }
                    else if (operation == "5")
                    {
                        // Операция печати всей очереди
                        output("Содержимое очереди:");
                        queue.PrintQueue();
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
