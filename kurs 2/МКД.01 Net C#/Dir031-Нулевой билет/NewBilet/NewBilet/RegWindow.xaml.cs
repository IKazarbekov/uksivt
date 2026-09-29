using NewBilet.models;
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

namespace NewBilet
{
    /// <summary>
    /// Логика взаимодействия для RegWindow.xaml
    /// </summary>
    public partial class RegWindow : Window
    {
        public RegWindow()
        {
            InitializeComponent();
        }

        private void log_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            Close();
        }

        private void reg_Click(object sender, RoutedEventArgs e)
        {
            User existsUser = Data.Users.Where(user => user.Name == login.Text).FirstOrDefault();
            if (existsUser != null)
            {
                MessageBox.Show("Логин занят, введите другой", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            foreach (char c in number.Text)
                if (!char.IsDigit(c))
                {
                    MessageBox.Show("В номере телефона не должно быть букв", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            if (number.Text.Length < 8 && number.Text.Length > 14)
            {
                MessageBox.Show("Кол-во цифр номера неверное", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            int id = 0;
            foreach (var u in Data.Users)
                if (u.ID > id)
                    id = u.ID;
            User user = new User() { ID = id, Name = login.Text, Password = password.Text, Role = UserRole.CLIENT, Number = number.Text };
            Data.Users.Add(user);
            Data.WriteFile();
            new ClientWindow().Show();
            Close();
        }
    }
}
