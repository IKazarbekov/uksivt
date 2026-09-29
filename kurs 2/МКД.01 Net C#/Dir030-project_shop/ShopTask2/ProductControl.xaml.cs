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
using ShopTask.models;

namespace ShopTask
{
    /// <summary>
    /// Логика взаимодействия для ProductControl.xaml
    /// </summary>
    public partial class ProductControl : UserControl
    {
        static List<ProductControl> list = new List<ProductControl>();
        Product product;
        public ProductControl(Product product_)
        {
            product = product_;
            InitializeComponent();
            DataContext = product;
            UpdateDataUI();
            list.Add(this);
        }

        public void UpdateDataUI()
        {
            if (product.InSeller == 0)
            {
                buttonAdd.Visibility = Visibility.Visible;
                gridSeller.Visibility = Visibility.Collapsed;
            }
            else
            {
                buttonAdd.Visibility = Visibility.Collapsed;
                gridSeller.Visibility = Visibility.Visible;
                textBlockCount.Text = product.InSeller + " шт";
            }

            buttonAdd.IsEnabled = product.Count > 0;
            buttonAdd2.IsEnabled = product.Count > 0;
            textBlockInfo2.Text = $"На складе: {product.Count};";
            //MessageBox.Show("" + product.Count + "---" + product.InSeller);
        }

        private void Button_Click_Add(object sender, RoutedEventArgs e)
        {
            product.InSeller++;
            product.Count--;
            UpdateDataUI();
        }

        private void Button_Click_Sub(object sender, RoutedEventArgs e)
        {
            product.InSeller--;
            product.Count++;
            UpdateDataUI();
        }

        public static void AllUpdateDataUI()
        {
            foreach (var control in list)
                control.UpdateDataUI();
        }
    }
}
