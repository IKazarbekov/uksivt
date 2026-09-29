using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using Попытка3_в_кабинете.models;

namespace Попытка3_в_кабинете
{
    class Data
    {
        public static ObservableCollection<User> Users = new ObservableCollection<User>();
        public static ObservableCollection<Lesson> Lessons = new ObservableCollection<Lesson>();
        public static ObservableCollection<Brone> Brones = new ObservableCollection<Brone>();

        const string USER_FILE = "user.json";
        const string LESSON_FILE = "lesson.json";
        const string BRONES_FILE = "brones.json";

        public static void ReadOrCreateFile()
        {
            // Метод загрузки данных из файлов

            // Проверка существования файлов
            if (File.Exists(USER_FILE) &&
                File.Exists(LESSON_FILE)&&
                File.Exists(BRONES_FILE))
            {

            }
            else
            // Если их нет то все данные перезаписываются в начальные значения
            {
                Users = new ObservableCollection<User>()
                {
                    new User{ID = 0, Login="admin", Password="admin", Role = UserRole.Admin, Name="admin"},
                };
            }
        }

        public static void WriteFile()
        {
            // Метод загрузки данных в файл
            if ()
        }
    }
}
