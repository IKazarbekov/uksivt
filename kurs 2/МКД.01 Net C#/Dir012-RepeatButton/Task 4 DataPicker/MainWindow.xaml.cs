using System;
using System.Collections.Generic;
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

namespace Task_4_DataPicker
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public const int PRICE = 1_000_000;

        public MainWindow()
        {
            InitializeComponent();
            dataPickerStart.DisplayDateStart = DateTime.Today;
            dataPickerEnd.DisplayDateStart = DateTime.Today;
            foreach (var dateRange in list)
            {
                dataPickerStart.BlackoutDates.Add(dateRange);
                dataPickerEnd.BlackoutDates.Add(dateRange);
            }
        }

        List<CalendarDateRange> list = new List<CalendarDateRange>()
        {
            new CalendarDateRange(new DateTime(2026, 3, 10), new DateTime(2026, 3, 15)),
            new CalendarDateRange(new DateTime(2026, 3, 25), new DateTime(2026, 3, 27))
        };

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DateTime? dateStart = dataPickerStart.SelectedDate;
            DateTime? dateEnd = dataPickerEnd.SelectedDate;

            if (!dateStart.HasValue)
            {
                MessageBox.Show("Выберите начало даты бронирования");
                return;
            }

            if (!dateEnd.HasValue)
            {
                MessageBox.Show("Выберите конец даты бронирования");
                return;
            }

            if (dateStart.Value > dateEnd.Value) {
                MessageBox.Show("Конец бронирования должен быть после начала");
                return;
            }

            int resultPrice = PRICE * (dateEnd.Value - dateStart.Value).Days;

            if (checkBox.IsChecked.Value)
                resultPrice += 1_000_300;

            MessageBox.Show($"Место успешно забронировано ! Итоговая цена - {resultPrice} рублей. Но так как на вашем счёте недостаточно денег, ваш банк уже одобрил кредит :)");
            /*
            list.Add(new CalendarDateRange(dateStart.Value, dateEnd.Value));

            var sortedList = list.OrderBy(r => r.Start).ToList();

            dataPickerStart.BlackoutDates.Clear();
            dataPickerEnd.BlackoutDates.Clear();

            foreach (var dateRange in sortedList)
            {
                dataPickerStart.BlackoutDates.Add(dateRange);
                dataPickerEnd.BlackoutDates.Add(dateRange);
            }*/
        }
    }
}
