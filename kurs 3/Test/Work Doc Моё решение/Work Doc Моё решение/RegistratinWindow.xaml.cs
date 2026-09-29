using System;
using System.Collections.Generic;
using System.Configuration;
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
    /// Логика взаимодействия для RegistratinWindow.xaml
    /// </summary>
    public partial class RegistratinWindow : Window
    {
        public RegistratinWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (Data.users.Where(u => u.Login == login.Text).Count() > 0) {
                MessageBox.Show("Логин занят", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            User user = new User()
            {
                Login = login.Text.Trim(),
                Name = name.Text.Trim(),
                Password = password.Password.Trim(),
                role = role.Text == "Сотрудник" ? Role.WORKER : Role.OPERATOR
            };
            Data.users.Add(user);
            MessageBox.Show("Вы зарегистрированы!", "Ура", MessageBoxButton.OK, MessageBoxImage.Information);
            Data.Save();
            new MainWindow().Show();
            Close();
        }

        private void login_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateButton();
        }

        void UpdateButton()
        {
            button.IsEnabled = login.Text.Trim().Length > 0 && password.Password.Trim().Length > 0 && role.SelectedIndex >= 0 && name.Text.Trim().Length > 0;
        }

        private void role_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateButton();
        }
    }
}
