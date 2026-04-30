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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace task_3__поторение_3
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<Employee> employees = new ObservableCollection<Employee>()
            {
                new Employee(){ id = 0, name="Tom", age=39, salary=20070},
                new Employee(){ id = 1, name="Bob", age=31, salary=20500},
                new Employee(){ id = 2, name="Dog", age=33, salary=23000},
                new Employee(){ id = 3, name="Tok", age=23, salary=20020},
            };
        class Employee
        {
            public int id { get; set; }
            public string name { get; set; }
            public int age { get; set; }
            public int salary { get; set; }
        }
        public MainWindow()
        {
            InitializeComponent();
            dataGridEmployee.ItemsSource = employees;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            employees.Add(new Employee());
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            int index = dataGridEmployee.SelectedIndex;
            if (index > 0 && index < employees.Count)
            {
                employees.RemoveAt(index);
            }
        }

        private void dataGridEmployee_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            var textBox = e.EditingElement as TextBox;
            var text = textBox.Text;
            string column = e.Column.Header.ToString();
            if (column == "age")
            {
                if (int.TryParse(textBox.Text, out int value))
                {
                    if (value <= 0)
                    {
                        e.Cancel = true;
                        MessageBox.Show("negative age");
                    }
                }
                else
                {
                    e.Cancel = true;
                    MessageBox.Show("age is not number");
                }
            }
            else if (column == "name")
            {
                if (string.IsNullOrEmpty(text)) {
                    e.Cancel = true;
                    MessageBox.Show("name is empty");
                }
            }
        }
    }
}
