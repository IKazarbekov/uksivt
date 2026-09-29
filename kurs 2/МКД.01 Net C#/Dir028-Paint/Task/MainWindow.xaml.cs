using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Task
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string currentPathImage = null;
        string currentPathStrokes = null;
        public MainWindow()
        {
            InitializeComponent();
            Pen_Click(this, new RoutedEventArgs());
        }

        private void Pen_Click(object sender, RoutedEventArgs e)
        {
            inkCanvas.EditingMode = InkCanvasEditingMode.Ink;
            inkCanvas.DefaultDrawingAttributes = new DrawingAttributes
            {
                Width = 5,
                Height = 5,
            };
        }

        private void Marker_Click(object sender, RoutedEventArgs e)
        {
            inkCanvas.EditingMode = InkCanvasEditingMode.Ink;
            inkCanvas.DefaultDrawingAttributes = new DrawingAttributes
            {
                Width = 15,
                Height = 15,
                IsHighlighter = true
            };
        }

        private void RemovePixel_Click(object sender, RoutedEventArgs e)
        {
            inkCanvas.EditingMode = InkCanvasEditingMode.EraseByPoint;
        }

        private void RemoveElement_Click(object sender, RoutedEventArgs e)
        {
            inkCanvas.EditingMode = InkCanvasEditingMode.EraseByStroke;
        }

        private void Select_Click(object sender, RoutedEventArgs e)
        {
            inkCanvas.EditingMode = InkCanvasEditingMode.Select;
        }

        private void None_Click(object sender, RoutedEventArgs e)
        {
            inkCanvas.EditingMode = InkCanvasEditingMode.None;
        }

        private void Fill_Click(object sender, RoutedEventArgs e)
        {
            inkCanvas.Background = new SolidColorBrush( inkCanvas.DefaultDrawingAttributes.Color);
        }

        private void SaveImage(object sender, RoutedEventArgs e)
        {
            int width = (int)inkCanvas.ActualWidth;
            int height = (int)inkCanvas.ActualHeight;

            RenderTargetBitmap rtb = new RenderTargetBitmap(
                width, height, 96, 96, PixelFormats.Pbgra32);

            DrawingVisual dv = new DrawingVisual();

            using (DrawingContext dc = dv.RenderOpen())
            {
                VisualBrush visualBrush = new VisualBrush(inkCanvas);
                dc.DrawRectangle(visualBrush, null, new Rect(0, 0, width, height));
            }

            rtb.Render(dv);

            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(rtb));

            string path = null;
            if (currentPathImage != null)
                path = currentPathImage;
            else
            {
                var dialog = new SaveFileDialog() { Filter = "images|*.png|all|*.*" };
                dialog.ShowDialog();
                path = dialog.FileName;
                if (path == null) return;
                currentPathImage = path;
                Title = $"Paint - {System.IO.Path.GetFileName(path)}";
            }
            try
            {

            using (FileStream fs = new FileStream(path, FileMode.Create))
                encoder.Save(fs);
            }
            catch
            {
                MessageBox.Show("Не выбран путь");
            }
        }

        private void SaveImageAs(object sender, RoutedEventArgs e)
        {
            int width = (int)inkCanvas.ActualWidth;
            int height = (int)inkCanvas.ActualHeight;

            RenderTargetBitmap rtb = new RenderTargetBitmap(
                width, height, 96, 96, PixelFormats.Pbgra32);

            DrawingVisual dv = new DrawingVisual();

            using (DrawingContext dc = dv.RenderOpen())
            {
                VisualBrush visualBrush = new VisualBrush(inkCanvas);
                dc.DrawRectangle(visualBrush, null, new Rect(0, 0, width, height));
            }

            rtb.Render(dv);

            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(rtb));

            var dialog = new SaveFileDialog() { Filter = "images|*.png|all|*.*" };
            dialog.ShowDialog();
            var path = dialog.FileName;
            if (path == null) return;
            currentPathImage = path;
            Title = $"Paint - {System.IO.Path.GetFileName(path)}";

            using (FileStream fs = new FileStream(path, FileMode.Create))
                encoder.Save(fs);
        }

        private void CreateImage(object sender, RoutedEventArgs e)
        {
            currentPathImage = null;
            currentPathStrokes = null;
            inkCanvas.Strokes.Clear();
            Title = $"Paint";
        }


        private void ChangeColor(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            inkCanvas.DefaultDrawingAttributes.Color = ((SolidColorBrush)button.Background).Color;
        }

        private void Fill_Picther(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog() { Filter = "images|*.png|all|*.*" };
            dialog.ShowDialog();
            var path = dialog.FileName;
            if (!File.Exists(path))
                return;
            inkCanvas.Background = new ImageBrush() { ImageSource = new BitmapImage(new Uri(path)) };
        }

        private void AddCub(object sender, RoutedEventArgs e)
        {
            StylusPointCollection points = new StylusPointCollection();
            points.Add(new StylusPoint(50,  50));
            points.Add(new StylusPoint(150,  50));
            points.Add(new StylusPoint(150,  100));
            points.Add(new StylusPoint(50,  100));
            points.Add(new StylusPoint(50, 50));

            Stroke rectangleStroke = new Stroke(points)
            {
                DrawingAttributes = new DrawingAttributes
                {
                    Color = inkCanvas.DefaultDrawingAttributes.Color,
                    Width = 3,
                    Height = 3,
                    StylusTip = StylusTip.Ellipse
                }
            };

            inkCanvas.Strokes.Add(rectangleStroke);
            inkCanvas.Select(new StrokeCollection() { rectangleStroke });
        }



        private void ClickDialogColor(object sender, RoutedEventArgs e)
        {
            var dialog = new ColorDialog();
            dialog.ShowDialog();
            var color = dialog.GetColor();
            inkCanvas.DefaultDrawingAttributes.Color = color;
        }

        private void ButtonAbout_Click(object sender, RoutedEventArgs e)
        {
            new WindowAbout().ShowDialog();
        }

        private void OpenStrokes(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog() { Filter = "strokes|*.isf|all|*.*" };
            dialog.ShowDialog();
            var path = dialog.FileName;
            if (!File.Exists(path))
                return;
            try
            {
                using (Stream fs = new FileStream(path, FileMode.Open))
                    inkCanvas.Strokes = new StrokeCollection(fs);
            }
            catch
            {
                MessageBox.Show("Файл повреждён");
            }
        }

        private void SaveStrokes(object sender, RoutedEventArgs e)
        {
            string path = null;
            if (currentPathStrokes == null)
            {
                var dialog = new SaveFileDialog() { Filter = "strokes|*.isf|all|*.*" };
                dialog.ShowDialog();
                path = dialog.FileName;
                if (!File.Exists(path))
                    return;
                currentPathStrokes = path;
            }
            else
                path = currentPathStrokes;
            using (Stream fs = new FileStream(path, FileMode.Create))
                inkCanvas.Strokes.Save(fs);
        }

        private void AddImage(object sender, RoutedEventArgs e)
        {
            // 1. Загружаем картинку
            var dialog = new OpenFileDialog() { Filter = "strokes|*.png|all|*.*" };
            dialog.ShowDialog();
            var path = dialog.FileName;
            if (!File.Exists(path))
                return;
            BitmapImage bitmap = new BitmapImage(new Uri(path));

            // 2. Создаём элемент Image
            Image image = new Image
            {
                Source = bitmap,
                Width = 120,
                Height = 100,
                Stretch = Stretch.Uniform
            };

            // 3. Позиционируем в центре InkCanvas
            double left = (inkCanvas.ActualWidth - image.Width) / 2;
            double top = (inkCanvas.ActualHeight - image.Height) / 2;

            // 4. Размещаем картинку
            InkCanvas.SetLeft(image, left);
            InkCanvas.SetTop(image, top);

            // 5. Добавляем на InkCanvas
            inkCanvas.Children.Add(image);
        }
    }
}
