using System.IO;
using System.Windows;
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
            TaskSelector.Items.Add("Перевернуть список L");
            TaskSelector.Items.Add("Меняем местами");
            TaskSelector.Items.Add("Различные элементы");
            TaskSelector.Items.Add("Удалить неуникальные элементы");
            TaskSelector.Items.Add("Вставка самого себя");
            TaskSelector.Items.Add("Новый элемент Е");
            TaskSelector.Items.Add("Удалить все элементы Е");
            TaskSelector.Items.Add("если Е входит в L");
            TaskSelector.Items.Add("Дописать к списоку L список Е");
            TaskSelector.Items.Add("Разбить на два списка");
            TaskSelector.Items.Add("Удваивает список");
            TaskSelector.Items.Add("Меняет местами два элемента");
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
            }

            if ((selectedTaskOption == "Выполнение команд стека" || selectedTaskOption == "Вычисление ОПЗ" || selectedTaskOption == "Выполнение команд очереди") && !File.Exists(filePath))
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

        //Перевод записи в ОПЗ
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
