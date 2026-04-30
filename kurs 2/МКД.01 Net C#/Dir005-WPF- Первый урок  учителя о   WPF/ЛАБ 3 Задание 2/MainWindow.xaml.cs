using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ЛАБ_3_Задание_2
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

        private void ChangeTool(object sender, RoutedEventArgs e)
        {
            if (RadioButtonInk.IsChecked == true)
            {
                InkCanvasMain.EditingMode = InkCanvasEditingMode.Ink;
            }
            else if (RadioButtonSelect.IsChecked == true)
            {
                InkCanvasMain.EditingMode = InkCanvasEditingMode.Select;
            }
            else if (RadioButtonErase.IsChecked == true)
            {
                InkCanvasMain.EditingMode = InkCanvasEditingMode.EraseByStroke;
            }
        }

        private void ChangeColor(object sender, RoutedEventArgs e)
        {
            switch (ComboBoxColor.SelectedIndex)
            {
                case 0: 
                    DrawingAttr.Color = Brushes.Red.Color;
                    break;
                case 1:
                    DrawingAttr.Color = Brushes.Green.Color;
                    break;
                case 2:
                    DrawingAttr.Color = Brushes.Blue.Color;
                    break;
                case 3:
                    DrawingAttr.Color = Brushes.Gray.Color;
                    break;
                case 4:
                    DrawingAttr.Color = Brushes.White.Color;
                    break;
                case 5:
                    DrawingAttr.Color = Brushes.Black.Color;
                    break;
                case 6:
                    DrawingAttr.Color = Brushes.Yellow.Color;
                    break;
                case 7:
                    DrawingAttr.Color = Brushes.Orange.Color;
                    break;
            }
        }

        private void ChangeSize(object sender, RoutedEventArgs e)
        {
            double size = SliderSize.Value;
            DrawingAttr.Height = size;
            DrawingAttr.Width = size;
        }
    }
}
