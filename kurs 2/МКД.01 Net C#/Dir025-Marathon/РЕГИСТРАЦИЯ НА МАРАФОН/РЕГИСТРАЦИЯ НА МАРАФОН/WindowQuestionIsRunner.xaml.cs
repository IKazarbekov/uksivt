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
using РЕГИСТРАЦИЯ_НА_МАРАФОН;

namespace РЕГИСТРАЦИЯ_НА_МАРАФОН
{
    /// <summary>
    /// Логика взаимодействия для WindowQuestionIsRunner.xaml
    /// </summary>
    public partial class WindowQuestionIsRunner : Window
    {
        public WindowQuestionIsRunner()
        {
            InitializeComponent();
        }

        private void RoundButtonRegister_Click(object sender, RoutedEventArgs e)
        {
            this.ToRegister();
        }

        private void RoundButtonLogin_Click(object sender, RoutedEventArgs e)
        {
            this.ToLogin();
        }

        private void TopPanel_Click(object sender, RoutedEventArgs e)
        {
            this.ToStart();
        }

        private void RoundButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
