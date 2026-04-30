using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Task
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void TabControl_Selected(object sender, RoutedEventArgs e)
        {
            string result = null;
            if (radioButton1.IsChecked == true)
            {
                if (radioButton5.IsChecked == true)
                {
                    result = "Ёжик";
                }
                else
                {
                    result = "Крош";
                }
            }
            else if (radioButton2.IsChecked == true)
            {
                result = "Пин";
            }
            else if (radioButton3.IsChecked == true)
            {
                if (radioButton5.IsChecked == true ||
                    radioButton23.IsChecked == true)
                {
                    if (checkBox.IsChecked == true)
                    {
                        result = "Бараш";
                    }
                    else
                    {
                        result = "Кар-Карыч";
                    }
                }
                else
                {
                    result = "Нюша";
                }
            }
            else if (radioButton20.IsChecked == true)
            {
                if (checkBox.IsChecked == true)
                {
                    result = "Копатыч";
                }
                else
                {
                    result = "Совунья";
                }
            }
            else if (radioButton4.IsChecked == true)
            {
                result = "Пин";
            }
            else if (radioButton20.IsChecked == true)
            {
                if (radioButton5.IsChecked == true)
                    result = "Совунья";
                if (radioButton6.IsChecked == true)
                    result = "Копатыч";
            }

            if (result == null) { 
                tabItemResult.Visibility = Visibility.Collapsed;
            }
            else
            {
                tabItemResult.Visibility = Visibility.Visible;
                labelResult.Content = result;
            }
        }

    }
}
