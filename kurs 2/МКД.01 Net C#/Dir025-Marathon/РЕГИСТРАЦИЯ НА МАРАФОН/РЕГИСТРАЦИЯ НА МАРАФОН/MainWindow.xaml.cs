using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace РЕГИСТРАЦИЯ_НА_МАРАФОН
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            if (File.Exists("json_file.json"))
            {
                string json = File.ReadAllText("json_file.json");
                Data.Runners = JsonSerializer.Deserialize<ObservableCollection<Runner>>(json);
            }
        }

        private void RoundButtonRunner_Click(object sender, RoutedEventArgs e)
        {
            Window window = new WindowQuestionIsRunner();
            window.Show();
            Close();
        }

        private void RoundButtonLogin_Click(object sender, RoutedEventArgs e)
        {
            this.ToLogin();
        }

        private void RoundButtonStop_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Ожидайте в будущем эту функцию");
        }

        private void RoundButton_Click(object sender, RoutedEventArgs e)
        {
            new WindowSuperUser().Show();
            Close();
        }

        private void RoundButtonClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}