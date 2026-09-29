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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Попытка_12.models;

namespace Попытка_12
{
    /// <summary>
    /// Логика взаимодействия для UserControl.xaml
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        public UserControl1(Brone brone)
        {
            InitializeComponent();

            Lesson lesson = Data.Lessons.Where(l => l.Id == brone.LessonId).First();
            User user = Data.Users.Where(l => l.Id == brone.UserId).First();

            trenner.Content = lesson.Trenner;
            type.Content = lesson.Type;
            accept.Content = brone.Accept;
            dateBrone.Text = brone.Date.ToShortDateString();
            dateLesson.Text = lesson.Date.ToShortDateString();
        }
    }
}
