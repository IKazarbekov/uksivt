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
using Попытка3_в_кабинете.models;

namespace Попытка3_в_кабинете
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

        private void ButtonTestAdmin_Click(object sender, RoutedEventArgs e)
        {
            new Window_MainMenu().Show();
            Close();
        }

        private void ButtonLogin_Click(object sender, RoutedEventArgs e)
        {
            string login = textBox_login.Text;
            string password = textBox_password.Text;
            try
            {
                User user = Data.Users.Where(user => user.Login == login && user.Password == password).ToList()[0];
                new WIndow_Client(user.ID).Show();
                Close();
            }
            catch(ArgumentOutOfRangeException exception)
            {
                MessageBox.Show("Логин или пароль неверны");
            }
        }
    }
}