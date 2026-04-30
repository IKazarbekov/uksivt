using Python.Runtime;
using System.Windows;

namespace Calculater2
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //Runtime.PythonDLL = "C:\\Users\\Казарбеков\\AppData\\Local\\Programs\\Python\\Python313\\python3.dll";
            ////PythonEngine.Initialize();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Py.Import("parsing");
        }
    }
}
