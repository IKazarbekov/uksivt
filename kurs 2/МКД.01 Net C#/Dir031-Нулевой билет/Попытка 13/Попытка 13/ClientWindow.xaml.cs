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
using Попытка_13.models;

namespace Попытка_13
{
    /// <summary>
    /// Логика взаимодействия для ClientWindow.xaml
    /// </summary>
    public partial class ClientWindow : Window
    {
        public ClientWindow(User user)
        {
            InitializeComponent();

            var brones = new List<Brone>();
            foreach (var b in Data.Brones)
                if (b.UserId == user.Id)
                    brones.Add(b);
            foreach (var b in brones)
                stacked.Children.Add(new UserControl1(b));
        }

        private void Button_Click_Size(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
                WindowState = WindowState.Normal;
            else
                WindowState = WindowState.Maximized;
        }

        private void Button_Click_Exit(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            Close();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Button_Click_Close(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
