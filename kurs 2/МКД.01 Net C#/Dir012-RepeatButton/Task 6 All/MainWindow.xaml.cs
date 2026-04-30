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

namespace Task_6_All
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string name = textBoxName.Text;
            bool isMen = radioButtonPol.IsChecked.Value;
            string pol = isMen ? "Мужчина" : "Женщина";
            int age = (int)(sliderAge.Value);
            string country = comboBoxCountry.Text;
            string hobby = "спорт";
            if (checkBoxMusic.IsChecked.Value)
                hobby = "музыку";
            else if (checkBoxReading.IsChecked.Value)
                hobby = "чтение";
            else if (checkBoxMovie.IsChecked.Value)
                hobby = "фильмы";
            string result = "";
            if (string.IsNullOrEmpty(name))
                result += "НЕТ ИМЕНИ! ";
            result += $"{pol} {name} любит {hobby}, живёт в {country} и ему(ей) {age} лет.";
            textBlock.Text = result;
        }

        private void sliderAge_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            textBlockAge.Text = $"Возраст {sliderAge.Value:.0F} лет";
        }
    }
}
