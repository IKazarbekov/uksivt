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

namespace task1_Employees
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public class Employee 
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Position { get; set; }
            public decimal Salary { get; set; }
        }

        public ObservableCollection<Employee> employees = new ObservableCollection<Employee> {
                new Employee(){Id = 0, Name="Tom", Position="Developer", Salary=10},
                new Employee(){Id = 1, Name="Bob", Position="Manager", Salary=10000},
                new Employee(){Id = 2, Name="Kik", Position="Director", Salary=12}
                };

        public MainWindow()
        {
            InitializeComponent();
            EmployeeDataGrid.ItemsSource = employees;
        }

        private void Button_Click_Add_Employee(object sender, RoutedEventArgs e)
        {
            employees.Add(new Employee());
        }

        private void Button_Click_Delete_Employee(object sender, RoutedEventArgs e)
        {
            int index = EmployeeDataGrid.SelectedIndex;
            employees.RemoveAt(index);
        }

        private void EmployeeDataGrid_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            var employee = e.Row.Item as Employee;
            if (employee == null)
                return;
            MessageBox.Show(employee.Name, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);

            switch (e.Column.Header.ToString())
            {
                case "Name":
                    if (string.IsNullOrEmpty(employee.Name))
                    {
                        e.Cancel = true;
                        MessageBox.Show("Введите поле имени", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    break;
            }
        }
    }
}
