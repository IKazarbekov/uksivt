using NewBilet.models;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NewBilet
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Data.ReadOrCreateFile();
        }

        private void log_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(login.Text))
            {
                MessageBox.Show("Пустое поле логина", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrEmpty(password.Text))
            {
                MessageBox.Show("Пустое поле пароля", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            User user = Data.Users.Where(user => user.Name == login.Text && user.Password == password.Text).FirstOrDefault();
            if (user == null)
            {
                MessageBox.Show("Логин или пароль неверные", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (user.Role == UserRole.ADMIN)
                new AdminWindow().Show();
            else
                new ClientWindow().Show();

            Close();
        }

        private void reg_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            Close();
        }
    }
}