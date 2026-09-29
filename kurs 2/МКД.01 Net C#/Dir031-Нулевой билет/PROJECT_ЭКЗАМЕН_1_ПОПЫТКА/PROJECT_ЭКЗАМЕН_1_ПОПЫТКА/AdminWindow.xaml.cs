using PROJECT_ЭКЗАМЕН_1_ПОПЫТКА.models;
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

namespace PROJECT_ЭКЗАМЕН_1_ПОПЫТКА
{
    /// <summary>
    /// Логика взаимодействия для AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        public AdminWindow()
        {
            InitializeComponent();

            viewUser.ItemsSource = Data.Users;
            viewBrone.ItemsSource = Data.Brones;
            viewLesson.ItemsSource = Data.Lessons;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Data.WriteData();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int id = 0;
            foreach (Lesson l in Data.Lessons)
            {
                if (l.ID > id)
                    id = l.ID;
            }
            int count = int.Parse(textBoxCountSpace.Text);
            string trainer = textBoxTranner.Text;
            DateTime date = datePicker.SelectedDate.Value;
            Lesson lesson = new Lesson() { ID =  id , CountSpace = count, Trainer = trainer, dateTime = date};
            Data.Lessons.Add(lesson);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Lesson lesson = (Lesson)viewLesson.SelectedItem;
            Data.Lessons.Remove(lesson);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            User user = viewUser.SelectedItem as User;
            if (user == null)
            {
                MessageBox.Show("Выберите пользователя");
                return;
            }
            Lesson lesson = viewLesson.SelectedItem as Lesson;
            if (lesson == null)
            {
                MessageBox.Show("Выберите тренировку");
                return;
            }
            if (!datePickerBrone.SelectedDate.HasValue)
            {
                MessageBox.Show("Выберите дату бронировки");
                return;
            }
            int id = 0;
            foreach (Brone l in Data.Brones)
            {
                if (l.ID > id)
                    id = l.ID;
            }
            BroneStatus status = (checkBox.IsChecked.Value) ? BroneStatus.ACCEPT : BroneStatus.CANCEL;


            DateTime date = datePickerBrone.SelectedDate.Value;
            Brone brone = new Brone() { Date = date, ID = id, LessonID = lesson.ID, Status = status, UserID = user.ID };
            Data.Brones.Add(brone);
        }
    }
}
