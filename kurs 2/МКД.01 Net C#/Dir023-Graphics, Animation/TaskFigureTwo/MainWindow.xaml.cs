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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TaskFigureTwo
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += StartTurtleAnimation;
        }

        private void StartTurtleAnimation(object sender, RoutedEventArgs e)
        {
            DoubleAnimation turtleMove = new DoubleAnimation();

            turtleMove.From = 0;              
            turtleMove.To = 300;              
            turtleMove.Duration = TimeSpan.FromSeconds(3); 

            turtleMove.AutoReverse = true;     

            turtleMove.EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseInOut };
            /*
            void end(EventHandler e)
            {
                MessageBox.Show("Анимация завершена");
            }
            turtleMove.Completed += end;*/
            turtleMove.Completed += delegate {
                MessageBox.Show("Анимация завершена");
            };

            TurtleTransform.BeginAnimation(TranslateTransform.XProperty, turtleMove);
        }
    }
}
