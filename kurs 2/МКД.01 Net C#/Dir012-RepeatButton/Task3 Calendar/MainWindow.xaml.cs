using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Task3_Calendar
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            collectionViewSource.Source = tasks;
            listBoxTasks.ItemsSource = collectionViewSource.View;
        }

        CollectionViewSource collectionViewSource = new CollectionViewSource();

        class Task
        {
            public DateTime Date {  get; set; }
            public string Description { get; set; }
            public bool IsCompleted { get; set; }
        }

        ObservableCollection<Task> tasks = new ObservableCollection<Task>()
        {
            new Task {
            Date = new DateTime(2026, 1, 15),
            Description = "Купить новогоднюю ёлку (со скидкой 90%)",
            IsCompleted = true
        },
        new Task {
            Date = new DateTime(2026, 1, 20),
            Description = "Сходить на каток с друзьями",
            IsCompleted = true
        },
        new Task {
            Date = new DateTime(2026, 1, 25),
            Description = "Написать курсовую по C#",
            IsCompleted = false
        },
        
        // ФЕВРАЛЬ 2026
        new Task {
            Date = new DateTime(2026, 2, 14),
            Description = "Купить подарок на День Святого Валентина",
            IsCompleted = false
        },
        new Task {
            Date = new DateTime(2026, 2, 23),
            Description = "Поздравить папу с 23 февраля",
            IsCompleted = false
        },
        
        // МАРТ 2026 (весна)
        new Task {
            Date = new DateTime(2026, 3, 1),
            Description = "Помыть окна после зимы",
            IsCompleted = false
        },
        new Task {
            Date = new DateTime(2026, 3, 8),
            Description = "Купить цветы маме на 8 марта",
            IsCompleted = false
        },
        new Task {
            Date = new DateTime(2026, 3, 15),
            Description = "Выбросить новогоднюю ёлку (пора)",
            IsCompleted = true
        },
        
        // АПРЕЛЬ 2026
        new Task {
            Date = new DateTime(2026, 4, 1),
            Description = "Никому не верить (первое апреля)",
            IsCompleted = false
        },
        new Task {
            Date = new DateTime(2026, 4, 12),
            Description = "Убраться в комнате",
            IsCompleted = false
        },
        new Task {
            Date = new DateTime(2026, 4, 20),
            Description = "Сдать отчет на работе",
            IsCompleted = false
        },
        new Task {
            Date = new DateTime(2026, 4, 25),
            Description = "Купить новый жесткий диск",
            IsCompleted = true
        },
        
        // МАЙ 2026
        new Task {
            Date = new DateTime(2026, 5, 1),
            Description = "Шашлыки на майские",
            IsCompleted = false
        },
        new Task {
            Date = new DateTime(2026, 5, 9),
            Description = "Поздравить ветеранов",
            IsCompleted = false
        },
        new Task {
            Date = new DateTime(2026, 5, 15),
            Description = "Переустановить Windows 7",
            IsCompleted = true
        },
        new Task {
            Date = new DateTime(2026, 5, 25),
            Description = "Начать учить английский (опять)",
            IsCompleted = false
        },
        
        // ИЮНЬ 2026 (лето)
        new Task {
            Date = new DateTime(2026, 6, 1),
            Description = "Купить солнцезащитные очки",
            IsCompleted = false
        },
        new Task {
            Date = new DateTime(2026, 6, 10),
            Description = "Поехать на море",
            IsCompleted = false
        },
        new Task {
            Date = new DateTime(2026, 6, 20),
            Description = "Сделать ремонт в ванной",
            IsCompleted = false
        },
        new Task {
            Date = new DateTime(2026, 6, 30),
            Description = "Подвести итоги полугодия",
            IsCompleted = false
        }
        };

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            InputWindow inputWindow = new InputWindow();
            inputWindow.ShowDialog();

            string text = inputWindow.textBox.Text;
            DateTime? dateh = inputWindow.calendar.SelectedDate;

            if (!dateh.HasValue)
            {
                MessageBox.Show("Выберите дату");
                return;
            }

            DateTime date = dateh.Value;

            var task = new Task() { Date = date, Description = text, IsCompleted = false};

            tasks.Add(task);

        }

        private void Calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            List<DateTime> dates = calendar.SelectedDates.ToList();

            collectionViewSource.View.Filter = objTask =>
            {
                Task task = objTask as Task;

                return dates.Contains(task.Date);
            };

            collectionViewSource.View.Refresh();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            collectionViewSource.View.Filter = null;
            calendar.SelectedDates.Clear();
            foreach (Task task in tasks)
                calendar.SelectedDates.Add(task.Date);
        }
    }
}
