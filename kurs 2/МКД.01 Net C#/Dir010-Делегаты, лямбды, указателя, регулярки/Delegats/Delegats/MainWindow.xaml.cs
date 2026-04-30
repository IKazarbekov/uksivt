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

namespace Delegats
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public delegate void Handler(string message);

        public class Client
        {
            public int Name { get; set; }
            public int Mount { get; set; }
            private Handler handler;
            public string MountString { get { return $"Текущий счёт: {Mount}"; } }
            public void SetHandler(Handler handler)
            {
                this.handler += handler;
            }

            public void RemoveHandler(Handler handler)
            {
                this.handler -= handler;
            }

            public void Add(int mount)
            {
                Mount += mount;
                handler?.Invoke($"Добавлены деньги {mount}.  {MountString}");
            }

            public void Pop(int mount)
            {
                if (Mount < mount)
                    handler?.Invoke($"Не достаточно средств чтобы снять {mount} денег");
                else
                {
                    Mount -= mount;
                    handler?.Invoke($"Средства сняты с карты, {mount} денег. {MountString}");
                }
            }
        }

        Client client = new Client();


        public MainWindow()
        {
            InitializeComponent();
    }

        void PrintMessage(string message)
        {
            MessageBox.Show(message);
        }

        void PrintInTextBlock(string message)
        {
            textBlock.Text = message;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            client.Add(1000);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            client.Pop(1000);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            client.SetHandler(PrintMessage);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            client.RemoveHandler(PrintMessage);
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            client.SetHandler(PrintInTextBlock);
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            client.RemoveHandler(PrintInTextBlock);
        }
    }
}
