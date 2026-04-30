using System.Windows;
using System.Windows.Media;

namespace ЛАБ_3_задание_1
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

        private void ChangeColor(object sender, RoutedEventArgs e)
        {
            if (RadioButtonColorBlack.IsChecked == true)
                Background = Brushes.Black;
            else if (RadioButtonColorWhite.IsChecked == true)
                Background = Brushes.White;
            else if (RadioButtonColorRed.IsChecked == true)
                Background = Brushes.Red;
            else if (RadioButtonColorGreen.IsChecked == true)
                Background = Brushes.Green;
            else if (RadioButtonColorYellow.IsChecked == true)
                Background = Brushes.Yellow;
        }

        private void About(object sender, RoutedEventArgs e)    => MessageBox.Show("Разработчик: Казарбеков Ильяс");

        private void Exit(object sender, RoutedEventArgs e) => Close();

        private void MouseMoveChangeColor(object sender, RoutedEventArgs e)
        {
            TextAboutNameItem.Text = "Редактировать цвет";
            TextAboutItem.Text = "Описание: Изменить фоновый цвет окна";
        }

        private void MouseMoveAbout(object sender, RoutedEventArgs e)
        {
            TextAboutNameItem.Text = "О разработчике";
            TextAboutItem.Text = "Описание: Покажет разработчика";
        }

        private void MouseMoveExit(object sender, RoutedEventArgs e)
        {
            TextAboutNameItem.Text = "Закрыть окно";
            TextAboutItem.Text = "Описание: Закроет окно";
        }

        private void ChangeColorOnRed(object sender, RoutedEventArgs e) => Background = Brushes.Red;
        private void ChangeColorOnGreen(object sender, RoutedEventArgs e) => Background = Brushes.Green;
        private void ChangeColorOnBlack(object sender, RoutedEventArgs e) => Background = Brushes.Black;  
        private void ChangeColorOnWhite(object sender, RoutedEventArgs e) => Background = Brushes.White;
        private void ChangeColorOnYellow(object sender, RoutedEventArgs e) => Background = Brushes.Yellow;

    }
}
