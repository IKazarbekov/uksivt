using System.Runtime.CompilerServices;
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

namespace Work_Doc_Моё_решение
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Data.Load();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new RegistratinWindow().Show();
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var user = Data.users.Where(u => u.Login == login.Text).FirstOrDefault();
            if (user == null)
            {
                MessageBox.Show("Логин не найден!", "Хаха", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (user.Password == password.Password)
            {
                if (user.role == Role.OPERATOR)
                    new OperatorWindow().Show();
                else
                    new WorkerWindow(user.Name).Show();
                Close();
            }
            else
            {
                MessageBox.Show("Не верный пароль", "Хохо", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            Update();
        }

        void Update()
        {
            button.IsEnabled = login.Text.Trim().Length > 0 && password.Password.Trim().Length > 0;
        }

        private void password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            Update();
        }
    }
}