using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfDelegat
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {


        public delegate void Handler(string message);
        public class Client
        {
            public string Name { get; set; }
            private int mount;
            public int Mount
            {
                get
                {
                    Handler?.Invoke($"Было получено данные баланса у {Name}. Текущий баланс: {mount}");
                    return mount;
                }
                set
                {
                    if (value < 0)
                    {
                        Handler?.Invoke($"Была попытка присвоить отрицательный баланс у {Name}. Текущий баланс: {mount}");
                        return;
                    }
                    Handler?.Invoke($"Изменён баланс у {Name}. Текущий баланс: {value}");
                    mount = value;
                }
            }
            public Handler Handler { get; set; }
            public Client()
            {
                mount = 1000;
            }
        }
        Client client = new Client();

        public MainWindow()
        {
            InitializeComponent();
            client.Name = "Bob";
            client.Mount = 500;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            client.Mount += 100;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            client.Mount -= 100;

        }
        void MBox(string mes) => MessageBox.Show(mes);

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (client.Handler != MBox)
            {
                client.Handler = MBox;
                MessageBox.Show("MEssageBOx accept");
            }

        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            if (client.Handler == MBox)
            {
                client.Handler -= MBox;
                MessageBox.Show("MEssageBOx delete");
            }
        }

        void SSS(string s)
        {
            textBlock1.Text = s;
        }
        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            client.Handler += SSS;
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            client.Handler -= SSS;

        }
        void SSS2(string s)
        {
            textBlock2.Text = s;
        }
        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            client.Handler += SSS2;
        }

        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            client.Handler -= SSS2;

        }
    }
}
