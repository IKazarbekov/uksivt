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

namespace Попытка_13
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var user = Data.Users.Where(u => u.Login == login.Text && u.Password == password.Password).FirstOrDefault();
            
            if (user == null)
            {
                MessageBox.Show("Проверьте логин или пароль");
                return;
            }

            if (user.Role == models.UserRole.ADMIN)
                new AdminWindow().Show();
            else
                new ClientWindow(user).Show();
            Close();
        }

        private void login_TextChanged(object sender, TextChangedEventArgs e)
        {
            EnabledButton();
        }

        private void password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            EnabledButton();
        }

        void EnabledButton()
        {
            button.IsEnabled = login.Text.Trim().Length > 0 && password.Password.Trim().Length > 0;
        }

        private void Button_Click_Close(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
    }
}