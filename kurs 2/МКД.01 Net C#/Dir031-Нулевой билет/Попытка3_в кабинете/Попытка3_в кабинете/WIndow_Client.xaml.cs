using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Попытка3_в_кабинете.models;

namespace Попытка3_в_кабинете
{
    /// <summary>
    /// Логика взаимодействия для WIndow_Client.xaml
    /// </summary>
    public partial class WIndow_Client : Window
    {
        public WIndow_Client(int userID)
        {
            InitializeComponent();

            CollectionView view = new CollectionView(Data.Brones);
            view.Filter += br => ((Brone)br).UserID == userID;
            DataContext = view;
        }
    }
}
