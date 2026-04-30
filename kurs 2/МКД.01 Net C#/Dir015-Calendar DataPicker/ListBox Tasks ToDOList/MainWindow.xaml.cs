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

namespace ListBox_Tasks_ToDOList
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        class Task
        {
            public string Name {  get; set; }
            public string Description {  get; set; }
            public int Days { get; set; }

        }
        ObservableCollection<Task> tasks = new ObservableCollection<Task>();
        public MainWindow()
        {
            InitializeComponent();
            listBox.ItemsSource = tasks;
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {

            TextRange textRange = new TextRange(
                textBoxDescription.Document.ContentStart,
                textBoxDescription.Document.ContentEnd
                );

            var result = MessageBox.Show($"Вы уверены, что хотите добавить задачу {textBoxName}, с описанием {textRange.Text} за кол-во дней {slider.Value}", "Предупреждение", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.No) {
                return;
            }


            Task task = new Task(){Name = textBoxName.Text, Description = textRange.Text, Days = (int)(slider.Value)};
            tasks.Add(task);
        }

        private void ButtonRemove_Click(object sender, RoutedEventArgs e)
        {
            tasks.Remove((Task)listBox.SelectedItem);
        }

        private void ButtonInfo_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Программа для задач, очень красивая но очень бесполезная");
        }

        private void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            textBlockDays.Text = $"за {(int)slider.Value} дней";
        }
    }
}
