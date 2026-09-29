using System.Windows;
using System.Windows.Media;
using Билет0_Попытка_1.data;

namespace Билет0_Попытка_1
{
    /// <summary>
    /// Логика взаимодействия для WindowRegistration.xaml
    /// </summary>
    public partial class WindowRegistration : Window
    {
        public WindowRegistration()
        {
            InitializeComponent();
            Data._ClearData();
        }

        private void Button_Click_OpenWindowAuth(object sender, RoutedEventArgs e)
        {
            // Open auth window
            new WindowAuth().Show();
            Close();
        }

        private void Button_Click_Registration(object sender, RoutedEventArgs e)
        {
            // Check all textBoxs and write errors in textBlocks

            // Input datas
            bool isCurrectData = true;
            string stringLogin = textBlockLogin.Text;
            string stringPassword = textBlockPassword.Text;
            string stringConfirmPassword = textBlockConfirmPassword.Text;
            string stringPhoneNumber = textBlockPhone.Text;

            // Check login
            textBlockLogin.Foreground = Brushes.Red;
            textBoxLogin.BorderBrush = Brushes.Red;
            if (stringLogin.Length < 4)
            {
                isCurrectData = false;
                textBlockLogin.Text = "Логин минимум 4 символа";
            }
            else
            {
                textBlockLogin.Foreground = Brushes.Black;
                textBoxLogin.BorderBrush = Brushes.Black;
                textBlockLogin.Text = "Логин ✓";
            }

            // Check password
            textBlockPassword.Foreground = Brushes.Red;
            textBoxPassword.BorderBrush = Brushes.Red;
            if (stringPassword.Length < 6)
            {
                isCurrectData = false;
                textBlockConfirmPassword.Text = "Пароли не равны";
            }
            else
            {
                textBlockConfirmPassword.Text = "Пароль ✓";
                textBlockPassword.Foreground = Brushes.Black;
                textBoxPassword.BorderBrush = Brushes.Black;
                // Check confirm password
                textBlockConfirmPassword.Foreground = Brushes.Red;
                textBoxConfirmPassword.BorderBrush = Brushes.Red;
                if (stringPassword.Equals( stringConfirmPassword))
                {
                    isCurrectData = false;
                    textBlockConfirmPassword.Text = "Пароли не равны";
                }
                else
                {
                    textBlockConfirmPassword.Text = "Повторите пароль ✓";
                    textBlockConfirmPassword.Foreground = Brushes.Red;
                    textBoxConfirmPassword.BorderBrush = Brushes.Red;
                }
            }

            // Check phone number
            textBlockPhone.Foreground = Brushes.Red;
            textBoxPhone.BorderBrush = Brushes.Red;
            if (stringPhoneNumber.Length > 11)
            {
                isCurrectData = false;
                textBlockPhone.Text = "Некоректный телефон";
            }
            else if (stringPhoneNumber.Length < 11)
            {
                isCurrectData = false;
                textBlockPhone.Text = "Некоректный телефон";
            }
            else
            {
                textBlockPhone.Foreground = Brushes.Black;
                textBoxPhone.BorderBrush = Brushes.Black;
                textBlockPhone.Text = "Телефон ✓";
            }

            // open next window
            if (isCurrectData)
            {
                new WindowAuth().Show();
                Close();
            }
        }
    }
}
