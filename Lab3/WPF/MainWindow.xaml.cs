using System.Diagnostics;
using System.IO;
using System.Windows;
using Lab3.Queue;
using Lab3.Stack;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;

namespace Lab3
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            StackRadioButton.IsChecked = true;
            StackRadioButton.Checked += StackRadioButton_Checked;
            QueueRadioButton.Checked += QueueRadioButton_Checked;
            CustomListRadioButton.Checked += CustomListRadioButton_Checked;
            PopulateStackTasks();
            TaskSelector.SelectedIndex = 0;
            TaskSelector.SelectionChanged += TaskSelector_SelectionChanged;
            ConfigureChart();
        }

        private void StackRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            PopulateStackTasks();
        }
        private void QueueRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            PopulateQueueTasks();
        }
        private void CustomListRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            PopulateCustomListTasks();
            FilePathTextBlock.Text = "Ввести элемент";
        }
        private void PopulateStackTasks()
        {
            TaskSelector.Items.Clear();
            TaskSelector.Items.Add("Выполнение команд стека");
            TaskSelector.Items.Add("Вычисление ОПЗ");
            TaskSelector.Items.Add("Перевод записи в ОПЗ");
            TaskSelector.Items.Add("График команд стека");
            TaskSelector.Items.Add("График вычисления ОПЗ");
        }
        private void PopulateQueueTasks()
        {
            TaskSelector.Items.Clear();
            TaskSelector.Items.Add("Выполнение команд очереди");
            TaskSelector.Items.Add("График");
        }
        private void PopulateCustomListTasks()
        {
            TaskSelector.Items.Clear();
            TaskSelector.Items.Add("1. Перевернуть список L");
            TaskSelector.Items.Add("2. Меняем местами");
            TaskSelector.Items.Add("3. Различные элементы");
            TaskSelector.Items.Add("4. Удалить неуникальные элементы");
            TaskSelector.Items.Add("5. Вставка самого себя");
            TaskSelector.Items.Add("6. Новый элемент Е");
            TaskSelector.Items.Add("7. Удалить все элементы Е");
            TaskSelector.Items.Add("8. Вставка элемента перед первым вхождением");
            TaskSelector.Items.Add("9. Дописать к списку L список E");
            TaskSelector.Items.Add("10. Разбить список по первому вхождению");
            TaskSelector.Items.Add("11. Удвоить список");
            TaskSelector.Items.Add("12. Поменять местами два элемента");
        }
        private void TaskSelector_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            string selectedTask = TaskSelector.SelectedItem?.ToString();
            FilePathTextBox.Text = string.Empty;

            if (CustomListRadioButton.IsChecked == true)
            {
                FilePathTextBlock.Text = "Ввести элемент";
            }
            else if (selectedTask == "Перевод записи в ОПЗ")
            {
                FilePathTextBlock.Text = "Ввести свою запись";
            }
            else
            {
                FilePathTextBlock.Text = "Другой путь к файлу";
            }

            if (selectedTask == "График" || selectedTask == "График команд стека" || selectedTask == "График вычисления ОПЗ")
            {
                DisplayCanvas.Visibility = Visibility.Collapsed;
                OutputTextBox.Visibility = Visibility.Collapsed;
                MyChart.Visibility = Visibility.Visible;
            }
            else
            {
                DisplayCanvas.Visibility = Visibility.Visible;
                OutputTextBox.Visibility = Visibility.Visible;
                MyChart.Visibility = Visibility.Collapsed;
            }
        }
        
        private void ConfigureChart()
        {
            MyChart.AxisX.Clear();
            MyChart.AxisX.Add(new Axis
            {
                Title = "Количество операций",
                LabelFormatter = value => value.ToString("F0"),
                MinValue = 0
            });

            MyChart.AxisY.Clear();
            MyChart.AxisY.Add(new Axis
            {
                Title = "Время выполнения (мс)",
                LabelFormatter = value => value.ToString("F0"),
                MinValue = 0
            });

            MyChart.AnimationsSpeed = TimeSpan.FromMilliseconds(300);
            MyChart.Zoom = ZoomingOptions.Xy;
            MyChart.LegendLocation = LegendLocation.None;
        }
        
        //Кнопка "Начать"
        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            DisplayCanvas.Children.Clear();
            OutputTextBox.Clear();
            string selectedTaskOption = TaskSelector.SelectedItem?.ToString();

            if (selectedTaskOption.StartsWith("5. Вставка самого себя"))
            {
                string listFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "List.txt");
                if (!File.Exists(listFilePath))
                {
                    OutputTextBox.Text = "Ошибка: Файл List.txt не найден.";
                    return;
                }
                
                var list = LoadListFromFile(listFilePath);
                
                if (list == null || list.IsEmpty())
                {
                    OutputTextBox.Text = "Ошибка: Список из файла пуст или не был загружен.";
                    return;
                }
                
                OutputTextBox.Text = "Изначальный список: " + list.GetAllElementsAsString() + Environment.NewLine;
                
                if (int.TryParse(FilePathTextBox.Text, out int x))
                {
                    list.InsertSelfAfterFirstOccurrence(x);
                    
                    string result = list.GetAllElementsAsString();
                    OutputTextBox.Text += result != null ? "Модифицированный список: " + result : "Ошибка: Список не был модифицирован.";
                }
                else
                {
                    OutputTextBox.Text = "Ошибка: Укажите корректное значение для числа x.";
                }
            }
            else if (selectedTaskOption.StartsWith("6. Новый элемент Е"))
            {
                string listFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "List.txt");
                if (!File.Exists(listFilePath))
                {
                    OutputTextBox.Text = "Ошибка: Файл List.txt не найден.";
                    return;
                }
                
                var list = LoadListFromFile(listFilePath);
                
                if (list == null || list.IsEmpty())
                {
                    OutputTextBox.Text = "Ошибка: Список из файла пуст или не был загружен.";
                    return;
                }
                
                OutputTextBox.Text = "Изначальный список: " + list.GetAllElementsAsString() + Environment.NewLine;
                
                if (int.TryParse(FilePathTextBox.Text, out int element))
                {
                    list.InsertInOrder(element);
                    
                    string result = list.GetAllElementsAsString();
                    OutputTextBox.Text += result != null ? "Модифицированный список: " + result : "Ошибка: Список не был модифицирован.";
                }
                else
                {
                    OutputTextBox.Text = "Ошибка: Укажите корректное значение для элемента E.";
                }
            }
            else if (selectedTaskOption.StartsWith("7. Удалить все элементы Е"))
            {
                // Чтение списка из файла List.txt
                string listFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "List.txt");
                if (!File.Exists(listFilePath))
                {
                    OutputTextBox.Text = "Ошибка: Файл List.txt не найден.";
                    return;
                }

                // Загружаем список из файла
                var list = LoadListFromFile(listFilePath);

                // Проверяем, что список загружен
                if (list == null || list.IsEmpty())
                {
                    OutputTextBox.Text = "Ошибка: Список из файла пуст или не был загружен.";
                    return;
                }

                // Выводим изначальный список
                OutputTextBox.Text = "Изначальный список: " + list.GetAllElementsAsString() + Environment.NewLine;

                // Используем значение из FilePathTextBox как элемент для удаления
                if (int.TryParse(FilePathTextBox.Text, out int elementToRemove))
                {
                    // Удаляем все вхождения элемента
                    list.RemoveAllOccurrences(elementToRemove);

                    // Проверяем результат и выводим модифицированный список
                    string result = list.GetAllElementsAsString();
                    OutputTextBox.Text += result != null ? "Список после удаления всех вхождений элемента " + elementToRemove + ": " + result : "Ошибка: Список не был модифицирован.";
                }
                else
                {
                    OutputTextBox.Text = "Ошибка: Укажите корректное значение для элемента E.";
                }
            }
            else if (selectedTaskOption.StartsWith("8. Вставка элемента перед первым вхождением"))
            {
                // Чтение списка из файла List.txt
                string listFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "List.txt");
                if (!File.Exists(listFilePath))
                {
                    OutputTextBox.Text = "Ошибка: Файл List.txt не найден.";
                    return;
                }

                // Загружаем список из файла
                var list = LoadListFromFile(listFilePath);

                // Проверяем, что список загружен
                if (list == null || list.IsEmpty())
                {
                    OutputTextBox.Text = "Ошибка: Список из файла пуст или не был загружен.";
                    return;
                }

                OutputTextBox.Text = "Изначальный список: " + list.GetAllElementsAsString() + Environment.NewLine;

                // Чтение двух чисел (новый элемент и целевой элемент) из пользовательского ввода
                string[] inputElements = FilePathTextBox.Text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (inputElements.Length == 2 && int.TryParse(inputElements[0], out int newElement) && int.TryParse(inputElements[1], out int targetElement))
                {
                    list.InsertBeforeFirstOccurrence(newElement, targetElement);
                    OutputTextBox.Text += $"Список после вставки элемента {newElement} перед первым вхождением {targetElement}: " + list.GetAllElementsAsString();
                }
                else
                {
                    OutputTextBox.Text += "Ошибка: Укажите два числа (новый элемент и целевой элемент) через пробел.";
                }
            }
            else if (selectedTaskOption.StartsWith("9. Дописать к списку L список E"))
            {
                // Чтение списка из файла List.txt
                string listFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "List.txt");
                if (!File.Exists(listFilePath))
                {
                    OutputTextBox.Text = "Ошибка: Файл List.txt не найден.";
                    return;
                }

                // Загружаем список из файла
                var list = LoadListFromFile(listFilePath);

                // Проверяем, что список загружен
                if (list == null || list.IsEmpty())
                {
                    OutputTextBox.Text = "Ошибка: Список из файла пуст или не был загружен.";
                    return;
                }

                OutputTextBox.Text = "Изначальный список L: " + list.GetAllElementsAsString() + Environment.NewLine;

                // Чтение второго списка из пользовательского ввода
                string[] inputElements = FilePathTextBox.Text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                var secondList = new CustomLinkedList<int>();

                foreach (string element in inputElements)
                {
                    if (int.TryParse(element, out int number))
                    {
                        secondList.AddLast(number);
                    }
                }

                // Проверка на пустой второй список
                if (secondList.IsEmpty())
                {
                    OutputTextBox.Text += "Ошибка: Второй список пуст. Введите элементы через пробел.";
                    return;
                }

                // Дописываем второй список к первому
                list.AppendList(secondList);
                OutputTextBox.Text += "Список L после добавления списка E: " + list.GetAllElementsAsString();
            }
            else if (selectedTaskOption.StartsWith("10. Разбить список по первому вхождению"))
            {
                // Чтение списка из файла List.txt
                string listFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "List.txt");
                if (!File.Exists(listFilePath))
                {
                    OutputTextBox.Text = "Ошибка: Файл List.txt не найден.";
                    return;
                }

                // Загружаем список из файла
                var list = LoadListFromFile(listFilePath);

                // Проверяем, что список загружен
                if (list == null || list.IsEmpty())
                {
                    OutputTextBox.Text = "Ошибка: Список из файла пуст или не был загружен.";
                    return;
                }

                OutputTextBox.Text = "Изначальный список: " + list.GetAllElementsAsString() + Environment.NewLine;

                // Используем значение из FilePathTextBox как элемент для разделения
                if (int.TryParse(FilePathTextBox.Text, out int splitValue))
                {
                    var (firstList, secondList) = list.SplitAtFirstOccurrence(splitValue);

                    OutputTextBox.Text += "Первый список: " + firstList.GetAllElementsAsString() + Environment.NewLine;
                    OutputTextBox.Text += "Второй список: " + (secondList.IsEmpty() ? "пуст" : secondList.GetAllElementsAsString());
                }
                else
                {
                    OutputTextBox.Text = "Ошибка: Укажите корректное значение для числа.";
                }
            }
            else if (selectedTaskOption.StartsWith("12. Поменять местами два элемента"))
            {
                // Чтение списка из файла List.txt
                string listFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "List.txt");
                if (!File.Exists(listFilePath))
                {
                    OutputTextBox.Text = "Ошибка: Файл List.txt не найден.";
                    return;
                }

                // Загружаем список из файла
                var list = LoadListFromFile(listFilePath);

                // Проверяем, что список загружен
                if (list == null || list.IsEmpty())
                {
                    OutputTextBox.Text = "Ошибка: Список из файла пуст или не был загружен.";
                    return;
                }

                OutputTextBox.Text = "Изначальный список: " + list.GetAllElementsAsString() + Environment.NewLine;

                // Чтение двух элементов для обмена
                string[] inputElements = FilePathTextBox.Text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (inputElements.Length == 2 && int.TryParse(inputElements[0], out int firstElement) && int.TryParse(inputElements[1], out int secondElement))
                {
                    list.SwapElements(firstElement, secondElement);
                    OutputTextBox.Text += "Список после обмена элементов: " + list.GetAllElementsAsString();
                }
                else
                {
                    OutputTextBox.Text += "Ошибка: Укажите два элемента для обмена через пробел.";
                }
            }
            else
            {
                // Логика для остальных задач, где filePath может быть важен
                string filePath = FilePathTextBox.Text;
                if (string.IsNullOrWhiteSpace(filePath))
                {
                    if (selectedTaskOption == "Выполнение команд стека")
                    {
                        filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "input.txt");
                    }
                    else if (selectedTaskOption == "Вычисление ОПЗ")
                    {
                        filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "postfix.txt");
                    }
                    else if (selectedTaskOption == "Перевод записи в ОПЗ")
                    {
                        filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "infix.txt");
                    }
                    else if (selectedTaskOption == "Выполнение команд очереди")
                    {
                        filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "input.txt");
                    }
                    else if (selectedTaskOption == "График команд стека")
                    {
                        string generatedFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Generator.txt");
                        GenerateCommandsFile(generatedFilePath, 1000);
                        GenerateGraph(generatedFilePath);
                        return;
                    }
                    else if (selectedTaskOption == "График вычисления ОПЗ")
                    {
                        string generatedFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Generator2.txt");
                        GeneratePostfixExpressionsFile(generatedFilePath, 1000);
                        GeneratePostfixGraph(generatedFilePath);
                        return;
                    }

                    else if (selectedTaskOption.StartsWith("1. Перевернуть список L") ||
                             selectedTaskOption.StartsWith("2. Меняем местами") ||
                             selectedTaskOption.StartsWith("3. Различные элементы") ||
                             selectedTaskOption.StartsWith("4. Удалить неуникальные элементы") ||
                             selectedTaskOption.StartsWith("11. Удвоить список"))
                    {
                        filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "List.txt");
                    }
                }

                // Проверка на существование файла для остальных задач
                if ((selectedTaskOption == "Выполнение команд стека" || selectedTaskOption == "Вычисление ОПЗ" || selectedTaskOption == "Выполнение команд очереди" || selectedTaskOption == "График команд стека") && !File.Exists(filePath))
                {
                    OutputTextBox.Text = "Ошибка: Указан неверный путь к файлу.";
                    return;
                }

                // Обработка задач, которые требуют filePath
                if (selectedTaskOption == "Выполнение команд стека")
                {
                    ExecuteStackCommands(filePath);
                }
                else if (selectedTaskOption == "Вычисление ОПЗ")
                {
                    EvaluatePostfixExpression(filePath);
                }
                else if (selectedTaskOption == "Перевод записи в ОПЗ")
                {
                    string infixExpression = string.IsNullOrWhiteSpace(FilePathTextBox.Text)
                        ? File.ReadAllText(filePath)
                        : FilePathTextBox.Text;

                    if (string.IsNullOrWhiteSpace(infixExpression))
                    {
                        OutputTextBox.Text = "Ошибка: Введите инфиксное выражение.";
                        return;
                    }

                    ConvertInfixToPostfix(infixExpression);
                }
                else if (selectedTaskOption == "Выполнение команд очереди")
                {
                    ExecuteQueueCommands(filePath);
                }
                else if (selectedTaskOption.StartsWith("1. Перевернуть список L"))
                {
                    ExecuteListOperation(filePath, "Reverse");
                }
                else if (selectedTaskOption.StartsWith("2. Меняем местами"))
                {
                    ExecuteListOperation(filePath, "MoveElements");
                }
                else if (selectedTaskOption.StartsWith("3. Различные элементы"))
                {
                    ExecuteListOperation(filePath, "CountDistinct");
                }
                else if (selectedTaskOption.StartsWith("4. Удалить неуникальные элементы"))
                {
                    ExecuteListOperation(filePath, "RemoveNonUnique");
                }
                else if (selectedTaskOption.StartsWith("11. Удвоить список"))
                {
                    ExecuteListOperation(filePath, "Duplicate");
                }
                else
                {
                    OutputTextBox.Text = "Ошибка: Неизвестное задание.";
                }
            }
        }

        //Загрузка списка из файла
        private CustomLinkedList<int> LoadListFromFile(string filePath)
        {
            var list = new CustomLinkedList<int>();
            string fileContent = File.ReadAllText(filePath);
            
            string[] elements = fileContent.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            foreach (var element in elements)
            {
                if (int.TryParse(element, out int number))
                {
                    list.AddLast(number);
                }
            }

            return list;
        }
        
        //Выполнение команд стека
        private void ExecuteStackCommands(string filePath)
        {
            try
            {
                Lab3.Stack.Stack<string> stack = new Lab3.Stack.Stack<string>();
                stack.ExecuteCommandsFromFile(filePath, text => OutputTextBox.AppendText(text + Environment.NewLine));
            }
            catch (Exception ex)
            {
                OutputTextBox.Text = $"Ошибка при выполнении команд стека: {ex.Message}";
            }
        }

        //Вычисление ОПЗ
        private void EvaluatePostfixExpression(string filePath)
        {
            try
            {
                PostfixEvaluator<double> evaluator = new PostfixEvaluator<double>();
                double result = evaluator.EvaluateExpressionFromFile(filePath);
                OutputTextBox.Text = $"Результат вычисления ОПЗ: {result}";
            }
            catch (Exception ex)
            {
                OutputTextBox.Text = $"Ошибка при вычислении ОПЗ: {ex.Message}";
            }
        }

        //Перевод записи в  ОПЗ
        private void ConvertInfixToPostfix(string infixExpression)
        {
            try
            {
                InfixToPostfixConverter converter = new InfixToPostfixConverter();
                string postfixExpression = converter.Convert(infixExpression);
                OutputTextBox.Text = $"Изначальная инфиксная запись: {infixExpression}\nЗапись в ОПЗ: {postfixExpression}";
            }
            catch (Exception ex)
            {
                OutputTextBox.Text = $"Ошибка при преобразовании в ОПЗ: {ex.Message}";
            }
        }
        
        //График для стека
        private void GenerateGraph(string filePath)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(filePath))
                {
                    filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "input.txt");
                }
                
                if (!File.Exists(filePath))
                {
                    OutputTextBox.Text = "Ошибка: Файл не найден.";
                    return;
                }
                
                string[] operations = File.ReadAllText(filePath).Split(' ');
                
                ChartValues<ObservablePoint> points = new ChartValues<ObservablePoint>();

                for (int i = 0; i < operations.Length; i++)
                {
                    double totalExecutionTime = 0;
                    int runs = 10;

                    for (int run = 0; run < runs; run++)
                    {
                        Lab3.Stack.Stack<string> stack = new Lab3.Stack.Stack<string>();
                        Stopwatch executionStopwatch = new Stopwatch();
                        executionStopwatch.Start();

                        // Выполнение операции
                        string operation = operations[i];
                        if (operation.StartsWith("1,"))
                        {
                            string value = operation.Substring(2);
                            stack.Push(value);
                        }
                        else if (operation == "2")
                        {
                            if (!stack.IsEmpty())
                            {
                                stack.Pop();
                            }
                        }
                        else if (operation == "3")
                        {
                            if (!stack.IsEmpty())
                            {
                                stack.Top();
                            }
                        }
                        else if (operation == "4")
                        {
                            stack.IsEmpty();
                        }
                        else if (operation == "5")
                        {
                            stack.Print(output => { });
                        }

                        executionStopwatch.Stop();
                        totalExecutionTime += executionStopwatch.Elapsed.TotalMilliseconds;
                    }

                    double averageExecutionTime = totalExecutionTime / runs;

                    // Добавление точки на график
                    points.Add(new ObservablePoint(i + 1, averageExecutionTime));
                    OutputTextBox.AppendText($"Операция {i + 1} среднее время за {runs} прогонов: {averageExecutionTime:F5} мс\n");
                }

                // Настройка и отображение графика
                MyChart.Series.Clear();
                MyChart.AxisX.Clear();
                MyChart.AxisY.Clear();

                MyChart.AxisX.Add(new Axis
                {
                    Title = "Номер операции",
                    LabelFormatter = value => value.ToString("F0"),
                    MinValue = 1
                });

                MyChart.AxisY.Add(new Axis
                {
                    Title = "Время выполнения (мс)",
                    LabelFormatter = value => value.ToString("F5"),
                    MinValue = 0
                });

                MyChart.Series.Add(new LineSeries
                {
                    Title = "Среднее время выполнения",
                    Values = points,
                    PointGeometry = DefaultGeometries.Circle,
                    PointGeometrySize = 5,
                    Fill = System.Windows.Media.Brushes.LightBlue
                });

                MyChart.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                OutputTextBox.Text = $"Ошибка при генерации графика: {ex.Message}";
            }
        }
        
        //Генератор списка для графика
        private void GenerateCommandsFile(string filePath, int numberOfCommands)
        {
            string[] possibleCommands = { "1,2", "1,3", "1,4", "1,5", "1,cat", "1,dog", "1,pig", "1,hello", "2", "3", "4", "5" };
            Random random = new Random();
            List<string> commands = new List<string>();

            for (int i = 0; i < numberOfCommands; i++)
            {
                int randomIndex = random.Next(possibleCommands.Length);
                commands.Add(possibleCommands[randomIndex]);
            }

            File.WriteAllText(filePath, string.Join(" ", commands));
            OutputTextBox.AppendText($"Файл сгенерирован и содержит {numberOfCommands} команд.\n");
        }
        
        //График ОПЗ
        private void GeneratePostfixGraph(string filePath)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(filePath))
                {
                    filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Generator2.txt");
                }

                if (!File.Exists(filePath))
                {
                    OutputTextBox.Text = "Ошибка: Файл не найден.";
                    return;
                }

                string[] expressions = File.ReadAllLines(filePath);
                ChartValues<ObservablePoint> points = new ChartValues<ObservablePoint>();

                for (int i = 0; i < expressions.Length; i++)
                {
                    double totalExecutionTime = 0;
                    int runs = 10;

                    for (int run = 0; run < runs; run++)
                    {
                        Stopwatch executionStopwatch = new Stopwatch();
                        executionStopwatch.Start();

                        // Вычисление выражения
                        PostfixEvaluator<double> evaluator = new PostfixEvaluator<double>();
                        evaluator.EvaluateExpression(expressions[i]);

                        executionStopwatch.Stop();
                        totalExecutionTime += executionStopwatch.Elapsed.TotalMilliseconds;
                    }

                    double averageExecutionTime = totalExecutionTime / runs;

                    // Добавление точки на график
                    points.Add(new ObservablePoint(i + 1, averageExecutionTime));
                    OutputTextBox.AppendText($"Выражение {i + 1} среднее время за {runs} прогонов: {averageExecutionTime:F5} мс\n");
                }

                // Настройка и отображение графика
                MyChart.Series.Clear();
                MyChart.AxisX.Clear();
                MyChart.AxisY.Clear();

                MyChart.AxisX.Add(new Axis
                {
                    Title = "Номер выражения",
                    LabelFormatter = value => value.ToString("F0"),
                    MinValue = 1,
                });

                MyChart.AxisY.Add(new Axis
                {
                    Title = "Время выполнения (мс)",
                    LabelFormatter = value => value.ToString("F5"),
                    MinValue = 0,
                    MaxValue = points.Max(p => p.Y) * 0.1, // Установка максимального значения
                    Separator = new Separator
                    {
                        Step = (points.Max(p => p.Y) * 0.05) / 4, // Деление диапазона на 4 части для отображения 5 значений
                        IsEnabled = true
                    }
                });

                MyChart.Series.Add(new LineSeries
                {
                    Title = "Среднее время выполнения",
                    Values = points,
                    PointGeometry = DefaultGeometries.Circle,
                    PointGeometrySize = 5,
                    Fill = System.Windows.Media.Brushes.LightBlue
                });

                MyChart.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                OutputTextBox.Text = $"Ошибка при генерации графика: {ex.Message}";
            }
        }
        
        //Генератор списка выражений для графика
        private void GeneratePostfixExpressionsFile(string filePath, int numberOfExpressions)
        {
            string[] possibleOperands = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10" };
            string[] possibleOperators = { "+", "-", "*", "/", "ln", "cos", "sin", "sqrt" };
            Random random = new Random();
            List<string> expressions = new List<string>();

            for (int i = 0; i < numberOfExpressions; i++)
            {
                int operandCount = random.Next(3, 6); // Количество операндов (минимум 3)
                List<string> expression = new List<string>();

                for (int j = 0; j < operandCount; j++)
                {
                    int operandIndex = random.Next(possibleOperands.Length);
                    expression.Add(possibleOperands[operandIndex]);
                }

                // Добавление операторов
                for (int j = 0; j < operandCount - 1; j++)
                {
                    int operatorIndex = random.Next(possibleOperators.Length);
                    expression.Add(possibleOperators[operatorIndex]);
                }

                // Перемешивание, чтобы сохранить корректность постфиксной записи
                expressions.Add(string.Join(" ", expression));
            }

            File.WriteAllText(filePath, string.Join("\n", expressions));
            OutputTextBox.AppendText($"Файл сгенерирован и содержит {numberOfExpressions} выражений в постфиксной записи.\n");
        }

        
        //Выполнение команд очереди
        private void ExecuteQueueCommands(string filePath)
        {
            try
            {
                Lab3.Queue.CustomQueue<string> queue = new Lab3.Queue.CustomQueue<string>();
                QueueCommandExecutor<string> executor = new QueueCommandExecutor<string>(queue);
                executor.ExecuteQueueCommandsFromFile(filePath, text => OutputTextBox.AppendText(text + Environment.NewLine));
            }
            catch (Exception ex)
            {
                OutputTextBox.Text = $"Ошибка при выполнении команд очереди: {ex.Message}";
            }
        }
        
        //Операции над своим списком
        private void ExecuteListOperation(string filePath, string operation, int? parameter = null)
        {
            try
            {
                CustomLinkedList<int> LoadOriginalList()
                {
                    string[] elements = File.ReadAllText(filePath).Split(' ');
                    CustomLinkedList<int> list = new CustomLinkedList<int>();

                    foreach (string element in elements)
                    {
                        if (int.TryParse(element, out int number))
                        {
                            list.AddLast(number);
                        }
                    }

                    return list;
                }

                CustomLinkedList<int> list = LoadOriginalList();
                OutputTextBox.AppendText("Изначальный список: " + list.GetAllElementsAsString() + Environment.NewLine);

                if (operation == "Reverse")
                {
                    list.Reverse();
                    OutputTextBox.AppendText("Перевёрнутый список: " + list.GetAllElementsAsString() + Environment.NewLine);
                }
                else if (operation == "MoveElements")
                {
                    list.MoveLastToFirst();
                    OutputTextBox.AppendText("Список после переноса последнего элемента в начало: " + list.GetAllElementsAsString() + Environment.NewLine);

                    list = LoadOriginalList();
                    list.MoveFirstToLast();
                    OutputTextBox.AppendText("Список после переноса первого элемента в конец: " + list.GetAllElementsAsString() + Environment.NewLine);
                }
                else if (operation == "CountDistinct")
                {
                    int distinctCount = list.CountDistinct();
                    OutputTextBox.AppendText($"Количество различных элементов: {distinctCount}" + Environment.NewLine);
                }
                else if (operation == "RemoveNonUnique")
                {
                    list.RemoveNonUnique();
                    OutputTextBox.AppendText("Список после удаления неуникальных элементов: " + list.GetAllElementsAsString() + Environment.NewLine);
                }
                else if (operation == "InsertSelfAfterFirstOccurrence" && parameter.HasValue)
                {
                    list.InsertSelfAfterFirstOccurrence(parameter.Value);
                    OutputTextBox.AppendText($"Список после вставки самого в себя после первого вхождения {parameter.Value}: " + list.GetAllElementsAsString() + Environment.NewLine);
                }
                else if (operation == "InsertInOrder" && parameter.HasValue)
                {
                    list.InsertInOrder(parameter.Value);
                    OutputTextBox.AppendText($"Список после вставки элемента {parameter.Value} в упорядоченный список: " + list.GetAllElementsAsString() + Environment.NewLine);
                }
                else if (operation == "SplitAtFirstOccurrence" && parameter.HasValue)
                {
                    var (firstList, secondList) = list.SplitAtFirstOccurrence(parameter.Value);
                    OutputTextBox.AppendText($"Первый список после разделения: {firstList.GetAllElementsAsString()}" + Environment.NewLine);
                    OutputTextBox.AppendText($"Второй список после разделения: {(secondList.IsEmpty() ? "пуст" : secondList.GetAllElementsAsString())}" + Environment.NewLine);
                }
                else if (operation == "Duplicate")
                {
                    list.Duplicate();
                    OutputTextBox.AppendText("Список после удвоения: " + list.GetAllElementsAsString() + Environment.NewLine);
                }
                else if (operation == "SwapElements")
                {
                    // Ввод двух элементов для обмена через пробел
                    string[] inputElements = FilePathTextBox.Text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    if (inputElements.Length == 2 && int.TryParse(inputElements[0], out int firstElement) && int.TryParse(inputElements[1], out int secondElement))
                    {
                        list.SwapElements(firstElement, secondElement);
                        OutputTextBox.AppendText("Список после обмена элементов: " + list.GetAllElementsAsString() + Environment.NewLine);
                    }
                    else
                    {
                        OutputTextBox.AppendText("Ошибка: Укажите два элемента для обмена через пробел." + Environment.NewLine);
                    }
                }
                else if (operation == "AppendList")
                {
                    // Ввод второго списка пользователем через пробел
                    string[] inputElements = FilePathTextBox.Text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    var secondList = new CustomLinkedList<int>();

                    foreach (string element in inputElements)
                    {
                        if (int.TryParse(element, out int number))
                        {
                            secondList.AddLast(number);
                        }
                    }

                    list.AppendList(secondList);
                    OutputTextBox.AppendText("Список после добавления второго списка: " + list.GetAllElementsAsString() + Environment.NewLine);
                }
                else if (operation == "InsertBeforeFirstOccurrence")
                {
                    string[] inputElements = FilePathTextBox.Text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                    if (inputElements.Length == 2 && int.TryParse(inputElements[0], out int newElement) && int.TryParse(inputElements[1], out int targetElement))
                    {
                        list.InsertBeforeFirstOccurrence(newElement, targetElement);
                        OutputTextBox.AppendText($"Список после вставки элемента {newElement} перед первым вхождением {targetElement}: " + list.GetAllElementsAsString() + Environment.NewLine);
                    }
                    else
                    {
                        OutputTextBox.AppendText("Ошибка: Укажите два числа (новый элемент и целевой элемент) через пробел." + Environment.NewLine);
                    }
                }
                else
                {
                    OutputTextBox.AppendText("Ошибка: Неправильная операция или отсутствует параметр." + Environment.NewLine);
                }
            }
            catch (Exception ex)
            {
                OutputTextBox.Text = $"Ошибка при выполнении операций со списком: {ex.Message}";
            }
        }
    }
}
