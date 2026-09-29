using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using на_сдачу_учебной_пратикик.models;

namespace на_сдачу_учебной_пратикик
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
            dataGridLesson.ItemsSource = Data.Lessons;
            dataPickerLesson.DisplayDateStart = DateTime.Now;
            UpdateButtonAddLesson();
            dataGridLessonForBrone.ItemsSource = Data.Lessons;
            dataGridUserForBrone.ItemsSource = Data.Users;
            dataGridBrone.ItemsSource = Data.Brones;
        }
        private void dataPickerLesson_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateButtonAddLesson();
        }
        private void UpdateButtonAddLesson()
        {
            if (textBoxTrenner.Text.Trim().Length > 0 && comboBoxLessonType.SelectedItem != null && textBoxCount.Text.Trim().Length > 0 && textBoxCount.Text.All(char.IsDigit) && dataPickerLesson.SelectedDate.HasValue)
            {
                buttonAddLesson.IsEnabled = true;
            }
            else
            {
                buttonAddLesson.IsEnabled = false;
            }
        }
        private void buttonAddLesson_Click(object sender, RoutedEventArgs e)
        {
            int id = 0;
            foreach (var l in Data.Lessons)
            {
                if (id <= l.Id)
                {
                    id = l.Id + 1;
                }
            }
            Lesson lesson = new Lesson()
            {
                Id = id,
                Count = int.Parse(textBoxCount.Text),
                Date = dataPickerLesson.SelectedDate.Value,
                Type = comboBoxLessonType.Text,
                Trenner = textBoxTrenner.Text,
            };

            Data.Lessons.Add(lesson);
        }
        private void comboBoxLessonType_Selected(object sender, RoutedEventArgs e)
        {
            UpdateButtonAddLesson();
        }
        private void textBoxTrenner_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateButtonAddLesson();
        }
        private void textBoxCount_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateButtonAddLesson();
        }
        private void Window_Closed(object sender, EventArgs e)
        {
            Data.Write();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
                WindowState = WindowState.Normal;
            else
                WindowState = WindowState.Maximized;
                
        }

        private void dataGridUserForBrone_Selected(object sender, RoutedEventArgs e)
        {
            buttonAddBrone.Content = "Добавить";
            buttonAddBrone.IsEnabled = dataGridLessonForBrone.SelectedItem != null && dataGridUserForBrone.SelectedItem != null;
        }

        private void buttonAddBrone_Click(object sender, RoutedEventArgs e)
        {
            User user = dataGridUserForBrone.SelectedItem as User;
            Lesson lesson = dataGridLessonForBrone.SelectedItem as Lesson;
            Brone has_brone = Data.Brones.Where(b => b.UserID == user.ID && b.LessonID == lesson.Id).FirstOrDefault();
            if (has_brone != null)
            {
                MessageBox.Show("Такая бронь уже оформлена", "Бронь существует", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            int id = 0;
            foreach (Brone b in Data.Brones)
                if (id <= b.Id)
                    id = b.Id + 1;
            Brone brone = new Brone() {
                Id = id,
                LessonID = lesson.Id,
                UserID = user.ID,
                Date = DateTime.Now,
                Accept = true,
            };
            Data.Brones.Add(brone);
            dataGridLessonForBrone.SelectedItem = null;
            dataGridUserForBrone.SelectedItem = null;
            buttonAddBrone.Content = "Готово";
            buttonAddBrone.IsEnabled = false;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            new LoginWindow().Show();
            Close();
        }
    }
}
