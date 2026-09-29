using NewBilet.models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Text.Json;

namespace NewBilet
{
    internal class Data
    {
        static public ObservableCollection<User> Users { get; set; }
        static public ObservableCollection<Lesson> Lessons { get; set; }

        const string USER_FILE = "users.json";
        const string LESSON_FILE = "lesson.json";

        static public void DeleteFile()
        {
            File.Delete(USER_FILE);
            File.Delete(LESSON_FILE);
        }

        static public void ReadOrCreateFile()
        {
            if (File.Exists(USER_FILE) && File.Exists(LESSON_FILE))
            {
                string user_json = File.ReadAllText(USER_FILE);
                string lesson_json = File.ReadAllText(LESSON_FILE);
                Users = JsonSerializer.Deserialize<ObservableCollection<User>>(user_json);
                Lessons = JsonSerializer.Deserialize<ObservableCollection<Lesson>>(lesson_json);
            }
            else
            {
                DeleteFile();
                Users = new ObservableCollection<User>()
                {
                    new User(){ID = 0, Name = "admin", Password = "asd", Number = "777", Role = UserRole.ADMIN},
                    new User(){ID = 1, Name = "user", Password = "123", Number = "888", Role = UserRole.CLIENT}
                };
                Lessons = new ObservableCollection<Lesson>()
                {

                };
            }
        }

        public static void WriteFile()
        {
            string user_json = JsonSerializer.Serialize(Users, new JsonSerializerOptions()
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });
            string lesson_json = JsonSerializer.Serialize(Lessons, new JsonSerializerOptions()
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            });

            File.WriteAllText(USER_FILE, user_json);
            File.WriteAllText(LESSON_FILE, lesson_json);
        }
    }
}
