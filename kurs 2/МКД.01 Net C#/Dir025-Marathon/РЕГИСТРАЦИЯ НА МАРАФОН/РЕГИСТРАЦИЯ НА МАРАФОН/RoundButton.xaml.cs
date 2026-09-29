using System.Windows;
using System.Windows.Controls;

namespace РЕГИСТРАЦИЯ_НА_МАРАФОН
{
    /// <summary>
    /// Логика взаимодействия для RoundButton.xaml
    /// </summary>
    public partial class RoundButton : UserControl
    {
        public object Text
        {
            get => button.Content; set => button.Content = value;
        }

        public event RoutedEventHandler Click
        {
            add {  button.Click += value; }
            remove { button.Click -= value; }
        }

        public RoundButton()
        {
            InitializeComponent();
        }
    }
}
