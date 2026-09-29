using ShopTask.data;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ShopTask
{
    /// <summary>
    /// Логика взаимодействия для ProductsWindow.xaml
    /// </summary>
    public partial class ProductsWindow : Window
    {
        public ProductsWindow()
        {
            InitializeComponent();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            InitData.Save();
        }

        private void Window_MouseDown(object sender, MouseEventArgs e)
        {
            this.DragMove();
        }

        private void TabControl_Selected(object sender, RoutedEventArgs e)
        {
            var tabControl = (TabControl)sender;

            MessageBox.Show((String)tabControl.SelectedValue);
            if ((String)tabControl.SelectedValue == "Товары")
            {

            }
        }
    }
}
