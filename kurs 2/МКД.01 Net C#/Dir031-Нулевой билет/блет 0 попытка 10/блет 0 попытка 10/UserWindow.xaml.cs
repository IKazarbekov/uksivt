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

namespace блет_0_попытка_10
{
    /// <summary>
    /// Логика взаимодействия для UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        public UserWindow()
        {
            InitializeComponent();
        }
        class Item
        {
            public Item(Brone brone)
            {
                Status = 
                Status = brone.Status;
            }
            public string Type{ get; set; }
            public string Trenner { get; set; }
            public DateTime DateLesson { get; set; }
            public DateTime DateBrone { get; set; }
            public string Status { get; set; }
        }
        void Update()
        {
            
        }
    }
}
