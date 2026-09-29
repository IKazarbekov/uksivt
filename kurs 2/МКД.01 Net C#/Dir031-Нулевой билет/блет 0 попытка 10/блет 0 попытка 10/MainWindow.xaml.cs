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

namespace блет_0_попытка_10
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {/*
            if (string.IsNullOrEmpty(login.Text.Trim()) && string.IsNullOrEmpty(password.Text))
            {
                MessageBox.Show("Пустые поля");
                return;
            }*/
            new MainWindow().Show();
            
        }
    }
}