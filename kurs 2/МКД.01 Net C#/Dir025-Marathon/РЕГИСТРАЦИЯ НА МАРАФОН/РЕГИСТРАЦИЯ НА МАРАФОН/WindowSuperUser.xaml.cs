using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace РЕГИСТРАЦИЯ_НА_МАРАФОН
{
    /// <summary>
    /// Логика взаимодействия для WindowSuperUser.xaml
    /// </summary>
    public partial class WindowSuperUser : Window
    {
        public WindowSuperUser()
        {
            InitializeComponent();
            dataGrid.ItemsSource = Data.Runners;
            view = CollectionViewSource.GetDefaultView(dataGrid.ItemsSource);
            view2 = CollectionViewSource.GetDefaultView(dataGrid.ItemsSource);

            textBlockCount.Text = "Всего пользователей: " + dataGrid.Items.Count;
            
        }
        ICollectionView view;
        ICollectionView view2;

        private void RoundButtonSort_Click(object sender, RoutedEventArgs e)
        {
            view.SortDescriptions.Clear();

            if (comboBoxSort.SelectedItem != null)
                view.SortDescriptions.Add(new SortDescription((string)comboBoxSort.SelectedValue, ListSortDirection.Ascending));

            if (comboBoxRole.SelectedItem != null)
            {
                string role = comboBoxRole.SelectedValue as string;
                view.Filter = item => ((Runner)item).Role == role;
            }

            if (textBoxFilter.Text.Length > 0) {
                view2.Filter = item => {

                    var runner = item as Runner;
                    string filterText = textBoxFilter.Text;

                    if (runner.Email.Contains(filterText) ||
                        runner.FirstName.Contains(filterText) ||
                        runner.LastName.Contains(filterText))
                        return true;

                    return false;
                };
            }

            view.Refresh();
            view2.Refresh();
            textBlockCount.Text = "Всего пользователей: " + dataGrid.Items.Count;
        }

        private void TopPanel_Click(object sender, RoutedEventArgs e)
        {
            this.ToStart();
        }

        private void RoundButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            new WindowRegistration(true).ShowDialog();
        }

        private void RoundButtonEdit_Click(object sender, RoutedEventArgs e)
        {
            new WindowRegistration((Runner)dataGrid.SelectedItem).ShowDialog();
        }

        private void TopPanel_ClickLogout(object sender, RoutedEventArgs e)
        {
            this.ToStart();
            Close();
        }
    }
}
