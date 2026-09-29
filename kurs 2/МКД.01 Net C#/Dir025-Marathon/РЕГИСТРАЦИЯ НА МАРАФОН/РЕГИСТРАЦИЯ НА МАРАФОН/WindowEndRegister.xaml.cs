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

namespace РЕГИСТРАЦИЯ_НА_МАРАФОН
{
    /// <summary>
    /// Логика взаимодействия для WindowEndRegister.xaml
    /// </summary>
    public partial class WindowEndRegister : Window
    {
        public WindowEndRegister()
        {
            InitializeComponent();
        }

        private void TopPanel_Click(object sender, RoutedEventArgs e)
        {
            this.ToStart();
        }
    }
}
