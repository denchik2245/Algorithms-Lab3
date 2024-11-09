using System;
using System.IO;
using System.Windows;
using Logic;

namespace WpfApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            TaskSelector.Items.Add("Выполнение команд стека");
            TaskSelector.Items.Add("Вычисление ОПЗ");
            TaskSelector.Items.Add("Перевод записи в ОПЗ");
            TaskSelector.SelectedIndex = 0;

            TaskSelector.SelectionChanged += TaskSelector_SelectionChanged;
        }

        private void TaskSelector_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            string selectedTask = TaskSelector.SelectedItem?.ToString();
            if (selectedTask == "Перевод записи в ОПЗ")
            {
                FilePathTextBox.Text = string.Empty; // Очищаем текстовое поле
                FilePathTextBlock.Text = "Ввести свою запись"; // Меняем текст метки
            }
            else
            {
                FilePathTextBlock.Text = "Другой путь к файлу"; // Возвращаем текст метки
            }
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            string filePath = FilePathTextBox.Text;

            string selectedTaskOption = TaskSelector.SelectedItem?.ToString();

            // Проверка пустого поля и установка пути по умолчанию в зависимости от выбранной задачи
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
            }

            // Проверяем существование файла только для задач, где нужен путь к файлу (исключая "Перевод записи в ОПЗ")
            if ((selectedTaskOption == "Выполнение команд стека" || selectedTaskOption == "Вычисление ОПЗ") && !File.Exists(filePath))
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
                // Если поле ввода не пустое, используем его содержимое, иначе читаем из файла
                string infixExpression = string.IsNullOrWhiteSpace(FilePathTextBox.Text)
                    ? File.ReadAllText(filePath) // Считываем инфиксное выражение из файла
                    : FilePathTextBox.Text; // Используем введенное выражение

                if (string.IsNullOrWhiteSpace(infixExpression))
                {
                    OutputTextBox.Text = "Ошибка: Введите инфиксное выражение.";
                    return;
                }

                ConvertInfixToPostfix(infixExpression);
            }
            else
            {
                OutputTextBox.Text = "Ошибка: Неизвестное задание.";
            }
        }

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
    }
}
