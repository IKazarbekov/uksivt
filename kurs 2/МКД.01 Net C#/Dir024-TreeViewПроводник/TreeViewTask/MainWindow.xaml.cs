using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace TreeViewTask
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        class TypeAnimal
        {
            public string Name { get; set; }
            public BitmapImage ImageOne { get; set; }
            public BitmapImage ImageTwo { get; set; }
        }

        List<TypeAnimal> dogs;
        List<TypeAnimal> cats;
        List<TypeAnimal> birds;

        public MainWindow()
        {
            InitializeComponent();

            
        }

        void templateImage()
        {
            var stackPanel = new FrameworkElementFactory(
                typeof(StackPanel));
            stackPanel.SetValue(StackPanel.OrientationProperty, Orientation.Vertical);
            var image1 = new FrameworkElementFactory(typeof(Image));
            var image2 = new FrameworkElementFactory(typeof(Image));
            image1.SetBinding(Image.SourceProperty, new Binding("ImageOne"));
            image2.SetBinding(Image.SourceProperty, new Binding("ImageTwo"));
            stackPanel.AppendChild(image1);
            stackPanel.AppendChild(image2);
            DataTemplate template = new DataTemplate();
            template.DataType = typeof(TypeAnimal);
            template.VisualTree = stackPanel;
            listView.ItemTemplate = template;
        }
        void templateWord()
        {
            var textBox = new FrameworkElementFactory(typeof(
                TextBlock
            ));
            textBox.SetBinding(TextBlock.TextProperty, new Binding("Name"));
            var template = new DataTemplate();
            template.DataType = typeof(TypeAnimal);
            template.VisualTree = textBox;
            listView.ItemTemplate = template;
        }

        private void tviCats_Selected(object sender, RoutedEventArgs e)
        {
            templateWord();
            listView.ItemsSource = cats;

        }

        private void tviDogs_Selected(object sender, RoutedEventArgs e)
        {
            templateWord();
            listView.ItemsSource = dogs;
        }

        private void tviBirds_Selected(object sender, RoutedEventArgs e)
        {
            templateWord();
            listView.ItemsSource = birds;
        }

        private void Window_Loaded(object senderr, RoutedEventArgs er)
        {
            BitmapImage Image(string name) => (BitmapImage)Resources[name];

            dogs = new List<TypeAnimal>()
            {
                new TypeAnimal()
                {
                    Name="Австралийская овчарка",
                    ImageOne=Image("dog1_1"),
                    ImageTwo=Image("dog1_2")
                },
                new TypeAnimal()
                {
                    Name="Английский мастиф",
                    ImageOne=Image("dog2_1"),
                    ImageTwo=Image("dog2_2")
                },
                new TypeAnimal()
                {
                    Name="Дратхаар",
                    ImageOne=Image("dog3_1"),
                    ImageTwo=Image("dog3_2")
                },
            };

            cats = new List<TypeAnimal>()
            {
                new TypeAnimal()
                {
                    Name="Корат",
                    ImageOne=Image("cat1_1"),
                    ImageTwo=Image("cat1_2")
                },
                new TypeAnimal()
                {
                    Name="Бурмилла",
                    ImageOne=Image("cat2_1"),
                    ImageTwo=Image("cat2_2")
                },
                new TypeAnimal()
                {
                    Name="Дрeдуаар",
                    ImageOne=Image("cat3_1"),
                    ImageTwo=Image("cat3_2")
                },
            };

            birds = new List<TypeAnimal>()
            {
                new TypeAnimal()
                {
                    Name="Волнистые попугаи",
                    ImageOne=Image("bir1_1"),
                    ImageTwo=Image("bir1_2")
                },
                new TypeAnimal()
                {
                    Name="Какаду",
                    ImageOne=Image("bir2_1"),
                    ImageTwo=Image("bir2_2")
                },
                new TypeAnimal()
                {
                    Name="Благородный попугай",
                    ImageOne=Image("bir3_1"),
                    ImageTwo=Image("bir3_2")
                },
            };

            foreach (TypeAnimal animal in dogs)
            {
                void update(object sender, RoutedEventArgs e)
                {
                    templateImage();
                    e.Handled = true;
                    listView.ItemsSource = new List<TypeAnimal>()
                    {
                        animal
                    };
                }

                var item = new TreeViewItem();
                item.Header = animal.Name;
                item.Selected += update;
                tviDogs.Items.Add(item);
            }

            foreach (TypeAnimal animal in cats)
            {
                void update(object sender, RoutedEventArgs e)
                {
                    templateImage();
                    e.Handled = true;
                    listView.ItemsSource = new List<TypeAnimal>()
                    {
                        animal
                    };
                }
                var item = new TreeViewItem();
                item.Header = animal.Name;
                item.Selected += update;
                tviCats.Items.Add(item);
            }

            foreach (TypeAnimal animal in birds)
            {
                void update(object sender, RoutedEventArgs e)
                {
                    templateImage();
                    e.Handled = true;
                    listView.ItemsSource = new List<TypeAnimal>()
                    {
                        animal
                    };
                }
                var item = new TreeViewItem();
                item.Header = animal.Name;
                item.Selected += update;
                tviBirds.Items.Add(item);
            }
        }
    }
}
