using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace TaskWindow
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static int index = 0;
        class Arduino
        {
            public string Name { get; set; }
            public BitmapImage Image { get; set; }
        }
        List<Arduino> arduinos = new List<Arduino>()
        {
            new Arduino()
            {
                Name="Nano"
            },
            new Arduino()
            {
                Name="Uno"
            },
             new Arduino()
            {
                Name="Mega"
            },
            new Arduino()
            {
                Name="Mini"
            }
        };
        public MainWindow()
        {
            InitializeComponent();
            arduinos[0].Image = (BitmapImage)this.Resources["nano"];
            arduinos[1].Image = (BitmapImage)this.Resources["uno"];
            arduinos[2].Image = (BitmapImage)this.Resources["mega"];
            arduinos[3].Image = (BitmapImage)this.Resources["mini"];
            
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        public void ChangeImage()
        {
            if (index == 4)
                index = 0;
            else
                index++;
            image.Source = arduinos[index].Image;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (textBox.Text == arduinos[index].Name) {
                var window = new WindowWin();
                window.Owner = this;
                window.ShowDialog();
            }
            else
            {
                var window = new WindowNoWin();
                ((WindowNoWin)window).textBox.Text = $"нЕ ПРАВИЛЬНО {textBox.Text}";
                window.ShowDialog();
            }
        }
    }
}
