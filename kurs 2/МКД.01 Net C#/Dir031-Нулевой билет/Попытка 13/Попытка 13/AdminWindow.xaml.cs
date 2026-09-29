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
    /// Логика взаимодействия для AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        public AdminWindow()
        {
            InitializeComponent();

            Data.Read();
            lessons.ItemsSource = Data.Lessons;
            brones.ItemsSource = Data.Brones;
            lessonsForBrone.ItemsSource = Data.Lessons;
            users.ItemsSource = Data.Users;
            date.DisplayDateStart = DateTime.Now;
        }

        private void Button_Click_BroneAdd(object sender, RoutedEventArgs e)
        {
            int id = 0;
            foreach (var l in Data.Brones)
                if (id < l.Id)
                    id = l.Id + 1;
            var brone = new Brone()
            {
                Id = id,
                Date = DateTime.Now,
                LessonId = ((Lesson)lessonsForBrone.SelectedItem).Id,
                UserId = ((User)users.SelectedItem).Id,
                Accept = true
            };
            Data.Brones.Add(brone);
            Data.Write();
        }

        private void Button_Click_LessonAdd(object sender, RoutedEventArgs e)
        {
            int id = 0;
            foreach (var l in Data.Lessons)
                if (id <=  l.Id)
                    id = l.Id + 1;
            try
            {
                var lesson = new Lesson()
                {
                    Id = id,
                    Count = int.Parse(count.Text),
                    Date = date.SelectedDate.Value,
                    Trenner = trenner.Text,
                    Type = type.Text
                };

                var existsLesson = Data.Lessons.Where(l => l.Date == date.SelectedDate.Value && l.Type == type.Text).FirstOrDefault();
                if (existsLesson != null)
                {
                    MessageBox.Show("Такая тренировка в этот день уже существует");
                    return;
                }

                Data.Lessons.Add(lesson);
                Data.Write();
                type.Clear();
                trenner.Clear();
                count.Clear();
                date.SelectedDate = DateTime.Now;
            }
            catch
            {
                MessageBox.Show("Введите кол-во мест нормально");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            Close();
        }

        private void Button_Click_Size(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
                WindowState = WindowState.Normal;
            else
                WindowState = WindowState.Maximized;
        }

        private void trenner_TextChanged(object sender, TextChangedEventArgs e)
        {
            EnabledButtons();
        }

        void EnabledButtons()
        {
            buttonLesson.IsEnabled = type.Text.Trim().Length > 0 && trenner.Text.Trim().Length > 0 && count.Text.Trim().Length > 0 && date.SelectedDate.HasValue;
            buttonBrone.IsEnabled = lessonsForBrone.SelectedItem != null && users.SelectedItem != null;
        }

        private void date_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            EnabledButtons();
        }

        private void lessonsForBrone_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            EnabledButtons();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click_See(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Button_Click_Close(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
