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
using System.Timers;
using System.Media;

namespace FifeNightAtUKSIVT
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
           

            
 /*
            MediaElement media = new MediaElement();
            media.Source = new Uri("musics/Пианино.mp3", UriKind.Relative);
            media.LoadedBehavior = MediaState.Play;
            media.LoadedBehavior = MediaState.Manual;
            media.Play();*/

            new Game_House2_v1().Show();
            Close();
        }
    }
}
