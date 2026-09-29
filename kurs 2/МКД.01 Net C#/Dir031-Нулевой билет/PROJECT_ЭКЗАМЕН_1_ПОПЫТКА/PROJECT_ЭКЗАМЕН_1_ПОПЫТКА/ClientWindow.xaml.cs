using PROJECT_ЭКЗАМЕН_1_ПОПЫТКА.models;
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

namespace PROJECT_ЭКЗАМЕН_1_ПОПЫТКА
{
    /// <summary>
    /// Логика взаимодействия для ClientWindow.xaml
    /// </summary>
    public partial class ClientWindow : Window
    {
        ObservableCollection<Brone> lessons;
        public ClientWindow()
        {
            InitializeComponent();

            lessons = new ObservableCollection<Brone>(Data.Brones.Where(l => l.UserID == Data.CurrentUser.ID).ToList());
            DataContext = lessons;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();

        }
    }
}
