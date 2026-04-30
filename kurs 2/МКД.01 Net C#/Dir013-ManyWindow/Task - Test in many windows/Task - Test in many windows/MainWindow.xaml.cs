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

namespace Task___Test_in_many_windows
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

        private void Button_Begin_Test(object sender, RoutedEventArgs e)
        {
            Data.currentName = textBoxName.Text;
            if (string.IsNullOrEmpty(Data.currentName))
            {
                MessageBox.Show("Введите имя");
                return;
            }
            if (Data.statistic.ContainsKey(Data.currentName))
            {
                MessageBox.Show("Имя было использовано ранее");
                return;
            }
            Data.questionNumber = 0;
            Data.currentQuestionNumber = 0;
            Data.testsClose.Clear();
            Data.testClosesUser.Clear();
            Utills.NextWindow();
            Close();
        }
    }
}
