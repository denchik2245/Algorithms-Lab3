﻿using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Logic;

namespace Lab3;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        LinkedStack<int> linkedStack = new LinkedStack<int>();

        // Добавление элементов в стек
        linkedStack.Push(10);
        linkedStack.Push(20);
        linkedStack.Push(30);

        // Вывод содержимого стека в TextBlock
        StackOutput.Text = linkedStack.PrintWPF();
    }
}