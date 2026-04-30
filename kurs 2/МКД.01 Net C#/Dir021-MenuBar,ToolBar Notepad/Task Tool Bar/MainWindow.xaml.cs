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

namespace Task_Tool_Bar
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

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            ComboBoxItem item = comboBox.SelectedItem as ComboBoxItem;
            imageBrush.ImageSource = new BitmapImage(new Uri("images/" + item.Content + ".jpg", UriKind.Relative));
        }

        private void ComboBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            ComboBoxItem item = comboBox.SelectedItem as ComboBoxItem;
            switch (item.Content)
            {
                case "Red":
                    toolBArTray.Background = new SolidColorBrush(Colors.Red);
                    break;
                case "Black":
                    toolBArTray.Background = new SolidColorBrush(Colors.Black);
                    break;
                case "Blue":
                    toolBArTray.Background = new SolidColorBrush(Colors.Blue);
                    break;
            }
        }

        private void TextBoxWidth_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                this.Width = int.Parse(((TextBox)sender).Text);
            }
            catch { }
        }

        private void TextBoxHeight_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                this.Height = int.Parse(((TextBox)sender).Text);
            }
            catch { }
        }

        private void Button_ClickFill(object sender, RoutedEventArgs e)
        {
            imageBrush.Stretch = Stretch.Fill;
        }

        private void Button_ClickNo(object sender, RoutedEventArgs e)
        {
            imageBrush.Stretch = Stretch.None;
        }

        private void Button_ClickUniform(object sender, RoutedEventArgs e)
        {
            imageBrush.Stretch = Stretch.Uniform;
        }

        private void Button_ClickUniformToFill(object sender, RoutedEventArgs e)
        {
            imageBrush.Stretch = Stretch.UniformToFill;
        }
    }
}
