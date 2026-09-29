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
using Попытка_11.models;

namespace Попытка_11
{
    /// <summary>
    /// Логика взаимодействия для AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        public AdminWindow()
        {
            InitializeComponent();
            dataGrid_Lessons.ItemsSource = Data.Lessons;
            
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Data.WriteData();
        }

        private void Button_Click_AddLesson(object sender, RoutedEventArgs e)
        {

            Lesson lesson = new Lesson() { };
        }
    }
}
