using System.Diagnostics;
using System.IO;
using System.Windows;
using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using Logic;
using Queue;

namespace WpfApp
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
        }
        private void PopulateStackTasks()
        {
            TaskSelector.Items.Clear();
            TaskSelector.Items.Add("Выполнение команд стека");
            TaskSelector.Items.Add("Вычисление ОПЗ");
            TaskSelector.Items.Add("Перевод записи в ОПЗ");
            TaskSelector.Items.Add("График");
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
            TaskSelector.Items.Add("8. Если Е входит в L");
            TaskSelector.Items.Add("9. Дописать к списоку L список Е");
            TaskSelector.Items.Add("10. Разбить на два списка");
            TaskSelector.Items.Add("11. Удваивает список");
            TaskSelector.Items.Add("12. Меняет местами два элемента");
        }
        private void TaskSelector_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            string selectedTask = TaskSelector.SelectedItem?.ToString();
            if (selectedTask == "Перевод записи в ОПЗ")
            {
                FilePathTextBox.Text = string.Empty;
                FilePathTextBlock.Text = "Ввести свою запись";
            }
            else
            {
                FilePathTextBlock.Text = "Другой путь к файлу";
            }
            if (selectedTask == "График")
            {
                DisplayCanvas.Visibility = Visibility.Collapsed; // Скрываем Canvas
                OutputTextBox.Visibility = Visibility.Collapsed; // Скрываем TextBox
                MyChart.Visibility = Visibility.Visible; // Показываем график
            }
            else
            {
                DisplayCanvas.Visibility = Visibility.Visible; // Показываем Canvas
                OutputTextBox.Visibility = Visibility.Visible; // Показываем TextBox
                MyChart.Visibility = Visibility.Collapsed; // Скрываем график
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
            string filePath = FilePathTextBox.Text;
            string selectedTaskOption = TaskSelector.SelectedItem?.ToString();

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
                else if (selectedTaskOption == "График")
                {
                    string generatedFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Generator.txt");
                    // Генерация 100 команд (можно изменить количество)
                    GenerateCommandsFile(generatedFilePath, 1000);
                    // Использование сгенерированного файла для замера времени и построения графика
                    GenerateGraph(generatedFilePath);
                }
            }

            if ((selectedTaskOption == "Выполнение команд стека" || selectedTaskOption == "Вычисление ОПЗ" || selectedTaskOption == "Выполнение команд очереди" || selectedTaskOption == "График") && !File.Exists(filePath))
            {
                OutputTextBox.Text = "Ошибка: Указан неверный путь к файлу.";
                return;
            }

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
            else if (selectedTaskOption == "График")
            {
                GenerateGraph(filePath);
            }
            else
            {
                OutputTextBox.Text = "Ошибка: Неизвестное задание.";
            }
        }
        
        //Выполнение команд стека
        private void ExecuteStackCommands(string filePath)
        {
            try
            {
                Logic.Stack<string> stack = new Logic.Stack<string>();
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
        
        //График
        private void GenerateGraph(string filePath)
        {
            try
            {
                // Проверка пути к файлу и установка стандартного файла, если путь не указан
                if (string.IsNullOrWhiteSpace(filePath))
                {
                    filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "input.txt");
                }

                // Проверка существования файла
                if (!File.Exists(filePath))
                {
                    OutputTextBox.Text = "Ошибка: Файл не найден.";
                    return;
                }

                // Считывание содержимого файла
                string[] operations = File.ReadAllText(filePath).Split(' ');

                // Подготовка данных для графика
                ChartValues<ObservablePoint> points = new ChartValues<ObservablePoint>();

                for (int i = 0; i < operations.Length; i++)
                {
                    double totalExecutionTime = 0;
                    int runs = 10; // Количество прогонов для усреднения

                    for (int run = 0; run < runs; run++)
                    {
                        Logic.Stack<string> stack = new Logic.Stack<string>();
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
                            stack.Print(output => { }); // Пустой вывод
                        }

                        executionStopwatch.Stop();
                        totalExecutionTime += executionStopwatch.Elapsed.TotalMilliseconds;
                    }

                    double averageExecutionTime = totalExecutionTime / runs; // Среднее время выполнения

                    // Добавление точки на график
                    points.Add(new ObservablePoint(i + 1, averageExecutionTime)); // i + 1 для оси X (номер операции)
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
                    Fill = System.Windows.Media.Brushes.LightBlue // Заливка области под графиком
                });

                MyChart.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                OutputTextBox.Text = $"Ошибка при генерации графика: {ex.Message}";
            }
        }
        
        private void GenerateCommandsFile(string filePath, int numberOfCommands)
        {
            string[] possibleCommands = { "1,2", "1,3", "1,4", "1,5", "1,cat", "1,dog", "1,pig", "1,hello", "2", "3", "4", "5" }; // Пример возможных команд
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

        
        //Выполнение команд очереди
        private void ExecuteQueueCommands(string filePath)
        {
            try
            {
                CustomQueue<string> queue = new CustomQueue<string>();
                QueueCommandExecutor<string> executor = new QueueCommandExecutor<string>(queue);
                executor.ExecuteQueueCommandsFromFile(filePath, text => OutputTextBox.AppendText(text + Environment.NewLine));
            }
            catch (Exception ex)
            {
                OutputTextBox.Text = $"Ошибка при выполнении команд очереди: {ex.Message}";
            }
        }
        
        
    }
}
