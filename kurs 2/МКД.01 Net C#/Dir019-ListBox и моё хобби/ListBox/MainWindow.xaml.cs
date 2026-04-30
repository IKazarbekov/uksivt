using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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
using System.Timers;
using System.IO;

namespace ListBox
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<Product> products = new ObservableCollection<Product>()
        {
            new Product(){Name="Nano ATmega328P | 16 МГц | 2KB RAM, 32KB Flash | 14 (6 PWM) | 8 | ~1 800 ₽", PathImage="/images/nano.jpeg"},
            new Product(){Name="Uno АТмега328П | 16 МБ | 2 КБ ОЗУ, 32 КБ флэш-памяти | 14 (6 ШИМ) | 6 | ~2000 ₽ ", PathImage="/images/uno.jpg"},
            new Product(){Name="Mega ATmega2560 | 16 МГц | 8KB RAM, 256KB Flash | 54 (15 PWM) | 16 | ~3 500 ₽", PathImage="/images/mega.jpeg"},
            new Product(){Name="Mini АТмега328П | 8/16 МБ | 2 КБ ОЗУ, 32 КБ флэш-памяти | 14 (6 ШИМ) | 6 | ~900 ₽ ", PathImage="/images/mini.jpeg"}
        };

        public MainWindow()
        {
            InitializeComponent();
            listBox.ItemsSource = products;
            Timer timer = new Timer(20000);
            string[] fon_images = Directory.GetFiles("images/fons/");
            int index_fon_image = 0;
            timer.Elapsed += (o, e) => {
                Dispatcher.Invoke(() => {
                    if (index_fon_image < fon_images.Length)
                    {
                        imageBrush.ImageSource = new BitmapImage(new Uri(fon_images[index_fon_image++], UriKind.Relative));
                    }
                    else
                    {
                        index_fon_image = 0;
                    }
                });
            };
            timer.Start();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(textBox.Text))
            {
               MessageBoxResult msResult =  MessageBox.Show("Вы не ввели описание электроники, вы хотите продолжить ?", "Предупреждение", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (msResult == MessageBoxResult.No)
                    return;
            }

            OpenFileDialog dialog = new OpenFileDialog();
            dialog.DefaultExt = ".png";
            dialog.Filter = "PNG|*.png|JPEG|*.jpeg";
            bool? result = dialog.ShowDialog();
            products.Add(new Product() { Name=textBox.Text, PathImage=result == true ? dialog.FileName : "/images/icon.png"});
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            products.Remove((Product)listBox.SelectedItem);
        }
    }



    class Product
    {
        public string Name {  get; set; }
        public string PathImage { get; set;}
    }
}
