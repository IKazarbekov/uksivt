using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace Task1___повтор_2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataGridEmployees.ItemsSource = employees;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            employees.Add(new Employee());
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            int index = DataGridEmployees.SelectedIndex;
            if (index >= 0 && index < employees.Count)
                employees.RemoveAt(index);
        }

        ObservableCollection<Employee> employees = new ObservableCollection<Employee>()
        {
            new Employee(){name = "Salavat", id = 0, age = 17, salary = 5},
            new Employee(){name = "Danila", id = 1, age = 22, salary = 90},
            new Employee(){name = "Danil", id = 2, age = 25, salary = 50},
            new Employee(){name = "Danilel", id = 3, age = 50, salary = 5000000000000000000}
        };

        class Employee
        {
            public int id { get; set; }
            public string name { get; set; }
            public decimal salary { get; set; }
            public int age { get; set; }
        }

        private void DataGridEmployees_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            var textBox = e.EditingElement as TextBox;
            var column = e.Column.Header.ToString();

            if (column == "age")
            {
                if (int.TryParse(textBox.Text, out int value))
                {
                    if (value <= 0)
                    {
                        MessageBox.Show("negative age");
                        e.Cancel = true;
                    }
                }
                else
                {
                    MessageBox.Show("age is not number");
                    e.Cancel = true;
                }
            }
            else if (column == "name" && string.IsNullOrEmpty(textBox.Text))
            {
                MessageBox.Show("text empty");
                e.Cancel = true;
            }
        }

    }
}
