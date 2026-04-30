using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Timers;

namespace FifeNightAtUKSIVT
{
    /// <summary>
    /// Логика взаимодействия для Game_House2_v1.xaml
    /// </summary>
    public partial class Game_House2_v1 : Window
    {

        enum Place
        {
            AtNorebook,
            AtDoor,
            CloseDoor,
            OpenDoorLeft,
            OpenDoorRight,
            InCamera
        }

        Place place = Place.AtNorebook;

        public Game_House2_v1()
        {
            InitializeComponent();
            /*
            void Runner(object sender, ElapsedEventArgs e)
            {
                DataChanger.RunerMainInTimer();
                MessageBox.Show(Data.StageIlyas + "");
            }

            Timer timer = new Timer(5000);
            timer.Elapsed += Runner;
            timer.Start();*/
        }

        private void ButtonGoAtDoor_Click(object sender, RoutedEventArgs e)
        {
            place = Place.AtDoor;
            new AtDoorGameHouse2_v1().Show();
        }

        private void ButtonGoInNotebook_Click(object sender, RoutedEventArgs e)
        {
            new WindowCameras().Show();
            Close();
        }
    }
}
