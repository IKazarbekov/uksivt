using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Work_Doc_Моё_решение
{
    /// <summary>
    /// Логика взаимодействия для OperatorWindow.xaml
    /// </summary>
    public partial class OperatorWindow : Window
    {
        public OperatorWindow()
        {
            InitializeComponent();
            list.ItemsSource = Data.problems;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            Close();
        }

        private void button_Click_1(object sender, RoutedEventArgs e)
        {
            var problem = list.SelectedItem as Problem;
            problem.IsAccept = true;
            Data.Save();
        }

        private void list_Selected(object sender, RoutedEventArgs e)
        {
            var problem = list.SelectedItem as Problem;
            if (problem != null)
            {
                button.IsEnabled = problem.IsAccept == false;
            }
            
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
