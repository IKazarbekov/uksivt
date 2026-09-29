using PROJECT_ЭКЗАМЕН_1_ПОПЫТКА.models;
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

namespace PROJECT_ЭКЗАМЕН_1_ПОПЫТКА
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Data.ReadOrCreateData();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string password = textBoxPassword.Text;
            string login = textBoxLogin.Text;

            User user = Data.Users.Where(user => user.Login == login && user.Password == password).FirstOrDefault();
            if (user == null)
            {
                MessageBox.Show("Неверный логин или пароль");
                return;
            }
            Data.CurrentUser = user;
            if (user.Role == UserRole.ADMIN)
            {
                new AdminWindow().Show();
            }
            else
            {
                new ClientWindow().Show();
            }
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            new AdminWindow().Show();
            Close();
        }
    }
}