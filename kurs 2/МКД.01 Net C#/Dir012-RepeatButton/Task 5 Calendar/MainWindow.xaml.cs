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

namespace Task_5_Calendar
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            var dateStart = calendar.SelectedDates[0];
            var dateEnd = calendar.SelectedDates[calendar.SelectedDates.Count - 1];

            var result = $" C {dateStart.ToString("d")} по {dateEnd.ToString("d")}";

            if ((dateEnd - dateStart).Days >= 7)
                result += " Внимание - много дней !";

            textBlock.Text = result;
        }
    }
}
