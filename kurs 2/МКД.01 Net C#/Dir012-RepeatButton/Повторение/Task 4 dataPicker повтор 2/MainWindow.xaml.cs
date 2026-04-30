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

namespace Task_4_dataPicker_повтор_2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            datePickerStart.DisplayDateStart = DateTime.Today;
            datePickerStart.DisplayDateEnd = DateTime.Today;

            List<CalendarDateRange> list = new List<CalendarDateRange>()
            {
                new CalendarDateRange(new DateTime(2026, 2, 20), new DateTime(2026, 3, 10)),
                new CalendarDateRange(new DateTime(2026, 3, 20), new DateTime(2026, 4, 13))
            };

            foreach (var dateRange in list)
            {
                datePickerStart.BlackoutDates.Add(dateRange);
                datePickerEnd.BlackoutDates.Add(dateRange);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var dateStart = datePickerStart.SelectedDate;
            var dateEnd = datePickerEnd.SelectedDate;

            if (!dateStart.HasValue)
            {
                MessageBox.Show("Выберите начальную дату");
                return;
            }

            if (!dateEnd.HasValue)
            {
                MessageBox.Show("Выберите конечную дату");
                return;
            }

            if (dateStart.Value > dateEnd.Value)
            {
                MessageBox.Show("Начальная дата должна быть раньше чем конечная");
                return;
            }

            int PRICE = 1_000_000;

            int result = (dateEnd.Value - dateStart.Value).Days * PRICE;

            if (checkBox.IsChecked == true)
                result += 1_354_243;

            MessageBox.Show("Вы забронированы ! Сумма " + result);
        }
    }
}
