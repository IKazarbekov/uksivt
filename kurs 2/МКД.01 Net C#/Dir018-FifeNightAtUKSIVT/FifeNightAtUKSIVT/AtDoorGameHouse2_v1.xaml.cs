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
using System.Windows.Shapes;

namespace FifeNightAtUKSIVT
{
    /// <summary>
    /// Логика взаимодействия для AtDoorGameHouse2_v1.xaml
    /// </summary>
    public partial class AtDoorGameHouse2_v1 : Window
    {
        public AtDoorGameHouse2_v1()
        {
            InitializeComponent();
        }

        private void ButtonGoAtNotebook_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new AtPenDoorGameHouse2_v1().Show();
        }
    }
}
