using System.Collections.ObjectModel;

namespace блет_0_попытка_10
{
    enum UserRole
    {
        ADMIN,
        CLIENT
    }

    class User
    {
        public int ID { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }
    }
    class Lesson
    {
        public int ID { get; set; }
        public string Type { get; set; }
        public string Tranner { get; set; }
        public DateTime Date { get; set; }
        public int Count { get; set; }
    }

    class Brone
    {
        public int ID { get; set; }
        public int LessonID { get; set; }
        public DateTime Date { get; set; }
        public bool Status { get; set; }
    }

    internal class Data
    {
        static public ObservableCollection<User> Users = new ObservableCollection<User>();
        static public ObservableCollection<Brone> Brones = new ObservableCollection<Brone>();
        static public ObservableCollection<Lesson> Lessons = new ObservableCollection<Lesson>();
    }
}
