using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.TextFormatting;

namespace task1_DataGrid
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        const string FILE_NAME = "file.bin";

        ObservableCollection<Employee> employees = new ObservableCollection<Employee>();

        public MainWindow()
        {
            InitializeComponent();

            try
            {
                using (FileStream fileStream = new FileStream(FILE_NAME, FileMode.Open))
                using (BinaryReader reader = new BinaryReader(fileStream))
                {
                    int countEmployees = reader.ReadInt32();
                    MessageBox.Show("" + countEmployees);
                    for (int i = 0; i < countEmployees; i++)
                    {
                        int id = reader.ReadInt32();
                        string name = reader.ReadString();
                        int age = reader.ReadInt32();
                        int salary = reader.ReadInt32();

                        employees.Add(new Employee() { id = id, name = name, age = age, salary = salary });
                    }
                }
            }
            catch(FileNotFoundException e)
            {

            }
            catch (Exception e){
                MessageBox.Show(e.Message);
            }


            DataGridEmployees.ItemsSource = employees;
        }

        private void Button_Click_Add(object sender, RoutedEventArgs e)
        {
            employees.Add(new Employee());
        }

        private void Button_Click_Remove(object sender, RoutedEventArgs e)
        {
            int index = DataGridEmployees.SelectedIndex;
            if (index != -1 && index < employees.Count)
                employees.RemoveAt(index);
        }

        private void DataGridEmployees_CellEditEnding(object sender, System.Windows.Controls.DataGridCellEditEndingEventArgs e)
        {

            TextBox messageBox = e.EditingElement as TextBox;
            string text = messageBox.Text;
            string type = e.Column.Header.ToString();

            if (type == "age")
            {
                if (int.TryParse(text, out int value))
                {
                    if (value <= 0)
                    {
                        e.Cancel = true;
                        MessageBox.Show("negative age");
                        return;
                    }
                }
                else
                {
                    e.Cancel = true;
                    MessageBox.Show("age is not number");
                    return;
                }
            }
            else if (type == "name")
            {
                if (string.IsNullOrEmpty(text))
                {
                    e.Cancel = true;
                    MessageBox.Show("name if empty");
                    return;
                }
            }
        }

        private void Window_Closed(object sender, System.EventArgs e)
        {
            using (FileStream fileStream = new FileStream(FILE_NAME, FileMode.Create))
            using (BinaryWriter writer = new BinaryWriter(fileStream))
            {
                writer.Write(employees.Count);
                foreach (Employee employee in employees)
                {
                    if (employee.name != null)
                    {
                        writer.Write(employee.id);
                        writer.Write(employee.name);
                        writer.Write(employee.age);
                        writer.Write(employee.salary);
                    }
                }
            }
        }


        public class Employee
        {
            public int id { get; set; }
            public string name { get; set; }
            public int age { get; set; }
            public decimal salary { get; set; }
        }
    }
}
