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

namespace Попытка3_в_кабинете
{
    /// <summary>
    /// Логика взаимодействия для Window_MainMenu.xaml
    /// </summary>
    public partial class Window_MainMenu : Window
    {
        public Window_MainMenu()
        {
            InitializeComponent();

            listView_user.ItemsSource = Data.Users;
            dataGridTrainings.ItemsSource = Data.Lessons;
            dataGridBrone.ItemsSource = Data.Brones;
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            Close();
        }
    }
}
