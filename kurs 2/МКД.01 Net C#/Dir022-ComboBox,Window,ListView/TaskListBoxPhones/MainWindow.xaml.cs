using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace TaskListBoxPhones
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            listBox.ItemsSource = phoneList;
        }

        public class Phone
        {
            public string Model { get; set; }
            public int Price { get; set;  }
            public string PriceStr { get { 
                return "" + Price + "Рублей";
                }
            }
            public string Manufacturer { get; set; }
            public string ImageFileName { get; set; }
            public BitmapImage Image { get
                {
                    return new BitmapImage(new Uri(ImageFileName, UriKind.Absolute));
                }
            }
        }

        public ObservableCollection<Phone> phoneList = new ObservableCollection<Phone>
        {
            new Phone{ Model="K15n", Price=1500, Manufacturer="Имеет в наличии браузер, мощный фонарик и невероятную ёмкость аккумулятора, тонно полезных программ, онлайн игры, переводчик, телеграмм",
            ImageFileName="Y:\\Dir022-ComboBox,Window,ListView\\K15n.png"},
            new Phone{ Model="86 ds up", Price=1200, Manufacturer="Имеет большие кнопки, поставку, уникальное зарядное устройство, голосовое управление, тонно полезных программ, онлайн игры, переводчик, телеграмм",
            ImageFileName="Y:\\Dir022-ComboBox,Window,ListView\\bigbuttonphone.jpg"},
            new Phone{ Model="Android", Price=99999, Manufacturer="Лагает, отнимает время, быстро разряжается а через год умирает, ТОнна лишних программ",
            ImageFileName="Y:\\Dir022-ComboBox,Window,ListView\\android.jpg"},
        };

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var window = new WindowAdd();
            window.Owner = this;
            window.ShowDialog();
        }
    }
}