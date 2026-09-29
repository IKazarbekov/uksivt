using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Попытка_14
{
    /// <summary>
    /// Логика взаимодействия для AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        public AdminWindow()
        {
            InitializeComponent();

            users.ItemsSource = Data.Users;
            rooms.ItemsSource = Data.Rooms;
            cbRooms.ItemsSource = Data.Rooms;
            cbUsers.ItemsSource = Data.Users;
            brones.ItemsSource = Data.Brones;
        }

        private void Button_Click_Size(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
                WindowState = WindowState.Normal;
            else WindowState = WindowState.Maximized;
        }

        private void Button_Click_Close(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Button_Click_Hidden(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void Button_Click_Exit(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            Close();
        }

        private void name_TextChanged(object sender, TextChangedEventArgs e)
        {
            buttonRoom.IsEnabled = name.Text.Trim().Length > 0;
        }

        private void buttonRoom_Click(object sender, RoutedEventArgs e)
        {
            int id = 0;
            foreach (Room r in Data.Rooms)
            {
                if (id <= r.Id)
                    id = r.Id + 1;
                if (r.Name == name.Text)
                {
                    MessageBox.Show("Комната с таким название уже существует, придумайте оригинальное название еомнаты", "Название занято", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            Room room = new Room()
            {
                Id = id,
                Name = name.Text,
                Count = (int)count.Value,
                Atash = (int)atash.Value,
                Proector = proector.IsChecked.Value
            };
            Data.Rooms.Add(room);
            Data.Write();

            name.Clear();
            proector.IsChecked = false;
            atash.Value = 0;
            count.Value = 0;
        }

        private void cb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            buttonBrone.IsEnabled = cbRooms.SelectedItem != null && cbUsers.SelectedItem != null;
        }

        private void buttonBrone_Click(object sender, RoutedEventArgs e)
        {
            User user = cbUsers.SelectedItem as User;
            Room room = cbRooms.SelectedItem as Room;

            int id = 0;
            foreach (Brone b in Data.Brones)
            {
                if (id <= b.Id)
                    id = b.Id + 1;
                if (b.UserId == user.Id && b.RoomId == room.Id)
                {
                    MessageBox.Show($"Данная комната уже забронирована этим же пользователем, выберите другую комнату или пользователя", "Комната уже занята", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            Brone brone = new Brone()
            {
                Id = id,
                UserId = user.Id,
                RoomId = room.Id,
                Date = DateTime.Now,
                Status = true,
            };

            Data.Brones.Add(brone);
            Data.Write();
            cbUsers.SelectedItem = null;
            cbRooms.SelectedItem = null;
        }

        private void count_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
                countText.Text = "Вместимость:" + ((int)count.Value).ToString() + " человек";
        }

        private void atash_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
                atashText.Text = "Этаж " + ((int)atash.Value).ToString();
        }

        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch
            {

            }
        }

        bool oldButtonDeleteBroneIsEnabled = false;
        private void brones_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            buttonDeleteBrone.IsEnabled = brones.SelectedItem != null;
            var animation = new ThicknessAnimation();
            if (!oldButtonDeleteBroneIsEnabled)
            {
                animation.From = new Thickness(200, 50, 0, 0);
                animation.To = new Thickness(0, 50, 0, 0);
                animation.Duration = TimeSpan.FromSeconds(0.3);
                buttonDeleteBrone.BeginAnimation(Button.MarginProperty, animation);
                oldButtonDeleteBroneIsEnabled = true;
            }
        }

        private void buttonDeleteBrone_Click(object sender, RoutedEventArgs e)
        {
            Room room = (Room) rooms.SelectedItem;
  
            if (MessageBox.Show("Вы уверены, что хотите удалить комнату ?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                return;
            Data.Brones.Remove((Brone)brones.SelectedItem);

            var animation = new ThicknessAnimation();
            animation.From = new Thickness(0, 50, 0, 0);
            animation.To = new Thickness(200, 50, 0, 0);
            animation.Duration = TimeSpan.FromSeconds(0.3);
            buttonDeleteBrone.BeginAnimation(Button.MarginProperty, animation);
            oldButtonDeleteBroneIsEnabled = false;
        }

        bool oldButtonDeleteRoomIsEnabled = false;
        private void buttonDeleteRoom_Click(object sender, RoutedEventArgs e)
        {
            foreach (Brone brone in Data.Brones)
                if (brone.RoomId == ((Room)rooms.SelectedItem).Id)
                {
                    MessageBox.Show($"Комната уже забронирована пользователем: {brone.UserName}. Удалите бронирование с этим пользователем, потом вернитесь к удалению комнаты.", "Комната забронирована", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            if (MessageBox.Show("Вы уверены, что хотите удалить бронирование?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                return;
            Data.Rooms.Remove((Room)rooms.SelectedItem);

            var animation = new ThicknessAnimation();
            animation.To = new Thickness(100, 5, 0, 0);
            animation.Duration = TimeSpan.FromSeconds(0.3);
            buttonDeleteRoom.BeginAnimation(Button.MarginProperty, animation);
            oldButtonDeleteBroneIsEnabled = false;
        }

        private void rooms_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            buttonDeleteRoom.IsEnabled = rooms.SelectedItem != null;

            if (!oldButtonDeleteBroneIsEnabled)
            {
                var animation = new ThicknessAnimation();
                animation.To = new Thickness(0, 5, 0, 0);
                animation.Duration = TimeSpan.FromSeconds(0.3);
                buttonDeleteRoom.BeginAnimation(Button.MarginProperty, animation);
                oldButtonDeleteBroneIsEnabled = true;
            }
        }
    }
}
