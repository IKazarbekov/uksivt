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
using System.Windows.Shapes;
using на_сдачу_учебной_пратикик.models;

namespace на_сдачу_учебной_пратикик
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void textBoxLogin_TextChanged(object sender, RoutedEventArgs e)
        {
            buttonLogin.IsEnabled = textBoxLogin.Text.Trim().Length > 0 && textBoxPassword.Password.Trim().Length > 0;
        }

        private void textBoxPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            buttonLogin.IsEnabled = textBoxLogin.Text.Trim().Length > 0 && textBoxPassword.Password.Trim().Length > 0;
        }

        private void buttonZabil_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Как жаль вас");
        }

        private void buttonLogin_Click(object sender, RoutedEventArgs e)
        {
            User user = Data.Users.Where(u => u.Login == textBoxLogin.Text && u.Password == textBoxPassword.Password).FirstOrDefault();
            if (user == null)
            {
                MessageBox.Show("Неверный логин или пароль", "Ошибка авторизации", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (user.Role == UserRole.ADMIN)
                new AdminWindow().Show();
            else
                new ClientWindow(user).Show();
            Close();
        }
    }
}
