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
            TaskSelector.SelectedIndex = 0; // Устанавливаем начальное значение
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            string filePath = FilePathTextBox.Text;

            if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
            {
                OutputTextBox.Text = "Ошибка: Указан неверный путь к файлу.";
                return;
            }

            string selectedTask = TaskSelector.SelectedItem.ToString();

            if (selectedTask == "Выполнение команд стека")
            {
                ExecuteStackCommands(filePath);
            }
            else if (selectedTask == "Вычисление ОПЗ")
            {
                EvaluatePostfixExpression(filePath);
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
                // Создаем стек и выполняем команды из файла
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
    }
}
