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

namespace Попытка_14
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Data.Read();
        }

        private void Button_Click_Size(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
                WindowState = WindowState.Normal;
            else WindowState = WindowState.Maximized;
        }

        private void Button_Click_Close(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_Click_Hidden(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void login_TextChanged(object sender, TextChangedEventArgs e)
        {
            buttonLogin.IsEnabled = login.Text.Trim().Length > 0 && password.Password.Trim().Length > 0;
        }

        private void password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            buttonLogin.IsEnabled = login.Text.Trim().Length > 0 && password.Password.Trim().Length > 0;
        }

        private void buttonLogin_Click(object sender, RoutedEventArgs e)
        {
            User user = Data.Users.Where(u => u.Login == login.Text.Trim() && u.Password == password.Password).FirstOrDefault();

            if (user == null)
            {
                MessageBox.Show("Неверный логин или пароль", "Ошибка входа", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (user.Role == UserRole.ADMIN)
                new AdminWindow().Show();
            else
                new ClientWindow(user).Show();

            Close();
        }

        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch
            {

            }
        }
    }
}