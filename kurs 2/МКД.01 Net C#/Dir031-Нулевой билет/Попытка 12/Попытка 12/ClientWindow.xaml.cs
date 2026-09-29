using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Попытка_12.models;

namespace Попытка_12
{
    /// <summary>
    /// Логика взаимодействия для ClientWindow.xaml
    /// </summary>
    public partial class ClientWindow : Window
    {
        public ClientWindow(int UserId)
        {
            InitializeComponent();

            List<Brone> brones = new List<Brone>();
            foreach (var b in Data.Brones)
                if (b.Id == UserId)
                    brones.Add(b);

            foreach (var b in brones)
            {
                brones_stack.Children.Add(
                    new UserControl1(b)
                    );
            }
        }
    }
}
