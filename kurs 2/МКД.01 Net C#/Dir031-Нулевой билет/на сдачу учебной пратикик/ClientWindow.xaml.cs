using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Shapes;
using на_сдачу_учебной_пратикик.models;

namespace на_сдачу_учебной_пратикик
{
    /// <summary>
    /// Логика взаимодействия для ClientWindow.xaml
    /// </summary>
    public partial class ClientWindow : Window
    {
        class Item
        {
            public string Type { get; set; }
            public string Trenner { get; set; }
            public string DateLesson { get; set; }
            public string Date { get; set; }
            public string Status { get; set; }
        }
        ObservableCollection<Item> items = new ObservableCollection<Item>();
        public ClientWindow(User user)
        {
            InitializeComponent();
            foreach (Brone brone in Data.Brones)
            {
                if (brone.UserID == user.ID)
                {
                    Lesson lesson = Data.Lessons.Where(l => l.Id == brone.Id).First();
                    items.Add(new Item()
                    {
                        Date = brone.Date.ToShortDateString(),
                        Status = brone.Accept ? "Принят" : "Отказано",
                        Type = lesson.Type,
                        DateLesson = lesson.Date.ToShortDateString(),
                        Trenner = lesson.Trenner
                    });
                }    
            }
            DataContext = items;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new LoginWindow().Show();
            Close();
        }
    }
}
