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
    /// Логика взаимодействия для WindowCameras.xaml
    /// </summary>
    public partial class WindowCameras : Window
    {
        public WindowCameras()
        {
            InitializeComponent();
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            new Game_House2_v1().Show();
            Close();
        }
    }
}
