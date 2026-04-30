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
using Microsoft.Win32;

namespace ViewImages
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

        List<string> images = new List<string>();
        int index = 0;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            index = 0;
            var dialog = new OpenFileDialog();
            dialog.Multiselect = true;
            dialog.DefaultExt = "*.png";
            dialog.Filter = "Изображение|*.png|Все файлы|*";
            bool? result = dialog.ShowDialog();
            if (result == true)
            {
                {
                    images = new List<string>( dialog.FileNames);
                    textBlock.Text = $"Кол-во изображений: {images.Count}";
                    UpdateImage();
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            index++;
            if (index >= images.Count)
                index = 0;
            UpdateImage();
        }

        void UpdateImage()
        {
            if (images.Count > 0)
                image.ImageSource = new BitmapImage(new Uri(images[index], UriKind.Absolute));
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            index--;
            if (index < 0)
                index = images.Count - 1;
            UpdateImage();
        }
    }
}
