using System.Collections.ObjectModel;

namespace ShopTask.data
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Role { get; set; }
        public string FullName { get; set; }
    }

    public static class UsersData
    {
        public static ObservableCollection<User> Users { get; set; } = new ObservableCollection<User>();

        static UsersData()
        {
            // Тестовый штат сотрудников
            Users.Add(new User { Id = 1, Login = "admin", Role = "Главный менеджер", FullName = "Иванов Иван Иванович" });
            Users.Add(new User { Id = 2, Login = "seller_1", Role = "Старший Кассир", FullName = "Петрова Анна Сергеевна" });
            Users.Add(new User { Id = 3, Login = "seller_2", Role = "Младший продавец", FullName = "Сидоров Алексей Владимирович" });
        }
    }
}