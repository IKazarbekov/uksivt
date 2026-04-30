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
    /// Логика взаимодействия для AtPenDoorGameHouse2_v1.xaml
    /// </summary>
    public partial class AtPenDoorGameHouse2_v1 : Window
    {
        public AtPenDoorGameHouse2_v1()
        {
            InitializeComponent();
        }

        private void ButtonCloseDoor_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ButtonOpenDoor_Click(object sender, RoutedEventArgs e)
        {
            new LongRoomLeftGameHouse2_v1().Show();
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
