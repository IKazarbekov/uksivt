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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Попытка_14
{
    /// <summary>
    /// Логика взаимодействия для UserControlBrone.xaml
    /// </summary>
    public partial class UserControlBrone : UserControl
    {
        public UserControlBrone(Brone brone)
        {
            InitializeComponent();

            roomName.Text = brone.RoomName;
            dateBrone.Text = brone.Date.ToShortDateString();
            accept.Text = brone.Status ? "Подтвержденно" : "Отказано";
        }
    }
}
