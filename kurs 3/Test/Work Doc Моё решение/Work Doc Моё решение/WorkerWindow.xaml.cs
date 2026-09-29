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

namespace Work_Doc_Моё_решение
{
    /// <summary>
    /// Логика взаимодействия для WorkerWindow.xaml
    /// </summary>
    public partial class WorkerWindow : Window
    {
        string UserName;
        ObservableCollection<Problem> problems = new ObservableCollection<Problem>();
        public WorkerWindow(string UserName)
        {
            this.UserName = UserName;
            InitializeComponent();
            list.ItemsSource = Data.problems;
            date.SelectedDate = DateTime.Now;
            name.Text = UserName;
            problems.Clear();
            foreach(var pr in Data.problems)
            {
                if (pr.UserName == UserName)
                    problems.Add(pr);
            }
        }

        private void describe_TextChanged(object sender, TextChangedEventArgs e)
        {
            Update();
        }

        string Describe()
        {
            return new TextRange(describe.Document.ContentStart, describe.Document.ContentEnd).Text;
        }

        void Update()
        {
            try
            {
                button.IsEnabled = Describe().Length > 0 && int.Parse(room.Text) > 0;
            }
            catch
            {
                button.IsEnabled = false;
            }
        }

        private void room_TextChanged(object sender, TextChangedEventArgs e)
        {
            Update();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Problem problem = new Problem()
            {
                Describe = Describe(),
                UserName = this.UserName,
                Date = date.SelectedDate.Value,
                Room = int.Parse(room.Text),
                IsAccept = false,
            };
            Data.problems.Add(problem);
            Data.Save();
            MessageBox.Show("Заявка отправлена", "Всё", MessageBoxButton.OK, MessageBoxImage.Information);
            describe.Document = new FlowDocument();
            room.Clear();
            problems.Clear();
            foreach (var pr in Data.problems)
            {
                if (pr.UserName == UserName)
                    problems.Add(pr);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            Close();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
