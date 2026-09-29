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
using Попытка_12.models;

namespace Попытка_12
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Data.ReadData();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            User user = Data.Users.Where(u => u.Login == login.Text && u.Password == password.Password).FirstOrDefault();
            if (user == null)
            {
                MessageBox.Show("Такого пользователя не существует, проверьте логин или пароль");
                return;
            }
            if (user.Role == UserRole.ADMIN)
                new AdminWindow().Show();
            else
                new ClientWindow(user.Id).Show();
            this.Close();
        }
    }
}