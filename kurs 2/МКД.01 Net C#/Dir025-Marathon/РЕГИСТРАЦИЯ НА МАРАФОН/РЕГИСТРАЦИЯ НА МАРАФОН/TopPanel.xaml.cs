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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace РЕГИСТРАЦИЯ_НА_МАРАФОН
{
    /// <summary>
    /// Логика взаимодействия для TopPanel.xaml
    /// </summary>
    public partial class TopPanel : UserControl
    {
        public event RoutedEventHandler Click
        {
            add { buttonExit.Click += value; }
            remove { buttonExit.Click -= value; }
        }
        public event RoutedEventHandler ClickLogout
        {
            add {
                buttonLogout.Click += value;
                buttonLogout.Visibility = Visibility.Visible;
            }
            remove { buttonLogout.Click -= value; }
        }
        public TopPanel()
        {
            InitializeComponent();
        }

    }
}
