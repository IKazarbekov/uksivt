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
using Попытка_12.models;

namespace Попытка_12
{
    /// <summary>
    /// Логика взаимодействия для AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        public AdminWindow()
        {
            InitializeComponent();

            Data.ReadData();
            ViewLessons.ItemsSource = Data.Lessons;
            users.ItemsSource = Data.Users;
            lessonsForBroning.ItemsSource = Data.Lessons;
            brones.ItemsSource = Data.Brones;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Data.WriteData();
            new MainWindow().Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int id = 0;
            foreach (var l in Data.Lessons)
                if (id <+ l.Id)
                    id = l.Id + 1;
            var lesson = new Lesson()
            {
                Id = id,
                Date = date.SelectedDate.Value,
                Trenner = trenner.Text,
                Type = type.Text,
                Count = int.Parse(count.Text),
            };
            Data.Lessons.Add(lesson);
            Data.WriteData();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            User user = users.SelectedItem as User;
            Lesson lesson = lessonsForBroning.SelectedItem as Lesson;
            int id = 0;
            foreach (var b in Data.Brones)
                if (id <= b.Id)
                    id = b.Id + 1;
            Brone brone = new Brone()
            {
                Id = id,
                UserId = user.Id,
                LessonId = lesson.Id,
                Accept = true
            };
            Data.Brones.Add(brone);
            Data.WriteData();
        }
    }
}
