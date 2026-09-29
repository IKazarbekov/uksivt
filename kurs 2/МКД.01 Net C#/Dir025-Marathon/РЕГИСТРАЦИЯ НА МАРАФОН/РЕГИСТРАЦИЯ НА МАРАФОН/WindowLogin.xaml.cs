using System.Windows;

namespace РЕГИСТРАЦИЯ_НА_МАРАФОН
{
    /// <summary>
    /// Логика взаимодействия для WindowLogin.xaml
    /// </summary>
    public partial class WindowLogin : Window
    {
        public WindowLogin()
        {
            InitializeComponent();
        }

        private void RoundButtonLogin_Click(object sender, RoutedEventArgs e)
        {
            string email = textBoxEmail.Text;
            string password = textBoxPassword.Text;
            var runnerList = Data.Runners.Where(runner => runner.Email == email && runner.Password == password).ToList();
            if (runnerList.Count > 0)
            {
                if (runnerList[0].Role == "Админ")
                    new WindowSuperUser().Show();
                else
                    new WindowEndRegister().Show();
                Close();
            }
            else
            {
                MessageBox.Show("Почта или пароль не верны");
            }
        }

        private void RoundButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            Close();
        }

        private void TopPanel_Click(object sender, RoutedEventArgs e)
        {
            this.ToStart();
        }
    }
}
