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
    /// Логика взаимодействия для LongRoomLeftGameHouse2_v1.xaml
    /// </summary>
    public partial class LongRoomLeftGameHouse2_v1 : Window
    {
        public LongRoomLeftGameHouse2_v1()
        {
            InitializeComponent();
        }

        private void ButtonBack_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ButtonDoLeft_Click(object sender, RoutedEventArgs e)
        {
            new LongRoomRightGameHouse2_v1().Show();
        }

        private void Button_KeyUp(object sender, KeyEventArgs e)
        {
            mainImageBrush.ImageSource = new BitmapImage(new Uri("images/LongDoorLeftBlack.jpg", UriKind.Relative));
        }

        private void Button_KeyDown(object sender, KeyEventArgs e)
        {
            mainImageBrush.ImageSource = new BitmapImage(new Uri("images/LongDoorLeft.jpg", UriKind.Relative));

        }
    }
}
