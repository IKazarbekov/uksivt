using ShopTask;
using ShopTask.data;
using ShopTask.models;
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

namespace ShopTask2
{
    /// <summary>
    /// Логика взаимодействия для LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
            TextChanged(this, null);
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_Click2(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Как жалко");
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            User user = UsersData.Users.Where(u => TxtPassword.Password == u.password && TxtUsername.Text == u.login).FirstOrDefault();
            if (user == null)
                MessageBox.Show("Не верный логин или пароль");
            else
                new ProductsWindow(user).Show();
        }

        private void TextChanged(object sender, TextChangedEventArgs e)
        {
            BtnLogin.Visibility = TxtPassword.Password.Trim().Length > 0 && TxtUsername.Text.Trim().Length > 0 ? Visibility.Visible : Visibility.Collapsed;
        }

        private void TxtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            BtnLogin.Visibility = TxtPassword.Password.Trim().Length > 0 && TxtUsername.Text.Trim().Length > 0 ? Visibility.Visible : Visibility.Collapsed;
        }
    }
}
